// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LabelBarStyle
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class LabelBarStyle : DependencyObject
{
  public static readonly DependencyProperty SelectedLabelBrushProperty = DependencyProperty.Register(nameof (SelectedLabelBrush), typeof (SolidColorBrush), typeof (LabelBarStyle), new PropertyMetadata((object) new SolidColorBrush(Colors.White), new PropertyChangedCallback(LabelBarStyle.OnPropertyChanged)));
  public static readonly DependencyProperty LabelHorizontalAlignmentProperty = DependencyProperty.Register(nameof (LabelHorizontalAlignment), typeof (HorizontalAlignment), typeof (LabelBarStyle), new PropertyMetadata((object) HorizontalAlignment.Center, new PropertyChangedCallback(LabelBarStyle.OnPropertyChanged)));
  public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(nameof (Background), typeof (Brush), typeof (LabelBarStyle), new PropertyMetadata((object) LabelBarStyle.DefaultBackground, new PropertyChangedCallback(LabelBarStyle.OnPropertyChanged)));
  public static readonly DependencyProperty SelectedLabelStyleProperty = DependencyProperty.Register(nameof (SelectedLabelStyle), typeof (Style), typeof (LabelBarStyle), new PropertyMetadata((object) null, new PropertyChangedCallback(LabelBarStyle.OnPropertyChanged)));
  public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof (Position), typeof (BarPosition), typeof (LabelBarStyle), new PropertyMetadata((object) BarPosition.Outside, new PropertyChangedCallback(LabelBarStyle.OnPropertyChanged)));
  private static readonly SolidColorBrush DefaultBackground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 221, (byte) 221, (byte) 221));

  public HorizontalAlignment LabelHorizontalAlignment
  {
    get => (HorizontalAlignment) this.GetValue(LabelBarStyle.LabelHorizontalAlignmentProperty);
    set => this.SetValue(LabelBarStyle.LabelHorizontalAlignmentProperty, (object) value);
  }

  public Brush Background
  {
    get => (Brush) this.GetValue(LabelBarStyle.BackgroundProperty);
    set => this.SetValue(LabelBarStyle.BackgroundProperty, (object) value);
  }

  public SolidColorBrush SelectedLabelBrush
  {
    get => (SolidColorBrush) this.GetValue(LabelBarStyle.SelectedLabelBrushProperty);
    set => this.SetValue(LabelBarStyle.SelectedLabelBrushProperty, (object) value);
  }

  public Style SelectedLabelStyle
  {
    get => (Style) this.GetValue(LabelBarStyle.SelectedLabelStyleProperty);
    set => this.SetValue(LabelBarStyle.SelectedLabelStyleProperty, (object) value);
  }

  public BarPosition Position
  {
    get => (BarPosition) this.GetValue(LabelBarStyle.PositionProperty);
    set => this.SetValue(LabelBarStyle.PositionProperty, (object) value);
  }

  internal SfDateTimeRangeNavigator DateTimeRangeNavigator { get; set; }

  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    LabelBarStyle labelBarStyle = d as LabelBarStyle;
    if (labelBarStyle.DateTimeRangeNavigator == null)
      return;
    labelBarStyle.DateTimeRangeNavigator.SetLabelPosition();
    labelBarStyle.DateTimeRangeNavigator.Scheduleupdate();
  }
}
