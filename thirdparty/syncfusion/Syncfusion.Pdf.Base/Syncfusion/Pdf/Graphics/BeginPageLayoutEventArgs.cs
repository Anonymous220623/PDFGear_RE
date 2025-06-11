// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.BeginPageLayoutEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class BeginPageLayoutEventArgs : PdfCancelEventArgs
{
  private RectangleF m_bounds;
  private PdfPage m_page;

  public RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  public PdfPage Page => this.m_page;

  public BeginPageLayoutEventArgs(RectangleF bounds, PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    this.m_bounds = bounds;
    this.m_page = page;
  }
}
