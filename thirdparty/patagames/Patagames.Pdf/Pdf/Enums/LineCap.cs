// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.LineCap
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Represents line cap styles. The line cap style specifies the shape to be used at the ends of open subpaths (and dashes, if any) when they are stroked.
/// </summary>
/// <remarks>
/// <para><strong>TABLE 4.4 Line cap styles</strong></para>
///  <list type="table">
///  <listheader>
///  <term>Style</term><term>Appearance</term><description>Description</description>
///  </listheader>
///  <item><term>Butt</term><term><img src="../Media/LineCapStyle0.png" /></term><description>The stroke is squared off at the endpoint of the path. There is no projection beyond the end of the path.</description></item>
///  <item><term>Round</term><term><img src="../Media/LineCapStyle1.png" /></term><description>A semicircular arc with a diameter equal to the line width is drawn around the endpoint and filled in.</description></item>
///  <item><term>Square</term><term><img src="../Media/LineCapStyle2.png" /></term>The stroke continues beyond the endpoint of the path for a distance equal to half the line width and is squared off.<description></description></item>
///  </list>
///  </remarks>
public enum LineCap
{
  /// <summary>Butt cap.</summary>
  Butt,
  /// <summary>Round cap.</summary>
  Round,
  /// <summary>Projecting square cap.</summary>
  Square,
}
