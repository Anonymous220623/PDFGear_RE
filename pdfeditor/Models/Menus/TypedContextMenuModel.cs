// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.TypedContextMenuModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

#nullable disable
namespace pdfeditor.Models.Menus;

public class TypedContextMenuModel : ContextMenuModel
{
  private ToolbarChildCheckableButtonModel owner;

  public TypedContextMenuModel(AnnotationMode mode)
  {
    this.SelectedItems = new ContextMenuModelTypedSelectedAccessor((ContextMenuModel) this);
    this.SelectedItems.SelectionChanged += (EventHandler<SelectedAccessorSelectionChangedEventArgs>) ((s, a) =>
    {
      EventHandler<SelectedAccessorSelectionChangedEventArgs> selectionChanged = this.SelectionChanged;
      if (selectionChanged == null)
        return;
      selectionChanged((object) this, a);
    });
    this.Mode = mode;
  }

  public AnnotationMode Mode { get; }

  public ContextMenuModelTypedSelectedAccessor SelectedItems { get; }

  public ToolbarChildCheckableButtonModel Owner
  {
    get => this.owner;
    set
    {
      this.SetProperty<ToolbarChildCheckableButtonModel>(ref this.owner, value, nameof (Owner));
    }
  }

  protected override void InsertItem(int index, IContextMenuModel item)
  {
    if (!(item is ContextMenuSeparator))
    {
      TypedContextMenuItemModel model = item as TypedContextMenuItemModel;
      if (model == null || !this.OfType<TypedContextMenuItemModel>().All<TypedContextMenuItemModel>((Func<TypedContextMenuItemModel, bool>) (c => c.Type != model.Type)))
      {
        base.InsertItem(index, item);
        return;
      }
    }
    base.InsertItem(index, item);
    if (item is SelectableContextMenuItemModel source)
      WeakEventManager<SelectableContextMenuItemModel, SelectableModelSelectionChangedEventArgs>.AddHandler(source, "SelectionChanged", new EventHandler<SelectableModelSelectionChangedEventArgs>(this.Item_SelectionChanged));
    this.UpdateSelectedItem();
  }

  protected override void SetItem(int index, IContextMenuModel item)
  {
    IContextMenuModel oldItem = this[index];
    if (oldItem is SelectableContextMenuItemModel source1)
      WeakEventManager<SelectableContextMenuItemModel, SelectableModelSelectionChangedEventArgs>.AddHandler(source1, "SelectionChanged", new EventHandler<SelectableModelSelectionChangedEventArgs>(this.Item_SelectionChanged));
    if (!(item is ContextMenuSeparator))
    {
      TypedContextMenuItemModel model = item as TypedContextMenuItemModel;
      if (model == null || !this.Where<IContextMenuModel>((Func<IContextMenuModel, bool>) (c => c != oldItem)).OfType<TypedContextMenuItemModel>().All<TypedContextMenuItemModel>((Func<TypedContextMenuItemModel, bool>) (c => c.Type != model.Type)))
      {
        base.InsertItem(index, item);
        goto label_7;
      }
    }
    base.SetItem(index, item);
    if (item is SelectableContextMenuItemModel source2)
      WeakEventManager<SelectableContextMenuItemModel, SelectableModelSelectionChangedEventArgs>.AddHandler(source2, "SelectionChanged", new EventHandler<SelectableModelSelectionChangedEventArgs>(this.Item_SelectionChanged));
label_7:
    this.UpdateSelectedItem();
  }

  protected override void RemoveItem(int index)
  {
    if (this[index] is SelectableContextMenuItemModel source)
      WeakEventManager<SelectableContextMenuItemModel, SelectableModelSelectionChangedEventArgs>.RemoveHandler(source, "SelectionChanged", new EventHandler<SelectableModelSelectionChangedEventArgs>(this.Item_SelectionChanged));
    base.RemoveItem(index);
    this.UpdateSelectedItem();
  }

  protected override void ClearItems()
  {
    foreach (IContextMenuModel contextMenuModel in (Collection<IContextMenuModel>) this)
    {
      if (contextMenuModel is SelectableContextMenuItemModel source)
        WeakEventManager<SelectableContextMenuItemModel, SelectableModelSelectionChangedEventArgs>.RemoveHandler(source, "SelectionChanged", new EventHandler<SelectableModelSelectionChangedEventArgs>(this.Item_SelectionChanged));
    }
    base.ClearItems();
    this.UpdateSelectedItem();
  }

  private void Item_SelectionChanged(object sender, SelectableModelSelectionChangedEventArgs args)
  {
    this.UpdateSelectedItem();
  }

  private void UpdateSelectedItem() => this.OnPropertyChanged("SelectedItems");

  public event EventHandler<SelectedAccessorSelectionChangedEventArgs> SelectionChanged;
}
