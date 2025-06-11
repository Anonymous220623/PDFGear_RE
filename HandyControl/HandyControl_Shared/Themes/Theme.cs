// Decompiled with JetBrains decompiler
// Type: HandyControl.Themes.Theme
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Helper;
using HandyControl.Tools.Interop;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#nullable disable
namespace HandyControl.Themes;

public class Theme : ResourceDictionary
{
  private SkinType _manualSkinType;
  private SkinType _skin;
  public static readonly DependencyProperty SkinProperty = DependencyProperty.RegisterAttached(nameof (Skin), typeof (SkinType), typeof (Theme), new PropertyMetadata((object) SkinType.Default, new PropertyChangedCallback(Theme.OnSkinChanged)));
  private bool _syncWithSystem;
  private System.Windows.Media.Color? _accentColor;
  private Uri _source;
  private SkinType _prevSkinType;
  private ResourceDictionary _precSkin;

  public Theme()
  {
    if (DesignerHelper.IsInDesignMode)
    {
      this.MergedDictionaries.Add(HandyControl.Tools.ResourceHelper.GetSkin(SkinType.Default));
      this.MergedDictionaries.Add(HandyControl.Tools.ResourceHelper.GetTheme());
    }
    else
      this.InitResource();
  }

  public virtual SkinType Skin
  {
    get => this._skin;
    set
    {
      if (this._skin == value)
        return;
      this._skin = value;
      this.UpdateSkin();
    }
  }

  private static void OnSkinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is FrameworkElement frameworkElement))
      return;
    SkinType newValue = (SkinType) e.NewValue;
    List<Theme> themes = new List<Theme>();
    Theme.GetAllThemes(frameworkElement.Resources, ref themes);
    if (themes.Count > 0)
    {
      foreach (Theme theme in themes)
        theme.Skin = newValue;
    }
    else
      frameworkElement.Resources.MergedDictionaries.Add((ResourceDictionary) new Theme()
      {
        Skin = newValue
      });
  }

  private static void GetAllThemes(ResourceDictionary resourceDictionary, ref List<Theme> themes)
  {
    if (resourceDictionary is Theme theme)
      themes.Add(theme);
    foreach (ResourceDictionary mergedDictionary in resourceDictionary.MergedDictionaries)
      Theme.GetAllThemes(mergedDictionary, ref themes);
  }

  public static void SetSkin(DependencyObject element, SkinType value)
  {
    element.SetValue(Theme.SkinProperty, (object) value);
  }

  public static SkinType GetSkin(DependencyObject element)
  {
    return (SkinType) element.GetValue(Theme.SkinProperty);
  }

  public bool SyncWithSystem
  {
    get => this._syncWithSystem;
    set
    {
      this._syncWithSystem = value;
      if (value)
      {
        this._manualSkinType = this._skin;
        this.SyncWithSystemTheme();
        SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(this.SystemEvents_UserPreferenceChanged);
      }
      else
      {
        SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(this.SystemEvents_UserPreferenceChanged);
        this._skin = this._manualSkinType;
        this.UpdateSkin();
      }
    }
  }

  private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
  {
    if (e.Category != UserPreferenceCategory.General)
      return;
    this.SyncWithSystemTheme();
  }

  private void SyncWithSystemTheme()
  {
    this._skin = SystemHelper.DetermineIfInLightThemeMode() ? SkinType.Default : SkinType.Dark;
    this.UpdateSkin();
  }

  public System.Windows.Media.Color? AccentColor
  {
    get => this._accentColor;
    set
    {
      this._accentColor = value;
      if (!value.HasValue)
        this._precSkin = (ResourceDictionary) null;
      this.UpdateSkin();
    }
  }

  public new Uri Source
  {
    get => !DesignerHelper.IsInDesignMode ? this._source : (Uri) null;
    set => this._source = value;
  }

  public string Name { get; set; }

  public virtual ResourceDictionary GetSkin(SkinType skinType)
  {
    if (this._precSkin == null || this._prevSkinType != skinType)
    {
      this._precSkin = HandyControl.Tools.ResourceHelper.GetSkin(skinType);
      this._prevSkinType = skinType;
    }
    if (!this.SyncWithSystem)
    {
      System.Windows.Media.Color? accentColor = this.AccentColor;
      if (accentColor.HasValue)
      {
        accentColor = this.AccentColor;
        this.UpdateAccentColor(accentColor.Value);
      }
    }
    else
    {
      uint pcrColorization;
      InteropMethods.DwmGetColorizationColor(out pcrColorization, out bool _);
      this.UpdateAccentColor(ColorHelper.ToColor(pcrColorization));
    }
    return this._precSkin;
  }

  public static Theme GetTheme(string name, ResourceDictionary resourceDictionary)
  {
    return string.IsNullOrEmpty(name) || resourceDictionary == null ? (Theme) null : resourceDictionary.MergedDictionaries.OfType<Theme>().FirstOrDefault<Theme>((Func<Theme, bool>) (item => object.Equals((object) item.Name, (object) name)));
  }

  public virtual ResourceDictionary GetTheme() => HandyControl.Tools.ResourceHelper.GetTheme();

  private void InitResource()
  {
    if (DesignerHelper.IsInDesignMode)
      return;
    this.MergedDictionaries.Clear();
    this.MergedDictionaries.Add(this.GetSkin(this.Skin));
    this.MergedDictionaries.Add(this.GetTheme());
  }

  private void UpdateAccentColor(System.Windows.Media.Color color)
  {
    this._precSkin[(object) "PrimaryColor"] = (object) color;
    this._precSkin[(object) "DarkPrimaryColor"] = (object) color;
    this._precSkin[(object) "TitleColor"] = (object) color;
    this._precSkin[(object) "SecondaryTitleColor"] = (object) color;
  }

  private void UpdateSkin() => this.MergedDictionaries[0] = this.GetSkin(this.Skin);
}
