// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfFunction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Base class of function objects that represent parameterized classes of functions,
/// including mathematical formulas and sampled representations with arbitrary resolution.
/// </summary>
public abstract class PdfFunction : IDisposable
{
  private IntPtr _handle;

  /// <summary>
  /// Gets the Pdfium SDK handle that the function object is bound to.
  /// </summary>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  public IntPtr Handle
  {
    get
    {
      if (this.IsDisposed)
        throw new ObjectDisposedException((string) null);
      return this._handle;
    }
  }

  /// <summary>
  /// Gets a value indicating whether the function object has been disposed of.
  /// </summary>
  /// <value>true if function object has been disposed of; otherwise, false.</value>
  public bool IsDisposed { get; private set; }

  /// <summary>Gets the function type.</summary>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.InvalidFunctionException">The underlying Dictionary/Stream is not in the correct format.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The underlying Dictionary/Stream has been disposed; or function <see cref="P:Patagames.Pdf.Net.PdfFunction.Handle" /> is invalid.</exception>
  public FunctionTypes FunctionType => Pdfium.FPDFFunction_GetType(this.Handle);

  /// <summary>Gets the number of input values.</summary>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  public int Inputs => Pdfium.FPDFFunction_GetCountInput(this.Handle);

  /// <summary>Gets the number of output values.</summary>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.InvalidFunctionException">The underlying Dictionary/Stream is not in the correct format.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException">There is not enough data to perform the requested operation.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The underlying Dictionary/Stream has been disposed; or function <see cref="P:Patagames.Pdf.Net.PdfFunction.Handle" /> is invalid.</exception>
  public int Outputs => Pdfium.FPDFFunction_GetCountOutput(this.Handle);

  /// <summary>Get the domain of the function.</summary>
  /// <value>A readonly array of <i>2 × <see cref="P:Patagames.Pdf.Net.PdfFunction.Inputs" /></i> numbers; null if any error occurred.</value>
  /// <remarks>
  /// For each <i>i</i> from <i>0</i> to <i><see cref="P:Patagames.Pdf.Net.PdfFunction.Inputs" /> − 1</i>, <i>Domain<sub>2i</sub></i> must be less than or equal to
  /// <i>Domain<sub>2i+1</sub></i>, and the <i>i</i>th input value, <i>x<sub>i</sub></i>, must lie in the interval <i>Domain<sub>2i</sub> ≤ x<sub>i</sub> ≤ Domain<sub>2i+1</sub></i>.
  /// Input values outside the declared domain are clipped to the nearest boundary value.
  /// </remarks>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException">There is not enough data to perform the requested operation.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The data area passed to a system call is too small; or the parameter is incorrect.</exception>
  public float[] Domain => Pdfium.FPDFFunction_GetDomainArray(this.Handle);

  /// <summary>Get the range of the function.</summary>
  /// <value>A readonly array of <i>2 × <see cref="P:Patagames.Pdf.Net.PdfFunction.Outputs" /></i> numbers; null if any error occurred.</value>
  /// <remarks>
  /// For each <i>j</i> from <i>0</i> to <i><see cref="P:Patagames.Pdf.Net.PdfFunction.Outputs" /> − 1</i>, <i>Range<sub>2j</sub></i> must be less than or equal to <i>Range<sub>2j+1</sub></i>,
  /// and the <i>j</i>th output value, <i>y<sub>j</sub></i>, must lie in the interval
  /// <i>Range<sub>2j</sub> ≤ y<sub>j</sub> ≤ Range<sub>2j+1</sub></i>.
  /// Output values outside the declared range are clipped to the nearest boundary value. If this entry is absent, no clipping is done.
  /// </remarks>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException">There is not enough data to perform the requested operation.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The data area passed to a system call is too small; or the parameter is incorrect.</exception>
  public float[] Range => Pdfium.FPDFFunction_GetRangeArray(this.Handle);

  /// <summary>Initialize new instance of PdfFunction class</summary>
  /// <param name="handle">The Pdfium SDK handle that the function object is bound to.</param>
  protected PdfFunction(IntPtr handle)
  {
    this._handle = handle;
    if (this._handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
  }

  /// <summary>
  /// Identifies the type of the specified function object and creates an instance of this type.
  /// </summary>
  /// <param name="handle">A handle to a function object.</param>
  /// <returns>An instance of a particular class of a function object.</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The <paramref name="handle" /> points to a function of unknown type.</exception>
  public static PdfFunction FromHandle(IntPtr handle)
  {
    if (handle == IntPtr.Zero)
      throw new ArgumentException(string.Format(Error.err0064, (object) nameof (handle), (object) "IntPtr.Zero"));
    switch (Pdfium.FPDFFunction_GetType(handle))
    {
      case FunctionTypes.Sampled:
        return (PdfFunction) new PdfFuncSampled(handle);
      case FunctionTypes.Exponential:
        return (PdfFunction) new PdfFuncExponential(handle);
      case FunctionTypes.Stitching:
        return (PdfFunction) new PdfFuncStitching(handle);
      case FunctionTypes.PostScript:
        return (PdfFunction) new PdfFuncPostScript(handle);
      default:
        throw new UnknownFunctionTypeException();
    }
  }

  /// <summary>
  /// Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
  /// </summary>
  ~PdfFunction() => this.Dispose(false);

  /// <summary>Releases all resources used by the object.</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the object.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize || this._handle == IntPtr.Zero)
      return;
    if (this._handle != IntPtr.Zero)
      Pdfium.FPDFFunction_CloseHandle(this._handle);
    this._handle = IntPtr.Zero;
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }
}
