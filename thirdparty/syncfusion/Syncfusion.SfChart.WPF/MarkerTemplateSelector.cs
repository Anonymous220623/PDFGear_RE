// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.MarkerTemplateSelector
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class MarkerTemplateSelector : SegmentTemplateSelector
{
  public static readonly DependencyProperty MarkerBrushProperty = DependencyProperty.Register(nameof (MarkerBrush), typeof (Brush), typeof (MarkerTemplateSelector), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 27, (byte) 161, (byte) 226))));
  public static readonly DependencyProperty MarkerTemplateProperty = DependencyProperty.Register(nameof (MarkerTemplate), typeof (DataTemplate), typeof (MarkerTemplateSelector), new PropertyMetadata((object) null, new PropertyChangedCallback(MarkerTemplateSelector.OnMarkerTemplateChanged)));
  public static readonly DependencyProperty MarkerHeightProperty = DependencyProperty.Register(nameof (MarkerHeight), typeof (double), typeof (MarkerTemplateSelector), new PropertyMetadata((object) 5.0, new PropertyChangedCallback(MarkerTemplateSelector.OnMarkerPropertyChanged)));
  public static readonly DependencyProperty MarkerWidthProperty = DependencyProperty.Register(nameof (MarkerWidth), typeof (double), typeof (MarkerTemplateSelector), new PropertyMetadata((object) 5.0, new PropertyChangedCallback(MarkerTemplateSelector.OnMarkerPropertyChanged)));
  private Binding defaultBinding;

  public Brush MarkerBrush
  {
    get => (Brush) this.GetValue(MarkerTemplateSelector.MarkerBrushProperty);
    set => this.SetValue(MarkerTemplateSelector.MarkerBrushProperty, (object) value);
  }

  public DataTemplate MarkerTemplate
  {
    get => (DataTemplate) this.GetValue(MarkerTemplateSelector.MarkerTemplateProperty);
    set => this.SetValue(MarkerTemplateSelector.MarkerTemplateProperty, (object) value);
  }

  public double MarkerHeight
  {
    get => (double) this.GetValue(MarkerTemplateSelector.MarkerHeightProperty);
    set => this.SetValue(MarkerTemplateSelector.MarkerHeightProperty, (object) value);
  }

  public double MarkerWidth
  {
    get => (double) this.GetValue(MarkerTemplateSelector.MarkerWidthProperty);
    set => this.SetValue(MarkerTemplateSelector.MarkerWidthProperty, (object) value);
  }

  internal override void BindVisual(double x, double y, Shape marker)
  {
    base.BindVisual(x, y, marker);
    if (this.MarkerBrush != null)
    {
      if (this.NegativePointBrush == null)
      {
        if (this.defaultBinding == null)
          this.BindFillProperty("MarkerBrush", marker, ref this.defaultBinding);
        else
          marker.SetBinding(Shape.FillProperty, (BindingBase) this.defaultBinding);
      }
      else if (y >= 0.0)
      {
        if (this.defaultBinding == null)
          this.BindFillProperty("MarkerBrush", marker, ref this.defaultBinding);
        else
          marker.SetBinding(Shape.FillProperty, (BindingBase) this.defaultBinding);
      }
    }
    base.BindVisual(x, y, marker);
    this.BindVisualSize(marker);
  }

  protected internal override DataTemplate SelectTemplate(double x, double y)
  {
    return this.MarkerTemplate;
  }

  private static void OnMarkerPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    MarkerTemplateSelector templateSelector = d as MarkerTemplateSelector;
    if (templateSelector.Sparkline == null)
      return;
    templateSelector.Sparkline.UpdateArea();
  }

  private static void OnMarkerTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as MarkerTemplateSelector).ClearMarkerPresenter();
  }

  private void ClearMarkerPresenter()
  {
    if (!(this.Sparkline is MarkerBase sparkline))
      return;
    sparkline.MarkerPresenter.Children.Clear();
  }

  private void BindVisualSize(Shape elelment)
  {
    elelment.SetBinding(FrameworkElement.WidthProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("MarkerWidth", new object[0])
    });
    elelment.SetBinding(FrameworkElement.HeightProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("MarkerHeight", new object[0])
    });
  }
}
