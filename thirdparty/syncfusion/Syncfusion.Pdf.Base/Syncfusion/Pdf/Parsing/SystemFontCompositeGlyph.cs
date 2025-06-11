// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontCompositeGlyph
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontCompositeGlyph(
  SystemFontOpenTypeFontSourceBase fontFile,
  ushort glyphIndex) : SystemFontGlyphData(fontFile, glyphIndex)
{
  private List<SystemFontOutlinePoint[]> contours;

  internal override IEnumerable<SystemFontOutlinePoint[]> Contours
  {
    get => (IEnumerable<SystemFontOutlinePoint[]>) this.contours;
  }

  private static SystemFontOutlinePoint GetTransformedPoint(
    SystemFontMatrix matrix,
    SystemFontOutlinePoint point)
  {
    return new SystemFontOutlinePoint(point.Flags)
    {
      Point = (Syncfusion.PdfViewer.Base.Point) matrix.Transform((System.Drawing.Point) point.Point)
    };
  }

  private void AddGlyph(SystemFontGlyphDescription gd)
  {
    SystemFontGlyphData glyphData = this.FontSource.GetGlyphData(gd.GlyphIndex);
    if (glyphData == null)
      return;
    foreach (SystemFontOutlinePoint[] contour in glyphData.Contours)
    {
      SystemFontOutlinePoint[] fontOutlinePointArray = new SystemFontOutlinePoint[contour.Length];
      for (int index = 0; index < contour.Length; ++index)
        fontOutlinePointArray[index] = SystemFontCompositeGlyph.GetTransformedPoint(gd.Transform, contour[index]);
      this.contours.Add(fontOutlinePointArray);
    }
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    this.contours = new List<SystemFontOutlinePoint[]>();
    SystemFontGlyphDescription gd;
    do
    {
      gd = new SystemFontGlyphDescription();
      gd.Read(reader);
      this.AddGlyph(gd);
    }
    while (gd.CheckFlag((byte) 5));
  }
}
