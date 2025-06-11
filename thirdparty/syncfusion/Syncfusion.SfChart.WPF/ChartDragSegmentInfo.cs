// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartDragSegmentInfo
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartDragSegmentInfo : ChartDragPointinfo
{
  private object newValue;
  private object newXValue;
  private double baseValue;

  public object NewValue
  {
    get => this.newValue;
    set
    {
      this.newValue = value;
      this.OnPropertyChanged(nameof (NewValue));
    }
  }

  public object NewXValue
  {
    get => this.newXValue;
    set
    {
      this.newXValue = value;
      this.OnPropertyChanged(nameof (NewXValue));
    }
  }

  public double BaseValue
  {
    get => this.baseValue;
    set
    {
      this.baseValue = value;
      this.OnPropertyChanged(nameof (BaseValue));
    }
  }
}
