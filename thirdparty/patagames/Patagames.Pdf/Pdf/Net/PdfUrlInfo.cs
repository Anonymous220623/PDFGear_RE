// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfUrlInfo
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents a url information of specified web link on the page.
/// </summary>
public class PdfUrlInfo
{
  /// <summary>
  /// Gets a url associated with this instance of <see cref="T:Patagames.Pdf.Net.PdfUrlInfo" /> class.
  /// </summary>
  /// <remarks>This ignores characters without unicode information</remarks>
  public string Url { get; private set; }

  /// <summary>
  /// Gets a collection of rectangular areas bounding specified url.
  /// </summary>
  public ReadOnlyList<FS_RECTF> Rects { get; private set; }

  /// <summary>Initializes a new instance of the PdfDocument class.</summary>
  /// <param name="webLinkHandle">Handle to the <see cref="T:Patagames.Pdf.Net.PdfWebLink" /></param>
  /// <param name="index">Index for the web link.</param>
  internal PdfUrlInfo(IntPtr webLinkHandle, int index)
  {
    this.Rects = new ReadOnlyList<FS_RECTF>();
    this.Url = Pdfium.FPDFLink_GetURL(webLinkHandle, index);
    int num = Pdfium.FPDFLink_CountRects(webLinkHandle, index);
    for (int rect_index = 0; rect_index < num; ++rect_index)
    {
      double left;
      double top;
      double right;
      double bottom;
      Pdfium.FPDFLink_GetRect(webLinkHandle, index, rect_index, out left, out top, out right, out bottom);
      this.Rects.Add(new FS_RECTF()
      {
        bottom = (float) bottom,
        top = (float) top,
        right = (float) right,
        left = (float) left
      });
    }
  }
}
