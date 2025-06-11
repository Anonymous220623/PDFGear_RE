// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.SectionPropertyException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class SectionPropertyException : BaseWordRecord
{
  private SinglePropertyModifierArray m_arrSprms = new SinglePropertyModifierArray();

  internal SinglePropertyModifierArray Properties
  {
    get => this.m_arrSprms;
    set => this.m_arrSprms = value;
  }

  internal int Count => this.m_arrSprms.Count;

  internal override int Length => 2 + this.m_arrSprms.Length;

  internal ushort HeaderHeight
  {
    get => this.m_arrSprms.GetUShort(45079, (ushort) 0);
    set
    {
      if (value == (ushort) 0)
        return;
      this.m_arrSprms.SetUShortValue(45079, value);
    }
  }

  internal ushort FooterHeight
  {
    get => this.m_arrSprms.GetUShort(45080, (ushort) 0);
    set
    {
      if (value == (ushort) 0)
        return;
      this.m_arrSprms.SetUShortValue(45080, value);
    }
  }

  internal bool IsTitlePage
  {
    get => this.m_arrSprms.GetBoolean(12298, false);
    set
    {
      if (!value)
        return;
      this.m_arrSprms.SetBoolValue(12298, value);
    }
  }

  internal byte BreakCode
  {
    get => this.m_arrSprms.GetByte(12297, (byte) 2);
    set
    {
      if (value == (byte) 2)
        return;
      this.m_arrSprms.SetByteValue(12297, value);
    }
  }

  internal int ColumnsCount
  {
    get => (int) this.m_arrSprms.GetUShort(20491, (ushort) 0) + 1;
    set
    {
      if (value < 1)
        throw new ArgumentOutOfRangeException();
      this.m_arrSprms.SetUShortValue(20491, (ushort) (value - 1));
    }
  }

  internal SectionPropertyException()
  {
  }

  internal SectionPropertyException(bool isDefaultSEP)
  {
    if (!isDefaultSEP)
      return;
    this.m_arrSprms.SetUShortValue(45071, (ushort) 720);
    this.m_arrSprms.SetUShortValue(45072, (ushort) 720);
    this.m_arrSprms.SetBoolValue(12306, true);
    this.m_arrSprms.SetBoolValue(12293, true);
    this.m_arrSprms.SetUShortValue(45087, (ushort) 12240);
    this.m_arrSprms.SetUShortValue(45088, (ushort) 15840);
    this.m_arrSprms.SetUShortValue(45079, (ushort) 720);
    this.m_arrSprms.SetUShortValue(45080, (ushort) 720);
    this.m_arrSprms.SetBoolValue(12317, true);
    this.m_arrSprms.SetUShortValue(36876, (ushort) 720);
    this.m_arrSprms.SetUShortValue(36899, (ushort) 1440);
    this.m_arrSprms.SetUShortValue(45089, (ushort) 1440);
    this.m_arrSprms.SetUShortValue(36900, (ushort) 1440);
    this.m_arrSprms.SetUShortValue(45090, (ushort) 1440);
    this.m_arrSprms.SetUShortValue(20508, (ushort) 1);
  }

  internal SectionPropertyException(Stream stream) => this.Parse(stream);

  internal override void Close()
  {
    base.Close();
    if (this.m_arrSprms == null)
      return;
    this.m_arrSprms = (SinglePropertyModifierArray) null;
  }

  private void Parse(Stream stream)
  {
    byte[] buffer = new byte[2];
    if (stream.Read(buffer, 0, 2) != 2)
      throw new Exception("Was unable to read required bytes from the stream");
    int num = (int) ((long) BitConverter.ToUInt16(buffer, 0) + stream.Position);
    while (stream.Position < (long) num)
      this.m_arrSprms.Add(new SinglePropertyModifierRecord(stream));
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    ushort length = (ushort) this.m_arrSprms.Length;
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset + (int) length > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    int num = iOffset;
    BitConverter.GetBytes(length).CopyTo((Array) arrData, iOffset);
    iOffset += 2;
    iOffset += this.m_arrSprms.Save(arrData, iOffset);
    return iOffset - num;
  }
}
