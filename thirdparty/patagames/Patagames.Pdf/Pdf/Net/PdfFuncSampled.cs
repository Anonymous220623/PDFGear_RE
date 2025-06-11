// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfFuncSampled
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represent sampled function.</summary>
public class PdfFuncSampled : PdfFunction
{
  private PdfTypeStream _stream;

  /// <summary>Gets the stream of the function.</summary>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  public PdfTypeStream Stream
  {
    get
    {
      if (this._stream == null)
        this._stream = new PdfTypeStream(Pdfium.FPDFFunction_GetObject(this.Handle));
      return this._stream;
    }
  }

  /// <summary>
  /// The number of samples in each input dimension of the sample table.
  /// </summary>
  /// <value>A readonly array of numbers; null if any error occurred.</value>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException">There is not enough data to perform the requested operation.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The data area passed to a system call is too small; or the parameter is incorrect.</exception>
  public int[] NumOfSamples => Pdfium.FPDFFunctionSampled_GetNumOfSamlesArray(this.Handle);

  /// <summary>Gets an encode array.</summary>
  /// <value>A readonly array of numbers; null if any error occurred.</value>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException">There is not enough data to perform the requested operation.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The data area passed to a system call is too small; or the parameter is incorrect.</exception>
  public float[] Encode => Pdfium.FPDFFunctionSampled_GetEncodeArray(this.Handle);

  /// <summary>Gets a decode array.</summary>
  /// <value>A readonly array of numbers; null if any error occurred.</value>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException">There is not enough data to perform the requested operation.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The data area passed to a system call is too small; or the parameter is incorrect.</exception>
  public float[] Decode => Pdfium.FPDFFunctionSampled_GetDecodeArray(this.Handle);

  /// <summary>Gets a samples table.</summary>
  /// <value>A readonly array of bytes; null if any error occurred.</value>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException">There is not enough data to perform the requested operation.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The data area passed to a system call is too small; or the parameter is incorrect.</exception>
  public byte[] Samples => Pdfium.FPDFFunctionSampled_GetSamplesTable(this.Handle);

  /// <summary>
  /// Gets the number of bits used to represent each sample.
  /// </summary>
  /// <value>Valid values are 1, 2, 4, 8, 12, 16, 24, and 32; -1 if any error occured.</value>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The underlying <see cref="P:Patagames.Pdf.Net.PdfFuncSampled.Stream" /> has been disposed; or function <see cref="P:Patagames.Pdf.Net.PdfFunction.Handle" /> is invalid.</exception>
  public int Bps => Pdfium.FPDFFunctionSampled_GetBitsPerSample(this.Handle);

  /// <summary>Gets the order of interpolation between samples.</summary>
  /// <value>Valid values are 1 and 3, specifying linear and cubic spline interpolation, respectively; -1 if any error occured.</value>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The underlying <see cref="P:Patagames.Pdf.Net.PdfFuncSampled.Stream" /> has been disposed; or function <see cref="P:Patagames.Pdf.Net.PdfFunction.Handle" /> is invalid.</exception>
  public int Order => Pdfium.FPDFFunctionSampled_GetOrder(this.Handle);

  /// <summary>Create sampled function.</summary>
  /// <param name="numOfInputs">The number of input values of a function.</param>
  /// <param name="numOfOutputs">The number of output values of a function.</param>
  /// <param name="domain">The domain of definition of a function.</param>
  /// <param name="range">The range of a function.</param>
  /// <param name="numOfSamples">The number of samples in each input dimension of the sample table.</param>
  /// <param name="bitsPerSample">The number of bits used to represent each sample.</param>
  /// <param name="sampleTable">A sequence of sample values which are organized as an <paramref name="numOfInputs" /> - dimensional table.</param>
  /// <param name="encode">The linear mapping of input values into the domain of the function’s sample table.</param>
  /// <param name="decode">The linear mapping of sample values into the range appropriate for the function’s output values.</param>
  /// <param name="order">The order of interpolation between samples.</param>
  /// <remarks>
  /// <list type="table">
  /// <item><term><paramref name="domain" /></term><term>Required</term><description>An array of <i>2 ×</i> <paramref name="numOfInputs" /> numbers. Input values outside the declared domain are clipped to the nearest boundary value.</description></item>
  /// <item><term><paramref name="range" /></term><term>Required</term><description>An array of <i>2 ×</i> <paramref name="numOfOutputs" /> numbers. Output values outside the declared range are clipped to the nearest boundary value.</description></item>
  /// <item><term><paramref name="numOfSamples" /></term><term>Required</term><description>An array of <paramref name="numOfInputs" /> positive integers specifying the number of samples in each input dimension of the sample table.</description></item>
  /// <item><term><paramref name="bitsPerSample" /></term><term>Required</term><description>A number of bits used to represent each sample. (If the function has multiple output values, each one occupies BitsPerSample bits.) Valid values are 1, 2, 4, 8, 12, 16, 24, and 32.</description></item>
  /// <item><term><paramref name="encode" /></term><term>Optional</term><description>An array of <i>2 ×</i> <paramref name="numOfInputs" /> numbers specifying the linear mapping of input values into the domain of the function’s sample table.</description></item>
  /// <item><term><paramref name="decode" /></term><term>Optional</term><description>An array of <i>2 ×</i> <paramref name="numOfOutputs" /> numbers specifying the linear mapping of sample values into the range appropriate for the function’s output values.</description></item>
  /// <item><term><paramref name="order" /></term><term>Optional</term><description>The order of interpolation between samples. Valid values are 1 and 3, specifying linear and cubic spline interpolation, respectively.</description></item>
  /// </list>
  /// <note type="note">
  /// This type implements the <see cref="T:System.IDisposable" /> interface. When you have finished using the type,
  /// you should dispose of it either directly or indirectly. To dispose of the type directly, call its <see cref="M:System.IDisposable.Dispose" /> method in a try/catch block.
  /// To dispose of it indirectly, use a language construct such as using (in C#) or Using (in Visual Basic).
  /// </note>
  /// </remarks>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">One or more of the passed parameters are invalid.</exception>
  public PdfFuncSampled(
    int numOfInputs,
    int numOfOutputs,
    float[] domain,
    float[] range,
    int[] numOfSamples,
    int bitsPerSample,
    byte[] sampleTable,
    float[] encode = null,
    float[] decode = null,
    int order = 0)
    : base(Pdfium.FPDFFunction_CreateSampled(numOfInputs, numOfOutputs, domain, range, numOfSamples, bitsPerSample, sampleTable, encode, decode, order))
  {
  }

  internal PdfFuncSampled(IntPtr handle)
    : base(handle)
  {
  }
}
