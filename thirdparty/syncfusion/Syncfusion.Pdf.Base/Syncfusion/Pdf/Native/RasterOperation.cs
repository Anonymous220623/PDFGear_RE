// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.RasterOperation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Native;

public enum RasterOperation
{
  R2_BLACK = 1,
  R2_NOTMERGEPEN = 2,
  R2_MASKNOTPEN = 3,
  R2_NOTCOPYPEN = 4,
  R2_MASKPENNOT = 5,
  R2_NOT = 6,
  R2_XORPEN = 7,
  R2_NOTMASKPEN = 8,
  R2_MASKPEN = 9,
  R2_NOTXORPEN = 10, // 0x0000000A
  R2_NOP = 11, // 0x0000000B
  R2_MERGENOTPEN = 12, // 0x0000000C
  R2_COPYPEN = 13, // 0x0000000D
  R2_MERGEPENNOT = 14, // 0x0000000E
  R2_MERGEPEN = 15, // 0x0000000F
  R2_WHITE = 16, // 0x00000010
}
