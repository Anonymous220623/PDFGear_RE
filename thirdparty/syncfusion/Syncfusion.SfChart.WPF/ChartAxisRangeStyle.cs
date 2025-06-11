// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAxisRangeStyle
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartAxisRangeStyle : DependencyObject
{
  public static readonly DependencyProperty StartProperty = DependencyProperty.Register(nameof (Start), typeof (object), typeof (ChartAxisRangeStyle), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty EndProperty = DependencyProperty.Register(nameof (End), typeof (object), typeof (ChartAxisRangeStyle), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MajorGridLineStyleProperty = DependencyProperty.Register(nameof (MajorGridLineStyle), typeof (Style), typeof (ChartAxisRangeStyle), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MinorGridLineStyleProperty = DependencyProperty.Register(nameof (MinorGridLineStyle), typeof (Style), typeof (ChartAxisRangeStyle), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MajorTickLineStyleProperty = DependencyProperty.Register(nameof (MajorTickLineStyle), typeof (Style), typeof (ChartAxisRangeStyle), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MinorTickLineStyleProperty = DependencyProperty.Register(nameof (MinorTickLineStyle), typeof (Style), typeof (ChartAxisRangeStyle), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty LabelStyleProperty = DependencyProperty.Register(nameof (LabelStyle), typeof (LabelStyle), typeof (ChartAxisRangeStyle), new PropertyMetadata((PropertyChangedCallback) null));
  private DoubleRange range = DoubleRange.Empty;

  public object Start
  {
    get => this.GetValue(ChartAxisRangeStyle.StartProperty);
    set => this.SetValue(ChartAxisRangeStyle.StartProperty, value);
  }

  public object End
  {
    get => this.GetValue(ChartAxisRangeStyle.EndProperty);
    set => this.SetValue(ChartAxisRangeStyle.EndProperty, value);
  }

  public LabelStyle LabelStyle
  {
    get => (LabelStyle) this.GetValue(ChartAxisRangeStyle.LabelStyleProperty);
    set => this.SetValue(ChartAxisRangeStyle.LabelStyleProperty, (object) value);
  }

  public Style MajorGridLineStyle
  {
    get => (Style) this.GetValue(ChartAxisRangeStyle.MajorGridLineStyleProperty);
    set => this.SetValue(ChartAxisRangeStyle.MajorGridLineStyleProperty, (object) value);
  }

  public Style MinorGridLineStyle
  {
    get => (Style) this.GetValue(ChartAxisRangeStyle.MinorGridLineStyleProperty);
    set => this.SetValue(ChartAxisRangeStyle.MinorGridLineStyleProperty, (object) value);
  }

  public Style MajorTickLineStyle
  {
    get => (Style) this.GetValue(ChartAxisRangeStyle.MajorTickLineStyleProperty);
    set => this.SetValue(ChartAxisRangeStyle.MajorTickLineStyleProperty, (object) value);
  }

  public Style MinorTickLineStyle
  {
    get => (Style) this.GetValue(ChartAxisRangeStyle.MinorTickLineStyleProperty);
    set => this.SetValue(ChartAxisRangeStyle.MinorTickLineStyleProperty, (object) value);
  }

  internal DoubleRange Range
  {
    get => this.range;
    set
    {
      if (this.range == value)
        return;
      this.range = value;
    }
  }
}
