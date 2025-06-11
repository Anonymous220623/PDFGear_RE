// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartPointInfo
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartPointInfo : INotifyPropertyChanged
{
  internal bool isOutlier;
  private ChartSeriesBase series;
  private ChartAxis axis;
  private string valueX;
  private string valueY;
  private string high;
  private string low;
  private string open;
  private string close;
  private string upperLine;
  private string lowerLine;
  private string signalLine;
  private Brush interior;
  private Brush foreground;
  private Brush borderBrush;
  private PointCollection polygonPoints;
  private object item;
  private ObservableCollection<string> seriesvalues;
  private ChartAlignment verticalAlignment;
  private ChartAlignment horizontalAlignment;
  private string median;

  public event PropertyChangedEventHandler PropertyChanged;

  public ObservableCollection<string> SeriesValues
  {
    get
    {
      if (this.seriesvalues == null)
        this.seriesvalues = new ObservableCollection<string>();
      return this.seriesvalues;
    }
    set => value = this.seriesvalues;
  }

  public ChartSeriesBase Series
  {
    get => this.series;
    set
    {
      if (value == this.series)
        return;
      this.series = value;
      this.OnPropertyChanged(nameof (Series));
    }
  }

  public ChartAxis Axis
  {
    get => this.axis;
    set => this.axis = value;
  }

  public object Item
  {
    get => this.item;
    set
    {
      this.item = value;
      this.OnPropertyChanged(nameof (Item));
    }
  }

  public Brush Interior
  {
    get => this.interior;
    set
    {
      this.interior = value;
      this.OnPropertyChanged(nameof (Interior));
    }
  }

  public Brush Foreground
  {
    get => this.foreground;
    set
    {
      this.foreground = value;
      this.OnPropertyChanged(nameof (Foreground));
    }
  }

  public Brush BorderBrush
  {
    get => this.borderBrush;
    set
    {
      this.borderBrush = value;
      this.OnPropertyChanged(nameof (BorderBrush));
    }
  }

  public string ValueX
  {
    get => this.valueX;
    set
    {
      if (!(value != this.valueX))
        return;
      this.valueX = value;
      this.OnPropertyChanged(nameof (ValueX));
    }
  }

  public string ValueY
  {
    get => this.valueY;
    set
    {
      if (!(value != this.valueY))
        return;
      this.valueY = value;
      this.OnPropertyChanged(nameof (ValueY));
    }
  }

  public string High
  {
    get => this.high;
    set
    {
      if (!(value != this.high))
        return;
      this.high = value;
      this.OnPropertyChanged(nameof (High));
    }
  }

  public string Low
  {
    get => this.low;
    set
    {
      if (!(value != this.low))
        return;
      this.low = value;
      this.OnPropertyChanged(nameof (Low));
    }
  }

  public string Open
  {
    get => this.open;
    set
    {
      if (!(value != this.open))
        return;
      this.open = value;
      this.OnPropertyChanged(nameof (Open));
    }
  }

  public string Close
  {
    get => this.close;
    set
    {
      if (!(value != this.close))
        return;
      this.close = value;
      this.OnPropertyChanged(nameof (Close));
    }
  }

  public string Median
  {
    get => this.median;
    set
    {
      if (!(value != this.median))
        return;
      this.median = value;
      this.OnPropertyChanged(nameof (Median));
    }
  }

  public string UpperLine
  {
    get => this.upperLine;
    set
    {
      if (!(value != this.upperLine))
        return;
      this.upperLine = value;
      this.OnPropertyChanged(nameof (UpperLine));
    }
  }

  public string LowerLine
  {
    get => this.lowerLine;
    set
    {
      if (!(value != this.lowerLine))
        return;
      this.lowerLine = value;
      this.OnPropertyChanged(nameof (LowerLine));
    }
  }

  public string SignalLine
  {
    get => this.signalLine;
    set
    {
      if (!(value != this.signalLine))
        return;
      this.signalLine = value;
      this.OnPropertyChanged(nameof (SignalLine));
    }
  }

  public double BaseX { get; set; }

  public double BaseY { get; set; }

  public double X { get; set; }

  public double Y { get; set; }

  public PointCollection PolygonPoints
  {
    get => this.polygonPoints;
    set
    {
      this.polygonPoints = value;
      this.OnPropertyChanged(nameof (PolygonPoints));
    }
  }

  internal ChartAlignment VerticalAlignment
  {
    get => this.verticalAlignment;
    set => this.verticalAlignment = value;
  }

  internal ChartAlignment HorizontalAlignment
  {
    get => this.horizontalAlignment;
    set => this.horizontalAlignment = value;
  }

  public void OnPropertyChanged(string propertyName)
  {
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }
}
