// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfSectionTemplate
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

public class PdfSectionTemplate : PdfDocumentTemplate
{
  private bool m_left;
  private bool m_top;
  private bool m_right;
  private bool m_bottom;
  private bool m_stamp;

  public bool ApplyDocumentLeftTemplate
  {
    get => this.m_left;
    set => this.m_left = value;
  }

  public bool ApplyDocumentTopTemplate
  {
    get => this.m_top;
    set => this.m_top = value;
  }

  public bool ApplyDocumentRightTemplate
  {
    get => this.m_right;
    set => this.m_right = value;
  }

  public bool ApplyDocumentBottomTemplate
  {
    get => this.m_bottom;
    set => this.m_bottom = value;
  }

  public bool ApplyDocumentStamps
  {
    get => this.m_stamp;
    set => this.m_stamp = value;
  }

  public PdfSectionTemplate()
  {
    this.m_left = this.m_top = this.m_right = this.m_bottom = this.m_stamp = true;
  }
}
