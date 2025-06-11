// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LegendItemsAutomationPeer
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Text;
using System.Windows.Automation.Peers;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal class LegendItemsAutomationPeer : ItemAutomationPeer
{
  private LegendItem legendItem;

  public LegendItemsAutomationPeer(
    object item,
    ItemsControlAutomationPeer itemsControlAutomationPeer)
    : base(item, itemsControlAutomationPeer)
  {
    this.legendItem = item as LegendItem;
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Custom;
  }

  protected override string GetClassNameCore() => "LegendItem";

  protected override string GetItemStatusCore()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{this.legendItem.IconVisibility};");
    stringBuilder.Append($"{this.legendItem.CheckBoxVisibility};");
    stringBuilder.Append($"{this.legendItem.VisibilityOnLegend};");
    stringBuilder.Append($"{this.legendItem.IsSeriesVisible};");
    stringBuilder.Append($"{(string.IsNullOrEmpty(this.legendItem.Label) ? (object) "Null" : (object) this.legendItem.Label)};");
    stringBuilder.Append($"{this.legendItem.IconWidth};");
    stringBuilder.Append($"{this.legendItem.IconHeight};");
    stringBuilder.Append($"{this.legendItem.ItemMargin};");
    return stringBuilder.ToString();
  }
}
