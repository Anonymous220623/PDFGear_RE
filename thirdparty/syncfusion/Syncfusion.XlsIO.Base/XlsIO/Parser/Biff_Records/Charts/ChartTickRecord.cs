// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartTickRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartTick)]
public class ChartTickRecord : BiffRecordRaw
{
  public const ushort DEF_ROTATION_MASK = 28;
  public const ushort DEF_FIRST_ROTATION_BIT = 2;
  public const int DEF_RECORD_SIZE = 30;
  private const int DEF_MAX_ANGLE = 90;
  private const int ReservedFieldSize = 16 /*0x10*/;
  [BiffRecordPos(0, 1)]
  private byte m_MajorMark = 2;
  [BiffRecordPos(1, 1)]
  private byte m_MinorMark;
  [BiffRecordPos(2, 1)]
  private byte m_labelPos = 3;
  [BiffRecordPos(3, 1)]
  private byte m_BackgroundMode = 1;
  [BiffRecordPos(4, 4)]
  private uint m_uiTextColor;
  [BiffRecordPos(4, 4)]
  private int m_iTextColor;
  [BiffRecordPos(24, 0, TFieldType.Bit)]
  private bool m_bAutoTextColor = true;
  [BiffRecordPos(24, 2)]
  private ushort m_usFlags;
  [BiffRecordPos(24, 1, TFieldType.Bit)]
  private bool m_bAutoTextBack = true;
  [BiffRecordPos(24, 5, TFieldType.Bit)]
  private bool m_bAutoRotation = true;
  [BiffRecordPos(26, 2)]
  private ushort m_usTickColorIndex;
  [BiffRecordPos(28, 2, true)]
  private short m_sRotationAngle;
  [BiffRecordPos(25, 6, TFieldType.Bit)]
  private bool m_bIsLeftToRight;
  [BiffRecordPos(25, 7, TFieldType.Bit)]
  private bool m_bIsRightToLeft;

  public ExcelTickMark MajorMark
  {
    get => (ExcelTickMark) this.m_MajorMark;
    set => this.m_MajorMark = (byte) value;
  }

  public ExcelTickMark MinorMark
  {
    get => (ExcelTickMark) this.m_MinorMark;
    set => this.m_MinorMark = (byte) value;
  }

  public ExcelTickLabelPosition LabelPos
  {
    get => (ExcelTickLabelPosition) this.m_labelPos;
    set => this.m_labelPos = (byte) value;
  }

  public ExcelChartBackgroundMode BackgroundMode
  {
    get => (ExcelChartBackgroundMode) this.m_BackgroundMode;
    set => this.m_BackgroundMode = (byte) value;
  }

  public uint TextColor
  {
    get => this.m_uiTextColor;
    set => this.m_uiTextColor = value;
  }

  internal int TextColorInInt
  {
    get => this.m_iTextColor;
    set => this.m_iTextColor = value;
  }

  public ushort Flags => this.m_usFlags;

  public ushort TickColorIndex
  {
    get => this.m_usTickColorIndex;
    set => this.m_usTickColorIndex = value;
  }

  public short RotationAngle
  {
    get
    {
      return this.m_sRotationAngle > (short) 90 ? (short) (90 - (int) this.m_sRotationAngle) : this.m_sRotationAngle;
    }
    set
    {
      if (value < (short) -90 || value > (short) 90)
        throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less -90 and greater than 90");
      if (value >= (short) 0)
        this.m_sRotationAngle = value;
      else
        this.m_sRotationAngle = (short) (90 - (int) value);
    }
  }

  public bool IsAutoTextColor
  {
    get => this.m_bAutoTextColor;
    set => this.m_bAutoTextColor = value;
  }

  public bool IsAutoTextBack
  {
    get => this.m_bAutoTextBack;
    set => this.m_bAutoTextBack = value;
  }

  public bool IsAutoRotation
  {
    get => this.m_bAutoRotation;
    set => this.m_bAutoRotation = value;
  }

  public bool IsLeftToRight
  {
    get => this.m_bIsLeftToRight;
    set => this.m_bIsLeftToRight = value;
  }

  public bool IsRightToLeft
  {
    get => this.m_bIsRightToLeft;
    set => this.m_bIsRightToLeft = value;
  }

  public TRotation Rotation
  {
    get => (TRotation) ((int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usFlags, (ushort) 28) >> 2);
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usFlags, (ushort) 28, (ushort) ((uint) (ushort) value << 2));
    }
  }

  public override int MinimumRecordSize => 30;

  public override int MaximumRecordSize => 30;

  public ChartTickRecord()
  {
  }

  public ChartTickRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartTickRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_MajorMark = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_MinorMark = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_labelPos = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_BackgroundMode = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_uiTextColor = provider.ReadUInt32(iOffset);
    this.m_iTextColor = provider.ReadInt32(iOffset);
    iOffset += 4;
    int num = 0;
    while (num < 16 /*0x10*/)
    {
      provider.WriteByte(iOffset, (byte) 0);
      ++num;
      ++iOffset;
    }
    this.m_usFlags = provider.ReadUInt16(iOffset);
    this.m_bAutoTextColor = provider.ReadBit(iOffset, 0);
    this.m_bAutoTextBack = provider.ReadBit(iOffset, 1);
    this.m_bAutoRotation = provider.ReadBit(iOffset, 5);
    ++iOffset;
    this.m_bIsLeftToRight = provider.ReadBit(iOffset, 6);
    this.m_bIsRightToLeft = provider.ReadBit(iOffset, 7);
    ++iOffset;
    this.m_usTickColorIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_sRotationAngle = provider.ReadInt16(iOffset);
    if (this.m_sRotationAngle != (short) 0)
      this.m_bAutoRotation = false;
    iOffset += 2;
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteByte(iOffset, this.m_MajorMark);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_MinorMark);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_labelPos);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_BackgroundMode);
    ++iOffset;
    provider.WriteInt32(iOffset, this.m_iTextColor);
    iOffset += 4;
    iOffset += 16 /*0x10*/;
    provider.WriteUInt16(iOffset, this.m_usFlags);
    provider.WriteBit(iOffset, this.m_bAutoTextColor, 0);
    provider.WriteBit(iOffset, this.m_bAutoTextBack, 1);
    provider.WriteBit(iOffset, this.m_bAutoRotation, 5);
    ++iOffset;
    provider.WriteBit(iOffset, this.m_bIsLeftToRight, 6);
    provider.WriteBit(iOffset, this.m_bIsRightToLeft, 7);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_usTickColorIndex);
    iOffset += 2;
    provider.WriteInt16(iOffset, this.m_sRotationAngle);
    iOffset += 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 30;
}
