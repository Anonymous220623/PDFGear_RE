// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.TransitionContentControl
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Primitives;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.Windows.Controls;

public class TransitionContentControl : ContentControl
{
  public static readonly DependencyProperty TransitionProperty = DependencyProperty.Register(nameof (Transition), typeof (ContentTransition), typeof (TransitionContentControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty EnableAnimationProperty = DependencyProperty.Register(nameof (EnableAnimation), typeof (bool), typeof (TransitionContentControl), new PropertyMetadata((object) true));
  private ContentPresenter part_Content;
  private ContentControl part_TempContent;
  private Grid part_LayoutRoot;

  public TransitionContentControl()
  {
    this.DefaultStyleKey = (object) typeof (TransitionContentControl);
  }

  public ContentTransition Transition
  {
    get => (ContentTransition) this.GetValue(TransitionContentControl.TransitionProperty);
    set => this.SetValue(TransitionContentControl.TransitionProperty, (object) value);
  }

  public bool EnableAnimation
  {
    get => (bool) this.GetValue(TransitionContentControl.EnableAnimationProperty);
    set => this.SetValue(TransitionContentControl.EnableAnimationProperty, (object) value);
  }

  public void Dispose() => this.Dispose(true);

  public override void OnApplyTemplate()
  {
    this.part_Content = this.GetTemplateChild("PART_Content") as ContentPresenter;
    this.part_TempContent = this.GetTemplateChild("PART_TempContent") as ContentControl;
    this.part_LayoutRoot = this.GetTemplateChild("PART_LayoutRoot") as Grid;
    base.OnApplyTemplate();
  }

  protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
  {
    TranslateTransform translateTransform = new TranslateTransform();
    if (this.part_LayoutRoot != null && this.part_Content != null && this.part_TempContent != null && this.Transition is SlideTransition)
    {
      this.part_LayoutRoot.Clip = (Geometry) new RectangleGeometry()
      {
        Rect = new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight)
      };
      this.part_TempContent.RenderTransform = (Transform) new TranslateTransform();
      if (sizeInfo.HeightChanged)
        translateTransform.X = this.ActualWidth;
      else
        translateTransform.Y = this.ActualHeight;
      this.part_TempContent.RenderTransform = (Transform) translateTransform;
    }
    base.OnRenderSizeChanged(sizeInfo);
  }

  protected override void OnContentChanged(object oldContent, object newContent)
  {
    if (this.part_Content != null && this.part_TempContent != null && this.part_LayoutRoot != null && this.EnableAnimation)
    {
      if (this.Transition is SlideTransition)
      {
        this.part_TempContent.Content = oldContent;
        this.part_Content.RenderTransform = (Transform) new TranslateTransform();
        this.part_TempContent.RenderTransform = (Transform) new TranslateTransform();
        if (this.ActualWidth == 0.0 && this.ActualHeight == 0.0)
          this.part_LayoutRoot.Clip = (Geometry) new RectangleGeometry()
          {
            Rect = new Rect(0.0, 0.0, this.Width, this.Height)
          };
        else
          this.part_LayoutRoot.Clip = (Geometry) new RectangleGeometry()
          {
            Rect = new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight)
          };
        Timeline enterAnimation;
        Timeline exitAnimation;
        if ((this.Transition as SlideTransition).Direction == SlideDirection.Left || (this.Transition as SlideTransition).Direction == SlideDirection.Right)
        {
          enterAnimation = (this.Transition as SlideTransition).CreateEnterAnimation(this.ActualWidth);
          exitAnimation = (this.Transition as SlideTransition).CreateExitAnimation(this.ActualWidth);
        }
        else
        {
          enterAnimation = (this.Transition as SlideTransition).CreateEnterAnimation(this.ActualHeight);
          exitAnimation = (this.Transition as SlideTransition).CreateExitAnimation(this.ActualHeight);
        }
        Storyboard storyboard1 = new Storyboard();
        storyboard1.Children.Add(enterAnimation);
        Storyboard.SetTarget((DependencyObject) enterAnimation, (DependencyObject) this.part_Content);
        if ((this.Transition as SlideTransition).Direction == SlideDirection.Left || (this.Transition as SlideTransition).Direction == SlideDirection.Right)
          Storyboard.SetTargetProperty((DependencyObject) enterAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)", new object[0]));
        else
          Storyboard.SetTargetProperty((DependencyObject) enterAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)", new object[0]));
        Storyboard storyboard2 = new Storyboard();
        storyboard2.Children.Add(exitAnimation);
        Storyboard.SetTarget((DependencyObject) exitAnimation, (DependencyObject) this.part_TempContent);
        if ((this.Transition as SlideTransition).Direction == SlideDirection.Left || (this.Transition as SlideTransition).Direction == SlideDirection.Right)
          Storyboard.SetTargetProperty((DependencyObject) exitAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)", new object[0]));
        else
          Storyboard.SetTargetProperty((DependencyObject) exitAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)", new object[0]));
        storyboard2.Begin();
        storyboard1.Begin();
      }
      else if (this.Transition is RotateTransition)
      {
        DoubleAnimationUsingKeyFrames element1 = this.part_Content.Visibility != Visibility.Visible ? (this.Transition as RotateTransition).BuildTimeLine(180.0, 360.0) : (this.Transition as RotateTransition).BuildTimeLine(0.0, 180.0);
        ObjectAnimationUsingKeyFrames element2 = (this.Transition as RotateTransition).BuildObjectTimeline((object) Visibility.Collapsed);
        ObjectAnimationUsingKeyFrames element3 = (this.Transition as RotateTransition).BuildObjectTimeline((object) Visibility.Visible);
        Storyboard storyboard = new Storyboard();
        storyboard.Children.Add((Timeline) element1);
        storyboard.Children.Add((Timeline) element2);
        storyboard.Children.Add((Timeline) element3);
        Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) this.part_LayoutRoot);
        if (this.part_Content.Visibility == Visibility.Visible)
        {
          this.part_Content.Content = newContent;
          this.part_TempContent.Visibility = Visibility.Visible;
          this.part_TempContent.Content = oldContent;
          this.part_Content.Visibility = Visibility.Collapsed;
          this.part_TempContent.Visibility = Visibility.Visible;
          this.part_Content.RenderTransformOrigin = new Point(0.5, 0.5);
          this.part_Content.RenderTransform = (Transform) new ScaleTransform()
          {
            ScaleY = -1.0
          };
          Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) this.part_TempContent);
          Storyboard.SetTarget((DependencyObject) element3, (DependencyObject) this.part_Content);
        }
        else
        {
          this.part_Content.RenderTransformOrigin = new Point(0.5, 0.5);
          Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) this.part_Content);
          Storyboard.SetTarget((DependencyObject) element3, (DependencyObject) this.part_TempContent);
        }
        Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath("(UIElement.Projection).(PlaneProjection.RotationX)", new object[0]));
        Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath("(UIElement.Visibility)", new object[0]));
        Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath("(UIElement.Visibility)", new object[0]));
        storyboard.Begin();
      }
      else if (this.Transition is FadeTransition)
      {
        this.part_TempContent.Visibility = Visibility.Visible;
        this.part_TempContent.Content = oldContent;
        this.part_TempContent.Opacity = 1.0;
        this.part_Content.Opacity = 0.0;
        FadeTransition transition = this.Transition as FadeTransition;
        Timeline element4 = transition.BuildAnimation(1.0, 0.0, transition.Duration, transition.Easing);
        Timeline element5 = transition.BuildAnimation(0.0, 1.0, transition.Duration, transition.Easing);
        Storyboard.SetTarget((DependencyObject) element4, (DependencyObject) this.part_TempContent);
        Storyboard.SetTarget((DependencyObject) element5, (DependencyObject) this.part_Content);
        Storyboard.SetTargetProperty((DependencyObject) element4, new PropertyPath("(UIElement.Opacity)", new object[0]));
        Storyboard.SetTargetProperty((DependencyObject) element5, new PropertyPath("(UIElement.Opacity)", new object[0]));
        Storyboard storyboard = new Storyboard();
        storyboard.Children.Add(element4);
        storyboard.Children.Add(element5);
        storyboard.Begin();
      }
    }
    base.OnContentChanged(oldContent, newContent);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    this.part_Content = (ContentPresenter) null;
    this.part_TempContent = (ContentControl) null;
    if (this.Transition != null && this.Transition is FadeTransition)
      (this.Transition as FadeTransition).ClearValue(FrameworkElement.StyleProperty);
    this.Style = (Style) null;
    this.Transition = (ContentTransition) null;
    if (this.part_LayoutRoot != null)
    {
      this.part_LayoutRoot.Children.Clear();
      this.part_LayoutRoot = (Grid) null;
    }
    this.ClearValue(TransitionContentControl.TransitionProperty);
    this.ClearValue(TransitionContentControl.EnableAnimationProperty);
  }
}
