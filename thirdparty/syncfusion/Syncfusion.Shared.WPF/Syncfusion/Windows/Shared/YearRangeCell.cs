// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.YearRangeCell
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class YearRangeCell : Cell
{
  public static readonly DependencyProperty YearsProperty = DependencyProperty.Register(nameof (Years), typeof (YearsRange), typeof (YearRangeCell));
  public static readonly DependencyProperty IsBelongToCurrentRangeProperty = DependencyProperty.Register(nameof (IsBelongToCurrentRange), typeof (bool), typeof (YearRangeCell));

  static YearRangeCell()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (YearRangeCell), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (YearRangeCell)));
  }

  public YearsRange Years
  {
    get => (YearsRange) this.GetValue(YearRangeCell.YearsProperty);
    set => this.SetValue(YearRangeCell.YearsProperty, (object) value);
  }

  public bool IsBelongToCurrentRange
  {
    get => (bool) this.GetValue(YearRangeCell.IsBelongToCurrentRangeProperty);
    set => this.SetValue(YearRangeCell.IsBelongToCurrentRangeProperty, (object) value);
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new CalendarEditCellAutomationPeer((Cell) this);
  }
}
