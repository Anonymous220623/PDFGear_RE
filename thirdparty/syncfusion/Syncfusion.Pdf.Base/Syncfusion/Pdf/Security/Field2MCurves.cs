// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Field2MCurves
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Field2MCurves : EllipticCurves
{
  private readonly int pointM;
  private readonly int elementX;
  private readonly int elementY;
  private readonly int elementZ;
  private readonly Number numberX;
  private readonly Number numberY;
  private readonly Finite2MPoint infinityPoint;
  private sbyte mPoint;
  private Number[] collection;

  public Field2MCurves(
    int pointM,
    int pointX,
    Number elementA,
    Number elementB,
    Number numberX,
    Number numberY)
    : this(pointM, pointX, 0, 0, elementA, elementB, numberX, numberY)
  {
  }

  public Field2MCurves(
    int pointM,
    int elementX,
    int elementY,
    int elementZ,
    Number elementA,
    Number elementB)
    : this(pointM, elementX, elementY, elementZ, elementA, elementB, (Number) null, (Number) null)
  {
  }

  public Field2MCurves(
    int pointM,
    int elementX,
    int elementY,
    int elementZ,
    Number elementA,
    Number elementB,
    Number numberX,
    Number numberY)
  {
    this.pointM = pointM;
    this.elementX = elementX;
    this.elementY = elementY;
    this.elementZ = elementZ;
    this.numberX = numberX;
    this.numberY = numberY;
    this.infinityPoint = new Finite2MPoint((EllipticCurves) this, (EllipticCurveElements) null, (EllipticCurveElements) null);
    if (elementX == 0)
      throw new ArgumentException("elementX must be > 0");
    if (elementY == 0)
    {
      if (elementZ != 0)
        throw new ArgumentException("elementZ must be 0 if elementY == 0");
    }
    else
    {
      if (elementY <= elementX)
        throw new ArgumentException("elementY must be > elementX");
      if (elementZ <= elementY)
        throw new ArgumentException("elementZ must be > elementY");
    }
    this.elementA = this.ECNumber(elementA);
    this.elementB = this.ECNumber(elementB);
  }

  public override EllipticPoint IsInfinity => (EllipticPoint) this.infinityPoint;

  public override int Size => this.pointM;

  public override EllipticCurveElements ECNumber(Number number)
  {
    return (EllipticCurveElements) new Finite2MFieldObject(this.pointM, this.elementX, this.elementY, this.elementZ, number);
  }

  public bool IsKOBLITZ
  {
    get
    {
      return this.numberX != null && this.numberY != null && (this.elementA.ToIntValue().Equals((object) Number.Zero) || this.elementA.ToIntValue().Equals((object) Number.One)) && this.elementB.ToIntValue().Equals((object) Number.One);
    }
  }

  internal sbyte MU()
  {
    if (this.mPoint == (sbyte) 0)
    {
      lock (this)
      {
        if (this.mPoint == (sbyte) 0)
          this.mPoint = ECTanFunction.FindMU(this);
      }
    }
    return this.mPoint;
  }

  internal Number[] SI()
  {
    if (this.collection == null)
    {
      lock (this)
      {
        if (this.collection == null)
          this.collection = ECTanFunction.FindSI(this);
      }
    }
    return this.collection;
  }

  public override EllipticPoint ECPoints(Number numberX1, Number numberY1, bool isCompress)
  {
    return (EllipticPoint) new Finite2MPoint((EllipticCurves) this, this.ECNumber(numberX1), this.ECNumber(numberY1), isCompress);
  }

  protected override EllipticPoint GetDecompressECPoint(int point, Number numberX1)
  {
    EllipticCurveElements pointX = this.ECNumber(numberX1);
    EllipticCurveElements pointY;
    if (pointX.ToIntValue().SignValue == 0)
    {
      pointY = this.elementB;
      for (int index = 0; index < this.pointM - 1; ++index)
        pointY = pointY.Square();
    }
    else
    {
      EllipticCurveElements ellipticCurveElements = this.ECEquation(pointX.SumValue(this.elementA).SumValue(this.elementB.Multiply(pointX.Square().Invert())));
      if (ellipticCurveElements == null)
        throw new ArithmeticException("Incorrect point");
      if ((ellipticCurveElements.ToIntValue().TestBit(0) ? 1 : 0) != point)
        ellipticCurveElements = ellipticCurveElements.SumValue(this.ECNumber(Number.One));
      pointY = pointX.Multiply(ellipticCurveElements);
    }
    return (EllipticPoint) new Finite2MPoint((EllipticCurves) this, pointX, pointY, true);
  }

  private EllipticCurveElements ECEquation(EllipticCurveElements betaPoint)
  {
    if (betaPoint.ToIntValue().SignValue == 0)
      return this.ECNumber(Number.Zero);
    EllipticCurveElements ellipticCurveElements1 = (EllipticCurveElements) null;
    for (EllipticCurveElements ellipticCurveElements2 = this.ECNumber(Number.Zero); ellipticCurveElements2.ToIntValue().SignValue == 0; ellipticCurveElements2 = ellipticCurveElements1.Square().SumValue(ellipticCurveElements1))
    {
      EllipticCurveElements ellipticCurveElements3 = this.ECNumber(new Number(this.pointM, new SecureRandomAlgorithm()));
      ellipticCurveElements1 = this.ECNumber(Number.Zero);
      EllipticCurveElements ellipticCurveElements4 = betaPoint;
      for (int index = 1; index <= this.pointM - 1; ++index)
      {
        EllipticCurveElements ellipticCurveElements5 = ellipticCurveElements4.Square();
        ellipticCurveElements1 = ellipticCurveElements1.Square().SumValue(ellipticCurveElements5.Multiply(ellipticCurveElements3));
        ellipticCurveElements4 = ellipticCurveElements5.SumValue(betaPoint);
      }
      if (ellipticCurveElements4.ToIntValue().SignValue != 0)
        return (EllipticCurveElements) null;
    }
    return ellipticCurveElements1;
  }

  public override bool Equals(object element)
  {
    if (element == this)
      return true;
    return element is Field2MCurves element1 && this.Equals(element1);
  }

  protected bool Equals(Field2MCurves element)
  {
    return this.pointM == element.pointM && this.elementX == element.elementX && this.elementY == element.elementY && this.elementZ == element.elementZ && this.Equals((EllipticCurves) element);
  }

  public override int GetHashCode()
  {
    return base.GetHashCode() ^ this.pointM ^ this.elementX ^ this.elementY ^ this.elementZ;
  }

  public int PointM => this.pointM;

  public int ElementX => this.elementX;

  public int ElementY => this.elementY;

  public int ElementZ => this.elementZ;

  public Number NumberY => this.numberY;
}
