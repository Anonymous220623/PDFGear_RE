// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TileViewDragEventArgs
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class TileViewDragEventArgs : EventArgs
{
  internal TileViewDragEventArgs()
  {
  }

  public TileViewDragEventArgs(
    double horizontalChange,
    double verticalChange,
    MouseEventArgs mouseEventArgs,
    string eventName)
  {
    this.HorizontalChange = horizontalChange;
    this.VerticalChange = verticalChange;
    this.MouseEventArgs = mouseEventArgs;
    this.Event = eventName;
  }

  public double HorizontalChange { get; set; }

  public string Event { get; set; }

  public double VerticalChange { get; set; }

  public MouseEventArgs MouseEventArgs { get; set; }
}
