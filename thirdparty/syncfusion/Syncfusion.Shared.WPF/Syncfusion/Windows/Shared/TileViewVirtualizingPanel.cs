// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TileViewVirtualizingPanel
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class TileViewVirtualizingPanel : VirtualizingPanel, IScrollInfo
{
  internal int FirstVisibleIndex;
  internal int LastVisibleIndex;
  internal Size m_extentSize = new Size(0.0, 0.0);
  internal Size extent;
  internal bool IsMaximizedButtonClick;
  internal bool isTilesRestore;
  internal int VirtualColumn = 12;
  internal int VirtualRow = 10;
  private ItemsControl parentItemsControl;
  private double scrollStep;
  private Size m_viewportSize = new Size(0.0, 0.0);
  private IScrollInfo m_scrollInfo;
  private ScrollViewer m_scrollOwner;
  private TileViewControl parentTileView;
  private bool m_bCanVerticallyScroll;
  private bool m_bCanHorizontallyScroll;
  private bool isScrollActive;
  private Point m_offsetPoint;
  private Thumb vScrollThumb;
  private Thumb hScrollThumb;
  private bool isThumUp;
  private bool isMouseDown;

  internal TileViewControl ParentTileViewControl
  {
    get
    {
      if (this.parentTileView == null)
        this.parentTileView = VisualUtils.FindAncestor((Visual) this, typeof (TileViewControl)) as TileViewControl;
      return this.parentTileView;
    }
  }

  internal IScrollInfo ScrollInfo
  {
    get
    {
      if (this.m_scrollInfo == null)
        this.m_scrollInfo = (IScrollInfo) this;
      return this.m_scrollInfo;
    }
  }

  bool IScrollInfo.CanHorizontallyScroll
  {
    get => this.m_bCanHorizontallyScroll;
    set => this.m_bCanHorizontallyScroll = value;
  }

  bool IScrollInfo.CanVerticallyScroll
  {
    get => this.m_bCanVerticallyScroll;
    set => this.m_bCanVerticallyScroll = value;
  }

  double IScrollInfo.ExtentHeight => this.m_extentSize.Height;

  double IScrollInfo.ExtentWidth => this.m_extentSize.Width;

  double IScrollInfo.HorizontalOffset => this.m_offsetPoint.X;

  ScrollViewer IScrollInfo.ScrollOwner
  {
    get => this.m_scrollOwner;
    set => this.m_scrollOwner = value;
  }

  double IScrollInfo.VerticalOffset => this.m_offsetPoint.Y;

  double IScrollInfo.ViewportHeight => this.m_viewportSize.Height;

  double IScrollInfo.ViewportWidth => this.m_viewportSize.Width;

  internal void Dispose(bool isDisposable)
  {
    if (!isDisposable)
      return;
    this.m_scrollInfo = (IScrollInfo) null;
    this.m_scrollOwner = (ScrollViewer) null;
    this.parentItemsControl = (ItemsControl) null;
    if (this.vScrollThumb != null)
    {
      this.vScrollThumb.PreviewMouseDown -= new MouseButtonEventHandler(this.scrollThumb_PreviewMouseDown);
      this.vScrollThumb.PreviewMouseUp -= new MouseButtonEventHandler(this.scrollThumb_PreviewMouseUp);
    }
    if (this.hScrollThumb == null)
      return;
    this.hScrollThumb.PreviewMouseDown -= new MouseButtonEventHandler(this.scrollThumb_PreviewMouseDown);
    this.hScrollThumb.PreviewMouseUp -= new MouseButtonEventHandler(this.scrollThumb_PreviewMouseUp);
  }

  protected override void OnInitialized(EventArgs e)
  {
    base.OnInitialized(e);
    this.parentItemsControl = ItemsControl.GetItemsOwner((DependencyObject) this);
    this.m_scrollOwner = (this.parentItemsControl as TileViewControl).ScrollHost;
    if (this.ParentTileViewControl == null)
      return;
    if (this.ParentTileViewControl.RowCount != 0)
      this.VirtualRow = this.ParentTileViewControl.RowCount;
    if (this.ParentTileViewControl.ColumnCount == 0)
      return;
    this.VirtualColumn = this.ParentTileViewControl.ColumnCount;
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    UIElementCollection internalChildren = this.InternalChildren;
    IItemContainerGenerator containerGenerator = this.ItemContainerGenerator;
    if (this.ParentTileViewControl != null && this.ParentTileViewControl.IsVirtualizing && this.ParentTileViewControl.Items != null && this.ParentTileViewControl.Items.Count <= 1 && this.InternalChildren != null)
    {
      foreach (UIElement internalChild in this.InternalChildren)
      {
        if (internalChild != null)
        {
          TileViewItem element = (TileViewItem) internalChild;
          if (element != null)
          {
            Canvas.SetLeft((UIElement) element, 0.0);
            Canvas.SetTop((UIElement) element, 0.0);
          }
        }
      }
    }
    if (this.FirstVisibleIndex < 0)
      this.FirstVisibleIndex = 0;
    int num1 = this.VirtualColumn * this.VirtualRow - 1;
    if (this.ParentTileViewControl.maximizedItem == null && this.FirstVisibleIndex % this.VirtualColumn != 0)
      this.FirstVisibleIndex = Convert.ToInt32(this.FirstVisibleIndex / this.VirtualColumn) * this.VirtualColumn;
    bool flag = false;
    GeneratorPosition position1 = containerGenerator.GeneratorPositionFromIndex(this.FirstVisibleIndex);
    this.UpdateScrollerOffset(this.FirstVisibleIndex, availableSize);
    int index = position1.Offset == 0 ? position1.Index : position1.Index + 1;
    this.ParentTileViewControl.firstVisibleIndex = this.FirstVisibleIndex;
    if (this.ParentTileViewControl.RemoveAtVirtualPostion != -1 && this.ParentTileViewControl.RemoveAtVirtualPostion > this.FirstVisibleIndex && this.ParentTileViewControl.RemoveAtVirtualPostion < num1 + this.FirstVisibleIndex)
    {
      this.RemoveInternalChildRange(this.ParentTileViewControl.RemoveAtVirtualPostion, 1);
      GeneratorPosition position2 = new GeneratorPosition(this.ParentTileViewControl.RemoveAtVirtualPostion, 0);
      containerGenerator.Remove(position2, 1);
      this.ParentTileViewControl.RemoveAtVirtualPostion = -1;
    }
    using (containerGenerator.StartAt(position1, GeneratorDirection.Forward, true))
    {
      int num2 = -1;
      int firstVisibleIndex = this.FirstVisibleIndex;
      while (firstVisibleIndex <= this.parentItemsControl.Items.Count)
      {
        ++num2;
        bool isNewlyRealized;
        if (containerGenerator.GenerateNext(out isNewlyRealized) is UIElement next)
        {
          if (isNewlyRealized)
          {
            if (index >= this.InternalChildren.Count)
              this.AddInternalChild(next);
            else
              this.InsertInternalChild(index, next);
            flag = true;
            containerGenerator.PrepareItemContainer((DependencyObject) next);
          }
          next.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
          this.LastVisibleIndex = firstVisibleIndex;
          if (num2 != num1)
          {
            ++firstVisibleIndex;
            ++index;
          }
          else
            break;
        }
        else
          break;
      }
    }
    this.CleanUpItems(this.FirstVisibleIndex, this.LastVisibleIndex);
    this.ParentTileViewControl.VirtualizingTileItemsCollection = this.InternalChildren;
    if (flag || this.ParentTileViewControl.IsScroll)
    {
      this.ParentTileViewControl.SetRowsAndColumns(this.ParentTileViewControl.GetTileViewItemOrder());
      this.CalculateMinimizedItemSize();
      this.ParentTileViewControl.UpdateTileViewLayout();
    }
    if (this.isTilesRestore)
    {
      this.ParentTileViewControl.SetRowsAndColumns(this.ParentTileViewControl.GetTileViewItemOrder());
      this.CalculateMinimizedItemSize();
      this.ParentTileViewControl.UpdateTileViewLayout(true);
      this.ParentTileViewControl.IsInsertOrRemoveItem = false;
    }
    if (this.ParentTileViewControl.IsInsertOrRemoveItem && !this.ParentTileViewControl.isTileItemsLoaded)
    {
      this.ParentTileViewControl.GetTileViewItemOrder();
      this.ParentTileViewControl.UpdateTileViewLayout();
    }
    if (this.ParentTileViewControl.ActualHeight != 0.0 && this.ParentTileViewControl.ActualHeight != double.NaN && this.ParentTileViewControl.RowHeight.Value != 0.0 && availableSize.Height > this.ParentTileViewControl.ActualHeight)
      availableSize.Height = this.ParentTileViewControl.ActualHeight;
    if (this.ParentTileViewControl.ActualWidth != 0.0 && this.ParentTileViewControl.ActualWidth != double.NaN && this.ParentTileViewControl.ColumnWidth.Value != 0.0 && availableSize.Width > this.ParentTileViewControl.ActualWidth)
      availableSize.Width = this.ParentTileViewControl.ActualWidth - 17.0;
    this.UpdateScrollInfo(availableSize);
    return base.MeasureOverride(availableSize);
  }

  private void CalculateMinimizedItemSize()
  {
    if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
      this.ParentTileViewControl.MinimizedVirtualItemSize = this.ParentTileViewControl.ActualHeight / (double) this.ParentTileViewControl.Rows;
    else
      this.ParentTileViewControl.MinimizedVirtualItemSize = this.ParentTileViewControl.ActualWidth / (double) this.ParentTileViewControl.Columns;
  }

  protected override Size ArrangeOverride(Size arrangeSize)
  {
    foreach (UIElement internalChild in this.InternalChildren)
    {
      if (internalChild != null)
      {
        double x = 0.0;
        double y = 0.0;
        double left = Canvas.GetLeft(internalChild);
        if (!double.IsNaN(left))
        {
          x = left;
        }
        else
        {
          if (this.InternalChildren.IndexOf(internalChild) != 0 && !double.IsNaN(Canvas.GetLeft(this.InternalChildren[this.InternalChildren.IndexOf(internalChild) - 1])) && this.ParentTileViewControl.maximizedItem != null)
            x = this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right ? Canvas.GetLeft(this.InternalChildren[this.InternalChildren.IndexOf(internalChild) - 1]) : Canvas.GetLeft(this.InternalChildren[this.InternalChildren.IndexOf(internalChild) - 1]) + this.ParentTileViewControl.MinimizedVirtualItemSize;
          double right = Canvas.GetRight(internalChild);
          if (!double.IsNaN(right))
            x = arrangeSize.Width - internalChild.DesiredSize.Width - right;
        }
        double top = Canvas.GetTop(internalChild);
        if (!double.IsNaN(top))
        {
          y = top;
        }
        else
        {
          if (this.InternalChildren.IndexOf(internalChild) != 0 && !double.IsNaN(Canvas.GetTop(this.InternalChildren[this.InternalChildren.IndexOf(internalChild) - 1])) && this.ParentTileViewControl.maximizedItem != null)
            y = this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right ? Canvas.GetTop(this.InternalChildren[this.InternalChildren.IndexOf(internalChild) - 1]) + this.ParentTileViewControl.MinimizedVirtualItemSize : Canvas.GetTop(this.InternalChildren[this.InternalChildren.IndexOf(internalChild) - 1]);
          double bottom = Canvas.GetBottom(internalChild);
          if (!double.IsNaN(bottom))
            y = arrangeSize.Height - internalChild.DesiredSize.Height - bottom;
        }
        Size size = internalChild.DesiredSize;
        if (this.ParentTileViewControl.maximizedItem != null && internalChild.DesiredSize == new Size(0.0, 0.0) && internalChild as TileViewItem != this.ParentTileViewControl.maximizedItem)
          size = this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right ? new Size(internalChild.DesiredSize.Width, this.ParentTileViewControl.MinimizedVirtualItemSize - (internalChild as TileViewItem).Margin.Bottom) : new Size(this.ParentTileViewControl.MinimizedVirtualItemSize - (internalChild as TileViewItem).Margin.Right, internalChild.DesiredSize.Height);
        if (this.ParentTileViewControl.maximizedItem == null && internalChild.DesiredSize == new Size(0.0, 0.0) && !this.ParentTileViewControl.IsInsertOrRemoveItem && (internalChild as TileViewItem).TileViewItemState == TileViewItemState.Normal)
        {
          TileViewItem tileViewItem = internalChild as TileViewItem;
          size = new Size(Convert.ToDouble(arrangeSize.Width / Convert.ToDouble(this.ParentTileViewControl.Columns) + tileViewItem.Margin.Left), Convert.ToDouble(arrangeSize.Height / Convert.ToDouble(this.ParentTileViewControl.Rows) + tileViewItem.Margin.Top));
        }
        internalChild.Arrange(new Rect(new Point(x, y), size));
      }
    }
    if (this.ParentTileViewControl.maximizedItem == null)
      this.scrollStep = this.ParentTileViewControl.RowHeight.Value == 1.0 ? Convert.ToDouble(arrangeSize.Height / Convert.ToDouble(this.ParentTileViewControl.Rows)) : this.ParentTileViewControl.RowHeight.Value;
    if (this.isThumUp)
    {
      this.isThumUp = false;
      this.InvalidateMeasure();
    }
    if (this.isTilesRestore || this.ParentTileViewControl.IsSizechanged)
    {
      this.InvalidateMeasure();
      this.ParentTileViewControl.IsSizechanged = false;
      this.isTilesRestore = false;
    }
    if (this.ParentTileViewControl.IsInsertOrRemoveItem && !this.ParentTileViewControl.isTileItemsLoaded)
    {
      this.isTilesRestore = true;
      this.ParentTileViewControl.Timer.Start();
    }
    if (this.ParentTileViewControl.IsScroll)
    {
      this.InvalidateMeasure();
      this.ParentTileViewControl.IsScroll = false;
    }
    return arrangeSize;
  }

  private void UpdateScrollerOffset(int firstVisibleIndex, Size availableSize)
  {
    if (this.ParentTileViewControl.maximizedItem == null && (this.isTilesRestore || this.ParentTileViewControl.IsSizechanged))
    {
      this.UpdateScrollInfo(availableSize);
      this.ScrollInfo.SetVerticalOffset((double) (firstVisibleIndex / this.ParentTileViewControl.Columns) * Convert.ToDouble(availableSize.Height / Convert.ToDouble(this.ParentTileViewControl.Rows)));
      this.ScrollInfo.SetHorizontalOffset(0.0);
    }
    else
    {
      if (this.ParentTileViewControl.maximizedItem == null || !this.IsMaximizedButtonClick)
        return;
      this.UpdateScrollInfo(availableSize);
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
        this.ScrollInfo.SetVerticalOffset((double) firstVisibleIndex * this.ParentTileViewControl.MinimizedVirtualItemSize);
      else
        this.ScrollInfo.SetHorizontalOffset((double) firstVisibleIndex * this.ParentTileViewControl.MinimizedVirtualItemSize);
      this.isScrollActive = false;
      this.IsMaximizedButtonClick = false;
    }
  }

  private void CleanUpItems(int firstVisibleItemIndex, int lastVisibleItemIndex)
  {
    UIElementCollection internalChildren = this.InternalChildren;
    IItemContainerGenerator containerGenerator = this.ItemContainerGenerator;
    if (this.ParentTileViewControl.RemoveAtVirtualPostion != -1 && this.ParentTileViewControl.RemoveAtVirtualPostion >= firstVisibleItemIndex && this.ParentTileViewControl.RemoveAtVirtualPostion <= lastVisibleItemIndex + 1)
    {
      this.RemoveInternalChildRange(this.ParentTileViewControl.RemoveAtVirtualPostion, 1);
      GeneratorPosition generatorPosition = new GeneratorPosition(this.ParentTileViewControl.RemoveAtVirtualPostion, 0);
      this.ParentTileViewControl.RemoveAtVirtualPostion = -1;
      this.ParentTileViewControl.IsScroll = true;
    }
    for (int index = internalChildren.Count - 1; index >= 0; --index)
    {
      GeneratorPosition position = new GeneratorPosition(index, 0);
      int num = containerGenerator.IndexFromGeneratorPosition(position);
      if (num >= 0 && (num < firstVisibleItemIndex || num > lastVisibleItemIndex))
      {
        TileViewItem tileViewItem = internalChildren[index] as TileViewItem;
        if (this.ParentTileViewControl.maximizedItem == null || tileViewItem != this.ParentTileViewControl.maximizedItem)
        {
          containerGenerator.Remove(position, 1);
          this.RemoveInternalChildRange(index, 1);
          tileViewItem.Dispose();
        }
      }
    }
  }

  internal void RemoveDraggedItems(int oldIndex, int newIndex)
  {
    UIElementCollection internalChildren = this.InternalChildren;
    IItemContainerGenerator containerGenerator = this.ItemContainerGenerator;
    List<int> intList = new List<int>();
    for (int index = internalChildren.Count - 1; index >= 0; --index)
    {
      GeneratorPosition position = new GeneratorPosition(index, 0);
      int num = containerGenerator.IndexFromGeneratorPosition(position);
      if (num == newIndex || num == oldIndex)
        intList.Add(index);
    }
    foreach (int index in intList)
    {
      GeneratorPosition position = new GeneratorPosition(index, 0);
      containerGenerator.Remove(position, 1);
      this.RemoveInternalChildRange(index, 1);
    }
  }

  private void UpdateScrollInfo(Size availableSize)
  {
    this.ScrollInfo.ScrollOwner.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
    this.ScrollInfo.ScrollOwner.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
    double num1 = 0.0;
    int result = 90;
    try
    {
      int.TryParse(Math.Ceiling(Convert.ToDouble(availableSize.Width / (double) this.ParentTileViewControl.Columns)).ToString(), out result);
    }
    catch (Exception ex)
    {
      Console.WriteLine((object) ex);
    }
    int num2 = 0;
    if (this.ParentTileViewControl.Columns != 0)
    {
      num2 = this.parentTileView.Items.Count / this.ParentTileViewControl.Columns;
      if (this.parentTileView.Items.Count % this.ParentTileViewControl.Columns != 0)
        ++num2;
    }
    double num3;
    if (this.parentTileView.maximizedItem == null)
    {
      if (this.ParentTileViewControl.Rows != 0)
        num1 = this.ParentTileViewControl.RowHeight.Value == 1.0 ? (double) num2 * Convert.ToDouble(availableSize.Height / (double) this.ParentTileViewControl.Rows) : (double) num2 * this.ParentTileViewControl.RowHeight.Value;
      num3 = this.ParentTileViewControl.ColumnWidth.Value == 1.0 ? availableSize.Width : (double) this.ParentTileViewControl.Columns * this.ParentTileViewControl.ColumnWidth.Value;
    }
    else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
    {
      num1 = (double) (this.ParentTileViewControl.Items.Count - 1) * this.ParentTileViewControl.MinimizedVirtualItemSize;
      num3 = availableSize.Width;
    }
    else
    {
      num1 = availableSize.Height;
      num3 = (double) (this.ParentTileViewControl.Items.Count - 1) * this.ParentTileViewControl.MinimizedVirtualItemSize;
    }
    if (num1 > availableSize.Height)
    {
      this.extent.Height = num1;
      this.ParentTileViewControl.RightMargin = 20.0;
    }
    else
    {
      this.extent.Height = 0.0;
      this.ParentTileViewControl.RightMargin = 0.0;
    }
    if (num3 > availableSize.Width)
    {
      this.extent.Width = num3;
      this.ParentTileViewControl.BottomMargin = 20.0;
    }
    else
    {
      this.extent.Width = 0.0;
      this.ParentTileViewControl.BottomMargin = 0.0;
    }
    this.m_extentSize.Height = this.extent.Height;
    this.m_extentSize.Width = this.extent.Width;
    if (this.ScrollInfo.ScrollOwner != null)
      this.ScrollInfo.ScrollOwner.InvalidateScrollInfo();
    if (availableSize != this.m_viewportSize)
    {
      double num4 = availableSize.Height - this.m_viewportSize.Height;
      double num5 = availableSize.Width - this.m_viewportSize.Width;
      this.m_viewportSize = availableSize;
      if (this.parentItemsControl != null)
      {
        if (this.parentItemsControl != null)
        {
          if (this.m_extentSize.Width > availableSize.Width && this.m_extentSize.Width - availableSize.Width < this.ScrollInfo.HorizontalOffset)
            this.ScrollInfo.SetHorizontalOffset(this.ScrollInfo.HorizontalOffset + num5);
        }
        else
        {
          if (this.m_extentSize.Height - availableSize.Height < this.ScrollInfo.VerticalOffset)
            this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset + num4);
          if (num5 > 0.0 && this.m_extentSize.Width - availableSize.Width < this.ScrollInfo.HorizontalOffset)
            this.ScrollInfo.SetHorizontalOffset(this.ScrollInfo.HorizontalOffset + num5);
        }
      }
      if (this.ScrollInfo.ScrollOwner != null)
        this.ScrollInfo.ScrollOwner.InvalidateScrollInfo();
    }
    ScrollBar name1 = this.ScrollInfo.ScrollOwner.Template.FindName("PART_VerticalScrollBar", (FrameworkElement) this.ScrollInfo.ScrollOwner) as ScrollBar;
    ScrollBar name2 = this.ScrollInfo.ScrollOwner.Template.FindName("PART_HorizontalScrollBar", (FrameworkElement) this.ScrollInfo.ScrollOwner) as ScrollBar;
    Track descendant1 = VisualUtils.FindDescendant((Visual) name1, typeof (Track)) as Track;
    Track descendant2 = VisualUtils.FindDescendant((Visual) name2, typeof (Track)) as Track;
    if (descendant1 != null && this.vScrollThumb == null)
    {
      this.vScrollThumb = VisualUtils.FindDescendant((Visual) descendant1, typeof (Thumb)) as Thumb;
      if (this.vScrollThumb != null)
      {
        this.vScrollThumb.PreviewMouseDown += new MouseButtonEventHandler(this.scrollThumb_PreviewMouseDown);
        this.vScrollThumb.PreviewMouseUp += new MouseButtonEventHandler(this.scrollThumb_PreviewMouseUp);
      }
    }
    if (descendant2 == null || this.hScrollThumb != null)
      return;
    this.hScrollThumb = VisualUtils.FindDescendant((Visual) descendant2, typeof (Thumb)) as Thumb;
    if (this.hScrollThumb == null)
      return;
    this.hScrollThumb.PreviewMouseDown += new MouseButtonEventHandler(this.scrollThumb_PreviewMouseDown);
    this.hScrollThumb.PreviewMouseUp += new MouseButtonEventHandler(this.scrollThumb_PreviewMouseUp);
  }

  private void scrollThumb_PreviewMouseUp(object sender, MouseButtonEventArgs e)
  {
    this.isScrollActive = true;
    if (this.ParentTileViewControl.maximizedItem != null)
    {
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
      {
        this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset);
        this.CalculateFirstVisibleIndex(this.ScrollInfo.VerticalOffset);
        this.ParentTileViewControl.MinimizedScrollStep = this.ScrollInfo.VerticalOffset - (double) this.FirstVisibleIndex * this.ParentTileViewControl.MinimizedVirtualItemSize;
      }
      else
      {
        this.ScrollInfo.SetHorizontalOffset(this.m_offsetPoint.X);
        this.CalculateFirstVisibleIndex(this.ScrollInfo.HorizontalOffset);
        this.ParentTileViewControl.MinimizedScrollStep = this.ScrollInfo.HorizontalOffset - (double) this.FirstVisibleIndex * this.ParentTileViewControl.MinimizedVirtualItemSize;
      }
      if (this.FirstVisibleIndex != 0)
        this.ParentTileViewControl.MinimizedScrollStep += this.ParentTileViewControl.MinimizedVirtualItemSize;
      this.isThumUp = true;
      this.ParentTileViewControl.IsScroll = true;
    }
    else
    {
      this.FirstVisibleIndex = Convert.ToInt32(this.ScrollInfo.VerticalOffset / this.scrollStep) * this.ParentTileViewControl.Columns;
      this.ParentTileViewControl.IsScroll = true;
      if (this.ScrollInfo.VerticalOffset + this.ScrollInfo.ViewportHeight == this.extent.Height)
      {
        this.CalculateFirstVisibleIndex((double) (Convert.ToInt32(this.ScrollInfo.VerticalOffset / this.scrollStep) * this.ParentTileViewControl.Columns));
        this.ParentTileViewControl.MaximizedScrollStep = (double) this.VirtualRow * this.scrollStep - this.ScrollInfo.ViewportHeight;
        this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset);
      }
      else
      {
        this.ParentTileViewControl.MaximizedScrollStep = 0.0;
        this.ScrollInfo.SetVerticalOffset((double) (this.FirstVisibleIndex / this.ParentTileViewControl.Columns) * this.scrollStep);
      }
      this.ParentTileViewControl.listClear();
    }
    this.isMouseDown = false;
  }

  private void scrollThumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    this.isMouseDown = true;
  }

  private void CalculateFirstVisibleIndex(double offset)
  {
    if (this.ParentTileViewControl.maximizedItem != null)
    {
      this.FirstVisibleIndex = Convert.ToInt32(Math.Floor(Math.Floor(offset / this.ParentTileViewControl.MinimizedVirtualItemSize) / (double) this.ParentTileViewControl.Columns)) * this.ParentTileViewControl.Columns;
      if (this.FirstVisibleIndex + this.ParentTileViewControl.Rows * this.ParentTileViewControl.Columns <= this.ParentTileViewControl.Items.Count)
        return;
      int int32 = Convert.ToInt32(this.ParentTileViewControl.Items.Count / this.ParentTileViewControl.Columns);
      if (this.ParentTileViewControl.Items.Count % this.ParentTileViewControl.Columns != 0)
        ++int32;
      this.FirstVisibleIndex = (int32 - this.ParentTileViewControl.Rows) * this.ParentTileViewControl.Columns;
      if (this.FirstVisibleIndex % this.ParentTileViewControl.Columns == 0)
        return;
      this.FirstVisibleIndex = this.FirstVisibleIndex / this.ParentTileViewControl.Columns * this.ParentTileViewControl.Columns;
    }
    else
    {
      this.FirstVisibleIndex = Convert.ToInt32(this.ScrollInfo.VerticalOffset / this.scrollStep) * this.ParentTileViewControl.Columns;
      if (this.FirstVisibleIndex + this.ParentTileViewControl.Rows * this.ParentTileViewControl.Columns <= this.ParentTileViewControl.Items.Count)
        return;
      int int32 = Convert.ToInt32(this.ParentTileViewControl.Items.Count / this.ParentTileViewControl.Columns);
      if (this.ParentTileViewControl.Items.Count % this.ParentTileViewControl.Columns != 0)
        ++int32;
      this.FirstVisibleIndex = (int32 - this.ParentTileViewControl.Rows) * this.ParentTileViewControl.Columns;
      if (this.FirstVisibleIndex % this.ParentTileViewControl.Columns == 0)
        return;
      this.FirstVisibleIndex = this.FirstVisibleIndex / this.ParentTileViewControl.Columns * this.ParentTileViewControl.Columns;
    }
  }

  void IScrollInfo.LineDown()
  {
    if (this.ParentTileViewControl.maximizedItem == null)
    {
      if (this.ScrollInfo.VerticalOffset + this.scrollStep + this.ParentTileViewControl.ScrollHost.ViewportHeight <= this.extent.Height)
      {
        this.FirstVisibleIndex += this.ParentTileViewControl.Columns;
        this.ParentTileViewControl.IsScroll = true;
        this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset + this.scrollStep);
      }
      else if (this.ScrollInfo.VerticalOffset + this.ParentTileViewControl.ScrollHost.ViewportHeight < this.extent.Height)
      {
        this.ParentTileViewControl.IsScroll = true;
        this.ParentTileViewControl.MaximizedScrollStep = this.extent.Height - this.ParentTileViewControl.ScrollHost.ViewportHeight - this.ScrollInfo.VerticalOffset;
        this.ScrollInfo.SetVerticalOffset(this.extent.Height);
      }
    }
    else
    {
      this.ParentTileViewControl.IsScroll = true;
      if (this.ScrollInfo.VerticalOffset + 10.0 + this.ScrollInfo.ViewportHeight > this.extent.Height)
        return;
      this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset + 10.0);
      foreach (UIElement internalChild in this.InternalChildren)
      {
        if (this.LastVisibleIndex != this.ParentTileViewControl.Items.Count - 1)
        {
          if (this.scrollStep <= this.ParentTileViewControl.MinimizedScrollStep)
          {
            this.ParentTileViewControl.MinimizedScrollStep = 0.0;
            this.FirstVisibleIndex += this.ParentTileViewControl.Columns;
            break;
          }
        }
        else
          break;
      }
      if (this.ScrollInfo.VerticalOffset + 10.0 + this.ScrollInfo.ViewportHeight > this.extent.Height)
        return;
      this.ParentTileViewControl.MinimizedScrollStep += 10.0;
    }
    this.isScrollActive = true;
  }

  void IScrollInfo.LineLeft()
  {
    if (this.ParentTileViewControl.maximizedItem == null)
    {
      this.ScrollInfo.SetHorizontalOffset(this.ScrollInfo.HorizontalOffset - this.scrollStep);
      this.ParentTileViewControl.IsScroll = true;
    }
    else
    {
      if (this.ScrollInfo.HorizontalOffset == 0.0)
        return;
      this.ParentTileViewControl.IsScroll = true;
      if (this.FirstVisibleIndex == 0)
        return;
      this.ScrollInfo.SetHorizontalOffset(this.ScrollInfo.HorizontalOffset - 10.0);
      foreach (UIElement internalChild in this.InternalChildren)
      {
        if (this.ParentTileViewControl.MinimizedScrollStep == 0.0)
        {
          this.FirstVisibleIndex -= this.ParentTileViewControl.Columns;
          this.ParentTileViewControl.MinimizedScrollStep = this.ParentTileViewControl.ItemContainerGenerator.IndexFromContainer((DependencyObject) this.ParentTileViewControl.maximizedItem) < this.FirstVisibleIndex || this.ParentTileViewControl.ItemContainerGenerator.IndexFromContainer((DependencyObject) this.ParentTileViewControl.maximizedItem) > this.FirstVisibleIndex + this.ParentTileViewControl.Columns ? (double) this.ParentTileViewControl.Columns * this.ParentTileViewControl.MinimizedVirtualItemSize : (double) (this.ParentTileViewControl.Columns - 1) * this.ParentTileViewControl.MinimizedVirtualItemSize;
          break;
        }
      }
    }
    this.ParentTileViewControl.MinimizedScrollStep -= 10.0;
    this.isScrollActive = true;
  }

  void IScrollInfo.LineRight()
  {
    if (this.ParentTileViewControl.maximizedItem == null)
    {
      this.ParentTileViewControl.IsScroll = true;
      this.ScrollInfo.SetHorizontalOffset(this.ScrollInfo.HorizontalOffset + this.scrollStep);
    }
    else
    {
      if (this.ScrollInfo.HorizontalOffset + 10.0 + this.ScrollInfo.ViewportWidth > this.extent.Width)
        return;
      this.ParentTileViewControl.IsScroll = true;
      this.ScrollInfo.SetHorizontalOffset(this.ScrollInfo.HorizontalOffset + 10.0);
      foreach (UIElement internalChild in this.InternalChildren)
      {
        if (this.LastVisibleIndex != this.ParentTileViewControl.Items.Count - 1)
        {
          if (this.scrollStep <= this.ParentTileViewControl.MinimizedScrollStep)
          {
            this.ParentTileViewControl.MinimizedScrollStep = 0.0;
            this.FirstVisibleIndex += this.ParentTileViewControl.Columns;
            break;
          }
        }
        else
          break;
      }
    }
    this.ParentTileViewControl.MinimizedScrollStep += 10.0;
    this.isScrollActive = true;
  }

  void IScrollInfo.LineUp()
  {
    if (this.ParentTileViewControl.maximizedItem == null)
    {
      if (this.ParentTileViewControl.MaximizedScrollStep != 0.0)
      {
        this.ParentTileViewControl.IsScroll = true;
        this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset - this.ParentTileViewControl.MaximizedScrollStep);
        this.ParentTileViewControl.MaximizedScrollStep = 0.0;
      }
      else if (Convert.ToInt32(this.ScrollInfo.VerticalOffset) - Convert.ToInt32(this.scrollStep) >= 0)
      {
        this.FirstVisibleIndex -= this.ParentTileViewControl.Columns;
        this.ParentTileViewControl.IsScroll = true;
        this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset - this.scrollStep);
      }
    }
    else
    {
      this.ParentTileViewControl.IsScroll = true;
      this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset - 10.0);
      foreach (UIElement internalChild in this.InternalChildren)
      {
        if (this.FirstVisibleIndex != 0)
        {
          if (this.ParentTileViewControl.MinimizedScrollStep < 0.0)
          {
            this.FirstVisibleIndex -= this.ParentTileViewControl.Columns;
            this.ParentTileViewControl.MinimizedScrollStep = this.ParentTileViewControl.ItemContainerGenerator.IndexFromContainer((DependencyObject) this.ParentTileViewControl.maximizedItem) < this.FirstVisibleIndex || this.ParentTileViewControl.ItemContainerGenerator.IndexFromContainer((DependencyObject) this.ParentTileViewControl.maximizedItem) > this.FirstVisibleIndex + this.ParentTileViewControl.Columns ? (double) this.ParentTileViewControl.Columns * this.ParentTileViewControl.MinimizedVirtualItemSize : (double) (this.ParentTileViewControl.Columns - 1) * this.ParentTileViewControl.MinimizedVirtualItemSize;
            break;
          }
        }
        else
          break;
      }
      if (this.ScrollInfo.VerticalOffset > 0.0)
        this.ParentTileViewControl.MinimizedScrollStep -= 10.0;
      else
        this.ParentTileViewControl.MinimizedScrollStep = 0.0;
    }
    this.isScrollActive = true;
  }

  Rect IScrollInfo.MakeVisible(Visual visual, Rect rectangle) => rectangle;

  void IScrollInfo.MouseWheelDown()
  {
    if (this.ParentTileViewControl.maximizedItem == null)
    {
      if (this.ScrollInfo.VerticalOffset + this.scrollStep + this.ParentTileViewControl.ScrollHost.ViewportHeight <= this.extent.Height)
      {
        this.FirstVisibleIndex += this.ParentTileViewControl.Columns;
        this.ParentTileViewControl.IsScroll = true;
        this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset + this.scrollStep);
      }
      else if (this.ScrollInfo.VerticalOffset + this.ParentTileViewControl.ScrollHost.ViewportHeight < this.extent.Height)
      {
        this.ParentTileViewControl.IsScroll = true;
        if (this.LastVisibleIndex != this.ParentTileViewControl.Items.Count - 1)
          this.FirstVisibleIndex += this.ParentTileViewControl.Columns;
        else
          this.ParentTileViewControl.MaximizedScrollStep = this.extent.Height - this.ParentTileViewControl.ScrollHost.ViewportHeight - this.ScrollInfo.VerticalOffset;
        this.ScrollInfo.SetVerticalOffset(this.extent.Height);
      }
    }
    else if (this.ParentTileViewControl.maximizedItem != null)
    {
      this.ParentTileViewControl.IsScroll = true;
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
      {
        if (this.ScrollInfo.VerticalOffset + 10.0 + this.ScrollInfo.ViewportHeight > this.extent.Height)
          return;
        this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset + 10.0);
      }
      else
      {
        if (this.ScrollInfo.HorizontalOffset + 10.0 + this.ScrollInfo.ViewportWidth > this.extent.Width)
          return;
        this.ScrollInfo.SetHorizontalOffset(this.ScrollInfo.HorizontalOffset + 10.0);
      }
      foreach (UIElement internalChild in this.InternalChildren)
      {
        if (this.LastVisibleIndex != this.ParentTileViewControl.Items.Count - 1)
        {
          if ((this.ParentTileViewControl.ItemContainerGenerator.IndexFromContainer((DependencyObject) this.ParentTileViewControl.maximizedItem) < this.FirstVisibleIndex || this.ParentTileViewControl.ItemContainerGenerator.IndexFromContainer((DependencyObject) this.ParentTileViewControl.maximizedItem) > this.FirstVisibleIndex + this.ParentTileViewControl.Columns ? (double) this.ParentTileViewControl.Columns * this.ParentTileViewControl.MinimizedVirtualItemSize : (double) (this.ParentTileViewControl.Columns - 1) * this.ParentTileViewControl.MinimizedVirtualItemSize) <= this.ParentTileViewControl.MinimizedScrollStep)
          {
            this.ParentTileViewControl.MinimizedScrollStep = 0.0;
            this.FirstVisibleIndex += this.ParentTileViewControl.Columns;
            break;
          }
        }
        else
          break;
      }
      this.ParentTileViewControl.MinimizedScrollStep += 10.0;
    }
    this.isScrollActive = true;
  }

  void IScrollInfo.MouseWheelLeft()
  {
    this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset - this.ScrollInfo.ViewportHeight);
    this.isScrollActive = true;
  }

  void IScrollInfo.MouseWheelRight()
  {
    this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset + this.ScrollInfo.ViewportHeight);
    this.isScrollActive = true;
  }

  void IScrollInfo.MouseWheelUp()
  {
    if (this.ParentTileViewControl.maximizedItem == null)
    {
      if (this.ParentTileViewControl.MaximizedScrollStep != 0.0)
      {
        this.ParentTileViewControl.IsScroll = true;
        this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset - this.ParentTileViewControl.MaximizedScrollStep);
        this.ParentTileViewControl.MaximizedScrollStep = 0.0;
      }
      else if (Convert.ToInt32(this.ScrollInfo.VerticalOffset) - Convert.ToInt32(this.scrollStep) >= 0)
      {
        this.FirstVisibleIndex -= this.ParentTileViewControl.Columns;
        this.ParentTileViewControl.IsScroll = true;
        this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset - this.scrollStep);
      }
    }
    else if (this.ParentTileViewControl.maximizedItem != null)
    {
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
        this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset - 10.0);
      else
        this.ScrollInfo.SetHorizontalOffset(this.ScrollInfo.HorizontalOffset - 10.0);
      this.ParentTileViewControl.IsScroll = true;
      foreach (UIElement internalChild in this.InternalChildren)
      {
        if (this.ParentTileViewControl.MinimizedScrollStep < 10.0)
        {
          this.FirstVisibleIndex -= this.ParentTileViewControl.Columns;
          this.ParentTileViewControl.MinimizedScrollStep = this.ParentTileViewControl.ItemContainerGenerator.IndexFromContainer((DependencyObject) this.ParentTileViewControl.maximizedItem) < this.FirstVisibleIndex || this.ParentTileViewControl.ItemContainerGenerator.IndexFromContainer((DependencyObject) this.ParentTileViewControl.maximizedItem) > this.FirstVisibleIndex + this.ParentTileViewControl.Columns ? (double) this.ParentTileViewControl.Columns * this.ParentTileViewControl.MinimizedVirtualItemSize : (double) (this.ParentTileViewControl.Columns - 1) * this.ParentTileViewControl.MinimizedVirtualItemSize;
          break;
        }
      }
      if (this.ScrollInfo.VerticalOffset > 0.0 || this.ScrollInfo.HorizontalOffset > 0.0)
        this.ParentTileViewControl.MinimizedScrollStep -= 10.0;
      else
        this.ParentTileViewControl.MinimizedScrollStep = 0.0;
    }
    this.isScrollActive = true;
  }

  void IScrollInfo.PageDown()
  {
    if (this.ParentTileViewControl.maximizedItem == null)
    {
      if (this.ScrollInfo.VerticalOffset + this.ScrollInfo.ViewportHeight + this.ParentTileViewControl.ScrollHost.ViewportHeight <= this.extent.Height)
      {
        if (this.LastVisibleIndex != this.ParentTileViewControl.Items.Count - 1)
          this.FirstVisibleIndex = Convert.ToInt32((this.ScrollInfo.VerticalOffset + this.ScrollInfo.ViewportHeight) / this.scrollStep * (double) this.ParentTileViewControl.Columns);
        this.ParentTileViewControl.IsScroll = true;
        this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset + this.ScrollInfo.ViewportHeight);
      }
      else if (this.ScrollInfo.VerticalOffset + this.ScrollInfo.ViewportHeight < this.extent.Height)
      {
        if (this.LastVisibleIndex != this.ParentTileViewControl.Items.Count - 1)
          this.FirstVisibleIndex = Convert.ToInt32((this.ScrollInfo.VerticalOffset + this.ScrollInfo.ViewportHeight) / this.scrollStep * (double) this.ParentTileViewControl.Columns);
        this.ParentTileViewControl.IsScroll = true;
        this.ParentTileViewControl.MaximizedScrollStep = Math.Ceiling(Convert.ToDouble(this.LastVisibleIndex - this.FirstVisibleIndex) / (double) this.VirtualColumn) * this.scrollStep - this.ScrollInfo.ViewportHeight;
        this.ScrollInfo.SetVerticalOffset(this.extent.Height);
      }
    }
    else if (this.ParentTileViewControl.maximizedItem != null)
    {
      this.ParentTileViewControl.IsScroll = true;
      int firstVisibleIndex = this.FirstVisibleIndex;
      this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset + this.ScrollInfo.ViewportHeight);
      this.CalculateFirstVisibleIndex(this.ScrollInfo.VerticalOffset + this.ScrollInfo.ViewportHeight);
      if (firstVisibleIndex == this.FirstVisibleIndex)
        this.ParentTileViewControl.MinimizedScrollStep = this.ScrollInfo.ViewportHeight;
      this.ParentTileViewControl.IsScroll = true;
    }
    this.isScrollActive = true;
  }

  void IScrollInfo.PageLeft()
  {
    if (this.ParentTileViewControl.maximizedItem != null)
    {
      this.ScrollInfo.SetHorizontalOffset(this.ScrollInfo.HorizontalOffset - this.ScrollInfo.ViewportWidth);
      this.CalculateFirstVisibleIndex(this.ScrollInfo.HorizontalOffset + this.ScrollInfo.ViewportHeight);
      this.ParentTileViewControl.IsScroll = true;
    }
    else
    {
      this.ScrollInfo.SetHorizontalOffset(this.ScrollInfo.HorizontalOffset - this.ScrollInfo.ViewportWidth);
      this.ParentTileViewControl.IsScroll = true;
    }
    this.isScrollActive = true;
  }

  void IScrollInfo.PageRight()
  {
    if (this.ParentTileViewControl.maximizedItem != null)
    {
      this.ScrollInfo.SetHorizontalOffset(this.ScrollInfo.HorizontalOffset + this.ScrollInfo.ViewportWidth);
      this.CalculateFirstVisibleIndex(this.ScrollInfo.HorizontalOffset + this.ScrollInfo.ViewportHeight);
      this.ParentTileViewControl.IsScroll = true;
    }
    else
    {
      this.ScrollInfo.SetHorizontalOffset(this.ScrollInfo.HorizontalOffset + this.ScrollInfo.ViewportWidth);
      this.ParentTileViewControl.IsScroll = true;
    }
    this.isScrollActive = true;
  }

  void IScrollInfo.PageUp()
  {
    if (this.ParentTileViewControl.maximizedItem == null)
    {
      this.ParentTileViewControl.MaximizedScrollStep = 0.0;
      if (this.ScrollInfo.VerticalOffset - this.ScrollInfo.ViewportHeight > 0.0)
      {
        this.FirstVisibleIndex = Convert.ToInt32((this.ScrollInfo.VerticalOffset - this.ScrollInfo.ViewportHeight) / this.scrollStep * (double) this.ParentTileViewControl.Columns);
        this.ParentTileViewControl.IsScroll = true;
        this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset - this.ScrollInfo.ViewportHeight);
      }
      else
      {
        this.FirstVisibleIndex = 0;
        this.ParentTileViewControl.IsScroll = true;
        this.ScrollInfo.SetVerticalOffset(0.0);
      }
    }
    else
    {
      this.ScrollInfo.SetVerticalOffset(this.ScrollInfo.VerticalOffset - this.ScrollInfo.ViewportHeight);
      if (this.ParentTileViewControl.maximizedItem != null)
      {
        this.ParentTileViewControl.IsScroll = true;
        this.CalculateFirstVisibleIndex(this.ScrollInfo.VerticalOffset + this.ScrollInfo.ViewportHeight);
      }
      this.ParentTileViewControl.IsScroll = true;
    }
    this.isScrollActive = true;
  }

  void IScrollInfo.SetHorizontalOffset(double offset)
  {
    if (offset < 0.0 || this.ScrollInfo.ViewportWidth >= this.ScrollInfo.ExtentWidth)
      offset = 0.0;
    else if (offset + this.ScrollInfo.ViewportWidth >= this.ScrollInfo.ExtentWidth)
      offset = this.ScrollInfo.ExtentWidth - this.ScrollInfo.ViewportWidth;
    if (this.ParentTileViewControl.maximizedItem != null && this.isScrollActive && this.IsMaximizedButtonClick)
    {
      if (this.FirstVisibleIndex != 0 && offset == 0.0)
        offset = (double) this.FirstVisibleIndex * this.ParentTileViewControl.MinimizedVirtualItemSize;
      this.isScrollActive = false;
      this.IsMaximizedButtonClick = false;
    }
    this.m_offsetPoint.X = offset;
    if (this.ScrollInfo.ScrollOwner != null)
      this.ScrollInfo.ScrollOwner.InvalidateScrollInfo();
    this.InvalidateMeasure();
  }

  void IScrollInfo.SetVerticalOffset(double offset)
  {
    if (!this.isMouseDown)
    {
      if (offset < 0.0 || this.extent.Height <= this.ScrollInfo.ViewportHeight)
        offset = 0.0;
      else if (offset + this.ScrollInfo.ViewportHeight >= this.extent.Height)
        offset = this.extent.Height - this.ScrollInfo.ViewportHeight;
      else if (this.ParentTileViewControl.maximizedItem == null)
        this.ParentTileViewControl.listClear();
    }
    if (this.ParentTileViewControl.maximizedItem != null && this.isScrollActive && this.IsMaximizedButtonClick)
    {
      if (this.FirstVisibleIndex != 0 && offset == 0.0)
        offset = (double) this.FirstVisibleIndex * this.ParentTileViewControl.MinimizedVirtualItemSize;
      this.isScrollActive = false;
      this.IsMaximizedButtonClick = false;
    }
    this.m_offsetPoint.Y = offset;
    if (this.ParentTileViewControl.ScrollHost != null)
      this.ParentTileViewControl.ScrollHost.InvalidateScrollInfo();
    this.InvalidateMeasure();
  }
}
