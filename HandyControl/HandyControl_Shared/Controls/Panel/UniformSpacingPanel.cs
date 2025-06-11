// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.UniformSpacingPanel
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Expression.Drawing;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class UniformSpacingPanel : Panel
{
  private Orientation _orientation;
  public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(typeof (UniformSpacingPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(UniformSpacingPanel.OnOrientationChanged)));
  public static readonly DependencyProperty ChildWrappingProperty = DependencyProperty.Register(nameof (ChildWrapping), typeof (VisualWrapping), typeof (UniformSpacingPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) VisualWrapping.NoWrap, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(nameof (Spacing), typeof (double), typeof (UniformSpacingPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(UniformSpacingPanel.IsSpacingValid));
  public static readonly DependencyProperty HorizontalSpacingProperty = DependencyProperty.Register(nameof (HorizontalSpacing), typeof (double), typeof (UniformSpacingPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(UniformSpacingPanel.IsSpacingValid));
  public static readonly DependencyProperty VerticalSpacingProperty = DependencyProperty.Register(nameof (VerticalSpacing), typeof (double), typeof (UniformSpacingPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(UniformSpacingPanel.IsSpacingValid));
  public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(nameof (ItemWidth), typeof (double), typeof (UniformSpacingPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(UniformSpacingPanel.IsWidthHeightValid));
  public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(nameof (ItemHeight), typeof (double), typeof (UniformSpacingPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(UniformSpacingPanel.IsWidthHeightValid));
  public static readonly DependencyProperty ItemHorizontalAlignmentProperty = DependencyProperty.Register(nameof (ItemHorizontalAlignment), typeof (HorizontalAlignment?), typeof (UniformSpacingPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) HorizontalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty ItemVerticalAlignmentProperty = DependencyProperty.Register(nameof (ItemVerticalAlignment), typeof (VerticalAlignment?), typeof (UniformSpacingPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) VerticalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure));

  public UniformSpacingPanel() => this._orientation = Orientation.Horizontal;

  private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((UniformSpacingPanel) d)._orientation = (Orientation) e.NewValue;
  }

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(UniformSpacingPanel.OrientationProperty);
    set => this.SetValue(UniformSpacingPanel.OrientationProperty, (object) value);
  }

  public VisualWrapping ChildWrapping
  {
    get => (VisualWrapping) this.GetValue(UniformSpacingPanel.ChildWrappingProperty);
    set => this.SetValue(UniformSpacingPanel.ChildWrappingProperty, (object) value);
  }

  public double Spacing
  {
    get => (double) this.GetValue(UniformSpacingPanel.SpacingProperty);
    set => this.SetValue(UniformSpacingPanel.SpacingProperty, (object) value);
  }

  public double HorizontalSpacing
  {
    get => (double) this.GetValue(UniformSpacingPanel.HorizontalSpacingProperty);
    set => this.SetValue(UniformSpacingPanel.HorizontalSpacingProperty, (object) value);
  }

  public double VerticalSpacing
  {
    get => (double) this.GetValue(UniformSpacingPanel.VerticalSpacingProperty);
    set => this.SetValue(UniformSpacingPanel.VerticalSpacingProperty, (object) value);
  }

  [TypeConverter(typeof (LengthConverter))]
  public double ItemWidth
  {
    get => (double) this.GetValue(UniformSpacingPanel.ItemWidthProperty);
    set => this.SetValue(UniformSpacingPanel.ItemWidthProperty, (object) value);
  }

  [TypeConverter(typeof (LengthConverter))]
  public double ItemHeight
  {
    get => (double) this.GetValue(UniformSpacingPanel.ItemHeightProperty);
    set => this.SetValue(UniformSpacingPanel.ItemHeightProperty, (object) value);
  }

  public HorizontalAlignment? ItemHorizontalAlignment
  {
    get
    {
      return (HorizontalAlignment?) this.GetValue(UniformSpacingPanel.ItemHorizontalAlignmentProperty);
    }
    set => this.SetValue(UniformSpacingPanel.ItemHorizontalAlignmentProperty, (object) value);
  }

  public VerticalAlignment? ItemVerticalAlignment
  {
    get => (VerticalAlignment?) this.GetValue(UniformSpacingPanel.ItemVerticalAlignmentProperty);
    set => this.SetValue(UniformSpacingPanel.ItemVerticalAlignmentProperty, (object) value);
  }

  private static bool IsWidthHeightValid(object value)
  {
    double d = (double) value;
    if (double.IsNaN(d))
      return true;
    return d >= 0.0 && !double.IsPositiveInfinity(d);
  }

  private static bool IsSpacingValid(object value)
  {
    if (!(value is double d))
      return false;
    return double.IsNaN(d) || d > 0.0;
  }

  private void ArrangeWrapLine(
    double v,
    double lineV,
    int start,
    int end,
    bool useItemU,
    double itemU,
    double spacing)
  {
    double num1 = 0.0;
    bool flag = this._orientation == Orientation.Horizontal;
    UIElementCollection internalChildren = this.InternalChildren;
    for (int index = start; index < end; ++index)
    {
      UIElement uiElement = internalChildren[index];
      if (uiElement != null)
      {
        PanelUvSize panelUvSize = new PanelUvSize(this._orientation, uiElement.DesiredSize);
        double num2 = useItemU ? itemU : panelUvSize.U;
        uiElement.Arrange(flag ? new Rect(num1, v, num2, lineV) : new Rect(v, num1, lineV, num2));
        if (num2 > 0.0)
          num1 += num2 + spacing;
      }
    }
  }

  private void ArrangeLine(double lineV, bool useItemU, double itemU, double spacing)
  {
    double num1 = 0.0;
    bool flag = this._orientation == Orientation.Horizontal;
    UIElementCollection internalChildren = this.InternalChildren;
    for (int index = 0; index < internalChildren.Count; ++index)
    {
      UIElement uiElement = internalChildren[index];
      if (uiElement != null)
      {
        PanelUvSize panelUvSize = new PanelUvSize(this._orientation, uiElement.DesiredSize);
        double num2 = useItemU ? itemU : panelUvSize.U;
        uiElement.Arrange(flag ? new Rect(num1, 0.0, num2, lineV) : new Rect(0.0, num1, lineV, num2));
        if (num2 > 0.0)
          num1 += num2 + spacing;
      }
    }
  }

  protected override Size MeasureOverride(Size constraint)
  {
    PanelUvSize panelUvSize1 = new PanelUvSize(this._orientation);
    PanelUvSize panelUvSize2 = new PanelUvSize(this._orientation);
    PanelUvSize panelUvSize3 = new PanelUvSize(this._orientation, constraint);
    double itemWidth = this.ItemWidth;
    double itemHeight = this.ItemHeight;
    bool flag1 = !double.IsNaN(itemWidth);
    bool flag2 = !double.IsNaN(itemHeight);
    int childWrapping = (int) this.ChildWrapping;
    HorizontalAlignment? horizontalAlignment = this.ItemHorizontalAlignment;
    VerticalAlignment? verticalAlignment = this.ItemVerticalAlignment;
    bool hasValue1 = horizontalAlignment.HasValue;
    bool hasValue2 = this.ItemVerticalAlignment.HasValue;
    PanelUvSize spacingSize = this.GetSpacingSize();
    Size availableSize1 = new Size(flag1 ? itemWidth : constraint.Width, flag2 ? itemHeight : constraint.Height);
    UIElementCollection internalChildren = this.InternalChildren;
    bool flag3 = true;
    if (childWrapping == 1)
    {
      int index = 0;
      for (int count = internalChildren.Count; index < count; ++index)
      {
        UIElement uiElement = internalChildren[index];
        if (uiElement != null)
        {
          if (hasValue1)
            uiElement.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, (object) horizontalAlignment);
          if (hasValue2)
            uiElement.SetCurrentValue(FrameworkElement.VerticalAlignmentProperty, (object) verticalAlignment);
          uiElement.Measure(availableSize1);
          PanelUvSize panelUvSize4;
          ref PanelUvSize local = ref panelUvSize4;
          int orientation = (int) this._orientation;
          Size desiredSize;
          double width;
          if (!flag1)
          {
            desiredSize = uiElement.DesiredSize;
            width = desiredSize.Width;
          }
          else
            width = itemWidth;
          double height;
          if (!flag2)
          {
            desiredSize = uiElement.DesiredSize;
            height = desiredSize.Height;
          }
          else
            height = itemHeight;
          local = new PanelUvSize((Orientation) orientation, width, height);
          if (MathHelper.GreaterThan(panelUvSize1.U + panelUvSize4.U + spacingSize.U, panelUvSize3.U))
          {
            panelUvSize2.U = Math.Max(panelUvSize1.U, panelUvSize2.U);
            panelUvSize2.V += panelUvSize1.V + spacingSize.V;
            panelUvSize1 = panelUvSize4;
            if (MathHelper.GreaterThan(panelUvSize4.U, panelUvSize3.U))
            {
              panelUvSize2.U = Math.Max(panelUvSize4.U, panelUvSize2.U);
              panelUvSize2.V += panelUvSize4.V + spacingSize.V;
              panelUvSize1 = new PanelUvSize(this._orientation);
            }
          }
          else
          {
            panelUvSize1.U += flag3 ? panelUvSize4.U : panelUvSize4.U + spacingSize.U;
            panelUvSize1.V = Math.Max(panelUvSize4.V, panelUvSize1.V);
            flag3 = false;
          }
        }
      }
    }
    else
    {
      Size availableSize2 = constraint;
      if (this._orientation == Orientation.Horizontal)
        availableSize2.Width = double.PositiveInfinity;
      else
        availableSize2.Height = double.PositiveInfinity;
      int index = 0;
      for (int count = internalChildren.Count; index < count; ++index)
      {
        UIElement uiElement = internalChildren[index];
        if (uiElement != null)
        {
          if (hasValue1)
            uiElement.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, (object) horizontalAlignment);
          if (hasValue2)
            uiElement.SetCurrentValue(FrameworkElement.VerticalAlignmentProperty, (object) verticalAlignment);
          uiElement.Measure(availableSize2);
          PanelUvSize panelUvSize5;
          ref PanelUvSize local = ref panelUvSize5;
          int orientation = (int) this._orientation;
          Size desiredSize;
          double width;
          if (!flag1)
          {
            desiredSize = uiElement.DesiredSize;
            width = desiredSize.Width;
          }
          else
            width = itemWidth;
          double height;
          if (!flag2)
          {
            desiredSize = uiElement.DesiredSize;
            height = desiredSize.Height;
          }
          else
            height = itemHeight;
          local = new PanelUvSize((Orientation) orientation, width, height);
          panelUvSize1.U += flag3 ? panelUvSize5.U : panelUvSize5.U + spacingSize.U;
          panelUvSize1.V = Math.Max(panelUvSize5.V, panelUvSize1.V);
          flag3 = false;
        }
      }
    }
    panelUvSize2.U = Math.Max(panelUvSize1.U, panelUvSize2.U);
    panelUvSize2.V += panelUvSize1.V;
    return new Size(panelUvSize2.Width, panelUvSize2.Height);
  }

  private PanelUvSize GetSpacingSize()
  {
    double spacing = this.Spacing;
    if (!double.IsNaN(spacing))
      return new PanelUvSize(this._orientation, spacing, spacing);
    double num1 = this.HorizontalSpacing;
    if (double.IsNaN(num1))
      num1 = 0.0;
    double num2 = this.VerticalSpacing;
    if (double.IsNaN(num2))
      num2 = 0.0;
    return new PanelUvSize(this._orientation, num1, num2);
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    int start = 0;
    double itemWidth = this.ItemWidth;
    double itemHeight = this.ItemHeight;
    double v = 0.0;
    double itemU = this._orientation == Orientation.Horizontal ? itemWidth : itemHeight;
    PanelUvSize panelUvSize1 = new PanelUvSize(this._orientation);
    PanelUvSize panelUvSize2 = new PanelUvSize(this._orientation, finalSize);
    bool flag1 = !double.IsNaN(itemWidth);
    bool flag2 = !double.IsNaN(itemHeight);
    bool useItemU = this._orientation == Orientation.Horizontal ? flag1 : flag2;
    int childWrapping = (int) this.ChildWrapping;
    PanelUvSize spacingSize = this.GetSpacingSize();
    UIElementCollection internalChildren = this.InternalChildren;
    bool flag3 = true;
    if (childWrapping == 1)
    {
      int num = 0;
      for (int count = internalChildren.Count; num < count; ++num)
      {
        UIElement uiElement = internalChildren[num];
        if (uiElement != null)
        {
          PanelUvSize panelUvSize3 = new PanelUvSize(this._orientation, flag1 ? itemWidth : uiElement.DesiredSize.Width, flag2 ? itemHeight : uiElement.DesiredSize.Height);
          if (MathHelper.GreaterThan(panelUvSize1.U + (flag3 ? panelUvSize3.U : panelUvSize3.U + spacingSize.U), panelUvSize2.U))
          {
            this.ArrangeWrapLine(v, panelUvSize1.V, start, num, useItemU, itemU, spacingSize.U);
            v += panelUvSize1.V + spacingSize.V;
            panelUvSize1 = panelUvSize3;
            start = num;
          }
          else
          {
            panelUvSize1.U += flag3 ? panelUvSize3.U : panelUvSize3.U + spacingSize.U;
            panelUvSize1.V = Math.Max(panelUvSize3.V, panelUvSize1.V);
          }
          flag3 = false;
        }
      }
      if (start < internalChildren.Count)
        this.ArrangeWrapLine(v, panelUvSize1.V, start, internalChildren.Count, useItemU, itemU, spacingSize.U);
    }
    else
      this.ArrangeLine(panelUvSize2.V, useItemU, itemU, spacingSize.U);
    return finalSize;
  }
}
