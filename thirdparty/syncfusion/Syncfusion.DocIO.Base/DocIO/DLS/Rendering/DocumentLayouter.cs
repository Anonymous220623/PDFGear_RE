// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Rendering.DocumentLayouter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS.Rendering;

internal class DocumentLayouter : ILayoutProcessHandler
{
  private PageCollection m_pages = new PageCollection();
  private Page m_currPage;
  private IWidgetContainer m_docWidget;
  private IWSection m_currSection;
  private DocumentLayouter.HeaderFooterLPHandler m_headerLPHandler;
  private DocumentLayouter.HeaderFooterLPHandler m_footerLPHandler;
  private int m_columnIndex;
  private float m_columnsWidth;
  private int m_nextPageIndex;
  private bool m_bFirstPageForSection = true;
  private byte m_bFlag;
  private byte m_bitFlag;
  private bool m_preserveFormFields;
  private bool m_bisNeedToRestartFootnote = true;
  private bool m_bisNeedToRestartEndnote = true;
  private bool m_bDirty;
  private float m_totalColumnWidth;
  private WordToPDFResult m_pageResult;
  private int m_footnoteCount;
  private int m_endnoteCount;
  private float m_usedHeight;
  private float m_clientHeight;
  private float m_totalHeight;
  private float m_sectionFixedHeight;
  private float m_pageTop;
  private int m_sectionIndex = -1;
  private int m_sectionPagesCount;
  private int lineHeigthCount;
  private float m_firstParaHeight;
  private float m_footnoteHeight;
  private bool m_isFirstPage = true;
  private bool m_sectionNewPage;
  private bool m_createNewPage;
  private bool m_isContinuousSectionLayouted;
  private List<float> m_lineHeights = new List<float>();
  private List<float> m_columnHeight = new List<float>();
  private List<bool> m_columnHasBreakItem = new List<bool>();
  private List<float> m_prevColumnsWidth = new List<float>();
  private List<FloatingItem> m_prevFloatingItems;
  private List<float> m_absolutePositionedTableHeights;
  private Dictionary<int, List<string>> m_tocLevels;
  private List<ParagraphItem> m_tocParaItems;
  private Dictionary<Entity, int> m_tocEntryPageNumbers;
  private Dictionary<Entity, int> m_bkStartPageNumbers;
  private bool m_useTCFields;
  private Entity m_lastTocEntity;
  [ThreadStatic]
  internal static bool m_UpdatingPageFields;
  internal bool m_isNeedtoAdjustFooter;
  internal List<FloatingItem> m_FloatingItems;
  private List<FloatingItem> m_WrapFloatingItems = new List<FloatingItem>();
  internal WParagraph m_dynamicParagraph;
  internal WTable m_dynamicTable;
  internal List<Entity> m_notFittedfloatingItems = new List<Entity>();
  internal LayoutedWidget m_maintainltWidget;
  internal int[] m_interSectingPoint;
  internal IWidgetContainer m_pageEndWidget;
  internal ExportBookmarkType m_exportBookmarkType;
  private List<LayoutedWidget> m_editableFormFieldinEMF = new List<LayoutedWidget>();
  private int[] m_sectionNumPages;
  private float m_removedWidgetsHeight;
  [ThreadStatic]
  private static byte m_bFlags;
  [ThreadStatic]
  internal static DrawingContext m_dc;
  [ThreadStatic]
  internal static int PageNumber;
  [ThreadStatic]
  private static List<Dictionary<string, DocumentLayouter.BookmarkHyperlink>> m_bookmarkHyperlinks = (List<Dictionary<string, DocumentLayouter.BookmarkHyperlink>>) null;
  [ThreadStatic]
  private static List<BookmarkPosition> m_bookmarks = (List<BookmarkPosition>) null;
  [ThreadStatic]
  private static List<EquationField> m_equationFields = (List<EquationField>) null;
  [ThreadStatic]
  internal static int m_footnoteIDRestartEachPage = 1;
  [ThreadStatic]
  internal static int m_endnoteIDRestartEachPage = 1;
  [ThreadStatic]
  private static bool m_isPageEnd;
  [ThreadStatic]
  private static bool m_isEndUpdateTOC;
  [ThreadStatic]
  internal static int m_footnoteIDRestartEachSection = 1;
  [ThreadStatic]
  internal static int m_footnoteId;
  [ThreadStatic]
  internal static int m_endnoteId;
  private static bool m_isAzureInvoked;
  private static bool m_isAzureCompatible;

  internal static bool IsAzureCompatible
  {
    get
    {
      if (!DocumentLayouter.m_isAzureInvoked && !DocumentLayouter.m_isAzureCompatible)
        DocumentLayouter.m_isAzureCompatible = DocumentLayouter.IsEMFAzureCompatible();
      return DocumentLayouter.m_isAzureCompatible;
    }
  }

  internal static DrawingContext DrawingContext
  {
    get
    {
      if (DocumentLayouter.m_dc == null)
        DocumentLayouter.m_dc = new DrawingContext();
      return DocumentLayouter.m_dc;
    }
  }

  internal static bool IsFirstLayouting
  {
    get => ((int) DocumentLayouter.m_bFlags & 1) != 0;
    set
    {
      DocumentLayouter.m_bFlags = (byte) ((int) DocumentLayouter.m_bFlags & 254 | (value ? 1 : 0));
    }
  }

  internal static bool IsUpdatingTOC
  {
    get => ((int) DocumentLayouter.m_bFlags & 2) >> 1 != 0;
    set
    {
      DocumentLayouter.m_bFlags = (byte) ((int) DocumentLayouter.m_bFlags & 253 | (value ? 1 : 0) << 1);
    }
  }

  internal static bool IsLayoutingHeaderFooter
  {
    get => ((int) DocumentLayouter.m_bFlags & 4) >> 2 != 0;
    set
    {
      DocumentLayouter.m_bFlags = (byte) ((int) DocumentLayouter.m_bFlags & 251 | (value ? 1 : 0) << 2);
    }
  }

  internal static bool IsEndPage
  {
    get => DocumentLayouter.m_isPageEnd;
    set => DocumentLayouter.m_isPageEnd = value;
  }

  internal static List<Dictionary<string, DocumentLayouter.BookmarkHyperlink>> BookmarkHyperlinks
  {
    get
    {
      if (DocumentLayouter.m_bookmarkHyperlinks == null)
        DocumentLayouter.m_bookmarkHyperlinks = new List<Dictionary<string, DocumentLayouter.BookmarkHyperlink>>();
      return DocumentLayouter.m_bookmarkHyperlinks;
    }
  }

  internal static List<BookmarkPosition> Bookmarks
  {
    get
    {
      if (DocumentLayouter.m_bookmarks == null)
        DocumentLayouter.m_bookmarks = new List<BookmarkPosition>();
      return DocumentLayouter.m_bookmarks;
    }
  }

  internal bool IsForceFitLayout
  {
    get => ((int) this.m_bitFlag & 1) != 0;
    set => this.m_bitFlag = (byte) ((int) this.m_bitFlag & 254 | (value ? 1 : 0));
  }

  public bool PreserveFormField
  {
    get => this.m_preserveFormFields;
    set => this.m_preserveFormFields = value;
  }

  internal bool PreserveOleEquationAsBitmap
  {
    get => ((int) DocumentLayouter.m_bFlags & 8) >> 3 != 0;
    set
    {
      DocumentLayouter.m_bFlags = (byte) ((int) DocumentLayouter.m_bFlags & 247 | (value ? 1 : 0) << 3);
    }
  }

  internal List<LayoutedWidget> EditableFormFieldinEMF
  {
    get => this.m_editableFormFieldinEMF;
    set => this.m_editableFormFieldinEMF = value;
  }

  public PageCollection Pages => this.m_pages;

  internal Entity LastTocEntity
  {
    get => this.m_lastTocEntity;
    set => this.m_lastTocEntity = value;
  }

  internal static bool IsEndUpdateTOC
  {
    get => DocumentLayouter.m_isEndUpdateTOC;
    set => DocumentLayouter.m_isEndUpdateTOC = value;
  }

  internal ExportBookmarkType ExportBookmarks
  {
    get => this.m_exportBookmarkType;
    set => this.m_exportBookmarkType = value;
  }

  internal Page CurrentPage => this.m_currPage;

  internal IWSection CurrentSection => this.m_currSection;

  protected Column CurrentColumn
  {
    get
    {
      return this.CurrentSection.Columns.Count != 0 ? this.CurrentSection.Columns[this.m_columnIndex] : (Column) null;
    }
  }

  public WordToPDFResult PageResult => this.m_pageResult;

  protected bool IsEvenPage => this.m_nextPageIndex % 2 == 0;

  internal bool EnablePdfConformanceLevel
  {
    get => ((int) this.m_bFlag & 1) != 0;
    set => this.m_bFlag = (byte) ((int) this.m_bFlag & 254 | (value ? 1 : 0));
  }

  internal float RemovedWidgetsHeight
  {
    get => !this.m_isContinuousSectionLayouted ? 0.0f : this.m_removedWidgetsHeight;
    set => this.m_removedWidgetsHeight = value;
  }

  internal List<ParagraphItem> tocParaItems
  {
    get
    {
      if (this.m_tocParaItems == null)
        this.m_tocParaItems = new List<ParagraphItem>();
      return this.m_tocParaItems;
    }
    set => this.m_tocParaItems = value;
  }

  internal List<FloatingItem> FloatingItems
  {
    get
    {
      if (this.m_FloatingItems == null)
        this.m_FloatingItems = new List<FloatingItem>();
      return this.m_FloatingItems;
    }
    set => this.m_FloatingItems = value;
  }

  internal List<FloatingItem> WrapFloatingItems => this.m_WrapFloatingItems;

  internal static List<EquationField> EquationFields
  {
    get
    {
      if (DocumentLayouter.m_equationFields == null)
        DocumentLayouter.m_equationFields = new List<EquationField>();
      return DocumentLayouter.m_equationFields;
    }
  }

  internal LayoutedWidget MaintainltWidget
  {
    get
    {
      if (this.m_maintainltWidget == null)
        this.m_maintainltWidget = new LayoutedWidget();
      return this.m_maintainltWidget;
    }
    set => this.m_maintainltWidget = value;
  }

  internal int[] InterSectingPoint
  {
    get
    {
      if (this.m_interSectingPoint == null)
        this.m_interSectingPoint = new int[4]
        {
          int.MinValue,
          int.MinValue,
          int.MinValue,
          int.MinValue
        };
      return this.m_interSectingPoint;
    }
    set => this.m_interSectingPoint = value;
  }

  internal bool IsCreateNewPage
  {
    set => this.m_createNewPage = value;
  }

  internal Dictionary<Entity, int> TOCEntryPageNumbers
  {
    get
    {
      if (this.m_tocEntryPageNumbers == null)
        this.m_tocEntryPageNumbers = new Dictionary<Entity, int>();
      return this.m_tocEntryPageNumbers;
    }
  }

  internal Dictionary<Entity, int> BookmarkStartPageNumbers
  {
    get
    {
      if (this.m_bkStartPageNumbers == null)
        this.m_bkStartPageNumbers = new Dictionary<Entity, int>();
      return this.m_bkStartPageNumbers;
    }
  }

  internal int[] SectionNumPages
  {
    get
    {
      if (this.m_sectionNumPages == null)
        this.m_sectionNumPages = !(this.m_docWidget is WordDocument) ? new int[1] : new int[(this.m_docWidget as WordDocument).Sections.Count];
      return this.m_sectionNumPages;
    }
  }

  internal bool UseTCFields
  {
    get => this.m_useTCFields;
    set => this.m_useTCFields = value;
  }

  public DocumentLayouter()
  {
    this.m_pageResult = new WordToPDFResult();
    this.m_headerLPHandler = new DocumentLayouter.HeaderFooterLPHandler(this, false);
    this.m_footerLPHandler = new DocumentLayouter.HeaderFooterLPHandler(this, true);
  }

  public PageCollection Layout(IWordDocument doc)
  {
    if (doc.Sections.Count < 1)
      return (PageCollection) null;
    this.m_docWidget = doc as IWidgetContainer;
    if (this.m_docWidget == null)
      throw new DLSException("Document can't support IWidgetContainer interface");
    this.m_currSection = (IWSection) doc.Sections[0];
    DocumentLayouter.IsFirstLayouting = true;
    if (!this.LayoutPages())
    {
      this.m_bFirstPageForSection = true;
      DocumentLayouter.IsFirstLayouting = false;
      this.MaintainltWidget.ChildWidgets.Clear();
      int index1 = 0;
      for (int count = this.m_pages.Count; index1 < count; ++index1)
      {
        Page page = this.m_pages[index1];
        this.m_currSection.Document.PageCount = count;
        page.UpdateFieldsNumPages(count);
        if (this.m_currSection.Document.HasCoverPage && this.m_currSection.Document.Sections[0].PageSetup.HasKey(19) && this.m_currSection.Document.Sections[0].PageSetup.PageStartingNumber == 0)
        {
          int numPages = count - 1;
          page.UpdateFieldsNumPages(numPages);
        }
        for (int index2 = 0; index2 < page.FootnoteWidgets.Count; ++index2)
        {
          WTextBody wtextBody = page.FootnoteWidgets[index2].Widget is WTextBody ? page.FootnoteWidgets[index2].Widget as WTextBody : (page.FootnoteWidgets[index2].Widget is SplitWidgetContainer ? (page.FootnoteWidgets[index2].Widget as SplitWidgetContainer).RealWidgetContainer as WTextBody : (WTextBody) null);
          if (wtextBody != null && wtextBody.Owner is WFootnote)
          {
            (wtextBody.Owner as WFootnote).IsLayouted = false;
            (wtextBody.Owner as IWidget).LayoutInfo.IsSkip = false;
          }
        }
        for (int index3 = 0; index3 < page.EndnoteWidgets.Count; ++index3)
        {
          WTextBody wtextBody = page.EndnoteWidgets[index3].Widget is WTextBody ? page.EndnoteWidgets[index3].Widget as WTextBody : (page.EndnoteWidgets[index3].Widget is SplitWidgetContainer ? (page.EndnoteWidgets[index3].Widget as SplitWidgetContainer).RealWidgetContainer as WTextBody : (WTextBody) null);
          if (wtextBody != null && wtextBody.Owner is WFootnote)
            (wtextBody.Owner as WFootnote).IsLayouted = false;
        }
      }
      this.m_currSection = (IWSection) doc.Sections[0];
      this.m_isFirstPage = true;
      this.LayoutPages();
      DocumentLayouter.IsFirstLayouting = true;
    }
    if (doc is WordDocument && (doc as WordDocument).IsNeedToAddLineNumbers())
      this.AddLineNumbers(doc);
    return this.m_pages;
  }

  private void AddLineNumbers(IWordDocument doc)
  {
    WPageSetup pageSetup1 = doc.Sections[0].PageSetup;
    bool flag = true;
    int lineNumber = pageSetup1.LineNumberingStartValue <= 0 ? pageSetup1.LineNumberingStartValue + 1 : pageSetup1.LineNumberingStartValue;
    this.IntializeGraphics();
    for (int index1 = 0; index1 < this.m_pages.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.m_pages[index1].PageWidgets.Count; ++index2)
      {
        LayoutedWidget pageWidget = this.m_pages[index1].PageWidgets[index2];
        if (!(pageWidget.Widget is HeaderFooter))
        {
          for (int index3 = 0; index3 < pageWidget.ChildWidgets.Count; ++index3)
          {
            LayoutedWidget childWidget1 = pageWidget.ChildWidgets[index3];
            WPageSetup pageSetup2 = this.UpdateSectionPageSetup(childWidget1.Widget);
            if (!(pageSetup2.OwnerBase is WSection) || (pageSetup2.OwnerBase as WSection).LineNumbersEnabled())
            {
              if (pageSetup2.LineNumberingMode == LineNumberingMode.RestartSection || flag && pageSetup2.LineNumberingMode == LineNumberingMode.RestartPage)
                lineNumber = pageSetup2.LineNumberingStartValue <= 0 ? pageSetup2.LineNumberingStartValue + 1 : pageSetup2.LineNumberingStartValue;
              flag = false;
              float x = childWidget1.Bounds.X;
              for (int index4 = 0; index4 < childWidget1.ChildWidgets.Count; ++index4)
              {
                LayoutedWidget childWidget2 = childWidget1.ChildWidgets[index4];
                if (!(childWidget2.Widget is WParagraph wparagraph) && childWidget2.Widget is SplitWidgetContainer)
                  wparagraph = (childWidget2.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
                if (wparagraph != null)
                {
                  for (int index5 = 0; index5 < childWidget2.ChildWidgets.Count; ++index5)
                  {
                    if (lineNumber % pageSetup2.LineNumberingStep == 0)
                    {
                      if ((!wparagraph.SectionEndMark || wparagraph.ChildEntities.Count != 0) && !wparagraph.ParagraphFormat.SuppressLineNumbers)
                      {
                        this.LayoutLineNumber(childWidget2.ChildWidgets[index5], pageSetup2, lineNumber, x);
                        ++lineNumber;
                      }
                    }
                    else
                      ++lineNumber;
                  }
                }
              }
            }
          }
        }
      }
      flag = true;
    }
    DocumentLayouter.DrawingContext.Graphics.Dispose();
  }

  private void LayoutLineNumber(
    LayoutedWidget ltLineWidget,
    WPageSetup pageSetup,
    int lineNumber,
    float xPosition)
  {
    WTextRange txtRange = new WTextRange((IWordDocument) pageSetup.Document);
    txtRange.Text = lineNumber.ToString();
    WCharacterFormat numberingFormat = this.GetNumberingFormat(pageSetup.Document);
    if (numberingFormat != null)
      txtRange.ApplyCharacterFormat(numberingFormat);
    float ascentOfText = this.GetAscentOfText(ltLineWidget);
    float ascent = DocumentLayouter.DrawingContext.GetAscent(txtRange.CharacterFormat.GetFontToRender(txtRange.ScriptType));
    LayoutedWidget layoutedWidget = new LayoutedWidget((IWidget) txtRange);
    txtRange.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Horizontal);
    txtRange.m_layoutInfo.Font = new SyncFont(txtRange.CharacterFormat.GetFontToRender(txtRange.ScriptType));
    SizeF sizeF = txtRange.m_layoutInfo.Size = DocumentLayouter.DrawingContext.MeasureTextRange(txtRange, txtRange.Text);
    float y = ltLineWidget.Bounds.Y + ascentOfText - ascent;
    this.UpdateXPositionBasedOnFloatingItem(ltLineWidget, ref xPosition);
    layoutedWidget.Bounds = new RectangleF(xPosition - pageSetup.LineNumberingDistanceFromText - sizeF.Width, y, sizeF.Width, sizeF.Height);
    layoutedWidget.Owner = ltLineWidget;
    ltLineWidget.ChildWidgets.Insert(0, layoutedWidget);
  }

  private void UpdateXPositionBasedOnFloatingItem(LayoutedWidget ltLineWidget, ref float xPosition)
  {
    if (ltLineWidget.IntersectingBounds.Count <= 0)
      return;
    float num = ltLineWidget.ChildWidgets.Count > 0 ? this.GetFirstInlineItemXPosition(ltLineWidget) : ltLineWidget.Bounds.X;
    foreach (RectangleF intersectingBound in ltLineWidget.IntersectingBounds)
    {
      if (Math.Round((double) intersectingBound.Right, 1) <= Math.Round((double) num, 2))
      {
        xPosition = intersectingBound.Right;
        break;
      }
    }
  }

  private float GetFirstInlineItemXPosition(LayoutedWidget lineWidget)
  {
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) lineWidget.ChildWidgets)
    {
      IWidget widget = childWidget.Widget;
      switch (widget)
      {
        case SplitStringWidget _:
          return childWidget.Bounds.X;
        case ParagraphItem _:
          if (!(widget as ParagraphItem).IsFloatingItem(false))
            return childWidget.Bounds.X;
          continue;
        default:
          continue;
      }
    }
    return lineWidget.Bounds.X;
  }

  private float GetAscentOfText(LayoutedWidget paraLtWidget)
  {
    if (!(paraLtWidget.Owner.Widget is WParagraph paragraph) && paraLtWidget.Owner.Widget is SplitWidgetContainer)
      paragraph = (paraLtWidget.Owner.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
    bool isFirstLineOfParagraph = false;
    bool isLastLineOfParagraph = false;
    if (paraLtWidget.ChildWidgets.Count > 0)
    {
      isFirstLineOfParagraph = paragraph.IsFirstLine(paraLtWidget.ChildWidgets[0]);
      isLastLineOfParagraph = paragraph.IsLastLine(paraLtWidget.ChildWidgets[paraLtWidget.ChildWidgets.Count - 1]);
    }
    IStringWidget lastTextWidget = (IStringWidget) null;
    double maxAscent;
    paraLtWidget.CalculateMaxChildWidget(DocumentLayouter.DrawingContext, paragraph, isFirstLineOfParagraph, isLastLineOfParagraph, out double _, out maxAscent, out double _, out double _, out double _, out float _, out double _, out lastTextWidget, out bool _, out bool _, out bool _, out bool _);
    return (float) maxAscent;
  }

  private WPageSetup UpdateSectionPageSetup(IWidget widget)
  {
    switch (widget)
    {
      case WSection _:
        return (widget as WSection).PageSetup;
      case SplitWidgetContainer _:
        if ((widget as SplitWidgetContainer).RealWidgetContainer is WSection)
          return ((widget as SplitWidgetContainer).RealWidgetContainer as WSection).PageSetup;
        break;
    }
    return (WPageSetup) null;
  }

  private void IntializeGraphics()
  {
    DocumentLayouter.DrawingContext.Graphics = Graphics.FromImage(this.CreateImage(this.Pages[0].Setup, ImageType.Metafile, (MemoryStream) null));
    DocumentLayouter.DrawingContext.Graphics.PageUnit = GraphicsUnit.Point;
  }

  private WCharacterFormat GetNumberingFormat(WordDocument doc)
  {
    foreach (IStyle style in (IEnumerable) doc.Styles)
    {
      if (style.Name == "Line Number" && style.StyleType == StyleType.CharacterStyle && style is WCharacterStyle)
        return (style as WCharacterStyle).CharacterFormat;
    }
    return (WCharacterFormat) null;
  }

  public void InitLayoutInfo()
  {
    for (int index = 0; index < this.m_pages.Count; ++index)
      this.m_pages[index].InitLayoutInfo();
  }

  public void Close()
  {
    if (DocumentLayouter.m_dc != null)
    {
      DocumentLayouter.m_dc.Close();
      DocumentLayouter.m_dc = (DrawingContext) null;
    }
    if (DocumentLayouter.m_bookmarkHyperlinks != null)
    {
      DocumentLayouter.m_bookmarkHyperlinks.Clear();
      DocumentLayouter.m_bookmarkHyperlinks = (List<Dictionary<string, DocumentLayouter.BookmarkHyperlink>>) null;
    }
    if (DocumentLayouter.m_bookmarks != null)
    {
      DocumentLayouter.m_bookmarks.Clear();
      DocumentLayouter.m_bookmarks = (List<BookmarkPosition>) null;
    }
    if (DocumentLayouter.m_equationFields != null)
    {
      DocumentLayouter.m_equationFields.Clear();
      DocumentLayouter.m_equationFields = (List<EquationField>) null;
    }
    if (this.FloatingItems != null)
    {
      this.FloatingItems.Clear();
      this.FloatingItems = (List<FloatingItem>) null;
    }
    if (this.m_WrapFloatingItems != null)
    {
      this.m_WrapFloatingItems.Clear();
      this.m_WrapFloatingItems = (List<FloatingItem>) null;
    }
    if (this.MaintainltWidget != null)
    {
      this.MaintainltWidget.ChildWidgets.Clear();
      this.MaintainltWidget = (LayoutedWidget) null;
    }
    if (this.m_tocEntryPageNumbers != null)
    {
      this.m_tocEntryPageNumbers.Clear();
      this.m_tocEntryPageNumbers = (Dictionary<Entity, int>) null;
    }
    if (this.m_tocParaItems != null)
    {
      this.m_tocParaItems.Clear();
      this.m_tocParaItems = (List<ParagraphItem>) null;
    }
    if (this.m_tocLevels != null)
    {
      this.m_tocLevels.Clear();
      this.m_tocLevels = (Dictionary<int, List<string>>) null;
    }
    if (this.m_absolutePositionedTableHeights != null)
    {
      this.m_absolutePositionedTableHeights.Clear();
      this.m_absolutePositionedTableHeights = (List<float>) null;
    }
    if (this.m_prevFloatingItems != null)
    {
      this.m_prevFloatingItems.Clear();
      this.m_prevFloatingItems = (List<FloatingItem>) null;
    }
    if (this.m_prevColumnsWidth != null)
    {
      this.m_prevColumnsWidth.Clear();
      this.m_prevColumnsWidth = (List<float>) null;
    }
    if (this.m_columnHasBreakItem != null)
    {
      this.m_columnHasBreakItem.Clear();
      this.m_columnHasBreakItem = (List<bool>) null;
    }
    if (this.m_columnHeight != null)
    {
      this.m_columnHeight.Clear();
      this.m_columnHeight = (List<float>) null;
    }
    if (this.m_lineHeights != null)
    {
      this.m_lineHeights.Clear();
      this.m_lineHeights = (List<float>) null;
    }
    if (this.m_pages != null)
    {
      this.m_pages.Clear();
      this.m_pages = (PageCollection) null;
    }
    if (this.m_bkStartPageNumbers != null)
    {
      this.m_bkStartPageNumbers.Clear();
      this.m_bkStartPageNumbers = (Dictionary<Entity, int>) null;
    }
    if (this.m_currPage != null && this.m_currPage.TrackChangesMarkups != null)
    {
      this.m_currPage.TrackChangesMarkups.Clear();
      this.m_currPage.TrackChangesMarkups = (List<TrackChangesMarkups>) null;
    }
    if (this.m_currPage != null)
      this.m_currPage = (Page) null;
    if (this.m_docWidget != null)
      this.m_docWidget = (IWidgetContainer) null;
    if (this.m_currSection != null)
      this.m_currSection = (IWSection) null;
    if (this.m_headerLPHandler != null)
      this.m_headerLPHandler = (DocumentLayouter.HeaderFooterLPHandler) null;
    if (this.m_footerLPHandler != null)
      this.m_footerLPHandler = (DocumentLayouter.HeaderFooterLPHandler) null;
    if (this.m_pageResult != null)
      this.m_pageResult = (WordToPDFResult) null;
    if (this.m_notFittedfloatingItems != null)
    {
      this.m_notFittedfloatingItems.Clear();
      this.m_notFittedfloatingItems = (List<Entity>) null;
    }
    this.m_sectionNumPages = (int[]) null;
  }

  public Image[] DrawToImage(
    int startPageIndex,
    int noOfPages,
    ImageType imageType,
    MemoryStream stream)
  {
    if (this.m_pages.Count < startPageIndex || startPageIndex < 0)
      return (Image[]) null;
    if (this.m_pages.Count < startPageIndex + noOfPages)
      noOfPages = this.m_pages.Count - startPageIndex;
    int length = noOfPages == -1 ? this.m_pages.Count : startPageIndex + noOfPages;
    Image[] image1 = new Image[length];
    for (int index = startPageIndex; index < length; ++index)
    {
      Image image2 = this.CreateImage(this.m_pages[index].Setup, imageType, stream);
      DocumentLayouter.PageNumber = index + 1;
      using (Graphics graphics = Graphics.FromImage(image2))
      {
        graphics.PageUnit = GraphicsUnit.Point;
        graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, (int) this.m_pages[index].Setup.PageSize.Width, (int) this.m_pages[index].Setup.PageSize.Height));
        DrawingContext drawingContext = new DrawingContext(graphics, GraphicsUnit.Point);
        drawingContext.FontNames = this.CurrentSection.Document.FontSettings.FontNames;
        bool flag = !this.CurrentSection.PageSetup.IsFrontPageBorder;
        if (flag)
          drawingContext.DrawPageBorder(index, this.m_pages);
        drawingContext.Draw(this.m_pages[index]);
        if (!flag)
          drawingContext.DrawPageBorder(index, this.m_pages);
        this.m_pageResult.Pages.Add(new Syncfusion.DocIO.DLS.Rendering.PageResult(image2, drawingContext.Hyperlinks, DrawingContext.BookmarkHyperlinksList));
        graphics.Dispose();
      }
      image1[index] = image2;
    }
    this.InitLayoutInfo();
    return image1;
  }

  public Stream DrawToStream(
    int startPageIndex,
    int noOfPages,
    ImageType imageType,
    MemoryStream stream)
  {
    if (this.m_pages.Count < startPageIndex || startPageIndex < 0)
      return (Stream) null;
    if (this.m_pages.Count < startPageIndex + noOfPages)
      noOfPages = this.m_pages.Count - startPageIndex;
    int length = noOfPages == -1 ? this.m_pages.Count : startPageIndex + noOfPages;
    Image[] imageArray = new Image[length];
    for (int index = startPageIndex; index < length; ++index)
    {
      stream = new MemoryStream();
      Image image = this.CreateImage(this.m_pages[index].Setup, imageType, stream);
      DocumentLayouter.PageNumber = index + 1;
      using (Graphics graphics = Graphics.FromImage(image))
      {
        graphics.PageUnit = GraphicsUnit.Point;
        graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, (int) this.m_pages[index].Setup.PageSize.Width, (int) this.m_pages[index].Setup.PageSize.Height));
        DrawingContext drawingContext = new DrawingContext(graphics, GraphicsUnit.Point);
        drawingContext.FontNames = this.CurrentSection.Document.FontSettings.FontNames;
        bool flag = !this.CurrentSection.PageSetup.IsFrontPageBorder;
        if (flag)
          drawingContext.DrawPageBorder(index, this.m_pages);
        drawingContext.Draw(this.m_pages[index]);
        if (!flag)
          drawingContext.DrawPageBorder(index, this.m_pages);
        this.m_pageResult.Pages.Add(new Syncfusion.DocIO.DLS.Rendering.PageResult(image, drawingContext.Hyperlinks, DrawingContext.BookmarkHyperlinksList));
        graphics.Dispose();
      }
      imageArray[index] = image;
    }
    return (Stream) stream;
  }

  internal void DrawToImage(
    int startPageIndex,
    int noOfPages,
    ImageType imageType,
    ref bool isContainsComplexScript,
    ref List<KeyValuePair<string, bool>> commentStartMarks)
  {
    if (this.m_pages.Count < startPageIndex || startPageIndex < 0)
      return;
    if (this.m_pages.Count < startPageIndex + noOfPages)
      noOfPages = this.m_pages.Count - startPageIndex;
    MemoryStream stream = (MemoryStream) null;
    Image image = this.CreateImage(this.m_pages[startPageIndex].Setup, imageType, stream);
    DocumentLayouter.PageNumber = startPageIndex + 1;
    using (Graphics graphics = Graphics.FromImage(image))
    {
      if (this.m_pages[0] != null && this.m_pages[0].DocSection.Document.TrackChangesBalloonCount > 0)
      {
        float right = this.m_pages[startPageIndex].Setup.Margins.Right;
        double sx = (double) this.m_pages[startPageIndex].Setup.PageSize.Width / ((double) this.m_pages[startPageIndex].Setup.PageSize.Width + (250.0 - (double) right + 10.0));
        graphics.ScaleTransform((float) sx, 1f);
      }
      graphics.PageUnit = GraphicsUnit.Point;
      Brush brush = Brushes.Transparent;
      if (this.EnablePdfConformanceLevel)
        brush = Brushes.White;
      graphics.FillRectangle(brush, new Rectangle(0, 0, (int) this.m_pages[startPageIndex].Setup.PageSize.Width, (int) this.m_pages[startPageIndex].Setup.PageSize.Height));
      DrawingContext drawingContext = new DrawingContext(graphics, GraphicsUnit.Point);
      if (commentStartMarks != null)
        drawingContext.m_previousLineCommentStartMarks = commentStartMarks;
      drawingContext.ExportBookmarks = this.ExportBookmarks;
      drawingContext.FontStreams = this.CurrentSection.Document.FontSettings.FontStreams;
      drawingContext.PrivateFonts = this.CurrentSection.Document.FontSettings.PrivateFonts;
      drawingContext.FontNames = this.CurrentSection.Document.FontSettings.FontNames;
      drawingContext.EditableFormFieldinEMF = this.EditableFormFieldinEMF;
      drawingContext.PreserveFormField = this.PreserveFormField;
      drawingContext.PreserveOleEquationAsBitmap = this.PreserveOleEquationAsBitmap;
      bool flag = !this.CurrentSection.PageSetup.IsFrontPageBorder;
      if (flag)
        drawingContext.DrawPageBorder(startPageIndex, this.m_pages);
      drawingContext.Draw(this.m_pages[startPageIndex]);
      if (!flag)
        drawingContext.DrawPageBorder(startPageIndex, this.m_pages);
      isContainsComplexScript = drawingContext.EnableComplexScript;
      if (drawingContext.m_previousLineCommentStartMarks != null)
        commentStartMarks = drawingContext.m_previousLineCommentStartMarks;
      this.m_pageResult.Pages.Add(new Syncfusion.DocIO.DLS.Rendering.PageResult(image, drawingContext.Hyperlinks, DrawingContext.BookmarkHyperlinksList));
      graphics.Dispose();
    }
  }

  private bool LayoutPages()
  {
    this.m_bDirty = false;
    this.m_pages.Clear();
    DocumentLayouter.BookmarkHyperlinks.Clear();
    DocumentLayouter.Bookmarks.Clear();
    this.m_nextPageIndex = 0;
    this.CreateNewPage(ref this.m_docWidget);
    using (Graphics graphics = Graphics.FromImage(this.CreateImage(this.m_currPage.Setup, ImageType.Metafile, (MemoryStream) null)))
    {
      graphics.PageUnit = GraphicsUnit.Point;
      graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, (int) this.m_currPage.Setup.PageSize.Width, (int) this.m_currPage.Setup.PageSize.Height));
      DocumentLayouter.m_dc = new DrawingContext(graphics, GraphicsUnit.Point);
      if (this.m_docWidget is WordDocument)
      {
        DocumentLayouter.m_dc.PrivateFonts = (this.m_docWidget as WordDocument).FontSettings.PrivateFonts;
        DocumentLayouter.m_dc.FontNames = (this.m_docWidget as WordDocument).FontSettings.FontNames;
      }
      this.LayoutHeaderFooter();
      this.ClearFields();
      if (this.m_docWidget is WordDocument)
        (this.m_docWidget as WordDocument).ClearLists();
      this.m_bFirstPageForSection = false;
      Layouter layouter = new Layouter();
      layouter.LeafLayoutAfter += new Layouter.LeafLayoutEventHandler(this.Layouter_LeafLayoutAfter);
      layouter.Layout(this.m_docWidget, (ILayoutProcessHandler) this, DocumentLayouter.m_dc);
      layouter.LeafLayoutAfter -= new Layouter.LeafLayoutEventHandler(this.Layouter_LeafLayoutAfter);
      graphics.Dispose();
    }
    if (this.m_docWidget is WordDocument)
    {
      if (DocumentLayouter.IsFirstLayouting && (this.m_docWidget as WordDocument).LastSection == this.CurrentSection && this.CurrentSection is WSection)
        this.SectionNumPages[(this.CurrentSection as WSection).Index] = this.m_sectionPagesCount;
      (this.m_docWidget as WordDocument).ClearLists();
    }
    return !this.m_bDirty;
  }

  internal byte[] ConvertAsImage(IWidget widget)
  {
    this.m_docWidget = (IWidgetContainer) (widget as Entity).Document;
    this.m_currSection = (IWSection) ((widget as Entity).GetOwnerSection(widget as Entity) as WSection);
    this.m_currPage = new Page(this.m_currSection, 0);
    LayoutedWidget ltWidget = (LayoutedWidget) null;
    using (Graphics graphics = Graphics.FromImage(this.CreateImage(this.m_currPage.Setup, ImageType.Metafile, (MemoryStream) null)))
    {
      graphics.PageUnit = GraphicsUnit.Point;
      graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, (int) this.m_currPage.Setup.PageSize.Width, (int) this.m_currPage.Setup.PageSize.Height));
      DrawingContext drawingContext = new DrawingContext(graphics, GraphicsUnit.Point);
      DocumentLayouter.m_dc = drawingContext;
      if (this.m_docWidget is WordDocument)
      {
        DocumentLayouter.m_dc.PrivateFonts = (this.m_docWidget as WordDocument).FontSettings.PrivateFonts;
        DocumentLayouter.m_dc.FontNames = (this.m_docWidget as WordDocument).FontSettings.FontNames;
      }
      Layouter layouter = new Layouter();
      layouter.LeafLayoutAfter += new Layouter.LeafLayoutEventHandler(this.Layouter_LeafLayoutAfter);
      layouter.DrawingContext = drawingContext;
      RectangleF rect = new RectangleF(0.0f, 0.0f, this.CurrentPage.Setup.PageSize.Width, this.CurrentPage.Setup.PageSize.Height);
      layouter.m_clientLayoutArea = rect;
      ltWidget = LayoutContext.Create(widget, (ILCOperator) layouter, true).Layout(rect);
      layouter.LeafLayoutAfter -= new Layouter.LeafLayoutEventHandler(this.Layouter_LeafLayoutAfter);
    }
    return this.DrawAsImage(widget, ltWidget);
  }

  private byte[] DrawAsImage(IWidget widget, LayoutedWidget ltWidget)
  {
    float width = (float) Math.Ceiling((double) ltWidget.Bounds.Width);
    float height = (float) Math.Ceiling((double) ltWidget.Bounds.Height);
    Image image = this.CreateImage(width + 2f, height + 2f);
    if ((double) ltWidget.Bounds.X != 0.0 || (double) ltWidget.Bounds.Y != 0.0)
      ltWidget.ShiftLocation(-(double) ltWidget.Bounds.X, -(double) ltWidget.Bounds.Y, true, false);
    using (Graphics graphics = Graphics.FromImage(image))
    {
      graphics.PageUnit = GraphicsUnit.Point;
      graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
      graphics.SmoothingMode = SmoothingMode.HighQuality;
      graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, (int) width, (int) height));
      DrawingContext drawingContext = new DrawingContext(graphics, GraphicsUnit.Point);
      if (widget is WMath)
        new MathRenderer(drawingContext).Draw(widget as WMath, ltWidget);
      else
        drawingContext.Draw(ltWidget, true);
      graphics.Dispose();
    }
    byte[] byteArray = this.ConvertToByteArray(image);
    image.Dispose();
    return byteArray;
  }

  private byte[] ConvertToByteArray(Image image)
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      image.Save((Stream) memoryStream, ImageFormat.Png);
      return memoryStream.ToArray();
    }
  }

  private Image CreateImage(float pageWidth, float pageHeight)
  {
    return (Image) new Bitmap((int) UnitsConvertor.Instance.ConvertToPixels(pageWidth, PrintUnits.Point), (int) UnitsConvertor.Instance.ConvertToPixels(pageHeight, PrintUnits.Point), PixelFormat.Format32bppPArgb);
  }

  private void CreateNewPage(ref IWidgetContainer curWidget)
  {
    if (this.MaintainltWidget.ChildWidgets.Count > 0 && (this.m_dynamicParagraph != null || this.m_dynamicTable != null))
    {
      LayoutedWidget layoutedWidget = this.MaintainltWidget;
      if (this.m_dynamicTable != null)
      {
        while (!(layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1].Widget is WTable))
          layoutedWidget = layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1];
      }
      else
      {
        while (!(layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1].Widget is WParagraph))
          layoutedWidget = layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1];
      }
      layoutedWidget.ChildWidgets.Remove(layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1]);
      this.CurrentPage.PageWidgets.RemoveRange(2, this.CurrentPage.PageWidgets.Count - 2);
      for (int index = 0; index < this.MaintainltWidget.ChildWidgets.Count; ++index)
        this.CurrentPage.PageWidgets.Add(this.MaintainltWidget.ChildWidgets[index]);
      this.m_dynamicParagraph = (WParagraph) null;
      this.m_dynamicTable = (WTable) null;
      curWidget = this.m_pageEndWidget;
    }
    this.m_currPage = new Page(this.CurrentSection, this.m_nextPageIndex);
    if (DocumentLayouter.IsFirstLayouting)
      ++this.m_sectionPagesCount;
    if (this.m_bisNeedToRestartFootnote)
      DocumentLayouter.m_footnoteIDRestartEachPage = 1;
    if (this.m_currSection.PageSetup.RestartPageNumbering && this.m_bFirstPageForSection || this.m_nextPageIndex == 0 && this.m_currSection.PageSetup.PageStartingNumber > 0)
    {
      this.m_currPage.Number = this.m_currSection.PageSetup.PageStartingNumber - 1;
      this.m_nextPageIndex = this.m_currPage.Number;
    }
    this.ResetNotAddedFloatingEntityProperty();
    if (this.m_FloatingItems.Count > 0)
      this.ResetFloatingItemsProperties();
    this.MaintainltWidget.ChildWidgets.Clear();
    this.InterSectingPoint = (int[]) null;
    this.m_FloatingItems.Clear();
    this.m_notFittedfloatingItems.Clear();
    this.m_WrapFloatingItems.Clear();
    if (!DocumentLayouter.IsUpdatingTOC)
      this.m_pages.Add(this.m_currPage);
    ++this.m_nextPageIndex;
    this.IsForceFitLayout = true;
    DocumentLayouter.PageNumber = this.m_nextPageIndex;
    this.m_columnsWidth = 0.0f;
    this.m_columnIndex = 0;
    DocumentLayouter.IsEndPage = false;
  }

  private void ResetFloatingItemsProperties()
  {
    for (int index = 0; index < this.m_FloatingItems.Count; ++index)
    {
      WParagraph ownerParagraph = this.m_FloatingItems[index].OwnerParagraph;
      if (ownerParagraph != null)
      {
        ownerParagraph.IsFloatingItemsLayouted = false;
        Entity floatingEntity = this.m_FloatingItems[index].FloatingEntity;
        switch (floatingEntity)
        {
          case WTextBox _ when floatingEntity.IsFloatingItem(true):
            (floatingEntity as WTextBox).TextBoxFormat.IsWrappingBoundsAdded = false;
            continue;
          case WPicture _ when floatingEntity.IsFloatingItem(true):
            (floatingEntity as WPicture).IsWrappingBoundsAdded = false;
            continue;
          case Shape _ when floatingEntity.IsFloatingItem(true):
            (floatingEntity as Shape).WrapFormat.IsWrappingBoundsAdded = false;
            continue;
          case WChart _ when floatingEntity.IsFloatingItem(true):
            (floatingEntity as WChart).WrapFormat.IsWrappingBoundsAdded = false;
            continue;
          case GroupShape _ when floatingEntity.IsFloatingItem(true):
            (floatingEntity as GroupShape).WrapFormat.IsWrappingBoundsAdded = false;
            continue;
          default:
            continue;
        }
      }
    }
  }

  private void ResetNotAddedFloatingEntityProperty()
  {
    for (int index = 0; index < this.FloatingItems.Count; ++index)
    {
      if (!this.FloatingItems[index].IsFloatingItemFit)
      {
        if (this.FloatingItems[index].FloatingEntity is WTextBox)
          (this.FloatingItems[index].FloatingEntity as WTextBox).TextBoxFormat.IsWrappingBoundsAdded = false;
        else if (this.FloatingItems[index].FloatingEntity is WPicture)
          (this.FloatingItems[index].FloatingEntity as WPicture).IsWrappingBoundsAdded = false;
        else if (this.FloatingItems[index].FloatingEntity is Shape)
          (this.FloatingItems[index].FloatingEntity as Shape).WrapFormat.IsWrappingBoundsAdded = false;
        else if (this.FloatingItems[index].FloatingEntity is GroupShape)
          (this.FloatingItems[index].FloatingEntity as GroupShape).WrapFormat.IsWrappingBoundsAdded = false;
      }
    }
  }

  private void CreateNewSection()
  {
    this.m_columnIndex = 0;
    this.m_columnsWidth = 0.0f;
    this.m_columnHeight.Clear();
    this.m_columnHasBreakItem.Clear();
    this.m_prevColumnsWidth.Clear();
  }

  private bool CheckSectionBreak(bool isFromDynmicLayout, IWidgetContainer curWidget)
  {
    bool flag = false;
    if (this.m_bFirstPageForSection || isFromDynmicLayout && this.CurrentPage.PageWidgets.Count > 2)
    {
      this.m_usedHeight += this.m_sectionFixedHeight;
      switch (this.CurrentSection.BreakCode)
      {
        case SectionBreakCode.NoBreak:
          int num = this.CurrentSection.Document.Sections.IndexOf(this.CurrentSection);
          WSection previousSibling = this.CurrentSection.PreviousSibling as WSection;
          if (this.CurrentSection.Body.ChildEntities.Count > 0)
          {
            if (this.CurrentSection.Body.ChildEntities[0] is WParagraph && (this.CurrentSection.Body.ChildEntities[0] as WParagraph).ParagraphFormat.PageBreakBefore)
            {
              this.m_createNewPage = true;
              this.MaintainltWidget.ChildWidgets.Clear();
            }
            else if (this.CurrentSection.Body.ChildEntities[0] is WTable && (this.CurrentSection.Body.ChildEntities[0] as WTable).Rows[0].Cells[0].Paragraphs.Count > 0 && (this.CurrentSection.Body.ChildEntities[0] as WTable).Rows[0].Cells[0].Paragraphs[0].ParagraphFormat.PageBreakBefore)
            {
              this.m_createNewPage = true;
              this.MaintainltWidget.ChildWidgets.Clear();
            }
          }
          if (this.CurrentSection is WSection && (previousSibling == null || this.CurrentSection.PageSetup.Orientation == previousSibling.PageSetup.Orientation) && (this.CurrentPage.FootnoteWidgets.Count == 0 || this.CurrentSection.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || this.CurrentSection.Document.DOP.Dop2000.Copts.FtnLayoutLikeWW8) && (previousSibling == null || (double) this.CurrentSection.PageSetup.PageSize.Height == (double) previousSibling.PageSetup.PageSize.Height && (double) this.CurrentSection.PageSetup.PageSize.Width == (double) previousSibling.PageSetup.PageSize.Width))
          {
            if (previousSibling != null && previousSibling.Columns.Count <= 1)
            {
              this.m_usedHeight = this.CurrentPage.PageWidgets[this.CurrentPage.PageWidgets.Count - 1].Bounds.Bottom - this.m_pageTop;
              for (int index = 0; index < this.CurrentPage.EndnoteWidgets.Count; ++index)
              {
                if (num - 1 == this.CurrentPage.EndNoteSectionIndex[index])
                  this.m_usedHeight += this.CurrentPage.EndnoteWidgets[index].Bounds.Height;
              }
              this.m_sectionFixedHeight = this.m_usedHeight;
            }
            if (!this.m_createNewPage)
            {
              if (this.CurrentPage.FootnoteWidgets.Count > 0)
                (this.CurrentSection as WSection).IsSectionFitInSamePage = true;
              this.CreateNewSection();
              this.m_bFirstPageForSection = false;
              this.m_createNewPage = false;
              flag = false;
            }
            else
              flag = true;
          }
          else
            flag = true;
          DocumentLayouter.m_footnoteIDRestartEachSection = 1;
          break;
        case SectionBreakCode.NewColumn:
        case SectionBreakCode.NewPage:
          if (!isFromDynmicLayout)
            flag = true;
          this.m_sectionFixedHeight = 0.0f;
          DocumentLayouter.m_footnoteIDRestartEachSection = 1;
          break;
        case SectionBreakCode.EvenPage:
          if (this.IsEvenPage && this.CurrentSection.PageSetup.PageStartingNumber == 0)
            this.CreateNewPage(ref curWidget);
          flag = true;
          this.m_sectionFixedHeight = 0.0f;
          DocumentLayouter.m_footnoteIDRestartEachSection = 1;
          break;
        case SectionBreakCode.Oddpage:
          if (!this.IsEvenPage && this.CurrentSection.PageSetup.PageStartingNumber == 0)
            this.CreateNewPage(ref curWidget);
          flag = true;
          this.m_sectionFixedHeight = 0.0f;
          DocumentLayouter.m_footnoteIDRestartEachSection = 1;
          break;
      }
    }
    return flag;
  }

  private void HandlePageBreak()
  {
    if (this.CurrentPage.PageWidgets.Count <= 2)
      return;
    LayoutedWidget layoutedWidget = this.CurrentPage.PageWidgets[this.CurrentPage.PageWidgets.Count - 1];
    while (layoutedWidget.ChildWidgets.Count != 0)
      layoutedWidget = layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1];
    if (!(layoutedWidget.Widget is Break) || (layoutedWidget.Widget as Break).BreakType != BreakType.PageBreak)
      return;
    this.m_createNewPage = true;
    this.MaintainltWidget.ChildWidgets.Clear();
  }

  private void LayoutHeaderFooter()
  {
    DocumentLayouter.IsLayoutingHeaderFooter = true;
    IWidgetContainer currentHeader = (IWidgetContainer) this.GetCurrentHeader(this.CurrentSection);
    IWidgetContainer currentFooter = (IWidgetContainer) this.GetCurrentFooter();
    Layouter layouter1 = new Layouter();
    layouter1.LeafLayoutAfter += new Layouter.LeafLayoutEventHandler(this.Layouter_LeafLayoutAfter);
    layouter1.Layout(currentHeader, (ILayoutProcessHandler) this.m_headerLPHandler, DocumentLayouter.m_dc);
    layouter1.LeafLayoutAfter -= new Layouter.LeafLayoutEventHandler(this.Layouter_LeafLayoutAfter);
    if ((double) this.CurrentSection.PageSetup.Margins.Top < (double) this.CurrentPage.PageWidgets[0].Bounds.Bottom && this.FloatingItems.Count > 0)
      this.ShiftItemsForVerticalAlignment();
    Layouter layouter2 = new Layouter();
    layouter2.LeafLayoutAfter += new Layouter.LeafLayoutEventHandler(this.Layouter_LeafLayoutAfter);
    layouter2.Layout(currentFooter, (ILayoutProcessHandler) this.m_footerLPHandler, DocumentLayouter.m_dc);
    layouter2.LeafLayoutAfter -= new Layouter.LeafLayoutEventHandler(this.Layouter_LeafLayoutAfter);
    DocumentLayouter.IsLayoutingHeaderFooter = false;
    this.ResetFloatingItemsProperties();
  }

  private void ShiftItemsForVerticalAlignment()
  {
    for (int index1 = 0; index1 < this.FloatingItems.Count; ++index1)
    {
      FloatingItem floatingItem = this.FloatingItems[index1];
      if (floatingItem.FloatingEntity is ParagraphItem)
      {
        VerticalOrigin verticalOrigin = (floatingItem.FloatingEntity as ParagraphItem).GetVerticalOrigin();
        WParagraph ownerParagraph = (floatingItem.FloatingEntity as ParagraphItem).OwnerParagraph;
        if (verticalOrigin == VerticalOrigin.Margin && !ownerParagraph.IsInCell && !ownerParagraph.ParagraphFormat.IsFrame)
        {
          float verticalPosition = (floatingItem.FloatingEntity as ParagraphItem).GetVerticalPosition();
          if ((double) verticalPosition != 0.0)
          {
            float y = verticalPosition + this.CurrentPage.PageWidgets[0].Bounds.Bottom;
            float yOffset = y - floatingItem.TextWrappingBounds.Y;
            floatingItem.TextWrappingBounds = new RectangleF(floatingItem.TextWrappingBounds.X, y, floatingItem.TextWrappingBounds.Width, floatingItem.TextWrappingBounds.Height);
            LayoutedWidget pageWidget = this.CurrentPage.PageWidgets[0];
            for (int index2 = 0; index2 < pageWidget.ChildWidgets.Count; ++index2)
            {
              if (pageWidget.ChildWidgets[index2].Widget is WParagraph)
              {
                foreach (LayoutedWidget childWidget1 in (List<LayoutedWidget>) pageWidget.ChildWidgets[index2].ChildWidgets)
                {
                  foreach (LayoutedWidget childWidget2 in (List<LayoutedWidget>) childWidget1.ChildWidgets)
                  {
                    if (floatingItem.FloatingEntity == childWidget2.Widget)
                    {
                      childWidget2.ShiftLocation(0.0, (double) yOffset, true, false);
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

  internal WTextBody GetCurrentHeader(IWSection section)
  {
    WHeadersFooters headersFooters = section.HeadersFooters;
    WTextBody headerFooter = this.GetHeaderFooter(section, headersFooters.OddHeader);
    if (headersFooters.LinkToPrevious)
    {
      if (!(section.PreviousSibling is WSection))
        return headerFooter;
      WSection previousSibling = section.PreviousSibling as WSection;
      if (section.PageSetup.DifferentOddAndEvenPages && this.IsEvenPage)
        headerFooter = this.GetHeaderFooter((IWSection) previousSibling, previousSibling.HeadersFooters.EvenHeader);
      if (section.PageSetup.DifferentFirstPage && this.m_bFirstPageForSection)
        headerFooter = this.GetHeaderFooter((IWSection) previousSibling, previousSibling.HeadersFooters.FirstPageHeader);
    }
    else
    {
      if (section.PageSetup.DifferentOddAndEvenPages && this.IsEvenPage)
        headerFooter = this.GetHeaderFooter(section, headersFooters.EvenHeader);
      if (section.PageSetup.DifferentFirstPage && this.m_bFirstPageForSection)
        headerFooter = this.GetHeaderFooter(section, headersFooters.FirstPageHeader);
    }
    return headerFooter;
  }

  private WTextBody GetCurrentFooter()
  {
    WHeadersFooters headersFooters = this.CurrentSection.HeadersFooters;
    WTextBody headerFooter = this.GetHeaderFooter(this.CurrentSection, headersFooters.OddFooter);
    if (headersFooters.LinkToPrevious)
    {
      if (!(this.CurrentSection.PreviousSibling is WSection))
        return headerFooter;
      WSection previousSibling = this.CurrentSection.PreviousSibling as WSection;
      if (this.CurrentSection.PageSetup.DifferentOddAndEvenPages && this.IsEvenPage)
        headerFooter = this.GetHeaderFooter((IWSection) previousSibling, previousSibling.HeadersFooters.EvenFooter);
      if (this.CurrentSection.PageSetup.DifferentFirstPage && this.m_bFirstPageForSection)
        headerFooter = this.GetHeaderFooter((IWSection) previousSibling, previousSibling.HeadersFooters.FirstPageFooter);
    }
    else
    {
      if (this.CurrentSection.PageSetup.DifferentOddAndEvenPages && this.IsEvenPage)
        headerFooter = this.GetHeaderFooter(this.CurrentSection, headersFooters.EvenFooter);
      if (this.CurrentSection.PageSetup.DifferentFirstPage && this.m_bFirstPageForSection)
        headerFooter = this.GetHeaderFooter(this.CurrentSection, headersFooters.FirstPageFooter);
    }
    return headerFooter;
  }

  private WTextBody GetHeaderFooter(IWSection section, HeaderFooter headerFooter)
  {
    WTextBody headerFooter1 = (WTextBody) headerFooter;
    if (headerFooter.LinkToPrevious)
    {
      IEntity entity = (IEntity) section;
      if (entity is Entity && (entity as Entity).Index > 0)
      {
        for (int index = (entity as Entity).Index; index > 0; --index)
        {
          WSection section1 = section.Document.Sections[index - 1];
          headerFooter1 = (WTextBody) section1.HeadersFooters[headerFooter.Type];
          if (!section1.HeadersFooters[headerFooter.Type].LinkToPrevious)
            break;
        }
      }
    }
    return headerFooter1;
  }

  private void OnNextSection()
  {
    if (DocumentLayouter.IsFirstLayouting)
      this.m_sectionPagesCount = this.m_currSection.BreakCode == SectionBreakCode.NoBreak ? 1 : 0;
    this.m_bFirstPageForSection = true;
    this.m_sectionNewPage = false;
    this.m_isContinuousSectionLayouted = false;
    this.m_columnIndex = 0;
    this.m_columnsWidth = 0.0f;
    this.m_totalHeight = 0.0f;
  }

  internal Dictionary<Entity, int> GetTOCEntryPageNumbers(WordDocument doc)
  {
    this.m_docWidget = (IWidgetContainer) doc;
    this.m_currSection = (IWSection) doc.Sections[0];
    DocumentLayouter.IsFirstLayouting = true;
    DocumentLayouter.IsUpdatingTOC = true;
    this.LayoutPages();
    if (this.CurrentPage != null && this.CurrentPage.DocSection is WSection)
    {
      WParagraph lastLtParagraph = this.GetLastLtParagraph();
      int index1 = (this.CurrentPage.DocSection as WSection).Index;
      bool isLastTOCEntry = false;
      for (int index2 = 0; index2 <= index1; ++index2)
        doc.Sections[index2].InitLayoutInfo((Entity) lastLtParagraph, ref isLastTOCEntry);
    }
    DocumentLayouter.IsUpdatingTOC = false;
    DocumentLayouter.IsEndUpdateTOC = false;
    this.LastTocEntity = (Entity) null;
    return this.TOCEntryPageNumbers;
  }

  private WParagraph GetLastLtParagraph()
  {
    if (this.CurrentPage.PageWidgets.Count < 3)
      return (WParagraph) null;
    LayoutedWidget layoutedWidget = this.CurrentPage.PageWidgets[this.CurrentPage.PageWidgets.Count - 1];
    while (layoutedWidget != null)
    {
      layoutedWidget = layoutedWidget.ChildWidgets.Count > 0 ? layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1] : (LayoutedWidget) null;
      if (layoutedWidget != null)
      {
        if (layoutedWidget.Widget is WParagraph)
          return layoutedWidget.Widget as WParagraph;
        if (layoutedWidget.Widget is SplitWidgetContainer && (layoutedWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
          return (layoutedWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
      }
      else
        break;
    }
    return (WParagraph) null;
  }

  internal void UpdatePageFields(WordDocument doc, bool isUpdateFromWordToPDF)
  {
    this.m_docWidget = (IWidgetContainer) doc;
    this.m_currSection = (IWSection) doc.Sections[0];
    DocumentLayouter.m_UpdatingPageFields = true;
    DocumentLayouter.IsFirstLayouting = true;
    if (isUpdateFromWordToPDF)
      this.Layout((IWordDocument) doc);
    else
      this.LayoutPages();
    DocumentLayouter.m_UpdatingPageFields = false;
  }

  private static bool IsEMFAzureCompatible()
  {
    try
    {
      DocumentLayouter.CreateImage();
      return false;
    }
    catch
    {
      return true;
    }
  }

  private static void CreateImage()
  {
    int pixels1 = (int) UnitsConvertor.Instance.ConvertToPixels(612f, PrintUnits.Point);
    int pixels2 = (int) UnitsConvertor.Instance.ConvertToPixels(792f, PrintUnits.Point);
    MemoryStream memoryStream = new MemoryStream();
    using (Bitmap bitmap = new Bitmap(pixels1, pixels2))
    {
      using (Graphics graphics = Graphics.FromImage((Image) bitmap))
      {
        IntPtr hdc = graphics.GetHdc();
        RectangleF frameRect = new RectangleF(0.0f, 0.0f, (float) pixels1, (float) pixels2);
        Image image = (Image) new Metafile((Stream) memoryStream, hdc, frameRect, MetafileFrameUnit.Pixel, EmfType.EmfPlusDual);
        graphics.ReleaseHdc(hdc);
        graphics.Dispose();
        image.Dispose();
      }
    }
  }

  internal Image CreateImage(WPageSetup pageSetup, ImageType imageType, MemoryStream stream)
  {
    int pixels1 = (int) UnitsConvertor.Instance.ConvertToPixels(pageSetup.PageSize.Width, PrintUnits.Point);
    int pixels2 = (int) UnitsConvertor.Instance.ConvertToPixels(pageSetup.PageSize.Height, PrintUnits.Point);
    if (DocumentLayouter.IsAzureCompatible)
      imageType = ImageType.Bitmap;
    Image image;
    switch (imageType)
    {
      case ImageType.Metafile:
        if (stream == null)
          stream = new MemoryStream();
        using (Bitmap bitmap = new Bitmap(pixels1, pixels2))
        {
          using (Graphics graphics = Graphics.FromImage((Image) bitmap))
          {
            IntPtr hdc = graphics.GetHdc();
            RectangleF frameRect = new RectangleF(0.0f, 0.0f, (float) pixels1, (float) pixels2);
            image = (Image) new Metafile((Stream) stream, hdc, frameRect, MetafileFrameUnit.Pixel, EmfType.EmfPlusDual);
            graphics.ReleaseHdc(hdc);
            graphics.Dispose();
            break;
          }
        }
      case ImageType.Bitmap:
        image = (Image) new Bitmap(pixels1, pixels2, PixelFormat.Format32bppPArgb);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (imageType));
    }
    return image;
  }

  private WParagraph IsTOCParagraph(IWidget widget)
  {
    bool flag = false;
    WParagraph wparagraph = (WParagraph) null;
    switch (widget)
    {
      case WParagraph _ when !string.IsNullOrEmpty((widget as WParagraph).Text) || (widget as WParagraph).ListFormat.ListType == ListType.Numbered:
      case WParagraph _ when (widget as WParagraph).IsContainsInLineImage():
        wparagraph = widget as WParagraph;
        flag = true;
        break;
      case SplitWidgetContainer _ when (widget as SplitWidgetContainer).RealWidgetContainer is WParagraph:
        wparagraph = (widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
        if (!string.IsNullOrEmpty(wparagraph.Text) || wparagraph.IsContainsInLineImage())
        {
          flag = true;
          break;
        }
        break;
    }
    return !flag ? (WParagraph) null : wparagraph;
  }

  private void Layouter_LeafLayoutAfter(
    object sender,
    LayoutedWidget ltWidget,
    bool isFromTOCLinkStyle)
  {
    if (ltWidget.Widget is WField widget1 && (widget1.FieldType == FieldType.FieldPage || widget1.FieldType == FieldType.FieldNumPages || widget1.FieldType == FieldType.FieldSectionPages || widget1.FieldType == FieldType.FieldTOCEntry || widget1.FieldType == FieldType.FieldPageRef || widget1.FieldType == FieldType.FieldAutoNum))
    {
      if (widget1.FieldType == FieldType.FieldPage)
      {
        string numberFormatValue = this.CurrentSection.PageSetup.GetNumberFormatValue((byte) this.CurrentSection.PageSetup.PageNumberStyle, this.CurrentPage.Number + 1);
        ltWidget.TextTag = numberFormatValue;
        if (DocumentLayouter.m_UpdatingPageFields)
          widget1.UpdateFieldResult(numberFormatValue);
        WCharacterFormat characterFormatValue = widget1.GetCharacterFormatValue();
        SizeF size = DocumentLayouter.DrawingContext.MeasureString(ltWidget.TextTag, characterFormatValue.GetFontToRender(widget1.ScriptType), (StringFormat) null, characterFormatValue, false);
        ltWidget.Bounds = new RectangleF(ltWidget.Bounds.Location, size);
        if (!DocumentLayouter.m_UpdatingPageFields || !widget1.IsSkipFieldResult())
          return;
        widget1.SkipLayoutingFieldItems(false);
      }
      else if (widget1.FieldType == FieldType.FieldAutoNum)
      {
        ltWidget.TextTag = widget1.Text;
        WCharacterFormat characterFormatValue = widget1.GetCharacterFormatValue();
        SizeF size = DocumentLayouter.DrawingContext.MeasureString(ltWidget.TextTag, characterFormatValue.GetFontToRender(widget1.ScriptType), (StringFormat) null, characterFormatValue, false);
        ltWidget.Bounds = new RectangleF(ltWidget.Bounds.Location, size);
      }
      else if (widget1.FieldType == FieldType.FieldSectionPages)
      {
        if (!DocumentLayouter.IsFirstLayouting)
        {
          if (this.CurrentSection is WSection)
            ltWidget.TextTag = this.SectionNumPages[(this.CurrentSection as WSection).Index].ToString();
          WCharacterFormat characterFormatValue = widget1.GetCharacterFormatValue();
          SizeF size = DocumentLayouter.DrawingContext.MeasureString(ltWidget.TextTag, characterFormatValue.GetFontToRender(widget1.ScriptType), (StringFormat) null, characterFormatValue, false);
          ltWidget.Bounds = new RectangleF(ltWidget.Bounds.Location, size);
        }
        else
          this.m_bDirty = true;
      }
      else if (widget1.FieldType == FieldType.FieldNumPages)
      {
        this.CurrentPage.AddCachedFields((IWField) widget1);
        this.m_bDirty = true;
      }
      else if (widget1.FieldType == FieldType.FieldPageRef)
      {
        widget1.FieldResult = (this.CurrentPage.Number + 1).ToString();
      }
      else
      {
        if (widget1.FieldType != FieldType.FieldTOCEntry || DocumentLayouter.IsLayoutingHeaderFooter || !DocumentLayouter.IsUpdatingTOC || !this.UseTCFields)
          return;
        if (this.TOCEntryPageNumbers.Count > 0 && this.TOCEntryPageNumbers.ContainsKey((Entity) widget1))
          this.TOCEntryPageNumbers[(Entity) widget1] = this.CurrentPage.Number + 1;
        else
          this.TOCEntryPageNumbers.Add((Entity) widget1, this.CurrentPage.Number + 1);
        if (widget1 != this.LastTocEntity)
          return;
        DocumentLayouter.IsEndUpdateTOC = true;
      }
    }
    else
    {
      Entity key1;
      if (DocumentLayouter.IsUpdatingTOC && !DocumentLayouter.IsLayoutingHeaderFooter && (WParagraph) (key1 = (Entity) this.IsTOCParagraph(ltWidget.Widget)) != null)
      {
        if (this.TOCEntryPageNumbers.Count > 0 && this.TOCEntryPageNumbers.ContainsKey(key1))
          this.TOCEntryPageNumbers[key1] = this.CurrentPage.Number + 1;
        else
          this.TOCEntryPageNumbers.Add(key1, this.CurrentPage.Number + 1);
        if (key1 != this.LastTocEntity || ltWidget.Widget.LayoutInfo.IsKeepWithNext)
          return;
        DocumentLayouter.IsEndUpdateTOC = true;
      }
      else if (DocumentLayouter.IsUpdatingTOC && !DocumentLayouter.IsLayoutingHeaderFooter && isFromTOCLinkStyle)
      {
        Entity key2 = ltWidget.Widget as Entity;
        if (ltWidget.Widget is SplitStringWidget)
          key2 = (ltWidget.Widget as SplitStringWidget).RealStringWidget as Entity;
        if (this.TOCEntryPageNumbers.Count > 0 && this.TOCEntryPageNumbers.ContainsKey(key2))
          this.TOCEntryPageNumbers[key2] = this.CurrentPage.Number + 1;
        else
          this.TOCEntryPageNumbers.Add(key2, this.CurrentPage.Number + 1);
        if (key2 != this.LastTocEntity)
          return;
        DocumentLayouter.IsEndUpdateTOC = true;
      }
      else
      {
        if (!(ltWidget.Widget is BookmarkStart))
          return;
        BookmarkStart widget = ltWidget.Widget as BookmarkStart;
        WTextBody entityOwnerTextBody = widget.Document.GetEntityOwnerTextBody(widget.GetOwnerParagraphValue());
        if (entityOwnerTextBody is HeaderFooter || entityOwnerTextBody.Owner is WComment)
        {
          if (this.BookmarkStartPageNumbers.Count > 0 && this.BookmarkStartPageNumbers.ContainsKey((Entity) widget))
            this.BookmarkStartPageNumbers[(Entity) widget] = 1;
          else
            this.BookmarkStartPageNumbers.Add((Entity) widget, 1);
        }
        else if (this.BookmarkStartPageNumbers.Count > 0 && this.BookmarkStartPageNumbers.ContainsKey((Entity) widget))
          this.BookmarkStartPageNumbers[(Entity) widget] = this.CurrentPage.Number + 1;
        else
          this.BookmarkStartPageNumbers.Add((Entity) widget, this.CurrentPage.Number + 1);
      }
    }
  }

  bool ILayoutProcessHandler.GetNextArea(
    out RectangleF area,
    ref int columnIndex,
    ref bool isContinuousSection,
    bool isSplittedWidget,
    ref float topMargin,
    bool isFromDynmicLayout,
    ref IWidgetContainer curWidget)
  {
    int count = this.CurrentSection.Columns.Count;
    if (!isSplittedWidget || isFromDynmicLayout)
    {
      if (isFromDynmicLayout)
      {
        LayoutedWidget childWidget = this.MaintainltWidget.ChildWidgets[this.InterSectingPoint[0]].ChildWidgets[this.InterSectingPoint[1]];
        if (this.InterSectingPoint[0] != int.MinValue && childWidget.Widget is WSection)
          this.m_currSection = (IWSection) (childWidget.Widget as WSection);
        if ((double) childWidget.Bounds.X == (double) Layouter.GetLeftMargin(this.m_currSection as WSection))
        {
          this.m_columnIndex = 0;
          this.m_columnsWidth = 0.0f;
        }
        else
        {
          int index = 0;
          int num1 = 0;
          float num2 = 0.0f;
          for (; Math.Round((double) childWidget.Bounds.X) != Math.Round((double) this.MaintainltWidget.ChildWidgets[index].Bounds.X); ++index)
          {
            if ((double) num2 != (double) this.MaintainltWidget.ChildWidgets[index].Bounds.Right)
            {
              num2 = this.MaintainltWidget.ChildWidgets[index].Bounds.Right;
              ++num1;
            }
          }
          this.m_columnIndex = num1;
          Column column = this.m_currSection.Columns.Count > columnIndex ? this.m_currSection.Columns[columnIndex] : (Column) null;
          if (column != null)
            num2 += column.Space;
          this.m_columnsWidth = num2 - Layouter.GetLeftMargin(this.m_currSection as WSection);
        }
      }
      else
      {
        this.m_columnIndex = 0;
        this.m_columnsWidth = 0.0f;
      }
    }
    bool flag1 = count == 0 && this.m_columnIndex > 0 || count > 0 && this.m_columnIndex > count - 1;
    if (this.CurrentSection.Document.Sections.Count > 1)
    {
      bool flag2 = this.CheckSectionBreak(isFromDynmicLayout, curWidget);
      flag1 = flag1 || flag2;
    }
    this.HandlePageBreak();
    this.HandleDynamicRelayouting(isFromDynmicLayout);
    if (flag1 && ((double) this.m_usedHeight == (double) this.m_clientHeight || !isContinuousSection) || this.m_createNewPage || DocumentLayouter.IsEndPage)
    {
      this.CreateNewPage(ref curWidget);
      this.LayoutHeaderFooter();
      this.ClearFields();
    }
    area = this.GetSectionClientArea(isSplittedWidget, curWidget);
    topMargin = this.m_pageTop;
    isContinuousSection = this.CheckNextSectionBreakType(isContinuousSection);
    if (isContinuousSection && !this.m_isContinuousSectionLayouted && this.CurrentSection.Columns.Count > 1 && this.m_columnIndex == 0 && isSplittedWidget && !this.CurrentSection.PageSetup.EqualColumnWidth && !this.IsEqualColumnWidth())
    {
      float num = 0.0f;
      for (int index = 0; index < this.CurrentSection.Columns.Count; ++index)
        num += this.CurrentSection.Columns[index].Width;
      area.Width = num;
      this.m_totalColumnWidth = num;
    }
    if (this.m_isContinuousSectionLayouted)
    {
      if (this.m_columnIndex == 0)
        this.m_sectionFixedHeight = this.GetRequiredHeightForContinuousSection();
      isContinuousSection = false;
      if (((double) this.CurrentPage.PageWidgets[0].Bounds.Height + (double) this.CurrentSection.PageSetup.HeaderDistance > (double) this.CurrentSection.PageSetup.Margins.Top ? (Math.Round((double) area.Y, 2) == Math.Round((double) this.CurrentPage.PageWidgets[0].Bounds.Height + (double) this.CurrentSection.PageSetup.HeaderDistance, 2) ? 1 : 0) : ((double) area.Y == (double) this.CurrentSection.PageSetup.Margins.Top ? 1 : 0)) != 0)
        this.IsForceFitLayout = true;
      if ((double) area.Height > (double) this.m_sectionFixedHeight + (double) this.RemovedWidgetsHeight)
        area.Height = this.m_sectionFixedHeight + this.RemovedWidgetsHeight;
      this.RemovedWidgetsHeight = 0.0f;
      this.m_firstParaHeight = 0.0f;
      this.lineHeigthCount = 0;
      this.m_footnoteHeight = 0.0f;
      this.m_lineHeights.Clear();
    }
    columnIndex = this.m_columnIndex;
    ++this.m_columnIndex;
    return !area.Equals((object) RectangleF.Empty);
  }

  private void HandleDynamicRelayouting(bool isFromDynmicLayout)
  {
    if (!isFromDynmicLayout)
      return;
    LayoutedWidget childWidget1 = this.InterSectingPoint[0] >= this.MaintainltWidget.ChildWidgets.Count || this.InterSectingPoint[1] >= this.MaintainltWidget.ChildWidgets[this.InterSectingPoint[0]].ChildWidgets.Count ? (LayoutedWidget) null : this.MaintainltWidget.ChildWidgets[this.InterSectingPoint[0]].ChildWidgets[this.InterSectingPoint[1]];
    LayoutedWidget childWidget2 = childWidget1?.ChildWidgets[childWidget1.ChildWidgets.Count - 1];
    LayoutedWidget childWidget3 = childWidget1 == null || this.InterSectingPoint[2] >= childWidget1.ChildWidgets.Count ? (LayoutedWidget) null : childWidget1.ChildWidgets[this.InterSectingPoint[2]];
    if (childWidget2 == null || childWidget3 == null)
      return;
    bool isItemIntersected = this.IsIntersectedItem(childWidget3, childWidget2);
    float num1 = childWidget3.Bounds.Y - this.MaintainltWidget.ChildWidgets[0].Bounds.Y;
    float top = this.CurrentPage.Setup.Margins.Top;
    float val2_1 = this.CurrentPage.PageWidgets[0].ChildWidgets.Count == 0 || (double) top < 0.0 ? 0.0f : this.CurrentPage.PageWidgets[0].Bounds.Height + ((double) this.CurrentPage.Setup.HeaderDistance != -0.05000000074505806 ? this.CurrentPage.Setup.HeaderDistance : 36f);
    float val2_2 = this.CurrentPage.PageWidgets[1].ChildWidgets.Count == 0 || (double) this.CurrentPage.Setup.Margins.Bottom < 0.0 ? 0.0f : this.CurrentPage.PageWidgets[1].Bounds.Height + ((double) this.CurrentPage.Setup.FooterDistance != -0.05000000074505806 ? this.CurrentPage.Setup.FooterDistance : 36f);
    if ((double) this.CurrentPage.Setup.Margins.Gutter > 0.0 && this.CurrentPage.Setup.Document.DOP.GutterAtTop)
      top += this.CurrentPage.Setup.Margins.Gutter;
    float num2 = this.CurrentPage.Setup.PageSize.Height - (Math.Max(top, val2_1) + Math.Max(this.CurrentPage.Setup.Margins.Bottom, val2_2)) - num1;
    float intersectingHeight = this.GetIntersectingHeight(childWidget1, childWidget3, isItemIntersected);
    bool flag = (double) childWidget2.Bounds.Height + (double) intersectingHeight <= (double) num2;
    if (!isItemIntersected || flag || (double) childWidget2.Bounds.Width < (double) this.m_currSection.PageSetup.ClientWidth || this.m_notFittedfloatingItems.Count != 0)
      return;
    this.m_createNewPage = true;
  }

  private float GetIntersectingHeight(
    LayoutedWidget layoutedSectionWidget,
    LayoutedWidget intersectingItem,
    bool isItemIntersected)
  {
    float num = 0.0f;
    float intersectingHeight;
    if (intersectingItem.Widget is WTable)
    {
      intersectingHeight = layoutedSectionWidget.ChildWidgets[layoutedSectionWidget.ChildWidgets.Count - 2].Bounds.Bottom - intersectingItem.Bounds.Y;
    }
    else
    {
      LayoutedWidget childWidget = intersectingItem.ChildWidgets[this.InterSectingPoint[3]];
      intersectingHeight = isItemIntersected ? intersectingItem.ChildWidgets[intersectingItem.ChildWidgets.Count - 1].Bounds.Bottom - childWidget.Bounds.Y : num;
      int index = this.InterSectingPoint[2] + 1;
      if (this.InterSectingPoint[2] < layoutedSectionWidget.ChildWidgets.Count - 2 && index < layoutedSectionWidget.ChildWidgets.Count)
        intersectingHeight += layoutedSectionWidget.ChildWidgets[layoutedSectionWidget.ChildWidgets.Count - 2].Bounds.Bottom - layoutedSectionWidget.ChildWidgets[index].Bounds.Y;
    }
    return intersectingHeight;
  }

  private bool IsIntersectedItem(
    LayoutedWidget intersectingItem,
    LayoutedWidget layoutedFloatingItem)
  {
    if (intersectingItem.Widget is WTable)
      return true;
    bool flag = false;
    for (int index = 0; index < intersectingItem.ChildWidgets.Count && !flag; ++index)
      flag = layoutedFloatingItem.Bounds.IntersectsWith(intersectingItem.ChildWidgets[index].Bounds);
    return flag;
  }

  private RectangleF GetSectionClientArea(bool isSplittedWidget, IWidgetContainer curWidget)
  {
    RectangleF sectionClientArea = this.GetColumnClientArea(isSplittedWidget);
    if (this.CurrentSection.BreakCode == SectionBreakCode.NoBreak)
    {
      float firstLineHeight = this.GetFirstLineHeight();
      if ((double) sectionClientArea.Height < (double) firstLineHeight)
      {
        if (this.CurrentSection.Body.ChildEntities.Count > 0 && this.CurrentSection.Body.ChildEntities[0] is WParagraph && (this.IsFirstItemBreakItems(this.CurrentSection.Body.ChildEntities[0] as WParagraph) || this.IsFirstInlineTextWrappingStyleItem(this.CurrentSection.Body.ChildEntities[0] as WParagraph)))
        {
          sectionClientArea = new RectangleF(sectionClientArea.X, sectionClientArea.Y, sectionClientArea.Width, firstLineHeight);
        }
        else
        {
          this.CreateNewPage(ref curWidget);
          this.LayoutHeaderFooter();
          this.ClearFields();
          sectionClientArea = this.GetColumnClientArea(isSplittedWidget);
        }
      }
    }
    return sectionClientArea;
  }

  private bool IsFirstInlineTextWrappingStyleItem(WParagraph paragraph)
  {
    for (int index = 0; index < paragraph.ChildEntities.Count; ++index)
    {
      Entity childEntity = paragraph.ChildEntities[index];
      if (!(childEntity is BookmarkStart) && !(childEntity is BookmarkEnd) && !(childEntity as ParagraphItem).ParaItemCharFormat.Hidden)
      {
        switch (childEntity)
        {
          case WPicture _:
          case WTextBox _:
          case Shape _:
          case GroupShape _:
          case WChart _:
            if ((childEntity as ParagraphItem).GetTextWrappingStyle() == TextWrappingStyle.Inline)
              return true;
            continue;
          default:
            continue;
        }
      }
    }
    return false;
  }

  private bool IsFirstItemBreakItems(WParagraph paragraph)
  {
    for (int index = 0; index < paragraph.ChildEntities.Count; ++index)
    {
      Entity childEntity = paragraph.ChildEntities[index];
      if (!(childEntity is BookmarkStart) && !(childEntity is BookmarkEnd) && !(childEntity as ParagraphItem).ParaItemCharFormat.Hidden)
        return childEntity is Break && (childEntity as Break).BreakType != BreakType.LineBreak;
    }
    return true;
  }

  private RectangleF GetColumnClientArea(bool isSplittedWidget)
  {
    RectangleF empty = RectangleF.Empty;
    RectangleF columnClientArea;
    if (this.CurrentSection.BreakCode == SectionBreakCode.NoBreak && !this.m_isFirstPage)
    {
      if (this.m_sectionNewPage)
      {
        columnClientArea = this.CurrentPage.GetColumnArea(this.m_columnIndex, ref this.m_columnsWidth, this.m_isNeedtoAdjustFooter);
        this.m_pageTop = columnClientArea.Y;
        this.m_clientHeight = columnClientArea.Height;
      }
      else
        columnClientArea = this.CurrentPage.GetSectionArea(this.m_columnIndex, ref this.m_columnsWidth, this.m_isContinuousSectionLayouted, isSplittedWidget);
      columnClientArea.Y = this.m_pageTop + this.m_usedHeight;
      columnClientArea.Height = this.m_clientHeight - this.m_usedHeight;
    }
    else
    {
      columnClientArea = this.CurrentPage.GetColumnArea(this.m_columnIndex, ref this.m_columnsWidth, this.m_isNeedtoAdjustFooter);
      this.m_pageTop = columnClientArea.Y;
      this.m_sectionNewPage = false;
      this.m_isFirstPage = false;
      this.m_clientHeight = columnClientArea.Height;
      DocumentLayouter.IsEndPage = false;
    }
    this.m_isNeedtoAdjustFooter = false;
    int num = this.CurrentSection.Document.Sections.IndexOf(this.CurrentSection);
    for (int index = 0; index < this.CurrentPage.EndnoteWidgets.Count; ++index)
    {
      if (num - 1 == this.CurrentPage.EndNoteSectionIndex[index])
        columnClientArea.Y = this.CurrentPage.EndnoteWidgets[index].Bounds.Bottom;
    }
    return columnClientArea;
  }

  private void ClearFields()
  {
    this.m_columnHeight.Clear();
    this.m_lineHeights.Clear();
    this.m_columnHasBreakItem.Clear();
    this.m_prevColumnsWidth.Clear();
    this.m_totalHeight = 0.0f;
    this.m_usedHeight = 0.0f;
    this.m_sectionFixedHeight = 0.0f;
    this.m_sectionNewPage = true;
    this.m_createNewPage = false;
    this.m_isContinuousSectionLayouted = false;
    this.m_bFirstPageForSection = false;
    this.RemovedWidgetsHeight = 0.0f;
    this.m_footnoteHeight = 0.0f;
    this.m_firstParaHeight = 0.0f;
    this.lineHeigthCount = 0;
  }

  private float GetRequiredHeightForContinuousSection()
  {
    this.FloatingItems = this.m_prevFloatingItems;
    float floatingItemHeight = this.GetFloatingItemHeight(this.FloatingItems);
    this.ResetFloatingItemsProperties();
    this.m_prevFloatingItems.Clear();
    int count = this.CurrentSection.Columns.Count;
    float num = 0.0f;
    if (this.CurrentSection.PageSetup.EqualColumnWidth || this.IsEqualColumnWidth())
    {
      foreach (float lineHeight in this.m_lineHeights)
      {
        num += lineHeight;
        if ((double) num >= ((double) this.m_totalHeight + (double) floatingItemHeight) / (double) (count - this.m_columnIndex))
          break;
      }
    }
    else
      num = this.GetRequiredHeightForUnEqualColumns();
    if (this.m_absolutePositionedTableHeights != null)
    {
      foreach (float positionedTableHeight in this.m_absolutePositionedTableHeights)
        num += positionedTableHeight;
      this.m_absolutePositionedTableHeights.Clear();
    }
    return (double) this.m_firstParaHeight > (double) num && this.lineHeigthCount <= 3 && (double) this.m_totalHeight > (double) num ? (float) Math.Ceiling((double) this.m_totalHeight + (double) this.m_footnoteHeight) : (float) Math.Ceiling((double) num + (double) this.m_footnoteHeight);
  }

  private float GetFloatingItemHeight(List<FloatingItem> floatingItems)
  {
    foreach (FloatingItem floatingItem in floatingItems)
    {
      if (floatingItem.TextWrappingStyle != TextWrappingStyle.InFrontOfText && floatingItem.TextWrappingStyle != TextWrappingStyle.Behind && (floatingItem.TextWrappingStyle == TextWrappingStyle.TopAndBottom || (double) floatingItem.TextWrappingBounds.Width >= (double) this.CurrentSection.Columns[0].Width))
        return floatingItem.TextWrappingBounds.Height;
    }
    return 0.0f;
  }

  private float GetRequiredHeightForUnEqualColumns()
  {
    int forMinColumnWidth = this.GetColumnIndexForMinColumnWidth();
    int forMaxColumnWidth = this.GetColumnIndexForMaxColumnWidth();
    float firstLineHeight = this.GetFirstLineHeight();
    float num1 = this.CurrentSection.Columns[forMaxColumnWidth].Width - this.CurrentSection.Columns[forMinColumnWidth].Width;
    if ((double) num1 < (double) this.CurrentSection.Columns[forMinColumnWidth].Width)
    {
      if (this.CurrentSection.PageSetup.Orientation == PageOrientation.Landscape)
        return this.m_totalColumnWidth * this.m_totalHeight / this.CurrentSection.Columns[forMaxColumnWidth].Width - this.m_totalHeight;
      float num2 = this.m_totalColumnWidth * this.m_totalHeight / this.CurrentSection.Columns[forMinColumnWidth].Width - this.m_totalHeight;
      float num3 = 0.0f;
      for (int index = 0; index < this.CurrentSection.Columns.Count; ++index)
        num3 += (float) (2 * this.CurrentSection.Columns.Count - 1) * Math.Abs(this.CurrentSection.Columns[index].Width - this.CurrentSection.Columns[forMinColumnWidth].Width);
      return Math.Abs(num2 - (float) (this.CurrentSection.Columns.Count - 1) * num3);
    }
    float num4 = forMaxColumnWidth >= forMinColumnWidth ? (float) ((double) num1 * (double) this.m_totalHeight / (double) this.m_totalColumnWidth + (double) num1 * (double) this.CurrentSection.Columns[forMinColumnWidth].Width / ((double) this.CurrentSection.Columns.Count * (double) this.CurrentSection.Columns[forMaxColumnWidth].Width)) : (float) ((double) num1 * (double) this.m_totalHeight / (double) this.m_totalColumnWidth + (double) this.CurrentSection.Columns.Count * (double) this.CurrentSection.Columns[forMaxColumnWidth].Width / ((double) num1 * (double) this.CurrentSection.Columns[forMinColumnWidth].Width));
    string[] strArray = (this.CurrentSection.Columns[forMaxColumnWidth].Width / this.CurrentSection.Columns[forMinColumnWidth].Width).ToString((IFormatProvider) CultureInfo.InvariantCulture).Split('.');
    float num5 = 0.0f;
    if (strArray.Length > 1)
      num5 = (float) Convert.ToDouble(strArray[1]) * this.m_totalHeight / ((float) this.CurrentSection.Columns.Count * (float) Math.Pow(10.0, (double) strArray[1].Length));
    if (forMaxColumnWidth < forMinColumnWidth)
      return num4 + num5 + firstLineHeight;
    float num6 = num4 - num5;
    if (this.CurrentSection.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && this.CurrentSection.Body.Items.FirstItem is WParagraph && (this.CurrentSection.Body.Items.FirstItem as WParagraph).ParagraphFormat.HasValue(8))
    {
      float num7 = firstLineHeight + (this.CurrentSection.Body.Items.FirstItem as WParagraph).ParagraphFormat.BeforeSpacing;
      return (double) num7 > (double) num6 ? num7 - num6 : num6 + num7;
    }
    return (double) num6 > (double) firstLineHeight ? num6 - firstLineHeight : firstLineHeight;
  }

  private float GetFirstLineHeight()
  {
    float firstLineHeight = 0.0f;
    if (this.CurrentSection.Body.Items.FirstItem is WParagraph)
    {
      WParagraph firstItem = this.CurrentSection.Body.Items.FirstItem as WParagraph;
      firstLineHeight = firstItem.GetHeight(firstItem, firstItem.ChildEntities.Count > 0 ? firstItem.ChildEntities[0] as ParagraphItem : (ParagraphItem) null);
    }
    else if (this.CurrentSection.Body.Items.FirstItem is WTable)
    {
      WTable firstItem = this.CurrentSection.Body.Items.FirstItem as WTable;
      if (firstItem.Rows[0].Cells[0].LastParagraph != null)
      {
        WParagraph lastParagraph = firstItem.Rows[0].Cells[0].LastParagraph as WParagraph;
        firstLineHeight = lastParagraph.GetHeight(lastParagraph, lastParagraph.ChildEntities.Count > 0 ? lastParagraph.ChildEntities[0] as ParagraphItem : (ParagraphItem) null);
      }
      float num = (double) firstItem.Rows[0].Height >= 0.0 ? firstItem.Rows[0].Height : -1f * firstItem.Rows[0].Height;
      if (firstItem.Rows[0].HeightType == TableRowHeightType.Exactly && (double) num < (double) firstLineHeight)
        firstLineHeight = num;
      else if (firstItem.Rows[0].HeightType == TableRowHeightType.AtLeast && (double) num > (double) firstLineHeight)
        firstLineHeight = num;
    }
    return firstLineHeight;
  }

  private bool IsEqualColumnWidth()
  {
    float width = this.CurrentSection.Columns[0].Width;
    bool flag = true;
    foreach (Column column in (CollectionImpl) this.CurrentSection.Columns)
    {
      if ((double) width != (double) column.Width)
      {
        flag = false;
        break;
      }
      width = column.Width;
    }
    return flag;
  }

  private bool CheckNextSectionBreakType(bool isContinuousSection)
  {
    int num = this.CurrentSection.Document.Sections.IndexOf(this.CurrentSection);
    if (this.CurrentSection.Document.Sections.Count - 1 > num)
    {
      if (this.CurrentSection.Document.Sections[num + 1].BreakCode == SectionBreakCode.NoBreak && this.CurrentSection.Columns.Count > 1)
      {
        isContinuousSection = true;
        this.m_prevFloatingItems = this.FloatingItems;
        if (this.m_columnIndex == 0 && this.m_sectionIndex != num)
        {
          this.m_createNewPage = false;
          this.m_isContinuousSectionLayouted = false;
          this.m_sectionIndex = num;
        }
      }
      else
      {
        isContinuousSection = false;
        this.m_isContinuousSectionLayouted = false;
      }
    }
    else
    {
      isContinuousSection = false;
      this.m_isContinuousSectionLayouted = false;
    }
    return isContinuousSection;
  }

  void ILayoutProcessHandler.PushLayoutedWidget(
    LayoutedWidget ltWidget,
    RectangleF layoutArea,
    bool isNeedToRestartFootnote,
    bool isNeedToRestartEndnote,
    LayoutState state,
    bool isNeedToFindInterSectingPoint)
  {
    if (this.CurrentPage.FootnoteWidgets.Count > 0)
    {
      if (this.CurrentSection.PageSetup.FootnotePosition == FootnotePosition.PrintImmediatelyBeneathText)
        this.FootnotePushLayoutedWidget(ltWidget.Bounds);
      else
        this.FootnotePushLayoutedWidget(layoutArea);
    }
    if (this.CurrentPage.EndnoteWidgets.Count > 0)
      this.EndnotePushLayoutedWidget(ltWidget.Bounds, ltWidget);
    this.m_bisNeedToRestartFootnote = isNeedToRestartFootnote;
    this.m_bisNeedToRestartEndnote = isNeedToRestartFootnote;
    this.CurrentPage.PageWidgets.Add(ltWidget);
    if (state != LayoutState.DynamicRelayout || !isNeedToFindInterSectingPoint)
      return;
    this.FindIntersectPointAndRemovltWidget(ltWidget);
  }

  private void FindIntersectPointAndRemovltWidget(LayoutedWidget ltwidget)
  {
    while (!(ltwidget.ChildWidgets[ltwidget.ChildWidgets.Count - 1].Widget is ParagraphItem) && !(ltwidget.ChildWidgets[ltwidget.ChildWidgets.Count - 1].Widget is SplitStringWidget) && !(ltwidget.ChildWidgets[ltwidget.ChildWidgets.Count - 1].Widget is WTable))
    {
      int index = ltwidget.ChildWidgets.Count - 1;
      if (ltwidget.ChildWidgets.Count > 1 && ltwidget.ChildWidgets[index].ChildWidgets.Count == 0 && ltwidget.ChildWidgets[index].Widget is SplitWidgetContainer)
      {
        ltwidget.ChildWidgets.RemoveAt(index);
        --index;
      }
      ltwidget = ltwidget.ChildWidgets[index];
      if (ltwidget.ChildWidgets.Count <= 0)
        break;
    }
    for (int index = 2; index < this.CurrentPage.PageWidgets.Count; ++index)
      this.MaintainltWidget.ChildWidgets.Add(new LayoutedWidget(this.CurrentPage.PageWidgets[index]));
    if (ltwidget.ChildWidgets[ltwidget.ChildWidgets.Count - 1].Widget is WTable)
      this.m_dynamicTable = ltwidget.ChildWidgets[ltwidget.ChildWidgets.Count - 1].Widget as WTable;
    float y = ltwidget.ChildWidgets[ltwidget.ChildWidgets.Count - 1].Bounds.Y;
    float bottom = ltwidget.ChildWidgets[ltwidget.ChildWidgets.Count - 1].Bounds.Bottom;
    float num = ltwidget.ChildWidgets[ltwidget.ChildWidgets.Count - 1].Bounds.Right;
    if (ltwidget.Widget is WParagraph && (ltwidget.Widget as WParagraph).ParagraphFormat != null && (ltwidget.Widget as WParagraph).ParagraphFormat.IsFrame)
      num = ltwidget.Bounds.X + (ltwidget.Widget as WParagraph).ParagraphFormat.FrameWidth;
    int index1 = 0;
    for (int index2 = this.CurrentPage.PageWidgets.Count - 1; index2 >= 2; --index2)
    {
      if (index1 == 0 && (double) bottom >= (double) this.CurrentPage.PageWidgets[index2].Bounds.Y && (double) num > (double) this.CurrentPage.PageWidgets[index2].Bounds.X)
      {
        if (this.CurrentSection.Columns.Count <= 1)
        {
          this.m_usedHeight -= this.CurrentPage.PageWidgets[this.CurrentPage.PageWidgets.Count - 1].Bounds.Bottom - y;
          this.m_sectionFixedHeight = 0.0f;
        }
        this.InterSectingPoint[0] = index2 - 2;
        for (int index3 = index2; index3 < this.CurrentPage.PageWidgets.Count; ++index3)
          this.CurrentPage.PageWidgets[index3].Widget.InitLayoutInfo();
        ltwidget = this.CurrentPage.PageWidgets[index2];
        this.CurrentPage.PageWidgets.RemoveRange(index2, this.CurrentPage.PageWidgets.Count - index2);
        ++index1;
        break;
      }
    }
    if (this.InterSectingPoint[0] != int.MinValue)
    {
      for (int index4 = ltwidget.ChildWidgets.Count - 1; index4 >= 0; --index4)
      {
        if (this.m_dynamicTable != ltwidget.ChildWidgets[index4].Widget && !this.IsFloatingTextBodyItem(ltwidget.ChildWidgets[index4].Widget) && (double) y >= (double) ltwidget.ChildWidgets[index4].Bounds.Bottom && (double) num > (double) ltwidget.ChildWidgets[index4].Bounds.X && index1 < 4)
        {
          this.InterSectingPoint[index1] = index4;
          ltwidget = ltwidget.ChildWidgets[index4];
          index4 = ltwidget.ChildWidgets.Count;
          if (!(ltwidget.Widget is WTable) && (ltwidget.ChildWidgets.Count <= 0 || !(ltwidget.ChildWidgets[0].Widget is ParagraphItem) && !(ltwidget.ChildWidgets[0].Widget is SplitStringWidget)))
            ++index1;
          else
            break;
        }
      }
    }
    for (int index5 = 0; index5 < 4; ++index5)
    {
      if (this.InterSectingPoint[index5] < 0)
        this.InterSectingPoint[index5] = 0;
    }
    if (this.m_dynamicTable == null)
    {
      ltwidget = this.MaintainltWidget;
      while (!(ltwidget.Widget is WParagraph) && (!(ltwidget.Widget is SplitWidgetContainer) || !((ltwidget.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)))
        ltwidget = ltwidget.ChildWidgets[ltwidget.ChildWidgets.Count - 1];
      if (index1 == 0)
      {
        this.InterSectingPoint[0] = this.InterSectingPoint[1] = this.InterSectingPoint[2] = this.InterSectingPoint[3] = 0;
        this.m_dynamicParagraph = ltwidget.Widget as WParagraph;
        this.m_usedHeight -= this.CurrentPage.PageWidgets[this.CurrentPage.PageWidgets.Count - 1].Bounds.Bottom - y;
        this.CurrentPage.PageWidgets[2].Widget.InitLayoutInfo();
        this.CurrentPage.PageWidgets.RemoveAt(2);
      }
      else
        this.m_dynamicParagraph = ltwidget.Widget as WParagraph;
    }
    if (this.FloatingItems.Count > 0)
    {
      for (int index6 = 0; index6 < this.FloatingItems.Count - 1; ++index6)
      {
        FloatingItem floatingItem = this.FloatingItems[index6];
        if (floatingItem.FloatingEntity is ParagraphItem)
        {
          ParagraphItem floatingEntity = floatingItem.FloatingEntity as ParagraphItem;
          if ((floatingItem.FloatingEntity as ParagraphItem).GetVerticalOrigin() == VerticalOrigin.Paragraph && floatingEntity.IsWrappingBoundsAdded() && (double) floatingEntity.GetVerticalPosition() >= 0.0)
          {
            floatingEntity.SetIsWrappingBoundsAdded(false);
            floatingEntity.OwnerParagraph.IsFloatingItemsLayouted = false;
            this.FloatingItems.RemoveAt(index6);
            --index6;
          }
        }
      }
    }
    if (this.CurrentPage.TrackChangesMarkups.Count > 0)
    {
      for (int index7 = 0; index7 < this.CurrentPage.TrackChangesMarkups.Count; ++index7)
      {
        TrackChangesMarkups trackChangesMarkup = this.CurrentPage.TrackChangesMarkups[index7];
        if (trackChangesMarkup is CommentsMarkups)
        {
          CommentsMarkups commentsMarkups = trackChangesMarkup as CommentsMarkups;
          if (commentsMarkups.Comment.OwnerParagraph == null || !commentsMarkups.Comment.OwnerParagraph.IsInCell || !(commentsMarkups.Comment.OwnerParagraph.GetOwnerEntity() is WTableCell ownerEntity) || ownerEntity.OwnerRow == null || ownerEntity.OwnerRow.OwnerTable == null || !ownerEntity.OwnerRow.OwnerTable.TableFormat.WrapTextAround)
          {
            this.CurrentPage.TrackChangesMarkups.RemoveAt(index7);
            --index7;
          }
        }
      }
    }
    if (this.CurrentPage.PageWidgets.Count != 2 || this.InterSectingPoint[1] != 0)
      return;
    this.IsForceFitLayout = true;
  }

  private bool IsFloatingTextBodyItem(IWidget widget)
  {
    return widget is WTable && (widget as WTable).TableFormat.WrapTextAround || widget is WParagraph && (widget as WParagraph).ParagraphFormat.IsFrame && (widget as WParagraph).ParagraphFormat.WrapFrameAround != FrameWrapMode.None;
  }

  bool ILayoutProcessHandler.HandleSplittedWidget(
    SplitWidgetContainer stWidgetContainer,
    LayoutState state,
    LayoutedWidget ltWidget,
    ref bool isLayoutedWidgetNeedToPushed)
  {
    if (stWidgetContainer == null)
      throw new ArgumentNullException(nameof (stWidgetContainer));
    IWidget widget = stWidgetContainer.Count >= 1 ? stWidgetContainer[0] : throw new DLSException("Split widget container (document) must contains at last one child element!");
    switch (widget)
    {
      case SplitWidgetContainer _:
        pattern_0 = (widget as SplitWidgetContainer).m_currentChild as IWSection;
        break;
    }
    bool isContinueNextSection = this.IsContinueLayoutingNextSection(pattern_0, ltWidget, isLayoutedWidgetNeedToPushed);
    if (!isContinueNextSection && pattern_0 != null && isLayoutedWidgetNeedToPushed)
      return false;
    if (pattern_0 == null)
    {
      for (SplitWidgetContainer splitWidgetContainer = widget as SplitWidgetContainer; splitWidgetContainer != null; splitWidgetContainer = splitWidgetContainer.m_currentChild as SplitWidgetContainer)
      {
        if (splitWidgetContainer.RealWidgetContainer is WSection)
        {
          pattern_0 = splitWidgetContainer.RealWidgetContainer as IWSection;
          break;
        }
      }
    }
    isLayoutedWidgetNeedToPushed = this.HandleColumnAndPageBreakInLayoutedWidget(ltWidget, isLayoutedWidgetNeedToPushed, isContinueNextSection);
    if (pattern_0 == null)
      throw new DLSException("Child of SplitWidgetContainer object can't support ISecton interface!");
    if (this.m_currSection != pattern_0)
    {
      if (DocumentLayouter.IsFirstLayouting && this.CurrentSection is WSection)
        this.SectionNumPages[(this.CurrentSection as WSection).Index] = this.m_sectionPagesCount;
      this.m_currSection = pattern_0;
      this.OnNextSection();
    }
    return true;
  }

  private bool IsContinueLayoutingNextSection(
    IWSection nextSection,
    LayoutedWidget ltWidget,
    bool isLayoutedWidgetNeedToPushed)
  {
    bool flag = false;
    if (nextSection != null && isLayoutedWidgetNeedToPushed)
    {
      isLayoutedWidgetNeedToPushed = false;
      this.m_isContinuousSectionLayouted = true;
      float columnsWidth = this.m_columnsWidth;
      this.m_columnIndex = 0;
      this.m_columnsWidth = 0.0f;
      for (int index = 0; index < this.m_columnHasBreakItem.Count; ++index)
      {
        if (this.m_columnHasBreakItem[index])
        {
          this.m_columnIndex = index + 1;
          this.m_columnsWidth = this.m_prevColumnsWidth[index];
          isLayoutedWidgetNeedToPushed = true;
          flag = true;
        }
      }
      if (flag)
      {
        this.m_columnHeight.Insert(this.m_columnHeight.Count, ltWidget.Bounds.Height);
        this.m_columnHasBreakItem.Insert(this.m_columnHasBreakItem.Count, false);
        this.m_prevColumnsWidth.Insert(this.m_prevColumnsWidth.Count, columnsWidth);
        this.UpdateSectionHeight(false);
      }
    }
    else if ((this.m_columnIndex == this.CurrentSection.Columns.Count || DocumentLayouter.IsEndPage) && isLayoutedWidgetNeedToPushed)
    {
      isLayoutedWidgetNeedToPushed = true;
      flag = true;
      float columnsWidth = this.m_columnsWidth;
      this.m_columnHeight.Insert(this.m_columnHeight.Count, ltWidget.Bounds.Height);
      this.m_columnHasBreakItem.Insert(this.m_columnHasBreakItem.Count, false);
      this.m_prevColumnsWidth.Insert(this.m_prevColumnsWidth.Count, columnsWidth);
      this.UpdateSectionHeight(true);
    }
    return flag;
  }

  private bool HandleColumnAndPageBreakInLayoutedWidget(
    LayoutedWidget ltWidget,
    bool isLayoutedWidgetNeedToPushed,
    bool isContinueNextSection)
  {
    if (this.CurrentPage.PageWidgets.Count >= 2 && isLayoutedWidgetNeedToPushed && !this.m_isContinuousSectionLayouted)
    {
      isLayoutedWidgetNeedToPushed = false;
      LayoutedWidget layoutedWidget = ltWidget;
      while (layoutedWidget.ChildWidgets.Count != 0)
        layoutedWidget = layoutedWidget.ChildWidgets[layoutedWidget.ChildWidgets.Count - 1];
      if (layoutedWidget.Widget is Break)
      {
        if ((layoutedWidget.Widget as Break).BreakType == BreakType.PageBreak || (layoutedWidget.Widget as Break).BreakType == BreakType.ColumnBreak)
        {
          isLayoutedWidgetNeedToPushed = true;
          this.m_columnHeight.Insert(this.m_columnIndex - 1, ltWidget.Bounds.Height);
          this.m_columnHasBreakItem.Insert(this.m_columnIndex - 1, true);
          this.m_prevColumnsWidth.Insert(this.m_columnIndex - 1, this.m_columnsWidth);
          if ((layoutedWidget.Widget as Break).BreakType == BreakType.PageBreak)
          {
            this.m_createNewPage = true;
            this.MaintainltWidget.ChildWidgets.Clear();
          }
        }
      }
      else
      {
        this.m_columnHeight.Insert(this.m_columnIndex - 1, ltWidget.Bounds.Height);
        this.m_columnHasBreakItem.Insert(this.m_columnIndex - 1, false);
        this.m_prevColumnsWidth.Insert(this.m_columnIndex - 1, this.m_columnsWidth);
      }
    }
    else if (!isContinueNextSection)
      isLayoutedWidgetNeedToPushed = false;
    if (this.CurrentSection.Columns.Count > 1 && (this.m_columnIndex == this.CurrentSection.Columns.Count || DocumentLayouter.IsEndPage))
      isLayoutedWidgetNeedToPushed = true;
    return isLayoutedWidgetNeedToPushed;
  }

  private void UpdateSectionHeight(bool isLastcolumOfCurrentPage)
  {
    this.m_sectionFixedHeight = 0.0f;
    foreach (float val2 in this.m_columnHeight)
      this.m_sectionFixedHeight = Math.Max(this.m_sectionFixedHeight, Math.Min(this.m_clientHeight, val2));
    if ((double) this.m_usedHeight + (double) this.m_sectionFixedHeight + 10.0 < (double) this.m_clientHeight && !isLastcolumOfCurrentPage)
      return;
    this.m_createNewPage = true;
    this.MaintainltWidget.ChildWidgets.Clear();
  }

  void ILayoutProcessHandler.HandleLayoutedWidget(LayoutedWidget ltWidget)
  {
    this.m_totalHeight += ltWidget.Bounds.Height;
    for (int index = 0; index < ltWidget.ChildWidgets.Count; ++index)
      this.GetLinesHeight(ltWidget.ChildWidgets[index]);
  }

  private void GetLinesHeight(LayoutedWidget ltWidget)
  {
    for (int index1 = 0; index1 < ltWidget.ChildWidgets.Count; ++index1)
    {
      LayoutedWidget childWidget1 = ltWidget.ChildWidgets[index1];
      if (childWidget1.Widget is WParagraph || index1 == 0 && childWidget1.Widget is SplitWidgetContainer && (childWidget1.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph || childWidget1.Widget is WTable)
      {
        if (childWidget1.Widget is WTable && (childWidget1.Widget as WTable).TableFormat.WrapTextAround)
        {
          float num = 0.0f;
          if (this.m_absolutePositionedTableHeights == null)
            this.m_absolutePositionedTableHeights = new List<float>();
          if ((childWidget1.Widget as WTable).TableFormat.Positioning.VertRelationTo == VerticalRelation.Paragraph)
            num = (childWidget1.Widget as WTable).TableFormat.Positioning.VertPosition;
          this.m_absolutePositionedTableHeights.Add(childWidget1.Bounds.Height + num);
        }
        else
        {
          for (int index2 = 0; index2 < childWidget1.ChildWidgets.Count; ++index2)
          {
            IWidget widget1 = childWidget1.Widget is SplitWidgetContainer ? (IWidget) (childWidget1.Widget as SplitWidgetContainer).RealWidgetContainer : childWidget1.Widget;
            float height = childWidget1.ChildWidgets[index2].Bounds.Height;
            if (widget1 is WParagraph)
            {
              bool widowControl = (widget1 as WParagraph).ParagraphFormat.WidowControl;
              if ((double) height == 0.0 && (widget1 as WParagraph).SectionEndMark && (widget1 as WParagraph).IsEmptyParagraph())
                height = widget1.LayoutInfo.Size.Height;
              if (widget1.LayoutInfo is ParagraphLayoutInfo)
              {
                if (index2 == 0)
                  height += (widget1.LayoutInfo as ParagraphLayoutInfo).Margins.Top;
                if (index2 == childWidget1.ChildWidgets.Count - 1 && !(widget1 as WParagraph).IsEndOfSection)
                  height += (widget1.LayoutInfo as ParagraphLayoutInfo).Margins.Bottom;
              }
              if (index1 == 0 && widowControl)
              {
                ++this.lineHeigthCount;
                this.m_firstParaHeight += height;
              }
              foreach (LayoutedWidget childWidget2 in (List<LayoutedWidget>) childWidget1.ChildWidgets[index2].ChildWidgets)
              {
                if (childWidget2.Widget is WFootnote)
                  this.m_footnoteHeight += ((FootnoteLayoutInfo) childWidget2.Widget.LayoutInfo).FootnoteHeight;
              }
            }
            else if (childWidget1.Widget is WTable && childWidget1.ChildWidgets[index2].Widget is WTableRow widget2 && widget2.m_layoutInfo is RowLayoutInfo)
              height += (widget2.m_layoutInfo as RowLayoutInfo).Paddings.Bottom;
            this.m_lineHeights.Insert(this.m_lineHeights.Count, height);
          }
        }
      }
      else if (childWidget1.Widget is BlockContentControl || childWidget1.Widget is SplitWidgetContainer && (childWidget1.Widget as SplitWidgetContainer).RealWidgetContainer is BlockContentControl)
        this.GetLinesHeight(childWidget1);
    }
  }

  private int GetColumnIndexForMinColumnWidth()
  {
    int forMinColumnWidth = 0;
    float width = this.CurrentSection.Columns[0].Width;
    for (int index = 1; index < this.CurrentSection.Columns.Count; ++index)
    {
      if ((double) this.CurrentSection.Columns[index].Width < (double) width)
      {
        forMinColumnWidth = index;
        width = this.CurrentSection.Columns[index].Width;
      }
    }
    return forMinColumnWidth;
  }

  private int GetColumnIndexForMaxColumnWidth()
  {
    int forMaxColumnWidth = 0;
    float width = this.CurrentSection.Columns[0].Width;
    for (int index = 1; index < this.CurrentSection.Columns.Count; ++index)
    {
      if ((double) this.CurrentSection.Columns[index].Width > (double) width)
      {
        forMaxColumnWidth = index;
        width = this.CurrentSection.Columns[index].Width;
      }
    }
    return forMaxColumnWidth;
  }

  internal bool HeaderGetNextArea(out RectangleF area)
  {
    area = this.CurrentPage.PageWidgets.Count != 0 ? RectangleF.Empty : this.CurrentPage.GetHeaderArea();
    return !area.Equals((object) RectangleF.Empty);
  }

  internal void HeaderPushLayoutedWidget(LayoutedWidget ltWidget)
  {
    this.CurrentPage.PageWidgets.Add(ltWidget);
  }

  internal void FootnotePushLayoutedWidget(RectangleF layoutArea)
  {
    float num = this.CurrentPage.FootnoteWidgets[this.CurrentPage.FootnoteWidgets.Count - 1].Bounds.Bottom - layoutArea.Y;
    if (this.CurrentSection.PageSetup.FootnotePosition == FootnotePosition.PrintImmediatelyBeneathText)
      num = 0.0f;
    for (int footnoteCount = this.m_footnoteCount; footnoteCount < this.CurrentPage.FootnoteWidgets.Count; ++footnoteCount)
      this.CurrentPage.FootnoteWidgets[footnoteCount].ShiftLocation(0.0, (double) layoutArea.Height - (double) num, true, false);
    if (this.m_currSection.Columns.Count > 1)
      this.m_footnoteCount = this.m_currPage.FootnoteWidgets.Count;
    if (this.m_columnIndex != this.m_currSection.Columns.Count && this.m_currSection.Columns.Count != 1)
      return;
    this.m_footnoteCount = 0;
  }

  internal void EndnotePushLayoutedWidget(RectangleF layoutArea, LayoutedWidget ltWidget)
  {
    Entity entity = ltWidget.Widget as Entity;
    if (ltWidget.Widget is SplitWidgetContainer)
      entity = (ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer as Entity;
    float num1 = 0.0f;
    WSection section = ltWidget.ChildWidgets[0].Widget is SplitWidgetContainer ? (ltWidget.ChildWidgets[0].Widget as SplitWidgetContainer).RealWidgetContainer as WSection : ltWidget.ChildWidgets[0].Widget as WSection;
    int num2 = 0;
    if (entity != null)
      num2 = entity.Document.Sections.IndexOf((IWSection) section);
    for (int index = 0; index < this.CurrentPage.FootnoteWidgets.Count; ++index)
    {
      if (num2 == this.CurrentPage.FootNoteSectionIndex[index])
        num1 += this.CurrentPage.FootnoteWidgets[index].Bounds.Height;
    }
    for (int endnoteCount = this.m_endnoteCount; endnoteCount < this.CurrentPage.EndnoteWidgets.Count; ++endnoteCount)
    {
      if (entity != null && entity.Document.EndnotePosition == EndnotePosition.DisplayEndOfSection)
      {
        if (num2 == this.CurrentPage.EndNoteSectionIndex[endnoteCount])
          this.CurrentPage.EndnoteWidgets[endnoteCount].ShiftLocation(0.0, (double) layoutArea.Height + (double) num1, true, false);
      }
      else
        this.CurrentPage.EndnoteWidgets[endnoteCount].ShiftLocation(0.0, (double) layoutArea.Height + (double) num1, true, false);
    }
    if (this.m_currSection.Columns.Count > 1)
      this.m_endnoteCount = this.m_currPage.EndnoteWidgets.Count;
    if (this.m_columnIndex != this.m_currSection.Columns.Count && this.m_currSection.Columns.Count != 1)
      return;
    this.m_endnoteCount = 0;
  }

  internal bool FooterGetNextArea(out RectangleF area)
  {
    area = this.CurrentPage.PageWidgets.Count != 1 ? RectangleF.Empty : this.CurrentPage.GetFooterArea();
    return !area.Equals((object) RectangleF.Empty);
  }

  internal void FooterPushLayoutedWidget(LayoutedWidget ltWidget)
  {
    float footerHeight = Math.Max(ltWidget.Bounds.Height + ((double) this.CurrentPage.Setup.FooterDistance != -0.05000000074505806 ? this.CurrentPage.Setup.FooterDistance : 36f), this.CurrentPage.Setup.Margins.Bottom);
    RectangleF bounds = ltWidget.Bounds;
    if (ltWidget.ChildWidgets.Count >= 1 && ltWidget.ChildWidgets[0].Widget is WTable && (double) ltWidget.ChildWidgets[0].Bounds.Y >= (double) ltWidget.ChildWidgets[ltWidget.ChildWidgets.Count - 1].Bounds.Bottom && (ltWidget.ChildWidgets[0].Widget as WTable).Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
    {
      RowFormat tableFormat = (ltWidget.ChildWidgets[0].Widget as WTable).TableFormat;
      if (tableFormat.WrapTextAround && tableFormat.Positioning.VertRelationTo == VerticalRelation.Paragraph)
        bounds.Height += ltWidget.ChildWidgets[0].Bounds.Height;
    }
    ltWidget.ShiftLocation(0.0, -(double) bounds.Height, footerHeight, this.CurrentPage.Setup.PageSize.Height);
    this.CurrentPage.PageWidgets.Add(ltWidget);
  }

  internal class HeaderFooterLPHandler : ILayoutProcessHandler
  {
    private DocumentLayouter m_dl;
    private bool m_bFooter;

    public HeaderFooterLPHandler(DocumentLayouter dl, bool bFooter)
    {
      this.m_dl = dl;
      this.m_bFooter = bFooter;
    }

    public bool GetNextArea(
      out RectangleF area,
      ref int columnIndex,
      ref bool isContinuousSection,
      bool isSplittedWidget,
      ref float topMargin,
      bool isFromDynmicLayout,
      ref IWidgetContainer curWidget)
    {
      return !this.m_bFooter ? this.m_dl.HeaderGetNextArea(out area) : this.m_dl.FooterGetNextArea(out area);
    }

    public void PushLayoutedWidget(
      LayoutedWidget ltWidget,
      RectangleF layoutArea,
      bool isNeedToRestartFootnote,
      bool isNeedToRestartEndnote,
      LayoutState state,
      bool isNeedToFindInterSectingPoint)
    {
      if (!this.m_bFooter)
        this.m_dl.HeaderPushLayoutedWidget(ltWidget);
      else
        this.m_dl.FooterPushLayoutedWidget(ltWidget);
    }

    public bool HandleSplittedWidget(
      SplitWidgetContainer stWidgetContainer,
      LayoutState state,
      LayoutedWidget ltWidget,
      ref bool isLayoutedWidgetNeedToPushed)
    {
      return false;
    }

    public void HandleLayoutedWidget(LayoutedWidget ltWidget)
    {
    }
  }

  internal class BookmarkHyperlink
  {
    private RectangleF m_sourceBounds;
    private RectangleF m_targetBounds;
    private int m_targetPageNumber;
    private int m_sourcePageNumber;
    private string m_hyperlinkValue;
    private int m_tocLevel;
    private string m_tocText;
    private bool m_isTargetNull;

    public RectangleF SourceBounds
    {
      get => this.m_sourceBounds;
      set => this.m_sourceBounds = value;
    }

    public RectangleF TargetBounds
    {
      get => this.m_targetBounds;
      set => this.m_targetBounds = value;
    }

    public int TargetPageNumber
    {
      get => this.m_targetPageNumber;
      set => this.m_targetPageNumber = value;
    }

    public int SourcePageNumber
    {
      get => this.m_sourcePageNumber;
      set => this.m_sourcePageNumber = value;
    }

    public string HyperlinkValue
    {
      get => this.m_hyperlinkValue;
      set => this.m_hyperlinkValue = value;
    }

    public int TOCLevel
    {
      get => this.m_tocLevel;
      set => this.m_tocLevel = value;
    }

    public string TOCText
    {
      get => this.m_tocText;
      set => this.m_tocText = value;
    }

    public bool IsTargetNull
    {
      get => this.m_isTargetNull;
      set => this.m_isTargetNull = value;
    }

    public BookmarkHyperlink()
    {
      this.SourceBounds = new RectangleF();
      this.TargetBounds = new RectangleF();
      this.TargetPageNumber = 0;
      this.SourcePageNumber = 0;
      this.TOCLevel = 0;
      this.TOCText = string.Empty;
      this.HyperlinkValue = string.Empty;
    }
  }
}
