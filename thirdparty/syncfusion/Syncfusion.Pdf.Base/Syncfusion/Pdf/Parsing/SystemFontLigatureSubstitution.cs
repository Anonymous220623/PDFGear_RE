// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontLigatureSubstitution
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontLigatureSubstitution(SystemFontOpenTypeFontSourceBase fontFile) : 
  SystemFontSubTable(fontFile)
{
  private ushort coverageOffset;
  private ushort[] ligatureSetOffsets;
  private SystemFontLigatureSet[] ligatureSets;
  private SystemFontCoverage coverage;

  public SystemFontCoverage Coverage
  {
    get
    {
      if (this.coverage == null)
        this.coverage = this.ReadCoverage(this.Reader, this.coverageOffset);
      return this.coverage;
    }
  }

  public SystemFontLigatureSet[] LigatureSets
  {
    get
    {
      if (this.ligatureSets == null)
      {
        this.ligatureSets = new SystemFontLigatureSet[this.ligatureSetOffsets.Length];
        for (int index = 0; index < this.ligatureSets.Length; ++index)
          this.ligatureSets[index] = this.ReadLigatureSet(this.Reader, this.ligatureSetOffsets[index]);
      }
      return this.ligatureSets;
    }
  }

  private SystemFontLigatureSet ReadLigatureSet(SystemFontOpenTypeFontReader reader, ushort offset)
  {
    reader.BeginReadingBlock();
    long offset1 = this.Offset + (long) offset;
    reader.Seek(offset1, SeekOrigin.Begin);
    SystemFontLigatureSet systemFontLigatureSet = new SystemFontLigatureSet(this.FontSource);
    systemFontLigatureSet.Read(reader);
    systemFontLigatureSet.Offset = offset1;
    reader.EndReadingBlock();
    return systemFontLigatureSet;
  }

  public override SystemFontGlyphsSequence Apply(SystemFontGlyphsSequence glyphIDs)
  {
    SystemFontGlyphsSequence fontGlyphsSequence = new SystemFontGlyphsSequence(glyphIDs.Count);
    for (int index = 0; index < glyphIDs.Count; ++index)
    {
      int coverageIndex = this.Coverage.GetCoverageIndex(glyphIDs[index].GlyphId);
      if (coverageIndex < 0)
      {
        fontGlyphsSequence.Add(glyphIDs[index]);
      }
      else
      {
        SystemFontLigature ligature = this.LigatureSets[coverageIndex].FindLigature(glyphIDs, index);
        if (ligature != null)
        {
          fontGlyphsSequence.Add(ligature.LigatureGlyphId);
          index += ligature.Length;
        }
        else
          fontGlyphsSequence.Add(glyphIDs[index]);
      }
    }
    return fontGlyphsSequence;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    int num = (int) reader.ReadUShort();
    this.coverageOffset = reader.ReadUShort();
    ushort length = reader.ReadUShort();
    this.ligatureSetOffsets = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.ligatureSetOffsets[index] = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    this.Coverage.Write(writer);
    writer.WriteUShort((ushort) this.ligatureSetOffsets.Length);
    foreach (SystemFontTableBase ligatureSet in this.LigatureSets)
      ligatureSet.Write(writer);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    this.coverage = SystemFontCoverage.ImportCoverageTable(this.FontSource, reader);
    ushort length = reader.ReadUShort();
    this.ligatureSets = new SystemFontLigatureSet[(int) length];
    for (int index = 0; index < (int) length; ++index)
    {
      SystemFontLigatureSet systemFontLigatureSet = new SystemFontLigatureSet(this.FontSource);
      systemFontLigatureSet.Import(reader);
      this.ligatureSets[index] = systemFontLigatureSet;
    }
  }
}
