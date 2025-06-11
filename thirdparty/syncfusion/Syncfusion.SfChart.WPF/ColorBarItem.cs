// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ColorBarItem
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ColorBarItem : DependencyObject, INotifyPropertyChanged
{
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (ColorBarItem), new PropertyMetadata((object) Orientation.Horizontal));
  public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(nameof (Background), typeof (Brush), typeof (ColorBarItem), new PropertyMetadata((object) null));
  public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof (Label), typeof (string), typeof (ColorBarItem), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register(nameof (IconHeight), typeof (double), typeof (ColorBarItem), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(nameof (IconWidth), typeof (double), typeof (ColorBarItem), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty ShowLabelProperty = DependencyProperty.Register(nameof (ShowLabel), typeof (bool), typeof (ColorBarItem), new PropertyMetadata((object) false));
  private ChartColorBar _colorBar;

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(ColorBarItem.OrientationProperty);
    set => this.SetValue(ColorBarItem.OrientationProperty, (object) value);
  }

  public Brush Background
  {
    get => (Brush) this.GetValue(ColorBarItem.BackgroundProperty);
    set => this.SetValue(ColorBarItem.BackgroundProperty, (object) value);
  }

  public string Label
  {
    get => (string) this.GetValue(ColorBarItem.LabelProperty);
    set => this.SetValue(ColorBarItem.LabelProperty, (object) value);
  }

  public double IconHeight
  {
    get => (double) this.GetValue(ColorBarItem.IconHeightProperty);
    set => this.SetValue(ColorBarItem.IconHeightProperty, (object) value);
  }

  public double IconWidth
  {
    get => (double) this.GetValue(ColorBarItem.IconWidthProperty);
    set => this.SetValue(ColorBarItem.IconWidthProperty, (object) value);
  }

  public bool ShowLabel
  {
    get => (bool) this.GetValue(ColorBarItem.ShowLabelProperty);
    set => this.SetValue(ColorBarItem.ShowLabelProperty, (object) value);
  }

  internal ChartColorBar ColorBar
  {
    get => this._colorBar;
    set
    {
      this._colorBar = value;
      if (this._colorBar == null)
        return;
      BindingOperations.SetBinding((DependencyObject) this, ColorBarItem.ShowLabelProperty, (BindingBase) new Binding()
      {
        Source = (object) this._colorBar,
        Path = new PropertyPath("ShowLabel", new object[0])
      });
    }
  }

  public event PropertyChangedEventHandler PropertyChanged;

  internal void OnPropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }
}
