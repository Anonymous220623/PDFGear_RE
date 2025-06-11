// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.CjkDifferentWidth
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class CjkDifferentWidth : CjkWidth
{
  private int m_from;
  private int[] m_width;

  internal override int From => this.m_from;

  internal override int To => this.From + this.m_width.Length - 1;

  internal override int this[int index]
  {
    get
    {
      if (index < this.From || index > this.To)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is out of range.");
      return this.m_width[index - this.From];
    }
  }

  public CjkDifferentWidth(int from, int[] widths)
  {
    if (widths == null)
      throw new ArgumentNullException(nameof (widths));
    this.m_from = from;
    this.m_width = widths;
  }

  internal override void AppendToArray(PdfArray arr)
  {
    arr.Add((IPdfPrimitive) new PdfNumber(this.From));
    PdfArray element = new PdfArray(this.m_width);
    arr.Add((IPdfPrimitive) element);
  }

  public override CjkWidth Clone()
  {
    CjkDifferentWidth cjkDifferentWidth = this.MemberwiseClone() as CjkDifferentWidth;
    cjkDifferentWidth.m_width = (int[]) this.m_width.Clone();
    return (CjkWidth) cjkDifferentWidth;
  }
}
