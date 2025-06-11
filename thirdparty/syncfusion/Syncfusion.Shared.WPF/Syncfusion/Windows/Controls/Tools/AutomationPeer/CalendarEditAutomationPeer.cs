// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.Tools.AutomationPeer.CalendarEditAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Controls.Tools.AutomationPeer;

[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class CalendarEditAutomationPeer(CalendarEdit owner) : 
  FrameworkElementAutomationPeer((FrameworkElement) owner),
  IMultipleViewProvider,
  ISelectionProvider,
  ITableProvider,
  IGridProvider
{
  private CalendarEdit OwningCalendar => this.Owner as CalendarEdit;

  private CalendarEditGrid OwningGrid
  {
    get
    {
      if (this.OwningCalendar == null)
        return (CalendarEditGrid) null;
      return this.OwningCalendar.CalendarStyle == CalendarStyle.Standard ? this.OwningCalendar.FindCurrentGrid(CalendarVisualMode.Days) : this.OwningCalendar.FindCurrentGrid(this.OwningCalendar.VisualMode);
    }
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

  protected override List<System.Windows.Automation.Peers.AutomationPeer> GetChildrenCore()
  {
    List<System.Windows.Automation.Peers.AutomationPeer> childrenCore = new List<System.Windows.Automation.Peers.AutomationPeer>();
    if (this.OwningCalendar != null)
    {
      if (this.OwningCalendar.m_prevButton != null)
      {
        System.Windows.Automation.Peers.AutomationPeer peerForElement = (System.Windows.Automation.Peers.AutomationPeer) (UIElementAutomationPeer.CreatePeerForElement((UIElement) this.OwningCalendar.m_prevButton) as CalendarEditNavigatorAutomationPeer);
        if (peerForElement != null)
          childrenCore.Add(peerForElement);
      }
      if (this.OwningCalendar.m_nextButton != null)
      {
        System.Windows.Automation.Peers.AutomationPeer peerForElement = (System.Windows.Automation.Peers.AutomationPeer) (UIElementAutomationPeer.CreatePeerForElement((UIElement) this.OwningCalendar.m_nextButton) as CalendarEditNavigatorAutomationPeer);
        if (peerForElement != null)
          childrenCore.Add(peerForElement);
      }
      if (this.OwningCalendar.m_monthButton1 != null)
      {
        System.Windows.Automation.Peers.AutomationPeer peerForElement = (System.Windows.Automation.Peers.AutomationPeer) (UIElementAutomationPeer.CreatePeerForElement((UIElement) this.OwningCalendar.m_monthButton1) as CalendarEditHeaderAutomationPeer);
        if (peerForElement != null)
          childrenCore.Add(peerForElement);
      }
      if (this.OwningCalendar.m_monthButton2 != null)
      {
        System.Windows.Automation.Peers.AutomationPeer peerForElement = (System.Windows.Automation.Peers.AutomationPeer) (UIElementAutomationPeer.CreatePeerForElement((UIElement) this.OwningCalendar.m_monthButton2) as CalendarEditHeaderAutomationPeer);
        if (peerForElement != null)
          childrenCore.Add(peerForElement);
      }
      if (this.OwningGrid != null)
      {
        System.Windows.Automation.Peers.AutomationPeer peerForElement1 = UIElementAutomationPeer.CreatePeerForElement((UIElement) this.OwningGrid);
        if (peerForElement1 != null)
          childrenCore.Add(peerForElement1);
        foreach (UIElement cells in this.OwningGrid.CellsCollection)
        {
          if (UIElementAutomationPeer.CreatePeerForElement(cells) is CalendarEditCellAutomationPeer)
          {
            (UIElementAutomationPeer.CreatePeerForElement(cells) as CalendarEditCellAutomationPeer).Calendar = this.OwningCalendar;
            System.Windows.Automation.Peers.AutomationPeer peerForElement2 = (System.Windows.Automation.Peers.AutomationPeer) (UIElementAutomationPeer.CreatePeerForElement(cells) as CalendarEditCellAutomationPeer);
            if (peerForElement2 != null)
              childrenCore.Add(peerForElement2);
          }
        }
      }
      if (this.OwningCalendar.DayNamesGrid != null && this.OwningCalendar.DayNamesGrid.IsVisible)
      {
        foreach (UIElement dayNameCell in this.OwningCalendar.DayNamesGrid.DayNameCells)
        {
          System.Windows.Automation.Peers.AutomationPeer peerForElement = (System.Windows.Automation.Peers.AutomationPeer) (UIElementAutomationPeer.CreatePeerForElement(dayNameCell) as CalendarEditDayNamesAutomationPeer);
          if (peerForElement != null)
            childrenCore.Add(peerForElement);
        }
      }
      if (this.OwningCalendar.VisualMode == CalendarVisualMode.Days && this.OwningCalendar.WeekNumbersGrid != null && this.OwningCalendar.WeekNumbersGrid.IsVisible)
      {
        foreach (UIElement weekNumberCell in this.OwningCalendar.WeekNumbersGrid.WeekNumberCells)
        {
          System.Windows.Automation.Peers.AutomationPeer peerForElement = (System.Windows.Automation.Peers.AutomationPeer) (UIElementAutomationPeer.CreatePeerForElement(weekNumberCell) as CalendarEditCellAutomationPeer);
          if (peerForElement != null)
            childrenCore.Add(peerForElement);
        }
      }
    }
    return childrenCore;
  }

  int IGridProvider.ColumnCount => this.OwningGrid != null ? this.OwningGrid.ColumnsCount : 0;

  int IGridProvider.RowCount
  {
    get
    {
      if (this.OwningGrid == null)
        return 0;
      return this.OwningCalendar.VisualMode == CalendarVisualMode.Days ? Math.Max(0, this.OwningGrid.RowsCount) : this.OwningGrid.RowsCount;
    }
  }

  IRawElementProviderSimple IGridProvider.GetItem(int row, int column)
  {
    if (this.OwningGrid != null && row >= 0 && row < this.OwningGrid.RowsCount && column >= 0 && column < this.OwningGrid.ColumnsCount)
    {
      foreach (UIElement cells in this.OwningGrid.CellsCollection)
      {
        int num1 = (int) cells.GetValue(Grid.RowProperty);
        int num2 = (int) cells.GetValue(Grid.ColumnProperty);
        if (num1 == row && num2 == column)
        {
          System.Windows.Automation.Peers.AutomationPeer peerForElement = (System.Windows.Automation.Peers.AutomationPeer) (UIElementAutomationPeer.CreatePeerForElement(cells) as CalendarEditCellAutomationPeer);
          if (peerForElement != null)
            return this.ProviderFromPeer(peerForElement);
        }
      }
    }
    return (IRawElementProviderSimple) null;
  }

  int IMultipleViewProvider.CurrentView
  {
    get => this.OwningCalendar != null ? (int) this.OwningCalendar.VisualMode : 0;
  }

  int[] IMultipleViewProvider.GetSupportedViews()
  {
    return new int[5]{ 2, 4, 8, 16 /*0x10*/, 1 };
  }

  string IMultipleViewProvider.GetViewName(int viewId)
  {
    switch (viewId)
    {
      case 0:
        return Syncfusion.Windows.Controls.SR.Get(Syncfusion.Windows.Controls.SRID.CalendarEditAutomationPeer_DaysMode);
      case 1:
        return Syncfusion.Windows.Controls.SR.Get(Syncfusion.Windows.Controls.SRID.CalendarEditAutomationPeer_MonthMode);
      case 2:
        return Syncfusion.Windows.Controls.SR.Get(Syncfusion.Windows.Controls.SRID.CalendarEditAutomationPeer_YearMode);
      case 3:
        return Syncfusion.Windows.Controls.SR.Get(Syncfusion.Windows.Controls.SRID.CalendarEditAutomationPeer_YearRangeMode);
      case 4:
        return Syncfusion.Windows.Controls.SR.Get(Syncfusion.Windows.Controls.SRID.CalendarEditAutomationPeer_WeekNumbersMode);
      default:
        return string.Empty;
    }
  }

  void IMultipleViewProvider.SetCurrentView(int viewId)
  {
    if (this.OwningCalendar == null)
      return;
    this.OwningCalendar.VisualMode = (CalendarVisualMode) viewId;
  }

  bool ISelectionProvider.CanSelectMultiple
  {
    get => this.OwningCalendar != null && this.OwningCalendar.AllowMultiplySelection;
  }

  bool ISelectionProvider.IsSelectionRequired
  {
    get => this.OwningCalendar != null && this.OwningCalendar.AllowSelection;
  }

  IRawElementProviderSimple[] ISelectionProvider.GetSelection()
  {
    List<IRawElementProviderSimple> elementProviderSimpleList = new List<IRawElementProviderSimple>();
    if (this.OwningCalendar != null && this.OwningGrid != null)
    {
      if (this.OwningCalendar.VisualMode == CalendarVisualMode.Days && this.OwningCalendar.SelectedDates != null && this.OwningCalendar.SelectedDates.Count != 0)
      {
        foreach (UIElement cells in this.OwningGrid.CellsCollection)
        {
          if (cells is DayCell && cells is DayCell element && element.IsSelected)
          {
            System.Windows.Automation.Peers.AutomationPeer peerForElement = (System.Windows.Automation.Peers.AutomationPeer) (UIElementAutomationPeer.CreatePeerForElement((UIElement) element) as CalendarEditCellAutomationPeer);
            if (peerForElement != null)
              elementProviderSimpleList.Add(this.ProviderFromPeer(peerForElement));
          }
        }
      }
      else
      {
        foreach (UIElement cells in this.OwningGrid.CellsCollection)
        {
          if (cells is Cell && cells is Cell element && element.IsFocused)
          {
            System.Windows.Automation.Peers.AutomationPeer peerForElement = (System.Windows.Automation.Peers.AutomationPeer) (UIElementAutomationPeer.CreatePeerForElement((UIElement) element) as CalendarEditCellAutomationPeer);
            if (peerForElement != null)
              elementProviderSimpleList.Add(this.ProviderFromPeer(peerForElement));
          }
        }
      }
    }
    return elementProviderSimpleList.Count > 0 ? elementProviderSimpleList.ToArray() : (IRawElementProviderSimple[]) null;
  }

  RowOrColumnMajor ITableProvider.RowOrColumnMajor => RowOrColumnMajor.RowMajor;

  IRawElementProviderSimple[] ITableProvider.GetColumnHeaders()
  {
    if (this.OwningCalendar.VisualMode == CalendarVisualMode.Days && this.OwningCalendar != null && this.OwningCalendar.DayNamesGrid != null)
    {
      List<IRawElementProviderSimple> elementProviderSimpleList = new List<IRawElementProviderSimple>();
      foreach (UIElement dayNameCell in this.OwningCalendar.DayNamesGrid.DayNameCells)
      {
        System.Windows.Automation.Peers.AutomationPeer peerForElement = (System.Windows.Automation.Peers.AutomationPeer) (UIElementAutomationPeer.CreatePeerForElement(dayNameCell) as CalendarEditDayNamesAutomationPeer);
        if (peerForElement != null)
          elementProviderSimpleList.Add(this.ProviderFromPeer(peerForElement));
      }
      if (elementProviderSimpleList.Count > 0)
        return elementProviderSimpleList.ToArray();
    }
    return (IRawElementProviderSimple[]) null;
  }

  IRawElementProviderSimple[] ITableProvider.GetRowHeaders()
  {
    if (this.OwningCalendar.VisualMode == CalendarVisualMode.Days && this.OwningCalendar != null && this.OwningCalendar.WeekNumbersGrid != null && this.OwningCalendar.WeekNumbersGrid.IsVisible)
    {
      List<IRawElementProviderSimple> elementProviderSimpleList = new List<IRawElementProviderSimple>();
      foreach (UIElement weekNumberCell in this.OwningCalendar.WeekNumbersGrid.WeekNumberCells)
      {
        System.Windows.Automation.Peers.AutomationPeer peerForElement = (System.Windows.Automation.Peers.AutomationPeer) (UIElementAutomationPeer.CreatePeerForElement(weekNumberCell) as CalendarEditCellAutomationPeer);
        if (peerForElement != null)
          elementProviderSimpleList.Add(this.ProviderFromPeer(peerForElement));
      }
      if (elementProviderSimpleList.Count > 0)
        return elementProviderSimpleList.ToArray();
    }
    return (IRawElementProviderSimple[]) null;
  }
}
