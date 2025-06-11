// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarShareToggleButton
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Menus;

public class ToolbarShareToggleButton : ToggleButton
{
  private ContextMenu innerContextMenu;
  private DependencyPropertyDescriptor contextMenuPropertyDesc;
  private Rectangle indicator;
  public static readonly DependencyProperty OpenContextMenuOnCheckedProperty = DependencyProperty.Register(nameof (OpenContextMenuOnChecked), typeof (bool), typeof (ToolbarShareToggleButton), new PropertyMetadata((object) false, new PropertyChangedCallback(ToolbarShareToggleButton.OnOpenContextMenuOnCheckedPropertyChanged)));
  public static readonly DependencyProperty IsDropDownIconVisibleProperty = ToolbarButtonHelper.IsDropDownIconVisibleProperty.AddOwner(typeof (ToolbarShareToggleButton), new PropertyMetadata((object) true, new PropertyChangedCallback(ToolbarShareToggleButton.OnIsDropDownIconVisiblePropertyChanged)));
  public static readonly DependencyProperty IndicatorBrushProperty = ToolbarButtonHelper.IndicatorBrushProperty.AddOwner(typeof (ToolbarShareToggleButton));

  static ToolbarShareToggleButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ToolbarShareToggleButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ToolbarShareToggleButton)));
  }

  public ToolbarShareToggleButton()
  {
    this.Loaded += new RoutedEventHandler(this.ToolbarChildToggleButton_Loaded);
    this.Unloaded += new RoutedEventHandler(this.ToolbarChildToggleButton_Unloaded);
    ToolbarButtonHelper.RegisterIsKeyboardFocused((ButtonBase) this);
  }

  private ContextMenu InnerContextMenu
  {
    get => this.innerContextMenu;
    set
    {
      if (this.innerContextMenu == value)
        return;
      if (this.innerContextMenu != null)
      {
        this.innerContextMenu.Opened -= new RoutedEventHandler(this.ContextMenu_Opened);
        this.innerContextMenu.Closed -= new RoutedEventHandler(this.ContextMenu_Closed);
      }
      this.innerContextMenu = value;
      if (this.innerContextMenu != null)
      {
        this.innerContextMenu.Opened += new RoutedEventHandler(this.ContextMenu_Opened);
        this.innerContextMenu.Closed += new RoutedEventHandler(this.ContextMenu_Closed);
      }
      this.UpdateOpenContextMenuOnChecked();
    }
  }

  private Rectangle Indicator
  {
    get => this.indicator;
    set
    {
      if (this.indicator == value)
        return;
      if (this.indicator != null)
        this.indicator.SizeChanged -= new SizeChangedEventHandler(this.Indicator_SizeChanged);
      this.indicator = value;
      if (this.indicator != null)
        this.indicator.SizeChanged += new SizeChangedEventHandler(this.Indicator_SizeChanged);
      this.UpdateIndicatorSize();
    }
  }

  private void Indicator_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.UpdateIndicatorSize();
  }

  private void UpdateIndicatorSize()
  {
    if (this.Indicator == null)
      return;
    if (!(this.Indicator.Clip is RectangleGeometry rectangleGeometry) || rectangleGeometry.IsFrozen)
    {
      rectangleGeometry = new RectangleGeometry();
      this.Indicator.Clip = (Geometry) rectangleGeometry;
    }
    rectangleGeometry.Rect = new Rect(0.0, this.Indicator.ActualHeight / 2.0, this.Indicator.ActualWidth, this.Indicator.ActualHeight / 2.0);
  }

  private void ToolbarChildToggleButton_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.contextMenuPropertyDesc != null)
    {
      this.contextMenuPropertyDesc.RemoveValueChanged((object) this, new EventHandler(this.OnContextMenuPropertyChanged));
      this.contextMenuPropertyDesc = (DependencyPropertyDescriptor) null;
    }
    this.contextMenuPropertyDesc = DependencyPropertyDescriptor.FromProperty(FrameworkElement.ContextMenuProperty, typeof (FrameworkElement));
    this.contextMenuPropertyDesc.AddValueChanged((object) this, new EventHandler(this.OnContextMenuPropertyChanged));
    this.InnerContextMenu = this.ContextMenu;
  }

  private void ToolbarChildToggleButton_Unloaded(object sender, RoutedEventArgs e)
  {
    if (this.contextMenuPropertyDesc != null)
    {
      this.contextMenuPropertyDesc.RemoveValueChanged((object) this, new EventHandler(this.OnContextMenuPropertyChanged));
      this.contextMenuPropertyDesc = (DependencyPropertyDescriptor) null;
    }
    this.InnerContextMenu = (ContextMenu) null;
  }

  private void OnContextMenuPropertyChanged(object sender, EventArgs e)
  {
    this.InnerContextMenu = this.ContextMenu;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.Indicator = this.GetTemplateChild("Indicator") as Rectangle;
    ToolbarButtonHelper.UpdateDropDownIconState((ButtonBase) this);
  }

  public bool OpenContextMenuOnChecked
  {
    get => (bool) this.GetValue(ToolbarShareToggleButton.OpenContextMenuOnCheckedProperty);
    set => this.SetValue(ToolbarShareToggleButton.OpenContextMenuOnCheckedProperty, (object) value);
  }

  private static void OnOpenContextMenuOnCheckedPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((bool) e.NewValue == (bool) e.OldValue || !(d is ToolbarShareToggleButton shareToggleButton))
      return;
    shareToggleButton.UpdateOpenContextMenuOnChecked();
  }

  public bool IsDropDownIconVisible
  {
    get => (bool) this.GetValue(ToolbarShareToggleButton.IsDropDownIconVisibleProperty);
    set => this.SetValue(ToolbarShareToggleButton.IsDropDownIconVisibleProperty, (object) value);
  }

  private static void OnIsDropDownIconVisiblePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((bool) e.NewValue == (bool) e.OldValue || !(d is ToolbarShareToggleButton button))
      return;
    ToolbarButtonHelper.UpdateDropDownIconState((ButtonBase) button);
  }

  public Brush IndicatorBrush
  {
    get => (Brush) this.GetValue(ToolbarShareToggleButton.IndicatorBrushProperty);
    set => this.SetValue(ToolbarShareToggleButton.IndicatorBrushProperty, (object) value);
  }

  private void UpdateOpenContextMenuOnChecked()
  {
    if (this.ContextMenu == null)
      return;
    bool valueOrDefault = this.IsChecked.GetValueOrDefault();
    if (this.ContextMenu.IsOpen == valueOrDefault)
      return;
    this.ContextMenu.PlacementTarget = (UIElement) this;
    this.ContextMenu.Placement = PlacementMode.Bottom;
    this.ContextMenu.IsOpen = valueOrDefault;
  }

  protected override void OnChecked(RoutedEventArgs e)
  {
    base.OnChecked(e);
    this.UpdateOpenContextMenuOnChecked();
  }

  protected override void OnUnchecked(RoutedEventArgs e)
  {
    base.OnUnchecked(e);
    this.UpdateOpenContextMenuOnChecked();
  }

  protected override void OnIndeterminate(RoutedEventArgs e)
  {
    base.OnIndeterminate(e);
    this.UpdateOpenContextMenuOnChecked();
  }

  private void ContextMenu_Opened(object sender, RoutedEventArgs e)
  {
    this.IsChecked = new bool?(true);
  }

  private void ContextMenu_Closed(object sender, RoutedEventArgs e)
  {
    this.IsChecked = new bool?(false);
  }

  protected override void OnContextMenuOpening(ContextMenuEventArgs e)
  {
    base.OnContextMenuOpening(e);
    e.Handled = true;
    if (this.ContextMenu == null)
      return;
    this.ContextMenu.IsOpen = false;
  }
}
