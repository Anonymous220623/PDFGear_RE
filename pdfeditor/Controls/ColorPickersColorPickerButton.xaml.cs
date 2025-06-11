// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ColorPickers.ColorPickerButton
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.ColorPickers;

public partial class ColorPickerButton : Button
{
  private static object defaultStandardColorsLocker = new object();
  private static IReadOnlyList<Color> defaultStandardColors;
  private ColorPicker picker;
  private Popup popup;
  private Rectangle Indicator;
  public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof (SelectedColor), typeof (Color), typeof (ColorPickerButton), new PropertyMetadata((object) Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0), (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ColorPickerButton colorPickerButton2) || object.Equals(a.OldValue, a.NewValue))
      return;
    colorPickerButton2.Content = a.NewValue;
    colorPickerButton2.UpdateIndicatorColor();
    colorPickerButton2.RaiseSelectedColorChangedEvent();
  })));
  public static readonly DependencyProperty DefaultColorProperty = DependencyProperty.Register(nameof (DefaultColor), typeof (Color?), typeof (ColorPickerButton), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty StandardColorsProperty = DependencyProperty.Register(nameof (StandardColors), typeof (IReadOnlyCollection<Color>), typeof (ColorPickerButton), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ColorPickerButton colorPickerButton4) || object.Equals(a.OldValue, a.NewValue))
      return;
    colorPickerButton4.UpdateStandardColors();
  })));
  public static readonly DependencyProperty RecentColorsKeyProperty = DependencyProperty.Register(nameof (RecentColorsKey), typeof (string), typeof (ColorPickerButton), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsMoreColorEnabledProperty = DependencyProperty.Register(nameof (IsMoreColorEnabled), typeof (bool), typeof (ColorPickerButton), new PropertyMetadata((object) true));
  public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(nameof (Placement), typeof (PlacementMode), typeof (ColorPickerButton), new PropertyMetadata((object) PlacementMode.Bottom, new PropertyChangedCallback(ColorPickerButton.OnPlacementPropertyChanged)));
  public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent("SelectedColorChanged", RoutingStrategy.Bubble, typeof (EventHandler<ColorPickerButtonSelectedColorChangedEventArgs>), typeof (ColorPickerButton));
  public static readonly RoutedEvent ItemClickEvent = EventManager.RegisterRoutedEvent("ItemClick", RoutingStrategy.Bubble, typeof (EventHandler<ColorPickerButtonItemClickEventArgs>), typeof (ColorPickerButton));

  static ColorPickerButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ColorPickerButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ColorPickerButton)));
  }

  public ColorPickerButton()
  {
    this.Content = (object) this.SelectedColor;
    if (ColorPickerButton.defaultStandardColors == null)
    {
      lock (ColorPickerButton.defaultStandardColorsLocker)
      {
        if (ColorPickerButton.defaultStandardColors == null)
          ColorPickerButton.defaultStandardColors = (IReadOnlyList<Color>) new Color[10]
          {
            hex(12584722U),
            hex(16518427U),
            hex(16629805U),
            hex(16776504U),
            hex(9817944U),
            hex(1749076U),
            hex(2011886U),
            hex(1077950U),
            hex(139870U),
            hex(7288223U)
          };
      }
    }
    this.StandardColors = (IReadOnlyCollection<Color>) ColorPickerButton.defaultStandardColors;

    static Color hex(uint _rgb) => ColorHelper.FromRgb(_rgb);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.Indicator = this.GetTemplateChild("Indicator") as Rectangle;
    this.UpdateIndicatorColor();
  }

  private void InitPopup()
  {
    if (this.picker == null)
    {
      this.picker = new ColorPicker();
      this.UpdateStandardColors();
      this.picker.SetBinding(ColorPicker.DefaultColorProperty, (BindingBase) new Binding("DefaultColor")
      {
        Source = (object) this
      });
      this.picker.SetBinding(ColorPicker.RecentColorsKeyProperty, (BindingBase) new Binding("RecentColorsKey")
      {
        Source = (object) this
      });
      this.picker.SetBinding(ColorPicker.IsMoreColorEnabledProperty, (BindingBase) new Binding("IsMoreColorEnabled")
      {
        Source = (object) this
      });
      this.picker.ItemClick += new EventHandler<ColorPickerItemClickEventArgs>(this.Picker_ItemClick);
    }
    if (this.popup != null)
      return;
    this.popup = new Popup()
    {
      Child = (UIElement) this.picker,
      AllowsTransparency = true,
      StaysOpen = false,
      Placement = this.Placement,
      PlacementTarget = (UIElement) this,
      PopupAnimation = PopupAnimation.Slide
    };
  }

  protected override void OnClick()
  {
    base.OnClick();
    this.InitPopup();
    this.popup.IsOpen = true;
  }

  private void Picker_ItemClick(object sender, ColorPickerItemClickEventArgs e)
  {
    this.SelectedColor = e.Item.Color;
    this.popup.IsOpen = false;
    this.RaiseItemClickEvent(e.Item);
  }

  public Color SelectedColor
  {
    get => (Color) this.GetValue(ColorPickerButton.SelectedColorProperty);
    set => this.SetValue(ColorPickerButton.SelectedColorProperty, (object) value);
  }

  public Color? DefaultColor
  {
    get => (Color?) this.GetValue(ColorPickerButton.DefaultColorProperty);
    set => this.SetValue(ColorPickerButton.DefaultColorProperty, (object) value);
  }

  public IReadOnlyCollection<Color> StandardColors
  {
    get => (IReadOnlyCollection<Color>) this.GetValue(ColorPickerButton.StandardColorsProperty);
    set => this.SetValue(ColorPickerButton.StandardColorsProperty, (object) value);
  }

  public string RecentColorsKey
  {
    get => (string) this.GetValue(ColorPickerButton.RecentColorsKeyProperty);
    set => this.SetValue(ColorPickerButton.RecentColorsKeyProperty, (object) value);
  }

  public bool IsMoreColorEnabled
  {
    get => (bool) this.GetValue(ColorPickerButton.IsMoreColorEnabledProperty);
    set => this.SetValue(ColorPickerButton.IsMoreColorEnabledProperty, (object) value);
  }

  public PlacementMode Placement
  {
    get => (PlacementMode) this.GetValue(ColorPickerButton.PlacementProperty);
    set => this.SetValue(ColorPickerButton.PlacementProperty, (object) value);
  }

  private static void OnPlacementPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (object.Equals(e.NewValue, e.OldValue) || !(d is ColorPickerButton colorPickerButton) || colorPickerButton.popup == null)
      return;
    colorPickerButton.popup.Placement = (PlacementMode) e.NewValue;
  }

  private void UpdateStandardColors()
  {
    if (this.picker == null)
      return;
    if (this.StandardColors == null)
      this.picker.StandardColors = (object) null;
    else
      this.picker.StandardColors = (object) this.StandardColors.Select<Color, ColorValue>((Func<Color, ColorValue>) (c => new ColorValue(c, true))).Take<ColorValue>(10).ToList<ColorValue>();
  }

  private void UpdateIndicatorColor()
  {
    if (this.Indicator == null)
      return;
    this.Indicator.Fill = (Brush) new SolidColorBrush(this.SelectedColor);
  }

  public event EventHandler<ColorPickerButtonSelectedColorChangedEventArgs> SelectedColorChanged
  {
    add => this.AddHandler(ColorPickerButton.SelectedColorChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(ColorPickerButton.SelectedColorChangedEvent, (Delegate) value);
  }

  private void RaiseSelectedColorChangedEvent()
  {
    this.RaiseEvent((RoutedEventArgs) new ColorPickerButtonSelectedColorChangedEventArgs((object) this, this.SelectedColor));
  }

  public event EventHandler<ColorPickerButtonItemClickEventArgs> ItemClick
  {
    add => this.AddHandler(ColorPickerButton.ItemClickEvent, (Delegate) value);
    remove => this.RemoveHandler(ColorPickerButton.ItemClickEvent, (Delegate) value);
  }

  private void RaiseItemClickEvent(ColorValue item)
  {
    if (item == null)
      return;
    this.RaiseEvent((RoutedEventArgs) new ColorPickerButtonItemClickEventArgs((object) this, item));
  }
}
