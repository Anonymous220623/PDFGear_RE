// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FS_QUADPOINTSF
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// The structure defines the coordinates of the quadrilateral.
/// </summary>
public struct FS_QUADPOINTSF
{
  /// <summary>x coordinate of the first point.</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float x1;
  /// <summary>y coordinate of the first point.</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float y1;
  /// <summary>x coordinate of the second point.</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float x2;
  /// <summary>y coordinate of the second point.</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float y2;
  /// <summary>x coordinate of the third point.</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float x3;
  /// <summary>y coordinate of the third point.</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float y3;
  /// <summary>x coordinate of the fourth point</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float x4;
  /// <summary>y coordinate of the fourth point</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float y4;

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> structure.
  /// </summary>
  /// <param name="x1">x coordinate of the first point.</param>
  /// <param name="y1">y coordinate of the first point.</param>
  /// <param name="x2">x coordinate of the second point.</param>
  /// <param name="y2">y coordinate of the second point.</param>
  /// <param name="x3">x coordinate of the third point.</param>
  /// <param name="y3">y coordinate of the third point.</param>
  /// <param name="x4">x coordinate of the fourth point</param>
  /// <param name="y4">y coordinate of the fourth point</param>
  public FS_QUADPOINTSF(
    float x1,
    float y1,
    float x2,
    float y2,
    float x3,
    float y3,
    float x4,
    float y4)
  {
    this.x1 = x1;
    this.y1 = y1;
    this.x2 = x2;
    this.y2 = y2;
    this.x3 = x3;
    this.y3 = y3;
    this.x4 = x4;
    this.y4 = y4;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> structure.
  /// </summary>
  /// <param name="x1">x coordinate of the first point.</param>
  /// <param name="y1">y coordinate of the first point.</param>
  /// <param name="x2">x coordinate of the second point.</param>
  /// <param name="y2">y coordinate of the second point.</param>
  /// <param name="x3">x coordinate of the third point.</param>
  /// <param name="y3">y coordinate of the third point.</param>
  /// <param name="x4">x coordinate of the fourth point</param>
  /// <param name="y4">y coordinate of the fourth point</param>
  public FS_QUADPOINTSF(
    double x1,
    double y1,
    double x2,
    double y2,
    double x3,
    double y3,
    double x4,
    double y4)
  {
    this.x1 = (float) x1;
    this.y1 = (float) y1;
    this.x2 = (float) x2;
    this.y2 = (float) y2;
    this.x3 = (float) x3;
    this.y3 = (float) y3;
    this.x4 = (float) x4;
    this.y4 = (float) y4;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> structure.
  /// </summary>
  /// <param name="leftBottom">The left-bottom point of the quadrilateral.</param>
  /// <param name="rightBottom">The right-bottom point of the quadrilateral.</param>
  /// <param name="rightTop">The right-top point of the quadrilateral.</param>
  /// <param name="leftTop">The left-top point of the quadrilateral.</param>
  public FS_QUADPOINTSF(
    FS_POINTF leftBottom,
    FS_POINTF rightBottom,
    FS_POINTF rightTop,
    FS_POINTF leftTop)
  {
    this.x1 = leftBottom.X;
    this.y1 = leftBottom.Y;
    this.x2 = rightBottom.X;
    this.y2 = rightBottom.Y;
    this.x3 = rightTop.X;
    this.y3 = rightTop.Y;
    this.x4 = leftTop.X;
    this.y4 = leftTop.Y;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> structure with the specified array.
  /// </summary>
  /// <param name="array">The <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeArray" /> or <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeIndirect" /> array of 8 numbers specifying the coordinates of quadrilateral given in the order x1, y1, x2, y2, x3, y3, x4, y4. </param>
  public FS_QUADPOINTSF(PdfTypeBase array)
  {
    PdfTypeArray pdfTypeArray = array.As<PdfTypeArray>();
    int count = pdfTypeArray.Count;
    this.x1 = count > 0 ? pdfTypeArray[0].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.y1 = count > 1 ? pdfTypeArray[1].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.x2 = count > 2 ? pdfTypeArray[2].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.y2 = count > 3 ? pdfTypeArray[3].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.x3 = count > 4 ? pdfTypeArray[4].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.y3 = count > 5 ? pdfTypeArray[5].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.x4 = count > 6 ? pdfTypeArray[6].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.y4 = count > 7 ? pdfTypeArray[7].As<PdfTypeNumber>().FloatValue : 0.0f;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> structure with the specified rectangle.
  /// </summary>
  /// <param name="rect">The <see cref="T:Patagames.Pdf.FS_RECTF" /> to initialize this quadrileteral.</param>
  public FS_QUADPOINTSF(FS_RECTF rect)
    : this()
  {
    this.x1 = rect.left;
    this.y1 = rect.bottom;
    this.x2 = rect.right;
    this.y2 = rect.bottom;
    this.x3 = rect.right;
    this.y3 = rect.top;
    this.x4 = rect.left;
    this.y4 = rect.top;
  }

  /// <summary>
  /// Returns a <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeArray" /> representation of the quadrilaterals.
  /// </summary>
  /// <returns>An array of 8 numbers specifying the coordinates of quadrilateral given in the order x1, y1, x2, y2, x3, y3, x4, y4.</returns>
  public PdfTypeArray ToArray()
  {
    PdfTypeArray array = PdfTypeArray.Create();
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.x1));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.y1));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.x2));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.y2));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.x3));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.y3));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.x4));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.y4));
    return array;
  }

  /// <summary>
  /// Specifies whether this <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> contains the same points as the specified object.
  /// </summary>
  /// <param name="obj">The System.Object to test.</param>
  /// <returns>This method returns true if obj is a <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" />and has the same points as this <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" />.</returns>
  public override bool Equals(object obj)
  {
    return obj is FS_QUADPOINTSF fsQuadpointsf && this.Equals(fsQuadpointsf);
  }

  /// <summary>
  /// Specifies whether this <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> contains the same points as the specified structure.
  /// </summary>
  /// <param name="obj">The <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> to test.</param>
  /// <returns>This method returns true if obj has the same points as this <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" />.</returns>
  public bool Equals(FS_QUADPOINTSF obj)
  {
    return obj.x1.Equals(this.x1) && obj.y1.Equals(this.y1) && obj.x2.Equals(this.x2) && obj.y2.Equals(this.y2) && obj.x3.Equals(this.x3) && obj.y3.Equals(this.y3) && obj.x4.Equals(this.x4) && obj.y4.Equals(this.y4);
  }

  /// <summary>
  /// Compares two <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> structures.
  /// </summary>
  /// <param name="left">A <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> to compare.</param>
  /// <param name="right">A <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> to compare.</param>
  /// <remarks>
  /// The result specifies whether the points of the both <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> structures are equal.
  /// </remarks>
  /// <returns>
  /// true if the  points of both <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> structures are equal; otherwise, false.
  /// </returns>
  public static bool operator ==(FS_QUADPOINTSF left, FS_QUADPOINTSF right) => left.Equals(right);

  /// <summary>
  /// Determines whether the points of the specified quad points strucures are not equal.
  /// </summary>
  /// <param name="left">A <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> to compare.</param>
  /// <param name="right">A <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> to compare.</param>
  /// <returns>true to indicate the <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> structures are not equal; otherwise, false.</returns>
  public static bool operator !=(FS_QUADPOINTSF left, FS_QUADPOINTSF right) => !(left == right);

  /// <summary>Returns the hash code for this instance.</summary>
  /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
  public override int GetHashCode()
  {
    return (((((((17 * 23 + this.x1.GetHashCode()) * 23 + this.x2.GetHashCode()) * 23 + this.x3.GetHashCode()) * 23 + this.x4.GetHashCode()) * 23 + this.y1.GetHashCode()) * 23 + this.y2.GetHashCode()) * 23 + this.y3.GetHashCode()) * 23 + this.y4.GetHashCode();
  }
}
