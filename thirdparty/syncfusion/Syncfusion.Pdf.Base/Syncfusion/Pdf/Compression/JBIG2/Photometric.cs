// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Photometric
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal enum Photometric
{
  MINISWHITE = 0,
  MINISBLACK = 1,
  RGB = 2,
  PALETTE = 3,
  MASK = 4,
  SEPARATED = 5,
  YCBCR = 6,
  CIELAB = 8,
  ICCLAB = 9,
  ITULAB = 10, // 0x0000000A
  LOGL = 32844, // 0x0000804C
  LOGLUV = 32845, // 0x0000804D
}
