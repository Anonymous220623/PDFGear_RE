// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECWMultiplier
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECWMultiplier : EllipticMultiplier
{
  public sbyte[] CheckBitValue(sbyte width, Number number)
  {
    sbyte[] sourceArray = new sbyte[number.BitLength + 1];
    short num1 = (short) (1 << (int) width);
    Number m = Number.ValueOf((long) num1);
    int index = 0;
    int num2 = 0;
    while (number.SignValue > 0)
    {
      if (number.TestBit(0))
      {
        Number number1 = number.Mod(m);
        sourceArray[index] = !number1.TestBit((int) width - 1) ? (sbyte) number1.IntValue : (sbyte) (number1.IntValue - (int) num1);
        number = number.Subtract(Number.ValueOf((long) sourceArray[index]));
        num2 = index;
      }
      else
        sourceArray[index] = (sbyte) 0;
      number = number.ShiftRight(1);
      ++index;
    }
    int length = num2 + 1;
    sbyte[] destinationArray = new sbyte[length];
    Array.Copy((Array) sourceArray, 0, (Array) destinationArray, 0, length);
    return destinationArray;
  }

  public EllipticPoint Multiply(EllipticPoint pointP, Number number, EllipticComp preInfo)
  {
    EllipticWComp preInfo1 = preInfo == null || !(preInfo is EllipticWComp) ? new EllipticWComp() : (EllipticWComp) preInfo;
    int bitLength = number.BitLength;
    sbyte width;
    int length1;
    if (bitLength < 13)
    {
      width = (sbyte) 2;
      length1 = 1;
    }
    else if (bitLength < 41)
    {
      width = (sbyte) 3;
      length1 = 2;
    }
    else if (bitLength < 121)
    {
      width = (sbyte) 4;
      length1 = 4;
    }
    else if (bitLength < 337)
    {
      width = (sbyte) 5;
      length1 = 8;
    }
    else if (bitLength < 897)
    {
      width = (sbyte) 6;
      length1 = 16 /*0x10*/;
    }
    else if (bitLength < 2305)
    {
      width = (sbyte) 7;
      length1 = 32 /*0x20*/;
    }
    else
    {
      width = (sbyte) 8;
      length1 = (int) sbyte.MaxValue;
    }
    int length2 = 1;
    EllipticPoint[] ellipticPointArray = preInfo1.FindComp();
    EllipticPoint twiceP = preInfo1.FindTwice();
    if (ellipticPointArray == null)
      ellipticPointArray = new EllipticPoint[1]{ pointP };
    else
      length2 = ellipticPointArray.Length;
    if (twiceP == null)
      twiceP = pointP.Twice();
    if (length2 < length1)
    {
      EllipticPoint[] sourceArray = ellipticPointArray;
      ellipticPointArray = new EllipticPoint[length1];
      Array.Copy((Array) sourceArray, 0, (Array) ellipticPointArray, 0, length2);
      for (int index = length2; index < length1; ++index)
        ellipticPointArray[index] = twiceP.SumValue(ellipticPointArray[index - 1]);
    }
    sbyte[] numArray = this.CheckBitValue(width, number);
    int length3 = numArray.Length;
    EllipticPoint ellipticPoint = pointP.Curve.IsInfinity;
    for (int index = length3 - 1; index >= 0; --index)
    {
      ellipticPoint = ellipticPoint.Twice();
      if (numArray[index] != (sbyte) 0)
        ellipticPoint = numArray[index] <= (sbyte) 0 ? ellipticPoint.Subtract(ellipticPointArray[((int) -numArray[index] - 1) / 2]) : ellipticPoint.SumValue(ellipticPointArray[((int) numArray[index] - 1) / 2]);
    }
    preInfo1.SetComp(ellipticPointArray);
    preInfo1.TwicePoint(twiceP);
    pointP.SetInfo((EllipticComp) preInfo1);
    return ellipticPoint;
  }
}
