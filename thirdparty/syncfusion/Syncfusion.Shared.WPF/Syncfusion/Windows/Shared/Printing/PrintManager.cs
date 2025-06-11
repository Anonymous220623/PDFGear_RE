// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.PrintManager
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public class PrintManager : INotifyPropertyChanged, IDisposable
{
  private int pagecount;
  private bool isdisposed;
  private double printPageHeaderHeight;
  private double printPageFooterHeight;
  private PageSizeUnit _unit;
  private double printPageHeight = 1122.52;
  private double printPageWidth = 793.7;
  private DataTemplate printPageHeaderTemplate;
  private DataTemplate printPageFooterTemplate;
  private Thickness printPageMargin = new Thickness(94.49);
  private PrintOrientation printPageOrientation;
  private int selectedScaleIndex;
  private Collation selectedCollation = Collation.Collated;
  private PageRangeSelection selectedPageRange;
  private int _copiesCount = 1;
  private int _frompage = 1;
  private int _topage = 1;
  internal PrintDialog dialog;
  internal bool NeedToChangeDefaultPrinter = true;
  private PrintQueueOption selectedPrinter;
  private List<PrintQueueOption> printers;
  private List<PrintPageSize> pageSizeOptions;
  private PrintOptionsControl printOptionsControl;
  protected internal bool isSuspended;
  internal Action InValidatePreviewPanel;
  protected internal string SelectedPageMediaName = string.Empty;
  protected internal bool isPagesInitialized;
  private PrintSettingsBase _printSettings;

  internal Dictionary<PrintPageSize, Tuple<string, int, int>> printQueueCapablitySizes { get; set; }

  protected internal PrintSettingsBase PrintSettings
  {
    get
    {
      if (this._printSettings == null)
        this._printSettings = new PrintSettingsBase();
      return this._printSettings;
    }
  }

  protected internal List<PrintPageControl> Pages { get; internal set; }

  internal PrintOptionsControl PrintOptionsControl
  {
    get => this.printOptionsControl;
    set
    {
      this.printOptionsControl = value;
      if (value == null)
        return;
      this.OnPrintOptionControlChanged();
    }
  }

  public PrintManager(PrintSettingsBase settings)
  {
    if (this.Pages == null)
      this.Pages = new List<PrintPageControl>();
    this.dialog = new PrintDialog();
    this.Printers = new List<PrintQueueOption>();
    List<PrintPageSize> pageSizeOptions = new List<PrintPageSize>();
    this.printQueueCapablitySizes = this.GetPrinterSupportedPaperSizes(out pageSizeOptions, this.PageSizeDisplayUnit);
    this.PageSizeOptions = pageSizeOptions;
    try
    {
      PrintQueueCollection printQueues = new LocalPrintServer().GetPrintQueues(new EnumeratedPrintQueueTypes[2]
      {
        EnumeratedPrintQueueTypes.Local,
        EnumeratedPrintQueueTypes.Connections
      });
      foreach (PrintQueue printQueue in printQueues.Reverse<PrintQueue>())
      {
        PrintQueueOption printQueueOption1 = new PrintQueueOption()
        {
          PrintQueue = printQueue
        };
        if (printQueue.Description.Contains("Fax"))
        {
          PrintQueueOption printQueueOption2 = printQueueOption1;
          Path path1 = new Path();
          path1.Data = Geometry.Parse("M3,16 L3,17 19,17 19,16 z M20.5,11 C20.223999,11 20,11.223999 20,11.5 20,11.776001 20.223999,12 20.5,12 20.776001,12 21,11.776001 21,11.5 21,11.223999 20.776001,11 20.5,11 z M2,6.9999999 L3,6.9999999 3,7.9999999 3,9 3,10 19,10 19,9 19,7.9999999 19,6.9999999 20,6.9999999 C21.104,7 22,7.8959999 22,9 L22,18 C22,19.104 21.104,20 20,20 L2,20 C0.89599991,20 0,19.104 0,18 L0,9 C0,7.8959999 0.89599991,7 2,6.9999999 z M7,4.9999998 L15,4.9999998 15,5.9999999 7,5.9999999 z M7,3 L15,3 15,4.0000002 7,4.0000002 z M5,1 L5,7.9999999 17,7.9999999 17,1 z M4,0 L18,0 18,9 4,9 z");
          path1.Height = 20.0;
          path1.Width = 22.0;
          Path path2 = path1;
          printQueueOption2.ImagePath = path2;
        }
        else
        {
          PrintQueueOption printQueueOption3 = printQueueOption1;
          Path path3 = new Path();
          path3.Data = Geometry.Parse("M6.9999998,20 L15,20 15,21 6.9999998,21 z M6.9999998,18 L15,18 15,19 6.9999998,19 z M4.9999998,16 L4.9999998,23 17,23 17,16 z M3.9999998,15 L18,15 18,24 3.9999998,24 z M20.5,8.0000002 C20.223999,8 20,8.223999 20,8.5 20,8.776001 20.223999,9 20.5,8.9999995 20.776001,9 21,8.776001 21,8.5 21,8.223999 20.776001,8 20.5,8.0000002 z M1.9999998,5.0000002 L3,5.0000002 3,6 3,7 3,8.0000002 19,8.0000002 19,7 19,6 19,5.0000002 19.994003,5.0000002 C21.102997,5 22,5.8980007 22,7.0050011 L22,15 C22,16.104 21.103996,17 20,17 L19,17 19,16 19,14 3,14 3,16 3,17 2.0050009,17 C0.8979986,17 -2.086162E-07,16.103001 4.2632564E-14,14.993999 L4.2632564E-14,7 C-2.086162E-07,5.8959999 0.8959997,5 1.9999998,5.0000002 z M4.9999998,1 L4.9999998,6 17,6 17,1 z M3.9999998,0 L18,0 18,7 3.9999998,7 z");
          path3.Height = 24.0;
          path3.Width = 22.0;
          Path path4 = path3;
          printQueueOption3.ImagePath = path4;
        }
        this.Printers.Add(printQueueOption1);
        if (this.dialog.PrintQueue == null && printQueues.Count<PrintQueue>() > 0 && printQueueOption1.PrintQueue == printQueues.ToList<PrintQueue>()[0])
          this.SelectedPrinter = printQueueOption1;
        if (this.dialog.PrintQueue != null && this.dialog.PrintQueue.FullName != null && this.dialog.PrintQueue.FullName.Equals(printQueue.FullName))
        {
          this.SelectedPrinter = printQueueOption1;
          printQueueOption1.IsDefault = true;
        }
      }
    }
    catch
    {
    }
    this._printSettings = settings;
  }

  public int SelectedScaleIndex
  {
    get => this.selectedScaleIndex;
    set
    {
      if (this.selectedScaleIndex == value)
        return;
      this.selectedScaleIndex = value;
      this.OnPrintPropertyChanged();
      this.OnPropertyChanged("selectedScaleIndex");
    }
  }

  public PageSizeUnit PageSizeDisplayUnit
  {
    get => this._unit;
    set
    {
      if (this._unit == value)
        return;
      this._unit = value;
      this.OnPropertyChanged(nameof (PageSizeDisplayUnit));
    }
  }

  public int PageCount
  {
    get => this.pagecount;
    protected set => this.pagecount = value;
  }

  public Thickness PageMargin
  {
    get => this.printPageMargin;
    set
    {
      if (this.printPageMargin == value)
        return;
      this.printPageMargin = value;
      this.OnPrintPropertyChanged();
      this.OnPropertyChanged(nameof (PageMargin));
    }
  }

  public double PageHeight
  {
    get => this.printPageHeight;
    set
    {
      if (this.printPageHeight == value)
        return;
      this.printPageHeight = value;
      this.OnPrintPropertyChanged();
      this.OnPropertyChanged(nameof (PageHeight));
    }
  }

  public double PageWidth
  {
    get => this.printPageWidth;
    set
    {
      if (this.printPageWidth == value)
        return;
      this.printPageWidth = value;
      this.OnPrintPropertyChanged();
      this.OnPropertyChanged(nameof (PageWidth));
    }
  }

  public double PageHeaderHeight
  {
    get => this.printPageHeaderHeight;
    set
    {
      if (this.printPageHeaderHeight == value)
        return;
      this.printPageHeaderHeight = value;
      this.OnPrintPropertyChanged();
      this.OnPropertyChanged(nameof (PageHeaderHeight));
    }
  }

  public double PageFooterHeight
  {
    get => this.printPageFooterHeight;
    set
    {
      if (this.printPageFooterHeight == value)
        return;
      this.printPageFooterHeight = value;
      this.OnPrintPropertyChanged();
      this.OnPropertyChanged(nameof (PageFooterHeight));
    }
  }

  public DataTemplate PageHeaderTemplate
  {
    get => this.printPageHeaderTemplate;
    set
    {
      if (this.printPageHeaderTemplate == value)
        return;
      this.printPageHeaderTemplate = value;
      this.OnPrintPropertyChanged();
      this.OnPropertyChanged(nameof (PageHeaderTemplate));
    }
  }

  public DataTemplate PageFooterTemplate
  {
    get => this.printPageFooterTemplate;
    set
    {
      if (this.printPageFooterTemplate == value)
        return;
      this.printPageFooterTemplate = value;
      this.OnPrintPropertyChanged();
      this.OnPropertyChanged(nameof (PageFooterTemplate));
    }
  }

  public Collation Collation
  {
    get => this.selectedCollation;
    internal set
    {
      if (this.selectedCollation == value)
        return;
      this.selectedCollation = value;
      this.dialog.PrintTicket.Collation = new Collation?(value);
      this.OnPropertyChanged(nameof (Collation));
    }
  }

  public PageRangeSelection PageRangeSelection
  {
    get => this.selectedPageRange;
    internal set
    {
      if (this.selectedPageRange == value)
        return;
      this.selectedPageRange = value;
      this.dialog.PageRangeSelection = value;
      this.OnPropertyChanged(nameof (PageRangeSelection));
    }
  }

  public int CopiesCount
  {
    get => this._copiesCount;
    internal set
    {
      if (this._copiesCount == value)
        return;
      this._copiesCount = value;
      this.dialog.PrintTicket.CopyCount = new int?(value);
      this.OnPropertyChanged(nameof (CopiesCount));
    }
  }

  public int FromPage
  {
    get => this._frompage;
    internal set
    {
      if (this._frompage == value)
        return;
      this._frompage = value;
      if (this.FromPage != 0 && this.ToPage != 0)
        this.dialog.PageRange = new PageRange(this.FromPage = value, this.ToPage = this.ToPage);
      this.OnPropertyChanged(nameof (FromPage));
    }
  }

  public int ToPage
  {
    get => this._topage;
    internal set
    {
      if (this._topage == value)
        return;
      this._topage = value;
      if (this.FromPage != 0 && this.ToPage != 0)
        this.dialog.PageRange = new PageRange(this.FromPage = this.FromPage, this.ToPage = value);
      this.OnPropertyChanged(nameof (ToPage));
    }
  }

  public PrintOrientation PageOrientation
  {
    get => this.printPageOrientation;
    set
    {
      if (this.printPageOrientation == value)
        return;
      this.printPageOrientation = value;
      this.OnPrintPropertyChanged();
      this.OnPropertyChanged("Orientation");
    }
  }

  public List<PrintQueueOption> Printers
  {
    get => this.printers;
    internal set
    {
      if (this.printers == value)
        return;
      this.printers = value;
      this.OnPropertyChanged(nameof (Printers));
    }
  }

  public PrintQueueOption SelectedPrinter
  {
    get => this.selectedPrinter;
    internal set
    {
      if (this.selectedPrinter == value)
        return;
      this.selectedPrinter = value;
      this.OnPropertyChanged(nameof (SelectedPrinter));
      this.dialog.PrintQueue = value.PrintQueue;
      this.OnSelectedPrintQueueChanged();
    }
  }

  public List<PrintPageSize> PageSizeOptions
  {
    get => this.pageSizeOptions;
    internal set
    {
      if (this.pageSizeOptions == value)
        return;
      this.pageSizeOptions = value;
      this.OnPropertyChanged(nameof (PageSizeOptions));
    }
  }

  protected internal void OnPrintPropertyChanged()
  {
    if (this.isSuspended || this.InValidatePreviewPanel == null)
      return;
    this.InValidatePreviewPanel();
  }

  public void Print()
  {
    this.InitializePrint();
    if (this.PageCount != 0)
    {
      if (this.PageRangeSelection == PageRangeSelection.AllPages)
      {
        this.FromPage = 1;
        this.ToPage = this.PageCount;
      }
    }
    try
    {
      PrintDialog printDialog1 = new PrintDialog()
      {
        PrintQueue = this.dialog.PrintQueue,
        PrintTicket = this.GetCustomizedPrintTicket(new Size?(), false)
      };
      printDialog1.PrintTicket.PageOrientation = new System.Printing.PageOrientation?(this.PageOrientation == PrintOrientation.Landscape ? System.Printing.PageOrientation.Landscape : System.Printing.PageOrientation.Portrait);
      PrintDialog printDialog2 = printDialog1;
      PageRange pageRange;
      if (!(this.dialog.PageRange == new PageRange()
      {
        PageFrom = 0,
        PageTo = 0
      }))
        pageRange = this.dialog.PageRange;
      else
        pageRange = new PageRange()
        {
          PageFrom = this.FromPage,
          PageTo = this.ToPage
        };
      printDialog2.PageRange = pageRange;
      printDialog1.PageRangeSelection = this.dialog.PageRangeSelection;
      printDialog1.PrintTicket.CopyCount = this.dialog.PrintTicket.CopyCount;
      printDialog1.PrintTicket.Collation = this.dialog.PrintTicket.Collation;
      this.Print(printDialog1, false);
    }
    catch
    {
    }
  }

  public virtual void Print(PrintDialog printDialog, bool showPrintDialog)
  {
    if (printDialog == null)
      throw new ArgumentNullException(nameof (printDialog), "printDialog can't be null");
    this.InitializePrint();
    if (showPrintDialog)
      this.PrintDocument(printDialog, printDialog.ShowDialog());
    else
      this.PrintDocument(printDialog, new bool?(true));
  }

  protected virtual void PrintDocument(PrintDialog printDialog, bool? canPrint)
  {
    try
    {
      if (printDialog == null)
        printDialog = new PrintDialog();
      printDialog.PrintTicket.PageMediaSize = this.PageOrientation != PrintOrientation.Landscape ? new PageMediaSize(this.GetPageWidth(), this.GetPageHeight()) : new PageMediaSize(this.GetPageHeight(), this.GetPageWidth());
      if (canPrint.HasValue && canPrint.Value)
      {
        FixedDocument fixedDocument = new FixedDocument();
        int num1 = 1;
        int num2 = this.PageCount;
        if (printDialog.PageRange.PageFrom != 0 && printDialog.PageRange.PageTo != 0)
        {
          num1 = printDialog.PageRange.PageFrom;
          num2 = printDialog.PageRange.PageTo > num2 ? num2 : printDialog.PageRange.PageTo;
        }
        for (int i = num1; i <= num2; ++i)
        {
          PrintPageControl element = this.Pages.FirstOrDefault<PrintPageControl>((Func<PrintPageControl, bool>) (x => x.PageIndex == i)) ?? this.CreatePage(i);
          PageContent newPageContent = new PageContent();
          FixedPage fixedPage1 = new FixedPage();
          fixedPage1.Height = this.PageHeight;
          fixedPage1.Width = this.PageWidth;
          fixedPage1.PrintTicket = (object) printDialog.PrintTicket;
          FixedPage fixedPage2 = fixedPage1;
          fixedPage2.Children.Add((UIElement) element);
          ((IAddChild) newPageContent).AddChild((object) fixedPage2);
          fixedDocument.Pages.Add(newPageContent);
        }
        printDialog.PrintDocument(fixedDocument.DocumentPaginator, "Printing");
        foreach (PageContent page in fixedDocument.Pages)
          page.Child.Children.Clear();
      }
      else
        this.Pages.Clear();
    }
    catch
    {
    }
  }

  protected internal virtual PrintPageControl CreatePage(int pageIndex)
  {
    int pageIndex1 = pageIndex;
    PrintPageControl printPageControl = new PrintPageControl(this);
    printPageControl.PageIndex = pageIndex;
    printPageControl.DataContext = (object) this;
    PrintPageControl pageControl = printPageControl;
    return this.CreatePage(pageIndex1, pageControl);
  }

  protected internal virtual PrintPageControl CreatePage(
    int pageIndex,
    PrintPageControl pageControl)
  {
    pageControl.PageIndex = pageIndex;
    return pageControl;
  }

  internal double GetPageWidth()
  {
    return this.PageOrientation == PrintOrientation.Portrait && this.PageHeight < this.PageWidth || this.PageOrientation == PrintOrientation.Landscape && this.PageHeight > this.PageWidth ? this.PageHeight : this.PageWidth;
  }

  internal double GetPageHeight()
  {
    return this.PageOrientation == PrintOrientation.Portrait && this.PageHeight < this.PageWidth || this.PageOrientation == PrintOrientation.Landscape && this.PageHeight > this.PageWidth ? this.PageWidth : this.PageHeight;
  }

  internal virtual void InitializePrint()
  {
    this.isSuspended = true;
    if (!this.isPagesInitialized && this.PrintSettings != null)
    {
      this.isPagesInitialized = true;
      this.InitializeProperties();
    }
    if (this.PageOrientation == PrintOrientation.Portrait && this.PageHeight < this.PageWidth || this.PageOrientation == PrintOrientation.Landscape && this.PageHeight > this.PageWidth)
    {
      double pageWidth = this.PageWidth;
      this.PageWidth = this.PageHeight;
      this.PageHeight = pageWidth;
    }
    this.isSuspended = false;
    this.ComputePageCount();
  }

  protected virtual void ComputePageCount()
  {
  }

  protected virtual void InitializeProperties()
  {
    this.PageHeaderHeight = this.PrintSettings.PageHeaderHeight;
    this.PageFooterHeight = this.PrintSettings.PageFooterHeight;
    this.PageFooterTemplate = this.PrintSettings.PageFooterTemplate;
    this.PageHeaderTemplate = this.PrintSettings.PageHeaderTemplate;
    this.isSuspended = true;
    this.PageHeight = this.PrintSettings.PageHeight;
    this.PageWidth = this.PrintSettings.PageWidth;
    this.isSuspended = false;
    this.PageMargin = this.PrintSettings.PageMargin;
    this.PageOrientation = this.PrintSettings.Orientation;
    if (this.dialog.PrintQueue == null)
      return;
    this.OnSelectedPrinterChanged(this.dialog.PrintQueue);
  }

  public virtual FixedDocument GetPrintDocument()
  {
    this.InitializePrint();
    return this.GetPrintDocument(1, this.PageCount);
  }

  public virtual FixedDocument GetPrintDocument(int start, int end)
  {
    if (start < 1 || end > this.PageCount || end < 1)
      throw new ArgumentOutOfRangeException("Start Index and End Index must be greater than  1 and less than  or equal to" + (object) this.PageCount);
    FixedDocument printDocument = new FixedDocument();
    for (int i = start; i <= end; ++i)
    {
      PrintPageControl element = this.Pages.FirstOrDefault<PrintPageControl>((Func<PrintPageControl, bool>) (x => x.PageIndex == i)) ?? this.CreatePage(i);
      PageContent newPageContent = new PageContent();
      FixedPage fixedPage1 = new FixedPage();
      fixedPage1.Height = this.PageHeight;
      fixedPage1.Width = this.PageWidth;
      FixedPage fixedPage2 = fixedPage1;
      fixedPage2.Children.Add((UIElement) element);
      ((IAddChild) newPageContent).AddChild((object) fixedPage2);
      printDocument.Pages.Add(newPageContent);
    }
    return printDocument;
  }

  public virtual void OnSelectedPrinterChanged(PrintQueue printQueue)
  {
  }

  protected internal virtual void OnPrintMarginChanged(Thickness value) => this.PageMargin = value;

  protected internal virtual void OnPageSizeChanged(double width, double height)
  {
    this.isSuspended = true;
    this.PageWidth = width;
    this.PageHeight = height;
    this.isSuspended = false;
    if (this.InValidatePreviewPanel == null)
      return;
    this.InValidatePreviewPanel();
  }

  protected internal virtual void OnPrintOrientationChanged(PrintOrientation value)
  {
    if (this.isSuspended)
      return;
    this.isSuspended = true;
    if (value == PrintOrientation.Portrait && this.PageHeight < this.PageWidth || value == PrintOrientation.Landscape && this.PageHeight > this.PageWidth)
    {
      double pageWidth = this.PageWidth;
      this.PageWidth = this.PageHeight;
      this.PageHeight = pageWidth;
    }
    this.PageOrientation = value;
    this.isSuspended = false;
    if (this.InValidatePreviewPanel == null)
      return;
    this.InValidatePreviewPanel();
  }

  protected internal virtual List<PrintScaleInfo> GetScaleOptions() => (List<PrintScaleInfo>) null;

  protected internal virtual void OnPrintScaleOptionChanged(int index)
  {
  }

  internal void PrintWithDialog()
  {
    PrintDialog printDialog = new PrintDialog();
    try
    {
      if (this.dialog != null)
      {
        printDialog.PrintQueue = this.dialog.PrintQueue;
        printDialog.PrintTicket = this.GetCustomizedPrintTicket(new Size?(), false);
        printDialog.PrintTicket.PageOrientation = new System.Printing.PageOrientation?(this.PageOrientation == PrintOrientation.Landscape ? System.Printing.PageOrientation.Landscape : System.Printing.PageOrientation.Portrait);
      }
    }
    catch
    {
    }
    this.Print(printDialog, true);
  }

  private void OnSelectedPrintQueueChanged()
  {
    List<PrintPageSize> pageSizeOptions = new List<PrintPageSize>();
    this.printQueueCapablitySizes = this.GetPrinterSupportedPaperSizes(out pageSizeOptions, this.PageSizeDisplayUnit);
    this.PageSizeOptions = pageSizeOptions.ToList<PrintPageSize>();
    if (this.dialog.PrintQueue != null)
      this.OnSelectedPrinterChanged(this.dialog.PrintQueue);
    List<PrintPageSize> list = this.PageSizeOptions.Except<PrintPageSize>((IEnumerable<PrintPageSize>) pageSizeOptions).ToList<PrintPageSize>();
    if (list.Count<PrintPageSize>() > 0)
    {
      foreach (PrintPageSize key in list)
      {
        key.PageSizeUnit = this.PageSizeDisplayUnit;
        this.printQueueCapablitySizes.Add(key, new Tuple<string, int, int>(key.PageSizeName, (int) (key.Size.Width * 264.584), (int) (key.Size.Height * 264.584)));
      }
    }
    if (this.PrintOptionsControl != null)
      this.PrintOptionsControl.PageSizeOptions = this.PageSizeOptions;
    this.SelectDefaultPageMediaSize();
  }

  private void OnPrintOptionControlChanged()
  {
    List<PrintPageSize> pageSizeOptions = new List<PrintPageSize>();
    this.printQueueCapablitySizes = this.GetPrinterSupportedPaperSizes(out pageSizeOptions, this.PageSizeDisplayUnit);
    this.PageSizeOptions = pageSizeOptions.ToList<PrintPageSize>();
    this.printOptionsControl.Printers = this.Printers;
    this.printOptionsControl.SelectedPrinter = this.SelectedPrinter;
    if (this.dialog.PrintQueue != null)
      this.OnSelectedPrinterChanged(this.dialog.PrintQueue);
    List<PrintPageSize> list = this.PageSizeOptions.Except<PrintPageSize>((IEnumerable<PrintPageSize>) pageSizeOptions).ToList<PrintPageSize>();
    if (list.Count<PrintPageSize>() > 0)
    {
      foreach (PrintPageSize key in list)
      {
        key.PageSizeUnit = this.PageSizeDisplayUnit;
        this.printQueueCapablitySizes.Add(key, new Tuple<string, int, int>(key.PageSizeName, (int) (key.Size.Width * 264.584), (int) (key.Size.Height * 264.584)));
      }
    }
    this.printOptionsControl.PageSizeOptions = this.PageSizeOptions;
    double width = this.PageWidth;
    double height = this.PageHeight;
    if (this.PageOrientation == PrintOrientation.Landscape)
    {
      double num = width;
      width = height;
      height = num;
    }
    this.dialog.PrintTicket = this.GetCustomizedPrintTicket(new Size?(new Size(width, height)), true);
    this.SelectDefaultPageMediaSize();
  }

  public event PropertyChangedEventHandler PropertyChanged;

  protected virtual void OnPropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  protected virtual void Dispose(bool isDisposing)
  {
    if (this.isdisposed)
      return;
    if (isDisposing)
    {
      this.Pages.Clear();
      this.PrintOptionsControl = (PrintOptionsControl) null;
      this._printSettings = (PrintSettingsBase) null;
    }
    this.isdisposed = true;
  }
}
