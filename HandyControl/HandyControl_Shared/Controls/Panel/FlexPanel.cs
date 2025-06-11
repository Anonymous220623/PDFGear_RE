// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.FlexPanel
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class FlexPanel : Panel
{
  private FlexPanel.UVSize _uvConstraint;
  private int _lineCount;
  private readonly List<FlexPanel.FlexItemInfo> _orderList = new List<FlexPanel.FlexItemInfo>();
  public static readonly DependencyProperty OrderProperty = DependencyProperty.RegisterAttached("Order", typeof (int), typeof (FlexPanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Int0Box, new PropertyChangedCallback(FlexPanel.OnItemPropertyChanged)));
  public static readonly DependencyProperty FlexGrowProperty = DependencyProperty.RegisterAttached("FlexGrow", typeof (double), typeof (FlexPanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, new PropertyChangedCallback(FlexPanel.OnItemPropertyChanged)), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosDoubleIncludeZero));
  public static readonly DependencyProperty FlexShrinkProperty = DependencyProperty.RegisterAttached("FlexShrink", typeof (double), typeof (FlexPanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double1Box, new PropertyChangedCallback(FlexPanel.OnItemPropertyChanged)), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosDoubleIncludeZero));
  public static readonly DependencyProperty FlexBasisProperty = DependencyProperty.RegisterAttached("FlexBasis", typeof (double), typeof (FlexPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) double.NaN, new PropertyChangedCallback(FlexPanel.OnItemPropertyChanged)));
  public static readonly DependencyProperty AlignSelfProperty = DependencyProperty.RegisterAttached("AlignSelf", typeof (FlexItemAlignment), typeof (FlexPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) FlexItemAlignment.Auto, new PropertyChangedCallback(FlexPanel.OnItemPropertyChanged)));
  public static readonly DependencyProperty FlexDirectionProperty = DependencyProperty.Register(nameof (FlexDirection), typeof (FlexDirection), typeof (FlexPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) FlexDirection.Row, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty FlexWrapProperty = DependencyProperty.Register(nameof (FlexWrap), typeof (FlexWrap), typeof (FlexPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) FlexWrap.NoWrap, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty JustifyContentProperty = DependencyProperty.Register(nameof (JustifyContent), typeof (FlexContentJustify), typeof (FlexPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) FlexContentJustify.FlexStart, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty AlignItemsProperty = DependencyProperty.Register(nameof (AlignItems), typeof (FlexItemsAlignment), typeof (FlexPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) FlexItemsAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty AlignContentProperty = DependencyProperty.Register(nameof (AlignContent), typeof (FlexContentAlignment), typeof (FlexPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) FlexContentAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure));

  public static void SetOrder(DependencyObject element, int value)
  {
    element.SetValue(FlexPanel.OrderProperty, (object) value);
  }

  public static int GetOrder(DependencyObject element)
  {
    return (int) element.GetValue(FlexPanel.OrderProperty);
  }

  public static void SetFlexGrow(DependencyObject element, double value)
  {
    element.SetValue(FlexPanel.FlexGrowProperty, (object) value);
  }

  public static double GetFlexGrow(DependencyObject element)
  {
    return (double) element.GetValue(FlexPanel.FlexGrowProperty);
  }

  public static void SetFlexShrink(DependencyObject element, double value)
  {
    element.SetValue(FlexPanel.FlexShrinkProperty, (object) value);
  }

  public static double GetFlexShrink(DependencyObject element)
  {
    return (double) element.GetValue(FlexPanel.FlexShrinkProperty);
  }

  public static void SetFlexBasis(DependencyObject element, double value)
  {
    element.SetValue(FlexPanel.FlexBasisProperty, (object) value);
  }

  public static double GetFlexBasis(DependencyObject element)
  {
    return (double) element.GetValue(FlexPanel.FlexBasisProperty);
  }

  public static void SetAlignSelf(DependencyObject element, FlexItemAlignment value)
  {
    element.SetValue(FlexPanel.AlignSelfProperty, (object) value);
  }

  public static FlexItemAlignment GetAlignSelf(DependencyObject element)
  {
    return (FlexItemAlignment) element.GetValue(FlexPanel.AlignSelfProperty);
  }

  public FlexDirection FlexDirection
  {
    get => (FlexDirection) this.GetValue(FlexPanel.FlexDirectionProperty);
    set => this.SetValue(FlexPanel.FlexDirectionProperty, (object) value);
  }

  public FlexWrap FlexWrap
  {
    get => (FlexWrap) this.GetValue(FlexPanel.FlexWrapProperty);
    set => this.SetValue(FlexPanel.FlexWrapProperty, (object) value);
  }

  public FlexContentJustify JustifyContent
  {
    get => (FlexContentJustify) this.GetValue(FlexPanel.JustifyContentProperty);
    set => this.SetValue(FlexPanel.JustifyContentProperty, (object) value);
  }

  public FlexItemsAlignment AlignItems
  {
    get => (FlexItemsAlignment) this.GetValue(FlexPanel.AlignItemsProperty);
    set => this.SetValue(FlexPanel.AlignItemsProperty, (object) value);
  }

  public FlexContentAlignment AlignContent
  {
    get => (FlexContentAlignment) this.GetValue(FlexPanel.AlignContentProperty);
    set => this.SetValue(FlexPanel.AlignContentProperty, (object) value);
  }

  private static void OnItemPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is UIElement reference) || !(VisualTreeHelper.GetParent((DependencyObject) reference) is FlexPanel parent))
      return;
    parent.InvalidateMeasure();
  }

  protected override Size MeasureOverride(Size constraint)
  {
    FlexDirection flexDirection = this.FlexDirection;
    FlexWrap flexWrap = this.FlexWrap;
    FlexPanel.UVSize uvSize1 = new FlexPanel.UVSize(flexDirection);
    FlexPanel.UVSize uvSize2 = new FlexPanel.UVSize(flexDirection);
    this._uvConstraint = new FlexPanel.UVSize(flexDirection, constraint);
    Size availableSize = new Size(constraint.Width, constraint.Height);
    this._lineCount = 1;
    UIElementCollection internalChildren = this.InternalChildren;
    this._orderList.Clear();
    for (int index = 0; index < internalChildren.Count; ++index)
    {
      UIElement element = internalChildren[index];
      if (element != null)
        this._orderList.Add(new FlexPanel.FlexItemInfo(index, FlexPanel.GetOrder((DependencyObject) element)));
    }
    this._orderList.Sort();
    for (int index = 0; index < internalChildren.Count; ++index)
    {
      UIElement element = internalChildren[this._orderList[index].Index];
      if (element != null)
      {
        double flexBasis = FlexPanel.GetFlexBasis((DependencyObject) element);
        if (!flexBasis.IsNaN())
          element.SetCurrentValue(FrameworkElement.WidthProperty, (object) flexBasis);
        element.Measure(availableSize);
        FlexPanel.UVSize uvSize3 = new FlexPanel.UVSize(flexDirection, element.DesiredSize);
        if (flexWrap == FlexWrap.NoWrap)
        {
          uvSize1.U += uvSize3.U;
          uvSize1.V = Math.Max(uvSize3.V, uvSize1.V);
        }
        else if (MathHelper.GreaterThan(uvSize1.U + uvSize3.U, this._uvConstraint.U))
        {
          uvSize2.U = Math.Max(uvSize1.U, uvSize2.U);
          uvSize2.V += uvSize1.V;
          uvSize1 = uvSize3;
          ++this._lineCount;
          if (MathHelper.GreaterThan(uvSize3.U, this._uvConstraint.U))
          {
            uvSize2.U = Math.Max(uvSize3.U, uvSize2.U);
            uvSize2.V += uvSize3.V;
            uvSize1 = new FlexPanel.UVSize(flexDirection);
            ++this._lineCount;
          }
        }
        else
        {
          uvSize1.U += uvSize3.U;
          uvSize1.V = Math.Max(uvSize3.V, uvSize1.V);
        }
      }
    }
    uvSize2.U = Math.Max(uvSize1.U, uvSize2.U);
    uvSize2.V += uvSize1.V;
    return new Size(uvSize2.Width, uvSize2.Height);
  }

  protected override Size ArrangeOverride(Size arrangeSize)
  {
    FlexDirection flexDirection = this.FlexDirection;
    FlexWrap flexWrap = this.FlexWrap;
    FlexContentAlignment alignContent = this.AlignContent;
    FlexPanel.UVSize uvSize1 = new FlexPanel.UVSize(flexDirection, arrangeSize);
    if (MathHelper.IsZero(uvSize1.U) || MathHelper.IsZero(uvSize1.V))
      return arrangeSize;
    UIElementCollection internalChildren = this.InternalChildren;
    int index1 = 0;
    FlexPanel.UVSize[] uvSizeArray = new FlexPanel.UVSize[this._lineCount];
    uvSizeArray[0] = new FlexPanel.UVSize(flexDirection);
    int[] numArray1 = new int[this._lineCount];
    for (int index2 = 0; index2 < this._lineCount; ++index2)
      numArray1[index2] = int.MaxValue;
    for (int index3 = 0; index3 < internalChildren.Count; ++index3)
    {
      UIElement uiElement = internalChildren[this._orderList[index3].Index];
      if (uiElement != null)
      {
        FlexPanel.UVSize uvSize2 = new FlexPanel.UVSize(flexDirection, uiElement.DesiredSize);
        if (flexWrap == FlexWrap.NoWrap)
        {
          uvSizeArray[index1].U += uvSize2.U;
          uvSizeArray[index1].V = Math.Max(uvSize2.V, uvSizeArray[index1].V);
        }
        else if (MathHelper.GreaterThan(uvSizeArray[index1].U + uvSize2.U, uvSize1.U))
        {
          numArray1[index1] = index3;
          ++index1;
          uvSizeArray[index1] = uvSize2;
          if (MathHelper.GreaterThan(uvSize2.U, uvSize1.U))
          {
            numArray1[index1] = index3;
            ++index1;
            uvSizeArray[index1] = new FlexPanel.UVSize(flexDirection);
          }
        }
        else
        {
          uvSizeArray[index1].U += uvSize2.U;
          uvSizeArray[index1].V = Math.Max(uvSize2.V, uvSizeArray[index1].V);
        }
      }
    }
    double num1 = Math.Min(this._uvConstraint.U / uvSize1.U, 1.0);
    int num2 = 0;
    int num3 = 0;
    int num4 = flexWrap == FlexWrap.WrapReverse ? -1 : 1;
    int num5 = flexWrap == FlexWrap.WrapReverse ? 1 : 0;
    double num6 = 0.0;
    double num7 = 0.0;
    double v = uvSize1.V;
    foreach (FlexPanel.UVSize uvSize3 in uvSizeArray)
      v -= uvSize3.V;
    double num8 = v;
    double[] numArray2 = new double[this._lineCount];
    switch (alignContent)
    {
      case FlexContentAlignment.Stretch:
        if (this._lineCount > 1)
        {
          num8 = v / (double) this._lineCount;
          for (int index4 = 0; index4 < this._lineCount; ++index4)
            numArray2[index4] = num8;
          num7 = flexWrap == FlexWrap.WrapReverse ? uvSize1.V - uvSizeArray[0].V - numArray2[0] : 0.0;
          break;
        }
        break;
      case FlexContentAlignment.FlexStart:
        num3 = flexWrap != FlexWrap.WrapReverse ? 1 : 0;
        if (this._lineCount > 1)
        {
          num7 = flexWrap == FlexWrap.WrapReverse ? uvSize1.V - uvSizeArray[0].V : 0.0;
          break;
        }
        num3 = 0;
        break;
      case FlexContentAlignment.FlexEnd:
        num3 = flexWrap == FlexWrap.WrapReverse ? 1 : 0;
        if (this._lineCount > 1)
        {
          num7 = flexWrap == FlexWrap.WrapReverse ? uvSize1.V - uvSizeArray[0].V - v : v;
          break;
        }
        num3 = 0;
        break;
      case FlexContentAlignment.Center:
        if (this._lineCount > 1)
        {
          num7 = flexWrap == FlexWrap.WrapReverse ? uvSize1.V - uvSizeArray[0].V - v * 0.5 : v * 0.5;
          break;
        }
        break;
      case FlexContentAlignment.SpaceBetween:
        if (this._lineCount > 1)
        {
          num8 = v / (double) (this._lineCount - 1);
          for (int index5 = 0; index5 < this._lineCount - 1; ++index5)
            numArray2[index5] = num8;
          num7 = flexWrap == FlexWrap.WrapReverse ? uvSize1.V - uvSizeArray[0].V : 0.0;
          break;
        }
        break;
      case FlexContentAlignment.SpaceAround:
        if (this._lineCount > 1)
        {
          num8 = v / (double) this._lineCount * 0.5;
          for (int index6 = 0; index6 < this._lineCount - 1; ++index6)
            numArray2[index6] = num8 * 2.0;
          num7 = flexWrap == FlexWrap.WrapReverse ? uvSize1.V - uvSizeArray[0].V - num8 : num8;
          break;
        }
        break;
    }
    int index7 = 0;
    FlexPanel.FlexLineInfo lineInfo;
    for (int index8 = 0; index8 < internalChildren.Count; ++index8)
    {
      UIElement uiElement = internalChildren[this._orderList[index8].Index];
      if (uiElement != null)
      {
        FlexPanel.UVSize uvSize4 = new FlexPanel.UVSize(flexDirection, uiElement.DesiredSize);
        if (flexWrap != FlexWrap.NoWrap && index8 >= numArray1[index7])
        {
          lineInfo = new FlexPanel.FlexLineInfo();
          lineInfo.ItemsU = num6;
          lineInfo.OffsetV = num7 + num8 * (double) num3;
          lineInfo.LineV = uvSizeArray[index7].V;
          lineInfo.LineFreeV = num8;
          lineInfo.LineU = uvSize1.U;
          lineInfo.ItemStartIndex = num2;
          lineInfo.ItemEndIndex = index8;
          lineInfo.ScaleU = num1;
          this.ArrangeLine(lineInfo);
          num7 += (numArray2[index7] + uvSizeArray[index7 + num5].V) * (double) num4;
          ++index7;
          num6 = 0.0;
          if (index8 >= numArray1[index7])
          {
            lineInfo = new FlexPanel.FlexLineInfo();
            lineInfo.ItemsU = num6;
            lineInfo.OffsetV = num7 + num8 * (double) num3;
            lineInfo.LineV = uvSizeArray[index7].V;
            lineInfo.LineFreeV = num8;
            lineInfo.LineU = uvSize1.U;
            lineInfo.ItemStartIndex = index8;
            lineInfo.ItemEndIndex = ++index8;
            lineInfo.ScaleU = num1;
            this.ArrangeLine(lineInfo);
            num7 += (numArray2[index7] + uvSizeArray[index7 + num5].V) * (double) num4;
            ++index7;
            num6 = 0.0;
          }
          num2 = index8;
        }
        num6 += uvSize4.U;
      }
    }
    if (num2 < internalChildren.Count)
    {
      lineInfo = new FlexPanel.FlexLineInfo();
      lineInfo.ItemsU = num6;
      lineInfo.OffsetV = num7 + num8 * (double) num3;
      lineInfo.LineV = uvSizeArray[index7].V;
      lineInfo.LineFreeV = num8;
      lineInfo.LineU = uvSize1.U;
      lineInfo.ItemStartIndex = num2;
      lineInfo.ItemEndIndex = internalChildren.Count;
      lineInfo.ScaleU = num1;
      this.ArrangeLine(lineInfo);
    }
    return arrangeSize;
  }

  private void ArrangeLine(FlexPanel.FlexLineInfo lineInfo)
  {
    FlexDirection flexDirection = this.FlexDirection;
    FlexWrap flexWrap = this.FlexWrap;
    FlexContentJustify justifyContent = this.JustifyContent;
    FlexItemsAlignment alignItems = this.AlignItems;
    bool flag1 = flexDirection == FlexDirection.Row || flexDirection == FlexDirection.RowReverse;
    bool flag2 = flexDirection == FlexDirection.RowReverse || flexDirection == FlexDirection.ColumnReverse;
    int length = lineInfo.ItemEndIndex - lineInfo.ItemStartIndex;
    UIElementCollection internalChildren = this.InternalChildren;
    double num1 = lineInfo.LineU - lineInfo.ItemsU;
    double num2 = this._uvConstraint.U - lineInfo.ItemsU;
    double num3 = 0.0;
    double num4;
    if (flag2)
    {
      double num5;
      switch (justifyContent)
      {
        case FlexContentJustify.FlexStart:
          num5 = lineInfo.LineU;
          break;
        case FlexContentJustify.FlexEnd:
          num5 = lineInfo.ItemsU;
          break;
        case FlexContentJustify.Center:
          num5 = (lineInfo.LineU + lineInfo.ItemsU) * 0.5;
          break;
        case FlexContentJustify.SpaceBetween:
          num5 = lineInfo.LineU;
          break;
        case FlexContentJustify.SpaceAround:
          num5 = lineInfo.LineU;
          break;
        default:
          num5 = num3;
          break;
      }
      num4 = num5;
    }
    else
    {
      double num6;
      switch (justifyContent)
      {
        case FlexContentJustify.FlexEnd:
          num6 = num1;
          break;
        case FlexContentJustify.Center:
          num6 = num1 * 0.5;
          break;
        default:
          num6 = num3;
          break;
      }
      num4 = num6;
    }
    double num7 = num4 * lineInfo.ScaleU;
    double[] numArray1 = new double[length];
    if (num2 > 0.0)
    {
      bool flag3 = true;
      double num8 = 0.0;
      for (int index = 0; index < length; ++index)
      {
        double flexGrow = FlexPanel.GetFlexGrow((DependencyObject) internalChildren[this._orderList[index].Index]);
        flag3 &= MathHelper.IsVerySmall(flexGrow);
        numArray1[index] = flexGrow;
        num8 += flexGrow;
      }
      if (!flag3)
      {
        double num9 = 0.0;
        if (num8 > 0.0)
        {
          num9 = num2 / num8;
          lineInfo.ScaleU = 1.0;
          num1 = 0.0;
        }
        for (int index = 0; index < length; ++index)
          numArray1[index] *= num9;
      }
      else
        numArray1 = new double[length];
    }
    double[] numArray2 = new double[length];
    if (num2 < 0.0)
    {
      bool flag4 = true;
      double num10 = 0.0;
      for (int index = 0; index < length; ++index)
      {
        double flexShrink = FlexPanel.GetFlexShrink((DependencyObject) internalChildren[this._orderList[index].Index]);
        flag4 &= MathHelper.IsVerySmall(flexShrink - 1.0);
        numArray2[index] = flexShrink;
        num10 += flexShrink;
      }
      if (!flag4)
      {
        double num11 = 0.0;
        if (num10 > 0.0)
        {
          num11 = num2 / num10;
          lineInfo.ScaleU = 1.0;
          num1 = 0.0;
        }
        for (int index = 0; index < length; ++index)
          numArray2[index] *= num11;
      }
      else
        numArray2 = new double[length];
    }
    double[] numArray3 = new double[length];
    if (num1 > 0.0)
    {
      switch (justifyContent)
      {
        case FlexContentJustify.SpaceBetween:
          double num12 = num1 / (double) (length - 1);
          for (int index = 1; index < length; ++index)
            numArray3[index] = num12;
          break;
        case FlexContentJustify.SpaceAround:
          double num13 = num1 / (double) length * 0.5;
          numArray3[0] = num13;
          for (int index = 1; index < length; ++index)
            numArray3[index] = num13 * 2.0;
          break;
      }
    }
    int itemStartIndex = lineInfo.ItemStartIndex;
    int index1 = 0;
    while (itemStartIndex < lineInfo.ItemEndIndex)
    {
      UIElement element = internalChildren[this._orderList[itemStartIndex].Index];
      if (element != null)
      {
        FlexPanel.UVSize uvSize;
        ref FlexPanel.UVSize local = ref uvSize;
        int direction = (int) flexDirection;
        Size desiredSize;
        Size size;
        if (!flag1)
        {
          desiredSize = element.DesiredSize;
          double width = desiredSize.Width;
          desiredSize = element.DesiredSize;
          double height = desiredSize.Height * lineInfo.ScaleU;
          size = new Size(width, height);
        }
        else
        {
          desiredSize = element.DesiredSize;
          double width = desiredSize.Width * lineInfo.ScaleU;
          desiredSize = element.DesiredSize;
          double height = desiredSize.Height;
          size = new Size(width, height);
        }
        local = new FlexPanel.UVSize((FlexDirection) direction, size);
        uvSize.U += numArray1[index1] + numArray2[index1];
        if (flag2)
          num7 = num7 - uvSize.U - numArray3[index1];
        else
          num7 += numArray3[index1];
        double offsetV = lineInfo.OffsetV;
        FlexItemAlignment alignSelf = FlexPanel.GetAlignSelf((DependencyObject) element);
        switch (alignSelf == FlexItemAlignment.Auto ? (int) alignItems : (int) alignSelf)
        {
          case 0:
            uvSize.V = this._lineCount != 1 || flexWrap != FlexWrap.NoWrap ? lineInfo.LineV : lineInfo.LineV + lineInfo.LineFreeV;
            break;
          case 2:
            offsetV += lineInfo.LineV - uvSize.V;
            break;
          case 3:
            offsetV += (lineInfo.LineV - uvSize.V) * 0.5;
            break;
        }
        element.Arrange(flag1 ? new Rect(num7, offsetV, uvSize.U, uvSize.V) : new Rect(offsetV, num7, uvSize.V, uvSize.U));
        if (!flag2)
          num7 += uvSize.U;
      }
      ++itemStartIndex;
      ++index1;
    }
  }

  private readonly struct FlexItemInfo(int index, int order) : IComparable<FlexPanel.FlexItemInfo>
  {
    private int Order { get; } = order;

    public int Index { get; } = index;

    public int CompareTo(FlexPanel.FlexItemInfo other)
    {
      int num = this.Order.CompareTo(other.Order);
      return num != 0 ? num : this.Index.CompareTo(other.Index);
    }
  }

  private struct FlexLineInfo
  {
    public double ItemsU { get; set; }

    public double OffsetV { get; set; }

    public double LineU { get; set; }

    public double LineV { get; set; }

    public double LineFreeV { get; set; }

    public int ItemStartIndex { get; set; }

    public int ItemEndIndex { get; set; }

    public double ScaleU { get; set; }
  }

  private struct UVSize
  {
    public UVSize(FlexDirection direction, Size size)
    {
      this.U = this.V = 0.0;
      this.FlexDirection = direction;
      this.Width = size.Width;
      this.Height = size.Height;
    }

    public UVSize(FlexDirection direction)
    {
      this.U = this.V = 0.0;
      this.FlexDirection = direction;
    }

    public double U { get; set; }

    public double V { get; set; }

    private FlexDirection FlexDirection { get; }

    public double Width
    {
      get
      {
        return this.FlexDirection != FlexDirection.Row && this.FlexDirection != FlexDirection.RowReverse ? this.V : this.U;
      }
      private set
      {
        if (this.FlexDirection == FlexDirection.Row || this.FlexDirection == FlexDirection.RowReverse)
          this.U = value;
        else
          this.V = value;
      }
    }

    public double Height
    {
      get
      {
        return this.FlexDirection != FlexDirection.Row && this.FlexDirection != FlexDirection.RowReverse ? this.U : this.V;
      }
      private set
      {
        if (this.FlexDirection == FlexDirection.Row || this.FlexDirection == FlexDirection.RowReverse)
          this.V = value;
        else
          this.U = value;
      }
    }
  }
}
