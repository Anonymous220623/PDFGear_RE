// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SfWinLossSparkline
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SfWinLossSparkline : ColumnBase
{
  public static readonly DependencyProperty NegativePointBrushProperty = DependencyProperty.Register(nameof (NegativePointBrush), typeof (Brush), typeof (SfWinLossSparkline), new PropertyMetadata((object) new SolidColorBrush(Colors.Red)));
  public static readonly DependencyProperty NeutralBrushProperty = DependencyProperty.Register(nameof (NeutralBrush), typeof (Brush), typeof (SfWinLossSparkline), new PropertyMetadata((object) new SolidColorBrush(Colors.Blue)));
  private Rectangle rectSegment;

  public Brush NegativePointBrush
  {
    get => (Brush) this.GetValue(SfWinLossSparkline.NegativePointBrushProperty);
    set => this.SetValue(SfWinLossSparkline.NegativePointBrushProperty, (object) value);
  }

  public Brush NeutralBrush
  {
    get => (Brush) this.GetValue(SfWinLossSparkline.NeutralBrushProperty);
    set => this.SetValue(SfWinLossSparkline.NeutralBrushProperty, (object) value);
  }

  protected override void UpdateMinMaxValues()
  {
    base.UpdateMinMaxValues();
    this.maxXValue += 0.8;
    this.deltaX = this.maxXValue - this.minXValue;
  }

  protected override void RenderSegments()
  {
    base.RenderSegments();
    for (int index = 0; index < this.yValues.Count; ++index)
    {
      double xValue = this.xValues[index];
      double yValue = this.yValues[index];
      if (!double.IsNaN(yValue))
      {
        if (this.SegmentPresenter.Children.Count > index)
        {
          this.rectSegment = this.SegmentPresenter.Children[index] as Rectangle;
        }
        else
        {
          this.rectSegment = new Rectangle();
          this.SegmentPresenter.Children.Add((UIElement) this.rectSegment);
          this.rectSegment.Tag = (object) new object[3]
          {
            (object) "Selectable",
            (object) xValue,
            (object) yValue
          };
        }
        double num1 = Math.Round(this.availableWidth * ((xValue - this.minXValue) / this.deltaX));
        double num2 = Math.Round(this.availableWidth * ((xValue + 0.8 - this.minXValue) / this.deltaX));
        double num3 = this.availableHeight / 2.0;
        double num4 = 0.0;
        if (yValue > 0.0)
          this.BindFillProperty((Shape) this.rectSegment, "Interior");
        else if (yValue < 0.0)
        {
          this.BindFillProperty((Shape) this.rectSegment, "NegativePointBrush");
          num4 = num3;
        }
        else
        {
          this.BindFillProperty((Shape) this.rectSegment, "NeutralBrush");
          double num5 = num3;
          num3 /= 10.0;
          num4 = num5 - num3 / 2.0;
        }
        this.rectSegment.SetValue(Canvas.LeftProperty, (object) num1);
        this.rectSegment.SetValue(Canvas.TopProperty, (object) num4);
        this.rectSegment.Height = num3;
        this.rectSegment.Width = num2 - num1;
      }
    }
  }
}
