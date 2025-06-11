// Decompiled with JetBrains decompiler
// Type: pdfconverter.ViewModels.ImageToPDFUCViewModel
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
using Syncfusion.Pdf.Graphics;
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

public class ImageToPDFUCViewModel : ObservableObject
{
  private 
  #nullable disable
  ToPDFFileItemCollection _toPDFItemList;
  private List<PageSizeItem> pageSizeItems = new List<PageSizeItem>();
  private List<PageMarginItem> contentMargins = new List<PageMarginItem>();
  private ToPDFFileItem _selectedItem;
  private string _OutputPath;
  private string _OutputFilename;
  private string _OutputFileFullName;
  private string _MergeFileName;
  private bool? _ViewFileInExplore = new bool?(ConfigManager.GetConvertViewFileInExplore());
  private bool? _OutputInOneFile = new bool?(true);
  private PageSizeItem pageSize;
  private PageMarginItem contentMargin;
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
  private RelayCommand openOneFileInExplorer;
  private RelayCommand openWithOneFileEditor;
  private RelayCommand<ToPDFFileItem> removeFromList;

  public ImageToPDFUCViewModel() => this.InitializeEnv();

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
    if (App.convertType.Equals((object) ConvToPDFType.ImageToPDF) && App.selectedFile != null && App.selectedFile.Length != 0)
    {
      foreach (string fileName in App.selectedFile)
        this.AddOneFileToFileList(fileName);
    }
    if (this.pageSizeItems.Count == 0)
    {
      this.pageSizeItems.Add(new PageSizeItem(Resources.MainWinImageToPDFPageSizeMatchSource, pdfconverter.Models.PDFPageSize.MatchSource));
      this.pageSizeItems.Add(new PageSizeItem(Resources.MainWinImageToPDFPageSizeA4Portrait, pdfconverter.Models.PDFPageSize.A4_Portrait));
      this.pageSizeItems.Add(new PageSizeItem(Resources.MainWinImageToPDFPageSizeA4Landscape, pdfconverter.Models.PDFPageSize.A4_Landscape));
      this.pageSizeItems.Add(new PageSizeItem(Resources.MainWinImageToPDFPageSizeA3Portrait, pdfconverter.Models.PDFPageSize.A3_Portrait));
      this.pageSizeItems.Add(new PageSizeItem(Resources.MainWinImageToPDFPageSizeA3Landscape, pdfconverter.Models.PDFPageSize.A3_Landscape));
    }
    if (this.contentMargins.Count != 0)
      return;
    this.contentMargins.Add(new PageMarginItem(Resources.MainWinImageToPDFPageMarginsNoMargin, pdfconverter.Models.ContentMargin.NoMargin));
    this.contentMargins.Add(new PageMarginItem(Resources.MainWinImageToPDFPageMarginsSmallMargin, pdfconverter.Models.ContentMargin.SmallMargin));
    this.contentMargins.Add(new PageMarginItem(Resources.MainWinImageToPDFPageMarginsBigMargin, pdfconverter.Models.ContentMargin.BigMargin));
  }

  public List<PageSizeItem> PageSizeItems => this.pageSizeItems;

  public List<PageMarginItem> PageMarginItems => this.contentMargins;

  public ToPDFFileItemCollection FileList
  {
    get => this._toPDFItemList ?? (this._toPDFItemList = new ToPDFFileItemCollection());
  }

  public ToPDFFileItem SelectedItem
  {
    get => this._selectedItem;
    set => this.SetProperty<ToPDFFileItem>(ref this._selectedItem, value, nameof (SelectedItem));
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
      ToPDFFileItem toPdfFileItem = new ToPDFFileItem(fileName, ConvToPDFType.ImageToPDF);
      if (toPdfFileItem != null)
      {
        if (UtilsManager.IsnotSupportFile(fileName, UtilManager.ImageExtentions))
        {
          toPdfFileItem.Status = ToPDFItemStatus.Unsupport;
          toPdfFileItem.IsEnable = new bool?(false);
          toPdfFileItem.IsFileSelected = new bool?(false);
        }
        this.FileList.Add(toPdfFileItem);
        toPdfFileItem.ParseFile();
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
        string[] strArray = ConvertManager.selectMultiFiles("Image Format", UtilManager.ImageExtention);
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
        string pdfFilePath = this.GetPDFFilePath(this.OutputPath, model.FilePath, this._OutputInOneFile.Value);
        if (!File.Exists(pdfFilePath))
          return;
        UtilsManager.OpenFileInExplore(pdfFilePath, true);
      })));
    }
  }

  public RelayCommand OpenOneFileInExplorer
  {
    get
    {
      return this.openOneFileInExplorer ?? (this.openOneFileInExplorer = new RelayCommand((Action) (() =>
      {
        string str = Path.Combine(this.OutputPath, this._MergeFileName);
        if (!File.Exists(str))
          return;
        UtilsManager.OpenFileInExplore(str, true);
      })));
    }
  }

  public RelayCommand<ToPDFFileItem> OpenWithEditor
  {
    get
    {
      return this.openWithEditor ?? (this.openWithEditor = new RelayCommand<ToPDFFileItem>((Action<ToPDFFileItem>) (model =>
      {
        string pdfFilePath = this.GetPDFFilePath(this.OutputPath, model.FilePath, this._OutputInOneFile.Value);
        if (!File.Exists(pdfFilePath))
          return;
        UtilsManager.OpenFile(pdfFilePath);
      })));
    }
  }

  public RelayCommand OpenWithOneFileEditor
  {
    get
    {
      return this.openWithOneFileEditor ?? (this.openWithOneFileEditor = new RelayCommand((Action) (() =>
      {
        string str = Path.Combine(this.OutputPath, this._MergeFileName);
        if (!File.Exists(str))
          return;
        UtilsManager.OpenFile(str);
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

  public bool? OurputInOneFile
  {
    get => this._OutputInOneFile;
    set
    {
      this.UIStatus = WorkQueenState.Init;
      if (value.GetValueOrDefault())
      {
        foreach (ToPDFFileItem file in (Collection<ToPDFFileItem>) this.FileList)
          file.Status = ToPDFItemStatus.Loaded;
      }
      this.SetProperty<bool?>(ref this._OutputInOneFile, value, nameof (OurputInOneFile));
    }
  }

  public PageSizeItem PDFPageSize
  {
    get => this.pageSize;
    set => this.SetProperty<PageSizeItem>(ref this.pageSize, value, nameof (PDFPageSize));
  }

  public PageMarginItem ContentMargin
  {
    get => this.contentMargin;
    set => this.SetProperty<PageMarginItem>(ref this.contentMargin, value, nameof (ContentMargin));
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
    GAManager.SendEvent("ImageToPDF", "BtnClick", "Count", 1L);
    if (this.FileList.Where<ToPDFFileItem>((Func<ToPDFFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())).Count<ToPDFFileItem>() <= 0)
    {
      int num1 = (int) ModernMessageBox.Show(Resources.WinConvertAddFileTipText, UtilManager.GetProductName());
    }
    else if (string.IsNullOrWhiteSpace(this.OutputPath))
    {
      int num2 = (int) ModernMessageBox.Show(Resources.WinConvertSetOutputFolderText, UtilManager.GetProductName());
    }
    else
    {
      UtilsManager.getPDFFileName(this.OutputPath, this.FileList.Where<ToPDFFileItem>((Func<ToPDFFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())).First<ToPDFFileItem>().FilePath);
      List<ToPDFFileItem> flist = this.FileList.Where<ToPDFFileItem>((Func<ToPDFFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())).ToList<ToPDFFileItem>();
      foreach (ToPDFFileItem toPdfFileItem in flist)
        toPdfFileItem.Status = ToPDFItemStatus.Working;
      bool mergeRet = false;
      await Task.Run(TaskExceptionHelper.ExceptionBoundary((Func<Task>) (async () =>
      {
        GAManager.SendEvent("ImageToPDF", "Begin", "Count", 1L);
        this.UIStatus = WorkQueenState.Working;
        try
        {
          int selectCount = flist.Count;
          int sucessCount = 0;
          ParaConvert.GetPdfPagesize(this.PDFPageSize);
          int pdfeOrientation = (int) ParaConvert.GetPdfeOrientation(this.PDFPageSize);
          PdfMargins margins = ParaConvert.GetMargins(this.ContentMargin);
          if (!this.OurputInOneFile.Value)
          {
            List<Task> taskList = new List<Task>();
            string outputFolder = this.GetValidOutFolder(this.OutputPath, flist.FirstOrDefault<ToPDFFileItem>().FilePath);
            foreach (ToPDFFileItem toPdfFileItem in flist)
              toPdfFileItem.OutputPath = this.GetValidOutFileName(flist, outputFolder, toPdfFileItem);
            this.OutputFilename = outputFolder;
            if (!Directory.Exists(outputFolder))
              Directory.CreateDirectory(outputFolder);
            foreach (ToPDFFileItem toPdfFileItem in flist)
            {
              ToPDFFileItem file = toPdfFileItem;
              Task task = Task.Run(TaskExceptionHelper.ExceptionBoundary((Func<Task>) (async () =>
              {
                if (await ConToPDFUtils.ImageToMultiPDFByRangeAsync(file, this.PDFPageSize, margins, (IProgress<double>) null, token))
                {
                  sucessCount++;
                  file.Status = ToPDFItemStatus.Succ;
                }
                else
                  file.Status = ToPDFItemStatus.Fail;
              })));
              taskList.Add(task);
            }
            await Task.WhenAll((IEnumerable<Task>) taskList).ConfigureAwait(true);
            if (sucessCount == selectCount && Directory.Exists(outputFolder))
            {
              this.OutputFileFullName = outputFolder;
              mergeRet = true;
            }
            outputFolder = (string) null;
          }
          else
          {
            this._MergeFileName = this.GetMergeFileName(this.OutputPath, flist.First<ToPDFFileItem>().FilePath);
            CancellationToken token;
            if (await ConToPDFUtils.ImageToSiglePDFByRangeAsync(flist, this._MergeFileName, this.PDFPageSize, margins, (IProgress<double>) null, token) && File.Exists(this._MergeFileName))
            {
              this.OutputFileFullName = this._MergeFileName;
              mergeRet = true;
            }
          }
        }
        catch
        {
        }
        if (this.OurputInOneFile.Value)
        {
          foreach (ToPDFFileItem toPdfFileItem in this.FileList.Where<ToPDFFileItem>((Func<ToPDFFileItem, bool>) (f => f.IsFileSelected.GetValueOrDefault())))
            toPdfFileItem.Status = mergeRet ? ToPDFItemStatus.Succ : ToPDFItemStatus.Fail;
        }
        if (mergeRet && this.ViewFileInExplore.GetValueOrDefault() && !this.OurputInOneFile.Value)
          UtilsManager.OpenFolderInExplore(this.OutputFileFullName);
        else if (mergeRet && this.ViewFileInExplore.GetValueOrDefault() && this.OurputInOneFile.Value)
          UtilsManager.OpenFileInExplore(this.OutputFileFullName, true);
        if (mergeRet)
          return;
        GAManager.SendEvent("ImageToPDF", "Failed", "Count", 1L);
        if (MessageBox.Show(Resources.FileConvertMsgConvertFailSupport, UtilManager.GetProductName(), MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) != MessageBoxResult.OK)
          return;
        Application.Current.Dispatcher.Invoke((Action) (() =>
        {
          FeedbackWindow feedbackWindow = new FeedbackWindow();
          if (feedbackWindow == null)
            return;
          try
          {
            foreach (ToPDFFileItem file in (Collection<ToPDFFileItem>) this.FileList)
            {
              if (file.IsFileSelected.GetValueOrDefault())
                feedbackWindow.flist.Add(file.FilePath);
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
      this.SelectAllMergeFiles(false);
      this.UIStatus = mergeRet ? WorkQueenState.Succ : WorkQueenState.Fail;
    }
  }

  private string GetValidOutFileName(
    List<ToPDFFileItem> list,
    string parentFolder,
    ToPDFFileItem item)
  {
    if (!string.IsNullOrWhiteSpace(item.FilePath))
    {
      string withoutExtension = Path.GetFileNameWithoutExtension(item.FilePath);
      string str = withoutExtension;
      for (int index = 2; index < 100; ++index)
      {
        string fname_full = $"{parentFolder}\\{str}.pdf";
        if (list.Count<ToPDFFileItem>((Func<ToPDFFileItem, bool>) (f => f.OutputPath == fname_full)) == 0)
          return fname_full;
        str = withoutExtension + $"_{index}";
      }
    }
    return "";
  }

  private string GetMergeFileName(string outFolder, string source)
  {
    string withoutExtension = Path.GetFileNameWithoutExtension(source);
    int num = 0;
    string path = Path.Combine(outFolder, withoutExtension + ".pdf");
    while (File.Exists(path))
    {
      path = Path.Combine(outFolder, $"{withoutExtension}_{num}.pdf");
      ++num;
    }
    return path;
  }

  private string GetValidOutFolder(string root, string firstFileName)
  {
    string path2 = Path.GetFileNameWithoutExtension(firstFileName).Trim();
    if (string.IsNullOrWhiteSpace(path2))
      path2 = "PDF Files";
    string path = Path.Combine(root, path2);
    int num = 1;
    while (Directory.Exists(path))
    {
      path = Path.Combine(root, $"{path2} {num.ToString()}");
      ++num;
    }
    return path;
  }

  private string GetPDFFilePath(string outFolder, string FileName, bool IsOutputInOneFile)
  {
    return !IsOutputInOneFile ? Path.Combine(outFolder, this.OutputFilename, Path.GetFileNameWithoutExtension(FileName) + ".pdf") : Path.Combine(outFolder, this._OutputFileFullName);
  }
}
