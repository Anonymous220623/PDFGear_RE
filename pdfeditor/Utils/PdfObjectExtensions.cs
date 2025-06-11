// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.PdfObjectExtensions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Controls;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Models.Annotations;
using pdfeditor.ViewModels;
using PDFKit;
using PDFKit.Services;
using PDFKit.Utils;
using PDFKit.Utils.PdfRichTextStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

#nullable disable
namespace pdfeditor.Utils;

public static class PdfObjectExtensions
{
  public static AnnotationHolderManager GetAnnotationHolderManager(PdfViewer viewer)
  {
    return PdfObjectExtensions.GetAnnotationHolderManager(viewer != null ? viewer.GetPdfControl() : (PDFKit.PdfControl) null);
  }

  public static AnnotationHolderManager GetAnnotationHolderManager(PDFKit.PdfControl pdfControl)
  {
    if (pdfControl == null)
      return (AnnotationHolderManager) null;
    if (DispatcherHelper.UIDispatcher.CheckAccess())
      return GetAnnotationHolderManagerCore(pdfControl);
    AnnotationHolderManager holders = (AnnotationHolderManager) null;
    DispatcherHelper.UIDispatcher.Invoke<AnnotationHolderManager>((Func<AnnotationHolderManager>) (() => holders = GetAnnotationHolderManagerCore(pdfControl)));
    return holders;

    static AnnotationHolderManager GetAnnotationHolderManagerCore(PDFKit.PdfControl _pdfControl)
    {
      return PdfObjectExtensions.GetAnnotationCanvas(_pdfControl)?.HolderManager;
    }
  }

  public static AnnotationCanvas GetAnnotationCanvas(PdfViewer viewer)
  {
    return PdfObjectExtensions.GetAnnotationCanvas(viewer != null ? viewer.GetPdfControl() : (PDFKit.PdfControl) null);
  }

  public static AnnotationCanvas GetAnnotationCanvas(PDFKit.PdfControl pdfControl)
  {
    return pdfControl?.Parent is Panel parent ? parent.Children.OfType<AnnotationCanvas>().FirstOrDefault<AnnotationCanvas>() : (AnnotationCanvas) null;
  }

  public static void DeleteAnnotation(this PdfAnnotation annot)
  {
    if ((PdfWrapper) annot == (PdfWrapper) null || annot.Page == null || annot.Page.Annots == null)
      return;
    PdfPage page = annot.Page;
    string name = "";
    System.Collections.Generic.IReadOnlyList<BaseAnnotation> source = AnnotationModelUtils.CreateFlattenedRecord(annot).Item2;
    switch (annot)
    {
      case PdfPopupAnnotation pdfPopupAnnotation when pdfPopupAnnotation.Parent is PdfMarkupAnnotation parent:
        parent.Popup = (PdfPopupAnnotation) null;
        break;
      case PdfFreeTextAnnotation freeTextAnnotation:
        name = freeTextAnnotation.Name;
        break;
    }
    foreach (BaseAnnotation baseAnnotation in source.Reverse<BaseAnnotation>())
      page.Annots.RemoveAt(baseAnnotation.AnnotIndex);
    if (string.IsNullOrEmpty(name))
      return;
    PdfRichTextString.RemoveCacheByName(name);
  }

  public static async Task<PdfAnnotation> DuplicateAnnotationAsync(
    this PdfAnnotation annot,
    FS_POINTF? markupOffset = null,
    FS_POINTF? popupOffset = null)
  {
    FS_POINTF? nullable = markupOffset;
    FS_POINTF _markupOffset = nullable ?? new FS_POINTF(20f, -20f);
    nullable = popupOffset;
    FS_POINTF fsPointf = nullable ?? new FS_POINTF(0.0f, -20f);
    if (!((PdfWrapper) annot != (PdfWrapper) null))
      return (PdfAnnotation) null;
    PdfPage page = annot.Page;
    System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation> replies;
    BaseAnnotation record = AnnotationModelUtils.CloneRecord(annot, out replies);
    System.Collections.Generic.IReadOnlyList<BaseAnnotation> flattens = AnnotationModelUtils.FlattenRecord(record, replies);
    bool hasPopup = false;
    foreach (BaseAnnotation baseAnnotation in (IEnumerable<BaseAnnotation>) flattens)
    {
      if (baseAnnotation is LineAnnotation lineAnnotation)
      {
        FS_POINTF[] array = lineAnnotation.Line.Select<FS_POINTF, FS_POINTF>((Func<FS_POINTF, FS_POINTF>) (c =>
        {
          c.X += _markupOffset.X;
          c.Y += _markupOffset.Y;
          return c;
        })).ToArray<FS_POINTF>();
        lineAnnotation.Line = (System.Collections.Generic.IReadOnlyList<FS_POINTF>) array;
      }
      else if (baseAnnotation is InkAnnotation inkAnnotation)
      {
        System.Collections.Generic.IReadOnlyList<FS_POINTF>[] array = inkAnnotation.InkList.Select<System.Collections.Generic.IReadOnlyList<FS_POINTF>, System.Collections.Generic.IReadOnlyList<FS_POINTF>>((Func<System.Collections.Generic.IReadOnlyList<FS_POINTF>, System.Collections.Generic.IReadOnlyList<FS_POINTF>>) (x => (System.Collections.Generic.IReadOnlyList<FS_POINTF>) x.Select<FS_POINTF, FS_POINTF>((Func<FS_POINTF, FS_POINTF>) (c =>
        {
          c.X += _markupOffset.X;
          c.Y += _markupOffset.Y;
          return c;
        })).ToArray<FS_POINTF>())).ToArray<System.Collections.Generic.IReadOnlyList<FS_POINTF>>();
        inkAnnotation.InkList = (System.Collections.Generic.IReadOnlyList<System.Collections.Generic.IReadOnlyList<FS_POINTF>>) array;
      }
      else if (baseAnnotation is BaseMarkupAnnotation markupAnnotation)
      {
        if (markupAnnotation.Relationship != RelationTypes.Reply)
        {
          FS_RECTF rectangle = markupAnnotation.Rectangle;
          rectangle.left += _markupOffset.X;
          rectangle.right += _markupOffset.X;
          rectangle.top += _markupOffset.Y;
          rectangle.bottom += _markupOffset.Y;
          markupAnnotation.Rectangle = rectangle;
        }
      }
      else if (baseAnnotation is PopupAnnotation popupAnnotation)
      {
        FS_RECTF rectangle = popupAnnotation.Rectangle;
        rectangle.top += fsPointf.Y;
        rectangle.bottom += fsPointf.Y;
        popupAnnotation.Rectangle = rectangle;
      }
      else if (baseAnnotation is LinkAnnotation linkAnnotation)
      {
        FS_RECTF rectangle = linkAnnotation.Rectangle;
        rectangle.left += _markupOffset.X;
        rectangle.right += _markupOffset.X;
        rectangle.top += _markupOffset.Y;
        rectangle.bottom += _markupOffset.Y;
        linkAnnotation.Rectangle = rectangle;
      }
      if (baseAnnotation is PopupAnnotation)
        hasPopup = true;
    }
    AnnotationModelUtils.InsertRecord(page.Document, record, flattens);
    PDFKit.PdfControl viewer = PDFKit.PdfControl.GetPdfControl(page.Document);
    MainViewModel dataContext1 = viewer.DataContext as MainViewModel;
    if (!dataContext1.IsAnnotationVisible)
      dataContext1.IsAnnotationVisible = true;
    await dataContext1.OperationManager.AddOperationAsync((Func<PdfDocument, Task>) (async doc =>
    {
      PDFKit.PdfControl _pdfControl = PDFKit.PdfControl.GetPdfControl(doc);
      if (_pdfControl?.DataContext is MainViewModel dataContext3 && !dataContext3.IsAnnotationVisible)
        dataContext3.IsAnnotationVisible = true;
      AnnotationModelUtils.RemoveRecord(doc, record, flattens);
      await doc.Pages[record.PageIndex].TryRedrawPageAsync();
      if (!hasPopup)
      {
        _pdfControl = (PDFKit.PdfControl) null;
      }
      else
      {
        AnnotationCanvas annotationCanvas = PdfObjectExtensions.GetAnnotationCanvas(_pdfControl);
        if (annotationCanvas == null)
        {
          _pdfControl = (PDFKit.PdfControl) null;
        }
        else
        {
          annotationCanvas.PopupHolder.FlushAnnotationPopup();
          _pdfControl = (PDFKit.PdfControl) null;
        }
      }
    }), (Func<PdfDocument, Task>) (async doc =>
    {
      PDFKit.PdfControl _pdfControl = PDFKit.PdfControl.GetPdfControl(doc);
      if (_pdfControl?.DataContext is MainViewModel dataContext5 && !dataContext5.IsAnnotationVisible)
        dataContext5.IsAnnotationVisible = true;
      AnnotationModelUtils.InsertRecord(doc, record, flattens);
      await doc.Pages[record.PageIndex].TryRedrawPageAsync();
      if (!hasPopup)
      {
        _pdfControl = (PDFKit.PdfControl) null;
      }
      else
      {
        AnnotationCanvas annotationCanvas = PdfObjectExtensions.GetAnnotationCanvas(_pdfControl);
        if (annotationCanvas == null)
        {
          _pdfControl = (PDFKit.PdfControl) null;
        }
        else
        {
          annotationCanvas.PopupHolder.FlushAnnotationPopup();
          _pdfControl = (PDFKit.PdfControl) null;
        }
      }
    }));
    await page.TryRedrawPageAsync();
    if (hasPopup)
      PdfObjectExtensions.GetAnnotationCanvas(viewer)?.PopupHolder.FlushAnnotationPopup();
    return page.Annots[record.AnnotIndex];
  }

  public static async Task TryRedrawPageAsync(
    this PdfPage page,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    if (page == null)
      return;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(page.Document);
    if (pdfControl != null)
    {
      AnnotationHolderManager annotationHolderManager = PdfObjectExtensions.GetAnnotationHolderManager(pdfControl);
      int? pageIndex1 = annotationHolderManager?.CurrentHolder?.CurrentPage?.PageIndex;
      int pageIndex2 = page.PageIndex;
      if (pageIndex1.GetValueOrDefault() == pageIndex2 & pageIndex1.HasValue)
        annotationHolderManager.CurrentHolder.Cancel();
    }
    for (int i = 0; i < 3; ++i)
    {
      bool flag = page.IsDisposed;
      if (!flag)
        flag = PdfDocumentStateService.CanDisposePage(page);
      ProgressiveStatus progressiveStatus;
      if (!flag && PdfObjectExtensions.TryGetProgressiveStatus(page, out progressiveStatus))
        flag = progressiveStatus != ProgressiveStatus.ToBeContinued && progressiveStatus != ProgressiveStatus.Failed;
      if (flag)
      {
        try
        {
          PageDisposeHelper.DisposePage(page);
          PdfDocumentStateService.TryRedrawViewerCurrentPage(page);
          break;
        }
        catch
        {
          break;
        }
      }
      else
        await Task.Delay(150, cancellationToken);
    }
  }

  public static async Task TryRedrawVisiblePageAsync(
    this PdfViewer viewer,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    PdfViewer pdfViewer = viewer;
    PDFKit.PdfControl pdfControl = pdfViewer != null ? pdfViewer.GetPdfControl() : (PDFKit.PdfControl) null;
    if (pdfControl == null)
      return;
    await pdfControl.TryRedrawVisiblePageAsync(cancellationToken);
  }

  public static async Task TryRedrawVisiblePageAsync(
    this PDFKit.PdfControl pdfControl,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    if (pdfControl == null)
      return;
    (int startPage, int endPage) = pdfControl.GetVisiblePageRange();
    if (startPage == -1 || endPage == -1)
      return;
    AnnotationHolderManager annotationHolderManager = PdfObjectExtensions.GetAnnotationHolderManager(pdfControl);
    int num = annotationHolderManager?.CurrentHolder?.CurrentPage?.PageIndex ?? -1;
    if (num != -1 && (num <= startPage || num <= endPage))
      annotationHolderManager.CurrentHolder.Cancel();
    for (int pageIdx = startPage; pageIdx <= endPage && pageIdx < (pdfControl.Document?.Pages?.Count ?? -1); ++pageIdx)
    {
      PdfPage page = pdfControl.Document.Pages[pageIdx];
      for (int i = 0; i < 3; ++i)
      {
        bool flag = page.IsDisposed;
        if (!flag)
          flag = PdfDocumentStateService.CanDisposePage(page);
        ProgressiveStatus progressiveStatus;
        if (!flag && PdfObjectExtensions.TryGetProgressiveStatus(page, out progressiveStatus))
          flag = progressiveStatus != ProgressiveStatus.ToBeContinued && progressiveStatus != ProgressiveStatus.Failed;
        if (flag)
        {
          try
          {
            PageDisposeHelper.DisposePage(page);
            PdfDocumentStateService.TryRedrawViewerCurrentPage(page);
            break;
          }
          catch
          {
            break;
          }
        }
        else
          await Task.Delay(150, cancellationToken);
      }
      page = (PdfPage) null;
    }
  }

  public static bool ShouldDispose(this PdfPage page)
  {
    ProgressiveStatus progressiveStatus;
    object texts;
    return page != null && (page.Document?.Pages == null || page.PageIndex != page.Document.Pages.CurrentIndex) && !page.IsDisposed && (!PdfObjectExtensions.TryGetProgressiveStatus(page, out progressiveStatus) || progressiveStatus != ProgressiveStatus.ToBeContinued && progressiveStatus != ProgressiveStatus.Failed) && (!PdfObjectExtensions.TryGetText(page, out texts) || texts == null) && (page.Document.FormFill == null || page.Document.FormFill.IsDisposed || page.Document.FormFill.InterForm == null || page.Document.FormFill.InterForm.GetPageControls(page)?.GetFocused() == null);
  }

  public static async Task<bool> WaitProgressiveDoneAsync(
    this PdfPage page,
    CancellationToken cancellationToken)
  {
    return await page.WaitProgressiveDoneAsync(TimeSpan.Zero, cancellationToken);
  }

  public static async Task<bool> WaitProgressiveDoneAsync(
    this PdfPage page,
    TimeSpan timeout,
    CancellationToken cancellationToken)
  {
    if (page == null)
      return false;
    CancellationToken token = cancellationToken;
    if (timeout > TimeSpan.Zero)
      token = CancellationTokenSource.CreateLinkedTokenSource(new CancellationTokenSource(timeout).Token, cancellationToken).Token;
    ProgressiveStatus progressiveStatus;
    while (PdfObjectExtensions.TryGetProgressiveStatus(page, out progressiveStatus))
    {
      if (progressiveStatus != ProgressiveStatus.ToBeContinued)
        return true;
      await Task.Delay(20, token).ConfigureAwait(false);
    }
    return false;
  }

  public static bool TryGetProgressiveStatus(PdfPage page, out ProgressiveStatus progressiveStatus)
  {
    progressiveStatus = ProgressiveStatus.Ready;
    if (page == null || !page.IsParsed)
      return false;
    if (page.IsDisposed)
      return true;
    try
    {
      Func<PdfPage, ProgressiveStatus> orPropertyGetter = CommomLib.Commom.TypeHelper.CreateFieldOrPropertyGetter<PdfPage, ProgressiveStatus>("_progressiveStatus", BindingFlags.Instance | BindingFlags.NonPublic);
      if (orPropertyGetter == null)
        return false;
      progressiveStatus = orPropertyGetter(page);
      return true;
    }
    catch
    {
      progressiveStatus = ProgressiveStatus.Ready;
      return false;
    }
  }

  private static bool TryGetText(PdfPage page, out object texts)
  {
    texts = (object) null;
    if (page == null)
      return false;
    try
    {
      Func<PdfPage, object> orPropertyGetter = CommomLib.Commom.TypeHelper.CreateFieldOrPropertyGetter<PdfPage, object>("_text", BindingFlags.Instance | BindingFlags.NonPublic);
      if (orPropertyGetter == null)
        return false;
      texts = orPropertyGetter(page);
      return true;
    }
    catch
    {
      texts = (object) null;
      return false;
    }
  }

  public static bool TryGetModificationDate(
    this PdfAnnotation annot,
    out DateTimeOffset modificationDate)
  {
    modificationDate = new DateTimeOffset();
    try
    {
      return !((PdfWrapper) annot == (PdfWrapper) null) && !string.IsNullOrEmpty(annot.ModificationDate) && PdfObjectExtensions.TryParseModificationDate(annot.ModificationDate, out modificationDate);
    }
    catch
    {
    }
    return false;
  }

  public static string ToModificationDateString(this DateTimeOffset dateTime)
  {
    return PdfAttributeUtils.ConvertToModificationDateString(dateTime);
  }

  public static bool TryParseModificationDate(string modificationDate, out DateTimeOffset dateTime)
  {
    return PdfAttributeUtils.TryParseModificationDate(modificationDate, out dateTime);
  }

  public static bool TryParsePageRange(string range, out int[] pageIndexes, out int errorCharIndex)
  {
    pageIndexes = (int[]) null;
    int[][] pageIndexes1;
    if (!PdfObjectExtensions.TryParsePageRangeCore(range, out pageIndexes1, out errorCharIndex))
      return false;
    pageIndexes = ((IEnumerable<int[]>) pageIndexes1).SelectMany<int[], int>((Func<int[], IEnumerable<int>>) (c => (IEnumerable<int>) c)).Distinct<int>().OrderBy<int, int>((Func<int, int>) (c => c)).ToArray<int>();
    return true;
  }

  public static bool TryParsePageRange2(
    string range,
    out int[][] pageIndexes,
    out int errorCharIndex)
  {
    return PdfObjectExtensions.TryParsePageRangeCore(range, out pageIndexes, out errorCharIndex);
  }

  private static bool TryParsePageRangeCore(
    string range,
    out int[][] pageIndexes,
    out int errorCharIndex)
  {
    pageIndexes = (int[][]) null;
    errorCharIndex = -1;
    if (string.IsNullOrEmpty(range))
      return false;
    List<List<int>> list = new List<List<int>>();
    PdfObjectExtensions.PageRangeReader pageRangeReader = new PdfObjectExtensions.PageRangeReader(range);
    int from = -1;
    int to = -1;
    bool isTo = false;
    int num = -1;
    while (pageRangeReader.HasMore)
    {
      (PdfObjectExtensions.PageRangeTokenType type, string str, int startIdx) = pageRangeReader.GetNextToken();
      num = startIdx;
      switch (type)
      {
        case PdfObjectExtensions.PageRangeTokenType.Number:
          if (!isTo)
          {
            if (from == -1)
            {
              if (!int.TryParse(str, out from))
              {
                errorCharIndex = startIdx;
                return false;
              }
              continue;
            }
            errorCharIndex = startIdx;
            return false;
          }
          if (to == -1)
          {
            if (!int.TryParse(str, out to))
            {
              errorCharIndex = startIdx;
              return false;
            }
            if (to < from)
            {
              errorCharIndex = startIdx;
              return false;
            }
            continue;
          }
          errorCharIndex = startIdx;
          return false;
        case PdfObjectExtensions.PageRangeTokenType.Dash:
          if (from == -1 | isTo || to != -1)
          {
            errorCharIndex = startIdx;
            return false;
          }
          isTo = true;
          continue;
        case PdfObjectExtensions.PageRangeTokenType.Comma:
          if (!Complete())
          {
            errorCharIndex = startIdx;
            return false;
          }
          continue;
        default:
          if (pageRangeReader.HasMore)
          {
            errorCharIndex = startIdx;
            return false;
          }
          continue;
      }
    }
    if (!Complete())
    {
      errorCharIndex = num;
      return false;
    }
    pageIndexes = list.Select<List<int>, int[]>((Func<List<int>, int[]>) (c => c.OrderBy<int, int>((Func<int, int>) (x => x)).ToArray<int>())).ToArray<int[]>();
    return true;

    bool Complete()
    {
      if (from == -1)
        return false;
      if (to == -1)
      {
        if (isTo)
          return false;
        list.Add(new List<int>() { from - 1 });
      }
      else if (from < to)
      {
        list.Add(new List<int>(Enumerable.Range(from - 1, to - from + 1)));
      }
      else
      {
        if (from != to)
          return false;
        list.Add(new List<int>() { from - 1 });
      }
      from = -1;
      to = -1;
      isTo = false;
      return true;
    }
  }

  public static string ConvertToRange(this IEnumerable<int> pageIndexes)
  {
    return pageIndexes.ConvertToRange(out int[] _);
  }

  public static string ConvertToRange(
    this IEnumerable<int> pageIndexes,
    out int[] sortedPageIndexes)
  {
    sortedPageIndexes = (int[]) null;
    if (pageIndexes == null)
      return string.Empty;
    int[] array = pageIndexes.ToArray<int>();
    if (array.Length == 0)
      return string.Empty;
    Array.Sort<int>(array);
    sortedPageIndexes = array;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(array[0] + 1);
    bool flag = false;
    for (int index = 1; index < array.Length; ++index)
    {
      if (array[index] != array[index - 1])
      {
        if (array[index] - 1 == array[index - 1])
        {
          flag = true;
        }
        else
        {
          if (flag)
          {
            stringBuilder.Append('-').Append(array[index - 1] + 1);
            flag = false;
          }
          stringBuilder.Append(',').Append(array[index] + 1);
        }
      }
    }
    if (flag)
      stringBuilder.Append('-').Append(array[array.Length - 1] + 1);
    return stringBuilder.ToString();
  }

  public static PdfBorderStyle GetBorderStyle(this PdfAnnotation annot)
  {
    if (!annot.IsExists("BS"))
      return (PdfBorderStyle) null;
    try
    {
      PdfBorderStyle pdfBorderStyle = new PdfBorderStyle(annot.Dictionary["BS"]);
      PdfBorderStyle borderStyle = new PdfBorderStyle();
      float[] dashPattern = pdfBorderStyle.DashPattern;
      borderStyle.DashPattern = dashPattern != null ? ((IEnumerable<float>) dashPattern).ToArray<float>() : (float[]) null;
      borderStyle.Style = pdfBorderStyle.Style;
      borderStyle.Width = pdfBorderStyle.Width;
      return borderStyle;
    }
    catch
    {
    }
    return (PdfBorderStyle) null;
  }

  public static void SetBorderStyle(this PdfAnnotation annot, PdfBorderStyle borderStyle)
  {
    if (annot.Dictionary.ContainsKey("BS") && (PdfWrapper) borderStyle == (PdfWrapper) null)
      annot.Dictionary.Remove("BS");
    else
      annot.Dictionary["BS"] = (PdfTypeBase) borderStyle.Dictionary;
  }

  private class PageRangeReader
  {
    private readonly string pageRange;
    private int curIdx;
    private StringBuilder sb;
    private PdfObjectExtensions.PageRangeTokenType curType;

    public PageRangeReader(string pageRange)
    {
      this.pageRange = !string.IsNullOrEmpty(pageRange) ? pageRange : throw new ArgumentException(nameof (pageRange));
      this.sb = new StringBuilder();
      this.curType = PdfObjectExtensions.PageRangeTokenType.None;
    }

    public bool HasMore => this.curIdx < this.pageRange.Length;

    public (PdfObjectExtensions.PageRangeTokenType type, string value, int startIdx) GetNextToken()
    {
      int curIdx = this.curIdx;
      for (; this.curIdx < this.pageRange.Length; ++this.curIdx)
      {
        char ch = this.pageRange[this.curIdx];
        if (ch >= '0' && ch <= '9')
        {
          if (this.curType == PdfObjectExtensions.PageRangeTokenType.None)
            this.curType = PdfObjectExtensions.PageRangeTokenType.Number;
          this.sb.Append(ch);
        }
        else
        {
          switch (ch)
          {
            case ' ':
              if (this.curType != PdfObjectExtensions.PageRangeTokenType.Number)
              {
                ++curIdx;
                continue;
              }
              goto label_17;
            case ',':
              if (this.curType == PdfObjectExtensions.PageRangeTokenType.None)
              {
                this.curType = PdfObjectExtensions.PageRangeTokenType.Comma;
                this.sb.Append(ch);
                ++this.curIdx;
                goto label_17;
              }
              if (this.curType == PdfObjectExtensions.PageRangeTokenType.Number || this.curType == PdfObjectExtensions.PageRangeTokenType.Dash)
                goto label_17;
              continue;
            case '-':
              if (this.curType == PdfObjectExtensions.PageRangeTokenType.None)
              {
                this.curType = PdfObjectExtensions.PageRangeTokenType.Dash;
                this.sb.Append(ch);
                ++this.curIdx;
                goto label_17;
              }
              if (this.curType == PdfObjectExtensions.PageRangeTokenType.Number || this.curType == PdfObjectExtensions.PageRangeTokenType.Comma)
                goto label_17;
              continue;
            default:
              this.curType = PdfObjectExtensions.PageRangeTokenType.None;
              this.sb.Length = 0;
              goto label_17;
          }
        }
      }
label_17:
      int curType = (int) this.curType;
      string str1 = this.sb.ToString();
      this.curType = PdfObjectExtensions.PageRangeTokenType.None;
      this.sb.Length = 0;
      string str2 = str1;
      int num = curIdx;
      return ((PdfObjectExtensions.PageRangeTokenType) curType, str2, num);
    }
  }

  private enum PageRangeTokenType
  {
    None,
    Number,
    Dash,
    Comma,
  }
}
