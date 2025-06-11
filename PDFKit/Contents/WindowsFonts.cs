// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.WindowsFonts
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Microsoft.Win32;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

#nullable disable
namespace PDFKit.Contents;

public static class WindowsFonts
{
  private static readonly IReadOnlyDictionary<FontCharSet, string[]> charSetLanguageNameMap = (IReadOnlyDictionary<FontCharSet, string[]>) new Dictionary<FontCharSet, string[]>()
  {
    [FontCharSet.ANSI_CHARSET] = new string[21]
    {
      "sv",
      "sw",
      "es",
      "pt",
      "nn",
      "nb",
      "ms",
      "it",
      "id",
      "is",
      "de",
      "gl",
      "fr",
      "fi",
      "fo",
      "en",
      "nl",
      "da",
      "ca",
      "eu",
      "af"
    },
    [FontCharSet.SYMBOL_CHARSET] = new string[1]{ "" },
    [FontCharSet.SHIFTJIS_CHARSET] = new string[1]{ "ja" },
    [FontCharSet.HANGEUL_CHARSET] = new string[1]{ "ko" },
    [FontCharSet.GB2312_CHARSET] = new string[2]
    {
      "zh-Hans",
      "zh-CHS"
    },
    [FontCharSet.CHINESEBIG5_CHARSET] = new string[2]
    {
      "zh-Hant",
      "zh-CHT"
    },
    [FontCharSet.HEBREW_CHARSET] = new string[1]{ "he" },
    [FontCharSet.ARABIC_CHARSET] = new string[1]{ "ar" },
    [FontCharSet.GREEK_CHARSET] = new string[1]{ "el" },
    [FontCharSet.TURKISH_CHARSET] = new string[3]
    {
      "uz",
      "tr",
      "az"
    },
    [FontCharSet.VIETNAMESE_CHARSET] = new string[1]{ "vi" },
    [FontCharSet.THAI_CHARSET] = new string[1]{ "th" },
    [FontCharSet.EASTEUROPE_CHARSET] = new string[9]
    {
      "sl",
      "sk",
      "sr",
      "ro",
      "pl",
      "hu",
      "cs",
      "hr",
      "sq"
    },
    [FontCharSet.RUSSIAN_CHARSET] = new string[11]
    {
      "uz",
      "uk",
      "tt",
      "sr",
      "ru",
      "mn",
      "kk",
      "mk",
      "bg",
      "be",
      "az"
    },
    [FontCharSet.BALTIC_CHARSET] = new string[3]
    {
      "lt",
      "lv",
      "et"
    }
  };
  private static IReadOnlyDictionary<string, FontCharSet[]> languageNameCharSetMap;
  private static readonly object locker = new object();
  private static WindowsFontFamily[] allFontFamilies;
  private static IReadOnlyDictionary<string, WindowsFontFamily> fontFamiliesDict;

  static WindowsFonts()
  {
    SystemEvents.InstalledFontsChanged += new EventHandler(WindowsFonts.SystemEvents_InstalledFontsChanged);
  }

  private static void SystemEvents_InstalledFontsChanged(object sender, EventArgs e)
  {
    lock (WindowsFonts.locker)
    {
      WindowsFonts.allFontFamilies = (WindowsFontFamily[]) null;
      WindowsFonts.fontFamiliesDict = (IReadOnlyDictionary<string, WindowsFontFamily>) null;
    }
  }

  public static WindowsFontFamily[] GetAllFontFamilies()
  {
    lock (WindowsFonts.locker)
    {
      if (WindowsFonts.allFontFamilies == null)
        WindowsFonts.BuildFontFamilies();
      return WindowsFonts.allFontFamilies;
    }
  }

  public static WindowsFontFamily GetFontFamily(string name)
  {
    lock (WindowsFonts.locker)
    {
      if (WindowsFonts.fontFamiliesDict == null)
        WindowsFonts.BuildFontFamilies();
      WindowsFontFamily windowsFontFamily;
      return WindowsFonts.fontFamiliesDict.TryGetValue(name, out windowsFontFamily) ? windowsFontFamily : (WindowsFontFamily) null;
    }
  }

  private static void BuildFontFamilies()
  {
    lock (WindowsFonts.locker)
    {
      WindowsFonts.allFontFamilies = WindowsFonts.GetAllFontFamiliesCore();
      WindowsFonts.fontFamiliesDict = (IReadOnlyDictionary<string, WindowsFontFamily>) ((IEnumerable<WindowsFontFamily>) WindowsFonts.allFontFamilies).ToDictionary<WindowsFontFamily, string>((Func<WindowsFontFamily, string>) (c => c.Name), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }
  }

  private static WindowsFontFamily[] GetAllFontFamiliesCore()
  {
    Dictionary<string, List<LOGFONT>> dict = new Dictionary<string, List<LOGFONT>>();
    WindowsFonts.EnumFontExDelegate lpEnumFontFamExProc = new WindowsFonts.EnumFontExDelegate(EnumFontEx);
    IntPtr num1 = IntPtr.Zero;
    WindowsFonts._LOGFONT structure = new WindowsFonts._LOGFONT()
    {
      lfCharSet = FontCharSet.DEFAULT_CHARSET,
      lfFaceName = "",
      lfPitchAndFamily = LOGFONT.FontPitchAndFamily.DEFAULT_PITCH
    };
    IntPtr num2 = IntPtr.Zero;
    try
    {
      num2 = Marshal.AllocHGlobal(Marshal.SizeOf<WindowsFonts._LOGFONT>(structure));
      Marshal.StructureToPtr<WindowsFonts._LOGFONT>(structure, num2, false);
      num1 = WindowsFonts.GetDC(IntPtr.Zero);
      WindowsFonts.EnumFontFamiliesEx(num1, num2, lpEnumFontFamExProc, IntPtr.Zero, 0U);
    }
    finally
    {
      try
      {
        if (num2 != IntPtr.Zero)
          Marshal.FreeHGlobal(num2);
      }
      catch
      {
      }
      if (num1 != IntPtr.Zero)
        WindowsFonts.ReleaseDC(IntPtr.Zero, num1);
    }
    return dict.Select<KeyValuePair<string, List<LOGFONT>>, WindowsFontFamily>((Func<KeyValuePair<string, List<LOGFONT>>, WindowsFontFamily>) (c => new WindowsFontFamily(c.Key, (IEnumerable<LOGFONT>) c.Value))).ToArray<WindowsFontFamily>();

    int EnumFontEx(
      ref WindowsFonts.ENUMLOGFONTEX lpelfe,
      ref WindowsFonts.NEWTEXTMETRICEX lpntme,
      int FontType,
      int lParam)
    {
      if (!string.IsNullOrEmpty(lpelfe.elfFullName) && lpelfe.elfFullName[0] != '@')
      {
        LOGFONT logfont = new LOGFONT()
        {
          lfPitchAndFamily = lpelfe.elfLogFont.lfPitchAndFamily,
          lfCharSet = lpelfe.elfLogFont.lfCharSet,
          lfClipPrecision = lpelfe.elfLogFont.lfClipPrecision,
          lfFaceName = lpelfe.elfLogFont.lfFaceName,
          lfItalic = lpelfe.elfLogFont.lfItalic,
          lfOutPrecision = lpelfe.elfLogFont.lfOutPrecision,
          lfQuality = lpelfe.elfLogFont.lfQuality,
          lfWeight = lpelfe.elfLogFont.lfWeight
        };
        List<LOGFONT> logfontList1;
        if (dict.TryGetValue(lpelfe.elfFullName, out logfontList1))
        {
          logfontList1.Add(logfont);
        }
        else
        {
          List<LOGFONT> logfontList2 = new List<LOGFONT>()
          {
            logfont
          };
          dict[lpelfe.elfFullName] = logfontList2;
        }
      }
      return 1;
    }
  }

  internal static IReadOnlyCollection<FontCharSet> MapCultureInfoToCharSet(CultureInfo cultureInfo)
  {
    if (cultureInfo == null || cultureInfo.Name == "")
      return (IReadOnlyCollection<FontCharSet>) Array.Empty<FontCharSet>();
    CultureInfo cultureInfo1 = cultureInfo;
    while (cultureInfo1 != null && !string.IsNullOrEmpty(cultureInfo1.Name) && !cultureInfo1.IsNeutralCulture)
      cultureInfo1 = cultureInfo1.Parent;
    if (WindowsFonts.languageNameCharSetMap == null)
    {
      lock (WindowsFonts.charSetLanguageNameMap)
      {
        if (WindowsFonts.languageNameCharSetMap == null)
        {
          WindowsFonts.charSetLanguageNameMap.SelectMany<KeyValuePair<FontCharSet, string[]>, string>((Func<KeyValuePair<FontCharSet, string[]>, IEnumerable<string>>) (c => (IEnumerable<string>) c.Value)).GroupBy<string, string>((Func<string, string>) (c => c)).Select<IGrouping<string, string>, (string, int)>((Func<IGrouping<string, string>, (string, int)>) (c => (c.Key, c.Count<string>()))).ToList<(string, int)>();
          WindowsFonts.languageNameCharSetMap = (IReadOnlyDictionary<string, FontCharSet[]>) WindowsFonts.charSetLanguageNameMap.SelectMany<KeyValuePair<FontCharSet, string[]>, string, (FontCharSet, string)>((Func<KeyValuePair<FontCharSet, string[]>, IEnumerable<string>>) (c => (IEnumerable<string>) c.Value), (Func<KeyValuePair<FontCharSet, string[]>, string, (FontCharSet, string)>) ((s, c) => (s.Key, c))).GroupBy<(FontCharSet, string), string>((Func<(FontCharSet, string), string>) (c => c.lang)).ToDictionary<IGrouping<string, (FontCharSet, string)>, string, FontCharSet[]>((Func<IGrouping<string, (FontCharSet, string)>, string>) (c => c.Key), (Func<IGrouping<string, (FontCharSet, string)>, FontCharSet[]>) (c => c.Select<(FontCharSet, string), FontCharSet>((Func<(FontCharSet, string), FontCharSet>) (x => x.charset)).ToArray<FontCharSet>()), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
        }
      }
    }
    FontCharSet[] fontCharSetArray;
    return WindowsFonts.languageNameCharSetMap.TryGetValue(cultureInfo1.Name, out fontCharSetArray) ? (IReadOnlyCollection<FontCharSet>) fontCharSetArray : (IReadOnlyCollection<FontCharSet>) Array.Empty<FontCharSet>();
  }

  [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
  private static extern int EnumFontFamiliesEx(
    IntPtr hdc,
    [In] IntPtr pLogfont,
    WindowsFonts.EnumFontExDelegate lpEnumFontFamExProc,
    IntPtr lParam,
    uint dwFlags);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  private static extern IntPtr GetDC(IntPtr hWnd);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

  private delegate int EnumFontExDelegate(
    ref WindowsFonts.ENUMLOGFONTEX lpelfe,
    ref WindowsFonts.NEWTEXTMETRICEX lpntme,
    int FontType,
    int lParam);

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  private struct ENUMLOGFONTEX
  {
    public WindowsFonts._LOGFONT elfLogFont;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64 /*0x40*/)]
    public string elfFullName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
    public string elfStyle;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
    public string elfScript;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  private struct NEWTEXTMETRIC
  {
    public int tmHeight;
    public int tmAscent;
    public int tmDescent;
    public int tmInternalLeading;
    public int tmExternalLeading;
    public int tmAveCharWidth;
    public int tmMaxCharWidth;
    public int tmWeight;
    public int tmOverhang;
    public int tmDigitizedAspectX;
    public int tmDigitizedAspectY;
    public char tmFirstChar;
    public char tmLastChar;
    public char tmDefaultChar;
    public char tmBreakChar;
    public byte tmItalic;
    public byte tmUnderlined;
    public byte tmStruckOut;
    public byte tmPitchAndFamily;
    public byte tmCharSet;
    private int ntmFlags;
    private int ntmSizeEM;
    private int ntmCellHeight;
    private int ntmAvgWidth;
  }

  private struct FONTSIGNATURE
  {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
    private int[] fsUsb;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
    private int[] fsCsb;
  }

  private struct NEWTEXTMETRICEX
  {
    private WindowsFonts.NEWTEXTMETRIC ntmTm;
    private WindowsFonts.FONTSIGNATURE ntmFontSig;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  private class _LOGFONT
  {
    public int lfHeight;
    public int lfWidth;
    public int lfEscapement;
    public int lfOrientation;
    public FontWeight lfWeight;
    [MarshalAs(UnmanagedType.U1)]
    public bool lfItalic;
    [MarshalAs(UnmanagedType.U1)]
    public bool lfUnderline;
    [MarshalAs(UnmanagedType.U1)]
    public bool lfStrikeOut;
    public FontCharSet lfCharSet;
    public LOGFONT.FontPrecision lfOutPrecision;
    public LOGFONT.FontClipPrecision lfClipPrecision;
    public LOGFONT.FontQuality lfQuality;
    public LOGFONT.FontPitchAndFamily lfPitchAndFamily;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
    public string lfFaceName;
  }
}
