// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.InvokeCommandAction
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Interactivity;

public sealed class InvokeCommandAction : TriggerAction<DependencyObject>
{
  public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (InvokeCommandAction), (PropertyMetadata) null);
  public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (InvokeCommandAction), (PropertyMetadata) null);
  private string _commandName;

  public string CommandName
  {
    get
    {
      this.ReadPreamble();
      return this._commandName;
    }
    set
    {
      if (this.CommandName == value)
        return;
      this.WritePreamble();
      this._commandName = value;
      this.WritePostscript();
    }
  }

  public ICommand Command
  {
    get => (ICommand) this.GetValue(InvokeCommandAction.CommandProperty);
    set => this.SetValue(InvokeCommandAction.CommandProperty, (object) value);
  }

  public object CommandParameter
  {
    get => this.GetValue(InvokeCommandAction.CommandParameterProperty);
    set => this.SetValue(InvokeCommandAction.CommandParameterProperty, value);
  }

  protected override void Invoke(object parameter)
  {
    if (this.AssociatedObject == null)
      return;
    ICommand command = this.ResolveCommand();
    if (command == null || !command.CanExecute(this.CommandParameter))
      return;
    command.Execute(this.CommandParameter);
  }

  private ICommand ResolveCommand()
  {
    ICommand command = (ICommand) null;
    if (this.Command != null)
      command = this.Command;
    else if (this.AssociatedObject != null)
    {
      foreach (PropertyInfo property in this.AssociatedObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        if (typeof (ICommand).IsAssignableFrom(property.PropertyType) && string.Equals(property.Name, this.CommandName, StringComparison.Ordinal))
          command = (ICommand) property.GetValue((object) this.AssociatedObject, (object[]) null);
      }
    }
    return command;
  }
}
