// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.IconScaleTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>The type of scaling to use.</summary>
public enum IconScaleTypes
{
  /// <summary>
  /// Scale the icon to fill the annotation rectangle exactly, without regard to its original aspect ratio (ratio of width to height).
  /// </summary>
  [Description("A")] Anamorphic,
  /// <summary>
  /// Scale the icon to fit the width or height of the annotation rectangle while maintaining the icon’s original aspect ratio.
  /// If the required horizontal and vertical scaling factors are different, use the smaller of the two, centering the icon within the annotation rectangle in the other dimension.
  /// </summary>
  [Description("P")] Proportional,
}
