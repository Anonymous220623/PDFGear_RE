// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECTanFunction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECTanFunction
{
  public const sbyte WidthValue = 4;
  public const sbyte Power2Width = 16 /*0x10*/;
  private static readonly Number degOne = Number.One.Negate();
  private static readonly Number degTwo = Number.Two.Negate();
  private static readonly Number degThree = Number.Three.Negate();
  private static readonly Number mFour = Number.ValueOf(4L);
  public static readonly ITanuElement[] AlphaZeo = new ITanuElement[9]
  {
    null,
    new ITanuElement(Number.One, Number.Zero),
    null,
    new ITanuElement(ECTanFunction.degThree, ECTanFunction.degOne),
    null,
    new ITanuElement(ECTanFunction.degOne, ECTanFunction.degOne),
    null,
    new ITanuElement(Number.One, ECTanFunction.degOne),
    null
  };
  public static readonly sbyte[][] AlphaZeroT = new sbyte[8][]
  {
    null,
    new sbyte[1]{ (sbyte) 1 },
    null,
    new sbyte[3]{ (sbyte) -1, (sbyte) 0, (sbyte) 1 },
    null,
    new sbyte[3]{ (sbyte) 1, (sbyte) 0, (sbyte) 1 },
    null,
    new sbyte[4]{ (sbyte) -1, (sbyte) 0, (sbyte) 0, (sbyte) 1 }
  };
  public static readonly ITanuElement[] AlphaOne = new ITanuElement[9]
  {
    null,
    new ITanuElement(Number.One, Number.Zero),
    null,
    new ITanuElement(ECTanFunction.degThree, Number.One),
    null,
    new ITanuElement(ECTanFunction.degOne, Number.One),
    null,
    new ITanuElement(Number.One, Number.One),
    null
  };
  public static readonly sbyte[][] AlphaOneT = new sbyte[8][]
  {
    null,
    new sbyte[1]{ (sbyte) 1 },
    null,
    new sbyte[3]{ (sbyte) -1, (sbyte) 0, (sbyte) 1 },
    null,
    new sbyte[3]{ (sbyte) 1, (sbyte) 0, (sbyte) 1 },
    null,
    new sbyte[4]{ (sbyte) -1, (sbyte) 0, (sbyte) 0, (sbyte) -1 }
  };

  public static Number Norm(sbyte byteMu, ITanuElement lambdaValue)
  {
    Number number1 = lambdaValue.num1.Multiply(lambdaValue.num1);
    Number n = lambdaValue.num1.Multiply(lambdaValue.num2);
    Number number2 = lambdaValue.num2.Multiply(lambdaValue.num2).ShiftLeft(1);
    if (byteMu == (sbyte) 1)
      return number1.Add(n).Add(number2);
    if (byteMu == (sbyte) -1)
      return number1.Subtract(n).Add(number2);
    throw new ArgumentException("byteMu must be 1 or -1");
  }

  public static ITanuElement Round(LargeDecimal lambda0, LargeDecimal lambda1, sbyte byteMu)
  {
    int scale = lambda0.Scale;
    if (lambda1.Scale != scale)
      throw new ArgumentException("lambda0 and lambda1 do not have same scale");
    if (byteMu != (sbyte) 1 && byteMu != (sbyte) -1)
      throw new ArgumentException("byteMu must be 1 or -1");
    Number number1 = lambda0.Round();
    Number number2 = lambda1.Round();
    LargeDecimal largeDecimal1 = lambda0.Subtract(number1);
    LargeDecimal largeDecimal2 = lambda1.Subtract(number2);
    LargeDecimal largeDecimal3 = largeDecimal1.Add(largeDecimal1);
    LargeDecimal largeDecimal4 = byteMu != (sbyte) 1 ? largeDecimal3.Subtract(largeDecimal2) : largeDecimal3.Add(largeDecimal2);
    LargeDecimal largeDecimal5 = largeDecimal2.Add(largeDecimal2).Add(largeDecimal2);
    LargeDecimal largeDecimal6 = largeDecimal5.Add(largeDecimal2);
    LargeDecimal largeDecimal7;
    LargeDecimal largeDecimal8;
    if (byteMu == (sbyte) 1)
    {
      largeDecimal7 = largeDecimal1.Subtract(largeDecimal5);
      largeDecimal8 = largeDecimal1.Add(largeDecimal6);
    }
    else
    {
      largeDecimal7 = largeDecimal1.Add(largeDecimal5);
      largeDecimal8 = largeDecimal1.Subtract(largeDecimal6);
    }
    sbyte num1 = 0;
    sbyte num2 = 0;
    if (largeDecimal4.CompareTo(Number.One) >= 0)
    {
      if (largeDecimal7.CompareTo(ECTanFunction.degOne) < 0)
        num2 = byteMu;
      else
        num1 = (sbyte) 1;
    }
    else if (largeDecimal8.CompareTo(Number.Two) >= 0)
      num2 = byteMu;
    if (largeDecimal4.CompareTo(ECTanFunction.degOne) < 0)
    {
      if (largeDecimal7.CompareTo(Number.One) >= 0)
        num2 = -byteMu;
      else
        num1 = (sbyte) -1;
    }
    else if (largeDecimal8.CompareTo(ECTanFunction.degTwo) < 0)
      num2 = -byteMu;
    return new ITanuElement(number1.Add(Number.ValueOf((long) num1)), number2.Add(Number.ValueOf((long) num2)));
  }

  public static LargeDecimal DivideByN(
    Number numberA,
    Number numberS,
    Number numberVM,
    sbyte a,
    int m,
    int c)
  {
    int num = (m + 5) / 2 + c;
    Number val1 = numberA.ShiftRight(m - num - 2 + (int) a);
    Number number1 = numberS.Multiply(val1);
    Number val2 = number1.ShiftRight(m);
    Number number2 = numberVM.Multiply(val2);
    Number number3 = number1.Add(number2);
    Number digit = number3.ShiftRight(num - c);
    if (number3.TestBit(num - c - 1))
      digit = digit.Add(Number.One);
    return new LargeDecimal(digit, c);
  }

  public static Finite2MPoint GetTanU(Finite2MPoint pointP)
  {
    if (pointP.IsInfinity)
      return pointP;
    EllipticCurveElements pointX = pointP.PointX;
    EllipticCurveElements pointY = pointP.PointY;
    return new Finite2MPoint(pointP.Curve, pointX.Square(), pointY.Square(), pointP.IsCompressed);
  }

  public static sbyte FindMU(Field2MCurves curve)
  {
    Number intValue = curve.ElementA.ToIntValue();
    if (intValue.SignValue == 0)
      return -1;
    if (intValue.Equals((object) Number.One))
      return 1;
    throw new ArgumentException("multiplication not possible");
  }

  public static Number[] TraceLC(sbyte byteMu, int numberA, bool div)
  {
    if (byteMu != (sbyte) 1 && byteMu != (sbyte) -1)
      throw new ArgumentException("byteMu must be 1 or -1");
    Number number1;
    Number number2;
    if (div)
    {
      number1 = Number.Two;
      number2 = Number.ValueOf((long) byteMu);
    }
    else
    {
      number1 = Number.Zero;
      number2 = Number.One;
    }
    for (int index = 1; index < numberA; ++index)
    {
      Number number3 = (byteMu != (sbyte) 1 ? number2.Negate() : number2).Subtract(number1.ShiftLeft(1));
      number1 = number2;
      number2 = number3;
    }
    return new Number[2]{ number1, number2 };
  }

  public static Number FindTW(sbyte byteMu, int w)
  {
    if (w == 4)
      return byteMu == (sbyte) 1 ? Number.ValueOf(6L) : Number.ValueOf(10L);
    Number[] numberArray = ECTanFunction.TraceLC(byteMu, w, false);
    Number m = Number.Zero.SetBit(w);
    Number val = numberArray[1].ModInverse(m);
    return Number.Two.Multiply(numberArray[0]).Multiply(val).Mod(m);
  }

  public static Number[] FindSI(Field2MCurves curve)
  {
    int num = curve.IsKOBLITZ ? curve.PointM : throw new ArgumentException("si is defined for Koblitz curves only");
    int intValue1 = curve.ElementA.ToIntValue().IntValue;
    sbyte byteMu = curve.MU();
    int intValue2 = curve.NumberY.IntValue;
    int numberA = num + 3 - intValue1;
    Number[] numberArray = ECTanFunction.TraceLC(byteMu, numberA, false);
    Number number1;
    Number number2;
    if (byteMu == (sbyte) 1)
    {
      number1 = Number.One.Subtract(numberArray[1]);
      number2 = Number.One.Subtract(numberArray[0]);
    }
    else
    {
      if (byteMu != (sbyte) -1)
        throw new ArgumentException("byteMu must be 1 or -1");
      number1 = Number.One.Add(numberArray[1]);
      number2 = Number.One.Add(numberArray[0]);
    }
    Number[] si = new Number[2];
    if (intValue2 == 2)
    {
      si[0] = number1.ShiftRight(1);
      si[1] = number2.ShiftRight(1).Negate();
    }
    else
    {
      if (intValue2 != 4)
        throw new ArgumentException("Cofactor");
      si[0] = number1.ShiftRight(2);
      si[1] = number2.ShiftRight(2).Negate();
    }
    return si;
  }

  public static ITanuElement MODFun(
    Number numberA,
    int m,
    sbyte a,
    Number[] numberS,
    sbyte byteMu,
    sbyte c)
  {
    Number number = byteMu != (sbyte) 1 ? numberS[0].Subtract(numberS[1]) : numberS[0].Add(numberS[1]);
    Number numberVM = ECTanFunction.TraceLC(byteMu, m, true)[1];
    ITanuElement itanuElement = ECTanFunction.Round(ECTanFunction.DivideByN(numberA, numberS[0], numberVM, a, m, (int) c), ECTanFunction.DivideByN(numberA, numberS[1], numberVM, a, m, (int) c), byteMu);
    return new ITanuElement(numberA.Subtract(number.Multiply(itanuElement.num1)).Subtract(Number.ValueOf(2L).Multiply(numberS[1]).Multiply(itanuElement.num2)), numberS[1].Multiply(itanuElement.num1).Subtract(numberS[0].Multiply(itanuElement.num2)));
  }

  public static Finite2MPoint MultiplyFromTnaf(Finite2MPoint pointP, sbyte[] u)
  {
    Finite2MPoint pointP1 = (Finite2MPoint) pointP.Curve.IsInfinity;
    for (int index = u.Length - 1; index >= 0; --index)
    {
      pointP1 = ECTanFunction.GetTanU(pointP1);
      if (u[index] == (sbyte) 1)
        pointP1 = pointP1.AddSimple(pointP);
      else if (u[index] == (sbyte) -1)
        pointP1 = pointP1.SubtractSimple(pointP);
    }
    return pointP1;
  }

  public static sbyte[] GetTAdic(
    sbyte byteMu,
    ITanuElement lambdaValue,
    sbyte width,
    Number pow2w,
    Number tw,
    ITanuElement[] alpha)
  {
    if (byteMu != (sbyte) 1 && byteMu != (sbyte) -1)
      throw new ArgumentException("byteMu must be 1 or -1");
    int bitLength = ECTanFunction.Norm(byteMu, lambdaValue).BitLength;
    sbyte[] tadic = new sbyte[bitLength > 30 ? bitLength + 4 + (int) width : 34 + (int) width];
    Number number1 = pow2w.ShiftRight(1);
    Number number2 = lambdaValue.num1;
    Number number3 = lambdaValue.num2;
    int index1 = 0;
    while (!number2.Equals((object) Number.Zero) || !number3.Equals((object) Number.Zero))
    {
      if (number2.TestBit(0))
      {
        Number number4 = number2.Add(number3.Multiply(tw)).Mod(pow2w);
        sbyte index2 = number4.CompareTo(number1) < 0 ? (sbyte) number4.IntValue : (sbyte) number4.Subtract(pow2w).IntValue;
        tadic[index1] = index2;
        bool flag = true;
        if (index2 < (sbyte) 0)
        {
          flag = false;
          index2 = -index2;
        }
        if (flag)
        {
          number2 = number2.Subtract(alpha[(int) index2].num1);
          number3 = number3.Subtract(alpha[(int) index2].num2);
        }
        else
        {
          number2 = number2.Add(alpha[(int) index2].num1);
          number3 = number3.Add(alpha[(int) index2].num2);
        }
      }
      else
        tadic[index1] = (sbyte) 0;
      Number number5 = number2;
      number2 = byteMu != (sbyte) 1 ? number3.Subtract(number2.ShiftRight(1)) : number3.Add(number2.ShiftRight(1));
      number3 = number5.ShiftRight(1).Negate();
      ++index1;
    }
    return tadic;
  }

  public static Finite2MPoint[] FindComp(Finite2MPoint pointP, sbyte a)
  {
    Finite2MPoint[] comp = new Finite2MPoint[16 /*0x10*/];
    comp[1] = pointP;
    sbyte[][] numArray = a != (sbyte) 0 ? ECTanFunction.AlphaOneT : ECTanFunction.AlphaZeroT;
    int length = numArray.Length;
    for (int index = 3; index < length; index += 2)
      comp[index] = ECTanFunction.MultiplyFromTnaf(pointP, numArray[index]);
    return comp;
  }
}
