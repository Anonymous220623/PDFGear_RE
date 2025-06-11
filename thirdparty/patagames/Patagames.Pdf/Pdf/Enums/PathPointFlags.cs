// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.PathPointFlags
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represents the types of the point in the Path.</summary>
[Flags]
public enum PathPointFlags
{
  /// <summary>
  /// Specifies that the point is the last point in a closed subpath (figure).
  /// </summary>
  CloseFigure = 1,
  /// <summary>
  /// Indicates that the point is one of the two endpoints of a line.
  /// </summary>
  LineTo = 2,
  /// <summary>
  /// Indicates that the point is an endpoint or control point of a cubic Bézier spline.
  /// </summary>
  BezierTo = 4,
  /// <summary>Starts path</summary>
  MoveTo = BezierTo | LineTo, // 0x00000006
}
