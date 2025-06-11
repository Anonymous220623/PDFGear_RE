// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarChildToggleButton
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Menus;

public class ToolbarChildToggleButton : ToggleButton
{
  private static List<WeakReference<ToolbarChildToggleButton>> readyToShowContextElements;
  private ContextMenu innerContextMenu;
  private DependencyPropertyDescriptor contextMenuPropertyDesc;
  private Rectangle indicator;
  private bool attached;
  public static readonly DependencyProperty OpenContextMenuOnCheckedProperty = DependencyProperty.Register(nameof (OpenContextMenuOnChecked), typeof (bool), typeof (ToolbarChildToggleButton), new PropertyMetadata((object) false, new PropertyChangedCallback(ToolbarChildToggleButton.OnOpenContextMenuOnCheckedPropertyChanged)));
  public static readonly DependencyProperty IsDropDownIconVisibleProperty = ToolbarButtonHelper.IsDropDownIconVisibleProperty.AddOwner(typeof (ToolbarChildToggleButton), new PropertyMetadata((object) true, new PropertyChangedCallback(ToolbarChildToggleButton.OnIsDropDownIconVisiblePropertyChanged)));
  public static readonly DependencyProperty IndicatorBrushProperty = ToolbarButtonHelper.IndicatorBrushProperty.AddOwner(typeof (ToolbarChildToggleButton));

  static ToolbarChildToggleButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ToolbarChildToggleButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ToolbarChildToggleButton)));
    ToolbarChildToggleButton.readyToShowContextElements = new List<WeakReference<ToolbarChildToggleButton>>();
  }

  public ToolbarChildToggleButton()
  {
    this.Loaded += new RoutedEventHandler(this.ToolbarChildToggleButton_Loaded);
    this.Unloaded += new RoutedEventHandler(this.ToolbarChildToggleButton_Unloaded);
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.ToolbarChildToggleButton_IsVisibleChanged);
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
      this.attached = false;
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

  private void ToolbarChildToggleButton_IsVisibleChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
    if (this.IsVisible)
      return;
    this.attached = false;
    if (this.IsChecked.GetValueOrDefault())
      this.IsChecked = new bool?(false);
    ContextMenu innerContextMenu = this.InnerContextMenu;
    if (innerContextMenu == null)
      return;
    innerContextMenu.IsOpen = false;
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
    get => (bool) this.GetValue(ToolbarChildToggleButton.OpenContextMenuOnCheckedProperty);
    set => this.SetValue(ToolbarChildToggleButton.OpenContextMenuOnCheckedProperty, (object) value);
  }

  private static void OnOpenContextMenuOnCheckedPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((bool) e.NewValue == (bool) e.OldValue || !(d is ToolbarChildToggleButton childToggleButton))
      return;
    childToggleButton.UpdateOpenContextMenuOnChecked();
  }

  public bool IsDropDownIconVisible
  {
    get => (bool) this.GetValue(ToolbarChildToggleButton.IsDropDownIconVisibleProperty);
    set => this.SetValue(ToolbarChildToggleButton.IsDropDownIconVisibleProperty, (object) value);
  }

  private static void OnIsDropDownIconVisiblePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((bool) e.NewValue == (bool) e.OldValue || !(d is ToolbarChildToggleButton button))
      return;
    ToolbarButtonHelper.UpdateDropDownIconState((ButtonBase) button);
  }

  public Brush IndicatorBrush
  {
    get => (Brush) this.GetValue(ToolbarChildToggleButton.IndicatorBrushProperty);
    set => this.SetValue(ToolbarChildToggleButton.IndicatorBrushProperty, (object) value);
  }

  private void UpdateOpenContextMenuOnChecked()
  {
    if (this.ContextMenu == null || !this.IsVisible)
      return;
    this.IsChecked.GetValueOrDefault();
    if (this.IsChecked.GetValueOrDefault())
    {
      lock (ToolbarChildToggleButton.readyToShowContextElements)
        ToolbarChildToggleButton.readyToShowContextElements.Add(new WeakReference<ToolbarChildToggleButton>(this));
      this.ContextMenu.PlacementTarget = (UIElement) this;
      this.Dispatcher.InvokeAsync((Action) (() =>
      {
        List<ToolbarChildToggleButton> source = new List<ToolbarChildToggleButton>();
        lock (ToolbarChildToggleButton.readyToShowContextElements)
        {
          for (int index = ToolbarChildToggleButton.readyToShowContextElements.Count - 1; index >= 0; --index)
          {
            ToolbarChildToggleButton target;
            if (ToolbarChildToggleButton.readyToShowContextElements[index].TryGetTarget(out target) && target.IsLoaded && target.IsVisible && target.InnerContextMenu != null)
              source.Add(target);
            ToolbarChildToggleButton.readyToShowContextElements.RemoveAt(index);
          }
        }
        ToolbarChildToggleButton childToggleButton = (ToolbarChildToggleButton) null;
        double num1 = double.MaxValue;
        for (int index = 0; index < source.Count; ++index)
        {
          if (source[index].IsMouseOver)
          {
            childToggleButton = source[index];
            break;
          }
          double num2 = source[index].ActualWidth / 2.0;
          double num3 = source[index].ActualHeight / 2.0;
          Point position = Mouse.GetPosition((IInputElement) source[index]);
          double x = position.X;
          double num4 = num2 - x;
          double num5 = num3 - position.Y;
          double num6 = num4 * num4 + num5 * num5;
          if (num6 < num1)
          {
            childToggleButton = source[index];
            num1 = num6;
          }
        }
        if (num1 > 2500.0)
          childToggleButton = source.FirstOrDefault<ToolbarChildToggleButton>();
        if (childToggleButton == null)
          return;
        childToggleButton.attached = true;
        ContextMenu innerContextMenu = childToggleButton.innerContextMenu;
        innerContextMenu.Placement = PlacementMode.Bottom;
        innerContextMenu.IsOpen = true;
      }), DispatcherPriority.Render);
    }
    else
    {
      if (this.attached)
      {
        this.attached = false;
        this.IsChecked = new bool?(false);
      }
      this.ContextMenu.IsOpen = false;
      this.ContextMenu.PlacementTarget = (UIElement) null;
    }
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
    if (!this.attached)
      return;
    this.IsChecked = new bool?(true);
  }

  private void ContextMenu_Closed(object sender, RoutedEventArgs e)
  {
    if (!this.attached)
      return;
    this.attached = false;
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
