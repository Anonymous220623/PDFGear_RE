// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PageInformationFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class PageInformationFlags : JBIG2BaseFlags
{
  internal const string DEFAULT_PIXEL_VALUE = "DEFAULT_PIXEL_VALUE";
  internal const string DEFAULT_COMBINATION_OPERATOR = "DEFAULT_COMBINATION_OPERATOR";

  internal override void setFlags(int flagAsInt)
  {
    this.flagsAsInt = flagAsInt;
    this.flags.Add((object) "DEFAULT_PIXEL_VALUE", (object) new int?(flagAsInt >> 2 & 1));
    this.flags.Add((object) "DEFAULT_COMBINATION_OPERATOR", (object) new int?(flagAsInt >> 3 & 3));
  }
}
