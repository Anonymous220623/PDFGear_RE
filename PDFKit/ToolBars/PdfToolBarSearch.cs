// Decompiled with JetBrains decompiler
// Type: PDFKit.ToolBars.PdfToolBarSearch
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace PDFKit.ToolBars;

public class PdfToolBarSearch : PdfToolBar
{
  private List<PdfSearch.FoundText> _foundText = new List<PdfSearch.FoundText>();
  private List<PdfSearch.FoundText> _forHighlight = new List<PdfSearch.FoundText>();
  private DispatcherTimer _searchTimer;
  private int _searchPageIndex;
  private string _searchText;
  private FindFlags _searchFlags;
  private int _prevRecord;

  public FS_RECTF InflateHighlight { get; set; }

  public Color HighlightColor { get; set; }

  public Color ActiveRecordColor { get; set; }

  public string SearchText
  {
    get => (this.Items[0] as SearchBar).SearchText;
    set => (this.Items[0] as SearchBar).SearchText = value;
  }

  public FindFlags SearchFlags
  {
    get => (this.Items[0] as SearchBar).FindFlags;
    set => (this.Items[0] as SearchBar).FindFlags = value;
  }

  public int CurrentRecord
  {
    get => (this.Items[0] as SearchBar).CurrentRecord;
    set => (this.Items[0] as SearchBar).CurrentRecord = value;
  }

  public int TotalRecords => (this.Items[0] as SearchBar).TotalRecords;

  public PdfToolBarSearch()
  {
    this._searchTimer = new DispatcherTimer();
    this._searchTimer.Interval = TimeSpan.FromMilliseconds(1.0);
    this._searchTimer.Tick += new EventHandler(this._searchTimer_Tick);
    this.HighlightColor = Color.FromArgb((byte) 50, byte.MaxValue, byte.MaxValue, (byte) 0);
    this.ActiveRecordColor = Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 0);
    this.InflateHighlight = new FS_RECTF(2.0, 3.5, 2.0, 2.0);
  }

  protected override void InitializeButtons()
  {
    SearchBar newItem = new SearchBar();
    newItem.Name = "btnSearchBar";
    newItem.CurrentRecordChanged += new EventHandler(this.SearchBar_CurrentRecordChanged);
    newItem.NeedSearch += new EventHandler(this.SearchBar_NeedSearch);
    this.Items.Add((object) newItem);
  }

  protected override void UpdateButtons()
  {
    if (this.Items[0] is SearchBar searchBar)
    {
      searchBar.IsEnabled = this.PdfViewer != null && this.PdfViewer.Document != null;
      searchBar.TotalRecords = 0;
      searchBar.SearchText = "";
    }
    if (this.PdfViewer != null && this.PdfViewer.Document != null)
      ;
  }

  protected override void OnPdfViewerChanging(PdfViewer oldValue, PdfViewer newValue)
  {
    base.OnPdfViewerChanging(oldValue, newValue);
    if (oldValue != null)
      this.UnsubscribePdfViewEvents(oldValue);
    if (newValue != null)
      this.SubscribePdfViewEvents(newValue);
    if (oldValue != null && oldValue.Document != null)
      this.PdfViewer_DocumentClosed((object) this, EventArgs.Empty);
    if (newValue == null || newValue.Document == null)
      return;
    this.PdfViewer_DocumentLoaded((object) this, EventArgs.Empty);
  }

  private void PdfViewer_DocumentClosed(object sender, EventArgs e)
  {
    this.UpdateButtons();
    this.StopSearch();
  }

  private void PdfViewer_DocumentLoaded(object sender, EventArgs e) => this.UpdateButtons();

  private void SearchBar_NeedSearch(object sender, EventArgs e)
  {
    this.OnNeedSearch(this.SearchFlags, this.SearchText);
  }

  private void SearchBar_CurrentRecordChanged(object sender, EventArgs e)
  {
    this.OnCurrentRecordChanged(this.CurrentRecord, this.TotalRecords);
  }

  protected virtual void OnCurrentRecordChanged(int currentRecord, int totalRecords)
  {
    this.ScrollToRecord(currentRecord);
    this.HighlightRecord(this._prevRecord, currentRecord);
    this._prevRecord = currentRecord;
  }

  protected virtual void OnNeedSearch(FindFlags searchFlags, string searchText)
  {
    this.StartSearch(searchFlags, searchText);
  }

  private void UnsubscribePdfViewEvents(PdfViewer oldValue)
  {
    oldValue.AfterDocumentChanged -= new EventHandler(this.PdfViewer_DocumentLoaded);
    oldValue.DocumentLoaded -= new EventHandler(this.PdfViewer_DocumentLoaded);
    oldValue.DocumentClosed -= new EventHandler(this.PdfViewer_DocumentClosed);
  }

  private void SubscribePdfViewEvents(PdfViewer newValue)
  {
    newValue.AfterDocumentChanged += new EventHandler(this.PdfViewer_DocumentLoaded);
    newValue.DocumentLoaded += new EventHandler(this.PdfViewer_DocumentLoaded);
    newValue.DocumentClosed += new EventHandler(this.PdfViewer_DocumentClosed);
  }

  private void StartSearch(FindFlags searchFlags, string searchText)
  {
    this.StopSearch();
    if (searchText == "")
      return;
    this._prevRecord = -1;
    this._searchText = searchText;
    this._searchFlags = searchFlags;
    this._searchPageIndex = 0;
    this._searchTimer.Start();
  }

  private void StopSearch()
  {
    this._searchPageIndex = -1;
    this._searchTimer.Stop();
    this._foundText.Clear();
    this._forHighlight.Clear();
    this.PdfViewer.RemoveHighlightFromText();
    if (!(this.Items[0] is SearchBar searchBar))
      return;
    searchBar.CurrentRecord = 0;
  }

  private void ScrollToRecord(int currentRecord)
  {
    if (currentRecord < 1 || currentRecord > this._foundText.Count)
      return;
    PdfSearch.FoundText foundText = this._foundText[currentRecord - 1];
    this.PdfViewer.CurrentIndex = foundText.PageIndex;
    List<Int32Rect> highlightedRects = this.PdfViewer.GetHighlightedRects(foundText.PageIndex, new HighlightInfo()
    {
      CharIndex = foundText.CharIndex,
      CharsCount = foundText.CharsCount,
      Inflate = this.InflateHighlight
    });
    if (highlightedRects.Count <= 0)
      return;
    Rect rect1 = new Rect(0.0, 0.0, this.PdfViewer.ViewportWidth, this.PdfViewer.ViewportHeight);
    Rect rect2;
    ref Rect local = ref rect2;
    double x1 = (double) highlightedRects[0].X;
    double y1 = (double) highlightedRects[0].Y;
    Int32Rect int32Rect = highlightedRects[0];
    double width = (double) int32Rect.Width;
    int32Rect = highlightedRects[0];
    double height = (double) int32Rect.Height;
    local = new Rect(x1, y1, width, height);
    if (highlightedRects.Count > 0 && !rect1.Contains(rect2))
    {
      PdfViewer pdfViewer = this.PdfViewer;
      int pageIndex = foundText.PageIndex;
      int32Rect = highlightedRects[0];
      double x2 = (double) int32Rect.X;
      int32Rect = highlightedRects[0];
      double y2 = (double) int32Rect.Y;
      Point pt = new Point(x2, y2);
      Point page = pdfViewer.ClientToPage(pageIndex, pt);
      this.PdfViewer.ScrollToPoint(foundText.PageIndex, page);
    }
  }

  private void HighlightRecord(int prevRecord, int currentRecord)
  {
    if (prevRecord >= 1 && prevRecord <= this._foundText.Count)
    {
      PdfSearch.FoundText foundText = this._foundText[prevRecord - 1];
      this.PdfViewer.HighlightText(foundText.PageIndex, foundText.CharIndex, foundText.CharsCount, this.HighlightColor, this.InflateHighlight);
    }
    if (currentRecord < 1 || currentRecord > this._foundText.Count)
      return;
    PdfSearch.FoundText foundText1 = this._foundText[currentRecord - 1];
    this.PdfViewer.HighlightText(foundText1.PageIndex, foundText1.CharIndex, foundText1.CharsCount, this.ActiveRecordColor, this.InflateHighlight);
  }

  private void UpdateResults()
  {
    if (!(this.Items[0] is SearchBar searchBar))
      return;
    searchBar.TotalRecords = this._foundText.Count;
    foreach (PdfSearch.FoundText foundText in this._forHighlight)
    {
      HighlightInfo highlightInfo = new HighlightInfo()
      {
        CharIndex = foundText.CharIndex,
        CharsCount = foundText.CharsCount,
        Color = this.HighlightColor,
        Inflate = this.InflateHighlight
      };
      if (!this.PdfViewer.HighlightedTextInfo.ContainsKey(foundText.PageIndex))
        this.PdfViewer.HighlightedTextInfo.Add(foundText.PageIndex, new List<HighlightInfo>());
      this.PdfViewer.HighlightedTextInfo[foundText.PageIndex].Add(highlightInfo);
    }
    this._forHighlight.Clear();
  }

  private void _searchTimer_Tick(object sender, EventArgs e)
  {
    if (this._searchPageIndex < 0)
      return;
    PdfDocument document = this.PdfViewer.Document;
    if (this._searchPageIndex >= document.Pages.Count)
    {
      this._searchTimer.Stop();
    }
    else
    {
      IntPtr page = Pdfium.FPDF_LoadPage(document.Handle, this._searchPageIndex);
      if (page == IntPtr.Zero)
      {
        this._searchTimer.Stop();
      }
      else
      {
        IntPtr text_page = Pdfium.FPDFText_LoadPage(page);
        if (text_page == IntPtr.Zero)
        {
          this._searchTimer.Stop();
        }
        else
        {
          IntPtr start = Pdfium.FPDFText_FindStart(text_page, this._searchText, this._searchFlags, 0);
          if (start == IntPtr.Zero)
          {
            this._searchTimer.Stop();
          }
          else
          {
            while (Pdfium.FPDFText_FindNext(start))
            {
              int schResultIndex = Pdfium.FPDFText_GetSchResultIndex(start);
              int schCount = Pdfium.FPDFText_GetSchCount(start);
              PdfSearch.FoundText foundText = new PdfSearch.FoundText()
              {
                CharIndex = schResultIndex,
                CharsCount = schCount,
                PageIndex = this._searchPageIndex
              };
              this._foundText.Add(foundText);
              this._forHighlight.Add(foundText);
            }
            Pdfium.FPDFText_FindClose(start);
            Pdfium.FPDFText_ClosePage(text_page);
            Pdfium.FPDF_ClosePage(page);
            this.UpdateResults();
            ++this._searchPageIndex;
          }
        }
      }
    }
  }
}
