// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.CircleProgressBar
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Expression.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_Indicator", Type = typeof (Arc))]
public class CircleProgressBar : RangeBase
{
  private const string IndicatorTemplateName = "PART_Indicator";
  public static readonly DependencyProperty ArcThicknessProperty = DependencyProperty.Register(nameof (ArcThickness), typeof (double), typeof (CircleProgressBar), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(nameof (ShowText), typeof (bool), typeof (CircleProgressBar), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (CircleProgressBar), new PropertyMetadata((object) null));
  public static readonly DependencyProperty IsIndeterminateProperty = ProgressBar.IsIndeterminateProperty.AddOwner(typeof (CircleProgressBar), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox));
  private Arc _indicator;

  static CircleProgressBar()
  {
    UIElement.FocusableProperty.OverrideMetadata(typeof (CircleProgressBar), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox));
    RangeBase.MaximumProperty.OverrideMetadata(typeof (CircleProgressBar), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double100Box));
  }

  public string Text
  {
    get => (string) this.GetValue(CircleProgressBar.TextProperty);
    set => this.SetValue(CircleProgressBar.TextProperty, (object) value);
  }

  public bool ShowText
  {
    get => (bool) this.GetValue(CircleProgressBar.ShowTextProperty);
    set => this.SetValue(CircleProgressBar.ShowTextProperty, ValueBoxes.BooleanBox(value));
  }

  public double ArcThickness
  {
    get => (double) this.GetValue(CircleProgressBar.ArcThicknessProperty);
    set => this.SetValue(CircleProgressBar.ArcThicknessProperty, (object) value);
  }

  public bool IsIndeterminate
  {
    get => (bool) this.GetValue(CircleProgressBar.IsIndeterminateProperty);
    set => this.SetValue(CircleProgressBar.IsIndeterminateProperty, ValueBoxes.BooleanBox(value));
  }

  private void SetProgressBarIndicatorAngle()
  {
    if (this._indicator == null)
      return;
    double minimum = this.Minimum;
    double maximum = this.Maximum;
    double num = this.Value;
    this._indicator.EndAngle = (maximum <= minimum ? 0.0 : (num - minimum) / (maximum - minimum)) * 360.0;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._indicator = this.GetTemplateChild("PART_Indicator") as Arc;
    if (this._indicator != null)
    {
      this._indicator.StartAngle = 0.0;
      this._indicator.EndAngle = 0.0;
    }
    this.SetProgressBarIndicatorAngle();
  }

  protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
  {
    base.OnMinimumChanged(oldMinimum, newMinimum);
    this.SetProgressBarIndicatorAngle();
  }

  protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
  {
    base.OnMaximumChanged(oldMaximum, newMaximum);
    this.SetProgressBarIndicatorAngle();
  }

  protected override void OnValueChanged(double oldValue, double newValue)
  {
    base.OnValueChanged(oldValue, newValue);
    this.SetProgressBarIndicatorAngle();
  }
}
