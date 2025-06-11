// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.PdfColumnCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class PdfColumnCollection : PdfCollection
{
  public PdfColumn this[int index] => this.List[index] as PdfColumn;

  internal PdfColumnCollection()
  {
  }

  public void Add(PdfColumn column) => this.List.Add((object) column);

  internal float[] GetWidths(float totalWidth)
  {
    return this.GetWidths(totalWidth, 0, this.Count - 1, false);
  }

  internal float[] GetWidths(
    float totalWidth,
    int startColumn,
    int endColumn,
    bool columnProportionalSizing)
  {
    int length = endColumn - startColumn + 1;
    float[] widths = length <= this.Count ? new float[length] : throw new ArgumentException("The start and end column indices doesn't match.");
    float num1 = 0.0f;
    int num2 = length;
    for (int index = startColumn; index <= endColumn; ++index)
    {
      float width = this[index].Width;
      widths[index - startColumn] = width;
      num1 += width;
    }
    if ((double) totalWidth > 0.0)
    {
      float num3 = totalWidth / num1;
      if (!columnProportionalSizing)
      {
        for (int index = 0; index < length; ++index)
        {
          if (this[index].isCustomWidth)
          {
            widths[index] = this[index].Width;
            totalWidth -= this[index].Width;
            --num2;
          }
          else
            widths[index] = -1f;
        }
        for (int index = 0; index < length; ++index)
        {
          float num4 = totalWidth / (float) num2;
          if ((double) widths[index] <= 0.0)
            widths[index] = num4;
        }
      }
      else
      {
        for (int index = 0; index < length; ++index)
          widths[index] *= num3;
      }
    }
    return widths;
  }
}
