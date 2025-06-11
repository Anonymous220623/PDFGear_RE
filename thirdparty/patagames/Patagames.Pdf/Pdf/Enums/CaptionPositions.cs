// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.CaptionPositions
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Represents the position of the caption for the <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineAnnotation" />
/// </summary>
public enum CaptionPositions
{
  /// <summary>the caption will be centered inside the line.</summary>
  [Description("Inline")] Inline,
  /// <summary>the caption will be on top of the line</summary>
  [Description("Top")] Top,
}
