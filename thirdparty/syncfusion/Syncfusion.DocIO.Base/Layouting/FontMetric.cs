// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.FontMetric
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class FontMetric
{
  public double Ascent(Font font)
  {
    int emHeight = font.FontFamily.GetEmHeight(font.Style);
    int cellAscent = font.FontFamily.GetCellAscent(font.Style);
    return (double) font.SizeInPoints * (double) cellAscent / (double) emHeight;
  }

  public double Descent(Font font)
  {
    int emHeight = font.FontFamily.GetEmHeight(font.Style);
    int cellDescent = font.FontFamily.GetCellDescent(font.Style);
    return (double) font.SizeInPoints * (double) cellDescent / (double) emHeight;
  }
}
