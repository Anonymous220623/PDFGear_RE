// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontIDs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal static class SystemFontIDs
{
  internal const ushort FONT_FAMILY_ID = 1;
  internal const ushort MANUFACTURER_NAME_ID = 8;
  internal const ushort DESIGNER_ID = 9;
  internal const ushort SAMPLE_TEXT_ID = 19;
  internal const ushort WINDOWS_PLATFORM_ID = 3;
  internal const ushort WINDOWS_SYMBOL_ENCODING_ID = 0;
  internal const ushort WINDOWS_UNICODE_BMP_ENCODING_ID = 1;
  internal const ushort ENGLISH_US_ID = 1033;

  internal static Encoding GetEncodingFromEncodingID(ushort encodingId)
  {
    switch (encodingId)
    {
      case 0:
      case 1:
        return Encoding.BigEndianUnicode;
      default:
        return (Encoding) null;
    }
  }
}
