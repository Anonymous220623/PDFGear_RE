// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutInfo : ILayoutInfo
{
  private ushort m_bFlags = 64 /*0x40*/;
  private ChildrenLayoutDirection m_childrenLayoutDirection;
  private SizeF m_size;
  private SyncFont m_font;

  public LayoutInfo() => this.IsSkip = true;

  public LayoutInfo(ChildrenLayoutDirection childLayoutDirection)
  {
    this.m_childrenLayoutDirection = childLayoutDirection;
  }

  public bool IsClipped
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65534 | (value ? 1 : 0));
  }

  public SizeF Size
  {
    get => this.m_size;
    set => this.m_size = value;
  }

  public bool IsSkip
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65533 | (value ? 2 : 0));
  }

  public bool IsSkipBottomAlign
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65531 | (value ? 4 : 0));
  }

  public bool IsVerticalText
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65527 | (value ? 8 : 0));
  }

  public bool IsLineContainer
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65519 | (value ? 16 /*0x10*/ : 0));
  }

  public ChildrenLayoutDirection ChildrenLayoutDirection => this.m_childrenLayoutDirection;

  public bool IsLineBreak
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65503 | (value ? 32 /*0x20*/ : 0));
  }

  public bool TextWrap
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65471 | (value ? 64 /*0x40*/ : 0));
  }

  public bool IsPageBreakItem
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65407 | (value ? 128 /*0x80*/ : 0));
  }

  public bool IsFirstItemInPage
  {
    get => ((int) this.m_bFlags & 256 /*0x0100*/) >> 8 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65279 | (value ? 256 /*0x0100*/ : 0));
  }

  public bool IsKeepWithNext
  {
    get => ((int) this.m_bFlags & 512 /*0x0200*/) >> 9 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65023 | (value ? 512 /*0x0200*/ : 0));
  }

  public bool IsHiddenRow
  {
    get => ((int) this.m_bFlags & 1024 /*0x0400*/) >> 10 != 0;
    set
    {
      this.m_bFlags = (ushort) ((long) ((uint) this.m_bFlags & 4294966271U) | (long) ((value ? 1 : 0) << 10));
    }
  }

  public SyncFont Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }
}
