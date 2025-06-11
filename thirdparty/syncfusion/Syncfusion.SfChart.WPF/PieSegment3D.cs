// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.PieSegment3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class PieSegment3D : ChartSegment3D
{
  private const double DtoR = 0.017453292519943295;
  internal static readonly DependencyProperty ActualStartValueProperty = DependencyProperty.Register(nameof (ActualStartValue), typeof (double), typeof (PieSegment3D), new PropertyMetadata((object) 0.0));
  internal static readonly DependencyProperty ActualEndValueProperty = DependencyProperty.Register(nameof (ActualEndValue), typeof (double), typeof (PieSegment3D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(PieSegment3D.OnValuesChanged)));
  private readonly SfChart3D area;
  private readonly PieSeries3D series3D;
  private double inSideRadius;
  private double depth;
  private double radius;
  private int pieIndex;

  public PieSegment3D() => this.Points = new List<Point>();

  public PieSegment3D(
    ChartSeries3D series,
    Vector3D center,
    double start,
    double end,
    double height,
    double r,
    int i,
    double y,
    double insideRadius)
  {
    this.Points = new List<Point>();
    if (series.ToggledLegendIndex.Contains(i))
      this.IsSegmentVisible = false;
    else
      this.IsSegmentVisible = true;
    this.Series = (ChartSeriesBase) (this.series3D = series as PieSeries3D);
    this.area = series.Area;
    this.Item = series.ActualData[i];
    this.StartValue = start;
    this.EndValue = end;
    this.depth = height;
    this.radius = r;
    if (this.series3D != null)
      this.pieIndex = this.series3D.GetPieSeriesIndex();
    this.Index = i;
    this.YData = y;
    this.Center = center;
    this.inSideRadius = insideRadius;
    if (series.CanAnimate)
      return;
    if (this.series3D.StartAngle < this.series3D.EndAngle)
    {
      this.ActualStartValue = start;
      this.ActualEndValue = end - start;
    }
    else
    {
      this.ActualStartValue = end;
      this.ActualEndValue = start - end;
    }
  }

  public double YData { get; set; }

  public double XData { get; set; }

  internal int Index { get; set; }

  internal List<Point> Points { get; set; }

  internal Vector3D Center { get; set; }

  internal double StartValue { get; set; }

  internal double EndValue { get; set; }

  internal double ActualStartValue
  {
    get => (double) this.GetValue(PieSegment3D.ActualStartValueProperty);
    set => this.SetValue(PieSegment3D.ActualStartValueProperty, (object) value);
  }

  internal double ActualEndValue
  {
    get => (double) this.GetValue(PieSegment3D.ActualEndValueProperty);
    set => this.SetValue(PieSegment3D.ActualEndValueProperty, (object) value);
  }

  public override UIElement CreateVisual(Size size) => (UIElement) null;

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override void Update(IChartTransformer transformer) => this.CreateSector();

  public override void OnSizeChanged(Size size)
  {
  }

  public override void SetData(params double[] values)
  {
    this.StartValue = values[0];
    this.EndValue = values[1];
    this.depth = values[2];
    this.radius = values[3];
    this.YData = values[4];
    this.Center = new Vector3D(values[5], values[6], values[7]);
    this.inSideRadius = values[8];
    if (this.series3D.StartAngle < this.series3D.EndAngle)
    {
      this.ActualStartValue = values[0];
      this.ActualEndValue = values[1] - values[0];
    }
    else
    {
      this.ActualStartValue = values[1];
      this.ActualEndValue = values[0] - values[1];
    }
  }

  internal Polygon3D[][] CreateSector()
  {
    this.Points.Clear();
    int length = (int) Math.Ceiling(this.ActualEndValue / 6.0);
    if ((double) length < 1.0)
      return (Polygon3D[][]) null;
    Polygon3D[][] sector = new Polygon3D[4][];
    double num = this.ActualEndValue / (double) length;
    Point[] pointArray1 = new Point[length + 1];
    Point[] pointArray2 = new Point[length + 1];
    for (int index = 0; index < length + 1; ++index)
    {
      float x1 = (float) (this.Center.X + this.radius * Math.Cos((this.ActualStartValue + (double) index * num) * (Math.PI / 180.0)));
      float y1 = (float) (this.Center.Y + this.radius * Math.Sin((this.ActualStartValue + (double) index * num) * (Math.PI / 180.0)));
      pointArray1[index] = new Point((double) x1, (double) y1);
      float x2 = (float) (this.Center.X + this.inSideRadius * Math.Cos((this.ActualStartValue + (double) index * num) * (Math.PI / 180.0)));
      float y2 = (float) (this.Center.Y + this.inSideRadius * Math.Sin((this.ActualStartValue + (double) index * num) * (Math.PI / 180.0)));
      pointArray2[index] = new Point((double) x2, (double) y2);
      this.Points.Add(new Point((double) x1, (double) y1));
    }
    Polygon3D[] polygon3DArray1 = new Polygon3D[length];
    for (int index = 0; index < length; ++index)
    {
      Vector3D[] points = new Vector3D[4]
      {
        new Vector3D(pointArray1[index].X, pointArray1[index].Y, 0.0),
        new Vector3D(pointArray1[index].X, pointArray1[index].Y, this.depth),
        new Vector3D(pointArray1[index + 1].X, pointArray1[index + 1].Y, this.depth),
        new Vector3D(pointArray1[index + 1].X, pointArray1[index + 1].Y, 0.0)
      };
      polygon3DArray1[index] = new Polygon3D(points, (DependencyObject) this, this.Index, this.Stroke, this.StrokeThickness, this.Interior);
      polygon3DArray1[index].CalcNormal(points[0], points[1], points[2]);
      polygon3DArray1[index].CalcNormal();
    }
    sector[1] = polygon3DArray1;
    if (this.inSideRadius > 0.0)
    {
      Polygon3D[] polygon3DArray2 = new Polygon3D[length];
      for (int index = 0; index < length; ++index)
      {
        Vector3D[] points = new Vector3D[4]
        {
          new Vector3D(pointArray2[index].X, pointArray2[index].Y, 0.0),
          new Vector3D(pointArray2[index].X, pointArray2[index].Y, this.depth),
          new Vector3D(pointArray2[index + 1].X, pointArray2[index + 1].Y, this.depth),
          new Vector3D(pointArray2[index + 1].X, pointArray2[index + 1].Y, 0.0)
        };
        polygon3DArray2[index] = new Polygon3D(points, (DependencyObject) this, this.Index, this.Stroke, this.StrokeThickness, this.Interior);
        polygon3DArray2[index].CalcNormal(points[0], points[1], points[2]);
        polygon3DArray2[index].CalcNormal();
      }
      sector[3] = polygon3DArray2;
    }
    List<Vector3D> vector3DList1 = new List<Vector3D>();
    List<Vector3D> vector3DList2 = new List<Vector3D>();
    for (int index = 0; index < length + 1; ++index)
    {
      vector3DList1.Add(new Vector3D(pointArray1[index].X, pointArray1[index].Y, 0.0));
      vector3DList2.Add(new Vector3D(pointArray1[index].X, pointArray1[index].Y, this.depth));
    }
    if (this.inSideRadius > 0.0)
    {
      for (int index = length; index > -1; --index)
      {
        vector3DList1.Add(new Vector3D(pointArray2[index].X, pointArray2[index].Y, 0.0));
        vector3DList2.Add(new Vector3D(pointArray2[index].X, pointArray2[index].Y, this.depth));
      }
    }
    else
    {
      vector3DList1.Add(this.Center);
      vector3DList2.Add(new Vector3D(this.Center.X, this.Center.Y, this.depth));
    }
    Polygon3D polygon3D1 = new Polygon3D(vector3DList1.ToArray(), (DependencyObject) this, this.Index, this.Stroke, this.StrokeThickness, this.Interior);
    polygon3D1.CalcNormal(vector3DList1.ToArray()[0], vector3DList1.ToArray()[1], vector3DList1.ToArray()[2]);
    polygon3D1.CalcNormal();
    Polygon3D polygon3D2 = new Polygon3D(vector3DList2.ToArray(), (DependencyObject) this, this.Index, this.Stroke, this.StrokeThickness, this.Interior);
    polygon3D2.CalcNormal(vector3DList2.ToArray()[0], vector3DList2.ToArray()[1], vector3DList2.ToArray()[2]);
    polygon3D2.CalcNormal();
    sector[0] = new Polygon3D[2]{ polygon3D1, polygon3D2 };
    if (this.inSideRadius > 0.0)
    {
      Vector3D[] points1 = new Vector3D[4]
      {
        new Vector3D(pointArray1[0].X, this.ActualStartValue >= 0.0 && this.ActualStartValue <= 90.0 || this.ActualStartValue >= 270.0 && this.ActualStartValue <= 360.0 ? pointArray1[0].Y + 0.1 : pointArray1[0].Y - 0.1, 0.0),
        new Vector3D(pointArray1[0].X, this.ActualStartValue >= 0.0 && this.ActualStartValue <= 90.0 || this.ActualStartValue >= 270.0 && this.ActualStartValue <= 360.0 ? pointArray1[0].Y + 0.1 : pointArray1[0].Y - 0.1, this.depth),
        new Vector3D(pointArray2[0].X, pointArray2[0].Y, this.depth),
        new Vector3D(pointArray2[0].X, pointArray2[0].Y, 0.0)
      };
      Vector3D[] points2 = new Vector3D[4]
      {
        new Vector3D(pointArray1[length].X, pointArray1[length].Y, 0.0),
        new Vector3D(pointArray1[length].X, pointArray1[length].Y, this.depth),
        new Vector3D(pointArray2[length].X, pointArray2[length].Y, this.depth),
        new Vector3D(pointArray2[length].X, pointArray2[length].Y, 0.0)
      };
      Polygon3D polygon3D3 = new Polygon3D(points1, (DependencyObject) this, this.Index, this.Stroke, this.StrokeThickness, this.Interior);
      polygon3D3.CalcNormal(points1[0], points1[1], points1[2]);
      polygon3D3.CalcNormal();
      Polygon3D polygon3D4 = new Polygon3D(points2, (DependencyObject) this, this.Index, this.Stroke, this.StrokeThickness, this.Interior);
      polygon3D4.CalcNormal(points2[0], points2[1], points2[2]);
      polygon3D4.CalcNormal();
      sector[2] = new Polygon3D[2]{ polygon3D3, polygon3D4 };
    }
    else
    {
      Vector3D[] points3 = new Vector3D[4]
      {
        new Vector3D(pointArray1[0].X, this.ActualStartValue >= 0.0 && this.ActualStartValue <= 90.0 || this.ActualStartValue >= 270.0 && this.ActualStartValue <= 360.0 ? pointArray1[0].Y + 0.1 : pointArray1[0].Y - 0.1, 0.0),
        new Vector3D(pointArray1[0].X, this.ActualStartValue >= 0.0 && this.ActualStartValue <= 90.0 || this.ActualStartValue >= 270.0 && this.ActualStartValue <= 360.0 ? pointArray1[0].Y + 0.1 : pointArray1[0].Y - 0.1, this.depth),
        new Vector3D(this.Center.X, this.Center.Y, this.depth),
        new Vector3D(this.Center.X, this.Center.Y, 0.0)
      };
      Vector3D[] points4 = new Vector3D[4]
      {
        new Vector3D(pointArray1[length].X, pointArray1[length].Y, 0.0),
        new Vector3D(pointArray1[length].X, pointArray1[length].Y, this.depth),
        new Vector3D(this.Center.X, this.Center.Y, this.depth),
        new Vector3D(this.Center.X, this.Center.Y, 0.0)
      };
      Polygon3D polygon3D5 = new Polygon3D(points3, (DependencyObject) this, this.Index, this.Stroke, this.StrokeThickness, this.Interior);
      polygon3D5.CalcNormal(points3[0], points3[1], points3[2]);
      polygon3D5.CalcNormal();
      Polygon3D polygon3D6 = new Polygon3D(points4, (DependencyObject) this, this.Index, this.Stroke, this.StrokeThickness, this.Interior);
      polygon3D6.CalcNormal(points4[0], points4[1], points4[2]);
      polygon3D6.CalcNormal();
      sector[2] = new Polygon3D[2]{ polygon3D5, polygon3D6 };
    }
    return sector;
  }

  private static void OnValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is PieSegment3D pieSegment3D))
      return;
    pieSegment3D.ScheduleRender();
  }

  private void ScheduleRender()
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new Action(this.OnValuesChanged));
  }

  private void OnValuesChanged()
  {
    if (this.series3D == null || !this.series3D.EnableAnimation || this.series3D.Segments == null)
      return;
    int num = this.series3D.Segments.IndexOf((ChartSegment) this);
    List<Polygon3D> visual = this.area.Graphics3D.GetVisual();
    foreach (Polygon3D polygon3D in visual.Where<Polygon3D>((Func<Polygon3D, bool>) (item => item.Tag == this)).ToList<Polygon3D>())
      visual.Remove(polygon3D);
    if (num != this.series3D.Segments.Count - 1)
      return;
    this.series3D.UpdateOnSeriesBoundChanged(Size.Empty);
    if (this.series3D.adornmentInfo != null)
    {
      foreach (UIElement3D polygon in this.area.Graphics3D.GetVisual().OfType<UIElement3D>().ToList<UIElement3D>())
      {
        this.area.Graphics3D.Remove((Polygon3D) polygon);
        this.area.Graphics3D.AddVisual((Polygon3D) polygon);
      }
    }
    if (this.pieIndex != 0)
      return;
    this.ScheduleView();
  }

  private void ScheduleView()
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new Action(this.OnViewChanged));
  }

  private void OnViewChanged()
  {
    this.area.Graphics3D.PrepareView();
    this.area.Graphics3D.View((Panel) this.area.RootPanel);
  }
}
