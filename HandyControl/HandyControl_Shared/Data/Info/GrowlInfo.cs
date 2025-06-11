// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.GrowlInfo
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Properties.Langs;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace HandyControl.Data;

public class GrowlInfo
{
  public string Message { get; set; }

  public bool ShowDateTime { get; set; } = true;

  public int WaitTime { get; set; } = 6;

  public string CancelStr { get; set; } = Lang.Cancel;

  public string ConfirmStr { get; set; } = Lang.Confirm;

  public Func<bool, bool> ActionBeforeClose { get; set; }

  public bool StaysOpen { get; set; }

  public bool IsCustom { get; set; }

  public InfoType Type { get; set; }

  public Geometry Icon { get; set; }

  public string IconKey { get; set; }

  public Brush IconBrush { get; set; }

  public string IconBrushKey { get; set; }

  public bool ShowCloseButton { get; set; } = true;

  public string Token { get; set; }

  public FlowDirection FlowDirection { get; set; }

  public Dispatcher Dispatcher { get; set; }
}
