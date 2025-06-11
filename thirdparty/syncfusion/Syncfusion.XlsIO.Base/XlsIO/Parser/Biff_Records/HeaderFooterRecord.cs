// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.HeaderFooterRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Exceptions;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Header)]
[Biff(TBIFFRecord.Footer)]
public class HeaderFooterRecord : BiffRecordRaw
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
    ExcelVersion version)
  {
    if (this.m_iLength <= 0)
      return;
    int iFullLength;
    this.m_strValue = provider.ReadString16Bit(iOffset, out iFullLength);
    if (iFullLength != this.m_iLength)
      throw new WrongBiffRecordDataException("Wrong string or data length.");
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    if (this.m_iLength <= 0)
      return;
    provider.WriteString16BitUpdateOffset(ref iOffset, this.m_strValue);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    return this.m_strValue != null && this.m_strValue.Length != 0 ? 3 + Encoding.Unicode.GetByteCount(this.m_strValue) : 0;
  }
}
