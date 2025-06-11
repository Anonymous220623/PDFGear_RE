// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.math.BigInteger
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using Syncfusion.Licensing.security;
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Syncfusion.Licensing.math;

[EditorBrowsable(EditorBrowsableState.Never)]
public class BigInteger
{
  public const long IMASK = 4294967295 /*0xFFFFFFFF*/;
  private int sign;
  private int[] magnitude;
  private int nBits = -1;
  private int nBitLength = -1;
  private long mQuote = -1;
  private static readonly int BITS_PER_BYTE = 8;
  private static readonly int BYTES_PER_INT = 4;
  private static readonly byte[] rndMask = new byte[8]
  {
    byte.MaxValue,
    (byte) 127 /*0x7F*/,
    (byte) 63 /*0x3F*/,
    (byte) 31 /*0x1F*/,
    (byte) 15,
    (byte) 7,
    (byte) 3,
    (byte) 1
  };
  private static readonly byte[] bitCounts = new byte[256 /*0x0100*/]
  {
    (byte) 0,
    (byte) 1,
    (byte) 1,
    (byte) 2,
    (byte) 1,
    (byte) 2,
    (byte) 2,
    (byte) 3,
    (byte) 1,
    (byte) 2,
    (byte) 2,
    (byte) 3,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 1,
    (byte) 2,
    (byte) 2,
    (byte) 3,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 1,
    (byte) 2,
    (byte) 2,
    (byte) 3,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 1,
    (byte) 2,
    (byte) 2,
    (byte) 3,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 5,
    (byte) 6,
    (byte) 6,
    (byte) 7,
    (byte) 1,
    (byte) 2,
    (byte) 2,
    (byte) 3,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 5,
    (byte) 6,
    (byte) 6,
    (byte) 7,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 5,
    (byte) 6,
    (byte) 6,
    (byte) 7,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 5,
    (byte) 6,
    (byte) 6,
    (byte) 7,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 5,
    (byte) 6,
    (byte) 6,
    (byte) 7,
    (byte) 5,
    (byte) 6,
    (byte) 6,
    (byte) 7,
    (byte) 6,
    (byte) 7,
    (byte) 7,
    (byte) 8
  };
  private static readonly byte[] bitLengths = new byte[256 /*0x0100*/]
  {
    (byte) 0,
    (byte) 1,
    (byte) 2,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8,
    (byte) 8
  };
  public static readonly BigInteger ZERO = new BigInteger(0, new byte[0]);
  public static readonly BigInteger ONE = BigInteger.valueOf(1L);
  private static readonly BigInteger TWO = BigInteger.valueOf(2L);

  private BigInteger()
  {
  }

  private BigInteger(int signum, int[] mag)
  {
    this.sign = signum;
    if (mag.Length > 0)
    {
      int sourceIndex = 0;
      while (sourceIndex < mag.Length && mag[sourceIndex] == 0)
        ++sourceIndex;
      if (sourceIndex == 0)
      {
        this.magnitude = mag;
      }
      else
      {
        int[] destinationArray = new int[mag.Length - sourceIndex];
        Array.Copy((Array) mag, sourceIndex, (Array) destinationArray, 0, destinationArray.Length);
        this.magnitude = destinationArray;
        if (destinationArray.Length != 0)
          return;
        this.sign = 0;
      }
    }
    else
    {
      this.magnitude = mag;
      this.sign = 0;
    }
  }

  public BigInteger(string sval)
    : this(sval, 10)
  {
  }

  public BigInteger(string sval, int rdx)
  {
    if (sval.Length == 0)
      throw new FormatException("Zero length BigInteger");
    NumberStyles style;
    switch (rdx)
    {
      case 10:
        style = NumberStyles.Integer;
        break;
      case 16 /*0x10*/:
        style = NumberStyles.AllowHexSpecifier;
        break;
      default:
        throw new FormatException("Only base 10 or 16 alllowed");
    }
    int index = 0;
    this.sign = 1;
    if (sval[0] == '-')
    {
      if (sval.Length == 1)
        throw new FormatException("Zero length BigInteger");
      this.sign = -1;
      index = 1;
    }
    while (index < sval.Length && int.Parse(sval[index].ToString(), style) == 0)
      ++index;
    if (index >= sval.Length)
    {
      this.sign = 0;
      this.magnitude = new int[0];
    }
    else
    {
      BigInteger bigInteger = BigInteger.ZERO;
      BigInteger val = BigInteger.valueOf((long) rdx);
      for (; index < sval.Length; ++index)
        bigInteger = bigInteger.multiply(val).add(BigInteger.valueOf((long) int.Parse(sval[index].ToString(), style)));
      this.magnitude = bigInteger.magnitude;
    }
  }

  public BigInteger(byte[] bval)
  {
    if (bval.Length == 0)
      throw new FormatException("Zero length BigInteger");
    this.sign = 1;
    if (bval[0] < (byte) 0)
    {
      this.sign = -1;
      int index = 0;
      while (index < bval.Length && bval[index] == byte.MaxValue)
        ++index;
      this.magnitude = new int[(bval.Length - index) / 2 + 1];
    }
    else
      this.magnitude = this.makeMagnitude(bval);
  }

  private int[] makeMagnitude(byte[] bval)
  {
    int index1 = 0;
    while (index1 < bval.Length && bval[index1] == (byte) 0)
      ++index1;
    if (index1 >= bval.Length)
      return new int[0];
    int length = (bval.Length - index1 + 3) / 4;
    int num1 = (bval.Length - index1) % 4;
    if (num1 == 0)
      num1 = 4;
    int[] numArray = new int[length];
    int num2 = 0;
    int index2 = 0;
    for (int index3 = index1; index3 < bval.Length; ++index3)
    {
      num2 = num2 << 8 | (int) bval[index3] & (int) byte.MaxValue;
      --num1;
      if (num1 <= 0)
      {
        numArray[index2] = num2;
        ++index2;
        num1 = 4;
        num2 = 0;
      }
    }
    if (index2 < numArray.Length)
      numArray[index2] = num2;
    return numArray;
  }

  public BigInteger(int sign, byte[] mag)
  {
    if (sign < -1 || sign > 1)
      throw new FormatException("Invalid sign value");
    if (sign == 0)
    {
      this.sign = 0;
      this.magnitude = new int[0];
    }
    else
    {
      this.magnitude = this.makeMagnitude(mag);
      this.sign = sign;
    }
  }

  public BigInteger(int numBits, Random rnd)
  {
    if (numBits < 0)
      throw new ArgumentException("numBits must be non-negative");
    int length = (numBits + 7) / 8;
    byte[] numArray = new byte[length];
    if (length > 0)
    {
      this.nextRndBytes(rnd, numArray);
      numArray[0] &= BigInteger.rndMask[8 * length - numBits];
    }
    this.magnitude = this.makeMagnitude(numArray);
    this.sign = 1;
    this.nBits = -1;
    this.nBitLength = -1;
  }

  private void nextRndBytes(Random rnd, byte[] bytes)
  {
    int length = bytes.Length;
    int num1 = 0;
    int num2 = 0;
    if (typeof (SecureRandom).IsInstanceOfType((object) rnd))
    {
      ((SecureRandom) rnd).nextBytes(bytes);
    }
    else
    {
label_2:
      for (int index = 0; index < BigInteger.BYTES_PER_INT; ++index)
      {
        if (num1 == length)
          return;
        num2 = index == 0 ? rnd.Next() : num2 >> BigInteger.BITS_PER_BYTE;
        bytes[num1++] = (byte) num2;
      }
      goto label_2;
    }
  }

  public BigInteger(int bitLength, int certainty, Random rnd)
  {
    int length = (bitLength + 7) / 8;
    byte[] numArray = new byte[length];
    do
    {
      if (length > 0)
      {
        this.nextRndBytes(rnd, numArray);
        numArray[0] &= BigInteger.rndMask[8 * length - bitLength];
      }
      this.magnitude = this.makeMagnitude(numArray);
      this.sign = 1;
      this.nBits = -1;
      this.nBitLength = -1;
      this.mQuote = -1L;
      if (certainty > 0 && bitLength > 2)
        this.magnitude[this.magnitude.Length - 1] |= 1;
    }
    while (this.bitLength() != bitLength || !this.isProbablePrime(certainty));
  }

  public BigInteger abs() => this.sign < 0 ? this.negate() : this;

  private int[] add(int[] a, int[] b)
  {
    int index = a.Length - 1;
    int num1 = b.Length - 1;
    long num2 = 0;
    while (num1 >= 0)
    {
      long num3 = num2 + (((long) a[index] & (long) uint.MaxValue) + ((long) b[num1--] & (long) uint.MaxValue));
      a[index--] = (int) num3;
      num2 = num3 >>> 32 /*0x20*/;
    }
    long num4;
    for (; index >= 0 && num2 != 0L; num2 = num4 >>> 32 /*0x20*/)
    {
      num4 = num2 + ((long) a[index] & (long) uint.MaxValue);
      a[index--] = (int) num4;
    }
    return a;
  }

  public BigInteger add(BigInteger val)
  {
    if (val.sign == 0 || val.magnitude.Length == 0)
      return this;
    if (this.sign == 0 || this.magnitude.Length == 0)
      return val;
    if (val.sign < 0)
    {
      if (this.sign > 0)
        return this.subtract(val.negate());
    }
    else if (this.sign < 0)
      return val.subtract(this.negate());
    int[] numArray;
    int[] magnitude;
    if (this.magnitude.Length < val.magnitude.Length)
    {
      numArray = new int[val.magnitude.Length + 1];
      Array.Copy((Array) val.magnitude, 0, (Array) numArray, 1, val.magnitude.Length);
      magnitude = this.magnitude;
    }
    else
    {
      numArray = new int[this.magnitude.Length + 1];
      Array.Copy((Array) this.magnitude, 0, (Array) numArray, 1, this.magnitude.Length);
      magnitude = val.magnitude;
    }
    return new BigInteger(this.sign, this.add(numArray, magnitude));
  }

  private int bitLength(int indx, int[] mag)
  {
    if (mag.Length == 0)
      return 0;
    while (indx != mag.Length && mag[indx] == 0)
      ++indx;
    if (indx == mag.Length)
      return 0;
    int num = 32 /*0x20*/ * (mag.Length - indx - 1) + BigInteger.bitLen(mag[indx]);
    if (this.sign < 0)
    {
      bool flag = (int) BigInteger.bitCounts[mag[indx] & (int) byte.MaxValue] + (int) BigInteger.bitCounts[mag[indx] >> 8 & (int) byte.MaxValue] + (int) BigInteger.bitCounts[mag[indx] >> 16 /*0x10*/ & (int) byte.MaxValue] + (int) BigInteger.bitCounts[mag[indx] >> 24 & (int) byte.MaxValue] == 1;
      for (int index = indx + 1; index < mag.Length && flag; ++index)
        flag = mag[index] == 0;
      num -= flag ? 1 : 0;
    }
    return num;
  }

  public int bitLength()
  {
    if (this.nBitLength == -1)
      this.nBitLength = this.sign != 0 ? this.bitLength(0, this.magnitude) : 0;
    return this.nBitLength;
  }

  private static int bitLen(int w)
  {
    if (w >= 32768 /*0x8000*/)
      return w >= 8388608 /*0x800000*/ ? (w >= 134217728 /*0x08000000*/ ? (w >= 536870912 /*0x20000000*/ ? (w >= 1073741824 /*0x40000000*/ ? 31 /*0x1F*/ : 30) : (w >= 268435456 /*0x10000000*/ ? 29 : 28)) : (w >= 33554432 /*0x02000000*/ ? (w >= 67108864 /*0x04000000*/ ? 27 : 26) : (w >= 16777216 /*0x01000000*/ ? 25 : 24))) : (w >= 524288 /*0x080000*/ ? (w >= 2097152 /*0x200000*/ ? (w >= 4194304 /*0x400000*/ ? 23 : 22) : (w >= 1048576 /*0x100000*/ ? 21 : 20)) : (w >= 131072 /*0x020000*/ ? (w >= 262144 /*0x040000*/ ? 19 : 18) : (w >= 65536 /*0x010000*/ ? 17 : 16 /*0x10*/)));
    if (w >= 128 /*0x80*/)
      return w >= 2048 /*0x0800*/ ? (w >= 8192 /*0x2000*/ ? (w >= 16384 /*0x4000*/ ? 15 : 14) : (w >= 4096 /*0x1000*/ ? 13 : 12)) : (w >= 512 /*0x0200*/ ? (w >= 1024 /*0x0400*/ ? 11 : 10) : (w >= 256 /*0x0100*/ ? 9 : 8));
    if (w >= 8)
      return w >= 32 /*0x20*/ ? (w >= 64 /*0x40*/ ? 7 : 6) : (w >= 16 /*0x10*/ ? 5 : 4);
    if (w >= 2)
      return w >= 4 ? 3 : 2;
    if (w >= 1)
      return 1;
    return w >= 0 ? 0 : 32 /*0x20*/;
  }

  private int compareTo(int xIndx, int[] x, int yIndx, int[] y)
  {
    while (xIndx != x.Length && x[xIndx] == 0)
      ++xIndx;
    while (yIndx != y.Length && y[yIndx] == 0)
      ++yIndx;
    if (x.Length - xIndx < y.Length - yIndx)
      return -1;
    if (x.Length - xIndx > y.Length - yIndx)
      return 1;
    while (xIndx < x.Length)
    {
      long num1 = (long) x[xIndx++] & (long) uint.MaxValue;
      long num2 = (long) y[yIndx++] & (long) uint.MaxValue;
      if (num1 < num2)
        return -1;
      if (num1 > num2)
        return 1;
    }
    return 0;
  }

  public int compareTo(BigInteger val)
  {
    if (this.sign < val.sign)
      return -1;
    return this.sign > val.sign ? 1 : this.compareTo(0, this.magnitude, 0, val.magnitude);
  }

  private int[] divide(int[] x, int[] y)
  {
    int num1 = this.compareTo(0, x, 0, y);
    int[] numArray1;
    if (num1 > 0)
    {
      int num2 = this.bitLength(0, x) - this.bitLength(0, y);
      int[] numArray2;
      if (num2 > 1)
      {
        numArray2 = this.shiftLeft(y, num2 - 1);
        numArray1 = this.shiftLeft(BigInteger.ONE.magnitude, num2 - 1);
        if (num2 % 32 /*0x20*/ == 0)
        {
          int[] destinationArray = new int[num2 / 32 /*0x20*/ + 1];
          Array.Copy((Array) numArray1, 0, (Array) destinationArray, 1, destinationArray.Length - 1);
          destinationArray[0] = 0;
          numArray1 = destinationArray;
        }
      }
      else
      {
        numArray2 = new int[x.Length];
        numArray1 = new int[1];
        Array.Copy((Array) y, 0, (Array) numArray2, numArray2.Length - y.Length, y.Length);
        numArray1[0] = 1;
      }
      int[] numArray3 = new int[numArray1.Length];
      this.subtract(0, x, 0, numArray2);
      Array.Copy((Array) numArray1, 0, (Array) numArray3, 0, numArray1.Length);
      int index1 = 0;
      int index2 = 0;
      int start = 0;
      int num3;
      while (true)
      {
        do
        {
          for (int index3 = this.compareTo(index1, x, index2, numArray2); index3 >= 0; index3 = this.compareTo(index1, x, index2, numArray2))
          {
            this.subtract(index1, x, index2, numArray2);
            this.add(numArray1, numArray3);
          }
          num3 = this.compareTo(index1, x, 0, y);
          if (num3 > 0)
          {
            if (x[index1] == 0)
              ++index1;
            int n = this.bitLength(index2, numArray2) - this.bitLength(index1, x);
            if (n == 0)
            {
              numArray2 = this.shiftRightOne(index2, numArray2);
              numArray3 = this.shiftRightOne(start, numArray3);
            }
            else
            {
              numArray2 = this.shiftRight(index2, numArray2, n);
              numArray3 = this.shiftRight(start, numArray3, n);
            }
            if (numArray2[index2] == 0)
              ++index2;
          }
          else
            goto label_19;
        }
        while (numArray3[start] != 0);
        ++start;
      }
label_19:
      if (num3 == 0)
      {
        this.add(numArray1, BigInteger.ONE.magnitude);
        for (int index4 = index1; index4 != x.Length; ++index4)
          x[index4] = 0;
      }
    }
    else if (num1 == 0)
      numArray1 = new int[1]{ 1 };
    else
      numArray1 = new int[1]{ 0 };
    return numArray1;
  }

  public BigInteger divide(BigInteger val)
  {
    if (val.sign == 0)
      throw new ArithmeticException("Divide by zero");
    if (this.sign == 0)
      return BigInteger.ZERO;
    if (val.compareTo(BigInteger.ONE) == 0)
      return this;
    int[] numArray = new int[this.magnitude.Length];
    Array.Copy((Array) this.magnitude, 0, (Array) numArray, 0, numArray.Length);
    return new BigInteger(this.sign * val.sign, this.divide(numArray, val.magnitude));
  }

  public override bool Equals(object val)
  {
    if (val == this)
      return true;
    if (!typeof (BigInteger).IsInstanceOfType(val))
      return false;
    BigInteger bigInteger = (BigInteger) val;
    if (bigInteger.sign != this.sign || bigInteger.magnitude.Length != this.magnitude.Length)
      return false;
    for (int index = 0; index < this.magnitude.Length; ++index)
    {
      if (bigInteger.magnitude[index] != this.magnitude[index])
        return false;
    }
    return true;
  }

  public bool isProbablePrime(int certainty)
  {
    if (certainty == 0)
      return true;
    BigInteger bigInteger1 = this.abs();
    if (bigInteger1.Equals((object) BigInteger.TWO))
      return true;
    if (bigInteger1.Equals((object) BigInteger.ONE) || !bigInteger1.testBit(0))
      return false;
    if ((certainty & 1) == 1)
      certainty = certainty / 2 + 1;
    else
      certainty /= 2;
    BigInteger bigInteger2 = bigInteger1.subtract(BigInteger.ONE);
    int lowestSetBit = bigInteger2.getLowestSetBit();
    BigInteger exponent = bigInteger2.shiftRight(lowestSetBit);
    Random rnd = new Random();
    for (int index = 0; index <= certainty; ++index)
    {
      BigInteger bigInteger3;
      do
      {
        bigInteger3 = new BigInteger(bigInteger1.bitLength(), rnd);
      }
      while (bigInteger3.compareTo(BigInteger.ONE) <= 0 || bigInteger3.compareTo(bigInteger1) >= 0);
      int num = 0;
      for (BigInteger bigInteger4 = bigInteger3.modPow(exponent, bigInteger1); (num != 0 || !bigInteger4.Equals((object) BigInteger.ONE)) && !bigInteger4.Equals((object) bigInteger1.subtract(BigInteger.ONE)); bigInteger4 = bigInteger4.modPow(BigInteger.TWO, bigInteger1))
      {
        if (num > 0 && bigInteger4.Equals((object) BigInteger.ONE) || ++num == lowestSetBit)
          return false;
      }
    }
    return true;
  }

  public long longValue()
  {
    if (this.magnitude.Length == 0)
      return 0;
    long num = this.magnitude.Length <= 1 ? (long) this.magnitude[this.magnitude.Length - 1] & (long) uint.MaxValue : (long) this.magnitude[this.magnitude.Length - 2] << 32 /*0x20*/ | (long) this.magnitude[this.magnitude.Length - 1] & (long) uint.MaxValue;
    return this.sign < 0 ? -num : num;
  }

  public BigInteger mod(BigInteger m)
  {
    BigInteger bigInteger = m.sign > 0 ? this.remainder(m) : throw new ArithmeticException("BigInteger: modulus is not positive");
    return bigInteger.sign < 0 ? bigInteger.add(m) : bigInteger;
  }

  public BigInteger modInverse(BigInteger m)
  {
    if (m.sign != 1)
      throw new ArithmeticException("Modulus must be positive");
    BigInteger u1Out = new BigInteger();
    BigInteger u2Out = new BigInteger();
    if (!BigInteger.extEuclid(this, m, u1Out, u2Out).Equals((object) BigInteger.ONE))
      throw new ArithmeticException("Numbers not relatively prime.");
    if (u1Out.compareTo(BigInteger.ZERO) < 0)
      u1Out = u1Out.add(m);
    return u1Out;
  }

  private static BigInteger extEuclid(
    BigInteger a,
    BigInteger b,
    BigInteger u1Out,
    BigInteger u2Out)
  {
    BigInteger bigInteger1 = BigInteger.ONE;
    BigInteger bigInteger2 = a;
    BigInteger bigInteger3 = BigInteger.ZERO;
    BigInteger bigInteger4;
    for (BigInteger val1 = b; val1.compareTo(BigInteger.ZERO) > 0; val1 = bigInteger4)
    {
      BigInteger val2 = bigInteger2.divide(val1);
      BigInteger bigInteger5 = bigInteger1.subtract(bigInteger3.multiply(val2));
      bigInteger1 = bigInteger3;
      bigInteger3 = bigInteger5;
      bigInteger4 = bigInteger2.subtract(val1.multiply(val2));
      bigInteger2 = val1;
    }
    u1Out.sign = bigInteger1.sign;
    u1Out.magnitude = bigInteger1.magnitude;
    BigInteger bigInteger6 = bigInteger2.subtract(bigInteger1.multiply(a)).divide(b);
    u2Out.sign = bigInteger6.sign;
    u2Out.magnitude = bigInteger6.magnitude;
    return bigInteger2;
  }

  private void zero(int[] x)
  {
    for (int index = 0; index != x.Length; ++index)
      x[index] = 0;
  }

  public BigInteger modPow(BigInteger exponent, BigInteger m)
  {
    int[] numArray1 = (int[]) null;
    int[] numArray2 = (int[]) null;
    bool flag = (m.magnitude[m.magnitude.Length - 1] & 1) == 1;
    long mQuote = 0;
    if (flag)
    {
      mQuote = m.getMQuote();
      numArray1 = this.shiftLeft(32 /*0x20*/ * m.magnitude.Length).mod(m).magnitude;
      flag = numArray1.Length <= m.magnitude.Length;
      if (flag)
      {
        numArray2 = new int[m.magnitude.Length + 1];
        if (numArray1.Length < m.magnitude.Length)
        {
          int[] destinationArray = new int[m.magnitude.Length];
          Array.Copy((Array) numArray1, 0, (Array) destinationArray, destinationArray.Length - numArray1.Length, numArray1.Length);
          numArray1 = destinationArray;
        }
      }
    }
    if (!flag)
    {
      if (this.magnitude.Length <= m.magnitude.Length)
      {
        numArray1 = new int[m.magnitude.Length];
        Array.Copy((Array) this.magnitude, 0, (Array) numArray1, numArray1.Length - this.magnitude.Length, this.magnitude.Length);
      }
      else
      {
        BigInteger bigInteger = this.remainder(m);
        numArray1 = new int[m.magnitude.Length];
        Array.Copy((Array) bigInteger.magnitude, 0, (Array) numArray1, numArray1.Length - bigInteger.magnitude.Length, bigInteger.magnitude.Length);
      }
      numArray2 = new int[m.magnitude.Length * 2];
    }
    int[] numArray3 = new int[m.magnitude.Length];
    for (int index = 0; index < exponent.magnitude.Length; ++index)
    {
      int num1 = exponent.magnitude[index];
      int num2 = 0;
      if (index == 0)
      {
        while (num1 > 0)
        {
          num1 <<= 1;
          ++num2;
        }
        Array.Copy((Array) numArray1, 0, (Array) numArray3, 0, numArray1.Length);
        num1 <<= 1;
        ++num2;
      }
      for (; num1 != 0; num1 <<= 1)
      {
        if (flag)
        {
          this.multiplyMonty(numArray2, numArray3, numArray3, m.magnitude, mQuote);
        }
        else
        {
          this.square(numArray2, numArray3);
          this.remainder(numArray2, m.magnitude);
          Array.Copy((Array) numArray2, numArray2.Length - numArray3.Length, (Array) numArray3, 0, numArray3.Length);
          this.zero(numArray2);
        }
        ++num2;
        if (num1 < 0)
        {
          if (flag)
          {
            this.multiplyMonty(numArray2, numArray3, numArray1, m.magnitude, mQuote);
          }
          else
          {
            this.multiply(numArray2, numArray3, numArray1);
            this.remainder(numArray2, m.magnitude);
            Array.Copy((Array) numArray2, numArray2.Length - numArray3.Length, (Array) numArray3, 0, numArray3.Length);
            this.zero(numArray2);
          }
        }
      }
      for (; num2 < 32 /*0x20*/; ++num2)
      {
        if (flag)
        {
          this.multiplyMonty(numArray2, numArray3, numArray3, m.magnitude, mQuote);
        }
        else
        {
          this.square(numArray2, numArray3);
          this.remainder(numArray2, m.magnitude);
          Array.Copy((Array) numArray2, numArray2.Length - numArray3.Length, (Array) numArray3, 0, numArray3.Length);
          this.zero(numArray2);
        }
      }
    }
    if (flag)
    {
      this.zero(numArray1);
      numArray1[numArray1.Length - 1] = 1;
      this.multiplyMonty(numArray2, numArray3, numArray1, m.magnitude, mQuote);
    }
    return new BigInteger(1, numArray3);
  }

  private int[] square(int[] w, int[] x)
  {
    if (w.Length != 2 * x.Length)
      throw new ArgumentException("no I don't think so...");
    for (int index1 = x.Length - 1; index1 != 0; --index1)
    {
      long num1 = (long) x[index1] & (long) uint.MaxValue;
      long num2 = num1 * num1;
      long num3 = num2 >>> 32 /*0x20*/;
      long num4 = (num2 & (long) uint.MaxValue) + ((long) w[2 * index1 + 1] & (long) uint.MaxValue);
      w[2 * index1 + 1] = (int) num4;
      long num5 = num3 + (num4 >> 32 /*0x20*/);
      for (int index2 = index1 - 1; index2 >= 0; --index2)
      {
        long num6 = ((long) x[index2] & (long) uint.MaxValue) * num1;
        long num7 = num6 >>> 31 /*0x1F*/;
        long num8 = ((num6 & (long) int.MaxValue) << 1) + (((long) w[index1 + index2 + 1] & (long) uint.MaxValue) + num5);
        w[index1 + index2 + 1] = (int) num8;
        num5 = num7 + (num8 >>> 32 /*0x20*/);
      }
      long num9 = num5 + ((long) w[index1] & (long) uint.MaxValue);
      w[index1] = (int) num9;
      w[index1 - 1] = (int) (num9 >> 32 /*0x20*/);
    }
    long num10 = (long) x[0] & (long) uint.MaxValue;
    long num11 = num10 * num10;
    long num12 = num11 >>> 32 /*0x20*/;
    long num13 = (num11 & (long) uint.MaxValue) + ((long) w[1] & (long) uint.MaxValue);
    w[1] = (int) num13;
    w[0] = (int) (num12 + (num13 >> 32 /*0x20*/) + (long) w[0]);
    return w;
  }

  private int[] multiply(int[] x, int[] y, int[] z)
  {
    for (int index1 = z.Length - 1; index1 >= 0; --index1)
    {
      long num1 = (long) z[index1] & (long) uint.MaxValue;
      long num2 = 0;
      for (int index2 = y.Length - 1; index2 >= 0; --index2)
      {
        long num3 = num2 + (num1 * ((long) y[index2] & (long) uint.MaxValue) + ((long) x[index1 + index2 + 1] & (long) uint.MaxValue));
        x[index1 + index2 + 1] = (int) num3;
        num2 = num3 >>> 32 /*0x20*/;
      }
      x[index1] = (int) num2;
    }
    return x;
  }

  private long getMQuote()
  {
    if (this.mQuote != -1L)
      return this.mQuote;
    if ((this.magnitude[this.magnitude.Length - 1] & 1) == 0)
      return -1;
    BigInteger m = new BigInteger(1, new byte[5]
    {
      (byte) 1,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    });
    this.mQuote = this.negate().mod(m).modInverse(m).longValue();
    return this.mQuote;
  }

  public void multiplyMonty(int[] a, int[] x, int[] y, int[] m, long mQuote)
  {
    int length = m.Length;
    int num1 = length - 1;
    long num2 = (long) y[length - 1] & (long) uint.MaxValue;
    for (int index = 0; index <= length; ++index)
      a[index] = 0;
    for (int index1 = length; index1 > 0; --index1)
    {
      long num3 = (long) x[index1 - 1] & (long) uint.MaxValue;
      long num4 = (((long) a[length] & (long) uint.MaxValue) + (num3 * num2 & (long) uint.MaxValue) & (long) uint.MaxValue) * mQuote & (long) uint.MaxValue;
      long num5 = num3 * num2;
      long num6 = num4 * ((long) m[length - 1] & (long) uint.MaxValue);
      long num7 = ((long) a[length] & (long) uint.MaxValue) + (num5 & (long) uint.MaxValue) + (num6 & (long) uint.MaxValue);
      long num8 = (num5 >>> 32 /*0x20*/) + (num6 >>> 32 /*0x20*/) + (num7 >>> 32 /*0x20*/);
      for (int index2 = num1; index2 > 0; --index2)
      {
        long num9 = num3 * ((long) y[index2 - 1] & (long) uint.MaxValue);
        long num10 = num4 * ((long) m[index2 - 1] & (long) uint.MaxValue);
        long num11 = ((long) a[index2] & (long) uint.MaxValue) + (num9 & (long) uint.MaxValue) + (num10 & (long) uint.MaxValue) + (num8 & (long) uint.MaxValue);
        num8 = (num8 >>> 32 /*0x20*/) + (num9 >>> 32 /*0x20*/) + (num10 >>> 32 /*0x20*/) + (num11 >>> 32 /*0x20*/);
        a[index2 + 1] = (int) num11;
      }
      long num12 = num8 + ((long) a[0] & (long) uint.MaxValue);
      a[1] = (int) num12;
      a[0] = (int) (num12 >>> 32 /*0x20*/);
    }
    if (this.compareTo(0, a, 0, m) >= 0)
      this.subtract(0, a, 0, m);
    for (int index = 0; index < length; ++index)
      x[index] = a[index + 1];
  }

  public BigInteger multiply(BigInteger val)
  {
    if (this.sign == 0 || val.sign == 0)
      return BigInteger.ZERO;
    int[] x = new int[this.magnitude.Length + val.magnitude.Length];
    return new BigInteger(this.sign * val.sign, this.multiply(x, this.magnitude, val.magnitude));
  }

  public BigInteger negate() => new BigInteger(-this.sign, this.magnitude);

  private int[] remainder(int[] x, int[] y)
  {
    int num1 = this.compareTo(0, x, 0, y);
    if (num1 > 0)
    {
      int num2 = this.bitLength(0, x) - this.bitLength(0, y);
      int[] numArray;
      if (num2 > 1)
      {
        numArray = this.shiftLeft(y, num2 - 1);
      }
      else
      {
        numArray = new int[x.Length];
        Array.Copy((Array) y, 0, (Array) numArray, numArray.Length - y.Length, y.Length);
      }
      this.subtract(0, x, 0, numArray);
      int index1 = 0;
      int index2 = 0;
      int num3;
      while (true)
      {
        do
        {
          for (int index3 = this.compareTo(index1, x, index2, numArray); index3 >= 0; index3 = this.compareTo(index1, x, index2, numArray))
            this.subtract(index1, x, index2, numArray);
          num3 = this.compareTo(index1, x, 0, y);
          if (num3 > 0)
          {
            if (x[index1] == 0)
              ++index1;
            int n = this.bitLength(index2, numArray) - this.bitLength(index1, x);
            numArray = n != 0 ? this.shiftRight(index2, numArray, n) : this.shiftRightOne(index2, numArray);
          }
          else
            goto label_13;
        }
        while (numArray[index2] != 0);
        ++index2;
      }
label_13:
      if (num3 == 0)
      {
        for (int index4 = index1; index4 != x.Length; ++index4)
          x[index4] = 0;
      }
    }
    else if (num1 == 0)
    {
      for (int index = 0; index != x.Length; ++index)
        x[index] = 0;
    }
    return x;
  }

  public BigInteger remainder(BigInteger val)
  {
    if (val.sign == 0)
      throw new ArithmeticException("BigInteger: Divide by zero");
    if (this.sign == 0)
      return BigInteger.ZERO;
    int[] numArray = new int[this.magnitude.Length];
    Array.Copy((Array) this.magnitude, 0, (Array) numArray, 0, numArray.Length);
    return new BigInteger(this.sign, this.remainder(numArray, val.magnitude));
  }

  private int[] shiftLeft(int[] mag, int n)
  {
    int num1 = n >>> 5;
    int num2 = n & 31 /*0x1F*/;
    int length = mag.Length;
    int[] numArray;
    if (num2 == 0)
    {
      numArray = new int[length + num1];
      for (int index = 0; index < length; ++index)
        numArray[index] = mag[index];
    }
    else
    {
      int index1 = 0;
      int num3 = 32 /*0x20*/ - num2;
      int num4 = mag[0] >>> num3;
      if (num4 != 0)
      {
        numArray = new int[length + num1 + 1];
        numArray[index1++] = num4;
      }
      else
        numArray = new int[length + num1];
      int num5 = mag[0];
      for (int index2 = 0; index2 < length - 1; ++index2)
      {
        int num6 = mag[index2 + 1];
        numArray[index1++] = num5 << num2 | num6 >>> num3;
        num5 = num6;
      }
      numArray[index1] = mag[length - 1] << num2;
    }
    return numArray;
  }

  public BigInteger shiftLeft(int n)
  {
    if (this.sign == 0 || this.magnitude.Length == 0)
      return BigInteger.ZERO;
    if (n == 0)
      return this;
    return n < 0 ? this.shiftRight(-n) : new BigInteger(this.sign, this.shiftLeft(this.magnitude, n));
  }

  private int[] shiftRight(int start, int[] mag, int n)
  {
    int index1 = (n >>> 5) + start;
    int num1 = n & 31 /*0x1F*/;
    int length = mag.Length;
    if (index1 != start)
    {
      int num2 = index1 - start;
      for (int index2 = length - 1; index2 >= index1; --index2)
        mag[index2] = mag[index2 - num2];
      for (int index3 = index1 - 1; index3 >= start; --index3)
        mag[index3] = 0;
    }
    if (num1 != 0)
    {
      int num3 = 32 /*0x20*/ - num1;
      int num4 = mag[length - 1];
      for (int index4 = length - 1; index4 >= index1 + 1; --index4)
      {
        int num5 = mag[index4 - 1];
        mag[index4] = num4 >>> num1 | num5 << num3;
        num4 = num5;
      }
      mag[index1] = mag[index1] >>> num1;
    }
    return mag;
  }

  private int[] shiftRightOne(int start, int[] mag)
  {
    int length = mag.Length;
    int num1 = mag[length - 1];
    for (int index = length - 1; index >= start + 1; --index)
    {
      int num2 = mag[index - 1];
      mag[index] = num1 >>> 1 | num2 << 31 /*0x1F*/;
      num1 = num2;
    }
    mag[start] = mag[start] >>> 1;
    return mag;
  }

  public BigInteger shiftRight(int n)
  {
    if (n == 0)
      return this;
    if (n < 0)
      return this.shiftLeft(-n);
    if (n >= this.bitLength())
      return this.sign >= 0 ? BigInteger.ZERO : BigInteger.valueOf(-1L);
    int[] numArray = new int[this.magnitude.Length];
    Array.Copy((Array) this.magnitude, 0, (Array) numArray, 0, numArray.Length);
    return new BigInteger(this.sign, this.shiftRight(0, numArray, n));
  }

  private int[] subtract(int xStart, int[] x, int yStart, int[] y)
  {
    int index = x.Length - 1;
    int num1 = y.Length - 1;
    int num2 = 0;
    do
    {
      long num3 = ((long) x[index] & (long) uint.MaxValue) - ((long) y[num1--] & (long) uint.MaxValue) + (long) num2;
      x[index--] = (int) num3;
      num2 = num3 >= 0L ? 0 : -1;
    }
    while (num1 >= yStart);
    while (index >= xStart)
    {
      long num4 = ((long) x[index] & (long) uint.MaxValue) + (long) num2;
      x[index--] = (int) num4;
      if (num4 < 0L)
        num2 = -1;
      else
        break;
    }
    return x;
  }

  public BigInteger subtract(BigInteger val)
  {
    if (val.sign == 0 || val.magnitude.Length == 0)
      return this;
    if (this.sign == 0 || this.magnitude.Length == 0)
      return val.negate();
    if (val.sign < 0)
    {
      if (this.sign > 0)
        return this.add(val.negate());
    }
    else if (this.sign < 0)
      return this.add(val.negate());
    int num = this.compareTo(val);
    if (num == 0)
      return BigInteger.ZERO;
    BigInteger bigInteger1;
    BigInteger bigInteger2;
    if (num < 0)
    {
      bigInteger1 = val;
      bigInteger2 = this;
    }
    else
    {
      bigInteger1 = this;
      bigInteger2 = val;
    }
    int[] numArray = new int[bigInteger1.magnitude.Length];
    Array.Copy((Array) bigInteger1.magnitude, 0, (Array) numArray, 0, numArray.Length);
    return new BigInteger(this.sign * num, this.subtract(0, numArray, 0, bigInteger2.magnitude));
  }

  public byte[] toByteArray()
  {
    byte[] byteArray = new byte[this.bitLength() / 8 + 1];
    int num1 = 4;
    int num2 = 0;
    int num3 = this.magnitude.Length - 1;
    int num4 = 1;
    for (int index = byteArray.Length - 1; index >= 0; --index)
    {
      if (num1 == 4 && num3 >= 0)
      {
        if (this.sign < 0)
        {
          long num5 = ((long) ~this.magnitude[num3--] & (long) uint.MaxValue) + (long) num4;
          num4 = (num5 & -4294967296L) == 0L ? 0 : 1;
          num2 = (int) (num5 & (long) uint.MaxValue);
        }
        else
          num2 = this.magnitude[num3--];
        num1 = 1;
      }
      else
      {
        num2 >>>= 8;
        ++num1;
      }
      byteArray[index] = (byte) num2;
    }
    return byteArray;
  }

  public override string ToString() => this.ToString(10);

  public string ToString(int rdx)
  {
    string format;
    switch (rdx)
    {
      case 10:
        format = "d";
        break;
      case 16 /*0x10*/:
        format = "x";
        break;
      default:
        throw new FormatException("Only base 10 or 16 are allowed");
    }
    if (this.magnitude == null)
      return "null";
    if (this.sign == 0)
      return "0";
    string str1 = "";
    if (rdx == 16 /*0x10*/)
    {
      for (int index = 0; index < this.magnitude.Length; ++index)
      {
        string str2 = "0000000" + this.magnitude[index].ToString("x");
        string str3 = str2.Substring(str2.Length - 8);
        str1 += str3;
      }
    }
    else
    {
      Stack stack = new Stack();
      BigInteger bigInteger1 = new BigInteger(rdx.ToString());
      for (BigInteger bigInteger2 = new BigInteger(this.abs().ToString(16 /*0x10*/), 16 /*0x10*/); !bigInteger2.Equals((object) BigInteger.ZERO); bigInteger2 = bigInteger2.divide(bigInteger1))
      {
        BigInteger bigInteger3 = bigInteger2.mod(bigInteger1);
        if (bigInteger3.Equals((object) BigInteger.ZERO))
          stack.Push((object) "0");
        else
          stack.Push((object) bigInteger3.magnitude[0].ToString(format));
      }
      while (stack.Count != 0)
        str1 += (string) stack.Pop();
    }
    while (str1.Length > 1 && str1[0] == '0')
      str1 = str1.Substring(1);
    if (str1.Length == 0)
      str1 = "0";
    else if (this.sign == -1)
      str1 = "-" + str1;
    return str1;
  }

  public static BigInteger valueOf(long val)
  {
    if (val == 0L)
      return BigInteger.ZERO;
    byte[] bval = new byte[8];
    for (int index = 0; index < 8; ++index)
    {
      bval[7 - index] = (byte) val;
      val >>= 8;
    }
    return new BigInteger(bval);
  }

  public int getLowestSetBit()
  {
    if (this.Equals((object) BigInteger.ZERO))
      return -1;
    int index = this.magnitude.Length - 1;
    while (index >= 0 && this.magnitude[index] == 0)
      --index;
    int num = 31 /*0x1F*/;
    while (num > 0 && this.magnitude[index] << num != int.MinValue)
      --num;
    return (this.magnitude.Length - 1 - index) * 32 /*0x20*/ + (31 /*0x1F*/ - num);
  }

  public bool testBit(int n)
  {
    if (n < 0)
      throw new ArithmeticException("Bit position must not be negative");
    return n / 32 /*0x20*/ >= this.magnitude.Length ? this.sign < 0 : (this.magnitude[this.magnitude.Length - 1 - n / 32 /*0x20*/] >> n % 32 /*0x20*/ & 1) > 0;
  }
}
