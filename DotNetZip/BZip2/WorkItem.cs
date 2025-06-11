// Decompiled with JetBrains decompiler
// Type: Ionic.BZip2.WorkItem
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System.IO;

#nullable disable
namespace Ionic.BZip2;

internal class WorkItem
{
  public int index;
  public MemoryStream ms;
  public int ordinal;
  public BitWriter bw;

  public BZip2Compressor Compressor { get; private set; }

  public WorkItem(int ix, int blockSize)
  {
    this.ms = new MemoryStream();
    this.bw = new BitWriter((Stream) this.ms);
    this.Compressor = new BZip2Compressor(this.bw, blockSize);
    this.index = ix;
  }
}
