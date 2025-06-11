// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Printer.WinPrinterProgress
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using pdfeditor.Utils.Printer;
using PDFKit;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Printer;

public partial class WinPrinterProgress : Window, IComponentConnector
{
  private PrintArgs printArgs;
  private readonly bool printAnnotations;
  private readonly StandardPrintController printController = new StandardPrintController();
  private double printedCount;
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double), typeof (WinPrinterProgress), new PropertyMetadata((object) 0.0));
  internal TextBlock DocumentTitle;
  internal CommomLib.Controls.ProgressBar ProgressBar;
  internal TextBlock ProgressText;
  internal Button CancelButton;
  private bool _contentLoaded;

  public WinPrinterProgress(PrintArgs args, bool printAnnotations = true)
  {
    WinPrinterProgress winPrinterProgress = this;
    this.InitializeComponent();
    this.printArgs = args;
    this.printAnnotations = printAnnotations;
    if (!string.IsNullOrEmpty(args.DocumentTitle))
    {
      this.DocumentTitle.Visibility = Visibility.Visible;
      this.DocumentTitle.Text = args.DocumentTitle;
    }
    Task.Run((Action) (() => winPrinterProgress.SilentPrinting(args)));
  }

  private async Task<bool> SilentPrinting(PrintArgs args)
  {
    WinPrinterProgress winPrinterProgress = this;
    try
    {
      await Task.Run((Action) (() =>
      {
        using (PdfPrintDocument pdfPrintDocument = new PdfPrintDocument(args.Document, isAnnotationVisible: this.printAnnotations))
        {
          pdfPrintDocument.PrinterSettings.PrinterName = args.PrinterName;
          PageSettings pageSettings = new PageSettings(pdfPrintDocument.PrinterSettings);
          pdfPrintDocument.DefaultPageSettings = pageSettings;
          pdfPrintDocument.PrinterSettings.SetHdevmode(args.PrintDevMode);
          pdfPrintDocument.AutoRotate = args.AutoRotate;
          pdfPrintDocument.AutoCenter = args.AutoCenter;
          pdfPrintDocument.PrinterSettings.Copies = (short) args.Copies;
          pdfPrintDocument.PrintSizeMode = args.PrintSizeMode;
          pdfPrintDocument.Scale = args.Scale;
          pdfPrintDocument.PrinterSettings.Collate = args.Collate;
          pdfPrintDocument.PrinterSettings.Duplex = args.Duplex;
          if (args.Grayscale)
            pdfPrintDocument.RenderFlags |= RenderFlags.FPDF_GRAYSCALE;
          pageSettings.PaperSize = args.PaperSize;
          pageSettings.Landscape = args.Landscape;
          pageSettings.Color = !args.Grayscale;
          pdfPrintDocument.PrintController = (PrintController) this.printController;
          pdfPrintDocument.PrintPage += new PrintPageEventHandler(this.PrintPage);
          pdfPrintDocument.EndPrint += new PrintEventHandler(this.EndPrint);
          pdfPrintDocument.Print();
        }
      }));
      return true;
    }
    catch (Exception ex)
    {
      CommomLib.Commom.GAManager.SendEvent("Exception", "PrintBtn", ex.Message, 1L);
      winPrinterProgress.Dispatcher.Invoke((Action) (() => this.Close()));
      return false;
    }
    finally
    {
      if (args.IsTempDocument)
      {
        try
        {
          args.Document.Dispose();
          args.Document = (PdfDocument) null;
        }
        catch
        {
        }
      }
    }
  }

  private void EndPrint(object sender, PrintEventArgs e)
  {
    this.Dispatcher.Invoke((Action) (() =>
    {
      this.Value = 1.0;
      this.Close();
    }));
  }

  private void PrintPage(object sender, PrintPageEventArgs e)
  {
    this.Dispatcher.Invoke((Action) (() =>
    {
      ++this.printedCount;
      this.ProgressText.Text = $"{this.printedCount} / {this.printArgs.AllCount}";
      this.Value = this.printedCount / Convert.ToDouble(this.printArgs.AllCount);
    }));
  }

  public double Value
  {
    get => (double) this.GetValue(WinPrinterProgress.ValueProperty);
    set => this.SetValue(WinPrinterProgress.ValueProperty, (object) value);
  }

  private void Window_MouseMove(object sender, MouseEventArgs e)
  {
    if (e.LeftButton != MouseButtonState.Pressed)
      return;
    this.DragMove();
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e)
  {
    if (CommomLib.Commom.ModernMessageBox.Show((Window) this, pdfeditor.Properties.Resources.WinPrinterCancelPrintContent, UtilManager.GetProductName(), MessageBoxButton.YesNo, isButtonReversed: true) != MessageBoxResult.Yes)
      return;
    CommomLib.Commom.GAManager.SendEvent("PdfPrintDocument", "CancelBtnClick", "Count", 1L);
    PdfPrintDocument.IsCancel = true;
    this.Close();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/printer/winprinterprogress.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        ((UIElement) target).MouseMove += new MouseEventHandler(this.Window_MouseMove);
        break;
      case 2:
        this.DocumentTitle = (TextBlock) target;
        break;
      case 3:
        this.ProgressBar = (CommomLib.Controls.ProgressBar) target;
        break;
      case 4:
        this.ProgressText = (TextBlock) target;
        break;
      case 5:
        this.CancelButton = (Button) target;
        this.CancelButton.Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
