// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontLigatureSet
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontLigatureSet(SystemFontOpenTypeFontSourceBase fontFile) : SystemFontTableBase(fontFile)
{
  private ushort[] ligatureOffsets;
  private SystemFontLigature[] ligatures;

  public SystemFontLigature[] Ligatures
  {
    get
    {
      if (this.ligatures == null)
      {
        this.ligatures = new SystemFontLigature[this.ligatureOffsets.Length];
        for (int index = 0; index < this.ligatures.Length; ++index)
          this.ligatures[index] = this.ReadLigature(this.Reader, this.ligatureOffsets[index]);
      }
      return this.ligatures;
    }
  }

  private SystemFontLigature ReadLigature(SystemFontOpenTypeFontReader reader, ushort offset)
  {
    reader.BeginReadingBlock();
    reader.Seek(this.Offset + (long) offset, SeekOrigin.Begin);
    SystemFontLigature systemFontLigature = new SystemFontLigature(this.FontSource);
    systemFontLigature.Read(reader);
    reader.EndReadingBlock();
    return systemFontLigature;
  }

  public SystemFontLigature FindLigature(SystemFontGlyphsSequence glyphIDs, int startIndex)
  {
    foreach (SystemFontLigature ligature in this.Ligatures)
    {
      if (ligature.IsMatch(glyphIDs, startIndex + 1))
        return ligature;
    }
    return (SystemFontLigature) null;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    ushort length = reader.ReadUShort();
    this.ligatureOffsets = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.ligatureOffsets[index] = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort((ushort) this.Ligatures.Length);
    for (int index = 0; index < this.Ligatures.Length; ++index)
      this.Ligatures[index].Write(writer);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    ushort length = reader.ReadUShort();
    this.ligatures = new SystemFontLigature[(int) length];
    for (int index = 0; index < (int) length; ++index)
    {
      SystemFontLigature systemFontLigature = new SystemFontLigature(this.FontSource);
      systemFontLigature.Import(reader);
      this.ligatures[index] = systemFontLigature;
    }
  }
}
