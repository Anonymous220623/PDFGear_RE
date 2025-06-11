// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelChartToImageConverter.ViewModel
// Assembly: Syncfusion.ExcelChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 8A92A829-7139-4C93-8632-144655877EB3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelChartToImageConverter.Wpf.dll

using System;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace Syncfusion.ExcelChartToImageConverter;

internal class ViewModel
{
  internal ViewModel(int count)
  {
    this.Products = new ObservableCollection<ChartPoint>();
    for (int index = 0; index < count; ++index)
      this.Products.Add(new ChartPoint());
  }

  internal ViewModel() => this.Products = new ObservableCollection<ChartPoint>();

  internal ObservableCollection<ChartPoint> Products { get; set; }

  internal static string GetXorYValuesAsString(
    ObservableCollection<ChartPoint> points,
    bool isX,
    bool isNaturalLog)
  {
    string str1 = isX ? points.Aggregate<ChartPoint, string>("{", (Func<string, ChartPoint, string>) ((str, s) =>
    {
      string str2 = str;
      // ISSUE: variable of a boxed type
      __Boxed<double> local = (ValueType) (isNaturalLog ? Math.Log(s.Value) : s.Value);
      return str = $"{str2}{(object) local},";
    })) : points.Aggregate<ChartPoint, string>("{", (Func<string, ChartPoint, string>) ((str, s) =>
    {
      string str3 = str;
      // ISSUE: variable of a boxed type
      __Boxed<double> local = (ValueType) (isNaturalLog ? Math.Log(s.Value) : s.Value);
      return str = $"{str3}{(object) local},";
    }));
    return str1.Remove(str1.Length - 1) + "}";
  }
}
