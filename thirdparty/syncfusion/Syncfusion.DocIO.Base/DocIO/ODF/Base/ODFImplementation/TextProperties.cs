// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.TextProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class TextProperties
{
  private const int FontReliefKey = 0;
  private const int TextDisplayKey = 1;
  private const int TextConditionKey = 2;
  private const int UseWindowFontColorKey = 3;
  private const int TextUnderlineWidthKey = 4;
  private const int TextUnderlineTypeKey = 5;
  private const int TextUnderlineStyleKey = 6;
  private const int TextUnderlineModeKey = 7;
  private const int TextUnderlineColorKey = 8;
  private const int TextScalingKey = 9;
  private const int TextRotationScaleKey = 10;
  private const int TextOverlineWidthKey = 11;
  private const int TextOverlineTypeKey = 12;
  private const int TextOverlineStyleKey = 13;
  private const int TextOverlineModeKey = 14;
  private const int TextOverlineColorKey = 15;
  private const int FontNameKey = 16 /*0x10*/;
  private const int FontSizeKey = 17;
  private const int TextRotationAngleKey = 18;
  private const int TextScaleKey = 19;
  private const int BackgroundColorKey = 20;
  private const int TextPositionKey = 21;
  private const int FontWeightKey = 22;
  private const int ColorKey = 23;
  private const int CountryCodeKey = 24;
  private const int FontFamilyKey = 25;
  private const int FontStyleKey = 26;
  private const int IsTextDisplayKey = 27;
  private const int FontVariantKey = 28;
  private const int HyphenateKey = 29;
  private const int HyphenationPushCharCountKey = 30;
  private const int HyphenationRemainCharCountKey = 31 /*0x1F*/;
  private const int LanguageKey = 0;
  private const int LetterSpacingKey = 1;
  private const int ShadowKey = 2;
  private const int TextTransformKey = 3;
  private const int CountryAsianKey = 4;
  private const int CountryComplexKey = 5;
  private const int FontCharsetKey = 6;
  private const int FontCharsetAsianKey = 7;
  private const int FontCharsetComplexKey = 8;
  private const int FontFamliyComplexKey = 9;
  private const int FontFamilyAsianKey = 10;
  private const int FontFamilyGenericKey = 11;
  private const int FontFamilyGenericAsianKey = 12;
  private const int FontFamilyGenericComplexKey = 13;
  private const int FontNameComplexKey = 14;
  private const int FontNameAsianKey = 15;
  private const int FontPitchKey = 16 /*0x10*/;
  private const int FontPitchComplexKey = 17;
  private const int FontPitchAsianKey = 18;
  private const int FontSizeRelComplexKey = 19;
  private const int FontSizeRelAsianKey = 20;
  private const int FontSizeRelKey = 21;
  private const int FontStyleComplexKey = 22;
  private const int FontStyleAsianKey = 23;
  private const int FontStyleNameKey = 24;
  private const int FontStyleNameAsianKey = 25;
  private const int FontStyleNameComplexKey = 26;
  private const int FontWeightAsianKey = 27;
  private const int FontWeightComplexKey = 28;
  private const int RfcLanguageTagComplexKey = 29;
  private const int RfcLanguageTagAsianKey = 30;
  private const int RfcLanguageTagKey = 31 /*0x1F*/;
  private const int LetterKerningKey = 0;
  private const int LanguageComplexKey = 1;
  private const int LanguageAsianKey = 2;
  private const int TextCombineStartCharKey = 3;
  private const int TextCombineEndCharKey = 4;
  private const int TextCombineKey = 5;
  private const int TextBlinkingKey = 6;
  private const int TextOutlineKey = 7;
  private const int LinethroughWidthKey = 8;
  private const int LinethroughTypeKey = 9;
  private const int LinethroughTextStyleKey = 10;
  private const int LinethroughTextKey = 11;
  private const byte LinethroughStyleKey = 12;
  private const int LinethroughModeKey = 13;
  private const int LinethroughColorKey = 14;
  private const int TextEmphasizeKey = 15;
  private string m_fontFamily;
  private string m_fontName;
  private double m_fontSize;
  private FontWeight m_fontWeight;
  private int m_textRotationAngle;
  private float m_textScale;
  private Color m_backgroundColor;
  private Color m_color;
  private string m_countryCode;
  private string m_textPosition;
  private ODFFontStyle m_fontStyle;
  private FontVariant m_fontVariant;
  private bool m_hyphenate;
  private int m_hyphenation_push_char_count;
  private int m_hyphenation_remain_char_count;
  private string m_language;
  private float m_letterSpacing;
  private bool m_shadow;
  private Transform m_textTransform;
  private string m_countryAsian;
  private string m_countryComplex;
  private string m_font_charset;
  private string m_font_charset_asian;
  private string m_font_charset_complex;
  private string m_fontFamilyAsian;
  private string m_fontFamliyComplex;
  private FontFamilyGeneric m_fontFamilyGeneric;
  private FontFamilyGeneric m_fontFamilyGenericComplex;
  private FontFamilyGeneric m_fontFamilyGenericAsian;
  private string m_fontNameAsian;
  private string m_fontNameComplex;
  private FontPitch m_fontPitch;
  private FontPitch m_fontPitchAsian;
  private FontPitch m_fontPitchComplex;
  private int m_fontSizeRel;
  private int m_fontSizeRelAsian;
  private int m_fontSizeRelComplex;
  private bool m_fontStyleAsian;
  private bool m_fontStyleComplex;
  private string m_fontStyleName;
  private string m_fontStyleNameComplex;
  private string m_fontStyleNameAsian;
  private FontWeight m_fontWeightComplex;
  private FontWeight m_fontWeightAsian;
  private string m_languageAsian;
  private string m_languageComplex;
  private bool m_letterKerning;
  private string m_rfcLanguageTag;
  private string m_rfcLanguageTagAsian;
  private string m_rfcLanguageTagComplex;
  private bool m_textBlinking;
  private Combine m_textCombine;
  private char m_textCombineEndChar;
  private char m_textCombineStartChar;
  private Emphasize m_textEmphasize;
  private string m_linethroughColor;
  private LineMode m_linethroughMode;
  private BorderLineStyle m_linethroughStyle;
  private string m_linethroughText;
  private LineType m_linethroughTextStyle;
  private LineType m_linethroughType;
  private LineWidth m_linethroughWidth;
  private bool m_textOutline;
  private string m_textOverlineColor;
  private LineMode m_textOverlineMode;
  private BorderLineStyle m_textOverlineStyle;
  private LineType m_textOverlineType;
  private LineWidth m_textOverlineWidth;
  private TextRotationScale m_textRotationScale;
  private int m_textScaling;
  private string m_textUnderlineColor;
  private LineMode m_textUnderlineMode;
  private BorderLineStyle m_textUnderlineStyle;
  private LineType m_textUnderlineType;
  private LineWidth m_textUnderlineWidth;
  private bool m_useWindowFontColor;
  private string m_textCondition;
  private TextDisplay m_textDisplay;
  private bool m_isTextDisplay;
  private FontRelief m_fontRelief;
  private string m_charStyleName;
  internal int m_textFlag1;
  internal int m_textFlag2;
  internal int m_textFlag3;

  internal string CharStyleName
  {
    get => this.m_charStyleName;
    set => this.m_charStyleName = value;
  }

  internal FontRelief FontRelief
  {
    get => this.m_fontRelief;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294967294L | 1L);
      this.m_fontRelief = value;
    }
  }

  public TextDisplay TextDisplay
  {
    get => this.m_textDisplay;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294967293L | 2L);
      this.m_textDisplay = value;
    }
  }

  internal string TextCondition
  {
    get => this.m_textCondition;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294967291L | 4L);
      this.m_textCondition = value;
    }
  }

  internal bool UseWindowFontColor
  {
    get => this.m_useWindowFontColor;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294967287L | 8L);
      this.m_useWindowFontColor = value;
    }
  }

  internal LineWidth TextUnderlineWidth
  {
    get => this.m_textUnderlineWidth;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294967279L | 16L /*0x10*/);
      this.m_textUnderlineWidth = value;
    }
  }

  internal LineType TextUnderlineType
  {
    get => this.m_textUnderlineType;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294967263L | 32L /*0x20*/);
      this.m_textUnderlineType = value;
    }
  }

  internal BorderLineStyle TextUnderlineStyle
  {
    get => this.m_textUnderlineStyle;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294967231L | 64L /*0x40*/);
      this.m_textUnderlineStyle = value;
    }
  }

  internal LineMode TextUnderlineMode
  {
    get => this.m_textUnderlineMode;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294967167L | 128L /*0x80*/);
      this.m_textUnderlineMode = value;
    }
  }

  internal string TextUnderlineColor
  {
    get => this.m_textUnderlineColor;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294967039L | 256L /*0x0100*/);
      this.m_textUnderlineColor = value;
    }
  }

  internal int TextScaling
  {
    get => this.m_textScaling;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294966783L | 512L /*0x0200*/);
      this.m_textScaling = value;
    }
  }

  internal TextRotationScale TextRotationScale
  {
    get => this.m_textRotationScale;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294966271L | 1024L /*0x0400*/);
      this.m_textRotationScale = value;
    }
  }

  internal LineWidth TextOverlineWidth
  {
    get => this.m_textOverlineWidth;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294965247L | 2048L /*0x0800*/);
      this.m_textOverlineWidth = value;
    }
  }

  internal LineType TextOverlineType
  {
    get => this.m_textOverlineType;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294963199L | 4096L /*0x1000*/);
      this.m_textOverlineType = value;
    }
  }

  internal BorderLineStyle TextOverlineStyle
  {
    get => this.m_textOverlineStyle;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294959103L | 8192L /*0x2000*/);
      this.m_textOverlineStyle = value;
    }
  }

  internal LineMode TextOverlineMode
  {
    get => this.m_textOverlineMode;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294950911L | 16384L /*0x4000*/);
      this.m_textOverlineMode = value;
    }
  }

  public string TextOverlineColor
  {
    get => this.m_textOverlineColor;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294934527L | 32768L /*0x8000*/);
      this.m_textOverlineColor = value;
    }
  }

  internal string FontName
  {
    get => this.m_fontName;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294901759L | 65536L /*0x010000*/);
      this.m_fontName = value;
    }
  }

  internal double FontSize
  {
    get => this.m_fontSize;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294836223L | 131072L /*0x020000*/);
      this.m_fontSize = value;
    }
  }

  internal int TextRotationAngle
  {
    get => this.m_textRotationAngle;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294705151L | 262144L /*0x040000*/);
      this.m_textRotationAngle = value;
    }
  }

  internal float TextScale
  {
    get => this.m_textScale;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4294443007L | 524288L /*0x080000*/);
      this.m_textScale = value;
    }
  }

  internal Color BackgroundColor
  {
    get => this.m_backgroundColor;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4293918719L | 1048576L /*0x100000*/);
      this.m_backgroundColor = value;
    }
  }

  internal string TextPosition
  {
    get => this.m_textPosition;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4292870143L | 2097152L /*0x200000*/);
      this.m_textPosition = value;
    }
  }

  internal FontWeight FontWeight
  {
    get => this.m_fontWeight;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4290772991L | 4194304L /*0x400000*/);
      this.m_fontWeight = value;
    }
  }

  internal Color Color
  {
    get => this.m_color;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4286578687L | 8388608L /*0x800000*/);
      this.m_color = value;
    }
  }

  internal string CountryCode
  {
    get => this.m_countryCode;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4278190079L | 16777216L /*0x01000000*/);
      this.m_countryCode = value;
    }
  }

  internal string FontFamily
  {
    get => this.m_fontFamily;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4261412863L | 33554432L /*0x02000000*/);
      this.m_fontFamily = value;
    }
  }

  internal ODFFontStyle FontStyle
  {
    get => this.m_fontStyle;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4227858431L | 67108864L /*0x04000000*/);
      this.m_fontStyle = value;
    }
  }

  internal bool IsTextDisplay
  {
    get => this.m_isTextDisplay;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4160749567L | 134217728L /*0x08000000*/);
      this.m_isTextDisplay = value;
    }
  }

  internal FontVariant FontVariant
  {
    get => this.m_fontVariant;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 4026531839L /*0xEFFFFFFF*/ | 268435456L /*0x10000000*/);
      this.m_fontVariant = value;
    }
  }

  internal bool Hyphenate
  {
    get => this.m_hyphenate;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 3758096383L /*0xDFFFFFFF*/ | 536870912L /*0x20000000*/);
      this.m_hyphenate = value;
    }
  }

  internal int HyphenationPushCharCount
  {
    get => this.m_hyphenation_push_char_count;
    set
    {
      this.m_textFlag1 = (int) ((long) this.m_textFlag1 & 3221225471L /*0xBFFFFFFF*/ | 1073741824L /*0x40000000*/);
      this.m_hyphenation_push_char_count = value;
    }
  }

  internal int HyphenationRemainCharCount
  {
    get => this.m_hyphenation_remain_char_count;
    set
    {
      this.m_textFlag1 = this.m_textFlag1 & int.MaxValue | int.MinValue;
      this.m_hyphenation_remain_char_count = value;
    }
  }

  internal string Language
  {
    get => this.m_language;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294967294L | 1L);
      this.m_language = value;
    }
  }

  internal float LetterSpacing
  {
    get => this.m_letterSpacing;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294967293L | 2L);
      this.m_letterSpacing = value;
    }
  }

  internal bool Shadow
  {
    get => this.m_shadow;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294967291L | 4L);
      this.m_shadow = value;
    }
  }

  internal Transform TextTransform
  {
    get => this.m_textTransform;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294967287L | 8L);
      this.m_textTransform = value;
    }
  }

  internal string CountryAsian
  {
    get => this.m_countryAsian;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294967279L | 16L /*0x10*/);
      this.m_countryAsian = value;
    }
  }

  internal string CountryComplex
  {
    get => this.m_countryComplex;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294967263L | 32L /*0x20*/);
      this.m_countryComplex = value;
    }
  }

  internal string FontCharset
  {
    get => this.m_font_charset;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294967231L | 64L /*0x40*/);
      this.m_font_charset = value;
    }
  }

  internal string FontCharsetAsian
  {
    get => this.m_font_charset_asian;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294967167L | 128L /*0x80*/);
      this.m_font_charset_asian = value;
    }
  }

  internal string FontCharsetComplex
  {
    get => this.m_font_charset_complex;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294967039L | 256L /*0x0100*/);
      this.m_font_charset_complex = value;
    }
  }

  internal string FontFamliyComplex
  {
    get => this.m_fontFamliyComplex;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294966783L | 512L /*0x0200*/);
      this.m_fontFamliyComplex = value;
    }
  }

  internal string FontFamilyAsian
  {
    get => this.m_fontFamilyAsian;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294966271L | 1024L /*0x0400*/);
      this.m_fontFamilyAsian = value;
    }
  }

  internal FontFamilyGeneric FontFamilyGeneric
  {
    get => this.m_fontFamilyGeneric;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294965247L | 2048L /*0x0800*/);
      this.m_fontFamilyGeneric = value;
    }
  }

  internal FontFamilyGeneric FontFamilyGenericAsian
  {
    get => this.m_fontFamilyGenericAsian;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294963199L | 4096L /*0x1000*/);
      this.m_fontFamilyGenericAsian = value;
    }
  }

  internal FontFamilyGeneric FontFamilyGenericComplex
  {
    get => this.m_fontFamilyGenericComplex;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294959103L | 8192L /*0x2000*/);
      this.m_fontFamilyGenericComplex = value;
    }
  }

  internal string FontNameComplex
  {
    get => this.m_fontNameComplex;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294950911L | 16384L /*0x4000*/);
      this.m_fontNameComplex = value;
    }
  }

  internal string FontNameAsian
  {
    get => this.m_fontNameAsian;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294934527L | 32768L /*0x8000*/);
      this.m_fontNameAsian = value;
    }
  }

  internal FontPitch FontPitch
  {
    get => this.m_fontPitch;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294901759L | 65536L /*0x010000*/);
      this.m_fontPitch = value;
    }
  }

  internal FontPitch FontPitchComplex
  {
    get => this.m_fontPitchComplex;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294836223L | 131072L /*0x020000*/);
      this.m_fontPitchComplex = value;
    }
  }

  internal FontPitch FontPitchAsian
  {
    get => this.m_fontPitchAsian;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294705151L | 262144L /*0x040000*/);
      this.m_fontPitchAsian = value;
    }
  }

  internal int FontSizeRelComplex
  {
    get => this.m_fontSizeRelComplex;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4294443007L | 524288L /*0x080000*/);
      this.m_fontSizeRelComplex = value;
    }
  }

  internal int FontSizeRelAsian
  {
    get => this.m_fontSizeRelAsian;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4293918719L | 1048576L /*0x100000*/);
      this.m_fontSizeRelAsian = value;
    }
  }

  internal int FontSizeRel
  {
    get => this.m_fontSizeRel;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4292870143L | 2097152L /*0x200000*/);
      this.m_fontSizeRel = value;
    }
  }

  internal bool FontStyleComplex
  {
    get => this.m_fontStyleComplex;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4290772991L | 4194304L /*0x400000*/);
      this.m_fontStyleComplex = value;
    }
  }

  internal bool FontStyleAsian
  {
    get => this.m_fontStyleAsian;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4286578687L | 8388608L /*0x800000*/);
      this.m_fontStyleAsian = value;
    }
  }

  internal string FontStyleName
  {
    get => this.m_fontStyleName;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4278190079L | 16777216L /*0x01000000*/);
      this.m_fontStyleName = value;
    }
  }

  internal string FontStyleNameAsian
  {
    get => this.m_fontStyleNameAsian;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4261412863L | 33554432L /*0x02000000*/);
      this.m_fontStyleNameAsian = value;
    }
  }

  internal string FontStyleNameComplex
  {
    get => this.m_fontStyleNameComplex;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4227858431L | 67108864L /*0x04000000*/);
      this.m_fontStyleNameComplex = value;
    }
  }

  internal FontWeight FontWeightAsian
  {
    get => this.m_fontWeightAsian;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4160749567L | 134217728L /*0x08000000*/);
      this.m_fontWeightAsian = value;
    }
  }

  internal FontWeight FontWeightComplex
  {
    get => this.m_fontWeightComplex;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 4026531839L /*0xEFFFFFFF*/ | 268435456L /*0x10000000*/);
      this.m_fontWeightComplex = value;
    }
  }

  internal string RfcLanguageTagComplex
  {
    get => this.m_rfcLanguageTagComplex;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 3758096383L /*0xDFFFFFFF*/ | 536870912L /*0x20000000*/);
      this.m_rfcLanguageTagComplex = value;
    }
  }

  internal string RfcLanguageTagAsian
  {
    get => this.m_rfcLanguageTagAsian;
    set
    {
      this.m_textFlag2 = (int) ((long) this.m_textFlag2 & 3221225471L /*0xBFFFFFFF*/ | 1073741824L /*0x40000000*/);
      this.m_rfcLanguageTagAsian = value;
    }
  }

  internal string RfcLanguageTag
  {
    get => this.m_rfcLanguageTag;
    set
    {
      this.m_textFlag2 = this.m_textFlag2 & int.MaxValue | int.MinValue;
      this.m_rfcLanguageTag = value;
    }
  }

  internal bool LetterKerning
  {
    get => this.m_letterKerning;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 65534 | 1;
      this.m_letterKerning = value;
    }
  }

  internal string LanguageComplex
  {
    get => this.m_languageComplex;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 65533 | 2;
      this.m_languageComplex = value;
    }
  }

  internal string LanguageAsian
  {
    get => this.m_languageAsian;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 65531 | 4;
      this.m_languageAsian = value;
    }
  }

  internal char TextCombineStartChar
  {
    get => this.m_textCombineStartChar;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 65527 | 8;
      this.m_textCombineStartChar = value;
    }
  }

  internal char TextCombineEndChar
  {
    get => this.m_textCombineEndChar;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 65519 | 16 /*0x10*/;
      this.m_textCombineEndChar = value;
    }
  }

  internal Combine TextCombine
  {
    get => this.m_textCombine;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 65503 | 32 /*0x20*/;
      this.m_textCombine = value;
    }
  }

  internal bool TextBlinking
  {
    get => this.m_textBlinking;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 65471 | 64 /*0x40*/;
      this.m_textBlinking = value;
    }
  }

  internal bool TextOutline
  {
    get => this.m_textOutline;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 65407 | 128 /*0x80*/;
      this.m_textOutline = value;
    }
  }

  internal LineWidth LinethroughWidth
  {
    get => this.m_linethroughWidth;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 65279 | 256 /*0x0100*/;
      this.m_linethroughWidth = value;
    }
  }

  internal LineType LinethroughType
  {
    get => this.m_linethroughType;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 65023 | 512 /*0x0200*/;
      this.m_linethroughType = value;
    }
  }

  internal LineType LinethroughTextStyle
  {
    get => this.m_linethroughTextStyle;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 64511 | 512 /*0x0200*/;
      this.m_linethroughTextStyle = value;
    }
  }

  internal string LinethroughText
  {
    get => this.m_linethroughText;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 63487 | 1024 /*0x0400*/;
      this.m_linethroughText = value;
    }
  }

  internal BorderLineStyle LinethroughStyle
  {
    get => this.m_linethroughStyle;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 61439 /*0xEFFF*/ | 2048 /*0x0800*/;
      this.m_linethroughStyle = value;
    }
  }

  internal LineMode LinethroughMode
  {
    get => this.m_linethroughMode;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 57343 /*0xDFFF*/ | 4096 /*0x1000*/;
      this.m_linethroughMode = value;
    }
  }

  internal string LinethroughColor
  {
    get => this.m_linethroughColor;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & 49151 /*0xBFFF*/ | 8192 /*0x2000*/;
      this.m_linethroughColor = value;
    }
  }

  internal Emphasize TextEmphasize
  {
    get => this.m_textEmphasize;
    set
    {
      this.m_textFlag3 = this.m_textFlag3 & (int) short.MaxValue | 16384 /*0x4000*/;
      this.m_textEmphasize = value;
    }
  }

  internal bool HasKey(int propertyKey, int flagname)
  {
    return (flagname & (int) Math.Pow(2.0, (double) propertyKey)) >> propertyKey != 0;
  }

  public override bool Equals(object obj)
  {
    TextProperties textProperties = obj as TextProperties;
    bool flag = false;
    if (textProperties == null)
      return false;
    if (this.HasKey(20, this.m_textFlag1) && textProperties.HasKey(20, this.m_textFlag1))
      flag = this.BackgroundColor.Equals((object) textProperties.BackgroundColor);
    if (this.HasKey(23, this.m_textFlag1) && textProperties.HasKey(23, this.m_textFlag1))
      flag = this.Color.Equals((object) textProperties.Color);
    if (this.HasKey(4, this.m_textFlag2) && textProperties.HasKey(4, this.m_textFlag2))
      flag = this.CountryAsian.Equals(textProperties.CountryAsian);
    if (this.HasKey(24, this.m_textFlag2) && textProperties.HasKey(24, this.m_textFlag2))
      flag = this.CountryCode.Equals(textProperties.CountryCode);
    if (this.HasKey(5, this.m_textFlag2) && textProperties.HasKey(5, this.m_textFlag2))
      flag = this.CountryComplex.Equals(textProperties.CountryComplex);
    if (this.HasKey(6, this.m_textFlag2) && textProperties.HasKey(6, this.m_textFlag2))
      flag = this.FontCharset.Equals(textProperties.FontCharset);
    if (this.HasKey(7, this.m_textFlag2) && textProperties.HasKey(7, this.m_textFlag2))
      flag = this.FontCharsetAsian.Equals(textProperties.FontCharsetAsian);
    if (this.HasKey(8, this.m_textFlag2) && textProperties.HasKey(8, this.m_textFlag2))
      flag = this.FontCharsetComplex.Equals(textProperties.FontCharsetComplex);
    if (this.HasKey(16 /*0x10*/, this.m_textFlag1) && textProperties.HasKey(16 /*0x10*/, this.m_textFlag1))
      flag = this.FontName.Equals(textProperties.FontName);
    if (this.HasKey(15, this.m_textFlag2) && textProperties.HasKey(15, this.m_textFlag2))
      flag = this.FontNameAsian.Equals(textProperties.FontNameAsian);
    if (this.HasKey(14, this.m_textFlag2) && textProperties.HasKey(14, this.m_textFlag2))
      flag = this.FontNameComplex.Equals(textProperties.FontNameComplex);
    if (this.HasKey(16 /*0x10*/, this.m_textFlag2) && textProperties.HasKey(16 /*0x10*/, this.m_textFlag2))
      flag = this.FontPitch.Equals((object) textProperties.FontPitch);
    if (this.HasKey(0, this.m_textFlag1) && textProperties.HasKey(0, this.m_textFlag1))
      flag = this.FontRelief.Equals((object) textProperties.FontRelief);
    if (this.HasKey(17, this.m_textFlag1) && textProperties.HasKey(17, this.m_textFlag1))
      flag = this.FontSize.Equals(textProperties.FontSize);
    if (this.HasKey(21, this.m_textFlag2) && textProperties.HasKey(21, this.m_textFlag2))
      flag = this.FontSizeRel.Equals(textProperties.FontSizeRel);
    if (this.HasKey(26, this.m_textFlag2) && textProperties.HasKey(26, this.m_textFlag2))
      flag = this.FontStyle.Equals((object) textProperties.FontStyle);
    if (this.HasKey(24, this.m_textFlag2) && textProperties.HasKey(24, this.m_textFlag2))
      flag = this.FontStyleName.Equals(textProperties.FontStyleName);
    if (this.HasKey(28, this.m_textFlag1) && textProperties.HasKey(28, this.m_textFlag1))
      flag = this.FontVariant.Equals((object) textProperties.FontVariant);
    if (this.HasKey(22, this.m_textFlag1) && textProperties.HasKey(22, this.m_textFlag1))
      flag = this.FontWeight.Equals((object) textProperties.FontWeight);
    if (this.HasKey(29, this.m_textFlag1) && textProperties.HasKey(29, this.m_textFlag1))
      flag = this.Hyphenate.Equals(textProperties.Hyphenate);
    if (this.HasKey(30, this.m_textFlag1) && textProperties.HasKey(30, this.m_textFlag1))
      flag = this.HyphenationPushCharCount.Equals(textProperties.HyphenationPushCharCount);
    if (this.HasKey(31 /*0x1F*/, this.m_textFlag1) && textProperties.HasKey(31 /*0x1F*/, this.m_textFlag1))
      flag = this.HyphenationRemainCharCount.Equals(textProperties.HyphenationRemainCharCount);
    if (this.HasKey(27, this.m_textFlag1) && textProperties.HasKey(27, this.m_textFlag1))
      flag = this.IsTextDisplay.Equals(textProperties.IsTextDisplay);
    if (this.HasKey(0, this.m_textFlag2) && textProperties.HasKey(0, this.m_textFlag2))
      flag = this.Language.Equals(textProperties.Language);
    if (this.HasKey(0, this.m_textFlag3) && textProperties.HasKey(0, this.m_textFlag3))
      flag = this.LetterKerning.Equals(textProperties.LetterKerning);
    if (this.HasKey(1, this.m_textFlag3) && textProperties.HasKey(1, this.m_textFlag3))
      flag = this.LanguageComplex.Equals(textProperties.LanguageComplex);
    if (this.HasKey(1, this.m_textFlag2) && textProperties.HasKey(1, this.m_textFlag2))
      flag = this.LetterSpacing.Equals(textProperties.LetterSpacing);
    if (this.HasKey(14, this.m_textFlag3) && textProperties.HasKey(14, this.m_textFlag3))
      flag = this.LinethroughColor.Equals(textProperties.LinethroughColor);
    if (this.HasKey(13, this.m_textFlag3) && textProperties.HasKey(13, this.m_textFlag3))
      flag = this.LinethroughMode.Equals((object) textProperties.LinethroughMode);
    if (this.HasKey(12, this.m_textFlag3) && textProperties.HasKey(12, this.m_textFlag3))
      flag = this.LinethroughStyle.Equals((object) textProperties.LinethroughStyle);
    if (this.HasKey(10, this.m_textFlag3) && textProperties.HasKey(10, this.m_textFlag3))
      flag = this.LinethroughTextStyle.Equals((object) textProperties.LinethroughTextStyle);
    if (this.HasKey(9, this.m_textFlag3) && textProperties.HasKey(9, this.m_textFlag3))
      flag = this.LinethroughType.Equals((object) textProperties.LinethroughType);
    if (this.HasKey(8, this.m_textFlag3) && textProperties.HasKey(8, this.m_textFlag3))
      flag = this.LinethroughWidth.Equals((object) textProperties.LinethroughWidth);
    if (this.HasKey(31 /*0x1F*/, this.m_textFlag2) && textProperties.HasKey(31 /*0x1F*/, this.m_textFlag2))
      flag = this.RfcLanguageTag.Equals(textProperties.RfcLanguageTag);
    if (this.HasKey(30, this.m_textFlag2) && textProperties.HasKey(30, this.m_textFlag2))
      flag = this.RfcLanguageTagAsian.Equals(textProperties.RfcLanguageTagAsian);
    if (this.HasKey(29, this.m_textFlag2) && textProperties.HasKey(29, this.m_textFlag2))
      flag = this.RfcLanguageTagComplex.Equals(textProperties.RfcLanguageTagComplex);
    if (this.HasKey(2, this.m_textFlag2) && textProperties.HasKey(2, this.m_textFlag2))
      flag = this.Shadow.Equals(textProperties.Shadow);
    if (this.HasKey(6, this.m_textFlag3) && textProperties.HasKey(6, this.m_textFlag3))
      flag = this.TextBlinking.Equals(textProperties.TextBlinking);
    if (this.HasKey(5, this.m_textFlag3) && textProperties.HasKey(5, this.m_textFlag3))
      flag = this.TextCombine.Equals((object) textProperties.TextCombine);
    if (this.HasKey(4, this.m_textFlag3) && textProperties.HasKey(4, this.m_textFlag3))
      flag = this.TextCombineEndChar.Equals(textProperties.TextCombineEndChar);
    if (this.HasKey(3, this.m_textFlag3) && textProperties.HasKey(3, this.m_textFlag3))
      flag = this.TextCombineStartChar.Equals(textProperties.TextCombineStartChar);
    if (this.HasKey(2, this.m_textFlag1) && textProperties.HasKey(2, this.m_textFlag1))
      flag = this.TextCondition.Equals(textProperties.TextCondition);
    if (this.HasKey(1, this.m_textFlag1) && textProperties.HasKey(1, this.m_textFlag1))
      flag = this.TextDisplay.Equals((object) textProperties.TextDisplay);
    if (this.HasKey(15, this.m_textFlag3) && textProperties.HasKey(15, this.m_textFlag3))
      flag = this.TextEmphasize.Equals((object) textProperties.TextEmphasize);
    if (this.HasKey(7, this.m_textFlag3) && textProperties.HasKey(7, this.m_textFlag3))
      flag = this.TextOutline.Equals(textProperties.TextOutline);
    if (this.HasKey(15, this.m_textFlag1) && textProperties.HasKey(15, this.m_textFlag1))
      flag = this.TextOverlineColor.Equals(textProperties.TextOverlineColor);
    if (this.HasKey(14, this.m_textFlag1) && textProperties.HasKey(14, this.m_textFlag1))
      flag = this.TextOverlineMode.Equals((object) textProperties.TextOverlineMode);
    if (this.HasKey(13, this.m_textFlag1) && textProperties.HasKey(13, this.m_textFlag1))
      flag = this.TextOverlineStyle.Equals((object) textProperties.TextOverlineStyle);
    if (this.HasKey(12, this.m_textFlag1) && textProperties.HasKey(12, this.m_textFlag1))
      flag = this.TextOverlineType.Equals((object) textProperties.TextOverlineType);
    if (this.HasKey(11, this.m_textFlag1) && textProperties.HasKey(11, this.m_textFlag1))
      flag = this.TextOverlineWidth.Equals((object) textProperties.TextOverlineWidth);
    if (this.HasKey(21, this.m_textFlag1) && textProperties.HasKey(21, this.m_textFlag1))
      flag = this.TextPosition.Equals(textProperties.TextPosition);
    if (this.HasKey(18, this.m_textFlag1) && textProperties.HasKey(18, this.m_textFlag1))
      flag = this.TextRotationAngle.Equals(textProperties.TextRotationAngle);
    if (this.HasKey(10, this.m_textFlag1) && textProperties.HasKey(10, this.m_textFlag1))
      flag = this.TextRotationScale.Equals((object) textProperties.TextRotationScale);
    if (this.HasKey(19, this.m_textFlag1) && textProperties.HasKey(19, this.m_textFlag1))
      flag = this.TextScale.Equals(textProperties.TextScale);
    if (this.HasKey(9, this.m_textFlag1) && textProperties.HasKey(9, this.m_textFlag1))
      flag = this.TextScaling.Equals(textProperties.TextScaling);
    if (this.HasKey(3, this.m_textFlag2) && textProperties.HasKey(3, this.m_textFlag2))
      flag = this.TextTransform.Equals((object) textProperties.TextTransform);
    if (this.HasKey(8, this.m_textFlag1) && textProperties.HasKey(8, this.m_textFlag1))
      flag = this.TextUnderlineColor.Equals(textProperties.TextUnderlineColor);
    if (this.HasKey(7, this.m_textFlag1) && textProperties.HasKey(7, this.m_textFlag1))
      flag = this.TextUnderlineMode.Equals((object) textProperties.TextUnderlineMode);
    if (this.HasKey(6, this.m_textFlag1) && textProperties.HasKey(6, this.m_textFlag1))
      flag = this.TextUnderlineStyle.Equals((object) textProperties.TextUnderlineStyle);
    if (this.HasKey(5, this.m_textFlag1) && textProperties.HasKey(5, this.m_textFlag1))
      flag = this.TextUnderlineType.Equals((object) textProperties.TextUnderlineType);
    if (this.HasKey(4, this.m_textFlag1) && textProperties.HasKey(4, this.m_textFlag1))
      flag = this.TextUnderlineWidth.Equals((object) textProperties.TextUnderlineWidth);
    if (this.HasKey(3, this.m_textFlag1) && textProperties.HasKey(3, this.m_textFlag1))
      flag = this.UseWindowFontColor.Equals(textProperties.UseWindowFontColor);
    return flag;
  }
}
