// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontCoverage
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontCoverage : SystemFontTableBase
{
  private static SystemFontCoverage CreateCoverageTable(
    SystemFontOpenTypeFontSourceBase fontSource,
    SystemFontOpenTypeFontReader reader)
  {
    long position = reader.Position;
    SystemFontCoverage coverageTable;
    switch (reader.ReadUShort())
    {
      case 1:
        coverageTable = (SystemFontCoverage) new SystemFontCoverageFormat1(fontSource);
        break;
      case 2:
        coverageTable = (SystemFontCoverage) new SystemFontCoverageFormat2(fontSource);
        break;
      default:
        return (SystemFontCoverage) null;
    }
    coverageTable.Offset = position;
    return coverageTable;
  }

  internal static SystemFontCoverage ReadCoverageTable(
    SystemFontOpenTypeFontSourceBase fontSource,
    SystemFontOpenTypeFontReader reader)
  {
    SystemFontCoverage coverageTable = SystemFontCoverage.CreateCoverageTable(fontSource, reader);
    coverageTable?.Read(reader);
    return coverageTable;
  }

  internal static SystemFontCoverage ImportCoverageTable(
    SystemFontOpenTypeFontSourceBase fontSource,
    SystemFontOpenTypeFontReader reader)
  {
    SystemFontCoverage coverageTable = SystemFontCoverage.CreateCoverageTable(fontSource, reader);
    coverageTable?.Import(reader);
    return coverageTable;
  }

  public SystemFontCoverage(SystemFontOpenTypeFontSourceBase fontFile)
    : base(fontFile)
  {
  }

  internal override void Import(SystemFontOpenTypeFontReader reader) => this.Read(reader);

  public abstract int GetCoverageIndex(ushort glyphIndex);
}
