// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.DoubleRowPanel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace pdfeditor.Controls.Menus;

public class DoubleRowPanel : Panel
{
  public static readonly DependencyProperty RowSpaceProperty = DependencyProperty.Register(nameof (RowSpace), typeof (double), typeof (DoubleRowPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
  public static readonly DependencyProperty ColumnSpaceProperty = DependencyProperty.Register(nameof (ColumnSpace), typeof (double), typeof (DoubleRowPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
  public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(nameof (Padding), typeof (Thickness), typeof (DoubleRowPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

  public double RowSpace
  {
    get => (double) this.GetValue(DoubleRowPanel.RowSpaceProperty);
    set => this.SetValue(DoubleRowPanel.RowSpaceProperty, (object) value);
  }

  public double ColumnSpace
  {
    get => (double) this.GetValue(DoubleRowPanel.ColumnSpaceProperty);
    set => this.SetValue(DoubleRowPanel.ColumnSpaceProperty, (object) value);
  }

  public Thickness Padding
  {
    get => (Thickness) this.GetValue(DoubleRowPanel.PaddingProperty);
    set => this.SetValue(DoubleRowPanel.PaddingProperty, (object) value);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    double val1 = 0.0;
    double val2_1 = 0.0;
    double val2_2 = 0.0;
    double val2_3 = 0.0;
    Thickness padding = this.Padding;
    double columnSpace = this.ColumnSpace;
    double rowSpace = this.RowSpace;
    Size availableSize1 = availableSize;
    if (!double.IsInfinity(availableSize.Height))
    {
      double val2_4 = (availableSize.Height - rowSpace - padding.Top - padding.Bottom) / 2.0;
      availableSize1 = new Size(Math.Max(0.0, availableSize.Width - this.Padding.Left - this.Padding.Right), Math.Max(0.0, val2_4));
    }
    int num1 = (int) Math.Ceiling((double) this.InternalChildren.Count / 2.0);
    for (int index = 0; index < this.InternalChildren.Count; ++index)
    {
      UIElement internalChild = this.InternalChildren[index];
      internalChild.Measure(availableSize1);
      Size desiredSize;
      if (index < num1)
      {
        double num2 = val1;
        desiredSize = internalChild.DesiredSize;
        double num3 = desiredSize.Width + columnSpace;
        val1 = num2 + num3;
        desiredSize = internalChild.DesiredSize;
        val2_1 = Math.Max(desiredSize.Height, val2_1);
      }
      else
      {
        double num4 = val2_2;
        desiredSize = internalChild.DesiredSize;
        double num5 = desiredSize.Width + columnSpace;
        val2_2 = num4 + num5;
        desiredSize = internalChild.DesiredSize;
        val2_3 = Math.Max(desiredSize.Height, val2_3);
      }
    }
    return new Size(double.IsNaN(this.Width) ? Math.Max(val1, val2_2) : this.Width, double.IsNaN(this.Height) ? val2_1 + val2_3 + padding.Top + padding.Bottom + rowSpace : this.Height);
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    Thickness padding = this.Padding;
    double columnSpace = this.ColumnSpace;
    double rowSpace = this.RowSpace;
    double left = padding.Left;
    double y = padding.Top;
    double height = (finalSize.Height - rowSpace - padding.Top - padding.Bottom) / 2.0;
    int length = (int) Math.Ceiling((double) this.InternalChildren.Count / 2.0);
    double[] numArray1 = new double[length];
    Size desiredSize;
    for (int index1 = 0; index1 < length; ++index1)
    {
      if (index1 + length < this.InternalChildren.Count)
      {
        double[] numArray2 = numArray1;
        int index2 = index1;
        desiredSize = this.InternalChildren[index1].DesiredSize;
        double width1 = desiredSize.Width;
        desiredSize = this.InternalChildren[index1 + length].DesiredSize;
        double width2 = desiredSize.Width;
        double num = Math.Max(width1, width2);
        numArray2[index2] = num;
      }
      else
      {
        double[] numArray3 = numArray1;
        int index3 = index1;
        desiredSize = this.InternalChildren[index1].DesiredSize;
        double width = desiredSize.Width;
        numArray3[index3] = width;
      }
    }
    for (int index = 0; index < this.InternalChildren.Count; ++index)
    {
      UIElement internalChild = this.InternalChildren[index];
      if (index == length)
      {
        left = padding.Left;
        y = padding.Top + height + rowSpace;
      }
      if (index < length)
      {
        internalChild.Arrange(new Rect(left, y, numArray1[index], height));
        left += numArray1[index] + columnSpace;
      }
      else
      {
        internalChild.Arrange(new Rect(left, y, numArray1[index - length], height));
        left += numArray1[index - length] + columnSpace;
      }
    }
    return finalSize;
  }
}
