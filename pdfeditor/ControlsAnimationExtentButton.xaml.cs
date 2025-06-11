// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.AnimationExtentButton
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls;

public partial class AnimationExtentButton : Button
{
  private bool isMouseOverInternal;
  private Border contentContainer;
  private PathFigure StartFigure;
  private ArcSegment LeftArc;
  private LineSegment CenterLine1;
  private ArcSegment RightArc;
  private LineSegment CenterLine2;
  private Path ContentBackground;
  private EasingFunctionBase backgroundAnimationFunc;
  private Storyboard currentSb;
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (AnimationExtentButton), new PropertyMetadata((object) null, new PropertyChangedCallback(AnimationExtentButton.OnHeaderPropertyChanged)));
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (AnimationExtentButton), new PropertyMetadata((PropertyChangedCallback) null));

  static AnimationExtentButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (AnimationExtentButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (AnimationExtentButton)));
  }

  public AnimationExtentButton()
  {
    this.Unloaded += new RoutedEventHandler(this.AnimationExtentButton_Unloaded);
    ExponentialEase exponentialEase = new ExponentialEase();
    exponentialEase.EasingMode = EasingMode.EaseInOut;
    exponentialEase.Exponent = 7.0;
    this.backgroundAnimationFunc = (EasingFunctionBase) exponentialEase;
  }

  private Border ContentContainer
  {
    get => this.contentContainer;
    set
    {
      if (this.contentContainer == value)
        return;
      if (this.contentContainer != null)
        this.contentContainer.SizeChanged -= new SizeChangedEventHandler(this.ContentContainer_SizeChanged);
      this.contentContainer = value;
      if (this.contentContainer != null)
        this.contentContainer.SizeChanged += new SizeChangedEventHandler(this.ContentContainer_SizeChanged);
      this.UpdateContentSize();
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.ContentContainer = this.GetTemplateChild("ContentContainer") as Border;
    this.StartFigure = this.GetTemplateChild("StartFigure") as PathFigure;
    this.LeftArc = this.GetTemplateChild("LeftArc") as ArcSegment;
    this.CenterLine1 = this.GetTemplateChild("CenterLine1") as LineSegment;
    this.RightArc = this.GetTemplateChild("RightArc") as ArcSegment;
    this.CenterLine2 = this.GetTemplateChild("CenterLine2") as LineSegment;
    this.ContentBackground = this.GetTemplateChild("ContentBackground") as Path;
    this.UpdateMouseOverState();
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    base.OnMouseEnter(e);
    this.UpdateMouseOverState();
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    this.UpdateMouseOverState();
  }

  protected override void OnIsMouseDirectlyOverChanged(DependencyPropertyChangedEventArgs e)
  {
    base.OnIsMouseDirectlyOverChanged(e);
    this.UpdateMouseOverState();
  }

  protected override void OnPreviewMouseMove(MouseEventArgs e)
  {
    base.OnPreviewMouseMove(e);
    this.UpdateMouseOverState();
  }

  private void AnimationExtentButton_Unloaded(object sender, RoutedEventArgs e)
  {
    this.UpdateMouseOverState();
  }

  private void ContentContainer_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.UpdateContentSize();
  }

  public object Header
  {
    get => this.GetValue(AnimationExtentButton.HeaderProperty);
    set => this.SetValue(AnimationExtentButton.HeaderProperty, value);
  }

  private static void OnHeaderPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is AnimationExtentButton animationExtentButton))
      return;
    animationExtentButton.RemoveLogicalChild(e.OldValue);
    if (!(e.NewValue is DependencyObject newValue))
      return;
    animationExtentButton.AddLogicalChild((object) newValue);
  }

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(AnimationExtentButton.HeaderTemplateProperty);
    set => this.SetValue(AnimationExtentButton.HeaderTemplateProperty, (object) value);
  }

  private void UpdateMouseOverState(bool disableAnimation = false)
  {
    bool isMouseOver = this.IsMouseOver;
    if (isMouseOver == this.isMouseOverInternal)
      return;
    if (this.IsMouseOver)
    {
      VisualStateManager.GoToState((FrameworkElement) this, "IsMouseOverState", true);
      this.currentSb?.Stop();
      this.currentSb = this.BuildShowStoryboard();
      if (disableAnimation)
        this.currentSb.SkipToFill();
      else
        this.currentSb.Begin();
    }
    else
    {
      VisualStateManager.GoToState((FrameworkElement) this, "Normal", true);
      this.currentSb?.Stop();
      this.currentSb = this.BuildHideStoryboard();
      if (disableAnimation)
        this.currentSb.SkipToFill();
      else
        this.currentSb.Begin();
    }
    this.isMouseOverInternal = isMouseOver;
  }

  private void UpdateContentSize()
  {
    if (this.ContentContainer == null)
      return;
    Canvas.SetLeft((UIElement) this.ContentContainer, -this.ContentContainer.ActualWidth);
  }

  private Storyboard BuildShowStoryboard()
  {
    return this.ContentContainer != null ? this.BuildStoryboardCore(this.ContentContainer.ActualWidth, TimeSpan.FromSeconds(0.3), TimeSpan.Zero) : (Storyboard) null;
  }

  private Storyboard BuildHideStoryboard()
  {
    return this.ContentContainer != null ? this.BuildStoryboardCore(0.0, TimeSpan.FromSeconds(0.3), TimeSpan.Zero) : (Storyboard) null;
  }

  private Storyboard BuildStoryboardCore(double width, TimeSpan duration, TimeSpan beginTime)
  {
    if (this.ContentContainer == null || this.StartFigure == null || this.LeftArc == null || this.CenterLine1 == null || this.RightArc == null || this.CenterLine2 == null)
      return (Storyboard) null;
    Storyboard _sb = new Storyboard();
    AddPointAnimation(_sb, new Point(20.0 - width, 0.0), (DependencyObject) this.StartFigure, PathFigure.StartPointProperty, duration, beginTime);
    AddPointAnimation(_sb, new Point(20.0 - width, 40.0), (DependencyObject) this.LeftArc, ArcSegment.PointProperty, duration, beginTime);
    AddPointAnimation(_sb, new Point(20.0 - width, 0.0), (DependencyObject) this.CenterLine2, LineSegment.PointProperty, duration, beginTime);
    _sb.Freeze();
    return _sb;

    void AddPointAnimation(
      Storyboard _sb,
      Point _to,
      DependencyObject _target,
      DependencyProperty _property,
      TimeSpan _duration,
      TimeSpan _beginTime)
    {
      PointAnimation pointAnimation = new PointAnimation();
      pointAnimation.To = new Point?(_to);
      pointAnimation.EasingFunction = (IEasingFunction) this.backgroundAnimationFunc;
      pointAnimation.Duration = (Duration) _duration;
      pointAnimation.BeginTime = new TimeSpan?(_beginTime);
      PointAnimation element = pointAnimation;
      Storyboard.SetTarget((DependencyObject) element, _target);
      Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath((object) _property));
      _sb.Children.Add((Timeline) element);
    }
  }
}
