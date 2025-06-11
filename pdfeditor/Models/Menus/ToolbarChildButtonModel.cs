// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarChildButtonModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Models.Menus;

public class ToolbarChildButtonModel : ObservableObject
{
  private bool isDropDownIconVisible;
  private ICommand command;
  private object commandParameter;
  private bool isEnabled = true;
  private ToolbarButtonModel parent;

  public ToolbarChildButtonModel() => this.isDropDownIconVisible = true;

  public bool IsDropDownIconVisible
  {
    get => this.isDropDownIconVisible;
    set
    {
      this.SetProperty<bool>(ref this.isDropDownIconVisible, value, nameof (IsDropDownIconVisible));
    }
  }

  public ICommand Command
  {
    get => this.command;
    set => this.SetProperty<ICommand>(ref this.command, value, nameof (Command));
  }

  public object CommandParameter
  {
    get => this.commandParameter;
    set => this.SetProperty<object>(ref this.commandParameter, value, nameof (CommandParameter));
  }

  public virtual void Tap() => this.Command?.Execute(this.CommandParameter ?? (object) this);

  public bool IsEnabled
  {
    get => this.isEnabled;
    set => this.SetProperty<bool>(ref this.isEnabled, value, nameof (IsEnabled));
  }

  public ToolbarButtonModel Parent
  {
    get => this.parent;
    set => this.SetProperty<ToolbarButtonModel>(ref this.parent, value, nameof (Parent));
  }
}
