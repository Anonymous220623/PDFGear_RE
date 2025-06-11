// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfImageMask
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing.Imaging;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfImageMask : PdfMask
{
  private PdfBitmap m_imageMask;
  private bool m_softMask;

  public PdfBitmap Mask => this.m_imageMask;

  public bool SoftMask => this.m_softMask;

  public PdfImageMask(PdfBitmap imageMask)
  {
    if (imageMask == null)
      throw new ArgumentNullException(nameof (imageMask));
    switch (imageMask.InternalImage.PixelFormat)
    {
      case PixelFormat.Format1bppIndexed:
        this.m_softMask = false;
        break;
      case PixelFormat.Format8bppIndexed:
        this.m_softMask = true;
        break;
      default:
        throw new ArgumentException(nameof (imageMask), "Image mask should be gray scale or black and white.");
    }
    this.m_imageMask = imageMask;
  }
}
