// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartValueRangeRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartValueRange)]
[CLSCompliant(false)]
public class ChartValueRangeRecord : BiffRecordRaw, IMaxCross
{
  public const int DEF_RECORD_SIZE = 42;
  [BiffRecordPos(0, 8, TFieldType.Float)]
  private double m_dNumMin;
  [BiffRecordPos(8, 8, TFieldType.Float)]
  private double m_dNumMax;
  [BiffRecordPos(16 /*0x10*/, 8, TFieldType.Float)]
  private double m_dNumMajor;
  [BiffRecordPos(24, 8, TFieldType.Float)]
  private double m_dNumMinor;
  [BiffRecordPos(32 /*0x20*/, 8, TFieldType.Float)]
  private double m_dNumCross;
  [BiffRecordPos(40, 2)]
  private ushort m_usFormatFlags;
  [BiffRecordPos(40, 0, TFieldType.Bit)]
  private bool m_bAutoMin = true;
  [BiffRecordPos(40, 1, TFieldType.Bit)]
  private bool m_bAutoMax = true;
  [BiffRecordPos(40, 2, TFieldType.Bit)]
  private bool m_bAutoMajor = true;
  [BiffRecordPos(40, 3, TFieldType.Bit)]
  private bool m_bAutoMinor = true;
  [BiffRecordPos(40, 4, TFieldType.Bit)]
  private bool m_bAutoCross = true;
  [BiffRecordPos(40, 5, TFieldType.Bit)]
  private bool m_bLogScale;
  [BiffRecordPos(40, 6, TFieldType.Bit)]
  private bool m_bReverse;
  [BiffRecordPos(40, 7, TFieldType.Bit)]
  private bool m_bMaxCross;

  public double NumMin
  {
    get => this.m_dNumMin;
    set => this.m_dNumMin = value;
  }

  public double NumMax
  {
    get => this.m_dNumMax;
    set => this.m_dNumMax = value;
  }

  public double NumMajor
  {
    get => this.m_dNumMajor;
    set => this.m_dNumMajor = value;
  }

  public double NumMinor
  {
    get => this.m_dNumMinor;
    set => this.m_dNumMinor = value;
  }

  public double NumCross
  {
    get => this.m_dNumCross;
    set => this.m_dNumCross = value;
  }

  public ushort FormatFlags => this.m_usFormatFlags;

  public bool IsAutoMin
  {
    get => this.m_bAutoMin;
    set => this.m_bAutoMin = value;
  }

  public bool IsAutoMax
  {
    get => this.m_bAutoMax;
    set => this.m_bAutoMax = value;
  }

  public bool IsAutoMajor
  {
    get => this.m_bAutoMajor;
    set => this.m_bAutoMajor = value;
  }

  public bool IsAutoMinor
  {
    get => this.m_bAutoMinor;
    set => this.m_bAutoMinor = value;
  }

  public bool IsAutoCross
  {
    get => this.m_bAutoCross;
    set => this.m_bAutoCross = value;
  }

  public bool IsLogScale
  {
    get => this.m_bLogScale;
    set => this.m_bLogScale = value;
  }

  public bool IsReverse
  {
    get => this.m_bReverse;
    set => this.m_bReverse = value;
  }

  public bool IsMaxCross
  {
    get => this.m_bMaxCross;
    set => this.m_bMaxCross = value;
  }

  public override int MinimumRecordSize => 42;

  public override int MaximumRecordSize => 42;

  public ChartValueRangeRecord()
  {
  }

  public ChartValueRangeRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartValueRangeRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_dNumMin = provider.ReadDouble(iOffset);
    iOffset += 8;
    this.m_dNumMax = provider.ReadDouble(iOffset);
    iOffset += 8;
    this.m_dNumMajor = provider.ReadDouble(iOffset);
    iOffset += 8;
    this.m_dNumMinor = provider.ReadDouble(iOffset);
    iOffset += 8;
    this.m_dNumCross = provider.ReadDouble(iOffset);
    iOffset += 8;
    this.m_usFormatFlags = provider.ReadUInt16(iOffset);
    this.m_bAutoMin = provider.ReadBit(iOffset, 0);
    this.m_bAutoMax = provider.ReadBit(iOffset, 1);
    this.m_bAutoMajor = provider.ReadBit(iOffset, 2);
    this.m_bAutoMinor = provider.ReadBit(iOffset, 3);
    this.m_bAutoCross = provider.ReadBit(iOffset, 4);
    this.m_bLogScale = provider.ReadBit(iOffset, 5);
    this.m_bReverse = provider.ReadBit(iOffset, 6);
    this.m_bMaxCross = provider.ReadBit(iOffset, 7);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteDouble(iOffset, this.m_dNumMin);
    iOffset += 8;
    provider.WriteDouble(iOffset, this.m_dNumMax);
    iOffset += 8;
    provider.WriteDouble(iOffset, this.m_dNumMajor);
    iOffset += 8;
    provider.WriteDouble(iOffset, this.m_dNumMinor);
    iOffset += 8;
    provider.WriteDouble(iOffset, this.m_dNumCross);
    iOffset += 8;
    provider.WriteUInt16(iOffset, this.m_usFormatFlags);
    provider.WriteBit(iOffset, this.m_bAutoMin, 0);
    provider.WriteBit(iOffset, this.m_bAutoMax, 1);
    provider.WriteBit(iOffset, this.m_bAutoMajor, 2);
    provider.WriteBit(iOffset, this.m_bAutoMinor, 3);
    provider.WriteBit(iOffset, this.m_bAutoCross, 4);
    provider.WriteBit(iOffset, this.m_bLogScale, 5);
    provider.WriteBit(iOffset, this.m_bReverse, 6);
    provider.WriteBit(iOffset, this.m_bMaxCross, 7);
  }

  public override int GetStoreSize(ExcelVersion version) => 42;
}
