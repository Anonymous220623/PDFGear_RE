// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutArea
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutArea
{
  private RectangleF m_area;
  private RectangleF m_clientArea;
  private RectangleF m_clientActiveArea;
  private ILayoutSpacingsInfo m_spacings;
  private bool m_bSkipSubtractWhenInvalidParameter = true;

  public double Width => (double) this.OuterArea.Width;

  public double Height => (double) this.OuterArea.Height;

  public bool SkipSubtractWhenInvalidParameter
  {
    get => this.m_bSkipSubtractWhenInvalidParameter;
    set => this.m_bSkipSubtractWhenInvalidParameter = value;
  }

  public Spacings Margins => this.m_spacings.Margins;

  public Spacings Paddings => this.m_spacings.Paddings;

  public RectangleF OuterArea => this.m_area;

  public RectangleF ClientArea => this.m_clientArea;

  public RectangleF ClientActiveArea => this.m_clientActiveArea;

  public LayoutArea()
    : this(new RectangleF(), (ILayoutSpacingsInfo) null, (IWidget) null)
  {
  }

  public LayoutArea(RectangleF area)
    : this(area, (ILayoutSpacingsInfo) null, (IWidget) null)
  {
  }

  public LayoutArea(RectangleF area, ILayoutSpacingsInfo spacings, IWidget widget)
  {
    this.m_area = area;
    this.m_spacings = spacings;
    this.UpdateClientArea(widget);
  }

  public void CutFromLeft(double x, bool isSkip)
  {
    if (x < (double) this.m_clientActiveArea.Left || x > (double) this.m_clientActiveArea.Right)
    {
      if (!this.SkipSubtractWhenInvalidParameter)
        throw new ArgumentException(nameof (x));
      if (x < (double) this.m_clientActiveArea.Left && !isSkip)
        x = (double) this.m_clientActiveArea.Left;
    }
    RectangleF clientActiveArea = this.m_clientActiveArea;
    clientActiveArea.Width = clientActiveArea.Right - (float) x;
    if ((double) clientActiveArea.Width < 0.0)
      clientActiveArea.Width = 0.0f;
    clientActiveArea.X = (float) x;
    this.m_clientActiveArea = clientActiveArea;
  }

  public void CutFromLeft(double x) => this.CutFromLeft(x, false);

  internal void UpdateDynamicRelayoutBounds(
    float x,
    float y,
    bool isNeedToUpdateWidth,
    float width)
  {
    RectangleF clientActiveArea = this.m_clientActiveArea;
    clientActiveArea.Height += clientActiveArea.Y - y;
    clientActiveArea.X = x;
    clientActiveArea.Y = y;
    if (isNeedToUpdateWidth)
      clientActiveArea.Width = width;
    this.m_clientActiveArea = clientActiveArea;
  }

  public void CutFromTop(double y) => this.CutFromTop(y, 0.0f);

  public void CutFromTop(double y, float footnoteHeight)
  {
    this.CutFromTop(y, footnoteHeight, false);
  }

  internal void CutFromTop(double y, float footnoteHeight, bool isSkip)
  {
    if (y < (double) this.m_clientActiveArea.Top || y > (double) this.m_clientActiveArea.Bottom)
    {
      if (!this.SkipSubtractWhenInvalidParameter)
        throw new ArgumentException(nameof (y));
      if (y < (double) this.m_clientActiveArea.Top)
        y = (double) this.m_clientActiveArea.Top;
      else if (y > (double) this.m_clientActiveArea.Bottom && !isSkip)
        y = (double) this.m_clientActiveArea.Bottom;
    }
    RectangleF clientActiveArea = this.m_clientActiveArea;
    float num = clientActiveArea.Bottom - (float) y - footnoteHeight;
    clientActiveArea.Height = (double) num > 0.0 ? num : 0.0f;
    clientActiveArea.Y = (float) y;
    this.m_clientActiveArea = clientActiveArea;
  }

  public void CutFromTop() => this.CutFromTop((double) this.ClientActiveArea.Bottom);

  internal void UpdateClientActiveArea(RectangleF rectangle) => this.m_clientActiveArea = rectangle;

  private void UpdateClientArea(IWidget widget)
  {
    double num1 = 0.0;
    double num2 = 0.0;
    double num3 = 0.0;
    double num4 = 0.0;
    if (this.m_spacings != null)
    {
      num1 = (double) this.Margins.Left + (double) this.Paddings.Left;
      num2 = (double) this.Margins.Top + (double) this.Paddings.Top;
      num3 = (double) this.Margins.Right + (double) this.Paddings.Right;
      num4 = widget is WTableCell ? (double) this.Margins.Bottom : (double) this.Paddings.Bottom;
    }
    if ((double) this.m_area.X < 0.0)
    {
      double x = (double) this.m_area.X;
    }
    WParagraph wparagraph;
    switch (widget)
    {
      case WParagraph _:
        wparagraph = widget as WParagraph;
        break;
      case SplitWidgetContainer _:
        if ((widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
        {
          wparagraph = (widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
          break;
        }
        goto default;
      default:
        wparagraph = (WParagraph) null;
        break;
    }
    if (wparagraph != null && this.m_spacings != null)
    {
      num1 = (double) this.Margins.Left;
      num3 = (double) this.Margins.Right;
    }
    double num5 = (double) this.m_area.Y;
    double num6 = (double) this.m_area.Width;
    double height = (double) this.m_area.Height;
    WTableCell wtableCell1;
    switch (widget)
    {
      case WTableCell _:
        wtableCell1 = widget as WTableCell;
        break;
      case SplitWidgetContainer _:
        if ((widget as SplitWidgetContainer).RealWidgetContainer is WTableCell)
        {
          wtableCell1 = (widget as SplitWidgetContainer).RealWidgetContainer as WTableCell;
          break;
        }
        goto default;
      default:
        wtableCell1 = (WTableCell) null;
        break;
    }
    WTableCell wtableCell2 = wtableCell1;
    double num7;
    double num8;
    double num9;
    if (wtableCell2 != null && widget.LayoutInfo.IsVerticalText)
    {
      num6 = (double) this.m_area.Height;
      num7 = (double) this.m_area.Width;
      if (wtableCell2.CellFormat.TextDirection == TextDirection.VerticalTopToBottom)
      {
        num8 = (double) this.m_area.X + num2;
        num9 = (double) this.m_area.Height - num2 - num4;
        if (wtableCell2.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || (widget.LayoutInfo as CellLayoutInfo).VerticalAlignment != VerticalAlignment.Middle)
        {
          num5 = (double) this.m_area.Y + num3;
          num7 = (double) this.m_area.Width - num3;
        }
      }
      else
      {
        num8 = (double) this.m_area.X + num4;
        num9 = (double) this.m_area.Height - num2 - num4;
        if (wtableCell2.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || (widget.LayoutInfo as CellLayoutInfo).VerticalAlignment != VerticalAlignment.Middle)
        {
          num5 = (double) this.m_area.Y + num1;
          num7 = (double) this.m_area.Width - num1;
        }
      }
    }
    else
    {
      num8 = (double) this.m_area.X + num1;
      num5 = (double) this.m_area.Y + num2;
      num9 = (double) this.m_area.Width - num1 - num3;
      num7 = (double) this.m_area.Height - num2 - num4;
    }
    if (widget != null && widget.LayoutInfo is ParagraphLayoutInfo && !(widget.LayoutInfo as ParagraphLayoutInfo).IsFirstLine && this.m_spacings != null)
    {
      num5 -= (double) this.Margins.Top;
      num7 += (double) this.Margins.Top;
    }
    if (num9 < 0.0)
      num9 = 0.0;
    if (num7 < 0.0)
      num7 = 0.0;
    this.m_clientArea = new RectangleF((float) Math.Round(num8, 2), (float) Math.Round(num5, 2), (float) Math.Round(num9, 2), (float) Math.Round(num7, 2));
    this.m_clientActiveArea = this.m_clientArea;
    if (wtableCell2 == null)
      return;
    (widget.LayoutInfo as CellLayoutInfo).CellContentLayoutingBounds = this.m_clientArea;
  }

  internal void UpdateBounds(float topPad)
  {
    float num = Math.Abs(topPad - (this.m_spacings != null ? this.Margins.Top + this.Paddings.Top : 0.0f));
    this.m_clientArea.Y += num;
    this.m_clientArea.Height -= num;
    this.m_clientActiveArea = this.m_clientArea;
  }

  internal void UpdateBoundsBasedOnTextWrap(float bottom)
  {
    float num = bottom - this.m_clientArea.Y;
    this.m_clientArea.Y = bottom;
    this.m_clientArea.Height -= num;
    this.m_clientActiveArea = this.m_clientArea;
  }

  internal void UpdateWidth(float previousTabPosition)
  {
    this.m_clientArea.Width = (double) previousTabPosition != 0.0 ? (float) Math.Round(1584.0 - (double) previousTabPosition) : (float) (1584.0 - ((double) this.m_clientActiveArea.X - (double) this.m_area.X));
    this.m_clientActiveArea.Width = this.m_clientArea.Width;
  }

  internal void UpdateLeftPosition(float x)
  {
    RectangleF clientActiveArea = this.m_clientActiveArea;
    clientActiveArea.Width += clientActiveArea.X - x;
    clientActiveArea.X = x;
    this.m_clientActiveArea = clientActiveArea;
  }
}
