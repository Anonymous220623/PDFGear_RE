// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DayCell
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class DayCell : Cell
{
  public static readonly DependencyProperty IsCurrentMonthProperty = DependencyProperty.Register(nameof (IsCurrentMonth), typeof (bool), typeof (DayCell), (PropertyMetadata) new UIPropertyMetadata((object) true));
  public static readonly DependencyProperty IsTodayProperty = DependencyProperty.Register(nameof (IsToday), typeof (bool), typeof (DayCell), (PropertyMetadata) new UIPropertyMetadata((object) false));
  public static readonly DependencyProperty IsDateProperty = DependencyProperty.Register(nameof (IsDate), typeof (bool), typeof (DayCell), (PropertyMetadata) new UIPropertyMetadata((object) false));
  public static readonly DependencyProperty DateProperty = DependencyProperty.Register(nameof (Date), typeof (Date), typeof (DayCell), (PropertyMetadata) new UIPropertyMetadata((object) new Date()));
  public static readonly RoutedEvent HighlightEvent = EventManager.RegisterRoutedEvent("Highlight", RoutingStrategy.Bubble, typeof (EventHandler), typeof (DayCell));
  public static readonly DependencyProperty IsFirstDayofMonthProperty = DependencyProperty.Register(nameof (IsFirstDayofMonth), typeof (bool), typeof (DayCell), (PropertyMetadata) new UIPropertyMetadata((object) false));

  static DayCell()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (DayCell), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (DayCell)));
  }

  public event RoutedEventHandler Highlight
  {
    add => this.AddHandler(DayCell.HighlightEvent, (Delegate) value);
    remove => this.RemoveHandler(DayCell.HighlightEvent, (Delegate) value);
  }

  public bool IsCurrentMonth
  {
    get => (bool) this.GetValue(DayCell.IsCurrentMonthProperty);
    set => this.SetValue(DayCell.IsCurrentMonthProperty, (object) value);
  }

  public bool IsToday
  {
    get => (bool) this.GetValue(DayCell.IsTodayProperty);
    set => this.SetValue(DayCell.IsTodayProperty, (object) value);
  }

  public bool IsFirstDayofMonth
  {
    get => (bool) this.GetValue(DayCell.IsFirstDayofMonthProperty);
    set => this.SetValue(DayCell.IsFirstDayofMonthProperty, (object) value);
  }

  public bool IsDate
  {
    get => (bool) this.GetValue(DayCell.IsDateProperty);
    set => this.SetValue(DayCell.IsDateProperty, (object) value);
  }

  public Date Date
  {
    get => (Date) this.GetValue(DayCell.DateProperty);
    set => this.SetValue(DayCell.DateProperty, (object) value);
  }

  protected internal void UpdateCellTemplateAndSelector(
    DataTemplate template,
    DataTemplateSelector selector)
  {
    if (selector != null)
      this.ContentTemplateSelector = selector;
    else
      this.ClearValue(ContentControl.ContentTemplateSelectorProperty);
    if (template != null)
      this.ContentTemplate = template;
    else if (selector == null)
      this.ClearValue(ContentControl.ContentTemplateProperty);
    else
      this.ContentTemplate = (DataTemplate) null;
  }

  protected internal void SetTemplate(DataTemplate template)
  {
    if (template == null)
      return;
    this.ClearValue(ContentControl.ContentTemplateSelectorProperty);
    this.ContentTemplate = template;
  }

  protected internal void SetStyle(Style style) => this.Style = style;

  protected internal void FireHighlightEvent()
  {
    this.RaiseEvent(new RoutedEventArgs(DayCell.HighlightEvent, (object) this));
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new CalendarEditCellAutomationPeer((Cell) this);
  }

  internal void Dispose()
  {
    this.Content = (object) null;
    this.ContentTemplate = (DataTemplate) null;
    this.ContentTemplateSelector = (DataTemplateSelector) null;
    this.Style = (Style) null;
    this.Template = (ControlTemplate) null;
  }
}
