// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSingleSubstitution
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontSingleSubstitution : SystemFontSubTable
{
  private ushort coverageOffset;
  private SystemFontCoverage coverage;

  protected SystemFontCoverage Coverage
  {
    get
    {
      if (this.coverage == null)
        this.coverage = this.ReadCoverage(this.Reader, this.coverageOffset);
      return this.coverage;
    }
  }

  internal static SystemFontSingleSubstitution CreateSingleSubstitutionTable(
    SystemFontOpenTypeFontSourceBase fontFile,
    ushort format)
  {
    switch (format)
    {
      case 1:
        return (SystemFontSingleSubstitution) new SystemFontSingleSubstitutionFormat1(fontFile);
      case 2:
        return (SystemFontSingleSubstitution) new SystemFontSingleSubstitutionFormat2(fontFile);
      default:
        return (SystemFontSingleSubstitution) null;
    }
  }

  public SystemFontSingleSubstitution(SystemFontOpenTypeFontSourceBase fontFile)
    : base(fontFile)
  {
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    this.coverageOffset = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer) => this.Coverage.Write(writer);

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    this.coverage = SystemFontCoverage.ImportCoverageTable(this.FontSource, reader);
  }
}
