// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.FormatRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.Format)]
[CLSCompliant(false)]
internal class FormatRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usIndex;
  [BiffRecordPos(2, 2)]
  private ushort m_usFormatStringLen;
  private string m_strFormatString = string.Empty;

  public int Index
  {
    get => (int) this.m_usIndex;
    set => this.m_usIndex = (ushort) value;
  }

  public string FormatString
  {
    get => this.m_strFormatString;
    set
    {
      if (!(this.m_strFormatString != value))
        return;
      this.m_strFormatString = value;
      this.m_usFormatStringLen = (ushort) this.m_strFormatString.Length;
    }
  }

  public override int MinimumRecordSize => 5;

  public FormatRecord()
  {
  }

  public FormatRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public FormatRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usIndex = provider.ReadUInt16(iOffset);
    this.m_usFormatStringLen = provider.ReadUInt16(iOffset + 2);
    int iBytesInString;
    this.FormatString = provider.ReadString(iOffset + 4, (int) this.m_usFormatStringLen, out iBytesInString, false);
    if (this.m_iLength != 5 + iBytesInString)
      throw new WrongBiffRecordDataException("m_iLength and String length do not fit each other.");
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usIndex);
    provider.WriteUInt16(iOffset + 2, this.m_usFormatStringLen);
    provider.WriteByte(iOffset + 4, (byte) 1);
    provider.WriteBytes(iOffset + 5, Encoding.Unicode.GetBytes(this.m_strFormatString), 0, (int) this.m_usFormatStringLen * 2);
  }

  public override int GetStoreSize(OfficeVersion version) => 5 + (int) this.m_usFormatStringLen * 2;
}
