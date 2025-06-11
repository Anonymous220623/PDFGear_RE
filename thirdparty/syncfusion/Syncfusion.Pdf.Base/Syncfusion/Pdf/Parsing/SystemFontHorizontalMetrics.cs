// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontHorizontalMetrics
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontHorizontalMetrics(SystemFontOpenTypeFontSourceBase fontFile) : 
  SystemFontTrueTypeTableBase(fontFile)
{
  private SystemFontLongHorMetric[] hMetrics;
  private short[] leftSideBearing;

  internal override uint Tag => SystemFontTags.HMTX_TABLE;

  public ushort GetAdvancedWidth(int glyphID)
  {
    if (this.hMetrics == null)
      return 0;
    return glyphID < this.hMetrics.Length ? this.hMetrics[glyphID].AdvanceWidth : this.hMetrics[this.hMetrics.Length - 1].AdvanceWidth;
  }

  public short GetLeftSideBearing(int glyphID)
  {
    return glyphID < this.hMetrics.Length ? this.hMetrics[glyphID].LSB : this.leftSideBearing[glyphID - this.hMetrics.Length];
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    this.hMetrics = new SystemFontLongHorMetric[(int) this.FontSource.NumberOfHorizontalMetrics];
    for (int index = 0; index < this.hMetrics.Length; ++index)
    {
      SystemFontLongHorMetric fontLongHorMetric = new SystemFontLongHorMetric();
      fontLongHorMetric.Read(reader);
      this.hMetrics[index] = fontLongHorMetric;
    }
    this.leftSideBearing = new short[(int) this.FontSource.GlyphCount - (int) this.FontSource.NumberOfHorizontalMetrics];
    for (int index = 0; index < this.leftSideBearing.Length; ++index)
      this.leftSideBearing[index] = reader.ReadShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    for (int index = 0; index < this.hMetrics.Length; ++index)
      this.hMetrics[index].Write(writer);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    this.hMetrics = new SystemFontLongHorMetric[(int) this.FontSource.NumberOfHorizontalMetrics];
    for (int index = 0; index < (int) this.FontSource.NumberOfHorizontalMetrics; ++index)
    {
      SystemFontLongHorMetric fontLongHorMetric = new SystemFontLongHorMetric();
      fontLongHorMetric.Read(reader);
      this.hMetrics[index] = fontLongHorMetric;
    }
  }
}
