// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CarouselPanel
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.Windows.Shared;

[CLSCompliant(false)]
public class CarouselPanel : Canvas
{
  private const double C_DEFAULT_ROTATION_SPEED = 200.0;
  private const double C_MINIMUM_ROTATION_SPEED = 1.0;
  private const double C_MAXIMUM_ROTATION_SPEED = 1000.0;
  private const double C_DEFAULT_FADE = 0.5;
  private const double C_MINIMUM_FADE = 0.0;
  private const double C_MAXIMUM_FADE = 1.0;
  private const double C_DEFAULT_SCALE = 0.5;
  private const double C_MINIMUM_SCALE = 0.0;
  private const double C_MAXIMUM_SCALE = 1.0;
  private const double INTERNAL_SCALE_COEFFICIENT = 0.6;
  private double rotationDiff;
  protected double X_SCALE;
  protected double Y_SCALE;
  protected double _targetRotation;
  protected internal DispatcherTimer _timer = new DispatcherTimer();
  internal int currentIndex;
  internal double _rotationToGo;
  internal double _currentRotation;
  public static readonly DependencyProperty ScaleFractionProperty = DependencyProperty.Register(nameof (ScaleFraction), typeof (double), typeof (CarouselPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsArrange, new PropertyChangedCallback(CarouselPanel.OnScaleFractionChanged), new CoerceValueCallback(CarouselPanel.CoerceScaleFractions)));
  public static readonly DependencyProperty OpacityFractionProperty = DependencyProperty.Register(nameof (OpacityFraction), typeof (double), typeof (CarouselPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsArrange, new PropertyChangedCallback(CarouselPanel.OnOpacityFractionChanged), new CoerceValueCallback(CarouselPanel.CoerceOpacityFractions)));
  internal static readonly DependencyProperty SkewAngleXFractionProperty = DependencyProperty.Register(nameof (SkewAngleXFraction), typeof (double), typeof (CarouselPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsArrange));
  internal static readonly DependencyProperty SkewAngleYFractionProperty = DependencyProperty.Register(nameof (SkewAngleYFraction), typeof (double), typeof (CarouselPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsArrange));
  public static readonly DependencyProperty RotationSpeedProperty = DependencyProperty.Register(nameof (RotationSpeed), typeof (double), typeof (CarouselPanel), (PropertyMetadata) new UIPropertyMetadata((object) 200.0, new PropertyChangedCallback(CarouselPanel.OnRotationSpeedChanged), new CoerceValueCallback(CarouselPanel.CoerceRotateSpeed)));
  public static readonly DependencyProperty EnableRotationAnimationProperty = DependencyProperty.Register(nameof (EnableRotationAnimation), typeof (bool), typeof (CarouselPanel), (PropertyMetadata) new UIPropertyMetadata((object) true));
  public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register(nameof (RadiusX), typeof (double), typeof (CarouselPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsArrange));
  public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register(nameof (RadiusY), typeof (double), typeof (CarouselPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsArrange));
  internal static readonly DependencyProperty ScalingEnabledProperty = DependencyProperty.Register(nameof (ScalingEnabled), typeof (bool), typeof (CarouselPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsArrange));
  internal static readonly DependencyProperty OpacityEnabledProperty = DependencyProperty.Register(nameof (OpacityEnabled), typeof (bool), typeof (CarouselPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsArrange));
  internal static readonly DependencyProperty SkewAngleXEnabledProperty = DependencyProperty.Register(nameof (SkewAngleXEnabled), typeof (bool), typeof (CarouselPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsArrange));
  internal static readonly DependencyProperty SkewAngleYEnabledProperty = DependencyProperty.Register(nameof (SkewAngleYEnabled), typeof (bool), typeof (CarouselPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsArrange));

  public CarouselPanel()
  {
    this._timer.Tick += new EventHandler(this.TimerTick);
    this._timer.Interval = TimeSpan.FromMilliseconds(10.0);
  }

  public double ScaleFraction
  {
    get => (double) this.GetValue(CarouselPanel.ScaleFractionProperty);
    set => this.SetValue(CarouselPanel.ScaleFractionProperty, (object) value);
  }

  public double OpacityFraction
  {
    get => (double) this.GetValue(CarouselPanel.OpacityFractionProperty);
    set => this.SetValue(CarouselPanel.OpacityFractionProperty, (object) value);
  }

  internal double SkewAngleXFraction
  {
    get => (double) this.GetValue(CarouselPanel.SkewAngleXFractionProperty);
    set => this.SetValue(CarouselPanel.SkewAngleXFractionProperty, (object) value);
  }

  internal double SkewAngleYFraction
  {
    get => (double) this.GetValue(CarouselPanel.SkewAngleYFractionProperty);
    set => this.SetValue(CarouselPanel.SkewAngleYFractionProperty, (object) value);
  }

  public double RotationSpeed
  {
    get => (double) this.GetValue(CarouselPanel.RotationSpeedProperty);
    set => this.SetValue(CarouselPanel.RotationSpeedProperty, (object) value);
  }

  public bool EnableRotationAnimation
  {
    get => (bool) this.GetValue(CarouselPanel.EnableRotationAnimationProperty);
    set => this.SetValue(CarouselPanel.EnableRotationAnimationProperty, (object) value);
  }

  public double RadiusX
  {
    get => (double) this.GetValue(CarouselPanel.RadiusXProperty);
    set => this.SetValue(CarouselPanel.RadiusXProperty, (object) value);
  }

  public double RadiusY
  {
    get => (double) this.GetValue(CarouselPanel.RadiusYProperty);
    set => this.SetValue(CarouselPanel.RadiusYProperty, (object) value);
  }

  internal bool ScalingEnabled
  {
    get => (bool) this.GetValue(CarouselPanel.ScalingEnabledProperty);
    set => this.SetValue(CarouselPanel.ScalingEnabledProperty, (object) value);
  }

  internal bool OpacityEnabled
  {
    get => (bool) this.GetValue(CarouselPanel.OpacityEnabledProperty);
    set => this.SetValue(CarouselPanel.OpacityEnabledProperty, (object) value);
  }

  internal bool SkewAngleXEnabled
  {
    get => (bool) this.GetValue(CarouselPanel.SkewAngleXEnabledProperty);
    set => this.SetValue(CarouselPanel.SkewAngleXEnabledProperty, (object) value);
  }

  internal bool SkewAngleYEnabled
  {
    get => (bool) this.GetValue(CarouselPanel.SkewAngleYEnabledProperty);
    set => this.SetValue(CarouselPanel.SkewAngleYEnabledProperty, (object) value);
  }

  private double RotationAmount => this.EnableRotationAnimation ? 0.01 * this.RotationSpeed : 360.0;

  protected override void OnInitialized(EventArgs e)
  {
    this.Loaded += new RoutedEventHandler(this.CarouselPanel_Loaded);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    for (int index = 0; index < this.Children.Count; ++index)
      (this.Children[index] as FrameworkElement).Measure(availableSize);
    return !double.IsInfinity(availableSize.Width) && !double.IsInfinity(availableSize.Height) ? availableSize : base.MeasureOverride(availableSize);
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    if (this.Children.Count == 0)
      return finalSize;
    double num1 = (90.0 + this._currentRotation) * (Math.PI / 180.0);
    double num2 = 360.0 / (double) this.Children.Count * (Math.PI / 180.0);
    double num3 = this.CoerceRadius(this.RadiusX, finalSize.Width, this.Children[0].DesiredSize.Width);
    double num4 = this.CoerceRadius(this.RadiusY, finalSize.Height, this.Children[0].DesiredSize.Height);
    for (int index = 0; index < this.Children.Count; ++index)
    {
      FrameworkElement child = this.Children[index] as FrameworkElement;
      double num5 = child.DesiredSize.Width / 2.0;
      double num6 = child.DesiredSize.Height / 2.0;
      double degrees = 360.0 * ((double) index / (double) this.Children.Count) + this._currentRotation;
      Point point1 = new Point(num3 * Math.Cos(num1), num4 * Math.Sin(num1));
      Point point2 = new Point(finalSize.Width / 2.0 + point1.X - child.DesiredSize.Width / 2.0, finalSize.Height / 2.0 + point1.Y - child.DesiredSize.Height / 2.0);
      child.Arrange(new Rect(point2.X, point2.Y, child.DesiredSize.Width, child.DesiredSize.Height));
      if (!(child.RenderTransform is ScaleTransform scaleTransform))
      {
        scaleTransform = new ScaleTransform();
        child.RenderTransform = (Transform) scaleTransform;
      }
      scaleTransform.CenterX = num5;
      scaleTransform.CenterY = num6;
      if (this.ScalingEnabled)
        scaleTransform.ScaleX = scaleTransform.ScaleY = this.GetScaledSize(degrees);
      Panel.SetZIndex((UIElement) child, this.GetZValue(degrees));
      this.SetOpacity(child, degrees);
      SkewTransform renderTransform = child.RenderTransform as SkewTransform;
      SkewTransform skewTransform = new SkewTransform();
      if (this.SkewAngleXEnabled)
        skewTransform.AngleX = this.SkewAngleXFraction;
      if (this.SkewAngleYEnabled)
        skewTransform.AngleY = this.SkewAngleYFraction;
      MatrixTransform matrixTransform = new MatrixTransform(scaleTransform.Value * skewTransform.Value);
      child.RenderTransform = (Transform) matrixTransform;
      num1 += num2;
    }
    return finalSize;
  }

  private static object CoerceScaleFractions(DependencyObject sender, object obj)
  {
    return (object) (double.IsNaN((double) obj) ? (sender as CarouselPanel).ScaleFraction : Math.Min(Math.Max((double) obj, 0.0), 1.0));
  }

  private static void OnScaleFractionChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
  }

  private static object CoerceOpacityFractions(DependencyObject sender, object obj)
  {
    return (object) (double.IsNaN((double) obj) ? (sender as CarouselPanel).OpacityFraction : Math.Min(Math.Max((double) obj, 0.0), 1.0));
  }

  private static void OnOpacityFractionChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
  }

  private static object CoerceRotateSpeed(DependencyObject sender, object obj)
  {
    return (object) Math.Min(Math.Max((double) obj, 1.0), 1000.0);
  }

  private static void OnRotationSpeedChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
  }

  internal static double GetFrontElementSpace(
    double currentRotation,
    int targetIndex,
    int totalNumberOfElements)
  {
    double num = -(180.0 - (currentRotation + 360.0 * ((double) targetIndex / (double) totalNumberOfElements)));
    return num > 180.0 ? -(360.0 - num) : num;
  }

  public void SelectElement(FrameworkElement element)
  {
    if (element == null)
      return;
    this.rotationDiff = (double) (360 / this.Children.Count) * (double) (this.currentIndex - this.Children.IndexOf((UIElement) element));
    this.MoveToItem(element, false, false);
  }

  [Obsolete("GetSelecteItem is deprecated, please use GetSelectedItem instead.")]
  public int GetSelecteItem(FrameworkElement element) => this.GetSelectedItem(element);

  public int GetSelectedItem(FrameworkElement element)
  {
    return this.Children.IndexOf((UIElement) element);
  }

  internal void SelectElement(FrameworkElement element, bool forward, bool backward)
  {
    if (element == null)
      return;
    this.rotationDiff = (double) (360 / this.Children.Count) * (double) (this.currentIndex - this.Children.IndexOf((UIElement) element));
    this.MoveToItem(element, forward, backward);
  }

  internal double SetDegrees(double rawDegrees)
  {
    if (rawDegrees > 360.0)
      return rawDegrees - 360.0;
    return rawDegrees < 0.0 ? rawDegrees + 360.0 : rawDegrees;
  }

  internal void Rotate(double numberOfDegrees)
  {
    this._rotationToGo = numberOfDegrees;
    if (this._timer.IsEnabled)
      return;
    this._timer.Start();
  }

  protected void SetOpacity(FrameworkElement element, double degrees)
  {
    double num = this.OpacityEnabled ? this.OpacityFraction : 0.0;
    element.Opacity = 1.0 - num + num * this.GetCoefficient(degrees);
  }

  protected int GetZValue(double degrees) => (int) (360.0 * this.GetCoefficient(degrees));

  protected double GetScaledSize(double degrees)
  {
    return 1.0 - this.ScaleFraction + this.ScaleFraction * this.GetCoefficient(degrees);
  }

  protected virtual void TimerTick(object sender, EventArgs e)
  {
    if (this._rotationToGo < this.RotationAmount && this._rotationToGo > -this.RotationAmount)
    {
      this._rotationToGo = 0.0;
      if (this._currentRotation != this._targetRotation)
      {
        this._currentRotation = this._targetRotation;
      }
      else
      {
        this._timer.Stop();
        return;
      }
    }
    else if (this._rotationToGo < 0.0)
    {
      this._rotationToGo += this.RotationAmount;
      this._currentRotation = this.SetDegrees(this._currentRotation + this.RotationAmount);
    }
    else
    {
      this._rotationToGo -= this.RotationAmount;
      this._currentRotation = this.SetDegrees(this._currentRotation - this.RotationAmount);
    }
    this.InvalidateArrange();
  }

  private void CarouselPanel_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.Children.Count <= 0 || this.currentIndex < 0)
      return;
    this.MoveToItem(this.Children[this.currentIndex] as FrameworkElement, false, false);
  }

  private double CoerceRadius(double radius, double distance, double itemLength)
  {
    return radius < distance / 2.0 - itemLength ? radius : distance / 2.0 - itemLength;
  }

  private double RadsConvert(double degrees) => degrees * Math.PI / 180.0;

  private double GetCoefficient(double degrees)
  {
    return 1.0 - Math.Cos(this.RadsConvert(degrees)) / 2.0 - 0.5;
  }

  private void MoveToItem(FrameworkElement element, bool forward, bool backward)
  {
    double frontElementSpace = CarouselPanel.GetFrontElementSpace(this._currentRotation, this.Children.IndexOf((UIElement) element), this.Children.Count);
    this._targetRotation = this.SetDegrees(this._currentRotation - frontElementSpace);
    this.Rotate(forward ? Math.Abs(frontElementSpace) : (backward ? -Math.Abs(frontElementSpace) : frontElementSpace));
  }
}
