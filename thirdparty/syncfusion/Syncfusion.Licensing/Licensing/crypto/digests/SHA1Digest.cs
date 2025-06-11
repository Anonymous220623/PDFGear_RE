// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.crypto.digests.SHA1Digest
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using System.ComponentModel;

#nullable disable
namespace Syncfusion.Licensing.crypto.digests;

[EditorBrowsable(EditorBrowsableState.Never)]
public class SHA1Digest : GeneralDigest
{
  private static readonly int DIGEST_LENGTH = 20;
  private int H1;
  private int H2;
  private int H3;
  private int H4;
  private int H5;
  private int[] X = new int[80 /*0x50*/];
  private int xOff;
  private static readonly int Y1 = 1518500249;
  private static readonly int Y2 = 1859775393;
  private static readonly int Y3 = -1894007588;
  private static readonly int Y4 = -899497514;

  public SHA1Digest() => this.reset();

  public override string getAlgorithmName() => "SHA-1";

  public override int getDigestSize() => SHA1Digest.DIGEST_LENGTH;

  protected override void processWord(byte[] inBytes, int inOff)
  {
    this.X[this.xOff++] = ((int) inBytes[inOff] & (int) byte.MaxValue) << 24 | ((int) inBytes[inOff + 1] & (int) byte.MaxValue) << 16 /*0x10*/ | ((int) inBytes[inOff + 2] & (int) byte.MaxValue) << 8 | (int) inBytes[inOff + 3] & (int) byte.MaxValue;
    if (this.xOff != 16 /*0x10*/)
      return;
    this.processBlock();
  }

  private void unpackWord(int word, byte[] outBytes, int outOff)
  {
    outBytes[outOff] = (byte) (word >>> 24);
    outBytes[outOff + 1] = (byte) (word >>> 16 /*0x10*/);
    outBytes[outOff + 2] = (byte) (word >>> 8);
    outBytes[outOff + 3] = (byte) word;
  }

  protected override void processLength(long bitLength)
  {
    if (this.xOff > 14)
      this.processBlock();
    this.X[14] = (int) (bitLength >>> 32 /*0x20*/);
    this.X[15] = (int) (bitLength & (long) uint.MaxValue);
  }

  public override int doFinal(byte[] outBytes, int outOff)
  {
    this.finish();
    this.unpackWord(this.H1, outBytes, outOff);
    this.unpackWord(this.H2, outBytes, outOff + 4);
    this.unpackWord(this.H3, outBytes, outOff + 8);
    this.unpackWord(this.H4, outBytes, outOff + 12);
    this.unpackWord(this.H5, outBytes, outOff + 16 /*0x10*/);
    this.reset();
    return SHA1Digest.DIGEST_LENGTH;
  }

  public override void reset()
  {
    base.reset();
    this.H1 = 1732584193;
    this.H2 = -271733879;
    this.H3 = -1732584194;
    this.H4 = 271733878;
    this.H5 = -1009589776;
    this.xOff = 0;
    for (int index = 0; index != this.X.Length; ++index)
      this.X[index] = 0;
  }

  private int f(int u, int v, int w) => u & v | ~u & w;

  private int h(int u, int v, int w) => u ^ v ^ w;

  private int g(int u, int v, int w) => u & v | u & w | v & w;

  private int rotateLeft(int x, int n) => x << n | x >>> 32 /*0x20*/ - n;

  protected override void processBlock()
  {
    for (int index = 16 /*0x10*/; index <= 79; ++index)
      this.X[index] = this.rotateLeft(this.X[index - 3] ^ this.X[index - 8] ^ this.X[index - 14] ^ this.X[index - 16 /*0x10*/], 1);
    int x = this.H1;
    int num1 = this.H2;
    int v = this.H3;
    int w = this.H4;
    int num2 = this.H5;
    for (int index = 0; index <= 19; ++index)
    {
      int num3 = this.rotateLeft(x, 5) + this.f(num1, v, w) + num2 + this.X[index] + SHA1Digest.Y1;
      num2 = w;
      w = v;
      v = this.rotateLeft(num1, 30);
      num1 = x;
      x = num3;
    }
    for (int index = 20; index <= 39; ++index)
    {
      int num4 = this.rotateLeft(x, 5) + this.h(num1, v, w) + num2 + this.X[index] + SHA1Digest.Y2;
      num2 = w;
      w = v;
      v = this.rotateLeft(num1, 30);
      num1 = x;
      x = num4;
    }
    for (int index = 40; index <= 59; ++index)
    {
      int num5 = this.rotateLeft(x, 5) + this.g(num1, v, w) + num2 + this.X[index] + SHA1Digest.Y3;
      num2 = w;
      w = v;
      v = this.rotateLeft(num1, 30);
      num1 = x;
      x = num5;
    }
    for (int index = 60; index <= 79; ++index)
    {
      int num6 = this.rotateLeft(x, 5) + this.h(num1, v, w) + num2 + this.X[index] + SHA1Digest.Y4;
      num2 = w;
      w = v;
      v = this.rotateLeft(num1, 30);
      num1 = x;
      x = num6;
    }
    this.H1 += x;
    this.H2 += num1;
    this.H3 += v;
    this.H4 += w;
    this.H5 += num2;
    this.xOff = 0;
    for (int index = 0; index != this.X.Length; ++index)
      this.X[index] = 0;
  }
}
