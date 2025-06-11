// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.YearCell
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class YearCell : Cell
{
  public static readonly DependencyProperty YearProperty = DependencyProperty.Register(nameof (Year), typeof (int), typeof (YearCell));
  public static readonly DependencyProperty IsBelongToCurrentRangeProperty = DependencyProperty.Register(nameof (IsBelongToCurrentRange), typeof (bool), typeof (YearCell));

  static YearCell()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (YearCell), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (YearCell)));
  }

  public int Year
  {
    get => (int) this.GetValue(YearCell.YearProperty);
    set => this.SetValue(YearCell.YearProperty, (object) value);
  }

  public bool IsBelongToCurrentRange
  {
    get => (bool) this.GetValue(YearCell.IsBelongToCurrentRangeProperty);
    set => this.SetValue(YearCell.IsBelongToCurrentRangeProperty, (object) value);
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new CalendarEditCellAutomationPeer((Cell) this);
  }
}
