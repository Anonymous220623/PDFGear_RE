// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.WeekNumberGridPanel
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class WeekNumberGridPanel : CalendarEditGrid
{
  private const int DEFROWSCOUNT = 7;
  private const int DEFCOLUMNSCOUNT = 8;
  public static int NumberOfWeeks;
  private CalendarEdit mparentCalendar;

  protected internal CalendarEdit ParentCalendar
  {
    get => this.mparentCalendar;
    set => this.mparentCalendar = value;
  }

  static WeekNumberGridPanel()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (WeekNumberGridPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (WeekNumberGridPanel)));
  }

  public WeekNumberGridPanel()
    : base(7, 8)
  {
  }

  protected void UpdateParent()
  {
    if (this.Parent == null)
      throw new NullReferenceException("Parent is null");
    this.ParentCalendar = this.Parent is CalendarEdit parent ? parent : throw new NotSupportedException("Parent must be inherited from CalendarEdit type.");
  }

  protected override void OnVisualParentChanged(DependencyObject oldParent)
  {
    base.OnVisualParentChanged(oldParent);
    this.UpdateParent();
    if (this.ParentCalendar == null || this.ParentCalendar.VisualMode != CalendarVisualMode.WeekNumbers)
      return;
    this.Initialize(this.ParentCalendar.VisibleData, this.ParentCalendar.Culture, this.ParentCalendar.Calendar);
  }

  public override void Initialize(VisibleDate date, CultureInfo culture, Calendar calendar)
  {
    this.SetWeekNumber(date, culture, calendar);
    this.SetWeekCellContent();
    this.SetIsSelected(date);
    for (int index = 0; index < this.CellsCollection.Count; ++index)
    {
      if (((Cell) this.CellsCollection[index]).IsSelected)
        this.FocusedCellIndex = index + 1;
    }
  }

  protected internal void SetWeekNumber(VisibleDate date, CultureInfo culture, Calendar calendar)
  {
    try
    {
      Thread.CurrentThread.CurrentCulture = culture;
      int visibleYear = date.VisibleYear;
      CalendarWeekRule calendarWeekRule = Thread.CurrentThread.CurrentCulture.DateTimeFormat.CalendarWeekRule;
      int monthsInYear = culture.Calendar.GetMonthsInYear(visibleYear, 0);
      DateTime dateTime = new DateTime(date.VisibleYear, 1, 1);
      int daysInMonth = culture.Calendar.GetDaysInMonth(visibleYear, monthsInYear);
      WeekNumberGridPanel.NumberOfWeeks = Thread.CurrentThread.CurrentCulture.Calendar.GetWeekOfYear(new DateTime(visibleYear, monthsInYear, daysInMonth), calendarWeekRule, dateTime.DayOfWeek);
      int num = 1;
      foreach (WeekNumberCellPanel cells in this.CellsCollection)
      {
        if (num <= WeekNumberGridPanel.NumberOfWeeks)
        {
          string str = Convert.ToString(num);
          cells.WeekNumber = str;
        }
        ++num;
      }
    }
    catch (Exception ex)
    {
    }
  }

  protected internal void SetWeekCellContent()
  {
    foreach (WeekNumberCellPanel cells in this.CellsCollection)
    {
      if (cells.Visibility == Visibility.Visible)
        cells.Content = (object) cells.WeekNumber;
    }
  }

  public override void SetIsSelected(VisibleDate date)
  {
    foreach (WeekNumberCellPanel cells in this.CellsCollection)
    {
      string clickedweeknumber = CalendarEdit.clickedweeknumber;
      if (cells.WeekNumber == clickedweeknumber)
        cells.IsSelected = true;
      else
        cells.IsSelected = false;
    }
  }

  protected override Cell CreateCell() => (Cell) new WeekNumberCellPanel();

  public new void Dispose() => this.Dispose(true);

  protected override void Dispose(bool disposing)
  {
    if (this.mparentCalendar != null)
      this.mparentCalendar = (CalendarEdit) null;
    if (this.CellsCollection != null)
    {
      this.CellsCollection.Clear();
      this.CellsCollection = (ArrayList) null;
    }
    base.Dispose(disposing);
  }
}
