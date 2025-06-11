// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Commets.AllPageCommetCollectionView
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf.Net;
using pdfeditor.Controls;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Models.Commets;

public class AllPageCommetCollectionView : ObservableCollection<PageCommetCollection>, IDisposable
{
  private bool disposedValue;
  private bool isLoading;
  private bool isCompleted;
  private bool isCompletedInternal;
  private int loadedPageInternal = -1;
  private object notifyChangedLocker = new object();
  private CancellationTokenSource cts;

  public AllPageCommetCollectionView(PdfDocument document)
  {
    this.Document = document ?? throw new ArgumentNullException(nameof (document));
    this.cts = new CancellationTokenSource();
    this.userList = new ObservableCollection<AnnotationsFilterModel>();
    this.AnnotationList = new ObservableCollection<AnnotationsFilterModel>();
    this.commetModels = new ObservableCollection<PageCommetCollection>();
  }

  public PdfDocument Document { get; }

  public ObservableCollection<AnnotationsFilterModel> userList { get; private set; }

  public ObservableCollection<AnnotationsFilterModel> AnnotationList { get; private set; }

  public ObservableCollection<PageCommetCollection> commetModels { get; private set; }

  public bool IsLoading
  {
    get => this.isLoading;
    private set
    {
      if (this.isLoading == value)
        return;
      this.isLoading = value;
      this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (IsLoading)));
    }
  }

  public bool IsCompleted
  {
    get => this.isCompleted;
    private set
    {
      if (this.isCompleted == value)
        return;
      this.isCompleted = value;
      this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (IsCompleted)));
    }
  }

  public double Progress
  {
    get
    {
      return !this.isCompletedInternal ? (double) (this.loadedPageInternal + 1) * 1.0 / (double) this.Document.Pages.Count : 1.0;
    }
  }

  public async void StartLoad()
  {
    AllPageCommetCollectionView commetCollectionView = this;
    if (commetCollectionView.isCompletedInternal || commetCollectionView.IsLoading)
      return;
    commetCollectionView.IsLoading = true;
    try
    {
      for (int i = 0; i < commetCollectionView.Document.Pages.Count; ++i)
      {
        if (i != 0 && i % 3 == 0)
        {
          await Task.Delay(10, commetCollectionView.cts.Token);
          commetCollectionView.cts.Token.ThrowIfCancellationRequested();
        }
        PageCommetCollection commetCollection = PageCommetCollection.Create(commetCollectionView.Document, i);
        if (commetCollection != null && commetCollection.Count > 0)
        {
          // ISSUE: explicit non-virtual call
          __nonvirtual (commetCollectionView.Add(commetCollection));
          commetCollectionView.commetModels.Add(commetCollection);
        }
        commetCollectionView.loadedPageInternal = i;
        commetCollectionView.OnPropertyChanged(new PropertyChangedEventArgs("Progress"));
        commetCollectionView.cts.Token.ThrowIfCancellationRequested();
      }
      commetCollectionView.isCompletedInternal = true;
      lock (commetCollectionView.notifyChangedLocker)
      {
        commetCollectionView.IsCompleted = true;
        commetCollectionView.IsLoading = false;
      }
      foreach (PageCommetCollection commetModel1 in (Collection<PageCommetCollection>) commetCollectionView.commetModels)
      {
        foreach (CommetModel commetModel2 in commetModel1)
        {
          AnnotationsFilterModel annotationsFilterModel1 = new AnnotationsFilterModel()
          {
            Text = commetModel2.Text
          };
          AnnotationsFilterModel annotationsFilterModel2 = new AnnotationsFilterModel()
          {
            Text = commetModel2.Title
          };
          if (commetCollectionView.userList.Count < 1)
          {
            commetCollectionView.userList.Add(annotationsFilterModel1);
          }
          else
          {
            bool flag = false;
            for (int index = 0; index < commetCollectionView.userList.Count; ++index)
            {
              if (commetCollectionView.userList[index].Text == commetModel2.Text)
              {
                ++commetCollectionView.userList[index].Count;
                flag = true;
              }
            }
            if (!flag)
              commetCollectionView.userList.Add(annotationsFilterModel1);
          }
          if (commetCollectionView.AnnotationList.Count < 1)
          {
            commetCollectionView.AnnotationList.Add(annotationsFilterModel2);
          }
          else
          {
            bool flag = false;
            for (int index = 0; index < commetCollectionView.AnnotationList.Count; ++index)
            {
              if (commetCollectionView.AnnotationList[index].Text == commetModel2.Title)
              {
                ++commetCollectionView.AnnotationList[index].Count;
                flag = true;
              }
            }
            if (!flag)
              commetCollectionView.AnnotationList.Add(annotationsFilterModel2);
          }
        }
      }
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      requiredService.AllCount = 0;
      foreach (PageCommetCollection commetCollection in (Collection<PageCommetCollection>) commetCollectionView)
        requiredService.AllCount += commetCollection.Count;
      requiredService.IsUserFilterAllChecked = new bool?(true);
      requiredService.IsKindFilterAllChecked = new bool?(true);
    }
    catch (OperationCanceledException ex)
    {
    }
  }

  public void FilterShowItems()
  {
    this.Clear();
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    requiredService.IsSelectedAll = new bool?(false);
    for (int index1 = 0; index1 < this.commetModels.Count; ++index1)
    {
      List<CommetModel> models = new List<CommetModel>();
      foreach (CommetModel commetModel in this.commetModels[index1])
      {
        if (this.userList != null && this.AnnotationList != null)
        {
          foreach (AnnotationsFilterModel user in (Collection<AnnotationsFilterModel>) this.userList)
          {
            foreach (AnnotationsFilterModel annotation in (Collection<AnnotationsFilterModel>) this.AnnotationList)
            {
              if (user.IsSelect && user.Text == commetModel.Text && annotation.IsSelect && annotation.Text == commetModel.Title)
                models.Add(commetModel.Clone());
            }
          }
        }
      }
      PageCommetCollection commetCollection = new PageCommetCollection(this.commetModels[index1].Document, this.commetModels[index1].PageIndex, models);
      for (int index2 = 0; index2 < models.Count; ++index2)
        models[index2].Parent = (ITreeViewNode) commetCollection;
      if (commetCollection != null && commetCollection.Count > 0)
        this.Add(commetCollection);
    }
    requiredService.AllCount = 0;
    foreach (PageCommetCollection commetCollection in (Collection<PageCommetCollection>) this)
      requiredService.AllCount += commetCollection.Count;
  }

  public void NotifyPageAnnotationChanged(int pageIndex)
  {
    this.NotifyPageAnnotationChanged(pageIndex, true);
  }

  public void NotifyDeletePageAnnotationChanged()
  {
    if (this.Document == null)
      return;
    for (int pageIndex = 0; pageIndex <= this.loadedPageInternal; pageIndex++)
    {
      lock (this.notifyChangedLocker)
      {
        try
        {
          this.IsLoading = true;
          this.IsCompleted = false;
          this.FirstOrDefault<PageCommetCollection>((Func<PageCommetCollection, bool>) (c => c.PageIndex == pageIndex));
          PageCommetCollection commetCollection1 = this.commetModels.FirstOrDefault<PageCommetCollection>((Func<PageCommetCollection, bool>) (c => c.PageIndex == pageIndex));
          PageCommetCollection commetCollection2 = PageCommetCollection.Create(this.Document, pageIndex);
          if (commetCollection1 == null)
          {
            if (commetCollection2 != null)
            {
              int index = 0;
              while (index < this.commetModels.Count && this.commetModels[index].PageIndex <= pageIndex)
                ++index;
              this.commetModels.Insert(index, commetCollection2);
            }
          }
          else
          {
            int index = this.commetModels.IndexOf(commetCollection1);
            if (commetCollection2 != null)
              this.commetModels[index] = commetCollection2;
            else if (index >= 0)
              this.commetModels.RemoveAt(index);
          }
        }
        finally
        {
          this.IsLoading = !this.isCompletedInternal;
          this.IsCompleted = this.isCompletedInternal;
        }
      }
    }
    this.ReflashUserList();
  }

  private void NotifyPageAnnotationChanged(int pageIndex, bool requeue)
  {
    if (pageIndex < 0 || this.Document == null)
      return;
    if (pageIndex <= this.loadedPageInternal)
    {
      lock (this.notifyChangedLocker)
      {
        try
        {
          this.IsLoading = true;
          this.IsCompleted = false;
          this.FirstOrDefault<PageCommetCollection>((Func<PageCommetCollection, bool>) (c => c.PageIndex == pageIndex));
          PageCommetCollection commetCollection1 = this.commetModels.FirstOrDefault<PageCommetCollection>((Func<PageCommetCollection, bool>) (c => c.PageIndex == pageIndex));
          PageCommetCollection commetCollection2 = PageCommetCollection.Create(this.Document, pageIndex);
          if (commetCollection1 == null)
          {
            if (commetCollection2 != null)
            {
              int index = 0;
              while (index < this.commetModels.Count && this.commetModels[index].PageIndex <= pageIndex)
                ++index;
              this.commetModels.Insert(index, commetCollection2);
            }
          }
          else
          {
            int index = this.commetModels.IndexOf(commetCollection1);
            if (commetCollection2 != null)
              this.commetModels[index] = commetCollection2;
            else if (index >= 0)
              this.commetModels.RemoveAt(index);
          }
        }
        finally
        {
          this.IsLoading = !this.isCompletedInternal;
          this.IsCompleted = this.isCompletedInternal;
        }
      }
    }
    else if (((pageIndex != this.loadedPageInternal + 1 ? 0 : (this.IsLoading ? 1 : 0)) & (requeue ? 1 : 0)) != 0)
      DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() => this.NotifyPageAnnotationChanged(pageIndex, false)));
    this.ReflashUserList();
  }

  private void ReflashUserList()
  {
    try
    {
      ObservableCollection<AnnotationsFilterModel> observableCollection1 = new ObservableCollection<AnnotationsFilterModel>();
      foreach (AnnotationsFilterModel user in (Collection<AnnotationsFilterModel>) this.userList)
      {
        user.Count = 0;
        observableCollection1.Add(user);
      }
      ObservableCollection<AnnotationsFilterModel> observableCollection2 = new ObservableCollection<AnnotationsFilterModel>();
      foreach (AnnotationsFilterModel annotation in (Collection<AnnotationsFilterModel>) this.AnnotationList)
      {
        annotation.Count = 0;
        observableCollection2.Add(annotation);
      }
      this.AnnotationList.Clear();
      this.userList.Clear();
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      foreach (PageCommetCollection commetModel1 in (Collection<PageCommetCollection>) this.commetModels)
      {
        foreach (CommetModel commetModel2 in commetModel1)
        {
          CommetModel value = commetModel2;
          AnnotationsFilterModel annotationsFilterModel1 = new AnnotationsFilterModel()
          {
            Text = value.Text
          };
          AnnotationsFilterModel annotationsFilterModel2 = new AnnotationsFilterModel()
          {
            Text = value.Title
          };
          if (observableCollection1.Count < 1)
          {
            observableCollection1.Add(annotationsFilterModel1);
            this.userList.Add(annotationsFilterModel1);
          }
          else
          {
            bool flag1 = false;
            for (int index = 0; index < observableCollection1.Count; ++index)
            {
              if (observableCollection1[index].Text == value.Text)
              {
                if (this.userList.Where<AnnotationsFilterModel>((Func<AnnotationsFilterModel, bool>) (x => x.Text == value.Text)).Count<AnnotationsFilterModel>() < 1)
                  this.userList.Add(observableCollection1[index]);
                ++observableCollection1[index].Count;
                flag1 = true;
                break;
              }
            }
            if (!flag1)
            {
              if (!requiredService.IsUserFilterAllChecked.GetValueOrDefault())
                annotationsFilterModel1.IsSelect = false;
              observableCollection1.Add(annotationsFilterModel1);
              this.userList.Add(annotationsFilterModel1);
              bool? nullable1 = requiredService.IsUserFilterAllChecked;
              bool flag2 = false;
              if (nullable1.GetValueOrDefault() == flag2 & nullable1.HasValue)
              {
                MainViewModel mainViewModel = requiredService;
                nullable1 = new bool?();
                bool? nullable2 = nullable1;
                mainViewModel.IsUserFilterAllChecked = nullable2;
              }
            }
          }
          if (observableCollection2.Count < 1)
          {
            observableCollection2.Add(annotationsFilterModel2);
            this.AnnotationList.Add(annotationsFilterModel2);
          }
          else
          {
            bool flag3 = false;
            for (int index = 0; index < observableCollection2.Count; ++index)
            {
              if (observableCollection2[index].Text == value.Title)
              {
                if (this.AnnotationList.Where<AnnotationsFilterModel>((Func<AnnotationsFilterModel, bool>) (x => x.Text == value.Title)).Count<AnnotationsFilterModel>() < 1)
                  this.AnnotationList.Add(observableCollection2[index]);
                ++observableCollection2[index].Count;
                flag3 = true;
                break;
              }
            }
            if (!flag3)
            {
              if (!requiredService.IsKindFilterAllChecked.GetValueOrDefault())
                annotationsFilterModel2.IsSelect = false;
              observableCollection2.Add(annotationsFilterModel2);
              this.AnnotationList.Add(annotationsFilterModel2);
              bool? nullable3 = requiredService.IsKindFilterAllChecked;
              bool flag4 = false;
              if (nullable3.GetValueOrDefault() == flag4 & nullable3.HasValue)
              {
                MainViewModel mainViewModel = requiredService;
                nullable3 = new bool?();
                bool? nullable4 = nullable3;
                mainViewModel.IsKindFilterAllChecked = nullable4;
              }
            }
          }
        }
      }
      bool flag = false;
      foreach (AnnotationsFilterModel annotationsFilterModel in (Collection<AnnotationsFilterModel>) observableCollection2)
      {
        for (int index = 0; index < this.AnnotationList.Count; ++index)
        {
          if (this.AnnotationList[index].Text == annotationsFilterModel.Text)
          {
            this.AnnotationList[index].Count = annotationsFilterModel.Count;
            if (this.AnnotationList[index].IsSelect)
              flag = true;
          }
        }
      }
      if (!flag)
      {
        if (!requiredService.IsKindFilterAllChecked.HasValue)
          requiredService.IsKindFilterAllChecked = new bool?(false);
      }
      else
        flag = true;
      foreach (AnnotationsFilterModel annotationsFilterModel in (Collection<AnnotationsFilterModel>) observableCollection1)
      {
        for (int index = 0; index < this.userList.Count; ++index)
        {
          if (this.userList[index].Text == annotationsFilterModel.Text)
          {
            this.userList[index].Count = annotationsFilterModel.Count;
            if (this.userList[index].IsSelect)
              flag = true;
          }
        }
      }
      if (!flag && !requiredService.IsUserFilterAllChecked.HasValue)
        requiredService.IsUserFilterAllChecked = new bool?(false);
      this.FilterShowItems();
    }
    catch
    {
    }
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (disposing)
      this.cts.Cancel();
    this.disposedValue = true;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }
}
