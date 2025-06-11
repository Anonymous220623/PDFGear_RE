// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.WorkItem
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

#nullable disable
namespace Ionic.Zlib;

internal class WorkItem
{
  public byte[] buffer;
  public byte[] compressed;
  public int crc;
  public int index;
  public int ordinal;
  public int inputBytesAvailable;
  public int compressedBytesAvailable;
  public ZlibCodec compressor;

  public WorkItem(int size, CompressionLevel compressLevel, CompressionStrategy strategy, int ix)
  {
    this.buffer = new byte[size];
    this.compressed = new byte[size + (size / 32768 /*0x8000*/ + 1) * 5 * 2];
    this.compressor = new ZlibCodec();
    this.compressor.InitializeDeflate(compressLevel, false);
    this.compressor.OutputBuffer = this.compressed;
    this.compressor.InputBuffer = this.buffer;
    this.index = ix;
  }
}
