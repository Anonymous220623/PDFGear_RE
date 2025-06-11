// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfTextInfo
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents a text information of specified text on the page.
/// </summary>
public class PdfTextInfo
{
  /// <summary>
  /// Gets a text string associated with this instance of <see cref="T:Patagames.Pdf.Net.PdfTextInfo" /> class.
  /// </summary>
  /// <remarks>This ignores characters without unicode information</remarks>
  public string Text { get; private set; }

  /// <summary>
  /// Gets a collection of rectangular areas bounding specified text.
  /// </summary>
  public ReadOnlyList<FS_RECTF> Rects { get; private set; }

  /// <summary>Initializes a new instance of the PdfDocument class.</summary>
  /// <param name="textPage">Handle to a text page information structure. Returned by <see cref="M:Patagames.Pdf.Pdfium.FPDFText_LoadPage(System.IntPtr)" /> function.</param>
  /// <param name="index">Index for the start characters.</param>
  /// <param name="count">Number of characters to be extracted.</param>
  internal PdfTextInfo(PdfText textPage, int index, int count)
  {
    this.Text = Pdfium.FPDFText_GetText(textPage.Handle, index, count);
    int num = Pdfium.FPDFText_CountRects(textPage.Handle, index, count);
    this.Rects = new ReadOnlyList<FS_RECTF>();
    for (int rect_index = 0; rect_index < num; ++rect_index)
    {
      double left;
      double top;
      double right;
      double bottom;
      Pdfium.FPDFText_GetRect(textPage.Handle, rect_index, out left, out top, out right, out bottom);
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
