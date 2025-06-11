// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartFrameRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartFrame)]
public class ChartFrameRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 4;
  [BiffRecordPos(0, 2)]
  private ushort m_usRectStyle;
  [BiffRecordPos(2, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(2, 0, TFieldType.Bit)]
  private bool m_bAutoSize = true;
  [BiffRecordPos(2, 1, TFieldType.Bit)]
  private bool m_bAutoPosition = true;

  public ushort Options => this.m_usOptions;

  public ExcelRectangleStyle Rectangle
  {
    get => (ExcelRectangleStyle) this.m_usRectStyle;
    set => this.m_usRectStyle = (ushort) value;
  }

  public bool AutoSize
  {
    get => this.m_bAutoSize;
    set => this.m_bAutoSize = value;
  }

  public bool AutoPosition
  {
    get => this.m_bAutoPosition;
    set => this.m_bAutoPosition = value;
  }

  public override int MinimumRecordSize => 4;

  public override int MaximumRecordSize => 4;

  public ChartFrameRecord()
  {
  }

  public ChartFrameRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartFrameRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usRectStyle = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bAutoSize = provider.ReadBit(iOffset, 0);
    this.m_bAutoPosition = provider.ReadBit(iOffset, 1);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usRectStyle);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bAutoSize, 0);
    provider.WriteBit(iOffset, this.m_bAutoPosition, 1);
  }

  public override int GetStoreSize(ExcelVersion version) => 4;
}
