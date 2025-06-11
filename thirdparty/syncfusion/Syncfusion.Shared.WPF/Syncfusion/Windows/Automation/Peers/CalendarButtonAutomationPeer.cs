// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Automation.Peers.CalendarButtonAutomationPeer
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
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Automation.Peers;

public sealed class CalendarButtonAutomationPeer(CalendarButton owner) : 
  ButtonAutomationPeer((Button) owner),
  IGridItemProvider,
  ISelectionItemProvider
{
  private Syncfusion.Windows.Controls.Calendar OwningCalendar => this.OwningCalendarButton.Owner;

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

  private CalendarButton OwningCalendarButton => this.Owner as CalendarButton;

  private DateTime? Date
  {
    get
    {
      return this.OwningCalendarButton != null && this.OwningCalendarButton.DataContext is DateTime ? (DateTime?) this.OwningCalendarButton.DataContext : new DateTime?();
    }
  }

  public override object GetPattern(PatternInterface patternInterface)
  {
    object pattern;
    switch (patternInterface)
    {
      case PatternInterface.GridItem:
      case PatternInterface.SelectionItem:
        pattern = this.OwningCalendar == null || this.OwningCalendar.MonthControl == null || this.OwningCalendarButton == null ? base.GetPattern(patternInterface) : (object) this;
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

  protected override string GetLocalizedControlTypeCore()
  {
    return Syncfusion.Windows.Controls.SR.Get(Syncfusion.Windows.Controls.SRID.CalendarAutomationPeer_CalendarButtonLocalizedControlType);
  }

  protected override string GetHelpTextCore()
  {
    DateTime? date = this.Date;
    return !date.HasValue ? base.GetHelpTextCore() : Syncfusion.Windows.Controls.DateTimeHelper.ToLongDateString(date, Syncfusion.Windows.Controls.DateTimeHelper.GetCulture((FrameworkElement) this.OwningCalendarButton));
  }

  protected override string GetNameCore()
  {
    DateTime? date = this.Date;
    if (!date.HasValue)
      return base.GetNameCore();
    return this.OwningCalendar.DisplayMode == Syncfusion.Windows.Controls.CalendarMode.Decade ? Syncfusion.Windows.Controls.DateTimeHelper.ToYearString(date, this.OwningCalendar.Culture, this.OwningCalendar.FormatCalendar) : Syncfusion.Windows.Controls.DateTimeHelper.ToYearMonthPatternString(date, Syncfusion.Windows.Controls.DateTimeHelper.GetCulture((FrameworkElement) this.OwningCalendarButton), this.OwningCalendar.AbbreviatedMonthNames, this.OwningCalendar.FormatCalendar);
  }

  int IGridItemProvider.Column => (int) this.OwningCalendarButton.GetValue(Grid.ColumnProperty);

  int IGridItemProvider.ColumnSpan
  {
    get => (int) this.OwningCalendarButton.GetValue(Grid.ColumnSpanProperty);
  }

  IRawElementProviderSimple IGridItemProvider.ContainingGrid => this.OwningCalendarAutomationPeer;

  int IGridItemProvider.Row => (int) this.OwningCalendarButton.GetValue(Grid.RowSpanProperty);

  int IGridItemProvider.RowSpan => 1;

  bool ISelectionItemProvider.IsSelected => this.OwningCalendarButton.IsFocused;

  IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
  {
    get => this.OwningCalendarAutomationPeer;
  }

  void ISelectionItemProvider.AddToSelection()
  {
  }

  void ISelectionItemProvider.RemoveFromSelection()
  {
  }

  void ISelectionItemProvider.Select()
  {
    if (!this.OwningCalendarButton.IsEnabled)
      throw new ElementNotEnabledException();
    this.OwningCalendarButton.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
  }
}
