// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.RStringRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.RString)]
[CLSCompliant(false)]
internal class RStringRecord : BiffRecordRawWithArray, ICellPositionFormat, IStringValue
{
  [BiffRecordPos(0, 2)]
  private int m_iRow;
  [BiffRecordPos(2, 2)]
  private int m_iColumn;
  [BiffRecordPos(4, 2)]
  private ushort m_usExtFormat;
  private string m_strValue;
  private ushort m_usFRunsNumber;
  private RStringRecord.TFormattingRun[] m_arrFormattingRuns;

  public int Row
  {
    get => this.m_iRow;
    set => this.m_iRow = value;
  }

  public int Column
  {
    get => this.m_iColumn;
    set => this.m_iColumn = value;
  }

  public ushort ExtendedFormatIndex
  {
    get => this.m_usExtFormat;
    set => this.m_usExtFormat = value;
  }

  public string Value
  {
    get => this.m_strValue;
    set => this.m_strValue = value;
  }

  public RStringRecord.TFormattingRun[] FormattingRun
  {
    get => this.m_arrFormattingRuns;
    set
    {
      this.m_arrFormattingRuns = value;
      this.m_usFRunsNumber = value != null ? (ushort) value.Length : (ushort) 0;
    }
  }

  public override int MinimumRecordSize => 8;

  public RStringRecord()
  {
  }

  public RStringRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public RStringRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure()
  {
    this.m_iRow = (int) this.GetUInt16(0);
    this.m_iColumn = (int) this.GetUInt16(2);
    this.m_usExtFormat = this.GetUInt16(4);
    int offset1 = 6;
    this.m_strValue = this.GetString16BitUpdateOffset(ref offset1);
    this.m_usFRunsNumber = this.GetUInt16(offset1);
    int offset2 = offset1 + 2;
    this.m_arrFormattingRuns = new RStringRecord.TFormattingRun[(int) this.m_usFRunsNumber];
    int index = 0;
    while (index < (int) this.m_usFRunsNumber)
    {
      this.m_arrFormattingRuns[index].FirstChar = this.GetUInt16(offset2);
      this.m_arrFormattingRuns[index].FormatIndex = this.GetUInt16(offset2 + 2);
      ++index;
      offset2 += 4;
    }
  }

  public override void InfillInternalData(OfficeVersion version)
  {
    this.AutoGrowData = true;
    this.SetUInt16(0, (ushort) this.m_iRow);
    this.SetUInt16(2, (ushort) this.m_iColumn);
    this.SetUInt16(4, this.m_usExtFormat);
    this.m_iLength = 6;
    this.SetString16BitUpdateOffset(ref this.m_iLength, this.m_strValue);
    this.SetUInt16(this.m_iLength, this.m_usFRunsNumber);
    this.m_iLength += 2;
    int index = 0;
    while (index < (int) this.m_usFRunsNumber)
    {
      this.SetUInt16(this.m_iLength, this.m_arrFormattingRuns[index].FirstChar);
      this.SetUInt16(this.m_iLength + 2, this.m_arrFormattingRuns[index].FormatIndex);
      ++index;
      this.m_iLength += 4;
    }
  }

  string IStringValue.StringValue => this.Value;

  [CLSCompliant(false)]
  public struct TFormattingRun
  {
    public ushort FirstChar;
    public ushort FormatIndex;
  }
}
