// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartBehaviorsCollection
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartBehaviorsCollection : ObservableCollection<ChartBehavior>
{
  internal SfChart Area;

  public ChartBehaviorsCollection(SfChart area) => this.Area = area;

  public ChartBehaviorsCollection()
  {
  }

  protected override void InsertItem(int index, ChartBehavior item)
  {
    item.ChartArea = this.Area;
    item.AdorningCanvas = this.Area.GetAdorningCanvas();
    if (item.AdorningCanvas != null)
      item.InternalAttachElements();
    base.InsertItem(index, item);
  }

  protected override void RemoveItem(int index)
  {
    ChartBehavior chartBehavior = this.Items[index];
    chartBehavior.DetachElements();
    chartBehavior.ChartArea = this.Area;
    base.RemoveItem(index);
  }

  protected override void ClearItems()
  {
    foreach (ChartBehavior chartBehavior in (IEnumerable<ChartBehavior>) this.Items)
    {
      chartBehavior.DetachElements();
      chartBehavior.ChartArea = this.Area;
    }
    base.ClearItems();
  }
}
