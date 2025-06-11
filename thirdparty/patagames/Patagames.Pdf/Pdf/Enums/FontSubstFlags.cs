// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FontSubstFlags
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>FontSubstFlags</summary>
[Flags]
public enum FontSubstFlags
{
  /// <summary>FXFONT_SUBST_MM</summary>
  FXFONT_SUBST_MM = 1,
  /// <summary>FXFONT_SUBST_GLYPHPATH</summary>
  FXFONT_SUBST_GLYPHPATH = 4,
  /// <summary>FXFONT_SUBST_CLEARTYPE</summary>
  FXFONT_SUBST_CLEARTYPE = 8,
  /// <summary>FXFONT_SUBST_TRANSFORM</summary>
  FXFONT_SUBST_TRANSFORM = 16, // 0x00000010
  /// <summary>FXFONT_SUBST_NONSYMBOL</summary>
  FXFONT_SUBST_NONSYMBOL = 32, // 0x00000020
  /// <summary>FXFONT_SUBST_EXACT</summary>
  FXFONT_SUBST_EXACT = 64, // 0x00000040
  /// <summary>FXFONT_SUBST_STANDARD</summary>
  FXFONT_SUBST_STANDARD = 128, // 0x00000080
}
