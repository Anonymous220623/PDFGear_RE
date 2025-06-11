// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfWatermarkAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents a watermark annotation</summary>
/// <remarks>
/// <para>
/// A watermark annotation (PDF 1.6) is used to represent graphics that are expected
/// to be printed at a fixed size and position on a page, regardless of the dimensions
/// of the printed page.
/// </para>
/// <para>
/// Watermark annotations have no pop-up window or other interactive elements.
/// When displaying a watermark annotation on-screen, viewer applications should
/// use the dimensions of the media box as the page size so that the scroll and zoom
/// behavior is the same as for other annotations.
/// </para>
/// <note type="note">
/// Since many printing devices have nonprintable margins, it is recommended
/// that such margins be taken into consideration when positioning watermark annotations near the edge of a page.
/// </note>
/// </remarks>
public class PdfWatermarkAnnotation : PdfAnnotation
{
  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfWatermarkAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfWatermarkAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Watermark");
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfWatermarkAnnotation" /> with text object as specified.
  /// </summary>
  /// <param name="page">The <see cref="T:Patagames.Pdf.Net.PdfPage" /> where watremark must be placed.</param>
  /// <param name="text">The <see cref="T:Patagames.Pdf.Net.PdfTextObject" /> that will be used as a watermark.</param>
  /// <param name="alignment">Align the watermark on the page.</param>
  /// <param name="vDistance">Set the vertical distance between the target page and the watermark.</param>
  /// <param name="hDistance">Set the horizontal distance between the target page and the watermark.</param>
  /// <param name="rotation">The rotation degree.</param>
  /// <param name="scale">Absolute scale.</param>
  /// <param name="showOnPrint">Show watermark when ptinting.</param>
  /// <param name="showOnDisplay">Show watermark when displaying on screen.</param>
  public PdfWatermarkAnnotation(
    PdfPage page,
    PdfTextObject text,
    PdfContentAlignment alignment = PdfContentAlignment.MiddleCenter,
    float vDistance = 0.0f,
    float hDistance = 0.0f,
    float rotation = 0.0f,
    float scale = 1f,
    bool showOnPrint = true,
    bool showOnDisplay = true)
    : this(page)
  {
    this.Contents = text.TextUnicode;
    FS_MATRIX fsMatrix = text.Matrix ?? new FS_MATRIX(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    fsMatrix.Scale(scale, scale);
    fsMatrix.Rotate((float) ((double) rotation * 3.1400001049041748 / 180.0));
    text.Matrix = fsMatrix;
    this.Create(page, (PdfPageObject) text, showOnPrint, showOnDisplay, vDistance, hDistance, rotation, scale, alignment);
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfWatermarkAnnotation" /> with image object as specified.
  /// </summary>
  /// <param name="page">The <see cref="T:Patagames.Pdf.Net.PdfPage" /> where watremark must be placed.</param>
  /// <param name="image">The <see cref="T:Patagames.Pdf.Net.PdfImageObject" /> that will be used as a watermark.</param>
  /// <param name="alignment">Align the watermark on the page.</param>
  /// <param name="vDistance">Set the vertical distance between the target page and the watermark.</param>
  /// <param name="hDistance">Set the horizontal distance between the target page and the watermark.</param>
  /// <param name="rotation">The rotation degree.</param>
  /// <param name="scale">Absolute scale.</param>
  /// <param name="showOnPrint">Show watermark when ptinting.</param>
  /// <param name="showOnDisplay">Show watermark when displaying on screen.</param>
  public PdfWatermarkAnnotation(
    PdfPage page,
    PdfImageObject image,
    PdfContentAlignment alignment = PdfContentAlignment.MiddleCenter,
    float vDistance = 0.0f,
    float hDistance = 0.0f,
    float rotation = 0.0f,
    float scale = 1f,
    bool showOnPrint = true,
    bool showOnDisplay = true)
    : this(page)
  {
    FS_MATRIX fsMatrix = image.Matrix ?? new FS_MATRIX((float) image.Bitmap.Width, 0.0f, 0.0f, (float) image.Bitmap.Height, 0.0f, 0.0f);
    fsMatrix.Scale(scale, scale);
    fsMatrix.Rotate((float) ((double) rotation * 3.1400001049041748 / 180.0));
    image.Matrix = fsMatrix;
    this.Create(page, (PdfPageObject) image, showOnPrint, showOnDisplay, vDistance, hDistance, rotation, scale, alignment);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfWatermarkAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfWatermarkAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }

  private void Create(
    PdfPage page,
    PdfPageObject obj,
    bool showOnPrint,
    bool showOnDisplay,
    float vDistance,
    float hDistance,
    float rotation,
    float scale,
    PdfContentAlignment alignment)
  {
    this.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    this.NormalAppearance.Add(obj);
    if (showOnPrint)
      this.Flags = AnnotationFlags.Print;
    if (!showOnDisplay)
      this.Flags |= AnnotationFlags.NoView;
    float width = page.Width;
    float height = page.Height;
    float left;
    float top;
    float right;
    float bottom;
    Pdfium.FPDFPageObj_GetBBox(obj.Handle, (FS_MATRIX) null, out left, out top, out right, out bottom);
    int num1 = 0;
    int num2 = 0;
    switch (alignment)
    {
      case PdfContentAlignment.TopLeft:
        num2 = 1;
        num1 = 0;
        break;
      case PdfContentAlignment.TopCenter:
        num2 = 1;
        num1 = 2;
        break;
      case PdfContentAlignment.TopRight:
        num2 = 1;
        num1 = 1;
        break;
      case PdfContentAlignment.MiddleLeft:
        num2 = 2;
        num1 = 0;
        break;
      case PdfContentAlignment.MiddleCenter:
        num2 = 2;
        num1 = 2;
        break;
      case PdfContentAlignment.MiddleRight:
        num2 = 2;
        num1 = 1;
        break;
      case PdfContentAlignment.BottomLeft:
        num2 = 0;
        num1 = 0;
        break;
      case PdfContentAlignment.BottomCenter:
        num2 = 0;
        num1 = 2;
        break;
      case PdfContentAlignment.BottomRight:
        num2 = 0;
        num1 = 1;
        break;
    }
    float num3 = num1 == 0 ? 0.0f : (width - right + left) / (float) num1;
    float num4 = num2 == 0 ? 0.0f : (height - top + bottom) / (float) num2;
    float l = num3 + hDistance;
    float b = num4 + vDistance;
    this.GenerateAppearance(AppearanceStreamModes.Normal);
    this.Rectangle = new FS_RECTF(l, b + (top - bottom), l + (right - left), b);
  }
}
