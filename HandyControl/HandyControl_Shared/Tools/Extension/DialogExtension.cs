// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Extension.DialogExtension
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Controls;
using System;
using System.Threading.Tasks;
using System.Windows;

#nullable disable
namespace HandyControl.Tools.Extension;

public static class DialogExtension
{
  public static Task<TResult> GetResultAsync<TResult>(this Dialog dialog)
  {
    TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
    try
    {
      if (dialog.IsClosed)
      {
        SetResult();
      }
      else
      {
        dialog.Unloaded += new RoutedEventHandler(OnUnloaded);
        dialog.GetViewModel<IDialogResultable<TResult>>().CloseAction = new Action(dialog.Close);
      }
    }
    catch (Exception ex)
    {
      tcs.TrySetException(ex);
    }
    return tcs.Task;

    void OnUnloaded(object sender, RoutedEventArgs args)
    {
      dialog.Unloaded -= new RoutedEventHandler(OnUnloaded);
      SetResult();
    }

    void SetResult()
    {
      try
      {
        tcs.TrySetResult(dialog.GetViewModel<IDialogResultable<TResult>>().Result);
      }
      catch (Exception ex)
      {
        tcs.TrySetException(ex);
      }
    }
  }

  public static Dialog Initialize<TViewModel>(this Dialog dialog, Action<TViewModel> configure)
  {
    if (configure != null)
      configure(dialog.GetViewModel<TViewModel>());
    return dialog;
  }

  public static TViewModel GetViewModel<TViewModel>(this Dialog dialog)
  {
    if (!(dialog.Content is FrameworkElement content))
      throw new InvalidOperationException("The dialog is not a derived class of the FrameworkElement. ");
    if (content.DataContext is TViewModel dataContext)
      return dataContext;
    throw new InvalidOperationException($"The view model of the dialog is not the {typeof (TViewModel)} type or its derived class. ");
  }
}
