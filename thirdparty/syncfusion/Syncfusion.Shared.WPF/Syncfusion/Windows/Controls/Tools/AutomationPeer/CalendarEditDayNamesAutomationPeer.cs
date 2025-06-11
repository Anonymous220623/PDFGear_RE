// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.Tools.AutomationPeer.CalendarEditDayNamesAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System.Windows;
using System.Windows.Automation.Peers;

#nullable disable
namespace Syncfusion.Windows.Controls.Tools.AutomationPeer;

public class CalendarEditDayNamesAutomationPeer(DayNameCell owner) : FrameworkElementAutomationPeer((FrameworkElement) owner)
{
  private DayNameCell MyOwner => this.Owner as DayNameCell;

  protected override string GetAutomationIdCore()
  {
    return string.IsNullOrEmpty(base.GetAutomationIdCore()) && this.MyOwner != null && this.MyOwner.Name != null ? this.MyOwner.Name.ToString() : base.GetAutomationIdCore();
  }

  protected override string GetClassNameCore()
  {
    return string.IsNullOrEmpty(base.GetClassNameCore()) ? this.Owner.GetType().ToString() : base.GetClassNameCore();
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Text;
  }

  protected override string GetNameCore()
  {
    return string.IsNullOrEmpty(base.GetNameCore()) && this.Owner is DayNameCell ? (this.Owner as DayNameCell).Name : base.GetNameCore();
  }
}
