// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ColorPickers.ColorValue
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.ColorPickers;

public class ColorValue
{
  private static object colorsLocker = new object();
  private static Dictionary<Color, string> colorsDict;

  public ColorValue(Color color, string name, string displayName)
  {
    this.Color = color;
    this.Name = name ?? "";
    this.DisplayName = displayName ?? "";
  }

  public ColorValue(Color color, string name)
    : this(color, name, name)
  {
  }

  public ColorValue(Color color, bool usePresetName = false)
  {
    this.Color = color;
    if (usePresetName)
    {
      if (ColorValue.colorsDict == null)
      {
        lock (ColorValue.colorsLocker)
          ColorValue.colorsDict = ((IEnumerable<PropertyInfo>) typeof (Colors).GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (c => c.PropertyType == typeof (Color))).Select<PropertyInfo, (Color, string)>((Func<PropertyInfo, (Color, string)>) (c => ((Color) c.GetValue((object) null), c.Name))).GroupBy<(Color, string), Color>((Func<(Color, string), Color>) (c => c.color)).ToDictionary<IGrouping<Color, (Color, string)>, Color, string>((Func<IGrouping<Color, (Color, string)>, Color>) (c => c.Key), (Func<IGrouping<Color, (Color, string)>, string>) (c => c.First<(Color, string)>().Item2));
      }
      string str;
      if (ColorValue.colorsDict.TryGetValue(color, out str))
      {
        this.Name = str;
        this.DisplayName = str;
      }
      else
        usePresetName = false;
    }
    if (usePresetName)
      return;
    string str1;
    if (color.A == byte.MaxValue)
      str1 = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    else
      str1 = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    this.Name = str1;
    this.DisplayName = str1;
  }

  public Color Color { get; }

  public string Name { get; }

  public string DisplayName { get; }

  public static implicit operator ColorValue(Color color) => new ColorValue(color, true);
}
