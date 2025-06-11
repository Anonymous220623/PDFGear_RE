// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Lists.ListBeginPageLayoutEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Lists;

public class ListBeginPageLayoutEventArgs : BeginPageLayoutEventArgs
{
  private PdfList m_list;

  public PdfList List => this.m_list;

  internal ListBeginPageLayoutEventArgs(RectangleF bounds, PdfPage page, PdfList list)
    : base(bounds, page)
  {
    this.m_list = list;
  }
}
