// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Automation.Peers.CalendarAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Controls.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Automation.Peers;

public sealed class CalendarAutomationPeer(Syncfusion.Windows.Controls.Calendar owner) : 
  FrameworkElementAutomationPeer((FrameworkElement) owner),
  IMultipleViewProvider,
  ISelectionProvider,
  ITableProvider,
  IGridProvider
{
  private Dictionary<CalendarAutomationPeer.DateTimeCalendarModePair, CalendarChildrenAutomationPeer> calendarChildren = new Dictionary<CalendarAutomationPeer.DateTimeCalendarModePair, CalendarChildrenAutomationPeer>();
  private List<AutomationPeer> peers;

  private Syncfusion.Windows.Controls.Calendar OwningCalendar => this.Owner as Syncfusion.Windows.Controls.Calendar;

  private Grid OwningGrid
  {
    get
    {
      if (this.OwningCalendar == null || this.OwningCalendar.MonthControl == null)
        return (Grid) null;
      return this.OwningCalendar.DisplayMode == Syncfusion.Windows.Controls.CalendarMode.Month ? this.OwningCalendar.MonthControl.MonthView : this.OwningCalendar.MonthControl.YearView;
    }
  }

  private Dictionary<CalendarAutomationPeer.DateTimeCalendarModePair, CalendarChildrenAutomationPeer> CalendarChildrenPeers
  {
    get => this.calendarChildren;
    set => this.calendarChildren = value;
  }

  public override object GetPattern(PatternInterface patternInterface)
  {
    switch (patternInterface)
    {
      case PatternInterface.Selection:
      case PatternInterface.Grid:
      case PatternInterface.MultipleView:
      case PatternInterface.Table:
        if (this.OwningGrid != null)
          return (object) this;
        break;
    }
    return base.GetPattern(patternInterface);
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Calendar;
  }

  protected override string GetClassNameCore() => this.Owner.GetType().Name;

  protected override List<AutomationPeer> GetChildrenCore()
  {
    Dictionary<CalendarAutomationPeer.DateTimeCalendarModePair, CalendarChildrenAutomationPeer> dictionary = new Dictionary<CalendarAutomationPeer.DateTimeCalendarModePair, CalendarChildrenAutomationPeer>();
    if (this.OwningCalendar.MonthControl == null)
      return (List<AutomationPeer>) null;
    if (this.peers != null)
    {
      this.peers.Clear();
      this.peers = (List<AutomationPeer>) null;
    }
    this.peers = new List<AutomationPeer>();
    AutomationPeer peerForElement1 = UIElementAutomationPeer.CreatePeerForElement((UIElement) this.OwningCalendar.MonthControl.PreviousButton);
    if (peerForElement1 != null)
      this.peers.Add(peerForElement1);
    AutomationPeer peerForElement2 = UIElementAutomationPeer.CreatePeerForElement((UIElement) this.OwningCalendar.MonthControl.HeaderButton);
    if (peerForElement2 != null)
      this.peers.Add(peerForElement2);
    AutomationPeer peerForElement3 = UIElementAutomationPeer.CreatePeerForElement((UIElement) this.OwningCalendar.MonthControl.NextButton);
    if (peerForElement3 != null)
      this.peers.Add(peerForElement3);
    foreach (UIElement child in this.OwningGrid.Children)
    {
      int num = (int) child.GetValue(Grid.RowProperty);
      if (this.OwningCalendar.DisplayMode == Syncfusion.Windows.Controls.CalendarMode.Month && num == 0)
      {
        AutomationPeer peerForElement4 = UIElementAutomationPeer.CreatePeerForElement(child);
        if (peerForElement4 != null)
          this.peers.Add(peerForElement4);
      }
      else if (child is Button && child is Button button && button.DataContext is DateTime)
      {
        DateTime dataContext = (DateTime) button.DataContext;
        CalendarChildrenAutomationPeer childrenAutomationPeer = this.GetCalendarChildrenAutomationPeer(dataContext, this.OwningCalendar.DisplayMode);
        this.peers.Add((AutomationPeer) childrenAutomationPeer);
        CalendarAutomationPeer.DateTimeCalendarModePair key = new CalendarAutomationPeer.DateTimeCalendarModePair(dataContext, this.OwningCalendar.DisplayMode);
        dictionary.Add(key, childrenAutomationPeer);
      }
    }
    if (this.CalendarChildrenPeers != null)
    {
      this.CalendarChildrenPeers.Clear();
      this.CalendarChildrenPeers = (Dictionary<CalendarAutomationPeer.DateTimeCalendarModePair, CalendarChildrenAutomationPeer>) null;
    }
    this.CalendarChildrenPeers = dictionary;
    return this.peers;
  }

  private CalendarChildrenAutomationPeer GetCalendarChildrenAutomationPeer(
    DateTime date,
    Syncfusion.Windows.Controls.CalendarMode displayMode)
  {
    CalendarAutomationPeer.DateTimeCalendarModePair key = new CalendarAutomationPeer.DateTimeCalendarModePair(date, displayMode);
    CalendarChildrenAutomationPeer childrenAutomationPeer = (CalendarChildrenAutomationPeer) null;
    if (this.CalendarChildrenPeers != null)
      this.CalendarChildrenPeers.TryGetValue(key, out childrenAutomationPeer);
    if (childrenAutomationPeer == null)
      childrenAutomationPeer = new CalendarChildrenAutomationPeer(date, this.OwningCalendar, displayMode);
    return childrenAutomationPeer;
  }

  internal void RaiseSelectionEvents(SelectionChangedEventArgs e)
  {
    int count1 = this.OwningCalendar.SelectedDates.Count;
    int count2 = e.AddedItems.Count;
    if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) && count1 == 1 && count2 == 1)
    {
      CalendarDayButton dayButtonFromDay = this.OwningCalendar.FindDayButtonFromDay((DateTime) e.AddedItems[0]);
      if (dayButtonFromDay == null)
        return;
      UIElementAutomationPeer.FromElement((UIElement) dayButtonFromDay)?.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
    }
    else
    {
      if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection))
      {
        foreach (DateTime addedItem in (IEnumerable) e.AddedItems)
        {
          CalendarDayButton dayButtonFromDay = this.OwningCalendar.FindDayButtonFromDay(addedItem);
          if (dayButtonFromDay != null)
            UIElementAutomationPeer.FromElement((UIElement) dayButtonFromDay)?.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementAddedToSelection);
        }
      }
      if (!AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
        return;
      foreach (DateTime removedItem in (IEnumerable) e.RemovedItems)
      {
        CalendarDayButton dayButtonFromDay = this.OwningCalendar.FindDayButtonFromDay(removedItem);
        if (dayButtonFromDay != null)
          UIElementAutomationPeer.FromElement((UIElement) dayButtonFromDay)?.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
      }
    }
  }

  int IGridProvider.ColumnCount
  {
    get => this.OwningGrid != null ? this.OwningGrid.ColumnDefinitions.Count : 0;
  }

  int IGridProvider.RowCount
  {
    get
    {
      if (this.OwningGrid == null)
        return 0;
      return this.OwningCalendar.DisplayMode == Syncfusion.Windows.Controls.CalendarMode.Month ? Math.Max(0, this.OwningGrid.RowDefinitions.Count - 1) : this.OwningGrid.RowDefinitions.Count;
    }
  }

  IRawElementProviderSimple IGridProvider.GetItem(int row, int column)
  {
    if (this.OwningCalendar.DisplayMode == Syncfusion.Windows.Controls.CalendarMode.Month)
      ++row;
    if (this.OwningGrid != null && row >= 0 && row < this.OwningGrid.RowDefinitions.Count && column >= 0 && column < this.OwningGrid.ColumnDefinitions.Count)
    {
      foreach (UIElement child in this.OwningGrid.Children)
      {
        int num1 = (int) child.GetValue(Grid.RowProperty);
        int num2 = (int) child.GetValue(Grid.ColumnProperty);
        if (num1 == row && num2 == column)
        {
          AutomationPeer peerForElement = UIElementAutomationPeer.CreatePeerForElement(child);
          if (peerForElement != null)
            return this.ProviderFromPeer(peerForElement);
        }
      }
    }
    return (IRawElementProviderSimple) null;
  }

  int IMultipleViewProvider.CurrentView => (int) this.OwningCalendar.DisplayMode;

  int[] IMultipleViewProvider.GetSupportedViews()
  {
    return new int[3]{ 0, 1, 2 };
  }

  string IMultipleViewProvider.GetViewName(int viewId)
  {
    switch (viewId)
    {
      case 0:
        return Syncfusion.Windows.Controls.SR.Get(Syncfusion.Windows.Controls.SRID.CalendarAutomationPeer_MonthMode);
      case 1:
        return Syncfusion.Windows.Controls.SR.Get(Syncfusion.Windows.Controls.SRID.CalendarAutomationPeer_YearMode);
      case 2:
        return Syncfusion.Windows.Controls.SR.Get(Syncfusion.Windows.Controls.SRID.CalendarAutomationPeer_DecadeMode);
      default:
        return string.Empty;
    }
  }

  void IMultipleViewProvider.SetCurrentView(int viewId)
  {
    this.OwningCalendar.DisplayMode = (Syncfusion.Windows.Controls.CalendarMode) viewId;
  }

  bool ISelectionProvider.CanSelectMultiple
  {
    get
    {
      return this.OwningCalendar.SelectionMode == Syncfusion.Windows.Controls.CalendarSelectionMode.SingleRange || this.OwningCalendar.SelectionMode == Syncfusion.Windows.Controls.CalendarSelectionMode.MultipleRange;
    }
  }

  bool ISelectionProvider.IsSelectionRequired => false;

  IRawElementProviderSimple[] ISelectionProvider.GetSelection()
  {
    List<IRawElementProviderSimple> elementProviderSimpleList = new List<IRawElementProviderSimple>();
    if (this.OwningGrid != null)
    {
      if (this.OwningCalendar.DisplayMode == Syncfusion.Windows.Controls.CalendarMode.Month && this.OwningCalendar.SelectedDates != null && this.OwningCalendar.SelectedDates.Count != 0)
      {
        foreach (UIElement child in this.OwningGrid.Children)
        {
          if ((int) child.GetValue(Grid.RowProperty) != 0 && child is CalendarDayButton element && element.IsSelected)
          {
            AutomationPeer peerForElement = UIElementAutomationPeer.CreatePeerForElement((UIElement) element);
            if (peerForElement != null)
              elementProviderSimpleList.Add(this.ProviderFromPeer(peerForElement));
          }
        }
      }
      else
      {
        foreach (UIElement child in this.OwningGrid.Children)
        {
          if (child is CalendarButton element && element.IsFocused)
          {
            AutomationPeer peerForElement = UIElementAutomationPeer.CreatePeerForElement((UIElement) element);
            if (peerForElement != null)
            {
              elementProviderSimpleList.Add(this.ProviderFromPeer(peerForElement));
              break;
            }
            break;
          }
        }
      }
      if (elementProviderSimpleList.Count > 0)
        return elementProviderSimpleList.ToArray();
    }
    return (IRawElementProviderSimple[]) null;
  }

  RowOrColumnMajor ITableProvider.RowOrColumnMajor => RowOrColumnMajor.RowMajor;

  IRawElementProviderSimple[] ITableProvider.GetColumnHeaders()
  {
    if (this.OwningCalendar.DisplayMode == Syncfusion.Windows.Controls.CalendarMode.Month)
    {
      List<IRawElementProviderSimple> elementProviderSimpleList = new List<IRawElementProviderSimple>();
      foreach (UIElement child in this.OwningGrid.Children)
      {
        if ((int) child.GetValue(Grid.RowProperty) == 0)
        {
          AutomationPeer peerForElement = UIElementAutomationPeer.CreatePeerForElement(child);
          if (peerForElement != null)
            elementProviderSimpleList.Add(this.ProviderFromPeer(peerForElement));
        }
      }
      if (elementProviderSimpleList.Count > 0)
        return elementProviderSimpleList.ToArray();
    }
    return (IRawElementProviderSimple[]) null;
  }

  IRawElementProviderSimple[] ITableProvider.GetRowHeaders() => (IRawElementProviderSimple[]) null;

  internal void Dispose()
  {
    if (this.calendarChildren != null)
    {
      foreach (KeyValuePair<CalendarAutomationPeer.DateTimeCalendarModePair, CalendarChildrenAutomationPeer> calendarChild in this.calendarChildren)
        calendarChild.Value.OwningCalendar = (Syncfusion.Windows.Controls.Calendar) null;
      this.calendarChildren.Clear();
      this.calendarChildren = (Dictionary<CalendarAutomationPeer.DateTimeCalendarModePair, CalendarChildrenAutomationPeer>) null;
    }
    if (this.peers == null)
      return;
    this.peers.Clear();
    this.peers = (List<AutomationPeer>) null;
  }

  internal struct DateTimeCalendarModePair
  {
    private Syncfusion.Windows.Controls.CalendarMode DisplayMode;
    private DateTime Date;

    internal DateTimeCalendarModePair(DateTime date, Syncfusion.Windows.Controls.CalendarMode mode)
    {
      this.DisplayMode = mode;
      this.Date = date;
    }
  }
}
