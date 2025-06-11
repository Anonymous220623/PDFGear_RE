// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfLayoutParams
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfLayoutParams
{
  private PdfPage m_page;
  private RectangleF m_bounds;
  private PdfLayoutFormat m_format;

  public PdfPage Page
  {
    get => this.m_page;
    set => this.m_page = value;
  }

  public RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  public PdfLayoutFormat Format
  {
    get => this.m_format;
    set => this.m_format = value;
  }
}
