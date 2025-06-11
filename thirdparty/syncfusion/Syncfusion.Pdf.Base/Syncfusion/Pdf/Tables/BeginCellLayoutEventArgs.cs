// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.BeginCellLayoutEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class BeginCellLayoutEventArgs : CellLayoutEventArgs
{
  private bool m_bSkip;

  public bool Skip
  {
    get => this.m_bSkip;
    set => this.m_bSkip = value;
  }

  internal BeginCellLayoutEventArgs(
    PdfGraphics graphics,
    int rowIndex,
    int cellInder,
    RectangleF bounds,
    string value)
    : base(graphics, rowIndex, cellInder, bounds, value)
  {
  }
}
