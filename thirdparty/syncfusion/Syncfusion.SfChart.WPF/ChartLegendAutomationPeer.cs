// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartLegendAutomationPeer
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Text;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal class ChartLegendAutomationPeer(ChartLegend chartLegend) : ItemsControlAutomationPeer((ItemsControl) chartLegend)
{
  protected override bool IsControlElementCore() => true;

  protected override string GetNameCore() => this.Owner.GetType().Name;

  protected override string GetClassNameCore() => this.Owner.GetType().Name;

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Custom;
  }

  protected override string GetItemStatusCore()
  {
    if (!(this.Owner is ChartLegend))
      return (string) null;
    if (!(this.Owner is ChartLegend owner))
      return (string) null;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{(owner.Header == null ? (object) "null" : owner.Header ?? (object) "null")};");
    stringBuilder.Append($"{owner.Orientation};");
    stringBuilder.Append($"{owner.DockPosition};");
    stringBuilder.Append($"{owner.LegendPosition};");
    stringBuilder.Append($"{owner.IconVisibility};");
    stringBuilder.Append($"{owner.CheckBoxVisibility};");
    stringBuilder.Append($"{owner.IconWidth};");
    stringBuilder.Append($"{owner.IconHeight};");
    stringBuilder.Append($"{owner.ItemMargin};");
    stringBuilder.Append($"{owner.Background};");
    stringBuilder.Append($"{owner.OffsetX};");
    stringBuilder.Append($"{owner.OffsetY};");
    return stringBuilder.ToString();
  }

  protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
  {
    return (ItemAutomationPeer) new LegendItemsAutomationPeer(item, (ItemsControlAutomationPeer) this);
  }
}
