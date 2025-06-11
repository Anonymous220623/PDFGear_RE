// Decompiled with JetBrains decompiler
// Type: pdfconverter.Views.MainWindow2
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using pdfconverter.Controls;
using pdfconverter.Models;
using pdfconverter.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfconverter.Views;

public partial class MainWindow2 : Window, IComponentConnector
{
  internal ListBox Menus;
  internal SplitPDFUserControl UCSplitPDF;
  internal MergePDFUserControl UCMergePDF;
  internal PPTToPDFUserControl UCPPTToPDF;
  internal RTFToPDFUserControl UCRTFToPDF;
  internal TXTToPDFUserControl UCTXTToPDF;
  internal WordToPDFUserControl UCWordToPDF;
  internal HtmlToPDFUserControl UCHtmlToPDF;
  internal ExcelToPDFUserControl UCExcelToPDF;
  internal ImageToPDFUserControl UCImageToPDF;
  internal CompressPDFUserControl CompressPDF;
  private bool _contentLoaded;

  public MainWindow2ViewModel VM => this.DataContext as MainWindow2ViewModel;

  public MainWindow2()
  {
    this.InitializeComponent();
    this.DataContext = (object) Ioc.Default.GetRequiredService<MainWindow2ViewModel>();
  }

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    try
    {
      switch ((ConvToPDFType) App.convertType)
      {
        case ConvToPDFType.MergePDF:
          this.HidenUC((FrameworkElement) this.UCMergePDF);
          this.Menus.SelectedIndex = 0;
          break;
        case ConvToPDFType.SplitPDF:
          this.Menus.SelectedIndex = 1;
          this.HidenUC((FrameworkElement) this.UCSplitPDF);
          break;
        case ConvToPDFType.CompressPDF:
          this.Menus.SelectedIndex = 2;
          this.HidenUC((FrameworkElement) this.CompressPDF);
          break;
        case ConvToPDFType.WordToPDF:
          this.Menus.SelectedIndex = 3;
          this.HidenUC((FrameworkElement) this.UCWordToPDF);
          break;
        case ConvToPDFType.ExcelToPDF:
          this.Menus.SelectedIndex = 4;
          this.HidenUC((FrameworkElement) this.UCExcelToPDF);
          break;
        case ConvToPDFType.PPTToPDF:
          this.Menus.SelectedIndex = 5;
          this.HidenUC((FrameworkElement) this.UCPPTToPDF);
          break;
        case ConvToPDFType.ImageToPDF:
          this.Menus.SelectedIndex = 6;
          this.HidenUC((FrameworkElement) this.UCImageToPDF);
          break;
        case ConvToPDFType.RtfToPDF:
          this.Menus.SelectedIndex = 7;
          this.HidenUC((FrameworkElement) this.UCRTFToPDF);
          break;
        case ConvToPDFType.TxtToPDF:
          this.Menus.SelectedIndex = 8;
          this.HidenUC((FrameworkElement) this.UCTXTToPDF);
          break;
      }
    }
    catch (Exception ex)
    {
      CommomLib.Commom.Log.Instance.Error<Exception>(ex);
    }
  }

  private void Menus_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    try
    {
      string tag = (this.Menus.SelectedItem as ActionMenuGroup).Tag;
      if (tag == null)
        return;
      switch (tag.Length)
      {
        case 8:
          switch (tag[0])
          {
            case 'm':
              if (!(tag == "mergepdf"))
                return;
              this.HidenUC((FrameworkElement) this.UCMergePDF);
              return;
            case 'n':
              return;
            case 'o':
              return;
            case 'p':
              if (!(tag == "ppttopdf"))
                return;
              this.HidenUC((FrameworkElement) this.UCPPTToPDF);
              return;
            case 'q':
              return;
            case 'r':
              if (!(tag == "rtftopdf"))
                return;
              this.HidenUC((FrameworkElement) this.UCRTFToPDF);
              return;
            case 's':
              if (!(tag == "splitpdf"))
                return;
              this.HidenUC((FrameworkElement) this.UCSplitPDF);
              return;
            case 't':
              if (!(tag == "txttopdf"))
                return;
              this.HidenUC((FrameworkElement) this.UCTXTToPDF);
              return;
            default:
              return;
          }
        case 9:
          switch (tag[0])
          {
            case 'h':
              if (!(tag == "htmltopdf"))
                return;
              this.HidenUC((FrameworkElement) this.UCHtmlToPDF);
              return;
            case 'w':
              if (!(tag == "wordtopdf"))
                return;
              this.HidenUC((FrameworkElement) this.UCWordToPDF);
              return;
            default:
              return;
          }
        case 10:
          switch (tag[0])
          {
            case 'e':
              if (!(tag == "exceltopdf"))
                return;
              this.HidenUC((FrameworkElement) this.UCExcelToPDF);
              return;
            case 'i':
              if (!(tag == "imagetopdf"))
                return;
              this.HidenUC((FrameworkElement) this.UCImageToPDF);
              return;
            default:
              return;
          }
        case 11:
          if (!(tag == "compresspdf"))
            break;
          this.HidenUC((FrameworkElement) this.CompressPDF);
          break;
      }
    }
    catch (Exception ex)
    {
      CommomLib.Commom.Log.Instance.Error<Exception>(ex);
    }
  }

  private void HidenUC(FrameworkElement element)
  {
    this.UCSplitPDF.Visibility = Visibility.Hidden;
    this.UCMergePDF.Visibility = Visibility.Hidden;
    this.UCWordToPDF.Visibility = Visibility.Hidden;
    this.UCExcelToPDF.Visibility = Visibility.Hidden;
    this.UCPPTToPDF.Visibility = Visibility.Hidden;
    this.UCRTFToPDF.Visibility = Visibility.Hidden;
    this.UCTXTToPDF.Visibility = Visibility.Hidden;
    this.UCHtmlToPDF.Visibility = Visibility.Hidden;
    this.UCImageToPDF.Visibility = Visibility.Hidden;
    this.CompressPDF.Visibility = Visibility.Hidden;
    element.Visibility = Visibility.Visible;
  }

  private void AddOneFileToMergeList(string file) => File.Exists(file);

  private void splitModeHelpBtn_Click(object sender, RoutedEventArgs e)
  {
    int num = (int) ModernMessageBox.Show($"{pdfconverter.Properties.Resources.WinMergeSplitSplitModeCustomRangeHelpAsgMsg}\r\n{pdfconverter.Properties.Resources.WinMergeSplitSplitModeFixedRangeHelpAsgMsg}", UtilManager.GetProductName());
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfconverter;component/views/mainwindow2.xaml", UriKind.Relative));
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
        ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
        break;
      case 2:
        this.Menus = (ListBox) target;
        this.Menus.SelectionChanged += new SelectionChangedEventHandler(this.Menus_SelectionChanged);
        break;
      case 3:
        this.UCSplitPDF = (SplitPDFUserControl) target;
        break;
      case 4:
        this.UCMergePDF = (MergePDFUserControl) target;
        break;
      case 5:
        this.UCPPTToPDF = (PPTToPDFUserControl) target;
        break;
      case 6:
        this.UCRTFToPDF = (RTFToPDFUserControl) target;
        break;
      case 7:
        this.UCTXTToPDF = (TXTToPDFUserControl) target;
        break;
      case 8:
        this.UCWordToPDF = (WordToPDFUserControl) target;
        break;
      case 9:
        this.UCHtmlToPDF = (HtmlToPDFUserControl) target;
        break;
      case 10:
        this.UCExcelToPDF = (ExcelToPDFUserControl) target;
        break;
      case 11:
        this.UCImageToPDF = (ImageToPDFUserControl) target;
        break;
      case 12:
        this.CompressPDF = (CompressPDFUserControl) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
