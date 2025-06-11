// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Lists.BeginItemLayoutEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Lists;

public class BeginItemLayoutEventArgs
{
  private PdfListItem m_item;
  private PdfPage m_page;

  public PdfListItem Item => this.m_item;

  public PdfPage Page => this.m_page;

  internal BeginItemLayoutEventArgs(PdfListItem item, PdfPage page)
  {
    this.m_item = item;
    this.m_page = page;
  }
}
