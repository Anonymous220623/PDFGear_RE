// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CalendarEditGrid
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
public abstract class CalendarEditGrid : FrameworkElement, IDisposable
{
  private Grid minnerGrid;
  private int mcolumnsCount;
  private int mrowsCount;
  private ArrayList mcellsCollection;
  private int mfocusedCellIndex;

  public CalendarEditGrid(int rowsCount, int columnsCount)
  {
    this.minnerGrid = new Grid();
    this.minnerGrid.SnapsToDevicePixels = true;
    this.RowsCount = rowsCount;
    this.ColumnsCount = columnsCount;
    this.CellsCollection = new ArrayList();
    this.GenerateGrid();
    this.FillGrid();
    this.Loaded += new RoutedEventHandler(this.CalendarEditGrid_Loaded);
    this.AddLogicalChild((object) this.minnerGrid);
    this.AddVisualChild((Visual) this.minnerGrid);
  }

  private void CalendarEditGrid_Loaded(object sender, RoutedEventArgs e)
  {
    BindingUtils.SetRelativeBinding((DependencyObject) this.minnerGrid, FrameworkElement.FlowDirectionProperty, typeof (CalendarEdit), (object) FrameworkElement.FlowDirectionProperty, BindingMode.OneWay, 1);
  }

  protected internal int FocusedCellIndex
  {
    get => this.mfocusedCellIndex;
    set
    {
      if (this.mfocusedCellIndex == value)
        return;
      this.mfocusedCellIndex = value;
    }
  }

  protected internal int RowsCount
  {
    get => this.mrowsCount;
    set => this.mrowsCount = value;
  }

  protected internal int ColumnsCount
  {
    get => this.mcolumnsCount;
    set => this.mcolumnsCount = value;
  }

  protected internal ArrayList CellsCollection
  {
    get => this.mcellsCollection;
    set => this.mcellsCollection = value;
  }

  protected override int VisualChildrenCount => 1;

  public virtual void SetIsSelected(VisibleDate date)
  {
  }

  public virtual void Initialize(VisibleDate date, CultureInfo culture, System.Globalization.Calendar calendar)
  {
  }

  protected void AddToInnerGrid(UIElement element) => this.minnerGrid.Children.Add(element);

  protected abstract Cell CreateCell();

  protected override Visual GetVisualChild(int index)
  {
    if (index != 0)
      throw new ArgumentOutOfRangeException("Only one child element is present.");
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

  private void GenerateGrid()
  {
    ArrayList arrayList1 = new ArrayList();
    ArrayList arrayList2 = new ArrayList();
    for (int index = 0; index < this.ColumnsCount; ++index)
      arrayList1.Add((object) new ColumnDefinition()
      {
        Width = new GridLength(1.0, GridUnitType.Star)
      });
    foreach (ColumnDefinition columnDefinition in arrayList1)
      this.minnerGrid.ColumnDefinitions.Add(columnDefinition);
    for (int index = 0; index < this.RowsCount; ++index)
      arrayList2.Add((object) new RowDefinition()
      {
        Height = new GridLength(1.0, GridUnitType.Star)
      });
    foreach (RowDefinition rowDefinition in arrayList2)
      this.minnerGrid.RowDefinitions.Add(rowDefinition);
  }

  private void FillGrid()
  {
    for (int index = 0; index < this.ColumnsCount * this.RowsCount; ++index)
      this.CellsCollection.Add((object) this.CreateCell());
    int num = 0;
    int index1 = 0;
    for (; num < this.RowsCount; ++num)
    {
      for (int index2 = 0; index2 < this.ColumnsCount; ++index2)
      {
        Grid.SetRow((UIElement) this.CellsCollection[index1], num);
        Grid.SetColumn((UIElement) this.CellsCollection[index1], index2);
        this.minnerGrid.Children.Add((UIElement) this.CellsCollection[index1]);
        ++index1;
      }
    }
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    if (this.mcellsCollection != null)
    {
      this.mcellsCollection.Clear();
      this.mcellsCollection = (ArrayList) null;
    }
    if (this.minnerGrid == null)
      return;
    this.RemoveVisualChild((Visual) this.minnerGrid);
    this.minnerGrid.Children.Clear();
    this.minnerGrid = (Grid) null;
  }

  public void Dispose()
  {
    this.Dispose(true);
    this.Loaded -= new RoutedEventHandler(this.CalendarEditGrid_Loaded);
  }
}
