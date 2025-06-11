// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.TextRenderingModes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Represents the text rendering mode, Tmode, determines whether showing text causes glyph
/// outlines to be stroked, filled, used as a clipping boundary, or some combination
/// of the three.
/// </summary>
/// <remarks>
/// <para>
/// Stroking, filling, and clipping have the same effects for a text object
/// as they do for a path object, although they are specified in an entirely different way.
/// The graphics state parameters affecting those operations, such as line
/// width, are interpreted in user space rather than in text space.
/// </para>
/// <note type="note">The text rendering mode has no effect on text displayed in a Type 3 font.</note>
/// </remarks>
public enum TextRenderingModes
{
  /// <summary>Fill text.</summary>
  Fill,
  /// <summary>Stroke text.</summary>
  Stroke,
  /// <summary>Fill, then stroke text.</summary>
  FillThenStroke,
  /// <summary>Neither fill nor stroke text (invisible).</summary>
  Nothing,
  /// <summary>Fill text and add to path for clipping.</summary>
  FillClip,
  /// <summary>Stroke text and add to path for clipping.</summary>
  StrokeClip,
  /// <summary>Fill, then stroke text and add to path for clipping.</summary>
  FillThenStrokeClip,
  /// <summary>Add text to path for clipping.</summary>
  Clipping,
}
