// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.Zip.CompressionMethod
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Compression.Zip;

public enum CompressionMethod
{
  Stored = 0,
  Shrunk = 1,
  ReducedFactor1 = 2,
  ReducedFactor2 = 3,
  ReducedFactor3 = 4,
  ReducedFactor4 = 5,
  Imploded = 6,
  Tokenizing = 7,
  Deflated = 8,
  Defalte64 = 9,
  PRWARE = 10, // 0x0000000A
  BZIP2 = 12, // 0x0000000C
  LZMA = 14, // 0x0000000E
  IBMTerse = 18, // 0x00000012
  LZ77 = 19, // 0x00000013
  PPMd = 98, // 0x00000062
}
