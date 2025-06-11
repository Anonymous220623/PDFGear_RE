// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TileViewItemAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;

#nullable disable
namespace Syncfusion.Windows.Shared;

[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class TileViewItemAutomationPeer(TileViewItem control) : FrameworkElementAutomationPeer((FrameworkElement) control)
{
  protected override string GetNameCore()
  {
    if (!string.IsNullOrEmpty(base.GetNameCore()))
      return base.GetNameCore();
    return this.Owner is TileViewItem && (this.Owner as TileViewItem).Header != null ? (this.Owner as TileViewItem).Header.ToString() : "TileViewItem";
  }

  protected override string GetClassNameCore()
  {
    return string.IsNullOrEmpty(base.GetClassNameCore()) ? "TileViewItem" : base.GetClassNameCore();
  }

  protected override string GetAutomationIdCore()
  {
    if (!string.IsNullOrEmpty(base.GetAutomationIdCore()))
      return base.GetAutomationIdCore();
    return string.IsNullOrEmpty(this.MyOwner.Name) ? "TileViewItemID" : this.MyOwner.Name.ToString();
  }

  protected override string GetHelpTextCore()
  {
    return string.IsNullOrEmpty(base.GetHelpTextCore()) ? "TileViewItem" : base.GetHelpTextCore();
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Custom;
  }

  private TileViewItem MyOwner => (TileViewItem) this.Owner;

  protected override string GetItemStatusCore()
  {
    if (!(this.Owner is TileViewItem))
      return (string) null;
    if (!(this.Owner is TileViewItem owner))
      return (string) null;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{owner.BorderRow}#");
    stringBuilder.Append($"{owner.BorderColumn}#");
    if (owner.MinimizedHeader != null)
      stringBuilder.Append($"{owner.MinimizedHeader}#");
    else
      stringBuilder.Append($"{"null"}#");
    if (owner.MaximizedHeader != null)
      stringBuilder.Append($"{owner.MaximizedHeader}#");
    else
      stringBuilder.Append($"{"null"}#");
    stringBuilder.Append($"{owner.CloseButtonVisibility}#");
    stringBuilder.Append($"{owner.CloseMode}#");
    stringBuilder.Append($"{owner.HeaderVisibility}#");
    stringBuilder.Append($"{owner.IsSelected}#");
    stringBuilder.Append($"{owner.MinMaxButtonVisibility}#");
    stringBuilder.Append($"{owner.MinMaxButtonToolTip}#");
    stringBuilder.Append($"{owner.ShareSpace}#");
    stringBuilder.Append($"{owner.TileViewItemState}#");
    return stringBuilder.ToString();
  }
}
