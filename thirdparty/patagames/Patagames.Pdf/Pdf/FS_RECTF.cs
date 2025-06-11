// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FS_RECTF
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// Rectangle area(float) in device or page coordination system.
/// </summary>
public struct FS_RECTF
{
  /// <summary>
  /// The x-coordinate of the left edge of this <see cref="T:Patagames.Pdf.FS_RECTF" /> structure.
  /// </summary>
  public float left;
  /// <summary>
  /// The y-coordinate of the top edge of this <see cref="T:Patagames.Pdf.FS_RECTF" /> structure.
  /// </summary>
  public float top;
  /// <summary>
  /// The x-coordinate of the right edge of this <see cref="T:Patagames.Pdf.FS_RECTF" /> structure.
  /// </summary>
  public float right;
  /// <summary>
  /// The y-coordinate of the bottom edge of this <see cref="T:Patagames.Pdf.FS_RECTF" /> structure.
  /// </summary>
  public float bottom;

  /// <summary>Gets the width of current rectangle</summary>
  public float Width => this.right - this.left;

  /// <summary>Gets the height of current rectangle</summary>
  public float Height => this.top - this.bottom;

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.FS_RECTF" /> structure with the specified location and size.
  /// </summary>
  /// <param name="l">The x-coordinate of the left edge of this <see cref="T:Patagames.Pdf.FS_RECTF" /> structure.</param>
  /// <param name="t">The y-coordinate of the top edge of this <see cref="T:Patagames.Pdf.FS_RECTF" /> structure.</param>
  /// <param name="r">The x-coordinate of the right edge of this <see cref="T:Patagames.Pdf.FS_RECTF" /> structure.</param>
  /// <param name="b">The y-coordinate of the bottom edge of this <see cref="T:Patagames.Pdf.FS_RECTF" /> structure.</param>
  public FS_RECTF(float l, float t, float r, float b)
  {
    this.left = l;
    this.right = r;
    this.bottom = b;
    this.top = t;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.FS_RECTF" /> structure with the specified location and size.
  /// </summary>
  /// <param name="l">The x-coordinate of the left edge of this <see cref="T:Patagames.Pdf.FS_RECTF" /> structure.</param>
  /// <param name="t">The y-coordinate of the top edge of this <see cref="T:Patagames.Pdf.FS_RECTF" /> structure.</param>
  /// <param name="r">The x-coordinate of the right edge of this <see cref="T:Patagames.Pdf.FS_RECTF" /> structure.</param>
  /// <param name="b">The y-coordinate of the bottom edge of this <see cref="T:Patagames.Pdf.FS_RECTF" /> structure.</param>
  public FS_RECTF(double l, double t, double r, double b)
  {
    this.left = (float) l;
    this.right = (float) r;
    this.bottom = (float) b;
    this.top = (float) t;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.FS_RECTF" /> structure with the specified array.
  /// </summary>
  /// <param name="array">The <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeArray" /> or <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeIndirect" /> array that contains left, bottom, right and top points of rectangle</param>
  public FS_RECTF(PdfTypeBase array)
  {
    PdfTypeArray pdfTypeArray = array.As<PdfTypeArray>();
    int count = pdfTypeArray.Count;
    this.left = count > 0 ? pdfTypeArray[0].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.bottom = count > 1 ? pdfTypeArray[1].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.right = count > 2 ? pdfTypeArray[2].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.top = count > 3 ? pdfTypeArray[3].As<PdfTypeNumber>().FloatValue : 0.0f;
  }

  /// <summary>
  /// Returns a <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeArray" /> representation of the rectangle.
  /// </summary>
  /// <returns>An array of 4 numbers specifying the coordinates of rectangle given in the order left edge, bottom edge, right edge, top edge.</returns>
  public PdfTypeArray ToArray()
  {
    PdfTypeArray array = PdfTypeArray.Create();
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.left));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.bottom));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.right));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.top));
    return array;
  }

  /// <summary>
  /// Specifies whether this <see cref="T:Patagames.Pdf.FS_RECTF" /> contains the same coordinates of left, right, top and bottom edges as the specified object.
  /// </summary>
  /// <param name="obj">The System.Object to test.</param>
  /// <returns>This method returns true if obj is a <see cref="T:Patagames.Pdf.FS_RECTF" />and has the same coordinates of left, right, top and bottom edges as this <see cref="T:Patagames.Pdf.FS_RECTF" />.</returns>
  public override bool Equals(object obj) => obj is FS_RECTF fsRectf && this.Equals(fsRectf);

  /// <summary>
  /// Specifies whether this <see cref="T:Patagames.Pdf.FS_RECTF" /> contains the same coordinates of left, right, top and bottom edges as the specified rectangle.
  /// </summary>
  /// <param name="obj">The <see cref="T:Patagames.Pdf.FS_RECTF" /> to test.</param>
  /// <returns>This method returns true if obj has the same coordinates of left, right, top and bottom edges as this <see cref="T:Patagames.Pdf.FS_RECTF" />.</returns>
  public bool Equals(FS_RECTF obj)
  {
    return obj.left.Equals(this.left) && obj.right.Equals(this.right) && obj.top.Equals(this.top) && obj.bottom.Equals(this.bottom);
  }

  /// <summary>
  /// Compares two <see cref="T:Patagames.Pdf.FS_RECTF" /> structures.
  /// </summary>
  /// <param name="left">A <see cref="T:Patagames.Pdf.FS_RECTF" /> to compare.</param>
  /// <param name="right">A <see cref="T:Patagames.Pdf.FS_RECTF" /> to compare.</param>
  /// <remarks>
  /// The result specifies whether the
  /// values of the <see cref="F:Patagames.Pdf.FS_RECTF.left" />, <see cref="F:Patagames.Pdf.FS_RECTF.right" />, <see cref="F:Patagames.Pdf.FS_RECTF.top" /> and <see cref="F:Patagames.Pdf.FS_RECTF.bottom" /> properties
  /// of the two <see cref="T:Patagames.Pdf.FS_RECTF" /> structures are equal.
  /// </remarks>
  /// <returns>
  /// true if the <see cref="F:Patagames.Pdf.FS_RECTF.left" />, <see cref="F:Patagames.Pdf.FS_RECTF.right" />, <see cref="F:Patagames.Pdf.FS_RECTF.top" /> and <see cref="F:Patagames.Pdf.FS_RECTF.bottom" /> values of the
  /// left and right <see cref="T:Patagames.Pdf.FS_RECTF" /> structures are equal; otherwise, false.
  /// </returns>
  public static bool operator ==(FS_RECTF left, FS_RECTF right) => left.Equals(right);

  /// <summary>
  /// Determines whether the coordinates of left, right, top and bottom edges of the specified rectangle are not equal.
  /// </summary>
  /// <param name="left">A <see cref="T:Patagames.Pdf.FS_RECTF" /> to compare.</param>
  /// <param name="right">A <see cref="T:Patagames.Pdf.FS_RECTF" /> to compare.</param>
  /// <returns>true to indicate the <see cref="F:Patagames.Pdf.FS_RECTF.left" />, <see cref="F:Patagames.Pdf.FS_RECTF.top" />, <see cref="F:Patagames.Pdf.FS_RECTF.right" /> and <see cref="F:Patagames.Pdf.FS_RECTF.bottom" /> values of left and right are not equal; otherwise, false.</returns>
  public static bool operator !=(FS_RECTF left, FS_RECTF right) => !(left == right);

  /// <summary>Returns the hash code for this instance.</summary>
  /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
  public override int GetHashCode()
  {
    return (((17 * 23 + this.left.GetHashCode()) * 23 + this.right.GetHashCode()) * 23 + this.top.GetHashCode()) * 23 + this.bottom.GetHashCode();
  }

  /// <summary>
  /// Enlarges a FS_RECTF structure by the specified amount.
  /// </summary>
  /// <param name="rect">The amount to inflate this rectangle.</param>
  public void Inflate(FS_RECTF rect)
  {
    this.left -= rect.left;
    this.top += rect.top;
    this.right += rect.right;
    this.bottom -= rect.bottom;
  }
}
