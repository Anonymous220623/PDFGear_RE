// Decompiled with JetBrains decompiler
// Type: pdfeditor.Views.BatchPrintWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Controls;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using Patagames.Pdf.Net;
using pdfeditor.Controls;
using pdfeditor.Controls.Printer;
using pdfeditor.Models;
using pdfeditor.Models.Print;
using pdfeditor.Utils.Printer;
using PDFKit;
using PDFKit.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Views;

public partial class BatchPrintWindow : Window, IComponentConnector
{
  private ObservableCollection<BatchPrintDocumentModel> documents;
  private List<PaperSizeInfo> paperSizesLst = new List<PaperSizeInfo>();
  private List<PrinterInfo> printLst = new List<PrinterInfo>();
  private CancellationTokenSource closeTokenSource;
  private CancellationTokenSource updatePreviewCts;
  internal Grid LeftFileListContainer;
  internal Button AddDocumentButton;
  internal ListBox DocumentListBox;
  internal ComboBox cboxPrinterList;
  internal Button PreferenceBtn;
  internal NumberBox tboxCopies;
  internal CheckBox chkbPrintCollate;
  internal Image PrintCollateImage;
  internal CheckBox chkbRevertPrint;
  internal CheckBox GrayscaleCheckbox;
  internal RadioButton rdbtnAllPages;
  internal RadioButton rdbtnCurrentPage;
  internal RadioButton rdbtnSelectPage;
  internal TextBox tboxPageRang;
  internal TextBlock tbRangTip;
  internal TextBlock tbSubset;
  internal ComboBox cboxSubset;
  internal StackPanel DuplexPanel;
  internal CheckBox chkbDuplex;
  internal StackPanel DuplexRadioButtonPanel;
  internal RadioButton rdbtnDuplexVertical;
  internal RadioButton rdbtnDuplexHorizontal;
  internal ComboBox cboxPaperSize;
  internal RadioButton rdbtnPortrait;
  internal RadioButton rdbtnLandscape;
  internal RadioButton rdbtnFitPage;
  internal RadioButton rdbtnActualSize;
  internal RadioButton rdbtnScale;
  internal NumberBox tboxScaleUnit;
  internal TextBlock tbSymbol;
  internal PrintPreviewControl PreviewControl;
  internal Run FileCountRun;
  internal Run PagesCount;
  internal Button btnCacel;
  internal Button btnPrint;
  internal Grid ProgressBackground;
  private bool _contentLoaded;

  public BatchPrintWindow()
  {
    this.InitializeComponent();
    this.closeTokenSource = new CancellationTokenSource();
    this.documents = new ObservableCollection<BatchPrintDocumentModel>();
    this.DocumentListBox.ItemsSource = (IEnumerable) this.documents;
    this.GetPrinterList();
    this.UpdateTotalDocumentNumber();
  }

  public BatchPrintWindow(string fileName, PdfDocument doc)
    : this()
  {
    this.AddDocument(fileName, doc);
  }

  private void AddDocument(string fileName, PdfDocument doc)
  {
    this.AddDocumentCore(new BatchPrintDocumentModel(fileName, doc));
  }

  private void AddDocument(DocumentWrapper wrapper)
  {
    this.AddDocumentCore(new BatchPrintDocumentModel(wrapper));
  }

  private void AddDocumentCore(BatchPrintDocumentModel item)
  {
    this.documents.Add(item);
    WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler((INotifyPropertyChanged) item, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.OnDocumentPropertyChanged));
    item.RemoveCmd = (ICommand) new RelayCommand((Action) (() =>
    {
      CommomLib.Commom.GAManager.SendEvent("PdfBatchPrintDocument", "RemoveDocumentBtnClick", "Count", 1L);
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.RemoveHandler((INotifyPropertyChanged) item, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.OnDocumentPropertyChanged));
      this.documents.Remove(item);
      item.Dispose();
      this.UpdateTotalDocumentNumber();
    }));
    if (this.DocumentListBox.SelectedItem == null)
      this.DocumentListBox.SelectedItem = (object) item;
    this.UpdateTotalDocumentNumber();
  }

  private async Task AddDocumentFromFiles(string[] files)
  {
    BatchPrintWindow batchPrintWindow1 = this;
    if (files == null || files.Length == 0)
      return;
    string[] strArray = files;
    for (int index = 0; index < strArray.Length; ++index)
    {
      BatchPrintWindow batchPrintWindow = batchPrintWindow1;
      string fileName = strArray[index];
      if (batchPrintWindow1.closeTokenSource.IsCancellationRequested)
        return;
      try
      {
        if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
          DocumentWrapper wrapper = new DocumentWrapper();
          wrapper.PasswordRequested += (EventHandler<DocumentPasswordRequestedEventArgs>) ((s, a) =>
          {
            if (batchPrintWindow.closeTokenSource.IsCancellationRequested)
            {
              a.Cancel = true;
            }
            else
            {
              EnterPasswordDialog enterPasswordDialog = new EnterPasswordDialog(Path.GetFileName(fileName))
              {
                Owner = (Window) batchPrintWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
              };
              a.Cancel = !enterPasswordDialog.ShowDialog().GetValueOrDefault();
              a.Password = enterPasswordDialog.Password;
            }
          });
          if (await wrapper.OpenAsync(fileName))
          {
            if (batchPrintWindow1.closeTokenSource.IsCancellationRequested)
              return;
            batchPrintWindow1.AddDocument(wrapper);
          }
          wrapper = (DocumentWrapper) null;
        }
      }
      catch
      {
      }
    }
    strArray = (string[]) null;
  }

  private void OnDocumentPropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    this.UpdateTotalDocumentNumber();
  }

  private void UpdateTotalDocumentNumber()
  {
    this.FileCountRun.Text = $"{this.documents.Count}";
    this.PagesCount.Text = $"{this.documents.Sum<BatchPrintDocumentModel>((Func<BatchPrintDocumentModel, int>) (c => c.SelectedPageCount))}";
    this.UpdatePrintButtonState();
  }

  private void UpdatePrintButtonState()
  {
    this.btnPrint.IsEnabled = this.documents.Sum<BatchPrintDocumentModel>((Func<BatchPrintDocumentModel, int>) (c => c.SelectedPageCount)) > 0;
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    base.OnMouseDown(e);
    if (Keyboard.FocusedElement is FrameworkElement focusedElement)
    {
      TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
      focusedElement.MoveFocus(request);
    }
    Keyboard.ClearFocus();
  }

  private void GetPrinterList()
  {
    try
    {
      string sDefault = new PrinterSettings().PrinterName;
      foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
        this.printLst.Insert(0, new PrinterInfo()
        {
          PrinterName = installedPrinter
        });
      PrinterInfo printerInfo = this.printLst.Where<PrinterInfo>((Func<PrinterInfo, bool>) (p => p.PrinterName == sDefault)).FirstOrDefault<PrinterInfo>();
      if (printerInfo != null)
      {
        this.printLst.Remove(printerInfo);
        this.printLst.Insert(0, printerInfo);
      }
      if (this.printLst.Count == 0)
      {
        this.printLst.Add(new PrinterInfo()
        {
          PrinterName = pdfeditor.Properties.Resources.WinPrinterNoPrinterContent
        });
        this.btnPrint.IsEnabled = false;
      }
      this.cboxPrinterList.ItemsSource = (IEnumerable) this.printLst;
      this.cboxPrinterList.SelectedIndex = 0;
    }
    catch (Exception ex)
    {
    }
  }

  private PrintArgs GetPrintArgs()
  {
    PrinterInfo selectedItem = this.cboxPrinterList.SelectedItem as PrinterInfo;
    PrintArgs printArgs1 = new PrintArgs();
    printArgs1.Grayscale = this.GrayscaleCheckbox.IsChecked.GetValueOrDefault();
    printArgs1.PrinterName = selectedItem.PrinterName;
    printArgs1.AutoRotate = true;
    printArgs1.AutoCenter = true;
    printArgs1.Copies = (int) this.tboxCopies.Value;
    printArgs1.Collate = this.chkbPrintCollate.IsChecked.Value;
    printArgs1.Landscape = this.rdbtnLandscape.IsChecked.Value;
    printArgs1.PaperSize = ((PaperSizeInfo) this.cboxPaperSize.SelectedItem).PaperSize;
    printArgs1.PrintDevMode = selectedItem.PrintDevMode;
    bool? isChecked;
    int num;
    if (this.chkbDuplex.IsChecked.GetValueOrDefault())
    {
      isChecked = this.rdbtnDuplexVertical.IsChecked;
      num = !isChecked.GetValueOrDefault() ? 3 : 2;
    }
    else
      num = 1;
    printArgs1.Duplex = (Duplex) num;
    PrintArgs printArgs2 = printArgs1;
    isChecked = this.rdbtnFitPage.IsChecked;
    if (isChecked.GetValueOrDefault())
    {
      printArgs2.PrintSizeMode = PrintSizeMode.Fit;
    }
    else
    {
      isChecked = this.rdbtnActualSize.IsChecked;
      if (isChecked.GetValueOrDefault())
      {
        printArgs2.PrintSizeMode = PrintSizeMode.ActualSize;
      }
      else
      {
        printArgs2.PrintSizeMode = PrintSizeMode.CustomScale;
        printArgs2.Scale = (int) this.tboxScaleUnit.Value;
      }
    }
    return printArgs2;
  }

  private void ApplyPrintArgs(PrintArgs args)
  {
    if (args == null)
      return;
    this.GrayscaleCheckbox.IsChecked = new bool?(args.Grayscale);
    PrinterInfo printerInfo = this.printLst.FirstOrDefault<PrinterInfo>((Func<PrinterInfo, bool>) (c => c.PrinterName == args.PrinterName));
    this.cboxPrinterList.SelectedItem = (object) printerInfo ?? this.cboxPrinterList.SelectedItem ?? (object) this.printLst.FirstOrDefault<PrinterInfo>();
    if (printerInfo != null)
    {
      if (printerInfo.PrintDevMode != null)
      {
        printerInfo.PrintDevMode.Dispose();
        printerInfo.PrintDevMode = (PrintDevModeHandle) null;
      }
      printerInfo.PrintDevMode = args.PrintDevMode;
    }
    this.tboxCopies.Value = (double) args.Copies;
    this.chkbPrintCollate.IsChecked = new bool?(args.Collate);
    if (args.Landscape)
      this.rdbtnLandscape.IsChecked = new bool?(true);
    else
      this.rdbtnPortrait.IsChecked = new bool?(true);
    this.chkbDuplex.IsChecked = new bool?(args.Duplex != Duplex.Simplex);
    this.rdbtnDuplexVertical.IsChecked = new bool?(args.Duplex == Duplex.Vertical);
    this.rdbtnDuplexHorizontal.IsChecked = new bool?(args.Duplex != Duplex.Vertical);
    if (args.PrintSizeMode == PrintSizeMode.Fit)
      this.rdbtnFitPage.IsChecked = new bool?(true);
    else if (args.PrintSizeMode == PrintSizeMode.ActualSize)
    {
      this.rdbtnActualSize.IsChecked = new bool?(true);
    }
    else
    {
      this.rdbtnScale.IsChecked = new bool?(true);
      this.tboxScaleUnit.Value = (double) args.Scale;
    }
    this.UpdatePrintProperties(args.PaperSize);
  }

  private void UpdatePrintProperties(PaperSize paperSize = null)
  {
    if (!(this.cboxPrinterList.SelectedItem is PrinterInfo selectedItem))
      this.cboxPaperSize.IsEnabled = false;
    PrinterSettings printerSettings = new PrinterSettings()
    {
      PrinterName = selectedItem.PrinterName
    };
    PrinterSettings.PaperSizeCollection paperSizes = printerSettings.PaperSizes;
    if (paperSizes.Count > 0)
      this.cboxPaperSize.IsEnabled = true;
    this.paperSizesLst = paperSizes.OfType<PaperSize>().Select<PaperSize, PaperSizeInfo>((Func<PaperSize, PaperSizeInfo>) (c => new PaperSizeInfo()
    {
      FriendlyName = c.PaperName,
      PaperSize = c
    })).ToList<PaperSizeInfo>();
    this.cboxPaperSize.ItemsSource = (IEnumerable) this.paperSizesLst;
    PaperSize defaultPaperSize = paperSize ?? printerSettings.DefaultPageSettings.PaperSize;
    PrintSettings PrintSettings = PrintService.LoadSettings(selectedItem.PrinterName);
    if (PrintSettings != null)
    {
      this.cboxPaperSize.SelectedItem = (object) (this.paperSizesLst.FirstOrDefault<PaperSizeInfo>((Func<PaperSizeInfo, bool>) (c => c.PaperSize.Kind == PrintSettings.Paper.Kind && c.PaperSize.PaperName == PrintSettings.Paper.PaperName)) ?? this.paperSizesLst.FirstOrDefault<PaperSizeInfo>((Func<PaperSizeInfo, bool>) (c => c.FriendlyName == "A4")) ?? this.paperSizesLst.FirstOrDefault<PaperSizeInfo>());
      this.DuplexPanel.IsEnabled = printerSettings.CanDuplex;
      if (this.DuplexPanel.IsEnabled)
      {
        if (PrintSettings.Duplex == Duplex.Horizontal)
        {
          this.chkbDuplex.IsChecked = new bool?(true);
          this.rdbtnDuplexHorizontal.IsChecked = new bool?(true);
        }
        else if (PrintSettings.Duplex == Duplex.Vertical)
        {
          this.chkbDuplex.IsChecked = new bool?(true);
          this.rdbtnDuplexVertical.IsChecked = new bool?(true);
        }
        else
          this.chkbDuplex.IsChecked = new bool?(false);
      }
      else
        this.chkbDuplex.IsChecked = new bool?(false);
      if (PrintSettings.IsGrayscale)
        this.GrayscaleCheckbox.IsChecked = new bool?(true);
      else
        this.GrayscaleCheckbox.IsChecked = new bool?(false);
      if (PrintSettings.Landscape)
        this.rdbtnLandscape.IsChecked = new bool?(true);
      else
        this.rdbtnPortrait.IsChecked = new bool?(true);
    }
    else
    {
      this.cboxPaperSize.SelectedItem = (object) (this.paperSizesLst.FirstOrDefault<PaperSizeInfo>((Func<PaperSizeInfo, bool>) (c => defaultPaperSize != null && c.PaperSize.Kind == defaultPaperSize.Kind && c.PaperSize.PaperName == defaultPaperSize.PaperName)) ?? this.paperSizesLst.FirstOrDefault<PaperSizeInfo>((Func<PaperSizeInfo, bool>) (c => c.FriendlyName == "A4")) ?? this.paperSizesLst.FirstOrDefault<PaperSizeInfo>());
      if (!printerSettings.CanDuplex)
        this.chkbDuplex.IsChecked = new bool?(false);
      this.DuplexPanel.IsEnabled = printerSettings.CanDuplex;
    }
    this.UpdatePreview();
  }

  private (PdfDocument document, bool isTmpDoc, int allcount) GetDocument(
    BatchPrintDocumentModel model)
  {
    int num = 0;
    PdfDocument pdfDocument = (PdfDocument) null;
    bool flag = false;
    if (model.PageRangeMode == BatchPrintDocumentModel.PageRangeEnum.AllPages && model.SubsetMode == BatchPrintDocumentModel.SubsetEnum.AllPages)
    {
      pdfDocument = model.Document;
      flag = false;
      num = model.Document.Pages.Count;
    }
    else
    {
      int[] indexes;
      string actualPageRange = model.GetActualPageRange(out indexes);
      if (!string.IsNullOrEmpty(actualPageRange) && indexes != null && indexes.Length != 0)
      {
        PageDisposeHelper.TryFixResource(model.Document, ((IEnumerable<int>) indexes).Min(), ((IEnumerable<int>) indexes).Max());
        pdfDocument = PdfDocument.CreateNew();
        pdfDocument.Pages.ImportPages(model.Document, actualPageRange, 0);
        flag = true;
        num = indexes.Length;
      }
    }
    return (pdfDocument, flag, num);
  }

  private async void AddDocumentButton_Click(object sender, RoutedEventArgs e)
  {
    BatchPrintWindow owner = this;
    owner.IsEnabled = false;
    owner.ProgressBackground.Visibility = Visibility.Visible;
    try
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.Filter = "pdf|*.pdf";
      openFileDialog1.ShowReadOnly = false;
      openFileDialog1.ReadOnlyChecked = true;
      openFileDialog1.Multiselect = true;
      OpenFileDialog openFileDialog2 = openFileDialog1;
      if (!openFileDialog2.ShowDialog((Window) owner).GetValueOrDefault())
        return;
      CommomLib.Commom.GAManager.SendEvent("PdfBatchPrintDocument", "AddDocumentBtnClick", "Count", 1L);
      await owner.AddDocumentFromFiles(openFileDialog2.FileNames);
    }
    finally
    {
      owner.IsEnabled = true;
      owner.ProgressBackground.Visibility = Visibility.Collapsed;
    }
  }

  private void LeftFileListContainer_DragEnter(object sender, DragEventArgs e)
  {
    e.Handled = true;
    if (e.Data.GetDataPresent(DataFormats.FileDrop))
      e.Effects = DragDropEffects.Copy;
    else
      e.Effects = DragDropEffects.None;
  }

  private async void LeftFileListContainer_Drop(object sender, DragEventArgs e)
  {
    BatchPrintWindow batchPrintWindow = this;
    e.Handled = true;
    if (!e.Data.GetDataPresent(DataFormats.FileDrop))
      return;
    string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
    if (files == null)
      return;
    batchPrintWindow.IsEnabled = false;
    batchPrintWindow.ProgressBackground.Visibility = Visibility.Visible;
    if (batchPrintWindow.closeTokenSource.IsCancellationRequested)
      return;
    string[] pdfFiles = (string[]) null;
    try
    {
      pdfFiles = await Task.Run<string[]>((Func<string[]>) (() => ((IEnumerable<string>) files).SelectMany<string, string>((Func<string, IEnumerable<string>>) (c =>
      {
        try
        {
          return GetPdfFilesRecursion(c, this.closeTokenSource.Token);
        }
        catch
        {
        }
        return (IEnumerable<string>) null;
      })).ToArray<string>()), batchPrintWindow.closeTokenSource.Token);
    }
    catch
    {
    }
    if (batchPrintWindow.closeTokenSource.IsCancellationRequested)
      return;
    try
    {
      CommomLib.Commom.GAManager.SendEvent("PdfBatchPrintDocument", "AddDocumentDragDrop", "Count", 1L);
      await batchPrintWindow.AddDocumentFromFiles(pdfFiles);
    }
    finally
    {
      batchPrintWindow.IsEnabled = true;
      batchPrintWindow.ProgressBackground.Visibility = Visibility.Collapsed;
    }
    pdfFiles = (string[]) null;

    static IEnumerable<string> GetPdfFilesRecursion(string fileOrFolder, CancellationToken token)
    {
      token.ThrowIfCancellationRequested();
      try
      {
        if (string.IsNullOrEmpty(fileOrFolder))
          return Enumerable.Empty<string>();
        if (fileOrFolder.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) && File.Exists(fileOrFolder))
          return (IEnumerable<string>) new string[1]
          {
            fileOrFolder
          };
        if (Directory.Exists(fileOrFolder))
          return ((IEnumerable<string>) Directory.GetFileSystemEntries(fileOrFolder)).SelectMany<string, string>((Func<string, IEnumerable<string>>) (c => GetPdfFilesRecursion(c, token)));
      }
      catch
      {
      }
      return Enumerable.Empty<string>();
    }
  }

  private void DocumentListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (this.DocumentListBox.SelectedItem is BatchPrintDocumentModel selectedItem)
    {
      switch (selectedItem.PageRangeMode)
      {
        case BatchPrintDocumentModel.PageRangeEnum.AllPages:
          this.rdbtnAllPages.IsChecked = new bool?(true);
          break;
        case BatchPrintDocumentModel.PageRangeEnum.CurrentPage:
          this.rdbtnCurrentPage.IsChecked = new bool?(true);
          break;
        case BatchPrintDocumentModel.PageRangeEnum.SelectedPages:
          this.rdbtnSelectPage.IsChecked = new bool?(true);
          break;
      }
      switch (selectedItem.SubsetMode)
      {
        case BatchPrintDocumentModel.SubsetEnum.AllPages:
          this.cboxSubset.SelectedIndex = 0;
          break;
        case BatchPrintDocumentModel.SubsetEnum.Odd:
          this.cboxSubset.SelectedIndex = 2;
          break;
        case BatchPrintDocumentModel.SubsetEnum.Even:
          this.cboxSubset.SelectedIndex = 1;
          break;
      }
    }
    else if (this.documents.Count > 0)
    {
      this.DocumentListBox.SelectedIndex = 0;
    }
    else
    {
      this.rdbtnAllPages.IsChecked = new bool?(true);
      this.cboxSubset.SelectedIndex = 0;
    }
    this.UpdatePreview();
  }

  private void PrinterPreferenceButton_Click(object sender, RoutedEventArgs e)
  {
    PrintArgs printArgs = this.GetPrintArgs();
    PrinterSettings settings = new PrinterSettings();
    if (printArgs.PrintDevMode != null)
      settings.SetHdevmode(printArgs.PrintDevMode);
    settings.PrinterName = printArgs.PrinterName;
    settings.Copies = (short) printArgs.Copies;
    settings.Collate = printArgs.Collate;
    settings.Duplex = printArgs.Duplex;
    settings.DefaultPageSettings.PaperSize = printArgs.PaperSize;
    settings.DefaultPageSettings.Landscape = printArgs.Landscape;
    settings.DefaultPageSettings.Color = !printArgs.Grayscale;
    PrintDevModeHandle printDevModeHandle = PrinterDevModeHelper.OpenPrinterConfigure((Window) this, settings);
    if (printDevModeHandle.IsInvalid)
      return;
    printArgs.PaperSize = settings.DefaultPageSettings.PaperSize;
    printArgs.Landscape = settings.DefaultPageSettings.Landscape;
    printArgs.Grayscale = !settings.DefaultPageSettings.Color;
    printArgs.PrinterName = settings.PrinterName;
    printArgs.Copies = (int) settings.Copies;
    printArgs.Collate = settings.Collate;
    printArgs.Duplex = settings.Duplex;
    printArgs.PrintDevMode = printDevModeHandle;
    PrintService.SaveSettings(new PrintSettings()
    {
      Device = printArgs.PrinterName,
      Paper = printArgs.PaperSize,
      IsGrayscale = printArgs.Grayscale,
      Duplex = printArgs.Duplex,
      Landscape = printArgs.Landscape
    });
    this.ApplyPrintArgs(printArgs);
    this.UpdatePreview();
  }

  private void cboxPrinterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.UpdatePrintProperties(this.cboxPaperSize.SelectedItem is PaperSizeInfo selectedItem ? selectedItem.PaperSize : (PaperSize) null);
  }

  private void PageRangeRadioButton_Click(object sender, RoutedEventArgs e)
  {
    RadioButton source = e.Source as RadioButton;
    BatchPrintDocumentModel item = this.DocumentListBox.SelectedItem as BatchPrintDocumentModel;
    if (source?.GroupName != "printRange" || item == null)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
    {
      if (this.rdbtnAllPages.IsChecked.GetValueOrDefault())
        item.PageRangeMode = BatchPrintDocumentModel.PageRangeEnum.AllPages;
      else if (this.rdbtnCurrentPage.IsChecked.GetValueOrDefault())
        item.PageRangeMode = BatchPrintDocumentModel.PageRangeEnum.CurrentPage;
      else if (this.rdbtnSelectPage.IsChecked.GetValueOrDefault())
        item.PageRangeMode = BatchPrintDocumentModel.PageRangeEnum.SelectedPages;
      this.UpdatePreview();
    }));
  }

  private void cboxSubset_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (!(this.DocumentListBox.SelectedItem is BatchPrintDocumentModel selectedItem))
      return;
    switch (((Selector) sender).SelectedIndex)
    {
      case 0:
        selectedItem.SubsetMode = BatchPrintDocumentModel.SubsetEnum.AllPages;
        break;
      case 1:
        selectedItem.SubsetMode = BatchPrintDocumentModel.SubsetEnum.Even;
        break;
      case 2:
        selectedItem.SubsetMode = BatchPrintDocumentModel.SubsetEnum.Odd;
        break;
    }
    this.UpdatePreview();
  }

  private void tboxCopies_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
  {
    Action func = (Action) (() =>
    {
      if (e.NewValue - 1.0 < 0.0001)
      {
        this.PrintCollateImage.Opacity = 0.6;
        this.chkbPrintCollate.IsEnabled = false;
      }
      else
      {
        this.PrintCollateImage.Opacity = 1.0;
        this.chkbPrintCollate.IsEnabled = true;
      }
    });
    Action func2 = (Action) null;
    func2 = (Action) (() =>
    {
      if (!this.IsLoaded)
        this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) func2);
      else
        func();
    });
    func2();
  }

  private void tboxPageRang_PreviewTextInput(object sender, TextCompositionEventArgs e)
  {
    if (!(e.Text != ",") || !(e.Text != "-"))
      return;
    Regex regex = new Regex("[^0-9]+");
    e.Handled = regex.IsMatch(e.Text);
  }

  private void btnCancel_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("PdfBatchPrintDocument", "CancelBtnClick", "Count", 1L);
    this.Close();
  }

  private void btnPrint_Click(object sender, RoutedEventArgs e)
  {
    NumberBox lastErrBox = (NumberBox) null;
    if (!this.tboxCopies.IsValid)
    {
      lastErrBox = this.tboxCopies;
      int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinPrinterPageRangErrorContent);
    }
    else if (!this.tboxScaleUnit.IsValid)
    {
      lastErrBox = this.tboxScaleUnit;
      int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinPrinterPageRangErrorContent);
    }
    if (lastErrBox != null)
    {
      this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (async () =>
      {
        lastErrBox.SelectAll();
        lastErrBox.Focus();
        Keyboard.Focus((IInputElement) lastErrBox);
      }));
    }
    else
    {
      BatchPrintDocumentModel printDocumentModel = this.documents.FirstOrDefault<BatchPrintDocumentModel>((Func<BatchPrintDocumentModel, bool>) (c => c.DocumentTotalPageCount > 0 && c.SelectedPageCount <= 0));
      if (printDocumentModel != null)
      {
        this.DocumentListBox.SelectedItem = (object) printDocumentModel;
        int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinWrongPageRangTipContent);
      }
      else
      {
        BatchPrintDocumentModel[] array = this.documents.ToArray<BatchPrintDocumentModel>();
        CommomLib.Commom.GAManager.SendEvent("PdfBatchPrintDocument", "PrintBtnClick", array.Length.ToString(), 1L);
        for (int index = 0; index < array.Length; ++index)
        {
          BatchPrintDocumentModel model = array[index];
          (PdfDocument document, bool isTmpDoc, int allcount) document = this.GetDocument(model);
          if (document.document != null && document.allcount >= 1)
          {
            PrintArgs printArgs = this.GetPrintArgs();
            printArgs.DocumentTitle = model.FileName;
            printArgs.Document = document.document;
            printArgs.AllCount = document.allcount;
            printArgs.IsTempDocument = document.isTmpDoc;
            WinPrinterProgress winPrinterProgress = new WinPrinterProgress(printArgs, this.PreviewControl.PrintAnnotations);
            winPrinterProgress.Owner = (Window) this;
            winPrinterProgress.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            winPrinterProgress.ShowDialog();
          }
        }
        PrintArgs printArgs1 = this.GetPrintArgs();
        PrintService.SaveSettings(new PrintSettings()
        {
          Device = printArgs1.PrinterName,
          Paper = printArgs1.PaperSize,
          IsGrayscale = printArgs1.Grayscale,
          Duplex = printArgs1.Duplex,
          Landscape = printArgs1.Landscape
        });
        this.Close();
      }
    }
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    this.closeTokenSource.Cancel();
    this.ClearPrinterList();
    this.ClearDocuments();
  }

  private void ClearPrinterList()
  {
    if (this.printLst == null)
      return;
    lock (this.printLst)
    {
      foreach (PrinterInfo printerInfo in this.printLst)
      {
        printerInfo.PrintDevMode?.Dispose();
        printerInfo.PrintDevMode = (PrintDevModeHandle) null;
      }
    }
    this.printLst.Clear();
  }

  private void ClearDocuments()
  {
    List<BatchPrintDocumentModel> list = this.documents.ToList<BatchPrintDocumentModel>();
    this.documents.Clear();
    foreach (BatchPrintDocumentModel printDocumentModel in list)
      printDocumentModel.Dispose();
    this.PreviewControl.PrintArgs = (PrintArgs) null;
  }

  private void UpdatePreview()
  {
    // ISSUE: unable to decompile the method.
  }

  private void cboxPaperSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.UpdatePreview();
  }

  private void PrintOrientation_Checked(object sender, RoutedEventArgs e)
  {
    if (!(e.Source is RadioButton))
      return;
    this.UpdatePreview();
  }

  private void PrintMode_Checked(object sender, RoutedEventArgs e)
  {
    if (!(e.Source is RadioButton))
      return;
    this.UpdatePreview();
  }

  private void tboxScaleUnit_LostFocus(object sender, RoutedEventArgs e) => this.UpdatePreview();

  private void tboxPageRang_LostFocus(object sender, RoutedEventArgs e) => this.UpdatePreview();

  private async void GrayscaleCheckbox_Click(object sender, RoutedEventArgs e)
  {
    BatchPrintWindow batchPrintWindow = this;
    // ISSUE: reference to a compiler-generated method
    await batchPrintWindow.Dispatcher.InvokeAsync(new Action(batchPrintWindow.\u003CGrayscaleCheckbox_Click\u003Eb__41_0));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/views/batchprintwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.LeftFileListContainer = (Grid) target;
        this.LeftFileListContainer.DragEnter += new DragEventHandler(this.LeftFileListContainer_DragEnter);
        this.LeftFileListContainer.Drop += new DragEventHandler(this.LeftFileListContainer_Drop);
        break;
      case 2:
        this.AddDocumentButton = (Button) target;
        this.AddDocumentButton.Click += new RoutedEventHandler(this.AddDocumentButton_Click);
        break;
      case 3:
        this.DocumentListBox = (ListBox) target;
        this.DocumentListBox.SelectionChanged += new SelectionChangedEventHandler(this.DocumentListBox_SelectionChanged);
        break;
      case 4:
        this.cboxPrinterList = (ComboBox) target;
        this.cboxPrinterList.SelectionChanged += new SelectionChangedEventHandler(this.cboxPrinterList_SelectionChanged);
        break;
      case 5:
        this.PreferenceBtn = (Button) target;
        this.PreferenceBtn.Click += new RoutedEventHandler(this.PrinterPreferenceButton_Click);
        break;
      case 6:
        this.tboxCopies = (NumberBox) target;
        this.tboxCopies.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.tboxCopies_ValueChanged);
        break;
      case 7:
        this.chkbPrintCollate = (CheckBox) target;
        break;
      case 8:
        this.PrintCollateImage = (Image) target;
        break;
      case 9:
        this.chkbRevertPrint = (CheckBox) target;
        break;
      case 10:
        this.GrayscaleCheckbox = (CheckBox) target;
        this.GrayscaleCheckbox.Click += new RoutedEventHandler(this.GrayscaleCheckbox_Click);
        break;
      case 11:
        ((UIElement) target).AddHandler(ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.PageRangeRadioButton_Click));
        break;
      case 12:
        this.rdbtnAllPages = (RadioButton) target;
        break;
      case 13:
        this.rdbtnCurrentPage = (RadioButton) target;
        break;
      case 14:
        this.rdbtnSelectPage = (RadioButton) target;
        break;
      case 15:
        this.tboxPageRang = (TextBox) target;
        this.tboxPageRang.PreviewTextInput += new TextCompositionEventHandler(this.tboxPageRang_PreviewTextInput);
        this.tboxPageRang.LostFocus += new RoutedEventHandler(this.tboxPageRang_LostFocus);
        break;
      case 16 /*0x10*/:
        this.tbRangTip = (TextBlock) target;
        break;
      case 17:
        this.tbSubset = (TextBlock) target;
        break;
      case 18:
        this.cboxSubset = (ComboBox) target;
        this.cboxSubset.SelectionChanged += new SelectionChangedEventHandler(this.cboxSubset_SelectionChanged);
        break;
      case 19:
        this.DuplexPanel = (StackPanel) target;
        break;
      case 20:
        this.chkbDuplex = (CheckBox) target;
        break;
      case 21:
        this.DuplexRadioButtonPanel = (StackPanel) target;
        break;
      case 22:
        this.rdbtnDuplexVertical = (RadioButton) target;
        break;
      case 23:
        this.rdbtnDuplexHorizontal = (RadioButton) target;
        break;
      case 24:
        this.cboxPaperSize = (ComboBox) target;
        this.cboxPaperSize.SelectionChanged += new SelectionChangedEventHandler(this.cboxPaperSize_SelectionChanged);
        break;
      case 25:
        ((UIElement) target).AddHandler(ToggleButton.CheckedEvent, (Delegate) new RoutedEventHandler(this.PrintOrientation_Checked));
        break;
      case 26:
        this.rdbtnPortrait = (RadioButton) target;
        break;
      case 27:
        this.rdbtnLandscape = (RadioButton) target;
        break;
      case 28:
        ((UIElement) target).AddHandler(ToggleButton.CheckedEvent, (Delegate) new RoutedEventHandler(this.PrintMode_Checked));
        break;
      case 29:
        this.rdbtnFitPage = (RadioButton) target;
        break;
      case 30:
        this.rdbtnActualSize = (RadioButton) target;
        break;
      case 31 /*0x1F*/:
        this.rdbtnScale = (RadioButton) target;
        break;
      case 32 /*0x20*/:
        this.tboxScaleUnit = (NumberBox) target;
        this.tboxScaleUnit.LostFocus += new RoutedEventHandler(this.tboxScaleUnit_LostFocus);
        break;
      case 33:
        this.tbSymbol = (TextBlock) target;
        break;
      case 34:
        this.PreviewControl = (PrintPreviewControl) target;
        break;
      case 35:
        this.FileCountRun = (Run) target;
        break;
      case 36:
        this.PagesCount = (Run) target;
        break;
      case 37:
        this.btnCacel = (Button) target;
        this.btnCacel.Click += new RoutedEventHandler(this.btnCancel_Click);
        break;
      case 38:
        this.btnPrint = (Button) target;
        this.btnPrint.Click += new RoutedEventHandler(this.btnPrint_Click);
        break;
      case 39:
        this.ProgressBackground = (Grid) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
