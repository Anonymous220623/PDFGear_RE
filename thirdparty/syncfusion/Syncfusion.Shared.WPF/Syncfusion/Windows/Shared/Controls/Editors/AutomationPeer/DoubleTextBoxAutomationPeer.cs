// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Controls.Editors.AutomationPeer.DoubleTextBoxAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared.Controls.Editors.AutomationPeer;

[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class DoubleTextBoxAutomationPeer(DoubleTextBox owner) : TextBoxAutomationPeer((TextBox) owner)
{
  private DoubleTextBox MyOwner => (DoubleTextBox) this.Owner;

  protected override string GetAutomationIdCore()
  {
    return string.IsNullOrEmpty(base.GetAutomationIdCore()) && this.MyOwner != null && !string.IsNullOrEmpty(this.MyOwner.Name) ? this.MyOwner.Name.ToString() : base.GetAutomationIdCore();
  }

  protected override string GetClassNameCore() => this.MyOwner.GetType().Name;

  protected override string GetNameCore()
  {
    return string.IsNullOrEmpty(base.GetNameCore()) && this.MyOwner != null && !string.IsNullOrEmpty(this.MyOwner.Name) ? this.MyOwner.Name.ToString() : base.GetNameCore();
  }

  protected override string GetHelpTextCore()
  {
    if (string.IsNullOrEmpty(base.GetHelpTextCore()))
    {
      if (this.MyOwner.ToolTip != null && !string.IsNullOrEmpty(this.MyOwner.ToolTip.ToString()))
        return this.MyOwner.ToolTip.ToString();
      if (!string.IsNullOrEmpty(this.MyOwner.WatermarkText))
        return this.MyOwner.WatermarkText;
    }
    return base.GetHelpTextCore();
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Edit;
  }

  protected override List<System.Windows.Automation.Peers.AutomationPeer> GetChildrenCore()
  {
    return (List<System.Windows.Automation.Peers.AutomationPeer>) null;
  }
}
