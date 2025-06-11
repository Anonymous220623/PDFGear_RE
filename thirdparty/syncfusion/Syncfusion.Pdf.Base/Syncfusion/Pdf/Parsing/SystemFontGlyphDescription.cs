// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontGlyphDescription
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontGlyphDescription
{
  internal const int ARG_1_AND_2_ARE_WORDS = 0;
  internal const int ARGS_ARE_XY_VALUES = 1;
  internal const int ROUND_XY_TO_GRID = 2;
  internal const int WE_HAVE_A_SCALE = 3;
  internal const int MORE_COMPONENTS = 5;
  internal const int WE_HAVE_AN_X_AND_Y_SCALE = 6;
  internal const int WE_HAVE_A_TWO_BY_TWO = 7;
  internal const int WE_HAVE_INSTRUCTIONS = 8;
  internal const int USE_MY_METRICS = 9;
  internal const int OVERLAP_COMPOUND = 10;

  public ushort Flags { get; private set; }

  public ushort GlyphIndex { get; private set; }

  public SystemFontMatrix Transform { get; private set; }

  internal bool CheckFlag(byte bit) => SystemFontBitsHelper.GetBit((int) this.Flags, bit);

  public void Read(SystemFontOpenTypeFontReader reader)
  {
    this.Flags = reader.ReadUShort();
    this.GlyphIndex = reader.ReadUShort();
    int offsetX = 0;
    int offsetY = 0;
    if (this.CheckFlag((byte) 0))
    {
      if (this.CheckFlag((byte) 1))
      {
        offsetX = (int) reader.ReadShort();
        offsetY = (int) reader.ReadShort();
      }
      else
      {
        int num1 = (int) reader.ReadUShort();
        int num2 = (int) reader.ReadUShort();
      }
    }
    else if (this.CheckFlag((byte) 1))
    {
      offsetX = (int) reader.ReadChar();
      offsetY = (int) reader.ReadChar();
    }
    else
    {
      int num3 = (int) reader.ReadChar();
      int num4 = (int) reader.ReadChar();
    }
    float m11 = 1f;
    float m12 = 0.0f;
    float m21 = 0.0f;
    float m22 = 1f;
    if (this.CheckFlag((byte) 3))
      m22 = m11 = reader.Read2Dot14();
    else if (this.CheckFlag((byte) 6))
    {
      m11 = reader.Read2Dot14();
      m22 = reader.Read2Dot14();
    }
    else if (this.CheckFlag((byte) 7))
    {
      m11 = reader.Read2Dot14();
      m12 = reader.Read2Dot14();
      m21 = reader.Read2Dot14();
      m22 = reader.Read2Dot14();
    }
    this.Transform = new SystemFontMatrix((double) m11, (double) m12, (double) m21, (double) m22, (double) offsetX, (double) offsetY);
  }
}
