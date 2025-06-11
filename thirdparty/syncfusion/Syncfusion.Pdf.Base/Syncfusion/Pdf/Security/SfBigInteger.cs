// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SfBigInteger
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SfBigInteger
{
  private const int maxLength = 70;
  private uint[] data;
  private int dataLength;

  internal SfBigInteger()
  {
    this.data = new uint[70];
    this.dataLength = 1;
  }

  internal SfBigInteger(long value)
  {
    this.data = new uint[70];
    long num = value;
    for (this.dataLength = 0; value != 0L && this.dataLength < 70; ++this.dataLength)
    {
      this.data[this.dataLength] = (uint) ((ulong) value & (ulong) uint.MaxValue);
      value >>= 32 /*0x20*/;
    }
    if (num > 0L)
    {
      if (value != 0L || ((int) this.data[69] & int.MinValue) != 0)
        throw new ArithmeticException("Positive overflow in constructor.");
    }
    else if (num < 0L && (value != -1L || ((int) this.data[this.dataLength - 1] & int.MinValue) == 0))
      throw new ArithmeticException("Negative underflow in constructor.");
    if (this.dataLength != 0)
      return;
    this.dataLength = 1;
  }

  private SfBigInteger(SfBigInteger bi)
  {
    this.data = new uint[70];
    this.dataLength = bi.dataLength;
    for (int index = 0; index < this.dataLength; ++index)
      this.data[index] = bi.data[index];
  }

  private SfBigInteger(uint[] inData)
  {
    this.dataLength = inData.Length;
    if (this.dataLength > 70)
      throw new ArithmeticException("Byte overflow in constructor.");
    this.data = new uint[70];
    int index1 = this.dataLength - 1;
    int index2 = 0;
    while (index1 >= 0)
    {
      this.data[index2] = inData[index1];
      --index1;
      ++index2;
    }
    while (this.dataLength > 1 && this.data[this.dataLength - 1] == 0U)
      --this.dataLength;
  }

  internal SfBigInteger(byte[] inData)
  {
    this.dataLength = inData.Length >> 2;
    int num = inData.Length & 3;
    if (num != 0)
      ++this.dataLength;
    if (this.dataLength > 70)
      throw new ArithmeticException("Byte overflow in constructor.");
    this.data = new uint[70];
    int index1 = inData.Length - 1;
    int index2 = 0;
    while (index1 >= 3)
    {
      this.data[index2] = (uint) (((int) inData[index1 - 3] << 24) + ((int) inData[index1 - 2] << 16 /*0x10*/) + ((int) inData[index1 - 1] << 8)) + (uint) inData[index1];
      index1 -= 4;
      ++index2;
    }
    switch (num)
    {
      case 1:
        this.data[this.dataLength - 1] = (uint) inData[0];
        break;
      case 2:
        this.data[this.dataLength - 1] = ((uint) inData[0] << 8) + (uint) inData[1];
        break;
      case 3:
        this.data[this.dataLength - 1] = (uint) (((int) inData[0] << 16 /*0x10*/) + ((int) inData[1] << 8)) + (uint) inData[2];
        break;
    }
    while (this.dataLength > 1 && this.data[this.dataLength - 1] == 0U)
      --this.dataLength;
  }

  private SfBigInteger(ulong value)
  {
    this.data = new uint[70];
    for (this.dataLength = 0; value != 0UL && this.dataLength < 70; ++this.dataLength)
    {
      this.data[this.dataLength] = (uint) (value & (ulong) uint.MaxValue);
      value >>= 32 /*0x20*/;
    }
    if (value != 0UL || ((int) this.data[69] & int.MinValue) != 0)
      throw new ArithmeticException("Positive overflow in constructor.");
    if (this.dataLength != 0)
      return;
    this.dataLength = 1;
  }

  private long LongValue()
  {
    long num = (long) this.data[0];
    try
    {
      num |= (long) this.data[1] << 32 /*0x20*/;
    }
    catch (Exception ex)
    {
      if (((int) this.data[0] & int.MinValue) != 0)
        num = (long) (int) this.data[0];
    }
    return num;
  }

  internal int IntValue() => (int) this.data[0];

  public static bool operator <(SfBigInteger b1, SfBigInteger b2)
  {
    int index1 = 69;
    if (((int) b1.data[index1] & int.MinValue) != 0 && ((int) b2.data[index1] & int.MinValue) == 0)
      return true;
    if (((int) b1.data[index1] & int.MinValue) == 0 && ((int) b2.data[index1] & int.MinValue) != 0)
      return false;
    int index2 = (b1.dataLength > b2.dataLength ? b1.dataLength : b2.dataLength) - 1;
    while (index2 >= 0 && (int) b1.data[index2] == (int) b2.data[index2])
      --index2;
    return index2 >= 0 && b1.data[index2] < b2.data[index2];
  }

  public static SfBigInteger operator -(SfBigInteger b1)
  {
    if (b1.dataLength == 1 && b1.data[0] == 0U)
      return new SfBigInteger();
    SfBigInteger sfBigInteger = new SfBigInteger(b1);
    for (int index = 0; index < 70; ++index)
      sfBigInteger.data[index] = ~b1.data[index];
    long num1 = 1;
    for (int index = 0; num1 != 0L && index < 70; ++index)
    {
      long num2 = (long) sfBigInteger.data[index] + 1L;
      sfBigInteger.data[index] = (uint) ((ulong) num2 & (ulong) uint.MaxValue);
      num1 = num2 >> 32 /*0x20*/;
    }
    if (((int) b1.data[69] & int.MinValue) == ((int) sfBigInteger.data[69] & int.MinValue))
      throw new ArithmeticException("Overflow in negation.\n");
    sfBigInteger.dataLength = 70;
    while (sfBigInteger.dataLength > 1 && sfBigInteger.data[sfBigInteger.dataLength - 1] == 0U)
      --sfBigInteger.dataLength;
    return sfBigInteger;
  }

  public static SfBigInteger operator %(SfBigInteger b1, SfBigInteger b2)
  {
    SfBigInteger outQuotient = new SfBigInteger();
    SfBigInteger outRemainder = new SfBigInteger(b1);
    int index = 69;
    bool flag = false;
    if (((int) b1.data[index] & int.MinValue) != 0)
    {
      b1 = -b1;
      flag = true;
    }
    if (((int) b2.data[index] & int.MinValue) != 0)
      b2 = -b2;
    if (b1 < b2)
      return outRemainder;
    if (b2.dataLength == 1)
      SfBigInteger.SingleByteDivide(b1, b2, outQuotient, outRemainder);
    else
      SfBigInteger.MultiByteDivide(b1, b2, outQuotient, outRemainder);
    return flag ? -outRemainder : outRemainder;
  }

  public static implicit operator SfBigInteger(long value) => new SfBigInteger(value);

  public static implicit operator SfBigInteger(ulong value) => new SfBigInteger(value);

  public static implicit operator SfBigInteger(int value) => new SfBigInteger((long) value);

  public static implicit operator SfBigInteger(uint value) => new SfBigInteger((ulong) value);

  public static SfBigInteger operator --(SfBigInteger b1)
  {
    SfBigInteger sfBigInteger = new SfBigInteger(b1);
    bool flag = true;
    int index1;
    for (index1 = 0; flag && index1 < 70; ++index1)
    {
      long num = (long) sfBigInteger.data[index1] - 1L;
      sfBigInteger.data[index1] = (uint) ((ulong) num & (ulong) uint.MaxValue);
      if (num >= 0L)
        flag = false;
    }
    if (index1 > sfBigInteger.dataLength)
      sfBigInteger.dataLength = index1;
    while (sfBigInteger.dataLength > 1 && sfBigInteger.data[sfBigInteger.dataLength - 1] == 0U)
      --sfBigInteger.dataLength;
    int index2 = 69;
    if (((int) b1.data[index2] & int.MinValue) != 0 && ((int) sfBigInteger.data[index2] & int.MinValue) != ((int) b1.data[index2] & int.MinValue))
      throw new ArithmeticException("Underflow in --.");
    return sfBigInteger;
  }

  public static SfBigInteger operator <<(SfBigInteger b1, int shiftVal)
  {
    SfBigInteger sfBigInteger = new SfBigInteger(b1);
    sfBigInteger.dataLength = SfBigInteger.ShiftLeft(sfBigInteger.data, shiftVal);
    return sfBigInteger;
  }

  public static SfBigInteger operator *(SfBigInteger b1, SfBigInteger b2)
  {
    int index1 = 69;
    bool flag1 = false;
    bool flag2 = false;
    try
    {
      if (((int) b1.data[index1] & int.MinValue) != 0)
      {
        flag1 = true;
        b1 = -b1;
      }
      if (((int) b2.data[index1] & int.MinValue) != 0)
      {
        flag2 = true;
        b2 = -b2;
      }
    }
    catch (Exception ex)
    {
    }
    SfBigInteger sfBigInteger = new SfBigInteger();
    try
    {
      for (int index2 = 0; index2 < b1.dataLength; ++index2)
      {
        if (b1.data[index2] != 0U)
        {
          ulong num1 = 0;
          int index3 = 0;
          int index4 = index2;
          while (index3 < b2.dataLength)
          {
            ulong num2 = (ulong) b1.data[index2] * (ulong) b2.data[index3] + (ulong) sfBigInteger.data[index4] + num1;
            sfBigInteger.data[index4] = (uint) (num2 & (ulong) uint.MaxValue);
            num1 = num2 >> 32 /*0x20*/;
            ++index3;
            ++index4;
          }
          if (num1 != 0UL)
            sfBigInteger.data[index2 + b2.dataLength] = (uint) num1;
        }
      }
    }
    catch (Exception ex)
    {
      throw new ArithmeticException("Multiplication overflow.");
    }
    sfBigInteger.dataLength = b1.dataLength + b2.dataLength;
    if (sfBigInteger.dataLength > 70)
      sfBigInteger.dataLength = 70;
    while (sfBigInteger.dataLength > 1 && sfBigInteger.data[sfBigInteger.dataLength - 1] == 0U)
      --sfBigInteger.dataLength;
    if (((int) sfBigInteger.data[index1] & int.MinValue) != 0)
    {
      if (flag1 != flag2 && sfBigInteger.data[index1] == 2147483648U /*0x80000000*/)
      {
        if (sfBigInteger.dataLength == 1)
          return sfBigInteger;
        bool flag3 = true;
        for (int index5 = 0; index5 < sfBigInteger.dataLength - 1 && flag3; ++index5)
        {
          if (sfBigInteger.data[index5] != 0U)
            flag3 = false;
        }
        if (flag3)
          return sfBigInteger;
      }
      throw new ArithmeticException("Multiplication overflow.");
    }
    return flag1 != flag2 ? -sfBigInteger : sfBigInteger;
  }

  private static void MultiByteDivide(
    SfBigInteger b1,
    SfBigInteger b2,
    SfBigInteger outQuotient,
    SfBigInteger outRemainder)
  {
    uint[] numArray = new uint[70];
    int length1 = b1.dataLength + 1;
    uint[] buffer = new uint[length1];
    uint num1 = 2147483648 /*0x80000000*/;
    uint num2 = b2.data[b2.dataLength - 1];
    int shiftVal = 0;
    int num3 = 0;
    for (; num1 != 0U && ((int) num2 & (int) num1) == 0; num1 >>= 1)
      ++shiftVal;
    for (int index = 0; index < b1.dataLength; ++index)
      buffer[index] = b1.data[index];
    SfBigInteger.ShiftLeft(buffer, shiftVal);
    b2 <<= shiftVal;
    int num4 = length1 - b2.dataLength;
    int index1 = length1 - 1;
    ulong num5 = (ulong) b2.data[b2.dataLength - 1];
    ulong num6 = (ulong) b2.data[b2.dataLength - 2];
    int length2 = b2.dataLength + 1;
    uint[] inData = new uint[length2];
    for (; num4 > 0; --num4)
    {
      ulong num7 = ((ulong) buffer[index1] << 32 /*0x20*/) + (ulong) buffer[index1 - 1];
      ulong num8 = num7 / num5;
      ulong num9 = num7 % num5;
      bool flag = false;
      while (!flag)
      {
        flag = true;
        if (num8 == 4294967296UL /*0x0100000000*/ || num8 * num6 > (num9 << 32 /*0x20*/) + (ulong) buffer[index1 - 2])
        {
          --num8;
          num9 += num5;
          if (num9 < 4294967296UL /*0x0100000000*/)
            flag = false;
        }
      }
      for (int index2 = 0; index2 < length2; ++index2)
        inData[index2] = buffer[index1 - index2];
      SfBigInteger sfBigInteger1 = new SfBigInteger(inData);
      SfBigInteger sfBigInteger2 = b2 * (SfBigInteger) (long) num8;
      while (sfBigInteger2 > sfBigInteger1)
      {
        --num8;
        sfBigInteger2 -= b2;
      }
      SfBigInteger sfBigInteger3 = sfBigInteger1 - sfBigInteger2;
      for (int index3 = 0; index3 < length2; ++index3)
        buffer[index1 - index3] = sfBigInteger3.data[b2.dataLength - index3];
      numArray[num3++] = (uint) num8;
      --index1;
    }
    outQuotient.dataLength = num3;
    int index4 = 0;
    int index5 = outQuotient.dataLength - 1;
    while (index5 >= 0)
    {
      outQuotient.data[index4] = numArray[index5];
      --index5;
      ++index4;
    }
    for (; index4 < 70; ++index4)
      outQuotient.data[index4] = 0U;
    while (outQuotient.dataLength > 1 && outQuotient.data[outQuotient.dataLength - 1] == 0U)
      --outQuotient.dataLength;
    if (outQuotient.dataLength == 0)
      outQuotient.dataLength = 1;
    outRemainder.dataLength = SfBigInteger.ShiftRight(buffer, shiftVal);
    int index6;
    for (index6 = 0; index6 < outRemainder.dataLength; ++index6)
      outRemainder.data[index6] = buffer[index6];
    for (; index6 < 70; ++index6)
      outRemainder.data[index6] = 0U;
  }

  public static SfBigInteger operator -(SfBigInteger b1, SfBigInteger b2)
  {
    SfBigInteger sfBigInteger = new SfBigInteger();
    sfBigInteger.dataLength = b1.dataLength > b2.dataLength ? b1.dataLength : b2.dataLength;
    long num1 = 0;
    for (int index = 0; index < sfBigInteger.dataLength; ++index)
    {
      long num2 = (long) b1.data[index] - (long) b2.data[index] - num1;
      sfBigInteger.data[index] = (uint) ((ulong) num2 & (ulong) uint.MaxValue);
      num1 = num2 >= 0L ? 0L : 1L;
    }
    if (num1 != 0L)
    {
      for (int dataLength = sfBigInteger.dataLength; dataLength < 70; ++dataLength)
        sfBigInteger.data[dataLength] = uint.MaxValue;
      sfBigInteger.dataLength = 70;
    }
    while (sfBigInteger.dataLength > 1 && sfBigInteger.data[sfBigInteger.dataLength - 1] == 0U)
      --sfBigInteger.dataLength;
    int index1 = 69;
    if (((int) b1.data[index1] & int.MinValue) != ((int) b2.data[index1] & int.MinValue) && ((int) sfBigInteger.data[index1] & int.MinValue) != ((int) b1.data[index1] & int.MinValue))
      throw new ArithmeticException();
    return sfBigInteger;
  }

  public static bool operator >(SfBigInteger b1, SfBigInteger b2)
  {
    int index1 = 69;
    if (((int) b1.data[index1] & int.MinValue) != 0 && ((int) b2.data[index1] & int.MinValue) == 0)
      return false;
    if (((int) b1.data[index1] & int.MinValue) == 0 && ((int) b2.data[index1] & int.MinValue) != 0)
      return true;
    int index2 = (b1.dataLength > b2.dataLength ? b1.dataLength : b2.dataLength) - 1;
    while (index2 >= 0 && (int) b1.data[index2] == (int) b2.data[index2])
      --index2;
    return index2 >= 0 && b1.data[index2] > b2.data[index2];
  }

  private static int ShiftRight(uint[] buffer, int shiftVal)
  {
    int num1 = 32 /*0x20*/;
    int num2 = 0;
    int length = buffer.Length;
    while (length > 1 && buffer[length - 1] == 0U)
      --length;
    for (int index1 = shiftVal; index1 > 0; index1 -= num1)
    {
      if (index1 < num1)
      {
        num1 = index1;
        num2 = 32 /*0x20*/ - num1;
      }
      ulong num3 = 0;
      for (int index2 = length - 1; index2 >= 0; --index2)
      {
        ulong num4 = (ulong) buffer[index2] >> num1 | num3;
        num3 = (ulong) buffer[index2] << num2;
        buffer[index2] = (uint) num4;
      }
    }
    while (length > 1 && buffer[length - 1] == 0U)
      --length;
    return length;
  }

  private static void SingleByteDivide(
    SfBigInteger b1,
    SfBigInteger b2,
    SfBigInteger outQuotient,
    SfBigInteger outRemainder)
  {
    uint[] numArray = new uint[70];
    int num1 = 0;
    for (int index = 0; index < 70; ++index)
      outRemainder.data[index] = b1.data[index];
    outRemainder.dataLength = b1.dataLength;
    while (outRemainder.dataLength > 1 && outRemainder.data[outRemainder.dataLength - 1] == 0U)
      --outRemainder.dataLength;
    ulong num2 = (ulong) b2.data[0];
    int index1 = outRemainder.dataLength - 1;
    ulong num3 = (ulong) outRemainder.data[index1];
    if (num3 >= num2)
    {
      ulong num4 = num3 / num2;
      numArray[num1++] = (uint) num4;
      outRemainder.data[index1] = (uint) (num3 % num2);
    }
    ulong num5;
    for (int index2 = index1 - 1; index2 >= 0; outRemainder.data[index2--] = (uint) (num5 % num2))
    {
      num5 = ((ulong) outRemainder.data[index2 + 1] << 32 /*0x20*/) + (ulong) outRemainder.data[index2];
      ulong num6 = num5 / num2;
      numArray[num1++] = (uint) num6;
      outRemainder.data[index2 + 1] = 0U;
    }
    outQuotient.dataLength = num1;
    int index3 = 0;
    int index4 = outQuotient.dataLength - 1;
    while (index4 >= 0)
    {
      outQuotient.data[index3] = numArray[index4];
      --index4;
      ++index3;
    }
    for (; index3 < 70; ++index3)
      outQuotient.data[index3] = 0U;
    while (outQuotient.dataLength > 1 && outQuotient.data[outQuotient.dataLength - 1] == 0U)
      --outQuotient.dataLength;
    if (outQuotient.dataLength == 0)
      outQuotient.dataLength = 1;
    while (outRemainder.dataLength > 1 && outRemainder.data[outRemainder.dataLength - 1] == 0U)
      --outRemainder.dataLength;
  }

  private static int ShiftLeft(uint[] buffer, int shiftVal)
  {
    int num1 = 32 /*0x20*/;
    int length = buffer.Length;
    while (length > 1 && buffer[length - 1] == 0U)
      --length;
    for (int index1 = shiftVal; index1 > 0; index1 -= num1)
    {
      if (index1 < num1)
        num1 = index1;
      ulong num2 = 0;
      for (int index2 = 0; index2 < length; ++index2)
      {
        ulong num3 = (ulong) buffer[index2] << num1 | num2;
        buffer[index2] = (uint) (num3 & (ulong) uint.MaxValue);
        num2 = num3 >> 32 /*0x20*/;
      }
      if (num2 != 0UL && length + 1 <= buffer.Length)
      {
        buffer[length] = (uint) num2;
        ++length;
      }
    }
    return length;
  }
}
