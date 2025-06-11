// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Office;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedWidget
{
  private RectangleF m_bounds = RectangleF.Empty;
  private IWidget m_widget;
  private LayoutedWidgetList m_ltWidgets = new LayoutedWidgetList();
  private string m_textTag;
  private float m_wordSpace;
  private HAlignment m_horizontalAlign;
  private float m_subWidth;
  private int m_spaces;
  private TabJustification m_prevTabJustification;
  private LayoutedWidget m_owner;
  private byte m_bFlags;
  internal float m_footnoteHeight;
  internal float m_endnoteHeight;
  internal List<RectangleF> m_intersectingBounds;
  private bool m_isTrackChanges;
  private CharacterRangeType m_charRangeType;

  internal TabJustification PrevTabJustification
  {
    get => this.m_prevTabJustification;
    set => this.m_prevTabJustification = value;
  }

  internal bool IsTrackChanges
  {
    get => this.m_isTrackChanges;
    set => this.m_isTrackChanges = value;
  }

  public int Spaces
  {
    get => this.m_spaces;
    set => this.m_spaces = value;
  }

  public bool IsLastLine
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  public float SubWidth
  {
    get => this.m_subWidth;
    set => this.m_subWidth = value;
  }

  public HAlignment HorizontalAlign
  {
    get => this.m_horizontalAlign;
    set => this.m_horizontalAlign = value;
  }

  public float WordSpace
  {
    get => this.m_wordSpace;
    set
    {
      if (this.IsNotWord2013())
        this.m_wordSpace = Math.Abs(Convert.ToSingle(value));
      else
        this.m_wordSpace = value;
    }
  }

  public string TextTag
  {
    get => this.m_textTag;
    set => this.m_textTag = value;
  }

  public RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  internal IWidget Widget => this.m_widget;

  public LayoutedWidgetList ChildWidgets
  {
    get => this.m_ltWidgets;
    set => this.m_ltWidgets = value;
  }

  internal CharacterRangeType CharacterRange
  {
    get => this.m_charRangeType;
    set => this.m_charRangeType = value;
  }

  public LayoutedWidget Owner
  {
    get => this.m_owner;
    set => this.m_owner = value;
  }

  internal bool IsLastItemInPage
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsNotFitted
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsContainsSpaceCharAtEnd
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal List<RectangleF> IntersectingBounds
  {
    get
    {
      if (this.m_intersectingBounds == null)
        this.m_intersectingBounds = new List<RectangleF>();
      return this.m_intersectingBounds;
    }
  }

  public LayoutedWidget(IWidget widget) => this.m_widget = widget;

  public LayoutedWidget()
  {
  }

  public LayoutedWidget(IWidget widget, PointF location)
  {
    this.m_widget = widget;
    this.m_bounds = new RectangleF(location, new SizeF());
  }

  public LayoutedWidget(LayoutedWidget ltWidget)
  {
    this.Bounds = ltWidget.Bounds;
    for (int index = 0; index < ltWidget.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = ltWidget.ChildWidgets[index];
      if (childWidget is LayoutedMathWidget)
        this.ChildWidgets.Add((LayoutedWidget) new LayoutedMathWidget(childWidget));
      else
        this.ChildWidgets.Add(new LayoutedWidget(childWidget));
    }
    for (int index = 0; index < ltWidget.IntersectingBounds.Count; ++index)
      this.IntersectingBounds.Add(ltWidget.IntersectingBounds[index]);
    this.HorizontalAlign = ltWidget.HorizontalAlign;
    this.IsLastItemInPage = ltWidget.IsLastItemInPage;
    this.IsLastLine = ltWidget.IsLastLine;
    this.IsNotFitted = ltWidget.IsNotFitted;
    this.Owner = ltWidget.Owner;
    this.PrevTabJustification = ltWidget.PrevTabJustification;
    this.Spaces = ltWidget.Spaces;
    this.SubWidth = ltWidget.SubWidth;
    this.TextTag = ltWidget.TextTag;
    this.m_widget = ltWidget.Widget;
    this.WordSpace = ltWidget.WordSpace;
    this.m_isTrackChanges = ltWidget.m_isTrackChanges;
    this.m_charRangeType = ltWidget.m_charRangeType;
  }

  internal void GetFootnoteHeight(ref float height)
  {
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.ChildWidgets)
    {
      if (childWidget.Widget is WTableRow)
        this.GetFootnoteHeightForTableRow(ref height, childWidget);
      else if (childWidget.Widget is WFootnote)
        height += ((FootnoteLayoutInfo) childWidget.Widget.LayoutInfo).FootnoteHeight;
      else if (childWidget.Widget is IWidgetContainer)
        childWidget.GetFootnoteHeight(ref height);
    }
  }

  internal void GetFootnoteHeightForTableRow(ref float height, LayoutedWidget row)
  {
    foreach (LayoutedWidget childWidget1 in (List<LayoutedWidget>) row.ChildWidgets)
    {
      LayoutedWidget paragraphWidgets = this.GetChildParagraphWidgets(childWidget1);
      if (paragraphWidgets != null)
      {
        foreach (LayoutedWidget childWidget2 in (List<LayoutedWidget>) paragraphWidgets.ChildWidgets)
        {
          foreach (LayoutedWidget childWidget3 in (List<LayoutedWidget>) childWidget2.ChildWidgets)
          {
            if (childWidget3.Widget is WFootnote)
              height += ((FootnoteLayoutInfo) childWidget3.Widget.LayoutInfo).FootnoteHeight;
            else if (childWidget3.Widget is IWidgetContainer)
              childWidget3.GetFootnoteHeight(ref height);
          }
        }
      }
    }
  }

  private LayoutedWidget GetChildParagraphWidgets(LayoutedWidget layoutedWidget)
  {
    if (layoutedWidget.ChildWidgets != null && layoutedWidget.ChildWidgets.Count > 0)
    {
      if (!this.IsTableWidget(layoutedWidget.ChildWidgets[0].Widget))
        return this.GetChildParagraphWidgets(layoutedWidget.ChildWidgets[0]);
      if (layoutedWidget.ChildWidgets[0].Widget is WParagraph || layoutedWidget.ChildWidgets[0].Widget is SplitWidgetContainer && (layoutedWidget.ChildWidgets[0].Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
        return layoutedWidget;
    }
    return (LayoutedWidget) null;
  }

  private bool IsTableWidget(IWidget widget)
  {
    return !(widget is WTableCell) && !(widget is WTableRow) && !(widget is WTable) && (!(widget is SplitWidgetContainer) || !((widget as SplitWidgetContainer).RealWidgetContainer is WTableCell)) && (!(widget is SplitWidgetContainer) || !((widget as SplitWidgetContainer).RealWidgetContainer is WTableRow)) && (!(widget is SplitWidgetContainer) || !((widget as SplitWidgetContainer).RealWidgetContainer is WTable));
  }

  internal RectangleF GetFrameClipBounds(
    RectangleF bounds,
    WParagraphFormat paragraphFormat,
    ParagraphLayoutInfo paraLayoutInfo)
  {
    float distanceFromText = paragraphFormat.FrameHorizontalDistanceFromText;
    float xposition = paraLayoutInfo.XPosition;
    float num = (double) paragraphFormat.FrameWidth != 0.0 ? paragraphFormat.FrameWidth - (paragraphFormat.LeftIndent + paragraphFormat.RightIndent) : bounds.Width;
    if ((double) paragraphFormat.FirstLineIndent < 0.0)
      num -= paragraphFormat.FirstLineIndent;
    float left = paraLayoutInfo.Paddings.Left;
    float right = paraLayoutInfo.Paddings.Right;
    float top = paraLayoutInfo.Paddings.Top;
    float bottom = paraLayoutInfo.Paddings.Bottom;
    Borders borders = paragraphFormat.Borders;
    return new RectangleF((float) ((double) xposition + (double) left - (double) distanceFromText - 1.5 - (double) borders.Left.LineWidth / 2.0), (float) ((double) bounds.Y - (double) top - (double) borders.Top.LineWidth / 2.0), (float) ((double) num + (double) distanceFromText - ((double) left + (double) right) + (double) borders.Left.LineWidth / 2.0 + (double) borders.Right.LineWidth / 2.0 + 3.0), (float) ((double) bounds.Height + ((double) top + (double) bottom) + (double) borders.Top.LineWidth / 2.0 + (double) borders.Bottom.LineWidth / 2.0));
  }

  internal bool IsBehindWidget()
  {
    return (this.Widget is WPicture || this.Widget is WTextBox || this.Widget is WChart || this.Widget is Shape || this.Widget is GroupShape) && ((ParagraphItem) this.Widget).GetTextWrappingStyle() == TextWrappingStyle.Behind;
  }

  public void InitLayoutInfo(bool resetTabLayoutInfo)
  {
    this.m_widget.InitLayoutInfo();
    int index = 0;
    for (int count = this.m_ltWidgets.Count; index < count; ++index)
    {
      LayoutedWidget ltWidget = this.m_ltWidgets[index];
      if (ltWidget is LayoutedMathWidget)
      {
        ltWidget.Widget.InitLayoutInfo();
        (ltWidget as LayoutedMathWidget).Dispose();
      }
      else if (ltWidget != null && (resetTabLayoutInfo || !(ltWidget.Widget is WTextRange) || !(((WidgetBase) ltWidget.Widget).m_layoutInfo is TabsLayoutInfo)))
        ltWidget.InitLayoutInfo(resetTabLayoutInfo);
    }
  }

  public void InitLayoutInfoForTextWrapElements()
  {
    if (this.m_widget is WPicture)
      (this.m_widget as WPicture).m_layoutInfo = (ILayoutInfo) null;
    else if (this.m_widget is WTextBox)
      (this.m_widget as WTextBox).m_layoutInfo = (ILayoutInfo) null;
    else if (this.m_widget is Shape)
      (this.m_widget as Shape).m_layoutInfo = (ILayoutInfo) null;
    else if (this.m_widget is GroupShape)
    {
      (this.m_widget as GroupShape).m_layoutInfo = (ILayoutInfo) null;
    }
    else
    {
      if (!(this.m_widget is WChart))
        return;
      (this.m_widget as WChart).m_layoutInfo = (ILayoutInfo) null;
    }
  }

  public void InitLayoutInfoAll()
  {
    if (this.m_widget == null)
      return;
    if (this.m_widget is SplitStringWidget)
      (this.m_widget as SplitStringWidget).RealStringWidget.InitLayoutInfo();
    else if (this.m_widget is SplitWidgetContainer)
      (this.m_widget as SplitWidgetContainer).RealWidgetContainer.InitLayoutInfo();
    else if (this.m_widget is SplitTableWidget)
      (this.m_widget as SplitTableWidget).TableWidget.InitLayoutInfo();
    else
      this.m_widget.InitLayoutInfo();
    if (this.m_widget != null)
    {
      int index1 = 0;
      for (int count = this.m_ltWidgets.Count; index1 < count; ++index1)
      {
        LayoutedWidget ltWidget = this.m_ltWidgets[index1];
        WParagraph wparagraph = ltWidget.Widget is SplitWidgetContainer ? (ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph : ltWidget.Widget as WParagraph;
        if (wparagraph != null)
        {
          ltWidget.InitLayoutInfo(true);
          for (int index2 = 0; index2 < wparagraph.ChildEntities.Count; ++index2)
          {
            if (wparagraph.ChildEntities[index2] is IWidget childEntity)
            {
              childEntity.InitLayoutInfo();
              if (childEntity is InlineContentControl)
              {
                InlineContentControl inlineContentControl = childEntity as InlineContentControl;
                for (int index3 = 0; index3 < inlineContentControl.ParagraphItems.Count; ++index3)
                  ((IWidget) inlineContentControl.ParagraphItems[index3]).InitLayoutInfo();
              }
            }
            else if (wparagraph.ChildEntities.InnerList[index2] is SplitStringWidget)
            {
              wparagraph.ChildEntities.InnerList.RemoveAt(index2);
              --index2;
            }
          }
        }
        else
          ltWidget.InitLayoutInfoAll();
      }
    }
    this.m_widget = (IWidget) null;
    this.m_owner = (LayoutedWidget) null;
    this.TextTag = (string) null;
    this.m_ltWidgets.Clear();
    this.m_ltWidgets = (LayoutedWidgetList) null;
  }

  public void ShiftLocation(
    double xOffset,
    double yOffset,
    bool isPictureNeedToBeShifted,
    bool isFromFloatingItemVerticalAlignment)
  {
    this.ShiftLocation(xOffset, yOffset, isPictureNeedToBeShifted, isFromFloatingItemVerticalAlignment, true);
  }

  public void ShiftLocation(
    double xOffset,
    double yOffset,
    bool isPictureNeedToBeShifted,
    bool isFromFloatingItemVerticalAlignment,
    bool isNeedToShiftOwnerWidget)
  {
    if (isNeedToShiftOwnerWidget)
    {
      this.m_bounds = new RectangleF(new PointF(this.m_bounds.X + (float) xOffset, this.m_bounds.Y + (float) yOffset), this.m_bounds.Size);
      if (this.m_widget is WTextBox || this.m_widget is Shape || this.m_widget is ChildShape)
      {
        RectangleF rectangleF = this.m_widget is WTextBox ? (this.m_widget as WTextBox).TextLayoutingBounds : (this.m_widget is Shape ? (this.m_widget as Shape).TextLayoutingBounds : (this.m_widget as ChildShape).TextLayoutingBounds);
        rectangleF = new RectangleF(rectangleF.X + (float) xOffset, rectangleF.Y + (float) yOffset, rectangleF.Width, rectangleF.Height);
        if (this.m_widget is WTextBox)
        {
          WTextBox widget = this.m_widget as WTextBox;
          widget.TextLayoutingBounds = rectangleF;
          if (widget.TextBoxFormat.VMLPathPoints != null && widget.TextBoxFormat.VMLPathPoints.Count > 0)
            widget.ReUpdateVMLPathPoints((float) xOffset, (float) yOffset, widget.TextBoxFormat.VMLPathPoints);
        }
        else if (this.m_widget is Shape)
        {
          Shape widget = this.m_widget as Shape;
          widget.TextLayoutingBounds = rectangleF;
          if (widget.VMLPathPoints != null && widget.VMLPathPoints.Count > 0)
            widget.ReUpdateVMLPathPoints((float) xOffset, (float) yOffset, widget.VMLPathPoints);
        }
        else
        {
          ChildShape widget = this.m_widget as ChildShape;
          widget.TextLayoutingBounds = rectangleF;
          if (widget.VMLPathPoints != null && widget.VMLPathPoints.Count > 0)
            widget.ReUpdateVMLPathPoints((float) xOffset, (float) yOffset, widget.VMLPathPoints);
        }
      }
    }
    if (this.m_widget is WMath)
    {
      (this as LayoutedMathWidget).ShiftXYPosition((float) xOffset, (float) yOffset, isNeedToShiftOwnerWidget);
    }
    else
    {
      for (int index1 = 0; index1 < this.ChildWidgets.Count; ++index1)
      {
        LayoutedWidget childWidget = this.ChildWidgets[index1];
        if (childWidget != null)
        {
          if (childWidget.Widget is WTable || childWidget.Widget is WTextBox)
          {
            if (childWidget.Widget is WTable)
            {
              WTable widget = childWidget.Widget as WTable;
              if (widget.TableFormat.WrapTextAround)
              {
                if (!this.IsShiftAbsTableBasedOnPageBottom(childWidget, xOffset) && widget.TableFormat.Positioning.VertRelationTo == VerticalRelation.Paragraph && isPictureNeedToBeShifted)
                {
                  childWidget.ShiftLocation(0.0, yOffset, isPictureNeedToBeShifted, isFromFloatingItemVerticalAlignment);
                  continue;
                }
                continue;
              }
            }
            else
            {
              WTextBox widget = childWidget.Widget as WTextBox;
              double xOffset1 = 0.0;
              if (widget.IsFloatingItem(false))
              {
                if ((childWidget.Widget as ParagraphItem).GetHorizontalOrigin() == HorizontalOrigin.Character)
                  xOffset1 = xOffset;
                if (!this.IsShiftAbsTableBasedOnPageBottom(childWidget, xOffset) && (widget.TextBoxFormat.VerticalOrigin == VerticalOrigin.Line || widget.TextBoxFormat.VerticalOrigin == VerticalOrigin.Paragraph) && isPictureNeedToBeShifted)
                {
                  childWidget.ShiftLocation(xOffset1, yOffset, isPictureNeedToBeShifted, isFromFloatingItemVerticalAlignment);
                  continue;
                }
                if (isFromFloatingItemVerticalAlignment && this.IsFloatingItemNeedToBeAlign(childWidget.Widget))
                {
                  childWidget.ShiftLocation(0.0, yOffset, isPictureNeedToBeShifted, isFromFloatingItemVerticalAlignment);
                  continue;
                }
                if (xOffset1 > 0.0)
                {
                  childWidget.ShiftLocation(xOffset1, 0.0, isPictureNeedToBeShifted, isFromFloatingItemVerticalAlignment);
                  continue;
                }
                continue;
              }
            }
          }
          if (childWidget.Widget is WPicture || childWidget.Widget is Shape || childWidget.Widget is WChart || childWidget.Widget is GroupShape)
          {
            TextWrappingStyle textWrappingStyle = (childWidget.Widget as ParagraphItem).GetTextWrappingStyle();
            VerticalOrigin verticalOrigin = (childWidget.Widget as ParagraphItem).GetVerticalOrigin();
            double xOffset2 = 0.0;
            if (textWrappingStyle != TextWrappingStyle.Inline)
            {
              if ((childWidget.Widget as ParagraphItem).GetHorizontalOrigin() == HorizontalOrigin.Character)
                xOffset2 = xOffset;
              if ((verticalOrigin == VerticalOrigin.Paragraph || verticalOrigin == VerticalOrigin.Line) && isPictureNeedToBeShifted)
              {
                childWidget.ShiftLocation(xOffset2, yOffset, isPictureNeedToBeShifted, isFromFloatingItemVerticalAlignment);
                continue;
              }
              if (isFromFloatingItemVerticalAlignment && this.IsFloatingItemNeedToBeAlign(childWidget.Widget))
              {
                childWidget.ShiftLocation(0.0, yOffset, isPictureNeedToBeShifted, isFromFloatingItemVerticalAlignment);
                continue;
              }
              if (xOffset2 > 0.0)
              {
                childWidget.ShiftLocation(xOffset2, 0.0, isPictureNeedToBeShifted, isFromFloatingItemVerticalAlignment);
                continue;
              }
              continue;
            }
          }
          ParagraphLayoutInfo layoutInfo = childWidget.Widget.LayoutInfo as ParagraphLayoutInfo;
          if (childWidget.Widget is WParagraph && childWidget.ChildWidgets.Count > 0 && childWidget.ChildWidgets[0].Widget == childWidget.Widget && layoutInfo != null && layoutInfo.ListValue != string.Empty && layoutInfo.ListYPositions.Count >= 1)
          {
            List<float> listYpositions;
            int index2;
            (listYpositions = layoutInfo.ListYPositions)[index2 = layoutInfo.ListYPositions.Count - 1] = listYpositions[index2] + (float) yOffset;
          }
          childWidget.ShiftLocation(xOffset, yOffset, isPictureNeedToBeShifted, isFromFloatingItemVerticalAlignment);
        }
      }
    }
  }

  private bool IsFloatingItemNeedToBeAlign(IWidget widget)
  {
    bool layoutInCell = false;
    switch (widget)
    {
      case WPicture _:
        layoutInCell = (widget as WPicture).LayoutInCell;
        break;
      case Shape _:
        layoutInCell = (widget as Shape).LayoutInCell;
        break;
      case WTextBox _:
        layoutInCell = (widget as WTextBox).TextBoxFormat.AllowInCell;
        break;
      case WChart _:
        layoutInCell = (widget as WChart).LayoutInCell;
        break;
      case GroupShape _:
        layoutInCell = (widget as GroupShape).LayoutInCell;
        break;
    }
    return this.CompatibilityCheck(((Syncfusion.DocIO.DLS.OwnerHolder) widget).Document.Settings.CompatibilityMode, ((Syncfusion.DocIO.DLS.OwnerHolder) widget).Document.DOP.Dop2000.Copts.DontVertAlignCellWithSp, layoutInCell);
  }

  private bool CompatibilityCheck(
    CompatibilityMode compatibilityMode,
    bool dontVertAlignCellWithSp,
    bool layoutInCell)
  {
    if (compatibilityMode == CompatibilityMode.Word2013 || compatibilityMode == CompatibilityMode.Word2003 && !dontVertAlignCellWithSp)
      return true;
    return compatibilityMode != CompatibilityMode.Word2003 && layoutInCell;
  }

  public void ShiftLocation(double xOffset, double yOffset, float footerHeight, float pageHeight)
  {
    this.ShiftLocation(xOffset, yOffset, footerHeight, (float) yOffset, pageHeight);
  }

  public void ShiftLocation(
    double xOffset,
    double yOffset,
    float footerHeight,
    float originalDistance,
    float pageHeight)
  {
    this.m_bounds = new RectangleF(new PointF(this.m_bounds.X + (float) xOffset, this.m_bounds.Y + (float) yOffset), this.m_bounds.Size);
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.ChildWidgets)
    {
      if (childWidget != null)
      {
        if (childWidget.Widget is WTable || childWidget.Widget is WTextBox)
        {
          if (childWidget.Widget is WTable)
          {
            WTable widget = childWidget.Widget as WTable;
            if (widget.TableFormat.WrapTextAround && !widget.IsInCell)
            {
              if (!this.IsShiftAbsTableBasedOnPageBottom(childWidget, xOffset, footerHeight) && widget.TableFormat.Positioning.VertRelationTo == VerticalRelation.Paragraph)
              {
                childWidget.ShiftLocation(0.0, yOffset, true, false);
                continue;
              }
              continue;
            }
          }
          else
          {
            WTextBox widget = childWidget.Widget as WTextBox;
            if (widget.IsFloatingItem(false))
            {
              if (!this.IsShiftAbsTableBasedOnPageBottom(childWidget, xOffset) && (widget.TextBoxFormat.VerticalOrigin == VerticalOrigin.Line || widget.TextBoxFormat.VerticalOrigin == VerticalOrigin.Paragraph))
              {
                childWidget.ShiftLocation(0.0, yOffset, true, false);
                continue;
              }
              if (widget.TextBoxFormat.VerticalOrigin == VerticalOrigin.BottomMargin)
              {
                switch (widget.TextBoxFormat.VerticalAlignment)
                {
                  case ShapeVerticalAlignment.Top:
                  case ShapeVerticalAlignment.Inside:
                    childWidget.Bounds = new RectangleF(childWidget.Bounds.X, pageHeight - footerHeight, childWidget.Bounds.Width, childWidget.Bounds.Height);
                    continue;
                  case ShapeVerticalAlignment.Center:
                    childWidget.Bounds = new RectangleF(childWidget.Bounds.X, (float) ((double) pageHeight - (double) footerHeight + ((double) footerHeight - (double) childWidget.Bounds.Height) / 2.0), childWidget.Bounds.Width, childWidget.Bounds.Height);
                    continue;
                  case ShapeVerticalAlignment.Bottom:
                  case ShapeVerticalAlignment.Outside:
                    childWidget.Bounds = new RectangleF(childWidget.Bounds.X, pageHeight - childWidget.Bounds.Height, childWidget.Bounds.Width, childWidget.Bounds.Height);
                    continue;
                  default:
                    continue;
                }
              }
              else
                continue;
            }
          }
        }
        if (childWidget.Widget is WPicture || childWidget.Widget is Shape || childWidget.Widget is WChart || childWidget.Widget is GroupShape)
        {
          CompatibilityMode compatibilityMode = ((Syncfusion.DocIO.DLS.OwnerHolder) childWidget.Widget).Document.Settings.CompatibilityMode;
          TextWrappingStyle textWrappingStyle = ((ParagraphItem) childWidget.Widget).GetTextWrappingStyle();
          float verticalPosition = ((ParagraphItem) childWidget.Widget).GetVerticalPosition();
          VerticalOrigin verticalOrigin = ((ParagraphItem) childWidget.Widget).GetVerticalOrigin();
          ShapeVerticalAlignment verticalAlignment = ((ParagraphItem) childWidget.Widget).GetShapeVerticalAlignment();
          if (textWrappingStyle != TextWrappingStyle.Inline)
          {
            switch (verticalOrigin)
            {
              case VerticalOrigin.Paragraph:
              case VerticalOrigin.Line:
                childWidget.ShiftLocation(0.0, yOffset, true, false);
                break;
              case VerticalOrigin.BottomMargin:
                switch (verticalAlignment)
                {
                  case ShapeVerticalAlignment.None:
                    childWidget.Bounds = new RectangleF(childWidget.Bounds.X, pageHeight - footerHeight + verticalPosition, childWidget.Bounds.Width, childWidget.Bounds.Height);
                    break;
                  case ShapeVerticalAlignment.Top:
                  case ShapeVerticalAlignment.Inside:
                    childWidget.Bounds = new RectangleF(childWidget.Bounds.X, pageHeight - footerHeight, childWidget.Bounds.Width, childWidget.Bounds.Height);
                    break;
                  case ShapeVerticalAlignment.Center:
                    childWidget.Bounds = new RectangleF(childWidget.Bounds.X, (float) ((double) pageHeight - (double) footerHeight + ((double) footerHeight - (double) childWidget.Bounds.Height) / 2.0), childWidget.Bounds.Width, childWidget.Bounds.Height);
                    break;
                  case ShapeVerticalAlignment.Bottom:
                  case ShapeVerticalAlignment.Outside:
                    childWidget.Bounds = new RectangleF(childWidget.Bounds.X, pageHeight - childWidget.Bounds.Height, childWidget.Bounds.Width, childWidget.Bounds.Height);
                    break;
                }
                break;
            }
            if ((textWrappingStyle == TextWrappingStyle.Square || textWrappingStyle == TextWrappingStyle.Through || textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.TopAndBottom) && verticalOrigin == VerticalOrigin.Paragraph && compatibilityMode != CompatibilityMode.Word2013)
            {
              float num = pageHeight;
              if (compatibilityMode == CompatibilityMode.Word2003)
                num += originalDistance;
              if ((double) num < (double) childWidget.Bounds.Bottom)
              {
                childWidget.Bounds = new RectangleF(childWidget.Bounds.X, childWidget.Bounds.Y - (childWidget.Bounds.Bottom - num), childWidget.Bounds.Width, childWidget.Bounds.Height);
                continue;
              }
              continue;
            }
            continue;
          }
        }
        ParagraphLayoutInfo layoutInfo = childWidget.Widget.LayoutInfo as ParagraphLayoutInfo;
        if (childWidget.Widget is WParagraph)
        {
          WParagraph widget = childWidget.Widget as WParagraph;
          WParagraphFormat paragraphFormat = widget.ParagraphFormat;
          if (!paragraphFormat.IsFrame || widget.OwnerTextBody.Owner is WTextBox || widget.OwnerTextBody.Owner is Shape || widget.IsInCell || paragraphFormat.FrameVerticalAnchor == (byte) 2)
          {
            if (childWidget.ChildWidgets.Count > 0 && childWidget.ChildWidgets[0].Widget == childWidget.Widget && layoutInfo != null && layoutInfo.ListValue != string.Empty && layoutInfo.ListYPositions.Count >= 1)
            {
              List<float> listYpositions;
              int index;
              (listYpositions = layoutInfo.ListYPositions)[index = layoutInfo.ListYPositions.Count - 1] = listYpositions[index] + (float) yOffset;
            }
          }
          else
            continue;
        }
        childWidget.ShiftLocation(xOffset, yOffset, footerHeight, originalDistance, pageHeight);
      }
    }
  }

  private bool IsShiftAbsTableBasedOnPageBottom(LayoutedWidget ltWidget, double xOffset)
  {
    bool flag = false;
    Entity baseEntity = this.GetBaseEntity(ltWidget.Widget as Entity);
    if (baseEntity.Owner != null && baseEntity is HeaderFooter && ((baseEntity as HeaderFooter).Type == HeaderFooterType.OddFooter || (baseEntity as HeaderFooter).Type == HeaderFooterType.FirstPageFooter || (baseEntity as HeaderFooter).Type == HeaderFooterType.EvenFooter))
    {
      float height = (baseEntity.Owner as WSection).PageSetup.PageSize.Height;
      if ((double) height <= (double) ltWidget.Bounds.Bottom && baseEntity.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && (ltWidget.Widget is WTable && (ltWidget.Widget as WTable).TableFormat.Positioning.VertRelationTo == VerticalRelation.Paragraph || ltWidget.Widget is WTextBox && ((ltWidget.Widget as WTextBox).TextBoxFormat.VerticalOrigin == VerticalOrigin.Line || (ltWidget.Widget as WTextBox).TextBoxFormat.VerticalOrigin == VerticalOrigin.Paragraph)))
      {
        flag = true;
        float num = ltWidget.Bounds.Y - height + ltWidget.Bounds.Height;
        ltWidget.ShiftLocation(xOffset, -(double) num, true, false);
      }
      if (baseEntity.Document.Settings.CompatibilityMode != CompatibilityMode.Word2003 && flag)
        return true;
    }
    return false;
  }

  private bool IsShiftAbsTableBasedOnPageBottom(
    LayoutedWidget ltWidget,
    double xOffset,
    float footerHeight)
  {
    bool flag = false;
    Entity baseEntity = this.GetBaseEntity(ltWidget.Widget as Entity);
    if (baseEntity.Owner != null && baseEntity is HeaderFooter && ((baseEntity as HeaderFooter).Type == HeaderFooterType.OddFooter || (baseEntity as HeaderFooter).Type == HeaderFooterType.FirstPageFooter || (baseEntity as HeaderFooter).Type == HeaderFooterType.EvenFooter))
    {
      float height = (baseEntity.Owner as WSection).PageSetup.PageSize.Height;
      if ((double) height <= (double) ltWidget.Bounds.Bottom && baseEntity.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && (baseEntity.Document.Settings.CompatibilityMode != CompatibilityMode.Word2010 && baseEntity.Document.Settings.CompatibilityMode != CompatibilityMode.Word2007 || (double) ltWidget.Bounds.Height >= (double) footerHeight) && (ltWidget.Widget is WTable && (ltWidget.Widget as WTable).TableFormat.Positioning.VertRelationTo == VerticalRelation.Paragraph || ltWidget.Widget is WTextBox && ((ltWidget.Widget as WTextBox).TextBoxFormat.VerticalOrigin == VerticalOrigin.Line || (ltWidget.Widget as WTextBox).TextBoxFormat.VerticalOrigin == VerticalOrigin.Paragraph)))
      {
        flag = true;
        float num = ltWidget.Bounds.Y - height + ltWidget.Bounds.Height;
        ltWidget.ShiftLocation(xOffset, -(double) num, true, false);
      }
      if (baseEntity.Document.Settings.CompatibilityMode != CompatibilityMode.Word2003 && flag)
        return true;
    }
    return false;
  }

  internal void ShiftLocationOfCommentsMarkups(
    float xOffset,
    float yOffset,
    List<TrackChangesMarkups> trackChangesMarkups)
  {
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.ChildWidgets)
    {
      if (childWidget != null)
      {
        if (childWidget.Widget is WCommentMark && ((childWidget.Widget as WCommentMark).Type == CommentMarkType.CommentEnd || (childWidget.Widget as WCommentMark).Type == CommentMarkType.CommentStart && (childWidget.Widget as WCommentMark).Comment != null && (childWidget.Widget as WCommentMark).Comment.CommentRangeEnd == null))
        {
          for (int index = trackChangesMarkups.Count - 1; index >= 0; --index)
          {
            if (trackChangesMarkups[index] is CommentsMarkups trackChangesMarkup && trackChangesMarkup.CommentID == (childWidget.Widget as WCommentMark).CommentId)
            {
              trackChangesMarkup.Position = new PointF(trackChangesMarkup.Position.X + xOffset, trackChangesMarkup.Position.Y + yOffset);
              break;
            }
          }
        }
        childWidget.ShiftLocationOfCommentsMarkups(xOffset, yOffset, trackChangesMarkups);
      }
    }
  }

  private float GetExtentWidth(Entity entity)
  {
    float extentWidth = 0.0f;
    ChildShapeCollection childShapeCollection = entity is GroupShape ? (entity as GroupShape).ChildShapes : (entity as ChildGroupShape).ChildShapes;
    float num1 = entity is GroupShape ? (entity as GroupShape).CoordinateXOrigin : (entity as ChildGroupShape).CoordinateXOrigin;
    foreach (ChildShape childShape1 in (Syncfusion.DocIO.DLS.CollectionImpl) childShapeCollection)
    {
      if (childShape1 != null)
      {
        ChildShape childShape2 = childShape1;
        float num2 = childShape2.LeftMargin - num1 + childShape2.Width;
        if ((double) extentWidth < (double) num2)
          extentWidth = num2;
      }
    }
    return extentWidth;
  }

  private float GetExtentHeight(Entity entity)
  {
    float extentHeight = 0.0f;
    ChildShapeCollection childShapeCollection = entity is GroupShape ? (entity as GroupShape).ChildShapes : (entity as ChildGroupShape).ChildShapes;
    float num1 = entity is GroupShape ? (entity as GroupShape).CoordinateYOrigin : (entity as ChildGroupShape).CoordinateYOrigin;
    foreach (ChildShape childShape1 in (Syncfusion.DocIO.DLS.CollectionImpl) childShapeCollection)
    {
      if (childShape1 != null)
      {
        ChildShape childShape2 = childShape1;
        float num2 = childShape2.TopMargin - num1 + childShape2.Height;
        if ((double) extentHeight < (double) num2)
          extentHeight = num2;
      }
    }
    return extentHeight;
  }

  internal void GetGroupShapeExtent(
    ref float extensionWidth,
    ref float extensionHeight,
    Entity entity,
    RectangleF groupShapeBounds)
  {
    bool flag = entity is GroupShape ? (entity as GroupShape).Is2007Shape : (entity as ChildGroupShape).Is2007Shape;
    float num1 = entity is GroupShape ? (entity as GroupShape).Width : groupShapeBounds.Width;
    float num2 = entity is GroupShape ? (entity as GroupShape).Height : groupShapeBounds.Height;
    float num3 = flag ? this.GetExtentWidth(entity) : (entity is GroupShape ? (entity as GroupShape).ExtentXValue : (entity as ChildGroupShape).ExtentXValue);
    float num4 = flag ? this.GetExtentHeight(entity) : (entity is GroupShape ? (entity as GroupShape).ExtentYValue : (entity as ChildGroupShape).ExtentYValue);
    extensionWidth = (float) Math.Round(((double) num1 > 0.0 ? (double) num1 : 1.0) / (double) num3, 4);
    extensionHeight = (float) Math.Round(((double) num2 > 0.0 ? (double) num2 : 1.0) / (double) num4, 4);
    if ((double) extensionWidth == 0.0 || float.IsNaN(extensionWidth) || float.IsInfinity(extensionWidth))
      extensionWidth = 1f;
    if ((double) extensionHeight != 0.0 && !float.IsNaN(extensionHeight) && !float.IsInfinity(extensionHeight))
      return;
    extensionHeight = 1f;
  }

  private Entity GetBaseEntity(Entity entity)
  {
    Entity baseEntity = entity;
    while (baseEntity.Owner != null && !(baseEntity is WTextBox))
    {
      baseEntity = baseEntity.Owner;
      if (baseEntity is WSection || baseEntity is HeaderFooter)
        return baseEntity;
    }
    return baseEntity;
  }

  public void UpdateLtWidgetBounds(float width, float height, float totalWidth, float totalHeight)
  {
    if ((double) this.m_bounds.Width > (double) width && (double) totalHeight == 0.0)
      this.m_bounds = new RectangleF(this.m_bounds.X, this.m_bounds.Y, width, this.m_bounds.Height);
    else if ((double) this.m_bounds.Height > (double) totalHeight && (double) totalWidth == 0.0)
      this.m_bounds = new RectangleF(this.m_bounds.X, this.m_bounds.Y, this.m_bounds.Width, height);
    for (int index1 = 0; index1 < this.ChildWidgets.Count; ++index1)
    {
      LayoutedWidget childWidget = this.ChildWidgets[index1];
      float num = 0.0f;
      for (int index2 = index1 - 1; index2 >= 0 && index1 != 0; --index2)
      {
        if ((this.ChildWidgets[index2] == null || this.IsLtWidgetBoundsNeedToUpdate(this.ChildWidgets[index2])) && this.ChildWidgets[index2] != null && this.ChildWidgets[index2].Widget is ParagraphItem)
          num += this.ChildWidgets[index2].Bounds.Width;
      }
      if (childWidget != null && this.IsLtWidgetBoundsNeedToUpdate(childWidget))
      {
        Entity entity = childWidget.Widget as Entity;
        while (true)
        {
          switch (entity)
          {
            case WTableCell _:
            case null:
            case WTable _:
              goto label_14;
            default:
              entity = entity.Owner;
              continue;
          }
        }
label_14:
        if (entity is WTableCell && !(childWidget.Widget is WTableCell))
        {
          if ((double) totalHeight == 0.0)
            width = totalWidth - (((CellLayoutInfo) ((IWidget) (entity as WTableCell)).LayoutInfo).Margins.Left + ((CellLayoutInfo) ((IWidget) (entity as WTableCell)).LayoutInfo).Paddings.Left) - num;
          else
            height = totalHeight - (((CellLayoutInfo) ((IWidget) (entity as WTableCell)).LayoutInfo).Margins.Top + ((CellLayoutInfo) ((IWidget) (entity as WTableCell)).LayoutInfo).Paddings.Top);
        }
        if ((double) width < 0.0)
          width = 0.0f;
        childWidget.UpdateLtWidgetBounds(width, height, totalWidth, totalHeight);
      }
    }
  }

  private bool IsLtWidgetBoundsNeedToUpdate(LayoutedWidget ltWidget)
  {
    return !(ltWidget.Widget is WPicture) && !(ltWidget.Widget is WChart) && !(ltWidget.Widget is Shape) && !(ltWidget.Widget is WTextBox) && !(ltWidget.Widget is GroupShape) || ((ParagraphItem) ltWidget.Widget).GetTextWrappingStyle() != TextWrappingStyle.InFrontOfText && ((ParagraphItem) ltWidget.Widget).GetTextWrappingStyle() != TextWrappingStyle.Behind;
  }

  public void AlignBottom(
    DrawingContext dc,
    float remClientAreaHeight,
    float clientAreaBottom,
    bool isRowFitInSamePage,
    bool isLayoutingHeaderRow,
    bool isLayoutingHeaderFooter,
    bool isForceFitLayout)
  {
    WParagraph paragraph = this.GetParagraph();
    bool isFirstLineOfParagraph = false;
    bool isLastLineOfParagraph = false;
    if (this.ChildWidgets.Count == 0 && (paragraph.m_layoutInfo.IsSkip || paragraph.IsNeedToSkip || paragraph.SectionEndMark && paragraph.PreviousSibling != null))
      return;
    if (this.ChildWidgets.Count != 0 && paragraph.IsEmptyParagraph() && paragraph.SectionEndMark && paragraph.PreviousSibling != null)
    {
      this.m_bounds = new RectangleF(this.m_bounds.X, this.m_bounds.Y, this.m_bounds.Width, 0.0f);
    }
    else
    {
      if (this.ChildWidgets.Count != 0)
      {
        isFirstLineOfParagraph = paragraph.IsFirstLine(this.ChildWidgets[0]);
        isLastLineOfParagraph = paragraph.IsLastLine(this.ChildWidgets[this.ChildWidgets.Count - 1]);
      }
      double maxHeight;
      double maxAscent;
      double maxTextHeight;
      double maxTextAscent;
      double maxTextDescent;
      float maxY;
      double maxAscentOfLoweredPos;
      bool isClippedLine;
      bool isTextInLine;
      bool containsInlinePicture;
      bool isAllWordsContainLoweredPos;
      bool maxChildWidget = this.CalculateMaxChildWidget(dc, paragraph, isFirstLineOfParagraph, isLastLineOfParagraph, out maxHeight, out maxAscent, out maxTextHeight, out maxTextAscent, out maxTextDescent, out maxY, out maxAscentOfLoweredPos, out IStringWidget _, out isClippedLine, out isTextInLine, out containsInlinePicture, out isAllWordsContainLoweredPos);
      bool flag = false;
      if (paragraph.Document.Settings.CompatibilityOptions[CompatibilityOption.SuppressTopSpacing] && isForceFitLayout && paragraph.Owner.Owner is WSection && (paragraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.AtLeast || paragraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.Exactly) && (this.Widget.LayoutInfo as ParagraphLayoutInfo).ListValue == string.Empty && !isLayoutingHeaderFooter)
        flag = true;
      if (maxAscent > maxHeight)
        maxAscent = maxHeight;
      if (maxTextAscent > maxAscent)
        maxTextAscent = maxAscent;
      if ((double) maxY != 3.4028234663852886E+38)
        this.m_bounds.Y = maxY;
      float num1 = maxHeight <= maxTextHeight || !isTextInLine || !containsInlinePicture ? 0.0f : (float) maxTextDescent;
      double topLineSpace = 0.0;
      double num2 = 0.0;
      float lineSpacing = paragraph.ParagraphFormat.LineSpacing;
      if (maxHeight != 0.0 || maxTextHeight != 0.0)
      {
        switch (paragraph.ParagraphFormat.LineSpacingRule)
        {
          case LineSpacingRule.AtLeast:
            if (maxHeight < (double) lineSpacing && (!isClippedLine || Math.Round(maxHeight, 2) != Math.Round((double) remClientAreaHeight, 2)))
            {
              topLineSpace = (double) lineSpacing - (maxHeight + (double) num1);
              maxHeight = (double) lineSpacing;
              break;
            }
            break;
          case LineSpacingRule.Exactly:
            float num3 = Math.Abs(lineSpacing) - (float) maxTextHeight;
            topLineSpace = (double) num3 * 80.0 / 100.0;
            num2 = (double) num3 * 20.0 / 100.0;
            maxHeight = (double) Math.Abs(lineSpacing);
            break;
          case LineSpacingRule.Multiple:
            if ((double) lineSpacing != 12.0)
            {
              if ((double) lineSpacing < 12.0)
              {
                topLineSpace = maxTextHeight * ((double) lineSpacing / 12.0) - maxTextHeight;
                maxHeight -= Math.Abs(topLineSpace);
                break;
              }
              num2 = maxTextHeight * ((double) lineSpacing / 12.0) - maxTextHeight;
              break;
            }
            break;
        }
      }
      float minValue = float.MinValue;
      bool isInlineDrawingObject = paragraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.Exactly && containsInlinePicture;
      if (!flag)
      {
        if ((isFirstLineOfParagraph || this.ChildWidgets.Count == 0 && this.IsNeedToUpdateListYPos()) && this.Widget.LayoutInfo is ParagraphLayoutInfo && (this.Widget.LayoutInfo as ParagraphLayoutInfo).ListValue != string.Empty)
        {
          if (!isLayoutingHeaderRow)
            ((ParagraphLayoutInfo) this.Widget.LayoutInfo).ListYPositions.Clear();
          this.ShiftListYPosition(paragraph, dc, maxAscent, topLineSpace, maxHeight, ref minValue, ref maxAscentOfLoweredPos, ref isAllWordsContainLoweredPos);
        }
        if (!maxChildWidget)
        {
          if (isInlineDrawingObject)
          {
            foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.ChildWidgets)
            {
              if (childWidget != null && !childWidget.Widget.LayoutInfo.IsSkipBottomAlign && (!(childWidget.Widget is WField) || (childWidget.Widget as WField).FieldType != FieldType.FieldHyperlink) && this.IsInlineFloatingItem(childWidget.Widget))
              {
                double yOffset = (double) lineSpacing * 80.0 / 100.0 - (double) childWidget.Bounds.Height;
                childWidget.ShiftLocation(0.0, yOffset, true, false);
              }
            }
            maxAscent = maxTextAscent;
            num1 = 0.0f;
          }
          this.ShiftLineWidgetYPosition(dc, maxAscent, topLineSpace, maxHeight, ref minValue, isInlineDrawingObject, ref maxAscentOfLoweredPos, ref isAllWordsContainLoweredPos);
        }
      }
      float rowHeight = 0.0f;
      if ((double) minValue != -3.4028234663852886E+38)
      {
        rowHeight = minValue;
        this.ShiftLocation(0.0, (double) minValue, true, false);
      }
      if (paragraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.Exactly || paragraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.AtLeast)
        num2 = 0.0;
      this.m_bounds = !flag ? new RectangleF(this.m_bounds.X, this.m_bounds.Y - rowHeight, this.m_bounds.Width, (float) maxHeight + (float) num2 + num1) : new RectangleF(this.m_bounds.X, this.m_bounds.Y, this.m_bounds.Width, this.m_bounds.Height);
      if (!isForceFitLayout && !paragraph.ParagraphFormat.IsInFrame() && paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 && Math.Round((double) this.m_bounds.Bottom, 2) > Math.Round((double) clientAreaBottom, 2) && paragraph.OwnerTextBody.OwnerBase is WSection && paragraph.ChildEntities.Count == 1 && paragraph.ChildEntities[0] is Shape && (paragraph.ChildEntities[0] as Shape).IsHorizontalRule && paragraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.Multiple && (double) paragraph.ParagraphFormat.LineSpacing != 12.0)
        this.m_bounds.Height -= (float) num2;
      if (DocumentLayouter.IsEndPage)
        return;
      if (paragraph.IsInCell && !isRowFitInSamePage && !paragraph.IsExactlyRowHeight(paragraph.GetOwnerEntity() as WTableCell, ref rowHeight))
      {
        if (isLastLineOfParagraph)
          this.m_bounds.Height += (paragraph.m_layoutInfo as ParagraphLayoutInfo).Margins.Bottom;
        if (isForceFitLayout || (double) remClientAreaHeight <= 0.0 || Math.Round((double) this.m_bounds.Bottom, 2) <= Math.Round((double) clientAreaBottom, 2) || (paragraph.GetOwnerEntity() as WTableCell).m_layoutInfo.IsVerticalText)
          return;
        this.IsNotFitted = true;
      }
      else
      {
        if (isForceFitLayout || (double) remClientAreaHeight <= 0.0 || Math.Round((double) this.m_bounds.Bottom, 2) <= Math.Round((double) clientAreaBottom, 2) || !(paragraph.OwnerTextBody.OwnerBase is WSection) || paragraph.ParagraphFormat.IsInFrame() || !this.IsNeedtoMoveNextPage(paragraph))
          return;
        this.IsNotFitted = true;
      }
    }
  }

  private bool IsNeedToUpdateListYPos()
  {
    return this.Widget is WParagraph widget && widget.ChildEntities.Count >= 1;
  }

  private bool IsLineContainsPicture()
  {
    bool flag = false;
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.ChildWidgets)
    {
      if (childWidget.Widget is WPicture)
        flag = true;
    }
    return flag;
  }

  private bool IsNeedtoMoveNextPage(WParagraph paragraph)
  {
    if (paragraph.ChildEntities.Count < 1)
      return false;
    for (int index = 0; index < paragraph.ChildEntities.Count; ++index)
    {
      IWidget widget = paragraph.Items[index] != null ? (IWidget) paragraph.Items[index] : (IWidget) null;
      if ((widget == null || !widget.LayoutInfo.IsSkip || widget is WChart) && (!(widget is Shape) || !(widget as Shape).IsHorizontalRule))
        return false;
    }
    return true;
  }

  private void ShiftLineWidgetYPosition(
    DrawingContext dc,
    double maxAscent,
    double topLineSpace,
    double maxHeight,
    ref float extraLineAscent,
    bool isInlineDrawingObject,
    ref double maxAscentOfLoweredPos,
    ref bool isAllWordsContainLoweredPos)
  {
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.ChildWidgets)
    {
      if (childWidget != null && !childWidget.Widget.LayoutInfo.IsSkipBottomAlign && (!(childWidget.Widget is WField) || ((WField) childWidget.Widget).FieldType != FieldType.FieldHyperlink))
      {
        IWidget widget1 = childWidget.Widget;
        float topEdgeExtent = 0.0f;
        float bottomEdgeExtent = 0.0f;
        if (widget1 is ParagraphItem)
          (widget1 as ParagraphItem).GetEffectExtentValues(out float _, out float _, out topEdgeExtent, out bottomEdgeExtent);
        if (!isInlineDrawingObject || !this.IsInlineFloatingItem(widget1))
        {
          if (childWidget.Widget.LayoutInfo is FootnoteLayoutInfo layoutInfo)
            widget1 = (IWidget) layoutInfo.TextRange;
          if (!(widget1 is IStringWidget stringWidget) && childWidget.Widget is SplitStringWidget widget2)
            stringWidget = widget2.RealStringWidget;
          float minValue = float.MinValue;
          double textAscent = 0.0;
          WSymbol widget3 = childWidget.Widget as WSymbol;
          if (childWidget is LayoutedMathWidget)
            textAscent = (childWidget as LayoutedMathWidget).ChildWidgets.Count != 0 ? (double) dc.GetAscent((childWidget as LayoutedMathWidget).GetFont()) : 0.0;
          else if (stringWidget != null || widget3 != null)
          {
            WField widget4 = childWidget.Widget as WField;
            textAscent = widget3 == null ? (widget4 == null || widget4.FieldType != FieldType.FieldExpression ? stringWidget.GetTextAscent(dc, ref minValue) : (double) dc.GetAscentValueForEQField(widget4)) : (double) dc.GetAscent(widget3.GetFont(dc));
          }
          else if (childWidget.Widget is WCommentMark)
            textAscent = maxAscent;
          else if (!(childWidget.Widget is BookmarkEnd) && !(childWidget.Widget is BookmarkStart) && !(childWidget.Widget is WFieldMark) && (!(childWidget.Widget is Break) || ((Break) childWidget.Widget).BreakType == BreakType.LineBreak))
            textAscent = (double) childWidget.Bounds.Height + (double) topEdgeExtent + (double) bottomEdgeExtent;
          WCharacterFormat charFormat = stringWidget is WTextRange ? (stringWidget as WTextRange).CharacterFormat : widget3?.CharacterFormat;
          this.ShiftYPosition(dc, textAscent, maxAscent, topLineSpace, maxHeight, minValue, ref extraLineAscent, charFormat, childWidget, SizeF.Empty, ref maxAscentOfLoweredPos, ref isAllWordsContainLoweredPos);
        }
      }
    }
  }

  private void ShiftListYPosition(
    WParagraph paragraph,
    DrawingContext dc,
    double maxAscent,
    double topLineSpace,
    double maxHeight,
    ref float extraLineAscent,
    ref double maxAscentOfLoweredPos,
    ref bool isAllWordsContainLoweredPos)
  {
    if (!(this.Widget.LayoutInfo is ParagraphLayoutInfo layoutInfo))
      return;
    float minValue = float.MinValue;
    WListFormat listFormatValue = paragraph.GetListFormatValue();
    SizeF size;
    float textAscent;
    if (layoutInfo.CurrentListType == ListType.Bulleted && paragraph.GetListLevel(listFormatValue).PicBullet != null)
    {
      size = dc.MeasurePictureBulletSize(paragraph.GetListLevel(listFormatValue).PicBullet, (this.Widget.LayoutInfo as ParagraphLayoutInfo).ListFont.GetFont(paragraph.Document));
      textAscent = size.Height;
    }
    else
    {
      textAscent = dc.GetAscent(layoutInfo.ListFont.GetFont(layoutInfo.CharacterFormat.Document));
      size = dc.MeasureString(layoutInfo.ListValue, layoutInfo.ListFont.GetFont(layoutInfo.CharacterFormat.Document), (StringFormat) null, layoutInfo.CharacterFormat, true);
    }
    this.ShiftYPosition(dc, (double) textAscent, maxAscent, topLineSpace, maxHeight, minValue, ref extraLineAscent, layoutInfo.CharacterFormat, (LayoutedWidget) null, size, ref maxAscentOfLoweredPos, ref isAllWordsContainLoweredPos);
  }

  private void ShiftYPosition(
    DrawingContext dc,
    double textAscent,
    double maxAscent,
    double topLineSpace,
    double maxHeight,
    float exceededLineAscent,
    ref float extraLineAscent,
    WCharacterFormat charFormat,
    LayoutedWidget ltWidget,
    SizeF size,
    ref double maxAscentOfLoweredPos,
    ref bool isAllWordsContainLoweredPos)
  {
    double yOffset = !isAllWordsContainLoweredPos || charFormat == null || (double) Math.Abs(charFormat.Position) <= maxAscentOfLoweredPos ? (maxAscent > textAscent + (charFormat == null || (double) charFormat.Position <= 0.0 ? 0.0 : (double) charFormat.Position) ? maxAscent - (textAscent + (charFormat == null || (double) charFormat.Position <= 0.0 ? 0.0 : (double) charFormat.Position)) : 0.0) + topLineSpace + (charFormat == null || (double) charFormat.Position >= 0.0 ? 0.0 : (double) Math.Abs(charFormat.Position)) : (maxAscentOfLoweredPos != 0.0 || (double) Math.Abs(charFormat.Position) - textAscent <= 0.0 ? Math.Abs(maxAscentOfLoweredPos - (double) Math.Abs(charFormat.Position)) + topLineSpace : (double) Math.Abs(charFormat.Position) - textAscent);
    float height1 = ltWidget != null ? ltWidget.Bounds.Height : size.Height;
    if (charFormat != null && charFormat.SubSuperScript != SubSuperScript.None)
    {
      float height2 = height1 / 1.5f;
      if (charFormat.SubSuperScript == SubSuperScript.SubScript)
        yOffset += (double) height1 - (double) height2;
      if (ltWidget != null)
        ltWidget.Bounds = new RectangleF(ltWidget.Bounds.X, ltWidget.Bounds.Y, ltWidget.Bounds.Width, height2);
    }
    if ((Math.Round(maxHeight, 1) != Math.Round((double) height1, 1) || textAscent != 0.0 && textAscent < maxAscent && (this.IsMaxHeightInLine(dc, height1) || Math.Round(maxHeight, 1) == Math.Round((double) height1, 1) && this.HasRaisedPosition()) || charFormat != null && charFormat.SubSuperScript == SubSuperScript.SubScript) && yOffset != 0.0)
    {
      if (ltWidget != null)
        ltWidget.ShiftLocation(0.0, yOffset, true, false);
      else
        ((ParagraphLayoutInfo) this.Widget.LayoutInfo).ListYPositions.Add(this.m_bounds.Y + (float) yOffset);
    }
    else if (ltWidget != null)
      ltWidget.ShiftLocation(0.0, topLineSpace, true, false);
    else
      ((ParagraphLayoutInfo) this.Widget.LayoutInfo).ListYPositions.Add(this.m_bounds.Y + (float) topLineSpace);
    if ((double) exceededLineAscent == -3.4028234663852886E+38 || (double) extraLineAscent >= (double) exceededLineAscent)
      return;
    extraLineAscent = exceededLineAscent;
  }

  private bool HasRaisedPosition()
  {
    for (int index = 0; index < this.ChildWidgets.Count; ++index)
    {
      if (this.ChildWidgets[index].Widget is WTextRange)
      {
        WTextRange widget = this.ChildWidgets[index].Widget as WTextRange;
        if (widget.CharacterFormat != null && (double) widget.CharacterFormat.Position > 0.0)
          return true;
      }
      else if (this.ChildWidgets[index].Widget is WSymbol)
      {
        WSymbol widget = this.ChildWidgets[index].Widget as WSymbol;
        if (widget.CharacterFormat != null && (double) widget.CharacterFormat.Position > 0.0)
          return true;
      }
    }
    return false;
  }

  private bool IsMaxHeightInLine(DrawingContext dc, float height)
  {
    int num = 0;
    WParagraph paragraph = this.GetParagraph();
    ParagraphLayoutInfo layoutInfo = this.Widget.LayoutInfo as ParagraphLayoutInfo;
    if (paragraph != null && this.ChildWidgets.Count >= 1 && paragraph.IsFirstLine(this.ChildWidgets[0]) && layoutInfo != null && layoutInfo.ListValue != string.Empty && (double) dc.MeasureString(layoutInfo.ListValue, layoutInfo.ListFont.GetFont(paragraph.Document), (StringFormat) null, layoutInfo.CharacterFormat, true).Height >= (double) height)
      ++num;
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.ChildWidgets)
    {
      if ((double) childWidget.Bounds.Height >= (double) height)
        ++num;
      if (num > 1)
        break;
    }
    return num == 1;
  }

  internal WParagraph GetParagraph()
  {
    WParagraph paragraph = (WParagraph) null;
    if (this.Widget is WParagraph)
      paragraph = this.Widget as WParagraph;
    else if (this.Widget is SplitWidgetContainer && (this.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
      paragraph = (this.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
    return paragraph;
  }

  internal bool CalculateMaxChildWidget(
    DrawingContext dc,
    WParagraph paragraph,
    bool isFirstLineOfParagraph,
    bool isLastLineOfParagraph,
    out double maxHeight,
    out double maxAscent,
    out double maxTextHeight,
    out double maxTextAscent,
    out double maxTextDescent,
    out float maxY,
    out double maxAscentOfLoweredPos,
    out IStringWidget lastTextWidget,
    out bool isClippedLine,
    out bool isTextInLine,
    out bool containsInlinePicture,
    out bool isAllWordsContainLoweredPos)
  {
    bool maxChildWidget = true;
    maxHeight = 0.0;
    maxAscent = 0.0;
    maxTextHeight = 0.0;
    maxTextAscent = 0.0;
    maxTextDescent = 0.0;
    maxAscentOfLoweredPos = 0.0;
    maxY = float.MaxValue;
    isClippedLine = false;
    isTextInLine = false;
    containsInlinePicture = false;
    isAllWordsContainLoweredPos = true;
    if (isFirstLineOfParagraph && this.Widget.LayoutInfo is ParagraphLayoutInfo && (this.Widget.LayoutInfo as ParagraphLayoutInfo).ListValue != string.Empty)
    {
      bool flag = false;
      foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.ChildWidgets)
      {
        if (flag = LayoutedWidget.IsIncludeWidgetInLineHeight(childWidget.Widget))
          break;
      }
      if (flag)
      {
        WListFormat listFormatValue = paragraph.GetListFormatValue();
        if ((this.Widget.LayoutInfo as ParagraphLayoutInfo).CurrentListType == ListType.Bulleted && paragraph.GetListLevel(listFormatValue).PicBullet != null)
        {
          SizeF sizeF = dc.MeasurePictureBulletSize(paragraph.GetListLevel(listFormatValue).PicBullet, (this.Widget.LayoutInfo as ParagraphLayoutInfo).ListFont.GetFont(paragraph.Document));
          maxHeight = (double) sizeF.Height;
          maxAscent = maxHeight;
        }
        else
        {
          maxAscent = (double) dc.GetAscent((this.Widget.LayoutInfo as ParagraphLayoutInfo).ListFont.GetFont(paragraph.Document));
          maxHeight = (double) dc.MeasureString((this.Widget.LayoutInfo as ParagraphLayoutInfo).ListValue, (this.Widget.LayoutInfo as ParagraphLayoutInfo).ListFont.GetFont(paragraph.Document), (StringFormat) null, (WCharacterFormat) null, true).Height;
        }
        maxTextHeight = maxHeight;
        maxTextAscent = maxAscent;
        maxTextDescent = maxHeight - maxAscent;
        WCharacterFormat characterFormat = (this.Widget.LayoutInfo as ParagraphLayoutInfo).CharacterFormat;
        if ((double) characterFormat.Position < 0.0 && (double) characterFormat.Position + maxAscent > 0.0)
          maxAscentOfLoweredPos = (double) characterFormat.Position + maxAscent;
        else
          isAllWordsContainLoweredPos = false;
      }
    }
    lastTextWidget = (IStringWidget) null;
    double num1 = 0.0;
    for (int index1 = 0; index1 < this.ChildWidgets.Count; ++index1)
    {
      LayoutedWidget childWidget1 = this.ChildWidgets[index1];
      IWidget widget1 = childWidget1.Widget;
      if (childWidget1 != null && !childWidget1.Widget.LayoutInfo.IsSkipBottomAlign && (!(widget1 is WField) || (widget1 as WField).FieldType != FieldType.FieldHyperlink && (widget1 as WField).FieldType != FieldType.FieldIncludePicture))
      {
        if (childWidget1.Widget.LayoutInfo is FootnoteLayoutInfo)
          widget1 = (IWidget) (childWidget1.Widget.LayoutInfo as FootnoteLayoutInfo).TextRange;
        bool flag1 = LayoutedWidget.IsIncludeTextWidgetInLineHeight(widget1);
        if (!(widget1 is IStringWidget stringWidget) && childWidget1.Widget is SplitStringWidget widget2)
          stringWidget = widget2.RealStringWidget;
        if (!flag1 && !(childWidget1.Widget.LayoutInfo is FootnoteLayoutInfo) || childWidget1.Widget.LayoutInfo is TabsLayoutInfo)
        {
          lastTextWidget = stringWidget;
          num1 = (double) childWidget1.Bounds.Height;
        }
        else
        {
          float topEdgeExtent = 0.0f;
          float bottomEdgeExtent = 0.0f;
          if (widget1 is ParagraphItem)
            (widget1 as ParagraphItem).GetEffectExtentValues(out float _, out float _, out topEdgeExtent, out bottomEdgeExtent);
          if ((this.ChildWidgets.Count == 1 && maxHeight == 0.0 || maxHeight < (double) childWidget1.Bounds.Height + (double) topEdgeExtent + (double) bottomEdgeExtent) && (!(childWidget1.Widget is Break) || (childWidget1.Widget as Break).BreakType != BreakType.LineBreak))
            maxHeight = (double) childWidget1.Bounds.Height + (double) topEdgeExtent + (double) bottomEdgeExtent;
          if (Math.Round((double) childWidget1.Bounds.Y - (double) topEdgeExtent, 2) > Math.Round((double) this.m_bounds.Y, 2))
            maxY = childWidget1.Bounds.Y;
          if (!isClippedLine && widget1.LayoutInfo.IsClipped)
            isClippedLine = true;
          if (stringWidget != null && (!(childWidget1.Widget is WField) || (childWidget1.Widget as WField).FieldType != FieldType.FieldExpression) || childWidget1.Widget is WSymbol)
          {
            float minValue = float.MinValue;
            double num2 = !(childWidget1.Widget is WSymbol) ? stringWidget.GetTextAscent(dc, ref minValue) : (double) dc.GetAscent((childWidget1.Widget as WSymbol).GetFont(dc));
            WCharacterFormat wcharacterFormat = stringWidget != null ? (stringWidget as WTextRange).CharacterFormat : (childWidget1.Widget as WSymbol).CharacterFormat;
            if (childWidget1.Widget is WCheckBox)
            {
              float height = dc.MeasureString(" ", childWidget1.Widget.LayoutInfo.Font.GetFont(paragraph.Document), dc.StringFormt).Height;
              if (maxHeight < (double) height)
                maxHeight = (double) height;
            }
            if ((double) childWidget1.Bounds.Height != 0.0 && isAllWordsContainLoweredPos)
              isAllWordsContainLoweredPos = (double) wcharacterFormat.Position < 0.0 && isAllWordsContainLoweredPos;
            double num3 = num2 + (double) wcharacterFormat.Position;
            if ((double) childWidget1.Bounds.Height != 0.0 && (double) wcharacterFormat.Position < 0.0 && num3 > 0.0 && num3 > maxAscentOfLoweredPos)
              maxAscentOfLoweredPos = num3;
            if (maxAscent < num2 + (double) wcharacterFormat.Position && (double) childWidget1.Bounds.Height != 0.0)
              maxAscent = num2 + (double) wcharacterFormat.Position;
            if ((double) wcharacterFormat.Position != 0.0)
            {
              float num4 = 0.04f;
              if (maxHeight < maxAscent)
                maxHeight = maxAscent;
              else if (maxHeight < maxAscent + ((double) childWidget1.Bounds.Height - (num2 + (double) wcharacterFormat.Position)))
                maxHeight = maxAscent + ((double) childWidget1.Bounds.Height + (double) wcharacterFormat.Font.Size * (double) num4 - (num2 + (double) wcharacterFormat.Position));
            }
            maxChildWidget = false;
            if ((!(widget1 is WField) || (widget1 as WField).FieldType == FieldType.FieldExpression) && !(widget1 is WCheckBox) && !(widget1 is WDropDownFormField))
              isTextInLine = true;
            if (maxTextHeight < (double) childWidget1.Bounds.Height)
              maxTextHeight = (double) childWidget1.Bounds.Height;
            if (maxTextAscent < num2)
              maxTextAscent = num2;
            if (maxTextDescent < (double) childWidget1.Bounds.Height - num2)
              maxTextDescent = (double) childWidget1.Bounds.Height - num2;
          }
          else if (!(childWidget1.Widget is BookmarkEnd) && !(childWidget1.Widget is BookmarkStart) && !(childWidget1.Widget is InlineShapeObject) && !(childWidget1.Widget is WCommentMark) && !(childWidget1.Widget is WFieldMark) && (!(childWidget1.Widget is Break) || (childWidget1.Widget as Break).BreakType == BreakType.LineBreak || (childWidget1.Widget as Break).BreakType == BreakType.ColumnBreak && paragraph.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013))
          {
            if (childWidget1.Widget is Break)
            {
              if (maxTextHeight == 0.0 && maxHeight == 0.0)
              {
                maxTextHeight = (double) childWidget1.Bounds.Height;
                maxHeight = (double) childWidget1.Bounds.Height;
              }
            }
            else
            {
              if (maxAscent < maxHeight)
                maxAscent = !(childWidget1.Widget is WField) || (childWidget1.Widget as WField).FieldType != FieldType.FieldExpression ? (!(childWidget1 is LayoutedMathWidget) ? maxHeight : ((childWidget1 as LayoutedMathWidget).ChildWidgets.Count != 0 ? (double) dc.GetAscent((childWidget1 as LayoutedMathWidget).GetFont()) : 0.0)) : (double) dc.GetAscentValueForEQField(childWidget1.Widget as WField);
              if (this.IsInlineFloatingItem(childWidget1.Widget) || childWidget1.Widget is WField && (childWidget1.Widget as WField).FieldType == FieldType.FieldExpression)
              {
                if (!(childWidget1.Widget is WField))
                  containsInlinePicture = true;
                if (childWidget1.Widget is Shape && (childWidget1.Widget as Shape).IsHorizontalRule)
                {
                  ParagraphItem widget3 = childWidget1.Widget as ParagraphItem;
                  double height = (double) dc.MeasureString(" ", widget3.ParaItemCharFormat.Font, (StringFormat) null, (WCharacterFormat) null, true).Height;
                  double ascent = (double) dc.GetAscent(widget3.ParaItemCharFormat.Font);
                  if (maxHeight < height)
                    maxHeight = height;
                  if (maxAscent < ascent)
                    maxAscent = ascent;
                  maxTextHeight = maxHeight;
                }
                else if (paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
                {
                  ParagraphItem widget4 = childWidget1.Widget as ParagraphItem;
                  WCharacterFormat charFormat = widget4.GetCharFormat();
                  if (charFormat != null)
                  {
                    double num5 = (double) dc.MeasureString(" ", widget4.ParaItemCharFormat.GetFontToRender(FontScriptType.English), (StringFormat) null, (WCharacterFormat) null, true).Height;
                    double num6 = (double) dc.GetAscent(widget4.ParaItemCharFormat.GetFontToRender(FontScriptType.English));
                    if (maxAscent < num6 + (double) charFormat.Position && (double) childWidget1.Bounds.Height != 0.0)
                      maxAscent = num6 + (double) charFormat.Position;
                    if ((double) charFormat.Position != 0.0)
                    {
                      if (maxHeight == (double) childWidget1.Bounds.Height && maxHeight == maxAscent && (double) charFormat.Position < 0.0 && maxHeight > (double) Math.Abs(charFormat.Position))
                      {
                        maxAscent = maxHeight + (double) charFormat.Position;
                        num5 = maxHeight;
                        num6 = maxAscent;
                      }
                      else if (maxHeight < maxAscent)
                        maxHeight = maxAscent;
                      else if (maxHeight < maxAscent + ((double) childWidget1.Bounds.Height - (num6 + (double) charFormat.Position)))
                        maxHeight = maxAscent + ((double) childWidget1.Bounds.Height - (num6 + (double) charFormat.Position));
                    }
                    if (maxTextHeight < num5)
                      maxTextHeight = num5;
                    if (maxTextAscent < num6)
                      maxTextAscent = num6;
                    if (maxAscent < num6)
                      maxAscent = num6;
                  }
                }
                else if (maxTextHeight == 0.0)
                {
                  bool flag2 = false;
                  for (int index2 = index1 + 1; index2 < this.ChildWidgets.Count; ++index2)
                  {
                    LayoutedWidget childWidget2 = this.ChildWidgets[index2];
                    LayoutedWidget childWidget3 = this.ChildWidgets[index2];
                  }
                  if (!flag2)
                  {
                    double height = (double) paragraph.m_layoutInfo.Size.Height;
                    double ascent = (double) dc.GetAscent(paragraph.BreakCharacterFormat.GetFontToRender(FontScriptType.English));
                    if (maxHeight < height)
                      maxHeight = height;
                    if (maxTextHeight < height)
                      maxTextHeight = height;
                    if (maxTextAscent < ascent)
                      maxTextAscent = ascent;
                    if (maxAscent < ascent)
                      maxAscent = ascent;
                  }
                }
              }
            }
            maxChildWidget = false;
          }
        }
      }
    }
    if (maxChildWidget && !this.IsSkipFieldCodeParagraphHeight())
    {
      double num7;
      if (!isLastLineOfParagraph && lastTextWidget != null)
      {
        float minValue = float.MinValue;
        num7 = lastTextWidget.GetTextAscent(dc, ref minValue);
      }
      else
      {
        num1 = (double) paragraph.m_layoutInfo.Size.Height;
        num7 = (double) dc.GetAscent(paragraph.BreakCharacterFormat.GetFontToRender(FontScriptType.English));
        if (this.ChildWidgets.Count == 1 && paragraph.ChildEntities.Count > 0 && paragraph.ChildEntities[0] is Break && (paragraph.ChildEntities[0] as Break).BreakType == BreakType.ColumnBreak && ((paragraph.ChildEntities[0] as Break).CharacterFormat.HasKey(3) || (paragraph.ChildEntities[0] as Break).CharacterFormat.HasKey(2)))
        {
          num1 = (double) dc.MeasureString(" ", (paragraph.ChildEntities[0] as Break).CharacterFormat.Font, (StringFormat) null).Height;
          num7 = (double) dc.GetAscent((paragraph.ChildEntities[0] as Break).CharacterFormat.GetFontToRender(FontScriptType.English));
        }
        lastTextWidget = (IStringWidget) null;
      }
      if (maxAscent < num7)
        maxAscent = num7;
      if (maxHeight < num1)
        maxHeight = num1;
      if (maxTextHeight < num1)
        maxTextHeight = num1;
      if (maxTextAscent < num7)
        maxTextAscent = num7;
      if (maxTextDescent < num1 - num7)
        maxTextDescent = num1 - num7;
    }
    return maxChildWidget;
  }

  private WField GetField()
  {
    int index = 0;
    if (this.Widget is SplitWidgetContainer && (this.Widget as SplitWidgetContainer).m_currentChild is Entity currentChild)
      index = currentChild.Index;
    for (WParagraph paragraph = this.GetParagraph(); index < paragraph.Items.Count; ++index)
    {
      if (paragraph.Items[index] is WField)
        return paragraph.Items[index] as WField;
      if (!(paragraph.Items[index] is WFieldMark))
        break;
    }
    return (WField) null;
  }

  private bool IsSkipFieldCodeParagraphHeight()
  {
    WField field = this.GetField();
    WParagraph paragraph = this.GetParagraph();
    if (this.ChildWidgets.Count > 0 || field == null || field.FieldSeparator == null && field.FieldEnd != null && paragraph.Items.Contains((IEntity) field.FieldEnd))
      return false;
    if (field.FieldSeparator == null)
      return true;
    for (int index = field.Index + 1; index < paragraph.Items.Count; ++index)
    {
      if (paragraph.Items[index] == field.FieldSeparator)
        return false;
    }
    return true;
  }

  private bool IsInlineFloatingItem(IWidget item)
  {
    switch (item)
    {
      case WPicture _:
      case Shape _:
      case WTextBox _:
      case WChart _:
      case GroupShape _:
        return true;
      default:
        return false;
    }
  }

  public void AlignCenter(DrawingContext dc, double subWidth, bool isAlignCenter)
  {
    this.AlignCenterorRight(dc, subWidth, isAlignCenter);
  }

  public double AlignRight(DrawingContext dc, double subWidth, bool isAlignCenter)
  {
    return this.AlignCenterorRight(dc, subWidth, isAlignCenter);
  }

  internal double AlignCenterorRight(DrawingContext dc, double subWidth, bool isAlignCenter)
  {
    if (this.ChildWidgets.Count > 0)
    {
      LayoutedWidget childWidget = this.ChildWidgets[this.ChildWidgets.Count - 1];
      string str1 = childWidget.Widget is SplitStringWidget widget ? widget.SplittedText : string.Empty;
      if (!string.IsNullOrEmpty(str1) && str1.EndsWith(ControlChar.Space))
      {
        int length = str1.Length;
        string str2 = str1.TrimEnd(ControlChar.SpaceChar);
        int num = length - str2.Length;
        if (widget != null && widget.Length > 1)
          widget.Length -= num;
        else
          widget.Length = str2.Length;
        childWidget.IsContainsSpaceCharAtEnd = true;
        if (str2 == string.Empty)
        {
          if (isAlignCenter)
            subWidth += (double) childWidget.Bounds.Width / 2.0;
          else
            subWidth += (double) childWidget.Bounds.Width;
          childWidget.Bounds = new RectangleF(childWidget.Bounds.X, childWidget.Bounds.Y, 0.0f, childWidget.Bounds.Height);
        }
        else
        {
          SizeF sizeF = widget.Measure(dc);
          if (isAlignCenter)
            subWidth += ((double) childWidget.Bounds.Width - (double) sizeF.Width) / 2.0;
          else
            subWidth += (double) childWidget.Bounds.Width - (double) sizeF.Width;
          childWidget.Bounds = new RectangleF(childWidget.Bounds.X, childWidget.Bounds.Y, sizeF.Width, childWidget.Bounds.Height);
        }
      }
      else
        subWidth += (double) this.ValidLayoutedItemCenterorRightPosition(dc, isAlignCenter, subWidth);
    }
    this.ShiftLocation(subWidth, 0.0, true, false);
    return subWidth;
  }

  private float ValidLayoutedItemCenterorRightPosition(
    DrawingContext dc,
    bool isAlignCenter,
    double subWidth)
  {
    int count = this.ChildWidgets.Count;
    float num1 = 0.0f;
    for (int index = count - 1; index >= 0; --index)
    {
      LayoutedWidget childWidget = this.ChildWidgets[index];
      string str = childWidget.Widget is WField ? ((childWidget.Widget as WField).FieldType == FieldType.FieldNumPages ? (childWidget.Widget as WField).FieldResult : ((childWidget.Widget as WField).FieldType == FieldType.FieldPage ? childWidget.TextTag : (string) null)) : (childWidget.Widget is WTextRange ? (childWidget.Widget as WTextRange).Text : (string) null);
      if (!(childWidget.Widget is BookmarkStart) && !(childWidget.Widget is BookmarkEnd))
      {
        if (str != null)
        {
          if (str.TrimEnd(ControlChar.SpaceChar) == "" && !(childWidget.Widget.LayoutInfo is TabsLayoutInfo) && !(childWidget.Widget is WCheckBox) && !(childWidget.Widget is WDropDownFormField))
            goto label_4;
        }
        if (str != null && str.EndsWith(ControlChar.Space))
        {
          if (str.TrimEnd(ControlChar.SpaceChar) != "")
          {
            string text = (childWidget.Widget as WTextRange).Text.TrimEnd(ControlChar.SpaceChar);
            SizeF sizeF = dc.MeasureTextRange(childWidget.Widget as WTextRange, text);
            float num2 = childWidget.Bounds.Width - sizeF.Width;
            if ((double) num2 > 0.0)
            {
              num1 += num2;
              break;
            }
            break;
          }
        }
        if (childWidget.Widget is WMath && !(childWidget.Widget as WMath).IsInline)
        {
          num1 = (float) -subWidth;
          isAlignCenter = false;
          continue;
        }
        if (!(childWidget.Widget is Break))
          break;
        continue;
      }
label_4:
      num1 += childWidget.Bounds.Width;
    }
    if (isAlignCenter)
      num1 /= 2f;
    return num1;
  }

  public void AlignJustify(
    DrawingContext dc,
    double subWidth,
    bool isFromInterSectingFloattingItem,
    bool isParaBidi)
  {
    this.m_bounds.Width += (float) subWidth;
    int[] widgetSpaces = new int[this.ChildWidgets.Count];
    int countAllSpaces1 = 0;
    string[] strArray = new string[this.ChildWidgets.Count];
    int tabIndex = this.GetTabIndex();
    for (int index = tabIndex; index < widgetSpaces.Length; ++index)
    {
      LayoutedWidget childWidget = this.ChildWidgets[index];
      IStringWidget widget1 = childWidget.Widget as IStringWidget;
      string str1 = (string) null;
      RectangleF bounds;
      if (widget1 == null)
      {
        if (!(childWidget.Widget is SplitStringWidget widget2) || widget2.SplittedText == null)
        {
          widgetSpaces[index] = 0;
        }
        else
        {
          if (widgetSpaces.Length == 1 || index == 0 && this.IsNeedToTrimTextRange(index + 1, this.ChildWidgets))
          {
            string splittedText = widget2.SplittedText;
            int length1 = splittedText.Length;
            string str2 = splittedText.TrimStart(ControlChar.SpaceChar);
            int num1 = length1 - str2.Length;
            widget2.StartIndex += num1;
            if (widget2.Length > 1)
              widget2.Length -= num1;
            int length2 = str2.Length;
            string str3 = str2.TrimEnd(ControlChar.SpaceChar);
            if (widget2.Length > 1)
              widget2.Length -= length2 - str3.Length;
            else
              widget2.Length = str3.Length;
            if (length2 - str3.Length > 0)
            {
              childWidget.IsContainsSpaceCharAtEnd = true;
              if (widget2.SplittedText == string.Empty)
              {
                double num2 = subWidth;
                bounds = childWidget.Bounds;
                double width = (double) bounds.Width;
                subWidth = num2 + width;
                childWidget.Bounds = new RectangleF(childWidget.Bounds.X, childWidget.Bounds.Y, 0.0f, childWidget.Bounds.Height);
              }
              else
              {
                SizeF sizeF = widget2.Measure(dc);
                subWidth += (double) childWidget.Bounds.Width - (double) sizeF.Width;
                childWidget.Bounds = new RectangleF(childWidget.Bounds.X, childWidget.Bounds.Y, sizeF.Width, childWidget.Bounds.Height);
              }
            }
          }
          else if (index == 0)
          {
            string splittedText = widget2.SplittedText;
            int num = splittedText.Length - splittedText.TrimStart(ControlChar.SpaceChar).Length;
            widget2.StartIndex += num;
            widget2.Length -= num;
          }
          else if (index == widgetSpaces.Length - 1 || this.IsNeedToTrimTextRange(index + 1, this.ChildWidgets))
          {
            string splittedText = widget2.SplittedText;
            int length = splittedText.Length;
            string str4 = splittedText.TrimEnd(ControlChar.SpaceChar);
            widget2.Length -= length - str4.Length;
            if (length - str4.Length > 0)
            {
              childWidget.IsContainsSpaceCharAtEnd = true;
              if (widget2.SplittedText == string.Empty)
              {
                subWidth += (double) childWidget.Bounds.Width;
                childWidget.Bounds = new RectangleF(childWidget.Bounds.X, childWidget.Bounds.Y, 0.0f, childWidget.Bounds.Height);
              }
              else
              {
                SizeF sizeF = widget2.Measure(dc);
                subWidth += (double) childWidget.Bounds.Width - (double) sizeF.Width;
                childWidget.Bounds = new RectangleF(childWidget.Bounds.X, childWidget.Bounds.Y, sizeF.Width, childWidget.Bounds.Height);
              }
            }
          }
          str1 = widget2.GetText();
        }
      }
      else
      {
        str1 = childWidget.Widget is WField ? ((childWidget.Widget as WField).FieldType == FieldType.FieldNumPages ? (childWidget.Widget as WField).FieldResult : ((childWidget.Widget as WField).FieldType == FieldType.FieldPage ? childWidget.TextTag : string.Empty)) : (widget1 as WTextRange).Text;
        WTextRange wtextRange = (WTextRange) null;
        if (childWidget.Widget is WTextRange && !(childWidget.Widget is WField))
          wtextRange = childWidget.Widget as WTextRange;
        if (wtextRange != null)
        {
          str1 = wtextRange.Text;
          if (index == 0)
            str1 = str1.TrimStart(ControlChar.SpaceChar);
          if (index == widgetSpaces.Length - 1 || this.IsNeedToTrimTextRange(index + 1, this.ChildWidgets))
          {
            string str5 = str1.TrimEnd(ControlChar.SpaceChar);
            if (str1.Length - str5.Length > 0)
            {
              childWidget.IsContainsSpaceCharAtEnd = true;
              str1 = str5;
              if (str5 == string.Empty)
              {
                subWidth += (double) childWidget.Bounds.Width;
                LayoutedWidget layoutedWidget = childWidget;
                double x = (double) childWidget.Bounds.X;
                bounds = childWidget.Bounds;
                double y = (double) bounds.Y;
                bounds = childWidget.Bounds;
                double height = (double) bounds.Height;
                RectangleF rectangleF = new RectangleF((float) x, (float) y, 0.0f, (float) height);
                layoutedWidget.Bounds = rectangleF;
              }
              else
              {
                WTextRange clonedTextRange = wtextRange.Clone() as WTextRange;
                clonedTextRange.Text = wtextRange.Text.TrimEnd(ControlChar.SpaceChar);
                SizeF textRangeSize = wtextRange.GetTextRangeSize(clonedTextRange);
                double num3 = subWidth;
                bounds = childWidget.Bounds;
                double num4 = (double) bounds.Width - (double) textRangeSize.Width;
                subWidth = num3 + num4;
                LayoutedWidget layoutedWidget = childWidget;
                bounds = childWidget.Bounds;
                double x = (double) bounds.X;
                bounds = childWidget.Bounds;
                double y = (double) bounds.Y;
                double width = (double) textRangeSize.Width;
                bounds = childWidget.Bounds;
                double height = (double) bounds.Height;
                RectangleF rectangleF = new RectangleF((float) x, (float) y, (float) width, (float) height);
                layoutedWidget.Bounds = rectangleF;
              }
            }
          }
        }
      }
      if (str1 != null)
      {
        int num = str1.Split(' ').Length - 1;
        widgetSpaces[index] = num;
        this.ChildWidgets[index].Spaces = num;
        countAllSpaces1 += num;
        strArray[index] = str1;
      }
    }
    float spaceDelta = this.GetSpaceDelta(countAllSpaces1, subWidth, widgetSpaces, 0);
    double xOffset = 0.0;
    int countAllSpaces2 = countAllSpaces1;
    for (int index = tabIndex; index < widgetSpaces.Length; ++index)
    {
      LayoutedWidget childWidget = this.ChildWidgets[index];
      if (!isFromInterSectingFloattingItem || !(childWidget.Widget is Entity) || !(childWidget.Widget as Entity).IsFloatingItem(false))
      {
        IStringWidget widget = childWidget.Widget as IStringWidget;
        SplitStringWidget splitStringWidget = (SplitStringWidget) null;
        if (!isParaBidi)
          childWidget.ShiftLocation(xOffset, 0.0, true, false);
        if (widget == null)
          splitStringWidget = childWidget.Widget as SplitStringWidget;
        if (widget != null || splitStringWidget != null)
        {
          RectangleF bounds = childWidget.Bounds;
          double num = (double) spaceDelta * (double) widgetSpaces[index];
          if (index != widgetSpaces.Length - 1 && this.ChildWidgets[index + 1].Widget != null && this.ChildWidgets[index + 1].Widget.LayoutInfo is TabsLayoutInfo)
          {
            num = 0.0;
            countAllSpaces2 -= widgetSpaces[index];
            spaceDelta = this.GetSpaceDelta(countAllSpaces2, subWidth - xOffset, widgetSpaces, index + 1);
          }
          bounds.Width += (float) num;
          childWidget.Bounds = bounds;
          xOffset += num;
          if (isParaBidi)
            childWidget.ShiftLocation(-xOffset, 0.0, true, false);
        }
      }
    }
  }

  private bool IsNeedToTrimTextRange(int index, LayoutedWidgetList childWidgets)
  {
    for (int index1 = childWidgets.Count - 1; index1 >= index; --index1)
    {
      if ((double) childWidgets[index1].Bounds.Width != 0.0)
      {
        if (childWidgets[index1].Widget is IStringWidget && !(childWidgets[index1].Widget is WField) && !(childWidgets[index1].Widget.LayoutInfo is TabsLayoutInfo))
        {
          if ((childWidgets[index1].Widget as IStringWidget as WTextRange).Text.Trim(ControlChar.SpaceChar) != string.Empty)
            return false;
        }
        else
        {
          if (!(childWidgets[index1].Widget is SplitStringWidget) || childWidgets[index1].Widget.LayoutInfo is TabsLayoutInfo)
            return false;
          if ((childWidgets[index1].Widget as SplitStringWidget).SplittedText.Trim(ControlChar.SpaceChar) != string.Empty)
            return false;
        }
      }
    }
    return true;
  }

  private int GetTabIndex()
  {
    int tabIndex = 0;
    bool flag = false;
    for (int index = 0; index < this.ChildWidgets.Count; ++index)
    {
      if (this.ChildWidgets[index].Widget.LayoutInfo is TabsLayoutInfo)
      {
        tabIndex = index + 1;
        flag = true;
      }
      else if (tabIndex >= index && flag)
      {
        WTextRange textRange = this.GetTextRange(this.ChildWidgets[index].Widget);
        if (textRange != null)
        {
          if (textRange.Text.Trim(' ') == string.Empty)
            goto label_7;
        }
        if (textRange != null)
        {
          flag = false;
          continue;
        }
label_7:
        tabIndex = index + 1;
      }
    }
    return tabIndex;
  }

  private WTextRange GetTextRange(IWidget widget)
  {
    switch (widget)
    {
      case WTextRange _:
        return widget as WTextRange;
      case SplitStringWidget _:
        return (widget as SplitStringWidget).RealStringWidget as WTextRange;
      default:
        return (WTextRange) null;
    }
  }

  private bool IsNotWord2013()
  {
    Entity entity = this.m_widget as Entity;
    if (this.m_widget is SplitStringWidget)
      entity = (this.m_widget as SplitStringWidget).RealStringWidget as Entity;
    else if (this.m_widget is SplitWidgetContainer)
      entity = (this.m_widget as SplitWidgetContainer).RealWidgetContainer as Entity;
    return entity != null && entity.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013;
  }

  private float GetSpaceDelta(int countAllSpaces, double subWidth, int[] widgetSpaces, int index)
  {
    float spaceDelta = countAllSpaces != 0 ? Convert.ToSingle(subWidth) / (float) countAllSpaces : 0.0f;
    this.SubWidth = Convert.ToSingle(subWidth);
    bool flag = this.IsNotWord2013();
    for (int index1 = index; index1 < this.ChildWidgets.Count; ++index1)
    {
      if ((double) spaceDelta < 1.0 && flag)
        this.ChildWidgets[index1].WordSpace = Convert.ToSingle(spaceDelta) * -1f;
      else
        this.ChildWidgets[index1].WordSpace = Convert.ToSingle(spaceDelta);
      this.ChildWidgets[index1].SubWidth = (float) widgetSpaces[index1] * spaceDelta;
    }
    return spaceDelta;
  }

  internal void UpdateParaFirstLineHorizontalPositions(
    ParagraphLayoutInfo paragraphInfo,
    IWidget widget,
    ref float x,
    ref float width)
  {
    float firstLineIndent = paragraphInfo.FirstLineIndent;
    float listTab = paragraphInfo.ListTab;
    if (!(widget is SplitWidgetContainer))
    {
      if (paragraphInfo.LevelNumber != -1)
        firstLineIndent += listTab;
      x += firstLineIndent;
      width -= firstLineIndent;
    }
    WParagraph realWidgetContainer = widget is SplitWidgetContainer ? (widget as SplitWidgetContainer).RealWidgetContainer as WParagraph : (WParagraph) null;
    if (realWidgetContainer == null)
      return;
    int count1 = realWidgetContainer.ChildEntities.Count;
    int count2 = (widget as SplitWidgetContainer).Count;
    if (realWidgetContainer.IsInCell || count1 <= count2 || !(realWidgetContainer.ChildEntities[count1 - 1 - count2] is Break) || !(realWidgetContainer.ChildEntities[count1 - 1 - count2] is Break childEntity) || childEntity.BreakType != BreakType.PageBreak && childEntity.BreakType != BreakType.ColumnBreak)
      return;
    x += firstLineIndent + listTab;
    width -= firstLineIndent + listTab;
  }

  internal static bool IsIncludeWidgetInLineHeight(IWidget widget)
  {
    switch (widget)
    {
      case BookmarkStart _:
      case BookmarkEnd _:
      case WFieldMark _:
      case Break _:
      case WComment _:
label_2:
        return false;
      default:
        if (!(widget.LayoutInfo is TabsLayoutInfo))
        {
          if (widget.LayoutInfo is FootnoteLayoutInfo)
            widget = (IWidget) (widget.LayoutInfo as FootnoteLayoutInfo).TextRange;
          return LayoutedWidget.IsIncludeTextWidgetInLineHeight(widget);
        }
        goto label_2;
    }
  }

  internal static bool IsIncludeTextWidgetInLineHeight(IWidget widget)
  {
    string str = "";
    WTextRange wtextRange = (WTextRange) null;
    if (widget is WTextRange && !(widget is WField))
    {
      wtextRange = widget as WTextRange;
      str = wtextRange is WDropDownFormField ? (wtextRange as WDropDownFormField).DropDownValue : wtextRange.Text;
    }
    else if (widget is SplitStringWidget)
    {
      str = (widget as SplitStringWidget).SplittedText;
      wtextRange = (widget as SplitStringWidget).RealStringWidget as WTextRange;
    }
    if (wtextRange != null && wtextRange.Owner != null)
    {
      if (str.Trim(ControlChar.SpaceChar) == string.Empty)
        return false;
    }
    return true;
  }
}
