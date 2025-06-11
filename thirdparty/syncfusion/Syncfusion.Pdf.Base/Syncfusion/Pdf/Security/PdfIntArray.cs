// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfIntArray
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PdfIntArray
{
  private int[] intValues;

  public PdfIntArray(int length) => this.intValues = new int[length];

  private PdfIntArray(int[] values) => this.intValues = values;

  public PdfIntArray(Number bigInterger, int minimumLength)
  {
    if (bigInterger.SignValue == -1)
      throw new ArgumentException("Only positive Integers allowed", "bigint");
    if (bigInterger.SignValue == 0)
    {
      this.intValues = new int[1];
    }
    else
    {
      byte[] byteArrayUnsigned = bigInterger.ToByteArrayUnsigned();
      int length = byteArrayUnsigned.Length;
      int val1 = (length + 3) / 4;
      this.intValues = new int[Math.Max(val1, minimumLength)];
      int num1 = length % 4;
      int num2 = 0;
      if (0 < num1)
      {
        int num3 = (int) byteArrayUnsigned[num2++];
        while (num2 < num1)
          num3 = num3 << 8 | (int) byteArrayUnsigned[num2++];
        this.intValues[--val1] = num3;
      }
      int num4;
      for (; val1 > 0; this.intValues[--val1] = num4)
      {
        num4 = (int) byteArrayUnsigned[num2++];
        for (int index = 1; index < 4; ++index)
          num4 = num4 << 8 | (int) byteArrayUnsigned[num2++];
      }
    }
  }

  public int GetLength()
  {
    int length = this.intValues.Length;
    if (length < 1)
      return 0;
    if (this.intValues[0] != 0)
    {
      do
        ;
      while (this.intValues[--length] == 0);
      return length + 1;
    }
    while (this.intValues[--length] == 0)
    {
      if (length <= 0)
        return 0;
    }
    return length + 1;
  }

  public int BitLength
  {
    get
    {
      int length = this.GetLength();
      if (length == 0)
        return 0;
      int index = length - 1;
      uint intValue = (uint) this.intValues[index];
      int bitLength = (index << 5) + 1;
      if (intValue > (uint) ushort.MaxValue)
      {
        if (intValue > 16777215U /*0xFFFFFF*/)
        {
          bitLength += 24;
          intValue >>= 24;
        }
        else
        {
          bitLength += 16 /*0x10*/;
          intValue >>= 16 /*0x10*/;
        }
      }
      else if (intValue > (uint) byte.MaxValue)
      {
        bitLength += 8;
        intValue >>= 8;
      }
      for (; intValue > 1U; intValue >>= 1)
        ++bitLength;
      return bitLength;
    }
  }

  private int[] ResizedValues(int length)
  {
    int[] destinationArray = new int[length];
    int length1 = this.intValues.Length;
    int length2 = length1 < length ? length1 : length;
    Array.Copy((Array) this.intValues, 0, (Array) destinationArray, 0, length2);
    return destinationArray;
  }

  public Number ToBigInteger()
  {
    int length = this.GetLength();
    if (length == 0)
      return Number.Zero;
    int intValue = this.intValues[length - 1];
    byte[] numArray = new byte[4];
    int num1 = 0;
    bool flag = false;
    for (int index = 3; index >= 0; --index)
    {
      byte num2 = (byte) (intValue >>> 8 * index);
      if (flag || num2 != (byte) 0)
      {
        flag = true;
        numArray[num1++] = num2;
      }
    }
    byte[] bytes = new byte[4 * (length - 1) + num1];
    for (int index = 0; index < num1; ++index)
      bytes[index] = numArray[index];
    for (int index1 = length - 2; index1 >= 0; --index1)
    {
      for (int index2 = 3; index2 >= 0; --index2)
        bytes[num1++] = (byte) (this.intValues[index1] >>> 8 * index2);
    }
    return new Number(1, bytes);
  }

  public void ShiftLeft()
  {
    int length = this.GetLength();
    if (length == 0)
      return;
    if (this.intValues[length - 1] < 0)
    {
      ++length;
      if (length > this.intValues.Length)
        this.intValues = this.ResizedValues(this.intValues.Length + 1);
    }
    bool flag1 = false;
    for (int index = 0; index < length; ++index)
    {
      bool flag2 = this.intValues[index] < 0;
      this.intValues[index] <<= 1;
      if (flag1)
        this.intValues[index] |= 1;
      flag1 = flag2;
    }
  }

  public PdfIntArray ShiftLeft(int number)
  {
    int length = this.GetLength();
    if (length == 0 || number == 0)
      return this;
    if (number > 31 /*0x1F*/)
      throw new ArgumentException("bit shift is not possible");
    int[] values = new int[length + 1];
    int num = 32 /*0x20*/ - number;
    values[0] = this.intValues[0] << number;
    for (int index = 1; index < length; ++index)
      values[index] = this.intValues[index] << number | this.intValues[index - 1] >>> num;
    values[length] = this.intValues[length - 1] >>> num;
    return new PdfIntArray(values);
  }

  public void AddShifted(PdfIntArray values, int shift)
  {
    int length1 = values.GetLength();
    int length2 = length1 + shift;
    if (length2 > this.intValues.Length)
      this.intValues = this.ResizedValues(length2);
    for (int index = 0; index < length1; ++index)
      this.intValues[index + shift] ^= values.intValues[index];
  }

  public int Length => this.intValues.Length;

  public bool TestBit(int number) => (this.intValues[number >> 5] & 1 << number) != 0;

  public void FlipBit(int number) => this.intValues[number >> 5] ^= 1 << number;

  public void SetBit(int number) => this.intValues[number >> 5] |= 1 << number;

  public PdfIntArray Multiply(PdfIntArray values, int value)
  {
    int length = value + 31 /*0x1F*/ >> 5;
    if (this.intValues.Length < length)
      this.intValues = this.ResizedValues(length);
    PdfIntArray values1 = new PdfIntArray(values.ResizedValues(values.Length + 1));
    PdfIntArray pdfIntArray = new PdfIntArray(value + value + 31 /*0x1F*/ >> 5);
    int num = 1;
    for (int index = 0; index < 32 /*0x20*/; ++index)
    {
      for (int shift = 0; shift < length; ++shift)
      {
        if ((this.intValues[shift] & num) != 0)
          pdfIntArray.AddShifted(values1, shift);
      }
      num <<= 1;
      values1.ShiftLeft();
    }
    return pdfIntArray;
  }

  public void Reduce(int value, int[] redPol)
  {
    for (int number1 = value + value - 2; number1 >= value; --number1)
    {
      if (this.TestBit(number1))
      {
        int number2 = number1 - value;
        this.FlipBit(number2);
        this.FlipBit(number1);
        int length = redPol.Length;
        while (--length >= 0)
          this.FlipBit(redPol[length] + number2);
      }
    }
    this.intValues = this.ResizedValues(value + 31 /*0x1F*/ >> 5);
  }

  public PdfIntArray Square(int value)
  {
    int[] numArray = new int[16 /*0x10*/]
    {
      0,
      1,
      4,
      5,
      16 /*0x10*/,
      17,
      20,
      21,
      64 /*0x40*/,
      65,
      68,
      69,
      80 /*0x50*/,
      81,
      84,
      85
    };
    int length = value + 31 /*0x1F*/ >> 5;
    if (this.intValues.Length < length)
      this.intValues = this.ResizedValues(length);
    PdfIntArray pdfIntArray = new PdfIntArray(length + length);
    for (int index1 = 0; index1 < length; ++index1)
    {
      int num1 = 0;
      for (int index2 = 0; index2 < 4; ++index2)
      {
        int num2 = num1 >>> 8;
        int index3 = this.intValues[index1] >>> index2 * 4 & 15;
        int num3 = numArray[index3] << 24;
        num1 = num2 | num3;
      }
      pdfIntArray.intValues[index1 + index1] = num1;
      int num4 = 0;
      int num5 = this.intValues[index1] >>> 16 /*0x10*/;
      for (int index4 = 0; index4 < 4; ++index4)
      {
        int num6 = num4 >>> 8;
        int index5 = num5 >>> index4 * 4 & 15;
        int num7 = numArray[index5] << 24;
        num4 = num6 | num7;
      }
      pdfIntArray.intValues[index1 + index1 + 1] = num4;
    }
    return pdfIntArray;
  }

  public override bool Equals(object o)
  {
    if (!(o is PdfIntArray))
      return false;
    PdfIntArray pdfIntArray = (PdfIntArray) o;
    int length = this.GetLength();
    if (pdfIntArray.GetLength() != length)
      return false;
    for (int index = 0; index < length; ++index)
    {
      if (this.intValues[index] != pdfIntArray.intValues[index])
        return false;
    }
    return true;
  }

  public override int GetHashCode()
  {
    int length = this.GetLength();
    int hashCode = length;
    while (--length >= 0)
      hashCode = hashCode * 17 ^ this.intValues[length];
    return hashCode;
  }

  internal PdfIntArray Copy() => new PdfIntArray((int[]) this.intValues.Clone());

  public override string ToString()
  {
    int length1 = this.GetLength();
    if (length1 == 0)
      return "0";
    StringBuilder stringBuilder = new StringBuilder(Convert.ToString(this.intValues[length1 - 1], 2));
    for (int index = length1 - 2; index >= 0; --index)
    {
      string str = Convert.ToString(this.intValues[index], 2);
      for (int length2 = str.Length; length2 < 8; ++length2)
        str = "0" + str;
      stringBuilder.Append(str);
    }
    return stringBuilder.ToString();
  }
}
