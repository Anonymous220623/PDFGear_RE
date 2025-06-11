// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsofbtClientAnchor
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
[Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtClientAnchor)]
internal class MsofbtClientAnchor : MsoBase
{
  private const uint DEF_CELL_MASK = 65535 /*0xFFFF*/;
  private const uint DEF_OFFSET_MASK = 4294901760;
  private const int DEF_OFFSET_START_BIT = 16 /*0x10*/;
  private const int DEF_SHORT_LENGTH = 8;
  private const int DEF_RECORD_SIZE = 18;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bNotMoveWithCell;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_bNotSizeWithCell;
  [BiffRecordPos(2, 4)]
  private uint m_uiLeft;
  [BiffRecordPos(10, 4)]
  private uint m_uiRight;
  private bool m_bShortVersion;
  private int m_iTopRow;
  private int m_iTopOffset;
  private int m_iBottomRow;
  private int m_iBottomOffset;
  private bool m_bOneCellAnchor;

  public ushort Options
  {
    get => this.m_usOptions;
    set => this.m_usOptions = value;
  }

  public bool IsSizeWithCell
  {
    get => !this.m_bNotSizeWithCell;
    set => this.m_bNotSizeWithCell = !value;
  }

  public bool IsMoveWithCell
  {
    get => !this.m_bNotMoveWithCell;
    set => this.m_bNotMoveWithCell = !value;
  }

  public int LeftColumn
  {
    get => (int) BiffRecordRaw.GetUInt32BitsByMask(this.m_uiLeft, (uint) ushort.MaxValue);
    set
    {
      BiffRecordRaw.SetUInt32BitsByMask(ref this.m_uiLeft, (uint) ushort.MaxValue, (uint) value);
    }
  }

  public int RightColumn
  {
    get => (int) BiffRecordRaw.GetUInt32BitsByMask(this.m_uiRight, (uint) ushort.MaxValue);
    set
    {
      BiffRecordRaw.SetUInt32BitsByMask(ref this.m_uiRight, (uint) ushort.MaxValue, (uint) value);
    }
  }

  public int TopRow
  {
    get => this.m_iTopRow;
    set => this.m_iTopRow = value;
  }

  public int BottomRow
  {
    get => this.m_iBottomRow;
    set => this.m_iBottomRow = value;
  }

  public int LeftOffset
  {
    get => (int) (BiffRecordRaw.GetUInt32BitsByMask(this.m_uiLeft, 4294901760U) >> 16 /*0x10*/);
    set
    {
      BiffRecordRaw.SetUInt32BitsByMask(ref this.m_uiLeft, 4294901760U, (uint) (value << 16 /*0x10*/));
    }
  }

  public int TopOffset
  {
    get => this.m_iTopOffset;
    set => this.m_iTopOffset = value;
  }

  public int RightOffset
  {
    get => (int) (BiffRecordRaw.GetUInt32BitsByMask(this.m_uiRight, 4294901760U) >> 16 /*0x10*/);
    set
    {
      BiffRecordRaw.SetUInt32BitsByMask(ref this.m_uiRight, 4294901760U, (uint) (value << 16 /*0x10*/));
    }
  }

  public int BottomOffset
  {
    get => this.m_iBottomOffset;
    set => this.m_iBottomOffset = value;
  }

  public bool IsShortVersion
  {
    get => this.m_bShortVersion;
    set => this.m_bShortVersion = value;
  }

  public bool OneCellAnchor
  {
    get => this.m_bOneCellAnchor;
    set => this.m_bOneCellAnchor = value;
  }

  public MsofbtClientAnchor(MsoBase parent)
    : base(parent)
  {
  }

  public MsofbtClientAnchor(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    uint destination1 = 0;
    uint destination2 = 0;
    BiffRecordRaw.SetUInt32BitsByMask(ref destination1, (uint) ushort.MaxValue, (uint) this.m_iTopRow);
    BiffRecordRaw.SetUInt32BitsByMask(ref destination1, 4294901760U, (uint) (this.m_iTopOffset << 16 /*0x10*/));
    BiffRecordRaw.SetUInt32BitsByMask(ref destination2, (uint) ushort.MaxValue, (uint) this.m_iBottomRow);
    BiffRecordRaw.SetUInt32BitsByMask(ref destination2, 4294901760U, (uint) (this.m_iBottomOffset << 16 /*0x10*/));
    if (!this.m_bShortVersion)
    {
      this.m_iLength = 18;
      this.SetBitInVar(ref this.m_usOptions, this.m_bNotMoveWithCell, 0);
      this.SetBitInVar(ref this.m_usOptions, this.m_bNotSizeWithCell, 1);
      MsoBase.WriteUInt16(stream, this.m_usOptions);
      MsoBase.WriteUInt32(stream, this.m_uiLeft);
      MsoBase.WriteUInt32(stream, destination1);
      MsoBase.WriteUInt32(stream, this.m_uiRight);
      MsoBase.WriteUInt32(stream, destination2);
    }
    else
    {
      this.m_iLength = 8;
      MsoBase.WriteUInt32(stream, this.m_uiLeft);
      MsoBase.WriteUInt32(stream, destination1);
    }
  }

  public override void ParseStructure(Stream stream)
  {
    this.m_bShortVersion = this.m_iLength == 8;
    uint num1 = 0;
    uint num2;
    if (!this.m_bShortVersion)
    {
      this.m_usOptions = MsoBase.ReadUInt16(stream);
      this.m_bNotMoveWithCell = BiffRecordRaw.GetBitFromVar(this.m_usOptions, 0);
      this.m_bNotSizeWithCell = BiffRecordRaw.GetBitFromVar(this.m_usOptions, 1);
      this.m_uiLeft = MsoBase.ReadUInt32(stream);
      num2 = MsoBase.ReadUInt32(stream);
      this.m_uiRight = MsoBase.ReadUInt32(stream);
      num1 = MsoBase.ReadUInt32(stream);
    }
    else
    {
      this.m_uiLeft = MsoBase.ReadUInt32(stream);
      num2 = MsoBase.ReadUInt32(stream);
    }
    this.m_iTopRow = (int) BiffRecordRaw.GetUInt32BitsByMask(num2, (uint) ushort.MaxValue);
    this.m_iTopOffset = (int) (BiffRecordRaw.GetUInt32BitsByMask(num2, 4294901760U) >> 16 /*0x10*/);
    this.m_iBottomRow = (int) BiffRecordRaw.GetUInt32BitsByMask(num1, (uint) ushort.MaxValue);
    this.m_iBottomOffset = (int) (BiffRecordRaw.GetUInt32BitsByMask(num1, 4294901760U) >> 16 /*0x10*/);
  }

  public override int GetStoreSize(OfficeVersion version) => !this.m_bShortVersion ? 18 : 8;
}
