// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Exceptions.LicenseKeyIsExpiredException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Net.Exceptions;

/// <summary>
/// The exception that is thrown when the key provided to a initialization method is expired.
/// </summary>
public class LicenseKeyIsExpiredException : PdfiumException
{
  /// <summary>
  /// Initializes a new instance of the LicenseKeyIsExpiredException class.
  /// </summary>
  /// <remarks>
  /// <para>This usually indicates that the trial key has expired. The regular license key never expires.</para>
  /// </remarks>
  public LicenseKeyIsExpiredException()
    : base(536871169U /*0x20000101*/)
  {
  }
}
