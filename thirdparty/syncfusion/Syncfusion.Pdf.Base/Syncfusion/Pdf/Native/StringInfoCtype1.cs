// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.StringInfoCtype1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Native;

internal enum StringInfoCtype1 : ushort
{
  C1_UPPER = 1,
  C1_LOWER = 2,
  C1_DIGIT = 4,
  C1_SPACE = 8,
  C1_PUNCT = 16, // 0x0010
  C1_CNTRL = 32, // 0x0020
  C1_BLANK = 64, // 0x0040
  C1_XDIGIT = 128, // 0x0080
  C1_ALPHA = 256, // 0x0100
}
