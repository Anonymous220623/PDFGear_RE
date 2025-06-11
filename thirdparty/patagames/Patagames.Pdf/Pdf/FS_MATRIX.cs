// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FS_MATRIX
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Represents the transformation matrices</summary>
/// <remarks>
/// To understand the mathematics of coordinate transformations in PDF, it is vital to remember two points:
/// <list type="bullet">
/// <item>Transformations alter coordinate systems, not graphics objects. All objects painted before a transformation is applied are unaffected by the transformation. Objects painted after the transformation is applied are interpreted in the transformed coordinate system. </item>
/// <item>Transformation matrices specify the transformation from the new (transformed) coordinate system to the original (untransformed) coordinate system. All coordinates used after the transformation are expressed in the transformed coordinate system. PDF applies the transformation matrix to find the equivalent coordinates in the untransformed coordinate system. </item>
/// </list>
/// <note type="note">Many computer graphics textbooks consider transformations of graphics objects rather than of coordinate systems. Although either approach is correct and selfconsistent, some details of the calculations differ depending on which point of view is taken. </note>
/// <para>PDF represents coordinates in a two-dimensional space. The point (x, y) in such a space can be expressed in vector form as [x y 1]. The constant third element of this vector (1) is needed so that the vector can be used with 3-by-3 matrices in the calculations described below. </para>
/// <para>The transformation between two coordinate systems is represented by a 3-by-3 transformation matrix written as follows:</para>
/// <pre>
/// |a b 0|
/// |c d 0|
/// |e f 1|
/// </pre>
/// <para>Because a transformation matrix has only six elements that can be changed, it is usually specified in PDF as the six-element array [a b c d e f ]. </para>
/// <para>Coordinate transformations are expressed as matrix multiplications:</para>
/// <pre>
///                            |a b 0|
/// [ x′ y′ 1 ] =  [ x y 1 ] x |c d 0|
///                            |e f 1|
/// </pre>
/// <para>
/// Because PDF transformation matrices specify the conversion from the transformed coordinate system to the
/// original (untransformed) coordinate system, x′ and y′ in this equation are the coordinates in the untransformed coordinate system,
/// and x and y are the coordinates in the transformed system. The multiplication is carried out as follows:
/// </para>
/// <pre>
/// x′ = a × x + c × y + e
/// y′ = b × x + d × y + f
/// </pre>
/// <para>If a series of transformations is carried out, the matrices representing each of the individual transformations can be multiplied together to produce a single equivalent matrix representing the composite transformation</para>
/// <para>
/// Matrix multiplication is not commutative — the order in which matrices are multiplied is significant. Consider a sequence of two transformations: a scaling transformation
/// applied to the user space coordinate system, followed by a conversion from the resulting scaled user space to device space. Let Ms be the matrix specifying
/// the scaling and Mc the current transformation matrix, which transforms user space to device space. Recalling that coordinates are always specified in the transformed
/// space, the correct order of transformations must first convert the scaled coordinates to default user space and then the default user space coordinates to device space. This can be expressed as
/// </para>
/// <pre>
/// Xd = Xu × Mc  = (Xs × Ms) × Mc = Xs × (Ms × Mc)
/// </pre>
/// where
/// <pre>
/// Xd denotes the coordinates in device space<markup><br /></markup>
/// Xu denotes the coordinates in default user space<markup><br /></markup>
/// Xs denotes the coordinates in scaled user space
/// </pre>
/// <para>This shows that when a new transformation is concatenated with an existing one, the matrix representing it must be multiplied before (premultiplied with) the existing transformation matrix. </para>
/// <para>This result is true in general for PDF: when a sequence of transformations is carried
/// out, the matrix representing the combined transformation (M′) is calculated
/// by premultiplying the matrix representing the additional transformation (Mt)
/// with the one representing all previously existing transformations (M): </para>
/// <pre>
/// M′ = Mt × M
/// </pre>
/// <note type="note">When rendering graphics objects, it is sometimes necessary for an application
/// to perform the inverse of a transformation—that is, to find the user space coordinates
/// that correspond to a given pair of device space coordinates. Not all transformations
/// are invertible, however. For example, if a matrix contains a, b, c, and d
/// elements that are all zero, all user coordinates map to the same device coordinates
/// and there is no unique inverse transformation. Such noninvertible transformations
/// are not very useful and generally arise from unintended operations, such as scaling
/// by 0. Use of a noninvertible matrix when painting graphics objects can result in unpredictable behavior.</note>
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public class FS_MATRIX
{
  /// <summary>Coefficient a.</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float a;
  /// <summary>Coefficient b.</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float b;
  /// <summary>Coefficient c.</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float c;
  /// <summary>Coefficient d.</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float d;
  /// <summary>Coefficient e.</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float e;
  /// <summary>Coefficient f.</summary>
  [MarshalAs(UnmanagedType.R4)]
  public float f;

  /// <summary>Initialize a new instance of FS_MATRIX class</summary>
  public FS_MATRIX()
  {
  }

  /// <summary>Initialize a new instance of FS_MATRIX class</summary>
  /// <param name="a">Coefficient a</param>
  /// <param name="b">Coefficient b</param>
  /// <param name="c">Coefficient c</param>
  /// <param name="d">Coefficient d</param>
  /// <param name="e">Coefficient e</param>
  /// <param name="f">Coefficient f</param>
  public FS_MATRIX(float a, float b, float c, float d, float e, float f)
  {
    this.a = a;
    this.b = b;
    this.c = c;
    this.d = d;
    this.e = e;
    this.f = f;
  }

  /// <summary>Initialize a new instance of FS_MATRIX class</summary>
  /// <param name="a">Coefficient a</param>
  /// <param name="b">Coefficient b</param>
  /// <param name="c">Coefficient c</param>
  /// <param name="d">Coefficient d</param>
  /// <param name="e">Coefficient e</param>
  /// <param name="f">Coefficient f</param>
  public FS_MATRIX(double a, double b, double c, double d, double e, double f)
  {
    this.a = (float) a;
    this.b = (float) b;
    this.c = (float) c;
    this.d = (float) d;
    this.e = (float) e;
    this.f = (float) f;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.FS_MATRIX" /> structure with the specified array.
  /// </summary>
  /// <param name="array">The <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeArray" /> or <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeIndirect" /> array that contains a, b, c, d, e and f values of matrix</param>
  public FS_MATRIX(PdfTypeBase array)
  {
    PdfTypeArray pdfTypeArray = array.As<PdfTypeArray>();
    int count = pdfTypeArray.Count;
    this.a = count > 0 ? pdfTypeArray[0].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.b = count > 1 ? pdfTypeArray[1].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.c = count > 2 ? pdfTypeArray[2].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.d = count > 3 ? pdfTypeArray[3].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.e = count > 4 ? pdfTypeArray[4].As<PdfTypeNumber>().FloatValue : 0.0f;
    this.f = count > 5 ? pdfTypeArray[5].As<PdfTypeNumber>().FloatValue : 0.0f;
  }

  /// <summary>
  /// Returns a <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeArray" /> representation of the matrix.
  /// </summary>
  /// <returns>An array of 6 numbers specifying the matrix coefficients given in the order a, b, c, d, e, f.</returns>
  public PdfTypeArray ToArray()
  {
    PdfTypeArray array = PdfTypeArray.Create();
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.a));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.b));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.c));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.d));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.e));
    array.Add((PdfTypeBase) PdfTypeNumber.Create(this.f));
    return array;
  }

  internal static FS_MATRIX CreateRotation(double ang)
  {
    return new FS_MATRIX(Math.Cos(ang), -Math.Sin(ang), Math.Sin(ang), Math.Cos(ang), 0.0, 0.0);
  }

  /// <summary>
  /// Initializes this instance of the <see cref="T:Patagames.Pdf.FS_MATRIX" /> class as the identity matrix.
  /// </summary>
  public void SetIdentity() => Pdfium.FPDFMatrix_SetIdentity(this);

  /// <summary>Inverts this matrix, if it is invertible.</summary>
  public void SetReverse()
  {
    this.SetReverse(new FS_MATRIX(this.a, this.b, this.c, this.d, this.e, this.f));
  }

  /// <summary>
  /// Inverts the specified matrix, if it is invertible and set it to this matrix.
  /// </summary>
  /// <param name="matrix">The matrix by which this matrix is to be set.</param>
  public void SetReverse(FS_MATRIX matrix) => Pdfium.FPDFMatrix_SetReverse(this, matrix);

  /// <summary>
  /// Multiplies this matrix by the matrix specified in the matrix parameter, and in the order specified in the prepended parameter.
  /// </summary>
  /// <param name="matrix">The <see cref="T:Patagames.Pdf.FS_MATRIX" /> by which this matrix is to be multiplied.</param>
  /// <param name="prepended">Represents the order of the multiplication.</param>
  /// <remarks>
  /// <para>
  /// if the specified order is Prepend (the prepend parameter is True),
  /// this Matrix is multiplied by the specified matrix in a prepended order.
  /// Otherwise, this Matrix is multiplied by the specified matrix in an appended order.
  /// </para>
  /// <para>
  /// <list type="bullet">
  /// <item>Prepended order: [SpecifiedMatrix] x [ThisMatrix]</item>
  /// <item>Appended  order: [ThisMatrix] x [SpecifiedMatrix]</item>
  /// </list>
  /// </para>
  /// </remarks>
  public void Concat(FS_MATRIX matrix, bool prepended = false)
  {
    Pdfium.FPDFMatrix_Concat(this, matrix, prepended);
  }

  /// <summary>
  /// Inverts the matrix specified in the matrix parameter and than multiplies this matrix by the inverted matrix, and in the order specified in the prepended parameter.
  /// </summary>
  /// <param name="matrix">The <see cref="T:Patagames.Pdf.FS_MATRIX" /> by which this matrix is to be multiplied.</param>
  /// <param name="prepended">Represents the order of the multiplication.</param>
  /// <remarks>
  /// <para>
  /// if the specified order is Prepend (the prepend parameter is True),
  /// this Matrix is multiplied by the specified matrix in a prepended order.
  /// Otherwise, this Matrix is multiplied by the specified matrix in an appended order.
  /// </para>
  /// <para>
  /// <list type="bullet">
  /// <item>Prepended order: [SpecifiedMatrix] x [ThisMatrix]</item>
  /// <item>Appended  order: [ThisMatrix] x [SpecifiedMatrix]</item>
  /// </list>
  /// </para>
  /// </remarks>
  public void ConcatInverse(FS_MATRIX matrix, bool prepended = false)
  {
    Pdfium.FPDFMatrix_ConcatInverse(this, matrix, prepended);
  }

  /// <summary>
  /// Applies a clockwise rotation of an amount specified in the angle parameter, around the origin (zero x and y coordinates) for this Matrix.
  /// </summary>
  /// <param name="angle">The angle of the rotation, in radians.</param>
  /// <param name="prepended">Specifies the order (append or prepend) in which the rotation is applied to this Matrix.</param>
  public void Rotate(float angle, bool prepended = false)
  {
    Pdfium.FPDFMatrix_Rotate(this, angle, prepended);
  }

  /// <summary>
  /// Applies a clockwise rotation about the specified point to this Matrix in the specified order.
  /// </summary>
  /// <param name="angle">The angle of the rotation, in radians.</param>
  /// <param name="x">The x-coordinate of the point that represents the center of the rotation.</param>
  /// <param name="y">The y-coordinate of the point that represents the center of the rotation.</param>
  /// <param name="prepended">Specifies the order (append or prepend) in which the rotation is applied to this Matrix.</param>
  public void Rotate(float angle, float x, float y, bool prepended = false)
  {
    Pdfium.FPDFMatrix_RotateAt(this, angle, x, y, prepended);
  }

  /// <summary>
  /// Applies the specified scale vector (sx and sy) to this Matrix using the specified order.
  /// </summary>
  /// <param name="sx">The value by which to scale this Matrix in the x-axis direction.</param>
  /// <param name="sy">The value by which to scale this Matrix in the y-axis direction.</param>
  /// <param name="prepended">Specifies the order (append or prepend) in which the scale vector is applied to this Matrix.</param>
  public void Scale(float sx, float sy, bool prepended = false)
  {
    Pdfium.FPDFMatrix_Scale(this, sx, sy, prepended);
  }

  /// <summary>
  /// Applies the specified translation vector to this Matrix in the specified order.
  /// </summary>
  /// <param name="x">The x value by which to translate this Matrix.</param>
  /// <param name="y">The y value by which to translate this Matrix.</param>
  /// <param name="prepended">Specifies the order (append or prepend) in which the translation is applied to this Matrix.</param>
  public void Translate(float x, float y, bool prepended = false)
  {
    Pdfium.FPDFMatrix_Translate(this, x, y, prepended);
  }

  /// <summary>
  /// Applies the specified translation vector to this Matrix in the specified order.
  /// </summary>
  /// <param name="x">The x value by which to translate this Matrix.</param>
  /// <param name="y">The y value by which to translate this Matrix.</param>
  /// <param name="prepended">Specifies the order (append or prepend) in which the translation is applied to this Matrix.</param>
  public void Translate(int x, int y, bool prepended = false)
  {
    Pdfium.FPDFMatrix_Translate(this, x, y, prepended);
  }

  /// <summary>
  /// Applies the specified shear vector to this Matrix in the specified order.
  /// </summary>
  /// <param name="shearX">The horizontal shear factor.</param>
  /// <param name="shearY">The vertical shear factor.</param>
  /// <param name="prepended">Specifies the order (append or prepend) in which the shear is applied.</param>
  public void Shear(float shearX, float shearY, bool prepended = false)
  {
    Pdfium.FPDFMatrix_Shear(this, shearX, shearY, prepended);
  }

  /// <summary>
  /// Applies the geometric transform represented by this Matrix to a specified point.
  /// </summary>
  /// <param name="x">The x-coordinate of the point</param>
  /// <param name="y">The y-coordinate of the point</param>
  public void TransformPoint(ref float x, ref float y)
  {
    Pdfium.FPDFMatrix_TransformPoint(this, ref x, ref y);
  }

  /// <summary>
  /// Applies the geometric transform represented by this Matrix to a specified point.
  /// </summary>
  /// <param name="x">The x-coordinate of the point</param>
  /// <param name="y">The y-coordinate of the point</param>
  public void TransformPoint(ref int x, ref int y)
  {
    Pdfium.FPDFMatrix_TransformPoint(this, ref x, ref y);
  }

  /// <summary>
  /// Applies the geometric transform represented by this Matrix to a distance.
  /// </summary>
  /// <param name="dx">The x-coordinate of the distance vector</param>
  /// <param name="dy">The Y-coordinate of the distance vector</param>
  /// <returns>The length of hte distance vector</returns>
  public float TransformDistance(float dx, float dy)
  {
    return Pdfium.FPDFMatrix_TransformDistance(this, dx, dy);
  }

  /// <summary>
  /// Applies the geometric transform represented by this Matrix to a distance.
  /// </summary>
  /// <param name="dx">The x-coordinate of the distance vector</param>
  /// <param name="dy">The Y-coordinate of the distance vector</param>
  /// <returns>The length of hte distance vector</returns>
  public int TransformDistance(int dx, int dy) => Pdfium.FPDFMatrix_TransformDistance(this, dx, dy);

  /// <summary>
  /// Applies the geometric transform represented by this Matrix to a distance.
  /// </summary>
  /// <param name="distance">The distance vector</param>
  public void TransformDistance(ref float distance)
  {
    Pdfium.FPDFMatrix_TransformDistance(this, ref distance);
  }

  /// <summary>
  /// Applies the geometric transform represented by this Matrix to a x-coordinate of the distance vector.
  /// </summary>
  /// <param name="dx">The x-coordinate of the distance vector</param>
  public void TransformXDistance(ref float dx)
  {
    Pdfium.FPDFMatrix_TransformXDistance(this, ref dx);
  }

  /// <summary>
  /// Applies the geometric transform represented by this Matrix to a x-coordinate of the distance vector.
  /// </summary>
  /// <param name="dx">The x-coordinate of the distance vector</param>
  public void TransformXDistance(ref int dx) => Pdfium.FPDFMatrix_TransformXDistance(this, ref dx);

  /// <summary>
  /// Applies the geometric transform represented by this Matrix to a y-coordinate of the distance vector.
  /// </summary>
  /// <param name="dy">The y-coordinate of the distance vector</param>
  public void TransformYDistance(ref float dy)
  {
    Pdfium.FPDFMatrix_TransformYDistance(this, ref dy);
  }

  /// <summary>
  /// Applies the geometric transform represented by this Matrix to a y-coordinate of the distance vector.
  /// </summary>
  /// <param name="dy">The y-coordinate of the distance vector</param>
  public void TransformYDistance(ref int dy) => Pdfium.FPDFMatrix_TransformYDistance(this, ref dy);

  /// <summary>
  /// Applies the geometric transform represented by this Matrix to a specified rectangle.
  /// </summary>
  /// <param name="rect">The rectangle to transform</param>
  public void TransformRect(ref FS_RECTF rect) => Pdfium.FPDFMatrix_TransformRect(this, ref rect);

  /// <summary>
  /// Applies only the scale and rotate components of this Matrix to the specified point.
  /// </summary>
  /// <param name="vx">The x-coordinate of the point</param>
  /// <param name="vy">The y-coordinate of the point</param>
  public void TransformVector(ref float vx, ref float vy)
  {
    Pdfium.FPDFMatrix_TransformVector(this, ref vx, ref vy);
  }

  /// <summary>Get unit area</summary>
  /// <returns>Unit area</returns>
  public float GetUnitArea() => Pdfium.FPDFMatrix_GetUnitArea(this);

  /// <summary>Get x unit</summary>
  /// <returns>x unit</returns>
  public float GetXUnit() => Pdfium.FPDFMatrix_GetXUnit(this);

  /// <summary>Get y unit</summary>
  /// <returns>y unit</returns>
  public float GetYUnit() => Pdfium.FPDFMatrix_GetYUnit(this);

  /// <summary>
  /// Gets a value indicating whether this Matrix is rotated.
  /// </summary>
  /// <returns>true if this Matrix is rotated; otherwise, false.</returns>
  public bool Is90Rotated() => Pdfium.FPDFMatrix_Is90Rotated(this);

  /// <summary>
  /// Gets a value indicating whether this Matrix is the identity matrix.
  /// </summary>
  /// <returns>true if this Matrix is identity; otherwise, false.</returns>
  public bool IsIdentity() => Pdfium.FPDFMatrix_IsIdentity(this);

  /// <summary>
  /// Gets a value indicating whether this Matrix is invertible.
  /// </summary>
  /// <returns>true if this Matrix is invertible; otherwise, false.</returns>
  public bool IsInvertible() => Pdfium.FPDFMatrix_IsInvertible(this);

  /// <summary>
  /// Gets a value indicating whether this Matrix is scaled.
  /// </summary>
  /// <returns>true if this Matrix is scaled; otherwise, false.</returns>
  public bool IsScaled() => Pdfium.FPDFMatrix_IsScaled(this);

  /// <summary>
  /// Inverts this Matrix, if it is invertible and return as a new Matrix.
  /// </summary>
  /// <returns>The new inctance of the <see cref="T:Patagames.Pdf.FS_MATRIX" /> class.</returns>
  public FS_MATRIX GetReverse()
  {
    FS_MATRIX reverse = new FS_MATRIX();
    reverse.SetReverse(this);
    return reverse;
  }
}
