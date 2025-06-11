// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfFuncPostScript
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represent a PostScript calculator function.</summary>
public class PdfFuncPostScript : PdfFunction
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
  /// Gets a PostScript program, represented by an array of bytes.
  /// </summary>
  /// <exception cref="T:System.ObjectDisposedException">The function object has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The underlying <see cref="P:Patagames.Pdf.Net.PdfFuncPostScript.Stream" /> has been disposed; or function <see cref="P:Patagames.Pdf.Net.PdfFunction.Handle" /> is invalid.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownFunctionTypeException">The requested operation could not be performed on a function of this type.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">The data area passed to a system call is too small.</exception>
  public byte[] Program => Pdfium.FPDFFunctionPs_GetProg(this.Handle);

  /// <summary>Create a PostScript calculator function.</summary>
  /// <param name="numOfInputs">The number of input values of a function.</param>
  /// <param name="numOfOutputs">The number of output values of a function.</param>
  /// <param name="domain">The domain of definition of a function.</param>
  /// <param name="range">The range of a function.</param>
  /// <param name="psProg">A PostScript program.</param>
  /// <remarks>
  /// <list type="table">
  /// <item><term><paramref name="domain" /></term><term>Required</term><description>An array of <i>2 × </i><paramref name="numOfInputs" /> numbers. Input values outside the declared domain are clipped to the nearest boundary value.</description></item>
  /// <item><term><paramref name="range" /></term><term>Required</term><description>An array of <i>2 × </i><paramref name="numOfOutputs" /> numbers. Output values outside the declared range are clipped to the nearest boundary value.</description></item>
  /// <item><term><paramref name="psProg" /></term><term>Required</term><description>A PostScript code written in a small subset of the PostScript language.</description></item>
  /// </list>
  /// <note type="note">
  /// This type implements the <see cref="T:System.IDisposable" /> interface. When you have finished using the type,
  /// you should dispose of it either directly or indirectly. To dispose of the type directly, call its <see cref="M:System.IDisposable.Dispose" /> method in a try/catch block.
  /// To dispose of it indirectly, use a language construct such as using (in C#) or Using (in Visual Basic).
  /// </note>
  /// </remarks>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException"><paramref name="domain" />, <paramref name="range" /> or <paramref name="psProg" /> is null; or <paramref name="numOfInputs" /> or <paramref name="numOfOutputs" /> <![CDATA[ <= ]]> 0; or lengh of <paramref name="psProg" /> is zero; or some error in application program.</exception>
  public PdfFuncPostScript(
    int numOfInputs,
    int numOfOutputs,
    float[] domain,
    float[] range,
    byte[] psProg)
    : base(Pdfium.FPDFFunction_CreatePS(numOfInputs, numOfOutputs, domain, range, psProg))
  {
  }

  internal PdfFuncPostScript(IntPtr handle)
    : base(handle)
  {
  }
}
