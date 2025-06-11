// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DayGrid
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class DayGrid : CalendarEditGrid
{
  internal const int DEFROWSCOUNT = 6;
  internal const int DEFCOLUMNSCOUNT = 7;
  private Border mselectionBorder;
  private int[,] mcalendarMatrix;
  private CalendarEdit mparentCalendar;
  private Hashtable mdateCells;
  private List<int> mweekNumbers;
  private List<WeekNumberCell> mweekNumberCells;
  private Hashtable moldTooltipIndexes = new Hashtable();
  private Hashtable mnewTooltipDates = new Hashtable();

  static DayGrid()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (DayGrid), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (DayGrid)));
  }

  public DayGrid()
    : base(6, 7)
  {
    this.mweekNumbers = new List<int>();
    this.SelectionBorder = new Border();
    this.DateCells = new Hashtable();
    this.SelectionBorder.Opacity = 0.0;
    Grid.SetRowSpan((UIElement) this.SelectionBorder, this.RowsCount);
    this.AddToInnerGrid((UIElement) this.SelectionBorder);
  }

  internal List<WeekNumberCell> WeekNumbers => this.mweekNumberCells;

  protected internal Hashtable DateCells
  {
    get => this.mdateCells;
    set => this.mdateCells = value;
  }

  protected internal int[,] CalendarMatrix
  {
    get => this.mcalendarMatrix;
    set => this.mcalendarMatrix = value;
  }

  protected internal CalendarEdit ParentCalendar
  {
    get => this.mparentCalendar;
    set => this.mparentCalendar = value;
  }

  protected internal Border SelectionBorder
  {
    get => this.mselectionBorder;
    set => this.mselectionBorder = value;
  }

  public override void Initialize(VisibleDate data, CultureInfo culture, System.Globalization.Calendar calendar)
  {
    if (this.ParentCalendar == null)
      return;
    DateTimeFormatInfo dateTimeFormat = culture.DateTimeFormat;
    VisibleDate visibleDate = data;
    this.SetDayCellDate(data, dateTimeFormat, calendar);
    this.SetDayCellContent();
    this.SetIsCurrentMonth(data.VisibleMonth);
    this.SetIsToday(calendar);
    this.SetIsFirstDayofMonth();
    this.UpdateDateCells(calendar, visibleDate.VisibleMonth);
    this.SetIsDate(calendar);
    this.SetIsSelected();
    this.SetIsInvalid();
    this.ParentCalendar.InitilizeDayCellTemplates(this);
    this.ParentCalendar.InitilizeDayCellStyles(this);
    if (this.Visibility != Visibility.Hidden || this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    for (int index = 0; index < this.CellsCollection.Count; ++index)
    {
      if (((DayCell) this.CellsCollection[index]).IsDate)
        this.FocusedCellIndex = index;
    }
  }

  internal void SetDayCellToolTip(DayCell dc, int index)
  {
    if (this.moldTooltipIndexes != null && this.moldTooltipIndexes.Count > 0 && this.IsInTooltipIndexArrays(index))
      this.SetOldCellTooltip(dc, index);
    Hashtable tooltipDates = this.ParentCalendar.TooltipDates;
    if (tooltipDates == null || tooltipDates.Count <= 0 || !tooltipDates.ContainsKey((object) dc.Date))
      return;
    this.SetNewCellTooltip(dc, tooltipDates, index);
  }

  protected internal void SetIsSelected()
  {
    bool flag = false;
    if (this.ParentCalendar != null && this.ParentCalendar.SelectedDatesList != null && this.ParentCalendar.SelectedDatesList.Count != 0)
    {
      if (this.ParentCalendar.SelectedDatesList.Count == 1 || this.ParentCalendar.AllowMultiplySelection)
      {
        foreach (DayCell cells in this.CellsCollection)
        {
          if (this.ParentCalendar.SelectedDatesList.BinarySearch(cells.Date) >= 0)
            flag = true;
          cells.IsSelected = flag;
          flag = false;
        }
      }
      else
        this.ParentCalendar.SelectedDatesList.Clear();
    }
    else
    {
      if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
        return;
      foreach (Cell cells in this.CellsCollection)
        cells.IsSelected = false;
    }
  }

  protected internal void SetIsInvalid()
  {
    bool flag = false;
    if (this.ParentCalendar != null && this.ParentCalendar.InvalidDates != null && this.ParentCalendar.InvalidDates.Count != 0)
    {
      foreach (DayCell cells in this.CellsCollection)
      {
        if (this.ParentCalendar.InvalidDates.BinarySearch(cells.Date) >= 0)
          flag = true;
        cells.IsInvalidDate = flag;
        flag = false;
      }
    }
    else
    {
      if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
        return;
      foreach (Cell cells in this.CellsCollection)
        cells.IsInvalidDate = false;
    }
  }

  protected internal void SetIsDate(System.Globalization.Calendar calendar)
  {
    Date date = new Date(this.ParentCalendar.Date, calendar);
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (DayCell cells in this.CellsCollection)
      cells.IsDate = cells.Date == date;
  }

  protected internal void SetIsFirstDayofMonth()
  {
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (DayCell cells in this.CellsCollection)
    {
      Date date = cells.Date;
      cells.IsFirstDayofMonth = date.Day == 1;
    }
  }

  protected internal void SetIsToday(System.Globalization.Calendar calendar)
  {
    Date date = new Date(DateTime.Now, calendar);
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (DayCell cells in this.CellsCollection)
      cells.IsToday = cells.Date == date;
  }

  protected internal void SetIsCurrentMonth(int month)
  {
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (DayCell cells in this.CellsCollection)
    {
      Date date = cells.Date;
      cells.IsCurrentMonth = date.Month == month;
    }
  }

  protected internal void SetDayCellContent()
  {
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (DayCell cells in this.CellsCollection)
      cells.Content = (object) cells.Date;
  }

  protected internal void SetDayCellDate(
    VisibleDate data,
    DateTimeFormatInfo format,
    System.Globalization.Calendar calendar)
  {
    int visibleMonth = data.VisibleMonth;
    int visibleYear = data.VisibleYear;
    DateTime firstDayOfMonth = DateUtils.GetFirstDayOfMonth(visibleYear, visibleMonth, calendar);
    int dayOfWeek = (int) calendar.GetDayOfWeek(firstDayOfMonth);
    int firstDayOfWeek = (int) format.FirstDayOfWeek;
    int daysInMonth = calendar.GetDaysInMonth(visibleYear, visibleMonth);
    Date date1 = new Date();
    int num = (6 + (dayOfWeek - firstDayOfWeek)) % 7 + 1;
    this.CalendarMatrix = DateUtils.GenerateMatrix(visibleMonth, visibleYear, format, calendar);
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    for (int index = 0; index < this.CellsCollection.Count; ++index)
    {
      int row = Grid.GetRow((UIElement) this.CellsCollection[index]);
      int column = Grid.GetColumn((UIElement) this.CellsCollection[index]);
      if (index >= num && index < daysInMonth + num)
      {
        date1.Year = visibleYear;
        date1.Month = visibleMonth;
        date1.Day = this.CalendarMatrix[row, column];
      }
      if (index < num)
      {
        date1.Year = visibleMonth != 1 ? visibleYear : visibleYear - 1;
        if (date1.Year < calendar.MinSupportedDateTime.Year)
          date1.Year = calendar.MaxSupportedDateTime.Year;
        date1.Month = DateUtils.AddMonth(visibleMonth, -1);
        date1.Day = this.CalendarMatrix[row, column];
      }
      if (index >= daysInMonth + num)
      {
        date1.Year = visibleMonth != 12 ? visibleYear : visibleYear + 1;
        date1.Month = DateUtils.AddMonth(visibleMonth, 1);
        date1.Day = this.CalendarMatrix[row, column];
      }
      Date date2;
      Date date3;
      if (this.ParentCalendar.MinMaxHidden)
      {
        date2 = new Date(this.ParentCalendar.MaxDate, calendar);
        date3 = new Date(this.ParentCalendar.MinDate, calendar);
      }
      else
      {
        date2 = new Date(this.ParentCalendar.mxDate, calendar);
        date3 = new Date(this.ParentCalendar.miDate, calendar);
      }
      DayCell cells = (DayCell) this.CellsCollection[index];
      if (this.ParentCalendar.MinMaxHidden)
      {
        if (date1 > date2 || date1 < date3)
        {
          cells.Visibility = Visibility.Hidden;
          cells.Date = new Date(0, 0, 0);
        }
        else
        {
          if (cells.Visibility == Visibility.Hidden)
            cells.Visibility = Visibility.Visible;
          cells.Opacity = 1.0;
          cells.IsEnabled = true;
          cells.Date = date1;
        }
      }
      else
      {
        if (cells.Visibility == Visibility.Hidden)
          cells.Visibility = Visibility.Visible;
        Date date4 = new Date(this.ParentCalendar.MaxDate, calendar);
        Date date5 = new Date(this.ParentCalendar.MinDate, calendar);
        if (date1 > date4 || date1 < date5)
        {
          cells.IsEnabled = false;
          cells.Opacity = 0.5;
          cells.Date = date1;
          if (cells.Date.Day == 0)
            cells.Visibility = Visibility.Hidden;
        }
        else
        {
          cells.IsEnabled = true;
          cells.Opacity = 1.0;
          cells.Date = date1;
        }
      }
      this.SetDayCellToolTip(cells, index);
    }
  }

  protected internal void UpdateTemplateAndSelector(
    DataTemplate template,
    DataTemplateSelector selector,
    DataTemplatesDictionary dateTemplates)
  {
    if (template != null && selector != null)
      throw new ArgumentException("Both template and selector can not be set at one time.");
    if (this.ParentCalendar == null)
      return;
    System.Globalization.Calendar calendar = this.ParentCalendar.Calendar;
    if (dateTemplates == null)
    {
      if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
        return;
      foreach (DayCell cells in this.CellsCollection)
        cells.UpdateCellTemplateAndSelector(template, selector);
    }
    else
    {
      if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
        return;
      foreach (DayCell cells in this.CellsCollection)
      {
        if (cells.Date.Day > 0 && cells.Visibility != Visibility.Hidden)
        {
          DateTime dateTime = cells.Date.ToDateTime(calendar);
          if (!dateTemplates.Contains((object) dateTime))
            cells.UpdateCellTemplateAndSelector(template, selector);
          else
            cells.SetTemplate((DataTemplate) dateTemplates[(object) dateTime]);
          List<DateTime> dateTimeList = new List<DateTime>();
          if (this.ParentCalendar.BlackoutDates != null)
          {
            foreach (BlackoutDatesRange blackoutDate in (Collection<BlackoutDatesRange>) this.ParentCalendar.BlackoutDates)
            {
              if (blackoutDate.StartDate <= dateTime && blackoutDate.EndDate >= dateTime)
                dateTimeList.Add(dateTime);
            }
          }
          if (this.ParentCalendar.SpecialDates != null)
          {
            foreach (SpecialDate specialDate in (Collection<SpecialDate>) this.ParentCalendar.SpecialDates)
            {
              if (specialDate.Date.Date == dateTime.Date && specialDate.CellTemplate != null && (dateTimeList == null || dateTimeList != null && !dateTimeList.Contains(dateTime)))
                cells.ContentTemplate = specialDate.CellTemplate;
            }
          }
        }
      }
    }
  }

  internal void SetSpecialDatesTemplate()
  {
    foreach (DayCell cells in this.CellsCollection)
    {
      if (cells.Date.Day > 0 && cells.Visibility != Visibility.Hidden)
      {
        DateTime dateTime = cells.Date.ToDateTime(this.ParentCalendar.Calendar);
        List<DateTime> dateTimeList = new List<DateTime>();
        if (this.ParentCalendar.BlackoutDates != null)
        {
          foreach (BlackoutDatesRange blackoutDate in (Collection<BlackoutDatesRange>) this.ParentCalendar.BlackoutDates)
          {
            if (blackoutDate.StartDate <= dateTime && blackoutDate.EndDate >= dateTime)
              dateTimeList.Add(dateTime);
          }
        }
        if (this.ParentCalendar.SpecialDates != null)
        {
          foreach (SpecialDate specialDate in (Collection<SpecialDate>) this.ParentCalendar.SpecialDates)
          {
            if (specialDate.Date.Date == dateTime.Date && specialDate.CellTemplate != null && (dateTimeList == null || dateTimeList != null && !dateTimeList.Contains(dateTime)))
              cells.ContentTemplate = specialDate.CellTemplate;
          }
        }
      }
    }
  }

  protected internal void UpdateStyles(Style style, StylesDictionary dateStyles)
  {
    if (this.ParentCalendar == null)
      return;
    System.Globalization.Calendar calendar = this.ParentCalendar.Calendar;
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    if (dateStyles == null)
    {
      foreach (DayCell cells in this.CellsCollection)
        cells.SetStyle(style);
    }
    else
    {
      foreach (DayCell cells in this.CellsCollection)
      {
        if (cells.Date.Day > 0 && cells.Visibility != Visibility.Hidden)
        {
          DateTime dateTime = cells.Date.ToDateTime(calendar);
          if (!dateStyles.Contains((object) dateTime))
            cells.SetStyle(style);
          else
            cells.SetStyle((Style) dateStyles[(object) dateTime]);
        }
      }
    }
  }

  protected override void OnVisualParentChanged(DependencyObject oldParent)
  {
    base.OnVisualParentChanged(oldParent);
    this.UpdateParent();
    if (this.ParentCalendar == null || this.ParentCalendar.VisualMode != CalendarVisualMode.Days && this.ParentCalendar.VisualMode != CalendarVisualMode.All)
      return;
    this.Initialize(this.ParentCalendar.VisibleData, this.ParentCalendar.Culture, this.ParentCalendar.Calendar);
    if (this.ParentCalendar.WeekNumbersGrid == null)
      return;
    this.ParentCalendar.WeekNumbersGrid.SetWeekNumbers(this.WeekNumbers);
  }

  protected override Cell CreateCell() => (Cell) new DayCell();

  private void UpdateParent()
  {
    if (this.Parent == null)
      throw new NullReferenceException("Parent is null");
    this.ParentCalendar = this.Parent is CalendarEdit parent ? parent : throw new NotSupportedException("Parent must be inherited from CalendarEdit type.");
  }

  private void UpdateDateCells(System.Globalization.Calendar calendar, int visibleMonth)
  {
    if (this.DateCells != null)
      this.DateCells.Clear();
    this.mweekNumbers.Clear();
    if (this.CellsCollection != null && this.CellsCollection.Count > 0)
    {
      foreach (DayCell cells in this.CellsCollection)
      {
        if (cells.Date.Day > 0 && cells.Date.Year <= 9999 && cells.Visibility != Visibility.Hidden)
        {
          DateTime dateTime = cells.Date.ToDateTime(calendar);
          int weekNumber = this.ParentCalendar.GetWeekNumber(dateTime);
          if (dateTime.DayOfWeek == DayOfWeek.Thursday && !this.mweekNumbers.Contains(weekNumber))
            this.mweekNumbers.Add(weekNumber);
          if (this.DateCells != null)
            this.DateCells.Add((object) dateTime, (object) cells);
          this.HideNextMonthDays(cells, visibleMonth);
          this.HidePrevMonthDays(cells, visibleMonth);
        }
      }
    }
    this.FillWeekNumberCells();
  }

  private int GetNextMonth(int visibleMonth)
  {
    int nextMonth = visibleMonth + 1;
    if (visibleMonth == 12)
      nextMonth = 1;
    return nextMonth;
  }

  private int GetPrevMonth(int visibleMonth)
  {
    int prevMonth = visibleMonth - 1;
    if (visibleMonth == 1)
      prevMonth = 12;
    return prevMonth;
  }

  private void HidePrevMonthDays(DayCell dayCell, int visibleMonth)
  {
    if (this.ParentCalendar.ShowPreviousMonthDays || dayCell.Date.Month != this.GetPrevMonth(visibleMonth))
      return;
    dayCell.Visibility = Visibility.Hidden;
  }

  private void HideNextMonthDays(DayCell dayCell, int visibleMonth)
  {
    if (this.ParentCalendar.ShowNextMonthDays || dayCell.Date.Month != this.GetNextMonth(visibleMonth))
      return;
    dayCell.Visibility = Visibility.Hidden;
  }

  private void FillWeekNumberCells()
  {
    this.mweekNumberCells = new List<WeekNumberCell>();
    foreach (int mweekNumber in this.mweekNumbers)
    {
      WeekNumberCell weekNumberCell = new WeekNumberCell();
      weekNumberCell.Content = (object) mweekNumber;
      this.mweekNumberCells.Add(weekNumberCell);
    }
  }

  private void SetNewCellTooltip(DayCell dc, Hashtable toolTipDates, int index)
  {
    if (!this.moldTooltipIndexes.Contains((object) index))
      this.moldTooltipIndexes.Add((object) index, dc.ToolTip);
    System.Windows.Controls.ToolTip toolTipDate = toolTipDates[(object) dc.Date] as System.Windows.Controls.ToolTip;
    dc.ToolTip = (object) toolTipDate;
    if (this.mnewTooltipDates.Contains((object) index))
      return;
    this.mnewTooltipDates.Add((object) index, (object) dc.Date);
  }

  private void SetOldCellTooltip(DayCell dc, int index)
  {
    Date mnewTooltipDate = (Date) this.mnewTooltipDates[(object) index];
    if (dc.Content == null || !((Date) dc.Content == mnewTooltipDate))
      return;
    System.Windows.Controls.ToolTip moldTooltipIndex = this.moldTooltipIndexes[(object) index] as System.Windows.Controls.ToolTip;
    dc.ToolTip = (object) moldTooltipIndex;
    this.mnewTooltipDates.Remove((object) index);
    this.moldTooltipIndexes.Remove((object) index);
  }

  private bool IsInTooltipIndexArrays(int index)
  {
    return this.moldTooltipIndexes.Contains((object) index) && this.mnewTooltipDates.Contains((object) index);
  }

  public new void Dispose() => this.Dispose(true);

  protected override void Dispose(bool disposing)
  {
    if (this.moldTooltipIndexes != null)
    {
      this.moldTooltipIndexes.Clear();
      this.moldTooltipIndexes = (Hashtable) null;
    }
    if (this.mnewTooltipDates != null)
    {
      this.mnewTooltipDates.Clear();
      this.mnewTooltipDates = (Hashtable) null;
    }
    if (this.mweekNumberCells != null)
    {
      for (int index = 0; index < this.mweekNumberCells.Count; ++index)
      {
        this.mweekNumberCells[index].Dispose();
        this.mweekNumberCells[index] = (WeekNumberCell) null;
      }
      this.mweekNumberCells.Clear();
    }
    if (this.mweekNumbers != null)
      this.mweekNumbers.Clear();
    if (this.mdateCells != null)
    {
      this.mdateCells.Clear();
      this.mdateCells = (Hashtable) null;
    }
    if (this.mparentCalendar != null)
      this.mparentCalendar = (CalendarEdit) null;
    this.mselectionBorder = (Border) null;
    if (this.CreateCell() is DayCell)
      (this.CreateCell() as DayCell).Dispose();
    base.Dispose(disposing);
  }
}
