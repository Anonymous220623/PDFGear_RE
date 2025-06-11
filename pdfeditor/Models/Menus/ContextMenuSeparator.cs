// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ContextMenuSeparator
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.ComponentModel;

#nullable disable
namespace pdfeditor.Models.Menus;

public class ContextMenuSeparator : 
  ObservableObject,
  IContextMenuModel,
  INotifyPropertyChanging,
  INotifyPropertyChanged
{
  private IContextMenuModel parent;

  public IContextMenuModel Parent
  {
    get => this.parent;
    set
    {
      if (this.parent != value)
        this.OnPropertyChanging("Level");
      if (!this.SetProperty<IContextMenuModel>(ref this.parent, value, nameof (Parent)))
        return;
      this.OnPropertyChanged("Level");
    }
  }

  public int Level
  {
    get
    {
      IContextMenuModel parent = this.Parent;
      return parent == null ? -1 : parent.Level + 1;
    }
  }

  public string Name => "Separator";
}
