// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.ProgressRingTemplateSettings
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace CommomLib.Controls;

public class ProgressRingTemplateSettings : DependencyObject
{
  private const double Radius = 40.0;
  public static readonly DependencyProperty RadiusRatioProperty = DependencyProperty.Register(nameof (RadiusRatio), typeof (double), typeof (ProgressRingTemplateSettings), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ProgressRingTemplateSettings.OnRadiusRatioPropertyChanged)));
  private static readonly DependencyPropertyKey StartPointPropertyKey = DependencyProperty.RegisterReadOnly(nameof (StartPoint), typeof (Point), typeof (ProgressRingTemplateSettings), new PropertyMetadata((object) new Point()));
  public static readonly DependencyProperty StartPointProperty = ProgressRingTemplateSettings.StartPointPropertyKey.DependencyProperty;
  private static readonly DependencyPropertyKey ArcPointPropertyKey = DependencyProperty.RegisterReadOnly(nameof (ArcPoint), typeof (Point), typeof (ProgressRingTemplateSettings), new PropertyMetadata((object) new Point()));
  public static readonly DependencyProperty ArcPointProperty = ProgressRingTemplateSettings.ArcPointPropertyKey.DependencyProperty;
  private static readonly DependencyPropertyKey ArcRotationAnglePropertyKey = DependencyProperty.RegisterReadOnly(nameof (ArcRotationAngle), typeof (double), typeof (ProgressRingTemplateSettings), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty ArcRotationAngleProperty = ProgressRingTemplateSettings.ArcRotationAnglePropertyKey.DependencyProperty;
  private static readonly DependencyPropertyKey IsLargeArcPropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsLargeArc), typeof (bool), typeof (ProgressRingTemplateSettings), new PropertyMetadata((object) false));
  public static readonly DependencyProperty IsLargeArcProperty = ProgressRingTemplateSettings.IsLargeArcPropertyKey.DependencyProperty;
  private static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(nameof (IsIndeterminate), typeof (bool), typeof (ProgressRingTemplateSettings), new PropertyMetadata((object) true, new PropertyChangedCallback(ProgressRingTemplateSettings.OnIsIndeterminatePropertyChanged)));

  public double RadiusRatio
  {
    get => (double) this.GetValue(ProgressRingTemplateSettings.RadiusRatioProperty);
    set => this.SetValue(ProgressRingTemplateSettings.RadiusRatioProperty, (object) value);
  }

  private static void OnRadiusRatioPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is ProgressRingTemplateSettings templateSettings))
      return;
    templateSettings.UpdateProperties();
  }

  public Point StartPoint
  {
    get => (Point) this.GetValue(ProgressRingTemplateSettings.StartPointProperty);
    private set
    {
      this.SetValue(ProgressRingTemplateSettings.StartPointPropertyKey, (object) value);
    }
  }

  public Point ArcPoint
  {
    get => (Point) this.GetValue(ProgressRingTemplateSettings.ArcPointProperty);
    private set => this.SetValue(ProgressRingTemplateSettings.ArcPointPropertyKey, (object) value);
  }

  public double ArcRotationAngle
  {
    get => (double) this.GetValue(ProgressRingTemplateSettings.ArcRotationAngleProperty);
    private set
    {
      this.SetValue(ProgressRingTemplateSettings.ArcRotationAnglePropertyKey, (object) value);
    }
  }

  public bool IsLargeArc
  {
    get => (bool) this.GetValue(ProgressRingTemplateSettings.IsLargeArcProperty);
    private set
    {
      this.SetValue(ProgressRingTemplateSettings.IsLargeArcPropertyKey, (object) value);
    }
  }

  private void UpdateProperties()
  {
    double num1 = Math.Min(1.0, Math.Max(0.0, this.RadiusRatio));
    double num2 = num1 * 360.0;
    double angle = num1 <= 0.5 || !this.IsIndeterminate ? num2 : num2 - 180.0;
    this.ArcRotationAngle = angle;
    if (num1 <= 0.5 || !this.IsIndeterminate)
    {
      this.StartPoint = new Point(0.0, -40.0);
      Matrix identity = Matrix.Identity;
      if (angle == 360.0)
        angle -= 0.5;
      identity.Rotate(angle);
      this.ArcPoint = identity.Transform(new Point(0.0, -40.0));
      this.IsLargeArc = !this.IsIndeterminate && num1 > 0.5;
    }
    else
    {
      Matrix identity = Matrix.Identity;
      identity.Rotate(angle);
      this.StartPoint = identity.Transform(new Point(0.0, -40.0));
      this.ArcPoint = new Point(0.0, 40.0);
    }
  }

  internal bool IsIndeterminate
  {
    get => (bool) this.GetValue(ProgressRingTemplateSettings.IsIndeterminateProperty);
    set => this.SetValue(ProgressRingTemplateSettings.IsIndeterminateProperty, (object) value);
  }

  private static void OnIsIndeterminatePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ProgressRingTemplateSettings templateSettings))
      return;
    templateSettings.UpdateProperties();
  }
}
