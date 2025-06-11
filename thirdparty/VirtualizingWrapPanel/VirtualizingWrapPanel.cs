// Decompiled with JetBrains decompiler
// Type: WpfToolkit.Controls.VirtualizingWrapPanel
// Assembly: VirtualizingWrapPanel, Version=1.5.4.0, Culture=neutral, PublicKeyToken=null
// MVID: E61E2A8E-A00C-4FB4-9D6E-5B7404CFB214
// Assembly location: D:\PDFGear\bin\VirtualizingWrapPanel.dll

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable enable
namespace WpfToolkit.Controls;

public class VirtualizingWrapPanel : VirtualizingPanelBase
{
  [Obsolete("Use SpacingMode")]
  public static readonly DependencyProperty IsSpacingEnabledProperty = DependencyProperty.Register(nameof (IsSpacingEnabled), typeof (bool), typeof (VirtualizingWrapPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty SpacingModeProperty = DependencyProperty.Register(nameof (SpacingMode), typeof (SpacingMode), typeof (VirtualizingWrapPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) SpacingMode.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (VirtualizingWrapPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure, (PropertyChangedCallback) ((obj, args) => ((VirtualizingWrapPanel) obj).Orientation_Changed())));
  public static readonly DependencyProperty ItemSizeProperty = DependencyProperty.Register(nameof (ItemSize), typeof (Size), typeof (VirtualizingWrapPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) Size.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty StretchItemsProperty = DependencyProperty.Register(nameof (StretchItems), typeof (bool), typeof (VirtualizingWrapPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsArrange));
  protected Size childSize;
  protected int rowCount;
  protected int itemsPerRowCount;

  [Obsolete("Use IsSpacingEnabled")]
  public bool SpacingEnabled
  {
    get => this.IsSpacingEnabled;
    set => this.IsSpacingEnabled = value;
  }

  [Obsolete("Use SpacingMode")]
  public bool IsSpacingEnabled
  {
    get => (bool) this.GetValue(VirtualizingWrapPanel.IsSpacingEnabledProperty);
    set => this.SetValue(VirtualizingWrapPanel.IsSpacingEnabledProperty, (object) value);
  }

  [Obsolete("Use ItemSize")]
  public Size ChildrenSize
  {
    get => this.ItemSize;
    set => this.ItemSize = value;
  }

  public SpacingMode SpacingMode
  {
    get => (SpacingMode) this.GetValue(VirtualizingWrapPanel.SpacingModeProperty);
    set => this.SetValue(VirtualizingWrapPanel.SpacingModeProperty, (object) value);
  }

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(VirtualizingWrapPanel.OrientationProperty);
    set => this.SetValue(VirtualizingWrapPanel.OrientationProperty, (object) value);
  }

  public Size ItemSize
  {
    get => (Size) this.GetValue(VirtualizingWrapPanel.ItemSizeProperty);
    set => this.SetValue(VirtualizingWrapPanel.ItemSizeProperty, (object) value);
  }

  public bool StretchItems
  {
    get => (bool) this.GetValue(VirtualizingWrapPanel.StretchItemsProperty);
    set => this.SetValue(VirtualizingWrapPanel.StretchItemsProperty, (object) value);
  }

  private void Orientation_Changed()
  {
    this.MouseWheelScrollDirection = this.Orientation == Orientation.Vertical ? ScrollDirection.Vertical : ScrollDirection.Horizontal;
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    this.UpdateChildSize(availableSize);
    return base.MeasureOverride(availableSize);
  }

  private void UpdateChildSize(Size availableSize)
  {
    if (this.ItemsOwner is IHierarchicalVirtualizationAndScrollInfo itemsOwner && VirtualizingPanel.GetIsVirtualizingWhenGrouping((DependencyObject) this.ItemsControl))
    {
      if (this.Orientation == Orientation.Vertical)
      {
        availableSize.Width = itemsOwner.Constraints.Viewport.Size.Width;
        availableSize.Width = Math.Max(availableSize.Width - (this.Margin.Left + this.Margin.Right), 0.0);
      }
      else
      {
        availableSize.Height = itemsOwner.Constraints.Viewport.Size.Height;
        availableSize.Height = Math.Max(availableSize.Height - (this.Margin.Top + this.Margin.Bottom), 0.0);
      }
    }
    this.childSize = !(this.ItemSize != Size.Empty) ? (this.InternalChildren.Count == 0 ? this.CalculateChildSize(availableSize) : this.InternalChildren[0].DesiredSize) : this.ItemSize;
    this.itemsPerRowCount = !double.IsInfinity(this.GetWidth(availableSize)) ? Math.Max(1, (int) Math.Floor(this.GetWidth(availableSize) / this.GetWidth(this.childSize))) : this.Items.Count;
    this.rowCount = (int) Math.Ceiling((double) this.Items.Count / (double) this.itemsPerRowCount);
  }

  private Size CalculateChildSize(Size availableSize)
  {
    if (this.Items.Count == 0)
      return new Size(0.0, 0.0);
    using (this.ItemContainerGenerator.StartAt(this.ItemContainerGenerator.GeneratorPositionFromIndex(0), GeneratorDirection.Forward, true))
    {
      UIElement next = (UIElement) this.ItemContainerGenerator.GenerateNext();
      this.AddInternalChild(next);
      this.ItemContainerGenerator.PrepareItemContainer((DependencyObject) next);
      next.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      return next.DesiredSize;
    }
  }

  protected override Size CalculateExtent(Size availableSize)
  {
    double width = !this.IsSpacingEnabled || this.SpacingMode == SpacingMode.None || double.IsInfinity(this.GetWidth(availableSize)) ? this.GetWidth(this.childSize) * (double) this.itemsPerRowCount : this.GetWidth(availableSize);
    if (this.ItemsOwner is IHierarchicalVirtualizationAndScrollInfo)
      width = this.Orientation != Orientation.Vertical ? Math.Max(width - (this.Margin.Top + this.Margin.Bottom), 0.0) : Math.Max(width - (this.Margin.Left + this.Margin.Right), 0.0);
    double height = this.GetHeight(this.childSize) * (double) this.rowCount;
    return this.CreateSize(width, height);
  }

  protected void CalculateSpacing(Size finalSize, out double innerSpacing, out double outerSpacing)
  {
    Size childArrangeSize = this.CalculateChildArrangeSize(finalSize);
    double width = this.GetWidth(finalSize);
    double num1 = Math.Min(this.GetWidth(childArrangeSize) * (double) this.itemsPerRowCount, width);
    double num2 = width - num1;
    switch (this.IsSpacingEnabled ? (int) this.SpacingMode : 0)
    {
      case 1:
        innerSpacing = outerSpacing = num2 / (double) (this.itemsPerRowCount + 1);
        break;
      case 2:
        innerSpacing = num2 / (double) Math.Max(this.itemsPerRowCount - 1, 1);
        outerSpacing = 0.0;
        break;
      case 3:
        innerSpacing = 0.0;
        outerSpacing = num2 / 2.0;
        break;
      default:
        innerSpacing = 0.0;
        outerSpacing = 0.0;
        break;
    }
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    double x = this.GetX(this.Offset);
    double num1 = this.GetY(this.Offset);
    if (this.ItemsOwner is IHierarchicalVirtualizationAndScrollInfo)
      num1 = 0.0;
    Size childArrangeSize = this.CalculateChildArrangeSize(finalSize);
    double innerSpacing;
    double outerSpacing;
    this.CalculateSpacing(finalSize, out innerSpacing, out outerSpacing);
    for (int index = 0; index < this.InternalChildren.Count; ++index)
    {
      UIElement internalChild = this.InternalChildren[index];
      int indexFromChildIndex = this.GetItemIndexFromChildIndex(index);
      int num2 = indexFromChildIndex % this.itemsPerRowCount;
      int num3 = indexFromChildIndex / this.itemsPerRowCount;
      double num4 = outerSpacing + (double) num2 * (this.GetWidth(childArrangeSize) + innerSpacing);
      double num5 = (double) num3 * this.GetHeight(childArrangeSize);
      if (this.GetHeight(finalSize) == 0.0)
        internalChild.Arrange(new Rect(0.0, 0.0, 0.0, 0.0));
      else
        internalChild.Arrange(this.CreateRect(num4 - x, num5 - num1, childArrangeSize.Width, childArrangeSize.Height));
    }
    return finalSize;
  }

  protected Size CalculateChildArrangeSize(Size finalSize)
  {
    if (!this.StretchItems)
      return this.childSize;
    if (this.Orientation == Orientation.Vertical)
    {
      double val2 = this.ReadItemContainerStyle<double>(FrameworkElement.MaxWidthProperty, double.PositiveInfinity);
      return new Size(Math.Min(finalSize.Width / (double) this.itemsPerRowCount, val2), this.childSize.Height);
    }
    double val2_1 = this.ReadItemContainerStyle<double>(FrameworkElement.MaxHeightProperty, double.PositiveInfinity);
    return new Size(this.childSize.Width, Math.Min(finalSize.Height / (double) this.itemsPerRowCount, val2_1));
  }

  private T ReadItemContainerStyle<T>(DependencyProperty property, T fallbackValue) where T : notnull
  {
    Style itemContainerStyle = this.ItemsControl.ItemContainerStyle;
    return (T) ((itemContainerStyle != null ? itemContainerStyle.Setters.OfType<Setter>().FirstOrDefault<Setter>((Func<Setter, bool>) (setter => setter.Property == property))?.Value : (object) null) ?? (object) fallbackValue);
  }

  protected override ItemRange UpdateItemRange()
  {
    if (!this.IsVirtualizing)
      return new ItemRange(0, this.Items.Count - 1);
    int startIndex;
    int endIndex;
    if (this.ItemsOwner is IHierarchicalVirtualizationAndScrollInfo itemsOwner)
    {
      if (!VirtualizingPanel.GetIsVirtualizingWhenGrouping((DependencyObject) this.ItemsControl))
        return new ItemRange(0, this.Items.Count - 1);
      Point point = new Point(this.Offset.X, itemsOwner.Constraints.Viewport.Location.Y);
      int num1;
      double num2;
      if (this.ScrollUnit == ScrollUnit.Item)
      {
        num1 = this.GetY(point) >= 1.0 ? (int) this.GetY(point) - 1 : 0;
        num2 = (double) num1 * this.GetHeight(this.childSize);
      }
      else
      {
        num2 = Math.Min(Math.Max(this.GetY(point) - this.GetHeight(itemsOwner.HeaderDesiredSizes.PixelSize), 0.0), this.GetHeight(this.Extent));
        num1 = this.GetRowIndex(num2);
      }
      double num3 = Math.Min(this.GetHeight(this.Viewport), Math.Max(this.GetHeight(this.Extent) - num2, 0.0));
      int num4 = (int) Math.Ceiling((num2 + num3) / this.GetHeight(this.childSize)) - (int) Math.Floor(num2 / this.GetHeight(this.childSize));
      startIndex = num1 * this.itemsPerRowCount;
      endIndex = Math.Min((num1 + num4) * this.itemsPerRowCount - 1, this.Items.Count - 1);
      if (this.CacheLengthUnit == VirtualizationCacheLengthUnit.Pixel)
      {
        VirtualizationCacheLength cacheLength = this.CacheLength;
        double num5 = Math.Min(cacheLength.CacheBeforeViewport, num2);
        cacheLength = this.CacheLength;
        double num6 = Math.Min(cacheLength.CacheAfterViewport, this.GetHeight(this.Extent) - num3 - num2);
        double height = this.GetHeight(this.childSize);
        int num7 = (int) (num5 / height);
        int num8 = (int) Math.Ceiling((num2 + num3 + num6) / this.GetHeight(this.childSize)) - (int) Math.Ceiling((num2 + num3) / this.GetHeight(this.childSize));
        startIndex = Math.Max(startIndex - num7 * this.itemsPerRowCount, 0);
        endIndex = Math.Min(endIndex + num8 * this.itemsPerRowCount, this.Items.Count - 1);
      }
      else if (this.CacheLengthUnit == VirtualizationCacheLengthUnit.Item)
      {
        startIndex = Math.Max(startIndex - (int) this.CacheLength.CacheBeforeViewport, 0);
        endIndex = Math.Min(endIndex + (int) this.CacheLength.CacheAfterViewport, this.Items.Count - 1);
      }
    }
    else
    {
      double location1 = this.GetY(this.Offset);
      double location2 = this.GetY(this.Offset) + this.GetHeight(this.Viewport);
      VirtualizationCacheLength cacheLength;
      if (this.CacheLengthUnit == VirtualizationCacheLengthUnit.Pixel)
      {
        location1 = Math.Max(location1 - this.CacheLength.CacheBeforeViewport, 0.0);
        double num = location2;
        cacheLength = this.CacheLength;
        double cacheAfterViewport = cacheLength.CacheAfterViewport;
        location2 = Math.Min(num + cacheAfterViewport, this.GetHeight(this.Extent));
      }
      startIndex = this.GetRowIndex(location1) * this.itemsPerRowCount;
      endIndex = Math.Min(this.GetRowIndex(location2) * this.itemsPerRowCount + (this.itemsPerRowCount - 1), this.Items.Count - 1);
      if (this.CacheLengthUnit == VirtualizationCacheLengthUnit.Page)
      {
        int num9 = endIndex - startIndex + 1;
        int num10 = startIndex;
        cacheLength = this.CacheLength;
        int num11 = (int) cacheLength.CacheBeforeViewport * num9;
        startIndex = Math.Max(num10 - num11, 0);
        int num12 = endIndex;
        cacheLength = this.CacheLength;
        int num13 = (int) cacheLength.CacheAfterViewport * num9;
        endIndex = Math.Min(num12 + num13, this.Items.Count - 1);
      }
      else if (this.CacheLengthUnit == VirtualizationCacheLengthUnit.Item)
      {
        int num14 = startIndex;
        cacheLength = this.CacheLength;
        int cacheBeforeViewport = (int) cacheLength.CacheBeforeViewport;
        startIndex = Math.Max(num14 - cacheBeforeViewport, 0);
        int num15 = endIndex;
        cacheLength = this.CacheLength;
        int cacheAfterViewport = (int) cacheLength.CacheAfterViewport;
        endIndex = Math.Min(num15 + cacheAfterViewport, this.Items.Count - 1);
      }
    }
    return new ItemRange(startIndex, endIndex);
  }

  private int GetRowIndex(double location)
  {
    return Math.Max(Math.Min((int) Math.Floor(location / this.GetHeight(this.childSize)), (int) Math.Ceiling((double) this.Items.Count / (double) this.itemsPerRowCount)), 0);
  }

  protected override void BringIndexIntoView(int index)
  {
    double offset = (double) (index / this.itemsPerRowCount) * this.GetHeight(this.childSize);
    if (this.Orientation == Orientation.Horizontal)
      this.SetHorizontalOffset(offset);
    else
      this.SetVerticalOffset(offset);
  }

  protected override double GetLineUpScrollAmount()
  {
    return -Math.Min(this.childSize.Height * this.ScrollLineDeltaItem, this.Viewport.Height);
  }

  protected override double GetLineDownScrollAmount()
  {
    return Math.Min(this.childSize.Height * this.ScrollLineDeltaItem, this.Viewport.Height);
  }

  protected override double GetLineLeftScrollAmount()
  {
    return -Math.Min(this.childSize.Width * this.ScrollLineDeltaItem, this.Viewport.Width);
  }

  protected override double GetLineRightScrollAmount()
  {
    return Math.Min(this.childSize.Width * this.ScrollLineDeltaItem, this.Viewport.Width);
  }

  protected override double GetMouseWheelUpScrollAmount()
  {
    return -Math.Min(this.childSize.Height * (double) this.MouseWheelDeltaItem, this.Viewport.Height);
  }

  protected override double GetMouseWheelDownScrollAmount()
  {
    return Math.Min(this.childSize.Height * (double) this.MouseWheelDeltaItem, this.Viewport.Height);
  }

  protected override double GetMouseWheelLeftScrollAmount()
  {
    return -Math.Min(this.childSize.Width * (double) this.MouseWheelDeltaItem, this.Viewport.Width);
  }

  protected override double GetMouseWheelRightScrollAmount()
  {
    return Math.Min(this.childSize.Width * (double) this.MouseWheelDeltaItem, this.Viewport.Width);
  }

  protected override double GetPageUpScrollAmount() => -this.Viewport.Height;

  protected override double GetPageDownScrollAmount() => this.Viewport.Height;

  protected override double GetPageLeftScrollAmount() => -this.Viewport.Width;

  protected override double GetPageRightScrollAmount() => this.Viewport.Width;

  protected double GetX(Point point)
  {
    return this.Orientation != Orientation.Vertical ? point.Y : point.X;
  }

  protected double GetY(Point point)
  {
    return this.Orientation != Orientation.Vertical ? point.X : point.Y;
  }

  protected double GetWidth(Size size)
  {
    return this.Orientation != Orientation.Vertical ? size.Height : size.Width;
  }

  protected double GetHeight(Size size)
  {
    return this.Orientation != Orientation.Vertical ? size.Width : size.Height;
  }

  protected Size CreateSize(double width, double height)
  {
    return this.Orientation != Orientation.Vertical ? new Size(height, width) : new Size(width, height);
  }

  protected Rect CreateRect(double x, double y, double width, double height)
  {
    return this.Orientation != Orientation.Vertical ? new Rect(y, x, width, height) : new Rect(x, y, width, height);
  }
}
