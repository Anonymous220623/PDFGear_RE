// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LCContainer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.Office;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LCContainer(IWidgetContainer widget, ILCOperator lcOperator, bool isForceFitLayout) : 
  LayoutContext((IWidget) widget, lcOperator, isForceFitLayout)
{
  protected int m_curWidgetIndex;
  protected LayoutedWidget m_currChildLW;
  protected bool m_bAtLastOneChildFitted;

  protected IWidgetContainer WidgetContainer => this.m_widget as IWidgetContainer;

  protected IWidget CurrentChildWidget
  {
    get
    {
      return this.m_curWidgetIndex <= -1 || this.m_curWidgetIndex >= this.WidgetContainer.Count ? (IWidget) null : this.WidgetContainer[this.m_curWidgetIndex];
    }
  }

  public override LayoutedWidget Layout(RectangleF rect)
  {
    this.CreateLayoutArea(rect);
    this.CreateLayoutedWidget(rect.Location);
    bool isInnerLayouting = this.IsInnerLayouting;
    LayoutContext childContext;
    do
    {
      childContext = this.CreateNextChildContext();
      if (this.m_currChildLW != null && isInnerLayouting && this.m_currChildLW.TextTag == "Splitted")
        childContext = (LayoutContext) null;
      if (childContext == null)
      {
        if (this.m_bAtLastOneChildFitted)
        {
          this.m_ltState = LayoutState.Fitted;
          this.IsTabStopBeyondRightMarginExists = false;
          break;
        }
        break;
      }
      if (childContext is LCContainer && this.m_ltWidget.Widget is WParagraph && this.m_ltWidget.ChildWidgets.Count > 0 && (this.m_lcOperator as Layouter).UnknownField != null && (this.m_ltWidget.Widget as WParagraph).IsInCell)
      {
        this.m_bAtLastOneChildFitted = true;
        this.m_ltState = LayoutState.Fitted;
        this.IsTabStopBeyondRightMarginExists = false;
        break;
      }
      if (childContext.Widget is WParagraph)
      {
        WParagraph widget = childContext.Widget as WParagraph;
        if (childContext is LCLineContainer && (double) (this.m_lcOperator as Layouter).HiddenLineBottom > 0.0 && !((this.m_lcOperator as Layouter).FieldEntity is WFieldMark))
          (this.m_lcOperator as Layouter).IsFirstItemInLine = false;
        if (widget.ParagraphFormat != null)
          childContext.ClientLayoutAreaRight = this.ClientLayoutAreaRight - widget.ParagraphFormat.RightIndent;
      }
      else
        childContext.ClientLayoutAreaRight = this.ClientLayoutAreaRight;
      childContext.IsTabStopBeyondRightMarginExists = this.IsTabStopBeyondRightMarginExists;
      childContext.IsNeedToWrap = this.IsNeedToWrap;
      childContext.LayoutInfo.TextWrap = this.LayoutInfo.TextWrap;
      this.DoLayoutChild(childContext);
      if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
        return (LayoutedWidget) null;
      this.IsTabStopBeyondRightMarginExists = childContext.IsTabStopBeyondRightMarginExists;
      this.SaveChildContextState(childContext);
      if ((this.m_lcOperator as Layouter).IsNeedToRelayout && this is LCLineContainer)
        this.AddToCollectionAndRelayout(childContext);
      if (childContext is LCContainer && !(childContext is LCLineContainer) && this.m_currChildLW.IsNotFitted)
      {
        WParagraph wparagraph = childContext.Widget is WParagraph ? childContext.Widget as WParagraph : (childContext.Widget is SplitWidgetContainer ? ((childContext.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph ? (childContext.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph : (WParagraph) null) : (WParagraph) null);
        if (wparagraph == null || wparagraph.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 || !DocumentLayouter.IsEndPage || this.m_ltWidget.IsLastItemInPage)
          this.SaveChildContextState(childContext);
      }
    }
    while (((this.State != LayoutState.Unknown || !(childContext is LCTable)) && (this.State != LayoutState.DynamicRelayout || childContext is LCTable) || !(this.m_ltWidget.Widget is WTableCell) || !(this.m_lcOperator as Layouter).IsNeedToRelayoutTable) && (this.State == LayoutState.Unknown || this.State == LayoutState.DynamicRelayout && (this.m_ltWidget.Widget is SplitWidgetContainer ? ((this.m_ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WTableCell ? 1 : 0) : (this.m_ltWidget.Widget is WTableCell ? 1 : 0)) != 0));
    this.DoLayoutAfter();
    return DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC ? (LayoutedWidget) null : this.m_ltWidget;
  }

  private void AddToCollectionAndRelayout(LayoutContext childContext)
  {
    for (int index = 0; index < this.m_currChildLW.ChildWidgets.Count; ++index)
    {
      ParagraphItem widget = this.m_currChildLW.ChildWidgets[index].Widget as ParagraphItem;
      if (this.IsDrawingElement(widget as ILeafWidget) && widget.GetHorizontalOrigin() == HorizontalOrigin.Character && !widget.IsWrappingBoundsAdded())
      {
        WPageSetup pageSetup = (this.m_lcOperator as Layouter).CurrentSection.PageSetup;
        RectangleF bounds = this.m_currChildLW.ChildWidgets[index].Bounds;
        float num = pageSetup.PageSize.Width - pageSetup.Margins.Right;
        if ((double) bounds.Right > (double) num && (widget.GetVerticalOrigin() == VerticalOrigin.Paragraph || widget.GetVerticalOrigin() == VerticalOrigin.Line))
          bounds.X = num - bounds.Width;
        this.m_currChildLW.ChildWidgets[index].Bounds = bounds;
        this.AddToFloatingItems(this.m_currChildLW.ChildWidgets[index], widget as ILeafWidget);
      }
    }
    this.SplitedUpWidget(childContext.SplittedWidget, false);
    this.m_ltState = LayoutState.DynamicRelayout;
    this.m_currChildLW.Owner = this.m_ltWidget;
    (this.m_lcOperator as Layouter).IsNeedToRelayout = false;
  }

  private bool IsFloatingTextBodyItem(IWidget widget)
  {
    return widget is WTable && ((widget as WTable).TableFormat.WrapTextAround || (widget as WTable).IsFrame) || widget is WParagraph && (widget as WParagraph).ParagraphFormat.IsFrame && (widget as WParagraph).ParagraphFormat.WrapFrameAround != FrameWrapMode.None;
  }

  private float GetRightPosition(float rightPosition, ref bool isNeedToUpdateXPosition)
  {
    if ((this.m_lcOperator as Layouter).FloatingItems.Count != 0)
    {
      for (int index1 = this.m_currChildLW.ChildWidgets.Count - 1; index1 >= 0; --index1)
      {
        for (int index2 = this.m_currChildLW.ChildWidgets[index1].ChildWidgets.Count - 1; index2 >= 0; --index2)
        {
          if ((!(this.m_currChildLW.ChildWidgets[index1].ChildWidgets[index2].Widget is ParagraphItem widget) || !widget.IsFloatingItem(false)) && widget != null)
          {
            isNeedToUpdateXPosition = true;
            return this.m_currChildLW.ChildWidgets[index1].ChildWidgets[index2].Bounds.Right;
          }
        }
      }
    }
    else
      isNeedToUpdateXPosition = true;
    return rightPosition;
  }

  private bool IsSkipParaMarkItem(IWidget widget)
  {
    if (widget is WTextRange && (widget as WTextRange).Owner == null && (widget as WTextRange).Text == " " && this.WidgetContainer.WidgetInnerCollection.Count > 1 && this.WidgetContainer.WidgetInnerCollection.Owner is WParagraph)
    {
      WParagraph owner = this.WidgetContainer.WidgetInnerCollection.Owner as WParagraph;
      if (owner.IsContainFloatingItems() && this.IsSizeExceedsClientSize(owner))
      {
        widget.LayoutInfo.IsSkip = true;
        return true;
      }
    }
    return false;
  }

  private bool IsSkipParagraphBreak(IWidget widget)
  {
    bool flag = false;
    if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && this.IsForceFitLayout && DocumentLayouter.PageNumber != 1 && widget is WParagraph)
    {
      WParagraph wparagraph = widget as WParagraph;
      if (wparagraph.ChildEntities.Count == 1 && wparagraph.ChildEntities[0] is Break && (wparagraph.ChildEntities[0] as Break).BreakType == BreakType.PageBreak && (wparagraph.BreakCharacterFormat.Hidden || wparagraph.BreakCharacterFormat.IsDeleteRevision) && wparagraph.NextSibling != null)
      {
        widget.LayoutInfo.IsSkip = true;
        return !flag;
      }
    }
    return flag;
  }

  private bool CheckKeepWithNextForHiddenPara(IEntity widget)
  {
    IEntity nextSibling = (widget as Entity).NextSibling;
    return !(nextSibling is WParagraph) || !(nextSibling as WParagraph).BreakCharacterFormat.Hidden || (nextSibling as WParagraph).m_layoutInfo.IsKeepWithNext;
  }

  private bool IsSizeExceedsClientSize(WParagraph paragraph)
  {
    bool flag = false;
    foreach (ParagraphItem paragraphItem in (Syncfusion.DocIO.DLS.CollectionImpl) paragraph.Items)
    {
      switch (paragraphItem)
      {
        case WFieldMark _:
        case BookmarkStart _:
        case BookmarkEnd _:
          continue;
        default:
          if (paragraphItem.IsFloatingItem(true))
          {
            SizeF sizeF = (paragraphItem as ILeafWidget).Measure(this.DrawingContext);
            if (paragraphItem is WPicture && paragraphItem is WPicture wpicture && (double) wpicture.Rotation != 0.0 && (wpicture.TextWrappingStyle == TextWrappingStyle.Behind || wpicture.TextWrappingStyle == TextWrappingStyle.InFrontOfText || wpicture.TextWrappingStyle == TextWrappingStyle.Square || wpicture.TextWrappingStyle == TextWrappingStyle.TopAndBottom))
              sizeF = this.DrawingContext.GetBoundingBoxCoordinates(new RectangleF(0.0f, 0.0f, sizeF.Width, sizeF.Height), wpicture.Rotation).Size;
            if ((double) sizeF.Height > (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Height)
              flag = this.IsWord2013(paragraph.Document) || (double) sizeF.Width > (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Width;
            if (flag)
              goto label_13;
            continue;
          }
          continue;
      }
    }
label_13:
    return flag;
  }

  private IEntity GetPreviousParagraphDeletedItem(Entity childWidget)
  {
    if (childWidget.Index == 0)
    {
      WParagraph ownerParagraphValue = (childWidget as ParagraphItem).GetOwnerParagraphValue();
      if (ownerParagraphValue == null)
        return (IEntity) null;
      WParagraph previousParagraph = this.GetPreviousParagraph(ownerParagraphValue);
      return previousParagraph != null && previousParagraph.BreakCharacterFormat.IsDeleteRevision && previousParagraph.ChildEntities.Count > 0 ? (IEntity) previousParagraph.LastItem : (IEntity) null;
    }
    IEntity paragraphDeletedItem;
    switch (childWidget)
    {
      case ParagraphItem _:
        paragraphDeletedItem = (childWidget as ParagraphItem).PreviousSibling;
        break;
      case Break _:
        paragraphDeletedItem = (childWidget as Break).PreviousSibling;
        break;
      default:
        paragraphDeletedItem = (IEntity) null;
        break;
    }
    return paragraphDeletedItem;
  }

  protected virtual LayoutContext CreateNextChildContext()
  {
    IWidget currentChildWidget;
    while (true)
    {
      int index1;
      do
      {
        do
        {
          LayoutedWidget interSectWidget1 = this.CheckNullConditionAndReturnltwidget();
          int[] interSectingPoint1 = (this.m_lcOperator as Layouter).m_interSectingPoint;
          if (interSectingPoint1[2] != int.MinValue && interSectWidget1 != null)
          {
            IWidget widget = this.CurrentChildWidget is SplitWidgetContainer ? (IWidget) (this.CurrentChildWidget as SplitWidgetContainer).RealWidgetContainer : (this.CurrentChildWidget is SplitTableWidget ? (IWidget) (this.CurrentChildWidget as SplitTableWidget).TableWidget : this.CurrentChildWidget);
            if (widget != null && widget is WSection)
            {
              this.AddLayoutWidgetInBeforeInsectingPoint(interSectWidget1, interSectingPoint1[1]);
              this.m_curWidgetIndex = interSectingPoint1[1];
            }
            else if (interSectWidget1.ChildWidgets.Count > interSectingPoint1[2] && (widget is WTable || widget is WParagraph))
            {
              if (this.IsFloatingTextBodyItem(interSectWidget1.ChildWidgets[interSectingPoint1[2]].Widget))
              {
                (this.m_lcOperator as Layouter).m_interSectingPoint[0] = int.MinValue;
              }
              else
              {
                this.AddLayoutWidgetInBeforeInsectingPoint(interSectWidget1.ChildWidgets[interSectingPoint1[2]], interSectingPoint1[2]);
                this.m_curWidgetIndex = interSectingPoint1[2];
              }
              if (this.CurrentChildWidget == null || this.CurrentChildWidget is WTable || this.CurrentChildWidget is SplitTableWidget || this.CurrentChildWidget.LayoutInfo.IsSkip)
              {
                (this.m_lcOperator as Layouter).m_interSectingPoint[0] = int.MinValue;
                (this.m_lcOperator as Layouter).m_interSectingPoint[1] = int.MinValue;
                (this.m_lcOperator as Layouter).m_interSectingPoint[2] = int.MinValue;
                (this.m_lcOperator as Layouter).m_interSectingPoint[3] = int.MinValue;
              }
            }
          }
          if (this.State == LayoutState.DynamicRelayout)
          {
            this.m_ltState = LayoutState.Unknown;
            (this.m_lcOperator as Layouter).MaintainltWidget = new LayoutedWidget(this.m_ltWidget);
            this.FindIntersectPointAndRemovltWidgetForTable();
            LayoutedWidget interSectWidget2 = this.CheckNullConditionAndReturnltwidget();
            int[] interSectingPoint2 = (this.m_lcOperator as Layouter).m_interSectingPoint;
            if (interSectingPoint2[0] != int.MinValue && interSectWidget2 != null)
            {
              this.AddLayoutWidgetInBeforeInsectingPoint(interSectWidget2, interSectingPoint2[1]);
              this.RemoveLineLayoutedWidgetFromTable();
              this.m_curWidgetIndex = interSectingPoint2[0];
              if ((this.m_lcOperator as Layouter).DynamicTable != null)
                (this.m_lcOperator as Layouter).m_interSectingPoint[0] = int.MinValue;
            }
          }
          currentChildWidget = this.CurrentChildWidget;
          if (currentChildWidget is WField && (currentChildWidget as WField).FieldSeparator != null && (currentChildWidget as WField).FieldEnd != null && (currentChildWidget as WField).FieldSeparator.OwnerParagraph != (currentChildWidget as WField).OwnerParagraph && this.IsInSameTextBody(currentChildWidget as WField))
            (this.m_lcOperator as Layouter).FieldEntity = (IEntity) (currentChildWidget as WField);
          if (currentChildWidget is WAbsoluteTab && DocumentLayouter.IsLayoutingHeaderFooter && (currentChildWidget as WAbsoluteTab).m_layoutInfo is TabsLayoutInfo)
            this.UpdateAboluteTabPosition(currentChildWidget as WAbsoluteTab);
          if (!(this.m_lcOperator as Layouter).IsLayoutingTrackChanges)
          {
            if (currentChildWidget is WCommentMark && this.IsNeedToShowComments((TextBodyItem) (currentChildWidget as ParagraphItem).OwnerParagraph))
              this.CreateBalloonForComments(currentChildWidget);
            if (currentChildWidget is ParagraphItem && this.IsNeedToShowDeletedMarkUp((TextBodyItem) (currentChildWidget as ParagraphItem).OwnerParagraph) && ((currentChildWidget as ParagraphItem).IsDeleteRevision || currentChildWidget is Break && (currentChildWidget as Break).CharacterFormat.IsDeleteRevision))
              this.CreateBalloonForDeletedParagraphItem(currentChildWidget);
            else if (currentChildWidget is WParagraph && this.IsNeedToShowDeletedMarkUp((TextBodyItem) (currentChildWidget as WParagraph)) && (currentChildWidget as WParagraph).BreakCharacterFormat.IsDeleteRevision && (currentChildWidget as WParagraph).IsDeletionParagraph())
              this.CreateBalloonForDeletedParagraphText(currentChildWidget);
            if (currentChildWidget is WTextRange && this.IsNeedToShowFormattingMarkUp((TextBodyItem) (currentChildWidget as ParagraphItem).OwnerParagraph) && ((currentChildWidget as ParagraphItem).IsChangedCFormat || currentChildWidget is Break && (currentChildWidget as Break).CharacterFormat.IsChangedFormat))
              this.CreateBalloonValueForCFormat(currentChildWidget);
            else if (currentChildWidget is WParagraph && this.IsNeedToShowFormattingMarkUp((TextBodyItem) (currentChildWidget as WParagraph)) && (currentChildWidget as WParagraph).IsChangedPFormat)
            {
              this.CreateBalloonValueForPFormat(currentChildWidget);
              if (!(currentChildWidget as WParagraph).ListFormat.IsEmptyList)
                this.CreateBalloonValueForListFormat(currentChildWidget);
            }
            else if (currentChildWidget is WTable && this.IsNeedToShowFormattingMarkUp((TextBodyItem) (currentChildWidget as WTable)) && (currentChildWidget as WTable).TableFormat.IsFormattingChange)
              this.CreateBalloonValueForTableFormat(currentChildWidget);
          }
          if (DocumentLayouter.IsLayoutingHeaderFooter)
            this.UpdateExpressionField(currentChildWidget);
          if (currentChildWidget is WField)
          {
            WField wfield = currentChildWidget as WField;
            if (wfield.FieldType == FieldType.FieldUnknown && wfield.FieldEnd != null && wfield.FieldEnd.OwnerParagraph.Owner is WTableCell && (!(wfield.OwnerParagraph.Owner is WTableCell) || wfield.FieldEnd.OwnerParagraph.Owner != wfield.OwnerParagraph.Owner))
              (this.m_lcOperator as Layouter).UnknownField = wfield;
          }
          this.LayoutFootnoteSplittedWidgets(currentChildWidget);
          this.LayoutEndnoteSplittedWidgets(currentChildWidget);
          if (currentChildWidget != null)
          {
            if (currentChildWidget.LayoutInfo != null && currentChildWidget.LayoutInfo.IsSkip || this.IsSkipParaMarkItem(currentChildWidget) || this.IsSkipParagraphBreak(currentChildWidget))
            {
              if (currentChildWidget is TableOfContent)
                (this.m_lcOperator as Layouter).LayoutingTOC = currentChildWidget as TableOfContent;
              if (currentChildWidget is WFieldMark && (this.m_lcOperator as Layouter).LayoutingTOC != null && (this.m_lcOperator as Layouter).LayoutingTOC.TOCField != null && currentChildWidget == (this.m_lcOperator as Layouter).LayoutingTOC.TOCField.FieldEnd)
                (this.m_lcOperator as Layouter).LayoutingTOC = (TableOfContent) null;
              if (currentChildWidget is XmlParagraphItem xmlParagraphItem && !(xmlParagraphItem.Owner is InlineContentControl) && xmlParagraphItem.MathParaItemsCollection != null && xmlParagraphItem.MathParaItemsCollection.Count > 0)
              {
                int index2 = this.m_curWidgetIndex + 1;
                if (this.WidgetContainer is SplitWidgetContainer)
                  index2 = this.WidgetContainer.WidgetInnerCollection.InnerList.IndexOf((object) currentChildWidget) + 1;
                foreach (ParagraphItem mathParaItems in (Syncfusion.DocIO.DLS.CollectionImpl) xmlParagraphItem.MathParaItemsCollection)
                {
                  if (this.WidgetContainer.WidgetInnerCollection.InnerList.IndexOf((object) mathParaItems) == -1)
                  {
                    if (xmlParagraphItem.Owner is WParagraph && xmlParagraphItem.OwnerParagraph.HasSDTInlineItem)
                      this.WidgetContainer.WidgetInnerCollection.InnerList.Insert(index2, (object) mathParaItems);
                    else
                      this.WidgetContainer.WidgetInnerCollection.Insert(index2, (IEntity) mathParaItems);
                    ++index2;
                  }
                }
              }
            }
            else
              goto label_58;
          }
          else
            goto label_88;
        }
        while (this.NextChildWidget());
        this.m_bAtLastOneChildFitted = true;
        return (LayoutContext) null;
label_58:
        if (currentChildWidget is Break && (currentChildWidget as Break).BreakType == BreakType.PageBreak)
        {
          index1 = (currentChildWidget as Break).Index;
          WParagraph ownerParagraph = (currentChildWidget as Break).OwnerParagraph;
          bool flag = false;
          if (this.WidgetContainer.WidgetInnerCollection.Count - 1 != index1 && !this.IsNeedToSkipMovingBreakItem(ownerParagraph))
          {
            for (int index3 = index1 + 1; index3 < this.WidgetContainer.WidgetInnerCollection.Count; ++index3)
            {
              Entity widgetInner = this.WidgetContainer.WidgetInnerCollection[index3];
              switch (widgetInner)
              {
                case BookmarkStart _:
                case BookmarkEnd _:
                case WFieldMark _:
label_63:
                  flag = true;
                  continue;
                default:
                  if (!widgetInner.IsFloatingItem(false))
                  {
                    flag = false;
                    goto label_66;
                  }
                  goto label_63;
              }
            }
          }
label_66:
          if (flag)
          {
            this.WidgetContainer.WidgetInnerCollection.AddToInnerList((Entity) (currentChildWidget as Break));
            this.WidgetContainer.WidgetInnerCollection.RemoveFromInnerList(index1);
          }
          else
            goto label_69;
        }
        else
          goto label_69;
      }
      while (!(this.WidgetContainer is SplitWidgetContainer) || (this.WidgetContainer as SplitWidgetContainer).m_currentChild != currentChildWidget);
      (this.WidgetContainer as SplitWidgetContainer).m_currentChild = this.WidgetContainer.WidgetInnerCollection[index1] as IWidget;
    }
label_69:
    if (currentChildWidget is WTable table && !table.TableFormat.WrapTextAround && this.IsInFrame(table))
    {
      if (this.IsForceFitLayout)
        currentChildWidget.LayoutInfo.IsFirstItemInPage = true;
      this.GetFrameBounds(table.Rows[0].Cells[0].Paragraphs[0], this.m_layoutArea.ClientActiveArea);
    }
    if ((this.m_lcOperator as Layouter).DynamicParagraph != null && currentChildWidget is TextBodyItem)
    {
      IList innerList = (this.WidgetContainer.WidgetInnerCollection as BodyItemCollection).InnerList;
      int num = innerList.IndexOf((object) currentChildWidget);
      if (num > 0)
      {
        TextBodyItem textBodyItem = innerList[num - 1] as TextBodyItem;
        if (textBodyItem is WParagraph && (this.m_lcOperator as Layouter).DynamicParagraph == textBodyItem as WParagraph || textBodyItem is WTable && this.LastRowHaveDynamicPara((textBodyItem as WTable).LastRow))
          (this.m_lcOperator as Layouter).DynamicParagraph = (WParagraph) null;
      }
      else if (num == 0 && currentChildWidget is WParagraph && (currentChildWidget as WParagraph).Owner is WTableCell)
      {
        WTableCell owner = (currentChildWidget as WParagraph).Owner as WTableCell;
        if (owner.PreviousSibling is WTableCell)
        {
          if ((owner.PreviousSibling as WTableCell).ChildEntities.Contains((IEntity) (this.m_lcOperator as Layouter).DynamicParagraph))
            (this.m_lcOperator as Layouter).DynamicParagraph = (WParagraph) null;
        }
        else if (owner.OwnerRow.Index > 0 && this.LastRowHaveDynamicPara(owner.OwnerRow.PreviousSibling as WTableRow))
          (this.m_lcOperator as Layouter).DynamicParagraph = (WParagraph) null;
      }
    }
    this.UpdateTextBodyItemPosition(currentChildWidget);
    if (currentChildWidget is WParagraph || currentChildWidget is SplitWidgetContainer && (currentChildWidget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
    {
      (this.m_lcOperator as Layouter).IsTwoLinesLayouted = false;
      (this.m_lcOperator as Layouter).IsFootnoteHeightAdjusted = false;
    }
    if (currentChildWidget is WTextRange || currentChildWidget is WFootnote)
    {
      WParagraph wparagraph = currentChildWidget is WFootnote ? (currentChildWidget as WFootnote).OwnerParagraph : (currentChildWidget as WTextRange).OwnerParagraph;
      currentChildWidget.LayoutInfo.IsClipped = wparagraph == null ? this.LayoutInfo.IsClipped : ((IWidget) wparagraph).LayoutInfo.IsClipped;
    }
    return LayoutContext.Create(currentChildWidget, this.m_lcOperator, this.IsForceFitLayout);
label_88:
    this.m_bAtLastOneChildFitted = true;
    return (LayoutContext) null;
  }

  private void RemoveLineLayoutedWidgetFromTable()
  {
    int index = 0;
    while (index < this.m_ltWidget.ChildWidgets.Count)
    {
      LayoutedWidget childWidget = this.m_ltWidget.ChildWidgets[index];
      if (childWidget.ChildWidgets.Count >= 1 && (childWidget.ChildWidgets[0].Widget is SplitWidgetContainer ? ((childWidget.ChildWidgets[0].Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph ? 1 : 0) : (childWidget.ChildWidgets[0].Widget is WParagraph ? 1 : 0)) == 0)
        this.m_ltWidget.ChildWidgets.Remove(childWidget);
      else
        ++index;
    }
  }

  private bool IsInSameTextBody(WField field)
  {
    return field.OwnerParagraph.OwnerTextBody == field.FieldSeparator.OwnerParagraph.OwnerTextBody;
  }

  private void UpdateAboluteTabPosition(WAbsoluteTab absoluteTab)
  {
    if (absoluteTab.OwnerParagraph == null)
      return;
    (absoluteTab.m_layoutInfo as TabsLayoutInfo).m_list[0].Position = absoluteTab.GetAbsolutePosition((IEntity) (this.m_lcOperator as Layouter).CurrentSection, 0.0f);
  }

  private bool IsNeedToShowComments(TextBodyItem bodyItem)
  {
    return bodyItem != null && bodyItem.Document != null && bodyItem.Document.RevisionOptions.CommentDisplayMode == CommentDisplayMode.ShowInBalloons;
  }

  private void CreateBalloonForComments(IWidget childWidget)
  {
    CommentMarkType type = (childWidget as WCommentMark).Type;
    WComment comment = (childWidget as WCommentMark).Comment;
    if (comment == null || type != CommentMarkType.CommentEnd && comment.CommentRangeEnd != null)
      return;
    CommentsMarkups commentsMarkups = new CommentsMarkups((childWidget as ParagraphItem).Document, comment);
    commentsMarkups.AppendInCommentsBalloon();
    commentsMarkups.Position = this.LayoutArea.ClientActiveArea.Location;
    commentsMarkups.BallonYPosition = this.LayoutArea.ClientActiveArea.Location.Y;
    (this.m_lcOperator as Layouter).TrackChangesMarkups.Add((TrackChangesMarkups) commentsMarkups);
  }

  private bool IsNeedToShowDeletedMarkUp(TextBodyItem bodyItem)
  {
    if (bodyItem == null || bodyItem.Document == null)
      return false;
    RevisionOptions revisionOptions = bodyItem.Document.RevisionOptions;
    return (revisionOptions.ShowMarkup & RevisionType.Deletions) == RevisionType.Deletions && (revisionOptions.BalloonOptions & RevisionBalloonsOptions.Deletions) == RevisionBalloonsOptions.Deletions && (revisionOptions.ShowInBalloons & RevisionType.Deletions) == RevisionType.Deletions;
  }

  private bool IsNeedToShowFormattingMarkUp(TextBodyItem bodyItem)
  {
    if (bodyItem == null || bodyItem.Document == null)
      return false;
    RevisionOptions revisionOptions = bodyItem.Document.RevisionOptions;
    return (revisionOptions.ShowMarkup & RevisionType.Formatting) == RevisionType.Formatting && (revisionOptions.BalloonOptions & RevisionBalloonsOptions.Formatting) == RevisionBalloonsOptions.Formatting && (revisionOptions.ShowInBalloons & RevisionType.Formatting) == RevisionType.Formatting;
  }

  private bool IsNeedToCreateNewBalloonForCFormat(
    ParagraphItem paragraphItem,
    string newBalloonValue)
  {
    if (paragraphItem.Owner != null && paragraphItem.Owner is InlineContentControl)
      return true;
    IEntity previousSibling = paragraphItem.PreviousSibling;
    if (previousSibling == null)
    {
      WParagraph ownerParagraphValue = paragraphItem.GetOwnerParagraphValue();
      if (ownerParagraphValue == null)
        return true;
      WParagraph previousParagraph = this.GetPreviousParagraph(ownerParagraphValue);
      return previousParagraph == null || !previousParagraph.BreakCharacterFormat.IsChangedFormat || !this.GetPreviousFormattedBalloonValue().Equals(newBalloonValue, StringComparison.OrdinalIgnoreCase);
    }
    return previousSibling == null || (!(previousSibling is WTextRange) || !(previousSibling as ParagraphItem).IsChangedCFormat) && (!(previousSibling is Break) || !(previousSibling as Break).CharacterFormat.IsChangedFormat) || !this.GetPreviousFormattedBalloonValue().Equals(newBalloonValue, StringComparison.OrdinalIgnoreCase);
  }

  private void CreateBalloonForDeletedParagraphItem(IWidget childWidget)
  {
    if (!(childWidget as ParagraphItem).Document.RevisionOptions.ShowDeletedText)
      childWidget.LayoutInfo.IsSkip = true;
    TrackChangesMarkups trackChangesMarkups = new TrackChangesMarkups((childWidget as ParagraphItem).Document);
    IEntity paragraphDeletedItem = this.GetPreviousParagraphDeletedItem(childWidget as Entity);
    bool flag = false;
    if (paragraphDeletedItem is ParagraphItem && ((paragraphDeletedItem as ParagraphItem).IsDeleteRevision || paragraphDeletedItem is Break && (paragraphDeletedItem as Break).CharacterFormat.IsDeleteRevision) && (this.m_lcOperator as Layouter).TrackChangesMarkups.Count > 0 && !((this.m_lcOperator as Layouter).TrackChangesMarkups[(this.m_lcOperator as Layouter).TrackChangesMarkups.Count - 1] is CommentsMarkups))
    {
      flag = true;
      if (childWidget is Break)
        trackChangesMarkups.ChangedValue.AddParagraph().AppendBreak(BreakType.LineBreak);
      else if (childWidget is WTextRange)
      {
        trackChangesMarkups = (this.m_lcOperator as Layouter).TrackChangesMarkups[(this.m_lcOperator as Layouter).TrackChangesMarkups.Count - 1];
        if (trackChangesMarkups.ChangedValue.ChildEntities.Count == 0)
          trackChangesMarkups.ChangedValue.AddParagraph();
        trackChangesMarkups.AppendInDeletionBalloon(childWidget as WTextRange);
      }
    }
    else
    {
      trackChangesMarkups.TypeOfMarkup = RevisionType.Deletions;
      if (childWidget is WTextRange)
      {
        trackChangesMarkups.ChangedValue.AddParagraph();
        trackChangesMarkups.ChangedValue.LastParagraph.AppendText(trackChangesMarkups.GetBalloonValueForMarkupType() + ": ").CharacterFormat.Bold = true;
        trackChangesMarkups.AppendInDeletionBalloon(childWidget as WTextRange);
        trackChangesMarkups.Position = this.LayoutArea.ClientActiveArea.Location;
        trackChangesMarkups.BallonYPosition = this.LayoutArea.ClientActiveArea.Location.Y;
      }
      else if (childWidget is WPicture)
      {
        trackChangesMarkups.ChangedValue.AddParagraph();
        trackChangesMarkups.ChangedValue.LastParagraph.AppendText(trackChangesMarkups.GetBalloonValueForMarkupType() + ": ").CharacterFormat.Bold = true;
        trackChangesMarkups.ChangedValue.LastParagraph.AppendPicture((childWidget as WPicture).ImageBytes);
        trackChangesMarkups.Position = this.LayoutArea.ClientActiveArea.Location;
        trackChangesMarkups.BallonYPosition = this.LayoutArea.ClientActiveArea.Location.Y;
      }
    }
    if (!flag)
      (this.m_lcOperator as Layouter).TrackChangesMarkups.Add(trackChangesMarkups);
    if (this.m_ltWidget == null)
      return;
    this.m_ltWidget.IsTrackChanges = true;
  }

  private void CreateBalloonForDeletedParagraphText(IWidget childWidget)
  {
    if (!(childWidget as WParagraph).Document.RevisionOptions.ShowDeletedText)
      childWidget.LayoutInfo.IsSkip = true;
    TrackChangesMarkups trackChangesMarkups = new TrackChangesMarkups((childWidget as WParagraph).Document);
    WParagraph previousParagraph = this.GetPreviousParagraph(childWidget as WParagraph);
    bool flag = false;
    if (previousParagraph != null && previousParagraph.BreakCharacterFormat.IsDeleteRevision && (this.m_lcOperator as Layouter).TrackChangesMarkups.Count > 0 && !((this.m_lcOperator as Layouter).TrackChangesMarkups[(this.m_lcOperator as Layouter).TrackChangesMarkups.Count - 1] is CommentsMarkups))
    {
      flag = true;
      trackChangesMarkups = (this.m_lcOperator as Layouter).TrackChangesMarkups[(this.m_lcOperator as Layouter).TrackChangesMarkups.Count - 1];
      if (trackChangesMarkups.ChangedValue.ChildEntities.Count == 0)
        trackChangesMarkups.ChangedValue.AddParagraph();
      trackChangesMarkups.ChangedValue.LastParagraph.AppendBreak(BreakType.LineBreak);
      foreach (ParagraphItem textRange in (Syncfusion.DocIO.DLS.CollectionImpl) (childWidget as WParagraph).Items)
      {
        if (textRange is WTextRange)
          trackChangesMarkups.AppendInDeletionBalloon(textRange as WTextRange);
      }
    }
    else
    {
      trackChangesMarkups.TypeOfMarkup = RevisionType.Deletions;
      trackChangesMarkups.ChangedValue.AddParagraph();
      trackChangesMarkups.ChangedValue.LastParagraph.AppendText(trackChangesMarkups.GetBalloonValueForMarkupType() + ": ").CharacterFormat.Bold = true;
      foreach (ParagraphItem textRange in (Syncfusion.DocIO.DLS.CollectionImpl) (childWidget as WParagraph).Items)
      {
        if (textRange is WTextRange)
          trackChangesMarkups.AppendInDeletionBalloon(textRange as WTextRange);
      }
      trackChangesMarkups.Position = this.LayoutArea.ClientActiveArea.Location;
      trackChangesMarkups.BallonYPosition = this.LayoutArea.ClientActiveArea.Location.Y;
    }
    if (!flag)
      (this.m_lcOperator as Layouter).TrackChangesMarkups.Add(trackChangesMarkups);
    if (this.m_ltWidget == null)
      return;
    this.m_ltWidget.IsTrackChanges = true;
  }

  private void CreateBalloonValueForCFormat(IWidget childWidget)
  {
    TrackChangesMarkups trackChangesMarkups = new TrackChangesMarkups((childWidget as ParagraphItem).Document);
    Dictionary<int, object> dictionary1 = childWidget is WTextRange ? new Dictionary<int, object>((IDictionary<int, object>) (childWidget as WTextRange).CharacterFormat.PropertiesHash) : new Dictionary<int, object>((IDictionary<int, object>) (childWidget as Break).CharacterFormat.PropertiesHash);
    Dictionary<int, object> standardDic1 = childWidget is WTextRange ? new Dictionary<int, object>((IDictionary<int, object>) (childWidget as WTextRange).CharacterFormat.OldPropertiesHash) : new Dictionary<int, object>((IDictionary<int, object>) (childWidget as Break).CharacterFormat.OldPropertiesHash);
    this.RemoveSameValues(dictionary1, standardDic1);
    if (dictionary1.Count <= 0)
      return;
    FontScriptType scriptType = FontScriptType.English;
    if (childWidget is WTextRange)
      scriptType = (childWidget as WTextRange).ScriptType;
    Dictionary<int, string> hierarchyOrder = new Dictionary<int, string>();
    WCharacterFormat characterformat = childWidget is WTextRange ? (childWidget as WTextRange).CharacterFormat : (childWidget as Break).CharacterFormat;
    trackChangesMarkups.DisplayBalloonValueCFormat(scriptType, dictionary1, characterformat, ref hierarchyOrder);
    Dictionary<int, object> standardDic2 = childWidget is WTextRange ? new Dictionary<int, object>((IDictionary<int, object>) (childWidget as WTextRange).CharacterFormat.PropertiesHash) : new Dictionary<int, object>((IDictionary<int, object>) (childWidget as Break).CharacterFormat.PropertiesHash);
    Dictionary<int, object> dictionary2 = childWidget is WTextRange ? new Dictionary<int, object>((IDictionary<int, object>) (childWidget as WTextRange).CharacterFormat.OldPropertiesHash) : new Dictionary<int, object>((IDictionary<int, object>) (childWidget as Break).CharacterFormat.OldPropertiesHash);
    this.RemoveSameValues(dictionary2, standardDic2);
    if (dictionary2.Count > 0)
      trackChangesMarkups.DisplayBalloonValueforRemovedCFormat(dictionary2, characterformat, ref hierarchyOrder);
    trackChangesMarkups.ChangedValue.AddParagraph();
    trackChangesMarkups.TypeOfMarkup = RevisionType.Formatting;
    trackChangesMarkups.ChangedValue.LastParagraph.AppendText(trackChangesMarkups.GetBalloonValueForMarkupType() + ": ").CharacterFormat.Bold = true;
    trackChangesMarkups.ChangedValue.LastParagraph.AppendText(trackChangesMarkups.ConvertDictionaryValuesToString(hierarchyOrder));
    trackChangesMarkups.Position = this.LayoutArea.ClientActiveArea.Location;
    trackChangesMarkups.BallonYPosition = this.LayoutArea.ClientActiveArea.Location.Y;
    if (!this.IsNeedToCreateNewBalloonForCFormat(childWidget as ParagraphItem, trackChangesMarkups.ChangedValue.LastParagraph.Text) || string.IsNullOrEmpty(trackChangesMarkups.ChangedValue.LastParagraph.Text))
      return;
    (this.m_lcOperator as Layouter).TrackChangesMarkups.Add(trackChangesMarkups);
    if (this.m_ltWidget == null)
      return;
    this.m_ltWidget.IsTrackChanges = true;
  }

  private void RemoveSameValues(
    Dictionary<int, object> dicToRemove,
    Dictionary<int, object> standardDic)
  {
    foreach (KeyValuePair<int, object> keyValuePair in standardDic)
    {
      if (dicToRemove.ContainsKey(keyValuePair.Key) && dicToRemove[keyValuePair.Key].Equals(keyValuePair.Value))
        dicToRemove.Remove(keyValuePair.Key);
    }
  }

  private void CreateBalloonValueForPFormat(IWidget childWidget)
  {
    TrackChangesMarkups trackChangesMarkups = new TrackChangesMarkups((childWidget as WParagraph).Document);
    Dictionary<int, object> newpropertyhash1 = new Dictionary<int, object>((IDictionary<int, object>) (childWidget as WParagraph).ParagraphFormat.PropertiesHash);
    foreach (KeyValuePair<int, object> keyValuePair in new Dictionary<int, object>((IDictionary<int, object>) (childWidget as WParagraph).ParagraphFormat.OldPropertiesHash))
    {
      if (newpropertyhash1.ContainsKey(keyValuePair.Key) && newpropertyhash1[keyValuePair.Key].Equals(keyValuePair.Value))
        newpropertyhash1.Remove(keyValuePair.Key);
    }
    if (newpropertyhash1.Count <= 0)
      return;
    Dictionary<int, string> hierarchyOrder = new Dictionary<int, string>();
    trackChangesMarkups.DisplayBalloonValueForPFormat(newpropertyhash1, (childWidget as WParagraph).ParagraphFormat, ref hierarchyOrder);
    Dictionary<int, object> dictionary = childWidget is WParagraph ? new Dictionary<int, object>((IDictionary<int, object>) (childWidget as WParagraph).ParagraphFormat.PropertiesHash) : new Dictionary<int, object>((IDictionary<int, object>) (childWidget as Break).ParaItemCharFormat.PropertiesHash);
    Dictionary<int, object> newpropertyhash2 = childWidget is WParagraph ? new Dictionary<int, object>((IDictionary<int, object>) (childWidget as WParagraph).ParagraphFormat.OldPropertiesHash) : new Dictionary<int, object>((IDictionary<int, object>) (childWidget as Break).ParaItemCharFormat.OldPropertiesHash);
    foreach (KeyValuePair<int, object> keyValuePair in dictionary)
    {
      if (newpropertyhash2.ContainsKey(keyValuePair.Key) && newpropertyhash2[keyValuePair.Key].Equals(keyValuePair.Value))
        newpropertyhash2.Remove(keyValuePair.Key);
    }
    if (newpropertyhash2.Count > 0)
      trackChangesMarkups.DisplayBalloonValueForRemovedPFormat(newpropertyhash2, (childWidget as WParagraph).ParagraphFormat, ref hierarchyOrder);
    trackChangesMarkups.TypeOfMarkup = RevisionType.Formatting;
    trackChangesMarkups.ChangedValue.AddParagraph().AppendText(trackChangesMarkups.GetBalloonValueForMarkupType() + ":").CharacterFormat.Bold = true;
    trackChangesMarkups.ChangedValue.LastParagraph.AppendText(trackChangesMarkups.ConvertDictionaryValuesToString(hierarchyOrder));
    trackChangesMarkups.Position = this.LayoutArea.ClientActiveArea.Location;
    trackChangesMarkups.BallonYPosition = this.LayoutArea.ClientActiveArea.Location.Y;
    (this.m_lcOperator as Layouter).TrackChangesMarkups.Add(trackChangesMarkups);
    if (this.m_ltWidget == null)
      return;
    this.m_ltWidget.IsTrackChanges = true;
  }

  private void CreateBalloonValueForListFormat(IWidget childWidget)
  {
    TrackChangesMarkups trackChangesMarkup = (this.m_lcOperator as Layouter).TrackChangesMarkups[(this.m_lcOperator as Layouter).TrackChangesMarkups.Count - 1];
    Dictionary<int, object> dictionary = new Dictionary<int, object>((IDictionary<int, object>) (childWidget as WParagraph).ListFormat.PropertiesHash);
    Dictionary<int, object> standardDic = new Dictionary<int, object>((IDictionary<int, object>) (childWidget as WParagraph).ListFormat.OldPropertiesHash);
    this.RemoveSameValues(dictionary, standardDic);
    if (trackChangesMarkup.ChangedValue.ChildEntities.Count == 0)
      trackChangesMarkup.ChangedValue.AddParagraph();
    if (dictionary.Count > 0)
      trackChangesMarkup.ChangedValue.LastParagraph.AppendText(", " + trackChangesMarkup.DisplayBalloonValueForListFormat(dictionary, (childWidget as WParagraph).ListFormat));
    trackChangesMarkup.TypeOfMarkup = RevisionType.Formatting;
    trackChangesMarkup.Position = this.LayoutArea.ClientActiveArea.Location;
    trackChangesMarkup.BallonYPosition = this.LayoutArea.ClientActiveArea.Location.Y;
    (this.m_lcOperator as Layouter).TrackChangesMarkups.Add(trackChangesMarkup);
    if (this.m_ltWidget == null)
      return;
    this.m_ltWidget.IsTrackChanges = true;
  }

  private void CreateBalloonValueForTableFormat(IWidget childWidget)
  {
    TrackChangesMarkups trackChangesMarkups = new TrackChangesMarkups((childWidget as WTable).Document);
    Dictionary<int, object> dicToRemove = new Dictionary<int, object>((IDictionary<int, object>) (childWidget as WTable).TableFormat.PropertiesHash);
    Dictionary<int, object> standardDic = new Dictionary<int, object>((IDictionary<int, object>) (childWidget as WTable).TableFormat.OldPropertiesHash);
    this.RemoveSameValues(dicToRemove, standardDic);
    if (dicToRemove.Count > 0)
    {
      trackChangesMarkups.TypeOfMarkup = RevisionType.Formatting;
      trackChangesMarkups.ChangedValue.AddParagraph().AppendText(trackChangesMarkups.GetBalloonValueForMarkupType() + ": ").CharacterFormat.Bold = true;
      trackChangesMarkups.ChangedValue.LastParagraph.AppendText("Table");
      trackChangesMarkups.Position = this.LayoutArea.ClientActiveArea.Location;
      trackChangesMarkups.BallonYPosition = this.LayoutArea.ClientActiveArea.Location.Y;
    }
    (this.m_lcOperator as Layouter).TrackChangesMarkups.Add(trackChangesMarkups);
    if (this.m_ltWidget == null)
      return;
    this.m_ltWidget.IsTrackChanges = true;
  }

  private bool LastRowHaveDynamicPara(WTableRow row)
  {
    if (row != null)
    {
      for (int index = row.ChildEntities.Count - 1; index >= 0; --index)
      {
        WTableCell childEntity = row.ChildEntities[index] as WTableCell;
        if (childEntity.ChildEntities.Contains((IEntity) (this.m_lcOperator as Layouter).DynamicParagraph) || this.IsLastItemTable(childEntity))
          return true;
      }
    }
    return false;
  }

  private bool IsLastItemTable(WTableCell cell)
  {
    for (int index = cell.ChildEntities.Count - 1; index >= 0; --index)
    {
      Entity childEntity = cell.ChildEntities[index];
      switch (childEntity)
      {
        case WTable _:
          return this.LastRowHaveDynamicPara((childEntity as WTable).LastRow);
        case WParagraph _:
          if ((childEntity as WParagraph).m_layoutInfo != null && (childEntity as WParagraph).m_layoutInfo.IsSkip)
            continue;
          break;
      }
      return false;
    }
    return false;
  }

  private bool IsNeedToSkipMovingBreakItem(WParagraph para)
  {
    return para != null && (para.Document.Settings.CompatibilityOptions[CompatibilityOption.SplitPgBreakAndParaMark] || para == para.Document.LastParagraph || (para.Document.ActualFormatType == FormatType.Doc ? (para.Document.WordVersion <= (ushort) 268 ? 1 : 0) : 0) != 0);
  }

  private void UpdateExpressionField(IWidget childWidget)
  {
    WField wfield = childWidget as WField;
    if (!DocumentLayouter.IsFirstLayouting)
    {
      if (wfield != null && wfield.IsNumPagesInsideExpressionField && !wfield.IsFieldInsideUnknownField && (wfield.FieldType == FieldType.FieldCompare || wfield.FieldType == FieldType.FieldIf || wfield.FieldType == FieldType.FieldFormula))
      {
        wfield.Update();
        wfield.SkipLayoutingOfFieldCode();
      }
      else
      {
        if (wfield == null || wfield.FieldType != FieldType.FieldNumPages || !wfield.IsNumPageUsedForEvaluation)
          return;
        childWidget.LayoutInfo.IsSkip = true;
        for (int index = 0; index < wfield.Range.Items.Count; ++index)
          (wfield.Range.Items[index] as IWidget).LayoutInfo.IsSkip = true;
      }
    }
    else
    {
      if (wfield == null || !wfield.IsNumPageUsedForEvaluation || wfield.FieldType == FieldType.FieldNumPages)
        return;
      childWidget.LayoutInfo.IsSkip = true;
      for (int index = 0; index < wfield.Range.Items.Count; ++index)
        (wfield.Range.Items[index] as IWidget).LayoutInfo.IsSkip = true;
    }
  }

  internal LayoutedWidget CheckNullConditionAndReturnltwidget()
  {
    LayoutedWidget layoutedWidget = (this.m_lcOperator as Layouter).MaintainltWidget;
    int[] interSectingPoint = (this.m_lcOperator as Layouter).m_interSectingPoint;
    return (interSectingPoint[0] == int.MinValue || interSectingPoint[1] == int.MinValue || layoutedWidget == null || layoutedWidget.ChildWidgets.Count <= interSectingPoint[0] ? 0 : ((layoutedWidget = layoutedWidget.ChildWidgets[interSectingPoint[0]]) != null ? 1 : 0)) != 0 ? layoutedWidget.ChildWidgets[interSectingPoint[1]] : (LayoutedWidget) null;
  }

  private void FindIntersectPointAndRemovltWidgetForTable()
  {
    LayoutedWidget layoutedWidget1 = this.m_ltWidget;
    while (!(layoutedWidget1.ChildWidgets[layoutedWidget1.ChildWidgets.Count - 1].Widget is ParagraphItem) && !(layoutedWidget1.ChildWidgets[layoutedWidget1.ChildWidgets.Count - 1].Widget is WTableRow))
    {
      layoutedWidget1 = layoutedWidget1.ChildWidgets[layoutedWidget1.ChildWidgets.Count - 1];
      if (layoutedWidget1.ChildWidgets.Count == 0)
        return;
    }
    float y = layoutedWidget1.ChildWidgets[layoutedWidget1.ChildWidgets.Count - 1].Bounds.Y;
    IWidget widget = layoutedWidget1.Widget is SplitWidgetContainer ? (IWidget) (layoutedWidget1.Widget as SplitWidgetContainer).RealWidgetContainer : (layoutedWidget1.Widget is SplitTableWidget ? (IWidget) (layoutedWidget1.Widget as SplitTableWidget).TableWidget : layoutedWidget1.Widget);
    switch (widget)
    {
      case WTable _:
        (this.m_lcOperator as Layouter).DynamicTable = widget as WTable;
        break;
      case WParagraph _:
        (this.m_lcOperator as Layouter).DynamicParagraph = widget as WParagraph;
        break;
    }
    LayoutedWidget layoutedWidget2 = this.m_ltWidget;
    int index1 = 0;
    for (int index2 = layoutedWidget2.ChildWidgets.Count - 1; index2 >= 0; --index2)
    {
      if (index1 == 0 && (double) y >= (double) layoutedWidget2.ChildWidgets[index2].Bounds.Bottom)
      {
        this.m_layoutArea.UpdateDynamicRelayoutBounds(layoutedWidget2.ChildWidgets[index2].Bounds.X, layoutedWidget2.ChildWidgets[index2].Bounds.Y, false, 0.0f);
        this.m_curWidgetIndex = index2;
        (this.m_lcOperator as Layouter).m_interSectingPoint[0] = index2;
        this.m_ltWidget.ChildWidgets.RemoveRange(index2, this.m_ltWidget.ChildWidgets.Count - index2);
        ++index1;
        layoutedWidget2 = (this.m_lcOperator as Layouter).MaintainltWidget.ChildWidgets[index2];
        index2 = layoutedWidget2.ChildWidgets.Count;
      }
      else if ((double) y >= (double) layoutedWidget2.ChildWidgets[index2].Bounds.Bottom)
      {
        (this.m_lcOperator as Layouter).m_interSectingPoint[index1] = index2;
        layoutedWidget2 = layoutedWidget2.ChildWidgets[index2];
        index2 = layoutedWidget2.ChildWidgets.Count;
        if (layoutedWidget2.ChildWidgets.Count <= 0 || !(layoutedWidget2.ChildWidgets[0].Widget is ParagraphItem) && !(layoutedWidget2.ChildWidgets[0].Widget is SplitStringWidget))
          ++index1;
        else
          break;
      }
    }
    if (index1 != 0)
      return;
    this.m_ltWidget.ChildWidgets.RemoveRange(0, this.m_ltWidget.ChildWidgets.Count);
    (this.m_lcOperator as Layouter).m_interSectingPoint[0] = 0;
    (this.m_lcOperator as Layouter).m_interSectingPoint[1] = 0;
  }

  private void LayoutFootnoteSplittedWidgets(IWidget childWidget)
  {
    if ((this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Count <= 0 || !(childWidget is SplitWidgetContainer) || !((childWidget as SplitWidgetContainer).RealWidgetContainer is WSection))
      return;
    float height = 0.0f;
    SplitWidgetContainer[] array = new SplitWidgetContainer[(this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Count];
    (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.CopyTo(array);
    (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Clear();
    if ((this.m_lcOperator as Layouter).IsNeedToRestartFootnote && (array[0].RealWidgetContainer as WTextBody).Owner != null)
    {
      this.LayoutFootnoteTextBody((IWidgetContainer) ((array[0].RealWidgetContainer as WTextBody).Owner as WFootnote).Document.Footnotes.ContinuationSeparator, ref height, this.m_layoutArea.ClientActiveArea.Height, false);
      (this.m_lcOperator as Layouter).IsNeedToRestartFootnote = false;
    }
    for (int index = 0; index < array.Length; ++index)
    {
      if (index > 0)
        height = 0.0f;
      this.LayoutFootnoteTextBody((IWidgetContainer) array[index], ref height, this.m_layoutArea.ClientActiveArea.Height, false);
      this.CreateLayoutArea(new RectangleF(this.m_layoutArea.ClientActiveArea.X, this.m_layoutArea.ClientActiveArea.Y, this.m_layoutArea.ClientActiveArea.Width, this.m_layoutArea.ClientActiveArea.Height - height));
    }
    int count = (this.m_lcOperator as Layouter).FootNoteSectionIndex.Count;
    while ((this.m_lcOperator as Layouter).FootnoteWidgets.Count > (this.m_lcOperator as Layouter).FootNoteSectionIndex.Count)
      (this.m_lcOperator as Layouter).FootNoteSectionIndex.Add(count + 1);
    this.UpdateForceFitLayoutState((LayoutContext) this);
  }

  private void LayoutEndnoteSplittedWidgets(IWidget childWidget)
  {
    if ((this.m_lcOperator as Layouter).EndnoteSplittedWidgets.Count <= 0 || !(childWidget is SplitWidgetContainer) || !((childWidget as SplitWidgetContainer).RealWidgetContainer is WSection))
      return;
    float height = 0.0f;
    SplitWidgetContainer[] array = new SplitWidgetContainer[(this.m_lcOperator as Layouter).EndnoteSplittedWidgets.Count];
    (this.m_lcOperator as Layouter).EndnoteSplittedWidgets.CopyTo(array);
    (this.m_lcOperator as Layouter).EndnoteSplittedWidgets.Clear();
    if ((this.m_lcOperator as Layouter).IsNeedToRestartEndnote)
    {
      this.LayoutEndnoteTextBody((IWidgetContainer) (array[0].RealWidgetContainer as WTextBody).Document.Footnotes.ContinuationSeparator, ref height, this.m_layoutArea.ClientActiveArea.Height);
      (this.m_lcOperator as Layouter).IsNeedToRestartEndnote = false;
    }
    for (int index = 0; index < array.Length; ++index)
    {
      if (index > 0)
        height = 0.0f;
      this.LayoutEndnoteTextBody((IWidgetContainer) array[index], ref height, this.m_layoutArea.ClientActiveArea.Height);
      this.CreateLayoutArea(new RectangleF(this.m_layoutArea.ClientActiveArea.X, this.m_layoutArea.ClientActiveArea.Y, this.m_layoutArea.ClientActiveArea.Width, this.m_layoutArea.ClientActiveArea.Height - height));
    }
    int count = (this.m_lcOperator as Layouter).EndNoteSectionIndex.Count;
    while ((this.m_lcOperator as Layouter).EndnoteWidgets.Count > (this.m_lcOperator as Layouter).EndNoteSectionIndex.Count)
      (this.m_lcOperator as Layouter).EndNoteSectionIndex.Add(count + 1);
  }

  private bool IsParagraphSplittedByPageBreak(WParagraph paragraph)
  {
    if (this.SplittedWidget is SplitWidgetContainer splittedWidget && (splittedWidget.m_currentChild is WParagraph || splittedWidget.m_currentChild is SplitWidgetContainer && (splittedWidget.m_currentChild as SplitWidgetContainer).RealWidgetContainer is WParagraph))
    {
      if (!(paragraph.PreviousSibling is WParagraph previousSibling))
        return false;
      int num = !(splittedWidget.m_currentChild is WParagraph) ? ((splittedWidget.m_currentChild as SplitWidgetContainer).m_currentChild is Entity ? ((splittedWidget.m_currentChild as SplitWidgetContainer).m_currentChild as Entity).Index : -1) : previousSibling.ChildEntities.Count;
      if (num > 0 && previousSibling.ChildEntities.Count > num - 1 && previousSibling.ChildEntities[num - 1] is Break && (previousSibling.ChildEntities[num - 1] as Break).BreakType == BreakType.PageBreak)
        return true;
    }
    return false;
  }

  private bool IsPreviousParagraphHaveSectionBreak(WParagraph paragraph)
  {
    WSection previousSibling = this.GetBaseEntity((Entity) paragraph).PreviousSibling as WSection;
    if (paragraph.PreviousSibling != null || previousSibling == null)
      return false;
    Entity childEntity1 = previousSibling.ChildEntities[0];
    if (childEntity1 is WTextBody)
    {
      WTextBody wtextBody = childEntity1 as WTextBody;
      if (wtextBody.ChildEntities[wtextBody.ChildEntities.Count - 1] is WParagraph childEntity2 && childEntity2.IsParagraphHasSectionBreak())
        return true;
    }
    return false;
  }

  private bool IsHeaderContentExceedsTopMargin()
  {
    IWSection currentSection = (this.m_lcOperator as Layouter).CurrentSection;
    return currentSection != null && (double) (this.m_lcOperator as Layouter).PageTopMargin > (double) currentSection.PageSetup.Margins.Top;
  }

  private void UpdateTextBodyItemPosition(IWidget childWidget)
  {
    if (childWidget is WParagraph)
    {
      ParagraphLayoutInfo layoutInfo = childWidget.LayoutInfo as ParagraphLayoutInfo;
      WParagraph paragraph = childWidget as WParagraph;
      layoutInfo.IsFirstLine = true;
      float num = 0.0f;
      if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && this.IsForceFitLayout && this.IsBaseFromSection((Entity) paragraph) && DocumentLayouter.PageNumber != 1 && !this.IsPreviousParagraphHaveSectionBreak(paragraph) && (paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || !paragraph.ParagraphFormat.PageBreakBefore && !this.IsParagraphSplittedByPageBreak(paragraph) && !this.IsHeaderContentExceedsTopMargin()) && !(this.m_lcOperator as Layouter).IsLayoutingFootnote)
      {
        num = layoutInfo.Margins.Top;
        layoutInfo.Margins.Top = 0.0f;
      }
      this.UpdateParagraphMargins(paragraph);
      if ((double) num > 0.0)
        layoutInfo.TopMargin = num;
      if (this.IsInFrame(paragraph))
      {
        RectangleF frameBounds = this.GetFrameBounds(paragraph, this.m_layoutArea.ClientActiveArea);
        (this.m_lcOperator as Layouter).ParagraphYPosition = layoutInfo.YPosition = frameBounds.Y;
        layoutInfo.XPosition = frameBounds.X;
      }
      else
      {
        layoutInfo.YPosition = this.m_layoutArea.ClientActiveArea.Y;
        layoutInfo.YPosition -= this.GetParagraphTopMargin(paragraph);
        if ((double) layoutInfo.PargaraphOriginalYPosition != -3.4028234663852886E+38)
          (this.m_lcOperator as Layouter).ParagraphYPosition = layoutInfo.PargaraphOriginalYPosition;
        else
          (this.m_lcOperator as Layouter).ParagraphYPosition = layoutInfo.YPosition;
        layoutInfo.XPosition = this.m_layoutArea.ClientActiveArea.X;
      }
      float xposition = layoutInfo.XPosition;
      this.UpdateParagraphXPositionBasedOnTextWrap(paragraph, xposition, layoutInfo.YPosition);
      if ((double) xposition != (double) layoutInfo.XPosition)
        paragraph.IsXpositionUpated = true;
    }
    if (childWidget is WTable)
      (childWidget.LayoutInfo as TableLayoutInfo).IsHeaderNotRepeatForAllPages = false;
    if (!(childWidget is SplitWidgetContainer) || !((childWidget as SplitWidgetContainer).RealWidgetContainer is WParagraph))
      return;
    WParagraph realWidgetContainer = (childWidget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
    int count = realWidgetContainer.ChildEntities.Count;
    if (realWidgetContainer.IsInCell || count <= (childWidget as SplitWidgetContainer).Count || !(realWidgetContainer.m_layoutInfo is ParagraphLayoutInfo))
      return;
    (realWidgetContainer.m_layoutInfo as ParagraphLayoutInfo).YPosition = this.m_layoutArea.ClientActiveArea.Y;
    (this.m_lcOperator as Layouter).ParagraphYPosition = (realWidgetContainer.m_layoutInfo as ParagraphLayoutInfo).YPosition;
    (realWidgetContainer.m_layoutInfo as ParagraphLayoutInfo).XPosition = this.m_layoutArea.ClientActiveArea.X;
    this.UpdateParagraphXPositionBasedOnTextWrap(realWidgetContainer, (realWidgetContainer.m_layoutInfo as ParagraphLayoutInfo).XPosition, (childWidget.LayoutInfo as ParagraphLayoutInfo).YPosition);
  }

  internal float GetFootnoteHeight()
  {
    float height = 0.0f;
    WParagraph wparagraph = this.m_currChildLW.Widget is WParagraph ? this.m_currChildLW.Widget as WParagraph : (!(this.m_currChildLW.Widget is SplitWidgetContainer) || !((this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph) ? (WParagraph) null : (this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph);
    if (wparagraph != null)
    {
      if (wparagraph.IsInCell)
      {
        if (!((wparagraph.GetOwnerEntity() as WTableCell).OwnerRow.m_layoutInfo as RowLayoutInfo).IsFootnoteReduced)
          this.m_currChildLW.GetFootnoteHeight(ref height);
      }
      else
        this.m_currChildLW.GetFootnoteHeight(ref height);
    }
    else if (this.m_currChildLW.Widget is WTable)
      this.m_currChildLW.GetFootnoteHeight(ref height);
    return height;
  }

  private bool IsUpdatedParagraph(ParagraphLayoutInfo paraInfo)
  {
    if (!DocumentLayouter.IsLayoutingHeaderFooter)
      return false;
    return (double) paraInfo.TopMargin > 0.0 || (double) paraInfo.BottomMargin > 0.0 || (double) paraInfo.TopPadding > 0.0 || (double) paraInfo.BottomPadding > 0.0;
  }

  private void UpdateParagraphMargins(WParagraph paragraph)
  {
    ParagraphLayoutInfo layoutInfo = ((IWidget) paragraph).LayoutInfo as ParagraphLayoutInfo;
    if (DocumentLayouter.IsFirstLayouting && !this.IsUpdatedParagraph(layoutInfo))
    {
      layoutInfo.TopMargin = layoutInfo.Margins.Top;
      layoutInfo.BottomMargin = layoutInfo.Margins.Bottom;
      layoutInfo.TopPadding = layoutInfo.Paddings.Top;
      layoutInfo.BottomPadding = layoutInfo.Paddings.Bottom;
      if (!paragraph.IsParagraphBeforeSpacingNeedToSkip())
        return;
      layoutInfo.Margins.Top = 0.0f;
      layoutInfo.Margins.Bottom = 0.0f;
    }
    else
    {
      layoutInfo.Margins.Top = layoutInfo.TopMargin;
      layoutInfo.Margins.Bottom = layoutInfo.BottomMargin;
      layoutInfo.Paddings.Top = layoutInfo.TopPadding;
      layoutInfo.Paddings.Bottom = layoutInfo.BottomPadding;
      this.ResetFloatingEntityProperty(paragraph);
    }
  }

  protected virtual void MarkAsNotFitted(LayoutContext childContext, bool isFootnote)
  {
    if (childContext.Widget is WFootnote)
      childContext.Widget.InitLayoutInfo();
    this.IsVerticalNotFitted = childContext.IsVerticalNotFitted;
    IWidget splittedWidget = (IWidget) null;
    bool commitKeepWithNext = this.IsNeedToCommitKeepWithNext();
    this.CommitKeepWithNext(ref splittedWidget, commitKeepWithNext);
    if (this.m_ltWidget.ChildWidgets.Count > 0 && this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Widget is WParagraph)
    {
      this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].IsLastItemInPage = true;
      RectangleF bounds = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Bounds;
      Borders borders = (this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Widget as WParagraph).ParagraphFormat.Borders;
      if (!borders.NoBorder && borders.Bottom.BorderType != BorderStyle.None && (double) (this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Widget.LayoutInfo as ParagraphLayoutInfo).Paddings.Bottom == 0.0)
      {
        (this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Widget.LayoutInfo as ParagraphLayoutInfo).Paddings.Bottom = borders.Bottom.Space;
        bounds.Height += borders.Bottom.Space;
        this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Bounds = bounds;
      }
    }
    WSection wsection = this.m_sptWidget is WSection ? this.m_sptWidget as WSection : (this.m_sptWidget is SplitWidgetContainer ? (this.m_sptWidget as SplitWidgetContainer).RealWidgetContainer as WSection : (WSection) null);
    if (this.IsNeedToSetNotFitted(commitKeepWithNext))
      this.m_ltState = LayoutState.NotFitted;
    else if (this.m_bAtLastOneChildFitted || (this.m_lcOperator as Layouter).FootnoteWidgets.Count > 1 && wsection != null && (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Height - ((double) (this.m_lcOperator as Layouter).FootnoteWidgets[(this.m_lcOperator as Layouter).FootnoteWidgets.Count - 1].Bounds.Bottom - (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Y) < (double) this.CurrentChildWidget.LayoutInfo.Size.Height || splittedWidget == null && this.m_sptWidget is WParagraph && !(this.m_sptWidget as WParagraph).IsInCell && !this.IsInFrame(this.m_sptWidget as WParagraph) && (this.m_lcOperator as Layouter).DynamicParagraph == null)
    {
      if (splittedWidget != null)
      {
        this.SplitedUpWidget(splittedWidget, false);
      }
      else
      {
        if (isFootnote && this.CurrentChildWidget is WFootnote && (this.m_lcOperator as Layouter).FootnoteWidgets.Count == 0)
        {
          if (this.WidgetContainer is SplitWidgetContainer)
            this.m_sptWidget = (IWidget) (this.WidgetContainer as SplitWidgetContainer);
          else
            this.m_sptWidget = (IWidget) new SplitWidgetContainer(this.WidgetContainer, this.WidgetContainer.WidgetInnerCollection[0] as IWidget, 0);
          this.m_ltState = LayoutState.NotFitted;
          return;
        }
        this.SplitedUpWidget(this.CurrentChildWidget, false);
      }
      if (childContext is LCLineContainer)
      {
        WParagraph wparagraph = childContext.Widget is SplitWidgetContainer ? (childContext.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph : childContext.Widget as WParagraph;
        WParagraph dynamicParagraph = (this.m_lcOperator as Layouter).DynamicParagraph;
        if (wparagraph != null && (this.m_lcOperator as Layouter).DynamicParagraph == wparagraph && !wparagraph.ParagraphFormat.IsInFrame())
          (this.m_lcOperator as Layouter).DynamicParagraph = (WParagraph) null;
        else if (wparagraph != null && wparagraph.IsInCell && dynamicParagraph != null && dynamicParagraph.IsInCell && wparagraph.Owner == dynamicParagraph.Owner && !wparagraph.ParagraphFormat.IsInFrame() && dynamicParagraph.Index > wparagraph.Index)
          (this.m_lcOperator as Layouter).DynamicParagraph = (WParagraph) null;
      }
      this.UpdateSplittedWidgetIndex(childContext);
      if (this.LayoutInfo is ParagraphLayoutInfo layoutInfo)
        layoutInfo.IsFirstLine = false;
      this.m_ltState = LayoutState.Splitted;
    }
    else
      this.m_ltState = LayoutState.NotFitted;
  }

  internal bool IsNeedToSetNotFitted(bool isKeep)
  {
    return this.m_ltWidget.Widget is BlockContentControl && (!isKeep || (this.m_ltWidget.Widget as BlockContentControl).ChildEntities.Count == 0) && (this.m_ltWidget.Widget.LayoutInfo.IsFirstItemInPage ? 0 : (this.IsNotFittedItem((this.m_ltWidget.Widget as BlockContentControl).PreviousSibling as IWidget) ? 1 : 0)) != 0 || this.m_ltWidget.Widget is SplitWidgetContainer && (this.m_ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer is BlockContentControl && (!isKeep || ((this.m_ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer as BlockContentControl).ChildEntities.Count == 0) && !(this.m_ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer.LayoutInfo.IsFirstItemInPage;
  }

  private IWidget GetPriviousSibling(IWidget widget)
  {
    switch (widget)
    {
      case WParagraph _:
        return (widget as WParagraph).PreviousSibling as IWidget;
      case WTable _:
        return (widget as WTable).PreviousSibling as IWidget;
      default:
        return (IWidget) null;
    }
  }

  private bool IsNotFittedItem(IWidget widget)
  {
    if (widget != null && !widget.LayoutInfo.IsKeepWithNext)
      return true;
    return widget != null && !widget.LayoutInfo.IsFirstItemInPage && this.IsNotFittedItem(this.GetPriviousSibling(widget));
  }

  internal void RemoveAutoHyphenatedString(IWidget SplittedWidget, bool isAutoHyphen)
  {
    IWidget widget = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Widget;
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

  private bool IsPageBreakInTable(IWidget LeafWidget)
  {
    if (LeafWidget is Break)
    {
      Break @break = LeafWidget as Break;
      if (@break.OwnerParagraph != null && @break.OwnerParagraph.IsInCell)
        return true;
    }
    return false;
  }

  internal WParagraph GetParagraph()
  {
    WParagraph paragraph = (WParagraph) null;
    if (this.m_currChildLW.Widget is WParagraph)
      paragraph = this.m_currChildLW.Widget as WParagraph;
    else if (this.m_currChildLW.Widget is SplitWidgetContainer && (this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
      paragraph = (this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
    return paragraph;
  }

  private void RemoveXmlMathParaItems(WParagraph paragraph)
  {
    for (int index1 = 0; index1 < paragraph.Items.Count; ++index1)
    {
      if (paragraph.Items[index1] is XmlParagraphItem)
      {
        XmlParagraphItem xmlParagraphItem = paragraph.Items[index1] as XmlParagraphItem;
        if (xmlParagraphItem.MathParaItemsCollection != null && xmlParagraphItem.MathParaItemsCollection.Count > 0)
        {
          foreach (ParagraphItem mathParaItems in (Syncfusion.DocIO.DLS.CollectionImpl) xmlParagraphItem.MathParaItemsCollection)
          {
            int index2 = paragraph.Items.InnerList.IndexOf((object) mathParaItems);
            if (index2 != -1)
              paragraph.Items.RemoveAt(index2);
          }
        }
      }
    }
  }

  protected virtual void MarkAsFitted(LayoutContext childContext)
  {
    this.AddChildLW(childContext);
    switch (childContext)
    {
      case LCContainer _ when !(childContext is LCLineContainer) && this.m_currChildLW.IsNotFitted:
        return;
      case LCLineContainer _:
        WParagraph paragraph = this.GetParagraph();
        if (paragraph != null && !paragraph.HasSDTInlineItem)
          this.RemoveXmlMathParaItems(paragraph);
        string breakCFormatBalloonValue = "";
        if (paragraph != null && paragraph.BreakCharacterFormat.IsChangedFormat && this.IsNeedToShowFormattingMarkUp((TextBodyItem) paragraph) && this.IsNeedToCreateBalloonForBreakCharacterFormat(paragraph, ref breakCFormatBalloonValue) && !(this.m_lcOperator as Layouter).IsLayoutingTrackChanges)
        {
          TrackChangesMarkups trackChangesMarkups = new TrackChangesMarkups(paragraph.Document);
          trackChangesMarkups.TypeOfMarkup = RevisionType.Formatting;
          trackChangesMarkups.ChangedValue.AddParagraph().AppendText(trackChangesMarkups.GetBalloonValueForMarkupType() + ": ").CharacterFormat.Bold = true;
          trackChangesMarkups.ChangedValue.LastParagraph.AppendText(breakCFormatBalloonValue);
          trackChangesMarkups.Position = new PointF(this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].Bounds.Right, this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].Bounds.Y);
          trackChangesMarkups.BallonYPosition = this.LayoutArea.ClientActiveArea.Location.Y;
          (this.m_lcOperator as Layouter).TrackChangesMarkups.Add(trackChangesMarkups);
          if (this.m_ltWidget != null)
            this.m_ltWidget.IsTrackChanges = true;
          this.ShiftTrackChangesBalloons(this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].Bounds.Y, this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].Bounds.Bottom, this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].Bounds.Bottom);
          break;
        }
        break;
    }
    IWSection baseEntity = (IWSection) (this.GetBaseEntity(childContext.Widget as Entity) as WSection);
    if (childContext.Widget is WFootnote && (childContext.Widget as WFootnote).FootnoteType == FootnoteType.Footnote && baseEntity.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && (baseEntity as WSection).IsSectionFitInSamePage)
    {
      (baseEntity as WSection).IsSectionFitInSamePage = false;
      this.m_ltWidget.IsNotFitted = true;
    }
    else
    {
      if (childContext.Widget is WFootnote && (childContext.Widget as WFootnote).FootnoteType == FootnoteType.Footnote && !(childContext.Widget as WFootnote).IsLayouted && this.IsNeedToLayoutFootnoteTextBody(childContext))
      {
        bool twoLinesLayouted = (this.m_lcOperator as Layouter).IsTwoLinesLayouted;
        this.LayoutFootnote(childContext.Widget as WFootnote, this.m_currChildLW.Owner, false);
        (this.m_lcOperator as Layouter).IsTwoLinesLayouted = twoLinesLayouted;
        if ((this.m_lcOperator as Layouter).FootnoteWidgets.Count == 0)
        {
          this.MarkAsNotFitted(childContext, true);
          (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Clear();
          return;
        }
        while ((this.m_lcOperator as Layouter).FootnoteWidgets.Count > (this.m_lcOperator as Layouter).FootNoteSectionIndex.Count)
          (this.m_lcOperator as Layouter).FootNoteSectionIndex.Add(baseEntity.Document.Sections.IndexOf(baseEntity));
      }
      else if (childContext.Widget is WFootnote && (childContext.Widget as WFootnote).FootnoteType == FootnoteType.Endnote && !(this.m_lcOperator as Layouter).EndnotesInstances.Contains(childContext.Widget as Entity))
        (this.m_lcOperator as Layouter).EndnotesInstances.Add(childContext.Widget as Entity);
      bool flag = this.NextChildWidget();
      if (this.WidgetContainer is SplitWidgetContainer && (this.WidgetContainer as SplitWidgetContainer).RealWidgetContainer is WTableCell && !flag)
        this.m_sptWidget = (IWidget) null;
      ParagraphItem widget = childContext.Widget as ParagraphItem;
      if (childContext.LayoutInfo.IsLineBreak && this.CurrentChildWidget != null || this.IsTableNextParagraphNeedToSplit() || (widget != null && this.IsHorizantalRule(this.CurrentChildWidget) && widget.OwnerParagraph.HasInlineItem(widget.Index) || this.IsHorizantalRule(childContext.Widget)) && this.CurrentChildWidget != null)
      {
        this.SplitedUpWidget(this.CurrentChildWidget, false);
        this.m_ltState = LayoutState.Splitted;
        this.m_ltWidget.TextTag = "Splitted";
      }
      else if (childContext.LayoutInfo.IsPageBreakItem && !this.IsPageBreakInTable(childContext.Widget))
      {
        if (this.CurrentChildWidget != null)
          this.SplitedUpWidget(this.CurrentChildWidget, false);
        else
          this.m_sptWidget = (IWidget) new SplitWidgetContainer(this.WidgetContainer, childContext.Widget, this.WidgetContainer.Count - 1);
        if (this.CurrentChildWidget != null && (this.CurrentChildWidget as Entity).PreviousSibling is WTable)
          this.m_ltState = LayoutState.Splitted;
        else
          this.m_ltState = LayoutState.Breaked;
      }
      else if (!this.m_bAtLastOneChildFitted)
        this.m_bAtLastOneChildFitted = true;
      WSection wsection = this.m_sptWidget is WSection ? this.m_sptWidget as WSection : (this.m_sptWidget is SplitWidgetContainer ? (this.m_sptWidget as SplitWidgetContainer).RealWidgetContainer as WSection : (WSection) null);
      if (!flag && wsection != null && this.m_ltState != LayoutState.Splitted && (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Count > 0)
      {
        this.m_sptWidget = (IWidget) new SplitWidgetContainer(this.WidgetContainer);
        this.m_ltState = LayoutState.Splitted;
      }
      this.UpdateForceFitLayoutState(childContext);
    }
  }

  private bool IsHorizantalRule(IWidget currentChildWidget)
  {
    return currentChildWidget is ParagraphItem && (currentChildWidget is Shape && (currentChildWidget as Shape).IsHorizontalRule || currentChildWidget is WPicture && (currentChildWidget as WPicture).IsShape && (currentChildWidget as WPicture).PictureShape.IsHorizontalRule);
  }

  private bool IsNeedToCreateBalloonForBreakCharacterFormat(
    WParagraph paragraph,
    ref string breakCFormatBalloonValue)
  {
    bool flag = false;
    if (paragraph != null && paragraph.IsEmptyParagraph())
    {
      WParagraph previousParagraph = this.GetPreviousParagraph(paragraph);
      if (previousParagraph != null)
      {
        flag = previousParagraph.BreakCharacterFormat.IsChangedFormat;
      }
      else
      {
        WSection ownerSection = paragraph.GetOwnerSection();
        WSection previousSibling = ownerSection == null || ownerSection.Index <= 0 ? (WSection) null : ownerSection.PreviousSibling as WSection;
        if (previousSibling != null)
        {
          WParagraph paragraph1 = previousSibling.Paragraphs.Count > 0 ? previousSibling.Paragraphs[previousSibling.Paragraphs.Count - 1] : (WParagraph) null;
          string cformatBallloonValue = paragraph1 == null || !paragraph1.BreakCharacterFormat.IsChangedFormat ? (string) null : this.GetCFormatBallloonValue(paragraph1.BreakCharacterFormat);
          breakCFormatBalloonValue = this.GetCFormatBallloonValue(paragraph.BreakCharacterFormat);
          return !string.IsNullOrEmpty(breakCFormatBalloonValue) && !string.Equals(breakCFormatBalloonValue, cformatBallloonValue, StringComparison.OrdinalIgnoreCase);
        }
      }
    }
    else
    {
      for (int index = paragraph.ChildEntities.Count - 1; index >= 0; --index)
      {
        Entity childEntity = paragraph.ChildEntities[index];
        switch (childEntity)
        {
          case WTextRange _:
          case Break _:
            int num;
            switch (childEntity)
            {
              case WTextRange _ when (childEntity as WTextRange).IsChangedCFormat:
                num = 1;
                break;
              case Break _:
                num = (childEntity as Break).CharacterFormat.IsChangedFormat ? 1 : 0;
                break;
              default:
                num = 0;
                break;
            }
            flag = num != 0;
            goto label_16;
          case BookmarkStart _:
          case BookmarkEnd _:
          case WFieldMark _:
          case EditableRangeStart _:
          case EditableRangeEnd _:
            continue;
          default:
            goto label_16;
        }
      }
    }
label_16:
    breakCFormatBalloonValue = this.GetCFormatBallloonValue(paragraph.BreakCharacterFormat);
    if (!flag)
      return !string.IsNullOrEmpty(breakCFormatBalloonValue);
    string formattedBalloonValue = this.GetPreviousFormattedBalloonValue();
    return !string.IsNullOrEmpty(breakCFormatBalloonValue) && !string.Equals("Formatted: " + breakCFormatBalloonValue, formattedBalloonValue, StringComparison.OrdinalIgnoreCase);
  }

  private string GetCFormatBallloonValue(WCharacterFormat characterFormat)
  {
    TrackChangesMarkups trackChangesMarkups = new TrackChangesMarkups(characterFormat.Document);
    Dictionary<int, object> dictionary1 = new Dictionary<int, object>((IDictionary<int, object>) characterFormat.PropertiesHash);
    Dictionary<int, object> standardDic1 = new Dictionary<int, object>((IDictionary<int, object>) characterFormat.OldPropertiesHash);
    this.RemoveSameValues(dictionary1, standardDic1);
    Dictionary<int, string> hierarchyOrder = new Dictionary<int, string>();
    if (dictionary1.Count > 0)
      trackChangesMarkups.DisplayBalloonValueCFormat(FontScriptType.English, dictionary1, characterFormat, ref hierarchyOrder);
    Dictionary<int, object> standardDic2 = new Dictionary<int, object>((IDictionary<int, object>) characterFormat.PropertiesHash);
    Dictionary<int, object> dictionary2 = new Dictionary<int, object>((IDictionary<int, object>) characterFormat.OldPropertiesHash);
    this.RemoveSameValues(dictionary2, standardDic2);
    if (dictionary2.Count > 0)
      trackChangesMarkups.DisplayBalloonValueforRemovedCFormat(dictionary2, characterFormat, ref hierarchyOrder);
    return trackChangesMarkups.ConvertDictionaryValuesToString(hierarchyOrder);
  }

  private string GetPreviousFormattedBalloonValue()
  {
    for (int index = (this.m_lcOperator as Layouter).TrackChangesMarkups.Count - 1; index >= 0; --index)
    {
      if ((this.m_lcOperator as Layouter).TrackChangesMarkups[index].TypeOfMarkup == RevisionType.Formatting && (this.m_lcOperator as Layouter).TrackChangesMarkups[index].ChangedValue.ChildEntities.Count > 0)
        return (this.m_lcOperator as Layouter).TrackChangesMarkups[index].ChangedValue.LastParagraph.Text;
    }
    return "";
  }

  private bool HasCommentMark(string commentId)
  {
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.m_currChildLW.ChildWidgets)
    {
      if (childWidget.Widget is WCommentMark && (childWidget.Widget as WCommentMark).CommentId == commentId && ((childWidget.Widget as WCommentMark).Type == CommentMarkType.CommentEnd || (childWidget.Widget as WCommentMark).Type == CommentMarkType.CommentStart && (childWidget.Widget as WCommentMark).Comment != null && (childWidget.Widget as WCommentMark).Comment.CommentRangeEnd == null))
        return true;
    }
    return false;
  }

  protected void ShiftTrackChangesBalloons(
    float lineYPostion,
    float bottomPositionWithLineSpacing,
    float bottomPositionWithoutLineSpacing)
  {
    if ((this.m_lcOperator as Layouter).IsLayoutingTrackChanges)
      return;
    for (int index = (this.m_lcOperator as Layouter).TrackChangesMarkups.Count - 1; index >= 0 && this.m_currChildLW.ChildWidgets.Count > 0; --index)
    {
      float num = (float) Math.Round((double) (this.m_lcOperator as Layouter).TrackChangesMarkups[index].Position.Y, 2);
      lineYPostion = (float) Math.Round((double) lineYPostion, 2);
      bottomPositionWithLineSpacing = (float) Math.Round((double) bottomPositionWithLineSpacing, 2);
      if ((double) num < (double) lineYPostion)
        break;
      if ((double) num >= (double) lineYPostion && (double) num <= (double) bottomPositionWithLineSpacing)
      {
        if ((this.m_lcOperator as Layouter).TrackChangesMarkups[index] is CommentsMarkups)
        {
          CommentsMarkups trackChangesMarkup = (this.m_lcOperator as Layouter).TrackChangesMarkups[index] as CommentsMarkups;
          if (this.HasCommentMark(trackChangesMarkup.CommentID))
          {
            WParagraph paragraph = this.GetParagraph();
            if (paragraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.Multiple)
            {
              if ((double) bottomPositionWithoutLineSpacing - (double) lineYPostion > 0.0)
              {
                trackChangesMarkup.ExtraSpacing = bottomPositionWithLineSpacing - bottomPositionWithoutLineSpacing;
              }
              else
              {
                float height = paragraph.m_layoutInfo.Size.Height;
                trackChangesMarkup.ExtraSpacing = bottomPositionWithLineSpacing - (height + lineYPostion);
              }
            }
            (this.m_lcOperator as Layouter).TrackChangesMarkups[index].Position = new PointF((this.m_lcOperator as Layouter).TrackChangesMarkups[index].Position.X, bottomPositionWithLineSpacing - 0.125f);
          }
        }
        else
          (this.m_lcOperator as Layouter).TrackChangesMarkups[index].Position = new PointF((this.m_lcOperator as Layouter).TrackChangesMarkups[index].Position.X, bottomPositionWithLineSpacing - 0.125f);
      }
    }
  }

  private bool IsNeedToLayoutFootnoteTextBody(LayoutContext childContext)
  {
    IWSection baseEntity = (IWSection) (this.GetBaseEntity(childContext.Widget as Entity) as WSection);
    return baseEntity.PageSetup.FootnotePosition == FootnotePosition.PrintAtBottomOfPage && (childContext.Widget as WFootnote).OwnerParagraph.ParagraphFormat.WidowControl && (this.m_lcOperator as Layouter).IsTwoLinesLayouted || !(childContext.Widget as WFootnote).OwnerParagraph.ParagraphFormat.WidowControl || baseEntity.PageSetup.FootnotePosition != FootnotePosition.PrintAtBottomOfPage;
  }

  private void MarkAsWrapText(LayoutContext childContext)
  {
    this.AddChildLW(childContext);
    int num = this.m_curWidgetIndex;
    if (this.WidgetContainer is SplitWidgetContainer)
      num = this.WidgetContainer.WidgetInnerCollection.InnerList.IndexOf((object) childContext.Widget);
    if (childContext.Widget is SplitStringWidget)
    {
      if (num == -1)
        num = (childContext.Widget as SplitStringWidget).m_prevWidgetIndex;
      else
        (childContext.SplittedWidget as SplitStringWidget).m_prevWidgetIndex = num;
    }
    if (num < this.WidgetContainer.WidgetInnerCollection.InnerList.Count)
    {
      EntityCollection widgetInnerCollection = this.WidgetContainer.WidgetInnerCollection;
      widgetInnerCollection.InnerList.Insert(num + 1, (object) childContext.SplittedWidget);
      widgetInnerCollection.UpdateIndex(num + 2, true);
    }
    else
      this.WidgetContainer.WidgetInnerCollection.InnerList.Add((object) childContext.SplittedWidget);
    this.m_bAtLastOneChildFitted = true;
    this.NextChildWidget();
  }

  private bool IsTableNextParagraphNeedToSplit()
  {
    if (!(this.CurrentChildWidget is WParagraph currentChildWidget) || currentChildWidget.IsInCell || !(currentChildWidget.PreviousSibling is WTable previousSibling))
      return false;
    TableLayoutInfo layoutInfo = previousSibling.m_layoutInfo as TableLayoutInfo;
    return this.m_currChildLW.ChildWidgets != null && this.m_currChildLW.ChildWidgets.Count > 0 && this.m_currChildLW.ChildWidgets[0].Widget is WTableRow widget && layoutInfo != null && !currentChildWidget.IsInCell && !widget.IsHeader && layoutInfo.IsHeaderNotRepeatForAllPages && this.IsWord2013(previousSibling.Document) && (double) this.m_currChildLW.Bounds.Height != 0.0;
  }

  protected virtual void MarkAsSplitted(LayoutContext childContext)
  {
    this.IsVerticalNotFitted = childContext.IsVerticalNotFitted;
    if (this.LayoutInfo is ParagraphLayoutInfo layoutInfo1)
      layoutInfo1.IsFirstLine = false;
    this.AddChildLW(childContext);
    if (childContext is LCLineContainer && this.m_currChildLW.Widget is WParagraph)
    {
      if ((this.m_currChildLW.Widget as WParagraph).IsParagraphBeforeSpacingNeedToSkip() && this.m_currChildLW.Widget.LayoutInfo is ParagraphLayoutInfo layoutInfo2)
      {
        layoutInfo2.Margins.Top = layoutInfo2.TopMargin;
        layoutInfo2.Margins.Bottom = layoutInfo2.BottomMargin;
      }
    }
    if (childContext is LCContainer && !(childContext is LCLineContainer) && this.m_currChildLW.IsNotFitted)
      return;
    this.UpdateSplittedWidgetIndex(childContext);
    this.SplitedUpWidget(childContext.SplittedWidget, false);
    this.m_ltState = LayoutState.Splitted;
    this.UpdateForceFitLayoutState(childContext);
  }

  private void UpdateSplittedWidgetIndex(LayoutContext childContext)
  {
    int num = this.m_curWidgetIndex;
    if (this.WidgetContainer is SplitWidgetContainer)
      num = this.WidgetContainer.WidgetInnerCollection.InnerList.IndexOf((object) childContext.Widget);
    if (!(childContext.SplittedWidget is SplitStringWidget))
      return;
    if (num != -1)
    {
      (childContext.SplittedWidget as SplitStringWidget).m_prevWidgetIndex = num;
    }
    else
    {
      if (!(childContext.Widget is SplitStringWidget))
        return;
      (childContext.SplittedWidget as SplitStringWidget).m_prevWidgetIndex = (childContext.Widget as SplitStringWidget).m_prevWidgetIndex + 1;
    }
  }

  protected virtual void MarkAsBreaked(LayoutContext childContext)
  {
    this.AddChildLW(childContext);
    if (childContext is LCContainer && !(childContext is LCLineContainer) && this.m_currChildLW.IsNotFitted)
      return;
    LayoutedWidget childWidget1 = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1];
    if (childWidget1 != null)
    {
      LayoutedWidget childWidget2 = childWidget1.ChildWidgets[childWidget1.ChildWidgets.Count - 1];
      if (childWidget2 != null && childWidget2.Widget.LayoutInfo.IsPageBreakItem)
      {
        for (int index = 0; index < this.WidgetContainer.Count; ++index)
        {
          if (this.WidgetContainer[index] == childWidget2.Widget)
          {
            this.m_curWidgetIndex = index;
            break;
          }
        }
      }
    }
    this.NextChildWidget();
    if (this.CurrentChildWidget != null && (!(this.CurrentChildWidget is BookmarkEnd) || !this.StartsWithExt((this.CurrentChildWidget as BookmarkEnd).Name.ToLower(), "_toc") || this.CurrentChildWidget != this.WidgetContainer[this.WidgetContainer.Count - 1]))
    {
      this.SplitedUpWidget(this.CurrentChildWidget, false);
      this.m_ltState = LayoutState.Splitted;
      this.UpdateSplittedWidgetIndex(childContext);
    }
    else
      this.m_ltState = LayoutState.Breaked;
  }

  protected virtual void UpdateClientArea()
  {
    RectangleF bounds = this.m_currChildLW.Bounds;
    if ((this.m_currChildLW.Widget is WPicture || this.m_currChildLW.Widget is Shape || this.m_currChildLW.Widget is WChart || this.m_currChildLW.Widget is GroupShape) && (this.m_currChildLW.Widget as ParagraphItem).GetTextWrappingStyle() != TextWrappingStyle.Inline)
      return;
    if (this.m_currChildLW.Widget is WParagraph)
    {
      if (this.IsInFrame(this.m_currChildLW.Widget as WParagraph))
      {
        bool isAtleastHeight = ((int) (ushort) ((double) (this.m_currChildLW.Widget as WParagraph).ParagraphFormat.FrameHeight * 20.0) & 32768 /*0x8000*/) != 0;
        if (isAtleastHeight && this.m_currChildLW.Widget.LayoutInfo is ILayoutSpacingsInfo)
          bounds.Height += (this.m_currChildLW.Widget.LayoutInfo as ILayoutSpacingsInfo).Paddings.Bottom;
        this.UpdateFrameBounds(bounds, isAtleastHeight);
        return;
      }
      if (this.m_currChildLW.Widget.LayoutInfo is ILayoutSpacingsInfo)
        bounds.Height += (this.m_currChildLW.Widget.LayoutInfo as ILayoutSpacingsInfo).Paddings.Bottom;
      this.m_currChildLW.Bounds = bounds;
      if (((this.m_currChildLW.Widget as WParagraph).BreakCharacterFormat.Hidden || (this.m_currChildLW.Widget as WParagraph).BreakCharacterFormat.IsDeleteRevision || (this.m_lcOperator as Layouter).FieldEntity is WField) && this.m_currChildLW.TextTag == null)
      {
        WParagraph widget = this.m_currChildLW.Widget as WParagraph;
        if ((this.m_currChildLW.Widget as WParagraph).HasNonHiddenPara() && this.IsNextTextBodyItemIsParagraph(widget) || !(this.m_currChildLW.Widget as WParagraph).BreakCharacterFormat.Hidden && !(this.m_currChildLW.Widget as WParagraph).BreakCharacterFormat.IsDeleteRevision && widget.NextSibling is WTable)
        {
          float height = 0.0f;
          bool isNeedToUpdateXPosition = false;
          float rightPosition = this.GetRightPosition(bounds.Right, ref isNeedToUpdateXPosition);
          this.m_currChildLW.GetFootnoteHeight(ref height);
          if (isNeedToUpdateXPosition)
            this.m_layoutArea.CutFromLeft((double) rightPosition);
          if ((this.m_lcOperator as Layouter).FieldEntity is WField)
            this.m_layoutArea.CutFromTop((double) bounds.Y, height);
          if (!((this.m_lcOperator as Layouter).FieldEntity is WFieldMark))
            (this.m_lcOperator as Layouter).HiddenLineBottom = this.m_currChildLW.Bounds.Bottom;
          if ((this.m_lcOperator as Layouter).FieldEntity != null)
            (this.m_lcOperator as Layouter).FieldEntity = (IEntity) ((this.m_lcOperator as Layouter).FieldEntity as WField).FieldEnd;
          if (this.m_currChildLW.ChildWidgets.Count > 0 && this.m_currChildLW.ChildWidgets[0].ChildWidgets.Count > 0 && (double) this.m_currChildLW.ChildWidgets[0].Bounds.Width > 0.0)
            return;
        }
      }
      else if (((this.m_currChildLW.Widget as WParagraph).BreakCharacterFormat.Hidden || (this.m_currChildLW.Widget as WParagraph).BreakCharacterFormat.IsDeleteRevision || (this.m_lcOperator as Layouter).FieldEntity is WField) && this.m_currChildLW.TextTag == "Splitted")
      {
        WParagraph widget = this.m_currChildLW.Widget as WParagraph;
        if ((this.m_currChildLW.Widget as WParagraph).HasNonHiddenPara() && this.IsNextTextBodyItemIsParagraph(widget) || !(this.m_currChildLW.Widget as WParagraph).BreakCharacterFormat.Hidden && !(this.m_currChildLW.Widget as WParagraph).BreakCharacterFormat.IsDeleteRevision && widget.NextSibling is WTable)
        {
          this.m_layoutArea.CutFromLeft((double) this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].Bounds.Right, true);
          if (!((this.m_lcOperator as Layouter).FieldEntity is WFieldMark))
            (this.m_lcOperator as Layouter).HiddenLineBottom = this.m_currChildLW.Bounds.Bottom;
          if (this.m_currChildLW.ChildWidgets.Count > 0 && this.m_currChildLW.ChildWidgets[0].ChildWidgets.Count > 0 && (double) this.m_currChildLW.ChildWidgets[0].Bounds.Width > 0.0)
          {
            float height = 0.0f;
            this.m_currChildLW.GetFootnoteHeight(ref height);
            if ((double) (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems != -3.4028234663852886E+38 && (double) (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems != 3.4028234663852886E+38)
            {
              this.m_layoutArea.UpdateLeftPosition((this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems);
              (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems = float.MaxValue;
              return;
            }
            if ((double) (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems == 3.4028234663852886E+38)
            {
              RectangleF clientActiveArea = this.m_layoutArea.ClientActiveArea;
              clientActiveArea.Width += clientActiveArea.X - (this.m_lcOperator as Layouter).ClientLayoutArea.X;
              clientActiveArea.X = (this.m_lcOperator as Layouter).ClientLayoutArea.X;
              this.m_layoutArea.UpdateClientActiveArea(clientActiveArea);
              (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems = float.MinValue;
            }
            this.m_layoutArea.CutFromTop((double) bounds.Bottom - (double) this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].Bounds.Height, height);
            if ((this.m_lcOperator as Layouter).FieldEntity != null)
              (this.m_lcOperator as Layouter).FieldEntity = (IEntity) ((this.m_lcOperator as Layouter).FieldEntity as WField).FieldEnd;
            (this.m_lcOperator as Layouter).m_canSplitbyCharacter = true;
            (this.m_lcOperator as Layouter).m_canSplitByTab = false;
            (this.m_lcOperator as Layouter).IsFirstItemInLine = true;
            return;
          }
        }
      }
    }
    if (this.m_currChildLW.Widget is WParagraph && ((this.m_currChildLW.Widget as WParagraph).IsPreviousParagraphMarkIsHidden() || (this.m_currChildLW.Widget as WParagraph).IsPreviousParagraphMarkIsInDeletion() || (this.m_lcOperator as Layouter).FieldEntity is WFieldMark))
    {
      float pageMarginLeft = this.GetPageMarginLeft(this.m_currChildLW.Widget as WParagraph);
      if ((double) pageMarginLeft != (double) this.m_currChildLW.Bounds.X)
        this.m_layoutArea.UpdateLeftPosition(pageMarginLeft);
      if ((double) (this.m_lcOperator as Layouter).HiddenLineBottom != 0.0 && (double) (this.m_lcOperator as Layouter).HiddenLineBottom > (double) bounds.Bottom && !((this.m_lcOperator as Layouter).FieldEntity is WFieldMark))
        bounds.Height = (this.m_lcOperator as Layouter).HiddenLineBottom - bounds.Y;
      (this.m_lcOperator as Layouter).HiddenLineBottom = 0.0f;
      (this.m_lcOperator as Layouter).FieldEntity = (IEntity) null;
    }
    if (this.m_currChildLW.Widget is BlockContentControl && ((this.m_currChildLW.Widget as BlockContentControl).IsHiddenParagraphMarkIsInLastItemOfSDTContent() || (this.m_currChildLW.Widget as BlockContentControl).IsDeletionParagraphMarkIsInLastItemOfSDTContent()))
    {
      this.m_layoutArea.CutFromLeft((double) this.m_currChildLW.Bounds.Right);
    }
    else
    {
      if (this.m_currChildLW.Widget is WTextBox && (this.m_currChildLW.Widget as WTextBox).TextBoxFormat.TextWrappingStyle != TextWrappingStyle.Inline)
        return;
      if (this.m_currChildLW.Widget is WTable)
      {
        WTable widget = this.m_currChildLW.Widget as WTable;
        if (widget.TableFormat.WrapTextAround)
          return;
        if (this.IsInFrame(widget))
        {
          this.UpdateFrameBounds(bounds, false);
          return;
        }
      }
      if (this.m_currChildLW.Widget is BlockContentControl)
      {
        BlockContentControl widget = this.m_currChildLW.Widget as BlockContentControl;
        if (widget.ChildEntities != null && widget.ChildEntities.Count == 1 && widget.ChildEntities.FirstItem is WTable && (widget.ChildEntities.FirstItem as WTable).TableFormat.WrapTextAround)
          return;
      }
      if (this.m_currChildLW.Widget.LayoutInfo is ILayoutSpacingsInfo && (this.m_currChildLW.Widget is WParagraph ? (!(this.m_currChildLW.Widget as WParagraph).IsNeedToSkip ? 1 : 0) : 1) != 0)
        bounds = this.UpdateSpacingInfo(bounds);
      float rightEgeExtent = 0.0f;
      float topEdgeExtent = 0.0f;
      float bottomEdgeExtent = 0.0f;
      if (this.m_currChildLW.Widget is ParagraphItem)
        (this.m_currChildLW.Widget as ParagraphItem).GetEffectExtentValues(out float _, out rightEgeExtent, out topEdgeExtent, out bottomEdgeExtent);
      switch (this.LayoutInfo.ChildrenLayoutDirection)
      {
        case ChildrenLayoutDirection.Horizontal:
          float num1 = 0.0f;
          if (this.m_currChildLW.Widget is WTextRange)
            num1 = (this.m_currChildLW.Widget as WTextRange).CharacterFormat.Position;
          else if (this.m_currChildLW.Widget is WPicture || this.m_currChildLW.Widget is Shape || this.m_currChildLW.Widget is WChart)
            num1 = this.m_currChildLW.Widget is Shape ? (this.m_currChildLW.Widget as Shape).ParaItemCharFormat.Position : (this.m_currChildLW.Widget is WChart ? (this.m_currChildLW.Widget as WChart).ParaItemCharFormat.Position : (this.m_currChildLW.Widget is GroupShape ? (this.m_currChildLW.Widget as GroupShape).ParaItemCharFormat.Position : (this.m_currChildLW.Widget as WPicture).CharacterFormat.Position));
          if ((double) bounds.Y + (double) num1 != (double) this.m_layoutArea.ClientActiveArea.Y && (double) bounds.Y > (double) this.m_layoutArea.ClientActiveArea.Y && (double) topEdgeExtent + (double) bottomEdgeExtent == 0.0)
          {
            this.m_layoutArea.CutFromTop((double) bounds.Y);
            if (this.m_ltWidget.ChildWidgets.IndexOf(this.m_currChildLW) == 0)
              this.m_ltWidget.Bounds = this.m_ltWidget.Bounds with
              {
                Y = this.m_currChildLW.Bounds.Y
              };
          }
          if ((double) (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems != -3.4028234663852886E+38 && (double) (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems != 3.4028234663852886E+38 && this.IsTabStopBeyondRightMarginExists)
          {
            float num2 = 0.0f;
            WParagraph ownerParagraphValue = (this.m_currChildLW.Widget is SplitStringWidget ? (this.m_currChildLW.Widget as SplitStringWidget).RealStringWidget as ParagraphItem : this.m_currChildLW.Widget as ParagraphItem)?.GetOwnerParagraphValue();
            if (ownerParagraphValue != null)
            {
              ParagraphLayoutInfo layoutInfo = ((IWidget) ownerParagraphValue).LayoutInfo as ParagraphLayoutInfo;
              num2 = !layoutInfo.IsFirstLine ? layoutInfo.Margins.Left : layoutInfo.Margins.Left + layoutInfo.FirstLineIndent;
            }
            this.m_layoutArea.UpdateWidth((this.m_lcOperator as Layouter).PreviousTabWidth + num2);
            this.IsTabStopBeyondRightMarginExists = false;
          }
          this.m_layoutArea.CutFromLeft((double) bounds.Right + (double) rightEgeExtent);
          break;
        case ChildrenLayoutDirection.Vertical:
          float footnoteHeight = 0.0f;
          if (!(this.m_currChildLW.Widget is WParagraph widget1) || !widget1.IsExactlyRowHeight())
            footnoteHeight = this.GetFootnoteHeight();
          float num3 = 0.0f;
          if ((double) (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems != -3.4028234663852886E+38 && (double) (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems != 3.4028234663852886E+38)
          {
            this.m_layoutArea.UpdateLeftPosition((this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems);
            (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems = float.MaxValue;
            break;
          }
          if ((double) (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems == 3.4028234663852886E+38)
          {
            RectangleF clientActiveArea = this.m_layoutArea.ClientActiveArea;
            clientActiveArea.Width += clientActiveArea.X - (this.m_lcOperator as Layouter).ClientLayoutArea.X;
            clientActiveArea.X = (this.m_lcOperator as Layouter).ClientLayoutArea.X;
            this.m_layoutArea.UpdateClientActiveArea(clientActiveArea);
            (this.m_lcOperator as Layouter).RightPositionOfTabStopInterSectingFloattingItems = float.MinValue;
          }
          this.m_layoutArea.CutFromTop((double) bounds.Bottom + (double) num3, footnoteHeight);
          (this.m_lcOperator as Layouter).m_canSplitbyCharacter = true;
          (this.m_lcOperator as Layouter).m_canSplitByTab = false;
          (this.m_lcOperator as Layouter).IsFirstItemInLine = true;
          break;
      }
    }
  }

  private RectangleF UpdateSpacingInfo(RectangleF bounds)
  {
    ILayoutSpacingsInfo layoutInfo = this.m_currChildLW.Widget.LayoutInfo as ILayoutSpacingsInfo;
    bounds.X -= layoutInfo.Margins.Left;
    bounds.Y -= layoutInfo.Margins.Top;
    bounds.Width += layoutInfo.Margins.Left + layoutInfo.Margins.Right;
    bounds.Height += layoutInfo.Margins.Top + layoutInfo.Margins.Bottom;
    if (layoutInfo is ParagraphLayoutInfo && !(layoutInfo as ParagraphLayoutInfo).IsFirstLine)
    {
      bounds.Y += layoutInfo.Margins.Top;
      bounds.Height -= layoutInfo.Margins.Top;
    }
    if (this.m_currChildLW.Widget is WParagraph || this.m_currChildLW.Widget is SplitWidgetContainer && (this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
    {
      WParagraph wparagraph = !(this.m_currChildLW.Widget is WParagraph widget) ? (this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph : widget;
      LayoutedWidgetList layoutedWidgetList = (LayoutedWidgetList) null;
      if (this.m_currChildLW.ChildWidgets.Count > 0)
        layoutedWidgetList = this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].ChildWidgets;
      float rowHeight = 0.0f;
      if (wparagraph != null && layoutedWidgetList != null && layoutedWidgetList.Count > 0 && (wparagraph.IsLastLine(layoutedWidgetList[layoutedWidgetList.Count - 1]) || this.m_currChildLW.TextTag == "Splitted") && wparagraph.IsInCell && !wparagraph.IsExactlyRowHeight(wparagraph.GetOwnerEntity() as WTableCell, ref rowHeight))
        bounds.Height -= layoutInfo.Margins.Bottom;
    }
    return bounds;
  }

  private bool IsNextTextBodyItemIsParagraph(WParagraph paragraph)
  {
    return paragraph != null && (paragraph.NextSibling is WParagraph || paragraph.OwnerTextBody != null && paragraph.OwnerTextBody.Owner != null && paragraph.OwnerTextBody.Owner is BlockContentControl && (paragraph.OwnerTextBody.Owner as BlockContentControl).NextSibling is WParagraph);
  }

  private bool IsInFrame(WTable table)
  {
    bool flag = true;
    if (table.IsInCell)
      flag = false;
    return table.IsFrame && flag;
  }

  private void UpdateFrameBounds(RectangleF bounds, bool isAtleastHeight)
  {
    this.m_currChildLW.Bounds = bounds;
    if (this.m_currChildLW.Widget.LayoutInfo is ILayoutSpacingsInfo)
      bounds = this.UpdateSpacingInfo(bounds);
    float num1 = bounds.Bottom;
    if ((double) num1 < (double) (this.m_lcOperator as Layouter).FrameLayoutArea.Top)
      num1 = (this.m_lcOperator as Layouter).FrameLayoutArea.Top;
    else if ((double) num1 > (double) (this.m_lcOperator as Layouter).FrameLayoutArea.Bottom)
      num1 = (this.m_lcOperator as Layouter).FrameLayoutArea.Bottom;
    RectangleF frameLayoutArea = (this.m_lcOperator as Layouter).FrameLayoutArea;
    if (isAtleastHeight)
      (this.m_lcOperator as Layouter).FrameHeight -= bounds.Height;
    frameLayoutArea.Height = frameLayoutArea.Bottom - num1;
    frameLayoutArea.Y = num1;
    float num2 = frameLayoutArea.Y - this.m_currChildLW.Bounds.Bottom;
    bounds = this.m_currChildLW.Bounds;
    bounds.Height += num2;
    this.m_currChildLW.Bounds = bounds;
    (this.m_lcOperator as Layouter).FrameLayoutArea = frameLayoutArea;
  }

  protected virtual void ChangeChildsAlignment()
  {
  }

  protected bool NextChildWidget()
  {
    if (this.m_curWidgetIndex > -1 && this.m_curWidgetIndex < this.WidgetContainer.Count - 1)
    {
      ++this.m_curWidgetIndex;
      return true;
    }
    this.m_curWidgetIndex = -1;
    return false;
  }

  internal void SplitedUpWidget(IWidget splitWidget, bool isEndNoteSplitWidgets)
  {
    int firstIndex = this.m_curWidgetIndex;
    if (isEndNoteSplitWidgets && this.m_curWidgetIndex < 0)
      firstIndex = 0;
    this.m_sptWidget = (IWidget) new SplitWidgetContainer(this.WidgetContainer, splitWidget, firstIndex);
  }

  protected void SaveChildContextState(LayoutContext childContext)
  {
    switch (childContext.State)
    {
      case LayoutState.Unknown:
        this.m_ltState = LayoutState.Unknown;
        break;
      case LayoutState.NotFitted:
        this.m_ltWidget.IsNotFitted = false;
        this.MarkAsNotFitted(childContext, false);
        this.m_ltWidget.TextTag = "Splitted";
        break;
      case LayoutState.Splitted:
        this.m_ltWidget.TextTag = "Splitted";
        this.MarkAsSplitted(childContext);
        break;
      case LayoutState.WrapText:
        this.MarkAsWrapText(childContext);
        break;
      case LayoutState.Fitted:
        this.MarkAsFitted(childContext);
        break;
      case LayoutState.Breaked:
        this.MarkAsBreaked(childContext);
        break;
      case LayoutState.DynamicRelayout:
        this.AddChildLW(childContext);
        break;
    }
  }

  private float GetTableHeight(WTable table)
  {
    float tableHeight = 0.0f;
    foreach (WTableRow row in (Syncfusion.DocIO.DLS.CollectionImpl) table.Rows)
      tableHeight += row.Height;
    return tableHeight;
  }

  protected virtual void DoLayoutChild(LayoutContext childContext)
  {
    if (this.IsTabStopBeyondRightMarginExists && !this.IsAreaUpdated)
    {
      this.UpdateAreaWidth(0.0f);
      this.IsAreaUpdated = true;
    }
    RectangleF rectangleF = this.m_layoutArea.ClientActiveArea;
    bool flag = childContext.Widget is WTable;
    if (childContext.Widget is WParagraph && this.IsInFrame(childContext.Widget as WParagraph) || flag && !(childContext.Widget as WTable).TableFormat.WrapTextAround && this.IsInFrame(childContext.Widget as WTable))
    {
      WParagraph wparagraph = childContext.Widget as WParagraph;
      float num1 = 0.0f;
      if (wparagraph == null && flag)
      {
        wparagraph = (childContext.Widget as WTable).Rows[0].Cells[0].Paragraphs[0];
        num1 = this.GetTableHeight(childContext.Widget as WTable);
      }
      if (wparagraph != null && !this.IsForceFitLayout && !DocumentLayouter.IsLayoutingHeaderFooter)
      {
        WParagraphFormat paragraphFormat = wparagraph.ParagraphFormat;
        float num2 = (float) (((int) (ushort) ((double) paragraphFormat.FrameHeight * 20.0) & (int) short.MaxValue) / 20);
        int num3 = ((int) (ushort) ((double) paragraphFormat.FrameWidth * 20.0) & (int) short.MaxValue) / 20;
        if (paragraphFormat.FrameVerticalAnchor == (byte) 2 && (wparagraph.GetOwnerSection().Columns.Count == 1 && (double) num2 > (double) rectangleF.Height || flag && (double) num1 > (double) rectangleF.Height))
        {
          childContext.m_ltState = LayoutState.NotFitted;
          this.m_currChildLW = (LayoutedWidget) null;
          return;
        }
      }
      rectangleF = (this.m_lcOperator as Layouter).FrameLayoutArea;
      childContext.ClientLayoutAreaRight = (this.m_lcOperator as Layouter).FrameLayoutArea.Width;
    }
    if (childContext is LeafLayoutContext)
    {
      ParagraphItem paragraphItem = childContext.Widget is SplitStringWidget ? (childContext.Widget as SplitStringWidget).RealStringWidget as ParagraphItem : childContext.Widget as ParagraphItem;
      WParagraph ownerParagraphValue = paragraphItem?.GetOwnerParagraphValue();
      bool hasTextInRange = this.HasTextInRange(childContext);
      if ((!(childContext.Widget is Break) || (childContext.Widget as Break).BreakType != BreakType.LineBreak || !((childContext.Widget as Break).Owner.Owner is WTableCell)) && !(paragraphItem is BookmarkStart) && !(paragraphItem is BookmarkEnd) && (this.IsSplitLine(rectangleF, ownerParagraphValue, hasTextInRange) || this.m_ltWidget.ChildWidgets.Count > 0 && this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Widget is SplitStringWidget && (this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Widget as SplitStringWidget).IsTrailSpacesWrapped && (!(childContext.Widget is Break) || (childContext.Widget as Break).BreakType != BreakType.LineBreak) && hasTextInRange))
      {
        childContext.m_ltState = LayoutState.NotFitted;
        this.m_currChildLW = (LayoutedWidget) null;
      }
      else
        this.m_currChildLW = childContext.Layout(rectangleF);
    }
    else
      this.m_currChildLW = childContext.Layout(rectangleF);
    this.UpdateWrappingDifferenceValue(this.m_currChildLW);
  }

  private bool IsSplitLine(RectangleF clientArea, WParagraph ownerpara, bool hasTextInRange)
  {
    if ((double) clientArea.X <= (double) this.m_layoutArea.ClientArea.Right || ownerpara == null || !hasTextInRange && (this.m_ltWidget.ChildWidgets.Count <= 0 || this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Widget is WTextRange) || ownerpara.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
      return false;
    return !ownerpara.IsInCell || !(ownerpara.GetOwnerEntity() as WTableCell).OwnerRow.OwnerTable.TableFormat.IsAutoResized;
  }

  private bool HasTextInRange(LayoutContext childContext)
  {
    if (childContext.Widget is WTextRange && !(childContext.Widget is WField))
    {
      if ((childContext.Widget as WTextRange).Text.TrimEnd(ControlChar.SpaceChar) == string.Empty)
        return childContext.Widget.LayoutInfo is TabsLayoutInfo;
    }
    return true;
  }

  private bool IsNeedToUpdateFloatingEntityBounds(Entity entity)
  {
    if (entity == null)
      return false;
    TextWrappingStyle textWrappingStyle = TextWrappingStyle.Inline;
    switch (entity.EntityType)
    {
      case EntityType.Picture:
        textWrappingStyle = (entity as WPicture).TextWrappingStyle;
        break;
      case EntityType.Shape:
      case EntityType.AutoShape:
        if (entity is Shape)
        {
          textWrappingStyle = (entity as Shape).WrapFormat.TextWrappingStyle;
          break;
        }
        break;
      case EntityType.TextBox:
        textWrappingStyle = (entity as WTextBox).TextBoxFormat.TextWrappingStyle;
        break;
      case EntityType.Chart:
        textWrappingStyle = (entity as WChart).WrapFormat.TextWrappingStyle;
        break;
      case EntityType.GroupShape:
        if (entity is GroupShape)
        {
          textWrappingStyle = (entity as GroupShape).WrapFormat.TextWrappingStyle;
          break;
        }
        break;
    }
    return textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through;
  }

  private FloatingItem GetFloatingItemFromCollection(Entity entity)
  {
    for (int index = 0; index < (this.m_lcOperator as Layouter).FloatingItems.Count; ++index)
    {
      if ((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity == entity)
        return (this.m_lcOperator as Layouter).FloatingItems[index];
    }
    return (FloatingItem) null;
  }

  protected void AddChildLW(LayoutContext childContext)
  {
    WParagraph wparagraph1 = (WParagraph) null;
    if (DocumentLayouter.IsLayoutingHeaderFooter)
      wparagraph1 = this.m_currChildLW.Widget is WParagraph ? this.m_currChildLW.Widget as WParagraph : (WParagraph) null;
    if (!this.m_currChildLW.Widget.LayoutInfo.IsSkip && (wparagraph1 == null || !wparagraph1.IsNeedToSkip) || this.m_currChildLW.ChildWidgets.Count != 0)
    {
      if (this.IsNeedToUpdateFloatingEntityBounds(this.m_currChildLW.Widget as Entity))
      {
        FloatingItem itemFromCollection = this.GetFloatingItemFromCollection(this.m_currChildLW.Widget as Entity);
        if (itemFromCollection != null && !itemFromCollection.IsDoesNotDenotesRectangle)
        {
          SizeF sizeF = (this.m_currChildLW.Widget as ILeafWidget).Measure(this.DrawingContext);
          this.m_currChildLW.Bounds = new RectangleF(this.m_currChildLW.Bounds.X, this.m_currChildLW.Bounds.Y, sizeF.Width, sizeF.Height);
          this.m_currChildLW.Bounds = this.ResetAdjustedboundsBasedOnWrapPolygon(itemFromCollection, this.m_currChildLW.Bounds);
        }
      }
      this.m_ltWidget.ChildWidgets.Add(this.m_currChildLW);
      if (childContext is LeafLayoutContext && ((double) this.m_currChildLW.Bounds.Width > 0.0 || this.m_currChildLW.Widget is WTextRange && (this.m_currChildLW.Widget as WTextRange).m_layoutInfo is TabsLayoutInfo) && (this.m_lcOperator as Layouter).IsFirstItemInLine)
      {
        if (!(this.m_currChildLW.Widget is Entity widget))
          (this.m_lcOperator as Layouter).IsFirstItemInLine = false;
        else if (!widget.IsFloatingItem(false))
          (this.m_lcOperator as Layouter).IsFirstItemInLine = false;
      }
    }
    if (this.m_ltWidget.Widget is WSection || this.m_ltWidget.Widget is SplitWidgetContainer && (this.m_ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WSection)
      (this.m_lcOperator as Layouter).CountForConsecutiveLimit = 0;
    if (this.m_currChildLW.IsBehindWidget())
      this.AddBehindWidgets(this.m_currChildLW);
    if (childContext.State == LayoutState.DynamicRelayout)
    {
      IWidget splitWidget = childContext.SplittedWidget;
      if (childContext is LCLineContainer && splitWidget is SplitWidgetContainer && (splitWidget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
      {
        if ((this.m_lcOperator as Layouter).NotFittedFloatingItems.Count > 0)
        {
          if (childContext.Widget is SplitWidgetContainer)
            this.SplittedWidget = (childContext.Widget as SplitWidgetContainer).m_currentChild;
          else
            splitWidget = (childContext.Widget as WParagraph).ChildEntities[0] as IWidget;
        }
        else
          splitWidget = (splitWidget as SplitWidgetContainer).m_currentChild;
      }
      this.SplitedUpWidget(splitWidget, false);
      this.m_ltState = LayoutState.DynamicRelayout;
      this.m_currChildLW.Owner = this.m_ltWidget;
    }
    else
    {
      if (childContext.Widget is ParagraphItem || childContext.Widget is SplitStringWidget)
      {
        ParagraphItem paragraphItem = childContext.Widget is ParagraphItem ? childContext.Widget as ParagraphItem : (childContext.Widget as SplitStringWidget).RealStringWidget as ParagraphItem;
        if ((paragraphItem.IsInsertRevision || paragraphItem.IsChangedCFormat) && this.m_ltWidget != null)
          this.m_ltWidget.IsTrackChanges = true;
      }
      if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && childContext.Widget is WPicture && (childContext.Widget as WPicture).IsWrappingBoundsAdded && (childContext.Widget as WPicture).WrapCollectionIndex >= (short) 0 && (int) (childContext.Widget as WPicture).WrapCollectionIndex < (this.m_lcOperator as Layouter).FloatingItems.Count)
        (this.m_lcOperator as Layouter).FloatingItems[(int) (childContext.Widget as WPicture).WrapCollectionIndex].IsFloatingItemFit = true;
      if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && childContext.Widget is Shape && (childContext.Widget as Shape).WrapFormat.IsWrappingBoundsAdded && (childContext.Widget as Shape).WrapFormat.WrapCollectionIndex >= 0 && (childContext.Widget as Shape).WrapFormat.WrapCollectionIndex < (this.m_lcOperator as Layouter).FloatingItems.Count)
        (this.m_lcOperator as Layouter).FloatingItems[(childContext.Widget as Shape).WrapFormat.WrapCollectionIndex].IsFloatingItemFit = true;
      if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && childContext.Widget is WTextBox && (childContext.Widget as WTextBox).TextBoxFormat.IsWrappingBoundsAdded && (childContext.Widget as WTextBox).TextBoxFormat.WrapCollectionIndex >= (short) 0 && (int) (childContext.Widget as WTextBox).TextBoxFormat.WrapCollectionIndex < (this.m_lcOperator as Layouter).FloatingItems.Count)
        (this.m_lcOperator as Layouter).FloatingItems[(int) (childContext.Widget as WTextBox).TextBoxFormat.WrapCollectionIndex].IsFloatingItemFit = true;
      if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && childContext.Widget is WChart && (childContext.Widget as WChart).WrapFormat.IsWrappingBoundsAdded && (childContext.Widget as WChart).WrapFormat.WrapCollectionIndex >= 0 && (childContext.Widget as WChart).WrapFormat.WrapCollectionIndex < (this.m_lcOperator as Layouter).FloatingItems.Count)
        (this.m_lcOperator as Layouter).FloatingItems[(childContext.Widget as WChart).WrapFormat.WrapCollectionIndex].IsFloatingItemFit = true;
      if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && childContext.Widget is GroupShape && (childContext.Widget as GroupShape).WrapFormat.IsWrappingBoundsAdded && (childContext.Widget as GroupShape).WrapFormat.WrapCollectionIndex >= 0 && (childContext.Widget as GroupShape).WrapFormat.WrapCollectionIndex < (this.m_lcOperator as Layouter).FloatingItems.Count)
        (this.m_lcOperator as Layouter).FloatingItems[(childContext.Widget as GroupShape).WrapFormat.WrapCollectionIndex].IsFloatingItemFit = true;
      if (this.m_currChildLW.Widget is ParagraphItem || this.m_currChildLW.Widget is SplitStringWidget)
      {
        WParagraph wparagraph2 = !(this.m_currChildLW.Widget is ParagraphItem) ? ((this.m_currChildLW.Widget as SplitStringWidget).RealStringWidget as ParagraphItem).OwnerParagraph : (this.m_currChildLW.Widget as ParagraphItem).OwnerParagraph;
        if (wparagraph2 != null && (double) (((IWidget) wparagraph2).LayoutInfo as ParagraphLayoutInfo).XPosition > (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Left && (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Left == (double) this.m_currChildLW.Bounds.X && !(childContext.Widget is BookmarkStart) && !(childContext.Widget is BookmarkEnd))
        {
          wparagraph2.IsXpositionUpated = true;
          (((IWidget) wparagraph2).LayoutInfo as ParagraphLayoutInfo).IsXPositionReUpdate = true;
        }
      }
      if (this.m_ltWidget.Widget is WParagraph && (this.m_ltWidget.Widget as WParagraph).Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && this.m_ltWidget.ChildWidgets.Count > 2 && this.m_currChildLW.Widget is WTextRange && !(this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 2].Widget is WTextRange))
      {
        for (int index1 = 0; index1 < this.m_ltWidget.ChildWidgets.Count - 1; ++index1)
        {
          if (this.CheckWidgetWrappingType(this.m_ltWidget.ChildWidgets[index1].Widget) && (double) this.m_currChildLW.Bounds.X >= (double) this.m_ltWidget.ChildWidgets[index1].Bounds.Right + (this.m_ltWidget.ChildWidgets[index1].Widget is WPicture ? (double) (this.m_ltWidget.ChildWidgets[index1].Widget as WPicture).DistanceFromRight : (this.m_ltWidget.ChildWidgets[index1].Widget is WTextBox ? (double) (this.m_ltWidget.ChildWidgets[index1].Widget as WTextBox).TextBoxFormat.WrapDistanceRight : (this.m_ltWidget.ChildWidgets[index1].Widget is Shape ? (double) (this.m_ltWidget.ChildWidgets[index1].Widget as Shape).WrapFormat.DistanceRight : (this.m_ltWidget.ChildWidgets[index1].Widget is GroupShape ? (double) (this.m_ltWidget.ChildWidgets[index1].Widget as GroupShape).WrapFormat.DistanceRight : 0.0)))))
          {
            for (int index2 = 0; index2 < this.m_ltWidget.ChildWidgets.Count - 1; ++index2)
            {
              if (this.CheckWidgetWrappingTypeAndHorizontalOrigin(this.m_ltWidget.ChildWidgets[index2].Widget))
              {
                float right = this.m_ltWidget.ChildWidgets[index1].Bounds.Right;
                float num = 0.0f;
                ShapeHorizontalAlignment horizontalAlignment = ShapeHorizontalAlignment.None;
                switch ((this.m_ltWidget.ChildWidgets[index1].Widget as Entity).EntityType)
                {
                  case EntityType.Picture:
                    right += (this.m_ltWidget.ChildWidgets[index1].Widget as WPicture).DistanceFromRight;
                    break;
                  case EntityType.Shape:
                  case EntityType.AutoShape:
                    right += (this.m_ltWidget.ChildWidgets[index1].Widget as Shape).WrapFormat.DistanceRight;
                    break;
                  case EntityType.TextBox:
                    right += (this.m_ltWidget.ChildWidgets[index1].Widget as WTextBox).TextBoxFormat.WrapDistanceRight;
                    break;
                  case EntityType.Chart:
                    right += (this.m_ltWidget.ChildWidgets[index1].Widget as WChart).WrapFormat.DistanceRight;
                    break;
                  case EntityType.GroupShape:
                    right += (this.m_ltWidget.ChildWidgets[index1].Widget as GroupShape).WrapFormat.DistanceRight;
                    break;
                }
                switch ((this.m_ltWidget.ChildWidgets[index2].Widget as Entity).EntityType)
                {
                  case EntityType.Picture:
                    num = (this.m_ltWidget.ChildWidgets[index2].Widget as WPicture).HorizontalPosition;
                    horizontalAlignment = (this.m_ltWidget.ChildWidgets[index2].Widget as WPicture).HorizontalAlignment;
                    break;
                  case EntityType.Shape:
                  case EntityType.AutoShape:
                    num = (this.m_ltWidget.ChildWidgets[index2].Widget as Shape).HorizontalPosition;
                    horizontalAlignment = (this.m_ltWidget.ChildWidgets[index2].Widget as Shape).HorizontalAlignment;
                    break;
                  case EntityType.TextBox:
                    num = (this.m_ltWidget.ChildWidgets[index2].Widget as WTextBox).TextBoxFormat.HorizontalPosition;
                    horizontalAlignment = (this.m_ltWidget.ChildWidgets[index2].Widget as WTextBox).TextBoxFormat.HorizontalAlignment;
                    break;
                  case EntityType.Chart:
                    num = (this.m_ltWidget.ChildWidgets[index2].Widget as WChart).HorizontalPosition;
                    horizontalAlignment = (this.m_ltWidget.ChildWidgets[index2].Widget as WChart).HorizontalAlignment;
                    break;
                  case EntityType.GroupShape:
                    num = (this.m_ltWidget.ChildWidgets[index2].Widget as GroupShape).HorizontalPosition;
                    horizontalAlignment = (this.m_ltWidget.ChildWidgets[index2].Widget as GroupShape).HorizontalAlignment;
                    break;
                }
                switch (horizontalAlignment)
                {
                  case ShapeHorizontalAlignment.Left:
                    num = (this.m_lcOperator as Layouter).ClientLayoutArea.Left;
                    break;
                  case ShapeHorizontalAlignment.Center:
                    num = (this.m_lcOperator as Layouter).ClientLayoutArea.Left + (float) (((double) (this.m_lcOperator as Layouter).ClientLayoutArea.Width - (double) this.m_ltWidget.ChildWidgets[index2].Bounds.Width) / 2.0);
                    break;
                  case ShapeHorizontalAlignment.Right:
                    num = (this.m_lcOperator as Layouter).ClientLayoutArea.Left + (this.m_lcOperator as Layouter).ClientLayoutArea.Width - this.m_ltWidget.ChildWidgets[index2].Bounds.Width;
                    break;
                }
                this.m_ltWidget.ChildWidgets[index2].Bounds = new RectangleF(right + num, this.m_ltWidget.ChildWidgets[index2].Bounds.Y, this.m_ltWidget.ChildWidgets[index2].Bounds.Width, this.m_ltWidget.ChildWidgets[index2].Bounds.Height);
              }
            }
            break;
          }
        }
      }
      if (childContext is LCLineContainer)
        this.UpdateParagraphYPosition(this.m_ltWidget, childContext);
      this.m_currChildLW.Owner = this.m_ltWidget;
      this.UpdateClientArea();
      if (childContext is LCLineContainer && (this.m_lcOperator as Layouter).UnknownField != null && (this.m_lcOperator as Layouter).UnknownField.OwnerParagraph.m_layoutInfo is ParagraphLayoutInfo && this.m_currChildLW.Widget is WParagraph && (this.m_currChildLW.Widget as WParagraph).Items.Contains((IEntity) (this.m_lcOperator as Layouter).UnknownField.FieldEnd))
        (this.m_lcOperator as Layouter).UnknownField = (WField) null;
      if (childContext is LCContainer && !(childContext is LCLineContainer) && this.m_currChildLW.IsNotFitted && this.m_ltWidget.ChildWidgets.Contains(this.m_currChildLW))
      {
        if (childContext.SplittedWidget is SplitWidgetContainer && this.m_currChildLW.Widget is SplitWidgetContainer && (childContext.SplittedWidget as SplitWidgetContainer).m_currentChild is SplitStringWidget && (this.m_currChildLW.Widget as SplitWidgetContainer).m_currentChild is SplitStringWidget && (childContext.SplittedWidget as SplitWidgetContainer).m_currentChild != (this.m_currChildLW.Widget as SplitWidgetContainer).m_currentChild)
          childContext.SplittedWidget = this.m_currChildLW.Widget;
        this.m_ltWidget.ChildWidgets.Remove(this.m_currChildLW);
        childContext.m_ltState = LayoutState.NotFitted;
      }
      else
      {
        this.UpdateLWBounds(childContext);
        IWordDocument wordDocument = this.m_ltWidget.Widget is WordDocument ? (IWordDocument) (this.m_ltWidget.Widget as WordDocument) : (this.m_ltWidget.Widget is SplitWidgetContainer ? (IWordDocument) ((this.m_ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WordDocument) : (IWordDocument) null);
        IWSection section = this.m_currChildLW.Widget is WSection ? (IWSection) (this.m_currChildLW.Widget as WSection) : (this.m_currChildLW.Widget is SplitWidgetContainer ? (IWSection) ((this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer as WSection) : (IWSection) null);
        if (wordDocument != null && section != null && childContext.State != LayoutState.Splitted)
        {
          if ((this.m_lcOperator as Layouter).EndnotesInstances.Count > 0)
          {
            if (section.Document.EndnotePosition == EndnotePosition.DisplayEndOfDocument && section.Document.Sections.IndexOf(section) == section.Document.Sections.Count - 1)
            {
              if ((this.m_lcOperator as Layouter).EndnotesInstances.Count > 0)
              {
                for (int index = 0; index < (this.m_lcOperator as Layouter).EndnotesInstances.Count; ++index)
                {
                  if (!((this.m_lcOperator as Layouter).EndnotesInstances[index] as WFootnote).IsLayouted)
                    this.LayoutEndnote((this.m_lcOperator as Layouter).EndnotesInstances[index] as WFootnote, this.m_currChildLW.Owner);
                  ((this.m_lcOperator as Layouter).EndnotesInstances[index] as WFootnote).IsLayouted = true;
                }
                (this.m_lcOperator as Layouter).EndnotesInstances.Clear();
              }
              if ((this.m_lcOperator as Layouter).EndnoteWidgets.Count == 0)
              {
                this.MarkAsNotFitted(childContext, true);
                (this.m_lcOperator as Layouter).EndnoteWidgets.Clear();
                (this.m_lcOperator as Layouter).EndNoteSectionIndex.Clear();
                this.SplitEndNoteWidgets();
                return;
              }
            }
            else if (section.Document.EndnotePosition == EndnotePosition.DisplayEndOfSection)
            {
              if ((this.m_lcOperator as Layouter).EndnotesInstances.Count > 0)
              {
                for (int index = 0; index < (this.m_lcOperator as Layouter).EndnotesInstances.Count; ++index)
                {
                  if (!((this.m_lcOperator as Layouter).EndnotesInstances[index] as WFootnote).IsLayouted)
                    this.LayoutEndnote((this.m_lcOperator as Layouter).EndnotesInstances[index] as WFootnote, this.m_currChildLW.Owner);
                  ((this.m_lcOperator as Layouter).EndnotesInstances[index] as WFootnote).IsLayouted = true;
                }
                (this.m_lcOperator as Layouter).EndnotesInstances.Clear();
              }
              if ((this.m_lcOperator as Layouter).EndnoteWidgets.Count == 0)
              {
                this.MarkAsNotFitted(childContext, true);
                (this.m_lcOperator as Layouter).EndnoteSplittedWidgets.Clear();
                return;
              }
            }
            while ((this.m_lcOperator as Layouter).EndnoteWidgets.Count > (this.m_lcOperator as Layouter).EndNoteSectionIndex.Count)
              (this.m_lcOperator as Layouter).EndNoteSectionIndex.Add(section.Document.Sections.IndexOf(section));
          }
          this.SplitEndNoteWidgets();
        }
        if (!(childContext is LCContainer) || childContext is LCLineContainer)
          return;
        WParagraph wparagraph3 = this.m_ltWidget.Widget is WParagraph ? this.m_ltWidget.Widget as WParagraph : (this.m_ltWidget.Widget is SplitWidgetContainer ? (this.m_ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph : (WParagraph) null);
        bool flag = wparagraph3 != null && wparagraph3.GetOwnerSection() != null && wparagraph3.GetOwnerSection().PageSetup.FootnotePosition == FootnotePosition.PrintAtBottomOfPage;
        if (flag && this.m_ltWidget.ChildWidgets.Count == 1 && this.m_currChildLW.ChildWidgets.Count > 0 && wparagraph3 != null && wparagraph3.ParagraphFormat.WidowControl && (wparagraph3.IsLastLine(this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1]) || this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].Widget is Break && (this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].Widget as Break).BreakType == BreakType.PageBreak))
          this.LayoutFootnoteOfLayoutedLines(this.m_ltWidget);
        if (flag && this.m_ltWidget.ChildWidgets.Count == 2 && wparagraph3 != null && wparagraph3.ParagraphFormat.WidowControl)
        {
          this.LayoutFootnoteOfLayoutedLines(this.m_ltWidget);
          (this.m_lcOperator as Layouter).IsTwoLinesLayouted = true;
        }
        if (this.m_ltState != LayoutState.NotFitted)
          return;
        (this.m_lcOperator as Layouter).IsTwoLinesLayouted = false;
        (this.m_lcOperator as Layouter).IsFootnoteHeightAdjusted = false;
        this.m_currChildLW.IsNotFitted = true;
      }
    }
  }

  private void LayoutFootnoteOfLayoutedLines(LayoutedWidget layoutedWidget)
  {
    IWSection section = (IWSection) null;
    bool flag = false;
    float footnoteHeight = 0.0f;
    for (int index1 = 0; index1 < layoutedWidget.ChildWidgets.Count; ++index1)
    {
      for (int index2 = 0; index2 < layoutedWidget.ChildWidgets[index1].ChildWidgets.Count; ++index2)
      {
        if (layoutedWidget.ChildWidgets[index1].ChildWidgets[index2].Widget is WFootnote widget && !widget.IsLayouted && widget.FootnoteType != FootnoteType.Endnote)
        {
          flag = true;
          bool twoLinesLayouted = (this.m_lcOperator as Layouter).IsTwoLinesLayouted;
          LayoutContext childContext = LayoutContext.Create(layoutedWidget.ChildWidgets[index1].ChildWidgets[index2].Widget, this.m_lcOperator, this.IsForceFitLayout);
          section = (IWSection) (this.GetBaseEntity(childContext.Widget as Entity) as WSection);
          this.LayoutFootnote(widget, layoutedWidget.ChildWidgets[index1], true);
          (this.m_lcOperator as Layouter).IsTwoLinesLayouted = twoLinesLayouted;
          if ((this.m_lcOperator as Layouter).FootnoteWidgets.Count == 0 && layoutedWidget.Widget is WParagraph)
          {
            string footnoteId = (childContext.Widget.LayoutInfo as FootnoteLayoutInfo).FootnoteID;
            this.MarkAsNotFitted(childContext, true);
            (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Clear();
            childContext.Widget.InitLayoutInfo();
            (childContext.Widget.LayoutInfo as FootnoteLayoutInfo).FootnoteID = (childContext.Widget.LayoutInfo as FootnoteLayoutInfo).FootnoteID.Equals(footnoteId) ? (childContext.Widget.LayoutInfo as FootnoteLayoutInfo).FootnoteID : footnoteId;
            this.m_sptWidget = (IWidget) new SplitWidgetContainer((IWidgetContainer) (layoutedWidget.Widget as WParagraph), (IWidget) (layoutedWidget.Widget as WParagraph).Items[0], 0);
            this.m_ltState = LayoutState.NotFitted;
            return;
          }
          (this.m_lcOperator as Layouter).IsFootnoteHeightAdjusted = layoutedWidget.ChildWidgets.Count == 2 && (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Count > 0;
          while ((this.m_lcOperator as Layouter).FootnoteWidgets.Count > (this.m_lcOperator as Layouter).FootNoteSectionIndex.Count)
            (this.m_lcOperator as Layouter).FootNoteSectionIndex.Add(section.Document.Sections.IndexOf(section));
          footnoteHeight += (layoutedWidget.ChildWidgets[index1].ChildWidgets[index2].Widget.LayoutInfo as FootnoteLayoutInfo).FootnoteHeight;
        }
      }
    }
    if ((double) footnoteHeight > 0.0 && (!(layoutedWidget.Widget is WParagraph) || !(layoutedWidget.Widget as WParagraph).IsExactlyRowHeight()))
      this.m_layoutArea.CutFromTop((double) layoutedWidget.Bounds.Bottom, footnoteHeight);
    if (!flag || section == null || !(layoutedWidget.Widget is WParagraph) || section.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 || !this.IsLastFootnoteNotInSamePage(layoutedWidget))
      return;
    this.UpdateFootnoteWidgets(layoutedWidget);
    (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Clear();
    this.m_sptWidget = (IWidget) new SplitWidgetContainer((IWidgetContainer) (layoutedWidget.Widget as WParagraph), (IWidget) (layoutedWidget.Widget as WParagraph).Items[0], 0);
    this.m_ltState = LayoutState.NotFitted;
  }

  private bool IsLastFootnoteNotInSamePage(LayoutedWidget layoutedWidget)
  {
    for (int index1 = layoutedWidget.ChildWidgets.Count - 1; index1 >= 0; --index1)
    {
      for (int index2 = layoutedWidget.ChildWidgets[index1].ChildWidgets.Count - 1; index2 >= 0; --index2)
      {
        if (layoutedWidget.ChildWidgets[index1].ChildWidgets[index2].Widget is WFootnote widget1 && widget1.FootnoteType != FootnoteType.Endnote)
        {
          WFootnote widget = layoutedWidget.ChildWidgets[index1].ChildWidgets[index2].Widget as WFootnote;
          bool flag = false;
          for (int index3 = (this.m_lcOperator as Layouter).FootnoteWidgets.Count - 1; index3 >= 0; --index3)
          {
            if (((this.m_lcOperator as Layouter).FootnoteWidgets[index3].Widget is WTextBody ? (Entity) ((this.m_lcOperator as Layouter).FootnoteWidgets[index3].Widget as WTextBody) : (Entity) (((this.m_lcOperator as Layouter).FootnoteWidgets[index3].Widget as SplitWidgetContainer).RealWidgetContainer as WTextBody)).Owner as WFootnote == widget)
            {
              flag = true;
              break;
            }
          }
          return !flag;
        }
      }
    }
    return false;
  }

  private void UpdateParagraphYPosition(LayoutedWidget widget, LayoutContext ltwidget)
  {
    if ((!(ltwidget.Widget is WParagraph) || !(ltwidget.Widget as WParagraph).ParagraphFormat.IsInFrame() || (ltwidget.Widget as WParagraph).ParagraphFormat.IsNextParagraphInSameFrame()) && (!(ltwidget.Widget is SplitWidgetContainer) || !((ltwidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph).ParagraphFormat.IsInFrame() || ((ltwidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph).ParagraphFormat.IsNextParagraphInSameFrame()))
      return;
    WParagraph wparagraph = ltwidget.Widget is WParagraph ? ltwidget.Widget as WParagraph : (ltwidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
    WSection ownerSection = wparagraph.GetOwnerSection();
    ushort num1 = (ushort) ((ltwidget.Widget is WParagraph ? (double) (ltwidget.Widget as WParagraph).ParagraphFormat.FrameHeight : (double) ((ltwidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph).ParagraphFormat.FrameHeight) * 20.0);
    float frameheight = (float) (((int) num1 & (int) short.MaxValue) / 20);
    if (((int) num1 & 32768 /*0x8000*/) == 0 && (double) wparagraph.ParagraphFormat.FrameHeight != 0.0)
      return;
    int index = widget.ChildWidgets.Count - 1;
    float num2 = this.GetsFrameHeight(ref index, widget, frameheight);
    if ((double) wparagraph.ParagraphFormat.FrameY <= (double) ownerSection.PageSetup.PageSize.Height - (double) num2 || index == -1 || wparagraph.ParagraphFormat.FrameVerticalPos != (byte) 1)
      return;
    float num3 = ownerSection.PageSetup.PageSize.Height - num2;
    float num4 = widget.ChildWidgets[index].Bounds.Y - num3;
    this.shiftYPosition(widget, index, num4);
    this.UpdateFloatingItemBounds(num4);
  }

  private void shiftYPosition(LayoutedWidget ltwidget, int index, float yposition)
  {
    for (int index1 = index; index1 < ltwidget.ChildWidgets.Count; ++index1)
      ltwidget.ChildWidgets[index1].ShiftLocation(0.0, -(double) yposition, false, false);
  }

  private float GetsFrameHeight(ref int index, LayoutedWidget LayoutedWidget, float frameheight)
  {
    float num = 0.0f;
    while (index >= 0)
    {
      LayoutedWidget childWidget = LayoutedWidget.ChildWidgets[index];
      WParagraph wparagraph = !(childWidget.Widget is WParagraph) || !(childWidget.Widget as WParagraph).ParagraphFormat.IsInFrame() ? (!(childWidget.Widget is SplitWidgetContainer) || !((childWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph).ParagraphFormat.IsInFrame() ? (WParagraph) null : (childWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph) : childWidget.Widget as WParagraph;
      if (wparagraph != null)
      {
        num += childWidget.Bounds.Height + (childWidget.Widget.LayoutInfo as ParagraphLayoutInfo).Margins.Top + (childWidget.Widget.LayoutInfo as ParagraphLayoutInfo).Margins.Bottom;
        if ((double) num > (double) frameheight)
          frameheight = num;
        if (wparagraph.PreviousSibling == null || !(wparagraph.PreviousSibling is WParagraph) || !wparagraph.ParagraphFormat.IsPreviousParagraphInSameFrame())
          return frameheight;
        --index;
      }
      else
        --index;
    }
    return frameheight;
  }

  private void UpdateFloatingItemBounds(float yPosition)
  {
    for (int index = (this.m_lcOperator as Layouter).FloatingItems.Count - 1; index >= 0; --index)
    {
      FloatingItem floatingItem = (this.m_lcOperator as Layouter).FloatingItems[index];
      if (floatingItem.FloatingEntity is WParagraph)
      {
        floatingItem.TextWrappingBounds = new RectangleF(floatingItem.TextWrappingBounds.X, floatingItem.TextWrappingBounds.Y - yPosition, floatingItem.TextWrappingBounds.Width, floatingItem.TextWrappingBounds.Height);
        if (floatingItem.FloatingEntity is WParagraph && !(floatingItem.FloatingEntity as WParagraph).ParagraphFormat.IsPreviousParagraphInSameFrame())
          break;
      }
    }
  }

  private RectangleF ResetAdjustedboundsBasedOnWrapPolygon(
    FloatingItem floatingItem,
    RectangleF currChildLWBounds)
  {
    float num1 = 21600f / currChildLWBounds.Width;
    float num2 = 21600f / currChildLWBounds.Height;
    float num3 = 0.0f;
    float num4 = 0.0f;
    if (floatingItem.FloatingEntity is WTextBox)
    {
      num3 = (floatingItem.FloatingEntity as WTextBox).TextBoxFormat.WrapPolygon.Vertices[0].X / num1;
      num4 = (floatingItem.FloatingEntity as WTextBox).TextBoxFormat.WrapPolygon.Vertices[0].Y / num2;
    }
    else if (floatingItem.FloatingEntity is WPicture)
    {
      num3 = (floatingItem.FloatingEntity as WPicture).WrapPolygon.Vertices[0].X / num1;
      num4 = (floatingItem.FloatingEntity as WPicture).WrapPolygon.Vertices[0].Y / num2;
    }
    else if (floatingItem.FloatingEntity is Shape)
    {
      num3 = (floatingItem.FloatingEntity as Shape).WrapFormat.WrapPolygon.Vertices[0].X / num1;
      num4 = (floatingItem.FloatingEntity as Shape).WrapFormat.WrapPolygon.Vertices[0].Y / num2;
    }
    else if (floatingItem.FloatingEntity is GroupShape)
    {
      num3 = (floatingItem.FloatingEntity as GroupShape).WrapFormat.WrapPolygon.Vertices[0].X / num1;
      num4 = (floatingItem.FloatingEntity as GroupShape).WrapFormat.WrapPolygon.Vertices[0].Y / num2;
    }
    else if (floatingItem.FloatingEntity is WChart)
    {
      num3 = (floatingItem.FloatingEntity as WChart).WrapFormat.WrapPolygon.Vertices[0].X / num1;
      num4 = (floatingItem.FloatingEntity as WChart).WrapFormat.WrapPolygon.Vertices[0].Y / num2;
    }
    currChildLWBounds.X -= num3;
    currChildLWBounds.Y -= num4;
    return currChildLWBounds;
  }

  private void SplitEndNoteWidgets()
  {
    if ((this.m_lcOperator as Layouter).EndnoteSplittedWidgets.Count <= 0)
      return;
    this.m_sptWidget = (IWidget) new SplitWidgetContainer(this.m_currChildLW.Widget is SplitWidgetContainer ? (IWidgetContainer) ((this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer as Syncfusion.DocIO.DLS.WidgetContainer) : (IWidgetContainer) (this.m_currChildLW.Widget as Syncfusion.DocIO.DLS.WidgetContainer));
    this.SplitedUpWidget(this.m_sptWidget, true);
    this.m_ltState = LayoutState.Splitted;
  }

  private void AddBehindWidgets(LayoutedWidget ltWidget)
  {
    if ((this.m_lcOperator as Layouter).IsLayoutingHeaderFooter)
    {
      if ((this.m_lcOperator as Layouter).IsLayoutingHeader)
        ++(this.m_lcOperator as Layouter).NumberOfBehindWidgetsInHeader;
      else
        ++(this.m_lcOperator as Layouter).NumberOfBehindWidgetsInFooter;
    }
    int index1 = -1;
    for (int index2 = 0; index2 < (this.m_lcOperator as Layouter).BehindWidgets.Count; ++index2)
    {
      if ((this.m_lcOperator as Layouter).BehindWidgets[index2].Widget == ltWidget.Widget)
      {
        index1 = index2;
        break;
      }
    }
    if (index1 == -1)
      (this.m_lcOperator as Layouter).BehindWidgets.Add(ltWidget);
    else
      (this.m_lcOperator as Layouter).BehindWidgets[index1] = ltWidget;
  }

  private bool CheckWidgetWrappingType(IWidget widget)
  {
    switch (widget)
    {
      case WPicture _:
      case WTextBox _:
      case Shape _:
      case WChart _:
      case GroupShape _:
        TextWrappingStyle textWrappingStyle;
        return (textWrappingStyle = (widget as ParagraphItem).GetTextWrappingStyle()) == TextWrappingStyle.Square || textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through;
      default:
        return false;
    }
  }

  private bool CheckWidgetWrappingTypeAndHorizontalOrigin(IWidget widget)
  {
    ParagraphItem paragraphItem = widget as ParagraphItem;
    switch (widget)
    {
      case WPicture _:
      case Shape _:
      case WTextBox _:
      case WChart _:
      case GroupShape _:
        return this.CheckWrappingTypeAndHorizontalOrigin(paragraphItem.GetTextWrappingStyle(), paragraphItem.GetHorizontalOrigin());
      default:
        return false;
    }
  }

  private bool CheckWrappingTypeAndHorizontalOrigin(
    TextWrappingStyle textWrappingStyle,
    HorizontalOrigin horizontalOrigin)
  {
    if (textWrappingStyle != TextWrappingStyle.InFrontOfText && textWrappingStyle != TextWrappingStyle.Behind)
      return false;
    return horizontalOrigin == HorizontalOrigin.Column || horizontalOrigin == HorizontalOrigin.Character;
  }

  private void IsDynamicRelayoutOccurByFrame(LayoutContext childContext)
  {
    int index = this.m_ltWidget.ChildWidgets.IndexOf(this.m_currChildLW);
    if (index <= 0)
      return;
    float previousItemBottom = this.GetPreviousItemBottom(index);
    float frameYposition = this.GetFrameYPosition((this.m_currChildLW.Widget as WParagraph).ParagraphFormat, index);
    if ((double) previousItemBottom <= 0.0 || (double) previousItemBottom <= (double) frameYposition)
      return;
    if (this.m_lcOperator is Layouter lcOperator && lcOperator.MaintainltWidget.ChildWidgets.Count > 0)
      lcOperator.MaintainltWidget.ChildWidgets.RemoveRange(0, lcOperator.MaintainltWidget.ChildWidgets.Count);
    this.SplitedUpWidget(childContext.SplittedWidget, false);
    this.m_ltState = LayoutState.DynamicRelayout;
  }

  private float GetFrameYPosition(WParagraphFormat paraFormat, int index)
  {
    for (int index1 = index - 1; index1 >= 0; --index1)
    {
      LayoutedWidget childWidget = this.m_ltWidget.ChildWidgets[index1];
      if (!(childWidget.Widget is WParagraph) || !(childWidget.Widget as WParagraph).ParagraphFormat.IsInSameFrame(paraFormat))
        return this.m_ltWidget.ChildWidgets[index1 + 1].Bounds.Y;
    }
    return this.m_ltWidget.ChildWidgets[index].Bounds.Y;
  }

  private float GetPreviousItemBottom(int index)
  {
    for (int index1 = index - 1; index1 >= 0; --index1)
    {
      LayoutedWidget childWidget = this.m_ltWidget.ChildWidgets[index1];
      if ((double) childWidget.Bounds.Y <= (double) this.m_currChildLW.Bounds.Bottom)
      {
        if (childWidget.Widget is WParagraph && !(childWidget.Widget as WParagraph).ParagraphFormat.IsInFrame() || childWidget.Widget is SplitWidgetContainer && (childWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph && !((childWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph).ParagraphFormat.IsInFrame())
          return this.GetPreviousLineBottom(childWidget, this.m_currChildLW.Bounds.Bottom);
        if (childWidget.Widget is WTable && !(childWidget.Widget as WTable).TableFormat.WrapTextAround && !(childWidget.Widget as WTable).IsFrame)
          return childWidget.Bounds.Bottom;
      }
    }
    return 0.0f;
  }

  private float GetPreviousLineBottom(LayoutedWidget lineContainer, float frameBottom)
  {
    for (int index = lineContainer.ChildWidgets.Count - 1; index >= 0; --index)
    {
      if ((double) lineContainer.ChildWidgets[index].Bounds.Y <= (double) frameBottom)
        return lineContainer.ChildWidgets[index].Bounds.Bottom;
    }
    return lineContainer.Bounds.Bottom;
  }

  private void UpdateLWBounds(LayoutContext childContext)
  {
    float num1 = 0.0f;
    RectangleF bounds1 = this.m_ltWidget.Bounds;
    bool flag1 = false;
    double num2 = this.m_bSkipAreaSpacing ? 0.0 : (double) childContext.BoundsPaddingRight;
    double num3 = this.m_bSkipAreaSpacing ? 0.0 : (double) childContext.BoundsMarginBottom;
    bool flag2 = false;
    RectangleF bounds2;
    if (childContext is LCLineContainer)
    {
      WParagraph wparagraph = this.m_currChildLW.Widget is WParagraph ? this.m_currChildLW.Widget as WParagraph : (this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
      num3 += (double) wparagraph.ParagraphFormat.Borders.Bottom.LineWidth;
      WParagraph nextSibling = wparagraph.NextSibling is WParagraph ? wparagraph.NextSibling as WParagraph : (WParagraph) null;
      if (nextSibling != null && !wparagraph.IsInCell && nextSibling.SectionEndMark)
        num3 = 0.0;
      flag1 = wparagraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.Exactly;
      bounds2 = this.m_currChildLW.Bounds;
      num1 = bounds2.Height;
      LayoutedWidgetList layoutedWidgetList = (LayoutedWidgetList) null;
      if (this.m_currChildLW.ChildWidgets.Count > 0)
        layoutedWidgetList = this.m_currChildLW.ChildWidgets[this.m_currChildLW.ChildWidgets.Count - 1].ChildWidgets;
      float rowHeight = 0.0f;
      if (wparagraph != null && wparagraph.IsInCell && layoutedWidgetList != null && layoutedWidgetList.Count > 0 && (wparagraph.IsLastLine(layoutedWidgetList[layoutedWidgetList.Count - 1]) || this.m_currChildLW.TextTag == "Splitted") && !wparagraph.IsExactlyRowHeight(wparagraph.GetOwnerEntity() as WTableCell, ref rowHeight) && this.m_currChildLW.Widget.LayoutInfo is ILayoutSpacingsInfo)
        num3 -= (double) (this.m_currChildLW.Widget.LayoutInfo as ILayoutSpacingsInfo).Margins.Bottom;
    }
    float num4 = num1;
    if ((double) num1 > (double) bounds1.Height && (double) bounds1.Height != 0.0)
      num4 -= bounds1.Height;
    if (this.m_ltWidget.ChildWidgets.Count > 0 && this.m_ltWidget.Widget is WParagraph && !(childContext.Widget is ParagraphItem) && this.IsInFrame(this.m_ltWidget.Widget as WParagraph))
    {
      LayoutedWidget childWidget = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1];
      WParagraph widget1 = this.m_ltWidget.Widget as WParagraph;
      IWidget widget2 = childWidget.Widget;
      if (childWidget.ChildWidgets.Count > 0)
        widget2 = childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1].Widget;
      if (widget2 is SplitStringWidget && (widget2 as SplitStringWidget).SplittedText != null && ((widget2 as SplitStringWidget).SplittedText == string.Empty || (int) (widget2 as SplitStringWidget).SplittedText[(widget2 as SplitStringWidget).SplittedText.Length - 1] == (int) ((widget2 as SplitStringWidget).RealStringWidget as WTextRange).Text[((widget2 as SplitStringWidget).RealStringWidget as WTextRange).Text.Length - 1]))
        widget2 = (IWidget) (widget2 as SplitStringWidget).RealStringWidget;
      if (widget2 is ParagraphItem)
      {
        int count = widget1.ChildEntities.Count;
        if (widget1.ChildEntities.IndexOf((IEntity) (widget2 as Entity)) == count - 1)
        {
          flag2 = true;
          this.UpdateFrameBounds(widget1);
        }
        else
        {
          for (int index = widget1.ChildEntities.IndexOf((IEntity) (widget2 as Entity)) + 1; index < widget1.ChildEntities.Count && (widget1.ChildEntities[index] as IWidget).LayoutInfo.IsSkip; ++index)
            --count;
          if (widget1.ChildEntities.IndexOf((IEntity) (widget2 as Entity)) == count - 1)
          {
            this.UpdateFrameBounds(widget1);
            flag2 = true;
          }
        }
      }
    }
    float rightEgeExtent = 0.0f;
    float bottomEdgeExtent = 0.0f;
    if (this.m_currChildLW.Widget is ParagraphItem)
      (this.m_currChildLW.Widget as ParagraphItem).GetEffectExtentValues(out float _, out rightEgeExtent, out float _, out bottomEdgeExtent);
    RectangleF bounds3 = this.m_currChildLW.Bounds;
    double num5 = Math.Max((double) bounds3.Right + num2 + (double) rightEgeExtent, (double) bounds1.Right);
    double num6 = 0.0;
    WTextRange textRange = this.GetTextRange(childContext.Widget);
    if (this.IsBottomPositionNeedToBeUpdate(childContext))
      num6 = (double) bounds1.Bottom <= (double) bounds3.Bottom || (double) bounds1.X >= (double) bounds3.X ? ((double) num1 <= (double) bounds1.Height ? (!flag1 || !(this.m_currChildLW.Widget is SplitWidgetContainer) || flag2 || !((this.m_currChildLW.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph) ? Math.Max((double) bounds3.Bottom + num3 + (double) bottomEdgeExtent, (double) bounds1.Bottom) : (double) bounds3.Y + (double) num1) : Math.Max((double) bounds3.Bottom + num3 + (double) bottomEdgeExtent, (double) bounds1.Bottom + (double) num4)) : Math.Max((double) bounds3.Bottom + num3 + (double) bottomEdgeExtent, (double) bounds1.Bottom);
    bool flag3 = (childContext.Widget is WPicture || childContext.Widget is WChart || childContext.Widget is Shape || childContext.Widget is GroupShape) && (childContext.Widget as ParagraphItem).GetTextWrappingStyle() == TextWrappingStyle.Inline;
    WParagraph wparagraph1 = textRange != null ? textRange.OwnerParagraph : (flag3 ? (childContext.Widget as ParagraphItem).GetOwnerParagraphValue() : (WParagraph) null);
    if (wparagraph1 == null && textRange != null)
    {
      if (textRange.Owner is InlineContentControl || textRange.Owner is XmlParagraphItem)
        wparagraph1 = textRange.GetOwnerParagraphValue();
      else if (textRange.OwnerParagraph == null)
        wparagraph1 = textRange.CharacterFormat.BaseFormat.OwnerBase as WParagraph;
    }
    if ((textRange != null || flag3) && (double) bounds3.Height != 0.0 && ((double) num1 > (double) bounds3.Height || flag1) && !childContext.Widget.LayoutInfo.IsLineBreak && wparagraph1 != null && (wparagraph1.ParagraphFormat.LineSpacingRule != LineSpacingRule.Multiple || (double) num1 < 12.0 && (double) Math.Abs(wparagraph1.ParagraphFormat.LineSpacing) < 12.0 && !flag3))
    {
      float top = (wparagraph1.m_layoutInfo as ParagraphLayoutInfo).Margins.Top;
      if ((!this.LayoutInfo.IsClipped || !wparagraph1.IsInCell || (double) (wparagraph1.GetOwnerEntity() as WTableCell).OwnerRow.Height >= (double) num1) && (!(wparagraph1.OwnerTextBody.Owner is Shape) || (double) (wparagraph1.OwnerTextBody.Owner as Shape).TextLayoutingBounds.Height >= (double) num1))
      {
        if ((double) num1 < (double) bounds3.Height && flag1)
          this.m_currChildLW.Bounds = new RectangleF(bounds3.X, (float) num6 + top - bounds3.Height, bounds3.Width, bounds3.Height);
        else if (!flag3 && textRange != null && textRange.CharacterFormat.SubSuperScript == SubSuperScript.None)
          this.m_currChildLW.Bounds = new RectangleF(bounds3.X, (float) num6 - bounds3.Height, bounds3.Width, bounds3.Height);
        else if (flag3 && (double) num1 > (double) bounds3.Height && wparagraph1.ParagraphFormat.LineSpacingRule == LineSpacingRule.AtLeast)
          this.m_currChildLW.Bounds = new RectangleF(bounds3.X, (float) num6 - bounds3.Height, bounds3.Width, bounds3.Height);
      }
    }
    SizeF size1 = new SizeF((float) num5 - bounds1.Left, (float) num6 - bounds1.Top);
    if (flag2)
    {
      float num7 = bounds3.X - bounds1.X;
      size1.Width = bounds3.Width + ((double) num7 > 0.0 ? num7 : 0.0f);
    }
    if (childContext.Widget is WParagraph || childContext.Widget is SplitWidgetContainer && (childContext.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
    {
      if (!(childContext.Widget is WParagraph paragraph))
        paragraph = (childContext.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
      WParagraphStyle style = paragraph.GetStyle() as WParagraphStyle;
      if (!paragraph.ParagraphFormat.Borders.NoBorder || style != null && style.ParagraphFormat != null && !style.ParagraphFormat.Borders.NoBorder || style != null && style.BaseStyle != null && style.BaseStyle.ParagraphFormat != null && !style.BaseStyle.ParagraphFormat.Borders.NoBorder)
        size1.Width = this.m_layoutArea.ClientActiveArea.Width;
      if (childContext is LCLineContainer && paragraph != null && paragraph.ParagraphFormat.IsInFrame())
      {
        if (paragraph.GetBaseEntity((Entity) paragraph) is WSection baseEntity && baseEntity.Columns.Count == 1)
        {
          if (paragraph == (this.m_lcOperator as Layouter).DynamicParagraph)
            (this.m_lcOperator as Layouter).DynamicParagraph = (WParagraph) null;
          else if ((this.m_lcOperator as Layouter).DynamicParagraph == null && (this.m_lcOperator as Layouter).DynamicTable == null)
            this.IsDynamicRelayoutOccurByFrame(childContext);
        }
        if (this.IsInFrame(paragraph) && paragraph.ParagraphFormat.IsFrameYAlign(paragraph.ParagraphFormat.FrameY))
          this.UpdateVerticalAlignment((short) paragraph.ParagraphFormat.FrameY);
      }
    }
    if (childContext.Widget is WPicture || childContext.Widget is Shape || childContext.Widget is WChart || childContext.Widget is GroupShape)
    {
      WPicture widget3 = childContext.Widget as WPicture;
      Shape widget4 = childContext.Widget as Shape;
      WChart widget5 = childContext.Widget as WChart;
      GroupShape widget6 = childContext.Widget as GroupShape;
      TextWrappingStyle textWrappingStyle = (childContext.Widget as ParagraphItem).GetTextWrappingStyle();
      bool flag4 = widget4 != null ? widget4.WrapFormat.IsWrappingBoundsAdded : (widget5 != null ? widget5.WrapFormat.IsWrappingBoundsAdded : (widget6 != null ? widget6.WrapFormat.IsWrappingBoundsAdded : widget3.IsWrappingBoundsAdded));
      WParagraph ownerParagraphValue = (childContext.Widget as ParagraphItem).GetOwnerParagraphValue();
      if (textWrappingStyle == TextWrappingStyle.InFrontOfText || textWrappingStyle == TextWrappingStyle.Behind)
      {
        if (ownerParagraphValue != null && ownerParagraphValue.ChildEntities.Count == 1 && (double) this.m_ltWidget.Bounds.Height == 0.0)
        {
          float num8 = Math.Abs(ownerParagraphValue.ParagraphFormat.LineSpacing);
          SizeF size2 = ((IWidget) ownerParagraphValue).LayoutInfo.Size;
          size1.Height = ownerParagraphValue.ParagraphFormat.LineSpacingRule != LineSpacingRule.Exactly ? (ownerParagraphValue.ParagraphFormat.LineSpacingRule != LineSpacingRule.AtLeast ? size2.Height * (num8 / 12f) : ((double) size2.Height <= (double) num8 ? num8 : size2.Height)) : num8;
        }
        else
          size1.Height = this.m_ltWidget.Bounds.Height;
      }
      if (textWrappingStyle != TextWrappingStyle.Inline && textWrappingStyle != TextWrappingStyle.InFrontOfText && textWrappingStyle != TextWrappingStyle.Behind && !flag4 && !(this.m_lcOperator as Layouter).IsNeedToRelayout)
      {
        FloatingItem floatingItem = new FloatingItem();
        floatingItem.TextWrappingBounds = this.m_currChildLW.Bounds;
        floatingItem.FloatingEntity = this.m_currChildLW.Widget as Entity;
        (this.m_lcOperator as Layouter).FloatingItems.Add(floatingItem);
        floatingItem.WrapCollectionIndex = (this.m_lcOperator as Layouter).FloatingItems.Count - 1;
        floatingItem.IsFloatingItemFit = true;
      }
      if (textWrappingStyle != TextWrappingStyle.Inline && textWrappingStyle != TextWrappingStyle.InFrontOfText && textWrappingStyle != TextWrappingStyle.Behind && num6 == (double) bounds3.Bottom && ((double) this.m_ltWidget.Bounds.Height == 0.0 || (double) size1.Height != (double) this.m_ltWidget.Bounds.Height))
      {
        SizeF size3 = ((IWidget) ownerParagraphValue).LayoutInfo.Size;
        size1.Height = size3.Height;
      }
    }
    if (childContext.Widget is WTextBox)
    {
      WTextBox widget7 = childContext.Widget as WTextBox;
      if (widget7.TextBoxFormat.TextWrappingStyle != TextWrappingStyle.Inline)
      {
        if (!widget7.TextBoxFormat.IsWrappingBoundsAdded)
        {
          FloatingItem floatingItem1 = new FloatingItem();
          FloatingItem floatingItem2 = floatingItem1;
          bounds2 = this.m_currChildLW.ChildWidgets[0].Bounds;
          double x = (double) bounds2.X;
          bounds2 = this.m_currChildLW.ChildWidgets[0].Bounds;
          double y = (double) bounds2.Y;
          bounds2 = this.m_currChildLW.Bounds;
          double width = (double) bounds2.Width;
          bounds2 = this.m_currChildLW.Bounds;
          double height = (double) bounds2.Height;
          RectangleF rectangleF = new RectangleF((float) x, (float) y, (float) width, (float) height);
          floatingItem2.TextWrappingBounds = rectangleF;
          floatingItem1.FloatingEntity = this.m_currChildLW.Widget as Entity;
          (this.m_lcOperator as Layouter).FloatingItems.Add(floatingItem1);
          widget7.TextBoxFormat.WrapCollectionIndex = (short) ((this.m_lcOperator as Layouter).FloatingItems.Count - 1);
          floatingItem1.WrapCollectionIndex = (this.m_lcOperator as Layouter).FloatingItems.Count - 1;
          floatingItem1.IsFloatingItemFit = true;
        }
        if (this.m_ltWidget.Widget is WParagraph)
        {
          if (this.m_ltWidget.Widget is WParagraph widget8)
          {
            bounds2 = this.m_ltWidget.Bounds;
            if ((double) bounds2.Height == 0.0 && widget8.Text == string.Empty)
            {
              SizeF size4 = ((IWidget) widget8).LayoutInfo.Size;
              float num9 = Math.Abs(widget8.ParagraphFormat.LineSpacing);
              size1.Height = widget8.ParagraphFormat.LineSpacingRule != LineSpacingRule.Exactly ? (widget8.ParagraphFormat.LineSpacingRule != LineSpacingRule.AtLeast ? size4.Height * (num9 / 12f) : ((double) size4.Height <= (double) num9 ? num9 : size4.Height)) : num9;
              goto label_73;
            }
          }
          ref SizeF local = ref size1;
          bounds2 = this.m_ltWidget.Bounds;
          double height = (double) bounds2.Height;
          local.Height = (float) height;
        }
        else
        {
          ref SizeF local = ref size1;
          bounds2 = this.m_ltWidget.Bounds;
          double height = (double) bounds2.Height;
          local.Height = (float) height;
        }
      }
    }
label_73:
    if (childContext.Widget is WTable)
    {
      WTable widget = childContext.Widget as WTable;
      if (widget != (this.m_lcOperator as Layouter).DynamicTable && (childContext.Widget as WTable).TableFormat.WrapTextAround)
      {
        RowFormat.TablePositioning positioning = (childContext.Widget as WTable).TableFormat.Positioning;
        FloatingItem floatingItem3 = new FloatingItem();
        if (this.m_currChildLW.ChildWidgets.Count > 0)
        {
          bounds2 = this.m_currChildLW.Bounds;
          float a = bounds2.Width + positioning.DistanceFromLeft + positioning.DistanceFromRight;
          FloatingItem floatingItem4 = floatingItem3;
          bounds2 = this.m_currChildLW.Bounds;
          double x = (double) bounds2.X - (double) positioning.DistanceFromLeft;
          bounds2 = this.m_currChildLW.Bounds;
          double y = (double) bounds2.Y - (double) positioning.DistanceFromTop;
          double width = Math.Round((double) a);
          bounds2 = this.m_currChildLW.Bounds;
          double height = (double) bounds2.Height + (double) positioning.DistanceFromTop + (double) positioning.DistanceFromBottom;
          RectangleF rectangleF = new RectangleF((float) x, (float) y, (float) width, (float) height);
          floatingItem4.TextWrappingBounds = rectangleF;
        }
        else
          floatingItem3.TextWrappingBounds = this.m_currChildLW.Bounds;
        floatingItem3.FloatingEntity = this.m_currChildLW.Widget as Entity;
        (this.m_lcOperator as Layouter).FloatingItems.Add(floatingItem3);
        floatingItem3.WrapCollectionIndex = (this.m_lcOperator as Layouter).FloatingItems.Count - 1;
      }
      else if (this.IsInFrame(widget))
        this.UpdateFrameBounds(widget);
      int index1 = this.m_ltWidget.ChildWidgets.IndexOf(this.m_currChildLW);
      int index2 = -1;
      if (index1 > 0)
        index2 = this.GetPreviousItemIndex(index1);
      if (index2 > -1)
      {
        bounds2 = this.m_ltWidget.ChildWidgets[index2].Bounds;
        double num10 = Math.Round((double) bounds2.Bottom);
        bounds2 = this.m_currChildLW.Bounds;
        double num11 = Math.Round((double) bounds2.Y);
        if (num10 > num11)
        {
          bounds2 = this.m_ltWidget.ChildWidgets[index2].Bounds;
          if ((double) bounds2.Height > 0.0 && !(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && (this.m_lcOperator as Layouter).DynamicTable == null)
          {
            Layouter lcOperator = this.m_lcOperator as Layouter;
            if (lcOperator.MaintainltWidget.ChildWidgets.Count > 0)
              lcOperator.MaintainltWidget.ChildWidgets.RemoveRange(0, lcOperator.MaintainltWidget.ChildWidgets.Count);
            this.SplitedUpWidget(childContext.SplittedWidget, false);
            this.m_ltState = LayoutState.DynamicRelayout;
          }
        }
      }
      if (widget == (this.m_lcOperator as Layouter).DynamicTable)
        (this.m_lcOperator as Layouter).DynamicTable = (WTable) null;
    }
    if ((double) size1.Height < 0.0)
      size1.Height = 0.0f;
    if ((double) size1.Width < 0.0)
      size1.Width = 0.0f;
    bounds2 = this.m_currChildLW.Bounds;
    if ((double) bounds2.Height == 0.0 && this.m_currChildLW.Widget is WParagraph && this.m_currChildLW.Widget.LayoutInfo.IsSkip && this.m_currChildLW.ChildWidgets.Count == 0)
      size1 = new SizeF();
    this.m_ltWidget.Bounds = new RectangleF(bounds1.Location, size1);
    if (!flag2)
      return;
    this.AddFrameBounds();
  }

  private int GetPreviousItemIndex(int index)
  {
    for (int index1 = index - 1; index1 >= 0; --index1)
    {
      if (this.m_ltWidget.ChildWidgets[index1].Widget is WParagraph && !this.IsFloatingFrame(this.m_ltWidget.ChildWidgets[index1].Widget as WParagraph) || this.m_ltWidget.ChildWidgets[index1].Widget is SplitWidgetContainer && (this.m_ltWidget.ChildWidgets[index1].Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph && !this.IsFloatingFrame((this.m_ltWidget.ChildWidgets[index1].Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph) || this.m_ltWidget.ChildWidgets[index1].Widget is WTable && !(this.m_ltWidget.ChildWidgets[index1].Widget as WTable).TableFormat.WrapTextAround && !(this.m_ltWidget.ChildWidgets[index1].Widget as WTable).IsFrame)
        return index1;
    }
    return -1;
  }

  private bool IsFloatingFrame(WParagraph paragraph)
  {
    return paragraph != null && paragraph.ParagraphFormat.IsFrame && paragraph.ParagraphFormat.WrapFrameAround != FrameWrapMode.None;
  }

  private bool IsBottomPositionNeedToBeUpdate(LayoutContext childContext)
  {
    if (!(this.m_ltWidget.Widget is WSection) && !(this.m_ltWidget.Widget is HeaderFooter) && (!(this.m_ltWidget.Widget is BlockContentControl) || !this.IsInSection(this.m_ltWidget.Widget as BlockContentControl) || childContext.Widget is WTable && (childContext.Widget as WTable).TableFormat.WrapTextAround) && (!(this.m_ltWidget.Widget is SplitWidgetContainer) || !((this.m_ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WSection)))
      return true;
    if (childContext.Widget is WParagraph && this.IsInFrame(childContext.Widget as WParagraph) || childContext.Widget is SplitWidgetContainer && (childContext.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph && this.IsInFrame((childContext.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph) && this.m_ltWidget.Widget is BlockContentControl)
      return false;
    if (!(childContext.Widget is WTable))
      return true;
    return !(childContext.Widget as WTable).TableFormat.WrapTextAround && !this.IsInFrame(childContext.Widget as WTable);
  }

  private bool IsInSection(BlockContentControl blockContentControl)
  {
    bool flag = false;
    if (blockContentControl.OwnerTextBody.Owner is WSection)
      flag = true;
    else if (blockContentControl.OwnerTextBody.Owner is BlockContentControl)
      flag = this.IsInSection(blockContentControl.OwnerTextBody.Owner as BlockContentControl);
    return flag;
  }

  private void AddFrameBounds()
  {
    RectangleF bounds = this.m_ltWidget.Bounds;
    WParagraph widget = this.m_ltWidget.Widget as WParagraph;
    bool flag1 = true;
    if (widget != null && widget.IsInCell && (widget.GetOwnerEntity() as WTableCell).OwnerRow.OwnerTable.IsFrame)
      flag1 = false;
    bool flag2 = false;
    ILayoutSpacingsInfo layoutInfo = this.LayoutInfo as ILayoutSpacingsInfo;
    if (widget == null || !widget.ParagraphFormat.IsFrame || !flag1 || widget.ParagraphFormat.WrapFrameAround == FrameWrapMode.None)
      return;
    float num1 = 72f;
    if ((double) bounds.Bottom < (double) (this.m_lcOperator as Layouter).FrameLayoutArea.Y)
      bounds.Height += (this.m_lcOperator as Layouter).FrameLayoutArea.Y - bounds.Bottom;
    if ((double) widget.ParagraphFormat.FrameWidth != 0.0)
      bounds.Width = widget.ParagraphFormat.FrameWidth;
    else if (!widget.ParagraphFormat.IsNextParagraphInSameFrame() && !widget.ParagraphFormat.IsPreviousParagraphInSameFrame())
    {
      bounds.Height += 2f * widget.ParagraphFormat.FrameVerticalDistanceFromText;
      flag2 = true;
    }
    else
    {
      bounds.Width = (this.m_lcOperator as Layouter).FrameLayoutArea.Width;
      bounds.Height += (float) (2.0 * (double) widget.ParagraphFormat.FrameVerticalDistanceFromText + (layoutInfo != null ? (double) layoutInfo.Margins.Top + (double) layoutInfo.Margins.Bottom : 0.0));
    }
    if (!widget.ParagraphFormat.IsNextParagraphInSameFrame() || !widget.ParagraphFormat.IsPreviousParagraphInSameFrame())
      bounds.Y -= (float) ((double) widget.ParagraphFormat.FrameVerticalDistanceFromText + (layoutInfo != null ? (double) layoutInfo.Margins.Top : 0.0) + ((((IWidget) widget).LayoutInfo as ParagraphLayoutInfo).SkipTopBorder ? 0.0 : (double) widget.ParagraphFormat.Borders.Top.GetLineWidthValue() + (double) widget.ParagraphFormat.Borders.Top.Space));
    if (!widget.ParagraphFormat.IsNextParagraphInSameFrame() || !widget.ParagraphFormat.IsPreviousParagraphInSameFrame())
      bounds.Height += (float) (2.0 * (double) widget.ParagraphFormat.FrameVerticalDistanceFromText + (layoutInfo != null ? (double) layoutInfo.Margins.Top + (double) layoutInfo.Margins.Bottom : 0.0) + ((((IWidget) widget).LayoutInfo as ParagraphLayoutInfo).SkipTopBorder ? 0.0 : (double) widget.ParagraphFormat.Borders.Bottom.GetLineWidthValue() + (double) widget.ParagraphFormat.Borders.Bottom.Space));
    FloatingItem floatingItem = new FloatingItem();
    if ((double) bounds.X > (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Left && (double) bounds.X - (double) num1 < (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Left)
    {
      float num2 = bounds.X - (this.m_lcOperator as Layouter).ClientLayoutArea.Left;
      bounds.X = (this.m_lcOperator as Layouter).ClientLayoutArea.Left;
      bounds.Width += num2;
      floatingItem.TextWrappingBounds = bounds;
    }
    else
    {
      if (flag2)
      {
        bounds.Width += 2f * widget.ParagraphFormat.FrameHorizontalDistanceFromText;
        bounds.X -= widget.ParagraphFormat.FrameHorizontalDistanceFromText;
      }
      else
      {
        if ((double) widget.ParagraphFormat.FrameWidth != 0.0)
          bounds.Width += 2f * widget.ParagraphFormat.FrameHorizontalDistanceFromText;
        else
          bounds.Width += (float) (2.0 * (double) widget.ParagraphFormat.FrameHorizontalDistanceFromText + (layoutInfo != null ? (double) layoutInfo.Margins.Left + (double) layoutInfo.Margins.Right : 0.0));
        bounds.X -= widget.ParagraphFormat.FrameHorizontalDistanceFromText + (layoutInfo != null ? layoutInfo.Margins.Left : 0.0f);
      }
      floatingItem.TextWrappingBounds = bounds;
    }
    floatingItem.FloatingEntity = this.m_ltWidget.Widget as Entity;
    if (this.IsFloatingEntityExist(floatingItem.FloatingEntity))
      return;
    (this.m_lcOperator as Layouter).FloatingItems.Add(floatingItem);
    floatingItem.WrapCollectionIndex = (this.m_lcOperator as Layouter).FloatingItems.Count - 1;
  }

  private bool IsFloatingEntityExist(Entity entity)
  {
    foreach (FloatingItem floatingItem in (this.m_lcOperator as Layouter).FloatingItems)
    {
      if (floatingItem.FloatingEntity == entity)
        return true;
    }
    return false;
  }

  private void UpdateFrameBounds(WParagraph paragraph)
  {
    RectangleF bounds = this.m_ltWidget.Bounds;
    IEntity nextSibling = paragraph.NextSibling;
    if ((nextSibling == null || nextSibling is WParagraph && !paragraph.ParagraphFormat.IsNextParagraphInSameFrame() || nextSibling is WTable && !(nextSibling as WTable).IsFrame) && (double) paragraph.ParagraphFormat.FrameHeight != 0.0)
    {
      bool flag = ((int) (ushort) ((double) paragraph.ParagraphFormat.FrameHeight * 20.0) & 32768 /*0x8000*/) != 0;
      float num = this.m_currChildLW.Bounds.Bottom - this.m_ltWidget.Bounds.Y;
      if (flag && (double) num < (double) (this.m_lcOperator as Layouter).FrameHeight)
      {
        bounds.Height = (this.m_lcOperator as Layouter).FrameHeight;
        this.m_currChildLW.Bounds = bounds;
      }
      else if (!flag)
      {
        bounds.Height = bounds.Height + (this.m_lcOperator as Layouter).FrameLayoutArea.Bottom - bounds.Bottom;
        this.m_currChildLW.Bounds = bounds;
      }
    }
    if ((double) paragraph.ParagraphFormat.FrameWidth >= (double) this.m_layoutArea.ClientActiveArea.Width || !paragraph.ParagraphFormat.IsFrameXAlign(paragraph.ParagraphFormat.FrameX))
      return;
    this.UpdateHorizontalAlignment((short) paragraph.ParagraphFormat.FrameX);
  }

  private void UpdateFrameBounds(WTable table)
  {
    RectangleF bounds = this.m_currChildLW.Bounds;
    IEntity nextSibling = table.NextSibling;
    if ((nextSibling == null || nextSibling is WParagraph && !(nextSibling as WParagraph).ParagraphFormat.IsFrame || nextSibling is WTable && !(nextSibling as WTable).IsFrame) && table.Rows.Count > 0 && table.Rows[0].Cells.Count > 0 && table.Rows[0].Cells[0].Paragraphs.Count > 0 && (double) table.Rows[0].Cells[0].Paragraphs[0].ParagraphFormat.FrameHeight != 0.0)
    {
      ushort num1 = (ushort) ((double) table.Rows[0].Cells[0].Paragraphs[0].ParagraphFormat.FrameHeight * 20.0);
      bool flag = ((int) num1 & 32768 /*0x8000*/) != 0;
      float num2 = (float) (((int) num1 & (int) short.MaxValue) / 20);
      if (!flag || (double) this.m_currChildLW.Bounds.Height < (double) num2)
      {
        bounds.Height = bounds.Height + (this.m_lcOperator as Layouter).FrameLayoutArea.Bottom - bounds.Bottom;
        this.m_currChildLW.Bounds = bounds;
      }
    }
    WParagraphFormat paragraphFormat = table.Rows[0].Cells[0].Paragraphs[0].ParagraphFormat;
    if (paragraphFormat.IsFrameXAlign(paragraphFormat.FrameX))
      this.UpdateHorizontalAlignment((short) paragraphFormat.FrameX);
    if (paragraphFormat.IsFrameYAlign(paragraphFormat.FrameY))
      this.UpdateVerticalAlignment((short) paragraphFormat.FrameY);
    FloatingItem floatingItem = new FloatingItem();
    Layouter lcOperator = this.m_lcOperator as Layouter;
    floatingItem.TextWrappingBounds = new RectangleF(lcOperator.FrameLayoutArea.X, this.m_currChildLW.Bounds.Y, this.m_currChildLW.Bounds.Width, this.m_currChildLW.Bounds.Height);
    floatingItem.FloatingEntity = this.m_currChildLW.Widget as Entity;
    lcOperator.FloatingItems.Add(floatingItem);
    floatingItem.WrapCollectionIndex = lcOperator.FloatingItems.Count - 1;
  }

  protected virtual void UpdateHorizontalAlignment(short xAlignment)
  {
  }

  private void UpdateVerticalAlignment(short yAlginment)
  {
    RectangleF bounds = this.m_currChildLW.Bounds;
    switch (yAlginment)
    {
      case -20:
      case -12:
        this.m_currChildLW.ShiftLocation(0.0, -(double) bounds.Height, true, false);
        break;
      case -8:
        this.m_currChildLW.ShiftLocation(0.0, -(double) bounds.Height / 2.0, true, false);
        break;
    }
  }

  private void CommitKeepWithNext(ref IWidget splittedWidget, bool isKeep)
  {
    WSection section = this.GetSection();
    bool isLastTocParagraphRemoved = false;
    if (section != null && section.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
    {
      this.CommitWithKeepWithNexForWord2013Format(ref splittedWidget, section, ref isLastTocParagraphRemoved);
      if (!isLastTocParagraphRemoved)
        this.IsEndOfTOC();
      if (splittedWidget == null)
        return;
      this.RemoveAutoHyphenatedString(splittedWidget, section.Document.DOP.AutoHyphen);
    }
    else
    {
      bool isWidgetsRemoved = false;
      int splittedWidgetIndex = 0;
      if (isKeep)
        this.RemoveltWidgets(ref isLastTocParagraphRemoved, this.m_ltWidget, ref splittedWidget, false, ref isWidgetsRemoved, ref splittedWidgetIndex);
      if (section != null && isKeep && !isLastTocParagraphRemoved)
        this.IsEndOfTOC();
      if (section == null || splittedWidget == null)
        return;
      this.RemoveAutoHyphenatedString(splittedWidget, section.Document.DOP.AutoHyphen);
    }
  }

  private WSection GetSection()
  {
    if (this.m_sptWidget is WSection)
      return this.m_sptWidget as WSection;
    if (this.m_sptWidget is BlockContentControl)
      return (this.m_sptWidget as BlockContentControl).GetOwnerSection(this.m_sptWidget as Entity) as WSection;
    if (this.m_sptWidget is SplitWidgetContainer)
    {
      if ((this.m_sptWidget as SplitWidgetContainer).RealWidgetContainer is WSection)
        return (this.m_sptWidget as SplitWidgetContainer).RealWidgetContainer as WSection;
      if ((this.m_sptWidget as SplitWidgetContainer).RealWidgetContainer is BlockContentControl)
        return ((this.m_sptWidget as SplitWidgetContainer).RealWidgetContainer as BlockContentControl).GetOwnerSection(this.m_sptWidget as Entity) as WSection;
    }
    return (WSection) null;
  }

  private bool RemoveltWidgets(
    ref bool isLastTocParagraphRemoved,
    LayoutedWidget lwtWidget,
    ref IWidget splittedWidget,
    bool isBlockContentControlChild,
    ref bool isWidgetsRemoved,
    ref int splittedWidgetIndex)
  {
    while (lwtWidget.ChildWidgets.Count > 0 && !(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter)
    {
      bool flag1 = false;
      IWidget widget = lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1].Widget;
      if (widget is WTable && !(widget as WTable).TableFormat.WrapTextAround && !(widget as WTable).IsInCell)
      {
        LayoutedWidget childWidget = lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1];
        int count = childWidget.ChildWidgets.Count;
        while (childWidget.ChildWidgets.Count > 0 && childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1].Widget.LayoutInfo.IsKeepWithNext && !this.IsForceFitLayout)
        {
          if (this.IsLastTOCParagraph(childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1].Widget as Entity))
            isLastTocParagraphRemoved = true;
          this.RemoveBehindWidgets(childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1]);
          childWidget.ChildWidgets.RemoveAt(childWidget.ChildWidgets.Count - 1);
          isWidgetsRemoved = true;
          flag1 = true;
          --count;
        }
        if (!flag1)
          return true;
        if (childWidget.ChildWidgets.Count > 0)
        {
          splittedWidget = (IWidget) new SplitTableWidget(childWidget.Widget as ITableWidget, count + 1);
          this.m_ltState = LayoutState.Splitted;
          if (isBlockContentControlChild)
            --splittedWidgetIndex;
          else
            --this.m_curWidgetIndex;
          return true;
        }
        this.RemoveBehindWidgets(lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1]);
        lwtWidget.ChildWidgets.RemoveAt(lwtWidget.ChildWidgets.Count - 1);
        if (isBlockContentControlChild)
          --splittedWidgetIndex;
        else
          --this.m_curWidgetIndex;
      }
      else if (widget is BlockContentControl)
      {
        LayoutedWidget childWidget = lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1];
        if (widget.LayoutInfo.IsKeepWithNext)
        {
          this.RemoveBehindWidgets(childWidget);
          lwtWidget.ChildWidgets.Remove(childWidget);
          isWidgetsRemoved = true;
          if (isBlockContentControlChild)
          {
            splittedWidget = widget;
            --splittedWidgetIndex;
          }
          else
            --this.m_curWidgetIndex;
        }
        else
        {
          int num = splittedWidgetIndex;
          bool flag2 = isWidgetsRemoved;
          splittedWidgetIndex = childWidget.ChildWidgets.Count;
          isWidgetsRemoved = false;
          bool flag3 = childWidget.ChildWidgets.Count <= 0 || this.RemoveltWidgets(ref isLastTocParagraphRemoved, childWidget, ref splittedWidget, true, ref isWidgetsRemoved, ref splittedWidgetIndex);
          if (splittedWidget != null && splittedWidget is SplitWidgetContainer)
            --splittedWidgetIndex;
          if (splittedWidget != null && isWidgetsRemoved)
            splittedWidget = (IWidget) new SplitWidgetContainer(widget as IWidgetContainer, splittedWidget, splittedWidgetIndex);
          splittedWidgetIndex = num;
          if (!isBlockContentControlChild && isWidgetsRemoved)
            --this.m_curWidgetIndex;
          if (!isWidgetsRemoved)
            isWidgetsRemoved = flag2;
          if (flag3)
            return flag3;
        }
      }
      else
      {
        if (!(widget.LayoutInfo is ParagraphLayoutInfo) || this.IsForceFitLayout || !widget.LayoutInfo.IsKeepWithNext || !this.CheckKeepWithNextForHiddenPara(widget as IEntity) || !this.m_bAtLastOneChildFitted || ((widget as WParagraph).Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 ? (!(widget as WParagraph).IsInCell ? 1 : 0) : 1) == 0)
          return true;
        this.UpdateFootnoteWidgets(lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1]);
        if (DocumentLayouter.IsUpdatingTOC && this.IsLastTOCParagraph(lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1].Widget as Entity))
          isLastTocParagraphRemoved = true;
        if (isBlockContentControlChild)
          splittedWidget = lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1].Widget;
        this.RemoveBehindWidgets(lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1]);
        lwtWidget.ChildWidgets.RemoveAt(lwtWidget.ChildWidgets.Count - 1);
        isWidgetsRemoved = true;
        if (isBlockContentControlChild)
          --splittedWidgetIndex;
        else
          --this.m_curWidgetIndex;
      }
    }
    return false;
  }

  private void IsEndOfTOC()
  {
    if (!DocumentLayouter.IsUpdatingTOC || (this.m_lcOperator as Layouter).LastTOCParagraph == null || !(this.m_lcOperator as Layouter).LastTOCParagraph.ParagraphFormat.KeepFollow || !this.IsEndOfTocParagraphLayouted())
      return;
    DocumentLayouter.IsEndUpdateTOC = true;
  }

  private bool IsEndOfTocParagraphLayouted()
  {
    foreach (KeyValuePair<Entity, int> tocEntryPageNumber in (this.m_lcOperator as Layouter).TOCEntryPageNumbers)
    {
      if (this.IsLastTOCParagraph(tocEntryPageNumber.Key))
        return true;
    }
    return false;
  }

  private bool IsLastTOCParagraph(Entity entity)
  {
    switch (entity)
    {
      case WParagraph _:
        return entity == (this.m_lcOperator as Layouter).LastTOCParagraph;
      case WTableRow _:
        IEnumerator enumerator = (entity as WTableRow).Cells.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            foreach (WParagraph paragraph in (IEnumerable) ((WTextBody) enumerator.Current).Paragraphs)
            {
              if (paragraph == (this.m_lcOperator as Layouter).LastTOCParagraph)
                return true;
            }
          }
          break;
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
    }
    return false;
  }

  private void CommitWithKeepWithNexForWord2013Format(
    ref IWidget splittedWidget,
    WSection section,
    ref bool isLastTocParagraphRemoved)
  {
    int columnIndex = this.DrawingContext.GetColumnIndex(section, this.m_ltWidget.Bounds);
    bool flag = columnIndex == 0 && this.IsNeedToRemoveItems(this.m_ltWidget);
    bool isWidgetsRemoved = false;
    int splittedWidgetIndex = 0;
    if (columnIndex == 0 && flag || columnIndex != 0)
    {
      this.RemoveItemsFromltWidgets(ref splittedWidget, ref isLastTocParagraphRemoved, this.m_ltWidget, false, ref isWidgetsRemoved, ref splittedWidgetIndex);
    }
    else
    {
      if (this.m_ltWidget.Widget is BlockContentControl)
        return;
      DocumentLayouter.IsEndPage = true;
    }
  }

  private bool IsNeedToRemoveItems(LayoutedWidget ltWidget)
  {
    for (int index1 = 0; index1 < ltWidget.ChildWidgets.Count; ++index1)
    {
      IWidget widget = ltWidget.ChildWidgets[index1].Widget;
      if (widget is WTable && !(widget as WTable).TableFormat.WrapTextAround && !(widget as WTable).IsInCell)
      {
        LayoutedWidget childWidget = ltWidget.ChildWidgets[index1];
        for (int index2 = 0; childWidget.ChildWidgets.Count > index2; ++index2)
        {
          if (!childWidget.ChildWidgets[index2].Widget.LayoutInfo.IsKeepWithNext)
            return true;
        }
      }
      else if (widget is BlockContentControl)
      {
        if (this.IsNeedToRemoveItems(ltWidget.ChildWidgets[index1]))
          return true;
      }
      else
      {
        if (!(widget.LayoutInfo is ParagraphLayoutInfo) || !widget.LayoutInfo.IsKeepWithNext)
          return true;
        int num = 3;
        WParagraph wparagraph1;
        switch (widget)
        {
          case WParagraph _:
            wparagraph1 = widget as WParagraph;
            break;
          case SplitWidgetContainer _:
            wparagraph1 = (widget as SplitWidgetContainer).RealWidgetContainer is WParagraph ? (widget as SplitWidgetContainer).RealWidgetContainer as WParagraph : (WParagraph) null;
            break;
          default:
            wparagraph1 = (WParagraph) null;
            break;
        }
        WParagraph wparagraph2 = wparagraph1;
        if (wparagraph2 != null && !wparagraph2.ParagraphFormat.WidowControl)
          --num;
        if (wparagraph2 != null && ltWidget.ChildWidgets[index1].ChildWidgets.Count > num && !wparagraph2.ParagraphFormat.Keep)
          return true;
      }
    }
    return false;
  }

  private bool RemoveItemsFromltWidgets(
    ref IWidget splittedWidget,
    ref bool isLastTocParagraphRemoved,
    LayoutedWidget lwtWidget,
    bool isBlockContentControlChild,
    ref bool isWidgetsRemoved,
    ref int splittedWidgetIndex)
  {
    while (lwtWidget.ChildWidgets.Count > 0 && !(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter)
    {
      bool flag1 = false;
      IWidget widget = lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1].Widget;
      if (widget is WTable && !(widget as WTable).TableFormat.WrapTextAround && !(widget as WTable).IsInCell)
      {
        LayoutedWidget childWidget = lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1];
        int count = childWidget.ChildWidgets.Count;
        int num = childWidget.ChildWidgets.Count > 0 ? this.StartRowIndex(childWidget, ref count) : 0;
        while (childWidget.ChildWidgets.Count > 0 && childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1].Widget.LayoutInfo.IsKeepWithNext)
        {
          if (this.IsLastTOCParagraph(childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1].Widget as Entity))
            isLastTocParagraphRemoved = true;
          this.RemoveBehindWidgets(childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1]);
          childWidget.ChildWidgets.RemoveAt(childWidget.ChildWidgets.Count - 1);
          isWidgetsRemoved = true;
          flag1 = true;
          --count;
        }
        if (!flag1)
          return true;
        if (childWidget.ChildWidgets.Count > 0)
        {
          splittedWidget = (IWidget) new SplitTableWidget(childWidget.Widget as ITableWidget, num + count + 1);
          this.m_ltState = LayoutState.Splitted;
          if (isBlockContentControlChild)
            --splittedWidgetIndex;
          else
            --this.m_curWidgetIndex;
          return true;
        }
        this.RemoveBehindWidgets(lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1]);
        lwtWidget.ChildWidgets.RemoveAt(lwtWidget.ChildWidgets.Count - 1);
        bool flag2 = false;
        if (isBlockContentControlChild)
        {
          splittedWidget = widget;
          --splittedWidgetIndex;
        }
        else
        {
          while (!flag2)
          {
            --this.m_curWidgetIndex;
            if (this.CurrentChildWidget is WTable && (childWidget.Widget as WTable).Index == (this.CurrentChildWidget as WTable).Index || this.CurrentChildWidget is SplitTableWidget && (childWidget.Widget as WTable).Index == ((this.CurrentChildWidget as SplitTableWidget).TableWidget as WTable).Index)
              flag2 = true;
          }
        }
      }
      else if (widget is BlockContentControl)
      {
        LayoutedWidget childWidget = lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1];
        if (widget.LayoutInfo.IsKeepWithNext)
        {
          this.RemoveBehindWidgets(childWidget);
          lwtWidget.ChildWidgets.Remove(childWidget);
          isWidgetsRemoved = true;
          if (isBlockContentControlChild)
          {
            splittedWidget = widget;
            --splittedWidgetIndex;
          }
          else
            --this.m_curWidgetIndex;
        }
        else
        {
          int num = splittedWidgetIndex;
          bool flag3 = isWidgetsRemoved;
          splittedWidgetIndex = childWidget.ChildWidgets.Count;
          isWidgetsRemoved = false;
          bool flag4 = childWidget.ChildWidgets.Count <= 0 || this.RemoveItemsFromltWidgets(ref splittedWidget, ref isLastTocParagraphRemoved, childWidget, true, ref isWidgetsRemoved, ref splittedWidgetIndex);
          if (splittedWidget != null && splittedWidget is SplitWidgetContainer)
            --splittedWidgetIndex;
          if (splittedWidget != null && isWidgetsRemoved)
            splittedWidget = (IWidget) new SplitWidgetContainer(widget as IWidgetContainer, splittedWidget, splittedWidgetIndex);
          splittedWidgetIndex = num;
          if (!isBlockContentControlChild && isWidgetsRemoved)
            --this.m_curWidgetIndex;
          if (!isWidgetsRemoved)
            isWidgetsRemoved = flag3;
          if (flag4)
            return flag4;
        }
      }
      else
      {
        if (!(widget.LayoutInfo is ParagraphLayoutInfo) || !widget.LayoutInfo.IsKeepWithNext || widget is WParagraph && this.IsInFrame(widget as WParagraph) && (widget as WParagraph).ParagraphFormat != null && ((widget as WParagraph).ParagraphFormat.FrameVerticalPos == (byte) 1 || (widget as WParagraph).ParagraphFormat.FrameVerticalPos == (byte) 0))
          return true;
        LayoutedWidget childWidget = lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1];
        int num1 = 3;
        WParagraph wparagraph1;
        switch (widget)
        {
          case WParagraph _:
            wparagraph1 = widget as WParagraph;
            break;
          case SplitWidgetContainer _:
            wparagraph1 = (widget as SplitWidgetContainer).RealWidgetContainer is WParagraph ? (widget as SplitWidgetContainer).RealWidgetContainer as WParagraph : (WParagraph) null;
            break;
          default:
            wparagraph1 = (WParagraph) null;
            break;
        }
        WParagraph wparagraph2 = wparagraph1;
        if (wparagraph2 != null && !wparagraph2.ParagraphFormat.WidowControl)
          --num1;
        if (childWidget.ChildWidgets.Count <= num1 || wparagraph2 != null && wparagraph2.ParagraphFormat.Keep)
        {
          this.UpdateFootnoteWidgets(childWidget);
          if (DocumentLayouter.IsUpdatingTOC && this.IsLastTOCParagraph(lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1].Widget as Entity))
            isLastTocParagraphRemoved = true;
          this.RemoveBehindWidgets(lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1]);
          (this.m_lcOperator as Layouter).RemovedWidgetsHeight += lwtWidget.ChildWidgets[lwtWidget.ChildWidgets.Count - 1].Bounds.Height;
          if (isBlockContentControlChild)
            splittedWidget = childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1].Widget;
          lwtWidget.ChildWidgets.RemoveAt(lwtWidget.ChildWidgets.Count - 1);
          isWidgetsRemoved = true;
          bool flag5 = false;
          if (isBlockContentControlChild)
          {
            --splittedWidgetIndex;
          }
          else
          {
            while (!flag5)
            {
              --this.m_curWidgetIndex;
              if (this.CurrentChildWidget is WParagraph && (childWidget.Widget as WParagraph).Index == (this.CurrentChildWidget as WParagraph).Index || this.CurrentChildWidget is SplitWidgetContainer && ((childWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph).Index == ((childWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph).Index)
                flag5 = true;
            }
          }
        }
        else
        {
          int num2 = 0;
          while (true)
          {
            this.UpdateFootnoteWidgets(childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1]);
            childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1].InitLayoutInfo(false);
            ++num2;
            if (num2 != num1 - 1)
            {
              this.RemoveBehindWidgets(childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1]);
              childWidget.ChildWidgets.RemoveAt(childWidget.ChildWidgets.Count - 1);
              if (isBlockContentControlChild)
                --splittedWidgetIndex;
              else
                --this.m_curWidgetIndex;
            }
            else
              break;
          }
          splittedWidget = (IWidget) (childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1].Widget as SplitWidgetContainer);
          this.RemoveBehindWidgets(childWidget.ChildWidgets[childWidget.ChildWidgets.Count - 1]);
          childWidget.ChildWidgets.RemoveAt(childWidget.ChildWidgets.Count - 1);
          isWidgetsRemoved = true;
          if (num1 == 2 && !isBlockContentControlChild)
            --this.m_curWidgetIndex;
          else if (isBlockContentControlChild)
            --splittedWidgetIndex;
          return true;
        }
      }
    }
    return false;
  }

  private int StartRowIndex(LayoutedWidget tableWidget, ref int rowCount)
  {
    for (int index = 0; index < tableWidget.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = tableWidget.ChildWidgets[index];
      if (!(childWidget.Widget is WTableRow) || !(childWidget.Widget as WTableRow).IsHeader)
        return (tableWidget.ChildWidgets[index].Widget as WTableRow).Index;
      --rowCount;
    }
    return (tableWidget.ChildWidgets[0].Widget as WTableRow).Index;
  }

  private bool IsAllTextBodyItemHavingKeepWithNext()
  {
    for (int index = 0; index < this.m_ltWidget.ChildWidgets.Count; ++index)
    {
      if (!this.m_ltWidget.ChildWidgets[index].Widget.LayoutInfo.IsKeepWithNext)
        return false;
    }
    return true;
  }

  private bool IsNeedToCommitKeepWithNext()
  {
    bool commitKeepWithNext = false;
    if (this.m_ltWidget.ChildWidgets.Count > 0)
    {
      for (int index = this.m_ltWidget.ChildWidgets.Count - 1; index >= 0; --index)
      {
        IWidget widget = this.m_ltWidget.ChildWidgets[index].Widget;
        if (!(widget is WTable) || !(widget as WTable).TableFormat.WrapTextAround)
        {
          if (!widget.LayoutInfo.IsKeepWithNext)
          {
            if (widget is BlockContentControl)
            {
              bool isKeepWithNext = false;
              if (this.SetKeepWithNextForBlockContentControl(widget, ref isKeepWithNext))
              {
                widget.LayoutInfo.IsKeepWithNext = true;
              }
              else
              {
                commitKeepWithNext = true;
                break;
              }
            }
            else
            {
              commitKeepWithNext = true;
              break;
            }
          }
          if (this.IsForceFitLayout)
          {
            if (!DocumentLayouter.IsFirstLayouting)
            {
              commitKeepWithNext = widget.LayoutInfo.IsKeepWithNext;
              break;
            }
            break;
          }
        }
        else
        {
          commitKeepWithNext = true;
          break;
        }
      }
      WSection widget1 = this.m_ltWidget.Widget is WSection ? this.m_ltWidget.Widget as WSection : (WSection) null;
      if (widget1 != null && widget1.BreakCode == SectionBreakCode.NoBreak && widget1.GetIndexInOwnerCollection() > 0 && this.IsAllTextBodyItemHavingKeepWithNext())
        commitKeepWithNext = true;
    }
    return commitKeepWithNext;
  }

  internal bool SetKeepWithNextForBlockContentControl(IWidget widget, ref bool isKeepWithNext)
  {
    BlockContentControl blockContentControl = widget as BlockContentControl;
    for (int index = blockContentControl.ChildEntities.Count - 1; index >= 0; --index)
    {
      Entity childEntity = blockContentControl.ChildEntities[index];
      switch (childEntity)
      {
        case WParagraph _:
          if ((childEntity as WParagraph).m_layoutInfo.IsKeepWithNext)
          {
            isKeepWithNext = true;
            break;
          }
          isKeepWithNext = false;
          return false;
        case BlockContentControl _:
          IWidget widget1 = childEntity as IWidget;
          this.SetKeepWithNextForBlockContentControl(widget1, ref isKeepWithNext);
          if (!isKeepWithNext)
            return false;
          widget1.LayoutInfo.IsKeepWithNext = true;
          break;
        case WTable _:
          if ((childEntity as WTable).m_layoutInfo.IsKeepWithNext)
          {
            isKeepWithNext = true;
            break;
          }
          isKeepWithNext = false;
          return false;
        default:
          isKeepWithNext = false;
          return false;
      }
    }
    return isKeepWithNext;
  }
}
