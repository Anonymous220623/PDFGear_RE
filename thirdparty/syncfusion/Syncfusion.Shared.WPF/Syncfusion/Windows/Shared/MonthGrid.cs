// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.MonthGrid
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
public class MonthGrid : CalendarEditGrid
{
  internal const int DEFROWSCOUNT = 3;
  internal const int DEFCOLUMNSCOUNT = 4;
  private CalendarEdit mparentCalendar;

  protected internal CalendarEdit ParentCalendar
  {
    get => this.mparentCalendar;
    set => this.mparentCalendar = value;
  }

  static MonthGrid()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (MonthGrid), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (MonthGrid)));
  }

  public MonthGrid()
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
    if (this.ParentCalendar == null || this.ParentCalendar.VisualMode != CalendarVisualMode.Months)
      return;
    this.Initialize(this.ParentCalendar.VisibleData, this.ParentCalendar.Culture, this.ParentCalendar.Calendar);
  }

  public override void Initialize(VisibleDate date, CultureInfo culture, Calendar calendar)
  {
    this.SetMonthNumber(date, calendar);
    this.SetMonthCellContent(culture);
    this.SetIsSelected(date);
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    for (int index = 0; index < this.CellsCollection.Count; ++index)
    {
      MonthCell cells = (MonthCell) this.CellsCollection[index];
      if (this.ParentCalendar != null)
      {
        DateTime maxDate = this.ParentCalendar.MaxDate;
        DateTime minDate = this.ParentCalendar.MinDate;
        if (date.VisibleYear == this.ParentCalendar.MaxDate.Year && cells.MonthNumber > this.ParentCalendar.MaxDate.Month)
          cells.Visibility = Visibility.Hidden;
        if (date.VisibleYear == this.ParentCalendar.MinDate.Year && cells.MonthNumber < this.ParentCalendar.MinDate.Month)
          cells.Visibility = Visibility.Hidden;
      }
      if (cells.IsSelected)
        this.FocusedCellIndex = index;
    }
  }

  public override void SetIsSelected(VisibleDate date)
  {
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (MonthCell cells in this.CellsCollection)
    {
      if (cells.MonthNumber == date.VisibleMonth)
        cells.IsSelected = true;
      else
        cells.IsSelected = false;
    }
  }

  protected internal void SetMonthCellContent(CultureInfo culture)
  {
    DateTimeFormatInfo dateTimeFormat = culture.DateTimeFormat;
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (MonthCell cells in this.CellsCollection)
    {
      if (cells.Visibility == Visibility.Visible)
      {
        if (culture.Name == "ja-JP" || culture.Name == "zh-CN")
          cells.Content = (object) dateTimeFormat.MonthNames[cells.MonthNumber - 1];
        else
          cells.Content = (object) dateTimeFormat.AbbreviatedMonthNames[cells.MonthNumber - 1];
      }
    }
  }

  protected internal void SetMonthNumber(VisibleDate date, Calendar calendar)
  {
    int month = 1;
    Date date1 = new Date(calendar.MinSupportedDateTime, calendar);
    Date date2 = new Date(calendar.MaxSupportedDateTime, calendar);
    if (this.CellsCollection == null || this.CellsCollection.Count <= 0)
      return;
    foreach (MonthCell cells in this.CellsCollection)
    {
      Date date3 = new Date(date.VisibleYear, month, 1);
      if (date3 > date2 || date3 < date1)
      {
        cells.Visibility = Visibility.Hidden;
        cells.MonthNumber = -1;
      }
      else
      {
        if (cells.Visibility == Visibility.Hidden)
          cells.Visibility = Visibility.Visible;
        cells.MonthNumber = month;
      }
      ++month;
    }
  }

  protected override Cell CreateCell() => (Cell) new MonthCell();

  public new void Dispose() => this.Dispose(true);

  protected override void Dispose(bool disposing)
  {
    if (this.mparentCalendar != null)
      this.mparentCalendar = (CalendarEdit) null;
    base.Dispose(disposing);
  }
}
