// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Border3D
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class Border3D : Border
{
  private const double DEF_LINE_THICKNESS = 1.0;
  private static readonly Color ColorBightest = Colors.White;
  private static readonly Color ColorBright = SystemColors.ControlColor;
  private static readonly Color ColorDark = Color.FromRgb((byte) 128 /*0x80*/, (byte) 128 /*0x80*/, (byte) 128 /*0x80*/);
  private static readonly Color ColorDarkest = Color.FromRgb((byte) 64 /*0x40*/, (byte) 64 /*0x40*/, (byte) 64 /*0x40*/);
  private Pen m_penBrightest;
  private Pen m_penBright;
  private Pen m_penDark;
  private Pen m_penDarkest;
  public static readonly DependencyProperty IsInvertedProperty = DependencyProperty.Register(nameof (IsInverted), typeof (bool), typeof (Border3D), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsRender));

  public bool IsInverted
  {
    get => (bool) this.GetValue(Border3D.IsInvertedProperty);
    set => this.SetValue(Border3D.IsInvertedProperty, (object) value);
  }

  public Border3D() => this.PreparePens();

  private Pen CreatePen(Color color)
  {
    Brush brush = (Brush) new SolidColorBrush(color);
    brush.Freeze();
    Pen pen = new Pen(brush, 1.0);
    pen.StartLineCap = PenLineCap.Square;
    pen.EndLineCap = PenLineCap.Square;
    pen.Freeze();
    return pen;
  }

  private void PreparePens()
  {
    this.m_penBrightest = this.CreatePen(Border3D.ColorBightest);
    this.m_penBright = this.CreatePen(Border3D.ColorBright);
    this.m_penDark = this.CreatePen(Border3D.ColorDark);
    this.m_penDarkest = this.CreatePen(Border3D.ColorDarkest);
  }

  protected override void OnRender(DrawingContext dc)
  {
    base.OnRender(dc);
    bool flag = this.BorderThickness.Left > 0.0 || this.BorderThickness.Right > 0.0 || this.BorderThickness.Top > 0.0 || this.BorderThickness.Bottom > 0.0;
    if (this.BorderBrush != null || !flag)
      return;
    double num1 = 0.5;
    double x1 = num1;
    double x2 = x1 + 1.0;
    double y1 = num1;
    double y2 = y1 + 1.0;
    double num2 = this.RenderSize.Width - num1;
    double num3 = num2 - 1.0;
    double y3 = this.RenderSize.Height - num1;
    double y4 = y3 - 1.0;
    double num4 = 0.5;
    GuidelineSet guidelines = new GuidelineSet(new double[4]
    {
      x1 - num4,
      x2 - num4,
      num3 + num4,
      num2 + num4
    }, new double[4]
    {
      y1 - num4,
      y2 - num4,
      y4 + num4,
      y3 + num4
    });
    guidelines.Freeze();
    dc.PushGuidelineSet(guidelines);
    Pen pen1 = !this.IsInverted ? this.m_penBrightest : this.m_penDarkest;
    Pen pen2 = !this.IsInverted ? this.m_penBright : this.m_penDark;
    Pen pen3 = !this.IsInverted ? this.m_penDark : this.m_penBright;
    Pen pen4 = !this.IsInverted ? this.m_penDarkest : this.m_penBrightest;
    if (this.BorderThickness.Left > 0.0)
    {
      dc.DrawLine(pen1, new Point(x1, y1 + this.CornerRadius.TopLeft), new Point(x1, y4 - this.CornerRadius.BottomLeft));
      dc.DrawLine(pen2, new Point(x2, y2 + this.CornerRadius.TopLeft), new Point(x2, y4 - this.CornerRadius.BottomLeft));
    }
    CornerRadius cornerRadius;
    if (this.BorderThickness.Top > 0.0)
    {
      dc.DrawLine(pen1, new Point(x1 + this.CornerRadius.TopLeft, y1), new Point(num3 - this.CornerRadius.TopRight, y1));
      DrawingContext drawingContext = dc;
      Pen pen5 = pen2;
      double num5 = x2;
      cornerRadius = this.CornerRadius;
      double topLeft = cornerRadius.TopLeft;
      Point point0 = new Point(num5 + topLeft, y2);
      double num6 = num3;
      cornerRadius = this.CornerRadius;
      double topRight = cornerRadius.TopRight;
      Point point1 = new Point(num6 - topRight, y2);
      drawingContext.DrawLine(pen5, point0, point1);
    }
    if (this.BorderThickness.Right > 0.0)
    {
      DrawingContext drawingContext1 = dc;
      Pen pen6 = pen3;
      double x3 = num3;
      double num7 = y2;
      cornerRadius = this.CornerRadius;
      double topRight1 = cornerRadius.TopRight;
      double y5 = num7 + topRight1;
      Point point0_1 = new Point(x3, y5);
      double x4 = num3;
      double num8 = y4;
      cornerRadius = this.CornerRadius;
      double bottomRight1 = cornerRadius.BottomRight;
      double y6 = num8 - bottomRight1;
      Point point1_1 = new Point(x4, y6);
      drawingContext1.DrawLine(pen6, point0_1, point1_1);
      DrawingContext drawingContext2 = dc;
      Pen pen7 = pen4;
      double x5 = num2;
      double num9 = y1;
      cornerRadius = this.CornerRadius;
      double topRight2 = cornerRadius.TopRight;
      double y7 = num9 + topRight2;
      Point point0_2 = new Point(x5, y7);
      double x6 = num2;
      double num10 = y3;
      cornerRadius = this.CornerRadius;
      double bottomRight2 = cornerRadius.BottomRight;
      double y8 = num10 - bottomRight2;
      Point point1_2 = new Point(x6, y8);
      drawingContext2.DrawLine(pen7, point0_2, point1_2);
    }
    if (this.BorderThickness.Bottom > 0.0)
    {
      DrawingContext drawingContext3 = dc;
      Pen pen8 = pen3;
      double num11 = x2;
      cornerRadius = this.CornerRadius;
      double bottomLeft1 = cornerRadius.BottomLeft;
      Point point0_3 = new Point(num11 + bottomLeft1, y4);
      double num12 = num3;
      cornerRadius = this.CornerRadius;
      double bottomRight3 = cornerRadius.BottomRight;
      Point point1_3 = new Point(num12 - bottomRight3, y4);
      drawingContext3.DrawLine(pen8, point0_3, point1_3);
      DrawingContext drawingContext4 = dc;
      Pen pen9 = pen4;
      double num13 = x1;
      cornerRadius = this.CornerRadius;
      double bottomLeft2 = cornerRadius.BottomLeft;
      Point point0_4 = new Point(num13 + bottomLeft2, y3);
      double num14 = num2;
      cornerRadius = this.CornerRadius;
      double bottomRight4 = cornerRadius.BottomRight;
      Point point1_4 = new Point(num14 - bottomRight4, y3);
      drawingContext4.DrawLine(pen9, point0_4, point1_4);
    }
    cornerRadius = this.CornerRadius;
    if (cornerRadius.TopLeft > 0.0 && (this.BorderThickness.Left > 0.0 || this.BorderThickness.Top > 0.0))
    {
      ArcSegment arcSegment1 = new ArcSegment();
      ArcSegment arcSegment2 = arcSegment1;
      double num15 = x1;
      cornerRadius = this.CornerRadius;
      double topLeft1 = cornerRadius.TopLeft;
      Point point1 = new Point(num15 + topLeft1, y1);
      arcSegment2.Point = point1;
      ArcSegment arcSegment3 = arcSegment1;
      cornerRadius = this.CornerRadius;
      double topLeft2 = cornerRadius.TopLeft;
      cornerRadius = this.CornerRadius;
      double topLeft3 = cornerRadius.TopLeft;
      Size size1 = new Size(topLeft2, topLeft3);
      arcSegment3.Size = size1;
      arcSegment1.SweepDirection = SweepDirection.Clockwise;
      PathFigure pathFigure1 = new PathFigure();
      PathFigure pathFigure2 = pathFigure1;
      double x7 = x1;
      double num16 = y1;
      cornerRadius = this.CornerRadius;
      double topLeft4 = cornerRadius.TopLeft;
      double y9 = num16 + topLeft4;
      Point point2 = new Point(x7, y9);
      pathFigure2.StartPoint = point2;
      pathFigure1.Segments.Add((PathSegment) arcSegment1);
      Drawing drawing1 = (Drawing) new GeometryDrawing((Brush) null, pen1, (Geometry) new PathGeometry((IEnumerable<PathFigure>) new List<PathFigure>()
      {
        pathFigure1
      }));
      ArcSegment arcSegment4 = new ArcSegment();
      ArcSegment arcSegment5 = arcSegment4;
      double num17 = x2;
      cornerRadius = this.CornerRadius;
      double topLeft5 = cornerRadius.TopLeft;
      Point point3 = new Point(num17 + topLeft5, y2);
      arcSegment5.Point = point3;
      ArcSegment arcSegment6 = arcSegment4;
      cornerRadius = this.CornerRadius;
      double topLeft6 = cornerRadius.TopLeft;
      cornerRadius = this.CornerRadius;
      double topLeft7 = cornerRadius.TopLeft;
      Size size2 = new Size(topLeft6, topLeft7);
      arcSegment6.Size = size2;
      arcSegment4.SweepDirection = SweepDirection.Clockwise;
      PathFigure pathFigure3 = new PathFigure();
      PathFigure pathFigure4 = pathFigure3;
      double x8 = x2;
      double num18 = y1;
      cornerRadius = this.CornerRadius;
      double topLeft8 = cornerRadius.TopLeft;
      double y10 = num18 + topLeft8;
      Point point4 = new Point(x8, y10);
      pathFigure4.StartPoint = point4;
      pathFigure3.Segments.Add((PathSegment) arcSegment4);
      Drawing drawing2 = (Drawing) new GeometryDrawing((Brush) null, pen2, (Geometry) new PathGeometry((IEnumerable<PathFigure>) new List<PathFigure>()
      {
        pathFigure3
      }));
      dc.DrawDrawing(drawing1);
      dc.DrawDrawing(drawing2);
    }
    cornerRadius = this.CornerRadius;
    if (cornerRadius.TopRight > 0.0 && (this.BorderThickness.Top > 0.0 || this.BorderThickness.Right > 0.0))
    {
      ArcSegment arcSegment7 = new ArcSegment();
      ArcSegment arcSegment8 = arcSegment7;
      double x9 = num2;
      double num19 = y1;
      cornerRadius = this.CornerRadius;
      double topRight3 = cornerRadius.TopRight;
      double y11 = num19 + topRight3;
      Point point5 = new Point(x9, y11);
      arcSegment8.Point = point5;
      ArcSegment arcSegment9 = arcSegment7;
      cornerRadius = this.CornerRadius;
      double topRight4 = cornerRadius.TopRight;
      cornerRadius = this.CornerRadius;
      double topRight5 = cornerRadius.TopRight;
      Size size3 = new Size(topRight4, topRight5);
      arcSegment9.Size = size3;
      arcSegment7.SweepDirection = SweepDirection.Clockwise;
      PathFigure pathFigure5 = new PathFigure();
      PathFigure pathFigure6 = pathFigure5;
      double num20 = num2;
      cornerRadius = this.CornerRadius;
      double topRight6 = cornerRadius.TopRight;
      Point point6 = new Point(num20 - topRight6, y1);
      pathFigure6.StartPoint = point6;
      pathFigure5.Segments.Add((PathSegment) arcSegment7);
      Drawing drawing3 = (Drawing) new GeometryDrawing((Brush) null, pen1, (Geometry) new PathGeometry((IEnumerable<PathFigure>) new List<PathFigure>()
      {
        pathFigure5
      }));
      ArcSegment arcSegment10 = new ArcSegment();
      ArcSegment arcSegment11 = arcSegment10;
      double x10 = num3;
      double num21 = y2;
      cornerRadius = this.CornerRadius;
      double topRight7 = cornerRadius.TopRight;
      double y12 = num21 + topRight7;
      Point point7 = new Point(x10, y12);
      arcSegment11.Point = point7;
      ArcSegment arcSegment12 = arcSegment10;
      cornerRadius = this.CornerRadius;
      double topRight8 = cornerRadius.TopRight;
      cornerRadius = this.CornerRadius;
      double topRight9 = cornerRadius.TopRight;
      Size size4 = new Size(topRight8, topRight9);
      arcSegment12.Size = size4;
      arcSegment10.SweepDirection = SweepDirection.Clockwise;
      PathFigure pathFigure7 = new PathFigure();
      PathFigure pathFigure8 = pathFigure7;
      double num22 = num3;
      cornerRadius = this.CornerRadius;
      double topRight10 = cornerRadius.TopRight;
      Point point8 = new Point(num22 - topRight10, y2);
      pathFigure8.StartPoint = point8;
      pathFigure7.Segments.Add((PathSegment) arcSegment10);
      Drawing drawing4 = (Drawing) new GeometryDrawing((Brush) null, pen2, (Geometry) new PathGeometry((IEnumerable<PathFigure>) new List<PathFigure>()
      {
        pathFigure7
      }));
      dc.DrawDrawing(drawing3);
      dc.DrawDrawing(drawing4);
    }
    cornerRadius = this.CornerRadius;
    if (cornerRadius.BottomRight > 0.0 && (this.BorderThickness.Right > 0.0 || this.BorderThickness.Bottom > 0.0))
    {
      ArcSegment arcSegment13 = new ArcSegment();
      ArcSegment arcSegment14 = arcSegment13;
      double num23 = num2;
      cornerRadius = this.CornerRadius;
      double bottomRight5 = cornerRadius.BottomRight;
      Point point9 = new Point(num23 - bottomRight5, y3);
      arcSegment14.Point = point9;
      ArcSegment arcSegment15 = arcSegment13;
      cornerRadius = this.CornerRadius;
      double bottomRight6 = cornerRadius.BottomRight;
      cornerRadius = this.CornerRadius;
      double bottomRight7 = cornerRadius.BottomRight;
      Size size5 = new Size(bottomRight6, bottomRight7);
      arcSegment15.Size = size5;
      arcSegment13.SweepDirection = SweepDirection.Clockwise;
      PathFigure pathFigure9 = new PathFigure();
      PathFigure pathFigure10 = pathFigure9;
      double x11 = num2;
      double num24 = y3;
      cornerRadius = this.CornerRadius;
      double bottomRight8 = cornerRadius.BottomRight;
      double y13 = num24 - bottomRight8;
      Point point10 = new Point(x11, y13);
      pathFigure10.StartPoint = point10;
      pathFigure9.Segments.Add((PathSegment) arcSegment13);
      Drawing drawing5 = (Drawing) new GeometryDrawing((Brush) null, pen4, (Geometry) new PathGeometry((IEnumerable<PathFigure>) new List<PathFigure>()
      {
        pathFigure9
      }));
      ArcSegment arcSegment16 = new ArcSegment();
      ArcSegment arcSegment17 = arcSegment16;
      double num25 = num3;
      cornerRadius = this.CornerRadius;
      double bottomRight9 = cornerRadius.BottomRight;
      Point point11 = new Point(num25 - bottomRight9, y4);
      arcSegment17.Point = point11;
      ArcSegment arcSegment18 = arcSegment16;
      cornerRadius = this.CornerRadius;
      double bottomRight10 = cornerRadius.BottomRight;
      cornerRadius = this.CornerRadius;
      double bottomRight11 = cornerRadius.BottomRight;
      Size size6 = new Size(bottomRight10, bottomRight11);
      arcSegment18.Size = size6;
      arcSegment16.SweepDirection = SweepDirection.Clockwise;
      PathFigure pathFigure11 = new PathFigure();
      PathFigure pathFigure12 = pathFigure11;
      double x12 = num3;
      double num26 = y4;
      cornerRadius = this.CornerRadius;
      double bottomRight12 = cornerRadius.BottomRight;
      double y14 = num26 - bottomRight12;
      Point point12 = new Point(x12, y14);
      pathFigure12.StartPoint = point12;
      pathFigure11.Segments.Add((PathSegment) arcSegment16);
      Drawing drawing6 = (Drawing) new GeometryDrawing((Brush) null, pen3, (Geometry) new PathGeometry((IEnumerable<PathFigure>) new List<PathFigure>()
      {
        pathFigure11
      }));
      dc.DrawDrawing(drawing5);
      dc.DrawDrawing(drawing6);
    }
    cornerRadius = this.CornerRadius;
    if (cornerRadius.BottomLeft > 0.0 && (this.BorderThickness.Bottom > 0.0 || this.BorderThickness.Left > 0.0))
    {
      ArcSegment arcSegment19 = new ArcSegment();
      ArcSegment arcSegment20 = arcSegment19;
      double x13 = x1;
      double num27 = y3;
      cornerRadius = this.CornerRadius;
      double bottomLeft3 = cornerRadius.BottomLeft;
      double y15 = num27 - bottomLeft3;
      Point point13 = new Point(x13, y15);
      arcSegment20.Point = point13;
      ArcSegment arcSegment21 = arcSegment19;
      cornerRadius = this.CornerRadius;
      double bottomLeft4 = cornerRadius.BottomLeft;
      cornerRadius = this.CornerRadius;
      double bottomLeft5 = cornerRadius.BottomLeft;
      Size size7 = new Size(bottomLeft4, bottomLeft5);
      arcSegment21.Size = size7;
      arcSegment19.SweepDirection = SweepDirection.Clockwise;
      PathFigure pathFigure13 = new PathFigure();
      PathFigure pathFigure14 = pathFigure13;
      double num28 = x1;
      cornerRadius = this.CornerRadius;
      double bottomLeft6 = cornerRadius.BottomLeft;
      Point point14 = new Point(num28 + bottomLeft6, y3);
      pathFigure14.StartPoint = point14;
      pathFigure13.Segments.Add((PathSegment) arcSegment19);
      Drawing drawing7 = (Drawing) new GeometryDrawing((Brush) null, pen1, (Geometry) new PathGeometry((IEnumerable<PathFigure>) new List<PathFigure>()
      {
        pathFigure13
      }));
      ArcSegment arcSegment22 = new ArcSegment();
      ArcSegment arcSegment23 = arcSegment22;
      double x14 = x2;
      double num29 = y4;
      cornerRadius = this.CornerRadius;
      double bottomLeft7 = cornerRadius.BottomLeft;
      double y16 = num29 - bottomLeft7;
      Point point15 = new Point(x14, y16);
      arcSegment23.Point = point15;
      ArcSegment arcSegment24 = arcSegment22;
      cornerRadius = this.CornerRadius;
      double bottomLeft8 = cornerRadius.BottomLeft;
      cornerRadius = this.CornerRadius;
      double bottomLeft9 = cornerRadius.BottomLeft;
      Size size8 = new Size(bottomLeft8, bottomLeft9);
      arcSegment24.Size = size8;
      arcSegment22.SweepDirection = SweepDirection.Clockwise;
      PathFigure pathFigure15 = new PathFigure();
      PathFigure pathFigure16 = pathFigure15;
      double num30 = x2;
      cornerRadius = this.CornerRadius;
      double bottomLeft10 = cornerRadius.BottomLeft;
      Point point16 = new Point(num30 + bottomLeft10, y4);
      pathFigure16.StartPoint = point16;
      pathFigure15.Segments.Add((PathSegment) arcSegment22);
      Drawing drawing8 = (Drawing) new GeometryDrawing((Brush) null, pen2, (Geometry) new PathGeometry((IEnumerable<PathFigure>) new List<PathFigure>()
      {
        pathFigure15
      }));
      dc.DrawDrawing(drawing7);
      dc.DrawDrawing(drawing8);
    }
    dc.Pop();
  }
}
