// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontLigature
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontLigature(SystemFontOpenTypeFontSourceBase fontFile) : SystemFontTableBase(fontFile)
{
  private ushort[] componentGlyphIds;

  public ushort LigatureGlyphId { get; private set; }

  public int Length => this.componentGlyphIds.Length;

  public bool IsMatch(SystemFontGlyphsSequence glyphIDs, int startIndex)
  {
    for (int index = 0; index < this.componentGlyphIds.Length; ++index)
    {
      if (index + startIndex >= glyphIDs.Count || (int) this.componentGlyphIds[index] != (int) glyphIDs[index + startIndex].GlyphId)
        return false;
    }
    return true;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    this.LigatureGlyphId = reader.ReadUShort();
    ushort length = (ushort) ((uint) reader.ReadUShort() - 1U);
    this.componentGlyphIds = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.componentGlyphIds[index] = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort(this.LigatureGlyphId);
    writer.WriteUShort((ushort) (this.componentGlyphIds.Length + 1));
    for (int index = 0; index < this.componentGlyphIds.Length; ++index)
      writer.WriteUShort(this.componentGlyphIds[index]);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader) => this.Read(reader);
}
