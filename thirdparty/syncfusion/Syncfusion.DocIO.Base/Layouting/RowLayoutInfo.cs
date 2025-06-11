// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.RowLayoutInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.Layouting;

internal class RowLayoutInfo : LayoutInfo, ILayoutSpacingsInfo
{
  private ushort m_bFlags;
  private double m_rowHeight;
  private Spacings m_margins;
  private Spacings m_paddings;

  internal bool IsFootnoteReduced
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65534 | (value ? 1 : 0));
  }

  internal bool IsFootnoteSplitted
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65533 | (value ? 1 : 0) << 1);
  }

  internal bool IsExactlyRowHeight
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65531 | (value ? 1 : 0) << 2);
  }

  internal bool IsRowSplitted
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65527 | (value ? 1 : 0) << 3);
  }

  internal double RowHeight
  {
    get
    {
      if (this.m_rowHeight < 0.0)
        this.m_rowHeight = -this.m_rowHeight;
      return this.m_rowHeight;
    }
  }

  internal bool IsRowHasVerticalMergeContinueCell
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65519 | (value ? 1 : 0) << 4);
  }

  internal bool IsRowHasVerticalMergeEndCell
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65503 | (value ? 1 : 0) << 5);
  }

  internal bool IsRowHasVerticalMergeStartCell
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65471 | (value ? 1 : 0) << 6);
  }

  internal bool IsRowHasVerticalTextCell
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65407 | (value ? 1 : 0) << 7);
  }

  internal bool IsRowBreakByPageBreakBefore
  {
    get => ((int) this.m_bFlags & 256 /*0x0100*/) >> 8 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65279 | (value ? 1 : 0) << 8);
  }

  internal bool IsRowHeightExceedsClientByFloatingItem
  {
    get => ((int) this.m_bFlags & 512 /*0x0200*/) >> 9 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65023 | (value ? 1 : 0) << 9);
  }

  internal bool IsCellPaddingUpdated
  {
    get => ((int) this.m_bFlags & 1024 /*0x0400*/) >> 10 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 64511 | (value ? 1 : 0) << 10);
  }

  internal bool IsRowSplittedByFloatingItem
  {
    get => ((int) this.m_bFlags & 2048 /*0x0800*/) >> 11 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 63487 | (value ? 1 : 0) << 11);
  }

  public Spacings Paddings
  {
    get
    {
      if (this.m_paddings == null)
        this.m_paddings = new Spacings();
      return this.m_paddings;
    }
  }

  public Spacings Margins
  {
    get
    {
      if (this.m_margins == null)
        this.m_margins = new Spacings();
      return this.m_margins;
    }
  }

  public RowLayoutInfo(bool isExactlyRow, float rowHeight)
  {
    this.IsExactlyRowHeight = isExactlyRow;
    this.m_rowHeight = (double) rowHeight;
  }
}
