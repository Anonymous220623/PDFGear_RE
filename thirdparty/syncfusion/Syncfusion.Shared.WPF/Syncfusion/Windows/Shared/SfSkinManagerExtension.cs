// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.SfSkinManagerExtension
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
public static class SfSkinManagerExtension
{
  private static Type sfskinmanagertype;
  private static MethodInfo getThemeMethod;
  private static MethodInfo setThemeMethod;
  private static MethodInfo getThemeNameMethod;
  private static MethodInfo getBaseThemeNameMethod;
  private static MethodInfo getThemeDesignMethod;
  private static MethodInfo setSizeModeMethod;
  private static MethodInfo getSizeModeMethod;
  private static MethodInfo getShowAcrylicBackgroundMethod;

  public static string GetThemeName(DependencyObject obj)
  {
    if (SfSkinManagerExtension.sfskinmanagertype == (Type) null)
      SfSkinManagerExtension.sfskinmanagertype = Type.GetType("Syncfusion.SfSkinManager.SfSkinManager, Syncfusion.SfSkinManager.WPF");
    if (SfSkinManagerExtension.sfskinmanagertype != (Type) null)
    {
      if (SfSkinManagerExtension.getThemeNameMethod == (MethodInfo) null)
        SfSkinManagerExtension.getThemeNameMethod = SfSkinManagerExtension.sfskinmanagertype.GetMethod(nameof (GetThemeName), BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
      if (SfSkinManagerExtension.getThemeNameMethod != (MethodInfo) null)
        return (string) SfSkinManagerExtension.getThemeNameMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[1]
        {
          (object) obj
        }, CultureInfo.CurrentUICulture);
    }
    return (string) null;
  }

  public static string GetBaseThemeName(DependencyObject obj)
  {
    if (SfSkinManagerExtension.sfskinmanagertype == (Type) null)
      SfSkinManagerExtension.sfskinmanagertype = Type.GetType("Syncfusion.SfSkinManager.SfSkinManager, Syncfusion.SfSkinManager.WPF");
    if (SfSkinManagerExtension.sfskinmanagertype != (Type) null)
    {
      if (SfSkinManagerExtension.getBaseThemeNameMethod == (MethodInfo) null)
        SfSkinManagerExtension.getBaseThemeNameMethod = SfSkinManagerExtension.sfskinmanagertype.GetMethod(nameof (GetBaseThemeName), BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
      if (SfSkinManagerExtension.getBaseThemeNameMethod != (MethodInfo) null)
        return (string) SfSkinManagerExtension.getBaseThemeNameMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[1]
        {
          (object) obj
        }, CultureInfo.CurrentUICulture);
    }
    return (string) null;
  }

  public static string GetThemeDesign(DependencyObject obj)
  {
    if (SfSkinManagerExtension.sfskinmanagertype == (Type) null)
      SfSkinManagerExtension.sfskinmanagertype = Type.GetType("Syncfusion.SfSkinManager.SfSkinManager, Syncfusion.SfSkinManager.WPF");
    if (SfSkinManagerExtension.sfskinmanagertype != (Type) null)
    {
      if (SfSkinManagerExtension.getThemeDesignMethod == (MethodInfo) null)
        SfSkinManagerExtension.getThemeDesignMethod = SfSkinManagerExtension.sfskinmanagertype.GetMethod(nameof (GetThemeDesign), BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
      if (SfSkinManagerExtension.getThemeDesignMethod != (MethodInfo) null)
        return (string) SfSkinManagerExtension.getThemeDesignMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[1]
        {
          (object) obj
        }, CultureInfo.CurrentUICulture);
    }
    return (string) null;
  }

  public static void SetTheme(DependencyObject obj, object value)
  {
    if (SfSkinManagerExtension.sfskinmanagertype == (Type) null)
      SfSkinManagerExtension.sfskinmanagertype = Type.GetType("Syncfusion.SfSkinManager.SfSkinManager, Syncfusion.SfSkinManager.WPF");
    if (SfSkinManagerExtension.sfskinmanagertype == (Type) null)
      return;
    if (SfSkinManagerExtension.setThemeMethod == (MethodInfo) null)
      SfSkinManagerExtension.setThemeMethod = SfSkinManagerExtension.sfskinmanagertype.GetMethod(nameof (SetTheme), BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
    if (SfSkinManagerExtension.setThemeMethod == (MethodInfo) null)
      return;
    SfSkinManagerExtension.setThemeMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[2]
    {
      (object) obj,
      value
    }, CultureInfo.CurrentUICulture);
  }

  public static void SetTheme(DependencyObject sourceobject, DependencyObject childobject)
  {
    if (SfSkinManagerExtension.sfskinmanagertype == (Type) null)
      SfSkinManagerExtension.sfskinmanagertype = Type.GetType("Syncfusion.SfSkinManager.SfSkinManager, Syncfusion.SfSkinManager.WPF");
    if (SfSkinManagerExtension.sfskinmanagertype == (Type) null)
      return;
    if (SfSkinManagerExtension.getThemeMethod == (MethodInfo) null)
      SfSkinManagerExtension.getThemeMethod = SfSkinManagerExtension.sfskinmanagertype.GetMethod("GetTheme", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
    if (SfSkinManagerExtension.setThemeMethod == (MethodInfo) null)
      SfSkinManagerExtension.setThemeMethod = SfSkinManagerExtension.sfskinmanagertype.GetMethod(nameof (SetTheme), BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
    if (SfSkinManagerExtension.getThemeMethod == (MethodInfo) null || SfSkinManagerExtension.setThemeMethod == (MethodInfo) null)
      return;
    object obj = SfSkinManagerExtension.getThemeMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[1]
    {
      (object) sourceobject
    }, CultureInfo.CurrentUICulture);
    SfSkinManagerExtension.setThemeMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[2]
    {
      (object) childobject,
      obj
    }, CultureInfo.CurrentUICulture);
    SfSkinManagerExtension.SetSizeMode(sourceobject, childobject);
  }

  public static string GetSizeMode(DependencyObject obj)
  {
    if (SfSkinManagerExtension.sfskinmanagertype == (Type) null)
      SfSkinManagerExtension.sfskinmanagertype = Type.GetType("Syncfusion.SfSkinManager.SfSkinManager, Syncfusion.SfSkinManager.WPF");
    if (SfSkinManagerExtension.sfskinmanagertype != (Type) null)
    {
      if (SfSkinManagerExtension.getSizeModeMethod == (MethodInfo) null)
        SfSkinManagerExtension.getSizeModeMethod = SfSkinManagerExtension.sfskinmanagertype.GetMethod("GetSizeModeValue", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
      if (SfSkinManagerExtension.getSizeModeMethod != (MethodInfo) null)
        return (string) SfSkinManagerExtension.getSizeModeMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[1]
        {
          (object) obj
        }, CultureInfo.CurrentUICulture);
    }
    return (string) null;
  }

  public static bool GetShowAcrylicBackground(DependencyObject obj)
  {
    if (SfSkinManagerExtension.sfskinmanagertype == (Type) null)
      SfSkinManagerExtension.sfskinmanagertype = Type.GetType("Syncfusion.SfSkinManager.SfSkinManager, Syncfusion.SfSkinManager.WPF");
    if (SfSkinManagerExtension.sfskinmanagertype != (Type) null)
    {
      if (SfSkinManagerExtension.getShowAcrylicBackgroundMethod == (MethodInfo) null)
        SfSkinManagerExtension.getShowAcrylicBackgroundMethod = SfSkinManagerExtension.sfskinmanagertype.GetMethod(nameof (GetShowAcrylicBackground), BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
      if (SfSkinManagerExtension.getShowAcrylicBackgroundMethod != (MethodInfo) null)
        return (bool) SfSkinManagerExtension.getShowAcrylicBackgroundMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[1]
        {
          (object) obj
        }, CultureInfo.CurrentUICulture);
    }
    return false;
  }

  public static void SetSizeMode(DependencyObject obj, object value)
  {
    if (SfSkinManagerExtension.sfskinmanagertype == (Type) null)
      SfSkinManagerExtension.sfskinmanagertype = Type.GetType("Syncfusion.SfSkinManager.SfSkinManager, Syncfusion.SfSkinManager.WPF");
    if (SfSkinManagerExtension.sfskinmanagertype == (Type) null)
      return;
    if (SfSkinManagerExtension.setSizeModeMethod == (MethodInfo) null)
      SfSkinManagerExtension.setSizeModeMethod = SfSkinManagerExtension.sfskinmanagertype.GetMethod("SetSizeModeValue", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
    if (SfSkinManagerExtension.setSizeModeMethod == (MethodInfo) null)
      return;
    SfSkinManagerExtension.setSizeModeMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[2]
    {
      (object) obj,
      (object) value.ToString()
    }, CultureInfo.CurrentUICulture);
  }

  public static void SetSizeMode(DependencyObject sourceobject, DependencyObject childobject)
  {
    if (SfSkinManagerExtension.sfskinmanagertype == (Type) null)
      SfSkinManagerExtension.sfskinmanagertype = Type.GetType("Syncfusion.SfSkinManager.SfSkinManager, Syncfusion.SfSkinManager.WPF");
    if (SfSkinManagerExtension.sfskinmanagertype == (Type) null)
      return;
    if (SfSkinManagerExtension.getSizeModeMethod == (MethodInfo) null)
      SfSkinManagerExtension.getSizeModeMethod = SfSkinManagerExtension.sfskinmanagertype.GetMethod("GetSizeModeValue", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
    if (SfSkinManagerExtension.setSizeModeMethod == (MethodInfo) null)
      SfSkinManagerExtension.setSizeModeMethod = SfSkinManagerExtension.sfskinmanagertype.GetMethod("SetSizeModeValue", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
    if (SfSkinManagerExtension.getSizeModeMethod == (MethodInfo) null || SfSkinManagerExtension.setSizeModeMethod == (MethodInfo) null)
      return;
    object obj = SfSkinManagerExtension.getSizeModeMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[1]
    {
      (object) sourceobject
    }, CultureInfo.CurrentUICulture);
    SfSkinManagerExtension.setSizeModeMethod.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[2]
    {
      (object) childobject,
      obj
    }, CultureInfo.CurrentUICulture);
  }
}
