// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.DestinationTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represents the type of destintion</summary>
public enum DestinationTypes
{
  /// <summary>
  /// Display the page designated by <see cref="P:Patagames.Pdf.Net.PdfDestination.PageIndex" />, with the coordinates (<see cref="P:Patagames.Pdf.Net.PdfDestination.Left" />, <see cref="P:Patagames.Pdf.Net.PdfDestination.Top" />)
  /// positioned at the upper-left corner of the window and the contents of the page
  /// magnified by the factor <see cref="P:Patagames.Pdf.Net.PdfDestination.Zoom" />.
  /// </summary>
  /// <remarks>
  /// A null value for any of the parameters left, top, or zoom specifies that the current value of that parameter is to be retained unchanged.
  /// A zoom value of 0 has the same meaning as a null value.
  /// </remarks>
  [Description("XYZ")] XYZ,
  /// <summary>
  /// Display the page designated by <see cref="P:Patagames.Pdf.Net.PdfDestination.PageIndex" />, with its contents magnified just enough
  /// to fit the entire page within the window both horizontally and vertically.
  /// </summary>
  /// <remarks>
  /// If the required horizontal and vertical magnification factors are different, use
  /// the smaller of the two, centering the page within the window in the other dimension.
  /// </remarks>
  [Description("Fit")] Fit,
  /// <summary>
  /// Display the page designated by <see cref="P:Patagames.Pdf.Net.PdfDestination.PageIndex" />,
  /// with the vertical coordinate <see cref="P:Patagames.Pdf.Net.PdfDestination.Top" /> positioned
  /// at the top edge of the window and the contents of the page magnified
  /// just enough to fit the entire width of the page within the window.
  /// </summary>
  /// <remarks>
  /// A null value for top specifies that the current value of that parameter is to be retained unchanged.
  /// </remarks>
  [Description("FitH")] FitH,
  /// <summary>
  /// Display the page designated by <see cref="P:Patagames.Pdf.Net.PdfDestination.PageIndex" />,
  /// with the horizontal coordinate <see cref="P:Patagames.Pdf.Net.PdfDestination.Left" /> positioned
  /// at the left edge of the window and the contents of the page magnified
  /// just enough to fit the entire height of the page within the window.
  /// </summary>
  /// <remarks>
  /// A null value for left specifies that the current value of that parameter is to be retained unchanged.
  /// </remarks>
  [Description("FitV")] FitV,
  /// <summary>
  /// Display the page designated by <see cref="P:Patagames.Pdf.Net.PdfDestination.PageIndex" />,
  /// with its contents magnified just enough to fit the rectangle specified
  /// by the coordinates <see cref="P:Patagames.Pdf.Net.PdfDestination.Left" />, <see cref="P:Patagames.Pdf.Net.PdfDestination.Bottom" />,
  /// <see cref="P:Patagames.Pdf.Net.PdfDestination.Right" />, and <see cref="P:Patagames.Pdf.Net.PdfDestination.Top" />
  /// entirely within the window both horizontally and vertically.
  /// </summary>
  /// <remarks>
  /// <para>If the required horizontal and vertical magnification factors are different, use the smaller of
  /// the two, centering the rectangle within the window in the other dimension. </para>
  /// <para>A null value for any of the parameters may result in unpredictable behavior.</para>
  /// </remarks>
  [Description("FitR")] FitR,
  /// <summary>
  /// Display the page designated by <see cref="P:Patagames.Pdf.Net.PdfDestination.PageIndex" />, with its contents magnified
  /// just enough to fit its bounding box entirely within the window both horizontally and vertically.
  /// </summary>
  /// <remarks>
  /// If the required horizontal and vertical magnification
  /// factors are different, use the smaller of the two, centering the bounding box
  /// within the window in the other dimension.
  /// </remarks>
  [Description("FitB")] FitB,
  /// <summary>
  /// Display the page designated by <see cref="P:Patagames.Pdf.Net.PdfDestination.PageIndex" />,
  /// with the vertical coordinate <see cref="P:Patagames.Pdf.Net.PdfDestination.Top" /> positioned
  /// at the top edge of the window and the contents of the page
  /// magnified just enough to fit the entire width of its bounding box within the window.
  /// </summary>
  /// <remarks>
  /// A null value for top specifies that the current value of that parameter is to be retained unchanged.
  /// </remarks>
  [Description("FitBH")] FitBH,
  /// <summary>
  /// Display the page designated by <see cref="P:Patagames.Pdf.Net.PdfDestination.PageIndex" />,
  /// with the horizontal coordinate <see cref="P:Patagames.Pdf.Net.PdfDestination.Left" /> positioned
  /// at the left edge of the window and the contents of the page magnified just enough
  /// to fit the entire height of its bounding box within the window.
  /// </summary>
  /// <remarks>
  /// A null value for left specifies that the current value of that parameter is to be retained unchanged.
  /// </remarks>
  [Description("FitBV")] FitBV,
}
