// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.YearRangeGrid
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class YearRangeGrid : CalendarEditGrid
{
  internal const int DEFROWSCOUNT = 3;
  internal const int DEFCOLUMNSCOUNT = 4;
  private CalendarEdit mparentCalendar;

  protected internal CalendarEdit ParentCalendar
  {
    get => this.mparentCalendar;
    set => this.mparentCalendar = value;
  }

  static YearRangeGrid()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (YearRangeGrid), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (YearRangeGrid)));
  }

  public YearRangeGrid()
    : base(3, 4)
  {
  }

  private void UpdateParent()
  {
    if (this.Parent == null)
      throw new NullReferenceException("Parent is null");
    this.ParentCalendar = this.Parent is CalendarEdit parent ? parent : throw new NotSupportedException("Parent must be inherited from CalendarEdit type.");
  }

  protected override void OnVisualParentChanged(DependencyObject oldParent)
  {
    base.OnVisualParentChanged(oldParent);
    this.UpdateParent();
    if (this.ParentCalendar == null || this.ParentCalendar.VisualMode != CalendarVisualMode.YearsRange)
      return;
    this.Initialize(this.ParentCalendar.VisibleData, this.ParentCalendar.Culture, this.ParentCalendar.Calendar);
  }

  public override void Initialize(VisibleDate date, CultureInfo culture, Calendar calendar)
  {
    this.SetYearRange(date, calendar);
    this.SetYearRangeCellContent();
    this.SetIsSelected(date);
    this.SetIsBelongToCurrentRange();
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    for (int index = 0; index < this.CellsCollection.Count; ++index)
    {
      YearRangeCell cells = (YearRangeCell) this.CellsCollection[index];
      if (this.ParentCalendar != null)
      {
        DateTime maxDate = this.ParentCalendar.MaxDate;
        DateTime minDate = this.ParentCalendar.MinDate;
        if ((cells.Years.StartYear > this.ParentCalendar.MinDate.Year || cells.Years.EndYear < this.ParentCalendar.MinDate.Year) && (cells.Years.StartYear < this.ParentCalendar.MinDate.Year || cells.Years.EndYear < this.ParentCalendar.MinDate.Year) || (cells.Years.StartYear > this.ParentCalendar.MaxDate.Year || cells.Years.EndYear > this.ParentCalendar.MaxDate.Year) && (cells.Years.StartYear > this.ParentCalendar.MaxDate.Year || cells.Years.EndYear < this.ParentCalendar.MaxDate.Year))
          cells.Visibility = Visibility.Hidden;
      }
      if (cells.IsSelected)
        this.FocusedCellIndex = index;
    }
  }

  public override void SetIsSelected(VisibleDate date)
  {
    int visibleYear = date.VisibleYear;
    bool flag = false;
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (YearRangeCell cells in this.CellsCollection)
    {
      if (visibleYear >= cells.Years.StartYear && visibleYear <= cells.Years.EndYear)
      {
        cells.IsSelected = true;
        flag = true;
      }
      else
        cells.IsSelected = false;
    }
    if (flag)
      return;
    (this.CellsCollection[this.CellsCollection.Count - 1] as Cell).IsSelected = true;
  }

  protected internal void SetYearRangeCellContent()
  {
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (YearRangeCell cells in this.CellsCollection)
    {
      if (cells.Visibility == Visibility.Visible)
      {
        YearsRange years = cells.Years;
        int startYear = years.StartYear;
        int endYear = years.EndYear;
        cells.Content = (object) $"{startYear.ToString()}-\n{endYear.ToString()}";
      }
    }
  }

  protected internal void SetYearRange(VisibleDate date, Calendar calendar)
  {
    int visibleYear = date.VisibleYear;
    Date date1 = new Date(calendar.MinSupportedDateTime, calendar);
    Date date2 = new Date(calendar.MaxSupportedDateTime, calendar);
    while (visibleYear % 10 != 0)
      --visibleYear;
    while (visibleYear % 100 != 0)
      visibleYear -= 10;
    int year = visibleYear - 10;
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (YearRangeCell cells in this.CellsCollection)
    {
      Date date3 = year != 0 ? new Date(year, date.VisibleMonth, 1) : new Date(1, date.VisibleMonth, 1);
      Date date4 = new Date(year + 9, date.VisibleMonth, 1);
      if (date3 > date2 || date3 < date1 || date4 > date2 || date4 < date1)
      {
        cells.Visibility = Visibility.Hidden;
        cells.Years = new YearsRange(-1, -1);
      }
      else
      {
        if (cells.Visibility == Visibility.Hidden)
          cells.Visibility = Visibility.Visible;
        cells.Years = new YearsRange(date3.Year, date4.Year);
      }
      year += 10;
    }
  }

  protected internal void SetIsBelongToCurrentRange()
  {
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    for (int index = 0; index < this.CellsCollection.Count; ++index)
      ((YearRangeCell) this.CellsCollection[index]).IsBelongToCurrentRange = index != 0 && index != this.CellsCollection.Count - 1;
  }

  protected override Cell CreateCell() => (Cell) new YearRangeCell();

  public new void Dispose() => this.Dispose(true);

  protected override void Dispose(bool disposing)
  {
    if (this.mparentCalendar != null)
      this.mparentCalendar = (CalendarEdit) null;
    base.Dispose(disposing);
  }
}
