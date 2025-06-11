// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.PdfPageEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain PdfPage as event data.
/// </summary>
public class PdfPageEventArgs : EventArgs
{
  /// <summary>The page</summary>
  public PdfPage Page { get; private set; }

  /// <summary>
  /// Construct <see cref="T:Patagames.Pdf.Net.EventArguments.PdfPageEventArgs" /> object
  /// </summary>
  /// <param name="page">The page</param>
  public PdfPageEventArgs(PdfPage page) => this.Page = page;
}
