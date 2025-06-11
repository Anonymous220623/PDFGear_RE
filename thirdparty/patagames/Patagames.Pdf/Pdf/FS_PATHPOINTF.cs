// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FS_PATHPOINTF
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Represents a point of the path.</summary>
public struct FS_PATHPOINTF
{
  /// <summary>
  /// Gets or sets the x-coordinate of this <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />.
  /// </summary>
  public float X { get; set; }

  /// <summary>
  /// Gets or sets the y-coordinate of this <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />.
  /// </summary>
  public float Y { get; set; }

  /// <summary>Gets the type of the point in the path.</summary>
  public PathPointFlags Flags { get; set; }

  /// <summary>
  /// Specifies whether this <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> contains the same coordinates and flags as the specified object.
  /// </summary>
  /// <param name="obj">The System.Object to test.</param>
  /// <returns>This method returns true if obj is a <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> and has the same coordinates and flags as this <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />.</returns>
  public override bool Equals(object obj)
  {
    return obj is FS_PATHPOINTF fsPathpointf && this.Equals(fsPathpointf);
  }

  /// <summary>
  /// Specifies whether this <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> contains the same coordinates and flags as the specified point.
  /// </summary>
  /// <param name="obj">The <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> to test.</param>
  /// <returns>This method returns true if obj has the same coordinates and flags as this <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />.</returns>
  public bool Equals(FS_PATHPOINTF obj) => this == obj;

  /// <summary>
  /// Compares two <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> structures.
  /// </summary>
  /// <param name="left">A <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> to compare.</param>
  /// <param name="right">A <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> to compare.</param>
  /// <remarks>
  /// The result specifies whether the
  /// values of the <see cref="P:Patagames.Pdf.FS_PATHPOINTF.X" />, <see cref="P:Patagames.Pdf.FS_PATHPOINTF.Y" /> and <see cref="P:Patagames.Pdf.FS_PATHPOINTF.Flags" /> properties
  /// of the two <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> structures are equal.
  /// </remarks>
  /// <returns>
  /// true if the<see cref="P:Patagames.Pdf.FS_PATHPOINTF.X" />, <see cref="P:Patagames.Pdf.FS_PATHPOINTF.Y" /> and <see cref="P:Patagames.Pdf.FS_PATHPOINTF.Flags" /> values of the
  /// left and right <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> structures are equal; otherwise, false.
  /// </returns>
  public static bool operator ==(FS_PATHPOINTF left, FS_PATHPOINTF right)
  {
    return left.X.Equals(right.X) && left.Y.Equals(right.Y) && left.Flags.Equals((object) right.Flags);
  }

  /// <summary>
  /// Determines whether the coordinates of the specified points are not equal.
  /// </summary>
  /// <param name="left">A <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> to compare.</param>
  /// <param name="right">A <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> to compare.</param>
  /// <returns>true to indicate the <see cref="P:Patagames.Pdf.FS_PATHPOINTF.X" />, <see cref="P:Patagames.Pdf.FS_PATHPOINTF.Y" /> and <see cref="P:Patagames.Pdf.FS_PATHPOINTF.Flags" /> values of left and right are not equal; otherwise, false.</returns>
  public static bool operator !=(FS_PATHPOINTF left, FS_PATHPOINTF right) => !(left == right);

  /// <summary>Returns the hash code for this instance.</summary>
  /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
  public override int GetHashCode()
  {
    return ((17 * 23 + this.X.GetHashCode()) * 23 + this.Y.GetHashCode()) * 23 + this.Flags.GetHashCode();
  }

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> structure
  /// </summary>
  /// <param name="x">The x-coordinate of this <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />.</param>
  /// <param name="y">The y-coordinate of this <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />.</param>
  /// <param name="flags">The flags of this <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />.</param>
  public FS_PATHPOINTF(float x, float y, PathPointFlags flags)
  {
    this.X = x;
    this.Y = y;
    this.Flags = flags;
  }

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> structure
  /// </summary>
  /// <param name="x">The x-coordinate of this <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />.</param>
  /// <param name="y">The y-coordinate of this <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />.</param>
  /// <param name="flags">The flags of this <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />.</param>
  public FS_PATHPOINTF(double x, double y, PathPointFlags flags)
  {
    this.X = (float) x;
    this.Y = (float) y;
    this.Flags = flags;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> structure with the specified array.
  /// </summary>
  /// <param name="array">The <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeArray" /> or <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeIndirect" /> array that contains X, Y coordinates and flag.</param>
  public FS_PATHPOINTF(PdfTypeBase array)
  {
    PdfTypeArray pdfTypeArray = array.As<PdfTypeArray>();
    int count = pdfTypeArray.Count;
    this.X = count > 0 ? pdfTypeArray[0].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.Y = count > 1 ? pdfTypeArray[1].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.Flags = count > 2 ? (PathPointFlags) pdfTypeArray[2].As<PdfTypeNumber>().IntValue : PathPointFlags.MoveTo;
  }

  /// <summary>
  /// Returns a <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeArray" /> representation of the path point.
  /// </summary>
  /// <returns>An array of 3 numbers specifying the coordinates of point given in the order X, Y and flags.</returns>
  public PdfTypeArray ToArray()
  {
    PdfTypeArray array = PdfTypeArray.Create();
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.X));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.Y));
    array.Add((PdfTypeBase) PdfTypeNumber.Create((int) this.Flags));
    return array;
  }
}
