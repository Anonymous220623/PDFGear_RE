// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.LightTableBeginPageLayoutEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class LightTableBeginPageLayoutEventArgs : BeginPageLayoutEventArgs
{
  private int m_startRow;

  public int StartRowIndex => this.m_startRow;

  internal LightTableBeginPageLayoutEventArgs(RectangleF bounds, PdfPage page, int startRow)
    : base(bounds, page)
  {
    this.m_startRow = startRow;
  }
}
