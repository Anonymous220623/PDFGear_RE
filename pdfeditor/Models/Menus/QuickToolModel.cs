// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.QuickToolModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Models.Menus;

public class QuickToolModel : ObservableObject
{
  private bool isVisible;
  private ICommand command;

  public bool IsVisible
  {
    get => this.isVisible;
    set => this.SetProperty<bool>(ref this.isVisible, value, nameof (IsVisible));
  }

  public ICommand Command
  {
    get => this.command;
    set => this.SetProperty<ICommand>(ref this.command, value, nameof (Command));
  }
}
