// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.Deflate64.InflaterState
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

#nullable disable
namespace Ionic.Zip.Deflate64;

internal enum InflaterState
{
  ReadingHeader = 0,
  ReadingBFinal = 2,
  ReadingBType = 3,
  ReadingNumLitCodes = 4,
  ReadingNumDistCodes = 5,
  ReadingNumCodeLengthCodes = 6,
  ReadingCodeLengthCodes = 7,
  ReadingTreeCodesBefore = 8,
  ReadingTreeCodesAfter = 9,
  DecodeTop = 10, // 0x0000000A
  HaveInitialLength = 11, // 0x0000000B
  HaveFullLength = 12, // 0x0000000C
  HaveDistCode = 13, // 0x0000000D
  UncompressedAligning = 15, // 0x0000000F
  UncompressedByte1 = 16, // 0x00000010
  UncompressedByte2 = 17, // 0x00000011
  UncompressedByte3 = 18, // 0x00000012
  UncompressedByte4 = 19, // 0x00000013
  DecodingUncompressed = 20, // 0x00000014
  StartReadingFooter = 21, // 0x00000015
  ReadingFooter = 22, // 0x00000016
  VerifyingFooter = 23, // 0x00000017
  Done = 24, // 0x00000018
}
