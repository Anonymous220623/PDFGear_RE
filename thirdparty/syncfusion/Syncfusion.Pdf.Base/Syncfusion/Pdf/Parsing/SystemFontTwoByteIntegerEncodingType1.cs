// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontTwoByteIntegerEncodingType1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontTwoByteIntegerEncodingType1 : SystemFontByteEncoding
{
  public SystemFontTwoByteIntegerEncodingType1()
    : base((byte) 247, (byte) 250)
  {
  }

  public override object Read(SystemFontEncodedDataReader reader)
  {
    return (object) (short) (((int) reader.Read() - 247) * 256 /*0x0100*/ + (int) reader.Read() + 108);
  }
}
