// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontCoverageFormat2
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontCoverageFormat2(SystemFontOpenTypeFontSourceBase fontFile) : 
  SystemFontCoverage(fontFile)
{
  private SystemFontRangeRecord[] rangeRecords;

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    ushort length = reader.ReadUShort();
    this.rangeRecords = new SystemFontRangeRecord[(int) length];
    for (int index = 0; index < (int) length; ++index)
    {
      this.rangeRecords[index] = new SystemFontRangeRecord();
      this.rangeRecords[index].Read(reader);
    }
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort((ushort) 2);
    writer.WriteUShort((ushort) this.rangeRecords.Length);
    for (int index = 0; index < this.rangeRecords.Length; ++index)
      this.rangeRecords[index].Write(writer);
  }

  public override int GetCoverageIndex(ushort glyphIndex)
  {
    foreach (SystemFontRangeRecord rangeRecord in this.rangeRecords)
    {
      if ((int) rangeRecord.Start <= (int) glyphIndex && (int) glyphIndex <= (int) rangeRecord.End)
        return rangeRecord.GetCoverageIndex(glyphIndex);
    }
    return -1;
  }
}
