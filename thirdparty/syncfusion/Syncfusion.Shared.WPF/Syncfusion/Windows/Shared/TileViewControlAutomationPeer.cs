// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TileViewControlAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;

#nullable disable
namespace Syncfusion.Windows.Shared;

[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
public class TileViewControlAutomationPeer(TileViewControl control) : FrameworkElementAutomationPeer((FrameworkElement) control)
{
  protected override string GetAutomationIdCore()
  {
    if (!string.IsNullOrEmpty(base.GetAutomationIdCore()))
      return base.GetAutomationIdCore();
    return string.IsNullOrEmpty(this.MyOwner.Name) ? "TileViewControlID" : this.MyOwner.Name.ToString();
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Custom;
  }

  protected override string GetNameCore() => "TileViewControl";

  protected override string GetClassNameCore() => "TileViewControl";

  private TileViewControl MyOwner => (TileViewControl) this.Owner;

  protected override string GetItemStatusCore()
  {
    base.GetItemStatusCore();
    if (!(this.Owner is TileViewControl))
      return (string) null;
    if (!(this.Owner is TileViewControl owner))
      return (string) null;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{owner.RowCount}#");
    stringBuilder.Append($"{owner.ColumnCount}#");
    stringBuilder.Append($"{owner.SplitterThickness}#");
    stringBuilder.Append($"{owner.MinimizedItemsPercentage}#");
    stringBuilder.Append($"{owner.AllowItemRepositioning.ToString()}#");
    stringBuilder.Append($"{owner.ClickHeaderToMaximize.ToString()}#");
    stringBuilder.Append($"{owner.UseNormalState.ToString()}#");
    stringBuilder.Append($"{owner.IsMinMaxButtonOnMouseOverOnly.ToString()}#");
    stringBuilder.Append($"{owner.MinimizedItemsOrientation.ToString()}#");
    stringBuilder.Append($"{owner.SplitterVisibility.ToString()}#");
    stringBuilder.Append($"{owner.SplitterColor.ToString()}#");
    return stringBuilder.ToString();
  }
}
