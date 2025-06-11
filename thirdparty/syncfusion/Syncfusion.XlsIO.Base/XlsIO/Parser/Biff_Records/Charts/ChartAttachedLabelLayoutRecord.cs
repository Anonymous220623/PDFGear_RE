// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAttachedLabelLayoutRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartAttachedLabelLayout)]
public class ChartAttachedLabelLayoutRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 60;
  private byte[] DEF_HEADER = new byte[12]
  {
    (byte) 157,
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
  private bool m_unUsed;
  private byte m_autoLayoutType;
  private byte[] m_reserved1;
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

  internal byte AutoLayoutType
  {
    get => this.m_autoLayoutType;
    set => this.m_autoLayoutType = value;
  }

  public ChartAttachedLabelLayoutRecord() => this.m_frtHeader = this.DEF_HEADER;

  public ChartAttachedLabelLayoutRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
    this.m_frtHeader = this.DEF_HEADER;
  }

  public ChartAttachedLabelLayoutRecord(int iReserve)
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
    this.m_autoLayoutType = provider.ReadByte(iOffset + 16 /*0x10*/);
    this.m_wXMode = (LayoutModes) Enum.Parse(typeof (LayoutModes), provider.ReadInt16(iOffset + 18).ToString(), true);
    this.m_wYMode = (LayoutModes) Enum.Parse(typeof (LayoutModes), provider.ReadInt16(iOffset + 20).ToString(), true);
    this.m_wWidthMode = (LayoutModes) Enum.Parse(typeof (LayoutModes), provider.ReadInt16(iOffset + 22).ToString(), true);
    this.m_wHeightMode = (LayoutModes) Enum.Parse(typeof (LayoutModes), provider.ReadInt16(iOffset + 24).ToString(), true);
    this.m_x = provider.ReadDouble(iOffset + 26);
    this.m_y = provider.ReadDouble(iOffset + 34);
    this.m_dx = provider.ReadDouble(iOffset + 42);
    this.m_dy = provider.ReadDouble(iOffset + 50);
    provider.ReadArray(iOffset + 58, this.m_reserved2);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteBytes(iOffset, this.m_frtHeader);
    provider.WriteInt32(iOffset + 12, this.m_dwCheckSum);
    provider.WriteInt32(iOffset + 16 /*0x10*/, this.m_info);
    provider.WriteInt32(iOffset + 18, (int) Enum.Parse(typeof (LayoutModes), this.m_wXMode.ToString(), true));
    provider.WriteInt32(iOffset + 20, (int) Enum.Parse(typeof (LayoutModes), this.m_wYMode.ToString(), true));
    provider.WriteInt32(iOffset + 22, (int) Enum.Parse(typeof (LayoutModes), this.m_wWidthMode.ToString(), true));
    provider.WriteInt32(iOffset + 24, (int) Enum.Parse(typeof (LayoutModes), this.m_wHeightMode.ToString(), true));
    provider.WriteDouble(iOffset + 26, this.m_x);
    provider.WriteDouble(iOffset + 34, this.m_y);
    provider.WriteDouble(iOffset + 42, this.m_dx);
    provider.WriteDouble(iOffset + 50, this.m_dy);
    provider.WriteBytes(iOffset + 58, this.m_reserved2);
  }

  public override int GetStoreSize(ExcelVersion version) => 60;

  private int CheckSum() => 0;
}
