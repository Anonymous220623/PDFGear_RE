// Decompiled with JetBrains decompiler
// Type: PDFKit.PdfPrint
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net;
using System;
using System.Printing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;

#nullable disable
namespace PDFKit;

public class PdfPrint
{
  private PdfDocument _document;
  private object _syncIsBusy = new object();
  private bool _isBusy = false;
  private object _syncIsEnd = new object();
  private bool _isEnd = false;

  private bool IsEnd
  {
    get
    {
      lock (this._syncIsEnd)
        return this._isEnd;
    }
    set
    {
      lock (this._syncIsEnd)
        this._isEnd = value;
    }
  }

  public event EventHandler PrintStarted = null;

  public event EventHandler PrintCompleted = null;

  public event EventHandler<PagePrintedEventArgs> PagePrinted = null;

  public event EventHandler CancelationPending = null;

  public bool IsBusy
  {
    get
    {
      lock (this._syncIsBusy)
        return this._isBusy;
    }
    set
    {
      lock (this._syncIsBusy)
        this._isBusy = value;
    }
  }

  protected virtual void OnPrintStarted()
  {
    if (this.PrintStarted == null)
      return;
    this.PrintStarted((object) this, EventArgs.Empty);
  }

  protected virtual void OnPrintCompleted()
  {
    if (this.PrintCompleted == null)
      return;
    this.PrintCompleted((object) this, EventArgs.Empty);
  }

  protected virtual void OnPagePrinted(PagePrintedEventArgs e)
  {
    if (this.PagePrinted == null)
      return;
    this.PagePrinted((object) this, e);
  }

  protected virtual void OnCancelationPending()
  {
    if (this.CancelationPending == null)
      return;
    this.CancelationPending((object) this, EventArgs.Empty);
  }

  public PdfPrint(PdfDocument document) => this._document = document;

  public void ShowDialog(Window window)
  {
    IntPtr parameter = IntPtr.Zero;
    if (window != null)
      parameter = new WindowInteropHelper(window).Handle;
    Thread thread = new Thread(new ParameterizedThreadStart(this.ThreadProc));
    thread.SetApartmentState(ApartmentState.STA);
    this.IsBusy = true;
    this.IsEnd = false;
    thread.Start((object) parameter);
  }

  public void StartPrintAsync() => this.ShowDialog((Window) null);

  public void End()
  {
    if (this.IsBusy)
      this.OnCancelationPending();
    this.IsEnd = true;
  }

  private void ThreadProc(object param)
  {
    IntPtr hwnd = (IntPtr) param;
    ThreadSafePrintDialog threadSafePrintDialog = new ThreadSafePrintDialog()
    {
      MinPage = 1,
      MaxPage = (uint) this._document.Pages.Count
    };
    threadSafePrintDialog.PageRange = new PageRange((int) threadSafePrintDialog.MinPage, (int) threadSafePrintDialog.MaxPage);
    threadSafePrintDialog.UserPageRangeEnabled = true;
    if (hwnd == IntPtr.Zero || threadSafePrintDialog.ShowDialog(hwnd))
    {
      this.OnPrintStarted();
      PrintTicket printTicket = threadSafePrintDialog.PrintTicket;
      double printableAreaWidth = threadSafePrintDialog.PrintableAreaWidth;
      double printableAreaHeight = threadSafePrintDialog.PrintableAreaHeight;
      PdfDocumentPaginator paginator = new PdfDocumentPaginator(this._document, threadSafePrintDialog.PageRange);
      paginator.PagePrinted += new EventHandler<PagePrintedEventArgs>(this.Paginator_PagePrinted);
      paginator.PrinterTicket = printTicket;
      paginator.PageSize = new Size(printableAreaWidth, printableAreaHeight);
      threadSafePrintDialog.PrintDocument((DocumentPaginator) paginator, this._document.Title);
      this.OnPrintCompleted();
    }
    this.IsBusy = false;
  }

  private void Paginator_PagePrinted(object sender, PagePrintedEventArgs e)
  {
    this.OnPagePrinted(e);
    if (!this.IsEnd)
      return;
    e.Cancel = true;
  }
}
