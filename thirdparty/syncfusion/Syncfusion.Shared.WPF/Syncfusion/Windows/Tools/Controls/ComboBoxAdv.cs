// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ComboBoxAdv
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (ComboBoxAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ComboBoxAdv/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (ComboBoxAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ComboBoxAdv/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (ComboBoxAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ComboBoxAdv/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (ComboBoxAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ComboBoxAdv/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (ComboBoxAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ComboBoxAdv/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (ComboBoxAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ComboBoxAdv/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (ComboBoxAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ComboBoxAdv/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (ComboBoxAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ComboBoxAdv/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (ComboBoxAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ComboBoxAdv/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (ComboBoxAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ComboBoxAdv/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (ComboBoxAdv), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/ComboBoxAdv/Themes/Generic.xaml")]
public class ComboBoxAdv : Selector, IDisposable, ITextInputLayoutSelector
{
  private Point lastMousePosition;
  internal bool IsGotKeyBoardFocus;
  internal object oldItem;
  internal object newItem;
  internal Window MainWindow;
  internal int itemcount;
  internal bool removeFlag;
  internal bool internalSelect;
  private ItemsControl selectedItems;
  internal ScrollViewer DropDownScrollBar;
  private ContentPresenter selectedContent;
  internal TextBlock defaultText;
  internal TextBox IsEditDefaultText;
  private ToggleButton toggleButton;
  internal ComboBoxItemAdv SelectAllItem;
  internal Button OKButton;
  internal Button CancelButton;
  public string searchText = "";
  private TextBox Part_IsEdit;
  private int charindex;
  private ItemsControl tokenItemsControl;
  private Border tokenBorder;
  private int lastRowTokenItemIndex;
  private TextBlock NoRecords;
  private Key KeyPressed;
  internal bool IsTokenRemoved;
  private string oldFilter = string.Empty;
  private List<string> AddedTokenItems = new List<string>();
  private DispatcherTimer timer = new DispatcherTimer()
  {
    Interval = TimeSpan.FromSeconds(0.5)
  };
  private bool isTextInputLayoutChild;
  internal Popup popup;
  internal SelectionChangedEventArgs SelectionChangedEvent;
  internal bool internalChange;
  private bool _keypressed;
  internal static readonly DependencyProperty InactiveBrushProperty = DependencyProperty.Register(nameof (InactiveBrush), typeof (Brush), typeof (ComboBoxAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register(nameof (IsEditable), typeof (bool), typeof (ComboBoxAdv), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(ComboBoxAdv.OnIsEditableChanged)));
  public static readonly DependencyProperty EnableTokenProperty = DependencyProperty.Register(nameof (EnableToken), typeof (bool), typeof (ComboBoxAdv), new PropertyMetadata((object) false, new PropertyChangedCallback(ComboBoxAdv.OnEnableTokenChanged)));
  public static readonly DependencyProperty AutoCompleteModeProperty = DependencyProperty.Register(nameof (AutoCompleteMode), typeof (AutoCompleteModes), typeof (ComboBoxAdv), new PropertyMetadata((object) AutoCompleteModes.None, new PropertyChangedCallback(ComboBoxAdv.OnAutoCompleteModeChanged)));
  private bool isReadOnly;
  public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof (IsReadOnly), typeof (bool), typeof (ComboBoxAdv), (PropertyMetadata) new UIPropertyMetadata((object) false));
  public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(nameof (MaxDropDownHeight), typeof (double), typeof (ComboBoxAdv), new PropertyMetadata((object) (SystemParameters.MaximizedPrimaryScreenHeight / 3.0)));
  public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof (IsDropDownOpen), typeof (bool), typeof (ComboBoxAdv), new PropertyMetadata((object) false, new PropertyChangedCallback(ComboBoxAdv.OnIsDropDownOpenChanged)));
  public static readonly DependencyProperty SelectionBoxItemTemplateProperty = DependencyProperty.Register(nameof (SelectionBoxItemTemplate), typeof (DataTemplate), typeof (ComboBoxAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty SelectionBoxItemProperty = DependencyProperty.Register(nameof (SelectionBoxItem), typeof (object), typeof (ComboBoxAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty SelectionBoxItemStringFormatProperty = DependencyProperty.Register(nameof (SelectionBoxItemStringFormat), typeof (string), typeof (ComboBoxAdv));
  public static readonly DependencyProperty AllowMultiSelectProperty = DependencyProperty.Register(nameof (AllowMultiSelect), typeof (bool), typeof (ComboBoxAdv), new PropertyMetadata((object) false, new PropertyChangedCallback(ComboBoxAdv.OnAllowMultiSelectChanged)));
  public static readonly DependencyProperty AllowSelectAllProperty = DependencyProperty.Register(nameof (AllowSelectAll), typeof (bool), typeof (ComboBoxAdv), new PropertyMetadata((object) false));
  public static readonly DependencyProperty EnableOKCancelProperty = DependencyProperty.Register(nameof (EnableOKCancel), typeof (bool), typeof (ComboBoxAdv), new PropertyMetadata((object) false));
  public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(nameof (SelectedItems), typeof (IEnumerable), typeof (ComboBoxAdv), new PropertyMetadata((object) null, new PropertyChangedCallback(ComboBoxAdv.OnSelectedItemsChanged)));
  public static readonly DependencyProperty SelectedValueDelimiterProperty = DependencyProperty.Register(nameof (SelectedValueDelimiter), typeof (string), typeof (ComboBoxAdv), new PropertyMetadata((object) " - ", new PropertyChangedCallback(ComboBoxAdv.OnSelectedValueDelimiterChanged)));
  public static readonly DependencyProperty SelectionBoxTemplateProperty = DependencyProperty.Register(nameof (SelectionBoxTemplate), typeof (DataTemplate), typeof (ComboBoxAdv), new PropertyMetadata((object) null, new PropertyChangedCallback(ComboBoxAdv.OnSelectionBoxTemplateChanged)));
  public static readonly DependencyProperty DefaultTextProperty = DependencyProperty.Register(nameof (DefaultText), typeof (string), typeof (ComboBoxAdv), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty DropDownContentTemplateProperty = DependencyProperty.Register(nameof (DropDownContentTemplate), typeof (DataTemplate), typeof (ComboBoxAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (ComboBoxAdv), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ComboBoxAdv.OnTextChanged)));

  static ComboBoxAdv()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ComboBoxAdv), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ComboBoxAdv)));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public event EventHandler DropDownClosed;

  public event EventHandler DropDownOpened;

  public ComboBoxAdv()
  {
    if (this.SelItemsInternal == null)
      this.SelItemsInternal = (IList) new ObservableCollection<object>();
    if (this.ChangedItems == null)
      this.ChangedItems = new List<object>();
    EventManager.RegisterClassHandler(typeof (ComboBoxAdv), Mouse.MouseWheelEvent, (Delegate) new MouseWheelEventHandler(ComboBoxAdv.OnMouseWheel), true);
    EventManager.RegisterClassHandler(typeof (ComboBoxAdv), Mouse.MouseDownEvent, (Delegate) new MouseButtonEventHandler(ComboBoxAdv.OnMouseButtonDown), true);
    EventManager.RegisterClassHandler(typeof (TextBox), Mouse.MouseDownEvent, (Delegate) new MouseButtonEventHandler(this.OnMouseLeftButtonDown), true);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public char? oldTempChar { get; set; }

  public char newTempChar { get; set; }

  bool ITextInputLayoutSelector.IsTextInputLayoutChild
  {
    get => this.isTextInputLayoutChild;
    set => this.isTextInputLayoutChild = value;
  }

  protected override void OnLostFocus(RoutedEventArgs e)
  {
    base.OnLostFocus(e);
    if (this.Part_IsEdit != null && !this.IsDropDownOpen && !this.Part_IsEdit.IsVisible)
    {
      this.timer.Stop();
      this.searchText = "";
    }
    if (this.toggleButton == null)
      return;
    bool? isChecked = this.toggleButton.IsChecked;
    if ((!isChecked.GetValueOrDefault() ? 0 : (isChecked.HasValue ? 1 : 0)) == 0 || this.popup.IsKeyboardFocusWithin)
      return;
    this.toggleButton.IsChecked = new bool?(false);
  }

  protected override void OnDisplayMemberPathChanged(
    string oldDisplayMemberPath,
    string newDisplayMemberPath)
  {
    base.OnDisplayMemberPathChanged(oldDisplayMemberPath, newDisplayMemberPath);
    this.UpdateSelectionBox();
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is ComboBoxItemAdv;

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new ComboBoxItemAdv();
  }

  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    ComboBoxItemAdv element1 = element as ComboBoxItemAdv;
    if (this.ItemsSource != null && this.ItemTemplate != null)
    {
      element1.ContentTemplate = this.ItemTemplate;
      if (this.SelectionBoxItemTemplate == null)
        this.SelectionBoxItemTemplate = this.ItemTemplate;
    }
    if (element1.CheckBox != null)
      this.UpdateCheckBoxVisibility(element1);
    else if (!this.AllowMultiSelect && !this.IsDropDownOpen)
      this.UpdateSelectionOnDropDownOpen(this);
    if (this.ItemsSource != null && this.ItemTemplateSelector != null)
      element1.ContentTemplateSelector = this.ItemTemplateSelector;
    if (item is ComboBoxItemAdv)
      base.PrepareContainerForItemOverride((DependencyObject) element1, item);
    else
      element1.Content = item;
    if (this.popup == null || !(this.popup.Child is FrameworkElement child) || child.ActualWidth <= child.MinWidth)
      return;
    child.MinWidth = child.ActualWidth;
  }

  protected override void OnSelectionChanged(SelectionChangedEventArgs e)
  {
    if (!this.internalChange)
      base.OnSelectionChanged(e);
    if (this.SelectedItem == null || this.SelectedIndex < 0)
    {
      this.SelectedItem = (object) null;
      this.SelectedIndex = -1;
      this.SelectionBoxItem = (object) null;
      if (this.Part_IsEdit != null && this.IsEditable)
        this.Part_IsEdit.Text = string.Empty;
      if (this.selectedContent != null && this.selectedContent.Content != null)
        this.selectedContent.Content = (object) null;
    }
    if (this.SelectedItem != null && this.SelItemsInternal != null && this.SelItemsInternal.Count <= 0)
    {
      this.UpdateText();
      this.SelItemsInternal.Add(this.SelectedItem);
      if (this.SelectedItems == null && this.SelItemsInternal.Count > 0)
        this.SelectedItems = (IEnumerable) this.SelItemsInternal;
    }
    this.UpdateSelectAllItemState();
    this.SelectionChangedEvent = e;
  }

  protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
  {
    base.OnRenderSizeChanged(sizeInfo);
    if (!this.popup.IsOpen)
      return;
    ++this.popup.VerticalOffset;
    this.popup.VerticalOffset = 0.0;
  }

  protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    if (this.IsLoaded && this.AutoCompleteMode == AutoCompleteModes.Suggest && (this.EnableToken && this.AllowMultiSelect && this.IsEditable || this.IsEditable && !this.AllowMultiSelect))
    {
      if (newValue != null)
        CollectionViewSource.GetDefaultView((object) newValue).Filter += new Predicate<object>(this.FilterItem);
      if (oldValue != null)
      {
        ICollectionView defaultView = CollectionViewSource.GetDefaultView((object) oldValue);
        if (defaultView != null)
          defaultView.Filter -= new Predicate<object>(this.FilterItem);
      }
    }
    base.OnItemsSourceChanged(oldValue, newValue);
    if (!this.IsLoaded || this.SelectedItems == null)
      return;
    this.SelectedItems = (IEnumerable) null;
  }

  private bool FilterItem(object value)
  {
    if (!string.IsNullOrEmpty(this.DisplayMemberPath))
    {
      value = this.GetDisplayMemberValue(value);
      if (this.Part_IsEdit != null && this.Part_IsEdit.Text.Length == 0)
        return value.ToString().ToLower().Contains(this.Part_IsEdit.Text.ToLower());
    }
    return value != null && this.Part_IsEdit != null && this.Part_IsEdit.Text != null && value.ToString().ToLower().Contains(this.Part_IsEdit.Text.ToLower());
  }

  internal ComboBoxItemAdv GetItemContainer(object obj)
  {
    return this.ItemContainerGenerator.ContainerFromItem(obj) as ComboBoxItemAdv;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.selectedItems = this.GetTemplateChild("PART_SelectedItems") as ItemsControl;
    this.selectedContent = this.GetTemplateChild("ContentPresenter") as ContentPresenter;
    this.defaultText = this.GetTemplateChild("PART_DefaultText") as TextBlock;
    this.popup = this.GetTemplateChild("PART_Popup") as Popup;
    this.toggleButton = this.GetTemplateChild("PART_ToggleButton") as ToggleButton;
    this.IsEditDefaultText = this.GetTemplateChild("PART_IsEditDefaultText") as TextBox;
    this.SelectAllItem = this.GetTemplateChild("PART_SelectAll") as ComboBoxItemAdv;
    this.OKButton = this.GetTemplateChild("PART_OKButton") as Button;
    this.CancelButton = this.GetTemplateChild("PART_CancelButton") as Button;
    this.DropDownScrollBar = this.GetTemplateChild("DropDownScrollViewer") as ScrollViewer;
    this.NoRecords = this.GetTemplateChild("No_Records") as TextBlock;
    this.tokenItemsControl = this.GetTemplateChild("PART_TokenItems") as ItemsControl;
    this.tokenBorder = this.GetTemplateChild("PART_Border") as Border;
    if (this.isTextInputLayoutChild && this.toggleButton != null)
      this.toggleButton.Visibility = Visibility.Collapsed;
    if (this.DropDownScrollBar != null)
      this.DropDownScrollBar.PanningMode = PanningMode.Both;
    this.Part_IsEdit = this.tokenItemsControl == null ? this.GetTemplateChild("PART_Editable") as TextBox : this.tokenItemsControl.ItemContainerGenerator.ContainerFromIndex(this.tokenItemsControl.Items.Count - 1) as TextBox;
    this.MainWindow = VisualUtils.FindAncestor((Visual) this, typeof (Window)) as Window;
    this.UpdateText();
    this.UpdateToken();
    this.UpdateSelectionBox();
    this.UpdateSelectMode();
    this.WireEvent();
  }

  private void TokenBorder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (this.Part_IsEdit == null)
      return;
    this.Part_IsEdit.Focus();
  }

  private void WireEvent()
  {
    if (this.Part_IsEdit != null)
    {
      this.Part_IsEdit.TextChanged += new TextChangedEventHandler(this.Part_IsEdit_TextChanged);
      this.Part_IsEdit.PreviewKeyDown += new KeyEventHandler(this.Part_IsEdit_PreviewKeyDown);
      this.Part_IsEdit.LostFocus += new RoutedEventHandler(this.Part_IsEdit_LostFocus);
      this.Part_IsEdit.GotFocus += new RoutedEventHandler(this.Part_IsEdit_GotFocus);
    }
    if (this.tokenItemsControl != null)
      this.tokenItemsControl.SizeChanged += new SizeChangedEventHandler(this.TokenItemsControl_SizeChanged);
    if (this.tokenBorder != null)
      this.tokenBorder.PreviewMouseDown += new MouseButtonEventHandler(this.TokenBorder_PreviewMouseDown);
    this.GotFocus -= new RoutedEventHandler(this.ComboBoxAdv_GotFocus);
    this.GotFocus += new RoutedEventHandler(this.ComboBoxAdv_GotFocus);
    this.LostFocus += new RoutedEventHandler(this.ComboBoxAdv_LostFocus);
    if (this.popup != null)
    {
      this.popup.Opened += new EventHandler(this.Popup_Opened);
      this.popup.Closed -= new EventHandler(this.popup_Closed);
      this.popup.Closed += new EventHandler(this.popup_Closed);
    }
    if (this.MainWindow != null)
    {
      this.MainWindow.LocationChanged -= new EventHandler(this.MainWindow_LocationChanged);
      this.MainWindow.LocationChanged += new EventHandler(this.MainWindow_LocationChanged);
      this.MainWindow.Deactivated -= new EventHandler(this.MainWindow_Deactivated);
      this.MainWindow.Deactivated += new EventHandler(this.MainWindow_Deactivated);
    }
    this.Unloaded -= new RoutedEventHandler(this.ComboBoxAdv_Unloaded);
    this.Unloaded += new RoutedEventHandler(this.ComboBoxAdv_Unloaded);
    this.Loaded += new RoutedEventHandler(this.ComboBoxAdv_Loaded);
    this.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxAdv_SelectionChanged);
    if (this.OKButton != null)
    {
      this.OKButton.Click -= new RoutedEventHandler(this.OnOKButtonClick);
      this.OKButton.Click += new RoutedEventHandler(this.OnOKButtonClick);
    }
    if (this.CancelButton != null)
    {
      this.CancelButton.Click -= new RoutedEventHandler(this.OnCancelButtonClick);
      this.CancelButton.Click += new RoutedEventHandler(this.OnCancelButtonClick);
    }
    this.timer.Tick += new EventHandler(this.timer_Tick);
  }

  private void ComboBoxAdv_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.tokenItemsControl != null && this.Part_IsEdit == null)
    {
      this.Part_IsEdit = this.tokenItemsControl.ItemContainerGenerator.ContainerFromIndex(this.tokenItemsControl.Items.Count - 1) as TextBox;
      if (this.Part_IsEdit != null)
      {
        this.Part_IsEdit.TextChanged += new TextChangedEventHandler(this.Part_IsEdit_TextChanged);
        this.Part_IsEdit.PreviewKeyDown += new KeyEventHandler(this.Part_IsEdit_PreviewKeyDown);
        this.Part_IsEdit.LostFocus += new RoutedEventHandler(this.Part_IsEdit_LostFocus);
        this.Part_IsEdit.GotFocus += new RoutedEventHandler(this.Part_IsEdit_GotFocus);
      }
    }
    if (!this.IsDropDownOpen || !this.IsEditable || this.AllowMultiSelect || this.AutoCompleteMode != AutoCompleteModes.Suggest)
      return;
    this.OnFilterApplied();
  }

  private void TokenItemsControl_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (this.tokenItemsControl.Items.Count > 0 && e.NewSize.Height > 0.0)
    {
      if (this.Part_IsEdit == null)
        this.Part_IsEdit = this.tokenItemsControl.ItemContainerGenerator.ContainerFromIndex(this.tokenItemsControl.Items.Count - 1) as TextBox;
      double num1 = this.tokenBorder.ActualWidth - this.tokenBorder.BorderThickness.Left - this.tokenBorder.BorderThickness.Right;
      double num2 = 0.0;
      double num3 = 0.0;
      for (int rowTokenItemIndex = this.lastRowTokenItemIndex; rowTokenItemIndex < this.tokenItemsControl.Items.Count - 1; ++rowTokenItemIndex)
      {
        num3 = (this.tokenItemsControl.ItemContainerGenerator.ContainerFromIndex(rowTokenItemIndex) as ComboBoxTokenItem).ActualWidth;
        num2 += num3;
      }
      double num4 = num1 > num2 ? num1 - num2 : num1 - num3;
      if (num4 > 75.0)
      {
        if (num1 < num2)
          this.lastRowTokenItemIndex = this.tokenItemsControl.Items.Count - 2;
        this.Part_IsEdit.MinWidth = num4;
        this.MinHeight = e.NewSize.Height + this.Padding.Top + this.Padding.Bottom;
      }
      else
      {
        this.lastRowTokenItemIndex = this.tokenItemsControl.Items.Count - 2;
        this.Part_IsEdit.MinWidth = num1;
        this.MinHeight = e.NewSize.Height + this.Padding.Top + this.Padding.Bottom;
      }
    }
    else
      this.MinHeight = e.NewSize.Height + this.Padding.Top + this.Padding.Bottom;
  }

  private void ComboBoxAdv_LostFocus(object sender, RoutedEventArgs e)
  {
    if (this.Part_IsEdit != null && !string.IsNullOrEmpty(this.Part_IsEdit.Text) && this.KeyPressed != Key.Up && this.KeyPressed != Key.Down && this.KeyPressed != Key.Home && this.KeyPressed != Key.End && this.KeyPressed != Key.Return && (this.EnableToken && this.IsEditable && this.AllowMultiSelect || !this.AllowMultiSelect && this.IsEditable && this.SelectedIndex == -1))
      this.Part_IsEdit.Text = string.Empty;
    if (this.EnableToken && this.IsEditable && !this.IsDropDownOpen && this.ShowDefaultText())
      this.IsEditDefaultText.Visibility = Visibility.Visible;
    if (this.NoRecords == null || this.NoRecords.Visibility != Visibility.Visible || this.IsDropDownOpen)
      return;
    this.NoRecords.Visibility = Visibility.Collapsed;
  }

  private void UpdateToken()
  {
    this.AddedTokenItems.Clear();
    if (!this.EnableToken)
      return;
    foreach (object obj in (IEnumerable) this.SelItemsInternal)
    {
      string text = this.ItemsSource != null || !(obj is ComboBoxItemAdv) ? (!string.IsNullOrEmpty(this.DisplayMemberPath) ? this.GetDisplayMemberValue(obj).ToString() : obj.ToString()) : (obj as ComboBoxItemAdv).Content.ToString();
      if (text != null)
      {
        if (this.tokenItemsControl != null && !this.tokenItemsControl.Items.Contains((object) text))
          this.AddToken(text, Key.Return);
        if (this.EnableOKCancel && this.AddedTokenItems != null && !this.AddedTokenItems.Contains(text))
          this.AddedTokenItems.Add(text);
      }
    }
  }

  private void UpdateText()
  {
    if (this.SelectedItem != null)
    {
      if (this.SelectedItem is ComboBoxItemAdv)
      {
        if ((this.SelectedItem as ComboBoxItemAdv).Parent != null && this.SelItemsInternal.Count <= 0)
          (this.SelectedItem as ComboBoxItemAdv).UpdateSelection();
        this.SelectionBoxItem = (this.SelectedItem as ComboBoxItemAdv).Content;
        if (this.Part_IsEdit != null && this.Part_IsEdit.Visibility == Visibility.Visible)
        {
          this.Part_IsEdit.Text = this.searchText;
          if (this.searchText != null)
          {
            if (this.SelectionBoxItem == null)
            {
              this.Part_IsEdit.Text = this.SelectionBoxItem.ToString().Remove(this.searchText.Count<char>(), this.SelectionBoxItem.ToString().Count<char>() - this.searchText.Count<char>());
              this.Part_IsEdit.CaretIndex = this.searchText.Count<char>();
              this.Part_IsEdit.AppendText(this.SelectedItem.ToString().Remove(0, this.searchText.Count<char>()));
              this.Part_IsEdit.SelectionStart = this.searchText.Count<char>();
              this.Part_IsEdit.SelectionLength = this.SelectedItem.ToString().Count<char>() - this.searchText.Count<char>();
            }
            else
            {
              this.Part_IsEdit.Text = this.SelectionBoxItem.ToString().Remove(this.searchText.Count<char>(), this.SelectionBoxItem.ToString().Count<char>() - this.searchText.Count<char>());
              this.Part_IsEdit.CaretIndex = this.searchText.Count<char>();
              this.Part_IsEdit.AppendText(this.SelectionBoxItem.ToString().Remove(0, this.searchText.Count<char>()));
              this.Part_IsEdit.SelectionStart = this.searchText.Count<char>();
              this.Part_IsEdit.SelectionLength = this.SelectionBoxItem.ToString().Count<char>() - this.searchText.Count<char>();
            }
          }
        }
        else if (!this.AllowMultiSelect && !this.SelectionBoxItem.ToString().Equals(this.Text))
          this.Text = this.SelectionBoxItem.ToString();
      }
      else
      {
        this.SelectionBoxItem = this.SelectedItem;
        if (this.SelectedItem != null && this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) != null && this.selectedContent != null && string.IsNullOrEmpty(this.selectedContent.Content.ToString()) && this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) is ComboBoxItemAdv)
          this.selectedContent.Content = (object) (this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) as ComboBoxItemAdv).ToString();
        if (this.Part_IsEdit != null && this.Part_IsEdit.Visibility == Visibility.Visible)
        {
          if (string.IsNullOrEmpty(this.DisplayMemberPath))
          {
            if (!string.IsNullOrEmpty(this.searchText))
            {
              this.Part_IsEdit.Text = this.SelectionBoxItem.ToString().Remove(this.searchText.Count<char>(), this.SelectionBoxItem.ToString().Count<char>() - this.searchText.Count<char>());
              this.Part_IsEdit.CaretIndex = this.searchText.Count<char>();
              this.Part_IsEdit.AppendText(this.SelectionBoxItem.ToString().Remove(0, this.searchText.Count<char>()));
              this.Part_IsEdit.SelectionStart = this.searchText.Count<char>();
              this.Part_IsEdit.SelectionLength = this.SelectionBoxItem.ToString().Count<char>() - this.searchText.Count<char>();
            }
            else
            {
              this.Part_IsEdit.Text = this.SelectionBoxItem.ToString();
              this.Part_IsEdit.SelectAll();
            }
          }
          else
          {
            object displayMemberValue = this.GetDisplayMemberValue(this.SelectionBoxItem);
            if (displayMemberValue != null)
            {
              if (!string.IsNullOrEmpty(this.searchText))
              {
                this.Part_IsEdit.Text = displayMemberValue.ToString().Remove(this.searchText.Count<char>(), displayMemberValue.ToString().Count<char>() - this.searchText.Count<char>());
                this.Part_IsEdit.CaretIndex = this.searchText.Count<char>();
                this.Part_IsEdit.AppendText(displayMemberValue.ToString().Remove(0, this.searchText.Count<char>()));
                this.Part_IsEdit.SelectionStart = this.searchText.Count<char>();
                this.Part_IsEdit.SelectionLength = displayMemberValue.ToString().Count<char>() - this.searchText.Count<char>();
              }
              else
              {
                this.Part_IsEdit.Text = displayMemberValue.ToString();
                this.Part_IsEdit.SelectAll();
              }
            }
          }
        }
        else if (!this.AllowMultiSelect)
        {
          if (string.IsNullOrEmpty(this.DisplayMemberPath))
          {
            if (!this.SelectionBoxItem.ToString().Equals(this.Text))
              this.Text = this.SelectionBoxItem.ToString();
          }
          else
          {
            PropertyInfo property = this.SelectionBoxItem.GetType().GetProperty(this.DisplayMemberPath);
            object obj = property == (PropertyInfo) null ? (object) null : property.GetValue(this.SelectionBoxItem, (object[]) null);
            if (obj != null && !obj.ToString().Equals(this.Text))
              this.Text = obj.ToString();
          }
        }
      }
    }
    else if (this.Part_IsEdit != null && this.Part_IsEdit.Visibility == Visibility.Visible && !this.Part_IsEdit.Text.Equals(this.Text))
      this.Part_IsEdit.Text = this.Text;
    if (this.IsEditDefaultText == null)
      return;
    if (this.ShowDefaultText() && this.defaultText.Visibility != Visibility.Visible)
      this.IsEditDefaultText.Visibility = Visibility.Visible;
    else
      this.IsEditDefaultText.Visibility = Visibility.Collapsed;
  }

  private void Popup_Opened(object sender, EventArgs e)
  {
    if (!this.IsEditable || this.Part_IsEdit == null || !this.Part_IsEdit.IsVisible)
      return;
    this.Part_IsEdit.Focus();
  }

  private void ComboBoxAdv_GotFocus(object sender, RoutedEventArgs e)
  {
    if (this.IsEditDefaultText != null && this.IsEditDefaultText.IsVisible)
      this.IsEditDefaultText.Visibility = Visibility.Collapsed;
    if (this.Part_IsEdit == null || !this.Part_IsEdit.IsVisible || this.toggleButton == null)
      return;
    bool? isChecked = this.toggleButton.IsChecked;
    if ((!isChecked.GetValueOrDefault() ? 1 : (!isChecked.HasValue ? 1 : 0)) == 0)
      return;
    this.Part_IsEdit.Focus();
  }

  private void Part_IsEdit_GotFocus(object sender, RoutedEventArgs e)
  {
    if (this.SelectedItem == null || !(sender is TextBox))
      return;
    if (this.KeyPressed == Key.Return && !this.AllowMultiSelect)
    {
      (sender as TextBox).SelectionStart = this.SelectedItem.ToString().Count<char>();
    }
    else
    {
      (sender as TextBox).SelectionStart = 0;
      (sender as TextBox).SelectionLength = this.SelectedItem.ToString().Count<char>();
    }
  }

  private void Part_IsEdit_LostFocus(object sender, RoutedEventArgs e)
  {
    if (this.IsEditable && this.KeyPressed != Key.Up && this.KeyPressed != Key.Down && this.KeyPressed != Key.Home && this.KeyPressed != Key.End && this.KeyPressed != Key.Escape)
      this.ValidateItem(Key.Return);
    if (this.Part_IsEdit != null && string.IsNullOrEmpty(this.Part_IsEdit.Text) && this.IsEditDefaultText != null && this.IsEditDefaultText.Visibility == Visibility.Collapsed)
    {
      if (this.EnableToken && this.tokenItemsControl != null && this.tokenItemsControl.Items.Count != 0)
        this.IsEditDefaultText.Visibility = Visibility.Collapsed;
      else
        this.IsEditDefaultText.Visibility = Visibility.Visible;
    }
    if (!(sender is TextBox))
      return;
    this.searchText = "";
    (sender as TextBox).SelectionStart = 0;
    (sender as TextBox).SelectionLength = 0;
  }

  private void Part_IsEdit_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.Tab)
    {
      if (this.AllowMultiSelect || !this.IsEditable)
        return;
      this.Focusable = false;
    }
    else if (e.Key != Key.Back && e.Key != Key.Delete)
    {
      if (!char.IsLetter(e.Key.ToString().ToCharArray()[0]))
        return;
      this._keypressed = true;
    }
    else
    {
      this._keypressed = false;
      this.SelectedItem = (object) null;
    }
  }

  private void Part_IsEdit_TextChanged(object sender, TextChangedEventArgs e)
  {
    if (this.Part_IsEdit == null || this.Part_IsEdit.Visibility != Visibility.Visible)
      return;
    this.OnTextChanged(this.Part_IsEdit.Text);
    if (!this.Part_IsEdit.Text.Equals(this.Text))
      this.Text = this.Part_IsEdit.Text;
    if (this.IsEditDefaultText == null || !this.IsEditDefaultText.IsVisible || string.IsNullOrEmpty(this.Part_IsEdit.Text))
      return;
    this.IsEditDefaultText.Visibility = Visibility.Collapsed;
  }

  private void MainWindow_Deactivated(object sender, EventArgs e)
  {
    if (!this.IsDropDownOpen)
      return;
    this.IsDropDownOpen = false;
  }

  private void MainWindow_LocationChanged(object sender, EventArgs e)
  {
    if (!this.IsDropDownOpen)
      return;
    this.IsDropDownOpen = false;
  }

  private void popup_Closed(object sender, EventArgs e)
  {
    if (!this.IsDropDownOpen)
      return;
    this.IsDropDownOpen = false;
  }

  private void ComboBoxAdv_Unloaded(object sender, RoutedEventArgs e)
  {
    if (!(VisualUtils.FindAncestor((Visual) this, typeof (Window)) is Window ancestor))
      return;
    ancestor.LocationChanged -= new EventHandler(this.MainWindow_LocationChanged);
    ancestor.Deactivated -= new EventHandler(this.MainWindow_Deactivated);
  }

  private void OnCancelButtonClick(object sender, RoutedEventArgs e)
  {
    if (this.EnableToken && this.IsTokenRemoved)
    {
      if (this.AutoCompleteMode == AutoCompleteModes.Suggest)
        this.RefreshFilter();
      this.ResetTokenItems();
    }
    this.IsTokenRemoved = false;
    this.IsDropDownOpen = false;
  }

  private void OnOKButtonClick(object sender, RoutedEventArgs e)
  {
    this.UpdateSelectedItems();
    this.IsDropDownOpen = false;
  }

  protected internal void UpdateSelectAllItemState()
  {
    if (this.internalSelect || !this.AllowMultiSelect || !this.AllowSelectAll || this.SelectAllItem == null || this.SelectAllItem.CheckBox == null || this.SelItemsInternal == null)
      return;
    int count = this.SelItemsInternal.Count;
    this.internalSelect = true;
    if (count == 0)
    {
      this.SelectAllItem.CheckBox.IsThreeState = false;
      this.SelectAllItem.IsSelected = false;
      this.SelectAllItem.CheckBox.IsChecked = new bool?(false);
    }
    else if (count == this.Items.Count)
    {
      this.SelectAllItem.CheckBox.IsThreeState = false;
      this.SelectAllItem.IsSelected = true;
      this.SelectAllItem.CheckBox.IsChecked = new bool?(true);
    }
    else
    {
      this.SelectAllItem.CheckBox.IsThreeState = true;
      this.SelectAllItem.IsSelected = false;
      this.SelectAllItem.CheckBox.IsChecked = new bool?();
    }
    this.internalSelect = false;
  }

  protected internal void ResetSelectedItems()
  {
    if (this.ChangedItems.Count == 0)
      return;
    this.internalChange = true;
    this.ChangedItems.ForEach((Action<object>) (item =>
    {
      if (this.SelItemsInternal.Contains(item))
      {
        this.SelItemsInternal.Remove(item);
      }
      else
      {
        if (this.SelItemsInternal.Contains(item))
          return;
        this.SelItemsInternal.Add(item);
      }
    }));
    this.internalChange = false;
  }

  protected internal void UpdateSelectedItems()
  {
    this.internalChange = true;
    List<object> addedItems = new List<object>();
    List<object> removedItems = new List<object>();
    this.ChangedItems.ForEach((Action<object>) (item =>
    {
      if (this.SelItemsInternal.Contains(item))
        addedItems.Add(item);
      else
        removedItems.Add(item);
    }));
    if (this.SelItemsInternal.Count > 0)
    {
      ComboBoxItemAdv container = this.ItemContainerGenerator.ContainerFromItem(this.SelItemsInternal[0]) as ComboBoxItemAdv;
      int num = -1;
      if (container != null)
        num = this.ItemContainerGenerator.IndexFromContainer((DependencyObject) container);
      this.SelectedIndex = num;
      if (this.EnableOKCancel && this.EnableToken && this.IsEditable && this.AllowMultiSelect)
        this.UpdateToken();
      if (this.ItemsSource != null)
      {
        if (container != null && container.DataContext != null)
          this.SelectedItem = container.DataContext;
        else
          this.SelectedItem = (object) container;
      }
      else
        this.SelectionBoxItem = container == null ? (object) container : container.Content;
    }
    else
    {
      if (this.EnableToken && this.Part_IsEdit != null)
        this.Part_IsEdit.Text = string.Empty;
      this.SelectedItem = (object) null;
    }
    this.internalChange = false;
    this.FireOnSelectionChanged(removedItems, addedItems);
    this.UpdateSelectionBox();
    this.ChangedItems.Clear();
  }

  private void ResetTokenItems()
  {
    ICollectionView collectionView = this.ItemsSource == null || this.AutoCompleteMode != AutoCompleteModes.Suggest ? (ICollectionView) this.Items : CollectionViewSource.GetDefaultView((object) this.ItemsSource);
    List<string> stringList = new List<string>();
    foreach (object obj in (IEnumerable) this.SelItemsInternal)
    {
      string str = this.ItemsSource == null || string.IsNullOrEmpty(this.DisplayMemberPath) ? (this.ItemsSource != null || !(obj is ComboBoxItemAdv) ? obj.ToString() : (obj as ComboBoxItemAdv).Content.ToString()) : this.GetDisplayMemberValue(obj).ToString();
      if (str != null)
        stringList.Add(str);
    }
    foreach (object obj in (IEnumerable) collectionView)
    {
      string str = this.ItemsSource != null || !(obj is ComboBoxItemAdv) ? (!string.IsNullOrEmpty(this.DisplayMemberPath) ? this.GetDisplayMemberValue(obj).ToString() : obj.ToString()) : (obj as ComboBoxItemAdv).Content.ToString();
      if (!stringList.Contains(str) && this.AddedTokenItems.Contains(str.ToString()))
        this.SelItemsInternal.Add(obj);
    }
    if (this.IsEditDefaultText == null)
      return;
    if (this.ShowDefaultText())
      this.IsEditDefaultText.Visibility = Visibility.Visible;
    else
      this.IsEditDefaultText.Visibility = Visibility.Collapsed;
  }

  private bool ShowDefaultText()
  {
    return this.Part_IsEdit != null && string.IsNullOrEmpty(this.Part_IsEdit.Text) && this.IsEditDefaultText != null && !string.IsNullOrEmpty(this.IsEditDefaultText.Text) && this.SelItemsInternal.Count == 0 && this.SelectedItem == null;
  }

  private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!(sender is ComboBoxAdv comboBoxAdv))
      return;
    if (!comboBoxAdv.IsKeyboardFocusWithin)
      comboBoxAdv.Focus();
    e.Handled = true;
    if (e.OriginalSource != comboBoxAdv)
      return;
    comboBoxAdv.Close();
  }

  private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!this.IsDropDownOpen)
      return;
    this.IsDropDownOpen = false;
  }

  private static void OnMouseWheel(object sender, MouseWheelEventArgs e)
  {
    if (sender is ComboBoxAdv comboBoxAdv && comboBoxAdv.IsKeyboardFocusWithin)
    {
      if (!comboBoxAdv.IsDropDownOpen)
      {
        int num = e.Delta >= 0 ? (comboBoxAdv.SelectedIndex - 1 >= 0 ? comboBoxAdv.SelectedIndex - 1 : 0) : (comboBoxAdv.SelectedIndex + 1 <= comboBoxAdv.Items.Count ? comboBoxAdv.SelectedIndex + 1 : comboBoxAdv.Items.Count - 1);
        comboBoxAdv.SelectedIndex = num;
      }
      comboBoxAdv.IsGotKeyBoardFocus = true;
      e.Handled = true;
    }
    else
    {
      if (comboBoxAdv == null || !comboBoxAdv.IsDropDownOpen)
        return;
      e.Handled = true;
    }
  }

  internal void Close()
  {
    if (!this.IsDropDownOpen)
      return;
    this.ClearValue(ComboBoxAdv.IsDropDownOpenProperty);
    if (!this.IsDropDownOpen)
      return;
    this.IsDropDownOpen = false;
    this.popup.IsOpen = false;
  }

  private void ComboBoxAdv_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (this.SelectedIndex >= 0 && this.SelectedItem != null && !this.AllowMultiSelect)
    {
      this.SelItemsInternal.Clear();
      bool? synchronizedWithCurrentItem = this.IsSynchronizedWithCurrentItem;
      if (synchronizedWithCurrentItem.HasValue)
      {
        bool? nullable = synchronizedWithCurrentItem;
        if ((nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
          this.SelItemsInternal.Add(this.SelectedItem);
      }
    }
    this.UpdateSelectionBox();
    if (this.DropDownContentTemplate == null || !(sender is ComboBoxAdv comboBoxAdv) || comboBoxAdv.SelectedItem != null || e.RemovedItems.Count <= 0)
      return;
    comboBoxAdv.SelectedItem = e.RemovedItems[0];
  }

  internal void UpdateSelectMode()
  {
    if (this.selectedItems != null && this.selectedContent != null)
    {
      if (this.AllowMultiSelect)
      {
        if (this.EnableToken && this.IsEditable)
          this.selectedItems.Visibility = Visibility.Collapsed;
        else
          this.selectedItems.Visibility = Visibility.Visible;
        this.selectedContent.Visibility = Visibility.Collapsed;
      }
      else
      {
        this.selectedItems.Visibility = Visibility.Collapsed;
        if (!this.IsEditable && this.Part_IsEdit != null && !this.Part_IsEdit.IsVisible)
          this.selectedContent.Visibility = Visibility.Visible;
      }
    }
    for (int index = 0; index < this.Items.Count; ++index)
      this.UpdateCheckBoxVisibility(this.ItemContainerGenerator.ContainerFromIndex(index) as ComboBoxItemAdv);
  }

  private void UpdateCheckBoxVisibility(ComboBoxItemAdv item)
  {
    if (item == null || item.CheckBox == null)
      return;
    if (this.AllowMultiSelect)
      item.CheckBox.Visibility = Visibility.Visible;
    else
      item.CheckBox.Visibility = Visibility.Collapsed;
  }

  internal void UpdateItemSelectMode(ComboBoxItemAdv item)
  {
    if (this.selectedItems != null && this.selectedContent != null)
    {
      if (this.AllowMultiSelect)
      {
        if (this.EnableToken && this.IsEditable)
          this.selectedItems.Visibility = Visibility.Collapsed;
        else
          this.selectedItems.Visibility = Visibility.Visible;
        this.selectedContent.Visibility = Visibility.Collapsed;
      }
      else
      {
        this.selectedItems.Visibility = Visibility.Collapsed;
        if (!this.IsEditable && this.Part_IsEdit != null && !this.Part_IsEdit.IsVisible)
          this.selectedContent.Visibility = Visibility.Visible;
      }
    }
    this.UpdateCheckBoxVisibility(item);
  }

  internal void UpdateDefaultTextVisibility()
  {
    ObservableCollection<object> observableCollection = (ObservableCollection<object>) null;
    if (this.SelectedItems != null)
      observableCollection = new ObservableCollection<object>(this.SelectedItems.Cast<object>());
    if (this.defaultText == null)
      return;
    if (this.AllowMultiSelect)
    {
      if (observableCollection != null && observableCollection.Count == 0 && (!this.IsEditable || !this.EnableToken))
        this.defaultText.Visibility = Visibility.Visible;
      else
        this.defaultText.Visibility = Visibility.Collapsed;
    }
    else
    {
      if (this.IsEditable)
        return;
      if (this.SelectedItem == null)
      {
        this.defaultText.Visibility = Visibility.Visible;
        if (this.Part_IsEdit == null || !string.IsNullOrEmpty(this.Part_IsEdit.Text) || !this.IsEditable || this.defaultText == null || this.defaultText.Text.Equals(this.Part_IsEdit.Text))
          return;
        this.defaultText.Visibility = Visibility.Collapsed;
      }
      else
        this.defaultText.Visibility = Visibility.Collapsed;
    }
  }

  internal void UpdateSelectionBox()
  {
    if (this.SelectionBoxTemplate == null && this.ItemsSource != null && this.ItemTemplate != null)
      this.SelectionBoxTemplate = this.ItemTemplate;
    if (this.SelectedItem == null && this.selectedContent != null && this.SelectionBoxTemplate != null)
      this.selectedContent.ContentTemplate = (DataTemplate) null;
    else if (this.SelectedItem != null && this.selectedContent != null && this.SelectionBoxTemplate != null)
      this.selectedContent.ContentTemplate = this.SelectionBoxTemplate;
    if (this.SelectedItems == null)
      return;
    this.internalChange = false;
    ObservableCollection<object> observableCollection = new ObservableCollection<object>(this.SelectedItems.Cast<object>());
    if (this.AllowMultiSelect)
    {
      if ((this.SelectedItem == null || this.SelectedIndex < 0) && this.SelItemsInternal != null && this.SelItemsInternal.Count > 0)
      {
        for (int index = 0; index < this.SelItemsInternal.Count; ++index)
        {
          if (this.SelectedItem != this.SelItemsInternal[index])
          {
            this.SelectedItem = this.SelItemsInternal[index];
            if (this.SelectedItem != null)
              break;
          }
        }
        if (this.SelectedItem is ComboBoxItemAdv)
        {
          if ((this.SelectedItem as ComboBoxItemAdv).Content != null)
            this.SelectionBoxItem = (this.SelectedItem as ComboBoxItemAdv).Content;
        }
        else if (this.SelectionBoxItem == null || this.SelectionBoxItem != this.SelectedItem)
          this.SelectionBoxItem = this.SelectedItem;
      }
      if (this.selectedItems != null && this.SelectedItems != null)
      {
        ItemsControl itemsControl;
        if (this.IsEditable && this.EnableToken && this.tokenItemsControl != null)
        {
          itemsControl = this.tokenItemsControl;
          while (itemsControl.Items.Count > 1)
            itemsControl.Items.RemoveAt(0);
        }
        else
        {
          itemsControl = this.selectedItems;
          itemsControl.Items.Clear();
        }
        foreach (object source in (IEnumerable) this.SelItemsInternal)
        {
          ContentControl newItem = new ContentControl();
          newItem.ContentTemplate = this.SelectionBoxTemplate;
          newItem.IsTabStop = false;
          if (source != null && (this.AutoCompleteMode == AutoCompleteModes.Suggest || this.Items.Contains(source)))
          {
            if (source is ComboBoxItemAdv)
            {
              ComboBoxItemAdv comboBoxItemAdv = source as ComboBoxItemAdv;
              this.SelectItems();
              newItem.Content = comboBoxItemAdv.Content;
            }
            else if (!string.IsNullOrEmpty(this.DisplayMemberPath))
            {
              Type type1 = source.GetType();
              PropertyInfo property1 = type1.GetProperty(this.DisplayMemberPath, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty);
              if (property1 == (PropertyInfo) null && this.DisplayMemberPath.Contains<char>('.'))
              {
                string empty1 = string.Empty;
                string empty2 = string.Empty;
                if (!string.IsNullOrEmpty(this.DisplayMemberPath.Split('.')[0]))
                  empty1 = this.DisplayMemberPath.Split('.')[0];
                if (!string.IsNullOrEmpty(this.DisplayMemberPath.Split('.')[1]))
                  empty2 = this.DisplayMemberPath.Split('.')[1];
                if (!string.IsNullOrEmpty(empty1))
                {
                  object obj = type1.GetProperty(empty1, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty).GetValue(source, (object[]) null);
                  if (obj != null)
                  {
                    Type type2 = obj.GetType();
                    if (!string.IsNullOrEmpty(empty2))
                    {
                      PropertyInfo property2 = type2.GetProperty(empty2, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty);
                      newItem.Content = property2.GetValue(obj, (object[]) null);
                    }
                  }
                }
              }
              else if ((PropertyInfo) null != property1)
              {
                newItem.Content = property1.GetValue(source, (object[]) null);
              }
              else
              {
                switch (source)
                {
                  case DataRowView _:
                    int columnIndex = -1;
                    IEnumerator enumerator = (source as DataRowView).DataView.Table.Columns.GetEnumerator();
                    try
                    {
                      while (enumerator.MoveNext())
                      {
                        DataColumn current = (DataColumn) enumerator.Current;
                        ++columnIndex;
                        if (current.Caption == this.DisplayMemberPath)
                          newItem.Content = (source as DataRowView).Row[columnIndex];
                      }
                      break;
                    }
                    finally
                    {
                      if (enumerator is IDisposable disposable)
                        disposable.Dispose();
                    }
                  case ExpandoObject _:
                    newItem.Content = (source as ExpandoObject).Where<KeyValuePair<string, object>>((System.Func<KeyValuePair<string, object>, bool>) (i => i.Key == this.DisplayMemberPath)).FirstOrDefault<KeyValuePair<string, object>>().Value;
                    break;
                  default:
                    throw new InvalidOperationException("DisplayMemberPath has invalid property name");
                }
              }
            }
            else
              newItem.Content = source;
            if (this.IsEditable && this.EnableToken && this.tokenItemsControl != null)
            {
              ItemCollection items = itemsControl.Items;
              int insertIndex = itemsControl.Items.Count > 0 ? itemsControl.Items.Count - 1 : 0;
              ComboBoxTokenItem comboBoxTokenItem = new ComboBoxTokenItem();
              comboBoxTokenItem.Content = (object) newItem;
              ComboBoxTokenItem insertItem = comboBoxTokenItem;
              items.Insert(insertIndex, (object) insertItem);
            }
            else
              itemsControl.Items.Add((object) newItem);
            if (this.SelItemsInternal.IndexOf(source) < observableCollection.Count - 1 && (!this.EnableToken || !this.IsEditable))
              itemsControl.Items.Add((object) this.SelectedValueDelimiter);
          }
        }
        if (itemsControl.Items.Count > 0 && (!this.EnableToken || !this.IsEditable))
        {
          for (int index = 0; index < itemsControl.Items.Count; ++index)
          {
            if (this.SelectedValueDelimiter == itemsControl.Items[index].ToString() && itemsControl.Items.Count == index + 1)
              itemsControl.Items.RemoveAt(index);
          }
        }
      }
    }
    else
    {
      if (this.SelectedItem != null)
      {
        if (this.SelectedItem is ComboBoxItemAdv)
          this.SelectionBoxItem = (this.SelectedItem as ComboBoxItemAdv).Content;
        else if (this.SelectionBoxItem == null || this.SelectionBoxItem != this.SelectedItem)
          this.SelectionBoxItem = this.SelectedItem;
      }
      if (this.selectedContent != null && this.SelectedItem != null && this.SelectionBoxTemplate != null)
      {
        this.selectedContent.ContentTemplate = this.SelectionBoxTemplate;
        if (this.DisplayMemberPath == string.Empty)
        {
          this.selectedContent.Content = this.SelectedItem;
        }
        else
        {
          PropertyInfo property = this.SelectedItem.GetType().GetProperty(this.DisplayMemberPath, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty);
          this.selectedContent.Content = (PropertyInfo) null != property ? property.GetValue(this.SelectedItem, (object[]) null) : throw new InvalidOperationException("DisplayMemberPath has invalid property name");
        }
      }
    }
    this.UpdateDefaultTextVisibility();
  }

  protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
  {
    base.OnItemsChanged(e);
    if (this.AllowMultiSelect && this.AllowSelectAll)
      this.UpdateSelectAllItemState();
    this.UpdateSelectionBox();
  }

  protected override void OnTextInput(TextCompositionEventArgs e)
  {
    if (this.IsTextSearchEnabled && (this.ItemsSource != null && this.AutoCompleteMode == AutoCompleteModes.None || this.ItemsSource == null))
    {
      if (e.Text.Count<char>() > 0)
      {
        this.newTempChar = e.Text[0];
        this.searchText += (string) (object) this.newTempChar;
      }
      this.OnTextChanged(this.searchText);
      this.oldTempChar = new char?(this.newTempChar);
      this.timer.Start();
      this._keypressed = false;
    }
    base.OnTextInput(e);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    container = (ComboBoxItemAdv) null;
    int index1 = -1;
    if (e.OriginalSource is ComboBoxItemAdv)
    {
      if (!(e.OriginalSource is ComboBoxItemAdv container))
        container = this.ItemContainerGenerator.ContainerFromItem(e.OriginalSource) as ComboBoxItemAdv;
      index1 = this.ItemContainerGenerator.IndexFromContainer((DependencyObject) container);
    }
    else if (e.OriginalSource is ComboBoxAdv && (e.OriginalSource as ComboBoxAdv).SelectedItem != null)
    {
      container = (e.OriginalSource as ComboBoxAdv).SelectedItem as ComboBoxItemAdv;
      if (!this.AllowMultiSelect)
      {
        if (container == null)
          container = this.GetItemContainer((e.OriginalSource as ComboBoxAdv).SelectedItem);
        index1 = this.SelectedIndex;
      }
      else if (this.SelItemsInternal != null && this.SelItemsInternal.Count > 0 && this.Items.Count > 0)
      {
        if (container == null)
          container = this.GetItemContainer(this.SelItemsInternal[this.SelItemsInternal.Count - 1]);
        index1 = this.Items.IndexOf(this.SelItemsInternal[this.SelItemsInternal.Count - 1]);
      }
    }
    else if (this.IsEditable && !this.AllowMultiSelect)
    {
      if (this.SelectedItem is ComboBoxItemAdv)
        container = this.SelectedItem as ComboBoxItemAdv;
      else if (this.SelectedIndex != -1 && this.ItemContainerGenerator.ContainerFromIndex(this.SelectedIndex) is ComboBoxItemAdv)
        container = this.ItemContainerGenerator.ContainerFromIndex(this.SelectedIndex) as ComboBoxItemAdv;
    }
    if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt && (e.SystemKey == Key.Down || e.SystemKey == Key.Up))
    {
      if (this.Part_IsEdit != null)
        this.Part_IsEdit.Focus();
      this.IsDropDownOpen = !this.IsDropDownOpen;
      if (!this.AllowMultiSelect && this.IsEditable && this.Part_IsEdit != null && this.SelectedIndex != -1 && string.IsNullOrEmpty(this.Part_IsEdit.Text))
      {
        this.UpdateSelectedText(this.SelectedItem);
        this.Part_IsEdit.SelectionStart = this.Part_IsEdit.Text.Length;
      }
      this.DropDownScrollBar.ScrollToVerticalOffset((double) this.SelectedIndex);
    }
    if (e.Key == Key.Up || !this.IsEditable && e.Key == Key.Left)
    {
      this.KeyPressed = e.Key;
      if (this.Part_IsEdit != null && this.Part_IsEdit.IsVisible && !this.IsDropDownOpen && this.SelectedIndex != 0)
      {
        this.Part_IsEdit.Text = "";
        this.searchText = "";
        if (this.SelectedItem != null)
          index1 = this.Items.IndexOf(this.SelectedItem);
      }
      else if (this.SelectedItem != null && index1 == -1)
        index1 = this.Items.IndexOf(this.SelectedItem);
      if (index1 == 0 && this.AllowMultiSelect && this.AllowSelectAll && !this.SelectAllItem.IsHighlighted)
      {
        this.SelectAllItem.Focus();
        e.Handled = true;
        base.OnPreviewKeyDown(e);
        return;
      }
      if (index1 == 0)
        index1 = 0;
      else if (index1 < 0)
      {
        index1 = -1;
      }
      else
      {
        for (int index2 = index1; index2 < this.Items.Count && index2 > 0; --index2)
        {
          if (this.Items[index2 - 1] != null)
          {
            if (this.Items[index2 - 1] is ComboBoxItemAdv)
            {
              if ((this.Items[index2 - 1] as ComboBoxItemAdv).IsEnabled)
              {
                index1 = index2 - 1;
                break;
              }
            }
            else
            {
              index1 = index2 - 1;
              break;
            }
          }
        }
      }
      if (index1 >= 0)
      {
        if (!this.IsDropDownOpen && !this.AllowMultiSelect)
        {
          if (this.SelectedIndex != -1)
          {
            if (this.SelectedItem is ComboBoxItemAdv)
              (this.SelectedItem as ComboBoxItemAdv).IsSelected = false;
            else if (this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) != null && this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) is ComboBoxItemAdv comboBoxItemAdv)
              comboBoxItemAdv.IsSelected = false;
          }
          this.SelectedIndex = index1;
          this.ClearSelection();
        }
        else
        {
          if (this.ItemContainerGenerator.ContainerFromIndex(index1) is ComboBoxItemAdv comboBoxItemAdv1)
            comboBoxItemAdv1.Focus();
          if (comboBoxItemAdv1 != null && this.IsEditable)
          {
            if (this.SelectedIndex >= 0 && this.ItemContainerGenerator.ContainerFromIndex(this.SelectedIndex) is ComboBoxItemAdv comboBoxItemAdv2 && !this.AllowMultiSelect)
              comboBoxItemAdv2.IsSelected = false;
            if (!this.AllowMultiSelect)
            {
              if (this.Part_IsEdit != null)
              {
                if (this.ItemsSource != null)
                  this.UpdateSelectedText(comboBoxItemAdv1.Content);
                else
                  this.UpdateSelectedText((object) comboBoxItemAdv1);
              }
              comboBoxItemAdv1.IsHighlighted = true;
            }
          }
        }
      }
      e.Handled = true;
    }
    else if (e.Key == Key.Down || !this.IsEditable && e.Key == Key.Right)
    {
      this.KeyPressed = e.Key;
      if (this.Part_IsEdit != null && this.Part_IsEdit.IsVisible && !this.IsDropDownOpen)
      {
        if (this.SelectedIndex == this.Items.Count - 1)
        {
          index1 = this.SelectedIndex;
        }
        else
        {
          this.Part_IsEdit.Text = "";
          this.searchText = "";
          if (this.SelectedItem != null && this.Items != null)
            index1 = this.Items.IndexOf(this.SelectedItem);
        }
      }
      else if (this.Items != null && this.SelectedItem != null && index1 == -1)
        index1 = this.Items.IndexOf(this.SelectedItem);
      if (index1 == -1 && this.AllowMultiSelect && this.AllowSelectAll && !this.SelectAllItem.IsHighlighted)
      {
        this.SelectAllItem.Focus();
        e.Handled = true;
        base.OnPreviewKeyDown(e);
        return;
      }
      if (index1 == this.Items.Count - 1)
      {
        index1 = this.Items.Count - 1;
      }
      else
      {
        for (int index3 = index1; index3 + 1 < this.Items.Count; ++index3)
        {
          if (this.Items[index3 + 1] != null)
          {
            if (this.Items[index3 + 1] is ComboBoxItemAdv)
            {
              if ((this.Items[index3 + 1] as ComboBoxItemAdv).IsEnabled)
              {
                index1 = index3 + 1;
                break;
              }
            }
            else
            {
              index1 = index3 + 1;
              break;
            }
          }
        }
      }
      if (index1 >= 0)
      {
        if (!this.IsDropDownOpen && !this.AllowMultiSelect)
        {
          if (this.SelectedIndex != -1)
          {
            if (this.SelectedItem is ComboBoxItemAdv)
              (this.SelectedItem as ComboBoxItemAdv).IsSelected = false;
            else if (this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) != null && this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) is ComboBoxItemAdv comboBoxItemAdv)
              comboBoxItemAdv.IsSelected = false;
          }
          this.SelectedIndex = index1;
          this.ClearSelection();
        }
        else
        {
          if (this.ItemContainerGenerator.ContainerFromIndex(index1) is ComboBoxItemAdv comboBoxItemAdv3)
            comboBoxItemAdv3.Focus();
          if (comboBoxItemAdv3 != null && this.IsEditable)
          {
            if (this.SelectedIndex >= 0 && this.ItemContainerGenerator.ContainerFromIndex(this.SelectedIndex) is ComboBoxItemAdv comboBoxItemAdv4 && !this.AllowMultiSelect)
              comboBoxItemAdv4.IsSelected = false;
            if (!this.AllowMultiSelect)
            {
              if (this.Part_IsEdit != null)
              {
                if (this.ItemsSource != null)
                  this.UpdateSelectedText(comboBoxItemAdv3.Content);
                else
                  this.UpdateSelectedText((object) comboBoxItemAdv3);
              }
              comboBoxItemAdv3.IsHighlighted = true;
            }
          }
        }
      }
      e.Handled = true;
    }
    else if (e.Key == Key.Back)
    {
      if (this.EnableToken && this.AllowMultiSelect && this.IsEditable && this.Part_IsEdit != null && this.Part_IsEdit.Text == string.Empty)
      {
        this.IsTokenRemoved = true;
        if (this.SelItemsInternal.Count > 0)
          this.SelItemsInternal.Remove(this.SelItemsInternal[this.SelItemsInternal.Count - 1]);
        this.UpdateSelectionBox();
        this.Part_IsEdit.Focus();
      }
      if (this.Part_IsEdit != null && string.IsNullOrEmpty(this.Part_IsEdit.Text) && this.AutoCompleteMode == AutoCompleteModes.Suggest && this.IsEditable)
      {
        this.RefreshFilter();
        this.Part_IsEdit.Focus();
      }
    }
    else if (e.Key == Key.Return)
    {
      this.KeyPressed = e.Key;
      if (this.IsDropDownOpen && (container != null || this.EnableOKCancel && this.Part_IsEdit != null && string.IsNullOrEmpty(this.Part_IsEdit.Text)))
      {
        if (!this.AllowMultiSelect)
        {
          this.ClearSelection();
          if (this.SelectedIndex != -1)
          {
            if (this.SelectedItem is ComboBoxItemAdv)
              (this.SelectedItem as ComboBoxItemAdv).IsSelected = false;
            else if (this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) != null && this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) is ComboBoxItemAdv comboBoxItemAdv)
              comboBoxItemAdv.IsSelected = false;
          }
          if (container != null)
            container.IsSelected = true;
        }
        if (this.AllowMultiSelect && this.EnableOKCancel)
          this.UpdateSelectedItems();
        e.Handled = true;
      }
      if (container == null)
      {
        if (this.Part_IsEdit != null && this.Part_IsEdit.Text.Length > 0 && this.Items.Count < 1)
        {
          this.Part_IsEdit.SelectionStart = this.Part_IsEdit.Text.Length;
        }
        else
        {
          this.IsDropDownOpen = false;
          if (this.Part_IsEdit != null)
            this.Part_IsEdit.SelectionStart = 0;
        }
      }
      else if (container != null)
      {
        this.IsDropDownOpen = false;
        if (this.Part_IsEdit != null)
          this.Part_IsEdit.SelectionStart = 0;
      }
      if (this.Part_IsEdit != null)
        this.Part_IsEdit.Focus();
      if (this.IsEditable && !this.AllowMultiSelect)
      {
        this.SelectedIndex = -1;
        for (int index4 = 0; index4 < this.Items.Count; ++index4)
        {
          string str = this.ItemsSource == null ? (this.Items[index4] as ComboBoxItemAdv).Content.ToString() : (!string.IsNullOrEmpty(this.DisplayMemberPath) ? this.GetDisplayMemberValue(this.Items[index4]).ToString() : this.Items[index4].ToString());
          if (this.Part_IsEdit != null && this.Part_IsEdit.Text == str)
          {
            this.SelectedIndex = index4;
            break;
          }
        }
        if (container == null && this.AutoCompleteMode == AutoCompleteModes.Suggest && this.Items.Count < 1)
          this.IsDropDownOpen = true;
      }
      this.GetBindingExpression(ComboBoxAdv.TextProperty)?.UpdateSource();
    }
    else if (e.Key == Key.Escape)
    {
      this.KeyPressed = e.Key;
      if (this.IsDropDownOpen)
      {
        this.IsDropDownOpen = false;
        e.Handled = true;
      }
      if (this.Part_IsEdit != null && this.IsEditable)
      {
        if (!this.AllowMultiSelect)
        {
          if (this.SelectedIndex == -1)
            this.Part_IsEdit.Text = string.Empty;
          else
            this.UpdateSelectedText(this.SelectedItem);
          this.Part_IsEdit.SelectionStart = string.IsNullOrEmpty(this.Part_IsEdit.Text) ? 0 : this.Part_IsEdit.Text.Length;
        }
        else if (this.AllowMultiSelect && this.EnableToken)
        {
          this.Part_IsEdit.Text = string.Empty;
          if (this.IsTokenRemoved && this.EnableOKCancel)
            this.ResetTokenItems();
        }
        Keyboard.Focus((IInputElement) this.Part_IsEdit);
      }
      this.IsTokenRemoved = false;
    }
    else if (e.Key == Key.Tab && e.KeyboardDevice.Modifiers == ModifierKeys.None)
    {
      this.KeyPressed = Key.Return;
      if (this.IsDropDownOpen)
        this.IsDropDownOpen = false;
      if (this.Part_IsEdit != null)
      {
        if (this.IsEditable)
          this.ValidateItem(this.KeyPressed);
        if (!this.AllowMultiSelect)
        {
          if (!string.IsNullOrEmpty(this.Part_IsEdit.Text))
            this.Part_IsEdit.SelectionStart = this.Part_IsEdit.Text.Length;
          this.Part_IsEdit.Focus();
        }
      }
    }
    else if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.Tab)
    {
      if (this.AllowMultiSelect && this.IsEditable && this.EnableToken)
        this.Focusable = false;
    }
    else if (e.Key == Key.Space)
    {
      if (this.IsDropDownOpen && this.AllowMultiSelect && e.OriginalSource is ComboBoxItemAdv)
      {
        if (this.AllowSelectAll && this.SelectAllItem.IsHighlighted)
        {
          ComboBoxItemAdv selectAllItem = this.SelectAllItem;
          bool? isChecked = this.SelectAllItem.CheckBox.IsChecked;
          int num = (!isChecked.GetValueOrDefault() ? 1 : (!isChecked.HasValue ? 1 : 0)) != 0 ? 1 : 0;
          selectAllItem.IsSelected = num != 0;
        }
        else
        {
          ComboBoxItemAdv comboBoxItemAdv5;
          ComboBoxItemAdv comboBoxItemAdv6 = comboBoxItemAdv5 = this.ItemContainerGenerator.ContainerFromIndex(index1) as ComboBoxItemAdv;
          if (comboBoxItemAdv6 != null && comboBoxItemAdv6.CheckBox != null)
          {
            CheckBox checkBox = comboBoxItemAdv6.CheckBox;
            bool? isChecked = comboBoxItemAdv6.CheckBox.IsChecked;
            bool? nullable = isChecked.HasValue ? new bool?(!isChecked.GetValueOrDefault()) : new bool?();
            checkBox.IsChecked = nullable;
          }
        }
      }
    }
    else if (e.Key == Key.F4)
    {
      if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.None)
      {
        this.IsDropDownOpen = !this.IsDropDownOpen;
        e.Handled = true;
      }
    }
    else if (e.Key == Key.Home && this.IsDropDownOpen)
    {
      this.KeyPressed = e.Key;
      ComboBoxItemAdv comboBoxItemAdv = this.ItemContainerGenerator.ContainerFromIndex(0) as ComboBoxItemAdv;
      if (!this.AllowMultiSelect && this.Items.Count > 0 && this.IsEditable && this.Part_IsEdit != null)
      {
        if (this.ItemsSource != null)
          this.UpdateSelectedText(comboBoxItemAdv.Content);
        else
          this.UpdateSelectedText((object) comboBoxItemAdv);
        comboBoxItemAdv.IsHighlighted = true;
      }
      if (this.AllowSelectAll && this.AllowMultiSelect)
      {
        this.SelectAllItem.Focus();
      }
      else
      {
        this.ClearSelection();
        comboBoxItemAdv?.Focus();
      }
    }
    else if (e.Key == Key.End && this.IsDropDownOpen)
    {
      this.KeyPressed = e.Key;
      this.DropDownScrollBar.ScrollToEnd();
      this.DropDownScrollBar.UpdateLayout();
      ComboBoxItemAdv comboBoxItemAdv = (ComboBoxItemAdv) null;
      if (this.Items.Count > 0)
        comboBoxItemAdv = this.ItemContainerGenerator.ContainerFromIndex(this.Items.Count - 1) as ComboBoxItemAdv;
      if (!this.AllowMultiSelect && this.IsEditable && this.Part_IsEdit != null)
      {
        if (this.ItemsSource != null)
          this.UpdateSelectedText(comboBoxItemAdv.Content);
        else
          this.UpdateSelectedText((object) comboBoxItemAdv);
        comboBoxItemAdv.IsHighlighted = true;
      }
      this.ClearSelection();
      comboBoxItemAdv?.Focus();
    }
    else if (char.IsLetter(e.Key.ToString().ToCharArray()[0]) && (this.ItemsSource != null && this.AutoCompleteMode == AutoCompleteModes.None || this.ItemsSource == null))
    {
      if (!this.IsDropDownOpen && this.Part_IsEdit != null && !this.Part_IsEdit.IsVisible && this.IsTextSearchEnabled)
      {
        if (this.timer != null)
          this.timer.Stop();
        this._keypressed = true;
      }
      if (this.IsDropDownOpen && this.IsTextSearchEnabled && this.ItemContainerGenerator.ContainerFromIndex(0) is ComboBoxItemAdv comboBoxItemAdv)
        comboBoxItemAdv.Focus();
    }
    if (this.AutoCompleteMode == AutoCompleteModes.Suggest && this.IsEditable && this.Part_IsEdit != null)
      this.oldFilter = this.Part_IsEdit.Text;
    this.KeyPressed = Key.None;
    base.OnPreviewKeyDown(e);
  }

  protected override void OnKeyUp(KeyEventArgs e)
  {
    if (e.Key == Key.Return && this.IsEditable)
    {
      this.KeyPressed = Key.Return;
      if (this.Part_IsEdit != null && !string.IsNullOrEmpty(this.Part_IsEdit.Text))
        this.ValidateItem(this.KeyPressed);
      if (this.Part_IsEdit != null)
      {
        this.Part_IsEdit.Focus();
        if (this.AllowMultiSelect && this.EnableToken)
          this.Part_IsEdit.SelectionStart = this.Part_IsEdit.Text.Length;
      }
    }
    if (this.ItemsSource != null && this.AutoCompleteMode == AutoCompleteModes.Suggest && this.IsEditable && this.Part_IsEdit != null && this.Part_IsEdit.Text != this.oldFilter)
    {
      this.RefreshFilter();
      this.IsDropDownOpen = true;
      this.Part_IsEdit.SelectionStart = int.MaxValue;
    }
    this.KeyPressed = Key.None;
    base.OnKeyUp(e);
  }

  internal void RefreshFilter()
  {
    if (this.ItemsSource == null)
      return;
    ICollectionView defaultView = CollectionViewSource.GetDefaultView((object) this.ItemsSource);
    defaultView.Filter -= new Predicate<object>(this.FilterItem);
    defaultView.Filter += new Predicate<object>(this.FilterItem);
    defaultView.Refresh();
    if (this.NoRecords == null)
      return;
    if (this.Items.Count == 0)
    {
      this.NoRecords.Visibility = Visibility.Visible;
      if (!this.AllowMultiSelect || !this.AllowSelectAll)
        return;
      this.SelectAllItem.Visibility = Visibility.Collapsed;
    }
    else
    {
      this.NoRecords.Visibility = Visibility.Collapsed;
      if (!this.AllowMultiSelect || !this.AllowSelectAll)
        return;
      this.SelectAllItem.Visibility = Visibility.Visible;
    }
  }

  internal void RemoveToken(object tokenItem)
  {
    int index1 = -1;
    for (int index2 = 0; index2 < this.SelItemsInternal.Count; ++index2)
    {
      string str = this.ItemsSource == null ? (this.SelItemsInternal[index2] as ComboBoxItemAdv).Content.ToString() : (!string.IsNullOrEmpty(this.DisplayMemberPath) ? this.GetDisplayMemberValue(this.SelItemsInternal[index2]).ToString() : this.SelItemsInternal[index2].ToString());
      if ((tokenItem as ContentControl).Content.ToString().Equals(str))
      {
        index1 = index2;
        break;
      }
    }
    this.IsTokenRemoved = true;
    this.lastRowTokenItemIndex = 0;
    if (index1 < this.SelItemsInternal.Count)
      this.SelItemsInternal.RemoveAt(index1);
    this.UpdateSelectionBox();
    if (this.Part_IsEdit == null)
      return;
    this.Part_IsEdit.Text = string.Empty;
    if (this.IsDropDownOpen)
      return;
    this.Part_IsEdit.Focus();
    this.Part_IsEdit.SelectionStart = 0;
  }

  internal IList GetDisplayMemberItems()
  {
    IList displayMemberItems = (IList) new List<string>();
    if (!string.IsNullOrEmpty(this.DisplayMemberPath))
    {
      foreach (object obj in (IEnumerable) this.Items)
      {
        object displayMemberValue = this.GetDisplayMemberValue(obj);
        if (displayMemberValue != null && !displayMemberItems.Contains((object) displayMemberValue.ToString()))
          displayMemberItems.Add((object) displayMemberValue.ToString());
      }
    }
    return displayMemberItems;
  }

  internal object GetDisplayMemberValue(object item)
  {
    PropertyInfo property = item.GetType().GetProperty(this.DisplayMemberPath);
    return !(property == (PropertyInfo) null) ? property.GetValue(item, (object[]) null) : (object) null;
  }

  internal void AddToken(string text, Key pressedkey)
  {
    List<string> stringList = new List<string>();
    if (this.ItemsSource == null)
    {
      foreach (object obj in (IEnumerable) this.Items)
        stringList.Add((obj as ComboBoxItemAdv).Content.ToString());
    }
    IList list = this.ItemsSource == null ? (IList) stringList : (!string.IsNullOrEmpty(this.DisplayMemberPath) ? this.GetDisplayMemberItems() : (IList) this.Items);
    if (list.Contains((object) text) && pressedkey == Key.Return)
    {
      foreach (object obj in (IEnumerable) this.Items)
      {
        string str1 = this.ItemsSource == null ? (obj as ComboBoxItemAdv).Content.ToString() : (!string.IsNullOrEmpty(this.DisplayMemberPath) ? this.GetDisplayMemberValue(obj).ToString() : obj.ToString());
        if (str1 != null)
        {
          foreach (string str2 in (IEnumerable) list)
          {
            if (str2.Equals(text) && text.Equals(str1) && !this.SelItemsInternal.Contains(obj))
              this.SelItemsInternal.Add(obj);
          }
          if (this.EnableOKCancel && text.Equals(str1) && this.AddedTokenItems != null && !this.AddedTokenItems.Contains(str1))
            this.AddedTokenItems.Add(str1.ToString());
        }
      }
      this.SelectedItems = (IEnumerable) this.SelItemsInternal;
      this.UpdateSelectionBox();
    }
    if (this.Part_IsEdit == null)
      return;
    this.Part_IsEdit.Text = string.Empty;
  }

  private void timer_Tick(object sender, EventArgs e)
  {
    if (!(sender is DispatcherTimer))
      return;
    (sender as DispatcherTimer).Stop();
    this.searchText = "";
  }

  private void ValidateItem(Key pressedkey)
  {
    for (int index = 0; index < this.Items.Count; ++index)
    {
      string text = this.ItemsSource == null ? (this.Items[index] as ComboBoxItemAdv).Content.ToString() : (!string.IsNullOrEmpty(this.DisplayMemberPath) ? this.GetDisplayMemberValue(this.Items[index]).ToString() : this.Items[index].ToString());
      if (this.Part_IsEdit != null && (this.Part_IsEdit.Text == text || this.Part_IsEdit.Text.ToLower() == text.ToLower()))
      {
        if (this.AllowMultiSelect)
        {
          this.AddToken(text, pressedkey);
        }
        else
        {
          this.SelectedIndex = index;
          break;
        }
      }
      else if (!this.AllowMultiSelect)
      {
        this.SelectedIndex = -1;
        if (this.SelectedItems != null && this.SelItemsInternal.Count > index)
        {
          this.SelItemsInternal.RemoveAt(index);
          this.SelectedItems = (IEnumerable) this.SelItemsInternal;
        }
      }
    }
    if (this.Part_IsEdit != null)
    {
      if (this.AllowMultiSelect)
      {
        if (this.AutoCompleteMode != AutoCompleteModes.Suggest || this.Items.Count >= 1)
          this.Part_IsEdit.Text = string.Empty;
      }
      else if (!this.AllowMultiSelect)
      {
        if (this.IsEditable && this.SelItemsInternal.Count <= 0)
          this.Part_IsEdit.Text = string.Empty;
        else if (this.IsEditable && this.SelectedIndex != -1)
          this.Part_IsEdit.Text = this.ItemsSource != null || !(this.Items[this.SelectedIndex] is ComboBoxItemAdv) ? (!string.IsNullOrEmpty(this.DisplayMemberPath) ? this.GetDisplayMemberValue(this.Items[this.SelectedIndex]).ToString() : this.Items[this.SelectedIndex].ToString()) : (this.Items[this.SelectedIndex] as ComboBoxItemAdv).Content.ToString();
      }
    }
    if (!this.AllowMultiSelect || this.SelectedItems != null || this.SelItemsInternal.Count <= 0)
      return;
    ObservableCollection<object> observableCollection = new ObservableCollection<object>();
    foreach (object obj in (IEnumerable) this.SelItemsInternal)
      observableCollection.Add(obj);
    this.SelectedItems = (IEnumerable) observableCollection;
  }

  private void UpdateSelectedText(object item)
  {
    if (this.Part_IsEdit == null)
      return;
    if (this.ItemsSource != null)
    {
      if (!string.IsNullOrEmpty(this.DisplayMemberPath))
        this.Part_IsEdit.Text = this.GetDisplayMemberValue(item).ToString();
      else
        this.Part_IsEdit.Text = item.ToString();
    }
    else
      this.Part_IsEdit.Text = (item as ComboBoxItemAdv).Content.ToString();
  }

  protected internal virtual ObservableCollection<object> OnItemChecked(
    object checkedItem,
    ObservableCollection<object> selectedItems)
  {
    return selectedItems;
  }

  protected internal virtual ObservableCollection<object> OnItemUnchecked(
    object unCheckedItem,
    ObservableCollection<object> selectedItems)
  {
    return selectedItems;
  }

  private void ClearSelection()
  {
    if (this.AllowMultiSelect && this.AllowSelectAll && this.SelectAllItem != null && this.SelectAllItem.IsHighlighted)
      this.SelectAllItem.IsHighlighted = false;
    int count = this.Items.Count;
    for (int index = 0; index < count; ++index)
    {
      object obj = this.Items[index];
      if (obj is ComboBoxItemAdv && (obj as ComboBoxItemAdv).IsHighlighted)
        (obj as ComboBoxItemAdv).IsHighlighted = false;
      else if (this.GetItemContainer(obj) != null && this.GetItemContainer(obj).IsHighlighted)
        this.GetItemContainer(obj).IsHighlighted = false;
    }
  }

  private static void OnAllowMultiSelectChanged(
    object sender,
    DependencyPropertyChangedEventArgs args)
  {
    ComboBoxAdv comboBoxAdv = sender as ComboBoxAdv;
    comboBoxAdv.Focusable = true;
    if (comboBoxAdv != null && comboBoxAdv.SelItemsInternal != null)
    {
      if (!comboBoxAdv.AllowMultiSelect)
      {
        if (comboBoxAdv.SelectedItem == null && comboBoxAdv.SelectedIndex < 0 && comboBoxAdv.SelItemsInternal.Count > 0)
        {
          comboBoxAdv.SelectedItem = comboBoxAdv.SelItemsInternal[0];
          comboBoxAdv.SelectionBoxItem = !(comboBoxAdv.SelectedItem is ComboBoxItemAdv) ? comboBoxAdv.SelectedItem : (comboBoxAdv.SelectedItem as ComboBoxItemAdv).Content;
        }
        if (comboBoxAdv.SelectedItem != null && comboBoxAdv.SelectedIndex < 0 && comboBoxAdv.SelItemsInternal.Count <= 0)
          comboBoxAdv.SelectedItem = (object) null;
        comboBoxAdv.SelItemsInternal.Clear();
        foreach (object obj in (IEnumerable) comboBoxAdv.Items)
        {
          if (comboBoxAdv.ItemContainerGenerator.ContainerFromItem(obj) is ComboBoxItemAdv comboBoxItemAdv && comboBoxAdv.SelectedItem != null && comboBoxItemAdv.Content != null && !comboBoxItemAdv.Content.Equals(comboBoxAdv.SelectedItem))
            comboBoxItemAdv.IsSelected = false;
        }
        comboBoxAdv.SelectionBoxItem = !(comboBoxAdv.SelectedItem is ComboBoxItemAdv) ? comboBoxAdv.SelectedItem : (comboBoxAdv.SelectedItem as ComboBoxItemAdv).Content;
        if (comboBoxAdv.EnableToken)
          comboBoxAdv.MinHeight = comboBoxAdv.Height;
      }
      if (comboBoxAdv.SelectedItem != null && comboBoxAdv.SelItemsInternal.Count <= 0)
        comboBoxAdv.SelItemsInternal.Add(comboBoxAdv.SelectedItem);
      comboBoxAdv.IsDropDownOpen = false;
      comboBoxAdv.UpdateSelectionBox();
      comboBoxAdv.UpdateDefaultTextVisibility();
      comboBoxAdv.UpdateSelectMode();
      comboBoxAdv.IsTokenRemoved = false;
    }
    if (!comboBoxAdv.IsLoaded)
      return;
    comboBoxAdv.OnFilterApplied();
  }

  private static void OnAutoCompleteModeChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    ComboBoxAdv comboBoxAdv = sender as ComboBoxAdv;
    if (!comboBoxAdv.IsLoaded)
      return;
    if (comboBoxAdv.IsEditable && comboBoxAdv.AutoCompleteMode == AutoCompleteModes.Suggest && comboBoxAdv.ItemsSource != null)
    {
      ICollectionView defaultView = CollectionViewSource.GetDefaultView((object) comboBoxAdv.ItemsSource);
      defaultView.Filter -= new Predicate<object>(comboBoxAdv.FilterItem);
      defaultView.Filter += new Predicate<object>(comboBoxAdv.FilterItem);
    }
    if (comboBoxAdv.AutoCompleteMode == AutoCompleteModes.None && comboBoxAdv.EnableOKCancel)
      comboBoxAdv.RefreshFilter();
    if (comboBoxAdv.AutoCompleteMode == AutoCompleteModes.Suggest || comboBoxAdv.ItemsSource == null)
      return;
    CollectionViewSource.GetDefaultView((object) comboBoxAdv.ItemsSource).Filter -= new Predicate<object>(comboBoxAdv.FilterItem);
  }

  private static void OnEnableTokenChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs e)
  {
    ComboBoxAdv comboBoxAdv = sender as ComboBoxAdv;
    comboBoxAdv.Focusable = true;
    if (comboBoxAdv.EnableToken)
    {
      if (comboBoxAdv.AllowMultiSelect && comboBoxAdv.IsEditable)
      {
        if (comboBoxAdv.Part_IsEdit != null)
          comboBoxAdv.Part_IsEdit.Text = string.Empty;
        comboBoxAdv.UpdateToken();
        comboBoxAdv.UpdateSelectMode();
        if (comboBoxAdv.IsEditDefaultText != null)
          comboBoxAdv.IsEditDefaultText.Visibility = Visibility.Collapsed;
      }
    }
    else
    {
      if (comboBoxAdv.AutoCompleteMode == AutoCompleteModes.Suggest)
        comboBoxAdv.RefreshFilter();
      comboBoxAdv.UpdateSelectionBox();
      comboBoxAdv.UpdateSelectMode();
      comboBoxAdv.MinHeight = comboBoxAdv.Height;
    }
    if (comboBoxAdv.IsEditDefaultText != null && comboBoxAdv.AllowMultiSelect)
    {
      if (!comboBoxAdv.EnableToken)
        comboBoxAdv.IsEditDefaultText.Visibility = Visibility.Collapsed;
      else if (comboBoxAdv.ShowDefaultText() && comboBoxAdv.IsEditable)
        comboBoxAdv.IsEditDefaultText.Visibility = Visibility.Visible;
    }
    comboBoxAdv.UpdateDefaultTextVisibility();
    if (!comboBoxAdv.IsLoaded)
      return;
    comboBoxAdv.OnFilterApplied();
  }

  private static void OnIsEditableChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs e)
  {
    ComboBoxAdv comboBoxAdv = sender as ComboBoxAdv;
    comboBoxAdv.Focusable = true;
    if (comboBoxAdv.AllowMultiSelect && comboBoxAdv.EnableToken && !comboBoxAdv.IsEditable)
    {
      comboBoxAdv.UpdateSelectMode();
      comboBoxAdv.MinHeight = comboBoxAdv.Height;
    }
    else if (comboBoxAdv.AllowMultiSelect && comboBoxAdv.EnableToken && comboBoxAdv.IsEditable)
    {
      comboBoxAdv.UpdateToken();
      if (comboBoxAdv.IsEditDefaultText != null)
        comboBoxAdv.IsEditDefaultText.Visibility = Visibility.Collapsed;
      comboBoxAdv.UpdateSelectMode();
    }
    else if (!comboBoxAdv.AllowMultiSelect && comboBoxAdv.IsEditable && comboBoxAdv.SelectedIndex != -1)
      comboBoxAdv.UpdateSelectedText(comboBoxAdv.SelectedItem);
    comboBoxAdv.UpdateSelectionBox();
    if (comboBoxAdv.IsEditDefaultText != null)
    {
      if (!comboBoxAdv.IsEditable)
        comboBoxAdv.IsEditDefaultText.Visibility = Visibility.Collapsed;
      else if (comboBoxAdv.ShowDefaultText() && (comboBoxAdv.EnableToken || !comboBoxAdv.AllowMultiSelect && !comboBoxAdv.EnableToken && comboBoxAdv.IsEditable))
        comboBoxAdv.IsEditDefaultText.Visibility = Visibility.Visible;
    }
    if (!comboBoxAdv.IsLoaded)
      return;
    comboBoxAdv.OnFilterApplied();
  }

  private void OnFilterApplied()
  {
    if (this.ItemsSource == null)
      return;
    if (!this.IsEditable || this.AllowMultiSelect && !this.EnableToken && this.AutoCompleteMode == AutoCompleteModes.Suggest)
    {
      CollectionViewSource.GetDefaultView((object) this.ItemsSource).Filter -= new Predicate<object>(this.FilterItem);
    }
    else
    {
      if (!this.IsEditable || this.AutoCompleteMode != AutoCompleteModes.Suggest)
        return;
      ICollectionView defaultView = CollectionViewSource.GetDefaultView((object) this.ItemsSource);
      defaultView.Filter -= new Predicate<object>(this.FilterItem);
      defaultView.Filter += new Predicate<object>(this.FilterItem);
    }
  }

  private void UpdateSelectionOnDropDownOpen(ComboBoxAdv instance)
  {
    if (instance == null || !this.IsDropDownOpen)
      return;
    instance.ClearSelection();
    if (instance.popup != null && !instance.AllowMultiSelect && instance.SelectedIndex >= 0)
    {
      this.DropDownScrollBar.ScrollToVerticalOffset((double) instance.SelectedIndex);
      if (instance.SelectedItem == null)
        return;
      if (!(instance.SelectedItem is ComboBoxItemAdv comboBoxItemAdv))
        comboBoxItemAdv = instance.GetItemContainer(instance.SelectedItem);
      if (comboBoxItemAdv == null || comboBoxItemAdv == null)
        return;
      comboBoxItemAdv.IsHighlighted = true;
      comboBoxItemAdv.Focus();
    }
    else
    {
      if (instance.SelItemsInternal.Count <= 0)
        return;
      if (instance.AllowMultiSelect && instance.AllowSelectAll)
      {
        this.SelectAllItem.Focus();
      }
      else
      {
        int index = instance.SelItemsInternal.Count - 1;
        this.DropDownScrollBar.ScrollToVerticalOffset((double) instance.Items.IndexOf(instance.SelItemsInternal[index]));
        if (instance.SelItemsInternal == null)
          return;
        if (!(instance.SelItemsInternal[index] is ComboBoxItemAdv itemContainer))
          itemContainer = instance.GetItemContainer(instance.SelItemsInternal[index]);
        if (itemContainer == null || itemContainer == null)
          return;
        itemContainer.Focus();
      }
    }
  }

  private void OnDropDownClosed()
  {
    if (this.DropDownClosed == null)
      return;
    this.DropDownClosed((object) this, new EventArgs());
  }

  private static void OnIsDropDownOpenChanged(
    object sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (sender is ComboBoxAdv comboBoxAdv)
    {
      comboBoxAdv.UpdateSelectionOnDropDownOpen(comboBoxAdv);
      if (comboBoxAdv.IsDropDownOpen && comboBoxAdv.DropDownOpened != null)
        comboBoxAdv.DropDownOpened(sender, new EventArgs());
    }
    if ((bool) args.NewValue)
    {
      Mouse.Capture((IInputElement) comboBoxAdv, CaptureMode.SubTree);
      if (!comboBoxAdv.AllowMultiSelect)
      {
        if (comboBoxAdv.SelectAllItem != null)
          comboBoxAdv.SelectAllItem.Visibility = Visibility.Collapsed;
        if (comboBoxAdv.OKButton != null)
          comboBoxAdv.OKButton.Visibility = Visibility.Collapsed;
        if (comboBoxAdv.CancelButton != null)
          comboBoxAdv.CancelButton.Visibility = Visibility.Collapsed;
      }
      else if (comboBoxAdv.AllowSelectAll && comboBoxAdv.SelectAllItem != null)
      {
        if (comboBoxAdv.Items.Count == 0)
          comboBoxAdv.SelectAllItem.Visibility = Visibility.Collapsed;
        else
          comboBoxAdv.SelectAllItem.Visibility = Visibility.Visible;
        if (comboBoxAdv.DropDownClosed != null && !comboBoxAdv.IsDropDownOpen)
          comboBoxAdv.OnDropDownClosed();
      }
      if (comboBoxAdv.Part_IsEdit != null && !comboBoxAdv.AllowMultiSelect && comboBoxAdv.IsEditable && comboBoxAdv.Part_IsEdit.Text != string.Empty)
      {
        if (comboBoxAdv.IsLoaded)
          comboBoxAdv.OnFilterApplied();
        comboBoxAdv.Part_IsEdit.SelectionStart = 0;
        comboBoxAdv.Part_IsEdit.SelectionLength = comboBoxAdv.Part_IsEdit.Text.Length;
      }
      if (comboBoxAdv.Part_IsEdit != null && !comboBoxAdv.AllowMultiSelect && comboBoxAdv.IsEditable && !comboBoxAdv.EnableToken && comboBoxAdv.AutoCompleteMode == AutoCompleteModes.Suggest)
        comboBoxAdv.RefreshFilter();
      if (!comboBoxAdv.AllowMultiSelect || !comboBoxAdv.IsEditable || !comboBoxAdv.EnableToken || comboBoxAdv.AutoCompleteMode != AutoCompleteModes.Suggest)
        return;
      comboBoxAdv.RefreshFilter();
    }
    else
    {
      if (comboBoxAdv.popup != null && comboBoxAdv.popup.Child is FrameworkElement child && child.MinWidth != comboBoxAdv.ActualWidth)
        child.MinWidth = comboBoxAdv.ActualWidth;
      if (comboBoxAdv.AllowMultiSelect && comboBoxAdv.EnableOKCancel)
      {
        if (comboBoxAdv.KeyPressed == Key.Return)
        {
          comboBoxAdv.SelectedItems = (IEnumerable) comboBoxAdv.SelItemsInternal;
          if (comboBoxAdv.EnableToken)
            comboBoxAdv.UpdateToken();
          comboBoxAdv.UpdateSelectionBox();
        }
        if (comboBoxAdv.KeyPressed != Key.Return || comboBoxAdv.KeyPressed == Key.None)
          comboBoxAdv.ResetSelectedItems();
        comboBoxAdv.SelectItems();
      }
      if (comboBoxAdv.DropDownClosed != null && !comboBoxAdv.IsDropDownOpen)
        comboBoxAdv.OnDropDownClosed();
      comboBoxAdv.ChangedItems.Clear();
      if (comboBoxAdv != null && comboBoxAdv.IsKeyboardFocusWithin)
        comboBoxAdv.Focus();
      if (comboBoxAdv != null && Mouse.Captured == comboBoxAdv)
        Mouse.Capture((IInputElement) null);
      if (comboBoxAdv != null && comboBoxAdv.IsDropDownOpen)
        comboBoxAdv.OnDropDownClosed();
      if (comboBoxAdv.Part_IsEdit == null || comboBoxAdv.AllowMultiSelect || !comboBoxAdv.IsEditable || comboBoxAdv.AutoCompleteMode != AutoCompleteModes.Suggest)
        return;
      if (comboBoxAdv.SelectedItem != null)
      {
        comboBoxAdv.UpdateSelectedText(comboBoxAdv.SelectedItem);
        comboBoxAdv.RefreshFilter();
      }
      else
        comboBoxAdv.RefreshFilter();
    }
  }

  private static void OnSelectedValueDelimiterChanged(
    object sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(sender is ComboBoxAdv comboBoxAdv))
      return;
    comboBoxAdv.UpdateSelectionBox();
    comboBoxAdv.UpdateSelectMode();
  }

  private void OnTextChanged(string searchText)
  {
    if (!this.IsTextSearchEnabled || (this.ItemsSource == null || this.AutoCompleteMode != AutoCompleteModes.None) && this.ItemsSource != null)
      return;
    List<object> source1 = new List<object>();
    object obj1 = (object) null;
    if (this.Items.Count > 0)
      obj1 = this.Items[0];
    bool flag1 = obj1 is string;
    bool flag2 = this.DisplayMemberPath.Equals(string.Empty);
    if (!this._keypressed)
      return;
    if (this.Part_IsEdit != null && this.IsEditable && !string.IsNullOrEmpty(searchText))
      this.searchText = this.Part_IsEdit.Text;
    if (this.Items == null)
      return;
    if (this.ItemsSource == null && this.Items.Count > 0)
    {
      if (searchText != "")
      {
        IEnumerable<ComboBoxItemAdv> source2 = !this.IsTextSearchCaseSensitive ? this.Items.OfType<ComboBoxItemAdv>().Where<ComboBoxItemAdv>((System.Func<ComboBoxItemAdv, bool>) (item => item.Content.ToString().ToUpper().StartsWith(searchText, StringComparison.OrdinalIgnoreCase))) : this.Items.OfType<ComboBoxItemAdv>().Where<ComboBoxItemAdv>((System.Func<ComboBoxItemAdv, bool>) (item => item.Content.ToString().StartsWith(searchText, StringComparison.Ordinal)));
        if (!this.IsEditable)
        {
          char? oldTempChar = this.oldTempChar;
          int newTempChar = (int) this.newTempChar;
          if (((int) oldTempChar.GetValueOrDefault() != newTempChar ? 0 : (oldTempChar.HasValue ? 1 : 0)) != 0)
          {
            if (source2.Count<ComboBoxItemAdv>() <= 0)
              return;
            if (this.charindex + 1 >= source2.Count<ComboBoxItemAdv>())
              this.charindex = -1;
            this.SelectedIndex = this.Items.IndexOf((object) source2.ToList<ComboBoxItemAdv>()[++this.charindex]);
            return;
          }
        }
        if (source2.Count<ComboBoxItemAdv>() > 0)
        {
          this._keypressed = false;
          this.SelectedItem = (object) null;
          this.SelectedIndex = this.Items.IndexOf((object) source2.ToList<ComboBoxItemAdv>()[0]);
        }
        else
          this.SelectedItem = (object) null;
      }
      else
        this._keypressed = false;
    }
    else
    {
      if (this.ItemsSource == null)
        return;
      int num1 = 0;
      if (flag1)
      {
        foreach (object obj2 in (IEnumerable) this.Items)
        {
          if (obj2 != null)
            source1.Add((object) $"{(string) obj2},{(object) num1}");
        }
      }
      else
      {
        foreach (object obj3 in (IEnumerable) this.Items)
        {
          if (!flag2)
          {
            PropertyInfo property = obj3.GetType().GetProperty(this.DisplayMemberPath);
            object obj4 = property == (PropertyInfo) null ? (object) null : property.GetValue(obj3, (object[]) null);
            if (obj4 != null)
              source1.Add((object) $"{obj4.ToString()},{(object) num1}");
          }
          else if (!this.SelectedValuePath.Equals(""))
          {
            object obj5 = obj3.GetType().GetProperty(this.SelectedValuePath).GetValue(obj3, (object[]) null);
            if (obj5 != null)
              source1.Add((object) $"{obj5.ToString()},{(object) num1}");
          }
          else
            source1.Add((object) $"{obj3},{(object) num1}");
          ++num1;
        }
      }
      IEnumerable<object> source3 = !this.IsTextSearchCaseSensitive ? source1.Cast<object>().Where<object>((System.Func<object, bool>) (item => searchText != null && item.ToString().StartsWith(searchText, StringComparison.OrdinalIgnoreCase))) : source1.Cast<object>().Where<object>((System.Func<object, bool>) (item => searchText != null && item.ToString().StartsWith(searchText, StringComparison.Ordinal)));
      if (!this.IsEditable)
      {
        char? oldTempChar = this.oldTempChar;
        int newTempChar = (int) this.newTempChar;
        if (((int) oldTempChar.GetValueOrDefault() != newTempChar ? 0 : (oldTempChar.HasValue ? 1 : 0)) != 0)
        {
          if (source3.Count<object>() <= 0)
            return;
          if (this.charindex + 1 >= source3.Count<object>())
            this.charindex = -1;
          this.SelectedIndex = int.Parse(source3.ToList<object>()[++this.charindex].ToString().Split(',')[1]);
          return;
        }
      }
      if (!this.IsEditable && source3.Count<object>() > 0)
      {
        this.charindex = 0;
        this.SelectedIndex = int.Parse(source3.ToList<object>()[this.charindex].ToString().Split(',')[1]);
      }
      else
      {
        if (source3.Count<object>() <= 0)
          return;
        this.charindex = 0;
        string[] source4 = source3.ToList<object>()[this.charindex].ToString().Split(',');
        int num2 = -1;
        for (int index = 0; index < ((IEnumerable<string>) source4).Count<string>(); ++index)
        {
          int result;
          if (int.TryParse(source4[index], out result))
          {
            num2 = result;
            break;
          }
        }
        this.oldTempChar = new char?(this.newTempChar);
        this.timer.Start();
        this._keypressed = false;
        this.SelectedItem = (object) null;
        this.SelectedIndex = num2;
      }
    }
  }

  private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
  {
    ComboBoxAdv comboBoxAdv = sender as ComboBoxAdv;
    if (comboBoxAdv.Part_IsEdit != null && string.IsNullOrEmpty((string) arg.NewValue))
    {
      comboBoxAdv.Part_IsEdit.Text = string.Empty;
    }
    else
    {
      if (comboBoxAdv.Part_IsEdit == null || comboBoxAdv.Part_IsEdit.Text.Equals(arg.NewValue.ToString()))
        return;
      comboBoxAdv.Part_IsEdit.Text = arg.NewValue.ToString();
    }
  }

  private static void OnSelectionBoxTemplateChanged(
    object sender,
    DependencyPropertyChangedEventArgs args)
  {
    ComboBoxAdv comboBoxAdv = sender as ComboBoxAdv;
    if (comboBoxAdv.DisplayMemberPath != null && comboBoxAdv.DisplayMemberPath != "" && comboBoxAdv.SelectionBoxTemplate != null)
      throw new XamlParseException("Cannot set both DisplayMemberPath and SelectionBoxTemplate");
  }

  internal void NotifyComboBoxItemAdvEnter(ComboBoxItemAdv item, bool state)
  {
    if (!this.IsDropDownOpen)
      return;
    if (!this.AllowMultiSelect)
    {
      ComboBoxItemAdv itemContainer = this.GetItemContainer(this.SelectedItem);
      if (itemContainer != null && !itemContainer.Equals((object) item) && itemContainer.IsHighlighted && !itemContainer.IsMouseOver)
        itemContainer.IsHighlighted = false;
    }
    else if (this.SelItemsInternal.Count - 1 >= 0)
    {
      ComboBoxItemAdv itemContainer = this.GetItemContainer(this.SelItemsInternal[this.SelItemsInternal.Count - 1]);
      if (itemContainer != null && this.SelItemsInternal != null && this.SelItemsInternal.Count > 0 && itemContainer.IsHighlighted && !itemContainer.IsMouseOver)
        itemContainer.IsHighlighted = false;
    }
    item.IsHighlighted = state;
    if (this.IsEditable || item.IsKeyboardFocusWithin || !state || !this.IsMouseMove())
      return;
    item.Focus();
  }

  internal bool IsMouseMove()
  {
    Point position = Mouse.GetPosition((IInputElement) this);
    if (!(position != this.lastMousePosition))
      return false;
    this.lastMousePosition = position;
    return true;
  }

  internal Brush InactiveBrush
  {
    get => (Brush) this.GetValue(ComboBoxAdv.InactiveBrushProperty);
    set => this.SetValue(ComboBoxAdv.InactiveBrushProperty, (object) value);
  }

  public bool IsEditable
  {
    get => (bool) this.GetValue(ComboBoxAdv.IsEditableProperty);
    set => this.SetValue(ComboBoxAdv.IsEditableProperty, (object) value);
  }

  public bool EnableToken
  {
    get => (bool) this.GetValue(ComboBoxAdv.EnableTokenProperty);
    set => this.SetValue(ComboBoxAdv.EnableTokenProperty, (object) value);
  }

  public AutoCompleteModes AutoCompleteMode
  {
    get => (AutoCompleteModes) this.GetValue(ComboBoxAdv.AutoCompleteModeProperty);
    set => this.SetValue(ComboBoxAdv.AutoCompleteModeProperty, (object) value);
  }

  public bool IsReadOnly
  {
    get => this.isReadOnly;
    set => this.isReadOnly = value;
  }

  public double MaxDropDownHeight
  {
    get => (double) this.GetValue(ComboBoxAdv.MaxDropDownHeightProperty);
    set => this.SetValue(ComboBoxAdv.MaxDropDownHeightProperty, (object) value);
  }

  public bool IsDropDownOpen
  {
    get => (bool) this.GetValue(ComboBoxAdv.IsDropDownOpenProperty);
    set => this.SetValue(ComboBoxAdv.IsDropDownOpenProperty, (object) value);
  }

  public DataTemplate SelectionBoxItemTemplate
  {
    get => (DataTemplate) this.GetValue(ComboBoxAdv.SelectionBoxItemTemplateProperty);
    internal set => this.SetValue(ComboBoxAdv.SelectionBoxItemTemplateProperty, (object) value);
  }

  public object SelectionBoxItem
  {
    get => this.GetValue(ComboBoxAdv.SelectionBoxItemProperty);
    internal set => this.SetValue(ComboBoxAdv.SelectionBoxItemProperty, value);
  }

  public string SelectionBoxItemStringFormat
  {
    get => (string) this.GetValue(ComboBoxAdv.SelectionBoxItemStringFormatProperty);
    internal set => this.SetValue(ComboBoxAdv.SelectionBoxItemStringFormatProperty, (object) value);
  }

  public bool AllowMultiSelect
  {
    get => (bool) this.GetValue(ComboBoxAdv.AllowMultiSelectProperty);
    set => this.SetValue(ComboBoxAdv.AllowMultiSelectProperty, (object) value);
  }

  public bool AllowSelectAll
  {
    get => (bool) this.GetValue(ComboBoxAdv.AllowSelectAllProperty);
    set => this.SetValue(ComboBoxAdv.AllowSelectAllProperty, (object) value);
  }

  public bool EnableOKCancel
  {
    get => (bool) this.GetValue(ComboBoxAdv.EnableOKCancelProperty);
    set => this.SetValue(ComboBoxAdv.EnableOKCancelProperty, (object) value);
  }

  public IEnumerable SelectedItems
  {
    get => (IEnumerable) this.GetValue(ComboBoxAdv.SelectedItemsProperty);
    set
    {
      if (value == null)
        value = (IEnumerable) new ObservableCollection<object>();
      this.SetValue(ComboBoxAdv.SelectedItemsProperty, (object) value);
    }
  }

  private ObservableCollection<ComboBoxItemAdv> GetSelectedItems()
  {
    ObservableCollection<ComboBoxItemAdv> selectedItems = new ObservableCollection<ComboBoxItemAdv>();
    foreach (object selectedItem in this.SelectedItems)
    {
      if (this.ItemContainerGenerator.ContainerFromItem(selectedItem) is ComboBoxItemAdv comboBoxItemAdv)
        selectedItems.Add(comboBoxItemAdv);
    }
    return selectedItems;
  }

  private void coll_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (!this.AllowMultiSelect || this.internalChange)
      return;
    ObservableCollection<object> observableCollection = (ObservableCollection<object>) null;
    if (this.ItemsSource != null)
      observableCollection = new ObservableCollection<object>(this.SelectedItems.Cast<object>());
    else if (this.Items != null)
      observableCollection = new ObservableCollection<object>((IEnumerable<object>) this.SelectedItems.OfType<ComboBoxItemAdv>());
    if (this.SelItemsInternal.Count <= 0)
    {
      this.internalSelect = true;
      this.SelectedItem = (object) null;
      this.internalSelect = false;
    }
    if (observableCollection.Count > 0 && observableCollection[0] is ComboBoxItemAdv)
    {
      int num = this.ItemContainerGenerator.IndexFromContainer((DependencyObject) (observableCollection[0] as ComboBoxItemAdv));
      if (this.SelectedIndex == num)
      {
        this.internalChange = true;
        if (this.AllowSelectAll)
        {
          List<object> addedItems = new List<object>();
          List<object> removedItems = new List<object>();
          if (e.Action == NotifyCollectionChangedAction.Add)
          {
            if (this.Items.Count > e.NewStartingIndex)
              addedItems.Add(this.Items[e.NewStartingIndex]);
            removedItems.Clear();
          }
          else if (e.Action == NotifyCollectionChangedAction.Remove)
          {
            if (this.Items.Count > e.OldStartingIndex)
              removedItems.Add(this.Items[e.OldStartingIndex]);
            addedItems.Clear();
          }
          this.FireOnSelectionChanged(removedItems, addedItems);
        }
      }
      else
      {
        this.SelectedIndex = num;
        this.internalChange = false;
      }
      if (observableCollection[0] is ComboBoxItemAdv && (observableCollection[0] as ComboBoxItemAdv).DataContext != null && this.ItemsSource != null)
        this.SelectedItem = (observableCollection[0] as ComboBoxItemAdv).DataContext;
      else
        this.SelectedItem = observableCollection[0];
      this.SelectionBoxItem = !(this.SelectedItem is ComboBoxItemAdv) ? this.SelectedItem : (this.SelectedItem as ComboBoxItemAdv).Content;
    }
    else if (this.SelItemsInternal.Count <= 0)
    {
      this.internalChange = false;
      this.SelectedIndex = -1;
    }
    if (!this.internalChange && this.SelectionChangedEvent != null)
    {
      base.OnSelectionChanged(this.SelectionChangedEvent);
      this.internalChange = true;
    }
    this.removeFlag = false;
    if (e.OldItems != null)
    {
      this.oldItem = e.OldItems[0];
      if (this.Items.Contains(e.OldItems[0]) && this.ItemContainerGenerator.ContainerFromItem(e.OldItems[0]) is ComboBoxItemAdv comboBoxItemAdv && comboBoxItemAdv.CheckBox != null)
      {
        this.internalSelect = true;
        comboBoxItemAdv.CheckBox.IsChecked = new bool?(false);
        this.internalSelect = false;
      }
    }
    else if (e.NewItems != null && this.Items.Contains(e.NewItems[0]))
    {
      this.newItem = e.NewItems[0];
      foreach (object obj1 in (IEnumerable) this.SelItemsInternal)
      {
        foreach (object obj2 in (IEnumerable) this.Items)
        {
          if (obj2 != null && obj2.Equals(obj1) && this.newItem.Equals(obj2))
          {
            if (this.itemcount >= 1 && (this.ItemContainerGenerator.ContainerFromItem(this.newItem) is ComboBoxItemAdv comboBoxItemAdv && comboBoxItemAdv.IsSelected || comboBoxItemAdv == null))
              this.removeFlag = true;
            ++this.itemcount;
          }
        }
      }
      if (this.removeFlag && this.SelItemsInternal.Count > 0)
        this.SelItemsInternal.Remove(e.NewItems[0]);
      this.SelectItems();
      this.itemcount = 0;
    }
    else
    {
      if (this.SelItemsInternal.Count > 0)
        this.SelItemsInternal.Remove(e.NewItems[0]);
      this.SelectItems();
    }
    this.UpdateSelectionBox();
    if (this.Part_IsEdit != null && !string.IsNullOrEmpty(this.Part_IsEdit.Text) && this.EnableToken)
      this.Part_IsEdit.Text = string.Empty;
    this.UpdateSelectMode();
    this.UpdateSelectAllItemState();
  }

  internal void SelectItems()
  {
    foreach (object obj in (IEnumerable) this.Items)
    {
      ComboBoxItemAdv comboBoxItemAdv = this.ItemsSource == null ? obj as ComboBoxItemAdv : this.ItemContainerGenerator.ContainerFromItem(obj) as ComboBoxItemAdv;
      this.internalSelect = true;
      if (comboBoxItemAdv != null && comboBoxItemAdv.CheckBox != null)
      {
        if (!this.SelItemsInternal.Contains(obj))
          comboBoxItemAdv.CheckBox.IsChecked = new bool?(false);
        else
          comboBoxItemAdv.CheckBox.IsChecked = new bool?(true);
      }
      this.internalSelect = false;
    }
    this.UpdateSelectAllItemState();
  }

  private static void OnSelectedItemsChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    (sender as ComboBoxAdv).OnSelectedItemsChanged(args);
  }

  internal IList SelItemsInternal { get; set; }

  internal List<object> ChangedItems { get; set; }

  private void FireOnSelectionChanged(List<object> removedItems, List<object> addedItems)
  {
    if (this.SelectionChangedEvent == null)
      return;
    this.SelectionChangedEvent = new SelectionChangedEventArgs(this.SelectionChangedEvent.RoutedEvent, (IList) removedItems, (IList) addedItems);
    base.OnSelectionChanged(this.SelectionChangedEvent);
  }

  internal void OnSelectedItemsChanged(DependencyPropertyChangedEventArgs args)
  {
    INotifyCollectionChanged newValue = (INotifyCollectionChanged) args.NewValue;
    if (newValue != null)
    {
      newValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.coll_CollectionChanged);
      newValue.CollectionChanged += new NotifyCollectionChangedEventHandler(this.coll_CollectionChanged);
    }
    this.SelItemsInternal = (IList) newValue;
    if (this.EnableToken && !this.EnableOKCancel && this.AllowMultiSelect && this.IsEditable && this.Part_IsEdit != null)
    {
      this.Part_IsEdit.Text = string.Empty;
      this.UpdateToken();
    }
    if (this.EnableOKCancel && this.SelItemsInternal.Count <= 0)
      this.SelectItems();
    if (this.AllowMultiSelect && this.SelItemsInternal != null)
    {
      foreach (object obj in (IEnumerable) this.SelItemsInternal)
      {
        if (obj != null && this.Items.Contains(obj))
        {
          this.SelectItems();
        }
        else
        {
          this.newItem = obj;
          this.SelectItems();
        }
      }
      this.UpdateSelectAllItemState();
    }
    if (this.internalChange)
      return;
    this.UpdateSelectionBox();
    if (this.AllowMultiSelect && !this.EnableOKCancel)
      return;
    if (this.SelItemsInternal != null && this.SelItemsInternal.Count <= 0)
      this.SelectItems();
    if (this.SelItemsInternal != null)
      return;
    this.SelectedItems = (IEnumerable) null;
    this.SelectItems();
    this.UpdateSelectionBox();
  }

  public string SelectedValueDelimiter
  {
    get => (string) this.GetValue(ComboBoxAdv.SelectedValueDelimiterProperty);
    set => this.SetValue(ComboBoxAdv.SelectedValueDelimiterProperty, (object) value);
  }

  public DataTemplate SelectionBoxTemplate
  {
    get => (DataTemplate) this.GetValue(ComboBoxAdv.SelectionBoxTemplateProperty);
    set => this.SetValue(ComboBoxAdv.SelectionBoxTemplateProperty, (object) value);
  }

  public string DefaultText
  {
    get => (string) this.GetValue(ComboBoxAdv.DefaultTextProperty);
    set => this.SetValue(ComboBoxAdv.DefaultTextProperty, (object) value);
  }

  public DataTemplate DropDownContentTemplate
  {
    get => (DataTemplate) this.GetValue(ComboBoxAdv.DropDownContentTemplateProperty);
    set => this.SetValue(ComboBoxAdv.DropDownContentTemplateProperty, (object) value);
  }

  public string Text
  {
    get => (string) this.GetValue(ComboBoxAdv.TextProperty);
    set => this.SetValue(ComboBoxAdv.TextProperty, (object) value);
  }

  protected override AutomationPeer OnCreateAutomationPeer()
  {
    return (AutomationPeer) new ComboBoxAdvAutomationPeer(this);
  }

  public void Dispose()
  {
    if (this.popup != null)
    {
      this.popup.Opened -= new EventHandler(this.Popup_Opened);
      this.popup.Closed -= new EventHandler(this.popup_Closed);
      this.popup = (Popup) null;
    }
    this.Part_IsEdit = this.GetTemplateChild("PART_Editable") as TextBox;
    if (this.Part_IsEdit != null)
    {
      this.Part_IsEdit.TextChanged -= new TextChangedEventHandler(this.Part_IsEdit_TextChanged);
      this.Part_IsEdit.PreviewKeyDown -= new KeyEventHandler(this.Part_IsEdit_PreviewKeyDown);
      this.Part_IsEdit.LostFocus -= new RoutedEventHandler(this.Part_IsEdit_LostFocus);
      this.Part_IsEdit.GotFocus -= new RoutedEventHandler(this.Part_IsEdit_GotFocus);
      this.RemoveLogicalChild((object) this.Part_IsEdit);
      this.Part_IsEdit = (TextBox) null;
    }
    if (this.tokenItemsControl != null)
    {
      this.tokenItemsControl.SizeChanged -= new SizeChangedEventHandler(this.TokenItemsControl_SizeChanged);
      this.tokenItemsControl = (ItemsControl) null;
    }
    if (this.tokenBorder != null)
    {
      this.tokenBorder.PreviewMouseDown -= new MouseButtonEventHandler(this.TokenBorder_PreviewMouseDown);
      this.tokenBorder = (Border) null;
    }
    this.SelectedItems = (IEnumerable) null;
    if (this.MainWindow != null)
    {
      this.MainWindow.LocationChanged -= new EventHandler(this.MainWindow_LocationChanged);
      this.MainWindow.Deactivated -= new EventHandler(this.MainWindow_Deactivated);
    }
    if (this.DropDownScrollBar != null)
      this.DropDownScrollBar = (ScrollViewer) null;
    if (this.toggleButton != null)
      this.toggleButton = (ToggleButton) null;
    if (this.selectedContent != null)
      this.selectedContent = (ContentPresenter) null;
    if (this.DropDownScrollBar != null)
      this.DropDownScrollBar = (ScrollViewer) null;
    if (this.IsEditDefaultText != null)
      this.IsEditDefaultText = (TextBox) null;
    if (this.defaultText != null)
      this.defaultText = (TextBlock) null;
    if (this.selectedItems != null)
      this.selectedItems = (ItemsControl) null;
    this.internalChange = false;
    this.internalSelect = false;
    if (this.oldItem != null)
      this.oldItem = (object) null;
    if (this.newItem != null)
      this.newItem = (object) null;
    this.itemcount = 0;
    this.removeFlag = false;
    if (this.OKButton != null)
      this.OKButton = (Button) null;
    if (this.CancelButton != null)
      this.CancelButton = (Button) null;
    if (this.searchText != null)
      this.searchText = (string) null;
    if (this.ChangedItems != null)
      this.ChangedItems = (List<object>) null;
    if (this.SelItemsInternal != null)
      this.SelItemsInternal = (IList) null;
    this.lastMousePosition = new Point(0.0, 0.0);
    if (this.timer != null)
    {
      this.timer.Tick -= new EventHandler(this.timer_Tick);
      this.timer = (DispatcherTimer) null;
    }
    this.SelectionChanged -= new SelectionChangedEventHandler(this.ComboBoxAdv_SelectionChanged);
    this.Loaded -= new RoutedEventHandler(this.ComboBoxAdv_Loaded);
    this.LostFocus -= new RoutedEventHandler(this.ComboBoxAdv_LostFocus);
    this.SelectionChangedEvent = (SelectionChangedEventArgs) null;
    if (this.Items.Count > 0)
    {
      this.ItemsSource = (IEnumerable) null;
      this.Items.Clear();
    }
    if (this.AddedTokenItems == null)
      return;
    this.AddedTokenItems = (List<string>) null;
  }

  IEnumerable ITextInputLayoutSelector.GetSelectedItems() => this.SelectedItems;
}
