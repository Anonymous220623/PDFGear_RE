// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FunctionTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>The function type.</summary>
public enum FunctionTypes
{
  /// <summary>Unknown function type</summary>
  Unknown = -1, // 0xFFFFFFFF
  /// <summary>Sampled function.</summary>
  Sampled = 0,
  /// <summary>Exponential interpolation function.</summary>
  Exponential = 2,
  /// <summary>Stitching function.</summary>
  Stitching = 3,
  /// <summary>PostScript calculator function.</summary>
  PostScript = 4,
}
