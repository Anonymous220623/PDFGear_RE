// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.MessageBoxInfo
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Data;

public class MessageBoxInfo
{
  public string Message { get; set; }

  public string Caption { get; set; }

  public MessageBoxButton Button { get; set; }

  public Geometry Icon { get; set; }

  public string IconKey { get; set; }

  public Brush IconBrush { get; set; }

  public string IconBrushKey { get; set; }

  public MessageBoxResult DefaultResult { get; set; }

  public Style Style { get; set; }

  public string StyleKey { get; set; }
}
