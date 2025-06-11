// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.StandardWidthTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class StandardWidthTable : WidthTable
{
  private int[] m_widths;

  public override int this[int index]
  {
    get
    {
      if (index < 0 || index >= this.m_widths.Length)
        throw new ArgumentOutOfRangeException(nameof (index), "The character is not supported by the font.");
      return this.m_widths[index];
    }
  }

  public int Length => this.m_widths.Length;

  internal StandardWidthTable(int[] widths)
  {
    this.m_widths = widths != null ? widths : throw new ArgumentNullException(nameof (widths));
  }

  public override WidthTable Clone()
  {
    StandardWidthTable standardWidthTable = this.MemberwiseClone() as StandardWidthTable;
    standardWidthTable.m_widths = (int[]) this.m_widths.Clone();
    return (WidthTable) standardWidthTable;
  }

  internal override PdfArray ToArray() => new PdfArray(this.m_widths);
}
