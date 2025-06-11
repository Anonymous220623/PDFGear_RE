// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartStripLine
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartStripLine : FrameworkElement, INotifyPropertyChanged
{
  public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(nameof (Background), typeof (Brush), typeof (ChartStripLine), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(nameof (BorderBrush), typeof (Brush), typeof (ChartStripLine), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof (BorderThickness), typeof (Thickness), typeof (ChartStripLine), new PropertyMetadata((object) new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0)));
  public static readonly DependencyProperty StartProperty = DependencyProperty.Register(nameof (Start), typeof (double), typeof (ChartStripLine), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(ChartStripLine.OnStartPropertChanged)));
  public static readonly DependencyProperty SegmentStartValueProperty = DependencyProperty.Register(nameof (SegmentStartValue), typeof (double), typeof (ChartStripLine), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(ChartStripLine.OnSegmentStartValueChanged)));
  public static readonly DependencyProperty SegmentEndValueProperty = DependencyProperty.Register(nameof (SegmentEndValue), typeof (double), typeof (ChartStripLine), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(ChartStripLine.OnSegmentEndValueChanged)));
  public static readonly DependencyProperty SegmentAxisNameProperty = DependencyProperty.Register(nameof (SegmentAxisName), typeof (string), typeof (ChartStripLine), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ChartStripLine.OnSegmentAxisNameChanged)));
  public static readonly DependencyProperty IsSegmentedProperty = DependencyProperty.Register(nameof (IsSegmented), typeof (bool), typeof (ChartStripLine), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartStripLine.OnIsSegmentedPropertyChanged)));
  public static readonly DependencyProperty RepeatEveryProperty = DependencyProperty.Register(nameof (RepeatEvery), typeof (double), typeof (ChartStripLine), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartStripLine.OnRepeatEveryPropertyChanged)));
  public static readonly DependencyProperty RepeatUntilProperty = DependencyProperty.Register(nameof (RepeatUntil), typeof (double), typeof (ChartStripLine), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(ChartStripLine.OnRepeatUntilPropertyChanged)));
  public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof (Label), typeof (object), typeof (ChartStripLine), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty LabelTemplateProperty = DependencyProperty.Register(nameof (LabelTemplate), typeof (DataTemplate), typeof (ChartStripLine), new PropertyMetadata((PropertyChangedCallback) null));
  public new static readonly DependencyProperty WidthProperty = DependencyProperty.Register(nameof (Width), typeof (double), typeof (ChartStripLine), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartStripLine.OnWidthPropertyChanged)));
  public static readonly DependencyProperty LabelAngleProperty = DependencyProperty.Register(nameof (LabelAngle), typeof (double), typeof (ChartStripLine), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartStripLine.OnLabelAnglePropertyChanged)));
  public static readonly DependencyProperty IsPixelWidthProperty = DependencyProperty.Register(nameof (IsPixelWidth), typeof (bool), typeof (ChartStripLine), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartStripLine.OnIsPixelWidthPropertyChanged)));
  public static readonly DependencyProperty LabelHorizontalAlignmentProperty = DependencyProperty.Register(nameof (LabelHorizontalAlignment), typeof (HorizontalAlignment), typeof (ChartStripLine), new PropertyMetadata((object) HorizontalAlignment.Center));
  public static readonly DependencyProperty LabelVerticalAlignmentProperty = DependencyProperty.Register(nameof (LabelVerticalAlignment), typeof (VerticalAlignment), typeof (ChartStripLine), new PropertyMetadata((object) VerticalAlignment.Center));

  public ChartStripLine() => this.DefaultStyleKey = (object) typeof (ChartStripLine);

  public event PropertyChangedEventHandler PropertyChanged;

  public double Start
  {
    get => (double) this.GetValue(ChartStripLine.StartProperty);
    set => this.SetValue(ChartStripLine.StartProperty, (object) value);
  }

  public Brush Background
  {
    get => (Brush) this.GetValue(ChartStripLine.BackgroundProperty);
    set => this.SetValue(ChartStripLine.BackgroundProperty, (object) value);
  }

  public Brush BorderBrush
  {
    get => (Brush) this.GetValue(ChartStripLine.BorderBrushProperty);
    set => this.SetValue(ChartStripLine.BorderBrushProperty, (object) value);
  }

  public Thickness BorderThickness
  {
    get => (Thickness) this.GetValue(ChartStripLine.BorderThicknessProperty);
    set => this.SetValue(ChartStripLine.BorderThicknessProperty, (object) value);
  }

  public double SegmentStartValue
  {
    get => (double) this.GetValue(ChartStripLine.SegmentStartValueProperty);
    set => this.SetValue(ChartStripLine.SegmentStartValueProperty, (object) value);
  }

  public double SegmentEndValue
  {
    get => (double) this.GetValue(ChartStripLine.SegmentEndValueProperty);
    set => this.SetValue(ChartStripLine.SegmentEndValueProperty, (object) value);
  }

  public string SegmentAxisName
  {
    get => (string) this.GetValue(ChartStripLine.SegmentAxisNameProperty);
    set => this.SetValue(ChartStripLine.SegmentAxisNameProperty, (object) value);
  }

  public bool IsSegmented
  {
    get => (bool) this.GetValue(ChartStripLine.IsSegmentedProperty);
    set => this.SetValue(ChartStripLine.IsSegmentedProperty, (object) value);
  }

  public double RepeatEvery
  {
    get => (double) this.GetValue(ChartStripLine.RepeatEveryProperty);
    set => this.SetValue(ChartStripLine.RepeatEveryProperty, (object) value);
  }

  public double RepeatUntil
  {
    get => (double) this.GetValue(ChartStripLine.RepeatUntilProperty);
    set => this.SetValue(ChartStripLine.RepeatUntilProperty, (object) value);
  }

  public object Label
  {
    get => this.GetValue(ChartStripLine.LabelProperty);
    set => this.SetValue(ChartStripLine.LabelProperty, value);
  }

  public DataTemplate LabelTemplate
  {
    get => (DataTemplate) this.GetValue(ChartStripLine.LabelTemplateProperty);
    set => this.SetValue(ChartStripLine.LabelTemplateProperty, (object) value);
  }

  public new double Width
  {
    get => (double) this.GetValue(ChartStripLine.WidthProperty);
    set => this.SetValue(ChartStripLine.WidthProperty, (object) value);
  }

  public double LabelAngle
  {
    get => (double) this.GetValue(ChartStripLine.LabelAngleProperty);
    set => this.SetValue(ChartStripLine.LabelAngleProperty, (object) value);
  }

  public bool IsPixelWidth
  {
    get => (bool) this.GetValue(ChartStripLine.IsPixelWidthProperty);
    set => this.SetValue(ChartStripLine.IsPixelWidthProperty, (object) value);
  }

  public HorizontalAlignment LabelHorizontalAlignment
  {
    get => (HorizontalAlignment) this.GetValue(ChartStripLine.LabelHorizontalAlignmentProperty);
    set => this.SetValue(ChartStripLine.LabelHorizontalAlignmentProperty, (object) value);
  }

  public VerticalAlignment LabelVerticalAlignment
  {
    get => (VerticalAlignment) this.GetValue(ChartStripLine.LabelVerticalAlignmentProperty);
    set => this.SetValue(ChartStripLine.LabelVerticalAlignmentProperty, (object) value);
  }

  public DependencyObject Clone() => this.CloneStripline((DependencyObject) null);

  protected virtual DependencyObject CloneStripline(DependencyObject obj)
  {
    ChartStripLine chartStripLine = new ChartStripLine()
    {
      Start = this.Start,
      Label = this.Label,
      Background = this.Background,
      BorderBrush = this.BorderBrush,
      BorderThickness = this.BorderThickness,
      SegmentEndValue = this.SegmentEndValue,
      SegmentStartValue = this.SegmentStartValue,
      SegmentAxisName = this.SegmentAxisName,
      IsSegmented = this.IsSegmented,
      RepeatEvery = this.RepeatEvery,
      RepeatUntil = this.RepeatUntil,
      LabelTemplate = this.LabelTemplate,
      LabelAngle = this.LabelAngle,
      LabelVerticalAlignment = this.LabelVerticalAlignment,
      LabelHorizontalAlignment = this.LabelHorizontalAlignment
    };
    chartStripLine.IsPixelWidth = chartStripLine.IsPixelWidth;
    chartStripLine.Width = this.Width;
    return (DependencyObject) chartStripLine;
  }

  private static void OnStartPropertChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartStripLine).OnPropertyChanged("Start");
  }

  private static void OnSegmentStartValueChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartStripLine).OnPropertyChanged("SegmentStartValue");
  }

  private static void OnSegmentEndValueChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartStripLine).OnPropertyChanged("SegmentEndValue");
  }

  private static void OnSegmentAxisNameChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartStripLine).OnPropertyChanged("SegmentAxisName");
  }

  private static void OnIsSegmentedPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartStripLine).OnPropertyChanged("IsSegmented");
  }

  private static void OnRepeatEveryPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartStripLine).OnPropertyChanged("RepeatEvery");
  }

  private static void OnRepeatUntilPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartStripLine).OnPropertyChanged("RepeatUntil");
  }

  private static void OnWidthPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartStripLine).OnPropertyChanged("Width");
  }

  private static void OnLabelAnglePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartStripLine).OnPropertyChanged("LabelAngle");
  }

  private static void OnIsPixelWidthPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartStripLine).OnPropertyChanged("IsPixelWidth");
  }

  private void OnPropertyChanged(string name)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(name));
  }
}
