// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Exceptions.UnknownErrorException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Properties;

#nullable disable
namespace Patagames.Pdf.Net.Exceptions;

/// <summary>
/// Represents a fatal runtime error that occurs inside Pdfium SDK.
/// </summary>
public class UnknownErrorException : PdfiumException
{
  /// <summary>
  /// Initializes a new instance of the UnknownErrorException class.
  /// </summary>
  public UnknownErrorException()
    : base(1U, Error.err0027)
  {
  }

  /// <summary>
  /// Initializes a new instance of the UnknownErrorException class.
  /// </summary>
  /// <param name="message">The error message that explains the reason for the exception.</param>
  public UnknownErrorException(string message)
    : base(1U, message)
  {
  }
}
