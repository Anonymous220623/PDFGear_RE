// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.TiffFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

[Flags]
internal enum TiffFlags
{
  MSB2LSB = 1,
  LSB2MSB = 2,
  FILLORDER = LSB2MSB | MSB2LSB, // 0x00000003
  DIRTYDIRECT = 8,
  BUFFERSETUP = 16, // 0x00000010
  CODERSETUP = 32, // 0x00000020
  BEENWRITING = 64, // 0x00000040
  SWAB = 128, // 0x00000080
  NOBITREV = 256, // 0x00000100
  MYBUFFER = 512, // 0x00000200
  ISTILED = 1024, // 0x00000400
  POSTENCODE = 4096, // 0x00001000
  INSUBIFD = 8192, // 0x00002000
  UPSAMPLED = 16384, // 0x00004000
  STRIPCHOP = 32768, // 0x00008000
  HEADERONLY = 65536, // 0x00010000
  NOREADRAW = 131072, // 0x00020000
}
