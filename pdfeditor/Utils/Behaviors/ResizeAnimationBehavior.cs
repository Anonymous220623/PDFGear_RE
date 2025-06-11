// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Behaviors.ResizeAnimationBehavior
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace pdfeditor.Utils.Behaviors;

public class ResizeAnimationBehavior : Behavior<FrameworkElement>
{
  public static readonly DependencyProperty KeepAspectRatioProperty = DependencyProperty.Register(nameof (KeepAspectRatio), typeof (bool), typeof (ResizeAnimationBehavior), new PropertyMetadata((object) false));
  public static readonly DependencyProperty HorizontalAlignmentRatioProperty = DependencyProperty.Register(nameof (HorizontalAlignmentRatio), typeof (double), typeof (ResizeAnimationBehavior), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty VerticalAlignmentRatioProperty = DependencyProperty.Register(nameof (VerticalAlignmentRatio), typeof (double), typeof (ResizeAnimationBehavior), new PropertyMetadata((object) 0.0));
  private Storyboard currentSb;

  protected override void OnAttached()
  {
    base.OnAttached();
    this.AssociatedObject.SizeChanged += new SizeChangedEventHandler(this.AssociatedObject_SizeChanged);
  }

  protected override void OnDetaching()
  {
    this.TryStopAnimation();
    base.OnDetaching();
    this.AssociatedObject.SizeChanged -= new SizeChangedEventHandler(this.AssociatedObject_SizeChanged);
  }

  public bool KeepAspectRatio
  {
    get => (bool) this.GetValue(ResizeAnimationBehavior.KeepAspectRatioProperty);
    set => this.SetValue(ResizeAnimationBehavior.KeepAspectRatioProperty, (object) value);
  }

  public double HorizontalAlignmentRatio
  {
    get => (double) this.GetValue(ResizeAnimationBehavior.HorizontalAlignmentRatioProperty);
    set => this.SetValue(ResizeAnimationBehavior.HorizontalAlignmentRatioProperty, (object) value);
  }

  public double VerticalAlignmentRatio
  {
    get => (double) this.GetValue(ResizeAnimationBehavior.VerticalAlignmentRatioProperty);
    set => this.SetValue(ResizeAnimationBehavior.VerticalAlignmentRatioProperty, (object) value);
  }

  private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.TryStopAnimation();
    Matrix startMatrix = this.GetStartMatrix(e.PreviousSize, e.NewSize);
    if (startMatrix.IsIdentity)
      return;
    MatrixTransform matrixTransform = new MatrixTransform(startMatrix);
    Storyboard animation = this.CreateAnimation(TimeSpan.FromSeconds(0.15), startMatrix, matrixTransform);
    animation.Completed += (EventHandler) ((s, a) => this.TryStopAnimation());
    this.AssociatedObject.RenderTransform = (Transform) matrixTransform;
    animation.Begin();
    this.currentSb = animation;
  }

  private void TryStopAnimation()
  {
    if (this.currentSb == null)
      return;
    this.currentSb.Stop();
    this.currentSb = (Storyboard) null;
    this.AssociatedObject.RenderTransform = (Transform) null;
  }

  private Matrix GetStartMatrix(Size oldSize, Size newSize)
  {
    if (newSize.Width == 0.0 || newSize.Height == 0.0)
      return Matrix.Identity;
    double scaleX = oldSize.Width / newSize.Width;
    double scaleY = oldSize.Height / newSize.Height;
    if (this.KeepAspectRatio && scaleX != scaleY)
    {
      if (scaleX == 1.0)
        scaleX = scaleY;
      else if (scaleY == 1.0)
        scaleY = scaleX;
    }
    double offsetX = (newSize.Width - oldSize.Width) * this.HorizontalAlignmentRatio;
    double offsetY = (newSize.Height - oldSize.Height) * this.VerticalAlignmentRatio;
    Matrix identity = Matrix.Identity;
    identity.Scale(scaleX, scaleY);
    identity.Translate(offsetX, offsetY);
    return identity;
  }

  private Storyboard CreateAnimation(
    TimeSpan duration,
    Matrix startValue,
    MatrixTransform matrixTransform)
  {
    if (duration.TotalSeconds <= 0.0)
      return (Storyboard) null;
    ResizeAnimationBehavior.LinearMatrixAnimation linearMatrixAnimation = new ResizeAnimationBehavior.LinearMatrixAnimation();
    linearMatrixAnimation.Duration = new Duration(duration);
    linearMatrixAnimation.From = new Matrix?(startValue);
    linearMatrixAnimation.To = new Matrix?(Matrix.Identity);
    ResizeAnimationBehavior.LinearMatrixAnimation element = linearMatrixAnimation;
    Storyboard.SetTarget((DependencyObject) element, (DependencyObject) matrixTransform);
    Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath((object) MatrixTransform.MatrixProperty));
    element.Freeze();
    Storyboard animation = new Storyboard();
    animation.Children.Add((Timeline) element);
    return animation;
  }

  public class LinearMatrixAnimation : AnimationTimeline
  {
    public static DependencyProperty FromProperty = DependencyProperty.Register(nameof (From), typeof (Matrix?), typeof (ResizeAnimationBehavior.LinearMatrixAnimation), new PropertyMetadata((PropertyChangedCallback) null));
    public static DependencyProperty ToProperty = DependencyProperty.Register(nameof (To), typeof (Matrix?), typeof (ResizeAnimationBehavior.LinearMatrixAnimation), new PropertyMetadata((PropertyChangedCallback) null));

    public Matrix? From
    {
      set
      {
        this.SetValue(ResizeAnimationBehavior.LinearMatrixAnimation.FromProperty, (object) value);
      }
      get
      {
        return new Matrix?((Matrix) this.GetValue(ResizeAnimationBehavior.LinearMatrixAnimation.FromProperty));
      }
    }

    public Matrix? To
    {
      set
      {
        this.SetValue(ResizeAnimationBehavior.LinearMatrixAnimation.ToProperty, (object) value);
      }
      get
      {
        return new Matrix?((Matrix) this.GetValue(ResizeAnimationBehavior.LinearMatrixAnimation.ToProperty));
      }
    }

    public LinearMatrixAnimation()
    {
    }

    public LinearMatrixAnimation(Matrix from, Matrix to, Duration duration)
    {
      this.Duration = duration;
      this.From = new Matrix?(from);
      this.To = new Matrix?(to);
    }

    public override object GetCurrentValue(
      object defaultOriginValue,
      object defaultDestinationValue,
      AnimationClock animationClock)
    {
      if (!animationClock.CurrentProgress.HasValue)
        return (object) null;
      double num = animationClock.CurrentProgress.Value;
      Matrix? nullable = this.From;
      Matrix matrix1 = nullable ?? (Matrix) defaultOriginValue;
      nullable = this.To;
      if (!nullable.HasValue)
        return (object) Matrix.Identity;
      nullable = this.To;
      Matrix matrix2 = nullable.Value;
      return (object) new Matrix((matrix2.M11 - matrix1.M11) * num + matrix1.M11, 0.0, 0.0, (matrix2.M22 - matrix1.M22) * num + matrix1.M22, (matrix2.OffsetX - matrix1.OffsetX) * num + matrix1.OffsetX, (matrix2.OffsetY - matrix1.OffsetY) * num + matrix1.OffsetY);
    }

    protected override Freezable CreateInstanceCore()
    {
      return (Freezable) new ResizeAnimationBehavior.LinearMatrixAnimation();
    }

    public override Type TargetPropertyType => typeof (Matrix);
  }
}
