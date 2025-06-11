// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.Layouter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.Rendering;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class Layouter : ILCOperator
{
  private DrawingContext m_drawingContext;
  internal RectangleF m_clientLayoutArea;
  private float m_pageTop;
  private List<SplitWidgetContainer> m_footnoteSplittedWidgets = new List<SplitWidgetContainer>();
  private List<SplitWidgetContainer> m_endnoteSplittedWidgets;
  private List<Entity> m_endnotesInstance;
  private TabsLayoutInfo.LayoutTab m_previousTab;
  private float m_previousTabWidth;
  private RectangleF m_frameLayoutArea;
  private float m_wrappingDifference = float.MinValue;
  private float m_rightPositionOfTabStopInterSectingFloattingItems = float.MinValue;
  private float m_maxRightPositionOfTabStopInterSectingFloattingItems = float.MinValue;
  private float m_frameHeight;
  internal float m_firstItemInPageYPosition;
  private ushort m_bFlags = 286;
  private byte m_byteFlag;
  private TableOfContent m_layoutingTOC;
  internal IEntity m_fieldentity;
  private int m_currentColumnIndex;
  private int m_countForConsecutiveLimit;
  private float m_ParagraphYPosition;
  internal List<float> m_lineSpaceWidths;
  internal float m_effectiveJustifyWidth;
  private WField m_unknownField;
  private bool m_isLayoutTrackChanges;
  private float m_hiddenLineBottom;

  public event Layouter.LeafLayoutEventHandler LeafLayoutAfter;

  internal float HiddenLineBottom
  {
    get => this.m_hiddenLineBottom;
    set => this.m_hiddenLineBottom = value;
  }

  internal bool IsLayoutingVerticalMergeStartCell
  {
    get => ((int) this.m_byteFlag & 1) != 0;
    set => this.m_byteFlag = (byte) ((int) this.m_byteFlag & 254 | (value ? 1 : 0));
  }

  internal bool IsNeedToRestartFootnote
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65533 | (value ? 1 : 0) << 1);
  }

  internal bool m_canSplitbyCharacter
  {
    get => ((int) this.m_bFlags & 2048 /*0x0800*/) >> 11 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 63487 | (value ? 1 : 0) << 11);
  }

  internal bool m_canSplitByTab
  {
    get => ((int) this.m_bFlags & 4096 /*0x1000*/) >> 12 != 0;
    set
    {
      this.m_bFlags = (ushort) ((int) this.m_bFlags & 61439 /*0xEFFF*/ | (value ? 1 : 0) << 12);
    }
  }

  internal bool IsNeedToRestartEndnote
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65531 | (value ? 1 : 0) << 2);
  }

  internal bool IsNeedToRestartFootnoteID
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65527 | (value ? 1 : 0) << 3);
  }

  internal bool IsNeedToRestartEndnoteID
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65519 | (value ? 1 : 0) << 4);
  }

  internal int CurrPageIndex => (this.LeafLayoutAfter.Target as DocumentLayouter).Pages.Count;

  internal bool IsNeedToRelayout
  {
    get => ((int) this.m_byteFlag & 2) >> 1 != 0;
    set => this.m_byteFlag = (byte) ((int) this.m_byteFlag & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsWord2013WordFitLayout
  {
    get => ((int) this.m_byteFlag & 4) >> 2 != 0;
    set => this.m_byteFlag = (byte) ((int) this.m_byteFlag & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsNeedToRelayoutTable
  {
    get => ((int) this.m_byteFlag & 8) >> 3 != 0;
    set => this.m_byteFlag = (byte) ((int) this.m_byteFlag & 247 | (value ? 1 : 0) << 3);
  }

  internal int PageNumber => (this.LeafLayoutAfter.Target as DocumentLayouter).CurrentPage.Number;

  internal float RemovedWidgetsHeight
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).RemovedWidgetsHeight;
    set => (this.LeafLayoutAfter.Target as DocumentLayouter).RemovedWidgetsHeight = value;
  }

  internal bool IsRowFitInSamePage
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65534 | (value ? 1 : 0));
  }

  internal bool IsLayoutingHeaderRow
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65471 | (value ? 1 : 0) << 6);
  }

  internal bool IsLayoutingTrackChanges
  {
    get => this.m_isLayoutTrackChanges;
    set => this.m_isLayoutTrackChanges = value;
  }

  internal IEntity FieldEntity
  {
    get => this.m_fieldentity;
    set => this.m_fieldentity = value;
  }

  public DrawingContext DrawingContext
  {
    get => this.m_drawingContext;
    set => this.m_drawingContext = value;
  }

  internal WField UnknownField
  {
    get => this.m_unknownField;
    set => this.m_unknownField = value;
  }

  internal RectangleF ClientLayoutArea => this.m_clientLayoutArea;

  internal float PageTopMargin => this.m_pageTop;

  internal RectangleF FrameLayoutArea
  {
    get => this.m_frameLayoutArea;
    set => this.m_frameLayoutArea = value;
  }

  internal float FrameHeight
  {
    get => this.m_frameHeight;
    set => this.m_frameHeight = value;
  }

  internal bool IsLayoutingHeaderFooter
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65503 | (value ? 1 : 0) << 5);
  }

  internal bool IsLayoutingHeader
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65407 | (value ? 128 /*0x80*/ : 0));
  }

  internal bool IsFirstItemInLine
  {
    get => ((int) this.m_bFlags & 512 /*0x0200*/) >> 9 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65023 | (value ? 1 : 0) << 9);
  }

  internal bool IsLayoutingFootnote
  {
    get => ((int) this.m_bFlags & 1024 /*0x0400*/) >> 10 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 64511 | (value ? 1 : 0) << 10);
  }

  internal float WrappingDifference
  {
    get => this.m_wrappingDifference;
    set => this.m_wrappingDifference = value;
  }

  internal float MaxRightPositionOfTabStopInterSectingFloattingItems
  {
    get => this.m_maxRightPositionOfTabStopInterSectingFloattingItems;
    set => this.m_maxRightPositionOfTabStopInterSectingFloattingItems = value;
  }

  internal float RightPositionOfTabStopInterSectingFloattingItems
  {
    get => this.m_rightPositionOfTabStopInterSectingFloattingItems;
    set => this.m_rightPositionOfTabStopInterSectingFloattingItems = value;
  }

  internal Dictionary<Entity, int> TOCEntryPageNumbers
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).TOCEntryPageNumbers;
  }

  internal List<ParagraphItem> tocParaItems
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).tocParaItems;
  }

  internal WParagraph LastTOCParagraph
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).LastTocEntity as WParagraph;
  }

  internal bool UpdatingPageFields => DocumentLayouter.m_UpdatingPageFields;

  internal bool IsNeedtoAdjustFooter
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).m_isNeedtoAdjustFooter;
    set => (this.LeafLayoutAfter.Target as DocumentLayouter).m_isNeedtoAdjustFooter = value;
  }

  internal LayoutedWidgetList FootnoteWidgets
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).CurrentPage.FootnoteWidgets;
  }

  internal LayoutedWidgetList LineNumberWidgets
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).CurrentPage.LineNumberWidgets;
  }

  internal LayoutedWidgetList EndnoteWidgets
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).CurrentPage.EndnoteWidgets;
  }

  internal List<Syncfusion.Layouting.TrackChangesMarkups> TrackChangesMarkups
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).CurrentPage.TrackChangesMarkups;
  }

  internal LayoutedWidgetList BehindWidgets
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).CurrentPage.BehindWidgets;
  }

  internal int NumberOfBehindWidgetsInHeader
  {
    get
    {
      return (this.LeafLayoutAfter.Target as DocumentLayouter).CurrentPage.NumberOfBehindWidgetsInHeader;
    }
    set
    {
      (this.LeafLayoutAfter.Target as DocumentLayouter).CurrentPage.NumberOfBehindWidgetsInHeader = value;
    }
  }

  internal int NumberOfBehindWidgetsInFooter
  {
    get
    {
      return (this.LeafLayoutAfter.Target as DocumentLayouter).CurrentPage.NumberOfBehindWidgetsInFooter;
    }
    set
    {
      (this.LeafLayoutAfter.Target as DocumentLayouter).CurrentPage.NumberOfBehindWidgetsInFooter = value;
    }
  }

  internal List<int> EndNoteSectionIndex
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).CurrentPage.EndNoteSectionIndex;
  }

  internal IWSection CurrentSection
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).CurrentSection;
  }

  internal List<int> FootNoteSectionIndex
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).CurrentPage.FootNoteSectionIndex;
  }

  internal List<SplitWidgetContainer> FootnoteSplittedWidgets
  {
    get
    {
      if (this.m_footnoteSplittedWidgets == null)
        this.m_footnoteSplittedWidgets = new List<SplitWidgetContainer>();
      return this.m_footnoteSplittedWidgets;
    }
    set => this.m_footnoteSplittedWidgets = value;
  }

  internal List<SplitWidgetContainer> EndnoteSplittedWidgets
  {
    get
    {
      if (this.m_endnoteSplittedWidgets == null)
        this.m_endnoteSplittedWidgets = new List<SplitWidgetContainer>();
      return this.m_endnoteSplittedWidgets;
    }
    set => this.m_endnoteSplittedWidgets = value;
  }

  internal List<Entity> EndnotesInstances
  {
    get
    {
      if (this.m_endnotesInstance == null)
        this.m_endnotesInstance = new List<Entity>();
      return this.m_endnotesInstance;
    }
    set => this.m_endnotesInstance = value;
  }

  internal List<FloatingItem> FloatingItems
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).FloatingItems;
  }

  internal List<FloatingItem> WrapFloatingItems
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).WrapFloatingItems;
  }

  internal WParagraph DynamicParagraph
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).m_dynamicParagraph;
    set => (this.LeafLayoutAfter.Target as DocumentLayouter).m_dynamicParagraph = value;
  }

  internal WTable DynamicTable
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).m_dynamicTable;
    set => (this.LeafLayoutAfter.Target as DocumentLayouter).m_dynamicTable = value;
  }

  internal List<Entity> NotFittedFloatingItems
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).m_notFittedfloatingItems;
    set => (this.LeafLayoutAfter.Target as DocumentLayouter).m_notFittedfloatingItems = value;
  }

  internal LayoutedWidget MaintainltWidget
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).MaintainltWidget;
    set => (this.LeafLayoutAfter.Target as DocumentLayouter).MaintainltWidget = value;
  }

  internal int[] m_interSectingPoint
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).InterSectingPoint;
    set => (this.LeafLayoutAfter.Target as DocumentLayouter).InterSectingPoint = value;
  }

  internal IWidgetContainer PageEndWidget
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).m_pageEndWidget;
    set => (this.LeafLayoutAfter.Target as DocumentLayouter).m_pageEndWidget = value;
  }

  internal bool IsForceFitLayout
  {
    get => (this.LeafLayoutAfter.Target as DocumentLayouter).IsForceFitLayout;
    set => (this.LeafLayoutAfter.Target as DocumentLayouter).IsForceFitLayout = value;
  }

  internal TabsLayoutInfo.LayoutTab PreviousTab
  {
    get
    {
      if (this.m_previousTab == null)
        this.m_previousTab = new TabsLayoutInfo.LayoutTab();
      return this.m_previousTab;
    }
    set => this.m_previousTab = value;
  }

  internal float PreviousTabWidth
  {
    get => this.m_previousTabWidth;
    set => this.m_previousTabWidth = value;
  }

  internal bool IsTabWidthUpdatedBasedOnIndent
  {
    get => ((int) this.m_bFlags & 32768 /*0x8000*/) >> 15 != 0;
    set
    {
      this.m_bFlags = (ushort) ((int) this.m_bFlags & (int) short.MaxValue | (value ? 1 : 0) << 15);
    }
  }

  internal TableOfContent LayoutingTOC
  {
    get => this.m_layoutingTOC;
    set => this.m_layoutingTOC = value;
  }

  internal int CurrentColumnIndex
  {
    get => this.m_currentColumnIndex;
    set => this.m_currentColumnIndex = value;
  }

  internal int CountForConsecutiveLimit
  {
    get => this.m_countForConsecutiveLimit;
    set => this.m_countForConsecutiveLimit = value;
  }

  internal float ParagraphYPosition
  {
    get => this.m_ParagraphYPosition;
    set => this.m_ParagraphYPosition = value;
  }

  internal bool IsTwoLinesLayouted
  {
    get => ((int) this.m_bFlags & 8192 /*0x2000*/) >> 13 != 0;
    set
    {
      this.m_bFlags = (ushort) ((int) this.m_bFlags & 57343 /*0xDFFF*/ | (value ? 1 : 0) << 13);
    }
  }

  internal bool IsFootnoteHeightAdjusted
  {
    get => ((int) this.m_bFlags & 16384 /*0x4000*/) >> 14 != 0;
    set
    {
      this.m_bFlags = (ushort) ((int) this.m_bFlags & 49151 /*0xBFFF*/ | (value ? 1 : 0) << 14);
    }
  }

  internal List<float> LineSpaceWidths
  {
    get
    {
      if (this.m_lineSpaceWidths == null)
        this.m_lineSpaceWidths = new List<float>();
      return this.m_lineSpaceWidths;
    }
  }

  public void Layout(IWidgetContainer widget, ILayoutProcessHandler handler, DrawingContext dc)
  {
    this.IsLayoutingHeaderFooter = widget is HeaderFooter;
    this.IsLayoutingHeader = this.IsLayoutingHeaderFooter && (widget as HeaderFooter).Type.ToString().Contains("Header");
    bool isContinuousSection = false;
    bool isSplittedWidget = true;
    IWidgetContainer curWidget = widget;
    List<IWidgetContainer> widgetContainerList = new List<IWidgetContainer>();
    List<LayoutedWidget> layoutedWidgetList = new List<LayoutedWidget>();
    bool isCurrentWidgetNeedToLayout = false;
    int columnIndex = 0;
    bool isFromDynmicLayout = false;
    this.m_drawingContext = dc;
    RectangleF rect;
    while (handler.GetNextArea(out rect, ref columnIndex, ref isContinuousSection, isSplittedWidget, ref this.m_pageTop, isFromDynmicLayout, ref curWidget))
    {
      this.CurrentColumnIndex = columnIndex;
      isFromDynmicLayout = false;
      if (rect.Equals((object) RectangleF.Empty))
        break;
      this.m_clientLayoutArea = rect;
      this.m_wrappingDifference = float.MinValue;
      this.IsNeedToRestartFootnote = true;
      this.IsNeedToRestartFootnoteID = true;
      this.IsNeedToRestartEndnote = true;
      this.IsNeedToRestartEndnoteID = true;
      if (this.CurrentSection is WSection && (this.CurrentSection as WSection).IsSectionFitInSamePage)
        this.IsNeedToRestartFootnote = false;
      else if (columnIndex == 0)
      {
        this.FootnoteWidgets.Clear();
        this.FootNoteSectionIndex.Clear();
      }
      LayoutContext lc = LayoutContext.Create((IWidget) curWidget, (ILCOperator) this, this.IsForceFitLayout);
      if (!this.IsLayoutingHeaderFooter)
        this.IsForceFitLayout = false;
      lc.ClientLayoutAreaRight = rect.Width;
      LayoutedWidget ltWidget = lc.Layout(rect);
      if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
        break;
      if (this.FootnoteSplittedWidgets.Count == 0)
        this.IsNeedToRestartFootnoteID = true;
      if (this.EndnoteSplittedWidgets.Count == 0)
        this.IsNeedToRestartEndnoteID = true;
      if (lc.State != LayoutState.DynamicRelayout)
      {
        this.IsCurrentWidgetNeedToLayout(lc, columnIndex, ref isSplittedWidget, ref isCurrentWidgetNeedToLayout, isContinuousSection);
        if (isCurrentWidgetNeedToLayout)
          continue;
      }
      if (!this.IsLayoutingHeaderFooter)
        this.LayoutTrackChangesBalloon(this);
      if (!isContinuousSection)
      {
        widgetContainerList.Clear();
        layoutedWidgetList.Clear();
        handler.PushLayoutedWidget(ltWidget, this.m_clientLayoutArea, this.IsNeedToRestartFootnoteID, this.IsNeedToRestartEndnoteID, lc.State, true);
      }
      else
      {
        widgetContainerList.Insert(widgetContainerList.Count, curWidget);
        layoutedWidgetList.Insert(layoutedWidgetList.Count, ltWidget);
        if (lc.State == LayoutState.DynamicRelayout)
        {
          int count = layoutedWidgetList.Count;
          for (int index = 0; index < count; ++index)
          {
            handler.PushLayoutedWidget(layoutedWidgetList[0], this.m_clientLayoutArea, this.IsNeedToRestartFootnoteID, this.IsNeedToRestartEndnoteID, lc.State, index == count - 1);
            widgetContainerList.RemoveAt(0);
            layoutedWidgetList.RemoveAt(0);
          }
        }
      }
      if (!lc.IsEnsureSplitted() && (lc.State != LayoutState.NotFitted || lc.SplittedWidget == null || !(lc.SplittedWidget is SplitWidgetContainer) || !((lc.SplittedWidget as SplitWidgetContainer)[0] is WSection) && (!((lc.SplittedWidget as SplitWidgetContainer)[0] is SplitWidgetContainer) || !(((lc.SplittedWidget as SplitWidgetContainer)[0] as SplitWidgetContainer).RealWidgetContainer is WSection))) && lc.State != LayoutState.DynamicRelayout || this.IsLayoutingHeaderFooter)
        break;
      if (lc.m_ltState == LayoutState.DynamicRelayout && this.m_interSectingPoint[0] != int.MinValue)
      {
        if (this.m_interSectingPoint[0] == 0)
          curWidget = this.MaintainltWidget.ChildWidgets[this.m_interSectingPoint[0]].Widget as IWidgetContainer;
        else if (this.m_interSectingPoint[0] != int.MinValue)
        {
          curWidget.InitLayoutInfo(this.MaintainltWidget.ChildWidgets[this.m_interSectingPoint[0]].ChildWidgets[this.m_interSectingPoint[1]].Widget);
          curWidget = !(curWidget is SplitWidgetContainer) || !((curWidget as SplitWidgetContainer).m_currentChild is SplitWidgetContainer) || !(((curWidget as SplitWidgetContainer).m_currentChild as SplitWidgetContainer).RealWidgetContainer is WordDocument) ? curWidget : (curWidget as SplitWidgetContainer).m_currentChild as IWidgetContainer;
        }
        this.PageEndWidget = lc.SplittedWidget as IWidgetContainer;
        isFromDynmicLayout = true;
        (this.LeafLayoutAfter.Target as DocumentLayouter).IsCreateNewPage = false;
        lc.m_ltState = LayoutState.Unknown;
      }
      else
      {
        if (lc.m_ltState == LayoutState.DynamicRelayout)
          lc.m_ltState = LayoutState.Unknown;
        if (lc.State == LayoutState.NotFitted && lc.SplittedWidget != null)
          (this.LeafLayoutAfter.Target as DocumentLayouter).IsCreateNewPage = true;
        SplitWidgetContainer splittedWidget = lc.SplittedWidget as SplitWidgetContainer;
        bool isLayoutedWidgetNeedToPushed = isContinuousSection;
        if (handler.HandleSplittedWidget(splittedWidget, lc.State, ltWidget, ref isLayoutedWidgetNeedToPushed))
        {
          if (isLayoutedWidgetNeedToPushed && isContinuousSection)
          {
            int count = layoutedWidgetList.Count;
            for (int index = 0; index < count; ++index)
            {
              handler.PushLayoutedWidget(layoutedWidgetList[0], this.m_clientLayoutArea, this.IsNeedToRestartFootnoteID, this.IsNeedToRestartEndnoteID, lc.State, true);
              widgetContainerList.RemoveAt(0);
              layoutedWidgetList.RemoveAt(0);
            }
          }
          else if (isContinuousSection)
            handler.HandleLayoutedWidget(ltWidget);
          curWidget = (IWidgetContainer) splittedWidget;
        }
        else
        {
          handler.HandleLayoutedWidget(ltWidget);
          curWidget = widgetContainerList[0];
        }
      }
    }
  }

  private void LayoutTrackChangesBalloon(Layouter layouter)
  {
    if (layouter.TrackChangesMarkups.Count <= 0)
      return;
    List<FloatingItem> floatingItemList = (List<FloatingItem>) null;
    if (layouter.FloatingItems.Count > 0)
    {
      floatingItemList = new List<FloatingItem>();
      foreach (FloatingItem floatingItem in layouter.FloatingItems)
        floatingItemList.Add(floatingItem);
      layouter.FloatingItems.Clear();
    }
    layouter.IsLayoutingTrackChanges = true;
    List<Syncfusion.Layouting.TrackChangesMarkups> updatedTrackChangesMarkups = new List<Syncfusion.Layouting.TrackChangesMarkups>();
    for (int index = 0; index < layouter.TrackChangesMarkups.Count; ++index)
    {
      Syncfusion.Layouting.TrackChangesMarkups trackChangesMarkup = layouter.TrackChangesMarkups[index];
      trackChangesMarkup.LtWidget = this.GetBalloonLayoutedWidget(trackChangesMarkup.ChangedValue, trackChangesMarkup.BallonYPosition, updatedTrackChangesMarkups, layouter);
      trackChangesMarkup.LtWidget.Bounds = new RectangleF(trackChangesMarkup.LtWidget.Bounds.X, trackChangesMarkup.LtWidget.Bounds.Y, trackChangesMarkup.LtWidget.Bounds.Width, trackChangesMarkup.LtWidget.Bounds.Height + 1f);
      this.AdjustBalloonPosition(trackChangesMarkup, updatedTrackChangesMarkups, trackChangesMarkup.BallonYPosition, layouter);
      updatedTrackChangesMarkups.Add(trackChangesMarkup);
    }
    updatedTrackChangesMarkups.Clear();
    layouter.IsLayoutingTrackChanges = false;
    if (floatingItemList == null)
      return;
    foreach (FloatingItem floatingItem in floatingItemList)
      layouter.FloatingItems.Add(floatingItem);
    floatingItemList.Clear();
  }

  private LayoutedWidget GetBalloonLayoutedWidget(
    WTextBody changedText,
    float yPosition,
    List<Syncfusion.Layouting.TrackChangesMarkups> updatedTrackChangesMarkups,
    Layouter layouter)
  {
    float y = updatedTrackChangesMarkups.Count < 1 ? yPosition : this.GetBalloonYposition(yPosition, updatedTrackChangesMarkups, layouter);
    if (updatedTrackChangesMarkups.Count > 0)
      y += 3f;
    RectangleF rect = new RectangleF((float) ((double) layouter.CurrentSection.PageSetup.PageSize.Width - (double) layouter.CurrentSection.PageSetup.Margins.Right + 32.0), y, 200f, layouter.ClientLayoutArea.Height);
    bool splitbyCharacter = layouter.m_canSplitbyCharacter;
    bool mCanSplitByTab = layouter.m_canSplitByTab;
    bool isFirstItemInLine = layouter.IsFirstItemInLine;
    TabsLayoutInfo.LayoutTab previousTab = layouter.PreviousTab;
    float previousTabWidth = layouter.PreviousTabWidth;
    LayoutContext layoutContext = LayoutContext.Create((IWidget) changedText, (ILCOperator) layouter, this.IsForceFitLayout);
    layoutContext.ClientLayoutAreaRight = rect.Width;
    float paragraphYposition = layouter.ParagraphYPosition;
    LayoutedWidget balloonLayoutedWidget = layoutContext.Layout(rect);
    layouter.ParagraphYPosition = paragraphYposition;
    layouter.PreviousTab = previousTab;
    layouter.PreviousTabWidth = previousTabWidth;
    layouter.m_canSplitbyCharacter = splitbyCharacter;
    layouter.m_canSplitByTab = mCanSplitByTab;
    layouter.IsFirstItemInLine = isFirstItemInLine;
    return balloonLayoutedWidget;
  }

  private float GetBalloonYposition(
    float yPosition,
    List<Syncfusion.Layouting.TrackChangesMarkups> updatedTrackChangesMarkups,
    Layouter layouter)
  {
    Syncfusion.Layouting.TrackChangesMarkups trackChangesMarkup1 = updatedTrackChangesMarkups[updatedTrackChangesMarkups.Count - 1];
    if ((double) yPosition < (double) trackChangesMarkup1.LtWidget.Bounds.Bottom)
    {
      if ((double) layouter.ClientLayoutArea.Bottom > (double) yPosition)
      {
        yPosition = trackChangesMarkup1.LtWidget.Bounds.Bottom;
      }
      else
      {
        float num1 = trackChangesMarkup1.LtWidget.Bounds.Bottom - yPosition;
        for (int index1 = updatedTrackChangesMarkups.Count - 1; index1 >= 0; --index1)
        {
          Syncfusion.Layouting.TrackChangesMarkups trackChangesMarkup2 = updatedTrackChangesMarkups[index1];
          if ((double) trackChangesMarkup2.EmptySpace == -3.4028234663852886E+38)
          {
            yPosition = trackChangesMarkup2.LtWidget.Bounds.Bottom;
            break;
          }
          if ((double) trackChangesMarkup2.EmptySpace > 0.0)
          {
            float num2 = (double) num1 < (double) trackChangesMarkup2.EmptySpace ? num1 : trackChangesMarkup2.EmptySpace;
            num1 -= num2;
            for (int index2 = index1; index2 < updatedTrackChangesMarkups.Count; ++index2)
            {
              LayoutedWidget ltWidget = updatedTrackChangesMarkups[index2].LtWidget;
              ltWidget.ShiftLocation(0.0, -(double) num2, false, false);
              updatedTrackChangesMarkups[index2].LtWidget = ltWidget;
              updatedTrackChangesMarkups[index2].EmptySpace -= num2;
            }
          }
          if ((double) num1 == 0.0)
            break;
        }
        yPosition += num1;
      }
    }
    return yPosition;
  }

  private void AdjustBalloonPosition(
    Syncfusion.Layouting.TrackChangesMarkups trackChangesMarkups,
    List<Syncfusion.Layouting.TrackChangesMarkups> updatedTrackChangesMarkups,
    float clientAreaY,
    Layouter layouter)
  {
    if (updatedTrackChangesMarkups.Count == 0)
    {
      if ((double) layouter.ClientLayoutArea.Bottom < (double) trackChangesMarkups.LtWidget.Bounds.Bottom)
      {
        float num1 = trackChangesMarkups.LtWidget.Bounds.Bottom - layouter.ClientLayoutArea.Top;
        float num2 = trackChangesMarkups.LtWidget.Bounds.Top - layouter.ClientLayoutArea.Top;
        float num3 = (double) num1 < (double) num2 ? num1 : num2;
        trackChangesMarkups.LtWidget.ShiftLocation(0.0, -(double) num3, false, false);
      }
      float num = clientAreaY - layouter.ClientLayoutArea.Y;
      if ((double) num == 0.0)
        trackChangesMarkups.EmptySpace = float.MinValue;
      else
        trackChangesMarkups.EmptySpace = num;
    }
    else
    {
      Syncfusion.Layouting.TrackChangesMarkups trackChangesMarkup = updatedTrackChangesMarkups[updatedTrackChangesMarkups.Count - 1];
      float num = clientAreaY - trackChangesMarkup.LtWidget.Bounds.Bottom;
      if ((double) num == 0.0 && (double) trackChangesMarkup.EmptySpace == -3.4028234663852886E+38)
        trackChangesMarkups.EmptySpace = float.MinValue;
      else
        trackChangesMarkups.EmptySpace = num;
    }
  }

  private void IsCurrentWidgetNeedToLayout(
    LayoutContext lc,
    int columnIndex,
    ref bool isSplittedWidget,
    ref bool isCurrentWidgetNeedToLayout,
    bool isContinuousSection)
  {
    if (lc.IsEnsureSplitted() && !isCurrentWidgetNeedToLayout && isContinuousSection && columnIndex == 0 && lc.SplittedWidget is SplitWidgetContainer && !((lc.SplittedWidget as SplitWidgetContainer)[0] is WSection) && (lc.SplittedWidget as SplitWidgetContainer)[0] is SplitWidgetContainer && ((lc.SplittedWidget as SplitWidgetContainer)[0] as SplitWidgetContainer).RealWidgetContainer is WSection)
    {
      WSection realWidgetContainer = ((lc.SplittedWidget as SplitWidgetContainer)[0] as SplitWidgetContainer).RealWidgetContainer as WSection;
      if (realWidgetContainer.Columns.Count <= 1)
        return;
      float width = realWidgetContainer.Columns[0].Width;
      bool flag = true;
      foreach (Column column in (CollectionImpl) realWidgetContainer.Columns)
      {
        if ((double) width != (double) column.Width)
        {
          flag = false;
          break;
        }
        width = column.Width;
      }
      if (flag || realWidgetContainer.PageSetup.EqualColumnWidth)
        return;
      isSplittedWidget = false;
      isCurrentWidgetNeedToLayout = true;
    }
    else
    {
      isSplittedWidget = true;
      isCurrentWidgetNeedToLayout = false;
    }
  }

  void ILCOperator.SendLeafLayoutAfter(LayoutedWidget ltWidget, bool isFromTOCLinkStyle)
  {
    if (this.LeafLayoutAfter == null)
      return;
    this.LeafLayoutAfter((object) this, ltWidget, isFromTOCLinkStyle);
  }

  internal void ResetWordLayoutingFlags(
    bool canSplitByCharacter,
    bool canSplitByTab,
    bool isFirstItemInLine,
    List<float> lineSpaceWidths,
    float width)
  {
    this.m_canSplitbyCharacter = canSplitByCharacter;
    this.m_canSplitByTab = canSplitByTab;
    this.IsFirstItemInLine = isFirstItemInLine;
    this.m_lineSpaceWidths = lineSpaceWidths;
    this.m_effectiveJustifyWidth = width;
  }

  internal static float GetLeftMargin(WSection section)
  {
    float left = section.PageSetup.Margins.Left;
    if (section.Document.DOP.MirrorMargins && section.PageSetup.Orientation == PageOrientation.Portrait && DocumentLayouter.PageNumber % 2 == 0)
      return section.PageSetup.Margins.Right;
    return !section.Document.DOP.GutterAtTop ? left + section.PageSetup.Margins.Gutter : left;
  }

  internal static float GetRightMargin(WSection section)
  {
    float right = section.PageSetup.Margins.Right;
    if (!section.Document.DOP.MirrorMargins || section.PageSetup.Orientation != PageOrientation.Portrait || DocumentLayouter.PageNumber % 2 != 0)
      return right;
    float left = section.PageSetup.Margins.Left;
    return !section.Document.DOP.GutterAtTop ? left + section.PageSetup.Margins.Gutter : left;
  }

  public delegate void LeafLayoutEventHandler(
    object sender,
    LayoutedWidget ltWidget,
    bool isFromTOCLinkStyle);
}
