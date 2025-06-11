// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFiveByteIntegerEncoding
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontFiveByteIntegerEncoding : SystemFontByteEncoding
{
  public SystemFontFiveByteIntegerEncoding()
    : base((byte) 29, (byte) 29)
  {
  }

  public override object Read(SystemFontEncodedDataReader reader)
  {
    int num = (int) reader.Read();
    return (object) ((int) reader.Read() << 24 | (int) reader.Read() << 16 /*0x10*/ | (int) reader.Read() << 8 | (int) reader.Read());
  }
}
