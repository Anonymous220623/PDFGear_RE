// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartSeriesTextRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartSeriesText)]
[CLSCompliant(false)]
public class ChartSeriesTextRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 3;
  [BiffRecordPos(0, 2)]
  private ushort m_usTextId;
  [BiffRecordPos(2, 1, TFieldType.String)]
  private string m_strText = string.Empty;

  public ushort TextId
  {
    get => this.m_usTextId;
    set => this.m_usTextId = value;
  }

  public string Text
  {
    get => this.m_strText;
    set => this.m_strText = value;
  }

  public override int MinimumRecordSize => 3;

  public ChartSeriesTextRecord()
  {
  }

  public ChartSeriesTextRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartSeriesTextRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usTextId = provider.ReadUInt16(iOffset);
    this.m_strText = provider.ReadString8Bit(iOffset + 2, out int _);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usTextId);
    int num = iOffset;
    iOffset += 2;
    provider.WriteString8BitUpdateOffset(ref iOffset, this.m_strText);
    this.m_iLength = iOffset - num;
  }

  public override int GetStoreSize(ExcelVersion version) => 4 + this.m_strText.Length * 2;
}
