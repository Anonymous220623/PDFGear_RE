// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Exceptions.BadFormatException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Properties;

#nullable disable
namespace Patagames.Pdf.Net.Exceptions;

/// <summary>
/// The exception that is thrown when the PDF file is invalid.
/// </summary>
public class BadFormatException : PdfiumException
{
  /// <summary>
  /// Initializes a new instance of the BadFormatException class.
  /// </summary>
  public BadFormatException()
    : base(3U, Error.err0020)
  {
  }
}
