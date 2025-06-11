// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.InvalidatePageEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for invalidate the client area within the specified rectangle.
/// </summary>
public class InvalidatePageEventArgs : EventArgs
{
  /// <summary>PdfPage object</summary>
  public PdfPage Page { get; private set; }

  /// <summary>Rectangle to invalidate</summary>
  /// <remarks>All positions are measured in PDF "user space". Implementation should call <see cref="O:Patagames.Pdf.Net.PdfPage.Render" /> method for repainting a specified page area.</remarks>
  public FS_RECTF Rect { get; private set; }

  /// <summary>Construct InvalidatePageEventArgs object</summary>
  /// <param name="page">PdfPage object</param>
  /// <param name="rect">Rectangle to invalidate</param>
  public InvalidatePageEventArgs(PdfPage page, FS_RECTF rect)
  {
    this.Page = page;
    this.Rect = rect;
  }
}
