// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.LargeDecimal
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class LargeDecimal
{
  private readonly Number m_bigDigit;
  private readonly int m_scale;

  public LargeDecimal(Number digit, int value)
  {
    if (value < 0)
      throw new ArgumentException("value may not be negative");
    this.m_bigDigit = digit;
    this.m_scale = value;
  }

  private void CheckScale(LargeDecimal value)
  {
    if (this.m_scale != value.m_scale)
      throw new ArgumentException("same vlaue");
  }

  public LargeDecimal AdjustScale(int newScale)
  {
    if (newScale < 0)
      throw new ArgumentException("Not be negative");
    return newScale == this.m_scale ? this : new LargeDecimal(this.m_bigDigit.ShiftLeft(newScale - this.m_scale), newScale);
  }

  public LargeDecimal Add(LargeDecimal value)
  {
    this.CheckScale(value);
    return new LargeDecimal(this.m_bigDigit.Add(value.m_bigDigit), this.m_scale);
  }

  public LargeDecimal Negate() => new LargeDecimal(this.m_bigDigit.Negate(), this.m_scale);

  public LargeDecimal Subtract(LargeDecimal value) => this.Add(value.Negate());

  public LargeDecimal Subtract(Number value)
  {
    return new LargeDecimal(this.m_bigDigit.Subtract(value.ShiftLeft(this.m_scale)), this.m_scale);
  }

  public int CompareTo(Number val) => this.m_bigDigit.CompareTo(val.ShiftLeft(this.m_scale));

  public Number Floor() => this.m_bigDigit.ShiftRight(this.m_scale);

  public Number Round()
  {
    return this.Add(new LargeDecimal(Number.One, 1).AdjustScale(this.m_scale)).Floor();
  }

  public int Scale => this.m_scale;

  public override string ToString()
  {
    if (this.m_scale == 0)
      return this.m_bigDigit.ToString();
    Number number = this.Floor();
    Number n = this.m_bigDigit.Subtract(number.ShiftLeft(this.m_scale));
    if (this.m_bigDigit.SignValue < 0)
      n = Number.One.ShiftLeft(this.m_scale).Subtract(n);
    if (number.SignValue == -1 && !n.Equals((object) Number.Zero))
      number = number.Add(Number.One);
    string str1 = number.ToString();
    char[] chArray = new char[this.m_scale];
    string str2 = n.ToString(2);
    int length = str2.Length;
    int num = this.m_scale - length;
    for (int index = 0; index < num; ++index)
      chArray[index] = '0';
    for (int index = 0; index < length; ++index)
      chArray[num + index] = str2[index];
    string str3 = new string(chArray);
    StringBuilder stringBuilder = new StringBuilder(str1);
    stringBuilder.Append(".");
    stringBuilder.Append(str3);
    return stringBuilder.ToString();
  }

  public override bool Equals(object obj)
  {
    if (this == obj)
      return true;
    return obj is LargeDecimal largeDecimal && this.m_bigDigit.Equals((object) largeDecimal.m_bigDigit) && this.m_scale == largeDecimal.m_scale;
  }

  public override int GetHashCode() => this.m_bigDigit.GetHashCode() ^ this.m_scale;
}
