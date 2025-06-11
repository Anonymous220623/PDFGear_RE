// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontGlyphData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.PdfViewer.Base;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontGlyphData : SystemFontTrueTypeTableBase
{
  private readonly ushort glyphIndex;

  internal virtual IEnumerable<SystemFontOutlinePoint[]> Contours
  {
    get => SystemFontEnumerable.Empty<SystemFontOutlinePoint[]>();
  }

  internal override uint Tag => SystemFontTags.GLYF_TABLE;

  internal short NumberOfContours { get; set; }

  public ushort GlyphIndex => this.glyphIndex;

  public Rectangle BoundingRect { get; set; }

  public static SystemFontGlyphData ReadGlyf(
    SystemFontOpenTypeFontSourceBase fontFile,
    ushort glyphIndex)
  {
    short num = fontFile.Reader.ReadShort();
    SystemFontGlyphData systemFontGlyphData = num != (short) 0 ? (num <= (short) 0 ? (SystemFontGlyphData) new SystemFontCompositeGlyph(fontFile, glyphIndex) : (SystemFontGlyphData) new SystemFontSimpleGlyph(fontFile, glyphIndex)) : new SystemFontGlyphData(fontFile, glyphIndex);
    systemFontGlyphData.NumberOfContours = num;
    systemFontGlyphData.BoundingRect = (Rectangle) new Rect(new Syncfusion.PdfViewer.Base.Point((double) fontFile.Reader.ReadShort(), (double) fontFile.Reader.ReadShort()), new Syncfusion.PdfViewer.Base.Point((double) fontFile.Reader.ReadShort(), (double) fontFile.Reader.ReadShort()));
    systemFontGlyphData.Read(fontFile.Reader);
    return systemFontGlyphData;
  }

  public SystemFontGlyphData(SystemFontOpenTypeFontSourceBase fontFile, ushort glyphIndex)
    : base(fontFile)
  {
    this.glyphIndex = glyphIndex;
    this.BoundingRect = (Rectangle) Rect.Empty;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
  }
}
