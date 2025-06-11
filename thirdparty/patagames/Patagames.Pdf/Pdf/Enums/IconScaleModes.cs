// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.IconScaleModes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// The circumstances under which the icon should be scaled inside the annotation rectangle
/// </summary>
public enum IconScaleModes
{
  /// <summary>Always scale.</summary>
  [Description("A")] Always,
  /// <summary>
  /// Scale only when the icon is bigger than the annotation rectangle.
  /// </summary>
  [Description("B")] Bigger,
  /// <summary>
  /// Scale only when the icon is smaller than the annotation rectangle.
  /// </summary>
  [Description("S")] Smaller,
  /// <summary>Never scale.</summary>
  [Description("N")] Never,
}
