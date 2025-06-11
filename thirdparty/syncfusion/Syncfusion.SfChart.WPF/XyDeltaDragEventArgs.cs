// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.XyDeltaDragEventArgs
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class XyDeltaDragEventArgs : XySegmentDragEventArgs
{
  public object BaseXValue { get; set; }

  public object NewXValue { get; set; }

  public object DeltaX { get; set; }
}
