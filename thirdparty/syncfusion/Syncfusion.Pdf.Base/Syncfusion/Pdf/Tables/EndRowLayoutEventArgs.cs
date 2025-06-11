// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.EndRowLayoutEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class EndRowLayoutEventArgs : EventArgs
{
  private int m_rowIndex;
  private bool m_bDrawnCompletely;
  private bool m_bCancel;
  private RectangleF m_bounds;

  public int RowIndex => this.m_rowIndex;

  public bool LayoutCompleted => this.m_bDrawnCompletely;

  public bool Cancel
  {
    get => this.m_bCancel;
    set => this.m_bCancel = value;
  }

  public RectangleF Bounds => this.m_bounds;

  internal EndRowLayoutEventArgs(int rowIndex, bool drawnCompletely, RectangleF rowBounds)
  {
    this.m_rowIndex = rowIndex;
    this.m_bDrawnCompletely = drawnCompletely;
    this.m_bounds = rowBounds;
  }
}
