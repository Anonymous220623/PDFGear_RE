// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.ShutdownAppCommand
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Interactivity;

public class ShutdownAppCommand : ICommand
{
  public bool CanExecute(object parameter) => true;

  public void Execute(object parameter) => Application.Current.Shutdown();

  public event EventHandler CanExecuteChanged;
}
