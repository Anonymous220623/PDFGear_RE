// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.EndPageLayoutEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class EndPageLayoutEventArgs : PdfCancelEventArgs
{
  private PdfLayoutResult m_result;
  private PdfPage m_nextPage;

  public PdfLayoutResult Result => this.m_result;

  public PdfPage NextPage
  {
    get => this.m_nextPage;
    set => this.m_nextPage = value;
  }

  public EndPageLayoutEventArgs(PdfLayoutResult result)
  {
    this.m_result = result != null ? result : throw new ArgumentNullException(nameof (result));
  }
}
