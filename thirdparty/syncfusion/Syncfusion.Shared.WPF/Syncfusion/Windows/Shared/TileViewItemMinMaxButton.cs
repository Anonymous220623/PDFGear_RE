// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TileViewItemMinMaxButton
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class TileViewItemMinMaxButton : ToggleButton
{
  public TileViewItemMinMaxButton() => this.DefaultStyleKey = (object) typeof (ToggleButton);

  protected override AutomationPeer OnCreateAutomationPeer()
  {
    return (AutomationPeer) new TileViewItemMinMaxButtonAutomationPeer(this);
  }
}
