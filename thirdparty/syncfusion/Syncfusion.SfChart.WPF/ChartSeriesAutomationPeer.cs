// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartSeriesAutomationPeer
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal class ChartSeriesAutomationPeer(ChartSeries chartSeries) : FrameworkElementAutomationPeer((FrameworkElement) chartSeries)
{
  protected override string GetNameCore() => this.Owner.GetType().Name;

  protected override string GetClassNameCore() => this.Owner.GetType().Name;

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Custom;
  }

  protected override string GetItemStatusCore()
  {
    if (!(this.Owner is ChartSeries))
      return (string) null;
    if (!(this.Owner is ChartSeries owner))
      return (string) null;
    StringBuilder stringBuilder = new StringBuilder();
    if (owner.Area != null)
      stringBuilder.Append($"{owner.Area};");
    stringBuilder.Append($"{owner.IsSeriesVisible};");
    stringBuilder.Append($"{owner.Interior ?? (Brush) Brushes.Transparent};");
    stringBuilder.Append($"{owner.Stroke ?? (Brush) Brushes.Transparent};");
    stringBuilder.Append($"{owner.StrokeThickness};");
    stringBuilder.Append($"{owner.SeriesSelectionBrush ?? (Brush) Brushes.Transparent};");
    stringBuilder.Append($"{owner.LegendIcon};");
    stringBuilder.Append($"{owner.VisibilityOnLegend};");
    stringBuilder.Append($"{(owner.Label == null ? (object) "null" : (object) owner.Label)};");
    stringBuilder.Append($"{owner.Palette};");
    if (owner.Segments != null)
      stringBuilder.Append($"{owner.Segments.Count<ChartSegment>()};");
    if (owner.Adornments != null)
      stringBuilder.Append($"{owner.Adornments.Count<ChartAdornment>()};");
    return stringBuilder.ToString();
  }
}
