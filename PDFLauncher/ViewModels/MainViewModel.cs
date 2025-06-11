// Decompiled with JetBrains decompiler
// Type: PDFLauncher.ViewModels.MainViewModel
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PDFLauncher.Models;
using PDFLauncher.Properties;
using PDFLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

#nullable enable
namespace PDFLauncher.ViewModels;

public class MainViewModel : ObservableObject
{
  private Visibility openBtnTipsVisibility = Visibility.Collapsed;
  private Visibility allToolsSwitchIsVisile = Visibility.Hidden;
  private Visibility pdfContextVisibility;
  private Visibility pdfMenuItemVisibility;
  private Visibility imgContextVisibility;
  private Visibility docContextVisibility;
  private Visibility xlsContextVisibility;
  private Visibility pptContextVisibility;
  private Visibility rtfContextVisibility;
  private Visibility txtContextVisibility;
  private Visibility otherFormatVisibility;
  private Visibility otherContextVisibility;
  private 
  #nullable disable
  ObservableCollection<OpenHistoryModel> historyModels;
  private bool? isAllFileSelect = new bool?(false);
  private bool allSwitchChecked;
  private bool clearbtnIsEnable;
  private readonly string wordExtention = ".docx;.doc";
  private readonly string excelExtention = ".xlsx;.xls";
  private readonly string pptxExtention = ".pptx;.ppt";
  private readonly string imageExtention = ".bmp;.ico;.jpeg;.jpg;.png";
  private readonly string rtfExtention = ".rtf";
  private readonly string txtExtention = ".txt";
  private List<string> menus = new List<string>();
  private List<OpenHistoryModel> selectedModels = new List<OpenHistoryModel>();
  private RelayCommand<object> compressPDF;
  private RelayCommand<object> pdftoWord;
  private RelayCommand<object> pdftoExcel;
  private RelayCommand<object> pdftoPng;
  private RelayCommand<object> pdftoRtf;
  private RelayCommand<object> pdftoTxt;
  private RelayCommand<object> pdftoWeb;
  private RelayCommand<object> pdftoXML;
  private RelayCommand<object> pdftoJPG;
  private RelayCommand<object> pdftoPPT;
  private RelayCommand<object> splitPDF;
  private RelayCommand<object> mergePDF;
  private RelayCommand<object> wordtoPDF;
  private RelayCommand<object> exceltoPDF;
  private RelayCommand<object> ppttoPDF;
  private RelayCommand<object> imgtoPDF;
  private RelayCommand<object> rtftoPDF;
  private RelayCommand<object> txttoPDF;
  private RelayCommand<object> xmltoPDF;
  private RelayCommand<object> webtoPDF;
  private RelayCommand<object> covtoPDF;
  private RelayCommand<object> runEditor;
  private RelayCommand<object> removeOne;
  private RelayCommand<object> openFolder;
  private RelayCommand clearSelectModel;
  private RelayCommand openOneFileCMD;
  private RelayCommand fillFormCMD;
  private RelayCommand createFileCMD;

  public MainViewModel()
  {
    this.ReadHistory();
    this.ReadPropertySource();
  }

  private void ReadPropertySource()
  {
    this.menus.Add(Resources.WinListHeaderTabHotTools);
    this.menus.Add(Resources.WinListHeaderConvertFromPDF);
    this.menus.Add(Resources.WinListHeaderConvertToPDF);
    this.menus.Add(Resources.WinListHeaderMergeOrSplit);
    this.menus.Add(Resources.WinListHeaderTabAllTools);
  }

  public bool ClearbtnIsEnable
  {
    get => this.clearbtnIsEnable;
    set => this.SetProperty<bool>(ref this.clearbtnIsEnable, value, nameof (ClearbtnIsEnable));
  }

  public Visibility OpenBtnTipsVisibility
  {
    get => this.openBtnTipsVisibility;
    set
    {
      this.SetProperty<Visibility>(ref this.openBtnTipsVisibility, value, nameof (OpenBtnTipsVisibility));
    }
  }

  public Visibility AllToolsSwitchIsVisile
  {
    get => this.allToolsSwitchIsVisile;
    set
    {
      this.SetProperty<Visibility>(ref this.allToolsSwitchIsVisile, value, nameof (AllToolsSwitchIsVisile));
    }
  }

  public Visibility PDFContextVisibility
  {
    get => this.pdfContextVisibility;
    set
    {
      this.SetProperty<Visibility>(ref this.pdfContextVisibility, value, nameof (PDFContextVisibility));
    }
  }

  public Visibility PDFMenuItemVisibility
  {
    get => this.pdfMenuItemVisibility;
    set
    {
      this.SetProperty<Visibility>(ref this.pdfMenuItemVisibility, value, nameof (PDFMenuItemVisibility));
    }
  }

  public Visibility OtherContextVisibility
  {
    get => this.otherContextVisibility;
    set
    {
      this.SetProperty<Visibility>(ref this.otherContextVisibility, value, nameof (OtherContextVisibility));
    }
  }

  public Visibility ImgContextVisibility
  {
    get => this.imgContextVisibility;
    set
    {
      this.SetProperty<Visibility>(ref this.imgContextVisibility, value, nameof (ImgContextVisibility));
    }
  }

  public Visibility DocContextVisibility
  {
    get => this.docContextVisibility;
    set
    {
      this.SetProperty<Visibility>(ref this.docContextVisibility, value, nameof (DocContextVisibility));
    }
  }

  public Visibility XlsContextVisibility
  {
    get => this.xlsContextVisibility;
    set
    {
      this.SetProperty<Visibility>(ref this.xlsContextVisibility, value, nameof (XlsContextVisibility));
    }
  }

  public Visibility PPTContextVisibility
  {
    get => this.pptContextVisibility;
    set
    {
      this.SetProperty<Visibility>(ref this.pptContextVisibility, value, nameof (PPTContextVisibility));
    }
  }

  public Visibility RTFContextVisibility
  {
    get => this.rtfContextVisibility;
    set
    {
      this.SetProperty<Visibility>(ref this.rtfContextVisibility, value, nameof (RTFContextVisibility));
    }
  }

  public Visibility TXTContextVisibility
  {
    get => this.txtContextVisibility;
    set
    {
      this.SetProperty<Visibility>(ref this.txtContextVisibility, value, nameof (TXTContextVisibility));
    }
  }

  public Visibility OtherFormatVisibility
  {
    get => this.otherFormatVisibility;
    set
    {
      this.SetProperty<Visibility>(ref this.otherFormatVisibility, value, nameof (OtherFormatVisibility));
    }
  }

  public ObservableCollection<OpenHistoryModel> HistoryModels
  {
    get
    {
      return this.historyModels ?? (this.historyModels = new ObservableCollection<OpenHistoryModel>());
    }
    set
    {
      this.SetProperty<ObservableCollection<OpenHistoryModel>>(ref this.historyModels, value, nameof (HistoryModels));
    }
  }

  public bool? IsAllFileSelect
  {
    get => this.isAllFileSelect;
    set => this.SetProperty<bool?>(ref this.isAllFileSelect, value, nameof (IsAllFileSelect));
  }

  public bool AllToolsSwitchIsChecked
  {
    get => this.allSwitchChecked;
    set
    {
      this.SetProperty<bool>(ref this.allSwitchChecked, value, nameof (AllToolsSwitchIsChecked));
    }
  }

  public void ReadHistory()
  {
    try
    {
      this.HistoryModels?.Clear();
      if (HistoryManager.ReadHistory() == null)
        return;
      foreach (OpenedInfo openedInfo in HistoryManager.ReadHistory().FilesPath)
      {
        if (Path.GetDirectoryName(openedInfo.FilePath) == Path.Combine(UtilManager.GetTemporaryPath(), "Documents"))
          HistoryManager.UpdateHistory(openedInfo.FilePath, true);
        else if (!File.Exists(openedInfo.FilePath))
        {
          OpenHistoryModel openHistoryModel = new OpenHistoryModel()
          {
            FilePath = openedInfo.FilePath,
            FileName = Path.GetFileName(openedInfo.FilePath),
            FileSize = "0"
          };
          long result = 0;
          long.TryParse(openedInfo.OpenDate, out result);
          openHistoryModel.FileLastOpen = new DateTime(result).ToString("yyyy/MM/dd HH:mm:ss");
          this.HistoryModels.Add(openHistoryModel);
        }
        else
        {
          OpenHistoryModel openHistoryModel = new OpenHistoryModel()
          {
            FilePath = openedInfo.FilePath,
            FileName = FileInfoUtils.GetFileName(openedInfo.FilePath),
            FileSize = FileInfoUtils.GetFileSize(openedInfo.FilePath)
          };
          long result = 0;
          long.TryParse(openedInfo.OpenDate, out result);
          openHistoryModel.FileLastOpen = new DateTime(result).ToString("yyyy/MM/dd HH:mm:ss");
          this.HistoryModels.Add(openHistoryModel);
        }
      }
      this.SelectItemsPropertyChange();
    }
    catch
    {
    }
  }

  public void SelectAll()
  {
    this.selectedModels = (List<OpenHistoryModel>) null;
    this.selectedModels = this.HistoryModels.ToList<OpenHistoryModel>();
    foreach (OpenHistoryModel selectedModel in this.selectedModels)
      selectedModel.IsSelect = true;
    this.IsAllFileSelect = new bool?(true);
    List<string> source = this.distinctFileFormat(this.HistoryModels.ToList<OpenHistoryModel>());
    if (source.Count == 1)
    {
      if (this.parseBtnVisibility(source.First<string>()))
        this.AllToolsSwitchIsVisile = Visibility.Visible;
      else
        this.AllToolsSwitchIsVisile = Visibility.Collapsed;
    }
    else
      this.AllToolsSwitchIsVisile = Visibility.Collapsed;
  }

  public void ClearHistory()
  {
    this.HistoryModels.Clear();
    this.selectedModels.Clear();
    HistoryManager.UpdateHistory(new FilesArgsModel());
    this.SelectItemsPropertyChange();
  }

  public void SelectItemsPropertyChange()
  {
    this.selectedModels = this.HistoryModels.Where<OpenHistoryModel>((Func<OpenHistoryModel, bool>) (x => x.IsSelect)).ToList<OpenHistoryModel>();
    if (this.selectedModels == null)
    {
      this.IsAllFileSelect = new bool?(false);
      this.allToolsSwitchIsVisile = Visibility.Hidden;
    }
    List<OpenHistoryModel> selectedModels1 = this.selectedModels;
    // ISSUE: explicit non-virtual call
    if ((selectedModels1 != null ? (__nonvirtual (selectedModels1.Count) == 0 ? 1 : 0) : 0) != 0)
    {
      this.IsAllFileSelect = new bool?(false);
      this.allToolsSwitchIsVisile = Visibility.Hidden;
    }
    else
    {
      List<OpenHistoryModel> selectedModels2 = this.selectedModels;
      // ISSUE: explicit non-virtual call
      if ((selectedModels2 != null ? (__nonvirtual (selectedModels2.Count) > 0 ? 1 : 0) : 0) != 0)
        this.IsAllFileSelect = this.selectedModels.Count != this.HistoryModels.Count ? new bool?() : new bool?(true);
    }
    List<OpenHistoryModel> selectedModels3 = this.selectedModels;
    // ISSUE: explicit non-virtual call
    if ((selectedModels3 != null ? (__nonvirtual (selectedModels3.Count) > 1 ? 1 : 0) : 0) != 0)
    {
      List<string> source = this.distinctFileFormat(this.selectedModels);
      this.AllToolsSwitchIsVisile = source.Count != 1 ? Visibility.Collapsed : (!this.parseBtnVisibility(source.First<string>()) ? Visibility.Collapsed : Visibility.Visible);
      this.ClearbtnIsEnable = true;
    }
    else
      this.AllToolsSwitchIsVisile = Visibility.Hidden;
  }

  private bool IsSupportedToPDFFormat(string ext)
  {
    return !string.IsNullOrEmpty(ext) && (this.wordExtention.Contains(ext) || this.excelExtention.Contains(ext) || this.pptxExtention.Contains(ext) || this.imageExtention.Contains(ext) || this.rtfExtention.Contains(ext) || this.txtExtention.Contains(ext));
  }

  public void SelectFilesSupportPropertChange(OpenHistoryModel model)
  {
    if (!string.IsNullOrEmpty(model.Extension))
    {
      if (model.Extension == ".pdf")
      {
        this.PDFContextVisibility = Visibility.Visible;
        this.OtherContextVisibility = Visibility.Collapsed;
        return;
      }
      if (this.IsSupportedToPDFFormat(model.Extension))
      {
        this.PDFContextVisibility = Visibility.Collapsed;
        this.OtherContextVisibility = Visibility.Visible;
        return;
      }
    }
    this.PDFContextVisibility = Visibility.Collapsed;
    this.OtherContextVisibility = Visibility.Collapsed;
  }

  public void SelectNone()
  {
    foreach (OpenHistoryModel historyModel in (Collection<OpenHistoryModel>) this.HistoryModels)
      historyModel.IsSelect = false;
    this.IsAllFileSelect = new bool?(false);
    this.AllToolsSwitchIsVisile = Visibility.Collapsed;
    this.selectedModels = (List<OpenHistoryModel>) null;
  }

  private bool parseBtnVisibility(string extention)
  {
    bool btnVisibility = false;
    this.DocContextVisibility = Visibility.Collapsed;
    this.XlsContextVisibility = Visibility.Collapsed;
    this.ImgContextVisibility = Visibility.Collapsed;
    this.PPTContextVisibility = Visibility.Collapsed;
    this.RTFContextVisibility = Visibility.Collapsed;
    this.TXTContextVisibility = Visibility.Collapsed;
    this.PDFMenuItemVisibility = Visibility.Collapsed;
    if (this.wordExtention.Contains(extention))
    {
      this.DocContextVisibility = Visibility.Visible;
      btnVisibility = true;
    }
    else if (this.excelExtention.Contains(extention))
    {
      this.XlsContextVisibility = Visibility.Visible;
      btnVisibility = true;
    }
    else if (this.imageExtention.Contains(extention))
    {
      this.ImgContextVisibility = Visibility.Visible;
      btnVisibility = true;
    }
    else if (this.pptxExtention.Contains(extention))
    {
      this.PPTContextVisibility = Visibility.Visible;
      btnVisibility = true;
    }
    else if (this.rtfExtention.Contains(extention))
    {
      this.RTFContextVisibility = Visibility.Visible;
      btnVisibility = true;
    }
    else if (this.txtExtention.Contains(extention))
    {
      this.TXTContextVisibility = Visibility.Visible;
      btnVisibility = true;
    }
    else if (extention == ".pdf")
    {
      this.PDFMenuItemVisibility = Visibility.Visible;
      btnVisibility = true;
    }
    return btnVisibility;
  }

  private List<string> distinctFileFormat(List<OpenHistoryModel> models)
  {
    if (models == null)
      return new List<string>();
    List<string> stringList = new List<string>();
    foreach (OpenHistoryModel model in models)
    {
      if (string.IsNullOrEmpty(model.Extension))
      {
        if (!stringList.Contains(".notsupport"))
          stringList.Add(".notsupport");
      }
      else if (model.Extension == ".pdf")
      {
        if (!stringList.Contains(".pdf"))
          stringList.Add(".pdf");
      }
      else if (this.wordExtention.Contains(model.Extension))
      {
        if (!stringList.Contains(".doc"))
          stringList.Add(".doc");
      }
      else if (this.excelExtention.Contains(model.Extension))
      {
        if (!stringList.Contains(".xls"))
          stringList.Add(".xls");
      }
      else if (this.pptxExtention.Contains(model.Extension))
      {
        if (!stringList.Contains(".ppt"))
          stringList.Add(".ppt");
      }
      else if (this.imageExtention.Contains(model.Extension))
      {
        if (!stringList.Contains(".png"))
          stringList.Add(".png");
      }
      else if (this.rtfExtention.Contains(model.Extension))
      {
        if (!stringList.Contains(".rtf"))
          stringList.Add(".rtf");
      }
      else if (this.txtExtention.Contains(model.Extension))
      {
        if (!stringList.Contains(".txt"))
          stringList.Add(".txt");
      }
      else if (!stringList.Contains(".notsupport"))
        stringList.Add(".notsupport");
    }
    return stringList;
  }

  public List<string> Menu
  {
    get => this.menus;
    set => this.SetProperty<List<string>>(ref this.menus, value, nameof (Menu));
  }

  public RelayCommand<object> RemoveOne
  {
    get
    {
      return this.removeOne ?? (this.removeOne = new RelayCommand<object>((Action<object>) (args =>
      {
        OpenHistoryModel openHistoryModel = args as OpenHistoryModel;
        this.HistoryModels.Remove(openHistoryModel);
        if (this.selectedModels != null)
          this.selectedModels.Remove(openHistoryModel);
        HistoryManager.UpdateHistory(openHistoryModel.FilePath, true);
        this.SelectItemsPropertyChange();
      })));
    }
  }

  public RelayCommand<object> OpenFolder
  {
    get
    {
      return this.openFolder ?? (this.openFolder = new RelayCommand<object>((Action<object>) (args => Process.Start("explorer.exe", "/select," + (args as OpenHistoryModel).FilePath))));
    }
  }

  public RelayCommand ClearSelectModel
  {
    get
    {
      return this.clearSelectModel ?? (this.clearSelectModel = new RelayCommand((Action) (() =>
      {
        if (this.HistoryModels.Count == 0 || ModernMessageBox.Show(Resources.WinHistoryClearLabelClickTips, UtilManager.GetProductName(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
          return;
        this.ClearHistory();
      })));
    }
  }

  public RelayCommand<object> RunEditor
  {
    get
    {
      return this.runEditor ?? (this.runEditor = new RelayCommand<object>((Action<object>) (args =>
      {
        OpenHistoryModel openHistoryModel = args as OpenHistoryModel;
        GAManager.SendEvent("WelcomeWindow", "MenuOpenFileBtnClick", "Count", 1L);
        FileManager.OpenOneFile(openHistoryModel.FilePath);
      })));
    }
  }

  public RelayCommand OpenOneFileCMD
  {
    get
    {
      return this.openOneFileCMD ?? (this.openOneFileCMD = new RelayCommand((Action) (() =>
      {
        ConfigManager.SetWelcomeOpenBtnTipsFlag(true);
        this.OpenBtnTipsVisibility = Visibility.Collapsed;
        string file = FileManager.SelectFileForOpen();
        if (string.IsNullOrWhiteSpace(file))
          return;
        GAManager.SendEvent("WelcomeWindow", "OpenOneFileBtnClick", "Count", 1L);
        DocsPathUtils.WriteFilesPathJson("unknow", (object) file);
        FileManager.OpenOneFile(file);
      })));
    }
  }

  public RelayCommand CreateFileCMD
  {
    get
    {
      return this.createFileCMD ?? (this.createFileCMD = new RelayCommand((Action) (() =>
      {
        GAManager.SendEvent("WelcomeWindow", "NewFileBtnClick", "Count", 1L);
        CreateNewFileUtils.CreateBlankPDFByEditor();
      })));
    }
  }

  public RelayCommand FillFormCMD
  {
    get
    {
      return this.fillFormCMD ?? (this.fillFormCMD = new RelayCommand((Action) (() =>
      {
        string file = FileManager.SelectPDFFileForOpen();
        if (string.IsNullOrWhiteSpace(file))
          return;
        GAManager.SendEvent("WelcomeWindow", "FillFormBtnClick", "Count", 1L);
        DocsPathUtils.WriteFilesPathJson("unknow", (object) file);
        FileManager.OpenOneFile(file, "tab:fillform");
      })));
    }
  }

  public RelayCommand<object> CompressPDF
  {
    get
    {
      return this.compressPDF ?? (this.compressPDF = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvToPDFType.CompressPDF, args))));
    }
  }

  public RelayCommand<object> PdftoWord
  {
    get
    {
      return this.pdftoWord ?? (this.pdftoWord = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvFromPDFType.PDFToWord, args))));
    }
  }

  public RelayCommand<object> PdftoExcel
  {
    get
    {
      return this.pdftoExcel ?? (this.pdftoExcel = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvFromPDFType.PDFToExcel, args))));
    }
  }

  public RelayCommand<object> PdftoPng
  {
    get
    {
      return this.pdftoPng ?? (this.pdftoPng = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvFromPDFType.PDFToPng, args))));
    }
  }

  public RelayCommand<object> PdftoRtf
  {
    get
    {
      return this.pdftoRtf ?? (this.pdftoRtf = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvFromPDFType.PDFToRTF, args))));
    }
  }

  public RelayCommand<object> PdftoTxt
  {
    get
    {
      return this.pdftoTxt ?? (this.pdftoTxt = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvFromPDFType.PDFToTxt, args))));
    }
  }

  public RelayCommand<object> PdftoWeb
  {
    get
    {
      return this.pdftoWeb ?? (this.pdftoWeb = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvFromPDFType.PDFToHtml, args))));
    }
  }

  public RelayCommand<object> PdftoXML
  {
    get
    {
      return this.pdftoXML ?? (this.pdftoXML = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvFromPDFType.PDFToXml, args))));
    }
  }

  public RelayCommand<object> PdftoJPG
  {
    get
    {
      return this.pdftoJPG ?? (this.pdftoJPG = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvFromPDFType.PDFToJpg, args))));
    }
  }

  public RelayCommand<object> PdftoPPT
  {
    get
    {
      return this.pdftoPPT ?? (this.pdftoPPT = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvFromPDFType.PDFToPPT, args))));
    }
  }

  public RelayCommand<object> SplitPDF
  {
    get
    {
      return this.splitPDF ?? (this.splitPDF = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvToPDFType.SplitPDF, args))));
    }
  }

  public RelayCommand<object> MergePDF
  {
    get
    {
      return this.mergePDF ?? (this.mergePDF = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvToPDFType.MergePDF, args))));
    }
  }

  public RelayCommand<object> WordtoPDF
  {
    get
    {
      return this.wordtoPDF ?? (this.wordtoPDF = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvToPDFType.WordToPDF, args))));
    }
  }

  public RelayCommand<object> ConvtoPDF
  {
    get
    {
      return this.covtoPDF ?? (this.covtoPDF = new RelayCommand<object>((Action<object>) (args =>
      {
        ConvToPDFType convType = this.GetConvType((args as OpenHistoryModel).FilePath);
        switch (convType)
        {
          case ConvToPDFType.WordToPDF:
          case ConvToPDFType.ExcelToPDF:
          case ConvToPDFType.PPTToPDF:
          case ConvToPDFType.ImageToPDF:
          case ConvToPDFType.RtfToPDF:
          case ConvToPDFType.TxtToPDF:
            this.StartApp((object) convType, args);
            break;
        }
      })));
    }
  }

  public RelayCommand<object> ExceltoPDF
  {
    get
    {
      return this.exceltoPDF ?? (this.exceltoPDF = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvToPDFType.ExcelToPDF, args))));
    }
  }

  public RelayCommand<object> PPTtoPDF
  {
    get
    {
      return this.ppttoPDF ?? (this.ppttoPDF = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvToPDFType.PPTToPDF, args))));
    }
  }

  public RelayCommand<object> IMGtoPDF
  {
    get
    {
      return this.imgtoPDF ?? (this.imgtoPDF = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvToPDFType.ImageToPDF, args))));
    }
  }

  public RelayCommand<object> RTFtoPDF
  {
    get
    {
      return this.rtftoPDF ?? (this.rtftoPDF = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvToPDFType.RtfToPDF, args))));
    }
  }

  public RelayCommand<object> TXTtoPDF
  {
    get
    {
      return this.txttoPDF ?? (this.txttoPDF = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvToPDFType.TxtToPDF, args))));
    }
  }

  public RelayCommand<object> WebtoPDF
  {
    get
    {
      return this.webtoPDF ?? (this.webtoPDF = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvToPDFType.HtmlToPDF, args))));
    }
  }

  public RelayCommand<object> XMLtoPDF
  {
    get
    {
      return this.xmltoPDF ?? (this.xmltoPDF = new RelayCommand<object>((Action<object>) (args => this.StartApp((object) ConvToPDFType.XmlToPDf, args))));
    }
  }

  private void StartApp(object opType, object arg)
  {
    switch (opType)
    {
      case ConvFromPDFType convFromPdfType:
        GAManager.SendEvent("PDFConverterBtnClick", convFromPdfType.ToString(), "Welcome", 1L);
        if (!IAPUtils.IsPaidUserWrapper() && !ConfigManager.GetTest())
        {
          IAPUtils.ShowPurchaseWindows("Welcome", ".welcome");
          return;
        }
        break;
      case ConvToPDFType convToPdfType:
        GAManager.SendEvent("PDFConverterBtnClick", convToPdfType.ToString(), "Welcome", 1L);
        if ((convToPdfType == ConvToPDFType.MergePDF || convToPdfType == ConvToPDFType.CompressPDF || convToPdfType == ConvToPDFType.SplitPDF) && !IAPUtils.IsPaidUserWrapper() && !ConfigManager.GetTest())
        {
          IAPUtils.ShowPurchaseWindows("Welcome", ".welcome");
          return;
        }
        break;
    }
    OpenHistoryModel openHistoryModel = arg as OpenHistoryModel;
    List<string> stringList = new List<string>();
    if (arg != null)
      stringList.Add(openHistoryModel.FilePath);
    else if (this.selectedModels != null)
    {
      foreach (OpenHistoryModel selectedModel in this.selectedModels)
        stringList.Add(selectedModel.FilePath);
    }
    DocsPathUtils.WriteFilesPathJson("", (object) stringList);
    switch (opType)
    {
      case ConvToPDFType type1:
        CommomLib.Commom.AppManager.OpenPDFConverterToPdf(type1, stringList.ToArray());
        break;
      case ConvFromPDFType type2:
        CommomLib.Commom.AppManager.OpenPDFConverterFromPdf(type2, stringList.ToArray());
        break;
    }
    this.AllToolsSwitchIsChecked = false;
  }

  private ConvToPDFType GetConvType(string path)
  {
    string extension = new FileInfo(path).Extension;
    if (string.IsNullOrEmpty(extension))
      return ConvToPDFType.MergePDF;
    if (this.wordExtention.Contains(extension))
      return ConvToPDFType.WordToPDF;
    if (this.excelExtention.Contains(extension))
      return ConvToPDFType.ExcelToPDF;
    if (this.pptxExtention.Contains(extension))
      return ConvToPDFType.PPTToPDF;
    if (this.imageExtention.Contains(extension))
      return ConvToPDFType.ImageToPDF;
    if (this.rtfExtention.Contains(extension))
      return ConvToPDFType.RtfToPDF;
    return this.txtExtention.Contains(extension) ? ConvToPDFType.TxtToPDF : ConvToPDFType.MergePDF;
  }
}
