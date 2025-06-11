// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PatternDictionaryFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class PatternDictionaryFlags : JBIG2BaseFlags
{
  public const string HD_MMR = "HD_MMR";
  public const string HD_TEMPLATE = "HD_TEMPLATE";

  internal override void setFlags(int flagsAsInt)
  {
    this.flagsAsInt = flagsAsInt;
    this.flags.Add((object) "HD_MMR", (object) (flagsAsInt & 1));
    this.flags.Add((object) "HD_TEMPLATE", (object) (flagsAsInt >> 1 & 3));
  }
}
