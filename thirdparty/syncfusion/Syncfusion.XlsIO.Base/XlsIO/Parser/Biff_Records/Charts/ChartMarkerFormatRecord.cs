// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartMarkerFormatRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartMarkerFormat)]
[CLSCompliant(false)]
public class ChartMarkerFormatRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 20;
  [BiffRecordPos(0, 4, true)]
  private int m_iForeColor;
  [BiffRecordPos(4, 4, true)]
  private int m_iBackColor;
  [BiffRecordPos(8, 2)]
  private ushort m_usMarkerType = 1;
  [BiffRecordPos(10, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(12, 2)]
  private ushort m_usBorderColorIndex;
  [BiffRecordPos(14, 2)]
  private ushort m_usFillColorIndex;
  [BiffRecordPos(16 /*0x10*/, 4, true)]
  private int m_iLineSize = 100;
  [BiffRecordPos(10, 0, TFieldType.Bit)]
  private bool m_bAutoColor = true;
  [BiffRecordPos(10, 4, TFieldType.Bit)]
  private bool m_bNotShowInt;
  [BiffRecordPos(10, 5, TFieldType.Bit)]
  private bool m_bNotShowBrd;
  private byte m_flagOptions;

  public int ForeColor
  {
    get => this.m_iForeColor;
    set
    {
      this.m_iForeColor = value;
      this.m_flagOptions |= (byte) 1;
    }
  }

  public int BackColor
  {
    get => this.m_iBackColor;
    set
    {
      this.m_iBackColor = value;
      this.m_flagOptions |= (byte) 2;
    }
  }

  public ExcelChartMarkerType MarkerType
  {
    get => (ExcelChartMarkerType) this.m_usMarkerType;
    set
    {
      this.m_usMarkerType = (ushort) value;
      this.m_flagOptions |= (byte) 16 /*0x10*/;
    }
  }

  public ushort Options => this.m_usOptions;

  public ushort BorderColorIndex
  {
    get => this.m_usBorderColorIndex;
    set
    {
      this.m_usBorderColorIndex = value;
      this.m_flagOptions |= (byte) 2;
    }
  }

  public ushort FillColorIndex
  {
    get => this.m_usFillColorIndex;
    set
    {
      this.m_usFillColorIndex = value;
      this.m_flagOptions |= (byte) 1;
    }
  }

  public int LineSize
  {
    get => this.m_iLineSize;
    set
    {
      this.m_iLineSize = value;
      this.m_flagOptions |= (byte) 32 /*0x20*/;
    }
  }

  public bool IsAutoColor
  {
    get => this.m_bAutoColor;
    set
    {
      this.m_bAutoColor = value;
      if (!value)
        return;
      this.m_flagOptions = (byte) 0;
    }
  }

  public bool IsNotShowInt
  {
    get => this.m_bNotShowInt;
    set
    {
      this.m_bNotShowInt = value;
      this.m_flagOptions |= (byte) 4;
    }
  }

  public bool IsNotShowBrd
  {
    get => this.m_bNotShowBrd;
    set
    {
      this.m_bNotShowBrd = value;
      this.m_flagOptions |= (byte) 8;
    }
  }

  public override int MinimumRecordSize => 20;

  public override int MaximumRecordSize => 20;

  internal bool HasLineProperties => ((int) this.m_flagOptions & 2) != 0;

  internal byte FlagOptions
  {
    get => this.m_flagOptions;
    set => this.m_flagOptions = value;
  }

  public ChartMarkerFormatRecord()
  {
  }

  public ChartMarkerFormatRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartMarkerFormatRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_iForeColor = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iBackColor = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_usMarkerType = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bAutoColor = provider.ReadBit(iOffset, 0);
    this.m_bNotShowInt = provider.ReadBit(iOffset, 4);
    this.m_bNotShowBrd = provider.ReadBit(iOffset, 5);
    iOffset += 2;
    this.m_usBorderColorIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usFillColorIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_iLineSize = provider.ReadInt32(iOffset);
    if (this.m_bAutoColor)
      return;
    this.m_flagOptions = (byte) 63 /*0x3F*/;
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteInt32(iOffset, this.m_iForeColor);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iBackColor);
    iOffset += 4;
    provider.WriteUInt16(iOffset, this.m_usMarkerType);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bAutoColor, 0);
    provider.WriteBit(iOffset, this.m_bNotShowInt, 4);
    provider.WriteBit(iOffset, this.m_bNotShowBrd, 5);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usBorderColorIndex);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usFillColorIndex);
    iOffset += 2;
    provider.WriteInt32(iOffset, this.m_iLineSize);
  }

  public enum TMarker
  {
    NoMarker,
    Square,
    Diamond,
    Triangle,
    X,
    Star,
    DowJones,
    StandardDeviation,
    Circle,
    PlusSign,
  }
}
