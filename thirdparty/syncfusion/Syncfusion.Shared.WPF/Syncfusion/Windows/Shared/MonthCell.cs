// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.MonthCell
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using System;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class MonthCell : Cell
{
  public static readonly DependencyProperty MonthNumberProperty = DependencyProperty.Register(nameof (MonthNumber), typeof (int), typeof (MonthCell));

  public MonthCell() => this.DefaultStyleKey = (object) typeof (MonthCell);

  public int MonthNumber
  {
    get => (int) this.GetValue(MonthCell.MonthNumberProperty);
    set
    {
      if ((value < 1 || value > 12) && value != -1)
        throw new ArgumentException("MonthNumber must be in the range 1..12");
      this.SetValue(MonthCell.MonthNumberProperty, (object) value);
    }
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new CalendarEditCellAutomationPeer((Cell) this);
  }
}
