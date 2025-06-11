// Decompiled with JetBrains decompiler
// Type: pdfconverter.ViewModels.CompressPDFUCViewModel
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using CommomLib.IAP;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using pdfconverter.Models;
using pdfconverter.Properties;
using pdfconverter.Utils;
using pdfconverter.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

#nullable enable
namespace pdfconverter.ViewModels;

public class CompressPDFUCViewModel : ObservableObject
{
  private 
  #nullable disable
  RelayCommand updateItem;
  private RelayCommand addOneFile;
  private RelayCommand clearFiles;
  private RelayCommand selectPath;
  private RelayCommand setLowCompressMode;
  private RelayCommand setMidCompressMode;
  private RelayCommand setHighCompressMode;
  private RelayCommand beginWorks;
  private RelayCommand<CompressItem> openWithEditor;
  private RelayCommand<CompressItem> removeFromList;
  private RelayCommand<CompressItem> openInExplorer;
  private CompressItemCollection compressItemList;
  private CompressItem _selectedItem;
  private string _OutputPath;
  private string _OutputFilename;
  private string _OutputFileFullName;
  private bool? _ViewFileInExplore = new bool?(ConfigManager.GetConvertViewFileInExplore());
  private bool isSetAllLowCompress;
  private bool isSetAllMidCompress = true;
  private bool IsLock;
  private bool isSetAllHighCompress;
  private WorkQueenState _UIStatus;
  private CompressMode compressMode = CompressMode.Medium;

  public CompressPDFUCViewModel() => this.InitializeEnv();

  private void InitializeEnv()
  {
    string path = string.Empty;
    try
    {
      path = ConfigManager.GetConvertPath();
      if (!string.IsNullOrEmpty(path))
      {
        if (Directory.Exists(path))
          goto label_6;
      }
      path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\PDFgear";
    }
    catch
    {
    }
label_6:
    if (!string.IsNullOrEmpty(path))
      this.OutputPath = path;
    CompressMode? defaultMode = this.GetDefaultMode();
    if (!defaultMode.HasValue)
      return;
    if (defaultMode.Value == CompressMode.Low)
    {
      this.IsSetAllLowCompress = true;
      this.SetLowCompress.Execute((object) null);
    }
    if (defaultMode.Value == CompressMode.Medium)
    {
      this.IsSetAllMidCompress = true;
      this.SetMidCompress.Execute((object) null);
    }
    if (defaultMode.Value != CompressMode.High)
      return;
    this.IsSetAllHighCompress = true;
    this.SetHighCompress.Execute((object) null);
  }

  private CompressMode? GetDefaultMode()
  {
    try
    {
      string path = Path.Combine(AppDataHelper.LocalCacheFolder, "TmpSetting", "compress");
      if (File.Exists(path))
      {
        string str = File.ReadAllText(path);
        File.Delete(str);
        int result;
        if (int.TryParse(str, out result))
          return new CompressMode?((CompressMode) result);
      }
    }
    catch
    {
    }
    return new CompressMode?();
  }

  public bool IsSetAllLowCompress
  {
    get => this.isSetAllLowCompress;
    set
    {
      this.SetProperty<bool>(ref this.isSetAllLowCompress, value, nameof (IsSetAllLowCompress));
    }
  }

  public bool IsSetAllMidCompress
  {
    get => this.isSetAllMidCompress;
    set
    {
      this.SetProperty<bool>(ref this.isSetAllMidCompress, value, nameof (IsSetAllMidCompress));
    }
  }

  public bool IsSetAllHighCompress
  {
    get => this.isSetAllHighCompress;
    set
    {
      this.SetProperty<bool>(ref this.isSetAllHighCompress, value, nameof (IsSetAllHighCompress));
    }
  }

  public CompressMode CompressMode
  {
    get => this.compressMode;
    set => this.SetProperty<CompressMode>(ref this.compressMode, value, nameof (CompressMode));
  }

  public string OutputFilename
  {
    get => this._OutputFilename;
    set => this.SetProperty<string>(ref this._OutputFilename, value, nameof (OutputFilename));
  }

  public string OutputFileFullName
  {
    get => this._OutputFileFullName;
    set
    {
      this.SetProperty<string>(ref this._OutputFileFullName, value, nameof (OutputFileFullName));
    }
  }

  public string OutputPath
  {
    get => this._OutputPath;
    set
    {
      this.UIStatus = WorkQueenState.Init;
      this.SetProperty<string>(ref this._OutputPath, value, nameof (OutputPath));
    }
  }

  public WorkQueenState UIStatus
  {
    get => this._UIStatus;
    set
    {
      this.SetProperty<WorkQueenState>(ref this._UIStatus, value, nameof (UIStatus));
      if (this._UIStatus != WorkQueenState.Init)
        return;
      this.OutputFileFullName = "";
    }
  }

  public bool? ViewFileInExplore
  {
    get => this._ViewFileInExplore;
    set
    {
      this.SetProperty<bool?>(ref this._ViewFileInExplore, value, nameof (ViewFileInExplore));
      ConfigManager.SetConvertViewFileInExplore(value.Value);
    }
  }

  public bool? IsAllMergeFilesSelected
  {
    get
    {
      if (this.FileList == null || this.FileList.Count<CompressItem>() <= 0)
        return new bool?(false);
      int num1 = this.FileList.Count<CompressItem>();
      int num2 = this.FileList.Count<CompressItem>((Func<CompressItem, bool>) (f => f.IsFileSelected.GetValueOrDefault()));
      if (num2 <= 0)
        return new bool?(false);
      return num1 == num2 ? new bool?(true) : new bool?();
    }
    set
    {
      bool? nullable = value;
      if (!nullable.HasValue)
        nullable = new bool?(false);
      this.SelectAllMergeFiles(nullable.Value);
    }
  }

  public void SelectModeChanged()
  {
    if (this.IsLock)
      return;
    if (this.FileList.GroupBy<CompressItem, int>((Func<CompressItem, int>) (f => f.Compress_Mode)).Count<IGrouping<int, CompressItem>>() == 1)
    {
      switch (this.FileList.FirstOrDefault<CompressItem>().Compress_Mode)
      {
        case 0:
          this.IsSetAllHighCompress = true;
          this.IsSetAllMidCompress = false;
          this.IsSetAllLowCompress = false;
          break;
        case 1:
          this.IsSetAllHighCompress = false;
          this.IsSetAllMidCompress = true;
          this.IsSetAllLowCompress = false;
          break;
        case 2:
          this.IsSetAllHighCompress = false;
          this.IsSetAllMidCompress = false;
          this.IsSetAllLowCompress = true;
          break;
      }
    }
    else
    {
      this.IsSetAllHighCompress = false;
      this.IsSetAllMidCompress = false;
      this.IsSetAllLowCompress = false;
    }
  }

  public RelayCommand BeginWorks
  {
    get
    {
      return this.beginWorks ?? (this.beginWorks = new RelayCommand((Action) (() => this.ProcessingFiles())));
    }
  }

  public CompressItemCollection FileList
  {
    get => this.compressItemList ?? (this.compressItemList = new CompressItemCollection());
  }

  public RelayCommand<CompressItem> OpenInExplorer
  {
    get
    {
      return this.openInExplorer ?? (this.openInExplorer = new RelayCommand<CompressItem>((Action<CompressItem>) (model =>
      {
        string pdfResult = UtilsManager.getPDFResult(this.OutputPath, model.FilePath);
        if (!File.Exists(pdfResult))
          return;
        UtilsManager.OpenFileInExplore(pdfResult, true);
      })));
    }
  }

  public RelayCommand SetLowCompress
  {
    get
    {
      return this.setLowCompressMode ?? (this.setLowCompressMode = new RelayCommand((Action) (() =>
      {
        this.CompressMode = CompressMode.Low;
        this.NotifyCompressModeChanged();
      })));
    }
  }

  public RelayCommand SetMidCompress
  {
    get
    {
      return this.setMidCompressMode ?? (this.setMidCompressMode = new RelayCommand((Action) (() =>
      {
        this.CompressMode = CompressMode.Medium;
        this.NotifyCompressModeChanged();
      })));
    }
  }

  public RelayCommand SetHighCompress
  {
    get
    {
      return this.setHighCompressMode ?? (this.setHighCompressMode = new RelayCommand((Action) (() =>
      {
        this.CompressMode = CompressMode.High;
        this.NotifyCompressModeChanged();
      })));
    }
  }

  public CompressItem SelectedItem
  {
    get => this._selectedItem;
    set => this.SetProperty<CompressItem>(ref this._selectedItem, value, nameof (SelectedItem));
  }

  public RelayCommand SelectPath
  {
    get
    {
      return this.selectPath ?? (this.selectPath = new RelayCommand((Action) (() =>
      {
        string str = ConvertManager.selectOutputFolder(this.OutputPath);
        if (string.IsNullOrWhiteSpace(str))
          return;
        this.OutputPath = str;
        ConfigManager.SetConvertPath(this.OutputPath);
      })));
    }
  }

  public RelayCommand UpdateItem
  {
    get
    {
      return this.updateItem ?? (this.updateItem = new RelayCommand((Action) (() => this.NotifyAllMergeFilesSelectedChanged())));
    }
  }

  public RelayCommand AddOneFile
  {
    get
    {
      return this.addOneFile ?? (this.addOneFile = new RelayCommand((Action) (() =>
      {
        string[] strArray = ConvertManager.selectMultiFiles("Portable Document Format", ".pdf;*.pdf;");
        if (strArray == null || strArray.Length == 0)
          return;
        foreach (string fileName in strArray)
          this.AddOneFileToFileList(fileName);
      })));
    }
  }

  public RelayCommand<CompressItem> OpenWithEditor
  {
    get
    {
      return this.openWithEditor ?? (this.openWithEditor = new RelayCommand<CompressItem>((Action<CompressItem>) (model =>
      {
        string str = Path.Combine(this.OutputPath, model.FileName);
        if (string.IsNullOrWhiteSpace(str) || !File.Exists(str))
          return;
        UtilsManager.OpenFile(str);
      })));
    }
  }

  public RelayCommand<CompressItem> RemoveFromList
  {
    get
    {
      return this.removeFromList ?? (this.removeFromList = new RelayCommand<CompressItem>((Action<CompressItem>) (model => this.RemoveOneToPDFFileItem(model))));
    }
  }

  public RelayCommand ClearFiles
  {
    get
    {
      return this.clearFiles ?? (this.clearFiles = new RelayCommand((Action) (() =>
      {
        if (this.FileList.Count<CompressItem>((Func<CompressItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())) == 0)
        {
          int num = (int) ModernMessageBox.Show(Resources.MainWinOthersToPDFDeleteNoFile);
        }
        else
        {
          if (ModernMessageBox.Show(Resources.WinMergeSplitClearFileAskMsg, UtilManager.GetProductName(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
          this.ClearAllSelectedItems();
        }
      })));
    }
  }

  public int AddOneFileToFileList(string fileName, string password = null)
  {
    try
    {
      if (string.IsNullOrWhiteSpace(fileName) || this.FileList == null)
        return 2;
      if (this.IsFileHasBeenAdded(fileName))
      {
        int num = (int) ModernMessageBox.Show(Resources.WinMergeSplitAddFileCheckMsg.Replace("XXX", fileName), UtilManager.GetProductName());
        return 1;
      }
      bool? nullable = ConToPDFUtils.CheckAccess(fileName);
      bool flag1 = false;
      if (nullable.GetValueOrDefault() == flag1 & nullable.HasValue)
      {
        nullable = new FileOccupation(fileName).ShowDialog();
        bool flag2 = false;
        if (nullable.GetValueOrDefault() == flag2 & nullable.HasValue)
          return 0;
      }
      if (password == null)
        password = "";
      CompressItem compressItem = new CompressItem(fileName, this.CompressMode, this);
      compressItem.Status = ToPDFItemStatus.Loaded;
      if (compressItem != null)
      {
        if (UtilsManager.IsnotSupportFile(fileName, UtilManager.PDFExtentions))
        {
          compressItem.Status = ToPDFItemStatus.Unsupport;
          compressItem.IsFileSelected = new bool?(false);
          compressItem.IsEnable = new bool?(false);
          this.FileList.Add(compressItem);
        }
        else if (ConToPDFUtils.CheckPassword(fileName, ref password, Application.Current.MainWindow))
        {
          compressItem.Password = password;
          this.FileList.Add(compressItem);
        }
        else
        {
          this.FileList.Add(compressItem);
          compressItem.IsFileSelected = new bool?(false);
          compressItem.IsEnable = new bool?(false);
          compressItem.Status = ToPDFItemStatus.LoadedFailed;
        }
        this.SelectModeChanged();
        if (this.FileList.Count == 1)
        {
          string fileName1 = Path.GetFileNameWithoutExtension(compressItem.FilePath);
          if (string.IsNullOrWhiteSpace(fileName1))
            fileName1 = "default";
          string validFileName = UtilsManager.getValidFileName(this.OutputPath, fileName1);
          if (!string.IsNullOrWhiteSpace(validFileName))
            this.OutputFilename = validFileName;
        }
      }
      this.NotifyAllMergeFilesSelectedChanged();
      return 0;
    }
    catch
    {
    }
    return 1;
  }

  public void ClearAllSelectedItems()
  {
    try
    {
      if (this.FileList == null)
        return;
      foreach (CompressItem compressItem in this.FileList.Where<CompressItem>((Func<CompressItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())).ToList<CompressItem>())
        this.FileList.Remove(compressItem);
      this.CompressMode = CompressMode.Medium;
      this.NotifyAllMergeFilesSelectedChanged();
    }
    catch
    {
    }
  }

  private void SelectAllMergeFiles(bool bSelectAll)
  {
    if (this.FileList == null || this.FileList.Count<CompressItem>() <= 0)
      return;
    foreach (CompressItem file in (Collection<CompressItem>) this.FileList)
    {
      if (file.Status != ToPDFItemStatus.Unsupport)
      {
        bool? isFileSelected = file.IsFileSelected;
        bool flag = bSelectAll;
        if (!(isFileSelected.GetValueOrDefault() == flag & isFileSelected.HasValue) && file.Status != ToPDFItemStatus.LoadedFailed)
          file.IsFileSelected = new bool?(bSelectAll);
      }
    }
    this.NotifyAllMergeFilesSelectedChanged();
  }

  public void RemoveOneToPDFFileItem(CompressItem item)
  {
    try
    {
      if (item == null || this.FileList == null)
        return;
      if (this.FileList.Contains(item))
        this.FileList.Remove(item);
      this.SelectModeChanged();
      this.NotifyAllMergeFilesSelectedChanged();
    }
    catch
    {
    }
  }

  public void NotifyAllMergeFilesSelectedChanged()
  {
    this.OnPropertyChanged("IsAllMergeFilesSelected");
    this.OnPropertyChanged("FileList");
    if (this.UIStatus == WorkQueenState.Working)
      return;
    this.UIStatus = WorkQueenState.Init;
  }

  public void NotifyCompressModeChanged()
  {
    this.IsLock = true;
    foreach (CompressItem compressItem in this.FileList.Where<CompressItem>((Func<CompressItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())).ToList<CompressItem>())
      compressItem.Compress_Mode = (int) this.CompressMode;
    this.IsLock = false;
    if (this.UIStatus == WorkQueenState.Working)
      return;
    this.UIStatus = WorkQueenState.Init;
  }

  public async void ProcessingFiles()
  {
    CompressPDFUCViewModel compressPdfucViewModel = this;
    GAManager.SendEvent("CompressPDF", "BtnClick", "Count", 1L);
    if (!IAPHelper.IsPaidUser && !ConfigManager.GetTest())
      IAPHelper.ShowActivationWindow("Compress", "Compress");
    else if (compressPdfucViewModel.FileList.Count<CompressItem>((Func<CompressItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())) <= 0)
    {
      int num1 = (int) ModernMessageBox.Show(Resources.WinCompressAddFileTipText, UtilManager.GetProductName());
    }
    else if (string.IsNullOrWhiteSpace(compressPdfucViewModel.OutputPath))
    {
      int num2 = (int) ModernMessageBox.Show(Resources.WinConvertSetOutputFolderText, UtilManager.GetProductName());
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      await Task.Run(TaskExceptionHelper.ExceptionBoundary(new Func<Task>(compressPdfucViewModel.\u003CProcessingFiles\u003Eb__90_1))).ConfigureAwait(false);
      compressPdfucViewModel.SelectAllMergeFiles(false);
    }
  }

  private void CheckSucceedCount()
  {
    if (this.FileList.Count<CompressItem>((Func<CompressItem, bool>) (f => f.Status != ToPDFItemStatus.Succ)) != 0)
      return;
    if (this.ViewFileInExplore.GetValueOrDefault())
      UtilsManager.OpenFolderInExplore(this.OutputPath);
    this.SelectAllMergeFiles(false);
  }

  private bool IsFileHasBeenAdded(string fileName)
  {
    return !string.IsNullOrWhiteSpace(fileName) && this.FileList != null && this.FileList.Where<CompressItem>((Func<CompressItem, bool>) (f => f.FilePath.Equals(fileName))).ToList<CompressItem>().Count<CompressItem>() > 0;
  }

  public string GetFileSize(string FileName)
  {
    if (!File.Exists(FileName))
      return "";
    long length = new FileInfo(FileName).Length;
    double x = 1024.0;
    if ((double) length < x)
      return length.ToString() + "B";
    if ((double) length < Math.Pow(x, 2.0))
      return ((double) length / x).ToString("f2") + "K";
    if ((double) length < Math.Pow(x, 3.0))
      return ((double) length / Math.Pow(x, 2.0)).ToString("f2") + "M";
    return (double) length < Math.Pow(x, 4.0) ? ((double) length / Math.Pow(x, 3.0)).ToString("f2") + "G" : ((double) length / Math.Pow(x, 4.0)).ToString("f2") + "T";
  }
}
