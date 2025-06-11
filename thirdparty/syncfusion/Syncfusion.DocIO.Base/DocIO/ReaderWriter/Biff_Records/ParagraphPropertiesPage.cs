// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ParagraphPropertiesPage
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class ParagraphPropertiesPage : BaseWordRecord
{
  private const int DEF_FC_SIZE = 4;
  private uint[] m_arrFC;
  private BXStructure[] m_arrHeight;
  private ParagraphExceptionInDiskPage[] m_arrPAPX;

  internal uint[] FileCharPos => this.m_arrFC;

  internal BXStructure[] Heights => this.m_arrHeight;

  internal ParagraphExceptionInDiskPage[] ParagraphProperties => this.m_arrPAPX;

  internal int RunsCount
  {
    get => this.m_arrPAPX == null ? 0 : this.m_arrPAPX.Length;
    set
    {
      if (value < 0)
        throw new ArgumentOutOfRangeException(nameof (RunsCount));
      if (this.RunsCount == value)
        return;
      this.m_arrPAPX = new ParagraphExceptionInDiskPage[value];
      this.m_arrFC = new uint[value + 1];
      this.m_arrHeight = new BXStructure[value];
    }
  }

  internal override int Length => 512 /*0x0200*/;

  internal ParagraphPropertiesPage()
  {
  }

  internal ParagraphPropertiesPage(FKPStructure structure) => this.Parse(structure);

  internal override void Close()
  {
    base.Close();
    if (this.m_arrHeight != null)
      this.m_arrHeight = (BXStructure[]) null;
    if (this.m_arrPAPX == null)
      return;
    this.m_arrPAPX = (ParagraphExceptionInDiskPage[]) null;
  }

  private void Parse(FKPStructure structure)
  {
    byte[] pageData = structure.PageData;
    this.m_arrFC = new uint[(int) structure.Count + 1];
    int count = ((int) structure.Count + 1) * 4;
    Buffer.BlockCopy((Array) pageData, 0, (Array) this.m_arrFC, 0, count);
    this.m_arrPAPX = new ParagraphExceptionInDiskPage[(int) structure.Count];
    this.m_arrHeight = new BXStructure[(int) structure.Count];
    int iOffset1 = count;
    for (int index = 0; index < (int) structure.Count; ++index)
    {
      this.m_arrHeight[index] = new BXStructure();
      this.m_arrHeight[index].Parse(pageData, iOffset1);
      iOffset1 += 13;
    }
    for (int index = 0; index < (int) structure.Count; ++index)
    {
      int iOffset2 = (int) this.m_arrHeight[index].Offset * 2;
      this.m_arrPAPX[index] = new ParagraphExceptionInDiskPage();
      this.m_arrPAPX[index].Parse(pageData, iOffset2);
    }
  }

  internal FKPStructure Save()
  {
    FKPStructure fkpStructure = new FKPStructure();
    int runsCount = this.RunsCount;
    int count = this.m_arrFC.Length * 4;
    fkpStructure.Count = (byte) runsCount;
    Buffer.BlockCopy((Array) this.m_arrFC, 0, (Array) fkpStructure.PageData, 0, count);
    int iOffset = count;
    byte maxValue = byte.MaxValue;
    for (int index = 0; index < runsCount; ++index)
    {
      if (this.m_arrPAPX[index] != null)
      {
        maxValue -= (byte) (this.m_arrPAPX[index].Length / 2);
        if (this.m_arrHeight[index] == null)
          this.m_arrHeight[index] = new BXStructure();
        BXStructure bxStructure = this.m_arrHeight[index];
        bxStructure.Offset = maxValue;
        bxStructure.Save(fkpStructure.PageData, iOffset);
        iOffset += 13;
        this.m_arrPAPX[index].Save(fkpStructure.PageData, (int) bxStructure.Offset * 2);
      }
    }
    return fkpStructure;
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset + 512 /*0x0200*/ > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    this.Save().Save(arrData, iOffset);
    return 512 /*0x0200*/;
  }

  internal void SaveToStream(BinaryWriter writer, Stream stream)
  {
    long position = stream.Position;
    int runsCount = this.RunsCount;
    int num1 = this.m_arrFC.Length * 4;
    int index1 = 0;
    for (int length = this.m_arrFC.Length; index1 < length; ++index1)
      writer.Write(this.m_arrFC[index1]);
    int num2 = num1;
    byte maxValue = byte.MaxValue;
    for (int index2 = 0; index2 < runsCount; ++index2)
    {
      if (this.m_arrPAPX[index2] != null)
      {
        maxValue -= (byte) (this.m_arrPAPX[index2].Length / 2);
        if (this.m_arrHeight[index2] == null)
          this.m_arrHeight[index2] = new BXStructure();
        BXStructure bxStructure = this.m_arrHeight[index2];
        bxStructure.Offset = maxValue;
        stream.Position = position + (long) num2;
        bxStructure.Save(writer);
        num2 += 13;
        stream.Position = position + (long) ((int) bxStructure.Offset * 2);
        this.m_arrPAPX[index2].Save(writer, stream);
      }
    }
    stream.Position = position + 511L /*0x01FF*/;
    stream.WriteByte((byte) runsCount);
  }
}
