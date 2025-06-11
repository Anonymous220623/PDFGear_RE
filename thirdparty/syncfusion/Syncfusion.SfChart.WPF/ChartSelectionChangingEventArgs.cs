// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartSelectionChangingEventArgs
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartSelectionChangingEventArgs : EventArgs
{
  private bool isDataPointSelection = true;

  public ChartSeriesBase SelectedSeries { get; internal set; }

  public ChartSegment SelectedSegment { get; internal set; }

  public List<ChartSegment> SelectedSegments { get; set; }

  public int SelectedIndex { get; internal set; }

  public int PreviousSelectedIndex { get; internal set; }

  public bool Cancel { get; set; }

  public bool IsDataPointSelection
  {
    get => this.isDataPointSelection;
    internal set => this.isDataPointSelection = value;
  }

  public bool IsSelected { get; internal set; }
}
