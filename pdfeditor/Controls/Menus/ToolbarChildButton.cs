// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarChildButton
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Menus;

public class ToolbarChildButton : Button
{
  private Rectangle indicator;
  public static readonly DependencyProperty IsDropDownIconVisibleProperty = ToolbarButtonHelper.IsDropDownIconVisibleProperty.AddOwner(typeof (ToolbarChildButton), new PropertyMetadata((object) true, new PropertyChangedCallback(ToolbarChildButton.OnIsDropDownIconVisiblePropertyChanged)));
  public static readonly DependencyProperty IndicatorBrushProperty = ToolbarButtonHelper.IndicatorBrushProperty.AddOwner(typeof (ToolbarChildButton));

  static ToolbarChildButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ToolbarChildButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ToolbarChildButton)));
  }

  public ToolbarChildButton() => ToolbarButtonHelper.RegisterIsKeyboardFocused((ButtonBase) this);

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
    if (!(this.Indicator.Clip is RectangleGeometry rectangleGeometry))
    {
      rectangleGeometry = new RectangleGeometry();
      this.Indicator.Clip = (Geometry) rectangleGeometry;
    }
    rectangleGeometry.Rect = new Rect(0.0, this.Indicator.ActualHeight / 2.0, this.Indicator.ActualWidth, this.Indicator.ActualHeight / 2.0);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.Indicator = this.GetTemplateChild("Indicator") as Rectangle;
    ToolbarButtonHelper.UpdateDropDownIconState((ButtonBase) this);
  }

  public bool IsDropDownIconVisible
  {
    get => (bool) this.GetValue(ToolbarChildButton.IsDropDownIconVisibleProperty);
    set => this.SetValue(ToolbarChildButton.IsDropDownIconVisibleProperty, (object) value);
  }

  private static void OnIsDropDownIconVisiblePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((bool) e.NewValue == (bool) e.OldValue || !(d is ToolbarChildButton button))
      return;
    ToolbarButtonHelper.UpdateDropDownIconState((ButtonBase) button);
  }

  public Brush IndicatorBrush
  {
    get => (Brush) this.GetValue(ToolbarChildButton.IndicatorBrushProperty);
    set => this.SetValue(ToolbarChildButton.IndicatorBrushProperty, (object) value);
  }
}
