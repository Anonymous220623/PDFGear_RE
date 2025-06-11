// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartDataUtils
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartDataUtils
{
  private static CultureInfo invariantCulture = CultureInfo.InvariantCulture;

  internal static object GetPropertyDescriptor(object obj, string path)
  {
    IPropertyAccessor propertyAccessor = (IPropertyAccessor) null;
    if (path.Contains(".") || path.Contains("["))
    {
      if (path.Contains("."))
      {
        string[] strArray = path.Split('.');
        int index = 0;
        object instance = obj;
        for (; index != strArray.Length; ++index)
        {
          PropertyInfo propertyInfo = ChartDataUtils.GetPropertyInfo(instance, strArray[index]);
          if (propertyInfo != (PropertyInfo) null)
            propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);
          object propertyDescriptor = propertyAccessor.GetValue(instance);
          if (index == strArray.Length - 1)
            return propertyDescriptor;
        }
      }
      else if (path.Contains("["))
      {
        int int32 = Convert.ToInt32(path.Substring(path.IndexOf('[') + 1, path.IndexOf(']') - path.IndexOf('[') - 1));
        string path1 = path.Replace(path.Substring(path.IndexOf('[')), string.Empty);
        PropertyInfo propertyInfo = ChartDataUtils.GetPropertyInfo(obj, path1);
        if (propertyInfo != (PropertyInfo) null)
          propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);
        if (propertyAccessor.GetValue(obj) is IList list && list.Count > int32)
          return list[int32];
      }
      return (object) null;
    }
    if (obj.GetType() == typeof (DictionaryEntry) || obj.GetType().ToString().Contains("KeyValuePair"))
    {
      PropertyInfo propertyInfo1 = ChartDataUtils.GetPropertyInfo(obj, "Value");
      if (propertyInfo1 != (PropertyInfo) null)
        propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo1);
      object instance = propertyAccessor.GetValue(obj);
      if (instance != null && path != "Key")
      {
        PropertyInfo propertyInfo2 = ChartDataUtils.GetPropertyInfo(instance, path);
        if (propertyInfo2 != (PropertyInfo) null)
          propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo2);
        return propertyAccessor.GetValue(instance);
      }
      PropertyInfo propertyInfo3 = ChartDataUtils.GetPropertyInfo(obj, path);
      if (propertyInfo3 != (PropertyInfo) null)
        propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo3);
      return propertyAccessor.GetValue(obj);
    }
    PropertyInfo propertyInfo4 = ChartDataUtils.GetPropertyInfo(obj, path);
    if (propertyInfo4 != (PropertyInfo) null)
      propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo4);
    return propertyAccessor.GetValue(obj);
  }

  public static object GetObjectByPath(object obj, string path)
  {
    if (string.IsNullOrEmpty(path))
      return obj;
    try
    {
      return ChartDataUtils.GetPropertyDescriptor(obj, path);
    }
    catch
    {
      return (object) null;
    }
  }

  public static int ConvertPathObjectToPositionValue(object obj)
  {
    try
    {
      return Convert.ToInt32(obj);
    }
    catch
    {
      return int.MinValue;
    }
  }

  public static double GetPositionalPathValue(object obj, string path)
  {
    return (double) ChartDataUtils.ConvertPathObjectToPositionValue(ChartDataUtils.GetObjectByPath(obj, path));
  }

  public static PropertyInfo GetPropertyInfo(object obj, string path)
  {
    return obj.GetType().GetRuntimeProperty(path);
  }

  internal static double ObjectToDouble(object obj)
  {
    if (obj is DateTime dateTime)
      return dateTime.ToOADate();
    double result = 0.0;
    double.TryParse(obj.ToString(), out result);
    return result;
  }

  internal static PointCollection GetTooltipPolygonPoints(
    Rect rect,
    double seriesTipHeight,
    bool isReversed,
    ChartAlignment horizontalAlignment,
    ChartAlignment verticalAlignment)
  {
    double x = rect.X;
    double y = rect.Y;
    double width = rect.Width;
    double height = rect.Height;
    double num1;
    if (verticalAlignment == ChartAlignment.Center && horizontalAlignment != ChartAlignment.Center || horizontalAlignment == ChartAlignment.Center && verticalAlignment != ChartAlignment.Center)
    {
      num1 = 2.0 * seriesTipHeight / Math.Sqrt(3.0) / 2.0;
    }
    else
    {
      double num2 = height / 3.0;
      num1 = num2 > 25.0 ? num2 * 2.0 / 3.0 : num2;
    }
    PointCollection tooltipPolygonPoints = new PointCollection();
    tooltipPolygonPoints.Add(new Point(0.0, 0.0));
    if (horizontalAlignment == ChartAlignment.Far)
    {
      tooltipPolygonPoints.Add(new Point(0.0, height / 2.0 - num1));
      if (!isReversed || verticalAlignment == ChartAlignment.Center)
      {
        switch (verticalAlignment)
        {
          case ChartAlignment.Near:
            tooltipPolygonPoints.Add(new Point(-seriesTipHeight, y - 4.0));
            break;
          case ChartAlignment.Far:
            tooltipPolygonPoints.Add(new Point(-seriesTipHeight, y + 4.0));
            break;
          default:
            tooltipPolygonPoints.Add(new Point(-seriesTipHeight, y));
            break;
        }
      }
      tooltipPolygonPoints.Add(new Point(0.0, height / 2.0 + num1));
    }
    tooltipPolygonPoints.Add(new Point(0.0, height));
    if (verticalAlignment == ChartAlignment.Near)
    {
      tooltipPolygonPoints.Add(new Point(width / 2.0 - num1, height));
      if (isReversed || horizontalAlignment == ChartAlignment.Center)
      {
        switch (horizontalAlignment)
        {
          case ChartAlignment.Near:
            tooltipPolygonPoints.Add(new Point(x - 4.0, seriesTipHeight + height));
            break;
          case ChartAlignment.Far:
            tooltipPolygonPoints.Add(new Point(x, seriesTipHeight + height));
            break;
          default:
            tooltipPolygonPoints.Add(new Point(x, seriesTipHeight + height));
            break;
        }
      }
      tooltipPolygonPoints.Add(new Point(width / 2.0 + num1, height));
    }
    tooltipPolygonPoints.Add(new Point(width, height));
    if (horizontalAlignment == ChartAlignment.Near)
    {
      tooltipPolygonPoints.Add(new Point(width, height / 2.0 + num1));
      if (!isReversed || verticalAlignment == ChartAlignment.Center)
      {
        switch (verticalAlignment)
        {
          case ChartAlignment.Near:
            tooltipPolygonPoints.Add(new Point(seriesTipHeight + width, y - 4.0));
            break;
          case ChartAlignment.Far:
            tooltipPolygonPoints.Add(new Point(seriesTipHeight + width, y + 4.0));
            break;
          default:
            tooltipPolygonPoints.Add(new Point(seriesTipHeight + width, y));
            break;
        }
      }
      tooltipPolygonPoints.Add(new Point(width, height / 2.0 - num1));
    }
    tooltipPolygonPoints.Add(new Point(width, 0.0));
    if (verticalAlignment == ChartAlignment.Far)
    {
      tooltipPolygonPoints.Add(new Point(width / 2.0 + num1, 0.0));
      if (isReversed || horizontalAlignment == ChartAlignment.Center)
      {
        switch (horizontalAlignment)
        {
          case ChartAlignment.Near:
            tooltipPolygonPoints.Add(new Point(x - 4.0, -seriesTipHeight));
            break;
          case ChartAlignment.Far:
            tooltipPolygonPoints.Add(new Point(x, -seriesTipHeight));
            break;
          default:
            tooltipPolygonPoints.Add(new Point(x, -seriesTipHeight));
            break;
        }
      }
      tooltipPolygonPoints.Add(new Point(width / 2.0 - num1, 0.0));
    }
    tooltipPolygonPoints.Add(new Point(0.0, 0.0));
    return tooltipPolygonPoints;
  }

  internal static string GenerateTooltipPolygon(
    Size labelSize,
    HorizontalPosition horizontal,
    VerticalPosition vertical)
  {
    double num = 3.0;
    double rotateAngle = 0.0;
    double isLargeArc = 0.0;
    double sweepDirection = 1.0;
    Point drawPoint = new Point(num, 0.0);
    double width = labelSize.Width;
    double height = labelSize.Height;
    string str1 = "M0," + num.ToString((IFormatProvider) ChartDataUtils.invariantCulture);
    for (int index = 0; index < 4; ++index)
    {
      if (index == 0)
      {
        string arc = ChartDataUtils.CreateArc(num, rotateAngle, isLargeArc, sweepDirection, drawPoint);
        string str2 = str1 + arc + " ";
        str1 = vertical != VerticalPosition.Top ? $"{str2}L{(width - num).ToString((IFormatProvider) ChartDataUtils.invariantCulture)},0" : str2 + ChartDataUtils.drawNosePointer(horizontal, vertical, width, height);
      }
      else if (index == 1)
      {
        drawPoint = new Point(width, num);
        string arc = ChartDataUtils.CreateArc(num, rotateAngle, isLargeArc, sweepDirection, drawPoint);
        str1 = $"{str1 + arc + " "}L{width.ToString((IFormatProvider) ChartDataUtils.invariantCulture)},{(height - num).ToString((IFormatProvider) ChartDataUtils.invariantCulture)}";
      }
      else if (index == 2)
      {
        drawPoint = new Point(width - num, height);
        string arc = ChartDataUtils.CreateArc(num, rotateAngle, isLargeArc, sweepDirection, drawPoint);
        string str3 = str1 + arc + " ";
        if (vertical == VerticalPosition.Bottom)
          str1 = str3 + ChartDataUtils.drawNosePointer(horizontal, vertical, width, height);
        else
          str1 = $"{str3}L{num.ToString((IFormatProvider) ChartDataUtils.invariantCulture)},{height.ToString((IFormatProvider) ChartDataUtils.invariantCulture)}";
      }
      else if (index == 3)
      {
        drawPoint = new Point(0.0, height - num);
        string arc = ChartDataUtils.CreateArc(num, rotateAngle, isLargeArc, sweepDirection, drawPoint);
        str1 = str1 + arc + " ";
      }
    }
    return str1 + "z";
  }

  private static string CreateArc(
    double arcSize,
    double rotateAngle,
    double isLargeArc,
    double sweepDirection,
    Point drawPoint)
  {
    return $" A{arcSize.ToString((IFormatProvider) ChartDataUtils.invariantCulture)},{arcSize.ToString((IFormatProvider) ChartDataUtils.invariantCulture)} {rotateAngle.ToString((IFormatProvider) ChartDataUtils.invariantCulture)} {isLargeArc.ToString((IFormatProvider) ChartDataUtils.invariantCulture)} {sweepDirection.ToString((IFormatProvider) ChartDataUtils.invariantCulture)} {drawPoint.X.ToString((IFormatProvider) ChartDataUtils.invariantCulture)},{drawPoint.Y.ToString((IFormatProvider) ChartDataUtils.invariantCulture)}";
  }

  internal static string drawNosePointer(
    HorizontalPosition horizontal,
    VerticalPosition vertical,
    double width,
    double height)
  {
    double depth = 4.0;
    double depthWidth = depth + 1.5;
    double num1 = width / 2.0 + depthWidth;
    double num2 = width / 2.0 - depthWidth;
    double num3 = height / 2.0;
    double num4 = height / 2.0;
    double num5 = (num1 + num2) / 2.0;
    string str = "";
    switch (vertical)
    {
      case VerticalPosition.Top:
        str = ChartDataUtils.DrawTopPolygon(width, depthWidth, depth, horizontal);
        break;
      case VerticalPosition.Bottom:
        str = ChartDataUtils.DrawBottomPolygon(width, height, depthWidth, depth, horizontal);
        break;
    }
    return str;
  }

  private static string DrawBottomPolygon(
    double width,
    double height,
    double depthWidth,
    double depth,
    HorizontalPosition horizontal)
  {
    string str = "";
    Point point1 = new Point();
    Point point2 = new Point();
    Point point3 = new Point();
    Point point4 = new Point();
    double num;
    switch (horizontal)
    {
      case HorizontalPosition.Left:
        double x1 = 3.0 + depthWidth * 2.0;
        double x2 = 4.0;
        num = (x1 + x2) / 2.0;
        point1 = new Point(x1, height);
        point2 = new Point(0.0, height + depth);
        point3 = new Point(x2, height);
        point4 = new Point(3.0, height);
        break;
      case HorizontalPosition.Right:
        double x3 = width - 4.0;
        double x4 = width - depthWidth * 2.0;
        num = (x3 + x4) / 2.0;
        point1 = new Point(x3, height);
        point2 = new Point(width, height + depth);
        point3 = new Point(x4, height);
        point4 = new Point(3.0, height);
        break;
      case HorizontalPosition.Center:
        double x5 = width / 2.0 + depthWidth;
        double x6 = width / 2.0 - depthWidth;
        double x7 = (x5 + x6) / 2.0;
        point1 = new Point(x5, height);
        point2 = new Point(x7, height + depth);
        point3 = new Point(x6, height);
        point4 = new Point(3.0, height);
        break;
    }
    return $"{$"{$"{$"{str}L{point1.X.ToString((IFormatProvider) ChartDataUtils.invariantCulture)},{point1.Y.ToString((IFormatProvider) ChartDataUtils.invariantCulture)}"}L{point2.X.ToString((IFormatProvider) ChartDataUtils.invariantCulture)},{point2.Y.ToString((IFormatProvider) ChartDataUtils.invariantCulture)}"}L{point3.X.ToString((IFormatProvider) ChartDataUtils.invariantCulture)},{point3.Y.ToString((IFormatProvider) ChartDataUtils.invariantCulture)}"}L{point4.X.ToString((IFormatProvider) ChartDataUtils.invariantCulture)},{point4.Y.ToString((IFormatProvider) ChartDataUtils.invariantCulture)}";
  }

  private static string DrawTopPolygon(
    double width,
    double depthWidth,
    double depth,
    HorizontalPosition horizontal)
  {
    string str = "";
    Point point1 = new Point();
    Point point2 = new Point();
    Point point3 = new Point();
    Point point4 = new Point();
    double num;
    switch (horizontal)
    {
      case HorizontalPosition.Left:
        double x1 = 3.0 + depthWidth * 2.0;
        double x2 = 4.0;
        num = (x1 + x2) / 2.0;
        point1 = new Point(x2, 0.0);
        point2 = new Point(0.0, -depth);
        point3 = new Point(x1, 0.0);
        point4 = new Point(width - 3.0, 0.0);
        break;
      case HorizontalPosition.Right:
        double x3 = width - 4.0;
        double x4 = width - depthWidth * 2.0;
        num = (x3 + x4) / 2.0;
        point1 = new Point(x4, 0.0);
        point2 = new Point(width, -depth);
        point3 = new Point(x3, 0.0);
        point4 = new Point(width - 3.0, 0.0);
        break;
      case HorizontalPosition.Center:
        double x5 = width / 2.0 + depthWidth;
        double x6 = width / 2.0 - depthWidth;
        double x7 = (x5 + x6) / 2.0;
        point1 = new Point(x6, 0.0);
        point2 = new Point(x7, -depth);
        point3 = new Point(x5, 0.0);
        point4 = new Point(width - 3.0, 0.0);
        break;
    }
    return $"{$"{$"{$"{str}L{point1.X.ToString((IFormatProvider) ChartDataUtils.invariantCulture)},{point1.Y.ToString((IFormatProvider) ChartDataUtils.invariantCulture)}"}L{point2.X.ToString((IFormatProvider) ChartDataUtils.invariantCulture)},{point2.Y.ToString((IFormatProvider) ChartDataUtils.invariantCulture)}"}L{point3.X.ToString((IFormatProvider) ChartDataUtils.invariantCulture)},{point3.Y.ToString((IFormatProvider) ChartDataUtils.invariantCulture)}"}L{point4.X.ToString((IFormatProvider) ChartDataUtils.invariantCulture)},{point4.Y.ToString((IFormatProvider) ChartDataUtils.invariantCulture)}";
  }
}
