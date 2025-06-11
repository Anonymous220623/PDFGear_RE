// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.CompressionMethod
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

#nullable disable
namespace Ionic.Zip;

public enum CompressionMethod
{
  None = 0,
  Deflate = 8,
  Deflate64 = 9,
  BZip2 = 12, // 0x0000000C
}
