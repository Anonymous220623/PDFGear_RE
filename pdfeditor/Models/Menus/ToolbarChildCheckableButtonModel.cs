// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarChildCheckableButtonModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Windows;

#nullable disable
namespace pdfeditor.Models.Menus;

public class ToolbarChildCheckableButtonModel : ToolbarChildButtonModel
{
  private IContextMenuModel contextMenu;
  private bool isChecked;
  private bool openContextMenuOnChecked = true;

  public bool IsChecked
  {
    get => this.isChecked;
    set => this.SetProperty<bool>(ref this.isChecked, value, nameof (IsChecked));
  }

  public bool OpenContextMenuOnChecked
  {
    get => this.openContextMenuOnChecked;
    set
    {
      this.SetProperty<bool>(ref this.openContextMenuOnChecked, value, nameof (OpenContextMenuOnChecked));
    }
  }

  public IContextMenuModel ContextMenu
  {
    get => this.contextMenu;
    set
    {
      IContextMenuModel contextMenu1 = this.contextMenu;
      if (!this.SetProperty<IContextMenuModel>(ref this.contextMenu, value, nameof (ContextMenu)))
        return;
      if (contextMenu1 is TypedContextMenuModel source)
      {
        source.Owner = (ToolbarChildCheckableButtonModel) null;
        WeakEventManager<TypedContextMenuModel, SelectedAccessorSelectionChangedEventArgs>.RemoveHandler(source, "SelectionChanged", new EventHandler<SelectedAccessorSelectionChangedEventArgs>(this.ContextMenu_SelectionChanged));
      }
      if (!(this.contextMenu is TypedContextMenuModel contextMenu2))
        return;
      contextMenu2.Owner = this;
      WeakEventManager<TypedContextMenuModel, SelectedAccessorSelectionChangedEventArgs>.AddHandler(contextMenu2, "SelectionChanged", new EventHandler<SelectedAccessorSelectionChangedEventArgs>(this.ContextMenu_SelectionChanged));
    }
  }

  private void ContextMenu_SelectionChanged(
    object sender,
    SelectedAccessorSelectionChangedEventArgs e)
  {
    EventHandler<SelectedAccessorSelectionChangedEventArgs> selectionChanged = this.ContextMenuSelectionChanged;
    if (selectionChanged == null)
      return;
    selectionChanged((object) this, e);
  }

  public override void Tap()
  {
    this.IsChecked = !this.IsChecked;
    base.Tap();
  }

  public event EventHandler<SelectedAccessorSelectionChangedEventArgs> ContextMenuSelectionChanged;
}
