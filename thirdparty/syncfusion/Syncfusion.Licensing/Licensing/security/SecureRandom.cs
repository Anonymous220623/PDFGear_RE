// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.security.SecureRandom
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using Syncfusion.Licensing.crypto.digests;
using System;

#nullable disable
namespace Syncfusion.Licensing.security;

public class SecureRandom : Random
{
  private static SecureRandom rand = new SecureRandom();
  private long counter = 1;
  private SHA1Digest digest = new SHA1Digest();
  private byte[] state;
  private byte[] intBytes = new byte[4];
  private byte[] longBytes = new byte[8];

  public SecureRandom()
    : base(0)
  {
    this.state = new byte[this.digest.getDigestSize()];
    this.setSeed(DateTime.Now.Ticks);
  }

  public virtual void setSeed(byte[] inSeed) => this.digest.update(inSeed, 0, inSeed.Length);

  public virtual void setSeed(long rSeed)
  {
    if (rSeed == 0L)
      return;
    this.setSeed(this.longToBytes(rSeed));
  }

  public static byte[] getSeed(int numBytes)
  {
    byte[] bytes = new byte[numBytes];
    SecureRandom.rand.setSeed(DateTime.Now.Ticks);
    SecureRandom.rand.nextBytes(bytes);
    return bytes;
  }

  public virtual byte[] generateSeed(int numBytes)
  {
    byte[] bytes = new byte[numBytes];
    this.nextBytes(bytes);
    return bytes;
  }

  public virtual void nextBytes(byte[] bytes)
  {
    int num = 0;
    this.digest.doFinal(this.state, 0);
    for (int index = 0; index != bytes.Length; ++index)
    {
      if (num == this.state.Length)
      {
        byte[] bytes1 = this.longToBytes(this.counter++);
        this.digest.update(bytes1, 0, bytes1.Length);
        this.digest.update(this.state, 0, this.state.Length);
        this.digest.doFinal(this.state, 0);
        num = 0;
      }
      bytes[index] = this.state[num++];
    }
    byte[] bytes2 = this.longToBytes(this.counter++);
    this.digest.update(bytes2, 0, bytes2.Length);
    this.digest.update(this.state, 0, this.state.Length);
  }

  public virtual int nextInt()
  {
    this.nextBytes(this.intBytes);
    int num = 0;
    for (int index = 0; index < 4; ++index)
      num = (num << 8) + ((int) this.intBytes[index] & (int) byte.MaxValue);
    return num;
  }

  protected int next(int numBits)
  {
    int length = (numBits + 7) / 8;
    byte[] bytes = new byte[length];
    this.nextBytes(bytes);
    int num = 0;
    for (int index = 0; index < length; ++index)
      num = (num << 8) + ((int) bytes[index] & (int) byte.MaxValue);
    return num & (1 << numBits) - 1;
  }

  private byte[] longToBytes(long val)
  {
    for (int index = 0; index != 8; ++index)
    {
      this.longBytes[index] = (byte) val;
      val >>>= 8;
    }
    return this.longBytes;
  }
}
