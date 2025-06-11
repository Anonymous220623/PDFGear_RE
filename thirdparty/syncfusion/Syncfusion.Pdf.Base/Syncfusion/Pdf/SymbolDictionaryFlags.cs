// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.SymbolDictionaryFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class SymbolDictionaryFlags : JBIG2BaseFlags
{
  public const string SD_HUFF = "SD_HUFF";
  public const string SD_REF_AGG = "SD_REF_AGG";
  public const string SD_HUFF_DH = "SD_HUFF_DH";
  public const string SD_HUFF_DW = "SD_HUFF_DW";
  public const string SD_HUFF_BM_SIZE = "SD_HUFF_BM_SIZE";
  public const string SD_HUFF_AGG_INST = "SD_HUFF_AGG_INST";
  public const string BITMAP_CC_USED = "BITMAP_CC_USED";
  public const string BITMAP_CC_RETAINED = "BITMAP_CC_RETAINED";
  public const string SD_TEMPLATE = "SD_TEMPLATE";
  public const string SD_R_TEMPLATE = "SD_R_TEMPLATE";

  internal override void setFlags(int flagsAsInt)
  {
    this.flagsAsInt = flagsAsInt;
    this.flags.Add((object) "SD_HUFF", (object) new int?(flagsAsInt & 1));
    this.flags.Add((object) "SD_REF_AGG", (object) new int?(flagsAsInt >> 1 & 1));
    this.flags.Add((object) "SD_HUFF_DH", (object) new int?(flagsAsInt >> 2 & 3));
    this.flags.Add((object) "SD_HUFF_DW", (object) new int?(flagsAsInt >> 4 & 3));
    this.flags.Add((object) "SD_HUFF_BM_SIZE", (object) new int?(flagsAsInt >> 6 & 1));
    this.flags.Add((object) "SD_HUFF_AGG_INST", (object) new int?(flagsAsInt >> 7 & 1));
    this.flags.Add((object) "BITMAP_CC_USED", (object) new int?(flagsAsInt >> 8 & 1));
    this.flags.Add((object) "BITMAP_CC_RETAINED", (object) new int?(flagsAsInt >> 9 & 1));
    this.flags.Add((object) "SD_TEMPLATE", (object) new int?(flagsAsInt >> 10 & 3));
    this.flags.Add((object) "SD_R_TEMPLATE", (object) new int?(flagsAsInt >> 12 & 1));
  }
}
