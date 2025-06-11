// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontTwoByteIntegerEncodingType2
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontTwoByteIntegerEncodingType2 : SystemFontByteEncoding
{
  public SystemFontTwoByteIntegerEncodingType2()
    : base((byte) 251, (byte) 254)
  {
  }

  public override object Read(SystemFontEncodedDataReader reader)
  {
    return (object) (short) (-((int) reader.Read() - 251) * 256 /*0x0100*/ - (int) reader.Read() - 108);
  }
}
