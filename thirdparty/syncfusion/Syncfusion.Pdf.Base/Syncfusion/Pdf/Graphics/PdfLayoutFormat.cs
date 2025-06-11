// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfLayoutFormat
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfLayoutFormat
{
  private bool m_boundsSet;
  private RectangleF m_paginateBounds;
  private PdfLayoutType m_layout;
  private PdfLayoutBreakType m_break;

  public PdfLayoutType Layout
  {
    get => this.m_layout;
    set => this.m_layout = value;
  }

  public PdfLayoutBreakType Break
  {
    get => this.m_break;
    set => this.m_break = value;
  }

  public RectangleF PaginateBounds
  {
    get => this.m_paginateBounds;
    set
    {
      this.m_paginateBounds = value;
      this.m_boundsSet = true;
    }
  }

  internal bool UsePaginateBounds => this.m_boundsSet;

  public PdfLayoutFormat()
  {
  }

  public PdfLayoutFormat(PdfLayoutFormat baseFormat)
    : this()
  {
    this.Break = baseFormat != null ? baseFormat.Break : throw new ArgumentNullException(nameof (baseFormat));
    this.Layout = baseFormat.Layout;
    this.PaginateBounds = baseFormat.PaginateBounds;
    this.m_boundsSet = baseFormat.UsePaginateBounds;
  }
}
