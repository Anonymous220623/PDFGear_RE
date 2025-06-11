// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSingleSubstitutionFormat1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontSingleSubstitutionFormat1(SystemFontOpenTypeFontSourceBase fontFile) : 
  SystemFontSingleSubstitution(fontFile)
{
  private ushort deltaGlyphId;

  public override SystemFontGlyphsSequence Apply(SystemFontGlyphsSequence glyphIDs)
  {
    SystemFontGlyphsSequence fontGlyphsSequence = new SystemFontGlyphsSequence(glyphIDs.Count);
    for (int index = 0; index < glyphIDs.Count; ++index)
    {
      ushort glyphId1 = glyphIDs[index].GlyphId;
      if (this.Coverage.GetCoverageIndex(glyphId1) < 0)
      {
        fontGlyphsSequence.Add(glyphIDs[index]);
      }
      else
      {
        ushort glyphId2 = (ushort) ((uint) glyphId1 + (uint) this.deltaGlyphId);
        fontGlyphsSequence.Add(glyphId2);
      }
    }
    return fontGlyphsSequence;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    base.Read(reader);
    this.deltaGlyphId = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort((ushort) 1);
    base.Write(writer);
    writer.WriteUShort(this.deltaGlyphId);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    base.Import(reader);
    this.deltaGlyphId = reader.ReadUShort();
  }
}
