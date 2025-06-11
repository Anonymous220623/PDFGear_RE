// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.FinitePFieldObject
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class FinitePFieldObject : EllipticCurveElements
{
  private readonly Number numberPQ;
  private readonly Number numberPX;

  public FinitePFieldObject(Number numberPQ, Number numberPX)
  {
    this.numberPQ = numberPX.CompareTo(numberPQ) < 0 ? numberPQ : throw new ArgumentException("value too large in field element");
    this.numberPX = numberPX;
  }

  public override Number ToIntValue() => this.numberPX;

  public override string ECElementName => "Fp";

  public override int ElementSize => this.numberPQ.BitLength;

  public override EllipticCurveElements SumValue(EllipticCurveElements value)
  {
    return (EllipticCurveElements) new FinitePFieldObject(this.numberPQ, this.numberPX.Add(value.ToIntValue()).Mod(this.numberPQ));
  }

  public override EllipticCurveElements Subtract(EllipticCurveElements value)
  {
    return (EllipticCurveElements) new FinitePFieldObject(this.numberPQ, this.numberPX.Subtract(value.ToIntValue()).Mod(this.numberPQ));
  }

  public override EllipticCurveElements Multiply(EllipticCurveElements value)
  {
    return (EllipticCurveElements) new FinitePFieldObject(this.numberPQ, this.numberPX.Multiply(value.ToIntValue()).Mod(this.numberPQ));
  }

  public override EllipticCurveElements Divide(EllipticCurveElements value)
  {
    return (EllipticCurveElements) new FinitePFieldObject(this.numberPQ, this.numberPX.Multiply(value.ToIntValue().ModInverse(this.numberPQ)).Mod(this.numberPQ));
  }

  public override EllipticCurveElements Negate()
  {
    return (EllipticCurveElements) new FinitePFieldObject(this.numberPQ, this.numberPX.Negate().Mod(this.numberPQ));
  }

  public override EllipticCurveElements Square()
  {
    return (EllipticCurveElements) new FinitePFieldObject(this.numberPQ, this.numberPX.Multiply(this.numberPX).Mod(this.numberPQ));
  }

  public override EllipticCurveElements Invert()
  {
    return (EllipticCurveElements) new FinitePFieldObject(this.numberPQ, this.numberPX.ModInverse(this.numberPQ));
  }

  public override EllipticCurveElements SquareRoot()
  {
    if (!this.numberPQ.TestBit(0))
      throw new PdfException("even value");
    if (this.numberPQ.TestBit(1))
    {
      EllipticCurveElements ellipticCurveElements = (EllipticCurveElements) new FinitePFieldObject(this.numberPQ, this.numberPX.ModPow(this.numberPQ.ShiftRight(2).Add(Number.One), this.numberPQ));
      return !this.Equals(ellipticCurveElements.Square()) ? (EllipticCurveElements) null : ellipticCurveElements;
    }
    Number number1 = this.numberPQ.Subtract(Number.One);
    Number e = number1.ShiftRight(1);
    if (!this.numberPX.ModPow(e, this.numberPQ).Equals((object) Number.One))
      return (EllipticCurveElements) null;
    Number numberK1 = number1.ShiftRight(2).ShiftLeft(1).Add(Number.One);
    Number numberPx = this.numberPX;
    Number n = numberPx.ShiftLeft(2).Mod(this.numberPQ);
    Number number2;
    do
    {
      SecureRandomAlgorithm random = new SecureRandomAlgorithm();
      Number number3;
      do
      {
        number3 = new Number(this.numberPQ.BitLength, random);
      }
      while (number3.CompareTo(this.numberPQ) >= 0 || !number3.Multiply(number3).Subtract(n).ModPow(e, this.numberPQ).Equals((object) number1));
      Number[] numberArray = FinitePFieldObject.FLSequence(this.numberPQ, number3, numberPx, numberK1);
      number2 = numberArray[0];
      Number val = numberArray[1];
      if (val.Multiply(val).Mod(this.numberPQ).Equals((object) n))
      {
        if (val.TestBit(0))
          val = val.Add(this.numberPQ);
        return (EllipticCurveElements) new FinitePFieldObject(this.numberPQ, val.ShiftRight(1));
      }
    }
    while (number2.Equals((object) Number.One) || number2.Equals((object) number1));
    return (EllipticCurveElements) null;
  }

  private static Number[] FLSequence(
    Number valueP,
    Number numberP,
    Number numberQ,
    Number numberK1)
  {
    int bitLength = numberK1.BitLength;
    int lowestSetBit = numberK1.GetLowestSetBit();
    Number number1 = Number.One;
    Number val1 = Number.Two;
    Number val2 = numberP;
    Number number2 = Number.One;
    Number val3 = Number.One;
    for (int index = bitLength - 1; index >= lowestSetBit + 1; --index)
    {
      number2 = number2.Multiply(val3).Mod(valueP);
      if (numberK1.TestBit(index))
      {
        val3 = number2.Multiply(numberQ).Mod(valueP);
        number1 = number1.Multiply(val2).Mod(valueP);
        val1 = val2.Multiply(val1).Subtract(numberP.Multiply(number2)).Mod(valueP);
        val2 = val2.Multiply(val2).Subtract(val3.ShiftLeft(1)).Mod(valueP);
      }
      else
      {
        val3 = number2;
        number1 = number1.Multiply(val1).Subtract(number2).Mod(valueP);
        val2 = val2.Multiply(val1).Subtract(numberP.Multiply(number2)).Mod(valueP);
        val1 = val1.Multiply(val1).Subtract(number2.ShiftLeft(1)).Mod(valueP);
      }
    }
    Number number3 = number2.Multiply(val3).Mod(valueP);
    Number val4 = number3.Multiply(numberQ).Mod(valueP);
    Number number4 = number1.Multiply(val1).Subtract(number3).Mod(valueP);
    Number val5 = val2.Multiply(val1).Subtract(numberP.Multiply(number3)).Mod(valueP);
    Number val6 = number3.Multiply(val4).Mod(valueP);
    for (int index = 1; index <= lowestSetBit; ++index)
    {
      number4 = number4.Multiply(val5).Mod(valueP);
      val5 = val5.Multiply(val5).Subtract(val6.ShiftLeft(1)).Mod(valueP);
      val6 = val6.Multiply(val6).Mod(valueP);
    }
    return new Number[2]{ number4, val5 };
  }

  public override bool Equals(object element)
  {
    if (element == this)
      return true;
    return element is FinitePFieldObject element1 && this.Equals(element1);
  }

  protected bool Equals(FinitePFieldObject element)
  {
    return this.numberPQ.Equals((object) element.numberPQ) && this.Equals((EllipticCurveElements) element);
  }

  public override int GetHashCode() => this.numberPQ.GetHashCode() ^ base.GetHashCode();
}
