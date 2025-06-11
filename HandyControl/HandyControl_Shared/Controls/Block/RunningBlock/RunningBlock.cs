// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.RunningBlock
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Expression.Drawing;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_ContentElement", Type = typeof (FrameworkElement))]
[TemplatePart(Name = "PART_Panel", Type = typeof (Panel))]
public class RunningBlock : ContentControl
{
  private const string ElementContent = "PART_ContentElement";
  private const string ElementPanel = "PART_Panel";
  protected Storyboard _storyboard;
  private FrameworkElement _elementContent;
  private FrameworkElement _elementPanel;
  public static readonly DependencyProperty RunawayProperty = DependencyProperty.Register(nameof (Runaway), typeof (bool), typeof (RunningBlock), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AutoRunProperty = DependencyProperty.Register(nameof (AutoRun), typeof (bool), typeof (RunningBlock), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (RunningBlock), (PropertyMetadata) new FrameworkPropertyMetadata((object) Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof (Duration), typeof (Duration), typeof (RunningBlock), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Duration(TimeSpan.FromSeconds(5.0)), FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register(nameof (Speed), typeof (double), typeof (RunningBlock), (PropertyMetadata) new FrameworkPropertyMetadata((object) double.NaN, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register(nameof (IsRunning), typeof (bool), typeof (RunningBlock), new PropertyMetadata(ValueBoxes.TrueBox, (PropertyChangedCallback) ((o, args) =>
  {
    RunningBlock runningBlock = (RunningBlock) o;
    if ((bool) args.NewValue)
      runningBlock._storyboard?.Resume();
    else
      runningBlock._storyboard?.Pause();
  })));
  public static readonly DependencyProperty AutoReverseProperty = DependencyProperty.Register(nameof (AutoReverse), typeof (bool), typeof (RunningBlock), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty RunningDirectionProperty = DependencyProperty.Register(nameof (RunningDirection), typeof (RunningDirection), typeof (RunningBlock), new PropertyMetadata((object) RunningDirection.EndToStart));

  public override void OnApplyTemplate()
  {
    if (this._elementPanel != null)
      this._elementPanel.SizeChanged -= new SizeChangedEventHandler(this.ElementPanel_SizeChanged);
    base.OnApplyTemplate();
    this._elementContent = this.GetTemplateChild("PART_ContentElement") as FrameworkElement;
    this._elementPanel = (FrameworkElement) (this.GetTemplateChild("PART_Panel") as Panel);
    if (this._elementPanel != null)
      this._elementPanel.SizeChanged += new SizeChangedEventHandler(this.ElementPanel_SizeChanged);
    this.UpdateContent();
  }

  protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
  {
    base.OnRenderSizeChanged(sizeInfo);
    this.UpdateContent();
  }

  private void ElementPanel_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.UpdateContent();
  }

  public bool Runaway
  {
    get => (bool) this.GetValue(RunningBlock.RunawayProperty);
    set => this.SetValue(RunningBlock.RunawayProperty, ValueBoxes.BooleanBox(value));
  }

  public bool AutoRun
  {
    get => (bool) this.GetValue(RunningBlock.AutoRunProperty);
    set => this.SetValue(RunningBlock.AutoRunProperty, ValueBoxes.BooleanBox(value));
  }

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(RunningBlock.OrientationProperty);
    set => this.SetValue(RunningBlock.OrientationProperty, (object) value);
  }

  public Duration Duration
  {
    get => (Duration) this.GetValue(RunningBlock.DurationProperty);
    set => this.SetValue(RunningBlock.DurationProperty, (object) value);
  }

  public double Speed
  {
    get => (double) this.GetValue(RunningBlock.SpeedProperty);
    set => this.SetValue(RunningBlock.SpeedProperty, (object) value);
  }

  public bool IsRunning
  {
    get => (bool) this.GetValue(RunningBlock.IsRunningProperty);
    set => this.SetValue(RunningBlock.IsRunningProperty, ValueBoxes.BooleanBox(value));
  }

  public bool AutoReverse
  {
    get => (bool) this.GetValue(RunningBlock.AutoReverseProperty);
    set => this.SetValue(RunningBlock.AutoReverseProperty, ValueBoxes.BooleanBox(value));
  }

  public RunningDirection RunningDirection
  {
    get => (RunningDirection) this.GetValue(RunningBlock.RunningDirectionProperty);
    set => this.SetValue(RunningBlock.RunningDirectionProperty, (object) value);
  }

  private void UpdateContent()
  {
    if (this._elementContent == null || this._elementPanel == null || MathHelper.IsZero(this._elementPanel.ActualWidth) || MathHelper.IsZero(this._elementPanel.ActualHeight))
      return;
    this._storyboard?.Stop();
    double num1;
    double num2;
    PropertyPath path;
    if (this.Orientation == Orientation.Horizontal)
    {
      if (this.AutoRun && this._elementPanel.ActualWidth < this.ActualWidth)
        return;
      if (this.Runaway)
      {
        num1 = -this._elementPanel.ActualWidth;
        num2 = this.ActualWidth;
      }
      else
      {
        num1 = 0.0;
        num2 = this.ActualWidth - this._elementPanel.ActualWidth;
        this.SetCurrentValue(RunningBlock.AutoReverseProperty, ValueBoxes.TrueBox);
      }
      path = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)", Array.Empty<object>());
    }
    else
    {
      if (this.AutoRun && this._elementPanel.ActualHeight < this.ActualHeight)
        return;
      if (this.Runaway)
      {
        num1 = -this._elementPanel.ActualHeight;
        num2 = this.ActualHeight;
      }
      else
      {
        num1 = 0.0;
        num2 = this.ActualHeight - this._elementPanel.ActualHeight;
        this.SetCurrentValue(RunningBlock.AutoReverseProperty, ValueBoxes.TrueBox);
      }
      path = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)", Array.Empty<object>());
    }
    Duration duration = double.IsNaN(this.Speed) ? this.Duration : (!MathHelper.IsVerySmall(this.Speed) ? (Duration) TimeSpan.FromSeconds(Math.Abs(num2 - num1) / this.Speed) : this.Duration);
    DoubleAnimation element = this.RunningDirection == RunningDirection.EndToStart ? new DoubleAnimation(num2, num1, duration) : new DoubleAnimation(num1, num2, duration);
    element.RepeatBehavior = RepeatBehavior.Forever;
    element.AutoReverse = this.AutoReverse;
    Storyboard.SetTargetProperty((DependencyObject) element, path);
    Storyboard.SetTarget((DependencyObject) element, (DependencyObject) this._elementContent);
    this._storyboard = new Storyboard();
    this._storyboard.Children.Add((Timeline) element);
    this._storyboard.Begin();
  }
}
