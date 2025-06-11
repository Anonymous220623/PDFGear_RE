// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.TiffType
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal enum TiffType : short
{
  ANY = 0,
  NOTYPE = 0,
  BYTE = 1,
  ASCII = 2,
  SHORT = 3,
  LONG = 4,
  RATIONAL = 5,
  SBYTE = 6,
  UNDEFINED = 7,
  SSHORT = 8,
  SLONG = 9,
  SRATIONAL = 10, // 0x000A
  FLOAT = 11, // 0x000B
  DOUBLE = 12, // 0x000C
  IFD = 13, // 0x000D
}
