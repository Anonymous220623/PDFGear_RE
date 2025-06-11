// Decompiled with JetBrains decompiler
// Type: WpfToolkit.Controls.VirtualizingPanelBase
// Assembly: VirtualizingWrapPanel, Version=1.5.4.0, Culture=neutral, PublicKeyToken=null
// MVID: E61E2A8E-A00C-4FB4-9D6E-5B7404CFB214
// Assembly location: D:\PDFGear\bin\VirtualizingWrapPanel.dll

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#nullable enable
namespace WpfToolkit.Controls;

public abstract class VirtualizingPanelBase : VirtualizingPanel, IScrollInfo
{
  public static readonly DependencyProperty ScrollLineDeltaProperty = DependencyProperty.Register(nameof (ScrollLineDelta), typeof (double), typeof (VirtualizingPanelBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) 16.0));
  public static readonly DependencyProperty MouseWheelDeltaProperty = DependencyProperty.Register(nameof (MouseWheelDelta), typeof (double), typeof (VirtualizingPanelBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) 48.0));
  public static readonly DependencyProperty ScrollLineDeltaItemProperty = DependencyProperty.Register(nameof (ScrollLineDeltaItem), typeof (int), typeof (VirtualizingPanelBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1));
  public static readonly DependencyProperty MouseWheelDeltaItemProperty = DependencyProperty.Register(nameof (MouseWheelDeltaItem), typeof (int), typeof (VirtualizingPanelBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) 3));
  private DependencyObject? _itemsOwner;
  private IRecyclingItemContainerGenerator? _itemContainerGenerator;

  public ScrollViewer? ScrollOwner { get; set; }

  public bool CanVerticallyScroll { get; set; }

  public bool CanHorizontallyScroll { get; set; }

  protected override bool CanHierarchicallyScrollAndVirtualizeCore => true;

  public double ScrollLineDelta
  {
    get => (double) this.GetValue(VirtualizingPanelBase.ScrollLineDeltaProperty);
    set => this.SetValue(VirtualizingPanelBase.ScrollLineDeltaProperty, (object) value);
  }

  public double MouseWheelDelta
  {
    get => (double) this.GetValue(VirtualizingPanelBase.MouseWheelDeltaProperty);
    set => this.SetValue(VirtualizingPanelBase.MouseWheelDeltaProperty, (object) value);
  }

  public double ScrollLineDeltaItem
  {
    get => (double) (int) this.GetValue(VirtualizingPanelBase.ScrollLineDeltaItemProperty);
    set => this.SetValue(VirtualizingPanelBase.ScrollLineDeltaItemProperty, (object) value);
  }

  public int MouseWheelDeltaItem
  {
    get => (int) this.GetValue(VirtualizingPanelBase.MouseWheelDeltaItemProperty);
    set => this.SetValue(VirtualizingPanelBase.MouseWheelDeltaItemProperty, (object) value);
  }

  protected ScrollUnit ScrollUnit
  {
    get => VirtualizingPanel.GetScrollUnit((DependencyObject) this.ItemsControl);
  }

  protected ScrollDirection MouseWheelScrollDirection { get; set; }

  protected bool IsVirtualizing
  {
    get => VirtualizingPanel.GetIsVirtualizing((DependencyObject) this.ItemsControl);
  }

  protected VirtualizationMode VirtualizationMode
  {
    get => VirtualizingPanel.GetVirtualizationMode((DependencyObject) this.ItemsControl);
  }

  protected bool IsRecycling => this.VirtualizationMode == VirtualizationMode.Recycling;

  protected VirtualizationCacheLength CacheLength { get; private set; }

  protected VirtualizationCacheLengthUnit CacheLengthUnit { get; private set; }

  protected ItemsControl ItemsControl => ItemsControl.GetItemsOwner((DependencyObject) this);

  protected DependencyObject ItemsOwner
  {
    get
    {
      if (this._itemsOwner == null)
        this._itemsOwner = (DependencyObject) typeof (ItemsControl).GetMethod("GetItemsOwnerInternal", BindingFlags.Static | BindingFlags.NonPublic, (Binder) null, new Type[1]
        {
          typeof (DependencyObject)
        }, (ParameterModifier[]) null).Invoke((object) null, new object[1]
        {
          (object) this
        });
      return this._itemsOwner;
    }
  }

  protected ReadOnlyCollection<object?> Items
  {
    get => ((System.Windows.Controls.ItemContainerGenerator) this.ItemContainerGenerator).Items;
  }

  protected IRecyclingItemContainerGenerator ItemContainerGenerator
  {
    get
    {
      if (this._itemContainerGenerator == null)
      {
        UIElementCollection internalChildren = this.InternalChildren;
        this._itemContainerGenerator = (IRecyclingItemContainerGenerator) base.ItemContainerGenerator;
      }
      return this._itemContainerGenerator;
    }
  }

  public double ExtentWidth => this.Extent.Width;

  public double ExtentHeight => this.Extent.Height;

  protected Size Extent { get; private set; } = new Size(0.0, 0.0);

  public double HorizontalOffset => this.Offset.X;

  public double VerticalOffset => this.Offset.Y;

  protected Size Viewport { get; private set; } = new Size(0.0, 0.0);

  public double ViewportWidth => this.Viewport.Width;

  public double ViewportHeight => this.Viewport.Height;

  protected Point Offset { get; private set; } = new Point(0.0, 0.0);

  protected ItemRange ItemRange { get; set; }

  protected virtual void UpdateScrollInfo(Size availableSize, Size extent)
  {
    bool flag = false;
    if (extent != this.Extent)
    {
      this.Extent = extent;
      flag = true;
    }
    if (availableSize != this.Viewport)
    {
      this.Viewport = availableSize;
      flag = true;
    }
    if (this.ViewportHeight != 0.0 && this.VerticalOffset != 0.0 && this.VerticalOffset + this.ViewportHeight + 1.0 >= this.ExtentHeight)
    {
      this.Offset = new Point(this.Offset.X, extent.Height - availableSize.Height);
      flag = true;
    }
    if (this.ViewportWidth != 0.0 && this.HorizontalOffset != 0.0 && this.HorizontalOffset + this.ViewportWidth + 1.0 >= this.ExtentWidth)
    {
      this.Offset = new Point(extent.Width - availableSize.Width, this.Offset.Y);
      flag = true;
    }
    if (!flag)
      return;
    this.ScrollOwner?.InvalidateScrollInfo();
  }

  public virtual Rect MakeVisible(Visual visual, Rect rectangle)
  {
    Point point = visual.TransformToAncestor((Visual) this).Transform(this.Offset);
    double x = 0.0;
    double y1 = 0.0;
    Point offset;
    if (point.X < this.Offset.X)
    {
      x = -(this.Offset.X - point.X);
    }
    else
    {
      double num1 = point.X + rectangle.Width;
      offset = this.Offset;
      double num2 = offset.X + this.Viewport.Width;
      if (num1 > num2)
      {
        double num3 = point.X + rectangle.Width;
        offset = this.Offset;
        double num4 = offset.X + this.Viewport.Width;
        x = num3 - num4;
      }
    }
    double y2 = point.Y;
    offset = this.Offset;
    double y3 = offset.Y;
    if (y2 < y3)
    {
      offset = this.Offset;
      y1 = -(offset.Y - point.Y);
    }
    else
    {
      double num5 = point.Y + rectangle.Height;
      offset = this.Offset;
      double num6 = offset.Y + this.Viewport.Height;
      if (num5 > num6)
      {
        double num7 = point.Y + rectangle.Height;
        offset = this.Offset;
        double num8 = offset.Y + this.Viewport.Height;
        y1 = num7 - num8;
      }
    }
    offset = this.Offset;
    this.SetHorizontalOffset(offset.X + x);
    offset = this.Offset;
    this.SetVerticalOffset(offset.Y + y1);
    double width = Math.Min(rectangle.Width, this.Viewport.Width);
    double height = Math.Min(rectangle.Height, this.Viewport.Height);
    return new Rect(x, y1, width, height);
  }

  protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
  {
    switch (args.Action)
    {
      case NotifyCollectionChangedAction.Remove:
      case NotifyCollectionChangedAction.Replace:
        this.RemoveInternalChildRange(args.Position.Index, args.ItemUICount);
        break;
      case NotifyCollectionChangedAction.Move:
        this.RemoveInternalChildRange(args.OldPosition.Index, args.ItemUICount);
        break;
    }
  }

  protected int GetItemIndexFromChildIndex(int childIndex)
  {
    return this.ItemContainerGenerator.IndexFromGeneratorPosition(this.GetGeneratorPositionFromChildIndex(childIndex));
  }

  protected virtual GeneratorPosition GetGeneratorPositionFromChildIndex(int childIndex)
  {
    return new GeneratorPosition(childIndex, 0);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    HierarchicalVirtualizationConstraints constraints;
    Size extent;
    Size availableSize1;
    if (this.ItemsOwner is IHierarchicalVirtualizationAndScrollInfo itemsOwner)
    {
      constraints = itemsOwner.Constraints;
      Size size = constraints.Viewport.Size;
      Size pixelSize = itemsOwner.HeaderDesiredSizes.PixelSize;
      availableSize = new Size(Math.Max(size.Width - 5.0, 0.0), Math.Max(size.Height - pixelSize.Height, 0.0));
      extent = this.CalculateExtent(availableSize);
      availableSize1 = new Size(extent.Width, extent.Height);
    }
    else
    {
      if (this.ScrollOwner != null && (this.ScrollOwner.VerticalScrollBarVisibility == ScrollBarVisibility.Auto && this.ScrollOwner.ComputedVerticalScrollBarVisibility != Visibility.Visible && this.ViewportHeight < this.ExtentHeight || this.ScrollOwner.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto && this.ScrollOwner.ComputedHorizontalScrollBarVisibility != Visibility.Visible && this.ViewportWidth < this.ExtentWidth))
        return availableSize;
      extent = this.CalculateExtent(availableSize);
      availableSize1 = new Size(Math.Min(availableSize.Width, extent.Width), Math.Min(availableSize.Height, extent.Height));
    }
    if (itemsOwner != null)
    {
      this.Extent = extent;
      constraints = itemsOwner.Constraints;
      this.Offset = constraints.Viewport.Location;
      constraints = itemsOwner.Constraints;
      this.Viewport = constraints.Viewport.Size;
      constraints = itemsOwner.Constraints;
      this.CacheLength = constraints.CacheLength;
      constraints = itemsOwner.Constraints;
      this.CacheLengthUnit = constraints.CacheLengthUnit;
    }
    else
    {
      this.UpdateScrollInfo(availableSize1, extent);
      this.CacheLength = VirtualizingPanel.GetCacheLength(this.ItemsOwner);
      this.CacheLengthUnit = VirtualizingPanel.GetCacheLengthUnit(this.ItemsOwner);
    }
    this.ItemRange = this.UpdateItemRange();
    this.RealizeItems();
    this.VirtualizeItems();
    return availableSize1;
  }

  protected virtual void RealizeItems()
  {
    GeneratorPosition position = this.ItemContainerGenerator.GeneratorPositionFromIndex(this.ItemRange.StartIndex);
    int index = position.Offset == 0 ? position.Index : position.Index + 1;
    using (this.ItemContainerGenerator.StartAt(position, GeneratorDirection.Forward, true))
    {
      int startIndex = this.ItemRange.StartIndex;
      while (startIndex <= this.ItemRange.EndIndex)
      {
        bool isNewlyRealized;
        UIElement next = (UIElement) this.ItemContainerGenerator.GenerateNext(out isNewlyRealized);
        if (isNewlyRealized || !this.InternalChildren.Contains(next))
        {
          if (index >= this.InternalChildren.Count)
            this.AddInternalChild(next);
          else
            this.InsertInternalChild(index, next);
          this.ItemContainerGenerator.PrepareItemContainer((DependencyObject) next);
          next.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        }
        if (next is IHierarchicalVirtualizationAndScrollInfo virtualizationAndScrollInfo)
        {
          virtualizationAndScrollInfo.Constraints = new HierarchicalVirtualizationConstraints(new VirtualizationCacheLength(0.0), VirtualizationCacheLengthUnit.Item, new Rect(0.0, 0.0, this.ViewportWidth, this.ViewportHeight));
          next.Measure(new Size(this.ViewportWidth, this.ViewportHeight));
        }
        ++startIndex;
        ++index;
      }
    }
  }

  protected virtual void VirtualizeItems()
  {
    for (int index = this.InternalChildren.Count - 1; index >= 0; --index)
    {
      GeneratorPosition positionFromChildIndex = this.GetGeneratorPositionFromChildIndex(index);
      if (!this.ItemRange.Contains(this.ItemContainerGenerator.IndexFromGeneratorPosition(positionFromChildIndex)))
      {
        if (this.VirtualizationMode == VirtualizationMode.Recycling)
          this.ItemContainerGenerator.Recycle(positionFromChildIndex, 1);
        else
          this.ItemContainerGenerator.Remove(positionFromChildIndex, 1);
        this.RemoveInternalChildRange(index, 1);
      }
    }
  }

  protected abstract Size CalculateExtent(Size availableSize);

  protected abstract ItemRange UpdateItemRange();

  public void SetVerticalOffset(double offset)
  {
    if (offset < 0.0 || this.Viewport.Height >= this.Extent.Height)
    {
      offset = 0.0;
    }
    else
    {
      double num = offset + this.Viewport.Height;
      Size size = this.Extent;
      double height1 = size.Height;
      if (num >= height1)
      {
        size = this.Extent;
        double height2 = size.Height;
        size = this.Viewport;
        double height3 = size.Height;
        offset = height2 - height3;
      }
    }
    this.Offset = new Point(this.Offset.X, offset);
    this.ScrollOwner?.InvalidateScrollInfo();
    this.InvalidateMeasure();
  }

  public void SetHorizontalOffset(double offset)
  {
    if (offset < 0.0 || this.Viewport.Width >= this.Extent.Width)
    {
      offset = 0.0;
    }
    else
    {
      double num = offset + this.Viewport.Width;
      Size size = this.Extent;
      double width1 = size.Width;
      if (num >= width1)
      {
        size = this.Extent;
        double width2 = size.Width;
        size = this.Viewport;
        double width3 = size.Width;
        offset = width2 - width3;
      }
    }
    this.Offset = new Point(offset, this.Offset.Y);
    this.ScrollOwner?.InvalidateScrollInfo();
    this.InvalidateMeasure();
  }

  protected void ScrollVertical(double amount)
  {
    this.SetVerticalOffset(this.VerticalOffset + amount);
  }

  protected void ScrollHorizontal(double amount)
  {
    this.SetHorizontalOffset(this.HorizontalOffset + amount);
  }

  public void LineUp()
  {
    this.ScrollVertical(this.ScrollUnit == ScrollUnit.Pixel ? -this.ScrollLineDelta : this.GetLineUpScrollAmount());
  }

  public void LineDown()
  {
    this.ScrollVertical(this.ScrollUnit == ScrollUnit.Pixel ? this.ScrollLineDelta : this.GetLineDownScrollAmount());
  }

  public void LineLeft()
  {
    this.ScrollHorizontal(this.ScrollUnit == ScrollUnit.Pixel ? -this.ScrollLineDelta : this.GetLineLeftScrollAmount());
  }

  public void LineRight()
  {
    this.ScrollHorizontal(this.ScrollUnit == ScrollUnit.Pixel ? this.ScrollLineDelta : this.GetLineRightScrollAmount());
  }

  public void MouseWheelUp()
  {
    if (this.MouseWheelScrollDirection == ScrollDirection.Vertical)
      this.ScrollVertical(this.ScrollUnit == ScrollUnit.Pixel ? -this.MouseWheelDelta : this.GetMouseWheelUpScrollAmount());
    else
      this.MouseWheelLeft();
  }

  public void MouseWheelDown()
  {
    if (this.MouseWheelScrollDirection == ScrollDirection.Vertical)
      this.ScrollVertical(this.ScrollUnit == ScrollUnit.Pixel ? this.MouseWheelDelta : this.GetMouseWheelDownScrollAmount());
    else
      this.MouseWheelRight();
  }

  public void MouseWheelLeft()
  {
    this.ScrollHorizontal(this.ScrollUnit == ScrollUnit.Pixel ? -this.MouseWheelDelta : this.GetMouseWheelLeftScrollAmount());
  }

  public void MouseWheelRight()
  {
    this.ScrollHorizontal(this.ScrollUnit == ScrollUnit.Pixel ? this.MouseWheelDelta : this.GetMouseWheelRightScrollAmount());
  }

  public void PageUp()
  {
    this.ScrollVertical(this.ScrollUnit == ScrollUnit.Pixel ? -this.ViewportHeight : this.GetPageUpScrollAmount());
  }

  public void PageDown()
  {
    this.ScrollVertical(this.ScrollUnit == ScrollUnit.Pixel ? this.ViewportHeight : this.GetPageDownScrollAmount());
  }

  public void PageLeft()
  {
    this.ScrollHorizontal(this.ScrollUnit == ScrollUnit.Pixel ? -this.ViewportHeight : this.GetPageLeftScrollAmount());
  }

  public void PageRight()
  {
    this.ScrollHorizontal(this.ScrollUnit == ScrollUnit.Pixel ? this.ViewportHeight : this.GetPageRightScrollAmount());
  }

  protected abstract double GetLineUpScrollAmount();

  protected abstract double GetLineDownScrollAmount();

  protected abstract double GetLineLeftScrollAmount();

  protected abstract double GetLineRightScrollAmount();

  protected abstract double GetMouseWheelUpScrollAmount();

  protected abstract double GetMouseWheelDownScrollAmount();

  protected abstract double GetMouseWheelLeftScrollAmount();

  protected abstract double GetMouseWheelRightScrollAmount();

  protected abstract double GetPageUpScrollAmount();

  protected abstract double GetPageDownScrollAmount();

  protected abstract double GetPageLeftScrollAmount();

  protected abstract double GetPageRightScrollAmount();
}
