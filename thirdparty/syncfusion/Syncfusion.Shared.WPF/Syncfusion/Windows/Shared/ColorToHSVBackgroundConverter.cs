// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ColorToHSVBackgroundConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class ColorToHSVBackgroundConverter : IMultiValueConverter
{
  private HSV m_hsv;
  private float m_h;
  private float m_s;
  private float m_v;

  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values[0] is HSV)
      this.m_hsv = (HSV) values[0];
    if (values[1] is float)
      this.m_h = (float) values[1];
    if (values[2] is float)
      this.m_s = (float) values[2];
    if (values[3] is float)
      this.m_v = (float) values[3];
    if (parameter.ToString() == "Background")
      return (object) this.GenerateHSVBrush();
    if (parameter.ToString() == "VerticalSlider")
      return (object) this.GenerateSliderBrushHSV();
    return parameter.ToString() == "HorizontalSlider" ? (object) this.GenerateHorizontalSliderBrush() : (object) null;
  }

  public object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  private Brush GenerateHSVBrush()
  {
    switch (this.m_hsv)
    {
      case HSV.H:
        return this.GenerateHBrush();
      case HSV.S:
        return this.GenerateSBrush();
      case HSV.V:
        return this.GenerateVBrush();
      default:
        return this.GenerateHBrush();
    }
  }

  private Brush GenerateHBrush()
  {
    Color rgb = HsvColor.ConvertHsvToRgb((double) this.m_h, 1.0, 1.0);
    DrawingBrush hbrush = new DrawingBrush();
    DrawingGroup drawingGroup = new DrawingGroup();
    RectangleGeometry rectangleGeometry = new RectangleGeometry();
    rectangleGeometry.Rect = new Rect(0.0, 0.0, 1.0, 1.0);
    GeometryDrawing geometryDrawing1 = new GeometryDrawing();
    LinearGradientBrush linearGradientBrush1 = new LinearGradientBrush();
    linearGradientBrush1.StartPoint = new Point(1.0, 0.5);
    linearGradientBrush1.EndPoint = new Point(0.0, 0.5);
    linearGradientBrush1.GradientStops.Add(new GradientStop(rgb, 0.0));
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 1.0));
    geometryDrawing1.Brush = (Brush) linearGradientBrush1;
    geometryDrawing1.Geometry = (Geometry) rectangleGeometry;
    GeometryDrawing geometryDrawing2 = new GeometryDrawing();
    LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush();
    linearGradientBrush2.StartPoint = new Point(0.5, 1.0);
    linearGradientBrush2.EndPoint = new Point(0.5, 0.0);
    linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0), 0.0));
    linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb((byte) 0, (byte) 0, (byte) 0, (byte) 0), 1.0));
    geometryDrawing2.Brush = (Brush) linearGradientBrush2;
    geometryDrawing2.Geometry = (Geometry) rectangleGeometry;
    drawingGroup.Children.Add((Drawing) geometryDrawing1);
    drawingGroup.Children.Add((Drawing) geometryDrawing2);
    hbrush.Drawing = (Drawing) drawingGroup;
    return (Brush) hbrush;
  }

  private Brush GenerateSBrush()
  {
    DrawingBrush sbrush = new DrawingBrush();
    DrawingGroup drawingGroup = new DrawingGroup();
    RectangleGeometry rectangleGeometry = new RectangleGeometry();
    rectangleGeometry.Rect = new Rect(0.0, 0.0, 1.0, 1.0);
    GeometryDrawing geometryDrawing1 = new GeometryDrawing();
    LinearGradientBrush linearGradientBrush1 = new LinearGradientBrush();
    linearGradientBrush1.StartPoint = new Point(0.0, 0.5);
    linearGradientBrush1.EndPoint = new Point(1.0, 0.5);
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromScRgb(this.m_s, 1f, 0.0f, 0.0f), 0.0));
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromScRgb(this.m_s, 1f, 1f, 0.0f), 0.166));
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromScRgb(this.m_s, 0.0f, 1f, 0.0f), 0.333));
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromScRgb(this.m_s, 0.0f, 1f, 1f), 0.5));
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromScRgb(this.m_s, 0.0f, 0.0f, 1f), 0.666));
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromScRgb(this.m_s, 1f, 0.0f, 1f), 0.833));
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromScRgb(this.m_s, 1f, 0.0f, 0.0f), 1.0));
    geometryDrawing1.Brush = (Brush) linearGradientBrush1;
    geometryDrawing1.Geometry = (Geometry) rectangleGeometry;
    GeometryDrawing geometryDrawing2 = new GeometryDrawing();
    LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush();
    linearGradientBrush2.StartPoint = new Point(0.5, 0.0);
    linearGradientBrush2.EndPoint = new Point(0.5, 1.0);
    linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromScRgb(0.0f, 0.0f, 0.0f, 0.0f), 0.0));
    linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromScRgb(1f, 0.0f, 0.0f, 0.0f), 1.0));
    geometryDrawing2.Brush = (Brush) linearGradientBrush2;
    geometryDrawing2.Geometry = (Geometry) rectangleGeometry;
    drawingGroup.Children.Add((Drawing) geometryDrawing1);
    drawingGroup.Children.Add((Drawing) geometryDrawing2);
    sbrush.Drawing = (Drawing) drawingGroup;
    return (Brush) sbrush;
  }

  private Brush GenerateVBrush()
  {
    DrawingBrush vbrush = new DrawingBrush();
    DrawingGroup drawingGroup = new DrawingGroup();
    RectangleGeometry rectangleGeometry = new RectangleGeometry();
    rectangleGeometry.Rect = new Rect(0.0, 0.0, 1.0, 1.0);
    GeometryDrawing geometryDrawing1 = new GeometryDrawing();
    LinearGradientBrush linearGradientBrush1 = new LinearGradientBrush();
    linearGradientBrush1.StartPoint = new Point(0.0, 0.5);
    linearGradientBrush1.EndPoint = new Point(1.0, 0.5);
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 0, (byte) 0), 0.0));
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 0), 0.166));
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, (byte) 0, byte.MaxValue, (byte) 0), 0.333));
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, (byte) 0, byte.MaxValue, byte.MaxValue), 0.5));
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, byte.MaxValue), 0.666));
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue), 0.833));
    linearGradientBrush1.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 0, (byte) 0), 1.0));
    geometryDrawing1.Brush = (Brush) linearGradientBrush1;
    geometryDrawing1.Geometry = (Geometry) rectangleGeometry;
    GeometryDrawing geometryDrawing2 = new GeometryDrawing();
    LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush();
    linearGradientBrush2.StartPoint = new Point(0.5, 0.0);
    linearGradientBrush2.EndPoint = new Point(0.5, 1.0);
    float a = 1f - this.m_v;
    linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromScRgb(a, 0.0f, 0.0f, 0.0f), 0.0));
    linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromScRgb(a, 0.0f, 0.0f, 0.0f), 1.0));
    geometryDrawing2.Brush = (Brush) linearGradientBrush2;
    geometryDrawing2.Geometry = (Geometry) rectangleGeometry;
    GeometryDrawing geometryDrawing3 = new GeometryDrawing();
    LinearGradientBrush linearGradientBrush3 = new LinearGradientBrush();
    linearGradientBrush3.StartPoint = new Point(0.5, 0.0);
    linearGradientBrush3.EndPoint = new Point(0.5, 1.0);
    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromScRgb(0.0f, 1f, 1f, 1f), 0.0));
    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromScRgb(1f, 1f, 1f, 1f), 1.0));
    geometryDrawing3.Brush = (Brush) linearGradientBrush3;
    geometryDrawing3.Geometry = (Geometry) rectangleGeometry;
    drawingGroup.Children.Add((Drawing) geometryDrawing1);
    drawingGroup.Children.Add((Drawing) geometryDrawing3);
    drawingGroup.Children.Add((Drawing) geometryDrawing2);
    vbrush.Drawing = (Drawing) drawingGroup;
    return (Brush) vbrush;
  }

  private Brush GenerateSliderBrushHSV()
  {
    switch (this.m_hsv)
    {
      case HSV.H:
        return this.GenerateHSliderBrush();
      case HSV.S:
        return this.GenerateSSliderBrush();
      case HSV.V:
        return this.GenerateVSliderBrush();
      default:
        return this.GenerateHSliderBrush();
    }
  }

  private Brush GenerateHorizontalSliderBrush() => (Brush) this.GetHSliderBrush(0.0, 0.0, 1.0, 0.0);

  private Brush GenerateHSliderBrush() => (Brush) this.GetHSliderBrush(0.5, 1.0, 0.5, 0.0);

  private DrawingBrush GetHSliderBrush(
    double brushStartX,
    double brushStartY,
    double brushEndX,
    double brushEndY)
  {
    DrawingBrush hsliderBrush = new DrawingBrush();
    DrawingGroup drawingGroup = new DrawingGroup();
    RectangleGeometry rectangleGeometry = new RectangleGeometry();
    rectangleGeometry.Rect = new Rect(0.0, 0.0, 100.0, 100.0);
    GeometryDrawing geometryDrawing = new GeometryDrawing();
    LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
    linearGradientBrush.StartPoint = new Point(brushStartX, brushStartY);
    linearGradientBrush.EndPoint = new Point(brushEndX, brushEndY);
    linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromScRgb(1f, 1f, 0.0f, 0.0f), 0.0));
    linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromScRgb(1f, 1f, 1f, 0.0f), 0.166));
    linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromScRgb(1f, 0.0f, 1f, 0.0f), 0.333));
    linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromScRgb(1f, 0.0f, 1f, 1f), 0.5));
    linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromScRgb(1f, 0.0f, 0.0f, 1f), 0.666));
    linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromScRgb(1f, 1f, 0.0f, 1f), 0.833));
    linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromScRgb(1f, 1f, 0.0f, 0.0f), 1.0));
    geometryDrawing.Brush = (Brush) linearGradientBrush;
    geometryDrawing.Geometry = (Geometry) rectangleGeometry;
    drawingGroup.Children.Add((Drawing) geometryDrawing);
    hsliderBrush.Drawing = (Drawing) drawingGroup;
    return hsliderBrush;
  }

  private Brush GenerateSSliderBrush()
  {
    LinearGradientBrush ssliderBrush = new LinearGradientBrush();
    ssliderBrush.StartPoint = new Point(0.5, 1.0);
    ssliderBrush.EndPoint = new Point(0.5, 0.0);
    ssliderBrush.GradientStops.Add(new GradientStop(HsvColor.ConvertHsvToRgb((double) this.m_h, 0.0, (double) this.m_v), 0.0));
    ssliderBrush.GradientStops.Add(new GradientStop(HsvColor.ConvertHsvToRgb((double) this.m_h, 1.0, (double) this.m_v), 1.0));
    return (Brush) ssliderBrush;
  }

  private Brush GenerateVSliderBrush()
  {
    LinearGradientBrush vsliderBrush = new LinearGradientBrush();
    vsliderBrush.StartPoint = new Point(0.5, 0.0);
    vsliderBrush.EndPoint = new Point(0.5, 1.0);
    vsliderBrush.GradientStops.Add(new GradientStop(HsvColor.ConvertHsvToRgb((double) this.m_h, (double) this.m_s, 0.0), 1.0));
    vsliderBrush.GradientStops.Add(new GradientStop(HsvColor.ConvertHsvToRgb((double) this.m_h, (double) this.m_s, 1.0), 0.0));
    return (Brush) vsliderBrush;
  }
}
