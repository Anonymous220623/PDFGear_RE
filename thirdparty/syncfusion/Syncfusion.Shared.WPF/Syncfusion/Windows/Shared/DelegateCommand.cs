// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DelegateCommand
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class DelegateCommand : ICommand
{
  private Predicate<object> canExecute;
  private Action<object> executeAction;
  private bool canExecuteCache = true;

  public event EventHandler CanExecuteChanged;

  public DelegateCommand(Action<object> executeAction) => this.executeAction = executeAction;

  public DelegateCommand(Action<object> executeAction, Predicate<object> canExecute)
  {
    this.executeAction = executeAction;
    this.canExecute = canExecute;
  }

  public bool CanExecute(object parameter)
  {
    if (this.canExecute != null)
    {
      bool flag = this.canExecute(parameter);
      if (this.canExecuteCache != flag)
      {
        this.canExecuteCache = flag;
        this.RaiseCanExecuteChanged();
      }
    }
    return this.canExecuteCache;
  }

  public void RaiseCanExecuteChanged()
  {
    if (this.CanExecuteChanged == null)
      return;
    this.CanExecuteChanged((object) this, new EventArgs());
  }

  public void Execute(object parameter) => this.executeAction(parameter);
}
