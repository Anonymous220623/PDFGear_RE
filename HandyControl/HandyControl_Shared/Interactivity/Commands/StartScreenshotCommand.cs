// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.StartScreenshotCommand
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Controls;
using System;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Interactivity;

public class StartScreenshotCommand : ICommand
{
  public bool CanExecute(object parameter) => true;

  public void Execute(object parameter) => new Screenshot().Start();

  public event EventHandler CanExecuteChanged;
}
