// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAxisCollection
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.ObjectModel;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartAxisCollection : ObservableCollection<ChartAxis>
{
  public ChartAxis this[string name]
  {
    get
    {
      if (string.IsNullOrEmpty(name))
        return (ChartAxis) null;
      foreach (ChartAxis chartAxis in (Collection<ChartAxis>) this)
      {
        if (chartAxis.Name == name)
          return chartAxis;
      }
      return (ChartAxis) null;
    }
  }

  protected override void InsertItem(int index, ChartAxis item)
  {
    if (item != null && !this.Contains(item))
      base.InsertItem(index, item);
    if (!this.Contains(item) || item.Area.DependentSeriesAxes == null || !item.Area.DependentSeriesAxes.Contains(item))
      return;
    item.Area.DependentSeriesAxes.Remove(item);
  }

  internal void RemoveItem(ChartAxis axis, bool flag)
  {
    if (!flag)
      return;
    this.Remove(axis);
  }
}
