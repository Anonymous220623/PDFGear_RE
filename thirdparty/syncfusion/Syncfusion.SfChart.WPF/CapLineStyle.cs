// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.CapLineStyle
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class CapLineStyle : LineStyle
{
  public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register(nameof (Visibility), typeof (Visibility), typeof (CapLineStyle), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(CapLineStyle.OnPropertyChange)));
  public static readonly DependencyProperty LineWidthProperty = DependencyProperty.Register(nameof (LineWidth), typeof (double), typeof (CapLineStyle), new PropertyMetadata((object) 10.0, new PropertyChangedCallback(CapLineStyle.OnPropertyChange)));

  public CapLineStyle(ChartSeriesBase series)
    : base(series)
  {
  }

  public CapLineStyle()
  {
  }

  public Visibility Visibility
  {
    get => (Visibility) this.GetValue(CapLineStyle.VisibilityProperty);
    set => this.SetValue(CapLineStyle.VisibilityProperty, (object) value);
  }

  private static void OnPropertyChange(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
  {
    (obj as CapLineStyle).Series?.ActualArea.ScheduleUpdate();
  }

  public double LineWidth
  {
    get => (double) this.GetValue(CapLineStyle.LineWidthProperty);
    set => this.SetValue(CapLineStyle.LineWidthProperty, (object) value);
  }
}
