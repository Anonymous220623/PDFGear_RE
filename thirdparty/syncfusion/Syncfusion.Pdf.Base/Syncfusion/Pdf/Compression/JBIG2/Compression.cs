// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Compression
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal enum Compression
{
  NONE = 1,
  CCITTRLE = 2,
  CCITTFAX3 = 3,
  CCITT_T4 = 3,
  CCITTFAX4 = 4,
  CCITT_T6 = 4,
  LZW = 5,
  OJPEG = 6,
  JPEG = 7,
  ADOBE_DEFLATE = 8,
  NEXT = 32766, // 0x00007FFE
  CCITTRLEW = 32771, // 0x00008003
  PACKBITS = 32773, // 0x00008005
  THUNDERSCAN = 32809, // 0x00008029
  IT8CTPAD = 32895, // 0x0000807F
  IT8LW = 32896, // 0x00008080
  IT8MP = 32897, // 0x00008081
  IT8BL = 32898, // 0x00008082
  PIXARFILM = 32908, // 0x0000808C
  PIXARLOG = 32909, // 0x0000808D
  DEFLATE = 32946, // 0x000080B2
  DCS = 32947, // 0x000080B3
  JBIG = 34661, // 0x00008765
  SGILOG = 34676, // 0x00008774
  SGILOG24 = 34677, // 0x00008775
  JP2000 = 34712, // 0x00008798
}
