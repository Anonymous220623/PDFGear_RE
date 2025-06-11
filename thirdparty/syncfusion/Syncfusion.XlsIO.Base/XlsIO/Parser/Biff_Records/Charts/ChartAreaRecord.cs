// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAreaRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartArea)]
[CLSCompliant(false)]
public class ChartAreaRecord : BiffRecordRaw, IChartType
{
  private const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bStacked;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_bCategoryPercentage;
  [BiffRecordPos(0, 2, TFieldType.Bit)]
  private bool m_bShadowArea;

  public ushort Options => this.m_usOptions;

  public bool IsStacked
  {
    get => this.m_bStacked;
    set => this.m_bStacked = value;
  }

  public bool IsCategoryBrokenDown
  {
    get => this.m_bCategoryPercentage;
    set => this.m_bCategoryPercentage = value;
  }

  public bool IsAreaShadowed
  {
    get => this.m_bShadowArea;
    set => this.m_bShadowArea = value;
  }

  public ChartAreaRecord()
  {
  }

  public ChartAreaRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartAreaRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bStacked = provider.ReadBit(iOffset, 0);
    this.m_bCategoryPercentage = provider.ReadBit(iOffset, 1);
    this.m_bShadowArea = provider.ReadBit(iOffset, 2);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_usOptions &= (ushort) 7;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bStacked, 0);
    provider.WriteBit(iOffset, this.m_bCategoryPercentage, 1);
    provider.WriteBit(iOffset, this.m_bShadowArea, 2);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 2;

  bool IChartType.ShowAsPercents
  {
    get => this.IsCategoryBrokenDown;
    set => this.IsCategoryBrokenDown = value;
  }

  bool IChartType.StackValues
  {
    get => this.IsStacked;
    set => this.IsStacked = value;
  }
}
