// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TileViewItemCloseButtonAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;
using System.Windows.Automation.Peers;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class TileViewItemCloseButtonAutomationPeer(TileViewItemCloseButton control) : 
  FrameworkElementAutomationPeer((FrameworkElement) control)
{
  protected override string GetNameCore()
  {
    return string.IsNullOrEmpty(base.GetNameCore()) ? "TileViewItemCloseButton" : base.GetNameCore();
  }

  protected override string GetAutomationIdCore()
  {
    if (!string.IsNullOrEmpty(base.GetAutomationIdCore()))
      return base.GetAutomationIdCore();
    return string.IsNullOrEmpty(this.MyOwner.Name) ? "TileViewItemCloseButtonID" : this.MyOwner.Name.ToString();
  }

  protected override string GetClassNameCore()
  {
    return string.IsNullOrEmpty(base.GetClassNameCore()) ? "TileViewItemCloseButton" : base.GetClassNameCore();
  }

  protected override string GetHelpTextCore()
  {
    return string.IsNullOrEmpty(base.GetHelpTextCore()) ? "TileViewItemCloseButton" : base.GetHelpTextCore();
  }

  public override object GetPattern(PatternInterface patternInterface)
  {
    return patternInterface == PatternInterface.Invoke ? (object) this : base.GetPattern(patternInterface);
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Button;
  }

  private TileViewItemCloseButton MyOwner => (TileViewItemCloseButton) this.Owner;
}
