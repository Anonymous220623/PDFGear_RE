// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaPageSettings
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaPageSettings
{
  private PdfXfaPageOrientation m_pageOrientation;
  private SizeF m_size = PdfPageSize.A4;
  private PdfMargins m_margins = new PdfMargins();

  public PdfXfaPageOrientation PageOrientation
  {
    get => this.m_pageOrientation;
    set => this.m_pageOrientation = value;
  }

  public SizeF PageSize
  {
    get => this.m_size;
    set
    {
      if (!(value != SizeF.Empty))
        return;
      this.m_size = value;
    }
  }

  public PdfMargins Margins
  {
    get => this.m_margins;
    set => this.m_margins = value;
  }
}
