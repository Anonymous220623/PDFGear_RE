// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.GlyphDescription
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf;

internal class GlyphDescription
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
  private ushort m_flags;
  private ushort m_glyphIndex;
  private Matrix m_transform;

  public ushort Flags
  {
    get => this.m_flags;
    private set => this.m_flags = value;
  }

  public ushort GlyphIndex
  {
    get => this.m_glyphIndex;
    private set => this.m_glyphIndex = value;
  }

  public Matrix Transform
  {
    get => this.m_transform;
    private set => this.m_transform = value;
  }

  internal bool CheckFlag(byte bit) => this.GetBit((int) this.Flags, bit);

  public bool GetBit(int n, byte bit) => (n & 1 << (int) bit) != 0;

  public PointF Transformpoint(PointF point)
  {
    double x = (double) point.X;
    double y = (double) point.Y;
    return new PointF((float) (x * (double) this.m_transform.Elements[0] + y * (double) this.m_transform.Elements[1]) + this.m_transform.OffsetX, (float) (x * (double) this.m_transform.Elements[2] + y * (double) this.m_transform.Elements[3]) + this.m_transform.OffsetY);
  }

  public void Read(ReadFontArray reader)
  {
    this.m_flags = reader.getnextUshort();
    this.m_glyphIndex = reader.getnextUshort();
    int dx = 0;
    int dy = 0;
    if (this.CheckFlag((byte) 0))
    {
      if (this.CheckFlag((byte) 1))
      {
        dx = (int) reader.getnextshort();
        dy = (int) reader.getnextshort();
      }
      else
      {
        int num1 = (int) reader.getnextUshort();
        int num2 = (int) reader.getnextUshort();
      }
    }
    else if (this.CheckFlag((byte) 1))
    {
      dx = (int) reader.ReadChar();
      dy = (int) reader.ReadChar();
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
      m22 = m11 = reader.Get2Dot14();
    else if (this.CheckFlag((byte) 6))
    {
      m11 = reader.Get2Dot14();
      m22 = reader.Get2Dot14();
    }
    else if (this.CheckFlag((byte) 7))
    {
      m11 = reader.Get2Dot14();
      m12 = reader.Get2Dot14();
      m21 = reader.Get2Dot14();
      m22 = reader.Get2Dot14();
    }
    this.m_transform = new Matrix(m11, m12, m21, m22, (float) dx, (float) dy);
  }
}
