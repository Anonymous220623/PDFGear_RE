// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.UnicodeTrueTypeFont
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Microsoft.Win32;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Native;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class UnicodeTrueTypeFont : ITrueTypeFont
{
  private const string c_driverName = "DISPLAY";
  private const string c_nameString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
  private const string c_cmapPrefix = "/CIDInit /ProcSet findresource begin\n12 dict begin\nbegincmap\r\n/CIDSystemInfo << /Registry (Adobe)/Ordering (UCS)/Supplement 0>> def\n/CMapName /Adobe-Identity-UCS def\n/CMapType 2 def\n1 begincodespacerange\r\n";
  private const string c_cmapEndCodespaceRange = "endcodespacerange\r\n";
  private const string c_cmapSuffix = "endbfrange\nendcmap\nCMapName currentdict /CMap defineresource pop\nend end\r\n";
  private const string c_cmapBeginRange = "beginbfrange\r\n";
  private const string c_cmapEndRange = "endbfrange\r\n";
  private const int c_cmapNextRangeValue = 100;
  private const string c_registry = "Adobe";
  private const int c_defWidthIndex = 32 /*0x20*/;
  private const int c_cidStreamLength = 11;
  private static object s_syncLock = new object();
  private Stream m_fontStream;
  internal bool is_filePath;
  private Font m_font;
  private string m_filePath;
  private float m_size;
  private PdfFontMetrics m_metrics;
  private PdfDictionary m_fontDictionary;
  private PdfDictionary m_descendantFont;
  private PdfDictionary m_fontDescriptor;
  private PdfStream m_fontProgram;
  private PdfStream m_cmap;
  private PdfStream m_CidStream;
  private TtfReader m_ttfReader;
  private Dictionary<int, OtfGlyphInfo> m_openTypeGlyphs;
  private List<TtfGlyphInfo> glyphInfo;
  internal bool m_isIncreasedUsedChar;
  private ConcurrentDictionary<char, char> m_usedChars;
  private string m_subsetName;
  internal TtfMetrics m_ttfMetrics;
  private CompositeFontType m_type;
  private string metricsName = string.Empty;
  private bool m_isEmbedFont;
  private bool m_isAzureCompatible;
  private bool m_isFontFilePath;
  private bool m_isCompress;
  private bool m_isSkipFontEmbed;
  internal bool conformanceEnabled;
  internal bool m_isXPSFontStream;

  public float Size => this.m_size;

  Font ITrueTypeFont.Font => this.m_font;

  internal bool IsEmbed
  {
    get => this.m_isEmbedFont;
    set => this.m_isEmbedFont = value;
  }

  public PdfFontMetrics Metrics => this.m_metrics;

  internal TtfReader TtfReader => this.m_ttfReader;

  internal string FontFile => this.m_filePath;

  internal TtfMetrics TtfMetrics => this.m_ttfMetrics;

  internal CompositeFontType FontType
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  internal bool SkipFontEmbed
  {
    get => this.m_isSkipFontEmbed;
    set => this.m_isSkipFontEmbed = value;
  }

  public UnicodeTrueTypeFont(Font font, float size, CompositeFontType type)
    : this(font, size, type, false)
  {
  }

  internal UnicodeTrueTypeFont(
    Font font,
    float size,
    CompositeFontType type,
    bool isAzureCompatible)
  {
    this.m_font = font != null ? font : throw new ArgumentNullException(nameof (font));
    this.m_size = size;
    this.m_type = type;
    this.m_isAzureCompatible = isAzureCompatible;
    this.Initialize();
  }

  public UnicodeTrueTypeFont(string filePath, float size, CompositeFontType type)
  {
    switch (filePath)
    {
      case null:
        throw new ArgumentNullException(nameof (filePath));
      case "":
        throw new ArgumentException("filePath - string can not be empty");
      default:
        this.m_filePath = filePath;
        this.m_size = size;
        this.m_type = type;
        this.Initialize();
        break;
    }
  }

  internal UnicodeTrueTypeFont(Stream font, float size, CompositeFontType type, string name)
  {
    this.metricsName = name;
    this.m_fontStream = font != null ? font : throw new ArgumentNullException(nameof (font));
    this.m_size = size;
    this.m_type = type;
    byte[] buffer = new byte[font.Length];
    font.Read(buffer, 0, buffer.Length);
    using (MemoryStream font1 = new MemoryStream(buffer))
      this.Initialize((Stream) font1);
  }

  public UnicodeTrueTypeFont(Stream font, float size, CompositeFontType type)
  {
    this.m_fontStream = font != null ? font : throw new ArgumentNullException(nameof (font));
    this.m_size = size;
    this.m_type = type;
    byte[] buffer = new byte[font.Length];
    font.Read(buffer, 0, buffer.Length);
    using (MemoryStream font1 = new MemoryStream(buffer))
      this.Initialize((Stream) font1);
  }

  public UnicodeTrueTypeFont(UnicodeTrueTypeFont prototype)
  {
    this.m_ttfReader = prototype != null ? prototype.m_ttfReader : throw new ArgumentNullException(nameof (prototype));
    this.m_fontStream = prototype.m_fontStream;
    this.m_ttfMetrics = prototype.TtfMetrics;
    this.m_font = ((ITrueTypeFont) prototype).Font;
    this.m_filePath = prototype.FontFile;
    this.m_size = prototype.Size;
  }

  public void SetSymbols(string text)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (this.m_usedChars == null)
      this.m_usedChars = new ConcurrentDictionary<char, char>();
    for (int index = 0; index < text.Length; ++index)
      this.m_usedChars[text[index]] = char.MinValue;
  }

  public void SetSymbols(ushort[] glyphs)
  {
    if (glyphs == null)
      throw new ArgumentNullException(nameof (glyphs));
    if (this.m_usedChars == null)
      this.m_usedChars = new ConcurrentDictionary<char, char>();
    for (int index = 0; index < glyphs.Length; ++index)
    {
      TtfGlyphInfo glyph = this.m_ttfReader.GetGlyph((int) glyphs[index]);
      if (!glyph.Empty)
        this.m_usedChars[(char) glyph.CharCode] = char.MinValue;
    }
    if (this.m_isEmbedFont)
      return;
    this.GetDescendantWidth();
  }

  internal void SetSymbols(string text, bool opentype)
  {
    if (this.m_openTypeGlyphs == null)
      this.m_openTypeGlyphs = new Dictionary<int, OtfGlyphInfo>();
    foreach (char charCode in text)
    {
      TtfGlyphInfo glyph = this.m_ttfReader.GetGlyph(charCode);
      if (glyph.Index > -1)
        this.m_openTypeGlyphs[glyph.Index] = new OtfGlyphInfo(glyph.CharCode, glyph.Index, (float) glyph.Width);
    }
  }

  internal void SetSymbols(ushort[] glyphs, bool openType)
  {
    if (this.m_openTypeGlyphs == null)
      this.m_openTypeGlyphs = new Dictionary<int, OtfGlyphInfo>();
    for (int index = 0; index < glyphs.Length; ++index)
    {
      TtfGlyphInfo glyph = this.m_ttfReader.GetGlyph((int) glyphs[index]);
      if (!glyph.Empty && glyph.Index > -1)
        this.m_openTypeGlyphs[glyph.Index] = new OtfGlyphInfo(glyph.CharCode, glyph.Index, (float) glyph.Width);
    }
  }

  internal void SetSymbols(OtfGlyphInfoList line)
  {
    if (this.m_openTypeGlyphs == null)
      this.m_openTypeGlyphs = new Dictionary<int, OtfGlyphInfo>();
    foreach (OtfGlyphInfo glyph in line.Glyphs)
      this.m_openTypeGlyphs[glyph.Index] = glyph;
  }

  public IPdfPrimitive GetInternals() => (IPdfPrimitive) this.m_fontDictionary;

  public bool EqualsToFont(PdfFont font)
  {
    bool font1 = false;
    if (font is PdfTrueTypeFont pdfTrueTypeFont && pdfTrueTypeFont.Unicode)
    {
      if (this.m_font != null && pdfTrueTypeFont.InternalFont.Font != null)
      {
        bool flag1 = this.m_font.Name.Equals(pdfTrueTypeFont.InternalFont.Font.Name);
        bool flag2 = this.m_font.Style == pdfTrueTypeFont.InternalFont.Font.Style;
        font1 = flag1 && flag2;
      }
      else
      {
        UnicodeTrueTypeFont internalFont = (UnicodeTrueTypeFont) pdfTrueTypeFont.InternalFont;
        bool flag3 = this.m_ttfMetrics.FontFamily.Equals(internalFont.m_ttfMetrics.FontFamily);
        bool flag4 = this.m_ttfMetrics.MacStyle == internalFont.m_ttfMetrics.MacStyle;
        font1 = flag3 && flag4;
        if (font1)
        {
          bool flag5 = this.IsEqualFontStream(this.m_fontStream, internalFont.m_fontStream);
          font1 = font1 && flag5;
        }
      }
    }
    else if (pdfTrueTypeFont != null && this.SkipFontEmbed)
    {
      UnicodeTrueTypeFont internalFont = (UnicodeTrueTypeFont) pdfTrueTypeFont.InternalFont;
      bool flag6 = this.m_ttfMetrics.FontFamily.Equals(internalFont.m_ttfMetrics.FontFamily);
      bool flag7 = this.m_ttfMetrics.MacStyle == internalFont.m_ttfMetrics.MacStyle;
      font1 = flag6 && flag7;
      if (font1)
      {
        bool flag8 = this.IsEqualFontStream(this.m_fontStream, internalFont.m_fontStream);
        font1 = font1 && flag8;
      }
    }
    return font1;
  }

  private bool IsEqualFontStream(Stream currentFont, Stream previousFont)
  {
    if (currentFont == null || !currentFont.CanRead || previousFont == null || !previousFont.CanRead || currentFont.Length != previousFont.Length)
      return false;
    currentFont.Position = 0L;
    previousFont.Position = 0L;
    long num1 = currentFont.Length > 1024L /*0x0400*/ ? 1024L /*0x0400*/ : currentFont.Length;
    for (int index = 0; (long) index < num1; ++index)
    {
      int num2 = currentFont.ReadByte();
      if (currentFont.Position == previousFont.Position)
        return true;
      int num3 = previousFont.ReadByte();
      if (num2.CompareTo(num3) != 0)
        return false;
    }
    return true;
  }

  public void CreateInternals()
  {
    this.m_fontDictionary = new PdfDictionary();
    this.m_fontProgram = new PdfStream();
    this.m_cmap = new PdfStream();
    if (PdfDocument.ConformanceLevel != PdfConformanceLevel.None && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_X1A2001)
      this.m_CidStream = new PdfStream();
    this.m_descendantFont = new PdfDictionary();
    this.m_metrics = new PdfFontMetrics();
    if (!this.m_isFontFilePath)
    {
      this.m_ttfReader.Reader = this.GetFontData();
    }
    else
    {
      string fontFile = this.GetFontFile(this.m_font);
      if (fontFile != null)
      {
        this.m_ttfReader.Reader = new BinaryReader((Stream) new FileStream(fontFile, FileMode.Open, FileAccess.Read, FileShare.Read), TtfReader.Encoding);
        this.m_isFontFilePath = false;
      }
    }
    this.m_ttfReader.CreateInternals();
    this.m_ttfMetrics = this.m_ttfReader.Metrics;
    this.InitializeMetrics();
    this.m_subsetName = this.GetFontName();
    if (PdfDocument.ConformanceLevel != PdfConformanceLevel.None && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_X1A2001)
      this.CreateCidSet();
    this.CreateDescendantFont();
    this.CreateCmap();
    this.CreateFontDictionary();
    this.CreateFontProgram();
  }

  public int GetCharWidth(char charCode) => this.m_ttfReader.GetCharWidth(charCode);

  public int GetLineWidth(string line)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    int lineWidth = 0;
    int index = 0;
    for (int length = line.Length; index < length; ++index)
    {
      int charWidth = this.GetCharWidth(line[index]);
      lineWidth += charWidth;
    }
    return lineWidth;
  }

  public void Close()
  {
    if (this.m_fontDictionary != null)
    {
      if (!this.SkipFontEmbed)
        this.m_fontDictionary.BeginSave -= new SavePdfPrimitiveEventHandler(this.FontDictionaryBeginSave);
      this.m_fontDictionary.Clear();
      this.m_fontDictionary = (PdfDictionary) null;
    }
    if (this.m_fontDescriptor != null)
    {
      if (PdfDocument.ConformanceLevel != PdfConformanceLevel.None && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_X1A2001)
        this.m_fontDescriptor.BeginSave -= new SavePdfPrimitiveEventHandler(this.FontDescriptorBeginSave);
      this.m_fontDescriptor.Clear();
      this.m_fontDescriptor = (PdfDictionary) null;
    }
    if (this.m_descendantFont != null)
    {
      this.m_descendantFont.BeginSave -= new SavePdfPrimitiveEventHandler(this.DescendantFontBeginSave);
      this.m_descendantFont.Clear();
      this.m_descendantFont = (PdfDictionary) null;
    }
    if (this.m_fontProgram != null)
    {
      this.m_fontProgram.BeginSave -= new SavePdfPrimitiveEventHandler(this.FontProgramBeginSave);
      this.m_fontProgram.Clear();
      this.m_fontProgram = (PdfStream) null;
    }
    if (this.m_cmap != null)
    {
      this.m_cmap.BeginSave -= new SavePdfPrimitiveEventHandler(this.CmapBeginSave);
      this.m_cmap.Clear();
      this.m_cmap = (PdfStream) null;
    }
    if (this.m_CidStream != null)
    {
      this.m_CidStream.BeginSave -= new SavePdfPrimitiveEventHandler(this.CidBeginSave);
      this.m_CidStream.Clear();
      this.m_CidStream = (PdfStream) null;
    }
    if (this.m_ttfReader != null)
    {
      this.m_ttfReader.Close();
      this.m_ttfReader = (TtfReader) null;
    }
    if (this.m_usedChars != null)
    {
      this.m_usedChars.Clear();
      this.m_usedChars = (ConcurrentDictionary<char, char>) null;
    }
    if (this.m_fontStream != null)
      this.m_fontStream = (Stream) null;
    this.m_filePath = (string) null;
    this.m_metrics = (PdfFontMetrics) null;
    this.m_subsetName = (string) null;
  }

  private void Initialize()
  {
    using (BinaryReader fontData = this.GetFontData())
    {
      if (fontData == null && this.m_isAzureCompatible)
      {
        this.m_font = new Font("Micorsoft Sans Serif", this.m_font.Size, this.m_font.Style);
        this.m_ttfReader = new TtfReader(this.GetFontData(), this.m_font, this.m_isAzureCompatible);
      }
      else
        this.m_ttfReader = new TtfReader(fontData, this.m_font, this.m_isAzureCompatible);
      this.m_ttfMetrics = this.m_ttfReader.Metrics;
      if (this.m_font == null || !(this.m_ttfMetrics.FontFamily != this.m_font.FontFamily.Name.ToString()))
        return;
      string fontFile = this.GetFontFile(this.m_font);
      if (fontFile == null)
        return;
      this.m_isFontFilePath = true;
      this.m_ttfReader = new TtfReader(new BinaryReader((Stream) new FileStream(fontFile, FileMode.Open, FileAccess.Read, FileShare.Read), TtfReader.Encoding), this.m_font);
      this.m_ttfMetrics = this.m_ttfReader.Metrics;
    }
  }

  private BinaryReader GetFontData()
  {
    Stream input;
    if (this.m_font != null)
    {
      if (!this.m_isAzureCompatible)
      {
        input = this.GetFontData(this.m_font);
      }
      else
      {
        input = this.ReadFile(this.m_font);
        if (input == null)
          return (BinaryReader) null;
      }
    }
    else if (this.m_fontStream != null)
    {
      input = this.m_fontStream;
      if (input.CanRead)
        input.Position = 0L;
    }
    else
    {
      try
      {
        input = (Stream) new FileStream(this.m_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
      }
      catch (Exception ex)
      {
        throw new Exception($"Cannot open file: {this.m_filePath} for reading.");
      }
    }
    return new BinaryReader(input, TtfReader.Encoding);
  }

  private Stream ReadFile(Font font)
  {
    string fontFile = this.GetFontFile(this.m_font);
    return fontFile != null && fontFile != string.Empty ? (Stream) new FileStream(fontFile, FileMode.Open, FileAccess.Read, FileShare.Read) : (Stream) null;
  }

  private string GetFontFile(Font font)
  {
    string name1 = font.Name;
    if (font.Bold)
      name1 += " Bold";
    if (font.Italic)
      name1 += " Italic";
    string name2 = name1 + " (TrueType)";
    RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts") ?? Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Fonts");
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
    string path2 = registryKey.GetValue(name2, (object) string.Empty) as string;
    if (string.IsNullOrEmpty(path2))
    {
      path2 = registryKey.GetValue(font.Name + " (TrueType)", (object) string.Empty) as string;
      if (this.m_isAzureCompatible && path2 == string.Empty)
      {
        string str = "";
        Regex regex = new Regex($"^(?:.+ & )?{Regex.Escape(font.Name)}(?: & .+)?(?<suffix>{str}) \\(TrueType\\)$", RegexOptions.Compiled);
        foreach (string valueName in registryKey.GetValueNames())
        {
          Match match = regex.Match(valueName);
          if (match.Success)
          {
            name2 = match.Value;
            break;
          }
        }
        path2 = registryKey.GetValue(name2, (object) string.Empty) as string;
      }
    }
    return path2 != null && path2 != string.Empty ? Path.Combine(folderPath, path2) : (string) null;
  }

  private Stream GetFontData(Font font)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (PdfDocument.EnableCache && PdfDocument.Cache.FontData.ContainsKey(font))
      return (Stream) new MemoryStream(PdfDocument.Cache.FontData[font]);
    IntPtr dc = GdiApi.CreateDC("DISPLAY", (string) null, (string) null, IntPtr.Zero);
    IntPtr hfont = font.ToHfont();
    IntPtr hgdiobj = GdiApi.SelectObject(dc, hfont);
    uint fontData = GdiApi.GetFontData(dc, 0U, 0U, (byte[]) null, 0U);
    if (fontData == uint.MaxValue)
    {
      int lastError = (int) KernelApi.GetLastError();
      throw new PdfException("Can't parse the font");
    }
    byte[] numArray = new byte[(IntPtr) fontData];
    if (GdiApi.GetFontData(dc, 0U, 0U, numArray, fontData) == uint.MaxValue)
    {
      int lastError = (int) KernelApi.GetLastError();
      throw new PdfException("Can't parse the font");
    }
    GdiApi.SelectObject(dc, hgdiobj);
    GdiApi.DeleteObject(hfont);
    GdiApi.DeleteDC(dc);
    if (PdfDocument.EnableCache)
    {
      lock (PdfDocument.Cache)
      {
        if (!PdfDocument.Cache.FontData.ContainsKey(font))
          PdfDocument.Cache.FontData.Add(font, numArray);
      }
    }
    return (Stream) new MemoryStream(numArray, 0, numArray.Length, false);
  }

  private void Initialize(Stream font)
  {
    using (BinaryReader reader = new BinaryReader(font, TtfReader.Encoding))
    {
      this.m_ttfReader = !(this.metricsName != string.Empty) ? new TtfReader(reader) : new TtfReader(reader, this.metricsName);
      this.m_ttfMetrics = this.m_ttfReader.Metrics;
    }
  }

  private void InitializeMetrics()
  {
    TtfMetrics metrics = this.m_ttfReader.Metrics;
    this.m_metrics.Ascent = this.m_font == null || !(this.m_font.Name == "Optima LT") || (double) metrics.WinAscent == (double) metrics.MacAscent ? metrics.MacAscent : metrics.WinAscent;
    this.m_metrics.Descent = metrics.MacDescent;
    this.m_metrics.Height = metrics.MacAscent - metrics.MacDescent + (float) metrics.LineGap;
    this.m_metrics.Name = metrics.FontFamily;
    this.m_metrics.PostScriptName = metrics.PostScriptName;
    this.m_metrics.Size = this.m_size;
    this.m_metrics.WidthTable = (WidthTable) new StandardWidthTable(metrics.WidthTable);
    this.m_metrics.LineGap = metrics.LineGap;
    this.m_metrics.SubScriptSizeFactor = metrics.SubScriptSizeFactor;
    this.m_metrics.SuperscriptSizeFactor = metrics.SuperscriptSizeFactor;
    this.m_metrics.IsBold = metrics.IsBold;
  }

  private void CreateFontProgram()
  {
    this.m_fontProgram.BeginSave += new SavePdfPrimitiveEventHandler(this.FontProgramBeginSave);
  }

  private void GenerateFontProgram()
  {
    this.m_usedChars = this.m_usedChars == null ? new ConcurrentDictionary<char, char>() : this.m_usedChars;
    this.m_ttfReader.InternalReader.Seek(0L);
    byte[] numArray;
    if (this.m_type == CompositeFontType.Type0 && (this.m_ttfReader.Font != null || this.m_isCompress || this.m_ttfReader.Font == null && this.m_usedChars.Count > 0 && !this.m_isXPSFontStream) && this.m_openTypeGlyphs == null)
    {
      if (this.m_ttfMetrics.ContainsCFF && this.m_ttfReader.isOpenTypeFont)
      {
        Stream stream = this.TtfReader.ReadCffTable();
        numArray = new byte[stream.Length];
        stream.Read(numArray, 0, numArray.Length);
        this.m_fontProgram["Subtype"] = (IPdfPrimitive) new PdfName("CIDFontType0C");
      }
      else
        numArray = this.m_ttfReader.ReadFontProgram(this.m_usedChars);
      if (this.m_isCompress)
        numArray = new PdfOptimizer().OptimizeType0Font(new MemoryStream(numArray), this.m_usedChars);
      this.m_isXPSFontStream = false;
    }
    else if (this.m_type == CompositeFontType.Type0 && this.m_openTypeGlyphs != null && this.m_openTypeGlyphs.Count > 0)
    {
      if (this.m_ttfMetrics.ContainsCFF && this.m_ttfReader.isOpenTypeFont)
      {
        Stream stream = this.TtfReader.ReadCffTable();
        numArray = new byte[stream.Length];
        stream.Read(numArray, 0, numArray.Length);
        this.m_fontProgram["Subtype"] = (IPdfPrimitive) new PdfName("CIDFontType0C");
      }
      else
        numArray = this.m_ttfReader.ReadOpenTypeFontProgram(this.m_openTypeGlyphs);
    }
    else if (this.m_ttfMetrics.ContainsCFF && this.m_ttfReader.isOpenTypeFont)
    {
      Stream stream = this.TtfReader.ReadCffTable();
      numArray = new byte[stream.Length];
      stream.Read(numArray, 0, numArray.Length);
      this.m_fontProgram["Subtype"] = (IPdfPrimitive) new PdfName("CIDFontType0C");
    }
    else
    {
      Stream baseStream = this.GetFontData().BaseStream;
      numArray = new byte[baseStream.Length];
      this.m_fontProgram["Length1"] = (IPdfPrimitive) new PdfNumber(numArray.Length);
      baseStream.Read(numArray, 0, (int) baseStream.Length - 1);
      if (PdfDocument.EnableCache && this.m_fontStream == null)
        baseStream.Dispose();
    }
    this.m_fontProgram.Clear();
    this.m_fontProgram.Write(numArray);
  }

  private void CreateFontDictionary()
  {
    this.m_fontDictionary.IsFont = true;
    if (this.SkipFontEmbed)
      this.m_fontDictionary["Widths"] = (IPdfPrimitive) new PdfArray(this.m_ttfMetrics.WidthTable);
    else
      this.m_fontDictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.FontDictionaryBeginSave);
    this.m_fontDictionary["Type"] = (IPdfPrimitive) new PdfName("Font");
    this.m_fontDictionary["BaseFont"] = (IPdfPrimitive) new PdfName(this.m_subsetName);
    if (this.m_type == CompositeFontType.Type0)
    {
      this.m_fontDictionary["Subtype"] = (IPdfPrimitive) new PdfName("Type0");
      this.m_fontDictionary["Encoding"] = (IPdfPrimitive) new PdfName("Identity-H");
      PdfArray pdfArray = new PdfArray();
      PdfReferenceHolder element = new PdfReferenceHolder((IPdfPrimitive) this.m_descendantFont);
      pdfArray.IsFont = true;
      pdfArray.Add((IPdfPrimitive) element);
      this.m_fontDictionary["DescendantFonts"] = (IPdfPrimitive) pdfArray;
    }
    else
    {
      this.m_fontDictionary["Name"] = (IPdfPrimitive) new PdfName(this.m_subsetName);
      this.m_fontDictionary["Subtype"] = (IPdfPrimitive) new PdfName("TrueType");
      this.m_fontDictionary["Encoding"] = (IPdfPrimitive) new PdfName("WinAnsiEncoding");
      this.m_fontDictionary["Widths"] = (IPdfPrimitive) new PdfArray(this.m_ttfMetrics.WidthTable);
      this.m_fontDictionary["FirstChar"] = (IPdfPrimitive) new PdfNumber(0);
      this.m_fontDictionary["LastChar"] = (IPdfPrimitive) new PdfNumber((int) byte.MaxValue);
      this.m_fontDictionary["FontDescriptor"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.CreateFontDescriptor());
    }
  }

  private void CreateDescendantFont()
  {
    this.m_descendantFont.IsFont = true;
    this.m_descendantFont.BeginSave += new SavePdfPrimitiveEventHandler(this.DescendantFontBeginSave);
    this.m_descendantFont["Type"] = (IPdfPrimitive) new PdfName("Font");
    string str = "CIDFontType2";
    if (this.TtfReader != null && this.TtfReader.isOpenTypeFont)
      str = "CIDFontType0";
    this.m_descendantFont["Subtype"] = (IPdfPrimitive) new PdfName(str);
    this.m_descendantFont["BaseFont"] = (IPdfPrimitive) new PdfName(this.m_subsetName);
    this.m_descendantFont["CIDToGIDMap"] = (IPdfPrimitive) new PdfName("Identity");
    this.m_descendantFont["DW"] = (IPdfPrimitive) new PdfNumber(1000);
    this.m_fontDescriptor = this.CreateFontDescriptor();
    if (PdfDocument.ConformanceLevel != PdfConformanceLevel.None && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_X1A2001)
      this.m_fontDescriptor.BeginSave += new SavePdfPrimitiveEventHandler(this.FontDescriptorBeginSave);
    this.m_descendantFont["FontDescriptor"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_fontDescriptor);
    this.m_descendantFont["CIDSystemInfo"] = this.CreateSystemInfo();
  }

  internal int GetUsedCharsCount() => this.m_usedChars.Count;

  internal void SetGlyphInfo(List<TtfGlyphInfo> collection) => this.glyphInfo = collection;

  private void CreateCmap()
  {
    this.m_cmap.BeginSave += new SavePdfPrimitiveEventHandler(this.CmapBeginSave);
  }

  private void CreateCidSet()
  {
    this.m_CidStream.BeginSave += new SavePdfPrimitiveEventHandler(this.CidBeginSave);
  }

  private void GenerateCmap()
  {
    if (this.m_usedChars != null && this.m_usedChars.Count > 0 && this.m_openTypeGlyphs == null)
    {
      Dictionary<int, int> glyphChars = this.m_ttfReader.GetGlyphChars(this.m_usedChars);
      if (glyphChars.Count <= 0)
        return;
      int[] array = new int[glyphChars.Count];
      glyphChars.Keys.CopyTo(array, 0);
      Array.Sort<int>(array);
      List<int> intList = new List<int>(glyphChars.Keys.Count);
      intList.AddRange((IEnumerable<int>) glyphChars.Keys);
      Array.Sort<int>(intList.ToArray());
      int n1 = array[0];
      int n2 = array[array.Length - 1];
      string str = $"{this.ToHexString(n1)}{this.ToHexString(n2)}\r\n";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("/CIDInit /ProcSet findresource begin\n12 dict begin\nbegincmap\r\n/CIDSystemInfo << /Registry (Adobe)/Ordering (UCS)/Supplement 0>> def\n/CMapName /Adobe-Identity-UCS def\n/CMapType 2 def\n1 begincodespacerange\r\n");
      stringBuilder.Append(str);
      stringBuilder.Append("endcodespacerange\r\n");
      int num = 0;
      int index = 0;
      for (int length = array.Length; index < length; ++index)
      {
        if (num == 0)
        {
          if (index != 0)
            stringBuilder.Append("endbfrange\r\n");
          num = Math.Min(100, array.Length - index);
          stringBuilder.Append(num);
          stringBuilder.Append(" ");
          stringBuilder.Append("beginbfrange\r\n");
        }
        --num;
        int key = array[index];
        stringBuilder.AppendFormat("<{0:X04}><{0:X04}><{1:X04}>\n", (object) key, (object) glyphChars[key]);
      }
      stringBuilder.Append("endbfrange\nendcmap\nCMapName currentdict /CMap defineresource pop\nend end\r\n");
      this.m_cmap.Clear();
      this.m_cmap.Write(stringBuilder.ToString());
    }
    else
      this.GenerateOpenTypeCmap();
  }

  private void GenerateOpenTypeCmap()
  {
    this.UpdateOpenTypeGlyphs();
    if (this.m_openTypeGlyphs == null || this.m_openTypeGlyphs.Count <= 0)
      return;
    int[] array = new int[this.m_openTypeGlyphs.Count];
    this.m_openTypeGlyphs.Keys.CopyTo(array, 0);
    Array.Sort<int>(array);
    int n1 = array[0];
    int n2 = array[array.Length - 1];
    string str = $"{this.ToHexString(n1)}{this.ToHexString(n2)}\r\n";
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.Append("/CIDInit /ProcSet findresource begin\n12 dict begin\nbegincmap\r\n/CIDSystemInfo << /Registry (Adobe)/Ordering (UCS)/Supplement 0>> def\n/CMapName /Adobe-Identity-UCS def\n/CMapType 2 def\n1 begincodespacerange\r\n");
    stringBuilder1.Append(str);
    stringBuilder1.Append("endcodespacerange\r\n");
    int num = 0;
    int index = 0;
    for (int length = array.Length; index < length; ++index)
    {
      if (num == 0)
      {
        if (index != 0)
          stringBuilder1.Append("endbfrange\r\n");
        num = Math.Min(100, array.Length - index);
        stringBuilder1.Append(num);
        stringBuilder1.Append(" ");
        stringBuilder1.Append("beginbfrange\r\n");
      }
      --num;
      int key = array[index];
      stringBuilder1.AppendFormat("<{0:X04}><{0:X04}>", (object) key);
      OtfGlyphInfo openTypeGlyph = this.m_openTypeGlyphs[key];
      if (openTypeGlyph.CharCode != -1)
      {
        stringBuilder1.AppendFormat("<{0:X04}>\n", (object) openTypeGlyph.CharCode);
      }
      else
      {
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append("<");
        if (openTypeGlyph.Characters != null)
        {
          foreach (char character in openTypeGlyph.Characters)
            stringBuilder2.AppendFormat("{0:X04}", (object) (int) character);
        }
        stringBuilder2.Append(">\n");
        stringBuilder1.Append(stringBuilder2.ToString());
      }
    }
    stringBuilder1.Append("endbfrange\nendcmap\nCMapName currentdict /CMap defineresource pop\nend end\r\n");
    this.m_cmap.Clear();
    this.m_cmap.Write(stringBuilder1.ToString());
  }

  private IPdfPrimitive CreateSystemInfo()
  {
    return (IPdfPrimitive) new PdfDictionary()
    {
      ["Registry"] = (IPdfPrimitive) new PdfString("Adobe"),
      ["Ordering"] = (IPdfPrimitive) new PdfString("Identity"),
      ["Supplement"] = (IPdfPrimitive) new PdfNumber(0)
    };
  }

  private PdfDictionary CreateFontDescriptor()
  {
    PdfDictionary fontDescriptor = new PdfDictionary();
    TtfMetrics metrics = this.m_ttfReader.Metrics;
    fontDescriptor.IsFont = true;
    fontDescriptor["Type"] = (IPdfPrimitive) new PdfName("FontDescriptor");
    fontDescriptor["FontName"] = (IPdfPrimitive) new PdfName(this.m_subsetName);
    fontDescriptor["Flags"] = (IPdfPrimitive) new PdfNumber(this.GetDescriptorFlags());
    fontDescriptor["FontBBox"] = (IPdfPrimitive) PdfArray.FromRectangle(this.GetBoundBox());
    fontDescriptor["MissingWidth"] = (IPdfPrimitive) new PdfNumber(metrics.WidthTable[32 /*0x20*/]);
    fontDescriptor["StemV"] = (IPdfPrimitive) new PdfNumber(metrics.IsBold ? 160 /*0xA0*/ : (int) metrics.StemV);
    fontDescriptor["ItalicAngle"] = (IPdfPrimitive) new PdfNumber((int) metrics.ItalicAngle);
    fontDescriptor["CapHeight"] = (IPdfPrimitive) new PdfNumber((int) metrics.CapHeight);
    fontDescriptor["Ascent"] = (IPdfPrimitive) new PdfNumber((int) metrics.WinAscent);
    fontDescriptor["Descent"] = (IPdfPrimitive) new PdfNumber((int) metrics.WinDescent);
    fontDescriptor["Leading"] = (IPdfPrimitive) new PdfNumber((int) metrics.Leading);
    fontDescriptor["AvgWidth"] = (IPdfPrimitive) new PdfNumber(metrics.WidthTable[32 /*0x20*/]);
    if (!this.m_isSkipFontEmbed)
    {
      if (this.m_ttfMetrics.ContainsCFF)
        fontDescriptor["FontFile3"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_fontProgram);
      else
        fontDescriptor["FontFile2"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_fontProgram);
    }
    fontDescriptor["MaxWidth"] = (IPdfPrimitive) new PdfNumber(metrics.WidthTable[32 /*0x20*/]);
    fontDescriptor["XHeight"] = (IPdfPrimitive) new PdfNumber(0);
    fontDescriptor["StemH"] = (IPdfPrimitive) new PdfNumber(0);
    return fontDescriptor;
  }

  private string FormatName(string fontName)
  {
    if (fontName == null)
      throw new ArgumentNullException(nameof (fontName));
    return !(fontName == string.Empty) ? fontName.Replace("(", "#28").Replace(")", "#29").Replace("[", "#5B").Replace("]", "#5D").Replace("<", "#3C").Replace(">", "#3E").Replace("{", "#7B").Replace("}", "#7D").Replace("/", "#2F").Replace("%", "#25").Replace(" ", "#20") : throw new ArgumentOutOfRangeException(nameof (fontName), "Parameter can not be empty");
  }

  private string GetFontName()
  {
    StringBuilder stringBuilder = new StringBuilder();
    SecureRandomAlgorithm secureRandomAlgorithm = new SecureRandomAlgorithm();
    bool resourceNaming = PdfDocument.m_resourceNaming;
    if (this.m_type == CompositeFontType.Type0)
    {
      if (!this.m_isEmbedFont)
      {
        if (!resourceNaming)
        {
          for (int index1 = 0; index1 < 6; ++index1)
          {
            int index2 = secureRandomAlgorithm.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length);
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[index2]);
          }
        }
        else
        {
          stringBuilder.Append("BCD");
          string str = "";
          int count = PdfDocument.Cache.FontData.Count;
          int num = 0;
          if (count == 0)
          {
            str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[0].ToString() + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[0].ToString() + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[0].ToString();
          }
          else
          {
            for (char ch1 = 'A'; ch1 <= 'Z'; ++ch1)
            {
              for (char ch2 = 'A'; ch2 <= 'Z'; ++ch2)
              {
                for (char ch3 = 'A'; ch3 <= 'Z'; ++ch3)
                {
                  str = ch1.ToString() + ch2.ToString() + ch3.ToString();
                  ++num;
                  if (num == count)
                    break;
                }
                if (num == count)
                  break;
              }
              if (num == count)
                break;
            }
          }
          stringBuilder.Append(str);
        }
        stringBuilder.Append('+');
      }
      stringBuilder.Append(this.m_ttfReader.Metrics.PostScriptName);
    }
    else
      stringBuilder.Append(this.m_ttfReader.Metrics.PostScriptName);
    if (this.SkipFontEmbed)
    {
      stringBuilder = new StringBuilder();
      stringBuilder.Append(this.m_ttfReader.Metrics.PostScriptName);
    }
    string fontFamily = stringBuilder.ToString();
    if (fontFamily == string.Empty)
      fontFamily = this.m_ttfReader.Metrics.FontFamily;
    return this.FormatName(fontFamily);
  }

  public PdfArray GetDescendantWidth()
  {
    PdfArray descendantWidth;
    if (this.m_usedChars != null && this.m_usedChars.Count > 0 && this.m_openTypeGlyphs == null)
    {
      descendantWidth = new PdfArray();
      if (this.glyphInfo == null)
        this.glyphInfo = new List<TtfGlyphInfo>();
      if (!this.m_isEmbedFont)
      {
        foreach (KeyValuePair<char, char> usedChar in this.m_usedChars)
        {
          TtfGlyphInfo glyph = this.m_ttfReader.GetGlyph(usedChar.Key);
          if (!glyph.Empty)
            this.glyphInfo.Add(glyph);
        }
        this.glyphInfo = this.GetGlyphInfo();
      }
      else
        this.glyphInfo = this.m_isIncreasedUsedChar ? this.GetGlyphInfo() : this.m_ttfReader.GetAllGlyphs();
      this.glyphInfo.Sort();
      int num1 = 0;
      int num2 = 0;
      bool flag1 = false;
      PdfArray element = new PdfArray();
      if (!this.m_isEmbedFont)
      {
        int index = 0;
        for (int count = this.glyphInfo.Count; index < count; ++index)
        {
          TtfGlyphInfo ttfGlyphInfo = this.glyphInfo[index];
          if (!flag1)
          {
            flag1 = true;
            num1 = ttfGlyphInfo.Index;
            num2 = ttfGlyphInfo.Index - 1;
          }
          if ((num2 + 1 != ttfGlyphInfo.Index || index + 1 == count) && count > 1)
          {
            descendantWidth.Add((IPdfPrimitive) new PdfNumber(num1));
            if (index != 0)
              descendantWidth.Add((IPdfPrimitive) element);
            num1 = ttfGlyphInfo.Index;
            element = new PdfArray();
          }
          element.Add((IPdfPrimitive) new PdfNumber(ttfGlyphInfo.Width));
          if (index + 1 == count)
          {
            descendantWidth.Add((IPdfPrimitive) new PdfNumber(num1));
            descendantWidth.Add((IPdfPrimitive) element);
          }
          num2 = ttfGlyphInfo.Index;
        }
      }
      else
      {
        bool flag2 = false;
        if (PdfDocument.ConformanceLevel != PdfConformanceLevel.None && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_X1A2001)
          flag2 = true;
        List<int> intList = new List<int>();
        int index1 = 0;
        for (int count = this.glyphInfo.Count; index1 < count; ++index1)
        {
          TtfGlyphInfo ttfGlyphInfo = this.glyphInfo[index1];
          if (!flag1)
          {
            flag1 = true;
            num2 = ttfGlyphInfo.Index - 1;
          }
          int index2 = ttfGlyphInfo.Index;
          if ((num2 + 1 == ttfGlyphInfo.Index || index1 + 1 == count || !intList.Contains(ttfGlyphInfo.Index)) && count > 1)
          {
            element.Add((IPdfPrimitive) new PdfNumber(ttfGlyphInfo.Width));
            descendantWidth.Add((IPdfPrimitive) new PdfNumber(index2));
            descendantWidth.Add((IPdfPrimitive) element);
            intList.Add(index2);
            element = new PdfArray();
            if (flag2 && descendantWidth.Count >= 8190)
              break;
          }
          num2 = ttfGlyphInfo.Index;
        }
        intList.Clear();
      }
    }
    else
      descendantWidth = this.GetOpenTypeDecendantWidth();
    return descendantWidth;
  }

  internal List<TtfGlyphInfo> GetGlyphInfo()
  {
    List<TtfGlyphInfo> glyphInfo = new List<TtfGlyphInfo>();
    foreach (KeyValuePair<char, char> usedChar in this.m_usedChars)
    {
      TtfGlyphInfo glyph = this.m_ttfReader.GetGlyph(usedChar.Key);
      if (!glyph.Empty)
        glyphInfo.Add(glyph);
    }
    return glyphInfo;
  }

  private PdfArray GetOpenTypeDecendantWidth()
  {
    this.UpdateOpenTypeGlyphs();
    PdfArray typeDecendantWidth = new PdfArray();
    if (this.m_openTypeGlyphs != null && this.m_openTypeGlyphs.Count > 0)
    {
      int[] array = new int[this.m_openTypeGlyphs.Count];
      this.m_openTypeGlyphs.Keys.CopyTo(array, 0);
      Array.Sort<int>(array);
      int index = 0;
      for (int length = array.Length; index < length; ++index)
      {
        PdfArray element = new PdfArray();
        element.Add((IPdfPrimitive) new PdfNumber(this.m_openTypeGlyphs[array[index]].Width));
        typeDecendantWidth.Add((IPdfPrimitive) new PdfNumber(array[index]));
        typeDecendantWidth.Add((IPdfPrimitive) element);
        if ((PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A1B || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A1A) && typeDecendantWidth.Count >= 8190)
          break;
      }
    }
    return typeDecendantWidth;
  }

  private void UpdateOpenTypeGlyphs()
  {
    if (this.m_openTypeGlyphs == null || this.m_usedChars == null || this.m_usedChars.Count <= 0)
      return;
    foreach (KeyValuePair<char, char> usedChar in this.m_usedChars)
    {
      TtfGlyphInfo glyph = this.m_ttfReader.GetGlyph(usedChar.Key);
      if (!glyph.Empty && !this.m_openTypeGlyphs.ContainsKey(glyph.Index))
      {
        OtfGlyphInfo otfGlyphInfo = new OtfGlyphInfo(glyph.CharCode, glyph.Index, (float) glyph.Width);
        this.m_openTypeGlyphs[glyph.Index] = otfGlyphInfo;
      }
    }
  }

  private string ToHexString(int n)
  {
    string str = Convert.ToString(n, 16 /*0x10*/);
    return $"{"<0000".Substring(0, 5 - str.Length)}{str}>";
  }

  private int GetDescriptorFlags()
  {
    int num = 0;
    TtfMetrics metrics = this.m_ttfReader.Metrics;
    if (metrics.IsFixedPitch)
      num |= 1;
    int descriptorFlags = !metrics.IsSymbol ? num | 32 /*0x20*/ : num | 4;
    if (metrics.IsItalic)
      descriptorFlags |= 64 /*0x40*/;
    if (metrics.IsBold)
      descriptorFlags |= 262144 /*0x040000*/;
    return descriptorFlags;
  }

  private RectangleF GetBoundBox()
  {
    RECT fontBox = this.m_ttfReader.Metrics.FontBox;
    int width = Math.Abs(fontBox.right - fontBox.left);
    int height = Math.Abs(fontBox.top - fontBox.bottom);
    return new RectangleF((float) fontBox.left, (float) fontBox.bottom, (float) width, (float) height);
  }

  private void FontDictionaryBeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    if ((this.m_usedChars == null || this.m_usedChars.Count <= 0) && (this.m_openTypeGlyphs == null || this.m_openTypeGlyphs.Count <= 0) || this.m_fontDescriptor.ContainsKey("ToUnicode"))
      return;
    this.m_fontDictionary["ToUnicode"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_cmap);
  }

  private void FontDescriptorBeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    if ((this.m_usedChars == null || this.m_usedChars.Count <= 0) && (this.m_openTypeGlyphs == null || this.m_openTypeGlyphs.Count <= 0) || this.m_fontDescriptor.ContainsKey("CIDSet") || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A3B || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A3A || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A3U || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A2B || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A2A || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A2U)
      return;
    this.m_fontDescriptor["CIDSet"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_CidStream);
  }

  private void FontProgramBeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    if (ars.Writer != null && ars.Writer is PdfWriter)
      this.m_isCompress = (ars.Writer as PdfWriter).isCompress;
    this.GenerateFontProgram();
  }

  private void CmapBeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.GenerateCmap();

  private void CidBeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.GenerateCidSet();

  private void DescendantFontBeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    if ((this.m_usedChars == null || this.m_usedChars.Count <= 0) && (this.m_openTypeGlyphs == null || this.m_openTypeGlyphs.Count <= 0))
      return;
    PdfArray descendantWidth = this.GetDescendantWidth();
    if (descendantWidth == null)
      return;
    this.m_descendantFont["W"] = (IPdfPrimitive) descendantWidth;
  }

  private void GenerateCidSet()
  {
    byte[] dummyBits = new byte[8]
    {
      (byte) 128 /*0x80*/,
      (byte) 64 /*0x40*/,
      (byte) 32 /*0x20*/,
      (byte) 16 /*0x10*/,
      (byte) 8,
      (byte) 4,
      (byte) 2,
      (byte) 1
    };
    if (this.m_usedChars != null && this.m_usedChars.Count > 0 && this.m_openTypeGlyphs == null)
    {
      Dictionary<int, int> glyphChars = this.m_ttfReader.GetGlyphChars(this.m_usedChars);
      byte[] data = (byte[]) null;
      if (glyphChars.Count > 0)
      {
        int[] array = new int[glyphChars.Count];
        glyphChars.Keys.CopyTo(array, 0);
        Array.Sort<int>(array);
        data = new byte[array[array.Length - 1] / 8 + 1];
        for (int index = 0; index < array.Length; ++index)
        {
          int num = array[index];
          data[num / 8] |= dummyBits[num % 8];
        }
      }
      this.m_CidStream.Write(data);
    }
    else
      this.GenerateOpenTypeCidSet(dummyBits);
  }

  private void GenerateOpenTypeCidSet(byte[] dummyBits)
  {
    this.UpdateOpenTypeGlyphs();
    if (this.m_openTypeGlyphs == null || this.m_openTypeGlyphs.Count <= 0)
      return;
    int[] array = new int[this.m_openTypeGlyphs.Count];
    this.m_openTypeGlyphs.Keys.CopyTo(array, 0);
    Array.Sort<int>(array);
    byte[] data = new byte[array[array.Length - 1] / 8 + 1];
    for (int index = 0; index < array.Length; ++index)
    {
      int num = array[index];
      data[num / 8] |= dummyBits[num % 8];
    }
    this.m_CidStream.Write(data);
  }
}
