// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SfChartAutomationPeer
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

internal class SfChartAutomationPeer(SfChart chart) : FrameworkElementAutomationPeer((FrameworkElement) chart)
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
    if (!(this.Owner is SfChart))
      return (string) null;
    if (!(this.Owner is SfChart owner))
      return (string) null;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{owner.Header};");
    stringBuilder.Append($"{owner.PrimaryAxis};");
    stringBuilder.Append($"{owner.SecondaryAxis};");
    stringBuilder.Append($"{owner.Palette};");
    stringBuilder.Append($"{owner.AreaBackground ?? (Brush) Brushes.Transparent};");
    stringBuilder.Append($"{owner.AreaBorderBrush};");
    stringBuilder.Append($"{owner.AreaBorderThickness};");
    stringBuilder.Append($"{owner.SideBySideSeriesPlacement};");
    if (owner.Series != null)
      stringBuilder.Append($"{owner.Series.Count<ChartSeries>()};");
    if (owner.Behaviors != null)
      stringBuilder.Append($"{owner.Behaviors.Count<ChartBehavior>()};");
    if (owner.Annotations != null)
      stringBuilder.Append($"{owner.Annotations.Count<Annotation>()};");
    if (owner.TechnicalIndicators != null)
      stringBuilder.Append($"{owner.TechnicalIndicators.Count<ChartSeries>()};");
    return stringBuilder.ToString();
  }
}
