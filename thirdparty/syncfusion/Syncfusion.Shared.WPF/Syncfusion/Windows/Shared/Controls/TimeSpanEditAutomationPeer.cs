// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Controls.TimeSpanEditAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

#nullable disable
namespace Syncfusion.Windows.Shared.Controls;

[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
public class TimeSpanEditAutomationPeer(TimeSpanEdit control) : 
  FrameworkElementAutomationPeer((FrameworkElement) control),
  IValueProvider
{
  private TimeSpanEdit MyOwner => (TimeSpanEdit) this.Owner;

  protected override string GetAutomationIdCore()
  {
    if (!string.IsNullOrEmpty(base.GetAutomationIdCore()))
      return base.GetAutomationIdCore();
    return this.MyOwner != null && !string.IsNullOrEmpty(this.MyOwner.Name) ? this.MyOwner.Name.ToString() : string.Empty;
  }

  protected override string GetClassNameCore() => this.MyOwner.GetType().Name;

  protected override List<AutomationPeer> GetChildrenCore()
  {
    List<AutomationPeer> childrenCore = base.GetChildrenCore();
    if (base.GetChildrenCore() != null)
    {
      foreach (AutomationPeer automationPeer in base.GetChildrenCore())
      {
        if (automationPeer is FrameworkElementAutomationPeer && (automationPeer as FrameworkElementAutomationPeer).Owner.Visibility == Visibility.Visible)
          childrenCore.Add(automationPeer);
      }
    }
    return childrenCore;
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Edit;
  }

  protected override string GetNameCore()
  {
    return string.IsNullOrEmpty(base.GetNameCore()) && this.MyOwner != null && !string.IsNullOrEmpty(this.MyOwner.Name) ? this.MyOwner.Name.ToString() : base.GetNameCore();
  }

  protected override string GetHelpTextCore() => base.GetHelpTextCore();

  void IValueProvider.SetValue(string value) => this.MyOwner.Text = value;

  string IValueProvider.Value => this.MyOwner.Value.ToString();

  bool IValueProvider.IsReadOnly => this.MyOwner.IsReadOnly;
}
