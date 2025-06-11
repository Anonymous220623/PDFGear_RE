// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.TtfReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Fonts;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Native;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class TtfReader
{
  internal const int WidthMultiplier = 1000;
  private const int c_ttfVersion1 = 65536 /*0x010000*/;
  private const int c_ttfVersion2 = 1330926671;
  private const int c_macTtfVersion = 1953658213;
  private const int c_fp = 16 /*0x10*/;
  internal static readonly Encoding Encoding = Encoding.GetEncoding("windows-1252");
  private static readonly string[] s_tableNames;
  private static readonly string[] m_tableNames;
  private static readonly short[] s_entrySelectors;
  private BigEndianReader m_reader;
  private Dictionary<string, TtfTableInfo> m_tableDirectory;
  private TtfMetrics m_metrics;
  private int[] m_width;
  private Dictionary<int, TtfGlyphInfo> m_macintosh;
  private Dictionary<int, TtfGlyphInfo> m_microsoft;
  private Dictionary<int, TtfGlyphInfo> m_macintoshGlyphs;
  private Dictionary<int, TtfGlyphInfo> m_microsoftGlyphs;
  internal bool m_bIsLocaShort;
  private bool m_subset;
  private char m_surrogateHigh;
  private long m_lowestPosition;
  private int m_maxMacIndex;
  private bool m_isFontPresent;
  internal int m_missedGlyphs;
  private string metricsName = string.Empty;
  private bool m_isTtcFont;
  private bool m_isMacTTF;
  private string m_fontName;
  private bool m_isAzureCompatible;
  private Dictionary<int, TtfGlyphInfo> m_completeGlyph;
  internal int unitsPerEM;
  internal List<ScriptTags> supportedScriptTags = new List<ScriptTags>();
  private Dictionary<int, TtfGlyphInfo> m_unicodeUCS4Glyph;
  private GSUBTable m_gsub;
  private GDEFTable m_gdef;
  private GPOSTable m_gpos;
  internal bool isOpenTypeFont;
  internal bool m_AnsiEncode;
  internal bool conformanceEnabled;
  private Font m_font;
  internal int m_missedGlyphCount;
  internal int m_EmHeight;
  internal int m_CellAscent;
  private static HashSet<string> glyphFonts = new HashSet<string>((IEnumerable<string>) new string[6]
  {
    "gautami",
    "latha",
    "shruti",
    "mangal",
    "tunga",
    "vrinda"
  }, (IEqualityComparer<string>) System.StringComparer.OrdinalIgnoreCase);

  public BinaryReader Reader
  {
    get => this.m_reader.Reader;
    set => this.m_reader.Reader = value;
  }

  internal bool IsFontPresent => this.m_isFontPresent;

  public BigEndianReader InternalReader => this.m_reader;

  public TtfMetrics Metrics => this.m_metrics;

  internal Dictionary<string, TtfTableInfo> TableDirectory
  {
    get
    {
      if (this.m_tableDirectory == null)
        this.m_tableDirectory = new Dictionary<string, TtfTableInfo>();
      return this.m_tableDirectory;
    }
  }

  private Dictionary<int, TtfGlyphInfo> Macintosh
  {
    get
    {
      if (this.m_macintosh == null)
        this.m_macintosh = new Dictionary<int, TtfGlyphInfo>();
      return this.m_macintosh;
    }
  }

  private Dictionary<int, TtfGlyphInfo> Microsoft
  {
    get
    {
      if (this.m_microsoft == null)
        this.m_microsoft = new Dictionary<int, TtfGlyphInfo>();
      return this.m_microsoft;
    }
  }

  private Dictionary<int, TtfGlyphInfo> MacintoshGlyphs
  {
    get
    {
      if (this.m_macintoshGlyphs == null)
        this.m_macintoshGlyphs = new Dictionary<int, TtfGlyphInfo>();
      return this.m_macintoshGlyphs;
    }
  }

  private Dictionary<int, TtfGlyphInfo> MicrosoftGlyphs
  {
    get
    {
      if (this.m_microsoftGlyphs == null)
        this.m_microsoftGlyphs = new Dictionary<int, TtfGlyphInfo>();
      return this.m_microsoftGlyphs;
    }
  }

  internal GSUBTable GSUB
  {
    get
    {
      if (this.m_gsub == null)
        this.m_gsub = new GSUBTable(this.m_reader, this.GetTable(nameof (GSUB)).Offset, this.GDEF, this);
      return this.m_gsub;
    }
  }

  internal GDEFTable GDEF
  {
    get
    {
      if (this.m_gdef == null)
        this.m_gdef = new GDEFTable(this.m_reader, this.GetTable(nameof (GDEF)));
      return this.m_gdef;
    }
  }

  internal GPOSTable GPOS
  {
    get
    {
      if (this.m_gpos == null)
        this.m_gpos = new GPOSTable(this.m_reader, this.GetTable(nameof (GPOS)).Offset, this.GDEF, this);
      return this.m_gpos;
    }
  }

  internal Dictionary<int, TtfGlyphInfo> UnicodeUCS4Glyph
  {
    get
    {
      if (this.m_unicodeUCS4Glyph == null)
        this.m_unicodeUCS4Glyph = new Dictionary<int, TtfGlyphInfo>();
      return this.m_unicodeUCS4Glyph;
    }
    set => this.m_unicodeUCS4Glyph = value;
  }

  internal Dictionary<int, TtfGlyphInfo> CompleteGlyph
  {
    get
    {
      if (this.m_completeGlyph == null)
      {
        this.m_completeGlyph = new Dictionary<int, TtfGlyphInfo>();
        if (this.UnicodeUCS4Glyph.Count == 0)
        {
          if (this.Microsoft != null && this.Microsoft.Count > 0)
            this.UnicodeUCS4Glyph = this.Microsoft;
          else if (this.Macintosh != null && this.Macintosh.Count > 0)
            this.UnicodeUCS4Glyph = this.Macintosh;
        }
        foreach (KeyValuePair<int, TtfGlyphInfo> keyValuePair in this.UnicodeUCS4Glyph)
        {
          if (!this.m_completeGlyph.ContainsKey(keyValuePair.Value.Index))
            this.m_completeGlyph.Add(keyValuePair.Value.Index, keyValuePair.Value);
        }
        for (int key = 0; key < this.m_width.Length; ++key)
        {
          if (!this.m_completeGlyph.ContainsKey(key))
            this.m_completeGlyph.Add(key, new TtfGlyphInfo()
            {
              Width = this.m_width[key],
              CharCode = -1,
              Index = key
            });
        }
      }
      return this.m_completeGlyph;
    }
  }

  private string[] TableNames
  {
    get => this.TrueTypeSubset ? TtfReader.m_tableNames : TtfReader.s_tableNames;
  }

  internal Font Font => this.m_font;

  internal bool TrueTypeSubset
  {
    get => this.m_subset;
    set => this.m_subset = value;
  }

  static TtfReader()
  {
    TtfReader.s_tableNames = new string[9];
    TtfReader.s_tableNames[0] = "cvt ";
    TtfReader.s_tableNames[1] = "fpgm";
    TtfReader.s_tableNames[2] = "glyf";
    TtfReader.s_tableNames[3] = "head";
    TtfReader.s_tableNames[4] = "hhea";
    TtfReader.s_tableNames[5] = "hmtx";
    TtfReader.s_tableNames[6] = "loca";
    TtfReader.s_tableNames[7] = "maxp";
    TtfReader.s_tableNames[8] = "prep";
    TtfReader.m_tableNames = new string[10];
    TtfReader.m_tableNames[0] = "cmap";
    TtfReader.m_tableNames[1] = "cvt ";
    TtfReader.m_tableNames[2] = "fpgm";
    TtfReader.m_tableNames[3] = "glyf";
    TtfReader.m_tableNames[4] = "head";
    TtfReader.m_tableNames[5] = "hhea";
    TtfReader.m_tableNames[6] = "hmtx";
    TtfReader.m_tableNames[7] = "loca";
    TtfReader.m_tableNames[8] = "maxp";
    TtfReader.m_tableNames[9] = "prep";
    TtfReader.s_entrySelectors = new short[21]
    {
      (short) 0,
      (short) 0,
      (short) 1,
      (short) 1,
      (short) 2,
      (short) 2,
      (short) 2,
      (short) 2,
      (short) 3,
      (short) 3,
      (short) 3,
      (short) 3,
      (short) 3,
      (short) 3,
      (short) 3,
      (short) 3,
      (short) 4,
      (short) 4,
      (short) 4,
      (short) 4,
      (short) 4
    };
  }

  internal TtfReader(BinaryReader reader, string name)
  {
    this.metricsName = name;
    this.m_reader = reader != null ? new BigEndianReader(reader) : throw new ArgumentNullException(nameof (reader));
    this.Initialize();
  }

  public TtfReader(BinaryReader reader)
  {
    this.m_reader = reader != null ? new BigEndianReader(reader) : throw new ArgumentNullException(nameof (reader));
    this.Initialize();
  }

  public TtfReader(BinaryReader reader, Font font)
    : this(reader, font, false)
  {
  }

  internal TtfReader(BinaryReader reader, Font font, bool isAzureCompatible)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    this.m_isAzureCompatible = isAzureCompatible;
    this.m_reader = new BigEndianReader(reader);
    if (font != null)
      this.m_fontName = font.Name;
    this.m_font = font;
    this.Initialize();
  }

  public void Close()
  {
    if (this.m_reader != null)
    {
      this.m_reader.Close();
      this.m_reader = (BigEndianReader) null;
    }
    if (this.m_tableDirectory != null)
    {
      this.m_tableDirectory.Clear();
      this.m_tableDirectory = (Dictionary<string, TtfTableInfo>) null;
    }
    if (this.m_macintosh != null)
    {
      this.m_macintosh.Clear();
      this.m_macintosh = (Dictionary<int, TtfGlyphInfo>) null;
    }
    if (this.m_microsoft != null)
    {
      this.m_microsoft.Clear();
      this.m_microsoft = (Dictionary<int, TtfGlyphInfo>) null;
    }
    if (this.m_macintoshGlyphs != null)
    {
      this.m_macintoshGlyphs.Clear();
      this.m_macintoshGlyphs = (Dictionary<int, TtfGlyphInfo>) null;
    }
    if (this.m_microsoftGlyphs != null)
    {
      this.m_microsoftGlyphs.Clear();
      this.m_microsoftGlyphs = (Dictionary<int, TtfGlyphInfo>) null;
    }
    this.m_width = (int[]) null;
  }

  public TtfGlyphInfo GetGlyph(char charCode)
  {
    object obj = (object) null;
    int key1 = (int) charCode;
    if (!this.m_metrics.IsSymbol && this.m_microsoft != null)
    {
      string fontName = this.m_fontName;
      if (this.m_microsoft.ContainsKey(key1))
      {
        obj = (object) this.m_microsoft[key1];
        if (key1 != 32 /*0x20*/)
          this.m_isFontPresent = true;
      }
      else if (key1 != 32 /*0x20*/)
      {
        if (fontName != null && fontName.Equals("segoe ui symbol", StringComparison.OrdinalIgnoreCase) && this.m_width.Length > key1)
          return new TtfGlyphInfo()
          {
            CharCode = key1,
            Index = key1,
            Width = this.m_width[key1]
          };
        if (this.m_unicodeUCS4Glyph != null && char.IsSurrogate(charCode))
        {
          TtfGlyphInfo glyph = new TtfGlyphInfo();
          if (char.IsHighSurrogate(charCode))
          {
            this.m_surrogateHigh = charCode;
            this.m_isFontPresent = true;
            glyph.Width = 0;
          }
          if (char.IsLowSurrogate(charCode) && char.IsSurrogatePair(this.m_surrogateHigh, charCode))
          {
            int utf32 = char.ConvertToUtf32(this.m_surrogateHigh, charCode);
            if (this.m_unicodeUCS4Glyph.ContainsKey(utf32))
              glyph = this.m_unicodeUCS4Glyph[utf32];
          }
          return glyph;
        }
        this.m_isFontPresent = false;
        ++this.m_missedGlyphCount;
      }
    }
    else if (this.m_macintosh != null && (this.m_metrics.IsSymbol || this.m_isMacTTF))
    {
      int key2 = this.m_maxMacIndex == 0 ? ((key1 & 65280) == 61440 /*0xF000*/ ? key1 & (int) byte.MaxValue : key1) : key1 % (this.m_maxMacIndex + 1);
      if (this.m_macintosh.ContainsKey(key2))
      {
        obj = (object) this.m_macintosh[key2];
        this.m_isFontPresent = true;
      }
      if (obj == null && PdfDocument.ConformanceLevel != PdfConformanceLevel.None && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_X1A2001)
        obj = (object) new TtfGlyphInfo();
    }
    if (charCode == ' ' && obj == null)
      obj = (object) new TtfGlyphInfo();
    if (obj == null && this.isOTFFont() && this.UnicodeUCS4Glyph != null && this.UnicodeUCS4Glyph.ContainsKey((int) charCode))
      obj = (object) this.UnicodeUCS4Glyph[(int) charCode];
    return obj != null ? (TtfGlyphInfo) obj : this.GetDefaultGlyph();
  }

  public TtfGlyphInfo GetGlyph(int glyphIndex)
  {
    object obj = (object) null;
    if (!this.m_metrics.IsSymbol && this.m_microsoftGlyphs != null)
    {
      if (this.m_microsoftGlyphs.ContainsKey(glyphIndex))
        obj = (object) this.m_microsoftGlyphs[glyphIndex];
    }
    else if (this.m_metrics.IsSymbol && this.m_macintoshGlyphs != null && this.m_macintoshGlyphs.ContainsKey(glyphIndex))
      obj = (object) this.m_macintoshGlyphs[glyphIndex];
    if (obj == null && this.m_unicodeUCS4Glyph != null && this.m_unicodeUCS4Glyph.ContainsKey(glyphIndex))
      obj = (object) this.m_unicodeUCS4Glyph[glyphIndex];
    else if (obj == null && this.m_completeGlyph != null && this.m_completeGlyph.ContainsKey(glyphIndex))
      obj = (object) this.m_completeGlyph[glyphIndex];
    return obj != null ? (TtfGlyphInfo) obj : this.GetDefaultGlyph();
  }

  internal bool IsFontContainsChar(int code)
  {
    bool flag = false;
    if (!this.m_metrics.IsSymbol && this.m_microsoft != null)
    {
      if (this.m_microsoft.ContainsKey(code))
      {
        if (code != 32 /*0x20*/)
          flag = true;
      }
      else if (code != 32 /*0x20*/)
        flag = false;
    }
    else if (this.m_metrics.IsSymbol && this.m_macintosh != null)
    {
      if (this.m_maxMacIndex != 0)
        code %= this.m_maxMacIndex + 1;
      else
        code = (code & 65280) == 61440 /*0xF000*/ ? code & (int) byte.MaxValue : code;
      if (this.m_macintosh.ContainsKey(code))
        flag = true;
    }
    if (!this.m_metrics.IsSymbol && this.m_microsoftGlyphs != null)
    {
      if (this.m_microsoftGlyphs.ContainsKey(code))
        flag = true;
    }
    else if (this.m_metrics.IsSymbol && this.m_macintoshGlyphs != null && this.m_macintoshGlyphs.ContainsKey(code))
      flag = true;
    return flag;
  }

  internal Stream ReadCffTable()
  {
    TtfTableInfo ttfTableInfo = this.TableDirectory["CFF "];
    byte[] buffer = new byte[ttfTableInfo.Length];
    this.m_reader.Seek((long) ttfTableInfo.Offset);
    this.m_reader.Read(buffer, 0, ttfTableInfo.Length);
    return (Stream) new MemoryStream(buffer);
  }

  internal List<TtfGlyphInfo> GetAllGlyphs()
  {
    List<TtfGlyphInfo> allGlyphs = new List<TtfGlyphInfo>();
    foreach (TtfGlyphInfo ttfGlyphInfo in this.CompleteGlyph.Values)
      allGlyphs.Add(ttfGlyphInfo);
    return allGlyphs;
  }

  public void CreateInternals() => this.ReadMetrics();

  public byte[] ReadFontProgram(ConcurrentDictionary<char, char> chars)
  {
    Dictionary<int, int> glyphChars = this.GetGlyphChars(chars);
    TtfLocaTable locaTable = this.ReadLocaTable(this.m_bIsLocaShort);
    if (glyphChars.Count < chars.Count)
      this.m_missedGlyphs = chars.Count - glyphChars.Count;
    this.UpdateGlyphChars(glyphChars, locaTable);
    int[] newLocaTable = (int[]) null;
    byte[] newGlyphTable = (byte[]) null;
    byte[] newLocaTableOut = (byte[]) null;
    uint glyphTable = this.GenerateGlyphTable(glyphChars, locaTable, out newLocaTable, out newGlyphTable);
    int locaTableSize = this.UpdateLocaTable(newLocaTable, this.m_bIsLocaShort, out newLocaTableOut);
    return this.GetFontProgram(newLocaTableOut, newGlyphTable, glyphTable, (uint) locaTableSize);
  }

  internal byte[] ReadOpenTypeFontProgram(Dictionary<int, OtfGlyphInfo> otGlyphs)
  {
    Dictionary<int, int> glyphChars = new Dictionary<int, int>();
    foreach (KeyValuePair<int, OtfGlyphInfo> otGlyph in otGlyphs)
    {
      if (otGlyph.Value.CharCode != -1)
        glyphChars[otGlyph.Key] = otGlyph.Value.CharCode;
      else if (otGlyph.Value != null && otGlyph.Value.Characters != null)
        glyphChars[otGlyph.Key] = (int) otGlyph.Value.Characters[0];
    }
    TtfLocaTable locaTable = this.ReadLocaTable(this.m_bIsLocaShort);
    if (glyphChars.Count < otGlyphs.Count)
      this.m_missedGlyphs = otGlyphs.Count - glyphChars.Count;
    this.UpdateGlyphChars(glyphChars, locaTable);
    int[] newLocaTable = (int[]) null;
    byte[] newGlyphTable = (byte[]) null;
    byte[] newLocaTableOut = (byte[]) null;
    uint glyphTable = this.GenerateGlyphTable(glyphChars, locaTable, out newLocaTable, out newGlyphTable);
    int locaTableSize = this.UpdateLocaTable(newLocaTable, this.m_bIsLocaShort, out newLocaTableOut);
    return this.GetFontProgram(newLocaTableOut, newGlyphTable, glyphTable, (uint) locaTableSize);
  }

  public string ConvertString(string text)
  {
    char[] chArray = text != null ? new char[text.Length] : throw new ArgumentNullException(nameof (text));
    int length1 = 0;
    int index = 0;
    for (int length2 = text.Length; index < length2; ++index)
    {
      TtfGlyphInfo glyph = this.GetGlyph(text[index]);
      if (!glyph.Empty)
        chArray[length1++] = (char) glyph.Index;
    }
    return new string(chArray, 0, length1);
  }

  internal bool IsFontContainsString(string text)
  {
    bool flag = false;
    int num = text != null ? text.Length : throw new ArgumentNullException(nameof (text));
    int index = 0;
    for (int length = text.Length; index < length; ++index)
    {
      if (this.IsFontContainsChar((int) text[index]))
      {
        flag = true;
      }
      else
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  public int GetCharWidth(char code)
  {
    TtfGlyphInfo glyph = this.GetGlyph(code);
    TtfGlyphInfo ttfGlyphInfo = !glyph.Empty ? glyph : this.GetDefaultGlyph();
    return !ttfGlyphInfo.Empty ? ttfGlyphInfo.Width : 0;
  }

  internal Dictionary<int, int> GetGlyphChars(ConcurrentDictionary<char, char> chars)
  {
    if (chars == null)
      throw new ArgumentNullException(nameof (chars));
    Dictionary<int, int> glyphChars = new Dictionary<int, int>();
    foreach (KeyValuePair<char, char> keyValuePair in chars)
    {
      char key = keyValuePair.Key;
      TtfGlyphInfo glyph = this.GetGlyph(key);
      if (!glyph.Empty)
        glyphChars[glyph.Index] = (int) key;
    }
    return glyphChars;
  }

  private void Initialize()
  {
    this.ReadFontDirectory();
    TtfNameTable nameTable = this.ReadNameTable();
    TtfHeadTable ttfHeadTable = this.ReadHeadTable();
    this.InitializeFontName(nameTable);
    this.m_metrics.MacStyle = (int) ttfHeadTable.MacStyle;
    this.AddSupportedTags();
  }

  private void AddSupportedTags()
  {
    this.supportedScriptTags.Add(ScriptTags.Arabic);
    this.supportedScriptTags.Add(ScriptTags.Bengali);
    this.supportedScriptTags.Add(ScriptTags.Devanagari);
    this.supportedScriptTags.Add(ScriptTags.Gujarati);
    this.supportedScriptTags.Add(ScriptTags.Gurmukhi);
    this.supportedScriptTags.Add(ScriptTags.Kannada);
    this.supportedScriptTags.Add(ScriptTags.Khmer);
    this.supportedScriptTags.Add(ScriptTags.Malayalam);
    this.supportedScriptTags.Add(ScriptTags.Oriya);
    this.supportedScriptTags.Add(ScriptTags.Tamil);
    this.supportedScriptTags.Add(ScriptTags.Telugu);
    this.supportedScriptTags.Add(ScriptTags.Thai);
  }

  private void ReadFontDirectory()
  {
    this.m_reader.Seek(0L);
    this.CheckPreambula();
    int num1 = (int) this.m_reader.ReadInt16();
    int num2 = 0;
    bool flag = false;
    this.m_reader.Skip(6L);
    for (int index = 0; index < num1; ++index)
    {
      TtfTableInfo table = new TtfTableInfo();
      string str = this.m_reader.ReadString(4);
      table.Checksum = this.m_reader.ReadInt32();
      table.Offset = this.m_reader.ReadInt32();
      table.Length = this.m_reader.ReadInt32();
      if (this.Font == null)
      {
        string empty = string.Empty;
      }
      else
        this.Font.Name.ToLower();
      if (PdfDocument.EnableCache)
      {
        lock (PdfDocument.Cache)
        {
          if (!flag && this.Font != null && PdfDocument.Cache.FontOffsetTable.ContainsKey(this.Font) && !this.m_isAzureCompatible)
          {
            num2 = PdfDocument.Cache.FontOffsetTable[this.Font];
            flag = true;
          }
          else if (!flag)
          {
            if (this.Font != null)
            {
              if (!PdfDocument.Cache.FontOffsetTable.ContainsKey(this.Font))
              {
                if (!this.m_isAzureCompatible)
                {
                  num2 = this.NormalizeOffset(table, str, this.m_reader);
                  PdfDocument.Cache.FontOffsetTable.Add(this.Font, num2);
                  flag = true;
                }
              }
            }
          }
        }
      }
      else if (!flag)
      {
        num2 = this.NormalizeOffset(table, str, this.m_reader);
        if (!this.m_isTtcFont)
          flag = true;
      }
      if (num2 != 0)
        table.Offset += num2;
      this.TableDirectory[str] = table;
    }
    if (flag)
      return;
    this.m_lowestPosition = this.m_reader.BaseStream.Position;
    if (this.m_isTtcFont)
      return;
    this.FixOffsets();
  }

  private void FixOffsets()
  {
    int num1 = int.MaxValue;
    foreach (KeyValuePair<string, TtfTableInfo> keyValuePair in this.TableDirectory)
    {
      int offset = keyValuePair.Value.Offset;
      if (num1 > offset)
      {
        num1 = offset;
        if ((long) num1 <= this.m_lowestPosition)
          break;
      }
    }
    int num2 = num1 - (int) this.m_lowestPosition;
    if (num2 == 0)
      return;
    Dictionary<string, TtfTableInfo> dictionary = new Dictionary<string, TtfTableInfo>();
    foreach (KeyValuePair<string, TtfTableInfo> keyValuePair in this.TableDirectory)
    {
      TtfTableInfo ttfTableInfo = this.TableDirectory[keyValuePair.Key];
      ttfTableInfo.Offset -= num2;
      dictionary[keyValuePair.Key] = ttfTableInfo;
    }
    this.m_tableDirectory = dictionary;
  }

  private void ReadMetrics()
  {
    this.m_metrics = new TtfMetrics();
    TtfNameTable nameTable = this.ReadNameTable();
    TtfHeadTable headTable = this.ReadHeadTable();
    this.m_EmHeight = (int) headTable.UnitsPerEm;
    this.m_bIsLocaShort = headTable.IndexToLocFormat == (short) 0;
    TtfHorizontalHeaderTable horizontalHeadTable = this.ReadHorizontalHeaderTable();
    TtfOS2Table os2Table = this.ReadOS2Table();
    this.m_CellAscent = (int) os2Table.UsWinAscent;
    TtfPostTable postTable = this.ReadPostTable();
    this.m_width = this.ReadWidthTable((int) horizontalHeadTable.NumberOfHMetrics, (int) headTable.UnitsPerEm);
    TtfCmapSubTable[] cmapTables = this.ReadCmapTable();
    this.InitializeMetrics(nameTable, headTable, horizontalHeadTable, os2Table, postTable, cmapTables);
  }

  internal bool isOTFFont()
  {
    return !this.GetTable("GDEF").Empty && !this.GetTable("GSUB").Empty && !this.GetTable("GPOS").Empty;
  }

  private void InitializeMetrics(
    TtfNameTable nameTable,
    TtfHeadTable headTable,
    TtfHorizontalHeaderTable horizontalHeadTable,
    TtfOS2Table os2Table,
    TtfPostTable postTable,
    TtfCmapSubTable[] cmapTables)
  {
    if (cmapTables == null)
      throw new ArgumentNullException(nameof (cmapTables));
    this.InitializeFontName(nameTable);
    bool flag = false;
    for (int index = 0; index < cmapTables.Length; ++index)
    {
      TtfCmapSubTable cmapTable = cmapTables[index];
      if (this.GetCmapEncoding((int) cmapTable.PlatformID, (int) cmapTable.EncodingID) == TtfCmapEncoding.Symbol)
      {
        flag = true;
        break;
      }
    }
    this.m_metrics.IsSymbol = flag;
    this.m_metrics.MacStyle = (int) headTable.MacStyle;
    this.m_metrics.IsFixedPitch = postTable.IsFixedPitch != 0U;
    this.m_metrics.ItalicAngle = postTable.ItalicAngle;
    float num = 1000f / (float) headTable.UnitsPerEm;
    this.m_metrics.WinAscent = (float) os2Table.STypoAscender * num;
    this.m_metrics.MacAscent = (float) horizontalHeadTable.Ascender * num;
    this.m_metrics.CapHeight = os2Table.SCapHeight != (short) 0 ? (float) os2Table.SCapHeight : 0.7f * (float) headTable.UnitsPerEm * num;
    this.m_metrics.WinDescent = (float) os2Table.STypoDescender * num;
    this.m_metrics.MacDescent = (float) horizontalHeadTable.Descender * num;
    this.m_metrics.Leading = (float) ((int) os2Table.STypoAscender - (int) os2Table.STypoDescender + (int) os2Table.STypoLineGap) * num;
    this.m_metrics.LineGap = (int) Math.Ceiling((double) horizontalHeadTable.LineGap * (double) num);
    this.m_metrics.FontBox = new RECT((int) ((double) headTable.XMin * (double) num), (double) this.m_metrics.MacAscent == 0.0 ? (int) headTable.YMax : (int) Math.Ceiling((double) this.m_metrics.MacAscent + (double) this.m_metrics.LineGap), (int) ((double) headTable.XMax * (double) num), (double) this.m_metrics.MacAscent == 0.0 ? (int) headTable.YMin : (int) this.m_metrics.MacDescent);
    this.m_metrics.StemV = 80f;
    this.m_metrics.WidthTable = this.UpdateWidth();
    this.m_metrics.ContainsCFF = this.TableDirectory.ContainsKey("CFF ");
    this.m_metrics.SubScriptSizeFactor = (float) headTable.UnitsPerEm / (float) os2Table.YSubscriptYSize;
    this.m_metrics.SuperscriptSizeFactor = (float) headTable.UnitsPerEm / (float) os2Table.YSuperscriptYSize;
  }

  private TtfNameTable ReadNameTable()
  {
    TtfTableInfo table = this.GetTable("name");
    this.m_reader.Seek((long) table.Offset);
    TtfNameTable ttfNameTable = new TtfNameTable()
    {
      FormatSelector = this.m_reader.ReadUInt16(),
      RecordsCount = this.m_reader.ReadUInt16(),
      Offset = this.m_reader.ReadUInt16()
    };
    ttfNameTable.NameRecords = new TtfNameRecord[(int) ttfNameTable.RecordsCount];
    long position = this.m_reader.BaseStream.Position;
    int num = 12;
    int index = 0;
    for (int recordsCount = (int) ttfNameTable.RecordsCount; index < recordsCount; ++index)
    {
      this.m_reader.Seek(position);
      TtfNameRecord ttfNameRecord = new TtfNameRecord();
      ttfNameRecord.PlatformID = this.m_reader.ReadUInt16();
      ttfNameRecord.EncodingID = this.m_reader.ReadUInt16();
      ttfNameRecord.LanguageID = this.m_reader.ReadUInt16();
      ttfNameRecord.NameID = this.m_reader.ReadUInt16();
      ttfNameRecord.Length = this.m_reader.ReadUInt16();
      ttfNameRecord.Offset = this.m_reader.ReadUInt16();
      this.m_reader.Seek((long) (table.Offset + (int) ttfNameTable.Offset + (int) ttfNameRecord.Offset));
      bool unicode = ttfNameRecord.PlatformID == (ushort) 0 || ttfNameRecord.PlatformID == (ushort) 3;
      ttfNameRecord.Name = this.m_reader.ReadString((int) ttfNameRecord.Length, unicode);
      TtfPlatformID ttfPlatformId = this.GetCmapEncoding((int) ttfNameRecord.PlatformID, (int) ttfNameRecord.EncodingID) == TtfCmapEncoding.Macintosh ? TtfPlatformID.Macintosh : TtfPlatformID.Microsoft;
      string currentOs = this.GetCurrentOS();
      if (ttfPlatformId.ToString() == currentOs)
        ttfNameTable.NameRecords[index] = ttfNameRecord;
      position += (long) num;
    }
    return ttfNameTable;
  }

  private TtfHeadTable ReadHeadTable()
  {
    this.m_reader.Seek((long) this.GetTable("head").Offset);
    TtfHeadTable ttfHeadTable = new TtfHeadTable();
    ttfHeadTable.Version = this.m_reader.ReadFixed();
    ttfHeadTable.FontRevision = this.m_reader.ReadFixed();
    ttfHeadTable.CheckSumAdjustment = this.m_reader.ReadUInt32();
    ttfHeadTable.MagicNumber = this.m_reader.ReadUInt32();
    ttfHeadTable.Flags = this.m_reader.ReadUInt16();
    this.unitsPerEM = (int) (ttfHeadTable.UnitsPerEm = this.m_reader.ReadUInt16());
    ttfHeadTable.Created = this.m_reader.ReadInt64();
    ttfHeadTable.Modified = this.m_reader.ReadInt64();
    ttfHeadTable.XMin = this.m_reader.ReadInt16();
    ttfHeadTable.YMin = this.m_reader.ReadInt16();
    ttfHeadTable.XMax = this.m_reader.ReadInt16();
    ttfHeadTable.YMax = this.m_reader.ReadInt16();
    ttfHeadTable.MacStyle = this.m_reader.ReadUInt16();
    ttfHeadTable.LowestRecPPEM = this.m_reader.ReadUInt16();
    ttfHeadTable.FontDirectionHint = this.m_reader.ReadInt16();
    ttfHeadTable.IndexToLocFormat = this.m_reader.ReadInt16();
    ttfHeadTable.GlyphDataFormat = this.m_reader.ReadInt16();
    return ttfHeadTable;
  }

  private TtfHorizontalHeaderTable ReadHorizontalHeaderTable()
  {
    this.m_reader.Seek((long) this.GetTable("hhea").Offset);
    TtfHorizontalHeaderTable horizontalHeaderTable = new TtfHorizontalHeaderTable();
    horizontalHeaderTable.Version = this.m_reader.ReadFixed();
    horizontalHeaderTable.Ascender = this.m_reader.ReadInt16();
    horizontalHeaderTable.Descender = this.m_reader.ReadInt16();
    horizontalHeaderTable.LineGap = this.m_reader.ReadInt16();
    horizontalHeaderTable.AdvanceWidthMax = this.m_reader.ReadUInt16();
    horizontalHeaderTable.MinLeftSideBearing = this.m_reader.ReadInt16();
    horizontalHeaderTable.MinRightSideBearing = this.m_reader.ReadInt16();
    horizontalHeaderTable.XMaxExtent = this.m_reader.ReadInt16();
    horizontalHeaderTable.CaretSlopeRise = this.m_reader.ReadInt16();
    horizontalHeaderTable.CaretSlopeRun = this.m_reader.ReadInt16();
    this.m_reader.Skip(10L);
    horizontalHeaderTable.MetricDataFormat = this.m_reader.ReadInt16();
    horizontalHeaderTable.NumberOfHMetrics = this.m_reader.ReadUInt16();
    return horizontalHeaderTable;
  }

  internal TtfGlyphInfo ReadGlyph(int index, bool isOpenType)
  {
    TtfGlyphInfo ttfGlyphInfo;
    if (isOpenType)
    {
      this.CompleteGlyph.TryGetValue(index, out ttfGlyphInfo);
    }
    else
    {
      int count = this.CompleteGlyph.Count;
      this.UnicodeUCS4Glyph.TryGetValue(index, out ttfGlyphInfo);
    }
    return ttfGlyphInfo;
  }

  private TtfOS2Table ReadOS2Table()
  {
    this.m_reader.Seek((long) this.GetTable("OS/2").Offset);
    TtfOS2Table ttfOs2Table = new TtfOS2Table();
    ttfOs2Table.Version = this.m_reader.ReadUInt16();
    ttfOs2Table.XAvgCharWidth = this.m_reader.ReadInt16();
    ttfOs2Table.UsWeightClass = this.m_reader.ReadUInt16();
    ttfOs2Table.UsWidthClass = this.m_reader.ReadUInt16();
    ttfOs2Table.FsType = this.m_reader.ReadInt16();
    ttfOs2Table.YSubscriptXSize = this.m_reader.ReadInt16();
    ttfOs2Table.YSubscriptYSize = this.m_reader.ReadInt16();
    ttfOs2Table.YSubscriptXOffset = this.m_reader.ReadInt16();
    ttfOs2Table.YSubscriptYOffset = this.m_reader.ReadInt16();
    ttfOs2Table.ySuperscriptXSize = this.m_reader.ReadInt16();
    ttfOs2Table.YSuperscriptYSize = this.m_reader.ReadInt16();
    ttfOs2Table.YSuperscriptXOffset = this.m_reader.ReadInt16();
    ttfOs2Table.YSuperscriptYOffset = this.m_reader.ReadInt16();
    ttfOs2Table.YStrikeoutSize = this.m_reader.ReadInt16();
    ttfOs2Table.YStrikeoutPosition = this.m_reader.ReadInt16();
    ttfOs2Table.SFamilyClass = this.m_reader.ReadInt16();
    ttfOs2Table.Panose = this.m_reader.ReadBytes(10);
    ttfOs2Table.UlUnicodeRange1 = this.m_reader.ReadUInt32();
    ttfOs2Table.UlUnicodeRange2 = this.m_reader.ReadUInt32();
    ttfOs2Table.UlUnicodeRange3 = this.m_reader.ReadUInt32();
    ttfOs2Table.UlUnicodeRange4 = this.m_reader.ReadUInt32();
    ttfOs2Table.AchVendID = this.m_reader.ReadBytes(4);
    ttfOs2Table.FsSelection = this.m_reader.ReadUInt16();
    ttfOs2Table.UsFirstCharIndex = this.m_reader.ReadUInt16();
    ttfOs2Table.UsLastCharIndex = this.m_reader.ReadUInt16();
    ttfOs2Table.STypoAscender = this.m_reader.ReadInt16();
    ttfOs2Table.STypoDescender = this.m_reader.ReadInt16();
    ttfOs2Table.STypoLineGap = this.m_reader.ReadInt16();
    ttfOs2Table.UsWinAscent = this.m_reader.ReadUInt16();
    ttfOs2Table.UsWinDescent = this.m_reader.ReadUInt16();
    ttfOs2Table.UlCodePageRange1 = this.m_reader.ReadUInt32();
    ttfOs2Table.UlCodePageRange2 = this.m_reader.ReadUInt32();
    if (ttfOs2Table.Version > (ushort) 1)
    {
      ttfOs2Table.SxHeight = this.m_reader.ReadInt16();
      ttfOs2Table.SCapHeight = this.m_reader.ReadInt16();
      ttfOs2Table.UsDefaultChar = this.m_reader.ReadUInt16();
      ttfOs2Table.UsBreakChar = this.m_reader.ReadUInt16();
      ttfOs2Table.UsMaxContext = this.m_reader.ReadUInt16();
    }
    return ttfOs2Table;
  }

  private TtfPostTable ReadPostTable()
  {
    this.m_reader.Seek((long) this.GetTable("post").Offset);
    return new TtfPostTable()
    {
      FormatType = this.m_reader.ReadFixed(),
      ItalicAngle = this.m_reader.ReadFixed(),
      UnderlinePosition = this.m_reader.ReadInt16(),
      UnderlineThickness = this.m_reader.ReadInt16(),
      IsFixedPitch = this.m_reader.ReadUInt32(),
      MinMemType42 = this.m_reader.ReadUInt32(),
      MaxMemType42 = this.m_reader.ReadUInt32(),
      MinMemType1 = this.m_reader.ReadUInt32(),
      MaxMemType1 = this.m_reader.ReadUInt32()
    };
  }

  private int[] ReadWidthTable(int glyphCount, int unitsPerEm)
  {
    this.m_reader.Seek((long) this.GetTable("hmtx").Offset);
    int[] numArray = new int[glyphCount];
    for (int index = 0; index < glyphCount; ++index)
    {
      int num = (int) new TtfLongHorMertric()
      {
        AdvanceWidth = this.m_reader.ReadUInt16(),
        Lsb = this.m_reader.ReadInt16()
      }.AdvanceWidth * 1000 / unitsPerEm;
      numArray[index] = num;
    }
    return numArray;
  }

  private TtfCmapSubTable[] ReadCmapTable()
  {
    this.m_reader.Seek((long) this.GetTable("cmap").Offset);
    TtfCmapTable ttfCmapTable = new TtfCmapTable();
    ttfCmapTable.Version = this.m_reader.ReadUInt16();
    ttfCmapTable.TablesCount = this.m_reader.ReadUInt16();
    long position = this.m_reader.BaseStream.Position;
    TtfCmapSubTable[] ttfCmapSubTableArray = new TtfCmapSubTable[(int) ttfCmapTable.TablesCount];
    for (int index = 0; index < (int) ttfCmapTable.TablesCount; ++index)
    {
      this.m_reader.Seek(position);
      TtfCmapSubTable subTable = new TtfCmapSubTable();
      subTable.PlatformID = this.m_reader.ReadUInt16();
      subTable.EncodingID = this.m_reader.ReadUInt16();
      subTable.Offset = this.m_reader.ReadUInt32();
      position = this.m_reader.BaseStream.Position;
      this.ReadCmapSubTable(subTable);
      ttfCmapSubTableArray[index] = subTable;
    }
    return ttfCmapSubTableArray;
  }

  private void ReadCmapSubTable(TtfCmapSubTable subTable)
  {
    this.m_reader.Seek((long) this.GetTable("cmap").Offset + (long) subTable.Offset);
    TtfCmapFormat ttfCmapFormat = (TtfCmapFormat) this.m_reader.ReadUInt16();
    TtfCmapEncoding cmapEncoding = this.GetCmapEncoding((int) subTable.PlatformID, (int) subTable.EncodingID);
    if (cmapEncoding == TtfCmapEncoding.Unknown)
      return;
    switch (ttfCmapFormat)
    {
      case TtfCmapFormat.Apple:
        this.ReadAppleCmapTable(subTable, cmapEncoding);
        break;
      case TtfCmapFormat.Microsoft:
        this.ReadMicrosoftCmapTable(subTable, cmapEncoding);
        break;
      case TtfCmapFormat.Trimmed:
        this.ReadTrimmedCmapTable(subTable, cmapEncoding);
        break;
      case TtfCmapFormat.MicrosoftExt:
        this.ReadUCS4CmapTable(subTable, cmapEncoding);
        break;
    }
  }

  private void ReadUCS4CmapTable(TtfCmapSubTable subTable, TtfCmapEncoding encoding)
  {
    this.m_reader.Seek((long) this.GetTable("cmap").Offset + (long) subTable.Offset);
    int num1 = (int) this.m_reader.ReadUInt16();
    this.m_reader.Skip(2L);
    this.m_reader.ReadInt32();
    this.m_reader.Skip(4L);
    int num2 = this.m_reader.ReadInt32();
    if (this.m_reader.BaseStream.Position >= this.m_reader.BaseStream.Length)
      return;
    int num3 = 0;
    for (int index1 = num2; num3 < index1; ++num3)
    {
      int num4 = this.m_reader.ReadInt32();
      int num5 = this.m_reader.ReadInt32();
      int glyphCode = this.m_reader.ReadInt32();
      for (int index2 = num4; index2 <= num5; ++index2)
      {
        this.AddGlyph(new TtfGlyphInfo()
        {
          CharCode = index2,
          Width = this.GetWidth(glyphCode),
          Index = glyphCode
        }, encoding, true);
        ++glyphCode;
      }
    }
  }

  private void ReadAppleCmapTable(TtfCmapSubTable subTable, TtfCmapEncoding encoding)
  {
    this.m_reader.Seek((long) this.GetTable("cmap").Offset + (long) subTable.Offset);
    TtfAppleCmapSubTable appleCmapSubTable = new TtfAppleCmapSubTable()
    {
      Format = this.m_reader.ReadUInt16(),
      Length = this.m_reader.ReadUInt16(),
      Version = this.m_reader.ReadUInt16()
    };
    if (this.m_reader.BaseStream.Position >= this.m_reader.BaseStream.Length)
      return;
    int num = 0;
    for (int index = 256 /*0x0100*/; num < index; ++num)
    {
      TtfGlyphInfo glyph = new TtfGlyphInfo()
      {
        Index = (int) this.m_reader.ReadByte()
      };
      glyph.Width = this.GetWidth(glyph.Index);
      glyph.CharCode = num;
      this.Macintosh[num] = glyph;
      this.AddGlyph(glyph, encoding);
      this.m_maxMacIndex = Math.Max(num, this.m_maxMacIndex);
    }
  }

  private void ReadMicrosoftCmapTable(TtfCmapSubTable subTable, TtfCmapEncoding encoding)
  {
    this.m_reader.Seek((long) this.GetTable("cmap").Offset + (long) subTable.Offset);
    Dictionary<int, TtfGlyphInfo> dictionary = encoding == TtfCmapEncoding.Unicode ? this.Microsoft : this.Macintosh;
    TtfMicrosoftCmapSubTable microsoftCmapSubTable = new TtfMicrosoftCmapSubTable();
    microsoftCmapSubTable.Format = this.m_reader.ReadUInt16();
    microsoftCmapSubTable.Length = this.m_reader.ReadUInt16();
    microsoftCmapSubTable.Version = this.m_reader.ReadUInt16();
    microsoftCmapSubTable.SegCountX2 = this.m_reader.ReadUInt16();
    microsoftCmapSubTable.SearchRange = this.m_reader.ReadUInt16();
    microsoftCmapSubTable.EntrySelector = this.m_reader.ReadUInt16();
    microsoftCmapSubTable.RangeShift = this.m_reader.ReadUInt16();
    int len1 = (int) microsoftCmapSubTable.SegCountX2 / 2;
    microsoftCmapSubTable.EndCount = this.ReadUshortArray(len1);
    microsoftCmapSubTable.ReservedPad = this.m_reader.ReadUInt16();
    microsoftCmapSubTable.StartCount = this.ReadUshortArray(len1);
    microsoftCmapSubTable.IdDelta = this.ReadUshortArray(len1);
    microsoftCmapSubTable.IdRangeOffset = this.ReadUshortArray(len1);
    int len2 = (int) microsoftCmapSubTable.Length / 2 - 8 - len1 * 4;
    microsoftCmapSubTable.GlyphID = this.ReadUshortArray(len2);
    for (int index1 = 0; index1 < len1; ++index1)
    {
      int num1 = (int) microsoftCmapSubTable.StartCount[index1];
      for (int index2 = (int) microsoftCmapSubTable.EndCount[index1]; num1 <= index2 && num1 != (int) ushort.MaxValue; ++num1)
      {
        int num2;
        if (microsoftCmapSubTable.IdRangeOffset[index1] == (ushort) 0)
        {
          num2 = num1 + (int) microsoftCmapSubTable.IdDelta[index1] & (int) ushort.MaxValue;
        }
        else
        {
          int index3 = index1 + (int) microsoftCmapSubTable.IdRangeOffset[index1] / 2 - len1 + num1 - (int) microsoftCmapSubTable.StartCount[index1];
          if (index3 < microsoftCmapSubTable.GlyphID.Length)
            num2 = (int) microsoftCmapSubTable.GlyphID[index3] + (int) microsoftCmapSubTable.IdDelta[index1] & (int) ushort.MaxValue;
          else
            continue;
        }
        TtfGlyphInfo glyph = new TtfGlyphInfo()
        {
          Index = num2
        };
        glyph.Width = this.GetWidth(glyph.Index);
        int key = encoding == TtfCmapEncoding.Symbol ? ((num1 & 65280) == 61440 /*0xF000*/ ? num1 & (int) byte.MaxValue : num1) : num1;
        glyph.CharCode = key;
        dictionary[key] = glyph;
        this.AddGlyph(glyph, encoding);
      }
    }
  }

  private void ReadTrimmedCmapTable(TtfCmapSubTable subTable, TtfCmapEncoding encoding)
  {
    this.m_reader.Seek((long) this.GetTable("cmap").Offset + (long) subTable.Offset);
    TtfTrimmedCmapSubTable trimmedCmapSubTable = new TtfTrimmedCmapSubTable();
    trimmedCmapSubTable.Format = this.m_reader.ReadUInt16();
    trimmedCmapSubTable.Length = this.m_reader.ReadUInt16();
    trimmedCmapSubTable.Version = this.m_reader.ReadUInt16();
    trimmedCmapSubTable.FirstCode = this.m_reader.ReadUInt16();
    trimmedCmapSubTable.EntryCount = this.m_reader.ReadUInt16();
    if (this.m_reader.BaseStream.Position >= this.m_reader.BaseStream.Length)
      return;
    int num = 0;
    for (int entryCount = (int) trimmedCmapSubTable.EntryCount; num < entryCount; ++num)
    {
      TtfGlyphInfo glyph = new TtfGlyphInfo()
      {
        Index = (int) this.m_reader.ReadUInt16()
      };
      glyph.Width = this.GetWidth(glyph.Index);
      glyph.CharCode = num + (int) trimmedCmapSubTable.FirstCode;
      this.Macintosh[num] = glyph;
      this.AddGlyph(glyph, encoding);
      this.m_maxMacIndex = Math.Max(num, this.m_maxMacIndex);
    }
  }

  private TtfLocaTable ReadLocaTable(bool bShort)
  {
    TtfTableInfo table = this.GetTable("loca");
    this.m_reader.Seek((long) table.Offset);
    TtfLocaTable ttfLocaTable = new TtfLocaTable();
    uint[] numArray;
    if (bShort)
    {
      int length = table.Length / 2;
      numArray = new uint[length];
      for (int index = 0; index < length; ++index)
        numArray[index] = (uint) this.m_reader.ReadUInt16() * 2U;
    }
    else
    {
      int length = table.Length / 4;
      numArray = new uint[length];
      for (int index = 0; index < length; ++index)
        numArray[index] = this.m_reader.ReadUInt32();
    }
    ttfLocaTable.Offsets = numArray;
    return ttfLocaTable;
  }

  private ushort[] ReadUshortArray(int len)
  {
    ushort[] numArray = new ushort[len];
    for (int index = 0; index < len; ++index)
      numArray[index] = this.m_reader.ReadUInt16();
    return numArray;
  }

  private uint[] ReadUintArray(int len)
  {
    uint[] numArray = new uint[len];
    for (int index = 0; index < len; ++index)
      numArray[index] = this.m_reader.ReadUInt32();
    return numArray;
  }

  private void AddGlyph(TtfGlyphInfo glyph, TtfCmapEncoding encoding)
  {
    Dictionary<int, TtfGlyphInfo> dictionary = (Dictionary<int, TtfGlyphInfo>) null;
    switch (encoding)
    {
      case TtfCmapEncoding.Symbol:
      case TtfCmapEncoding.Macintosh:
        dictionary = this.MacintoshGlyphs;
        break;
      case TtfCmapEncoding.Unicode:
        dictionary = this.MicrosoftGlyphs;
        break;
      case TtfCmapEncoding.UnicodeUCS4:
        dictionary = this.UnicodeUCS4Glyph;
        break;
    }
    dictionary[glyph.Index] = glyph;
  }

  private void AddGlyph(TtfGlyphInfo glyph, TtfCmapEncoding encoding, bool reverse)
  {
    Dictionary<int, TtfGlyphInfo> dictionary = (Dictionary<int, TtfGlyphInfo>) null;
    switch (encoding)
    {
      case TtfCmapEncoding.Symbol:
      case TtfCmapEncoding.Macintosh:
        dictionary = this.MacintoshGlyphs;
        break;
      case TtfCmapEncoding.Unicode:
        dictionary = this.MicrosoftGlyphs;
        break;
      case TtfCmapEncoding.UnicodeUCS4:
        dictionary = this.UnicodeUCS4Glyph;
        break;
    }
    dictionary[glyph.CharCode] = glyph;
  }

  private int GetWidth(int glyphCode)
  {
    glyphCode = glyphCode < this.m_width.Length ? glyphCode : this.m_width.Length - 1;
    return this.m_width[glyphCode];
  }

  private int[] UpdateWidth()
  {
    int length = 256 /*0x0100*/;
    int[] numArray = new int[length];
    if (this.m_metrics.IsSymbol)
    {
      for (int charCode = 0; charCode < length; ++charCode)
      {
        TtfGlyphInfo glyph = this.GetGlyph((char) charCode);
        numArray[charCode] = glyph.Empty ? 0 : glyph.Width;
      }
    }
    else
    {
      byte[] bytes = new byte[1];
      char ch = '?';
      char charCode = ' ';
      for (int index = 0; index < length; ++index)
      {
        bytes[0] = (byte) index;
        string empty = string.Empty;
        string str = !this.m_AnsiEncode ? TtfReader.Encoding.GetString(bytes, 0, bytes.Length) : new Windows1252Encoding().GetString(bytes, 0, bytes.Length);
        TtfGlyphInfo glyph = this.GetGlyph(str.Length > 0 ? str[0] : ch);
        if (!glyph.Empty)
        {
          numArray[index] = glyph.Width;
        }
        else
        {
          glyph = this.GetGlyph(charCode);
          numArray[index] = glyph.Empty ? 0 : glyph.Width;
        }
      }
    }
    return numArray;
  }

  private void CheckPreambula()
  {
    int num = this.m_reader.ReadInt32();
    if (num == 1953658213)
      this.m_isMacTTF = true;
    if (num != 65536 /*0x010000*/ && num != 1330926671 && num != 1953658213)
    {
      this.m_isTtcFont = true;
      this.m_reader.Seek(0L);
      if (this.m_reader.ReadString(4) != "ttcf")
        throw new PdfException("Can't read TTF font data");
      this.m_reader.Skip(4L);
      if (this.m_reader.ReadInt32() < 0)
        throw new PdfException("Can't read TTF font data");
      this.m_reader.Seek((long) this.m_reader.ReadInt32());
      num = this.m_reader.ReadInt32();
    }
    if (num != 1330926671)
      return;
    this.isOpenTypeFont = true;
  }

  private string GetCurrentOS()
  {
    PlatformID platform = Environment.OSVersion.Platform;
    string empty = string.Empty;
    switch (platform)
    {
      case PlatformID.Win32S:
      case PlatformID.Win32Windows:
      case PlatformID.Win32NT:
      case PlatformID.WinCE:
        empty = TtfPlatformID.Microsoft.ToString();
        break;
      case PlatformID.MacOSX:
        empty = TtfPlatformID.Macintosh.ToString();
        break;
    }
    return empty;
  }

  private TtfCmapEncoding GetCmapEncoding(int platformID, int encodingID)
  {
    TtfCmapEncoding cmapEncoding = TtfCmapEncoding.Unknown;
    if (platformID == 3 && encodingID == 0)
      cmapEncoding = TtfCmapEncoding.Symbol;
    else if (platformID == 3 && encodingID == 1)
      cmapEncoding = TtfCmapEncoding.Unicode;
    else if (platformID == 1 && encodingID == 0)
      cmapEncoding = TtfCmapEncoding.Macintosh;
    else if (platformID == 3 && encodingID == 10)
      cmapEncoding = TtfCmapEncoding.UnicodeUCS4;
    return cmapEncoding;
  }

  private TtfTableInfo GetTable(string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    object obj = (object) null;
    TtfTableInfo table = new TtfTableInfo();
    if (this.TableDirectory.ContainsKey(name))
      obj = (object) this.TableDirectory[name];
    if (obj != null)
      table = (TtfTableInfo) obj;
    return table;
  }

  private void UpdateGlyphChars(Dictionary<int, int> glyphChars, TtfLocaTable locaTable)
  {
    if (glyphChars == null)
      throw new ArgumentNullException(nameof (glyphChars));
    if (!glyphChars.ContainsKey(0))
      glyphChars.Add(0, 0);
    Dictionary<int, int> dictionary = new Dictionary<int, int>(glyphChars.Count);
    foreach (KeyValuePair<int, int> glyphChar in glyphChars)
      dictionary.Add(glyphChar.Key, glyphChar.Value);
    foreach (KeyValuePair<int, int> keyValuePair in dictionary)
    {
      int key = keyValuePair.Key;
      this.ProcessCompositeGlyph(glyphChars, key, locaTable);
    }
  }

  private void ProcessCompositeGlyph(
    Dictionary<int, int> glyphChars,
    int glyph,
    TtfLocaTable locaTable)
  {
    if (glyphChars == null)
      throw new ArgumentNullException(nameof (glyphChars));
    if (glyph >= locaTable.Offsets.Length - 1)
      return;
    uint offset = locaTable.Offsets[glyph];
    if ((int) offset == (int) locaTable.Offsets[glyph + 1])
      return;
    this.m_reader.Seek((long) this.GetTable("glyf").Offset + (long) offset);
    if (new TtfGlyphHeader()
    {
      numberOfContours = this.m_reader.ReadInt16(),
      XMin = this.m_reader.ReadInt16(),
      YMin = this.m_reader.ReadInt16(),
      XMax = this.m_reader.ReadInt16(),
      YMax = this.m_reader.ReadInt16()
    }.numberOfContours >= (short) 0)
      return;
    while (true)
    {
      ushort num = this.m_reader.ReadUInt16();
      int key = (int) this.m_reader.ReadUInt16();
      if (!glyphChars.ContainsKey(key))
        glyphChars.Add(key, 0);
      if (((int) num & 32 /*0x20*/) != 0)
      {
        int numBytes = ((int) num & 1) != 0 ? 4 : 2;
        if (((int) num & 8) != 0)
          numBytes += 2;
        else if (((int) num & 64 /*0x40*/) != 0)
          numBytes += 4;
        else if (((int) num & 128 /*0x80*/) != 0)
          numBytes += 8;
        this.m_reader.Skip((long) numBytes);
      }
      else
        break;
    }
  }

  private uint GenerateGlyphTable(
    Dictionary<int, int> glyphChars,
    TtfLocaTable locaTable,
    out int[] newLocaTable,
    out byte[] newGlyphTable)
  {
    if (glyphChars == null)
      throw new ArgumentNullException(nameof (glyphChars));
    newLocaTable = new int[locaTable.Offsets.Length];
    int[] array = new List<int>((IEnumerable<int>) glyphChars.Keys).ToArray();
    Array.Sort<int>(array);
    uint glyphTable = 0;
    int index1 = 0;
    for (int length = array.Length; index1 < length; ++index1)
    {
      int index2 = array[index1];
      if (locaTable.Offsets.Length > 0)
        glyphTable += locaTable.Offsets[index2 + 1] - locaTable.Offsets[index2];
    }
    uint length1 = this.Align(glyphTable);
    newGlyphTable = new byte[(IntPtr) length1];
    int index3 = 0;
    int index4 = 0;
    TtfTableInfo table = this.GetTable("glyf");
    int index5 = 0;
    for (int length2 = newLocaTable.Length; index5 < length2; ++index5)
    {
      newLocaTable[index5] = index3;
      if (index4 < array.Length && array[index4] == index5)
      {
        ++index4;
        newLocaTable[index5] = index3;
        int offset = (int) locaTable.Offsets[index5];
        int count = (int) locaTable.Offsets[index5 + 1] - offset;
        if (count > 0)
        {
          this.m_reader.Seek((long) (table.Offset + offset));
          this.m_reader.Read(newGlyphTable, index3, count);
          index3 += count;
        }
      }
    }
    return glyphTable;
  }

  private int UpdateLocaTable(int[] newLocaTable, bool bLocaIsShort, out byte[] newLocaTableOut)
  {
    if (newLocaTable == null)
      throw new ArgumentNullException(nameof (newLocaTable));
    int num1 = bLocaIsShort ? newLocaTable.Length * 2 : newLocaTable.Length * 4;
    BigEndianWriter bigEndianWriter = new BigEndianWriter((int) this.Align((uint) num1));
    newLocaTableOut = bigEndianWriter.Data;
    for (int index = 0; index < newLocaTable.Length; ++index)
    {
      int num2 = newLocaTable[index];
      if (bLocaIsShort)
      {
        int num3 = num2 / 2;
        bigEndianWriter.Write((short) num3);
      }
      else
        bigEndianWriter.Write(num2);
    }
    return num1;
  }

  private byte[] GetFontProgram(
    byte[] newLocaTableOut,
    byte[] newGlyphTable,
    uint glyphTableSize,
    uint locaTableSize)
  {
    if (newLocaTableOut == null)
      throw new ArgumentNullException(nameof (newLocaTableOut));
    if (newGlyphTable == null)
      throw new ArgumentNullException(nameof (newGlyphTable));
    string[] tableNames = this.TableNames;
    short numTables = 0;
    BigEndianWriter writer = new BigEndianWriter(this.GetFontProgramLength(newLocaTableOut, newGlyphTable, out numTables));
    writer.Write(65536 /*0x010000*/);
    writer.Write(numTables);
    short entrySelector = TtfReader.s_entrySelectors[(int) numTables];
    writer.Write((short) ((1 << (int) entrySelector) * 16 /*0x10*/));
    writer.Write(entrySelector);
    writer.Write((short) (((int) numTables - (1 << (int) entrySelector)) * 16 /*0x10*/));
    this.WriteCheckSums(writer, numTables, newLocaTableOut, newGlyphTable, glyphTableSize, locaTableSize);
    this.WriteGlyphs(writer, newLocaTableOut, newGlyphTable);
    return writer.Data;
  }

  private int GetFontProgramLength(
    byte[] newLocaTableOut,
    byte[] newGlyphTable,
    out short numTables)
  {
    if (newLocaTableOut == null)
      throw new ArgumentNullException(nameof (newLocaTableOut));
    if (newGlyphTable == null)
      throw new ArgumentNullException(nameof (newGlyphTable));
    numTables = (short) 2;
    string[] tableNames = this.TableNames;
    int num = 0;
    int index = 0;
    for (int length = tableNames.Length; index < length; ++index)
    {
      string name = tableNames[index];
      if (name != "glyf" && name != "loca")
      {
        TtfTableInfo table = this.GetTable(name);
        if (!table.Empty)
        {
          ++numTables;
          num += (int) this.Align((uint) table.Length);
        }
      }
    }
    return num + newLocaTableOut.Length + newGlyphTable.Length + ((int) numTables * 16 /*0x10*/ + 12);
  }

  private int CalculateCheckSum(byte[] bytes)
  {
    if (bytes == null)
      throw new ArgumentNullException(nameof (bytes));
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    int num5 = 0;
    int num6 = 0;
    for (int index1 = (bytes.Length + 1) / 4; num6 < index1; ++num6)
    {
      int num7 = num5;
      byte[] numArray1 = bytes;
      int index2 = num1;
      int num8 = index2 + 1;
      int num9 = (int) numArray1[index2] & (int) byte.MaxValue;
      num5 = num7 + num9;
      int num10 = num4;
      byte[] numArray2 = bytes;
      int index3 = num8;
      int num11 = index3 + 1;
      int num12 = (int) numArray2[index3] & (int) byte.MaxValue;
      num4 = num10 + num12;
      int num13 = num3;
      byte[] numArray3 = bytes;
      int index4 = num11;
      int num14 = index4 + 1;
      int num15 = (int) numArray3[index4] & (int) byte.MaxValue;
      num3 = num13 + num15;
      int num16 = num2;
      byte[] numArray4 = bytes;
      int index5 = num14;
      num1 = index5 + 1;
      int num17 = (int) numArray4[index5] & (int) byte.MaxValue;
      num2 = num16 + num17;
    }
    return num2 + (num3 << 8) + (num4 << 16 /*0x10*/) + (num5 << 24);
  }

  private void WriteCheckSums(
    BigEndianWriter writer,
    short numTables,
    byte[] newLocaTableOut,
    byte[] newGlyphTable,
    uint glyphTableSize,
    uint locaTableSize)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (newLocaTableOut == null)
      throw new ArgumentNullException(nameof (newLocaTableOut));
    if (newGlyphTable == null)
      throw new ArgumentNullException(nameof (newGlyphTable));
    string[] tableNames = this.TableNames;
    uint num1 = (uint) ((int) numTables * 16 /*0x10*/ + 12);
    int index = 0;
    for (int length = tableNames.Length; index < length; ++index)
    {
      string name = tableNames[index];
      TtfTableInfo table = this.GetTable(name);
      if (!table.Empty)
      {
        writer.Write(name);
        uint num2;
        switch (name)
        {
          case "glyf":
            int checkSum1 = this.CalculateCheckSum(newGlyphTable);
            writer.Write(checkSum1);
            num2 = glyphTableSize;
            break;
          case "loca":
            int checkSum2 = this.CalculateCheckSum(newLocaTableOut);
            writer.Write(checkSum2);
            num2 = locaTableSize;
            break;
          default:
            writer.Write(table.Checksum);
            num2 = (uint) table.Length;
            break;
        }
        writer.Write(num1);
        writer.Write(num2);
        num1 += this.Align(num2);
      }
    }
  }

  private void WriteGlyphs(BigEndianWriter writer, byte[] newLocaTableOut, byte[] newGlyphTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (newLocaTableOut == null)
      throw new ArgumentNullException(nameof (newLocaTableOut));
    if (newGlyphTable == null)
      throw new ArgumentNullException(nameof (newGlyphTable));
    string[] tableNames = this.TableNames;
    int index = 0;
    for (int length = tableNames.Length; index < length; ++index)
    {
      string name = tableNames[index];
      TtfTableInfo table = this.GetTable(name);
      if (!table.Empty)
      {
        switch (name)
        {
          case "glyf":
            writer.Write(newGlyphTable);
            continue;
          case "loca":
            writer.Write(newLocaTableOut);
            continue;
          default:
            byte[] buffer = new byte[(int) this.Align((uint) table.Length)];
            this.m_reader.Seek((long) table.Offset);
            this.m_reader.Read(buffer, 0, table.Length);
            writer.Write(buffer);
            continue;
        }
      }
    }
  }

  private void InitializeFontName(TtfNameTable nameTable)
  {
    for (int index = 0; index < (int) nameTable.RecordsCount; ++index)
    {
      TtfNameRecord nameRecord = nameTable.NameRecords[index];
      if (nameRecord.NameID == (ushort) 1)
        this.m_metrics.FontFamily = !(this.metricsName != string.Empty) ? nameRecord.Name : nameRecord.Name + this.metricsName;
      else if (nameRecord.NameID == (ushort) 6)
        this.m_metrics.PostScriptName = nameRecord.Name;
      if (this.m_metrics.FontFamily != null && this.m_metrics.PostScriptName != null)
        break;
    }
  }

  private ValueType ReadStructure(BinaryReader reader, Type type)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    int num1 = !(type == (Type) null) ? Marshal.SizeOf(type) : throw new ArgumentNullException(nameof (type));
    byte[] source = reader.ReadBytes(num1);
    IntPtr num2 = Marshal.AllocHGlobal(num1);
    Marshal.Copy(source, 0, num2, num1);
    ValueType structure = (ValueType) Marshal.PtrToStructure(num2, type);
    Marshal.FreeHGlobal(num2);
    return structure;
  }

  private uint Align(uint value) => (uint) ((ulong) (value + 3U) & 18446744073709551612UL);

  private TtfGlyphInfo GetDefaultGlyph() => this.GetGlyph(' ');

  private byte[] GetFontData(Font font, uint tableName)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    IntPtr dc = GdiApi.CreateDC("DISPLAY", (string) null, (string) null, IntPtr.Zero);
    IntPtr hfont = font.ToHfont();
    IntPtr hgdiobj = GdiApi.SelectObject(dc, hfont);
    uint fontData1 = GdiApi.GetFontData(dc, tableName, 0U, (byte[]) null, 0U);
    if (fontData1 == uint.MaxValue)
    {
      int lastError = (int) KernelApi.GetLastError();
      throw new PdfException("Can't parse the font");
    }
    byte[] lpvBuffer = new byte[(IntPtr) fontData1];
    int fontData2 = (int) GdiApi.GetFontData(dc, tableName, 0U, lpvBuffer, fontData1);
    GdiApi.SelectObject(dc, hgdiobj);
    GdiApi.DeleteObject(hfont);
    GdiApi.DeleteDC(hfont);
    GdiApi.DeleteDC(dc);
    return lpvBuffer;
  }

  private int NormalizeOffset(TtfTableInfo table, string name, BigEndianReader reader)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    int num = 0;
    if (this.Font != null && !this.m_isAzureCompatible)
    {
      byte[] fontData = this.GetFontData(this.Font, this.FormatTableName(name));
      if (fontData != null)
      {
        int position = (int) reader.BaseStream.Position;
        for (int offset = table.Offset; offset >= 0; offset -= 4)
        {
          reader.BaseStream.Position = (long) offset;
          byte[] buff2 = reader.ReadBytes(table.Length);
          if (this.CompareArrays(fontData, buff2))
          {
            num = offset - table.Offset;
            break;
          }
        }
        reader.BaseStream.Position = (long) position;
      }
    }
    return num;
  }

  private bool CompareArrays(byte[] buff1, byte[] buff2)
  {
    bool flag = false;
    if (buff1.Length == buff2.Length)
    {
      int index = 0;
      while (index < buff2.Length && (int) buff2[index] == (int) buff1[index])
        ++index;
      if (index == buff2.Length)
        flag = true;
    }
    return flag;
  }

  private uint FormatTableName(string name)
  {
    byte[] numArray = name != null ? Encoding.UTF8.GetBytes(name) : throw new ArgumentNullException(nameof (name));
    int uint32 = (int) BitConverter.ToUInt32(numArray, 0);
    return (uint) ((int) numArray[3] << 24 | (int) numArray[2] << 16 /*0x10*/ | (int) numArray[1] << 8) | (uint) numArray[0];
  }
}
