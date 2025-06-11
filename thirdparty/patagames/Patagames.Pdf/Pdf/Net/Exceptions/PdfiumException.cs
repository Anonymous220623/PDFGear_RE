// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Exceptions.PdfiumException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.Exceptions;

/// <summary>Represents errors that occur inside Pdfium SDK.</summary>
public class PdfiumException : Exception
{
  /// <summary>
  /// Gets a code that provide additional information about the exception.
  /// </summary>
  public uint ErrorCode { get; private set; }

  /// <summary>
  /// Initializes a new instance of the PdfiumException class.
  /// </summary>
  /// <param name="errorCode">Error code.</param>
  /// <param name="message">The error message that explains the reason for the exception.</param>
  public PdfiumException(uint errorCode, string message)
    : this(errorCode, message, (Exception) null)
  {
  }

  /// <summary>
  /// Initializes a new instance of the PdfiumException class.
  /// </summary>
  /// <param name="errorCode">Error code</param>
  /// <param name="message">The error message that explains the reason for the exception.</param>
  /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
  public PdfiumException(uint errorCode, string message, Exception innerException)
    : base(message, innerException)
  {
    this.ErrorCode = errorCode;
  }

  /// <summary>
  /// Initializes a new instance of the PdfiumException class.
  /// </summary>
  /// <param name="errorCode">Error code.</param>
  public PdfiumException(uint errorCode) => this.ErrorCode = errorCode;
}
