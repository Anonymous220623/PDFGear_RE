// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.FontStructure
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Exporting;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Properties;
using Syncfusion.PdfViewer.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Pdf;

internal class FontStructure
{
  private const string m_replacementCharacter = "�";
  private string m_fontName;
  private FontStyle m_fontStyle;
  private PdfDictionary m_fontDictionary;
  private Dictionary<double, string> m_characterMapTable;
  private Dictionary<string, double> m_reverseMapTable;
  private Dictionary<double, string> m_cidToGidTable;
  private Dictionary<string, string> m_differencesDictionary;
  private Font m_currentFont;
  private float m_fontSize;
  private string m_fontEncoding;
  private bool isGetFontCalled;
  private Dictionary<int, int> m_fontGlyphWidth;
  private bool m_containsCmap = true;
  private bool m_fontFileContainsCmap;
  private Dictionary<double, string> tempMapTable = new Dictionary<double, string>();
  private bool m_isSameFont;
  private PrivateFontCollection pfc = new PrivateFontCollection();
  private Dictionary<int, int> m_octDecMapTable;
  private Dictionary<int, int> m_cidToGidReverseMapTable;
  internal bool IsMappingDone;
  public bool IsSystemFontExist;
  private Dictionary<int, int> m_fontGlyphWidthMapping;
  private float m_type1GlyphHeight;
  internal Dictionary<string, byte[]> m_type1FontGlyphs = new Dictionary<string, byte[]>();
  internal bool IsType1Font;
  internal bool Is1C;
  private bool m_isCID;
  internal Dictionary<int, string> differenceTable = new Dictionary<int, string>();
  internal Dictionary<int, string> differenceEncoding = new Dictionary<int, string>();
  internal Dictionary<long, CffGlyphs> type1FontReference = new Dictionary<long, CffGlyphs>();
  internal static Dictionary<int, string> unicodeCharMapTable;
  private Dictionary<int, string> m_macEncodeTable;
  public byte[] fontfilebytess;
  private FontFile2 m_fontfile2;
  internal bool m_isContainFontfile;
  internal bool IsContainFontfile2;
  internal bool IsContainFontfile3;
  internal bool isEmbedded;
  internal Dictionary<string, int> ReverseDictMapping = new Dictionary<string, int>();
  private string m_zapfPostScript;
  private string m_fontRefNumber = string.Empty;
  internal FontFile3 fontFile3Type1Font;
  private FontFile m_fontFileType1Font;
  private string m_baseFontEncoding = string.Empty;
  internal PdfPageResources Type3FontGlyphImages;
  internal Dictionary<string, PdfStream> Type3FontCharProcsDict = new Dictionary<string, PdfStream>();
  private Dictionary<int, string> m_adobeJapanCidMapTable;
  private byte m_Flag;
  internal List<string> tempStringList = new List<string>();
  private bool m_isTextExtraction;
  private string[] m_differenceDictionaryValues;
  public GraphicsPath Graphic;
  internal CffGlyphs m_cffGlyphs = new CffGlyphs();
  public MemoryStream FontStream = new MemoryStream();
  internal float DefaultGlyphWidth;
  internal Dictionary<int, string> m_macRomanMapTable = new Dictionary<int, string>();
  internal Dictionary<int, string> m_winansiMapTable = new Dictionary<int, string>();
  private PdfDictionary m_cidSystemInfoDictionary;
  internal bool IsAdobeJapanFont;
  internal bool IsAdobeIdentity;
  private string m_subType;
  public PdfName fontType;
  internal bool IsOpenTypeFont;

  internal FontFile FontFileType1Font
  {
    get => this.m_fontFileType1Font;
    set
    {
      if (value == null)
        return;
      this.m_fontFileType1Font = value;
    }
  }

  internal bool IsTextExtraction
  {
    get => this.m_isTextExtraction;
    set => this.m_isTextExtraction = value;
  }

  internal PdfDictionary FontDictionary
  {
    get => this.m_fontDictionary;
    set
    {
      if (value == null)
        return;
      this.m_fontDictionary = value;
    }
  }

  public string ZapfPostScript
  {
    get => this.m_zapfPostScript;
    set => this.m_zapfPostScript = value;
  }

  public bool Issymbol => this.GetFlag((byte) 3);

  public bool IsNonSymbol => this.GetFlag((byte) 6);

  public PdfNumber Flags => this.GetFlagValue();

  internal bool IsCID
  {
    get
    {
      this.m_isCID = this.IsCIDFontType();
      return this.m_isCID;
    }
    set => this.m_isCID = value;
  }

  internal FontFile2 GlyphFontFile2
  {
    get => this.m_fontfile2;
    set => this.m_fontfile2 = value;
  }

  internal string FontRefNumber
  {
    get => this.m_fontRefNumber;
    set => this.m_fontRefNumber = value;
  }

  internal Dictionary<int, string> MacEncodeTable
  {
    get
    {
      if (this.m_macEncodeTable == null)
        this.GetMacEncodeTable();
      return this.m_macEncodeTable;
    }
    set => this.m_macEncodeTable = value;
  }

  internal Dictionary<int, string> UnicodeCharMapTable
  {
    get
    {
      if (FontStructure.unicodeCharMapTable == null)
        FontStructure.unicodeCharMapTable = this.GetUnicodeCharMapTable();
      return FontStructure.unicodeCharMapTable;
    }
    set => FontStructure.unicodeCharMapTable = value;
  }

  internal Dictionary<string, double> ReverseMapTable
  {
    get
    {
      if (this.m_reverseMapTable == null)
        this.m_reverseMapTable = this.GetReverseMapTable();
      return this.m_reverseMapTable;
    }
    set => this.m_reverseMapTable = value;
  }

  internal Dictionary<double, string> CharacterMapTable
  {
    get
    {
      if (this.m_characterMapTable == null)
        this.m_characterMapTable = this.GetCharacterMapTable();
      return this.m_characterMapTable;
    }
    set => this.m_characterMapTable = value;
  }

  internal string[] DifferencesDictionaryValues
  {
    get
    {
      if (this.m_differenceDictionaryValues == null)
        this.m_differenceDictionaryValues = this.getMapDifference();
      return this.m_differenceDictionaryValues;
    }
    set => this.m_differenceDictionaryValues = value;
  }

  internal Dictionary<string, string> DifferencesDictionary
  {
    get
    {
      if (this.m_differencesDictionary == null)
        this.m_differencesDictionary = this.GetDifferencesDictionary();
      return this.m_differencesDictionary;
    }
    set => this.m_differencesDictionary = value;
  }

  internal Dictionary<int, int> OctDecMapTable
  {
    get
    {
      if (this.m_octDecMapTable == null)
        this.m_octDecMapTable = new Dictionary<int, int>();
      return this.m_octDecMapTable;
    }
    set => this.m_octDecMapTable = value;
  }

  internal Dictionary<int, int> CidToGidReverseMapTable
  {
    get
    {
      if (this.m_cidToGidReverseMapTable == null)
        this.m_cidToGidReverseMapTable = new Dictionary<int, int>();
      return this.m_cidToGidReverseMapTable;
    }
    set => this.m_cidToGidReverseMapTable = value;
  }

  internal Dictionary<int, string> AdobeJapanCidMapTable => this.m_adobeJapanCidMapTable;

  internal bool IsHexaDecimalString
  {
    get => ((int) this.m_Flag & 2) >> 1 != 0;
    set => this.m_Flag = (byte) ((int) this.m_Flag & 253 | (value ? 1 : 0) << 1);
  }

  public FontStructure()
  {
  }

  ~FontStructure() => this.tempMapTable = new Dictionary<double, string>();

  public FontStructure(IPdfPrimitive fontDictionary)
  {
    this.m_fontDictionary = fontDictionary as PdfDictionary;
    this.fontType = this.m_fontDictionary.Items[new PdfName("Subtype")] as PdfName;
  }

  public FontStructure(IPdfPrimitive fontDictionary, string fontRefNum)
  {
    this.m_fontDictionary = fontDictionary as PdfDictionary;
    if (this.m_fontDictionary != null && this.m_fontDictionary.ContainsKey(new PdfName("Subtype")))
      this.fontType = this.m_fontDictionary.Items[new PdfName("Subtype")] as PdfName;
    this.m_fontRefNumber = fontRefNum;
    if (!(this.fontType != (PdfName) null) || !(this.fontType.Value == "Type3"))
      return;
    this.Type3FontGlyphImages = this.GetImageResources(this.m_fontDictionary);
    if (!this.m_fontDictionary.Items.ContainsKey(new PdfName("CharProcs")))
      return;
    if (!(this.m_fontDictionary["CharProcs"] is PdfDictionary font))
      font = (this.m_fontDictionary["CharProcs"] as PdfReferenceHolder).Object as PdfDictionary;
    PdfReferenceHolder[] array1 = new PdfReferenceHolder[font.Count];
    PdfName[] array2 = new PdfName[font.Count];
    font.Items.Values.CopyTo((IPdfPrimitive[]) array1, 0);
    font.Items.Keys.CopyTo(array2, 0);
    int index = 0;
    foreach (PdfReferenceHolder pdfReferenceHolder in font.Items.Values)
    {
      this.Type3FontCharProcsDict.Add(array2[index].Value, pdfReferenceHolder.Object as PdfStream);
      ++index;
    }
  }

  internal PdfPageResources GetImageResources(PdfDictionary resourceDictionary)
  {
    PdfPageResources imageResources = new PdfPageResources();
    if (resourceDictionary.ContainsKey("Resources"))
      resourceDictionary = resourceDictionary["Resources"] as PdfDictionary;
    if (resourceDictionary != null && resourceDictionary.ContainsKey("XObject"))
    {
      IPdfPrimitive pdfPrimitive = !(resourceDictionary["XObject"] is PdfDictionary) ? (resourceDictionary["XObject"] as PdfReferenceHolder).Object : resourceDictionary["XObject"];
      if (pdfPrimitive is PdfDictionary)
      {
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair1 in ((PdfDictionary) pdfPrimitive).Items)
        {
          if ((object) (keyValuePair1.Value as PdfReferenceHolder) != null)
          {
            PdfDictionary pdfDictionary = (keyValuePair1.Value as PdfReferenceHolder).Object as PdfDictionary;
            if (pdfDictionary.ContainsKey("Subtype"))
            {
              if ((pdfDictionary["Subtype"] as PdfName).Value == "Image")
              {
                ImageStructure resource = new ImageStructure((IPdfPrimitive) pdfDictionary, new PdfMatrix());
                Image embeddedImage = resource.EmbeddedImage;
                imageResources.Add(keyValuePair1.Key.Value, (object) resource);
              }
              else if ((pdfDictionary["Subtype"] as PdfName).Value == "Form")
              {
                if (pdfDictionary.ContainsKey("Resources") && pdfDictionary["Resources"] is PdfDictionary)
                {
                  foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair2 in (pdfDictionary["Resources"] as PdfDictionary).Items)
                  {
                    if (keyValuePair2.Key.Value == "XObject" && keyValuePair2.Value is PdfDictionary)
                    {
                      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair3 in (keyValuePair2.Value as PdfDictionary).Items)
                      {
                        PdfStream pdfStream = pdfDictionary as PdfStream;
                        pdfStream.isSkip = true;
                        pdfStream.Decompress();
                        MemoryStream internalStream = pdfStream.InternalStream;
                        internalStream.Position = 0L;
                        new PdfReader((Stream) internalStream).Position = 0L;
                      }
                    }
                  }
                }
                imageResources.Add(keyValuePair1.Key.Value, (object) new XObjectElement(pdfDictionary, keyValuePair1.Key.Value));
              }
              if (!imageResources.ContainsKey(keyValuePair1.Key.Value))
                imageResources.Add(keyValuePair1.Key.Value, (object) new XObjectElement(pdfDictionary, keyValuePair1.Key.Value));
            }
          }
          else
          {
            PdfDictionary xobjectDictionary = keyValuePair1.Value as PdfDictionary;
            imageResources.Add(keyValuePair1.Key.Value, (object) new XObjectElement(xobjectDictionary, keyValuePair1.Key.Value));
          }
        }
      }
    }
    return imageResources;
  }

  public string FontName
  {
    get
    {
      if (this.m_fontName == null)
        this.m_fontName = this.GetFontName();
      return this.m_fontName;
    }
  }

  public FontStyle FontStyle
  {
    get
    {
      if (this.m_fontStyle == FontStyle.Regular)
        this.m_fontStyle = this.GetFontStyle();
      return this.m_fontStyle;
    }
  }

  public bool IsSameFont
  {
    get => this.m_isSameFont;
    set => this.m_isSameFont = value;
  }

  internal Dictionary<double, string> CidToGidMap => this.m_cidToGidTable;

  internal Dictionary<int, int> FontGlyphWidths
  {
    get
    {
      if (this.FontEncoding == "Identity-H" || this.FontEncoding == "Identity#2DH")
        this.GetGlyphWidths();
      else
        this.GetGlyphWidthsNonIdH();
      return this.m_fontGlyphWidth;
    }
    set => this.m_fontGlyphWidth = value;
  }

  internal float Type1GlyphHeight => this.m_type1GlyphHeight;

  public Font CurrentFont
  {
    get
    {
      if (this.m_currentFont == null && !this.isGetFontCalled && (double) this.m_fontSize != 0.0)
        this.m_currentFont = this.GetFont(this.m_fontSize);
      return this.m_currentFont;
    }
  }

  public float FontSize
  {
    get => this.m_fontSize;
    set
    {
      this.m_fontSize = value;
      this.m_currentFont = this.GetFont(this.m_fontSize);
    }
  }

  public string FontEncoding
  {
    get
    {
      if (this.m_fontEncoding == null)
        this.m_fontEncoding = this.GetFontEncoding();
      return this.m_fontEncoding;
    }
  }

  public string BaseFontEncoding => this.m_baseFontEncoding;

  internal bool ContainsCmap => this.m_fontFileContainsCmap;

  public string Decode(string textToDecode, bool isSameFont)
  {
    string str1 = string.Empty;
    string str2 = textToDecode;
    this.m_isSameFont = isSameFont;
    bool flag1 = false;
    switch (str2[0])
    {
      case '(':
        if (str2.Contains("\\\n"))
        {
          StringBuilder stringBuilder = new StringBuilder(str2);
          stringBuilder.Replace("\\\n", "");
          str2 = stringBuilder.ToString();
        }
        str1 = this.GetLiteralString(str2.Substring(1, str2.Length - 2));
        if (str1.Contains("\\\\") && this.FontEncoding == "Identity-H")
        {
          flag1 = true;
          str1 = this.SkipEscapeSequence(str1);
        }
        if (this.m_fontDictionary.ContainsKey("Encoding") && (object) (this.m_fontDictionary["Encoding"] as PdfName) != null)
        {
          string str3 = (this.m_fontDictionary["Encoding"] as PdfName).Value;
          if (str3 == "Identity-H" || str3 == "GBK-EUC-H")
          {
            string text = str1;
            if (!flag1)
            {
              if (text.Contains("\\a") || text.Contains("\\") || text.Contains("\\b") || text.Contains("\\f") || text.Contains("\\r") || text.Contains("\\t") || text.Contains("\\n") || text.Contains("\\v") || text.Contains("\\'"))
              {
                while (text.Contains("\\a") || text.Contains("\\") || text.Contains("\\b") || text.Contains("\\f") || text.Contains("\\r") || text.Contains("\\t") || text.Contains("\\n") || text.Contains("\\v") || text.Contains("\\'"))
                  text = this.SkipEscapeSequence(text);
              }
              else
                text = this.SkipEscapeSequence(text);
            }
            List<byte> byteList = new List<byte>();
            foreach (char ch in text)
              byteList.Add((byte) ch);
            switch (str3)
            {
              case "Identity-H":
                str1 = Encoding.BigEndianUnicode.GetString(byteList.ToArray());
                break;
              case "GBK-EUC-H":
                str1 = Encoding.GetEncoding("gb2312").GetString(byteList.ToArray());
                break;
            }
            if (str1.Contains("\\"))
            {
              str1 = str1.Replace("\\", "\\\\");
              break;
            }
            break;
          }
          break;
        }
        break;
      case '<':
        str1 = this.GetHexaDecimalString(str2.Substring(1, str2.Length - 2));
        if (str1.Contains("\\"))
        {
          str1 = str1.Replace("\\", "\\\\");
          break;
        }
        break;
      case '[':
        if (str2.Contains("\\\n"))
        {
          StringBuilder stringBuilder = new StringBuilder(str2);
          stringBuilder.Replace("\\\n", "");
          str2 = stringBuilder.ToString();
        }
        int num1;
        for (string str4 = str2.Substring(1, str2.Length - 2); str4.Length > 0; str4 = str4.Substring(num1 + 1, str4.Length - num1 - 1))
        {
          bool flag2 = false;
          int num2 = str4.IndexOf('(');
          num1 = str4.IndexOf(')');
          int num3 = str4.IndexOf('<');
          int num4 = str4.IndexOf('>');
          if (num3 < num2 && num3 > -1)
          {
            num2 = num3;
            num1 = num4;
            flag2 = true;
          }
          if (num2 < 0)
          {
            num2 = str4.IndexOf('<');
            num1 = str4.IndexOf('>');
            if (num2 >= 0)
              flag2 = true;
            else
              break;
          }
          else if (num1 > 0)
          {
            while (str4[num1 - 1] == '\\' && str4.IndexOf(')', num1 + 1) >= 0)
              num1 = str4.IndexOf(')', num1 + 1);
          }
          string str5 = str4.Substring(num2 + 1, num1 - num2 - 1);
          str1 = !flag2 ? (flag2 || !(this.FontEncoding == "Identity-H") ? str1 + this.GetLiteralString(str5) : str1 + this.GetRawString(str5)) : str1 + this.GetHexaDecimalString(str5);
        }
        break;
    }
    if (str1.Contains("\0") && !this.CharacterMapTable.ContainsKey(0.0) && this.CharacterMapTable.Count > 0)
      str1 = str1.Replace("\0", "");
    if (!this.IsTextExtraction)
      str1 = this.SkipEscapeSequence(str1);
    if ((this.FontEncoding != "Identity-H" && this.fontType.Value != "TrueType" || this.FontEncoding == "Identity-H" && this.IsType1Font || this.FontEncoding == "Identity-H" && !this.isEmbedded) && this.fontType.Value != "Type3" && (!(this.FontName == "MinionPro") || !(this.FontEncoding == "Encoding")))
    {
      this.IsMappingDone = true;
      string str6 = str1;
      if (this.DifferencesDictionary.Count == this.CharacterMapTable.Count && this.FontEncoding != "" && this.FontName == "Univers")
      {
        if (this.DifferencesDictionary != null && this.DifferencesDictionary.Count > 0)
          str1 = this.MapDifferences(str1);
        else if (this.CharacterMapTable != null && this.CharacterMapTable.Count > 0)
          str1 = this.MapCharactersFromTable(str1);
        else if (this.FontEncoding != "")
          str1 = this.SkipEscapeSequence(str1);
      }
      else if (this.CharacterMapTable != null && this.CharacterMapTable.Count > 0)
        str1 = this.MapCharactersFromTable(str1);
      else if (this.DifferencesDictionary != null && this.DifferencesDictionary.Count > 0)
        str1 = this.MapDifferences(str1);
      else if (this.FontEncoding != "")
        str1 = this.SkipEscapeSequence(str1);
      if (str1 == str6 && this.FontName == "AllAndNone")
        this.IsMappingDone = false;
    }
    if (this.m_cidToGidTable != null)
      str1 = this.MapCidToGid(str1);
    if (this.FontName == "ZapfDingbats" && !this.isEmbedded)
      str1 = this.MapZapf(str1);
    return str1;
  }

  internal string ToGetEncodedText(string textElement, bool isSameFont)
  {
    string text1 = string.Empty;
    string str1 = textElement;
    this.m_isSameFont = isSameFont;
    bool flag1 = false;
    switch (str1[0])
    {
      case '(':
        if (str1.Contains("\\\n"))
        {
          StringBuilder stringBuilder = new StringBuilder(str1);
          stringBuilder.Replace("\\\n", "");
          str1 = stringBuilder.ToString();
        }
        text1 = this.GetLiteralString(str1.Substring(1, str1.Length - 2));
        if (text1.Contains("\\\\") && this.FontEncoding == "Identity-H")
        {
          flag1 = true;
          text1 = this.SkipEscapeSequence(text1);
        }
        if (this.m_fontDictionary.ContainsKey("Encoding") && (object) (this.m_fontDictionary["Encoding"] as PdfName) != null)
        {
          string str2 = (this.m_fontDictionary["Encoding"] as PdfName).Value;
          if (str2 == "Identity-H" || str2 == "GBK-EUC-H")
          {
            string text2 = text1;
            if (!flag1)
            {
              if (text2.Contains("\\a") || text2.Contains("\\") || text2.Contains("\\b") || text2.Contains("\\f") || text2.Contains("\\r") || text2.Contains("\\t") || text2.Contains("\\n") || text2.Contains("\\v") || text2.Contains("\\'"))
              {
                while (text2.Contains("\\a") || text2.Contains("\\") || text2.Contains("\\b") || text2.Contains("\\f") || text2.Contains("\\r") || text2.Contains("\\t") || text2.Contains("\\n") || text2.Contains("\\v") || text2.Contains("\\'"))
                  text2 = this.SkipEscapeSequence(text2);
              }
              else
                text2 = this.SkipEscapeSequence(text2);
            }
            List<byte> byteList = new List<byte>();
            foreach (char ch in text2)
              byteList.Add((byte) ch);
            switch (str2)
            {
              case "Identity-H":
                text1 = Encoding.BigEndianUnicode.GetString(byteList.ToArray());
                break;
              case "GBK-EUC-H":
                text1 = Encoding.GetEncoding("gb2312").GetString(byteList.ToArray());
                break;
            }
            if (text1.Contains("\\"))
            {
              text1 = text1.Replace("\\", "\\\\");
              break;
            }
            break;
          }
          break;
        }
        break;
      case '<':
        text1 = this.GetHexaDecimalString(str1.Substring(1, str1.Length - 2));
        if (text1.Contains("\\"))
        {
          text1 = text1.Replace("\\", "\\\\");
          break;
        }
        break;
      case '[':
        if (str1.Contains("\\\n"))
        {
          StringBuilder stringBuilder = new StringBuilder(str1);
          stringBuilder.Replace("\\\n", "");
          str1 = stringBuilder.ToString();
        }
        int num1;
        for (string str3 = str1.Substring(1, str1.Length - 2); str3.Length > 0; str3 = str3.Substring(num1 + 1, str3.Length - num1 - 1))
        {
          bool flag2 = false;
          int num2 = str3.IndexOf('(');
          num1 = str3.IndexOf(')');
          int num3 = str3.IndexOf('<');
          int num4 = str3.IndexOf('>');
          for (int index = num1 + 1; index < str3.Length && str3[index] != '('; ++index)
          {
            if (str3[index] == ')')
            {
              num1 = index;
              break;
            }
          }
          if (num3 < num2 && num3 > -1)
          {
            num2 = num3;
            num1 = num4;
            flag2 = true;
          }
          if (num2 < 0)
          {
            num2 = str3.IndexOf('<');
            num1 = str3.IndexOf('>');
            if (num2 >= 0)
              flag2 = true;
            else
              break;
          }
          else if (num1 > 0)
          {
            while (str3[num1 - 1] == '\\' && str3.IndexOf(')', num1 + 1) >= 0)
              num1 = str3.IndexOf(')', num1 + 1);
          }
          string str4 = str3.Substring(num2 + 1, num1 - num2 - 1);
          text1 = !flag2 ? (flag2 || !(this.FontEncoding == "Identity-H") ? text1 + this.GetLiteralString(str4) : text1 + this.GetRawString(str4)) : text1 + this.GetHexaDecimalString(str4);
        }
        break;
    }
    if (text1.Contains("\0") && !this.CharacterMapTable.ContainsKey(0.0) && this.CharacterMapTable.Count > 0)
      text1 = text1.Replace("\0", "");
    if (!this.IsTextExtraction)
      text1 = this.SkipEscapeSequence(text1);
    return text1;
  }

  public string DecodeType3FontData(string textToDecode)
  {
    string str = textToDecode;
    if (this.DifferencesDictionary != null && this.DifferencesDictionary.Count > 0)
      str = this.MapDifferences(str);
    else if (this.CharacterMapTable != null && this.CharacterMapTable.Count > 0)
      str = this.MapCharactersFromTable(str);
    else if (this.FontEncoding != "")
      str = this.SkipEscapeSequence(str);
    return str;
  }

  public List<string> DecodeTextTJ(string textToDecode, bool isSameFont)
  {
    string text = string.Empty;
    string str1 = textToDecode;
    this.m_isSameFont = isSameFont;
    List<string> stringList = new List<string>();
    this.IsHexaDecimalString = false;
    bool flag1 = false;
    switch (str1[0])
    {
      case '(':
        if (str1.Contains("\\\n"))
        {
          StringBuilder stringBuilder = new StringBuilder(str1);
          stringBuilder.Replace("\\\n", "");
          str1 = stringBuilder.ToString();
        }
        text = this.SkipEscapeSequence(this.GetLiteralString(str1.Substring(1, str1.Length - 2)));
        if (this.m_fontDictionary.ContainsKey("Encoding") && (object) (this.m_fontDictionary["Encoding"] as PdfName) != null && (this.m_fontDictionary["Encoding"] as PdfName).Value == "Identity-H")
        {
          List<byte> byteList = new List<byte>();
          foreach (char ch in text)
            byteList.Add((byte) ch);
          text = Encoding.BigEndianUnicode.GetString(byteList.ToArray());
          break;
        }
        break;
      case '<':
        text = this.GetHexaDecimalString(str1.Substring(1, str1.Length - 2));
        break;
      case '[':
        if (str1.Contains("\\\n"))
        {
          StringBuilder stringBuilder = new StringBuilder(str1);
          stringBuilder.Replace("\\\n", "");
          str1 = stringBuilder.ToString();
        }
        int num1;
        for (string str2 = str1.Substring(1, str1.Length - 2); str2.Length > 0; str2 = str2.Substring(num1 + 1, str2.Length - num1 - 1))
        {
          bool flag2 = false;
          bool flag3 = false;
          int length = str2.IndexOf('(');
          num1 = str2.IndexOf(')');
          for (int index = num1 + 1; index < str2.Length && str2[index] != '('; ++index)
          {
            if (str2[index] == ')')
            {
              num1 = index;
              break;
            }
          }
          int num2 = str2.IndexOf('<');
          int num3 = str2.IndexOf('>');
          if (num2 < length && num2 > -1)
          {
            length = num2;
            num1 = num3;
            flag2 = true;
          }
          if (length < 0)
          {
            length = str2.IndexOf('<');
            num1 = str2.IndexOf('>');
            if (length >= 0)
            {
              flag2 = true;
            }
            else
            {
              string str3 = str2;
              stringList.Add(str3);
              break;
            }
          }
          if (num1 < 0 && str2.Length > 0)
          {
            string str4 = str2;
            stringList.Add(str4);
            break;
          }
          if (num1 > 0)
          {
            while (str2[num1 - 1] == '\\' && (num1 - 1 <= 0 || str2[num1 - 2] != '\\') && str2.IndexOf(')', num1 + 1) >= 0)
              num1 = str2.IndexOf(')', num1 + 1);
          }
          if (length != 0)
          {
            string str5 = str2.Substring(0, length);
            stringList.Add(str5);
          }
          string str6 = str2.Substring(length + 1, num1 - length - 1);
          string str7;
          if (flag2)
          {
            str7 = this.GetHexaDecimalString(str6);
            if (str7.Contains("\\"))
              str7 = str7.Replace("\\", "\\\\");
            text += str7;
          }
          else if (!flag2 && this.FontEncoding == "Identity-H" && this.ReverseMapTable.Count != 0 && this.FontName != "MinionPro")
          {
            str7 = this.GetRawString(str6);
            text += str7;
          }
          else
          {
            str7 = this.GetLiteralString(str6);
            if (str7.Contains("\\\\") && this.FontEncoding == "Encoding")
              flag3 = true;
            text += str7;
          }
          if (str7.Contains("\\000"))
            str7 = str7.Replace("\\000", "");
          bool flag4 = false;
          if (str7.Contains("\0") && (!this.CharacterMapTable.ContainsKey(0.0) || this.CharacterMapTable.ContainsValue("\0")) && (this.CharacterMapTable.Count > 0 || this.IsCID && !this.FontDictionary.ContainsKey("ToUnicode")))
          {
            if (this.FontEncoding == "Identity-H" && this.IsType1Font && this.CharacterMapTable.Count == 0)
            {
              int num4 = 0;
              bool flag5 = false;
              foreach (char ch in this.SkipEscapeSequence(text))
              {
                if (num4 % 2 == 0 && ch != char.MinValue && ch != '(' && ch != ')')
                {
                  flag5 = true;
                  break;
                }
                if (num4 % 2 == 0 && (ch == '(' || ch == ')'))
                  num4 = 0;
                else
                  ++num4;
              }
              if (flag5)
              {
                flag4 = true;
                List<byte> byteList = new List<byte>();
                foreach (char ch in str7)
                  byteList.Add((byte) ch);
                str7 = Encoding.BigEndianUnicode.GetString(byteList.ToArray());
                byteList.Clear();
              }
              else
                str7 = str7.Replace("\0", "");
            }
            else if (this.IsCID && this.CharacterMapTable.Count > 0 && !flag4 && !flag2 && this.FontEncoding == "Identity-H" && this.ReverseMapTable.Count != 0 && this.FontName != "MinionPro" && this.GlyphFontFile2 != null && this.CidToGidMap != null && this.CidToGidMap.Count > 0 && this.CharacterMapTable.Count == this.CidToGidMap.Count)
            {
              List<byte> byteList = new List<byte>();
              flag4 = true;
              foreach (char ch in str7)
                byteList.Add((byte) ch);
              if (str7.Contains("\\"))
              {
                str7 = str7.Replace("\0", "");
              }
              else
              {
                str7 = Encoding.BigEndianUnicode.GetString(byteList.ToArray());
                byteList.Clear();
                flag4 = true;
              }
            }
            else
              str7 = str7.Replace("\0", "");
          }
          if (this.IsCID && this.CharacterMapTable.Count > 0 && !flag4 && !flag2 && this.FontEncoding == "Identity-H" && this.ReverseMapTable.Count != 0 && this.FontName != "MinionPro" && this.GlyphFontFile2 != null && this.CidToGidMap != null && this.CidToGidMap.Count > 0 && this.CharacterMapTable.Count == this.CidToGidMap.Count && this.isMpdfaaFonts())
          {
            List<byte> byteList = new List<byte>();
            foreach (char ch in str7)
              byteList.Add((byte) ch);
            str7 = Encoding.BigEndianUnicode.GetString(byteList.ToArray());
            byteList.Clear();
          }
          if (!this.IsTextExtraction)
            str7 = this.SkipEscapeSequence(str7);
          if ((this.FontEncoding != "Identity-H" && this.fontType.Value != "TrueType" || this.FontEncoding == "Identity-H" && this.IsType1Font || this.FontEncoding == "Identity-H" && !this.isEmbedded) && (this.FontName != "MinionPro" || this.FontName == "MinionPro" && this.IsCID) && this.fontType.Value != "Type3" && !flag3)
          {
            this.IsMappingDone = true;
            if (this.CharacterMapTable != null && this.CharacterMapTable.Count > 0)
              str7 = this.MapCharactersFromTable(str7);
            else if (this.DifferencesDictionary != null && this.DifferencesDictionary.Count > 0)
              str7 = this.MapDifferences(str7);
            else if (this.FontEncoding != "")
              str7 = this.SkipEscapeSequence(str7);
          }
          if (this.m_cidToGidTable != null)
            str7 = this.MapCidToGid(str7);
          if (str7.Length > 0)
          {
            if (str7[0] >= '\u0E00' && str7[0] <= '\u0E7F' && stringList.Count > 0)
            {
              string str8 = stringList[0];
              string str9 = str8.Remove(str8.Length - 1) + str7;
              stringList[0] = str9 + "s";
            }
            else if ((str7[0] == ' ' || str7[0] == '/') && str7.Length > 1)
            {
              if (str7[1] >= '\u0E00' && str7[1] <= '\u0E7F' && stringList.Count > 0)
              {
                string str10 = stringList[0];
                string str11 = str10.Remove(str10.Length - 1) + str7;
                stringList[0] = str11 + "s";
              }
              else
              {
                string str12 = str7 + "s";
                stringList.Add(str12);
              }
            }
            else
            {
              string str13 = str7 + "s";
              stringList.Add(str13);
            }
          }
          else
          {
            string str14 = str7 + "s";
            stringList.Add(str14);
          }
          flag1 = false;
        }
        break;
    }
    this.SkipEscapeSequence(text);
    return stringList;
  }

  internal List<string> DecodeTextExtractionTJ(string textToDecode, bool isSameFont)
  {
    string text1 = string.Empty;
    string str1 = textToDecode;
    this.m_isSameFont = isSameFont;
    List<string> stringList = new List<string>();
    switch (str1[0])
    {
      case '(':
        if (str1.Contains("\\\n"))
        {
          StringBuilder stringBuilder = new StringBuilder(str1);
          stringBuilder.Replace("\\\n", "");
          str1 = stringBuilder.ToString();
        }
        text1 = this.GetLiteralString(str1.Substring(1, str1.Length - 2));
        if (this.m_fontDictionary != null && this.m_fontDictionary.ContainsKey("Encoding") && (object) (this.m_fontDictionary["Encoding"] as PdfName) != null && (this.m_fontDictionary["Encoding"] as PdfName).Value == "Identity-H")
        {
          string str2 = this.SkipEscapeSequence(text1);
          List<byte> byteList = new List<byte>();
          foreach (char ch in str2)
            byteList.Add((byte) ch);
          text1 = Encoding.BigEndianUnicode.GetString(byteList.ToArray());
          break;
        }
        break;
      case '<':
        text1 = this.GetHexaDecimalString(str1.Substring(1, str1.Length - 2));
        break;
      case '[':
        if (str1.Contains("\\\n"))
        {
          StringBuilder stringBuilder = new StringBuilder(str1);
          stringBuilder.Replace("\\\n", "");
          str1 = stringBuilder.ToString();
        }
        int num1;
        for (string str3 = str1.Substring(1, str1.Length - 2); str3.Length > 0; str3 = str3.Substring(num1 + 1, str3.Length - num1 - 1))
        {
          bool flag = false;
          int length = str3.IndexOf('(');
          num1 = str3.IndexOf(')');
          for (int index = num1 + 1; index < str3.Length && str3[index] != '('; ++index)
          {
            if (str3[index] == ')')
            {
              num1 = index;
              break;
            }
          }
          int num2 = str3.IndexOf('<');
          int num3 = str3.IndexOf('>');
          if (num2 < length && num2 > -1)
          {
            length = num2;
            num1 = num3;
            flag = true;
          }
          if (length < 0)
          {
            length = str3.IndexOf('<');
            num1 = str3.IndexOf('>');
            if (length >= 0)
            {
              flag = true;
            }
            else
            {
              string str4 = str3;
              stringList.Add(str4);
              break;
            }
          }
          if (num1 < 0 && str3.Length > 0)
          {
            string str5 = str3;
            stringList.Add(str5);
            break;
          }
          if (num1 > 0)
          {
            while (str3[num1 - 1] == '\\' && (num1 - 1 <= 0 || str3[num1 - 2] != '\\') && str3.IndexOf(')', num1 + 1) >= 0)
              num1 = str3.IndexOf(')', num1 + 1);
          }
          if (length != 0)
          {
            string str6 = str3.Substring(0, length);
            stringList.Add(str6);
          }
          string str7 = str3.Substring(length + 1, num1 - length - 1);
          string text2;
          if (flag)
          {
            text2 = this.GetHexaDecimalString(str7);
            if (text2.Contains("\\"))
              text2 = text2.Replace("\\", "\\\\");
            text1 += text2;
          }
          else
          {
            text2 = this.GetLiteralString(str7);
            text1 += text2;
          }
          if (text2.Contains("\0") && !this.CharacterMapTable.ContainsKey(0.0))
            text2 = text2.Replace("\0", "");
          string str8 = this.SkipEscapeSequence(text2);
          if (this.FontEncoding != "Identity-H" || this.FontEncoding == "Identity-H" && this.CurrentFont == null || this.FontEncoding == "Identity-H" && this.m_containsCmap)
          {
            this.IsMappingDone = true;
            if (this.CharacterMapTable != null && this.CharacterMapTable.Count > 0)
              str8 = this.MapCharactersFromTable(str8);
            else if (this.DifferencesDictionary != null && this.DifferencesDictionary.Count > 0)
              str8 = this.MapDifferences(str8);
            else if (this.FontEncoding != "")
              str8 = this.SkipEscapeSequence(str8);
          }
          if (this.m_cidToGidTable != null && !this.m_isTextExtraction)
            str8 = this.MapCidToGid(str8);
          if (str8.Length > 0)
          {
            if (str8[0] >= '\u0E00' && str8[0] <= '\u0E7F' && stringList.Count > 0)
            {
              string str9 = stringList[0];
              string str10 = str9.Remove(str9.Length - 1) + str8;
              stringList[0] = str10 + "s";
            }
            else if ((str8[0] == ' ' || str8[0] == '/') && str8.Length > 1)
            {
              if (str8[1] >= '\u0E00' && str8[1] <= '\u0E7F' && stringList.Count > 0)
              {
                string str11 = stringList[0];
                string str12 = str11.Remove(str11.Length - 1) + str8;
                stringList[0] = str12 + "s";
              }
              else
              {
                string str13 = str8 + "s";
                stringList.Add(str13);
              }
            }
            else
            {
              string str14 = str8 + "s";
              stringList.Add(str14);
            }
          }
          else
          {
            string str15 = str8 + "s";
            stringList.Add(str15);
          }
        }
        break;
    }
    this.SkipEscapeSequence(text1);
    return stringList;
  }

  private bool HasEscapeCharacter(string text)
  {
    return text.Contains("\\a") || text.Contains("\\") || text.Contains("\\b") || text.Contains("\\f") || text.Contains("\\r") || text.Contains("\\t") || text.Contains("\\n") || text.Contains("\\v") || text.Contains("\\'") || text.Contains("\\0");
  }

  public string DecodeTextExtraction(string textToDecode, bool isSameFont)
  {
    string str1 = string.Empty;
    string str2 = textToDecode;
    this.m_isSameFont = isSameFont;
    bool flag1 = false;
    switch (str2[0])
    {
      case '(':
        if (str2.Contains("\\\n"))
        {
          StringBuilder stringBuilder = new StringBuilder(str2);
          stringBuilder.Replace("\\\n", "");
          str2 = stringBuilder.ToString();
        }
        str1 = this.GetLiteralString(str2.Substring(1, str2.Length - 2));
        if (str1.Contains("\\\\") && this.FontEncoding == "Identity-H")
        {
          flag1 = true;
          str1 = this.SkipEscapeSequence(str1);
        }
        if (this.m_fontDictionary != null && this.m_fontDictionary.ContainsKey("Encoding") && (object) (this.m_fontDictionary["Encoding"] as PdfName) != null && (this.m_fontDictionary["Encoding"] as PdfName).Value == "Identity-H")
        {
          string text = str1;
          if (!flag1)
          {
            do
            {
              text = this.SkipEscapeSequence(text);
            }
            while (this.HasEscapeCharacter(text));
          }
          List<byte> byteList = new List<byte>();
          foreach (char ch in text)
            byteList.Add((byte) ch);
          str1 = Encoding.BigEndianUnicode.GetString(byteList.ToArray());
          break;
        }
        break;
      case '<':
        str1 = this.GetHexaDecimalString(str2.Substring(1, str2.Length - 2));
        break;
      case '[':
        if (str2.Contains("\\\n"))
        {
          StringBuilder stringBuilder = new StringBuilder(str2);
          stringBuilder.Replace("\\\n", "");
          str2 = stringBuilder.ToString();
        }
        int num1;
        for (string str3 = str2.Substring(1, str2.Length - 2); str3.Length > 0; str3 = str3.Substring(num1 + 1, str3.Length - num1 - 1))
        {
          bool flag2 = false;
          int num2 = str3.IndexOf('(');
          num1 = str3.IndexOf(')');
          int num3 = str3.IndexOf('<');
          int num4 = str3.IndexOf('>');
          if (num3 < num2 && num3 > -1)
          {
            num2 = num3;
            num1 = num4;
            flag2 = true;
          }
          if (num2 < 0)
          {
            num2 = str3.IndexOf('<');
            num1 = str3.IndexOf('>');
            if (num2 >= 0)
              flag2 = true;
            else
              break;
          }
          else if (num1 > 0)
          {
            while (str3[num1 - 1] == '\\' && str3.IndexOf(')', num1 + 1) >= 0)
              num1 = str3.IndexOf(')', num1 + 1);
          }
          string str4 = str3.Substring(num2 + 1, num1 - num2 - 1);
          str1 = !flag2 ? str1 + this.GetLiteralString(str4) : str1 + this.GetHexaDecimalString(str4);
        }
        break;
    }
    if (this.FontEncoding != "Identity-H" || this.FontEncoding == "Identity-H" && this.CurrentFont == null || this.FontEncoding == "Identity-H" && this.m_containsCmap)
    {
      this.IsMappingDone = true;
      if (this.CharacterMapTable != null && this.CharacterMapTable.Count > 0)
        str1 = this.MapCharactersFromTable(str1);
      else if (this.DifferencesDictionary != null && this.DifferencesDictionary.Count > 0)
        str1 = this.MapDifferences(str1);
      else if (this.FontEncoding != "")
        str1 = this.SkipEscapeSequence(str1);
    }
    if (this.m_cidToGidTable != null && !this.IsTextExtraction)
      str1 = this.MapCidToGid(str1);
    if (this.FontName == "ZapfDingbats" && !this.isEmbedded)
      str1 = this.MapZapf(str1);
    if (this.FontEncoding == "MacRomanEncoding")
    {
      string empty = string.Empty;
      foreach (char key in str1)
      {
        if ((byte) key > (byte) 126)
        {
          string str5 = this.MacEncodeTable[(int) (byte) key];
          empty += str5;
        }
        else
          empty += (string) (object) key;
      }
      if (empty != string.Empty)
        str1 = empty;
    }
    if (str1.Contains("\u0092"))
      str1 = str1.Replace("\u0092", "’");
    else if (this.FontEncoding == "WinAnsiEncoding" && str1 == "\u0080")
    {
      byte[] bytes = new byte[str1.Length];
      for (int index = 0; index < str1.Length; ++index)
        bytes[index] = (byte) str1[index];
      str1 = Encoding.GetEncoding("Windows-1252").GetString(bytes);
    }
    return str1;
  }

  internal string GetLiteralString(string encodedText)
  {
    string literalString = encodedText;
    int startIndex = -1;
    int num1 = 3;
    bool flag1 = false;
    bool flag2 = false;
    while (literalString.Contains("\\") && !literalString.Contains("\\\\") || literalString.Contains("\0"))
    {
      string empty1 = string.Empty;
      if (literalString.IndexOf('\\', startIndex + 1) >= 0)
      {
        startIndex = literalString.IndexOf('\\', startIndex + 1);
      }
      else
      {
        startIndex = literalString.IndexOf(char.MinValue, startIndex + 1);
        if (startIndex >= 0)
          num1 = 2;
        else
          break;
      }
      for (int index = startIndex + 1; index <= startIndex + num1; ++index)
      {
        if (index < literalString.Length)
        {
          int result = 0;
          if (int.TryParse(literalString[index].ToString(), out result))
          {
            if (result <= 8)
              empty1 += (string) (object) literalString[index];
          }
          else
          {
            empty1 = string.Empty;
            break;
          }
        }
        else
          empty1 = string.Empty;
      }
      if (empty1 != string.Empty)
      {
        int uint64 = (int) Convert.ToUInt64(empty1, 8);
        string empty2 = string.Empty;
        char ch = (char) uint64;
        string str;
        if (this.CharacterMapTable != null && this.CharacterMapTable.Count > 0)
          str = ch.ToString();
        else if (this.DifferencesDictionary != null && this.DifferencesDictionary.Count > 0 && this.DifferencesDictionary.ContainsKey(uint64.ToString()))
          str = ch.ToString();
        else if (this.FontEncoding != "MacRomanEncoding")
        {
          str = (Encoding.GetEncoding(1252) ?? Encoding.UTF8).GetString(new byte[1]
          {
            Convert.ToByte(uint64)
          });
          char[] charArray = str.ToCharArray();
          int key = 0;
          foreach (int num2 in charArray)
            key = num2;
          if (!this.OctDecMapTable.ContainsKey(key))
            this.OctDecMapTable.Add(key, uint64);
          flag2 = true;
        }
        else
        {
          str = (Encoding.GetEncoding(10000) ?? Encoding.UTF8).GetString(new byte[1]
          {
            Convert.ToByte(uint64)
          });
          char[] charArray = str.ToCharArray();
          int key = 0;
          foreach (int num3 in charArray)
            key = num3;
          if (!this.OctDecMapTable.ContainsKey(key))
            this.OctDecMapTable.Add(key, uint64);
          flag1 = true;
        }
        literalString = literalString.Remove(startIndex, num1 + 1).Insert(startIndex, str);
      }
    }
    if (literalString.Contains("\\") && this.m_fontEncoding != "Identity-H" && literalString.Length > 1)
    {
      int num4 = literalString.IndexOf("\\");
      switch (literalString[num4 + 1])
      {
        case '(':
        case ')':
          Regex.Unescape(literalString);
          break;
        default:
          if (!literalString.Contains("\\\\"))
          {
            for (int index = 0; literalString.Contains("\\") && literalString.Length != index; literalString = this.SkipEscapeSequence(literalString))
              index = literalString.Length;
            break;
          }
          break;
      }
    }
    if (this.FontEncoding == "MacRomanEncoding" && !flag1)
    {
      this.GetMacEncodeTable();
      foreach (char ch in literalString)
      {
        int key = (int) ch;
        Encoding encoding = Encoding.GetEncoding(10000) ?? Encoding.UTF8;
        if (this.m_macEncodeTable.ContainsValue(ch.ToString()) && !this.m_macRomanMapTable.ContainsKey(key))
          this.m_macRomanMapTable.Add(key, encoding.GetString(new byte[1]
          {
            Convert.ToByte(key)
          }));
      }
    }
    if (this.FontEncoding == "WinAnsiEncoding" && !flag2)
    {
      string empty = string.Empty;
      foreach (char key1 in encodedText)
      {
        int key2 = (int) key1;
        switch (key2)
        {
          case (int) sbyte.MaxValue:
          case 129:
          case 131:
          case 136:
          case 141:
          case 143:
          case 144 /*0x90*/:
          case 152:
          case 157:
          case 173:
          case 209:
            char ch = '\u0095';
            if (!this.m_winansiMapTable.ContainsKey(key2))
            {
              this.m_winansiMapTable.Add((int) key1, ch.ToString());
              break;
            }
            break;
        }
      }
    }
    return literalString;
  }

  internal string GetHexaDecimalString(string hexEncodedText)
  {
    string hexaDecimalString = string.Empty;
    this.IsHexaDecimalString = true;
    if (!string.IsNullOrEmpty(hexEncodedText) && this.m_fontDictionary != null)
    {
      PdfName pdfName1 = this.m_fontDictionary.Items[new PdfName("Subtype")] as PdfName;
      int num = 2;
      if (pdfName1.Value != "Type1" && pdfName1.Value != "TrueType" && pdfName1.Value != "Type3")
        num = 4;
      hexEncodedText = this.EscapeSymbols(hexEncodedText);
      string s = hexEncodedText;
      string str1 = hexaDecimalString;
      string str2 = (string) null;
      while (hexEncodedText.Length > 0)
      {
        if (hexEncodedText.Length % 4 != 0)
          num = 2;
        string str3 = hexEncodedText.Substring(0, num);
        if (this.m_fontDictionary.ContainsKey("DescendantFonts") && !this.m_fontDictionary.ContainsKey("ToUnicode"))
        {
          if (this.m_fontDictionary["DescendantFonts"] is PdfArray font)
          {
            if (font[0] as PdfReferenceHolder != (PdfReferenceHolder) null)
            {
              PdfDictionary pdfDictionary1 = (font[0] as PdfReferenceHolder).Object as PdfDictionary;
              if ((pdfDictionary1["FontDescriptor"] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary2 && pdfDictionary1.ContainsKey("Subtype") && !pdfDictionary2.ContainsKey("FontFile2") && (pdfDictionary1["Subtype"] as PdfName).Value == "CIDFontType2")
                str3 = this.MapHebrewCharacters(str3);
            }
          }
          else if (this.m_fontDictionary.Items.ContainsKey(new PdfName("DescendantFonts")))
          {
            PdfReferenceHolder pdfReferenceHolder = this.m_fontDictionary.Items[new PdfName("DescendantFonts")] as PdfReferenceHolder;
            if (pdfReferenceHolder != (PdfReferenceHolder) null)
            {
              PdfArray pdfArray = pdfReferenceHolder.Object as PdfArray;
              if (pdfArray[0] as PdfReferenceHolder != (PdfReferenceHolder) null && (pdfArray[0] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary3 && pdfDictionary3.ContainsKey("CIDSystemInfo") && pdfDictionary3.ContainsKey("Subtype"))
              {
                PdfName pdfName2 = pdfDictionary3["Subtype"] as PdfName;
                if ((pdfDictionary3["CIDSystemInfo"] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Registry") && pdfDictionary.ContainsKey("Ordering") && pdfDictionary.ContainsKey("Supplement"))
                {
                  PdfString pdfString1 = pdfDictionary["Registry"] as PdfString;
                  PdfNumber pdfNumber = pdfDictionary["Supplement"] as PdfNumber;
                  PdfString pdfString2 = pdfDictionary["Ordering"] as PdfString;
                  if (pdfString1.Value != null)
                  {
                    int intValue = pdfNumber.IntValue;
                    if (pdfString2.Value != null && pdfString1.Value == "Adobe" && pdfString2.Value == "Identity" && pdfNumber.IntValue == 0 && pdfName2.Value == "CIDFontType2" && this.m_cidSystemInfoDictionary == null && !this.IsContainFontfile2)
                    {
                      this.IsAdobeIdentity = true;
                      str3 = this.MapIdentityCharacters(str3);
                    }
                  }
                }
              }
            }
          }
        }
        hexaDecimalString += (string) (object) (char) long.Parse(str3, NumberStyles.HexNumber);
        hexEncodedText = hexEncodedText.Substring(num, hexEncodedText.Length - num);
        str2 = hexaDecimalString.ToString();
      }
      if ((str2.Contains("\u0093") || str2.Contains("\u0094") || str2.Contains("\u0092")) && s.Length < num)
      {
        string str4 = str1;
        byte[] bytes = BitConverter.GetBytes(int.Parse(s, NumberStyles.HexNumber));
        hexEncodedText = Encoding.GetEncoding(1251).GetString(bytes);
        hexEncodedText = hexEncodedText.Remove(1);
        hexaDecimalString = str4 + hexEncodedText;
      }
    }
    return hexaDecimalString;
  }

  internal string MapIdentityCharacters(string hexChar)
  {
    if (hexChar.Substring(0, 2) == "00")
      hexChar = hexChar.Substring(2, 1) != "0" || hexChar.Substring(2, 1) != "1" ? (int.Parse(hexChar, NumberStyles.HexNumber) + 29).ToString("X") : (int.Parse(hexChar, NumberStyles.HexNumber) + 1335).ToString("X");
    return hexChar;
  }

  internal string GetRawString(string decodedText)
  {
    string empty = string.Empty;
    int num1 = 0;
    bool flag1 = false;
    if (this.FontEncoding == "Identity-H" && this.IsCID)
    {
      foreach (char ch1 in decodedText)
      {
        switch (ch1)
        {
          case '\u0001':
            num1 = (int) ch1;
            empty += (string) (object) ch1;
            break;
          case '\\':
            flag1 = true;
            empty += "\\";
            break;
          default:
            if (flag1)
            {
              char ch2 = ch1;
              bool flag2 = false;
              switch (ch2)
              {
                case 'b':
                  ch2 = '\b';
                  flag2 = true;
                  break;
                case 'f':
                  ch2 = '\f';
                  flag2 = true;
                  break;
                case 'n':
                  ch2 = '\n';
                  flag2 = true;
                  break;
                case 'r':
                  ch2 = '\r';
                  flag2 = true;
                  break;
                case 't':
                  ch2 = '\t';
                  flag2 = true;
                  break;
                default:
                  int num2 = num1 * 256 /*0x0100*/ + (int) ch1;
                  empty += Convert.ToString(Convert.ToChar(num2));
                  flag1 = false;
                  num1 = 0;
                  break;
              }
              if (flag2)
              {
                int num3 = (int) ch2;
                empty += Convert.ToString(Convert.ToChar(num3));
                flag1 = false;
                num1 = 0;
                break;
              }
              break;
            }
            int num4 = num1 * 256 /*0x0100*/ + (int) ch1;
            empty += Convert.ToString(Convert.ToChar(num4));
            num1 = 0;
            break;
        }
      }
      decodedText = empty;
    }
    return decodedText;
  }

  internal string MapHebrewCharacters(string hexChar)
  {
    if (hexChar.Substring(0, 2) == "02")
      hexChar = (int.Parse(hexChar, NumberStyles.HexNumber) + 816).ToString("X");
    else if (hexChar.Substring(0, 2) == "00")
      hexChar = hexChar.Substring(2, 1) == "0" || hexChar.Substring(2, 1) == "1" ? (int.Parse(hexChar, NumberStyles.HexNumber) + 29).ToString("X") : (int.Parse(hexChar, NumberStyles.HexNumber) + 1335).ToString("X");
    return hexChar;
  }

  private string GetFontName()
  {
    string fontName = string.Empty;
    this.IsSystemFontExist = false;
    List<string> stringList = new List<string>();
    if (this.m_fontDictionary != null && this.m_fontDictionary.ContainsKey("BaseFont"))
    {
      PdfName font = this.m_fontDictionary["BaseFont"] as PdfName;
      if (font == (PdfName) null)
        font = (this.m_fontDictionary["BaseFont"] as PdfReferenceHolder).Object as PdfName;
      string str = font.Value;
      if (str.Contains("#20") && !str.Contains("+"))
      {
        int length = str.LastIndexOf("#20");
        str = str.Substring(0, length) + "+";
      }
      str.Contains("+");
      if (!this.IsSystemFontExist)
      {
        if (font.Value.Contains("+"))
          fontName = font.Value.Split('+')[1];
        else
          fontName = font.Value;
        if (fontName.Contains("-"))
          fontName = fontName.Split('-')[0];
        else if (fontName.Contains(","))
          fontName = fontName.Split(',')[0];
        if (fontName.Contains("MT"))
          fontName = fontName.Replace("MT", "");
        if (fontName.Contains("#20"))
          fontName = fontName.Replace("#20", " ");
        if (fontName.Contains("#"))
          fontName = this.DecodeHexFontName(fontName);
      }
    }
    return fontName;
  }

  private string DecodeHexFontName(string fontName)
  {
    StringBuilder stringBuilder = new StringBuilder(fontName);
    for (int index = 0; index < fontName.Length; ++index)
    {
      if (fontName[index] == '#')
      {
        string s = fontName[index + 1].ToString() + fontName[index + 2].ToString();
        int num = int.Parse(s, NumberStyles.HexNumber);
        if (num != 0)
        {
          char ch = (char) num;
          stringBuilder.Replace("#" + s.ToString(), ch.ToString());
          index += 2;
        }
        if (!stringBuilder.ToString().Contains("#"))
          break;
      }
    }
    return stringBuilder.ToString();
  }

  private FontStyle GetFontStyle()
  {
    FontStyle fontStyle = FontStyle.Regular;
    if (this.m_fontDictionary.ContainsKey("BaseFont"))
    {
      PdfName font = this.m_fontDictionary["BaseFont"] as PdfName;
      if (font == (PdfName) null)
        font = (this.m_fontDictionary["BaseFont"] as PdfReferenceHolder).Object as PdfName;
      if (font.Value.Contains("-") || font.Value.Contains(","))
      {
        string empty = string.Empty;
        if (font.Value.Contains("-"))
          empty = font.Value.Split('-')[1];
        else if (font.Value.Contains(","))
          empty = font.Value.Split(',')[1];
        switch (empty.Replace("MT", ""))
        {
          case "Italic":
          case "Oblique":
            fontStyle = FontStyle.Italic;
            break;
          case "Bold":
          case "BoldMT":
            fontStyle = FontStyle.Bold;
            break;
          case "BoldItalic":
          case "BoldOblique":
            fontStyle = FontStyle.Bold | FontStyle.Italic;
            break;
        }
      }
      else
      {
        if (font.Value.Contains("Bold"))
          fontStyle = FontStyle.Bold;
        if (font.Value.Contains("BoldItalic") || font.Value.Contains("BoldOblique"))
          fontStyle = FontStyle.Bold | FontStyle.Italic;
        if (font.Value.Contains("Italic") || font.Value.Contains("Oblique"))
          fontStyle = FontStyle.Italic;
      }
    }
    return fontStyle;
  }

  private string GetFontEncoding()
  {
    PdfName pdfName = new PdfName();
    string fontEncoding = string.Empty;
    if (this.m_fontDictionary != null && this.m_fontDictionary.ContainsKey("Encoding"))
    {
      PdfName font = this.m_fontDictionary["Encoding"] as PdfName;
      if (font == (PdfName) null)
      {
        Type type = this.m_fontDictionary["Encoding"].GetType();
        pdfDictionary = new PdfDictionary();
        if (type.Name == "PdfDictionary")
        {
          if (!(this.m_fontDictionary["Encoding"] is PdfDictionary pdfDictionary))
            fontEncoding = ((this.m_fontDictionary["Encoding"] as PdfReferenceHolder).Object as PdfName).Value;
        }
        else if (type.Name == "PdfReferenceHolder")
          pdfDictionary = (this.m_fontDictionary["Encoding"] as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary != null && pdfDictionary.ContainsKey("Type"))
          fontEncoding = (pdfDictionary["Type"] as PdfName).Value;
        if (pdfDictionary != null && pdfDictionary.ContainsKey("BaseEncoding"))
          this.m_baseFontEncoding = (pdfDictionary["BaseEncoding"] as PdfName).Value;
      }
      else
        fontEncoding = font.Value;
    }
    if ("Identity#2dH".Equals(fontEncoding, StringComparison.InvariantCultureIgnoreCase))
      fontEncoding = "Identity-H";
    if (fontEncoding == "CMap")
      fontEncoding = "Identity-H";
    return fontEncoding;
  }

  private Dictionary<int, string> AdobeJapanCidMap(StreamReader reader)
  {
    string[] separator = new string[8]
    {
      "..",
      "/uni",
      ".vert",
      ".hw",
      ".dup1",
      "/Japan1.",
      ".dup2",
      ".italic"
    };
    string[] strArray1 = (string.Empty + reader.ReadToEnd()).Split('\n');
    Dictionary<int, string> dictionary = new Dictionary<int, string>();
    for (int index = 1; index < strArray1.Length; ++index)
    {
      string[] strArray2 = strArray1[index].Split(' ');
      int result;
      if (!int.TryParse(strArray2[0], out result))
      {
        if (strArray2[0].Contains(".."))
        {
          string[] strArray3 = strArray2[0].Split(separator, StringSplitOptions.None);
          int num1 = int.Parse(strArray3[0]);
          int num2 = int.Parse(strArray3[1]);
          int num3 = 0;
          for (int key = num1; key <= num2; ++key)
          {
            string str = char.ConvertFromUtf32(int.Parse(strArray2[1], NumberStyles.HexNumber) + num3);
            dictionary.Add(key, str);
            ++num3;
          }
        }
      }
      else
      {
        if (strArray2[1].Contains("\r"))
          strArray2[1] = strArray2[1].Replace("\r", "");
        if (strArray2[1].Contains("/Japan1"))
        {
          strArray2[1] = "JPN" + strArray2[1].Split(separator, StringSplitOptions.RemoveEmptyEntries)[0];
          dictionary.Add(result, strArray2[1]);
        }
        else
        {
          if (strArray2[1] == "/.notdef")
            strArray2[1] = "0000";
          else if (strArray2[1].Contains(","))
            strArray2[1] = strArray2[1].Split(',')[0];
          else if (strArray2[1].Contains("/uni"))
            strArray2[1] = strArray2[1].Split(separator, StringSplitOptions.RemoveEmptyEntries)[0];
          string str = char.ConvertFromUtf32(int.Parse(strArray2[1], NumberStyles.HexNumber));
          dictionary.Add(result, str);
        }
      }
    }
    return dictionary;
  }

  internal string AdobeJapanCidMapTableGlyphParser(string mapChar)
  {
    mapChar = this.AdobeJapanCidMapTable[(int) Convert.ToChar(mapChar)];
    if (mapChar.Contains("JPN"))
      mapChar = this.AdobeJapanCidMapReference(mapChar);
    return mapChar;
  }

  private string AdobeJapanCidMapReference(string mapValue)
  {
    int key = int.Parse(mapValue.Split(new string[1]
    {
      "JPN"
    }, StringSplitOptions.None)[1]);
    if (this.AdobeJapanCidMapTable.ContainsKey(key))
    {
      mapValue = this.AdobeJapanCidMapTable[key];
      if (mapValue.Contains("JPN"))
        mapValue = this.AdobeJapanCidMapReference(mapValue);
    }
    return mapValue;
  }

  public Font GetFont(float size)
  {
    this.isGetFontCalled = true;
    string fontName = this.FontName;
    FontStyle style = this.FontStyle != FontStyle.Regular ? this.FontStyle : this.CheckFontStyle(this.FontName);
    if (this.IsSystemFontExist)
      return (double) size >= 0.0 ? new Font(fontName, size, style) : new Font(fontName, -size, style);
    string familyName = FontStructure.CheckFontName(fontName);
    Font font = (double) size >= 0.0 ? new Font(familyName, size, style) : new Font(familyName, -size, style);
    if (this.FontEncoding == "Identity-H")
    {
      try
      {
        fontDictionary = this.m_fontDictionary;
        if (fontDictionary.ContainsKey("DescendantFonts"))
        {
          PdfArray pdfArray = (PdfArray) null;
          if (fontDictionary["DescendantFonts"] is PdfArray)
            pdfArray = fontDictionary["DescendantFonts"] as PdfArray;
          if ((object) (fontDictionary["DescendantFonts"] as PdfReferenceHolder) != null)
            pdfArray = (fontDictionary["DescendantFonts"] as PdfReferenceHolder).Object as PdfArray;
          if (!(pdfArray[0] is PdfDictionary fontDictionary))
            fontDictionary = (pdfArray[0] as PdfReferenceHolder).Object as PdfDictionary;
          if (fontDictionary["CIDSystemInfo"] is PdfDictionary)
            this.m_cidSystemInfoDictionary = fontDictionary["CIDSystemInfo"] as PdfDictionary;
          if (fontDictionary.ContainsKey("CIDToGIDMap") && (object) (fontDictionary["CIDToGIDMap"] as PdfReferenceHolder) != null)
          {
            MemoryStream memoryStream = new MemoryStream();
            PdfStream pdfStream = (fontDictionary["CIDToGIDMap"] as PdfReferenceHolder).Object as PdfStream;
            PdfDictionary streamDictionary = (fontDictionary["CIDToGIDMap"] as PdfReferenceHolder).Object as PdfDictionary;
            MemoryStream encodedStream = pdfStream.InternalStream;
            if (streamDictionary.ContainsKey("Filter"))
            {
              string[] fontFilter = this.GetFontFilter(streamDictionary);
              if (fontFilter != null)
              {
                for (int index = 0; index < fontFilter.Length; ++index)
                {
                  switch (fontFilter[index])
                  {
                    case "A85":
                    case "ASCII85Decode":
                      encodedStream = this.DecodeASCII85Stream(encodedStream);
                      break;
                    case "FlateDecode":
                      encodedStream = this.DecodeFlateStream(encodedStream);
                      break;
                  }
                }
              }
            }
            encodedStream.Position = 0L;
            this.m_cidToGidTable = this.GetCidToGidTable(encodedStream.GetBuffer());
          }
          if ((object) (fontDictionary["FontDescriptor"] as PdfReferenceHolder) != null)
            fontDictionary = (fontDictionary["FontDescriptor"] as PdfReferenceHolder).Object as PdfDictionary;
          else if (fontDictionary["FontDescriptor"] is PdfDictionary)
            fontDictionary = fontDictionary["FontDescriptor"] as PdfDictionary;
          if (this.m_cidSystemInfoDictionary != null && this.m_cidSystemInfoDictionary.ContainsKey("Registry") && this.m_cidSystemInfoDictionary.ContainsKey("Ordering") && this.m_cidSystemInfoDictionary.ContainsKey("Supplement") && this.m_cidSystemInfoDictionary["Registry"] is PdfString cidSystemInfo2)
          {
            string str1 = cidSystemInfo2.Value;
            if (this.m_cidSystemInfoDictionary["Ordering"] is PdfString cidSystemInfo1)
            {
              string str2 = cidSystemInfo1.Value;
              if (this.m_cidSystemInfoDictionary["Supplement"] is PdfNumber cidSystemInfo)
              {
                int intValue = cidSystemInfo.IntValue;
                if (str1 == "Adobe" && str2 == "Japan1" && intValue == 6)
                {
                  this.IsAdobeJapanFont = true;
                  using (MemoryStream memoryStream = new MemoryStream(Resources.Adobe_Japan1_6))
                  {
                    using (StreamReader reader = new StreamReader((Stream) memoryStream))
                      this.m_adobeJapanCidMapTable = this.AdobeJapanCidMap(reader);
                  }
                }
              }
            }
          }
        }
        else if (fontDictionary.ContainsKey("FontDescriptor"))
        {
          PdfReferenceHolder pdfReferenceHolder = fontDictionary["FontDescriptor"] as PdfReferenceHolder;
          if (pdfReferenceHolder != (PdfReferenceHolder) null)
            fontDictionary = pdfReferenceHolder.Object as PdfDictionary;
        }
        if (fontDictionary.ContainsKey("FontFile"))
        {
          this.m_isContainFontfile = true;
          this.IsType1Font = true;
          this.isEmbedded = true;
          long objNum = (fontDictionary["FontFile"] as PdfReferenceHolder).Reference.ObjNum;
          if (!this.type1FontReference.ContainsKey(objNum))
          {
            PdfDictionary streamDictionary = (fontDictionary["FontFile"] as PdfReferenceHolder).Object as PdfDictionary;
            MemoryStream encodedStream = (streamDictionary as PdfStream).InternalStream;
            string[] fontFilter = this.GetFontFilter(streamDictionary);
            if (fontFilter != null)
            {
              for (int index = 0; index < fontFilter.Length; ++index)
              {
                switch (fontFilter[index])
                {
                  case "A85":
                  case "ASCII85Decode":
                    encodedStream = this.DecodeASCII85Stream(encodedStream);
                    break;
                  case "FlateDecode":
                    encodedStream = this.DecodeFlateStream(encodedStream);
                    break;
                }
              }
            }
            encodedStream.Capacity = (int) encodedStream.Length;
            byte[] array = encodedStream.ToArray();
            this.FontFileType1Font = new FontFile();
            this.FontStream = encodedStream;
            if (streamDictionary.ContainsKey("Subtype"))
              this.m_subType = (streamDictionary["Subtype"] as PdfName).Value;
            else if (this.m_fontDictionary.ContainsKey("Subtype"))
              this.m_subType = (this.m_fontDictionary["Subtype"] as PdfName).Value;
            if (this.m_subType != "OpenType")
            {
              this.IsOpenTypeFont = false;
              this.m_cffGlyphs = this.FontFileType1Font.ParseType1FontFile(array);
              this.type1FontReference.Add(objNum, this.m_cffGlyphs);
            }
            else
            {
              this.IsOpenTypeFont = true;
              return (Font) null;
            }
          }
          else
            this.m_cffGlyphs = this.type1FontReference[objNum];
        }
        else
        {
          if (fontDictionary.ContainsKey("FontFile2"))
          {
            this.IsContainFontfile2 = true;
            this.isEmbedded = true;
            if (!this.m_isTextExtraction)
            {
              PdfDictionary streamDictionary = (fontDictionary["FontFile2"] as PdfReferenceHolder).Object as PdfDictionary;
              MemoryStream encodedStream = (streamDictionary as PdfStream).InternalStream;
              string[] fontFilter = this.GetFontFilter(streamDictionary);
              if (fontFilter != null)
              {
                for (int index1 = 0; index1 < fontFilter.Length; ++index1)
                {
                  switch (fontFilter[index1])
                  {
                    case "A85":
                    case "ASCII85Decode":
                      encodedStream = this.DecodeASCII85Stream(encodedStream);
                      break;
                    case "FlateDecode":
                      encodedStream = this.DecodeFlateStream(encodedStream);
                      break;
                    case "RunLengthDecode":
                      encodedStream.Position = 0L;
                      byte[] array = encodedStream.ToArray();
                      int index2 = 0;
                      byte[] numArray1 = new byte[0];
                      byte[] numArray2 = new byte[0];
                      while (index2 < array.Length - 1)
                      {
                        byte[] sourceArray1 = numArray1;
                        int num1 = (int) array[index2];
                        if (num1 >= 0 && num1 <= (int) sbyte.MaxValue)
                        {
                          int sourceIndex = index2 + 1;
                          byte[] numArray3 = new byte[num1 + 1];
                          System.Array.Copy((System.Array) array, sourceIndex, (System.Array) numArray3, 0, numArray3.Length);
                          numArray1 = new byte[sourceArray1.Length + numArray3.Length];
                          System.Array.Copy((System.Array) sourceArray1, 0, (System.Array) numArray1, 0, sourceArray1.Length);
                          System.Array.Copy((System.Array) numArray3, 0, (System.Array) numArray1, sourceArray1.Length, numArray3.Length);
                          index2 = sourceIndex + (num1 + 1);
                        }
                        else if (num1 >= 129 && num1 <= (int) byte.MaxValue)
                        {
                          int index3 = index2 + 1;
                          byte num2 = array[index3];
                          int length = 257 - num1;
                          byte[] sourceArray2 = new byte[length];
                          for (int index4 = 0; index4 < length; ++index4)
                            sourceArray2[index4] = num2;
                          numArray1 = new byte[sourceArray1.Length + sourceArray2.Length];
                          System.Array.Copy((System.Array) sourceArray1, 0, (System.Array) numArray1, 0, sourceArray1.Length);
                          System.Array.Copy((System.Array) sourceArray2, 0, (System.Array) numArray1, sourceArray1.Length, sourceArray2.Length);
                          index2 = index3 + 1;
                        }
                        else if (num1 == 128 /*0x80*/)
                          break;
                      }
                      encodedStream = new MemoryStream(numArray1);
                      encodedStream.Position = 0L;
                      break;
                  }
                }
              }
              encodedStream.Capacity = (int) encodedStream.Length;
              if (!this.m_isTextExtraction)
                this.m_fontfile2 = new FontFile2(encodedStream.ToArray());
              List<TableEntry> tableEntryList = new List<TableEntry>();
              FontDecode fontDecode = new FontDecode();
              this.GetGlyphWidths();
            }
            return font;
          }
          if (fontDictionary.ContainsKey("FontFile3"))
          {
            this.IsType1Font = true;
            this.Is1C = true;
            this.isEmbedded = true;
            long objNum = (fontDictionary["FontFile3"] as PdfReferenceHolder).Reference.ObjNum;
            if (!this.type1FontReference.ContainsKey(objNum))
            {
              PdfDictionary streamDictionary = (fontDictionary["FontFile3"] as PdfReferenceHolder).Object as PdfDictionary;
              MemoryStream encodedStream = (streamDictionary as PdfStream).InternalStream;
              string[] fontFilter = this.GetFontFilter(streamDictionary);
              if (fontFilter != null)
              {
                for (int index = 0; index < fontFilter.Length; ++index)
                {
                  switch (fontFilter[index])
                  {
                    case "A85":
                    case "ASCII85Decode":
                      encodedStream = this.DecodeASCII85Stream(encodedStream);
                      break;
                    case "FlateDecode":
                      encodedStream = this.DecodeFlateStream(encodedStream);
                      break;
                  }
                }
              }
              encodedStream.Capacity = (int) encodedStream.Length;
              byte[] array = encodedStream.ToArray();
              this.fontFile3Type1Font = new FontFile3();
              this.IsContainFontfile3 = true;
              this.FontStream = encodedStream;
              if (streamDictionary.ContainsKey("Subtype"))
                this.m_subType = (streamDictionary["Subtype"] as PdfName).Value;
              else if (this.m_fontDictionary.ContainsKey("Subtype"))
                this.m_subType = (this.m_fontDictionary["Subtype"] as PdfName).Value;
              if (this.m_subType != "OpenType")
              {
                this.IsOpenTypeFont = false;
                this.m_cffGlyphs = this.fontFile3Type1Font.readType1CFontFile(array);
                this.type1FontReference.Add(objNum, this.m_cffGlyphs);
                this.IsCID = this.fontFile3Type1Font.isCID;
              }
              else
              {
                this.IsOpenTypeFont = true;
                return (Font) null;
              }
            }
            else
              this.m_cffGlyphs = this.type1FontReference[objNum];
          }
        }
      }
      catch (Exception ex)
      {
        return (Font) null;
      }
    }
    else
    {
      if (!(this.FontEncoding == "WinAnsiEncoding"))
      {
        if (!(this.FontEncoding == ""))
        {
          if (!(this.FontEncoding == "BuiltIn"))
          {
            if (!(this.FontEncoding == "MacRomanEncoding"))
            {
              if (!(this.FontEncoding == "Encoding"))
                return font;
              try
              {
                PdfDictionary fontDictionary = this.m_fontDictionary;
                if (fontDictionary.ContainsKey("Encoding") && (object) (fontDictionary["Encoding"] as PdfReferenceHolder) != null)
                {
                  PdfDictionary pdfDictionary = (fontDictionary["Encoding"] as PdfReferenceHolder).Object as PdfDictionary;
                  if (pdfDictionary.ContainsKey("Differences"))
                  {
                    if (!(pdfDictionary["Differences"] is PdfArray pdfArray))
                      pdfArray = (pdfDictionary["Differences"] as PdfReferenceHolder).Object as PdfArray;
                    int key = 0;
                    for (int index = 0; index < pdfArray.Count; ++index)
                    {
                      IPdfPrimitive pdfPrimitive = pdfArray[index];
                      if (pdfPrimitive is PdfNumber)
                      {
                        key = (pdfPrimitive as PdfNumber).IntValue;
                      }
                      else
                      {
                        string str = (pdfPrimitive as PdfName).Value;
                        if (!this.differenceTable.ContainsKey(key))
                          this.differenceTable.Add(key, str);
                        ++key;
                      }
                    }
                  }
                }
                if (fontDictionary.ContainsKey("DescendantFonts"))
                {
                  PdfArray pdfArray = (PdfArray) null;
                  if (fontDictionary["DescendantFonts"] is PdfArray)
                    pdfArray = fontDictionary["DescendantFonts"] as PdfArray;
                  if ((object) (fontDictionary["DescendantFonts"] as PdfReferenceHolder) != null)
                    pdfArray = (fontDictionary["DescendantFonts"] as PdfReferenceHolder).Object as PdfArray;
                  fontDictionary = (pdfArray[0] as PdfReferenceHolder).Object as PdfDictionary;
                  if ((object) (fontDictionary["FontDescriptor"] as PdfReferenceHolder) != null)
                    fontDictionary = (fontDictionary["FontDescriptor"] as PdfReferenceHolder).Object as PdfDictionary;
                  else if (fontDictionary["FontDescriptor"] is PdfDictionary)
                    fontDictionary = fontDictionary["FontDescriptor"] as PdfDictionary;
                }
                else if (fontDictionary.ContainsKey("FontDescriptor") && !this.isMpdfaaFonts())
                  fontDictionary = (fontDictionary["FontDescriptor"] as PdfReferenceHolder).Object as PdfDictionary;
                if (fontDictionary.ContainsKey("FontFile"))
                {
                  this.m_isContainFontfile = true;
                  this.IsType1Font = true;
                  this.isEmbedded = true;
                  long objNum = (fontDictionary["FontFile"] as PdfReferenceHolder).Reference.ObjNum;
                  if (!this.type1FontReference.ContainsKey(objNum))
                  {
                    PdfDictionary streamDictionary = (fontDictionary["FontFile"] as PdfReferenceHolder).Object as PdfDictionary;
                    MemoryStream encodedStream = (streamDictionary as PdfStream).InternalStream;
                    string[] fontFilter = this.GetFontFilter(streamDictionary);
                    if (fontFilter != null)
                    {
                      for (int index = 0; index < fontFilter.Length; ++index)
                      {
                        switch (fontFilter[index])
                        {
                          case "A85":
                          case "ASCII85Decode":
                            encodedStream = this.DecodeASCII85Stream(encodedStream);
                            break;
                          case "FlateDecode":
                            encodedStream = this.DecodeFlateStream(encodedStream);
                            break;
                        }
                      }
                    }
                    encodedStream.Capacity = (int) encodedStream.Length;
                    byte[] array = encodedStream.ToArray();
                    this.FontFileType1Font = new FontFile();
                    this.FontStream = encodedStream;
                    if (streamDictionary.ContainsKey("Subtype"))
                      this.m_subType = (streamDictionary["Subtype"] as PdfName).Value;
                    else if (this.m_fontDictionary.ContainsKey("Subtype"))
                      this.m_subType = (this.m_fontDictionary["Subtype"] as PdfName).Value;
                    if (this.m_subType != "OpenType")
                    {
                      this.IsOpenTypeFont = false;
                      this.m_cffGlyphs = this.FontFileType1Font.ParseType1FontFile(array);
                      this.type1FontReference.Add(objNum, this.m_cffGlyphs);
                      goto label_247;
                    }
                    this.IsOpenTypeFont = true;
                    return (Font) null;
                  }
                  this.m_cffGlyphs = this.type1FontReference[objNum];
                  goto label_247;
                }
                if (fontDictionary.ContainsKey("FontFile2"))
                {
                  this.IsContainFontfile2 = true;
                  this.isEmbedded = true;
                  PdfDictionary streamDictionary = (fontDictionary["FontFile2"] as PdfReferenceHolder).Object as PdfDictionary;
                  MemoryStream encodedStream = (streamDictionary as PdfStream).InternalStream;
                  string[] fontFilter = this.GetFontFilter(streamDictionary);
                  if (fontFilter != null)
                  {
                    for (int index = 0; index < fontFilter.Length; ++index)
                    {
                      switch (fontFilter[index])
                      {
                        case "A85":
                        case "ASCII85Decode":
                          encodedStream = this.DecodeASCII85Stream(encodedStream);
                          break;
                        case "FlateDecode":
                          encodedStream = this.DecodeFlateStream(encodedStream);
                          break;
                      }
                    }
                  }
                  encodedStream.Capacity = (int) encodedStream.Length;
                  byte[] buffer = encodedStream.GetBuffer();
                  if (!this.m_isTextExtraction)
                    this.m_fontfile2 = new FontFile2(buffer);
                  List<TableEntry> tableEntryList = new List<TableEntry>();
                  FontDecode fontDecode = new FontDecode();
                  this.GetGlyphWidths();
                  return font;
                }
                if (fontDictionary.ContainsKey("FontFile3"))
                {
                  this.IsType1Font = true;
                  this.Is1C = true;
                  this.isEmbedded = true;
                  long objNum = (fontDictionary["FontFile3"] as PdfReferenceHolder).Reference.ObjNum;
                  if (!this.type1FontReference.ContainsKey(objNum))
                  {
                    PdfDictionary streamDictionary = (fontDictionary["FontFile3"] as PdfReferenceHolder).Object as PdfDictionary;
                    MemoryStream encodedStream = (streamDictionary as PdfStream).InternalStream;
                    string[] fontFilter = this.GetFontFilter(streamDictionary);
                    if (fontFilter != null)
                    {
                      for (int index = 0; index < fontFilter.Length; ++index)
                      {
                        switch (fontFilter[index])
                        {
                          case "A85":
                          case "ASCII85Decode":
                            encodedStream = this.DecodeASCII85Stream(encodedStream);
                            break;
                          case "FlateDecode":
                            encodedStream = this.DecodeFlateStream(encodedStream);
                            break;
                        }
                      }
                    }
                    encodedStream.Capacity = (int) encodedStream.Length;
                    byte[] array = encodedStream.ToArray();
                    this.fontFile3Type1Font = new FontFile3();
                    this.IsContainFontfile3 = true;
                    this.FontStream = encodedStream;
                    if (streamDictionary.ContainsKey("Subtype"))
                      this.m_subType = (streamDictionary["Subtype"] as PdfName).Value;
                    else if (this.m_fontDictionary.ContainsKey("Subtype"))
                      this.m_subType = (this.m_fontDictionary["Subtype"] as PdfName).Value;
                    if (this.m_subType != "OpenType")
                    {
                      this.IsOpenTypeFont = false;
                      this.m_cffGlyphs = this.fontFile3Type1Font.readType1CFontFile(array);
                      this.type1FontReference.Add(objNum, this.m_cffGlyphs);
                      this.IsCID = this.fontFile3Type1Font.isCID;
                      goto label_247;
                    }
                    this.IsOpenTypeFont = true;
                    return (Font) null;
                  }
                  this.m_cffGlyphs = this.type1FontReference[objNum];
                  goto label_247;
                }
                goto label_247;
              }
              catch (Exception ex)
              {
                return (Font) null;
              }
            }
          }
        }
      }
      try
      {
        PdfDictionary fontDictionary = this.m_fontDictionary;
        if (fontDictionary.ContainsKey("DescendantFonts"))
        {
          PdfArray pdfArray = (PdfArray) null;
          if (fontDictionary["DescendantFonts"] is PdfArray)
            pdfArray = fontDictionary["DescendantFonts"] as PdfArray;
          if ((object) (fontDictionary["DescendantFonts"] as PdfReferenceHolder) != null)
            pdfArray = (fontDictionary["DescendantFonts"] as PdfReferenceHolder).Object as PdfArray;
          fontDictionary = (pdfArray[0] as PdfReferenceHolder).Object as PdfDictionary;
          if ((object) (fontDictionary["FontDescriptor"] as PdfReferenceHolder) != null)
            fontDictionary = (fontDictionary["FontDescriptor"] as PdfReferenceHolder).Object as PdfDictionary;
          else if (fontDictionary["FontDescriptor"] is PdfDictionary)
            fontDictionary = fontDictionary["FontDescriptor"] as PdfDictionary;
        }
        else if (fontDictionary.ContainsKey("FontDescriptor") && fontDictionary["FontDescriptor"] as PdfReferenceHolder != (PdfReferenceHolder) null)
          fontDictionary = (fontDictionary["FontDescriptor"] as PdfReferenceHolder).Object as PdfDictionary;
        if (fontDictionary != null && fontDictionary.ContainsKey("FontFile"))
        {
          this.m_isContainFontfile = true;
          this.isEmbedded = true;
          this.IsType1Font = true;
          long objNum = (fontDictionary["FontFile"] as PdfReferenceHolder).Reference.ObjNum;
          if (!this.type1FontReference.ContainsKey(objNum))
          {
            PdfDictionary streamDictionary = (fontDictionary["FontFile"] as PdfReferenceHolder).Object as PdfDictionary;
            MemoryStream encodedStream = (streamDictionary as PdfStream).InternalStream;
            string[] fontFilter = this.GetFontFilter(streamDictionary);
            if (fontFilter != null)
            {
              for (int index = 0; index < fontFilter.Length; ++index)
              {
                switch (fontFilter[index])
                {
                  case "A85":
                  case "ASCII85Decode":
                    encodedStream = this.DecodeASCII85Stream(encodedStream);
                    break;
                  case "FlateDecode":
                    encodedStream = this.DecodeFlateStream(encodedStream);
                    break;
                }
              }
            }
            encodedStream.Capacity = (int) encodedStream.Length;
            byte[] array = encodedStream.ToArray();
            this.FontFileType1Font = new FontFile();
            this.FontStream = encodedStream;
            if (streamDictionary.ContainsKey("Subtype"))
              this.m_subType = (streamDictionary["Subtype"] as PdfName).Value;
            else if (this.m_fontDictionary.ContainsKey("Subtype"))
              this.m_subType = (this.m_fontDictionary["Subtype"] as PdfName).Value;
            if (this.m_subType != "OpenType")
            {
              this.IsOpenTypeFont = false;
              this.m_cffGlyphs = this.FontFileType1Font.ParseType1FontFile(array);
              this.type1FontReference.Add(objNum, this.m_cffGlyphs);
            }
            else
            {
              this.IsOpenTypeFont = true;
              return (Font) null;
            }
          }
          else
            this.m_cffGlyphs = this.type1FontReference[objNum];
        }
        else
        {
          if (fontDictionary != null && fontDictionary.ContainsKey("FontFile2"))
          {
            this.IsContainFontfile2 = true;
            this.isEmbedded = true;
            PdfDictionary streamDictionary = (fontDictionary["FontFile2"] as PdfReferenceHolder).Object as PdfDictionary;
            MemoryStream encodedStream = (streamDictionary as PdfStream).InternalStream;
            string[] fontFilter = this.GetFontFilter(streamDictionary);
            if (fontFilter != null)
            {
              for (int index5 = 0; index5 < fontFilter.Length; ++index5)
              {
                switch (fontFilter[index5])
                {
                  case "A85":
                  case "ASCII85Decode":
                    encodedStream = this.DecodeASCII85Stream(encodedStream);
                    break;
                  case "FlateDecode":
                    encodedStream = this.DecodeFlateStream(encodedStream);
                    break;
                  case "RunLengthDecode":
                    encodedStream.Position = 0L;
                    byte[] array = encodedStream.ToArray();
                    int index6 = 0;
                    byte[] numArray4 = new byte[0];
                    byte[] numArray5 = new byte[0];
                    while (index6 < array.Length - 1)
                    {
                      byte[] sourceArray3 = numArray4;
                      int num3 = (int) array[index6];
                      if (num3 >= 0 && num3 <= (int) sbyte.MaxValue)
                      {
                        int sourceIndex = index6 + 1;
                        byte[] numArray6 = new byte[num3 + 1];
                        System.Array.Copy((System.Array) array, sourceIndex, (System.Array) numArray6, 0, numArray6.Length);
                        numArray4 = new byte[sourceArray3.Length + numArray6.Length];
                        System.Array.Copy((System.Array) sourceArray3, 0, (System.Array) numArray4, 0, sourceArray3.Length);
                        System.Array.Copy((System.Array) numArray6, 0, (System.Array) numArray4, sourceArray3.Length, numArray6.Length);
                        index6 = sourceIndex + (num3 + 1);
                      }
                      else if (num3 >= 129 && num3 <= (int) byte.MaxValue)
                      {
                        int index7 = index6 + 1;
                        byte num4 = array[index7];
                        int length = 257 - num3;
                        byte[] sourceArray4 = new byte[length];
                        for (int index8 = 0; index8 < length; ++index8)
                          sourceArray4[index8] = num4;
                        numArray4 = new byte[sourceArray3.Length + sourceArray4.Length];
                        System.Array.Copy((System.Array) sourceArray3, 0, (System.Array) numArray4, 0, sourceArray3.Length);
                        System.Array.Copy((System.Array) sourceArray4, 0, (System.Array) numArray4, sourceArray3.Length, sourceArray4.Length);
                        index6 = index7 + 1;
                      }
                      else if (num3 == 128 /*0x80*/)
                        break;
                    }
                    encodedStream = new MemoryStream(numArray4);
                    encodedStream.Position = 0L;
                    break;
                }
              }
            }
            encodedStream.Capacity = (int) encodedStream.Length;
            if (!this.m_isTextExtraction)
              this.m_fontfile2 = new FontFile2(encodedStream.ToArray());
            this.GetGlyphWidths();
            encodedStream.Dispose();
            return font;
          }
          if (fontDictionary != null && fontDictionary.ContainsKey("FontFile3"))
          {
            this.IsType1Font = true;
            this.isEmbedded = true;
            this.Is1C = true;
            long objNum = (fontDictionary["FontFile3"] as PdfReferenceHolder).Reference.ObjNum;
            if (!this.type1FontReference.ContainsKey(objNum))
            {
              PdfDictionary streamDictionary = (fontDictionary["FontFile3"] as PdfReferenceHolder).Object as PdfDictionary;
              MemoryStream encodedStream = (streamDictionary as PdfStream).InternalStream;
              string[] fontFilter = this.GetFontFilter(streamDictionary);
              if (fontFilter != null)
              {
                for (int index = 0; index < fontFilter.Length; ++index)
                {
                  switch (fontFilter[index])
                  {
                    case "A85":
                    case "ASCII85Decode":
                      encodedStream = this.DecodeASCII85Stream(encodedStream);
                      break;
                    case "FlateDecode":
                      encodedStream = this.DecodeFlateStream(encodedStream);
                      break;
                  }
                }
              }
              encodedStream.Capacity = (int) encodedStream.Length;
              byte[] array = encodedStream.ToArray();
              this.fontFile3Type1Font = new FontFile3();
              this.IsContainFontfile3 = true;
              this.FontStream = encodedStream;
              if (streamDictionary.ContainsKey("Subtype"))
                this.m_subType = (streamDictionary["Subtype"] as PdfName).Value;
              else if (this.m_fontDictionary.ContainsKey("Subtype"))
                this.m_subType = (this.m_fontDictionary["Subtype"] as PdfName).Value;
              if (this.m_subType != "OpenType")
              {
                this.IsOpenTypeFont = false;
                this.m_cffGlyphs = this.fontFile3Type1Font.readType1CFontFile(array);
                this.type1FontReference.Add(objNum, this.m_cffGlyphs);
                this.IsCID = this.fontFile3Type1Font.isCID;
              }
              else
              {
                this.IsOpenTypeFont = true;
                return (Font) null;
              }
            }
            else
              this.m_cffGlyphs = this.type1FontReference[objNum];
          }
          else
          {
            if (fontDictionary != null && fontDictionary.ContainsKey("FontFamily"))
              return new Font((fontDictionary["FontFamily"] as PdfString).Value, this.FontSize, this.FontStyle);
            this.IsSystemFontExist = true;
          }
        }
      }
      catch (Exception ex)
      {
        return (Font) null;
      }
    }
label_247:
    return (Font) null;
  }

  private void GetGlyphWidthsType1()
  {
    int num = 0;
    PdfDictionary fontDictionary = this.m_fontDictionary;
    if (fontDictionary.ContainsKey("DW"))
      this.DefaultGlyphWidth = (fontDictionary["DW"] as PdfNumber).FloatValue;
    if (fontDictionary.ContainsKey("FirstChar"))
      num = (fontDictionary["FirstChar"] as PdfNumber).IntValue;
    if (fontDictionary.ContainsKey("LastChar"))
    {
      int intValue = (fontDictionary["LastChar"] as PdfNumber).IntValue;
    }
    this.m_fontGlyphWidth = new Dictionary<int, int>();
    this.m_fontGlyphWidthMapping = new Dictionary<int, int>();
    PdfArray pdfArray = (PdfArray) null;
    if (fontDictionary["Widths"] is PdfArray)
      pdfArray = fontDictionary["Widths"] as PdfArray;
    if ((object) (fontDictionary["Widths"] as PdfReferenceHolder) != null)
      pdfArray = (fontDictionary["Widths"] as PdfReferenceHolder).Object as PdfArray;
    if (pdfArray == null)
      return;
    try
    {
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        int key1 = num + index;
        if (this.CharacterMapTable.ContainsKey((double) key1))
        {
          int key2 = (int) this.CharacterMapTable[(double) key1].ToCharArray()[0];
          if (!this.m_fontGlyphWidthMapping.ContainsKey(key2))
            this.m_fontGlyphWidthMapping.Add(key2, (pdfArray[index] as PdfNumber).IntValue);
        }
        else if (this.DifferencesDictionary.ContainsKey(key1.ToString()))
        {
          string differences = this.DifferencesDictionary[key1.ToString()];
          int key3 = key1;
          if (differences.Length == 1)
            key3 = (int) differences.ToCharArray()[0];
          if (!this.m_fontGlyphWidthMapping.ContainsKey(key3))
            this.m_fontGlyphWidthMapping.Add(key3, (pdfArray[index] as PdfNumber).IntValue);
        }
        else if (!this.m_fontGlyphWidthMapping.ContainsKey(key1))
          this.m_fontGlyphWidthMapping.Add(key1, (pdfArray[index] as PdfNumber).IntValue);
        this.m_fontGlyphWidth.Add(key1, (pdfArray[index] as PdfNumber).IntValue);
      }
    }
    catch
    {
      this.m_fontGlyphWidth = (Dictionary<int, int>) null;
    }
  }

  private void GetGlyphWidthsNonIdH()
  {
    int num = 0;
    PdfDictionary fontDictionary = this.m_fontDictionary;
    if (fontDictionary.ContainsKey("DW"))
      this.DefaultGlyphWidth = (fontDictionary["DW"] as PdfNumber).FloatValue;
    if (fontDictionary.ContainsKey("FirstChar"))
      num = (fontDictionary["FirstChar"] as PdfNumber).IntValue;
    if (fontDictionary.ContainsKey("LastChar"))
    {
      int intValue1 = (fontDictionary["LastChar"] as PdfNumber).IntValue;
    }
    this.m_fontGlyphWidth = new Dictionary<int, int>();
    PdfArray pdfArray1 = (PdfArray) null;
    if (fontDictionary["Widths"] is PdfArray)
      pdfArray1 = fontDictionary["Widths"] as PdfArray;
    if ((object) (fontDictionary["Widths"] as PdfReferenceHolder) != null)
      pdfArray1 = (fontDictionary["Widths"] as PdfReferenceHolder).Object as PdfArray;
    if (fontDictionary.Items.ContainsKey(new PdfName("DescendantFonts")) && fontDictionary["DescendantFonts"] is PdfArray pdfArray2 && (object) (pdfArray2[0] as PdfReferenceHolder) != null)
    {
      PdfDictionary pdfDictionary = (pdfArray2[0] as PdfReferenceHolder).Object as PdfDictionary;
      if (pdfDictionary.ContainsKey("W"))
        pdfArray1 = pdfDictionary["W"] as PdfArray;
    }
    if (pdfArray1 == null)
      return;
    try
    {
      for (int index1 = 0; index1 < pdfArray1.Count; ++index1)
      {
        int key1 = num + index1;
        if (this.CharacterMapTable.Count > 0 || this.DifferencesDictionary.Count > 0)
        {
          if (this.CharacterMapTable.ContainsKey((double) key1))
          {
            string str = this.CharacterMapTable[(double) key1];
            int key2 = key1;
            if (!this.m_fontGlyphWidth.ContainsKey(key2))
              this.m_fontGlyphWidth.Add(key2, (pdfArray1[index1] as PdfNumber).IntValue);
          }
          else if (this.DifferencesDictionary.ContainsKey(key1.ToString()))
          {
            string differences = this.DifferencesDictionary[key1.ToString()];
            int key3 = key1;
            if (!this.m_fontGlyphWidth.ContainsKey(key3))
              this.m_fontGlyphWidth.Add(key3, (pdfArray1[index1] as PdfNumber).IntValue);
          }
          else if (!this.m_fontGlyphWidth.ContainsKey(key1))
            this.m_fontGlyphWidth.Add(key1, (pdfArray1[index1] as PdfNumber).IntValue);
        }
        else if (pdfArray1[index1] is PdfArray)
        {
          PdfArray pdfArray3 = pdfArray1[index1] as PdfArray;
          for (int index2 = index1; index2 < pdfArray3.Count; ++index2)
          {
            int key4 = num + index2;
            if (this.CharacterMapTable.Count > 0 || this.DifferencesDictionary.Count > 0)
            {
              if (this.CharacterMapTable.ContainsKey((double) key4))
              {
                int key5 = (int) this.CharacterMapTable[(double) key4].ToCharArray()[0];
                if (!this.m_fontGlyphWidth.ContainsKey(key5))
                  this.m_fontGlyphWidth.Add(key5, (pdfArray3[index2] as PdfNumber).IntValue);
              }
              else if (this.DifferencesDictionary.ContainsKey(key4.ToString()))
              {
                string differences = this.DifferencesDictionary[key4.ToString()];
                int key6 = key4;
                if (!this.m_fontGlyphWidth.ContainsKey(key6))
                  this.m_fontGlyphWidth.Add(key6, (pdfArray3[index2] as PdfNumber).IntValue);
              }
              else if (!this.m_fontGlyphWidth.ContainsKey(key4))
                this.m_fontGlyphWidth.Add(key4, (pdfArray3[index2] as PdfNumber).IntValue);
            }
            else
              this.m_fontGlyphWidth.Add(key4, (pdfArray3[index2] as PdfNumber).IntValue);
          }
        }
        else
        {
          int intValue2 = (pdfArray1[index1] as PdfNumber).IntValue;
          if (!this.m_fontGlyphWidth.ContainsKey(key1))
            this.m_fontGlyphWidth.Add(key1, intValue2);
        }
      }
    }
    catch
    {
      this.m_fontGlyphWidth = (Dictionary<int, int>) null;
    }
  }

  private bool isMpdfaaFonts()
  {
    bool flag = false;
    if (this.m_fontDictionary.ContainsKey("BaseFont"))
    {
      PdfName font = this.m_fontDictionary["BaseFont"] as PdfName;
      if (font != (PdfName) null)
      {
        string str;
        if (font.Value.Contains("+"))
          str = font.Value.Split('+')[0];
        else
          str = font.Value;
        if (str == "MPDFAA")
          flag = true;
      }
    }
    return flag;
  }

  private void GetGlyphWidths()
  {
    if (this.FontEncoding != "Identity-H")
      return;
    fontDictionary = this.m_fontDictionary;
    if (fontDictionary.ContainsKey("DescendantFonts"))
    {
      PdfArray pdfArray = (PdfArray) null;
      if (fontDictionary["DescendantFonts"] is PdfArray)
        pdfArray = fontDictionary["DescendantFonts"] as PdfArray;
      if ((object) (fontDictionary["DescendantFonts"] as PdfReferenceHolder) != null)
        pdfArray = (fontDictionary["DescendantFonts"] as PdfReferenceHolder).Object as PdfArray;
      if (!(pdfArray[0] is PdfDictionary fontDictionary))
        fontDictionary = (pdfArray[0] as PdfReferenceHolder).Object as PdfDictionary;
    }
    this.m_fontGlyphWidth = new Dictionary<int, int>();
    PdfArray pdfArray1 = (PdfArray) null;
    int key = 0;
    PdfArray pdfArray2 = (PdfArray) null;
    if (fontDictionary["W"] is PdfArray)
      pdfArray1 = fontDictionary["W"] as PdfArray;
    if ((object) (fontDictionary["W"] as PdfReferenceHolder) != null)
      pdfArray1 = (fontDictionary["W"] as PdfReferenceHolder).Object as PdfArray;
    if (fontDictionary.ContainsKey("DW"))
      this.DefaultGlyphWidth = (fontDictionary["DW"] as PdfNumber).FloatValue;
    try
    {
      if (pdfArray1 == null)
      {
        this.m_fontGlyphWidth = (Dictionary<int, int>) null;
        return;
      }
      int index1;
      for (int index2 = 0; index2 < pdfArray1.Count; index2 = index1 + 1)
      {
        if (pdfArray1[index2] is PdfNumber)
          key = (pdfArray1[index2] as PdfNumber).IntValue;
        index1 = index2 + 1;
        if (pdfArray1[index1] is PdfArray)
        {
          PdfArray pdfArray3 = pdfArray1[index1] as PdfArray;
          for (int index3 = 0; index3 < pdfArray3.Count; ++index3)
          {
            if (!this.m_containsCmap)
              this.m_fontGlyphWidth.Add(key, (pdfArray3[index3] as PdfNumber).IntValue);
            else if (!this.m_fontGlyphWidth.ContainsKey(key))
              this.m_fontGlyphWidth.Add(key, (pdfArray3[index3] as PdfNumber).IntValue);
            ++key;
          }
        }
        else if (pdfArray1[index1] is PdfNumber)
        {
          int intValue = (pdfArray1[index1] as PdfNumber).IntValue;
          ++index1;
          for (; key <= intValue; ++key)
          {
            if (!this.m_fontGlyphWidth.ContainsKey(key))
              this.m_fontGlyphWidth.Add(key, (pdfArray1[index1] as PdfNumber).IntValue);
          }
        }
        else if ((object) (pdfArray1[index1] as PdfReferenceHolder) != null)
        {
          PdfArray pdfArray4 = (pdfArray1[index1] as PdfReferenceHolder).Object as PdfArray;
          for (int index4 = 0; index4 < pdfArray4.Count; ++index4)
          {
            if (!this.m_containsCmap)
              this.m_fontGlyphWidth.Add(key, (pdfArray4[index4] as PdfNumber).IntValue);
            else if (!this.m_fontGlyphWidth.ContainsKey(key))
              this.m_fontGlyphWidth.Add(key, (pdfArray4[index4] as PdfNumber).IntValue);
            ++key;
          }
        }
      }
    }
    catch
    {
      this.m_fontGlyphWidth = (Dictionary<int, int>) null;
    }
    pdfArray2 = (PdfArray) null;
  }

  private string[] GetFontFilter(PdfDictionary streamDictionary)
  {
    string[] fontFilter = (string[]) null;
    if (streamDictionary != null && streamDictionary.ContainsKey("Filter"))
    {
      if ((object) (streamDictionary["Filter"] as PdfName) != null)
        fontFilter = new string[1]
        {
          (streamDictionary["Filter"] as PdfName).Value
        };
      else if (streamDictionary["Filter"] is PdfArray)
      {
        PdfArray stream = streamDictionary["Filter"] as PdfArray;
        fontFilter = new string[stream.Count];
        for (int index = 0; index < stream.Count; ++index)
          fontFilter[index] = (stream[index] as PdfName).Value;
      }
      else if ((object) (streamDictionary["Filter"] as PdfReferenceHolder) != null)
      {
        PdfArray pdfArray = (streamDictionary["Filter"] as PdfReferenceHolder).Object as PdfArray;
        fontFilter = new string[pdfArray.Count];
        for (int index = 0; index < pdfArray.Count; ++index)
          fontFilter[index] = (pdfArray[index] as PdfName).Value;
      }
    }
    return fontFilter;
  }

  private MemoryStream DecodeASCII85Stream(MemoryStream encodedStream)
  {
    byte[] buffer = new ASCII85().decode(encodedStream.GetBuffer());
    MemoryStream memoryStream = new MemoryStream(buffer, 0, buffer.Length, true, true);
    memoryStream.Position = 0L;
    return memoryStream;
  }

  internal MemoryStream DecodeFlateStream(MemoryStream encodedStream)
  {
    encodedStream.Position = 0L;
    encodedStream.ReadByte();
    encodedStream.ReadByte();
    MemoryStream memoryStream = new MemoryStream();
    using (DeflateStream deflateStream = new DeflateStream((Stream) encodedStream, CompressionMode.Decompress, true))
    {
      byte[] buffer = new byte[4096 /*0x1000*/];
      while (true)
      {
        int count = deflateStream.Read(buffer, 0, 4096 /*0x1000*/);
        if (count > 0)
          memoryStream.Write(buffer, 0, count);
        else
          break;
      }
    }
    return memoryStream;
  }

  private Dictionary<double, string> GetCidToGidTable(byte[] cidTOGidmap)
  {
    Dictionary<double, string> cidToGidTable = new Dictionary<double, string>();
    byte[] bytes = new byte[2];
    int key = 0;
    int num;
    for (int index = 0; index < cidTOGidmap.Length; index = num + 1)
    {
      bytes[0] = cidTOGidmap[index];
      bytes[1] = cidTOGidmap[num = index + 1];
      string str = (!(this.FontEncoding == "Identity-H") ? Encoding.ASCII.GetString(bytes) : Encoding.BigEndianUnicode.GetString(bytes)).Replace("\0", "");
      cidToGidTable.Add((double) key, str);
      ++key;
    }
    return cidToGidTable;
  }

  private Dictionary<string, double> GetReverseMapTable()
  {
    this.m_reverseMapTable = new Dictionary<string, double>();
    foreach (KeyValuePair<double, string> keyValuePair in this.CharacterMapTable)
    {
      if (!this.m_reverseMapTable.ContainsKey(keyValuePair.Value))
        this.m_reverseMapTable.Add(keyValuePair.Value, keyValuePair.Key);
    }
    return this.m_reverseMapTable;
  }

  private Dictionary<double, string> GetCharacterMapTable()
  {
    int num1 = 0;
    Dictionary<double, string> characterMapTable = new Dictionary<double, string>();
    if (this.m_fontDictionary != null && this.m_fontDictionary.ContainsKey("ToUnicode"))
    {
      IPdfPrimitive font = this.m_fontDictionary["ToUnicode"];
      PdfStream pdfStream = (object) (font as PdfReferenceHolder) == null ? font as PdfStream : (font as PdfReferenceHolder).Object as PdfStream;
      if (pdfStream != null)
      {
        pdfStream.isSkip = true;
        pdfStream.Decompress();
        string str1 = Encoding.UTF8.GetString(pdfStream.Data, 0, pdfStream.Data.Length);
        bool flag1 = false;
        bool flag2 = false;
        int num2 = str1.IndexOf("begincmap");
        int num3 = str1.IndexOf("endcmap");
        int startIndex1 = num2;
        int startIndex2 = num2;
        int num4 = num3;
        if (startIndex1 == -1)
          return characterMapTable;
label_4:
        string str2;
        do
        {
          if (!flag1)
          {
            startIndex2 = str1.IndexOf("beginbfchar", startIndex1);
            if (startIndex2 < 0)
            {
              flag2 = false;
              startIndex2 = num2;
              startIndex1 = num2;
              num4 = num3;
            }
            else
            {
              num4 = str1.IndexOf("endbfchar", startIndex2);
              startIndex1 = num4;
              flag2 = true;
            }
          }
          if (!flag2)
          {
            int num5 = str1.IndexOf("beginbfrange", startIndex1);
            if (num5 < 0)
            {
              flag1 = false;
            }
            else
            {
              int num6 = str1.IndexOf("endbfrange", startIndex1 + 5);
              startIndex2 = num5;
              num4 = num6;
              startIndex1 = num4;
              flag1 = true;
            }
          }
          if (flag2 || flag1)
          {
            str2 = str1.Substring(startIndex2, num4 - startIndex2);
            if (flag2)
            {
              char[] chArray = new char[2]{ '\n', '\r' };
              string[] strArray = str2.Split(chArray);
              if (!strArray[0].Contains("\n") && !strArray[0].Contains("\r"))
              {
                List<string> stringList1 = new List<string>();
                for (int index1 = 0; index1 < strArray.Length; ++index1)
                {
                  List<string> hexCode = FontStructure.GetHexCode(strArray[index1]);
                  int count = hexCode.Count;
                  for (int index2 = 0; index2 < count / 2; ++index2)
                  {
                    if (hexCode.Count >= 2)
                    {
                      List<string> stringList2 = new List<string>();
                      stringList2.Add(hexCode[0]);
                      stringList2.Add(hexCode[1]);
                      hexCode.Remove(hexCode[0]);
                      hexCode.Remove(hexCode[0]);
                      if (stringList2.Count > 1)
                      {
                        if (stringList2[1].Length > 4)
                        {
                          string str3 = stringList2[1].Replace(" ", "");
                          string charvalue = "";
                          int num7 = str3.Length / 4;
                          for (int index3 = 0; index3 < num7; ++index3)
                          {
                            char ch = (char) long.Parse(str3.Substring(0, 4), NumberStyles.HexNumber);
                            str3 = str3.Substring(4);
                            charvalue += ch.ToString();
                          }
                          string str4 = this.CheckContainInvalidChar(charvalue);
                          if (!characterMapTable.ContainsKey((double) long.Parse(stringList2[0], NumberStyles.HexNumber)))
                            characterMapTable.Add((double) long.Parse(stringList2[0], NumberStyles.HexNumber), str4.ToString());
                        }
                        else if (!characterMapTable.ContainsKey((double) long.Parse(stringList2[0], NumberStyles.HexNumber)))
                        {
                          char ch = (char) long.Parse(stringList2[1], NumberStyles.HexNumber);
                          characterMapTable.Add((double) long.Parse(stringList2[0], NumberStyles.HexNumber), ch.ToString());
                        }
                      }
                    }
                  }
                }
              }
              else
              {
                for (int index4 = 0; index4 < strArray.Length; ++index4)
                {
                  this.tempStringList = FontStructure.GetHexCode(strArray[index4]);
                  if (this.tempStringList.Count > 1)
                  {
                    if (this.tempStringList[1].Length > 4)
                    {
                      string str5 = this.tempStringList[1].Replace(" ", "");
                      string charvalue = "";
                      int num8 = str5.Length / 4;
                      for (int index5 = 0; index5 < num8; ++index5)
                      {
                        char ch = (char) long.Parse(str5.Substring(0, 4), NumberStyles.HexNumber);
                        str5 = str5.Substring(4);
                        charvalue += ch.ToString();
                      }
                      string str6 = this.CheckContainInvalidChar(charvalue);
                      if (!characterMapTable.ContainsKey((double) long.Parse(this.tempStringList[0], NumberStyles.HexNumber)))
                        characterMapTable.Add((double) long.Parse(this.tempStringList[0], NumberStyles.HexNumber), str6.ToString());
                    }
                    else if (!characterMapTable.ContainsKey((double) long.Parse(this.tempStringList[0], NumberStyles.HexNumber)))
                    {
                      char ch = (char) long.Parse(this.tempStringList[1], NumberStyles.HexNumber);
                      characterMapTable.Add((double) long.Parse(this.tempStringList[0], NumberStyles.HexNumber), ch.ToString());
                    }
                  }
                }
              }
            }
          }
          else
            goto label_81;
        }
        while (!flag1);
        char[] chArray1 = new char[2]{ '\n', '\r' };
        string[] strArray1 = str2.Split(chArray1);
        for (int index6 = 0; index6 < strArray1.Length; ++index6)
        {
          if (strArray1[index6].Contains("["))
          {
            int startIndex3 = strArray1[index6].IndexOf("[");
            int num9 = strArray1[index6].IndexOf("]");
            string hexCode1;
            if (num9 == -1)
            {
              string str7 = strArray1[index6].Substring(startIndex3, strArray1[index6].Length - startIndex3);
              for (++index6; !strArray1[index6].Contains("]"); ++index6)
                str7 += strArray1[index6];
              hexCode1 = str7 + strArray1[index6].Substring(0, strArray1[index6].IndexOf("]"));
            }
            else
              hexCode1 = strArray1[index6].Substring(startIndex3, num9 - startIndex3);
            List<string> stringList = new List<string>();
            List<string> hexCode2 = FontStructure.GetHexCode(hexCode1);
            string hexCode3 = " ";
            if (num9 == -1)
            {
              for (int index7 = num1 + 1; index7 <= index6; ++index7)
                hexCode3 += strArray1[index7];
              this.tempStringList = FontStructure.GetHexCode(hexCode3);
            }
            else
              this.tempStringList = FontStructure.GetHexCode(strArray1[index6]);
            num1 = index6;
            if (this.tempStringList.Count > 1)
            {
              double num10 = (double) long.Parse(this.tempStringList[0], NumberStyles.HexNumber);
              double num11 = (double) long.Parse(this.tempStringList[1], NumberStyles.HexNumber);
              int index8 = 0;
              double key = num10;
              double num12 = 0.0;
              while (key <= num11)
              {
                string empty = string.Empty;
                string str8 = hexCode2[index8];
                char[] chArray2 = new char[1]{ ' ' };
                foreach (string str9 in str8.Split(chArray2))
                {
                  char ch = (char) (double) long.Parse(((int) (double) Convert.ToInt64(str9, 16 /*0x10*/)).ToString("x"), NumberStyles.HexNumber);
                  empty += (string) (object) ch;
                }
                if (!characterMapTable.ContainsKey(key))
                  characterMapTable.Add(key, empty);
                ++key;
                ++num12;
                ++index8;
              }
            }
          }
          else
          {
            this.tempStringList = FontStructure.GetHexCode(strArray1[index6]);
            if (this.tempStringList.Count == 3)
            {
              double num13 = (double) long.Parse(this.tempStringList[0], NumberStyles.HexNumber);
              double num14 = (double) long.Parse(this.tempStringList[1], NumberStyles.HexNumber);
              string str10 = this.tempStringList[2];
              if (this.tempStringList[2].Length > 4)
                str10 = str10.Substring(1, 4);
              double int64 = (double) Convert.ToInt64(str10, 16 /*0x10*/);
              double key = num13;
              double num15 = 0.0;
              while (key <= num14)
              {
                char ch = (char) (double) long.Parse(((int) (int64 + num15)).ToString("x"), NumberStyles.HexNumber);
                if (!characterMapTable.ContainsKey(key))
                  characterMapTable.Add(key, ch.ToString());
                ++key;
                ++num15;
              }
            }
            else if (this.tempStringList.Count > 1)
            {
              int count = this.tempStringList.Count;
              for (int index9 = 0; index9 < count; index9 += 3)
              {
                char ch = (char) long.Parse(this.tempStringList[index9 + 2], NumberStyles.HexNumber);
                characterMapTable.Add((double) long.Parse(this.tempStringList[index9], NumberStyles.HexNumber), ch.ToString());
              }
            }
          }
        }
        goto label_4;
      }
label_81:;
    }
    if (this.m_isSameFont)
    {
      foreach (KeyValuePair<double, string> keyValuePair in characterMapTable)
      {
        if (!this.tempMapTable.ContainsKey(keyValuePair.Key))
        {
          this.tempMapTable.Add(keyValuePair.Key, keyValuePair.Value);
        }
        else
        {
          this.tempMapTable.Remove(keyValuePair.Key);
          this.tempMapTable.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
    }
    return characterMapTable;
  }

  private Dictionary<string, string> GetDifferencesDictionary()
  {
    Dictionary<string, string> differencesDictionary = new Dictionary<string, string>();
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (this.m_fontDictionary != null && this.m_fontDictionary.ContainsKey("Encoding"))
    {
      if ((object) (this.m_fontDictionary["Encoding"] as PdfReferenceHolder) != null)
        pdfDictionary = (this.m_fontDictionary["Encoding"] as PdfReferenceHolder).Object as PdfDictionary;
      else if (this.m_fontDictionary["Encoding"] is PdfDictionary)
        pdfDictionary = this.m_fontDictionary["Encoding"] as PdfDictionary;
      if (pdfDictionary != null && pdfDictionary.ContainsKey("Differences"))
      {
        int num = 0;
        if (!(pdfDictionary["Differences"] is PdfArray pdfArray))
          pdfArray = (pdfDictionary["Differences"] as PdfReferenceHolder).Object as PdfArray;
        for (int index = 0; index < pdfArray.Count; ++index)
        {
          string empty = string.Empty;
          if (pdfArray[index] is PdfNumber)
            num = int.Parse((pdfArray[index] as PdfNumber).FloatValue.ToString());
          else if ((object) (pdfArray[index] as PdfName) != null)
          {
            string decodedCharacter1 = (pdfArray[index] as PdfName).Value;
            if (this.fontType.Value == "Type1" && decodedCharacter1 == ".notdef")
            {
              string decodedCharacter2 = " ";
              differencesDictionary.Add(num.ToString(), FontStructure.GetLatinCharacter(decodedCharacter2));
              ++num;
            }
            else
            {
              string specialCharacter = FontStructure.GetSpecialCharacter(FontStructure.GetLatinCharacter(decodedCharacter1));
              if (!differencesDictionary.ContainsKey(num.ToString()))
                differencesDictionary.Add(num.ToString(), FontStructure.GetLatinCharacter(specialCharacter));
              ++num;
            }
          }
        }
      }
    }
    return differencesDictionary;
  }

  internal static string GetLatinCharacter(string decodedCharacter)
  {
    switch (decodedCharacter)
    {
      case "zero":
        return "0";
      case "one":
        return "1";
      case "two":
        return "2";
      case "three":
        return "3";
      case "four":
        return "4";
      case "five":
        return "5";
      case "six":
        return "6";
      case "seven":
        return "7";
      case "eight":
        return "8";
      case "nine":
        return "9";
      case "aacute":
        return "á";
      case "asciicircum":
        return "^";
      case "asciitilde":
        return "~";
      case "asterisk":
        return "*";
      case "at":
        return "@";
      case "atilde":
        return "ã";
      case "backslash":
        return "\\";
      case "bar":
        return "|";
      case "braceleft":
        return "{";
      case "braceright":
        return "}";
      case "bracketleft":
        return "[";
      case "bracketright":
        return "]";
      case "breve":
        return "˘";
      case "brokenbar":
        return "|";
      case "bullet3":
        return "•";
      case "bullet":
        return "•";
      case "caron":
        return "ˇ";
      case "ccedilla":
        return "ç";
      case "cedilla":
        return "¸";
      case "cent":
        return "¢";
      case "circumflex":
        return "ˆ";
      case "colon":
        return ":";
      case "comma":
        return ",";
      case "copyright":
        return "©";
      case "currency1":
        return "¤";
      case "dagger":
        return "†";
      case "daggerdbl":
        return "‡";
      case "degree":
        return "°";
      case "dieresis":
        return "¨";
      case "divide":
        return "÷";
      case "dollar":
        return "$";
      case "dotaccent":
        return "˙";
      case "dotlessi":
        return "ı";
      case "eacute":
        return "é";
      case "middot":
        return "˙";
      case "edieresis":
        return "ë";
      case "egrave":
        return "è";
      case "ellipsis":
        return "...";
      case "emdash":
        return "—";
      case "endash":
        return "–";
      case "equal":
        return "=";
      case "eth":
        return "ð";
      case "exclam":
        return "!";
      case "exclamdown":
        return "¡";
      case "florin":
        return "ƒ";
      case "fraction":
        return "⁄";
      case "germandbls":
        return "ß";
      case "grave":
        return "`";
      case "greater":
        return ">";
      case "guillemotleft4":
        return "«";
      case "guillemotright4":
        return "»";
      case "guilsinglleft":
        return "‹";
      case "guilsinglright":
        return "›";
      case "hungarumlaut":
        return "˝";
      case "hyphen5":
        return "-";
      case "iacute":
        return "í";
      case "icircumflex":
        return "î";
      case "idieresis":
        return "ï";
      case "igrave":
        return "ì";
      case "less":
        return "<";
      case "logicalnot":
        return "¬";
      case "lslash":
        return "ł";
      case "Lslash":
        return "Ł";
      case "macron":
        return "¯";
      case "minus":
        return "−";
      case "mu":
        return "μ";
      case "multiply":
        return "×";
      case "ntilde":
        return "ñ";
      case "numbersign":
        return "#";
      case "oacute":
        return "ó";
      case "ocircumflex":
        return "ô";
      case "odieresis":
        return "ö";
      case "oe":
        return "oe";
      case "ogonek":
        return "˛";
      case "ograve":
        return "ò";
      case "onehalf":
        return "1/2";
      case "onequarter":
        return "1/4";
      case "onesuperior":
        return "\u00B9";
      case "ordfeminine":
        return "ª";
      case "ordmasculine":
        return "º";
      case "otilde":
        return "õ";
      case "paragraph":
        return "¶";
      case "parenleft":
        return "(";
      case "parenright":
        return ")";
      case "percent":
        return "%";
      case "period":
        return ".";
      case "periodcentered":
        return "·";
      case "perthousand":
        return "‰";
      case "plus":
        return "+";
      case "plusminus":
        return "±";
      case "question":
        return "?";
      case "questiondown":
        return "¿";
      case "quotedbl":
        return "\"";
      case "quotedblbase":
        return "„";
      case "quotedblleft":
        return "“";
      case "quotedblright":
        return "”";
      case "quoteleft":
        return "‘";
      case "quoteright":
        return "’";
      case "quotesinglbase":
        return "‚";
      case "quotesingle":
        return "'";
      case "registered":
        return "®";
      case "ring":
        return "˚";
      case "scaron":
        return "š";
      case "section":
        return "§";
      case "semicolon":
        return ";";
      case "slash":
        return "/";
      case "space6":
        return " ";
      case "space":
        return " ";
      case "udieresis":
        return "ü";
      case "uacute":
        return "ú";
      case "Ecircumflex":
        return "Ê";
      case "hyphen":
        return "-";
      case "underscore":
        return "_";
      case "adieresis":
        return "ä";
      case "ampersand":
        return "&";
      case "Adieresis":
        return "Ä";
      case "Udieresis":
        return "Ü";
      case "ccaron":
        return "č";
      case "Scaron":
        return "Š";
      case "zcaron":
        return "ž";
      case "sterling":
        return "£";
      case "agrave":
        return "à";
      case "ecircumflex":
        return "ê";
      case "acircumflex":
        return "â";
      case "Oacute":
        return "Ó";
      default:
        return decodedCharacter;
    }
  }

  internal static string GetSpecialCharacter(string decodedCharacter)
  {
    switch (decodedCharacter)
    {
      case "head2right":
        return "➢";
      case "aacute":
        return "á";
      case "eacute":
        return "é";
      case "iacute":
        return "í";
      case "oacute":
        return "ó";
      case "uacute":
        return "ú";
      case "circleright":
        return "➲";
      case "bleft":
        return "⇦";
      case "bright":
        return "⇨";
      case "bup":
        return "⇧";
      case "bdown":
        return "⇩";
      case "barb4right":
        return "➔";
      case "bleftright":
        return "⬄";
      case "bupdown":
        return "⇳";
      case "bnw":
        return "⬀";
      case "bne":
        return "⬁";
      case "bsw":
        return "⬃";
      case "bse":
        return "⬂";
      case "bdash1":
        return "▭";
      case "bdash2":
        return "▫";
      case "xmarkbld":
        return "✗";
      case "checkbld":
        return "✓";
      case "boxxmarkbld":
        return "☒";
      case "boxcheckbld":
        return "☑";
      case "space":
        return " ";
      case "pencil":
        return "✏";
      case "scissors":
        return "✂";
      case "scissorscutting":
        return "✁";
      case "readingglasses":
        return "✁";
      case "bell":
        return "✁";
      case "book":
        return "✁";
      case "telephonesolid":
        return "✁";
      case "telhandsetcirc":
        return "✁";
      case "envelopeback":
        return "✁";
      case "hourglass":
        return "⌛";
      case "keyboard":
        return "⌨";
      case "tapereel":
        return "✇";
      case "handwrite":
        return "✍";
      case "handv":
        return "✌";
      case "handptleft":
        return "☜";
      case "handptright":
        return "☞";
      case "handptup":
        return "☝";
      case "handptdown":
        return "☟";
      case "smileface":
        return "☺";
      case "frownface":
        return "☹";
      case "skullcrossbones":
        return "☠";
      case "flag":
        return "⚐";
      case "pennant":
        return "Ὢ9";
      case "airplane":
        return "✈";
      case "sunshine":
        return "☼";
      case "droplet":
        return "Ὂ7";
      case "snowflake":
        return "❄";
      case "crossshadow":
        return "✞";
      case "crossmaltese":
        return "✠";
      case "starofdavid":
        return "✡";
      case "crescentstar":
        return "☪";
      case "yinyang":
        return "☯";
      case "om":
        return "ॐ";
      case "wheel":
        return "☸";
      case "aries":
        return "♈";
      case "taurus":
        return "♉";
      case "gemini":
        return "♊";
      case "cancer":
        return "♋";
      case "leo":
        return "♌";
      case "virgo":
        return "♍";
      case "libra":
        return "♎";
      case "scorpio":
        return "♏";
      case "saggitarius":
        return "♐";
      case "capricorn":
        return "♑";
      case "aquarius":
        return "♒";
      case "pisces":
        return "♓";
      case "ampersanditlc":
        return "&";
      case "ampersandit":
        return "&";
      case "circle6":
        return "●";
      case "circleshadowdwn":
        return "❍";
      case "square6":
        return "■";
      case "box3":
        return "□";
      case "boxshadowdwn":
        return "❑";
      case "boxshadowup":
        return "❒";
      case "lozenge4":
        return "⬧";
      case "lozenge6":
        return "⧫";
      case "rhombus6":
        return "◆";
      case "xrhombus":
        return "❖";
      case "rhombus4":
        return "⬥";
      case "clear":
        return "⌧";
      case "escape":
        return "⍓";
      case "command":
        return "⌘";
      case "rosette":
        return "❀";
      case "rosettesolid":
        return "✿";
      case "quotedbllftbld":
        return "❝";
      case "quotedblrtbld":
        return "❞";
      case ".notdef":
        return "▯";
      case "zerosans":
        return "\u24EA";
      case "onesans":
        return "\u2460";
      case "twosans":
        return "\u2461";
      case "threesans":
        return "\u2462";
      case "foursans":
        return "\u2463";
      case "fivesans":
        return "\u2464";
      case "sixsans":
        return "\u2465";
      case "sevensans":
        return "\u2466";
      case "eightsans":
        return "\u2467";
      case "ninesans":
        return "\u2468";
      case "tensans":
        return "\u2469";
      case "zerosansinv":
        return "\u24FF";
      case "onesansinv":
        return "\u2776";
      case "twosansinv":
        return "\u2777";
      case "threesansinv":
        return "\u2778";
      case "foursansinv":
        return "\u2779";
      case "circle2":
        return "·";
      case "circle4":
        return "•";
      case "square2":
        return "▪";
      case "ring2":
        return "○";
      case "ringbutton2":
        return "◉";
      case "target":
        return "◎";
      case "square4":
        return "▪";
      case "box2":
        return "◻";
      case "crosstar2":
        return "✦";
      case "pentastar2":
        return "★";
      case "hexstar2":
        return "✶";
      case "octastar2":
        return "✴";
      case "dodecastar3":
        return "✹";
      case "octastar4":
        return "✵";
      case "registercircle":
        return "⌖";
      case "cuspopen":
        return "⟡";
      case "cuspopen1":
        return "⌑";
      case "circlestar":
        return "★";
      case "starshadow":
        return "✰";
      case "deleteleft":
        return "⌫";
      case "deleteright":
        return "⌦";
      case "scissorsoutline":
        return "✄";
      case "telephone":
        return "☏";
      case "telhandset":
        return "ὍE";
      case "handptlft1":
        return "☜";
      case "handptrt1":
        return "☞";
      case "handptlftsld1":
        return "☚";
      case "handptrtsld1":
        return "☛";
      case "handptup1":
        return "☝";
      case "handptdwn1":
        return "☟";
      case "xmark":
        return "✗";
      case "check":
        return "✓";
      case "boxcheck":
        return "☑";
      case "boxx":
        return "☒";
      case "boxxbld":
        return "☒";
      case "circlex":
        return "=⌔";
      case "circlexbld":
        return "⌔";
      case "prohibit":
      case "prohibitbld":
        return "⦸";
      case "ampersanditaldm":
      case "ampersandbld":
      case "ampersandsans":
      case "ampersandsandm":
        return "&";
      case "interrobang":
      case "interrobangdm":
      case "interrobangsans":
      case "interrobngsandm":
        return "‽";
      case "sacute":
        return "ś";
      case "Sacute":
        return "Ś";
      case "eogonek":
        return "ę";
      case "cacute":
        return "ć";
      case "aogonek":
        return "ą";
      default:
        return decodedCharacter;
    }
  }

  internal string MapCharactersFromTable(string decodedText)
  {
    string empty = string.Empty;
    bool flag = false;
    GlyphWriter glyphWriter = new GlyphWriter(this.m_cffGlyphs.Glyphs, this.Is1C);
    foreach (char ch in decodedText)
    {
      if (this.CharacterMapTable.ContainsKey((double) ch) && !flag)
      {
        string mappingString = this.CharacterMapTable[(double) ch];
        if (mappingString.Contains("�"))
        {
          int startIndex = mappingString.IndexOf("�");
          mappingString = mappingString.Remove(startIndex, 1);
          if (this.FontName.Contains("ZapfDingbats"))
            mappingString = ch.ToString();
        }
        if (this.FontEncoding != "Identity-H" && !this.IsTextExtraction && this.CharacterMapTable.Count != this.ReverseMapTable.Count)
        {
          if (glyphWriter.glyphs.ContainsKey(ch.ToString()) || this.IsCancel(mappingString) || this.IsNonPrintableCharacter(ch))
            mappingString = ch.ToString();
        }
        else if (!this.IsTextExtraction && glyphWriter.glyphs.ContainsKey(Convert.ToInt32(ch).ToString()) && this.CharacterMapTable.Count != this.ReverseMapTable.Count)
        {
          string key = Convert.ToString((int) ch);
          if (glyphWriter.glyphs.ContainsKey(key))
            mappingString = ch.ToString();
        }
        empty += mappingString;
        flag = false;
      }
      else if (!this.CharacterMapTable.ContainsKey((double) ch) && !flag)
      {
        byte[] bytes = Encoding.BigEndianUnicode.GetBytes(ch.ToString());
        if (bytes[0] != (byte) 92 && this.CharacterMapTable.ContainsKey((double) bytes[0]))
        {
          empty += this.CharacterMapTable[(double) bytes[0]];
          flag = false;
        }
      }
      else if (this.tempMapTable.ContainsKey((double) ch) && !flag)
      {
        string str = this.tempMapTable[(double) ch];
        if (ch == '\\' && this.IsTextExtraction)
          str = "";
        if (str.Contains("�"))
        {
          int startIndex = str.IndexOf("�");
          str = str.Remove(startIndex, 1);
        }
        empty += str;
        flag = false;
      }
      else if (flag)
      {
        switch (ch.ToString())
        {
          case "n":
            if (this.CharacterMapTable.ContainsKey(10.0))
            {
              empty += this.CharacterMapTable[10.0];
              break;
            }
            break;
          case "r":
            if (this.CharacterMapTable.ContainsKey(13.0))
            {
              empty += this.CharacterMapTable[13.0];
              break;
            }
            break;
          case "b":
            if (this.CharacterMapTable.ContainsKey(8.0))
            {
              empty += this.CharacterMapTable[8.0];
              break;
            }
            break;
          case "a":
            if (this.CharacterMapTable.ContainsKey(7.0))
            {
              empty += this.CharacterMapTable[7.0];
              break;
            }
            break;
          case "f":
            if (this.CharacterMapTable.ContainsKey(12.0))
            {
              empty += this.CharacterMapTable[12.0];
              break;
            }
            break;
          case "t":
            if (this.CharacterMapTable.ContainsKey(9.0))
            {
              empty += this.CharacterMapTable[9.0];
              break;
            }
            break;
          case "v":
            if (this.CharacterMapTable.ContainsKey(11.0))
            {
              empty += this.CharacterMapTable[11.0];
              break;
            }
            break;
          case "'":
            if (this.CharacterMapTable.ContainsKey(39.0))
            {
              empty += this.CharacterMapTable[39.0];
              break;
            }
            break;
          default:
            if (this.CharacterMapTable.ContainsKey((double) ch))
            {
              empty += this.CharacterMapTable[(double) ch];
              break;
            }
            break;
        }
        flag = false;
      }
      else if (ch == '\\')
        flag = true;
      else
        empty += (string) (object) ch;
    }
    return empty;
  }

  internal string MapCidToGid(string decodedText)
  {
    string empty = string.Empty;
    bool flag = false;
    foreach (char key in decodedText)
    {
      if (this.m_cidToGidTable.ContainsKey((double) key) && !flag)
      {
        string str = this.m_cidToGidTable[(double) key];
        if (str.Contains("�"))
        {
          int startIndex = str.IndexOf("�");
          str = str.Remove(startIndex, 1);
        }
        if (str.Length > 0 && !this.CidToGidReverseMapTable.ContainsKey((int) str[0]))
          this.CidToGidReverseMapTable.Add((int) str[0], (int) key);
        empty += str;
        flag = false;
      }
      else if (this.tempMapTable.ContainsKey((double) key) && !flag)
      {
        string str = this.tempMapTable[(double) key];
        if (str.Contains("�"))
        {
          int startIndex = str.IndexOf("�");
          str = str.Remove(startIndex, 1);
        }
        empty += str;
        flag = false;
      }
      else if (flag)
      {
        switch (key.ToString())
        {
          case "n":
            if (this.m_cidToGidTable.ContainsKey(10.0))
            {
              empty += this.CharacterMapTable[10.0];
              break;
            }
            break;
          case "r":
            if (this.m_cidToGidTable.ContainsKey(13.0))
            {
              empty += this.CharacterMapTable[13.0];
              break;
            }
            break;
          case "b":
            if (this.m_cidToGidTable.ContainsKey(8.0))
            {
              empty += this.CharacterMapTable[8.0];
              break;
            }
            break;
          case "a":
            if (this.m_cidToGidTable.ContainsKey(7.0))
            {
              empty += this.CharacterMapTable[7.0];
              break;
            }
            break;
          case "f":
            if (this.m_cidToGidTable.ContainsKey(12.0))
            {
              empty += this.CharacterMapTable[12.0];
              break;
            }
            break;
          case "t":
            if (this.m_cidToGidTable.ContainsKey(9.0))
            {
              empty += this.CharacterMapTable[9.0];
              break;
            }
            break;
          case "v":
            if (this.m_cidToGidTable.ContainsKey(11.0))
            {
              empty += this.CharacterMapTable[11.0];
              break;
            }
            break;
          case "'":
            if (this.m_cidToGidTable.ContainsKey(39.0))
            {
              empty += this.CharacterMapTable[39.0];
              break;
            }
            break;
          default:
            if (this.m_cidToGidTable.ContainsKey((double) key))
            {
              empty += this.CharacterMapTable[(double) key];
              break;
            }
            break;
        }
        flag = false;
      }
      else if (key == '\\')
        flag = true;
    }
    return empty;
  }

  internal string MapDifferences(string encodedText)
  {
    string str = string.Empty;
    bool flag = false;
    if (this.IsTextExtraction)
    {
      try
      {
        encodedText = Regex.Unescape(encodedText);
      }
      catch (ArgumentException ex)
      {
        encodedText = !string.IsNullOrEmpty(encodedText) ? Regex.Unescape(Regex.Escape(encodedText)) : throw ex;
      }
    }
    else
      this.SkipEscapeSequence(encodedText);
    foreach (char c in encodedText)
    {
      if (this.DifferencesDictionary.ContainsKey(((int) c).ToString()))
      {
        string differences1 = this.DifferencesDictionary[((int) c).ToString()];
        if (differences1.Length > 1 && this.fontType.Value != "Type3" && !this.IsTextExtraction)
          str += c.ToString();
        else if (!this.IsTextExtraction)
        {
          string textDecoded = this.DifferencesDictionary[((int) c).ToString()];
          if (textDecoded.Length == 7 && textDecoded.ToLowerInvariant().StartsWith("uni"))
            textDecoded = this.DecodeToUnicode(textDecoded);
          str += textDecoded;
        }
        else if (!char.IsLetter(c) && !this.DifferencesDictionary.ContainsKey(((int) c).ToString()))
        {
          str += (string) (object) AdobeGlyphList.GetUnicode(differences1);
        }
        else
        {
          string differences2 = this.DifferencesDictionary[((int) c).ToString()];
          str += differences2;
        }
        if (!this.ReverseDictMapping.ContainsKey(this.DifferencesDictionary[((int) c).ToString()]))
          this.ReverseDictMapping.Add(this.DifferencesDictionary[((int) c).ToString()], (int) c);
        if (this.FontName == "Wingdings")
          str = this.MapDifferenceOfWingDings(str);
        string specialCharacter = PdfTextExtractor.GetSpecialCharacter(str);
        if (str != specialCharacter && !this.isEmbedded)
          str = str.Replace(str, specialCharacter);
        flag = false;
      }
      else if (flag)
      {
        switch (c)
        {
          case 'n':
            if (this.DifferencesDictionary.ContainsKey(10.ToString()))
            {
              str += this.DifferencesDictionary[10.ToString()];
              break;
            }
            break;
          case 'r':
            if (this.DifferencesDictionary.ContainsKey(13.ToString()))
            {
              str += this.DifferencesDictionary[13.ToString()];
              break;
            }
            break;
        }
        flag = false;
      }
      else if (c == '\\')
        flag = true;
      else
        str += (string) (object) c;
    }
    return str;
  }

  internal string DecodeToUnicode(string textDecoded)
  {
    return ((char) new int[1]
    {
      int.Parse(textDecoded.Substring(3), NumberStyles.HexNumber)
    }[0]).ToString();
  }

  private string MapDifferenceOfWingDings(string decodedText)
  {
    if (decodedText.Length > 1 && decodedText.Contains("c") && decodedText.IndexOf("c") == 0)
    {
      decodedText = decodedText.Remove(0, 1);
      int result = 0;
      int.TryParse(decodedText, out result);
      decodedText = ((char) result).ToString();
    }
    return decodedText;
  }

  internal string SkipEscapeSequence(string text)
  {
    if (text.Contains("\\"))
    {
      int num = text.IndexOf('\\');
      if (num + 1 != text.Length)
      {
        string str = text.Substring(num + 1, 1);
        switch (str)
        {
          case "a":
            text = text.Replace("\\a", "\a");
            break;
          case "b":
            text = text.Replace("\\b", "\b");
            break;
          case "e":
            text = text.Replace("\\e", "\\e");
            break;
          case "f":
            text = text.Replace("\\f", "\f");
            break;
          case "n":
            text = text.Replace("\\n", "\n");
            break;
          case "r":
            text = text.Replace("\\r", "\r");
            break;
          case "t":
            text = text.Replace("\\t", "\t");
            break;
          case "v":
            text = text.Replace("\\v", "\v");
            break;
          case "'":
            text = text.Replace("\\'", "'");
            break;
          default:
            if (str.ToCharArray()[0] == '\u0003')
            {
              text = text.Replace("\\", "\\");
              break;
            }
            if (str.ToCharArray()[0] >= '\u007F')
            {
              text = text.Replace("\\", "");
              break;
            }
            try
            {
              text = Regex.Unescape(text);
              break;
            }
            catch (ArgumentException ex)
            {
              text = !string.IsNullOrEmpty(text) ? Regex.Unescape(Regex.Escape(text)) : throw ex;
              break;
            }
        }
      }
      else if (num + 1 == text.Length && text.Equals("\0\\"))
        text = text.Replace("\\", "");
    }
    return text;
  }

  private string EscapeSymbols(string text)
  {
    while (text.Contains("\n"))
      text = text.Replace("\n", "");
    return text;
  }

  internal static List<string> GetHexCode(string hexCode)
  {
    List<string> hexCode1 = new List<string>();
    string str1 = hexCode;
    int num1 = 0;
    int num2 = 0;
    while (num1 >= 0)
    {
      num1 = str1.IndexOf('<');
      int num3 = str1.IndexOf('>');
      if (num1 >= 0 && num3 >= 0)
      {
        string str2 = str1.Substring(num1 + 1, num3 - 1 - num1);
        hexCode1.Add(str2);
        str1 = str1.Substring(num3 + 1, str1.Length - 1 - num3);
      }
      ++num2;
    }
    return hexCode1;
  }

  internal static string GetCharCode(string decodedCharacter)
  {
    char ch = decodedCharacter.ToCharArray()[0];
    switch (decodedCharacter)
    {
      case "0":
        return "zero";
      case "1":
        return "one";
      case "2":
        return "two";
      case "3":
        return "three";
      case "4":
        return "four";
      case "5":
        return "five";
      case "6":
        return "six";
      case "7":
        return "seven";
      case "8":
        return "eight";
      case "9":
        return "nine";
      case "´":
        return "acute";
      case "å":
        return "aring";
      case "^":
        return "asciicircum";
      case "~":
        return "asciitilde";
      case "*":
        return "asterisk";
      case "@":
        return "at";
      case "ã":
        return "atilde";
      case "\\":
        return "backslash";
      case "|":
        return "bar";
      case "{":
        return "braceleft";
      case "}":
        return "braceright";
      case "[":
        return "bracketleft";
      case "]":
        return "bracketright";
      case "˘":
        return "breve";
      case "·":
      case "•":
        return "bullet";
      case "ˇ":
        return "caron";
      case "ç":
        return "ccedilla";
      case "¸":
        return "cedilla";
      case "¢":
        return "cent";
      case "ˆ":
        return "circumflex";
      case ":":
        return "colon";
      case ",":
        return "comma";
      case "©":
        return "copyright";
      case "\uF6D9":
        return "copyrightserif";
      case "¤":
        return "currency1";
      case "†":
        return "dagger";
      case "‡":
        return "daggerdbl";
      case "°":
        return "degree";
      case "¨":
        return "dieresis";
      case "÷":
        return "divide";
      case "$":
        return "dollar";
      case "˙":
        return "dotaccent";
      case "ı":
        return "dotlessi";
      case "é":
        return "eacute";
      case "ë":
        return "edieresis";
      case "è":
        return "egrave";
      case "…":
      case "...":
        return "ellipsis";
      case "—":
        return "emdash";
      case "–":
        return "endash";
      case "=":
        return "equal";
      case "ð":
        return "eth";
      case "€":
        return "Euro";
      case "!":
        return "exclam";
      case "¡":
        return "exclamdown";
      case "ƒ":
        return "florin";
      case "⁄":
        return "fraction";
      case "ß":
        return "germandbls";
      case "`":
        return "grave";
      case ">":
        return "greater";
      case "«":
        return "guillemotleft4";
      case "»":
        return "guillemotright";
      case "‹":
        return "guilsinglleft";
      case "›":
        return "guilsinglright";
      case "˝":
        return "hungarumlaut";
      case "í":
        return "iacute";
      case "î":
        return "icircumflex";
      case "ï":
        return "idieresis";
      case "ì":
        return "igrave";
      case "<":
        return "less";
      case "¬":
        return "logicalnot";
      case "ł":
        return "lslash";
      case "Ł":
        return "Lslash";
      case "¯":
        return "macron";
      case "−":
        return "minus";
      case "µ":
      case "μ":
        return "mu";
      case "×":
        return "multiply";
      case "ñ":
        return "ntilde";
      case "#":
        return "numbersign";
      case "ó":
        return "oacute";
      case "ô":
        return "ocircumflex";
      case "ö":
        return "odieresis";
      case "oe":
        return "oe";
      case "˛":
        return "ogonek";
      case "ò":
        return "ograve";
      case "1/2":
      case "\u00BD":
        return "onehalf";
      case "1/4":
        return "onequarter";
      case "\u00B9":
        return "onesuperior";
      case "ª":
        return "ordfeminine";
      case "º":
        return "ordmasculine";
      case "ø":
        return "oslash";
      case "õ":
        return "otilde";
      case "¶":
        return "paragraph";
      case "(":
        return "parenleft";
      case ")":
        return "parenright";
      case "%":
        return "percent";
      case ".":
        return "period";
      case "‰":
        return "perthousand";
      case "+":
        return "plus";
      case "±":
        return "plusminus";
      case "?":
        return "question";
      case "¿":
        return "questiondown";
      case "\"":
        return "quotedbl";
      case "„":
        return "quotedblbase";
      case "“":
        return "quotedblleft";
      case "”":
        return "quotedblright";
      case "‘":
        return "quoteleft";
      case "’":
        return "quoteright";
      case "‚":
        return "quotesinglbase";
      case "'":
        return "quotesingle";
      case "®":
        return "registered";
      case "˚":
        return "ring";
      case "š":
        return "scaron";
      case "§":
        return "section";
      case ";":
        return "semicolon";
      case "/":
        return "slash";
      case " ":
        return "space";
      case "£":
        return "sterling";
      case "™":
        return "trademark";
      case "ü":
        return "udieresis";
      case "-":
        return "hyphen";
      case "_":
        return "underscore";
      case "ä":
        return "adieresis";
      case "&":
        return "ampersand";
      case "Ä":
        return "Adieresis";
      case "Ü":
        return "Udieresis";
      case "č":
        return "ccaron";
      case "Š":
        return "Scaron";
      case "ž":
        return "zcaron";
      case "à":
        return "agrave";
      case "ê":
        return "ecircumflex";
      case "ﬁ":
        return "fi";
      case "ﬂ":
        return "fl";
      case "á":
        return "aacute";
      case "Á":
        return "Aacute";
      case "â":
        return "acircumflex";
      case "Â":
        return "Acircumflex";
      case "Ã":
        return "Atilde";
      case "æ":
        return "ae";
      case "Ç":
        return "Ccedilla";
      case "É":
        return "Eacute";
      case "Í":
        return "Iacute";
      case "Ó":
        return "Oacute";
      case "Õ":
        return "Otilde";
      case "ú":
        return "uacute";
      case "Ú":
        return "Uacute";
      case "¥":
        return "yen";
      case "ś":
        return "sacute";
      case "Ś":
        return "Sacute";
      case "ę":
        return "eogonek";
      case "ć":
        return "cacute";
      case "ą":
        return "aogonek";
      default:
        if (ch == '\u0081')
          decodedCharacter = "bullet";
        if (ch == '\u0085')
          decodedCharacter = "ellipsis";
        if (ch == '\u0091')
          decodedCharacter = "quoteleft";
        if (ch == '\u0092')
          decodedCharacter = "quoteright";
        if (ch == '\u0093')
          decodedCharacter = "quotedblleft";
        if (ch == '\u0094')
          decodedCharacter = "quotedblright";
        if (ch == '\u0095')
          decodedCharacter = "bullet";
        if (ch == '\u0096')
          decodedCharacter = "endash";
        if (ch == '\u0097')
          decodedCharacter = "emdash";
        if (ch == '\u0099')
          decodedCharacter = "trademark";
        if (ch >= '\b' && ch <= '\r')
          decodedCharacter = "space";
        return decodedCharacter;
    }
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
    for (int index1 = bytes.Length / 4; num6 < index1; ++num6)
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

  internal bool IsCancel(string mappingString)
  {
    bool flag = false;
    if (mappingString.Equals("\u0018"))
      flag = true;
    return flag;
  }

  private bool IsNonPrintableCharacter(char character)
  {
    bool flag = false;
    if (!this.IsTextExtraction && this.fontType.Value == "Type1" && this.FontEncoding == "Encoding" && this.FontName != "ZapfDingbats" && this.CharacterMapTable.Count == this.DifferencesDictionary.Count && (character >= char.MinValue && character <= '\u001F' || character == '\u007F'))
      flag = true;
    return flag;
  }

  internal FontStyle CheckFontStyle(string fontName)
  {
    if (fontName.Contains("Regular"))
      return FontStyle.Regular;
    if (fontName.Contains("Bold"))
      return FontStyle.Bold;
    return fontName.Contains("Italic") ? FontStyle.Italic : FontStyle.Regular;
  }

  internal static string CheckFontName(string fontName)
  {
    string str1 = fontName;
    if (str1.Contains("#20"))
      str1 = str1.Replace("#20", " ");
    string[] sourceArray = new string[1]{ "" };
    int length = 0;
    for (int startIndex = 0; startIndex < str1.Length; ++startIndex)
    {
      string str2 = str1.Substring(startIndex, 1);
      if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".Contains(str2) && startIndex > 0 && !"ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".Contains(str1[startIndex - 1].ToString()))
      {
        ++length;
        string[] destinationArray = new string[length + 1];
        System.Array.Copy((System.Array) sourceArray, 0, (System.Array) destinationArray, 0, length);
        sourceArray = destinationArray;
      }
      string[] strArray;
      IntPtr index;
      (strArray = sourceArray)[(int) (index = (IntPtr) length)] = strArray[index] + str2;
    }
    fontName = string.Empty;
    foreach (string str3 in sourceArray)
    {
      str3.Trim();
      fontName = $"{fontName}{str3} ";
    }
    if (fontName.Contains("Times"))
      fontName = "Times New Roman";
    if (fontName == "Bookshelf Symbol Seven")
      fontName = "Bookshelf Symbol 7";
    if (fontName.Contains("Song Std"))
      fontName = "Adobe Song Std L";
    if (fontName.Contains("Regular"))
      fontName = fontName.Replace("Regular", "");
    else if (fontName.Contains("Bold"))
      fontName = fontName.Replace("Bold", "");
    else if (fontName.Contains("Italic"))
      fontName = fontName.Replace("Italic", "");
    fontName = fontName.Trim();
    return fontName;
  }

  internal string MapZapf(string encodedText)
  {
    string key = (string) null;
    foreach (int num in encodedText)
    {
      switch (num.ToString("X"))
      {
        case "20":
          key += " ";
          this.ZapfPostScript += "space ";
          break;
        case "21":
          key += "✁";
          this.ZapfPostScript += "a1 ";
          break;
        case "22":
          key += "✂";
          this.ZapfPostScript += "a2 ";
          break;
        case "23":
          key += "✃";
          this.ZapfPostScript += "a202 ";
          break;
        case "24":
          key += "✄";
          this.ZapfPostScript += "a3 ";
          break;
        case "25":
          key += "☎";
          this.ZapfPostScript += "a4 ";
          break;
        case "26":
          key += "✆";
          this.ZapfPostScript += "a5 ";
          break;
        case "27":
          key += "✇";
          this.ZapfPostScript += "a119 ";
          break;
        case "28":
          key += "✈";
          this.ZapfPostScript += "a118 ";
          break;
        case "29":
          key += "✉";
          this.ZapfPostScript += "a117 ";
          break;
        case "2A":
          key += "☛";
          this.ZapfPostScript += "a11 ";
          break;
        case "2B":
          key += "☞";
          this.ZapfPostScript += "a12 ";
          break;
        case "2C":
          key += "✌";
          this.ZapfPostScript += "a13 ";
          break;
        case "2D":
          key += "✍";
          this.ZapfPostScript += "a14 ";
          break;
        case "2E":
          key += "✎";
          this.ZapfPostScript += "a15 ";
          break;
        case "2F":
          key += "✏";
          this.ZapfPostScript += "a16 ";
          break;
        case "30":
          key += "✐";
          this.ZapfPostScript += "a105 ";
          break;
        case "31":
          key += "✑";
          this.ZapfPostScript += "a17 ";
          break;
        case "32":
          key += "✒";
          this.ZapfPostScript += "a18 ";
          break;
        case "33":
          key += "✓";
          this.ZapfPostScript += "a19 ";
          break;
        case "34":
          key += "✔";
          this.ZapfPostScript += "a20 ";
          break;
        case "35":
          key += "✕";
          this.ZapfPostScript += "a21 ";
          break;
        case "36":
          key += "✖";
          this.ZapfPostScript += "a22 ";
          break;
        case "37":
          key += "✗";
          this.ZapfPostScript += "a23 ";
          break;
        case "38":
          key += "✘";
          this.ZapfPostScript += "a24 ";
          break;
        case "39":
          key += "✙";
          this.ZapfPostScript += "a25 ";
          break;
        case "3A":
          key += "✚";
          this.ZapfPostScript += "a26 ";
          break;
        case "3B":
          key += "✛";
          this.ZapfPostScript += "a27 ";
          break;
        case "3C":
          key += "✜";
          this.ZapfPostScript += "a28 ";
          break;
        case "3D":
          key += "✝";
          this.ZapfPostScript += "a6 ";
          break;
        case "3E":
          key += "✞";
          this.ZapfPostScript += "a7 ";
          break;
        case "3F":
          key += "✟";
          this.ZapfPostScript += "a8 ";
          break;
        case "40":
          key += "✠";
          this.ZapfPostScript += "a9 ";
          break;
        case "41":
          key += "✡";
          this.ZapfPostScript += "a10 ";
          break;
        case "42":
          key += "✢";
          this.ZapfPostScript += "a29 ";
          break;
        case "43":
          key += "✣";
          this.ZapfPostScript += "a30 ";
          break;
        case "44":
          key += "✤";
          this.ZapfPostScript += "a31 ";
          break;
        case "45":
          key += "✥";
          this.ZapfPostScript += "a32 ";
          break;
        case "46":
          key += "✦";
          this.ZapfPostScript += "a33 ";
          break;
        case "47":
          key += "✧";
          this.ZapfPostScript += "a34 ";
          break;
        case "48":
          key += "★";
          this.ZapfPostScript += "a35 ";
          break;
        case "49":
          key += "✩";
          this.ZapfPostScript += "a36 ";
          break;
        case "4A":
          key += "✪";
          this.ZapfPostScript += "a37 ";
          break;
        case "4B":
          key += "✫";
          this.ZapfPostScript += "a38 ";
          break;
        case "4C":
          key += "✬";
          this.ZapfPostScript += "a39 ";
          break;
        case "4D":
          key += "✭";
          this.ZapfPostScript += "a40 ";
          break;
        case "4E":
          key += "✮";
          this.ZapfPostScript += "a41 ";
          break;
        case "4F":
          key += "✯";
          this.ZapfPostScript += "a42 ";
          break;
        case "50":
          key += "✰";
          this.ZapfPostScript += "a43 ";
          break;
        case "51":
          key += "✱";
          this.ZapfPostScript += "a44 ";
          break;
        case "52":
          key += "✲";
          this.ZapfPostScript += "a45 ";
          break;
        case "53":
          key += "✳";
          this.ZapfPostScript += "a46 ";
          break;
        case "54":
          key += "✴";
          this.ZapfPostScript += "a47 ";
          break;
        case "55":
          key += "✵";
          this.ZapfPostScript += "a48 ";
          break;
        case "56":
          key += "✶";
          this.ZapfPostScript += "a49 ";
          break;
        case "57":
          key += "✷";
          this.ZapfPostScript += "a50 ";
          break;
        case "58":
          key += "✸";
          this.ZapfPostScript += "a51 ";
          break;
        case "59":
          key += "✹";
          this.ZapfPostScript += "a52 ";
          break;
        case "5A":
          key += "✺";
          this.ZapfPostScript += "a53 ";
          break;
        case "5B":
          key += "✻";
          this.ZapfPostScript += "a54 ";
          break;
        case "5C":
          key += "✼";
          this.ZapfPostScript += "a55 ";
          break;
        case "5D":
          key += "✽";
          this.ZapfPostScript += "a56 ";
          break;
        case "5E":
          key += "✾";
          this.ZapfPostScript += "a57 ";
          break;
        case "5F":
          key += "✿";
          this.ZapfPostScript += "a58 ";
          break;
        case "60":
          key += "❀";
          this.ZapfPostScript += "a59 ";
          break;
        case "61":
          key += "❁";
          this.ZapfPostScript += "a60 ";
          break;
        case "62":
          key += "❂";
          this.ZapfPostScript += "a61 ";
          break;
        case "63":
          key += "❃";
          this.ZapfPostScript += "a62 ";
          break;
        case "64":
          key += "❄";
          this.ZapfPostScript += "a63 ";
          break;
        case "65":
          key += "❅";
          this.ZapfPostScript += "a64 ";
          break;
        case "66":
          key += "❆";
          this.ZapfPostScript += "a65 ";
          break;
        case "67":
          key += "❇";
          this.ZapfPostScript += "a66 ";
          break;
        case "68":
          key += "❈";
          this.ZapfPostScript += "a67 ";
          break;
        case "69":
          key += "❉";
          this.ZapfPostScript += "a68 ";
          break;
        case "6A":
          key += "❊";
          this.ZapfPostScript += "a69 ";
          break;
        case "6B":
          key += "❋";
          this.ZapfPostScript += "a70 ";
          break;
        case "6C":
          key += "●";
          this.ZapfPostScript += "a71 ";
          break;
        case "6D":
          key += "╍";
          this.ZapfPostScript += "a72 ";
          break;
        case "6E":
          key += "■";
          this.ZapfPostScript += "a73 ";
          break;
        case "6F":
          key += "❏";
          this.ZapfPostScript += "a74 ";
          break;
        case "70":
          key += "❐";
          this.ZapfPostScript += "a203 ";
          break;
        case "71":
          key += "❑";
          this.ZapfPostScript += "a75 ";
          break;
        case "72":
          key += "❒";
          this.ZapfPostScript += "a204 ";
          break;
        case "73":
          key += "▲";
          this.ZapfPostScript += "a76 ";
          break;
        case "74":
          key += "▼";
          this.ZapfPostScript += "a77 ";
          break;
        case "75":
          key += "⟆";
          this.ZapfPostScript += "a78 ";
          break;
        case "76":
          key += "❖";
          this.ZapfPostScript += "a79 ";
          break;
        case "77":
          key += "◗";
          this.ZapfPostScript += "a81 ";
          break;
        case "78":
          key += "❘";
          this.ZapfPostScript += "a82 ";
          break;
        case "79":
          key += "❙";
          this.ZapfPostScript += "a83 ";
          break;
        case "7A":
          key += "❚";
          this.ZapfPostScript += "a84 ";
          break;
        case "7B":
          key += "❛";
          this.ZapfPostScript += "a97 ";
          break;
        case "7C":
          key += "❜";
          this.ZapfPostScript += "a98 ";
          break;
        case "7D":
          key += "❝";
          this.ZapfPostScript += "a99 ";
          break;
        case "7E":
          key += "❞";
          this.ZapfPostScript += "a100 ";
          break;
        case "80":
          key += "\uF8D7";
          this.ZapfPostScript += "a89 ";
          break;
        case "81":
          key += "\uF8D8";
          this.ZapfPostScript += "a90 ";
          break;
        case "82":
          key += "\uF8D9";
          this.ZapfPostScript += "a93 ";
          break;
        case "83":
          key += "\uF8DA";
          this.ZapfPostScript += "a94 ";
          break;
        case "84":
          key += "\uF8DB";
          this.ZapfPostScript += "a91 ";
          break;
        case "85":
          key += "\uF8DC";
          this.ZapfPostScript += "a92 ";
          break;
        case "86":
          key += "\uF8DD";
          this.ZapfPostScript += "a205 ";
          break;
        case "87":
          key += "\uF8DE";
          this.ZapfPostScript += "a85 ";
          break;
        case "88":
          key += "\uF8DF";
          this.ZapfPostScript += "a206 ";
          break;
        case "89":
          key += "\uF8E0";
          this.ZapfPostScript += "a86 ";
          break;
        case "8A":
          key += "\uF8E1";
          this.ZapfPostScript += "a87 ";
          break;
        case "8B":
          key += "\uF8E2";
          this.ZapfPostScript += "a88 ";
          break;
        case "8C":
          key += "\uF8E3";
          this.ZapfPostScript += "a95 ";
          break;
        case "8D":
          key += "\uF8E4";
          this.ZapfPostScript += "a96 ";
          break;
        case "A1":
          key += "❡";
          this.ZapfPostScript += "a101 ";
          break;
        case "A2":
          key += "❢";
          this.ZapfPostScript += "a102 ";
          break;
        case "A3":
          key += "❣";
          this.ZapfPostScript += "a103 ";
          break;
        case "A4":
          key += "❤";
          this.ZapfPostScript += "a104 ";
          break;
        case "A5":
          key += "❥";
          this.ZapfPostScript += "a106 ";
          break;
        case "A6":
          key += "❦";
          this.ZapfPostScript += "a107 ";
          break;
        case "A7":
          key += "❧";
          this.ZapfPostScript += "a108 ";
          break;
        case "A8":
          key += "♣";
          this.ZapfPostScript += "a112 ";
          break;
        case "A9":
          key += "♦";
          this.ZapfPostScript += "a111 ";
          break;
        case "AA":
          key += "♥";
          this.ZapfPostScript += "a110 ";
          break;
        case "AB":
          key += "♠";
          this.ZapfPostScript += "a109 ";
          break;
        case "AC":
          key += "\u2460";
          this.ZapfPostScript += "a120 ";
          break;
        case "AD":
          key += "\u2461";
          this.ZapfPostScript += "a121 ";
          break;
        case "AE":
          key += "\u2462";
          this.ZapfPostScript += "a122 ";
          break;
        case "AF":
          key += "\u2463";
          this.ZapfPostScript += "a123 ";
          break;
        case "B0":
          key += "\u2464";
          this.ZapfPostScript += "a124 ";
          break;
        case "B1":
          key += "\u2465";
          this.ZapfPostScript += "a125 ";
          break;
        case "B2":
          key += "\u2466";
          this.ZapfPostScript += "a126 ";
          break;
        case "B3":
          key += "\u2467";
          this.ZapfPostScript += "a127 ";
          break;
        case "B4":
          key += "\u2468";
          this.ZapfPostScript += "a128 ";
          break;
        case "B5":
          key += "\u2469";
          this.ZapfPostScript += "a129 ";
          break;
        case "B6":
          key += "\u2776";
          this.ZapfPostScript += "a130 ";
          break;
        case "B7":
          key += "\u2777";
          this.ZapfPostScript += "a131 ";
          break;
        case "B8":
          key += "\u2778";
          this.ZapfPostScript += "a132 ";
          break;
        case "B9":
          key += "\u2779";
          this.ZapfPostScript += "a133 ";
          break;
        case "BA":
          key += "\u277A";
          this.ZapfPostScript += "a134 ";
          break;
        case "BB":
          key += "\u277B";
          this.ZapfPostScript += "a135 ";
          break;
        case "BC":
          key += "\u277C";
          this.ZapfPostScript += "a136 ";
          break;
        case "BD":
          key += "\u277D";
          this.ZapfPostScript += "a137 ";
          break;
        case "BE":
          key += "\u277E";
          this.ZapfPostScript += "a138 ";
          break;
        case "BF":
          key += "\u277F";
          this.ZapfPostScript += "a139 ";
          break;
        case "C0":
          key += "\u2780";
          this.ZapfPostScript += "a140 ";
          break;
        case "C1":
          key += "\u2781";
          this.ZapfPostScript += "a141 ";
          break;
        case "C2":
          key += "\u2782";
          this.ZapfPostScript += "a142 ";
          break;
        case "C3":
          key += "\u2783";
          this.ZapfPostScript += "a143 ";
          break;
        case "C4":
          key += "\u2784";
          this.ZapfPostScript += "a144 ";
          break;
        case "C5":
          key += "\u2785";
          this.ZapfPostScript += "a145 ";
          break;
        case "C6":
          key += "\u2786";
          this.ZapfPostScript += "a146 ";
          break;
        case "C7":
          key += "\u2787";
          this.ZapfPostScript += "a147 ";
          break;
        case "C8":
          key += "\u2788";
          this.ZapfPostScript += "a148 ";
          break;
        case "C9":
          key += "\u2789";
          this.ZapfPostScript += "a149 ";
          break;
        case "CA":
          key += "\u278A";
          this.ZapfPostScript += "150 ";
          break;
        case "CB":
          key += "\u278B";
          this.ZapfPostScript += "a151 ";
          break;
        case "CC":
          key += "\u278C";
          this.ZapfPostScript += "a152 ";
          break;
        case "CD":
          key += "\u278D";
          this.ZapfPostScript += "a153 ";
          break;
        case "CE":
          key += "\u278E";
          this.ZapfPostScript += "a154 ";
          break;
        case "CF":
          key += "\u278F";
          this.ZapfPostScript += "a155 ";
          break;
        case "D0":
          key += "\u2790";
          this.ZapfPostScript += "a156 ";
          break;
        case "D1":
          key += "\u2791";
          this.ZapfPostScript += "a157 ";
          break;
        case "D2":
          key += "\u2792";
          this.ZapfPostScript += "a158 ";
          break;
        case "D3":
          key += "\u2793";
          this.ZapfPostScript += "a159 ";
          break;
        case "D4":
          key += "➔";
          this.ZapfPostScript += "a160 ";
          break;
        case "D5":
          key += "→";
          this.ZapfPostScript += "a161 ";
          break;
        case "D6":
          key += "↔";
          this.ZapfPostScript += "a163 ";
          break;
        case "D7":
          key += "↕";
          this.ZapfPostScript += "a164 ";
          break;
        case "D8":
          key += "➘";
          this.ZapfPostScript += "a196 ";
          break;
        case "D9":
          key += "➙";
          this.ZapfPostScript += "a165 ";
          break;
        case "DA":
          key += "➚";
          this.ZapfPostScript += "a192 ";
          break;
        case "DB":
          key += "➛";
          this.ZapfPostScript += "a166 ";
          break;
        case "DC":
          key += "➜";
          this.ZapfPostScript += "a167 ";
          break;
        case "DD":
          key += "➝";
          this.ZapfPostScript += "a168 ";
          break;
        case "DE":
          key += "➞";
          this.ZapfPostScript += "a169 ";
          break;
        case "DF":
          key += "➟";
          this.ZapfPostScript += "a170 ";
          break;
        case "E0":
          key += "➠";
          this.ZapfPostScript += "a171 ";
          break;
        case "E1":
          key += "➡";
          this.ZapfPostScript += "a172 ";
          break;
        case "E2":
          key += "➢";
          this.ZapfPostScript += "a173 ";
          break;
        case "E3":
          key += "➣";
          this.ZapfPostScript += "a162 ";
          break;
        case "E4":
          key += "➤";
          this.ZapfPostScript += "a174 ";
          break;
        case "E5":
          key += "➥";
          this.ZapfPostScript += "a175 ";
          break;
        case "E6":
          key += "➦";
          this.ZapfPostScript += "a176 ";
          break;
        case "E7":
          key += "➧";
          this.ZapfPostScript += "a177 ";
          break;
        case "E8":
          key += "➨";
          this.ZapfPostScript += "a178 ";
          break;
        case "E9":
          key += "➩";
          this.ZapfPostScript += "a179 ";
          break;
        case "EA":
          key += "➪";
          this.ZapfPostScript += "a193 ";
          break;
        case "EB":
          key += "➫";
          this.ZapfPostScript += "a180 ";
          break;
        case "EC":
          key += "➬";
          this.ZapfPostScript += "a199 ";
          break;
        case "ED":
          key += "➭";
          this.ZapfPostScript += "a181 ";
          break;
        case "EE":
          key += "➮";
          this.ZapfPostScript += "a200 ";
          break;
        case "EF":
          key += "➯";
          this.ZapfPostScript += "a182 ";
          break;
        case "F1":
          key += "➱";
          this.ZapfPostScript += "a201 ";
          break;
        case "F2":
          key += "➲";
          this.ZapfPostScript += "a183 ";
          break;
        case "F3":
          key += "➳";
          this.ZapfPostScript += "a184 ";
          break;
        case "F4":
          key += "➴";
          this.ZapfPostScript += "a197 ";
          break;
        case "F5":
          key += "➵";
          this.ZapfPostScript += "a185 ";
          break;
        case "F6":
          key += "➶";
          this.ZapfPostScript += "a194 ";
          break;
        case "F7":
          key += "➷";
          this.ZapfPostScript += "a198 ";
          break;
        case "F8":
          key += "➸";
          this.ZapfPostScript += "a186 ";
          break;
        case "F9":
          key += "➹";
          this.ZapfPostScript += "a195 ";
          break;
        case "FA":
          key += "➺";
          this.ZapfPostScript += "a187 ";
          break;
        case "FB":
          key += "➻";
          this.ZapfPostScript += "a188 ";
          break;
        case "FC":
          key += "➼";
          this.ZapfPostScript += "a189 ";
          break;
        case "FD":
          key += "➽";
          this.ZapfPostScript += "a190 ";
          break;
        case "FE":
          key += "➾";
          this.ZapfPostScript += "a191 ";
          break;
        default:
          if (this.ReverseMapTable.ContainsKey(encodedText))
          {
            key = encodedText;
            this.ZapfPostScript = this.differenceTable[(int) this.ReverseMapTable[key]];
            break;
          }
          key = "✈";
          this.ZapfPostScript = "a118";
          break;
      }
    }
    return key;
  }

  internal Dictionary<int, string> GetUnicodeCharMapTable()
  {
    FontStructure.unicodeCharMapTable = new Dictionary<int, string>();
    StreamReader standardEncoding = Resources.standard_encoding;
    while (true)
    {
      string str1;
      int uint32;
      do
      {
        string str2;
        do
        {
          string str3 = standardEncoding.ReadLine();
          if (str3 != null)
          {
            string[] strArray = str3.Split(' ');
            str1 = strArray[0];
            str2 = strArray[1];
          }
          else
            goto label_5;
        }
        while (!char.IsDigit(str2[0]));
        uint32 = (int) Convert.ToUInt32(str2, 8);
      }
      while (FontStructure.unicodeCharMapTable.ContainsKey(uint32));
      FontStructure.unicodeCharMapTable.Add(uint32, str1);
    }
label_5:
    return FontStructure.unicodeCharMapTable;
  }

  internal bool IsCIDFontType()
  {
    bool flag = false;
    if (this.m_fontDictionary.Items.ContainsKey(new PdfName("DescendantFonts")))
    {
      PdfReferenceHolder pdfReferenceHolder = this.m_fontDictionary.Items[new PdfName("DescendantFonts")] as PdfReferenceHolder;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
      {
        PdfArray pdfArray = pdfReferenceHolder.Object as PdfArray;
        if (pdfArray[0] as PdfReferenceHolder != (PdfReferenceHolder) null)
        {
          PdfName pdfName = ((pdfArray[0] as PdfReferenceHolder).Object as PdfDictionary)["Subtype"] as PdfName;
          if (pdfName.Value == "CIDFontType2" || pdfName.Value == "CIDFontType0")
            flag = true;
        }
      }
      if (pdfReferenceHolder == (PdfReferenceHolder) null && this.m_fontDictionary.Items[new PdfName("DescendantFonts")] is PdfArray)
      {
        PdfArray pdfArray1 = this.m_fontDictionary.Items[new PdfName("DescendantFonts")] as PdfArray;
        PdfArray pdfArray2;
        if (pdfArray1[0] as PdfReferenceHolder != (PdfReferenceHolder) null)
        {
          PdfName pdfName = ((pdfArray1[0] as PdfReferenceHolder).Object as PdfDictionary)["Subtype"] as PdfName;
          pdfArray2 = (PdfArray) null;
          if (pdfName.Value == "CIDFontType2" || pdfName.Value == "CIDFontType0")
            flag = true;
        }
        else if (pdfArray1[0] is PdfDictionary)
        {
          PdfName pdfName = (pdfArray1[0] as PdfDictionary)["Subtype"] as PdfName;
          pdfArray2 = (PdfArray) null;
          if (pdfName.Value == "CIDFontType2" || pdfName.Value == "CIDFontType0")
            flag = true;
        }
      }
    }
    return flag;
  }

  private string CheckContainInvalidChar(string charvalue)
  {
    foreach (int num in charvalue.ToCharArray())
    {
      switch (num)
      {
        case 160 /*0xA0*/:
          charvalue = " ";
          break;
        case 61558:
          charvalue = "";
          break;
      }
    }
    return charvalue;
  }

  public GraphicsPath GetGlyph(char val)
  {
    if (this.GlyphFontFile2 != null && this.IsNonSymbol || this.FontEncoding != null && (this.FontEncoding == "MacRomanEncoding" || this.FontEncoding == "WinAnsiEncoding"))
    {
      CmapTables cmaptable1 = this.GlyphFontFile2.Cmap.GetCmaptable((ushort) 3, (ushort) 1);
      if (cmaptable1 != null)
      {
        this.Graphic = this.GlyphFontFile2.GetGlyfPathMicrosoftwithencoding(cmaptable1, val, this.DifferencesDictionaryValues);
        return this.Graphic;
      }
      CmapTables cmaptable2 = this.GlyphFontFile2.Cmap.GetCmaptable((ushort) 1, (ushort) 0);
      if (cmaptable2 != null)
      {
        bool isWinAnsiEncoding = this.FontEncoding == "WinAnsiEncoding";
        return this.Graphic = this.GlyphFontFile2.GetGlyphsPathMacWithEncoding(cmaptable2, val, isWinAnsiEncoding);
      }
      CmapTables cmaptable3 = this.GlyphFontFile2.Cmap.GetCmaptable((ushort) 0, (ushort) 1);
      if (cmaptable3 != null)
        return this.Graphic = this.GlyphFontFile2.GetGlyfPathMicrosoftwithencoding(cmaptable3, val, this.DifferencesDictionaryValues);
      CmapTables cmaptable4 = this.GlyphFontFile2.Cmap.GetCmaptable((ushort) 0, (ushort) 3);
      if (cmaptable4 != null)
      {
        bool isWinAnsiEncoding = this.FontEncoding == "WinAnsiEncoding";
        return this.Graphic = this.GlyphFontFile2.GetGlyphsPathMacWithEncoding(cmaptable4, val, isWinAnsiEncoding);
      }
    }
    else if (this.GlyphFontFile2 != null && this.Issymbol || this.FontEncoding == null)
    {
      CmapTables cmaptable5 = this.GlyphFontFile2.Cmap.GetCmaptable((ushort) 3, (ushort) 0);
      if (cmaptable5 != null)
      {
        this.Graphic = this.GlyphFontFile2.GetGlyfPathWindowsWithoutEncoding(cmaptable5, val);
        return this.Graphic;
      }
      CmapTables cmaptable6 = this.GlyphFontFile2.Cmap.GetCmaptable((ushort) 1, (ushort) 0);
      if (cmaptable6 != null)
      {
        this.Graphic = this.GlyphFontFile2.GetGlyphsPathMacWithoutencoding(cmaptable6, val);
        return this.Graphic;
      }
    }
    return (GraphicsPath) null;
  }

  public static bool GetBit(int n, byte bit) => (n & 1 << (int) bit) != 0;

  private bool GetFlag(byte bit)
  {
    --bit;
    return FontStructure.GetBit(this.Flags.IntValue, bit);
  }

  private string[] getMapDifference()
  {
    if (this.DifferencesDictionary != null && this.DifferencesDictionary.Count > 0)
    {
      this.m_differenceDictionaryValues = new string[256 /*0x0100*/];
      int result = 0;
      List<string> stringList1 = new List<string>((IEnumerable<string>) this.DifferencesDictionary.Keys);
      List<string> stringList2 = new List<string>((IEnumerable<string>) this.DifferencesDictionary.Values);
      for (int index = 0; index < stringList1.Count; ++index)
      {
        int.TryParse(stringList1[index], out result);
        if (result < 256 /*0x0100*/)
          this.m_differenceDictionaryValues[result] = stringList2[index];
      }
    }
    return this.m_differenceDictionaryValues;
  }

  private PdfNumber GetFlagValue()
  {
    if (this.FontEncoding != "Identity-H")
    {
      if (this.m_fontDictionary.Items.ContainsKey(new PdfName("FontDescriptor")))
      {
        PdfReferenceHolder pdfReferenceHolder = this.m_fontDictionary.Items[new PdfName("FontDescriptor")] as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Object is PdfDictionary)
        {
          PdfDictionary pdfDictionary = pdfReferenceHolder.Object as PdfDictionary;
          if (pdfDictionary != null && pdfDictionary.Items.ContainsKey(new PdfName("Flags")))
            return pdfDictionary.Items[new PdfName("Flags")] as PdfNumber;
        }
      }
    }
    else if (this.m_fontDictionary.Items.ContainsKey(new PdfName("DescendantFonts")))
    {
      if (this.m_fontDictionary.Items[new PdfName("DescendantFonts")] is PdfArray)
      {
        if (this.m_fontDictionary.Items[new PdfName("DescendantFonts")] is PdfArray pdfArray1 && (object) (pdfArray1[0] as PdfReferenceHolder) != null)
        {
          PdfReferenceHolder pdfReferenceHolder = pdfArray1[0] as PdfReferenceHolder;
          if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Object is PdfDictionary)
          {
            PdfDictionary pdfDictionary1 = pdfReferenceHolder.Object as PdfDictionary;
            if (pdfDictionary1 != null && pdfDictionary1.Items.ContainsKey(new PdfName("FontDescriptor")))
            {
              PdfDictionary pdfDictionary2;
              if (pdfDictionary1.Items[new PdfName("FontDescriptor")] is PdfDictionary)
                pdfDictionary2 = pdfDictionary1.Items[new PdfName("FontDescriptor")] as PdfDictionary;
              if ((object) (pdfDictionary1.Items[new PdfName("FontDescriptor")] as PdfReferenceHolder) != null)
              {
                PdfDictionary pdfDictionary3 = (pdfDictionary1.Items[new PdfName("FontDescriptor")] as PdfReferenceHolder).Object as PdfDictionary;
                if (pdfDictionary3 != null && pdfDictionary3.Items.ContainsKey(new PdfName("Flags")))
                {
                  PdfNumber flagValue = pdfDictionary3.Items[new PdfName("Flags")] as PdfNumber;
                  pdfDictionary2 = (PdfDictionary) null;
                  return flagValue;
                }
              }
            }
          }
        }
      }
      else
      {
        PdfReferenceHolder pdfReferenceHolder = this.m_fontDictionary.Items[new PdfName("DescendantFonts")] as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null)
        {
          PdfArray pdfArray2 = pdfReferenceHolder.Object as PdfArray;
          if (pdfArray2[0] as PdfReferenceHolder != (PdfReferenceHolder) null)
          {
            PdfName pdfName = ((pdfArray2[0] as PdfReferenceHolder).Object as PdfDictionary)["Subtype"] as PdfName;
            if (!(pdfName.Value == "CIDFontType2"))
            {
              int num = pdfName.Value == "CIDFontType0" ? 1 : 0;
            }
          }
        }
      }
    }
    return (PdfNumber) null;
  }

  private void GetMacEncodeTable()
  {
    this.m_macEncodeTable = new Dictionary<int, string>();
    this.m_macEncodeTable.Add((int) sbyte.MaxValue, " ");
    this.m_macEncodeTable.Add(128 /*0x80*/, "Ä");
    this.m_macEncodeTable.Add(129, "Å");
    this.m_macEncodeTable.Add(130, "Ç");
    this.m_macEncodeTable.Add(131, "É");
    this.m_macEncodeTable.Add(132, "Ñ");
    this.m_macEncodeTable.Add(133, "Ö");
    this.m_macEncodeTable.Add(134, "Ü");
    this.m_macEncodeTable.Add(135, "á");
    this.m_macEncodeTable.Add(136, "à");
    this.m_macEncodeTable.Add(137, "â");
    this.m_macEncodeTable.Add(138, "ä");
    this.m_macEncodeTable.Add(139, "ã");
    this.m_macEncodeTable.Add(140, "å");
    this.m_macEncodeTable.Add(141, "ç");
    this.m_macEncodeTable.Add(142, "é");
    this.m_macEncodeTable.Add(143, "è");
    this.m_macEncodeTable.Add(144 /*0x90*/, "ê");
    this.m_macEncodeTable.Add(145, "ë");
    this.m_macEncodeTable.Add(146, "í");
    this.m_macEncodeTable.Add(147, "ì");
    this.m_macEncodeTable.Add(148, "î");
    this.m_macEncodeTable.Add(149, "ï");
    this.m_macEncodeTable.Add(150, "ñ");
    this.m_macEncodeTable.Add(151, "ó");
    this.m_macEncodeTable.Add(152, "ò");
    this.m_macEncodeTable.Add(153, "ô");
    this.m_macEncodeTable.Add(154, "ö");
    this.m_macEncodeTable.Add(155, "õ");
    this.m_macEncodeTable.Add(156, "ú");
    this.m_macEncodeTable.Add(157, "ù");
    this.m_macEncodeTable.Add(158, "û");
    this.m_macEncodeTable.Add(159, "ü");
    this.m_macEncodeTable.Add(160 /*0xA0*/, "†");
    this.m_macEncodeTable.Add(161, "°");
    this.m_macEncodeTable.Add(162, "¢");
    this.m_macEncodeTable.Add(163, "£");
    this.m_macEncodeTable.Add(164, "§");
    this.m_macEncodeTable.Add(165, "•");
    this.m_macEncodeTable.Add(166, "¶");
    this.m_macEncodeTable.Add(167, "ß");
    this.m_macEncodeTable.Add(168, "®");
    this.m_macEncodeTable.Add(169, "©");
    this.m_macEncodeTable.Add(170, "™");
    this.m_macEncodeTable.Add(171, "´");
    this.m_macEncodeTable.Add(172, "¨");
    this.m_macEncodeTable.Add(173, "≠");
    this.m_macEncodeTable.Add(174, "Æ");
    this.m_macEncodeTable.Add(175, "Ø");
    this.m_macEncodeTable.Add(176 /*0xB0*/, "∞");
    this.m_macEncodeTable.Add(177, "±");
    this.m_macEncodeTable.Add(178, "≤");
    this.m_macEncodeTable.Add(179, "≥");
    this.m_macEncodeTable.Add(180, "¥");
    this.m_macEncodeTable.Add(181, "µ");
    this.m_macEncodeTable.Add(182, "∂");
    this.m_macEncodeTable.Add(183, "∑");
    this.m_macEncodeTable.Add(184, "∏");
    this.m_macEncodeTable.Add(185, "π");
    this.m_macEncodeTable.Add(186, "∫");
    this.m_macEncodeTable.Add(187, "ª");
    this.m_macEncodeTable.Add(188, "º");
    this.m_macEncodeTable.Add(189, "Ω");
    this.m_macEncodeTable.Add(190, "æ");
    this.m_macEncodeTable.Add(191, "ø");
    this.m_macEncodeTable.Add(192 /*0xC0*/, "¿");
    this.m_macEncodeTable.Add(193, "¡");
    this.m_macEncodeTable.Add(194, "¬");
    this.m_macEncodeTable.Add(195, "√");
    this.m_macEncodeTable.Add(196, "ƒ");
    this.m_macEncodeTable.Add(197, "≈");
    this.m_macEncodeTable.Add(198, "∆");
    this.m_macEncodeTable.Add(199, "«");
    this.m_macEncodeTable.Add(200, "»");
    this.m_macEncodeTable.Add(201, "…");
    this.m_macEncodeTable.Add(202, " ");
    this.m_macEncodeTable.Add(203, "À");
    this.m_macEncodeTable.Add(204, "Ã");
    this.m_macEncodeTable.Add(205, "Õ");
    this.m_macEncodeTable.Add(206, "Œ");
    this.m_macEncodeTable.Add(207, "œ");
    this.m_macEncodeTable.Add(208 /*0xD0*/, "–");
    this.m_macEncodeTable.Add(209, "—");
    this.m_macEncodeTable.Add(210, "“");
    this.m_macEncodeTable.Add(211, "”");
    this.m_macEncodeTable.Add(212, "‘");
    this.m_macEncodeTable.Add(213, "’");
    this.m_macEncodeTable.Add(214, "÷");
    this.m_macEncodeTable.Add(215, "◊");
    this.m_macEncodeTable.Add(216, "ÿ");
    this.m_macEncodeTable.Add(217, "Ÿ");
    this.m_macEncodeTable.Add(218, "⁄");
    this.m_macEncodeTable.Add(219, "€");
    this.m_macEncodeTable.Add(220, "‹");
    this.m_macEncodeTable.Add(221, "›");
    this.m_macEncodeTable.Add(222, "ﬁ");
    this.m_macEncodeTable.Add(223, "ﬂ");
    this.m_macEncodeTable.Add(224 /*0xE0*/, "‡");
    this.m_macEncodeTable.Add(225, "·");
    this.m_macEncodeTable.Add(226, ",");
    this.m_macEncodeTable.Add(227, "„");
    this.m_macEncodeTable.Add(228, "‰");
    this.m_macEncodeTable.Add(229, "Â");
    this.m_macEncodeTable.Add(230, "Ê");
    this.m_macEncodeTable.Add(231, "Á");
    this.m_macEncodeTable.Add(232, "Ë");
    this.m_macEncodeTable.Add(233, "È");
    this.m_macEncodeTable.Add(234, "Í");
    this.m_macEncodeTable.Add(235, "Î");
    this.m_macEncodeTable.Add(236, "Ï");
    this.m_macEncodeTable.Add(237, "Ì");
    this.m_macEncodeTable.Add(238, "Ó");
    this.m_macEncodeTable.Add(239, "Ô");
    this.m_macEncodeTable.Add(240 /*0xF0*/, "\uF8FF");
    this.m_macEncodeTable.Add(241, "Ò");
    this.m_macEncodeTable.Add(242, "Ú");
    this.m_macEncodeTable.Add(243, "Û");
    this.m_macEncodeTable.Add(244, "Ù");
    this.m_macEncodeTable.Add(245, "ı");
    this.m_macEncodeTable.Add(246, "ˆ");
    this.m_macEncodeTable.Add(247, "˜");
    this.m_macEncodeTable.Add(248, "¯");
    this.m_macEncodeTable.Add(249, "˘");
    this.m_macEncodeTable.Add(250, "˙");
    this.m_macEncodeTable.Add(251, "˚");
    this.m_macEncodeTable.Add(252, "¸");
    this.m_macEncodeTable.Add(253, "˝");
    this.m_macEncodeTable.Add(254, "˛");
    this.m_macEncodeTable.Add((int) byte.MaxValue, "ˇ");
  }
}
