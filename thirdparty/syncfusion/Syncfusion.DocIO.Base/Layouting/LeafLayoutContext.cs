// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LeafLayoutContext
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
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Layouting;

internal class LeafLayoutContext(
  ILeafWidget strWidget,
  ILCOperator lcOperator,
  bool isForceFitLayout) : LayoutContext((IWidget) strWidget, lcOperator, isForceFitLayout)
{
  private bool m_isXPositionUpdated;
  private bool m_isYPositionUpdated;
  private bool m_isWrapText;

  public ILeafWidget LeafWidget => this.m_widget as ILeafWidget;

  internal bool IsDecimaltabIsInCell(WParagraph paragraph, ILeafWidget leafWidget)
  {
    return paragraph != null && paragraph.IsInCell && paragraph.ParagraphFormat.Tabs.Count == 1 && paragraph.ParagraphFormat.Tabs[0].Justification == Syncfusion.DocIO.DLS.TabJustification.Decimal && leafWidget is WTextRange && (leafWidget as WTextRange).Text.Contains(".") && this.IsDecimalTabPoint(paragraph, leafWidget);
  }

  internal bool IsDecimalTabPoint(WParagraph paragraph, ILeafWidget leafWidget)
  {
    int num = -1;
    for (int index = 0; index < paragraph.Items.Count; ++index)
    {
      if (paragraph.Items[index] is WTextRange && (paragraph.Items[index] as WTextRange).Text.Contains(".") && num == -1)
      {
        num = (paragraph.Items[index] as WTextRange).Index;
        if (num == (leafWidget as WTextRange).Index)
          return true;
      }
      else if (paragraph.Items[index] is Break || paragraph.Items[index] is WTextRange && (paragraph.Items[index] as WTextRange).m_layoutInfo is TabsLayoutInfo)
        num = -1;
    }
    return false;
  }

  internal RectangleF NeedToUpdateClientArea(
    RectangleF rect,
    WParagraph paragraph,
    ILeafWidget leafWidget,
    Layouter layouter,
    ParagraphLayoutInfo paragraphLayoutInfo)
  {
    if (this.IsDecimaltabIsInCell(paragraph, leafWidget))
    {
      string text1 = paragraph.Text;
      int length = text1.IndexOf('.');
      string text2 = text1.Substring(0, length);
      string str = text1.Substring(length + 1).Split(' ')[0];
      float num1 = rect.Width - paragraph.ParagraphFormat.Tabs[0].Position;
      float width1 = this.DrawingContext.MeasureString(text2, (this.LeafWidget as WTextRange).CharacterFormat.GetFontToRender((this.LeafWidget as WTextRange).ScriptType), (StringFormat) null).Width;
      float width2 = this.DrawingContext.MeasureString("." + str, (this.LeafWidget as WTextRange).CharacterFormat.GetFontToRender((this.LeafWidget as WTextRange).ScriptType), (StringFormat) null).Width;
      float pageMarginLeft = this.GetPageMarginLeft();
      float num2 = paragraph.ParagraphFormat.Tabs[0].Position - (paragraphLayoutInfo == null || (double) paragraphLayoutInfo.XPosition == (double) pageMarginLeft ? 0.0f : paragraphLayoutInfo.XPosition - pageMarginLeft);
      WTableCell owner = paragraph.Owner as WTableCell;
      float num3 = 0.0f;
      if (owner != null)
        num3 = owner.GetRightPadding();
      if ((double) width1 <= (double) num2 - (double) num3 && (!layouter.IsFirstItemInLine || (double) rect.Right - ((double) pageMarginLeft + (double) paragraph.ParagraphFormat.Tabs[0].Position) - (double) num3 >= (double) width2))
        rect = new RectangleF(rect.X, rect.Y, num1 + width1, rect.Height);
    }
    else if ((double) (this.m_lcOperator as Layouter).m_effectiveJustifyWidth > 0.0)
      rect = new RectangleF(rect.X, rect.Y, rect.Width + (this.m_lcOperator as Layouter).m_effectiveJustifyWidth, rect.Height);
    return rect;
  }

  public override LayoutedWidget Layout(RectangleF rect)
  {
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    Layouter lcOperator = this.m_lcOperator as Layouter;
    ILeafWidget leafWidget1 = this.LeafWidget;
    ParagraphLayoutInfo paragraphLayoutInfo = (ParagraphLayoutInfo) null;
    float width1 = rect.Width;
    rect = this.NeedToUpdateClientArea(rect, ownerParagraph, leafWidget1, lcOperator, paragraphLayoutInfo);
    if (ownerParagraph != null)
      paragraphLayoutInfo = ((IWidget) ownerParagraph).LayoutInfo as ParagraphLayoutInfo;
    this.CreateLayoutArea(rect);
    float width2 = rect.Width;
    SizeF size = leafWidget1.Measure(this.DrawingContext);
    if (leafWidget1 is WField && (leafWidget1 as WField).FieldType == FieldType.FieldExpression && (double) size.Width > (double) this.m_layoutArea.ClientArea.Width)
      size = this.UpdateEQFieldWidth(this.DrawingContext, (leafWidget1 as WField).GetCharFormat());
    bool flag1 = lcOperator.m_canSplitbyCharacter;
    if (!lcOperator.IsLayoutingHeaderFooter && ownerParagraph != null && paragraphLayoutInfo.IsFirstItemInPage && (double) lcOperator.m_firstItemInPageYPosition == 0.0)
      lcOperator.m_firstItemInPageYPosition = rect.Y;
    float indentX = 0.0f;
    float indentY = 0.0f;
    if (this.IsFloatingItemLayouted(ownerParagraph))
      return this.m_ltWidget;
    if (leafWidget1 is WPicture && (leafWidget1 as WPicture).TextWrappingStyle == TextWrappingStyle.Inline)
    {
      if ((leafWidget1 as WPicture).HasBorder)
        this.UpdatePictureBorderSize(leafWidget1 as WPicture, ref size);
      if ((double) (leafWidget1 as WPicture).Rotation != 0.0 && !leafWidget1.LayoutInfo.IsVerticalText)
        this.GetPictureWrappingBounds(ref indentX, ref indentY, ref size, (leafWidget1 as WPicture).Rotation);
    }
    if (leafWidget1 is WTextRange && (leafWidget1 as WTextRange).Owner == null)
      size.Width = 0.0f;
    TabsLayoutInfo layoutInfo1 = leafWidget1.LayoutInfo as TabsLayoutInfo;
    WTextRange currTextRange = this.GetCurrTextRange();
    if (layoutInfo1 != null)
    {
      this.UpdateTabWidth(ref rect, ref size, currTextRange);
      if ((DocumentLayouter.IsUpdatingTOC || lcOperator.UpdatingPageFields) && currTextRange != null)
        currTextRange.Text = ControlChar.Tab;
      float pageMarginLeft = this.GetPageMarginLeft();
      if (!layoutInfo1.IsTabWidthUpdatedBasedOnIndent && (double) layoutInfo1.m_currTab.Position > 0.0 && (double) layoutInfo1.m_currTab.Position + (paragraphLayoutInfo == null || (double) paragraphLayoutInfo.XPosition == (double) pageMarginLeft ? (double) pageMarginLeft : (double) paragraphLayoutInfo.XPosition - (double) pageMarginLeft) > (double) this.GetPageMarginRight() - (double) paragraphLayoutInfo.Margins.Right && !this.IsTabStopBeyondRightMarginExists && (layoutInfo1.m_currTab.Justification != TabJustification.Decimal || !this.IsLeafWidgetIsInCell((ParagraphItem) currTextRange)) && (layoutInfo1.m_currTab.Justification != TabJustification.Decimal || !this.IsLeafWidgetIsInTextBox((ParagraphItem) currTextRange)) && (ownerParagraph != null ? (ownerParagraph.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 ? 1 : 0) : 0) != 0)
      {
        this.UpdateAreaMaxWidth();
        rect.Width = this.m_layoutArea.ClientArea.Width;
      }
      if (lcOperator.m_lineSpaceWidths != null)
        lcOperator.m_lineSpaceWidths.Clear();
    }
    RectangleF rectangleF = rect;
    bool flag2 = false;
    if (this.LeafWidget is Shape && (this.LeafWidget as Shape).IsHorizontalRule && (double) size.Width > (double) this.m_layoutArea.ClientActiveArea.Width)
      size.Width = this.m_layoutArea.ClientActiveArea.Width;
    if ((this.LeafWidget is WPicture || this.LeafWidget is Shape || this.LeafWidget is WTextBox || leafWidget1 is GroupShape || leafWidget1 is WChart) && (leafWidget1 as Entity).IsFloatingItem(false))
    {
      flag2 = true;
      this.GetFloattingItemPosition(ref indentX, ref indentY, ref size);
      if (leafWidget1 is WPicture && ((leafWidget1 as WPicture).TextWrappingStyle == TextWrappingStyle.Behind || (leafWidget1 as WPicture).TextWrappingStyle == TextWrappingStyle.InFrontOfText) && (double) this.DrawingContext.GetLineWidth(leafWidget1 as WPicture) > 0.0)
      {
        float lineWidth = this.DrawingContext.GetLineWidth(leafWidget1 as WPicture);
        indentX -= lineWidth;
        indentY -= lineWidth;
        size.Width += 2f * lineWidth;
        size.Height += 2f * lineWidth;
      }
      rect = new RectangleF(indentX, indentY, rect.Width - (indentX - rect.X), rect.Height - (indentY - rect.Y));
      this.CreateLayoutArea(rect);
    }
    if (leafWidget1 is WField && (leafWidget1 as WField).FieldType == FieldType.FieldPage)
      size = this.GetPageFieldSize(leafWidget1 as WField);
    if (!(leafWidget1 is WPicture) && !(leafWidget1 is WTextBox) && !(leafWidget1 is Shape) && !(leafWidget1 is GroupShape) && !(leafWidget1 is WChart) && !(leafWidget1 is WOleObject) || Math.Round((double) size.Height, 2) < Math.Round((double) this.m_layoutArea.ClientActiveArea.Height, 2) || !this.IsForceFitLayout || (this.LeafWidget as ParagraphItem).GetTextWrappingStyle() != TextWrappingStyle.Inline)
      this.AdjustClientAreaBasedOnTextWrap(leafWidget1, ref size, ref rect);
    bool flag3 = ((flag2 ? 1 : 0) | ((double) indentX != 0.0 ? 1 : ((double) indentY != 0.0 ? 1 : 0))) != 0;
    if (this.IsNeedToResetClientArea(ownerParagraph, leafWidget1, flag3))
    {
      indentX = rect.X;
      indentY = rect.Y;
      rect = rectangleF;
      this.CreateLayoutArea(rect);
    }
    if (currTextRange != null && (!(leafWidget1 is WField) || (leafWidget1 as WField).FieldType != FieldType.FieldExpression))
    {
      flag1 = (!lcOperator.m_canSplitByTab || !(currTextRange.m_layoutInfo is TabsLayoutInfo)) && lcOperator.m_canSplitbyCharacter;
      this.m_ltWidget = this.WordLayout(rect, size, currTextRange, ownerParagraph);
      if (this.m_ltWidget != null)
      {
        this.DoWord2013JustificationWordFit(ownerParagraph, width1, lcOperator);
        if (this.m_ltState == LayoutState.Splitted && this.m_isWrapText)
          this.m_ltState = LayoutState.WrapText;
        return this.m_ltWidget;
      }
      if (currTextRange.Text == '\u0003'.ToString())
        size.Width = (double) rect.Width >= 144.0 ? 144f : rect.Width;
      else if (currTextRange.Text == '\u0004'.ToString())
        size.Width = rect.Width;
    }
    float num = rect.X + size.Width;
    bool flag4 = true;
    if (!this.IsLeafWidgetNeedToBeSplitted(size, width2, rect) || layoutInfo1 != null && ((double) num <= (double) layoutInfo1.m_currTab.Position + layoutInfo1.PageMarginLeft && (double) size.Height <= (double) this.m_layoutArea.ClientArea.Height || currTextRange != null && currTextRange.Document.ActualFormatType == FormatType.Doc && layoutInfo1.m_list.Count > 0 && (double) layoutInfo1.m_list[layoutInfo1.m_list.Count - 1].Position > (double) this.ClientAreaRight(ownerParagraph, rect.Right) - (double) this.GetPageMarginLeft()))
    {
      this.FitWidget(size, (IWidget) leafWidget1, false, indentX, indentY, flag3);
      if (!(this.LayoutInfo is ParagraphLayoutInfo layoutInfo2) || !layoutInfo2.IsPageBreak)
        this.m_ltState = LayoutState.Fitted;
      else
        this.m_ltState = LayoutState.Breaked;
      if (this.LayoutInfo.IsPageBreakItem)
        this.m_ltState = LayoutState.Fitted;
    }
    else
    {
      this.IsTabStopBeyondRightMarginExists = false;
      ISplitLeafWidget leafWidget2 = this.LeafWidget as ISplitLeafWidget;
      bool flag5 = currTextRange != null && this.LeafWidget.LayoutInfo.IsClipped;
      float val2 = ownerParagraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.AtLeast ? ownerParagraph.ParagraphFormat.LineSpacing : 0.0f;
      if (leafWidget2 != null && ((double) Math.Max(size.Height, val2) <= (double) this.m_layoutArea.ClientArea.Height || currTextRange != null && ownerParagraph != null && ownerParagraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.Exactly && (double) Math.Abs(ownerParagraph.ParagraphFormat.LineSpacing) <= (double) this.m_layoutArea.ClientActiveArea.Height || flag5 && ((double) this.m_layoutArea.ClientArea.Height >= 0.0 || this.IsInFrame(ownerParagraph) && (double) this.m_layoutArea.ClientArea.Height >= 0.0) || this.IsForceFitLayout))
      {
        lcOperator.m_canSplitbyCharacter = flag1;
        this.SplitUpWidget(leafWidget2, width2);
      }
      else
      {
        this.m_ltState = LayoutState.NotFitted;
        if ((this.IsVerticalNotFitted = (double) size.Height > (double) this.m_layoutArea.ClientArea.Height) && lcOperator.IsRowFitInSamePage && ((double) this.m_layoutArea.ClientArea.Height > 0.0 || !this.IsWord2013(ownerParagraph.Document)))
        {
          this.FitWidget(size, (IWidget) leafWidget1, false, 0.0f, 0.0f, false);
          this.IsVerticalNotFitted = false;
          this.m_ltState = LayoutState.Fitted;
        }
        else
          flag4 = false;
      }
    }
    if (flag4)
      this.DoWord2013JustificationWordFit(ownerParagraph, width1, lcOperator);
    if (currTextRange != null && this.m_ltState == LayoutState.Splitted && this.m_isWrapText)
    {
      float width3 = size.Width;
      if (this.SplittedWidget is SplitStringWidget splittedWidget)
      {
        width3 = this.DrawingContext.MeasureTextRange(currTextRange, splittedWidget.SplittedText.Split(' ')[0]).Width;
        if (this.DrawingContext.IsUnicodeText(splittedWidget.SplittedText))
          width3 = this.DrawingContext.MeasureTextRange(currTextRange, splittedWidget.SplittedText[0].ToString()).Width;
      }
      else if (this.SplittedWidget is WTextRange && this.SplittedWidget.LayoutInfo is TabsLayoutInfo)
        width3 = size.Width;
      float right = lcOperator.ClientLayoutArea.Right;
      if (ownerParagraph != null && ownerParagraph.IsInCell)
        right = ((ownerParagraph.GetOwnerEntity() as WTableCell).m_layoutInfo as CellLayoutInfo).CellContentLayoutingBounds.Right;
      if ((double) this.m_ltWidget.Bounds.Right + (double) width3 + (double) paragraphLayoutInfo.Margins.Right < (double) right)
        this.m_ltState = LayoutState.WrapText;
    }
    this.DoLayoutAfter();
    if (this.m_ltWidget != null && (this.m_ltWidget.Widget is Shape || this.m_ltWidget.Widget is WTextBox || this.m_ltWidget.Widget is GroupShape))
    {
      if (this.m_ltWidget.Widget is WTextBox)
        this.LayoutTextBoxBody();
      else if (this.m_ltWidget.Widget is Shape)
        this.LayoutShapeTextBody();
      else
        this.LayoutGroupShapeTextBody();
    }
    this.HandleFloatingItemHaveCharacterOrigin(ownerParagraph);
    return this.m_ltWidget;
  }

  private void HandleFloatingItemHaveCharacterOrigin(WParagraph paragraph)
  {
    ParagraphLayoutInfo paragraphLayoutInfo = (ParagraphLayoutInfo) null;
    if (paragraph != null)
      paragraphLayoutInfo = ((IWidget) paragraph).LayoutInfo as ParagraphLayoutInfo;
    if (this.m_ltWidget == null || !this.IsDrawingElement(this.LeafWidget) || !(this.LeafWidget as ParagraphItem).IsFloatingItem(true) || (this.LeafWidget as ParagraphItem).GetHorizontalOrigin() != HorizontalOrigin.Character && (this.LeafWidget as ParagraphItem).GetVerticalOrigin() != VerticalOrigin.Line)
      return;
    if (paragraphLayoutInfo != null && (paragraphLayoutInfo.Justification == HAlignment.Left || (double) paragraphLayoutInfo.YPosition <= (double) this.m_ltWidget.Bounds.Y))
    {
      paragraph.IsFloatingItemsLayouted = true;
      this.AddToFloatingItems(this.m_ltWidget, this.LeafWidget);
      int i = (this.m_lcOperator as Layouter).FloatingItems.Count - 1;
      if (i < 0)
        return;
      this.IsDynamicRelayoutingOccur(paragraph, i);
    }
    else
      (this.m_lcOperator as Layouter).IsNeedToRelayout = true;
  }

  internal RectangleF FindWrappedPosition(SizeF widgetSize, RectangleF clientArea)
  {
    try
    {
      this.CreateLayoutArea(clientArea);
      this.AdjustClientAreaBasedOnTextWrap(this.LeafWidget, ref widgetSize, ref clientArea);
    }
    catch
    {
    }
    return clientArea;
  }

  private bool IsLineSpacingFitsWidget(WParagraph paragraph, float height)
  {
    if (paragraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.Exactly)
      return (double) Math.Abs(paragraph.ParagraphFormat.LineSpacing) <= (double) this.m_layoutArea.ClientActiveArea.Height;
    return paragraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.Multiple && (double) (paragraph.ParagraphFormat.LineSpacing / 12f) * (double) height <= (double) this.m_layoutArea.ClientActiveArea.Height;
  }

  private void UpdatePictureBorderSize(WPicture picture, ref SizeF size)
  {
    if (picture.IsShape)
    {
      float lineWidth1 = this.DrawingContext.GetLineWidth(picture.PictureShape.PictureDescriptor.BorderLeft);
      float lineWidth2 = this.DrawingContext.GetLineWidth(picture.PictureShape.PictureDescriptor.BorderRight);
      float lineWidth3 = this.DrawingContext.GetLineWidth(picture.PictureShape.PictureDescriptor.BorderTop);
      float lineWidth4 = this.DrawingContext.GetLineWidth(picture.PictureShape.PictureDescriptor.BorderBottom);
      size.Width += lineWidth1 + lineWidth2;
      size.Height += lineWidth3 + lineWidth4;
    }
    else
    {
      float lineWidth = this.DrawingContext.GetLineWidth(picture);
      size.Width += 2f * lineWidth;
      size.Height += 2f * lineWidth;
    }
  }

  private bool IsFloatingItemLayouted(WParagraph paragraph)
  {
    if ((this.LeafWidget is WPicture || this.LeafWidget is Shape || this.LeafWidget is WChart || this.LeafWidget is GroupShape || this.LeafWidget is WTextBox) && (this.LeafWidget as ParagraphItem).IsWrappingBoundsAdded())
    {
      int i = 0;
      if (this.IsFloatingItemExistInCollection(ref i))
      {
        this.CreateLayoutedWidget(i);
        this.IsDynamicRelayoutingOccur(paragraph, i);
        return true;
      }
    }
    return false;
  }

  private bool IsNeedToResetClientArea(
    WParagraph paragraph,
    ILeafWidget leafWidget,
    bool isFloating)
  {
    int num;
    if (!(this.LeafWidget is WPicture) && !(this.LeafWidget is Shape) && !(this.LeafWidget is WTextBox))
    {
      switch (leafWidget)
      {
        case GroupShape _:
        case WChart _:
          break;
        default:
          num = 0;
          goto label_4;
      }
    }
    num = (leafWidget as Entity).IsFloatingItem(true) ? 1 : 0;
label_4:
    bool flag = num != 0;
    ParagraphLayoutInfo layoutInfo = paragraph != null ? ((IWidget) paragraph).LayoutInfo as ParagraphLayoutInfo : (ParagraphLayoutInfo) null;
    return paragraph == null || layoutInfo == null || paragraph.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 || !paragraph.IsInCell || !flag ? isFloating : layoutInfo.IsFirstItemInPage;
  }

  private void IsDynamicRelayoutingOccur(WParagraph paragraph, int i)
  {
    WSection wsection = (WSection) null;
    if (paragraph != null)
      wsection = this.GetBaseEntity((Entity) paragraph) as WSection;
    ParagraphLayoutInfo paragraphLayoutInfo = (ParagraphLayoutInfo) null;
    if (paragraph != null)
      paragraphLayoutInfo = ((IWidget) paragraph).LayoutInfo as ParagraphLayoutInfo;
    Layouter lcOperator = this.m_lcOperator as Layouter;
    VerticalOrigin verticalOrigin = VerticalOrigin.Margin;
    if (lcOperator.FloatingItems.Count > i && lcOperator.FloatingItems[i].FloatingEntity is ParagraphItem)
      verticalOrigin = (lcOperator.FloatingItems[i].FloatingEntity as ParagraphItem).GetVerticalOrigin();
    bool flag = false;
    if (this.IsNeedToForceDynamicRelayout(paragraph, paragraphLayoutInfo, lcOperator.FloatingItems[i].TextWrappingStyle, lcOperator.FloatingItems[i].FloatingEntity))
      flag = true;
    if (paragraph != null && (!lcOperator.IsLayoutingHeaderFooter || this.IsFloatingItemLayoutInCell(paragraph, lcOperator.FloatingItems[i].TextWrappingStyle, lcOperator.FloatingItems[i].FloatingEntity)) && (((double) paragraphLayoutInfo.YPosition > (double) this.m_ltWidget.Bounds.Y || verticalOrigin == VerticalOrigin.Line && (double) this.m_layoutArea.ClientActiveArea.Y > (double) this.m_ltWidget.Bounds.Y || (double) paragraphLayoutInfo.XPosition > (double) this.m_ltWidget.Bounds.X && wsection != null && wsection.Columns.Count > 1 && lcOperator.CurrentColumnIndex != 0) && !paragraphLayoutInfo.IsFirstItemInPage || flag) && lcOperator.DynamicParagraph == null && !lcOperator.FloatingItems[i].IsFloatingItemFit && (double) lcOperator.m_firstItemInPageYPosition < (double) this.m_ltWidget.Bounds.Bottom)
    {
      if (lcOperator.MaintainltWidget.ChildWidgets.Count > 0)
        lcOperator.MaintainltWidget.ChildWidgets.RemoveRange(0, lcOperator.MaintainltWidget.ChildWidgets.Count);
      this.m_ltState = LayoutState.DynamicRelayout;
      if ((double) this.m_ltWidget.Bounds.Height > (double) lcOperator.CurrentSection.PageSetup.PageSize.Height || this.IsOwnerParaNotFittedInSamePage(lcOperator.FloatingItems[i].TextWrappingStyle, lcOperator.FloatingItems[i].FloatingEntity, lcOperator, this.m_ltWidget, paragraphLayoutInfo))
      {
        this.m_ltState = LayoutState.NotFitted;
        this.m_ltWidget = (LayoutedWidget) null;
      }
      FloatingItem floatingItem = lcOperator.FloatingItems[i];
      if (floatingItem.FloatingEntity is ParagraphItem && this.m_ltState == LayoutState.DynamicRelayout)
      {
        ParagraphItem floatingEntity = floatingItem.FloatingEntity as ParagraphItem;
        if ((floatingItem.FloatingEntity as ParagraphItem).GetVerticalOrigin() == VerticalOrigin.Paragraph && floatingEntity.IsWrappingBoundsAdded() && (double) floatingEntity.GetVerticalPosition() < 0.0)
          paragraphLayoutInfo.PargaraphOriginalYPosition = paragraphLayoutInfo.YPosition;
      }
      if (!flag)
        return;
      (this.m_lcOperator as Layouter).IsNeedToRelayoutTable = true;
    }
    else
      this.m_ltState = LayoutState.Fitted;
  }

  private bool IsFloatingItemExistInCollection(ref int i)
  {
    Layouter lcOperator = this.m_lcOperator as Layouter;
    i = 0;
    while (i < lcOperator.FloatingItems.Count)
    {
      if (lcOperator.FloatingItems[i].FloatingEntity == this.LeafWidget)
        return true;
      ++i;
    }
    return false;
  }

  private void CreateLayoutedWidget(int i)
  {
    this.m_ltWidget = new LayoutedWidget((IWidget) this.LeafWidget);
    Layouter lcOperator = this.m_lcOperator as Layouter;
    if (this.LeafWidget is WPicture)
    {
      WPicture leafWidget = this.LeafWidget as WPicture;
      this.SetBoundsForLayoutedWidget(lcOperator.FloatingItems[i].TextWrappingBounds, leafWidget.DistanceFromLeft, leafWidget.DistanceFromTop, leafWidget.DistanceFromRight, leafWidget.DistanceFromBottom);
    }
    else if (this.LeafWidget is Shape)
    {
      Shape leafWidget = this.LeafWidget as Shape;
      this.SetBoundsForLayoutedWidget(lcOperator.FloatingItems[i].TextWrappingBounds, leafWidget.WrapFormat.DistanceLeft, leafWidget.WrapFormat.DistanceTop, leafWidget.WrapFormat.DistanceRight, leafWidget.WrapFormat.DistanceBottom);
      if ((double) leafWidget.Rotation != 0.0)
      {
        RectangleF boundingBoxCoordinates = this.DrawingContext.GetBoundingBoxCoordinates(new RectangleF(0.0f, 0.0f, leafWidget.Width, leafWidget.Height), leafWidget.Rotation);
        SizeF sizeF = this.LeafWidget.Measure(this.DrawingContext);
        this.m_ltWidget.Bounds = new RectangleF(this.m_ltWidget.Bounds.X - boundingBoxCoordinates.X, this.m_ltWidget.Bounds.Y - boundingBoxCoordinates.Y, sizeF.Width, sizeF.Height);
      }
      this.LayoutShapeTextBody();
      if ((double) leafWidget.Rotation == 0.0)
        return;
      this.m_ltWidget.Bounds = this.DrawingContext.GetBoundingBoxCoordinates(this.m_ltWidget.Bounds, leafWidget.Rotation);
    }
    else if (this.LeafWidget is WTextBox)
    {
      WTextBoxFormat textBoxFormat = (this.LeafWidget as WTextBox).TextBoxFormat;
      this.SetBoundsForLayoutedWidget(lcOperator.FloatingItems[i].TextWrappingBounds, textBoxFormat.WrapDistanceLeft, textBoxFormat.WrapDistanceTop, textBoxFormat.WrapDistanceRight, textBoxFormat.WrapDistanceBottom);
      if ((double) textBoxFormat.Rotation != 0.0)
      {
        RectangleF boundingBoxCoordinates = this.DrawingContext.GetBoundingBoxCoordinates(new RectangleF(0.0f, 0.0f, textBoxFormat.Width, textBoxFormat.Height), textBoxFormat.Rotation);
        SizeF sizeF = this.LeafWidget.Measure(this.DrawingContext);
        this.m_ltWidget.Bounds = new RectangleF(this.m_ltWidget.Bounds.X - boundingBoxCoordinates.X, this.m_ltWidget.Bounds.Y - boundingBoxCoordinates.Y, sizeF.Width, sizeF.Height);
      }
      this.LayoutTextBoxBody();
      if ((double) textBoxFormat.Rotation == 0.0)
        return;
      this.m_ltWidget.Bounds = this.DrawingContext.GetBoundingBoxCoordinates(this.m_ltWidget.Bounds, textBoxFormat.Rotation);
    }
    else if (this.LeafWidget is GroupShape)
    {
      GroupShape leafWidget = this.LeafWidget as GroupShape;
      this.SetBoundsForLayoutedWidget(lcOperator.FloatingItems[i].TextWrappingBounds, leafWidget.WrapFormat.DistanceLeft, leafWidget.WrapFormat.DistanceTop, leafWidget.WrapFormat.DistanceRight, leafWidget.WrapFormat.DistanceBottom);
      if ((double) leafWidget.Rotation != 0.0)
      {
        RectangleF boundingBoxCoordinates = this.DrawingContext.GetBoundingBoxCoordinates(new RectangleF(0.0f, 0.0f, leafWidget.Width, leafWidget.Height), leafWidget.Rotation);
        this.m_ltWidget.Bounds = new RectangleF(this.m_ltWidget.Bounds.X - boundingBoxCoordinates.X, this.m_ltWidget.Bounds.Y - boundingBoxCoordinates.Y, leafWidget.Width, leafWidget.Height);
      }
      this.LayoutGroupShapeTextBody();
    }
    else
    {
      if (!(this.LeafWidget is WChart))
        return;
      WChart leafWidget = this.LeafWidget as WChart;
      this.SetBoundsForLayoutedWidget(lcOperator.FloatingItems[i].TextWrappingBounds, leafWidget.WrapFormat.DistanceLeft, leafWidget.WrapFormat.DistanceTop, leafWidget.WrapFormat.DistanceRight, leafWidget.WrapFormat.DistanceBottom);
    }
  }

  private void SetBoundsForLayoutedWidget(
    RectangleF textWrappingBounds,
    float distanceFromLeft,
    float distanceFromTop,
    float distanceFromRight,
    float distanceFromBottom)
  {
    this.m_ltWidget.Bounds = new RectangleF(textWrappingBounds.X + distanceFromLeft, textWrappingBounds.Y + distanceFromTop, textWrappingBounds.Width - (distanceFromRight + distanceFromLeft), textWrappingBounds.Height - (distanceFromBottom + distanceFromTop));
  }

  private void LayoutGroupShapeTextBody()
  {
    GroupShape widget = this.m_ltWidget.Widget as GroupShape;
    if (widget.Is2007Shape && !widget.IsFloatingItem(true) && !widget.HasChildGroupShape())
    {
      this.LayoutCustomChildShape((Entity) widget, this.m_ltWidget);
    }
    else
    {
      if (widget.GetTextWrappingStyle() == TextWrappingStyle.Inline)
      {
        if ((double) widget.Rotation != 0.0)
        {
          RectangleF boundingBoxCoordinates = this.DrawingContext.GetBoundingBoxCoordinates(new RectangleF(0.0f, 0.0f, this.m_ltWidget.Bounds.Width, this.m_ltWidget.Bounds.Height), widget.Rotation);
          this.m_ltWidget.Bounds = new RectangleF(this.m_ltWidget.Bounds.X - boundingBoxCoordinates.X, this.m_ltWidget.Bounds.Y - boundingBoxCoordinates.Y, this.m_ltWidget.Bounds.Width, this.m_ltWidget.Bounds.Height);
        }
        else
          this.m_ltWidget.Bounds = new RectangleF(this.m_ltWidget.Bounds.X + widget.LeftEdgeExtent, this.m_ltWidget.Bounds.Y + widget.TopEdgeExtent, this.m_ltWidget.Bounds.Width, this.m_ltWidget.Bounds.Height);
      }
      List<RectangleF> groupShapeBounds = new List<RectangleF>();
      List<float> groupShapeRotations = new List<float>();
      List<bool?> groupShapeHorzFlips = new List<bool?>();
      List<bool?> groupShapeVertFlips = new List<bool?>();
      groupShapeBounds.Add(this.m_ltWidget.Bounds);
      groupShapeRotations.Add(widget.Rotation);
      groupShapeHorzFlips.Add(widget.flipH);
      groupShapeVertFlips.Add(widget.flipV);
      this.LayoutChildGroupTextBody(widget.Rotation, (Entity) widget, this.m_ltWidget, 1f, 1f, groupShapeBounds, groupShapeRotations, groupShapeHorzFlips, groupShapeVertFlips);
      groupShapeBounds.Clear();
      groupShapeRotations.Clear();
      groupShapeHorzFlips.Clear();
      groupShapeVertFlips.Clear();
      if (!DocumentLayouter.IsUpdatingTOC)
        return;
      int num = DocumentLayouter.IsEndUpdateTOC ? 1 : 0;
    }
  }

  private void LayoutCustomChildShape(Entity entity, LayoutedWidget layoutedWidget)
  {
    RectangleF bounds = layoutedWidget.Bounds;
    GroupShape groupShape = (GroupShape) null;
    ChildGroupShape childGroupShape = (ChildGroupShape) null;
    if (entity is GroupShape)
      groupShape = entity as GroupShape;
    else
      childGroupShape = entity as ChildGroupShape;
    ChildShapeCollection childShapeCollection = groupShape != null ? groupShape.ChildShapes : childGroupShape.ChildShapes;
    float x1 = groupShape != null ? groupShape.CoordinateXOrigin : childGroupShape.CoordinateXOrigin;
    float y1 = groupShape != null ? groupShape.CoordinateYOrigin : childGroupShape.CoordinateYOrigin;
    string str = groupShape != null ? groupShape.CoordinateSize : childGroupShape.CoordinateSize;
    PointF pointF1 = new PointF(x1, y1);
    SizeF sizeF1 = new SizeF(1000f, 1000f);
    if (str != null)
    {
      string[] strArray = str.Split(new char[2]{ ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray.Length == 2)
      {
        float result;
        if (float.TryParse(strArray[0].Trim(), out result))
        {
          float num = float.Parse(strArray[0].Trim());
          if ((double) num > 0.0)
            sizeF1.Width = num;
        }
        if (float.TryParse(strArray[1].Trim(), out result))
        {
          float num = float.Parse(strArray[1].Trim());
          if ((double) num > 0.0)
            sizeF1.Height = num;
        }
      }
    }
    SizeF sizeF2 = new SizeF();
    sizeF2.Width = bounds.Width / sizeF1.Width;
    sizeF2.Height = bounds.Height / sizeF1.Height;
    PointF pointF2 = new PointF();
    pointF2.X = bounds.X - sizeF2.Width * pointF1.X;
    pointF2.Y = bounds.Y - sizeF2.Height * pointF1.Y;
    foreach (ChildShape childShape1 in (Syncfusion.DocIO.DLS.CollectionImpl) childShapeCollection)
    {
      if (childShape1 != null)
      {
        ChildShape childShape2 = childShape1;
        float leftMargin = childShape2.LeftMargin;
        float topMargin = childShape2.TopMargin;
        float width1 = childShape2.Width;
        float height1 = childShape2.Height;
        float num1 = leftMargin * sizeF2.Width;
        float num2 = topMargin * sizeF2.Height;
        float width2 = width1 * sizeF2.Width;
        float height2 = height1 * sizeF2.Height;
        float x2 = num1 + pointF2.X;
        float y2 = num2 + pointF2.Y;
        LayoutedWidget layoutedtWidget = new LayoutedWidget((IWidget) childShape2);
        layoutedtWidget.Bounds = new RectangleF(x2, y2, width2, height2);
        if (childShape1.VMLPathPoints != null && childShape1.VMLPathPoints.Count > 0)
        {
          childShape2.UpdateVMLPathPoints(layoutedtWidget.Bounds, childShape2.Path, new PointF(childShape2.CoordinateXOrigin, childShape2.CoordinateYOrigin), childShape2.CoordinateSize, childShape2.VMLPathPoints, childShape2.m_isVMLPathUpdated);
          childShape2.m_isVMLPathUpdated = true;
        }
        if (!childShape2.IsPicture)
          this.LayoutChildShapeTextBody(layoutedtWidget);
        layoutedWidget.ChildWidgets.Add(layoutedtWidget);
        layoutedtWidget.Owner = layoutedWidget;
      }
    }
  }

  private RectangleF GetChildShapePositionToDraw(
    RectangleF groupShapeBounds,
    float groupShapeRotation,
    RectangleF childShapeBounds)
  {
    double num1 = (double) groupShapeBounds.X + (double) groupShapeBounds.Width / 2.0;
    double num2 = (double) groupShapeBounds.Y + (double) groupShapeBounds.Height / 2.0;
    if ((double) groupShapeRotation > 360.0)
      groupShapeRotation %= 360f;
    double num3 = (double) groupShapeRotation * Math.PI / 180.0;
    double num4 = Math.Sin(num3);
    double num5 = Math.Cos(num3);
    double num6 = (double) childShapeBounds.X + (double) childShapeBounds.Width / 2.0;
    double num7 = (double) childShapeBounds.Y + (double) childShapeBounds.Height / 2.0;
    double num8 = num1 + ((double) childShapeBounds.X - num1) * num5 - ((double) childShapeBounds.Y - num2) * num4;
    double num9 = num2 + ((double) childShapeBounds.X - num1) * num4 + ((double) childShapeBounds.Y - num2) * num5;
    double num10 = num1 + (num6 - num1) * num5 - (num7 - num2) * num4;
    double num11 = num2 + (num6 - num1) * num4 + (num7 - num2) * num5;
    double num12 = (360.0 - (double) groupShapeRotation) * Math.PI / 180.0;
    double num13 = Math.Sin(num12);
    double num14 = Math.Cos(num12);
    return new RectangleF((float) (num10 + (num8 - num10) * num14 - (num9 - num11) * num13), (float) (num11 + (num8 - num10) * num13 + (num9 - num11) * num14), childShapeBounds.Width, childShapeBounds.Height);
  }

  private void LayoutChildShapeTextBody(LayoutedWidget layoutedtWidget)
  {
    ChildShape widget = layoutedtWidget.Widget as ChildShape;
    RectangleF layoutRect = widget.AutoShapeType == AutoShapeType.Rectangle ? layoutedtWidget.Bounds : this.GetBoundsToLayoutShapeTextBody(widget.AutoShapeType, widget.ShapeGuide, layoutedtWidget.Bounds);
    if ((widget.AutoShapeType == AutoShapeType.Rectangle || widget.IsTextBoxShape) && (widget.TextFrame.TextDirection == TextDirection.Horizontal || widget.TextFrame.TextDirection == TextDirection.HorizontalFarEast) && (double) layoutedtWidget.Bounds.Height < (double) this.m_layoutArea.ClientActiveArea.Height)
      layoutRect.Height = this.m_layoutArea.ClientActiveArea.Height;
    this.UpdateChildShapeBoundsToLayoutTextBody(ref layoutRect, widget.TextFrame.InternalMargin, layoutedtWidget);
    bool isNeedToUpdateWidth = false;
    bool isAutoFit = this.IsAutoFit(ref isNeedToUpdateWidth, layoutedtWidget.Widget);
    if ((widget.AutoShapeType == AutoShapeType.Rectangle || widget.IsTextBoxShape) && (isAutoFit || isNeedToUpdateWidth))
      layoutRect = this.UpdateAutoFitLayoutingBounds(layoutRect, !((IWidget) widget).LayoutInfo.IsVerticalText, isAutoFit, isNeedToUpdateWidth);
    if (((IWidget) widget).LayoutInfo.IsVerticalText)
      layoutRect = new RectangleF(layoutRect.X, layoutRect.Y, layoutRect.Height, layoutRect.Width);
    widget.TextLayoutingBounds = layoutRect;
    float paragraphYposition = (this.m_lcOperator as Layouter).ParagraphYPosition;
    LayoutedWidget layoutedWidget = LayoutContext.Create((IWidget) widget.TextBody, this.m_lcOperator, this.IsForceFitLayout).Layout(layoutRect);
    (this.m_lcOperator as Layouter).ParagraphYPosition = paragraphYposition;
    if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
      return;
    if ((widget.AutoShapeType == AutoShapeType.Rectangle || widget.IsTextBoxShape) && (isAutoFit || isNeedToUpdateWidth))
      this.UpdateAutoFitRenderingBounds(layoutedtWidget, layoutedWidget, !((IWidget) widget).LayoutInfo.IsVerticalText, isNeedToUpdateWidth, isAutoFit, widget.TextFrame.InternalMargin);
    if ((widget.AutoShapeType == AutoShapeType.Rectangle || widget.IsTextBoxShape) && (widget.TextFrame.TextDirection == TextDirection.Horizontal || widget.TextFrame.TextDirection == TextDirection.HorizontalFarEast) && (double) layoutedtWidget.Bounds.Height < (double) this.m_layoutArea.ClientActiveArea.Height)
      layoutRect.Height = layoutedtWidget.Bounds.Height - (widget.TextFrame.InternalMargin.Top + widget.TextFrame.InternalMargin.Bottom);
    this.UpdateLayoutedWidgetBasedOnVerticalAlignment(layoutRect, layoutedWidget, widget.TextFrame.TextVerticalAlignment);
    layoutedtWidget.ChildWidgets.Add(layoutedWidget);
    layoutedWidget.Owner = layoutedtWidget;
  }

  private void LayoutChildGroupTextBody(
    float groupShapeRotation,
    Entity entity,
    LayoutedWidget layoutedWidget,
    float extensionWidth,
    float extensionHeight,
    List<RectangleF> groupShapeBounds,
    List<float> groupShapeRotations,
    List<bool?> groupShapeHorzFlips,
    List<bool?> groupShapeVertFlips)
  {
    RectangleF bounds = layoutedWidget.Bounds;
    ChildShapeCollection childShapeCollection = entity is GroupShape ? (entity as GroupShape).ChildShapes : (entity as ChildGroupShape).ChildShapes;
    float num1 = entity is GroupShape ? (entity as GroupShape).CoordinateXOrigin : (entity as ChildGroupShape).CoordinateXOrigin;
    float num2 = entity is GroupShape ? (entity as GroupShape).CoordinateYOrigin : (entity as ChildGroupShape).CoordinateYOrigin;
    float num3 = entity is GroupShape ? (entity as GroupShape).X : (entity as ChildGroupShape).X;
    float num4 = entity is GroupShape ? (entity as GroupShape).Y : (entity as ChildGroupShape).Y;
    foreach (ChildShape childShape1 in (Syncfusion.DocIO.DLS.CollectionImpl) childShapeCollection)
    {
      layoutedWidget.GetGroupShapeExtent(ref extensionWidth, ref extensionHeight, entity, bounds);
      if (childShape1 is ChildGroupShape)
      {
        ChildGroupShape childGroupShape = childShape1 as ChildGroupShape;
        float num5;
        float num6;
        if (childGroupShape.Is2007Shape)
        {
          num5 = childGroupShape.LeftMargin - num1;
          num6 = childGroupShape.TopMargin - num2;
        }
        else
        {
          num5 = childGroupShape.OffsetXValue - num3;
          num6 = childGroupShape.OffsetYValue - num4;
        }
        float rotation = childGroupShape.Rotation;
        groupShapeRotations.Add(rotation);
        groupShapeHorzFlips.Add(childGroupShape.flipH);
        groupShapeVertFlips.Add(childGroupShape.flipV);
        float x = num5 * extensionWidth + bounds.X;
        float y = num6 * extensionHeight + bounds.Y;
        float width = childGroupShape.Width;
        float height = childGroupShape.Height;
        if ((double) rotation >= 45.0 && (double) rotation < 135.0 || (double) rotation >= 225.0 && (double) rotation < 315.0)
        {
          float num7 = extensionWidth;
          extensionWidth = extensionHeight;
          extensionHeight = num7;
          x -= (float) ((double) width * ((double) extensionWidth - (double) extensionHeight) / 2.0);
          y -= (float) ((double) height * ((double) extensionHeight - (double) extensionWidth) / 2.0);
        }
        LayoutedWidget layoutedWidget1 = new LayoutedWidget((IWidget) childGroupShape);
        layoutedWidget1.Bounds = new RectangleF(x, y, width * extensionWidth, height * extensionHeight);
        groupShapeBounds.Add(layoutedWidget1.Bounds);
        this.LayoutChildGroupTextBody(rotation, (Entity) childGroupShape, layoutedWidget1, extensionWidth, extensionHeight, groupShapeBounds, groupShapeRotations, groupShapeHorzFlips, groupShapeVertFlips);
        layoutedWidget.ChildWidgets.Add(layoutedWidget1);
        layoutedWidget1.Owner = layoutedWidget;
      }
      else if (childShape1 != null)
      {
        ChildShape childShape2 = childShape1;
        float num8;
        float num9;
        if (childShape2.Is2007Shape)
        {
          num8 = childShape2.LeftMargin - num1;
          num9 = childShape2.TopMargin - num2;
        }
        else
        {
          num8 = childShape2.X - num3;
          num9 = childShape2.Y - num4;
        }
        float rotation = childShape2.Rotation;
        float x = num8 * extensionWidth + bounds.X;
        float y = num9 * extensionHeight + bounds.Y;
        float width = childShape2.Width;
        float height = childShape2.Height;
        if ((double) rotation >= 45.0 && (double) rotation < 135.0 || (double) rotation >= 225.0 && (double) rotation < 315.0)
        {
          float num10 = extensionWidth;
          extensionWidth = extensionHeight;
          extensionHeight = num10;
          x -= (float) ((double) width * ((double) extensionWidth - (double) extensionHeight) / 2.0);
          y -= (float) ((double) height * ((double) extensionHeight - (double) extensionWidth) / 2.0);
        }
        LayoutedWidget layoutedtWidget = new LayoutedWidget((IWidget) childShape2);
        layoutedtWidget.Bounds = new RectangleF(x, y, width * extensionWidth, height * extensionHeight);
        float num11 = 0.0f;
        bool flag1 = false;
        bool flag2 = false;
        for (int index = 0; index <= groupShapeRotations.Count; ++index)
        {
          float num12 = index != groupShapeRotations.Count ? groupShapeRotations[index] : rotation;
          if (index > 0)
          {
            if (groupShapeHorzFlips[index - 1].HasValue)
              flag1 ^= groupShapeHorzFlips[index - 1].Value;
            if (groupShapeVertFlips[index - 1].HasValue)
              flag2 ^= groupShapeVertFlips[index - 1].Value;
          }
          if (flag1 ^ flag2)
            num11 -= num12;
          else
            num11 += num12;
        }
        float num13 = num11 % 360f;
        for (int index = groupShapeBounds.Count - 1; index >= 0; --index)
        {
          bool flag3 = false;
          bool flag4 = false;
          float groupShapeRotation1 = groupShapeRotations[index];
          RectangleF groupShapeBound = groupShapeBounds[index];
          if (groupShapeHorzFlips[index].HasValue)
            flag3 = groupShapeHorzFlips[index].Value;
          if (groupShapeVertFlips[index].HasValue)
            flag4 = groupShapeVertFlips[index].Value;
          if (flag3 || flag4)
          {
            PointF[] pointFArray = new PointF[4]
            {
              new PointF(layoutedtWidget.Bounds.X, layoutedtWidget.Bounds.Y),
              new PointF(layoutedtWidget.Bounds.X + layoutedtWidget.Bounds.Width, layoutedtWidget.Bounds.Y),
              new PointF(layoutedtWidget.Bounds.Right, layoutedtWidget.Bounds.Bottom),
              new PointF(layoutedtWidget.Bounds.X, layoutedtWidget.Bounds.Y + layoutedtWidget.Bounds.Height)
            };
            Matrix matrix1 = new Matrix();
            PointF pointF = new PointF(groupShapeBound.X + groupShapeBound.Width / 2f, groupShapeBound.Y + groupShapeBound.Height / 2f);
            Matrix matrix2 = new Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, 0.0f);
            Matrix matrix3 = new Matrix(-1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
            if (flag4)
            {
              matrix1.Multiply(matrix2, MatrixOrder.Append);
              matrix1.Translate(0.0f, pointF.Y * 2f, MatrixOrder.Append);
            }
            if (flag3)
            {
              matrix1.Multiply(matrix3, MatrixOrder.Append);
              matrix1.Translate(pointF.X * 2f, 0.0f, MatrixOrder.Append);
            }
            matrix1.TransformPoints(pointFArray);
            layoutedtWidget.Bounds = LeafLayoutContext.CreateRect(pointFArray);
          }
          if ((double) groupShapeRotation1 != 0.0)
            layoutedtWidget.Bounds = this.GetChildShapePositionToDraw(groupShapeBound, groupShapeRotation1, layoutedtWidget.Bounds);
        }
        if (this.IsGroupFlipV(childShape2.Owner) || this.IsGroupFlipH(childShape2.Owner))
        {
          int flipHcount = this.GetFlipHCount(childShape2.Owner, childShape2.FlipHorizantal ? 1 : 0);
          int flipVcount = this.GetFlipVCount(childShape2.Owner, childShape2.FlipVertical ? 1 : 0);
          bool flag5 = flipHcount % 2 != 0;
          bool flag6 = flipVcount % 2 != 0;
          childShape2.FlipVerticalToRender = flag6;
          childShape2.FlipHorizantalToRender = flag5;
          childShape2.RotationToRender = num13;
        }
        else
        {
          childShape2.FlipVerticalToRender = childShape2.FlipVertical;
          childShape2.FlipHorizantalToRender = childShape2.FlipHorizantal;
          childShape2.RotationToRender = num13;
        }
        if (!childShape2.IsPicture)
          this.LayoutChildShapeTextBody(layoutedtWidget);
        layoutedWidget.ChildWidgets.Add(layoutedtWidget);
        layoutedtWidget.Owner = layoutedWidget;
      }
    }
    if (groupShapeRotations.Count > 0)
      groupShapeRotations.RemoveAt(groupShapeRotations.Count - 1);
    if (groupShapeBounds.Count > 0)
      groupShapeBounds.RemoveAt(groupShapeBounds.Count - 1);
    if (groupShapeHorzFlips.Count > 0)
      groupShapeHorzFlips.RemoveAt(groupShapeHorzFlips.Count - 1);
    if (groupShapeVertFlips.Count <= 0)
      return;
    groupShapeVertFlips.RemoveAt(groupShapeVertFlips.Count - 1);
  }

  internal int GetFlipHCount(Entity entity, int count)
  {
    while (true)
    {
      switch (entity)
      {
        case GroupShape _:
        case ChildGroupShape _:
          if (entity is GroupShape && (entity as GroupShape).FlipHorizontal || entity is ChildGroupShape && (entity as ChildGroupShape).FlipHorizantal)
            ++count;
          entity = entity is GroupShape ? (entity as GroupShape).Owner : (entity as ChildGroupShape).Owner;
          continue;
        default:
          goto label_5;
      }
    }
label_5:
    return count;
  }

  internal int GetFlipVCount(Entity entity, int count)
  {
    while (true)
    {
      switch (entity)
      {
        case GroupShape _:
        case ChildGroupShape _:
          if (entity is GroupShape && (entity as GroupShape).FlipVertical || entity is ChildGroupShape && (entity as ChildGroupShape).FlipVertical)
            ++count;
          entity = entity is GroupShape ? (entity as GroupShape).Owner : (entity as ChildGroupShape).Owner;
          continue;
        default:
          goto label_5;
      }
    }
label_5:
    return count;
  }

  internal bool IsGroupFlipH(Entity entity)
  {
    while (true)
    {
      switch (entity)
      {
        case GroupShape _:
        case ChildGroupShape _:
          Entity owner;
          switch (entity)
          {
            case GroupShape _ when (entity as GroupShape).FlipHorizontal:
            case ChildGroupShape _ when (entity as ChildGroupShape).FlipHorizantal:
              goto label_2;
            case GroupShape _:
              owner = (entity as GroupShape).Owner;
              break;
            default:
              owner = (entity as ChildGroupShape).Owner;
              break;
          }
          entity = owner;
          continue;
        default:
          goto label_7;
      }
    }
label_2:
    return true;
label_7:
    return false;
  }

  internal bool IsGroupFlipV(Entity entity)
  {
    while (true)
    {
      switch (entity)
      {
        case GroupShape _:
        case ChildGroupShape _:
          Entity owner;
          switch (entity)
          {
            case GroupShape _ when (entity as GroupShape).FlipVertical:
            case ChildGroupShape _ when (entity as ChildGroupShape).FlipVertical:
              goto label_2;
            case GroupShape _:
              owner = (entity as GroupShape).Owner;
              break;
            default:
              owner = (entity as ChildGroupShape).Owner;
              break;
          }
          entity = owner;
          continue;
        default:
          goto label_7;
      }
    }
label_2:
    return true;
label_7:
    return false;
  }

  private static RectangleF CreateRect(PointF[] points)
  {
    float x1 = float.MaxValue;
    float y1 = float.MaxValue;
    float num1 = float.MinValue;
    float num2 = float.MinValue;
    int length = points.Length;
    for (int index = 0; index < length; ++index)
    {
      float x2 = points[index].X;
      float y2 = points[index].Y;
      if ((double) x2 < (double) x1)
        x1 = x2;
      if ((double) x2 > (double) num1)
        num1 = x2;
      if ((double) y2 < (double) y1)
        y1 = y2;
      if ((double) y2 > (double) num2)
        num2 = y2;
    }
    return new RectangleF(x1, y1, num1 - x1, num2 - y1);
  }

  private void LayoutShapeTextBody()
  {
    Shape widget = this.m_ltWidget.Widget as Shape;
    if (widget.Is2007Shape && widget.VMLPathPoints != null && widget.VMLPathPoints.Count > 0 && !widget.IsFloatingItem(true))
    {
      widget.UpdateVMLPathPoints(this.m_ltWidget.Bounds, widget.Path, new PointF(widget.CoordinateXOrigin, widget.CoordinateYOrigin), widget.CoordinateSize, widget.VMLPathPoints, widget.m_isVMLPathUpdated);
      widget.m_isVMLPathUpdated = true;
    }
    else
      widget.VMLPathPoints = (List<Path2D>) null;
    if (widget.GetTextWrappingStyle() == TextWrappingStyle.Inline && (double) widget.Rotation == 0.0)
      this.m_ltWidget.Bounds = new RectangleF(this.m_ltWidget.Bounds.X + widget.LeftEdgeExtent, this.m_ltWidget.Bounds.Y + widget.TopEdgeExtent, this.m_ltWidget.Bounds.Width, this.m_ltWidget.Bounds.Height);
    RectangleF layoutRect = this.GetBoundsToLayoutShapeTextBody(widget.AutoShapeType, widget.ShapeGuide, this.m_ltWidget.Bounds);
    this.UpdateShapeBoundsToLayoutTextBody(ref layoutRect, widget.TextFrame.InternalMargin);
    bool isNeedToUpdateWidth = false;
    bool isAutoFit = this.IsAutoFit(ref isNeedToUpdateWidth, this.m_ltWidget.Widget);
    if (widget.AutoShapeType == AutoShapeType.Rectangle && (isAutoFit || isNeedToUpdateWidth))
      layoutRect = this.UpdateAutoFitLayoutingBounds(layoutRect, !((IWidget) widget).LayoutInfo.IsVerticalText, isAutoFit, isNeedToUpdateWidth);
    if (((IWidget) widget).LayoutInfo.IsVerticalText)
      layoutRect = new RectangleF(layoutRect.X, layoutRect.Y, layoutRect.Height, layoutRect.Width);
    widget.TextLayoutingBounds = layoutRect;
    RectangleF rectangleF1 = this.m_ltWidget.Bounds;
    if ((double) widget.Rotation != 0.0 && !widget.IsWrappingBoundsAdded())
      rectangleF1 = this.DrawingContext.GetBoundingBoxCoordinates(this.m_ltWidget.Bounds, widget.Rotation);
    if ((double) widget.Rotation != 0.0 && widget.VerticalAlignment == ShapeVerticalAlignment.Top && (widget.VerticalOrigin == VerticalOrigin.Margin || widget.VerticalOrigin == VerticalOrigin.BottomMargin) && (double) widget.VerticalPosition == 0.0)
      rectangleF1.Y = this.m_ltWidget.Bounds.Y;
    RectangleF rectangleF2 = layoutRect;
    if ((double) widget.Rotation != 0.0 && widget.TextFrame.Upright)
      layoutRect = this.DrawingContext.GetBoundingBoxCoordinates(layoutRect, widget.Rotation);
    if ((double) widget.Rotation != 0.0 && widget.WrapFormat.TextWrappingStyle == TextWrappingStyle.Inline)
    {
      RectangleF rectangleF3 = rectangleF1;
      rectangleF1.X = this.m_ltWidget.Bounds.X;
      rectangleF1.Y = this.m_ltWidget.Bounds.Y;
      if (widget.TextFrame.Upright)
      {
        float num1 = layoutRect.X - rectangleF3.X;
        float num2 = layoutRect.Y - rectangleF3.Y;
        layoutRect.X = this.m_ltWidget.Bounds.X + num1;
        layoutRect.Y = this.m_ltWidget.Bounds.Y + num2;
        widget.TextLayoutingBounds = new RectangleF(layoutRect.X, layoutRect.Y, widget.TextLayoutingBounds.Height, widget.TextLayoutingBounds.Width);
      }
      else
      {
        layoutRect.X = rectangleF2.X;
        layoutRect.Y = rectangleF2.Y;
      }
    }
    bool splitbyCharacter = (this.m_lcOperator as Layouter).m_canSplitbyCharacter;
    bool mCanSplitByTab = (this.m_lcOperator as Layouter).m_canSplitByTab;
    bool isFirstItemInLine = (this.m_lcOperator as Layouter).IsFirstItemInLine;
    List<float> lineSpaceWidths = (this.m_lcOperator as Layouter).m_lineSpaceWidths;
    float effectiveJustifyWidth = (this.m_lcOperator as Layouter).m_effectiveJustifyWidth;
    LayoutContext layoutContext = LayoutContext.Create((IWidget) widget.TextBody, this.m_lcOperator, this.IsForceFitLayout);
    float paragraphYposition = (this.m_lcOperator as Layouter).ParagraphYPosition;
    LayoutedWidget layoutedWidget = layoutContext.Layout(layoutRect);
    (this.m_lcOperator as Layouter).ParagraphYPosition = paragraphYposition;
    if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
      return;
    if (widget.AutoShapeType == AutoShapeType.Rectangle && (isAutoFit || isNeedToUpdateWidth))
      this.UpdateAutoFitRenderingBounds(this.m_ltWidget, layoutedWidget, !((IWidget) widget).LayoutInfo.IsVerticalText, isNeedToUpdateWidth, isAutoFit, widget.TextFrame.InternalMargin);
    this.UpdateLayoutedWidgetBasedOnVerticalAlignment(layoutRect, layoutedWidget, widget.TextFrame.TextVerticalAlignment);
    this.m_ltWidget.ChildWidgets.Add(layoutedWidget);
    layoutedWidget.Owner = this.m_ltWidget;
    if ((double) widget.Rotation != 0.0 && !widget.IsWrappingBoundsAdded())
      this.m_ltWidget.Bounds = rectangleF1;
    (this.m_lcOperator as Layouter).ResetWordLayoutingFlags(splitbyCharacter, mCanSplitByTab, isFirstItemInLine, lineSpaceWidths, effectiveJustifyWidth);
  }

  private void LayoutTextBoxBody()
  {
    WTextBox widget = this.m_ltWidget.Widget as WTextBox;
    if (widget.TextBoxFormat.VMLPathPoints != null && widget.TextBoxFormat.VMLPathPoints.Count > 0 && !widget.IsFloatingItem(true))
    {
      widget.UpdateVMLPathPoints(this.m_ltWidget.Bounds, widget.TextBoxFormat.Path, new PointF(widget.TextBoxFormat.CoordinateXOrigin, widget.TextBoxFormat.CoordinateYOrigin), widget.TextBoxFormat.CoordinateSize, widget.TextBoxFormat.VMLPathPoints, widget.TextBoxFormat.m_isVMLPathUpdated);
      widget.TextBoxFormat.m_isVMLPathUpdated = true;
    }
    else
      widget.TextBoxFormat.VMLPathPoints = (List<Path2D>) null;
    WTextBoxFormat textBoxFormat = widget.TextBoxFormat;
    if (widget.GetTextWrappingStyle() == TextWrappingStyle.Inline && (double) textBoxFormat.Rotation == 0.0 && widget.Shape != null)
      this.m_ltWidget.Bounds = new RectangleF(this.m_ltWidget.Bounds.X + widget.Shape.LeftEdgeExtent, this.m_ltWidget.Bounds.Y + widget.Shape.TopEdgeExtent, this.m_ltWidget.Bounds.Width, this.m_ltWidget.Bounds.Height);
    RectangleF rectangleF1 = this.m_ltWidget.Bounds;
    if ((double) textBoxFormat.Rotation != 0.0 && !widget.IsWrappingBoundsAdded())
      rectangleF1 = this.DrawingContext.GetBoundingBoxCoordinates(this.m_ltWidget.Bounds, textBoxFormat.Rotation);
    if ((double) textBoxFormat.Rotation != 0.0 && textBoxFormat.VerticalAlignment == ShapeVerticalAlignment.Top && (textBoxFormat.VerticalOrigin == VerticalOrigin.Margin || textBoxFormat.VerticalOrigin == VerticalOrigin.BottomMargin) && (double) textBoxFormat.VerticalPosition == 0.0)
      rectangleF1.Y = this.m_ltWidget.Bounds.Y;
    if ((double) textBoxFormat.Rotation != 0.0 && textBoxFormat.TextWrappingStyle == TextWrappingStyle.Inline)
    {
      rectangleF1.X = this.m_ltWidget.Bounds.X;
      rectangleF1.Y = this.m_ltWidget.Bounds.Y;
    }
    RectangleF rectangleF2 = this.m_ltWidget.Bounds;
    if ((double) textBoxFormat.Rotation != 0.0 && widget.Shape != null && widget.Shape.TextFrame.Upright)
      rectangleF2 = rectangleF1;
    if ((textBoxFormat.TextDirection == TextDirection.Horizontal || textBoxFormat.TextDirection == TextDirection.HorizontalFarEast) && (double) this.m_ltWidget.Bounds.Height < (double) this.m_layoutArea.ClientActiveArea.Height)
      rectangleF2.Height = this.m_layoutArea.ClientActiveArea.Height;
    bool isHorizontalText = !((IWidget) widget).LayoutInfo.IsVerticalText;
    bool isNeedToUpdateWidth = false;
    bool isAutoFit = this.IsAutoFit(ref isNeedToUpdateWidth, this.m_ltWidget.Widget);
    if (isAutoFit || isNeedToUpdateWidth)
      rectangleF2 = this.UpdateAutoFitLayoutingBounds(rectangleF2, isHorizontalText, isAutoFit, isNeedToUpdateWidth);
    if (widget.IsNoNeedToConsiderLineWidth())
    {
      rectangleF2.X += textBoxFormat.InternalMargin.Left;
      rectangleF2.Y += textBoxFormat.InternalMargin.Top;
      rectangleF2.Width -= textBoxFormat.InternalMargin.Left + textBoxFormat.InternalMargin.Right;
      rectangleF2.Height -= textBoxFormat.InternalMargin.Top + textBoxFormat.InternalMargin.Bottom;
    }
    else
    {
      rectangleF2.X += textBoxFormat.InternalMargin.Left + textBoxFormat.LineWidth / 2f;
      rectangleF2.Y += textBoxFormat.InternalMargin.Top + textBoxFormat.LineWidth / 2f;
      rectangleF2.Width -= textBoxFormat.LineWidth + textBoxFormat.InternalMargin.Left + textBoxFormat.InternalMargin.Right;
      rectangleF2.Height -= textBoxFormat.InternalMargin.Top + textBoxFormat.InternalMargin.Bottom + textBoxFormat.LineWidth;
    }
    if ((double) rectangleF2.Width <= 0.0 && (double) textBoxFormat.InternalMargin.Right > 0.0)
      rectangleF2.Width = textBoxFormat.InternalMargin.Right;
    if (!isHorizontalText)
      rectangleF2 = new RectangleF(rectangleF2.X, rectangleF2.Y, rectangleF2.Height, rectangleF2.Width);
    else if (textBoxFormat.LineStyle == TextBoxLineStyle.ThinThick)
      rectangleF2.Y += textBoxFormat.LineWidth;
    widget.TextLayoutingBounds = rectangleF2;
    bool splitbyCharacter = (this.m_lcOperator as Layouter).m_canSplitbyCharacter;
    bool mCanSplitByTab = (this.m_lcOperator as Layouter).m_canSplitByTab;
    bool isFirstItemInLine = (this.m_lcOperator as Layouter).IsFirstItemInLine;
    List<float> lineSpaceWidths = (this.m_lcOperator as Layouter).m_lineSpaceWidths;
    float effectiveJustifyWidth = (this.m_lcOperator as Layouter).m_effectiveJustifyWidth;
    LayoutContext layoutContext = LayoutContext.Create((IWidget) widget.TextBoxBody, this.m_lcOperator, this.IsForceFitLayout);
    layoutContext.ClientLayoutAreaRight = rectangleF2.Width;
    float paragraphYposition = (this.m_lcOperator as Layouter).ParagraphYPosition;
    LayoutedWidget layoutedWidget = layoutContext.Layout(rectangleF2);
    (this.m_lcOperator as Layouter).ParagraphYPosition = paragraphYposition;
    if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
      return;
    if (isAutoFit || isNeedToUpdateWidth)
      this.UpdateAutoFitRenderingBounds(this.m_ltWidget, layoutedWidget, isHorizontalText, isNeedToUpdateWidth, isAutoFit, textBoxFormat.InternalMargin);
    if ((double) textBoxFormat.Rotation != 0.0 && isAutoFit && textBoxFormat.TextWrappingStyle == TextWrappingStyle.Square)
      this.UpdateAutoFitTextBoxBounds(widget, layoutedWidget, textBoxFormat);
    if (isAutoFit && (double) layoutedWidget.Bounds.Height != (double) rectangleF1.Height && !this.IsForceFitLayout && textBoxFormat.TextWrappingStyle == TextWrappingStyle.Inline && this.IsLeafWidgetNeedToBeSplitted(layoutedWidget.Bounds.Size, this.m_layoutArea.ClientActiveArea.Width, rectangleF2))
    {
      this.m_ltState = LayoutState.NotFitted;
    }
    else
    {
      if (isHorizontalText)
      {
        if ((textBoxFormat.TextDirection == TextDirection.Horizontal || textBoxFormat.TextDirection == TextDirection.HorizontalFarEast) && (double) this.m_ltWidget.Bounds.Height < (double) this.m_layoutArea.ClientActiveArea.Height)
          rectangleF2.Height = this.m_ltWidget.Bounds.Height - (textBoxFormat.InternalMargin.Top + textBoxFormat.InternalMargin.Bottom);
        this.UpdateLayoutedWidgetBasedOnVerticalAlignment(rectangleF2, layoutedWidget, textBoxFormat.TextVerticalAlignment);
      }
      this.m_ltWidget.ChildWidgets.Add(layoutedWidget);
      layoutedWidget.Owner = this.m_ltWidget;
      if ((double) textBoxFormat.Rotation != 0.0 && !widget.IsWrappingBoundsAdded())
        this.m_ltWidget.Bounds = rectangleF1;
      (this.m_lcOperator as Layouter).ResetWordLayoutingFlags(splitbyCharacter, mCanSplitByTab, isFirstItemInLine, lineSpaceWidths, effectiveJustifyWidth);
    }
  }

  private void UpdateAutoFitTextBoxBounds(
    WTextBox textBox,
    LayoutedWidget textBodyLtWidget,
    WTextBoxFormat textBoxFormat)
  {
    float height = 0.0f;
    float x1 = this.m_ltWidget.Bounds.X;
    float y1 = this.m_ltWidget.Bounds.Y;
    float num1 = x1;
    float num2 = y1;
    if (!textBox.IsWrappingBoundsAdded())
      return;
    float num3 = textBox.Shape != null ? textBox.Shape.Rotation : textBoxFormat.Rotation;
    if ((this.m_lcOperator as Layouter).FloatingItems.Count > 0)
    {
      for (int index = 0; index < (this.m_lcOperator as Layouter).FloatingItems.Count; ++index)
      {
        if ((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity == textBox)
          height = ((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity as WTextBox).TextLayoutingBounds.Height + textBoxFormat.InternalMargin.Top + textBoxFormat.InternalMargin.Bottom;
      }
    }
    if (((double) num3 < 45.0 || (double) num3 >= 135.0) && ((double) num3 < 225.0 || (double) num3 >= 315.0))
      return;
    float x2;
    float y2;
    if ((double) textBoxFormat.Height > (double) this.m_ltWidget.Bounds.Height)
    {
      float num4 = (float) ((double) textBoxFormat.Height / 2.0 - (double) height / 2.0);
      x2 = num1 - num4;
      y2 = num2 + num4;
    }
    else
    {
      float num5 = (float) ((double) height / 2.0 - (double) textBoxFormat.Height / 2.0);
      x2 = num1 + num5;
      y2 = num2 - num5;
    }
    this.m_ltWidget.Bounds = new RectangleF(x2, y2, this.m_ltWidget.Bounds.Width, height);
    float yOffset = 0.0f;
    float xOffset = 0.0f;
    if (Math.Round((double) x2, 2) != Math.Round((double) x1, 2))
      xOffset = x2 - x1;
    if (Math.Round((double) y2, 2) != Math.Round((double) y1, 2))
      yOffset = y2 - y1;
    if ((double) xOffset == 0.0 && (double) yOffset == 0.0)
      return;
    textBodyLtWidget.ShiftLocation((double) xOffset, (double) yOffset, true, false, true);
  }

  private void UpdateAutoFitRenderingBounds(
    LayoutedWidget textbodyOwnerWidget,
    LayoutedWidget textbodyWidget,
    bool isHorizontalText,
    bool isNeedToUpdateWidth,
    bool isAutoFit,
    InternalMargin margin)
  {
    RectangleF textLayoutingBounds = new RectangleF();
    if (textbodyOwnerWidget.Widget is WTextBox)
      textLayoutingBounds = (textbodyOwnerWidget.Widget as WTextBox).TextLayoutingBounds;
    else if (textbodyOwnerWidget.Widget is Shape)
      textLayoutingBounds = (textbodyOwnerWidget.Widget as Shape).TextLayoutingBounds;
    else if (textbodyOwnerWidget.Widget is ChildShape)
      textLayoutingBounds = (textbodyOwnerWidget.Widget as ChildShape).TextLayoutingBounds;
    if (isHorizontalText)
      this.UpdateHorizontalTextRenderingBounds(textbodyOwnerWidget, isAutoFit, isNeedToUpdateWidth, textbodyWidget, margin, textLayoutingBounds);
    else
      this.UpdateVerticalTextRenderingBounds(textbodyOwnerWidget, isAutoFit, isNeedToUpdateWidth, textbodyWidget, margin, textLayoutingBounds);
    this.UpdateXYPositionBasedOnModifiedSize(textbodyOwnerWidget.Bounds.Width, textbodyOwnerWidget.Bounds.Height, textbodyWidget);
    if (textbodyOwnerWidget.Widget is WTextBox)
      (textbodyOwnerWidget.Widget as WTextBox).TextLayoutingBounds = textbodyWidget.Bounds;
    else if (textbodyOwnerWidget.Widget is Shape)
    {
      (textbodyOwnerWidget.Widget as Shape).TextLayoutingBounds = textbodyWidget.Bounds;
    }
    else
    {
      if (!(textbodyOwnerWidget.Widget is ChildShape))
        return;
      (textbodyOwnerWidget.Widget as ChildShape).TextLayoutingBounds = textbodyWidget.Bounds;
    }
  }

  private void UpdateHorizontalTextRenderingBounds(
    LayoutedWidget textbodyOwnerWidget,
    bool isAutoFit,
    bool isNeedToUpdateWidth,
    LayoutedWidget ltWidget,
    InternalMargin margin,
    RectangleF textLayoutingBounds)
  {
    float num = 0.0f;
    if (textbodyOwnerWidget.Widget is WTextBox && !(textbodyOwnerWidget.Widget as WTextBox).IsNoNeedToConsiderLineWidth())
      num = (textbodyOwnerWidget.Widget as WTextBox).TextBoxFormat.LineWidth;
    else if (textbodyOwnerWidget.Widget is Shape && !(textbodyOwnerWidget.Widget as Shape).IsNoNeedToConsiderLineWidth())
      num = (textbodyOwnerWidget.Widget as Shape).LineFormat.Weight;
    if (isAutoFit)
    {
      float height = ltWidget.Bounds.Height + margin.Top + margin.Bottom + num;
      textbodyOwnerWidget.Bounds = new RectangleF(textbodyOwnerWidget.Bounds.X, textbodyOwnerWidget.Bounds.Y, textbodyOwnerWidget.Bounds.Width, height);
    }
    if (!isNeedToUpdateWidth)
      return;
    float updatedWidth = ltWidget.Bounds.Width + margin.Left + margin.Right;
    float width;
    if (this.IsAnyOfParagraphHasMultipleLines(ltWidget))
    {
      width = this.m_layoutArea.ClientActiveArea.Width;
    }
    else
    {
      float maximumWidth = 0.0f;
      this.UpdateBoundsBasedOnParagraphAlignments(ltWidget, ref updatedWidth, textLayoutingBounds.Width, ref maximumWidth, false);
      width = updatedWidth + (margin.Left + margin.Right);
    }
    textbodyOwnerWidget.Bounds = new RectangleF(textbodyOwnerWidget.Bounds.X, textbodyOwnerWidget.Bounds.Y, width, textbodyOwnerWidget.Bounds.Height);
  }

  private void UpdateVerticalTextRenderingBounds(
    LayoutedWidget textbodyOwnerWidget,
    bool isAutoFit,
    bool isNeedToUpdateWidth,
    LayoutedWidget ltWidget,
    InternalMargin margin,
    RectangleF textLayoutingBounds)
  {
    if (isNeedToUpdateWidth)
    {
      float updatedWidth = ltWidget.Bounds.Width + margin.Top + margin.Bottom;
      float height;
      if (this.IsAnyOfParagraphHasMultipleLines(ltWidget))
      {
        height = (this.m_lcOperator as Layouter).ClientLayoutArea.Height + margin.Top + margin.Bottom;
      }
      else
      {
        float maximumWidth = 0.0f;
        this.UpdateBoundsBasedOnParagraphAlignments(ltWidget, ref updatedWidth, textLayoutingBounds.Width, ref maximumWidth, false);
        height = updatedWidth + (margin.Top + margin.Bottom);
      }
      textbodyOwnerWidget.Bounds = new RectangleF(textbodyOwnerWidget.Bounds.X, textbodyOwnerWidget.Bounds.Y, textbodyOwnerWidget.Bounds.Width, height);
    }
    if (!isAutoFit)
      return;
    float num = margin.Left + margin.Right;
    float width = ltWidget.Bounds.Height + num;
    if ((double) width > 1584.0)
      width = 1584f;
    textbodyOwnerWidget.Bounds = new RectangleF(textbodyOwnerWidget.Bounds.X, textbodyOwnerWidget.Bounds.Y, width, textbodyOwnerWidget.Bounds.Height);
  }

  private void UpdateXYPositionBasedOnModifiedSize(
    float modifiedWidth,
    float modifiedHeight,
    LayoutedWidget textBodyLtWidget)
  {
    float x = this.m_ltWidget.Bounds.X;
    float y = this.m_ltWidget.Bounds.Y;
    float num1 = x;
    float num2 = y;
    SizeF size = new SizeF(modifiedWidth, modifiedHeight);
    WParagraph ownerParagraphValue = (this.m_ltWidget.Widget as ParagraphItem).GetOwnerParagraphValue();
    if (ownerParagraphValue != null && !ownerParagraphValue.IsInCell)
      this.GetFloattingItemPosition(ref x, ref y, ref size);
    this.m_ltWidget.Bounds = new RectangleF(x, y, this.m_ltWidget.Bounds.Width, this.m_ltWidget.Bounds.Height);
    float yOffset = 0.0f;
    float xOffset = 0.0f;
    if (Math.Round((double) x, 2) != Math.Round((double) num1, 2))
      xOffset = x - num1;
    if (Math.Round((double) y, 2) != Math.Round((double) num2, 2))
      yOffset = y - num2;
    if ((double) xOffset == 0.0 && (double) yOffset == 0.0)
      return;
    textBodyLtWidget.ShiftLocation((double) xOffset, (double) yOffset, true, false, true);
  }

  private RectangleF UpdateAutoFitLayoutingBounds(
    RectangleF bounds,
    bool isHorizontalText,
    bool isAutoFit,
    bool isNeedToUpdateWidth)
  {
    if (isHorizontalText)
    {
      if (isNeedToUpdateWidth)
        bounds.Width = this.m_layoutArea.ClientActiveArea.Width;
      if (isAutoFit)
        bounds.Height = !(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter || (this.m_lcOperator as Layouter).IsLayoutingHeader ? (this.m_lcOperator as Layouter).CurrentSection.PageSetup.PageSize.Height - bounds.Y : (this.m_lcOperator as Layouter).ClientLayoutArea.Bottom - bounds.Y;
    }
    else
    {
      if (isNeedToUpdateWidth)
        bounds.Height = (this.m_lcOperator as Layouter).ClientLayoutArea.Height;
      if (isAutoFit)
        bounds.Width = 1584f;
    }
    return bounds;
  }

  private bool IsAutoFit(ref bool isNeedToUpdateWidth, IWidget entity)
  {
    switch (entity)
    {
      case WTextBox _:
        WTextBox wtextBox = entity as WTextBox;
        if (wtextBox.TextBoxFormat.WrappingMode == Syncfusion.DocIO.WrapMode.None || wtextBox.IsShape && wtextBox.Shape.TextFrame.NoWrap)
          isNeedToUpdateWidth = true;
        if (wtextBox.TextBoxFormat.AutoFit || wtextBox.IsShape && wtextBox.Shape.TextFrame.ShapeAutoFit)
          return true;
        break;
      case Shape _:
        Shape shape = entity as Shape;
        if (shape.TextFrame.NoWrap)
          isNeedToUpdateWidth = true;
        if (shape.TextFrame.ShapeAutoFit)
          return true;
        break;
      case ChildShape _:
        ChildShape childShape = entity as ChildShape;
        if (childShape.TextFrame.NoWrap)
          isNeedToUpdateWidth = true;
        if (childShape.TextFrame.ShapeAutoFit)
          return true;
        break;
    }
    return false;
  }

  private bool IsAnyOfParagraphHasMultipleLines(LayoutedWidget textBodyItems)
  {
    for (int index = 0; index < textBodyItems.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = textBodyItems.ChildWidgets[index];
      if (childWidget.Widget is BlockContentControl && this.IsAnyOfParagraphHasMultipleLines(childWidget) || childWidget.Widget is WParagraph && textBodyItems.ChildWidgets[index].ChildWidgets.Count > 1)
        return true;
    }
    return false;
  }

  private void UpdateBoundsBasedOnParagraphAlignments(
    LayoutedWidget ltWidget,
    ref float updatedWidth,
    float layoutedClientWidth,
    ref float maximumWidth,
    bool isRecursiveCall)
  {
    for (int index = 0; index < ltWidget.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = ltWidget.ChildWidgets[index];
      if (childWidget.Widget is WParagraph)
      {
        float maximumWidth1 = this.GetMaximumWidth(childWidget);
        if ((double) maximumWidth < (double) maximumWidth1)
          maximumWidth = maximumWidth1;
      }
      else if (childWidget.Widget is BlockContentControl)
        this.UpdateBoundsBasedOnParagraphAlignments(childWidget, ref updatedWidth, layoutedClientWidth, ref maximumWidth, true);
    }
    if (isRecursiveCall || (double) maximumWidth == (double) layoutedClientWidth)
      return;
    this.ShiftXPositionBasedOnAlignment(ltWidget, maximumWidth - layoutedClientWidth);
    updatedWidth = maximumWidth;
  }

  private void ShiftXPositionBasedOnAlignment(LayoutedWidget ltWidget, float widthReduced)
  {
    for (int index = 0; index < ltWidget.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = ltWidget.ChildWidgets[index];
      if (childWidget.Widget is WParagraph)
      {
        if (childWidget.Widget.LayoutInfo is ParagraphLayoutInfo layoutInfo)
        {
          if (layoutInfo.Justification == HAlignment.Center)
            childWidget.ShiftLocation((double) widthReduced / 2.0, 0.0, true, false, false);
          else if (layoutInfo.Justification == HAlignment.Right)
            childWidget.ShiftLocation((double) widthReduced, 0.0, true, false, false);
        }
      }
      else if (childWidget.Widget is BlockContentControl)
        this.ShiftXPositionBasedOnAlignment(childWidget, widthReduced);
    }
  }

  private float GetMaximumWidth(LayoutedWidget lineContainer)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    ParagraphLayoutInfo layoutInfo = lineContainer.Widget.LayoutInfo as ParagraphLayoutInfo;
    WParagraph widget = lineContainer.Widget as WParagraph;
    if (widget.ListFormat != null && !widget.ListFormat.IsEmptyList && widget.ListFormat.ListType != ListType.NoList && (double) layoutInfo.FirstLineIndent > 0.0)
      num2 = Math.Abs(this.DrawingContext.GetListValue(widget, layoutInfo, widget.ListFormat));
    float num3 = num2 + ((double) layoutInfo.FirstLineIndent > 0.0 ? layoutInfo.FirstLineIndent : 0.0f) + (layoutInfo.Margins.Left + layoutInfo.Margins.Right);
    if (lineContainer.ChildWidgets.Count > 0)
    {
      LayoutedWidget childWidget = lineContainer.ChildWidgets[0];
      if (layoutInfo != null && (layoutInfo.Justification == HAlignment.Center || layoutInfo.Justification == HAlignment.Right))
      {
        RectangleF itemsRenderingBounds = this.DrawingContext.GetInnerItemsRenderingBounds(childWidget);
        if ((double) num1 < (double) itemsRenderingBounds.Width)
          num1 = itemsRenderingBounds.Width;
      }
      else if ((double) num1 < (double) childWidget.Bounds.Width)
        num1 = childWidget.Bounds.Width;
    }
    return num1 + num3;
  }

  private void UpdateLayoutedWidgetBasedOnVerticalAlignment(
    RectangleF bounds,
    LayoutedWidget ltWidget,
    VerticalAlignment textVerticalAlignment)
  {
    float yOffset = 0.0f;
    switch (textVerticalAlignment)
    {
      case VerticalAlignment.Middle:
        yOffset = (float) (((double) bounds.Height - (double) ltWidget.Bounds.Height) / 2.0);
        break;
      case VerticalAlignment.Bottom:
        yOffset = bounds.Height - ltWidget.Bounds.Height;
        break;
    }
    if ((double) yOffset <= 0.0)
      return;
    ltWidget.ShiftLocation(0.0, (double) yOffset, false, false);
  }

  private void UpdateShapeBoundsToLayoutTextBody(
    ref RectangleF layoutRect,
    InternalMargin internalMargin)
  {
    Shape widget = this.m_ltWidget.Widget as Shape;
    layoutRect.Height -= layoutRect.Y;
    layoutRect.Y += this.m_ltWidget.Bounds.Y;
    layoutRect.Width -= layoutRect.X;
    layoutRect.X += this.m_ltWidget.Bounds.X;
    if (widget.IsNoNeedToConsiderLineWidth())
    {
      layoutRect.X += internalMargin.Left;
      layoutRect.Y += internalMargin.Top;
      layoutRect.Width -= internalMargin.Left + internalMargin.Right;
      layoutRect.Height -= internalMargin.Top + internalMargin.Bottom;
    }
    else
    {
      layoutRect.X += internalMargin.Left + widget.LineFormat.Weight / 2f;
      layoutRect.Y += internalMargin.Top + widget.LineFormat.Weight / 2f;
      layoutRect.Width -= internalMargin.Left + internalMargin.Right + widget.LineFormat.Weight;
      layoutRect.Height -= internalMargin.Top + internalMargin.Bottom + widget.LineFormat.Weight;
    }
  }

  private void UpdateChildShapeBoundsToLayoutTextBody(
    ref RectangleF layoutRect,
    InternalMargin internalMargin,
    LayoutedWidget ltWidget)
  {
    ChildShape widget = ltWidget.Widget as ChildShape;
    if (widget.AutoShapeType != AutoShapeType.Rectangle)
    {
      layoutRect.Height -= layoutRect.Y;
      layoutRect.Y += ltWidget.Bounds.Y;
      layoutRect.Width -= layoutRect.X;
      layoutRect.X += ltWidget.Bounds.X;
      layoutRect.X += internalMargin.Left + widget.LineFormat.Weight / 2f;
      layoutRect.Y += internalMargin.Top + widget.LineFormat.Weight / 2f;
      layoutRect.Width -= internalMargin.Left + internalMargin.Right + widget.LineFormat.Weight;
      layoutRect.Height -= internalMargin.Top + internalMargin.Bottom + widget.LineFormat.Weight;
    }
    else
    {
      if (widget.ElementType == EntityType.TextBox && !widget.LineFormat.Line)
      {
        layoutRect.X += internalMargin.Left;
        layoutRect.Y += internalMargin.Top;
        layoutRect.Width -= internalMargin.Left + internalMargin.Right;
        layoutRect.Height -= internalMargin.Top + internalMargin.Bottom;
      }
      else
      {
        layoutRect.X += internalMargin.Left + widget.LineFormat.Weight / 2f;
        layoutRect.Y += internalMargin.Top + widget.LineFormat.Weight / 2f;
        layoutRect.Width -= internalMargin.Left + internalMargin.Right + widget.LineFormat.Weight;
        layoutRect.Height -= internalMargin.Top + internalMargin.Bottom + widget.LineFormat.Weight;
      }
      if ((double) layoutRect.Width > 0.0 || (double) internalMargin.Right <= 0.0)
        return;
      layoutRect.Width = internalMargin.Right;
    }
  }

  private RectangleF GetBoundsToLayoutShapeTextBody(
    AutoShapeType autoShapeType,
    Dictionary<string, string> shapeGuide,
    RectangleF bounds)
  {
    Dictionary<string, float> shapeFormula = new ShapePath(bounds, shapeGuide).ParseShapeFormula(autoShapeType);
    switch (autoShapeType)
    {
      case AutoShapeType.Parallelogram:
      case AutoShapeType.Hexagon:
      case AutoShapeType.Cross:
      case AutoShapeType.SmileyFace:
      case AutoShapeType.NoSymbol:
      case AutoShapeType.FlowChartTerminator:
      case AutoShapeType.FlowChartSummingJunction:
      case AutoShapeType.FlowChartOr:
      case AutoShapeType.Star16Point:
      case AutoShapeType.Star24Point:
      case AutoShapeType.Star32Point:
      case AutoShapeType.Wave:
      case AutoShapeType.OvalCallout:
      case AutoShapeType.SnipSameSideCornerRectangle:
      case AutoShapeType.Teardrop:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.Trapezoid:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.Diamond:
      case AutoShapeType.FlowChartDecision:
      case AutoShapeType.FlowChartCollate:
        return new RectangleF(bounds.Width / 4f, bounds.Height / 4f, shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RoundedRectangle:
      case AutoShapeType.Octagon:
      case AutoShapeType.Plaque:
      case AutoShapeType.RoundedRectangularCallout:
      case AutoShapeType.SnipDiagonalCornerRectangle:
        return new RectangleF(shapeFormula["il"], shapeFormula["il"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.IsoscelesTriangle:
        return new RectangleF(shapeFormula["x1"], bounds.Height / 2f, shapeFormula["x3"], bounds.Height);
      case AutoShapeType.RightTriangle:
        return new RectangleF(bounds.Width / 12f, shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.Oval:
      case AutoShapeType.Donut:
      case AutoShapeType.BlockArc:
      case AutoShapeType.Arc:
      case AutoShapeType.CircularArrow:
      case AutoShapeType.FlowChartConnector:
      case AutoShapeType.FlowChartSequentialAccessStorage:
      case AutoShapeType.DoubleWave:
      case AutoShapeType.CloudCallout:
      case AutoShapeType.Chord:
      case AutoShapeType.Cloud:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RegularPentagon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["it"], shapeFormula["x3"], shapeFormula["y2"]);
      case AutoShapeType.Can:
        return new RectangleF(0.0f, shapeFormula["y2"], bounds.Width, shapeFormula["y3"]);
      case AutoShapeType.Cube:
        return new RectangleF(0.0f, shapeFormula["y1"], shapeFormula["x4"], bounds.Height);
      case AutoShapeType.Bevel:
        return new RectangleF(shapeFormula["x1"], shapeFormula["x1"], shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.FoldedCorner:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y2"]);
      case AutoShapeType.Heart:
        return new RectangleF(shapeFormula["il"], bounds.Height / 4f, shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.LightningBolt:
        return new RectangleF(shapeFormula["x4"], shapeFormula["y4"], shapeFormula["x9"], shapeFormula["y10"]);
      case AutoShapeType.Sun:
        return new RectangleF(shapeFormula["x9"], shapeFormula["y9"], shapeFormula["x8"], shapeFormula["y8"]);
      case AutoShapeType.Moon:
        return new RectangleF(shapeFormula["g12w"], shapeFormula["g15h"], shapeFormula["g0w"], shapeFormula["g16h"]);
      case AutoShapeType.DoubleBracket:
      case AutoShapeType.DoubleBrace:
      case AutoShapeType.FlowChartAlternateProcess:
        return new RectangleF(shapeFormula["il"], shapeFormula["il"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.LeftBracket:
      case AutoShapeType.LeftBrace:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], bounds.Width, shapeFormula["ib"]);
      case AutoShapeType.RightBracket:
      case AutoShapeType.RightBrace:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RightArrow:
        return new RectangleF(0.0f, shapeFormula["y1"], shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.LeftArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], bounds.Width, shapeFormula["y2"]);
      case AutoShapeType.UpArrow:
      case AutoShapeType.MathEqual:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x2"], bounds.Height);
      case AutoShapeType.DownArrow:
        return new RectangleF(shapeFormula["x1"], 0.0f, shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.LeftRightArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x4"], shapeFormula["y2"]);
      case AutoShapeType.UpDownArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x2"], shapeFormula["y4"]);
      case AutoShapeType.QuadArrow:
        return new RectangleF(shapeFormula["il"], shapeFormula["y3"], shapeFormula["ir"], shapeFormula["y4"]);
      case AutoShapeType.LeftRightUpArrow:
        return new RectangleF(shapeFormula["il"], shapeFormula["y3"], shapeFormula["ir"], shapeFormula["y5"]);
      case AutoShapeType.UTurnArrow:
      case AutoShapeType.FlowChartProcess:
      case AutoShapeType.RectangularCallout:
      case AutoShapeType.StraightConnector:
        return new RectangleF(0.0f, 0.0f, bounds.Width, bounds.Height);
      case AutoShapeType.LeftUpArrow:
        return new RectangleF(shapeFormula["il"], shapeFormula["y3"], shapeFormula["x4"], shapeFormula["y5"]);
      case AutoShapeType.BentUpArrow:
        return new RectangleF(0.0f, shapeFormula["y2"], shapeFormula["x4"], bounds.Height);
      case AutoShapeType.StripedRightArrow:
        return new RectangleF(shapeFormula["x4"], shapeFormula["y1"], shapeFormula["x6"], shapeFormula["y2"]);
      case AutoShapeType.NotchedRightArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x3"], shapeFormula["y2"]);
      case AutoShapeType.Pentagon:
      case AutoShapeType.RoundSingleCornerRectangle:
        return new RectangleF(0.0f, 0.0f, shapeFormula["ir"], bounds.Height);
      case AutoShapeType.Chevron:
        return new RectangleF(shapeFormula["il"], 0.0f, shapeFormula["ir"], bounds.Height);
      case AutoShapeType.RightArrowCallout:
        return new RectangleF(0.0f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.LeftArrowCallout:
        return new RectangleF(shapeFormula["x2"], 0.0f, bounds.Width, bounds.Height);
      case AutoShapeType.UpArrowCallout:
        return new RectangleF(0.0f, shapeFormula["y2"], bounds.Width, bounds.Height);
      case AutoShapeType.DownArrowCallout:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y2"]);
      case AutoShapeType.LeftRightArrowCallout:
        return new RectangleF(shapeFormula["x2"], 0.0f, shapeFormula["x3"], bounds.Height);
      case AutoShapeType.UpDownArrowCallout:
        return new RectangleF(0.0f, shapeFormula["y2"], bounds.Width, shapeFormula["y3"]);
      case AutoShapeType.QuadArrowCallout:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y2"], shapeFormula["x7"], shapeFormula["y7"]);
      case AutoShapeType.FlowChartData:
        return new RectangleF(bounds.Width / 5f, 0.0f, shapeFormula["x5"], bounds.Height);
      case AutoShapeType.FlowChartPredefinedProcess:
        return new RectangleF(bounds.Width / 8f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartInternalStorage:
        return new RectangleF(bounds.Width / 8f, bounds.Height / 8f, bounds.Width, bounds.Height);
      case AutoShapeType.FlowChartDocument:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y1"]);
      case AutoShapeType.FlowChartMultiDocument:
        return new RectangleF(0.0f, shapeFormula["y2"], shapeFormula["x5"], shapeFormula["y8"]);
      case AutoShapeType.FlowChartPreparation:
        return new RectangleF(bounds.Width / 5f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartManualInput:
      case AutoShapeType.FlowChartCard:
        return new RectangleF(0.0f, bounds.Height / 5f, bounds.Width, bounds.Height);
      case AutoShapeType.FlowChartManualOperation:
        return new RectangleF(bounds.Width / 5f, 0.0f, shapeFormula["x3"], bounds.Height);
      case AutoShapeType.FlowChartOffPageConnector:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y1"]);
      case AutoShapeType.FlowChartPunchedTape:
        return new RectangleF(0.0f, bounds.Height / 5f, bounds.Width, shapeFormula["ib"]);
      case AutoShapeType.FlowChartSort:
        return new RectangleF(bounds.Width / 4f, bounds.Height / 4f, shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.FlowChartExtract:
        return new RectangleF(bounds.Width / 4f, bounds.Height / 2f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartMerge:
        return new RectangleF(bounds.Width / 4f, 0.0f, shapeFormula["x2"], bounds.Height / 2f);
      case AutoShapeType.FlowChartStoredData:
        return new RectangleF(bounds.Width / 6f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartDelay:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.FlowChartMagneticDisk:
        return new RectangleF(0.0f, bounds.Height / 3f, bounds.Width, shapeFormula["y3"]);
      case AutoShapeType.FlowChartDirectAccessStorage:
        return new RectangleF(bounds.Width / 6f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartDisplay:
        return new RectangleF(bounds.Width / 6f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.Explosion1:
        return new RectangleF(shapeFormula["x5"], shapeFormula["y3"], shapeFormula["x21"], shapeFormula["y9"]);
      case AutoShapeType.Explosion2:
        return new RectangleF(shapeFormula["x5"], shapeFormula["y3"], shapeFormula["x19"], shapeFormula["y17"]);
      case AutoShapeType.Star4Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx2"], shapeFormula["sy2"]);
      case AutoShapeType.Star5Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx4"], shapeFormula["sy3"]);
      case AutoShapeType.Star8Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx4"], shapeFormula["sy4"]);
      case AutoShapeType.UpRibbon:
        return new RectangleF(shapeFormula["x2"], 0.0f, shapeFormula["x9"], shapeFormula["y2"]);
      case AutoShapeType.DownRibbon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y2"], shapeFormula["x9"], bounds.Height);
      case AutoShapeType.CurvedUpRibbon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y6"], shapeFormula["x5"], shapeFormula["rh"]);
      case AutoShapeType.CurvedDownRibbon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["q1"], shapeFormula["x5"], shapeFormula["y6"]);
      case AutoShapeType.VerticalScroll:
        return new RectangleF(shapeFormula["ch"], shapeFormula["ch"], shapeFormula["x6"], shapeFormula["y4"]);
      case AutoShapeType.HorizontalScroll:
        return new RectangleF(shapeFormula["ch"], shapeFormula["ch"], shapeFormula["x4"], shapeFormula["y6"]);
      case AutoShapeType.DiagonalStripe:
        return new RectangleF(0.0f, 0.0f, shapeFormula["x3"], shapeFormula["y3"]);
      case AutoShapeType.Pie:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.Decagon:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y2"], shapeFormula["x4"], shapeFormula["y3"]);
      case AutoShapeType.Heptagon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y1"], shapeFormula["x5"], shapeFormula["ib"]);
      case AutoShapeType.Dodecagon:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x4"], shapeFormula["y4"]);
      case AutoShapeType.Star6Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx4"], shapeFormula["sy2"]);
      case AutoShapeType.Star7Point:
        return new RectangleF(shapeFormula["sx2"], shapeFormula["sy1"], shapeFormula["sx5"], shapeFormula["sy3"]);
      case AutoShapeType.Star10Point:
        return new RectangleF(shapeFormula["sx2"], shapeFormula["sy2"], shapeFormula["sx5"], shapeFormula["sy3"]);
      case AutoShapeType.Star12Point:
        return new RectangleF(shapeFormula["sx2"], shapeFormula["sy2"], shapeFormula["sx5"], shapeFormula["sy5"]);
      case AutoShapeType.RoundSameSideCornerRectangle:
        return new RectangleF(shapeFormula["il"], shapeFormula["tdx"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RoundDiagonalCornerRectangle:
        return new RectangleF(shapeFormula["dx"], shapeFormula["dx"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        return new RectangleF(shapeFormula["il"], shapeFormula["il"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.SnipSingleCornerRectangle:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.Frame:
        return new RectangleF(shapeFormula["x1"], shapeFormula["x1"], shapeFormula["x4"], shapeFormula["y4"]);
      case AutoShapeType.L_Shape:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.MathPlus:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y2"], shapeFormula["x4"], shapeFormula["y3"]);
      case AutoShapeType.MathMinus:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.MathMultiply:
        return new RectangleF(shapeFormula["xA"], shapeFormula["yB"], shapeFormula["xE"], shapeFormula["yH"]);
      case AutoShapeType.MathDivision:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y3"], shapeFormula["x3"], shapeFormula["y4"]);
      case AutoShapeType.MathNotEqual:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x8"], shapeFormula["y4"]);
      default:
        return new RectangleF(0.0f, 0.0f, bounds.Width, bounds.Height);
    }
  }

  private WTextRange GetCurrTextRange()
  {
    return this.LeafWidget is WTextRange ? this.LeafWidget as WTextRange : (!(this.LeafWidget is SplitStringWidget) || !((this.LeafWidget as SplitStringWidget).RealStringWidget is WTextRange) ? (WTextRange) null : (this.LeafWidget as SplitStringWidget).RealStringWidget as WTextRange);
  }

  private void UpdateTabWidth(ref RectangleF rect, ref SizeF size, WTextRange textRange)
  {
    Layouter lcOperator = this.m_lcOperator as Layouter;
    float xPosition = rect.X;
    float pageMarginLeft = this.GetPageMarginLeft();
    float pageMarginRight = this.GetPageMarginRight();
    float num1 = lcOperator.PreviousTab.Position + pageMarginLeft;
    float previousTabWidth = lcOperator.PreviousTabWidth;
    float width = rect.X - (num1 - previousTabWidth);
    TabJustification justification = lcOperator.PreviousTab.Justification;
    if (justification == TabJustification.Centered && (double) width / 2.0 < (double) previousTabWidth)
    {
      xPosition = num1 + width / 2f;
      if ((double) rect.Right < (double) xPosition)
        xPosition = rect.Right;
      rect.Width -= xPosition - rect.X;
      rect.X = xPosition;
      this.CreateLayoutArea(rect);
    }
    else if (justification == TabJustification.Right && (double) rect.X < (double) num1)
    {
      xPosition = num1;
      rect.Width -= xPosition - rect.X;
      rect.X = xPosition;
      this.CreateLayoutArea(rect);
    }
    else if (justification == TabJustification.Decimal)
      this.UpdateLeafWidgetPosition(ref rect, ref xPosition, width);
    TabsLayoutInfo layoutInfo1 = this.LeafWidget.LayoutInfo as TabsLayoutInfo;
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    float num2 = (double) ownerParagraph.ParagraphFormat.FirstLineIndent < 0.0 ? ownerParagraph.ParagraphFormat.LeftIndent : 0.0f;
    layoutInfo1.PageMarginLeft = (double) pageMarginLeft;
    layoutInfo1.PageMarginRight = (double) pageMarginRight;
    ParagraphLayoutInfo layoutInfo2 = ((IWidget) ownerParagraph).LayoutInfo as ParagraphLayoutInfo;
    float num3 = !layoutInfo2.IsFirstLine ? layoutInfo2.Margins.Left : layoutInfo2.Margins.Left + layoutInfo2.FirstLineIndent;
    float num4 = (float) layoutInfo1.GetNextTabPosition((double) xPosition - (double) pageMarginLeft);
    if (((double) ownerParagraph.ParagraphFormat.FirstLineIndent < 0.0 || layoutInfo2.IsFirstLine && (double) num3 < 0.0 && (double) width < 0.0 && (double) layoutInfo1.m_currTab.Position + (double) pageMarginLeft < (double) num4 && !lcOperator.IsLayoutingHeaderFooter) && Math.Round((double) xPosition - (double) pageMarginLeft, 2) < Math.Round((double) num2, 2) && ((double) layoutInfo1.m_currTab.Position == 0.0 || (double) layoutInfo1.m_currTab.Position > (double) num2) && !lcOperator.IsTabWidthUpdatedBasedOnIndent && ownerParagraph.Document.UseHangingIndentAsTabPosition && (layoutInfo1.m_list.Count <= 0 || (double) ownerParagraph.ParagraphFormat.FirstLineIndent >= 0.0 || (double) layoutInfo1.m_currTab.Position != 0.0 || (double) num2 <= 0.0 || (double) xPosition >= (double) pageMarginLeft))
    {
      if (Math.Round((double) xPosition + (double) num4, 2) >= Math.Round((double) pageMarginLeft + (double) num2, 2))
        layoutInfo1.IsTabWidthUpdatedBasedOnIndent = true;
      lcOperator.IsTabWidthUpdatedBasedOnIndent = true;
      num4 = num2 - (xPosition - pageMarginLeft);
      if ((double) ownerParagraph.ParagraphFormat.FirstLineIndent < 0.0 && layoutInfo2.IsFirstLine && (double) num3 < 0.0 && (double) width < 0.0 && this.IsInTextBox(ownerParagraph) != null)
        num4 += layoutInfo2.Margins.Left;
      if (layoutInfo2.IsFirstLine && Math.Round((double) xPosition + (double) num4 - (double) pageMarginLeft, 2) > Math.Round((double) this.ClientLayoutAreaRight, 2) && ownerParagraph.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && !this.IsLeafWidgetIsInCell((ParagraphItem) (this.LeafWidget as WTextRange)))
        this.UpdateAreaMaxWidth();
    }
    else if ((double) num3 > (double) this.ClientLayoutAreaRight && Math.Round((double) xPosition - (double) pageMarginLeft, 2) <= Math.Round((double) num3, 2) && (double) layoutInfo1.m_currTab.Position != (double) xPosition - (double) pageMarginLeft + (double) num4 && !this.IsLeafWidgetIsInCell((ParagraphItem) (this.LeafWidget as WTextRange)))
      num4 = num3 - (xPosition - pageMarginLeft);
    if (this.IsLeafWidgetIsInCell((ParagraphItem) (this.LeafWidget as WTextRange)) && (double) num4 > (double) this.m_layoutArea.ClientActiveArea.Width && (double) xPosition - (double) num3 == (double) pageMarginLeft && (double) layoutInfo1.m_currTab.Position == 0.0 && layoutInfo1.m_currTab.Justification == TabJustification.Left)
    {
      layoutInfo1.IsTabWidthUpdatedBasedOnIndent = false;
      num4 = this.m_layoutArea.ClientActiveArea.Width;
    }
    size.Width = (float) Math.Round((double) num4, 2);
    if (!layoutInfo1.IsTabWidthUpdatedBasedOnIndent && (layoutInfo1.CurrTabJustification == TabJustification.Centered || layoutInfo1.CurrTabJustification == TabJustification.Right || layoutInfo1.CurrTabJustification == TabJustification.Decimal && this.LeafWidget is ParagraphItem && (!this.IsLeafWidgetIsInCell(this.LeafWidget as ParagraphItem) || ownerParagraph.ParagraphFormat.Tabs.Count != 1)))
      size.Width = 0.0f;
    (this.m_lcOperator as Layouter).PreviousTabWidth = num4;
    layoutInfo1.TabWidth = num4;
    this.LeafWidget.LayoutInfo.Size = new SizeF(size.Width, this.LeafWidget.LayoutInfo.Size.Height);
  }

  private void UpdateAreaMaxWidth()
  {
    this.UpdateAreaWidth(0.0f);
    this.IsAreaUpdated = true;
    this.IsTabStopBeyondRightMarginExists = true;
  }

  private float ClientAreaRight(WParagraph paragraph, float rectRight)
  {
    return this.IsBaseFromSection((Entity) paragraph) ? (this.m_lcOperator as Layouter).ClientLayoutArea.Right : rectRight;
  }

  private float GetPageMarginLeft()
  {
    float left = (this.m_lcOperator as Layouter).ClientLayoutArea.Left;
    ParagraphItem paraItem = this.LeafWidget is ParagraphItem ? this.LeafWidget as ParagraphItem : (this.LeafWidget is SplitStringWidget ? (this.LeafWidget as SplitStringWidget).RealStringWidget as ParagraphItem : (ParagraphItem) null);
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    if (this.IsLeafWidgetIsInCell(paraItem))
      left = (((IWidget) (ownerParagraph.GetOwnerEntity() as WTableCell)).LayoutInfo as CellLayoutInfo).CellContentLayoutingBounds.Left;
    else if (ownerParagraph != null && !ownerParagraph.IsInCell && ownerParagraph.ParagraphFormat.IsFrame)
      left = (this.m_lcOperator as Layouter).FrameLayoutArea.Left;
    else if (ownerParagraph != null && ownerParagraph.OwnerBase != null)
    {
      if (ownerParagraph.OwnerBase.OwnerBase is WTextBox)
        left = (ownerParagraph.OwnerBase.OwnerBase as WTextBox).TextLayoutingBounds.Left;
      else if (ownerParagraph.OwnerBase.OwnerBase is Shape)
        left = (ownerParagraph.OwnerBase.OwnerBase as Shape).TextLayoutingBounds.Left;
    }
    return left;
  }

  private float GetPageMarginRight()
  {
    float pageMarginRight = (this.m_lcOperator as Layouter).ClientLayoutArea.Right;
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    if (ownerParagraph == null)
      return pageMarginRight;
    if (ownerParagraph.IsInCell)
    {
      CellLayoutInfo layoutInfo = ((IWidget) (ownerParagraph.GetOwnerEntity() as WTableCell)).LayoutInfo as CellLayoutInfo;
      pageMarginRight = layoutInfo.CellContentLayoutingBounds.Right + layoutInfo.Margins.Right;
    }
    else if (ownerParagraph != null && !ownerParagraph.IsInCell && ownerParagraph.ParagraphFormat.IsFrame)
      pageMarginRight = (this.m_lcOperator as Layouter).FrameLayoutArea.Right;
    else if (ownerParagraph.OwnerBase != null && ownerParagraph.OwnerBase.OwnerBase is WTextBox)
    {
      WTextBox ownerBase = ownerParagraph.OwnerBase.OwnerBase as WTextBox;
      pageMarginRight = ownerBase.TextLayoutingBounds.Right + ownerBase.TextBoxFormat.InternalMargin.Right;
    }
    return pageMarginRight;
  }

  private void UpdateLeafWidgetPosition(ref RectangleF rect, ref float xPosition, float width)
  {
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    if (ownerParagraph == null)
      return;
    int decimalTabEnd = ownerParagraph.ChildEntities.InnerList.IndexOf((object) (this.LeafWidget as WTextRange));
    int decimalTabStart = 0;
    for (int index = decimalTabEnd - 1; index >= 0; --index)
    {
      if (ownerParagraph.ChildEntities[index] is WTextRange && (ownerParagraph.ChildEntities[index] as ILeafWidget).LayoutInfo is TabsLayoutInfo)
      {
        decimalTabStart = index;
        break;
      }
    }
    float leftWidth = this.DrawingContext.GetLeftWidth(ownerParagraph, decimalTabStart, decimalTabEnd);
    Layouter lcOperator = this.m_lcOperator as Layouter;
    if ((double) leftWidth >= (double) lcOperator.PreviousTabWidth)
      return;
    width -= leftWidth;
    xPosition = lcOperator.PreviousTab.Position + this.GetPageMarginLeft() + width;
    rect.Width -= xPosition - rect.X;
    rect.X = xPosition;
    this.CreateLayoutArea(rect);
  }

  internal bool IsLeafWidgetNeedToBeSplitted(
    SizeF size,
    float clientActiveAreaWidth,
    RectangleF rect)
  {
    bool flag = this.IsClipped(size);
    return (double) size.Width < (double) clientActiveAreaWidth && this.IsTextContainsLineBreakCharacters() || (this.LeafWidget is WPicture || this.LeafWidget is WOleObject || this.LeafWidget is Shape || this.LeafWidget is GroupShape || this.LeafWidget is WTextBox || this.LeafWidget is WChart ? (!this.IsPictureFit(size, clientActiveAreaWidth, rect) ? 0 : (this.m_layoutArea.Width != 0.0 ? 1 : (this.IsParagraphItemNeedToFit(this.LeafWidget as ParagraphItem) ? 1 : 0))) : (this.TryFit(size) ? 1 : 0)) == 0 && !flag && (this.LeafWidget is WTextRange || this.LeafWidget is SplitStringWidget ? (this.IsTextRangeFitInClientActiveArea(size) ? 1 : 0) : 0) == 0;
  }

  private bool TryFit(SizeF s)
  {
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    float val2 = ownerParagraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.AtLeast ? ownerParagraph.ParagraphFormat.LineSpacing : 0.0f;
    if ((double) s.Width > (double) this.m_layoutArea.ClientActiveArea.Width)
      return false;
    if ((double) Math.Max(s.Height, val2) <= (double) this.m_layoutArea.ClientActiveArea.Height || this.IsForceFitLayout || ownerParagraph != null && (this.IsLineSpacingFitsWidget(ownerParagraph, s.Height) || ownerParagraph.IsZeroAutoLineSpace()) || this.IsNeedToFitItemOfLastParagraph())
      return true;
    return this.LeafWidget is Break && (this.LeafWidget as Break).BreakType == BreakType.LineBreak && this.IsNeedToFitLineBreak();
  }

  private bool IsNeedToFitItemOfLastParagraph()
  {
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    return ownerParagraph != null && (((IWidget) ownerParagraph).LayoutInfo as ParagraphLayoutInfo).IsNotFitted;
  }

  private bool IsNeedToFitLastParagraph(float columnWidth, SizeF size)
  {
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    return (ownerParagraph == null || ownerParagraph.ParagraphFormat.WidowControl) && (ownerParagraph == null || ownerParagraph.ChildEntities.Count <= 0 || !(ownerParagraph.ChildEntities[0] as IWidget).LayoutInfo.IsFirstItemInPage) && (ownerParagraph == null || !(this.LeafWidget is WOleObject) && !this.IsPictureFromOleObject() || this.IsWord2013(ownerParagraph.Document) || (double) size.Width <= (double) columnWidth);
  }

  private bool IsPictureFromOleObject()
  {
    if (!(this.LeafWidget is WPicture))
      return false;
    IEntity previousSibling = (this.LeafWidget as WPicture).PreviousSibling;
    return previousSibling != null && previousSibling is WFieldMark && (previousSibling as WFieldMark).Type == FieldMarkType.FieldSeparator && (previousSibling as WFieldMark).ParentField != null && (previousSibling as WFieldMark).ParentField.FieldType == FieldType.FieldEmbed && (previousSibling as WFieldMark).ParentField.OwnerBase is WOleObject;
  }

  private bool IsNeedToFitLineBreak()
  {
    bool fitLineBreak = false;
    ParagraphLayoutInfo layoutInfo = ((IWidget) this.GetOwnerParagraph()).LayoutInfo as ParagraphLayoutInfo;
    if (Math.Round((double) (layoutInfo.XPosition + layoutInfo.Margins.Left + layoutInfo.Paddings.Left), 2) != Math.Round((double) this.m_layoutArea.ClientActiveArea.X, 2))
      fitLineBreak = true;
    return fitLineBreak;
  }

  private bool IsTextContainsLineBreakCharacters()
  {
    string str = this.LeafWidget is WField ? ((this.LeafWidget as WField).FieldType == FieldType.FieldNumPages ? (this.LeafWidget as WField).FieldResult : string.Empty) : (this.LeafWidget is WTextRange ? (this.LeafWidget as WTextRange).Text : (string) null);
    SplitStringWidget leafWidget = this.LeafWidget as SplitStringWidget;
    if (str != null && (str.Contains(ControlChar.LineFeed) || str.Contains(ControlChar.CarriegeReturn)))
      return true;
    if (leafWidget == null || leafWidget.SplittedText == null)
      return false;
    return leafWidget.SplittedText.Contains(ControlChar.LineFeed) || leafWidget.SplittedText.Contains(ControlChar.CarriegeReturn);
  }

  private bool IsClipped(SizeF size)
  {
    bool flag1 = false;
    Entity entity = this.LeafWidget is WPicture ? (Entity) (this.LeafWidget as WPicture) : (this.LeafWidget is WChart ? (Entity) (this.LeafWidget as WChart) : (this.LeafWidget is WOleObject ? (Entity) (this.LeafWidget as WOleObject).OlePicture : this.LeafWidget as Entity));
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    if (this.LayoutInfo.IsClipped && !(this.LeafWidget is WTextRange) && !(this.LeafWidget is SplitStringWidget) && this.m_layoutArea.Height != 0.0 && this.m_layoutArea.Width != 0.0)
    {
      bool isInCell = false;
      bool isInTextBox = false;
      switch (entity)
      {
        case ParagraphItem paraItem:
          if (ownerParagraph != null)
          {
            isInCell = ownerParagraph.IsInCell;
            isInTextBox = this.GetBaseEntity((Entity) ownerParagraph) is WTextBox;
          }
          if (ownerParagraph != null && this.IsFitLeafWidgetInContainerHeight(paraItem, isInCell, isInTextBox, (Entity) null))
          {
            if ((double) size.Height > (double) this.m_layoutArea.ClientActiveArea.Height && ((IWidget) ownerParagraph).LayoutInfo.IsClipped && !this.LayoutInfo.IsVerticalText || this.LayoutInfo.IsVerticalText && (double) size.Height > (double) this.m_layoutArea.ClientActiveArea.Width)
              flag1 = true;
            bool flag2 = !ownerParagraph.IsInCell || (double) this.DrawingContext.GetCellWidth(paraItem) < (double) size.Width;
            if ((double) size.Height <= (double) this.m_layoutArea.ClientActiveArea.Height && flag2 && (this.m_lcOperator as Layouter).IsFirstItemInLine && !this.LayoutInfo.IsVerticalText || this.LayoutInfo.IsVerticalText && (double) size.Height <= (double) this.m_layoutArea.ClientActiveArea.Width && (double) size.Width > (double) this.m_layoutArea.ClientActiveArea.Height)
            {
              flag1 = true;
              break;
            }
            break;
          }
          break;
        case WPicture _:
        case Shape _:
        case WChart _:
        case GroupShape _:
          break;
        default:
          flag1 = true;
          break;
      }
    }
    WTextRange textRange = this.GetTextRange((IWidget) this.LeafWidget);
    if (this.LayoutInfo.IsClipped && textRange != null && this.m_layoutArea.Height != 0.0 && this.m_layoutArea.Width != 0.0 && ownerParagraph != null && ownerParagraph.IsInCell && this.LayoutInfo.IsVerticalText && (double) size.Height >= (double) this.m_layoutArea.ClientActiveArea.Height)
      flag1 = true;
    float num = this.m_layoutArea.ClientActiveArea.Width;
    if ((textRange != null && textRange.Text.Length == 1 || this.LeafWidget is WCheckBox || this.LeafWidget is WDropDownFormField || this.LeafWidget is SplitStringWidget && (this.LeafWidget as SplitStringWidget).SplittedText != null && (this.LeafWidget as SplitStringWidget).SplittedText.Length == 1) && ownerParagraph != null)
    {
      if (ownerParagraph.IsInCell)
      {
        num = this.DrawingContext.GetCellWidth((ParagraphItem) textRange);
        if ((double) size.Width > (double) num)
          flag1 = true;
      }
      else if (ownerParagraph.OwnerBase != null && ownerParagraph.OwnerBase.OwnerBase is WTextBox)
      {
        num = (ownerParagraph.OwnerBase.OwnerBase as WTextBox).TextLayoutingBounds.Width;
        if ((double) size.Width > (double) num)
          flag1 = true;
      }
      if (this.IsParagraphItemNeedToFit((ParagraphItem) textRange))
        flag1 = true;
    }
    Entity ownerEntity = ownerParagraph.GetOwnerEntity();
    if (this.LeafWidget is WSymbol && ownerParagraph != null && (ownerParagraph.IsInCell || ownerParagraph.IsNeedToFitSymbol(ownerParagraph)))
    {
      switch (ownerEntity)
      {
        case WTextBox _:
          num = (ownerEntity as WTextBox).TextLayoutingBounds.Width;
          break;
        case Shape _:
          num = (ownerEntity as Shape).Width;
          break;
        case ChildShape _:
          num = (ownerEntity as ChildShape).Width;
          break;
        default:
          num = this.DrawingContext.GetCellWidth((ParagraphItem) (this.LeafWidget as WSymbol));
          break;
      }
      if ((double) size.Width > (double) num)
        flag1 = true;
    }
    if ((double) size.Width < (double) num && (double) size.Height >= (double) this.m_layoutArea.ClientActiveArea.Height && this.LayoutInfo.IsClipped)
      flag1 = true;
    return flag1;
  }

  private bool IsParagraphItemNeedToFit(ParagraphItem paraItem)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    if (paraItem != null && ownerParagraph != null)
    {
      WTableCell ownerTextBody = ownerParagraph.OwnerTextBody as WTableCell;
      num2 = !ownerParagraph.IsInCell || ownerTextBody == null ? (this.m_lcOperator as Layouter).ClientLayoutArea.Width : ownerTextBody.Width;
      ParagraphLayoutInfo layoutInfo = ((IWidget) ownerParagraph).LayoutInfo as ParagraphLayoutInfo;
      num1 = !layoutInfo.IsFirstLine ? layoutInfo.Margins.Left + layoutInfo.Margins.Right : layoutInfo.Margins.Left + layoutInfo.Margins.Right + layoutInfo.FirstLineIndent;
      if ((this.m_lcOperator as Layouter).IsFirstItemInLine)
      {
        switch (paraItem)
        {
          case WPicture _:
          case WChart _:
          case Shape _:
          case GroupShape _:
          case WTextBox _:
            if (ownerTextBody != null)
              return true;
            break;
        }
      }
    }
    return (double) num1 >= (double) num2;
  }

  private float GetPararaphLeftIndent()
  {
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    if (ownerParagraph == null)
      return 0.0f;
    ParagraphLayoutInfo layoutInfo = ((IWidget) ownerParagraph).LayoutInfo as ParagraphLayoutInfo;
    return layoutInfo.IsFirstLine ? layoutInfo.Margins.Left + layoutInfo.FirstLineIndent : layoutInfo.Margins.Left;
  }

  private bool IsPictureFit(SizeF size, float clientActiveAreaWidth, RectangleF rect)
  {
    bool flag1 = false;
    float emptyTextRangeHeight = this.GetEmptyTextRangeHeight();
    ParagraphItem paragraphItem = this.LeafWidget is WOleObject ? (ParagraphItem) (this.LeafWidget as WOleObject).OlePicture : this.LeafWidget as ParagraphItem;
    if (paragraphItem == null)
      return true;
    TextWrappingStyle textWrappingStyle = paragraphItem.GetTextWrappingStyle();
    VerticalOrigin verticalOrigin = paragraphItem.GetVerticalOrigin();
    HorizontalOrigin horizontalOrigin = paragraphItem.GetHorizontalOrigin();
    bool flag2 = textWrappingStyle == TextWrappingStyle.Inline;
    float height = (this.m_lcOperator as Layouter).ClientLayoutArea.Height;
    WParagraph ownerParagraphValue = paragraphItem.GetOwnerParagraphValue();
    Entity entity = this.GetBaseEntity(paragraphItem, ownerParagraphValue);
    ParagraphLayoutInfo paragraphLayoutInfo1 = (ParagraphLayoutInfo) null;
    if (ownerParagraphValue != null)
      paragraphLayoutInfo1 = ((IWidget) ownerParagraphValue).LayoutInfo as ParagraphLayoutInfo;
    if (flag2)
    {
      float angle = this.GetAngle(paragraphItem);
      if ((double) angle != 0.0 && !(paragraphItem is WPicture))
      {
        rect = this.DrawingContext.GetBoundingBoxCoordinates(new RectangleF(rect.X, rect.Y, size.Width, size.Height), angle);
        size = new SizeF(rect.Width, rect.Height);
      }
      (this.m_lcOperator as Layouter).m_canSplitbyCharacter = false;
      float num = 0.0f;
      if (ownerParagraphValue != null)
        num = this.GetLineSpacing(ownerParagraphValue);
      float picHeight = ownerParagraphValue == null ? size.Height : (ownerParagraphValue.ParagraphFormat.LineSpacingRule == LineSpacingRule.Exactly ? num : (ownerParagraphValue.ParagraphFormat.LineSpacingRule != LineSpacingRule.AtLeast || (double) num <= (double) size.Height ? size.Height : num));
      switch (entity)
      {
        case WSection _:
          if (this.IsFitPictureInSection((this.m_lcOperator as Layouter).ClientLayoutArea.Width - (paragraphLayoutInfo1 == null ? 0.0f : (float) ((double) paragraphLayoutInfo1.Margins.Left + (double) paragraphLayoutInfo1.Margins.Right + (paragraphLayoutInfo1.IsFirstLine ? (double) paragraphLayoutInfo1.FirstLineIndent + (double) paragraphLayoutInfo1.ListTab : 0.0))), picHeight, height, size, clientActiveAreaWidth, (Entity) ownerParagraphValue, paragraphItem))
          {
            flag1 = true;
            break;
          }
          break;
        case WTextBox _:
          if (this.IsFitPictureInTextBox(size, picHeight, entity))
          {
            flag1 = true;
            break;
          }
          break;
        case WTable _:
          if (this.IsFitPictureInTable(this.DrawingContext.GetCellWidth(paragraphItem), picHeight, size, entity as WTable))
          {
            flag1 = true;
            break;
          }
          break;
        default:
          if (ownerParagraphValue != null && entity is HeaderFooter && entity.Owner is WSection)
          {
            double headerFooterPosition = 0.0;
            float pageHeight;
            if (ownerParagraphValue.ParagraphFormat.IsFrame)
              pageHeight = this.m_layoutArea.ClientActiveArea.Height;
            else if ((this.m_lcOperator as Layouter).IsLayoutingHeader)
            {
              headerFooterPosition = Math.Round((double) (entity.Owner as WSection).PageSetup.HeaderDistance, 2);
              pageHeight = this.m_layoutArea.ClientActiveArea.Height - (float) headerFooterPosition;
            }
            else
            {
              headerFooterPosition = Math.Round((double) (entity.Owner as WSection).PageSetup.PageSize.Height) - Math.Round((double) (entity.Owner as WSection).PageSetup.FooterDistance, 2);
              pageHeight = this.m_layoutArea.ClientActiveArea.Height - (entity.Owner as WSection).PageSetup.FooterDistance;
            }
            float columnWidth = (this.m_lcOperator as Layouter).ClientLayoutArea.Width - (paragraphLayoutInfo1 == null ? 0.0f : (float) ((double) paragraphLayoutInfo1.Margins.Left + (double) paragraphLayoutInfo1.Margins.Right + (paragraphLayoutInfo1.IsFirstLine ? (double) paragraphLayoutInfo1.FirstLineIndent + (double) paragraphLayoutInfo1.ListTab : 0.0)));
            if (this.IsFitPictureInHeaderFooter(picHeight, pageHeight, size, columnWidth, clientActiveAreaWidth, entity, headerFooterPosition))
            {
              flag1 = true;
              break;
            }
            break;
          }
          break;
      }
    }
    else if (!flag2 && (entity is WTable ? 1 : (verticalOrigin == VerticalOrigin.Paragraph ? 1 : 0)) != 0 && (textWrappingStyle == TextWrappingStyle.Square || textWrappingStyle == TextWrappingStyle.Through || textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.TopAndBottom))
    {
      if (entity is HeaderFooter)
        entity = entity.Owner;
      flag1 = this.IsFloatingItemFit(entity, size, ownerParagraphValue, paragraphItem, height, rect);
      if (flag1 && !this.IsNextSiblingFitted(verticalOrigin, horizontalOrigin, paragraphItem))
        flag1 = false;
    }
    else if ((double) emptyTextRangeHeight <= (double) this.m_layoutArea.ClientActiveArea.Height || paragraphItem is WTextBox)
    {
      flag1 = true;
      if (!this.IsNextSiblingFitted(verticalOrigin, horizontalOrigin, paragraphItem))
        flag1 = false;
    }
    else if ((double) size.Height <= (double) this.m_layoutArea.ClientActiveArea.Height && (double) size.Width <= (double) this.m_layoutArea.ClientActiveArea.Width || this.IsForceFitLayout)
      flag1 = true;
    if (ownerParagraphValue != null && !flag1 && ownerParagraphValue.ParagraphFormat.IsInFrame())
    {
      flag1 = true;
      ParagraphLayoutInfo paragraphLayoutInfo2 = (ParagraphLayoutInfo) null;
      if (ownerParagraphValue != null)
        paragraphLayoutInfo2 = ((IWidget) ownerParagraphValue).LayoutInfo as ParagraphLayoutInfo;
      if (flag2 && paragraphLayoutInfo2 != null)
        paragraphLayoutInfo2.SkipBottomBorder = true;
    }
    return flag1;
  }

  private bool IsNextSiblingFitted(
    VerticalOrigin vOrgin,
    HorizontalOrigin hOrigin,
    ParagraphItem paraItem)
  {
    if ((vOrgin == VerticalOrigin.Line || hOrigin == HorizontalOrigin.Character) && paraItem.IsFloatingItem(false) && paraItem.NextSibling != null)
    {
      IWidget inlineNextSibling = this.GetValidInlineNextSibling(paraItem);
      if (inlineNextSibling != null)
      {
        SizeF sizeF = new SizeF();
        if (inlineNextSibling is WTextRange)
        {
          WTextRange txtRange = inlineNextSibling as WTextRange;
          if (txtRange.m_layoutInfo is TabsLayoutInfo && txtRange.Text == "")
          {
            TabsLayoutInfo layoutInfo = txtRange.m_layoutInfo as TabsLayoutInfo;
            sizeF = new SizeF((float) layoutInfo.DefaultTabWidth, layoutInfo.Size.Height);
          }
          else
            sizeF = this.DrawingContext.MeasureTextRange(txtRange, txtRange.Text.Split(' ')[0]);
        }
        else
          sizeF = (inlineNextSibling as ILeafWidget).Measure(this.DrawingContext);
        if ((double) sizeF.Width > (double) this.m_layoutArea.ClientActiveArea.Width || (double) sizeF.Height > (double) this.m_layoutArea.ClientActiveArea.Height)
        {
          if (this.IsNeedToWrapFloatingItem(this.LeafWidget))
            (this.m_lcOperator as Layouter).WrapFloatingItems.RemoveAt((this.m_lcOperator as Layouter).WrapFloatingItems.Count - 1);
          return false;
        }
      }
    }
    return true;
  }

  private float GetLineSpacing(WParagraph ownerParagraph)
  {
    switch (ownerParagraph.ParagraphFormat.LineSpacingRule)
    {
      case LineSpacingRule.AtLeast:
      case LineSpacingRule.Exactly:
        return ownerParagraph.ParagraphFormat.LineSpacing;
      case LineSpacingRule.Multiple:
        return ownerParagraph.ParagraphFormat.LineSpacing / 12f;
      default:
        return 0.0f;
    }
  }

  private bool IsFitPictureInSection(
    float columnWidth,
    float picHeight,
    float pageHeight,
    SizeF size,
    float clientActiveAreaWidth,
    Entity ent,
    ParagraphItem paraItem)
  {
    float leftEdgeExtent;
    float rightEgeExtent;
    float topEdgeExtent;
    float bottomEdgeExtent;
    paraItem.GetEffectExtentValues(out leftEdgeExtent, out rightEgeExtent, out topEdgeExtent, out bottomEdgeExtent);
    float num1 = leftEdgeExtent + rightEgeExtent;
    float num2 = topEdgeExtent + bottomEdgeExtent;
    if ((double) picHeight + (double) num2 > (double) this.m_layoutArea.ClientActiveArea.Height && (!this.IsNeedToFitItemOfLastParagraph() || !this.IsNeedToFitLastParagraph(columnWidth, size)) && ((double) picHeight + (double) num2 < (double) this.m_layoutArea.ClientActiveArea.Height || !(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && !this.IsForceFitLayout))
      return false;
    return Math.Round((double) size.Width + (double) num1, 2) <= Math.Round((double) this.m_layoutArea.ClientActiveArea.Width, 2) || (double) size.Width + (double) num1 > (double) columnWidth && (this.m_lcOperator as Layouter).IsFirstItemInLine || Math.Round((double) clientActiveAreaWidth, 2) != Math.Round((double) this.m_layoutArea.ClientActiveArea.Width, 2);
  }

  private bool IsFitPictureInTextBox(SizeF size, float picHeight, Entity ent)
  {
    WTextBox wtextBox = ent as WTextBox;
    Layouter lcOperator = this.m_lcOperator as Layouter;
    if ((double) picHeight <= (double) this.m_layoutArea.ClientActiveArea.Height && ((double) size.Width <= (double) this.m_layoutArea.ClientActiveArea.Width || Math.Round((double) this.m_layoutArea.ClientActiveArea.Width, 2) == Math.Round((double) wtextBox.TextBoxFormat.InternalMargin.Top, 2)) && this.LayoutInfo != null && !this.LayoutInfo.IsVerticalText || this.LayoutInfo != null && this.LayoutInfo.IsVerticalText && (double) size.Height <= (double) this.m_layoutArea.ClientActiveArea.Width)
      return true;
    return (double) size.Height > (double) lcOperator.ClientLayoutArea.Height && this.LayoutInfo != null && !this.LayoutInfo.IsClipped && this.IsForceFitLayout;
  }

  private bool IsFitPictureInTable(float cellWidth, float picHeight, SizeF size, WTable table)
  {
    Layouter lcOperator = this.m_lcOperator as Layouter;
    float headerRowHeight = (((IWidget) table).LayoutInfo as TableLayoutInfo).HeaderRowHeight;
    if ((double) picHeight > (double) lcOperator.ClientLayoutArea.Height && this.IsForceFitLayout)
    {
      lcOperator.IsRowFitInSamePage = true;
      this.m_layoutArea = new LayoutArea(new RectangleF(this.m_layoutArea.ClientActiveArea.X, this.m_layoutArea.ClientActiveArea.Y, this.m_layoutArea.ClientActiveArea.Width, size.Height));
    }
    if (Math.Round((double) picHeight, 4) <= Math.Round((double) this.m_layoutArea.ClientActiveArea.Height, 4) + (!this.IsWord2013(table.Document) || !this.IsForceFitLayout ? 0.0 : (double) headerRowHeight) && ((double) size.Width <= (double) this.m_layoutArea.ClientActiveArea.Width || Math.Round((double) this.m_layoutArea.ClientActiveArea.Width, 2) == Math.Round((double) cellWidth, 2)) && this.LayoutInfo != null && !this.LayoutInfo.IsVerticalText || this.LayoutInfo != null && this.LayoutInfo.IsVerticalText && (double) size.Height <= (double) this.m_layoutArea.ClientActiveArea.Width)
      return true;
    return lcOperator.IsFirstItemInLine && this.IsForceFitLayout;
  }

  private bool IsFitPictureInHeaderFooter(
    float picHeight,
    float pageHeight,
    SizeF size,
    float columnWidth,
    float clientActiveAreaWidth,
    Entity ent,
    double headerFooterPosition)
  {
    if ((double) picHeight > (double) pageHeight && ((double) picHeight <= (double) pageHeight || !this.IsForceFitLayout))
      return false;
    return (double) size.Width <= (double) this.m_layoutArea.ClientActiveArea.Width || (double) size.Width > (double) columnWidth && (this.m_lcOperator as Layouter).IsFirstItemInLine || Math.Round((double) clientActiveAreaWidth, 2) != Math.Round((double) this.m_layoutArea.ClientActiveArea.Width, 2);
  }

  private bool IsFloatingItemFit(
    Entity ent,
    SizeF size,
    WParagraph ownerParagraph,
    ParagraphItem paraItem,
    float pageHeight,
    RectangleF rect)
  {
    Layouter lcOperator = this.m_lcOperator as Layouter;
    float angle = this.GetAngle(paraItem);
    if ((double) angle != 0.0)
    {
      rect = this.DrawingContext.GetBoundingBoxCoordinates(new RectangleF(rect.X, rect.Y, size.Width, size.Height), angle);
      size = new SizeF(rect.Width, rect.Height);
    }
    switch (ent)
    {
      case WSection _:
        if ((double) size.Height <= (double) this.m_layoutArea.ClientActiveArea.Height || (double) size.Height > (double) this.m_layoutArea.ClientActiveArea.Height && (lcOperator.IsLayoutingHeaderFooter || this.IsForceFitLayout))
          return true;
        break;
      case WTable _:
        if ((double) size.Height <= (double) this.m_layoutArea.ClientActiveArea.Height && (paraItem is WPicture && !this.LayoutInfo.IsVerticalText || !(paraItem is WPicture)) || (paraItem is WPicture && this.LayoutInfo.IsVerticalText || !(paraItem is WPicture)) && (double) size.Height <= (double) this.m_layoutArea.ClientActiveArea.Width || (double) size.Height > (double) lcOperator.ClientLayoutArea.Height && !this.LayoutInfo.IsClipped && this.IsForceFitLayout)
          return true;
        break;
    }
    return false;
  }

  private float GetAngle(ParagraphItem paraItem)
  {
    switch (paraItem)
    {
      case WPicture _:
        return (paraItem as WPicture).Rotation;
      case Shape _:
        return (paraItem as Shape).Rotation;
      case GroupShape _:
        return (paraItem as GroupShape).Rotation;
      case WTextBox _:
        return (paraItem as WTextBox).TextBoxFormat.Rotation;
      default:
        return 0.0f;
    }
  }

  private float GetEmptyTextRangeHeight()
  {
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    if (ownerParagraph == null)
      return 0.0f;
    float num = Math.Abs(ownerParagraph.ParagraphFormat.LineSpacing);
    float height = ownerParagraph.GetHeight(ownerParagraph, this.LeafWidget as ParagraphItem);
    return ownerParagraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.Exactly || ownerParagraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.AtLeast && (double) num > (double) height ? num : height;
  }

  private Entity GetBaseEntity(ParagraphItem entity, WParagraph ownerParagraph)
  {
    bool isInCell = ownerParagraph != null && ownerParagraph.IsInCell;
    Entity ent = (Entity) entity;
    while (!(ent is WSection) && (!(ent is WTable) && !(ent is WTextBox) || !this.IsFitLeafWidgetInContainerHeight(entity, isInCell, ent is WTextBox, ent)) && !(ent is HeaderFooter) && ent.Owner != null)
      ent = ent.Owner;
    return ent;
  }

  private bool IsTextRangeFitInClientActiveArea(SizeF size)
  {
    bool flag = false;
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    Entity baseEntity = this.GetBaseEntity((Entity) ownerParagraph);
    CellLayoutInfo layoutInfo = ownerParagraph.IsInCell ? (ownerParagraph.GetOwnerEntity() as IWidget).LayoutInfo as CellLayoutInfo : (CellLayoutInfo) null;
    if ((this.LayoutInfo.IsClipped && ownerParagraph.IsInCell && layoutInfo != null && layoutInfo.IsRowMergeStart && layoutInfo.IsRowMergeEnd || this.LayoutInfo.IsClipped && ownerParagraph.IsInCell && layoutInfo != null && !layoutInfo.IsRowMergeStart || this.LayoutInfo.IsClipped && !ownerParagraph.IsInCell) && (double) size.Width <= this.m_layoutArea.Width || this.IsTextRangeNeedToFit())
      flag = true;
    float height = (this.m_lcOperator as Layouter).ClientLayoutArea.Height;
    if (ownerParagraph != null && !ownerParagraph.IsInCell)
    {
      switch (baseEntity)
      {
        case WTextBox _:
        case Shape _:
        case GroupShape _:
          break;
        default:
          if ((double) size.Height > (double) height && (double) this.m_layoutArea.ClientActiveArea.Height > 0.0)
          {
            flag = true;
            break;
          }
          break;
      }
    }
    return flag;
  }

  private bool IsTextRangeNeedToFit()
  {
    WTextRange textRange = this.GetTextRange((IWidget) this.LeafWidget);
    TabsLayoutInfo layoutInfo = textRange.m_layoutInfo as TabsLayoutInfo;
    Layouter lcOperator = this.m_lcOperator as Layouter;
    float position = lcOperator.PreviousTab.Position;
    if (!this.IsLeafWidgetIsInCell((ParagraphItem) textRange))
    {
      if (layoutInfo == null)
        return false;
      return (double) position + (double) lcOperator.ClientLayoutArea.Left >= (double) this.m_layoutArea.ClientActiveArea.Right || textRange != null && layoutInfo != null && (double) layoutInfo.m_currTab.Position + (double) lcOperator.ClientLayoutArea.Left >= (double) this.m_layoutArea.ClientActiveArea.Right && !this.IsWord2013(textRange.Document) || (double) lcOperator.ClientLayoutArea.Left >= (double) this.m_layoutArea.ClientActiveArea.Right;
    }
    float cellWidth;
    return textRange != null && ((double) position > (double) (cellWidth = this.DrawingContext.GetCellWidth((ParagraphItem) textRange)) || layoutInfo != null && ((double) layoutInfo.m_currTab.Position > (double) cellWidth || (double) layoutInfo.m_currTab.Position == 0.0 && layoutInfo.m_currTab.Justification == TabJustification.Left && (double) textRange.m_layoutInfo.Size.Width > (double) cellWidth) || (double) cellWidth == 0.0) && !this.IsTabStopBeyondRightMarginExists;
  }

  private bool IsWordFittedByJustification(float availableLineWidth, float nextWordWidth)
  {
    if ((double) availableLineWidth >= (double) nextWordWidth / 2.0)
    {
      Layouter lcOperator = this.m_lcOperator as Layouter;
      float num1 = 0.0f;
      foreach (float lineSpaceWidth in lcOperator.LineSpaceWidths)
        num1 += lineSpaceWidth;
      float num2 = num1 / (float) lcOperator.LineSpaceWidths.Count;
      float num3 = ((double) ((availableLineWidth + num1) / (float) lcOperator.LineSpaceWidths.Count) - (double) num2) / (double) num2 * 100.0 > 33.0 ? num1 / 4f : num1 / 8f;
      if ((double) availableLineWidth + (double) num3 >= (double) nextWordWidth)
      {
        lcOperator.m_effectiveJustifyWidth = num3;
        return true;
      }
    }
    return false;
  }

  private void FitWordAndUpdateState()
  {
    ILeafWidget leafWidget = this.LeafWidget;
    string nextText = this.GetNextText();
    string str = nextText.TrimStart();
    char ch = str.Contains(" ") ? ' ' : '-';
    if (str.Contains(ch.ToString()))
    {
      int length1 = str.IndexOf(ch) + (nextText.Length - str.Length) + 1;
      int length2 = nextText.Length - length1;
      if (this.m_ltWidget != null && this.m_ltWidget.Widget is SplitStringWidget)
        length1 += (this.m_ltWidget.Widget as SplitStringWidget).SplittedText.Length;
      int startIndex1 = 0;
      if (leafWidget is SplitStringWidget)
        startIndex1 = (leafWidget as SplitStringWidget).StartIndex;
      WTextRange currTextRange = this.GetCurrTextRange();
      if (nextText.Length == str.Length && this.m_sptWidget is SplitStringWidget && this.m_ltWidget != null && this.m_ltWidget.Widget is SplitStringWidget)
      {
        int num = (this.m_ltWidget.Widget as SplitStringWidget).StartIndex + (this.m_ltWidget.Widget as SplitStringWidget).Length - 1;
        int startIndex2 = (this.m_sptWidget as SplitStringWidget).StartIndex;
        if (startIndex2 - num > 1)
          length2 += startIndex2 - num - 1;
      }
      SplitStringWidget splitStringWidget1 = new SplitStringWidget((IStringWidget) currTextRange, startIndex1, length1);
      SplitStringWidget splitStringWidget2 = new SplitStringWidget((IStringWidget) currTextRange, startIndex1 + length1, length2);
      this.ForceFitWidget((IWidget) splitStringWidget1, splitStringWidget1.Measure(this.DrawingContext));
      if (splitStringWidget2.SplittedText.Trim(' ') == "" && currTextRange.OwnerParagraph != null && currTextRange.Index == currTextRange.OwnerParagraph.ChildEntities.Count - 1)
      {
        this.m_sptWidget = (IWidget) null;
        this.m_ltState = LayoutState.Fitted;
      }
      else
      {
        this.m_sptWidget = (IWidget) splitStringWidget2;
        this.m_ltState = LayoutState.Splitted;
      }
    }
    else
    {
      this.ForceFitWidget((IWidget) leafWidget, leafWidget.Measure(this.DrawingContext));
      this.m_sptWidget = (IWidget) null;
      this.m_ltState = LayoutState.Fitted;
    }
  }

  private void ForceFitWidget(IWidget widget, SizeF size)
  {
    this.m_ltWidget = new LayoutedWidget(widget);
    this.m_ltWidget.Bounds = new RectangleF(this.m_layoutArea.ClientArea.X, this.m_layoutArea.ClientArea.Y, size.Width, size.Height);
  }

  private void DoWord2013JustificationWordFit(
    WParagraph paragraph,
    float clientWidth,
    Layouter layouter)
  {
    if (layouter.IsWord2013WordFitLayout)
    {
      if ((double) layouter.m_effectiveJustifyWidth <= 0.0 || this.m_ltWidget == null || (double) this.m_ltWidget.Bounds.Width <= (double) clientWidth)
        return;
      layouter.m_effectiveJustifyWidth -= this.m_ltWidget.Bounds.Width - clientWidth;
    }
    else
    {
      if (this.IsNotWord2013Jusitfy(paragraph) || this.GetCurrTextRange() == null)
        return;
      switch (this.m_ltState)
      {
        case LayoutState.NotFitted:
        case LayoutState.Splitted:
          float width = this.m_layoutArea.ClientActiveArea.Width;
          if (this.m_ltWidget != null)
          {
            this.UpdateSpaceWidth(this.m_ltWidget.Widget);
            width -= this.m_ltWidget.Bounds.Width;
          }
          float leadingSpaceWidth = 0.0f;
          float nextWordWidth = this.GetNextWordWidth(ref leadingSpaceWidth);
          float availableLineWidth = width - leadingSpaceWidth;
          if (!this.IsWordFittedByJustification(availableLineWidth, nextWordWidth))
            break;
          layouter.IsWord2013WordFitLayout = true;
          float num1 = this.m_ltWidget == null ? 0.0f : this.m_ltWidget.Bounds.Width;
          this.FitWordAndUpdateState();
          float num2 = availableLineWidth - (this.m_ltWidget.Bounds.Width - num1);
          if ((double) num2 >= 0.0)
            break;
          (this.m_lcOperator as Layouter).m_effectiveJustifyWidth += num2;
          break;
        case LayoutState.Fitted:
          this.UpdateSpaceWidth((IWidget) this.LeafWidget);
          break;
      }
    }
  }

  private string GetNextText()
  {
    return !(this.m_sptWidget is SplitStringWidget) ? (!(this.LeafWidget is SplitStringWidget) ? this.GetCurrTextRange().Text : (this.LeafWidget as SplitStringWidget).SplittedText) : (this.m_sptWidget as SplitStringWidget).SplittedText;
  }

  private bool IsNextWordFound(
    string text,
    Font font,
    WCharacterFormat charFormat,
    ref float nextWordWidth)
  {
    bool flag = true;
    if (text.Contains(" "))
      text = text.Remove(text.IndexOf(' '));
    else if (text.Contains("-"))
      text = text.Remove(text.IndexOf('-'));
    else
      flag = false;
    nextWordWidth += this.DrawingContext.MeasureString(text, font, (StringFormat) null, charFormat, false).Width;
    return flag;
  }

  private float GetNextWordWidth(ref float leadingSpaceWidth)
  {
    float nextWordWidth = 0.0f;
    WTextRange currTextRange = this.GetCurrTextRange();
    Font fontToRender1 = currTextRange.CharacterFormat.GetFontToRender(currTextRange.ScriptType);
    bool flag = true;
    string nextText = this.GetNextText();
    if (!string.IsNullOrEmpty(nextText))
    {
      if (nextText.StartsWith(" "))
        leadingSpaceWidth = this.AddLeadingSpaces(ref nextText, fontToRender1);
      if (this.IsNextWordFound(nextText, fontToRender1, currTextRange.CharacterFormat, ref nextWordWidth))
        return nextWordWidth;
      flag = false;
    }
    else
    {
      switch (currTextRange)
      {
        case WCheckBox _:
        case WDropDownFormField _:
        case WTextFormField _:
          return this.LeafWidget.Measure(this.DrawingContext).Width;
      }
    }
    IEntity nextSibling = currTextRange.NextSibling;
    while (nextSibling != null)
    {
      if (nextSibling is BookmarkStart || nextSibling is BookmarkEnd || (nextSibling as IWidget).LayoutInfo.IsSkip)
        nextSibling = nextSibling.NextSibling;
      else if (nextSibling is WTextRange)
      {
        WTextRange wtextRange = nextSibling as WTextRange;
        Font fontToRender2 = wtextRange.CharacterFormat.GetFontToRender(wtextRange.ScriptType);
        string text;
        if (flag)
        {
          text = wtextRange.Text;
          leadingSpaceWidth = this.AddLeadingSpaces(ref text, fontToRender2);
          flag = false;
        }
        else
          text = wtextRange.Text;
        if (!this.IsNextWordFound(text, fontToRender2, wtextRange.CharacterFormat, ref nextWordWidth))
          nextSibling = nextSibling.NextSibling;
        else
          break;
      }
      else
        break;
    }
    return nextWordWidth;
  }

  private float AddLeadingSpaces(ref string text, Font font)
  {
    float width = this.DrawingContext.MeasureString(" ", font, (StringFormat) null).Width;
    float num = 0.0f;
    while (text.Length > 0 && text[0] == ' ')
    {
      text = text.Remove(0, 1);
      (this.m_lcOperator as Layouter).LineSpaceWidths.Add(width);
      num += width;
    }
    return num;
  }

  private void UpdateSpaceWidth(IWidget leafWidget)
  {
    string str = (string) null;
    Font font = (Font) null;
    switch (leafWidget)
    {
      case WTextRange _:
        WTextRange wtextRange = leafWidget as WTextRange;
        str = wtextRange.Text;
        font = wtextRange.CharacterFormat.GetFontToRender(wtextRange.ScriptType);
        break;
      case SplitStringWidget _:
        WTextRange realStringWidget = (leafWidget as SplitStringWidget).RealStringWidget as WTextRange;
        str = (leafWidget as SplitStringWidget).SplittedText;
        font = realStringWidget.CharacterFormat.GetFontToRender(realStringWidget.ScriptType);
        break;
    }
    Layouter lcOperator = this.m_lcOperator as Layouter;
    if (str == null || !str.Contains(" "))
      return;
    float width = this.DrawingContext.MeasureString(" ", font, (StringFormat) null).Width;
    foreach (char ch in str)
    {
      if (ch == ' ')
        lcOperator.LineSpaceWidths.Add(width);
    }
  }

  internal LayoutedWidget WordLayout(
    RectangleF rect,
    SizeF size,
    WTextRange textRange,
    WParagraph paragraph)
  {
    Layouter lcOperator = this.m_lcOperator as Layouter;
    float clientWidth = this.IsTabStopBeyondRightMarginExists ? 1584f : textRange.GetClientWidth(this.DrawingContext, lcOperator.ClientLayoutArea.Width);
    string text = this.GetText();
    if (lcOperator.m_canSplitbyCharacter && (text.Contains(" ") || text.Contains('-'.ToString()) || text.Contains('\u001F'.ToString()) || lcOperator.m_canSplitByTab))
      lcOperator.m_canSplitbyCharacter = false;
    IWidget nextSibling = this.GetNextSibling(textRange);
    if (lcOperator.m_canSplitbyCharacter && !lcOperator.m_canSplitByTab && textRange.m_layoutInfo is TabsLayoutInfo)
    {
      lcOperator.m_canSplitByTab = true;
      if (!lcOperator.IsFirstItemInLine)
        lcOperator.m_canSplitbyCharacter = false;
      else
        lcOperator.m_canSplitByTab = false;
    }
    if ((double) size.Width > (double) clientWidth)
    {
      if (lcOperator.m_canSplitbyCharacter || this.DrawingContext.IsUnicodeText(this.GetText()) || text.Contains(" ") || text.Contains('-'.ToString()) || text.Contains('\u001F'.ToString()))
        return (LayoutedWidget) null;
      this.SplitByWord(this.LeafWidget as ISplitLeafWidget, size, textRange, clientWidth, false);
      this.DoLayoutAfter();
      return this.m_ltWidget;
    }
    if (textRange != null && textRange.CharacterRange == CharacterRangeType.RTL && paragraph.ParagraphFormat.Bidi && !textRange.CharacterFormat.Bidi && nextSibling is WTextRange && (nextSibling as WTextRange).Text != string.Empty && (int) (nextSibling as WTextRange).Text[0] == (int) ControlChar.SpaceChar && !lcOperator.IsFirstItemInLine && this.TryFit(size))
    {
      WTextRange nextText1 = nextSibling as WTextRange;
      SplitStringWidget leafWidget = this.LeafWidget is SplitStringWidget ? this.LeafWidget as SplitStringWidget : (SplitStringWidget) null;
      string str1 = leafWidget != null ? leafWidget.SplittedText : textRange.Text;
      string str2 = new string(str1.ToCharArray());
      bool flag1 = false;
      IWidget textRange1 = (IWidget) nextText1;
      float nextsiblingWidth = 0.0f;
      if (nextText1.Text.Trim() == string.Empty)
      {
        nextsiblingWidth = (nextSibling as ILeafWidget).Measure(this.DrawingContext).Width;
        for (; textRange1 is WTextRange; textRange1 = this.GetNextSibling(textRange1 as WTextRange))
        {
          WTextRange nextText2 = textRange1 as WTextRange;
          if (nextText2.Text.Trim() == string.Empty)
          {
            nextsiblingWidth += (nextSibling as ILeafWidget).Measure(this.DrawingContext).Width;
          }
          else
          {
            if ((int) nextText2.Text[0] == (int) ControlChar.SpaceChar)
            {
              this.UpdateSpaceWidth(nextText2, ref nextsiblingWidth);
              break;
            }
            break;
          }
        }
      }
      else
        this.UpdateSpaceWidth(nextText1, ref nextsiblingWidth);
      WTextRange wtextRange = textRange1 as WTextRange;
      for (int index = 0; wtextRange != null && index < wtextRange.TextLength; ++index)
      {
        if (TextSplitter.IsRTLChar(wtextRange.Text[index]))
          flag1 = true;
        else if ((int) wtextRange.Text[index] != (int) ControlChar.SpaceChar)
          break;
      }
      if ((double) size.Width + (double) nextsiblingWidth > (double) this.m_layoutArea.ClientActiveArea.Width && flag1)
      {
        bool flag2 = true;
        for (int index = str2.Length - 1; index >= 0; --index)
        {
          if ((int) str2[index] != (int) ControlChar.SpaceChar)
          {
            flag2 = false;
            str2 = str2.Remove(index);
          }
          else if (flag2)
            str2 = str2.Remove(index);
          else
            break;
        }
        string str3 = str1.Substring(str2.Length);
        ISplitLeafWidget[] splitLeafWidgetArray = new ISplitLeafWidget[2]
        {
          (ISplitLeafWidget) new SplitStringWidget(this.LeafWidget as IStringWidget, -1, -1),
          (ISplitLeafWidget) new SplitStringWidget(this.LeafWidget as IStringWidget, 0, textRange.TextLength)
        };
        if (str2 != string.Empty && str3 != string.Empty)
        {
          splitLeafWidgetArray[0] = (ISplitLeafWidget) new SplitStringWidget(this.LeafWidget as IStringWidget, 0, str2.Length);
          splitLeafWidgetArray[1] = (ISplitLeafWidget) new SplitStringWidget(this.LeafWidget as IStringWidget, str2.Length, str3.Length);
        }
        size = splitLeafWidgetArray[0].Measure(this.DrawingContext);
        this.FitWidget(size, (IWidget) splitLeafWidgetArray[0], false, 0.0f, 0.0f, false);
        this.m_sptWidget = (IWidget) splitLeafWidgetArray[1];
        this.m_ltState = LayoutState.Splitted;
        return this.m_ltWidget;
      }
    }
    TabsLayoutInfo tabsLayoutInfo = (TabsLayoutInfo) null;
    if (nextSibling != null)
      tabsLayoutInfo = nextSibling.LayoutInfo as TabsLayoutInfo;
    if ((double) size.Width > 0.0 || (double) size.Width == 0.0 && textRange.m_layoutInfo is TabsLayoutInfo && (textRange.m_layoutInfo as TabsLayoutInfo).CurrTabJustification == TabJustification.Right && tabsLayoutInfo == null)
    {
      if (this.IsTextRangeNeedToFit())
        return (LayoutedWidget) null;
      if (this.DrawingContext.IsUnicodeText(this.GetText()))
      {
        this.SplitUnicodeTextByWord(textRange, rect, size, clientWidth);
        this.DoLayoutAfter();
        return this.m_ltWidget;
      }
      if (this.IsTextNeedToBeSplitted(size, rect, textRange) && this.IsTextNeedToBeSplittedByWord(size, rect, (IWidget) textRange, clientWidth))
      {
        this.SplitByWord(this.LeafWidget as ISplitLeafWidget, size, textRange, clientWidth, false);
        this.DoLayoutAfter();
        return this.m_ltWidget;
      }
    }
    else if (tabsLayoutInfo != null && (double) size.Width < (double) rect.Width)
    {
      tabsLayoutInfo.GetNextTabPosition((double) this.m_layoutArea.ClientArea.X);
      TabsLayoutInfo layoutInfo = textRange.m_layoutInfo is TabsLayoutInfo ? textRange.m_layoutInfo as TabsLayoutInfo : (TabsLayoutInfo) null;
      if ((double) tabsLayoutInfo.m_currTab.Position > (double) this.ClientLayoutAreaRight)
      {
        if (this.IsTextRangeNeedToFit())
          return (LayoutedWidget) null;
        if (this.IsTextNeedToBeSplitted(size, rect, textRange) && this.IsTextNeedToBeSplittedByWord(size, new RectangleF(rect.X, rect.Y, 1584f - rect.Width, rect.Height), nextSibling, 1584f))
        {
          this.SplitByWord(this.LeafWidget as ISplitLeafWidget, size, textRange, this.m_layoutArea.ClientArea.Width, false);
          this.DoLayoutAfter();
          if (this.m_ltWidget.Widget is SplitStringWidget && !(this.m_ltWidget.Widget as SplitStringWidget).SplittedText.Equals(""))
            return this.m_ltWidget;
        }
        else if (lcOperator.m_canSplitByTab && textRange != null && layoutInfo != null && (double) layoutInfo.m_currTab.Position + (double) lcOperator.ClientLayoutArea.Left > (double) this.m_layoutArea.ClientActiveArea.Right && this.IsWord2013(textRange.Document))
        {
          this.SplitByWord(this.LeafWidget as ISplitLeafWidget, size, textRange, clientWidth, false);
          this.DoLayoutAfter();
          return this.m_ltWidget;
        }
      }
    }
    return this.m_ltWidget = (LayoutedWidget) null;
  }

  private void UpdateSpaceWidth(WTextRange nextText, ref float nextsiblingWidth)
  {
    string[] strArray = nextText.Text.Split(' ');
    for (int index = 0; index < strArray.Length && strArray[index] == string.Empty; ++index)
      nextsiblingWidth += this.DrawingContext.MeasureTextRange(nextText, " ").Width;
  }

  private void SplitUnicodeTextByWord(
    WTextRange textRange,
    RectangleF rect,
    SizeF size,
    float clientWidth)
  {
    if (!(this.GetNextSibling(textRange) is WTextRange nextSibling) || nextSibling.Text.Length <= 0 || !this.DrawingContext.IsBeginCharacter(nextSibling.Text[0]))
      return;
    float width = this.DrawingContext.MeasureTextRange(nextSibling, nextSibling.Text[0].ToString()).Width;
    string text = this.GetText();
    int length = text.Length;
    if (!this.IsBeginCJKCharacter(text, ref length) || length <= 0 || (double) size.Width + (double) width <= (double) rect.Width || !this.IsUnicodeTextNeedToBeSplittedByWord(size, rect, textRange, clientWidth))
      return;
    IStringWidget strWidget = (IStringWidget) textRange;
    ISplitLeafWidget[] splitLeafWidgetArray = new ISplitLeafWidget[2];
    if (length == 0)
    {
      splitLeafWidgetArray[0] = (ISplitLeafWidget) new SplitStringWidget(strWidget, -1, -1);
      splitLeafWidgetArray[1] = (ISplitLeafWidget) new SplitStringWidget(strWidget, 0, textRange.Text.Length);
    }
    else
    {
      splitLeafWidgetArray[0] = (ISplitLeafWidget) new SplitStringWidget(strWidget, 0, length - 1);
      splitLeafWidgetArray[1] = (ISplitLeafWidget) new SplitStringWidget(strWidget, length - 1, -1);
    }
    this.m_ltState = LayoutState.NotFitted;
    if (splitLeafWidgetArray == null)
      return;
    size = splitLeafWidgetArray[0].Measure(this.DrawingContext);
    if (!this.TryFit(size))
      size.Width = this.m_layoutArea.ClientArea.Width;
    this.FitWidget(size, (IWidget) splitLeafWidgetArray[0], false, 0.0f, 0.0f, false);
    this.m_sptWidget = (IWidget) splitLeafWidgetArray[1];
    this.m_ltState = LayoutState.Splitted;
  }

  private bool IsUnicodeTextNeedToBeSplittedByWord(
    SizeF size,
    RectangleF rect,
    WTextRange textRange,
    float clientWidth)
  {
    bool beSplittedByWord = false;
    if (this.GetNextSibling(textRange) is WTextRange nextSibling && (double) size.Width < (double) rect.Width)
    {
      string text = nextSibling.Text;
      float nextTextRangeWidth = this.GetUnicodeNextTextRangeWidth(nextSibling, ref text, size, rect);
      if ((double) size.Width + (double) nextTextRangeWidth > (double) rect.Width && (double) clientWidth >= (double) rect.Width && (double) nextTextRangeWidth < (double) clientWidth)
        beSplittedByWord = true;
    }
    return beSplittedByWord;
  }

  private string GetText()
  {
    if (this.LeafWidget is WField)
    {
      WField leafWidget = this.LeafWidget as WField;
      if (leafWidget.FieldType == FieldType.FieldExpression || leafWidget.FieldType == FieldType.FieldPageRef || leafWidget.FieldType == FieldType.FieldRef || this.LeafWidget.Measure(this.DrawingContext) == (SizeF) Size.Empty)
        return string.Empty;
    }
    return !(this.LeafWidget is WTextRange) ? (this.LeafWidget as SplitStringWidget).SplittedText : (this.LeafWidget as WTextRange).Text;
  }

  private bool IsTextNeedToBeSplittedByWord(
    SizeF size,
    RectangleF rect,
    IWidget iwidget,
    float clientWidth)
  {
    WTextRange textRange = iwidget is WTextRange ? iwidget as WTextRange : (WTextRange) null;
    bool beSplittedByWord = false;
    IWidget nextSibling = this.GetNextSibling(textRange);
    if ((nextSibling is WTextRange wtextRange && !this.StartsWithExt(wtextRange.Text, " ") && !this.GetText().EndsWith(" ") && !(((IWidget) wtextRange).LayoutInfo is TabsLayoutInfo) || nextSibling != null && nextSibling.LayoutInfo is FootnoteLayoutInfo) && (double) size.Width <= (double) rect.Width)
    {
      string nextSiblingText = !(nextSibling is WFootnote) ? wtextRange.Text : (nextSibling.LayoutInfo as FootnoteLayoutInfo).FootnoteID;
      float nextTextRangeWidth = this.GetNextTextRangeWidth(nextSibling, ref nextSiblingText, size, rect);
      if (!this.DrawingContext.IsUnicodeText(nextSiblingText) && !(this.m_lcOperator as Layouter).m_canSplitbyCharacter && (double) size.Width + (double) nextTextRangeWidth > (double) rect.Width && Math.Round((double) clientWidth, 2) >= Math.Round((double) rect.Width, 2))
        beSplittedByWord = true;
    }
    return beSplittedByWord;
  }

  private float GetWidthToFitText(WTextRange textRange, float nextTextRangeWidth)
  {
    string text = "";
    string[] strArray1 = this.GetText().Split(' ');
    if (strArray1.Length == 1)
      return textRange.Text != strArray1[0] ? this.DrawingContext.MeasureTextRange(textRange, strArray1[0]).Width + nextTextRangeWidth : ((IWidget) textRange).LayoutInfo.Size.Width + nextTextRangeWidth;
    for (int index = 0; index < strArray1.Length - 1; ++index)
      text = $"{text}{strArray1[index]} ";
    string[] strArray2 = strArray1[strArray1.Length - 1].Split('-');
    for (int index = 0; index < strArray2.Length - 1; ++index)
      text = text + strArray2[index] + '-'.ToString();
    return textRange.Text != text ? this.DrawingContext.MeasureTextRange(textRange, text).Width : ((IWidget) textRange).LayoutInfo.Size.Width;
  }

  private float GetNextTextRangeWidth(
    IWidget nextSiblingTextRange,
    ref string nextSiblingText,
    SizeF size,
    RectangleF rect)
  {
    WTextRange wtextRange = nextSiblingTextRange as WTextRange;
    if (nextSiblingTextRange is WFootnote)
      wtextRange = (nextSiblingTextRange.LayoutInfo as LayoutFootnoteInfoImpl).TextRange;
    SizeF sizeNext = new SizeF();
    bool beMeasure = this.IsNextSibligSizeNeedToBeMeasure(ref sizeNext, nextSiblingTextRange, rect, size);
    while (beMeasure && this.IsLeafWidgetNextSiblingIsTextRange(wtextRange) && (double) (size + sizeNext).Width < (double) rect.Width)
    {
      wtextRange = this.GetNextSibling(wtextRange) as WTextRange;
      if (this.IsNextSibligSizeNeedToBeMeasure(ref sizeNext, (IWidget) wtextRange, rect, size))
        nextSiblingText += wtextRange.Text;
      else
        break;
    }
    return sizeNext.Width;
  }

  private float GetUnicodeNextTextRangeWidth(
    WTextRange nextSiblingTextRange,
    ref string nextSiblingText,
    SizeF size,
    RectangleF rect)
  {
    SizeF sizeNext = new SizeF();
    bool beMeasure = this.IsUnicodeNextSibligSizeNeedToBeMeasure(ref sizeNext, nextSiblingTextRange, rect, size);
    while (beMeasure && this.IsLeafWidgetNextSiblingIsTextRange(nextSiblingTextRange) && (double) (size + sizeNext).Width < (double) rect.Width)
    {
      nextSiblingTextRange = this.GetNextSibling(nextSiblingTextRange) as WTextRange;
      if (this.IsUnicodeNextSibligSizeNeedToBeMeasure(ref sizeNext, nextSiblingTextRange, rect, size))
        nextSiblingText += nextSiblingTextRange.Text;
      else
        break;
    }
    return sizeNext.Width;
  }

  private bool IsNextSibligSizeNeedToBeMeasure(
    ref SizeF sizeNext,
    IWidget nextSiblingwidget,
    RectangleF rect,
    SizeF size)
  {
    WTextRange txtRange = nextSiblingwidget as WTextRange;
    string str;
    if (nextSiblingwidget is WFootnote)
    {
      txtRange = (nextSiblingwidget.LayoutInfo as LayoutFootnoteInfoImpl).TextRange;
      str = (nextSiblingwidget.LayoutInfo as LayoutFootnoteInfoImpl).FootnoteID;
    }
    else
      str = txtRange.Text;
    if (txtRange != null)
    {
      if (str.Contains(" ") || str.Contains('-'.ToString()) || str.Contains('\u001F'.ToString()) && (double) size.Width + (double) sizeNext.Width + (double) this.DrawingContext.MeasureString("-", nextSiblingwidget.LayoutInfo.Font.GetFont(txtRange.Document), new StringFormat(this.DrawingContext.StringFormt)).Width < (double) rect.Width || ((IWidget) txtRange).LayoutInfo is TabsLayoutInfo)
      {
        float width = ((IWidget) txtRange).LayoutInfo.Size.Width;
        if (str != str.Split(' ')[0])
          width = this.DrawingContext.MeasureTextRange(txtRange, str.Split(' ')[0]).Width;
        if ((double) size.Width + (double) sizeNext.Width + (double) width > (double) rect.Width && str.Contains('-'.ToString()))
        {
          if (str != str.Split('-')[0] + '-'.ToString())
            width = this.DrawingContext.MeasureTextRange(txtRange, str.Split('-')[0] + '-'.ToString()).Width;
        }
        sizeNext.Width += width;
        return false;
      }
      sizeNext += nextSiblingwidget.LayoutInfo.Size;
    }
    return true;
  }

  private bool IsUnicodeNextSibligSizeNeedToBeMeasure(
    ref SizeF sizeNext,
    WTextRange nextSiblingTextRange,
    RectangleF rect,
    SizeF size)
  {
    int index1 = 0;
    if (!this.IsBeginCJKCharacter(nextSiblingTextRange.Text, ref index1))
      return false;
    float width1 = ((IWidget) nextSiblingTextRange).LayoutInfo.Size.Width;
    string empty = string.Empty;
    for (int index2 = index1; index2 < nextSiblingTextRange.Text.Length && this.DrawingContext.IsBeginCharacter(nextSiblingTextRange.Text[index2]); ++index2)
      empty += (string) (object) nextSiblingTextRange.Text[index2];
    float width2 = this.DrawingContext.MeasureTextRange(nextSiblingTextRange, empty).Width;
    sizeNext.Width += width2;
    return empty == nextSiblingTextRange.Text;
  }

  private bool IsBeginCJKCharacter(string text, ref int index)
  {
    for (int index1 = 0; index1 < text.Length; ++index1)
    {
      if (this.DrawingContext.IsBeginCharacter(text[index1]))
      {
        index = index1;
        return true;
      }
    }
    return false;
  }

  private IWidget GetValidInlineNextSibling(ParagraphItem paragraphItem)
  {
    IEntity inlineNextSibling = paragraphItem.NextSibling;
    if (inlineNextSibling is WOleObject)
      inlineNextSibling = (IEntity) (inlineNextSibling as WOleObject).OlePicture;
    for (; inlineNextSibling != null; inlineNextSibling = inlineNextSibling.NextSibling)
    {
      if ((inlineNextSibling as IWidget).LayoutInfo.IsSkip || (inlineNextSibling as Entity).IsFloatingItem(false))
        continue;
      switch (inlineNextSibling)
      {
        case Break _:
        case InlineContentControl _:
        case BookmarkStart _:
        case BookmarkEnd _:
          continue;
        default:
          goto label_6;
      }
    }
label_6:
    return inlineNextSibling as IWidget;
  }

  private IWidget GetNextSibling(WTextRange textRange)
  {
    WParagraph ownerParagraph = this.GetOwnerParagraph();
    IWidget nextSibling = ownerParagraph.GetNextSibling((IWidget) textRange);
    while (nextSibling != null && (!(nextSibling is WTextRange) || nextSibling is WField && (nextSibling as WField).FieldType == FieldType.FieldHyperlink || nextSibling.LayoutInfo.IsSkip) && !(nextSibling is WFootnote) && (nextSibling is BookmarkStart || nextSibling is BookmarkEnd || nextSibling is WFieldMark || nextSibling is SplitStringWidget || nextSibling.LayoutInfo.IsSkip || nextSibling is WField && (nextSibling as WField).FieldType == FieldType.FieldHyperlink))
      nextSibling = ownerParagraph.GetNextSibling(nextSibling);
    return nextSibling;
  }

  internal bool IsLeafWidgetIsInCell(ParagraphItem paraItem)
  {
    bool flag = false;
    WParagraph wparagraph = (WParagraph) null;
    if (paraItem != null)
      wparagraph = paraItem.GetOwnerParagraphValue();
    if (wparagraph != null && wparagraph.IsInCell)
      flag = true;
    return flag;
  }

  internal bool IsLeafWidgetIsInTextBox(ParagraphItem paraItem)
  {
    bool flag = false;
    WParagraph wparagraph = (WParagraph) null;
    if (paraItem != null)
      wparagraph = paraItem.GetOwnerParagraphValue();
    if (wparagraph != null && wparagraph.OwnerBase != null && wparagraph.OwnerBase.OwnerBase is WTextBox)
      flag = true;
    return flag;
  }

  internal bool IsLeafWidgetNextSiblingIsTextRange(WTextRange textRange)
  {
    return this.GetNextSibling(textRange) is WTextRange nextSibling && nextSibling != null;
  }

  internal void SplitByWord(
    ISplitLeafWidget splitLeafWidget,
    SizeF size,
    WTextRange textRange,
    float clientWidth,
    bool isWrapTextBasedOnAbsTable)
  {
    if (splitLeafWidget != null && (double) size.Height <= (double) this.m_layoutArea.ClientArea.Height)
    {
      if (this.LayoutInfo is TabsLayoutInfo)
      {
        this.m_ltWidget = new LayoutedWidget((IWidget) splitLeafWidget);
        if (((TabsLayoutInfo) this.LayoutInfo).CurrTabLeader == TabLeader.NoLeader)
          this.m_ltWidget.Bounds = new RectangleF(this.m_layoutArea.ClientArea.X, this.m_layoutArea.ClientArea.Y, size.Width, size.Height);
        this.m_sptWidget = (IWidget) splitLeafWidget;
        this.m_ltState = LayoutState.Splitted;
      }
      else
      {
        IStringWidget strWidget = (IStringWidget) textRange;
        SplitStringWidget leafWidget1 = this.LeafWidget as SplitStringWidget;
        WTextRange leafWidget2 = this.LeafWidget as WTextRange;
        string str1 = "";
        string[] strArray1 = this.GetText().Split(' ');
        ISplitLeafWidget[] splitLeafWidgetArray1 = new ISplitLeafWidget[2];
        if (strArray1.Length == 1 && (double) size.Width > (double) clientWidth && (this.m_lcOperator as Layouter).m_canSplitbyCharacter)
        {
          int startIndex;
          int length;
          if (leafWidget2 != null)
          {
            startIndex = 0;
            length = leafWidget2.Text.IndexOf(' ');
          }
          else
          {
            startIndex = leafWidget1.StartIndex;
            length = leafWidget1.SplittedText.IndexOf(' ');
          }
          splitLeafWidgetArray1[0] = (ISplitLeafWidget) new SplitStringWidget(strWidget, startIndex, length);
          splitLeafWidgetArray1[1] = (ISplitLeafWidget) new SplitStringWidget(strWidget, -1, -1);
        }
        else if (strArray1[strArray1.Length - 1].EndsWith('-'.ToString()) || strArray1[strArray1.Length - 1].EndsWith('\u001F'.ToString()))
        {
          int startIndex;
          int length;
          if (leafWidget2 != null)
          {
            startIndex = 0;
            if (leafWidget2.Text == null)
              startIndex = length = int.MinValue;
            else
              length = leafWidget2.Text.Length;
          }
          else
          {
            startIndex = leafWidget1.StartIndex;
            if (leafWidget1.SplittedText == null)
              startIndex = length = int.MinValue;
            else
              length = leafWidget1.SplittedText.Length;
          }
          splitLeafWidgetArray1[0] = (ISplitLeafWidget) new SplitStringWidget(strWidget, startIndex, length);
          splitLeafWidgetArray1[1] = (ISplitLeafWidget) new SplitStringWidget(strWidget, -1, -1);
        }
        else
        {
          for (int index = 0; index < strArray1.Length - 1; ++index)
            str1 = $"{str1}{strArray1[index]} ";
          string str2 = strArray1[strArray1.Length - 1].Contains('\u001F'.ToString()) ? '\u001F'.ToString() : '-'.ToString();
          string[] strArray2 = strArray1[strArray1.Length - 1].Split(str2[0]);
          for (int index = 0; index < strArray2.Length - 1; ++index)
            str1 = str1 + strArray2[index] + str2;
          if (isWrapTextBasedOnAbsTable)
          {
            int startIndex;
            int length;
            if (leafWidget2 != null)
            {
              startIndex = 0;
              if (leafWidget2.Text == null)
                startIndex = length = int.MinValue;
              else
                length = leafWidget2.Text.Length;
            }
            else
            {
              startIndex = leafWidget1.StartIndex;
              if (leafWidget1.SplittedText == null)
                startIndex = length = int.MinValue;
              else
                length = leafWidget1.SplittedText.Length;
            }
            splitLeafWidgetArray1[0] = (ISplitLeafWidget) new SplitStringWidget(strWidget, -1, -1);
            splitLeafWidgetArray1[1] = (ISplitLeafWidget) new SplitStringWidget(strWidget, startIndex, length);
          }
          else
          {
            int startIndex1;
            int length1;
            if (leafWidget2 != null)
            {
              startIndex1 = 0;
              length1 = str1.Length;
            }
            else
            {
              startIndex1 = leafWidget1.StartIndex;
              length1 = str1.Length;
            }
            splitLeafWidgetArray1[0] = (ISplitLeafWidget) new SplitStringWidget(strWidget, startIndex1, length1);
            int startIndex2;
            int length2;
            if (leafWidget2 != null)
            {
              startIndex2 = str1.Length;
              length2 = strArray2[strArray2.Length - 1].Length;
            }
            else
            {
              startIndex2 = leafWidget1.StartIndex + str1.Length;
              length2 = strArray2[strArray2.Length - 1].Length;
            }
            splitLeafWidgetArray1[1] = (ISplitLeafWidget) new SplitStringWidget(strWidget, startIndex2, length2);
          }
        }
        ISplitLeafWidget[] splitLeafWidgetArray2 = splitLeafWidgetArray1;
        this.m_ltState = LayoutState.NotFitted;
        if (splitLeafWidgetArray2 == null)
          return;
        size = splitLeafWidgetArray2[0].Measure(this.DrawingContext);
        if (!this.TryFit(size))
          size.Width = this.m_layoutArea.ClientArea.Width;
        this.FitWidget(size, (IWidget) splitLeafWidgetArray2[0], false, 0.0f, 0.0f, false);
        this.m_sptWidget = (IWidget) splitLeafWidgetArray2[1];
        this.m_ltState = LayoutState.Splitted;
      }
    }
    else
    {
      this.m_ltState = LayoutState.NotFitted;
      this.IsVerticalNotFitted = (double) size.Height > (double) this.m_layoutArea.ClientArea.Height;
    }
  }

  internal bool IsTextNeedToBeSplitted(SizeF size, RectangleF rect, WTextRange textRange)
  {
    WTextRange previousSibling = this.DrawingContext.GetPreviousSibling(textRange) as WTextRange;
    IWidget nextSibling = this.GetNextSibling(textRange);
    WTextRange wtextRange = nextSibling as WTextRange;
    if (nextSibling is WFootnote)
      wtextRange = (nextSibling.LayoutInfo as LayoutFootnoteInfoImpl).TextRange;
    bool beSplitted = this.IsTextNeedToBeSplitted(previousSibling, wtextRange);
    if (wtextRange != null && wtextRange.Text.Contains(" ") && wtextRange.Text.IndexOf(" ") != -1)
    {
      SizeF sizeF = this.DrawingContext.MeasureTextRange(wtextRange, wtextRange.Text.Substring(0, wtextRange.Text.IndexOf(" ") + 1));
      beSplitted = (double) size.Width <= (double) rect.Width && (double) (size + sizeF).Width > (double) rect.Width;
    }
    return beSplitted;
  }

  internal bool IsTextNeedToBeSplitted(
    WTextRange prevSiblingTextRange,
    WTextRange nextSiblingTextRange)
  {
    bool beSplitted = true;
    string text = this.GetText();
    if (text.EndsWith(" ") || text.EndsWith(",") || nextSiblingTextRange != null && this.StartsWithExt(nextSiblingTextRange.Text, " ") || prevSiblingTextRange != null && prevSiblingTextRange != null && prevSiblingTextRange.Text.EndsWith("") && this.StartsWithExt(text, "") && text.EndsWith("") && (nextSiblingTextRange == null || !this.StartsWithExt(nextSiblingTextRange.Text, "") || !nextSiblingTextRange.Text.EndsWith("")))
      beSplitted = false;
    return beSplitted;
  }

  private WParagraph GetOwnerParagraph()
  {
    ParagraphItem paragraphItem = this.LeafWidget is SplitStringWidget ? (ParagraphItem) ((this.LeafWidget as SplitStringWidget).RealStringWidget as WTextRange) : (this.LeafWidget is ParagraphItem ? this.LeafWidget as ParagraphItem : (ParagraphItem) null);
    if (paragraphItem is WTextRange && (paragraphItem as WTextRange).Owner == null)
      return (paragraphItem as WTextRange).CharacterFormat.BaseFormat.OwnerBase as WParagraph;
    return paragraphItem?.GetOwnerParagraphValue();
  }

  private bool CheckWrappingStyleForWrapping(TextWrappingStyle textWrappingStyle)
  {
    return textWrappingStyle == TextWrappingStyle.InFrontOfText || textWrappingStyle == TextWrappingStyle.Behind;
  }

  private bool IsNeedToWrapLeafWidget(WParagraph ownerPara, Layouter layouter)
  {
    return layouter.FloatingItems.Count > 0 && this.IsNeedToWrap && ownerPara != null && (!layouter.IsLayoutingHeaderFooter || ownerPara.IsInCell || ownerPara.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013) && (!(this.LeafWidget is WPicture) && !(this.LeafWidget is WChart) && !(this.LeafWidget is Shape) && !(this.LeafWidget is WTextBox) && !(this.LeafWidget is GroupShape) || !this.CheckWrappingStyleForWrapping((this.LeafWidget as ParagraphItem).GetTextWrappingStyle())) && !this.IsLeafWidgetOwnerIsTextBox() && !this.IsInFrame(ownerPara) && !this.IsInFootnote(ownerPara);
  }

  private bool IsNeedToWrapParaMarkToRightSide(
    WParagraph ownerPara,
    RectangleF textWrappingBounds,
    float bottom,
    Layouter layouter,
    RectangleF rect,
    TextWrappingType textWrappingType,
    TextWrappingStyle textWrappingStyle,
    float minimumWidthRequired)
  {
    if (ownerPara == null || ownerPara.IsInCell || (double) textWrappingBounds.Right >= (double) layouter.ClientLayoutArea.Right - (double) minimumWidthRequired || !(this.LeafWidget is Break) || (this.LeafWidget as Break).BreakType != BreakType.LineBreak || textWrappingType != TextWrappingType.Both || (double) textWrappingBounds.X <= (double) rect.X || ((double) textWrappingBounds.Y > (double) rect.Y || (double) textWrappingBounds.Bottom < (double) rect.Y) && ((double) textWrappingBounds.Y > (double) bottom || (double) textWrappingBounds.Y < (double) rect.Y) && ((double) textWrappingBounds.Bottom < (double) rect.Y || (double) textWrappingBounds.Bottom > (double) bottom))
      return false;
    return textWrappingStyle == TextWrappingStyle.Square || textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through;
  }

  private bool IsNeedToWrapForSquareTightAndThrough(
    Layouter layouter,
    int wrapOwnerIndex,
    int wrapItemIndex,
    TextWrappingStyle textWrappingStyle,
    RectangleF textWrappingBounds,
    bool allowOverlap,
    int wrapCollectionIndex,
    Entity floatingEntity,
    bool isTextRangeInTextBox,
    RectangleF rect,
    SizeF size)
  {
    if (layouter.FloatingItems.Count <= 0 || wrapOwnerIndex == wrapCollectionIndex || wrapItemIndex == wrapCollectionIndex || textWrappingStyle == TextWrappingStyle.Inline || textWrappingStyle == TextWrappingStyle.TopAndBottom || textWrappingStyle == TextWrappingStyle.InFrontOfText || textWrappingStyle == TextWrappingStyle.Behind || (Math.Round((double) rect.Y + (double) size.Height, 2) <= Math.Round((double) textWrappingBounds.Y, 2) || !this.IsParaSpacingExceed(Math.Round((double) size.Height, 2), Math.Round((double) textWrappingBounds.Y, 2), rect)) && !this.IsTextFitBelow(textWrappingBounds, rect.Y + size.Height, floatingEntity) || Math.Round((double) rect.Y, 2) >= Math.Round((double) textWrappingBounds.Bottom, 2))
      return false;
    if (!allowOverlap)
      return true;
    if (isTextRangeInTextBox)
      return false;
    return !(this.LeafWidget is WPicture) && !(this.LeafWidget is Shape) && !(this.LeafWidget is WChart) && !(this.LeafWidget is WTextBox) && !(this.LeafWidget is GroupShape) || (this.LeafWidget as ParagraphItem).GetTextWrappingStyle() == TextWrappingStyle.Inline || !(this.LeafWidget as ParagraphItem).GetAllowOverlap();
  }

  private bool IsParaSpacingExceed(double textHeight, double floatingItemYpos, RectangleF rect)
  {
    WParagraphFormat paragraphFormat = this.GetOwnerParagraph()?.ParagraphFormat;
    return paragraphFormat == null || paragraphFormat.LineSpacingRule != LineSpacingRule.Exactly || Math.Round((double) paragraphFormat.LineSpacing, 2) <= 0.0 || Math.Round((double) paragraphFormat.LineSpacing, 2) >= textHeight || Math.Round((double) rect.Y + (double) paragraphFormat.LineSpacing, 2) > Math.Round(floatingItemYpos, 2);
  }

  private bool IsNeedToWrapForTopAndBottom(
    WParagraph currWidgetOwnerPara,
    Layouter layouter,
    int wrapOwnerIndex,
    int wrapItemIndex,
    TextWrappingStyle textWrappingStyle,
    RectangleF textWrappingBounds,
    bool allowOverlap,
    int wrapCollectionIndex,
    Entity floatingEntity,
    bool isTextRangeInTextBox,
    RectangleF rect,
    SizeF size)
  {
    if (currWidgetOwnerPara.IsInCell && textWrappingStyle == TextWrappingStyle.TopAndBottom && !(floatingEntity is WTable))
    {
      WParagraph wparagraph = floatingEntity == null || !(floatingEntity.Owner is WParagraph) ? floatingEntity as WParagraph : floatingEntity.Owner as WParagraph;
      if (wparagraph != null && wparagraph.IsInCell)
      {
        bool layOutInCell = (floatingEntity as ParagraphItem).GetLayOutInCell();
        WTableCell ownerEntity1 = wparagraph.GetOwnerEntity() as WTableCell;
        WTableCell ownerEntity2 = currWidgetOwnerPara.GetOwnerEntity() as WTableCell;
        if (ownerEntity1 != null && ownerEntity2 != null && ownerEntity1 != ownerEntity2 || !layOutInCell && currWidgetOwnerPara.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013)
          return false;
      }
    }
    if (layouter.FloatingItems.Count <= 0 || wrapOwnerIndex == wrapCollectionIndex || wrapItemIndex == wrapCollectionIndex || !this.IsFrameInClientArea(floatingEntity as WParagraph, textWrappingBounds) || textWrappingStyle != TextWrappingStyle.TopAndBottom || ((double) rect.Y < (double) textWrappingBounds.Y || (double) rect.Y >= (double) textWrappingBounds.Bottom) && (((double) rect.Y + (double) size.Height <= (double) textWrappingBounds.Y || !this.IsParaSpacingExceed(Math.Round((double) size.Height, 2), Math.Round((double) textWrappingBounds.Y, 2), rect)) && !this.IsTextFitBelow(textWrappingBounds, rect.Y + size.Height, floatingEntity) || (double) rect.Y + (double) size.Height >= (double) textWrappingBounds.Bottom))
      return false;
    if (!allowOverlap)
      return true;
    if (isTextRangeInTextBox)
      return false;
    return !(this.LeafWidget is WPicture) && !(this.LeafWidget is Shape) && !(this.LeafWidget is WChart) && !(this.LeafWidget is WTextBox) && !(this.LeafWidget is GroupShape) || (this.LeafWidget as ParagraphItem).GetTextWrappingStyle() == TextWrappingStyle.Inline || !(this.LeafWidget as ParagraphItem).GetAllowOverlap();
  }

  private bool IsNeedToForceDynamicRelayout(
    WParagraph currWidgetOwnerPara,
    ParagraphLayoutInfo paragraphLayoutInfo,
    TextWrappingStyle textWrappingStyle,
    Entity floatingEntity)
  {
    if (currWidgetOwnerPara.IsInCell && textWrappingStyle == TextWrappingStyle.TopAndBottom && !(floatingEntity is WTable))
    {
      WParagraph wparagraph = floatingEntity == null || !(floatingEntity.Owner is WParagraph) ? floatingEntity as WParagraph : floatingEntity.Owner as WParagraph;
      if (wparagraph != null && wparagraph.IsInCell && !(floatingEntity as ParagraphItem).GetLayOutInCell() && currWidgetOwnerPara.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && (double) paragraphLayoutInfo.YPosition + (double) paragraphLayoutInfo.Size.Height >= (double) this.m_ltWidget.Bounds.Y && (double) paragraphLayoutInfo.YPosition + (double) paragraphLayoutInfo.Size.Height < (double) this.m_ltWidget.Bounds.Bottom)
        return true;
    }
    return false;
  }

  private bool IsFloatingItemLayoutInCell(
    WParagraph currWidgetOwnerPara,
    TextWrappingStyle textWrappingStyle,
    Entity floatingEntity)
  {
    if (currWidgetOwnerPara.IsInCell && (textWrappingStyle != TextWrappingStyle.Behind || textWrappingStyle != TextWrappingStyle.InFrontOfText) && !(floatingEntity is WTable))
    {
      WParagraph wparagraph = floatingEntity == null || !(floatingEntity.Owner is WParagraph) ? floatingEntity as WParagraph : floatingEntity.Owner as WParagraph;
      if (wparagraph != null && wparagraph.IsInCell && ((floatingEntity as ParagraphItem).GetLayOutInCell() || currWidgetOwnerPara.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013))
        return true;
    }
    return false;
  }

  private bool IsOwnerParaNotFittedInSamePage(
    TextWrappingStyle textWrappingStyle,
    Entity floatingEntity,
    Layouter layouter,
    LayoutedWidget floatingWidget,
    ParagraphLayoutInfo paragraphLayoutInfo)
  {
    if (textWrappingStyle != TextWrappingStyle.Square && textWrappingStyle != TextWrappingStyle.TopAndBottom || floatingEntity is WTable)
      return false;
    bool flag1 = textWrappingStyle == TextWrappingStyle.Square && (double) floatingWidget.Bounds.X - 18.0 < (double) this.m_layoutArea.ClientActiveArea.X && (double) floatingWidget.Bounds.Right > (double) this.m_layoutArea.ClientActiveArea.Right;
    bool flag2 = (double) layouter.m_firstItemInPageYPosition > (double) floatingWidget.Bounds.Y && (double) paragraphLayoutInfo.YPosition + (double) floatingWidget.Bounds.Bottom > (double) this.m_layoutArea.ClientActiveArea.Bottom;
    return flag1 && flag2;
  }

  private bool IsNeedDoIntermediateWrapping(
    float remainingClientWidth,
    TextWrappingStyle textWrappingStyle,
    Layouter layouter,
    TextWrappingType textWrappingType,
    RectangleF rect,
    SizeF size,
    ParagraphLayoutInfo paragraphLayoutInfo,
    bool isDoesNotDenotesRectangle,
    RectangleF textWrappingBounds,
    ILeafWidget leafWidget,
    float minwidth,
    float minimumWidthRequired)
  {
    if (((double) remainingClientWidth > (double) minimumWidthRequired || (textWrappingStyle == TextWrappingStyle.Through || textWrappingStyle == TextWrappingStyle.Tight) && isDoesNotDenotesRectangle && (double) remainingClientWidth > (double) minwidth) && ((Math.Round((double) rect.Width, 2) <= Math.Round((double) minwidth, 2) || (double) rect.Width < (double) size.Width && leafWidget.LayoutInfo is TabsLayoutInfo) && textWrappingType != TextWrappingType.Left && textWrappingType != TextWrappingType.Largest || textWrappingType == TextWrappingType.Right || (double) rect.Width < (double) remainingClientWidth && textWrappingType == TextWrappingType.Largest))
      return true;
    if ((textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through) && isDoesNotDenotesRectangle && (double) textWrappingBounds.X < (double) paragraphLayoutInfo.XPosition || Math.Round((double) textWrappingBounds.X - (double) paragraphLayoutInfo.XPosition + (double) this.GetPararaphLeftIndent(), 2) >= (double) minimumWidthRequired && (!(leafWidget is WTextRange) || !this.IsFloatingItemOnLeft(rect, minwidth, textWrappingBounds)))
      return false;
    return textWrappingType != TextWrappingType.Left || (double) remainingClientWidth < (double) minimumWidthRequired;
  }

  internal bool IsFloatingItemOnLeft(
    RectangleF rect,
    float minwidth,
    RectangleF textWrappingBounds)
  {
    Layouter lcOperator = this.m_lcOperator as Layouter;
    List<FloatingItem> floatingItems = new List<FloatingItem>((IEnumerable<FloatingItem>) lcOperator.FloatingItems);
    FloatingItem.SortFloatingItems(floatingItems, SortPosition.Bottom);
    List<FloatingItem> floatingItemList = new List<FloatingItem>();
    for (int index = 0; index < floatingItems.Count; ++index)
    {
      RectangleF textWrappingBounds1 = floatingItems[index].TextWrappingBounds;
      textWrappingBounds1.Width += lcOperator.ClientLayoutArea.Width - textWrappingBounds1.Right;
      if (rect.IntersectsWith(textWrappingBounds1) && (double) rect.X >= (double) floatingItems[index].TextWrappingBounds.Right)
        floatingItemList.Add(floatingItems[index]);
    }
    return floatingItemList.Count > 0 && (double) rect.X + (double) minwidth > (double) textWrappingBounds.X;
  }

  private bool IsLineBreakIntersectOnFloatingItem(
    ILeafWidget leafWidget,
    TextWrappingStyle textWrappingStyle,
    RectangleF textWrappingBounds,
    RectangleF rect,
    SizeF size,
    WParagraph ownerPara)
  {
    float num1 = rect.Y + size.Height;
    float num2 = rect.X - ownerPara.ParagraphFormat.FirstLineIndent;
    return leafWidget is Break && (leafWidget as Break).BreakType == BreakType.LineBreak && (textWrappingStyle == TextWrappingStyle.Through || textWrappingStyle == TextWrappingStyle.Square || textWrappingStyle == TextWrappingStyle.Tight) && ownerPara.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && (this.m_lcOperator as Layouter).IsFirstItemInLine && (double) ownerPara.ParagraphFormat.FirstLineIndent > 0.0 && (double) num2 < (double) textWrappingBounds.X && (double) textWrappingBounds.X <= (double) rect.X && (double) textWrappingBounds.Y <= (double) num1 && (double) num1 <= (double) textWrappingBounds.Bottom;
  }

  internal void AdjustClientAreaBasedOnTextWrap(
    ILeafWidget leafWidget,
    ref SizeF size,
    ref RectangleF rect)
  {
    WParagraph ownerParagraph1 = this.GetOwnerParagraph();
    Layouter lcOperator = this.m_lcOperator as Layouter;
    float y1 = rect.Y;
    float bottom1 = 0.0f;
    RectangleF rectangleF1 = RectangleF.Empty;
    ParagraphLayoutInfo paragraphLayoutInfo = (ParagraphLayoutInfo) null;
    if (ownerParagraph1 != null)
      paragraphLayoutInfo = ((IWidget) ownerParagraph1).LayoutInfo as ParagraphLayoutInfo;
    if (this.IsNeedToWrapLeafWidget(ownerParagraph1, lcOperator) && !lcOperator.IsLayoutingFootnote && !this.SkipBookmark(size) && !this.SkipSectionBreak(ownerParagraph1, leafWidget))
    {
      RectangleF clientLayoutArea = lcOperator.ClientLayoutArea;
      int floattingItemIndex = this.GetFloattingItemIndex((Entity) ownerParagraph1);
      int wrapItemIndex = -1;
      switch (leafWidget)
      {
        case WPicture _:
        case WChart _:
        case WTextBox _:
        case Shape _:
        case GroupShape _:
          wrapItemIndex = (leafWidget as ParagraphItem).GetWrapCollectionIndex();
          break;
      }
      bool isTextRangeInTextBox = false;
      if (ownerParagraph1 != null)
      {
        switch (leafWidget)
        {
          case ParagraphItem _:
          case SplitStringWidget _:
            WTextBox wtextBox = this.IsEntityOwnerIsWTextbox((Entity) ownerParagraph1);
            if (wtextBox != null)
            {
              if (wtextBox.TextBoxFormat.TextWrappingStyle != TextWrappingStyle.Inline)
              {
                isTextRangeInTextBox = wtextBox.TextBoxFormat.AllowOverlap;
                break;
              }
              break;
            }
            if (ownerParagraph1.OwnerTextBody.Owner is Shape)
            {
              Shape owner = ownerParagraph1.OwnerTextBody.Owner as Shape;
              if (owner.WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline)
              {
                isTextRangeInTextBox = owner.WrapFormat.AllowOverlap;
                break;
              }
              break;
            }
            if (ownerParagraph1.OwnerTextBody.Owner is ChildShape)
            {
              GroupShape ownerGroupShape = (ownerParagraph1.OwnerTextBody.Owner as ChildShape).GetOwnerGroupShape();
              if (ownerGroupShape.WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline)
              {
                isTextRangeInTextBox = ownerGroupShape.WrapFormat.AllowOverlap;
                break;
              }
              break;
            }
            break;
        }
      }
      float x1 = rect.X;
      FloatingItem.SortFloatingItems(lcOperator.FloatingItems, SortPosition.Y);
      if (this.IsWord2013(ownerParagraph1.Document))
        FloatingItem.SortIntersectedYPostionFloatingItems(lcOperator.FloatingItems, SortPosition.X);
      else
        FloatingItem.SortSameYPostionFloatingItems(lcOperator.FloatingItems, SortPosition.X);
      for (int index1 = 0; index1 < lcOperator.FloatingItems.Count; ++index1)
      {
        bool allowOverlap = lcOperator.FloatingItems[index1].AllowOverlap;
        if (ownerParagraph1.IsInCell && allowOverlap && (ownerParagraph1.GetOwnerEntity() as WTableCell).OwnerRow.OwnerTable.TableFormat.Positioning.AllowOverlap && (!((this.m_lcOperator as Layouter).FloatingItems[index1].FloatingEntity is WTable) || !((this.m_lcOperator as Layouter).FloatingItems[index1].FloatingEntity as WTable).IsInCell))
        {
          WParagraph ownerParagraph2 = (this.m_lcOperator as Layouter).FloatingItems[index1].OwnerParagraph;
          if (ownerParagraph2 == null || !ownerParagraph2.IsInCell || ownerParagraph1.GetOwnerEntity() != ownerParagraph2.GetOwnerEntity())
            continue;
        }
        RectangleF rectangleF2 = lcOperator.FloatingItems[index1].TextWrappingBounds;
        float x2 = rectangleF2.X;
        RectangleF textWrappingBounds1 = lcOperator.FloatingItems[index1].TextWrappingBounds;
        TextWrappingStyle textWrappingStyle = lcOperator.FloatingItems[index1].TextWrappingStyle;
        TextWrappingType textWrappingType = lcOperator.FloatingItems[index1].TextWrappingType;
        if (!this.IsLineBreakIntersectOnFloatingItem(leafWidget, textWrappingStyle, textWrappingBounds1, rect, size, ownerParagraph1))
        {
          WTextBody ownerBody = (WTextBody) null;
          if ((this.IsInSameTextBody((TextBodyItem) ownerParagraph1, (this.m_lcOperator as Layouter).FloatingItems[index1], ref ownerBody) || !ownerParagraph1.IsInCell || !(ownerBody is WTableCell)) && (!this.IsInFrame((this.m_lcOperator as Layouter).FloatingItems[index1].FloatingEntity as WParagraph) || !this.IsOwnerCellInFrame(ownerParagraph1)))
          {
            if (ownerParagraph1.ParagraphFormat.Bidi && this.IsInSameTextBody((TextBodyItem) ownerParagraph1, (this.m_lcOperator as Layouter).FloatingItems[index1], ref ownerBody) && ownerParagraph1.IsInCell && ownerBody is WTableCell)
              this.ModifyXPositionForRTLLayouting(index1, ref textWrappingBounds1, this.m_layoutArea.ClientArea);
            else if (ownerParagraph1.ParagraphFormat.Bidi)
              this.ModifyXPositionForRTLLayouting(index1, ref textWrappingBounds1, (this.m_lcOperator as Layouter).ClientLayoutArea);
            float num1 = 0.0f;
            if (ownerParagraph1.IsInCell)
            {
              CellLayoutInfo layoutInfo = (ownerParagraph1.GetOwnerEntity() as IWidget).LayoutInfo as CellLayoutInfo;
              num1 = layoutInfo.Paddings.Left + layoutInfo.Paddings.Right;
            }
            float num2 = 18f;
            if (textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through)
              num2 = ownerParagraph1.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? 17.6f : 8f;
            float minimumWidthRequired = num2 - num1;
            if ((textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through) && lcOperator.FloatingItems[index1].IsDoesNotDenotesRectangle)
            {
              float x3 = rect.X;
              if (this.IsDoNotSuppressIndent(ownerParagraph1, rect.Y, textWrappingBounds1.Bottom, index1) && !(leafWidget.LayoutInfo is TabsLayoutInfo))
                rect.X -= paragraphLayoutInfo.Margins.Left;
              textWrappingBounds1 = this.AdjustTightAndThroughBounds(lcOperator.FloatingItems[index1], rect, size.Height);
              if (!(textWrappingBounds1 == lcOperator.FloatingItems[index1].TextWrappingBounds) || (double) rect.X < (double) textWrappingBounds1.X && (double) rect.Y + (double) size.Height > (double) textWrappingBounds1.Top && (double) textWrappingBounds1.Bottom > (double) rect.Y + (double) size.Height)
                rect.X = x3;
              else
                continue;
            }
            SizeF sizeF;
            int num3;
            if (textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through)
            {
              rectangleF2 = lcOperator.FloatingItems[index1].TextWrappingBounds;
              double width1 = (double) rectangleF2.Width;
              sizeF = (this.m_lcOperator as Layouter).CurrentSection.PageSetup.PageSize;
              double width2 = (double) sizeF.Width;
              if (width1 > width2)
              {
                num3 = lcOperator.FloatingItems[index1].IsDoesNotDenotesRectangle ? 1 : 0;
                goto label_41;
              }
            }
            num3 = 0;
label_41:
            bool flag1 = num3 != 0;
            double y2 = (double) rect.Y;
            sizeF = ((IWidget) ownerParagraph1).LayoutInfo.Size;
            double height1 = (double) sizeF.Height;
            float bottom2 = (float) (y2 + height1);
            if ((Math.Round((double) textWrappingBounds1.Y, 2) <= (double) rect.Y && (double) textWrappingBounds1.Bottom > (double) rect.Y || (double) rect.Y + (double) size.Height >= (double) textWrappingBounds1.Y && (double) rect.Y + (double) size.Height <= (double) textWrappingBounds1.Bottom) && (textWrappingStyle == TextWrappingStyle.Square || textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through) && leafWidget is Break && (leafWidget as Break).CharacterFormat.BreakClear == BreakClearType.All && !flag1 && (double) bottom1 < (double) textWrappingBounds1.Bottom)
              bottom1 = textWrappingBounds1.Bottom;
            if (this.CanWrapBreak(ownerParagraph1, (this.m_lcOperator as Layouter).FloatingItems[index1].OwnerParagraph) && this.IsNeedToWrapParaMarkToRightSide(ownerParagraph1, textWrappingBounds1, bottom2, lcOperator, rect, textWrappingType, textWrappingStyle, minimumWidthRequired) && !flag1)
            {
              if ((double) bottom1 != 0.0)
              {
                rect.Y = bottom1;
                this.m_layoutArea.UpdateBoundsBasedOnTextWrap(bottom1);
              }
              rect.X += textWrappingBounds1.Width;
              this.LeafWidget.LayoutInfo.IsLineBreak = false;
              size.Height = 0.0f;
              size.Width = textWrappingBounds1.Width;
              return;
            }
            WSection baseEntity = this.GetBaseEntity((Entity) ownerParagraph1) as WSection;
            if (ownerParagraph1.OwnerTextBody is HeaderFooter && textWrappingStyle == TextWrappingStyle.Square && lcOperator.FloatingItems[index1].FloatingEntity is WTable && (lcOperator.FloatingItems[index1].FloatingEntity as WTable).OwnerTextBody == ownerParagraph1.OwnerTextBody && (lcOperator.FloatingItems[index1].FloatingEntity as WTable).TableFormat.WrapTextAround && (lcOperator.FloatingItems[index1].FloatingEntity as WTable).TableFormat.Positioning.VertRelationTo == VerticalRelation.Page && (this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && !(this.m_lcOperator as Layouter).IsLayoutingHeader)
            {
              double num4 = (double) rect.Y + (double) baseEntity.PageSetup.Margins.Bottom + (double) size.Height;
              sizeF = baseEntity.PageSetup.PageSize;
              double height2 = (double) sizeF.Height;
              if (num4 > height2)
              {
                rect.Y = textWrappingBounds1.Y - size.Height;
                rect.Height -= size.Height;
                this.CreateLayoutArea(rect);
                (this.m_lcOperator as Layouter).IsNeedtoAdjustFooter = true;
              }
            }
            float adjustingValue = 0.0f;
            if ((this.IsNeedToConsiderAdjustValues(ref adjustingValue, ownerParagraph1, textWrappingStyle, index1) ? ((double) clientLayoutArea.X > (double) textWrappingBounds1.Right + (double) adjustingValue ? 1 : 0) : ((double) clientLayoutArea.X > (double) textWrappingBounds1.Right + (double) minimumWidthRequired ? 1 : ((double) clientLayoutArea.Right < (double) textWrappingBounds1.X - (double) minimumWidthRequired ? 1 : 0))) == 0 && !flag1)
            {
              if (this.IsNeedToWrapForSquareTightAndThrough(lcOperator, floattingItemIndex, wrapItemIndex, textWrappingStyle, textWrappingBounds1, allowOverlap, lcOperator.FloatingItems[index1].WrapCollectionIndex, lcOperator.FloatingItems[index1].FloatingEntity, isTextRangeInTextBox, rect, size))
              {
                float num5 = 0.0f;
                float num6 = 0.0f;
                float num7 = 0.0f;
                float num8 = paragraphLayoutInfo.IsFirstLine ? paragraphLayoutInfo.FirstLineIndent + paragraphLayoutInfo.ListTab : 0.0f;
                float num9 = (double) num8 > 0.0 ? num8 : 0.0f;
                bool flag2 = false;
                WTextRange currTextRange = this.GetCurrTextRange();
                if (ownerParagraph1 != null)
                {
                  if ((double) rect.X >= (double) textWrappingBounds1.X && textWrappingType != TextWrappingType.Left)
                    num5 = paragraphLayoutInfo.Margins.Right;
                  if ((double) rect.X < (double) textWrappingBounds1.X && textWrappingType != TextWrappingType.Right)
                    num6 = paragraphLayoutInfo.Margins.Left;
                  WListFormat listFormatValue;
                  WListLevel listLevel;
                  if (Math.Round((double) rect.X, 2) == Math.Round((double) clientLayoutArea.X + (double) paragraphLayoutInfo.Margins.Left, 2) && (listFormatValue = ownerParagraph1.GetListFormatValue()) != null && listFormatValue.CurrentListStyle != null && (listLevel = ownerParagraph1.GetListLevel(listFormatValue)) != null && (double) listLevel.ParagraphFormat.LeftIndent != 0.0)
                  {
                    num7 = paragraphLayoutInfo.Margins.Left;
                    flag2 = true;
                  }
                }
                if ((textWrappingStyle == TextWrappingStyle.Square || textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through) && leafWidget is Shape && (leafWidget as Shape).WrapFormat.TextWrappingStyle == TextWrappingStyle.Inline && (leafWidget as Shape).IsHorizontalRule)
                  size.Width = (double) textWrappingBounds1.X - (double) rect.X <= 18.0 ? size.Width - (textWrappingBounds1.Right - rect.X) - num5 : textWrappingBounds1.X - rect.X;
                float border = 0.0f;
                bool isBorderValueZero = false;
                WTable table = (WTable) null;
                bool isWord2013 = this.IsWord2013(ownerParagraph1.Document);
                float num10 = 0.0f;
                HorizontalPosition tableHorizontalPosition = HorizontalPosition.Left;
                IEntity floatingEntity1 = (IEntity) lcOperator.FloatingItems[index1].FloatingEntity;
                if (lcOperator.FloatingItems[index1].FloatingEntity is WTable)
                {
                  table = floatingEntity1 as WTable;
                  tableHorizontalPosition = table.TableFormat.Positioning.HorizPositionAbs;
                  border = this.GetMaximumRightCellBorderWidth(table);
                  if ((double) border == 0.0)
                  {
                    border = isWord2013 || tableHorizontalPosition != HorizontalPosition.Center ? 0.75f : 1.5f;
                    isBorderValueZero = true;
                  }
                  num10 = table.TableFormat.Borders.Left.LineWidth / 2f;
                }
                if ((textWrappingStyle != TextWrappingStyle.Tight && textWrappingStyle != TextWrappingStyle.Through || !lcOperator.FloatingItems[index1].IsDoesNotDenotesRectangle || !(textWrappingBounds1 == lcOperator.FloatingItems[index1].TextWrappingBounds)) && (double) rect.X + (double) num10 >= (double) textWrappingBounds1.X && (double) rect.X < (double) textWrappingBounds1.Right && (textWrappingType != TextWrappingType.Left || !isWord2013))
                {
                  rect.Width = rect.Width - (textWrappingBounds1.Right - rect.X) - num5 - border;
                  this.m_isWrapText = true;
                  bool flag3 = true;
                  if (floatingEntity1 is WParagraph && ownerParagraph1 != null)
                  {
                    if (isWord2013)
                    {
                      minimumWidthRequired = 22f;
                      if ((double) rect.Width < (double) size.Width)
                        flag3 = false;
                    }
                    else
                      minimumWidthRequired = 75f;
                    if ((double) minimumWidthRequired > (double) rect.Width)
                      flag3 = false;
                  }
                  if (table != null)
                    minimumWidthRequired = this.GetMinimumWidthRequiredForTable(currTextRange, table, border, isWord2013, tableHorizontalPosition, isBorderValueZero);
                  if (!flag3 || Math.Round((double) rect.Width) < (double) minimumWidthRequired || (double) rect.Width < (double) size.Width && leafWidget.LayoutInfo is TabsLayoutInfo || (double) textWrappingBounds1.X < (double) paragraphLayoutInfo.XPosition + (double) this.GetPararaphLeftIndent())
                  {
                    ref RectangleF local1 = ref rect;
                    rectangleF2 = this.m_layoutArea.ClientActiveArea;
                    double num11 = (double) rectangleF2.Right - (double) textWrappingBounds1.Right - (double) border - (flag2 ? (double) num7 : 0.0);
                    local1.Width = (float) num11;
                    float minwidth = currTextRange == null ? size.Width : this.GetMinWidth(currTextRange, size, rect);
                    if (Math.Round((double) rect.Width) < (double) minimumWidthRequired || (!(leafWidget is ISplitLeafWidget) ? ((double) rect.Width >= (double) size.Width ? 0 : (table == null || isWord2013 ? 1 : (textWrappingStyle != TextWrappingStyle.Square ? 1 : 0))) : ((double) rect.Width >= (double) minwidth || !(this.m_lcOperator as Layouter).m_canSplitbyCharacter || currTextRange == null ? 0 : (table != null || isWord2013 ? 1 : (textWrappingStyle != TextWrappingStyle.Square ? 1 : 0)))) != 0)
                    {
                      if (flag3 && (double) textWrappingBounds1.X - ((double) paragraphLayoutInfo.XPosition + (double) this.GetPararaphLeftIndent()) > (double) minimumWidthRequired)
                      {
                        if (baseEntity != null && baseEntity.Columns.Count != 1)
                        {
                          rectangleF2 = lcOperator.ClientLayoutArea;
                          if ((double) rectangleF2.Right - (double) textWrappingBounds1.Right <= (double) minimumWidthRequired)
                            goto label_83;
                        }
                        rect.Width = 0.0f;
                        if (textWrappingStyle == TextWrappingStyle.Square && (double) size.Width > 0.0 && (double) clientLayoutArea.X + (double) minimumWidthRequired < (double) textWrappingBounds1.X)
                        {
                          float forFloatingTable = this.GetTopMarginValueForFloatingTable(ownerParagraph1, lcOperator.FloatingItems[index1].FloatingEntity, rect.Y);
                          RectangleF intersectingItemBounds = FloatingItem.GetIntersectingItemBounds(this.m_lcOperator as Layouter, lcOperator.FloatingItems[index1], y1);
                          if (intersectingItemBounds != RectangleF.Empty && (double) intersectingItemBounds.Bottom <= (double) textWrappingBounds1.Bottom && (double) clientLayoutArea.X + (double) minimumWidthRequired > (double) intersectingItemBounds.X)
                          {
                            rect.X = x1;
                            rect.Width = textWrappingBounds1.X - rect.X - num5;
                            rect.Y = intersectingItemBounds.Bottom + forFloatingTable;
                            rect.Height = clientLayoutArea.Bottom - intersectingItemBounds.Bottom;
                            this.m_isYPositionUpdated = true;
                            goto label_91;
                          }
                          goto label_91;
                        }
                        goto label_91;
                      }
label_83:
                      if (textWrappingStyle != TextWrappingStyle.Tight && textWrappingStyle != TextWrappingStyle.Through || !lcOperator.FloatingItems[index1].IsDoesNotDenotesRectangle)
                      {
                        if ((textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through) && !(this.GetBaseEntity(lcOperator.FloatingItems[index1].FloatingEntity) is HeaderFooter) && !lcOperator.FloatingItems[index1].IsDoesNotDenotesRectangle)
                          textWrappingBounds1 = this.GetBottomPositionForTightAndThrough(this.GetFloattingItemBottom(lcOperator.FloatingItems[index1].FloatingEntity, textWrappingBounds1.Bottom), textWrappingBounds1, ownerParagraph1, rect.Y, size.Height);
                        float forFloatingTable = this.GetTopMarginValueForFloatingTable(ownerParagraph1, lcOperator.FloatingItems[index1].FloatingEntity, rect.Y);
                        bool flag4 = false;
                        rectangleF2 = lcOperator.ClientLayoutArea;
                        double num12 = (double) rectangleF2.X + (double) size.Width;
                        rectangleF2 = lcOperator.FloatingItems[index1].TextWrappingBounds;
                        double x4 = (double) rectangleF2.X;
                        if (num12 < x4)
                        {
                          RectangleF intersectingItemBounds = FloatingItem.GetIntersectingItemBounds(this.m_lcOperator as Layouter, lcOperator.FloatingItems[index1], y1);
                          if (intersectingItemBounds != RectangleF.Empty && (double) intersectingItemBounds.Bottom <= (double) textWrappingBounds1.Bottom)
                          {
                            rect.X = clientLayoutArea.X;
                            rect.Width = clientLayoutArea.Width;
                            rect.Y = intersectingItemBounds.Bottom + forFloatingTable;
                            rect.Height = clientLayoutArea.Bottom - intersectingItemBounds.Bottom;
                            this.m_isYPositionUpdated = true;
                            flag4 = true;
                          }
                        }
                        if (!flag4)
                        {
                          this.m_isYPositionUpdated = true;
                          ref RectangleF local2 = ref rect;
                          rectangleF2 = this.m_layoutArea.ClientArea;
                          double width = (double) rectangleF2.Width;
                          local2.Width = (float) width;
                          rect.Height -= textWrappingBounds1.Bottom + forFloatingTable - rect.Y;
                          rect.Y = textWrappingBounds1.Bottom + forFloatingTable;
                        }
                      }
label_91:
                      this.CreateLayoutArea(rect);
                      this.m_isWrapText = false;
                    }
                    else if ((double) lcOperator.RightPositionOfTabStopInterSectingFloattingItems == -3.4028234663852886E+38)
                    {
                      float x5 = rect.X;
                      TabsLayoutInfo tabsLayoutInfo = (TabsLayoutInfo) null;
                      rect.X = textWrappingBounds1.Right + (flag2 ? num7 : 0.0f) + num9;
                      rect.Width -= num9;
                      if (textWrappingStyle == TextWrappingStyle.Through && lcOperator.FloatingItems[index1].IsDoesNotDenotesRectangle)
                        this.UpdateXposition(textWrappingBounds1, index1, ref rect, size, minwidth);
                      if (textWrappingStyle == TextWrappingStyle.Square && (double) rect.Width < 0.0 && (double) size.Width > 0.0)
                      {
                        float forFloatingTable = this.GetTopMarginValueForFloatingTable(ownerParagraph1, lcOperator.FloatingItems[index1].FloatingEntity, rect.Y);
                        this.m_isYPositionUpdated = true;
                        ref RectangleF local3 = ref rect;
                        rectangleF2 = this.m_layoutArea.ClientArea;
                        double width = (double) rectangleF2.Width;
                        local3.Width = (float) width;
                        rect.Height -= textWrappingBounds1.Bottom + forFloatingTable - rect.Y;
                        rect.Y = textWrappingBounds1.Bottom + forFloatingTable;
                        rect.X = x5;
                      }
                      else
                        this.m_isXPositionUpdated = true;
                      this.CreateLayoutArea(rect);
                      if (!(leafWidget is Break))
                        this.AdjustClientAreaBasedOnExceededTab(leafWidget, size, ref rect, ownerParagraph1);
                      if (leafWidget != null)
                        tabsLayoutInfo = leafWidget.LayoutInfo as TabsLayoutInfo;
                      if (tabsLayoutInfo == null)
                      {
                        this.m_isWrapText = true;
                        this.CreateLayoutArea(rect);
                        if (floatingEntity1 is WTable)
                          this.m_isWrapText = false;
                      }
                    }
                  }
                  else if ((double) lcOperator.RightPositionOfTabStopInterSectingFloattingItems == -3.4028234663852886E+38)
                  {
                    rect.X = textWrappingBounds1.Right + (flag2 ? num7 : 0.0f) + num9;
                    ref RectangleF local4 = ref rect;
                    rectangleF2 = this.m_layoutArea.ClientActiveArea;
                    double num13 = (double) rectangleF2.Right - (double) textWrappingBounds1.Right - (flag2 ? (double) num7 : 0.0) - (double) num9;
                    local4.Width = (float) num13;
                    float minwidth = currTextRange == null ? size.Width : this.GetMinWidth(currTextRange, size, rect);
                    if (textWrappingStyle == TextWrappingStyle.Through && (this.m_lcOperator as Layouter).FloatingItems[index1].IsDoesNotDenotesRectangle)
                      this.UpdateXposition(textWrappingBounds1, index1, ref rect, size, minwidth);
                    if (this.IsFirstTextRangeInParagraph(leafWidget))
                      minwidth += paragraphLayoutInfo.ListTabWidth;
                    if (textWrappingStyle == TextWrappingStyle.Square && (double) rect.Width < 0.0 && (double) size.Width > 0.0 || (double) rect.Width < (double) minwidth)
                    {
                      float forFloatingTable = this.GetTopMarginValueForFloatingTable(ownerParagraph1, lcOperator.FloatingItems[index1].FloatingEntity, rect.Y);
                      this.m_isYPositionUpdated = true;
                      ref RectangleF local5 = ref rect;
                      rectangleF2 = this.m_layoutArea.ClientArea;
                      double width = (double) rectangleF2.Width;
                      local5.Width = (float) width;
                      rect.Height -= textWrappingBounds1.Bottom + forFloatingTable - rect.Y;
                      rect.Y = textWrappingBounds1.Bottom + forFloatingTable;
                      rect.X = x1;
                    }
                    else
                      this.m_isXPositionUpdated = true;
                    if (!(leafWidget is Break))
                      this.AdjustClientAreaBasedOnExceededTab(leafWidget, size, ref rect, ownerParagraph1);
                    if (ownerParagraph1 != null && ownerParagraph1.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
                      lcOperator.m_canSplitbyCharacter = false;
                    this.m_isWrapText = true;
                    this.CreateLayoutArea(rect);
                  }
                }
                else if ((double) textWrappingBounds1.X >= (double) rect.X && (double) rect.Right > (double) textWrappingBounds1.X)
                {
                  float num14 = 18f;
                  if (textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through)
                    num14 = ownerParagraph1.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? 18f : 9f;
                  rect.Width = textWrappingBounds1.X - rect.X - num5;
                  rectangleF2 = this.m_layoutArea.ClientActiveArea;
                  float num15 = rectangleF2.Right - textWrappingBounds1.Right;
                  float remainingClientWidth = (double) num15 > 0.0 ? num15 : 0.0f;
                  if ((double) remainingClientWidth == 0.0 && rectangleF1 != RectangleF.Empty && (double) rectangleF1.Bottom < (double) textWrappingBounds1.Y)
                    remainingClientWidth += rectangleF1.Width;
                  rectangleF1 = textWrappingBounds1;
                  this.m_isWrapText = true;
                  float minwidth = currTextRange == null ? size.Width : this.GetMinWidth(currTextRange, size, rect);
                  if (lcOperator.FloatingItems[index1].FloatingEntity is WParagraph && ownerParagraph1 != null)
                    minimumWidthRequired = !isWord2013 ? 75f : 22f;
                  if (table != null)
                    minimumWidthRequired = this.GetMinimumWidthRequiredForTable(currTextRange, table, border, isWord2013, tableHorizontalPosition, isBorderValueZero);
                  if ((double) remainingClientWidth < (double) minimumWidthRequired && textWrappingStyle != TextWrappingStyle.Tight && textWrappingStyle != TextWrappingStyle.Through || (double) remainingClientWidth < (double) minwidth)
                    this.m_isWrapText = false;
                  if (this.IsNeedDoIntermediateWrapping(remainingClientWidth, textWrappingStyle, lcOperator, textWrappingType, rect, size, paragraphLayoutInfo, lcOperator.FloatingItems[index1].IsDoesNotDenotesRectangle, textWrappingBounds1, leafWidget, minwidth, minimumWidthRequired))
                  {
                    rect.Width = remainingClientWidth;
                    this.m_isWrapText = true;
                    if ((double) rect.X + (double) minwidth > (double) textWrappingBounds1.X || textWrappingType == TextWrappingType.Right || textWrappingType == TextWrappingType.Largest || (double) clientLayoutArea.X > (double) textWrappingBounds1.X - (double) num14)
                    {
                      rect.X = textWrappingBounds1.Right;
                      WListFormat listFormatValue;
                      WListLevel listLevel;
                      if (paragraphLayoutInfo.IsFirstLine && (listFormatValue = ownerParagraph1.GetListFormatValue()) != null && listFormatValue.CurrentListStyle != null && (listLevel = ownerParagraph1.GetListLevel(listFormatValue)) != null && (double) listLevel.ParagraphFormat.LeftIndent != 0.0)
                      {
                        float x6 = 0.0f;
                        float width = rect.Width;
                        this.UpdateParaFirstLineHorizontalPositions(paragraphLayoutInfo, (IWidget) ownerParagraph1, ref x6, ref width);
                        rect.X += x6 + paragraphLayoutInfo.Margins.Left;
                        rect.Width -= x6 + paragraphLayoutInfo.Margins.Left;
                      }
                      this.m_isXPositionUpdated = true;
                      if (textWrappingStyle == TextWrappingStyle.Through && lcOperator.FloatingItems[index1].IsDoesNotDenotesRectangle)
                        this.UpdateXposition(textWrappingBounds1, index1, ref rect, size, minwidth);
                      if (textWrappingStyle == TextWrappingStyle.Through && lcOperator.FloatingItems[index1].IsDoesNotDenotesRectangle || (double) rect.Width > (double) minwidth || textWrappingType == TextWrappingType.Right || textWrappingType == TextWrappingType.Largest)
                        this.CreateLayoutArea(rect);
                    }
                    if ((double) rect.Width >= (double) minimumWidthRequired || (double) minwidth < (double) remainingClientWidth && (TextWrappingStyle.Tight == textWrappingStyle || textWrappingStyle == TextWrappingStyle.Through))
                    {
                      if ((double) rect.Width < (double) minwidth)
                      {
                        double num16 = Math.Round((double) rect.Right, 2);
                        rectangleF2 = lcOperator.ClientLayoutArea;
                        double num17 = Math.Round((double) rectangleF2.Right, 2);
                        if (num16 != num17 || textWrappingType != TextWrappingType.Both)
                          goto label_173;
                      }
                      else
                        goto label_173;
                    }
                    List<FloatingItem> floatingItems1 = new List<FloatingItem>((IEnumerable<FloatingItem>) lcOperator.FloatingItems);
                    List<FloatingItem> floatingItems2 = new List<FloatingItem>((IEnumerable<FloatingItem>) lcOperator.FloatingItems);
                    FloatingItem.SortFloatingItems(floatingItems1, SortPosition.X);
                    FloatingItem.SortFloatingItems(floatingItems2, SortPosition.Bottom);
                    RectangleF textWrappingBounds2 = textWrappingBounds1;
                    float floattingItemBottomPosition = float.MinValue;
                    if (floatingItems2.Count > 1)
                    {
                      for (int index2 = 0; index2 < floatingItems2.Count; ++index2)
                      {
                        if (ownerParagraph1.IsInCell || !this.IsInTable(floatingItems2[index2].FloatingEntity))
                        {
                          double y3 = (double) rect.Y;
                          rectangleF2 = floatingItems2[index2].TextWrappingBounds;
                          double bottom3 = (double) rectangleF2.Bottom;
                          if (y3 <= bottom3)
                          {
                            double bottom4 = (double) rect.Bottom;
                            rectangleF2 = floatingItems2[index2].TextWrappingBounds;
                            double y4 = (double) rectangleF2.Y;
                            if (bottom4 >= y4)
                            {
                              int index3 = floatingItems1.IndexOf(floatingItems2[index2]);
                              if (index3 == 0)
                              {
                                if (!(floatingItems1[1].FloatingEntity.Owner is HeaderFooter))
                                {
                                  rectangleF2 = floatingItems1[1].TextWrappingBounds;
                                  double bottom5 = (double) rectangleF2.Bottom;
                                  rectangleF2 = floatingItems1[index3].TextWrappingBounds;
                                  double bottom6 = (double) rectangleF2.Bottom;
                                  if (bottom5 - bottom6 > (double) minimumWidthRequired)
                                  {
                                    rectangleF2 = floatingItems1[index3].TextWrappingBounds;
                                    double num18 = (double) rectangleF2.Right + (double) minwidth;
                                    rectangleF2 = floatingItems1[index3 + 1].TextWrappingBounds;
                                    double x7 = (double) rectangleF2.X;
                                    if (num18 <= x7)
                                    {
                                      rectangleF2 = floatingItems1[index3 + 1].TextWrappingBounds;
                                      double x8 = (double) rectangleF2.X;
                                      rectangleF2 = floatingItems1[index3].TextWrappingBounds;
                                      double x9 = (double) rectangleF2.X;
                                      if (x8 - x9 <= (double) minimumWidthRequired)
                                        continue;
                                    }
                                    textWrappingBounds2 = floatingItems1[index3].TextWrappingBounds;
                                    if ((floatingItems1[index3].TextWrappingStyle == TextWrappingStyle.Tight || floatingItems1[index3].TextWrappingStyle == TextWrappingStyle.Through) && !floatingItems1[index3].IsDoesNotDenotesRectangle)
                                    {
                                      Entity floatingEntity2 = floatingItems1[index3].FloatingEntity;
                                      rectangleF2 = floatingItems1[index3].TextWrappingBounds;
                                      double bottom7 = (double) rectangleF2.Bottom;
                                      floattingItemBottomPosition = this.GetFloattingItemBottom(floatingEntity2, (float) bottom7);
                                      break;
                                    }
                                    break;
                                  }
                                }
                              }
                              else if (index3 == floatingItems2.Count - 1)
                              {
                                rectangleF2 = floatingItems1[index3 - 1].TextWrappingBounds;
                                double num19 = (double) rectangleF2.Right + (double) minwidth;
                                rectangleF2 = floatingItems1[index3].TextWrappingBounds;
                                double x10 = (double) rectangleF2.X;
                                if (num19 < x10)
                                {
                                  rectangleF2 = floatingItems1[index3 - 1].TextWrappingBounds;
                                  double bottom8 = (double) rectangleF2.Bottom;
                                  rectangleF2 = floatingItems1[index3].TextWrappingBounds;
                                  double bottom9 = (double) rectangleF2.Bottom;
                                  if (bottom8 - bottom9 > (double) minimumWidthRequired)
                                  {
                                    rectangleF2 = floatingItems1[index3 - 1].TextWrappingBounds;
                                    double right1 = (double) rectangleF2.Right;
                                    rectangleF2 = floatingItems1[index3].TextWrappingBounds;
                                    double right2 = (double) rectangleF2.Right;
                                    if (right1 - right2 > (double) minimumWidthRequired)
                                    {
                                      textWrappingBounds2 = floatingItems1[index3].TextWrappingBounds;
                                      if ((floatingItems1[index3].TextWrappingStyle == TextWrappingStyle.Tight || floatingItems1[index3].TextWrappingStyle == TextWrappingStyle.Through) && !floatingItems1[index3].IsDoesNotDenotesRectangle)
                                      {
                                        Entity floatingEntity3 = floatingItems1[index3].FloatingEntity;
                                        rectangleF2 = floatingItems1[index3].TextWrappingBounds;
                                        double bottom10 = (double) rectangleF2.Bottom;
                                        floattingItemBottomPosition = this.GetFloattingItemBottom(floatingEntity3, (float) bottom10);
                                        break;
                                      }
                                      break;
                                    }
                                  }
                                }
                              }
                              else
                              {
                                rectangleF2 = floatingItems1[index3 + 1].TextWrappingBounds;
                                double x11 = (double) rectangleF2.X;
                                rectangleF2 = floatingItems1[index3].TextWrappingBounds;
                                double x12 = (double) rectangleF2.X;
                                if (x11 - x12 > (double) minimumWidthRequired)
                                {
                                  rectangleF2 = floatingItems1[index3 + 1].TextWrappingBounds;
                                  double bottom11 = (double) rectangleF2.Bottom;
                                  rectangleF2 = floatingItems1[index3].TextWrappingBounds;
                                  double bottom12 = (double) rectangleF2.Bottom;
                                  if (bottom11 - bottom12 > (double) minimumWidthRequired)
                                  {
                                    rectangleF2 = floatingItems1[index3].TextWrappingBounds;
                                    double num20 = (double) rectangleF2.Right + (double) minwidth;
                                    rectangleF2 = floatingItems1[index3 + 1].TextWrappingBounds;
                                    double x13 = (double) rectangleF2.X;
                                    if (num20 > x13)
                                    {
                                      rectangleF2 = floatingItems1[index3 - 1].TextWrappingBounds;
                                      double num21 = (double) rectangleF2.Right + (double) minwidth;
                                      rectangleF2 = floatingItems1[index3 + 1].TextWrappingBounds;
                                      double x14 = (double) rectangleF2.X;
                                      if (num21 < x14)
                                      {
                                        textWrappingBounds2 = floatingItems1[index3].TextWrappingBounds;
                                        if ((floatingItems1[index3].TextWrappingStyle == TextWrappingStyle.Tight || floatingItems1[index3].TextWrappingStyle == TextWrappingStyle.Through) && !floatingItems1[index3].IsDoesNotDenotesRectangle)
                                        {
                                          Entity floatingEntity4 = floatingItems1[index3].FloatingEntity;
                                          rectangleF2 = floatingItems1[index3].TextWrappingBounds;
                                          double bottom13 = (double) rectangleF2.Bottom;
                                          floattingItemBottomPosition = this.GetFloattingItemBottom(floatingEntity4, (float) bottom13);
                                          break;
                                        }
                                        break;
                                      }
                                    }
                                  }
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                    if ((double) floattingItemBottomPosition != -3.4028234663852886E+38 && (textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through) && !(this.GetBaseEntity(lcOperator.FloatingItems[index1].FloatingEntity) is HeaderFooter) && !lcOperator.FloatingItems[index1].IsDoesNotDenotesRectangle)
                      textWrappingBounds2 = this.GetBottomPositionForTightAndThrough(floattingItemBottomPosition, textWrappingBounds2, ownerParagraph1, rect.Y, size.Height);
                    if (Math.Round((double) rect.X, 2) == Math.Round((double) this.GetPageMarginLeft() + (double) this.GetPararaphLeftIndent(), 2))
                    {
                      float forFloatingTable = this.GetTopMarginValueForFloatingTable(ownerParagraph1, lcOperator.FloatingItems[index1].FloatingEntity, rect.Y);
                      rect.Y = textWrappingBounds2.Bottom + forFloatingTable;
                      this.m_isYPositionUpdated = true;
                      ref RectangleF local = ref rect;
                      rectangleF2 = this.m_layoutArea.ClientArea;
                      double width = (double) rectangleF2.Width;
                      local.Width = (float) width;
                      rect.Height -= textWrappingBounds2.Height + forFloatingTable;
                      this.CreateLayoutArea(rect);
                      this.m_isWrapText = false;
                    }
                    else
                    {
                      double num22 = Math.Round((double) rect.Right, 2);
                      rectangleF2 = lcOperator.ClientLayoutArea;
                      double num23 = Math.Round((double) rectangleF2.Right, 2);
                      if (num22 >= num23 && textWrappingType == TextWrappingType.Both)
                      {
                        float forFloatingTable = this.GetTopMarginValueForFloatingTable(ownerParagraph1, lcOperator.FloatingItems[index1].FloatingEntity, rect.Y);
                        rect.Y = textWrappingBounds2.Bottom + forFloatingTable;
                        ref RectangleF local6 = ref rect;
                        rectangleF2 = lcOperator.ClientLayoutArea;
                        double width = (double) rectangleF2.Width;
                        local6.Width = (float) width;
                        rect.Height -= textWrappingBounds2.Height + forFloatingTable;
                        ref RectangleF local7 = ref rect;
                        rectangleF2 = lcOperator.ClientLayoutArea;
                        double num24 = (double) rectangleF2.X + (double) num6;
                        local7.X = (float) num24;
                        this.CreateLayoutArea(rect);
                        this.m_isXPositionUpdated = true;
                        this.m_isYPositionUpdated = true;
                        this.m_isWrapText = false;
                      }
                      else
                      {
                        rect.Width = 0.0f;
                        this.CreateLayoutArea(rect);
                      }
                    }
label_173:
                    this.CreateLayoutArea(rect);
                  }
                  else
                  {
                    if (lcOperator.IsFirstItemInLine && ownerParagraph1 != null && textWrappingStyle == TextWrappingStyle.Square && this.IsWord2013(ownerParagraph1.Document) && Math.Round((double) rect.Width, 2) <= Math.Round((double) minwidth, 2) && this.IsInSameTextBody((TextBodyItem) ownerParagraph1, lcOperator.FloatingItems[index1], ref ownerBody))
                    {
                      rect.X = clientLayoutArea.X + num6;
                      rect.Y = textWrappingBounds1.Bottom;
                      rect.Width = clientLayoutArea.Width;
                      rect.Height -= textWrappingBounds1.Bottom - rect.Y;
                    }
                    else if (Math.Round((double) rect.Width, 2) <= Math.Round((double) minwidth, 2))
                    {
                      double num25 = Math.Round((double) rect.X - (double) num6, 2);
                      rectangleF2 = (this.m_lcOperator as Layouter).ClientLayoutArea;
                      double num26 = Math.Round((double) rectangleF2.X, 2);
                      if (num25 != num26)
                        rect.Width = 0.0f;
                    }
                    this.CreateLayoutArea(rect);
                  }
                }
                else if ((double) rect.X > (double) textWrappingBounds1.X && Math.Round((double) rect.X, 2) > Math.Round((double) textWrappingBounds1.Right, 2) && textWrappingType != TextWrappingType.Left)
                {
                  TabsLayoutInfo tabsLayoutInfo = (TabsLayoutInfo) null;
                  if (leafWidget != null)
                    tabsLayoutInfo = leafWidget.LayoutInfo as TabsLayoutInfo;
                  rect.X = !this.IsDoNotSuppressIndent(ownerParagraph1, rect.Y, textWrappingBounds1.Bottom, index1) || !lcOperator.IsFirstItemInLine || leafWidget is Entity && (leafWidget as Entity).IsFloatingItem(false) ? rect.X : textWrappingBounds1.Right + (paragraphLayoutInfo.IsFirstLine ? paragraphLayoutInfo.FirstLineIndent : 0.0f);
                  float num27 = currTextRange == null ? size.Width : this.GetMinWidth(currTextRange, size, rect);
                  if (tabsLayoutInfo != null && (double) num27 + (double) size.Width > (double) rect.Width)
                  {
                    ref RectangleF local = ref rect;
                    rectangleF2 = this.m_layoutArea.ClientArea;
                    double x15 = (double) rectangleF2.X;
                    local.X = (float) x15;
                    this.m_isYPositionUpdated = true;
                    this.CreateLayoutArea(rect);
                  }
                  else
                  {
                    ref RectangleF local = ref rect;
                    rectangleF2 = this.m_layoutArea.ClientArea;
                    double width = (double) rectangleF2.Width;
                    local.Width = (float) width;
                    this.CreateLayoutArea(rect);
                  }
                }
                else if ((double) rect.X > (double) textWrappingBounds1.X && (double) rect.X < (double) textWrappingBounds1.Right && textWrappingType != TextWrappingType.Left)
                {
                  rect.Width -= textWrappingBounds1.Right - rect.X;
                  rect.X = textWrappingBounds1.Right;
                  if (textWrappingStyle == TextWrappingStyle.Through && lcOperator.FloatingItems[index1].IsDoesNotDenotesRectangle)
                  {
                    float minwidth = currTextRange == null ? size.Width : this.GetMinWidth(currTextRange, size, rect);
                    this.UpdateXposition(textWrappingBounds1, index1, ref rect, size, minwidth);
                  }
                  this.CreateLayoutArea(rect);
                  this.m_isXPositionUpdated = true;
                  this.m_isWrapText = true;
                }
                if (textWrappingType != TextWrappingType.Both)
                  this.m_isWrapText = false;
              }
              else if (this.IsNeedToWrapForTopAndBottom(ownerParagraph1, lcOperator, floattingItemIndex, wrapItemIndex, textWrappingStyle, textWrappingBounds1, allowOverlap, lcOperator.FloatingItems[index1].WrapCollectionIndex, lcOperator.FloatingItems[index1].FloatingEntity, isTextRangeInTextBox, rect, size))
              {
                if ((textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through) && !(this.GetBaseEntity(lcOperator.FloatingItems[index1].FloatingEntity) is HeaderFooter) && !lcOperator.FloatingItems[index1].IsDoesNotDenotesRectangle)
                  textWrappingBounds1 = this.GetBottomPositionForTightAndThrough(this.GetFloattingItemBottom(lcOperator.FloatingItems[index1].FloatingEntity, textWrappingBounds1.Bottom), textWrappingBounds1, ownerParagraph1, rect.Y, size.Height);
                float forFloatingTable = this.GetTopMarginValueForFloatingTable(ownerParagraph1, lcOperator.FloatingItems[index1].FloatingEntity, rect.Y);
                float y5 = rect.Y;
                rect.Y = textWrappingBounds1.Bottom + forFloatingTable;
                this.m_isYPositionUpdated = true;
                rect.Height -= textWrappingBounds1.Bottom - y5 + forFloatingTable;
                if ((double) rect.Y != (double) y1 && leafWidget is WTextRange && !(lcOperator.FloatingItems[index1].FloatingEntity is WTable) && paragraphLayoutInfo.IsFirstLine)
                {
                  rect.Y += paragraphLayoutInfo.Margins.Top;
                  y1 = rect.Y;
                }
                this.CreateLayoutArea(rect);
              }
            }
            this.ResetXPositionForRTLLayouting(index1, ref textWrappingBounds1, x2);
          }
        }
      }
      if (ownerParagraph1 != null)
        this.UpdateXYPositionBasedOnAdjacentFloatingItems(lcOperator.FloatingItems, rect, size, ownerParagraph1, true);
      if (this.IsWord2013(ownerParagraph1.Document))
      {
        FloatingItem.SortFloatingItems(lcOperator.FloatingItems, SortPosition.Y);
        FloatingItem.SortSameYPostionFloatingItems(lcOperator.FloatingItems, SortPosition.X);
      }
    }
    else if (this.IsNeedToWrapFloatingItem(this.LeafWidget) && !(this.LeafWidget as ParagraphItem).IsWrappingBoundsAdded())
    {
      RectangleF clientLayoutArea = lcOperator.ClientLayoutArea;
      FloatingItem.SortFloatingItems(lcOperator.WrapFloatingItems, SortPosition.Y);
      FloatingItem.SortSameYPostionFloatingItems(lcOperator.WrapFloatingItems, SortPosition.X);
      FloatingItem floatingItem = new FloatingItem();
      floatingItem.FloatingEntity = this.LeafWidget as Entity;
      for (int index = 0; index < lcOperator.WrapFloatingItems.Count; ++index)
      {
        RectangleF textWrappingBounds = lcOperator.WrapFloatingItems[index].TextWrappingBounds;
        if ((double) clientLayoutArea.X <= (double) textWrappingBounds.Right && (double) clientLayoutArea.Right >= (double) textWrappingBounds.X && (double) rect.X >= (double) textWrappingBounds.X && (double) rect.X < (double) textWrappingBounds.Right && (double) rect.Y >= (double) textWrappingBounds.Y && (double) rect.Y < (double) textWrappingBounds.Bottom)
        {
          if ((double) rect.Width < (double) size.Width)
            rect.Y = textWrappingBounds.Bottom;
          else
            rect.X = textWrappingBounds.Right;
          this.CreateLayoutArea(rect);
        }
      }
      floatingItem.TextWrappingBounds = new RectangleF(rect.X, rect.Y, size.Width, size.Height);
      if (this.LeafWidget is Shape)
        (this.LeafWidget as Shape).WrapFormat.IsWrappingBoundsAdded = true;
      else if (this.LeafWidget is WPicture)
        (this.LeafWidget as WPicture).IsWrappingBoundsAdded = true;
      else if (this.LeafWidget is WChart)
        (this.LeafWidget as WChart).WrapFormat.IsWrappingBoundsAdded = true;
      else if (this.LeafWidget is WTextBox)
        (this.LeafWidget as WTextBox).TextBoxFormat.IsWrappingBoundsAdded = true;
      else if (this.LeafWidget is GroupShape)
        (this.LeafWidget as GroupShape).WrapFormat.IsWrappingBoundsAdded = true;
      (this.m_lcOperator as Layouter).WrapFloatingItems.Add(floatingItem);
    }
    if ((double) bottom1 == 0.0)
      return;
    if ((double) bottom1 > (double) rect.Y)
      size.Height = 0.0f;
    rect.Y = bottom1;
    this.m_layoutArea.UpdateBoundsBasedOnTextWrap(bottom1);
  }

  private bool CanWrapBreak(WParagraph owner, WParagraph fItemOwner)
  {
    Entity ownerEntity = owner.GetOwnerEntity();
    if (fItemOwner != null)
    {
      switch (ownerEntity)
      {
        case WTextBox _:
        case Shape _:
          return fItemOwner.GetOwnerEntity() == ownerEntity;
      }
    }
    return true;
  }

  private float GetMinimumWidthRequiredForTable(
    WTextRange currTextRange,
    WTable table,
    float border,
    bool isWord2013,
    HorizontalPosition tableHorizontalPosition,
    bool isBorderValueZero)
  {
    return !isWord2013 ? (tableHorizontalPosition != HorizontalPosition.Center ? ((double) border != 0.25 ? 19.3f : 18.5f) : (!isBorderValueZero ? 18.5f + (float) Math.Round((double) border / 2.0, 1) : 19.25f)) : (tableHorizontalPosition != HorizontalPosition.Center ? (!isBorderValueZero ? 18.5f + border : 19.25f) : (!isBorderValueZero ? 18.5f + (float) Math.Round((double) border / 2.0, 1) : 18.5f + (float) Math.Round(0.375, 1)));
  }

  private float GetMaximumRightCellBorderWidth(WTable table)
  {
    float rightCellBorderWidth = 0.0f;
    foreach (WTableRow row in (Syncfusion.DocIO.DLS.CollectionImpl) table.Rows)
    {
      float lineWidth = row.Cells[row.Cells.Count - 1].CellFormat.Borders.Right.LineWidth;
      if ((double) rightCellBorderWidth < (double) lineWidth)
        rightCellBorderWidth = lineWidth;
    }
    return rightCellBorderWidth;
  }

  private bool IsNeedToWrapFloatingItem(ILeafWidget leafWidget)
  {
    return (this.LeafWidget is WPicture || this.LeafWidget is WChart || this.LeafWidget is Shape || this.LeafWidget is WTextBox || this.LeafWidget is GroupShape) && this.CheckWrappingStyleForWrapping((this.LeafWidget as ParagraphItem).GetTextWrappingStyle()) && !(this.LeafWidget as ParagraphItem).GetAllowOverlap();
  }

  private bool IsFirstTextRangeInParagraph(ILeafWidget leafWidget)
  {
    if (leafWidget is WTextRange && (leafWidget as WTextRange).Owner is WParagraph owner)
    {
      foreach (Entity childEntity in (Syncfusion.DocIO.DLS.CollectionImpl) owner.ChildEntities)
      {
        Entity entity;
        switch (childEntity)
        {
          case WTextRange _ when entity as WTextRange == leafWidget:
            return true;
          case BookmarkStart _:
          case BookmarkEnd _:
            continue;
          default:
            return false;
        }
      }
    }
    return false;
  }

  private bool SkipSectionBreak(WParagraph ownerPara, ILeafWidget leafWidget)
  {
    WParagraph lastParagraph = ownerPara.Document.LastParagraph;
    return leafWidget is WTextRange && (leafWidget as WTextRange).OwnerParagraph == null && ownerPara.ChildEntities.Count > 0 && ownerPara.IsContainFloatingItems() && ownerPara.IsParagraphHasSectionBreak() && lastParagraph != ownerPara;
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

  private bool SkipBookmark(SizeF size)
  {
    return (this.LeafWidget is BookmarkStart || this.LeafWidget is BookmarkEnd) && (double) size.Width <= 0.0 && (double) size.Height <= 0.0;
  }

  private bool IsTextFitBelow(RectangleF wrappingBounds, float textHeight, Entity entity)
  {
    switch (entity)
    {
      case WTextBox _:
      case Shape _:
      case GroupShape _:
        double num1;
        switch (entity)
        {
          case WTextBox _:
            num1 = (double) (entity as WTextBox).TextBoxFormat.LineWidth;
            break;
          case Shape _:
            num1 = (double) (entity as Shape).LineFormat.Weight;
            break;
          default:
            num1 = (double) (entity as GroupShape).LineFormat.Weight;
            break;
        }
        float d = (float) num1;
        int num2 = 2;
        double num3 = Math.Floor((double) d);
        while ((num3 - (double) num2) % 3.0 != 0.0 && num3 > 0.0)
        {
          --num3;
          if (num3 <= 0.0)
            goto label_11;
        }
        double num4 = num3 > 2.0 ? (double) textHeight + (num3 - (double) num2) / 3.0 * 1.5 : (double) textHeight;
        double num5 = num3 > (double) num2 ? (double) textHeight + (num3 - (double) num2) / 3.0 * 1.5 + 1.4700000286102295 : (double) textHeight + 1.4700000286102295;
        return num3 <= (double) num2 && (double) wrappingBounds.Y > Math.Round(num4, 2) && (double) wrappingBounds.Y < Math.Round(num5, 2) && (double) d > (double) num2 || num3 != 0.0 && (double) wrappingBounds.Y > Math.Round(num4, 2) && (double) wrappingBounds.Y < Math.Round(num5, 2) && (double) d > num3;
    }
label_11:
    return false;
  }

  private float GetTopMarginValueForFloatingTable(
    WParagraph paragraph,
    Entity floatingEntity,
    float yPosition)
  {
    ParagraphLayoutInfo paragraphLayoutInfo = (ParagraphLayoutInfo) null;
    if (paragraph != null)
      paragraphLayoutInfo = ((IWidget) paragraph).LayoutInfo as ParagraphLayoutInfo;
    return paragraphLayoutInfo != null && paragraphLayoutInfo.IsFirstLine && (double) yPosition > (double) paragraphLayoutInfo.YPosition && floatingEntity is WTable && ((floatingEntity as WTable).IsFrame ? (paragraph.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 ? 1 : 0) : 1) != 0 ? yPosition - paragraphLayoutInfo.YPosition : 0.0f;
  }

  private void UpdateXposition(
    RectangleF textWrappingBounds,
    int i,
    ref RectangleF rect,
    SizeF size,
    float minwidth)
  {
    bool flag = true;
    Layouter lcOperator = this.m_lcOperator as Layouter;
    while (flag)
    {
      textWrappingBounds = this.AdjustTightAndThroughBounds(lcOperator.FloatingItems[i], rect, size.Height);
      if ((double) textWrappingBounds.X != 0.0 && (double) textWrappingBounds.X != (double) lcOperator.FloatingItems[i].TextWrappingBounds.X)
      {
        rect.Width = textWrappingBounds.X - rect.X;
        if ((double) rect.Width > (double) minwidth)
        {
          flag = false;
        }
        else
        {
          rect.X = textWrappingBounds.Right;
          rect.Width = this.m_layoutArea.ClientActiveArea.Right - textWrappingBounds.Right;
        }
      }
      else
        flag = false;
    }
  }

  private void AdjustClientAreaBasedOnExceededTab(
    ILeafWidget leafWidget,
    SizeF size,
    ref RectangleF rect,
    WParagraph paragraph)
  {
    Layouter lcOperator = this.m_lcOperator as Layouter;
    float left = (((IWidget) paragraph).LayoutInfo as ParagraphLayoutInfo).Margins.Left;
    if (leafWidget.LayoutInfo is TabsLayoutInfo)
      return;
    Entity entity = !(leafWidget is SplitStringWidget) ? (leafWidget as Entity).PreviousSibling as Entity : ((leafWidget as SplitStringWidget).RealStringWidget as Entity).PreviousSibling as Entity;
    if ((double) lcOperator.RightPositionOfTabStopInterSectingFloattingItems != -3.4028234663852886E+38 || paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
      return;
    for (; entity != null; entity = entity.PreviousSibling as Entity)
    {
      if ((entity as IWidget).LayoutInfo.IsSkip)
        continue;
      switch (entity)
      {
        case BookmarkStart _:
        case BookmarkEnd _:
          continue;
        default:
          goto label_8;
      }
    }
label_8:
    TabsLayoutInfo tabsLayoutInfo = (TabsLayoutInfo) null;
    if (entity != null)
      tabsLayoutInfo = (entity as ILeafWidget).LayoutInfo as TabsLayoutInfo;
    if (tabsLayoutInfo == null)
      return;
    lcOperator.RightPositionOfTabStopInterSectingFloattingItems = rect.X - left;
    rect.X = lcOperator.PreviousTabWidth + this.GetPararaphLeftIndent() + Layouter.GetLeftMargin(lcOperator.CurrentSection as WSection);
    rect.Width = (float) Math.Round(1584.0 - (double) lcOperator.PreviousTabWidth - (double) this.GetPararaphLeftIndent());
    lcOperator.MaxRightPositionOfTabStopInterSectingFloattingItems = rect.Right;
    this.IsTabStopBeyondRightMarginExists = true;
  }

  private float GetMinWidth(WTextRange currTextRange, SizeF size, RectangleF rect)
  {
    string text1 = this.GetText();
    string[] strArray = text1.Split(' ');
    if (text1 != string.Empty && text1.Trim() == string.Empty && currTextRange != null && currTextRange.OwnerParagraph != null && currTextRange.PreviousSibling == null && currTextRange.NextSibling != null && currTextRange.OwnerParagraph.ChildEntities.IndexOf((IEntity) currTextRange) != -1 && currTextRange.OwnerParagraph.Text.Trim() != string.Empty)
      strArray = new string[2]{ text1, "" };
    string text2 = strArray[0];
    if (text2 == string.Empty)
      text2 = " ";
    float width = this.DrawingContext.MeasureTextRange(currTextRange, text2).Width;
    if (this.DrawingContext.IsUnicodeText(text1))
      width = this.DrawingContext.MeasureTextRange(currTextRange, text1[0].ToString()).Width;
    WTextRange nextSibling = this.GetNextSibling(currTextRange) as WTextRange;
    if (strArray.Length == 1 && nextSibling != null)
    {
      string text3 = nextSibling.Text;
      width += this.GetNextTextRangeWidth((IWidget) nextSibling, ref text3, size, rect);
    }
    else if (currTextRange != null && currTextRange.OwnerParagraph == null && nextSibling == null)
    {
      WParagraph ownerBase = currTextRange.CharacterFormat.BaseFormat.OwnerBase as WParagraph;
      width += ownerBase == null || !(((IWidget) ownerBase).LayoutInfo is ParagraphLayoutInfo layoutInfo) ? 0.0f : layoutInfo.Size.Width;
    }
    return width;
  }

  private bool IsLeafWidgetOwnerIsTextBox()
  {
    Entity entity = (Entity) this.GetOwnerParagraph();
    while (entity != null && entity.EntityType != EntityType.HeaderFooter && entity.EntityType != EntityType.Section && entity.Owner != null)
    {
      entity = entity.Owner;
      WTextBox wtextBox;
      if (entity is WTextBox && ((wtextBox = entity as WTextBox).TextBoxFormat.TextWrappingStyle == TextWrappingStyle.InFrontOfText || wtextBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Behind))
        return true;
    }
    return false;
  }

  private SizeF GetPageFieldSize(WField field)
  {
    IWSection currentSection = (this.m_lcOperator as Layouter).CurrentSection;
    string numberFormatValue = currentSection.PageSetup.GetNumberFormatValue((byte) currentSection.PageSetup.PageNumberStyle, (this.m_lcOperator as Layouter).PageNumber + 1);
    WCharacterFormat characterFormatValue = field.GetCharacterFormatValue();
    return this.DrawingContext.MeasureString(numberFormatValue, characterFormatValue.GetFontToRender(field.ScriptType), (StringFormat) null, characterFormatValue, false);
  }

  protected override void DoLayoutAfter()
  {
    FieldLayoutInfo layoutInfo = this.LayoutInfo as FieldLayoutInfo;
    Layouter lcOperator = this.m_lcOperator as Layouter;
    bool flag = layoutInfo != null;
    if (this.Widget is ParagraphItem && lcOperator.tocParaItems.Contains(this.Widget as ParagraphItem) && !lcOperator.IsLayoutingHeaderRow && this.m_ltWidget != null && lcOperator.LayoutingTOC == null)
      this.m_lcOperator.SendLeafLayoutAfter(this.m_ltWidget, true);
    if (flag && this.m_ltWidget != null && !lcOperator.IsLayoutingHeaderRow && lcOperator.LayoutingTOC == null)
      this.m_lcOperator.SendLeafLayoutAfter(this.m_ltWidget, false);
    if (this.m_ltWidget == null || !(this.m_ltWidget.Widget is BookmarkStart) || this.StartsWithExt((this.m_ltWidget.Widget as BookmarkStart).Name, "_"))
      return;
    this.m_lcOperator.SendLeafLayoutAfter(this.m_ltWidget, false);
  }

  private SizeF UpdateEQFieldWidth(DrawingContext dc, WCharacterFormat charFormat)
  {
    for (int index = 0; index < DocumentLayouter.EquationFields.Count; ++index)
    {
      if (DocumentLayouter.EquationFields[index].EQFieldEntity == this.LeafWidget)
      {
        LayoutedEQFields ltEQFiled = new LayoutedEQFields();
        dc.GenerateErrorFieldCode(ltEQFiled, 0.0f, 0.0f, charFormat);
        DocumentLayouter.EquationFields[index].LayouttedEQField = ltEQFiled;
        this.LeafWidget.LayoutInfo.Size = ltEQFiled.Bounds.Size;
      }
    }
    return this.LeafWidget.LayoutInfo.Size;
  }

  private void FitWidget(
    SizeF size,
    IWidget widget,
    bool isLastWordFit,
    float indentX,
    float indentY,
    bool isFloatingItem)
  {
    TabsLayoutInfo layoutInfo1 = widget.LayoutInfo as TabsLayoutInfo;
    ILayoutSpacingsInfo layoutInfo2 = this.LayoutInfo as ILayoutSpacingsInfo;
    Layouter lcOperator = this.m_lcOperator as Layouter;
    if (layoutInfo1 != null)
    {
      if (layoutInfo1.IsTabWidthUpdatedBasedOnIndent)
        lcOperator.PreviousTab = new TabsLayoutInfo.LayoutTab((widget as ParagraphItem).OwnerParagraph.ParagraphFormat.LeftIndent, TabJustification.Left, TabLeader.NoLeader);
      else
        (this.m_lcOperator as Layouter).PreviousTab = layoutInfo1.m_currTab;
    }
    double width = (double) size.Width + (layoutInfo2 != null ? (double) layoutInfo2.Paddings.Left + (double) layoutInfo2.Paddings.Right : 0.0);
    double height = (double) size.Height + (layoutInfo2 != null ? (double) layoutInfo2.Paddings.Top + (double) layoutInfo2.Paddings.Bottom : 0.0);
    ParagraphItem paraItem = this.LeafWidget is WOleObject ? (ParagraphItem) (this.LeafWidget as WOleObject).OlePicture : this.LeafWidget as ParagraphItem;
    WPicture wpicture = paraItem as WPicture;
    if (!isLastWordFit)
      width = this.UpdateLeafWidgetWidth(width, widget);
    ParagraphItem paragraphItem = widget as ParagraphItem;
    if (widget is SplitStringWidget)
      paragraphItem = (widget as SplitStringWidget).RealStringWidget as ParagraphItem;
    WParagraph wparagraph = paragraphItem.OwnerParagraph;
    if (paragraphItem.Owner is InlineContentControl || paragraphItem.Owner is XmlParagraphItem)
      wparagraph = paragraphItem.GetOwnerParagraphValue();
    bool isInCell = false;
    bool isInTextBox = false;
    if (wparagraph != null)
    {
      isInCell = wparagraph.IsInCell;
      isInTextBox = this.GetBaseEntity((Entity) wparagraph) is WTextBox;
    }
    int num;
    if (wpicture == null)
    {
      switch (paraItem)
      {
        case Shape _:
        case WTextBox _:
        case GroupShape _:
        case WChart _:
          break;
        default:
          num = 1;
          goto label_16;
      }
    }
    num = this.IsFitLeafWidgetInContainerHeight(paraItem, isInCell, isInTextBox, (Entity) null) ? 1 : 0;
label_16:
    if (num != 0 && height > (double) this.m_layoutArea.ClientArea.Height && (wparagraph == null || !wparagraph.IsZeroAutoLineSpace()) && !this.IsForceFitLayout)
      height = (double) this.m_layoutArea.ClientArea.Height;
    if (this.LayoutInfo.IsVerticalText && wpicture != null && wpicture.TextWrappingStyle == TextWrappingStyle.Inline && height > (double) this.m_layoutArea.ClientArea.Width)
    {
      CellLayoutInfo layoutInfo3 = ((IWidget) wparagraph.OwnerTextBody).LayoutInfo as CellLayoutInfo;
      height = (double) this.m_layoutArea.ClientArea.Width + (double) (((IWidget) wparagraph).LayoutInfo as ParagraphLayoutInfo).Margins.Right - 2.0 * ((double) layoutInfo3.Margins.Top + (double) layoutInfo3.Margins.Bottom) - ((double) layoutInfo3.Margins.Left + (double) layoutInfo3.Margins.Right);
    }
    this.m_ltWidget = new LayoutedWidget(widget);
    ILayoutSpacingsInfo layoutInfo4 = this.LayoutInfo as ILayoutSpacingsInfo;
    if (!isFloatingItem)
      this.m_ltWidget.Bounds = new RectangleF(this.m_layoutArea.ClientArea.X - (layoutInfo4 != null ? layoutInfo4.Paddings.Left : 0.0f) + indentX, this.m_layoutArea.ClientArea.Y - (layoutInfo4 != null ? layoutInfo4.Paddings.Top : 0.0f) + indentY, (float) width, (float) height);
    else
      this.m_ltWidget.Bounds = new RectangleF(indentX - (layoutInfo4 != null ? layoutInfo4.Paddings.Left : 0.0f), indentY - (layoutInfo4 != null ? layoutInfo4.Paddings.Top : 0.0f), (float) width, (float) height);
    if (!(this.m_ltWidget.Widget is WTextBox))
      this.m_ltWidget.PrevTabJustification = lcOperator.PreviousTab.Justification;
    if (!isLastWordFit)
      return;
    this.m_ltWidget.TextTag = "IsLastWordFit";
  }

  private bool GetFloattingItemPosition(ref float indentX, ref float indentY, ref SizeF size)
  {
    Layouter lcOperator = this.m_lcOperator as Layouter;
    bool floattingItemPosition = false;
    ParagraphItem paraItem = this.LeafWidget is WOleObject ? (ParagraphItem) (this.LeafWidget as WOleObject).OlePicture : this.LeafWidget as ParagraphItem;
    Shape shape1 = paraItem as Shape;
    WPicture wpicture = paraItem as WPicture;
    WTextBox wtextBox = paraItem as WTextBox;
    WChart wchart = paraItem as WChart;
    GroupShape groupShape = paraItem as GroupShape;
    float rightEdgeExtent = shape1 != null ? shape1.RightEdgeExtent : (groupShape != null ? groupShape.RightEdgeExtent : 0.0f);
    float leftEdgeExtent = shape1 != null ? shape1.LeftEdgeExtent : (groupShape != null ? groupShape.LeftEdgeExtent : 0.0f);
    switch (paraItem)
    {
      case WPicture _:
      case Shape _:
      case WTextBox _:
      case WChart _:
      case GroupShape _:
        floattingItemPosition = true;
        float leftMargin = 0.0f;
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        float num4 = 0.0f;
        float rightMargin = 0.0f;
        IWSection currentSection = lcOperator.CurrentSection;
        float num5 = 0.0f;
        float num6 = 0.0f;
        float num7 = 0.0f;
        float num8 = 0.0f;
        WParagraph ownerParagraphValue = paraItem.GetOwnerParagraphValue();
        ParagraphLayoutInfo paragraphLayoutInfo = (ParagraphLayoutInfo) null;
        if (ownerParagraphValue != null)
          paragraphLayoutInfo = ((IWidget) ownerParagraphValue).LayoutInfo as ParagraphLayoutInfo;
        SizeF sizeF;
        RectangleF rectangleF;
        if (paraItem.Owner != null)
        {
          leftMargin = Layouter.GetLeftMargin(currentSection as WSection);
          rightMargin = Layouter.GetRightMargin(currentSection as WSection);
          if (lcOperator.IsLayoutingHeaderFooter)
          {
            if (lcOperator.IsLayoutingHeader && (double) currentSection.PageSetup.Margins.Top <= 0.0)
            {
              double num9;
              if ((double) Math.Abs(currentSection.PageSetup.Margins.Top) <= 0.0)
              {
                double headerDistance = (double) currentSection.PageSetup.HeaderDistance;
                double num10;
                if (paragraphLayoutInfo == null)
                {
                  num10 = 0.0;
                }
                else
                {
                  sizeF = paragraphLayoutInfo.Size;
                  num10 = (double) sizeF.Height;
                }
                num9 = headerDistance + num10;
              }
              else
                num9 = (double) Math.Abs(currentSection.PageSetup.Margins.Top);
              num1 = (float) num9;
            }
            else
              num1 = (double) currentSection.PageSetup.Margins.Top > 0.0 ? currentSection.PageSetup.Margins.Top : 36f;
          }
          else
            num1 = lcOperator.CurrentSection.HeadersFooters.Header.Count == 0 || (double) currentSection.PageSetup.Margins.Top <= 0.0 ? Math.Abs(currentSection.PageSetup.Margins.Top) : currentSection.PageSetup.HeaderDistance;
          if ((double) currentSection.PageSetup.Margins.Gutter > 0.0 && currentSection.Document.DOP.GutterAtTop)
            num1 += currentSection.PageSetup.Margins.Gutter;
          if (!lcOperator.IsLayoutingHeaderFooter && (double) num1 < (double) lcOperator.ClientLayoutArea.Y && Math.Round((double) lcOperator.m_firstItemInPageYPosition, 2) >= Math.Round((double) lcOperator.ClientLayoutArea.Y, 2))
          {
            rectangleF = lcOperator.ClientLayoutArea;
            num1 = rectangleF.Y;
          }
          num2 = (double) currentSection.PageSetup.Margins.Bottom > 0.0 ? currentSection.PageSetup.Margins.Bottom : 36f;
          sizeF = currentSection.PageSetup.PageSize;
          num6 = sizeF.Height;
          sizeF = currentSection.PageSetup.PageSize;
          num5 = sizeF.Width;
          num7 = currentSection.PageSetup.ClientWidth;
          num4 = currentSection.PageSetup.FooterDistance;
          num3 = currentSection.PageSetup.HeaderDistance;
          sizeF = currentSection.PageSetup.PageSize;
          num8 = sizeF.Height - num1 - num2;
        }
        TextWrappingStyle textWrapStyle = shape1 != null ? shape1.WrapFormat.TextWrappingStyle : (wpicture != null ? wpicture.TextWrappingStyle : (wchart != null ? wchart.WrapFormat.TextWrappingStyle : (wtextBox != null ? wtextBox.TextBoxFormat.TextWrappingStyle : groupShape.WrapFormat.TextWrappingStyle)));
        bool flag1 = false;
        CellLayoutInfo cellLayoutInfo = (CellLayoutInfo) null;
        float cellWid = 0.0f;
        if (textWrapStyle != TextWrappingStyle.Inline)
        {
          VerticalOrigin verticalOrigin = paraItem.GetVerticalOrigin();
          HorizontalOrigin horizontalOrigin = paraItem.GetHorizontalOrigin();
          ShapeHorizontalAlignment horizontalAlignment = paraItem.GetShapeHorizontalAlignment();
          ShapeVerticalAlignment verticalAlignment = paraItem.GetShapeVerticalAlignment();
          float height1 = size.Height;
          if (shape1 != null && shape1.IsHorizontalRule)
          {
            double width1 = (double) size.Width;
            rectangleF = this.m_layoutArea.ClientActiveArea;
            double width2 = (double) rectangleF.Width;
            if (width1 > width2)
            {
              ref SizeF local = ref size;
              rectangleF = this.m_layoutArea.ClientActiveArea;
              double width3 = (double) rectangleF.Width;
              local.Width = (float) width3;
            }
          }
          float width4 = size.Width;
          float verticalPosition = paraItem.GetVerticalPosition();
          float horizontalPosition = paraItem.GetHorizontalPosition();
          bool flag2 = shape1 != null ? shape1.LayoutInCell : (wpicture != null ? wpicture.LayoutInCell : (wchart != null ? wchart.LayoutInCell : (groupShape != null ? groupShape.LayoutInCell : wtextBox == null || (wtextBox.Shape != null ? wtextBox.Shape.LayoutInCell : wtextBox.TextBoxFormat.AllowInCell))));
          if (ownerParagraphValue.IsInCell && (flag2 || ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013))
          {
            flag1 = true;
            cellLayoutInfo = this.GetCellLayoutInfo((Entity) ownerParagraphValue);
            cellWid = this.DrawingContext.GetCellWidth(paraItem);
            indentY = (float) this.GetVerticalPosition(paraItem, verticalPosition, verticalOrigin, textWrapStyle, cellLayoutInfo);
            indentX = (float) this.GetHorizontalPosition(size, paraItem, horizontalAlignment, horizontalOrigin, horizontalPosition, textWrapStyle, cellWid, rightEdgeExtent, leftEdgeExtent);
          }
          else
          {
            if (this.m_isYPositionUpdated)
            {
              ref float local = ref indentY;
              rectangleF = this.m_layoutArea.ClientArea;
              double y = (double) rectangleF.Y;
              local = (float) y;
            }
            else
            {
              switch (verticalOrigin)
              {
                case VerticalOrigin.Margin:
                  if (lcOperator.IsLayoutingHeader && (double) num3 > (double) num1)
                  {
                    ref float local = ref indentY;
                    double num11 = (double) num3;
                    double num12;
                    if (paragraphLayoutInfo == null)
                    {
                      num12 = 0.0;
                    }
                    else
                    {
                      sizeF = paragraphLayoutInfo.Size;
                      num12 = (double) sizeF.Height;
                    }
                    double num13 = num11 + num12 + (double) verticalPosition;
                    local = (float) num13;
                  }
                  else
                    indentY = num1 + verticalPosition;
                  switch (verticalAlignment)
                  {
                    case ShapeVerticalAlignment.None:
                      if (paraItem is WTextBox && (double) Math.Abs((paraItem as WTextBox).TextBoxFormat.VerticalRelativePercent) <= 1000.0)
                      {
                        float verticalRelativePercent = (paraItem as WTextBox).TextBoxFormat.VerticalRelativePercent;
                        float num14 = num8 * (verticalRelativePercent / 100f);
                        indentY = num1 + num14;
                        break;
                      }
                      break;
                    case ShapeVerticalAlignment.Top:
                      indentY = num1;
                      break;
                    case ShapeVerticalAlignment.Center:
                      indentY = num1 + (float) (((double) num8 - (double) height1) / 2.0);
                      break;
                    case ShapeVerticalAlignment.Bottom:
                    case ShapeVerticalAlignment.Outside:
                      if (lcOperator.CurrPageIndex % 2 != 0 || ownerParagraphValue != null && ownerParagraphValue.Document.MultiplePage != MultiplePage.MirrorMargins)
                      {
                        ref float local = ref indentY;
                        rectangleF = lcOperator.ClientLayoutArea;
                        double num15 = (double) rectangleF.Bottom - (double) height1;
                        local = (float) num15;
                        break;
                      }
                      indentY = num1;
                      break;
                    case ShapeVerticalAlignment.Inside:
                      indentY = lcOperator.CurrPageIndex % 2 == 0 || ownerParagraphValue != null && ownerParagraphValue.Document.MultiplePage != MultiplePage.MirrorMargins ? num1 + num8 - height1 : num1;
                      break;
                  }
                  break;
                case VerticalOrigin.Page:
                case VerticalOrigin.TopMargin:
                  indentY = verticalPosition;
                  switch (verticalAlignment)
                  {
                    case ShapeVerticalAlignment.None:
                      if (paraItem is WTextBox || paraItem is Shape)
                      {
                        float num16 = paraItem is Shape shape2 ? shape2.TextFrame.VerticalRelativePercent : (paraItem as WTextBox).TextBoxFormat.VerticalRelativePercent;
                        indentY = (double) Math.Abs(num16) > 1000.0 ? (shape2 != null ? shape2.VerticalPosition : (paraItem as WTextBox).TextBoxFormat.VerticalPosition) : (verticalOrigin != VerticalOrigin.Page ? num1 * (num16 / 100f) : num6 * (num16 / 100f));
                        break;
                      }
                      break;
                    case ShapeVerticalAlignment.Top:
                      indentY = verticalPosition;
                      break;
                    case ShapeVerticalAlignment.Center:
                      indentY = verticalOrigin != VerticalOrigin.TopMargin ? (float) (((double) num6 - (double) height1) / 2.0) : (float) (((double) num1 - (double) height1) / 2.0);
                      break;
                    case ShapeVerticalAlignment.Bottom:
                    case ShapeVerticalAlignment.Outside:
                      indentY = verticalOrigin != VerticalOrigin.Page || verticalAlignment != ShapeVerticalAlignment.Bottom ? (verticalOrigin != VerticalOrigin.TopMargin ? (lcOperator.CurrPageIndex % 2 == 0 ? num3 / 2f : (float) ((double) num6 - (double) height1 - (double) num4 / 2.0)) : num1 - height1) : num6 - height1;
                      break;
                    case ShapeVerticalAlignment.Inside:
                      if (verticalOrigin == VerticalOrigin.Page)
                      {
                        indentY = lcOperator.CurrPageIndex % 2 != 0 ? num3 / 2f : (float) ((double) num6 - (double) height1 - (double) num4 / 2.0);
                        break;
                      }
                      break;
                  }
                  break;
                case VerticalOrigin.Paragraph:
                  float num17 = 0.0f;
                  if (shape1 != null || wpicture != null)
                    num17 = this.GetFloatingItemSpacing(ownerParagraphValue);
                  indentY = (float) Math.Round((double) (this.m_lcOperator as Layouter).ParagraphYPosition, 2) + num17 + verticalPosition;
                  break;
                case VerticalOrigin.Line:
                  indentY = verticalPosition;
                  switch (verticalAlignment)
                  {
                    case ShapeVerticalAlignment.None:
                      ref float local1 = ref indentY;
                      rectangleF = this.m_layoutArea.ClientActiveArea;
                      double num18 = (double) rectangleF.Y + (double) verticalPosition;
                      local1 = (float) num18;
                      break;
                    case ShapeVerticalAlignment.Top:
                    case ShapeVerticalAlignment.Inside:
                      ref float local2 = ref indentY;
                      rectangleF = this.m_layoutArea.ClientActiveArea;
                      double y1 = (double) rectangleF.Y;
                      local2 = (float) y1;
                      break;
                    case ShapeVerticalAlignment.Center:
                      if (ownerParagraphValue.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013)
                      {
                        ref float local3 = ref indentY;
                        rectangleF = this.m_layoutArea.ClientActiveArea;
                        double num19 = (double) rectangleF.Y - (double) height1 / 2.0;
                        local3 = (float) num19;
                        break;
                      }
                      sizeF = ((IWidget) ownerParagraphValue).LayoutInfo.Size;
                      float height2 = sizeF.Height;
                      if ((double) ownerParagraphValue.ParagraphFormat.LineSpacing > 12.0)
                        height2 *= ownerParagraphValue.ParagraphFormat.LineSpacing / 12f;
                      int num20 = this.IsParagraphContainingLineBreakInFirst(ownerParagraphValue);
                      if (num20 == int.MinValue)
                      {
                        float num21 = (height2 + ownerParagraphValue.ParagraphFormat.BeforeSpacing + ownerParagraphValue.ParagraphFormat.AfterSpacing) / 2f;
                        ref float local4 = ref indentY;
                        rectangleF = this.m_layoutArea.ClientActiveArea;
                        double num22 = (double) rectangleF.Y - (double) ownerParagraphValue.ParagraphFormat.BeforeSpacing + (double) num21 - (double) height1 / 2.0;
                        local4 = (float) num22;
                        break;
                      }
                      float num23 = (height2 + ownerParagraphValue.ParagraphFormat.AfterSpacing) / 2f;
                      float num24;
                      if (num20 == 0)
                      {
                        double num25 = (double) num23;
                        sizeF = (ownerParagraphValue.ChildEntities[0] as ILeafWidget).Measure(this.DrawingContext);
                        double height3 = (double) sizeF.Height;
                        num24 = (float) (num25 + height3);
                      }
                      else
                        num24 = height2 / 2f;
                      ref float local5 = ref indentY;
                      rectangleF = this.m_layoutArea.ClientActiveArea;
                      double num26 = Math.Round((double) rectangleF.Y + (double) num24 - (double) height1 / 2.0);
                      local5 = (float) num26;
                      break;
                    case ShapeVerticalAlignment.Bottom:
                    case ShapeVerticalAlignment.Outside:
                      if (ownerParagraphValue.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013)
                      {
                        ref float local6 = ref indentY;
                        rectangleF = this.m_layoutArea.ClientActiveArea;
                        double num27 = (double) rectangleF.Y - (double) height1;
                        local6 = (float) num27;
                        break;
                      }
                      sizeF = ((IWidget) ownerParagraphValue).LayoutInfo.Size;
                      float height4 = sizeF.Height;
                      if ((double) ownerParagraphValue.ParagraphFormat.LineSpacing > 12.0)
                        height4 *= ownerParagraphValue.ParagraphFormat.LineSpacing / 12f;
                      ref float local7 = ref indentY;
                      rectangleF = this.m_layoutArea.ClientActiveArea;
                      double num28 = (double) rectangleF.Y + ((double) height4 + (double) ownerParagraphValue.ParagraphFormat.AfterSpacing - (double) height1);
                      local7 = (float) num28;
                      double num29 = (double) indentY;
                      rectangleF = lcOperator.ClientLayoutArea;
                      double y2 = (double) rectangleF.Y;
                      if (num29 < y2)
                      {
                        ref float local8 = ref indentY;
                        rectangleF = lcOperator.ClientLayoutArea;
                        double y3 = (double) rectangleF.Y;
                        local8 = (float) y3;
                        break;
                      }
                      break;
                  }
                  break;
                case VerticalOrigin.BottomMargin:
                  indentY = verticalPosition;
                  switch (verticalAlignment)
                  {
                    case ShapeVerticalAlignment.None:
                      indentY = num6 - num2 + verticalPosition;
                      if (paraItem is WTextBox && (double) Math.Abs((paraItem as WTextBox).TextBoxFormat.VerticalRelativePercent) <= 1000.0)
                      {
                        float num30 = num6 - num2;
                        float verticalRelativePercent = (paraItem as WTextBox).TextBoxFormat.VerticalRelativePercent;
                        float num31 = num2 * (verticalRelativePercent / 100f);
                        indentY = num30 + num31;
                        break;
                      }
                      break;
                    case ShapeVerticalAlignment.Top:
                    case ShapeVerticalAlignment.Inside:
                      indentY = num6 - num2;
                      break;
                    case ShapeVerticalAlignment.Center:
                      indentY = (float) ((double) num6 - (double) num2 + ((double) num2 - (double) height1) / 2.0);
                      break;
                    case ShapeVerticalAlignment.Bottom:
                    case ShapeVerticalAlignment.Outside:
                      indentY = lcOperator.CurrPageIndex % 2 == 0 ? num6 - num2 : num6 - height1;
                      break;
                  }
                  break;
                case VerticalOrigin.InsideMargin:
                case VerticalOrigin.OutsideMargin:
                  indentY = verticalPosition;
                  switch (verticalAlignment)
                  {
                    case ShapeVerticalAlignment.None:
                      if (paraItem is WTextBox && (double) Math.Abs((paraItem as WTextBox).TextBoxFormat.VerticalRelativePercent) <= 1000.0)
                      {
                        float verticalRelativePercent = (paraItem as WTextBox).TextBoxFormat.VerticalRelativePercent;
                        if ((lcOperator.CurrPageIndex % 2 == 0 ? (verticalOrigin == VerticalOrigin.InsideMargin ? 1 : 0) : (verticalOrigin == VerticalOrigin.OutsideMargin ? 1 : 0)) != 0)
                        {
                          float num32 = num6 - num2;
                          float num33 = num2 * (verticalRelativePercent / 100f);
                          indentY = num32 + num33;
                          break;
                        }
                        indentY = num1 * (verticalRelativePercent / 100f);
                        break;
                      }
                      break;
                    case ShapeVerticalAlignment.Top:
                      indentY = verticalOrigin != VerticalOrigin.InsideMargin ? (lcOperator.CurrPageIndex % 2 != 0 ? (ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? num6 - num2 : 0.0f) : (ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? 0.0f : num6 - num2)) : (lcOperator.CurrPageIndex % 2 != 0 ? 0.0f : num6 - num2);
                      break;
                    case ShapeVerticalAlignment.Center:
                      switch (verticalOrigin)
                      {
                        case VerticalOrigin.InsideMargin:
                          if (lcOperator.CurrPageIndex % 2 == 0)
                          {
                            indentY = (float) ((double) num6 - (double) num2 + ((double) num2 - (double) height1) / 2.0);
                            goto label_99;
                          }
                          break;
                        case VerticalOrigin.OutsideMargin:
                          indentY = lcOperator.CurrPageIndex % 2 != 0 ? (ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? (float) ((double) num6 - (double) num2 + ((double) num2 - (double) height1) / 2.0) : (float) (((double) num1 - (double) height1) / 2.0)) : (ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? (indentY = (float) (((double) num1 - (double) height1) / 2.0)) : (indentY = (float) ((double) num6 - (double) num2 + ((double) num2 - (double) height1) / 2.0)));
                          goto label_99;
                      }
                      indentY = (float) (((double) num1 - (double) height1) / 2.0);
                      break;
                    case ShapeVerticalAlignment.Bottom:
                      switch (verticalOrigin)
                      {
                        case VerticalOrigin.InsideMargin:
                          if (lcOperator.CurrPageIndex % 2 == 0)
                          {
                            indentY = num6 - height1;
                            goto label_99;
                          }
                          break;
                        case VerticalOrigin.OutsideMargin:
                          indentY = lcOperator.CurrPageIndex % 2 != 0 ? (ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? num6 - height1 : num1 - height1) : (ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? num1 - height1 : num6 - height1);
                          goto label_99;
                      }
                      indentY = num1 - height1;
                      break;
                    case ShapeVerticalAlignment.Inside:
                      indentY = verticalOrigin != VerticalOrigin.InsideMargin ? (lcOperator.CurrPageIndex % 2 != 0 ? (ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? num6 - num2 : 0.0f) : (ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? num1 - height1 : num6 - height1)) : (verticalOrigin != VerticalOrigin.InsideMargin || lcOperator.CurrPageIndex % 2 != 0 ? 0.0f : num6 - height1);
                      break;
                    case ShapeVerticalAlignment.Outside:
                      indentY = verticalOrigin != VerticalOrigin.InsideMargin ? (lcOperator.CurrPageIndex % 2 != 0 ? (ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? num6 - height1 : num1 - height1) : (ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? 0.0f : num6 - num2)) : (lcOperator.CurrPageIndex % 2 != 0 ? num1 - height1 : num6 - num2);
                      break;
                  }
                  break;
                default:
                  ref float local9 = ref indentY;
                  rectangleF = this.m_layoutArea.ClientArea;
                  double num34 = (double) rectangleF.Y - (this.LayoutInfo is ILayoutSpacingsInfo ? (double) (this.LayoutInfo as ILayoutSpacingsInfo).Paddings.Top : 0.0) + (double) verticalPosition;
                  local9 = (float) num34;
                  break;
              }
            }
label_99:
            if (this.m_isXPositionUpdated && horizontalOrigin != HorizontalOrigin.Column && horizontalAlignment != ShapeHorizontalAlignment.None)
            {
              ref float local10 = ref indentX;
              rectangleF = this.m_layoutArea.ClientArea;
              double x = (double) rectangleF.X;
              local10 = (float) x;
            }
            else if (ownerParagraphValue != null && ownerParagraphValue.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && textWrapStyle != TextWrappingStyle.InFrontOfText && textWrapStyle != TextWrappingStyle.Behind && verticalOrigin == VerticalOrigin.Paragraph && (double) width4 >= (double) num5)
            {
              indentX = 0.0f;
            }
            else
            {
              switch (horizontalOrigin)
              {
                case HorizontalOrigin.Margin:
                  if (currentSection != null)
                  {
                    indentX = leftMargin + horizontalPosition;
                    switch (horizontalAlignment)
                    {
                      case ShapeHorizontalAlignment.None:
                        if (paraItem is WTextBox && (double) Math.Abs((paraItem as WTextBox).TextBoxFormat.HorizontalRelativePercent) <= 1000.0)
                        {
                          float horizontalRelativePercent = (paraItem as WTextBox).TextBoxFormat.HorizontalRelativePercent;
                          float num35 = num7 * (horizontalRelativePercent / 100f);
                          indentX = leftMargin + num35;
                          break;
                        }
                        break;
                      case ShapeHorizontalAlignment.Left:
                        indentX = leftMargin + leftEdgeExtent;
                        break;
                      case ShapeHorizontalAlignment.Center:
                        indentX = leftMargin + (float) (((double) num7 - (double) width4) / 2.0);
                        break;
                      case ShapeHorizontalAlignment.Right:
                        indentX = leftMargin + num7 - width4 - rightEdgeExtent;
                        break;
                      case ShapeHorizontalAlignment.Inside:
                        if (lcOperator.CurrPageIndex % 2 == 0 || ownerParagraphValue != null && ownerParagraphValue.Document.MultiplePage != MultiplePage.MirrorMargins)
                        {
                          indentX = leftMargin + num7 - width4;
                          break;
                        }
                        break;
                      case ShapeHorizontalAlignment.Outside:
                        if (lcOperator.CurrPageIndex % 2 != 0 || ownerParagraphValue != null && ownerParagraphValue.Document.MultiplePage != MultiplePage.MirrorMargins)
                        {
                          indentX = leftMargin + num7 - width4;
                          break;
                        }
                        break;
                    }
                  }
                  else
                  {
                    ref float local11 = ref indentX;
                    rectangleF = this.m_layoutArea.ClientArea;
                    double num36 = (double) rectangleF.X + (double) horizontalPosition;
                    local11 = (float) num36;
                    break;
                  }
                  break;
                case HorizontalOrigin.Page:
                  indentX = horizontalPosition;
                  switch (horizontalAlignment)
                  {
                    case ShapeHorizontalAlignment.None:
                      if (flag1)
                      {
                        ref float local12 = ref indentX;
                        rectangleF = (((IWidget) ownerParagraphValue.OwnerTextBody).LayoutInfo as CellLayoutInfo).CellContentLayoutingBounds;
                        double num37 = (double) rectangleF.Left + (double) horizontalPosition;
                        local12 = (float) num37;
                        break;
                      }
                      switch (paraItem)
                      {
                        case WTextBox _:
                        case Shape _:
                          float num38 = paraItem is Shape shape3 ? shape3.TextFrame.HorizontalRelativePercent : (paraItem as WTextBox).TextBoxFormat.HorizontalRelativePercent;
                          indentX = (double) Math.Abs(num38) > 1000.0 ? (shape3 != null ? shape3.HorizontalPosition : (paraItem as WTextBox).TextBoxFormat.HorizontalPosition) : num5 * (num38 / 100f);
                          break;
                        default:
                          indentX = horizontalPosition;
                          break;
                      }
                      break;
                    case ShapeHorizontalAlignment.Left:
                      indentX = 0.0f + leftEdgeExtent;
                      break;
                    case ShapeHorizontalAlignment.Center:
                      indentX = !flag1 ? (float) (((double) num5 - (double) width4) / 2.0) : (float) (((double) this.DrawingContext.GetCellWidth(paraItem) - (double) width4) / 2.0);
                      break;
                    case ShapeHorizontalAlignment.Right:
                    case ShapeHorizontalAlignment.Outside:
                      indentX = !flag1 ? num5 - width4 : this.DrawingContext.GetCellWidth(paraItem) - width4;
                      indentX -= rightEdgeExtent;
                      break;
                  }
                  if ((double) indentX < 0.0 && flag1)
                  {
                    ref float local13 = ref indentX;
                    rectangleF = (((IWidget) ownerParagraphValue.OwnerTextBody).LayoutInfo as CellLayoutInfo).CellContentLayoutingBounds;
                    double left = (double) rectangleF.Left;
                    local13 = (float) left;
                    break;
                  }
                  break;
                case HorizontalOrigin.Column:
                  if (ownerParagraphValue.IsXpositionUpated)
                  {
                    rectangleF = lcOperator.ClientLayoutArea;
                    if ((double) rectangleF.Left >= (double) paragraphLayoutInfo.XPosition)
                    {
                      if (ownerParagraphValue.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && (textWrapStyle == TextWrappingStyle.InFrontOfText || textWrapStyle == TextWrappingStyle.Behind))
                      {
                        indentX = paragraphLayoutInfo.XPosition + horizontalPosition;
                        goto label_127;
                      }
                      ref float local14 = ref indentX;
                      rectangleF = lcOperator.ClientLayoutArea;
                      double num39 = (double) rectangleF.Left + (double) horizontalPosition;
                      local14 = (float) num39;
                      goto label_127;
                    }
                  }
                  float num40 = 0.0f;
                  if (ownerParagraphValue.IsInCell)
                  {
                    CellLayoutInfo layoutInfo = (ownerParagraphValue.GetOwnerEntity() as IWidget).LayoutInfo as CellLayoutInfo;
                    num40 = layoutInfo.Paddings.Left + layoutInfo.Paddings.Right;
                  }
                  float num41 = 18f;
                  if (textWrapStyle == TextWrappingStyle.Tight || textWrapStyle == TextWrappingStyle.Through)
                    num41 = ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? 17.6f : 8f;
                  float num42 = num41 - num40;
                  if ((ownerParagraphValue.IsXpositionUpated ? (ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? 1 : 0) : (!ownerParagraphValue.ParagraphFormat.IsFrame ? 1 : 0)) != 0 || (double) paragraphLayoutInfo.XPosition > (double) num5 - (double) num42 - (double) rightMargin || paragraphLayoutInfo.IsXPositionReUpdate)
                  {
                    ref float local15 = ref indentX;
                    rectangleF = lcOperator.ClientLayoutArea;
                    double num43 = (double) rectangleF.Left + (double) horizontalPosition;
                    local15 = (float) num43;
                  }
                  else
                    indentX = paragraphLayoutInfo.XPosition + horizontalPosition;
label_127:
                  if (textWrapStyle != TextWrappingStyle.InFrontOfText && textWrapStyle != TextWrappingStyle.Behind && Math.Round((double) indentX + (double) width4) > Math.Round((double) num5) && (double) width4 < (double) num5)
                    indentX = num5 - width4;
                  switch (horizontalAlignment)
                  {
                    case ShapeHorizontalAlignment.Left:
                      ref float local16 = ref indentX;
                      rectangleF = lcOperator.ClientLayoutArea;
                      double num44 = (double) rectangleF.Left + (double) leftEdgeExtent;
                      local16 = (float) num44;
                      break;
                    case ShapeHorizontalAlignment.Center:
                      ref float local17 = ref indentX;
                      rectangleF = lcOperator.ClientLayoutArea;
                      double left1 = (double) rectangleF.Left;
                      rectangleF = lcOperator.ClientLayoutArea;
                      double num45 = ((double) rectangleF.Width - (double) width4) / 2.0;
                      double num46 = left1 + num45;
                      local17 = (float) num46;
                      break;
                    case ShapeHorizontalAlignment.Right:
                      ref float local18 = ref indentX;
                      rectangleF = lcOperator.ClientLayoutArea;
                      double left2 = (double) rectangleF.Left;
                      rectangleF = lcOperator.ClientLayoutArea;
                      double width5 = (double) rectangleF.Width;
                      double num47 = left2 + width5 - (double) width4 - (double) rightEdgeExtent;
                      local18 = (float) num47;
                      break;
                  }
                  break;
                case HorizontalOrigin.Character:
                  switch (horizontalAlignment)
                  {
                    case ShapeHorizontalAlignment.Left:
                      ref float local19 = ref indentX;
                      rectangleF = this.m_layoutArea.ClientArea;
                      double num48 = (double) rectangleF.X + (double) horizontalPosition + (double) leftEdgeExtent;
                      local19 = (float) num48;
                      break;
                    case ShapeHorizontalAlignment.Center:
                    case ShapeHorizontalAlignment.Right:
                      indentX = this.GetLeftMarginHorizPosition(leftMargin, horizontalAlignment, horizontalPosition, width4, textWrapStyle, rightEdgeExtent, leftEdgeExtent, paraItem);
                      break;
                    default:
                      ref float local20 = ref indentX;
                      rectangleF = this.m_layoutArea.ClientArea;
                      double num49 = (double) rectangleF.X + (double) horizontalPosition;
                      local20 = (float) num49;
                      break;
                  }
                  break;
                case HorizontalOrigin.LeftMargin:
                  indentX = this.GetLeftMarginHorizPosition(leftMargin, horizontalAlignment, horizontalPosition, width4, textWrapStyle, rightEdgeExtent, leftEdgeExtent, paraItem);
                  break;
                case HorizontalOrigin.RightMargin:
                  indentX = this.GetRightMarginHorizPosition(num5, rightMargin, horizontalAlignment, horizontalPosition, width4, textWrapStyle, rightEdgeExtent, leftEdgeExtent, paraItem);
                  break;
                case HorizontalOrigin.InsideMargin:
                  indentX = lcOperator.CurrPageIndex % 2 != 0 ? this.GetLeftMarginHorizPosition(leftMargin, horizontalAlignment, horizontalPosition, width4, textWrapStyle, rightEdgeExtent, leftEdgeExtent, paraItem) : this.GetRightMarginHorizPosition(num5, rightMargin, horizontalAlignment, horizontalPosition, width4, textWrapStyle, rightEdgeExtent, leftEdgeExtent, paraItem);
                  break;
                case HorizontalOrigin.OutsideMargin:
                  indentX = lcOperator.CurrPageIndex % 2 != 0 ? this.GetRightMarginHorizPosition(num5, rightMargin, horizontalAlignment, horizontalPosition, width4, textWrapStyle, rightEdgeExtent, leftEdgeExtent, paraItem) : this.GetLeftMarginHorizPosition(leftMargin, horizontalAlignment, horizontalPosition, width4, textWrapStyle, rightEdgeExtent, leftEdgeExtent, paraItem);
                  break;
                default:
                  ref float local21 = ref indentX;
                  rectangleF = this.m_layoutArea.ClientArea;
                  double num50 = (double) rectangleF.X + (double) horizontalPosition;
                  local21 = (float) num50;
                  break;
              }
              if (ownerParagraphValue != null && ownerParagraphValue.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && textWrapStyle != TextWrappingStyle.InFrontOfText && textWrapStyle != TextWrappingStyle.Behind && verticalOrigin == VerticalOrigin.Paragraph && (double) num5 < (double) indentX + (double) width4)
                indentX = num5 - width4;
            }
          }
        }
        else
          floattingItemPosition = false;
        if (wpicture != null && (double) wpicture.Rotation != 0.0 && (wpicture.TextWrappingStyle == TextWrappingStyle.Behind || wpicture.TextWrappingStyle == TextWrappingStyle.InFrontOfText || wpicture.TextWrappingStyle == TextWrappingStyle.Square || wpicture.TextWrappingStyle == TextWrappingStyle.TopAndBottom))
          this.GetPictureWrappingBounds(ref indentX, ref indentY, ref size, wpicture.Rotation);
        VerticalOrigin verticalOrigin1 = paraItem.GetVerticalOrigin();
        if (textWrapStyle != TextWrappingStyle.Inline && textWrapStyle != TextWrappingStyle.InFrontOfText && textWrapStyle != TextWrappingStyle.Behind)
        {
          if (flag1)
          {
            rectangleF = cellLayoutInfo.CellContentLayoutingBounds;
            float top = rectangleF.Top;
            ILayoutSpacingsInfo layoutSpacingsInfo = (ILayoutSpacingsInfo) cellLayoutInfo;
            rectangleF = cellLayoutInfo.CellContentLayoutingBounds;
            float num51 = rectangleF.Left - layoutSpacingsInfo.Margins.Left;
            float num52 = cellWid - (layoutSpacingsInfo.Paddings.Left - layoutSpacingsInfo.Paddings.Right);
            if ((double) indentY < (double) top)
              indentY = top;
            if ((double) indentX < (double) num51 || (double) num52 < (double) size.Width)
            {
              indentX = num51;
              break;
            }
            break;
          }
          if (ownerParagraphValue != null && (verticalOrigin1 == VerticalOrigin.Paragraph || verticalOrigin1 == VerticalOrigin.Line) && !this.IsInFrame(ownerParagraphValue))
          {
            float height5 = size.Height;
            float width = size.Width;
            if ((double) this.GetAngle(paraItem) == 0.0)
            {
              if (ownerParagraphValue.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
              {
                if (!lcOperator.IsLayoutingHeaderFooter)
                {
                  double num53 = (double) indentY + (double) height5;
                  rectangleF = lcOperator.ClientLayoutArea;
                  double bottom = (double) rectangleF.Bottom;
                  if (num53 > bottom)
                  {
                    ref float local = ref indentY;
                    rectangleF = lcOperator.ClientLayoutArea;
                    double num54 = (double) rectangleF.Bottom - (double) height5;
                    local = (float) num54;
                  }
                  if ((double) indentY < (double) lcOperator.PageTopMargin)
                    indentY = lcOperator.PageTopMargin;
                  if ((double) indentX + (double) width > (double) num5)
                    indentX = num5 - width;
                  if ((double) indentX < 0.0)
                  {
                    indentX = 0.0f;
                    break;
                  }
                  break;
                }
                break;
              }
              if (!this.IsWord2013(currentSection.Document) && (lcOperator.IsLayoutingHeaderFooter ? (lcOperator.IsLayoutingHeader ? 1 : 0) : 1) != 0)
              {
                double num55 = (double) indentY + (double) height5;
                sizeF = currentSection.PageSetup.PageSize;
                double height6 = (double) sizeF.Height;
                if (num55 > height6)
                {
                  ref float local = ref indentY;
                  sizeF = currentSection.PageSetup.PageSize;
                  double num56 = (double) sizeF.Height - (double) height5;
                  local = (float) num56;
                }
              }
              if ((double) indentX < 0.0)
                indentX = 0.0f;
              if ((double) indentY < 0.0)
              {
                indentY = 0.0f;
                break;
              }
              break;
            }
            break;
          }
          break;
        }
        break;
    }
    return floattingItemPosition;
  }

  private CellLayoutInfo GetCellLayoutInfo(Entity entity)
  {
    while (!(entity is WTableCell))
      entity = entity.Owner;
    return (entity as IWidget).LayoutInfo as CellLayoutInfo;
  }

  private float GetFloatingItemSpacing(WParagraph paragraph)
  {
    IEntity previousSibling;
    if (paragraph.Document.DOP.Dop2000.Copts.DontUseHTMLParagraphAutoSpacing || this.IsForceFitLayout || (previousSibling = paragraph.PreviousSibling) == null || !(previousSibling is WParagraph) || ((double) paragraph.ParagraphFormat.BeforeSpacing <= (double) (previousSibling as WParagraph).ParagraphFormat.AfterSpacing || paragraph.ParagraphFormat.SpaceBeforeAuto) && (!paragraph.ParagraphFormat.SpaceBeforeAuto || (double) (previousSibling as WParagraph).ParagraphFormat.AfterSpacing >= 14.0) || paragraph.ParagraphFormat.ContextualSpacing)
      return 0.0f;
    return (previousSibling as WParagraph).ParagraphFormat.SpaceAfterAuto ? 14f : (previousSibling as WParagraph).ParagraphFormat.AfterSpacing;
  }

  private void GetPictureWrappingBounds(
    ref float indentX,
    ref float indentY,
    ref SizeF size,
    float angle)
  {
    RectangleF boundingBoxCoordinates = this.DrawingContext.GetBoundingBoxCoordinates(new RectangleF(indentX, indentY, size.Width, size.Height), angle);
    indentX = boundingBoxCoordinates.X;
    indentY = boundingBoxCoordinates.Y;
    size = boundingBoxCoordinates.Size;
  }

  private float GetRightMarginHorizPosition(
    float pageWidth,
    float rightMargin,
    ShapeHorizontalAlignment horzAlignment,
    float horzPosition,
    float shapeWidth,
    TextWrappingStyle textWrapStyle,
    float rightEdgeExtent,
    float leftEdgeExtent,
    ParagraphItem paraItem)
  {
    float num1 = pageWidth - rightMargin;
    float marginHorizPosition = num1 + horzPosition;
    switch (horzAlignment)
    {
      case ShapeHorizontalAlignment.None:
        if (paraItem is WTextBox && (double) Math.Abs((paraItem as WTextBox).TextBoxFormat.HorizontalRelativePercent) <= 1000.0)
        {
          float horizontalRelativePercent = (paraItem as WTextBox).TextBoxFormat.HorizontalRelativePercent;
          float num2 = rightMargin * (horizontalRelativePercent / 100f);
          marginHorizPosition = num1 + num2;
          break;
        }
        break;
      case ShapeHorizontalAlignment.Left:
        marginHorizPosition = num1 + leftEdgeExtent;
        break;
      case ShapeHorizontalAlignment.Center:
        marginHorizPosition = num1 + (float) (((double) rightMargin - (double) shapeWidth) / 2.0);
        break;
      case ShapeHorizontalAlignment.Right:
        marginHorizPosition = pageWidth - shapeWidth - rightEdgeExtent;
        break;
    }
    if (((double) marginHorizPosition < 0.0 || (double) marginHorizPosition + (double) shapeWidth > (double) pageWidth) && textWrapStyle != TextWrappingStyle.InFrontOfText && textWrapStyle != TextWrappingStyle.Behind)
      marginHorizPosition = pageWidth - shapeWidth;
    return marginHorizPosition;
  }

  private int IsParagraphContainingLineBreakInFirst(WParagraph paragraph)
  {
    for (int index = 0; index < paragraph.ChildEntities.Count; ++index)
    {
      if (!(paragraph.ChildEntities[index] is BookmarkStart) && !(paragraph.ChildEntities[index] is BookmarkEnd) && paragraph.ChildEntities[index] is Break && (paragraph.ChildEntities[index] as Break).BreakType == BreakType.LineBreak)
        return index;
    }
    return int.MinValue;
  }

  private float GetLeftMarginHorizPosition(
    float leftMargin,
    ShapeHorizontalAlignment horzAlignment,
    float horzPosition,
    float shapeWidth,
    TextWrappingStyle textWrapStyle,
    float rightEdgeExtent,
    float leftEdgeExtent,
    ParagraphItem paraItem)
  {
    float marginHorizPosition = horzPosition;
    switch (horzAlignment)
    {
      case ShapeHorizontalAlignment.None:
        if (paraItem is WTextBox && (double) Math.Abs((paraItem as WTextBox).TextBoxFormat.HorizontalRelativePercent) <= 1000.0)
        {
          float horizontalRelativePercent = (paraItem as WTextBox).TextBoxFormat.HorizontalRelativePercent;
          marginHorizPosition = leftMargin * (horizontalRelativePercent / 100f);
          break;
        }
        break;
      case ShapeHorizontalAlignment.Left:
        marginHorizPosition = 0.0f + leftEdgeExtent;
        break;
      case ShapeHorizontalAlignment.Center:
        marginHorizPosition = (float) (((double) leftMargin - (double) shapeWidth) / 2.0);
        break;
      case ShapeHorizontalAlignment.Right:
        marginHorizPosition = leftMargin - shapeWidth - rightEdgeExtent;
        break;
    }
    if ((double) marginHorizPosition < 0.0 && textWrapStyle != TextWrappingStyle.InFrontOfText && textWrapStyle != TextWrappingStyle.Behind)
      marginHorizPosition = 0.0f;
    return marginHorizPosition;
  }

  private bool IsFitLeafWidgetInContainerHeight(
    ParagraphItem paraItem,
    bool isInCell,
    bool isInTextBox,
    Entity ent)
  {
    if (!isInCell && !isInTextBox)
      return false;
    bool isWord2013 = paraItem.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013;
    switch (paraItem)
    {
      case WPicture _:
      case WChart _:
      case Shape _:
      case GroupShape _:
        return this.IsFitLeafWidgetInContainer(paraItem, isWord2013);
      case WTextBox _:
        if (isInCell && ent != null && ent is WTable)
          return isWord2013 || (paraItem as WTextBox).TextBoxFormat.AllowInCell || (paraItem as WTextBox).TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Inline;
        break;
    }
    return false;
  }

  private bool IsFitLeafWidgetInContainer(ParagraphItem paraItem, bool isWord2013)
  {
    return isWord2013 || paraItem.GetLayOutInCell() || paraItem.GetTextWrappingStyle() == TextWrappingStyle.Inline;
  }

  private double GetVerticalPosition(
    ParagraphItem paraItem,
    float vertPosition,
    VerticalOrigin vertOrigin,
    TextWrappingStyle textWrapStyle,
    CellLayoutInfo cellLayoutInfo)
  {
    WParagraph ownerParagraphValue = paraItem.GetOwnerParagraphValue();
    ParagraphLayoutInfo layoutInfo = ((IWidget) ownerParagraphValue).LayoutInfo as ParagraphLayoutInfo;
    Shape shape = paraItem as Shape;
    WPicture wpicture = paraItem as WPicture;
    double top = (double) cellLayoutInfo.CellContentLayoutingBounds.Top;
    double verticalPosition;
    switch (vertOrigin)
    {
      case VerticalOrigin.Margin:
      case VerticalOrigin.Page:
      case VerticalOrigin.TopMargin:
      case VerticalOrigin.BottomMargin:
      case VerticalOrigin.InsideMargin:
      case VerticalOrigin.OutsideMargin:
        verticalPosition = top + (double) vertPosition;
        break;
      case VerticalOrigin.Paragraph:
      case VerticalOrigin.Line:
        float num = 0.0f;
        if (shape != null || wpicture != null)
          num = this.GetFloatingItemSpacing(ownerParagraphValue);
        verticalPosition = (double) layoutInfo.YPosition + (double) vertPosition + (double) num;
        break;
      default:
        verticalPosition = (double) this.m_layoutArea.ClientActiveArea.Y + (double) vertPosition;
        break;
    }
    return verticalPosition;
  }

  private double GetHorizontalPosition(
    SizeF size,
    ParagraphItem paraItem,
    ShapeHorizontalAlignment horzAlignment,
    HorizontalOrigin horzOrigin,
    float horzPosition,
    TextWrappingStyle textWrapStyle,
    float cellWid,
    float rightEdgeExtent,
    float leftEdgeExtent)
  {
    double horizontalPosition = 0.0;
    CellLayoutInfo cellLayoutInfo = this.GetCellLayoutInfo((Entity) paraItem.GetOwnerParagraphValue());
    ILayoutSpacingsInfo layoutSpacingsInfo = (ILayoutSpacingsInfo) cellLayoutInfo;
    double num1 = (double) cellWid - (double) layoutSpacingsInfo.Paddings.Left - (double) layoutSpacingsInfo.Paddings.Right;
    double width = (double) cellLayoutInfo.CellContentLayoutingBounds.Width;
    double left = (double) cellLayoutInfo.CellContentLayoutingBounds.Left;
    double num2 = left - (double) layoutSpacingsInfo.Margins.Left;
    switch (horzOrigin)
    {
      case HorizontalOrigin.Margin:
      case HorizontalOrigin.Column:
        switch (horzAlignment)
        {
          case ShapeHorizontalAlignment.None:
            horizontalPosition = left + (double) horzPosition;
            break;
          case ShapeHorizontalAlignment.Left:
            horizontalPosition = left + (double) leftEdgeExtent;
            break;
          case ShapeHorizontalAlignment.Center:
            horizontalPosition = left + (width - (double) size.Width) / 2.0;
            break;
          case ShapeHorizontalAlignment.Right:
            horizontalPosition = left + (width - (double) size.Width) - (double) rightEdgeExtent;
            break;
        }
        break;
      case HorizontalOrigin.Page:
        horizontalPosition = (double) horzPosition;
        switch (horzAlignment)
        {
          case ShapeHorizontalAlignment.None:
            horizontalPosition = num2 + (double) horzPosition;
            break;
          case ShapeHorizontalAlignment.Left:
            horizontalPosition = num2 + (double) leftEdgeExtent;
            break;
          case ShapeHorizontalAlignment.Center:
            horizontalPosition = num2 + (num1 - (double) size.Width) / 2.0;
            break;
          case ShapeHorizontalAlignment.Right:
            horizontalPosition = num2 + (num1 - (double) size.Width) - (double) rightEdgeExtent;
            break;
        }
        break;
      case HorizontalOrigin.LeftMargin:
        switch (horzAlignment)
        {
          case ShapeHorizontalAlignment.None:
            horizontalPosition = num2 + (double) horzPosition;
            break;
          case ShapeHorizontalAlignment.Left:
            horizontalPosition = num2 + (double) leftEdgeExtent;
            break;
          case ShapeHorizontalAlignment.Center:
            horizontalPosition = (num2 - (double) size.Width) / 2.0;
            break;
          case ShapeHorizontalAlignment.Right:
            horizontalPosition = num2 - (double) size.Width - (double) rightEdgeExtent;
            break;
        }
        break;
      default:
        horizontalPosition = left + (double) horzPosition;
        break;
    }
    return horizontalPosition;
  }

  private double UpdateLeafWidgetWidth(double width, IWidget widget)
  {
    ParagraphItem paraItem = widget as ParagraphItem;
    if (widget is SplitStringWidget)
      paraItem = (widget as SplitStringWidget).RealStringWidget as ParagraphItem;
    WParagraph ownerParagraph = paraItem.OwnerParagraph;
    if (paraItem.Owner is InlineContentControl || paraItem.Owner is XmlParagraphItem)
      ownerParagraph = paraItem.GetOwnerParagraphValue();
    WPicture wpicture = this.LeafWidget is WOleObject ? (this.LeafWidget as WOleObject).OlePicture : this.LeafWidget as WPicture;
    Shape leafWidget1 = this.LeafWidget as Shape;
    WTextBox leafWidget2 = this.LeafWidget as WTextBox;
    WChart leafWidget3 = this.LeafWidget as WChart;
    GroupShape leafWidget4 = this.LeafWidget as GroupShape;
    Layouter lcOperator = this.m_lcOperator as Layouter;
    if (wpicture == null && leafWidget1 == null && leafWidget2 == null && leafWidget3 == null && leafWidget4 == null && width > (double) this.m_layoutArea.ClientArea.Width && (!(this.LeafWidget is WSymbol) || ownerParagraph == null || !ownerParagraph.IsNeedToFitSymbol(ownerParagraph)) && (ownerParagraph.IsInCell || !this.IsTextRangeNeedToFit()) && (lcOperator.PreviousTab.Justification != TabJustification.Right && lcOperator.PreviousTab.Justification != TabJustification.Centered || this.IsLeafWidgetIsInCell(paraItem) && lcOperator.PreviousTab.Justification == TabJustification.Right))
    {
      if (ownerParagraph.IsInCell)
        return (double) this.m_layoutArea.ClientActiveArea.Width + (double) ((ownerParagraph.GetOwnerEntity() as IWidget).LayoutInfo as CellLayoutInfo).Margins.Right;
      return ownerParagraph.OwnerTextBody.OwnerBase != null && ownerParagraph.OwnerTextBody.OwnerBase is WTextBox && (double) (ownerParagraph.OwnerTextBody.OwnerBase as WTextBox).TextBoxFormat.Width > 0.0 ? (double) this.m_layoutArea.ClientActiveArea.Width + (double) (ownerParagraph.OwnerTextBody.OwnerBase as WTextBox).TextBoxFormat.InternalMargin.Right : (double) this.m_layoutArea.ClientArea.Width;
    }
    if ((wpicture != null || leafWidget3 != null || leafWidget2 != null || leafWidget1 != null || leafWidget4 != null) && this.IsNeedToAddCellBounds(this.LeafWidget is WOleObject ? wpicture.TextWrappingStyle : (this.LeafWidget as ParagraphItem).GetTextWrappingStyle()) && ownerParagraph.IsInCell)
    {
      WTableCell ownerEntity = ownerParagraph.GetOwnerEntity() as WTableCell;
      CellLayoutInfo layoutInfo = ((IWidget) ownerEntity).LayoutInfo as CellLayoutInfo;
      if (width > (double) this.m_layoutArea.ClientActiveArea.Width + (double) layoutInfo.Margins.Right && !this.LayoutInfo.IsVerticalText)
        return width <= (double) ownerEntity.Width ? width : (double) ownerEntity.Width;
      if (width > (double) this.m_layoutArea.ClientActiveArea.Width + (double) layoutInfo.Margins.Bottom && this.LayoutInfo.IsVerticalText)
        return (double) this.m_layoutArea.ClientArea.Width + (double) layoutInfo.Margins.Bottom;
    }
    return width;
  }

  private bool IsNeedToAddCellBounds(TextWrappingStyle textWrappingStyle)
  {
    return textWrappingStyle != TextWrappingStyle.InFrontOfText && textWrappingStyle != TextWrappingStyle.Behind;
  }

  private void SplitUpWidget(ISplitLeafWidget splitLeafWidget, float clientActiveAreaWidth)
  {
    ISplitLeafWidget[] splitLeafWidgetArray = (ISplitLeafWidget[]) null;
    bool isLastWordFit = false;
    if (this.LayoutInfo is TabsLayoutInfo)
    {
      float pageMarginLeft = this.GetPageMarginLeft();
      float nextTabPosition = (float) (this.LayoutInfo as TabsLayoutInfo).GetNextTabPosition((double) this.m_layoutArea.ClientActiveArea.X - (double) pageMarginLeft);
      if ((this.m_lcOperator as Layouter).IsFirstItemInLine && Math.Round((double) this.m_layoutArea.ClientActiveArea.X + (double) nextTabPosition - (double) pageMarginLeft, 2) >= Math.Round((double) this.ClientLayoutAreaRight, 2))
      {
        SizeF size = new SizeF(clientActiveAreaWidth, 0.0f);
        this.m_ltState = LayoutState.Fitted;
        this.FitWidget(size, (IWidget) splitLeafWidget, isLastWordFit, 0.0f, 0.0f, false);
        return;
      }
      if ((double) this.m_layoutArea.ClientArea.Size.Width != 0.0)
        splitLeafWidgetArray = new ISplitLeafWidget[2]
        {
          splitLeafWidget,
          splitLeafWidget
        };
    }
    else
    {
      Layouter lcOperator = this.m_lcOperator as Layouter;
      float width = lcOperator.ClientLayoutArea.Width;
      WParagraph ownerParagraph = this.GetOwnerParagraph();
      if (ownerParagraph != null)
      {
        if (this.IsInFrame(ownerParagraph))
          width = lcOperator.FrameLayoutArea.Width;
        WTableCell wtableCell = (WTableCell) null;
        if (ownerParagraph.IsInCell)
        {
          wtableCell = ownerParagraph.GetOwnerEntity() as WTableCell;
          width = wtableCell.Width;
        }
        if (wtableCell != null && ((IWidget) wtableCell).LayoutInfo.IsVerticalText)
          width = (((IWidget) wtableCell).LayoutInfo as CellLayoutInfo).CellContentLayoutingBounds.Width;
      }
      bool isTabStopInterSectingfloattingItem = false;
      if ((double) lcOperator.RightPositionOfTabStopInterSectingFloattingItems != -3.4028234663852886E+38)
        isTabStopInterSectingfloattingItem = true;
      int consecutiveLimit = lcOperator.CountForConsecutiveLimit;
      splitLeafWidgetArray = splitLeafWidget.SplitBySize(this.DrawingContext, this.m_layoutArea.ClientArea.Size, width, clientActiveAreaWidth, ref isLastWordFit, isTabStopInterSectingfloattingItem, lcOperator.m_canSplitbyCharacter, lcOperator.IsFirstItemInLine, ref consecutiveLimit);
      lcOperator.CountForConsecutiveLimit = consecutiveLimit;
    }
    this.m_ltState = LayoutState.NotFitted;
    if (splitLeafWidgetArray == null)
      return;
    SizeF size1 = splitLeafWidgetArray[0].Measure(this.DrawingContext);
    if (splitLeafWidgetArray[0].LayoutInfo is TabsLayoutInfo)
      size1.Width = 0.0f;
    if ((double) size1.Width > (double) this.m_layoutArea.ClientArea.Width && !isLastWordFit)
      size1.Width = this.m_layoutArea.ClientArea.Width;
    this.FitWidget(size1, (IWidget) splitLeafWidgetArray[0], isLastWordFit, 0.0f, 0.0f, false);
    if (splitLeafWidgetArray[1] != null)
    {
      this.m_sptWidget = (IWidget) splitLeafWidgetArray[1];
      this.m_ltState = LayoutState.Splitted;
    }
    else
      this.m_ltState = LayoutState.Fitted;
  }
}
