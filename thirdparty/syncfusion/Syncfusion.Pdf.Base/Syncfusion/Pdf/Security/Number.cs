// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Number
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal sealed class Number
{
  private const long m_iMask = 4294967295 /*0xFFFFFFFF*/;
  private const ulong m_uMask = 4294967295 /*0xFFFFFFFF*/;
  private const int m_c2 = 1;
  private const int m_c8 = 1;
  private const int m_c10 = 19;
  private const int m_c16 = 16 /*0x10*/;
  private const int m_bByte = 8;
  private const int m_bInt = 32 /*0x20*/;
  private const int m_byteInt = 4;
  internal static readonly int[][] m_lists = new int[64 /*0x40*/][]
  {
    new int[8]{ 3, 5, 7, 11, 13, 17, 19, 23 },
    new int[5]{ 29, 31 /*0x1F*/, 37, 41, 43 },
    new int[5]{ 47, 53, 59, 61, 67 },
    new int[4]{ 71, 73, 79, 83 },
    new int[4]{ 89, 97, 101, 103 },
    new int[4]{ 107, 109, 113, (int) sbyte.MaxValue },
    new int[4]{ 131, 137, 139, 149 },
    new int[4]{ 151, 157, 163, 167 },
    new int[4]{ 173, 179, 181, 191 },
    new int[4]{ 193, 197, 199, 211 },
    new int[3]{ 223, 227, 229 },
    new int[3]{ 233, 239, 241 },
    new int[3]{ 251, 257, 263 },
    new int[3]{ 269, 271, 277 },
    new int[3]{ 281, 283, 293 },
    new int[3]{ 307, 311, 313 },
    new int[3]{ 317, 331, 337 },
    new int[3]{ 347, 349, 353 },
    new int[3]{ 359, 367, 373 },
    new int[3]{ 379, 383, 389 },
    new int[3]{ 397, 401, 409 },
    new int[3]{ 419, 421, 431 },
    new int[3]{ 433, 439, 443 },
    new int[3]{ 449, 457, 461 },
    new int[3]{ 463, 467, 479 },
    new int[3]{ 487, 491, 499 },
    new int[3]{ 503, 509, 521 },
    new int[3]{ 523, 541, 547 },
    new int[3]{ 557, 563, 569 },
    new int[3]{ 571, 577, 587 },
    new int[3]{ 593, 599, 601 },
    new int[3]{ 607, 613, 617 },
    new int[3]{ 619, 631, 641 },
    new int[3]{ 643, 647, 653 },
    new int[3]{ 659, 661, 673 },
    new int[3]{ 677, 683, 691 },
    new int[3]{ 701, 709, 719 },
    new int[3]{ 727, 733, 739 },
    new int[3]{ 743, 751, 757 },
    new int[3]{ 761, 769, 773 },
    new int[3]{ 787, 797, 809 },
    new int[3]{ 811, 821, 823 },
    new int[3]{ 827, 829, 839 },
    new int[3]{ 853, 857, 859 },
    new int[3]{ 863, 877, 881 },
    new int[3]{ 883, 887, 907 },
    new int[3]{ 911, 919, 929 },
    new int[3]{ 937, 941, 947 },
    new int[3]{ 953, 967, 971 },
    new int[3]{ 977, 983, 991 },
    new int[3]{ 997, 1009, 1013 },
    new int[3]{ 1019, 1021, 1031 },
    new int[3]{ 1033, 1039, 1049 },
    new int[3]{ 1051, 1061, 1063 },
    new int[3]{ 1069, 1087, 1091 },
    new int[3]{ 1093, 1097, 1103 },
    new int[3]{ 1109, 1117, 1123 },
    new int[3]{ 1129, 1151, 1153 },
    new int[3]{ 1163, 1171, 1181 },
    new int[3]{ 1187, 1193, 1201 },
    new int[3]{ 1213, 1217, 1223 },
    new int[3]{ 1229, 1231, 1237 },
    new int[3]{ 1249, 1259, 1277 },
    new int[3]{ 1279 /*0x04FF*/, 1283, 1289 }
  };
  internal static readonly int[] m_products;
  private static readonly int[] m_zeroMagnitude = new int[0];
  private static readonly byte[] m_zeroEncoding = new byte[0];
  private static readonly Number[] m_smallConstants = new Number[17];
  public static readonly Number Zero;
  public static readonly Number One;
  public static readonly Number Two;
  public static readonly Number Three;
  public static readonly Number Ten;
  private static readonly byte[] m_bitLengthTable = new byte[256 /*0x0100*/]
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
  private static readonly Number m_r2;
  private static readonly Number m_r2E;
  private static readonly Number m_r8;
  private static readonly Number m_r8E;
  private static readonly Number m_r10;
  private static readonly Number m_r10E;
  private static readonly Number m_r16;
  private static readonly Number m_r16E;
  private static readonly SecureRandomAlgorithm m_rs = new SecureRandomAlgorithm();
  private static readonly int[] m_eT = new int[8]
  {
    7,
    25,
    81,
    241,
    673,
    1793,
    4609,
    int.MaxValue
  };
  private int[] m_magnitude;
  private int m_sign;
  private int m_nBits = -1;
  private int m_nBitLength = -1;
  private int m_quote;

  static Number()
  {
    Number.Zero = new Number(0, Number.m_zeroMagnitude, false);
    Number.Zero.m_nBits = 0;
    Number.Zero.m_nBitLength = 0;
    Number.m_smallConstants[0] = Number.Zero;
    for (uint index = 1; (long) index < (long) Number.m_smallConstants.Length; ++index)
      Number.m_smallConstants[(IntPtr) index] = Number.CreateUValueOf((ulong) index);
    Number.One = Number.m_smallConstants[1];
    Number.Two = Number.m_smallConstants[2];
    Number.Three = Number.m_smallConstants[3];
    Number.Ten = Number.m_smallConstants[10];
    Number.m_r2 = Number.ValueOf(2L);
    Number.m_r2E = Number.m_r2.Pow(1);
    Number.m_r8 = Number.ValueOf(8L);
    Number.m_r8E = Number.m_r8.Pow(1);
    Number.m_r10 = Number.ValueOf(10L);
    Number.m_r10E = Number.m_r10.Pow(19);
    Number.m_r16 = Number.ValueOf(16L /*0x10*/);
    Number.m_r16E = Number.m_r16.Pow(16 /*0x10*/);
    Number.m_products = new int[Number.m_lists.Length];
    for (int index1 = 0; index1 < Number.m_lists.Length; ++index1)
    {
      int[] list = Number.m_lists[index1];
      int num = list[0];
      for (int index2 = 1; index2 < list.Length; ++index2)
        num *= list[index2];
      Number.m_products[index1] = num;
    }
  }

  private static int GetByteLength(int nBits) => (nBits + 8 - 1) / 8;

  private Number(int signum, int[] mag, bool checkMag)
  {
    if (checkMag)
    {
      int sourceIndex = 0;
      while (sourceIndex < mag.Length && mag[sourceIndex] == 0)
        ++sourceIndex;
      if (sourceIndex == mag.Length)
      {
        this.m_sign = 0;
        this.m_magnitude = Number.m_zeroMagnitude;
      }
      else
      {
        this.m_sign = signum;
        if (sourceIndex == 0)
        {
          this.m_magnitude = mag;
        }
        else
        {
          this.m_magnitude = new int[mag.Length - sourceIndex];
          Array.Copy((Array) mag, sourceIndex, (Array) this.m_magnitude, 0, this.m_magnitude.Length);
        }
      }
    }
    else
    {
      this.m_sign = signum;
      this.m_magnitude = mag;
    }
  }

  internal Number(string value)
    : this(value, 10)
  {
  }

  internal Number(string str, int radix)
  {
    NumberStyles style;
    int length;
    Number number1;
    Number val;
    switch (radix)
    {
      case 2:
        style = NumberStyles.Integer;
        length = 1;
        number1 = Number.m_r2;
        val = Number.m_r2E;
        break;
      case 8:
        style = NumberStyles.Integer;
        length = 1;
        number1 = Number.m_r8;
        val = Number.m_r8E;
        break;
      case 10:
        style = NumberStyles.Integer;
        length = 19;
        number1 = Number.m_r10;
        val = Number.m_r10E;
        break;
      case 16 /*0x10*/:
        style = NumberStyles.AllowHexSpecifier;
        length = 16 /*0x10*/;
        number1 = Number.m_r16;
        val = Number.m_r16E;
        break;
      default:
        throw new FormatException("Invalid base specified. Only bases 2, 8, 10, or 16 allowed");
    }
    int num1 = 0;
    this.m_sign = 1;
    if (str[0] == '-')
    {
      if (str.Length == 1)
        throw new FormatException("Invalid length");
      this.m_sign = -1;
      num1 = 1;
    }
    while (num1 < str.Length && int.Parse(str[num1].ToString(), style) == 0)
      ++num1;
    if (num1 >= str.Length)
    {
      this.m_sign = 0;
      this.m_magnitude = Number.m_zeroMagnitude;
    }
    else
    {
      Number number2 = Number.Zero;
      int num2 = num1 + length;
      if (num2 <= str.Length)
      {
        do
        {
          Number uvalueOf = Number.CreateUValueOf(ulong.Parse(str.Substring(num1, length), style));
          Number number3;
          switch (radix)
          {
            case 2:
              number3 = number2.ShiftLeft(1);
              break;
            case 8:
              number3 = number2.ShiftLeft(3);
              break;
            case 16 /*0x10*/:
              number3 = number2.ShiftLeft(64 /*0x40*/);
              break;
            default:
              number3 = number2.Multiply(val);
              break;
          }
          number2 = number3.Add(uvalueOf);
          num1 = num2;
          num2 += length;
        }
        while (num2 <= str.Length);
      }
      if (num1 < str.Length)
      {
        string s = str.Substring(num1);
        Number uvalueOf = Number.CreateUValueOf(ulong.Parse(s, style));
        number2 = number2.m_sign <= 0 ? uvalueOf : (radix != 16 /*0x10*/ ? number2.Multiply(number1.Pow(s.Length)) : number2.ShiftLeft(s.Length << 2)).Add(uvalueOf);
      }
      this.m_magnitude = number2.m_magnitude;
    }
  }

  internal Number(byte[] bytes)
    : this(bytes, 0, bytes.Length)
  {
  }

  internal Number(byte[] bytes, int offset, int length)
  {
    if ((sbyte) bytes[offset] < (sbyte) 0)
    {
      this.m_sign = -1;
      int num = offset + length;
      int index1 = offset;
      while (index1 < num && bytes[index1] == byte.MaxValue)
        ++index1;
      if (index1 >= num)
      {
        this.m_magnitude = Number.One.m_magnitude;
      }
      else
      {
        int length1 = num - index1;
        byte[] bytes1 = new byte[length1];
        int index2 = 0;
        while (index2 < length1)
          bytes1[index2++] = ~bytes[index1++];
        while (bytes1[--index2] == byte.MaxValue)
          bytes1[index2] = (byte) 0;
        ++bytes1[index2];
        this.m_magnitude = Number.MakeMagnitude(bytes1, 0, bytes1.Length);
      }
    }
    else
    {
      this.m_magnitude = Number.MakeMagnitude(bytes, offset, length);
      this.m_sign = this.m_magnitude.Length > 0 ? 1 : 0;
    }
  }

  private static int[] MakeMagnitude(byte[] bytes, int offset, int length)
  {
    int num1 = offset + length;
    int index1 = offset;
    while (index1 < num1 && bytes[index1] == (byte) 0)
      ++index1;
    if (index1 >= num1)
      return Number.m_zeroMagnitude;
    int length1 = (num1 - index1 + 3) / 4;
    int num2 = (num1 - index1) % 4;
    if (num2 == 0)
      num2 = 4;
    if (length1 < 1)
      return Number.m_zeroMagnitude;
    int[] numArray = new int[length1];
    int num3 = 0;
    int index2 = 0;
    for (int index3 = index1; index3 < num1; ++index3)
    {
      num3 = num3 << 8 | (int) bytes[index3] & (int) byte.MaxValue;
      --num2;
      if (num2 <= 0)
      {
        numArray[index2] = num3;
        ++index2;
        num2 = 4;
        num3 = 0;
      }
    }
    if (index2 < numArray.Length)
      numArray[index2] = num3;
    return numArray;
  }

  internal Number(int sign, byte[] bytes)
    : this(sign, bytes, 0, bytes.Length)
  {
  }

  internal Number(int sign, byte[] bytes, int offset, int length)
  {
    if (sign < -1 || sign > 1)
      throw new FormatException("Invalid sign value");
    if (sign == 0)
    {
      this.m_sign = 0;
      this.m_magnitude = Number.m_zeroMagnitude;
    }
    else
    {
      this.m_magnitude = Number.MakeMagnitude(bytes, offset, length);
      this.m_sign = this.m_magnitude.Length < 1 ? 0 : sign;
    }
  }

  internal Number(int value, SecureRandomAlgorithm random)
  {
    if (value < 0)
      throw new ArgumentException("Invalid entry. value must be non-negative");
    this.m_nBits = -1;
    this.m_nBitLength = -1;
    if (value == 0)
    {
      this.m_sign = 0;
      this.m_magnitude = Number.m_zeroMagnitude;
    }
    else
    {
      int byteLength = Number.GetByteLength(value);
      byte[] numArray = new byte[byteLength];
      random.NextBytes(numArray);
      int num = 8 * byteLength - value;
      numArray[0] &= (byte) ((uint) byte.MaxValue >> num);
      this.m_magnitude = Number.MakeMagnitude(numArray, 0, numArray.Length);
      this.m_sign = this.m_magnitude.Length < 1 ? 0 : 1;
    }
  }

  internal Number Absolute() => this.m_sign < 0 ? this.Negate() : this;

  internal static int[] AddMagnitudes(int[] a, int[] b)
  {
    int index = a.Length - 1;
    int num1 = b.Length - 1;
    long num2 = 0;
    while (num1 >= 0)
    {
      long num3 = num2 + ((long) (uint) a[index] + (long) (uint) b[num1--]);
      a[index--] = (int) num3;
      num2 = num3 >>> 32 /*0x20*/;
    }
    if (num2 != 0L)
    {
      while (index >= 0 && ++a[index--] == 0)
        ;
    }
    return a;
  }

  internal Number Add(Number value)
  {
    if (this.m_sign == 0)
      return value;
    if (this.m_sign == value.m_sign)
      return this.AddToMagnitude(value.m_magnitude);
    if (value.m_sign == 0)
      return this;
    return value.m_sign < 0 ? this.Subtract(value.Negate()) : value.Subtract(this.Negate());
  }

  private Number AddToMagnitude(int[] magToAdd)
  {
    int[] numArray;
    int[] b;
    if (this.m_magnitude.Length < magToAdd.Length)
    {
      numArray = magToAdd;
      b = this.m_magnitude;
    }
    else
    {
      numArray = this.m_magnitude;
      b = magToAdd;
    }
    uint maxValue = uint.MaxValue;
    if (numArray.Length == b.Length)
      maxValue -= (uint) b[0];
    bool checkMag = (uint) numArray[0] >= maxValue;
    int[] a;
    if (checkMag)
    {
      a = new int[numArray.Length + 1];
      numArray.CopyTo((Array) a, 1);
    }
    else
      a = (int[]) numArray.Clone();
    return new Number(this.m_sign, Number.AddMagnitudes(a, b), checkMag);
  }

  internal int BitCount
  {
    get
    {
      if (this.m_nBits == -1)
      {
        if (this.m_sign < 0)
        {
          this.m_nBits = this.Not().BitCount;
        }
        else
        {
          int num = 0;
          for (int index = 0; index < this.m_magnitude.Length; ++index)
            num += Number.BitCnt(this.m_magnitude[index]);
          this.m_nBits = num;
        }
      }
      return this.m_nBits;
    }
  }

  internal static int BitCnt(int i)
  {
    uint num1 = (uint) i;
    uint num2 = num1 - (num1 >> 1 & 1431655765U /*0x55555555*/);
    uint num3 = (uint) (((int) num2 & 858993459 /*0x33333333*/) + ((int) (num2 >> 2) & 858993459 /*0x33333333*/));
    uint num4 = (uint) ((int) num3 + (int) (num3 >> 4) & 252645135);
    uint num5 = num4 + (num4 >> 8);
    return (int) (num5 + (num5 >> 16 /*0x10*/) & 63U /*0x3F*/);
  }

  private static int CalcBitLength(int sign, int indx, int[] mag)
  {
    for (; indx < mag.Length; ++indx)
    {
      if (mag[indx] != 0)
      {
        int num1 = 32 /*0x20*/ * (mag.Length - indx - 1);
        int w = mag[indx];
        int num2 = num1 + Number.BitLen(w);
        if (sign < 0 && (w & -w) == w)
        {
          while (++indx < mag.Length)
          {
            if (mag[indx] != 0)
              goto label_8;
          }
          --num2;
        }
label_8:
        return num2;
      }
    }
    return 0;
  }

  internal int BitLength
  {
    get
    {
      if (this.m_nBitLength == -1)
        this.m_nBitLength = this.m_sign == 0 ? 0 : Number.CalcBitLength(this.m_sign, 0, this.m_magnitude);
      return this.m_nBitLength;
    }
  }

  private static int BitLen(int w)
  {
    uint index1 = (uint) w;
    uint index2 = index1 >> 24;
    if (index2 != 0U)
      return 24 + (int) Number.m_bitLengthTable[(IntPtr) index2];
    uint index3 = index1 >> 16 /*0x10*/;
    if (index3 != 0U)
      return 16 /*0x10*/ + (int) Number.m_bitLengthTable[(IntPtr) index3];
    uint index4 = index1 >> 8;
    return index4 != 0U ? 8 + (int) Number.m_bitLengthTable[(IntPtr) index4] : (int) Number.m_bitLengthTable[(IntPtr) index1];
  }

  private bool QuickPow2Check() => this.m_sign > 0 && this.m_nBits == 1;

  internal int CompareTo(object obj) => this.CompareTo((Number) obj);

  private static int CompareTo(int xIndx, int[] x, int yIndx, int[] y)
  {
    while (xIndx != x.Length && x[xIndx] == 0)
      ++xIndx;
    while (yIndx != y.Length && y[yIndx] == 0)
      ++yIndx;
    return Number.CompareNoLeadingZeroes(xIndx, x, yIndx, y);
  }

  private static int CompareNoLeadingZeroes(int xIndx, int[] x, int yIndx, int[] y)
  {
    int num1 = x.Length - y.Length - (xIndx - yIndx);
    if (num1 != 0)
      return num1 >= 0 ? 1 : -1;
    while (xIndx < x.Length)
    {
      uint num2 = (uint) x[xIndx++];
      uint num3 = (uint) y[yIndx++];
      if ((int) num2 != (int) num3)
        return num2 >= num3 ? 1 : -1;
    }
    return 0;
  }

  internal int CompareTo(Number value)
  {
    if (this.m_sign < value.m_sign)
      return -1;
    if (this.m_sign > value.m_sign)
      return 1;
    return this.m_sign != 0 ? this.m_sign * Number.CompareNoLeadingZeroes(0, this.m_magnitude, 0, value.m_magnitude) : 0;
  }

  private int[] Divide(int[] x, int[] y)
  {
    int index1 = 0;
    while (index1 < x.Length && x[index1] == 0)
      ++index1;
    int index2 = 0;
    while (index2 < y.Length && y[index2] == 0)
      ++index2;
    int num1 = Number.CompareNoLeadingZeroes(index1, x, index2, y);
    int[] a;
    if (num1 > 0)
    {
      int num2 = Number.CalcBitLength(1, index2, y);
      int num3 = Number.CalcBitLength(1, index1, x);
      int n1 = num3 - num2;
      int start = 0;
      int index3 = 0;
      int num4 = num2;
      int[] numArray1;
      int[] numArray2;
      if (n1 > 0)
      {
        numArray1 = new int[(n1 >> 5) + 1];
        numArray1[0] = 1 << n1 % 32 /*0x20*/;
        numArray2 = Number.ShiftLeft(y, n1);
        num4 += n1;
      }
      else
      {
        numArray1 = new int[1]{ 1 };
        int length = y.Length - index2;
        numArray2 = new int[length];
        Array.Copy((Array) y, index2, (Array) numArray2, 0, length);
      }
      a = new int[numArray1.Length];
label_11:
      if (num4 < num3 || Number.CompareNoLeadingZeroes(index1, x, index3, numArray2) >= 0)
      {
        Number.Subtract(index1, x, index3, numArray2);
        Number.AddMagnitudes(a, numArray1);
        while (x[index1] == 0)
        {
          if (++index1 == x.Length)
            return a;
        }
        num3 = 32 /*0x20*/ * (x.Length - index1 - 1) + Number.BitLen(x[index1]);
        if (num3 <= num2)
        {
          if (num3 < num2)
            return a;
          num1 = Number.CompareNoLeadingZeroes(index1, x, index2, y);
          if (num1 <= 0)
            goto label_30;
        }
      }
      int n2 = num4 - num3;
      if (n2 == 1 && (uint) (numArray2[index3] >>> 1) > (uint) x[index1])
        ++n2;
      if (n2 < 2)
      {
        Number.ShiftRightOneInPlace(index3, numArray2);
        --num4;
        Number.ShiftRightOneInPlace(start, numArray1);
      }
      else
      {
        Number.ShiftRightInPlace(index3, numArray2, n2);
        num4 -= n2;
        Number.ShiftRightInPlace(start, numArray1, n2);
      }
      while (numArray2[index3] == 0)
        ++index3;
      while (numArray1[start] == 0)
        ++start;
      goto label_11;
    }
    a = new int[1];
label_30:
    if (num1 == 0)
    {
      Number.AddMagnitudes(a, Number.One.m_magnitude);
      Array.Clear((Array) x, index1, x.Length - index1);
    }
    return a;
  }

  internal Number Divide(Number value)
  {
    if (value.m_sign == 0)
      throw new ArithmeticException("Invalid value. Division by zero error");
    if (this.m_sign == 0)
      return Number.Zero;
    if (value.QuickPow2Check())
    {
      Number number = this.Absolute().ShiftRight(value.Absolute().BitLength - 1);
      return value.m_sign != this.m_sign ? number.Negate() : number;
    }
    int[] x = (int[]) this.m_magnitude.Clone();
    return new Number(this.m_sign * value.m_sign, this.Divide(x, value.m_magnitude), true);
  }

  internal Number[] DivideAndRemainder(Number value)
  {
    if (value.m_sign == 0)
      throw new ArithmeticException("Invalid value. Division by zero error");
    Number[] numberArray = new Number[2];
    if (this.m_sign == 0)
    {
      numberArray[0] = Number.Zero;
      numberArray[1] = Number.Zero;
    }
    else if (value.QuickPow2Check())
    {
      int n = value.Absolute().BitLength - 1;
      Number number = this.Absolute().ShiftRight(n);
      int[] mag = this.LastNBits(n);
      numberArray[0] = value.m_sign == this.m_sign ? number : number.Negate();
      numberArray[1] = new Number(this.m_sign, mag, true);
    }
    else
    {
      int[] numArray = (int[]) this.m_magnitude.Clone();
      int[] mag = this.Divide(numArray, value.m_magnitude);
      numberArray[0] = new Number(this.m_sign * value.m_sign, mag, true);
      numberArray[1] = new Number(this.m_sign, numArray, true);
    }
    return numberArray;
  }

  public override bool Equals(object obj)
  {
    if (obj == this)
      return true;
    return obj is Number x && this.m_sign == x.m_sign && this.IsEqualMagnitude(x);
  }

  private bool IsEqualMagnitude(Number x)
  {
    int[] magnitude = x.m_magnitude;
    if (this.m_magnitude.Length != x.m_magnitude.Length)
      return false;
    for (int index = 0; index < this.m_magnitude.Length; ++index)
    {
      if (this.m_magnitude[index] != x.m_magnitude[index])
        return false;
    }
    return true;
  }

  public override int GetHashCode()
  {
    int length = this.m_magnitude.Length;
    if (this.m_magnitude.Length > 0)
    {
      length ^= this.m_magnitude[0];
      if (this.m_magnitude.Length > 1)
        length ^= this.m_magnitude[this.m_magnitude.Length - 1];
    }
    return this.m_sign >= 0 ? length : ~length;
  }

  private Number Inc()
  {
    if (this.m_sign == 0)
      return Number.One;
    return this.m_sign < 0 ? new Number(-1, Number.doSubBigLil(this.m_magnitude, Number.One.m_magnitude), true) : this.AddToMagnitude(Number.One.m_magnitude);
  }

  internal int IntValue
  {
    get
    {
      if (this.m_sign == 0)
        return 0;
      int num = this.m_magnitude[this.m_magnitude.Length - 1];
      return this.m_sign >= 0 ? num : -num;
    }
  }

  internal bool IsProbablePrime(int certainty)
  {
    if (certainty <= 0)
      return true;
    Number number = this.Absolute();
    if (!number.TestBit(0))
      return number.Equals((object) Number.Two);
    return !number.Equals((object) Number.One) && number.CheckProbablePrime(certainty, Number.m_rs);
  }

  private bool CheckProbablePrime(int certainty, SecureRandomAlgorithm random)
  {
    int num1 = Math.Min(this.BitLength - 1, Number.m_lists.Length);
    for (int index = 0; index < num1; ++index)
    {
      int num2 = this.Remainder(Number.m_products[index]);
      foreach (int num3 in Number.m_lists[index])
      {
        if (num2 % num3 == 0)
          return this.BitLength < 16 /*0x10*/ && this.IntValue == num3;
      }
    }
    return this.RabinMillerTest(certainty, random);
  }

  internal bool RabinMillerTest(int certainty, SecureRandomAlgorithm random)
  {
    Number number1 = this;
    int lowestSetBitMaskFirst = number1.GetLowestSetBitMaskFirst(-2);
    Number e = number1.ShiftRight(lowestSetBitMaskFirst);
    Number number2 = Number.One.ShiftLeft(32 /*0x20*/ * number1.m_magnitude.Length).Remainder(number1);
    Number x = number1.Subtract(number2);
    do
    {
      Number b1;
      do
      {
        b1 = new Number(number1.BitLength, random);
      }
      while (b1.m_sign == 0 || b1.CompareTo(number1) >= 0 || b1.IsEqualMagnitude(number2) || b1.IsEqualMagnitude(x));
      Number b2 = Number.ModPowMonty(b1, e, number1, false);
      if (!b2.Equals((object) number2))
      {
        int num = 0;
        while (!b2.Equals((object) x))
        {
          if (++num == lowestSetBitMaskFirst)
            return false;
          b2 = Number.ModPowMonty(b2, Number.Two, number1, false);
          if (b2.Equals((object) number2))
            return false;
        }
      }
      certainty -= 2;
    }
    while (certainty > 0);
    return true;
  }

  internal long LongValue
  {
    get
    {
      if (this.m_sign == 0)
        return 0;
      int length = this.m_magnitude.Length;
      long num = (long) this.m_magnitude[length - 1] & (long) uint.MaxValue;
      if (length > 1)
        num |= ((long) this.m_magnitude[length - 2] & (long) uint.MaxValue) << 32 /*0x20*/;
      return this.m_sign >= 0 ? num : -num;
    }
  }

  internal Number Mod(Number m)
  {
    Number number = this.Remainder(m);
    return number.m_sign < 0 ? number.Add(m) : number;
  }

  internal Number ModInverse(Number m)
  {
    Number u1Out;
    Number.ExtEuclid(this.Remainder(m), m, out u1Out);
    if (u1Out.m_sign < 0)
      u1Out = u1Out.Add(m);
    return u1Out;
  }

  private static int ModInverse32(int d)
  {
    int num1 = d + ((d + 1 & 4) << 1);
    int num2 = num1 * (2 - d * num1);
    int num3 = num2 * (2 - d * num2);
    return num3 * (2 - d * num3);
  }

  private static Number ExtEuclid(Number a, Number b, out Number u1Out)
  {
    Number number1 = Number.One;
    Number number2 = a;
    Number number3 = Number.Zero;
    Number[] numberArray;
    for (Number number4 = b; number4.m_sign > 0; number4 = numberArray[1])
    {
      numberArray = number2.DivideAndRemainder(number4);
      Number n = number3.Multiply(numberArray[0]);
      Number number5 = number1.Subtract(n);
      number1 = number3;
      number3 = number5;
      number2 = number4;
    }
    u1Out = number1;
    return number2;
  }

  internal Number ModPow(Number e, Number m)
  {
    if (m.m_sign < 1)
      throw new ArithmeticException("Invalid modulus. Negative value identified");
    if (m.Equals((object) Number.One))
      return Number.Zero;
    if (e.m_sign == 0)
      return Number.One;
    if (this.m_sign == 0)
      return Number.Zero;
    bool flag = e.m_sign < 0;
    if (flag)
      e = e.Negate();
    Number b = this.Mod(m);
    if (!e.Equals((object) Number.One))
      b = (m.m_magnitude[m.m_magnitude.Length - 1] & 1) != 0 ? Number.ModPowMonty(b, e, m, true) : Number.ModPowBarrett(b, e, m);
    if (flag)
      b = b.ModInverse(m);
    return b;
  }

  private static Number ModPowBarrett(Number b, Number e, Number m)
  {
    int length1 = m.m_magnitude.Length;
    Number mr = Number.One.ShiftLeft(length1 + 1 << 5);
    Number yu = Number.One.ShiftLeft(length1 << 6).Divide(m);
    int extraBits = 0;
    int bitLength = e.BitLength;
    while (bitLength > Number.m_eT[extraBits])
      ++extraBits;
    int length2 = 1 << extraBits;
    Number[] numberArray = new Number[length2];
    numberArray[0] = b;
    Number val = Number.ReduceBarrett(b.Square(), m, mr, yu);
    for (int index = 1; index < length2; ++index)
      numberArray[index] = Number.ReduceBarrett(numberArray[index - 1].Multiply(val), m, mr, yu);
    int[] windowList = Number.GetWindowList(e.m_magnitude, extraBits);
    int num1 = windowList[0];
    int num2 = num1 & (int) byte.MaxValue;
    int num3 = num1 >> 8;
    Number number;
    if (num2 == 1)
    {
      number = val;
      --num3;
    }
    else
      number = numberArray[num2 >> 1];
    int num4 = 1;
    while (true)
    {
      int[] numArray = windowList;
      int index1 = num4++;
      int num5;
      if ((num5 = numArray[index1]) != -1)
      {
        int index2 = num5 & (int) byte.MaxValue;
        int num6 = num3 + (int) Number.m_bitLengthTable[index2];
        for (int index3 = 0; index3 < num6; ++index3)
          number = Number.ReduceBarrett(number.Square(), m, mr, yu);
        number = Number.ReduceBarrett(number.Multiply(numberArray[index2 >> 1]), m, mr, yu);
        num3 = num5 >> 8;
      }
      else
        break;
    }
    for (int index = 0; index < num3; ++index)
      number = Number.ReduceBarrett(number.Square(), m, mr, yu);
    return number;
  }

  private static Number ReduceBarrett(Number x, Number m, Number mr, Number yu)
  {
    int bitLength1 = x.BitLength;
    int bitLength2 = m.BitLength;
    if (bitLength1 < bitLength2)
      return x;
    if (bitLength1 - bitLength2 > 1)
    {
      int length = m.m_magnitude.Length;
      Number number = x.DivideWords(length - 1).Multiply(yu).DivideWords(length + 1);
      x = x.RemainderWords(length + 1).Subtract(number.Multiply(m).RemainderWords(length + 1));
      if (x.m_sign < 0)
        x = x.Add(mr);
    }
    while (x.CompareTo(m) >= 0)
      x = x.Subtract(m);
    return x;
  }

  private static Number ModPowMonty(Number b, Number e, Number m, bool convert)
  {
    int length1 = m.m_magnitude.Length;
    int n = 32 /*0x20*/ * length1;
    bool smallMontyModulus = m.BitLength + 2 <= n;
    uint mquote = (uint) m.GetMQuote();
    if (convert)
      b = b.ShiftLeft(n).Remainder(m);
    int[] a = new int[length1 + 1];
    int[] data = b.m_magnitude;
    if (data.Length < length1)
    {
      int[] numArray = new int[length1];
      data.CopyTo((Array) numArray, length1 - data.Length);
      data = numArray;
    }
    int extraBits = 0;
    if (e.m_magnitude.Length > 1 || e.BitCount > 2)
    {
      int bitLength = e.BitLength;
      while (bitLength > Number.m_eT[extraBits])
        ++extraBits;
    }
    int length2 = 1 << extraBits;
    int[][] numArray1 = new int[length2][];
    numArray1[0] = data;
    int[] numArray2 = Asn1Constants.Clone(data);
    Number.SquareMonty(a, numArray2, m.m_magnitude, mquote, smallMontyModulus);
    for (int index = 1; index < length2; ++index)
    {
      numArray1[index] = Asn1Constants.Clone(numArray1[index - 1]);
      Number.MultiplyMonty(a, numArray1[index], numArray2, m.m_magnitude, mquote, smallMontyModulus);
    }
    int[] windowList = Number.GetWindowList(e.m_magnitude, extraBits);
    int num1 = windowList[0];
    int num2 = num1 & (int) byte.MaxValue;
    int num3 = num1 >> 8;
    int[] numArray3;
    if (num2 == 1)
    {
      numArray3 = numArray2;
      --num3;
    }
    else
      numArray3 = Asn1Constants.Clone(numArray1[num2 >> 1]);
    int num4 = 1;
    while (true)
    {
      int[] numArray4 = windowList;
      int index1 = num4++;
      int num5;
      if ((num5 = numArray4[index1]) != -1)
      {
        int index2 = num5 & (int) byte.MaxValue;
        int num6 = num3 + (int) Number.m_bitLengthTable[index2];
        for (int index3 = 0; index3 < num6; ++index3)
          Number.SquareMonty(a, numArray3, m.m_magnitude, mquote, smallMontyModulus);
        Number.MultiplyMonty(a, numArray3, numArray1[index2 >> 1], m.m_magnitude, mquote, smallMontyModulus);
        num3 = num5 >> 8;
      }
      else
        break;
    }
    for (int index = 0; index < num3; ++index)
      Number.SquareMonty(a, numArray3, m.m_magnitude, mquote, smallMontyModulus);
    if (convert)
      Number.MontgomeryReduce(numArray3, m.m_magnitude, mquote);
    else if (smallMontyModulus && Number.CompareTo(0, numArray3, 0, m.m_magnitude) >= 0)
      Number.Subtract(0, numArray3, 0, m.m_magnitude);
    return new Number(1, numArray3, true);
  }

  private static int[] GetWindowList(int[] mag, int extraBits)
  {
    int w = mag[0];
    int num1 = Number.BitLen(w);
    int[] windowList = new int[((mag.Length - 1 << 5) + num1) / (1 + extraBits) + 2];
    int num2 = 0;
    int num3 = 33 - num1;
    int num4 = w << num3;
    int mult = 1;
    int num5 = 1 << extraBits;
    int zeroes = 0;
    int index1 = 0;
    while (true)
    {
      for (; num3 < 32 /*0x20*/; ++num3)
      {
        if (mult < num5)
          mult = mult << 1 | num4 >>> 31 /*0x1F*/;
        else if (num4 < 0)
        {
          windowList[num2++] = Number.CreateWindowEntry(mult, zeroes);
          mult = 1;
          zeroes = 0;
        }
        else
          ++zeroes;
        num4 <<= 1;
      }
      if (++index1 != mag.Length)
      {
        num4 = mag[index1];
        num3 = 0;
      }
      else
        break;
    }
    int[] numArray = windowList;
    int index2 = num2;
    int index3 = index2 + 1;
    int windowEntry = Number.CreateWindowEntry(mult, zeroes);
    numArray[index2] = windowEntry;
    windowList[index3] = -1;
    return windowList;
  }

  private static int CreateWindowEntry(int mult, int zeroes)
  {
    while ((mult & 1) == 0)
    {
      mult >>= 1;
      ++zeroes;
    }
    return mult | zeroes << 8;
  }

  private static int[] Square(int[] w, int[] x)
  {
    int index1 = w.Length - 1;
    for (int index2 = x.Length - 1; index2 > 0; --index2)
    {
      ulong num1 = (ulong) (uint) x[index2];
      ulong num2 = num1 * num1 + (ulong) (uint) w[index1];
      w[index1] = (int) num2;
      ulong num3 = num2 >> 32 /*0x20*/;
      for (int index3 = index2 - 1; index3 >= 0; --index3)
      {
        ulong num4 = num1 * (ulong) (uint) x[index3];
        ulong num5 = num3 + (((ulong) (uint) w[--index1] & (ulong) uint.MaxValue) + (ulong) ((uint) num4 << 1));
        w[index1] = (int) num5;
        num3 = (num5 >> 32 /*0x20*/) + (num4 >> 31 /*0x1F*/);
      }
      int index4;
      ulong num6 = num3 + (ulong) (uint) w[index4 = index1 - 1];
      w[index4] = (int) num6;
      int index5;
      if ((index5 = index4 - 1) >= 0)
        w[index5] = (int) (num6 >> 32 /*0x20*/);
      index1 = index5 + index2;
    }
    ulong num7 = (ulong) (uint) x[0];
    ulong num8 = num7 * num7 + (ulong) (uint) w[index1];
    w[index1] = (int) num8;
    int index6;
    if ((index6 = index1 - 1) >= 0)
      w[index6] += (int) (num8 >> 32 /*0x20*/);
    return w;
  }

  private static int[] Multiply(int[] x, int[] y, int[] z)
  {
    int length = z.Length;
    if (length < 1)
      return x;
    int index1 = x.Length - y.Length;
    do
    {
      long num1 = (long) z[--length] & (long) uint.MaxValue;
      long num2 = 0;
      if (num1 != 0L)
      {
        for (int index2 = y.Length - 1; index2 >= 0; --index2)
        {
          long num3 = num2 + (num1 * ((long) y[index2] & (long) uint.MaxValue) + ((long) x[index1 + index2] & (long) uint.MaxValue));
          x[index1 + index2] = (int) num3;
          num2 = num3 >>> 32 /*0x20*/;
        }
      }
      --index1;
      if (index1 >= 0)
        x[index1] = (int) num2;
    }
    while (length > 0);
    return x;
  }

  private int GetMQuote()
  {
    return this.m_quote != 0 ? this.m_quote : (this.m_quote = Number.ModInverse32(-this.m_magnitude[this.m_magnitude.Length - 1]));
  }

  private static void MontgomeryReduce(int[] x, int[] m, uint mDash)
  {
    int length = m.Length;
    for (int index1 = length - 1; index1 >= 0; --index1)
    {
      uint num1 = (uint) x[length - 1];
      ulong num2 = (ulong) (num1 * mDash);
      ulong num3 = num2 * (ulong) (uint) m[length - 1] + (ulong) num1 >> 32 /*0x20*/;
      for (int index2 = length - 2; index2 >= 0; --index2)
      {
        ulong num4 = num3 + (num2 * (ulong) (uint) m[index2] + (ulong) (uint) x[index2]);
        x[index2 + 1] = (int) num4;
        num3 = num4 >> 32 /*0x20*/;
      }
      x[0] = (int) num3;
    }
    if (Number.CompareTo(0, x, 0, m) < 0)
      return;
    Number.Subtract(0, x, 0, m);
  }

  private static void MultiplyMonty(
    int[] a,
    int[] x,
    int[] y,
    int[] m,
    uint mDash,
    bool smallMontyModulus)
  {
    int length = m.Length;
    if (length == 1)
    {
      x[0] = (int) Number.MultiplyMontyNIsOne((uint) x[0], (uint) y[0], (uint) m[0], mDash);
    }
    else
    {
      uint num1 = (uint) y[length - 1];
      ulong num2 = (ulong) (uint) x[length - 1];
      ulong num3 = num2 * (ulong) num1;
      ulong num4 = (ulong) ((uint) num3 * mDash);
      ulong num5 = num4 * (ulong) (uint) m[length - 1];
      ulong num6 = (num3 + (ulong) (uint) num5 >> 32 /*0x20*/) + (num5 >> 32 /*0x20*/);
      for (int index = length - 2; index >= 0; --index)
      {
        ulong num7 = num2 * (ulong) (uint) y[index];
        ulong num8 = num4 * (ulong) (uint) m[index];
        ulong num9 = num6 + ((num7 & (ulong) uint.MaxValue) + (ulong) (uint) num8);
        a[index + 2] = (int) num9;
        num6 = (num9 >> 32 /*0x20*/) + (num7 >> 32 /*0x20*/) + (num8 >> 32 /*0x20*/);
      }
      a[1] = (int) num6;
      a[0] = (int) (num6 >> 32 /*0x20*/);
      for (int index1 = length - 2; index1 >= 0; --index1)
      {
        uint num10 = (uint) a[length];
        ulong num11 = (ulong) (uint) x[index1];
        ulong num12 = num11 * (ulong) num1;
        ulong num13 = (num12 & (ulong) uint.MaxValue) + (ulong) num10;
        ulong num14 = (ulong) ((uint) num13 * mDash);
        ulong num15 = num14 * (ulong) (uint) m[length - 1];
        ulong num16 = (num13 + (ulong) (uint) num15 >> 32 /*0x20*/) + (num12 >> 32 /*0x20*/) + (num15 >> 32 /*0x20*/);
        for (int index2 = length - 2; index2 >= 0; --index2)
        {
          ulong num17 = num11 * (ulong) (uint) y[index2];
          ulong num18 = num14 * (ulong) (uint) m[index2];
          ulong num19 = num16 + ((num17 & (ulong) uint.MaxValue) + (ulong) (uint) num18 + (ulong) (uint) a[index2 + 1]);
          a[index2 + 2] = (int) num19;
          num16 = (num19 >> 32 /*0x20*/) + (num17 >> 32 /*0x20*/) + (num18 >> 32 /*0x20*/);
        }
        ulong num20 = num16 + (ulong) (uint) a[0];
        a[1] = (int) num20;
        a[0] = (int) (num20 >> 32 /*0x20*/);
      }
      if (!smallMontyModulus && Number.CompareTo(0, a, 0, m) >= 0)
        Number.Subtract(0, a, 0, m);
      Array.Copy((Array) a, 1, (Array) x, 0, length);
    }
  }

  private static void SquareMonty(int[] a, int[] x, int[] m, uint mDash, bool smallMontyModulus)
  {
    int length = m.Length;
    if (length == 1)
    {
      uint num = (uint) x[0];
      x[0] = (int) Number.MultiplyMontyNIsOne(num, num, (uint) m[0], mDash);
    }
    else
    {
      ulong num1 = (ulong) (uint) x[length - 1];
      ulong num2 = num1 * num1;
      ulong num3 = (ulong) ((uint) num2 * mDash);
      ulong num4 = num3 * (ulong) (uint) m[length - 1];
      ulong num5 = (num2 + (ulong) (uint) num4 >> 32 /*0x20*/) + (num4 >> 32 /*0x20*/);
      for (int index = length - 2; index >= 0; --index)
      {
        ulong num6 = num1 * (ulong) (uint) x[index];
        ulong num7 = num3 * (ulong) (uint) m[index];
        ulong num8 = num5 + ((num7 & (ulong) uint.MaxValue) + (ulong) ((uint) num6 << 1));
        a[index + 2] = (int) num8;
        num5 = (num8 >> 32 /*0x20*/) + (num6 >> 31 /*0x1F*/) + (num7 >> 32 /*0x20*/);
      }
      a[1] = (int) num5;
      a[0] = (int) (num5 >> 32 /*0x20*/);
      for (int index1 = length - 2; index1 >= 0; --index1)
      {
        uint num9 = (uint) a[length];
        ulong num10 = (ulong) (num9 * mDash);
        ulong num11 = num10 * (ulong) (uint) m[length - 1] + (ulong) num9 >> 32 /*0x20*/;
        for (int index2 = length - 2; index2 > index1; --index2)
        {
          ulong num12 = num11 + (num10 * (ulong) (uint) m[index2] + (ulong) (uint) a[index2 + 1]);
          a[index2 + 2] = (int) num12;
          num11 = num12 >> 32 /*0x20*/;
        }
        ulong num13 = (ulong) (uint) x[index1];
        ulong num14 = num13 * num13;
        ulong num15 = num10 * (ulong) (uint) m[index1];
        ulong num16 = num11 + ((num14 & (ulong) uint.MaxValue) + (ulong) (uint) num15 + (ulong) (uint) a[index1 + 1]);
        a[index1 + 2] = (int) num16;
        ulong num17 = (num16 >> 32 /*0x20*/) + (num14 >> 32 /*0x20*/) + (num15 >> 32 /*0x20*/);
        for (int index3 = index1 - 1; index3 >= 0; --index3)
        {
          ulong num18 = num13 * (ulong) (uint) x[index3];
          ulong num19 = num10 * (ulong) (uint) m[index3];
          ulong num20 = num17 + ((num19 & (ulong) uint.MaxValue) + (ulong) ((uint) num18 << 1) + (ulong) (uint) a[index3 + 1]);
          a[index3 + 2] = (int) num20;
          num17 = (num20 >> 32 /*0x20*/) + (num18 >> 31 /*0x1F*/) + (num19 >> 32 /*0x20*/);
        }
        ulong num21 = num17 + (ulong) (uint) a[0];
        a[1] = (int) num21;
        a[0] = (int) (num21 >> 32 /*0x20*/);
      }
      if (!smallMontyModulus && Number.CompareTo(0, a, 0, m) >= 0)
        Number.Subtract(0, a, 0, m);
      Array.Copy((Array) a, 1, (Array) x, 0, length);
    }
  }

  private static uint MultiplyMontyNIsOne(uint x, uint y, uint m, uint mDash)
  {
    ulong num1 = (ulong) x * (ulong) y;
    uint num2 = (uint) num1 * mDash;
    ulong num3 = (ulong) m;
    ulong num4 = num3 * (ulong) num2;
    ulong num5 = (num1 + (ulong) (uint) num4 >> 32 /*0x20*/) + (num4 >> 32 /*0x20*/);
    if (num5 > num3)
      num5 -= num3;
    return (uint) num5;
  }

  internal Number Multiply(Number val)
  {
    if (val == this)
      return this.Square();
    if ((this.m_sign & val.m_sign) == 0)
      return Number.Zero;
    if (val.QuickPow2Check())
    {
      Number number = this.ShiftLeft(val.Absolute().BitLength - 1);
      return val.m_sign <= 0 ? number.Negate() : number;
    }
    if (this.QuickPow2Check())
    {
      Number number = val.ShiftLeft(this.Absolute().BitLength - 1);
      return this.m_sign <= 0 ? number.Negate() : number;
    }
    int[] numArray = new int[this.m_magnitude.Length + val.m_magnitude.Length];
    Number.Multiply(numArray, this.m_magnitude, val.m_magnitude);
    return new Number(this.m_sign ^ val.m_sign ^ 1, numArray, true);
  }

  internal Number Square()
  {
    if (this.m_sign == 0)
      return Number.Zero;
    if (this.QuickPow2Check())
      return this.ShiftLeft(this.Absolute().BitLength - 1);
    int length = this.m_magnitude.Length << 1;
    if (this.m_magnitude[0] >>> 16 /*0x10*/ == 0)
      --length;
    int[] numArray = new int[length];
    Number.Square(numArray, this.m_magnitude);
    return new Number(1, numArray, false);
  }

  internal Number Negate()
  {
    return this.m_sign == 0 ? this : new Number(-this.m_sign, this.m_magnitude, false);
  }

  internal Number Not() => this.Inc().Negate();

  internal Number Pow(int exp)
  {
    if (exp <= 0)
    {
      if (exp < 0)
        throw new ArithmeticException("Invalid exponent. Negative value identified");
      return Number.One;
    }
    if (this.m_sign == 0)
      return this;
    if (this.QuickPow2Check())
    {
      long n = (long) exp * (long) (this.BitLength - 1);
      return n <= (long) int.MaxValue ? Number.One.ShiftLeft((int) n) : throw new ArithmeticException("Result too large");
    }
    Number number = Number.One;
    Number val = this;
    while (true)
    {
      if ((exp & 1) == 1)
        number = number.Multiply(val);
      exp >>= 1;
      if (exp != 0)
        val = val.Multiply(val);
      else
        break;
    }
    return number;
  }

  private int Remainder(int m)
  {
    long num1 = 0;
    for (int index = 0; index < this.m_magnitude.Length; ++index)
    {
      long num2 = (long) (uint) this.m_magnitude[index];
      num1 = (num1 << 32 /*0x20*/ | num2) % (long) m;
    }
    return (int) num1;
  }

  private static int[] Remainder(int[] x, int[] y)
  {
    int index1 = 0;
    while (index1 < x.Length && x[index1] == 0)
      ++index1;
    int index2 = 0;
    while (index2 < y.Length && y[index2] == 0)
      ++index2;
    int num1 = Number.CompareNoLeadingZeroes(index1, x, index2, y);
    if (num1 > 0)
    {
      int num2 = Number.CalcBitLength(1, index2, y);
      int num3 = Number.CalcBitLength(1, index1, x);
      int n1 = num3 - num2;
      int index3 = 0;
      int num4 = num2;
      int[] numArray;
      if (n1 > 0)
      {
        numArray = Number.ShiftLeft(y, n1);
        num4 += n1;
      }
      else
      {
        int length = y.Length - index2;
        numArray = new int[length];
        Array.Copy((Array) y, index2, (Array) numArray, 0, length);
      }
label_10:
      if (num4 < num3 || Number.CompareNoLeadingZeroes(index1, x, index3, numArray) >= 0)
      {
        Number.Subtract(index1, x, index3, numArray);
        while (x[index1] == 0)
        {
          if (++index1 == x.Length)
            return x;
        }
        num3 = 32 /*0x20*/ * (x.Length - index1 - 1) + Number.BitLen(x[index1]);
        if (num3 <= num2)
        {
          if (num3 < num2)
            return x;
          num1 = Number.CompareNoLeadingZeroes(index1, x, index2, y);
          if (num1 <= 0)
            goto label_26;
        }
      }
      int n2 = num4 - num3;
      if (n2 == 1 && (uint) (numArray[index3] >>> 1) > (uint) x[index1])
        ++n2;
      if (n2 < 2)
      {
        Number.ShiftRightOneInPlace(index3, numArray);
        --num4;
      }
      else
      {
        Number.ShiftRightInPlace(index3, numArray, n2);
        num4 -= n2;
      }
      while (numArray[index3] == 0)
        ++index3;
      goto label_10;
    }
label_26:
    if (num1 == 0)
      Array.Clear((Array) x, index1, x.Length - index1);
    return x;
  }

  internal Number Remainder(Number n)
  {
    if (n.m_sign == 0)
      throw new ArithmeticException("Invalid entry. Division by zero error");
    if (this.m_sign == 0)
      return Number.Zero;
    if (n.m_magnitude.Length == 1)
    {
      int m = n.m_magnitude[0];
      if (m > 0)
      {
        if (m == 1)
          return Number.Zero;
        int num = this.Remainder(m);
        if (num == 0)
          return Number.Zero;
        return new Number(this.m_sign, new int[1]{ num }, false);
      }
    }
    return Number.CompareNoLeadingZeroes(0, this.m_magnitude, 0, n.m_magnitude) < 0 ? this : new Number(this.m_sign, !n.QuickPow2Check() ? Number.Remainder((int[]) this.m_magnitude.Clone(), n.m_magnitude) : this.LastNBits(n.Absolute().BitLength - 1), true);
  }

  private int[] LastNBits(int n)
  {
    if (n < 1)
      return Number.m_zeroMagnitude;
    int length = Math.Min((n + 32 /*0x20*/ - 1) / 32 /*0x20*/, this.m_magnitude.Length);
    int[] destinationArray = new int[length];
    Array.Copy((Array) this.m_magnitude, this.m_magnitude.Length - length, (Array) destinationArray, 0, length);
    int num = (length << 5) - n;
    if (num > 0)
      destinationArray[0] &= (int) (uint.MaxValue >> num);
    return destinationArray;
  }

  private Number DivideWords(int w)
  {
    int length = this.m_magnitude.Length;
    if (w >= length)
      return Number.Zero;
    int[] numArray = new int[length - w];
    Array.Copy((Array) this.m_magnitude, 0, (Array) numArray, 0, length - w);
    return new Number(this.m_sign, numArray, false);
  }

  private Number RemainderWords(int w)
  {
    int length = this.m_magnitude.Length;
    if (w >= length)
      return this;
    int[] numArray = new int[w];
    Array.Copy((Array) this.m_magnitude, length - w, (Array) numArray, 0, w);
    return new Number(this.m_sign, numArray, false);
  }

  private static int[] ShiftLeft(int[] mag, int n)
  {
    int num1 = n >>> 5;
    int num2 = n & 31 /*0x1F*/;
    int length = mag.Length;
    int[] numArray;
    if (num2 == 0)
    {
      numArray = new int[length + num1];
      mag.CopyTo((Array) numArray, 0);
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

  internal Number ShiftLeft(int n)
  {
    if (this.m_sign == 0 || this.m_magnitude.Length == 0)
      return Number.Zero;
    if (n == 0)
      return this;
    if (n < 0)
      return this.ShiftRight(-n);
    Number number = new Number(this.m_sign, Number.ShiftLeft(this.m_magnitude, n), true);
    if (this.m_nBits != -1)
      number.m_nBits = this.m_sign > 0 ? this.m_nBits : this.m_nBits + n;
    if (this.m_nBitLength != -1)
      number.m_nBitLength = this.m_nBitLength + n;
    return number;
  }

  private static void ShiftRightInPlace(int start, int[] mag, int n)
  {
    int index1 = (n >>> 5) + start;
    int num1 = n & 31 /*0x1F*/;
    int index2 = mag.Length - 1;
    if (index1 != start)
    {
      int num2 = index1 - start;
      for (int index3 = index2; index3 >= index1; --index3)
        mag[index3] = mag[index3 - num2];
      for (int index4 = index1 - 1; index4 >= start; --index4)
        mag[index4] = 0;
    }
    if (num1 == 0)
      return;
    int num3 = 32 /*0x20*/ - num1;
    int num4 = mag[index2];
    for (int index5 = index2; index5 > index1; --index5)
    {
      int num5 = mag[index5 - 1];
      mag[index5] = num4 >>> num1 | num5 << num3;
      num4 = num5;
    }
    mag[index1] = mag[index1] >>> num1;
  }

  private static void ShiftRightOneInPlace(int start, int[] mag)
  {
    int length = mag.Length;
    int num1 = mag[length - 1];
    while (--length > start)
    {
      int num2 = mag[length - 1];
      mag[length] = num1 >>> 1 | num2 << 31 /*0x1F*/;
      num1 = num2;
    }
    mag[start] = mag[start] >>> 1;
  }

  internal Number ShiftRight(int n)
  {
    if (n == 0)
      return this;
    if (n < 0)
      return this.ShiftLeft(-n);
    if (n >= this.BitLength)
      return this.m_sign >= 0 ? Number.Zero : Number.One.Negate();
    int length = this.BitLength - n + 31 /*0x1F*/ >> 5;
    int[] numArray = new int[length];
    int num1 = n >> 5;
    int num2 = n & 31 /*0x1F*/;
    if (num2 == 0)
    {
      Array.Copy((Array) this.m_magnitude, 0, (Array) numArray, 0, numArray.Length);
    }
    else
    {
      int num3 = 32 /*0x20*/ - num2;
      int index1 = this.m_magnitude.Length - 1 - num1;
      for (int index2 = length - 1; index2 >= 0; --index2)
      {
        numArray[index2] = this.m_magnitude[index1--] >>> num2;
        if (index1 >= 0)
          numArray[index2] |= this.m_magnitude[index1] << num3;
      }
    }
    return new Number(this.m_sign, numArray, false);
  }

  internal int SignValue => this.m_sign;

  private static int[] Subtract(int xStart, int[] x, int yStart, int[] y)
  {
    int length1 = x.Length;
    int length2 = y.Length;
    int num1 = 0;
    do
    {
      long num2 = ((long) x[--length1] & (long) uint.MaxValue) - ((long) y[--length2] & (long) uint.MaxValue) + (long) num1;
      x[length1] = (int) num2;
      num1 = (int) (num2 >> 63 /*0x3F*/);
    }
    while (length2 > yStart);
    if (num1 != 0)
    {
      while (--x[--length1] == -1)
        ;
    }
    return x;
  }

  internal Number Subtract(Number n)
  {
    if (n.m_sign == 0)
      return this;
    if (this.m_sign == 0)
      return n.Negate();
    if (this.m_sign != n.m_sign)
      return this.Add(n.Negate());
    int num = Number.CompareNoLeadingZeroes(0, this.m_magnitude, 0, n.m_magnitude);
    if (num == 0)
      return Number.Zero;
    Number number1;
    Number number2;
    if (num < 0)
    {
      number1 = n;
      number2 = this;
    }
    else
    {
      number1 = this;
      number2 = n;
    }
    return new Number(this.m_sign * num, Number.doSubBigLil(number1.m_magnitude, number2.m_magnitude), true);
  }

  private static int[] doSubBigLil(int[] bigMag, int[] lilMag)
  {
    return Number.Subtract(0, (int[]) bigMag.Clone(), 0, lilMag);
  }

  internal byte[] ToByteArray() => this.ToByteArray(false);

  internal byte[] ToByteArrayUnsigned() => this.ToByteArray(true);

  private byte[] ToByteArray(bool unsigned)
  {
    if (this.m_sign == 0)
      return !unsigned ? new byte[1] : Number.m_zeroEncoding;
    byte[] byteArray = new byte[Number.GetByteLength(!unsigned || this.m_sign <= 0 ? this.BitLength + 1 : this.BitLength)];
    int length = this.m_magnitude.Length;
    int num1 = byteArray.Length;
    int num2;
    if (this.m_sign > 0)
    {
      while (length > 1)
      {
        uint num3 = (uint) this.m_magnitude[--length];
        int num4;
        byteArray[num4 = num1 - 1] = (byte) num3;
        int num5;
        byteArray[num5 = num4 - 1] = (byte) (num3 >> 8);
        int num6;
        byteArray[num6 = num5 - 1] = (byte) (num3 >> 16 /*0x10*/);
        byteArray[num1 = num6 - 1] = (byte) (num3 >> 24);
      }
      uint num7;
      for (num7 = (uint) this.m_magnitude[0]; num7 > (uint) byte.MaxValue; num7 >>= 8)
        byteArray[--num1] = (byte) num7;
      byteArray[num2 = num1 - 1] = (byte) num7;
    }
    else
    {
      bool flag = true;
      while (length > 1)
      {
        uint num8 = (uint) ~this.m_magnitude[--length];
        if (flag)
          flag = ++num8 == 0U;
        int num9;
        byteArray[num9 = num1 - 1] = (byte) num8;
        int num10;
        byteArray[num10 = num9 - 1] = (byte) (num8 >> 8);
        int num11;
        byteArray[num11 = num10 - 1] = (byte) (num8 >> 16 /*0x10*/);
        byteArray[num1 = num11 - 1] = (byte) (num8 >> 24);
      }
      uint num12 = (uint) this.m_magnitude[0];
      if (flag)
        --num12;
      for (; num12 > (uint) byte.MaxValue; num12 >>= 8)
        byteArray[--num1] = (byte) ~num12;
      int num13;
      byteArray[num13 = num1 - 1] = (byte) ~num12;
      if (num13 > 0)
        byteArray[num2 = num13 - 1] = byte.MaxValue;
    }
    return byteArray;
  }

  public override string ToString() => this.ToString(10);

  public string ToString(int radix)
  {
    switch (radix)
    {
      case 2:
      case 8:
      case 10:
      case 16 /*0x10*/:
        if (this.m_magnitude == null)
          return "null";
        if (this.m_sign == 0)
          return "0";
        int index1 = 0;
        while (index1 < this.m_magnitude.Length && this.m_magnitude[index1] == 0)
          ++index1;
        if (index1 == this.m_magnitude.Length)
          return "0";
        StringBuilder sb = new StringBuilder();
        if (this.m_sign == -1)
          sb.Append('-');
        switch (radix)
        {
          case 2:
            int index2 = index1;
            sb.Append(Convert.ToString(this.m_magnitude[index2], 2));
            while (++index2 < this.m_magnitude.Length)
              Number.AppendZeroExtendedString(sb, Convert.ToString(this.m_magnitude[index2], 2), 32 /*0x20*/);
            break;
          case 8:
            int num1 = 1073741823 /*0x3FFFFFFF*/;
            Number number1 = this.Absolute();
            int bitLength = number1.BitLength;
            IList list1 = (IList) new ArrayList();
            for (; bitLength > 30; bitLength -= 30)
            {
              list1.Add((object) Convert.ToString(number1.IntValue & num1, 8));
              number1 = number1.ShiftRight(30);
            }
            sb.Append(Convert.ToString(number1.IntValue, 8));
            for (int index3 = list1.Count - 1; index3 >= 0; --index3)
              Number.AppendZeroExtendedString(sb, (string) list1[index3], 10);
            break;
          case 10:
            Number number2 = this.Absolute();
            if (number2.BitLength < 64 /*0x40*/)
            {
              sb.Append(Convert.ToString(number2.LongValue, radix));
              break;
            }
            long num2 = long.MaxValue / (long) radix;
            long num3 = (long) radix;
            int minLength = 1;
            while (num3 <= num2)
            {
              num3 *= (long) radix;
              ++minLength;
            }
            Number number3 = Number.ValueOf(num3);
            IList list2 = (IList) new ArrayList();
            Number[] numberArray;
            for (; number2.CompareTo(number3) >= 0; number2 = numberArray[0])
            {
              numberArray = number2.DivideAndRemainder(number3);
              list2.Add((object) Convert.ToString(numberArray[1].LongValue, radix));
            }
            sb.Append(Convert.ToString(number2.LongValue, radix));
            for (int index4 = list2.Count - 1; index4 >= 0; --index4)
              Number.AppendZeroExtendedString(sb, (string) list2[index4], minLength);
            break;
          case 16 /*0x10*/:
            int index5 = index1;
            sb.Append(Convert.ToString(this.m_magnitude[index5], 16 /*0x10*/));
            while (++index5 < this.m_magnitude.Length)
              Number.AppendZeroExtendedString(sb, Convert.ToString(this.m_magnitude[index5], 16 /*0x10*/), 8);
            break;
        }
        return sb.ToString();
      default:
        throw new FormatException("Invalid entry. Only bases 2, 8, 10, 16 are allowed");
    }
  }

  private static void AppendZeroExtendedString(StringBuilder sb, string s, int minLength)
  {
    for (int length = s.Length; length < minLength; ++length)
      sb.Append('0');
    sb.Append(s);
  }

  private static Number CreateUValueOf(ulong value)
  {
    int num1 = (int) (value >> 32 /*0x20*/);
    int num2 = (int) value;
    if (num1 != 0)
      return new Number(1, new int[2]{ num1, num2 }, false);
    if (num2 == 0)
      return Number.Zero;
    Number uvalueOf = new Number(1, new int[1]{ num2 }, false);
    if ((num2 & -num2) == num2)
      uvalueOf.m_nBits = 1;
    return uvalueOf;
  }

  private static Number CreateValueOf(long value)
  {
    if (value >= 0L)
      return Number.CreateUValueOf((ulong) value);
    return value == long.MinValue ? Number.CreateValueOf(~value).Not() : Number.CreateValueOf(-value).Negate();
  }

  public static Number ValueOf(long value)
  {
    return value >= 0L && value < (long) Number.m_smallConstants.Length ? Number.m_smallConstants[value] : Number.CreateValueOf(value);
  }

  private int GetLowestSetBitMaskFirst(int firstWordMask)
  {
    int length = this.m_magnitude.Length;
    int lowestSetBitMaskFirst = 0;
    int num1;
    uint num2 = (uint) (this.m_magnitude[num1 = length - 1] & firstWordMask);
    while (num2 == 0U)
    {
      num2 = (uint) this.m_magnitude[--num1];
      lowestSetBitMaskFirst += 32 /*0x20*/;
    }
    while (((int) num2 & (int) byte.MaxValue) == 0)
    {
      num2 >>= 8;
      lowestSetBitMaskFirst += 8;
    }
    while (((int) num2 & 1) == 0)
    {
      num2 >>= 1;
      ++lowestSetBitMaskFirst;
    }
    return lowestSetBitMaskFirst;
  }

  internal bool TestBit(int value)
  {
    if (value < 0)
      throw new ArithmeticException("Invalid entry. Bit position can not be less than zero");
    if (this.m_sign < 0)
      return !this.Not().TestBit(value);
    int num = value / 32 /*0x20*/;
    return num < this.m_magnitude.Length && (this.m_magnitude[this.m_magnitude.Length - 1 - num] >> value % 32 /*0x20*/ & 1) > 0;
  }

  internal Number Or(Number value)
  {
    if (this.m_sign == 0)
      return value;
    if (value.m_sign == 0)
      return this;
    int[] numArray1 = this.m_sign > 0 ? this.m_magnitude : this.Add(Number.One).m_magnitude;
    int[] numArray2 = value.m_sign > 0 ? value.m_magnitude : value.Add(Number.One).m_magnitude;
    bool flag = this.m_sign < 0 || value.m_sign < 0;
    int[] mag = new int[Math.Max(numArray1.Length, numArray2.Length)];
    int num1 = mag.Length - numArray1.Length;
    int num2 = mag.Length - numArray2.Length;
    for (int index = 0; index < mag.Length; ++index)
    {
      int num3 = index >= num1 ? numArray1[index - num1] : 0;
      int num4 = index >= num2 ? numArray2[index - num2] : 0;
      if (this.m_sign < 0)
        num3 = ~num3;
      if (value.m_sign < 0)
        num4 = ~num4;
      mag[index] = num3 | num4;
      if (flag)
        mag[index] = ~mag[index];
    }
    Number number = new Number(1, mag, true);
    if (flag)
      number = number.Not();
    return number;
  }

  internal Number SetBit(int value)
  {
    if (value < 0)
      throw new ArithmeticException("Invalid entry. Bit position can not be less than zero");
    if (this.TestBit(value))
      return this;
    return this.m_sign > 0 && value < this.BitLength - 1 ? this.FlipExistingBit(value) : this.Or(Number.One.ShiftLeft(value));
  }

  private Number FlipExistingBit(int value)
  {
    int[] mag = (int[]) this.m_magnitude.Clone();
    mag[mag.Length - 1 - (value >> 5)] ^= 1 << value;
    return new Number(this.m_sign, mag, false);
  }

  internal int GetLowestSetBit() => this.m_sign == 0 ? -1 : this.GetLowestSetBitMaskFirst(-1);
}
