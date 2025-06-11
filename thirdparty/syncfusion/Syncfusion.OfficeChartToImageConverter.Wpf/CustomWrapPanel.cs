// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.CustomWrapPanel
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class CustomWrapPanel : WrapPanel
{
  internal static readonly DependencyProperty IsTextWrappedProperty = DependencyProperty.RegisterAttached("IsTextWrapped", typeof (bool), typeof (CustomWrapPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsRender));
  private Tuple<bool, double, double> gapWidthHeight;
  private int noOflines = 1;
  private int noOfColumns = 1;
  internal double baseLineHeight;
  internal bool useBaseLineHeight;

  protected override Size MeasureOverride(Size constraint)
  {
    Size size = new Size(!double.IsNaN(this.ItemWidth) ? this.ItemWidth : constraint.Width, !double.IsNaN(this.ItemHeight) ? this.ItemHeight : constraint.Height);
    UIElementCollection internalChildren = this.InternalChildren;
    List<Size> sizeList;
    List<Size> source = sizeList = new List<Size>(internalChildren.Count);
    int index = 0;
    for (int count = internalChildren.Count; index < count; ++index)
    {
      UIElement uiElement = internalChildren[index];
      if (uiElement != null)
      {
        uiElement.Measure(new Size(double.MaxValue, double.MaxValue));
        source.Add(uiElement.DesiredSize);
      }
    }
    if (source != null && source.Count != 0)
    {
      double num1 = source.Max<Size>((Func<Size, double>) (x => x.Height));
      double num2 = source.Sum<Size>((Func<Size, double>) (x => x.Height));
      double num3 = source.Max<Size>((Func<Size, double>) (x => x.Width));
      double num4 = source.Sum<Size>((Func<Size, double>) (x => x.Width));
      double num5 = num3;
      double num6 = num1;
      if (num3 >= constraint.Width)
      {
        this.noOflines = (int) Math.Truncate(Math.Ceiling(num3 / constraint.Width));
        this.noOfColumns = 1;
        double num7 = num1 * (double) this.noOflines;
        double num8 = (this.noOflines == 1 ? (num2 >= constraint.Height ? 1 : 0) : (num1 * (double) this.noOflines >= constraint.Height ? 1 : 0)) == 0 ? (this.noOflines == 1 ? constraint.Height - num2 : constraint.Height - num1 * (double) this.noOflines * (double) source.Count) : 0.0;
        this.noOflines *= source.Count;
        this.ItemWidth = constraint.Width;
        this.ItemHeight = num7 + num8 / (double) source.Count;
      }
      else if (num4 >= constraint.Width)
      {
        this.noOfColumns = (int) Math.Truncate(Math.Floor(constraint.Width / num3));
        this.noOflines = (int) Math.Ceiling((double) source.Count / (double) this.noOfColumns);
        double num9 = !(bool) this.GetValue(CustomWrapPanel.IsTextWrappedProperty) ? constraint.Width - (double) this.noOfColumns * num3 : 0.0;
        double num10 = (double) this.noOflines * num1 <= constraint.Height ? constraint.Height - (double) this.noOflines * num1 : 0.0;
        if (num6 < constraint.Height)
        {
          if (this.noOfColumns == 1)
            this.gapWidthHeight = new Tuple<bool, double, double>(false, num9 / 2.0, num10);
          else if ((num5 + 5.0) * (double) this.noOfColumns < constraint.Width)
          {
            double left = num9 / (double) (this.noOfColumns + 1);
            this.ItemWidth = num5 + left;
            this.Margin = new Thickness(left, 0.0, 0.0, 0.0);
          }
          else
            this.ItemWidth = num5 + num9 / (double) this.noOfColumns;
          this.ItemHeight = num6 + num10 / (double) this.noOflines;
          this.baseLineHeight = this.ItemHeight;
          if (this.ItemHeight * (double) this.noOflines > constraint.Height)
          {
            double properHeight = this.GetProperHeight(internalChildren);
            if (properHeight != 0.0)
            {
              this.baseLineHeight = properHeight;
              this.useBaseLineHeight = true;
            }
          }
        }
      }
      else
      {
        this.noOflines = 1;
        this.noOfColumns = source.Count;
        double num11 = num1 < constraint.Height ? constraint.Height - num1 : 0.0;
        if (num3 * (double) this.noOfColumns <= constraint.Width)
        {
          double num12 = constraint.Width - num3 * (double) this.noOfColumns;
          this.ItemWidth = num5 + num12 / (double) this.noOfColumns;
          this.ItemHeight = num6 + num11 / (double) this.noOflines;
        }
        else
          this.gapWidthHeight = new Tuple<bool, double, double>(true, (constraint.Width - num4) / (double) (internalChildren.Count + 1), num11 / 2.0);
      }
    }
    return base.MeasureOverride(constraint);
  }

  private double GetProperHeight(UIElementCollection uIElements)
  {
    double properHeight = 0.0;
    foreach (UIElement uIelement in uIElements)
    {
      if (VisualTreeHelper.GetChildrenCount((DependencyObject) uIelement) > 0 && VisualTreeHelper.GetChild((DependencyObject) uIelement, 0) is WrapPanel child1 && child1.Children.Count > 1 && child1.Children[1] is Label child2 && child2.Content != null && child2.Content is AccessText content && VisualTreeHelper.GetChildrenCount((DependencyObject) content) > 0 && VisualTreeHelper.GetChild((DependencyObject) content, 0) is TextBlock child3 && child3.BaselineOffset > properHeight)
        properHeight = child3.BaselineOffset;
    }
    return properHeight;
  }

  protected override Size ArrangeOverride(Size constraint)
  {
    UIElementCollection internalChildren = this.InternalChildren;
    if (this.gapWidthHeight != null)
    {
      if (this.gapWidthHeight.Item1)
      {
        double num1 = 0.0;
        double num2 = this.gapWidthHeight.Item2 / 2.0;
        for (int index = 0; index < internalChildren.Count; ++index)
        {
          UIElement uiElement = internalChildren[index];
          if (uiElement != null)
          {
            uiElement.Arrange(new Rect(num1 + num2, this.gapWidthHeight.Item3, uiElement.DesiredSize.Width, uiElement.DesiredSize.Height));
            num1 += uiElement.DesiredSize.Width + this.gapWidthHeight.Item2;
            if (num1 > this.Width)
              break;
          }
        }
      }
      else
      {
        double y = 0.0;
        for (int index = 0; index < internalChildren.Count; ++index)
        {
          UIElement uiElement = internalChildren[index];
          if (uiElement != null)
          {
            if (y + this.baseLineHeight <= this.Height)
            {
              uiElement.Arrange(new Rect(this.gapWidthHeight.Item2, y, uiElement.DesiredSize.Width, this.ItemHeight));
              y += this.baseLineHeight;
            }
            else
              break;
          }
        }
      }
      return constraint;
    }
    if (this.noOfColumns <= 1 || this.noOflines <= 1 || this.ItemHeight * (double) this.noOflines <= constraint.Height && !this.useBaseLineHeight)
      return base.ArrangeOverride(constraint);
    double num3 = (constraint.Width - (double) this.noOfColumns * this.ItemWidth) / 2.0;
    double y1 = 0.0;
    int index1 = 0;
    int num4 = 0;
    for (; index1 < internalChildren.Count; ++index1)
    {
      if (num4 == this.noOfColumns)
      {
        num4 = 0;
        y1 += this.baseLineHeight;
      }
      double x = num3 + (double) num4 * this.ItemWidth;
      internalChildren[index1]?.Arrange(new Rect(x, y1, this.ItemWidth, this.ItemHeight));
      ++num4;
    }
    return constraint;
  }
}
