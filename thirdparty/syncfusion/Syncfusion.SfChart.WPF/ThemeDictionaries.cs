// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ThemeDictionaries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ThemeDictionaries : ResourceDictionary
{
  private ResourceDictionary lightThemeDictionary;
  private ResourceDictionary darkThemeDictionary;

  public ResourceDictionary LightThemeDictionary
  {
    get => this.lightThemeDictionary;
    set
    {
      this.lightThemeDictionary = value;
      if (ThemeDictionaries.IsDarkTheme || value == null)
        return;
      this.MergedDictionaries.Add(value);
    }
  }

  public ResourceDictionary DarkThemeDictionary
  {
    get => this.darkThemeDictionary;
    set
    {
      this.darkThemeDictionary = value;
      if (!ThemeDictionaries.IsDarkTheme || value == null)
        return;
      this.MergedDictionaries.Add(value);
    }
  }

  private static bool IsDarkTheme
  {
    get
    {
      return (Visibility) Application.Current.Resources[(object) "PhoneDarkThemeVisibility"] == Visibility.Visible;
    }
  }
}
