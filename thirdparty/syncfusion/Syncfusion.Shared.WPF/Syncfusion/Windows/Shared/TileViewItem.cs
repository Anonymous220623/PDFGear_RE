// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TileViewItem
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class TileViewItem : TileViewItemBase
{
  private const string MinMaxButtonName = "MinMaxButton";
  internal TileViewItem Currentitem;
  internal TileViewItem Nextitem;
  internal Point HeaderPoint = new Point(0.0, 0.0);
  internal Border split = new Border();
  internal Popup Splitpopup;
  internal bool Splitflag;
  private Point startpoint = new Point(0.0, 0.0);
  private Point endpoint = new Point(0.0, 0.0);
  public Grid mainGrid = new Grid();
  internal bool disablePropertyChangedNotify;
  internal ToggleButton minMaxButton;
  public ContentPresenter TileViewContent;
  public ContentPresenter HeaderContent;
  internal Button closeButton;
  internal Border HeaderPart;
  internal Grid PART_Content;
  private int TileViewItemIndex;
  internal MinimizedItemsOrientation MinPos;
  public static readonly DependencyProperty ShareSpaceProperty = DependencyProperty.Register(nameof (ShareSpace), typeof (bool), typeof (TileViewItem), new PropertyMetadata((object) true));
  public static readonly DependencyProperty IsOverrideItemTemplateProperty = DependencyProperty.Register(nameof (IsOverrideItemTemplate), typeof (bool), typeof (TileViewItem), new PropertyMetadata((object) false));
  public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof (ItemTemplate), typeof (DataTemplate), typeof (TileViewItem), new PropertyMetadata((object) null, new PropertyChangedCallback(TileViewItem.OnItemTemplateChanged)));
  public static readonly DependencyProperty MinimizedItemTemplateProperty = DependencyProperty.Register(nameof (MinimizedItemTemplate), typeof (DataTemplate), typeof (TileViewItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MaximizedItemTemplateProperty = DependencyProperty.Register(nameof (MaximizedItemTemplate), typeof (DataTemplate), typeof (TileViewItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty HeaderBorderBrushProperty = DependencyProperty.Register(nameof (HeaderBorderBrush), typeof (Brush), typeof (TileViewItem), new PropertyMetadata((object) new SolidColorBrush(Colors.Black)));
  public static readonly DependencyProperty HeaderBorderThicknessProperty = DependencyProperty.Register(nameof (HeaderBorderThickness), typeof (Thickness), typeof (TileViewItem), new PropertyMetadata((object) new Thickness(1.0)));
  public static readonly DependencyProperty MinMaxButtonToolTipProperty = DependencyProperty.Register(nameof (MinMaxButtonToolTip), typeof (string), typeof (TileViewItem), new PropertyMetadata((object) "Maximize"));
  public static readonly DependencyProperty HeaderCornerRadiusProperty = DependencyProperty.Register(nameof (HeaderCornerRadius), typeof (CornerRadius), typeof (TileViewItem), new PropertyMetadata((object) new CornerRadius(0.0)));
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (TileViewItem), new PropertyMetadata((object) new CornerRadius(0.0)));
  public static readonly DependencyProperty HeaderCursorProperty = DependencyProperty.Register(nameof (HeaderCursor), typeof (Cursor), typeof (TileViewItem), new PropertyMetadata((object) Cursors.Hand));
  public static readonly DependencyProperty MinMaxButtonMarginProperty = DependencyProperty.Register(nameof (MinMaxButtonMargin), typeof (Thickness), typeof (TileViewItem), new PropertyMetadata((object) new Thickness(0.0, 0.0, 0.0, 0.0)));
  public static readonly DependencyProperty CloseButtonMarginProperty = DependencyProperty.Register(nameof (CloseButtonMargin), typeof (Thickness), typeof (TileViewItem), new PropertyMetadata((object) new Thickness(0.0, 0.0, 0.0, 0.0)));
  public static readonly DependencyProperty MinMaxButtonStyleProperty = DependencyProperty.Register(nameof (MinMaxButtonStyle), typeof (Style), typeof (TileViewItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CloseButtonStyleProperty = DependencyProperty.Register(nameof (CloseButtonStyle), typeof (Style), typeof (TileViewItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty TileViewItemStateProperty = DependencyProperty.Register(nameof (TileViewItemState), typeof (TileViewItemState), typeof (TileViewItem), new PropertyMetadata((object) TileViewItemState.Normal, new PropertyChangedCallback(TileViewItem.OnEnumLoad)));
  public static readonly DependencyProperty MinMaxButtonBackgroundProperty = DependencyProperty.Register(nameof (MinMaxButtonBackground), typeof (Brush), typeof (TileViewItem), new PropertyMetadata((object) new SolidColorBrush()));
  internal static readonly DependencyProperty MinMaxButtonBorderBrushProperty = DependencyProperty.Register(nameof (MinMaxButtonBorderBrush), typeof (Brush), typeof (TileViewItem), new PropertyMetadata((object) new SolidColorBrush(Colors.White)));
  internal static readonly DependencyProperty MinMaxButtonThicknessProperty = DependencyProperty.Register(nameof (MinMaxButtonThickness), typeof (Thickness), typeof (TileViewItem), new PropertyMetadata((object) new Thickness(1.0, 1.0, 1.0, 1.0)));
  public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register(nameof (HeaderVisibility), typeof (Visibility), typeof (TileViewItem), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register(nameof (HeaderBackground), typeof (Brush), typeof (TileViewItem), new PropertyMetadata((object) new SolidColorBrush(Colors.Transparent)));
  public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register(nameof (HeaderForeground), typeof (Brush), typeof (TileViewItem), new PropertyMetadata((object) new SolidColorBrush(Colors.Black)));
  public static readonly DependencyProperty MinMaxButtonVisibilityProperty = DependencyProperty.Register(nameof (MinMaxButtonVisibility), typeof (Visibility), typeof (TileViewItem), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty CloseButtonVisibilityProperty = DependencyProperty.Register(nameof (CloseButtonVisibility), typeof (Visibility), typeof (TileViewItem), new PropertyMetadata((object) Visibility.Collapsed));
  public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.Register(nameof (HeaderHeight), typeof (double), typeof (TileViewItem), new PropertyMetadata((object) 20.0));
  public static readonly DependencyProperty SplitColumnProperty = DependencyProperty.Register(nameof (SplitColumn), typeof (int), typeof (TileViewItem), new PropertyMetadata((object) 1, (PropertyChangedCallback) null));
  public static readonly DependencyProperty SplitRowProperty = DependencyProperty.Register(nameof (SplitRow), typeof (int), typeof (TileViewItem), new PropertyMetadata((object) 0, (PropertyChangedCallback) null));
  public static readonly DependencyProperty BorderColumnProperty = DependencyProperty.Register(nameof (BorderColumn), typeof (int), typeof (TileViewItem), new PropertyMetadata((object) 1, (PropertyChangedCallback) null));
  public static readonly DependencyProperty BorderRowProperty = DependencyProperty.Register(nameof (BorderRow), typeof (int), typeof (TileViewItem), new PropertyMetadata((object) 0, (PropertyChangedCallback) null));
  public static readonly DependencyProperty OnMinimizedHeightProperty = DependencyProperty.Register(nameof (OnMinimizedHeight), typeof (GridLength), typeof (TileViewItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty OnMinimizedWidthProperty = DependencyProperty.Register(nameof (OnMinimizedWidth), typeof (GridLength), typeof (TileViewItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ItemContentTemplateProperty = DependencyProperty.Register(nameof (ItemContentTemplate), typeof (DataTemplate), typeof (TileViewItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MaximizedItemContentProperty = DependencyProperty.Register(nameof (MaximizedItemContent), typeof (object), typeof (TileViewItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MinimizedItemContentProperty = DependencyProperty.Register(nameof (MinimizedItemContent), typeof (object), typeof (TileViewItem), new PropertyMetadata((PropertyChangedCallback) null));
  public new static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (TileViewItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (TileViewItem), new PropertyMetadata((object) false, new PropertyChangedCallback(TileViewItem.OnIsSelectedPropertyChanged)));
  public static readonly DependencyProperty MinimizedHeaderProperty = DependencyProperty.Register(nameof (MinimizedHeader), typeof (object), typeof (TileViewItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MaximizedHeaderProperty = DependencyProperty.Register(nameof (MaximizedHeader), typeof (string), typeof (TileViewItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CloseModeProperty = DependencyProperty.Register(nameof (CloseMode), typeof (CloseMode), typeof (TileViewItem), new PropertyMetadata((object) CloseMode.Hide));
  private GridSplitter gsp;
  private int ind;
  private int itemscount;

  internal event EventHandler CardMaximized;

  internal event EventHandler CardNormal;

  internal event EventHandler CardMinimized;

  public event TileViewItem.TileViewEventHandler StateChanged;

  public event TileViewItem.TileViewCancelEventHandler StateChanging;

  public event RoutedEventHandler Selected;

  public TileViewItem()
  {
    this.DefaultStyleKey = (object) typeof (TileViewItem);
    this.KeyDown += new KeyEventHandler(this.TileViewItem_KeyDown);
  }

  private void TileViewItem_KeyDown(object sender, KeyEventArgs e)
  {
    TileViewItem container = sender as TileViewItem;
    int num = -1;
    if (container.ParentTileViewControl != null && container.ParentTileViewControl.ItemContainerGenerator != null)
      num = container.ParentTileViewControl.ItemContainerGenerator.IndexFromContainer((DependencyObject) container);
    container.ParentTileViewControl.SelectedIndex = num;
  }

  public event TileViewItem.CloseEventHandler Closed;

  public event TileViewItem.CloseEventHandler Closing;

  protected virtual void OnClosing(TileViewItem.CloseEventArgs e)
  {
    if (this.Closing == null)
      return;
    this.Closing((object) this, e);
  }

  protected virtual void OnClosed(TileViewItem.CloseEventArgs e)
  {
    if (this.Closed == null)
      return;
    this.Closed((object) this, e);
  }

  public bool IsOverrideItemTemplate
  {
    get => (bool) this.GetValue(TileViewItem.IsOverrideItemTemplateProperty);
    set => this.SetValue(TileViewItem.IsOverrideItemTemplateProperty, (object) value);
  }

  public DataTemplate ItemTemplate
  {
    get => (DataTemplate) this.GetValue(TileViewItem.ItemTemplateProperty);
    set => this.SetValue(TileViewItem.ItemTemplateProperty, (object) value);
  }

  public DataTemplate MinimizedItemTemplate
  {
    get => (DataTemplate) this.GetValue(TileViewItem.MinimizedItemTemplateProperty);
    set => this.SetValue(TileViewItem.MinimizedItemTemplateProperty, (object) value);
  }

  public DataTemplate MaximizedItemTemplate
  {
    get => (DataTemplate) this.GetValue(TileViewItem.MaximizedItemTemplateProperty);
    set => this.SetValue(TileViewItem.MaximizedItemTemplateProperty, (object) value);
  }

  internal int RepCardIndex
  {
    get => this.TileViewItemIndex;
    set => this.TileViewItemIndex = value;
  }

  internal ToggleButton MinMaxButton => this.minMaxButton;

  public TileViewItemState TileViewItemState
  {
    get => (TileViewItemState) this.GetValue(TileViewItem.TileViewItemStateProperty);
    set => this.SetValue(TileViewItem.TileViewItemStateProperty, (object) value);
  }

  public Brush MinMaxButtonBackground
  {
    get => (Brush) this.GetValue(TileViewItem.MinMaxButtonBackgroundProperty);
    set => this.SetValue(TileViewItem.MinMaxButtonBackgroundProperty, (object) value);
  }

  internal Brush MinMaxButtonBorderBrush
  {
    get => (Brush) this.GetValue(TileViewItem.MinMaxButtonBorderBrushProperty);
    set => this.SetValue(TileViewItem.MinMaxButtonBorderBrushProperty, (object) value);
  }

  internal Thickness MinMaxButtonThickness
  {
    get => (Thickness) this.GetValue(TileViewItem.MinMaxButtonThicknessProperty);
    set => this.SetValue(TileViewItem.MinMaxButtonThicknessProperty, (object) value);
  }

  public Brush HeaderBackground
  {
    get => (Brush) this.GetValue(TileViewItem.HeaderBackgroundProperty);
    set => this.SetValue(TileViewItem.HeaderBackgroundProperty, (object) value);
  }

  public Brush HeaderForeground
  {
    get => (Brush) this.GetValue(TileViewItem.HeaderForegroundProperty);
    set => this.SetValue(TileViewItem.HeaderForegroundProperty, (object) value);
  }

  public Visibility HeaderVisibility
  {
    get => (Visibility) this.GetValue(TileViewItem.HeaderVisibilityProperty);
    set => this.SetValue(TileViewItem.HeaderVisibilityProperty, (object) value);
  }

  public Thickness MinMaxButtonMargin
  {
    get => (Thickness) this.GetValue(TileViewItem.MinMaxButtonMarginProperty);
    set => this.SetValue(TileViewItem.MinMaxButtonMarginProperty, (object) value);
  }

  public Thickness CloseButtonMargin
  {
    get => (Thickness) this.GetValue(TileViewItem.CloseButtonMarginProperty);
    set => this.SetValue(TileViewItem.CloseButtonMarginProperty, (object) value);
  }

  public Style MinMaxButtonStyle
  {
    get => (Style) this.GetValue(TileViewItem.MinMaxButtonStyleProperty);
    set => this.SetValue(TileViewItem.MinMaxButtonStyleProperty, (object) value);
  }

  public Style CloseButtonStyle
  {
    get => (Style) this.GetValue(TileViewItem.CloseButtonStyleProperty);
    set => this.SetValue(TileViewItem.CloseButtonStyleProperty, (object) value);
  }

  public Brush HeaderBorderBrush
  {
    get => (Brush) this.GetValue(TileViewItem.HeaderBorderBrushProperty);
    set => this.SetValue(TileViewItem.HeaderBorderBrushProperty, (object) value);
  }

  public bool ShareSpace
  {
    get => (bool) this.GetValue(TileViewItem.ShareSpaceProperty);
    set => this.SetValue(TileViewItem.ShareSpaceProperty, (object) value);
  }

  public Thickness HeaderBorderThickness
  {
    get => (Thickness) this.GetValue(TileViewItem.HeaderBorderThicknessProperty);
    set => this.SetValue(TileViewItem.HeaderBorderThicknessProperty, (object) value);
  }

  public string MinMaxButtonToolTip
  {
    get => (string) this.GetValue(TileViewItem.MinMaxButtonToolTipProperty);
    set => this.SetValue(TileViewItem.MinMaxButtonToolTipProperty, (object) value);
  }

  public CornerRadius HeaderCornerRadius
  {
    get => (CornerRadius) this.GetValue(TileViewItem.HeaderCornerRadiusProperty);
    set => this.SetValue(TileViewItem.HeaderCornerRadiusProperty, (object) value);
  }

  public CornerRadius CornerRadius
  {
    get => (CornerRadius) this.GetValue(TileViewItem.CornerRadiusProperty);
    set => this.SetValue(TileViewItem.CornerRadiusProperty, (object) value);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public Cursor HeaderCursor
  {
    get => (Cursor) this.GetValue(TileViewItem.HeaderCursorProperty);
    set => this.SetValue(TileViewItem.HeaderCursorProperty, (object) value);
  }

  public Visibility MinMaxButtonVisibility
  {
    get => (Visibility) this.GetValue(TileViewItem.MinMaxButtonVisibilityProperty);
    set => this.SetValue(TileViewItem.MinMaxButtonVisibilityProperty, (object) value);
  }

  public Visibility CloseButtonVisibility
  {
    get => (Visibility) this.GetValue(TileViewItem.CloseButtonVisibilityProperty);
    set => this.SetValue(TileViewItem.CloseButtonVisibilityProperty, (object) value);
  }

  public double HeaderHeight
  {
    get => (double) this.GetValue(TileViewItem.HeaderHeightProperty);
    set => this.SetValue(TileViewItem.HeaderHeightProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public int SplitColumn
  {
    get => (int) this.GetValue(TileViewItem.SplitColumnProperty);
    set => this.SetValue(TileViewItem.SplitColumnProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public int SplitRow
  {
    get => (int) this.GetValue(TileViewItem.SplitRowProperty);
    set => this.SetValue(TileViewItem.SplitRowProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public int BorderColumn
  {
    get => (int) this.GetValue(TileViewItem.BorderColumnProperty);
    set => this.SetValue(TileViewItem.BorderColumnProperty, (object) value);
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public int BorderRow
  {
    get => (int) this.GetValue(TileViewItem.BorderRowProperty);
    set => this.SetValue(TileViewItem.BorderRowProperty, (object) value);
  }

  public GridLength OnMinimizedHeight
  {
    get => (GridLength) this.GetValue(TileViewItem.OnMinimizedHeightProperty);
    set => this.SetValue(TileViewItem.OnMinimizedHeightProperty, (object) value);
  }

  public GridLength OnMinimizedWidth
  {
    get => (GridLength) this.GetValue(TileViewItem.OnMinimizedWidthProperty);
    set => this.SetValue(TileViewItem.OnMinimizedWidthProperty, (object) value);
  }

  public object MaximizedItemContent
  {
    get => this.GetValue(TileViewItem.MaximizedItemContentProperty);
    set => this.SetValue(TileViewItem.MaximizedItemContentProperty, value);
  }

  public object MinimizedItemContent
  {
    get => this.GetValue(TileViewItem.MinimizedItemContentProperty);
    set => this.SetValue(TileViewItem.MinimizedItemContentProperty, value);
  }

  public DataTemplate ItemContentTemplate
  {
    get => (DataTemplate) this.GetValue(TileViewItem.ItemContentTemplateProperty);
    set => this.SetValue(TileViewItem.ItemContentTemplateProperty, (object) value);
  }

  public new DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(TileViewItem.HeaderTemplateProperty);
    set => this.SetValue(TileViewItem.HeaderTemplateProperty, (object) value);
  }

  public bool IsSelected
  {
    get => (bool) this.GetValue(TileViewItem.IsSelectedProperty);
    set => this.SetValue(TileViewItem.IsSelectedProperty, (object) value);
  }

  public object MinimizedHeader
  {
    get => this.GetValue(TileViewItem.MinimizedHeaderProperty);
    set => this.SetValue(TileViewItem.MinimizedHeaderProperty, value);
  }

  public object MaximizedHeader
  {
    get => this.GetValue(TileViewItem.MaximizedHeaderProperty);
    set => this.SetValue(TileViewItem.MaximizedHeaderProperty, value);
  }

  public CloseMode CloseMode
  {
    get => (CloseMode) this.GetValue(TileViewItem.CloseModeProperty);
    set => this.SetValue(TileViewItem.CloseModeProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    if (this.ParentTileViewControl != null)
    {
      this.ParentTileViewControl.IsMinMaxButtonOnMouseOverOnlyChanged -= new PropertyChangedCallback(this.tileviewControl_IsMinMaxButtonOnMouseOverOnlyChanged);
      this.ParentTileViewControl.AllowItemRepositioningChanged -= new PropertyChangedCallback(this.ParentTileViewControl_AllowItemRepositioningChanged);
      this.ParentTileViewControl.IsClickHeaderToMaximizePropertyChanged -= new PropertyChangedCallback(this.ParentTileViewControl_IsClickHeaderToMaximizePropertyChanged);
    }
    if (this.minMaxButton != null)
      this.minMaxButton.Click -= new RoutedEventHandler(this.MinMaxButton_Click);
    if (this.closeButton != null)
      this.closeButton.Click -= new RoutedEventHandler(this.closeButton_Click);
    if (this.split != null)
    {
      this.split.MouseLeftButtonDown -= new MouseButtonEventHandler(this.split_MouseLeftButtonDown);
      this.split.MouseLeftButtonUp -= new MouseButtonEventHandler(this.split_MouseLeftButtonUp);
      this.split.MouseMove -= new MouseEventHandler(this.split_MouseMove);
      this.split.MouseEnter -= new MouseEventHandler(this.split_MouseEnter);
      this.split.MouseLeave -= new MouseEventHandler(this.split_MouseLeave);
    }
    base.OnApplyTemplate();
    this.minMaxButton = this.GetTemplateChild("MinMaxButton") as ToggleButton;
    this.closeButton = this.GetTemplateChild("CloseButton") as Button;
    this.HeaderPart = this.GetTemplateChild("FloatPanelArea") as Border;
    if (this.HeaderPart != null && this.ParentTileViewControl != null && (this.ParentTileViewControl.ClickHeaderToMaximize || this.OnMinimizedHeight.Value != 0.0 || this.OnMinimizedWidth.Value != 0.0) && this.HeaderPart != null)
    {
      this.HeaderPart.MouseLeftButtonUp -= new MouseButtonEventHandler(this.HeaderPart_MouseLeftButtonUp);
      this.HeaderPart.MouseLeftButtonUp += new MouseButtonEventHandler(this.HeaderPart_MouseLeftButtonUp);
    }
    if (this.minMaxButton != null)
    {
      this.minMaxButton.Click += new RoutedEventHandler(this.MinMaxButton_Click);
      if (this.TileViewItemState == TileViewItemState.Normal)
        this.minMaxButton.IsChecked = new bool?(false);
      else if (this.TileViewItemState == TileViewItemState.Maximized)
        this.minMaxButton.IsChecked = new bool?(true);
      if (this.ParentTileViewControl != null)
      {
        this.ParentTileViewControl.IsMinMaxButtonOnMouseOverOnlyChanged += new PropertyChangedCallback(this.tileviewControl_IsMinMaxButtonOnMouseOverOnlyChanged);
        this.ParentTileViewControl.AllowItemRepositioningChanged += new PropertyChangedCallback(this.ParentTileViewControl_AllowItemRepositioningChanged);
        this.ParentTileViewControl.IsClickHeaderToMaximizePropertyChanged += new PropertyChangedCallback(this.ParentTileViewControl_IsClickHeaderToMaximizePropertyChanged);
        this.ParentTileViewControl.ChangeDataTemplate(this);
        this.ParentTileViewControl.ChangeHeaderTemplate(this);
        this.ParentTileViewControl.IsMinMaxButtonOnMouseOverOnlyChanged += new PropertyChangedCallback(this.ParentTileViewControl_IsMinMaxButtonOnMouseOverOnlyChanged);
        if (this.ParentTileViewControl.IsMinMaxButtonOnMouseOverOnly)
          this.makeToggleButtonVisible(false);
        this.ParentTileViewControl.initialTileViewItems.Clear();
        for (int index = 0; index < this.ParentTileViewControl.tileViewItems.Count; ++index)
        {
          if (!this.ParentTileViewControl.IsVirtualizing)
          {
            if (this.ParentTileViewControl.initialTileViewItems.Count < this.ParentTileViewControl.tileViewItems.Count)
              this.ParentTileViewControl.initialTileViewItems.Add(this.ParentTileViewControl.tileViewItems[index]);
          }
          else if (!this.ParentTileViewControl.initialTileViewItems.Contains(this.ParentTileViewControl.tileViewItems[index]) && this.ParentTileViewControl.initialTileViewItems.Count < this.ParentTileViewControl.tileViewItems.Count)
            this.ParentTileViewControl.initialTileViewItems.Add(this.ParentTileViewControl.tileViewItems[index]);
        }
        if (!this.ParentTileViewControl.needToUpdate && !this.ParentTileViewControl.IsVirtualizing)
          this.ParentTileViewControl.UpdateTileViewLayout();
      }
    }
    if (this.TileViewItemState == TileViewItemState.Hidden)
      this.TileViewItemOnHiddenState(this);
    if (this.closeButton != null)
      this.closeButton.Click += new RoutedEventHandler(this.closeButton_Click);
    if (this.Parent != null)
    {
      if (!DesignerProperties.GetIsInDesignMode(this.Parent))
      {
        this.Splitpopup = this.GetTemplateChild("Splitpopup") as Popup;
        this.split = this.GetTemplateChild("SplitBorder") as Border;
        this.mainGrid = this.GetTemplateChild("itemGrid") as Grid;
        this.TileViewContent = this.GetTemplateChild("tileviewcontent") as ContentPresenter;
        this.HeaderContent = this.GetTemplateChild("HeaderContent") as ContentPresenter;
        if (this.split != null)
        {
          this.split.MouseLeftButtonDown += new MouseButtonEventHandler(this.split_MouseLeftButtonDown);
          this.split.MouseLeftButtonUp += new MouseButtonEventHandler(this.split_MouseLeftButtonUp);
          this.split.MouseMove += new MouseEventHandler(this.split_MouseMove);
          this.split.MouseEnter += new MouseEventHandler(this.split_MouseEnter);
          this.split.MouseLeave += new MouseEventHandler(this.split_MouseLeave);
        }
        this.LoadSplitter();
      }
      else
      {
        this.Splitpopup = this.GetTemplateChild("Splitpopup") as Popup;
        this.split = this.GetTemplateChild("SplitBorder") as Border;
        this.mainGrid = this.GetTemplateChild("itemGrid") as Grid;
        this.TileViewContent = this.GetTemplateChild("tileviewcontent") as ContentPresenter;
        this.HeaderContent = this.GetTemplateChild("HeaderContent") as ContentPresenter;
        this.LoadinDesignMode();
      }
    }
    else
    {
      this.Splitpopup = this.GetTemplateChild("Splitpopup") as Popup;
      this.split = this.GetTemplateChild("SplitBorder") as Border;
      this.mainGrid = this.GetTemplateChild("itemGrid") as Grid;
      this.TileViewContent = this.GetTemplateChild("tileviewcontent") as ContentPresenter;
      this.HeaderContent = this.GetTemplateChild("HeaderContent") as ContentPresenter;
      this.LoadinDesignMode();
      if (this.split != null)
      {
        this.split.MouseLeftButtonDown += new MouseButtonEventHandler(this.split_MouseLeftButtonDown);
        this.split.MouseLeftButtonUp += new MouseButtonEventHandler(this.split_MouseLeftButtonUp);
        this.split.MouseMove += new MouseEventHandler(this.split_MouseMove);
        this.split.MouseEnter += new MouseEventHandler(this.split_MouseEnter);
        this.split.MouseLeave += new MouseEventHandler(this.split_MouseLeave);
      }
      this.LoadSplitter();
    }
    this.TileViewContent = this.GetTemplateChild("tileviewcontent") as ContentPresenter;
    if (this.ParentTileViewControl != null)
      this.ParentTileViewControl.ApplyTileViewContent(this);
    this.PART_Content = this.GetTemplateChild("PART_Content") as Grid;
    if (this.PART_Content == null)
      return;
    Binding binding = new Binding("EnableTouch")
    {
      Mode = BindingMode.TwoWay,
      Source = (object) this.ParentTileViewControl
    };
    this.PART_Content.SetBinding(UIElement.IsManipulationEnabledProperty, (BindingBase) binding);
  }

  internal void Dispose()
  {
    if (this.ParentTileViewControl != null)
    {
      this.ParentTileViewControl.IsMinMaxButtonOnMouseOverOnlyChanged -= new PropertyChangedCallback(this.tileviewControl_IsMinMaxButtonOnMouseOverOnlyChanged);
      this.ParentTileViewControl.IsMinMaxButtonOnMouseOverOnlyChanged -= new PropertyChangedCallback(this.ParentTileViewControl_IsMinMaxButtonOnMouseOverOnlyChanged);
      this.ParentTileViewControl.AllowItemRepositioningChanged -= new PropertyChangedCallback(this.ParentTileViewControl_AllowItemRepositioningChanged);
      this.ParentTileViewControl.IsClickHeaderToMaximizePropertyChanged -= new PropertyChangedCallback(this.ParentTileViewControl_IsClickHeaderToMaximizePropertyChanged);
      if (this.ParentTileViewControl.initialTileViewItems != null && this.ParentTileViewControl.initialTileViewItems.Contains(this))
        this.ParentTileViewControl.initialTileViewItems.Remove(this);
      if (this.ParentTileViewControl.updatedtileviewitems != null && this.ParentTileViewControl.updatedtileviewitems.Contains(this))
        this.ParentTileViewControl.updatedtileviewitems.Remove(this);
      int key = 0;
      if (this.ParentTileViewControl.ItemContainerGenerator != null)
        key = this.ParentTileViewControl.ItemContainerGenerator.IndexFromContainer((DependencyObject) this);
      if (this.ParentTileViewControl.TileViewItemOrder != null && this.ParentTileViewControl.TileViewItemOrder.ContainsKey(key))
        this.ParentTileViewControl.TileViewItemOrder.Remove(key);
      else if (this.ParentTileViewControl.TileViewItemOrder != null)
        this.ParentTileViewControl.TileViewItemOrder.Remove(0);
      if (this.ParentTileViewControl.MinimizeditemsOrder != null && this.ParentTileViewControl.MinimizeditemsOrder.Contains((object) this))
        this.ParentTileViewControl.MinimizeditemsOrder.Remove((object) this);
      if (this.ParentTileViewControl.Items.Count <= 0 && this.ParentTileViewControl.TileViewItemOrder != null)
        this.ParentTileViewControl.TileViewItemOrder.Clear();
      this.ParentTileViewControl.SwappedfromMinimized = (TileViewItem) null;
      this.ParentTileViewControl.SwappedfromMaximized = (TileViewItem) null;
    }
    this.KeyDown -= new KeyEventHandler(this.TileViewItem_KeyDown);
  }

  private void ParentTileViewControl_IsMinMaxButtonOnMouseOverOnlyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
  }

  internal void closeButton_Click(object sender, RoutedEventArgs e)
  {
    Button button = sender as Button;
    TileViewItem.CloseEventArgs e1 = new TileViewItem.CloseEventArgs();
    e1.Source = sender;
    if (button == null)
      return;
    switch (this.CloseMode)
    {
      case CloseMode.Hide:
        if (this.ParentTileViewControl.UseNormalState)
        {
          this.OnClosing(e1);
          if (!e1.Cancel)
            this.ParentTileViewControl.SetState(this, TileViewItemState.Hidden);
          this.OnClosed(e1);
          break;
        }
        object obj1 = (object) null;
        this.OnClosing(e1);
        if (!e1.Cancel)
          this.ParentTileViewControl.SetState(this, TileViewItemState.Hidden);
        this.OnClosed(e1);
        int tileViewItemIndex1 = this.TileViewItemIndex;
        TileViewItem tileViewItem1;
        do
        {
          if (tileViewItemIndex1 + 1 <= this.ParentTileViewControl.Items.Count)
            obj1 = this.ParentTileViewControl.Items[tileViewItemIndex1 + 1];
          ++tileViewItemIndex1;
          tileViewItem1 = obj1 == null || !(obj1 is TileViewItem) ? this.ParentTileViewControl.ItemContainerGenerator.ContainerFromItem(obj1) as TileViewItem : (TileViewItem) obj1;
        }
        while (tileViewItem1.TileViewItemState == TileViewItemState.Hidden);
        if (tileViewItem1 == null)
          break;
        this.ParentTileViewControl.SetState(tileViewItem1, TileViewItemState.Maximized);
        break;
      case CloseMode.Delete:
        if (this.ParentTileViewControl.UseNormalState)
        {
          this.ParentTileViewControl.SetState(this, TileViewItemState.Normal);
        }
        else
        {
          object obj2 = (object) null;
          int tileViewItemIndex2 = this.TileViewItemIndex;
          TileViewItem tileViewItem2;
          do
          {
            if (tileViewItemIndex2 + 1 <= this.ParentTileViewControl.Items.Count)
              obj2 = this.ParentTileViewControl.Items[tileViewItemIndex2 + 1];
            ++tileViewItemIndex2;
            tileViewItem2 = obj2 == null || !(obj2 is TileViewItem) ? this.ParentTileViewControl.ItemContainerGenerator.ContainerFromItem(obj2) as TileViewItem : (TileViewItem) obj2;
          }
          while (tileViewItem2 != null && tileViewItem2.TileViewItemState == TileViewItemState.Hidden || obj2 != null && obj2 == this && tileViewItem2.TileViewItemState == TileViewItemState.Maximized);
          if (tileViewItem2 != null)
            this.ParentTileViewControl.SetState(tileViewItem2, TileViewItemState.Maximized);
        }
        if (this.ParentTileViewControl.ItemsSource == null)
        {
          this.OnClosing(e1);
          if (!e1.Cancel)
            this.ParentTileViewControl.Items.Remove((object) this);
          this.OnClosed(e1);
        }
        else
        {
          this.OnClosing(e1);
          if (!e1.Cancel)
          {
            if (this.ParentTileViewControl.ItemsSource is IList itemsSource && this.DataContext != null)
              itemsSource.Remove(this.DataContext);
            this.OnClosed(e1);
          }
        }
        this.Dispose();
        break;
    }
  }

  private void LoadinDesignMode()
  {
    this.SplitRow = 1;
    this.BorderRow = 0;
    if (this.mainGrid == null)
      return;
    this.mainGrid.ColumnDefinitions.Clear();
    if (this.mainGrid.RowDefinitions.Count == 0)
    {
      RowDefinition rowDefinition1 = new RowDefinition();
      RowDefinition rowDefinition2 = new RowDefinition();
      this.mainGrid.RowDefinitions.Add(rowDefinition1);
      this.mainGrid.RowDefinitions.Add(rowDefinition2);
    }
    this.mainGrid.RowDefinitions[0].Height = new GridLength(1.0, GridUnitType.Star);
    this.mainGrid.RowDefinitions[1].Height = new GridLength(0.0, GridUnitType.Pixel);
    this.split.Visibility = Visibility.Collapsed;
  }

  internal void LoadSplitter()
  {
    if (this.ParentTileViewControl == null)
      return;
    if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
    {
      if (this.TileViewItemState == TileViewItemState.Minimized)
      {
        this.BorderRow = 0;
        this.SplitRow = 1;
        this.mainGrid.ColumnDefinitions.Clear();
        if (this.mainGrid.RowDefinitions.Count == 0)
        {
          RowDefinition rowDefinition1 = new RowDefinition();
          RowDefinition rowDefinition2 = new RowDefinition();
          this.mainGrid.RowDefinitions.Add(rowDefinition1);
          this.mainGrid.RowDefinitions.Add(rowDefinition2);
        }
        this.mainGrid.RowDefinitions[0].Height = new GridLength(1.0, GridUnitType.Star);
        if (this.ParentTileViewControl.SplitterVisibility == Visibility.Collapsed)
        {
          this.split.Visibility = Visibility.Collapsed;
          this.mainGrid.RowDefinitions[1].Height = new GridLength(0.0, GridUnitType.Pixel);
        }
        else
        {
          this.split.Visibility = Visibility.Visible;
          this.mainGrid.RowDefinitions[1].Height = new GridLength(this.ParentTileViewControl.SplitterThickness, GridUnitType.Pixel);
        }
      }
      else if (this.TileViewItemState == TileViewItemState.Maximized)
      {
        this.BorderColumn = 0;
        this.SplitColumn = 1;
        this.mainGrid.RowDefinitions.Clear();
        if (this.mainGrid.ColumnDefinitions.Count == 0)
        {
          ColumnDefinition columnDefinition1 = new ColumnDefinition();
          ColumnDefinition columnDefinition2 = new ColumnDefinition();
          this.mainGrid.ColumnDefinitions.Add(columnDefinition1);
          this.mainGrid.ColumnDefinitions.Add(columnDefinition2);
        }
        this.mainGrid.ColumnDefinitions[0].Width = new GridLength(1.0, GridUnitType.Star);
        if (this.ParentTileViewControl.SplitterVisibility == Visibility.Collapsed)
        {
          this.split.Visibility = Visibility.Collapsed;
          this.mainGrid.ColumnDefinitions[1].Width = new GridLength(0.0, GridUnitType.Pixel);
        }
        else
        {
          this.split.Visibility = Visibility.Visible;
          this.mainGrid.ColumnDefinitions[1].Width = new GridLength(this.ParentTileViewControl.SplitterThickness, GridUnitType.Pixel);
        }
      }
      else
      {
        if (this.TileViewItemState != TileViewItemState.Normal)
          return;
        this.BorderColumn = 0;
        this.SplitColumn = 1;
        this.mainGrid.RowDefinitions.Clear();
        if (this.mainGrid.ColumnDefinitions.Count == 0)
        {
          ColumnDefinition columnDefinition3 = new ColumnDefinition();
          ColumnDefinition columnDefinition4 = new ColumnDefinition();
          this.mainGrid.ColumnDefinitions.Add(columnDefinition3);
          this.mainGrid.ColumnDefinitions.Add(columnDefinition4);
        }
        this.mainGrid.ColumnDefinitions[0].Width = new GridLength(1.0, GridUnitType.Star);
        this.mainGrid.ColumnDefinitions[1].Width = new GridLength(0.0, GridUnitType.Pixel);
        if (this.ParentTileViewControl.SplitterVisibility == Visibility.Collapsed)
          this.split.Visibility = Visibility.Collapsed;
        else
          this.split.Visibility = Visibility.Visible;
      }
    }
    else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
    {
      if (this.TileViewItemState == TileViewItemState.Minimized)
      {
        this.BorderRow = 0;
        this.SplitRow = 1;
        this.mainGrid.ColumnDefinitions.Clear();
        if (this.mainGrid.RowDefinitions.Count == 0)
        {
          RowDefinition rowDefinition3 = new RowDefinition();
          RowDefinition rowDefinition4 = new RowDefinition();
          this.mainGrid.RowDefinitions.Add(rowDefinition3);
          this.mainGrid.RowDefinitions.Add(rowDefinition4);
        }
        this.mainGrid.RowDefinitions[0].Height = new GridLength(1.0, GridUnitType.Star);
        if (this.ParentTileViewControl.SplitterVisibility == Visibility.Collapsed)
        {
          this.split.Visibility = Visibility.Collapsed;
          this.mainGrid.RowDefinitions[1].Height = new GridLength(0.0, GridUnitType.Pixel);
        }
        else
        {
          this.split.Visibility = Visibility.Visible;
          this.mainGrid.RowDefinitions[1].Height = new GridLength(this.ParentTileViewControl.SplitterThickness, GridUnitType.Pixel);
        }
      }
      else if (this.TileViewItemState == TileViewItemState.Maximized)
      {
        this.SplitColumn = 0;
        this.BorderColumn = 1;
        this.mainGrid.RowDefinitions.Clear();
        if (this.mainGrid.ColumnDefinitions.Count == 0)
        {
          ColumnDefinition columnDefinition5 = new ColumnDefinition();
          ColumnDefinition columnDefinition6 = new ColumnDefinition();
          this.mainGrid.ColumnDefinitions.Add(columnDefinition5);
          this.mainGrid.ColumnDefinitions.Add(columnDefinition6);
        }
        this.mainGrid.ColumnDefinitions[1].Width = new GridLength(1.0, GridUnitType.Star);
        if (this.ParentTileViewControl.SplitterVisibility == Visibility.Collapsed)
        {
          this.split.Visibility = Visibility.Collapsed;
          this.mainGrid.ColumnDefinitions[0].Width = new GridLength(0.0, GridUnitType.Pixel);
        }
        else
        {
          this.split.Visibility = Visibility.Visible;
          this.mainGrid.ColumnDefinitions[0].Width = new GridLength(this.ParentTileViewControl.SplitterThickness, GridUnitType.Pixel);
        }
      }
      else
      {
        if (this.TileViewItemState != TileViewItemState.Normal)
          return;
        this.SplitColumn = 0;
        this.BorderColumn = 1;
        this.mainGrid.RowDefinitions.Clear();
        if (this.mainGrid.ColumnDefinitions.Count == 0)
        {
          ColumnDefinition columnDefinition7 = new ColumnDefinition();
          ColumnDefinition columnDefinition8 = new ColumnDefinition();
          this.mainGrid.ColumnDefinitions.Add(columnDefinition7);
          this.mainGrid.ColumnDefinitions.Add(columnDefinition8);
        }
        this.mainGrid.ColumnDefinitions[0].Width = new GridLength(0.0, GridUnitType.Pixel);
        this.mainGrid.ColumnDefinitions[1].Width = new GridLength(1.0, GridUnitType.Star);
        if (this.ParentTileViewControl.SplitterVisibility == Visibility.Collapsed)
          this.split.Visibility = Visibility.Collapsed;
        else
          this.split.Visibility = Visibility.Visible;
      }
    }
    else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Top)
    {
      if (this.TileViewItemState == TileViewItemState.Normal)
      {
        this.SplitRow = 0;
        this.BorderRow = 1;
        this.mainGrid.ColumnDefinitions.Clear();
        if (this.mainGrid.RowDefinitions.Count == 0)
        {
          RowDefinition rowDefinition5 = new RowDefinition();
          RowDefinition rowDefinition6 = new RowDefinition();
          this.mainGrid.RowDefinitions.Add(rowDefinition5);
          this.mainGrid.RowDefinitions.Add(rowDefinition6);
        }
        this.mainGrid.RowDefinitions[0].Height = new GridLength(0.0, GridUnitType.Pixel);
        this.mainGrid.RowDefinitions[1].Height = new GridLength(1.0, GridUnitType.Star);
        if (this.ParentTileViewControl.SplitterVisibility == Visibility.Collapsed)
          this.split.Visibility = Visibility.Collapsed;
        else
          this.split.Visibility = Visibility.Visible;
      }
      else if (this.TileViewItemState == TileViewItemState.Maximized)
      {
        this.SplitRow = 0;
        this.BorderRow = 1;
        this.mainGrid.ColumnDefinitions.Clear();
        if (this.mainGrid.RowDefinitions.Count == 0)
        {
          RowDefinition rowDefinition7 = new RowDefinition();
          RowDefinition rowDefinition8 = new RowDefinition();
          this.mainGrid.RowDefinitions.Add(rowDefinition7);
          this.mainGrid.RowDefinitions.Add(rowDefinition8);
        }
        this.mainGrid.RowDefinitions[1].Height = new GridLength(1.0, GridUnitType.Star);
        if (this.ParentTileViewControl.SplitterVisibility == Visibility.Collapsed)
        {
          this.split.Visibility = Visibility.Collapsed;
          this.mainGrid.RowDefinitions[0].Height = new GridLength(0.0, GridUnitType.Pixel);
        }
        else
        {
          this.split.Visibility = Visibility.Visible;
          this.mainGrid.RowDefinitions[0].Height = new GridLength(this.ParentTileViewControl.SplitterThickness, GridUnitType.Pixel);
        }
      }
      else
      {
        if (this.TileViewItemState != TileViewItemState.Minimized)
          return;
        this.SplitColumn = 1;
        this.BorderColumn = 0;
        this.mainGrid.RowDefinitions.Clear();
        if (this.mainGrid.ColumnDefinitions.Count == 0)
        {
          ColumnDefinition columnDefinition9 = new ColumnDefinition();
          ColumnDefinition columnDefinition10 = new ColumnDefinition();
          this.mainGrid.ColumnDefinitions.Add(columnDefinition9);
          this.mainGrid.ColumnDefinitions.Add(columnDefinition10);
        }
        this.mainGrid.ColumnDefinitions[0].Width = new GridLength(1.0, GridUnitType.Star);
        if (this.ParentTileViewControl.SplitterVisibility == Visibility.Collapsed)
        {
          this.split.Visibility = Visibility.Collapsed;
          this.mainGrid.ColumnDefinitions[1].Width = new GridLength(0.0, GridUnitType.Pixel);
        }
        else
        {
          this.split.Visibility = Visibility.Visible;
          this.mainGrid.ColumnDefinitions[1].Width = new GridLength(this.ParentTileViewControl.SplitterThickness, GridUnitType.Pixel);
        }
      }
    }
    else
    {
      if (this.ParentTileViewControl.MinimizedItemsOrientation != MinimizedItemsOrientation.Bottom)
        return;
      if (this.TileViewItemState == TileViewItemState.Normal)
      {
        this.SplitRow = 1;
        this.BorderRow = 0;
        this.mainGrid.ColumnDefinitions.Clear();
        if (this.mainGrid.RowDefinitions.Count == 0)
        {
          RowDefinition rowDefinition9 = new RowDefinition();
          RowDefinition rowDefinition10 = new RowDefinition();
          this.mainGrid.RowDefinitions.Add(rowDefinition9);
          this.mainGrid.RowDefinitions.Add(rowDefinition10);
        }
        this.mainGrid.RowDefinitions[0].Height = new GridLength(1.0, GridUnitType.Star);
        this.mainGrid.RowDefinitions[1].Height = new GridLength(0.0, GridUnitType.Pixel);
        if (this.ParentTileViewControl.SplitterVisibility == Visibility.Collapsed)
          this.split.Visibility = Visibility.Collapsed;
        else
          this.split.Visibility = Visibility.Visible;
      }
      else if (this.TileViewItemState == TileViewItemState.Maximized)
      {
        this.SplitRow = 1;
        this.BorderRow = 0;
        this.mainGrid.ColumnDefinitions.Clear();
        if (this.mainGrid.RowDefinitions.Count == 0)
        {
          RowDefinition rowDefinition11 = new RowDefinition();
          RowDefinition rowDefinition12 = new RowDefinition();
          this.mainGrid.RowDefinitions.Add(rowDefinition11);
          this.mainGrid.RowDefinitions.Add(rowDefinition12);
        }
        this.mainGrid.RowDefinitions[0].Height = new GridLength(1.0, GridUnitType.Star);
        if (this.ParentTileViewControl.SplitterVisibility == Visibility.Collapsed)
        {
          this.split.Visibility = Visibility.Collapsed;
          this.mainGrid.RowDefinitions[1].Height = new GridLength(0.0, GridUnitType.Pixel);
        }
        else
        {
          this.split.Visibility = Visibility.Visible;
          this.mainGrid.RowDefinitions[1].Height = new GridLength(this.ParentTileViewControl.SplitterThickness, GridUnitType.Pixel);
        }
      }
      else
      {
        if (this.TileViewItemState != TileViewItemState.Minimized)
          return;
        this.SplitColumn = 1;
        this.BorderColumn = 0;
        this.mainGrid.RowDefinitions.Clear();
        if (this.mainGrid.ColumnDefinitions.Count == 0)
        {
          ColumnDefinition columnDefinition11 = new ColumnDefinition();
          ColumnDefinition columnDefinition12 = new ColumnDefinition();
          this.mainGrid.ColumnDefinitions.Add(columnDefinition11);
          this.mainGrid.ColumnDefinitions.Add(columnDefinition12);
        }
        this.mainGrid.ColumnDefinitions[0].Width = new GridLength(1.0, GridUnitType.Star);
        if (this.ParentTileViewControl.SplitterVisibility == Visibility.Collapsed)
        {
          this.split.Visibility = Visibility.Collapsed;
          this.mainGrid.ColumnDefinitions[1].Width = new GridLength(0.0, GridUnitType.Pixel);
        }
        else
        {
          this.split.Visibility = Visibility.Visible;
          this.mainGrid.ColumnDefinitions[1].Width = new GridLength(this.ParentTileViewControl.SplitterThickness, GridUnitType.Pixel);
        }
      }
    }
  }

  internal void split_MouseEnter(object sender, MouseEventArgs e)
  {
    if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
    {
      if (this.TileViewItemState == TileViewItemState.Maximized)
      {
        this.Cursor = Cursors.SizeWE;
      }
      else
      {
        if (this.TileViewItemState != TileViewItemState.Minimized)
          return;
        this.Cursor = Cursors.SizeNS;
      }
    }
    else if (this.TileViewItemState == TileViewItemState.Maximized)
    {
      this.Cursor = Cursors.SizeNS;
    }
    else
    {
      if (this.TileViewItemState != TileViewItemState.Minimized)
        return;
      this.Cursor = Cursors.SizeWE;
    }
  }

  internal void split_MouseLeave(object sender, MouseEventArgs e) => this.Cursor = Cursors.Arrow;

  protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
  {
  }

  internal void split_MouseMove(object sender, MouseEventArgs e)
  {
    this.Splitflag = e.LeftButton == MouseButtonState.Pressed && !this.ParentTileViewControl.IsVirtualizing;
    if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
    {
      if (this.TileViewItemState == TileViewItemState.Maximized)
        this.Cursor = Cursors.SizeWE;
      else if (this.TileViewItemState == TileViewItemState.Minimized)
        this.Cursor = Cursors.SizeNS;
    }
    else if (this.TileViewItemState == TileViewItemState.Maximized)
      this.Cursor = Cursors.SizeNS;
    else if (this.TileViewItemState == TileViewItemState.Minimized)
      this.Cursor = Cursors.SizeWE;
    if (!this.Splitflag)
      return;
    Point position1 = e.GetPosition((IInputElement) this.ParentTileViewControl);
    Point position2 = e.GetPosition((IInputElement) this);
    if (this.TileViewItemState == TileViewItemState.Maximized)
    {
      GridSplitter gridSplitter = new GridSplitter();
      gridSplitter.Opacity = 0.0;
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
      {
        gridSplitter.Height = this.ActualHeight;
        gridSplitter.Width = this.ParentTileViewControl.SplitterThickness;
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
      {
        gridSplitter.Width = this.ActualWidth;
        gridSplitter.Height = this.ParentTileViewControl.SplitterThickness;
      }
      this.Splitpopup.Child = (UIElement) gridSplitter;
      this.Splitpopup.IsOpen = true;
      double num1 = this.ParentTileViewControl.ActualWidth - (this.ParentTileViewControl.LeftMargin + this.ParentTileViewControl.RightMargin);
      double actualHeight = this.ParentTileViewControl.ActualHeight;
      double topMargin = this.ParentTileViewControl.TopMargin;
      double bottomMargin = this.ParentTileViewControl.BottomMargin;
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
      {
        double num2 = num1 * 15.0 / 100.0;
        double num3 = num1 - num1 * 15.0 / 100.0;
        this.Splitpopup.HorizontalOffset = position1.X <= num2 || position1.X >= num3 ? (position1.X <= this.startpoint.X ? num2 : num3) : position1.X - this.startpoint.X;
        this.Splitpopup.VerticalOffset = -this.ActualHeight;
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
      {
        double num4 = num1 * 15.0 / 100.0;
        double num5 = num1 - num1 * 20.0 / 100.0;
        if (position1.X >= num4 && position1.X <= num5)
          this.Splitpopup.HorizontalOffset = position2.X - this.ActualWidth;
        this.Splitpopup.VerticalOffset = -this.ActualHeight;
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Top)
      {
        double headerHeight = this.HeaderHeight;
        double num6 = this.ParentTileViewControl.ActualHeight - (this.ParentTileViewControl.TopMargin + this.ParentTileViewControl.BottomMargin) - this.HeaderHeight;
        this.Splitpopup.HorizontalOffset = -this.ActualWidth;
        if (position1.Y < headerHeight || position1.Y > num6)
          return;
        this.Splitpopup.VerticalOffset = position2.Y - this.ActualHeight;
      }
      else
      {
        if (this.ParentTileViewControl.MinimizedItemsOrientation != MinimizedItemsOrientation.Bottom)
          return;
        double top1 = Canvas.GetTop((UIElement) this) + this.HeaderHeight;
        double top2 = Canvas.GetTop((UIElement) this) + (this.ParentTileViewControl.ActualHeight - (this.ParentTileViewControl.TopMargin + this.ParentTileViewControl.BottomMargin)) - this.HeaderHeight - this.Margin.Top - this.Margin.Bottom;
        this.Splitpopup.HorizontalOffset = -this.ActualWidth;
        if (position1.Y > top1 && position1.Y < top2)
        {
          this.Splitpopup.VerticalOffset = position2.Y - this.ActualHeight;
          this.Splitpopup.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
          this.HeaderPoint = position1;
        }
        else if (position1.Y < top1)
        {
          this.Splitpopup.Margin = new Thickness(0.0, top1, 0.0, 0.0);
          this.HeaderPoint = position1;
        }
        else
        {
          if (position1.Y <= top2)
            return;
          this.Splitpopup.Margin = new Thickness(0.0, top2, 0.0, 0.0);
          this.HeaderPoint = position1;
        }
      }
    }
    else
    {
      if (this.TileViewItemState != TileViewItemState.Minimized)
        return;
      GridSplitter gridSplitter = new GridSplitter();
      gridSplitter.Opacity = 0.0;
      double headerHeight1 = this.HeaderHeight;
      double headerHeight2 = this.HeaderHeight;
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
      {
        gridSplitter.Width = this.ActualWidth;
        gridSplitter.Height = this.ParentTileViewControl.SplitterThickness;
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
      {
        gridSplitter.Height = this.ActualHeight;
        gridSplitter.Width = this.ParentTileViewControl.SplitterThickness;
      }
      this.Splitpopup.Child = (UIElement) gridSplitter;
      this.Splitpopup.IsOpen = true;
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
      {
        if (this.Nextitem == null)
          return;
        double num7 = Canvas.GetTop((UIElement) this.Currentitem) + this.Currentitem.HeaderHeight;
        double num8 = Canvas.GetTop((UIElement) this.Nextitem) + this.Nextitem.ActualHeight - this.Nextitem.HeaderHeight - this.Nextitem.Margin.Top - this.Currentitem.Margin.Bottom;
        this.Splitpopup.HorizontalOffset = 0.0;
        if (position1.Y > num7 && position1.Y < num8)
        {
          this.Splitpopup.VerticalOffset = position2.Y - this.ActualHeight;
          this.HeaderPoint = position1;
        }
        this.Splitpopup.HorizontalOffset = -this.ActualWidth;
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
      {
        if (this.Nextitem == null)
          return;
        double num9 = Canvas.GetTop((UIElement) this.Currentitem) + this.Currentitem.HeaderHeight;
        double num10 = Canvas.GetTop((UIElement) this.Nextitem) + this.Nextitem.ActualHeight - this.Nextitem.HeaderHeight - this.Nextitem.Margin.Top - this.Currentitem.Margin.Bottom;
        this.Splitpopup.HorizontalOffset = 0.0;
        if (position1.Y > num9 && position1.Y < num10)
        {
          this.Splitpopup.VerticalOffset = position2.Y - this.ActualHeight;
          this.HeaderPoint = position1;
        }
        this.Splitpopup.HorizontalOffset = -this.ActualWidth;
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Top)
      {
        if (this.Nextitem == null)
          return;
        double num11 = Canvas.GetLeft((UIElement) this.Currentitem) + this.Currentitem.HeaderHeight;
        double num12 = Canvas.GetLeft((UIElement) this.Nextitem) + this.Nextitem.ActualWidth - this.Nextitem.HeaderHeight - this.Nextitem.Margin.Left - this.Currentitem.Margin.Right;
        if (position1.X > num11 && position1.X < num12)
        {
          this.Splitpopup.HorizontalOffset = position2.X - this.ActualWidth;
          this.HeaderPoint = position1;
        }
        this.Splitpopup.VerticalOffset = -this.ActualHeight;
      }
      else
      {
        if (this.ParentTileViewControl.MinimizedItemsOrientation != MinimizedItemsOrientation.Bottom || this.Nextitem == null)
          return;
        double num13 = Canvas.GetLeft((UIElement) this.Currentitem) + this.Currentitem.HeaderHeight;
        double num14 = Canvas.GetLeft((UIElement) this.Nextitem) + this.Nextitem.ActualWidth - this.Nextitem.HeaderHeight - this.Nextitem.Margin.Left - this.Currentitem.Margin.Right;
        if (position1.X > num13 && position1.X < num14)
        {
          this.Splitpopup.HorizontalOffset = position2.X - this.ActualWidth;
          this.HeaderPoint = position1;
        }
        this.Splitpopup.VerticalOffset = -this.ActualHeight;
      }
    }
  }

  internal void split_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.endpoint = new Point(0.0, 0.0);
    this.split.ReleaseMouseCapture();
    this.Splitpopup.IsOpen = false;
    this.endpoint = e.GetPosition((IInputElement) this);
    double num = 0.0;
    if (this.TileViewItemState == TileViewItemState.Maximized && this.Splitflag)
    {
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
        this.ParentTileViewControl.MinimizedItemsPercentage = (this.ParentTileViewControl.minimizedColumnWidth + this.startpoint.X - this.endpoint.X) * 100.0 / (this.ParentTileViewControl.ActualWidth - (this.ParentTileViewControl.LeftMargin + this.ParentTileViewControl.RightMargin));
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
        this.ParentTileViewControl.MinimizedItemsPercentage = (this.ParentTileViewControl.minimizedColumnWidth - (this.startpoint.X - this.endpoint.X)) * 100.0 / (this.ParentTileViewControl.ActualWidth - (this.ParentTileViewControl.LeftMargin + this.ParentTileViewControl.RightMargin));
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Top)
        this.ParentTileViewControl.MinimizedItemsPercentage = (this.ParentTileViewControl.minimizedRowHeight - (this.startpoint.Y - this.endpoint.Y)) * 100.0 / (this.ParentTileViewControl.ActualHeight - (this.ParentTileViewControl.TopMargin + this.ParentTileViewControl.BottomMargin));
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
      {
        if (this.Splitpopup.Margin.Top == 0.0)
          this.ParentTileViewControl.MinimizedItemsPercentage -= this.Splitpopup.VerticalOffset * 100.0 / this.ParentTileViewControl.ActualHeight;
        else
          this.ParentTileViewControl.MinimizedItemsPercentage += (this.startpoint.Y - this.Splitpopup.Margin.Top) * 100.0 / (this.ParentTileViewControl.ActualHeight - (this.ParentTileViewControl.TopMargin + this.ParentTileViewControl.BottomMargin));
        num = this.ParentTileViewControl.minimizedRowHeight - (this.startpoint.Y - (this.Splitpopup.VerticalOffset + this.ActualHeight));
      }
    }
    else if (this.TileViewItemState == TileViewItemState.Minimized)
    {
      this.ParentTileViewControl.IsSplitterUsedinMinimizedState = true;
      e.GetPosition((IInputElement) this);
      e.GetPosition((IInputElement) this.ParentTileViewControl);
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
      {
        if (this.Nextitem != null && this.HeaderPoint.Y > 0.0)
        {
          double actualHeight = this.ActualHeight;
          this.ParentTileViewControl.draggedHeight = this.startpoint.Y - this.HeaderPoint.Y;
          this.OnMinimizedHeight = new GridLength(this.ActualHeight - this.ParentTileViewControl.draggedHeight, GridUnitType.Pixel);
          this.ParentTileViewControl.marginFlag = false;
          this.ParentTileViewControl.DraggedItem = this;
          this.ParentTileViewControl.GetTileViewItemsSizesforSplitter();
          this.ParentTileViewControl.AnimateTileViewLayoutforSplitter();
        }
      }
      else if ((this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom) && this.Nextitem != null && this.HeaderPoint.X > 0.0)
      {
        double actualWidth = this.ActualWidth;
        this.ParentTileViewControl.draggedWidth = this.startpoint.X - this.HeaderPoint.X;
        this.OnMinimizedWidth = new GridLength(this.ActualWidth - this.ParentTileViewControl.draggedWidth, GridUnitType.Pixel);
        this.ParentTileViewControl.marginFlag = false;
        this.ParentTileViewControl.DraggedItem = this;
        this.ParentTileViewControl.GetTileViewItemsSizesforSplitter();
        this.ParentTileViewControl.AnimateTileViewLayoutforSplitter();
      }
    }
    this.Splitflag = false;
    this.Splitpopup.IsOpen = false;
    if (this.gsp == null)
      return;
    this.gsp.Visibility = Visibility.Collapsed;
  }

  internal void split_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.split.CaptureMouse();
    if (this.TileViewItemState == TileViewItemState.Maximized)
    {
      this.gsp = new GridSplitter();
      this.gsp.Opacity = 0.0;
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
      {
        this.gsp.Height = this.ActualHeight;
        this.gsp.Width = this.ParentTileViewControl.SplitterThickness;
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
      {
        this.gsp.Width = this.ActualWidth;
        this.gsp.Height = this.ParentTileViewControl.SplitterThickness;
      }
      this.Splitpopup.Child = (UIElement) this.gsp;
      this.Splitpopup.IsOpen = true;
      Point position = e.GetPosition((IInputElement) this);
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
      {
        this.Splitpopup.HorizontalOffset = 0.0;
        this.Splitpopup.VerticalOffset = -this.ActualHeight;
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
      {
        this.Splitpopup.HorizontalOffset = -this.ActualWidth;
        this.Splitpopup.VerticalOffset = -this.ActualHeight;
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Top)
      {
        this.Splitpopup.HorizontalOffset = 0.0;
        this.Splitpopup.VerticalOffset = position.Y - this.ActualHeight;
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
      {
        this.Splitpopup.HorizontalOffset = 0.0;
        this.Splitpopup.VerticalOffset = position.Y - this.ActualHeight;
      }
      this.startpoint = new Point(0.0, 0.0);
      this.startpoint = position;
    }
    else
    {
      if (this.TileViewItemState != TileViewItemState.Minimized)
        return;
      this.gsp = new GridSplitter();
      this.gsp.Opacity = 0.0;
      this.gsp.BorderThickness = new Thickness(this.ParentTileViewControl.SplitterThickness);
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
      {
        this.gsp.Width = this.ActualWidth;
        this.gsp.Height = this.ParentTileViewControl.SplitterThickness;
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Top || this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
      {
        this.gsp.Height = this.ActualHeight;
        this.gsp.Width = this.ParentTileViewControl.SplitterThickness;
      }
      this.Splitpopup.Child = (UIElement) this.gsp;
      this.Splitpopup.IsOpen = true;
      Point position = e.GetPosition((IInputElement) this);
      if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Right)
      {
        this.Splitpopup.HorizontalOffset = 0.0;
        bool flag = false;
        for (int key = 0; key < this.ParentTileViewControl.TileViewItemOrder.Count; ++key)
        {
          if (this.ParentTileViewControl.TileViewItemOrder.ContainsKey(key) && this.ParentTileViewControl.TileViewItemOrder[key] != this.ParentTileViewControl.maximizedItem)
          {
            if (this.ParentTileViewControl.TileViewItemOrder[key] == this)
            {
              this.Currentitem = this.ParentTileViewControl.TileViewItemOrder[key];
              flag = true;
            }
            else if (flag)
            {
              this.Nextitem = this.ParentTileViewControl.TileViewItemOrder[key];
              break;
            }
          }
        }
        this.Splitpopup.VerticalOffset = position.Y - this.ActualHeight;
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Left)
      {
        this.Splitpopup.HorizontalOffset = 0.0;
        this.Splitpopup.VerticalOffset = position.Y - this.ActualHeight;
        bool flag = false;
        for (int key = 0; key < this.ParentTileViewControl.TileViewItemOrder.Count; ++key)
        {
          if (this.ParentTileViewControl.TileViewItemOrder.ContainsKey(key) && this.ParentTileViewControl.TileViewItemOrder[key] != this.ParentTileViewControl.maximizedItem)
          {
            if (this.ParentTileViewControl.TileViewItemOrder[key] == this)
            {
              this.Currentitem = this.ParentTileViewControl.TileViewItemOrder[key];
              flag = true;
            }
            else if (flag)
            {
              this.Nextitem = this.ParentTileViewControl.TileViewItemOrder[key];
              break;
            }
          }
        }
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Top)
      {
        this.Splitpopup.HorizontalOffset = position.X - this.ActualWidth;
        this.Splitpopup.VerticalOffset = -this.ActualHeight;
        bool flag = false;
        for (int key = 0; key < this.ParentTileViewControl.TileViewItemOrder.Count; ++key)
        {
          if (this.ParentTileViewControl.TileViewItemOrder.ContainsKey(key) && this.ParentTileViewControl.TileViewItemOrder[key] != this.ParentTileViewControl.maximizedItem)
          {
            if (this.ParentTileViewControl.TileViewItemOrder[key] == this)
            {
              this.Currentitem = this.ParentTileViewControl.TileViewItemOrder[key];
              flag = true;
            }
            else if (flag)
            {
              this.Nextitem = this.ParentTileViewControl.TileViewItemOrder[key];
              break;
            }
          }
        }
      }
      else if (this.ParentTileViewControl.MinimizedItemsOrientation == MinimizedItemsOrientation.Bottom)
      {
        this.Splitpopup.HorizontalOffset = position.X - this.ActualWidth;
        this.Splitpopup.VerticalOffset = -this.ActualHeight;
        bool flag = false;
        for (int key = 0; key < this.ParentTileViewControl.TileViewItemOrder.Count; ++key)
        {
          if (this.ParentTileViewControl.TileViewItemOrder.ContainsKey(key) && this.ParentTileViewControl.TileViewItemOrder[key] != this.ParentTileViewControl.maximizedItem)
          {
            if (this.ParentTileViewControl.TileViewItemOrder[key] == this)
            {
              this.Currentitem = this.ParentTileViewControl.TileViewItemOrder[key];
              flag = true;
            }
            else if (flag)
            {
              this.Nextitem = this.ParentTileViewControl.TileViewItemOrder[key];
              break;
            }
          }
        }
      }
      this.startpoint = new Point(0.0, 0.0);
      this.startpoint = position;
      this.startpoint = e.GetPosition((IInputElement) this.ParentTileViewControl);
    }
  }

  internal void HeaderPart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (this.ParentTileViewControl.ClickHeaderToMaximize && this.minMaxButton != null && this.minMaxButton.IsEnabled)
    {
      Border border = sender as Border;
      this.IsSelected = true;
      if (border != null)
      {
        if (this.TileViewItemState == TileViewItemState.Maximized)
        {
          if (this.ParentTileViewControl.UseNormalState)
            this.ParentTileViewControl.SetState(this, TileViewItemState.Normal);
        }
        else
          this.ParentTileViewControl.SetState(this, TileViewItemState.Maximized);
      }
      this.ParentTileViewControl.UpdateTileViewLayout(true);
    }
    if (this.OnMinimizedWidth.Value == 0.0 && this.OnMinimizedHeight.Value == 0.0)
      return;
    this.ParentTileViewControl.isheaderClicked = true;
    this.ParentTileViewControl.UpdateTileViewLayout();
  }

  internal void MinMaxButton_Click(object sender, RoutedEventArgs e)
  {
    ToggleButton toggleButton = sender as ToggleButton;
    this.IsSelected = true;
    if (toggleButton == null)
      return;
    if (toggleButton.IsChecked.HasValue)
    {
      if (toggleButton.IsChecked.Value)
      {
        if (this.TileViewItemState == TileViewItemState.Maximized)
        {
          if (this.ParentTileViewControl.UseNormalState)
            this.ParentTileViewControl.SetState(this, TileViewItemState.Normal);
          else
            toggleButton.IsChecked = new bool?(true);
        }
        else
          this.ParentTileViewControl.SetState(this, TileViewItemState.Maximized);
      }
      else if (this.TileViewItemState == TileViewItemState.Normal || this.TileViewItemState == TileViewItemState.Minimized)
        this.ParentTileViewControl.SetState(this, TileViewItemState.Maximized);
      else
        this.ParentTileViewControl.SetState(this, TileViewItemState.Minimized);
    }
    else if (this.TileViewItemState == TileViewItemState.Minimized)
      this.ParentTileViewControl.SetState(this, TileViewItemState.Maximized);
    else if (this.ParentTileViewControl.UseNormalState)
      this.ParentTileViewControl.SetState(this, TileViewItemState.Normal);
    else
      toggleButton.IsChecked = new bool?(true);
    this.ParentTileViewControl.UpdateTileViewLayout(true);
  }

  internal void Onloadingitems()
  {
    if (this.TileViewItemState != TileViewItemState.Maximized)
      return;
    this.TileViewItemMaximize();
  }

  public override void UpdateCoordinate(Point Pt)
  {
    Canvas.SetLeft((UIElement) this, Pt.X);
    Canvas.SetTop((UIElement) this, Pt.Y);
  }

  internal virtual void TileViewItemMaximize()
  {
    Panel.SetZIndex((UIElement) this, TileViewItemBase.CurrentZIndex++);
    if (this.CardMaximized != null)
    {
      if (this.ParentTileViewControl != null)
      {
        this.ParentTileViewControl.SwappedfromMinimized = this;
        this.ParentTileViewControl.SwappedfromMaximized = this.ParentTileViewControl.maximizedItem;
        if (this.ParentTileViewControl.SwappedfromMaximized != null)
          this.ParentTileViewControl.IsSwapped = true;
        else
          this.ParentTileViewControl.IsSwapped = false;
      }
      this.CardMaximized((object) this, EventArgs.Empty);
    }
    this.MinMaxButtonToolTip = "Restore";
    this.LoadSplitter();
  }

  internal virtual void RestoreTileViewItems()
  {
    TileViewEventArgs args = new TileViewEventArgs();
    if (this.CardNormal != null)
      this.CardNormal((object) this, EventArgs.Empty);
    if (this.StateChanged != null)
      this.StateChanged((object) this, args);
    this.MinMaxButtonToolTip = "Maximize";
  }

  internal void TileViewItemsMinimizeMethod(MinimizedItemsOrientation mpos)
  {
    if (this.CardMinimized != null)
      this.CardMinimized((object) this, EventArgs.Empty);
    if (this.ParentTileViewControl != null)
      this.ParentTileViewControl.SetState(this, TileViewItemState.Minimized);
    this.MinMaxButtonToolTip = "Maximize";
  }

  protected override void OnGotFocus(RoutedEventArgs e) => base.OnGotFocus(e);

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    TileViewItem tileViewItem1 = this.ParentTileViewControl.ItemsSource != null ? this.ParentTileViewControl.ItemContainerGenerator.ContainerFromItem(this.ParentTileViewControl.SelectedItem) as TileViewItem : this.ParentTileViewControl.SelectedItem as TileViewItem;
    this.ind = this.ParentTileViewControl.Items.IndexOf((object) tileViewItem1);
    if (this.ind < 0)
      this.ind = this.ParentTileViewControl.ItemContainerGenerator.IndexFromContainer((DependencyObject) this);
    this.itemscount = this.ParentTileViewControl.Items.Count;
    if (VisualUtils.FindAncestor(e.OriginalSource as Visual, typeof (TileViewItemCloseButton)) is Button)
    {
      if (this.IsSelected)
      {
        this.IsSelected = false;
        for (int index1 = this.ind + 1; index1 <= this.ParentTileViewControl.Items.Count; ++index1)
        {
          if (this.ParentTileViewControl.ItemContainerGenerator.ContainerFromIndex(index1) is TileViewItem && (this.ParentTileViewControl.ItemContainerGenerator.ContainerFromIndex(index1) as TileViewItem).Visibility == Visibility.Visible)
          {
            this.ParentTileViewControl.SelectedIndex = index1;
            break;
          }
          if (index1 >= this.itemscount - 1)
          {
            for (int index2 = 0; index2 <= this.ind; ++index2)
            {
              if (this.ParentTileViewControl.ItemContainerGenerator.ContainerFromIndex(index2) is TileViewItem && (this.ParentTileViewControl.ItemContainerGenerator.ContainerFromIndex(index2) as TileViewItem).Visibility == Visibility.Visible)
              {
                (this.ParentTileViewControl.ItemContainerGenerator.ContainerFromIndex(index2) as TileViewItem).IsSelected = true;
                this.ParentTileViewControl.SelectedIndex = index2;
                break;
              }
            }
          }
        }
      }
      else
      {
        foreach (TileViewItem tileViewItem2 in (Collection<TileViewItem>) this.ParentTileViewControl.tileViewItems)
          tileViewItem2.IsSelected = tileViewItem2 == tileViewItem1;
      }
    }
    else
    {
      this.IsSelected = true;
      base.OnPreviewMouseLeftButtonDown(e);
      object originalSource = e.OriginalSource;
      foreach (TileViewItem tileViewItem3 in (Collection<TileViewItem>) this.ParentTileViewControl.tileViewItems)
        tileViewItem3.IsSelected = tileViewItem3 == this;
    }
  }

  private static void OnEnumLoad(DependencyObject obj, DependencyPropertyChangedEventArgs e)
  {
    ((TileViewItem) obj).OnEnumLoad(e);
  }

  protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    base.OnPropertyChanged(e);
    if (e.Property != UIElement.VisibilityProperty || this.ParentTileViewControl == null)
      return;
    if (this.Visibility == Visibility.Collapsed)
    {
      this.ParentTileViewControl.UpdateCurrentOrderFromTileItems();
      this.ParentTileViewControl.SetRowsAndColumns(this.ParentTileViewControl.GetTileViewItemOrder());
      this.ParentTileViewControl.UpdateTileViewLayout(true);
    }
    else
    {
      this.ParentTileViewControl.GetTileViewItems();
      this.ParentTileViewControl.SetRowsAndColumns(this.ParentTileViewControl.GetTileViewItemOrder());
      this.ParentTileViewControl.UpdateTileViewLayout(true);
    }
  }

  internal void TileViewItemOnHiddenState(TileViewItem item)
  {
    if (item.TileViewItemState != TileViewItemState.Hidden || this.ParentTileViewControl == null)
      return;
    this.ParentTileViewControl.initialTileViewItems.Remove(item);
    this.ParentTileViewControl.tileViewItems.Remove(item);
    item.Visibility = Visibility.Collapsed;
    if (this.ParentTileViewControl.tileViewItems.Count <= 0)
      return;
    this.ParentTileViewControl.SetRowsAndColumns(this.ParentTileViewControl.GetTileViewItemOrder());
    this.ParentTileViewControl.UpdateTileViewLayout(true);
  }

  protected virtual void OnEnumLoad(DependencyPropertyChangedEventArgs e)
  {
    TileViewItemState newValue = (TileViewItemState) e.NewValue;
    TileViewItem tileViewItem = this;
    TileViewCancelEventArgs e1 = new TileViewCancelEventArgs();
    this.OnStateChanging(e1);
    TileViewEventArgs e2 = new TileViewEventArgs();
    e2.OldState = (TileViewItemState) e.OldValue;
    e2.NewState = (TileViewItemState) e.NewValue;
    if (!e1.Cancel && !this.disablePropertyChangedNotify)
    {
      switch (newValue)
      {
        case TileViewItemState.Normal:
          if (this.ParentTileViewControl.UseNormalState)
          {
            this.OnStateChanged(e2);
            if (e2.OldState == TileViewItemState.Hidden && tileViewItem.CloseMode == CloseMode.Hide)
              this.HiddenStateChanged(tileViewItem);
            this.RestoreTileViewItems();
            break;
          }
          break;
        case TileViewItemState.Maximized:
          this.OnStateChanged(e2);
          if (e2.OldState == TileViewItemState.Hidden && tileViewItem.CloseMode == CloseMode.Hide)
            this.HiddenStateChanged(tileViewItem);
          this.TileViewItemMaximize();
          break;
        case TileViewItemState.Minimized:
          this.OnStateChanged(e2);
          break;
        case TileViewItemState.Hidden:
          this.OnStateChanged(e2);
          this.TileViewItemOnHiddenState(tileViewItem);
          break;
      }
      if (this.ParentTileViewControl != null)
        this.ParentTileViewControl.ApplyTileViewContent(this);
    }
    if (this.ParentTileViewControl == null || this.ParentTileViewControl.SplitterVisibility != Visibility.Visible)
      return;
    this.LoadSplitter();
  }

  internal void HiddenStateChanged(TileViewItem item)
  {
    item.Visibility = Visibility.Visible;
    this.ParentTileViewControl.tileViewItems.Add(item);
    Dictionary<int, TileViewItem> tileViewItemOrder1 = this.ParentTileViewControl.GetTileViewItemOrder();
    tileViewItemOrder1.Add(this.ParentTileViewControl.tileViewItems.Count, item);
    this.ParentTileViewControl.SetRowsAndColumns(tileViewItemOrder1);
    this.ParentTileViewControl.UpdateTileViewLayout(true);
    ObservableCollection<TileViewItem> tileViewItems = this.ParentTileViewControl.tileViewItems;
    Dictionary<int, TileViewItem> tileViewItemOrder2 = this.ParentTileViewControl.GetTileViewItemOrder();
    if (this.ParentTileViewControl.tileViewItems != null)
    {
      TileViewItem tileViewItem1 = this.ParentTileViewControl.tileViewItems[this.ParentTileViewControl.tileViewItems.Count - 1];
      tileViewItemOrder2.Clear();
      this.ParentTileViewControl.tileViewItems.Clear();
      int num = 0;
      tileViewItems.RemoveAt(tileViewItems.Count - 1);
      foreach (TileViewItem tileViewItem2 in (Collection<TileViewItem>) tileViewItems)
      {
        if (num == this.ParentTileViewControl.tileViewItems.Count)
        {
          tileViewItemOrder2.Add(0, tileViewItem1);
          this.ParentTileViewControl.tileViewItems.Add(tileViewItem1);
          this.ParentTileViewControl.AllowAdd = true;
          if (this.ParentTileViewControl.AllowAdd)
          {
            tileViewItemOrder2.Add(num + 1, tileViewItem2);
            this.ParentTileViewControl.tileViewItems.Add(tileViewItem2);
            ++num;
          }
        }
        else if (tileViewItem1 != tileViewItem2)
        {
          tileViewItemOrder2.Add(num + 1, tileViewItem2);
          this.ParentTileViewControl.tileViewItems.Add(tileViewItem2);
          ++num;
        }
      }
    }
    this.ParentTileViewControl.SetRowsAndColumns(tileViewItemOrder2);
    this.ParentTileViewControl.UpdateTileViewLayout(true);
  }

  protected virtual void OnStateChanging(TileViewCancelEventArgs e)
  {
    if (this.StateChanging == null)
      return;
    this.StateChanging((object) this, e);
  }

  protected virtual void OnStateChanged(TileViewEventArgs e)
  {
    if (this.StateChanged == null)
      return;
    this.StateChanged((object) this, e);
  }

  public MinimizedItemsOrientation minposmethod() => this.MinPos;

  internal override void AnimatePosition(double x, double y)
  {
    x = Math.Round(x);
    y = Math.Round(y);
    base.AnimatePosition(x, y);
    this.animationPosition.Completed += new EventHandler(this.animationPosition_Completed);
  }

  private void animationPosition_Completed(object sender, EventArgs e)
  {
    if (this.ParentTileViewControl != null)
    {
      this.ParentTileViewControl.ItemAnimationCompleted(this);
      if (this.ParentTileViewControl.IsVirtualizing && (this.ParentTileViewControl.Items.IndexOf((object) this) == (this.ParentTileViewControl.itemsPanel as TileViewVirtualizingPanel).LastVisibleIndex || this == this.ParentTileViewControl.maximizedItem))
      {
        this.ParentTileViewControl.Timer.Stop();
        (this.ParentTileViewControl.itemsPanel as TileViewVirtualizingPanel).InvalidateMeasure();
      }
    }
    this.animationPosition.Completed -= new EventHandler(this.animationPosition_Completed);
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    base.OnMouseEnter(e);
    if (this.MinMaxButtonVisibility != Visibility.Visible || this.ParentTileViewControl == null || !this.ParentTileViewControl.IsMinMaxButtonOnMouseOverOnly)
      return;
    this.makeToggleButtonVisible(true);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    if (this.ParentTileViewControl == null || !this.ParentTileViewControl.IsMinMaxButtonOnMouseOverOnly)
      return;
    this.makeToggleButtonVisible(false);
  }

  private void makeToggleButtonVisible(bool isvisible)
  {
    if (this.minMaxButton == null)
      return;
    if (isvisible)
      this.minMaxButton.SetValue(UIElement.VisibilityProperty, (object) Visibility.Visible);
    else
      this.minMaxButton.SetValue(UIElement.VisibilityProperty, (object) Visibility.Collapsed);
  }

  public static void OnItemTemplateChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs e)
  {
    TileViewItem tileViewItem = sender as TileViewItem;
    tileViewItem.ContentTemplate = tileViewItem.ItemTemplate;
  }

  internal void tileviewControl_IsMinMaxButtonOnMouseOverOnlyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (this.minMaxButton == null)
      return;
    if ((bool) e.NewValue)
      this.minMaxButton.SetValue(UIElement.VisibilityProperty, (object) Visibility.Collapsed);
    else
      this.minMaxButton.SetValue(UIElement.VisibilityProperty, (object) Visibility.Visible);
  }

  internal void ParentTileViewControl_IsClickHeaderToMaximizePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(bool) e.NewValue || this.HeaderPart == null)
      return;
    this.HeaderPart.MouseLeftButtonUp -= new MouseButtonEventHandler(this.HeaderPart_MouseLeftButtonUp);
    this.HeaderPart.MouseLeftButtonUp += new MouseButtonEventHandler(this.HeaderPart_MouseLeftButtonUp);
  }

  internal void ParentTileViewControl_AllowItemRepositioningChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (this.IsMovable)
      this.HeaderCursor = Cursors.Hand;
    else
      this.HeaderCursor = Cursors.Arrow;
  }

  private static void OnIsSelectedPropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    TileViewItem sender = obj as TileViewItem;
    if (sender.ParentTileViewControl != null && sender != null && sender.IsSelected)
    {
      if (sender.ParentTileViewControl.ItemsSource == null)
      {
        sender.ParentTileViewControl.SelectedIndex = sender.ParentTileViewControl.Items.IndexOf((object) sender);
        sender.ParentTileViewControl.SelectedItem = (object) sender;
      }
      else
      {
        sender.ParentTileViewControl.SelectedIndex = sender.ParentTileViewControl.Items.IndexOf(sender.DataContext);
        sender.ParentTileViewControl.SelectedItem = sender.DataContext;
      }
    }
    if (sender.Selected == null)
      return;
    sender.Selected((object) sender, new RoutedEventArgs()
    {
      Source = (object) sender
    });
  }

  protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
  {
    ScaleTransform scaleTransform1 = new ScaleTransform();
    if (e.CumulativeManipulation.Scale.X > 0.33)
    {
      ScaleTransform scaleTransform2 = scaleTransform1;
      ScaleTransform scaleTransform3 = scaleTransform1;
      Vector scale = e.CumulativeManipulation.Scale;
      double x;
      double num1 = x = scale.X;
      scaleTransform3.ScaleY = x;
      double num2 = num1;
      scaleTransform2.ScaleX = num2;
      this.RenderTransformOrigin = new Point(0.5, 0.5);
      this.RenderTransform = (Transform) scaleTransform1;
    }
    else if (e.CumulativeManipulation.Scale.X > 0.65)
      scaleTransform1.ScaleX = scaleTransform1.ScaleY = 1.0;
    base.OnManipulationDelta(e);
  }

  protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
  {
    ScaleTransform scaleTransform = new ScaleTransform()
    {
      ScaleX = 1.0,
      ScaleY = 1.0
    };
    if (e.TotalManipulation.Scale.X < 0.75)
    {
      this.ParentTileViewControl.SetState(this, TileViewItemState.Normal);
      this.RenderTransform = (Transform) scaleTransform;
    }
    else
      this.RenderTransform = (Transform) scaleTransform;
    base.OnManipulationCompleted(e);
  }

  protected override AutomationPeer OnCreateAutomationPeer()
  {
    return (AutomationPeer) new TileViewItemAutomationPeer(this);
  }

  public delegate void TileViewCancelEventHandler(object sender, TileViewCancelEventArgs args);

  public delegate void TileViewEventHandler(object sender, TileViewEventArgs args);

  public class CloseEventArgs : CancelEventArgs
  {
    public object Source { get; set; }

    public new bool Cancel { get; set; }
  }

  public delegate void CloseEventHandler(object sender, TileViewItem.CloseEventArgs args);
}
