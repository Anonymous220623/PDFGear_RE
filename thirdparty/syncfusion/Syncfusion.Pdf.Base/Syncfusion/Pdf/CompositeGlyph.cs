// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.CompositeGlyph
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class CompositeGlyph(FontFile2 fontFile, ushort glyphIndex) : TrueTypeGlyphs(fontFile, glyphIndex)
{
  private List<OutlinePoint[]> contours;

  internal new List<OutlinePoint[]> Contours => this.contours;

  private static OutlinePoint GetTransformedPoint(GlyphDescription compostite, OutlinePoint point)
  {
    return new OutlinePoint(point.Flags)
    {
      Point = compostite.Transformpoint(point.Point)
    };
  }

  private void AddGlyph(GlyphDescription gd)
  {
    TrueTypeGlyphs trueTypeGlyphs = this.FontSource.readGlyphdata(gd.GlyphIndex);
    if (trueTypeGlyphs == null)
      return;
    foreach (OutlinePoint[] contour in trueTypeGlyphs.Contours)
    {
      OutlinePoint[] outlinePointArray = new OutlinePoint[contour.Length];
      for (int index = 0; index < contour.Length; ++index)
        outlinePointArray[index] = CompositeGlyph.GetTransformedPoint(gd, contour[index]);
      this.contours.Add(outlinePointArray);
    }
  }

  public override void Read(ReadFontArray reader)
  {
    int num1 = (int) reader.getnextshort();
    int num2 = (int) reader.getnextshort();
    int num3 = (int) reader.getnextshort();
    int num4 = (int) reader.getnextshort();
    this.contours = new List<OutlinePoint[]>();
    GlyphDescription gd;
    do
    {
      gd = new GlyphDescription();
      gd.Read(reader);
      this.AddGlyph(gd);
    }
    while (gd.CheckFlag((byte) 5));
  }
}
