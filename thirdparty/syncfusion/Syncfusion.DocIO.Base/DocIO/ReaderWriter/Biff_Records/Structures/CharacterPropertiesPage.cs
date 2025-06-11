// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.CharacterPropertiesPage
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class CharacterPropertiesPage : BaseWordRecord
{
  private const int DEF_FC_SIZE = 4;
  private uint[] m_arrFC;
  private CharacterPropertyException[] m_arrCHPX;

  internal uint[] FileCharPos => this.m_arrFC;

  internal CharacterPropertyException[] CharacterProperties => this.m_arrCHPX;

  internal int RunsCount
  {
    get => this.m_arrCHPX.Length;
    set
    {
      if (value < 0)
        throw new ArgumentOutOfRangeException(nameof (RunsCount));
      if (this.m_arrCHPX != null && value == this.m_arrCHPX.Length)
        return;
      this.m_arrCHPX = new CharacterPropertyException[value];
      for (int index = 0; index < value; ++index)
        this.m_arrCHPX[index] = new CharacterPropertyException();
      this.m_arrFC = new uint[value + 1];
    }
  }

  internal override int Length => 512 /*0x0200*/;

  internal CharacterPropertiesPage()
  {
  }

  internal CharacterPropertiesPage(FKPStructure structure) => this.Parse(structure);

  internal override void Close()
  {
    base.Close();
    if (this.m_arrFC != null)
      this.m_arrFC = (uint[]) null;
    if (this.m_arrCHPX == null)
      return;
    this.m_arrCHPX = (CharacterPropertyException[]) null;
  }

  private void Parse(FKPStructure structure)
  {
    byte[] pageData = structure.PageData;
    this.m_arrFC = new uint[(int) structure.Count + 1];
    byte[] destinationArray = new byte[(int) structure.Count];
    int num = ((int) structure.Count + 1) * 4;
    Buffer.BlockCopy((Array) pageData, 0, (Array) this.m_arrFC, 0, num);
    Array.Copy((Array) pageData, num, (Array) destinationArray, 0, (int) structure.Count);
    this.m_arrCHPX = new CharacterPropertyException[(int) structure.Count];
    for (int index = 0; index < (int) structure.Count; ++index)
    {
      int iOffset = (int) destinationArray[index] * 2;
      this.m_arrCHPX[index] = new CharacterPropertyException(pageData, iOffset);
    }
  }

  private FKPStructure Save()
  {
    FKPStructure fkpStructure = new FKPStructure();
    int runsCount = this.RunsCount;
    int num1 = 0;
    fkpStructure.Count = (byte) runsCount;
    int num2 = (runsCount + 1) * 4;
    Buffer.BlockCopy((Array) this.m_arrFC, 0, (Array) fkpStructure.PageData, 0, num2);
    byte[] numArray = new byte[runsCount];
    int iOffset = 511 /*0x01FF*/;
    for (int index = runsCount - 1; index >= 0; --index)
    {
      if (this.m_arrCHPX[index] != null)
      {
        int length = this.m_arrCHPX[index].Length;
        int num3 = iOffset - length;
        if (num3 % 2 != 0)
          --num3;
        iOffset = num3;
        numArray[index] = (byte) (iOffset / 2);
        this.m_arrCHPX[index].Save(fkpStructure.PageData, iOffset);
        num1 += length + 4 + 1;
      }
    }
    if (num1 > 512 /*0x0200*/)
      throw new Exception("FKP Chpx buffer overflow: " + num1.ToString());
    if (iOffset < num2 + numArray.Length)
      throw new Exception($"FKP Chpx buffer overflow, ( chpx start at: {iOffset.ToString()}FC end: {(object) num2}, end of rgb: {(num2 + numArray.Length).ToString()}");
    numArray.CopyTo((Array) fkpStructure.PageData, num2);
    return fkpStructure;
  }

  internal override int Save(byte[] arrData, int iOffset) => this.Save().Save(arrData, iOffset);

  internal int SaveToStream(BinaryWriter writer, Stream stream)
  {
    long position1 = stream.Position;
    Dictionary<int, byte> dictionary = new Dictionary<int, byte>();
    int runsCount = this.RunsCount;
    int num1 = 0;
    int count = (runsCount + 1) * 4;
    byte[] numArray = new byte[count];
    Buffer.BlockCopy((Array) this.m_arrFC, 0, (Array) numArray, 0, count);
    stream.Write(numArray, 0, numArray.Length);
    byte[] buffer = new byte[runsCount];
    int num2 = 511 /*0x01FF*/;
    for (int index = runsCount - 1; index >= 0; --index)
    {
      int ReturnIndex = -1;
      if (index < runsCount - 1 && this.IsChpxRepeats(index, out ReturnIndex))
        buffer[index] = dictionary[ReturnIndex];
      else if (this.m_arrCHPX[index] != null)
      {
        int length = this.m_arrCHPX[index].Length;
        int num3 = num2 - length;
        if (num3 % 2 != 0)
          --num3;
        num2 = num3;
        buffer[index] = (byte) (num2 / 2);
        dictionary.Add(index, buffer[index]);
        stream.Position = position1 + (long) num2;
        this.m_arrCHPX[index].Save(writer, stream, length);
        num1 += length + 4 + 1;
      }
    }
    if (num1 > 512 /*0x0200*/)
      throw new Exception("FKP Chpx buffer overflow: " + num1.ToString());
    long position2 = stream.Position;
    stream.Position = position1 + (long) count;
    stream.Write(buffer, 0, buffer.Length);
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (position2 > position1 + 511L /*0x01FF*/)
      throw new Exception("chpx overflow");
    stream.Position = position1 + 511L /*0x01FF*/;
    stream.WriteByte((byte) runsCount);
    return (int) stream.Position;
  }

  internal bool IsChpxRepeats(int CurrentIndex, out int ReturnIndex)
  {
    CharacterPropertyException propertyException = this.m_arrCHPX[CurrentIndex];
    bool flag = false;
    ReturnIndex = -1;
    for (int index = this.m_arrCHPX.Length - 1; index > CurrentIndex; --index)
    {
      CharacterPropertyException chpx = this.m_arrCHPX[index];
      ReturnIndex = index;
      if (propertyException.Length == chpx.Length && propertyException.ModifiersCount == chpx.ModifiersCount && (flag = propertyException.Equals(chpx)))
        break;
    }
    return flag;
  }
}
