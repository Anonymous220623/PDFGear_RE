// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TextRegionHuffmanFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class TextRegionHuffmanFlags : JBIG2BaseFlags
{
  public const string SB_HUFF_FS = "SB_HUFF_FS";
  public const string SB_HUFF_DS = "SB_HUFF_DS";
  public const string SB_HUFF_DT = "SB_HUFF_DT";
  public const string SB_HUFF_RDW = "SB_HUFF_RDW";
  public const string SB_HUFF_RDH = "SB_HUFF_RDH";
  public const string SB_HUFF_RDX = "SB_HUFF_RDX";
  public const string SB_HUFF_RDY = "SB_HUFF_RDY";
  public const string SB_HUFF_RSIZE = "SB_HUFF_RSIZE";

  internal override void setFlags(int flagsAsInt)
  {
    this.flagsAsInt = flagsAsInt;
    this.flags.Add((object) "SB_HUFF_FS", (object) new int?(flagsAsInt & 3));
    this.flags.Add((object) "SB_HUFF_DS", (object) new int?(flagsAsInt >> 2 & 3));
    this.flags.Add((object) "SB_HUFF_DT", (object) new int?(flagsAsInt >> 4 & 3));
    this.flags.Add((object) "SB_HUFF_RDW", (object) new int?(flagsAsInt >> 6 & 3));
    this.flags.Add((object) "SB_HUFF_RDH", (object) new int?(flagsAsInt >> 8 & 3));
    this.flags.Add((object) "SB_HUFF_RDX", (object) new int?(flagsAsInt >> 10 & 3));
    this.flags.Add((object) "SB_HUFF_RDY", (object) new int?(flagsAsInt >> 12 & 3));
    this.flags.Add((object) "SB_HUFF_RSIZE", (object) new int?(flagsAsInt >> 14 & 1));
  }
}
