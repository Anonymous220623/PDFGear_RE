// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Carousel
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class Carousel : ItemsControl, IDisposable
{
  private static RoutedCommand selectFirstItemCommand = new RoutedCommand();
  private static RoutedCommand selectLastItemCommand = new RoutedCommand();
  private static RoutedCommand selectNextItemCommand = new RoutedCommand();
  private static RoutedCommand selectPreviousItemCommand = new RoutedCommand();
  private static RoutedCommand selectNextPageCommand = new RoutedCommand();
  private static RoutedCommand selectPreviousPageCommand = new RoutedCommand();
  private bool IsVisualChanged;
  private Panel _itemsHost;
  private ScrollBar horizontalScrollBar;
  private ScrollBar verticalScrollBar;
  private TranslateTransform transform = new TranslateTransform();
  internal CarouselItem previousSelected;
  internal bool isNextPage = true;
  internal bool isPreviousPage = true;
  public static readonly DependencyProperty SkewAngleXFractionProperty = DependencyProperty.Register(nameof (SkewAngleXFraction), typeof (double), typeof (Carousel), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(Carousel.OnFractionChanged)));
  public static readonly DependencyProperty SkewAngleYFractionProperty = DependencyProperty.Register(nameof (SkewAngleYFraction), typeof (double), typeof (Carousel), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(Carousel.OnFractionChanged)));
  public static readonly DependencyProperty EnableLoopingProperty = DependencyProperty.Register(nameof (EnableLooping), typeof (bool), typeof (Carousel), new PropertyMetadata((object) false, new PropertyChangedCallback(Carousel.OnEnableLoopingChanged)));
  public static readonly DependencyProperty ItemsPerPageProperty = DependencyProperty.Register(nameof (ItemsPerPage), typeof (int), typeof (Carousel), new PropertyMetadata((object) -1, (PropertyChangedCallback) ((s, a) => ((Carousel) s).OnItemsPerPageChanged(s))));
  public static readonly DependencyProperty ScaleFractionsProperty = DependencyProperty.Register(nameof (ScaleFractions), typeof (PathFractionCollection), typeof (Carousel), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) => ((Carousel) s).OnItemsVisualChanged(s))));
  public static readonly DependencyProperty ScalingEnabledProperty = DependencyProperty.Register(nameof (ScalingEnabled), typeof (bool), typeof (Carousel), new PropertyMetadata((object) true, (PropertyChangedCallback) ((s, a) => ((Carousel) s).OnItemsVisualChanged(s))));
  public static readonly DependencyProperty OpacityEnabledProperty = DependencyProperty.Register(nameof (OpacityEnabled), typeof (bool), typeof (Carousel), new PropertyMetadata((object) true, (PropertyChangedCallback) ((s, a) => ((Carousel) s).OnItemsVisualChanged(s))));
  public static readonly DependencyProperty OpacityFractionsProperty = DependencyProperty.Register(nameof (OpacityFractions), typeof (PathFractionCollection), typeof (Carousel), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) => ((Carousel) s).OnItemsVisualChanged(s))));
  public static readonly DependencyProperty SkewAngleXEnabledProperty = DependencyProperty.Register(nameof (SkewAngleXEnabled), typeof (bool), typeof (Carousel), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) => ((Carousel) s).OnItemsVisualChanged(s))));
  public static readonly DependencyProperty SkewAngleXFractionsProperty = DependencyProperty.Register(nameof (SkewAngleXFractions), typeof (PathFractionCollection), typeof (Carousel), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) => ((Carousel) s).OnItemsVisualChanged(s))));
  public static readonly DependencyProperty SkewAngleYEnabledProperty = DependencyProperty.Register(nameof (SkewAngleYEnabled), typeof (bool), typeof (Carousel), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) => ((Carousel) s).OnItemsVisualChanged(s))));
  public static readonly DependencyProperty SkewAngleYFractionsProperty = DependencyProperty.Register(nameof (SkewAngleYFractions), typeof (PathFractionCollection), typeof (Carousel), (PropertyMetadata) new UIPropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) => ((Carousel) s).OnItemsVisualChanged(s))));
  public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register(nameof (RadiusX), typeof (double), typeof (Carousel), (PropertyMetadata) new UIPropertyMetadata((object) 250.0));
  public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register(nameof (RadiusY), typeof (double), typeof (Carousel), (PropertyMetadata) new UIPropertyMetadata((object) 150.0));
  public static readonly DependencyProperty OpacityFractionProperty = DependencyProperty.Register(nameof (OpacityFraction), typeof (double), typeof (Carousel), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN, new PropertyChangedCallback(Carousel.OnFractionChanged)));
  public static readonly DependencyProperty ScaleFractionProperty = DependencyProperty.Register(nameof (ScaleFraction), typeof (double), typeof (Carousel), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN, new PropertyChangedCallback(Carousel.OnFractionChanged)));
  public static readonly DependencyProperty RotationAngleProperty = DependencyProperty.Register(nameof (RotationAngle), typeof (double), typeof (Carousel), (PropertyMetadata) new UIPropertyMetadata((object) 0.0, new PropertyChangedCallback(Carousel.OnRationAngleChanged)));
  public static readonly DependencyProperty RotationSpeedProperty = DependencyProperty.Register(nameof (RotationSpeed), typeof (double), typeof (Carousel), (PropertyMetadata) new UIPropertyMetadata((object) 200.0));
  public static readonly DependencyProperty EnableRotationAnimationProperty = DependencyProperty.Register(nameof (EnableRotationAnimation), typeof (bool), typeof (Carousel), (PropertyMetadata) new UIPropertyMetadata((object) true));
  public static readonly DependencyProperty TopItemPositionProperty = DependencyProperty.Register(nameof (TopItemPosition), typeof (double), typeof (Carousel), new PropertyMetadata((object) 0.5, (PropertyChangedCallback) ((s, a) => ((Carousel) s).OnTopItemPositionChanged(s))));
  public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof (Path), typeof (Path), typeof (Carousel), new PropertyMetadata((object) null, new PropertyChangedCallback(Carousel.OnPathChanged)));
  public static readonly DependencyProperty EnableVirtualizationProperty = DependencyProperty.Register(nameof (EnableVirtualization), typeof (bool), typeof (Carousel), (PropertyMetadata) new UIPropertyMetadata((object) false));
  public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof (SelectedItem), typeof (object), typeof (Carousel), new PropertyMetadata((object) null, new PropertyChangedCallback(Carousel.OnSelectedItemChanged)));
  public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(nameof (SelectedValue), typeof (object), typeof (Carousel), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) => ((Carousel) s).OnSelectedValueChanged(a))));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (Carousel), (PropertyMetadata) new FrameworkPropertyMetadata((object) -1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (PropertyChangedCallback) ((s, a) => ((Carousel) s).OnSelectedIndexChanged(a)), new CoerceValueCallback(Carousel.CoerceSelectedIndex)));
  public static readonly DependencyProperty VisualModeProperty = DependencyProperty.Register(nameof (VisualMode), typeof (VisualMode), typeof (Carousel), (PropertyMetadata) new UIPropertyMetadata((object) VisualMode.Standard));
  public static readonly DependencyProperty EnableTouchProperty = DependencyProperty.Register(nameof (EnableTouch), typeof (bool), typeof (Carousel), new PropertyMetadata((object) false));

  static Carousel() => FusionLicenseProvider.GetLicenseType(Platform.WPF);

  public Carousel()
  {
    this.DefaultStyleKey = (object) typeof (Carousel);
    this.LayoutUpdated += new EventHandler(this.Carousel_LayoutUpdated);
    this.CommandBindings.Add(new CommandBinding((ICommand) Carousel.SelectFirstItemCommand, new ExecutedRoutedEventHandler(this.SelectFirstItem), new CanExecuteRoutedEventHandler(this.CanSelectFirstItem)));
    this.CommandBindings.Add(new CommandBinding((ICommand) Carousel.SelectLastItemCommand, new ExecutedRoutedEventHandler(this.SelectLastItem), new CanExecuteRoutedEventHandler(this.CanSelectLastItem)));
    this.CommandBindings.Add(new CommandBinding((ICommand) Carousel.SelectNextItemCommand, new ExecutedRoutedEventHandler(this.SelectNextItem), new CanExecuteRoutedEventHandler(this.CanSelectNextItem)));
    this.CommandBindings.Add(new CommandBinding((ICommand) Carousel.SelectPreviousItemCommand, new ExecutedRoutedEventHandler(this.SelectPreviousItem), new CanExecuteRoutedEventHandler(this.CanSelectPreviousItem)));
    this.CommandBindings.Add(new CommandBinding((ICommand) Carousel.SelectNextPageCommand, new ExecutedRoutedEventHandler(this.SelectNextPage), new CanExecuteRoutedEventHandler(this.CanSelectNextPage)));
    this.CommandBindings.Add(new CommandBinding((ICommand) Carousel.SelectPreviousPageCommand, new ExecutedRoutedEventHandler(this.SelectPreviousPage), new CanExecuteRoutedEventHandler(this.CanSelectPreviousPage)));
    this.InputBindings.Add((InputBinding) new KeyBinding((ICommand) Carousel.SelectFirstItemCommand, Key.Home, ModifierKeys.None));
    this.InputBindings.Add((InputBinding) new KeyBinding((ICommand) Carousel.SelectFirstItemCommand, Key.Left, ModifierKeys.Control));
    this.InputBindings.Add((InputBinding) new KeyBinding((ICommand) Carousel.SelectFirstItemCommand, Key.Up, ModifierKeys.Control));
    this.InputBindings.Add((InputBinding) new KeyBinding((ICommand) Carousel.SelectLastItemCommand, Key.End, ModifierKeys.None));
    this.InputBindings.Add((InputBinding) new KeyBinding((ICommand) Carousel.SelectLastItemCommand, Key.Right, ModifierKeys.Control));
    this.InputBindings.Add((InputBinding) new KeyBinding((ICommand) Carousel.SelectLastItemCommand, Key.Down, ModifierKeys.Control));
    this.InputBindings.Add((InputBinding) new KeyBinding((ICommand) Carousel.SelectNextItemCommand, Key.Right, ModifierKeys.None));
    this.InputBindings.Add((InputBinding) new KeyBinding((ICommand) Carousel.SelectNextItemCommand, Key.Down, ModifierKeys.None));
    this.InputBindings.Add((InputBinding) new KeyBinding((ICommand) Carousel.SelectPreviousItemCommand, Key.Left, ModifierKeys.None));
    this.InputBindings.Add((InputBinding) new KeyBinding((ICommand) Carousel.SelectPreviousItemCommand, Key.Up, ModifierKeys.None));
    this.InputBindings.Add((InputBinding) new KeyBinding((ICommand) Carousel.SelectNextPageCommand, Key.Next, ModifierKeys.None));
    this.InputBindings.Add((InputBinding) new KeyBinding((ICommand) Carousel.SelectPreviousPageCommand, Key.Prior, ModifierKeys.None));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public double SkewAngleXFraction
  {
    get => (double) this.GetValue(Carousel.SkewAngleXFractionProperty);
    set => this.SetValue(Carousel.SkewAngleXFractionProperty, (object) value);
  }

  public double SkewAngleYFraction
  {
    get => (double) this.GetValue(Carousel.SkewAngleYFractionProperty);
    set => this.SetValue(Carousel.SkewAngleYFractionProperty, (object) value);
  }

  public bool EnableLooping
  {
    get => (bool) this.GetValue(Carousel.EnableLoopingProperty);
    set => this.SetValue(Carousel.EnableLoopingProperty, (object) value);
  }

  public int ItemsPerPage
  {
    get => (int) this.GetValue(Carousel.ItemsPerPageProperty);
    set => this.SetValue(Carousel.ItemsPerPageProperty, (object) value);
  }

  public PathFractionCollection ScaleFractions
  {
    get => (PathFractionCollection) this.GetValue(Carousel.ScaleFractionsProperty);
    set => this.SetValue(Carousel.ScaleFractionsProperty, (object) value);
  }

  public bool ScalingEnabled
  {
    get => (bool) this.GetValue(Carousel.ScalingEnabledProperty);
    set => this.SetValue(Carousel.ScalingEnabledProperty, (object) value);
  }

  public bool OpacityEnabled
  {
    get => (bool) this.GetValue(Carousel.OpacityEnabledProperty);
    set => this.SetValue(Carousel.OpacityEnabledProperty, (object) value);
  }

  public PathFractionCollection OpacityFractions
  {
    get => (PathFractionCollection) this.GetValue(Carousel.OpacityFractionsProperty);
    set => this.SetValue(Carousel.OpacityFractionsProperty, (object) value);
  }

  public bool SkewAngleXEnabled
  {
    get => (bool) this.GetValue(Carousel.SkewAngleXEnabledProperty);
    set => this.SetValue(Carousel.SkewAngleXEnabledProperty, (object) value);
  }

  public PathFractionCollection SkewAngleXFractions
  {
    get => (PathFractionCollection) this.GetValue(Carousel.SkewAngleXFractionsProperty);
    set => this.SetValue(Carousel.SkewAngleXFractionsProperty, (object) value);
  }

  public bool SkewAngleYEnabled
  {
    get => (bool) this.GetValue(Carousel.SkewAngleYEnabledProperty);
    set => this.SetValue(Carousel.SkewAngleYEnabledProperty, (object) value);
  }

  public PathFractionCollection SkewAngleYFractions
  {
    get => (PathFractionCollection) this.GetValue(Carousel.SkewAngleYFractionsProperty);
    set => this.SetValue(Carousel.SkewAngleYFractionsProperty, (object) value);
  }

  public double RadiusX
  {
    get => (double) this.GetValue(Carousel.RadiusXProperty);
    set => this.SetValue(Carousel.RadiusXProperty, (object) value);
  }

  public double RadiusY
  {
    get => (double) this.GetValue(Carousel.RadiusYProperty);
    set => this.SetValue(Carousel.RadiusYProperty, (object) value);
  }

  public double OpacityFraction
  {
    get => (double) this.GetValue(Carousel.OpacityFractionProperty);
    set => this.SetValue(Carousel.OpacityFractionProperty, (object) value);
  }

  public double ScaleFraction
  {
    get => (double) this.GetValue(Carousel.ScaleFractionProperty);
    set => this.SetValue(Carousel.ScaleFractionProperty, (object) value);
  }

  public double RotationAngle
  {
    get => (double) this.GetValue(Carousel.RotationAngleProperty);
    set => this.SetValue(Carousel.RotationAngleProperty, (object) value);
  }

  public double RotationSpeed
  {
    get => (double) this.GetValue(Carousel.RotationSpeedProperty);
    set => this.SetValue(Carousel.RotationSpeedProperty, (object) value);
  }

  public bool EnableRotationAnimation
  {
    get => (bool) this.GetValue(Carousel.EnableRotationAnimationProperty);
    set => this.SetValue(Carousel.EnableRotationAnimationProperty, (object) value);
  }

  public double TopItemPosition
  {
    get => (double) this.GetValue(Carousel.TopItemPositionProperty);
    set => this.SetValue(Carousel.TopItemPositionProperty, (object) value);
  }

  public Path Path
  {
    get => (Path) this.GetValue(Carousel.PathProperty);
    set => this.SetValue(Carousel.PathProperty, (object) value);
  }

  public bool EnableVirtualization
  {
    get => (bool) this.GetValue(Carousel.EnableVirtualizationProperty);
    set => this.SetValue(Carousel.EnableVirtualizationProperty, (object) value);
  }

  public object SelectedItem
  {
    get => this.GetValue(Carousel.SelectedItemProperty);
    set => this.SetValue(Carousel.SelectedItemProperty, value);
  }

  public object SelectedValue
  {
    get => this.GetValue(Carousel.SelectedValueProperty);
    set => this.SetValue(Carousel.SelectedValueProperty, value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(Carousel.SelectedIndexProperty);
    set => this.SetValue(Carousel.SelectedIndexProperty, (object) value);
  }

  public VisualMode VisualMode
  {
    get => (VisualMode) this.GetValue(Carousel.VisualModeProperty);
    set => this.SetValue(Carousel.VisualModeProperty, (object) value);
  }

  public bool EnableTouch
  {
    get => (bool) this.GetValue(Carousel.EnableTouchProperty);
    set => this.SetValue(Carousel.EnableTouchProperty, (object) value);
  }

  internal new Panel ItemsHost
  {
    get
    {
      if (this._itemsHost == null && this.ItemContainerGenerator != null)
      {
        this._itemsHost = VisualUtils.GetItemsPanel((ItemsControl) this, typeof (CustomPathCarouselPanel));
        if (this._itemsHost != null)
          (this._itemsHost as CustomPathCarouselPanel).Owner = this;
      }
      return this._itemsHost;
    }
  }

  internal Panel HostPanel
  {
    get
    {
      return (Panel) (VisualUtils.FindDescendant((Visual) this, typeof (CarouselPanel)) as CarouselPanel);
    }
  }

  public static RoutedCommand SelectFirstItemCommand
  {
    get => Carousel.selectFirstItemCommand;
    internal set => Carousel.selectFirstItemCommand = value;
  }

  public static RoutedCommand SelectLastItemCommand
  {
    get => Carousel.selectLastItemCommand;
    internal set => Carousel.selectLastItemCommand = value;
  }

  public static RoutedCommand SelectNextItemCommand
  {
    get => Carousel.selectNextItemCommand;
    internal set => Carousel.selectNextItemCommand = value;
  }

  public static RoutedCommand SelectPreviousItemCommand
  {
    get => Carousel.selectPreviousItemCommand;
    internal set => Carousel.selectPreviousItemCommand = value;
  }

  public static RoutedCommand SelectNextPageCommand
  {
    get => Carousel.selectNextPageCommand;
    internal set => Carousel.selectNextPageCommand = value;
  }

  public static RoutedCommand SelectPreviousPageCommand
  {
    get => Carousel.selectPreviousPageCommand;
    internal set => Carousel.selectPreviousPageCommand = value;
  }

  public event PropertyChangedCallback SelectionChanged;

  public event PropertyChangedCallback SelectedIndexChanged;

  public event PropertyChangedCallback SelectedValueChanged;

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => this.Focus();

  protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    base.OnPropertyChanged(e);
    if (e.Property != Control.PaddingProperty || this.ItemsHost == null)
      return;
    CustomPathCarouselPanel itemsHost = (CustomPathCarouselPanel) this.ItemsHost;
    if (itemsHost == null)
      return;
    itemsHost.InvalidateMeasure();
    itemsHost.InvalidateArrange();
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);
    if (e.Delta < 0)
      this.SelectPreviousItem();
    else
      this.SelectNextItem();
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is CarouselItem;

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new CarouselItem();
  }

  protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnItemsSourceChanged(oldValue, newValue);
    if (this.ItemsHost != null && this.IsLoaded)
      this.SelectedItem = (object) null;
    this.SetSelectedItem();
  }

  protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
  {
    base.OnItemsChanged(e);
    if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null && e.OldItems.Count > 0 && e.OldItems[0] != null && this.previousSelected != null && this.Items != null && this.Items.Count > 0 && !this.Items.Contains((object) this.previousSelected))
      this.previousSelected = (CarouselItem) null;
    if (e.Action != NotifyCollectionChangedAction.Reset)
      return;
    if (this.ItemsHost != null && this.IsLoaded)
    {
      (this.ItemsHost as CustomPathCarouselPanel).Refresh(false, 0);
    }
    else
    {
      if (this.previousSelected == null || this.Items == null || this.Items.Contains((object) this.previousSelected))
        return;
      this.previousSelected = (CarouselItem) null;
    }
  }

  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    if (element is CarouselItem carouselItem)
      carouselItem.Owner = this;
    base.PrepareContainerForItemOverride(element, item);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.horizontalScrollBar = this.GetTemplateChild("horizontalScrollBar") as ScrollBar;
    this.verticalScrollBar = this.GetTemplateChild("verticalScrollBar") as ScrollBar;
    this.UpdateLargeChangeValue();
    this.IsManipulationEnabled = true;
  }

  protected override void OnManipulationStarting(ManipulationStartingEventArgs e)
  {
    this.Focus();
    if (!this.EnableTouch)
      return;
    e.ManipulationContainer = (IInputElement) this;
    this.RenderTransform = (Transform) this.transform;
    e.Handled = true;
    base.OnManipulationStarting(e);
  }

  protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
  {
    if (!this.EnableTouch)
      return;
    if (Math.Abs(e.CumulativeManipulation.Translation.X) >= 30.0 && this.HostPanel is CarouselPanel)
    {
      (this.HostPanel as CarouselPanel)._currentRotation = e.CumulativeManipulation.Translation.X % 360.0;
      (this.HostPanel as CarouselPanel).InvalidateArrange();
    }
    base.OnManipulationDelta(e);
  }

  private static void OnFractionChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    Carousel carousel = (Carousel) obj;
    if (carousel == null || carousel.ItemsHost == null)
      return;
    ((CustomPathCarouselPanel) carousel.ItemsHost)?.Invalidate(false);
  }

  private static void OnPathChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
  {
    Carousel carousel = (Carousel) obj;
    if (carousel == null || carousel.ItemsHost == null)
      return;
    CustomPathCarouselPanel itemsHost = (CustomPathCarouselPanel) carousel.ItemsHost;
    if (itemsHost == null)
      return;
    itemsHost.Path = carousel.Path;
    itemsHost.Invalidate(false);
  }

  private static void OnEnableLoopingChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    Carousel carousel = (Carousel) obj;
    if (carousel == null)
      return;
    CustomPathCarouselPanel itemsHost = (CustomPathCarouselPanel) carousel.ItemsHost;
    if (itemsHost == null)
      return;
    itemsHost.InvalidateMeasure();
    itemsHost.InvalidateArrange();
    itemsHost.Refresh(true, 0);
  }

  private static void OnRationAngleChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(sender is Carousel carousel))
      return;
    carousel.RenderTransform = (Transform) new RotateTransform()
    {
      Angle = (double) args.NewValue
    };
  }

  private static void OnSelectedItemChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((Carousel) obj)?.OnSelectedItemChanged(args);
  }

  private static object CoerceSelectedIndex(DependencyObject d, object baseValue)
  {
    Carousel carousel = d as Carousel;
    return carousel.SelectedIndex >= 0 && (carousel.HostPanel is CarouselPanel && (carousel.HostPanel as CarouselPanel)._timer.IsEnabled || carousel.ItemsHost is CustomPathCarouselPanel && (carousel.ItemsHost as CustomPathCarouselPanel).tempVirtualizingPanelHandler != null && (carousel.ItemsHost as CustomPathCarouselPanel).tempVirtualizingPanelHandler.State == ItemMovementState.Started) ? (object) carousel.SelectedIndex : baseValue;
  }

  public void SelectFirstItem() => this.SelectedIndex = 0;

  public void SelectLastItem() => this.SelectedIndex = this.Items.Count - 1;

  public void SelectNextItem()
  {
    this.SelectedIndex = this.SelectedIndex < this.Items.Count - 1 ? this.SelectedIndex + 1 : (this.EnableLooping || this.VisualMode == VisualMode.Standard ? 0 : this.Items.Count - 1);
  }

  public void SelectPreviousItem()
  {
    this.SelectedIndex = this.SelectedIndex > 0 ? this.SelectedIndex - 1 : (this.EnableLooping || this.VisualMode == VisualMode.Standard ? this.Items.Count - 1 : 0);
  }

  public void SelectNextPage()
  {
    if (this.ItemsPerPage <= 0 || this.ItemsPerPage >= this.Items.Count)
      return;
    this.isNextPage = true;
    int num = this.SelectedIndex + this.ItemsPerPage;
    this.SelectedIndex = num < this.Items.Count ? num : (this.EnableLooping || this.VisualMode == VisualMode.Standard ? num % this.Items.Count : this.Items.Count - 1);
  }

  public void SelectPreviousPage()
  {
    if (this.ItemsPerPage <= 0 || this.ItemsPerPage >= this.Items.Count)
      return;
    this.isPreviousPage = true;
    int num = this.SelectedIndex - this.ItemsPerPage;
    this.SelectedIndex = num >= 0 ? num : (this.EnableLooping || this.VisualMode == VisualMode.Standard ? this.Items.Count + num : 0);
  }

  public void Dispose()
  {
    this.horizontalScrollBar = (ScrollBar) null;
    this.verticalScrollBar = (ScrollBar) null;
  }

  internal int CoerceItemsPerPageValue(int itemsPerPage)
  {
    if (itemsPerPage > 0 && itemsPerPage % 2 == 0)
      itemsPerPage = itemsPerPage >= this.Items.Count ? this.Items.Count : itemsPerPage + 1;
    return itemsPerPage;
  }

  protected void OnItemsPerPageChanged(DependencyObject obj)
  {
    if (this.ItemsHost != null)
      ((CustomPathCarouselPanel) this.ItemsHost).ItemsPerPage = this.CoerceItemsPerPageValue(this.ItemsPerPage);
    this.UpdateLargeChangeValue();
  }

  protected void OnTopItemPositionChanged(DependencyObject obj)
  {
  }

  protected void OnItemsVisualChanged(DependencyObject obj)
  {
    if (this.ItemsHost != null)
    {
      this.SetVisualProperties();
      this.ItemsHost.InvalidateMeasure();
      CustomPathCarouselPanel itemsHost = (CustomPathCarouselPanel) this.ItemsHost;
      itemsHost.ItemsPerPage = this.CoerceItemsPerPageValue(this.ItemsPerPage);
      if (this.TopItemPosition < 1.0)
        return;
      itemsHost.TopItemPosition = this.TopItemPosition;
      itemsHost.InvalidateMeasure();
    }
    else
      this.IsVisualChanged = true;
  }

  protected void OnSelectedItemChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.VisualMode == VisualMode.Standard)
    {
      if (this.HostPanel != null && this.HostPanel is CarouselPanel)
      {
        if (this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) is CarouselItem element)
        {
          this.SelectedValue = element.Content;
          this.SelectedItem = element.Content;
        }
        ((CarouselPanel) this.HostPanel).SelectElement((FrameworkElement) element, this.isNextPage, this.isPreviousPage);
        ((CarouselPanel) this.HostPanel).currentIndex = ((CarouselPanel) this.HostPanel).GetSelectedItem((FrameworkElement) element);
        this.isNextPage = this.isPreviousPage = false;
      }
    }
    else if (this.VisualMode == VisualMode.CustomPath)
    {
      if (args.OldValue != null)
      {
        CarouselItem carouselItem = (CarouselItem) null;
        if (args.OldValue is CarouselItem)
          carouselItem = args.OldValue as CarouselItem;
        else if (this.ItemContainerGenerator.ContainerFromItem(args.OldValue) is CarouselItem)
          carouselItem = this.ItemContainerGenerator.ContainerFromItem(args.OldValue) as CarouselItem;
        if (carouselItem != null)
          carouselItem.IsSelected = false;
      }
      if (this.SelectedItem == null)
      {
        this.SelectedIndex = -1;
        this.SelectedValue = (object) null;
      }
      else
      {
        int num = this.Items.IndexOf(this.SelectedItem);
        if (num != -1)
        {
          this.SelectedIndex = num;
          this.SelectedValue = this.SelectedItem;
        }
      }
      CarouselItem carouselItem1 = (CarouselItem) null;
      if (this.SelectedItem is CarouselItem)
        carouselItem1 = args.NewValue as CarouselItem;
      else if (this.ItemContainerGenerator.ContainerFromItem(args.NewValue) is CarouselItem)
        carouselItem1 = this.ItemContainerGenerator.ContainerFromItem(args.NewValue) as CarouselItem;
      switch (carouselItem1)
      {
        case null:
        case null:
          if (this.ItemsHost != null && carouselItem1 != null)
          {
            ((CustomPathCarouselPanel) this.ItemsHost).FinishItemMovements();
            ((CustomPathCarouselPanel) this.ItemsHost).BringItemIntoView((UIElement) carouselItem1, true);
            break;
          }
          if (this.ItemsHost != null)
          {
            int num = this.Items.IndexOf(args.OldValue);
            int displacement = this.Items.IndexOf(args.NewValue) - num;
            ((CustomPathCarouselPanel) this.ItemsHost).FinishItemMovements();
            if (Math.Abs(displacement) > 1)
            {
              if (!this.isNextPage && !this.isPreviousPage)
                this.isPreviousPage = this.isNextPage = true;
              else
                displacement = this.isNextPage ? Math.Abs(displacement) : (this.isPreviousPage ? -Math.Abs(displacement) : displacement);
            }
            ((CustomPathCarouselPanel) this.ItemsHost).MoveItemInternallyforoutofrange(displacement);
            break;
          }
          break;
        default:
          carouselItem1.IsSelected = true;
          goto case null;
      }
    }
    if (this.SelectionChanged == null)
      return;
    this.SelectionChanged((DependencyObject) this, args);
  }

  protected void OnSelectedValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.Items.Contains(this.SelectedValue))
      this.SelectedIndex = this.Items.IndexOf(this.SelectedValue);
    if (this.SelectedValueChanged == null)
      return;
    this.SelectedValueChanged((DependencyObject) this, args);
  }

  protected virtual void OnSelectedIndexChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.SelectedIndex < 0)
      this.SelectedItem = (object) null;
    else
      this.SetSelectedItem();
    if (this.SelectedIndexChanged == null)
      return;
    this.SelectedIndexChanged((DependencyObject) this, args);
  }

  private void Carousel_LayoutUpdated(object sender, EventArgs e)
  {
    if (this.ItemsHost == null)
      return;
    if (this.IsVisualChanged)
    {
      this.SetVisualProperties();
      this.InvalidateMeasure();
      this.IsVisualChanged = false;
    }
    CustomPathCarouselPanel itemsHost = (CustomPathCarouselPanel) this.ItemsHost;
    if (itemsHost != null)
    {
      if (this.Path != null)
        itemsHost.Path = this.Path;
      if (this.TopItemPosition != itemsHost.TopItemPosition)
        itemsHost.TopItemPosition = this.TopItemPosition;
      if (itemsHost.ItemsPerPage < 0)
        itemsHost.ItemsPerPage = this.ItemsPerPage == -1 || this.Items.Count < this.ItemsPerPage && this.Items.Count > 0 ? this.CoerceItemsPerPageValue(this.Items.Count) : this.CoerceItemsPerPageValue(this.ItemsPerPage);
    }
    this.LayoutUpdated -= new EventHandler(this.Carousel_LayoutUpdated);
  }

  private void SetSelectedItem()
  {
    if (this.SelectedIndex < 0 || this.SelectedIndex >= this.Items.Count || this.SelectedIndex == this.Items.IndexOf(this.SelectedItem))
      return;
    this.SelectedItem = this.Items[this.SelectedIndex];
    if (!(this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) is CarouselItem carouselItem))
      return;
    carouselItem.IsSelected = true;
    this.previousSelected = carouselItem;
  }

  private void UpdateLargeChangeValue()
  {
    if (this.ItemsPerPage < 0)
      return;
    if (this.horizontalScrollBar != null)
      this.horizontalScrollBar.LargeChange = Convert.ToDouble(this.ItemsPerPage);
    if (this.verticalScrollBar == null)
      return;
    this.verticalScrollBar.LargeChange = Convert.ToDouble(this.ItemsPerPage);
  }

  private void SetVisualProperties()
  {
    CustomPathCarouselPanel itemsHost = (CustomPathCarouselPanel) this.ItemsHost;
    if (itemsHost == null)
      return;
    itemsHost.ScalingEnabled = this.ScalingEnabled;
    if (this.ScaleFractions != null)
    {
      if (itemsHost.ScaleFractions != null)
      {
        itemsHost.ScaleFractions.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnFractionCollectionChanged);
        itemsHost.ScaleFractions = (PathFractionCollection) null;
      }
      itemsHost.ScaleFractions = new PathFractionCollection();
      itemsHost.ScaleFractions.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnFractionCollectionChanged);
      foreach (FractionValue scaleFraction in (Collection<FractionValue>) this.ScaleFractions)
        itemsHost.ScaleFractions.Add(scaleFraction);
    }
    itemsHost.OpacityEnabled = this.OpacityEnabled;
    if (this.OpacityFractions != null)
    {
      if (itemsHost.OpacityFractions != null)
      {
        itemsHost.OpacityFractions.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnFractionCollectionChanged);
        itemsHost.OpacityFractions = (PathFractionCollection) null;
      }
      itemsHost.OpacityFractions = new PathFractionCollection();
      itemsHost.OpacityFractions.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnFractionCollectionChanged);
      foreach (FractionValue opacityFraction in (Collection<FractionValue>) this.OpacityFractions)
        itemsHost.OpacityFractions.Add(opacityFraction);
    }
    itemsHost.SkewAngleXEnabled = this.SkewAngleXEnabled;
    if (this.SkewAngleXFractions != null)
    {
      if (itemsHost.SkewAngleXFractions != null)
      {
        itemsHost.SkewAngleXFractions.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnFractionCollectionChanged);
        itemsHost.SkewAngleXFractions = (PathFractionCollection) null;
      }
      itemsHost.SkewAngleXFractions = new PathFractionCollection();
      itemsHost.SkewAngleXFractions.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnFractionCollectionChanged);
      foreach (FractionValue skewAngleXfraction in (Collection<FractionValue>) this.SkewAngleXFractions)
        itemsHost.SkewAngleXFractions.Add(skewAngleXfraction);
    }
    itemsHost.SkewAngleYEnabled = this.SkewAngleYEnabled;
    if (this.SkewAngleYFractions == null)
      return;
    if (itemsHost.SkewAngleYFractions != null)
    {
      itemsHost.SkewAngleYFractions.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnFractionCollectionChanged);
      itemsHost.SkewAngleYFractions = (PathFractionCollection) null;
    }
    itemsHost.SkewAngleYFractions = new PathFractionCollection();
    itemsHost.SkewAngleYFractions.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnFractionCollectionChanged);
    foreach (FractionValue skewAngleYfraction in (Collection<FractionValue>) this.SkewAngleYFractions)
      itemsHost.SkewAngleYFractions.Add(skewAngleYfraction);
  }

  private void OnFractionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.OldItems != null)
    {
      foreach (FractionValue oldItem in (IEnumerable) e.OldItems)
        oldItem.PropertyChanged = (System.Action) null;
    }
    if (e.NewItems == null)
      return;
    foreach (FractionValue newItem in (IEnumerable) e.NewItems)
      newItem.PropertyChanged = new System.Action(this.OnFractionValuePropertyChanged);
  }

  private void OnFractionValuePropertyChanged()
  {
    if (this.ItemsHost == null)
      return;
    ((CustomPathCarouselPanel) this.ItemsHost)?.Invalidate(false);
  }

  private void CanSelectFirstItem(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.SelectedIndex != 0;
  }

  private void SelectFirstItem(object sender, ExecutedRoutedEventArgs e)
  {
    (sender as Carousel).SelectFirstItem();
  }

  private void CanSelectLastItem(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.SelectedIndex != this.Items.Count - 1;
  }

  private void SelectLastItem(object sender, ExecutedRoutedEventArgs e)
  {
    (sender as Carousel).SelectLastItem();
  }

  private void CanSelectNextItem(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.EnableLooping || this.VisualMode == VisualMode.Standard || this.SelectedIndex != this.Items.Count - 1;
  }

  private void SelectNextItem(object sender, ExecutedRoutedEventArgs e)
  {
    (sender as Carousel).SelectNextItem();
  }

  private void CanSelectPreviousItem(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.EnableLooping || this.VisualMode == VisualMode.Standard || this.SelectedIndex != 0;
  }

  private void SelectPreviousItem(object sender, ExecutedRoutedEventArgs e)
  {
    (sender as Carousel).SelectPreviousItem();
  }

  private void CanSelectNextPage(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.EnableLooping || this.VisualMode == VisualMode.Standard || this.SelectedIndex != this.Items.Count - 1;
  }

  private void SelectNextPage(object sender, ExecutedRoutedEventArgs e)
  {
    (sender as Carousel).SelectNextPage();
  }

  private void CanSelectPreviousPage(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.EnableLooping || this.VisualMode == VisualMode.Standard || this.SelectedIndex != 0;
  }

  private void SelectPreviousPage(object sender, ExecutedRoutedEventArgs e)
  {
    (sender as Carousel).SelectPreviousPage();
  }
}
