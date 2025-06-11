// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.SkinColorScheme
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class SkinColorScheme : SkinStorage
{
  public Color sskinColor = new Color();
  private static object value;

  public SkinColorScheme(Color skinColor) => this.sskinColor = skinColor;

  public SkinColorScheme(int skinColor)
  {
    this.sskinColor = Color.FromArgb((byte) (((long) skinColor & 4278190080L /*0xFF000000*/) >> 24), (byte) ((skinColor & 16711680 /*0xFF0000*/) >> 16 /*0x10*/), (byte) ((skinColor & 65280) >> 8), (byte) (skinColor & (int) byte.MaxValue));
  }

  private static GradientStop clone(GradientStop parent, GradientStop child)
  {
    child.Color = parent.Color;
    child.Offset = parent.Offset;
    return child;
  }

  public static ResourceDictionary ApplyCustomColorScheme(
    ResourceDictionary templatedictionary,
    Color skinColor)
  {
    if (!(skinColor != new Color()))
      return templatedictionary;
    for (int index = 0; index < templatedictionary.MergedDictionaries.Count; ++index)
    {
      ResourceDictionary mergedDictionary = templatedictionary.MergedDictionaries[index];
      if (mergedDictionary.Source.ToString().EndsWith("Brushes.xaml"))
      {
        ResourceDictionary dictionary = mergedDictionary;
        if (dictionary != null)
        {
          ResourceDictionary resourceDictionary = SkinColorScheme.MergeColors(dictionary, skinColor);
          templatedictionary.MergedDictionaries.RemoveAt(index);
          templatedictionary.MergedDictionaries.Insert(index, resourceDictionary);
        }
      }
      else
      {
        ResourceDictionary templatedictionary1 = mergedDictionary;
        if (templatedictionary1 != null)
        {
          ResourceDictionary resourceDictionary = SkinColorScheme.ApplyCustomColorScheme(templatedictionary1, skinColor);
          templatedictionary.MergedDictionaries.RemoveAt(index);
          templatedictionary.MergedDictionaries.Insert(index, resourceDictionary);
        }
      }
    }
    return SkinColorScheme.MergeColors(templatedictionary, skinColor);
  }

  private static ResourceDictionary MergeColors(ResourceDictionary dictionary, Color skinColor)
  {
    SkinColorScheme skinColorScheme = new SkinColorScheme(skinColor);
    foreach (object key in (IEnumerable) dictionary.Keys)
    {
      SkinColorScheme.value = dictionary[(object) Convert.ToString(key)];
      switch (SkinColorScheme.value)
      {
        case LinearGradientBrush _:
          LinearGradientBrush linearGradientBrush1 = SkinColorScheme.value as LinearGradientBrush;
          LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush();
          linearGradientBrush2.StartPoint = linearGradientBrush1.StartPoint;
          linearGradientBrush2.EndPoint = linearGradientBrush1.EndPoint;
          linearGradientBrush2.Transform = linearGradientBrush1.Transform;
          foreach (GradientStop gradientStop in linearGradientBrush1.GradientStops)
            linearGradientBrush2.GradientStops.Add(new GradientStop()
            {
              Offset = gradientStop.Offset,
              Color = skinColorScheme.GetColor(gradientStop.Color)
            });
          try
          {
            (dictionary[key] as LinearGradientBrush).StartPoint = linearGradientBrush2.StartPoint;
            (dictionary[key] as LinearGradientBrush).EndPoint = linearGradientBrush2.EndPoint;
            (dictionary[key] as LinearGradientBrush).Transform = linearGradientBrush2.Transform;
            (dictionary[key] as LinearGradientBrush).GradientStops.Clear();
            using (GradientStopCollection.Enumerator enumerator = linearGradientBrush2.GradientStops.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                GradientStop gradientStop = SkinColorScheme.clone(enumerator.Current, new GradientStop());
                (dictionary[key] as LinearGradientBrush).GradientStops.Add(gradientStop);
              }
              continue;
            }
          }
          catch
          {
            continue;
          }
        case SolidColorBrush _:
          SolidColorBrush solidColorBrush = new SolidColorBrush();
          try
          {
            (dictionary[key] as SolidColorBrush).Color = skinColorScheme.GetColor((SkinColorScheme.value as SolidColorBrush).Color);
            continue;
          }
          catch
          {
            continue;
          }
        case RadialGradientBrush _:
          RadialGradientBrush radialGradientBrush1 = SkinColorScheme.value as RadialGradientBrush;
          RadialGradientBrush radialGradientBrush2 = new RadialGradientBrush();
          radialGradientBrush2.Center = radialGradientBrush1.Center;
          radialGradientBrush2.GradientOrigin = radialGradientBrush1.GradientOrigin;
          radialGradientBrush2.RadiusX = radialGradientBrush1.RadiusX;
          radialGradientBrush2.RadiusY = radialGradientBrush1.RadiusY;
          radialGradientBrush2.RelativeTransform = radialGradientBrush1.RelativeTransform;
          foreach (GradientStop gradientStop in radialGradientBrush1.GradientStops)
            radialGradientBrush2.GradientStops.Add(new GradientStop()
            {
              Offset = gradientStop.Offset,
              Color = skinColorScheme.GetColor(gradientStop.Color)
            });
          try
          {
            (dictionary[key] as RadialGradientBrush).Center = radialGradientBrush2.Center;
            (dictionary[key] as RadialGradientBrush).GradientOrigin = radialGradientBrush2.GradientOrigin;
            (dictionary[key] as RadialGradientBrush).RelativeTransform = radialGradientBrush2.RelativeTransform;
            (dictionary[key] as RadialGradientBrush).RadiusX = radialGradientBrush2.RadiusX;
            (dictionary[key] as RadialGradientBrush).RadiusY = radialGradientBrush2.RadiusY;
            (dictionary[key] as RadialGradientBrush).GradientStops.Clear();
            using (GradientStopCollection.Enumerator enumerator = radialGradientBrush2.GradientStops.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                GradientStop gradientStop = SkinColorScheme.clone(enumerator.Current, new GradientStop());
                (dictionary[key] as RadialGradientBrush).GradientStops.Add(gradientStop);
              }
              continue;
            }
          }
          catch
          {
            continue;
          }
        default:
          continue;
      }
    }
    return dictionary;
  }

  internal Color GetColor(int rgb)
  {
    if (rgb == -1)
      return new Color();
    byte baseChannel1 = (byte) ((rgb & 16711680 /*0xFF0000*/) >> 16 /*0x10*/);
    byte baseChannel2 = (byte) ((rgb & 65280) >> 8);
    byte baseChannel3 = (byte) (rgb & (int) byte.MaxValue);
    return Color.FromArgb(byte.MaxValue, SkinColorScheme.MergeChannels((int) baseChannel1, (int) this.sskinColor.R), SkinColorScheme.MergeChannels((int) baseChannel2, (int) this.sskinColor.G), SkinColorScheme.MergeChannels((int) baseChannel3, (int) this.sskinColor.B));
  }

  internal Color GetColor(Color baseColor)
  {
    return baseColor.A == (byte) 0 ? baseColor : Color.FromArgb(baseColor.A, SkinColorScheme.MergeChannels((int) baseColor.R, (int) this.sskinColor.R), SkinColorScheme.MergeChannels((int) baseColor.G, (int) this.sskinColor.G), SkinColorScheme.MergeChannels((int) baseColor.B, (int) this.sskinColor.B));
  }

  internal static Color GetColor(Color baseColor, Color skinColor)
  {
    return baseColor.A == (byte) 0 ? baseColor : Color.FromArgb(baseColor.A, SkinColorScheme.MergeChannels((int) baseColor.R, (int) skinColor.R), SkinColorScheme.MergeChannels((int) baseColor.G, (int) skinColor.G), SkinColorScheme.MergeChannels((int) baseColor.B, (int) skinColor.B));
  }

  internal static byte MergeChannels(int baseChannel, int skinChannel)
  {
    int maxValue = (int) byte.MaxValue;
    int num1 = baseChannel * skinChannel / maxValue;
    int num2 = (maxValue - baseChannel) * (maxValue - skinChannel) / maxValue;
    int num3 = baseChannel * (maxValue - num2 - num1);
    return (byte) (num1 + num3 / (int) byte.MaxValue);
  }

  internal static Color MergeChannels(Color color, Color skinColor)
  {
    return Color.FromArgb(color.A, SkinColorScheme.MergeChannels((int) color.R, (int) skinColor.R), SkinColorScheme.MergeChannels((int) color.G, (int) skinColor.G), SkinColorScheme.MergeChannels((int) color.B, (int) skinColor.B));
  }
}
