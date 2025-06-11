// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TextRegionFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class TextRegionFlags : JBIG2BaseFlags
{
  public const string SB_HUFF = "SB_HUFF";
  public const string SB_REFINE = "SB_REFINE";
  public const string LOG_SB_STRIPES = "LOG_SB_STRIPES";
  public const string REF_CORNER = "REF_CORNER";
  public const string TRANSPOSED = "TRANSPOSED";
  public const string SB_COMB_OP = "SB_COMB_OP";
  public const string SB_DEF_PIXEL = "SB_DEF_PIXEL";
  public const string SB_DS_OFFSET = "SB_DS_OFFSET";
  public const string SB_R_TEMPLATE = "SB_R_TEMPLATE";

  internal override void setFlags(int flagsAsInt)
  {
    this.flagsAsInt = flagsAsInt;
    this.flags.Add((object) "SB_HUFF", (object) new int?(flagsAsInt & 1));
    this.flags.Add((object) "SB_REFINE", (object) new int?(flagsAsInt >> 1 & 1));
    this.flags.Add((object) "LOG_SB_STRIPES", (object) new int?(flagsAsInt >> 2 & 3));
    this.flags.Add((object) "REF_CORNER", (object) new int?(flagsAsInt >> 4 & 3));
    this.flags.Add((object) "TRANSPOSED", (object) new int?(flagsAsInt >> 6 & 1));
    this.flags.Add((object) "SB_COMB_OP", (object) new int?(flagsAsInt >> 7 & 3));
    this.flags.Add((object) "SB_DEF_PIXEL", (object) new int?(flagsAsInt >> 9 & 1));
    int num = flagsAsInt >> 10 & 31 /*0x1F*/;
    if ((num & 16 /*0x10*/) != 0)
      num |= -16;
    this.flags.Add((object) "SB_DS_OFFSET", (object) new int?(num));
    this.flags.Add((object) "SB_R_TEMPLATE", (object) new int?(flagsAsInt >> 15 & 1));
  }
}
