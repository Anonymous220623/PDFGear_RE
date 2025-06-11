// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFontsManager
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontFontsManager
{
  private const string RegistryFontPath = "Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts";
  private readonly Dictionary<SystemFontFontDescriptor, SystemFontOpenTypeFontSourceBase> fonts;
  private readonly string FontsFolder = Path.Combine(Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.System)).FullName, "Fonts");

  public SystemFontFontsManager()
  {
    this.fonts = new Dictionary<SystemFontFontDescriptor, SystemFontOpenTypeFontSourceBase>();
  }

  internal SystemFontOpenTypeFontSourceBase GetFontSource(SystemFontFontDescriptor descr)
  {
    string fontFamily = descr.FontFamily;
    FontStyle fontStyle = descr.FontStyle;
    SystemFontOpenTypeFontSourceBase openTypeFontSource;
    if (!this.fonts.TryGetValue(descr, out openTypeFontSource))
    {
      openTypeFontSource = (SystemFontOpenTypeFontSourceBase) this.GetOpenTypeFontSource(fontFamily, fontStyle);
      this.fonts[descr] = openTypeFontSource;
    }
    return openTypeFontSource;
  }

  internal SystemFontOpenTypeFontSource GetOpenTypeFontSource(string fontName, FontStyle style)
  {
    if (string.IsNullOrEmpty(fontName))
      fontName = "Arial";
    fontName = fontName.ToLower();
    if ((style & FontStyle.Bold) == FontStyle.Bold)
      fontName += " Bold";
    if ((style & FontStyle.Italic) == FontStyle.Italic)
      fontName += " Italic";
    string empty = string.Empty;
    return new SystemFontOpenTypeFontSource(new SystemFontOpenTypeFontReader(File.ReadAllBytes(Path.GetFullPath(Path.Combine(this.FontsFolder, this.FontFallback(fontName))))));
  }

  internal string FontFallback(string fontFamily)
  {
    fontFamily = fontFamily.ToLower();
    string[] strArray = fontFamily.Split(new string[4]
    {
      " ",
      ",",
      "(",
      ")"
    }, StringSplitOptions.RemoveEmptyEntries);
    string[] valueNames = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts").GetValueNames();
    List<string> stringList1 = new List<string>((IEnumerable<string>) valueNames);
    List<string> stringList2 = new List<string>();
    foreach (string name in stringList1)
    {
      if (name.ToLower().Contains(fontFamily))
        stringList2.Add(name);
      if (name.ToLower().Replace(" (truetype)", string.Empty).ToLower().Replace(" mt", string.Empty).Equals(fontFamily))
        return (string) Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts").GetValue(name);
    }
    string str1 = "arial.ttf";
    int num1 = 0;
    foreach (string name in valueNames)
    {
      string lower = name.ToLower();
      int num2 = 0;
      foreach (string str2 in strArray)
        num2 = (!lower.Contains(str2) ? num2 - 1 : num2 + 1) * 2;
      if (num2 > num1)
      {
        num1 = num2;
        str1 = (string) Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts").GetValue(name);
      }
    }
    return str1;
  }
}
