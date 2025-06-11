// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.PitchFamilyFlags
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Font pitch and family flags</summary>
[Flags]
public enum PitchFamilyFlags
{
  /// <summary>Fixed pitch</summary>
  FXFONT_FF_FIXEDPITCH = 1,
  /// <summary>Roman family</summary>
  FXFONT_FF_ROMAN = 16, // 0x00000010
  /// <summary>Script family</summary>
  FXFONT_FF_SCRIPT = 64, // 0x00000040
}
