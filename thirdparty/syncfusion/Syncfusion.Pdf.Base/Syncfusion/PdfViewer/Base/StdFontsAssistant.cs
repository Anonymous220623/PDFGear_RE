// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.StdFontsAssistant
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class StdFontsAssistant
{
  private const string Courier = "Courier";
  private const string CourierBold = "Courier-Bold";
  private const string CourierBoldOblique = "Courier-BoldOblique";
  private const string CourierOblique = "Courier-Oblique";
  private const string Helvetica = "Helvetica";
  private const string HelveticaBold = "Helvetica-Bold";
  private const string HelveticaBoldOblique = "Helvetica-BoldOblique";
  private const string HelveticaOblique = "Helvetica-Oblique";
  private const string TimesRoman = "Times-Roman";
  private const string TimesBold = "Times-Bold";
  private const string TimesBoldItalic = "Times-BoldItalic";
  private const string TimesItalic = "Times-Italic";
  private const string Symbol = "Symbol";
  private const string ZapfDingbats = "ZapfDingbats";
  private static readonly Dictionary<string, string> alternativeNames;
  internal static readonly Dictionary<string, StandardFontDescriptor> standardFontDescriptors = new Dictionary<string, StandardFontDescriptor>();
  private static Dictionary<string, byte[]> standardFontStreams;
  private readonly Dictionary<string, Type1FontSource> standardFontSources;
  private StdFontsAssistant systemFontsManager;
  private object syncObj = new object();

  public StdFontsAssistant SystemFontsManager
  {
    get
    {
      if (this.systemFontsManager == null)
        this.systemFontsManager = new StdFontsAssistant();
      return this.systemFontsManager;
    }
  }

  static StdFontsAssistant()
  {
    StdFontsAssistant.alternativeNames = new Dictionary<string, string>();
    StdFontsAssistant.InitializeStandardFontStreams();
    StdFontsAssistant.InitializeStandardFontDescriptors();
    StdFontsAssistant.InitializeAlternativeNames();
  }

  public StdFontsAssistant()
  {
    this.standardFontSources = new Dictionary<string, Type1FontSource>();
    this.InitializeStandardFontSources();
  }

  public static string StripFontName(string fontName)
  {
    int num = fontName.IndexOf("+");
    return num > 0 ? fontName.Substring(num + 1).Split(Chars.FontFamilyDelimiters)[0] : fontName.Split(Chars.FontFamilyDelimiters)[0];
  }

  public static void InitializeStandardFontStreams()
  {
    if (StdFontsAssistant.standardFontStreams != null)
      return;
    StdFontsAssistant.standardFontStreams = new Dictionary<string, byte[]>();
    StdFontsAssistant.RegisterStandardFontStream("Courier", StdFontsAssistant.GetApplicationResourceStream("Courier"));
    StdFontsAssistant.RegisterStandardFontStream("Courier-Bold", StdFontsAssistant.GetApplicationResourceStream("Courier-Bold"));
    StdFontsAssistant.RegisterStandardFontStream("Courier-BoldOblique", StdFontsAssistant.GetApplicationResourceStream("Courier-BoldOblique"));
    StdFontsAssistant.RegisterStandardFontStream("Courier-Oblique", StdFontsAssistant.GetApplicationResourceStream("Courier-Oblique"));
    StdFontsAssistant.RegisterStandardFontStream("Helvetica", StdFontsAssistant.GetApplicationResourceStream("Helvetica"));
    StdFontsAssistant.RegisterStandardFontStream("Helvetica-Bold", StdFontsAssistant.GetApplicationResourceStream("Helvetica-Bold"));
    StdFontsAssistant.RegisterStandardFontStream("Helvetica-BoldOblique", StdFontsAssistant.GetApplicationResourceStream("Helvetica-BoldOblique"));
    StdFontsAssistant.RegisterStandardFontStream("Helvetica-Oblique", StdFontsAssistant.GetApplicationResourceStream("Helvetica-Oblique"));
    StdFontsAssistant.RegisterStandardFontStream("Times-Roman", StdFontsAssistant.GetApplicationResourceStream("Times-Roman"));
    StdFontsAssistant.RegisterStandardFontStream("Times-Bold", StdFontsAssistant.GetApplicationResourceStream("Times-Bold"));
    StdFontsAssistant.RegisterStandardFontStream("Times-BoldItalic", StdFontsAssistant.GetApplicationResourceStream("Times-BoldItalic"));
    StdFontsAssistant.RegisterStandardFontStream("Times-Italic", StdFontsAssistant.GetApplicationResourceStream("Times-Italic"));
    StdFontsAssistant.RegisterStandardFontStream("Symbol", StdFontsAssistant.GetApplicationResourceStream("Symbol"));
    StdFontsAssistant.RegisterStandardFontStream("ZapfDingbats", StdFontsAssistant.GetApplicationResourceStream("ZapfDingbats"));
  }

  public static bool IsStandardFontName(string name)
  {
    return StdFontsAssistant.standardFontDescriptors.ContainsKey(name) || StdFontsAssistant.alternativeNames.ContainsKey(name);
  }

  public static bool IsAlternativeStdFontAvailable(string name)
  {
    return StdFontsAssistant.alternativeNames.ContainsKey(name);
  }

  public static StandardFontDescriptor GetStandardFontDescriptor(string fontName)
  {
    string standardFontName = StdFontsAssistant.GetStandardFontName(fontName);
    StandardFontDescriptor standardFontDescriptor;
    if (!StdFontsAssistant.standardFontDescriptors.TryGetValue(standardFontName, out standardFontDescriptor))
      throw new ArgumentException("Font name is not a standard font.", fontName);
    return standardFontDescriptor;
  }

  public Type1FontSource GetStandardFontSource(string fontName)
  {
    Monitor.Enter(this.syncObj);
    Type1FontSource standardFontSource;
    try
    {
      string standardFontName = StdFontsAssistant.GetStandardFontName(fontName);
      if (!this.standardFontSources.TryGetValue(standardFontName, out standardFontSource))
      {
        bool flag = false;
        foreach (KeyValuePair<string, byte[]> standardFontStream in StdFontsAssistant.standardFontStreams)
        {
          if (standardFontStream.Key == standardFontName)
          {
            this.RegisterStandardFontSource(standardFontStream.Key, StdFontsAssistant.CreateFontSource(standardFontStream.Value));
            standardFontSource = this.standardFontSources[standardFontName];
            flag = true;
            break;
          }
        }
        if (!flag)
          throw new ArgumentException("Font name is not a standard font.", fontName);
      }
    }
    finally
    {
      Monitor.Exit(this.syncObj);
    }
    return standardFontSource;
  }

  public Type1FontSource GetType1FallbackFontSource(string fontName)
  {
    string lower = fontName.ToLower();
    bool flag1 = StdFontsAssistant.IsBold(lower);
    bool flag2 = StdFontsAssistant.IsItalic(lower);
    string fontName1 = "Helvetica";
    if (flag1)
      fontName1 = !flag2 ? "Helvetica-Bold" : "Helvetica-BoldOblique";
    else if (flag2)
      fontName1 = "Helvetica-Oblique";
    return this.GetStandardFontSource(fontName1);
  }

  private static Stream GetApplicationResourceStream(string fontName)
  {
    Stream applicationResourceStream;
    try
    {
      switch (fontName)
      {
        case "Courier":
          applicationResourceStream = (Stream) new MemoryStream(Resources.Courier);
          break;
        case "Courier-Bold":
          applicationResourceStream = (Stream) new MemoryStream(Resources.Courier_Bold);
          break;
        case "Courier-BoldOblique":
          applicationResourceStream = (Stream) new MemoryStream(Resources.Courier_BoldOblique);
          break;
        case "Courier-Oblique":
          applicationResourceStream = (Stream) new MemoryStream(Resources.Courier_Oblique);
          break;
        case "Helvetica":
          applicationResourceStream = (Stream) new MemoryStream(Resources.Helvetica);
          break;
        case "Helvetica-Bold":
          applicationResourceStream = (Stream) new MemoryStream(Resources.Helvetica_Bold);
          break;
        case "Helvetica-BoldOblique":
          applicationResourceStream = (Stream) new MemoryStream(Resources.Helvetica_BoldOblique);
          break;
        case "Helvetica-Oblique":
          applicationResourceStream = (Stream) new MemoryStream(Resources.Helvetica_Oblique);
          break;
        case "Symbol":
          applicationResourceStream = (Stream) new MemoryStream(Resources.Symbol);
          break;
        case "Times-Bold":
          applicationResourceStream = (Stream) new MemoryStream(Resources.Times_Bold);
          break;
        case "Times-BoldItalic":
          applicationResourceStream = (Stream) new MemoryStream(Resources.Times_BoldItalic);
          break;
        case "Times-Italic":
          applicationResourceStream = (Stream) new MemoryStream(Resources.Times_Italic);
          break;
        case "Times-Roman":
          applicationResourceStream = (Stream) new MemoryStream(Resources.Times_Roman);
          break;
        case "ZapfDingbats":
          applicationResourceStream = (Stream) new MemoryStream(Resources.ZapfDingbats);
          break;
        default:
          applicationResourceStream = (Stream) null;
          break;
      }
    }
    catch
    {
      throw new InvalidOperationException("Cannot access the resource file");
    }
    return applicationResourceStream;
  }

  private static string GetStandardFontName(string fontName)
  {
    string standardFontName = fontName;
    lock (new object())
    {
      if (StdFontsAssistant.alternativeNames.ContainsKey(fontName))
        standardFontName = StdFontsAssistant.alternativeNames[fontName];
    }
    return standardFontName;
  }

  private static void RegisterStandardFontStream(string fontName, Stream stream)
  {
    StdFontsAssistant.standardFontStreams[fontName] = Extensions.ReadAllBytes(stream);
  }

  private static void RegisterStandardFontDescriptor(
    string name,
    StandardFontDescriptor fontDescriptor)
  {
    StdFontsAssistant.standardFontDescriptors[name] = fontDescriptor;
  }

  private static bool IsBold(string styles)
  {
    if (string.IsNullOrEmpty(styles))
      return false;
    return styles.Contains("bold") || styles.Contains("bd");
  }

  private static bool IsItalic(string styles)
  {
    if (string.IsNullOrEmpty(styles))
      return false;
    return styles.Contains("it") || styles.Contains("obl");
  }

  internal static void InitializeStandardFontDescriptors()
  {
    StandardFontDescriptor fontDescriptor1 = new StandardFontDescriptor();
    fontDescriptor1.Ascent = 629.0;
    fontDescriptor1.Descent = -157.0;
    StandardFontDescriptor fontDescriptor2 = new StandardFontDescriptor();
    fontDescriptor2.Ascent = 718.0;
    fontDescriptor2.Descent = -207.0;
    StandardFontDescriptor fontDescriptor3 = new StandardFontDescriptor();
    fontDescriptor2.Ascent = 683.0;
    fontDescriptor2.Descent = -217.0;
    StandardFontDescriptor fontDescriptor4 = new StandardFontDescriptor();
    fontDescriptor2.Ascent = 1000.0;
    fontDescriptor2.Descent = 0.0;
    StdFontsAssistant.RegisterStandardFontDescriptor("Courier", fontDescriptor1);
    StdFontsAssistant.RegisterStandardFontDescriptor("Courier-Bold", fontDescriptor1);
    StdFontsAssistant.RegisterStandardFontDescriptor("Courier-BoldOblique", fontDescriptor1);
    StdFontsAssistant.RegisterStandardFontDescriptor("Courier-Oblique", fontDescriptor1);
    StdFontsAssistant.RegisterStandardFontDescriptor("Helvetica", fontDescriptor2);
    StdFontsAssistant.RegisterStandardFontDescriptor("Helvetica-Bold", fontDescriptor2);
    StdFontsAssistant.RegisterStandardFontDescriptor("Helvetica-BoldOblique", fontDescriptor2);
    StdFontsAssistant.RegisterStandardFontDescriptor("Helvetica-Oblique", fontDescriptor2);
    StdFontsAssistant.RegisterStandardFontDescriptor("Times-Roman", fontDescriptor3);
    StdFontsAssistant.RegisterStandardFontDescriptor("Times-Bold", fontDescriptor3);
    StdFontsAssistant.RegisterStandardFontDescriptor("Times-BoldItalic", fontDescriptor3);
    StdFontsAssistant.RegisterStandardFontDescriptor("Times-Italic", fontDescriptor3);
    StdFontsAssistant.RegisterStandardFontDescriptor("Symbol", fontDescriptor4);
    StdFontsAssistant.RegisterStandardFontDescriptor("ZapfDingbats", fontDescriptor4);
  }

  private static void InitializeAlternativeNames()
  {
    StdFontsAssistant.RegisterAlternativeName("Helvetica", "Helvetica");
    StdFontsAssistant.RegisterAlternativeName("Helvetica-Bold", "Helvetica,Bold");
    StdFontsAssistant.RegisterAlternativeName("Helvetica-BoldOblique", "Helvetica,BoldItalic");
    StdFontsAssistant.RegisterAlternativeName("Helvetica-Oblique", "Helvetica,Italic");
    StdFontsAssistant.RegisterAlternativeName("Courier", "Courier New");
    StdFontsAssistant.RegisterAlternativeName("Courier-Bold", "Courier New,Bold");
    StdFontsAssistant.RegisterAlternativeName("Courier-BoldOblique", "Courier New,BoldItalic");
    StdFontsAssistant.RegisterAlternativeName("Courier-Oblique", "Courier New,Italic");
    StdFontsAssistant.RegisterAlternativeName("Times-Roman", "Times New Roman");
    StdFontsAssistant.RegisterAlternativeName("Times-Bold", "Times New Roman,Bold");
    StdFontsAssistant.RegisterAlternativeName("Times-BoldItalic", "Times New Roman,BoldItalic");
    StdFontsAssistant.RegisterAlternativeName("Times-Italic", "Times New Roman,Italic");
  }

  private static string GetFontStylesFromFontName(string fontName)
  {
    string[] strArray = fontName.Split(Chars.FontFamilyDelimiters);
    return strArray.Length > 1 ? strArray[1] : string.Empty;
  }

  private static void RegisterAlternativeName(string original, params string[] alternatives)
  {
    for (int index = 0; index < alternatives.Length; ++index)
    {
      string alternative = alternatives[index];
      StdFontsAssistant.alternativeNames[alternative] = original;
    }
  }

  private static Type1FontSource CreateFontSource(byte[] data)
  {
    try
    {
      return new Type1FontSource(data);
    }
    catch
    {
      throw new InvalidOperationException("Cannot create standard font.");
    }
  }

  private void InitializeStandardFontSources()
  {
    if (StdFontsAssistant.standardFontStreams == null || StdFontsAssistant.standardFontStreams.Count != 0)
      return;
    StdFontsAssistant.standardFontStreams = new Dictionary<string, byte[]>();
    StdFontsAssistant.RegisterStandardFontStream("Courier", StdFontsAssistant.GetApplicationResourceStream("Courier"));
    StdFontsAssistant.RegisterStandardFontStream("Courier-Bold", StdFontsAssistant.GetApplicationResourceStream("Courier-Bold"));
    StdFontsAssistant.RegisterStandardFontStream("Courier-BoldOblique", StdFontsAssistant.GetApplicationResourceStream("Courier-BoldOblique"));
    StdFontsAssistant.RegisterStandardFontStream("Courier-Oblique", StdFontsAssistant.GetApplicationResourceStream("Courier-Oblique"));
    StdFontsAssistant.RegisterStandardFontStream("Helvetica", StdFontsAssistant.GetApplicationResourceStream("Helvetica"));
    StdFontsAssistant.RegisterStandardFontStream("Helvetica-Bold", StdFontsAssistant.GetApplicationResourceStream("Helvetica-Bold"));
    StdFontsAssistant.RegisterStandardFontStream("Helvetica-BoldOblique", StdFontsAssistant.GetApplicationResourceStream("Helvetica-BoldOblique"));
    StdFontsAssistant.RegisterStandardFontStream("Helvetica-Oblique", StdFontsAssistant.GetApplicationResourceStream("Helvetica-Oblique"));
    StdFontsAssistant.RegisterStandardFontStream("Times-Roman", StdFontsAssistant.GetApplicationResourceStream("Times-Roman"));
    StdFontsAssistant.RegisterStandardFontStream("Times-Bold", StdFontsAssistant.GetApplicationResourceStream("Times-Bold"));
    StdFontsAssistant.RegisterStandardFontStream("Times-BoldItalic", StdFontsAssistant.GetApplicationResourceStream("Times-BoldItalic"));
    StdFontsAssistant.RegisterStandardFontStream("Times-Italic", StdFontsAssistant.GetApplicationResourceStream("Times-Italic"));
    StdFontsAssistant.RegisterStandardFontStream("Symbol", StdFontsAssistant.GetApplicationResourceStream("Symbol"));
    StdFontsAssistant.RegisterStandardFontStream("ZapfDingbats", StdFontsAssistant.GetApplicationResourceStream("ZapfDingbats"));
    StdFontsAssistant.InitializeStandardFontDescriptors();
    StdFontsAssistant.InitializeAlternativeNames();
  }

  private void RegisterStandardFontSource(string fontName, Type1FontSource fontSource)
  {
    this.standardFontSources[fontName] = fontSource;
  }

  public static void GetFontFamily(string fontName, out string fontFamily, out FontStyle fontStyle)
  {
    if (!PredefinedFontFamilies.TryGetFontFamily(fontName, out fontFamily))
      fontFamily = StdFontsAssistant.StripFontName(fontName);
    string lower = StdFontsAssistant.GetFontStylesFromFontName(fontName).ToLower();
    fontStyle = FontStyle.Regular;
    if (StdFontsAssistant.IsBold(lower) || lower.Contains("black"))
      fontStyle = FontStyle.Bold;
    if (!StdFontsAssistant.IsItalic(lower))
      return;
    fontStyle |= FontStyle.Italic;
  }

  internal void Dispose()
  {
    if (StdFontsAssistant.standardFontDescriptors != null && StdFontsAssistant.standardFontDescriptors.Count > 0)
      StdFontsAssistant.standardFontDescriptors.Clear();
    if (StdFontsAssistant.alternativeNames != null && StdFontsAssistant.alternativeNames.Count > 0)
      StdFontsAssistant.alternativeNames.Clear();
    if (StdFontsAssistant.standardFontStreams != null && StdFontsAssistant.standardFontStreams.Count > 0)
      StdFontsAssistant.standardFontStreams.Clear();
    if (this.standardFontSources.Count <= 0)
      return;
    this.standardFontSources.Clear();
  }
}
