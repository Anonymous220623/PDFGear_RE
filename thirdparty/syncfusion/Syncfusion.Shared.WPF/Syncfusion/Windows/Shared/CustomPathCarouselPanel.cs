// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CustomPathCarouselPanel
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class CustomPathCarouselPanel : VirtualizingPanel
{
  private Path _Path;
  private bool isRefreshing;
  private PathFractionRangeHandler m_PathFractionRangeHandler = new PathFractionRangeHandler();
  private SortedDictionary<double, UIElement> sorteditems = new SortedDictionary<double, UIElement>();
  private VirtualizingPanelHandler CurrentVirtualizingPanelHandler;
  private VirtualizingPanelHandler OldVirtualizingPanelHandler;
  private CarouselPanelHelper CarouselPanelHelper;
  internal Carousel Owner;
  internal CarouselItem removingItem;
  internal bool isRender = true;
  internal Size finalSize;
  internal Path DrawingPath;
  internal CarouselPathHelper carouselPathHelper;
  internal PathFractionCollection internalOpacityFractions;
  internal PathFractionCollection internalScaleFractions;
  internal PathFractionCollection internalSkewAngleXFractions;
  internal PathFractionCollection internalSkewAngleYFractions;
  internal int MaxPanelOffset;
  internal int ViewPanelOffset;
  internal int PanelOffset;
  internal VirtualizingPanelHandler NewVirtualizingPanelHandler;
  internal VirtualizingPanelHandler tempVirtualizingPanelHandler;
  public static readonly DependencyProperty ItemsPerPageProperty = DependencyProperty.Register(nameof (ItemsPerPage), typeof (int), typeof (CustomPathCarouselPanel), new PropertyMetadata((object) -1, (PropertyChangedCallback) ((s, a) => ((CustomPathCarouselPanel) s).OnItemsPerPageChanged(s))));
  public static readonly DependencyProperty TopItemPositionProperty = DependencyProperty.Register(nameof (TopItemPosition), typeof (double), typeof (CustomPathCarouselPanel), new PropertyMetadata((object) 0.5, new PropertyChangedCallback(CustomPathCarouselPanel.OnTopItemPositionChanged), new CoerceValueCallback(CustomPathCarouselPanel.CoerceTopItemPosition)));
  public static readonly DependencyProperty OpacityEnabledProperty = DependencyProperty.Register(nameof (OpacityEnabled), typeof (bool), typeof (CustomPathCarouselPanel), new PropertyMetadata((object) true));
  public static readonly DependencyProperty OpacityFractionsProperty = DependencyProperty.Register(nameof (OpacityFractions), typeof (PathFractionCollection), typeof (CustomPathCarouselPanel), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ScalingEnabledProperty = DependencyProperty.Register(nameof (ScalingEnabled), typeof (bool), typeof (CustomPathCarouselPanel), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ScaleFractionsProperty = DependencyProperty.Register(nameof (ScaleFractions), typeof (PathFractionCollection), typeof (CustomPathCarouselPanel), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty SkewAngleXEnabledProperty = DependencyProperty.Register(nameof (SkewAngleXEnabled), typeof (bool), typeof (CustomPathCarouselPanel), new PropertyMetadata((object) false));
  public static readonly DependencyProperty SkewAngleXFractionsProperty = DependencyProperty.Register(nameof (SkewAngleXFractions), typeof (PathFractionCollection), typeof (CustomPathCarouselPanel), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty SkewAngleYEnabledProperty = DependencyProperty.Register(nameof (SkewAngleYEnabled), typeof (bool), typeof (CustomPathCarouselPanel), new PropertyMetadata((object) false));
  public static readonly DependencyProperty SkewAngleYFractionsProperty = DependencyProperty.Register(nameof (SkewAngleYFractions), typeof (PathFractionCollection), typeof (CustomPathCarouselPanel), new PropertyMetadata((PropertyChangedCallback) null));
  private static readonly DependencyProperty ItemPathFractionManagerProperty;
  private static readonly DependencyProperty PathFractionProperty = DependencyProperty.RegisterAttached("PathFraction", typeof (double), typeof (CustomPathCarouselPanel), new PropertyMetadata((object) -1.0));

  static CustomPathCarouselPanel()
  {
    CustomPathCarouselPanel.ItemPathFractionManagerProperty = DependencyProperty.RegisterAttached("ItemMovementAnimationDataFraction", typeof (PathFractionManager), typeof (CustomPathCarouselPanel));
  }

  public CustomPathCarouselPanel()
  {
    this.CarouselPanelHelper = new CarouselPanelHelper(this);
    this.AddHandler(UIElement.MouseDownEvent, (Delegate) new RoutedEventHandler(this.SelectedItemChanged));
    this.Loaded += new RoutedEventHandler(this.CarouselPanel_Loaded);
  }

  public Path Path
  {
    get => this._Path;
    set => this._Path = value;
  }

  public int ItemsPerPage
  {
    get => (int) this.GetValue(CustomPathCarouselPanel.ItemsPerPageProperty);
    set => this.SetValue(CustomPathCarouselPanel.ItemsPerPageProperty, (object) value);
  }

  public double TopItemPosition
  {
    get => (double) this.GetValue(CustomPathCarouselPanel.TopItemPositionProperty);
    set => this.SetValue(CustomPathCarouselPanel.TopItemPositionProperty, (object) value);
  }

  public bool OpacityEnabled
  {
    get => (bool) this.GetValue(CustomPathCarouselPanel.OpacityEnabledProperty);
    set => this.SetValue(CustomPathCarouselPanel.OpacityEnabledProperty, (object) value);
  }

  public PathFractionCollection OpacityFractions
  {
    get => (PathFractionCollection) this.GetValue(CustomPathCarouselPanel.OpacityFractionsProperty);
    set => this.SetValue(CustomPathCarouselPanel.OpacityFractionsProperty, (object) value);
  }

  public bool ScalingEnabled
  {
    get => (bool) this.GetValue(CustomPathCarouselPanel.ScalingEnabledProperty);
    set => this.SetValue(CustomPathCarouselPanel.ScalingEnabledProperty, (object) value);
  }

  public PathFractionCollection ScaleFractions
  {
    get => (PathFractionCollection) this.GetValue(CustomPathCarouselPanel.ScaleFractionsProperty);
    set => this.SetValue(CustomPathCarouselPanel.ScaleFractionsProperty, (object) value);
  }

  public bool SkewAngleXEnabled
  {
    get => (bool) this.GetValue(CustomPathCarouselPanel.SkewAngleXEnabledProperty);
    set => this.SetValue(CustomPathCarouselPanel.SkewAngleXEnabledProperty, (object) value);
  }

  public PathFractionCollection SkewAngleXFractions
  {
    get
    {
      return (PathFractionCollection) this.GetValue(CustomPathCarouselPanel.SkewAngleXFractionsProperty);
    }
    set => this.SetValue(CustomPathCarouselPanel.SkewAngleXFractionsProperty, (object) value);
  }

  public bool SkewAngleYEnabled
  {
    get => (bool) this.GetValue(CustomPathCarouselPanel.SkewAngleYEnabledProperty);
    set => this.SetValue(CustomPathCarouselPanel.SkewAngleYEnabledProperty, (object) value);
  }

  public PathFractionCollection SkewAngleYFractions
  {
    get
    {
      return (PathFractionCollection) this.GetValue(CustomPathCarouselPanel.SkewAngleYFractionsProperty);
    }
    set => this.SetValue(CustomPathCarouselPanel.SkewAngleYFractionsProperty, (object) value);
  }

  internal PathFractionCollection InternalOpacityFractions
  {
    get
    {
      this.internalOpacityFractions = this.OpacityFractions == null || this.OpacityFractions.Count <= 0 ? CarouselPanelHelperMethods.GetDefaultFractionsCollection(double.IsNaN(this.Owner.OpacityFraction) ? 0.5 : this.Owner.OpacityFraction) : this.OpacityFractions;
      return this.internalOpacityFractions;
    }
  }

  internal PathFractionCollection InternalScaleFractions
  {
    get
    {
      this.internalScaleFractions = this.ScaleFractions == null || this.ScaleFractions.Count <= 0 ? CarouselPanelHelperMethods.GetDefaultFractionsCollection(double.IsNaN(this.Owner.ScaleFraction) ? 0.3 : this.Owner.ScaleFraction) : this.ScaleFractions;
      return this.internalScaleFractions;
    }
  }

  internal PathFractionCollection InternalSkewAngleXFractions
  {
    get
    {
      this.internalSkewAngleXFractions = this.SkewAngleXFractions == null || this.SkewAngleXFractions.Count <= 0 ? CarouselPanelHelperMethods.GetDefaultFractionsCollection(this.Owner.SkewAngleXFraction) : this.SkewAngleXFractions;
      return this.internalSkewAngleXFractions;
    }
  }

  internal PathFractionCollection InternalSkewAngleYFractions
  {
    get
    {
      this.internalSkewAngleYFractions = this.SkewAngleYFractions == null || this.SkewAngleYFractions.Count <= 0 ? CarouselPanelHelperMethods.GetDefaultFractionsCollection(this.Owner.SkewAngleYFraction) : this.SkewAngleYFractions;
      return this.internalSkewAngleYFractions;
    }
  }

  private bool ShouldLoadItems => true;

  protected override Size MeasureOverride(Size availableSize)
  {
    Size availableSize1 = new Size(0.0, 0.0);
    if (this.carouselPathHelper == null)
      return availableSize1;
    if (double.IsInfinity(availableSize.Width) && !double.IsInfinity(availableSize.Height))
    {
      availableSize1.Width = this.carouselPathHelper.Geometry.Bounds.Right - this.carouselPathHelper.Geometry.Bounds.Left;
      availableSize1.Height = availableSize.Height;
      this.MeasurePanel(availableSize1);
      return availableSize1;
    }
    if (!double.IsInfinity(availableSize.Width) && double.IsInfinity(availableSize.Height))
    {
      availableSize1.Width = availableSize.Width;
      availableSize1.Height = this.carouselPathHelper.Geometry.Bounds.Bottom - this.carouselPathHelper.Geometry.Bounds.Top;
      this.MeasurePanel(availableSize1);
      return availableSize1;
    }
    if (double.IsInfinity(availableSize.Width) && double.IsInfinity(availableSize.Height))
    {
      availableSize1.Width = this.carouselPathHelper.Geometry.Bounds.Right - this.carouselPathHelper.Geometry.Bounds.Left;
      availableSize1.Height = this.carouselPathHelper.Geometry.Bounds.Bottom - this.carouselPathHelper.Geometry.Bounds.Top;
      this.MeasurePanel(availableSize1);
      return availableSize1;
    }
    this.MeasurePanel(availableSize);
    return availableSize;
  }

  protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
  {
    return (UIElementCollection) new ObservableUIElementCollection((UIElement) this, logicalParent);
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    double childHeightStartPoint = 0.0;
    if (this.IsItemsHost)
    {
      foreach (UIElement internalChild in this.InternalChildren)
      {
        childHeightStartPoint = CustomPathCarouselPanel.ArrangeVisibleChild(childHeightStartPoint, internalChild);
        this.RecalculatePosition(internalChild);
      }
    }
    else
    {
      foreach (UIElement child in this.Children)
      {
        Point point;
        this.carouselPathHelper.Geometry.GetPointAtFractionLength((double) child.GetValue(CustomPathCarouselPanel.PathFractionProperty), out point, out Point _);
        TranslateTransform translateTransform = new TranslateTransform(point.X - child.DesiredSize.Width / 2.0, point.Y - child.DesiredSize.Height / 2.0);
        TransformGroup transformGroup = new TransformGroup();
        transformGroup.Children.Add((Transform) translateTransform);
        transformGroup.Freeze();
        child.RenderTransform = (Transform) transformGroup;
        child.Arrange(new Rect(0.0, 0.0, child.DesiredSize.Width, child.DesiredSize.Height));
      }
    }
    return finalSize;
  }

  protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
  {
    base.OnItemsChanged(sender, args);
    int pathDisplacement = 1;
    if (this.Owner != null && args.Action != NotifyCollectionChangedAction.Reset)
    {
      if (args.Action == NotifyCollectionChangedAction.Remove)
      {
        int selectedIndex = this.Owner.SelectedIndex;
        if (selectedIndex < 0)
          this.Owner.SelectedItem = (object) null;
        else if (!this.Owner.Items.Contains(this.Owner.SelectedItem))
        {
          if (this.Owner.Items.Count != 0)
          {
            for (int index = selectedIndex; index >= 0; --index)
            {
              if (index < this.Owner.Items.Count)
              {
                if (selectedIndex > index)
                  pathDisplacement = -1;
                this.Owner.SelectedItem = this.Owner.Items[index];
                break;
              }
            }
          }
          else
            this.Owner.SelectedItem = (object) null;
        }
      }
      this.Refresh(true, pathDisplacement);
    }
    else
    {
      if (this.Owner == null || args.Action != NotifyCollectionChangedAction.Reset)
        return;
      this.Owner.SelectedItem = (object) null;
    }
  }

  protected override void OnRender(DrawingContext dc) => base.OnRender(dc);

  private static double ArrangeVisibleChild(double childHeightStartPoint, UIElement child)
  {
    child.Arrange(new Rect(new Point(0.0, 0.0), child.DesiredSize));
    childHeightStartPoint += child.DesiredSize.Height;
    return childHeightStartPoint;
  }

  private static void OnTopItemPositionChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((CustomPathCarouselPanel) obj)?.Invalidate(false);
  }

  private static object CoerceTopItemPosition(DependencyObject obj, object value)
  {
    double num = (double) value;
    if (num > 1.0 || num < 0.0)
      num = 0.5;
    return (object) num;
  }

  private static double GetCurrentEffectValue(
    UIElement item,
    PathFractionCollection effectCollection)
  {
    double pathFraction = CustomPathCarouselPanel.GetPathFraction(item);
    FractionValue LeftNearestStopPint = (FractionValue) null;
    FractionValue RightNearestStopPint = (FractionValue) null;
    effectCollection.FindNearestPoints(pathFraction, out LeftNearestStopPint, out RightNearestStopPint);
    if (LeftNearestStopPint == null)
      return RightNearestStopPint.Value;
    return RightNearestStopPint == null ? LeftNearestStopPint.Value : CustomPathCarouselPanel.CalculateChange(pathFraction, LeftNearestStopPint, RightNearestStopPint);
  }

  internal static double CalculateChange(
    double currentPathFraction,
    FractionValue stop1,
    FractionValue stop2)
  {
    FractionValue fractionValue1;
    FractionValue fractionValue2;
    if (stop1.Value > stop2.Value)
    {
      fractionValue1 = stop1;
      fractionValue2 = stop2;
    }
    else
    {
      fractionValue1 = stop2;
      fractionValue2 = stop1;
    }
    double num1 = fractionValue1.Value - fractionValue2.Value;
    double num2 = (currentPathFraction - fractionValue2.Fraction) * num1 / (fractionValue1.Fraction - fractionValue2.Fraction);
    return fractionValue2.Value + num2;
  }

  private static int GetOffsetFromCurrentArrangement(VisibleItemsHandler arrangement)
  {
    int currentArrangement = 0;
    if (arrangement != null && arrangement.GetUsedPositions() > 0)
      currentArrangement = arrangement.GetLargestItemIndex() + arrangement.GetFreePositionsLeft() + 1;
    return currentArrangement;
  }

  internal static PathFractionManager GetPathFractionManager(UIElement element)
  {
    return (PathFractionManager) element.GetValue(CustomPathCarouselPanel.ItemPathFractionManagerProperty);
  }

  internal static void SetPathFractionManager(UIElement element, PathFractionManager value)
  {
    element.SetValue(CustomPathCarouselPanel.ItemPathFractionManagerProperty, (object) value);
  }

  internal static void SetPathFraction(UIElement element, double value)
  {
    element.SetValue(CustomPathCarouselPanel.PathFractionProperty, (object) value);
  }

  internal static double GetPathFraction(UIElement element)
  {
    return element == null ? -1.0 : (double) element.GetValue(CustomPathCarouselPanel.PathFractionProperty);
  }

  internal static int GetItemCountLater(PathFractionRangeHandler range, int itemCount)
  {
    return range.LastVisibleItemIndex >= itemCount ? 0 : itemCount - range.LastVisibleItemIndex - 1;
  }

  internal static int GetItemCountBefore(PathFractionRangeHandler range)
  {
    return range.FirstVisibleItemIndex < 0 ? 0 : range.FirstVisibleItemIndex;
  }

  public void RenderAgain()
  {
    for (int index = 0; index < this.InternalChildren.Count; ++index)
      this.InternalChildren[index].SetValue(CustomPathCarouselPanel.PathFractionProperty, (object) this.carouselPathHelper.PathFractions[index].PathFraction);
  }

  public void MoveBy(int displacement)
  {
    if (displacement == 0)
      return;
    this.MoveItemInternallyBy(displacement);
    this.UpdatePanelOffset(displacement);
  }

  public void BringItemIntoView(UIElement item, bool isItemSelected)
  {
    if (!this.Children.Contains(item))
      return;
    int offsetFromTopElement = this.GetMovementOffsetFromTopElement(item);
    int num = isItemSelected ? 1 : 0;
    this.MoveItemInternallyBy(offsetFromTopElement);
  }

  internal void MeasurePanel(Size availableSize)
  {
    this.finalSize = availableSize;
    if (this.Owner.SelectedIndex < 0)
      this.Owner.SelectedIndex = 0;
    this.SetPaths();
    this.carouselPathHelper.UpdateCustomPath(availableSize, this.Owner.Padding, this.InternalChildren.Count > 0 ? new Size(this.InternalChildren[0].DesiredSize.Width, this.InternalChildren[0].DesiredSize.Height) : new Size(0.0, 0.0));
    this.CleanUpItems();
    this.InitializeItemMovement();
    this.UpdateVisibleItems();
    foreach (UIElement internalChild in this.InternalChildren)
      internalChild.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    this.SetMaximumandViewPanelOffset();
    this.Start_ItemMovement();
  }

  internal void RecalculatePosition(UIElement child)
  {
    if (this.CurrentVirtualizingPanelHandler == null)
    {
      MatrixTransform transform = VirtualizingPanelHandler.RecalculateItemPosition(child, this.carouselPathHelper);
      this.ApplyOffsetTransform(child, transform);
    }
    this.UpdateVisualization();
  }

  internal VisibleItemsHandler GetCurrentItemPathArrangement()
  {
    if (this.carouselPathHelper == null)
      return (VisibleItemsHandler) null;
    VisibleItemsHandler itemPathArrangement = new VisibleItemsHandler(this.carouselPathHelper.GetVisiblePathFractionCount());
    if (itemPathArrangement.Count > 0)
    {
      foreach (VisiblePanelItem visiblePanelItem in this.m_PathFractionRangeHandler)
      {
        double pathFraction = CustomPathCarouselPanel.GetPathFraction(visiblePanelItem.Child);
        if (CarouselPathHelper.IsVisible(pathFraction))
        {
          int pathFractionIndex = this.carouselPathHelper.GetPathFractionIndex(pathFraction);
          if (pathFractionIndex != -1)
            itemPathArrangement.SetItemAtPosition(pathFractionIndex - 1, visiblePanelItem);
        }
      }
    }
    return itemPathArrangement;
  }

  internal void SetCustomPathWithItems(int numberOfItems)
  {
    if (numberOfItems <= 0 || !this.ShouldLoadItems || this.m_PathFractionRangeHandler.HasVisibleItems)
      return;
    int displacement = Math.Min(this.ItemsPerPage, numberOfItems);
    int itemCountlater = CarouselPanelHelperMethods.GetItemCountlater(this.m_PathFractionRangeHandler, this.CarouselPanelHelper.ItemsCount);
    int itemCountBefore = CarouselPanelHelperMethods.GetItemCountBefore(this.m_PathFractionRangeHandler);
    if (itemCountlater >= displacement)
    {
      this.MoveBy(displacement);
    }
    else
    {
      if (itemCountBefore < displacement)
        return;
      this.MoveBy(-displacement);
    }
  }

  internal UIElement FindClosestElementToPathFraction(double point)
  {
    double num1 = double.PositiveInfinity;
    UIElement uiElement = (UIElement) null;
    foreach (UIElement internalChild in this.InternalChildren)
    {
      double num2 = Math.Abs(CustomPathCarouselPanel.GetPathFraction(internalChild) - point);
      if (num2 < num1)
      {
        num1 = num2;
        uiElement = internalChild;
      }
    }
    return uiElement ?? (UIElement) null;
  }

  internal void UpdatePanelOffset(int displacement)
  {
    if (displacement == 0)
      return;
    int min = 1;
    int max = this.MaxPanelOffset - this.ViewPanelOffset - 1;
    this.PanelOffset = CarouselPanelHelperMethods.CoerceRangeValues(this.PanelOffset + displacement, min, max);
  }

  internal void MoveItemInternallyforoutofrange(int displacement)
  {
    if (this.CurrentVirtualizingPanelHandler != null)
    {
      if (!(this.CurrentVirtualizingPanelHandler is VirtualizingPanelItemMoveHandler virtualizingPanelHandler) || !virtualizingPanelHandler.IsOpposite(displacement))
        return;
      virtualizingPanelHandler.Reverse();
    }
    else
    {
      this.FinishItemMovements();
      if (displacement == 0 || this.NewVirtualizingPanelHandler != null || !this.IsInitialized)
        return;
      this.NewVirtualizingPanelHandler = (VirtualizingPanelHandler) new VirtualizingPanelItemMoveHandler(displacement, this.CarouselPanelHelper);
      this.Invalidate(false);
    }
  }

  internal void MoveItemInternallyBy(int displacement)
  {
    if (this.CurrentVirtualizingPanelHandler != null)
    {
      if (!(this.CurrentVirtualizingPanelHandler is VirtualizingPanelItemMoveHandler virtualizingPanelHandler) || !virtualizingPanelHandler.IsOpposite(displacement))
        return;
      virtualizingPanelHandler.Reverse();
    }
    else
    {
      this.FinishItemMovements();
      int displacement1 = this.CoerceDisplacement(displacement);
      if (displacement1 == 0 || this.NewVirtualizingPanelHandler != null || !this.IsInitialized)
        return;
      this.NewVirtualizingPanelHandler = (VirtualizingPanelHandler) new VirtualizingPanelItemMoveHandler(displacement1, this.CarouselPanelHelper);
      this.Invalidate(false);
    }
  }

  internal void Invalidate(bool invalidateVisual)
  {
    if (invalidateVisual)
    {
      this.InvalidateVisual();
    }
    else
    {
      this.InvalidateMeasure();
      this.InvalidateArrange();
    }
  }

  internal void Refresh(bool isCollectionModified, int pathDisplacement)
  {
    this.isRefreshing = true;
    this.CarouselPanelHelper = new CarouselPanelHelper(this);
    this.CurrentVirtualizingPanelHandler = (VirtualizingPanelHandler) null;
    this.ItemsPerPage = this.Owner.ItemsPerPage == -1 || this.Owner.Items.Count < this.Owner.ItemsPerPage && this.Owner.Items.Count > 0 ? this.Owner.CoerceItemsPerPageValue(this.Owner.Items.Count) : this.Owner.CoerceItemsPerPageValue(this.Owner.ItemsPerPage);
    int displacement1 = this.CoerceDisplacement(this.ItemsPerPage);
    this.m_PathFractionRangeHandler = new PathFractionRangeHandler();
    this.CheckPanelOffset();
    this.NewVirtualizingPanelHandler = (VirtualizingPanelHandler) null;
    if (!isCollectionModified)
    {
      this.MoveBy(displacement1);
    }
    else
    {
      this.SetPaths();
      int displacement2 = this.Owner.SelectedIndex + this.carouselPathHelper.TopElementPathFractionIndex;
      if (pathDisplacement < 0)
      {
        this.PanelOffset = displacement2 + 1;
        displacement2 = pathDisplacement;
      }
      this.MoveBy(displacement2);
    }
    this.isRefreshing = false;
  }

  internal void FinishItemMovements()
  {
    if (this.CurrentVirtualizingPanelHandler == null)
      return;
    this.UpdateVisualization();
    CompositionTarget.Rendering -= new EventHandler(this.TargetRendering);
    this.CurrentVirtualizingPanelHandler.EndItemMovement();
    this.CurrentVirtualizingPanelHandler.State = ItemMovementState.Finished;
    IList<VisiblePanelItem> endofArrangeOverride = this.CurrentVirtualizingPanelHandler.GetItemsToRemoveEndofArrangeOverride();
    if (endofArrangeOverride != null && endofArrangeOverride.Count > 0)
    {
      foreach (VisiblePanelItem visiblePanelItem in (IEnumerable<VisiblePanelItem>) endofArrangeOverride)
        CustomPathCarouselPanel.SetPathFraction(visiblePanelItem.Child, -1.0);
    }
    this.m_PathFractionRangeHandler.ScheduleClean(endofArrangeOverride);
    this.CurrentVirtualizingPanelHandler = (VirtualizingPanelHandler) null;
    this.CheckPanelOffset();
    this.Invalidate(false);
  }

  protected void OnItemsPerPageChanged(DependencyObject obj)
  {
    if (!(obj is CustomPathCarouselPanel pathCarouselPanel) || pathCarouselPanel.isRefreshing)
      return;
    pathCarouselPanel.SetPaths();
    if (this.Owner != null)
    {
      if (this.ItemsPerPage == -1 || this.Owner.Items.Count < this.Owner.ItemsPerPage && this.Owner.Items.Count > 0)
        this.ItemsPerPage = this.Owner.CoerceItemsPerPageValue(this.Owner.Items.Count);
      if (this.Owner.SelectedItem != null && pathCarouselPanel.NewVirtualizingPanelHandler == null && this.IsInitialized && this.Owner.SelectedItem is CarouselItem)
        pathCarouselPanel.NewVirtualizingPanelHandler = (VirtualizingPanelHandler) new VirtualizingPanelItemMoveHandler(this.CoerceDisplacement(this.GetMovementOffsetFromTopElement((UIElement) (this.Owner.SelectedItem as CarouselItem))), pathCarouselPanel.CarouselPanelHelper);
    }
    if (this.Owner != null && this.Owner.IsLoaded && pathCarouselPanel.IsLoaded)
    {
      pathCarouselPanel.SetCustomPathWithItems(pathCarouselPanel.ItemsPerPage);
      if (this.ItemsPerPage >= 0)
        this.Refresh(true, 1);
    }
    pathCarouselPanel.Invalidate(false);
  }

  private void CarouselPanel_Loaded(object sender, RoutedEventArgs e)
  {
    this.SetCustomPathWithItems(this.ItemsPerPage);
  }

  private void SetPaths()
  {
    if (this.Path == null)
    {
      this.SetDrawingPathFromPath(CarouselPanelHelperMethods.GetPath());
    }
    else
    {
      this.Path.Stretch = Stretch.Uniform;
      this.SetDrawingPathFromPath(this.Path);
    }
  }

  private void SetDrawingPathFromPath(Path drawingPath)
  {
    this.DrawingPath = drawingPath;
    this.SetFractionPathDrawing(drawingPath);
  }

  private void SetFractionPathDrawing(Path drawingPath)
  {
    if (drawingPath == null)
      return;
    this.carouselPathHelper = new CarouselPathHelper(drawingPath, this.ItemsPerPage);
    this.carouselPathHelper.SetTopElementPathFraction(new PathFractions(this.TopItemPosition));
  }

  private void DrawPath(DrawingContext dc)
  {
    Path path = this.Path;
    if (dc == null || path == null)
      return;
    dc.DrawGeometry(path.Fill, new Pen()
    {
      Brush = (Brush) new SolidColorBrush(Colors.Red),
      DashCap = path.StrokeDashCap,
      EndLineCap = path.StrokeEndLineCap,
      LineJoin = path.StrokeLineJoin,
      MiterLimit = path.StrokeMiterLimit,
      StartLineCap = path.StrokeStartLineCap,
      Thickness = 2.0
    }, (Geometry) this.carouselPathHelper.Geometry);
  }

  private void ApplyOffsetTransform(UIElement item, MatrixTransform transform)
  {
    Point point = new Point(0.0, 0.0);
    Matrix matrix = transform.Matrix;
    matrix.Translate(point.X, point.Y);
    MatrixTransform matrixTransform = new MatrixTransform(matrix);
    item.RenderTransform = (Transform) matrixTransform;
  }

  private void SelectedItemChanged(object sender, RoutedEventArgs e)
  {
  }

  private void CleanUpItems()
  {
    if (this.IsItemsHost)
      this.CleanGeneratedItems();
    this.m_PathFractionRangeHandler.ClearCleanUp();
  }

  private void RemoveDisconnectedItems()
  {
    for (int index = 0; index < this.InternalChildren.Count; ++index)
    {
      CarouselItem internalChild = this.InternalChildren[index] is CarouselItem ? this.InternalChildren[index] as CarouselItem : (CarouselItem) null;
      if (internalChild != null && internalChild.DataContext != null && internalChild.DataContext.ToString() == "{DisconnectedItem}")
      {
        this.RemoveInternalChildRange(index, 1);
        --index;
      }
    }
  }

  private void CleanGeneratedItems()
  {
    this.RemoveDisconnectedItems();
    UIElementCollection internalChildren = this.InternalChildren;
    IItemContainerGenerator containerGenerator = this.ItemContainerGenerator;
    foreach (VisiblePanelItem visiblePanelItem in this.m_PathFractionRangeHandler.ToCleanUp)
    {
      GeneratorPosition position = containerGenerator.GeneratorPositionFromIndex(visiblePanelItem.Index);
      int index = this.InternalChildren.IndexOf(visiblePanelItem.Child);
      if (position.Index >= 0 && index >= 0)
      {
        containerGenerator.Remove(position, 1);
        this.RemoveInternalChildRange(index, 1);
      }
    }
  }

  private void UpdateVisibleItems()
  {
    if (this.IsItemsHost)
      this.GenerateItems();
    else
      this.UpdateChildPairs();
  }

  private void UpdateChildPairs()
  {
    if (this.IsItemsHost)
      return;
    foreach (VisiblePanelItem visiblePanelItem in this.m_PathFractionRangeHandler)
      visiblePanelItem.Child = this.InternalChildren[visiblePanelItem.Index];
  }

  private void GenerateItems()
  {
    if (!this.IsItemsHost)
      return;
    this.GenerateChildrenWithItemContainerGenerator();
  }

  private void GenerateChildrenWithItemContainerGenerator()
  {
    if (this.m_PathFractionRangeHandler.HasVisibleItems)
    {
      UIElementCollection internalChildren = this.InternalChildren;
      IItemContainerGenerator containerGenerator = this.ItemContainerGenerator;
      foreach (VisiblePanelItem visiblePanelItem in this.m_PathFractionRangeHandler)
      {
        GeneratorPosition position = containerGenerator.GeneratorPositionFromIndex(visiblePanelItem.Index);
        using (containerGenerator.StartAt(position, GeneratorDirection.Forward, true))
        {
          bool isNewlyRealized;
          UIElement next = containerGenerator.GenerateNext(out isNewlyRealized) as UIElement;
          if (!isNewlyRealized)
          {
            if (visiblePanelItem.Child != null)
              continue;
          }
          visiblePanelItem.Child = next;
          try
          {
            this.InsertInternalChild(this.InternalChildren.Count, next);
            containerGenerator.PrepareItemContainer((DependencyObject) next);
          }
          catch (Exception ex)
          {
          }
        }
      }
    }
    if (this.InternalChildren.Count <= this.m_PathFractionRangeHandler.Count || !(this.NewVirtualizingPanelHandler is VirtualizingPanelItemMoveHandler))
      return;
    VirtualizingPanelItemMoveHandler virtualizingPanelHandler = this.NewVirtualizingPanelHandler as VirtualizingPanelItemMoveHandler;
    this.CleanUpItems(virtualizingPanelHandler.Collection.FirstVisibleIndex, virtualizingPanelHandler.Collection.LastVisibleIndex);
  }

  private void CleanUpItems(int firstVisibleItemIndex, int lastVisibleItemIndex)
  {
    UIElementCollection internalChildren = this.InternalChildren;
    IItemContainerGenerator containerGenerator = this.ItemContainerGenerator;
    for (int index = internalChildren.Count - 1; index >= 0; --index)
    {
      GeneratorPosition position = new GeneratorPosition(index, 0);
      int num1 = containerGenerator.IndexFromGeneratorPosition(position);
      if (this.Owner.EnableLooping)
      {
        int num2 = this.ItemsPerPage % 2 == 0 ? this.ItemsPerPage / 2 - 1 : this.ItemsPerPage / 2;
        int num3 = this.ItemsPerPage % 2 == 0 ? num2 + 1 : num2;
        int min1 = this.Owner.SelectedIndex >= 0 ? this.Owner.SelectedIndex - num2 : 0;
        if (min1 >= 0)
        {
          int num4 = this.Owner.SelectedIndex + num3 - (this.Owner.Items.Count - 1);
          if (!CarouselPanelHelperMethods.IsInRange(num1, min1, min1 + this.ItemsPerPage - 1) && !CarouselPanelHelperMethods.IsInRange(num1, 0, num4 - 1))
            containerGenerator.Remove(position, 1);
        }
        else
        {
          int min2 = this.Owner.Items.Count + min1;
          int max = min1 >= 0 ? this.ItemsPerPage - 1 : this.ItemsPerPage + min1 - 1;
          if (!CarouselPanelHelperMethods.IsInRange(num1, 0, max) && !CarouselPanelHelperMethods.IsInRange(num1, min2, this.Owner.Items.Count - 1))
            containerGenerator.Remove(position, 1);
        }
      }
      else if (num1 < firstVisibleItemIndex || num1 > lastVisibleItemIndex)
        containerGenerator.Remove(position, 1);
    }
    this.RemoveDisconnectedItems();
  }

  private void UpdateVisualization()
  {
    if (!this.m_PathFractionRangeHandler.HasVisibleItems)
      return;
    this.UpdateZorder();
    foreach (VisiblePanelItem visiblePanelItem in this.m_PathFractionRangeHandler)
    {
      UIElement child = visiblePanelItem.Child;
      child.RenderTransformOrigin = new Point(0.5, 0.5);
      this.UpdateOpacityFractions(child);
      SkewTransform transform1;
      this.UpdateSkewAngleXFractions(child, out transform1);
      SkewTransform transform2;
      this.UpdateSkewAngleYFractions(child, out transform2);
      ScaleTransform transform3;
      this.UpdateScaleFractions(child, out transform3);
      TranslateTransform translateTransform = new TranslateTransform(child.RenderTransform.Value.OffsetX, child.RenderTransform.Value.OffsetY);
      MatrixTransform matrixTransform = new MatrixTransform(transform3.Value * transform1.Value * transform2.Value * translateTransform.Value);
      child.RenderTransform = (Transform) matrixTransform;
    }
  }

  private void UpdateZorder()
  {
    if (this.FindClosestElementToPathFraction(this.TopItemPosition) == null)
      return;
    int num1 = 0;
    for (int index = 0; index < ((IEnumerable<PathFractions>) this.carouselPathHelper.PathFractions).Count<PathFractions>(); ++index)
    {
      if (this.carouselPathHelper.PathFractions[index].PathFraction == this.carouselPathHelper.topElementPathFraction.PathFraction)
      {
        num1 = index;
        break;
      }
    }
    this.sorteditems.Clear();
    foreach (UIElement child in this.Children)
    {
      if (!this.sorteditems.ContainsKey(CustomPathCarouselPanel.GetPathFraction(child)))
        this.sorteditems.Add(CustomPathCarouselPanel.GetPathFraction(child), child);
    }
    int num2 = this.Children.Count - 1;
    for (int index = num1; index >= 0; --index)
    {
      double pathFraction = this.carouselPathHelper.PathFractions[index].PathFraction;
      if (this.sorteditems.ContainsKey(pathFraction))
      {
        Panel.SetZIndex(this.sorteditems[pathFraction], num2);
        --num2;
      }
    }
    for (int index = num1 + 1; index < ((IEnumerable<PathFractions>) this.carouselPathHelper.PathFractions).Count<PathFractions>(); ++index)
    {
      double pathFraction = this.carouselPathHelper.PathFractions[index].PathFraction;
      if (this.sorteditems.ContainsKey(pathFraction))
      {
        Panel.SetZIndex(this.sorteditems[pathFraction], num2);
        --num2;
      }
    }
  }

  private void UpdateOpacityFractions(UIElement item)
  {
    if (this.OpacityEnabled && this.InternalOpacityFractions != null && this.InternalOpacityFractions.Count > 0)
    {
      double currentEffectValue = CustomPathCarouselPanel.GetCurrentEffectValue(item, this.InternalOpacityFractions);
      item.Opacity = currentEffectValue;
    }
    else
      item.Opacity = 1.0;
    if (CustomPathCarouselPanel.GetPathFraction(item) != 0.0 && CustomPathCarouselPanel.GetPathFraction(item) != 1.0)
      return;
    item.Opacity = 0.0;
  }

  private void UpdateScaleFractions(UIElement item, out ScaleTransform transform)
  {
    transform = new ScaleTransform(1.0, 1.0);
    if (this.ScalingEnabled && this.InternalScaleFractions != null && this.InternalScaleFractions.Count > 0)
    {
      double currentEffectValue = CustomPathCarouselPanel.GetCurrentEffectValue(item, this.InternalScaleFractions);
      transform = new ScaleTransform(currentEffectValue, currentEffectValue);
    }
    else
      transform = new ScaleTransform();
  }

  private void UpdateSkewAngleXFractions(UIElement item, out SkewTransform transform)
  {
    if (this.SkewAngleXEnabled && this.InternalSkewAngleXFractions != null && this.InternalSkewAngleXFractions.Count > 0)
    {
      double currentEffectValue = CustomPathCarouselPanel.GetCurrentEffectValue(item, this.InternalSkewAngleXFractions);
      transform = new SkewTransform(currentEffectValue, 0.0);
    }
    else
      transform = new SkewTransform();
  }

  private void UpdateSkewAngleYFractions(UIElement item, out SkewTransform transform)
  {
    if (this.SkewAngleYEnabled && this.InternalSkewAngleYFractions != null && this.InternalSkewAngleYFractions.Count > 0)
    {
      double currentEffectValue = CustomPathCarouselPanel.GetCurrentEffectValue(item, this.InternalSkewAngleYFractions);
      transform = new SkewTransform(0.0, currentEffectValue);
    }
    else
      transform = new SkewTransform();
  }

  private void CheckPanelOffset()
  {
    this.SetMaximumandViewPanelOffset();
    this.PanelOffset = CustomPathCarouselPanel.GetOffsetFromCurrentArrangement(this.GetCurrentItemPathArrangement());
  }

  private void SetMaximumandViewPanelOffset()
  {
    int maximumOffset = this.CalculateMaximumOffset();
    if (this.MaxPanelOffset != maximumOffset)
      this.MaxPanelOffset = maximumOffset;
    if (this.ItemsPerPage == this.ViewPanelOffset)
      return;
    this.ViewPanelOffset = this.ItemsPerPage;
  }

  private int CalculateMaximumOffset()
  {
    int maximumOffset = 0;
    if (this.CarouselPanelHelper.ItemsCount > 0)
      maximumOffset = this.CarouselPanelHelper.ItemsCount + this.ItemsPerPage + this.ItemsPerPage;
    return maximumOffset;
  }

  private int GetMovementOffsetFromTopElement(UIElement element)
  {
    double pathFraction = CustomPathCarouselPanel.GetPathFraction(element);
    return pathFraction == -1.0 ? 0 : this.carouselPathHelper.TopElementPathFractionIndex - this.carouselPathHelper.GetPathFractionIndex(pathFraction);
  }

  private void Start_ItemMovement()
  {
    if (this.NewVirtualizingPanelHandler == null)
      return;
    this.PrepareItemsToMove(this.NewVirtualizingPanelHandler);
    if (this.CurrentVirtualizingPanelHandler == null)
      CompositionTarget.Rendering += new EventHandler(this.TargetRendering);
    this.OldVirtualizingPanelHandler = this.NewVirtualizingPanelHandler;
    this.NewVirtualizingPanelHandler = (VirtualizingPanelHandler) null;
  }

  private void PrepareItemsToMove(VirtualizingPanelHandler _VirtualizingPanelHandler)
  {
    List<VisiblePanelItem> list = this.m_PathFractionRangeHandler.ToList<VisiblePanelItem>();
    if (_VirtualizingPanelHandler is VirtualizingPanelItemMoveHandler panelItemMoveHandler && panelItemMoveHandler.PathDisplacement < 0)
      list.Reverse();
    _VirtualizingPanelHandler.Duration = new TimeSpan(0, 0, 0, 0, 300);
    foreach (VisiblePanelItem visiblePanelItem in list)
    {
      visiblePanelItem.Child.RenderTransformOrigin = new Point(0.5, 0.5);
      _VirtualizingPanelHandler.AddItemToMove(visiblePanelItem, this.Owner.EnableLooping, this.Owner.SelectedIndex, this.Owner.isNextPage, this.Owner.isPreviousPage);
    }
  }

  private int CoerceDisplacement(int displacement)
  {
    int num = displacement;
    VisibleItemsHandler itemPathArrangement = this.GetCurrentItemPathArrangement();
    int itemCountLater = CustomPathCarouselPanel.GetItemCountLater(this.m_PathFractionRangeHandler, this.CarouselPanelHelper.ItemsCount);
    int itemCountBefore = CustomPathCarouselPanel.GetItemCountBefore(this.m_PathFractionRangeHandler);
    if (itemPathArrangement == null)
      return 0;
    if (displacement < 0)
      return -Math.Min(itemPathArrangement.GetUsedPositions() + itemPathArrangement.GetFreePositionsLeft() - 1 + itemCountBefore, -displacement);
    if (displacement > 0)
      num = Math.Min(itemPathArrangement.GetUsedPositions() + itemPathArrangement.GetFreePositionsRight() - 1 + itemCountLater, displacement);
    return num;
  }

  private void InitializeItemMovement()
  {
    if (this.NewVirtualizingPanelHandler == null)
      return;
    this.tempVirtualizingPanelHandler = this.NewVirtualizingPanelHandler;
    this.NewVirtualizingPanelHandler.Initialize(this.carouselPathHelper, this.GetCurrentItemPathArrangement());
    VisibleRangeAction action;
    LinkedList<VisiblePanelItem> itemsToAdd;
    this.NewVirtualizingPanelHandler.CalculateItemsToAdd(out action, out itemsToAdd, this.Owner.EnableLooping, this.Owner.SelectedIndex);
    this.m_PathFractionRangeHandler.UpdateVisibleRange(action, itemsToAdd);
  }

  private void CheckSelectedItem()
  {
    if (this.Owner == null)
      return;
    if (this.Owner.SelectedItem == null || this.Owner.SelectedItem is CarouselItem)
    {
      UIElement elementToPathFraction = this.FindClosestElementToPathFraction(this.Owner.TopItemPosition);
      if (elementToPathFraction != null && this.Owner.SelectedIndex == -1)
      {
        if (this.Owner.SelectedItem == null && elementToPathFraction is CarouselItem)
        {
          int num = this.Owner.Items.IndexOf((elementToPathFraction as CarouselItem).DataContext);
          if (num == -1)
            num = this.Owner.Items.IndexOf((object) (elementToPathFraction as CarouselItem));
          this.Owner.SelectedIndex = num;
        }
        else
        {
          if (this.Owner.SelectedItem == null || !(elementToPathFraction is CarouselItem) || elementToPathFraction.Equals(this.Owner.SelectedItem) || !(this.Owner.SelectedItem is CarouselItem))
            return;
          this.BringItemIntoView((UIElement) (this.Owner.SelectedItem as CarouselItem), true);
        }
      }
      else
      {
        if (this.Owner.SelectedIndex <= -1 || this.Owner.SelectedIndex > this.Owner.Items.Count - 1)
          return;
        if (this.Owner.SelectedItem == null)
          this.Owner.SelectedItem = this.Owner.Items[this.Owner.SelectedIndex];
        this.BringItemIntoView((UIElement) (this.Owner.ItemContainerGenerator.ContainerFromIndex(this.Owner.SelectedIndex) as CarouselItem), true);
      }
    }
    else
    {
      if (this.Owner.SelectedItem == null)
        return;
      this.BringItemIntoView((UIElement) (this.Owner.ItemContainerGenerator.ContainerFromItem(this.Owner.SelectedItem) as CarouselItem), true);
    }
  }

  private void TargetRendering(object sender, EventArgs e)
  {
    RenderingEventArgs renderingEventArgs = (RenderingEventArgs) e;
    if (this.OldVirtualizingPanelHandler != null)
    {
      this.CurrentVirtualizingPanelHandler = this.OldVirtualizingPanelHandler;
      this.OldVirtualizingPanelHandler = (VirtualizingPanelHandler) null;
      this.CurrentVirtualizingPanelHandler.BeginItemMovement(renderingEventArgs.RenderingTime);
    }
    if (this.CurrentVirtualizingPanelHandler == null)
      return;
    if (this.Owner.EnableRotationAnimation)
      this.CurrentVirtualizingPanelHandler.Update(renderingEventArgs.RenderingTime);
    this.UpdateVisualization();
    if (this.CurrentVirtualizingPanelHandler.State == ItemMovementState.Started)
      this.CheckSelectedItem();
    if (this.CurrentVirtualizingPanelHandler == null || this.CurrentVirtualizingPanelHandler.State != ItemMovementState.Finished)
      return;
    this.FinishItemMovements();
    this.CheckSelectedItem();
    this.Owner.isNextPage = this.Owner.isPreviousPage = false;
  }
}
