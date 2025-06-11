// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartMultiLevelLabel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartMultiLevelLabel : DependencyObject, INotifyPropertyChanged, ICloneable
{
  public static readonly DependencyProperty StartProperty = DependencyProperty.Register(nameof (Start), typeof (object), typeof (ChartMultiLevelLabel), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartMultiLevelLabel.OnStartPropertyChanged)));
  public static readonly DependencyProperty EndProperty = DependencyProperty.Register(nameof (End), typeof (object), typeof (ChartMultiLevelLabel), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartMultiLevelLabel.OnEndPropertyChanged)));
  public static readonly DependencyProperty LevelProperty = DependencyProperty.Register(nameof (Level), typeof (int), typeof (ChartMultiLevelLabel), new PropertyMetadata((object) 0, new PropertyChangedCallback(ChartMultiLevelLabel.OnLevelPropertyChanged)));
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (ChartMultiLevelLabel), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ChartMultiLevelLabel.OnTextPropertyChanged)));
  public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(nameof (FontSize), typeof (double), typeof (ChartMultiLevelLabel), new PropertyMetadata((object) 12.0, new PropertyChangedCallback(ChartMultiLevelLabel.OnFontSizePropertyChanged)));
  public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(nameof (Foreground), typeof (Brush), typeof (ChartMultiLevelLabel), new PropertyMetadata((object) new SolidColorBrush(Colors.Black)));
  public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(nameof (FontFamily), typeof (FontFamily), typeof (ChartMultiLevelLabel), new PropertyMetadata((object) new FontFamily("Segoe UI")));
  public static readonly DependencyProperty LabelAlignmentProperty = DependencyProperty.Register(nameof (LabelAlignment), typeof (LabelAlignment), typeof (ChartMultiLevelLabel), new PropertyMetadata((object) LabelAlignment.Center, new PropertyChangedCallback(ChartMultiLevelLabel.OnLabelAlignmentPropertyChanged)));

  public event PropertyChangedEventHandler PropertyChanged;

  public object Start
  {
    get => this.GetValue(ChartMultiLevelLabel.StartProperty);
    set => this.SetValue(ChartMultiLevelLabel.StartProperty, value);
  }

  public object End
  {
    get => this.GetValue(ChartMultiLevelLabel.EndProperty);
    set => this.SetValue(ChartMultiLevelLabel.EndProperty, value);
  }

  public int Level
  {
    get => (int) this.GetValue(ChartMultiLevelLabel.LevelProperty);
    set => this.SetValue(ChartMultiLevelLabel.LevelProperty, (object) value);
  }

  public string Text
  {
    get => (string) this.GetValue(ChartMultiLevelLabel.TextProperty);
    set => this.SetValue(ChartMultiLevelLabel.TextProperty, (object) value);
  }

  public double FontSize
  {
    get => (double) this.GetValue(ChartMultiLevelLabel.FontSizeProperty);
    set => this.SetValue(ChartMultiLevelLabel.FontSizeProperty, (object) value);
  }

  public Brush Foreground
  {
    get => (Brush) this.GetValue(ChartMultiLevelLabel.ForegroundProperty);
    set => this.SetValue(ChartMultiLevelLabel.ForegroundProperty, (object) value);
  }

  public FontFamily FontFamily
  {
    get => (FontFamily) this.GetValue(ChartMultiLevelLabel.FontFamilyProperty);
    set => this.SetValue(ChartMultiLevelLabel.FontFamilyProperty, (object) value);
  }

  public LabelAlignment LabelAlignment
  {
    get => (LabelAlignment) this.GetValue(ChartMultiLevelLabel.LabelAlignmentProperty);
    set => this.SetValue(ChartMultiLevelLabel.LabelAlignmentProperty, (object) value);
  }

  public DependencyObject Clone() => this.CloneMultiAxisLabel((DependencyObject) null);

  internal void SetVisualBinding(TextBlock textBlock, Border border, ChartAxisBase2D axis)
  {
    Binding binding1 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontSize", new object[0])
    };
    textBlock.SetBinding(TextBlock.FontSizeProperty, (BindingBase) binding1);
    Binding binding2 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Text", new object[0])
    };
    textBlock.SetBinding(TextBlock.TextProperty, (BindingBase) binding2);
    Binding binding3 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Foreground", new object[0])
    };
    textBlock.SetBinding(TextBlock.ForegroundProperty, (BindingBase) binding3);
    Binding binding4 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontFamily", new object[0])
    };
    textBlock.SetBinding(TextBlock.FontFamilyProperty, (BindingBase) binding4);
    textBlock.Tag = (object) this;
    Binding binding5 = new Binding()
    {
      Source = (object) axis,
      Path = new PropertyPath("LabelBorderBrush", new object[0])
    };
    border.SetBinding(Border.BorderBrushProperty, (BindingBase) binding5);
  }

  internal void SetBraceVisualBinding(
    TextBlock textBlock,
    Shape polyline1,
    Shape polyline2,
    ChartAxisBase2D axis)
  {
    Binding binding1 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontSize", new object[0])
    };
    textBlock.SetBinding(TextBlock.FontSizeProperty, (BindingBase) binding1);
    Binding binding2 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Foreground", new object[0])
    };
    textBlock.SetBinding(TextBlock.ForegroundProperty, (BindingBase) binding2);
    Binding binding3 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontFamily", new object[0])
    };
    textBlock.SetBinding(TextBlock.FontFamilyProperty, (BindingBase) binding3);
    Binding binding4 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Text", new object[0])
    };
    textBlock.SetBinding(TextBlock.TextProperty, (BindingBase) binding4);
    textBlock.Tag = (object) this;
    Binding binding5 = new Binding()
    {
      Source = (object) axis,
      Path = new PropertyPath("LabelBorderBrush", new object[0])
    };
    polyline1.SetBinding(Shape.StrokeProperty, (BindingBase) binding5);
    Binding binding6 = new Binding()
    {
      Source = (object) axis,
      Path = new PropertyPath("LabelBorderBrush", new object[0])
    };
    polyline2.SetBinding(Shape.StrokeProperty, (BindingBase) binding6);
    Binding binding7 = new Binding()
    {
      Source = (object) axis,
      Path = new PropertyPath("LabelBorderWidth", new object[0])
    };
    polyline1.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) binding7);
    Binding binding8 = new Binding()
    {
      Source = (object) axis,
      Path = new PropertyPath("LabelBorderWidth", new object[0])
    };
    polyline2.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) binding8);
  }

  protected virtual DependencyObject CloneMultiAxisLabel(DependencyObject obj)
  {
    return (DependencyObject) new ChartMultiLevelLabel()
    {
      Start = this.Start,
      End = this.End,
      Level = this.Level,
      Text = this.Text,
      FontSize = this.FontSize,
      Foreground = this.Foreground,
      FontFamily = this.FontFamily,
      LabelAlignment = this.LabelAlignment
    };
  }

  private static void OnStartPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartMultiLevelLabel).OnPropertyChanged("Start");
  }

  private static void OnEndPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartMultiLevelLabel).OnPropertyChanged("End");
  }

  private static void OnLevelPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartMultiLevelLabel).OnPropertyChanged("Level");
  }

  private static void OnTextPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartMultiLevelLabel).OnPropertyChanged("Text");
  }

  private static void OnFontSizePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartMultiLevelLabel).OnPropertyChanged("FontSize");
  }

  private static void OnLabelAlignmentPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartMultiLevelLabel).OnPropertyChanged("LabelAlignment");
  }

  private void OnPropertyChanged(string name)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(name));
  }
}
