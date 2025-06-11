// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.Fraction
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser;

public class Fraction
{
  private const int DEF_MAX_DIGITS = 9;
  private const double DEF_EPS = 1E-09;
  private double m_dNumerator;
  private double m_dDenumerator;

  public Fraction(double dNumerator, double dDenumerator)
  {
    if (dDenumerator == 0.0)
      throw new ArgumentOutOfRangeException(nameof (dDenumerator));
    this.m_dNumerator = dNumerator;
    this.m_dDenumerator = dDenumerator;
  }

  public Fraction(double dNumerator)
    : this(dNumerator, 1.0)
  {
  }

  public double Numerator
  {
    get => this.m_dNumerator;
    set => this.m_dNumerator = value;
  }

  public double Denumerator
  {
    get => this.m_dDenumerator;
    set => this.m_dDenumerator = value;
  }

  public int DenumeratorLen => (int) Math.Log10(this.Denumerator) + 1;

  public static Fraction operator +(Fraction term1, Fraction term2)
  {
    double dNumerator = term1.Numerator * term2.Denumerator + term1.Denumerator * term2.Numerator;
    double dDenumerator = term2.Denumerator * term1.Denumerator;
    double maximumCommonDevisor = Fraction.GetMaximumCommonDevisor(dNumerator, dDenumerator);
    return new Fraction(dNumerator / maximumCommonDevisor, dDenumerator / maximumCommonDevisor);
  }

  public static explicit operator double(Fraction fraction)
  {
    return fraction.Numerator / fraction.Denumerator;
  }

  public static explicit operator Fraction(List<double> arrFraction)
  {
    int num1 = arrFraction != null ? arrFraction.Count : throw new ArgumentNullException(nameof (arrFraction));
    Fraction fraction = (Fraction) null;
    if (num1 > 0)
    {
      fraction = new Fraction(arrFraction[num1 - 1], 1.0);
      for (int index = num1 - 2; index >= 0; --index)
      {
        double num2 = arrFraction[index];
        fraction = fraction.Reverse() + (Fraction) num2;
      }
    }
    return fraction;
  }

  public static explicit operator Fraction(double dValue) => new Fraction(dValue);

  public Fraction Reverse()
  {
    double dNumerator = this.m_dNumerator;
    this.m_dNumerator = this.m_dDenumerator;
    this.m_dDenumerator = dNumerator;
    return this;
  }

  public static Fraction ConvertToFraction(double value, int iDigitsNumber)
  {
    iDigitsNumber = iDigitsNumber >= 1 ? Math.Min(iDigitsNumber, 9) : throw new ArgumentOutOfRangeException(nameof (iDigitsNumber));
    List<double> arrFraction = new List<double>();
    double dLeft1 = value;
    double dLeft2 = Fraction.AddNextNumber(arrFraction, dLeft1);
    Fraction fraction1 = (Fraction) arrFraction;
    double num = Fraction.GetDelta(fraction1, value);
    while (Math.Abs(dLeft2) > 1E-09)
    {
      dLeft2 = Fraction.AddNextNumber(arrFraction, dLeft2);
      Fraction fraction2 = (Fraction) arrFraction;
      if (fraction2.DenumeratorLen <= iDigitsNumber)
      {
        double delta = Fraction.GetDelta(fraction2, value);
        if (delta < num)
        {
          fraction1 = fraction2;
          num = delta;
        }
      }
      else
        break;
    }
    return fraction1;
  }

  private static double GetMaximumCommonDevisor(double dNumerator, double dDenumerator)
  {
    double num1 = Math.Round(Math.Max(dNumerator, dDenumerator));
    double maximumCommonDevisor = Math.Round(Math.Min(dNumerator, dDenumerator));
    double num2 = num1 % maximumCommonDevisor;
    if (maximumCommonDevisor == 0.0)
      return 1.0;
    double num3;
    for (; num2 != 0.0; num2 = Math.Round(num3 % maximumCommonDevisor))
    {
      num3 = maximumCommonDevisor;
      maximumCommonDevisor = num2;
    }
    return maximumCommonDevisor;
  }

  private static double GetDelta(Fraction fraction, double value)
  {
    return Math.Abs((double) fraction - value);
  }

  private static double AddNextNumber(List<double> arrFraction, double dLeft)
  {
    if (Math.Abs(dLeft) < 1E-09)
      return 0.0;
    if (arrFraction.Count != 0)
      dLeft = 1.0 / dLeft;
    double num = Math.Floor(dLeft);
    arrFraction.Add(num);
    return dLeft - num;
  }

  public override string ToString()
  {
    return $"{this.m_dNumerator.ToString()} / {this.m_dDenumerator.ToString()}";
  }
}
