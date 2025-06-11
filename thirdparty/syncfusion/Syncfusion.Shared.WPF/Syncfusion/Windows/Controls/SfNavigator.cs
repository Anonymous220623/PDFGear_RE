// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.SfNavigator
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.Windows.Controls;

[ContentProperty("Host")]
public class SfNavigator : Control
{
  public static readonly DependencyProperty ActiveItemProperty = DependencyProperty.Register(nameof (ActiveItem), typeof (object), typeof (SfNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfNavigator.OnActiveItemChanged)));
  public static readonly DependencyProperty ActiveIndexProperty = DependencyProperty.Register(nameof (ActiveIndex), typeof (int), typeof (SfNavigator), new PropertyMetadata((object) -1, new PropertyChangedCallback(SfNavigator.OnActiveIndexChanged)));
  public static readonly DependencyProperty HostProperty = DependencyProperty.Register(nameof (Host), typeof (object), typeof (SfNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfNavigator.OnHostChanged)));
  private SfNavigator.Direction direction;
  private ContentControl part_Content;
  private ContentControl part_SupportingContent;
  private ContentControl activeContent;
  private ContentControl supportingContent;

  public SfNavigator()
  {
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
    this.DefaultStyleKey = (object) typeof (SfNavigator);
    this.Loaded += new RoutedEventHandler(this.NavigatorLoaded);
    this.SizeChanged -= new SizeChangedEventHandler(this.SfNavigator_SizeChanged);
    this.SizeChanged += new SizeChangedEventHandler(this.SfNavigator_SizeChanged);
    this.Items = new ChildCollection();
  }

  public event RoutedEventHandler Navigated;

  public object ActiveItem
  {
    get => this.GetValue(SfNavigator.ActiveItemProperty);
    set => this.SetValue(SfNavigator.ActiveItemProperty, value);
  }

  public int ActiveIndex
  {
    get => (int) this.GetValue(SfNavigator.ActiveIndexProperty);
    set => this.SetValue(SfNavigator.ActiveIndexProperty, (object) value);
  }

  public ChildCollection Items { get; private set; }

  public object Host
  {
    get => this.GetValue(SfNavigator.HostProperty);
    set => this.SetValue(SfNavigator.HostProperty, value);
  }

  public void Navigate(object child)
  {
    if (child == null)
      return;
    if (this.ActualHeight > 0.0 && this.ActualWidth > 0.0)
      this.Clip = (Geometry) new RectangleGeometry()
      {
        Rect = new Rect()
        {
          Height = this.ActualHeight,
          Width = this.ActualWidth,
          X = 0.0,
          Y = 0.0
        }
      };
    if (this.activeContent == null)
    {
      this.part_Content.Content = child;
      this.activeContent = this.part_Content;
      this.supportingContent = this.part_SupportingContent;
    }
    else
    {
      if (this.direction == SfNavigator.Direction.Next)
      {
        this.supportingContent.RenderTransform = (Transform) new TranslateTransform()
        {
          X = this.activeContent.ActualWidth
        };
        this.supportingContent.Content = child;
        this.activeContent.RenderTransform = (Transform) new TranslateTransform();
        Storyboard storyBoard1 = this.CreateStoryBoard((DependencyObject) this.activeContent, "(UIElement.RenderTransform).(TranslateTransform.X)", 0.0, -this.activeContent.ActualWidth);
        Storyboard storyBoard2 = this.CreateStoryBoard((DependencyObject) this.supportingContent, "(UIElement.RenderTransform).(TranslateTransform.X)", this.activeContent.ActualWidth, 0.0);
        if (this.Navigated != null)
          this.Navigated((object) this, new RoutedEventArgs());
        storyBoard1.Begin();
        storyBoard2.Begin();
      }
      else
      {
        this.supportingContent.RenderTransform = (Transform) new TranslateTransform()
        {
          X = -this.activeContent.ActualWidth
        };
        this.supportingContent.Content = child;
        this.activeContent.RenderTransform = (Transform) new TranslateTransform();
        Storyboard storyBoard3 = this.CreateStoryBoard((DependencyObject) this.activeContent, "(UIElement.RenderTransform).(TranslateTransform.X)", 0.0, this.activeContent.ActualWidth);
        Storyboard storyBoard4 = this.CreateStoryBoard((DependencyObject) this.supportingContent, "(UIElement.RenderTransform).(TranslateTransform.X)", -this.activeContent.ActualWidth, 0.0);
        storyBoard4.Completed += (EventHandler) ((sender, e) =>
        {
          if (this.Navigated == null)
            return;
          this.Navigated((object) this, new RoutedEventArgs());
        });
        storyBoard3.Begin();
        storyBoard4.Begin();
      }
      this.SwapActiveContent();
    }
  }

  public void UpdateTransform()
  {
    Thickness thickness = new Thickness(0.0);
    if (this.Parent is FrameworkElement)
      thickness = (this.Parent as FrameworkElement).Margin;
    if (this.activeContent == null || this.supportingContent == null)
      return;
    if ((this.supportingContent.RenderTransform as TranslateTransform).X < 0.0 && -(this.supportingContent.RenderTransform as TranslateTransform).X < this.activeContent.ActualWidth)
    {
      (this.supportingContent.RenderTransform as TranslateTransform).X = -this.activeContent.ActualWidth - thickness.Left - thickness.Right;
    }
    else
    {
      if ((this.supportingContent.RenderTransform as TranslateTransform).X <= 0.0 || (this.supportingContent.RenderTransform as TranslateTransform).X > this.activeContent.ActualWidth)
        return;
      (this.supportingContent.RenderTransform as TranslateTransform).X = this.activeContent.ActualWidth + thickness.Left + thickness.Right;
    }
  }

  public override void OnApplyTemplate()
  {
    this.part_Content = this.GetTemplateChild("PART_Content") as ContentControl;
    this.part_SupportingContent = this.GetTemplateChild("PART_SupportingContent") as ContentControl;
    base.OnApplyTemplate();
  }

  protected virtual void OnActiveItemChanged(DependencyPropertyChangedEventArgs args)
  {
    if (args.NewValue == null || !this.Items.Contains(args.NewValue))
      return;
    int num1 = this.Items.IndexOf(args.NewValue);
    int num2 = this.Items.IndexOf(args.OldValue);
    if (num1 < 0)
      return;
    this.direction = num1 > num2 ? SfNavigator.Direction.Next : SfNavigator.Direction.Previous;
    this.ActiveIndex = num1;
    if (this.part_Content == null || this.part_SupportingContent == null)
      return;
    this.Navigate(args.NewValue);
  }

  protected virtual void OnActiveIndexChanged(DependencyPropertyChangedEventArgs args)
  {
    this.ValidateActiveIndex((int) args.NewValue, (int) args.OldValue);
  }

  private void ValidateActiveIndex(int index)
  {
    if (index < 0 || index >= this.Items.Count)
      return;
    this.ActiveItem = this.Items[index];
  }

  private void ValidateActiveIndex(int index, int oldindex)
  {
    if (index < 0 || index >= this.Items.Count)
      return;
    this.direction = index > oldindex ? SfNavigator.Direction.Next : SfNavigator.Direction.Previous;
    this.ActiveItem = this.Items[index];
  }

  private static void OnActiveIndexChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(sender is SfNavigator sfNavigator))
      return;
    sfNavigator.OnActiveIndexChanged(args);
  }

  private static void OnActiveItemChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(sender is SfNavigator sfNavigator))
      return;
    sfNavigator.OnActiveItemChanged(args);
  }

  private static void OnHostChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    SfNavigator sfNavigator = sender as SfNavigator;
    sfNavigator.Items.Add(sfNavigator.Host);
  }

  private void SfNavigator_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    TranslateTransform translateTransform = new TranslateTransform();
    if (this.supportingContent == null || this.activeContent == null)
      return;
    translateTransform.X = this.activeContent.ActualWidth;
    this.supportingContent.RenderTransform = (Transform) translateTransform;
  }

  private void SfNavigator_LayoutUpdated(object sender, object e) => this.UpdateTransform();

  private void NavigatorLoaded(object sender, RoutedEventArgs e)
  {
    if (this.ActiveIndex >= 0)
      this.ValidateActiveIndex(this.ActiveIndex);
    this.LayoutUpdated += new EventHandler(this.SfNavigator_LayoutUpdated);
    this.Loaded -= new RoutedEventHandler(this.NavigatorLoaded);
  }

  private void SwapActiveContent()
  {
    ContentControl activeContent = this.activeContent;
    this.activeContent = this.supportingContent;
    this.supportingContent = activeContent;
  }

  private Timeline CreateAnimation(double fromvalue, double toValue)
  {
    DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
    EasingDoubleKeyFrame easingDoubleKeyFrame1 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame1.Value = fromvalue;
    easingDoubleKeyFrame1.KeyTime = (KeyTime) TimeSpan.FromSeconds(0.0);
    EasingDoubleKeyFrame keyFrame1 = easingDoubleKeyFrame1;
    EasingDoubleKeyFrame easingDoubleKeyFrame2 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame2.KeyTime = (KeyTime) TimeSpan.FromSeconds(0.3);
    easingDoubleKeyFrame2.Value = toValue;
    EasingDoubleKeyFrame keyFrame2 = easingDoubleKeyFrame2;
    keyFrame2.EasingFunction = (IEasingFunction) new PowerEase();
    animation.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
    animation.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
    return (Timeline) animation;
  }

  private Storyboard CreateStoryBoard(
    DependencyObject target,
    string property,
    double from,
    double to)
  {
    Timeline animation = this.CreateAnimation(from, to);
    Storyboard storyBoard = new Storyboard();
    Storyboard.SetTarget((DependencyObject) animation, target);
    Storyboard.SetTargetProperty((DependencyObject) animation, new PropertyPath(property, new object[0]));
    storyBoard.Children.Add(animation);
    return storyBoard;
  }

  public enum Direction
  {
    Next,
    Previous,
  }
}
