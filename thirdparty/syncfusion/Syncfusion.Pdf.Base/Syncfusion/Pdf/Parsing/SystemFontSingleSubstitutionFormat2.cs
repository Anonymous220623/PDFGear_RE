// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSingleSubstitutionFormat2
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontSingleSubstitutionFormat2(SystemFontOpenTypeFontSourceBase fontFile) : 
  SystemFontSingleSubstitution(fontFile)
{
  private ushort[] substitutes;

  public override SystemFontGlyphsSequence Apply(SystemFontGlyphsSequence glyphIDs)
  {
    SystemFontGlyphsSequence fontGlyphsSequence = new SystemFontGlyphsSequence(glyphIDs.Count);
    for (int index = 0; index < glyphIDs.Count; ++index)
    {
      int coverageIndex = this.Coverage.GetCoverageIndex(glyphIDs[index].GlyphId);
      if (coverageIndex < 0)
        fontGlyphsSequence.Add(glyphIDs[index]);
      else
        fontGlyphsSequence.Add(this.substitutes[coverageIndex]);
    }
    return fontGlyphsSequence;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    base.Read(reader);
    ushort length = reader.ReadUShort();
    this.substitutes = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.substitutes[index] = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort((ushort) 2);
    base.Write(writer);
    writer.WriteUShort((ushort) this.substitutes.Length);
    for (int index = 0; index < this.substitutes.Length; ++index)
      writer.WriteUShort(this.substitutes[index]);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    base.Import(reader);
    ushort length = reader.ReadUShort();
    this.substitutes = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.substitutes[index] = reader.ReadUShort();
  }
}
