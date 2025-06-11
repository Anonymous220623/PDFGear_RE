// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.BorderEffects
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represents the border effects</summary>
public enum BorderEffects
{
  /// <summary>
  /// No effect: the border is as described by the annotation dictionary’s BS entry.
  /// </summary>
  None,
  /// <summary>
  /// The border should appear “cloudy”. The width and dash array specified by BS are ignored.
  /// </summary>
  Cloudy,
}
