// Decompiled with JetBrains decompiler
// Type: PDFLauncher.RecoverWindow
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using PDFLauncher.CustomControl;
using PDFLauncher.Models;
using PDFLauncher.Utils;
using PDFLauncher.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace PDFLauncher;

public partial class RecoverWindow : Window, IComponentConnector, IStyleConnector
{
  internal ListView RecoverView;
  internal SaveFolderTextBox LocationTextBox;
  internal ButtonEx startBtn;
  internal ButtonEx discardBtn;
  private bool _contentLoaded;

  public RecoverWindow()
  {
    this.DataContext = (object) Ioc.Default.GetRequiredService<RecoverViewModel>();
    this.InitializeComponent();
  }

  public RecoverViewModel VM => this.DataContext as RecoverViewModel;

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent(nameof (RecoverWindow), "Show", "Count", 1L);
  }

  private void RecoverView_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
  }

  private async void StartBtn_Click(object sender, RoutedEventArgs e)
  {
    RecoverWindow recoverWindow = this;
    List<RecoverFileItem> all = recoverWindow.VM.RecoverFileList.ToList<RecoverFileItem>().FindAll((Predicate<RecoverFileItem>) (r => r.IsFileSelected.GetValueOrDefault()));
    string reoverOutputPath = recoverWindow.VM.ReoverOutputPath;
    if (!Directory.Exists(reoverOutputPath))
      Directory.CreateDirectory(reoverOutputPath);
    FileInfo[] files = new DirectoryInfo(RecoverViewModel.AutoSaveDir).GetFiles();
    GAManager.SendEvent(nameof (RecoverWindow), "Start", "Count", 1L);
    for (int index = 0; index < all.Count; ++index)
    {
      RecoverFileItem recoverItem = all[index];
      recoverItem.Status = RecoverStatus.Recovering;
      FileInfo fileInfo = ((IEnumerable<FileInfo>) files).ToList<FileInfo>().Find((Predicate<FileInfo>) (x => x.FullName.Equals(recoverItem.SourceFullFileName, StringComparison.OrdinalIgnoreCase)));
      if (fileInfo != null)
      {
        string withoutExtension = Path.GetFileNameWithoutExtension(recoverItem.FileName);
        string str = ".pdf";
        string path;
        do
        {
          string path2 = $"{withoutExtension} - {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}{str}";
          path = Path.Combine(reoverOutputPath, path2);
        }
        while (File.Exists(path));
        try
        {
          recoverItem.RecoverFullFileName = path;
          fileInfo.CopyTo(recoverItem.RecoverFullFileName);
          fileInfo.Delete();
        }
        catch (PathTooLongException ex)
        {
          recoverItem.RecoverFullFileName = Path.Combine(reoverOutputPath, Guid.NewGuid().ToString("N").ToLower() + ".pdf");
          fileInfo.CopyTo(recoverItem.RecoverFullFileName);
          fileInfo.Delete();
        }
        catch (DirectoryNotFoundException ex)
        {
          recoverItem.RecoverFullFileName = Path.Combine(reoverOutputPath, Guid.NewGuid().ToString("N").ToLower() + ".pdf");
          fileInfo.CopyTo(recoverItem.RecoverFullFileName);
          fileInfo.Delete();
        }
        catch (Exception ex)
        {
          GAManager.SendEvent("Exception", "Recover", $"{ex.GetType().Name}, {ex.Message}", 1L);
        }
        ConfigManager.DelAutoSaveFileAsync(recoverItem.FileGuid, new int?());
        recoverItem.Status = RecoverStatus.Converted;
        recoverItem.IsFileSelected = new bool?(false);
        recoverItem.RecoverDir = reoverOutputPath;
        recoverItem.DisplayName = recoverItem.RecoverFullFileName;
      }
      else
      {
        recoverItem.Status = RecoverStatus.Prepare;
        recoverItem.IsFileSelected = new bool?(false);
      }
    }
    if (all.Where<RecoverFileItem>((Func<RecoverFileItem, bool>) (x => x.Status == RecoverStatus.Converted)).Count<RecoverFileItem>() > 0)
    {
      int num = await ExplorerUtils.OpenFolderAsync(reoverOutputPath, all.Where<RecoverFileItem>((Func<RecoverFileItem, bool>) (x => x.Status == RecoverStatus.Converted)).Select<RecoverFileItem, string>((Func<RecoverFileItem, string>) (x => x.RecoverFullFileName)).ToArray<string>(), new CancellationToken()) ? 1 : 0;
    }
    if (recoverWindow.VM.RecoverFileList.Count<RecoverFileItem>((Func<RecoverFileItem, bool>) (x => x.Status == RecoverStatus.Converted)) != recoverWindow.VM.RecoverFileList.Count)
      return;
    recoverWindow.Close();
  }

  private void DiscardBtn_Click(object sender, RoutedEventArgs e)
  {
    Discardconfirm discardconfirm = new Discardconfirm();
    discardconfirm.ShowDialog();
    if (!discardconfirm.DialogResult.GetValueOrDefault())
      return;
    GAManager.SendEvent(nameof (RecoverWindow), "Discard", "Count", 1L);
    List<RecoverFileItem> all = this.VM.RecoverFileList.ToList<RecoverFileItem>().FindAll((Predicate<RecoverFileItem>) (r => r.IsFileSelected.GetValueOrDefault()));
    foreach (FileInfo file in new DirectoryInfo(RecoverViewModel.AutoSaveDir).GetFiles())
    {
      FileInfo f = file;
      RecoverFileItem recoverFileItem = all.Find((Predicate<RecoverFileItem>) (x => x.SourceFullFileName.Equals(f.FullName, StringComparison.OrdinalIgnoreCase)));
      if (recoverFileItem != null)
      {
        f.Delete();
        ConfigManager.DelAutoSaveFileAsync(recoverFileItem.FileGuid, new int?());
        this.VM.RecoverFileList.Remove(recoverFileItem);
      }
    }
    if (this.VM.RecoverFileList.Count<RecoverFileItem>((Func<RecoverFileItem, bool>) (x => x.Status == RecoverStatus.Converted)) != this.VM.RecoverFileList.Count)
      return;
    this.Close();
  }

  private void recoverFileItemCB_Checked(object sender, RoutedEventArgs e)
  {
    this.VM.NotifyAllRecoverFilesSelectedChanged();
  }

  private void recoverFileItemCB_Unchecked(object sender, RoutedEventArgs e)
  {
    this.VM.NotifyAllRecoverFilesSelectedChanged();
  }

  private void OpenFileBtn_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      if (!(sender is Button button))
        return;
      object tag = button.Tag;
      if (tag == null || string.IsNullOrEmpty(tag.ToString()))
        return;
      FileManager.OpenOneFile(Path.Combine(this.VM.ReoverOutputPath, tag.ToString()));
    }
    catch
    {
    }
  }

  private async void OpenDirBtn_Click(object sender, RoutedEventArgs e)
  {
    object tag = ((FrameworkElement) sender).Tag;
    if (tag == null || string.IsNullOrEmpty(tag.ToString()))
      return;
    int num = await ExplorerUtils.SelectItemInExplorerAsync(tag.ToString(), new CancellationToken()) ? 1 : 0;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/PDFLauncher;component/recoverwindow.xaml", UriKind.Relative));
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
        this.RecoverView = (ListView) target;
        this.RecoverView.SelectionChanged += new SelectionChangedEventHandler(this.RecoverView_SelectionChanged);
        break;
      case 6:
        this.LocationTextBox = (SaveFolderTextBox) target;
        break;
      case 7:
        this.startBtn = (ButtonEx) target;
        break;
      case 8:
        this.discardBtn = (ButtonEx) target;
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
      case 3:
        ((ToggleButton) target).Checked += new RoutedEventHandler(this.recoverFileItemCB_Checked);
        ((ToggleButton) target).Unchecked += new RoutedEventHandler(this.recoverFileItemCB_Unchecked);
        break;
      case 4:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OpenDirBtn_Click);
        break;
      case 5:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OpenFileBtn_Click);
        break;
    }
  }
}
