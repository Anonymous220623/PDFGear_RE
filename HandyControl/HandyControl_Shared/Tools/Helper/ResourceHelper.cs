// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.ResourceHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Reflection;
using System.Windows;

#nullable disable
namespace HandyControl.Tools;

public class ResourceHelper
{
  private static ResourceDictionary _theme;

  public static T GetResource<T>(string key)
  {
    return Application.Current.TryFindResource((object) key) is T resource ? resource : default (T);
  }

  internal static T GetResourceInternal<T>(string key)
  {
    return ResourceHelper.GetTheme()[(object) key] is T obj ? obj : default (T);
  }

  public static ResourceDictionary GetSkin(Assembly assembly, string themePath, SkinType skin)
  {
    try
    {
      Uri uri = new Uri($"pack://application:,,,/{assembly.GetName().Name};component/{themePath}/Skin{skin}.xaml");
      return new ResourceDictionary() { Source = uri };
    }
    catch
    {
      return new ResourceDictionary()
      {
        Source = new Uri($"pack://application:,,,/{assembly.GetName().Name};component/{themePath}/Skin{(Enum) SkinType.Default}.xaml")
      };
    }
  }

  public static ResourceDictionary GetSkin(SkinType skin)
  {
    return new ResourceDictionary()
    {
      Source = new Uri($"pack://application:,,,/HandyControl;component/Themes/Skin{skin}.xaml")
    };
  }

  public static ResourceDictionary GetTheme()
  {
    return ResourceHelper._theme ?? (ResourceHelper._theme = ResourceHelper.GetStandaloneTheme());
  }

  public static ResourceDictionary GetStandaloneTheme()
  {
    return new ResourceDictionary()
    {
      Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
    };
  }
}
