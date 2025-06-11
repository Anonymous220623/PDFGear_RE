// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSubRule
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontSubRule(SystemFontOpenTypeFontSourceBase fontFile) : SystemFontTableBase(fontFile)
{
  private ushort[] input;
  private SystemFontSubstLookupRecord[] substitutions;

  public bool IsMatch(SystemFontGlyphsSequence glyphIDs, int startIndex)
  {
    for (int index = 0; index < this.input.Length; ++index)
    {
      if ((int) glyphIDs[startIndex + index + 1].GlyphId != (int) this.input[index])
        return false;
    }
    return true;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    ushort length1 = reader.ReadUShort();
    ushort length2 = reader.ReadUShort();
    this.input = new ushort[(int) length1];
    this.substitutions = new SystemFontSubstLookupRecord[(int) length2];
    for (int index = 0; index < (int) length1; ++index)
      this.input[index] = reader.ReadUShort();
    for (int index = 0; index < (int) length2; ++index)
    {
      this.substitutions[index] = new SystemFontSubstLookupRecord(this.FontSource);
      this.substitutions[index].Read(reader);
    }
  }
}
