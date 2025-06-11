// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartZoomingToolBarAutomationPeer
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal class ChartZoomingToolBarAutomationPeer(ZoomingToolBarItem toolBarItem) : 
  FrameworkElementAutomationPeer((FrameworkElement) toolBarItem)
{
  protected override string GetClassNameCore() => this.Owner.GetType().Name;

  protected override string GetNameCore() => this.Owner.GetType().Name;

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Custom;
  }

  protected override string GetItemStatusCore()
  {
    if (!(this.Owner is ZoomingToolBarItem))
      return (string) null;
    if (!(this.Owner is ZoomingToolBarItem owner))
      return (string) null;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{owner.IconBackground};");
    stringBuilder.Append($"{owner.ToolBarIconHeight};");
    stringBuilder.Append($"{owner.ToolBarIconWidth};");
    stringBuilder.Append($"{owner.ToolBarIconMargin};");
    return stringBuilder.ToString();
  }
}
