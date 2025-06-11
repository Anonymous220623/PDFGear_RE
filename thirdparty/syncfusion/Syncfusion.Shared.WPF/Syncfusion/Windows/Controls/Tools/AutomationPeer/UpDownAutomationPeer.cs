// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.Tools.AutomationPeer.UpDownAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

#nullable disable
namespace Syncfusion.Windows.Controls.Tools.AutomationPeer;

[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class UpDownAutomationPeer(UpDown control) : 
  FrameworkElementAutomationPeer((FrameworkElement) control),
  IRangeValueProvider
{
  double IRangeValueProvider.Value => this.MyOwner.Value.Value;

  bool IRangeValueProvider.IsReadOnly => !this.MyOwner.IsEnabled;

  double IRangeValueProvider.Maximum => this.MyOwner.MaxValue;

  double IRangeValueProvider.Minimum => this.MyOwner.MinValue;

  double IRangeValueProvider.LargeChange => this.MyOwner.Step;

  double IRangeValueProvider.SmallChange => this.MyOwner.Step;

  private UpDown MyOwner => (UpDown) this.Owner;

  protected override string GetAutomationIdCore()
  {
    return string.IsNullOrEmpty(base.GetAutomationIdCore()) && this.MyOwner != null && !string.IsNullOrEmpty(this.MyOwner.Name) ? this.MyOwner.Name.ToString() : base.GetAutomationIdCore();
  }

  protected override List<System.Windows.Automation.Peers.AutomationPeer> GetChildrenCore()
  {
    List<System.Windows.Automation.Peers.AutomationPeer> childrenCore = new List<System.Windows.Automation.Peers.AutomationPeer>();
    if (base.GetChildrenCore() != null)
    {
      foreach (System.Windows.Automation.Peers.AutomationPeer automationPeer in base.GetChildrenCore())
      {
        if (automationPeer is FrameworkElementAutomationPeer && (automationPeer as FrameworkElementAutomationPeer).Owner.Visibility == Visibility.Visible)
          childrenCore.Add(automationPeer);
      }
    }
    return childrenCore;
  }

  protected override string GetClassNameCore() => this.MyOwner.GetType().Name;

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Spinner;
  }

  protected override string GetNameCore()
  {
    return string.IsNullOrEmpty(base.GetNameCore()) && this.MyOwner != null && !string.IsNullOrEmpty(this.MyOwner.Name) ? this.MyOwner.Name.ToString() : base.GetNameCore();
  }

  protected override string GetHelpTextCore()
  {
    return string.IsNullOrEmpty(base.GetHelpTextCore()) && this.MyOwner.ToolTip != null && !string.IsNullOrEmpty(this.MyOwner.ToolTip.ToString()) ? this.MyOwner.ToolTip.ToString() : base.GetHelpTextCore();
  }

  public override object GetPattern(PatternInterface patternInterface)
  {
    return patternInterface == PatternInterface.RangeValue ? (object) this : base.GetPattern(patternInterface);
  }

  public void SetValue(double value) => this.MyOwner.Value = new double?(value);
}
