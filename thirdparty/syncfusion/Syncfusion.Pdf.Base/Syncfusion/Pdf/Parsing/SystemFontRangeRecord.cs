// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontRangeRecord
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontRangeRecord
{
  public ushort Start { get; private set; }

  public ushort End { get; private set; }

  public ushort StartCoverageIndex { get; private set; }

  public int GetCoverageIndex(ushort glyphIndex)
  {
    return (int) this.StartCoverageIndex + (int) glyphIndex - (int) this.Start;
  }

  public void Read(SystemFontOpenTypeFontReader reader)
  {
    this.Start = reader.ReadUShort();
    this.End = reader.ReadUShort();
    this.StartCoverageIndex = reader.ReadUShort();
  }

  internal void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort(this.Start);
    writer.WriteUShort(this.End);
    writer.WriteUShort(this.StartCoverageIndex);
  }
}
