// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastReflectionFactories
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Reflection;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class FastReflectionFactories
{
  static FastReflectionFactories()
  {
    FastReflectionFactories.MethodInvokerFactory = (IFastReflectionFactory<MethodInfo, IMethodInvoker>) new Syncfusion.UI.Xaml.Charts.MethodInvokerFactory();
  }

  public static IFastReflectionFactory<MethodInfo, IMethodInvoker> MethodInvokerFactory { get; set; }
}
