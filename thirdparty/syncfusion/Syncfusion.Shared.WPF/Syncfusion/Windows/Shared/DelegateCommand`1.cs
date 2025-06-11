// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DelegateCommand`1
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class DelegateCommand<T> : ICommand
{
  private Predicate<T> _canExecute;
  private Action<T> _method;
  private bool _canExecuteCache = true;

  public DelegateCommand(Action<T> method)
    : this(method, (Predicate<T>) null)
  {
  }

  public DelegateCommand(Action<T> method, Predicate<T> canExecute)
  {
    this._method = method;
    this._canExecute = canExecute;
  }

  public bool CanExecute(object parameter)
  {
    if (this._canExecute != null)
    {
      bool flag = this._canExecute((T) parameter);
      if (this._canExecuteCache != flag)
      {
        this._canExecuteCache = flag;
        this.RaiseCanExecuteChanged();
      }
    }
    return this._canExecuteCache;
  }

  public void RaiseCanExecuteChanged()
  {
    if (this.CanExecuteChanged == null)
      return;
    this.CanExecuteChanged((object) this, new EventArgs());
  }

  public void Execute(object parameter)
  {
    if (this._method == null)
      return;
    this._method((T) parameter);
  }

  public event EventHandler CanExecuteChanged;
}
