// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.HalftoneRegionFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class HalftoneRegionFlags : JBIG2BaseFlags
{
  internal const string H_MMR = "H_MMR";
  internal const string H_TEMPLATE = "H_TEMPLATE";
  internal const string H_ENABLE_SKIP = "H_ENABLE_SKIP";
  internal const string H_COMB_OP = "H_COMB_OP";
  internal const string H_DEF_PIXEL = "H_DEF_PIXEL";

  internal override void setFlags(int flagsAsInt)
  {
    this.flagsAsInt = flagsAsInt;
    this.flags.Add((object) "H_MMR", (object) (flagsAsInt & 1));
    this.flags.Add((object) "H_TEMPLATE", (object) (flagsAsInt >> 1 & 3));
    this.flags.Add((object) "H_ENABLE_SKIP", (object) (flagsAsInt >> 3 & 1));
    this.flags.Add((object) "H_COMB_OP", (object) (flagsAsInt >> 4 & 7));
    this.flags.Add((object) "H_DEF_PIXEL", (object) (flagsAsInt >> 7 & 1));
  }
}
