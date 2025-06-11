// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.Printing
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class Printing
{
  private ChartBase chart;

  public Printing(ChartBase chart) => this.Chart = chart;

  public ChartBase Chart
  {
    get => this.chart;
    set => this.chart = value;
  }

  internal static DrawingVisual GetDrawingVisual(FrameworkElement contentElement)
  {
    DrawingVisual drawingVisual = new DrawingVisual();
    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
    {
      VisualBrush visualBrush1 = new VisualBrush((Visual) contentElement);
      visualBrush1.Stretch = Stretch.None;
      visualBrush1.AlignmentX = AlignmentX.Left;
      visualBrush1.AlignmentY = AlignmentY.Top;
      VisualBrush visualBrush2 = visualBrush1;
      drawingContext.DrawRectangle((Brush) visualBrush2, (Pen) null, new Rect(0.0, 0.0, contentElement.ActualWidth, contentElement.ActualHeight));
    }
    return drawingVisual;
  }

  public Visual Layout(
    FrameworkElement element,
    Size PrintableArea,
    string Document,
    HorizontalAlignment HorizontalAlignment,
    VerticalAlignment VerticalAlignment,
    Thickness PageMargin,
    bool PrintLandscape,
    bool ShrinkToFit)
  {
    Size size = new Size(element.ActualWidth, element.ActualHeight);
    if (double.IsNaN(element.ActualWidth) || double.IsNaN(element.ActualHeight))
      throw new Exception(ChartLocalizationResourceAccessor.Instance.GetString("PrintingExceptionMessage"));
    TransformGroup transformGroup = new TransformGroup();
    ScaleTransform scaleTransform1 = new ScaleTransform();
    ScaleTransform scaleTransform2 = (ScaleTransform) null;
    ScaleTransform scaleTransform3 = (ScaleTransform) null;
    transformGroup.Children.Add((Transform) new TranslateTransform()
    {
      X = ((PrintableArea.Width - size.Width) / 2.0),
      Y = ((PrintableArea.Height - size.Height) / 2.0)
    });
    double num1 = 1.0;
    double num2 = 1.0;
    if (PrintLandscape)
    {
      transformGroup.Children.Add((Transform) new RotateTransform()
      {
        Angle = 90.0,
        CenterX = (PrintableArea.Width / 2.0),
        CenterY = (PrintableArea.Height / 2.0)
      });
      if (ShrinkToFit)
      {
        if (size.Width + PageMargin.Left + PageMargin.Right > PrintableArea.Height)
          num1 = Math.Round(PrintableArea.Height / (size.Width + PageMargin.Left + PageMargin.Right), 2);
        if (size.Height + PageMargin.Top + PageMargin.Bottom > PrintableArea.Width)
        {
          double num3 = Math.Round(PrintableArea.Width / (size.Height + PageMargin.Top + PageMargin.Bottom), 2);
          num2 = num3 < num2 ? num3 : num2;
        }
      }
    }
    else if (ShrinkToFit)
    {
      if (size.Width + PageMargin.Left + PageMargin.Right > PrintableArea.Width)
        num1 = Math.Round(PrintableArea.Width / (size.Width + PageMargin.Left + PageMargin.Right), 2);
      if (size.Height + PageMargin.Top + PageMargin.Bottom > PrintableArea.Height)
      {
        double num4 = Math.Round(PrintableArea.Height / (size.Height + PageMargin.Top + PageMargin.Bottom), 2);
        num2 = num4 < num2 ? num4 : num2;
      }
    }
    ScaleTransform scaleTransform4 = new ScaleTransform()
    {
      ScaleX = num1,
      ScaleY = num2,
      CenterX = PrintableArea.Width / 2.0,
      CenterY = PrintableArea.Height / 2.0
    };
    switch (VerticalAlignment)
    {
      case VerticalAlignment.Top:
        if (PrintLandscape)
        {
          transformGroup.Children.Add((Transform) new TranslateTransform()
          {
            X = 0.0,
            Y = (PageMargin.Top - (PrintableArea.Height - size.Width * num2) / 2.0)
          });
          break;
        }
        transformGroup.Children.Add((Transform) new TranslateTransform()
        {
          X = 0.0,
          Y = (PageMargin.Top - (PrintableArea.Height - size.Height * num1) / 2.0)
        });
        break;
      case VerticalAlignment.Bottom:
        if (PrintLandscape)
        {
          transformGroup.Children.Add((Transform) new TranslateTransform()
          {
            X = 0.0,
            Y = ((PrintableArea.Height - size.Width * num2) / 2.0 - PageMargin.Bottom)
          });
          break;
        }
        transformGroup.Children.Add((Transform) new TranslateTransform()
        {
          X = 0.0,
          Y = ((PrintableArea.Height - size.Height * num2) / 2.0 - PageMargin.Bottom)
        });
        break;
      case VerticalAlignment.Stretch:
        num2 = Math.Round(PrintableArea.Height / (size.Height + PageMargin.Top + PageMargin.Bottom), 2);
        scaleTransform3 = new ScaleTransform()
        {
          ScaleX = num1,
          ScaleY = Math.Round(PrintableArea.Height / (size.Height + PageMargin.Top + PageMargin.Bottom), 2),
          CenterX = PrintableArea.Width / 2.0,
          CenterY = PrintableArea.Height / 2.0
        };
        break;
    }
    switch (HorizontalAlignment)
    {
      case HorizontalAlignment.Left:
        if (PrintLandscape)
        {
          transformGroup.Children.Add((Transform) new TranslateTransform()
          {
            X = (PageMargin.Left - (PrintableArea.Width - size.Height * num2) / 2.0),
            Y = 0.0
          });
          break;
        }
        transformGroup.Children.Add((Transform) new TranslateTransform()
        {
          X = (PageMargin.Left - (PrintableArea.Width - size.Width * num2) / 2.0),
          Y = 0.0
        });
        break;
      case HorizontalAlignment.Right:
        if (PrintLandscape)
        {
          transformGroup.Children.Add((Transform) new TranslateTransform()
          {
            X = ((PrintableArea.Width - size.Height * num1) / 2.0 - PageMargin.Right),
            Y = 0.0
          });
          break;
        }
        transformGroup.Children.Add((Transform) new TranslateTransform()
        {
          X = ((PrintableArea.Width - size.Width * num1) / 2.0 - PageMargin.Right),
          Y = 0.0
        });
        break;
      case HorizontalAlignment.Stretch:
        Math.Round(PrintableArea.Width / (size.Width + PageMargin.Left + PageMargin.Right), 2);
        scaleTransform2 = new ScaleTransform()
        {
          ScaleX = Math.Round(PrintableArea.Width / (size.Width + PageMargin.Left + PageMargin.Right), 2),
          ScaleY = num2,
          CenterX = PrintableArea.Width / 2.0,
          CenterY = PrintableArea.Height / 2.0
        };
        break;
    }
    if (scaleTransform3 != null)
      transformGroup.Children.Add((Transform) scaleTransform3);
    if (scaleTransform2 != null)
    {
      if (transformGroup.Children.Contains((Transform) scaleTransform3))
        transformGroup.Children.Remove((Transform) scaleTransform3);
      transformGroup.Children.Add((Transform) scaleTransform2);
    }
    if (scaleTransform2 == null && scaleTransform3 == null)
      transformGroup.Children.Add((Transform) scaleTransform4);
    DrawingVisual drawingVisual = Printing.GetDrawingVisual(element);
    drawingVisual.Transform = (Transform) transformGroup;
    return (Visual) drawingVisual;
  }
}
