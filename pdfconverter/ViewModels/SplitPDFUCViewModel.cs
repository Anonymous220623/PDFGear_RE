// Decompiled with JetBrains decompiler
// Type: pdfconverter.ViewModels.SplitPDFUCViewModel
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

#nullable enable
namespace pdfconverter.ViewModels;

public class SplitPDFUCViewModel : ObservableObject
{
  private 
  #nullable disable
  SplitFileItemCollection _splitPDFList;
  private SplitFileItem _selectedSplitFileItem;
  private string _splitOutputPath;
  private bool? _splitViewFileInExplore;
  private SplitPDFUIStatus _splitUIStatus;
  private RelayCommand addOneFile;
  private RelayCommand clearFiles;
  private RelayCommand selectPath;
  private RelayCommand beginSplit;
  private RelayCommand<SplitFileItem> openInExplorer;
  private RelayCommand<SplitFileItem> removeFromList;
  private RelayCommand updateItem;

  public SplitPDFUCViewModel()
  {
    try
    {
      this.SplitInitializeEnv();
    }
    catch
    {
    }
  }

  public RelayCommand UpdateItem
  {
    get
    {
      return this.updateItem ?? (this.updateItem = new RelayCommand((Action) (() => this.NotifyAllSplitFilesSelectedChanged())));
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
          this.AddOneFileToSplitList(fileName);
      })));
    }
  }

  public RelayCommand ClearFiles
  {
    get
    {
      return this.clearFiles ?? (this.clearFiles = new RelayCommand((Action) (() =>
      {
        if (this.SplitPDFList.Count<SplitFileItem>((Func<SplitFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())) == 0)
        {
          int num = (int) ModernMessageBox.Show(Resources.MainWinOthersToPDFDeleteNoFile);
        }
        else
        {
          if (ModernMessageBox.Show(Resources.WinMergeSplitClearFileAskMsg, UtilManager.GetProductName(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
          this.ClearAllSelectedSplitFileItems();
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
        string str = ConvertManager.selectOutputFolder(this.SplitOutputPath);
        if (string.IsNullOrWhiteSpace(str))
          return;
        this.SplitOutputPath = str;
        ConfigManager.SetConvertPath(this.SplitOutputPath);
      })));
    }
  }

  public RelayCommand BeginSplit
  {
    get
    {
      return this.beginSplit ?? (this.beginSplit = new RelayCommand((Action) (() => this.DoSplitPDFFiles())));
    }
  }

  public RelayCommand<SplitFileItem> OpenInExplorer
  {
    get
    {
      return this.openInExplorer ?? (this.openInExplorer = new RelayCommand<SplitFileItem>((Action<SplitFileItem>) (model => UtilsManager.OpenFolderInExplore(model.OutputPath))));
    }
  }

  public RelayCommand<SplitFileItem> RemoveFromList
  {
    get
    {
      return this.removeFromList ?? (this.removeFromList = new RelayCommand<SplitFileItem>((Action<SplitFileItem>) (model => this.RemoveOneSplitFileItem(model))));
    }
  }

  private void SplitInitializeEnv()
  {
    this._splitPDFList = new SplitFileItemCollection();
    this._splitViewFileInExplore = new bool?(ConfigManager.GetConvertViewFileInExplore());
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
    this.SplitOutputPath = path;
  }

  public SplitFileItemCollection SplitPDFList => this._splitPDFList;

  public SplitFileItem SelectedSplitFileItem
  {
    get => this._selectedSplitFileItem;
    set
    {
      this.SetProperty<SplitFileItem>(ref this._selectedSplitFileItem, value, nameof (SelectedSplitFileItem));
    }
  }

  private bool IsSplitFileHasBeenAdded(string fileName)
  {
    return !string.IsNullOrWhiteSpace(fileName) && this.SplitPDFList != null && this.SplitPDFList.Where<SplitFileItem>((Func<SplitFileItem, bool>) (f => f.FilePath.Equals(fileName))).ToList<SplitFileItem>().Count<SplitFileItem>() > 0;
  }

  public int AddOneFileToSplitList(string fileName, string password = null)
  {
    try
    {
      if (string.IsNullOrWhiteSpace(fileName) || this.SplitPDFList == null)
        return 2;
      if (this.IsSplitFileHasBeenAdded(fileName))
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
      SplitFileItem splitFileItem = new SplitFileItem(fileName);
      if (splitFileItem != null)
      {
        if (UtilsManager.IsnotSupportFile(fileName, UtilManager.PDFExtentions))
        {
          splitFileItem.IsFileSelected = new bool?(false);
          splitFileItem.Status = SplitStatus.Unsupport;
          this.SplitPDFList.Add(splitFileItem);
        }
        else if (ConToPDFUtils.CheckPassword(fileName, ref password, Application.Current.MainWindow))
        {
          this.SplitPDFList.Add(splitFileItem);
          splitFileItem.parseFile(password);
        }
        else
        {
          splitFileItem.IsFileSelected = new bool?(false);
          splitFileItem.Status = SplitStatus.LoadedFailed;
          this.SplitPDFList.Add(splitFileItem);
        }
      }
      this.NotifyAllSplitFilesSelectedChanged();
      return 0;
    }
    catch
    {
    }
    return 1;
  }

  private void RemoveOneSplitFileItem(SplitFileItem item)
  {
    try
    {
      if (item == null || this.SplitPDFList == null)
        return;
      if (this.SplitPDFList.Contains(item))
        this.SplitPDFList.Remove(item);
      this.NotifyAllSplitFilesSelectedChanged();
    }
    catch (Exception ex)
    {
    }
  }

  private void ClearAllSelectedSplitFileItems()
  {
    try
    {
      if (this.SplitPDFList == null)
        return;
      foreach (SplitFileItem splitFileItem in this.SplitPDFList.Where<SplitFileItem>((Func<SplitFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())).ToList<SplitFileItem>())
        this.SplitPDFList.Remove(splitFileItem);
      this.NotifyAllSplitFilesSelectedChanged();
    }
    catch
    {
    }
  }

  public bool? IsAllSplitFilesSelected
  {
    get
    {
      if (this.SplitPDFList == null || this.SplitPDFList.Count<SplitFileItem>() <= 0)
        return new bool?(false);
      int num1 = this.SplitPDFList.Count<SplitFileItem>();
      int num2 = this.SplitPDFList.Count<SplitFileItem>((Func<SplitFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault()));
      if (num2 <= 0)
        return new bool?(false);
      return num1 == num2 ? new bool?(true) : new bool?();
    }
    set
    {
      bool? nullable = value;
      if (!nullable.HasValue)
        nullable = new bool?(false);
      this.SelectAllSplitFiles(nullable.Value);
    }
  }

  private void SelectAllSplitFiles(bool bSelectAll)
  {
    if (this.SplitPDFList == null || this.SplitPDFList.Count<SplitFileItem>() <= 0)
      return;
    foreach (SplitFileItem splitPdf in (Collection<SplitFileItem>) this.SplitPDFList)
    {
      if (splitPdf.Status != SplitStatus.Unsupport)
      {
        bool? isFileSelected = splitPdf.IsFileSelected;
        bool flag = bSelectAll;
        if (!(isFileSelected.GetValueOrDefault() == flag & isFileSelected.HasValue) && splitPdf.Status != SplitStatus.LoadedFailed)
          splitPdf.IsFileSelected = new bool?(bSelectAll);
      }
    }
    this.NotifyAllSplitFilesSelectedChanged();
  }

  public void NotifyAllSplitFilesSelectedChanged()
  {
    this.OnPropertyChanged("IsAllSplitFilesSelected");
  }

  public string SplitOutputPath
  {
    get => this._splitOutputPath;
    set => this.SetProperty<string>(ref this._splitOutputPath, value, nameof (SplitOutputPath));
  }

  public bool? SplitViewFileInExplore
  {
    get => this._splitViewFileInExplore;
    set
    {
      this.SetProperty<bool?>(ref this._splitViewFileInExplore, value, nameof (SplitViewFileInExplore));
      ConfigManager.SetConvertViewFileInExplore(value.Value);
    }
  }

  public SplitPDFUIStatus SplitUIStatus
  {
    get => this._splitUIStatus;
    set
    {
      this.SetProperty<SplitPDFUIStatus>(ref this._splitUIStatus, value, nameof (SplitUIStatus));
    }
  }

  public async void DoSplitPDFFiles()
  {
    SplitPDFUCViewModel splitPdfucViewModel = this;
    GAManager.SendEvent("SplitPDF", "BtnClick", "Count", 1L);
    if (!IAPHelper.IsPaidUser && !ConfigManager.GetTest())
      IAPHelper.ShowActivationWindow("SplitPDF", ".convert");
    else if (splitPdfucViewModel.SplitPDFList.Where<SplitFileItem>((Func<SplitFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())).Count<SplitFileItem>() <= 0)
    {
      int num1 = (int) ModernMessageBox.Show(Resources.WinMergeSplitSplitFileCheckEmptyFileMsg, UtilManager.GetProductName());
    }
    else if (string.IsNullOrWhiteSpace(splitPdfucViewModel.SplitOutputPath))
    {
      int num2 = (int) ModernMessageBox.Show(Resources.WinConvertSetOutputFolderText, UtilManager.GetProductName());
    }
    else
    {
      foreach (SplitFileItem splitFileItem in splitPdfucViewModel.SplitPDFList.Where<SplitFileItem>((Func<SplitFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())))
      {
        if (string.IsNullOrWhiteSpace(splitFileItem.PageSplitModeStr))
        {
          splitPdfucViewModel.SelectedSplitFileItem = splitFileItem;
          int num3 = (int) ModernMessageBox.Show(Resources.WinMergeSplitSplitFileCheckModeMsg, UtilManager.GetProductName());
          return;
        }
        bool flag = false;
        if (splitFileItem.PageSplitMode == 0)
        {
          if (!PageRangeHelper.TryParsePageRange2(splitFileItem.PageSplitModeStr, out int[][] _, out int _))
          {
            int num4 = (int) ModernMessageBox.Show(Resources.WinMergeSplitSplitFileCheckpageRangeMsg, UtilManager.GetProductName());
            return;
          }
        }
        else if (splitFileItem.PageSplitMode == 1)
        {
          if (new Regex("^\\d+$").IsMatch(splitFileItem.PageSplitModeStr))
          {
            int int32 = Convert.ToInt32(splitFileItem.PageSplitModeStr);
            if (int32 <= 0 || int32 >= splitFileItem.PageCount)
              flag = true;
          }
          else
            flag = true;
        }
        else
          flag = true;
        if (flag)
        {
          splitPdfucViewModel.SelectedSplitFileItem = splitFileItem;
          int num5 = (int) ModernMessageBox.Show(Resources.WinMergeSplitSplitFileCheckpageRangeMsg, UtilManager.GetProductName());
          return;
        }
      }
      if (!Directory.Exists(splitPdfucViewModel.SplitOutputPath))
        Directory.CreateDirectory(splitPdfucViewModel.SplitOutputPath);
      // ISSUE: reference to a compiler-generated method
      await Task.Run(TaskExceptionHelper.ExceptionBoundary(new Func<Task>(splitPdfucViewModel.\u003CDoSplitPDFFiles\u003Eb__51_1))).ConfigureAwait(false);
    }
  }
}
