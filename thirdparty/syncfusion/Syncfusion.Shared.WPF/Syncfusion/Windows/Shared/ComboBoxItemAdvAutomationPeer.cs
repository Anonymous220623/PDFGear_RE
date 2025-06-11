// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ComboBoxItemAdvAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Tools.Controls;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;

#nullable disable
namespace Syncfusion.Windows.Shared;

[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
public class ComboBoxItemAdvAutomationPeer(ComboBoxItemAdv control) : FrameworkElementAutomationPeer((FrameworkElement) control)
{
  protected override string GetNameCore() => "ComboBoxItemAdv";

  protected override string GetClassNameCore()
  {
    return string.IsNullOrEmpty(base.GetClassNameCore()) ? "ComboBoxItemAdv" : base.GetClassNameCore();
  }

  protected override string GetAutomationIdCore()
  {
    if (!string.IsNullOrEmpty(base.GetAutomationIdCore()))
      return base.GetAutomationIdCore();
    return string.IsNullOrEmpty(this.MyOwner.Name) ? "ComboBoxItemAdvID" : this.MyOwner.Name.ToString();
  }

  protected override string GetHelpTextCore()
  {
    return string.IsNullOrEmpty(base.GetHelpTextCore()) ? "ComboBoxItemAdv" : base.GetHelpTextCore();
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Custom;
  }

  private ComboBoxItemAdv MyOwner => (ComboBoxItemAdv) this.Owner;
}
