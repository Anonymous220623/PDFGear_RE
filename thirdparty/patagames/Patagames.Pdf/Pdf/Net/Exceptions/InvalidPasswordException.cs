// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Exceptions.InvalidPasswordException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Properties;

#nullable disable
namespace Patagames.Pdf.Net.Exceptions;

/// <summary>
/// This exception is commonly raised when the password for a PDF file does not match.
/// </summary>
public class InvalidPasswordException : PdfiumException
{
  /// <summary>
  /// Initializes a new instance of the InvalidPasswordException class.
  /// </summary>
  public InvalidPasswordException()
    : base(4U, Error.err0024)
  {
  }
}
