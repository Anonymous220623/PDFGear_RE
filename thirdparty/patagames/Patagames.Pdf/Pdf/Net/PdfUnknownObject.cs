// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfUnknownObject
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents a page object with unsupported type.</summary>
public class PdfUnknownObject : PdfPageObject
{
  /// <summary>
  /// Initializes a new instance of the PdfUnknownObject class.
  /// </summary>
  /// <param name="handle">The Pdfium SDK handle that the new object will be bound to</param>
  internal PdfUnknownObject(IntPtr handle)
    : base(handle)
  {
  }
}
