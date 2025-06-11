// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.StringInfoCtype3
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Native;

internal enum StringInfoCtype3 : ushort
{
  C3_NOTAPPLICABLE = 0,
  C3_DIACRITIC = 2,
  C3_VOWELMARK = 4,
  C3_SYMBOL = 8,
  C3_KATAKANA = 16, // 0x0010
  C3_HIRAGANA = 32, // 0x0020
  C3_HALFWIDTH = 64, // 0x0040
  C3_FULLWIDTH = 128, // 0x0080
  C3_IDEOGRAPH = 256, // 0x0100
  C3_KASHIDA = 512, // 0x0200
  C3_LEXICAL = 1024, // 0x0400
  C3_ALPHA = 32768, // 0x8000
}
