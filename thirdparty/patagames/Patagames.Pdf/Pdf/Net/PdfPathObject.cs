// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfPathObject
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents a path object.</summary>
public class PdfPathObject : PdfPageObject
{
  /// <summary>Get the path associated with this PathObject.</summary>
  public PathPointsCollection Path { get; private set; }

  /// <summary>Gets or sets the fill type of the path object.</summary>
  public FillModes FillType
  {
    get => Pdfium.FPDFPathObj_GetFillType(this.Handle);
    set => Pdfium.FPDFPathObj_SetFillType(this.Handle, value);
  }

  /// <summary>Gets or sets the stroke flag.</summary>
  public bool IsStroke
  {
    get => Pdfium.FPDFPathObj_GetIsStroke(this.Handle);
    set => Pdfium.FPDFPathObj_SetIsStroke(this.Handle, value);
  }

  /// <summary>
  /// Gets or sets the thickness, in user space units, of paths to be stroked.
  /// </summary>
  /// <remarks>
  /// <para>The line width parameter specifies the thickness of the line used to stroke a path. It is a non-negative number expressed in user space units; stroking a path entails painting all points whose perpendicular distance from the path in user space is less than or equal to half the line width.</para>
  /// <para>A line width of 0 denotes the thinnest line that can be rendered at device resolution: 1 device pixel wide. However, some devices cannot reproduce 1-pixel lines,
  /// and on high-resolution devices, they are nearly invisible.Since the results of rendering such zero-width lines are device-dependent, their use is not recommended.</para>
  /// </remarks>
  public float LineWidth
  {
    get => Pdfium.FPDFPageObj_GetLineWidth(this.Handle);
    set => Pdfium.FPDFPageObj_SetLineWidth(this.Handle, value);
  }

  /// <summary>
  /// Set the maximum length of mitered line joins for stroked paths.
  /// </summary>
  /// <value>A non-negative number expressed in user space units, specifies the limitation of the length of “spikes” produced when line segments join at sharp angles.</value>
  /// <remarks>
  /// When two line segments meet at a sharp angle and <see cref="F:Patagames.Pdf.Enums.LineJoin.Miter" /> have been specified as the line join style, it is possible for the miter to extend far beyond the
  /// thickness of the line stroking the path. The miter limit imposes a maximum on the ratio of the miter length to the line width.
  /// When the limit is exceeded, the join is converted from a <see cref="F:Patagames.Pdf.Enums.LineJoin.Miter" /> to a <see cref="F:Patagames.Pdf.Enums.LineJoin.Bevel" />.
  /// <list type="table">
  /// <item><term><img src="../Media/MiterLimit.png" /></term></item>
  /// <listheader>
  /// <term>FIGURE 4.7 Miter length</term>
  /// </listheader>
  /// </list>
  /// </remarks>
  public float MiterLimit
  {
    get => Pdfium.FPDFPageObj_GetMiterLimit(this.Handle);
    set => Pdfium.FPDFPageObj_SetMiterLimit(this.Handle, value);
  }

  /// <summary>
  /// Gets or sets the shape of joints between connected segments of a stroked path.
  /// </summary>
  /// <value>One of the values of the <see cref="T:Patagames.Pdf.Enums.LineJoin" /> enumeration.</value>
  /// <remarks>
  /// <para>The line join style specifies the shape to be used at the corners of paths that are stroked.</para>
  /// <para>Please find more details <see cref="T:Patagames.Pdf.Enums.LineJoin">here</see>.</para>
  /// </remarks>
  public LineJoin LineJoin
  {
    get => Pdfium.FPDFPageObj_GetLineJoin(this.Handle);
    set => Pdfium.FPDFPageObj_SetLineJoin(this.Handle, value);
  }

  /// <summary>
  /// Gets or sets the shape of the endpoints for any open path that is stroked.
  /// </summary>
  /// <value>One of the values of the <see cref="T:Patagames.Pdf.Enums.LineCap" /> enumeration.</value>
  /// <remarks>Please find more details here: <see cref="T:Patagames.Pdf.Enums.LineCap" />.</remarks>
  public LineCap LineCap
  {
    get => Pdfium.FPDFPageObj_GetLineCap(this.Handle);
    set => Pdfium.FPDFPageObj_SetLineCap(this.Handle, value);
  }

  /// <summary>
  /// Gets or sets the dash phase. The dash phase specifies the distance into the dash pattern at which to start the dash.
  /// </summary>
  /// <value>A non-negative number expressed in user space units.</value>
  public float DashPhase
  {
    get => Pdfium.FPDFPageObj_GetDashPhase(this.Handle);
    set => Pdfium.FPDFPageObj_SetDashPhase(this.Handle, value);
  }

  internal PdfPathObject(IntPtr pathHandle)
    : base(pathHandle)
  {
    IntPtr path = Pdfium.FPDFPathObj_GetPath(this.Handle);
    this.Path = !(path == IntPtr.Zero) ? new PathPointsCollection(path, this) : throw Pdfium.ProcessLastError();
  }

  /// <summary>
  /// Create new instance of <see cref="T:Patagames.Pdf.Net.PdfPathObject" /> class
  /// </summary>
  /// <returns>New instance of <see cref="T:Patagames.Pdf.Net.PdfPathObject" />.</returns>
  [Obsolete("This method is obsolete. Please use Create(FillModes, bool) instead")]
  public static PdfPathObject Create()
  {
    IntPtr pathHandle = PdfPageObject.CreateObject(PageObjectTypes.PDFPAGE_PATH);
    return pathHandle == IntPtr.Zero ? (PdfPathObject) null : new PdfPathObject(pathHandle);
  }

  /// <summary>
  /// Create new instance of <see cref="T:Patagames.Pdf.Net.PdfPathObject" /> class
  /// </summary>
  /// <param name="fillMode">Sets the fill type of the path object.</param>
  /// <param name="isStroke">Indicates that the path should be stroked.</param>
  /// <returns>New instance of <see cref="T:Patagames.Pdf.Net.PdfPathObject" />.</returns>
  public static PdfPathObject Create(FillModes fillMode, bool isStroke)
  {
    if (fillMode != FillModes.None && fillMode != FillModes.Alternate && fillMode != FillModes.Winding)
      throw new ArgumentException(Error.err0053, nameof (fillMode));
    IntPtr pathHandle = PdfPageObject.CreateObject(PageObjectTypes.PDFPAGE_PATH);
    if (pathHandle == IntPtr.Zero)
      return (PdfPathObject) null;
    return new PdfPathObject(pathHandle)
    {
      IsStroke = isStroke,
      FillType = fillMode
    };
  }

  /// <summary>Calculate bounding box</summary>
  public void CalcBoundingBox() => Pdfium.FPDFPathObj_CalcBoundingBox(this.Handle);

  /// <summary>
  /// Transform (scale, rotate, shear, move) <see cref="T:Patagames.Pdf.Net.PathPointsCollection" /> collection.
  /// </summary>
  /// <param name="a">The coefficient "a" of the matrix</param>
  /// <param name="b">The coefficient "b" of the matrix</param>
  /// <param name="c">The coefficient "c" of the matrix</param>
  /// <param name="d">The coefficient "d" of the matrix</param>
  /// <param name="e">The coefficient "e" of the matrix</param>
  /// <param name="f">The coefficient "f" of the matrix</param>
  public void TransformPath(float a, float b, float c, float d, float e, float f)
  {
    this.TransformPath(new FS_MATRIX()
    {
      a = a,
      b = b,
      c = c,
      d = d,
      e = e,
      f = f
    });
  }

  /// <summary>
  /// Transform <see cref="T:Patagames.Pdf.Net.PathPointsCollection" /> collection with a specified matrix
  /// </summary>
  /// <param name="matrix">The transform matrix.</param>
  public void TransformPath(FS_MATRIX matrix) => this.Path.Transform(matrix);

  /// <summary>Get the line dash array.</summary>
  /// <returns>An array of  numbers that specify the lengths of alternating dashes and gaps; the numbers must be nonnegative and not all zero.</returns>
  public float[] GetDashArray() => Pdfium.FPDFPageObj_GetDashArray(this.Handle);

  /// <summary>Set the line dash array.</summary>
  /// <param name="dash">An array of  numbers that specify the lengths of alternating dashes and gaps; the numbers must be nonnegative and not all zero.</param>
  public void SetDashArray(float[] dash) => Pdfium.FPDFPageObj_SetDashArray(this.Handle, dash);
}
