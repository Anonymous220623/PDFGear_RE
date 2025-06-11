// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TileViewControl
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (TileViewControl), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/TileView/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/SyncOrangeStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TileView/Themes/ShinyRedStyle.xaml")]
public class TileViewControl : Selector, IDisposable
{
  internal int iCount;
  internal double draggedHeight;
  internal bool AllowAdd;
  internal double draggedWidth;
  internal TileViewItem DraggedItem;
  internal bool IsSplitterUsedinMinimizedState;
  internal TileViewItem SwappedfromMaximized;
  internal TileViewItem SwappedfromMinimized;
  internal bool IsSwapped;
  internal double OldDesiredHeight;
  internal double OldDesiredWidth;
  internal Dictionary<int, TileViewItem> TileViewItemOrder;
  internal ArrayList ItemsHeaderHeight = new ArrayList();
  internal ArrayList MinimizeditemsOrder = new ArrayList();
  internal double MinimizedVirtualItemSize = 100.0;
  internal TileViewVirtualizingPanel TileVirtualizingPanel;
  internal DispatcherTimer Timer = new DispatcherTimer();
  internal UIElementCollection VirtualizingTileItemsCollection;
  internal bool IsDragging;
  internal bool IsScroll;
  internal double MinimizedScrollStep;
  internal double MaximizedScrollStep;
  internal int RemoveAtVirtualPostion = -1;
  internal bool IsSizechanged;
  internal bool IsInsertOrRemoveItem;
  internal int firstVisibleIndex;
  internal TileViewItem PreviousMaximizedElement;
  internal int count;
  internal double ControlActualHeight;
  internal double ControlActualWidth;
  internal bool marginFlag = true;
  internal double LastminItemStore;
  internal int Rows = 1;
  internal int Columns = 1;
  private TileViewItem tileViewItem;
  internal TileViewItem maximizedItem;
  internal ObservableCollection<TileViewItem> tileViewItems = new ObservableCollection<TileViewItem>();
  internal List<TileViewItem> initialTileViewItems = new List<TileViewItem>();
  private DataTemplate tempContentTemplate;
  internal double LeftMargin;
  internal double RightMargin;
  internal double TopMargin;
  internal double BottomMargin;
  internal bool needToUpdate = true;
  internal Panel itemsPanel;
  internal bool isTileItemsLoaded;
  internal ScrollViewer scroll;
  internal Grid MainGrid;
  internal bool Update;
  internal double canvasheightonMinimized;
  internal double canvaswidthonMinimized;
  internal bool isheaderClicked;
  public static readonly DependencyProperty MinimizedItemsOrientationProperty = DependencyProperty.Register(nameof (MinimizedItemsOrientation), typeof (MinimizedItemsOrientation), typeof (TileViewControl), new PropertyMetadata((object) MinimizedItemsOrientation.Right, new PropertyChangedCallback(TileViewControl.OnMinimizedItemsOrientationChanged)));
  public static readonly DependencyProperty CurrentItemsOrderProperty = DependencyProperty.Register(nameof (CurrentItemsOrder), typeof (List<int>), typeof (TileViewControl), new PropertyMetadata((object) null, new PropertyChangedCallback(TileViewControl.OnCurrentItemsOrderChanged), new CoerceValueCallback(TileViewControl.CoerceCurrentItemsOrder)));
  public static readonly DependencyProperty EnableTouchProperty = DependencyProperty.Register(nameof (EnableTouch), typeof (bool), typeof (TileViewControl), new PropertyMetadata((object) false));
  public static readonly DependencyProperty EnableAnimationProperty = DependencyProperty.Register(nameof (EnableAnimation), typeof (bool), typeof (TileViewControl), new PropertyMetadata((object) true, new PropertyChangedCallback(TileViewControl.OnEnableAnimationChanged)));
  public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register(nameof (AnimationDuration), typeof (TimeSpan), typeof (TileViewControl), new PropertyMetadata((object) new TimeSpan(0, 0, 0, 0, 700), new PropertyChangedCallback(TileViewControl.OnAnimationDurationChanged)));
  public static readonly DependencyProperty AllowItemRepositioningProperty = DependencyProperty.Register(nameof (AllowItemRepositioning), typeof (bool), typeof (TileViewControl), new PropertyMetadata((object) true, new PropertyChangedCallback(TileViewControl.OnAllowItemRepositioningChanged)));
  public static readonly DependencyProperty MinimizedItemsPercentageProperty = DependencyProperty.Register(nameof (MinimizedItemsPercentage), typeof (double), typeof (TileViewControl), new PropertyMetadata((object) 20.0, new PropertyChangedCallback(TileViewControl.OnMinimizedItemsPercentageChanged)));
  public static readonly DependencyProperty IsMinMaxButtonOnMouseOverOnlyProperty = DependencyProperty.Register(nameof (IsMinMaxButtonOnMouseOverOnly), typeof (bool), typeof (TileViewControl), new PropertyMetadata((object) false, new PropertyChangedCallback(TileViewControl.OnIsMinMaxButtonOnMouseOverOnlyChanged)));
  public static readonly DependencyProperty IsVirtualizingProperty = DependencyProperty.Register(nameof (IsVirtualizing), typeof (bool), typeof (TileViewControl), new PropertyMetadata((object) false));
  public static readonly DependencyProperty RowHeightProperty = DependencyProperty.Register(nameof (RowHeight), typeof (GridLength), typeof (TileViewControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.Register(nameof (ColumnWidth), typeof (GridLength), typeof (TileViewControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty RowCountProperty = DependencyProperty.Register(nameof (RowCount), typeof (int), typeof (TileViewControl), new PropertyMetadata((object) 0, new PropertyChangedCallback(TileViewControl.OnRowCountChanged), new CoerceValueCallback(TileViewControl.CoerceRowCount)));
  public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.Register(nameof (ColumnCount), typeof (int), typeof (TileViewControl), new PropertyMetadata((object) 0, new PropertyChangedCallback(TileViewControl.OnColumnCountChanged), new CoerceValueCallback(TileViewControl.CoerceColumnCount)));
  public static readonly DependencyProperty SplitterThicknessProperty = DependencyProperty.Register(nameof (SplitterThickness), typeof (double), typeof (TileViewControl), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(TileViewControl.OnSplitterThicknessChanged)));
  public static readonly DependencyProperty SplitterColorProperty = DependencyProperty.Register(nameof (SplitterColor), typeof (Brush), typeof (TileViewControl), new PropertyMetadata((object) new SolidColorBrush(Colors.Gray)));
  public static readonly DependencyProperty SplitterVisibilityProperty = DependencyProperty.Register(nameof (SplitterVisibility), typeof (Visibility), typeof (TileViewControl), new PropertyMetadata((object) Visibility.Collapsed, new PropertyChangedCallback(TileViewControl.OnSplitterVisibilityChanged)));
  public static readonly DependencyProperty MinimizedItemTemplateProperty = DependencyProperty.Register(nameof (MinimizedItemTemplate), typeof (DataTemplate), typeof (TileViewControl), new PropertyMetadata((object) null, new PropertyChangedCallback(TileViewControl.OnMinimizedItemTemplateChanged)));
  public static readonly DependencyProperty MaximizedItemTemplateProperty = DependencyProperty.Register(nameof (MaximizedItemTemplate), typeof (DataTemplate), typeof (TileViewControl), new PropertyMetadata((object) null, new PropertyChangedCallback(TileViewControl.OnMaximizedItemTemplateChanged)));
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (TileViewControl), new PropertyMetadata((object) null, new PropertyChangedCallback(TileViewControl.OnHeaderTemplateChanged)));
  public static readonly DependencyProperty ClickHeaderToMaximizeProperty = DependencyProperty.Register(nameof (ClickHeaderToMaximize), typeof (bool), typeof (TileViewControl), new PropertyMetadata((object) false, new PropertyChangedCallback(TileViewControl.OnClickHeaderToMaximizePropertyChanged)));
  public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register(nameof (VerticalScrollBarVisibility), typeof (ScrollBarVisibility), typeof (TileViewControl), new PropertyMetadata((object) ScrollBarVisibility.Disabled, new PropertyChangedCallback(TileViewControl.OnScrollBarVisibilityPropertyChanged)));
  public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register(nameof (HorizontalScrollBarVisibility), typeof (ScrollBarVisibility), typeof (TileViewControl), new PropertyMetadata((object) ScrollBarVisibility.Disabled, new PropertyChangedCallback(TileViewControl.OnScrollBarVisibilityPropertyChanged)));
  public static readonly DependencyProperty MinimizedHeaderTemplateProperty = DependencyProperty.Register(nameof (MinimizedHeaderTemplate), typeof (DataTemplate), typeof (TileViewControl), new PropertyMetadata((object) null, new PropertyChangedCallback(TileViewControl.OnMinimizedHeaderTemplateChanged)));
  public static readonly DependencyProperty MaximizedHeaderTemplateProperty = DependencyProperty.Register(nameof (MaximizedHeaderTemplate), typeof (DataTemplate), typeof (TileViewControl), new PropertyMetadata((object) null, new PropertyChangedCallback(TileViewControl.OnMaximizedHeaderTemplateChanged)));
  public static readonly DependencyProperty UseNormalStateProperty = DependencyProperty.Register(nameof (UseNormalState), typeof (bool), typeof (TileViewControl), new PropertyMetadata((object) true, new PropertyChangedCallback(TileViewControl.OnUseNormalStatePropertyChanged)));
  private int tempitemcount;
  private List<object> _tileviewcanvasleft = new List<object>();
  private List<object> _tileviewcanvastop = new List<object>();
  private List<object> updatedcanvasleft = new List<object>();
  private List<object> sortedcanvasleft = new List<object>();
  private List<int> currentposition = new List<int>();
  internal List<TileViewItem> updatedtileviewitems = new List<TileViewItem>();
  internal List<TileViewItem> orderedtileviewitems = new List<TileViewItem>();
  private List<object> updatedcanvastop = new List<object>();
  internal bool isReOrdered;
  internal bool initialload = true;

  internal new ScrollViewer ScrollHost => (ScrollViewer) this.GetTemplateChild("scrollviewer");

  public TileViewControl()
  {
    if (DesignerProperties.GetIsInDesignMode((DependencyObject) this))
    {
      if (this.ActualHeight == 0.0)
        this.Height = 150.0;
      if (this.ActualWidth == 0.0)
        this.Width = 150.0;
    }
    this.SizeChanged -= new SizeChangedEventHandler(this.TileViewControl_SizeChanged);
    this.LayoutUpdated -= new EventHandler(this.TileViewControl_LayoutUpdated);
    this.SelectionChanged -= new SelectionChangedEventHandler(this.TileViewControl_SelectionChanged);
    this.ItemContainerGenerator.StatusChanged -= new EventHandler(this.ItemContainerGenerator_StatusChanged);
    this.Timer.Tick += new EventHandler(this.timer_Tick);
    this.Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
    this.count = 0;
    this.SizeChanged += new SizeChangedEventHandler(this.TileViewControl_SizeChanged);
    this.LayoutUpdated += new EventHandler(this.TileViewControl_LayoutUpdated);
    this.DefaultStyleKey = (object) typeof (TileViewControl);
    this.SelectionChanged += new SelectionChangedEventHandler(this.TileViewControl_SelectionChanged);
    this.ItemContainerGenerator.StatusChanged += new EventHandler(this.ItemContainerGenerator_StatusChanged);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void timer_Tick(object sender, EventArgs e)
  {
    this.TileVirtualizingPanel.InvalidateMeasure();
  }

  static TileViewControl() => FusionLicenseProvider.GetLicenseType(Platform.WPF);

  internal void SetState(TileViewItem item, TileViewItemState state)
  {
    item.SetCurrentValue(TileViewItem.TileViewItemStateProperty, (object) state);
  }

  private void TileViewControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (e.AddedItems.Count == 0)
      return;
    if (e.AddedItems[0] is TileViewItem addedItem)
    {
      addedItem.IsSelected = true;
    }
    else
    {
      if (this.SelectedIndex <= 0 || !(this.ItemContainerGenerator.ContainerFromIndex(this.SelectedIndex) is TileViewItem tileViewItem))
        return;
      tileViewItem.IsSelected = true;
    }
  }

  private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
  {
    if (this.SelectedIndex < 0 || !(this.ItemContainerGenerator.ContainerFromIndex(this.SelectedIndex) is TileViewItem tileViewItem))
      return;
    tileViewItem.IsSelected = true;
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    base.OnGotFocus(e);
    this.GetCanvasHeight();
    this.UpdateTileViewLayout();
  }

  protected override void OnInitialized(EventArgs e)
  {
    base.OnInitialized(e);
    this.Dispatcher.ShutdownFinished += new EventHandler(this.Dispatcher_ShutdownFinished);
    this.Loaded += new RoutedEventHandler(this.TileViewControl_Loaded);
    this.Unloaded += new RoutedEventHandler(this.TileViewControl_Unloaded);
  }

  private void TileViewControl_Loaded(object sender, RoutedEventArgs e)
  {
    this.isTileItemsLoaded = true;
    if (!this.IsVirtualizing)
    {
      if (this.initialTileViewItems != null && this.initialTileViewItems.Count != 0)
      {
        this.currentposition.Clear();
        for (int index = 0; index < this.initialTileViewItems.Count; ++index)
          this.currentposition.Add(index);
      }
      this.CurrentItemsOrder = this.ValidateCurrentItemsOrder(this.CurrentItemsOrder, this.currentposition);
      this.SetRowsAndColumns(this.GetTileViewItemOrder());
      this.UpdateTileViewLayout();
    }
    this.SizeChanged -= new SizeChangedEventHandler(this.TileViewControl_SizeChanged);
    this.SizeChanged += new SizeChangedEventHandler(this.TileViewControl_SizeChanged);
    this.LayoutUpdated -= new EventHandler(this.TileViewControl_LayoutUpdated);
    this.LayoutUpdated += new EventHandler(this.TileViewControl_LayoutUpdated);
    this.Dispatcher.ShutdownFinished -= new EventHandler(this.Dispatcher_ShutdownFinished);
    this.Dispatcher.ShutdownFinished += new EventHandler(this.Dispatcher_ShutdownFinished);
    this.Unloaded -= new RoutedEventHandler(this.TileViewControl_Unloaded);
    this.Unloaded += new RoutedEventHandler(this.TileViewControl_Unloaded);
    bool flag = false;
    foreach (object obj in (IEnumerable) this.Items)
    {
      if (this.ItemContainerGenerator.ContainerFromItem(obj) is TileViewItem tileViewItem && tileViewItem.TileViewItemState == TileViewItemState.Maximized)
        flag = true;
    }
    foreach (object obj in (IEnumerable) this.Items)
    {
      TileViewItem tileViewItem1 = this.ItemContainerGenerator.ContainerFromItem(obj) as TileViewItem;
      if (!this.UseNormalState)
      {
        TileViewItem tileViewItem2 = this.ItemContainerGenerator.ContainerFromItem(this.Items[0]) as TileViewItem;
        if (tileViewItem1 == tileViewItem2)
          this.SetState(tileViewItem1, TileViewItemState.Maximized);
        else
          this.SetState(tileViewItem1, TileViewItemState.Minimized);
      }
      else if (flag)
      {
        if (tileViewItem1 != null && tileViewItem1.TileViewItemState != TileViewItemState.Maximized)
          tileViewItem1.TileViewItemState = TileViewItemState.Minimized;
        else if (tileViewItem1 != null && tileViewItem1.TileViewItemState == TileViewItemState.Maximized)
          tileViewItem1.TileViewItemMaximize();
      }
      if (tileViewItem1 != null)
      {
        this.EndTileViewItemDragEvents(tileViewItem1);
        this.StartTileViewItemDragEvents(tileViewItem1);
      }
    }
  }

  private void TileViewControl_Unloaded(object sender, RoutedEventArgs e)
  {
    this.isTileItemsLoaded = false;
    foreach (object obj in (IEnumerable) this.Items)
    {
      if (this.ItemContainerGenerator.ContainerFromItem(obj) is TileViewItem RC)
        this.EndTileViewItemDragEvents(RC);
    }
    this.LayoutUpdated -= new EventHandler(this.TileViewControl_LayoutUpdated);
    this.Dispatcher.ShutdownFinished -= new EventHandler(this.Dispatcher_ShutdownFinished);
    this.Unloaded -= new RoutedEventHandler(this.TileViewControl_Unloaded);
  }

  private void ClearLocal(DependencyObject obj)
  {
    string str = "System.Windows.Style";
    LocalValueEnumerator localValueEnumerator = obj.GetLocalValueEnumerator();
    while (localValueEnumerator.MoveNext())
    {
      DependencyProperty property = localValueEnumerator.Current.Property;
      if (!property.ReadOnly && !(property.DefaultMetadata.DefaultValue is bool) && !property.PropertyType.FullName.Equals(str.ToString()))
        obj.ClearValue(property);
    }
  }

  private void Dispatcher_ShutdownFinished(object sender, EventArgs e)
  {
    foreach (object obj in (IEnumerable) this.Items)
    {
      if (this.ItemContainerGenerator.ContainerFromItem(obj) is TileViewItem RC)
        this.EndTileViewItemDragEvents(RC);
    }
    this.EndReferences();
    this.Timer.Tick -= new EventHandler(this.timer_Tick);
    this.SizeChanged -= new SizeChangedEventHandler(this.TileViewControl_SizeChanged);
    this.LayoutUpdated -= new EventHandler(this.TileViewControl_LayoutUpdated);
    this.Unloaded -= new RoutedEventHandler(this.TileViewControl_Unloaded);
    this.Dispatcher.ShutdownFinished -= new EventHandler(this.Dispatcher_ShutdownFinished);
    if (this.TileVirtualizingPanel == null)
      return;
    this.TileVirtualizingPanel.Dispose(true);
    this.TileVirtualizingPanel = (TileViewVirtualizingPanel) null;
  }

  private void EndReferences()
  {
    this.DraggedItem = (TileViewItem) null;
    this.SwappedfromMaximized = (TileViewItem) null;
    this.SwappedfromMinimized = (TileViewItem) null;
    this.ItemsHeaderHeight = (ArrayList) null;
    this.MinimizeditemsOrder = (ArrayList) null;
    this.tileViewItem = (TileViewItem) null;
    this.maximizedItem = (TileViewItem) null;
    if (this.tileViewItems != null)
      this.tileViewItems.Clear();
    this.tileViewItems = (ObservableCollection<TileViewItem>) null;
    this.VirtualizingTileItemsCollection = (UIElementCollection) null;
  }

  public event PropertyChangedCallback IsClickHeaderToMaximizePropertyChanged;

  public event PropertyChangedCallback IsMinMaxButtonOnMouseOverOnlyChanged;

  public event PropertyChangedCallback IsSplitterVisibilityChanged;

  public event PropertyChangedCallback RowCountChanged;

  public event PropertyChangedCallback ColumnCountChanged;

  public event PropertyChangedCallback AllowItemRepositioningChanged;

  public event PropertyChangedCallback MinimizedItemsOrientationChanged;

  public event PropertyChangedCallback MinimizedItemsPercentageChanged;

  public event PropertyChangedCallback SplitterThicknessChanged;

  public event TileViewOrderChangeEventHandler Repositioned;

  public event TileViewCancelRepositioningEventHandler Repositioning;

  private static object CoerceCurrentItemsOrder(DependencyObject d, object baseValue)
  {
    TileViewControl tileViewControl = (TileViewControl) d;
    return (object) tileViewControl.ValidateCurrentItemsOrder((List<int>) baseValue, tileViewControl.CurrentItemsOrder);
  }

  public bool EnableTouch
  {
    get => (bool) this.GetValue(TileViewControl.EnableTouchProperty);
    set => this.SetValue(TileViewControl.EnableTouchProperty, (object) value);
  }

  public bool EnableAnimation
  {
    get => (bool) this.GetValue(TileViewControl.EnableAnimationProperty);
    set => this.SetValue(TileViewControl.EnableAnimationProperty, (object) value);
  }

  private static void OnEnableAnimationChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    TileViewControl tileViewControl = d as TileViewControl;
    if (!(bool) e.NewValue)
    {
      foreach (object obj in (IEnumerable) tileViewControl.Items)
      {
        TileViewItem tileViewItem = tileViewControl.ItemsSource == null ? obj as TileViewItem : tileViewControl.ItemContainerGenerator.ContainerFromItem(obj) as TileViewItem;
        if (tileViewItem != null)
        {
          if (tileViewItem.animationWidthKeyFrameSize != null)
            tileViewItem.animationWidthKeyFrameSize.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1.0));
          if (tileViewItem.animationHeightKeyFrameSize != null)
            tileViewItem.animationHeightKeyFrameSize.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1.0));
          if (tileViewItem.animationXKeyFramePosition != null)
            tileViewItem.animationXKeyFramePosition.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1.0));
          if (tileViewItem.animationYKeyFramePosition != null)
            tileViewItem.animationYKeyFramePosition.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1.0));
        }
      }
    }
    else
    {
      foreach (object obj in (IEnumerable) tileViewControl.Items)
      {
        TileViewItem tileViewItem = tileViewControl.ItemsSource == null ? obj as TileViewItem : tileViewControl.ItemContainerGenerator.ContainerFromItem(obj) as TileViewItem;
        if (tileViewItem != null)
        {
          if (tileViewItem.animationWidthKeyFrameSize != null)
            tileViewItem.animationWidthKeyFrameSize.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(tileViewControl.AnimationDuration.TotalMilliseconds));
          if (tileViewItem.animationHeightKeyFrameSize != null)
            tileViewItem.animationHeightKeyFrameSize.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(tileViewControl.AnimationDuration.TotalMilliseconds));
          if (tileViewItem.animationXKeyFramePosition != null)
            tileViewItem.animationXKeyFramePosition.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(tileViewControl.AnimationDuration.TotalMilliseconds));
          if (tileViewItem.animationYKeyFramePosition != null)
            tileViewItem.animationYKeyFramePosition.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(tileViewControl.AnimationDuration.TotalMilliseconds));
        }
      }
    }
  }

  public TimeSpan AnimationDuration
  {
    get => (TimeSpan) this.GetValue(TileViewControl.AnimationDurationProperty);
    set => this.SetValue(TileViewControl.AnimationDurationProperty, (object) value);
  }

  private static void OnAnimationDurationChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    TileViewControl tileViewControl = d as TileViewControl;
    if (!tileViewControl.EnableAnimation)
      return;
    if (tileViewControl.AnimationDuration.TotalMilliseconds <= 0.0)
      tileViewControl.AnimationDuration = TimeSpan.FromMilliseconds(1.0);
    foreach (object obj in (IEnumerable) tileViewControl.Items)
    {
      TileViewItem tileViewItem = tileViewControl.ItemsSource == null ? obj as TileViewItem : tileViewControl.ItemContainerGenerator.ContainerFromItem(obj) as TileViewItem;
      if (tileViewItem != null)
      {
        if (tileViewItem.animationWidthKeyFrameSize != null)
          tileViewItem.animationWidthKeyFrameSize.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(tileViewControl.AnimationDuration.TotalMilliseconds));
        if (tileViewItem.animationHeightKeyFrameSize != null)
          tileViewItem.animationHeightKeyFrameSize.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(tileViewControl.AnimationDuration.TotalMilliseconds));
        if (tileViewItem.animationXKeyFramePosition != null)
          tileViewItem.animationXKeyFramePosition.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(tileViewControl.AnimationDuration.TotalMilliseconds));
        if (tileViewItem.animationYKeyFramePosition != null)
          tileViewItem.animationYKeyFramePosition.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(tileViewControl.AnimationDuration.TotalMilliseconds));
      }
    }
  }

  public GridLength RowHeight
  {
    get => (GridLength) this.GetValue(TileViewControl.RowHeightProperty);
    set => this.SetValue(TileViewControl.RowHeightProperty, (object) value);
  }

  public GridLength ColumnWidth
  {
    get => (GridLength) this.GetValue(TileViewControl.ColumnWidthProperty);
    set => this.SetValue(TileViewControl.ColumnWidthProperty, (object) value);
  }

  public List<int> CurrentItemsOrder
  {
    get => (List<int>) this.GetValue(TileViewControl.CurrentItemsOrderProperty);
    set => this.SetValue(TileViewControl.CurrentItemsOrderProperty, (object) value);
  }

  public DataTemplate MinimizedItemTemplate
  {
    get => (DataTemplate) this.GetValue(TileViewControl.MinimizedItemTemplateProperty);
    set => this.SetValue(TileViewControl.MinimizedItemTemplateProperty, (object) value);
  }

  public DataTemplate MaximizedItemTemplate
  {
    get => (DataTemplate) this.GetValue(TileViewControl.MaximizedItemTemplateProperty);
    set => this.SetValue(TileViewControl.MaximizedItemTemplateProperty, (object) value);
  }

  public MinimizedItemsOrientation MinimizedItemsOrientation
  {
    get
    {
      return (MinimizedItemsOrientation) this.GetValue(TileViewControl.MinimizedItemsOrientationProperty);
    }
    set => this.SetValue(TileViewControl.MinimizedItemsOrientationProperty, (object) value);
  }

  public Visibility SplitterVisibility
  {
    get => (Visibility) this.GetValue(TileViewControl.SplitterVisibilityProperty);
    set => this.SetValue(TileViewControl.SplitterVisibilityProperty, (object) value);
  }

  public Brush SplitterColor
  {
    get => (Brush) this.GetValue(TileViewControl.SplitterColorProperty);
    set => this.SetValue(TileViewControl.SplitterColorProperty, (object) value);
  }

  public bool IsMinMaxButtonOnMouseOverOnly
  {
    get => (bool) this.GetValue(TileViewControl.IsMinMaxButtonOnMouseOverOnlyProperty);
    set => this.SetValue(TileViewControl.IsMinMaxButtonOnMouseOverOnlyProperty, (object) value);
  }

  internal double minimizedColumnWidth
  {
    get => Convert.ToDouble(this.ActualWidth * this.MinimizedItemsPercentage / 100.0);
    set
    {
    }
  }

  public int RowCount
  {
    get => (int) this.GetValue(TileViewControl.RowCountProperty);
    set => this.SetValue(TileViewControl.RowCountProperty, (object) value);
  }

  public int ColumnCount
  {
    get => (int) this.GetValue(TileViewControl.ColumnCountProperty);
    set => this.SetValue(TileViewControl.ColumnCountProperty, (object) value);
  }

  public double SplitterThickness
  {
    get => (double) this.GetValue(TileViewControl.SplitterThicknessProperty);
    set => this.SetValue(TileViewControl.SplitterThicknessProperty, (object) value);
  }

  public double MinimizedItemsPercentage
  {
    get => (double) this.GetValue(TileViewControl.MinimizedItemsPercentageProperty);
    set => this.SetValue(TileViewControl.MinimizedItemsPercentageProperty, (object) value);
  }

  public bool IsVirtualizing
  {
    get => (bool) this.GetValue(TileViewControl.IsVirtualizingProperty);
    set => this.SetValue(TileViewControl.IsVirtualizingProperty, (object) value);
  }

  internal double minimizedRowHeight
  {
    get => Convert.ToDouble(this.ActualHeight * this.MinimizedItemsPercentage / 100.0);
  }

  public bool AllowItemRepositioning
  {
    get => (bool) this.GetValue(TileViewControl.AllowItemRepositioningProperty);
    set => this.SetValue(TileViewControl.AllowItemRepositioningProperty, (object) value);
  }

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(TileViewControl.HeaderTemplateProperty);
    set => this.SetValue(TileViewControl.HeaderTemplateProperty, (object) value);
  }

  public ScrollBarVisibility HorizontalScrollBarVisibility
  {
    get
    {
      return (ScrollBarVisibility) this.GetValue(TileViewControl.HorizontalScrollBarVisibilityProperty);
    }
    set => this.SetValue(TileViewControl.HorizontalScrollBarVisibilityProperty, (object) value);
  }

  public ScrollBarVisibility VerticalScrollBarVisibility
  {
    get => (ScrollBarVisibility) this.GetValue(TileViewControl.VerticalScrollBarVisibilityProperty);
    set => this.SetValue(TileViewControl.VerticalScrollBarVisibilityProperty, (object) value);
  }

  public DataTemplate MinimizedHeaderTemplate
  {
    get => (DataTemplate) this.GetValue(TileViewControl.MinimizedHeaderTemplateProperty);
    set => this.SetValue(TileViewControl.MinimizedHeaderTemplateProperty, (object) value);
  }

  public DataTemplate MaximizedHeaderTemplate
  {
    get => (DataTemplate) this.GetValue(TileViewControl.MaximizedHeaderTemplateProperty);
    set => this.SetValue(TileViewControl.MaximizedHeaderTemplateProperty, (object) value);
  }

  public bool ClickHeaderToMaximize
  {
    get => (bool) this.GetValue(TileViewControl.ClickHeaderToMaximizeProperty);
    set => this.SetValue(TileViewControl.ClickHeaderToMaximizeProperty, (object) value);
  }

  public bool UseNormalState
  {
    get => (bool) this.GetValue(TileViewControl.UseNormalStateProperty);
    set => this.SetValue(TileViewControl.UseNormalStateProperty, (object) value);
  }

  internal Dictionary<int, TileViewItem> GetTileViewItemOrder()
  {
    Dictionary<int, TileViewItem> tileViewItemOrder = new Dictionary<int, TileViewItem>();
    ObservableCollection<TileViewItem> observableCollection = new ObservableCollection<TileViewItem>();
    if (this.orderedtileviewitems != null && this.orderedtileviewitems.Count <= this.Items.Count && this.isReOrdered)
    {
      this.orderedtileviewitems.Clear();
      if (!this.IsVirtualizing)
      {
        for (int index = 0; index <= this.Items.Count; ++index)
          this.orderedtileviewitems.Add((TileViewItem) this.ItemContainerGenerator.ContainerFromIndex(index));
      }
      else
      {
        for (int index = 0; index < this.VirtualizingTileItemsCollection.Count; ++index)
          this.orderedtileviewitems.Add((TileViewItem) this.ItemContainerGenerator.ContainerFromItem((object) this.VirtualizingTileItemsCollection[index]));
      }
    }
    if (this.isReOrdered)
    {
      if (this.orderedtileviewitems != null)
      {
        for (int key = 0; key < this.orderedtileviewitems.Count; ++key)
        {
          TileViewItem tileViewItem1 = (TileViewItem) null;
          for (int index = 0; index < this.orderedtileviewitems.Count; ++index)
          {
            TileViewItem tileViewItem2 = this.orderedtileviewitems[index] ?? (!this.IsVirtualizing || this.ItemsSource == null ? this.GetItemContainer((object) this.orderedtileviewitems[index]) : this.VirtualizingTileItemsCollection[index] as TileViewItem);
            bool flag = observableCollection.Contains(tileViewItem2);
            if (tileViewItem2 != null && tileViewItem2.Visibility != Visibility.Collapsed && !flag && tileViewItem1 == null)
            {
              if (tileViewItem2.Header != null)
              {
                if (tileViewItem2.Header.ToString() != "{DisconnectedItem}")
                  tileViewItem1 = tileViewItem2;
              }
              else
                tileViewItem1 = tileViewItem2;
            }
          }
          if (tileViewItem1 != null)
          {
            observableCollection.Add(tileViewItem1);
            tileViewItemOrder.Add(key, tileViewItem1);
          }
        }
        this.tileViewItems = observableCollection;
        if (this.AllowItemRepositioning && this.CurrentItemsOrder != null && !this.IsVirtualizing && this.tileViewItems.Count == this.CurrentItemsOrder.Count)
        {
          tileViewItemOrder.Clear();
          tileViewItemOrder = this.GetTileItemsOrderFromCurrentOrder();
        }
      }
    }
    else if (this.Items != null)
    {
      if (!this.IsVirtualizing)
      {
        for (int key = 0; key < this.Items.Count; ++key)
        {
          TileViewItem tileViewItem = (TileViewItem) null;
          for (int index = 0; index < this.Items.Count; ++index)
          {
            if (!(this.Items[index] is TileViewItem itemContainer))
              itemContainer = this.GetItemContainer(this.Items[index]);
            bool flag = observableCollection.Contains(itemContainer);
            if (itemContainer != null && itemContainer.Visibility != Visibility.Collapsed && !flag && tileViewItem == null)
            {
              if (itemContainer.Header != null)
              {
                if (itemContainer.Header.ToString() != "{DisconnectedItem}")
                  tileViewItem = itemContainer;
              }
              else
                tileViewItem = itemContainer;
            }
          }
          if (tileViewItem != null)
          {
            observableCollection.Add(tileViewItem);
            tileViewItemOrder.Add(key, tileViewItem);
          }
        }
        this.tileViewItems = observableCollection;
        if (this.AllowItemRepositioning && this.CurrentItemsOrder != null && !this.IsVirtualizing && this.tileViewItems.Count == this.CurrentItemsOrder.Count)
        {
          tileViewItemOrder.Clear();
          tileViewItemOrder = this.GetTileItemsOrderFromCurrentOrder();
        }
      }
      else
      {
        for (int key = 0; key < this.VirtualizingTileItemsCollection.Count; ++key)
        {
          TileViewItem tileViewItem3 = (TileViewItem) null;
          for (int index = 0; index < this.VirtualizingTileItemsCollection.Count; ++index)
          {
            if (!(this.VirtualizingTileItemsCollection[index] is TileViewItem tileViewItem4))
              tileViewItem4 = this.GetItemContainer((object) this.VirtualizingTileItemsCollection[index]);
            bool flag = observableCollection.Contains(tileViewItem4);
            if (tileViewItem4 != null && tileViewItem4.Visibility != Visibility.Collapsed && !flag && tileViewItem3 == null)
            {
              if (tileViewItem4.Header != null)
              {
                if (tileViewItem4.Header.ToString() != "{DisconnectedItem}")
                  tileViewItem3 = tileViewItem4;
              }
              else
                tileViewItem3 = tileViewItem4;
            }
          }
          if (tileViewItem3 != null)
          {
            observableCollection.Add(tileViewItem3);
            tileViewItemOrder.Add(key, tileViewItem3);
          }
        }
        this.tileViewItems = observableCollection;
      }
    }
    return tileViewItemOrder;
  }

  internal TileViewItem GetItemContainer(object obj)
  {
    return this.ItemContainerGenerator.ContainerFromItem(obj) as TileViewItem;
  }

  internal void GetTileViewItems()
  {
    this.tileViewItems.Clear();
    foreach (object obj in (IEnumerable) this.Items)
    {
      if (!(obj is TileViewItem))
      {
        TileViewItem itemContainer = this.GetItemContainer(obj);
        if (itemContainer != null && itemContainer.Visibility != Visibility.Collapsed)
          this.tileViewItems.Add(itemContainer);
      }
      else if ((obj as TileViewItem).Visibility != Visibility.Collapsed)
        this.tileViewItems.Add(obj as TileViewItem);
    }
  }

  internal virtual void StartTileViewItemDragEvents(TileViewItem repCard)
  {
    repCard.DragStartedEvent += new TileViewDragEventHandler(this.repCard_DragStarted);
    repCard.DragCompletedEvent += new TileViewDragEventHandler(this.repCard_DragFinished);
    repCard.DragMouseMoveEvent += new TileViewDragEventHandler(this.repCard_DragMoved);
    repCard.CardMaximized += new EventHandler(this.repCard_Maximized);
    repCard.CardNormal += new EventHandler(this.repCard_Normal);
    if (repCard.TileViewItemState != TileViewItemState.Maximized)
      return;
    this.maximizedItem = repCard;
    if (this.tileViewItems == null || this.tileViewItems == null)
      return;
    foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
    {
      if (repCard != tileViewItem)
        tileViewItem.TileViewItemsMinimizeMethod(this.MinimizedItemsOrientation);
    }
  }

  internal virtual void EndTileViewItemDragEvents(TileViewItem RC)
  {
    RC.DragStartedEvent -= new TileViewDragEventHandler(this.repCard_DragStarted);
    RC.DragCompletedEvent -= new TileViewDragEventHandler(this.repCard_DragFinished);
    RC.DragMouseMoveEvent -= new TileViewDragEventHandler(this.repCard_DragMoved);
    RC.CardMaximized -= new EventHandler(this.repCard_Maximized);
    RC.CardNormal -= new EventHandler(this.repCard_Normal);
  }

  internal void SetRowsAndColumns(Dictionary<int, TileViewItem> TileViewItemOrder)
  {
    this.tempitemcount = this.tileViewItems.Count;
    foreach (UIElement tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
    {
      if (tileViewItem.Visibility == Visibility.Collapsed)
        --this.tempitemcount;
    }
    if (this.tempitemcount == 0)
      return;
    if (this.RowCount > 0)
      this.Rows = this.RowCount;
    else if (this.tileViewItems != null)
    {
      if (!this.IsVirtualizing)
        this.Rows = Convert.ToInt32(Math.Floor(Math.Sqrt(Convert.ToDouble(this.tempitemcount))));
      else if (this.itemsPanel != null && this.itemsPanel is TileViewVirtualizingPanel)
      {
        if (this.Items.Count < this.TileVirtualizingPanel.VirtualColumn * this.TileVirtualizingPanel.VirtualRow)
        {
          this.Rows = Convert.ToInt32(Math.Floor(Math.Sqrt(Convert.ToDouble(this.tempitemcount))));
          this.TileVirtualizingPanel.VirtualRow = this.Rows;
        }
        else
          this.Rows = this.TileVirtualizingPanel.VirtualRow;
      }
    }
    if (this.ColumnCount > 0)
      this.Columns = this.ColumnCount;
    else if (this.tileViewItems != null && this.tileViewItems.Count != 0)
    {
      if (!this.IsVirtualizing)
        this.Columns = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.tempitemcount) / Convert.ToDouble(this.Rows)));
      else if (this.itemsPanel != null && this.itemsPanel is TileViewVirtualizingPanel)
      {
        if (this.Items.Count < this.TileVirtualizingPanel.VirtualColumn * this.TileVirtualizingPanel.VirtualRow)
        {
          this.Columns = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.tempitemcount) / Convert.ToDouble(this.Rows)));
          this.TileVirtualizingPanel.VirtualColumn = this.Columns;
        }
        else
          this.Columns = this.TileVirtualizingPanel.VirtualColumn;
      }
    }
    int key = 0;
    for (int index1 = 0; index1 < this.Rows; ++index1)
    {
      for (int index2 = 0; index2 < this.Columns; ++index2)
      {
        if (TileViewItemOrder.ContainsKey(key) && TileViewItemOrder[key] != null)
        {
          Grid.SetRow((UIElement) TileViewItemOrder[key], index1);
          Grid.SetColumn((UIElement) TileViewItemOrder[key], index2);
        }
        ++key;
        if (this.tileViewItems != null && key == this.tempitemcount)
          break;
      }
      if (this.tileViewItems != null && key == this.tempitemcount)
        break;
    }
  }

  public void UpdateTileViewLayout(bool needAnimation)
  {
    if (needAnimation)
    {
      this.GetTileViewItemsSizes();
      this.AnimateTileViewLayout();
    }
    else
    {
      this.GetTileViewItemsSizes();
      this.UpdateTileViewLayout();
    }
    this.GetCanvasHeight();
  }

  public bool CloseTileViewItem(TileViewItem CloseTileViewItem)
  {
    if (CloseTileViewItem == null)
      return false;
    this.SetState(CloseTileViewItem, TileViewItemState.Normal);
    if (this.SelectedItem != null && (this.SelectedItem.Equals((object) CloseTileViewItem) || CloseTileViewItem.DataContext != null && this.SelectedItem.Equals(CloseTileViewItem.DataContext)))
      this.SelectedItem = (object) null;
    switch (CloseTileViewItem.CloseMode)
    {
      case CloseMode.Hide:
        this.SetState(CloseTileViewItem, TileViewItemState.Hidden);
        return true;
      case CloseMode.Delete:
        this.SetState(CloseTileViewItem, TileViewItemState.Normal);
        this.Items.Remove((object) this);
        return true;
      default:
        return false;
    }
  }

  internal void listClear()
  {
    if (this.orderedtileviewitems == null || this.updatedtileviewitems == null || this.sortedcanvasleft == null || this._tileviewcanvasleft == null || this._tileviewcanvastop == null || this.updatedcanvasleft == null || this.updatedcanvastop == null)
      return;
    this.sortedcanvasleft.Clear();
    this._tileviewcanvasleft.Clear();
    this._tileviewcanvastop.Clear();
    this.updatedcanvasleft.Clear();
    this.updatedcanvastop.Clear();
    this.updatedtileviewitems.Clear();
    this.orderedtileviewitems.Clear();
    this.isReOrdered = false;
  }

  internal void UpdateTileViewLayout()
  {
    this.listClear();
    if (!this.IsVirtualizing)
    {
      this.RightMargin = this.VerticalScrollBarVisibility != ScrollBarVisibility.Visible ? 0.0 : 20.0;
      this.BottomMargin = this.HorizontalScrollBarVisibility != ScrollBarVisibility.Visible ? 0.0 : 20.0;
    }
    if (double.IsInfinity(this.ActualWidth) || double.IsNaN(this.ActualWidth) || this.ActualWidth != 0.0)
    {
      if (this.maximizedItem != null && !this.tileViewItems.Contains(this.maximizedItem))
        this.maximizedItem = (TileViewItem) null;
      if (this.maximizedItem == null)
      {
        if (this.tileViewItems != null)
        {
          double num1 = this.ActualWidth - (this.RightMargin + this.LeftMargin);
          double num2 = this.ActualHeight - (this.TopMargin + this.BottomMargin);
          foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
          {
            if (tileViewItem.IsSelected && this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) as TileViewItem != tileViewItem && this.IsVirtualizing)
              tileViewItem.IsSelected = false;
            if (this.tileViewItems.Count <= 1)
            {
              if (tileViewItem.minMaxButton != null)
              {
                tileViewItem.minMaxButton.IsEnabled = false;
                if (tileViewItem.ShareSpace && this.isTileItemsLoaded)
                {
                  this.SetState(tileViewItem, TileViewItemState.Maximized);
                }
                else
                {
                  tileViewItem.minMaxButton.IsEnabled = true;
                  this.SetState(tileViewItem, TileViewItemState.Normal);
                }
              }
            }
            else if (tileViewItem.minMaxButton != null)
              tileViewItem.minMaxButton.IsEnabled = true;
            int key = Grid.GetRow((UIElement) tileViewItem) * this.Columns + Grid.GetColumn((UIElement) tileViewItem);
            if (this.TileViewItemOrder != null && !this.TileViewItemOrder.ContainsKey(key))
              this.TileViewItemOrder.Add(key, tileViewItem);
            double num3 = 0.0;
            double num4 = 0.0;
            if (this.RowHeight.Value > 1.0 && this.Rows > 0)
              num3 = (double) this.Rows * this.RowHeight.Value;
            if (this.ColumnWidth.Value > 1.0 && this.Columns > 0)
              num4 = (double) this.Columns * this.ColumnWidth.Value;
            double a1 = this.ColumnWidth.Value <= 1.0 ? Convert.ToDouble((double) Grid.GetColumn((UIElement) tileViewItem) * (num1 / Convert.ToDouble(this.Columns))) : Convert.ToDouble((double) Grid.GetColumn((UIElement) tileViewItem) * (num4 / Convert.ToDouble(this.Columns)));
            double a2 = this.RowHeight.Value <= 1.0 ? Convert.ToDouble((double) Grid.GetRow((UIElement) tileViewItem) * (num2 / Convert.ToDouble(this.Rows))) : Convert.ToDouble((double) Grid.GetRow((UIElement) tileViewItem) * (num3 / Convert.ToDouble(this.Rows)));
            this._tileviewcanvasleft.Add((object) a1);
            this._tileviewcanvastop.Add((object) a2);
            if (this.IsVirtualizing)
            {
              Canvas.SetLeft((UIElement) tileViewItem, Math.Round(a1) - this.TileVirtualizingPanel.ScrollInfo.HorizontalOffset);
              Canvas.SetTop((UIElement) tileViewItem, Math.Round(a2) - this.MaximizedScrollStep);
            }
            else
            {
              Canvas.SetLeft((UIElement) tileViewItem, Math.Round(a1));
              Canvas.SetTop((UIElement) tileViewItem, Math.Round(a2));
            }
            if (this.IsScroll)
            {
              tileViewItem.animationXKeyFramePosition.Value = Canvas.GetLeft((UIElement) tileViewItem);
              tileViewItem.animationYKeyFramePosition.Value = Canvas.GetTop((UIElement) tileViewItem);
            }
            double d1 = this.ColumnWidth.Value <= 1.0 ? Convert.ToDouble(num1 / Convert.ToDouble(this.Columns) - tileViewItem.Margin.Left - tileViewItem.Margin.Right) : Convert.ToDouble(num4 / Convert.ToDouble(this.Columns) - tileViewItem.Margin.Left - tileViewItem.Margin.Right);
            double d2 = this.RowHeight.Value <= 1.0 ? Convert.ToDouble(num2 / Convert.ToDouble(this.Rows) - tileViewItem.Margin.Top - tileViewItem.Margin.Bottom) : Convert.ToDouble(num3 / Convert.ToDouble(this.Rows) - tileViewItem.Margin.Top - tileViewItem.Margin.Bottom);
            if (d1 < 0.0)
              d1 = 0.0;
            if (d2 < 0.0)
              d2 = 0.0;
            if (!double.IsInfinity(d2))
              tileViewItem.Height = d2;
            if (!double.IsInfinity(d1))
              tileViewItem.Width = d1;
            if (this.IsScroll)
            {
              tileViewItem.animationHeightKeyFrameSize.Value = d2;
              tileViewItem.animationWidthKeyFrameSize.Value = d1;
            }
          }
          if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
          {
            foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
            {
              if (tileViewItem.TileViewItemState == TileViewItemState.Normal && this.ActualHeight > 0.0 && this.itemsPanel != null)
              {
                GridLength gridLength;
                if (this.RowHeight.Value > 1.0 && (double) this.Rows * this.RowHeight.Value > this.ActualHeight)
                {
                  Panel itemsPanel = this.itemsPanel;
                  double rows = (double) this.Rows;
                  gridLength = this.RowHeight;
                  double num5 = gridLength.Value;
                  double num6 = rows * num5;
                  itemsPanel.Height = num6;
                }
                else
                  this.itemsPanel.Height = this.ActualHeight;
                gridLength = this.ColumnWidth;
                if (gridLength.Value > 1.0)
                {
                  double columns1 = (double) this.Columns;
                  gridLength = this.ColumnWidth;
                  double num7 = gridLength.Value;
                  if (columns1 * num7 > this.ActualWidth)
                  {
                    Panel itemsPanel = this.itemsPanel;
                    double columns2 = (double) this.Columns;
                    gridLength = this.ColumnWidth;
                    double num8 = gridLength.Value;
                    double num9 = columns2 * num8;
                    itemsPanel.Width = num9;
                    continue;
                  }
                }
                this.itemsPanel.Width = this.ActualWidth;
              }
            }
          }
          else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
          {
            foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
            {
              if (tileViewItem.TileViewItemState == TileViewItemState.Normal && this.ActualWidth > 0.0 && this.itemsPanel != null)
              {
                GridLength gridLength = this.RowHeight;
                if (gridLength.Value > 1.0)
                {
                  double rows = (double) this.Rows;
                  gridLength = this.RowHeight;
                  double num10 = gridLength.Value;
                  if (rows * num10 > this.ActualHeight)
                  {
                    Panel itemsPanel = this.itemsPanel;
                    double num11 = Convert.ToDouble(this.Rows);
                    gridLength = this.RowHeight;
                    double num12 = gridLength.Value;
                    double num13 = num11 * num12;
                    itemsPanel.Height = num13;
                    goto label_66;
                  }
                }
                this.itemsPanel.Height = this.ActualHeight;
label_66:
                gridLength = this.ColumnWidth;
                if (gridLength.Value > 1.0)
                {
                  double columns = (double) this.Columns;
                  gridLength = this.ColumnWidth;
                  double num14 = gridLength.Value;
                  if (columns * num14 > this.ActualWidth)
                  {
                    Panel itemsPanel = this.itemsPanel;
                    double num15 = Convert.ToDouble(this.Columns);
                    gridLength = this.ColumnWidth;
                    double num16 = gridLength.Value;
                    double num17 = num15 * num16;
                    itemsPanel.Width = num17;
                    continue;
                  }
                }
                this.itemsPanel.Width = this.ActualWidth;
              }
            }
          }
        }
      }
      else
      {
        double num18 = 0.0;
        this.TileViewItemOrder = new Dictionary<int, TileViewItem>();
        if (this.tileViewItems != null)
        {
          for (int index = 0; index < this.tileViewItems.Count; ++index)
          {
            TileViewItem tileViewItem = this.tileViewItems[index];
            if (this.tileViewItems.Count <= 1)
            {
              if (tileViewItem.minMaxButton != null)
              {
                tileViewItem.minMaxButton.IsEnabled = false;
                this.SetState(tileViewItem, TileViewItemState.Maximized);
              }
            }
            else if (tileViewItem.minMaxButton != null)
              tileViewItem.minMaxButton.IsEnabled = true;
            int key = this.IsVirtualizing ? index : Grid.GetRow((UIElement) tileViewItem) * this.Columns + Grid.GetColumn((UIElement) tileViewItem);
            if (!this.TileViewItemOrder.ContainsKey(key))
              this.TileViewItemOrder.Add(key, tileViewItem);
            int num19 = key + 1;
          }
        }
        double num20 = 0.0;
        double num21 = this.ActualWidth - (this.RightMargin + this.LeftMargin);
        double num22 = this.ActualHeight - (this.TopMargin + this.BottomMargin);
        Thickness margin;
        for (int key = 0; key < this.TileViewItemOrder.Count; ++key)
        {
          if (this.TileViewItemOrder.ContainsKey(key))
          {
            if (this.TileViewItemOrder[key].TileViewItemState != TileViewItemState.Maximized)
            {
              double num23 = 0.0;
              double num24 = 0.0;
              if (this.TileViewItemOrder[key].IsSelected && this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) as TileViewItem != this.TileViewItemOrder[key] && this.IsVirtualizing)
                this.TileViewItemOrder[key].IsSelected = false;
              if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
              {
                if (this.IsSplitterUsedinMinimizedState)
                {
                  if (this.OldDesiredWidth > 0.0 && num21 > 0.0)
                  {
                    double num25 = this.TileViewItemOrder[key].OnMinimizedWidth.Value;
                    double oldDesiredWidth = this.OldDesiredWidth;
                    margin = this.TileViewItemOrder[key].Margin;
                    double right1 = margin.Right;
                    margin = this.TileViewItemOrder[key].Margin;
                    double left1 = margin.Left;
                    double num26 = right1 + left1;
                    double num27 = oldDesiredWidth - num26;
                    double num28 = num25 / num27;
                    double num29 = num21;
                    margin = this.TileViewItemOrder[key].Margin;
                    double right2 = margin.Right;
                    double num30 = num29 - right2;
                    margin = this.TileViewItemOrder[key].Margin;
                    double left2 = margin.Left;
                    double num31 = num30 + left2;
                    num24 = num28 * num31;
                  }
                }
                else if (this.OldDesiredWidth != 0.0 || this.isheaderClicked)
                {
                  if (this.TileViewItemOrder[key].ActualWidth != 0.0 && this.tileViewItems != null)
                  {
                    if (this.TileViewItemOrder[key].OnMinimizedWidth.Value != 0.0)
                    {
                      double num32 = num21 / Convert.ToDouble(this.tileViewItems.Count - 1);
                      margin = this.TileViewItemOrder[key].Margin;
                      double left = margin.Left;
                      margin = this.TileViewItemOrder[key].Margin;
                      double right = margin.Right;
                      double num33 = left + right;
                      if (num32 - num33 < this.TileViewItemOrder[key].OnMinimizedWidth.Value && Application.Current != null && Application.Current.MainWindow != null && Application.Current.MainWindow.WindowState == WindowState.Normal)
                      {
                        num24 = this.TileViewItemOrder[key].OnMinimizedWidth.Value;
                        goto label_104;
                      }
                    }
                    num24 = this.TileViewItemOrder[key].OnMinimizedWidth.Value;
                  }
                }
                else if (this.TileViewItemOrder.Count > 1)
                {
                  double num34 = num21;
                  margin = this.TileViewItemOrder[key].Margin;
                  double right = margin.Right;
                  margin = this.TileViewItemOrder[key].Margin;
                  double left = margin.Left;
                  double num35 = right + left;
                  num24 = (num34 - num35) / (double) (this.TileViewItemOrder.Count - 1);
                }
                else
                  num24 = num21;
label_104:
                if (this.IsVirtualizing)
                {
                  double minimizedVirtualItemSize = this.MinimizedVirtualItemSize;
                  margin = this.TileViewItemOrder[key].Margin;
                  double left = margin.Left;
                  double num36 = minimizedVirtualItemSize - left;
                  margin = this.TileViewItemOrder[key].Margin;
                  double right = margin.Right;
                  num24 = num36 - right;
                }
                double minimizedRowHeight = this.minimizedRowHeight;
                margin = this.TileViewItemOrder[key].Margin;
                double top = margin.Top;
                double num37 = minimizedRowHeight - top;
                margin = this.TileViewItemOrder[key].Margin;
                double bottom = margin.Bottom;
                num23 = num37 - bottom;
              }
              else
              {
                double minimizedColumnWidth = this.minimizedColumnWidth;
                margin = this.TileViewItemOrder[key].Margin;
                double left = margin.Left;
                double num38 = minimizedColumnWidth - left;
                margin = this.TileViewItemOrder[key].Margin;
                double right = margin.Right;
                num24 = num38 - right;
                if (this.IsSplitterUsedinMinimizedState)
                {
                  if (this.OldDesiredHeight > 0.0 && num22 > 0.0)
                  {
                    double num39 = this.TileViewItemOrder[key].OnMinimizedHeight.Value;
                    double oldDesiredHeight = this.OldDesiredHeight;
                    margin = this.TileViewItemOrder[key].Margin;
                    double top = margin.Top;
                    margin = this.TileViewItemOrder[key].Margin;
                    double bottom = margin.Bottom;
                    double num40 = top + bottom;
                    double num41 = oldDesiredHeight - num40;
                    num23 = num39 / num41 * (num22 - num18);
                  }
                }
                else if (this.OldDesiredHeight != 0.0 || this.isheaderClicked)
                {
                  if (this.TileViewItemOrder[key].ActualHeight != 0.0)
                  {
                    if (this.tileViewItems != null && this.tileViewItems.Count > 1)
                    {
                      if (this.TileViewItemOrder[key].OnMinimizedHeight.Value != 0.0)
                      {
                        double num42 = num22 / Convert.ToDouble(this.tileViewItems.Count - 1);
                        margin = this.TileViewItemOrder[key].Margin;
                        double top = margin.Top;
                        margin = this.TileViewItemOrder[key].Margin;
                        double bottom = margin.Bottom;
                        double num43 = top + bottom;
                        if (num42 - num43 < this.TileViewItemOrder[key].OnMinimizedHeight.Value && Application.Current != null && Application.Current.MainWindow != null && Application.Current.MainWindow.WindowState == WindowState.Normal)
                        {
                          num23 = this.TileViewItemOrder[key].OnMinimizedHeight.Value;
                          goto label_120;
                        }
                      }
                      double num44 = num22 / Convert.ToDouble(this.tileViewItems.Count - 1);
                      margin = this.TileViewItemOrder[key].Margin;
                      double top1 = margin.Top;
                      margin = this.TileViewItemOrder[key].Margin;
                      double bottom1 = margin.Bottom;
                      double num45 = top1 + bottom1;
                      num23 = num44 - num45;
                    }
                    else
                      num23 = num22;
                  }
                }
                else if (this.TileViewItemOrder.Count == this.tileViewItems.Count)
                {
                  double num46 = num22;
                  margin = this.TileViewItemOrder[key].Margin;
                  double top = margin.Top;
                  margin = this.TileViewItemOrder[key].Margin;
                  double bottom = margin.Bottom;
                  double num47 = top + bottom;
                  num23 = (num46 - num47) / (double) (this.TileViewItemOrder.Count - 1);
                }
label_120:
                if (this.IsVirtualizing)
                {
                  double minimizedVirtualItemSize = this.MinimizedVirtualItemSize;
                  margin = this.TileViewItemOrder[key].Margin;
                  double top = margin.Top;
                  double num48 = minimizedVirtualItemSize - top;
                  margin = this.TileViewItemOrder[key].Margin;
                  double bottom = margin.Bottom;
                  num23 = num48 - bottom;
                }
              }
              if (num23 < 0.0)
                num23 = 0.0;
              if (num24 < 0.0)
                num24 = 0.0;
              if (num23 > 0.0 && this.TileViewItemOrder[key].OnMinimizedHeight.Value == 0.0)
                this.TileViewItemOrder[key].OnMinimizedHeight = new GridLength(num23, GridUnitType.Pixel);
              if (num24 > 0.0 && this.TileViewItemOrder[key].OnMinimizedWidth.Value == 0.0)
                this.TileViewItemOrder[key].OnMinimizedWidth = new GridLength(num24, GridUnitType.Pixel);
              this.TileViewItemOrder[key].Width = num24;
              this.TileViewItemOrder[key].Height = num23;
              double a3 = 0.0;
              double a4 = num20;
              if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
              {
                a3 = 0.0;
                a4 = num20;
              }
              else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top)
              {
                a3 = num20;
                a4 = 0.0;
              }
              else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
              {
                a3 = this.TileViewItemOrder.Count > 1 ? num21 - this.minimizedColumnWidth : 0.0;
                a4 = num20;
              }
              else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
              {
                a3 = num20;
                a4 = this.TileViewItemOrder.Count > 1 ? num22 - this.minimizedRowHeight : 0.0;
              }
              Canvas.SetLeft((UIElement) this.TileViewItemOrder[key], Math.Round(a3));
              Canvas.SetTop((UIElement) this.TileViewItemOrder[key], Math.Round(a4));
              if (this.IsScroll)
              {
                if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
                {
                  Canvas.SetTop((UIElement) this.TileViewItemOrder[key], Math.Round(Canvas.GetTop((UIElement) this.TileViewItemOrder[key]) - this.MinimizedScrollStep));
                  this.TileViewItemOrder[key].animationYKeyFramePosition.Value = Canvas.GetTop((UIElement) this.TileViewItemOrder[key]);
                }
                else
                {
                  Canvas.SetLeft((UIElement) this.TileViewItemOrder[key], Math.Round(Canvas.GetLeft((UIElement) this.TileViewItemOrder[key]) - this.MinimizedScrollStep));
                  this.TileViewItemOrder[key].animationXKeyFramePosition.Value = Canvas.GetLeft((UIElement) this.TileViewItemOrder[key]);
                }
              }
              if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
              {
                if (this.TileViewItemOrder.Count - 1 != key)
                {
                  double num49 = num20;
                  double num50 = num23;
                  margin = this.TileViewItemOrder[key + 1].Margin;
                  double top = margin.Top;
                  double num51 = num50 + top;
                  margin = this.TileViewItemOrder[key].Margin;
                  double bottom = margin.Bottom;
                  double num52 = num51 + bottom;
                  num20 = num49 + num52;
                }
              }
              else if (this.TileViewItemOrder.Count - 1 != key)
              {
                double num53 = num20;
                double num54 = num24;
                margin = this.TileViewItemOrder[key + 1].Margin;
                double left = margin.Left;
                double num55 = num54 + left;
                margin = this.TileViewItemOrder[key].Margin;
                double right = margin.Right;
                double num56 = num55 + right;
                num20 = num53 + num56;
              }
            }
            else
            {
              double num57 = num21 - this.minimizedColumnWidth;
              margin = this.TileViewItemOrder[key].Margin;
              double left3 = margin.Left;
              double num58 = num57 - left3;
              margin = this.TileViewItemOrder[key].Margin;
              double right3 = margin.Right;
              double num59 = num58 - right3;
              double num60 = num22;
              margin = this.TileViewItemOrder[key].Margin;
              double top2 = margin.Top;
              double num61 = num60 - top2;
              margin = this.TileViewItemOrder[key].Margin;
              double bottom2 = margin.Bottom;
              double num62 = num61 - bottom2;
              double num63;
              double num64;
              if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
              {
                double num65 = num21;
                margin = this.TileViewItemOrder[key].Margin;
                double left4 = margin.Left;
                double num66 = num65 - left4;
                margin = this.TileViewItemOrder[key].Margin;
                double right4 = margin.Right;
                num63 = num66 - right4;
                if (this.TileViewItemOrder[key].TileViewItemState == TileViewItemState.Maximized && this.TileViewItemOrder.Count == 1)
                {
                  double num67 = num22;
                  margin = this.TileViewItemOrder[key].Margin;
                  double top3 = margin.Top;
                  double num68 = num67 - top3;
                  margin = this.TileViewItemOrder[key].Margin;
                  double bottom3 = margin.Bottom;
                  num64 = num68 - bottom3;
                }
                else
                {
                  double num69 = num22 - this.minimizedRowHeight;
                  margin = this.TileViewItemOrder[key].Margin;
                  double top4 = margin.Top;
                  double num70 = num69 - top4;
                  margin = this.TileViewItemOrder[key].Margin;
                  double bottom4 = margin.Bottom;
                  num64 = num70 - bottom4;
                }
              }
              else
              {
                if (this.TileViewItemOrder[key].TileViewItemState == TileViewItemState.Maximized && this.TileViewItemOrder.Count == 1)
                {
                  double num71 = num21;
                  margin = this.TileViewItemOrder[key].Margin;
                  double left5 = margin.Left;
                  double num72 = num71 - left5;
                  margin = this.TileViewItemOrder[key].Margin;
                  double right5 = margin.Right;
                  num63 = num72 - right5;
                }
                else
                {
                  double num73 = num21 - this.minimizedColumnWidth;
                  margin = this.TileViewItemOrder[key].Margin;
                  double left6 = margin.Left;
                  double num74 = num73 - left6;
                  margin = this.TileViewItemOrder[key].Margin;
                  double right6 = margin.Right;
                  num63 = num74 - right6;
                }
                double num75 = num22;
                margin = this.TileViewItemOrder[key].Margin;
                double top5 = margin.Top;
                double num76 = num75 - top5;
                margin = this.TileViewItemOrder[key].Margin;
                double bottom5 = margin.Bottom;
                num64 = num76 - bottom5;
              }
              if (num64 < 0.0)
                num64 = 0.0;
              if (num63 < 0.0)
                num63 = 0.0;
              this.TileViewItemOrder[key].Width = num63;
              this.TileViewItemOrder[key].Height = num64;
              double a5 = 0.0;
              double a6 = 0.0;
              if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
              {
                a5 = this.TileViewItemOrder.Count > 1 ? this.minimizedColumnWidth : 0.0;
                a6 = 0.0;
              }
              else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top)
              {
                a5 = 0.0;
                a6 = this.minimizedRowHeight;
              }
              else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
              {
                a5 = 0.0;
                a6 = 0.0;
              }
              else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
              {
                a5 = 0.0;
                a6 = 0.0;
              }
              Canvas.SetLeft((UIElement) this.TileViewItemOrder[key], Math.Round(a5));
              Canvas.SetTop((UIElement) this.TileViewItemOrder[key], Math.Round(a6));
            }
          }
        }
      }
    }
    int num77 = 0;
    int index1 = 0;
    if (this.tileViewItems != null && this.orderedtileviewitems != null)
    {
      for (int index2 = 0; index2 < this.tileViewItems.Count; ++index2)
      {
        for (int index3 = 0; index3 < this.updatedcanvastop.Count; ++index3)
        {
          if (this._tileviewcanvastop[index2].Equals(this.updatedcanvastop[index3]))
            ++num77;
        }
        if (num77 == 0 && this._tileviewcanvastop.Count > 0)
          this.updatedcanvastop.Add((object) Convert.ToDouble(this._tileviewcanvastop[index2]));
        else
          num77 = 0;
      }
      this.updatedcanvastop.Sort();
      int num78 = 0;
      for (; index1 < this.updatedcanvastop.Count; ++index1)
      {
        int index4 = 0;
        for (int index5 = 0; index5 < this.tileViewItems.Count; ++index5)
        {
          if (Convert.ToDouble(this._tileviewcanvastop[index5]) == Convert.ToDouble(this.updatedcanvastop[index1]))
          {
            this.updatedtileviewitems.Add(this.tileViewItems[index5]);
            this.updatedcanvasleft.Add(this._tileviewcanvasleft[index5]);
            this.sortedcanvasleft.Add(this._tileviewcanvasleft[index5]);
          }
        }
        this.sortedcanvasleft.Sort();
        for (int index6 = num78; index6 < this.updatedtileviewitems.Count; ++index6)
        {
          for (int index7 = num78; index7 < this.updatedtileviewitems.Count; ++index7)
          {
            if (index4 < this.sortedcanvasleft.Count && this.sortedcanvasleft[index4] == this.updatedcanvasleft[index7])
            {
              this.orderedtileviewitems.Add(this.updatedtileviewitems[index7]);
              this.isReOrdered = true;
            }
          }
          ++index4;
        }
        num78 = this.updatedtileviewitems.Count;
        this.sortedcanvasleft.Clear();
      }
      this.currentposition.Clear();
      List<TileViewItem> tileViewItemList = new List<TileViewItem>();
      List<TileViewItem> list = this.initialTileViewItems.ToList<TileViewItem>();
      if (this.orderedtileviewitems.Count > 0 && this.initialTileViewItems.Count != this.orderedtileviewitems.Count)
      {
        for (int index8 = 0; index8 < list.Count; ++index8)
        {
          bool flag = false;
          for (int index9 = 0; index9 < this.orderedtileviewitems.Count; ++index9)
          {
            if (list[index8] == this.orderedtileviewitems[index9])
            {
              flag = true;
              break;
            }
            flag = false;
          }
          if (!flag)
            this.initialTileViewItems.Remove(list[index8]);
        }
      }
      if (this.initialTileViewItems.Count == this.orderedtileviewitems.Count)
      {
        for (int index10 = 0; index10 < this.initialTileViewItems.Count; ++index10)
        {
          for (int index11 = 0; index11 < this.initialTileViewItems.Count; ++index11)
          {
            if (this.orderedtileviewitems.Count > 0 && this.initialTileViewItems[index11] == this.orderedtileviewitems[index10])
              this.currentposition.Add(index11);
          }
        }
      }
      for (int index12 = 0; index12 < this.tileViewItems.Count; ++index12)
      {
        if (this.orderedtileviewitems.Count > 0)
        {
          this.tileViewItems[index12] = this.orderedtileviewitems[index12];
          this.sortedcanvasleft.Add((object) Convert.ToDouble((double) Grid.GetColumn((UIElement) this.tileViewItems[index12]) * (this.ActualWidth / Convert.ToDouble(this.Columns))));
        }
      }
    }
    if (this.maximizedItem != null && !this.IsVirtualizing && this.initialTileViewItems != null && this.TileViewItemOrder != null && this.initialTileViewItems.Count == this.TileViewItemOrder.Count)
    {
      this.currentposition.Clear();
      for (int key = 0; key < this.initialTileViewItems.Count; ++key)
      {
        for (int index13 = 0; index13 < this.TileViewItemOrder.Count; ++index13)
        {
          if (this.initialTileViewItems[index13] == this.TileViewItemOrder[key])
            this.currentposition.Add(index13);
        }
      }
    }
    if (this.isTileItemsLoaded)
      this.CurrentItemsOrder = this.currentposition;
    if (this.Repositioned == null)
      return;
    this.Repositioned((object) this, new TileViewEventArgs());
  }

  internal void GetTileViewItemsSizes()
  {
    if (double.IsInfinity(this.ActualWidth) || double.IsNaN(this.ActualWidth) || this.ActualWidth == 0.0)
      return;
    if (this.maximizedItem != null && !this.tileViewItems.Contains(this.maximizedItem))
      this.maximizedItem = (TileViewItem) null;
    if (this.maximizedItem == null)
    {
      if (this.tileViewItems == null)
        return;
      double num1 = this.ActualWidth - (this.RightMargin + this.LeftMargin);
      double num2 = this.ActualHeight - (this.TopMargin + this.BottomMargin);
      foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      {
        if (this.tileViewItems.Count <= 1)
        {
          if (tileViewItem.minMaxButton != null)
          {
            tileViewItem.minMaxButton.IsEnabled = false;
            if (tileViewItem.ShareSpace)
              this.SetState(tileViewItem, TileViewItemState.Maximized);
            else
              this.SetState(tileViewItem, TileViewItemState.Normal);
          }
        }
        else if (tileViewItem.minMaxButton != null)
          tileViewItem.minMaxButton.IsEnabled = true;
        double num3 = 0.0;
        double num4 = 0.0;
        if (this.RowHeight.Value > 1.0 && this.Rows > 0)
          num3 = (double) this.Rows * this.RowHeight.Value;
        if (this.ColumnWidth.Value > 1.0 && this.Columns > 0)
          num4 = (double) this.Columns * this.ColumnWidth.Value;
        double width = this.ColumnWidth.Value <= 1.0 ? num1 / Convert.ToDouble(this.Columns) - tileViewItem.Margin.Left - tileViewItem.Margin.Right : num4 / Convert.ToDouble(this.Columns) - tileViewItem.Margin.Left - tileViewItem.Margin.Right;
        double height = this.RowHeight.Value <= 1.0 ? num2 / Convert.ToDouble(this.Rows) - tileViewItem.Margin.Top - tileViewItem.Margin.Bottom : num3 / Convert.ToDouble(this.Rows) - tileViewItem.Margin.Top - tileViewItem.Margin.Bottom;
        if (width < 0.0)
          width = 0.0;
        if (height < 0.0)
          height = 0.0;
        tileViewItem.AnimateSize(width, height);
      }
    }
    else
    {
      double num5 = 0.0;
      double num6 = this.ActualWidth - (this.RightMargin + this.LeftMargin);
      double num7 = this.ActualHeight - (this.TopMargin + this.BottomMargin);
      double num8 = 0.0;
      double num9 = 0.0;
      double num10 = 0.0;
      double num11 = 0.0;
      if (this.tileViewItems != null)
      {
        foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
        {
          if (this.tileViewItems.Count <= 1)
          {
            if (tileViewItem.minMaxButton != null)
            {
              tileViewItem.minMaxButton.IsEnabled = false;
              this.SetState(tileViewItem, TileViewItemState.Maximized);
            }
          }
          else if (tileViewItem.minMaxButton != null)
            tileViewItem.minMaxButton.IsEnabled = true;
          GridLength gridLength;
          Thickness margin;
          if (tileViewItem != this.maximizedItem)
          {
            if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
            {
              if (tileViewItem.OnMinimizedWidth.GridUnitType == GridUnitType.Auto)
              {
                ++num5;
                num10 += tileViewItem.Margin.Right + tileViewItem.Margin.Left;
              }
              else if (tileViewItem.OnMinimizedWidth.GridUnitType == GridUnitType.Star)
              {
                num5 += tileViewItem.OnMinimizedWidth.Value;
                num10 += tileViewItem.Margin.Right + tileViewItem.Margin.Left;
              }
              else
              {
                if (this.ActualWidth > 0.0)
                {
                  double num12 = tileViewItem.OnMinimizedWidth.Value != 0.0 ? tileViewItem.OnMinimizedWidth.Value : (this.ActualWidth - num10) / (double) (this.tileViewItems.Count - 1);
                  gridLength = tileViewItem.OnMinimizedWidth;
                  if (gridLength.Value > num12)
                    tileViewItem.OnMinimizedWidth = new GridLength(num12, GridUnitType.Pixel);
                }
                double num13 = num6;
                gridLength = tileViewItem.OnMinimizedWidth;
                double num14 = gridLength.Value;
                margin = tileViewItem.Margin;
                double right = margin.Right;
                double num15 = num14 + right;
                margin = tileViewItem.Margin;
                double left = margin.Left;
                double num16 = num15 + left;
                num6 = num13 - num16;
              }
            }
            else
            {
              gridLength = tileViewItem.OnMinimizedHeight;
              if (gridLength.GridUnitType == GridUnitType.Auto)
              {
                ++num5;
                double num17 = num10;
                margin = tileViewItem.Margin;
                double top = margin.Top;
                margin = tileViewItem.Margin;
                double bottom = margin.Bottom;
                double num18 = top + bottom;
                num10 = num17 + num18;
              }
              else
              {
                gridLength = tileViewItem.OnMinimizedHeight;
                if (gridLength.GridUnitType == GridUnitType.Star)
                {
                  double num19 = num5;
                  gridLength = tileViewItem.OnMinimizedHeight;
                  double num20 = gridLength.Value;
                  num5 = num19 + num20;
                  double num21 = num10;
                  margin = tileViewItem.Margin;
                  double top = margin.Top;
                  margin = tileViewItem.Margin;
                  double bottom = margin.Bottom;
                  double num22 = top + bottom;
                  num10 = num21 + num22;
                }
                else
                {
                  if (this.ActualHeight > 0.0)
                  {
                    gridLength = tileViewItem.OnMinimizedHeight;
                    double num23;
                    if (gridLength.Value == 0.0)
                    {
                      num23 = (this.ActualHeight - num10) / (double) (this.tileViewItems.Count - 1);
                    }
                    else
                    {
                      gridLength = tileViewItem.OnMinimizedHeight;
                      num23 = gridLength.Value;
                    }
                    gridLength = tileViewItem.OnMinimizedHeight;
                    if (gridLength.Value > num23)
                      tileViewItem.OnMinimizedHeight = new GridLength(num23, GridUnitType.Pixel);
                  }
                  double num24 = num7;
                  gridLength = tileViewItem.OnMinimizedHeight;
                  double num25 = gridLength.Value;
                  margin = tileViewItem.Margin;
                  double top = margin.Top;
                  double num26 = num25 + top;
                  margin = tileViewItem.Margin;
                  double bottom = margin.Bottom;
                  double num27 = num26 + bottom;
                  num7 = num24 - num27;
                }
              }
            }
          }
        }
      }
      if (num5 != 0.0)
      {
        double num28 = this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom ? (num6 - num10) / num5 : (num7 - num10) / num5;
      }
      int num29 = 0;
      if (this.tileViewItems == null)
        return;
      double a1 = this.ActualWidth - (this.RightMargin + this.LeftMargin);
      double num30 = this.ActualHeight - (this.TopMargin + this.BottomMargin);
      Thickness margin1;
      foreach (TileViewItem tileViewItem1 in (Collection<TileViewItem>) this.tileViewItems)
      {
        if (tileViewItem1 != this.maximizedItem)
        {
          ++num29;
          double height;
          GridLength gridLength1;
          double width;
          if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
          {
            double minimizedRowHeight = this.minimizedRowHeight;
            margin1 = tileViewItem1.Margin;
            double right1 = margin1.Right;
            double num31 = minimizedRowHeight - right1;
            margin1 = tileViewItem1.Margin;
            double left1 = margin1.Left;
            height = num31 - left1;
            gridLength1 = tileViewItem1.OnMinimizedWidth;
            if (gridLength1.GridUnitType == GridUnitType.Star)
            {
              width = (a1 - num8) / (double) (this.tileViewItems.Count - 1);
              tileViewItem1.OnMinimizedWidth = new GridLength(width, GridUnitType.Pixel);
            }
            else
            {
              gridLength1 = tileViewItem1.OnMinimizedWidth;
              if (gridLength1.GridUnitType == GridUnitType.Auto)
              {
                width = (a1 - num8) / (double) (this.tileViewItems.Count - 1);
                tileViewItem1.OnMinimizedWidth = new GridLength(width, GridUnitType.Pixel);
              }
              else
              {
                double a2 = 0.0;
                double num32 = 0.0;
                foreach (TileViewItem tileViewItem2 in (Collection<TileViewItem>) this.tileViewItems)
                {
                  if (tileViewItem2.TileViewItemState != TileViewItemState.Maximized)
                  {
                    double num33 = a2;
                    gridLength1 = tileViewItem2.OnMinimizedWidth;
                    double num34 = gridLength1.Value;
                    a2 = num33 + num34;
                    double num35 = num32;
                    margin1 = tileViewItem1.Margin;
                    double top = margin1.Top;
                    margin1 = tileViewItem1.Margin;
                    double bottom = margin1.Bottom;
                    double num36 = top + bottom;
                    num32 = num35 + num36;
                  }
                }
                if (Math.Round(a2) <= Math.Round(a1) + 1.0)
                {
                  width = (a1 - num32) / (double) (this.tileViewItems.Count - 1);
                  tileViewItem1.OnMinimizedWidth = new GridLength(width, GridUnitType.Pixel);
                }
                else
                {
                  gridLength1 = tileViewItem1.OnMinimizedWidth;
                  width = gridLength1.Value;
                }
              }
            }
            if (tileViewItem1 == this.SwappedfromMaximized && this.IsSwapped)
            {
              tileViewItem1.OnMinimizedWidth = new GridLength(width, GridUnitType.Pixel);
              this.IsSwapped = false;
              this.SwappedfromMaximized = (TileViewItem) null;
              this.SwappedfromMinimized = (TileViewItem) null;
            }
            if (num29 == this.tileViewItems.Count - 1)
            {
              double num37 = 0.0;
              gridLength1 = tileViewItem1.OnMinimizedWidth;
              if (gridLength1.Value == 0.0)
                num37 = a1 - num11;
              if (num37 > 0.0)
              {
                TileViewItem tileViewItem3 = tileViewItem1;
                double num38 = num37 - tileViewItem1.Margin.Right;
                margin1 = tileViewItem1.Margin;
                double left2 = margin1.Left;
                GridLength gridLength2 = new GridLength(num38 - left2, GridUnitType.Pixel);
                tileViewItem3.OnMinimizedWidth = gridLength2;
              }
              gridLength1 = tileViewItem1.OnMinimizedWidth;
              width = gridLength1.Value;
              num11 = 0.0;
            }
            if (this.IsVirtualizing)
            {
              double minimizedVirtualItemSize = this.MinimizedVirtualItemSize;
              margin1 = tileViewItem1.Margin;
              double right2 = margin1.Right;
              double num39 = minimizedVirtualItemSize - right2;
              margin1 = tileViewItem1.Margin;
              double left3 = margin1.Left;
              width = num39 - left3;
            }
            double num40 = num11;
            double num41 = width;
            margin1 = tileViewItem1.Margin;
            double right3 = margin1.Right;
            double num42 = num41 + right3;
            margin1 = tileViewItem1.Margin;
            double left4 = margin1.Left;
            double num43 = num42 + left4;
            num11 = num40 + num43;
          }
          else
          {
            double minimizedColumnWidth = this.minimizedColumnWidth;
            margin1 = tileViewItem1.Margin;
            double right4 = margin1.Right;
            double num44 = minimizedColumnWidth - right4;
            margin1 = tileViewItem1.Margin;
            double left5 = margin1.Left;
            width = num44 - left5;
            gridLength1 = tileViewItem1.OnMinimizedHeight;
            if (gridLength1.GridUnitType == GridUnitType.Star)
            {
              height = (num30 - num9) / (double) (this.tileViewItems.Count - 1);
              tileViewItem1.OnMinimizedHeight = new GridLength(height, GridUnitType.Pixel);
            }
            else
            {
              gridLength1 = tileViewItem1.OnMinimizedHeight;
              if (gridLength1.GridUnitType == GridUnitType.Auto)
              {
                height = (num30 - num9) / (double) (this.tileViewItems.Count - 1);
                tileViewItem1.OnMinimizedHeight = new GridLength(height, GridUnitType.Pixel);
              }
              else
              {
                double a3 = 0.0;
                double num45 = 0.0;
                if (!this.IsVirtualizing)
                {
                  foreach (TileViewItem tileViewItem4 in (Collection<TileViewItem>) this.tileViewItems)
                  {
                    if (tileViewItem4.TileViewItemState != TileViewItemState.Maximized)
                    {
                      double num46 = a3;
                      gridLength1 = tileViewItem4.OnMinimizedHeight;
                      double num47 = gridLength1.Value;
                      a3 = num46 + num47;
                      double num48 = num45;
                      margin1 = tileViewItem1.Margin;
                      double left6 = margin1.Left;
                      margin1 = tileViewItem1.Margin;
                      double right5 = margin1.Right;
                      double num49 = left6 + right5;
                      num45 = num48 + num49;
                    }
                  }
                }
                if (!this.IsVirtualizing && Math.Round(a3) <= Math.Round(a1) + 1.0)
                {
                  height = (num30 - num45) / (double) (this.tileViewItems.Count - 1);
                  tileViewItem1.OnMinimizedHeight = new GridLength(height, GridUnitType.Pixel);
                }
                else
                {
                  gridLength1 = tileViewItem1.OnMinimizedHeight;
                  height = gridLength1.Value;
                }
              }
            }
            if (tileViewItem1 == this.SwappedfromMaximized && this.IsSwapped)
            {
              tileViewItem1.OnMinimizedHeight = new GridLength(height, GridUnitType.Pixel);
              this.IsSwapped = false;
              this.SwappedfromMaximized = (TileViewItem) null;
              this.SwappedfromMinimized = (TileViewItem) null;
            }
            if (num29 == this.tileViewItems.Count - 1)
            {
              double num50 = num30 - num11;
              if (num50 > 0.0)
              {
                TileViewItem tileViewItem5 = tileViewItem1;
                double num51 = num50;
                margin1 = tileViewItem1.Margin;
                double top = margin1.Top;
                double num52 = num51 - top;
                margin1 = tileViewItem1.Margin;
                double bottom = margin1.Bottom;
                GridLength gridLength3 = new GridLength(num52 - bottom, GridUnitType.Pixel);
                tileViewItem5.OnMinimizedHeight = gridLength3;
              }
              gridLength1 = tileViewItem1.OnMinimizedHeight;
              height = gridLength1.Value;
              num11 = 0.0;
            }
            if (this.IsVirtualizing)
            {
              double minimizedVirtualItemSize = this.MinimizedVirtualItemSize;
              margin1 = tileViewItem1.Margin;
              double top = margin1.Top;
              double num53 = minimizedVirtualItemSize - top;
              margin1 = tileViewItem1.Margin;
              double bottom = margin1.Bottom;
              height = num53 - bottom;
            }
            double num54 = num11;
            double num55 = height;
            margin1 = tileViewItem1.Margin;
            double top1 = margin1.Top;
            double num56 = num55 + top1;
            margin1 = tileViewItem1.Margin;
            double bottom1 = margin1.Bottom;
            double num57 = num56 + bottom1;
            num11 = num54 + num57;
          }
          if (height < 0.0)
            height = 0.0;
          if (width < 0.0)
            width = 0.0;
          tileViewItem1.AnimateSize(width, height);
        }
        else
        {
          double width;
          double height;
          if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
          {
            double num58 = a1;
            margin1 = tileViewItem1.Margin;
            double right = margin1.Right;
            double num59 = num58 - right;
            margin1 = tileViewItem1.Margin;
            double left = margin1.Left;
            width = num59 - left;
            if (tileViewItem1.TileViewItemState == TileViewItemState.Maximized && this.tileViewItems.Count <= 1)
            {
              double num60 = num30;
              margin1 = tileViewItem1.Margin;
              double top = margin1.Top;
              double num61 = num60 - top;
              margin1 = tileViewItem1.Margin;
              double bottom = margin1.Bottom;
              height = num61 - bottom;
            }
            else
            {
              double num62 = num30 - this.minimizedRowHeight;
              margin1 = tileViewItem1.Margin;
              double top = margin1.Top;
              double num63 = num62 - top;
              margin1 = tileViewItem1.Margin;
              double bottom = margin1.Bottom;
              height = num63 - bottom;
            }
          }
          else
          {
            if (tileViewItem1.TileViewItemState == TileViewItemState.Maximized && this.tileViewItems.Count <= 1)
            {
              double num64 = a1;
              margin1 = tileViewItem1.Margin;
              double right = margin1.Right;
              double num65 = num64 - right;
              margin1 = tileViewItem1.Margin;
              double left = margin1.Left;
              width = num65 - left;
            }
            else
            {
              double num66 = a1 - this.minimizedColumnWidth;
              margin1 = tileViewItem1.Margin;
              double right = margin1.Right;
              double num67 = num66 - right;
              margin1 = tileViewItem1.Margin;
              double left = margin1.Left;
              width = num67 - left;
            }
            double num68 = num30;
            margin1 = tileViewItem1.Margin;
            double top = margin1.Top;
            double num69 = num68 - top;
            margin1 = tileViewItem1.Margin;
            double bottom = margin1.Bottom;
            height = num69 - bottom;
          }
          tileViewItem1.AnimateSize(width, height);
        }
      }
    }
  }

  internal void AnimateTileViewLayout()
  {
    if (!double.IsInfinity(this.ActualWidth) && !double.IsNaN(this.ActualWidth) && this.ActualWidth == 0.0)
      return;
    if (this.maximizedItem == null)
    {
      double num1 = this.ActualWidth - (this.RightMargin + this.LeftMargin);
      double num2 = this.ActualHeight - (this.TopMargin + this.BottomMargin);
      if (this.tileViewItems == null)
        return;
      foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      {
        if (tileViewItem != this.tileViewItem)
        {
          double num3 = 0.0;
          double num4 = 0.0;
          if (this.RowHeight.Value > 1.0 && this.Rows > 0)
            num3 = (double) this.Rows * this.RowHeight.Value;
          if (this.ColumnWidth.Value > 1.0 && this.Columns > 0)
            num4 = (double) this.Columns * this.ColumnWidth.Value;
          double y = this.RowHeight.Value <= 1.0 ? Convert.ToDouble((double) Grid.GetRow((UIElement) tileViewItem) * (num2 / Convert.ToDouble(this.Rows))) : Convert.ToDouble((double) Grid.GetRow((UIElement) tileViewItem) * (num3 / Convert.ToDouble(this.Rows)));
          double x = this.ColumnWidth.Value <= 1.0 ? Convert.ToDouble((double) Grid.GetColumn((UIElement) tileViewItem) * (num1 / Convert.ToDouble(this.Columns))) : Convert.ToDouble((double) Grid.GetColumn((UIElement) tileViewItem) * (num4 / Convert.ToDouble(this.Columns)));
          tileViewItem.AnimatePosition(x, y);
        }
      }
    }
    else
    {
      double actualWidth = this.ActualWidth;
      double actualHeight = this.ActualHeight;
      this.TileViewItemOrder = new Dictionary<int, TileViewItem>();
      if (this.tileViewItems != null)
      {
        foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
        {
          if (this.tileViewItems.Count <= 1)
          {
            if (tileViewItem.minMaxButton != null)
            {
              tileViewItem.minMaxButton.IsEnabled = false;
              this.SetState(tileViewItem, TileViewItemState.Maximized);
            }
          }
          else if (tileViewItem.minMaxButton != null)
            tileViewItem.minMaxButton.IsEnabled = true;
          int key = Grid.GetRow((UIElement) tileViewItem) * this.Columns + Grid.GetColumn((UIElement) tileViewItem);
          if (!this.TileViewItemOrder.ContainsKey(key))
            this.TileViewItemOrder.Add(key, tileViewItem);
        }
      }
      double num5 = 0.0;
      int num6 = 0;
      double num7 = this.ActualWidth - (this.RightMargin + this.LeftMargin);
      double num8 = this.ActualHeight - (this.TopMargin + this.BottomMargin);
      for (int key = 0; key < this.TileViewItemOrder.Count; ++key)
      {
        if (this.TileViewItemOrder.ContainsKey(key))
        {
          if (this.TileViewItemOrder[key] != this.maximizedItem)
          {
            ++num6;
            double num9 = 0.0;
            double num10 = num5;
            if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
            {
              num9 = 0.0;
              num10 = num5;
            }
            else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top)
            {
              num9 = num5;
              num10 = 0.0;
            }
            else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
            {
              num9 = num7 - this.minimizedColumnWidth;
              num10 = num5;
            }
            else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
            {
              num9 = num5;
              num10 = num8 - this.minimizedRowHeight;
            }
            if (this.IsVirtualizing && this.itemsPanel is TileViewVirtualizingPanel && this.TileVirtualizingPanel.isTilesRestore && this.MinimizedScrollStep != 0.0 && this.maximizedItem != null)
            {
              if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
                num10 -= this.MinimizedScrollStep;
              else
                num9 -= this.MinimizedScrollStep;
              if (this.TileViewItemOrder[key] == this.PreviousMaximizedElement)
                this.TileViewItemOrder[key].AnimatePosition(num9, num10);
              else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
              {
                Canvas.SetTop((UIElement) this.TileViewItemOrder[key], Math.Round(num10));
                this.TileViewItemOrder[key].animationYKeyFramePosition.Value = Canvas.GetTop((UIElement) this.TileViewItemOrder[key]);
              }
              else
              {
                Canvas.SetLeft((UIElement) this.TileViewItemOrder[key], Math.Round(num9));
                this.TileViewItemOrder[key].animationXKeyFramePosition.Value = Canvas.GetLeft((UIElement) this.TileViewItemOrder[key]);
              }
            }
            else
              this.TileViewItemOrder[key].AnimatePosition(num9, num10);
            if (Application.Current != null && Application.Current.MainWindow != null)
            {
              if (this.IsSplitterUsedinMinimizedState)
              {
                if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
                  num5 += this.TileViewItemOrder[key].OnMinimizedHeight.Value + this.TileViewItemOrder[key].Margin.Top + this.TileViewItemOrder[key].Margin.Bottom;
                else
                  num5 += this.TileViewItemOrder[key].OnMinimizedWidth.Value + this.TileViewItemOrder[key].Margin.Right + this.TileViewItemOrder[key].Margin.Left;
              }
              else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
              {
                if (this.IsVirtualizing)
                  num5 += this.MinimizedVirtualItemSize;
                else
                  num5 += this.TileViewItemOrder[key].OnMinimizedHeight.Value + this.TileViewItemOrder[key].Margin.Top + this.TileViewItemOrder[key].Margin.Bottom;
              }
              else if (this.IsVirtualizing)
                num5 += this.MinimizedVirtualItemSize;
              else
                num5 += this.TileViewItemOrder[key].OnMinimizedWidth.Value + this.TileViewItemOrder[key].Margin.Left + this.TileViewItemOrder[key].Margin.Right;
            }
            else if (this.IsSplitterUsedinMinimizedState)
            {
              if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
                num5 += num8 / Convert.ToDouble(this.tileViewItems.Count - 1) + this.TileViewItemOrder[key].Margin.Top + this.TileViewItemOrder[key].Margin.Bottom;
              else
                num5 += num7 / Convert.ToDouble(this.tileViewItems.Count - 1) + this.TileViewItemOrder[key].Margin.Right + this.TileViewItemOrder[key].Margin.Left;
            }
            else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
              num5 += num8 / Convert.ToDouble(this.tileViewItems.Count - 1) + this.TileViewItemOrder[key].Margin.Top + this.TileViewItemOrder[key].Margin.Bottom;
            else
              num5 += num7 / Convert.ToDouble(this.tileViewItems.Count - 1) + this.TileViewItemOrder[key].Margin.Left + this.TileViewItemOrder[key].Margin.Right;
            if (key == this.TileViewItemOrder.Count - 1 || num6 == this.TileViewItemOrder.Count - 1)
              this.TileViewItemOrder[key].split.Visibility = Visibility.Collapsed;
          }
          else
          {
            double x = 0.0;
            double y = 0.0;
            if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
            {
              if (this.scroll != null)
              {
                if (this.scroll.VerticalOffset > 0.0 && !this.IsVirtualizing)
                {
                  x = this.minimizedColumnWidth;
                  y = this.scroll.VerticalOffset;
                }
                else
                {
                  x = this.TileViewItemOrder.Count > 1 ? this.minimizedColumnWidth : 0.0;
                  y = 0.0;
                }
              }
              else
              {
                x = this.minimizedColumnWidth;
                y = 0.0;
              }
            }
            else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top)
            {
              if (this.scroll != null)
              {
                if (this.scroll.HorizontalOffset > 0.0 && !this.IsVirtualizing)
                {
                  x = this.scroll.HorizontalOffset;
                  y = this.minimizedRowHeight;
                }
                else
                {
                  x = 0.0;
                  y = this.TileViewItemOrder.Count > 1 ? this.minimizedRowHeight : 0.0;
                }
              }
              else
              {
                x = 0.0;
                y = this.minimizedRowHeight;
              }
            }
            if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
            {
              if (this.scroll != null)
              {
                if (this.scroll.VerticalOffset > 0.0 && !this.IsVirtualizing)
                {
                  x = 0.0;
                  y = this.scroll.VerticalOffset;
                }
                else
                {
                  x = 0.0;
                  y = 0.0;
                }
              }
              else
              {
                x = 0.0;
                y = 0.0;
              }
            }
            else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
            {
              if (this.scroll != null)
              {
                if (this.scroll.HorizontalOffset > 0.0 && !this.IsVirtualizing)
                {
                  x = this.scroll.HorizontalOffset;
                  y = 0.0;
                }
                else
                {
                  x = 0.0;
                  y = 0.0;
                }
              }
              else
              {
                x = 0.0;
                y = 0.0;
              }
            }
            this.TileViewItemOrder[key].AnimatePosition(x, y);
          }
        }
      }
    }
  }

  internal void ItemAnimationCompleted(TileViewItem item)
  {
    if (this.TileViewItemOrder != null && this.TileViewItemOrder.Count < 1)
    {
      if (item.minMaxButton != null)
      {
        item.minMaxButton.IsEnabled = false;
        this.SetState(item, TileViewItemState.Maximized);
      }
    }
    else if (item.minMaxButton != null)
      item.minMaxButton.IsEnabled = true;
    if (item != null && item.TileViewItemState == TileViewItemState.Maximized)
    {
      item.HeaderCursor = Cursors.Arrow;
      this.OnMaximized(new TileViewEventArgs()
      {
        Source = (object) item
      });
    }
    if (item != null && item.TileViewItemState == TileViewItemState.Normal)
    {
      item.HeaderCursor = Cursors.Hand;
      this.OnRestored(new TileViewEventArgs()
      {
        Source = (object) item
      });
    }
    if (item == null || item.TileViewItemState != TileViewItemState.Minimized)
      return;
    item.HeaderCursor = Cursors.Arrow;
    this.OnMinimized(new TileViewEventArgs()
    {
      Source = (object) item
    });
  }

  internal void ChangeDataTemplate(TileViewItem tileviewitem)
  {
    if (tileviewitem.IsOverrideItemTemplate)
      return;
    if (this.ItemTemplateSelector != null)
      tileviewitem.ContentTemplateSelector = this.ItemTemplateSelector;
    if (this.tempContentTemplate == null)
      this.tempContentTemplate = tileviewitem.ItemTemplate;
    switch (tileviewitem.TileViewItemState)
    {
      case TileViewItemState.Normal:
        if (this.tempContentTemplate != null && this.ItemTemplate == null)
        {
          tileviewitem.ItemTemplate = this.tempContentTemplate;
          break;
        }
        tileviewitem.ItemTemplate = this.ItemTemplate;
        break;
      case TileViewItemState.Maximized:
        if (this.MaximizedItemTemplate != null)
        {
          tileviewitem.ItemTemplate = this.MaximizedItemTemplate;
          break;
        }
        if (tileviewitem.MaximizedItemTemplate != null)
        {
          tileviewitem.ItemTemplate = this.MaximizedItemTemplate;
          break;
        }
        tileviewitem.ItemContentTemplate = this.ItemTemplate;
        break;
      case TileViewItemState.Minimized:
        if (this.MinimizedItemTemplate != null)
        {
          tileviewitem.ItemTemplate = this.MinimizedItemTemplate;
          break;
        }
        if (tileviewitem.MinimizedItemTemplate != null)
        {
          tileviewitem.ItemTemplate = this.MinimizedItemTemplate;
          break;
        }
        tileviewitem.ItemContentTemplate = this.ItemTemplate;
        break;
    }
  }

  private static void OnItemContainerStyleChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    TileViewControl tileViewControl = sender as TileViewControl;
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is TileViewItem;

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new TileViewItem();
  }

  protected override void PrepareContainerForItemOverride(DependencyObject Dobj, object obj)
  {
    bool flag = true;
    if (this.ItemsSource != null && this.ItemsSource is IList itemsSource && this.tileViewItems != null)
    {
      if (this.tileViewItems.Count > 0 && itemsSource.Count > 0 && !this.IsVirtualizing)
      {
        for (int index1 = 0; index1 < this.tileViewItems.Count; ++index1)
        {
          for (int index2 = 0; index2 < itemsSource.Count; ++index2)
          {
            TileViewItem tileViewItem = this.ItemContainerGenerator.ContainerFromItem(itemsSource[index2]) as TileViewItem;
            if (this.tileViewItems[index1] == tileViewItem)
              flag = false;
          }
        }
      }
      if (flag)
      {
        if (this.tileViewItems.Count == itemsSource.Count)
          this.initialload = false;
        this.tileViewItems.Clear();
        this.listClear();
      }
    }
    if (Dobj is TileViewItem tileViewItem1)
    {
      tileViewItem1.ParentTileViewControl = this;
      ++this.count;
      if (this.tileViewItems != null)
      {
        if (!this.tileViewItems.Contains(tileViewItem1))
          this.tileViewItems.Add(tileViewItem1);
        if (!this.IsVirtualizing)
        {
          foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
            tileViewItem?.AnimationChanged();
        }
        else
          tileViewItem1.AnimationChanged();
      }
      this.StartTileViewItemDragEvents(tileViewItem1);
      if (this.isTileItemsLoaded && !this.IsVirtualizing)
      {
        Dictionary<int, TileViewItem> tileViewItemOrder = this.GetTileViewItemOrder();
        tileViewItemOrder.Add(this.tileViewItems.Count, tileViewItem1);
        this.UpdateTileViewLayout();
        this.SetRowsAndColumns(tileViewItemOrder);
        if (this.initialload)
          this.UpdateTileViewLayout(true);
        else if (this.tileViewItems != null)
        {
          double length1 = 0.0;
          if (this.sortedcanvasleft.Count >= this.tileViewItems.Count)
            length1 = Convert.ToDouble(this.sortedcanvasleft[this.tileViewItems.Count - 1]);
          double length2 = Convert.ToDouble((double) Grid.GetRow((UIElement) tileViewItem1) * (this.ActualHeight / Convert.ToDouble(this.Rows)));
          Canvas.SetLeft((UIElement) tileViewItem1, length1);
          Canvas.SetTop((UIElement) tileViewItem1, length2);
        }
      }
      if (this.IsVirtualizing && this.initialload && this.isTileItemsLoaded && !this.IsScroll && this.itemsPanel is TileViewVirtualizingPanel && this.TileVirtualizingPanel.LastVisibleIndex - this.TileVirtualizingPanel.FirstVisibleIndex < this.Rows * this.Columns)
        this.IsInsertOrRemoveItem = true;
      tileViewItem1.Onloadingitems();
      this.ControlActualHeight = this.ActualHeight;
      this.ControlActualWidth = this.ActualWidth;
      this.ItemsHeaderHeight.Add((object) tileViewItem1.HeaderHeight);
      if (this.Items.Count == 0)
        this.itemsPanel = (Panel) null;
      else if (!this.IsVirtualizing)
      {
        this.itemsPanel = VisualTreeHelper.GetParent(this.ItemContainerGenerator.ContainerFromIndex(0)) as Panel;
      }
      else
      {
        if (this.firstVisibleIndex < 0)
          this.firstVisibleIndex = 0;
        this.itemsPanel = VisualTreeHelper.GetParent(this.ItemContainerGenerator.ContainerFromIndex(this.firstVisibleIndex)) as Panel;
        this.TileVirtualizingPanel = this.itemsPanel as TileViewVirtualizingPanel;
      }
      if (this.scroll != null)
        this.scroll.ScrollChanged -= new ScrollChangedEventHandler(this.scroll_ScrollChanged);
      this.scroll = this.GetTemplateChild("scrollviewer") as ScrollViewer;
      this.MainGrid = this.GetTemplateChild("MainGrid") as Grid;
    }
    this.GetScrollViewer();
    this.GetCanvasHeight();
    if (this.ItemTemplate != null && !tileViewItem1.IsOverrideItemTemplate)
      tileViewItem1.ItemTemplate = this.ItemTemplate;
    base.PrepareContainerForItemOverride(Dobj, obj);
  }

  internal void GetScrollViewer()
  {
    if (this.scroll == null)
      return;
    this.scroll.ScrollChanged += new ScrollChangedEventHandler(this.scroll_ScrollChanged);
  }

  internal void scroll_ScrollChanged(object obj, ScrollChangedEventArgs args)
  {
    if (this.IsVirtualizing)
      return;
    if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right && this.tileViewItems != null)
    {
      foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      {
        if (tileViewItem.TileViewItemState == TileViewItemState.Maximized && !this.IsVirtualizing)
        {
          Canvas.SetTop((UIElement) tileViewItem, this.scroll.VerticalOffset);
          this.scroll.UpdateLayout();
          this.scroll.ScrollToVerticalOffset(this.scroll.VerticalOffset);
        }
        if (tileViewItem.TileViewItemState == TileViewItemState.Minimized)
          this.scroll.UpdateLayout();
      }
    }
    else
    {
      if (this.MinimizedItemsOrientation != MinimizedItemsOrientation.Top && (this.MinimizedItemsOrientation != MinimizedItemsOrientation.Bottom || this.tileViewItems == null))
        return;
      foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      {
        if (tileViewItem.TileViewItemState == TileViewItemState.Maximized)
        {
          Canvas.SetLeft((UIElement) tileViewItem, this.scroll.HorizontalOffset);
          this.scroll.UpdateLayout();
          this.scroll.ScrollToHorizontalOffset(this.scroll.HorizontalOffset);
        }
        if (tileViewItem.TileViewItemState == TileViewItemState.Minimized)
          this.scroll.UpdateLayout();
      }
    }
  }

  internal void GetCanvasHeight()
  {
    bool flag = false;
    if (this.tileViewItems != null)
    {
      foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      {
        if (tileViewItem.TileViewItemState == TileViewItemState.Maximized)
          flag = true;
      }
    }
    if (flag)
    {
      ScrollBar scrollBar1 = (ScrollBar) null;
      ScrollBar scrollBar2 = (ScrollBar) null;
      if (this.scroll != null && this.scroll.Template != null && !this.IsVirtualizing)
      {
        this.UpdateTileViewLayout();
        scrollBar1 = this.scroll.Template.FindName("PART_VerticalScrollBar", (FrameworkElement) this.scroll) as ScrollBar;
        scrollBar2 = this.scroll.Template.FindName("PART_HorizontalScrollBar", (FrameworkElement) this.scroll) as ScrollBar;
        if (!DesignerProperties.GetIsInDesignMode((DependencyObject) this) && !this.IsVirtualizing)
          this.scroll.ScrollToVerticalOffset(this.scroll.VerticalOffset);
      }
      double num1 = this.ActualWidth - (scrollBar1 != null ? scrollBar1.ActualWidth : 0.0);
      double num2 = this.ActualHeight - (scrollBar2 != null ? scrollBar2.ActualHeight : 0.0);
      if (this.scroll != null)
      {
        int num3 = this.IsVirtualizing ? 1 : 0;
      }
      if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
      {
        this.canvasheightonMinimized = 0.0;
        if (this.tileViewItems != null)
        {
          foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
          {
            if (tileViewItem.TileViewItemState == TileViewItemState.Normal && this.ActualHeight > 0.0 && this.itemsPanel != null)
            {
              this.itemsPanel.Height = num2;
              this.itemsPanel.Width = num1;
            }
            if (tileViewItem.TileViewItemState == TileViewItemState.Minimized)
              this.canvasheightonMinimized += tileViewItem.OnMinimizedHeight.Value + tileViewItem.Margin.Top + tileViewItem.Margin.Bottom;
          }
          if (this.tileViewItems.Count == 1 && this.tileViewItems[0].TileViewItemState == TileViewItemState.Maximized && this.ActualHeight > 0.0 && this.ActualWidth > 0.0 && this.itemsPanel != null)
          {
            this.itemsPanel.Height = num2;
            this.itemsPanel.Width = num1;
          }
        }
        if (this.canvasheightonMinimized > 0.0 && num2 > 0.0)
        {
          if (num2 < this.canvasheightonMinimized)
          {
            if (this.itemsPanel != null)
            {
              this.itemsPanel.Height = this.canvasheightonMinimized;
              this.itemsPanel.Width = num1;
            }
          }
          else if (this.itemsPanel != null)
          {
            this.itemsPanel.Height = num2;
            this.itemsPanel.Width = num1;
          }
        }
      }
      if (this.MinimizedItemsOrientation != MinimizedItemsOrientation.Top && this.MinimizedItemsOrientation != MinimizedItemsOrientation.Bottom)
        return;
      this.canvaswidthonMinimized = 0.0;
      if (this.tileViewItems != null)
      {
        foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
        {
          if (tileViewItem.TileViewItemState == TileViewItemState.Normal && this.ActualWidth > 0.0 && this.itemsPanel != null)
          {
            this.itemsPanel.Height = num2;
            this.itemsPanel.Width = num1;
          }
          if (tileViewItem.TileViewItemState == TileViewItemState.Minimized)
            this.canvaswidthonMinimized += tileViewItem.OnMinimizedWidth.Value + tileViewItem.Margin.Left + tileViewItem.Margin.Right;
        }
      }
      if (this.canvaswidthonMinimized <= 0.0 || num1 <= 0.0)
        return;
      if (num1 < this.canvaswidthonMinimized)
      {
        if (this.itemsPanel == null)
          return;
        this.itemsPanel.Width = this.canvaswidthonMinimized;
        this.itemsPanel.Height = num2;
      }
      else
      {
        if (this.itemsPanel == null)
          return;
        this.itemsPanel.Height = num2;
        this.itemsPanel.Width = num1;
      }
    }
    else
    {
      double num4 = this.ActualWidth - (this.RightMargin + this.LeftMargin);
      double num5 = this.ActualHeight - (this.TopMargin + this.BottomMargin);
      if (this.itemsPanel == null || num4 <= 0.0 || num5 <= 0.0 || this.itemsPanel.Children.Count <= 1)
        return;
      if (this.RowHeight.Value > 1.0 && (double) this.Rows * this.RowHeight.Value > this.ActualHeight)
        this.itemsPanel.Height = (double) this.Rows * this.RowHeight.Value;
      else
        this.itemsPanel.Height = this.ActualHeight;
      if (this.ColumnWidth.Value > 1.0 && (double) this.Columns * this.ColumnWidth.Value > this.ActualWidth)
        this.itemsPanel.Width = (double) this.Columns * this.ColumnWidth.Value;
      else
        this.itemsPanel.Width = this.ActualWidth;
    }
  }

  protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
  {
    base.OnItemsChanged(e);
    if (this.Items.Count == 0 && this.orderedtileviewitems != null)
      this.orderedtileviewitems.Clear();
    if (this.tempitemcount == 0)
    {
      this.tempitemcount = this.tileViewItems.Count;
      foreach (UIElement tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      {
        if (tileViewItem.Visibility == Visibility.Collapsed)
          --this.tempitemcount;
      }
    }
    if (e.Action == NotifyCollectionChangedAction.Add && !this.IsVirtualizing)
    {
      if (this.ItemsSource == null)
      {
        Dictionary<int, TileViewItem> tileViewItemOrder = this.GetTileViewItemOrder();
        ObservableCollection<TileViewItem> tileViewItems = this.tileViewItems;
        if (this.tileViewItems != null && this.tempitemcount - e.NewStartingIndex > 1)
        {
          TileViewItem tileViewItem1 = this.tileViewItems[this.tempitemcount - 1];
          tileViewItemOrder.Clear();
          this.tileViewItems.Clear();
          int key = 0;
          foreach (TileViewItem tileViewItem2 in (Collection<TileViewItem>) tileViewItems)
          {
            if (key == e.NewStartingIndex)
            {
              tileViewItemOrder.Add(e.NewStartingIndex, tileViewItem2);
              this.tileViewItems.Add(tileViewItem2);
            }
            else if (key != e.NewStartingIndex)
            {
              tileViewItemOrder.Add(key, tileViewItem2);
              this.tileViewItems.Add(tileViewItem2);
            }
            ++key;
          }
        }
        this.SetRowsAndColumns(tileViewItemOrder);
      }
      this.UpdateTileViewLayout(true);
    }
    if (e.Action == NotifyCollectionChangedAction.Remove)
    {
      ObservableCollection<TileViewItem> observableCollection1 = new ObservableCollection<TileViewItem>();
      ObservableCollection<TileViewItem> observableCollection2 = new ObservableCollection<TileViewItem>();
      ObservableCollection<TileViewItem> tileViewItems = this.tileViewItems;
      foreach (object obj in (IEnumerable) this.Items)
      {
        TileViewItem tileViewItem = this.ItemContainerGenerator.ContainerFromItem(obj) as TileViewItem;
        observableCollection1.Add(tileViewItem);
      }
      if (this.tileViewItems != null && this.tileViewItems.Count > 0)
      {
        this.tileViewItems.Remove(e.OldItems[0] as TileViewItem);
        this.initialTileViewItems.Remove(e.OldItems[0] as TileViewItem);
      }
      if (this.orderedtileviewitems != null && this.orderedtileviewitems.Count > 0)
        this.orderedtileviewitems.Remove(e.OldItems[0] as TileViewItem);
      if (!(e.OldItems[0] is TileViewItem))
      {
        for (int index = tileViewItems.Count - 1; index >= 0; --index)
        {
          TileViewItem tileViewItem = tileViewItems[index];
          if (!observableCollection1.Contains(tileViewItem))
          {
            this.tileViewItems.Remove(tileViewItem);
            this.initialTileViewItems.Remove(tileViewItem);
          }
        }
        foreach (object obj in (IEnumerable) this.Items)
        {
          if (this.ItemContainerGenerator.ContainerFromItem(obj) is TileViewItem tileViewItem && tileViewItem.TileViewItemState == TileViewItemState.Minimized)
            ++this.iCount;
        }
        if (this.iCount == this.Items.Count)
        {
          foreach (object obj in (IEnumerable) this.Items)
          {
            if (this.ItemContainerGenerator.ContainerFromItem(obj) is TileViewItem tileViewItem && tileViewItem.TileViewItemState == TileViewItemState.Minimized)
              tileViewItem.TileViewItemState = TileViewItemState.Normal;
          }
        }
        else
        {
          this.UpdateTileViewLayout();
          this.iCount = 0;
        }
      }
      if (this.IsVirtualizing && this.itemsPanel is TileViewVirtualizingPanel && !this.IsDragging)
      {
        this.RemoveAtVirtualPostion = e.OldStartingIndex;
        this.TileVirtualizingPanel.isTilesRestore = false;
        this.IsInsertOrRemoveItem = true;
      }
      else
      {
        this.UpdateCurrentOrderFromTileItems();
        this.SetRowsAndColumns(this.GetTileViewItemOrder());
        this.UpdateTileViewLayout(true);
      }
      if (e.OldItems[0] is TileViewItem && e.OldItems[0] is TileViewItem)
        (e.OldItems[0] as TileViewItem).Dispose();
    }
    if (e.Action != NotifyCollectionChangedAction.Reset)
      return;
    if (this.tileViewItems != null && this.tileViewItems.Count != this.Items.Count && this.tileViewItems.Count > 0)
    {
      this.needToUpdate = true;
      this.tileViewItems.Clear();
      if (this.orderedtileviewitems == null)
        return;
      this.orderedtileviewitems.Clear();
    }
    else
      this.needToUpdate = false;
  }

  private List<int> ValidateCurrentItemsOrder(List<int> newValue, List<int> currentItemsOrder)
  {
    if (currentItemsOrder == null)
      return newValue;
    if (newValue == null || this.IsVirtualizing)
      return currentItemsOrder;
    List<int> list1 = newValue.Where<int>((Func<int, bool>) (t => t > this.tileViewItems.Count - 1)).ToList<int>();
    List<int> list2 = newValue.Where<int>((Func<int, bool>) (t => t < 0)).ToList<int>();
    return newValue.Select<int, int>((Func<int, int>) (i => i)).Distinct<int>().ToList<int>().Count == this.tileViewItems.Count && list1.Count == 0 && list2.Count == 0 ? newValue : currentItemsOrder;
  }

  internal void UpdateCurrentOrderFromTileItems()
  {
    if (this.initialTileViewItems.Count != this.tileViewItems.Count)
      return;
    this.currentposition.Clear();
    for (int index1 = 0; index1 < this.initialTileViewItems.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.initialTileViewItems.Count; ++index2)
      {
        if (this.orderedtileviewitems.Count > 0 && this.initialTileViewItems[index2] == this.tileViewItems[index1])
          this.currentposition.Add(index2);
      }
    }
    this.CurrentItemsOrder = this.currentposition;
  }

  internal Dictionary<int, TileViewItem> GetTileItemsOrderFromCurrentOrder()
  {
    ObservableCollection<TileViewItem> observableCollection = new ObservableCollection<TileViewItem>();
    Dictionary<int, TileViewItem> fromCurrentOrder = new Dictionary<int, TileViewItem>();
    for (int index1 = 0; index1 < this.CurrentItemsOrder.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.tileViewItems.Count; ++index2)
      {
        if (this.CurrentItemsOrder[index1] == this.tileViewItems.IndexOf(this.tileViewItems[index2]) && this.tileViewItems.Count > index2)
        {
          observableCollection.Add(this.tileViewItems[index2]);
          fromCurrentOrder.Add(index1, this.tileViewItems[index2]);
        }
      }
    }
    this.tileViewItems = observableCollection;
    return fromCurrentOrder;
  }

  private void ApplyContainerStyle(TileViewItem item, object obj)
  {
    item.SetBinding(FrameworkElement.StyleProperty, (BindingBase) new Binding("ItemContainerStyle")
    {
      Source = (object) this
    });
  }

  private void ApplyHeaderTemplate(TileViewItem item, object obj)
  {
    if (item.Header != null)
      return;
    item.SetValue(HeaderedContentControl.HeaderProperty, obj);
    if (this.HeaderTemplate == null)
    {
      item.Header = (object) "";
    }
    else
    {
      if (item.Header != null)
        return;
      item.Header = (object) "";
    }
  }

  internal void ChangeHeaderContent(TileViewItem item)
  {
    if (item.HeaderTemplate != null || item.HeaderContent == null)
      return;
    if (item.TileViewItemState == TileViewItemState.Maximized)
    {
      if (item.MaximizedHeader != null)
        item.HeaderContent.Content = item.MaximizedHeader;
      else
        item.HeaderContent.Content = item.Header;
    }
    else if (item.TileViewItemState == TileViewItemState.Minimized)
    {
      if (item.MinimizedHeader != null)
        item.HeaderContent.Content = item.MinimizedHeader;
      else
        item.HeaderContent.Content = item.Header;
    }
    else
      item.HeaderContent.Content = item.Header;
  }

  internal void ChangeHeaderTemplate(TileViewItem item)
  {
    switch (item.TileViewItemState)
    {
      case TileViewItemState.Normal:
        if (this.HeaderTemplate != null)
        {
          item.HeaderTemplate = this.HeaderTemplate;
          break;
        }
        if (item.Header == null)
          break;
        item.Header = item.Header;
        break;
      case TileViewItemState.Maximized:
        if (this.MaximizedHeaderTemplate != null)
        {
          item.HeaderTemplate = this.MaximizedHeaderTemplate;
          break;
        }
        item.HeaderTemplate = this.HeaderTemplate;
        break;
      case TileViewItemState.Minimized:
        if (this.MinimizedHeaderTemplate != null)
        {
          item.HeaderTemplate = this.MinimizedHeaderTemplate;
          break;
        }
        if (this.MinimizedHeaderTemplate != null)
          break;
        item.HeaderTemplate = this.HeaderTemplate;
        break;
    }
  }

  private void ApplyContentTemplate(TileViewItem item, object obj)
  {
    Style itemContainerStyle = this.ItemContainerStyle;
    if (itemContainerStyle != null && itemContainerStyle.Setters.Count > 0)
    {
      foreach (Setter setter in (Collection<SetterBase>) itemContainerStyle.Setters)
      {
        if (setter.Property == ContentControl.ContentProperty)
          item.ContentTemplate = setter.Value as DataTemplate;
      }
    }
    DataTemplate itemTemplate = this.ItemTemplate;
    if (itemTemplate == null)
      return;
    item.ItemContentTemplate = itemTemplate;
  }

  internal void ApplyTileViewContent(TileViewItem item)
  {
    if (item.ItemContentTemplate != null || item.TileViewContent == null)
      return;
    if (item.TileViewItemState == TileViewItemState.Maximized)
    {
      if (item.MaximizedItemContent != null)
        item.TileViewContent.Content = item.MaximizedItemContent;
      else
        item.TileViewContent.Content = item.Content;
    }
    else if (item.TileViewItemState == TileViewItemState.Minimized)
    {
      if (item.MinimizedItemContent != null)
        item.TileViewContent.Content = item.MinimizedItemContent;
      else
        item.TileViewContent.Content = item.Content;
    }
    else
      item.TileViewContent.Content = item.Content;
  }

  private static void OnSplitterThicknessChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnSplitterThicknessChanged(args);
  }

  protected virtual void OnSplitterThicknessChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.tileViewItems != null)
    {
      foreach (UIElement tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
        (tileViewItem as TileViewItem).LoadSplitter();
    }
    this.UpdateTileViewLayout(true);
    if (this.SplitterThicknessChanged == null)
      return;
    this.SplitterThicknessChanged((DependencyObject) this, args);
  }

  private static void OnMinimizedItemsPercentageChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnMinimizedItemsPercentageChanged(args);
  }

  protected virtual void OnMinimizedItemsPercentageChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.tileViewItems != null)
    {
      foreach (UIElement tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
        (tileViewItem as TileViewItem).LoadSplitter();
    }
    this.UpdateTileViewLayout(true);
    if (this.MinimizedItemsPercentageChanged == null)
      return;
    this.MinimizedItemsPercentageChanged((DependencyObject) this, args);
  }

  public static void OnUseNormalStatePropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnUseNormalStatePropertyChanged(args);
  }

  public static void OnScrollBarVisibilityPropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnScrollBarVisibilityPropertyChanged(args);
  }

  protected virtual void OnUseNormalStatePropertyChanged(DependencyPropertyChangedEventArgs args)
  {
    int index = 0;
    bool flag = true;
    if (!this.UseNormalState)
    {
      foreach (object obj in (IEnumerable) this.Items)
      {
        TileViewItem tileViewItem1;
        if (obj is TileViewItem)
        {
          tileViewItem1 = (TileViewItem) obj;
          do
          {
            tileViewItem2 = (TileViewItem) this.Items[index];
            if (tileViewItem2 != null && tileViewItem2.TileViewItemState == TileViewItemState.Hidden)
              ++index;
            else
              flag = false;
          }
          while (flag);
        }
        else
        {
          tileViewItem1 = this.ItemContainerGenerator.ContainerFromItem(obj) as TileViewItem;
          do
          {
            if (this.ItemContainerGenerator.ContainerFromItem(this.Items[0]) is TileViewItem tileViewItem2 && tileViewItem2.TileViewItemState == TileViewItemState.Hidden)
              ++index;
            else
              flag = false;
          }
          while (flag);
        }
        if (tileViewItem1 != null && tileViewItem1 == tileViewItem2)
        {
          this.SetState(tileViewItem1, TileViewItemState.Maximized);
          tileViewItem1.minMaxButton.Visibility = Visibility.Collapsed;
        }
        else if (tileViewItem1 != null && tileViewItem1.TileViewItemState != TileViewItemState.Hidden)
        {
          this.SetState(tileViewItem1, TileViewItemState.Minimized);
          tileViewItem1.minMaxButton.Visibility = Visibility.Visible;
        }
      }
    }
    else
    {
      foreach (object obj in (IEnumerable) this.Items)
      {
        TileViewItem tileViewItem = !(obj is TileViewItem) ? this.ItemContainerGenerator.ContainerFromItem(obj) as TileViewItem : (TileViewItem) obj;
        if (tileViewItem != null && tileViewItem.TileViewItemState != TileViewItemState.Hidden)
        {
          this.SetState(tileViewItem, TileViewItemState.Normal);
          tileViewItem.minMaxButton.Visibility = Visibility.Visible;
        }
      }
    }
    this.UpdateTileViewLayout();
  }

  protected virtual void OnScrollBarVisibilityPropertyChanged(
    DependencyPropertyChangedEventArgs args)
  {
    this.UpdateTileViewLayout();
  }

  private static void OnIsMinMaxButtonOnMouseOverOnlyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnIsMinMaxButtonOnMouseOverOnlyChanged(args);
  }

  private static void OnRowCountChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnRowCountChanged(args);
  }

  protected virtual void OnRowCountChanged(DependencyPropertyChangedEventArgs args)
  {
    if (args.OldValue != args.NewValue)
    {
      if (this.IsVirtualizing)
      {
        if (this.itemsPanel is TileViewVirtualizingPanel)
        {
          this.TileVirtualizingPanel.VirtualRow = Convert.ToInt32(args.NewValue);
          if (Convert.ToInt32(args.NewValue) == 0)
            this.TileVirtualizingPanel.VirtualRow = 10;
          this.IsScroll = true;
          this.TileVirtualizingPanel.InvalidateMeasure();
        }
      }
      else
      {
        this.SetRowsAndColumns(this.GetTileViewItemOrder());
        this.UpdateTileViewLayout(true);
      }
    }
    if (this.RowCountChanged == null)
      return;
    this.RowCountChanged((DependencyObject) this, args);
  }

  private static void OnColumnCountChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnColumnCountChanged(args);
  }

  protected virtual void OnColumnCountChanged(DependencyPropertyChangedEventArgs args)
  {
    if (args.OldValue != args.NewValue)
    {
      if (this.IsVirtualizing)
      {
        if (this.itemsPanel is TileViewVirtualizingPanel)
        {
          this.TileVirtualizingPanel.VirtualColumn = Convert.ToInt32(args.NewValue);
          if (Convert.ToInt32(args.NewValue) == 0)
            this.TileVirtualizingPanel.VirtualColumn = 12;
          this.TileVirtualizingPanel.isTilesRestore = true;
          this.TileVirtualizingPanel.InvalidateMeasure();
        }
      }
      else
      {
        this.SetRowsAndColumns(this.GetTileViewItemOrder());
        this.UpdateTileViewLayout(true);
      }
    }
    if (this.ColumnCountChanged == null)
      return;
    this.ColumnCountChanged((DependencyObject) this, args);
  }

  private static object CoerceColumnCount(DependencyObject d, object baseValue)
  {
    TileViewControl tileViewControl = (TileViewControl) d;
    if (tileViewControl.IsVirtualizing)
      return baseValue;
    int num = (int) baseValue * tileViewControl.RowCount;
    if (tileViewControl.RowCount == 0 || num >= tileViewControl.Items.Count)
      return baseValue;
    Decimal d1 = 0M;
    if (tileViewControl.ColumnCount != 0)
      d1 = (Decimal) tileViewControl.Items.Count / (Decimal) tileViewControl.RowCount;
    return (object) (int) Math.Ceiling(d1);
  }

  private static object CoerceRowCount(DependencyObject d, object baseValue) => baseValue;

  protected virtual void OnIsMinMaxButtonOnMouseOverOnlyChanged(
    DependencyPropertyChangedEventArgs args)
  {
    if (this.IsMinMaxButtonOnMouseOverOnlyChanged == null)
      return;
    this.IsMinMaxButtonOnMouseOverOnlyChanged((DependencyObject) this, args);
  }

  private static void OnMinimizedItemTemplateChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnMinimizedItemTemplateChanged(args);
  }

  private void OnMinimizedItemTemplateChanged(DependencyPropertyChangedEventArgs args)
  {
    foreach (UIElement tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      this.ChangeDataTemplate(tileViewItem as TileViewItem);
  }

  private static void OnMaximizedItemTemplateChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnMaximizedItemTemplateChanged(args);
  }

  private void OnMaximizedItemTemplateChanged(DependencyPropertyChangedEventArgs args)
  {
    foreach (UIElement tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      this.ChangeDataTemplate(tileViewItem as TileViewItem);
  }

  private static void OnMinimizedHeaderTemplateChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnMinimizedHeaderTemplateChanged(args);
  }

  private void OnMinimizedHeaderTemplateChanged(DependencyPropertyChangedEventArgs args)
  {
    foreach (UIElement tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      this.ChangeHeaderTemplate(tileViewItem as TileViewItem);
  }

  private static void OnMaximizedHeaderTemplateChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnMaximizedHeaderTemplateChanged(args);
  }

  private void OnMaximizedHeaderTemplateChanged(DependencyPropertyChangedEventArgs args)
  {
    foreach (UIElement tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      this.ChangeHeaderTemplate(tileViewItem as TileViewItem);
  }

  private static void OnHeaderTemplateChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnHeaderTemplateChanged(args);
  }

  private void OnHeaderTemplateChanged(DependencyPropertyChangedEventArgs args)
  {
    foreach (UIElement tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      this.ChangeHeaderTemplate(tileViewItem as TileViewItem);
  }

  private static void OnSplitterVisibilityChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnSplitterVisibilityChanged(args);
  }

  protected virtual void OnSplitterVisibilityChanged(DependencyPropertyChangedEventArgs args)
  {
    int num = 0;
    if ((Visibility) args.NewValue == Visibility.Visible)
    {
      this.SplitterVisibility = Visibility.Visible;
      foreach (UIElement tileViewItem1 in (Collection<TileViewItem>) this.tileViewItems)
      {
        TileViewItem tileViewItem2 = tileViewItem1 as TileViewItem;
        if (num == this.tileViewItems.Count - 1)
          return;
        tileViewItem2.LoadSplitter();
        ++num;
      }
    }
    else
    {
      this.SplitterVisibility = Visibility.Collapsed;
      if (this.tileViewItems != null)
      {
        foreach (UIElement tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
          (tileViewItem as TileViewItem).LoadSplitter();
      }
    }
    if (this.IsSplitterVisibilityChanged == null)
      return;
    this.IsSplitterVisibilityChanged((DependencyObject) this, args);
  }

  private static void OnMinimizedItemsOrientationChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnMinimizedItemsOrientationChanged(args);
  }

  protected virtual void OnMinimizedItemsOrientationChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.maximizedItem != null)
    {
      if (this.IsVirtualizing)
      {
        this.IsScroll = true;
        this.TileVirtualizingPanel.InvalidateMeasure();
      }
      else
      {
        this.UpdateTileViewLayout(true);
        this.GetCanvasHeight();
      }
      if (this.tileViewItems != null)
      {
        foreach (UIElement tileViewItem1 in (Collection<TileViewItem>) this.tileViewItems)
        {
          TileViewItem tileViewItem2 = tileViewItem1 as TileViewItem;
          if (this.count == this.tileViewItems.Count - 1)
            return;
          tileViewItem2.LoadSplitter();
          ++this.count;
        }
      }
    }
    if (this.MinimizedItemsOrientationChanged == null)
      return;
    this.MinimizedItemsOrientationChanged((DependencyObject) this, args);
  }

  private static void OnCurrentItemsOrderChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnCurrentItemsOrderChanged(args);
  }

  protected virtual void OnCurrentItemsOrderChanged(DependencyPropertyChangedEventArgs args)
  {
    if (args.OldValue == args.NewValue)
      return;
    List<int> first = new List<int>();
    List<int> second = new List<int>();
    if (args.NewValue != null)
      second = (List<int>) args.NewValue;
    if (args.OldValue != null)
      first = (List<int>) args.OldValue;
    if (this.IsVirtualizing || !this.AllowItemRepositioning || first.SequenceEqual<int>((IEnumerable<int>) second))
      return;
    this.SetRowsAndColumns(this.GetTileViewItemOrder());
    this.UpdateTileViewLayout(this.EnableAnimation);
  }

  private static void OnAllowItemRepositioningChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnAllowItemRepositioningChanged(args);
  }

  protected virtual void OnAllowItemRepositioningChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.tileViewItems != null)
    {
      foreach (UIElement tileViewItem1 in (Collection<TileViewItem>) this.tileViewItems)
      {
        TileViewItem tileViewItem2 = tileViewItem1 as TileViewItem;
        tileViewItem2.IsMovable = (bool) args.NewValue;
        if (tileViewItem2.TileViewItemState != TileViewItemState.Normal)
          tileViewItem2.IsMovable = false;
        else
          tileViewItem2.IsMovable = true;
      }
    }
    this.UpdateTileViewLayout(true);
    if (this.AllowItemRepositioningChanged == null)
      return;
    this.AllowItemRepositioningChanged((DependencyObject) this, args);
  }

  internal void repCard_Normal(object sender, EventArgs e)
  {
    if (this.IsVirtualizing && this.itemsPanel is TileViewVirtualizingPanel)
      this.TileVirtualizingPanel.isTilesRestore = true;
    this.MinimizedScrollStep = 0.0;
    TileViewCancelEventArgs e1 = new TileViewCancelEventArgs();
    e1.Source = (object) this.maximizedItem;
    this.OnRestoring(e1);
    if (e1.Cancel)
      return;
    this.maximizedItem = (TileViewItem) null;
    this.OnMaximizedItemChanged(new TileViewEventArgs()
    {
      Source = (object) null
    });
    if (this.tileViewItems != null)
    {
      foreach (UIElement tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      {
        TileViewItem tileviewitem = tileViewItem as TileViewItem;
        if (tileviewitem.minMaxButton.Visibility == Visibility.Collapsed && !this.IsMinMaxButtonOnMouseOverOnly)
          tileviewitem.minMaxButton.Visibility = Visibility.Visible;
        tileviewitem.minMaxButton.SetValue(ToggleButton.IsCheckedProperty, (object) false);
        tileviewitem.disablePropertyChangedNotify = true;
        this.SetState(tileviewitem, TileViewItemState.Normal);
        tileviewitem.disablePropertyChangedNotify = false;
        tileviewitem.IsMovable = true;
        tileviewitem.LoadSplitter();
        if (tileviewitem.minMaxButton != null)
          tileviewitem.minMaxButton.SetValue(ToggleButton.IsCheckedProperty, (object) null);
        this.ChangeDataTemplate(tileviewitem);
        this.ApplyTileViewContent(tileviewitem);
        this.ChangeHeaderTemplate(tileviewitem);
        this.ChangeHeaderContent(tileviewitem);
      }
    }
    this.UpdateTileViewLayout(true);
  }

  internal void repCard_Maximized(object sender, EventArgs e)
  {
    if (this.IsVirtualizing && this.itemsPanel is TileViewVirtualizingPanel)
    {
      if (this.maximizedItem == null)
      {
        this.TileVirtualizingPanel.IsMaximizedButtonClick = true;
      }
      else
      {
        this.PreviousMaximizedElement = this.maximizedItem;
        this.TileVirtualizingPanel.isTilesRestore = true;
      }
    }
    this.maximizedItem = sender as TileViewItem;
    TileViewCancelEventArgs e1 = new TileViewCancelEventArgs();
    if (this.maximizedItem != null)
    {
      e1.Source = (object) this.maximizedItem;
      this.OnMaximizing(e1);
      if (!e1.Cancel && this.maximizedItem.minMaxButton != null)
      {
        this.maximizedItem.minMaxButton.SetValue(ToggleButton.IsCheckedProperty, (object) true);
        this.maximizedItem.disablePropertyChangedNotify = true;
        this.SetState(this.maximizedItem, TileViewItemState.Maximized);
        this.maximizedItem.disablePropertyChangedNotify = false;
        this.MinimizeditemsOrder.Clear();
        if (this.tileViewItems != null)
        {
          if (this.tileViewItems.Count == 0 && this.IsVirtualizing)
            this.GetTileViewItemOrder();
          foreach (UIElement tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
          {
            TileViewItem tileviewitem = tileViewItem as TileViewItem;
            if (!this.AllowItemRepositioning)
              tileviewitem.IsMovable = false;
            if (tileviewitem != this.maximizedItem)
            {
              this.OnMinimizing(new TileViewEventArgs()
              {
                Source = (object) tileviewitem
              });
              this.MinimizeditemsOrder.Add((object) tileviewitem);
              tileviewitem.HeaderCursor = Cursors.Arrow;
              tileviewitem.TileViewItemsMinimizeMethod(this.MinimizedItemsOrientation);
              tileviewitem.minMaxButton.SetValue(ToggleButton.IsCheckedProperty, (object) false);
              tileviewitem.disablePropertyChangedNotify = true;
              this.SetState(tileviewitem, TileViewItemState.Minimized);
              tileviewitem.disablePropertyChangedNotify = false;
            }
            this.ChangeDataTemplate(tileviewitem);
            this.ApplyTileViewContent(tileviewitem);
            this.ChangeHeaderTemplate(tileviewitem);
            this.ChangeHeaderContent(tileviewitem);
          }
        }
        if (this.EnableAnimation)
        {
          if (this.IsVirtualizing)
          {
            if (this.Columns > 1 && (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top))
              this.BottomMargin = 20.0;
            if (this.Rows > 1 && (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right))
              this.RightMargin = 20.0;
          }
          this.UpdateTileViewLayout(true);
        }
        else
          this.UpdateTileViewLayout();
        this.OnMaximizedItemChanged(new TileViewEventArgs()
        {
          Source = (object) this.maximizedItem
        });
      }
    }
    if (this.scroll == null)
      return;
    this.scroll.InvalidateScrollInfo();
  }

  internal void repCard_DragMoved(object sender, TileViewDragEventArgs DEArgs)
  {
    this.IsDragging = true;
    TileViewItem tileViewItem1 = sender as TileViewItem;
    bool flag = tileViewItem1.IsMovable && tileViewItem1.TileViewItemState != TileViewItemState.Maximized;
    TileViewCancelEventArgs args = new TileViewCancelEventArgs();
    args.Cancel = false;
    if (this.Repositioning != null)
      this.Repositioning((object) this, args);
    if (args.Cancel || !flag || !this.AllowItemRepositioning)
      return;
    Point position = DEArgs.MouseEventArgs.GetPosition((IInputElement) this);
    int int32_1 = Convert.ToInt32(Math.Floor(position.Y / (this.ActualHeight / Convert.ToDouble(this.Rows))));
    int int32_2 = Convert.ToInt32(Math.Floor(position.X / (this.ActualWidth / Convert.ToDouble(this.Columns))));
    int int32_3 = Convert.ToInt32(Math.Floor(position.Y / (this.ActualHeight / Convert.ToDouble(this.tileViewItems.Count - 1))));
    int int32_4 = Convert.ToInt32(Math.Floor(position.X / (this.ActualWidth / Convert.ToDouble(this.tileViewItems.Count - 1))));
    TileViewItem element1 = (TileViewItem) null;
    if (this.tileViewItems != null)
    {
      if (this.maximizedItem == null)
      {
        foreach (UIElement tileViewItem2 in (Collection<TileViewItem>) this.tileViewItems)
        {
          TileViewItem element2 = tileViewItem2 as TileViewItem;
          if (Grid.GetRow((UIElement) element2) == int32_1 && element2 != this.tileViewItem && Grid.GetColumn((UIElement) element2) == int32_2)
          {
            element1 = element2;
            break;
          }
        }
      }
      else
      {
        int num1 = Grid.GetRow((UIElement) this.maximizedItem) * this.Columns + Grid.GetColumn((UIElement) this.maximizedItem);
        int num2 = Grid.GetRow((UIElement) this.maximizedItem) * this.Columns + Grid.GetColumn((UIElement) this.maximizedItem);
        foreach (UIElement tileViewItem3 in (Collection<TileViewItem>) this.tileViewItems)
        {
          if (tileViewItem3 is TileViewItem element3)
          {
            if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
            {
              int num3 = Grid.GetRow((UIElement) element3) * this.Columns + Grid.GetColumn((UIElement) element3);
              if (element3.TileViewItemState == TileViewItemState.Minimized)
              {
                if (num2 < num3)
                  --num3;
                if (num3 == int32_4 && element3 != this.tileViewItem)
                {
                  element1 = element3;
                  break;
                }
              }
            }
            else
            {
              int num4 = Grid.GetRow((UIElement) element3) * this.Columns + Grid.GetColumn((UIElement) element3);
              if (element3.TileViewItemState == TileViewItemState.Minimized)
              {
                if (num1 < num4)
                  --num4;
                if (num4 == int32_3 && element3 != this.tileViewItem)
                {
                  element1 = element3;
                  break;
                }
              }
            }
          }
        }
      }
    }
    if (element1 == null || this.tileViewItem == null)
      return;
    int column = Grid.GetColumn((UIElement) element1);
    int row = Grid.GetRow((UIElement) element1);
    Grid.SetColumn((UIElement) element1, Grid.GetColumn((UIElement) this.tileViewItem));
    Grid.SetRow((UIElement) element1, Grid.GetRow((UIElement) this.tileViewItem));
    Grid.SetColumn((UIElement) this.tileViewItem, column);
    Grid.SetRow((UIElement) this.tileViewItem, row);
    this.AnimateTileViewLayout();
  }

  private void repCard_DragFinished(object sender, TileViewDragEventArgs DEArgs)
  {
    if (this.tileViewItem == null)
      return;
    this.tileViewItem.Opacity = 1.0;
    this.tileViewItem = (TileViewItem) null;
    this.UpdateTileViewLayout();
  }

  internal void ChangeOrder(int[] arr)
  {
    Dictionary<int, TileViewItem> TileViewItemOrder = new Dictionary<int, TileViewItem>();
    if (this.tileViewItems != null)
    {
      for (int index = 0; index < this.Items.Count; ++index)
      {
        if (this.tileViewItems[index].GetType() == typeof (TileViewItem))
        {
          TileViewItem initialTileViewItem = this.initialTileViewItems[index];
          TileViewItemOrder.Add(index, initialTileViewItem);
        }
      }
    }
    this.GetCanvasHeight();
    this.SetRowsAndColumns(TileViewItemOrder);
    this.UpdateTileViewLayout();
  }

  public void SaveOrder()
  {
    int[] o = new int[this.Items.Count];
    for (int index1 = 0; index1 < this.Items.Count; ++index1)
    {
      object obj = this.Items[index1];
      for (int index2 = 0; index2 < this.tileViewItems.Count; ++index2)
      {
        ContentPresenter tileViewContent = this.tileViewItems[index2].TileViewContent;
        if (this.ItemsSource != null)
        {
          if (obj == tileViewContent.DataContext)
            o[index1] = index2;
        }
        else if (obj == tileViewContent.Content)
          o[index1] = index2;
      }
    }
    this.initialTileViewItems.Clear();
    for (int index = 0; index < o.Length; ++index)
      this.initialTileViewItems.Add(this.tileViewItems[index]);
    if (File.Exists("Order.xml") && File.Exists("Order.xml"))
      File.Delete("Order.xml");
    using (FileStream fileStream = new FileStream("Order.xml", FileMode.Create))
    {
      new XmlSerializer(typeof (int[])).Serialize((Stream) fileStream, (object) o);
      fileStream.Flush();
      fileStream.Close();
    }
  }

  public void LoadOrder()
  {
    int[] arr = new int[this.Items.Count];
    if (File.Exists("Order.xml"))
    {
      using (FileStream fileStream = new FileStream("Order.xml", FileMode.Open))
      {
        arr = (int[]) new XmlSerializer(typeof (int[])).Deserialize((Stream) fileStream);
        fileStream.Flush();
        fileStream.Close();
      }
    }
    this.ChangeOrder(arr);
  }

  private void repCard_DragStarted(object sender, TileViewDragEventArgs DEArgs)
  {
    TileViewItem tileViewItem = sender as TileViewItem;
    if (!tileViewItem.IsMovable || tileViewItem.TileViewItemState == TileViewItemState.Maximized || !this.AllowItemRepositioning)
      return;
    this.tileViewItem = sender as TileViewItem;
    this.tileViewItem.Opacity = 0.7;
  }

  private void TileViewControl_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.OldDesiredHeight = e.PreviousSize.Height;
    this.OldDesiredWidth = e.PreviousSize.Width;
    this.GetCanvasHeight();
    if (this.IsVirtualizing && this.itemsPanel is TileViewVirtualizingPanel)
    {
      this.IsScroll = true;
      this.IsSizechanged = true;
      this.TileVirtualizingPanel.InvalidateVisual();
      if (!this.isTileItemsLoaded)
        this.UpdateTileViewLayout();
      this.TileVirtualizingPanel.InvalidateMeasure();
    }
    else
      this.UpdateTileViewLayout();
  }

  private void TileViewControl_LayoutUpdated(object sender, EventArgs e)
  {
    if (!DesignerProperties.GetIsInDesignMode((DependencyObject) this))
      return;
    Dictionary<int, TileViewItem> TileViewItemOrder = new Dictionary<int, TileViewItem>();
    if (this.tileViewItems != null)
    {
      for (int index = 0; index < this.tileViewItems.Count; ++index)
      {
        if (this.tileViewItems[index].GetType() == typeof (TileViewItem))
        {
          TileViewItem tileViewItem = this.tileViewItems[index];
          TileViewItemOrder.Add(index, tileViewItem);
        }
      }
    }
    this.GetCanvasHeight();
    this.SetRowsAndColumns(TileViewItemOrder);
    this.UpdateTileViewLayout();
  }

  internal void RC_Minimized(object sender, CancelEventArgs e)
  {
  }

  public static void OnClickHeaderToMaximizePropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as TileViewControl).OnClickHeaderToMaximizePropertyChanged(args);
  }

  protected virtual void OnClickHeaderToMaximizePropertyChanged(
    DependencyPropertyChangedEventArgs args)
  {
    if (this.IsClickHeaderToMaximizePropertyChanged == null)
      return;
    this.IsClickHeaderToMaximizePropertyChanged((DependencyObject) this, args);
  }

  public event TileViewControl.TileViewEventHandler Minimized;

  public event TileViewControl.TileViewEventHandler Minimizing;

  public event TileViewControl.TileViewCancelEventHandler Maximizing;

  public event TileViewControl.TileViewEventHandler Maximized;

  public event TileViewControl.TileViewCancelEventHandler Restoring;

  public event TileViewControl.TileViewEventHandler Restored;

  public event TileViewControl.TileViewEventHandler MaximizedItemChanged;

  protected virtual void OnMinimized(TileViewEventArgs e)
  {
    if (!this.UseNormalState)
      this.ChangeMinMaxButtonVisibility(e.Source, Visibility.Visible);
    if (this.Minimized == null)
      return;
    this.Minimized((object) this, e);
  }

  protected virtual void OnMinimizing(TileViewEventArgs e)
  {
    if (this.Minimizing == null)
      return;
    this.Minimizing((object) this, e);
  }

  protected virtual void OnMaximizedItemChanged(TileViewEventArgs e)
  {
    if (this.MaximizedItemChanged == null)
      return;
    this.MaximizedItemChanged((object) this, e);
  }

  protected virtual void OnRestoring(TileViewCancelEventArgs e)
  {
    if (this.Restoring == null)
      return;
    this.Restoring((object) this, e);
  }

  protected virtual void OnRestored(TileViewEventArgs e)
  {
    if (!this.UseNormalState)
      this.ChangeMinMaxButtonVisibility(e.Source, Visibility.Visible);
    if (this.Restored == null)
      return;
    this.Restored((object) this, e);
  }

  protected virtual void OnMaximizing(TileViewCancelEventArgs e)
  {
    if (!this.UseNormalState)
      this.ChangeMinMaxButtonVisibility(e.Source, Visibility.Collapsed);
    if (this.Maximizing == null)
      return;
    this.Maximizing((object) this, e);
  }

  protected virtual void OnMaximized(TileViewEventArgs e)
  {
    if (this.Maximized == null)
      return;
    this.Maximized((object) this, e);
  }

  internal void ChangeMinMaxButtonVisibility(object obj, Visibility visible)
  {
    if (obj is TileViewItem)
    {
      (obj as TileViewItem).minMaxButton.Visibility = visible;
    }
    else
    {
      if (!(this.ItemContainerGenerator.ContainerFromItem(obj) is TileViewItem tileViewItem))
        return;
      tileViewItem.minMaxButton.Visibility = visible;
    }
  }

  public void GetTileViewItemsSizesforSplitter()
  {
    if (double.IsInfinity(this.ActualWidth) || double.IsNaN(this.ActualWidth) || this.ActualWidth == 0.0)
      return;
    if (this.maximizedItem == null)
    {
      foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      {
        double width = this.ActualWidth / Convert.ToDouble(this.Columns) - tileViewItem.Margin.Left - tileViewItem.Margin.Right;
        double height = this.ActualHeight / Convert.ToDouble(this.Rows) - tileViewItem.Margin.Top - tileViewItem.Margin.Bottom;
        if (width < 0.0)
          width = 0.0;
        if (height < 0.0)
          height = 0.0;
        tileViewItem.AnimateSize(width, height);
      }
    }
    else
    {
      double actualHeight1 = this.ActualHeight;
      double actualWidth1 = this.ActualWidth;
      int num1 = 0;
      int num2 = 1;
      bool flag = false;
      this.TileViewItemOrder = new Dictionary<int, TileViewItem>();
      int num3 = 0;
      if (this.tileViewItems != null && this.tileViewItems != null)
      {
        foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
          this.TileViewItemOrder.Add(Grid.GetRow((UIElement) tileViewItem) * this.Columns + Grid.GetColumn((UIElement) tileViewItem), tileViewItem);
      }
      for (int key = 0; key < this.TileViewItemOrder.Count; ++key)
      {
        if (this.TileViewItemOrder[key] != this.maximizedItem)
        {
          ++num3;
          double height;
          double width;
          if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
          {
            if (this.TileViewItemOrder[key] == this.DraggedItem)
            {
              height = this.minimizedRowHeight - this.TileViewItemOrder[key].Margin.Top - this.TileViewItemOrder[key].Margin.Bottom;
              flag = true;
              double num4 = this.TileViewItemOrder[key].OnMinimizedWidth.Value;
              actualHeight1 -= num4;
              num2 += num1;
              this.TileViewItemOrder[key].OnMinimizedWidth = new GridLength(num4, GridUnitType.Pixel);
              width = this.TileViewItemOrder[key].OnMinimizedWidth.Value;
            }
            else if (flag)
            {
              height = this.minimizedRowHeight - this.TileViewItemOrder[key].Margin.Top - this.TileViewItemOrder[key].Margin.Bottom;
              flag = false;
              this.TileViewItemOrder[key].OnMinimizedWidth = new GridLength(this.TileViewItemOrder[key].ActualWidth + this.draggedWidth, GridUnitType.Pixel);
              width = this.TileViewItemOrder[key].OnMinimizedWidth.Value;
            }
            else
            {
              height = this.minimizedRowHeight - this.TileViewItemOrder[key].Margin.Top - this.TileViewItemOrder[key].Margin.Bottom;
              this.TileViewItemOrder[key].OnMinimizedWidth = new GridLength(this.TileViewItemOrder[key].ActualWidth, GridUnitType.Pixel);
              width = this.TileViewItemOrder[key].OnMinimizedWidth.Value;
            }
            ++num1;
          }
          else
          {
            if (this.TileViewItemOrder[key] == this.DraggedItem)
            {
              width = this.minimizedColumnWidth - this.TileViewItemOrder[key].Margin.Right - this.TileViewItemOrder[key].Margin.Left;
              flag = true;
              double num5 = this.TileViewItemOrder[key].OnMinimizedHeight.Value;
              actualHeight1 -= num5;
              num2 += num1;
              this.TileViewItemOrder[key].OnMinimizedHeight = new GridLength(num5, GridUnitType.Pixel);
              height = this.TileViewItemOrder[key].OnMinimizedHeight.Value;
            }
            else if (flag)
            {
              width = this.minimizedColumnWidth - this.TileViewItemOrder[key].Margin.Right - this.TileViewItemOrder[key].Margin.Left;
              flag = false;
              this.TileViewItemOrder[key].OnMinimizedHeight = new GridLength(this.TileViewItemOrder[key].ActualHeight + this.draggedHeight, GridUnitType.Pixel);
              height = this.TileViewItemOrder[key].OnMinimizedHeight.Value;
            }
            else
            {
              width = this.minimizedColumnWidth - this.TileViewItemOrder[key].Margin.Right - this.TileViewItemOrder[key].Margin.Left;
              this.TileViewItemOrder[key].OnMinimizedHeight = new GridLength(this.TileViewItemOrder[key].ActualHeight, GridUnitType.Pixel);
              height = this.TileViewItemOrder[key].OnMinimizedHeight.Value;
            }
            ++num1;
          }
          if (height < 0.0)
            height = 10.0;
          if (width < 0.0)
            width = 10.0;
          this.TileViewItemOrder[key].AnimateSize(width, height);
        }
        else
        {
          double actualHeight2 = this.ActualHeight;
          double actualWidth2 = this.ActualWidth;
          double width;
          double height;
          if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
          {
            width = this.ActualWidth - this.TileViewItemOrder[key].Margin.Right - this.TileViewItemOrder[key].Margin.Left;
            height = this.ActualHeight - this.minimizedRowHeight - this.TileViewItemOrder[key].Margin.Top - this.TileViewItemOrder[key].Margin.Bottom;
          }
          else
          {
            width = this.ActualWidth - this.minimizedColumnWidth - this.TileViewItemOrder[key].Margin.Right - this.TileViewItemOrder[key].Margin.Left;
            height = this.ActualHeight - this.TileViewItemOrder[key].Margin.Top - this.TileViewItemOrder[key].Margin.Bottom;
          }
          this.TileViewItemOrder[key].AnimateSize(width, height);
        }
      }
    }
  }

  public void AnimateTileViewLayoutforSplitter()
  {
    if (!double.IsInfinity(this.ActualWidth) && !double.IsNaN(this.ActualWidth) && this.ActualWidth == 0.0)
      return;
    if (this.maximizedItem == null)
    {
      if (this.tileViewItems == null)
        return;
      foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
      {
        if (tileViewItem != this.tileViewItem)
        {
          double x = Convert.ToDouble((double) Grid.GetColumn((UIElement) tileViewItem) * (this.ActualWidth / Convert.ToDouble(this.Columns)));
          double y = Convert.ToDouble((double) Grid.GetRow((UIElement) tileViewItem) * (this.ActualHeight / Convert.ToDouble(this.Rows)));
          tileViewItem.AnimatePosition(x, y);
        }
      }
    }
    else
    {
      double actualHeight1 = this.ActualHeight;
      double actualWidth1 = this.ActualWidth;
      bool flag = false;
      int num1 = 0;
      this.TileViewItemOrder = new Dictionary<int, TileViewItem>();
      double num2 = 0.0;
      if (this.tileViewItems != null)
      {
        foreach (TileViewItem tileViewItem in (Collection<TileViewItem>) this.tileViewItems)
        {
          int key = Grid.GetRow((UIElement) tileViewItem) * this.Columns + Grid.GetColumn((UIElement) tileViewItem);
          if (!this.TileViewItemOrder.ContainsKey(key))
            this.TileViewItemOrder.Add(key, tileViewItem);
        }
      }
      for (int key = 0; key < this.TileViewItemOrder.Count; ++key)
      {
        if (this.TileViewItemOrder[key] != this.maximizedItem)
        {
          if (this.TileViewItemOrder[key] == this.DraggedItem)
          {
            flag = true;
            if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
            {
              double num3 = this.TileViewItemOrder[key].OnMinimizedWidth.Value;
            }
            else
            {
              double num4 = this.TileViewItemOrder[key].OnMinimizedHeight.Value;
            }
          }
          else if (flag)
          {
            if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
            {
              flag = false;
              double num5 = this.TileViewItemOrder[key].OnMinimizedWidth.Value;
              double right = this.TileViewItemOrder[key].Margin.Right;
              double left = this.TileViewItemOrder[key].Margin.Left;
            }
            else
            {
              flag = false;
              double num6 = this.TileViewItemOrder[key].OnMinimizedHeight.Value;
              double right = this.TileViewItemOrder[key].Margin.Right;
              double left = this.TileViewItemOrder[key].Margin.Left;
            }
          }
          else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
          {
            double actualWidth2 = this.TileViewItemOrder[key].ActualWidth;
          }
          else
          {
            double actualHeight2 = this.TileViewItemOrder[key].ActualHeight;
          }
          double x = 0.0;
          double y = num2;
          double actualHeight3 = this.ActualHeight;
          double actualWidth3 = this.ActualWidth;
          if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
          {
            x = 0.0;
            y = num2;
          }
          else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top)
          {
            x = num2;
            y = 0.0;
          }
          else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
          {
            x = this.ActualWidth - this.minimizedColumnWidth;
            y = num2;
          }
          else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
          {
            x = num2;
            y = this.ActualHeight - this.minimizedRowHeight;
          }
          this.TileViewItemOrder[key].AnimatePosition(x, y);
          if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
          {
            if (num1 > 0)
              num2 += this.TileViewItemOrder[key].OnMinimizedHeight.Value + this.TileViewItemOrder[key].Margin.Top + this.TileViewItemOrder[key].Margin.Bottom;
            else if (this.TileViewItemOrder[0].TileViewItemState == TileViewItemState.Minimized)
              num2 += this.TileViewItemOrder[key].OnMinimizedHeight.Value + this.TileViewItemOrder[key].Margin.Top + this.TileViewItemOrder[key].Margin.Bottom;
            else
              num2 += this.TileViewItemOrder[key].OnMinimizedHeight.Value;
          }
          else if (num1 > 0)
            num2 += this.TileViewItemOrder[key].OnMinimizedWidth.Value + this.TileViewItemOrder[key].Margin.Right + this.TileViewItemOrder[key].Margin.Left;
          else if (this.TileViewItemOrder[0].TileViewItemState == TileViewItemState.Minimized)
            num2 += this.TileViewItemOrder[key].OnMinimizedWidth.Value + this.TileViewItemOrder[key].Margin.Right + this.TileViewItemOrder[key].Margin.Left;
          else
            num2 += this.TileViewItemOrder[key].OnMinimizedWidth.Value;
        }
        else
        {
          double x = 0.0;
          double y = 0.0;
          if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
          {
            x = this.minimizedColumnWidth;
            y = 0.0;
          }
          else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Top)
          {
            x = 0.0;
            y = this.minimizedRowHeight;
          }
          if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
          {
            x = 0.0;
            y = 0.0;
          }
          else if (this.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
          {
            x = 0.0;
            y = 0.0;
          }
          this.TileViewItemOrder[key].AnimatePosition(x, y);
        }
        ++num1;
      }
    }
  }

  protected override AutomationPeer OnCreateAutomationPeer()
  {
    return (AutomationPeer) new TileViewControlAutomationPeer(this);
  }

  public void Dispose()
  {
    this.DraggedItem = (TileViewItem) null;
    this.SwappedfromMaximized = (TileViewItem) null;
    this.SwappedfromMinimized = (TileViewItem) null;
    this.ItemsHeaderHeight = (ArrayList) null;
    this.MinimizeditemsOrder = (ArrayList) null;
    this.tileViewItem = (TileViewItem) null;
    this.maximizedItem = (TileViewItem) null;
    this.TileViewItemOrder = (Dictionary<int, TileViewItem>) null;
    this.itemsPanel = (Panel) null;
    this.initialTileViewItems = (List<TileViewItem>) null;
    this.updatedtileviewitems = (List<TileViewItem>) null;
    this.orderedtileviewitems = (List<TileViewItem>) null;
    this.MainGrid = (Grid) null;
    this._tileviewcanvasleft = (List<object>) null;
    this._tileviewcanvastop = (List<object>) null;
    this.currentposition = (List<int>) null;
    this.scroll = (ScrollViewer) null;
    this.sortedcanvasleft = (List<object>) null;
    this.updatedcanvasleft = (List<object>) null;
    this.updatedcanvastop = (List<object>) null;
    this.TileVirtualizingPanel = (TileViewVirtualizingPanel) null;
    if (this.tileViewItems != null)
      this.tileViewItems.Clear();
    if (this.ItemsSource == null && this.Items != null)
    {
      foreach (object obj in (IEnumerable) this.Items)
      {
        if (obj != null && this.ItemContainerGenerator != null && this.ItemContainerGenerator.ContainerFromItem(obj) is TileViewItem tileViewItem)
          tileViewItem.Dispose();
      }
      this.Items.Clear();
    }
    else
      this.ItemsSource = (IEnumerable) null;
    this.tileViewItems = (ObservableCollection<TileViewItem>) null;
    this.VirtualizingTileItemsCollection = (UIElementCollection) null;
    GC.Collect();
    GC.SuppressFinalize((object) this);
  }

  public delegate void TileViewCancelEventHandler(object sender, TileViewCancelEventArgs args);

  public delegate void TileViewEventHandler(object sender, TileViewEventArgs args);
}
