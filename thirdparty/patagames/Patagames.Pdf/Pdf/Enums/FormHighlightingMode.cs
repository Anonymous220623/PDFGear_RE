// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FormHighlightingMode
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// The annotation’s highlighting mode, the visual effect to be used when the mouse button is pressed or held down inside its active area
/// </summary>
public enum FormHighlightingMode
{
  /// <summary>No highlighting</summary>
  None,
  /// <summary>Invert the contents of the annotation rectangle</summary>
  [Description("I")] Invert,
  /// <summary>Invert the annotation’s border</summary>
  [Description("O")] Outline,
  /// <summary>
  /// Display the annotation as if it were being pushed below the surface of the page
  /// </summary>
  [Description("P")] Push,
  /// <summary>
  /// Same as <see cref="F:Patagames.Pdf.Enums.FormHighlightingMode.Push" /> (which is preferred)
  /// </summary>
  [Description("T")] Toggle,
}
