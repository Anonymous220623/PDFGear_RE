// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LCLineContainer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.Office;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.Layouting;

internal class LCLineContainer : LCContainer
{
  private bool IsFirstItemInPage
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 191 | (value ? 1 : 0) << 6);
  }

  public LCLineContainer(IWidgetContainer container, ILCOperator lcOperator, bool isForceFitLayout)
    : base(container, lcOperator, isForceFitLayout)
  {
    this.m_bSkipAreaSpacing = true;
    (lcOperator as Layouter).m_canSplitbyCharacter = true;
    (lcOperator as Layouter).m_canSplitByTab = false;
    (lcOperator as Layouter).IsFirstItemInLine = true;
  }

  protected override void DoLayoutChild(LayoutContext childContext)
  {
    this.IsTabStopBeyondRightMarginExists = false;
    childContext.IsTabStopBeyondRightMarginExists = false;
    RectangleF clientActiveArea = this.m_layoutArea.ClientActiveArea;
    if (this.m_ltWidget.ChildWidgets.Count == 0)
    {
      float x = clientActiveArea.X;
      float width = clientActiveArea.Width;
      this.m_ltWidget.UpdateParaFirstLineHorizontalPositions(this.LayoutInfo as ParagraphLayoutInfo, childContext.Widget, ref x, ref width);
      clientActiveArea.X = x;
      clientActiveArea.Width = width;
    }
    if (this.IsForceFitLayout)
      this.IsFirstItemInPage = true;
    LayoutedWidget layoutedWidget = this.CheckNullConditionAndReturnltwidget();
    if ((this.CurrentChildWidget is ParagraphItem || this.CurrentChildWidget is SplitStringWidget) && layoutedWidget != null && !(layoutedWidget.Widget is SplitStringWidget))
    {
      int[] interSectingPoint = (this.m_lcOperator as Layouter).m_interSectingPoint;
      if (interSectingPoint[2] != int.MinValue && interSectingPoint[3] != int.MinValue && (layoutedWidget.ChildWidgets.Count > interSectingPoint[2] ? ((layoutedWidget = layoutedWidget.ChildWidgets[interSectingPoint[2]]) != null ? 1 : 0) : 0) != 0 && layoutedWidget.ChildWidgets.Count > interSectingPoint[3] && (this.m_lcOperator as Layouter).NotFittedFloatingItems.Count == 0)
      {
        LayoutedWidget childWidget = layoutedWidget.ChildWidgets[interSectingPoint[3]];
        childContext.Widget = childWidget.Widget;
        this.AddLayoutWidgetInBeforeInsectingPoint(childWidget, interSectingPoint[3]);
      }
      else if (layoutedWidget != null && interSectingPoint[3] == int.MinValue)
      {
        LayoutedWidget interSectWidget = layoutedWidget;
        childContext.Widget = interSectWidget.Widget;
        this.AddLayoutWidgetInBeforeInsectingPoint(interSectWidget, interSectingPoint[1]);
      }
      clientActiveArea = this.m_layoutArea.ClientActiveArea;
      (this.m_lcOperator as Layouter).m_interSectingPoint[0] = int.MinValue;
      (this.m_lcOperator as Layouter).m_interSectingPoint[1] = int.MinValue;
      (this.m_lcOperator as Layouter).m_interSectingPoint[2] = int.MinValue;
      (this.m_lcOperator as Layouter).m_interSectingPoint[3] = int.MinValue;
    }
    this.m_currChildLW = childContext.Layout(clientActiveArea);
    if (this.m_currChildLW != null && this.m_currChildLW.Widget is WParagraph && !(childContext is LCLineContainer))
    {
      WParagraph widget = this.m_currChildLW.Widget as WParagraph;
      if ((this.m_lcOperator as Layouter).FloatingItems.Count > 0 && this.IsNeedToUpdateIntersectingBounds(widget))
      {
        WParagraphFormat paragraphFormat = (this.m_currChildLW.Widget as WParagraph).ParagraphFormat;
        if (paragraphFormat.TextureStyle != TextureStyle.TextureNone || !paragraphFormat.BackColor.IsEmpty)
          this.UpdateItersectingFloatingItemBounds();
      }
      this.UpdatePositionByLineTextWrap(widget);
    }
    (this.m_lcOperator as Layouter).PreviousTab = new TabsLayoutInfo.LayoutTab();
    (this.m_lcOperator as Layouter).PreviousTabWidth = 0.0f;
    if ((this.m_lcOperator as Layouter).m_lineSpaceWidths != null)
    {
      (this.m_lcOperator as Layouter).m_lineSpaceWidths.Clear();
      (this.m_lcOperator as Layouter).m_lineSpaceWidths = (List<float>) null;
    }
    (this.m_lcOperator as Layouter).m_effectiveJustifyWidth = 0.0f;
    (this.m_lcOperator as Layouter).IsWord2013WordFitLayout = false;
  }

  private void UpdatePositionByLineTextWrap(WParagraph paragraph)
  {
    if (this.SkipUpdatingPosition(paragraph))
      return;
    bool flag = false;
    LayoutedWidget layoutedWidget = (LayoutedWidget) null;
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.m_currChildLW.ChildWidgets)
    {
      if (childWidget.Widget is ParagraphItem && (childWidget.Widget as ParagraphItem).IsFloatingItem(false) && (childWidget.Widget as ParagraphItem).GetHorizontalOrigin() == HorizontalOrigin.Column)
      {
        if (!flag)
        {
          TextWrappingStyle textWrappingStyle = (childWidget.Widget as ParagraphItem).GetTextWrappingStyle();
          flag = textWrappingStyle == TextWrappingStyle.Behind || textWrappingStyle == TextWrappingStyle.InFrontOfText;
        }
      }
      else if (layoutedWidget == null && childWidget.Widget is ILeafWidget && !(childWidget.Widget is BookmarkStart) && !(childWidget.Widget is BookmarkEnd))
        layoutedWidget = childWidget;
    }
    if (layoutedWidget == null || (double) this.m_currChildLW.Bounds.X != (double) layoutedWidget.Bounds.X || (double) this.m_currChildLW.Bounds.Height <= (double) layoutedWidget.Bounds.Height)
      return;
    RectangleF bounds = this.m_currChildLW.Bounds with
    {
      Width = this.m_layoutArea.ClientArea.Width,
      Height = this.m_layoutArea.ClientArea.Height
    };
    SizeF widgetSize = new SizeF(layoutedWidget.Bounds.Width, this.m_currChildLW.Bounds.Height);
    RectangleF wrappedPosition = new LeafLayoutContext(layoutedWidget.Widget as ILeafWidget, this.m_lcOperator, this.IsForceFitLayout).FindWrappedPosition(widgetSize, bounds);
    if ((double) bounds.X >= (double) wrappedPosition.X || (double) wrappedPosition.Width < (double) widgetSize.Width)
      return;
    float num = wrappedPosition.X - bounds.X;
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.m_currChildLW.ChildWidgets)
    {
      if (childWidget.Widget is ParagraphItem && (childWidget.Widget as ParagraphItem).IsFloatingItem(false) && (childWidget.Widget as ParagraphItem).GetHorizontalOrigin() == HorizontalOrigin.Column)
      {
        switch ((childWidget.Widget as ParagraphItem).GetTextWrappingStyle())
        {
          case TextWrappingStyle.InFrontOfText:
          case TextWrappingStyle.Behind:
            childWidget.Bounds = new RectangleF(childWidget.Bounds.X + num, childWidget.Bounds.Y, childWidget.Bounds.Width, childWidget.Bounds.Height);
            continue;
          default:
            continue;
        }
      }
    }
  }

  private bool SkipUpdatingPosition(WParagraph paragraph)
  {
    return paragraph.IsXpositionUpated || paragraph.ParagraphFormat.IsFrame || this.IsWord2013(paragraph.Document) || !this.IsFirstItemInPage || (this.m_lcOperator as Layouter).IsLayoutingHeaderFooter || (double) this.m_ltWidget.Bounds.X != (double) this.m_currChildLW.Bounds.X || !this.IsBaseFromSection((Entity) paragraph) || paragraph.GetOwnerSection().Columns.Count > 1;
  }

  private bool IsNeedToUpdateIntersectingBounds(WParagraph currentParagraph)
  {
    return ((this.m_lcOperator as Layouter).IsLayoutingHeaderFooter ? (currentParagraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? 1 : 0) : 1) != 0 && !currentParagraph.IsPreviousParagraphMarkIsHidden() && !currentParagraph.IsPreviousParagraphMarkIsInDeletion() && !this.IsInFrame(currentParagraph) && !this.IsInFootnote(currentParagraph) && this.IsBaseFromSection((Entity) currentParagraph);
  }

  private void UpdateItersectingFloatingItemBounds()
  {
    RectangleF bounds = this.m_currChildLW.Bounds;
    List<FloatingItem> floatingItems = new List<FloatingItem>((IEnumerable<FloatingItem>) (this.m_lcOperator as Layouter).FloatingItems);
    FloatingItem.SortFloatingItems(floatingItems, SortPosition.X);
    for (int index = 0; index < floatingItems.Count; ++index)
    {
      FloatingItem floatingItem = floatingItems[index];
      RectangleF floatingItemBounds = RectangleF.Empty;
      if (floatingItem.TextWrappingStyle == TextWrappingStyle.Square)
        floatingItemBounds = floatingItem.TextWrappingBounds;
      else if (floatingItem.TextWrappingStyle == TextWrappingStyle.Tight || floatingItem.TextWrappingStyle == TextWrappingStyle.Through)
        floatingItemBounds = !floatingItem.IsDoesNotDenotesRectangle ? floatingItem.TextWrappingBounds : this.AdjustTightAndThroughBounds(floatingItem, bounds, bounds.Height);
      if (floatingItemBounds != RectangleF.Empty && this.IsYPositionIntersect(floatingItemBounds, bounds, bounds.Height))
        this.m_currChildLW.IntersectingBounds.Add(floatingItemBounds);
    }
  }

  protected override LayoutContext CreateNextChildContext()
  {
    return this.WidgetContainer == null ? (LayoutContext) null : (LayoutContext) new LCContainer(this.WidgetContainer, this.m_lcOperator, this.IsForceFitLayout);
  }

  protected override void MarkAsNotFitted(LayoutContext childContext, bool isFootnote)
  {
    this.IsVerticalNotFitted = childContext.IsVerticalNotFitted;
    if (this.m_bAtLastOneChildFitted)
    {
      if (!this.IsKeepLineTogether(childContext))
      {
        WParagraph paragraph = this.GetParagraph();
        WSection ownerSection = paragraph?.GetOwnerSection();
        ParagraphLayoutInfo layoutInfo = this.LayoutInfo as ParagraphLayoutInfo;
        if (!this.IsLastParagraphNeedToBeLayout(childContext) && !this.m_ltWidget.IsNotFitted)
        {
          if (this.m_notFittedWidget != null)
          {
            if (ownerSection != null && this.SplittedWidget != null)
              this.RemoveAutoHyphenatedString(this.m_notFittedWidget, ownerSection.Document.DOP.AutoHyphen);
            this.m_sptWidget = this.m_notFittedWidget;
            this.m_notFittedWidget = (IWidget) null;
            this.RemoveTrackChangesBalloon(this.m_currChildLW.Bounds.Bottom);
            if ((this.m_lcOperator as Layouter).TrackChangesMarkups.Count > 0)
              this.RemoveCommentMarkUps(this.m_currChildLW.Bounds.Bottom);
          }
          else
          {
            if (paragraph != null && !paragraph.IsInCell)
            {
              LayoutContext layoutContext = LayoutContext.Create(childContext.SplittedWidget, this.m_lcOperator, this.IsForceFitLayout);
              RectangleF rect = new RectangleF((this.m_lcOperator as Layouter).ClientLayoutArea.X, (this.m_lcOperator as Layouter).ClientLayoutArea.Y, (this.m_lcOperator as Layouter).ClientLayoutArea.Width, (this.m_lcOperator as Layouter).ClientLayoutArea.Height);
              layoutContext.IsNeedToWrap = false;
              layoutContext.IsInnerLayouting = true;
              LayoutedWidget layoutedWidget = layoutContext.Layout(rect);
              if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
                return;
              this.UpdateFootnoteWidgets(layoutedWidget);
              layoutedWidget.InitLayoutInfo(false);
              if (this.IsNeedToResetSplitWidget(childContext, paragraph, layoutedWidget.TextTag == "Splitted") || this.IsNeedToSplitPreviousItem(layoutedWidget, childContext))
              {
                LayoutedWidget childWidget = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1];
                if (childWidget.Widget is SplitWidgetContainer)
                {
                  this.m_sptWidget = (IWidget) (childWidget.Widget as SplitWidgetContainer);
                  this.UpdateFootnoteWidgets(childWidget);
                  childWidget.InitLayoutInfo(false);
                  this.m_ltWidget.ChildWidgets.Remove(childWidget);
                  this.IsEndPage(paragraph, paragraph.PreviousSibling == null);
                  if (ownerSection != null && this.SplittedWidget != null)
                    this.RemoveAutoHyphenatedString(this.m_sptWidget, ownerSection.Document.DOP.AutoHyphen);
                  if ((this.m_lcOperator as Layouter).TrackChangesMarkups.Count > 0)
                    this.RemoveCommentMarkUps(this.m_currChildLW.Bounds.Bottom);
                }
                else
                {
                  if (ownerSection != null && this.SplittedWidget != null)
                    this.RemoveAutoHyphenatedString(childContext.SplittedWidget, ownerSection.Document.DOP.AutoHyphen);
                  this.m_sptWidget = childContext.SplittedWidget;
                }
              }
              else if (ownerSection != null && ownerSection.PageSetup.FootnotePosition == FootnotePosition.PrintAtBottomOfPage && (this.m_lcOperator as Layouter).IsFootnoteHeightAdjusted && this.m_ltWidget.ChildWidgets.Count > 2)
              {
                LayoutedWidget childWidget = this.m_ltWidget.ChildWidgets[2];
                if (childWidget.Widget is SplitWidgetContainer)
                {
                  this.m_sptWidget = (IWidget) (childWidget.Widget as SplitWidgetContainer);
                  for (int index = this.m_ltWidget.ChildWidgets.Count - 1; index > 1; --index)
                  {
                    this.UpdateFootnoteWidgets(this.m_ltWidget.ChildWidgets[index]);
                    childWidget.InitLayoutInfo(false);
                    this.RemoveTrackChangesBalloon(this.m_ltWidget.ChildWidgets[index].Bounds.Y);
                    this.m_ltWidget.ChildWidgets.Remove(this.m_ltWidget.ChildWidgets[index]);
                  }
                  this.IsEndPage(paragraph, paragraph.PreviousSibling == null);
                }
                else
                  this.m_sptWidget = childContext.SplittedWidget;
              }
              else
              {
                if (ownerSection != null && ownerSection.Document.DOP.AutoHyphen)
                  this.RemoveAutoHyphenatedString(childContext.SplittedWidget, ownerSection.Document.DOP.AutoHyphen);
                this.m_sptWidget = childContext.SplittedWidget;
              }
            }
            else
              this.m_sptWidget = childContext.SplittedWidget;
            if (paragraph != null && paragraph.IsInCell && !((paragraph.GetOwnerEntity() as WTableCell).OwnerRow.m_layoutInfo as RowLayoutInfo).IsExactlyRowHeight && paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
            {
              LayoutContext layoutContext = LayoutContext.Create(childContext.SplittedWidget, this.m_lcOperator, this.IsForceFitLayout);
              RectangleF rect = new RectangleF(this.m_layoutArea.ClientActiveArea.X, (this.m_lcOperator as Layouter).ClientLayoutArea.Y, this.m_layoutArea.ClientActiveArea.Width, (this.m_lcOperator as Layouter).ClientLayoutArea.Height);
              layoutContext.IsNeedToWrap = false;
              layoutContext.IsInnerLayouting = true;
              LayoutedWidget ltWidget = layoutContext.Layout(rect);
              if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
                return;
              this.UpdateFootnoteWidgets(ltWidget);
              ltWidget.InitLayoutInfo(false);
              if (this.IsNeedToResetSplitWidget(childContext, paragraph, ltWidget.TextTag == "Splitted"))
              {
                if (paragraph.GetIndexInOwnerCollection() == 0)
                  DocumentLayouter.IsEndPage = true;
                else if (this.m_ltWidget.ChildWidgets.Count == 2 && layoutInfo != null)
                {
                  DocumentLayouter.IsEndPage = true;
                  layoutInfo.IsNotFitted = true;
                  if (ownerSection != null && this.SplittedWidget != null)
                    this.RemoveAutoHyphenatedString(childContext.SplittedWidget, ownerSection.Document.DOP.AutoHyphen);
                  this.m_notFittedWidget = childContext.SplittedWidget;
                  this.m_ltWidget.IsNotFitted = true;
                  this.MarkAsSplitted(childContext);
                  return;
                }
              }
            }
          }
          this.m_ltWidget.IsLastItemInPage = true;
          this.m_ltState = LayoutState.Splitted;
        }
        else if (this.m_ltWidget.ChildWidgets.Count == 2 && layoutInfo != null && !this.IsLineContainOnlyNonRenderableItem(this.m_ltWidget.ChildWidgets[1]))
        {
          layoutInfo.IsNotFitted = true;
          if (ownerSection != null && this.SplittedWidget != null)
            this.RemoveAutoHyphenatedString(childContext.SplittedWidget, ownerSection.Document.DOP.AutoHyphen);
          this.m_notFittedWidget = childContext.SplittedWidget;
          this.m_ltWidget.IsNotFitted = true;
          this.MarkAsSplitted(childContext);
        }
        else if (paragraph != null && !paragraph.ParagraphFormat.WidowControl && !this.IsParagraphContainsBookMarksOnly() && !this.IsNeedToNotFitTheItem() || this.IsFirstItemInPage && this.m_ltWidget.ChildWidgets.Count >= 1)
        {
          if (ownerSection != null && this.SplittedWidget != null)
            this.RemoveAutoHyphenatedString(childContext.SplittedWidget, ownerSection.Document.DOP.AutoHyphen);
          this.m_sptWidget = childContext.SplittedWidget;
          this.m_ltWidget.IsLastItemInPage = true;
          this.m_ltState = LayoutState.Splitted;
        }
        else
        {
          this.m_notFittedWidget = (IWidget) null;
          this.m_ltState = LayoutState.NotFitted;
          if (paragraph != null && paragraph.PreviousSibling != null && paragraph.PreviousSibling is WParagraph)
          {
            WParagraph previousSibling = paragraph.PreviousSibling as WParagraph;
            this.IsEndPage(previousSibling, previousSibling.ParagraphFormat.KeepFollow);
          }
          else
            this.IsEndPage(paragraph, paragraph.PreviousSibling == null);
          this.UpdateFootnoteWidgets();
          this.ResetFloatingEntityProperty(this.GetParagraph());
          this.RemoveTrackChangesBalloon(this.m_ltWidget.Bounds.Y);
        }
      }
      else
      {
        this.UpdateFootnoteWidgets();
        this.ResetFloatingEntityProperty(this.GetParagraph());
        this.m_ltState = LayoutState.NotFitted;
      }
    }
    else
    {
      this.UpdateFootnoteWidgets();
      this.m_ltState = LayoutState.NotFitted;
    }
    if (this.m_ltState != LayoutState.Splitted)
      return;
    this.IsFloatingItemFitted(this.m_lcOperator as Layouter, childContext);
  }

  private void RemoveCommentMarkUps(float yPos)
  {
    yPos = (float) Math.Round((double) yPos, 2);
    for (int index = (this.m_lcOperator as Layouter).TrackChangesMarkups.Count - 1; index >= 0; --index)
    {
      float num = (float) Math.Round((double) (this.m_lcOperator as Layouter).TrackChangesMarkups[index].Position.Y + 0.125, 2);
      if ((this.m_lcOperator as Layouter).TrackChangesMarkups[index] is CommentsMarkups)
      {
        if ((double) num < (double) yPos)
          break;
        (this.m_lcOperator as Layouter).TrackChangesMarkups.RemoveAt(index);
      }
    }
  }

  private bool IsNeedToSplitPreviousItem(LayoutedWidget widget, LayoutContext childContext)
  {
    bool splitPreviousItem = false;
    LayoutedWidget layoutedWidget = widget.ChildWidgets.Count > 0 ? widget.ChildWidgets[widget.ChildWidgets.Count - 1] : widget;
    LayoutedWidget childWidget = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1];
    if (widget.TextTag == "Splitted" && layoutedWidget.ChildWidgets.Count > 0 && childWidget.ChildWidgets.Count > 0 && layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1].Widget is Break && (layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1].Widget as Break).BreakType == BreakType.LineBreak && (!(childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1].Widget is Break) || (childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1].Widget as Break).BreakType != BreakType.LineBreak) && (double) this.m_layoutArea.ClientActiveArea.Height < (double) layoutedWidget.Bounds.Height && (double) this.m_layoutArea.ClientActiveArea.Width > (double) childWidget.Bounds.Width + (double) layoutedWidget.Bounds.Width)
      splitPreviousItem = true;
    return splitPreviousItem;
  }

  private void IsFloatingItemFitted(Layouter layouter, LayoutContext childContext)
  {
    if (layouter.FloatingItems.Count <= 0 || this.m_ltWidget.ChildWidgets.Count <= 0)
      return;
    WParagraph paragraph = this.GetParagraph();
    if (paragraph == null || !paragraph.IsFloatingItemsLayouted)
      return;
    while (layouter.FloatingItems.Count > 0)
    {
      FloatingItem floatingItem = layouter.FloatingItems[layouter.FloatingItems.Count - 1];
      if (!floatingItem.IsFloatingItemFit)
      {
        layouter.NotFittedFloatingItems.Add(floatingItem.FloatingEntity);
        (floatingItem.FloatingEntity as ParagraphItem).SetIsWrappingBoundsAdded(false);
        layouter.FloatingItems.Remove(floatingItem);
      }
      else
        break;
    }
    if (layouter.NotFittedFloatingItems.Count <= 0)
      return;
    this.SplitedUpWidget(childContext.SplittedWidget, false);
    this.m_ltState = LayoutState.DynamicRelayout;
    this.m_currChildLW.Owner = this.m_ltWidget;
    paragraph.IsFloatingItemsLayouted = false;
  }

  private void RemoveTrackChangesBalloon(float yPos)
  {
    if (!(this.m_lcOperator as Layouter).IsLayoutingTrackChanges)
      return;
    yPos = (float) Math.Round((double) yPos, 2);
    for (int index = (this.m_lcOperator as Layouter).TrackChangesMarkups.Count - 1; index >= 0 && Math.Round((double) (this.m_lcOperator as Layouter).TrackChangesMarkups[index].Position.Y, 2) >= (double) yPos; --index)
      (this.m_lcOperator as Layouter).TrackChangesMarkups.RemoveAt(index);
  }

  private new void RemoveAutoHyphenatedString(IWidget SplittedWidget, bool isAutoHyphen)
  {
    string str1 = (string) null;
    bool flag1 = false;
    if (isAutoHyphen)
    {
      int index = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].ChildWidgets.Count - 1;
      SplitStringWidget widget = index >= 0 ? this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].ChildWidgets[index].Widget as SplitStringWidget : (SplitStringWidget) null;
      bool flag2 = SplittedWidget is SplitWidgetContainer && (SplittedWidget as SplitWidgetContainer).m_currentChild is SplitStringWidget && widget != null;
      SplitStringWidget currentChild = flag2 ? (SplittedWidget as SplitWidgetContainer).m_currentChild as SplitStringWidget : (SplitStringWidget) null;
      if (currentChild != null && flag2 && widget.SplittedText.EndsWith("-") && !widget.SplittedText.Trim().Equals("-") && !string.IsNullOrEmpty(currentChild.SplittedText))
      {
        str1 = this.GetPeekWord(widget.SplittedText);
        int startIndex = currentChild.StartIndex;
        StringBuilder stringBuilder = new StringBuilder((widget.RealStringWidget as WTextRange).Text);
        stringBuilder.Remove(widget.StartIndex + (widget.SplittedText.Length - 1), 1);
        (widget.RealStringWidget as WTextRange).Text = stringBuilder.ToString();
        currentChild.StartIndex = startIndex - (str1.Length + 1);
        currentChild.Length += str1.Length;
        string str2 = widget.SplittedText.TrimEnd('-').Trim();
        if (str1.Equals(str2))
        {
          this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].ChildWidgets.RemoveAt(index);
        }
        else
        {
          widget.Length -= str1.Length + 1;
          flag1 = true;
        }
      }
    }
    if (string.IsNullOrEmpty(str1))
      return;
    LayoutedWidget childWidget1 = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1];
    RectangleF bounds1 = childWidget1.Bounds;
    LayoutedWidget childWidget2 = childWidget1.ChildWidgets[childWidget1.ChildWidgets.Count - 1];
    RectangleF bounds2 = childWidget2.Bounds;
    WTextRange realStringWidget = childWidget2.Widget is SplitStringWidget ? (childWidget2.Widget as SplitStringWidget).RealStringWidget as WTextRange : (WTextRange) null;
    SizeF sizeF = this.DrawingContext.MeasureString(str1 + "-", realStringWidget.CharacterFormat.GetFontToRender(realStringWidget.ScriptType), (StringFormat) null);
    bounds2.Width -= sizeF.Width;
    bounds1.Width -= sizeF.Width;
    if (flag1)
      this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].ChildWidgets[this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].ChildWidgets.Count - 1].Bounds = new RectangleF(bounds2.X, bounds2.Y, bounds2.Width, bounds2.Height);
    this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Bounds = new RectangleF(bounds1.X, bounds1.Y, bounds1.Width, bounds1.Height);
    this.UpdateltBounds(this.m_ltWidget);
  }

  private void UpdateltBounds(LayoutedWidget widget)
  {
    widget.Bounds = new RectangleF(widget.Bounds.X, widget.Bounds.Y, -widget.Bounds.X + this.GetMaximumRight(widget), -widget.Bounds.Y + this.GetMaximumBottom(widget));
  }

  private float GetMaximumRight(LayoutedWidget widget)
  {
    float right = widget.Bounds.Right;
    for (int index = 0; index < widget.ChildWidgets.Count; ++index)
    {
      if (index == 0)
        right = widget.ChildWidgets[index].Bounds.Right;
      else if ((double) widget.ChildWidgets[index].Bounds.Right > (double) right)
        right = widget.ChildWidgets[index].Bounds.Right;
    }
    return right;
  }

  private float GetMaximumBottom(LayoutedWidget widget)
  {
    float bottom = widget.Bounds.Bottom;
    for (int index = 0; index < widget.ChildWidgets.Count; ++index)
    {
      if (index == 0)
        bottom = widget.ChildWidgets[index].Bounds.Bottom;
      else if ((double) widget.ChildWidgets[index].Bounds.Bottom >= (double) bottom)
        bottom = widget.ChildWidgets[index].Bounds.Bottom;
    }
    return bottom;
  }

  private string GetPeekWord(string hyphenatedLine)
  {
    int num = hyphenatedLine.Length - 1;
    int startIndex = num;
    for (int index = hyphenatedLine.Length - 2; index < hyphenatedLine.Length - 1 && index >= 0; --index)
    {
      switch (hyphenatedLine[index])
      {
        case '\t':
        case '\n':
        case '\r':
        case ' ':
        case '-':
          goto label_4;
        default:
          --startIndex;
          continue;
      }
    }
label_4:
    return startIndex < num && startIndex >= 0 ? hyphenatedLine.Substring(startIndex, num - startIndex) : hyphenatedLine;
  }

  private bool IsLineContainOnlyNonRenderableItem(LayoutedWidget lineWidget)
  {
    for (int index = 0; index < lineWidget.ChildWidgets.Count; ++index)
    {
      WField wfield = (WField) null;
      if (lineWidget.ChildWidgets[index].Widget is WField)
        wfield = lineWidget.ChildWidgets[index].Widget as WField;
      if ((wfield == null || wfield.FieldType != FieldType.FieldPageRef && wfield.FieldType != FieldType.FieldRef) && !(lineWidget.ChildWidgets[index].Widget is BookmarkStart) && !(lineWidget.ChildWidgets[index].Widget is BookmarkEnd))
        return false;
    }
    return true;
  }

  private void IsEndPage(WParagraph paragraph, bool keepFollow)
  {
    if (paragraph == null || !(this.GetBaseEntity((Entity) paragraph) is WSection baseEntity) || paragraph.IsInCell)
      return;
    int columnIndex = this.DrawingContext.GetColumnIndex(baseEntity, this.m_ltWidget.Bounds);
    if (!keepFollow || !paragraph.ParagraphFormat.Keep || columnIndex != baseEntity.Columns.Count - 1)
      return;
    DocumentLayouter.IsEndPage = true;
  }

  private bool IsNeedToResetSplitWidget(
    LayoutContext childContext,
    WParagraph paragraph,
    bool isSplittedLine)
  {
    if (isSplittedLine || !paragraph.ParagraphFormat.WidowControl || this.GetBaseEntity((Entity) paragraph) is WTextBox)
      return false;
    if (!(childContext.SplittedWidget is SplitWidgetContainer))
      return true;
    return !((childContext.SplittedWidget as SplitWidgetContainer).m_currentChild is WChart) && !((childContext.SplittedWidget as SplitWidgetContainer).m_currentChild is Shape) && !((childContext.SplittedWidget as SplitWidgetContainer).m_currentChild is GroupShape) && !((childContext.SplittedWidget as SplitWidgetContainer).m_currentChild is WOleObject);
  }

  private void UpdateFootnoteWidgets()
  {
    if ((this.m_lcOperator as Layouter).IsLayoutingFootnote)
      return;
    WParagraph paragraph = this.GetParagraph();
    if (paragraph == null)
      return;
    this.UpdateFootnoteWidgets(paragraph);
  }

  private bool IsKeepLineTogether(LayoutContext childContext)
  {
    ParagraphLayoutInfo layoutInfo = this.LayoutInfo as ParagraphLayoutInfo;
    bool flag = false;
    if (layoutInfo != null)
    {
      flag = layoutInfo.IsKeepTogether;
      WParagraph paragraph = this.GetParagraph();
      if (paragraph != null && this.IsInFrame(paragraph) || this.IsFirstItemInPage)
        flag = false;
    }
    return flag;
  }

  private bool IsLastParagraphNeedToBeLayout(LayoutContext childContext)
  {
    if (this.LayoutInfo is ParagraphLayoutInfo)
    {
      WParagraph paragraph = this.GetParagraph();
      if ((paragraph == null || !paragraph.IsInCell ? (paragraph.IsInCell || paragraph.OwnerBase != null && paragraph.OwnerBase.OwnerBase is WTextBox || this.IsInFrame(paragraph) || !(childContext.Widget is SplitWidgetContainer) ? 0 : (!this.IsForceFitLayout ? (this.m_ltWidget.ChildWidgets.Count < 3 ? 1 : 0) : (this.m_ltWidget.ChildWidgets.Count == 1 ? 1 : 0))) : (!this.IsWord2013(paragraph.Document) ? 0 : (this.IsNeedToLayout(paragraph.GetOwnerTableCell(paragraph.OwnerTextBody)) ? 1 : 0))) != 0)
        return true;
    }
    return false;
  }

  private bool IsNeedToLayout(WTableCell ownerTableCell)
  {
    return !this.IsForceFitLayout && this.m_ltWidget.ChildWidgets.Count < 2;
  }

  private bool IsParagraphContainsBookMarksOnly()
  {
    for (int index1 = 0; index1 < this.m_ltWidget.ChildWidgets.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.m_ltWidget.ChildWidgets[index1].ChildWidgets.Count; ++index2)
      {
        if (!(this.m_ltWidget.ChildWidgets[index1].ChildWidgets[index2].Widget is BookmarkStart) && !(this.m_ltWidget.ChildWidgets[index1].ChildWidgets[index2].Widget is BookmarkEnd))
          return false;
      }
    }
    return true;
  }

  private bool IsNeedToNotFitTheItem()
  {
    return this.CurrentChildWidget is ParagraphItem && this.m_ltWidget.ChildWidgets.Count == 1 && this.m_ltWidget.ChildWidgets[0].ChildWidgets.Count > 0 && !(this.CurrentChildWidget as ParagraphItem).IsFloatingItem(false) && this.m_ltWidget.ChildWidgets[0].ChildWidgets[this.m_ltWidget.ChildWidgets[0].ChildWidgets.Count - 1].Widget is ParagraphItem && (this.m_ltWidget.ChildWidgets[0].ChildWidgets[this.m_ltWidget.ChildWidgets[0].ChildWidgets.Count - 1].Widget as ParagraphItem).IsFloatingItem(false);
  }

  protected override void MarkAsFitted(LayoutContext childContext)
  {
    if (childContext.LayoutInfo is ParagraphLayoutInfo layoutInfo && this.m_ltWidget.IsNotFitted)
    {
      (this.LayoutInfo as ParagraphLayoutInfo).IsNotFitted = false;
      if (this.m_ltWidget.Widget is SplitWidgetContainer)
        this.m_ltWidget.IsNotFitted = false;
      this.MarkAsNotFitted(childContext, false);
    }
    else
    {
      this.AddChildLW(childContext);
      if (childContext is LCContainer && !(childContext is LCLineContainer) && this.m_currChildLW.IsNotFitted)
        return;
      WParagraph paragraph = this.GetParagraph();
      bool flag = layoutInfo != null && layoutInfo.IsPageBreak;
      if (flag && paragraph != null && paragraph.IsParagraphHasSectionBreak() && (paragraph.BreakCharacterFormat.Hidden || paragraph.BreakCharacterFormat.IsDeleteRevision) && (!paragraph.IsEmptyParagraph() || paragraph.PreviousSibling == null || paragraph.IsPreviousParagraphMarkIsHidden() || paragraph.IsPreviousParagraphMarkIsInDeletion()))
        flag = false;
      this.m_ltState = LayoutState.Fitted;
      if (flag)
      {
        this.m_layoutArea.CutFromTop();
        this.m_ltState = LayoutState.Breaked;
      }
    }
    this.UpdateForceFitLayoutState(childContext);
  }

  protected override void MarkAsSplitted(LayoutContext childContext)
  {
    if (this.LayoutInfo is ParagraphLayoutInfo layoutInfo && this.m_ltWidget.IsNotFitted && this.m_ltWidget.ChildWidgets.Count > 2)
    {
      layoutInfo.IsNotFitted = false;
      this.m_ltWidget.IsNotFitted = false;
      this.MarkAsNotFitted(childContext, false);
    }
    else
    {
      this.AddChildLW(childContext);
      if (childContext is LCContainer && !(childContext is LCLineContainer) && this.m_currChildLW.IsNotFitted)
        return;
      this.m_widget = childContext.SplittedWidget;
      this.m_bAtLastOneChildFitted = true;
      if (layoutInfo != null)
        layoutInfo.IsFirstLine = false;
    }
    this.UpdateForceFitLayoutState(childContext);
  }

  protected override void UpdateClientArea()
  {
    this.ChangeChildsAlignment();
    if (this.m_currChildLW.IsNotFitted)
      return;
    RectangleF bounds = this.m_currChildLW.Bounds;
    float footnoteHeight = this.GetFootnoteHeight();
    double y = (double) bounds.Bottom;
    if (this.m_currChildLW.Widget is WParagraph && ((this.m_currChildLW.Widget as WParagraph).IsPreviousParagraphMarkIsHidden() || (this.m_currChildLW.Widget as WParagraph).IsPreviousParagraphMarkIsInDeletion() || (this.m_lcOperator as Layouter).FieldEntity is WFieldMark) && this.m_currChildLW.TextTag == "Splitted")
    {
      float pageMarginLeft = this.GetPageMarginLeft(this.m_currChildLW.Widget as WParagraph);
      if ((double) pageMarginLeft != (double) this.m_currChildLW.Bounds.X)
        this.m_layoutArea.UpdateLeftPosition(pageMarginLeft);
      if ((double) (this.m_lcOperator as Layouter).HiddenLineBottom != 0.0 && (double) (this.m_lcOperator as Layouter).HiddenLineBottom > y && !((this.m_lcOperator as Layouter).FieldEntity is WFieldMark))
        y = (double) (this.m_lcOperator as Layouter).HiddenLineBottom;
      (this.m_lcOperator as Layouter).HiddenLineBottom = 0.0f;
    }
    if ((double) (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems != -3.4028234663852886E+38 && (double) (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems != 3.4028234663852886E+38)
    {
      this.m_layoutArea.UpdateLeftPosition((this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems);
      (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems = float.MaxValue;
    }
    else
    {
      if ((double) (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems == 3.4028234663852886E+38)
      {
        RectangleF clientActiveArea = this.m_layoutArea.ClientActiveArea;
        clientActiveArea.Width += clientActiveArea.X - (this.m_lcOperator as Layouter).ClientLayoutArea.X;
        clientActiveArea.X = (this.m_lcOperator as Layouter).ClientLayoutArea.X;
        this.m_layoutArea.UpdateClientActiveArea(clientActiveArea);
        (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems = float.MinValue;
      }
      this.m_layoutArea.CutFromTop(y, footnoteHeight);
      (this.m_lcOperator as Layouter).m_canSplitbyCharacter = true;
      (this.m_lcOperator as Layouter).m_canSplitByTab = false;
      (this.m_lcOperator as Layouter).IsFirstItemInLine = true;
    }
  }

  private bool IsRTLChar(char character)
  {
    if (character >= '\u0590' && character <= '\u05FF' || character >= '\u0600' && character <= 'ۿ' || character >= 'ݐ' && character <= 'ݿ' || character >= 'ࢠ' && character <= 'ࣿ' || character >= 'ﭐ' && character <= '\uFDFF' || character >= 'ﹰ' && character <= '\uFEFF' || character >= 'ꦀ' && character <= '꧟' || character >= '܀' && character <= 'ݏ' || character >= 'ހ' && character <= '\u07BF' || character >= 'ࡀ' && character <= '\u085F' || character >= '߀' && character <= '\u07FF' || character >= 'ࠀ' && character <= '\u083F')
      return true;
    return character >= 'ⴰ' && character <= '⵿';
  }

  private bool IsRTLText(string text)
  {
    if (text != null)
    {
      for (int index = 0; index < text.Length; ++index)
      {
        if (this.IsRTLChar(text[index]))
          return true;
      }
    }
    return false;
  }

  private bool IsLineContainsRTL()
  {
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.m_currChildLW.ChildWidgets)
    {
      if ((childWidget.Widget is WTextRange || childWidget.Widget is SplitStringWidget) && this.IsRTLText(childWidget.Widget is SplitStringWidget ? (childWidget.Widget as SplitStringWidget).SplittedText : (childWidget.Widget as WTextRange).Text))
        return true;
    }
    return false;
  }

  protected override void ChangeChildsAlignment()
  {
    WParagraph paragraph1 = this.GetParagraph();
    if ((this.m_currChildLW.Widget is WParagraph && this.m_currChildLW.TextTag != "Splitted" && this.m_currChildLW.ChildWidgets.Count > 0 || this.m_currChildLW.Widget is SplitWidgetContainer && (this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph && this.m_currChildLW.TextTag != "Splitted" && this.m_currChildLW.ChildWidgets.Count > 0) && !this.IsInnerLayouting)
      this.RemoveSplitStringWidget(paragraph1);
    float bottom = this.m_currChildLW.Bounds.Bottom;
    this.m_currChildLW.AlignBottom(this.DrawingContext, this.m_layoutArea.ClientActiveArea.Height, this.m_layoutArea.ClientActiveArea.Bottom, (this.m_lcOperator as Layouter).IsRowFitInSamePage, (this.m_lcOperator as Layouter).IsLayoutingHeaderRow, (this.m_lcOperator as Layouter).IsLayoutingHeaderFooter, this.IsForceFitLayout);
    this.ShiftTrackChangesBalloons(this.m_currChildLW.Bounds.Y, this.m_currChildLW.Bounds.Bottom, bottom);
    if (this.m_currChildLW.IsNotFitted || this.IsDisplayMath())
      return;
    this.DrawingContext.UpdateTabPosition(this.m_currChildLW, this.m_layoutArea.ClientActiveArea);
    bool flag1 = false;
    if (this.m_currChildLW.ChildWidgets.Count > 0)
    {
      WTextRange wtextRange = (WTextRange) null;
      LayoutedWidget layoutedWidget = (LayoutedWidget) null;
      for (int index = this.m_currChildLW.ChildWidgets.Count - 1; index >= 0; --index)
      {
        layoutedWidget = this.m_currChildLW.ChildWidgets[index];
        if (layoutedWidget.Widget is WTextRange || layoutedWidget.Widget is SplitStringWidget)
        {
          wtextRange = layoutedWidget.Widget is SplitStringWidget ? (layoutedWidget.Widget as SplitStringWidget).RealStringWidget as WTextRange : layoutedWidget.Widget as WTextRange;
          if (wtextRange.Text == '\u001F'.ToString())
          {
            if (wtextRange.OwnerParagraph != null && wtextRange.OwnerParagraph.Text.Replace('\u001F'.ToString(), string.Empty) != string.Empty)
            {
              flag1 = true;
              break;
            }
            break;
          }
          if (wtextRange.Text != string.Empty)
            break;
        }
      }
      if (flag1 && wtextRange != null && layoutedWidget != null)
      {
        StringFormat format = new StringFormat(this.DrawingContext.StringFormt);
        if (wtextRange.CharacterFormat.Bidi)
          format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
        else
          format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
        layoutedWidget.Bounds = new RectangleF(layoutedWidget.Bounds.X, layoutedWidget.Bounds.Y, this.DrawingContext.MeasureString("-", layoutedWidget.Widget.LayoutInfo.Font.GetFont(wtextRange.Document), format).Width, layoutedWidget.Bounds.Height);
        int num = this.m_currChildLW.ChildWidgets.IndexOf(layoutedWidget);
        if (num != this.m_currChildLW.ChildWidgets.Count - 1)
        {
          for (int index = num + 1; index < this.m_currChildLW.ChildWidgets.Count; ++index)
            this.m_currChildLW.ChildWidgets[index].ShiftLocation((double) layoutedWidget.Bounds.Width, 0.0, false, false);
        }
      }
    }
    bool isLastLine = false;
    HAlignment alignment = this.m_currChildLW.Widget.LayoutInfo is ParagraphLayoutInfo layoutInfo ? layoutInfo.Justification : HAlignment.Left;
    if ((this.m_lcOperator as Layouter).UnknownField != null && (this.m_lcOperator as Layouter).UnknownField.OwnerParagraph.m_layoutInfo is ParagraphLayoutInfo)
      alignment = ((this.m_lcOperator as Layouter).UnknownField.OwnerParagraph.m_layoutInfo as ParagraphLayoutInfo).Justification;
    double subWidth = 0.0;
    double subWidthBeforeSpaceTrim = 0.0;
    float rightMargin = layoutInfo.Margins.Right;
    WParagraph paragraph2 = this.GetParagraph();
    if (paragraph2 != null && !(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && paragraph2.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013)
    {
      bool flag2 = false;
      if (this.m_currChildLW.IntersectingBounds.Count <= 0)
      {
        this.UpdateItersectingFloatingItemBounds();
        flag2 = this.m_currChildLW.IntersectingBounds.Count > 0;
      }
      bool flag3 = false;
      List<RectangleF> intersectingBounds = this.m_currChildLW.IntersectingBounds;
      for (int index = 0; index < intersectingBounds.Count; ++index)
      {
        if ((double) this.m_currChildLW.Bounds.Right < (double) intersectingBounds[index].X)
        {
          flag3 = true;
          break;
        }
      }
      if (flag3)
        rightMargin = 0.0f;
      if (flag2 && !paragraph2.Document.IsNeedToAddLineNumbers())
        this.m_currChildLW.IntersectingBounds.Clear();
    }
    float num1 = 0.0f;
    float xPosition = 0.0f;
    LayoutedWidget m_backupWidget = new LayoutedWidget(this.m_currChildLW);
    LayoutedWidget layoutedWidget1 = new LayoutedWidget(this.m_currChildLW);
    layoutedWidget1.ChildWidgets.Clear();
    RectangleF interSectingFloattingItem = new RectangleF(0.0f, 0.0f, 0.0f, 0.0f);
    bool flag4 = paragraph1 != null && this.IsInFrame(paragraph1);
    float num2 = 0.0f;
    while (m_backupWidget.ChildWidgets.Count > 0)
    {
      this.SplitLineBasedOnInterSectingFlotingEntity(m_backupWidget, ref interSectingFloattingItem, layoutedWidget1);
      RectangleF rectangleF;
      if (this.m_currChildLW.ChildWidgets.Count > 0)
      {
        for (int index = 0; index < this.m_currChildLW.ChildWidgets.Count; ++index)
        {
          EntityType entityType = EntityType.WordDocument;
          if (this.m_currChildLW.ChildWidgets[index].Widget is SplitStringWidget)
            entityType = EntityType.TextRange;
          else if (this.m_currChildLW.ChildWidgets[index].Widget is Entity)
            entityType = (this.m_currChildLW.ChildWidgets[index].Widget as Entity).EntityType;
          if (this.m_currChildLW.ChildWidgets[index].Widget is WTextRange)
            entityType = EntityType.TextRange;
          switch (entityType)
          {
            case EntityType.TextRange:
              rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
              if ((double) rectangleF.Width < 0.0)
              {
                rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                num1 = rectangleF.Left;
                rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                xPosition = rectangleF.X;
              }
              else
              {
                rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                num1 = rectangleF.Right;
                rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                xPosition = rectangleF.X;
              }
              if (flag4)
              {
                double num3 = (double) num2;
                rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                double width = (double) rectangleF.Width;
                num2 = (float) (num3 + width);
                break;
              }
              break;
            case EntityType.Picture:
            case EntityType.OleObject:
              WPicture wpicture = this.m_currChildLW.ChildWidgets[index].Widget is WOleObject ? (this.m_currChildLW.ChildWidgets[index].Widget as WOleObject).OlePicture : this.m_currChildLW.ChildWidgets[index].Widget as WPicture;
              if (wpicture != null && wpicture.TextWrappingStyle == TextWrappingStyle.Inline)
              {
                rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                num1 = rectangleF.Right;
                if (flag4)
                {
                  double num4 = (double) num2;
                  rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                  double width = (double) rectangleF.Width;
                  num2 = (float) (num4 + width);
                  break;
                }
                break;
              }
              break;
            case EntityType.TextBox:
              if ((this.m_currChildLW.ChildWidgets[index].Widget as WTextBox).TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Inline)
              {
                rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                num1 = rectangleF.Right;
                if (flag4)
                {
                  double num5 = (double) num2;
                  rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                  double width = (double) rectangleF.Width;
                  num2 = (float) (num5 + width);
                  break;
                }
                break;
              }
              break;
            case EntityType.Symbol:
              num1 = this.m_currChildLW.ChildWidgets[index].Bounds.Right;
              if (flag4)
              {
                num2 += this.m_currChildLW.ChildWidgets[index].Bounds.Width;
                break;
              }
              break;
            case EntityType.Chart:
              if (this.m_currChildLW.ChildWidgets[index].Widget is WChart widget1 && widget1.WrapFormat.TextWrappingStyle == TextWrappingStyle.Inline)
              {
                rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                num1 = rectangleF.Right;
                if (flag4)
                {
                  double num6 = (double) num2;
                  rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                  double width = (double) rectangleF.Width;
                  num2 = (float) (num6 + width);
                  break;
                }
                break;
              }
              break;
            case EntityType.AutoShape:
              if (this.m_currChildLW.ChildWidgets[index].Widget is Shape widget2 && widget2.WrapFormat.TextWrappingStyle == TextWrappingStyle.Inline)
              {
                rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                num1 = rectangleF.Right;
                if (flag4)
                {
                  double num7 = (double) num2;
                  rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                  double width = (double) rectangleF.Width;
                  num2 = (float) (num7 + width);
                  break;
                }
                break;
              }
              break;
            case EntityType.GroupShape:
              if (this.m_currChildLW.ChildWidgets[index].Widget is GroupShape widget3 && widget3.WrapFormat.TextWrappingStyle == TextWrappingStyle.Inline)
              {
                rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
                num1 = rectangleF.Right;
                break;
              }
              break;
            case EntityType.Math:
              rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
              num1 = rectangleF.Right;
              break;
          }
          if ((double) num1 == 0.0 && layoutInfo.ListValue != string.Empty)
          {
            rectangleF = this.m_currChildLW.ChildWidgets[index].Bounds;
            num1 = rectangleF.Right;
          }
        }
        if ((double) interSectingFloattingItem.Bottom != 0.0)
        {
          subWidth = (double) interSectingFloattingItem.X - (double) num1 - (double) rightMargin;
        }
        else
        {
          rectangleF = this.m_layoutArea.ClientActiveArea;
          subWidth = (double) rectangleF.Right - (double) num1 - (double) rightMargin;
        }
      }
      else if ((double) interSectingFloattingItem.Bottom != 0.0)
      {
        double x = (double) interSectingFloattingItem.X;
        rectangleF = this.m_currChildLW.Bounds;
        double right = (double) rectangleF.Right;
        subWidth = x - right - (double) rightMargin;
      }
      else
      {
        rectangleF = this.m_layoutArea.ClientActiveArea;
        double right1 = (double) rectangleF.Right;
        rectangleF = this.m_currChildLW.Bounds;
        double right2 = (double) rectangleF.Right;
        subWidth = right1 - right2 - (double) rightMargin;
      }
      if ((double) interSectingFloattingItem.Bottom == 0.0 && alignment != HAlignment.Right)
        this.UpdateSubWidthBasedOnTextWrap(paragraph1, ref subWidth, xPosition, rightMargin);
      if (paragraph1 != null && this.IsInFrame(paragraph1))
      {
        if ((double) paragraph1.ParagraphFormat.FrameWidth == 0.0)
        {
          if (paragraph1.ParagraphFormat.IsNextParagraphInSameFrame() || paragraph1.ParagraphFormat.IsPreviousParagraphInSameFrame())
          {
            rectangleF = this.m_layoutArea.ClientActiveArea;
            double right3 = (double) rectangleF.Right;
            rectangleF = this.m_currChildLW.Bounds;
            double right4 = (double) rectangleF.Right;
            subWidth = right3 - right4 - (double) rightMargin;
          }
          else
            subWidth = 0.0;
        }
        else
        {
          if ((double) paragraph1.ParagraphFormat.FirstLineIndent < 0.0)
          {
            double num8 = (double) paragraph1.ParagraphFormat.FrameWidth - (double) paragraph1.ParagraphFormat.FirstLineIndent;
            rectangleF = this.m_currChildLW.Bounds;
            double width = (double) rectangleF.Width;
            subWidth = num8 - width - (double) rightMargin;
          }
          else
            subWidth = (double) paragraph1.ParagraphFormat.FrameWidth - (double) num2;
          for (int index = 0; index < layoutedWidget1.ChildWidgets.Count; ++index)
          {
            double num9 = subWidth;
            rectangleF = layoutedWidget1.ChildWidgets[index].Bounds;
            double width = (double) rectangleF.Width;
            subWidth = num9 - width;
          }
        }
      }
      if (subWidth < 0.0)
      {
        if ((double) (this.m_lcOperator as Layouter).MaxRightPositionOfTabStopInterSectingFloattingItems != -3.4028234663852886E+38)
        {
          subWidth = (double) (this.m_lcOperator as Layouter).MaxRightPositionOfTabStopInterSectingFloattingItems - (double) num1;
          (this.m_lcOperator as Layouter).MaxRightPositionOfTabStopInterSectingFloattingItems = float.MinValue;
        }
        else if (this.m_currChildLW.ChildWidgets.Count > 0 && this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].TextTag == "IsLastWordFit")
          this.m_currChildLW.ChildWidgets[0].TextTag = (string) null;
        else if (this.IsNotWord2013Jusitfy(paragraph1))
          subWidth = 0.0;
      }
      if ((this.m_currChildLW.Widget is WParagraph && this.m_currChildLW.TextTag != "Splitted" && this.m_currChildLW.ChildWidgets.Count > 0 || this.m_currChildLW.Widget is SplitWidgetContainer && (this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph && this.m_currChildLW.TextTag != "Splitted" && this.m_currChildLW.ChildWidgets.Count > 0) && this.m_currChildLW.ChildWidgets.Count > 0 && alignment == HAlignment.Justify && ((double) interSectingFloattingItem.Bottom == 0.0 || m_backupWidget.ChildWidgets.Count == 0))
        isLastLine = subWidth >= 0.0 || !this.IsWord2013(paragraph1.Document);
      float num10 = 0.0f;
      WParagraphFormat currentTabFormat = this.DrawingContext.GetCurrentTabFormat(paragraph1);
      if (this.m_currChildLW.ChildWidgets.Count > 0 && paragraph1.IsLastLine(this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1]) && paragraph1.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && paragraph1.Document.DOP.Dop2000.Copts.ForgetLastTabAlign && currentTabFormat != null && currentTabFormat.Tabs != null && currentTabFormat.Tabs.Count > 0 && subWidth > 0.0)
        num10 = this.GetLastTabWidth(currentTabFormat, currentTabFormat.Tabs.Count);
      if ((!(this.m_currChildLW.Widget is WParagraph) || !this.IsTOC(this.m_currChildLW.Widget as WParagraph)) && !isLastLine)
      {
        switch (alignment)
        {
          case HAlignment.Center:
            if ((double) interSectingFloattingItem.Bottom != 0.0 && (double) num1 == 0.0 && m_backupWidget.ChildWidgets.Count > 0)
            {
              double num11 = subWidth;
              rectangleF = m_backupWidget.Bounds;
              double width = (double) rectangleF.Width;
              subWidth = num11 - width;
            }
            subWidth = subWidth / 2.0 + (double) num10 / 2.0;
            this.m_currChildLW.AlignCenter(this.DrawingContext, subWidth, true);
            break;
          case HAlignment.Right:
            if ((double) interSectingFloattingItem.Bottom != 0.0 && (double) num1 == 0.0)
            {
              double width1 = (double) interSectingFloattingItem.Width;
              rectangleF = this.m_currChildLW.Bounds;
              double width2 = (double) rectangleF.Width;
              subWidth = width1 - width2 - (double) rightMargin;
              if (m_backupWidget.ChildWidgets.Count > 0)
              {
                double num12 = subWidth;
                rectangleF = m_backupWidget.Bounds;
                double width3 = (double) rectangleF.Width;
                subWidth = num12 - width3;
              }
            }
            subWidth += (double) num10;
            subWidthBeforeSpaceTrim = subWidth;
            subWidth = this.m_currChildLW.AlignRight(this.DrawingContext, subWidth, false);
            break;
          case HAlignment.Justify:
          case HAlignment.Distributed:
            if (layoutedWidget1.ChildWidgets.Count > 0 || (double) interSectingFloattingItem.Bottom != 0.0)
              this.m_currChildLW.AlignJustify(this.DrawingContext, subWidth, layoutedWidget1.ChildWidgets.Count > 0, false);
            for (int index = 0; index < this.m_currChildLW.ChildWidgets.Count; ++index)
              this.m_currChildLW.ChildWidgets[index].HorizontalAlign = alignment;
            break;
        }
      }
      if ((double) interSectingFloattingItem.Bottom != 0.0)
      {
        layoutedWidget1.ChildWidgets.AddRange((IEnumerable<LayoutedWidget>) this.m_currChildLW.ChildWidgets);
        num1 = 0.0f;
      }
      else
      {
        if (layoutedWidget1.ChildWidgets.Count > 0)
        {
          layoutedWidget1.ChildWidgets.AddRange((IEnumerable<LayoutedWidget>) this.m_currChildLW.ChildWidgets);
          break;
        }
        break;
      }
    }
    bool flag5 = false;
    bool isContainsRTL = false;
    LayoutedWidgetList layoutedWidgetList = (LayoutedWidgetList) null;
    if (!paragraph1.Document.Settings.CompatibilityOptions[CompatibilityOption.ExpShRtn] && (this.m_currChildLW.Widget.LayoutInfo as ParagraphLayoutInfo).Justification == HAlignment.Justify && this.m_currChildLW.ChildWidgets.Count > 0 && this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].Widget is Break && (this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].Widget as Break).BreakType == BreakType.LineBreak)
    {
      for (int index = 0; index < this.m_currChildLW.ChildWidgets.Count; ++index)
        this.m_currChildLW.ChildWidgets[index].HorizontalAlign = HAlignment.Left;
      alignment = HAlignment.Left;
    }
    if (layoutedWidget1.ChildWidgets.Count > 0)
    {
      flag5 = this.ShiftWidgetsForRTLLayouting(layoutedWidget1);
    }
    else
    {
      isContainsRTL = this.IsLineContainsRTL();
      layoutedWidgetList = this.m_currChildLW.ChildWidgets;
      bool isParaBidi = this.ShiftWidgetsForRTLLayouting(subWidth, subWidthBeforeSpaceTrim, alignment, layoutedWidget1.ChildWidgets.Count > 0, isLastLine, isContainsRTL);
      if ((!(this.m_currChildLW.Widget is WParagraph) || !this.IsTOC(this.m_currChildLW.Widget as WParagraph)) && !isLastLine && (alignment == HAlignment.Justify || alignment == HAlignment.Distributed))
        this.m_currChildLW.AlignJustify(this.DrawingContext, subWidth, layoutedWidget1.ChildWidgets.Count > 0, isParaBidi);
    }
    if (layoutedWidget1.ChildWidgets.Count > 0)
    {
      this.m_currChildLW.ChildWidgets.RemoveRange(0, this.m_currChildLW.ChildWidgets.Count);
      for (int index = 0; index < layoutedWidget1.ChildWidgets.Count; ++index)
      {
        this.m_currChildLW.ChildWidgets.Add(layoutedWidget1.ChildWidgets[index]);
        layoutedWidget1.ChildWidgets[index].Owner = this.m_currChildLW;
      }
      this.m_ltWidget.ChildWidgets.RemoveAt(this.m_ltWidget.ChildWidgets.Count - 1);
      this.m_ltWidget.ChildWidgets.Add(this.m_currChildLW);
      this.m_currChildLW.Owner = this.m_ltWidget;
    }
    else if (layoutedWidgetList != null)
      this.m_currChildLW.ChildWidgets = layoutedWidgetList;
    if (paragraph1 == null || paragraph1.Document.RevisionOptions.CommentDisplayMode != CommentDisplayMode.ShowInBalloons || (this.m_lcOperator as Layouter).IsLayoutingTrackChanges || alignment != HAlignment.Center && alignment != HAlignment.Right && alignment != HAlignment.Justify && alignment != HAlignment.Distributed && !isContainsRTL)
      return;
    this.UpdateXPositionOfCommentBalloon();
  }

  private void UpdateXPositionOfCommentBalloon()
  {
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.m_currChildLW.ChildWidgets)
    {
      if (childWidget.Widget is WCommentMark)
      {
        WCommentMark widget = childWidget.Widget as WCommentMark;
        if (widget.Type == CommentMarkType.CommentEnd || widget.Type == CommentMarkType.CommentStart && widget.Comment != null && widget.Comment.CommentRangeEnd == null)
        {
          foreach (TrackChangesMarkups trackChangesMarkup in (this.m_lcOperator as Layouter).TrackChangesMarkups)
          {
            if (trackChangesMarkup is CommentsMarkups && (trackChangesMarkup as CommentsMarkups).CommentID == widget.CommentId)
              (trackChangesMarkup as CommentsMarkups).Position = new PointF(childWidget.Bounds.X, trackChangesMarkup.Position.Y);
          }
        }
      }
    }
  }

  private bool IsDisplayMath()
  {
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.m_currChildLW.ChildWidgets)
    {
      if ((!(childWidget.Widget is WMath) || (childWidget.Widget as WMath).IsInline) && !(childWidget.Widget is BookmarkStart) && !(childWidget.Widget is BookmarkEnd))
        return false;
    }
    return this.m_currChildLW.ChildWidgets.Count > 0;
  }

  private bool HasTextRangeBidi(LayoutedWidgetList layoutedWidgets)
  {
    foreach (LayoutedWidget layoutedWidget in (List<LayoutedWidget>) layoutedWidgets)
    {
      if ((layoutedWidget.Widget is WTextRange || layoutedWidget.Widget is SplitStringWidget) && (layoutedWidget.Widget is SplitStringWidget ? (layoutedWidget.Widget as SplitStringWidget).RealStringWidget as WTextRange : layoutedWidget.Widget as WTextRange).CharacterFormat.Bidi)
        return true;
    }
    return false;
  }

  private void UpdateCharacterRange(
    int i,
    int rtlStartIndex,
    List<bool> splittedWidgetBidiValues,
    ref List<CharacterRangeType> characterRangeTypes)
  {
    int num = i;
    if (!splittedWidgetBidiValues[i])
    {
      if (characterRangeTypes[i] == CharacterRangeType.LTR)
        --num;
      for (int index = num; index >= rtlStartIndex; --index)
      {
        if (characterRangeTypes[index] != CharacterRangeType.WordSplit)
        {
          num = index;
          break;
        }
      }
    }
    for (int index1 = rtlStartIndex; index1 <= num; ++index1)
    {
      if (characterRangeTypes[index1] == CharacterRangeType.WordSplit)
      {
        characterRangeTypes[index1] = CharacterRangeType.RTL | CharacterRangeType.WordSplit;
        int index2 = index1 - 1;
        int index3 = index1 + 1;
        if (index2 >= 0 && index3 < characterRangeTypes.Count && characterRangeTypes[index2] == CharacterRangeType.RTL && (characterRangeTypes[index3] == CharacterRangeType.RTL || characterRangeTypes[index3] == CharacterRangeType.Number) && this.m_currChildLW.ChildWidgets[index1].Widget is WTextRange)
        {
          IWTextRange widget = (IWTextRange) this.m_currChildLW.ChildWidgets[index1].Widget;
          if (widget.CharacterFormat.FontNameFarEast == "Times New Roman")
          {
            char[] charArray = widget.Text.ToCharArray();
            Array.Reverse((Array) charArray);
            widget.Text = new string(charArray);
          }
        }
      }
    }
  }

  private LayoutedWidget GetNextValidWidget(int startIndex, LayoutedWidgetList layoutedWidgets)
  {
    int index = startIndex;
    if (index >= layoutedWidgets.Count)
      return (LayoutedWidget) null;
    LayoutedWidget layoutedWidget = layoutedWidgets[index];
    return layoutedWidgets[index];
  }

  private bool ShiftWidgetsForRTLLayouting(
    double subWidth,
    double subWidthBeforeSpaceTrim,
    HAlignment alignment,
    bool hasIntersectingFloatingItem,
    bool isLastLine,
    bool isContainsRTL)
  {
    bool isAutoFrame = false;
    bool paraBidi = false;
    bool flag = this.HasTextRangeBidi(this.m_currChildLW.ChildWidgets);
    if (this.m_currChildLW.Widget is WParagraph || this.m_currChildLW.Widget is SplitWidgetContainer && (this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
    {
      if (this.m_currChildLW.Widget is WParagraph)
      {
        WParagraph widget = this.m_currChildLW.Widget as WParagraph;
        isAutoFrame = widget.ParagraphFormat.IsFrame && (double) widget.ParagraphFormat.FrameWidth == 0.0;
        paraBidi = widget.ParagraphFormat.Bidi;
      }
      else
        paraBidi = ((this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph).ParagraphFormat.Bidi;
    }
    if (isContainsRTL || paraBidi || flag)
    {
      List<CharacterRangeType> characterRangeTypes = new List<CharacterRangeType>();
      List<bool> splittedWidgetBidiValues = new List<bool>();
      for (int index = 0; index < this.m_currChildLW.ChildWidgets.Count; ++index)
      {
        LayoutedWidget childWidget = this.m_currChildLW.ChildWidgets[index];
        if ((childWidget.Widget is WTextRange || childWidget.Widget is SplitStringWidget) && (double) childWidget.Bounds.Height > 0.0)
        {
          WTextRange wtextRange = childWidget.Widget is SplitStringWidget ? (childWidget.Widget as SplitStringWidget).RealStringWidget as WTextRange : childWidget.Widget as WTextRange;
          splittedWidgetBidiValues.Add(wtextRange.CharacterFormat.Bidi);
          if (childWidget.Widget.LayoutInfo is TabsLayoutInfo)
            characterRangeTypes.Add(CharacterRangeType.Tab);
          else
            characterRangeTypes.Add(wtextRange.CharacterRange);
        }
        else if (childWidget.Widget is WCommentMark)
        {
          WCommentMark widget = childWidget.Widget as WCommentMark;
          if (widget.Type == CommentMarkType.CommentStart && index < this.m_currChildLW.ChildWidgets.Count - 1)
          {
            LayoutedWidget nextValidWidget = this.GetNextValidWidget(index + 1, this.m_currChildLW.ChildWidgets);
            if (nextValidWidget != null && (nextValidWidget.Widget is WTextRange || nextValidWidget.Widget is SplitStringWidget) && (double) nextValidWidget.Bounds.Height > 0.0)
            {
              WTextRange wtextRange = nextValidWidget.Widget is SplitStringWidget ? (nextValidWidget.Widget as SplitStringWidget).RealStringWidget as WTextRange : nextValidWidget.Widget as WTextRange;
              splittedWidgetBidiValues.Add(wtextRange.CharacterFormat.Bidi);
              if (nextValidWidget.Widget.LayoutInfo is TabsLayoutInfo)
                characterRangeTypes.Add(CharacterRangeType.Tab);
              else
                characterRangeTypes.Add(wtextRange.CharacterRange);
            }
            else
            {
              splittedWidgetBidiValues.Add(false);
              characterRangeTypes.Add(CharacterRangeType.LTR);
            }
          }
          else if (widget.Type == CommentMarkType.CommentEnd && index > 0)
          {
            splittedWidgetBidiValues.Add(splittedWidgetBidiValues[splittedWidgetBidiValues.Count - 1]);
            characterRangeTypes.Add(characterRangeTypes[characterRangeTypes.Count - 1]);
          }
          else
          {
            splittedWidgetBidiValues.Add(false);
            characterRangeTypes.Add(CharacterRangeType.LTR);
          }
        }
        else
        {
          splittedWidgetBidiValues.Add(false);
          characterRangeTypes.Add(CharacterRangeType.LTR);
        }
      }
      int rtlStartIndex = -1;
      bool? nullable = new bool?();
      for (int index = 0; index < characterRangeTypes.Count; ++index)
      {
        if (index + 1 < splittedWidgetBidiValues.Count && splittedWidgetBidiValues[index] != splittedWidgetBidiValues[index + 1])
        {
          if (rtlStartIndex != -1)
          {
            this.UpdateCharacterRange(index, rtlStartIndex, splittedWidgetBidiValues, ref characterRangeTypes);
            rtlStartIndex = -1;
          }
          nullable = new bool?();
        }
        else
        {
          if (index > 0 && index != characterRangeTypes.Count - 1 && characterRangeTypes[index] == CharacterRangeType.WordSplit && splittedWidgetBidiValues[index] && characterRangeTypes[index - 1] == CharacterRangeType.Number && splittedWidgetBidiValues[index - 1] && characterRangeTypes[index + 1] == CharacterRangeType.Number && splittedWidgetBidiValues[index + 1] && this.IsNumberNonReversingCharacter(this.m_currChildLW.ChildWidgets[index]))
            characterRangeTypes[index] = CharacterRangeType.Number;
          else if (characterRangeTypes[index] == CharacterRangeType.RTL || characterRangeTypes[index] == CharacterRangeType.LTR || characterRangeTypes[index] == CharacterRangeType.Number && rtlStartIndex != -1 || (!nullable.HasValue || !nullable.Value) && splittedWidgetBidiValues[index])
          {
            if (rtlStartIndex == -1 && characterRangeTypes[index] != CharacterRangeType.LTR)
            {
              rtlStartIndex = index;
            }
            else
            {
              if (rtlStartIndex == -1)
              {
                if (characterRangeTypes[index] == CharacterRangeType.LTR)
                {
                  nullable = new bool?(true);
                  continue;
                }
                if (characterRangeTypes[index] == CharacterRangeType.RTL)
                {
                  nullable = new bool?(false);
                  continue;
                }
                continue;
              }
              if (characterRangeTypes[index] == CharacterRangeType.LTR)
              {
                this.UpdateCharacterRange(index, rtlStartIndex, splittedWidgetBidiValues, ref characterRangeTypes);
                rtlStartIndex = characterRangeTypes[index] == CharacterRangeType.RTL || characterRangeTypes[index] == CharacterRangeType.Number && rtlStartIndex != -1 ? index : -1;
              }
            }
          }
          if (characterRangeTypes[index] == CharacterRangeType.LTR)
            nullable = new bool?(true);
          else if (characterRangeTypes[index] == CharacterRangeType.RTL)
            nullable = new bool?(false);
        }
      }
      if (rtlStartIndex != -1 && rtlStartIndex < characterRangeTypes.Count - 1)
        this.UpdateCharacterRange(characterRangeTypes.Count - 1, rtlStartIndex, splittedWidgetBidiValues, ref characterRangeTypes);
      if (characterRangeTypes.Count != this.m_currChildLW.ChildWidgets.Count)
        throw new Exception("Splitted Widget count mismatch while reordering layouted child widgets of a line");
      LayoutedWidgetList reorderedWidgets = this.ReorderWidgets(characterRangeTypes, splittedWidgetBidiValues, paraBidi);
      splittedWidgetBidiValues.Clear();
      characterRangeTypes.Clear();
      if (this.m_currChildLW.ChildWidgets.Count > 0)
      {
        double trimmedSpaceDiff = 0.0;
        if (subWidthBeforeSpaceTrim != 0.0 && subWidthBeforeSpaceTrim != subWidth && this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1] != reorderedWidgets[reorderedWidgets.Count - 1])
        {
          trimmedSpaceDiff = subWidth - subWidthBeforeSpaceTrim;
          subWidth = subWidthBeforeSpaceTrim;
        }
        this.UpdateBounds(reorderedWidgets, paraBidi, alignment, subWidth, trimmedSpaceDiff, isAutoFrame);
        if (!paraBidi)
        {
          this.m_currChildLW.ChildWidgets = reorderedWidgets;
        }
        else
        {
          reorderedWidgets.Reverse();
          this.m_currChildLW.ChildWidgets = reorderedWidgets;
        }
      }
    }
    return paraBidi;
  }

  private bool ShiftWidgetsForRTLLayouting(LayoutedWidget resultedWidget)
  {
    bool isBidi = false;
    bool isNormalText = false;
    bool flag = false;
    if (this.m_currChildLW.Widget is WParagraph || this.m_currChildLW.Widget is SplitWidgetContainer && (this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
      flag = this.m_currChildLW.Widget is WParagraph ? (this.m_currChildLW.Widget as WParagraph).ParagraphFormat.Bidi : ((this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph).ParagraphFormat.Bidi;
    int rtlTextIndex = -1;
    int lastRtlTextIndex = -1;
    int engTextIndex = -1;
    for (int index = 0; index < this.m_currChildLW.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = this.m_currChildLW.ChildWidgets[index];
      if (childWidget.Widget is WTextRange || childWidget.Widget is SplitStringWidget)
      {
        WTextRange wtextRange = childWidget.Widget is SplitStringWidget ? (childWidget.Widget as SplitStringWidget).RealStringWidget as WTextRange : childWidget.Widget as WTextRange;
        if (this.IsRTLText(wtextRange.Text) || wtextRange.CharacterFormat.Bidi || wtextRange.CharacterFormat.BiDirectionalOverride == BiDirectionalOverride.RTL)
        {
          if (rtlTextIndex == -1)
            rtlTextIndex = index;
          lastRtlTextIndex = index;
          isBidi = true;
        }
        else
        {
          if (engTextIndex == -1)
            engTextIndex = index;
          isNormalText = true;
        }
      }
    }
    if (!flag)
      this.ShiftRTLText(rtlTextIndex, isBidi, isNormalText);
    if (flag)
    {
      if (resultedWidget.ChildWidgets.Count > 0)
      {
        float x = this.m_layoutArea.ClientActiveArea.X;
        float right = this.m_layoutArea.ClientActiveArea.Right;
        for (int index = 0; index < resultedWidget.ChildWidgets.Count; ++index)
        {
          LayoutedWidget childWidget = resultedWidget.ChildWidgets[index];
          if (childWidget.Widget is SplitStringWidget || childWidget.Widget is ParagraphItem && !(childWidget.Widget as ParagraphItem).IsFloatingItem(false))
          {
            float num = childWidget.Bounds.Right - x;
            childWidget.Bounds = new RectangleF(new PointF(right - num, childWidget.Bounds.Y), childWidget.Bounds.Size);
          }
        }
      }
      else if (this.m_currChildLW.ChildWidgets.Count > 1)
      {
        if (isBidi && !isNormalText)
        {
          float right = this.m_currChildLW.Bounds.Right;
          float x1 = this.m_currChildLW.Bounds.X;
          ParagraphLayoutInfo layoutInfo = this.m_currChildLW.Widget.LayoutInfo as ParagraphLayoutInfo;
          float widthToShiftLine = this.GetListWidthToShiftLine();
          float num = right - ((double) widthToShiftLine == 0.0 ? layoutInfo.Margins.Left : widthToShiftLine);
          for (int index = 0; index < this.m_currChildLW.ChildWidgets.Count; ++index)
          {
            LayoutedWidget childWidget = this.m_currChildLW.ChildWidgets[index];
            if (childWidget.Widget is WTextRange || childWidget.Widget is SplitStringWidget || !(childWidget.Widget as ParagraphItem).IsFloatingItem(false))
            {
              float x2 = num - childWidget.Bounds.Right + x1;
              childWidget.Bounds = new RectangleF(new PointF(x2, childWidget.Bounds.Y), childWidget.Bounds.Size);
            }
          }
        }
        else if (!isBidi && isNormalText)
          this.ShiftLineForListWidth();
        else if (isBidi)
          this.ShiftRTLAndNormalText(lastRtlTextIndex, engTextIndex, isNormalText);
      }
      else if (this.m_currChildLW.ChildWidgets.Count == 1 && (isBidi && !isNormalText || isNormalText && !isBidi))
        this.ShiftLineForListWidth();
    }
    return flag;
  }

  private float GetStartPosition(
    bool paraBidi,
    HAlignment alignment,
    double subWidth,
    double trimmedSpaceDiff,
    bool isAutoFrame)
  {
    float x = this.m_layoutArea.ClientActiveArea.X;
    float right = this.m_layoutArea.ClientActiveArea.Right;
    if (isAutoFrame)
    {
      x = this.m_currChildLW.ChildWidgets[0].Bounds.X;
      right = this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].Bounds.Right;
    }
    float num1 = this.GetFirstNonFloatingItemX() - (float) trimmedSpaceDiff;
    float num2 = num1 - x;
    if (alignment == HAlignment.Right)
      num2 -= (float) subWidth;
    if (alignment == HAlignment.Left)
      num2 += (float) subWidth;
    return paraBidi ? right - num2 : num1;
  }

  private void UpdateBounds(
    LayoutedWidgetList reorderedWidgets,
    bool paraBidi,
    HAlignment alignment,
    double subWidth,
    double trimmedSpaceDiff,
    bool isAutoFrame)
  {
    float startPosition = this.GetStartPosition(paraBidi, alignment, subWidth, trimmedSpaceDiff, isAutoFrame);
    if (paraBidi)
    {
      for (int index = reorderedWidgets.Count - 1; index >= 0; --index)
      {
        LayoutedWidget reorderedWidget = reorderedWidgets[index];
        if (!(reorderedWidget.Widget is ParagraphItem) || !(reorderedWidget.Widget as ParagraphItem).IsFloatingItem(false))
        {
          if (reorderedWidget.Widget.LayoutInfo is TabsLayoutInfo)
          {
            TabsLayoutInfo layoutInfo = reorderedWidget.Widget.LayoutInfo as TabsLayoutInfo;
            if ((double) reorderedWidget.Bounds.Width == 0.0 && (double) layoutInfo.TabWidth != (double) reorderedWidget.Bounds.Width)
              startPosition -= layoutInfo.TabWidth;
            else
              startPosition -= reorderedWidget.Bounds.Width;
          }
          else
            startPosition -= reorderedWidget.Bounds.Width;
          this.UpdateBounds(reorderedWidget, startPosition);
        }
      }
    }
    else
    {
      for (int index = 0; index < reorderedWidgets.Count; ++index)
      {
        LayoutedWidget reorderedWidget = reorderedWidgets[index];
        if (!(reorderedWidget.Widget is ParagraphItem) || !(reorderedWidget.Widget as ParagraphItem).IsFloatingItem(false))
        {
          this.UpdateBounds(reorderedWidget, startPosition);
          if (reorderedWidget.Widget.LayoutInfo is TabsLayoutInfo)
          {
            TabsLayoutInfo layoutInfo = reorderedWidget.Widget.LayoutInfo as TabsLayoutInfo;
            if ((double) reorderedWidget.Bounds.Width == 0.0 && (double) layoutInfo.TabWidth != (double) reorderedWidget.Bounds.Width)
              startPosition += layoutInfo.TabWidth;
            else
              startPosition += reorderedWidget.Bounds.Width;
          }
          else
            startPosition += reorderedWidget.Bounds.Width;
        }
      }
    }
  }

  private void UpdateBounds(LayoutedWidget childltWidget, float lineX)
  {
    if (childltWidget.Widget is WMath)
    {
      float xOffset = lineX - childltWidget.Bounds.X;
      childltWidget.ShiftLocation((double) xOffset, 0.0, true, false, true);
    }
    else
      childltWidget.Bounds = new RectangleF(new PointF(lineX, childltWidget.Bounds.Y), childltWidget.Bounds.Size);
  }

  private float GetFirstNonFloatingItemX()
  {
    for (int index = 0; index < this.m_currChildLW.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = this.m_currChildLW.ChildWidgets[index];
      if (!(childWidget.Widget is ParagraphItem) || !(childWidget.Widget as ParagraphItem).IsFloatingItem(false))
        return childWidget.Bounds.X;
    }
    return -1f;
  }

  private LayoutedWidgetList ReorderWidgets(
    List<CharacterRangeType> characterRangeTypes,
    List<bool> splittedWidgetBidiValues,
    bool paraBidi)
  {
    int index1 = 0;
    int num1 = -1;
    int num2 = 0;
    int num3 = 0;
    LayoutedWidgetList layoutedWidgetList = new LayoutedWidgetList();
    CharacterRangeType characterRangeType = CharacterRangeType.LTR;
    bool flag1 = false;
    for (int index2 = 0; index2 < this.m_currChildLW.ChildWidgets.Count; ++index2)
    {
      LayoutedWidget childWidget = this.m_currChildLW.ChildWidgets[index2];
      childWidget.CharacterRange = characterRangeTypes[index2];
      bool flag2 = (childWidget.CharacterRange & CharacterRangeType.RTL) == CharacterRangeType.RTL || childWidget.CharacterRange == CharacterRangeType.Number;
      bool flag3 = splittedWidgetBidiValues[index2];
      if (characterRangeTypes[index2] == CharacterRangeType.Tab)
      {
        if (paraBidi)
        {
          index1 = 0;
          num1 = -1;
          num2 = 0;
          characterRangeType = CharacterRangeType.LTR;
          flag1 = false;
          layoutedWidgetList.Insert(index1, childWidget);
          continue;
        }
        if (flag3)
          flag3 = false;
      }
      if (index2 > 0 && flag1 != flag3)
      {
        if (paraBidi)
        {
          index1 = 0;
          num1 = -1;
          num2 = 0;
        }
        else
          num1 = layoutedWidgetList.Count - 1;
        num3 = 0;
      }
      if (!flag3 && !flag2)
      {
        if (paraBidi)
        {
          if (num2 > 0 && flag1 == flag3)
            index1 += num2;
          layoutedWidgetList.Insert(index1, childWidget);
          ++index1;
        }
        else
        {
          layoutedWidgetList.Add(childWidget);
          index1 = index2 + 1;
        }
        num2 = 0;
        num1 = paraBidi ? index1 - 1 : layoutedWidgetList.Count - 1;
      }
      else if (flag2 || flag3 && childWidget.CharacterRange == CharacterRangeType.WordSplit && (characterRangeType == CharacterRangeType.RTL || this.IsInsertWordSplitToLeft(characterRangeTypes, splittedWidgetBidiValues, index2)))
      {
        ++num2;
        index1 = num1 + 1;
        if (childWidget.CharacterRange == CharacterRangeType.Number)
        {
          if (characterRangeType == CharacterRangeType.Number)
            index1 += num3;
          ++num3;
        }
        layoutedWidgetList.Insert(index1, childWidget);
      }
      else
      {
        layoutedWidgetList.Insert(index1, childWidget);
        ++index1;
        num2 = 0;
      }
      if (childWidget.CharacterRange != CharacterRangeType.Number)
        num3 = 0;
      if (childWidget.CharacterRange != CharacterRangeType.WordSplit)
        characterRangeType = childWidget.CharacterRange;
      flag1 = flag3;
    }
    return layoutedWidgetList;
  }

  private bool IsNumberNonReversingCharacter(LayoutedWidget childltWidget)
  {
    if ((childltWidget.Widget is WTextRange || childltWidget.Widget is SplitStringWidget) && (double) childltWidget.Bounds.Height > 0.0)
    {
      WTextRange wtextRange = childltWidget.Widget is SplitStringWidget ? (childltWidget.Widget as SplitStringWidget).RealStringWidget as WTextRange : childltWidget.Widget as WTextRange;
      if (wtextRange.Text.Length == 1)
      {
        if (!wtextRange.CharacterFormat.HasValueWithParent(75))
          return TextSplitter.IsNumberNonReversingCharacter(wtextRange.Text, wtextRange.CharacterFormat.Bidi);
        char ch = wtextRange.Text[0];
        if (ch == '/' && !this.IsNumberReverseLangForSlash(wtextRange.CharacterFormat.LocaleIdBidi) || (ch == '#' || ch == '$' || ch == '%' || ch == '+' || ch == '-') && !this.IsNumberReverseLangForOthers(wtextRange.CharacterFormat.LocaleIdBidi) || ch == ',' || ch == '.' || ch == ':' || ch == '،')
          return true;
      }
    }
    return false;
  }

  private bool IsNumberReverseLangForSlash(short id)
  {
    LocaleIDs localeIds = (LocaleIDs) id;
    return this.IsNumberReverseLangForOthers(id) || localeIds == LocaleIDs.ar_MA || localeIds == LocaleIDs.prs_AF || localeIds == LocaleIDs.dv_MV || localeIds == LocaleIDs.ks_Arab || localeIds == LocaleIDs.ps_AF || localeIds == LocaleIDs.fa_IR || localeIds == LocaleIDs.sd_Arab_PK || localeIds == LocaleIDs.syr_SY || localeIds == LocaleIDs.tzm_Arab_MA || localeIds == LocaleIDs.ug_CN || localeIds == LocaleIDs.ur_PK;
  }

  private bool IsNumberReverseLangForOthers(short id)
  {
    LocaleIDs localeIds = (LocaleIDs) id;
    switch (localeIds)
    {
      case LocaleIDs.ar_SA:
      case LocaleIDs.ar_IQ:
      case LocaleIDs.ar_EG:
      case LocaleIDs.ar_LY:
      case LocaleIDs.ar_DZ:
      case LocaleIDs.ar_TN:
      case LocaleIDs.ar_OM:
      case LocaleIDs.ar_SY:
      case LocaleIDs.ar_JO:
      case LocaleIDs.ar_LB:
      case LocaleIDs.ar_KW:
      case LocaleIDs.ar_AE:
      case LocaleIDs.ar_BH:
      case LocaleIDs.ar_QA:
        return true;
      default:
        return localeIds == LocaleIDs.ar_YE;
    }
  }

  private bool IsInsertWordSplitToLeft(
    List<CharacterRangeType> characterRangeTypes,
    List<bool> splittedWidgetBidiValues,
    int widgetIndex)
  {
    for (int index = widgetIndex + 1; index < characterRangeTypes.Count && (characterRangeTypes[index] & CharacterRangeType.RTL) != CharacterRangeType.RTL; ++index)
    {
      if (characterRangeTypes[index] == CharacterRangeType.LTR)
        return !splittedWidgetBidiValues[index];
    }
    return true;
  }

  private float GetListWidthToShiftLine()
  {
    float widthToShiftLine = 0.0f;
    if (this.m_currChildLW.Widget is WParagraph || this.m_currChildLW.Widget is SplitWidgetContainer && (this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
    {
      ParagraphLayoutInfo paragraphLayoutInfo = this.m_currChildLW.Widget is WParagraph ? (this.m_currChildLW.Widget as WParagraph).m_layoutInfo as ParagraphLayoutInfo : ((this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph).m_layoutInfo as ParagraphLayoutInfo;
      if (paragraphLayoutInfo.ListValue != string.Empty)
        widthToShiftLine = paragraphLayoutInfo.Margins.Left;
    }
    return widthToShiftLine;
  }

  private void ShiftLineForListWidth()
  {
    ParagraphLayoutInfo layoutInfo = this.m_currChildLW.Widget.LayoutInfo as ParagraphLayoutInfo;
    float widthToShiftLine = this.GetListWidthToShiftLine();
    float num = (double) widthToShiftLine == 0.0 ? layoutInfo.Margins.Left : widthToShiftLine;
    for (int index = 0; index < this.m_currChildLW.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = this.m_currChildLW.ChildWidgets[index];
      if (childWidget.Widget is WTextRange || childWidget.Widget is SplitStringWidget || !(childWidget.Widget as ParagraphItem).IsFloatingItem(false))
      {
        float x = childWidget.Bounds.X - num;
        childWidget.Bounds = new RectangleF(new PointF(x, childWidget.Bounds.Y), childWidget.Bounds.Size);
      }
    }
  }

  private void ShiftRTLAndNormalText(int lastRtlTextIndex, int engTextIndex, bool isNormalText)
  {
    float num1 = this.m_currChildLW.Bounds.Width;
    float x1 = this.m_currChildLW.Bounds.X;
    float widthToShiftLine = this.GetListWidthToShiftLine();
    float num2 = 0.0f;
    if (lastRtlTextIndex == -1)
      lastRtlTextIndex = this.m_currChildLW.ChildWidgets.Count - 1;
    ParagraphLayoutInfo layoutInfo = this.m_currChildLW.Widget.LayoutInfo as ParagraphLayoutInfo;
    for (int index = 0; index <= lastRtlTextIndex; ++index)
    {
      LayoutedWidget childWidget = this.m_currChildLW.ChildWidgets[index];
      if (childWidget.Widget.LayoutInfo is TabsLayoutInfo)
      {
        float x2 = index == 0 ? num1 - childWidget.Bounds.Width : this.m_currChildLW.ChildWidgets[index - 1].Bounds.X - childWidget.Bounds.Width;
        childWidget.Bounds = new RectangleF(new PointF(x2, childWidget.Bounds.Y), childWidget.Bounds.Size);
        num1 = (double) widthToShiftLine == 0.0 ? x2 - x1 + layoutInfo.Margins.Left : x2 - x1 + widthToShiftLine;
      }
      else if (childWidget.Widget is WTextRange || childWidget.Widget is SplitStringWidget || !(childWidget.Widget as ParagraphItem).IsFloatingItem(false))
      {
        float num3 = num1 - childWidget.Bounds.Width;
        float num4 = (double) widthToShiftLine == 0.0 ? layoutInfo.Margins.Left : widthToShiftLine;
        childWidget.Bounds = (double) childWidget.Bounds.Width != 0.0 ? new RectangleF(new PointF(num3 + x1 - num4, childWidget.Bounds.Y), childWidget.Bounds.Size) : new RectangleF(new PointF(num3 + x1, childWidget.Bounds.Y), childWidget.Bounds.Size);
        num1 = num3;
      }
      num2 += childWidget.Bounds.Width;
    }
    float num5 = num2 + ((double) widthToShiftLine == 0.0 ? layoutInfo.Margins.Left : widthToShiftLine);
    for (int index = lastRtlTextIndex + 1; index < this.m_currChildLW.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = this.m_currChildLW.ChildWidgets[index];
      if (childWidget.Widget.LayoutInfo is TabsLayoutInfo)
      {
        float x3 = this.m_currChildLW.ChildWidgets[index - 1].Bounds.X - childWidget.Bounds.Width;
        childWidget.Bounds = new RectangleF(new PointF(x3, childWidget.Bounds.Y), childWidget.Bounds.Size);
        num5 += childWidget.Bounds.Width;
      }
      else if (childWidget.Widget is WTextRange || childWidget.Widget is SplitStringWidget || !(childWidget.Widget as ParagraphItem).IsFloatingItem(false))
        childWidget.Bounds = new RectangleF(new PointF(childWidget.Bounds.X - num5, childWidget.Bounds.Y), childWidget.Bounds.Size);
    }
    if (!isNormalText)
      return;
    this.ShiftNormalText(engTextIndex);
  }

  private void ShiftNormalText(int engTextIndex)
  {
    int index1 = engTextIndex;
    int index2 = index1;
    int num = -1;
    for (; index2 < this.m_currChildLW.ChildWidgets.Count; ++index2)
    {
      LayoutedWidget childWidget = this.m_currChildLW.ChildWidgets[index2];
      if (childWidget.Widget is WTextRange || childWidget.Widget is SplitStringWidget)
      {
        WTextRange wtextRange = childWidget.Widget is SplitStringWidget ? (childWidget.Widget as SplitStringWidget).RealStringWidget as WTextRange : childWidget.Widget as WTextRange;
        if (this.IsRTLText(wtextRange.Text) || wtextRange.CharacterFormat.Bidi || wtextRange.CharacterFormat.BiDirectionalOverride == BiDirectionalOverride.RTL)
        {
          if (index1 != -1 && (num != -1 || !(this.m_currChildLW.ChildWidgets[index1].Widget is WTextRange) && !(this.m_currChildLW.ChildWidgets[index1].Widget is SplitStringWidget) || !((this.m_currChildLW.ChildWidgets[index1].Widget as WTextRange).Text == " ")))
          {
            if (num == -1)
              num = index1;
            float right = this.m_currChildLW.ChildWidgets[index2].Bounds.Right;
            for (int index3 = index1; index3 <= num; ++index3)
            {
              this.m_currChildLW.ChildWidgets[index3].Bounds = new RectangleF(new PointF(right, this.m_currChildLW.ChildWidgets[index3].Bounds.Y), this.m_currChildLW.ChildWidgets[index3].Bounds.Size);
              right = this.m_currChildLW.ChildWidgets[index3].Bounds.Right;
            }
            index2 = num + 1;
            index1 = -1;
          }
        }
        else
        {
          if (index1 == -1)
            index1 = index2;
          num = index2;
        }
      }
    }
  }

  private void ShiftRTLText(int rtlTextIndex, bool isBidi, bool isNormalText)
  {
    int num = rtlTextIndex;
    int index = num;
    int endIndex = -1;
    if (!isBidi)
      return;
    for (; index < this.m_currChildLW.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = this.m_currChildLW.ChildWidgets[index];
      if ((childWidget.Widget is WTextRange || childWidget.Widget is SplitStringWidget) && !(childWidget.Widget.LayoutInfo is TabsLayoutInfo))
      {
        WTextRange wtextRange = childWidget.Widget is SplitStringWidget ? (childWidget.Widget as SplitStringWidget).RealStringWidget as WTextRange : childWidget.Widget as WTextRange;
        if (this.IsRTLText(wtextRange.Text) || wtextRange.CharacterFormat.Bidi || wtextRange.CharacterFormat.BiDirectionalOverride == BiDirectionalOverride.RTL)
        {
          if (num == -1)
            num = index;
          endIndex = index;
          if (index == this.m_currChildLW.ChildWidgets.Count - 1)
          {
            this.ShiftWidgets(num, endIndex);
            num = -1;
            endIndex = -1;
            break;
          }
        }
        else if (num != -1 && (endIndex != -1 || (this.m_currChildLW.ChildWidgets[num].Widget is WTextRange ? ((this.m_currChildLW.ChildWidgets[num].Widget as WTextRange).Text == " " ? 1 : 0) : (this.m_currChildLW.ChildWidgets[num].Widget is SplitStringWidget ? (((this.m_currChildLW.ChildWidgets[num].Widget as SplitStringWidget).RealStringWidget as WTextRange).Text == " " ? 1 : 0) : 0)) == 0))
        {
          if (endIndex == -1)
            endIndex = num;
          this.ShiftWidgets(num, endIndex);
          index = endIndex + 1;
          num = -1;
        }
      }
      else if (childWidget.Widget.LayoutInfo is TabsLayoutInfo && num != -1 && endIndex != -1)
      {
        this.ShiftWidgets(num, endIndex);
        num = index + 1;
      }
    }
    if (num == -1 || endIndex == -1)
      return;
    this.ShiftWidgets(num, endIndex);
  }

  private void ShiftWidgets(int startIndex, int endIndex)
  {
    float right = this.m_currChildLW.ChildWidgets[endIndex].Bounds.Right;
    for (int index = startIndex; index <= endIndex; ++index)
    {
      this.m_currChildLW.ChildWidgets[index].Bounds = new RectangleF(new PointF(right - this.m_currChildLW.ChildWidgets[index].Bounds.Width, this.m_currChildLW.ChildWidgets[index].Bounds.Y), this.m_currChildLW.ChildWidgets[index].Bounds.Size);
      right -= this.m_currChildLW.ChildWidgets[index].Bounds.Width;
    }
  }

  private float GetLastTabWidth(WParagraphFormat paraFormat, int tabsCount)
  {
    float lastTabWidth = 0.0f;
    if (paraFormat.Tabs[tabsCount - 1].Justification != Syncfusion.DocIO.DLS.TabJustification.Left)
    {
      for (int index = this.m_currChildLW.ChildWidgets.Count - 1; index >= 0; --index)
      {
        if (this.m_currChildLW.ChildWidgets[index].Widget.LayoutInfo is TabsLayoutInfo)
        {
          lastTabWidth = this.m_currChildLW.ChildWidgets[index].Bounds.Width;
          break;
        }
      }
    }
    return lastTabWidth;
  }

  private void SplitLineBasedOnInterSectingFlotingEntity(
    LayoutedWidget m_backupWidget,
    ref RectangleF interSectingFloattingItem,
    LayoutedWidget m_resulttedWidgt)
  {
    RectangleF bounds = m_backupWidget.Bounds;
    int firstInlineItemIndex = this.GetFirstInlineItemIndex(m_backupWidget);
    if (firstInlineItemIndex == int.MinValue)
      return;
    bounds.X = m_backupWidget.ChildWidgets[firstInlineItemIndex].Bounds.X;
    bounds.Width = m_backupWidget.ChildWidgets[m_backupWidget.ChildWidgets.Count - 1].Bounds.Right - m_backupWidget.ChildWidgets[firstInlineItemIndex].Bounds.X;
    bounds.Height = m_backupWidget.ChildWidgets[firstInlineItemIndex].Bounds.Height;
    while (++firstInlineItemIndex < m_backupWidget.ChildWidgets.Count && (!(m_backupWidget.ChildWidgets[firstInlineItemIndex].Widget is Entity widget) || !widget.IsFloatingItem(true)))
    {
      if ((double) bounds.Height < (double) m_backupWidget.ChildWidgets[firstInlineItemIndex].Bounds.Height)
        bounds.Height = m_backupWidget.ChildWidgets[firstInlineItemIndex].Bounds.Height;
    }
    if (!(this.m_currChildLW.Widget is Entity entity) && this.m_currChildLW.Widget is SplitWidgetContainer)
      entity = (this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer as Entity;
    interSectingFloattingItem = (double) (this.m_lcOperator as Layouter).MaxRightPositionOfTabStopInterSectingFloattingItems != -3.4028234663852886E+38 || entity is WParagraph && ((entity as WParagraph).IsInCell || (entity as WParagraph).ParagraphFormat.IsFrame) || !(this.GetBaseEntity(entity) is WSection) || m_backupWidget.ChildWidgets.Count <= 1 || (this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && !(entity.Owner is WTableCell) && entity.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 ? new RectangleF(0.0f, 0.0f, 0.0f, 0.0f) : this.InterSectingFloattingItem(bounds);
    if ((double) interSectingFloattingItem.Bottom != 0.0)
    {
      this.m_currChildLW.ChildWidgets.RemoveRange(0, this.m_currChildLW.ChildWidgets.Count);
      for (int index = 0; index < m_backupWidget.ChildWidgets.Count; ++index)
      {
        if ((!(m_backupWidget.ChildWidgets[index].Widget is Entity) || !(m_backupWidget.ChildWidgets[index].Widget as Entity).IsFloatingItem(true)) && (double) m_backupWidget.ChildWidgets[index].Bounds.X > (double) interSectingFloattingItem.X)
        {
          bounds = this.m_currChildLW.Bounds with
          {
            X = m_backupWidget.Bounds.X
          };
          bounds.Width = m_backupWidget.ChildWidgets[index - 1].Bounds.Right - bounds.X;
          this.m_currChildLW.Bounds = bounds;
          m_backupWidget.ChildWidgets.RemoveRange(0, index);
          bounds = this.m_currChildLW.Bounds with
          {
            X = m_backupWidget.ChildWidgets[0].Bounds.X
          };
          bounds.Width = m_backupWidget.Bounds.Right - bounds.X;
          m_backupWidget.Bounds = bounds;
          break;
        }
        this.m_currChildLW.ChildWidgets.Add(m_backupWidget.ChildWidgets[index]);
        if (index == m_backupWidget.ChildWidgets.Count - 1 && m_backupWidget.ChildWidgets.Count > 1)
        {
          bounds = this.m_currChildLW.Bounds with
          {
            X = m_backupWidget.Bounds.X
          };
          bounds.Width = m_backupWidget.ChildWidgets[index - 1].Bounds.Right - bounds.X;
          this.m_currChildLW.Bounds = bounds;
          m_backupWidget.ChildWidgets.RemoveRange(0, m_backupWidget.ChildWidgets.Count);
        }
        else if (index == m_backupWidget.ChildWidgets.Count - 1 && m_backupWidget.ChildWidgets.Count == 1)
          m_backupWidget.ChildWidgets.RemoveRange(0, m_backupWidget.ChildWidgets.Count);
      }
    }
    else
    {
      if (m_resulttedWidgt.ChildWidgets.Count <= 0)
        return;
      this.m_currChildLW.ChildWidgets.RemoveRange(0, this.m_currChildLW.ChildWidgets.Count);
      this.m_currChildLW = m_backupWidget;
    }
  }

  private int GetFirstInlineItemIndex(LayoutedWidget ltWidget)
  {
    for (int index = 0; index < ltWidget.ChildWidgets.Count; ++index)
    {
      if (!(ltWidget.ChildWidgets[index].Widget is Entity) || !(ltWidget.ChildWidgets[index].Widget as Entity).IsFloatingItem(true))
        return index;
    }
    return int.MinValue;
  }

  private RectangleF InterSectingFloattingItem(RectangleF rect)
  {
    if ((double) rect.Width < 0.0)
      return new RectangleF(0.0f, 0.0f, 0.0f, 0.0f);
    for (int index = 0; index < (this.m_lcOperator as Layouter).FloatingItems.Count; ++index)
    {
      RectangleF rect1 = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds;
      TextWrappingStyle textWrappingStyle = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingStyle;
      if ((textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through) && (this.m_lcOperator as Layouter).FloatingItems[index].IsDoesNotDenotesRectangle)
      {
        rect1 = this.AdjustTightAndThroughBounds((this.m_lcOperator as Layouter).FloatingItems[index], rect, rect.Height);
        if (rect1 == (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds)
          continue;
      }
      else if (textWrappingStyle == TextWrappingStyle.Behind || textWrappingStyle == TextWrappingStyle.InFrontOfText || (double) rect1.Right > (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Right)
        continue;
      if ((double) rect.X <= (double) rect1.X && rect.IntersectsWith(rect1))
        return rect1;
    }
    return new RectangleF(0.0f, 0.0f, 0.0f, 0.0f);
  }

  private void RemoveSplitStringWidget(WParagraph paragraph)
  {
    IWidget widget = (IWidget) null;
    for (int index = 0; index < paragraph.ChildEntities.Count; ++index)
    {
      IWidget currentWidget;
      if ((currentWidget = (paragraph.ChildEntities as ParagraphItemCollection).GetCurrentWidget(index)) is SplitStringWidget || index > 0 && widget == currentWidget)
      {
        paragraph.ChildEntities.InnerList.RemoveAt(index);
        paragraph.ChildEntities.UpdateIndexForDuplicateEntity(index, false);
        --index;
      }
      widget = currentWidget;
    }
  }

  private void UpdateSubWidthBasedOnTextWrap(
    WParagraph paragraph,
    ref double subWidth,
    float xPosition,
    float rightMargin)
  {
    if (paragraph != null && paragraph.IsInCell)
    {
      switch (this.GetBaseEntity((Entity) paragraph))
      {
        case WTextBox _:
          return;
        case Shape _:
          return;
        case GroupShape _:
          return;
      }
    }
    if (paragraph == null || (this.m_lcOperator as Layouter).FloatingItems.Count <= 0 || this.m_currChildLW.ChildWidgets.Count <= 0 || (this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && !paragraph.IsInCell && paragraph.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 || this.IsInFootnote(paragraph) || this.IsInTextBox(paragraph) != null)
      return;
    float num1 = 0.0f;
    Entity baseEntity = this.GetBaseEntity((Entity) paragraph);
    for (int index = 0; index < (this.m_lcOperator as Layouter).FloatingItems.Count; ++index)
    {
      RectangleF textWrappingBounds = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds;
      TextWrappingStyle textWrappingStyle = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingStyle;
      if (baseEntity != (this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity && (double) xPosition < (double) textWrappingBounds.X && (double) this.m_layoutArea.ClientActiveArea.Right > (double) textWrappingBounds.X && (double) textWrappingBounds.X > (double) this.m_currChildLW.Bounds.X && (double) this.m_currChildLW.Bounds.Bottom > (double) textWrappingBounds.Y && Math.Round((double) this.m_currChildLW.Bounds.Y, 2) < Math.Round((double) textWrappingBounds.Bottom, 2) && textWrappingStyle != TextWrappingStyle.Inline && textWrappingStyle != TextWrappingStyle.TopAndBottom && textWrappingStyle != TextWrappingStyle.InFrontOfText && textWrappingStyle != TextWrappingStyle.Behind)
      {
        float num2 = this.m_layoutArea.ClientActiveArea.Right - textWrappingBounds.X - rightMargin;
        if (Math.Round((double) num1, 2) < Math.Round((double) num2, 2))
        {
          subWidth -= (double) num2 - (double) num1;
          num1 = num2;
        }
      }
    }
  }

  protected override void UpdateHorizontalAlignment(short xAlignment)
  {
    RectangleF bounds = this.m_currChildLW.Bounds;
    switch (xAlignment)
    {
      case -16:
      case -8:
        this.m_currChildLW.ShiftLocation(-(double) bounds.Width, 0.0, false, false);
        break;
      case -4:
        this.m_currChildLW.ShiftLocation(-(double) bounds.Width / 2.0, 0.0, false, false);
        break;
    }
  }

  internal bool IsTOC(WParagraph para)
  {
    return para != null && para.ChildEntities.FirstItem != null && (para.ChildEntities.FirstItem is TableOfContent || para.ChildEntities.FirstItem is WField && (para.ChildEntities.FirstItem as WField).FieldType == FieldType.FieldHyperlink && new Hyperlink(para.ChildEntities.FirstItem as WField).BookmarkName != null && new Hyperlink(para.ChildEntities.FirstItem as WField).BookmarkName.StartsWithExt("_Toc"));
  }

  protected override void DoLayoutAfter()
  {
    WParagraph wparagraph = (WParagraph) null;
    if (this.m_currChildLW.Widget is WParagraph)
      wparagraph = this.m_currChildLW.Widget as WParagraph;
    else if (this.m_currChildLW.Widget is SplitWidgetContainer)
    {
      SplitWidgetContainer widget = this.m_currChildLW.Widget as SplitWidgetContainer;
      if (widget.RealWidgetContainer is WParagraph)
      {
        wparagraph = widget.RealWidgetContainer as WParagraph;
        if (this.IsTOCNeedNotToBeUpdated(wparagraph, widget))
          return;
      }
    }
    if (DocumentLayouter.IsUpdatingTOC && !(this.m_lcOperator as Layouter).IsLayoutingHeaderRow && (this.m_lcOperator as Layouter).LayoutingTOC == null && !this.SkipUpdatingPageNumber(wparagraph))
      this.UpdateTOCPageNumber(wparagraph);
    if (!DocumentLayouter.IsUpdatingTOC)
      return;
    int num = DocumentLayouter.IsEndUpdateTOC ? 1 : 0;
  }

  private bool SkipUpdatingPageNumber(WParagraph paragraph)
  {
    if (this.m_ltState == LayoutState.NotFitted && this.m_ltWidget.ChildWidgets.Count == 1 && this.IsLineContainOnlyNonRenderableItem(this.m_ltWidget.ChildWidgets[0]))
      return true;
    LayoutedWidget layoutedWidget = this.m_ltWidget;
    while (layoutedWidget.ChildWidgets.Count > 0)
      layoutedWidget = layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1];
    if (layoutedWidget.Widget is Break && (layoutedWidget.Widget as Break).BreakType == BreakType.PageBreak)
    {
      int index = (layoutedWidget.Widget as Break).Index;
      if (this.IsParagraphContainsInvalidItemsOnly(paragraph, index))
        return true;
    }
    return false;
  }

  private bool IsParagraphContainsInvalidItemsOnly(WParagraph para, int pageBreakIndex)
  {
    for (int index = 0; index < pageBreakIndex && index < para.ChildEntities.Count; ++index)
    {
      ParagraphItem childEntity = para.ChildEntities[index] as ParagraphItem;
      switch (childEntity)
      {
        case BookmarkStart _:
        case BookmarkEnd _:
        case WFieldMark _:
          continue;
        case WTextRange _:
          if (childEntity is WField || !this.IsNullOrWhiteSpace((childEntity as WTextRange).Text))
            break;
          continue;
      }
      return false;
    }
    return true;
  }

  private bool IsNullOrWhiteSpace(string text)
  {
    if (string.IsNullOrEmpty(text))
      return true;
    for (int index = 0; index < text.Length; ++index)
    {
      if (!char.IsWhiteSpace(text[index]))
        return false;
    }
    return true;
  }

  private bool IsTOCNeedNotToBeUpdated(WParagraph paragraph, SplitWidgetContainer swc)
  {
    IWidget currentChild = swc.m_currentChild;
    if (currentChild is SplitWidgetContainer)
      currentChild = (currentChild as SplitWidgetContainer).m_currentChild;
    IEntity entity = !(currentChild is SplitStringWidget) ? currentChild as IEntity : (currentChild as SplitStringWidget).RealStringWidget as IEntity;
    int paraItemIndex = entity is Entity ? (entity as Entity).Index : -1;
    if (paraItemIndex == -1)
      return false;
    if (paragraph.HasSDTInlineItem)
      paraItemIndex = ((IWidgetContainer) paragraph).WidgetInnerCollection.IndexOf(entity);
    int validParaItemIndex = this.GetValidParaItemIndex(paragraph, paraItemIndex);
    return (this.m_lcOperator as Layouter).TOCEntryPageNumbers.Count > 0 && (this.m_lcOperator as Layouter).TOCEntryPageNumbers.ContainsKey((Entity) paragraph) && validParaItemIndex != int.MaxValue && validParaItemIndex < paraItemIndex;
  }

  private int GetValidParaItemIndex(WParagraph paragraph, int paraItemIndex)
  {
    for (int index = 0; index < paraItemIndex && index < paragraph.ChildEntities.Count; ++index)
    {
      ParagraphItem paragraphItem = !paragraph.HasSDTInlineItem ? paragraph.ChildEntities[index] as ParagraphItem : ((IWidgetContainer) paragraph).WidgetInnerCollection[index] as ParagraphItem;
      if (paragraphItem != null && (!(paragraphItem is WTextRange) || paragraphItem is WField || !((paragraphItem as WTextRange).Text.Trim() == "")) && !(paragraphItem is BookmarkStart) && !(paragraphItem is BookmarkEnd) && !paragraphItem.IsFloatingItem(false) && !((IWidget) paragraphItem).LayoutInfo.IsSkip && !(paragraphItem is Break))
        return paragraphItem.Index;
    }
    return int.MaxValue;
  }

  private void UpdateTOCPageNumber(WParagraph para)
  {
    if (para.Document.TOC.Values.Count <= 0 || para.IsEmptyParagraph() && para.ListFormat.ListType != ListType.Numbered)
      return;
    string styleName = para.StyleName;
    string str1 = styleName == null ? "normal" : styleName.ToLower().Replace(" ", "");
    foreach (TableOfContent tableOfContent in para.Document.TOC.Values)
    {
      foreach (KeyValuePair<int, List<string>> tocLevel in tableOfContent.TOCLevels)
      {
        foreach (string str2 in tocLevel.Value)
        {
          string str3 = str2.ToLower().Replace(" ", "");
          if (str1.StartsWithExt(str3))
          {
            this.m_lcOperator.SendLeafLayoutAfter(this.m_ltWidget, false);
            if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
              return;
          }
        }
      }
    }
  }
}
