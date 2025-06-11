// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Lists.ListInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;

#nullable disable
namespace Syncfusion.Pdf.Lists;

internal class ListInfo
{
  private int m_index;
  private PdfList m_list;
  private string m_number;
  private PdfBrush m_brush;
  private PdfPen m_pen;
  private PdfFont m_font;
  private PdfStringFormat m_format;
  internal float MarkerWidth;

  internal int Index
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  internal PdfList List
  {
    get => this.m_list;
    set => this.m_list = value;
  }

  internal string Number
  {
    get => this.m_number;
    set => this.m_number = value;
  }

  internal PdfBrush Brush
  {
    get => this.m_brush;
    set => this.m_brush = value;
  }

  internal PdfPen Pen
  {
    get => this.m_pen;
    set => this.m_pen = value;
  }

  internal PdfFont Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  internal PdfStringFormat Format
  {
    get => this.m_format;
    set => this.m_format = value;
  }

  internal ListInfo(PdfList list, int index, string number)
  {
    this.m_list = list;
    this.m_index = index;
    this.m_number = number;
  }

  internal ListInfo(PdfList list, int index)
    : this(list, index, string.Empty)
  {
  }
}
