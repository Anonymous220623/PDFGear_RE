// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Printer.WinPrinterDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Controls;
using Patagames.Pdf.Net;
using pdfeditor.Models.Print;
using pdfeditor.Utils;
using pdfeditor.Utils.Printer;
using pdfeditor.ViewModels;
using PDFKit;
using PDFKit.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Printer;

public class WinPrinterDialog : Window, IComponentConnector
{
  private MainViewModel VM;
  public List<PaperSizeInfo> paperSizesLst = new List<PaperSizeInfo>();
  public List<PrinterInfo> printLst = new List<PrinterInfo>();
  private CancellationTokenSource updatePreviewCts;
  internal PrintPreviewControl PreviewControl;
  internal System.Windows.Controls.ComboBox cboxPrinterList;
  internal System.Windows.Controls.Button PreferenceBtn;
  internal NumberBox tboxCopies;
  internal System.Windows.Controls.CheckBox chkbPrintCollate;
  internal Image PrintCollateImage;
  internal System.Windows.Controls.CheckBox chkbRevertPrint;
  internal System.Windows.Controls.CheckBox GrayscaleCheckbox;
  internal System.Windows.Controls.RadioButton rdbtnAllPages;
  internal System.Windows.Controls.RadioButton rdbtnCurrentPage;
  internal System.Windows.Controls.RadioButton rdbtnSelectPage;
  internal System.Windows.Controls.TextBox tboxPageRang;
  internal TextBlock tballCount;
  internal TextBlock tbRangTip;
  internal TextBlock tbSubset;
  internal System.Windows.Controls.ComboBox cboxSubset;
  internal StackPanel DuplexPanel;
  internal System.Windows.Controls.CheckBox chkbDuplex;
  internal StackPanel DuplexRadioButtonPanel;
  internal System.Windows.Controls.RadioButton rdbtnDuplexVertical;
  internal System.Windows.Controls.RadioButton rdbtnDuplexHorizontal;
  internal System.Windows.Controls.ComboBox cboxPaperSize;
  internal System.Windows.Controls.RadioButton rdbtnPortrait;
  internal System.Windows.Controls.RadioButton rdbtnLandscape;
  internal System.Windows.Controls.RadioButton rdbtnFitPage;
  internal System.Windows.Controls.RadioButton rdbtnActualSize;
  internal System.Windows.Controls.RadioButton rdbtnScale;
  internal NumberBox tboxScaleUnit;
  internal TextBlock tbSymbol;
  internal HyperlinkButton ClassicModeBtn;
  internal System.Windows.Controls.Button btnCacel;
  internal System.Windows.Controls.Button btnBatchPrint;
  internal System.Windows.Controls.Button btnPrint;
  private bool _contentLoaded;

  public WinPrinterDialog(MainViewModel model)
  {
    this.InitializeComponent();
    this.VM = model;
    this.PreviewControl.PrintAnnotations = this.VM.IsAnnotationVisible;
    this.GetPrinterList();
    this.InitPrinterSet();
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

  private void InitPrinterSet()
  {
    this.tboxPageRang.Text = $"1-{this.VM.Document.Pages.Count}";
    this.tballCount.Text = $"/ {this.VM.Document.Pages.Count}";
    this.rdbtnCurrentPage.Content = (object) $"{pdfeditor.Properties.Resources.WinWatermarkCurrentpageContent} ({this.VM.CurrnetPageIndex} / {this.VM.TotalPagesCount})";
  }

  private void GetPrinterList()
  {
    try
    {
      string sDefault = new PrintDocument().PrinterSettings.PrinterName;
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

  private void btnCancel_Click(object sender, RoutedEventArgs e)
  {
    this.Close();
    this.ClearPrinterList();
  }

  private async void btnBatchPrint_Click(object sender, RoutedEventArgs e)
  {
    WinPrinterDialog winPrinterDialog = this;
    winPrinterDialog.Close();
    winPrinterDialog.ClearPrinterList();
    await winPrinterDialog.VM.BatchPrintAsync("PrintWindow");
  }

  private void btnPrint_Click(object sender, RoutedEventArgs e)
  {
    // ISSUE: unable to decompile the method.
  }

  private void ClearPrinterList()
  {
    if (this.printLst != null)
    {
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
    this.PreviewControl.PrintArgs = (PrintArgs) null;
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
    printArgs1.DocumentPath = this.VM.DocumentWrapper.DocumentPath;
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

  public static string ConvertToRange(
    List<int> pageIndexes,
    out int[] sortedPageIndexes,
    int Parity,
    out int allCount,
    bool IsAllPage = true)
  {
    sortedPageIndexes = (int[]) null;
    if (pageIndexes == null)
    {
      allCount = 0;
      return string.Empty;
    }
    int[] array = pageIndexes.ToArray();
    if (array.Length == 0)
    {
      allCount = 0;
      return string.Empty;
    }
    allCount = 0;
    if (IsAllPage)
      Array.Sort<int>(array);
    sortedPageIndexes = array;
    StringBuilder stringBuilder = new StringBuilder();
    bool flag = false;
    for (int index = 0; index < array.Length; ++index)
    {
      if (stringBuilder.Length == 0)
      {
        if (WinPrinterDialog.IsLegal(Parity, array[index] + 1))
        {
          stringBuilder.Append(array[index] + 1);
          allCount = 1;
        }
      }
      else if (array[index] - 1 == array[index - 1] && Parity == 0)
      {
        flag = true;
        ++allCount;
      }
      else
      {
        if (flag)
        {
          stringBuilder.Append('-').Append(array[index - 1] + 1);
          flag = false;
        }
        if (WinPrinterDialog.IsLegal(Parity, array[index] + 1))
        {
          stringBuilder.Append(",");
          stringBuilder.Append(array[index] + 1);
          ++allCount;
        }
      }
    }
    if (flag)
      stringBuilder.Append('-').Append(array[array.Length - 1] + 1);
    return stringBuilder.ToString().Trim();
  }

  private static bool IsLegal(int Parity, int pageindex)
  {
    switch (Parity)
    {
      case 1:
        if (pageindex % 2 != 0)
          return false;
        break;
      case 2:
        if (pageindex % 2 == 0)
          return false;
        break;
    }
    return true;
  }

  private void cboxPrinterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.UpdatePrintProperties(this.cboxPaperSize.SelectedItem is PaperSizeInfo selectedItem ? selectedItem.PaperSize : (PaperSize) null);
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
    if (this.paperSizesLst.Count == 0)
      CommomLib.Commom.GAManager.SendEvent("PdfPrintDocument", "PaperSizeList", "Empty", 1L);
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

  private (PdfDocument Document, bool isTmpDoc, int Allcount) GetDocument()
  {
    int allCount = 0;
    PdfDocument pdfDocument = (PdfDocument) null;
    bool flag = false;
    int errorCharIndex;
    if (this.rdbtnAllPages.IsChecked.GetValueOrDefault())
    {
      int[] pageIndexes;
      if (!PdfObjectExtensions.TryParsePageRange($"1-{this.VM.Document.Pages.Count}", out pageIndexes, out errorCharIndex))
      {
        int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinWrongPageRangTipContent);
        return ((PdfDocument) null, false, 0);
      }
      if (((IEnumerable<int>) pageIndexes).Max() + 1 > this.VM.Document.Pages.Count || ((IEnumerable<int>) pageIndexes).Min() < 0)
      {
        int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinWrongPageRangTipContent);
        return ((PdfDocument) null, false, 0);
      }
      if (this.cboxSubset.SelectedIndex == 0)
      {
        pdfDocument = this.VM.Document;
        flag = false;
        allCount = this.VM.Document.Pages.Count;
      }
      else
      {
        int[] sortedPageIndexes;
        string range = WinPrinterDialog.ConvertToRange(((IEnumerable<int>) pageIndexes).ToList<int>(), out sortedPageIndexes, this.cboxSubset.SelectedIndex, out allCount);
        PageDisposeHelper.TryFixResource(this.VM.Document, ((IEnumerable<int>) sortedPageIndexes).Min(), ((IEnumerable<int>) sortedPageIndexes).Max());
        pdfDocument = PdfDocument.CreateNew();
        pdfDocument.Pages.ImportPages(this.VM.Document, range, 0);
        flag = true;
      }
    }
    else if (this.rdbtnCurrentPage.IsChecked.GetValueOrDefault())
    {
      PageDisposeHelper.TryFixResource(this.VM.Document, this.VM.CurrnetPageIndex, this.VM.CurrnetPageIndex + 1);
      pdfDocument = PdfDocument.CreateNew();
      pdfDocument.Pages.ImportPages(this.VM.Document, this.VM.CurrnetPageIndex.ToString(), 0);
      flag = true;
      allCount = 1;
    }
    else if (this.rdbtnSelectPage.IsChecked.GetValueOrDefault())
    {
      string range1 = this.tboxPageRang.Text.Trim();
      int[] pageIndexes;
      if (!PdfObjectExtensions.TryParsePageRange(range1, out pageIndexes, out errorCharIndex))
      {
        int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinWrongPageRangTipContent);
        return ((PdfDocument) null, false, 0);
      }
      if (((IEnumerable<int>) pageIndexes).Max() + 1 > this.VM.Document.Pages.Count || ((IEnumerable<int>) pageIndexes).Min() < 0)
      {
        int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinWrongPageRangTipContent);
        return ((PdfDocument) null, false, 0);
      }
      int[] sortedPageIndexes;
      string range2 = WinPrinterDialog.ConvertToRange(this.GetPageList(range1), out sortedPageIndexes, this.cboxSubset.SelectedIndex, out allCount, false);
      PageDisposeHelper.TryFixResource(this.VM.Document, ((IEnumerable<int>) sortedPageIndexes).Min(), ((IEnumerable<int>) sortedPageIndexes).Max());
      pdfDocument = PdfDocument.CreateNew();
      pdfDocument.Pages.ImportPages(this.VM.Document, range2, 0);
      flag = true;
    }
    return (pdfDocument, flag, allCount);
  }

  private List<int> GetPageList(string range)
  {
    List<int> pageList = new List<int>();
    try
    {
      string str1 = range;
      char[] chArray = new char[1]{ ',' };
      foreach (string str2 in str1.Split(chArray))
      {
        if (str2.Contains<char>('-'))
          pageList.AddRange((IEnumerable<int>) this.parseRang(str2));
        else if (!string.IsNullOrWhiteSpace(str2))
          pageList.Add(int.Parse(str2) - 1);
      }
    }
    catch
    {
    }
    return pageList;
  }

  private List<int> parseRang(string range)
  {
    List<int> rang = new List<int>();
    try
    {
      string[] strArray = range.Split('-');
      int num1 = int.Parse(strArray[0]);
      int num2 = int.Parse(strArray[1]);
      for (int index = num1 - 1; index < num2; ++index)
        rang.Add(index);
    }
    catch
    {
    }
    return rang;
  }

  private void tboxPageRang_TextInput(object sender, TextCompositionEventArgs e)
  {
  }

  private void tboxScaleUnit_PreviewTextInput(object sender, TextCompositionEventArgs e)
  {
    if (!(e.Text != "."))
      return;
    Regex regex = new Regex("[^0-9]+");
    e.Handled = regex.IsMatch(e.Text);
  }

  private void tboxPageRang_PreviewTextInput(object sender, TextCompositionEventArgs e)
  {
    if (!(e.Text != ",") || !(e.Text != "-"))
      return;
    Regex regex = new Regex("[^0-9]+");
    e.Handled = regex.IsMatch(e.Text);
  }

  private void PrinterPreferenceButton_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("PdfPrintDocument", "PrinterPreferenceBtnClick", "Count", 1L);
    PrintArgs printArgs = this.GetPrintArgs();
    if (printArgs == null)
      return;
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

  private void ClassicModeBtn_Click(object sender, RoutedEventArgs e)
  {
    this.Close();
    this.ClearPrinterList();
    CommomLib.Commom.GAManager.SendEvent("PdfPrintDocument", "ClassicModeBtnClick", "Count", 1L);
    PdfPrintDocument pdfPrintDocument = new PdfPrintDocument(this.VM.Document, isAnnotationVisible: this.PreviewControl.PrintAnnotations);
    System.Windows.Forms.PrintDialog dlg = new System.Windows.Forms.PrintDialog();
    dlg.AllowCurrentPage = true;
    dlg.AllowSomePages = true;
    dlg.UseEXDialog = true;
    dlg.Document = (PrintDocument) pdfPrintDocument;
    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (() =>
    {
      if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        return;
      try
      {
        dlg.Document.Print();
      }
      catch (Win32Exception ex)
      {
        CommomLib.Commom.GAManager.SendEvent("Exception", "PrintBtn", ex.Message, 1L);
      }
    }));
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

  private void UpdatePreview()
  {
    // ISSUE: unable to decompile the method.
  }

  private void PageRange_Checked(object sender, RoutedEventArgs e)
  {
    if (!(e.Source is System.Windows.Controls.RadioButton))
      return;
    this.UpdatePreview();
  }

  private void tboxPageRang_LostFocus(object sender, RoutedEventArgs e) => this.UpdatePreview();

  private void cboxSubset_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.UpdatePreview();
  }

  private void cboxPaperSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.UpdatePreview();
  }

  private void PrintOrientation_Checked(object sender, RoutedEventArgs e)
  {
    if (!(e.Source is System.Windows.Controls.RadioButton))
      return;
    this.UpdatePreview();
  }

  private void PrintMode_Checked(object sender, RoutedEventArgs e)
  {
    if (!(e.Source is System.Windows.Controls.RadioButton))
      return;
    this.UpdatePreview();
  }

  private void tboxScaleUnit_LostFocus(object sender, RoutedEventArgs e) => this.UpdatePreview();

  private async void GrayscaleCheckbox_Click(object sender, RoutedEventArgs e)
  {
    WinPrinterDialog winPrinterDialog = this;
    // ISSUE: reference to a compiler-generated method
    await winPrinterDialog.Dispatcher.InvokeAsync(new Action(winPrinterDialog.\u003CGrayscaleCheckbox_Click\u003Eb__35_0));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    System.Windows.Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/printer/winprinterdialog.xaml", UriKind.Relative));
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
        this.PreviewControl = (PrintPreviewControl) target;
        break;
      case 2:
        this.cboxPrinterList = (System.Windows.Controls.ComboBox) target;
        this.cboxPrinterList.SelectionChanged += new SelectionChangedEventHandler(this.cboxPrinterList_SelectionChanged);
        break;
      case 3:
        this.PreferenceBtn = (System.Windows.Controls.Button) target;
        this.PreferenceBtn.Click += new RoutedEventHandler(this.PrinterPreferenceButton_Click);
        break;
      case 4:
        this.tboxCopies = (NumberBox) target;
        this.tboxCopies.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.tboxCopies_ValueChanged);
        break;
      case 5:
        this.chkbPrintCollate = (System.Windows.Controls.CheckBox) target;
        break;
      case 6:
        this.PrintCollateImage = (Image) target;
        break;
      case 7:
        this.chkbRevertPrint = (System.Windows.Controls.CheckBox) target;
        break;
      case 8:
        this.GrayscaleCheckbox = (System.Windows.Controls.CheckBox) target;
        this.GrayscaleCheckbox.Click += new RoutedEventHandler(this.GrayscaleCheckbox_Click);
        break;
      case 9:
        ((UIElement) target).AddHandler(ToggleButton.CheckedEvent, (Delegate) new RoutedEventHandler(this.PageRange_Checked));
        break;
      case 10:
        this.rdbtnAllPages = (System.Windows.Controls.RadioButton) target;
        break;
      case 11:
        this.rdbtnCurrentPage = (System.Windows.Controls.RadioButton) target;
        break;
      case 12:
        this.rdbtnSelectPage = (System.Windows.Controls.RadioButton) target;
        break;
      case 13:
        this.tboxPageRang = (System.Windows.Controls.TextBox) target;
        this.tboxPageRang.TextInput += new TextCompositionEventHandler(this.tboxPageRang_TextInput);
        this.tboxPageRang.PreviewTextInput += new TextCompositionEventHandler(this.tboxPageRang_PreviewTextInput);
        this.tboxPageRang.LostFocus += new RoutedEventHandler(this.tboxPageRang_LostFocus);
        break;
      case 14:
        this.tballCount = (TextBlock) target;
        break;
      case 15:
        this.tbRangTip = (TextBlock) target;
        break;
      case 16 /*0x10*/:
        this.tbSubset = (TextBlock) target;
        break;
      case 17:
        this.cboxSubset = (System.Windows.Controls.ComboBox) target;
        this.cboxSubset.SelectionChanged += new SelectionChangedEventHandler(this.cboxSubset_SelectionChanged);
        break;
      case 18:
        this.DuplexPanel = (StackPanel) target;
        break;
      case 19:
        this.chkbDuplex = (System.Windows.Controls.CheckBox) target;
        break;
      case 20:
        this.DuplexRadioButtonPanel = (StackPanel) target;
        break;
      case 21:
        this.rdbtnDuplexVertical = (System.Windows.Controls.RadioButton) target;
        break;
      case 22:
        this.rdbtnDuplexHorizontal = (System.Windows.Controls.RadioButton) target;
        break;
      case 23:
        this.cboxPaperSize = (System.Windows.Controls.ComboBox) target;
        this.cboxPaperSize.SelectionChanged += new SelectionChangedEventHandler(this.cboxPaperSize_SelectionChanged);
        break;
      case 24:
        ((UIElement) target).AddHandler(ToggleButton.CheckedEvent, (Delegate) new RoutedEventHandler(this.PrintOrientation_Checked));
        break;
      case 25:
        this.rdbtnPortrait = (System.Windows.Controls.RadioButton) target;
        break;
      case 26:
        this.rdbtnLandscape = (System.Windows.Controls.RadioButton) target;
        break;
      case 27:
        ((UIElement) target).AddHandler(ToggleButton.CheckedEvent, (Delegate) new RoutedEventHandler(this.PrintMode_Checked));
        break;
      case 28:
        this.rdbtnFitPage = (System.Windows.Controls.RadioButton) target;
        break;
      case 29:
        this.rdbtnActualSize = (System.Windows.Controls.RadioButton) target;
        break;
      case 30:
        this.rdbtnScale = (System.Windows.Controls.RadioButton) target;
        break;
      case 31 /*0x1F*/:
        this.tboxScaleUnit = (NumberBox) target;
        this.tboxScaleUnit.LostFocus += new RoutedEventHandler(this.tboxScaleUnit_LostFocus);
        break;
      case 32 /*0x20*/:
        this.tbSymbol = (TextBlock) target;
        break;
      case 33:
        this.ClassicModeBtn = (HyperlinkButton) target;
        this.ClassicModeBtn.Click += new RoutedEventHandler(this.ClassicModeBtn_Click);
        break;
      case 34:
        this.btnCacel = (System.Windows.Controls.Button) target;
        this.btnCacel.Click += new RoutedEventHandler(this.btnCancel_Click);
        break;
      case 35:
        this.btnBatchPrint = (System.Windows.Controls.Button) target;
        this.btnBatchPrint.Click += new RoutedEventHandler(this.btnBatchPrint_Click);
        break;
      case 36:
        this.btnPrint = (System.Windows.Controls.Button) target;
        this.btnPrint.Click += new RoutedEventHandler(this.btnPrint_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
