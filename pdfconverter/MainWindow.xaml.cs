// Decompiled with JetBrains decompiler
// Type: pdfconverter.MainWindow
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using NSOCR_NameSpace;
using pdfconverter.Models;
using pdfconverter.Utils;
using pdfconverter.Views;
using SautinSoft;
using Syncfusion.Presentation;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace pdfconverter;

public partial class MainWindow : Window, IComponentConnector, IStyleConnector
{
  internal int CfgObj;
  internal int OcrObj;
  internal int ImgObj;
  internal int ScanObj;
  internal int SvrObj;
  internal bool OCRCreated;
  private string NsOCROnlineLanguage;
  private static readonly double PPTMaximum = 4032.0;
  private static readonly int PDFToImageDPI = 200;
  private string NsOCRLanguage;
  private ObservableCollection<ConvertFileItem> convertFilesList = new ObservableCollection<ConvertFileItem>();
  private CheckBox fileItemListCB;
  internal TextBlock titleTB;
  internal Button addFileBtn;
  internal ListView convertFilesView;
  internal TextBox outputPathTB;
  internal Button changeDestPathBtn;
  internal Grid ocrGrid;
  internal CheckBox OCRCB;
  internal StackPanel ocrSettingsPanel;
  internal Hyperlink curLangLink;
  internal TextBlock curLang;
  internal Grid excelGrid;
  internal CheckBox SingleSheetBtn;
  internal ComboBox outputFormatCB;
  internal ComboBoxItem outputCBItem_docx;
  internal ComboBoxItem outputCBItem_rtf;
  internal ComboBoxItem outputCBItem_xls;
  internal ComboBoxItem outputCBItem_html;
  internal ComboBoxItem outputCBItem_xml;
  internal ComboBoxItem outputCBItem_text;
  internal ComboBoxItem outputCBItem_png;
  internal ComboBoxItem outputCBItem_jpeg;
  internal ComboBoxItem outputCBItem_pptx;
  internal CheckBox ConvertOnlineCB;
  internal CheckBox viewFileAfterConvertCB;
  internal Button convertBtn;
  private bool _contentLoaded;

  public MainWindow()
  {
    this.InitializeComponent();
    this.InitializePDFConvert();
    this.convertFilesView.ItemsSource = (IEnumerable) this.convertFilesList;
  }

  private void InitializePDFConvert()
  {
    ConvFromPDFType convertType = (ConvFromPDFType) App.convertType;
    this.titleTB.Text = ConvertManager.getTitle(convertType);
    this.ocrGrid.Visibility = Visibility.Visible;
    switch (convertType)
    {
      case ConvFromPDFType.PDFToWord:
        this.outputCBItem_docx.Visibility = Visibility.Visible;
        this.outputFormatCB.SelectedItem = (object) this.outputCBItem_docx;
        break;
      case ConvFromPDFType.PDFToExcel:
        this.outputCBItem_xls.Visibility = Visibility.Visible;
        this.outputFormatCB.SelectedItem = (object) this.outputCBItem_xls;
        this.ocrGrid.Visibility = Visibility.Hidden;
        this.excelGrid.Visibility = Visibility.Visible;
        break;
      case ConvFromPDFType.PDFToPng:
        this.outputCBItem_png.Visibility = Visibility.Visible;
        this.outputCBItem_jpeg.Visibility = Visibility.Visible;
        this.outputFormatCB.SelectedItem = (object) this.outputCBItem_png;
        this.ocrGrid.Visibility = Visibility.Hidden;
        break;
      case ConvFromPDFType.PDFToJpg:
        this.outputCBItem_png.Visibility = Visibility.Visible;
        this.outputCBItem_jpeg.Visibility = Visibility.Visible;
        this.outputFormatCB.SelectedItem = (object) this.outputCBItem_jpeg;
        this.ocrGrid.Visibility = Visibility.Hidden;
        break;
      case ConvFromPDFType.PDFToTxt:
        this.outputCBItem_text.Visibility = Visibility.Visible;
        this.outputFormatCB.SelectedItem = (object) this.outputCBItem_text;
        break;
      case ConvFromPDFType.PDFToHtml:
        this.outputCBItem_html.Visibility = Visibility.Visible;
        this.outputFormatCB.SelectedItem = (object) this.outputCBItem_html;
        break;
      case ConvFromPDFType.PDFToXml:
        this.outputCBItem_xml.Visibility = Visibility.Visible;
        this.outputFormatCB.SelectedItem = (object) this.outputCBItem_xml;
        this.ocrGrid.Visibility = Visibility.Hidden;
        break;
      case ConvFromPDFType.PDFToRTF:
        this.outputCBItem_rtf.Visibility = Visibility.Visible;
        this.outputFormatCB.SelectedItem = (object) this.outputCBItem_rtf;
        break;
      case ConvFromPDFType.PDFToPPT:
        this.outputCBItem_pptx.Visibility = Visibility.Visible;
        this.ocrGrid.Visibility = Visibility.Hidden;
        this.outputFormatCB.SelectedItem = (object) this.outputCBItem_pptx;
        break;
    }
    string empty = string.Empty;
    string path = ConfigManager.GetConvertPath();
    try
    {
      path = ConfigManager.GetConvertPath();
      if (!string.IsNullOrEmpty(path))
      {
        if (Directory.Exists(path))
          goto label_15;
      }
      path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\PDFgear";
    }
    catch
    {
    }
label_15:
    if (!string.IsNullOrEmpty(path))
      this.outputPathTB.Text = path;
    if (App.selectedFile != null && App.selectedFile.Length != 0)
      this.AddPDFFilesToConvertList(App.selectedFile, App.seletedPassword);
    this.curLang.Text = ConvertManager.getOCRLanguageL10N();
    GAManager.SendEvent("ConvertMainWin", "Show", convertType.ToString(), 1L);
  }

  private void AddPDFFilesToConvertList(string[] files, string[] passwords = null)
  {
    if (files == null || files.Length == 0)
      return;
    for (int index = 0; index < files.Length; ++index)
    {
      string file = files[index];
      string password = "";
      if (passwords != null && passwords.Length != 0)
        password = passwords[index];
      if (!string.IsNullOrWhiteSpace(file))
      {
        bool? nullable = ConToPDFUtils.CheckAccess(file);
        bool flag1 = false;
        if (nullable.GetValueOrDefault() == flag1 & nullable.HasValue)
        {
          nullable = new FileOccupation(file).ShowDialog();
          bool flag2 = false;
          if (nullable.GetValueOrDefault() == flag2 & nullable.HasValue)
            continue;
        }
        if (UtilsManager.IsPDFFile(file))
        {
          if (!ConvertManager.IsFileAdded(this.convertFilesList, file))
          {
            if (ConToPDFUtils.CheckPassword(file, ref password, (Window) this))
            {
              ConvertFileItem convertFileItem = new ConvertFileItem(file);
              this.convertFilesList.Add(convertFileItem);
              convertFileItem.parseFile(password);
              this.updateFileItemListCBStatus();
            }
            else
            {
              ConvertFileItem convertFileItem = new ConvertFileItem(file);
              this.convertFilesList.Add(convertFileItem);
              convertFileItem.FileSelected = new bool?(false);
              convertFileItem.ConvertStatus = FileCovertStatus.ConvertLoadedFailed;
              this.updateFileItemListCBStatus();
            }
          }
          else
          {
            int num1 = (int) MessageBox.Show(pdfconverter.Properties.Resources.FileConvertMsgFileAdded, UtilManager.GetProductName(), MessageBoxButton.OK, MessageBoxImage.Exclamation);
          }
        }
        else if (!ConvertManager.IsFileAdded(this.convertFilesList, file))
        {
          this.convertFilesList.Add(new ConvertFileItem(file)
          {
            FileSelected = new bool?(false),
            ConvertStatus = FileCovertStatus.ConvertUnsupport
          });
          this.updateFileItemListCBStatus();
        }
        else
        {
          int num2 = (int) MessageBox.Show(pdfconverter.Properties.Resources.FileConvertMsgFileAdded, UtilManager.GetProductName(), MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
      }
    }
  }

  private void AddFileBtn_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("ConvertMainWin", "AddFileBtn", App.convertType.ToString(), 1L);
    string[] files = ConvertManager.selectMultiPDFFiles();
    if (files == null || files.Length == 0)
      return;
    this.AddPDFFilesToConvertList(files);
  }

  private void Grid_Drop(object sender, DragEventArgs e)
  {
    GAManager.SendEvent("ConvertMainWin", "DragDrop", App.convertType.ToString(), 1L);
    if (this.convertFilesList == null || !e.Data.GetDataPresent(DataFormats.FileDrop))
      return;
    string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
    if (data == null || data.Length == 0)
      return;
    this.AddPDFFilesToConvertList(data);
  }

  private void OpenFileBtn_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("ConvertMainWin", "OpenFile", App.convertType.ToString(), 1L);
    ConvertFileItem dataContext = (ConvertFileItem) ((FrameworkElement) sender).DataContext;
    if (dataContext == null || string.IsNullOrWhiteSpace(dataContext.outputFile) || dataContext.ConvertStatus != FileCovertStatus.ConvertSucc)
      return;
    UtilsManager.OpenFile(dataContext.outputFile);
  }

  private void OpenFileInExploreBtn_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("ConvertMainWin", "OpenInExplore", App.convertType.ToString(), 1L);
    ConvertFileItem dataContext = (ConvertFileItem) ((FrameworkElement) sender).DataContext;
    if (dataContext == null || string.IsNullOrWhiteSpace(dataContext.outputFile) || dataContext.ConvertStatus != FileCovertStatus.ConvertSucc)
      return;
    UtilsManager.OpenFileInExplore(dataContext.outputFile, true);
  }

  private void DeleteFileBtn_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("ConvertMainWin", "DeleteFile", App.convertType.ToString(), 1L);
    ConvertFileItem dataContext = (ConvertFileItem) ((FrameworkElement) sender).DataContext;
    if (dataContext == null)
      return;
    try
    {
      this.convertFilesList.Remove(dataContext);
    }
    catch
    {
    }
  }

  private void ChangeDestPathBtn_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("ConvertMainWin", "ChangeOutputPathBtn", App.convertType.ToString(), 1L);
    string str = ConvertManager.selectOutputFolder(this.outputPathTB.Text);
    if (string.IsNullOrWhiteSpace(str) || this.outputPathTB == null)
      return;
    this.outputPathTB.Text = str;
    ConfigManager.SetConvertPath(this.outputPathTB.Text);
  }

  private void OutputPathCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
  }

  private void OutputPathHL_Click(object sender, RoutedEventArgs e)
  {
  }

  private void ClearFilesBtn_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      GAManager.SendEvent("ConvertMainWin", "ClearFilesBtnClick", App.convertType.ToString(), 1L);
      this.convertFilesList.Clear();
      this.updateFileItemListCBStatus();
    }
    catch
    {
    }
  }

  private async void ConvertBtn_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("ConvertMainWin", "ConvertBtnClick", App.convertType.ToString(), 1L);
    if (this.convertFilesList == null || this.convertFilesList.Count <= 0)
    {
      int num1 = (int) ModernMessageBox.Show(pdfconverter.Properties.Resources.FileConvertMsgAddFileNoFile, UtilManager.GetProductName());
    }
    else
    {
      OutputFormat format = this.getOutputFormat();
      if (format == OutputFormat.Invalid)
      {
        int num2 = (int) ModernMessageBox.Show(pdfconverter.Properties.Resources.FileConvertMsgInvalidForat, UtilManager.GetProductName());
      }
      else
      {
        bool? viewFile = this.viewFileAfterConvertCB.IsChecked;
        bool? isChecked1 = this.OCRCB.IsChecked;
        bool? isChecked2 = this.ConvertOnlineCB.IsChecked;
        bool? isChecked3 = this.SingleSheetBtn.IsChecked;
        ConvertFileItem fileItemToView = (ConvertFileItem) null;
        if (isChecked1.GetValueOrDefault())
          this.OCRInit();
        int selectCount = 0;
        int convertSucc = 0;
        List<Task> taskList = new List<Task>();
        foreach (ConvertFileItem convertFiles in (Collection<ConvertFileItem>) this.convertFilesList)
        {
          ConvertFileItem it = convertFiles;
          bool? nullable = it.FileSelected;
          bool flag1 = false;
          if (!(nullable.GetValueOrDefault() == flag1 & nullable.HasValue) && !string.IsNullOrWhiteSpace(it.convertFile))
          {
            ++selectCount;
            it.WithOCR = isChecked1;
            it.SingleSheet = isChecked3;
            it.ConvertStatus = FileCovertStatus.ConvertCoverting;
            string destDir = this.getUserSetOutputPath(it.convertFile);
            if (string.IsNullOrWhiteSpace(destDir))
            {
              it.ConvertStatus = FileCovertStatus.ConvertFail;
            }
            else
            {
              nullable = this.ConvertOnlineCB.IsChecked;
              bool IsOnline = (nullable.Value ? 1 : 0) != 0;
              Task task = Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
              {
                GAManager.SendEvent("PDFConvert", "ConvertFile", App.convertType.ToString(), 1L);
                bool flag2 = false;
                try
                {
                  flag2 = this.doConvert(it, format, destDir, IsOnline);
                }
                catch
                {
                }
                if (flag2)
                {
                  GAManager.SendEvent("PDFConvert", "ConvertSucc", App.convertType.ToString(), 1L);
                  it.ConvertStatus = FileCovertStatus.ConvertSucc;
                  ++convertSucc;
                  if (viewFile.GetValueOrDefault() && fileItemToView == null)
                    fileItemToView = it;
                }
                else
                {
                  GAManager.SendEvent("PDFConvert", "ConvertFail", App.convertType.ToString(), 1L);
                  it.ConvertStatus = FileCovertStatus.ConvertFail;
                }
                this.actionAfterConvert(it, viewFile);
              })));
              taskList.Add(task);
            }
          }
        }
        await Task.WhenAll((IEnumerable<Task>) taskList).ConfigureAwait(true);
        if (selectCount <= 0)
        {
          int num3 = (int) ModernMessageBox.Show(pdfconverter.Properties.Resources.FileConvertMsgAddFileOneFile, UtilManager.GetProductName());
        }
        else
        {
          GAManager.SendEvent("ConvertMainWin", "ConvertFileCount", selectCount.ToString(), 1L);
          if (viewFile.GetValueOrDefault() && fileItemToView != null)
          {
            if (!fileItemToView.outputFileIsDir)
              UtilsManager.OpenFileInExplore(fileItemToView.outputFile, true);
            else
              UtilsManager.OpenFileInExplore(fileItemToView.outputFile, false);
          }
          if (convertSucc == selectCount)
            ;
          else if (MessageBox.Show(pdfconverter.Properties.Resources.FileConvertMsgConvertFailSupport, UtilManager.GetProductName(), MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) != MessageBoxResult.OK)
            ;
          else
          {
            FeedbackWindow feedbackWindow = new FeedbackWindow();
            if (feedbackWindow == null)
              ;
            else
            {
              try
              {
                foreach (ConvertFileItem convertFiles in (Collection<ConvertFileItem>) this.convertFilesList)
                {
                  if (convertFiles.ConvertStatus == FileCovertStatus.ConvertFail)
                    feedbackWindow.flist.Add(convertFiles.convertFile);
                }
              }
              catch
              {
              }
              feedbackWindow.source = "2";
              feedbackWindow.Owner = Application.Current.MainWindow;
              feedbackWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
              feedbackWindow.showAttachmentCB(true);
              feedbackWindow.ShowDialog();
            }
          }
        }
      }
    }
  }

  private OutputFormat getOutputFormat()
  {
    if (this.outputFormatCB.SelectedItem == this.outputCBItem_docx)
      return OutputFormat.Docx;
    if (this.outputFormatCB.SelectedItem == this.outputCBItem_rtf)
      return OutputFormat.Rtf;
    if (this.outputFormatCB.SelectedItem == this.outputCBItem_xls)
      return OutputFormat.Xls;
    if (this.outputFormatCB.SelectedItem == this.outputCBItem_html)
      return OutputFormat.Html;
    if (this.outputFormatCB.SelectedItem == this.outputCBItem_xml)
      return OutputFormat.Xml;
    if (this.outputFormatCB.SelectedItem == this.outputCBItem_text)
      return OutputFormat.Text;
    if (this.outputFormatCB.SelectedItem == this.outputCBItem_png)
      return OutputFormat.Png;
    if (this.outputFormatCB.SelectedItem == this.outputCBItem_jpeg)
      return OutputFormat.Jpeg;
    return this.outputFormatCB.SelectedItem == this.outputCBItem_pptx ? OutputFormat.Ppt : OutputFormat.Invalid;
  }

  private string getUserSetOutputPath(string srcPdf)
  {
    if (string.IsNullOrWhiteSpace(srcPdf))
      return (string) null;
    if (!string.IsNullOrWhiteSpace(this.outputPathTB.Text) && Directory.Exists(this.outputPathTB.Text))
    {
      ConfigManager.SetConvertPath(this.outputPathTB.Text);
      return this.outputPathTB.Text;
    }
    if (string.IsNullOrWhiteSpace(this.outputPathTB.Text))
      return (string) null;
    if (!Directory.Exists(this.outputPathTB.Text))
      Directory.CreateDirectory(this.outputPathTB.Text);
    ConfigManager.SetConvertPath(this.outputPathTB.Text);
    return this.outputPathTB.Text;
  }

  private string getVaildOutputFile(string fileNameWithoutExt, OutputFormat format, string destDir)
  {
    if (string.IsNullOrWhiteSpace(fileNameWithoutExt) || string.IsNullOrWhiteSpace(destDir))
      return (string) null;
    string outputExt = ConvertManager.getOutputExt(format);
    if (string.IsNullOrWhiteSpace(outputExt))
      return (string) null;
    string path1 = $"{destDir}\\{fileNameWithoutExt} conv{outputExt}";
    if (!File.Exists(path1) && !Directory.Exists(path1))
      return path1;
    for (int index = 1; index < 100; ++index)
    {
      string path2 = $"{destDir}\\{fileNameWithoutExt} conv{$" {index}"}{outputExt}";
      if (!File.Exists(path2) && !Directory.Exists(path2))
        return path2;
    }
    return (string) null;
  }

  private string getValidOutputDir(string dirName, string destDir)
  {
    if (string.IsNullOrWhiteSpace(dirName) || string.IsNullOrWhiteSpace(destDir))
      return (string) null;
    string str = dirName + " conv";
    for (int index = 1; index < 100; ++index)
    {
      string path = $"{destDir}\\{str}";
      if (!Directory.Exists(path) && !File.Exists(path))
        return path;
      str = $"{dirName} conv{$" {index}"}";
    }
    return (string) null;
  }

  private string getVaildSubFile(
    string fileNameWithoutExt,
    OutputFormat format,
    string destDir,
    int index)
  {
    if (string.IsNullOrWhiteSpace(fileNameWithoutExt) || string.IsNullOrWhiteSpace(destDir))
      return (string) null;
    string outputExt = ConvertManager.getOutputExt(format);
    if (string.IsNullOrWhiteSpace(outputExt))
      return (string) null;
    string path = $"{destDir}\\{fileNameWithoutExt} conv{$" {index}"}{outputExt}";
    return !File.Exists(path) && !Directory.Exists(path) ? path : (string) null;
  }

  private void actionAfterConvert(ConvertFileItem item, bool? viewFile)
  {
    if (item == null)
      return;
    item.FileSelected = new bool?(false);
  }

  private bool doConvert(ConvertFileItem item, OutputFormat format, string destDir, bool Online)
  {
    if (string.IsNullOrWhiteSpace(destDir))
      return false;
    string withoutExtension = Path.GetFileNameWithoutExtension(item.convertFile);
    if (string.IsNullOrWhiteSpace(withoutExtension))
      return false;
    FileInfo fileInfo = new FileInfo(item.convertFile);
    if (fileInfo != null && fileInfo.Exists && fileInfo.Length > 0L)
      GAManager.SendEvent("PDFFSize", "ConvertFromPDF", ((int) (fileInfo.Length / 1024L /*0x0400*/ / 1024L /*0x0400*/ / 10L)).ToString(), 1L);
    switch (format)
    {
      case OutputFormat.Docx:
      case OutputFormat.Rtf:
      case OutputFormat.Xls:
      case OutputFormat.Html:
      case OutputFormat.Xml:
      case OutputFormat.Text:
      case OutputFormat.Ppt:
        string vaildOutputFile = this.getVaildOutputFile(withoutExtension, format, destDir);
        if (string.IsNullOrWhiteSpace(vaildOutputFile))
          return false;
        bool flag1;
        if (!Online)
        {
          flag1 = this.doConvert_OutputSingleFile(format, item.convertFile, vaildOutputFile, item.PageFrom, item.PageTo, item.WithOCR, item.PassWord, item.SingleSheet);
          if (!flag1 && MessageBox.Show(pdfconverter.Properties.Resources.ConvertFailedByLocalTipMsg, UtilManager.GetProductName(), MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
          {
            this.ConvertOnlineCB.Dispatcher.Invoke((Action) (() => this.ConvertOnlineCB.IsChecked = new bool?(true)));
            flag1 = this.doConvertOnline_SingleFile(item, format, vaildOutputFile).Result;
          }
        }
        else
        {
          flag1 = this.doConvertOnline_SingleFile(item, format, vaildOutputFile).Result;
          if (!flag1)
            flag1 = this.doConvert_OutputSingleFile(format, item.convertFile, vaildOutputFile, item.PageFrom, item.PageTo, item.WithOCR, item.PassWord, item.SingleSheet);
        }
        if (flag1)
        {
          item.outputFile = vaildOutputFile;
          item.outputFileIsDir = false;
          return true;
        }
        if (format == OutputFormat.Xls)
        {
          int num = (int) MessageBox.Show(pdfconverter.Properties.Resources.MainWinConvertToExcelFaildSuggest);
        }
        return false;
      case OutputFormat.Png:
      case OutputFormat.Jpeg:
        string validOutputDir = this.getValidOutputDir(withoutExtension.Trim(), destDir);
        if (string.IsNullOrWhiteSpace(validOutputDir))
          return false;
        try
        {
          Directory.CreateDirectory(validOutputDir);
        }
        catch
        {
          return false;
        }
        bool flag2;
        if (!Online)
        {
          flag2 = this.doConvert_OutputMultiFiles(format, item.convertFile, withoutExtension, validOutputDir, item.PageFrom, item.PageTo, item.PassWord);
          if (!flag2 && MessageBox.Show(pdfconverter.Properties.Resources.ConvertFailedByLocalTipMsg, UtilManager.GetProductName(), MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
          {
            this.ConvertOnlineCB.Dispatcher.Invoke((Action) (() => this.ConvertOnlineCB.IsChecked = new bool?(true)));
            flag2 = this.doConvertOnline_SingleFile(item, format, validOutputDir, withoutExtension).Result;
          }
        }
        else
        {
          flag2 = this.doConvertOnline_SingleFile(item, format, validOutputDir, withoutExtension).Result;
          if (!flag2)
            flag2 = this.doConvert_OutputMultiFiles(format, item.convertFile, withoutExtension, validOutputDir, item.PageFrom, item.PageTo, item.PassWord);
        }
        if (!flag2)
          return false;
        item.outputFile = validOutputDir;
        item.outputFileIsDir = true;
        return true;
      default:
        return false;
    }
  }

  private async Task<bool> doConvertOnline_SingleFile(
    ConvertFileItem item,
    OutputFormat format,
    string destDir,
    string fileNameWithOutExtension = "")
  {
    try
    {
      if (string.IsNullOrWhiteSpace(destDir) || !string.IsNullOrEmpty(item.PassWord))
        return false;
      bool debugFlag = ConfigManager.GetDebugMode();
      ConnectModel model = this.OCRInitParaOnline(item);
      string splitFilePath = await this.SplitFileIfNeed(model, item);
      GAManager.SendEvent("OnlineConvert", "RequestService", format.ToString(), 1L);
      OnlineAuthResponsModel authResponsModel = await OnlineConvertUtils.IsServiceOnline(model);
      if (authResponsModel.Success)
      {
        if (debugFlag)
        {
          int num1 = (int) MessageBox.Show("Debug Message:Connect Service successful.");
        }
        string temPath = item.convertFile;
        if (!string.IsNullOrEmpty(splitFilePath))
          temPath = splitFilePath;
        GAManager.SendEvent("OnlineConvert", "RequestConvert", format.ToString(), 1L);
        int num2 = await OnlineConvertUtils.RequestConvertFile(temPath, destDir, authResponsModel.Token, model, fileNameWithOutExtension) ? 1 : 0;
        ConfigManager.AddOnlineRequestCount();
        if (num2 == 0)
        {
          GAManager.SendEvent("OnlineConvert", "RequestConvertFail", format.ToString(), 1L);
          if (debugFlag)
          {
            int num3 = (int) MessageBox.Show("Debug Message:Convert file online fail.");
          }
          if (Directory.Exists(destDir))
          {
            Directory.Delete(destDir, true);
            Directory.CreateDirectory(destDir);
          }
        }
        if (model.pageFrom != 1 || model.pageTo != model.pageCount)
          File.Delete(temPath);
        return num2 != 0;
      }
      if (debugFlag)
      {
        int num = (int) MessageBox.Show("Debug Message:Connect Service fail.");
      }
      return false;
    }
    catch
    {
      return false;
    }
  }

  private bool doConvert_OutputSingleFile(
    OutputFormat format,
    string srcFile,
    string outputFile,
    int fromPage,
    int toPage,
    bool? withOCR,
    string password,
    bool? singleSheet)
  {
    PdfFocus pdfFocus = new PdfFocus();
    pdfFocus.Serial = ConvertSDK.SautinSoftSerial;
    if (!string.IsNullOrEmpty(password))
      pdfFocus.Password = password;
    pdfFocus.OpenPdf(srcFile);
    int pageCount = pdfFocus.PageCount;
    if (pageCount <= 0)
    {
      pdfFocus.ClosePdf();
      return false;
    }
    if (withOCR.GetValueOrDefault())
    {
      pdfFocus.OCROptions.Method = new PdfFocus.COCROptions.OCRMethod(this.PerformOCRNicomsoft);
      pdfFocus.OCROptions.Mode = PdfFocus.COCROptions.eOCRMode.AllImages;
      pdfFocus.WordOptions.KeepCharScaleAndSpacing = false;
    }
    int num1 = -1;
    switch (format)
    {
      case OutputFormat.Docx:
        pdfFocus.WordOptions.Format = PdfFocus.CWordOptions.eWordDocument.Docx;
        num1 = pdfFocus.ToWord(outputFile, fromPage, toPage);
        break;
      case OutputFormat.Rtf:
        pdfFocus.WordOptions.Format = PdfFocus.CWordOptions.eWordDocument.Rtf;
        num1 = pdfFocus.ToWord(outputFile, fromPage, toPage);
        break;
      case OutputFormat.Xls:
        pdfFocus.ExcelOptions.SingleSheet = singleSheet.Value;
        num1 = pdfFocus.ToExcel(outputFile, fromPage, toPage);
        break;
      case OutputFormat.Html:
        num1 = pdfFocus.ToHtml(outputFile, fromPage, toPage);
        break;
      case OutputFormat.Xml:
        num1 = pdfFocus.ToXml(outputFile, fromPage, toPage);
        break;
      case OutputFormat.Text:
        num1 = pdfFocus.ToText(outputFile, fromPage, toPage);
        break;
      case OutputFormat.Ppt:
        SizeF sizeF = new SizeF();
        bool flag1 = false;
        bool flag2 = false;
        for (int pageNumber = fromPage; pageNumber <= toPage; ++pageNumber)
        {
          SizeF pageSize = pdfFocus.GetPageSize(pageNumber);
          if ((double) pageSize.Width > (double) sizeF.Width && !flag2)
          {
            if ((double) pageSize.Width > MainWindow.PPTMaximum)
            {
              sizeF.Width = (float) MainWindow.PPTMaximum;
              flag2 = true;
            }
            else
              sizeF.Width = pageSize.Width;
          }
          if ((double) pageSize.Height > (double) sizeF.Height && !flag1)
          {
            if ((double) pageSize.Height > MainWindow.PPTMaximum)
            {
              sizeF.Height = (float) MainWindow.PPTMaximum;
              flag1 = true;
            }
            else
              sizeF.Height = pageSize.Height;
          }
          if (flag1 & flag2)
            break;
        }
        IPresentation presentation = Syncfusion.Presentation.Presentation.Create();
        presentation.Masters[0].SlideSize.Width = (double) sizeF.Width;
        presentation.Masters[0].SlideSize.Height = (double) sizeF.Height;
        try
        {
          pdfFocus.ImageOptions.ImageFormat = ImageFormat.Png;
          pdfFocus.ImageOptions.Dpi = MainWindow.PDFToImageDPI;
          int num2 = 0;
          for (int index = fromPage; index <= toPage; ++index)
          {
            try
            {
              ISlide slide = presentation.Slides.Add(SlideLayoutType.Blank);
              byte[] image = pdfFocus.ToImage(index);
              SizeF pageSize = pdfFocus.GetPageSize(index);
              double left = (presentation.Masters[0].SlideSize.Width - (double) pageSize.Width) / 2.0;
              double top = (presentation.Masters[0].SlideSize.Height - (double) pageSize.Height) / 2.0;
              using (Stream stream = (Stream) new MemoryStream(image))
                slide.Pictures.AddPicture(stream, left, top, (double) pageSize.Width, (double) pageSize.Height);
              ++num2;
            }
            catch
            {
            }
          }
          num1 = num2 == 0 ? -1 : 0;
          break;
        }
        catch
        {
          num1 = -1;
          break;
        }
        finally
        {
          presentation.Save(outputFile);
          presentation.Close();
          presentation.Dispose();
        }
    }
    pdfFocus.ClosePdf();
    GAManager.SendEvent("ConvertPages", "PageCount", (toPage - fromPage + 1).ToString(), 1L);
    if (fromPage == 1 && toPage == pageCount)
      GAManager.SendEvent("ConvertPages", "WholePages", format.ToString(), 1L);
    else
      GAManager.SendEvent("ConvertPages", "PartPages", format.ToString(), 1L);
    if (num1 == 0 && File.Exists(outputFile))
    {
      GAManager.SendEvent("ConvertApi", "Succ", format.ToString(), 1L);
      return true;
    }
    GAManager.SendEvent("ConvertApi", "Fail", format.ToString(), 1L);
    GAManager.SendEvent("SDKConvertError", format.ToString(), num1.ToString(), 1L);
    return false;
  }

  private bool doConvert_OutputMultiFiles(
    OutputFormat format,
    string srcFile,
    string fileNameWithoutExt,
    string outputDir,
    int fromPage,
    int toPage,
    string password)
  {
    PdfFocus pdfFocus = new PdfFocus();
    pdfFocus.Serial = ConvertSDK.SautinSoftSerial;
    if (!string.IsNullOrEmpty(password))
      pdfFocus.Password = password;
    pdfFocus.OpenPdf(srcFile);
    int pageCount = pdfFocus.PageCount;
    if (pageCount <= 0)
    {
      pdfFocus.ClosePdf();
      return false;
    }
    bool flag = false;
    switch (format)
    {
      case OutputFormat.Png:
        pdfFocus.ImageOptions.ImageFormat = ImageFormat.Png;
        pdfFocus.ImageOptions.Dpi = MainWindow.PDFToImageDPI;
        break;
      case OutputFormat.Jpeg:
        pdfFocus.ImageOptions.ImageFormat = ImageFormat.Jpeg;
        pdfFocus.ImageOptions.Dpi = MainWindow.PDFToImageDPI;
        break;
    }
    for (int index = fromPage; index <= toPage; ++index)
    {
      string vaildSubFile = this.getVaildSubFile(fileNameWithoutExt, format, outputDir, index);
      if (!string.IsNullOrWhiteSpace(vaildSubFile))
      {
        int image = pdfFocus.ToImage(vaildSubFile, index);
        if (image == 0 && File.Exists(vaildSubFile))
          flag = true;
        else
          GAManager.SendEvent("SDKConvertError", format.ToString(), image.ToString(), 1L);
      }
    }
    pdfFocus.ClosePdf();
    GAManager.SendEvent("ConvertPages", "PageCount", (toPage - fromPage + 1).ToString(), 1L);
    if (fromPage == 1 && toPage == pageCount)
      GAManager.SendEvent("ConvertPages", "WholePages", format.ToString(), 1L);
    else
      GAManager.SendEvent("ConvertPages", "PartPages", format.ToString(), 1L);
    if (flag)
      GAManager.SendEvent("ConvertApi", "Succ", format.ToString(), 1L);
    else
      GAManager.SendEvent("ConvertApi", "Fail", format.ToString(), 1L);
    return flag;
  }

  private void updateFileItemListCBStatus()
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    bool? nullable = new bool?(false);
    foreach (ConvertFileItem convertFiles in (Collection<ConvertFileItem>) this.convertFilesList)
    {
      if (convertFiles.ConvertStatus != FileCovertStatus.ConvertUnsupport)
      {
        ++num3;
        if (convertFiles.FileSelected.GetValueOrDefault())
          ++num1;
        else
          ++num2;
      }
    }
    nullable = num3 > 0 ? (num3 != num1 ? (num3 != num2 ? new bool?() : new bool?(false)) : new bool?(true)) : new bool?(false);
    if (this.fileItemListCB == null)
      return;
    this.fileItemListCB.IsChecked = nullable;
  }

  private void FileItemCB_Checked(object sender, RoutedEventArgs e)
  {
    this.updateFileItemListCBStatus();
  }

  private void FileItemCB_Unchecked(object sender, RoutedEventArgs e)
  {
    this.updateFileItemListCBStatus();
  }

  private void FileItemListCB_Click(object sender, RoutedEventArgs e)
  {
    CheckBox checkBox = (CheckBox) sender;
    bool? nullable = checkBox.IsChecked;
    if (!nullable.HasValue)
    {
      checkBox.IsChecked = new bool?(false);
      nullable = new bool?(false);
    }
    bool? fileSelected;
    foreach (ConvertFileItem convertFiles in (Collection<ConvertFileItem>) this.convertFilesList)
    {
      if (convertFiles.ConvertStatus != FileCovertStatus.ConvertUnsupport && convertFiles.ConvertStatus != FileCovertStatus.ConvertLoadedFailed)
      {
        if (nullable.GetValueOrDefault())
        {
          fileSelected = convertFiles.FileSelected;
          bool flag = false;
          if (fileSelected.GetValueOrDefault() == flag & fileSelected.HasValue)
            convertFiles.FileSelected = new bool?(true);
        }
        else
        {
          fileSelected = convertFiles.FileSelected;
          if (fileSelected.GetValueOrDefault())
            convertFiles.FileSelected = new bool?(false);
        }
      }
    }
  }

  private void FileItemListCB_Loaded(object sender, RoutedEventArgs e)
  {
    this.fileItemListCB = (CheckBox) sender;
  }

  private void OCRInit()
  {
    TNSOCR.Engine_SetLicenseKey("AB2A4DD5FF2A");
    TNSOCR.Engine_InitializeAdvanced(out this.CfgObj, out this.OcrObj, out this.ImgObj);
    this.NsOCRLanguage = ConvertManager.getOCRLanguage();
    if (this.ConvertOnlineCB.IsChecked.GetValueOrDefault())
      this.NsOCROnlineLanguage = ConvertManager.GetOCROnlineLanguage();
    if (!string.IsNullOrWhiteSpace(this.NsOCRLanguage))
      TNSOCR.Cfg_SetOption(this.CfgObj, 0, this.NsOCRLanguage, "1");
    else
      TNSOCR.Cfg_SetOption(this.CfgObj, 0, "Languages/English", "1");
  }

  private ConnectModel OCRInitParaOnline(ConvertFileItem item)
  {
    ConnectModel connectModel = new ConnectModel();
    try
    {
      connectModel.appVersion = UtilManager.GetAppVersion();
      connectModel.utcTimestamp = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
      connectModel.convertType = MapOnlineConverttype.GetOnlineConvertStr((ConvFromPDFType) App.convertType);
      connectModel.uuid = UtilManager.GetUUID();
      connectModel.fileName = Path.GetFileName(item.convertFile);
      connectModel.convertCountToday = ConfigManager.GetOnlineRequestCount() + 1;
      connectModel.fileSize = new FileInfo(item.convertFile).Length;
      connectModel.pageCount = item.PageCount;
      connectModel.pageFrom = item.PageFrom;
      connectModel.pageTo = item.PageTo;
      connectModel.needOcr = item.WithOCR.Value;
      connectModel.OcrLang = this.NsOCROnlineLanguage;
    }
    catch
    {
      return (ConnectModel) null;
    }
    return connectModel;
  }

  private async Task<string> SplitFileIfNeed(ConnectModel model, ConvertFileItem item)
  {
    if (model.pageFrom == 1 && model.pageTo == model.pageCount || model.pageFrom == 0)
      return (string) null;
    string fileName = await TempFileUtils.SplitFileInRangeAsync(item.convertFile, model.pageFrom, model.pageTo);
    model.fileSize = new FileInfo(fileName).Length;
    return fileName;
  }

  public byte[] PerformOCRNicomsoft(byte[] image)
  {
    int CfgObj = 0;
    int OcrObj = 0;
    int ImgObj = 0;
    int SvrObj = 0;
    TNSOCR.Engine_SetLicenseKey("AB2A4DD5FF2A");
    TNSOCR.Engine_InitializeAdvanced(out CfgObj, out OcrObj, out ImgObj);
    TNSOCR.Cfg_SetOption(CfgObj, 0, "ImgAlizer/AutoScale", "0");
    TNSOCR.Cfg_SetOption(CfgObj, 0, "ImgAlizer/ScaleFactor", "4.0");
    this.NsOCRLanguage = ConvertManager.getOCRLanguage();
    if (!string.IsNullOrWhiteSpace(this.NsOCRLanguage))
      TNSOCR.Cfg_SetOption(CfgObj, 0, this.NsOCRLanguage, "1");
    else
      TNSOCR.Cfg_SetOption(CfgObj, 0, "Languages/English", "1");
    Array array = (Array) null;
    using (MemoryStream memoryStream = new MemoryStream(image))
    {
      memoryStream.Flush();
      array = (Array) memoryStream.ToArray();
    }
    GCHandle gcHandle1 = GCHandle.Alloc((object) array, GCHandleType.Pinned);
    IntPtr Bytes1 = gcHandle1.AddrOfPinnedObject();
    int num1 = TNSOCR.Img_LoadFromMemory(ImgObj, Bytes1, array.Length);
    gcHandle1.Free();
    int num2;
    if (num1 > 1879048192 /*0x70000000*/)
    {
      if (ImgObj != 0)
      {
        TNSOCR.Img_Unload(ImgObj);
        TNSOCR.Img_Destroy(ImgObj);
        num2 = 0;
      }
      return (byte[]) null;
    }
    TNSOCR.Svr_Create(CfgObj, 1, out SvrObj);
    TNSOCR.Svr_NewDocument(SvrObj);
    if (TNSOCR.Img_OCR(ImgObj, 0, (int) byte.MaxValue, 0) > 1879048192 /*0x70000000*/)
    {
      if (ImgObj != 0)
      {
        TNSOCR.Img_Unload(ImgObj);
        TNSOCR.Img_Destroy(ImgObj);
        num2 = 0;
      }
      return (byte[]) null;
    }
    if (TNSOCR.Svr_AddPage(SvrObj, ImgObj, 1) > 1879048192 /*0x70000000*/)
    {
      if (ImgObj != 0)
      {
        TNSOCR.Img_Unload(ImgObj);
        TNSOCR.Img_Destroy(ImgObj);
        num2 = 0;
      }
      return (byte[]) null;
    }
    if (ImgObj != 0)
    {
      TNSOCR.Img_Unload(ImgObj);
      TNSOCR.Img_Destroy(ImgObj);
      num2 = 0;
    }
    int memory1 = TNSOCR.Svr_SaveToMemory(SvrObj, (IntPtr) 0, 0);
    int num3;
    if (memory1 <= 0)
    {
      if (SvrObj != 0)
      {
        TNSOCR.Svr_Destroy(SvrObj);
        num3 = 0;
      }
      return (byte[]) null;
    }
    byte[] numArray = new byte[memory1];
    GCHandle gcHandle2 = GCHandle.Alloc((object) numArray, GCHandleType.Pinned);
    IntPtr Bytes2 = gcHandle2.AddrOfPinnedObject();
    int memory2 = TNSOCR.Svr_SaveToMemory(SvrObj, Bytes2, memory1);
    gcHandle2.Free();
    if (SvrObj != 0)
    {
      TNSOCR.Svr_Destroy(SvrObj);
      num3 = 0;
    }
    return memory2 > 1879048192 /*0x70000000*/ ? (byte[]) null : numArray;
  }

  private bool showOCRSettingsWindow()
  {
    OCRSettingsWindow ocrSettingsWindow = new OCRSettingsWindow();
    ocrSettingsWindow.Owner = (Window) this;
    ocrSettingsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    ocrSettingsWindow.ShowDialog();
    return ocrSettingsWindow.ret;
  }

  private void OCRCB_Checked(object sender, RoutedEventArgs e)
  {
    if (this.showOCRSettingsWindow())
    {
      this.curLang.Text = ConvertManager.getOCRLanguageL10N();
      this.curLangLink.Foreground = (System.Windows.Media.Brush) new SolidColorBrush(Colors.Red);
      this.curLangLink.IsEnabled = true;
      this.OCRCB.IsChecked = new bool?(true);
    }
    else
      this.OCRCB.IsChecked = new bool?(false);
  }

  private void OCRCB_Unchecked(object sender, RoutedEventArgs e)
  {
    this.curLangLink.Foreground = (System.Windows.Media.Brush) new SolidColorBrush(Colors.Gray);
    this.curLangLink.IsEnabled = false;
  }

  private void CurLang_Click(object sender, RoutedEventArgs e)
  {
    if (!this.OCRCB.IsChecked.GetValueOrDefault() || !this.showOCRSettingsWindow())
      return;
    this.curLang.Text = ConvertManager.getOCRLanguageL10N();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfconverter;component/mainwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        ((UIElement) target).Drop += new DragEventHandler(this.Grid_Drop);
        break;
      case 2:
        this.titleTB = (TextBlock) target;
        break;
      case 3:
        this.addFileBtn = (Button) target;
        this.addFileBtn.Click += new RoutedEventHandler(this.AddFileBtn_Click);
        break;
      case 4:
        this.convertFilesView = (ListView) target;
        break;
      case 11:
        this.outputPathTB = (TextBox) target;
        break;
      case 12:
        this.changeDestPathBtn = (Button) target;
        this.changeDestPathBtn.Click += new RoutedEventHandler(this.ChangeDestPathBtn_Click);
        break;
      case 13:
        this.ocrGrid = (Grid) target;
        break;
      case 14:
        this.OCRCB = (CheckBox) target;
        this.OCRCB.Checked += new RoutedEventHandler(this.OCRCB_Checked);
        this.OCRCB.Unchecked += new RoutedEventHandler(this.OCRCB_Unchecked);
        break;
      case 15:
        this.ocrSettingsPanel = (StackPanel) target;
        break;
      case 16 /*0x10*/:
        this.curLangLink = (Hyperlink) target;
        this.curLangLink.Click += new RoutedEventHandler(this.CurLang_Click);
        break;
      case 17:
        this.curLang = (TextBlock) target;
        break;
      case 18:
        this.excelGrid = (Grid) target;
        break;
      case 19:
        this.SingleSheetBtn = (CheckBox) target;
        break;
      case 20:
        this.outputFormatCB = (ComboBox) target;
        break;
      case 21:
        this.outputCBItem_docx = (ComboBoxItem) target;
        break;
      case 22:
        this.outputCBItem_rtf = (ComboBoxItem) target;
        break;
      case 23:
        this.outputCBItem_xls = (ComboBoxItem) target;
        break;
      case 24:
        this.outputCBItem_html = (ComboBoxItem) target;
        break;
      case 25:
        this.outputCBItem_xml = (ComboBoxItem) target;
        break;
      case 26:
        this.outputCBItem_text = (ComboBoxItem) target;
        break;
      case 27:
        this.outputCBItem_png = (ComboBoxItem) target;
        break;
      case 28:
        this.outputCBItem_jpeg = (ComboBoxItem) target;
        break;
      case 29:
        this.outputCBItem_pptx = (ComboBoxItem) target;
        break;
      case 30:
        this.ConvertOnlineCB = (CheckBox) target;
        break;
      case 31 /*0x1F*/:
        this.viewFileAfterConvertCB = (CheckBox) target;
        break;
      case 32 /*0x20*/:
        this.convertBtn = (Button) target;
        this.convertBtn.Click += new RoutedEventHandler(this.ConvertBtn_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 5:
        ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.FileItemListCB_Loaded);
        ((ButtonBase) target).Click += new RoutedEventHandler(this.FileItemListCB_Click);
        break;
      case 6:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.ClearFilesBtn_Click);
        break;
      case 7:
        ((ToggleButton) target).Checked += new RoutedEventHandler(this.FileItemCB_Checked);
        ((ToggleButton) target).Unchecked += new RoutedEventHandler(this.FileItemCB_Unchecked);
        break;
      case 8:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OpenFileBtn_Click);
        break;
      case 9:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OpenFileInExploreBtn_Click);
        break;
      case 10:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.DeleteFileBtn_Click);
        break;
    }
  }
}
