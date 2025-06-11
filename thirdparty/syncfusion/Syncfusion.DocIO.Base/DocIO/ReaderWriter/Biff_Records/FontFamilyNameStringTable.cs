// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.FontFamilyNameStringTable
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class FontFamilyNameStringTable : BaseWordRecord
{
  private ushort DEF_EXTENDED = ushort.MaxValue;
  private ushort m_extendedFlag;
  private ushort m_noStrings;
  private ushort m_extraDataLen;
  private FontFamilyNameRecord[] m_ffnRecords;

  internal override int Length
  {
    get
    {
      int length = 4;
      if (this.m_ffnRecords == null)
        return length;
      for (int index = 0; index < this.m_ffnRecords.Length; ++index)
      {
        if (this.m_ffnRecords[index] != null)
          length += this.m_ffnRecords[index].Length;
      }
      return length;
    }
  }

  internal int RecordsCount
  {
    get => this.m_ffnRecords != null ? this.m_ffnRecords.Length : 0;
    set => this.m_ffnRecords = new FontFamilyNameRecord[value];
  }

  internal FontFamilyNameRecord[] FontFamilyNameRecords => this.m_ffnRecords;

  internal FontFamilyNameStringTable()
  {
  }

  internal override void Parse(byte[] arrData, int iOffset, int iCount)
  {
    if (arrData.Length < 2)
      return;
    this.m_noStrings = this.m_extendedFlag = BaseWordRecord.ReadUInt16(arrData, iOffset);
    iOffset += 2;
    if ((int) this.m_extendedFlag == (int) this.DEF_EXTENDED)
      this.m_noStrings = BaseWordRecord.ReadUInt16(arrData, ref iOffset);
    this.m_extraDataLen = BaseWordRecord.ReadUInt16(arrData, ref iOffset);
    iCount = arrData.Length - iOffset;
    this.m_ffnRecords = new FontFamilyNameRecord[(int) this.m_noStrings];
    for (int index = 0; index < (int) this.m_noStrings; ++index)
    {
      int bytes = (this.m_ffnRecords[index] = new FontFamilyNameRecord()).ParseBytes(arrData, iOffset, iCount);
      iOffset += bytes;
      iCount -= bytes;
    }
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    int num = 0;
    this.m_noStrings = (ushort) this.m_ffnRecords.Length;
    BaseWordRecord.WriteUInt16(arrData, this.m_noStrings, ref iOffset);
    BaseWordRecord.WriteUInt16(arrData, this.m_extraDataLen, ref iOffset);
    for (int index = 0; index < this.m_ffnRecords.Length; ++index)
      iOffset = this.m_ffnRecords[index].Save(arrData, iOffset);
    return num;
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_ffnRecords == null)
      return;
    for (int index = 0; index < this.m_ffnRecords.Length; ++index)
    {
      if (this.m_ffnRecords[index] != null)
        this.m_ffnRecords[index].Close();
    }
    this.m_ffnRecords = (FontFamilyNameRecord[]) null;
  }
}
