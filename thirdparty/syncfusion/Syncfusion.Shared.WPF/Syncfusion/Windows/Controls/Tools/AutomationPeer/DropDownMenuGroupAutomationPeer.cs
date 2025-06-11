// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.Tools.AutomationPeer.DropDownMenuGroupAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Tools.Controls;
using System.Windows;
using System.Windows.Automation.Peers;

#nullable disable
namespace Syncfusion.Windows.Controls.Tools.AutomationPeer;

public class DropDownMenuGroupAutomationPeer(DropDownMenuGroup control) : 
  FrameworkElementAutomationPeer((FrameworkElement) control)
{
  protected override string GetNameCore()
  {
    if (!string.IsNullOrEmpty(base.GetNameCore()))
      return base.GetNameCore();
    return this.Owner is DropDownMenuItem && (this.Owner as DropDownMenuItem).Header != null ? (this.Owner as DropDownMenuItem).Header.ToString() : string.Empty;
  }

  protected override string GetClassNameCore()
  {
    return string.IsNullOrEmpty(base.GetClassNameCore()) ? "DropDownMenuGroup" : base.GetClassNameCore();
  }

  protected override string GetAutomationIdCore()
  {
    if (!string.IsNullOrEmpty(base.GetAutomationIdCore()))
      return base.GetAutomationIdCore();
    return this.MyOwner != null && this.MyOwner.Name != null ? this.MyOwner.Name.ToString() : string.Empty;
  }

  protected override string GetHelpTextCore() => base.GetHelpTextCore();

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Group;
  }

  private DropDownMenuGroup MyOwner => (DropDownMenuGroup) this.Owner;
}
