// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.TextObjectRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.TextObject)]
[CLSCompliant(false)]
public class TextObjectRecord : BiffRecordRawWithArray
{
  public const ushort HAlignmentBitMask = 14;
  public const ushort VAlignmentBitMask = 112 /*0x70*/;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(1, 2, TFieldType.Bit)]
  private bool m_bLockText;
  [BiffRecordPos(2, 2)]
  private ushort m_usRotation;
  [BiffRecordPos(4, 4)]
  private uint m_uiReserved1;
  [BiffRecordPos(8, 2)]
  private ushort m_usReserved2;
  [BiffRecordPos(10, 2)]
  private ushort m_usTextLen;
  [BiffRecordPos(12, 2)]
  private ushort m_usFormattingRunsLen;
  [BiffRecordPos(14, 4)]
  private uint m_uiReserved3;

  public ExcelCommentHAlign HAlignment
  {
    get
    {
      return (ExcelCommentHAlign) ((int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions, (ushort) 14) >> 1);
    }
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions, (ushort) 14, (ushort) ((uint) (ushort) value << 1));
    }
  }

  public ExcelCommentVAlign VAlignment
  {
    get
    {
      return (ExcelCommentVAlign) ((int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions, (ushort) 112 /*0x70*/) >> 4);
    }
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions, (ushort) 112 /*0x70*/, (ushort) ((uint) (ushort) value << 4));
    }
  }

  public bool IsLockText
  {
    get => this.m_bLockText;
    set => this.m_bLockText = value;
  }

  public ExcelTextRotation Rotation
  {
    get => (ExcelTextRotation) this.m_usRotation;
    set => this.m_usRotation = (ushort) value;
  }

  public ushort TextLen
  {
    get => this.m_usTextLen;
    set => this.m_usTextLen = value;
  }

  public ushort FormattingRunsLen
  {
    get => this.m_usFormattingRunsLen;
    set => this.m_usFormattingRunsLen = value;
  }

  public uint Reserved1 => this.m_uiReserved1;

  public ushort Reserved2 => this.m_usReserved2;

  public uint Reserved3 => this.m_uiReserved3;

  public override int MinimumRecordSize => 18;

  public override int MaximumRecordSize => 18;

  public TextObjectRecord()
  {
  }

  public TextObjectRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public TextObjectRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure()
  {
    this.m_usOptions = this.GetUInt16(0);
    this.m_bLockText = this.GetBit(1, 1);
    this.m_usRotation = this.GetUInt16(2);
    this.m_uiReserved1 = this.GetUInt32(4);
    this.m_usReserved2 = this.GetUInt16(8);
    this.m_usTextLen = this.GetUInt16(10);
    this.m_usFormattingRunsLen = this.GetUInt16(12);
    this.m_uiReserved3 = this.GetUInt32(14);
    this.m_data = new byte[0];
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    this.m_iLength = this.MinimumRecordSize;
    this.m_data = new byte[this.m_iLength];
    this.SetUInt16(0, this.m_usOptions);
    this.SetBit(1, this.m_bLockText, 2);
    this.SetBit(1, this.m_bLockText, 1);
    this.SetUInt16(2, this.m_usRotation);
    this.SetUInt32(4, this.m_uiReserved1);
    this.SetUInt16(8, this.m_usReserved2);
    this.SetUInt16(10, this.m_usTextLen);
    this.SetUInt16(12, this.m_usFormattingRunsLen);
    this.SetUInt32(14, this.m_uiReserved3);
  }
}
