// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSubTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontSubTable : SystemFontTableBase
{
  private static SystemFontSubTable CreateSubTable(
    SystemFontOpenTypeFontSourceBase fontSource,
    ushort type,
    SystemFontOpenTypeFontReader reader)
  {
    long position = reader.Position;
    switch (type)
    {
      case 1:
        ushort format = reader.ReadUShort();
        SystemFontSubTable substitutionTable = (SystemFontSubTable) SystemFontSingleSubstitution.CreateSingleSubstitutionTable(fontSource, format);
        substitutionTable.Offset = position;
        return substitutionTable;
      case 2:
        SystemFontSubTable subTable1 = (SystemFontSubTable) new SystemFontMultipleSubstitution(fontSource);
        subTable1.Offset = position;
        return subTable1;
      case 4:
        SystemFontSubTable subTable2 = (SystemFontSubTable) new SystemFontLigatureSubstitution(fontSource);
        subTable2.Offset = position;
        return subTable2;
      default:
        return (SystemFontSubTable) null;
    }
  }

  internal static SystemFontSubTable ReadSubTable(
    SystemFontOpenTypeFontSourceBase fontSource,
    SystemFontOpenTypeFontReader reader,
    ushort type)
  {
    SystemFontSubTable subTable = SystemFontSubTable.CreateSubTable(fontSource, type, reader);
    subTable?.Read(reader);
    return subTable;
  }

  internal static SystemFontSubTable ImportSubTable(
    SystemFontOpenTypeFontSourceBase fontSource,
    SystemFontOpenTypeFontReader reader,
    ushort type)
  {
    SystemFontSubTable subTable = SystemFontSubTable.CreateSubTable(fontSource, type, reader);
    subTable?.Import(reader);
    return subTable;
  }

  public SystemFontSubTable(SystemFontOpenTypeFontSourceBase fontFile)
    : base(fontFile)
  {
  }

  protected SystemFontCoverage ReadCoverage(SystemFontOpenTypeFontReader reader, ushort offset)
  {
    reader.BeginReadingBlock();
    reader.Seek(this.Offset + (long) offset, SeekOrigin.Begin);
    SystemFontCoverage systemFontCoverage = SystemFontCoverage.ReadCoverageTable(this.FontSource, reader);
    reader.EndReadingBlock();
    return systemFontCoverage;
  }

  public abstract SystemFontGlyphsSequence Apply(SystemFontGlyphsSequence glyphIndices);
}
