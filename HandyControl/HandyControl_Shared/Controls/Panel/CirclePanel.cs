// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.CirclePanel
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class CirclePanel : Panel
{
  public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register(nameof (Diameter), typeof (double), typeof (CirclePanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) 170.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty KeepVerticalProperty = DependencyProperty.Register(nameof (KeepVertical), typeof (bool), typeof (CirclePanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty OffsetAngleProperty = DependencyProperty.Register(nameof (OffsetAngle), typeof (double), typeof (CirclePanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure));

  public double Diameter
  {
    get => (double) this.GetValue(CirclePanel.DiameterProperty);
    set => this.SetValue(CirclePanel.DiameterProperty, (object) value);
  }

  public bool KeepVertical
  {
    get => (bool) this.GetValue(CirclePanel.KeepVerticalProperty);
    set => this.SetValue(CirclePanel.KeepVerticalProperty, ValueBoxes.BooleanBox(value));
  }

  public double OffsetAngle
  {
    get => (double) this.GetValue(CirclePanel.OffsetAngleProperty);
    set => this.SetValue(CirclePanel.OffsetAngleProperty, (object) value);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    double diameter = this.Diameter;
    if (this.Children.Count == 0)
      return new Size(diameter, diameter);
    Size availableSize1 = new Size(diameter, diameter);
    foreach (UIElement child in this.Children)
      child.Measure(availableSize1);
    return availableSize1;
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    bool keepVertical = this.KeepVertical;
    double offsetAngle = this.OffsetAngle;
    int num1 = 0;
    double num2 = 360.0 / (double) this.Children.Count;
    double num3 = this.Diameter / 2.0;
    foreach (UIElement child in this.Children)
    {
      Size desiredSize = child.DesiredSize;
      double num4 = desiredSize.Width / 2.0;
      desiredSize = child.DesiredSize;
      double num5 = desiredSize.Height / 2.0;
      double num6 = num2 * (double) num1++ + offsetAngle;
      RotateTransform rotateTransform = new RotateTransform()
      {
        CenterX = num4,
        CenterY = num5,
        Angle = keepVertical ? 0.0 : num6
      };
      child.RenderTransform = (Transform) rotateTransform;
      double num7 = Math.PI * num6 / 180.0;
      double num8 = num3 * Math.Cos(num7);
      double num9 = num3 * Math.Sin(num7);
      double num10 = num8 + finalSize.Width / 2.0 - num4;
      double num11 = finalSize.Height / 2.0;
      double num12 = num9 + num11 - num5;
      UIElement uiElement = child;
      double x = num10;
      double y = num12;
      desiredSize = child.DesiredSize;
      double width = desiredSize.Width;
      desiredSize = child.DesiredSize;
      double height = desiredSize.Height;
      Rect finalRect = new Rect(x, y, width, height);
      uiElement.Arrange(finalRect);
    }
    return finalSize;
  }
}
