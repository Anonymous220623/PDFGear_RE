// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.SlidingTabContainer
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_Sliding", Type = typeof (FrameworkElement))]
public class SlidingTabContainer : ContentControl
{
  private const string ElementSliding = "PART_Sliding";
  private FrameworkElement _sliding = new FrameworkElement();
  private System.Windows.Controls.TabControl _tabControl;

  public SlidingTabContainer() => this.Loaded += new RoutedEventHandler(this.OnLoaded);

  private void OnLoaded(object sender, RoutedEventArgs e)
  {
    this.Loaded -= new RoutedEventHandler(this.OnLoaded);
    this.OnSelectionChanged((object) null, (SelectionChangedEventArgs) null);
  }

  public override void OnApplyTemplate()
  {
    if (this._tabControl != null)
      this._tabControl.SelectionChanged -= new SelectionChangedEventHandler(this.OnSelectionChanged);
    base.OnApplyTemplate();
    this._tabControl = VisualHelper.GetParent<System.Windows.Controls.TabControl>((DependencyObject) this);
    this._sliding = this.GetTemplateChild("PART_Sliding") as FrameworkElement;
    if (this._tabControl == null)
      return;
    this._tabControl.SelectionChanged += new SelectionChangedEventHandler(this.OnSelectionChanged);
    if (!this.IsLoaded)
      return;
    this.OnSelectionChanged((object) null, (SelectionChangedEventArgs) null);
  }

  protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
  {
    base.OnRenderSizeChanged(sizeInfo);
    if (this._tabControl == null)
      return;
    this.OnSelectionChanged((object) null, (SelectionChangedEventArgs) null);
  }

  private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (this._sliding == null || !this.IsLoaded || !(this._tabControl.ItemContainerGenerator.ContainerFromItem(this._tabControl.SelectedItem) is System.Windows.Controls.TabItem reference))
      return;
    Vector offset = VisualTreeHelper.GetOffset((Visual) reference);
    double actualWidth = reference.ActualWidth;
    double actualHeight = reference.ActualHeight;
    if ((this._sliding.Width <= 0.0 ? 0 : (this._sliding.Height > 0.0 ? 1 : 0)) != 0)
    {
      Storyboard storyboard = new Storyboard();
      PowerEase powerEase1 = new PowerEase();
      powerEase1.EasingMode = EasingMode.EaseOut;
      PowerEase powerEase2 = powerEase1;
      DoubleAnimation animation1 = AnimationHelper.CreateAnimation(actualWidth);
      animation1.EasingFunction = (IEasingFunction) powerEase2;
      Storyboard.SetTarget((DependencyObject) animation1, (DependencyObject) this._sliding);
      Storyboard.SetTargetProperty((DependencyObject) animation1, new PropertyPath(FrameworkElement.WidthProperty.Name, Array.Empty<object>()));
      storyboard.Children.Add((Timeline) animation1);
      DoubleAnimation animation2 = AnimationHelper.CreateAnimation(actualHeight);
      animation2.EasingFunction = (IEasingFunction) powerEase2;
      Storyboard.SetTarget((DependencyObject) animation2, (DependencyObject) this._sliding);
      Storyboard.SetTargetProperty((DependencyObject) animation2, new PropertyPath(FrameworkElement.HeightProperty.Name, Array.Empty<object>()));
      storyboard.Children.Add((Timeline) animation2);
      DoubleAnimation animation3 = AnimationHelper.CreateAnimation(offset.X);
      animation3.EasingFunction = (IEasingFunction) powerEase2;
      Storyboard.SetTarget((DependencyObject) animation3, (DependencyObject) this._sliding);
      Storyboard.SetTargetProperty((DependencyObject) animation3, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)", Array.Empty<object>()));
      storyboard.Children.Add((Timeline) animation3);
      DoubleAnimation animation4 = AnimationHelper.CreateAnimation(offset.Y);
      animation4.EasingFunction = (IEasingFunction) powerEase2;
      Storyboard.SetTarget((DependencyObject) animation4, (DependencyObject) this._sliding);
      Storyboard.SetTargetProperty((DependencyObject) animation4, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)", Array.Empty<object>()));
      storyboard.Children.Add((Timeline) animation4);
      storyboard.Begin();
    }
    else
    {
      this._sliding.Width = reference.ActualWidth;
      this._sliding.Height = reference.ActualHeight;
      this._sliding.RenderTransform = (Transform) new TranslateTransform(offset.X, offset.Y);
    }
  }
}
