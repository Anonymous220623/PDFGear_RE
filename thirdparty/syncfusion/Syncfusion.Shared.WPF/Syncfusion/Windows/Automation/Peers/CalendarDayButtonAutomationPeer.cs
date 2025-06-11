// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Automation.Peers.CalendarDayButtonAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Controls.Primitives;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Automation.Peers;

public sealed class CalendarDayButtonAutomationPeer(CalendarDayButton owner) : 
  ButtonAutomationPeer((Button) owner),
  ISelectionItemProvider,
  ITableItemProvider,
  IGridItemProvider
{
  private Syncfusion.Windows.Controls.Calendar OwningCalendar => this.OwningCalendarDayButton.Owner;

  private IRawElementProviderSimple OwningCalendarAutomationPeer
  {
    get
    {
      if (this.OwningCalendar != null)
      {
        AutomationPeer peerForElement = UIElementAutomationPeer.CreatePeerForElement((UIElement) this.OwningCalendar);
        if (peerForElement != null)
          return this.ProviderFromPeer(peerForElement);
      }
      return (IRawElementProviderSimple) null;
    }
  }

  private CalendarDayButton OwningCalendarDayButton => this.Owner as CalendarDayButton;

  private DateTime? Date
  {
    get
    {
      return this.OwningCalendarDayButton != null && this.OwningCalendarDayButton.DataContext is DateTime ? (DateTime?) this.OwningCalendarDayButton.DataContext : new DateTime?();
    }
  }

  public override object GetPattern(PatternInterface patternInterface)
  {
    object pattern;
    switch (patternInterface)
    {
      case PatternInterface.GridItem:
      case PatternInterface.SelectionItem:
      case PatternInterface.TableItem:
        pattern = this.OwningCalendar == null || this.OwningCalendarDayButton == null ? base.GetPattern(patternInterface) : (object) this;
        break;
      default:
        pattern = base.GetPattern(patternInterface);
        break;
    }
    return pattern;
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Button;
  }

  protected override string GetClassNameCore() => this.Owner.GetType().Name;

  protected override string GetHelpTextCore()
  {
    if (!this.Date.HasValue)
      return base.GetHelpTextCore();
    string longDateString = Syncfusion.Windows.Controls.DateTimeHelper.ToLongDateString(this.Date, Syncfusion.Windows.Controls.DateTimeHelper.GetCulture((FrameworkElement) this.OwningCalendarDayButton));
    return this.OwningCalendarDayButton.IsBlackedOut ? string.Format((IFormatProvider) this.OwningCalendar.Culture.DateTimeFormat, Syncfusion.Windows.Controls.SR.Get(Syncfusion.Windows.Controls.SRID.CalendarAutomationPeer_BlackoutDayHelpText), (object) longDateString) : longDateString;
  }

  protected override string GetLocalizedControlTypeCore()
  {
    return Syncfusion.Windows.Controls.SR.Get(Syncfusion.Windows.Controls.SRID.CalendarAutomationPeer_DayButtonLocalizedControlType);
  }

  protected override string GetNameCore()
  {
    return !this.Date.HasValue ? base.GetNameCore() : Syncfusion.Windows.Controls.DateTimeHelper.ToLongDateString(this.Date, Syncfusion.Windows.Controls.DateTimeHelper.GetCulture((FrameworkElement) this.OwningCalendarDayButton));
  }

  int IGridItemProvider.Column => (int) this.OwningCalendarDayButton.GetValue(Grid.ColumnProperty);

  int IGridItemProvider.ColumnSpan
  {
    get => (int) this.OwningCalendarDayButton.GetValue(Grid.ColumnSpanProperty);
  }

  IRawElementProviderSimple IGridItemProvider.ContainingGrid => this.OwningCalendarAutomationPeer;

  int IGridItemProvider.Row => (int) this.OwningCalendarDayButton.GetValue(Grid.RowProperty) - 1;

  int IGridItemProvider.RowSpan
  {
    get => (int) this.OwningCalendarDayButton.GetValue(Grid.RowSpanProperty);
  }

  bool ISelectionItemProvider.IsSelected => this.OwningCalendarDayButton.IsSelected;

  IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
  {
    get => this.OwningCalendarAutomationPeer;
  }

  void ISelectionItemProvider.AddToSelection()
  {
    if (((ISelectionItemProvider) this).IsSelected || !this.EnsureSelection() || !(this.OwningCalendarDayButton.DataContext is DateTime))
      return;
    if (this.OwningCalendar.SelectionMode == Syncfusion.Windows.Controls.CalendarSelectionMode.SingleDate)
      this.OwningCalendar.SelectedDate = new DateTime?((DateTime) this.OwningCalendarDayButton.DataContext);
    else
      this.OwningCalendar.SelectedDates.Add((DateTime) this.OwningCalendarDayButton.DataContext);
  }

  void ISelectionItemProvider.RemoveFromSelection()
  {
    if (!((ISelectionItemProvider) this).IsSelected || !(this.OwningCalendarDayButton.DataContext is DateTime))
      return;
    this.OwningCalendar.SelectedDates.Remove((DateTime) this.OwningCalendarDayButton.DataContext);
  }

  void ISelectionItemProvider.Select()
  {
    if (!this.EnsureSelection())
      return;
    this.OwningCalendar.SelectedDates.Clear();
    if (!(this.OwningCalendarDayButton.DataContext is DateTime))
      return;
    this.OwningCalendar.SelectedDates.Add((DateTime) this.OwningCalendarDayButton.DataContext);
  }

  IRawElementProviderSimple[] ITableItemProvider.GetColumnHeaderItems()
  {
    if (this.OwningCalendar != null && this.OwningCalendarAutomationPeer != null)
    {
      IRawElementProviderSimple[] columnHeaders = ((ITableProvider) UIElementAutomationPeer.CreatePeerForElement((UIElement) this.OwningCalendar)).GetColumnHeaders();
      if (columnHeaders != null)
      {
        int column = ((IGridItemProvider) this).Column;
        return new IRawElementProviderSimple[1]
        {
          columnHeaders[column]
        };
      }
    }
    return (IRawElementProviderSimple[]) null;
  }

  IRawElementProviderSimple[] ITableItemProvider.GetRowHeaderItems()
  {
    return (IRawElementProviderSimple[]) null;
  }

  private bool EnsureSelection()
  {
    if (!this.OwningCalendarDayButton.IsEnabled)
      throw new ElementNotEnabledException();
    return !this.OwningCalendarDayButton.IsBlackedOut && this.OwningCalendar.SelectionMode != Syncfusion.Windows.Controls.CalendarSelectionMode.None;
  }
}
