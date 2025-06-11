// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.SelectedAdorner
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public class SelectedAdorner : Adorner
{
  private Pen _pen;

  public SelectedAdorner(UIElement adornedElement)
    : base(adornedElement)
  {
    this._pen = new Pen((Brush) Brushes.Black, 1.0)
    {
      DashStyle = new DashStyle((IEnumerable<double>) new double[2]
      {
        2.5,
        2.5
      }, 0.0)
    };
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    Rect adornedElementRect = this.GetAdornedElementRect();
    DrawingContext drawingContext1 = drawingContext;
    Pen pen1 = this._pen;
    Point point1 = adornedElementRect.TopLeft;
    double x1 = point1.X - 3.0;
    point1 = adornedElementRect.TopLeft;
    double y1 = point1.Y - 3.0;
    Point point0_1 = new Point(x1, y1);
    point1 = adornedElementRect.TopRight;
    double x2 = point1.X + 3.0;
    point1 = adornedElementRect.TopRight;
    double y2 = point1.Y - 3.0;
    Point point1_1 = new Point(x2, y2);
    drawingContext1.DrawLine(pen1, point0_1, point1_1);
    DrawingContext drawingContext2 = drawingContext;
    Pen pen2 = this._pen;
    Point point2 = adornedElementRect.TopRight;
    double x3 = point2.X + 3.0;
    point2 = adornedElementRect.TopRight;
    double y3 = point2.Y - 3.0;
    Point point0_2 = new Point(x3, y3);
    point2 = adornedElementRect.BottomRight;
    double x4 = point2.X + 3.0;
    point2 = adornedElementRect.BottomRight;
    double y4 = point2.Y + 3.0;
    Point point1_2 = new Point(x4, y4);
    drawingContext2.DrawLine(pen2, point0_2, point1_2);
    DrawingContext drawingContext3 = drawingContext;
    Pen pen3 = this._pen;
    Point point3 = adornedElementRect.BottomRight;
    double x5 = point3.X + 3.0;
    point3 = adornedElementRect.BottomRight;
    double y5 = point3.Y + 3.0;
    Point point0_3 = new Point(x5, y5);
    point3 = adornedElementRect.BottomLeft;
    double x6 = point3.X - 3.0;
    point3 = adornedElementRect.BottomLeft;
    double y6 = point3.Y + 3.0;
    Point point1_3 = new Point(x6, y6);
    drawingContext3.DrawLine(pen3, point0_3, point1_3);
    DrawingContext drawingContext4 = drawingContext;
    Pen pen4 = this._pen;
    Point point4 = adornedElementRect.BottomLeft;
    double x7 = point4.X - 3.0;
    point4 = adornedElementRect.BottomLeft;
    double y7 = point4.Y + 3.0;
    Point point0_4 = new Point(x7, y7);
    point4 = adornedElementRect.TopLeft;
    double x8 = point4.X - 3.0;
    point4 = adornedElementRect.TopLeft;
    double y8 = point4.Y - 3.0;
    Point point1_4 = new Point(x8, y8);
    drawingContext4.DrawLine(pen4, point0_4, point1_4);
    base.OnRender(drawingContext);
  }

  private Rect GetAdornedElementRect()
  {
    if (!(this.AdornedElement is Polyline))
      return new Rect(this.AdornedElement.RenderSize);
    Polyline adornedElement = (Polyline) this.AdornedElement;
    double x = adornedElement.Points.Min<Point>((Func<Point, double>) (p => p.X));
    double num1 = adornedElement.Points.Max<Point>((Func<Point, double>) (p => p.X));
    double y = adornedElement.Points.Min<Point>((Func<Point, double>) (p => p.Y));
    double num2 = adornedElement.Points.Max<Point>((Func<Point, double>) (p => p.Y));
    double width = num1 - x + adornedElement.StrokeThickness / 2.0;
    double num3 = y;
    double height = num2 - num3 + adornedElement.StrokeThickness / 2.0;
    return new Rect(x, y, width, height);
  }
}
