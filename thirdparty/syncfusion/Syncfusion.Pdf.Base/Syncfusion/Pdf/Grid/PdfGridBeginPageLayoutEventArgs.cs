// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.PdfGridBeginPageLayoutEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Grid;

public class PdfGridBeginPageLayoutEventArgs : BeginPageLayoutEventArgs
{
  private int m_startRow;

  public int StartRowIndex => this.m_startRow;

  internal PdfGridBeginPageLayoutEventArgs(RectangleF bounds, PdfPage page, int startRow)
    : base(bounds, page)
  {
    this.m_startRow = startRow;
  }
}
