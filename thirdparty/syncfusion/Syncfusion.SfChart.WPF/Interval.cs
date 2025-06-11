// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.Interval
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.ObjectModel;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class Interval : DependencyObject
{
  public static readonly DependencyProperty IntervalTypeProperty = DependencyProperty.Register(nameof (IntervalType), typeof (NavigatorIntervalType), typeof (Interval), new PropertyMetadata((object) NavigatorIntervalType.Year));
  public static readonly DependencyProperty LabelFormattersProperty = DependencyProperty.Register(nameof (LabelFormatters), typeof (ObservableCollection<string>), typeof (Interval), new PropertyMetadata((PropertyChangedCallback) null));

  public NavigatorIntervalType IntervalType
  {
    get => (NavigatorIntervalType) this.GetValue(Interval.IntervalTypeProperty);
    set => this.SetValue(Interval.IntervalTypeProperty, (object) value);
  }

  public ObservableCollection<string> LabelFormatters
  {
    get => (ObservableCollection<string>) this.GetValue(Interval.LabelFormattersProperty);
    set => this.SetValue(Interval.LabelFormattersProperty, (object) value);
  }
}
