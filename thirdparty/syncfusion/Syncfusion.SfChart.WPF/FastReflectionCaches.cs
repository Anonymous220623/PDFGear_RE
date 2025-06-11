// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastReflectionCaches
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Reflection;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class FastReflectionCaches
{
  static FastReflectionCaches()
  {
    FastReflectionCaches.MethodInvokerCache = (IFastReflectionCache<MethodInfo, IMethodInvoker>) new Syncfusion.UI.Xaml.Charts.MethodInvokerCache();
    FastReflectionCaches.PropertyAccessorCache = (IFastReflectionCache<PropertyInfo, IPropertyAccessor>) new Syncfusion.UI.Xaml.Charts.PropertyAccessorCache();
  }

  public static IFastReflectionCache<MethodInfo, IMethodInvoker> MethodInvokerCache { get; set; }

  public static IFastReflectionCache<PropertyInfo, IPropertyAccessor> PropertyAccessorCache { get; set; }
}
