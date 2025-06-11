// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Printer.PrintPreviewControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Controls;
using LruCacheNet;
using Microsoft.Toolkit.Mvvm.Input;
using Patagames.Pdf.Enums;
using pdfeditor.Utils.Printer;
using PDFKit;
using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Controls.Printer;

public partial class PrintPreviewControl : UserControl, IComponentConnector
{
  private PreviewPageInfo[] pageInfo;
  private CancellationTokenSource cts;
  private bool innerSet;
  private LruCache<int, (BitmapSource bitmap, double width, double height)> thumbnailCache;
  private ConcurrentDictionary<PrintArgs, int> printingArgs;
  private bool printing;
  private static readonly DependencyPropertyKey TotalPagePropertyKey = DependencyProperty.RegisterReadOnly(nameof (TotalPage), typeof (int), typeof (PrintPreviewControl), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(nameof (CurrentPage), typeof (int), typeof (PrintPreviewControl), new PropertyMetadata((object) -1, new PropertyChangedCallback(PrintPreviewControl.OnCurrentPageChanged)));
  public static readonly DependencyProperty PrintArgsProperty = DependencyProperty.Register(nameof (PrintArgs), typeof (PrintArgs), typeof (PrintPreviewControl), new PropertyMetadata((object) null, new PropertyChangedCallback(PrintPreviewControl.OnPrintArgsChanged)));
  public static readonly DependencyProperty UseAntiAliasProperty = DependencyProperty.Register(nameof (UseAntiAlias), typeof (bool), typeof (PrintPreviewControl), new PropertyMetadata((object) true));
  public static readonly DependencyPropertyKey NextPageCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof (NextPageCommand), typeof (ICommand), typeof (PrintPreviewControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyPropertyKey PrevPageCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof (PrevPageCommand), typeof (ICommand), typeof (PrintPreviewControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty PrintAnnotationsProperty = DependencyProperty.Register(nameof (PrintAnnotations), typeof (bool), typeof (PrintPreviewControl), new PropertyMetadata((object) true, new PropertyChangedCallback(PrintPreviewControl.OnPrintAnnotationPropertyChanged)));
  internal Grid LayoutRoot;
  internal CheckBox _PrintAnnotCheckBox;
  internal Grid _ImageContainer;
  internal ProgressRing _LoadingProgressRing;
  internal System.Windows.Controls.Image _Image;
  private bool _contentLoaded;

  public PrintPreviewControl()
  {
    this.InitializeComponent();
    this.thumbnailCache = new LruCache<int, (BitmapSource, double, double)>(5);
    this.NextPageCommand = (ICommand) new RelayCommand((Action) (() =>
    {
      if (this.CurrentPage + 1 <= this.TotalPage)
        ++this.CurrentPage;
      ((RelayCommand) this.PrevPageCommand).NotifyCanExecuteChanged();
    }), (Func<bool>) (() => this.CurrentPage + 1 <= this.TotalPage));
    this.PrevPageCommand = (ICommand) new RelayCommand((Action) (() =>
    {
      if (this.CurrentPage - 1 > 0)
        --this.CurrentPage;
      ((RelayCommand) this.NextPageCommand).NotifyCanExecuteChanged();
    }), (Func<bool>) (() => this.CurrentPage - 1 > 0));
    this.printingArgs = new ConcurrentDictionary<PrintArgs, int>();
    this.Unloaded += new RoutedEventHandler(this.PrintPreviewControl_Unloaded);
  }

  public int TotalPage
  {
    get => (int) this.GetValue(PrintPreviewControl.TotalPageProperty);
    private set => this.SetValue(PrintPreviewControl.TotalPagePropertyKey, (object) value);
  }

  public static DependencyProperty TotalPageProperty
  {
    get => PrintPreviewControl.TotalPagePropertyKey.DependencyProperty;
  }

  public int CurrentPage
  {
    get => (int) this.GetValue(PrintPreviewControl.CurrentPageProperty);
    set => this.SetValue(PrintPreviewControl.CurrentPageProperty, (object) value);
  }

  private static async void OnCurrentPageChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is PrintPreviewControl printPreviewControl) || object.Equals(e.NewValue, e.OldValue))
      return;
    if (e.NewValue is int newValue && printPreviewControl.TotalPage != 0 && (newValue <= 0 || newValue > printPreviewControl.TotalPage))
    {
      printPreviewControl.CurrentPage = (int) e.OldValue;
    }
    else
    {
      ((RelayCommand) printPreviewControl.NextPageCommand).NotifyCanExecuteChanged();
      ((RelayCommand) printPreviewControl.PrevPageCommand).NotifyCanExecuteChanged();
      if (printPreviewControl.innerSet)
        return;
      await printPreviewControl.UpdateImageAsync();
    }
  }

  public PrintArgs PrintArgs
  {
    get => (PrintArgs) this.GetValue(PrintPreviewControl.PrintArgsProperty);
    set => this.SetValue(PrintPreviewControl.PrintArgsProperty, (object) value);
  }

  private static async void OnPrintArgsChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is PrintPreviewControl printPreviewControl) || e.NewValue == e.OldValue)
      return;
    if (e.OldValue is PrintArgs oldValue)
      printPreviewControl.PrintArgsReleaseRef(oldValue);
    if (!(e.NewValue is PrintArgs newValue))
      return;
    printPreviewControl.PrintArgsAddRef(newValue);
    printPreviewControl.TotalPage = newValue.AllCount;
    printPreviewControl.innerSet = true;
    printPreviewControl.CurrentPage = 1;
    printPreviewControl.innerSet = false;
    lock (printPreviewControl.thumbnailCache)
      printPreviewControl.thumbnailCache.Clear();
    ((RelayCommand) printPreviewControl.NextPageCommand).NotifyCanExecuteChanged();
    ((RelayCommand) printPreviewControl.PrevPageCommand).NotifyCanExecuteChanged();
    await printPreviewControl.UpdateImageAsync();
  }

  public bool UseAntiAlias
  {
    get => (bool) this.GetValue(PrintPreviewControl.UseAntiAliasProperty);
    set => this.SetValue(PrintPreviewControl.UseAntiAliasProperty, (object) value);
  }

  public ICommand NextPageCommand
  {
    get => (ICommand) this.GetValue(PrintPreviewControl.NextPageCommandProperty);
    private set => this.SetValue(PrintPreviewControl.NextPageCommandPropertyKey, (object) value);
  }

  public static DependencyProperty NextPageCommandProperty
  {
    get => PrintPreviewControl.NextPageCommandPropertyKey.DependencyProperty;
  }

  public ICommand PrevPageCommand
  {
    get => (ICommand) this.GetValue(PrintPreviewControl.PrevPageCommandProperty);
    private set => this.SetValue(PrintPreviewControl.PrevPageCommandPropertyKey, (object) value);
  }

  public static DependencyProperty PrevPageCommandProperty
  {
    get => PrintPreviewControl.PrevPageCommandPropertyKey.DependencyProperty;
  }

  public bool PrintAnnotations
  {
    get => (bool) this.GetValue(PrintPreviewControl.PrintAnnotationsProperty);
    set => this.SetValue(PrintPreviewControl.PrintAnnotationsProperty, (object) value);
  }

  private static async void OnPrintAnnotationPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is PrintPreviewControl printPreviewControl) || object.Equals(e.NewValue, e.OldValue))
      return;
    lock (printPreviewControl.thumbnailCache)
      printPreviewControl.thumbnailCache.Clear();
    await printPreviewControl.UpdateImageAsync();
  }

  protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
  {
    base.OnDpiChanged(oldDpi, newDpi);
    lock (this.thumbnailCache)
      this.thumbnailCache.Clear();
    double width;
    double height;
    BitmapSource imageCore = this.GetImageCore(out width, out height);
    if (imageCore != null)
    {
      this._Image.Source = (ImageSource) imageCore;
      this._Image.Width = width;
      this._Image.Height = height;
      this._Image.Visibility = Visibility.Visible;
    }
    else
    {
      this._Image.Source = (ImageSource) null;
      this._Image.Visibility = Visibility.Collapsed;
    }
  }

  private async Task UpdateImageAsync()
  {
    int curPage = this.CurrentPage;
    BitmapSource bitmapSource = (BitmapSource) null;
    double width = 0.0;
    double height = 0.0;
    if (curPage >= 0 && curPage <= this.TotalPage)
    {
      lock (this.thumbnailCache)
      {
        (BitmapSource bitmap, double width, double height) data;
        if (this.thumbnailCache.TryGetValue(curPage, out data))
        {
          bitmapSource = data.bitmap;
          width = data.width;
          height = data.height;
        }
      }
      if (bitmapSource == null)
      {
        try
        {
          await this.ComputePreviewAsync();
        }
        catch
        {
          return;
        }
        bitmapSource = this.GetImageCore(out width, out height);
        lock (this.thumbnailCache)
          this.thumbnailCache[curPage] = (bitmapSource, width, height);
      }
    }
    if (bitmapSource != null)
    {
      this._Image.Source = (ImageSource) bitmapSource;
      this._Image.Width = width;
      this._Image.Height = height;
      this._Image.Visibility = Visibility.Visible;
    }
    else
    {
      this._Image.Source = (ImageSource) null;
      this._Image.Visibility = Visibility.Collapsed;
    }
  }

  private BitmapSource GetImageCore(out double width, out double height)
  {
    width = 0.0;
    height = 0.0;
    if (this.pageInfo != null && this.pageInfo.Length != 0)
    {
      PreviewPageInfo previewPageInfo = this.pageInfo[0];
      IntPtr num1 = IntPtr.Zero;
      try
      {
        System.Drawing.Size physicalSize = previewPageInfo.PhysicalSize;
        if (this._ImageContainer.ActualWidth == 0.0 || this._ImageContainer.ActualHeight == 0.0 || physicalSize.IsEmpty || physicalSize.Width == 0 || physicalSize.Height == 0)
          return (BitmapSource) null;
        double num2 = this._ImageContainer.ActualWidth;
        double num3 = num2 * (double) physicalSize.Height / (double) physicalSize.Width;
        if (num3 > this.ActualHeight)
        {
          num3 = this._ImageContainer.ActualHeight;
          num2 = num3 * (double) physicalSize.Width / (double) physicalSize.Height;
        }
        width = num2;
        height = num3;
        double pixelsPerDip = VisualTreeHelper.GetDpi((Visual) this._ImageContainer).PixelsPerDip;
        double a1 = num2 * pixelsPerDip;
        double a2 = num3 * pixelsPerDip;
        using (Bitmap bitmap = new Bitmap(physicalSize.Width * 2, physicalSize.Height * 2))
        {
          using (Graphics graphics = Graphics.FromImage((System.Drawing.Image) bitmap))
          {
            graphics.FillRectangle(System.Drawing.Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
            graphics.DrawImage(previewPageInfo.Image, 0, 0, bitmap.Width, bitmap.Height);
          }
          num1 = bitmap.GetHbitmap();
          return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(num1, IntPtr.Zero, new Int32Rect(0, 0, bitmap.Width, bitmap.Height), BitmapSizeOptions.FromWidthAndHeight((int) Math.Ceiling(a1), (int) Math.Ceiling(a2)));
        }
      }
      catch
      {
      }
      finally
      {
        if (num1 != IntPtr.Zero)
          PrintPreviewControl.DeleteObject(num1);
      }
    }
    return (BitmapSource) null;
  }

  private async Task ComputePreviewAsync()
  {
    this.cts?.Cancel();
    CancellationTokenSource cts = new CancellationTokenSource();
    this.cts = cts;
    PreviewPageInfo[] old = this.pageInfo;
    PrintArgs args = this.PrintArgs;
    this._Image.Visibility = Visibility.Collapsed;
    this._LoadingProgressRing.Visibility = Visibility.Visible;
    using (PrintDocument document = this.CreatePrintDocument(args))
    {
      int num;
      if (num != 0 && document == null)
      {
        this.pageInfo = new PreviewPageInfo[0];
      }
      else
      {
        try
        {
          this.PrintArgsAddRef(args);
          PrintController printController = document.PrintController;
          PreviewPrintController previewPrintController = new PreviewPrintController();
          previewPrintController.UseAntiAlias = this.UseAntiAlias;
          document.PrintController = (PrintController) previewPrintController;
          try
          {
            this.printing = true;
            document.BeginPrint += new PrintEventHandler(Document_BeginPrint);
            document.PrintPage += new PrintPageEventHandler(Document_PrintPage);
            await Task.Run((Action) (() =>
            {
              try
              {
                document.Print();
              }
              catch
              {
              }
            }), cts.Token);
            this.pageInfo = previewPrintController.GetPreviewPageInfo();
            document.PrintController = printController;
          }
          finally
          {
            this.printing = false;
            document.BeginPrint -= new PrintEventHandler(Document_BeginPrint);
            document.PrintPage -= new PrintPageEventHandler(Document_PrintPage);
            this.PrintArgsReleaseRef(args);
          }
          printController = (PrintController) null;
          previewPrintController = (PreviewPrintController) null;
        }
        catch
        {
          this.pageInfo = new PreviewPageInfo[0];
        }
      }
    }
    if (old != null)
    {
      foreach (PreviewPageInfo previewPageInfo in old)
        previewPageInfo.Image.Dispose();
    }
    cts.Token.ThrowIfCancellationRequested();
    this._LoadingProgressRing.Visibility = Visibility.Collapsed;
    old = (PreviewPageInfo[]) null;
    args = (PrintArgs) null;

    void Document_BeginPrint(object sender, PrintEventArgs e)
    {
      if (!cts.IsCancellationRequested)
        return;
      e.Cancel = true;
    }

    void Document_PrintPage(object sender, PrintPageEventArgs e)
    {
      if (!cts.IsCancellationRequested)
        return;
      e.Cancel = true;
    }
  }

  private PrintDocument CreatePrintDocument(PrintArgs args)
  {
    if (this.CurrentPage < 0 || this.TotalPage <= 0 || this.CurrentPage > this.TotalPage)
      return (PrintDocument) null;
    if (args == null || args.Document == null)
      return (PrintDocument) null;
    PdfPrintDocument printDocument = new PdfPrintDocument(args.Document, isAnnotationVisible: this.PrintAnnotations);
    printDocument.RenderFlags = RenderFlags.FPDF_ANNOT | RenderFlags.FPDF_LCD_TEXT;
    if (args.Grayscale)
      printDocument.RenderFlags |= RenderFlags.FPDF_GRAYSCALE;
    printDocument.PrinterSettings.PrinterName = args.PrinterName;
    PageSettings pageSettings = new PageSettings(printDocument.PrinterSettings);
    printDocument.DefaultPageSettings = pageSettings;
    printDocument.PrinterSettings.SetHdevmode(args.PrintDevMode);
    printDocument.AutoRotate = args.AutoRotate;
    printDocument.AutoCenter = args.AutoCenter;
    printDocument.PrintSizeMode = args.PrintSizeMode;
    printDocument.Scale = args.Scale;
    printDocument.PrinterSettings.PrintRange = PrintRange.SomePages;
    printDocument.PrinterSettings.FromPage = this.CurrentPage;
    printDocument.PrinterSettings.ToPage = this.CurrentPage;
    pageSettings.PaperSize = args.PaperSize;
    pageSettings.Landscape = args.Landscape;
    pageSettings.Color = !args.Grayscale;
    return (PrintDocument) printDocument;
  }

  private void PrintPreviewControl_Unloaded(object sender, RoutedEventArgs e)
  {
    this.thumbnailCache.Clear();
    this._Image.Source = (ImageSource) null;
    PreviewPageInfo[] pageInfo = this.pageInfo;
    this.pageInfo = (PreviewPageInfo[]) null;
    if (pageInfo == null)
      return;
    foreach (PreviewPageInfo previewPageInfo in pageInfo)
      previewPageInfo.Image.Dispose();
  }

  private void PrintArgsAddRef(PrintArgs args)
  {
    if (args == null)
      return;
    lock (this.printingArgs)
    {
      int num;
      if (this.printingArgs.TryGetValue(args, out num))
        this.printingArgs[args] = num + 1;
      else
        this.printingArgs[args] = 1;
    }
  }

  private void PrintArgsReleaseRef(PrintArgs args)
  {
    if (args == null)
      return;
    lock (this.printingArgs)
    {
      int num1;
      if (!this.printingArgs.TryGetValue(args, out num1))
        return;
      int num2 = num1 - 1;
      if (num2 == 0)
      {
        this.printingArgs.TryRemove(args, out int _);
        if (!args.IsTempDocument)
          return;
        args.Document.Dispose();
      }
      else
        this.printingArgs[args] = num2;
    }
  }

  [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool DeleteObject(IntPtr hObject);

  private async void _PrintAnnotCheckBox_Click(object sender, RoutedEventArgs e)
  {
    await Task.Yield();
    CommomLib.Commom.GAManager.SendEvent("PrintAnnotations", (sender as CheckBox).IsChecked.GetValueOrDefault() ? "Checked" : "Unchecked", "Count", 1L);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/printer/printpreviewcontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.LayoutRoot = (Grid) target;
        break;
      case 2:
        this._PrintAnnotCheckBox = (CheckBox) target;
        break;
      case 3:
        this._ImageContainer = (Grid) target;
        break;
      case 4:
        this._LoadingProgressRing = (ProgressRing) target;
        break;
      case 5:
        this._Image = (System.Windows.Controls.Image) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
