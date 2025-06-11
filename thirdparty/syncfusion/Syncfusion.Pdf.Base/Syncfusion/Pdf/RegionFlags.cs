// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.RegionFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class RegionFlags : JBIG2BaseFlags
{
  public const string EXTERNAL_COMBINATION_OPERATOR = "EXTERNAL_COMBINATION_OPERATOR";

  internal override void setFlags(int flagsAsInt)
  {
    this.flagsAsInt = flagsAsInt;
    this.flags.Add((object) "EXTERNAL_COMBINATION_OPERATOR", (object) (flagsAsInt & 7));
  }
}
