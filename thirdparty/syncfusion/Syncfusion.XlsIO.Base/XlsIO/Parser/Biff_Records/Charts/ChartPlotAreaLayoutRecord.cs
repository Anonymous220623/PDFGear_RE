// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartPlotAreaLayoutRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.PlotAreaLayout)]
[CLSCompliant(false)]
public class ChartPlotAreaLayoutRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 68;
  private byte[] DEF_HEADER = new byte[12]
  {
    (byte) 167,
    (byte) 8,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0
  };
  private byte[] m_frtHeader;
  private int m_dwCheckSum;
  private int m_info;
  private bool m_layoutTargetInner;
  private byte[] m_reserved1;
  private int m_xTL;
  private int m_yTL;
  private int m_xBR;
  private int m_yBR;
  private LayoutModes m_wXMode;
  private LayoutModes m_wYMode;
  private LayoutModes m_wWidthMode;
  private LayoutModes m_wHeightMode;
  private double m_x;
  private double m_y;
  private double m_dx;
  private double m_dy;
  private byte[] m_reserved2 = new byte[2];

  private byte[] FrtHeader => this.m_frtHeader;

  private int dwCheckSum
  {
    get => this.m_dwCheckSum;
    set => this.m_dwCheckSum = value;
  }

  internal bool LayoutTargetInner
  {
    get => this.m_layoutTargetInner;
    set => this.m_layoutTargetInner = value;
  }

  public int xTL
  {
    get => this.m_xTL;
    set => this.m_xTL = value;
  }

  public int yTL
  {
    get => this.m_yTL;
    set => this.m_yTL = value;
  }

  public int xBR
  {
    get => this.m_xBR;
    set => this.m_xBR = value;
  }

  public int yBR
  {
    get => this.m_yBR;
    set => this.m_yBR = value;
  }

  public LayoutModes WXMode
  {
    get => this.m_wXMode;
    set => this.m_wXMode = value;
  }

  public LayoutModes WYMode
  {
    get => this.m_wYMode;
    set => this.m_wYMode = value;
  }

  public LayoutModes WWidthMode
  {
    get => this.m_wWidthMode;
    set => this.m_wWidthMode = value;
  }

  public LayoutModes WHeightMode
  {
    get => this.m_wHeightMode;
    set => this.m_wHeightMode = value;
  }

  public double X
  {
    get => this.m_x;
    set => this.m_x = value;
  }

  public double Y
  {
    get => this.m_y;
    set => this.m_y = value;
  }

  public double Dx
  {
    get => this.m_dx;
    set => this.m_dx = value;
  }

  public double Dy
  {
    get => this.m_dy;
    set => this.m_dy = value;
  }

  public ChartPlotAreaLayoutRecord() => this.m_frtHeader = this.DEF_HEADER;

  public ChartPlotAreaLayoutRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
    this.m_frtHeader = this.DEF_HEADER;
  }

  public ChartPlotAreaLayoutRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    provider.ReadArray(iOffset, this.m_frtHeader);
    this.m_dwCheckSum = provider.ReadInt32(iOffset + 12);
    this.m_info = provider.ReadInt32(iOffset + 16 /*0x10*/);
    this.m_layoutTargetInner = provider.ReadBit(iOffset + 16 /*0x10*/, 0);
    this.m_xTL = (int) provider.ReadInt16(iOffset + 18);
    this.m_yTL = (int) provider.ReadInt16(iOffset + 20);
    this.m_xBR = (int) provider.ReadInt16(iOffset + 22);
    this.m_yBR = (int) provider.ReadInt16(iOffset + 24);
    this.m_wXMode = (LayoutModes) Enum.Parse(typeof (LayoutModes), provider.ReadInt16(iOffset + 26).ToString(), true);
    this.m_wYMode = (LayoutModes) Enum.Parse(typeof (LayoutModes), provider.ReadInt16(iOffset + 28).ToString(), true);
    this.m_wWidthMode = (LayoutModes) Enum.Parse(typeof (LayoutModes), provider.ReadInt16(iOffset + 30).ToString(), true);
    this.m_wHeightMode = (LayoutModes) Enum.Parse(typeof (LayoutModes), provider.ReadInt16(iOffset + 32 /*0x20*/).ToString(), true);
    this.m_x = provider.ReadDouble(iOffset + 34);
    this.m_y = provider.ReadDouble(iOffset + 42);
    this.m_dx = provider.ReadDouble(iOffset + 50);
    this.m_dy = provider.ReadDouble(iOffset + 58);
    provider.ReadArray(iOffset + 66, this.m_reserved2);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteBytes(iOffset, this.m_frtHeader);
    provider.WriteInt32(iOffset + 12, this.m_dwCheckSum);
    provider.WriteInt32(iOffset + 16 /*0x10*/, this.m_info);
    provider.WriteInt32(iOffset + 18, this.m_xTL);
    provider.WriteInt32(iOffset + 20, this.m_yTL);
    provider.WriteInt32(iOffset + 22, this.m_xBR);
    provider.WriteInt32(iOffset + 24, this.m_yBR);
    provider.WriteInt32(iOffset + 26, (int) Enum.Parse(typeof (LayoutModes), this.m_wXMode.ToString(), true));
    provider.WriteInt32(iOffset + 28, (int) Enum.Parse(typeof (LayoutModes), this.m_wYMode.ToString(), true));
    provider.WriteInt32(iOffset + 30, (int) Enum.Parse(typeof (LayoutModes), this.m_wWidthMode.ToString(), true));
    provider.WriteInt32(iOffset + 32 /*0x20*/, (int) Enum.Parse(typeof (LayoutModes), this.m_wHeightMode.ToString(), true));
    provider.WriteDouble(iOffset + 34, this.m_x);
    provider.WriteDouble(iOffset + 42, this.m_y);
    provider.WriteDouble(iOffset + 50, this.m_dx);
    provider.WriteDouble(iOffset + 58, this.m_dy);
    provider.WriteBytes(iOffset + 66, this.m_reserved2);
  }

  public override int GetStoreSize(ExcelVersion version) => 68;

  private int CheckSum() => 0;
}
