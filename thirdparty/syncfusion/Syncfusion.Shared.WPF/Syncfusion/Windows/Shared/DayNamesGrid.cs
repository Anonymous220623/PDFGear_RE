// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DayNamesGrid
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class DayNamesGrid : FrameworkElement, IDisposable
{
  private const int DEFCOLUMNSCOUNT = 7;
  private Grid minnerGrid;
  private CalendarEdit mparentCalendar;
  private ArrayList mdayNameCells;

  static DayNamesGrid()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (DayNamesGrid), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (DayNamesGrid)));
  }

  public DayNamesGrid()
  {
    this.minnerGrid = new Grid();
    this.DayNameCells = new ArrayList();
    this.GenerateGrid();
    this.FillGrid();
    this.AddLogicalChild((object) this.minnerGrid);
    this.AddVisualChild((Visual) this.minnerGrid);
    this.Loaded += new RoutedEventHandler(this.DayNamesGrid_Loaded);
  }

  private void DayNamesGrid_Loaded(object sender, RoutedEventArgs e)
  {
    BindingUtils.SetRelativeBinding((DependencyObject) this.minnerGrid, FrameworkElement.FlowDirectionProperty, typeof (CalendarEdit), (object) FrameworkElement.FlowDirectionProperty, BindingMode.OneWay, 1);
  }

  public ArrayList DayNameCells
  {
    get => this.mdayNameCells;
    set => this.mdayNameCells = value;
  }

  public CalendarEdit ParentCalendar
  {
    get => this.mparentCalendar;
    set => this.mparentCalendar = value;
  }

  protected override int VisualChildrenCount => 1;

  protected internal void SetDayNames(DateTimeFormatInfo format)
  {
    if (this.ParentCalendar == null)
      return;
    int firstDayOfWeek = (int) format.FirstDayOfWeek;
    string[] strArray = !this.ParentCalendar.ShowAbbreviatedDayNames ? format.DayNames : format.ShortestDayNames;
    if (this.DayNameCells == null || this.DayNameCells.Count <= 0)
      return;
    for (int index1 = 0; index1 < this.DayNameCells.Count; ++index1)
    {
      int index2 = (index1 + firstDayOfWeek) % 7;
      (this.DayNameCells[index1] as DayNameCell).Content = (object) strArray[index2];
    }
  }

  protected internal void UpdateTemplateAndSelector(
    DataTemplate template,
    DataTemplateSelector selector)
  {
    if (template != null && selector != null)
      throw new ArgumentException("Both template and selector can not be set at one time.");
    foreach (DayNameCell dayNameCell in this.DayNameCells)
      dayNameCell.UpdateCellTemplateAndSelector(template, selector);
  }

  protected internal void UpdateStyles(Style style)
  {
    foreach (DayNameCell dayNameCell in this.DayNameCells)
      dayNameCell.SetStyle(style);
  }

  protected void GenerateGrid()
  {
    ArrayList arrayList = new ArrayList();
    for (int index = 0; index < 7; ++index)
      arrayList.Add((object) new ColumnDefinition()
      {
        Width = new GridLength(1.0, GridUnitType.Star)
      });
    foreach (ColumnDefinition columnDefinition in arrayList)
      this.minnerGrid.ColumnDefinitions.Add(columnDefinition);
    this.minnerGrid.RowDefinitions.Add(new RowDefinition()
    {
      Height = new GridLength(15.0, GridUnitType.Pixel)
    });
  }

  protected void FillGrid()
  {
    for (int index = 0; index < 7; ++index)
      this.DayNameCells.Add((object) new DayNameCell());
    for (int index = 0; index < this.DayNameCells.Count; ++index)
    {
      Grid.SetRow((UIElement) this.DayNameCells[index], 0);
      Grid.SetColumn((UIElement) this.DayNameCells[index], index);
      this.minnerGrid.Children.Add((UIElement) this.DayNameCells[index]);
    }
  }

  protected void UpdateParent()
  {
    if (this.Parent == null)
      throw new NullReferenceException("Parent is null");
    this.ParentCalendar = this.Parent is CalendarEdit parent ? parent : throw new NotSupportedException("Parent must be inherited from CalendarEdit type.");
  }

  protected override Visual GetVisualChild(int index)
  {
    if (index != 0)
      throw new ArgumentOutOfRangeException("only one child element present");
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
    this.SetDayNames(this.ParentCalendar.Culture.DateTimeFormat);
  }

  internal void Dispose(bool Disposing)
  {
    if (this.mparentCalendar != null)
      this.mparentCalendar = (CalendarEdit) null;
    if (this.DayNameCells != null)
    {
      foreach (DayNameCell dayNameCell in this.DayNameCells)
        dayNameCell.Dispose();
      this.DayNameCells.Clear();
      this.DayNameCells = (ArrayList) null;
    }
    if (this.mdayNameCells != null)
    {
      for (int index = 0; index < this.mdayNameCells.Count; ++index)
        this.mdayNameCells[index] = (object) null;
      this.mdayNameCells.Clear();
    }
    if (this.minnerGrid == null)
      return;
    this.minnerGrid.Children.Clear();
    this.minnerGrid = (Grid) null;
  }

  public void Dispose()
  {
    this.Dispose(true);
    this.Loaded -= new RoutedEventHandler(this.DayNamesGrid_Loaded);
  }
}
