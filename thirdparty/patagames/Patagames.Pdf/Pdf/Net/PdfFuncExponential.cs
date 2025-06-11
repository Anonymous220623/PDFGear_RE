// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfFuncExponential
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represent an exponential interpolation function.</summary>
/// <remarks>
/// <para>
/// Exponential interpolation functions include a set of parameters that define an <i>exponential interpolation</i> of one input value
/// and <i><see cref="P:Patagames.Pdf.Net.PdfFunction.Outputs" /></i> output values:
/// </para>
/// <para>
/// <i>f(x) = y<sub>0</sub>, ..., y<sub>n-<see cref="P:Patagames.Pdf.Net.PdfFunction.Outputs" /></sub></i>
/// </para>
/// </remarks>
public class PdfFuncExponential : PdfFunction
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

  /// <summary>Gets the interpolation exponent.</summary>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The underlying Dictionary/Stream has been disposed; or function <see cref="P:Patagames.Pdf.Net.PdfFunction.Handle" /> is invalid.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  public float Exponent => Pdfium.FPDFFunctionExp_GetExponent(this.Handle);

  /// <summary>
  /// Get a readonly array of <see cref="P:Patagames.Pdf.Net.PdfFunction.Inputs" /> numbers defining the function result when <i>x = 0.0</i>.
  /// </summary>
  /// <value>An array of numbers; null if any error occurred.</value>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException">There is not enough data to perform the requested operation.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The data area passed to a system call is too small; or the parameter is incorrect.</exception>
  public float[] ValuesAt0 => Pdfium.FPDFFunctionExp_GetValuesArray0(this.Handle);

  /// <summary>
  /// Get a readonly array of <see cref="P:Patagames.Pdf.Net.PdfFunction.Outputs" /> numbers defining the function result when <i>x = 1.0</i>.
  /// </summary>
  /// <value>An array of numbers; null if any error occurred.</value>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException">There is not enough data to perform the requested operation.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The data area passed to a system call is too small; or the parameter is incorrect.</exception>
  public float[] ValuesAt1 => Pdfium.FPDFFunctionExp_GetValuesArray1(this.Handle);

  /// <summary>Create new exponential interpolation function.</summary>
  /// <param name="numOfInputs">The number of input values of a function.</param>
  /// <param name="numOfOutputs">The number of output values of a function.</param>
  /// <param name="exponent">The interpolation exponent.</param>
  /// <param name="domain">The domain of definition of a function.</param>
  /// <param name="range">The range of a function.</param>
  /// <param name="valuesAt0">An array of <paramref name="numOfOutputs" /> numbers defining the function result when <i>x = 0.0</i>.</param>
  /// <param name="valuesAt1">An array of <paramref name="numOfOutputs" /> numbers defining the function result when <i>x = 1.0</i>.</param>
  /// <remarks>
  /// <list type="table">
  /// <item><term><paramref name="exponent" /></term><term>Required</term><description>An interpolation exponent. Each input value <i>x</i> will return <paramref name="numOfOutputs" /> values, given by <i>y<sub>j</sub> = <paramref name="valuesAt0" /><sub>j</sub> + x<sup><paramref name="exponent" /></sup> × (<paramref name="valuesAt1" /><sub>j</sub> - <paramref name="valuesAt0" /><sub>j</sub>)</i>, for <i>0 ≤ j<![CDATA[ < ]]><paramref name="numOfOutputs" /></i>.</description></item>
  /// <item><term><paramref name="domain" /></term><term>Required</term><description>An array of <i>2 × </i><paramref name="numOfInputs" /> numbers. Input values outside the declared domain are clipped to the nearest boundary value.</description></item>
  /// <item><term><paramref name="range" /></term><term>Optional</term><description>An array of <i>2 × </i><paramref name="numOfOutputs" /> numbers. Output values outside the declared range are clipped to the nearest boundary value. If absent, no clipping is done.</description></item>
  /// <item><term><paramref name="valuesAt0" /></term><term>Optional</term><description>An array of <paramref name="numOfOutputs" /> numbers defining the function result when <i>x = 0.0</i>. Default value: <i>[0.0]</i></description></item>
  /// <item><term><paramref name="valuesAt1" /></term><term>Optional</term><description>An array of <paramref name="numOfOutputs" /> numbers defining the function result when <i>x = 1.0</i>. Default value: <i>[1.0]</i></description></item>
  /// </list>
  /// <para>
  /// Values of <paramref name="domain" /> must constrain <i>x</i> in such a way that if <paramref name="exponent" /> is not an integer, all
  /// values of <i>x</i> must be non-negative, and if <paramref name="exponent" /> is negative, no value of <i>x</i> may be zero.
  /// Typically, <paramref name="domain" /> is declared as <i>[0.0 1.0]</i>, and <paramref name="exponent" /> is a positive number.
  /// The <paramref name="range" /> attribute is optional and can be used to clip the output to a specified range.
  /// </para>
  /// <para>
  /// Note that when <paramref name="exponent" /> is <i>1</i>, the function performs a linear interpolation between <paramref name="valuesAt0" /> and <paramref name="valuesAt1" />;
  /// therefore, the function can also be expressed as a sampled function.
  /// </para>
  /// <note type="note">
  /// This type implements the <see cref="T:System.IDisposable" /> interface. When you have finished using the type,
  /// you should dispose of it either directly or indirectly. To dispose of the type directly, call its <see cref="M:System.IDisposable.Dispose" /> method in a try/catch block.
  /// To dispose of it indirectly, use a language construct such as using (in C#) or Using (in Visual Basic).
  /// </note>
  /// </remarks>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException"><see cref="P:Patagames.Pdf.Net.PdfFuncExponential.Dictionary" /><paramref name="domain" /> is null; or <paramref name="numOfInputs" /> <![CDATA[ <= ]]> 0; or some error in application program.</exception>
  public PdfFuncExponential(
    int numOfInputs,
    int numOfOutputs,
    float exponent,
    float[] domain,
    float[] range = null,
    float[] valuesAt0 = null,
    float[] valuesAt1 = null)
    : base(Pdfium.FPDFFunction_CreateExp(numOfInputs, numOfOutputs, exponent, domain, range, valuesAt0, valuesAt1))
  {
  }

  internal PdfFuncExponential(IntPtr handle)
    : base(handle)
  {
  }
}
