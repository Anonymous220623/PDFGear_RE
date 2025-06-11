// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartSkinManagerExtension
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Globalization;
using System.Reflection;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class ChartSkinManagerExtension
{
  private static Type sfskinmanagertype;
  private static MethodInfo getThemeMethod;
  private static MethodInfo setThemeMethod;
  private static MethodInfo getBaseThemeNameMethod;
  private static MethodInfo setSizeModeMethod;
  private static MethodInfo getSizeModeMethod;

  internal static string GetBaseThemeName(DependencyObject obj)
  {
    if (ChartSkinManagerExtension.sfskinmanagertype == (Type) null)
      ChartSkinManagerExtension.sfskinmanagertype = Type.GetType("Syncfusion.SfSkinManager.SfSkinManager, Syncfusion.SfSkinManager.WPF");
    if (ChartSkinManagerExtension.sfskinmanagertype != (Type) null)
    {
      if (ChartSkinManagerExtension.getBaseThemeNameMethod == (MethodInfo) null)
        ChartSkinManagerExtension.getBaseThemeNameMethod = ChartSkinManagerExtension.sfskinmanagertype.GetMethod(nameof (GetBaseThemeName), BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
      if (ChartSkinManagerExtension.getBaseThemeNameMethod != (MethodInfo) null)
        return (string) ChartSkinManagerExtension.getBaseThemeNameMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[1]
        {
          (object) obj
        }, CultureInfo.CurrentUICulture);
    }
    return (string) null;
  }

  internal static void SetTheme(DependencyObject sourceobject, DependencyObject childobject)
  {
    if (ChartSkinManagerExtension.sfskinmanagertype == (Type) null)
      ChartSkinManagerExtension.sfskinmanagertype = Type.GetType("Syncfusion.SfSkinManager.SfSkinManager, Syncfusion.SfSkinManager.WPF");
    if (ChartSkinManagerExtension.sfskinmanagertype == (Type) null)
      return;
    if (ChartSkinManagerExtension.getThemeMethod == (MethodInfo) null)
      ChartSkinManagerExtension.getThemeMethod = ChartSkinManagerExtension.sfskinmanagertype.GetMethod("GetTheme", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
    if (ChartSkinManagerExtension.setThemeMethod == (MethodInfo) null)
      ChartSkinManagerExtension.setThemeMethod = ChartSkinManagerExtension.sfskinmanagertype.GetMethod(nameof (SetTheme), BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
    if (ChartSkinManagerExtension.getThemeMethod == (MethodInfo) null || ChartSkinManagerExtension.setThemeMethod == (MethodInfo) null)
      return;
    object obj = ChartSkinManagerExtension.getThemeMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[1]
    {
      (object) sourceobject
    }, CultureInfo.CurrentUICulture);
    ChartSkinManagerExtension.setThemeMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[2]
    {
      (object) childobject,
      obj
    }, CultureInfo.CurrentUICulture);
    ChartSkinManagerExtension.SetSizeMode(sourceobject, childobject);
  }

  internal static void SetSizeMode(DependencyObject sourceobject, DependencyObject childobject)
  {
    if (ChartSkinManagerExtension.sfskinmanagertype == (Type) null)
      ChartSkinManagerExtension.sfskinmanagertype = Type.GetType("Syncfusion.SfSkinManager.SfSkinManager, Syncfusion.SfSkinManager.WPF");
    if (ChartSkinManagerExtension.sfskinmanagertype == (Type) null)
      return;
    if (ChartSkinManagerExtension.getSizeModeMethod == (MethodInfo) null)
      ChartSkinManagerExtension.getSizeModeMethod = ChartSkinManagerExtension.sfskinmanagertype.GetMethod("GetSizeModeValue", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
    if (ChartSkinManagerExtension.setSizeModeMethod == (MethodInfo) null)
      ChartSkinManagerExtension.setSizeModeMethod = ChartSkinManagerExtension.sfskinmanagertype.GetMethod("SetSizeModeValue", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
    if (ChartSkinManagerExtension.getSizeModeMethod == (MethodInfo) null || ChartSkinManagerExtension.setSizeModeMethod == (MethodInfo) null)
      return;
    object obj = ChartSkinManagerExtension.getSizeModeMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[1]
    {
      (object) sourceobject
    }, CultureInfo.CurrentUICulture);
    ChartSkinManagerExtension.setSizeModeMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[2]
    {
      (object) childobject,
      obj
    }, CultureInfo.CurrentUICulture);
  }
}
