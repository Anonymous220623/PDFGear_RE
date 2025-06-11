// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.TtfHeadTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal struct TtfHeadTable
{
  public long Modified;
  public long Created;
  public uint MagicNumber;
  public uint CheckSumAdjustment;
  public float FontRevision;
  public float Version;
  public short XMin;
  public short YMin;
  public ushort UnitsPerEm;
  public short YMax;
  public short XMax;
  public ushort MacStyle;
  public ushort Flags;
  public ushort LowestRecPPEM;
  public short FontDirectionHint;
  public short IndexToLocFormat;
  public short GlyphDataFormat;
}
