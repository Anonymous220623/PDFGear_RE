// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Exceptions.ImageObjectIsEmptyException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Properties;

#nullable disable
namespace Patagames.Pdf.Net.Exceptions;

/// <summary>
/// The exception that is thrown when the PdfImageObject don't contain PdfBitmap
/// </summary>
public class ImageObjectIsEmptyException : PdfiumException
{
  /// <summary>
  /// Initializes a new instance of the ImageObjectIsEmptyException class.
  /// </summary>
  public ImageObjectIsEmptyException()
    : base(536870913U /*0x20000001*/, Error.err0023)
  {
  }
}
