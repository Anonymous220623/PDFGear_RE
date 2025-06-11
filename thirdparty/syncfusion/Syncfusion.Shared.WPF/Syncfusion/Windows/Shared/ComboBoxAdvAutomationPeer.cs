// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ComboBoxAdvAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Tools.Controls;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;

#nullable disable
namespace Syncfusion.Windows.Shared;

[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
public class ComboBoxAdvAutomationPeer(ComboBoxAdv control) : SelectorAutomationPeer((Selector) control)
{
  protected override string GetAutomationIdCore()
  {
    if (!string.IsNullOrEmpty(base.GetAutomationIdCore()))
      return base.GetAutomationIdCore();
    return string.IsNullOrEmpty(this.MyOwner.Name) ? "ComboBoxAdvID" : this.MyOwner.Name.ToString();
  }

  protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
  {
    return (ItemAutomationPeer) new ComboBoxItemAdvDataAutomationPeer(item, (ItemsControlAutomationPeer) this);
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Custom;
  }

  protected override string GetNameCore() => "ComboBoxAdv";

  protected override string GetClassNameCore() => "ComboBoxAdv";

  private ComboBoxAdv MyOwner => (ComboBoxAdv) this.Owner;
}
