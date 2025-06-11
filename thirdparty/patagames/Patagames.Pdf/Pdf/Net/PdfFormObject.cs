// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfFormObject
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents a form object.</summary>
public class PdfFormObject : PdfPageObject, IDisposable
{
  private IntPtr _content;
  private PdfTypeStream _stream;

  /// <summary>
  /// Gets the underlying <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeStream" />.
  /// </summary>
  public PdfTypeStream Stream
  {
    get
    {
      if (this._stream == null || this._stream.IsDisposed)
      {
        IntPtr stream = Pdfium.FPDFFormContent_GetStream(this._content);
        if (stream != IntPtr.Zero)
          this._stream = PdfTypeStream.Create(stream);
      }
      return this._stream;
    }
  }

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the control has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>
  /// Gets the collection of PDF page objects found in the current PdfFormObject.
  /// </summary>
  public PdfPageObjectsCollection PageObjects { get; private set; }

  /// <summary>Check something</summary>
  public bool BackgroundAlphaNeeded => Pdfium.FPDFFormContent_BackgroundAlphaNeeded(this._content);

  /// <summary>
  /// Check that whether the current PdfFormObject is parsed.
  /// </summary>
  public bool IsParsed => Pdfium.FPDFFormContent_IsParsed(this._content);

  /// <summary>Gets transparency type for current PdfPageObject</summary>
  public FormObjectTransparency Transparency
  {
    get => Pdfium.FPDFFormContent_GetTransparency(this._content);
    set => Pdfium.FPDFFormContent_SetTransparency(this._content, value);
  }

  internal PdfFormObject(IntPtr formHandle)
    : base(formHandle)
  {
    this._content = Pdfium.FPDFFormObj_GetFormContent(this.Handle);
    this.PageObjects = new PdfPageObjectsCollection(this);
  }

  /// <summary>Create new instance of PdfFormObject class</summary>
  /// <param name="page">The PDF page in which the form object should be placed.</param>
  /// <returns>New instance of <see cref="T:Patagames.Pdf.Net.PdfFormObject" /></returns>
  public static PdfFormObject Create(PdfPage page)
  {
    IntPtr num = PdfPageObject.CreateObject(PageObjectTypes.PDFPAGE_FORM);
    if (num == IntPtr.Zero)
      return (PdfFormObject) null;
    if (!page.Dictionary.ContainsKey("Resources"))
      page.Dictionary["Resources"] = (PdfTypeBase) PdfTypeDictionary.Create();
    PdfTypeBase pdfTypeBase = page.Dictionary["Resources"];
    PdfTypeStream pdfTypeStream = PdfTypeStream.Create();
    pdfTypeStream.InitEmpty();
    IntPtr formContent = Pdfium.FPDFFormContent_Create(page.Document.Handle, pdfTypeStream.Handle, pdfTypeBase.Handle);
    if (formContent == IntPtr.Zero)
      return (PdfFormObject) null;
    Pdfium.FPDFFormObj_SetFormContent(num, formContent);
    return new PdfFormObject(num)
    {
      _stream = pdfTypeStream
    };
  }

  /// <summary>Releases all resources used by the PdfFormObject.</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfFormObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    if (this.PageObjects != null)
      this.PageObjects.Dispose();
    this.PageObjects = (PdfPageObjectsCollection) null;
    if (this._stream != null)
      this._stream.Dispose();
    this._stream = (PdfTypeStream) null;
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Calculate bounding box</summary>
  public void CalcBoundingBox() => Pdfium.FPDFFormObj_CalcBoundingBox(this.Handle);

  /// <summary>
  /// Transform (scale, rotate, shear, move) <see cref="P:Patagames.Pdf.Net.PdfFormObject.PageObjects" /> collection.
  /// </summary>
  /// <param name="a">The coefficient "a" of the matrix</param>
  /// <param name="b">The coefficient "b" of the matrix</param>
  /// <param name="c">The coefficient "c" of the matrix</param>
  /// <param name="d">The coefficient "d" of the matrix</param>
  /// <param name="e">The coefficient "e" of the matrix</param>
  /// <param name="f">The coefficient "f" of the matrix</param>
  public void TransformPageObjects(float a, float b, float c, float d, float e, float f)
  {
    this.TransformPageObjects(new FS_MATRIX()
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
  /// Transform <see cref="P:Patagames.Pdf.Net.PdfFormObject.PageObjects" /> collection with a specified matrix
  /// </summary>
  /// <param name="matrix">The transform matrix.</param>
  public void TransformPageObjects(FS_MATRIX matrix)
  {
    Pdfium.FPDFFormContent_TransformObjects(this._content, matrix);
  }
}
