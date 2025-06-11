// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontsManager
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontsManager
{
  private static readonly HashSet<string> fontsWithKerning = new HashSet<string>();
  private static readonly HashSet<string> monospacedFonts = new HashSet<string>();
  private static readonly List<string> systemFonts;

  static SystemFontsManager()
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    using (StreamReader streamReader = new StreamReader(SystemFontDocumentsHelper.GetResourceStream("Fonts.meta")))
    {
      string str1;
      while ((str1 = streamReader.ReadLine()) != null)
      {
        if (!str1.StartsWith("//"))
        {
          string[] strArray = str1.Split(new char[1]{ ';' }, StringSplitOptions.RemoveEmptyEntries);
          string str2 = strArray[0];
          if (strArray[1] == "1")
            SystemFontsManager.monospacedFonts.Add(str2);
          if (strArray[2] == "1")
            SystemFontsManager.fontsWithKerning.Add(str2);
          for (int index = 3; index < strArray.Length; ++index)
            dictionary[strArray[index].ToLower()] = str2;
        }
      }
    }
    SystemFontsManager.systemFonts = new List<string>((IEnumerable<string>) new HashSet<string>());
    SystemFontsManager.systemFonts.Sort();
  }

  public static IEnumerable<string> GetAvailableFonts()
  {
    return (IEnumerable<string>) SystemFontsManager.systemFonts;
  }

  public static bool HasKerning(string fontName)
  {
    return SystemFontsManager.fontsWithKerning.Contains(fontName);
  }

  public static bool IsMonospaced(string fontFamily)
  {
    if (fontFamily == null)
      return false;
    string str1 = fontFamily;
    char[] chArray = new char[1]{ ',' };
    foreach (string str2 in str1.Split(chArray))
    {
      if (!SystemFontsManager.monospacedFonts.Contains(str2.Trim()))
        return false;
    }
    return true;
  }

  private static string SanitizeFontFileName(string fileName)
  {
    fileName = fileName.ToLower();
    return Regex.Replace(Path.GetFileNameWithoutExtension(fileName), "_[0-9]+$", string.Empty) + Path.GetExtension(fileName);
  }
}
