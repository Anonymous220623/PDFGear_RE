// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontCharString
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontCharString : SystemFontIndex
{
  private SystemFontType1GlyphData[] glyphOutlines;
  private SystemFontTop top;

  public SystemFontType1GlyphData this[ushort index]
  {
    get
    {
      if (this.glyphOutlines[(int) index] == null)
        this.glyphOutlines[(int) index] = this.ReadGlyphData(this.Reader, this.Offsets[(int) index], this.GetDataLength((int) index));
      return this.glyphOutlines[(int) index];
    }
  }

  public SystemFontCharString(SystemFontTop top, long offset)
    : base(top.File, offset)
  {
    this.top = top;
  }

  public int GetAdvancedWidth(ushort glyphId, int defaultWidth, int nominalWidth)
  {
    SystemFontType1GlyphData fontType1GlyphData = this[glyphId];
    return !fontType1GlyphData.HasWidth ? defaultWidth : (int) fontType1GlyphData.AdvancedWidth + nominalWidth;
  }

  public void GetGlyphOutlines(SystemFontGlyph glyph, double fontSize)
  {
    SystemFontGlyphOutlinesCollection outlinesCollection = this[glyph.GlyphId].Oultlines.Clone();
    SystemFontMatrix fontMatrix = this.top.FontMatrix;
    fontMatrix.ScaleAppend(fontSize, -fontSize, 0.0, 0.0);
    outlinesCollection.Transform(fontMatrix);
    glyph.Outlines = outlinesCollection;
  }

  private SystemFontType1GlyphData ReadGlyphData(
    SystemFontCFFFontReader reader,
    uint offset,
    int length)
  {
    reader.BeginReadingBlock();
    reader.Seek(this.DataOffset + (long) offset, SeekOrigin.Begin);
    byte[] numArray = new byte[length];
    reader.Read(numArray, numArray.Length);
    SystemFontBuildChar systemFontBuildChar = new SystemFontBuildChar((ISystemFontBuildCharHolder) this.top);
    systemFontBuildChar.Execute(numArray);
    reader.EndReadingBlock();
    SystemFontGlyphOutlinesCollection glyphOutlines = systemFontBuildChar.GlyphOutlines;
    int? width = systemFontBuildChar.Width;
    return new SystemFontType1GlyphData(glyphOutlines, width.HasValue ? new ushort?((ushort) width.GetValueOrDefault()) : new ushort?());
  }

  public override void Read(SystemFontCFFFontReader reader)
  {
    base.Read(reader);
    this.glyphOutlines = new SystemFontType1GlyphData[(int) this.Count];
  }
}
