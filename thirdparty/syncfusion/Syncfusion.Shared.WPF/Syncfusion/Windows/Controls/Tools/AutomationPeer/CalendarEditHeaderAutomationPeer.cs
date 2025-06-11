// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.Tools.AutomationPeer.CalendarEditHeaderAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System.Windows;
using System.Windows.Automation.Peers;

#nullable disable
namespace Syncfusion.Windows.Controls.Tools.AutomationPeer;

public class CalendarEditHeaderAutomationPeer(MonthButton owner) : FrameworkElementAutomationPeer((FrameworkElement) owner)
{
  private MonthButton MyOwner => this.Owner as MonthButton;

  protected override string GetAutomationIdCore()
  {
    if (!string.IsNullOrEmpty(base.GetAutomationIdCore()))
      return base.GetAutomationIdCore();
    return this.MyOwner != null && this.MyOwner.Name != null ? this.MyOwner.Name.ToString() : string.Empty;
  }

  protected override string GetClassNameCore()
  {
    return string.IsNullOrEmpty(base.GetClassNameCore()) ? this.Owner.GetType().ToString() : base.GetClassNameCore();
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Button;
  }

  protected override string GetNameCore()
  {
    return string.IsNullOrEmpty(base.GetNameCore()) && this.Owner is MonthButton ? (this.Owner as MonthButton).Name : base.GetNameCore();
  }
}
