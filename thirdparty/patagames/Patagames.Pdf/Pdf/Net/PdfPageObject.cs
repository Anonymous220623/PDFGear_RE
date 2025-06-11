// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfPageObject
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents the base class for page objects that contain common data, and provides common methods.
/// </summary>
/// <remarks>
/// This class serves as the base class for all objects on the page.
/// For example, the <see cref="T:Patagames.Pdf.Net.PdfImageObject" /> class derives from PdfPageObject.
/// </remarks>
public abstract class PdfPageObject
{
  private PdfTypeBase _softMask;

  /// <summary>
  /// Gets the Pdfium SDK handle that the object is bound to
  /// </summary>
  public IntPtr Handle { get; protected set; }

  /// <summary>
  /// ets a value that represents that whether the specified PDF page object contains transparency.
  /// </summary>
  public bool IsTransparency => Pdfium.FPDFPageObj_HasTransparency(this.Handle);

  /// <summary>Gets page object type</summary>
  public PageObjectTypes ObjectType => Pdfium.FPDFPageObj_GetType(this.Handle);

  /// <summary>
  /// Gets the list of <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> items associated with this object.
  /// </summary>
  public PdfMarkedContentCollection MarkedContent { get; private set; }

  /// <summary>Gets page object bounding box.</summary>
  public FS_RECTF BoundingBox
  {
    get
    {
      float left;
      float top;
      float right;
      float bottom;
      Pdfium.FPDFPageObj_GetBBox(this.Handle, this.Container == null || this.Container.Form == null ? (FS_MATRIX) null : this.Container.Form.Matrix, out left, out top, out right, out bottom);
      return new FS_RECTF(left, top, right, bottom);
    }
  }

  /// <summary>Gets or sets the fill color of a page object.</summary>
  public FS_COLOR FillColor
  {
    get => new FS_COLOR(Pdfium.FPDFPageObj_GetFillColor(this.Handle));
    set => Pdfium.FPDFPageObj_SetFillColor(this.Handle, value.ToArgb());
  }

  /// <summary>Gets or sets the stroke color of a page object.</summary>
  public FS_COLOR StrokeColor
  {
    get => new FS_COLOR(Pdfium.FPDFPageObj_GetStrokeColor(this.Handle));
    set => Pdfium.FPDFPageObj_SetStrokeColor(this.Handle, value.ToArgb());
  }

  /// <summary>
  /// Gets or sets the precision with which curves are to be rendered on the output device.
  /// </summary>
  /// <value>A number in the range 0 to 100; a value of 0 specifies the output device’s default flatness tolerance.</value>
  /// <remarks>
  /// <para>The value of this parameter gives the maximum error tolerance, measured in output device pixels; smaller numbers give smoother curves at the expense of more computation and memory use.</para>
  /// <para>The flatness tolerance controls the maximum permitted distance in device pixels between the mathematically correct path and an approximation constructed from straight line segments, as shown in Figure 6.6 </para>
  /// <list type="table">
  /// <item><term><img src="../Media/Figure6.6FlatnessTolerance.png" /></term></item>
  /// <listheader>
  /// <term>FIGURE 6.6 Flatness tolerance</term>
  /// </listheader>
  /// </list>
  /// </remarks>
  public float Flatness
  {
    get => Pdfium.FPDFPageObj_GetFlatness(this.Handle);
    set => Pdfium.FPDFPageObj_SetFlatness(this.Handle, value);
  }

  /// <summary>
  /// Gets or sets the precision with which color gradients are to be rendered on the output device.
  /// </summary>
  /// <value>A number in the range 0.0 to 1.0.</value>
  /// <remarks>
  /// <para>
  /// The value of this parameter gives the maximum error tolerance, expressed as a fraction of the range of each color component;
  /// smaller numbers give smoother color transitions at the expense of more computation and memory use.
  /// </para>
  /// <para>
  ///  The smoothness tolerance controls the quality of smooth shading (type 2 patterns and the sh operator) and thus indirectly controls the rendering performance.
  ///  Smoothness is the allowable color error between a shading approximated by piecewise linear interpolation and the true value of a (possibly nonlinear) shading function. The error is measured for each color component, and
  ///  the maximum error is used.The allowable error(or tolerance) is expressed as a
  ///  fraction of the range of the color component, from 0.0 to 1.0. Thus, a smoothness
  ///  tolerance of 0.1 represents a tolerance of 10 percent in each color component.
  ///  </para>
  /// </remarks>
  public float Smoothness
  {
    get => Pdfium.FPDFPageObj_GetSmoothness(this.Handle);
    set => Pdfium.FPDFPageObj_SetSmoothness(this.Handle, value);
  }

  /// <summary>
  /// Gets or sets the blend mode to be used in the transparent imaging model.
  /// </summary>
  /// <value>One of the values of the <see cref="T:Patagames.Pdf.Enums.BlendTypes" /> enumeration.</value>
  /// <remarks>See <see cref="T:Patagames.Pdf.Enums.BlendTypes" /> enumeration for details. </remarks>
  public BlendTypes BlendMode
  {
    get => Pdfium.FPDFPageObj_GetBlendMode(this.Handle);
    set => Pdfium.FPDFPageObj_SetBlendMode(this.Handle, value);
  }

  /// <summary>
  /// Gets or sets the rendering intent to be used when converting CIE-based colors to device colors.
  /// </summary>
  /// <value>One of the values of the <see cref="T:Patagames.Pdf.Enums.RenderIntent" /> enumeration.</value>
  /// <remarks>See <see cref="P:Patagames.Pdf.Net.PdfPageObject.RenderIntent" /> enumeration for details. </remarks>
  public RenderIntent RenderIntent
  {
    get => Pdfium.FPDFPageObj_GetRenderIntent(this.Handle);
    set => Pdfium.FPDFPageObj_SetRenderIntent(this.Handle, value);
  }

  /// <summary>Gets or sets an overprint mode.</summary>
  /// <value>A code specifying whether a color component value of 0 in a DeviceCMYK color space should erase that component(<see cref="F:Patagames.Pdf.Enums.OverprintModes.Zero" />) or leave it unchanged(<see cref="F:Patagames.Pdf.Enums.OverprintModes.NonZero" />) when overprinting.</value>
  /// <remarks>See <see cref="T:Patagames.Pdf.Enums.OverprintModes" /> enumeration for details.</remarks>
  public OverprintModes OverprintMode
  {
    get => Pdfium.FPDFPageObj_GetOverprintMode(this.Handle);
    set => Pdfium.FPDFPageObj_SetOverprintMode(this.Handle, value);
  }

  /// <summary>
  /// Gets or sets an overprint flag for stroking operations.
  /// </summary>
  /// <value>A flag for stroking operations, specifying whether painting in one set of colorants should cause the corresponding areas of other colorants to be erased(false) or left unchanged(true). Please see remarks section for details.</value>
  /// <remarks>
  /// <para>
  /// If the <see cref="P:Patagames.Pdf.Net.PdfPageObject.OverprintMode" /> is set to <see cref="F:Patagames.Pdf.Enums.OverprintModes.Zero" /> (the default value), painting a color in any color space causes the corresponding areas of unspecified colorants to be erased(painted with a tint value of 0.0). The effect is that the color at any position on the page is whatever was painted there last, which is consistent with the normal painting behavior of the opaque imaging model.
  /// </para>
  /// <para>
  /// If the <see cref="P:Patagames.Pdf.Net.PdfPageObject.OverprintMode" /> is set to <see cref="F:Patagames.Pdf.Enums.OverprintModes.NonZero" /> and the output device supports overprinting, no such erasing actions are performed; anything previously painted in other colorants is left undisturbed. Consequently, the color at a given position on the page may be a combined result of several painting operations in different colorants. The effect produced by such overprinting is device-dependent and is not defined by the PDF language.
  /// </para>
  /// </remarks>
  public bool StrokeOverprint
  {
    get => Pdfium.FPDFPageObj_GetStrokeOverprintFlag(this.Handle);
    set => Pdfium.FPDFPageObj_SetStrokeOverprintFlag(this.Handle, value);
  }

  /// <summary>
  /// Gets or sets an overprint flag for non-stroking operations.
  /// </summary>
  /// <value>A flag for stroking operations, specifying whether painting in one set of colorants should cause the corresponding areas of other colorants to be erased(false) or left unchanged(true). Please see remarks section for details.</value>
  /// <remarks>See remarks section of the <see cref="P:Patagames.Pdf.Net.PdfPageObject.StrokeOverprint" /> for details.</remarks>
  public bool FillOverprint
  {
    get => Pdfium.FPDFPageObj_GetFillOverprintFlag(this.Handle);
    set => Pdfium.FPDFPageObj_SetFillOverprintFlag(this.Handle, value);
  }

  /// <summary>
  /// Gets or sets a flag specifying whether the current soft mask and alpha constant parameters are to be interpreted as shape values (true) or opacity values(false).
  /// </summary>
  public bool AlphaShape
  {
    get => Pdfium.FPDFPageObj_GetAlphaIsShapeFlag(this.Handle);
    set => Pdfium.FPDFPageObj_SetAlphaIsShapeFlag(this.Handle, value);
  }

  /// <summary>
  /// Gets or sets the current soft mask, specifying the mask shape or mask opacity values to be used in the transparent imaging model.
  /// </summary>
  /// <value>A soft-mask dictionary specifying the mask shape or mask opacity values to be used in the transparent imaging model, or the name None if no such mask is specified.</value>
  public PdfTypeBase SoftMask
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeBase) null;
      IntPtr softMask = Pdfium.FPDFPageObj_GetSoftMask(this.Handle);
      if (softMask == IntPtr.Zero)
        return (PdfTypeBase) null;
      if (this._softMask == null || this._softMask.IsDisposed || this._softMask.Handle != softMask)
        this._softMask = PdfTypeBase.Create(softMask);
      return this._softMask;
    }
    set
    {
      Pdfium.FPDFPageObj_SetSoftMask(this.Handle, value != null ? value.Handle : IntPtr.Zero);
      this._softMask = value;
    }
  }

  /// <summary>
  /// Gets or sets the transformation matrix for the page object.
  /// </summary>
  public FS_MATRIX Matrix
  {
    get
    {
      switch (this)
      {
        case PdfFormObject _:
          return Pdfium.FPDFFormObj_GetFormMatrix(this.Handle);
        case PdfImageObject _:
          return Pdfium.FPDFImageObj_GetMatrix(this.Handle);
        case PdfPathObject _:
          return Pdfium.FPDFPathObj_GetMatrix(this.Handle);
        case PdfShadingObject _:
          return Pdfium.FPDFShadingObj_GetMatrix(this.Handle);
        case PdfTextObject _:
          return Pdfium.FPDFTextObj_GetTextMatrix(this.Handle);
        default:
          return new FS_MATRIX(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
      }
    }
    set
    {
      switch (this)
      {
        case PdfFormObject _:
          Pdfium.FPDFFormObj_SetFormMatrix(this.Handle, value);
          break;
        case PdfImageObject _:
          Pdfium.FPDFImageObj_SetMatrix(this.Handle, value);
          break;
        case PdfPathObject _:
          Pdfium.FPDFPathObj_SetMatrix(this.Handle, value);
          break;
        case PdfShadingObject _:
          Pdfium.FPDFShadingObj_SetMatrix(this.Handle, value);
          break;
        case PdfTextObject _:
          Pdfium.FPDFTextObj_SetTextMatrix(this.Handle, value);
          break;
      }
    }
  }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfPageObjectsCollection" /> that contains this <see cref="T:Patagames.Pdf.Net.PdfPageObject" />.
  /// </summary>
  /// <value>The <see cref="T:Patagames.Pdf.Net.PdfPageObjectsCollection" /> that contains the <see cref="T:Patagames.Pdf.Net.PdfPageObject" />,
  /// if any, or null if the <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> is not added in a <see cref="T:Patagames.Pdf.Net.PdfPageObjectsCollection" />.</value>
  public PdfPageObjectsCollection Container { get; internal set; }

  /// <summary>
  /// Initializes a new instance of the PdfPageObjectsCollection class.
  /// </summary>
  /// <param name="handle">The Pdfium SDK handle that the new object will be bound to</param>
  internal PdfPageObject(IntPtr handle)
  {
    this.Handle = handle;
    this.MarkedContent = new PdfMarkedContentCollection(this);
  }

  /// <summary>
  /// Identifies the type of the specified object and creates an instance of this type.
  /// </summary>
  /// <param name="handle">A handle to the page object.</param>
  /// <returns>The instance of a newly created object</returns>
  public static PdfPageObject Create(IntPtr handle)
  {
    switch (Pdfium.FPDFPageObj_GetType(handle))
    {
      case PageObjectTypes.PDFPAGE_TEXT:
        return (PdfPageObject) new PdfTextObject(handle);
      case PageObjectTypes.PDFPAGE_PATH:
        return (PdfPageObject) new PdfPathObject(handle);
      case PageObjectTypes.PDFPAGE_IMAGE:
        return (PdfPageObject) new PdfImageObject(handle);
      case PageObjectTypes.PDFPAGE_SHADING:
        return (PdfPageObject) new PdfShadingObject(handle);
      case PageObjectTypes.PDFPAGE_FORM:
        return (PdfPageObject) new PdfFormObject(handle);
      default:
        return (PdfPageObject) new PdfUnknownObject(handle);
    }
  }

  /// <summary>Create new page object specified by type</summary>
  /// <param name="type">ypes of page object</param>
  /// <returns>Handle to a page object</returns>
  /// <remarks>
  /// Currently, It can be "PDFPAGE_TEXT", "PDFPAGE_IMAGE", "PDFPAGE_PATH",
  /// "PDFPAGE_SHADING", "PDFPAGE_FORM"
  /// </remarks>
  protected internal static IntPtr CreateObject(PageObjectTypes type)
  {
    return Pdfium.FPDFPageObj_Create(type);
  }

  /// <summary>Transform (scale, rotate, shear, move) page object.</summary>
  /// <param name="a">The coefficient "a" of the matrix</param>
  /// <param name="b">The coefficient "b" of the matrix</param>
  /// <param name="c">The coefficient "c" of the matrix</param>
  /// <param name="d">The coefficient "d" of the matrix</param>
  /// <param name="e">The coefficient "e" of the matrix</param>
  /// <param name="f">The coefficient "f" of the matrix</param>
  public virtual void Transform(float a, float b, float c, float d, float e, float f)
  {
    Pdfium.FPDFPageObj_Transform(this.Handle, (double) a, (double) b, (double) c, (double) d, (double) e, (double) f);
  }

  /// <summary>Transform (scale, rotate, shear, move) page object.</summary>
  /// <param name="matrix">The transform matrix.</param>
  public virtual void Transform(FS_MATRIX matrix)
  {
    Pdfium.FPDFPageObj_Transform(this.Handle, (double) matrix.a, (double) matrix.b, (double) matrix.c, (double) matrix.d, (double) matrix.e, (double) matrix.f);
  }

  /// <summary>
  /// Transform (scale, rotate, shear, move) the clip path of an object.
  /// </summary>
  /// <param name="matrix">The transform matrix.</param>
  public void TransformClipPath(FS_MATRIX matrix)
  {
    Pdfium.FPDFPageObj_TransformClipPath(this.Handle, (double) matrix.a, (double) matrix.b, (double) matrix.c, (double) matrix.d, (double) matrix.e, (double) matrix.f);
  }

  /// <summary>Copy page object information from one to another</summary>
  /// <param name="srcObj">Handle to source page object</param>
  [Obsolete("This method is obsolete", true)]
  public void Copy(PdfPageObject srcObj) => Pdfium.FPDFPageObj_Copy(this.Handle, srcObj.Handle);

  /// <summary>Create a new page object based on this page object.</summary>
  /// <returns>New page object.</returns>
  public PdfPageObject Clone()
  {
    IntPtr num = Pdfium.FPDFPageObj_Clone(this.Handle);
    if (num == IntPtr.Zero)
      return (PdfPageObject) null;
    switch (Pdfium.FPDFPageObj_GetType(num))
    {
      case PageObjectTypes.PDFPAGE_TEXT:
        return (PdfPageObject) new PdfTextObject(num);
      case PageObjectTypes.PDFPAGE_PATH:
        return (PdfPageObject) new PdfPathObject(num);
      case PageObjectTypes.PDFPAGE_IMAGE:
        return (PdfPageObject) new PdfImageObject(num);
      case PageObjectTypes.PDFPAGE_SHADING:
        return (PdfPageObject) new PdfShadingObject(num);
      case PageObjectTypes.PDFPAGE_FORM:
        return (PdfPageObject) new PdfFormObject(num);
      default:
        return (PdfPageObject) new PdfUnknownObject(num);
    }
  }

  /// <summary>Remove a clip from the page object.</summary>
  public void RemoveClipPath() => Pdfium.FPDFPageObj_RemoveClipPath(this.Handle);

  /// <summary>Copy clip path from one page object to another</summary>
  /// <param name="srcObj">Source page object</param>
  public void CopyClipPath(PdfPageObject srcObj)
  {
    Pdfium.FPDFPageObj_CopyClipPath(this.Handle, srcObj.Handle);
  }
}
