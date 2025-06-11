// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfFuncStitching
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represent a stitching function.</summary>
public class PdfFuncStitching : PdfFunction
{
  private PdfTypeDictionary _dict;

  /// <summary>Gets the dictionary of the function.</summary>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  public PdfTypeDictionary Dictionary
  {
    get
    {
      if (this._dict == null)
        this._dict = new PdfTypeDictionary(Pdfium.FPDFFunction_GetObject(this.Handle));
      return this._dict;
    }
  }

  /// <summary>Gets a bounds array.</summary>
  /// <value>A readonly array of numbers; null if any error occurred.</value>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException">There is not enough data to perform the requested operation.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The data area passed to a system call is too small; or the parameter is incorrect.</exception>
  public float[] Bounds => Pdfium.FPDFFunctionStitch_GetBoundsArray(this.Handle);

  /// <summary>Gets an encode array.</summary>
  /// <value>A readonly array of numbers; null if any error occurred.</value>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException">There is not enough data to perform the requested operation.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The data area passed to a system call is too small; or the parameter is incorrect.</exception>
  public float[] Encode => Pdfium.FPDFFunctionStitch_GetEncodeArray(this.Handle);

  /// <summary>Gets a functions array.</summary>
  /// <value>A readonly array of functions making up the stitching function.</value>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The underlying <see cref="P:Patagames.Pdf.Net.PdfFuncStitching.Dictionary" /> has been disposed; or function <see cref="P:Patagames.Pdf.Net.PdfFunction.Handle" /> is invalid.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.InvalidFunctionException">The underlying Dictionary/Stream is not in the correct format.</exception>
  public PdfFunction[] Functions
  {
    get
    {
      int functionsCount = Pdfium.FPDFFunctionStitch_GetFunctionsCount(this.Handle);
      PdfFunction[] functions = functionsCount >= 0 ? new PdfFunction[functionsCount] : throw Pdfium.ProcessLastError();
      for (int index = 0; index < functionsCount; ++index)
        functions[index] = PdfFunction.FromHandle(Pdfium.FPDFFunctionStitch_GetFunctionAt(this.Handle, index));
      return functions;
    }
  }

  /// <summary>
  /// Create a stitching of the subdomains of several 1-input functions to produce a single new 1-input function.
  /// </summary>
  /// <param name="doc">PDF document.</param>
  /// <param name="numOfOutputs">The number of output values of a function.</param>
  /// <param name="functions">An array of <i>k 1</i>-input functions making up the stitching function.</param>
  /// <param name="encode">An array of <i>2 × k</i> numbers that, taken in pairs, map each subset of the domain defined by <paramref name="domain" /> and the <paramref name="bounds" /> array to the domain of the corresponding function.</param>
  /// <param name="bounds">An array of <i>k − 1</i> numbers that, in combination with <paramref name="domain" />, define the intervals to which each function from the <paramref name="functions" /> array applies.</param>
  /// <param name="domain">The domain of definition of a function.</param>
  /// <param name="range">The range of a function.</param>
  /// <remarks>
  /// <list type="table">
  /// <item><term><paramref name="domain" /></term><term>Required</term><description>Since the resulting stitching function is a <i>1</i>-input function, the <paramref name="domain" /> is given by a two-element array.</description></item>
  /// <item><term><paramref name="range" /></term><term>Optional</term><description>An array of <i>2 ×</i> <paramref name="numOfOutputs" /> numbers. Output values outside the declared range are clipped to the nearest boundary value. If absent, no clipping is done.</description></item>
  /// <item><term><paramref name="functions" /></term><term>Required</term><description>An array of <i>k - 1</i> functions making up the stitching function. The output dimensionality of all functions must be the same, and compatible with the value of <paramref name="range" /> if <paramref name="range" /> is present.</description></item>
  /// <item><term><paramref name="bounds" /></term><term>Required</term><description>An array of <i>k − 1</i> numbers that, in combination with <paramref name="domain" />, define the intervals to which each function from the <paramref name="functions" /> array applies. <paramref name="bounds" /> elements must be in order of increasing value, and each value must be within the domain defined by <paramref name="domain" />.</description></item>
  /// <item><term><paramref name="encode" /></term><term>Required</term><description>An array of <i>2 × k</i> numbers that, taken in pairs, map each subset of the domain defined by <paramref name="domain" /> and the <paramref name="bounds" /> array to the domain of the corresponding function.</description></item>
  /// </list>
  /// <note type="note">
  /// This type implements the <see cref="T:System.IDisposable" /> interface. When you have finished using the type,
  /// you should dispose of it either directly or indirectly. To dispose of the type directly, call its <see cref="M:System.IDisposable.Dispose" /> method in a try/catch block.
  /// To dispose of it indirectly, use a language construct such as using (in C#) or Using (in Visual Basic).
  /// </note>
  /// </remarks>
  /// <exception cref="T:System.ArgumentNullException">Argument is null.</exception>
  /// <exception cref="T:System.ArgumentException">The <paramref name="functions" /> array contains null.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException"><paramref name="domain" />, <paramref name="encode" /> or <paramref name="bounds" /> is null or invalid.</exception>
  public PdfFuncStitching(
    PdfDocument doc,
    int numOfOutputs,
    PdfFunction[] functions,
    float[] encode,
    float[] bounds,
    float[] domain,
    float[] range = null)
    : base(PdfFuncStitching.CreateStiching(doc, numOfOutputs, functions, encode, bounds, domain, range))
  {
  }

  internal PdfFuncStitching(IntPtr handle)
    : base(handle)
  {
  }

  private static IntPtr CreateStiching(
    PdfDocument doc,
    int numOfOutputs,
    PdfFunction[] functions,
    float[] encode,
    float[] bounds,
    float[] domain,
    float[] range)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc));
    if (functions == null)
      throw new ArgumentNullException(nameof (functions));
    if (encode == null)
      throw new ArgumentNullException(nameof (encode));
    if (bounds == null)
      throw new ArgumentNullException(nameof (bounds));
    if (domain == null)
      throw new ArgumentNullException(nameof (domain));
    IntPtr[] functions1 = new IntPtr[functions.Length];
    for (int index = 0; index < functions.Length; ++index)
    {
      if (functions[index] == null)
        throw new ArgumentException(Error.err0063);
      functions1[index] = functions[index].Handle;
    }
    return Pdfium.FPDFFunction_CreateStitch(doc.Handle, numOfOutputs, functions1, encode, bounds, domain, range);
  }
}
