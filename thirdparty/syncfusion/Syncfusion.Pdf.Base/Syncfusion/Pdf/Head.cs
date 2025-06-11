// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Head
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

internal class Head(FontFile2 fontFile) : TableBase(fontFile)
{
  private ushort macStyle;
  private ushort m_flags;
  private short m_glyphDataFormat;
  private ushort m_unitsPerEm;
  private int m_id;
  private RectangleF m_bbox;
  private short m_indexFormat;

  internal override int Id => this.m_id;

  public ushort Flags
  {
    get => this.m_flags;
    private set => this.m_flags = value;
  }

  public short GlyphDataFormat
  {
    get => this.m_glyphDataFormat;
    private set => this.m_glyphDataFormat = value;
  }

  public ushort UnitsPerEm
  {
    get => this.m_unitsPerEm;
    private set => this.m_unitsPerEm = value;
  }

  public RectangleF BBox
  {
    get => this.m_bbox;
    private set => this.m_bbox = value;
  }

  public short IndexToLocFormat
  {
    get => this.m_indexFormat;
    private set => this.m_indexFormat = value;
  }

  public bool IsBold => this.CheckMacStyle((byte) 0);

  public bool IsItalic => this.CheckMacStyle((byte) 1);

  private bool CheckMacStyle(byte bit) => ((int) this.macStyle & 1 << (int) bit) != 0;

  public override void Read(ReadFontArray reader)
  {
    double num1 = (double) reader.getFixed();
    double num2 = (double) reader.getFixed();
    long num3 = (long) reader.getnextULong();
    long num4 = (long) reader.getnextULong();
    this.m_flags = reader.getnextUshort();
    this.m_unitsPerEm = reader.getnextUshort();
    reader.getLongDateTime();
    reader.getLongDateTime();
    this.m_bbox = new RectangleF((float) reader.getnextshort(), (float) reader.getnextshort(), (float) reader.getnextshort(), (float) reader.getnextshort());
    this.macStyle = reader.getnextUshort();
    int num5 = (int) reader.getnextUshort();
    int num6 = (int) reader.getnextshort();
    this.m_indexFormat = reader.getnextshort();
    int num7 = (int) reader.getnextshort();
  }
}
