// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutContext
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
using System.Globalization;

#nullable disable
namespace Syncfusion.Layouting;

internal abstract class LayoutContext
{
  internal const float DEF_LEFT_MIN_WIDTH_SQUARE = 18f;
  internal const float DEF_LEFT_MIN_WIDTH_2013_TIGHTANDTHROW = 18f;
  internal const float DEF_LEFT_MIN_WIDTH_TIGHTANDTHROW = 9f;
  internal const float DEF_MIN_WIDTH_SQUARE = 18f;
  internal const float DEF_MIN_WIDTH_2013_TIGHTANDTHROW = 17.6f;
  internal const float DEF_MIN_WIDTH_TIGHTANDTHROW = 8f;
  internal const float MAX_WIDTH = 1584f;
  private const float BottomOverlapDifferenceForTightAndThroughWrappingStyle = 2f;
  internal LayoutState m_ltState;
  protected IWidget m_sptWidget;
  protected IWidget m_notFittedWidget;
  protected IWidget m_LineNumberWidget;
  protected IWidget m_widget;
  protected LayoutedWidget m_ltWidget;
  internal bool m_bSkipAreaSpacing;
  protected LayoutArea m_layoutArea;
  protected ILCOperator m_lcOperator;
  private float m_clientLayoutAreaRight;
  protected byte m_bFlags = 16 /*0x10*/;

  public IWidget SplittedWidget
  {
    get => this.m_sptWidget;
    set => this.m_sptWidget = value;
  }

  public LayoutState State => this.m_ltState;

  public ILayoutInfo LayoutInfo => this.m_widget.LayoutInfo;

  public LayoutArea LayoutArea => this.m_layoutArea;

  public DrawingContext DrawingContext => this.m_lcOperator.DrawingContext;

  public float BoundsPaddingRight
  {
    get
    {
      float boundsPaddingRight = 0.0f;
      if (this.m_widget.LayoutInfo is ILayoutSpacingsInfo)
        boundsPaddingRight = (this.m_widget.LayoutInfo as ILayoutSpacingsInfo).Paddings.Right + (this.m_widget.LayoutInfo as ILayoutSpacingsInfo).Margins.Right;
      return boundsPaddingRight;
    }
  }

  public float BoundsMarginBottom
  {
    get
    {
      float boundsMarginBottom = 0.0f;
      if (this.m_widget.LayoutInfo is ILayoutSpacingsInfo)
        boundsMarginBottom = (this.m_widget.LayoutInfo as ILayoutSpacingsInfo).Margins.Bottom;
      return boundsMarginBottom;
    }
  }

  public IWidget Widget
  {
    get => this.m_widget;
    set => this.m_widget = value;
  }

  public bool IsVerticalNotFitted
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsInnerLayouting
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsAreaUpdated
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsTabStopBeyondRightMarginExists
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal bool IsNeedToWrap
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  protected bool IsForceFitLayout
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  internal float ClientLayoutAreaRight
  {
    get => this.m_clientLayoutAreaRight;
    set => this.m_clientLayoutAreaRight = value;
  }

  public LayoutContext(IWidget widget, ILCOperator lcOperator, bool isForceFitLayout)
  {
    this.m_widget = widget;
    this.m_sptWidget = widget;
    this.m_lcOperator = lcOperator;
    this.IsForceFitLayout = isForceFitLayout;
  }

  public abstract LayoutedWidget Layout(RectangleF rect);

  public bool IsEnsureSplitted()
  {
    return this.State == LayoutState.Splitted && this.SplittedWidget != null;
  }

  protected virtual void DoLayoutAfter()
  {
  }

  internal void LayoutFootnote(
    WFootnote footnote,
    LayoutedWidget currLtWidget,
    bool isFootnoteRefrencedlineLayouted)
  {
    if (!this.IsNeedToWrap)
      return;
    float height = 0.0f;
    WParagraph ownerParagraphValue1 = footnote.GetOwnerParagraphValue();
    bool isClippedLine = false;
    bool isTextInLine = false;
    bool isFirstLineOfParagraph1 = false;
    bool isLastLineOfParagraph1 = false;
    if (currLtWidget.ChildWidgets.Count > 0)
    {
      isFirstLineOfParagraph1 = ownerParagraphValue1.IsFirstLine(currLtWidget.ChildWidgets[0]);
      isLastLineOfParagraph1 = ownerParagraphValue1.IsLastLine(currLtWidget.ChildWidgets[currLtWidget.ChildWidgets.Count - 1]);
    }
    IStringWidget lastTextWidget = (IStringWidget) null;
    LayoutedWidget layoutedWidget = (LayoutedWidget) null;
    double maxHeight;
    double maxAscent;
    double maxTextHeight;
    double maxTextAscent;
    double maxTextDescent;
    float maxY;
    double maxAscentOfLoweredPos;
    bool containsInlinePicture;
    bool isAllWordsContainLoweredPos;
    if (isFootnoteRefrencedlineLayouted)
    {
      bool isFirstLineOfParagraph2 = false;
      layoutedWidget = currLtWidget.Owner.ChildWidgets[currLtWidget.Owner.ChildWidgets.Count - 1];
      bool isLastLineOfParagraph2 = ownerParagraphValue1.IsLastLine(layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1]);
      layoutedWidget.CalculateMaxChildWidget(DocumentLayouter.DrawingContext, ownerParagraphValue1, isFirstLineOfParagraph2, isLastLineOfParagraph2, out maxHeight, out maxAscent, out maxTextHeight, out maxTextAscent, out maxTextDescent, out maxY, out maxAscentOfLoweredPos, out lastTextWidget, out isClippedLine, out isTextInLine, out containsInlinePicture, out isAllWordsContainLoweredPos);
    }
    else
      currLtWidget.CalculateMaxChildWidget(DocumentLayouter.DrawingContext, ownerParagraphValue1, isFirstLineOfParagraph1, isLastLineOfParagraph1, out maxHeight, out maxAscent, out maxTextHeight, out maxTextAscent, out maxTextDescent, out maxY, out maxAscentOfLoweredPos, out lastTextWidget, out isClippedLine, out isTextInLine, out containsInlinePicture, out isAllWordsContainLoweredPos);
    float num1 = Math.Abs(ownerParagraphValue1.ParagraphFormat.LineSpacing);
    if (maxHeight != 0.0 || maxTextHeight != 0.0)
    {
      switch (ownerParagraphValue1.ParagraphFormat.LineSpacingRule)
      {
        case LineSpacingRule.AtLeast:
          if (maxHeight < (double) num1)
          {
            maxHeight = (double) num1;
            break;
          }
          break;
        case LineSpacingRule.Exactly:
          maxHeight = (double) Math.Abs(num1);
          break;
        case LineSpacingRule.Multiple:
          if ((double) num1 != 12.0)
          {
            maxHeight += maxTextHeight * ((double) num1 / 12.0) - maxTextHeight;
            break;
          }
          break;
      }
    }
    float num2 = 0.0f;
    ILayoutInfo layoutInfo = ownerParagraphValue1.m_layoutInfo;
    if (layoutInfo is ParagraphLayoutInfo)
      num2 = !isFootnoteRefrencedlineLayouted ? (float) (((double) maxY != 3.4028234663852886E+38 ? (double) maxY : (double) currLtWidget.Bounds.Y) + maxHeight) + (layoutInfo as ParagraphLayoutInfo).Margins.Bottom + currLtWidget.m_footnoteHeight : (float) (((double) maxY != 3.4028234663852886E+38 ? (double) maxY : (double) layoutedWidget.Bounds.Y) + maxHeight) + (layoutInfo as ParagraphLayoutInfo).Margins.Bottom + currLtWidget.m_footnoteHeight;
    float clientHeight = this.m_layoutArea.ClientActiveArea.Bottom - num2;
    bool flag = ownerParagraphValue1.GetOwnerEntity() is WTableCell ownerEntity && (ownerParagraphValue1.IsExactlyRowHeight() || this.IsExactlyRowVerticalMergeStartCell(ownerEntity) || ownerEntity.m_layoutInfo != null && ownerEntity.m_layoutInfo.IsVerticalText);
    if (flag)
      clientHeight = this.GetFootNoteLayoutingHeight();
    if ((this.m_lcOperator as Layouter).IsNeedToRestartFootnote)
    {
      (this.m_lcOperator as Layouter).IsLayoutingFootnote = true;
      this.LayoutFootnoteTextBody((IWidgetContainer) footnote.Document.Footnotes.Separator, ref height, clientHeight, isFootnoteRefrencedlineLayouted);
      if (flag)
        clientHeight -= height;
      (this.m_lcOperator as Layouter).IsLayoutingFootnote = false;
      (this.m_lcOperator as Layouter).IsNeedToRestartFootnote = false;
    }
    float num3 = 0.0f;
    if (footnote.TextBody.LastParagraph != null)
      num3 = this.DrawingContext.MeasureString(" ", footnote.TextBody.LastParagraph.BreakCharacterFormat.GetFontToRender(FontScriptType.English), (StringFormat) null).Height;
    if ((double) num3 > (double) clientHeight)
    {
      if (footnote.Owner is WParagraph && (footnote.Owner as WParagraph).IsInCell)
      {
        if (!(((footnote.Owner as WParagraph).GetOwnerEntity() as WTableCell).OwnerRow.m_layoutInfo as RowLayoutInfo).IsFootnoteSplitted)
          (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Add(new SplitWidgetContainer((IWidgetContainer) footnote.TextBody, (IWidget) footnote.TextBody.Items[0], 0));
      }
      else
        (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Add(new SplitWidgetContainer((IWidgetContainer) footnote.TextBody, (IWidget) footnote.TextBody.Items[0], 0));
      if ((this.m_lcOperator as Layouter).IsNeedToRestartFootnoteID)
      {
        DocumentLayouter.m_footnoteIDRestartEachPage = 1;
        (this.m_lcOperator as Layouter).IsNeedToRestartFootnoteID = false;
      }
      WParagraph ownerParagraphValue2 = footnote.GetOwnerParagraphValue();
      Entity entity = (Entity) null;
      if (ownerParagraphValue2 != null)
        entity = this.GetBaseEntity((Entity) ownerParagraphValue2);
      if (entity != null && entity is WSection && (entity as WSection).PageSetup.RestartIndexForFootnotes == FootnoteRestartIndex.RestartForEachPage)
      {
        (footnote.m_layoutInfo as FootnoteLayoutInfo).FootnoteID = (footnote.m_layoutInfo as FootnoteLayoutInfo).GetFootnoteID(footnote, DocumentLayouter.m_footnoteIDRestartEachPage++);
        if (footnote.CustomMarkerIsSymbol || footnote.CustomMarker != string.Empty)
          --DocumentLayouter.m_footnoteIDRestartEachPage;
      }
    }
    else
    {
      (this.m_lcOperator as Layouter).IsLayoutingFootnote = true;
      this.LayoutFootnoteTextBody((IWidgetContainer) footnote.TextBody, ref height, clientHeight, isFootnoteRefrencedlineLayouted);
      (this.m_lcOperator as Layouter).IsLayoutingFootnote = false;
    }
    currLtWidget.m_footnoteHeight += height;
    (footnote.m_layoutInfo as FootnoteLayoutInfo).FootnoteHeight = height;
    if ((this.m_lcOperator as Layouter).FootnoteWidgets.Count >= 2)
      return;
    (this.m_lcOperator as Layouter).FootnoteWidgets.Clear();
    (this.m_lcOperator as Layouter).FootNoteSectionIndex.Clear();
  }

  internal bool IsNeedToConsiderAdjustValues(
    ref float adjustingValue,
    WParagraph paragraph,
    TextWrappingStyle textWrappingStyle,
    int index)
  {
    Layouter lcOperator = this.m_lcOperator as Layouter;
    WSection baseEntity = this.GetBaseEntity((Entity) paragraph) as WSection;
    bool considerAdjustValues = textWrappingStyle == TextWrappingStyle.TopAndBottom && this.IsWord2013(paragraph.Document) && lcOperator.FloatingItems[index].FloatingEntity is WTextBox && baseEntity != null;
    if (considerAdjustValues)
    {
      WTextBox floatingEntity = lcOperator.FloatingItems[index].FloatingEntity as WTextBox;
      if (floatingEntity.CharacterFormat.Border != null && floatingEntity.IsShape)
      {
        Shape shape = floatingEntity.Shape;
        adjustingValue = shape.LineFormat != null ? this.AdjustingValueToWrap(baseEntity.PageSetup.Margins.Left, shape.LineFormat.Weight, shape.LineFormat.Line) : 0.0f;
      }
    }
    return considerAdjustValues;
  }

  internal float AdjustingValueToWrap(float margin, float borderValue, bool hasBorder)
  {
    string[] strArray = margin.ToString((IFormatProvider) CultureInfo.InvariantCulture).Split('.');
    if (hasBorder)
    {
      float num1 = 0.8f;
      foreach (char ch in strArray[0])
      {
        int num2 = int.Parse(ch.ToString());
        for (int index = 0; index < num2; ++index)
        {
          if (Math.Round((double) num1, 2) == 0.8)
            ++num1;
          else if (Math.Round((double) num1, 2) == 1.8)
            num1 -= 0.5f;
          else if (Math.Round((double) num1, 2) == 1.3)
            num1 -= 0.5f;
        }
      }
      if (strArray.Length == 2)
      {
        float num3 = float.Parse("0." + strArray[1]);
        if ((double) num1 + (double) num3 < 2.05)
        {
          num1 += num3;
        }
        else
        {
          float num4 = num3 - (2.05f - num1);
          float num5 = 2.05f;
          if ((double) num4 >= 0.25)
          {
            float num6 = num4 - 0.25f;
            num1 = num5 - 1.25f + num6;
          }
          else
            num1 = (float) (2.0499999523162842 - 5.0 * (double) num4);
        }
      }
      return (double) borderValue < 2.25 ? num1 : 1.5f * (float) ((int) (((double) borderValue - 2.25) / 1.5) + 1) + num1;
    }
    float wrap = 0.0f;
    if ((double) margin <= 10.0)
      return 0.0f;
    margin -= 10f;
    if ((double) margin % 15.0 == 0.0)
      wrap = 0.3f;
    return wrap;
  }

  private float GetFootNoteLayoutingHeight()
  {
    Layouter lcOperator = this.m_lcOperator as Layouter;
    float noteLayoutingHeight = lcOperator.ClientLayoutArea.Bottom - this.m_layoutArea.ClientActiveArea.Bottom;
    if (lcOperator.FootnoteWidgets.Count > 0)
      noteLayoutingHeight -= lcOperator.FootnoteWidgets[lcOperator.FootnoteWidgets.Count - 1].Bounds.Bottom - lcOperator.FootnoteWidgets[0].Bounds.Y;
    return noteLayoutingHeight;
  }

  private bool IsExactlyRowVerticalMergeStartCell(WTableCell cell)
  {
    return (this.m_lcOperator as Layouter).IsLayoutingVerticalMergeStartCell && cell.OwnerRow.HeightType == TableRowHeightType.Exactly;
  }

  internal WTextBox IsEntityOwnerIsWTextbox(Entity entity)
  {
    while (entity != null)
    {
      if (entity.EntityType == EntityType.HeaderFooter || entity.EntityType == EntityType.Section || entity.Owner == null)
        return (WTextBox) null;
      entity = entity.Owner;
      if (entity is WTextBox)
        return entity as WTextBox;
    }
    return (WTextBox) null;
  }

  internal bool IsWord2013(WordDocument document)
  {
    return document.Settings.CompatibilityMode == CompatibilityMode.Word2013;
  }

  internal bool IsNotWord2013Jusitfy(WParagraph paragraph)
  {
    if (paragraph == null || !this.IsWord2013(paragraph.Document) || !(paragraph.m_layoutInfo is ParagraphLayoutInfo))
      return true;
    return (paragraph.m_layoutInfo as ParagraphLayoutInfo).Justification != HAlignment.Justify && (paragraph.m_layoutInfo as ParagraphLayoutInfo).Justification != HAlignment.Distributed;
  }

  internal float GetTotalTopMarginAndPaddingValues(WTable table)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    Entity owner = table.Owner;
    while (true)
    {
      switch (owner)
      {
        case WTable _:
        case WTableRow _:
        case WTableCell _:
          if (owner is WTableCell)
          {
            ILayoutInfo layoutInfo = (owner as WTableCell).m_layoutInfo;
            if (layoutInfo is CellLayoutInfo)
            {
              num1 += (layoutInfo as CellLayoutInfo).TopPadding;
              num2 += (layoutInfo as CellLayoutInfo).Margins.Top;
            }
          }
          owner = owner.Owner;
          continue;
        default:
          goto label_6;
      }
    }
label_6:
    return num1 + num2;
  }

  internal void RemoveBehindWidgets(LayoutedWidget ltWidget)
  {
    if ((this.m_lcOperator as Layouter).BehindWidgets.Count == 0)
      return;
    switch (ltWidget.Widget is SplitWidgetContainer ? ((ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer as Entity).EntityType : (ltWidget.Widget as Entity).EntityType)
    {
      case EntityType.Paragraph:
        if (ltWidget.ChildWidgets.Count > 0 && ltWidget.ChildWidgets[0].Widget is WParagraph)
        {
          this.RemoveFromLayoutedParagraph(ltWidget);
          break;
        }
        this.RemoveFromLayoutedLine(ltWidget);
        break;
      case EntityType.Table:
        this.RemoveFromLayoutedTable(ltWidget);
        break;
      case EntityType.TableRow:
        this.RemoveFromLayoutedRow(ltWidget);
        break;
    }
  }

  private void RemoveFromLayoutedTable(LayoutedWidget ltTable)
  {
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) ltTable.ChildWidgets)
      this.RemoveFromLayoutedRow(childWidget);
  }

  private void RemoveFromLayoutedRow(LayoutedWidget ltRow)
  {
    foreach (LayoutedWidget childWidget1 in (List<LayoutedWidget>) ltRow.ChildWidgets)
    {
      foreach (LayoutedWidget childWidget2 in (List<LayoutedWidget>) childWidget1.ChildWidgets[0].ChildWidgets)
        this.RemoveFromLayoutedParagraph(childWidget2);
    }
  }

  private void RemoveFromLayoutedParagraph(LayoutedWidget ltWidget)
  {
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) ltWidget.ChildWidgets)
      this.RemoveFromLayoutedLine(childWidget);
  }

  private void RemoveFromLayoutedLine(LayoutedWidget lineLtWidget)
  {
    Layouter lcOperator = this.m_lcOperator as Layouter;
    if (lcOperator.BehindWidgets.Count == 0)
      return;
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) lineLtWidget.ChildWidgets)
    {
      if (childWidget.IsBehindWidget() && lcOperator.BehindWidgets.Contains(childWidget))
        lcOperator.BehindWidgets.Remove(childWidget);
    }
  }

  internal void LayoutEndnote(WFootnote endnote, LayoutedWidget currLtWidget)
  {
    if (!this.IsNeedToWrap)
      return;
    float height = 0.0f;
    ILayoutInfo layoutInfo = endnote.m_layoutInfo;
    float num1 = currLtWidget.Bounds.Bottom + currLtWidget.m_endnoteHeight;
    if ((this.m_lcOperator as Layouter).IsNeedToRestartEndnote)
    {
      this.LayoutEndnoteTextBody((IWidgetContainer) endnote.Document.Footnotes.Separator, ref height, this.m_layoutArea.ClientActiveArea.Bottom - num1);
      (this.m_lcOperator as Layouter).IsNeedToRestartEndnote = false;
    }
    float num2 = 0.0f;
    if (endnote.TextBody.LastParagraph != null)
      num2 = this.DrawingContext.MeasureString(" ", endnote.TextBody.LastParagraph.BreakCharacterFormat.GetFontToRender(FontScriptType.English), (StringFormat) null).Height;
    if ((double) num2 > (double) this.m_layoutArea.ClientActiveArea.Bottom - (double) num1)
    {
      (this.m_lcOperator as Layouter).EndnoteSplittedWidgets.Add(new SplitWidgetContainer((IWidgetContainer) endnote.TextBody, (IWidget) endnote.TextBody.Items[0], 0));
      if ((this.m_lcOperator as Layouter).IsNeedToRestartEndnote)
      {
        DocumentLayouter.m_footnoteIDRestartEachPage = 1;
        (this.m_lcOperator as Layouter).IsNeedToRestartEndnote = false;
      }
      WParagraph ownerParagraphValue = endnote.GetOwnerParagraphValue();
      Entity entity = (Entity) null;
      if (ownerParagraphValue != null)
        entity = this.GetBaseEntity((Entity) ownerParagraphValue);
      if (entity != null && entity is WSection && ownerParagraphValue != null && (entity as WSection).PageSetup.RestartIndexForFootnotes == FootnoteRestartIndex.RestartForEachPage && layoutInfo is FootnoteLayoutInfo)
      {
        (layoutInfo as FootnoteLayoutInfo).FootnoteID = (layoutInfo as FootnoteLayoutInfo).GetFootnoteID(endnote, DocumentLayouter.m_endnoteIDRestartEachPage++);
        if (endnote.CustomMarkerIsSymbol || endnote.CustomMarker != string.Empty)
          --DocumentLayouter.m_endnoteIDRestartEachPage;
      }
    }
    else
      this.LayoutEndnoteTextBody((IWidgetContainer) endnote.TextBody, ref height, this.m_layoutArea.ClientActiveArea.Bottom - num1);
    currLtWidget.m_endnoteHeight += height;
    if (layoutInfo is FootnoteLayoutInfo)
      (layoutInfo as FootnoteLayoutInfo).Endnoteheight = height;
    if ((this.m_lcOperator as Layouter).EndnoteWidgets.Count >= 2)
      return;
    (this.m_lcOperator as Layouter).EndnoteWidgets.Clear();
    (this.m_lcOperator as Layouter).EndNoteSectionIndex.Clear();
  }

  internal void LayoutFootnoteTextBody(
    IWidgetContainer widgetContainer,
    ref float height,
    float clientHeight,
    bool referencedLineIsLayouted)
  {
    bool splitbyCharacter = (this.m_lcOperator as Layouter).m_canSplitbyCharacter;
    bool mCanSplitByTab = (this.m_lcOperator as Layouter).m_canSplitByTab;
    bool isFirstItemInLine = (this.m_lcOperator as Layouter).IsFirstItemInLine;
    List<float> lineSpaceWidths = (this.m_lcOperator as Layouter).m_lineSpaceWidths;
    float effectiveJustifyWidth = (this.m_lcOperator as Layouter).m_effectiveJustifyWidth;
    LayoutContext layoutContext = LayoutContext.Create((IWidget) widgetContainer, this.m_lcOperator, false);
    float y = (this.m_lcOperator as Layouter).ClientLayoutArea.Y;
    if ((this.m_lcOperator as Layouter).FootnoteWidgets.Count > 0)
      y = (this.m_lcOperator as Layouter).FootnoteWidgets[(this.m_lcOperator as Layouter).FootnoteWidgets.Count - 1].Bounds.Bottom;
    if ((this.m_lcOperator as Layouter).FootnoteWidgets.Count == 1)
      clientHeight -= (this.m_lcOperator as Layouter).FootnoteWidgets[0].Bounds.Height;
    RectangleF rect = new RectangleF((this.m_lcOperator as Layouter).ClientLayoutArea.X, y, (this.m_lcOperator as Layouter).ClientLayoutArea.Width, clientHeight);
    layoutContext.ClientLayoutAreaRight = rect.Width;
    LayoutedWidget layoutedWidget = layoutContext.Layout(rect);
    (this.m_lcOperator as Layouter).ResetWordLayoutingFlags(splitbyCharacter, mCanSplitByTab, isFirstItemInLine, lineSpaceWidths, effectiveJustifyWidth);
    if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
      return;
    if (layoutContext.State == LayoutState.Splitted || layoutContext.State == LayoutState.NotFitted)
    {
      if (layoutContext.SplittedWidget is SplitWidgetContainer)
      {
        (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Add(layoutContext.SplittedWidget as SplitWidgetContainer);
        (this.m_lcOperator as Layouter).FootnoteWidgets.Add(layoutedWidget);
        height += layoutedWidget.Bounds.Height;
      }
      else
      {
        if (!(layoutContext.SplittedWidget is WTextBody))
          return;
        (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Add(new SplitWidgetContainer(widgetContainer, (IWidget) (layoutContext.SplittedWidget as WTextBody).Items[0], 0));
      }
    }
    else
    {
      WTextBody textBody = layoutedWidget.Widget is WTextBody ? layoutedWidget.Widget as WTextBody : (!(layoutedWidget.Widget is SplitWidgetContainer) || !((layoutedWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WTextBody) ? (WTextBody) null : (layoutedWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WTextBody);
      if (textBody != null && this.IsFootnoteSplitted(textBody))
      {
        (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Add(new SplitWidgetContainer(widgetContainer, (IWidget) textBody.Items[0], 0));
      }
      else
      {
        (this.m_lcOperator as Layouter).FootnoteWidgets.Add(layoutedWidget);
        if (textBody != null && textBody.Owner is WFootnote)
          (textBody.Owner as WFootnote).IsLayouted = true;
        height += layoutedWidget.Bounds.Height;
      }
    }
  }

  private bool IsFootnoteSplitted(WTextBody textBody)
  {
    WParagraph ownerParagraphValue = textBody.Owner is WFootnote ? (textBody.Owner as WFootnote).GetOwnerParagraphValue() : (WParagraph) null;
    if (ownerParagraphValue != null && ownerParagraphValue.IsInCell)
    {
      Entity ownerEntity = ownerParagraphValue.GetOwnerEntity();
      if (ownerEntity is WTableCell)
        return (((ownerEntity as WTableCell).Owner as WTableRow).m_layoutInfo as RowLayoutInfo).IsFootnoteSplitted;
    }
    return false;
  }

  internal void LayoutEndnoteTextBody(
    IWidgetContainer widgetContainer,
    ref float height,
    float clientHeight)
  {
    bool splitbyCharacter = (this.m_lcOperator as Layouter).m_canSplitbyCharacter;
    bool mCanSplitByTab = (this.m_lcOperator as Layouter).m_canSplitByTab;
    bool isFirstItemInLine = (this.m_lcOperator as Layouter).IsFirstItemInLine;
    List<float> lineSpaceWidths = (this.m_lcOperator as Layouter).m_lineSpaceWidths;
    float effectiveJustifyWidth = (this.m_lcOperator as Layouter).m_effectiveJustifyWidth;
    LayoutContext layoutContext = LayoutContext.Create((IWidget) widgetContainer, this.m_lcOperator, false);
    float y = (this.m_lcOperator as Layouter).ClientLayoutArea.Y;
    if ((this.m_lcOperator as Layouter).EndnoteWidgets.Count > 0)
      y = (this.m_lcOperator as Layouter).EndnoteWidgets[(this.m_lcOperator as Layouter).EndnoteWidgets.Count - 1].Bounds.Bottom;
    if ((this.m_lcOperator as Layouter).EndnoteWidgets.Count == 1)
      clientHeight -= (this.m_lcOperator as Layouter).EndnoteWidgets[0].Bounds.Height;
    RectangleF rect = new RectangleF((this.m_lcOperator as Layouter).ClientLayoutArea.X, y, (this.m_lcOperator as Layouter).ClientLayoutArea.Width, clientHeight);
    LayoutedWidget layoutedWidget = layoutContext.Layout(rect);
    (this.m_lcOperator as Layouter).ResetWordLayoutingFlags(splitbyCharacter, mCanSplitByTab, isFirstItemInLine, lineSpaceWidths, effectiveJustifyWidth);
    if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
      return;
    if ((this.m_lcOperator as Layouter).EndnoteSplittedWidgets.Count == 0)
    {
      if (layoutContext.State == LayoutState.Splitted || layoutContext.State == LayoutState.NotFitted)
      {
        if (layoutContext.SplittedWidget is SplitWidgetContainer)
        {
          (this.m_lcOperator as Layouter).EndnoteSplittedWidgets.Add(layoutContext.SplittedWidget as SplitWidgetContainer);
          (this.m_lcOperator as Layouter).EndnoteWidgets.Add(layoutedWidget);
        }
        else if (layoutContext.SplittedWidget is WTextBody)
          (this.m_lcOperator as Layouter).EndnoteSplittedWidgets.Add(new SplitWidgetContainer(widgetContainer, (IWidget) (layoutContext.SplittedWidget as WTextBody).Items[0], 0));
      }
      else
        (this.m_lcOperator as Layouter).EndnoteWidgets.Add(layoutedWidget);
    }
    else if (layoutContext.SplittedWidget is SplitWidgetContainer)
    {
      if (layoutContext.State == LayoutState.Splitted && layoutContext.Widget is WTextBody)
        (this.m_lcOperator as Layouter).EndnoteSplittedWidgets.Add(new SplitWidgetContainer(widgetContainer, (IWidget) (layoutContext.Widget as WTextBody).Items[0], 0));
      else
        (this.m_lcOperator as Layouter).EndnoteSplittedWidgets.Add(layoutContext.SplittedWidget as SplitWidgetContainer);
    }
    else if (layoutContext.SplittedWidget is WTextBody)
      (this.m_lcOperator as Layouter).EndnoteSplittedWidgets.Add(new SplitWidgetContainer(widgetContainer, (IWidget) (layoutContext.SplittedWidget as WTextBody).Items[0], 0));
    height += layoutedWidget.Bounds.Height;
  }

  internal void AddLayoutWidgetInBeforeInsectingPoint(LayoutedWidget interSectWidget, int index)
  {
    LayoutedWidget layoutedWidget = (this.m_lcOperator as Layouter).MaintainltWidget;
    while (!(layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1].Widget is ParagraphItem) && !(layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1].Widget is SplitStringWidget) && !(layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1].Widget is WTable))
      layoutedWidget = layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1];
    float x = interSectWidget.Bounds.X;
    float y = interSectWidget.Bounds.Y;
    float width = this.m_layoutArea.ClientActiveArea.Width;
    bool flag = interSectWidget.Owner != null && interSectWidget.Owner.Widget is WParagraph;
    ParagraphLayoutInfo paragraphInfo = (ParagraphLayoutInfo) null;
    if (interSectWidget.Widget is WParagraph && (interSectWidget.Widget as WParagraph).m_layoutInfo != null)
      paragraphInfo = interSectWidget.Widget.LayoutInfo as ParagraphLayoutInfo;
    else if (interSectWidget.Widget is SplitWidgetContainer && (interSectWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
      paragraphInfo = ((interSectWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph).m_layoutInfo as ParagraphLayoutInfo;
    if (interSectWidget.Widget is WTable)
      x = this.m_layoutArea.ClientActiveArea.X;
    else if (interSectWidget.Widget is SplitWidgetContainer && (interSectWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WTable)
      x = this.m_layoutArea.ClientActiveArea.X;
    if (paragraphInfo != null)
    {
      if (flag)
      {
        x = paragraphInfo.XPosition;
        if (index > 0)
          paragraphInfo.IsFirstLine = false;
        else if (index == 0)
        {
          interSectWidget.UpdateParaFirstLineHorizontalPositions(paragraphInfo, interSectWidget.Widget, ref x, ref width);
          width -= (double) x - (double) this.m_layoutArea.ClientActiveArea.X > 0.0 ? x - this.m_layoutArea.ClientActiveArea.X : 0.0f;
        }
      }
      else
        x -= paragraphInfo.Margins.Left;
      y -= paragraphInfo.Margins.Top;
    }
    if (flag && index == 0)
      this.m_layoutArea.UpdateDynamicRelayoutBounds(x, y, true, width);
    else
      this.m_layoutArea.UpdateDynamicRelayoutBounds(x, y, false, 0.0f);
    for (int index1 = 0; index1 < index; ++index1)
      this.m_ltWidget.ChildWidgets.Add(interSectWidget.Owner.ChildWidgets[index1]);
    if (!(this.m_ltWidget.Widget is WSection) && (!(this.m_ltWidget.Widget is SplitWidgetContainer) || !((this.m_ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WSection)) || this.m_ltWidget.ChildWidgets.Count <= 0)
      return;
    this.UpdateWrappingDifferenceValue(this.m_ltWidget.ChildWidgets[0]);
  }

  internal void UpdateWrappingDifferenceValue(LayoutedWidget firstItem)
  {
    if ((double) (this.m_lcOperator as Layouter).WrappingDifference != -3.4028234663852886E+38 || (this.m_lcOperator as Layouter).IsLayoutingHeaderFooter || firstItem == null || firstItem.ChildWidgets.Count <= 0 || !this.IsBaseFromSection(firstItem.Widget is SplitWidgetContainer ? (firstItem.Widget as SplitWidgetContainer).RealWidgetContainer as Entity : firstItem.Widget as Entity))
      return;
    if (firstItem.Widget is WTable)
      (this.m_lcOperator as Layouter).WrappingDifference = firstItem.Bounds.Y - (this.m_lcOperator as Layouter).ClientLayoutArea.Y;
    else
      (this.m_lcOperator as Layouter).WrappingDifference = firstItem.ChildWidgets[0].Bounds.Y - (this.m_lcOperator as Layouter).ClientLayoutArea.Y;
  }

  internal void UpdateFootnoteWidgets(LayoutedWidget ltWidget)
  {
    if (ltWidget.Widget is WParagraph)
    {
      this.UpdateFootnoteWidgets(ltWidget.Widget as WParagraph);
    }
    else
    {
      for (int index1 = 0; index1 < ltWidget.ChildWidgets.Count; ++index1)
      {
        if (ltWidget.ChildWidgets[index1].Widget is WFootnote)
        {
          for (int index2 = (this.m_lcOperator as Layouter).FootnoteWidgets.Count - 1; index2 >= 0; --index2)
          {
            if (((this.m_lcOperator as Layouter).FootnoteWidgets[index2].Widget is WTextBody ? (Entity) ((this.m_lcOperator as Layouter).FootnoteWidgets[index2].Widget as WTextBody) : (Entity) (((this.m_lcOperator as Layouter).FootnoteWidgets[index2].Widget as SplitWidgetContainer).RealWidgetContainer as WTextBody)).Owner as WFootnote == ltWidget.ChildWidgets[index1].Widget)
            {
              (this.m_lcOperator as Layouter).FootnoteWidgets.RemoveAt(index2);
              (ltWidget.ChildWidgets[index1].Widget as WFootnote).IsLayouted = false;
              ltWidget.ChildWidgets[index1].Widget.InitLayoutInfo();
              break;
            }
          }
          for (int index3 = (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Count - 1; index3 >= 0; --index3)
          {
            if (((this.m_lcOperator as Layouter).FootnoteSplittedWidgets[index3].RealWidgetContainer as WTextBody).Owner as WFootnote == ltWidget.ChildWidgets[index1].Widget)
            {
              (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.RemoveAt(index3);
              ltWidget.ChildWidgets[index1].Widget.InitLayoutInfo();
              break;
            }
          }
        }
      }
      if ((this.m_lcOperator as Layouter).FootnoteWidgets.Count >= 2 || !this.IsNeedToRemoveFootnoteSeparator(ltWidget))
        return;
      (this.m_lcOperator as Layouter).FootnoteWidgets.Clear();
      (this.m_lcOperator as Layouter).FootNoteSectionIndex.Clear();
    }
  }

  private bool IsNeedToRemoveFootnoteSeparator(LayoutedWidget ltWidget)
  {
    WParagraph wparagraph = ltWidget.Widget is WParagraph ? ltWidget.Widget as WParagraph : (ltWidget.Widget is SplitWidgetContainer ? (ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph : (WParagraph) null);
    if (wparagraph == null || !wparagraph.ParagraphFormat.WidowControl || !(this.m_lcOperator as Layouter).IsTwoLinesLayouted || ltWidget.ChildWidgets.Count <= 0)
      return true;
    while (ltWidget.ChildWidgets.Count > 0 && !(ltWidget.ChildWidgets[ltWidget.ChildWidgets.Count - 1].Widget is ParagraphItem) && !(ltWidget.ChildWidgets[ltWidget.ChildWidgets.Count - 1].Widget is SplitStringWidget))
      ltWidget = ltWidget.ChildWidgets[ltWidget.ChildWidgets.Count - 1];
    return wparagraph.IsLastLine(ltWidget.ChildWidgets[ltWidget.ChildWidgets.Count - 1]);
  }

  internal void UpdateFootnoteWidgets(WParagraph paragraph)
  {
    for (int index1 = 0; index1 < paragraph.ChildEntities.Count; ++index1)
    {
      if (paragraph.ChildEntities[index1] is WFootnote)
      {
        for (int index2 = (this.m_lcOperator as Layouter).FootnoteWidgets.Count - 1; index2 >= 0; --index2)
        {
          WTextBody wtextBody = (this.m_lcOperator as Layouter).FootnoteWidgets[index2].Widget is WTextBody ? (this.m_lcOperator as Layouter).FootnoteWidgets[index2].Widget as WTextBody : (((this.m_lcOperator as Layouter).FootnoteWidgets[index2].Widget as SplitWidgetContainer).RealWidgetContainer is WTextBody ? ((this.m_lcOperator as Layouter).FootnoteWidgets[index2].Widget as SplitWidgetContainer).RealWidgetContainer as WTextBody : (WTextBody) null);
          if (wtextBody != null && wtextBody.Owner as WFootnote == paragraph.ChildEntities[index1])
          {
            (this.m_lcOperator as Layouter).FootnoteWidgets.RemoveAt(index2);
            (paragraph.ChildEntities[index1] as WFootnote).IsLayouted = false;
            (this.m_lcOperator as Layouter).FootNoteSectionIndex.RemoveAt(index2);
            (paragraph.ChildEntities[index1] as IWidget).InitLayoutInfo();
            break;
          }
        }
        for (int index3 = (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Count - 1; index3 >= 0; --index3)
        {
          if (((this.m_lcOperator as Layouter).FootnoteSplittedWidgets[index3].RealWidgetContainer as WTextBody).Owner as WFootnote == paragraph.ChildEntities[index1])
          {
            (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.RemoveAt(index3);
            (paragraph.ChildEntities[index1] as IWidget).InitLayoutInfo();
            break;
          }
        }
      }
    }
    if ((this.m_lcOperator as Layouter).FootnoteWidgets.Count >= 2)
      return;
    (this.m_lcOperator as Layouter).FootnoteWidgets.Clear();
    (this.m_lcOperator as Layouter).FootNoteSectionIndex.Clear();
  }

  protected void CreateLayoutArea(RectangleF rect)
  {
    WParagraph wparagraph = this.m_widget is WParagraph ? this.m_widget as WParagraph : (!(this.m_widget is SplitWidgetContainer) || !((this.m_widget as SplitWidgetContainer).RealWidgetContainer is WParagraph) ? (WParagraph) null : (this.m_widget as SplitWidgetContainer).RealWidgetContainer as WParagraph);
    if (wparagraph != null && this.LayoutInfo is ParagraphLayoutInfo && !(this.LayoutInfo as ParagraphLayoutInfo).IsFirstLine && this.LayoutInfo is ILayoutSpacingsInfo)
      (this.LayoutInfo as ParagraphLayoutInfo).Paddings.Top = 0.0f;
    if (this.m_widget is WParagraph && (this.m_lcOperator as Layouter).IsTabWidthUpdatedBasedOnIndent)
      (this.m_lcOperator as Layouter).IsTabWidthUpdatedBasedOnIndent = false;
    this.LayoutInfo.IsFirstItemInPage = false;
    if (this.m_bSkipAreaSpacing)
    {
      this.m_layoutArea = new LayoutArea(rect);
      this.UpdateParagraphYPositionBasedonTextWrap();
    }
    else
    {
      this.m_layoutArea = new LayoutArea(rect, this.LayoutInfo as ILayoutSpacingsInfo, this.m_widget);
      bool flag = wparagraph == null || wparagraph.GetOwnerSection() == null || wparagraph.GetOwnerSection().Columns.Count < 2 ? this.IsForceFitLayout : this.LayoutInfo is ParagraphLayoutInfo && Math.Round((double) (this.LayoutInfo as ParagraphLayoutInfo).YPosition, 2) == Math.Round((double) (this.m_lcOperator as Layouter).PageTopMargin, 2);
      if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && flag)
      {
        this.LayoutInfo.IsFirstItemInPage = true;
        float topPad = this.LayoutInfo is ILayoutSpacingsInfo ? (this.LayoutInfo as ILayoutSpacingsInfo).Margins.Top + (this.LayoutInfo as ILayoutSpacingsInfo).Paddings.Top : 0.0f;
        WParagraph paragraph = this.m_widget is WParagraph ? this.m_widget as WParagraph : (this.m_widget is SplitWidgetContainer ? (this.m_widget as SplitWidgetContainer).RealWidgetContainer as WParagraph : (WParagraph) null);
        if (paragraph != null && !paragraph.IsInCell)
        {
          this.UpdateParagraphTopMargin(paragraph);
          if ((double) (this.LayoutInfo as ParagraphLayoutInfo).Margins.Top == 0.0 && (paragraph.PreviousSibling is WParagraph && ((paragraph.PreviousSibling as WParagraph).ParagraphFormat.IsInFrame() || (paragraph.PreviousSibling as WParagraph).Text != "") || paragraph.PreviousSibling is WTable && paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013))
            this.m_layoutArea = new LayoutArea(rect, this.LayoutInfo as ILayoutSpacingsInfo, this.m_widget);
          if (((double) (this.LayoutInfo as ParagraphLayoutInfo).Paddings.Top != 0.0 || (double) (this.LayoutInfo as ParagraphLayoutInfo).Margins.Top != 0.0) && this.LayoutInfo is ParagraphLayoutInfo && (this.LayoutInfo as ParagraphLayoutInfo).IsFirstLine)
            this.m_layoutArea.UpdateBounds(topPad);
        }
      }
      if (!(this.m_widget is WParagraph) && (!(this.m_widget is SplitWidgetContainer) || !((this.m_widget as SplitWidgetContainer).RealWidgetContainer is WParagraph) || !flag))
        return;
      this.LayoutTextWrapWidgets(this.m_widget);
    }
  }

  private void LayoutTextWrapWidgets(IWidget widget)
  {
    if ((!(widget is WParagraph) || (widget as WParagraph).IsFloatingItemsLayouted) && !(widget is SplitWidgetContainer))
      return;
    WParagraph paragraph = (WParagraph) null;
    int num = 0;
    if (widget is SplitWidgetContainer)
    {
      Entity owner = (widget as SplitWidgetContainer).WidgetInnerCollection.Owner;
      if (owner is WParagraph)
      {
        paragraph = owner as WParagraph;
        num = paragraph.ChildEntities.IndexOf((widget as SplitWidgetContainer).m_currentChild as IEntity);
      }
    }
    else
      paragraph = widget as WParagraph;
    Layouter lcOperator = this.m_lcOperator as Layouter;
    if (num >= 0)
    {
      for (int index = num; index < paragraph.ChildEntities.Count; ++index)
      {
        if (paragraph.ChildEntities[index] != null && paragraph.ChildEntities[index].IsFloatingItem(true) && paragraph.ChildEntities[index] is IWidget && !(paragraph.ChildEntities[index] as IWidget).LayoutInfo.IsSkip && (paragraph.ChildEntities[index] as ParagraphItem).GetHorizontalOrigin() != HorizontalOrigin.Character && (paragraph.ChildEntities[index] as ParagraphItem).GetVerticalOrigin() != VerticalOrigin.Line && (lcOperator.NotFittedFloatingItems.Count <= 0 || !lcOperator.NotFittedFloatingItems.Contains(paragraph.ChildEntities[index])))
        {
          LayoutedWidget ltWidget = LayoutContext.Create(paragraph.ChildEntities[index] as IWidget, this.m_lcOperator, this.IsForceFitLayout).Layout(new RectangleF(this.m_layoutArea.ClientActiveArea.X, this.m_layoutArea.ClientActiveArea.Y, this.m_layoutArea.ClientActiveArea.Width, this.m_layoutArea.ClientActiveArea.Height));
          if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
            return;
          if (ltWidget != null)
          {
            this.AddToFloatingItems(ltWidget, paragraph.ChildEntities[index] as ILeafWidget);
            ltWidget.InitLayoutInfoForTextWrapElements();
            paragraph.IsFloatingItemsLayouted = true;
          }
        }
      }
    }
    float xposition = (paragraph.m_layoutInfo as ParagraphLayoutInfo).XPosition;
    if (paragraph.IsFloatingItemsLayouted)
    {
      this.UpdateParagraphXPositionBasedOnTextWrap(paragraph, (paragraph.m_layoutInfo as ParagraphLayoutInfo).XPosition, (paragraph.m_layoutInfo as ParagraphLayoutInfo).YPosition);
      if ((double) xposition != (double) (paragraph.m_layoutInfo as ParagraphLayoutInfo).XPosition)
        paragraph.IsXpositionUpated = true;
      if (DocumentLayouter.IsUpdatingTOC)
        this.ResetFloatingEntityProperty(paragraph);
    }
    if (lcOperator.NotFittedFloatingItems.Count <= 0 || lcOperator.DynamicParagraph != paragraph)
      return;
    lcOperator.NotFittedFloatingItems.Clear();
    lcOperator.DynamicParagraph = (WParagraph) null;
  }

  protected void AddToFloatingItems(LayoutedWidget ltWidget, ILeafWidget leafWidget)
  {
    RectangleF rectangleF = ltWidget.Bounds;
    bool IsDoesNotDenotesRectangle = false;
    switch (leafWidget)
    {
      case WTextBox _ when !(leafWidget as WTextBox).TextBoxFormat.IsWrappingBoundsAdded:
        WTextBoxFormat textBoxFormat = (leafWidget as WTextBox).TextBoxFormat;
        FloatingItem floatingItem1 = new FloatingItem();
        if (textBoxFormat.TextWrappingStyle == TextWrappingStyle.Tight || textBoxFormat.TextWrappingStyle == TextWrappingStyle.Through)
          rectangleF = this.AdjustboundsBasedOnWrapPolygon(rectangleF, textBoxFormat.WrapPolygon.Vertices, textBoxFormat.Width, textBoxFormat.Height, ref IsDoesNotDenotesRectangle);
        floatingItem1.TextWrappingBounds = new RectangleF(rectangleF.X - textBoxFormat.WrapDistanceLeft, rectangleF.Y - textBoxFormat.WrapDistanceTop, rectangleF.Width + textBoxFormat.WrapDistanceRight + textBoxFormat.WrapDistanceLeft, rectangleF.Height + textBoxFormat.WrapDistanceBottom + textBoxFormat.WrapDistanceTop);
        floatingItem1.FloatingEntity = leafWidget as Entity;
        floatingItem1.IsDoesNotDenotesRectangle = IsDoesNotDenotesRectangle;
        (this.m_lcOperator as Layouter).FloatingItems.Add(floatingItem1);
        textBoxFormat.IsWrappingBoundsAdded = true;
        textBoxFormat.WrapCollectionIndex = (short) ((this.m_lcOperator as Layouter).FloatingItems.Count - 1);
        floatingItem1.WrapCollectionIndex = (this.m_lcOperator as Layouter).FloatingItems.Count - 1;
        break;
      case WPicture _ when !(leafWidget as WPicture).IsWrappingBoundsAdded:
        WPicture wpicture = leafWidget as WPicture;
        SizeF sizeF = this.DrawingContext.MeasureImage(wpicture);
        FloatingItem floatingItem2 = new FloatingItem();
        if (wpicture.TextWrappingStyle == TextWrappingStyle.Tight || wpicture.TextWrappingStyle == TextWrappingStyle.Through)
          rectangleF = this.AdjustboundsBasedOnWrapPolygon(rectangleF, wpicture.WrapPolygon.Vertices, sizeF.Width, sizeF.Height, ref IsDoesNotDenotesRectangle);
        float lineWidth = this.DrawingContext.GetLineWidth(wpicture);
        if ((double) lineWidth > 0.0 && (wpicture.TextWrappingStyle == TextWrappingStyle.Square || wpicture.TextWrappingStyle == TextWrappingStyle.TopAndBottom))
        {
          rectangleF.X -= lineWidth;
          rectangleF.Y -= lineWidth;
          rectangleF.Width += 2f * lineWidth;
          rectangleF.Height += 2f * lineWidth;
        }
        floatingItem2.TextWrappingBounds = new RectangleF(rectangleF.X - wpicture.DistanceFromLeft, rectangleF.Y - wpicture.DistanceFromTop, rectangleF.Width + wpicture.DistanceFromRight + wpicture.DistanceFromLeft, rectangleF.Height + wpicture.DistanceFromBottom + wpicture.DistanceFromTop);
        floatingItem2.FloatingEntity = leafWidget as Entity;
        floatingItem2.IsDoesNotDenotesRectangle = IsDoesNotDenotesRectangle;
        (this.m_lcOperator as Layouter).FloatingItems.Add(floatingItem2);
        (leafWidget as WPicture).IsWrappingBoundsAdded = true;
        (leafWidget as WPicture).WrapCollectionIndex = (short) ((this.m_lcOperator as Layouter).FloatingItems.Count - 1);
        floatingItem2.WrapCollectionIndex = (this.m_lcOperator as Layouter).FloatingItems.Count - 1;
        break;
      case Shape _ when !(leafWidget as Shape).WrapFormat.IsWrappingBoundsAdded:
        Shape shape = leafWidget as Shape;
        FloatingItem floatingItem3 = new FloatingItem();
        if (shape.WrapFormat.TextWrappingStyle == TextWrappingStyle.Tight || shape.WrapFormat.TextWrappingStyle == TextWrappingStyle.Through)
          rectangleF = this.AdjustboundsBasedOnWrapPolygon(rectangleF, shape.WrapFormat.WrapPolygon.Vertices, shape.Width, shape.Height, ref IsDoesNotDenotesRectangle);
        floatingItem3.TextWrappingBounds = new RectangleF(rectangleF.X - shape.WrapFormat.DistanceLeft, rectangleF.Y - shape.WrapFormat.DistanceTop, rectangleF.Width + shape.WrapFormat.DistanceRight + shape.WrapFormat.DistanceLeft, rectangleF.Height + shape.WrapFormat.DistanceBottom + shape.WrapFormat.DistanceTop);
        floatingItem3.FloatingEntity = leafWidget as Entity;
        floatingItem3.IsDoesNotDenotesRectangle = IsDoesNotDenotesRectangle;
        (this.m_lcOperator as Layouter).FloatingItems.Add(floatingItem3);
        (leafWidget as Shape).WrapFormat.IsWrappingBoundsAdded = true;
        (leafWidget as Shape).WrapFormat.WrapCollectionIndex = (this.m_lcOperator as Layouter).FloatingItems.Count - 1;
        floatingItem3.WrapCollectionIndex = (this.m_lcOperator as Layouter).FloatingItems.Count - 1;
        break;
      case GroupShape _ when !(leafWidget as GroupShape).WrapFormat.IsWrappingBoundsAdded:
        GroupShape groupShape = leafWidget as GroupShape;
        FloatingItem floatingItem4 = new FloatingItem();
        if ((double) groupShape.Rotation != 0.0)
          rectangleF = this.DrawingContext.GetBoundingBoxCoordinates(rectangleF, groupShape.Rotation);
        if (groupShape.WrapFormat.TextWrappingStyle == TextWrappingStyle.Tight || groupShape.WrapFormat.TextWrappingStyle == TextWrappingStyle.Through)
          rectangleF = this.AdjustboundsBasedOnWrapPolygon(rectangleF, groupShape.WrapFormat.WrapPolygon.Vertices, groupShape.Width, groupShape.Height, ref IsDoesNotDenotesRectangle);
        floatingItem4.TextWrappingBounds = new RectangleF(rectangleF.X - groupShape.WrapFormat.DistanceLeft, rectangleF.Y - groupShape.WrapFormat.DistanceTop, rectangleF.Width + groupShape.WrapFormat.DistanceRight + groupShape.WrapFormat.DistanceLeft, rectangleF.Height + groupShape.WrapFormat.DistanceBottom + groupShape.WrapFormat.DistanceTop);
        floatingItem4.FloatingEntity = leafWidget as Entity;
        floatingItem4.IsDoesNotDenotesRectangle = IsDoesNotDenotesRectangle;
        (this.m_lcOperator as Layouter).FloatingItems.Add(floatingItem4);
        (leafWidget as GroupShape).WrapFormat.IsWrappingBoundsAdded = true;
        (leafWidget as GroupShape).WrapFormat.WrapCollectionIndex = (this.m_lcOperator as Layouter).FloatingItems.Count - 1;
        floatingItem4.WrapCollectionIndex = (this.m_lcOperator as Layouter).FloatingItems.Count - 1;
        break;
      case WChart _ when !(leafWidget as WChart).WrapFormat.IsWrappingBoundsAdded:
        WChart wchart = leafWidget as WChart;
        FloatingItem floatingItem5 = new FloatingItem();
        if (wchart.WrapFormat.TextWrappingStyle == TextWrappingStyle.Tight || wchart.WrapFormat.TextWrappingStyle == TextWrappingStyle.Through)
          rectangleF = this.AdjustboundsBasedOnWrapPolygon(rectangleF, wchart.WrapFormat.WrapPolygon.Vertices, wchart.Width, wchart.Height, ref IsDoesNotDenotesRectangle);
        floatingItem5.TextWrappingBounds = new RectangleF(rectangleF.X - wchart.WrapFormat.DistanceLeft, rectangleF.Y - wchart.WrapFormat.DistanceTop, rectangleF.Width + wchart.WrapFormat.DistanceRight + wchart.WrapFormat.DistanceLeft, rectangleF.Height + wchart.WrapFormat.DistanceBottom + wchart.WrapFormat.DistanceTop);
        floatingItem5.FloatingEntity = leafWidget as Entity;
        floatingItem5.IsDoesNotDenotesRectangle = IsDoesNotDenotesRectangle;
        (this.m_lcOperator as Layouter).FloatingItems.Add(floatingItem5);
        (leafWidget as WChart).WrapFormat.IsWrappingBoundsAdded = true;
        (leafWidget as WChart).WrapFormat.WrapCollectionIndex = (this.m_lcOperator as Layouter).FloatingItems.Count - 1;
        floatingItem5.WrapCollectionIndex = (this.m_lcOperator as Layouter).FloatingItems.Count - 1;
        break;
    }
  }

  protected bool IsDrawingElement(ILeafWidget leafWidget)
  {
    switch (leafWidget)
    {
      case WPicture _:
      case Shape _:
      case WChart _:
      case GroupShape _:
        return true;
      default:
        return leafWidget is WTextBox;
    }
  }

  internal bool IsDoNotSuppressIndent(
    WParagraph paragraph,
    float yPosition,
    float wrappingBoundsBottom,
    int floatingItemIndex)
  {
    bool flag1 = false;
    if (paragraph.Document.Settings.CompatibilityOptions[CompatibilityOption.WW11IndentRules] && paragraph.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && (double) wrappingBoundsBottom > (double) yPosition)
    {
      WTextBody wtextBody1 = (WTextBody) null;
      bool flag2 = true;
      if (!(paragraph.Owner is WTextBody) || !((paragraph.Owner as WTextBody).Owner is WSection))
      {
        Entity floatingEntity = (this.m_lcOperator as Layouter).FloatingItems[floatingItemIndex].FloatingEntity;
        switch (floatingEntity)
        {
          case WTable _:
            wtextBody1 = (floatingEntity as WTable).OwnerTextBody;
            break;
          case WParagraph _:
            wtextBody1 = (floatingEntity as WParagraph).OwnerTextBody;
            break;
          case ParagraphItem _:
            wtextBody1 = (floatingEntity as ParagraphItem).OwnerParagraph.OwnerTextBody;
            break;
        }
        WTextBody wtextBody2 = paragraph.OwnerTextBody;
        if (wtextBody2 != null && wtextBody2.Owner is BlockContentControl)
          wtextBody2 = this.GetSDTOwnerTextBody(wtextBody2.Owner as BlockContentControl);
        if (wtextBody1 != null && wtextBody1.Owner is BlockContentControl)
          wtextBody1 = this.GetSDTOwnerTextBody(wtextBody1.Owner as BlockContentControl);
        flag2 = wtextBody1 == wtextBody2;
      }
      if (flag2)
        flag1 = true;
    }
    return flag1;
  }

  private WTextBody GetSDTOwnerTextBody(BlockContentControl sdtBlockContent)
  {
    return sdtBlockContent.Owner is BlockContentControl ? (sdtBlockContent.Owner as BlockContentControl).OwnerTextBody : (WTextBody) null;
  }

  internal void ResetFloatingEntityProperty(WParagraph paragraph)
  {
    if (paragraph.IsFloatingItemsLayouted)
    {
      for (int index = 0; index < paragraph.ChildEntities.Count; ++index)
      {
        if (paragraph.ChildEntities[index] is ParagraphItem && paragraph.ChildEntities[index].IsFloatingItem(true))
        {
          if (paragraph.ChildEntities[index] is WTextBox)
            (paragraph.ChildEntities[index] as WTextBox).TextBoxFormat.IsWrappingBoundsAdded = false;
          else if (paragraph.ChildEntities[index] is WPicture)
            (paragraph.ChildEntities[index] as WPicture).IsWrappingBoundsAdded = false;
          else if (paragraph.ChildEntities[index] is Shape)
            (paragraph.ChildEntities[index] as Shape).WrapFormat.IsWrappingBoundsAdded = false;
          else if (paragraph.ChildEntities[index] is WChart)
            (paragraph.ChildEntities[index] as WChart).WrapFormat.IsWrappingBoundsAdded = false;
          else if (paragraph.ChildEntities[index] is GroupShape)
            (paragraph.ChildEntities[index] as GroupShape).WrapFormat.IsWrappingBoundsAdded = false;
        }
      }
    }
    paragraph.IsFloatingItemsLayouted = false;
  }

  internal float GetParagraphTopMargin(WParagraph paragraph)
  {
    IEntity previousSibling;
    if (paragraph.Document.DOP.Dop2000.Copts.DontUseHTMLParagraphAutoSpacing || Math.Round((double) (paragraph.m_layoutInfo as ParagraphLayoutInfo).YPosition, 2) == Math.Round((double) (this.m_lcOperator as Layouter).PageTopMargin, 2) || (previousSibling = paragraph.PreviousSibling) == null || !(previousSibling is WParagraph) || ((double) paragraph.ParagraphFormat.BeforeSpacing <= (double) (previousSibling as WParagraph).ParagraphFormat.AfterSpacing || paragraph.ParagraphFormat.SpaceBeforeAuto) && (!paragraph.ParagraphFormat.SpaceBeforeAuto || (double) (previousSibling as WParagraph).ParagraphFormat.AfterSpacing >= 14.0) || paragraph.ParagraphFormat.ContextualSpacing)
      return 0.0f;
    return paragraph.ParagraphFormat.SpaceBeforeAuto ? 14f : paragraph.ParagraphFormat.BeforeSpacing;
  }

  internal WTextRange GetTextRange(IWidget widget)
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

  private RectangleF AdjustboundsBasedOnWrapPolygon(
    RectangleF rect,
    List<PointF> vertices,
    float imageWidth,
    float imageHeight,
    ref bool IsDoesNotDenotesRectangle)
  {
    if (vertices == null || vertices.Count < 2)
      return rect;
    float minX = 0.0f;
    float maxX = 0.0f;
    float minY = 0.0f;
    float maxY = 0.0f;
    RectangleF rectangleF1 = new RectangleF();
    RectangleF rectangleF2 = rect;
    if (this.IsWrapPolygonDenotesRectangle(vertices, ref minX, ref maxX, ref minY, ref maxY))
    {
      float num1 = 21600f / imageWidth;
      float num2 = vertices[0].X / num1;
      float num3 = vertices[0].Y / (21600f / imageHeight);
      rectangleF2.Width = (maxX - minX) / num1;
      rectangleF2.Height = (float) (((double) maxY - (double) minY) / (21600.0 / (double) imageHeight));
      rectangleF2.X = rect.X + num2;
      rectangleF2.Y = rect.Y + num3;
    }
    else
      IsDoesNotDenotesRectangle = true;
    return rectangleF2;
  }

  internal PointF LineIntersectionPoint(PointF ps1, PointF pe1, PointF ps2, PointF pe2)
  {
    float x1 = ps1.X;
    float y1 = ps1.Y;
    float x2 = pe1.X;
    float y2 = pe1.Y;
    float x3 = ps2.X;
    float y3 = ps2.Y;
    float x4 = pe2.X;
    float y4 = pe2.Y;
    float num1 = y2 - y1;
    float num2 = x1 - x2;
    float num3 = (float) ((double) x2 * (double) y1 - (double) x1 * (double) y2);
    float a1 = (float) ((double) num1 * (double) x3 + (double) num2 * (double) y3) + num3;
    float b1 = (float) ((double) num1 * (double) x4 + (double) num2 * (double) y4) + num3;
    if ((double) a1 != 0.0 && (double) b1 != 0.0 && this.sameSign(a1, b1))
      return new PointF(0.0f, 0.0f);
    float num4 = y4 - y3;
    float num5 = x3 - x4;
    float num6 = (float) ((double) x4 * (double) y3 - (double) x3 * (double) y4);
    float a2 = (float) ((double) num4 * (double) x1 + (double) num5 * (double) y1) + num6;
    float b2 = (float) ((double) num4 * (double) x2 + (double) num5 * (double) y2) + num6;
    if ((double) a2 != 0.0 && (double) b2 != 0.0 && this.sameSign(a2, b2))
      return new PointF(0.0f, 0.0f);
    float num7 = (float) ((double) num1 * (double) num5 - (double) num4 * (double) num2);
    if ((double) num7 == 0.0)
      return new PointF(0.0f, 0.0f);
    float num8 = (double) num7 >= 0.0 ? num7 / 2f : (float) (-(double) num7 / 2.0);
    float num9 = (float) ((double) num2 * (double) num6 - (double) num5 * (double) num3);
    float x5 = (double) num9 >= 0.0 ? (num9 + num8) / num7 : (num9 - num8) / num7;
    float num10 = (float) ((double) num4 * (double) num3 - (double) num1 * (double) num6);
    float y5 = (double) num10 >= 0.0 ? (num10 + num8) / num7 : (num10 - num8) / num7;
    return new PointF(x5, y5);
  }

  internal bool sameSign(float a, float b) => (double) a * (double) b >= 0.0;

  private bool IsWrapPolygonDenotesRectangle(
    List<PointF> vertices,
    ref float minX,
    ref float maxX,
    ref float minY,
    ref float maxY)
  {
    if (vertices.Count == 0)
      return false;
    minX = vertices[0].X;
    maxX = vertices[0].X;
    minY = vertices[0].Y;
    maxY = vertices[0].Y;
    for (int index = 0; index < vertices.Count - 1; ++index)
    {
      if (index % 2 == 0)
      {
        if ((double) vertices[index].X != (double) vertices[index + 1].X && (double) vertices[index].Y != (double) vertices[index + 1].Y)
          return false;
        if ((double) minX > (double) vertices[index].X)
          minX = vertices[index].X;
        if ((double) maxX < (double) vertices[index].X)
          maxX = vertices[index].X;
      }
      else
      {
        if ((double) vertices[index].Y != (double) vertices[index + 1].Y && (double) vertices[index].X != (double) vertices[index + 1].X)
          return false;
        if ((double) minY > (double) vertices[index].Y)
          minY = vertices[index].Y;
        if ((double) maxY < (double) vertices[index].Y)
          maxY = vertices[index].Y;
      }
    }
    return (double) vertices[0].X == (double) vertices[vertices.Count - 1].X && (double) minX != (double) maxX && (double) minY != (double) maxY;
  }

  internal void UpdateParagraphXPositionBasedOnTextWrap(
    WParagraph paragraph,
    float xPosition,
    float yPosition)
  {
    float num1 = 0.0f;
    bool flag = false;
    if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && (double) (this.m_lcOperator as Layouter).WrappingDifference == -3.4028234663852886E+38 && Math.Round((double) yPosition, 2) == Math.Round((double) (this.m_lcOperator as Layouter).PageTopMargin, 2))
    {
      num1 = yPosition;
      flag = true;
    }
    if ((this.m_lcOperator as Layouter).FloatingItems.Count <= 0 || (this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && !paragraph.IsInCell && paragraph.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 || this.IsInFootnote(paragraph) || (this.m_lcOperator as Layouter).IsLayoutingFootnote)
      return;
    RectangleF clientLayoutArea = (this.m_lcOperator as Layouter).ClientLayoutArea with
    {
      X = xPosition,
      Y = yPosition
    };
    float num2 = 0.0f;
    if (paragraph.IsInCell)
    {
      CellLayoutInfo layoutInfo = ((IWidget) (paragraph.GetOwnerEntity() as WTableCell)).LayoutInfo as CellLayoutInfo;
      num2 = layoutInfo.Paddings.Left + layoutInfo.Paddings.Right;
    }
    float num3 = 18f - num2;
    SizeF size = ((IWidget) paragraph).LayoutInfo.Size;
    for (int index = 0; index < (this.m_lcOperator as Layouter).FloatingItems.Count; ++index)
    {
      if (paragraph.IsInCell && (this.m_lcOperator as Layouter).FloatingItems[index].AllowOverlap && (paragraph.GetOwnerEntity() as WTableCell).OwnerRow.OwnerTable.TableFormat.Positioning.AllowOverlap && (!((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity is WTable) || !((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity as WTable).IsInCell))
      {
        WParagraph ownerParagraph = (this.m_lcOperator as Layouter).FloatingItems[index].OwnerParagraph;
        if (ownerParagraph == null || !ownerParagraph.IsInCell || paragraph.GetOwnerEntity() != ownerParagraph.GetOwnerEntity())
          continue;
      }
      float x = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds.X;
      RectangleF textWrappingBounds = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds;
      TextWrappingStyle textWrappingStyle = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingStyle;
      WTextBody ownerBody = (WTextBody) null;
      if ((this.IsInSameTextBody((TextBodyItem) paragraph, (this.m_lcOperator as Layouter).FloatingItems[index], ref ownerBody) || !paragraph.IsInCell || !(ownerBody is WTableCell)) && (!this.IsInFrame((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity as WParagraph) || !this.IsOwnerCellInFrame(paragraph)))
      {
        if (paragraph.ParagraphFormat.Bidi && this.IsInSameTextBody((TextBodyItem) paragraph, (this.m_lcOperator as Layouter).FloatingItems[index], ref ownerBody) && paragraph.IsInCell && ownerBody is WTableCell)
          this.ModifyXPositionForRTLLayouting(index, ref textWrappingBounds, this.m_layoutArea.ClientArea);
        else if (paragraph.ParagraphFormat.Bidi)
          this.ModifyXPositionForRTLLayouting(index, ref textWrappingBounds, (this.m_lcOperator as Layouter).ClientLayoutArea);
        float num4 = 18f;
        if (textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through)
          num4 = paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? 17.6f : 8f;
        float num5 = num4 - num2;
        float num6 = num5;
        if ((textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through) && (this.m_lcOperator as Layouter).FloatingItems[index].IsDoesNotDenotesRectangle)
        {
          RectangleF rectangleF = this.AdjustTightAndThroughBounds((this.m_lcOperator as Layouter).FloatingItems[index], clientLayoutArea, size.Height);
          if ((double) rectangleF.X != 0.0)
          {
            textWrappingBounds = rectangleF;
            num6 = size.Width;
          }
        }
        if (!paragraph.IsInCell && !paragraph.ParagraphFormat.IsFrame && !this.IsInTable((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity) && (double) clientLayoutArea.X <= (double) textWrappingBounds.Right + (double) num5 && (double) clientLayoutArea.Right >= (double) textWrappingBounds.X - (double) num5 && (this.m_lcOperator as Layouter).FloatingItems.Count > 0 && (double) clientLayoutArea.Y + (double) size.Height > (double) textWrappingBounds.Y && (double) clientLayoutArea.Y < (double) textWrappingBounds.Bottom && textWrappingStyle != TextWrappingStyle.Inline && textWrappingStyle != TextWrappingStyle.TopAndBottom && textWrappingStyle != TextWrappingStyle.InFrontOfText && textWrappingStyle != TextWrappingStyle.Behind)
        {
          float right = (((IWidget) paragraph).LayoutInfo as ParagraphLayoutInfo).Margins.Right;
          float num7 = (double) right < 0.0 ? Math.Abs(right) : 0.0f;
          if (paragraph.ParagraphFormat.GetAlignmentToRender() != HorizontalAlignment.Left && (double) clientLayoutArea.X < (double) textWrappingBounds.X && (double) clientLayoutArea.X + (double) size.Width > (double) textWrappingBounds.X)
            (paragraph.m_layoutInfo as ParagraphLayoutInfo).XPosition = clientLayoutArea.X;
          else if ((double) clientLayoutArea.X >= (double) textWrappingBounds.X && (double) clientLayoutArea.X < (double) textWrappingBounds.Right)
          {
            clientLayoutArea.Width = clientLayoutArea.Width - (textWrappingBounds.Right - clientLayoutArea.X) - num7;
            if ((double) clientLayoutArea.Width < (double) num6)
            {
              clientLayoutArea.Width = this.m_layoutArea.ClientActiveArea.Right - textWrappingBounds.Right - num7;
              if ((double) clientLayoutArea.Width < (double) num6)
              {
                (paragraph.m_layoutInfo as ParagraphLayoutInfo).XPosition = clientLayoutArea.X;
                clientLayoutArea.Width = (this.m_lcOperator as Layouter).ClientLayoutArea.Width;
                clientLayoutArea.Height = textWrappingBounds.Bottom - clientLayoutArea.X;
                clientLayoutArea.Y = textWrappingBounds.Bottom;
              }
              else
                (paragraph.m_layoutInfo as ParagraphLayoutInfo).XPosition = textWrappingBounds.Right;
            }
            else if (paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2007 || (double) clientLayoutArea.Y <= (double) textWrappingBounds.Bottom)
            {
              if (this.IsNeedToUpdateParagraphYPosition(clientLayoutArea.Y, textWrappingStyle, paragraph, clientLayoutArea.Y + size.Height + paragraph.ParagraphFormat.AfterSpacing, textWrappingBounds.Bottom))
              {
                (paragraph.m_layoutInfo as ParagraphLayoutInfo).XPosition = clientLayoutArea.X;
                clientLayoutArea.Width = (this.m_lcOperator as Layouter).ClientLayoutArea.Width;
                clientLayoutArea.Y = textWrappingBounds.Bottom;
                clientLayoutArea.Height = (this.m_lcOperator as Layouter).ClientLayoutArea.Height - (textWrappingBounds.Bottom - (this.m_lcOperator as Layouter).ClientLayoutArea.Y);
                this.m_layoutArea.UpdateBoundsBasedOnTextWrap(textWrappingBounds.Bottom);
              }
              else
              {
                ParagraphLayoutInfo layoutInfo = paragraph.m_layoutInfo as ParagraphLayoutInfo;
                if ((this.IsDoNotSuppressIndent(paragraph, clientLayoutArea.Y, textWrappingBounds.Bottom, index) ? 0.0 : (double) layoutInfo.Margins.Left) + (double) layoutInfo.FirstLineIndent + (double) clientLayoutArea.X < (double) textWrappingBounds.Right)
                {
                  (paragraph.m_layoutInfo as ParagraphLayoutInfo).XPosition = textWrappingBounds.Right;
                  clientLayoutArea.X = textWrappingBounds.Right;
                  if (paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
                    paragraph.IsXpositionUpated = true;
                }
              }
            }
          }
          else if ((double) textWrappingBounds.X - (double) num6 > (double) clientLayoutArea.X && (double) clientLayoutArea.Right > (double) textWrappingBounds.X || (double) clientLayoutArea.X > (double) textWrappingBounds.X && (double) clientLayoutArea.X > (double) textWrappingBounds.Right)
            (paragraph.m_layoutInfo as ParagraphLayoutInfo).XPosition = clientLayoutArea.X;
          else if ((double) clientLayoutArea.X > (double) textWrappingBounds.X - (double) num6 && (double) clientLayoutArea.X < (double) textWrappingBounds.Right)
          {
            if ((double) (clientLayoutArea.Width + (clientLayoutArea.X - textWrappingBounds.Right)) < (double) num6)
              clientLayoutArea.Y = textWrappingBounds.Bottom;
            else
              (paragraph.m_layoutInfo as ParagraphLayoutInfo).XPosition = textWrappingBounds.Right;
          }
          else if (this.IsNeedToUpdateParagraphYPosition(clientLayoutArea.Y, textWrappingStyle, paragraph, clientLayoutArea.Y + size.Height + paragraph.ParagraphFormat.AfterSpacing, textWrappingBounds.Bottom))
          {
            (paragraph.m_layoutInfo as ParagraphLayoutInfo).XPosition = clientLayoutArea.X;
            clientLayoutArea.Width = (this.m_lcOperator as Layouter).ClientLayoutArea.Width;
            clientLayoutArea.Y = textWrappingBounds.Bottom;
            clientLayoutArea.Height = (this.m_lcOperator as Layouter).ClientLayoutArea.Height - (textWrappingBounds.Bottom - (this.m_lcOperator as Layouter).ClientLayoutArea.Y);
            this.m_layoutArea.UpdateBoundsBasedOnTextWrap(textWrappingBounds.Bottom);
          }
        }
        this.ResetXPositionForRTLLayouting(index, ref textWrappingBounds, x);
      }
    }
    if (this.m_widget is WParagraph)
    {
      List<FloatingItem> floatingItems = new List<FloatingItem>((IEnumerable<FloatingItem>) (this.m_lcOperator as Layouter).FloatingItems);
      FloatingItem.SortFloatingItems(floatingItems, SortPosition.Y);
      this.UpdateXYPositionBasedOnAdjacentFloatingItems(floatingItems, clientLayoutArea, size, this.m_widget as WParagraph, false);
    }
    if (!flag || (double) num1 >= (double) yPosition)
      return;
    (this.m_lcOperator as Layouter).WrappingDifference = yPosition - (this.m_lcOperator as Layouter).PageTopMargin;
  }

  internal bool IsOwnerCellInFrame(WParagraph paragraph)
  {
    return paragraph.GetOwnerTableCell(paragraph.OwnerTextBody) != null && paragraph.GetOwnerTableCell(paragraph.OwnerTextBody).OwnerRow.OwnerTable.IsFrame;
  }

  internal void ModifyXPositionForRTLLayouting(
    int floatingItemIndex,
    ref RectangleF textWrappingBounds,
    RectangleF clientLayoutArea)
  {
    float num = clientLayoutArea.Right - textWrappingBounds.Right;
    textWrappingBounds.X = clientLayoutArea.X + num;
    (this.m_lcOperator as Layouter).FloatingItems[floatingItemIndex].TextWrappingBounds = textWrappingBounds;
  }

  internal void ResetXPositionForRTLLayouting(
    int floatingItemIndex,
    ref RectangleF textWrappingBounds,
    float floatingItemXPosition)
  {
    textWrappingBounds.X = floatingItemXPosition;
    textWrappingBounds.Y = (this.m_lcOperator as Layouter).FloatingItems[floatingItemIndex].TextWrappingBounds.Y;
    textWrappingBounds.Size = (this.m_lcOperator as Layouter).FloatingItems[floatingItemIndex].TextWrappingBounds.Size;
    (this.m_lcOperator as Layouter).FloatingItems[floatingItemIndex].TextWrappingBounds = textWrappingBounds;
  }

  private bool IsNeedToUpdateParagraphYPosition(
    float yPosition,
    TextWrappingStyle textWrappingStyle,
    WParagraph paragraph,
    float paraMarkEndPosition,
    float bottomPosition)
  {
    ILayoutSpacingsInfo layoutInfo = this.LayoutInfo as ILayoutSpacingsInfo;
    return (textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through) && paragraph.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && Math.Round((double) paraMarkEndPosition) > Math.Round((double) bottomPosition) && this.IsFloatingItemIntersectParaMark(yPosition - (layoutInfo != null ? layoutInfo.Margins.Top + layoutInfo.Paddings.Top : 0.0f), paraMarkEndPosition);
  }

  private bool IsFloatingItemIntersectParaMark(float startValue, float endValue)
  {
    for (int index = 0; index < (this.m_lcOperator as Layouter).FloatingItems.Count; ++index)
    {
      if ((double) startValue <= (double) (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds.Y && (double) endValue >= (double) (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds.Y)
        return true;
    }
    return false;
  }

  internal RectangleF AdjustTightAndThroughBounds(
    FloatingItem floatingItem,
    RectangleF clientArea,
    float size1)
  {
    if (floatingItem.TextWrappingStyle == TextWrappingStyle.Tight && (double) floatingItem.TextWrappingBounds.X < (double) clientArea.X)
      clientArea.X = floatingItem.TextWrappingBounds.X;
    RectangleF textWrappingBounds = floatingItem.TextWrappingBounds;
    PointF minimumInterSectPoint = new PointF(0.0f, 0.0f);
    PointF maximumIntersectPoint = new PointF(0.0f, 0.0f);
    float num = textWrappingBounds.X;
    if (floatingItem.FloatingEntity is WPicture)
    {
      WPicture floatingEntity = floatingItem.FloatingEntity as WPicture;
      textWrappingBounds.X += floatingEntity.DistanceFromLeft;
      textWrappingBounds.Y += floatingEntity.DistanceFromTop;
      textWrappingBounds.Width -= floatingEntity.DistanceFromRight + floatingEntity.DistanceFromLeft;
      textWrappingBounds.Height -= floatingEntity.DistanceFromBottom + floatingEntity.DistanceFromTop;
      SizeF sizeF = this.DrawingContext.MeasureImage(floatingEntity);
      this.FindMaxMinIntersectPoint(floatingEntity.WrapPolygon.Vertices, sizeF.Width, sizeF.Height, textWrappingBounds, size1, floatingItem, clientArea, ref minimumInterSectPoint, ref maximumIntersectPoint);
      if ((double) minimumInterSectPoint.X == 0.0 && (double) maximumIntersectPoint.X == 0.0)
        return floatingItem.TextWrappingBounds;
      if ((double) minimumInterSectPoint.X == 0.0)
      {
        textWrappingBounds.X -= floatingEntity.DistanceFromLeft;
        textWrappingBounds.Y -= floatingEntity.DistanceFromTop;
        textWrappingBounds.Width = maximumIntersectPoint.X - textWrappingBounds.X + floatingEntity.DistanceFromRight;
        textWrappingBounds.Height += floatingEntity.DistanceFromBottom + floatingEntity.DistanceFromTop;
      }
      else
      {
        num = minimumInterSectPoint.X - floatingEntity.DistanceFromLeft;
        textWrappingBounds.Y -= floatingEntity.DistanceFromTop;
        textWrappingBounds.Width = maximumIntersectPoint.X - num + floatingEntity.DistanceFromRight;
        textWrappingBounds.Height += floatingEntity.DistanceFromBottom + floatingEntity.DistanceFromTop;
      }
    }
    else if (floatingItem.FloatingEntity is Shape || floatingItem.FloatingEntity is WChart || floatingItem.FloatingEntity is GroupShape)
    {
      ShapeBase shapeBase = !(floatingItem.FloatingEntity is Shape) ? (!(floatingItem.FloatingEntity is GroupShape) ? (ShapeBase) (floatingItem.FloatingEntity as WChart) : (ShapeBase) (floatingItem.FloatingEntity as GroupShape)) : (ShapeBase) (floatingItem.FloatingEntity as Shape);
      textWrappingBounds.X += shapeBase.WrapFormat.DistanceLeft;
      textWrappingBounds.Y += shapeBase.WrapFormat.DistanceTop;
      textWrappingBounds.Width -= shapeBase.WrapFormat.DistanceRight + shapeBase.WrapFormat.DistanceLeft;
      textWrappingBounds.Height -= shapeBase.WrapFormat.DistanceBottom + shapeBase.WrapFormat.DistanceTop;
      this.FindMaxMinIntersectPoint(shapeBase.WrapFormat.WrapPolygon.Vertices, shapeBase.Width, shapeBase.Height, textWrappingBounds, size1, floatingItem, clientArea, ref minimumInterSectPoint, ref maximumIntersectPoint);
      if ((double) minimumInterSectPoint.X == 0.0 && (double) maximumIntersectPoint.X == 0.0)
        return floatingItem.TextWrappingBounds;
      if ((double) minimumInterSectPoint.X == 0.0)
      {
        textWrappingBounds.X -= shapeBase.WrapFormat.DistanceLeft;
        textWrappingBounds.Y -= shapeBase.WrapFormat.DistanceTop;
        textWrappingBounds.Width = maximumIntersectPoint.X - textWrappingBounds.X + shapeBase.WrapFormat.DistanceRight;
        textWrappingBounds.Height += shapeBase.WrapFormat.DistanceBottom + shapeBase.WrapFormat.DistanceTop;
      }
      else
      {
        num = minimumInterSectPoint.X - shapeBase.WrapFormat.DistanceLeft;
        textWrappingBounds.Y -= shapeBase.WrapFormat.DistanceTop;
        textWrappingBounds.Width = maximumIntersectPoint.X - num + shapeBase.WrapFormat.DistanceRight;
        textWrappingBounds.Height += shapeBase.WrapFormat.DistanceBottom + shapeBase.WrapFormat.DistanceTop;
      }
    }
    else if (floatingItem.FloatingEntity is WTextBox)
    {
      WTextBox floatingEntity = floatingItem.FloatingEntity as WTextBox;
      textWrappingBounds.X += floatingEntity.TextBoxFormat.WrapDistanceLeft;
      textWrappingBounds.Y += floatingEntity.TextBoxFormat.WrapDistanceTop;
      textWrappingBounds.Width -= floatingEntity.TextBoxFormat.WrapDistanceRight + floatingEntity.TextBoxFormat.WrapDistanceLeft;
      textWrappingBounds.Height -= floatingEntity.TextBoxFormat.WrapDistanceBottom + floatingEntity.TextBoxFormat.WrapDistanceTop;
      this.FindMaxMinIntersectPoint(floatingEntity.TextBoxFormat.WrapPolygon.Vertices, floatingEntity.TextBoxFormat.Width, floatingEntity.TextBoxFormat.Height, textWrappingBounds, size1, floatingItem, clientArea, ref minimumInterSectPoint, ref maximumIntersectPoint);
      if ((double) minimumInterSectPoint.X == 0.0 && (double) maximumIntersectPoint.X == 0.0)
        return floatingItem.TextWrappingBounds;
      if ((double) minimumInterSectPoint.X == 0.0)
      {
        textWrappingBounds.X -= floatingEntity.TextBoxFormat.WrapDistanceLeft;
        textWrappingBounds.Y -= floatingEntity.TextBoxFormat.WrapDistanceTop;
        textWrappingBounds.Width = maximumIntersectPoint.X - textWrappingBounds.X + floatingEntity.TextBoxFormat.WrapDistanceRight;
        textWrappingBounds.Height += floatingEntity.TextBoxFormat.WrapDistanceBottom + floatingEntity.TextBoxFormat.WrapDistanceTop;
      }
      else
      {
        num = minimumInterSectPoint.X - floatingEntity.TextBoxFormat.WrapDistanceLeft;
        textWrappingBounds.Y -= floatingEntity.TextBoxFormat.WrapDistanceTop;
        textWrappingBounds.Width = maximumIntersectPoint.X - num + floatingEntity.TextBoxFormat.WrapDistanceRight;
        textWrappingBounds.Height += floatingEntity.TextBoxFormat.WrapDistanceBottom + floatingEntity.TextBoxFormat.WrapDistanceTop;
      }
    }
    return new RectangleF((float) Math.Round((double) num, 1), (float) Math.Round((double) textWrappingBounds.Y, 1), (float) Math.Round((double) textWrappingBounds.Width, 1), (float) Math.Round((double) textWrappingBounds.Height, 1));
  }

  private void FindMaxMinIntersectPoint(
    List<PointF> vertices,
    float width,
    float height,
    RectangleF rect,
    float size1,
    FloatingItem floatingItem,
    RectangleF clientArea,
    ref PointF minimumInterSectPoint,
    ref PointF maximumIntersectPoint)
  {
    List<PointF> pointFList1 = new List<PointF>();
    for (int index = 0; index < vertices.Count; ++index)
    {
      float num1 = vertices[index].X / (21600f / width);
      float num2 = vertices[index].Y / (21600f / height);
      pointFList1.Add(new PointF(rect.X + num1, rect.Y + num2));
    }
    for (int index1 = 0; index1 < (int) Math.Ceiling((double) size1); ++index1)
    {
      List<PointF> pointFList2 = new List<PointF>();
      for (int index2 = 0; index2 < pointFList1.Count - 1; ++index2)
      {
        if (floatingItem.TextWrappingStyle == TextWrappingStyle.Tight && (double) clientArea.X < (double) rect.Right)
        {
          if ((double) clientArea.Y + (double) index1 > (double) pointFList1[index2].Y && (double) clientArea.Y + (double) index1 < (double) pointFList1[index2 + 1].Y || (double) clientArea.Y + (double) index1 < (double) pointFList1[index2].Y && (double) clientArea.Y + (double) index1 > (double) pointFList1[index2 + 1].Y)
          {
            PointF pointF = new PointF();
            pointF = (double) floatingItem.TextWrappingBounds.Right <= (double) clientArea.Right ? this.LineIntersectionPoint(new PointF(clientArea.X, clientArea.Y + (float) index1), new PointF(clientArea.Right, clientArea.Y + (float) index1), pointFList1[index2], pointFList1[index2 + 1]) : this.LineIntersectionPoint(new PointF(clientArea.X, clientArea.Y + (float) index1), new PointF(floatingItem.TextWrappingBounds.Right, clientArea.Y + (float) index1), pointFList1[index2], pointFList1[index2 + 1]);
            pointF.X = (float) Math.Round((double) pointF.X, 2);
            pointF.Y = (float) Math.Round((double) pointF.Y, 2);
            if ((double) pointF.X != 0.0 && (double) minimumInterSectPoint.X > (double) pointF.X || (double) minimumInterSectPoint.X == 0.0)
            {
              if (this.IsLineSlopeIsLeftToRight(pointFList1[index2], pointFList1[index2 + 1]))
              {
                minimumInterSectPoint = pointF;
                minimumInterSectPoint.X -= (float) Math.Round(1.0, 2);
              }
              else
                minimumInterSectPoint = pointF;
            }
            if ((double) maximumIntersectPoint.X < (double) pointF.X || (double) maximumIntersectPoint.X == 0.0)
            {
              if (this.IsLineSlopeIsLeftToRight(pointFList1[index2], pointFList1[index2 + 1]))
              {
                maximumIntersectPoint = pointF;
                ++maximumIntersectPoint.X;
              }
              else
                maximumIntersectPoint = pointF;
            }
          }
        }
        else if ((double) clientArea.Y + (double) index1 > (double) pointFList1[index2].Y && (double) clientArea.Y + (double) index1 < (double) pointFList1[index2 + 1].Y || (double) clientArea.Y + (double) index1 < (double) pointFList1[index2].Y && (double) clientArea.Y + (double) index1 > (double) pointFList1[index2 + 1].Y)
        {
          PointF pointF1 = new PointF();
          PointF pointF2 = (double) floatingItem.TextWrappingBounds.Right <= (double) clientArea.Right ? this.LineIntersectionPoint(new PointF(clientArea.X, clientArea.Y + (float) index1), new PointF(clientArea.Right, clientArea.Y + (float) index1), pointFList1[index2], pointFList1[index2 + 1]) : this.LineIntersectionPoint(new PointF(clientArea.X, clientArea.Y + (float) index1), new PointF(floatingItem.TextWrappingBounds.Right, clientArea.Y + (float) index1), pointFList1[index2], pointFList1[index2 + 1]);
          if ((double) pointF2.X != 0.0)
          {
            if (this.IsLineSlopeIsLeftToRight(pointFList1[index2], pointFList1[index2 + 1]))
              pointF2.Y = float.MinValue;
            pointFList2.Add(pointF2);
          }
        }
      }
      pointFList2.Sort((IComparer<PointF>) new SortPointByX());
      if (pointFList2.Count > 0)
      {
        if (pointFList2.Count > 1 && pointFList2.Count % 2 == 0 && (double) minimumInterSectPoint.X == 0.0)
        {
          minimumInterSectPoint = pointFList2[0];
          maximumIntersectPoint = pointFList2[1];
          if ((double) minimumInterSectPoint.Y == -3.4028234663852886E+38)
            minimumInterSectPoint.X -= (float) Math.Round(1.0, 2);
          if ((double) maximumIntersectPoint.Y == -3.4028234663852886E+38)
            ++maximumIntersectPoint.X;
        }
        else if (pointFList2.Count % 2 == 1 && (double) pointFList2[0].X != 0.0 && (double) maximumIntersectPoint.X < (double) pointFList2[0].X)
        {
          maximumIntersectPoint = pointFList2[0];
          if ((double) maximumIntersectPoint.Y == -3.4028234663852886E+38)
            ++maximumIntersectPoint.X;
        }
        else if (pointFList2.Count != 1)
        {
          PointF pointF3 = pointFList2[0];
          PointF pointF4 = pointFList2[1];
          if ((double) pointF3.X != 0.0 && (double) minimumInterSectPoint.X > (double) pointF3.X)
          {
            minimumInterSectPoint = pointF3;
            if ((double) minimumInterSectPoint.Y == -3.4028234663852886E+38)
              minimumInterSectPoint.X -= (float) Math.Round(1.0, 2);
          }
          if ((double) pointF3.X != 0.0 && (double) maximumIntersectPoint.X < (double) pointF4.X)
          {
            maximumIntersectPoint = pointF4;
            if ((double) maximumIntersectPoint.Y == -3.4028234663852886E+38)
              ++maximumIntersectPoint.X;
          }
        }
        pointFList2.Clear();
      }
    }
  }

  private bool IsLineSlopeIsLeftToRight(PointF firstPoint, PointF secondPoint)
  {
    return (double) firstPoint.Y < (double) secondPoint.Y && (double) firstPoint.X < (double) secondPoint.X || (double) firstPoint.Y > (double) secondPoint.Y && (double) firstPoint.X > (double) secondPoint.X;
  }

  private bool pnpoly(PointF[] poly, PointF pnt)
  {
    int length = poly.Length;
    bool flag = false;
    int index1 = 0;
    int index2 = length - 1;
    for (; index1 < length; index2 = index1++)
    {
      if ((double) poly[index1].Y > (double) pnt.Y != (double) poly[index2].Y > (double) pnt.Y && (double) pnt.X < ((double) poly[index2].X - (double) poly[index1].X) * ((double) pnt.Y - (double) poly[index1].Y) / ((double) poly[index2].Y - (double) poly[index1].Y) + (double) poly[index1].X)
        flag = !flag;
    }
    return flag;
  }

  private bool IsNeedToUpdateYPosition(Entity floatingEntity)
  {
    return !(this.GetBaseTextBody(floatingEntity) is HeaderFooter);
  }

  private void UpdateParagraphYPositionBasedonTextWrap()
  {
    if ((this.m_lcOperator as Layouter).FloatingItems.Count <= 0 || !(this.m_widget is WParagraph) || this.IsInFrame(this.m_widget as WParagraph) || this.IsInFootnote(this.m_widget as WParagraph) || this.GetFloattingItemIndex((Entity) (this.m_widget as WParagraph)) != -1 || (this.m_widget as WParagraph).IsFloatingItemsLayouted || (this.m_lcOperator as Layouter).IsLayoutingFootnote)
      return;
    SizeF size = this.m_widget.LayoutInfo.Size;
    bool flag1 = (this.m_widget as Entity).Document.Settings.CompatibilityMode == CompatibilityMode.Word2013;
    if ((this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && !(this.m_widget as WParagraph).IsInCell && !flag1)
      return;
    RectangleF clientActiveArea1 = this.m_layoutArea.ClientActiveArea;
    RectangleF clientActiveArea2 = this.m_layoutArea.ClientActiveArea;
    float y = clientActiveArea2.Y;
    ParagraphLayoutInfo layoutInfo1 = this.LayoutInfo as ParagraphLayoutInfo;
    float num1 = 0.0f;
    bool flag2 = false;
    if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && (double) (this.m_lcOperator as Layouter).WrappingDifference == -3.4028234663852886E+38 && Math.Round((double) clientActiveArea2.Y, 2) == Math.Round((double) (this.m_lcOperator as Layouter).PageTopMargin, 2))
    {
      num1 = clientActiveArea2.Y;
      flag2 = true;
    }
    bool flag3 = false;
    FloatingItem.SortXYPostionFloatingItems((this.m_lcOperator as Layouter).FloatingItems, clientActiveArea2, size);
    Entity previousItem = this.GetPreviousItem(this.m_widget as WParagraph);
    for (int index = 0; index < (this.m_lcOperator as Layouter).FloatingItems.Count; ++index)
    {
      if ((this.m_widget as WParagraph).IsInCell && (this.m_lcOperator as Layouter).FloatingItems[index].AllowOverlap && ((this.m_widget as WParagraph).GetOwnerEntity() as WTableCell).OwnerRow.OwnerTable.TableFormat.Positioning.AllowOverlap && (!((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity is WTable) || !((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity as WTable).IsInCell))
      {
        WParagraph ownerParagraph = (this.m_lcOperator as Layouter).FloatingItems[index].OwnerParagraph;
        if (ownerParagraph == null || !ownerParagraph.IsInCell || (this.m_widget as WParagraph).GetOwnerEntity() != ownerParagraph.GetOwnerEntity())
          continue;
      }
      float x = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds.X;
      RectangleF textWrappingBounds = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds;
      TextWrappingStyle textWrappingStyle = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingStyle;
      int num2;
      switch (textWrappingStyle)
      {
        case TextWrappingStyle.Tight:
        case TextWrappingStyle.Through:
          if ((double) (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds.Width > (double) (this.m_lcOperator as Layouter).CurrentSection.PageSetup.PageSize.Width)
          {
            num2 = (this.m_lcOperator as Layouter).FloatingItems[index].IsDoesNotDenotesRectangle ? 1 : 0;
            break;
          }
          goto default;
        default:
          num2 = 0;
          break;
      }
      bool flag4 = num2 != 0;
      WTextBody ownerBody = (WTextBody) null;
      WParagraph widget = this.m_widget as WParagraph;
      if ((this.IsInSameTextBody((TextBodyItem) widget, (this.m_lcOperator as Layouter).FloatingItems[index], ref ownerBody) || !widget.IsInCell || !(ownerBody is WTableCell)) && (!this.IsInFrame((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity as WParagraph) || !this.IsOwnerCellInFrame(widget)))
      {
        if ((this.m_widget as WParagraph).ParagraphFormat.Bidi && this.IsInSameTextBody((TextBodyItem) widget, (this.m_lcOperator as Layouter).FloatingItems[index], ref ownerBody) && widget.IsInCell && ownerBody is WTableCell)
          this.ModifyXPositionForRTLLayouting(index, ref textWrappingBounds, this.m_layoutArea.ClientArea);
        else if ((this.m_widget as WParagraph).ParagraphFormat.Bidi)
          this.ModifyXPositionForRTLLayouting(index, ref textWrappingBounds, (this.m_lcOperator as Layouter).ClientLayoutArea);
        float num3 = 0.0f;
        if ((this.m_widget as Entity).Owner is WTableCell)
        {
          CellLayoutInfo layoutInfo2 = ((this.m_widget as Entity).Owner as IWidget).LayoutInfo as CellLayoutInfo;
          num3 = layoutInfo2.Paddings.Left + layoutInfo2.Paddings.Right;
        }
        float num4 = 18f;
        if (textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through)
          num4 = flag1 ? 17.6f : 8f;
        float num5 = num4 - num3;
        bool allowOverlap = (this.m_lcOperator as Layouter).FloatingItems[index].AllowOverlap;
        if ((textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through) && !(this.GetBaseTextBody((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity) is HeaderFooter) && !(this.m_lcOperator as Layouter).FloatingItems[index].IsDoesNotDenotesRectangle)
        {
          float floattingItemBottom = this.GetFloattingItemBottom((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity, textWrappingBounds.Bottom);
          float multipleFactorValue;
          if ((this.m_widget as WParagraph).ParagraphFormat.LineSpacingRule == LineSpacingRule.Exactly)
          {
            multipleFactorValue = (this.m_widget as WParagraph).ParagraphFormat.LineSpacing;
          }
          else
          {
            multipleFactorValue = this.GetMultipleFactorValue(this.m_widget as WParagraph);
            if ((this.m_widget as WParagraph).ParagraphFormat.LineSpacingRule == LineSpacingRule.AtLeast && (double) multipleFactorValue < (double) (this.m_widget as WParagraph).ParagraphFormat.LineSpacing)
              multipleFactorValue = (this.m_widget as WParagraph).ParagraphFormat.LineSpacing;
          }
          float forTightAndThrough = this.GetExceededBottomValueForTightAndThrough(floattingItemBottom, clientActiveArea2.Y, multipleFactorValue, this.m_widget as WParagraph, false);
          if ((double) forTightAndThrough > 0.0)
          {
            float num6 = floattingItemBottom + forTightAndThrough;
            if ((double) num6 < (double) textWrappingBounds.Height)
              num6 += multipleFactorValue;
            textWrappingBounds.Height = num6 - textWrappingBounds.Y;
          }
        }
        if ((textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through) && (this.m_lcOperator as Layouter).FloatingItems[index].IsDoesNotDenotesRectangle)
          textWrappingBounds = this.AdjustTightAndThroughBounds((this.m_lcOperator as Layouter).FloatingItems[index], clientActiveArea2, size.Height);
        float adjustingValue = 0.0f;
        bool considerAdjustValues = this.IsNeedToConsiderAdjustValues(ref adjustingValue, widget, textWrappingStyle, index);
        if ((!(this.IsInTextBox(this.m_widget as WParagraph) is WTextBox) || !allowOverlap) && (considerAdjustValues ? ((double) clientActiveArea1.X > (double) textWrappingBounds.Right + (double) adjustingValue ? 1 : 0) : ((double) clientActiveArea1.X > (double) textWrappingBounds.Right + (double) num5 ? 1 : 0)) == 0 && (double) clientActiveArea1.Right >= (double) textWrappingBounds.X - (double) num5)
        {
          if (((double) clientActiveArea2.Y + (double) size.Height > (double) textWrappingBounds.Y || flag3) && (double) clientActiveArea2.Y < (double) textWrappingBounds.Bottom && textWrappingStyle != TextWrappingStyle.Inline && textWrappingStyle != TextWrappingStyle.TopAndBottom && textWrappingStyle != TextWrappingStyle.InFrontOfText && textWrappingStyle != TextWrappingStyle.Behind)
          {
            float right = layoutInfo1.Margins.Right;
            double left1 = (double) layoutInfo1.Margins.Left;
            if (layoutInfo1.IsFirstLine)
            {
              double left2 = (double) layoutInfo1.Margins.Left;
              double firstLineIndent = (double) layoutInfo1.FirstLineIndent;
            }
            float num7 = (double) right < 0.0 ? right : 0.0f;
            if ((this.m_widget as WParagraph).ParagraphFormat.GetAlignmentToRender() != HorizontalAlignment.Left && (double) clientActiveArea2.X < (double) textWrappingBounds.X && (double) clientActiveArea2.X + (double) size.Width > (double) textWrappingBounds.X)
            {
              if ((double) clientActiveArea2.Right > (double) textWrappingBounds.X)
                clientActiveArea2.Width -= clientActiveArea2.Right - textWrappingBounds.Right;
              if ((double) clientActiveArea2.Width < (double) num5)
              {
                if (flag1 || !((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity is WTable) || previousItem != (this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity)
                {
                  if (flag4)
                    this.m_layoutArea.UpdateBoundsBasedOnTextWrap(layoutInfo1.YPosition + size.Height);
                  else
                    this.m_layoutArea.UpdateBoundsBasedOnTextWrap(textWrappingBounds.Bottom);
                  clientActiveArea2 = this.m_layoutArea.ClientActiveArea;
                }
                if ((!layoutInfo1.IsFirstLine || !flag1 ? (!this.IsNeedToUpdateYPosition((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity) ? 0 : (!flag4 ? 1 : 0)) : 1) != 0)
                  layoutInfo1.YPosition = this.m_layoutArea.ClientActiveArea.Y;
              }
              else
                clientActiveArea2.X = textWrappingBounds.Right;
            }
            else if ((double) clientActiveArea2.X >= (double) textWrappingBounds.X && (double) clientActiveArea2.X < (double) textWrappingBounds.Right)
            {
              clientActiveArea2.Width = clientActiveArea2.Width - (textWrappingBounds.Right - clientActiveArea2.X) - num7;
              if ((double) clientActiveArea2.Width < (double) num5 || flag3)
              {
                clientActiveArea2.Width = this.m_layoutArea.ClientActiveArea.Right - textWrappingBounds.Right - num7;
                bool flag5 = false;
                if ((double) clientActiveArea2.Width < (double) num5 || flag3)
                {
                  if ((double) this.m_layoutArea.ClientActiveArea.X + (double) num5 < (double) (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds.X)
                  {
                    RectangleF intersectingItemBounds = FloatingItem.GetIntersectingItemBounds(this.m_lcOperator as Layouter, (this.m_lcOperator as Layouter).FloatingItems[index], y);
                    if (intersectingItemBounds != RectangleF.Empty && (double) intersectingItemBounds.Bottom <= (double) textWrappingBounds.Bottom)
                    {
                      this.m_layoutArea.UpdateBoundsBasedOnTextWrap(intersectingItemBounds.Bottom);
                      clientActiveArea2 = this.m_layoutArea.ClientActiveArea;
                      flag5 = true;
                    }
                  }
                  if ((flag1 || !((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity is WTable) || previousItem != (this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity) && !flag5)
                  {
                    if (flag4)
                      this.m_layoutArea.UpdateBoundsBasedOnTextWrap(layoutInfo1.YPosition + size.Height);
                    else
                      this.m_layoutArea.UpdateBoundsBasedOnTextWrap(textWrappingBounds.Bottom);
                    clientActiveArea2 = this.m_layoutArea.ClientActiveArea;
                  }
                  if ((!layoutInfo1.IsFirstLine || !flag1 ? (this.IsNeedToUpdateYPosition((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity) ? 1 : 0) : 1) != 0 && !flag4)
                    layoutInfo1.YPosition = this.m_layoutArea.ClientActiveArea.Y;
                }
                else
                  clientActiveArea2.X = textWrappingBounds.Right;
              }
              else
                clientActiveArea2.X = textWrappingBounds.Right;
            }
            else if ((double) textWrappingBounds.X > (double) clientActiveArea2.X && (double) clientActiveArea2.Right > (double) textWrappingBounds.X)
            {
              clientActiveArea2.Width = textWrappingBounds.X - clientActiveArea2.X - num7;
              if ((double) clientActiveArea2.Width < (double) num5)
              {
                clientActiveArea2.Width = this.m_layoutArea.ClientActiveArea.Right - textWrappingBounds.Right - num7;
                if ((double) clientActiveArea2.Width < (double) num5)
                {
                  if (flag1 || !((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity is WTable) || previousItem != (this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity)
                  {
                    if (flag4)
                      this.m_layoutArea.UpdateBoundsBasedOnTextWrap(layoutInfo1.YPosition + size.Height);
                    else
                      this.m_layoutArea.UpdateBoundsBasedOnTextWrap(textWrappingBounds.Bottom);
                    clientActiveArea2 = this.m_layoutArea.ClientActiveArea;
                  }
                  if ((!layoutInfo1.IsFirstLine || !flag1 ? (!this.IsNeedToUpdateYPosition((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity) ? 0 : (!flag4 ? 1 : 0)) : 1) != 0)
                    layoutInfo1.YPosition = this.m_layoutArea.ClientActiveArea.Y;
                }
                else
                  clientActiveArea2.X = textWrappingBounds.Right;
              }
            }
          }
          else if ((this.m_lcOperator as Layouter).FloatingItems.Count > 0 && ((double) clientActiveArea2.Y >= (double) textWrappingBounds.Y && (double) clientActiveArea2.Y < (double) textWrappingBounds.Bottom || (double) clientActiveArea2.Y + (double) size.Height >= (double) textWrappingBounds.Y && (double) clientActiveArea2.Y + (double) size.Height < (double) textWrappingBounds.Bottom) && textWrappingStyle == TextWrappingStyle.TopAndBottom && this.IsFrameInClientArea((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity as WParagraph, textWrappingBounds))
          {
            float squareAndTopandBottom = this.GetBottomValueForSquareAndTopandBottom(this.m_widget as WParagraph);
            if (flag1 || !((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity is WTable) || previousItem != (this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity)
            {
              if (flag4)
                this.m_layoutArea.UpdateBoundsBasedOnTextWrap(layoutInfo1.YPosition + size.Height);
              else if (layoutInfo1.IsFirstLine && (double) layoutInfo1.Margins.Top == 0.0)
                this.m_layoutArea.UpdateBoundsBasedOnTextWrap(textWrappingBounds.Bottom + squareAndTopandBottom);
              else
                this.m_layoutArea.UpdateBoundsBasedOnTextWrap(textWrappingBounds.Bottom);
              if (!flag1 && layoutInfo1.IsFirstLine && !flag4)
                flag3 = true;
            }
            if ((!layoutInfo1.IsFirstLine || !flag1 ? (!this.IsNeedToUpdateYPosition((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity) ? 0 : (!flag4 ? 1 : 0)) : 1) != 0)
              layoutInfo1.YPosition = this.m_layoutArea.ClientActiveArea.Y;
          }
        }
        this.ResetXPositionForRTLLayouting(index, ref textWrappingBounds, x);
      }
    }
    if (!flag2 || (double) num1 >= (double) clientActiveArea2.Y)
      return;
    (this.m_lcOperator as Layouter).WrappingDifference = clientActiveArea2.Y - (this.m_lcOperator as Layouter).PageTopMargin;
  }

  private Entity GetPreviousItem(WParagraph wParagraph)
  {
    Entity previousItem = wParagraph.PreviousSibling as Entity;
    if (previousItem is WParagraph)
    {
      wParagraph = previousItem as WParagraph;
      if (wParagraph.m_layoutInfo.IsSkip)
      {
        previousItem = wParagraph.PreviousSibling as Entity;
        if (previousItem is WParagraph)
          previousItem = this.GetPreviousItem(previousItem as WParagraph);
      }
    }
    return previousItem;
  }

  internal bool IsInSameTextBody(
    TextBodyItem bodyItem,
    FloatingItem fItem,
    ref WTextBody ownerBody)
  {
    if (fItem.FloatingEntity is WParagraph || fItem.FloatingEntity is WTable)
      ownerBody = fItem.FloatingEntity.Owner as WTextBody;
    else if (fItem.OwnerParagraph != null)
      ownerBody = fItem.OwnerParagraph.OwnerTextBody;
    return ownerBody != null && bodyItem.OwnerTextBody == ownerBody;
  }

  internal void UpdateXYPositionBasedOnAdjacentFloatingItems(
    List<FloatingItem> floatingItems,
    RectangleF rect,
    SizeF size,
    WParagraph paragraph,
    bool isFromLeafLayoutContext)
  {
    List<FloatingItem> floatingItemList = new List<FloatingItem>();
    for (int index = 0; index < floatingItems.Count; ++index)
    {
      if (this.IsSquareOrTightAndThrow(floatingItems[index]) && this.IsYPositionIntersect(floatingItems[index].TextWrappingBounds, rect, size.Height))
        floatingItemList.Add(floatingItems[index]);
    }
    if (floatingItemList.Count <= 1)
      return;
    FloatingItem.SortFloatingItems(floatingItemList, SortPosition.X);
    bool flag = false;
    string wordVersion = this.GetWordVersion(paragraph);
    for (int index = 0; index + 1 < floatingItemList.Count && (double) floatingItemList[index].TextWrappingBounds.Right <= (double) rect.X; ++index)
    {
      if ((double) floatingItemList[index + 1].TextWrappingBounds.X >= (double) rect.X)
      {
        float betweenFloatingItems = this.GetMinWidthBetweenFloatingItems(floatingItemList[index].TextWrappingStyle, floatingItemList[index + 1].TextWrappingStyle, wordVersion);
        if ((double) floatingItemList[index + 1].TextWrappingBounds.X - (double) floatingItemList[index].TextWrappingBounds.Right <= (double) betweenFloatingItems)
        {
          if ((double) this.m_layoutArea.ClientActiveArea.Right - (double) floatingItemList[index + 1].TextWrappingBounds.Right < (double) this.GetMinWidth(floatingItemList[index].TextWrappingStyle, paragraph))
          {
            flag = true;
            break;
          }
          rect.Width -= floatingItemList[index + 1].TextWrappingBounds.Right - rect.X;
          rect.X = floatingItemList[index + 1].TextWrappingBounds.Right;
          if (isFromLeafLayoutContext)
            this.CreateLayoutArea(rect);
          else
            (paragraph.m_layoutInfo as ParagraphLayoutInfo).XPosition = rect.X;
        }
      }
    }
    if (!flag)
      return;
    this.UpdateYPosition(floatingItemList, rect, size, paragraph, isFromLeafLayoutContext);
  }

  private void UpdateYPosition(
    List<FloatingItem> interSectingItems,
    RectangleF rect,
    SizeF size,
    WParagraph paragraph,
    bool isFromLeafLayoutContext)
  {
    int index = 0;
    float num1 = (this.m_lcOperator as Layouter).ClientLayoutArea.X;
    float num2 = interSectingItems[index].TextWrappingBounds.Bottom;
    float height = interSectingItems[index].TextWrappingBounds.Height;
    for (; index + 1 < interSectingItems.Count; ++index)
    {
      FloatingItem interSectingItem = interSectingItems[index + 1];
      if ((double) num2 >= (double) interSectingItem.TextWrappingBounds.Bottom)
      {
        num2 = this.GetFloattingItemBottom(interSectingItem.FloatingEntity, interSectingItem.TextWrappingBounds.Bottom);
        if ((double) num2 != -3.4028234663852886E+38 && (interSectingItem.TextWrappingStyle == TextWrappingStyle.Tight || interSectingItem.TextWrappingStyle == TextWrappingStyle.Through) && !(this.GetBaseEntity(interSectingItem.FloatingEntity) is HeaderFooter) && !interSectingItem.IsDoesNotDenotesRectangle)
        {
          RectangleF forTightAndThrough = this.GetBottomPositionForTightAndThrough(num2, interSectingItem.TextWrappingBounds, paragraph, rect.Y, size.Height);
          num1 = forTightAndThrough.Right;
          num2 = forTightAndThrough.Y;
          height = forTightAndThrough.Height;
        }
        else
        {
          num1 = interSectingItem.TextWrappingBounds.Right;
          height = interSectingItem.TextWrappingBounds.Height;
        }
      }
    }
    if (isFromLeafLayoutContext)
    {
      rect.Width -= num1 - rect.X;
      rect.Height -= height;
      rect.X = num1;
      rect.Y = num2;
      this.CreateLayoutArea(rect);
    }
    else
    {
      this.m_layoutArea.UpdateBoundsBasedOnTextWrap(num2);
      (paragraph.m_layoutInfo as ParagraphLayoutInfo).XPosition = num1;
    }
  }

  private float GetMinWidth(TextWrappingStyle textWrappingStyle, WParagraph ownerParagraph)
  {
    float num1 = 0.0f;
    if (ownerParagraph.Owner is WTableCell)
    {
      CellLayoutInfo layoutInfo = (ownerParagraph.Owner as IWidget).LayoutInfo as CellLayoutInfo;
      num1 = layoutInfo.Paddings.Left + layoutInfo.Paddings.Right;
    }
    float num2 = 18f;
    if (textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through)
      num2 = ownerParagraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? 17.6f : 8f;
    return num2 - num1;
  }

  private string GetWordVersion(WParagraph paragraph)
  {
    if (paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2003)
      return "Word2003";
    if (paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2007)
      return "Word2007";
    return paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2010 ? "Word2010" : "Word2013";
  }

  private float GetMinWidthBetweenFloatingItems(
    TextWrappingStyle leftStyle,
    TextWrappingStyle rightStyle,
    string wordVersion)
  {
    float betweenFloatingItems = 0.0f;
    if (leftStyle == TextWrappingStyle.Square && rightStyle == TextWrappingStyle.Square)
    {
      switch (wordVersion)
      {
        case "Word2003":
          betweenFloatingItems = 19f;
          break;
        default:
          betweenFloatingItems = 18f;
          break;
      }
    }
    else if (leftStyle == TextWrappingStyle.Square && (rightStyle == TextWrappingStyle.Tight || rightStyle == TextWrappingStyle.Through))
    {
      switch (wordVersion)
      {
        case "Word2003":
          betweenFloatingItems = 10f;
          break;
        case "Word2007":
        case "Word2010":
          betweenFloatingItems = 9f;
          break;
        case "Word2013":
          betweenFloatingItems = 18f;
          break;
      }
    }
    else if ((leftStyle == TextWrappingStyle.Tight || leftStyle == TextWrappingStyle.Through) && (rightStyle == TextWrappingStyle.Tight || rightStyle == TextWrappingStyle.Through || rightStyle == TextWrappingStyle.Square))
    {
      switch (wordVersion)
      {
        case "Word2003":
          betweenFloatingItems = rightStyle != TextWrappingStyle.Square ? 10f : 9f;
          break;
        case "Word2007":
        case "Word2010":
          betweenFloatingItems = 7f;
          break;
        case "Word2013":
          betweenFloatingItems = 16f;
          break;
      }
    }
    return betweenFloatingItems;
  }

  private bool IsSquareOrTightAndThrow(FloatingItem floatingItem)
  {
    switch (floatingItem.TextWrappingStyle)
    {
      case TextWrappingStyle.Square:
      case TextWrappingStyle.Tight:
      case TextWrappingStyle.Through:
        return true;
      default:
        return false;
    }
  }

  internal bool IsYPositionIntersect(
    RectangleF floatingItemBounds,
    RectangleF currentItemBounds,
    float height)
  {
    return Math.Round((double) currentItemBounds.Y, 2) > Math.Round((double) floatingItemBounds.Y, 2) && Math.Round((double) currentItemBounds.Y, 2) < Math.Round((double) floatingItemBounds.Bottom, 2) || Math.Round((double) currentItemBounds.Y + (double) height, 2) > Math.Round((double) floatingItemBounds.Y, 2) && Math.Round((double) currentItemBounds.Y + (double) height, 2) < Math.Round((double) floatingItemBounds.Bottom, 2) || Math.Round((double) currentItemBounds.Y, 2) < Math.Round((double) floatingItemBounds.Bottom, 2) && Math.Round((double) currentItemBounds.Y, 2) > Math.Round((double) floatingItemBounds.Y, 2);
  }

  internal RectangleF GetBottomPositionForTightAndThrough(
    float floattingItemBottomPosition,
    RectangleF textWrappingBounds,
    WParagraph paragraph,
    float yPostion,
    float leafWidgetHeight)
  {
    bool isSplittedLine = false;
    float multipleFactorValue;
    if (paragraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.Exactly)
    {
      multipleFactorValue = paragraph.ParagraphFormat.LineSpacing;
    }
    else
    {
      if (paragraph.ChildEntities.Count == 0)
      {
        multipleFactorValue = ((IWidget) paragraph).LayoutInfo.Size.Height;
      }
      else
      {
        isSplittedLine = true;
        multipleFactorValue = leafWidgetHeight;
      }
      if (paragraph.ParagraphFormat.LineSpacingRule == LineSpacingRule.AtLeast && (double) multipleFactorValue < (double) paragraph.ParagraphFormat.LineSpacing)
        multipleFactorValue = paragraph.ParagraphFormat.LineSpacing;
    }
    float forTightAndThrough = this.GetExceededBottomValueForTightAndThrough(floattingItemBottomPosition, yPostion, multipleFactorValue, paragraph, isSplittedLine);
    if ((double) forTightAndThrough > 0.0)
    {
      floattingItemBottomPosition += forTightAndThrough;
      if ((double) floattingItemBottomPosition < (double) textWrappingBounds.Height)
        floattingItemBottomPosition += multipleFactorValue;
      textWrappingBounds.Height = floattingItemBottomPosition - textWrappingBounds.Y;
    }
    return textWrappingBounds;
  }

  private float GetMultipleFactorValue(WParagraph paragraph)
  {
    float multipleFactorValue = 0.0f;
    if (paragraph.ChildEntities.Count > 0)
    {
      bool flag = false;
      int index;
      for (index = 0; index < paragraph.ChildEntities.Count; ++index)
      {
        if (paragraph.ChildEntities[index].EntityType != EntityType.BookmarkStart && paragraph.ChildEntities[index].EntityType != EntityType.BookmarkEnd)
        {
          flag = true;
          break;
        }
      }
      if (flag && paragraph.ChildEntities[index] is ILeafWidget)
        multipleFactorValue = (paragraph.ChildEntities[index] as ILeafWidget).Measure(this.DrawingContext).Height;
      else if (paragraph.m_layoutInfo is ParagraphLayoutInfo)
        multipleFactorValue = (paragraph.m_layoutInfo as ParagraphLayoutInfo).Size.Height;
    }
    else if (paragraph.m_layoutInfo is ParagraphLayoutInfo)
      multipleFactorValue = (paragraph.m_layoutInfo as ParagraphLayoutInfo).Size.Height;
    return multipleFactorValue;
  }

  internal float GetFloattingItemBottom(Entity entity, float bottom)
  {
    switch (entity)
    {
      case WPicture _:
        WPicture wpicture = entity as WPicture;
        bottom -= wpicture.DistanceFromBottom;
        break;
      case WTextBox _:
        WTextBox wtextBox = entity as WTextBox;
        bottom -= wtextBox.TextBoxFormat.WrapDistanceBottom;
        break;
      case Shape _:
      case WChart _:
      case GroupShape _:
        ShapeBase shapeBase = !(entity is Shape) ? (!(entity is GroupShape) ? (ShapeBase) (entity as WChart) : (ShapeBase) (entity as GroupShape)) : (ShapeBase) (entity as Shape);
        bottom -= shapeBase.WrapFormat.DistanceBottom;
        break;
    }
    return bottom;
  }

  private float GetExceededBottomValueForTightAndThrough(
    float floattingItemBottomPosition,
    float yPosition,
    float multipleFactorValue,
    WParagraph paragraph,
    bool isSplittedLine)
  {
    float num1 = 0.0f;
    bool flag = false;
    if (!isSplittedLine && (!(paragraph.m_layoutInfo is ParagraphLayoutInfo) || !(paragraph.m_layoutInfo as ParagraphLayoutInfo).IsFirstItemInPage))
    {
      WParagraph previousParagraph = this.GetPreviousParagraph(paragraph);
      if (previousParagraph != null)
      {
        if ((double) previousParagraph.ParagraphFormat.AfterSpacing >= (double) paragraph.ParagraphFormat.BeforeSpacing)
        {
          num1 = previousParagraph.ParagraphFormat.AfterSpacing - 2f;
        }
        else
        {
          flag = true;
          num1 = paragraph.ParagraphFormat.BeforeSpacing - 2f;
        }
        yPosition -= previousParagraph.ParagraphFormat.AfterSpacing - 2f;
      }
    }
    else if (paragraph.m_layoutInfo is ParagraphLayoutInfo && (paragraph.m_layoutInfo as ParagraphLayoutInfo).IsFirstItemInPage)
      num1 = paragraph.ParagraphFormat.BeforeSpacing - 2f;
    float num2 = (floattingItemBottomPosition - yPosition) % multipleFactorValue;
    float forTightAndThrough = ((double) num2 >= 2.0 ? multipleFactorValue - num2 : 0.0f) + num1;
    if (!flag)
      forTightAndThrough %= multipleFactorValue;
    return forTightAndThrough;
  }

  private float GetBottomValueForSquareAndTopandBottom(WParagraph paragraph)
  {
    float squareAndTopandBottom = 0.0f;
    WParagraph previousParagraph = this.GetPreviousParagraph(paragraph);
    if (previousParagraph != null)
      squareAndTopandBottom = (double) previousParagraph.ParagraphFormat.AfterSpacing > (double) paragraph.ParagraphFormat.BeforeSpacing ? 0.0f : paragraph.ParagraphFormat.BeforeSpacing - previousParagraph.ParagraphFormat.AfterSpacing;
    return squareAndTopandBottom;
  }

  internal WParagraph GetPreviousParagraph(WParagraph paragrph)
  {
    IEntity previousSibling = paragrph.PreviousSibling;
    switch (previousSibling)
    {
      case WParagraph _:
        return previousSibling as WParagraph;
      case BlockContentControl _:
        return this.GetPreviousParagraphIsInSDTContent(previousSibling as BlockContentControl);
      case WTable _:
        return this.GetPreviousParagraphIsInTable(previousSibling as WTable);
      default:
        return (WParagraph) null;
    }
  }

  private WParagraph GetPreviousParagraphIsInSDTContent(BlockContentControl sdtContent)
  {
    BodyItemCollection items = sdtContent.TextBody.Items;
    IEntity paragraphIsInSdtContent = (IEntity) items[items.Count - 1];
    switch (paragraphIsInSdtContent)
    {
      case WParagraph _:
        return paragraphIsInSdtContent as WParagraph;
      case BlockContentControl _:
        return this.GetPreviousParagraphIsInSDTContent(paragraphIsInSdtContent as BlockContentControl);
      case WTable _:
        return this.GetPreviousParagraphIsInTable(paragraphIsInSdtContent as WTable);
      default:
        return (WParagraph) null;
    }
  }

  private WParagraph GetPreviousParagraphIsInTable(WTable table)
  {
    IEntity widgetInner = (IEntity) table.LastCell.WidgetInnerCollection[table.LastCell.WidgetInnerCollection.Count - 1];
    switch (widgetInner)
    {
      case WParagraph _:
        return widgetInner as WParagraph;
      case BlockContentControl _:
        return this.GetPreviousParagraphIsInSDTContent(widgetInner as BlockContentControl);
      case WTable _:
        return this.GetPreviousParagraphIsInTable(widgetInner as WTable);
      default:
        return (WParagraph) null;
    }
  }

  internal int GetFloattingItemIndex(Entity entity)
  {
    for (; entity != null && entity.EntityType != EntityType.TextBox && entity != null && entity.EntityType != EntityType.TextBox; entity = entity.Owner)
    {
      switch (entity)
      {
        case Shape _:
        case GroupShape _:
          goto label_4;
        default:
          continue;
      }
    }
label_4:
    if (entity != null && entity is WTextBox)
      return (int) (entity as WTextBox).TextBoxFormat.WrapCollectionIndex;
    if (entity != null && entity is Shape)
      return (entity as Shape).WrapFormat.WrapCollectionIndex;
    return entity != null && entity is GroupShape ? (entity as GroupShape).WrapFormat.WrapCollectionIndex : -1;
  }

  internal bool IsInFootnote(WParagraph paragraph)
  {
    Entity owner = paragraph.Owner;
    while (true)
    {
      switch (owner)
      {
        case WFootnote _:
        case null:
          goto label_3;
        default:
          owner = owner.Owner;
          continue;
      }
    }
label_3:
    return owner is WFootnote;
  }

  internal Entity IsInTextBox(WParagraph paragraph)
  {
    Entity owner = paragraph.Owner;
    while (true)
    {
      switch (owner)
      {
        case WTextBox _:
        case null:
          goto label_3;
        default:
          owner = owner.Owner;
          continue;
      }
    }
label_3:
    return owner is WTextBox ? owner : (Entity) null;
  }

  internal float GetPageMarginLeft(WParagraph paragraph)
  {
    float left = (this.m_lcOperator as Layouter).ClientLayoutArea.Left;
    if (paragraph != null && paragraph.IsInCell)
      left = ((paragraph.GetOwnerEntity() as WTableCell).m_layoutInfo as CellLayoutInfo).CellContentLayoutingBounds.Left;
    return left;
  }

  private void UpdateParagraphTopMargin(WParagraph paragraph)
  {
    Borders borders = paragraph.ParagraphFormat.Borders;
    if (!borders.NoBorder && borders.Top.BorderType != BorderStyle.None && (double) (this.LayoutInfo as ParagraphLayoutInfo).Paddings.Top == 0.0 && this.LayoutInfo is ParagraphLayoutInfo && (this.LayoutInfo as ParagraphLayoutInfo).IsFirstLine)
      (this.LayoutInfo as ParagraphLayoutInfo).Paddings.Top = borders.Top.Space;
    if (!paragraph.IsTopMarginValueUpdated && (double) (this.LayoutInfo as ParagraphLayoutInfo).Margins.Top == 0.0)
    {
      if (paragraph.ParagraphFormat.SpaceBeforeAuto && !paragraph.Document.DOP.Dop2000.Copts.DontUseHTMLParagraphAutoSpacing)
      {
        if ((this.LayoutInfo as ParagraphLayoutInfo).ListValue != string.Empty || paragraph.IsFirstParagraphOfOwnerTextBody())
          (this.LayoutInfo as ParagraphLayoutInfo).Margins.Top = 0.0f;
        else
          (this.LayoutInfo as ParagraphLayoutInfo).Margins.Top = 14f;
      }
      else
        (this.LayoutInfo as ParagraphLayoutInfo).Margins.Top = paragraph.ParagraphFormat.BeforeSpacing;
    }
    if (paragraph.IsTopMarginValueUpdated || paragraph.IsFirstParagraphOfOwnerTextBody())
      return;
    if (!(paragraph.OwnerTextBody.Owner is BlockContentControl) && !paragraph.IsInCell && ((this.IsPageBreak(this.m_widget) || paragraph.ParagraphFormat.PageBreakBefore || paragraph.PreviousSibling is WParagraph && (paragraph.PreviousSibling as WParagraph).ParagraphFormat.PageBreakAfter) && paragraph.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 || this.IsSectionBreak(paragraph) || this.IsTOC(paragraph)))
    {
      if (paragraph.ParagraphFormat.SpaceBeforeAuto && !paragraph.Document.DOP.Dop2000.Copts.DontUseHTMLParagraphAutoSpacing)
        (this.LayoutInfo as ParagraphLayoutInfo).Margins.Top = 14f;
      else
        (this.LayoutInfo as ParagraphLayoutInfo).Margins.Top = paragraph.ParagraphFormat.BeforeSpacing;
    }
    else
      (this.LayoutInfo as ParagraphLayoutInfo).Margins.Top = 0.0f;
  }

  private bool IsTOC(WParagraph paragraph)
  {
    Hyperlink hyperlink;
    return (paragraph.ChildEntities.FirstItem is TableOfContent || paragraph.ChildEntities.FirstItem is WField && (paragraph.ChildEntities.FirstItem as WField).FieldType == FieldType.FieldHyperlink && (hyperlink = new Hyperlink(paragraph.ChildEntities.FirstItem as WField)).BookmarkName != null && this.StartsWithExt(hyperlink.BookmarkName, "_Toc")) && (this.IsPageBreak(this.m_widget) || paragraph.ParagraphFormat.PageBreakBefore || paragraph.PreviousSibling is WParagraph && (paragraph.PreviousSibling as WParagraph).ParagraphFormat.PageBreakAfter);
  }

  private bool IsPageBreak(IWidget childWidget)
  {
    if (!(childWidget is SplitWidgetContainer) || !((childWidget as SplitWidgetContainer).RealWidgetContainer is WParagraph))
      return false;
    WParagraph realWidgetContainer = (childWidget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
    int count = realWidgetContainer.ChildEntities.Count;
    if (count <= (childWidget as SplitWidgetContainer).Count || !(realWidgetContainer.ChildEntities[count - 1 - (childWidget as SplitWidgetContainer).Count] is Break))
      return false;
    Break childEntity = realWidgetContainer.ChildEntities[count - 1 - (childWidget as SplitWidgetContainer).Count] as Break;
    return childEntity.BreakType == BreakType.PageBreak || childEntity.BreakType == BreakType.ColumnBreak;
  }

  private bool IsSectionBreak(WParagraph para)
  {
    if (!(para.Owner is WTextBody) || para.IsInCell)
      return false;
    bool flag = (para.Owner as WTextBody).Items[0] == para;
    IWSection baseEntity = (IWSection) (this.GetBaseEntity((Entity) para) as WSection);
    if (!flag || baseEntity == null)
      return false;
    return baseEntity.BreakCode == SectionBreakCode.NewPage || baseEntity.BreakCode == SectionBreakCode.Oddpage || baseEntity.BreakCode == SectionBreakCode.EvenPage;
  }

  protected void CreateLayoutArea(RectangleF rect, Paddings cellPadding)
  {
    this.m_layoutArea = new LayoutArea(rect, this.LayoutInfo as ILayoutSpacingsInfo, this.m_widget);
  }

  protected void CreateLayoutedWidget(PointF location)
  {
    this.m_ltWidget = new LayoutedWidget(this.m_widget);
    RectangleF bounds = this.m_ltWidget.Bounds;
    if ((this.m_widget is WTableCell || this.m_widget is SplitWidgetContainer && (this.m_widget as SplitWidgetContainer).RealWidgetContainer is WTableCell) && this.LayoutInfo is ILayoutSpacingsInfo)
      location = this.m_layoutArea.ClientArea.Location;
    else if (this.LayoutInfo is ILayoutSpacingsInfo)
    {
      location.X += (this.LayoutInfo as ILayoutSpacingsInfo).Margins.Left;
      location.Y += (this.LayoutInfo as ILayoutSpacingsInfo).Margins.Top;
      if (this.LayoutInfo is ParagraphLayoutInfo && !(this.LayoutInfo as ParagraphLayoutInfo).IsFirstLine)
        location.Y -= (this.LayoutInfo as ILayoutSpacingsInfo).Margins.Top;
    }
    bounds.Location = location;
    this.m_ltWidget.Bounds = bounds;
  }

  internal void UpdateAreaWidth(float previousTabPosition)
  {
    this.m_layoutArea.UpdateWidth(previousTabPosition);
  }

  internal bool StartsWithExt(string text, string value) => text.StartsWithExt(value);

  protected void UpdateForceFitLayoutState(LayoutContext childContext)
  {
    if (!this.IsForceFitLayout || (!(this is LCLineContainer) || !(childContext is LCContainer)) && !(childContext is LCLineContainer) && !(childContext is LCTable) && !(this.Widget is WSection) && (!(this.Widget is SplitWidgetContainer) || !((this.Widget as SplitWidgetContainer).RealWidgetContainer is WSection)) && !(this.Widget is WordDocument) && (!(this.Widget is SplitWidgetContainer) || !((this.Widget as SplitWidgetContainer).RealWidgetContainer is WordDocument)))
      return;
    this.IsForceFitLayout = false;
  }

  public static LayoutContext Create(IWidget widget, ILCOperator lcOperator, bool isForceFitLayout)
  {
    if (widget is IWidgetContainer widgetContainer)
      return widgetContainer.LayoutInfo.IsLineContainer ? (LayoutContext) new LCLineContainer(widgetContainer, lcOperator, isForceFitLayout) : (LayoutContext) new LCContainer(widgetContainer, lcOperator, isForceFitLayout);
    ILeafWidget leafWidget = widget as ILeafWidget;
    if (widget is WField && (widget as WField).FieldType == FieldType.FieldSymbol)
      leafWidget = (ILeafWidget) (widget as WField).GetAsSymbol();
    if (leafWidget != null)
      return leafWidget is WMath ? (LayoutContext) new MathLayoutContext(leafWidget, lcOperator, isForceFitLayout) : (LayoutContext) new LeafLayoutContext(leafWidget, lcOperator, isForceFitLayout);
    ITableWidget table = (ITableWidget) null;
    if (widget is ITableWidget)
      table = widget as ITableWidget;
    if (table != null)
      return (LayoutContext) new LCTable(table, lcOperator, isForceFitLayout);
    if (widget is SplitTableWidget splitWidget)
      return (LayoutContext) new LCTable(splitWidget, lcOperator, isForceFitLayout);
    throw new ArgumentException("Invalid widget type: " + (object) widget.GetType());
  }

  internal bool IsInFrame(WParagraph paragraph)
  {
    return paragraph != null && paragraph.ParagraphFormat.IsFrame && !(paragraph.OwnerTextBody.Owner is WTextBox) && !(paragraph.OwnerTextBody.Owner is Shape) && !paragraph.IsInCell;
  }

  internal bool IsFrameInClientArea(WParagraph paragraph, RectangleF textWrappingBounds)
  {
    return !this.IsInFrame(paragraph) || !this.IsWord2013(paragraph.Document) && (this.m_lcOperator as Layouter).IsLayoutingHeaderFooter || (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Right > (double) textWrappingBounds.X - (double) paragraph.ParagraphFormat.FrameHorizontalDistanceFromText;
  }

  internal RectangleF GetFrameBounds(WParagraph paragraph, RectangleF bounds)
  {
    WSection section = !(this.m_lcOperator is Layouter lcOperator) || !lcOperator.IsLayoutingHeaderFooter ? this.GetBaseEntity((Entity) paragraph) as WSection : lcOperator.CurrentSection as WSection;
    WParagraphFormat paragraphFormat = paragraph.ParagraphFormat;
    float x = bounds.X;
    float y = bounds.Y;
    float width = bounds.Width;
    if ((double) paragraphFormat.FrameWidth != 0.0)
      width = paragraphFormat.FrameWidth;
    float height = bounds.Height;
    if (section != null && paragraphFormat != null && paragraphFormat.WrapFrameAround == FrameWrapMode.Around && !paragraph.IsInCell && section.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 && paragraphFormat.FrameHorizontalPos == (byte) 2 && paragraphFormat.FrameVerticalPos == (byte) 1)
      height = section.PageSetup.PageSize.Height;
    Entity baseTextBody = this.GetBaseTextBody((Entity) paragraph);
    if (baseTextBody is HeaderFooter)
      height = (baseTextBody.Owner as WSection).PageSetup.PageSize.Height;
    float frameHeight = 0.0f;
    bool IsAtleastHeight = false;
    if ((double) paragraphFormat.FrameHeight != 0.0)
    {
      ushort num = (ushort) ((double) paragraphFormat.FrameHeight * 20.0);
      IsAtleastHeight = ((int) num & 32768 /*0x8000*/) != 0;
      frameHeight = (float) (((int) num & (int) short.MaxValue) / 20);
    }
    if (!IsAtleastHeight && (double) paragraphFormat.FrameHeight != 0.0)
      height = frameHeight;
    if (section != null)
    {
      x = (double) paragraph.ParagraphFormat.FrameWidth != 0.0 || !paragraph.ParagraphFormat.IsNextParagraphInSameFrame() && !paragraph.ParagraphFormat.IsPreviousParagraphInSameFrame() ? this.GetPositionX(paragraphFormat, section, bounds, paragraphFormat.FrameWidth) : this.GetPositionX(paragraphFormat, section, bounds, bounds.Width);
      y = this.GetPositionY(paragraphFormat, section, bounds, frameHeight, IsAtleastHeight);
    }
    bounds = new RectangleF(x, y, width, height);
    if (paragraphFormat.IsPreviousParagraphInSameFrame())
      return (this.m_lcOperator as Layouter).FrameLayoutArea;
    (this.m_lcOperator as Layouter).FrameLayoutArea = bounds;
    if (IsAtleastHeight)
      (this.m_lcOperator as Layouter).FrameHeight = frameHeight;
    return bounds;
  }

  private Entity GetBaseTextBody(Entity entity)
  {
    Entity baseTextBody = entity;
    while (baseTextBody.Owner != null)
    {
      baseTextBody = baseTextBody.Owner;
      if (baseTextBody is WSection || baseTextBody is HeaderFooter)
        return baseTextBody;
    }
    return baseTextBody;
  }

  internal Entity GetBaseEntity(Entity entity)
  {
    if (entity == null)
      return (Entity) null;
    Entity baseEntity = entity;
    while (true)
    {
      switch (baseEntity)
      {
        case WSection _:
        case WTextBox _:
        case Shape _:
        case GroupShape _:
          goto label_6;
        default:
          if (baseEntity.Owner != null)
          {
            baseEntity = baseEntity.Owner;
            continue;
          }
          goto label_6;
      }
    }
label_6:
    return baseEntity;
  }

  internal bool IsBaseFromSection(Entity entity)
  {
    Entity entity1 = entity;
    while (true)
    {
      switch (entity1)
      {
        case WSection _:
        case WTextBox _:
        case Shape _:
        case GroupShape _:
        case WTableCell _:
          goto label_4;
        default:
          if (entity1.Owner != null)
          {
            entity1 = entity1.Owner;
            continue;
          }
          goto label_4;
      }
    }
label_4:
    return entity1 is WSection;
  }

  internal bool IsInTable(Entity entity)
  {
    Entity entity1 = entity;
    if (entity1.Owner == null)
      return false;
    Entity owner;
    for (owner = entity1.Owner; !(owner is WTable) && !(owner is WParagraph); owner = owner.Owner)
    {
      if (owner.Owner == null)
        return false;
    }
    return !(owner is WParagraph) || (owner as WParagraph).IsInCell;
  }

  private float GetPositionX(
    WParagraphFormat paraFormat,
    WSection section,
    RectangleF bounds,
    float frameWidth)
  {
    float positionX = 0.0f;
    if (paraFormat.IsFrameXAlign(paraFormat.FrameX))
    {
      switch ((short) paraFormat.FrameX)
      {
        case -16:
        case -8:
          switch (paraFormat.FrameHorizontalPos)
          {
            case 0:
              positionX = (this.m_lcOperator as Layouter).ClientLayoutArea.Width + (this.m_lcOperator as Layouter).ClientLayoutArea.Left;
              break;
            case 1:
              positionX = section.PageSetup.PageSize.Width - Layouter.GetRightMargin(section);
              break;
            case 2:
              positionX = section.PageSetup.PageSize.Width - frameWidth;
              break;
          }
          break;
        case -12:
        case 0:
          switch (paraFormat.FrameHorizontalPos)
          {
            case 0:
              positionX = (this.m_lcOperator as Layouter).ClientLayoutArea.X;
              break;
            case 1:
              positionX = Layouter.GetLeftMargin(section);
              break;
            case 2:
              positionX = 0.0f;
              break;
          }
          break;
        case -4:
          switch (paraFormat.FrameHorizontalPos)
          {
            case 0:
              RectangleF clientLayoutArea = (this.m_lcOperator as Layouter).ClientLayoutArea;
              positionX = (double) frameWidth >= (double) clientLayoutArea.Width ? clientLayoutArea.Left - (float) (((double) frameWidth - (double) clientLayoutArea.Width) / 2.0) : (this.m_lcOperator as Layouter).ClientLayoutArea.Left + (float) (((double) clientLayoutArea.Width - (double) frameWidth) / 2.0);
              break;
            case 1:
              positionX = Layouter.GetLeftMargin(section) + (float) (((double) section.PageSetup.PageSize.Width - (double) Layouter.GetRightMargin(section) - (double) frameWidth - (double) Layouter.GetLeftMargin(section)) / 2.0);
              break;
            case 2:
              positionX = (float) (((double) section.PageSetup.PageSize.Width - (double) frameWidth) / 2.0);
              break;
          }
          break;
        default:
          switch (paraFormat.FrameHorizontalPos)
          {
            case 0:
              positionX = (this.m_lcOperator as Layouter).ClientLayoutArea.X + paraFormat.FrameX;
              break;
            case 1:
              positionX = Layouter.GetLeftMargin(section) + paraFormat.FrameX;
              break;
            case 2:
              positionX = paraFormat.FrameX;
              break;
          }
          break;
      }
    }
    else
    {
      switch (paraFormat.FrameHorizontalPos)
      {
        case 0:
          positionX = (this.m_lcOperator as Layouter).ClientLayoutArea.X + paraFormat.FrameX;
          break;
        case 1:
          positionX = Layouter.GetLeftMargin(section) + paraFormat.FrameX;
          break;
        case 2:
          positionX = paraFormat.FrameX;
          break;
      }
    }
    return positionX;
  }

  private float GetPositionY(
    WParagraphFormat paraFormat,
    WSection section,
    RectangleF bounds,
    float frameHeight,
    bool IsAtleastHeight)
  {
    float positionY = 0.0f;
    float num1 = section.PageSetup.Margins.Top + (section.Document.DOP.GutterAtTop ? section.PageSetup.Margins.Gutter : 0.0f);
    float bottom = section.PageSetup.Margins.Bottom;
    float num2 = section.PageSetup.PageSize.Height - num1 - bottom;
    if ((int) ((double) paraFormat.FrameY * 20.0) == -3 && paraFormat.FrameVerticalAnchor == (byte) 2)
      return bounds.Y;
    if (paraFormat.IsFrameYAlign(paraFormat.FrameY))
    {
      switch ((short) paraFormat.FrameY)
      {
        case -20:
        case -12:
          switch (paraFormat.FrameVerticalAnchor)
          {
            case 0:
              positionY = num1 + num2;
              break;
            case 1:
              positionY = section.PageSetup.PageSize.Height;
              break;
          }
          break;
        case -16:
        case -4:
          switch (paraFormat.FrameVerticalAnchor)
          {
            case 0:
              positionY = num1;
              break;
            case 1:
              positionY = 0.0f;
              break;
          }
          break;
        case -8:
          switch (paraFormat.FrameVerticalAnchor)
          {
            case 0:
              positionY = num1 + num2 / 2f;
              break;
            case 1:
              positionY = section.PageSetup.PageSize.Height / 2f;
              break;
          }
          break;
        default:
          switch (paraFormat.FrameVerticalAnchor)
          {
            case 0:
              positionY = num1 + paraFormat.FrameY;
              break;
            case 1:
              positionY = IsAtleastHeight || (double) paraFormat.FrameY <= (double) section.PageSetup.PageSize.Height - (double) frameHeight || (double) paraFormat.FrameHeight == 0.0 ? paraFormat.FrameY : section.PageSetup.PageSize.Height - frameHeight;
              break;
            case 2:
              positionY = bounds.Y + paraFormat.FrameY;
              break;
          }
          break;
      }
    }
    else
    {
      switch (paraFormat.FrameVerticalAnchor)
      {
        case 0:
          positionY = num1 + paraFormat.FrameY;
          break;
        case 1:
          positionY = IsAtleastHeight || (double) paraFormat.FrameY <= (double) section.PageSetup.PageSize.Height - (double) frameHeight || (double) paraFormat.FrameHeight == 0.0 ? paraFormat.FrameY : section.PageSetup.PageSize.Height - frameHeight;
          break;
        case 2:
          positionY = bounds.Y + paraFormat.FrameY;
          break;
      }
    }
    return positionY;
  }
}
