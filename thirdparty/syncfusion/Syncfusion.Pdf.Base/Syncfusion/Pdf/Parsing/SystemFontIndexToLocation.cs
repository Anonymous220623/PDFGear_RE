// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontIndexToLocation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontIndexToLocation(SystemFontOpenTypeFontSource fontFile) : 
  SystemFontTrueTypeTableBase((SystemFontOpenTypeFontSourceBase) fontFile)
{
  private uint[] offsets;

  internal override uint Tag => SystemFontTags.LOCA_TABLE;

  public long GetOffset(ushort index)
  {
    return this.offsets == null || (int) index >= this.offsets.Length || (int) index < this.offsets.Length - 1 && (int) this.offsets[(int) index + 1] == (int) this.offsets[(int) index] ? -1L : (long) this.offsets[(int) index];
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    this.offsets = new uint[(int) this.FontSource.GlyphCount + 1];
    for (int index = 0; index < this.offsets.Length; ++index)
      this.offsets[index] = this.FontSource.Head.IndexToLocFormat != (short) 0 ? reader.ReadULong() : 2U * (uint) reader.ReadUShort();
  }
}
