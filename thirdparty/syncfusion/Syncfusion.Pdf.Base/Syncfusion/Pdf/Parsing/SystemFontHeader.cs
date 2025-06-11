// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontHeader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontHeader(SystemFontCFFFontFile file) : SystemFontCFFTable(file, 0L)
{
  public byte HeaderSize { get; set; }

  public byte OffSize { get; set; }

  public override void Read(SystemFontCFFFontReader reader)
  {
    int num1 = (int) reader.ReadCard8();
    int num2 = (int) reader.ReadCard8();
    this.HeaderSize = reader.ReadCard8();
    this.OffSize = reader.ReadOffSize();
  }
}
