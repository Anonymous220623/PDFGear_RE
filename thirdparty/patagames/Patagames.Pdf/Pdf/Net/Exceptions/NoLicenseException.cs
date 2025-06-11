// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Exceptions.NoLicenseException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Properties;

#nullable disable
namespace Patagames.Pdf.Net.Exceptions;

/// <summary>The exception that is thrown only in trial mode</summary>
public class NoLicenseException : PdfiumException
{
  /// <summary>
  /// Initializes a new instance of the NoLicenseException class.
  /// </summary>
  /// <param name="errorCode">Error code.</param>
  public NoLicenseException(uint errorCode)
    : base(errorCode, Error.err0025)
  {
  }

  /// <summary>
  /// Initializes a new instance of the NoLicenseException class.
  /// </summary>
  /// <param name="message">The error message that explains the reason for the exception.</param>
  /// <param name="errorCode">Error code.</param>
  public NoLicenseException(uint errorCode, string message)
    : base(errorCode, $"{Error.err0025} {message}")
  {
  }

  /// <summary>
  /// Initializes a new instance of the NoLicenseException class.
  /// </summary>
  /// <param name="message">The error message that explains the reason for the exception.</param>
  public NoLicenseException(string message)
    : this(1001U, message)
  {
  }
}
