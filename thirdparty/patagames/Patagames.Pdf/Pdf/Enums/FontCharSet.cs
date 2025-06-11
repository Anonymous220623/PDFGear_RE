// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FontCharSet
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>The character set.</summary>
public enum FontCharSet : byte
{
  /// <summary>Specifies 1252 Latin 1 (ANSI) character set.</summary>
  ANSI_CHARSET = 0,
  /// <summary>
  /// Specifies a character set based on the current system locale.
  /// </summary>
  DEFAULT_CHARSET = 1,
  /// <summary>Specifies symbolyc character set.</summary>
  SYMBOL_CHARSET = 2,
  /// <summary>Specifies macintosh character set.</summary>
  MAC_CHARSET = 77, // 0x4D
  /// <summary>Specifies 932 (DBCS. Japanese) character set.</summary>
  SHIFTJIS_CHARSET = 128, // 0x80
  /// <summary>Specifies 949 (DBCS, Korean) character set.</summary>
  HANGEUL_CHARSET = 129, // 0x81
  /// <summary>Specifies 949 (DBCS, Korean) character set.</summary>
  HANGUL_CHARSET = 129, // 0x81
  /// <summary>Specifies 1361 (DBCS, Korean) character set.</summary>
  JOHAB_CHARSET = 130, // 0x82
  /// <summary>
  /// Specifies 936 (DBCS, Simplified Chinese) character set.
  /// </summary>
  GB2312_CHARSET = 134, // 0x86
  /// <summary>
  /// Specifies 950 (DBCS, Traditional Chinese) character set.
  /// </summary>
  CHINESEBIG5_CHARSET = 136, // 0x88
  /// <summary>Specifies 1253 Greek character set.</summary>
  GREEK_CHARSET = 161, // 0xA1
  /// <summary>Specifies 1254 Latin 5 (Turkish) character set.</summary>
  TURKISH_CHARSET = 162, // 0xA2
  /// <summary>Specifies 1258 Vietnamese character set.</summary>
  VIETNAMESE_CHARSET = 163, // 0xA3
  /// <summary>Specifies 1255 Hebrew character set.</summary>
  HEBREW_CHARSET = 177, // 0xB1
  /// <summary>Specifies 1256 Arabic character set.</summary>
  ARABIC_CHARSET = 178, // 0xB2
  /// <summary>Specifies 1257 Baltic Rim character set.</summary>
  BALTIC_CHARSET = 186, // 0xBA
  /// <summary>Specifies 1251 Cyrillic (Slavic) character set.</summary>
  RUSSIAN_CHARSET = 204, // 0xCC
  /// <summary>Specifies 874 Thai character set.</summary>
  THAI_CHARSET = 222, // 0xDE
  /// <summary>
  /// Specifies 1250 Latin 2 (Central Europe) character set.
  /// </summary>
  EASTEUROPE_CHARSET = 238, // 0xEE
  /// <summary>
  /// Specifies a character set that is operating-system dependent.
  /// </summary>
  UNICODE_CHARSET = 255, // 0xFF
}
