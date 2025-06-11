// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.LineJoin
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Represents line join styles. The line join style specifies the shape to be used at the corners of paths that are stroked.
/// </summary>
/// <remarks>
///  Join styles are significant only at points where consecutive segments of a path connect at an angle; segments that
///  meet or intersect fortuitously receive no special treatment.
/// <para><strong>TABLE 4.5 Line join styles</strong></para>
///  <list type="table">
///  <listheader>
///  <term>Style</term><term>Appearance</term><description>Description</description>
///  </listheader>
///  <item><term>Miter</term><term><img src="../Media/LineJoinStyle0.png" /></term><description>The outer edges of the strokes for the two segments are extended until they meet at an angle, as in a picture frame. If the segments meet at too sharp an angle (as defined by the miter limit parameter), a bevel join is used instead.</description></item>
///  <item><term>Round</term><term><img src="../Media/LineJoinStyle1.png" /></term><description>An arc of a circle with a diameter equal to the line width is drawn around the point where the two segments meet, connecting the outer edges of the strokes for the two segments.This pieslice-shaped figure is filled in, producing a rounded corner.</description></item>
///  <item><term>Bevel</term><term><img src="../Media/LineJoinStyle2.png" /></term><description>The two segments are finished with butt caps and the resulting notch beyond the ends of the segments is filled with a triangle.</description></item>
///  </list>
///  </remarks>
public enum LineJoin
{
  /// <summary>Miter join.</summary>
  Miter,
  /// <summary>Round join.</summary>
  Round,
  /// <summary>Bevel join.</summary>
  Bevel,
}
