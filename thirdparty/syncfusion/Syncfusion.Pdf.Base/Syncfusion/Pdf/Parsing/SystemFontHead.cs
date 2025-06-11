// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontHead
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.PdfViewer.Base;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontHead(SystemFontOpenTypeFontSourceBase fontFile) : 
  SystemFontTrueTypeTableBase(fontFile)
{
  private ushort macStyle;

  internal override uint Tag => SystemFontTags.HEAD_TABLE;

  public ushort Flags { get; private set; }

  public short GlyphDataFormat { get; private set; }

  public ushort UnitsPerEm { get; private set; }

  public Rect BBox { get; private set; }

  public short IndexToLocFormat { get; private set; }

  public bool IsBold => this.CheckMacStyle((byte) 0);

  public bool IsItalic => this.CheckMacStyle((byte) 1);

  private bool CheckMacStyle(byte bit) => ((int) this.macStyle & 1 << (int) bit) != 0;

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    double num1 = (double) reader.ReadFixed();
    double num2 = (double) reader.ReadFixed();
    int num3 = (int) reader.ReadULong();
    int num4 = (int) reader.ReadULong();
    this.Flags = reader.ReadUShort();
    this.UnitsPerEm = reader.ReadUShort();
    reader.ReadLongDateTime();
    reader.ReadLongDateTime();
    this.BBox = new Rect(new Point((double) reader.ReadShort(), (double) reader.ReadShort()), new Point((double) reader.ReadShort(), (double) reader.ReadShort()));
    this.macStyle = reader.ReadUShort();
    int num5 = (int) reader.ReadUShort();
    int num6 = (int) reader.ReadShort();
    this.IndexToLocFormat = reader.ReadShort();
    int num7 = (int) reader.ReadShort();
  }

  internal override void Write(SystemFontFontWriter writer) => writer.WriteUShort(this.UnitsPerEm);

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    this.UnitsPerEm = reader.ReadUShort();
  }
}
