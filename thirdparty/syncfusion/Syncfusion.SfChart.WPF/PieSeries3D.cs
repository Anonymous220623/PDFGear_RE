// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.PieSeries3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class PieSeries3D : CircularSeriesBase3D
{
  public static readonly DependencyProperty ExplodeOnMouseClickProperty = DependencyProperty.Register(nameof (ExplodeOnMouseClick), typeof (bool), typeof (PieSeries3D), new PropertyMetadata((object) false));
  protected double actualWidth;
  protected double actualHeight;
  private bool allowExplode;
  private ChartSegment mouseUnderSegment;
  private int animateCount;

  public PieSeries3D()
  {
    this.InnerRadius = 0.0;
    this.Radius = 0.0;
    this.DefaultStyleKey = (object) typeof (PieSeries3D);
  }

  public bool ExplodeOnMouseClick
  {
    get => (bool) this.GetValue(PieSeries3D.ExplodeOnMouseClickProperty);
    set => this.SetValue(PieSeries3D.ExplodeOnMouseClickProperty, (object) value);
  }

  protected internal double InnerRadius { get; set; }

  protected internal double Radius { get; set; }

  public override void CreateSegments()
  {
    this.Area.ActualDepth = this.Area.Depth;
    this.InnerRadius = 0.0;
    if (this.Area != null)
      this.CreatePoints();
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(this.GetXValues(), false);
  }

  internal override void UpdateOnSeriesBoundChanged(Size size)
  {
    if (this.AdornmentsInfo != null)
    {
      this.AdornmentsInfo.UpdateElements();
      this.AdornmentsInfo.Measure(size, (Panel) null);
    }
    if (this is ISupportAxes supportAxes && (supportAxes == null || this.ActualXAxis == null || this.ActualYAxis == null))
      return;
    this.CreateTransformer(size, true);
    List<Polygon3D>[] polygon3DListArray = new List<Polygon3D>[4]
    {
      new List<Polygon3D>(),
      new List<Polygon3D>(),
      new List<Polygon3D>(),
      new List<Polygon3D>()
    };
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      if (segment is PieSegment3D pieSegment3D)
      {
        pieSegment3D.CreateSegmentVisual(size);
        Polygon3D[][] sector = pieSegment3D.CreateSector();
        if (sector != null)
        {
          for (int index1 = 0; index1 < sector.Length; ++index1)
          {
            if (sector[index1] != null)
            {
              for (int index2 = 0; index2 < sector[index1].Length; ++index2)
                polygon3DListArray[index1].Add(sector[index1][index2]);
            }
          }
        }
      }
    }
    for (int index = 0; index < polygon3DListArray.Length; ++index)
    {
      foreach (Polygon3D polygon in polygon3DListArray[index])
        this.Area.Graphics3D.AddVisual(polygon);
    }
  }

  internal override void UpdateEmptyPointSegments(List<double> xValues, bool isSidebySideSeries)
  {
    if (this.EmptyPointIndexes == null)
      return;
    foreach (int index in this.EmptyPointIndexes[0])
    {
      this.Segments[index].IsEmptySegmentInterior = true;
      if (this.Adornments.Count > 0)
        this.Adornments[index].IsEmptySegmentInterior = true;
    }
  }

  internal override bool GetAnimationIsActive()
  {
    return this.AnimationStoryboard != null && this.AnimationStoryboard.GetCurrentState() == ClockState.Active;
  }

  internal override void Animate()
  {
    if (this.AnimationStoryboard != null && this.animateCount <= this.Segments.Count)
      this.AnimationStoryboard = new Storyboard();
    else if (this.AnimationStoryboard != null)
    {
      this.AnimationStoryboard.Stop();
      if (!this.canAnimate)
      {
        using (IEnumerator<ChartSegment> enumerator = this.Segments.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            PieSegment3D current = enumerator.Current as PieSegment3D;
            current.ActualStartValue = this.StartAngle < this.EndAngle ? current.StartValue : current.EndValue;
            double num = this.StartAngle < this.EndAngle ? current.EndValue - current.StartValue : current.StartValue - current.EndValue;
            current.ActualEndValue = num == 0.0 ? 0.1 : num;
          }
          return;
        }
      }
    }
    else
      this.AnimationStoryboard = new Storyboard();
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      ++this.animateCount;
      if (!(segment is EmptyPointSegment))
      {
        PieSegment3D pieSegment3D = segment as PieSegment3D;
        double startValue = pieSegment3D.StartValue;
        double endValue = pieSegment3D.EndValue;
        DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
        SplineDoubleKeyFrame splineDoubleKeyFrame1 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame1.Value = this.StartAngle;
        SplineDoubleKeyFrame keyFrame1 = splineDoubleKeyFrame1;
        keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
        element1.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
        SplineDoubleKeyFrame splineDoubleKeyFrame2 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame2.Value = this.StartAngle < this.EndAngle ? startValue : endValue;
        SplineDoubleKeyFrame keyFrame2 = splineDoubleKeyFrame2;
        keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(this.AnimationDuration);
        KeySpline keySpline1 = new KeySpline()
        {
          ControlPoint1 = new Point(0.64, 0.84),
          ControlPoint2 = new Point(0.67, 0.95)
        };
        keyFrame2.KeySpline = keySpline1;
        element1.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
        Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) PieSegment3D.ActualStartValueProperty));
        Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) pieSegment3D);
        this.AnimationStoryboard.Children.Add((Timeline) element1);
        DoubleAnimationUsingKeyFrames element2 = new DoubleAnimationUsingKeyFrames();
        SplineDoubleKeyFrame splineDoubleKeyFrame3 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame3.Value = 0.0;
        SplineDoubleKeyFrame keyFrame3 = splineDoubleKeyFrame3;
        keyFrame3.KeyTime = keyFrame3.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
        element2.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
        double num = this.StartAngle < this.EndAngle ? endValue - startValue : startValue - endValue;
        SplineDoubleKeyFrame splineDoubleKeyFrame4 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame4.Value = num == 0.0 ? 0.1 : num;
        SplineDoubleKeyFrame keyFrame4 = splineDoubleKeyFrame4;
        keyFrame4.KeyTime = keyFrame4.KeyTime.GetKeyTime(this.AnimationDuration);
        KeySpline keySpline2 = new KeySpline()
        {
          ControlPoint1 = new Point(0.64, 0.84),
          ControlPoint2 = new Point(0.67, 0.95)
        };
        keyFrame4.KeySpline = keySpline2;
        element2.KeyFrames.Add((DoubleKeyFrame) keyFrame4);
        Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath((object) PieSegment3D.ActualEndValueProperty));
        Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) pieSegment3D);
        this.AnimationStoryboard.Children.Add((Timeline) element2);
      }
    }
    this.AnimationStoryboard.Begin();
  }

  protected internal override IChartTransformer CreateTransformer(Size size, bool create)
  {
    if (create || this.ChartTransformer == null)
      this.ChartTransformer = ChartTransform.CreateSimple(size);
    return this.ChartTransformer;
  }

  protected internal override void OnSeriesMouseMove(object source, Point pos)
  {
    this.allowExplode = false;
    base.OnSeriesMouseMove(source, pos);
  }

  protected internal override void OnSeriesMouseUp(object source, Point position)
  {
    ChartSegment tag = source is FrameworkElement frameworkElement ? frameworkElement.Tag as ChartSegment : (ChartSegment) null;
    int num = -1;
    if (this.ExplodeOnMouseClick && this.mouseUnderSegment == tag && this.allowExplode)
    {
      if (tag != null)
        num = (tag as PieSegment3D).Index;
      else if (this.Adornments.Count > 0)
        num = ChartExtensionUtils.GetAdornmentIndex(source);
      int explodeIndex = this.ExplodeIndex;
      if (num != explodeIndex)
        this.ExplodeIndex = num;
      else if (this.ExplodeIndex >= 0)
        this.ExplodeIndex = -1;
      this.allowExplode = false;
    }
    base.OnSeriesMouseUp(source, position);
  }

  protected internal override void OnSeriesMouseDown(object source, Point position)
  {
    this.allowExplode = true;
    this.mouseUnderSegment = source is FrameworkElement frameworkElement ? frameworkElement.Tag as ChartSegment : (ChartSegment) null;
    base.OnSeriesMouseDown(source, position);
  }

  protected static ChartAdornment CreateAdornment(
    ChartSeries3D series,
    double xVal,
    double yVal,
    double angle,
    double radius,
    double startDepth)
  {
    ChartPieAdornment3D adornment = new ChartPieAdornment3D(startDepth, xVal, yVal, angle, radius, series);
    adornment.XPos = adornment.XData = xVal;
    adornment.YPos = adornment.YData = yVal;
    adornment.Radius = radius;
    adornment.Angle = angle;
    adornment.Series = (ChartSeriesBase) series;
    adornment.StartDepth = startDepth;
    return (ChartAdornment) adornment;
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new PieSeries3D()
    {
      ExplodeOnMouseClick = this.ExplodeOnMouseClick
    });
  }

  protected virtual void CreatePoints()
  {
    int circularSeriesCount = this.GetCircularSeriesCount();
    int pieSeriesIndex = this.GetPieSeriesIndex();
    this.Segments.Clear();
    this.Adornments.Clear();
    IList<double> source = this.ToggledLegendIndex.Count <= 0 ? this.YValues : (IList<double>) this.GetYValues();
    if (this.Area.RootPanelDesiredSize.HasValue)
    {
      this.actualWidth = this.Area.RootPanelDesiredSize.Value.Width;
      this.actualHeight = this.Area.RootPanelDesiredSize.Value.Height;
    }
    double num1 = source.Select<double, double>((Func<double, double>) (item => Math.Abs(double.IsNaN(item) ? 0.0 : item))).Sum();
    int count = source.Count;
    if (this.InnerRadius == 0.0)
    {
      double num2 = Math.Min(this.actualWidth, this.actualHeight) / 2.0 / (double) circularSeriesCount;
      this.Radius = num2 * (double) (pieSeriesIndex + 1) - num2 * (1.0 - this.InternalCircleCoefficient);
      this.InnerRadius = num2 * (double) pieSeriesIndex;
    }
    double depth = this.Area.Depth;
    if (this.ExplodeIndex >= 0 && this.ExplodeIndex < count || this.ExplodeAll)
      this.Radius -= this.ExplodeRadius;
    double startAngle = this.StartAngle;
    double num3 = this.EndAngle - startAngle;
    if (Math.Abs(num3) > 360.0)
      num3 %= 360.0;
    int num4 = 0;
    for (int index = 0; index < count; ++index)
    {
      if (!double.IsNaN(this.YValues[index]))
      {
        double y = Math.Abs(double.IsNaN(source[index]) ? 0.0 : source[index]);
        double num5 = num1 == 0.0 ? 0.0 : Math.Abs(y) * (num3 / num1);
        Rect rect = new Rect(0.0, 0.0, this.actualWidth, this.actualHeight);
        if (this.ExplodeIndex == index || this.ExplodeAll)
        {
          Point point = new Point(Math.Cos(Math.PI * (2.0 * startAngle + num5) / 360.0), Math.Sin(Math.PI * (2.0 * startAngle + num5) / 360.0));
          rect.Offset(0.0099999997764825821 * this.Radius * point.X * this.ExplodeRadius, 0.0099999997764825821 * this.Radius * point.Y * this.ExplodeRadius);
        }
        if (circularSeriesCount == 1)
        {
          this.Center = this.GetActualCenter(new Point(rect.X + rect.Width / 2.0, rect.Y + rect.Height / 2.0), this.Radius);
        }
        else
        {
          this.Center = new Point(this.actualWidth / 2.0, this.actualHeight / 2.0);
          if (this.ExplodeAll || this.ExplodeIndex == index)
            this.Center = new Point(rect.X + this.Center.X, rect.Y + this.Center.Y);
        }
        Vector3D center = new Vector3D(this.Center.X, this.Center.Y, 0.0);
        if (num4 < this.Segments.Count)
        {
          this.Segments[num4].SetData(startAngle, startAngle + num5, depth, this.Radius, y, center.X, center.Y, center.Z, this.InnerRadius);
          if (this.ToggledLegendIndex.Contains(index))
            this.Segments[num4].IsSegmentVisible = false;
          else
            this.Segments[num4].IsSegmentVisible = true;
        }
        else
          this.Segments.Add((ChartSegment) new PieSegment3D((ChartSeries3D) this, center, startAngle, startAngle + num5, depth, this.Radius, index, y, this.InnerRadius));
        if (this.AdornmentsInfo != null)
          this.AddPieAdornments((double) num4, source[index], startAngle, startAngle + num5, index, this.Radius, this.Area.IsChartRotated() ? this.Area.Depth + 5.0 : 0.0);
        ++num4;
        startAngle += num5;
      }
    }
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    if (oldValue != null)
      this.animateCount = 0;
    base.OnDataSourceChanged(oldValue, newValue);
  }

  private void AddPieAdornments(
    double x,
    double y,
    double startAngle,
    double endAngle,
    int index,
    double radius,
    double startDepth)
  {
    startAngle = CircularSeriesBase3D.DegreeToRadianConverter(startAngle);
    endAngle = CircularSeriesBase3D.DegreeToRadianConverter(endAngle);
    double angle = (startAngle + endAngle) / 2.0;
    this.Adornments.Add(PieSeries3D.CreateAdornment((ChartSeries3D) this, x, y, angle, radius, startDepth));
    this.Adornments[(int) x].Item = this.ActualData[index];
  }
}
