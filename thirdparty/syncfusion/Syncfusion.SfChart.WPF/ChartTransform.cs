// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartTransform
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public static class ChartTransform
{
  public static IChartTransformer CreateSimple(Size viewport)
  {
    return (IChartTransformer) new ChartTransform.ChartSimpleTransformer(viewport);
  }

  public static IChartTransformer CreateSimple3D(Size viewPort)
  {
    return (IChartTransformer) new ChartTransform.ChartTransform3D(viewPort);
  }

  public static IChartTransformer CreateCartesian(Size viewport, ChartSeriesBase series)
  {
    return (IChartTransformer) new ChartTransform.ChartCartesianTransformer(viewport, series);
  }

  public static IChartTransformer CreateCartesian(Size viewport, ChartAxis xAxis, ChartAxis yAxis)
  {
    return (IChartTransformer) new ChartTransform.ChartCartesianTransformer(viewport, xAxis, yAxis);
  }

  public static IChartTransformer CreatePolar(Rect viewport, ChartSeriesBase series)
  {
    return (IChartTransformer) new ChartTransform.ChartPolarTransformer(viewport, series);
  }

  public static Point CoefficientToVector(double coefficient)
  {
    double num = Math.PI * (1.5 - 2.0 * coefficient);
    return new Point(Math.Cos(num), Math.Sin(num));
  }

  public static double RadianToPolarCoefficient(double radian)
  {
    return radian <= 3.0 * Math.PI / 2.0 ? (1.5 - radian / Math.PI) / 2.0 : 0.75 + (0.75 - (1.5 - (2.0 * Math.PI - radian) / Math.PI) / 2.0);
  }

  public static double PointToPolarRadian(Point point, double center)
  {
    double x = Math.Abs(point.X - center);
    double num = Math.Atan2(Math.Abs(point.Y - center), x);
    if (point.X - center < 0.0 && point.Y - center < 0.0)
      return Math.PI + num;
    if (point.X - center < 0.0 && point.Y - center > 0.0)
      return Math.PI - num;
    return point.X - center > 0.0 && point.Y - center < 0.0 ? 2.0 * Math.PI - num : num;
  }

  public static Point ValueToVector(ChartAxis axis, double value)
  {
    return ChartTransform.CoefficientToVector(axis.ValueToPolarCoefficient(value));
  }

  private class ChartSimpleTransformer : IChartTransformer
  {
    private Size m_viewport = Size.Empty;

    public Size Viewport => this.m_viewport;

    public ChartSimpleTransformer(Size viewport) => this.m_viewport = viewport;

    public Point TransformToVisible(double x, double y) => new Point(x, y);
  }

  public class ChartCartesianTransformer : IChartTransformer
  {
    private Size m_viewport = Size.Empty;
    public ChartAxis XAxis;
    public ChartAxis YAxis;
    internal ChartAxis ZAxis;
    private bool m_IsRoated;
    private bool x_IsLogarithmic;
    private bool y_IsLogarithmic;
    private bool z_IsLogarithmic;
    private double xlogarithmicBase;
    private double ylogarithmicBase;
    private double zlogarithmicBase;

    public Size Viewport => this.m_viewport;

    public ChartCartesianTransformer(Size viewport, ChartAxis xAxis, ChartAxis yAxis)
    {
      this.m_viewport = viewport;
      this.XAxis = xAxis;
      this.YAxis = yAxis;
    }

    public ChartCartesianTransformer(Size viewport, ChartSeriesBase series)
    {
      if (series.ActualXAxis == null || series.ActualYAxis == null)
        return;
      this.m_viewport = viewport;
      this.XAxis = series.ActualXAxis;
      this.YAxis = series.ActualYAxis;
      ChartAxis chartAxis1 = (ChartAxis) null;
      if (this.XAxis is LogarithmicAxis3D)
        chartAxis1 = (ChartAxis) (this.XAxis as LogarithmicAxis3D);
      else if (this.XAxis is LogarithmicAxis)
        chartAxis1 = (ChartAxis) (this.XAxis as LogarithmicAxis);
      ChartAxis chartAxis2 = (ChartAxis) null;
      if (this.YAxis is LogarithmicAxis3D)
        chartAxis2 = (ChartAxis) (this.YAxis as LogarithmicAxis3D);
      else if (this.YAxis is LogarithmicAxis)
        chartAxis2 = (ChartAxis) (this.YAxis as LogarithmicAxis);
      if (chartAxis1 != null)
      {
        this.x_IsLogarithmic = true;
        this.xlogarithmicBase = !(chartAxis1 is LogarithmicAxis) ? ((LogarithmicAxis3D) series.ActualXAxis).LogarithmicBase : ((LogarithmicAxis) series.ActualXAxis).LogarithmicBase;
      }
      if (chartAxis2 != null)
      {
        this.y_IsLogarithmic = true;
        this.ylogarithmicBase = !(chartAxis2 is LogarithmicAxis) ? ((LogarithmicAxis3D) series.ActualYAxis).LogarithmicBase : ((LogarithmicAxis) series.ActualYAxis).LogarithmicBase;
      }
      this.m_IsRoated = series.IsActualTransposed;
      if (!(series is XyzDataSeries3D xyzDataSeries3D) || string.IsNullOrEmpty(xyzDataSeries3D.ZBindingPath))
        return;
      this.ZAxis = xyzDataSeries3D.ActualZAxis;
      if (!(this.ZAxis is LogarithmicAxis3D zaxis))
        return;
      this.z_IsLogarithmic = true;
      this.zlogarithmicBase = zaxis.LogarithmicBase;
    }

    public Point TransformToVisible(double x, double y)
    {
      if (this.XAxis == null || this.YAxis == null || double.IsNaN(this.m_viewport.Width) || double.IsNaN(this.m_viewport.Height))
        return new Point(0.0, 0.0);
      ChartBase area = this.XAxis.Area;
      x = x = !this.x_IsLogarithmic || x <= 0.0 ? x : Math.Log(x, this.xlogarithmicBase);
      y = !this.y_IsLogarithmic || y <= 0.0 ? y : Math.Log(y, this.ylogarithmicBase);
      bool flag = area is SfChart;
      if (this.m_IsRoated)
      {
        double num1 = flag ? this.YAxis.RenderedRect.Left - this.XAxis.Area.SeriesClipRect.Left : this.YAxis.RenderedRect.Left;
        double num2 = flag ? this.XAxis.RenderedRect.Top - this.YAxis.Area.SeriesClipRect.Top : this.XAxis.RenderedRect.Top;
        return new Point(num1 + this.YAxis.RenderedRect.Width * this.YAxis.ValueToCoefficientCalc(y), num2 + this.XAxis.RenderedRect.Height * (1.0 - this.XAxis.ValueToCoefficientCalc(x)));
      }
      if (flag)
      {
        double num3 = this.XAxis.RenderedRect.Left - this.XAxis.Area.SeriesClipRect.Left;
        double num4 = this.YAxis.RenderedRect.Top - this.YAxis.Area.SeriesClipRect.Top;
        return new Point(num3 + this.XAxis.RenderedRect.Width * this.XAxis.ValueToCoefficientCalc(x), num4 + this.YAxis.RenderedRect.Height * (1.0 - this.YAxis.ValueToCoefficientCalc(y)));
      }
      double left = this.XAxis.RenderedRect.Left;
      double top = this.YAxis.RenderedRect.Top;
      return new Point(left + Math.Round(this.XAxis.RenderedRect.Width * this.XAxis.ValueToCoefficientCalc(x)), top + Math.Round(this.YAxis.RenderedRect.Height * (1.0 - this.YAxis.ValueToCoefficientCalc(y))));
    }

    internal Vector3D TransformToVisible3D(double x, double y, double z)
    {
      if (this.XAxis == null || this.YAxis == null || double.IsNaN(this.m_viewport.Width) || double.IsNaN(this.m_viewport.Height) || this.ZAxis == null || double.IsNaN(this.m_viewport.Width) || double.IsNaN(this.m_viewport.Height))
        return new Vector3D(0.0, 0.0, 0.0);
      ChartBase area = this.XAxis.Area;
      x = x = !this.x_IsLogarithmic || x <= 0.0 ? x : Math.Log(x, this.xlogarithmicBase);
      y = !this.y_IsLogarithmic || y <= 0.0 ? y : Math.Log(y, this.ylogarithmicBase);
      z = !this.z_IsLogarithmic || z <= 0.0 ? z : Math.Log(z, this.zlogarithmicBase);
      bool flag = area is SfChart;
      double num1 = flag ? this.ZAxis.RenderedRect.Left - this.ZAxis.Area.SeriesClipRect.Left : this.ZAxis.RenderedRect.Left;
      if (this.m_IsRoated)
      {
        double num2 = flag ? this.YAxis.RenderedRect.Left - this.XAxis.Area.SeriesClipRect.Left : this.YAxis.RenderedRect.Left;
        double num3 = flag ? this.XAxis.RenderedRect.Top - this.YAxis.Area.SeriesClipRect.Top : this.XAxis.RenderedRect.Top;
        return new Vector3D(num2 + this.YAxis.RenderedRect.Width * this.YAxis.ValueToCoefficientCalc(y), num3 + this.XAxis.RenderedRect.Height * (1.0 - this.XAxis.ValueToCoefficientCalc(x)), num1 + Math.Round(this.ZAxis.RenderedRect.Width * this.ZAxis.ValueToCoefficientCalc(z)));
      }
      double num4 = flag ? this.XAxis.RenderedRect.Left - this.XAxis.Area.SeriesClipRect.Left : this.XAxis.RenderedRect.Left;
      double num5 = flag ? this.YAxis.RenderedRect.Top - this.YAxis.Area.SeriesClipRect.Top : this.YAxis.RenderedRect.Top;
      return new Vector3D(num4 + Math.Round(this.XAxis.RenderedRect.Width * this.XAxis.ValueToCoefficientCalc(x)), num5 + Math.Round(this.YAxis.RenderedRect.Height * (1.0 - this.YAxis.ValueToCoefficientCalc(y))), num1 + Math.Round(this.ZAxis.RenderedRect.Width * this.ZAxis.ValueToCoefficientCalc(z)));
    }

    public Point TransformToVisible(double x, double y, bool isXInversed, bool isYInversed)
    {
      x = x = !this.x_IsLogarithmic || x <= 0.0 ? x : Math.Log(x, this.xlogarithmicBase);
      y = !this.y_IsLogarithmic || y <= 0.0 ? y : Math.Log(y, this.ylogarithmicBase);
      if (this.m_IsRoated)
      {
        double num1 = this.YAxis.RenderedRect.Left - this.XAxis.Area.SeriesClipRect.Left;
        double num2 = this.XAxis.RenderedRect.Top - this.YAxis.Area.SeriesClipRect.Top;
        return new Point(num1 + this.YAxis.RenderedRect.Width * this.YAxis.ValueToCoefficient(y, isYInversed), num2 + this.XAxis.RenderedRect.Height * (1.0 - this.XAxis.ValueToCoefficient(x, isXInversed)));
      }
      double num3 = this.XAxis.RenderedRect.Left - this.XAxis.Area.SeriesClipRect.Left;
      double num4 = this.YAxis.RenderedRect.Top - this.YAxis.Area.SeriesClipRect.Top;
      return new Point(num3 + Math.Round(this.XAxis.RenderedRect.Width * this.XAxis.ValueToCoefficient(x, isXInversed)), num4 + Math.Round(this.YAxis.RenderedRect.Height * (1.0 - this.YAxis.ValueToCoefficient(y, isYInversed))));
    }
  }

  public class ChartPolarTransformer : IChartTransformer
  {
    private double xlogarithmicBase;
    private double ylogarithmicBase;
    private bool y_IsLogarithmic;
    private bool x_IsLogarithmic;
    private Rect m_viewport = Rect.Empty;
    private ChartAxis m_xAxis;
    private ChartAxis m_yAxis;
    private Point m_center = new Point();
    private double m_radius;

    public Rect Viewport => this.m_viewport;

    public ChartPolarTransformer(Rect viewport, ChartAxis xAxis, ChartAxis yAxis)
    {
      this.m_viewport = viewport;
      this.m_xAxis = xAxis;
      this.m_yAxis = yAxis;
      this.m_center = ChartLayoutUtils.GetCenter(this.m_viewport);
      this.m_radius = 0.5 * Math.Min(this.m_viewport.Width, this.m_viewport.Height);
    }

    public ChartPolarTransformer(Rect viewport, ChartSeriesBase series)
    {
      this.m_viewport = viewport;
      this.m_xAxis = series.ActualXAxis;
      this.m_yAxis = series.ActualYAxis;
      this.m_center = ChartLayoutUtils.GetCenter(this.m_viewport);
      this.m_radius = 0.5 * Math.Min(this.m_viewport.Width, this.m_viewport.Height);
      this.x_IsLogarithmic = series.ActualXAxis is LogarithmicAxis;
      this.y_IsLogarithmic = series.ActualYAxis is LogarithmicAxis;
      if (this.x_IsLogarithmic)
        this.xlogarithmicBase = (series.ActualXAxis as LogarithmicAxis).LogarithmicBase;
      if (!this.y_IsLogarithmic)
        return;
      this.ylogarithmicBase = (series.ActualYAxis as LogarithmicAxis).LogarithmicBase;
    }

    public Point TransformToVisible(double x, double y)
    {
      x = x = !this.x_IsLogarithmic || x <= 0.0 ? x : Math.Log(x, this.xlogarithmicBase);
      y = !this.y_IsLogarithmic || y <= 0.0 ? y : Math.Log(y, this.ylogarithmicBase);
      double num = this.m_radius * this.m_yAxis.ValueToCoefficient(y);
      Point vector = ChartTransform.ValueToVector(this.m_xAxis, x);
      return new Point(this.m_center.X + num * vector.X, this.m_center.Y + num * vector.Y);
    }

    Size IChartTransformer.Viewport => Size.Empty;
  }

  public class ChartTransform3D : IChartTransformer
  {
    private Matrix3D perspective = Matrix3D.GetIdentity();
    private bool needUpdate = true;
    private Matrix3D centeredMatrix = Matrix3D.Identity;
    private Matrix3D viewMatrix = Matrix3D.Identity;
    private Matrix3D resultMatrix = Matrix3D.Identity;
    internal Size mViewport = Size.Empty;

    public ChartTransform3D(Size viewPort) => this.mViewport = viewPort;

    internal double Rotation { get; set; }

    internal double PerspectiveAngle { get; set; }

    internal double Tilt { get; set; }

    internal double Depth { get; set; }

    public Matrix3D Centered
    {
      get => this.centeredMatrix;
      set
      {
        if (this.centeredMatrix == value)
          return;
        this.centeredMatrix = value;
        this.needUpdate = true;
      }
    }

    public Matrix3D View
    {
      get => this.viewMatrix;
      set
      {
        if (this.viewMatrix == value)
          return;
        this.viewMatrix = value;
        this.needUpdate = true;
      }
    }

    public Matrix3D Result
    {
      get
      {
        if (!this.needUpdate)
          return this.resultMatrix;
        this.resultMatrix = Matrix3D.GetInvertal(this.centeredMatrix) * this.perspective * this.viewMatrix * this.centeredMatrix;
        this.needUpdate = false;
        return this.resultMatrix;
      }
    }

    public Size Viewport => this.mViewport;

    public Point TransformToVisible(double x, double y) => throw new NotImplementedException();

    internal void Transform()
    {
      double width = this.mViewport.Width;
      double height = this.mViewport.Height;
      if (width == 0.0)
        return;
      Vector3D vector3D = new Vector3D(Math.Cos(this.Rotation * 0.017444444444444446) * width, 0.0, Math.Sin(this.Rotation * 0.017444444444444446) * width);
      this.SetCenter(new Vector3D(width / 2.0, height / 2.0, this.Depth / 2.0));
      this.View = Matrix3D.Transform(0.0, 0.0, this.Depth);
      this.View *= Matrix3D.Turn(-1.0 * Math.PI / 180.0 * this.Rotation);
      this.View *= Matrix3D.TiltArbitrary(-1.0 * Math.PI / 180.0 * this.Tilt, vector3D);
      this.UpdatePerspective(this.PerspectiveAngle);
      this.needUpdate = true;
    }

    protected double DegreeToRadianConverter(double degree) => degree * Math.PI / 180.0;

    private void UpdatePerspective(double angle)
    {
      double num = (this.Viewport.Width + this.Viewport.Height) * Math.Tan(this.DegreeToRadianConverter((180.0 - Math.Abs(angle % 181.0)) / 2.0)) + this.Depth * 2.0 / 2.0;
      this.perspective[0, 0] = num;
      this.perspective[1, 1] = num;
      this.perspective[2, 3] = 1.0;
      this.perspective[3, 3] = num;
    }

    internal void SetCenter(Vector3D center)
    {
      this.centeredMatrix = Matrix3D.Transform(-center.X, -center.Y, -center.Z);
      this.needUpdate = true;
    }

    public Point ToScreen(Vector3D vector3D)
    {
      vector3D = this.Result * vector3D;
      return new Point(vector3D.X, vector3D.Y);
    }

    public Vector3D ToPlane(Point point, Polygon3D plane)
    {
      Vector3D vector3D1 = new Vector3D(point.X, point.Y, 0.0);
      Vector3D vector3D2 = vector3D1 + new Vector3D(0.0, 0.0, 1.0);
      Vector3D vector3D3 = this.centeredMatrix * vector3D1;
      Vector3D vector3D4 = this.centeredMatrix * vector3D2;
      Vector3D position = Matrix3D.GetInvertal(this.perspective) * vector3D3;
      Vector3D vector3D5 = Matrix3D.GetInvertal(this.perspective) * vector3D4;
      Vector3D point1 = plane.GetPoint(position, vector3D5 - position);
      Vector3D vector3D6 = Matrix3D.GetInvertal(this.viewMatrix) * point1;
      return Matrix3D.GetInvertal(this.centeredMatrix) * vector3D6;
    }
  }
}
