// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSingleByteIntegerEncoding
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontSingleByteIntegerEncoding : SystemFontByteEncoding
{
  public SystemFontSingleByteIntegerEncoding()
    : base((byte) 32 /*0x20*/, (byte) 246)
  {
  }

  public override object Read(SystemFontEncodedDataReader reader)
  {
    return (object) (sbyte) ((int) reader.Read() - 139);
  }
}
