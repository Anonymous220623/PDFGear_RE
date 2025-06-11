// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.CharacterSetTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Character sets for the font</summary>
public enum CharacterSetTypes
{
  /// <summary>ANSI character set</summary>
  FXFONT_ANSI_CHARSET = 0,
  /// <summary>Default character set</summary>
  FXFONT_DEFAULT_CHARSET = 1,
  /// <summary>Symbol character set</summary>
  FXFONT_SYMBOL_CHARSET = 2,
  /// <summary>Shiftjis character set</summary>
  FXFONT_SHIFTJIS_CHARSET = 128, // 0x00000080
  /// <summary>Hangeul character set</summary>
  FXFONT_HANGEUL_CHARSET = 129, // 0x00000081
  /// <summary>GB-2312 Character set</summary>
  FXFONT_GB2312_CHARSET = 134, // 0x00000086
  /// <summary>Chines big5 character set</summary>
  FXFONT_CHINESEBIG5_CHARSET = 136, // 0x00000088
}
