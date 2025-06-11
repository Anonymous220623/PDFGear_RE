// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.SelectableContextMenuItemModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

#nullable disable
namespace pdfeditor.Models.Menus;

public class SelectableContextMenuItemModel : ContextMenuItemModel
{
  private bool innerSet;

  public ContextMenuItemModel SelectedItem
  {
    get
    {
      return this.OfType<ContextMenuItemModel>().FirstOrDefault<ContextMenuItemModel>((Func<ContextMenuItemModel, bool>) (c => c.IsChecked && c.IsEndPoint));
    }
  }

  protected override void InsertItem(int index, IContextMenuModel item)
  {
    base.InsertItem(index, item);
    INotifyPropertyChanged source = (INotifyPropertyChanged) item;
    if (source != null)
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(source, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.Notify_PropertyChanged));
    this.OnItemPropertyChanged(item as ContextMenuItemModel);
  }

  protected override void SetItem(int index, IContextMenuModel item)
  {
    INotifyPropertyChanged source1 = (INotifyPropertyChanged) this[index];
    if (source1 != null)
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.RemoveHandler(source1, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.Notify_PropertyChanged));
    base.SetItem(index, item);
    INotifyPropertyChanged source2 = (INotifyPropertyChanged) item;
    if (source2 != null)
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(source2, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.Notify_PropertyChanged));
    this.OnItemPropertyChanged(item as ContextMenuItemModel);
  }

  protected override void RemoveItem(int index)
  {
    INotifyPropertyChanged source = (INotifyPropertyChanged) this[index];
    if (source != null)
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.RemoveHandler(source, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.Notify_PropertyChanged));
    base.RemoveItem(index);
    this.OnItemPropertyChanged((ContextMenuItemModel) null);
  }

  protected override void ClearItems()
  {
    foreach (INotifyPropertyChanged source in (Collection<IContextMenuModel>) this)
    {
      if (source != null)
        WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.RemoveHandler(source, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.Notify_PropertyChanged));
    }
    base.ClearItems();
    this.OnItemPropertyChanged((ContextMenuItemModel) null);
  }

  private void Notify_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (!(e.PropertyName == "IsChecked"))
      return;
    this.OnItemPropertyChanged((ContextMenuItemModel) sender);
  }

  private void OnItemPropertyChanged(ContextMenuItemModel item)
  {
    if (this.innerSet)
      return;
    try
    {
      this.innerSet = true;
      if (item != null && item.IsChecked)
      {
        foreach (ContextMenuItemModel contextMenuItemModel in this.OfType<ContextMenuItemModel>())
        {
          if (contextMenuItemModel != item)
            contextMenuItemModel.IsChecked = false;
        }
      }
      this.OnPropertyChanged("SelectedItem");
      SelectableModelSelectionChangedEventHandler selectionChanged = this.SelectionChanged;
      if (selectionChanged == null)
        return;
      selectionChanged((object) this, new SelectableModelSelectionChangedEventArgs(this.SelectedItem));
    }
    finally
    {
      this.innerSet = false;
    }
  }

  public event SelectableModelSelectionChangedEventHandler SelectionChanged;
}
