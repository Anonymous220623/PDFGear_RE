// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.BaseCommand
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

internal class BaseCommand : ICommand
{
  private readonly Action<object> _execute;
  private readonly Predicate<object> _canExecute;

  public BaseCommand(Action<object> execute)
    : this(execute, (Predicate<object>) null)
  {
  }

  public BaseCommand(Action<object> execute, Predicate<object> canExecute)
  {
    this._execute = execute != null ? execute : throw new ArgumentNullException(nameof (execute));
    this._canExecute = canExecute;
  }

  public bool CanExecute(object parameter)
  {
    return this._canExecute == null || this._canExecute(parameter);
  }

  public event EventHandler CanExecuteChanged
  {
    add => CommandManager.RequerySuggested += value;
    remove => CommandManager.RequerySuggested -= value;
  }

  public void Execute(object parameter) => this._execute(parameter);
}
