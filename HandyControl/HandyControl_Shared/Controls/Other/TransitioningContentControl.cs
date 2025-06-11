// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.TransitioningContentControl
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Controls;

public class TransitioningContentControl : ContentControl
{
  private FrameworkElement _contentPresenter;
  private static Storyboard StoryboardBuildInDefault;
  private Storyboard _storyboardBuildIn;
  public static readonly DependencyProperty TransitionModeProperty = DependencyProperty.Register(nameof (TransitionMode), typeof (TransitionMode), typeof (TransitioningContentControl), new PropertyMetadata((object) TransitionMode.Right2Left, new PropertyChangedCallback(TransitioningContentControl.OnTransitionModeChanged)));
  public static readonly DependencyProperty TransitionStoryboardProperty = DependencyProperty.Register(nameof (TransitionStoryboard), typeof (Storyboard), typeof (TransitioningContentControl), new PropertyMetadata((object) null));

  public TransitioningContentControl()
  {
    this.Loaded += new RoutedEventHandler(this.TransitioningContentControl_Loaded);
    this.Unloaded += new RoutedEventHandler(this.TransitioningContentControl_Unloaded);
  }

  private static void OnTransitionModeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((TransitioningContentControl) d).OnTransitionModeChanged((TransitionMode) e.NewValue);
  }

  private void OnTransitionModeChanged(TransitionMode newValue)
  {
    this._storyboardBuildIn = HandyControl.Tools.ResourceHelper.GetResourceInternal<Storyboard>($"{newValue}Transition");
    this.StartTransition();
  }

  public TransitionMode TransitionMode
  {
    get => (TransitionMode) this.GetValue(TransitioningContentControl.TransitionModeProperty);
    set => this.SetValue(TransitioningContentControl.TransitionModeProperty, (object) value);
  }

  public Storyboard TransitionStoryboard
  {
    get => (Storyboard) this.GetValue(TransitioningContentControl.TransitionStoryboardProperty);
    set => this.SetValue(TransitioningContentControl.TransitionStoryboardProperty, (object) value);
  }

  private void TransitioningContentControl_IsVisibleChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
    this.StartTransition();
  }

  private void TransitioningContentControl_Loaded(object sender, RoutedEventArgs e)
  {
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.TransitioningContentControl_IsVisibleChanged);
  }

  private void TransitioningContentControl_Unloaded(object sender, RoutedEventArgs e)
  {
    this.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.TransitioningContentControl_IsVisibleChanged);
  }

  private void StartTransition()
  {
    if (this._contentPresenter == null || !this.IsVisible)
      return;
    if (this.TransitionStoryboard != null)
      this.TransitionStoryboard.Begin(this._contentPresenter);
    else if (this._storyboardBuildIn != null)
    {
      this._storyboardBuildIn?.Begin(this._contentPresenter);
    }
    else
    {
      if (TransitioningContentControl.StoryboardBuildInDefault == null)
        TransitioningContentControl.StoryboardBuildInDefault = HandyControl.Tools.ResourceHelper.GetResourceInternal<Storyboard>($"{(Enum) TransitionMode.Right2Left}Transition");
      TransitioningContentControl.StoryboardBuildInDefault?.Begin(this._contentPresenter);
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._contentPresenter = VisualTreeHelper.GetChild((DependencyObject) this, 0) as FrameworkElement;
    if (this._contentPresenter != null)
    {
      this._contentPresenter.RenderTransformOrigin = new Point(0.5, 0.5);
      this._contentPresenter.RenderTransform = (Transform) new TransformGroup()
      {
        Children = {
          (Transform) new ScaleTransform(),
          (Transform) new SkewTransform(),
          (Transform) new RotateTransform(),
          (Transform) new TranslateTransform()
        }
      };
    }
    this.StartTransition();
  }

  protected override void OnContentChanged(object oldContent, object newContent)
  {
    base.OnContentChanged(oldContent, newContent);
    if (newContent == null)
      return;
    this.StartTransition();
  }
}
