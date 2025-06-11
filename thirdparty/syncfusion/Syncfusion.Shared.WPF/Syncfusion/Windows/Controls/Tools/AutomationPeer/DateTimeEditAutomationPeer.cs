// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.Tools.AutomationPeer.DateTimeEditAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

#nullable disable
namespace Syncfusion.Windows.Controls.Tools.AutomationPeer;

[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class DateTimeEditAutomationPeer(DateTimeEdit control) : 
  FrameworkElementAutomationPeer((FrameworkElement) control),
  IValueProvider,
  IExpandCollapseProvider
{
  public override object GetPattern(PatternInterface patternInterface)
  {
    return patternInterface == PatternInterface.ExpandCollapse || patternInterface == PatternInterface.Value ? (object) this : base.GetPattern(patternInterface);
  }

  protected override string GetAutomationIdCore()
  {
    if (!string.IsNullOrEmpty(base.GetAutomationIdCore()))
      return base.GetAutomationIdCore();
    return this.MyOwner != null && this.MyOwner.Name != null ? this.MyOwner.Name.ToString() : string.Empty;
  }

  protected override string GetClassNameCore() => this.MyOwner.GetType().Name;

  protected override List<System.Windows.Automation.Peers.AutomationPeer> GetChildrenCore()
  {
    List<System.Windows.Automation.Peers.AutomationPeer> childrenCore = base.GetChildrenCore();
    if (this.MyOwner.IsDropDownOpen && this.MyOwner.DateTimeCalender != null && UIElementAutomationPeer.CreatePeerForElement((UIElement) this.MyOwner.DateTimeCalender) is Syncfusion.Windows.Automation.Peers.CalendarAutomationPeer peerForElement)
      childrenCore.Add((System.Windows.Automation.Peers.AutomationPeer) peerForElement);
    return childrenCore;
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Edit;
  }

  protected override string GetNameCore()
  {
    return string.IsNullOrEmpty(base.GetNameCore()) && this.MyOwner != null && this.MyOwner.Name != null ? this.MyOwner.Name.ToString() : base.GetNameCore();
  }

  protected override string GetHelpTextCore() => base.GetHelpTextCore();

  protected override string GetLocalizedControlTypeCore()
  {
    return Syncfusion.Windows.Controls.SR.Get(Syncfusion.Windows.Controls.SRID.DateTimeEditAutomationPeer_DateTimeEditLocalizedControlType);
  }

  public ExpandCollapseState ExpandCollapseState
  {
    get
    {
      return this.MyOwner.IsDropDownOpen ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed;
    }
  }

  void IExpandCollapseProvider.Collapse() => this.MyOwner.IsDropDownOpen = false;

  void IExpandCollapseProvider.Expand() => this.MyOwner.IsDropDownOpen = true;

  private DateTimeEdit MyOwner => (DateTimeEdit) this.Owner;

  public string Value => this.MyOwner.Text;

  public bool IsReadOnly => this.MyOwner.IsReadOnly;

  void IValueProvider.SetValue(string value) => this.MyOwner.Text = value;
}
