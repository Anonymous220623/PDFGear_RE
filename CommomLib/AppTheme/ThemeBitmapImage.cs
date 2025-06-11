// Decompiled with JetBrains decompiler
// Type: CommomLib.AppTheme.ThemeBitmapImage
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace CommomLib.AppTheme;

public static class ThemeBitmapImage
{
  internal static readonly DependencyProperty ThemeUrisProperty = DependencyProperty.RegisterAttached("ThemeUris", typeof (object), typeof (ThemeBitmapImage), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) => ThemeBitmapImage.NotifyThemeChanged(s as DrawingImage))));
  internal static readonly DependencyProperty ThemeResourceDictionaryProperty = DependencyProperty.RegisterAttached("ThemeResourceDictionary", typeof (ThemeResourceDictionary), typeof (ThemeBitmapImage), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) => ThemeBitmapImage.UpdateThemeResourceDictionary(s as DrawingImage, a.OldValue as ThemeResourceDictionary, a.NewValue as ThemeResourceDictionary))));

  public static ImageSource CreateBitmapImage(Uri lightImageSource, Uri darkImageSource)
  {
    return ThemeBitmapImage.CreateBitmapImage(ThemeResourceDictionary.GetForCurrentApp(), new Dictionary<string, Uri>()
    {
      ["Light"] = lightImageSource,
      ["Dark"] = darkImageSource
    });
  }

  public static ImageSource CreateBitmapImage(
    ThemeResourceDictionary themeResourceDictionary,
    Dictionary<string, Uri> themeUris)
  {
    DrawingImage bitmapImage = new DrawingImage();
    bitmapImage.SetValue(ThemeBitmapImage.ThemeUrisProperty, (object) themeUris.ToDictionary<KeyValuePair<string, Uri>, string, Uri>((Func<KeyValuePair<string, Uri>, string>) (c => c.Key), (Func<KeyValuePair<string, Uri>, Uri>) (c => c.Value), themeUris.Comparer));
    bitmapImage.SetValue(ThemeBitmapImage.ThemeResourceDictionaryProperty, (object) themeResourceDictionary);
    ThemeBitmapImage.NotifyThemeChanged(bitmapImage);
    return (ImageSource) bitmapImage;
  }

  private static void NotifyThemeChanged(DrawingImage obj)
  {
    if (obj == null)
      return;
    Dictionary<string, Uri> dictionary = obj.GetValue(ThemeBitmapImage.ThemeUrisProperty) as Dictionary<string, Uri>;
    ThemeResourceDictionary resourceDictionary = obj.GetValue(ThemeBitmapImage.ThemeResourceDictionaryProperty) as ThemeResourceDictionary;
    if (dictionary == null || resourceDictionary == null)
      return;
    string actualTheme = resourceDictionary.ActualTheme;
    Drawing drawing = (Drawing) null;
    Uri uri;
    if (dictionary.TryGetValue(actualTheme, out uri))
    {
      BitmapImage bitmapImage = new BitmapImage();
      bitmapImage.BeginInit();
      bitmapImage.UriSource = uri;
      bitmapImage.EndInit();
      bitmapImage.Freeze();
      if (bitmapImage.Width > 0.0 && bitmapImage.Height > 0.0)
      {
        Rect rect = new Rect(0.0, 0.0, bitmapImage.Width, bitmapImage.Height);
        drawing = (Drawing) new ImageDrawing((ImageSource) bitmapImage, rect);
        drawing.Freeze();
      }
    }
    obj.Drawing = drawing;
  }

  private static void UpdateThemeResourceDictionary(
    DrawingImage obj,
    ThemeResourceDictionary oldValue,
    ThemeResourceDictionary newValue)
  {
    if (obj == null)
      return;
    WeakReference<DrawingImage> weakObj = new WeakReference<DrawingImage>(obj);
    if (oldValue != null)
      oldValue.ActualThemeChanged -= new EventHandler<ActualThemeChangedEventArgs>(OnActualThemeChanged);
    if (newValue != null)
      newValue.ActualThemeChanged += new EventHandler<ActualThemeChangedEventArgs>(OnActualThemeChanged);
    ThemeBitmapImage.NotifyThemeChanged(obj);

    void OnActualThemeChanged(object sender, ActualThemeChangedEventArgs e)
    {
      DrawingImage target;
      if (!weakObj.TryGetTarget(out target))
        return;
      ThemeBitmapImage.NotifyThemeChanged(target);
    }
  }
}
