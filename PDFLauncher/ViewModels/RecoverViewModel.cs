// Decompiled with JetBrains decompiler
// Type: PDFLauncher.ViewModels.RecoverViewModel
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using CommomLib.Commom;
using CommomLib.Config.ConfigModels;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using PDFLauncher.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

#nullable disable
namespace PDFLauncher.ViewModels;

public class RecoverViewModel : BindableBase
{
  public static string AutoSaveDir = Path.Combine(AppDataHelper.LocalFolder, "BackUp");
  private RecoverFileItem selectedRecoverFileItem;
  private string recoverOutputPath;
  private int selectedRecoveringCount;

  public ObservableCollection<RecoverFileItem> RecoverFileList { get; private set; }

  public RecoverFileItem SelectedRecoverFileItem
  {
    get => this.selectedRecoverFileItem;
    set
    {
      this.SetProperty<RecoverFileItem>(ref this.selectedRecoverFileItem, value, nameof (SelectedRecoverFileItem));
    }
  }

  public string ReoverOutputPath
  {
    get => this.recoverOutputPath;
    set => this.SetProperty<string>(ref this.recoverOutputPath, value, nameof (ReoverOutputPath));
  }

  public int SelectedRecoveringCount
  {
    get => this.selectedRecoveringCount;
    set
    {
      this.SetProperty<int>(ref this.selectedRecoveringCount, value, nameof (SelectedRecoveringCount));
    }
  }

  public bool? IsAllRecoverFileSelected
  {
    get
    {
      if (this.RecoverFileList == null || this.RecoverFileList.Count <= 0)
        return new bool?(false);
      int count = this.RecoverFileList.Count;
      int num = this.RecoverFileList.Count<RecoverFileItem>((Func<RecoverFileItem, bool>) (r => r.IsFileSelected.GetValueOrDefault()));
      if (num <= 0)
        return new bool?(false);
      return count == num ? new bool?(true) : new bool?();
    }
    set
    {
      bool? nullable = value;
      if (!nullable.HasValue)
        nullable = new bool?(false);
      this.SelectAllRecoverFiles(nullable.Value);
    }
  }

  public void SelectAllRecoverFiles(bool bSelectAll)
  {
    if (this.RecoverFileList == null || this.RecoverFileList.Count<RecoverFileItem>() <= 0)
      return;
    foreach (RecoverFileItem recoverFile in (Collection<RecoverFileItem>) this.RecoverFileList)
    {
      bool? isFileSelected = recoverFile.IsFileSelected;
      bool flag = bSelectAll;
      if (!(isFileSelected.GetValueOrDefault() == flag & isFileSelected.HasValue))
        recoverFile.IsFileSelected = new bool?(bSelectAll);
      if (recoverFile.Status == RecoverStatus.Converted)
        recoverFile.IsFileSelected = new bool?(false);
    }
    this.NotifyAllRecoverFilesSelectedChanged();
  }

  public void NotifyAllRecoverFilesSelectedChanged()
  {
    this.OnPropertyChanged("IsAllRecoverFileSelected");
    this.OnPropertyChanged("RecoverFileList");
  }

  public RecoverViewModel()
  {
    this.RecoverFileList = new ObservableCollection<RecoverFileItem>();
    this.RecoverFileList.CollectionChanged += new NotifyCollectionChangedEventHandler(this.RecoverFileList_CollectionChanged);
    this.ReoverOutputPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\PDFgear";
    this.LoadRecoverFiles();
  }

  private void RecoverFileList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.NewItems != null)
    {
      foreach (ObservableObject newItem in (IEnumerable) e.NewItems)
        newItem.PropertyChanged += new PropertyChangedEventHandler(this.RecoveFileItem_PropertyChanged);
    }
    if (e.OldItems != null)
    {
      foreach (ObservableObject oldItem in (IEnumerable) e.OldItems)
        oldItem.PropertyChanged -= new PropertyChangedEventHandler(this.RecoveFileItem_PropertyChanged);
    }
    this.SelectedRecoveringCount = this.RecoverFileList.ToList<RecoverFileItem>().Count<RecoverFileItem>((Func<RecoverFileItem, bool>) (r => r.IsFileSelected.GetValueOrDefault() && r.Status == RecoverStatus.Prepare));
  }

  private void RecoveFileItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (!(e.PropertyName == "IsFileSelected") && !(e.PropertyName == "Status"))
      return;
    this.SelectedRecoveringCount = this.RecoverFileList.ToList<RecoverFileItem>().Count<RecoverFileItem>((Func<RecoverFileItem, bool>) (r => r.IsFileSelected.GetValueOrDefault() && r.Status == RecoverStatus.Prepare));
  }

  private void LoadRecoverFiles()
  {
    this.RecoverFileList.Clear();
    if (!Directory.Exists(RecoverViewModel.AutoSaveDir))
      return;
    FileInfo[] files = new DirectoryInfo(RecoverViewModel.AutoSaveDir).GetFiles("*.data");
    if (files.Length == 0)
      return;
    foreach (FileInfo fileInfo in files)
    {
      int startIndex = fileInfo.Name.LastIndexOf("_") + 1;
      int length = fileInfo.Name.LastIndexOf(".") - startIndex;
      string fileguid = fileInfo.Name.Substring(startIndex, length);
      if (!this.GetAutoSaveFileByUnUsing(fileInfo.Name))
      {
        AutoSaveFileModel result = ConfigManager.GetAutoSaveFileModelAsync(new CancellationToken(), fileguid).GetAwaiter().GetResult();
        if (result != null)
          this.RecoverFileList.Add(new RecoverFileItem()
          {
            FileName = result.FileName,
            LastTime = result.LastSaveTime,
            Status = RecoverStatus.Prepare,
            FileGuid = result.Guid,
            IsFileSelected = new bool?(true),
            RecoverDir = "",
            SourceFullFileName = result.TempFileName,
            EditorSourceFullFileName = result.SoruceFileFullName,
            DisplayName = result.SoruceFileFullName
          });
      }
    }
  }

  public bool GetAutoSaveFileByUnUsing(string fileName)
  {
    List<AutoSaveFileModel> result = ConfigManager.GetAutoSaveFilesAsync(new CancellationToken()).GetAwaiter().GetResult();
    List<Process> pdfeditorProcesss = RecoverViewModel.GetPdfeditorProcesss();
    if (result != null && pdfeditorProcesss != null)
    {
      for (int index = 0; index < result.Count; ++index)
      {
        AutoSaveFileModel configFile = result[index];
        if (pdfeditorProcesss.Find((Predicate<Process>) (p => p.Id == configFile.CreatePid)) != null && fileName.Contains(configFile.Guid))
          return true;
      }
    }
    return false;
  }

  public static List<Process> GetPdfeditorProcesss()
  {
    return ((IEnumerable<Process>) Process.GetProcessesByName("pdfeditor")).ToList<Process>();
  }
}
