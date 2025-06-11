// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.LineEndingStyles
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represent the line enfing styles</summary>
/// <remarks>
/// <para><strong>TABLE 8.27 Line ending styles</strong></para>
///  <list type="table">
///  <listheader>
///  <term>Name</term><term>Appearance</term><description>Description</description>
///  </listheader>
///  <item><term>Square</term><term><img src="../Media/linending01.png" /></term><description>A square filled with the annotation’s interior color, if any</description></item>
///  <item><term>Circle</term><term><img src="../Media/linending02.png" /></term><description>A circle filled with the annotation’s interior color, if any</description></item>
///  <item><term>Diamond</term><term><img src="../Media/linending03.png" /></term><description>A diamond shape filled with the annotation’s interior color, if any</description></item>
///  <item><term>OpenArrow</term><term><img src="../Media/linending04.png" /></term><description>Two short lines meeting in an acute angle to form an open arrowhead</description></item>
///  <item><term>ClosedArrow</term><term><img src="../Media/linending05.png" /></term><description>Two short lines meeting in an acute angle as in the OpenArrow style (see above) and connected by a third line to form a triangular closed arrowhead filled with the annotation’s interior color, if any</description></item>
///  <item><term>None</term><term><img src="../Media/linending06.png" /></term><description>No line ending</description></item>
///  <item><term>Butt</term><term><img src="../Media/linending07.png" /></term><description>(PDF 1.5) A short line at the endpoint perpendicular to the line itself</description></item>
///  <item><term>ROpenArrow</term><term><img src="../Media/linending08.png" /></term><description>(PDF 1.5) Two short lines in the reverse direction from OpenArrow</description></item>
///  <item><term>RClosedArrow</term><term><img src="../Media/linending09.png" /></term><description>(PDF 1.5) A triangular closed arrowhead in the reverse direction from ClosedArrow</description></item>
///  <item><term>Slash</term><term><img src="../Media/linending10.png" /></term><description>(PDF 1.6) A short line at the endpoint approximately 30 degrees clockwise from perpendicular to the line itself</description></item>
///  </list>
///  </remarks>
public enum LineEndingStyles
{
  /// <summary>No line ending</summary>
  [Description("None")] None,
  /// <summary>
  /// A square filled with the annotation’s interior color, if any
  /// </summary>
  [Description("Square")] Square,
  /// <summary>
  /// A circle filled with the annotation’s interior color, if any
  /// </summary>
  [Description("Circle")] Circle,
  /// <summary>
  /// A diamond shape filled with the annotation’s interior color, if any
  /// </summary>
  [Description("Diamond")] Diamond,
  /// <summary>
  /// Two short lines meeting in an acute angle to form an open arrowhead
  /// </summary>
  [Description("OpenArrow")] OpenArrow,
  /// <summary>
  /// Two short lines meeting in an acute angle as in the OpenArrow style (see above) and connected by a third line to form a triangular closed arrowhead filled with the annotation’s interior color, if any
  /// </summary>
  [Description("ClosedArrow")] ClosedArrow,
  /// <summary>
  /// (PDF 1.5) A short line at the endpoint perpendicular to the line itself
  /// </summary>
  [Description("Butt")] Butt,
  /// <summary>
  /// (PDF 1.5) Two short lines in the reverse direction from OpenArrow
  /// </summary>
  [Description("ROpenArrow")] ROpenArrow,
  /// <summary>
  /// (PDF 1.5) A triangular closed arrowhead in the reverse direction from ClosedArrow
  /// </summary>
  [Description("RClosedArrow")] RClosedArrow,
  /// <summary>
  /// (PDF 1.6) A short line at the endpoint approximately 30 degrees clockwise from perpendicular to the line itself
  /// </summary>
  [Description("Slash")] Slash,
}
