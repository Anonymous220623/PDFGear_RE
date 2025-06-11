// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfStringLayoutResult
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfStringLayoutResult
{
  internal LineInfo[] m_lines;
  internal string m_remainder;
  internal SizeF m_actualSize;
  internal float m_lineHeight;

  public string Remainder => this.m_remainder;

  public SizeF ActualSize => this.m_actualSize;

  public LineInfo[] Lines => this.m_lines;

  public float LineHeight => this.m_lineHeight;

  internal bool Empty => this.m_lines == null || this.m_lines.Length == 0;

  internal int LineCount => !this.Empty ? this.m_lines.Length : 0;
}
