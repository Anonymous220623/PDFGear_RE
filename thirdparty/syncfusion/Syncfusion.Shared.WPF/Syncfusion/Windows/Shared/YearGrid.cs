// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.YearGrid
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
public class YearGrid : CalendarEditGrid
{
  internal const int DEFROWSCOUNT = 3;
  internal const int DEFCOLUMNSCOUNT = 4;
  private CalendarEdit mparentCalendar;

  protected internal CalendarEdit ParentCalendar
  {
    get => this.mparentCalendar;
    set => this.mparentCalendar = value;
  }

  static YearGrid()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (YearGrid), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (YearGrid)));
  }

  public YearGrid()
    : base(3, 4)
  {
  }

  private void UpdateParent()
  {
    if (this.Parent == null)
      throw new NullReferenceException("Parent is null");
    this.ParentCalendar = this.Parent is CalendarEdit parent ? parent : throw new NotSupportedException("Parent must be inherited from CalendarEdit type.");
  }

  internal void VisulParent(DependencyObject obj) => this.OnVisualParentChanged(obj);

  protected override void OnVisualParentChanged(DependencyObject oldParent)
  {
    base.OnVisualParentChanged(oldParent);
    this.UpdateParent();
    if (this.ParentCalendar == null || this.ParentCalendar.VisualMode != CalendarVisualMode.Years)
      return;
    this.Initialize(this.ParentCalendar.VisibleData, this.ParentCalendar.Culture, this.ParentCalendar.Calendar);
  }

  public override void Initialize(VisibleDate date, CultureInfo culture, Calendar calendar)
  {
    this.SetYear(date, calendar);
    this.SetYearCellContent();
    this.SetIsSelected(date);
    this.SetIsBelongToCurrentRange();
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    for (int index = 0; index < this.CellsCollection.Count; ++index)
    {
      YearCell cells = (YearCell) this.CellsCollection[index];
      if (this.ParentCalendar != null)
      {
        DateTime maxDate = this.ParentCalendar.MaxDate;
        DateTime minDate = this.ParentCalendar.MinDate;
        if (cells.Year > this.ParentCalendar.MaxDate.Year || cells.Year < this.ParentCalendar.MinDate.Year)
          cells.Visibility = Visibility.Hidden;
      }
      if (cells.IsSelected)
        this.FocusedCellIndex = index;
    }
  }

  public override void SetIsSelected(VisibleDate date)
  {
    int visibleYear = date.VisibleYear;
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (YearCell cells in this.CellsCollection)
    {
      if (cells.Year == visibleYear)
        cells.IsSelected = true;
      else
        cells.IsSelected = false;
    }
  }

  protected internal void SetIsBelongToCurrentRange()
  {
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    for (int index = 0; index < this.CellsCollection.Count; ++index)
      ((YearCell) this.CellsCollection[index]).IsBelongToCurrentRange = index != 0 && index != this.CellsCollection.Count - 1;
  }

  protected internal void SetYearCellContent()
  {
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (YearCell cells in this.CellsCollection)
    {
      if (cells.Visibility == Visibility.Visible)
        cells.Content = (object) cells.Year;
    }
  }

  protected internal void SetYear(VisibleDate date, Calendar calendar)
  {
    int visibleYear = date.VisibleYear;
    Date date1 = new Date(calendar.MinSupportedDateTime, calendar);
    Date date2 = new Date(calendar.MaxSupportedDateTime, calendar);
    while (visibleYear % 10 != 0)
      --visibleYear;
    if (visibleYear == date2.Year)
      visibleYear -= 10;
    int year = visibleYear - 1;
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (YearCell cells in this.CellsCollection)
    {
      Date date3 = new Date(year, date.VisibleMonth, 1);
      if (date3 > date2 || date3 < date1)
      {
        cells.Visibility = Visibility.Hidden;
        cells.Year = -1;
      }
      else
      {
        if (cells.Visibility == Visibility.Hidden)
          cells.Visibility = Visibility.Visible;
        cells.Year = year;
      }
      ++year;
    }
  }

  protected override Cell CreateCell() => (Cell) new YearCell();

  public new void Dispose() => this.Dispose(true);

  protected override void Dispose(bool disposing)
  {
    if (this.mparentCalendar != null)
      this.mparentCalendar = (CalendarEdit) null;
    base.Dispose(disposing);
  }
}
