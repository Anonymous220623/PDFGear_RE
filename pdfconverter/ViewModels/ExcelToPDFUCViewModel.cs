// Decompiled with JetBrains decompiler
// Type: pdfconverter.ViewModels.ExcelToPDFUCViewModel
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
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

public class ExcelToPDFUCViewModel : ObservableObject
{
  private 
  #nullable disable
  ToPDFFileItemCollection _toPDFItemList;
  private ToPDFFileItem _selectedItem;
  private string _OutputPath;
  private string _OutputFilename;
  private string _OutputFileFullName;
  private bool? _ViewFileInExplore = new bool?(ConfigManager.GetConvertViewFileInExplore());
  private bool fillAllColinOnePage = true;
  private WorkQueenState _UIStatus;
  private RelayCommand addOneFile;
  private RelayCommand clearFiles;
  private RelayCommand selectPath;
  private RelayCommand beginWorks;
  private RelayCommand updateItem;
  private RelayCommand<ToPDFFileItem> moveUpOneFile;
  private RelayCommand<ToPDFFileItem> moveDownFile;
  private RelayCommand<ToPDFFileItem> openInExplorer;
  private RelayCommand<ToPDFFileItem> openWithEditor;
  private RelayCommand<ToPDFFileItem> removeFromList;

  public ExcelToPDFUCViewModel() => this.InitializeEnv();

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
    if (!App.convertType.Equals((object) ConvToPDFType.ExcelToPDF) || App.selectedFile == null || App.selectedFile.Length == 0)
      return;
    foreach (string fileName in App.selectedFile)
      this.AddOneFileToFileList(fileName);
  }

  public ToPDFFileItemCollection FileList
  {
    get => this._toPDFItemList ?? (this._toPDFItemList = new ToPDFFileItemCollection());
  }

  public ToPDFFileItem SelectedItem
  {
    get => this._selectedItem;
    set => this.SetProperty<ToPDFFileItem>(ref this._selectedItem, value, nameof (SelectedItem));
  }

  public bool FillAllColinOnePage
  {
    get => this.fillAllColinOnePage;
    set
    {
      this.SetProperty<bool>(ref this.fillAllColinOnePage, value, nameof (FillAllColinOnePage));
    }
  }

  private bool IsFileHasBeenAdded(string fileName)
  {
    return !string.IsNullOrWhiteSpace(fileName) && this.FileList != null && this.FileList.Where<ToPDFFileItem>((Func<ToPDFFileItem, bool>) (f => f.FilePath.Equals(fileName))).ToList<ToPDFFileItem>().Count<ToPDFFileItem>() > 0;
  }

  public int AddOneFileToFileList(string fileName)
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
      ToPDFFileItem toPdfFileItem = new ToPDFFileItem(fileName, ConvToPDFType.ExcelToPDF);
      if (toPdfFileItem != null)
      {
        string password = "";
        if (UtilsManager.IsnotSupportFile(fileName, UtilManager.ExcelExtentions))
        {
          toPdfFileItem.Status = ToPDFItemStatus.Unsupport;
          toPdfFileItem.IsFileSelected = new bool?(false);
          toPdfFileItem.IsEnable = new bool?(false);
          this.FileList.Add(toPdfFileItem);
        }
        else if (ConToPDFUtils.CheckExcelPassword(fileName, out password, Application.Current.MainWindow))
        {
          toPdfFileItem.Password = password;
          this.FileList.Add(toPdfFileItem);
          toPdfFileItem.ParseFile();
        }
        else
        {
          toPdfFileItem.Status = ToPDFItemStatus.LoadedFailed;
          toPdfFileItem.IsFileSelected = new bool?(false);
          toPdfFileItem.IsEnable = new bool?(false);
          this.FileList.Add(toPdfFileItem);
        }
        if (this.FileList.Count == 1)
        {
          string fileName1 = Path.GetFileNameWithoutExtension(toPdfFileItem.FilePath);
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

  public void RemoveOneToPDFFileItem(ToPDFFileItem item)
  {
    try
    {
      if (item == null || this.FileList == null)
        return;
      if (this.FileList.Contains(item))
        this.FileList.Remove(item);
      this.NotifyAllMergeFilesSelectedChanged();
    }
    catch
    {
    }
  }

  public void MoveUpOneToPDFFileItem(ToPDFFileItem item)
  {
    try
    {
      if (item == null || this.FileList == null)
        return;
      if (this.FileList.Contains(item))
      {
        this.SelectedItem = item;
        int oldIndex = this.FileList.IndexOf(item);
        int newIndex = oldIndex - 1;
        if (oldIndex >= 0 && newIndex >= 0)
        {
          this.FileList.Move(oldIndex, newIndex);
          this.OnPropertyChanged("FileList");
        }
      }
      this.NotifyAllMergeFilesSelectedChanged();
    }
    catch
    {
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
        string[] strArray = ConvertManager.selectMultiFiles("Excel Work Sheet", UtilManager.ExcelExtention);
        if (strArray == null || strArray.Length == 0)
          return;
        foreach (string fileName in strArray)
          this.AddOneFileToFileList(fileName);
      })));
    }
  }

  public RelayCommand ClearFiles
  {
    get
    {
      return this.clearFiles ?? (this.clearFiles = new RelayCommand((Action) (() =>
      {
        if (this.FileList.Count<ToPDFFileItem>((Func<ToPDFFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())) == 0)
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

  public RelayCommand BeginWorks
  {
    get
    {
      return this.beginWorks ?? (this.beginWorks = new RelayCommand((Action) (() => this.ProcessingFiles())));
    }
  }

  public RelayCommand<ToPDFFileItem> OpenInExplorer
  {
    get
    {
      return this.openInExplorer ?? (this.openInExplorer = new RelayCommand<ToPDFFileItem>((Action<ToPDFFileItem>) (model =>
      {
        string pdfResult = UtilsManager.getPDFResult(this.OutputPath, model.FilePath);
        if (!File.Exists(pdfResult))
          return;
        UtilsManager.OpenFileInExplore(pdfResult, true);
      })));
    }
  }

  public RelayCommand<ToPDFFileItem> OpenWithEditor
  {
    get
    {
      return this.openWithEditor ?? (this.openWithEditor = new RelayCommand<ToPDFFileItem>((Action<ToPDFFileItem>) (model =>
      {
        string pdfResult = UtilsManager.getPDFResult(this.OutputPath, model.FilePath);
        if (!File.Exists(pdfResult))
          return;
        UtilsManager.OpenFile(pdfResult);
      })));
    }
  }

  public RelayCommand<ToPDFFileItem> RemoveFromList
  {
    get
    {
      return this.removeFromList ?? (this.removeFromList = new RelayCommand<ToPDFFileItem>((Action<ToPDFFileItem>) (model => this.RemoveOneToPDFFileItem(model))));
    }
  }

  public RelayCommand<ToPDFFileItem> MoveUpFile
  {
    get
    {
      return this.moveUpOneFile ?? (this.moveUpOneFile = new RelayCommand<ToPDFFileItem>((Action<ToPDFFileItem>) (model => this.MoveUpOneToPDFFileItem(model))));
    }
  }

  public RelayCommand<ToPDFFileItem> MoveDownFile
  {
    get
    {
      return this.moveDownFile ?? (this.moveDownFile = new RelayCommand<ToPDFFileItem>((Action<ToPDFFileItem>) (model => this.MoveDownOneToPDFFileItem(model))));
    }
  }

  public void MoveDownOneToPDFFileItem(ToPDFFileItem item)
  {
    try
    {
      if (item == null || this.FileList == null)
        return;
      if (this.FileList.Contains(item))
      {
        this.SelectedItem = item;
        int oldIndex = this.FileList.IndexOf(item);
        int newIndex = oldIndex + 1;
        if (oldIndex >= 0 && newIndex < this.FileList.Count)
        {
          this.FileList.Move(oldIndex, newIndex);
          this.OnPropertyChanged("FileList");
        }
      }
      this.NotifyAllMergeFilesSelectedChanged();
    }
    catch
    {
    }
  }

  public void ClearAllSelectedItems()
  {
    try
    {
      if (this.FileList == null)
        return;
      foreach (ToPDFFileItem toPdfFileItem in this.FileList.Where<ToPDFFileItem>((Func<ToPDFFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())).ToList<ToPDFFileItem>())
        this.FileList.Remove(toPdfFileItem);
      this.NotifyAllMergeFilesSelectedChanged();
    }
    catch
    {
    }
  }

  public bool? IsAllMergeFilesSelected
  {
    get
    {
      if (this.FileList == null || this.FileList.Count<ToPDFFileItem>() <= 0)
        return new bool?(false);
      int num1 = this.FileList.Count<ToPDFFileItem>();
      int num2 = this.FileList.Count<ToPDFFileItem>((Func<ToPDFFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault()));
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

  private void SelectAllMergeFiles(bool bSelectAll)
  {
    if (this.FileList == null || this.FileList.Count<ToPDFFileItem>() <= 0)
      return;
    foreach (ToPDFFileItem file in (Collection<ToPDFFileItem>) this.FileList)
    {
      if (file.Status != ToPDFItemStatus.Unsupport && file.Status != ToPDFItemStatus.LoadedFailed)
      {
        bool? isFileSelected = file.IsFileSelected;
        bool flag = bSelectAll;
        if (!(isFileSelected.GetValueOrDefault() == flag & isFileSelected.HasValue))
          file.IsFileSelected = new bool?(bSelectAll);
      }
    }
    this.NotifyAllMergeFilesSelectedChanged();
  }

  public void NotifyAllMergeFilesSelectedChanged()
  {
    this.OnPropertyChanged("IsAllMergeFilesSelected");
    this.OnPropertyChanged("FileList");
    this.UIStatus = WorkQueenState.Init;
  }

  public string OutputFilename
  {
    get => this._OutputFilename;
    set => this.SetProperty<string>(ref this._OutputFilename, value, nameof (OutputFilename));
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

  public string OutputFileFullName
  {
    get => this._OutputFileFullName;
    set
    {
      this.SetProperty<string>(ref this._OutputFileFullName, value, nameof (OutputFileFullName));
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

  public async void ProcessingFiles()
  {
    ExcelToPDFUCViewModel toPdfucViewModel = this;
    GAManager.SendEvent("ExcelToPDF", "BtnClick", "Count", 1L);
    if (toPdfucViewModel.FileList.Count<ToPDFFileItem>((Func<ToPDFFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())) <= 0)
    {
      int num1 = (int) ModernMessageBox.Show(Resources.WinConvertAddFileTipText, UtilManager.GetProductName());
    }
    else if (string.IsNullOrWhiteSpace(toPdfucViewModel.OutputPath))
    {
      int num2 = (int) ModernMessageBox.Show(Resources.WinConvertSetOutputFolderText, UtilManager.GetProductName());
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      await Task.Run(TaskExceptionHelper.ExceptionBoundary(new Func<Task>(toPdfucViewModel.\u003CProcessingFiles\u003Eb__74_1))).ConfigureAwait(false);
      toPdfucViewModel.SelectAllMergeFiles(false);
    }
  }
}
