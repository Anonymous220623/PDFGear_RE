// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Exceptions.InvalidFunctionException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Properties;

#nullable disable
namespace Patagames.Pdf.Net.Exceptions;

/// <summary>
///  The exception that is thrown when the underlying Dictionary / Stream is not in the correct format.
/// </summary>
public class InvalidFunctionException : PdfiumException
{
  /// <summary>
  /// Initializes a new instance of the InvalidFunctionException class.
  /// </summary>
  public InvalidFunctionException()
    : base(536871425U /*0x20000201*/, Error.err0066)
  {
  }
}
