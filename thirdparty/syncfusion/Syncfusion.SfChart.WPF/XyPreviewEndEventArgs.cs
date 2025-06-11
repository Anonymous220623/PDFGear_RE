// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.XyPreviewEndEventArgs
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.ComponentModel;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class XyPreviewEndEventArgs : CancelEventArgs
{
  public double BaseYValue { get; set; }

  public double NewYValue { get; set; }
}
