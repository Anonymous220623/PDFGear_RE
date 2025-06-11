// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.CjkSameWidth
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class CjkSameWidth : CjkWidth
{
  private int m_from;
  private int m_to;
  private int m_width;

  internal override int From => this.m_from;

  internal override int To => this.m_to;

  internal override int this[int index]
  {
    get
    {
      if (index < this.From || index > this.To)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is out of range.");
      return this.m_width;
    }
  }

  public CjkSameWidth(int from, int to, int width)
  {
    this.m_from = from <= to ? from : throw new ArgumentException("'From' can't be grater than 'to'.");
    this.m_to = to;
    this.m_width = width;
  }

  internal override void AppendToArray(PdfArray arr)
  {
    arr.Add((IPdfPrimitive) new PdfNumber(this.From));
    arr.Add((IPdfPrimitive) new PdfNumber(this.To));
    arr.Add((IPdfPrimitive) new PdfNumber(this.m_width));
  }

  public override CjkWidth Clone() => this.MemberwiseClone() as CjkWidth;
}
