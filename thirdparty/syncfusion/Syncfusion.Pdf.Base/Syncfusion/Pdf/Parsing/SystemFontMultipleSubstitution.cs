// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontMultipleSubstitution
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontMultipleSubstitution(SystemFontOpenTypeFontSourceBase fontFile) : 
  SystemFontSubTable(fontFile)
{
  private ushort coverageOffset;
  private ushort[] sequenceOffsets;
  private SystemFontCoverage coverage;
  private SystemFontSequence[] sequences;

  protected SystemFontCoverage Coverage
  {
    get
    {
      if (this.coverage == null)
        this.coverage = this.ReadCoverage(this.Reader, this.coverageOffset);
      return this.coverage;
    }
  }

  protected SystemFontSequence[] Sequences
  {
    get
    {
      if (this.sequences == null)
      {
        this.sequences = new SystemFontSequence[this.sequenceOffsets.Length];
        for (int index = 0; index < this.sequences.Length; ++index)
          this.sequences[index] = this.ReadSequence(this.Reader, this.sequenceOffsets[index]);
      }
      return this.sequences;
    }
  }

  private SystemFontSequence ReadSequence(SystemFontOpenTypeFontReader reader, ushort offset)
  {
    reader.BeginReadingBlock();
    reader.Seek(this.Offset + (long) offset, SeekOrigin.Begin);
    SystemFontSequence systemFontSequence = new SystemFontSequence();
    systemFontSequence.Read(reader);
    reader.EndReadingBlock();
    return systemFontSequence;
  }

  public override SystemFontGlyphsSequence Apply(SystemFontGlyphsSequence glyphIDs)
  {
    SystemFontGlyphsSequence fontGlyphsSequence = new SystemFontGlyphsSequence(glyphIDs.Count);
    for (int index = 0; index < glyphIDs.Count; ++index)
    {
      int coverageIndex = this.Coverage.GetCoverageIndex(glyphIDs[index].GlyphId);
      if (coverageIndex >= 0)
        fontGlyphsSequence.AddRange((IEnumerable<ushort>) this.sequences[coverageIndex].Subsitutes);
    }
    return fontGlyphsSequence;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    int num = (int) reader.ReadUShort();
    this.coverageOffset = reader.ReadUShort();
    ushort length = reader.ReadUShort();
    this.sequenceOffsets = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.sequenceOffsets[index] = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    this.Coverage.Write(writer);
    writer.WriteUShort((ushort) this.Sequences.Length);
    for (int index = 0; index < this.Sequences.Length; ++index)
      this.Sequences[index].Write(writer);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    this.coverage = SystemFontCoverage.ImportCoverageTable(this.FontSource, reader);
    ushort length = reader.ReadUShort();
    this.sequences = new SystemFontSequence[(int) length];
    for (int index = 0; index < (int) length; ++index)
    {
      SystemFontSequence systemFontSequence = new SystemFontSequence();
      systemFontSequence.Import(reader);
      this.sequences[index] = systemFontSequence;
    }
  }
}
