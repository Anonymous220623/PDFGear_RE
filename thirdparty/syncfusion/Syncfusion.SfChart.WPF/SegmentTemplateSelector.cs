// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SegmentTemplateSelector
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SegmentTemplateSelector : TemplateSelector
{
  public static readonly DependencyProperty FirstPointBrushProperty = DependencyProperty.Register(nameof (FirstPointBrush), typeof (Brush), typeof (SegmentTemplateSelector), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 27, (byte) 161, (byte) 226)), new PropertyChangedCallback(SegmentTemplateSelector.OnMarkerTemplateChanged)));
  public static readonly DependencyProperty LastPointBrushProperty = DependencyProperty.Register(nameof (LastPointBrush), typeof (Brush), typeof (SegmentTemplateSelector), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 27, (byte) 161, (byte) 226))));
  public static readonly DependencyProperty NegativePointBrushProperty = DependencyProperty.Register(nameof (NegativePointBrush), typeof (Brush), typeof (SegmentTemplateSelector), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 27, (byte) 161, (byte) 226))));
  public static readonly DependencyProperty HighPointBrushProperty = DependencyProperty.Register(nameof (HighPointBrush), typeof (Brush), typeof (SegmentTemplateSelector), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 27, (byte) 161, (byte) 226))));
  public static readonly DependencyProperty LowPointBrushProperty = DependencyProperty.Register(nameof (LowPointBrush), typeof (Brush), typeof (SegmentTemplateSelector), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 27, (byte) 161, (byte) 226))));
  private Binding firstPointBinding;
  private Binding lastPointBinding;
  private Binding negativePointBinding;
  private Binding highPointBinding;
  private Binding lowPointBinding;

  public Brush FirstPointBrush
  {
    get => (Brush) this.GetValue(SegmentTemplateSelector.FirstPointBrushProperty);
    set => this.SetValue(SegmentTemplateSelector.FirstPointBrushProperty, (object) value);
  }

  public Brush LastPointBrush
  {
    get => (Brush) this.GetValue(SegmentTemplateSelector.LastPointBrushProperty);
    set => this.SetValue(SegmentTemplateSelector.LastPointBrushProperty, (object) value);
  }

  public Brush NegativePointBrush
  {
    get => (Brush) this.GetValue(SegmentTemplateSelector.NegativePointBrushProperty);
    set => this.SetValue(SegmentTemplateSelector.NegativePointBrushProperty, (object) value);
  }

  public Brush HighPointBrush
  {
    get => (Brush) this.GetValue(SegmentTemplateSelector.HighPointBrushProperty);
    set => this.SetValue(SegmentTemplateSelector.HighPointBrushProperty, (object) value);
  }

  public Brush LowPointBrush
  {
    get => (Brush) this.GetValue(SegmentTemplateSelector.LowPointBrushProperty);
    set => this.SetValue(SegmentTemplateSelector.LowPointBrushProperty, (object) value);
  }

  internal virtual void BindVisual(double x, double y, Shape marker)
  {
    if (y < 0.0 && this.NegativePointBrush != null)
    {
      if (this.negativePointBinding == null)
        this.BindFillProperty("NegativePointBrush", marker, ref this.negativePointBinding);
      else
        marker.SetBinding(Shape.FillProperty, (BindingBase) this.negativePointBinding);
    }
    if (x == this.MinimumX && this.FirstPointBrush != null)
    {
      if (this.firstPointBinding == null)
        this.BindFillProperty("FirstPointBrush", marker, ref this.firstPointBinding);
      else
        marker.SetBinding(Shape.FillProperty, (BindingBase) this.firstPointBinding);
    }
    else if (x == (double) (this.DataCount - 1) && this.LastPointBrush != null)
    {
      if (this.lastPointBinding == null)
        this.BindFillProperty("LastPointBrush", marker, ref this.lastPointBinding);
      else
        marker.SetBinding(Shape.FillProperty, (BindingBase) this.lastPointBinding);
    }
    else if (y == this.MaximumY && this.HighPointBrush != null)
    {
      if (this.highPointBinding == null)
        this.BindFillProperty("HighPointBrush", marker, ref this.highPointBinding);
      else
        marker.SetBinding(Shape.FillProperty, (BindingBase) this.highPointBinding);
    }
    else
    {
      if (y != this.MinimumY || this.LowPointBrush == null)
        return;
      if (this.lowPointBinding == null)
        this.BindFillProperty("LowPointBrush", marker, ref this.lowPointBinding);
      else
        marker.SetBinding(Shape.FillProperty, (BindingBase) this.lowPointBinding);
    }
  }

  protected void BindFillProperty(string propertyName, Shape marker, ref Binding binding)
  {
    binding = new Binding();
    binding.Source = (object) this;
    binding.Path = new PropertyPath(propertyName, new object[0]);
    marker.SetBinding(Shape.FillProperty, (BindingBase) binding);
  }

  private static void OnMarkerTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
  }
}
