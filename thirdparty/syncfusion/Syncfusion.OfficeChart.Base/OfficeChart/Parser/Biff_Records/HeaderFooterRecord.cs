// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.HeaderFooterRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.Header)]
[Biff(TBIFFRecord.Footer)]
[CLSCompliant(false)]
internal class HeaderFooterRecord : BiffRecordRaw
{
  private string m_strValue = string.Empty;

  public string Value
  {
    get => this.m_strValue;
    set => this.m_strValue = value;
  }

  public override int MinimumRecordSize => 0;

  public HeaderFooterRecord()
  {
  }

  public HeaderFooterRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public HeaderFooterRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    if (this.m_iLength <= 0)
      return;
    int iFullLength;
    this.m_strValue = provider.ReadString16Bit(iOffset, out iFullLength);
    if (iFullLength != this.m_iLength)
      throw new WrongBiffRecordDataException("Wrong string or data length.");
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    if (this.m_iLength <= 0)
      return;
    provider.WriteString16BitUpdateOffset(ref iOffset, this.m_strValue);
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    return this.m_strValue != null && this.m_strValue.Length != 0 ? 3 + Encoding.Unicode.GetByteCount(this.m_strValue) : 0;
  }
}
