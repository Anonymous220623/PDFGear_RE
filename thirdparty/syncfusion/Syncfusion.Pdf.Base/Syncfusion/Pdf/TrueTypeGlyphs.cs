// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TrueTypeGlyphs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf;

internal class TrueTypeGlyphs : TableBase
{
  public GraphicsPath graphic = new GraphicsPath();
  public Dictionary<ushort, GraphicsPath> PathTable = new Dictionary<ushort, GraphicsPath>();
  private readonly ushort glyphIndex;
  private List<OutlinePoint[]> m_contours;
  private int m_id = 4;
  private short m_numberOfContours;

  internal List<OutlinePoint[]> Contours
  {
    get
    {
      if (this.m_contours == null)
        this.m_contours = new List<OutlinePoint[]>();
      return this.m_contours;
    }
    set => this.m_contours = value;
  }

  internal override int Id => this.m_id;

  internal short NumberOfContours
  {
    get => this.m_numberOfContours;
    set => this.m_numberOfContours = value;
  }

  public ushort GlyphIndex => this.glyphIndex;

  public static TrueTypeGlyphs ReadGlyf(FontFile2 fontFile, ushort glyphIndex)
  {
    short num = fontFile.FontArrayReader.getnextshort();
    TrueTypeGlyphs trueTypeGlyphs = num != (short) 0 ? (num <= (short) 0 ? (TrueTypeGlyphs) new CompositeGlyph(fontFile, glyphIndex) : (TrueTypeGlyphs) new SimpleGlyf(fontFile, glyphIndex)) : new TrueTypeGlyphs(fontFile, glyphIndex);
    trueTypeGlyphs.NumberOfContours = num;
    trueTypeGlyphs.Read(fontFile.FontArrayReader);
    switch (trueTypeGlyphs)
    {
      case SimpleGlyf _:
        SimpleGlyf simpleGlyf = trueTypeGlyphs as SimpleGlyf;
        trueTypeGlyphs.m_contours = simpleGlyf.Contours;
        break;
      case CompositeGlyph _:
        CompositeGlyph compositeGlyph = trueTypeGlyphs as CompositeGlyph;
        trueTypeGlyphs.m_contours = compositeGlyph.Contours;
        break;
    }
    return trueTypeGlyphs;
  }

  public TrueTypeGlyphs(FontFile2 fontFile, ushort glyphIndex)
    : base(fontFile)
  {
    this.glyphIndex = glyphIndex;
  }

  public TrueTypeGlyphs(FontFile2 fontFile)
    : base(fontFile)
  {
    this.glyphIndex = this.glyphIndex;
  }

  public override void Read(ReadFontArray reader)
  {
  }
}
