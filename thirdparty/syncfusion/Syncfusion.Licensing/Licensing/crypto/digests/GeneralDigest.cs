// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.crypto.digests.GeneralDigest
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using System.ComponentModel;

#nullable disable
namespace Syncfusion.Licensing.crypto.digests;

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class GeneralDigest : Digest
{
  private byte[] xBuf;
  private int xBufOff;
  private long byteCount;

  protected GeneralDigest()
  {
    this.xBuf = new byte[4];
    this.xBufOff = 0;
  }

  public void update(byte inbyte)
  {
    this.xBuf[this.xBufOff++] = inbyte;
    if (this.xBufOff == this.xBuf.Length)
    {
      this.processWord(this.xBuf, 0);
      this.xBufOff = 0;
    }
    ++this.byteCount;
  }

  public void update(byte[] inBytes, int inOff, int len)
  {
    for (; this.xBufOff != 0 && len > 0; --len)
    {
      this.update(inBytes[inOff]);
      ++inOff;
    }
    while (len > this.xBuf.Length)
    {
      this.processWord(inBytes, inOff);
      inOff += this.xBuf.Length;
      len -= this.xBuf.Length;
      this.byteCount += (long) this.xBuf.Length;
    }
    for (; len > 0; --len)
    {
      this.update(inBytes[inOff]);
      ++inOff;
    }
  }

  public void finish()
  {
    long bitLength = this.byteCount << 3;
    this.update((byte) 128 /*0x80*/);
    while (this.xBufOff != 0)
      this.update((byte) 0);
    this.processLength(bitLength);
    this.processBlock();
  }

  public virtual void reset()
  {
    this.byteCount = 0L;
    this.xBufOff = 0;
    for (int index = 0; index < this.xBuf.Length; ++index)
      this.xBuf[index] = (byte) 0;
  }

  protected abstract void processWord(byte[] inBytes, int inOff);

  protected abstract void processLength(long bitLength);

  protected abstract void processBlock();

  public abstract string getAlgorithmName();

  public abstract int getDigestSize();

  public abstract int doFinal(byte[] outBytes, int outOff);
}
