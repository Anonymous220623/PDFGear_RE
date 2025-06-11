// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontHorizontalHeader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontHorizontalHeader(SystemFontOpenTypeFontSourceBase fontFile) : 
  SystemFontTrueTypeTableBase(fontFile)
{
  internal override uint Tag => SystemFontTags.HHEA_TABLE;

  public short Ascender { get; private set; }

  public short Descender { get; private set; }

  public short LineGap { get; private set; }

  public ushort NumberOfHMetrics { get; private set; }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    double num1 = (double) reader.ReadFixed();
    this.Ascender = reader.ReadShort();
    this.Descender = reader.ReadShort();
    this.LineGap = reader.ReadShort();
    int num2 = (int) reader.ReadUShort();
    int num3 = (int) reader.ReadShort();
    int num4 = (int) reader.ReadShort();
    int num5 = (int) reader.ReadShort();
    int num6 = (int) reader.ReadShort();
    int num7 = (int) reader.ReadShort();
    int num8 = (int) reader.ReadShort();
    int num9 = (int) reader.ReadShort();
    int num10 = (int) reader.ReadShort();
    int num11 = (int) reader.ReadShort();
    int num12 = (int) reader.ReadShort();
    int num13 = (int) reader.ReadShort();
    this.NumberOfHMetrics = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    writer.WriteShort(this.Ascender);
    writer.WriteShort(this.Descender);
    writer.WriteShort(this.LineGap);
    writer.WriteUShort(this.NumberOfHMetrics);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    this.Ascender = reader.ReadShort();
    this.Descender = reader.ReadShort();
    this.LineGap = reader.ReadShort();
    this.NumberOfHMetrics = reader.ReadUShort();
  }
}
