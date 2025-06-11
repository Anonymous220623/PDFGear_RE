// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Screenshot
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class Screenshot
{
  public static event EventHandler<FunctionEventArgs<ImageSource>> Snapped;

  public void Start() => new ScreenshotWindow(this).Show();

  internal void OnSnapped(ImageSource source)
  {
    EventHandler<FunctionEventArgs<ImageSource>> snapped = Screenshot.Snapped;
    if (snapped == null)
      return;
    snapped((object) this, new FunctionEventArgs<ImageSource>(source));
  }
}
