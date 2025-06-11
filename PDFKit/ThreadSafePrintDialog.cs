// Decompiled with JetBrains decompiler
// Type: PDFKit.ThreadSafePrintDialog
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Microsoft.Win32;
using System;
using System.IO;
using System.Printing;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

#nullable disable
namespace PDFKit;

internal class ThreadSafePrintDialog
{
  private double mPrintableHeight;
  private double mPrintableWidth;
  private bool mHeightUpdated = false;
  private bool mWidthUpdated = false;
  private PrintQueue mPrintQueue = (PrintQueue) null;
  private PrintTicket mPrintTicket = (PrintTicket) null;
  private PageRangeSelection mPageRangeSelection = PageRangeSelection.AllPages;
  private PageRange mPageRange;
  public uint _minPage = 1;
  public uint _maxPage = 9999;
  public bool _userPageRangeEnabled = false;

  private void VerifyPrintSettings()
  {
    if (this.mPrintQueue == null)
      this.mPrintQueue = this.DefaultPrintQueue();
    if (this.mPrintTicket != null)
      return;
    this.mPrintTicket = this.DefaultPrintTicket();
  }

  private PrintQueue DefaultPrintQueue()
  {
    Helpers.SecurityAssert();
    PrintQueue printQueue = (PrintQueue) null;
    try
    {
      printQueue = new LocalPrintServer().DefaultPrintQueue;
    }
    catch (PrintSystemException ex)
    {
      printQueue = (PrintQueue) null;
    }
    finally
    {
      Helpers.SecurityRevert();
    }
    return printQueue;
  }

  private PrintTicket DefaultPrintTicket()
  {
    Helpers.SecurityAssert();
    PrintTicket printTicket = (PrintTicket) null;
    try
    {
      if (this.mPrintQueue != null)
        printTicket = this.mPrintQueue.UserPrintTicket ?? this.mPrintQueue.DefaultPrintTicket;
    }
    catch (PrintSystemException ex)
    {
      printTicket = (PrintTicket) null;
    }
    finally
    {
      Helpers.SecurityRevert();
    }
    if (printTicket == null)
      printTicket = new PrintTicket();
    return printTicket;
  }

  private void UpdateArea()
  {
    this.VerifyPrintSettings();
    PrintCapabilities printCapabilities = (PrintCapabilities) null;
    if (this.mPrintQueue != null)
      printCapabilities = this.mPrintQueue.GetPrintCapabilities(this.mPrintTicket);
    double? nullable;
    int num1;
    if (printCapabilities != null)
    {
      nullable = printCapabilities.OrientedPageMediaWidth;
      if (nullable.HasValue)
      {
        nullable = printCapabilities.OrientedPageMediaHeight;
        num1 = nullable.HasValue ? 1 : 0;
        goto label_6;
      }
    }
    num1 = 0;
label_6:
    if (num1 != 0)
    {
      nullable = printCapabilities.OrientedPageMediaWidth;
      this.mPrintableWidth = nullable.Value;
      nullable = printCapabilities.OrientedPageMediaHeight;
      this.mPrintableHeight = nullable.Value;
    }
    else
    {
      this.mPrintableWidth = 816.0;
      this.mPrintableHeight = 1056.0;
      int num2;
      if (this.mPrintTicket.PageMediaSize != null)
      {
        nullable = this.mPrintTicket.PageMediaSize.Width;
        if (nullable.HasValue)
        {
          nullable = this.mPrintTicket.PageMediaSize.Height;
          num2 = nullable.HasValue ? 1 : 0;
          goto label_12;
        }
      }
      num2 = 0;
label_12:
      if (num2 != 0)
      {
        nullable = this.mPrintTicket.PageMediaSize.Width;
        this.mPrintableWidth = nullable.Value;
        nullable = this.mPrintTicket.PageMediaSize.Height;
        this.mPrintableHeight = nullable.Value;
      }
      if (this.mPrintTicket.PageOrientation.HasValue)
      {
        PageOrientation? pageOrientation = this.mPrintTicket.PageOrientation;
        int num3;
        if (pageOrientation.Value != PageOrientation.Landscape)
        {
          pageOrientation = this.mPrintTicket.PageOrientation;
          num3 = pageOrientation.Value == PageOrientation.ReverseLandscape ? 1 : 0;
        }
        else
          num3 = 1;
        if (num3 != 0)
        {
          double mPrintableWidth = this.mPrintableWidth;
          this.mPrintableWidth = this.mPrintableHeight;
          this.mPrintableHeight = mPrintableWidth;
        }
      }
    }
  }

  public void PrintDocument(DocumentPaginator paginator, string description)
  {
    if (paginator == null)
      throw new ArgumentNullException(nameof (paginator), "No DocumentPaginator to print");
    this.VerifyPrintSettings();
    if (this.mPrintQueue.FullName.Contains("XPS"))
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Xps Document (*.xps) | *.xps";
      if (!saveFileDialog.ShowDialog().GetValueOrDefault())
        return;
      XpsDocument xpsPackage = new XpsDocument(saveFileDialog.FileName, FileAccess.Write);
      new XpsSerializationManager((BasePackagingPolicy) new XpsPackagingPolicy(xpsPackage), false).SaveAsXaml((object) paginator);
      xpsPackage.Close();
    }
    else
    {
      XpsDocumentWriter xpsDocumentWriter = (XpsDocumentWriter) null;
      Helpers.SecurityAssert();
      try
      {
        this.mPrintQueue.CurrentJobSettings.Description = description;
        xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(this.mPrintQueue);
        ThreadSafePrintDialog.TicketEventHandler ticketEventHandler = new ThreadSafePrintDialog.TicketEventHandler(this.mPrintTicket);
        xpsDocumentWriter.WritingPrintTicketRequired += new WritingPrintTicketRequiredEventHandler(ticketEventHandler.SetPrintTicket);
      }
      finally
      {
        Helpers.SecurityRevert();
      }
      xpsDocumentWriter.Write(paginator);
      this.mPrintableWidth = 0.0;
      this.mPrintableHeight = 0.0;
      this.mWidthUpdated = false;
      this.mHeightUpdated = false;
    }
  }

  public bool ShowDialog(IntPtr hwnd)
  {
    NativePrintDialog nativePrintDialog = new NativePrintDialog()
    {
      PrintTicket = this.mPrintTicket,
      PrintQueue = this.mPrintQueue,
      MinPage = this.MinPage,
      MaxPage = this.MaxPage,
      PageRangeEnabled = this.UserPageRangeEnabled,
      PageRange = new PageRange(Math.Max(1, this.mPageRange.PageFrom), this.mPageRange.PageTo),
      PageRangeSelection = this.mPageRangeSelection
    };
    uint num = nativePrintDialog.ShowDialog(hwnd);
    if (num == 1U || num == 2U)
    {
      this.mPrintQueue = nativePrintDialog.PrintQueue;
      this.mPrintTicket = nativePrintDialog.PrintTicket;
      this.mPageRange = nativePrintDialog.PageRange;
      this.mPageRangeSelection = nativePrintDialog.PageRangeSelection;
    }
    return num == 1U;
  }

  internal PageRange PageRange
  {
    get => this.mPageRange;
    set
    {
      if (value.PageTo <= 0 || value.PageFrom <= 0)
        throw new ArgumentException(nameof (PageRange), "PageRange is not valid.");
      this.mPageRange = value;
      if (this.mPageRange.PageFrom <= this.mPageRange.PageTo)
        return;
      int pageFrom = this.mPageRange.PageFrom;
      this.mPageRange.PageFrom = this.mPageRange.PageTo;
      this.mPageRange.PageTo = pageFrom;
    }
  }

  public PageRangeSelection PageRangeSelection
  {
    get => this.mPageRangeSelection;
    set => this.mPageRangeSelection = value;
  }

  public double PrintableAreaHeight
  {
    get
    {
      if (!this.mWidthUpdated && !this.mHeightUpdated || !this.mWidthUpdated && this.mHeightUpdated)
      {
        this.mWidthUpdated = false;
        this.mHeightUpdated = true;
        this.UpdateArea();
      }
      return this.mPrintableHeight;
    }
  }

  public double PrintableAreaWidth
  {
    get
    {
      if (!this.mWidthUpdated && !this.mHeightUpdated || this.mWidthUpdated && !this.mHeightUpdated)
      {
        this.mWidthUpdated = true;
        this.mHeightUpdated = false;
        this.UpdateArea();
      }
      return this.mPrintableWidth;
    }
  }

  public PrintQueue PrintQueue
  {
    get
    {
      this.VerifyPrintSettings();
      return this.mPrintQueue;
    }
    set => this.mPrintQueue = value;
  }

  public PrintTicket PrintTicket
  {
    get
    {
      this.VerifyPrintSettings();
      return this.mPrintTicket;
    }
    set => this.mPrintTicket = value;
  }

  public uint MinPage
  {
    get => this._minPage;
    set => this._minPage = value;
  }

  public uint MaxPage
  {
    get => this._maxPage;
    set => this._maxPage = value;
  }

  public bool UserPageRangeEnabled
  {
    get => this._userPageRangeEnabled;
    set => this._userPageRangeEnabled = value;
  }

  private class TicketEventHandler
  {
    private PrintTicket mPrintTicket;

    public TicketEventHandler(PrintTicket printTicket) => this.mPrintTicket = printTicket;

    public void SetPrintTicket(object sender, WritingPrintTicketRequiredEventArgs args)
    {
      if (args.CurrentPrintTicketLevel != PrintTicketLevel.FixedDocumentSequencePrintTicket)
        return;
      args.CurrentPrintTicket = this.mPrintTicket;
    }
  }
}
