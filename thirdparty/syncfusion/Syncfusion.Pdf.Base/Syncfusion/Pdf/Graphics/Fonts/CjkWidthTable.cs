// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.CjkWidthTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class CjkWidthTable : WidthTable
{
  private List<CjkWidth> m_width;
  private int m_defaultWidth;

  public CjkWidthTable(int defaultWidth)
  {
    this.m_width = new List<CjkWidth>();
    this.m_defaultWidth = defaultWidth;
  }

  public int DefaultWidth => this.m_defaultWidth;

  public override int this[int index]
  {
    get
    {
      int defaultWidth = this.DefaultWidth;
      foreach (CjkWidth cjkWidth in this.m_width)
      {
        if (index >= cjkWidth.From && index <= cjkWidth.To)
          defaultWidth = cjkWidth[index];
      }
      return defaultWidth;
    }
  }

  public void Add(CjkWidth widths)
  {
    if (widths == null)
      throw new ArgumentNullException(nameof (widths));
    this.m_width.Add(widths);
  }

  public override WidthTable Clone()
  {
    CjkWidthTable cjkWidthTable = this.MemberwiseClone() as CjkWidthTable;
    cjkWidthTable.m_width = new List<CjkWidth>(this.m_width.Count);
    foreach (CjkWidth cjkWidth in this.m_width)
      cjkWidthTable.m_width.Add(cjkWidth.Clone());
    return (WidthTable) cjkWidthTable;
  }

  internal override PdfArray ToArray()
  {
    PdfArray arr = new PdfArray();
    foreach (CjkWidth cjkWidth in this.m_width)
      cjkWidth.AppendToArray(arr);
    return arr;
  }
}
