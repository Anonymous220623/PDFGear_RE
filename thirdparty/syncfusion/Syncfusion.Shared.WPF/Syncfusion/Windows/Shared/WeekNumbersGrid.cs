// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.WeekNumbersGrid
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class WeekNumbersGrid : CalendarEditGrid
{
  private const int DEFCOLUMNSCOUNT = 1;
  private const int DEFROWSCOUNT = 6;
  private Grid minnerGrid;
  private CalendarEdit mparentCalendar;
  private ArrayList mweekNumberCells;

  static WeekNumbersGrid()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (WeekNumbersGrid), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (WeekNumbersGrid)));
  }

  public WeekNumbersGrid()
    : base(6, 1)
  {
    this.minnerGrid = new Grid();
    this.WeekNumberCells = new ArrayList();
    this.GenerateGrid();
    this.FillGrid();
    this.AddLogicalChild((object) this.minnerGrid);
    this.AddVisualChild((Visual) this.minnerGrid);
    this.Loaded += new RoutedEventHandler(this.WeekNumbersGrid_Loaded);
  }

  private void WeekNumbersGrid_Loaded(object sender, RoutedEventArgs e)
  {
    BindingUtils.SetRelativeBinding((DependencyObject) this.minnerGrid, FrameworkElement.FlowDirectionProperty, typeof (CalendarEdit), (object) FrameworkElement.FlowDirectionProperty, BindingMode.OneWay, 1);
  }

  public ArrayList WeekNumberCells
  {
    get => this.mweekNumberCells;
    set => this.mweekNumberCells = value;
  }

  public CalendarEdit ParentCalendar
  {
    get => this.mparentCalendar;
    set => this.mparentCalendar = value;
  }

  protected override int VisualChildrenCount => 1;

  protected internal void SetWeekNumbers(List<WeekNumberCell> list)
  {
    if (this.ParentCalendar == null)
      this.ParentCalendar = this.Parent as CalendarEdit;
    if (this.ParentCalendar == null)
      return;
    for (int index = 0; index < this.WeekNumberCells.Count && index < list.Count; ++index)
      (this.WeekNumberCells[index] as WeekNumberCell).Content = list[index].Content;
  }

  protected internal void UpdateTemplateAndSelector(
    DataTemplate template,
    DataTemplateSelector selector)
  {
    if (template != null && selector != null)
      throw new ArgumentException("Both template and selector can not be set at one time.");
    foreach (WeekNumberCell weekNumberCell in this.WeekNumberCells)
      weekNumberCell.UpdateCellTemplateAndSelector(template, selector);
  }

  protected internal void UpdateStyles(Style style)
  {
    foreach (WeekNumberCell weekNumberCell in this.WeekNumberCells)
      weekNumberCell.SetStyle(style);
  }

  protected void GenerateGrid()
  {
    foreach (ColumnDefinition columnDefinition in new ArrayList()
    {
      (object) new ColumnDefinition()
      {
        Width = new GridLength(1.0, GridUnitType.Star)
      }
    })
      this.minnerGrid.ColumnDefinitions.Add(columnDefinition);
    for (int index = 0; index < 6; ++index)
      this.minnerGrid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1.0, GridUnitType.Star)
      });
  }

  protected void FillGrid()
  {
    for (int index = 0; index < 6; ++index)
    {
      this.WeekNumberCells.Add((object) new WeekNumberCell());
      Grid.SetRow((UIElement) this.WeekNumberCells[index], index);
      Grid.SetColumn((UIElement) this.WeekNumberCells[index], 0);
      this.minnerGrid.Children.Add((UIElement) this.WeekNumberCells[index]);
    }
  }

  protected void UpdateParent()
  {
    if (this.Parent == null)
      throw new NullReferenceException("Parent is null");
    this.ParentCalendar = this.Parent is CalendarEdit parent ? parent : throw new NotSupportedException("Parent must be inherited from CalendarEdit type.");
  }

  protected override Cell CreateCell() => (Cell) new WeekNumberCell();

  protected override Visual GetVisualChild(int index)
  {
    if (index != 0)
      throw new ArgumentOutOfRangeException("Only one child element is present");
    return (Visual) this.minnerGrid;
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    this.minnerGrid.Measure(availableSize);
    return this.minnerGrid.DesiredSize;
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    this.minnerGrid.Arrange(new Rect(new Point(0.0, 0.0), finalSize));
    return finalSize;
  }

  protected override void OnVisualParentChanged(DependencyObject oldParent)
  {
    base.OnVisualParentChanged(oldParent);
    this.UpdateParent();
  }

  public new void Dispose() => this.Dispose(true);

  protected override void Dispose(bool disposing)
  {
    if (this.mweekNumberCells != null)
    {
      for (int index = 0; index < this.mweekNumberCells.Count; ++index)
        this.mweekNumberCells[index] = (object) null;
      this.mweekNumberCells.Clear();
    }
    if (this.CreateCell() is WeekNumberCell)
      (this.CreateCell() as WeekNumberCell).Dispose();
    if (this.mparentCalendar != null)
      this.mparentCalendar = (CalendarEdit) null;
    if (this.minnerGrid != null)
    {
      this.minnerGrid.Children.Clear();
      this.minnerGrid = (Grid) null;
    }
    this.Loaded -= new RoutedEventHandler(this.WeekNumbersGrid_Loaded);
    base.Dispose(disposing);
  }
}
