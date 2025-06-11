// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ContextMenuModelTypedSelectedAccessor
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

#nullable disable
namespace pdfeditor.Models.Menus;

public class ContextMenuModelTypedSelectedAccessor : ObservableObject
{
  private readonly ContextMenuModel parent;
  private Dictionary<ContextMenuItemType, ContextMenuItemModel> lastSelectedItems;

  public ContextMenuModelTypedSelectedAccessor(ContextMenuModel parent)
  {
    WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler((INotifyPropertyChanged) parent, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(this.OnParentPropertyChanged));
    this.parent = parent;
    this.lastSelectedItems = this.GetSelectedItems();
  }

  public ContextMenuItemModel StrokeColor
  {
    get => this.GetCachedSelectedItem(ContextMenuItemType.StrokeColor);
  }

  public ContextMenuItemModel FillColor
  {
    get => this.GetCachedSelectedItem(ContextMenuItemType.FillColor);
  }

  public ContextMenuItemModel StrokeThickness
  {
    get => this.GetCachedSelectedItem(ContextMenuItemType.StrokeThickness);
  }

  public ContextMenuItemModel FontSize => this.GetCachedSelectedItem(ContextMenuItemType.FontSize);

  public ContextMenuItemModel FontName => this.GetCachedSelectedItem(ContextMenuItemType.FontName);

  public ContextMenuItemModel FontColor
  {
    get => this.GetCachedSelectedItem(ContextMenuItemType.FontColor);
  }

  private void OnParentPropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    List<SelectedAccessorSelectionChangedEventArgs> changedEventArgsList = (List<SelectedAccessorSelectionChangedEventArgs>) null;
    try
    {
      Dictionary<ContextMenuItemType, ContextMenuItemModel> lastSelectedItems = this.lastSelectedItems;
      if (e.PropertyName == "SelectedItems")
      {
        if (lastSelectedItems != null)
        {
          Dictionary<ContextMenuItemType, ContextMenuItemModel> selectedItems = this.GetSelectedItems();
          IReadOnlyList<ContextMenuItemType> allValues = EnumHelper<ContextMenuItemType>.AllValues;
          for (int index = 0; index < allValues.Count; ++index)
          {
            if (allValues[index] != ContextMenuItemType.None)
            {
              ContextMenuItemType contextMenuItemType = allValues[index];
              ContextMenuItemModel oldItem = (ContextMenuItemModel) null;
              ContextMenuItemModel newItem = (ContextMenuItemModel) null;
              lastSelectedItems.TryGetValue(contextMenuItemType, out oldItem);
              selectedItems.TryGetValue(contextMenuItemType, out newItem);
              if (oldItem != newItem && this.SelectionChanged != null)
              {
                if (changedEventArgsList == null)
                  changedEventArgsList = new List<SelectedAccessorSelectionChangedEventArgs>();
                SelectedAccessorSelectionChangedEventArgs changedEventArgs = new SelectedAccessorSelectionChangedEventArgs(oldItem, newItem, contextMenuItemType);
                changedEventArgsList.Add(changedEventArgs);
              }
            }
          }
        }
      }
    }
    finally
    {
      this.lastSelectedItems = this.GetSelectedItems();
    }
    if (changedEventArgsList == null)
      return;
    foreach (SelectedAccessorSelectionChangedEventArgs e1 in changedEventArgsList)
    {
      EventHandler<SelectedAccessorSelectionChangedEventArgs> selectionChanged = this.SelectionChanged;
      if (selectionChanged != null)
        selectionChanged((object) this, e1);
    }
  }

  private ContextMenuItemModel GetCachedSelectedItem(ContextMenuItemType type)
  {
    if (this.lastSelectedItems == null)
      return (ContextMenuItemModel) null;
    ContextMenuItemModel contextMenuItemModel;
    return this.lastSelectedItems.TryGetValue(type, out contextMenuItemModel) ? contextMenuItemModel : (ContextMenuItemModel) null;
  }

  private ContextMenuItemModel GetSelectedItem(ContextMenuItemType type)
  {
    return this.parent.OfType<TypedContextMenuItemModel>().FirstOrDefault<TypedContextMenuItemModel>((Func<TypedContextMenuItemModel, bool>) (c => c.Type == type))?.SelectedItem;
  }

  private Dictionary<ContextMenuItemType, ContextMenuItemModel> GetSelectedItems()
  {
    return EnumHelper<ContextMenuItemType>.AllValues.Select<ContextMenuItemType, (ContextMenuItemType, ContextMenuItemModel)>((Func<ContextMenuItemType, (ContextMenuItemType, ContextMenuItemModel)>) (c => (c, this.GetSelectedItem(c)))).Where<(ContextMenuItemType, ContextMenuItemModel)>((Func<(ContextMenuItemType, ContextMenuItemModel), bool>) (c => c.Item2 != null)).ToDictionary<(ContextMenuItemType, ContextMenuItemModel), ContextMenuItemType, ContextMenuItemModel>((Func<(ContextMenuItemType, ContextMenuItemModel), ContextMenuItemType>) (c => c.c), (Func<(ContextMenuItemType, ContextMenuItemModel), ContextMenuItemModel>) (c => c.Item2));
  }

  public event EventHandler<SelectedAccessorSelectionChangedEventArgs> SelectionChanged;
}
