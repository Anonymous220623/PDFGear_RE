// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartTextRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartText)]
[CLSCompliant(false)]
internal class ChartTextRecord : BiffRecordRaw
{
  public const int DEF_ROTATION_MASK = 1792 /*0x0700*/;
  public const int DEF_FIRST_ROTATION_BIT = 8;
  public const int DEF_DATA_LABEL_MASK = 15;
  public const int DEF_DATA_LABEL_FIRST_BIT = 0;
  public const int DEF_RECORD_SIZE = 32 /*0x20*/;
  [BiffRecordPos(0, 1)]
  private byte m_HorzAlign = 1;
  [BiffRecordPos(1, 1)]
  private byte m_VertAlign = 1;
  [BiffRecordPos(2, 2)]
  private ushort m_usBkgMode = 1;
  [BiffRecordPos(4, 4)]
  private uint m_uiTextColor;
  [BiffRecordPos(8, 4)]
  private uint m_uiXPos;
  [BiffRecordPos(12, 4)]
  private uint m_uiYPos;
  [BiffRecordPos(16 /*0x10*/, 4)]
  private uint m_uiXSize;
  [BiffRecordPos(20, 4)]
  private uint m_uiYSize;
  [BiffRecordPos(24, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(26, 2)]
  private ushort m_usColorIndex;
  [BiffRecordPos(28, 2)]
  private ushort m_usOptions2;
  [BiffRecordPos(24, 0, TFieldType.Bit)]
  private bool m_bAutoColor = true;
  [BiffRecordPos(24, 1, TFieldType.Bit)]
  private bool m_bShowKey;
  [BiffRecordPos(24, 2, TFieldType.Bit)]
  private bool m_bShowValue;
  [BiffRecordPos(24, 3, TFieldType.Bit)]
  private bool m_bVertical;
  [BiffRecordPos(24, 4, TFieldType.Bit)]
  private bool m_bAutoText = true;
  [BiffRecordPos(24, 5, TFieldType.Bit)]
  private bool m_bGenerated = true;
  [BiffRecordPos(24, 6, TFieldType.Bit)]
  private bool m_bDeleted;
  [BiffRecordPos(24, 7, TFieldType.Bit)]
  private bool m_bAutoMode = true;
  [BiffRecordPos(25, 3, TFieldType.Bit)]
  private bool m_bShowLabelPercent;
  [BiffRecordPos(25, 4, TFieldType.Bit)]
  private bool m_bShowPercent;
  [BiffRecordPos(25, 5, TFieldType.Bit)]
  private bool m_bShowBubbleSizes;
  [BiffRecordPos(25, 6, TFieldType.Bit)]
  private bool m_bShowLabel;
  [BiffRecordPos(30, 2, true)]
  private short? m_sRotation;

  public ExcelChartHorzAlignment HorzAlign
  {
    get => (ExcelChartHorzAlignment) this.m_HorzAlign;
    set => this.m_HorzAlign = (byte) value;
  }

  public ExcelChartVertAlignment VertAlign
  {
    get => (ExcelChartVertAlignment) this.m_VertAlign;
    set => this.m_VertAlign = (byte) value;
  }

  public OfficeChartBackgroundMode BackgroundMode
  {
    get => (OfficeChartBackgroundMode) this.m_usBkgMode;
    set => this.m_usBkgMode = (ushort) value;
  }

  public uint TextColor
  {
    get => this.m_uiTextColor;
    set => this.m_uiTextColor = value;
  }

  public uint XPos
  {
    get => this.m_uiXPos;
    set => this.m_uiXPos = value;
  }

  public uint YPos
  {
    get => this.m_uiYPos;
    set => this.m_uiYPos = value;
  }

  public uint XSize
  {
    get => this.m_uiXSize;
    set => this.m_uiXSize = value;
  }

  public uint YSize
  {
    get => this.m_uiYSize;
    set => this.m_uiYSize = value;
  }

  public ushort Options => this.m_usOptions;

  public OfficeKnownColors ColorIndex
  {
    get => (OfficeKnownColors) this.m_usColorIndex;
    set => this.m_usColorIndex = (ushort) value;
  }

  public ushort Options2
  {
    get => this.m_usOptions2;
    set => this.m_usOptions2 = value;
  }

  public bool IsAutoColor
  {
    get => this.m_bAutoColor;
    set => this.m_bAutoColor = value;
  }

  public bool IsShowKey
  {
    get => this.m_bShowKey;
    set => this.m_bShowKey = value;
  }

  public bool IsShowValue
  {
    get => this.m_bShowValue;
    set => this.m_bShowValue = value;
  }

  public bool IsVertical
  {
    get => this.m_bVertical;
    set => this.m_bVertical = value;
  }

  public bool IsAutoText
  {
    get => this.m_bAutoText;
    set => this.m_bAutoText = value;
  }

  public bool IsGenerated
  {
    get => this.m_bGenerated;
    set => this.m_bGenerated = value;
  }

  public bool IsDeleted
  {
    get => this.m_bDeleted;
    set => this.m_bDeleted = value;
  }

  public bool IsAutoMode
  {
    get => this.m_bAutoMode;
    set => this.m_bAutoMode = value;
  }

  public bool IsShowLabelPercent
  {
    get => this.m_bShowLabelPercent;
    set => this.m_bShowLabelPercent = value;
  }

  public bool IsShowPercent
  {
    get => this.m_bShowPercent;
    set => this.m_bShowPercent = value;
  }

  public bool IsShowBubbleSizes
  {
    get => this.m_bShowBubbleSizes;
    set => this.m_bShowBubbleSizes = value;
  }

  public bool IsShowLabel
  {
    get => this.m_bShowLabel;
    set => this.m_bShowLabel = value;
  }

  public TRotation Rotation
  {
    get
    {
      return (TRotation) ((int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions, (ushort) 1792 /*0x0700*/) >> 8);
    }
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions, (ushort) 1792 /*0x0700*/, (ushort) ((uint) (ushort) value << 8));
    }
  }

  public OfficeDataLabelPosition DataLabelPlacement
  {
    get
    {
      return (OfficeDataLabelPosition) BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions2, (ushort) 15);
    }
    set => BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions2, (ushort) 15, (ushort) value);
  }

  public short TextRotation
  {
    get => this.m_sRotation ?? (short) 0;
    set => this.m_sRotation = new short?(value);
  }

  public short? TextRotationOrNull
  {
    get => this.m_sRotation;
    set => this.m_sRotation = value;
  }

  public override int MinimumRecordSize => 32 /*0x20*/;

  public override int MaximumRecordSize => 32 /*0x20*/;

  public ChartTextRecord()
  {
  }

  public ChartTextRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartTextRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_HorzAlign = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_VertAlign = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_usBkgMode = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_uiTextColor = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiXPos = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiYPos = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiXSize = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiYSize = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bAutoColor = provider.ReadBit(iOffset, 0);
    this.m_bShowKey = provider.ReadBit(iOffset, 1);
    this.m_bShowValue = provider.ReadBit(iOffset, 2);
    this.m_bVertical = provider.ReadBit(iOffset, 3);
    this.m_bAutoText = provider.ReadBit(iOffset, 4);
    this.m_bGenerated = provider.ReadBit(iOffset, 5);
    this.m_bDeleted = provider.ReadBit(iOffset, 6);
    this.m_bAutoMode = provider.ReadBit(iOffset, 7);
    ++iOffset;
    this.m_bShowLabelPercent = provider.ReadBit(iOffset, 3);
    this.m_bShowPercent = provider.ReadBit(iOffset, 4);
    this.m_bShowBubbleSizes = provider.ReadBit(iOffset, 5);
    this.m_bShowLabel = provider.ReadBit(iOffset, 6);
    ++iOffset;
    this.m_usColorIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usOptions2 = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_sRotation = new short?(provider.ReadInt16(iOffset));
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteByte(iOffset, this.m_HorzAlign);
    provider.WriteByte(iOffset + 1, this.m_VertAlign);
    provider.WriteUInt16(iOffset + 2, this.m_usBkgMode);
    provider.WriteUInt32(iOffset + 4, this.m_uiTextColor);
    provider.WriteUInt32(iOffset + 8, this.m_uiXPos);
    provider.WriteUInt32(iOffset + 12, this.m_uiYPos);
    provider.WriteUInt32(iOffset + 16 /*0x10*/, this.m_uiXSize);
    provider.WriteUInt32(iOffset + 20, this.m_uiYSize);
    provider.WriteUInt16(iOffset + 24, this.m_usOptions);
    provider.WriteBit(iOffset + 24, this.m_bAutoColor, 0);
    provider.WriteBit(iOffset + 24, this.m_bShowKey, 1);
    provider.WriteBit(iOffset + 24, this.m_bShowValue, 2);
    provider.WriteBit(iOffset + 24, this.m_bVertical, 3);
    provider.WriteBit(iOffset + 24, this.m_bAutoText, 4);
    provider.WriteBit(iOffset + 24, this.m_bGenerated, 5);
    provider.WriteBit(iOffset + 24, this.m_bDeleted, 6);
    provider.WriteBit(iOffset + 24, this.m_bAutoMode, 7);
    provider.WriteBit(iOffset + 25, this.m_bShowLabelPercent, 3);
    provider.WriteBit(iOffset + 25, this.m_bShowPercent, 4);
    provider.WriteBit(iOffset + 25, this.m_bShowBubbleSizes, 5);
    provider.WriteBit(iOffset + 25, this.m_bShowLabel, 6);
    provider.WriteUInt16(iOffset + 26, this.m_usColorIndex);
    provider.WriteUInt16(iOffset + 28, this.m_usOptions2);
    provider.WriteInt16(iOffset + 30, this.TextRotation);
    this.m_iLength = 32 /*0x20*/;
  }
}
