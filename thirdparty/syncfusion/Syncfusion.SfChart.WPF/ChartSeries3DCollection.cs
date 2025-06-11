// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartSeries3DCollection
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.ObjectModel;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartSeries3DCollection : ObservableCollection<ChartSeries3D>
{
  public ChartSeries3D this[string name]
  {
    get
    {
      foreach (ChartSeries3D chartSeries3D in (Collection<ChartSeries3D>) this)
      {
        if (chartSeries3D.Name == name)
          return chartSeries3D;
      }
      return (ChartSeries3D) null;
    }
  }
}
