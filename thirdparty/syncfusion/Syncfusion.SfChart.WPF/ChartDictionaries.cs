// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartDictionaries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class ChartDictionaries
{
  [ThreadStatic]
  private static ResourceDictionary genericLegendDictionary;
  [ThreadStatic]
  private static ResourceDictionary genericSymbolDictionary;
  [ThreadStatic]
  private static ResourceDictionary genericCommonDictionary;

  internal static ResourceDictionary GenericLegendDictionary
  {
    get
    {
      if (ChartDictionaries.genericLegendDictionary == null)
        ChartDictionaries.genericLegendDictionary = new ResourceDictionary()
        {
          Source = new Uri("/Syncfusion.SfChart.WPF;component/Themes/Generic.Legend.xaml", UriKind.Relative)
        };
      return ChartDictionaries.genericLegendDictionary;
    }
  }

  internal static ResourceDictionary GenericSymbolDictionary
  {
    get
    {
      if (ChartDictionaries.genericSymbolDictionary == null)
        ChartDictionaries.genericSymbolDictionary = new ResourceDictionary()
        {
          Source = new Uri("/Syncfusion.SfChart.WPF;component/Themes/Generic.Symbol.xaml", UriKind.Relative)
        };
      return ChartDictionaries.genericSymbolDictionary;
    }
  }

  internal static ResourceDictionary GenericCommonDictionary
  {
    get
    {
      if (ChartDictionaries.genericCommonDictionary == null)
        ChartDictionaries.genericCommonDictionary = new ResourceDictionary()
        {
          Source = new Uri("/Syncfusion.SfChart.WPF;component/Themes/Generic.Common.xaml", UriKind.Relative)
        };
      return ChartDictionaries.genericCommonDictionary;
    }
  }
}
