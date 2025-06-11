// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.SoundEncodingFormats
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represents the sound encoding formats.</summary>
public enum SoundEncodingFormats
{
  /// <summary>
  /// Unspecified or unsigned values in the range 0 to 2B − 1
  /// </summary>
  [Description("Raw")] Raw,
  /// <summary>Twos-complement values</summary>
  [Description("Signed")] Signed,
  /// <summary>μ-law–encoded samples</summary>
  [Description("muLaw")] muLaw,
  /// <summary>A-law–encoded samples</summary>
  [Description("ALaw")] ALaw,
}
