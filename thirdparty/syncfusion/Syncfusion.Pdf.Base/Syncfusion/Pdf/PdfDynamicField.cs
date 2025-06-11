// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfDynamicField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public abstract class PdfDynamicField : PdfAutomaticField
{
  public PdfDynamicField()
  {
  }

  public PdfDynamicField(PdfFont font)
    : base(font)
  {
  }

  public PdfDynamicField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfDynamicField(PdfFont font, RectangleF bounds)
    : base(font, bounds)
  {
  }

  internal static PdfPage GetPageFromGraphics(PdfGraphics graphics)
  {
    if (!(graphics.Page is PdfPage page))
      throw new NotSupportedException("The field was placed on not PdfPage class instance.");
    return page;
  }

  internal static PdfLoadedPage GetLoadedPageFromGraphics(PdfGraphics graphics)
  {
    if (!(graphics.Page is PdfLoadedPage page))
      throw new NotSupportedException("The field was placed on not PdfPage class instance.");
    return page;
  }
}
