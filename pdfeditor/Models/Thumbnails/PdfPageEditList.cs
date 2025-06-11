// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Thumbnails.PdfPageEditList
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

#nullable disable
namespace pdfeditor.Models.Thumbnails;

public class PdfPageEditList : ObservableCollection<PdfPageEditListModel>
{
  private List<PdfPageEditListModel> selectedItems;
  private double scale = 0.5;
  private double minAspectRatio;
  private double placeholderWidth = PdfPageEditListModel.DefaultThumbnailWidth * 0.5;
  private double placeholderHeight = PdfPageEditListModel.DefaultThumbnailWidth * 0.5 * 1.414;
  private bool? allItemSelected;
  private bool allItemSelectedPropChanging;

  public PdfPageEditList() => this.selectedItems = new List<PdfPageEditListModel>();

  public PdfPageEditList(IEnumerable<PdfPageEditListModel> source)
    : base(source)
  {
    this.selectedItems = new List<PdfPageEditListModel>();
    foreach (PdfPageEditListModel source1 in this.Items.OfType<PdfPageEditListModel>())
    {
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler((INotifyPropertyChanged) source1, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.OnItemPropertyChanged));
      if (source1.Selected)
        this.selectedItems.Add(source1);
    }
    this.UpdateMinAspectRatio();
    this.UpdateAllItemSelected();
  }

  public IReadOnlyCollection<PdfPageEditListModel> SelectedItems
  {
    get => (IReadOnlyCollection<PdfPageEditListModel>) this.selectedItems;
  }

  public double Scale
  {
    get => this.scale;
    set
    {
      if (this.scale == value)
        return;
      this.scale = value;
      this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (Scale)));
      this.UpdateChildrenThumbnailSize();
      this.PlaceholderWidth = PdfPageEditListModel.DefaultThumbnailWidth * value;
      if (this.minAspectRatio == 0.0)
        this.PlaceholderHeight = this.PlaceholderWidth * 1.414;
      else
        this.PlaceholderHeight = this.PlaceholderWidth / this.minAspectRatio;
    }
  }

  public double PlaceholderWidth
  {
    get => this.placeholderWidth;
    set
    {
      if (this.placeholderWidth == value)
        return;
      this.placeholderWidth = value;
      this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (PlaceholderWidth)));
    }
  }

  public double PlaceholderHeight
  {
    get => this.placeholderHeight;
    set
    {
      if (this.placeholderHeight == value)
        return;
      this.placeholderHeight = value;
      this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (PlaceholderHeight)));
    }
  }

  public double MinAspectRatio
  {
    get => this.minAspectRatio;
    set
    {
      if (this.minAspectRatio == value)
        return;
      this.minAspectRatio = value;
      this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (MinAspectRatio)));
      if (this.minAspectRatio == 0.0)
        this.PlaceholderHeight = this.PlaceholderWidth * 1.414;
      else
        this.PlaceholderHeight = this.PlaceholderWidth / this.minAspectRatio;
    }
  }

  public bool? AllItemSelected
  {
    get => this.allItemSelected;
    set
    {
      bool? allItemSelected = this.allItemSelected;
      bool? nullable = value;
      if (!(allItemSelected.GetValueOrDefault() == nullable.GetValueOrDefault() & allItemSelected.HasValue == nullable.HasValue))
      {
        if (!value.HasValue)
          throw new ArgumentException(nameof (AllItemSelected));
        lock (this)
        {
          this.allItemSelectedPropChanging = true;
          try
          {
            if (value.GetValueOrDefault())
            {
              foreach (PdfPageEditListModel pageEditListModel in (Collection<PdfPageEditListModel>) this)
                pageEditListModel.Selected = true;
            }
            else
            {
              foreach (PdfPageEditListModel pageEditListModel in (Collection<PdfPageEditListModel>) this)
                pageEditListModel.Selected = false;
            }
          }
          finally
          {
            this.allItemSelectedPropChanging = false;
          }
        }
      }
      this.SetAllItemSelectedCore(value);
      this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItems"));
    }
  }

  private void SetAllItemSelectedCore(bool? value)
  {
    bool? allItemSelected = this.allItemSelected;
    bool? nullable = value;
    if (allItemSelected.GetValueOrDefault() == nullable.GetValueOrDefault() & allItemSelected.HasValue == nullable.HasValue)
      return;
    this.allItemSelected = value;
    this.OnPropertyChanged(new PropertyChangedEventArgs("AllItemSelected"));
  }

  private void UpdateMinAspectRatio() => this.UpdateMaxPageSize(out bool _);

  private void UpdateMaxPageSize(out bool childrenThumbnailUpdated)
  {
    childrenThumbnailUpdated = false;
    if (this.selectedItems == null)
      return;
    PdfPageEditListModel[] source = (PdfPageEditListModel[]) null;
    lock (this)
      source = this.ToArray<PdfPageEditListModel>();
    double minAspectRatio1 = this.MinAspectRatio;
    this.MinAspectRatio = ((IEnumerable<PdfPageEditListModel>) source).Where<PdfPageEditListModel>((Func<PdfPageEditListModel, bool>) (c => c.PageHeight != 0.0)).Select<PdfPageEditListModel, double>((Func<PdfPageEditListModel, double>) (c => c.PageWidth / c.PageHeight)).DefaultIfEmpty<double>().Min();
    double minAspectRatio2 = this.MinAspectRatio;
    if (minAspectRatio1 == minAspectRatio2)
      return;
    this.UpdateChildrenThumbnailSize();
    childrenThumbnailUpdated = true;
  }

  private void UpdateChildrenThumbnailSize()
  {
    lock (this)
    {
      foreach (PdfPageEditListModel pageEditListModel in (Collection<PdfPageEditListModel>) this)
        pageEditListModel.UpdateThumbnailSize(this.Scale, this.MinAspectRatio);
    }
  }

  private void UpdateAllItemSelected()
  {
    lock (this.selectedItems)
    {
      bool? nullable = new bool?(false);
      nullable = this.Count == 0 || this.selectedItems.Count == 0 ? new bool?(false) : (this.Count != this.selectedItems.Count ? new bool?() : new bool?(true));
      this.SetAllItemSelectedCore(nullable);
    }
  }

  protected override void InsertItem(int index, PdfPageEditListModel item)
  {
    lock (this)
      base.InsertItem(index, item);
    bool childrenThumbnailUpdated;
    this.UpdateMaxPageSize(out childrenThumbnailUpdated);
    if (!childrenThumbnailUpdated)
      item.UpdateThumbnailSize(this.Scale, this.MinAspectRatio);
    INotifyPropertyChanged source = (INotifyPropertyChanged) item;
    if (source != null)
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(source, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.OnItemPropertyChanged));
    if (item.Selected)
    {
      lock (this.selectedItems)
      {
        this.selectedItems.Add(item);
        this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItems"));
      }
    }
    this.UpdateAllItemSelected();
  }

  protected override void SetItem(int index, PdfPageEditListModel item)
  {
    PdfPageEditListModel source1 = this.Items[index];
    if (source1 != null)
    {
      if (source1.Selected)
      {
        lock (this.selectedItems)
        {
          this.selectedItems.Remove(source1);
          this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItems"));
        }
      }
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.RemoveHandler((INotifyPropertyChanged) source1, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.OnItemPropertyChanged));
    }
    lock (this)
      base.SetItem(index, item);
    bool childrenThumbnailUpdated;
    this.UpdateMaxPageSize(out childrenThumbnailUpdated);
    if (!childrenThumbnailUpdated)
      item.UpdateThumbnailSize(this.Scale, this.MinAspectRatio);
    INotifyPropertyChanged source2 = (INotifyPropertyChanged) item;
    if (source2 != null)
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(source2, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.OnItemPropertyChanged));
    if (item.Selected)
    {
      lock (this.selectedItems)
      {
        this.selectedItems.Add(item);
        this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItems"));
      }
    }
    this.UpdateAllItemSelected();
  }

  protected override void RemoveItem(int index)
  {
    PdfPageEditListModel source = this.Items[index];
    if (source != null)
    {
      if (source.Selected)
      {
        lock (this.selectedItems)
        {
          this.selectedItems.Remove(source);
          this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItems"));
        }
      }
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.RemoveHandler((INotifyPropertyChanged) source, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.OnItemPropertyChanged));
    }
    lock (this)
      base.RemoveItem(index);
    this.UpdateMinAspectRatio();
    this.UpdateAllItemSelected();
  }

  protected override void ClearItems()
  {
    foreach (INotifyPropertyChanged source in this.Items.OfType<INotifyPropertyChanged>())
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.RemoveHandler(source, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.OnItemPropertyChanged));
    lock (this.selectedItems)
    {
      this.selectedItems.Clear();
      this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItems"));
    }
    lock (this)
      base.ClearItems();
    this.UpdateMinAspectRatio();
    this.UpdateAllItemSelected();
  }

  private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (!(sender is PdfPageEditListModel pageEditListModel))
      return;
    if (e.PropertyName == "Selected")
    {
      lock (this.selectedItems)
      {
        if (pageEditListModel.Selected)
          this.selectedItems.Add(pageEditListModel);
        else
          this.selectedItems.Remove(pageEditListModel);
        if (!this.allItemSelectedPropChanging)
          this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItems"));
      }
      if (!this.allItemSelectedPropChanging)
        this.UpdateAllItemSelected();
    }
    if (!(e.PropertyName == "PageWidth") && !(e.PropertyName == "PageHeight"))
      return;
    this.UpdateMinAspectRatio();
  }
}
