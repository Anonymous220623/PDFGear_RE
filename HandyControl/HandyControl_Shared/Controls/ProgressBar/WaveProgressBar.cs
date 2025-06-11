// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.WaveProgressBar
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Expression.Drawing;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_Wave", Type = typeof (FrameworkElement))]
[TemplatePart(Name = "PART_Clip", Type = typeof (FrameworkElement))]
public class WaveProgressBar : RangeBase
{
  private const string ElementWave = "PART_Wave";
  private const string ElementClip = "PART_Clip";
  private FrameworkElement _waveElement;
  private const double TranslateTransformMinY = -20.0;
  private double _translateTransformYRange;
  private TranslateTransform _translateTransform;
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (WaveProgressBar), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(nameof (ShowText), typeof (bool), typeof (WaveProgressBar), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty WaveFillProperty = DependencyProperty.Register(nameof (WaveFill), typeof (Brush), typeof (WaveProgressBar), new PropertyMetadata((object) null));
  public static readonly DependencyProperty WaveThicknessProperty = DependencyProperty.Register(nameof (WaveThickness), typeof (double), typeof (WaveProgressBar), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty WaveStrokeProperty = DependencyProperty.Register(nameof (WaveStroke), typeof (Brush), typeof (WaveProgressBar), new PropertyMetadata((object) null));

  static WaveProgressBar()
  {
    UIElement.FocusableProperty.OverrideMetadata(typeof (WaveProgressBar), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox));
    RangeBase.MaximumProperty.OverrideMetadata(typeof (WaveProgressBar), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double100Box));
  }

  protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
  {
    this.UpdateWave(this.Value);
  }

  protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
  {
    this.UpdateWave(this.Value);
  }

  protected override void OnValueChanged(double oldValue, double newValue)
  {
    base.OnValueChanged(oldValue, newValue);
    this.UpdateWave(newValue);
  }

  private void UpdateWave(double value)
  {
    if (this._translateTransform == null || MathHelper.IsVerySmall(this.Maximum))
      return;
    this._translateTransform.Y = this._translateTransformYRange * (1.0 - value / this.Maximum) - 20.0;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._waveElement = this.GetTemplateChild("PART_Wave") as FrameworkElement;
    FrameworkElement templateChild = this.GetTemplateChild("PART_Clip") as FrameworkElement;
    if (this._waveElement == null || templateChild == null)
      return;
    double height = templateChild.Height;
    this._translateTransform = new TranslateTransform()
    {
      Y = height
    };
    this._translateTransformYRange = height - -20.0;
    this._waveElement.RenderTransform = (Transform) new TransformGroup()
    {
      Children = {
        (Transform) this._translateTransform
      }
    };
    this.UpdateWave(this.Value);
  }

  public string Text
  {
    get => (string) this.GetValue(WaveProgressBar.TextProperty);
    set => this.SetValue(WaveProgressBar.TextProperty, (object) value);
  }

  public bool ShowText
  {
    get => (bool) this.GetValue(WaveProgressBar.ShowTextProperty);
    set => this.SetValue(WaveProgressBar.ShowTextProperty, ValueBoxes.BooleanBox(value));
  }

  public Brush WaveFill
  {
    get => (Brush) this.GetValue(WaveProgressBar.WaveFillProperty);
    set => this.SetValue(WaveProgressBar.WaveFillProperty, (object) value);
  }

  public double WaveThickness
  {
    get => (double) this.GetValue(WaveProgressBar.WaveThicknessProperty);
    set => this.SetValue(WaveProgressBar.WaveThicknessProperty, (object) value);
  }

  public Brush WaveStroke
  {
    get => (Brush) this.GetValue(WaveProgressBar.WaveStrokeProperty);
    set => this.SetValue(WaveProgressBar.WaveStrokeProperty, (object) value);
  }
}
