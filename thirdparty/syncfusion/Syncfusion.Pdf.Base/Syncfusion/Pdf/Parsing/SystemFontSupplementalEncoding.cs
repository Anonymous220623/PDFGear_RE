// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSupplementalEncoding
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontSupplementalEncoding
{
  private SystemFontSupplement[] supplements;

  public void Read(SystemFontCFFFontReader reader)
  {
    byte length = reader.ReadCard8();
    this.supplements = new SystemFontSupplement[(int) length];
    for (int index = 0; index < (int) length; ++index)
    {
      SystemFontSupplement systemFontSupplement = new SystemFontSupplement();
      systemFontSupplement.Read(reader);
      this.supplements[index] = systemFontSupplement;
    }
  }
}
