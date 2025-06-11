// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartDragPointinfo
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartDragPointinfo : INotifyPropertyChanged
{
  private FontFamily fontFamily;
  private double fontSize;
  private FontStyle fontStyle;
  private Brush foreground;
  private Brush brush;
  private ChartSegment segment;
  private Point screenCoordinates;
  private double delta;
  private bool isNegative;

  public event PropertyChangedEventHandler PropertyChanged;

  public FontFamily FontFamily
  {
    get => this.fontFamily;
    set
    {
      if (this.fontFamily == value)
        return;
      this.fontFamily = value;
      this.OnPropertyChanged(nameof (FontFamily));
    }
  }

  public double FontSize
  {
    get => this.fontSize;
    set
    {
      if (this.fontSize == value)
        return;
      this.fontSize = value;
      this.OnPropertyChanged(nameof (FontSize));
    }
  }

  public FontStyle FontStyle
  {
    get => this.fontStyle;
    set
    {
      if (!(this.fontStyle != value))
        return;
      this.fontStyle = value;
      this.OnPropertyChanged(nameof (FontStyle));
    }
  }

  public Brush Foreground
  {
    get => this.foreground;
    set
    {
      if (this.foreground == value)
        return;
      this.foreground = value;
      this.OnPropertyChanged(nameof (Foreground));
    }
  }

  public Brush Brush
  {
    get => this.brush;
    set
    {
      if (this.brush == value)
        return;
      this.brush = value;
      this.OnPropertyChanged(nameof (Brush));
    }
  }

  public ChartSegment Segment
  {
    get => this.segment;
    set
    {
      this.segment = value;
      this.OnPropertyChanged(nameof (Segment));
    }
  }

  public double Delta
  {
    get => this.delta;
    set
    {
      this.delta = value;
      this.OnPropertyChanged(nameof (Delta));
    }
  }

  public bool IsNegative
  {
    get => this.isNegative;
    set
    {
      this.isNegative = value;
      this.OnPropertyChanged(nameof (IsNegative));
    }
  }

  public Point ScreenCoordinates
  {
    get => this.screenCoordinates;
    set
    {
      this.screenCoordinates = value;
      this.OnPropertyChanged(nameof (ScreenCoordinates));
    }
  }

  public DataTemplate PrefixLabelTemplate { get; set; }

  public DataTemplate PostfixLabelTemplate { get; set; }

  public DataTemplate PrefixLabelTemplateX { get; set; }

  public DataTemplate PostfixLabelTemplateX { get; set; }

  protected virtual void OnPropertyChanged(string name)
  {
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs(name));
  }
}
