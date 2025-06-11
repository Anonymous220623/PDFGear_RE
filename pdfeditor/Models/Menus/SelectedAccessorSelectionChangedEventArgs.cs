// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.SelectedAccessorSelectionChangedEventArgs
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;

#nullable disable
namespace pdfeditor.Models.Menus;

public class SelectedAccessorSelectionChangedEventArgs : EventArgs
{
  public SelectedAccessorSelectionChangedEventArgs(
    ContextMenuItemModel oldItem,
    ContextMenuItemModel newItem,
    ContextMenuItemType type)
  {
    this.OldItem = oldItem;
    this.NewItem = newItem;
    this.Type = type;
  }

  public ContextMenuItemType Type { get; }

  public ContextMenuItemModel OldItem { get; }

  public ContextMenuItemModel NewItem { get; }
}
