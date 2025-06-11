// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.RefinementRegionFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class RefinementRegionFlags : JBIG2BaseFlags
{
  public const string GR_TEMPLATE = "GR_TEMPLATE";
  public const string TPGDON = "TPGDON";

  internal override void setFlags(int flagsAsInt)
  {
    this.flagsAsInt = flagsAsInt;
    this.flags.Add((object) "GR_TEMPLATE", (object) new int?(flagsAsInt & 1));
    this.flags.Add((object) "TPGDON", (object) new int?(flagsAsInt >> 1 & 1));
  }
}
