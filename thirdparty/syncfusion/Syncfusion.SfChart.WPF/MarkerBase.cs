// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.MarkerBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class MarkerBase : SparklineBase
{
  public static readonly DependencyProperty MarkerTemplateSelectorProperty = DependencyProperty.Register(nameof (MarkerTemplateSelector), typeof (TemplateSelector), typeof (MarkerBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty XBindingPathProperty = DependencyProperty.Register(nameof (XBindingPath), typeof (string), typeof (MarkerBase), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty AxisStyleProperty = DependencyProperty.Register(nameof (AxisStyle), typeof (Style), typeof (MarkerBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ShowAxisProperty = DependencyProperty.Register(nameof (ShowAxis), typeof (bool), typeof (MarkerBase), new PropertyMetadata((object) false, new PropertyChangedCallback(MarkerBase.OnShowAxisChanged)));
  public static readonly DependencyProperty AxisOriginProperty = DependencyProperty.Register(nameof (AxisOrigin), typeof (double), typeof (MarkerBase), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(MarkerBase.OnAxisOriginChanged)));
  public static readonly DependencyProperty TrackBallStyleProperty = DependencyProperty.Register(nameof (TrackBallStyle), typeof (Style), typeof (SparklineBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(nameof (LineStyle), typeof (Style), typeof (SparklineBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ShowTrackBallProperty = DependencyProperty.Register(nameof (ShowTrackBall), typeof (bool), typeof (MarkerBase), new PropertyMetadata((object) false, new PropertyChangedCallback(MarkerBase.OnShowTrackBallChanged)));
  public static readonly DependencyProperty MarkerVisibilityProperty = DependencyProperty.Register(nameof (MarkerVisibility), typeof (Visibility), typeof (MarkerBase), new PropertyMetadata((object) Visibility.Collapsed, new PropertyChangedCallback(MarkerBase.OnMarkerVisibilityChanged)));
  private Line trackLine;
  private Line axisLine;
  private Ellipse trackBall;
  private SparklinePointsInfo sparklineInfo;
  private Canvas trackBallCanvas;

  public MarkerBase() => this.DefaultStyleKey = (object) typeof (MarkerBase);

  public string XBindingPath
  {
    get => (string) this.GetValue(MarkerBase.XBindingPathProperty);
    set => this.SetValue(MarkerBase.XBindingPathProperty, (object) value);
  }

  public Style AxisStyle
  {
    get => (Style) this.GetValue(MarkerBase.AxisStyleProperty);
    set => this.SetValue(MarkerBase.AxisStyleProperty, (object) value);
  }

  public bool ShowAxis
  {
    get => (bool) this.GetValue(MarkerBase.ShowAxisProperty);
    set => this.SetValue(MarkerBase.ShowAxisProperty, (object) value);
  }

  public double AxisOrigin
  {
    get => (double) this.GetValue(MarkerBase.AxisOriginProperty);
    set => this.SetValue(MarkerBase.AxisOriginProperty, (object) value);
  }

  public Style TrackBallStyle
  {
    get => (Style) this.GetValue(MarkerBase.TrackBallStyleProperty);
    set => this.SetValue(MarkerBase.TrackBallStyleProperty, (object) value);
  }

  public Style LineStyle
  {
    get => (Style) this.GetValue(MarkerBase.LineStyleProperty);
    set => this.SetValue(MarkerBase.LineStyleProperty, (object) value);
  }

  public bool ShowTrackBall
  {
    get => (bool) this.GetValue(MarkerBase.ShowTrackBallProperty);
    set => this.SetValue(MarkerBase.ShowTrackBallProperty, (object) value);
  }

  public Visibility MarkerVisibility
  {
    get => (Visibility) this.GetValue(MarkerBase.MarkerVisibilityProperty);
    set => this.SetValue(MarkerBase.MarkerVisibilityProperty, (object) value);
  }

  public TemplateSelector MarkerTemplateSelector
  {
    get => (TemplateSelector) this.GetValue(MarkerBase.MarkerTemplateSelectorProperty);
    set => this.SetValue(MarkerBase.MarkerTemplateSelectorProperty, (object) value);
  }

  internal Canvas MarkerPresenter { get; set; }

  public override void Reset()
  {
    if (this.MarkerPresenter != null)
      this.MarkerPresenter.Children.Clear();
    base.Reset();
  }

  public void AddMarker(double screenPointX, double screenPointY, double x, double y)
  {
    if (this.MarkerTemplateSelector != null)
    {
      DataTemplate dataTemplate = this.MarkerTemplateSelector.SelectTemplate(x, y);
      if (dataTemplate == null)
      {
        Ellipse ellipse = new Ellipse();
        (this.MarkerTemplateSelector as Syncfusion.UI.Xaml.Charts.MarkerTemplateSelector).BindVisual(x, y, (Shape) ellipse);
        MarkerBase.PlaceMarker((FrameworkElement) ellipse, new Point(screenPointX, screenPointY));
        this.MarkerPresenter.Children.Add((UIElement) ellipse);
      }
      else
      {
        ContentPresenter element = new ContentPresenter();
        element.ContentTemplate = dataTemplate;
        element.Measure(new Size(this.availableWidth, this.availableHeight));
        element.SetValue(Canvas.LeftProperty, (object) (screenPointX - element.DesiredSize.Width / 2.0));
        element.SetValue(Canvas.TopProperty, (object) (screenPointY - element.DesiredSize.Height / 2.0));
        this.MarkerPresenter.Children.Add((UIElement) element);
      }
    }
    else
    {
      Ellipse ellipse = new Ellipse();
      ellipse.SetBinding(Shape.FillProperty, (BindingBase) new Binding()
      {
        Path = new PropertyPath("Interior", new object[0]),
        Source = (object) this
      });
      ellipse.Height = 10.0;
      ellipse.Width = 10.0;
      MarkerBase.PlaceMarker((FrameworkElement) ellipse, new Point(screenPointX, screenPointY));
      this.MarkerPresenter.Children.Add((UIElement) ellipse);
    }
  }

  internal virtual void ClearUnUsedMarkers(int dataCount)
  {
    if (this.MarkerVisibility == Visibility.Visible)
    {
      int count = this.MarkerPresenter.Children.Count;
      if (count <= dataCount)
        return;
      for (int index = dataCount; index < count; ++index)
        this.MarkerPresenter.Children.RemoveAt(dataCount);
    }
    else
      this.MarkerPresenter.Children.Clear();
  }

  internal override void SetBinding(Shape element)
  {
    base.SetBinding(element);
    element.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Interior", new object[0])
    });
  }

  protected override void SetIndividualPoints(int index, object obj, bool replace, string xPath)
  {
    base.SetIndividualPoints(index, obj, replace, this.XBindingPath);
  }

  protected override void GeneratePoints(string xPath) => base.GeneratePoints(this.XBindingPath);

  protected void RemoveAxis()
  {
    if (this.RootPanel.Children.Contains((UIElement) this.axisLine))
      this.RootPanel.Children.Remove((UIElement) this.axisLine);
    this.axisLine = (Line) null;
  }

  protected virtual void UpdateHorizontalAxis()
  {
    if (this.RootPanel == null || !this.ShowAxis)
      return;
    if (this.axisLine == null)
    {
      this.axisLine = new Line();
      this.StyleBinding((FrameworkElement) this.axisLine, this.AxisStyle, "AxisStyle");
      this.RootPanel.Children.Add((UIElement) this.axisLine);
    }
    Point visible = this.TransformToVisible(0.0, this.AxisOrigin);
    this.axisLine.X1 = 0.0;
    this.axisLine.X2 = this.availableWidth;
    this.axisLine.Y1 = visible.Y;
    this.axisLine.Y2 = visible.Y;
  }

  protected override void AnimateSegments(UIElementCollection elements)
  {
    Rect rect = new Rect(0.0, 0.0, this.availableWidth, this.availableHeight);
    RectangleGeometry rectangleGeometry = new RectangleGeometry();
    this.RootPanel.Clip = (Geometry) rectangleGeometry;
    RectAnimationUsingKeyFrames animation = new RectAnimationUsingKeyFrames();
    SplineRectKeyFrame keyFrame1 = new SplineRectKeyFrame(new Rect(0.0, rect.Y, 0.0, rect.Height), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0)));
    animation.KeyFrames.Add((RectKeyFrame) keyFrame1);
    SplineRectKeyFrame keyFrame2 = new SplineRectKeyFrame(new Rect(0.0, rect.Y, rect.Width, rect.Height), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.0)));
    animation.KeyFrames.Add((RectKeyFrame) keyFrame2);
    keyFrame2.KeySpline = new KeySpline(0.65, 0.84, 0.67, 0.95);
    rectangleGeometry.BeginAnimation(RectangleGeometry.RectProperty, (AnimationTimeline) animation);
  }

  protected override void RenderSegments()
  {
    if (this.ShowAxis)
      this.UpdateHorizontalAxis();
    if (this.MarkerTemplateSelector != null)
      this.MarkerTemplateSelector.SetData((SparklineBase) this, this.DataCount);
    if (this.MarkerPresenter != null)
      this.ClearUnUsedMarkers(this.xValues.Count - (this.EmptyPointIndexes.Count - 1));
    this.ClearUnUsedSegments(this.EmptyPointIndexes.Count);
  }

  protected void AddMarker(Point pointToScreen, double x, double y, int index)
  {
    if (this.MarkerPresenter == null)
      return;
    if (this.MarkerPresenter.Children.Count > index)
      this.UpdateMarker(pointToScreen.X, pointToScreen.Y, x, y, (FrameworkElement) this.MarkerPresenter.Children[index]);
    else
      this.AddMarker(pointToScreen.X, pointToScreen.Y, x, y);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (this.MarkerVisibility != Visibility.Visible || this.MarkerPresenter != null)
      return;
    this.MarkerPresenter = new Canvas();
    this.RootPanel.Children.Add((UIElement) this.MarkerPresenter);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    if (!this.ShowTrackBall)
      return;
    if (this.trackLine == null)
      this.CreateTrackBall();
    Point position = e.GetPosition((IInputElement) this);
    this.sparklineInfo = this.FindPoints(position.X, position.Y);
    double x = this.sparklineInfo.Coordinate.X;
    double y = this.sparklineInfo.Coordinate.Y;
    if (double.IsNaN(x) || double.IsNaN(y))
      return;
    this.trackLine.X1 = x;
    this.trackLine.X2 = x;
    this.trackLine.Y2 = this.availableHeight;
    this.trackBall.SetValue(Canvas.LeftProperty, (object) (x - this.trackBall.Width / 2.0));
    this.trackBall.SetValue(Canvas.TopProperty, (object) (y - this.trackBall.Height / 2.0));
  }

  private static void OnShowAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    MarkerBase markerBase = d as MarkerBase;
    if (!(bool) e.NewValue)
      markerBase.RemoveAxis();
    else
      markerBase.UpdateHorizontalAxis();
  }

  private static void OnAxisOriginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as MarkerBase).UpdateHorizontalAxis();
  }

  private static void OnShowTrackBallChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((bool) e.NewValue)
      return;
    (d as MarkerBase).ResetTrackBallCanvas();
  }

  private static void OnMarkerVisibilityChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as MarkerBase).OnMarkerVisibilityChanged(e);
  }

  private static void PlaceMarker(FrameworkElement marker, Point screenPoint)
  {
    marker.SetValue(Canvas.LeftProperty, (object) (screenPoint.X - marker.Width / 2.0));
    marker.SetValue(Canvas.TopProperty, (object) (screenPoint.Y - marker.Height / 2.0));
  }

  private void UpdateMarker(
    double screenPointX,
    double screenPointY,
    double x,
    double y,
    FrameworkElement element)
  {
    if (this.MarkerTemplateSelector != null)
    {
      if (element is ContentPresenter contentPresenter)
      {
        contentPresenter.ContentTemplate = this.MarkerTemplateSelector.SelectTemplate(x, y);
        contentPresenter.Measure(new Size(this.availableWidth, this.availableHeight));
        contentPresenter.SetValue(Canvas.LeftProperty, (object) (screenPointX - contentPresenter.DesiredSize.Width / 2.0));
        contentPresenter.SetValue(Canvas.TopProperty, (object) (screenPointY - contentPresenter.DesiredSize.Height / 2.0));
      }
      else
      {
        Shape marker = element as Shape;
        (this.MarkerTemplateSelector as Syncfusion.UI.Xaml.Charts.MarkerTemplateSelector).BindVisual(x, y, marker);
        MarkerBase.PlaceMarker((FrameworkElement) marker, new Point(screenPointX, screenPointY));
      }
    }
    else
      MarkerBase.PlaceMarker(element, new Point(screenPointX, screenPointY));
  }

  private void CreateTrackBall()
  {
    this.trackBallCanvas = new Canvas();
    this.trackLine = new Line();
    this.trackBall = new Ellipse();
    this.StyleBinding((FrameworkElement) this.trackBall, this.TrackBallStyle, "TrackBallStyle");
    this.StyleBinding((FrameworkElement) this.trackLine, this.TrackBallStyle, "LineStyle");
    this.trackBallCanvas.Children.Add((UIElement) this.trackLine);
    this.trackBallCanvas.Children.Add((UIElement) this.trackBall);
    this.RootPanel.Children.Add((UIElement) this.trackBallCanvas);
  }

  private void OnMarkerVisibilityChanged(DependencyPropertyChangedEventArgs e)
  {
    if ((Visibility) e.NewValue == Visibility.Visible)
    {
      if (this.MarkerPresenter == null && this.RootPanel != null)
      {
        this.MarkerPresenter = new Canvas();
        this.RootPanel.Children.Add((UIElement) this.MarkerPresenter);
      }
      this.UpdateArea();
    }
    else
    {
      this.RootPanel.Children.Remove((UIElement) this.MarkerPresenter);
      this.MarkerPresenter.Children.Clear();
      this.MarkerPresenter = (Canvas) null;
    }
  }

  private void ResetTrackBallCanvas()
  {
    if (this.trackBallCanvas == null)
      return;
    this.trackBallCanvas.Children.Clear();
    this.trackLine = (Line) null;
    this.trackBall = (Ellipse) null;
    this.RootPanel.Children.Remove((UIElement) this.trackBallCanvas);
  }
}
