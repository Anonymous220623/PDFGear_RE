// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAxisAutomationPeer
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal class ChartAxisAutomationPeer(ChartAxis chartAxis) : FrameworkElementAutomationPeer((FrameworkElement) chartAxis)
{
  protected override string GetNameCore() => this.Owner.GetType().Name;

  protected override string GetClassNameCore() => this.Owner.GetType().Name;

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Custom;
  }

  protected override string GetItemStatusCore()
  {
    if (!(this.Owner is ChartAxis))
      return (string) null;
    if (!(this.Owner is ChartAxis owner))
      return (string) null;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{owner.VisibleRange};");
    stringBuilder.Append($"{owner.Header};");
    stringBuilder.Append($"{owner.HeaderPosition};");
    stringBuilder.Append($"{owner.OpposedPosition};");
    stringBuilder.Append($"{owner.Orientation};");
    stringBuilder.Append($"{owner.ShowGridLines};");
    stringBuilder.Append($"{owner.IsInversed};");
    stringBuilder.Append($"{owner.ShowTrackBallInfo};");
    stringBuilder.Append($"{(string.IsNullOrEmpty(owner.LabelFormat) ? (object) "null" : (object) owner.LabelFormat)};");
    stringBuilder.Append($"{owner.LabelsSource ?? (object) "null"};");
    stringBuilder.Append($"{owner.LabelExtent};");
    stringBuilder.Append($"{owner.LabelsPosition};");
    stringBuilder.Append($"{owner.MaximumLabels};");
    stringBuilder.Append($"{owner.LabelsIntersectAction};");
    stringBuilder.Append($"{owner.LabelRotationAngle};");
    stringBuilder.Append($"{owner.EdgeLabelsDrawingMode};");
    stringBuilder.Append($"{owner.EdgeLabelsVisibilityMode};");
    stringBuilder.Append($"{owner.TickLinesPosition};");
    return stringBuilder.ToString();
  }
}
