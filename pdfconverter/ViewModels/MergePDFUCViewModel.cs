// Decompiled with JetBrains decompiler
// Type: pdfconverter.ViewModels.MergePDFUCViewModel
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

#nullable enable
namespace pdfconverter.ViewModels;

public class MergePDFUCViewModel : ObservableObject
{
  private 
  #nullable disable
  MergeFileItemCollection _mergePDFList;
  private MergeFileItem _selectedMergeFileItem;
  private string _mergeOutputPath;
  private string _mergeOutputFilename;
  private string _mergeOutputFileFullName;
  private bool? _mergeViewFileInExplore;
  private WorkQueenState _UIStatus;
  private RelayCommand addOneFile;
  private RelayCommand clearFiles;
  private RelayCommand selectPath;
  private RelayCommand beginMerge;
  private RelayCommand updateItem;
  private RelayCommand<MergeFileItem> moveUpOneFile;
  private RelayCommand<MergeFileItem> moveDownFile;
  private RelayCommand<MergeFileItem> openInExplorer;
  private RelayCommand<MergeFileItem> openWithEditor;
  private RelayCommand<MergeFileItem> removeFromList;

  public MergePDFUCViewModel() => this.MergeInitializeEnv();

  private void MergeInitializeEnv()
  {
    this._mergePDFList = new MergeFileItemCollection();
    this._mergeViewFileInExplore = new bool?(ConfigManager.GetConvertViewFileInExplore());
    this._UIStatus = WorkQueenState.Init;
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
    if (string.IsNullOrEmpty(path))
      return;
    this.MergeOutputPath = path;
  }

  public MergeFileItemCollection MergePDFList => this._mergePDFList;

  public MergeFileItem SelectedMergeFileItem
  {
    get => this._selectedMergeFileItem;
    set
    {
      this.SetProperty<MergeFileItem>(ref this._selectedMergeFileItem, value, nameof (SelectedMergeFileItem));
    }
  }

  private bool IsMergeFileHasBeenAdded(string fileName)
  {
    return !string.IsNullOrWhiteSpace(fileName) && this.MergePDFList != null && this.MergePDFList.Where<MergeFileItem>((Func<MergeFileItem, bool>) (f => f.FilePath.Equals(fileName))).ToList<MergeFileItem>().Count<MergeFileItem>() > 0;
  }

  public int AddOneFileToMergeList(string fileName, string password = null)
  {
    try
    {
      if (string.IsNullOrWhiteSpace(fileName) || this.MergePDFList == null)
        return 2;
      bool? nullable1 = ConToPDFUtils.CheckAccess(fileName);
      bool flag1 = false;
      if (nullable1.GetValueOrDefault() == flag1 & nullable1.HasValue)
      {
        bool? nullable2 = new FileOccupation(fileName).ShowDialog();
        bool flag2 = false;
        if (nullable2.GetValueOrDefault() == flag2 & nullable2.HasValue)
          return 0;
      }
      if (password == null)
        password = "";
      if (this.MergePDFList.Where<MergeFileItem>((Func<MergeFileItem, bool>) (f => f.FilePath.Equals(fileName))).ToList<MergeFileItem>().Count > 0)
        password = this.MergePDFList.FirstOrDefault<MergeFileItem>((Func<MergeFileItem, bool>) (c => c.FilePath == fileName)).Passwrod;
      MergeFileItem mergeFileItem = new MergeFileItem(fileName);
      if (mergeFileItem != null)
      {
        if (UtilsManager.IsnotSupportFile(fileName, UtilManager.PDFExtentions))
        {
          mergeFileItem.IsFileSelected = new bool?(false);
          mergeFileItem.Status = MergeStatus.Unsupport;
          this.MergePDFList.Add(mergeFileItem);
        }
        else if (ConToPDFUtils.CheckPassword(fileName, ref password, Application.Current.MainWindow))
        {
          this.MergePDFList.Add(mergeFileItem);
          mergeFileItem.parseFile(password);
        }
        else
        {
          this.MergePDFList.Add(mergeFileItem);
          mergeFileItem.IsFileSelected = new bool?(false);
          mergeFileItem.Status = MergeStatus.LoadedFailed;
        }
        mergeFileItem.Passwrod = password;
        if (this.MergePDFList.Count == 1)
        {
          string str = Path.GetFileNameWithoutExtension(mergeFileItem.FilePath);
          if (string.IsNullOrWhiteSpace(str))
            str = "default";
          string validFileName = UtilsManager.getValidFileName(this.MergeOutputPath, str + "_merge");
          if (!string.IsNullOrWhiteSpace(validFileName))
            this.MergeOutputFilename = validFileName;
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

  public void RemoveOneMergeFileItem(MergeFileItem item)
  {
    try
    {
      if (item == null || this.MergePDFList == null)
        return;
      if (this.MergePDFList.Contains(item))
        this.MergePDFList.Remove(item);
      this.NotifyAllMergeFilesSelectedChanged();
    }
    catch
    {
    }
  }

  public void MoveUpOneMergeFileItem(MergeFileItem item)
  {
    try
    {
      if (item == null || this.MergePDFList == null)
        return;
      if (this.MergePDFList.Contains(item))
      {
        this.SelectedMergeFileItem = item;
        int oldIndex = this.MergePDFList.IndexOf(item);
        int newIndex = oldIndex - 1;
        if (oldIndex >= 0 && newIndex >= 0)
        {
          this.MergePDFList.Move(oldIndex, newIndex);
          this.OnPropertyChanged("MergePDFList");
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
        string[] strArray = ConvertManager.selectMultiPDFFiles();
        if (strArray == null || strArray.Length == 0)
          return;
        foreach (string fileName in strArray)
          this.AddOneFileToMergeList(fileName);
      })));
    }
  }

  public RelayCommand ClearFiles
  {
    get
    {
      return this.clearFiles ?? (this.clearFiles = new RelayCommand((Action) (() =>
      {
        if (this.MergePDFList.Count<MergeFileItem>((Func<MergeFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())) == 0)
        {
          int num = (int) ModernMessageBox.Show(Resources.MainWinOthersToPDFDeleteNoFile);
        }
        else
        {
          if (ModernMessageBox.Show(Resources.WinMergeSplitClearFileAskMsg, UtilManager.GetProductName(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
          this.ClearAllSelectedMergeFileItems();
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
        string str = ConvertManager.selectOutputFolder(this.MergeOutputPath);
        if (string.IsNullOrWhiteSpace(str))
          return;
        this.MergeOutputPath = str;
        ConfigManager.SetConvertPath(this.MergeOutputPath);
      })));
    }
  }

  public RelayCommand BeginMerge
  {
    get
    {
      return this.beginMerge ?? (this.beginMerge = new RelayCommand((Action) (() => this.DoMergePDFFiles())));
    }
  }

  public RelayCommand<MergeFileItem> OpenInExplorer
  {
    get
    {
      return this.openInExplorer ?? (this.openInExplorer = new RelayCommand<MergeFileItem>((Action<MergeFileItem>) (model =>
      {
        if (string.IsNullOrWhiteSpace(this.MergeOutputFileFullName) || !File.Exists(this.MergeOutputFileFullName))
          return;
        UtilsManager.OpenFileInExplore(this.MergeOutputFileFullName, true);
      })));
    }
  }

  public RelayCommand<MergeFileItem> OpenWithEditor
  {
    get
    {
      return this.openWithEditor ?? (this.openWithEditor = new RelayCommand<MergeFileItem>((Action<MergeFileItem>) (model =>
      {
        if (string.IsNullOrWhiteSpace(this.MergeOutputFileFullName) || !File.Exists(this.MergeOutputFileFullName))
          return;
        UtilsManager.OpenFile(this.MergeOutputFileFullName);
      })));
    }
  }

  public RelayCommand<MergeFileItem> RemoveFromList
  {
    get
    {
      return this.removeFromList ?? (this.removeFromList = new RelayCommand<MergeFileItem>((Action<MergeFileItem>) (model => this.RemoveOneMergeFileItem(model))));
    }
  }

  public RelayCommand<MergeFileItem> MoveUpFile
  {
    get
    {
      return this.moveUpOneFile ?? (this.moveUpOneFile = new RelayCommand<MergeFileItem>((Action<MergeFileItem>) (model => this.MoveUpOneMergeFileItem(model))));
    }
  }

  public RelayCommand<MergeFileItem> MoveDownFile
  {
    get
    {
      return this.moveDownFile ?? (this.moveDownFile = new RelayCommand<MergeFileItem>((Action<MergeFileItem>) (model => this.MoveDownOneMergeFileItem(model))));
    }
  }

  public void changeMergeFileItem(MergeFileItem dropedFileItem, MergeFileItem oriFileItem)
  {
    try
    {
      if (dropedFileItem == null || this.MergePDFList == null || oriFileItem == null)
        return;
      if (this.MergePDFList.Contains(dropedFileItem) && this.MergePDFList.Contains(oriFileItem))
      {
        this.SelectedMergeFileItem = oriFileItem;
        int oldIndex = this.MergePDFList.IndexOf(dropedFileItem);
        int newIndex = this.MergePDFList.IndexOf(oriFileItem);
        if (oldIndex >= 0 && newIndex < this.MergePDFList.Count)
        {
          this.MergePDFList.Move(oldIndex, newIndex);
          this.OnPropertyChanged("MergePDFList");
        }
      }
      this.NotifyAllMergeFilesSelectedChanged();
    }
    catch
    {
    }
  }

  public void MoveDownOneMergeFileItem(MergeFileItem item)
  {
    try
    {
      if (item == null || this.MergePDFList == null)
        return;
      if (this.MergePDFList.Contains(item))
      {
        this.SelectedMergeFileItem = item;
        int oldIndex = this.MergePDFList.IndexOf(item);
        int newIndex = oldIndex + 1;
        if (oldIndex >= 0 && newIndex < this.MergePDFList.Count)
        {
          this.MergePDFList.Move(oldIndex, newIndex);
          this.OnPropertyChanged("MergePDFList");
        }
      }
      this.NotifyAllMergeFilesSelectedChanged();
    }
    catch
    {
    }
  }

  public void ClearAllSelectedMergeFileItems()
  {
    try
    {
      if (this.MergePDFList == null)
        return;
      foreach (MergeFileItem mergeFileItem in this.MergePDFList.Where<MergeFileItem>((Func<MergeFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())).ToList<MergeFileItem>())
        this.MergePDFList.Remove(mergeFileItem);
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
      if (this.MergePDFList == null || this.MergePDFList.Count<MergeFileItem>() <= 0)
        return new bool?(false);
      int num1 = this.MergePDFList.Count<MergeFileItem>();
      int num2 = this.MergePDFList.Count<MergeFileItem>((Func<MergeFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault()));
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
    if (this.MergePDFList == null || this.MergePDFList.Count<MergeFileItem>() <= 0)
      return;
    foreach (MergeFileItem mergePdf in (Collection<MergeFileItem>) this.MergePDFList)
    {
      if (mergePdf.Status != MergeStatus.Unsupport)
      {
        bool? isFileSelected = mergePdf.IsFileSelected;
        bool flag = bSelectAll;
        if (!(isFileSelected.GetValueOrDefault() == flag & isFileSelected.HasValue) && mergePdf.Status != MergeStatus.LoadedFailed)
          mergePdf.IsFileSelected = new bool?(bSelectAll);
      }
    }
    this.NotifyAllMergeFilesSelectedChanged();
  }

  public void NotifyAllMergeFilesSelectedChanged()
  {
    this.OnPropertyChanged("IsAllMergeFilesSelected");
    this.OnPropertyChanged("MergePDFList");
    this.UIStatus = WorkQueenState.Init;
  }

  public string MergeOutputFilename
  {
    get => this._mergeOutputFilename;
    set
    {
      this.UIStatus = WorkQueenState.Init;
      this.SetProperty<string>(ref this._mergeOutputFilename, value, nameof (MergeOutputFilename));
    }
  }

  public string MergeOutputPath
  {
    get => this._mergeOutputPath;
    set
    {
      this.UIStatus = WorkQueenState.Init;
      this.SetProperty<string>(ref this._mergeOutputPath, value, nameof (MergeOutputPath));
    }
  }

  public string MergeOutputFileFullName
  {
    get => this._mergeOutputFileFullName;
    set
    {
      this.SetProperty<string>(ref this._mergeOutputFileFullName, value, nameof (MergeOutputFileFullName));
    }
  }

  public bool? MergeViewFileInExplore
  {
    get => this._mergeViewFileInExplore;
    set
    {
      this.SetProperty<bool?>(ref this._mergeViewFileInExplore, value, nameof (MergeViewFileInExplore));
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
      this.MergeOutputFileFullName = "";
    }
  }

  public async void DoMergePDFFiles()
  {
    GAManager.SendEvent("MergePDF", "BtnClick", "Count", 1L);
    if (!IAPHelper.IsPaidUser && !ConfigManager.GetTest())
      IAPHelper.ShowActivationWindow("MergePDF", ".convert");
    else if (this.MergePDFList.Where<MergeFileItem>((Func<MergeFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())).Count<MergeFileItem>() <= 1)
    {
      int num1 = (int) ModernMessageBox.Show(Resources.WinMergeSplitMergeFileCheckEmptyFileMsg, UtilManager.GetProductName());
    }
    else if (string.IsNullOrWhiteSpace(this.MergeOutputPath) || string.IsNullOrWhiteSpace(this.MergeOutputFilename))
    {
      int num2 = (int) ModernMessageBox.Show(Resources.WinMergeSplitMergeFileCheckEmptyFilenameMsg, UtilManager.GetProductName());
    }
    else
    {
      if (!Directory.Exists(this.MergeOutputPath))
        Directory.CreateDirectory(this.MergeOutputPath);
      string mergeOutputFilename = this.MergeOutputFilename;
      if (!this.MergeOutputFilename.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        mergeOutputFilename += ".pdf";
      string outputFile = Path.Combine(this.MergeOutputPath, mergeOutputFilename);
      if (File.Exists(outputFile))
      {
        int num3 = (int) ModernMessageBox.Show(Resources.WinMergeSplitMergeFileCheckExistFilenameMsg, UtilManager.GetProductName());
      }
      else
      {
        List<PdfiumPdfRange> flist = new List<PdfiumPdfRange>();
        foreach (MergeFileItem mergeFileItem in this.MergePDFList.Where<MergeFileItem>((Func<MergeFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())))
        {
          mergeFileItem.Status = MergeStatus.Merging;
          PdfiumPdfRange pdfiumPdfRange = new PdfiumPdfRange(mergeFileItem.FilePath, mergeFileItem.PageFrom - 1, mergeFileItem.PageTo - 1, mergeFileItem.Passwrod);
          flist.Add(pdfiumPdfRange);
        }
        await Task.Run(TaskExceptionHelper.ExceptionBoundary((Func<Task>) (async () =>
        {
          GAManager.SendEvent("MergePDF", "Begin", "Count", 1L);
          bool mergeRet = false;
          this.UIStatus = WorkQueenState.Working;
          try
          {
            CancellationToken cancellationToken;
            if (await PdfiumNetHelper.MergeAsync((IReadOnlyList<PdfiumPdfRange>) flist, outputFile, (IProgress<double>) null, cancellationToken))
            {
              if (File.Exists(outputFile))
              {
                this.MergeOutputFileFullName = outputFile;
                mergeRet = true;
              }
            }
          }
          catch
          {
          }
          foreach (MergeFileItem mergeFileItem in this.MergePDFList.Where<MergeFileItem>((Func<MergeFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())))
            mergeFileItem.Status = mergeRet ? MergeStatus.Succ : MergeStatus.Fail;
          this.UIStatus = mergeRet ? WorkQueenState.Succ : WorkQueenState.Fail;
          if (mergeRet && this.MergeViewFileInExplore.GetValueOrDefault())
            UtilsManager.OpenFileInExplore(this.MergeOutputFileFullName, true);
          if (mergeRet)
            return;
          GAManager.SendEvent("MergePDF", "Failed", "Count", 1L);
          if (MessageBox.Show(Resources.WinMergeSplitMergeFileFailMsg, UtilManager.GetProductName(), MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) != MessageBoxResult.OK)
            return;
          Application.Current.Dispatcher.Invoke((Action) (() =>
          {
            FeedbackWindow feedbackWindow = new FeedbackWindow();
            if (feedbackWindow == null)
              return;
            try
            {
              foreach (MergeFileItem mergePdf in (Collection<MergeFileItem>) this.MergePDFList)
              {
                if (mergePdf.IsFileSelected.GetValueOrDefault())
                  feedbackWindow.flist.Add(mergePdf.FilePath);
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
          }));
        }))).ConfigureAwait(false);
      }
    }
  }
}
