// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Row
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class Row : Panel
{
  private ColLayoutStatus _layoutStatus;
  private double _maxChildDesiredHeight;
  private double _fixedWidth;
  public static readonly DependencyProperty GutterProperty = DependencyProperty.Register(nameof (Gutter), typeof (double), typeof (Row), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure, (PropertyChangedCallback) null, new CoerceValueCallback(Row.OnGutterCoerce)), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosDoubleIncludeZero));

  private static object OnGutterCoerce(DependencyObject d, object basevalue)
  {
    return !ValidateHelper.IsInRangeOfPosDoubleIncludeZero(basevalue) ? (object) 0.0 : basevalue;
  }

  public double Gutter
  {
    get => (double) this.GetValue(Row.GutterProperty);
    set => this.SetValue(Row.GutterProperty, (object) value);
  }

  protected override Size MeasureOverride(Size constraint)
  {
    double gutter = this.Gutter;
    int num1 = 0;
    int num2 = 1;
    this._fixedWidth = 0.0;
    this._maxChildDesiredHeight = 0.0;
    List<Col> list = this.InternalChildren.OfType<Col>().ToList<Col>();
    foreach (Col col in list)
    {
      if (col.GetLayoutCellCount(this._layoutStatus) == 0 || col.IsFixed)
      {
        col.Measure(constraint);
        double childDesiredHeight = this._maxChildDesiredHeight;
        Size desiredSize = col.DesiredSize;
        double height = desiredSize.Height;
        this._maxChildDesiredHeight = Math.Max(childDesiredHeight, height);
        double fixedWidth = this._fixedWidth;
        desiredSize = col.DesiredSize;
        double num3 = desiredSize.Width + gutter;
        this._fixedWidth = fixedWidth + num3;
      }
    }
    double num4 = Math.Max(0.0, (constraint.Width - this._fixedWidth + gutter) / 24.0);
    foreach (Col col in list)
    {
      int layoutCellCount = col.GetLayoutCellCount(this._layoutStatus);
      if (layoutCellCount > 0 && !col.IsFixed)
      {
        num1 += layoutCellCount;
        col.Measure(new Size((double) layoutCellCount * num4 - gutter, constraint.Height));
        this._maxChildDesiredHeight = Math.Max(this._maxChildDesiredHeight, col.DesiredSize.Height);
        if (num1 > 24)
        {
          num1 = layoutCellCount;
          ++num2;
        }
      }
    }
    return new Size(0.0, this._maxChildDesiredHeight * (double) num2);
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    double gutter = this.Gutter;
    int num1 = 0;
    List<Col> list = this.InternalChildren.OfType<Col>().ToList<Col>();
    double num2 = Math.Max(0.0, (finalSize.Width - this._fixedWidth + gutter) / 24.0);
    Rect finalRect = new Rect(0.0, 0.0, 0.0, this._maxChildDesiredHeight);
    this._layoutStatus = ColLayout.GetLayoutStatus(finalSize.Width);
    foreach (Col col in list)
    {
      if (col.IsVisible)
      {
        int layoutCellCount = col.GetLayoutCellCount(this._layoutStatus);
        num1 += layoutCellCount;
        double num3 = layoutCellCount <= 0 || col.IsFixed ? col.DesiredSize.Width : (double) layoutCellCount * num2 - gutter;
        finalRect.Width = num3;
        finalRect.X += (double) col.Offset * num2;
        if (num1 > 24)
        {
          finalRect.X = 0.0;
          finalRect.Y += this._maxChildDesiredHeight;
          num1 = layoutCellCount;
        }
        col.Arrange(finalRect);
        finalRect.X += num3 + gutter;
      }
    }
    return finalSize;
  }
}
