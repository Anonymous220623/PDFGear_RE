// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Net.Exceptions;

/// <summary>
/// The exception that is thrown when the required data is absent.
/// </summary>
public class RequiredDataIsAbsentException : PdfiumException
{
  /// <summary>
  /// Initializes a new instance of the RequiredDataIsAbsentException class.
  /// </summary>
  public RequiredDataIsAbsentException()
    : base(536871427U /*0x20000203*/)
  {
  }

  /// <summary>
  /// Initializes a new instance of the RequiredDataIsAbsentException class.
  /// </summary>
  /// <param name="message">Exception message.</param>
  public RequiredDataIsAbsentException(string message)
    : base(536871427U /*0x20000203*/, message)
  {
  }
}
