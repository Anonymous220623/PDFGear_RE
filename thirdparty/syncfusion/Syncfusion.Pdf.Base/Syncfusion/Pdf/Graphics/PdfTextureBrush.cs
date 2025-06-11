// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfTextureBrush
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class PdfTextureBrush : PdfTilingBrush
{
  private PdfImage m_textureImage;
  private RectangleF m_imageBounds = RectangleF.Empty;

  public PdfTextureBrush(PdfImage image, RectangleF dstRect, float transparency)
    : base(dstRect)
  {
    this.m_textureImage = image;
    this.m_imageBounds = dstRect;
    PdfGraphicsState state = this.Graphics.Save();
    this.Graphics.SetTransparency(transparency);
    this.Graphics.DrawImage(this.m_textureImage, 0.0f, 0.0f, dstRect.Width, dstRect.Height);
    this.Graphics.Restore(state);
  }
}
