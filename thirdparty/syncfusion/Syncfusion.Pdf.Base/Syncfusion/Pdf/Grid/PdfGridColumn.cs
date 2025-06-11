// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.PdfGridColumn
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;

#nullable disable
namespace Syncfusion.Pdf.Grid;

public class PdfGridColumn
{
  private PdfGrid m_grid;
  internal float m_width = float.MinValue;
  internal bool isCustomWidth;
  private PdfStringFormat m_format;

  public float Width
  {
    get => this.m_width;
    set
    {
      this.isCustomWidth = true;
      this.m_width = value;
    }
  }

  public PdfStringFormat Format
  {
    get
    {
      if (this.m_format == null)
        this.m_format = new PdfStringFormat();
      return this.m_format;
    }
    set => this.m_format = value;
  }

  public PdfGrid Grid => this.m_grid;

  public PdfGridColumn(PdfGrid grid) => this.m_grid = grid;
}
