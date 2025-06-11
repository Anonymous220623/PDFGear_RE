// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.SearchModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using PDFKit;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Models.Menus;

public class SearchModel : ObservableObject, IDisposable
{
  private PdfDocument document;
  private bool isSearchVisible;
  private List<PdfSearch.FoundText> foundText = new List<PdfSearch.FoundText>();
  private List<PdfSearch.FoundText> forHighlight = new List<PdfSearch.FoundText>();
  private DispatcherTimer searchTimer;
  private string searchText = string.Empty;
  private FindFlags searchFlags;
  private int prevRecord;
  private int searchPageIndex;
  private int totalRecords;
  private int currentRecord;
  private RelayCommand searchDownCmd;
  private RelayCommand searchUpCmd;
  private double progress;
  private int searchStartPage = -1;
  private int minAfterStartPage = -1;
  private bool scrollToRecord = true;
  private bool autoSelectDisabled;

  public SearchModel(PdfDocument document)
  {
    this.document = document;
    if (document != null)
    {
      this.searchTimer = new DispatcherTimer();
      this.searchTimer.Interval = TimeSpan.FromMilliseconds(1.0);
      this.searchTimer.Tick += new EventHandler(this.SearchTimer_Tick);
    }
    this.HighlightColor = Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 198, (byte) 131);
    this.ActiveRecordColor = Color.FromArgb(byte.MaxValue, (byte) 242, (byte) 131, (byte) 129);
    this.InflateHighlight = new FS_RECTF(2.0, 2.0, 2.0, 2.0);
  }

  public bool IsSearchEnabled => this.document != null;

  public bool IsSearchVisible
  {
    get => this.isSearchVisible;
    set
    {
      if (!this.SetProperty<bool>(ref this.isSearchVisible, value, nameof (IsSearchVisible)) || !value)
        return;
      this.DoSearch();
    }
  }

  public string SearchText
  {
    get => this.searchText;
    set
    {
      this.SetProperty<string>(ref this.searchText, value, nameof (SearchText));
      this.DoSearch();
    }
  }

  public FindFlags SearchFlag
  {
    get => this.searchFlags;
    private set
    {
      this.SetProperty<FindFlags>(ref this.searchFlags, value, nameof (SearchFlag));
      this.DoSearch();
    }
  }

  public bool MatchCase
  {
    get => (this.SearchFlag & FindFlags.MatchCase) != 0;
    set
    {
      if (this.MatchCase == value)
        return;
      this.OnPropertyChanging(nameof (MatchCase));
      if (value)
        this.SearchFlag |= FindFlags.MatchCase;
      else
        this.SearchFlag &= ~FindFlags.MatchCase;
      this.OnPropertyChanged(nameof (MatchCase));
    }
  }

  public bool MatchWholeWord
  {
    get => (this.SearchFlag & FindFlags.MatchWholeWord) != 0;
    set
    {
      if (this.MatchWholeWord == value)
        return;
      this.OnPropertyChanging(nameof (MatchWholeWord));
      if (value)
        this.SearchFlag |= FindFlags.MatchWholeWord;
      else
        this.SearchFlag &= ~FindFlags.MatchWholeWord;
      this.OnPropertyChanged(nameof (MatchWholeWord));
    }
  }

  public Color HighlightColor { get; }

  public FS_RECTF InflateHighlight { get; }

  public Color ActiveRecordColor { get; }

  public int TotalRecords
  {
    get => this.totalRecords;
    private set
    {
      int newValue = value;
      if (newValue < 0)
        newValue = 0;
      if (!this.SetProperty<int>(ref this.totalRecords, newValue, nameof (TotalRecords)))
        return;
      if (this.CurrentRecord > this.totalRecords)
      {
        this.scrollToRecord = false;
        this.CurrentRecord = this.totalRecords;
        this.scrollToRecord = true;
      }
      if (newValue > 0 && this.currentRecord == 0)
        this.CurrentRecord = 1;
      this.SearchUpCmd.NotifyCanExecuteChanged();
      this.SearchDownCmd.NotifyCanExecuteChanged();
    }
  }

  public int CurrentRecord
  {
    get => this.currentRecord;
    private set
    {
      this.SetProperty<int>(ref this.currentRecord, value, nameof (CurrentRecord));
      this.OnCurrentRecordChanged(this.CurrentRecord, this.TotalRecords);
    }
  }

  public double Progress
  {
    get => this.progress;
    set => this.SetProperty<double>(ref this.progress, value, nameof (Progress));
  }

  protected virtual void OnCurrentRecordChanged(int currentRecord, int totalRecords)
  {
    if (totalRecords <= 0 || currentRecord <= 0 || currentRecord > totalRecords)
      return;
    if (this.scrollToRecord)
      this.ScrollToRecord(currentRecord);
    this.HighlightRecord(this.prevRecord, currentRecord);
    this.prevRecord = currentRecord;
  }

  private void ScrollToRecord(int currentRecord)
  {
    if (currentRecord < 1 || currentRecord > this.foundText.Count)
      return;
    PdfSearch.FoundText foundText = this.foundText[currentRecord - 1];
    PdfViewer pdfViewer = this.GetPdfViewer();
    if (pdfViewer == null)
      return;
    pdfViewer.CurrentIndex = foundText.PageIndex;
    List<Int32Rect> highlightedRects = pdfViewer.GetHighlightedRects(foundText.PageIndex, new HighlightInfo()
    {
      CharIndex = foundText.CharIndex,
      CharsCount = foundText.CharsCount,
      Inflate = this.InflateHighlight
    });
    if (highlightedRects.Count <= 0)
      return;
    DpiScale dpiScale = VisualTreeHelper.GetDpi((Visual) pdfViewer);
    Rect[] array = highlightedRects.Select<Int32Rect, Rect>((Func<Int32Rect, Rect>) (c => new Rect((double) c.X / dpiScale.PixelsPerDip, (double) c.Y / dpiScale.PixelsPerDip, (double) c.Width / dpiScale.PixelsPerDip, (double) c.Height / dpiScale.PixelsPerDip))).ToArray<Rect>();
    Rect rect1 = new Rect(0.0, 0.0, pdfViewer.ViewportWidth, pdfViewer.ViewportHeight);
    Rect rect2 = new Rect(array[0].X, array[0].Y, array[0].Width, array[0].Height);
    if (array.Length == 0 || rect1.Contains(rect2))
      return;
    Point pagePoint;
    if (pdfViewer.TryGetPagePoint(foundText.PageIndex, new Point(array[0].X, array[0].Y), out pagePoint))
    {
      pdfViewer.ScrollToPoint(foundText.PageIndex, pagePoint);
    }
    else
    {
      pdfViewer.ScrollToPage(foundText.PageIndex);
      pdfViewer.UpdateLayout();
      if (!pdfViewer.TryGetPagePoint(foundText.PageIndex, new Point(array[0].X, array[0].Y), out pagePoint))
        return;
      pdfViewer.ScrollToPoint(foundText.PageIndex, pagePoint);
    }
  }

  private void HighlightRecord(int prevRecord, int currentRecord)
  {
    PdfViewer pdfViewer = this.GetPdfViewer();
    if (pdfViewer == null)
      return;
    if (prevRecord >= 1 && prevRecord <= this.foundText.Count)
    {
      PdfSearch.FoundText foundText = this.foundText[prevRecord - 1];
      pdfViewer.HighlightText(foundText.PageIndex, foundText.CharIndex, foundText.CharsCount, this.HighlightColor, this.InflateHighlight);
    }
    if (currentRecord < 1 || currentRecord > this.foundText.Count)
      return;
    PdfSearch.FoundText foundText1 = this.foundText[currentRecord - 1];
    pdfViewer.HighlightText(foundText1.PageIndex, foundText1.CharIndex, foundText1.CharsCount, this.ActiveRecordColor, this.InflateHighlight);
  }

  private void DoSearch()
  {
    if (this.document == null || this.document.Pages == null)
      return;
    this.StartSearch();
  }

  private void StartSearch()
  {
    if (!this.IsSearchEnabled || !this.IsSearchVisible)
      return;
    this.StopSearch();
    if (string.IsNullOrEmpty(this.searchText))
      return;
    this.prevRecord = -1;
    this.searchPageIndex = 0;
    this.searchStartPage = this.document?.Pages?.CurrentIndex ?? -1;
    this.autoSelectDisabled = false;
    this.minAfterStartPage = -1;
    if (this.document != null)
    {
      this.document.Pages.CurrentPageChanged -= new EventHandler(this.Pages_CurrentPageChanged);
      this.document.Pages.CurrentPageChanged += new EventHandler(this.Pages_CurrentPageChanged);
    }
    this.searchTimer.Start();
  }

  private void Pages_CurrentPageChanged(object sender, EventArgs e)
  {
    this.autoSelectDisabled = true;
  }

  private void StopSearch()
  {
    if (!this.IsSearchEnabled)
      return;
    if (this.document != null)
      this.document.Pages.CurrentPageChanged -= new EventHandler(this.Pages_CurrentPageChanged);
    this.Progress = 0.0;
    this.searchPageIndex = -1;
    this.searchStartPage = -1;
    this.minAfterStartPage = -1;
    this.autoSelectDisabled = false;
    this.searchTimer.Stop();
    this.foundText.Clear();
    this.forHighlight.Clear();
    PdfViewer pdfViewer = this.GetPdfViewer();
    if (pdfViewer != null && pdfViewer.Document != null)
      pdfViewer.RemoveHighlightFromText();
    this.CurrentRecord = 0;
    this.TotalRecords = 0;
  }

  private void SearchTimer_Tick(object sender, EventArgs e)
  {
    if (this.searchPageIndex < 0)
    {
      this.searchTimer.Stop();
      this.Progress = 0.0;
    }
    else
    {
      PdfDocument document = this.document;
      int count1 = document.Pages.Count;
      if (this.searchPageIndex >= count1)
      {
        this.searchTimer.Stop();
        this.Progress = 1.0;
      }
      else
      {
        IntPtr page = Pdfium.FPDF_LoadPage(document.Handle, this.searchPageIndex);
        if (page == IntPtr.Zero)
        {
          this.searchTimer.Stop();
          this.Progress = 1.0;
        }
        else
        {
          IntPtr text_page = Pdfium.FPDFText_LoadPage(page);
          if (text_page == IntPtr.Zero)
          {
            this.searchTimer.Stop();
            this.Progress = 1.0;
          }
          else
          {
            IntPtr start = Pdfium.FPDFText_FindStart(text_page, this.searchText, this.searchFlags, 0);
            if (start == IntPtr.Zero)
            {
              this.searchTimer.Stop();
              this.Progress = 1.0;
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
                  PageIndex = this.searchPageIndex
                };
                this.foundText.Add(foundText);
                this.forHighlight.Add(foundText);
                if (this.searchStartPage != -2 && this.minAfterStartPage == -1 && this.searchPageIndex >= this.searchStartPage)
                  this.minAfterStartPage = this.searchPageIndex;
              }
              Pdfium.FPDFText_FindClose(start);
              Pdfium.FPDFText_ClosePage(text_page);
              Pdfium.FPDF_ClosePage(page);
              this.UpdateResults();
              this.Progress = Math.Max(this.Progress, (double) this.searchPageIndex * 1.0 / (double) count1);
              ++this.searchPageIndex;
              if (this.searchStartPage == -2)
                return;
              if (this.minAfterStartPage != -1)
              {
                int index = 0;
                while (index < this.foundText.Count && this.foundText[index].PageIndex != this.minAfterStartPage)
                  ++index;
                if (index >= this.foundText.Count)
                  return;
                this.searchStartPage = -2;
                this.scrollToRecord = !this.autoSelectDisabled;
                this.CurrentRecord = index + 1;
                this.scrollToRecord = true;
              }
              else
              {
                int searchPageIndex = this.searchPageIndex;
                int? count2 = this.document?.Pages?.Count;
                int valueOrDefault = count2.GetValueOrDefault();
                if (!(searchPageIndex == valueOrDefault & count2.HasValue))
                  return;
                this.scrollToRecord = !this.autoSelectDisabled;
                this.CurrentRecord = this.TotalRecords != 0 ? 1 : 0;
                this.scrollToRecord = true;
              }
            }
          }
        }
      }
    }
  }

  private void UpdateResults()
  {
    if (this.foundText != null)
    {
      this.scrollToRecord = false;
      this.TotalRecords = this.foundText.Count;
      this.scrollToRecord = true;
    }
    PdfViewer pdfViewer = this.GetPdfViewer();
    if (pdfViewer == null || pdfViewer.Document == null)
      return;
    foreach (PdfSearch.FoundText foundText in this.forHighlight)
    {
      HighlightInfo highlightInfo = new HighlightInfo()
      {
        CharIndex = foundText.CharIndex,
        CharsCount = foundText.CharsCount,
        Color = this.HighlightColor,
        Inflate = this.InflateHighlight
      };
      if (!pdfViewer.HighlightedTextInfo.ContainsKey(foundText.PageIndex))
        pdfViewer.HighlightedTextInfo.Add(foundText.PageIndex, new List<HighlightInfo>());
      pdfViewer.HighlightedTextInfo[foundText.PageIndex].Add(highlightInfo);
    }
    this.forHighlight.Clear();
  }

  public RelayCommand SearchDownCmd
  {
    get
    {
      return this.searchDownCmd ?? (this.searchDownCmd = new RelayCommand((Action) (() => this.SearchDown()), (Func<bool>) (() => this.CanSearchDown())));
    }
  }

  private void SearchDown()
  {
    if (this.document == null || this.document.Pages == null)
      return;
    if (this.TotalRecords <= 0)
    {
      this.TotalRecords = 0;
      this.CurrentRecord = 0;
    }
    else
    {
      this.searchStartPage = -2;
      if (this.CurrentRecord < this.TotalRecords)
        ++this.CurrentRecord;
      else
        this.CurrentRecord = 1;
    }
  }

  private bool CanSearchDown() => this.document != null && this.TotalRecords > 0;

  public RelayCommand SearchUpCmd
  {
    get
    {
      return this.searchUpCmd ?? (this.searchUpCmd = new RelayCommand((Action) (() => this.SearchUp()), (Func<bool>) (() => this.CanSearchUp())));
    }
  }

  private void SearchUp()
  {
    if (this.document == null || this.document.Pages == null)
      return;
    if (this.TotalRecords <= 0)
    {
      this.TotalRecords = 0;
      this.CurrentRecord = 0;
    }
    else
    {
      this.searchStartPage = -2;
      if (this.CurrentRecord > 1)
        --this.CurrentRecord;
      else
        this.CurrentRecord = this.TotalRecords;
    }
  }

  private bool CanSearchUp() => this.document != null && this.TotalRecords > 0;

  private PdfViewer GetPdfViewer()
  {
    if (this.document == null)
      return (PdfViewer) null;
    return PDFKit.PdfControl.GetPdfControl(this.document)?.Viewer;
  }

  public void Dispose()
  {
    this.StopSearch();
    if (this.document != null)
      this.document = (PdfDocument) null;
    this.OnPropertyChanged("IsSearchEnabled");
    if (this.searchTimer == null)
      return;
    this.searchTimer.Tick -= new EventHandler(this.SearchTimer_Tick);
    this.searchTimer = (DispatcherTimer) null;
  }
}
