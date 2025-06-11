// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Finite2MPoint
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Finite2MPoint : ECPointBase
{
  internal Finite2MPoint(
    EllipticCurves curve,
    EllipticCurveElements pointX,
    EllipticCurveElements pointY)
    : this(curve, pointX, pointY, false)
  {
  }

  internal Finite2MPoint(
    EllipticCurves curve,
    EllipticCurveElements pointX,
    EllipticCurveElements pointY,
    bool isCompress)
    : base(curve, pointX, pointY, isCompress)
  {
    if (pointX != null && pointY == null || pointX == null && pointY != null)
      throw new ArgumentException("elements is null");
    if (pointX == null)
      return;
    Finite2MFieldObject.ValidateElements(this.pointX, this.pointY);
    Finite2MFieldObject.ValidateElements(this.pointX, this.curve.ElementA);
  }

  protected internal override bool BasePoint
  {
    get
    {
      return this.PointX.ToIntValue().SignValue != 0 && this.PointY.Multiply(this.PointX.Invert()).ToIntValue().TestBit(0);
    }
  }

  private static void ValidatePoints(EllipticPoint bitA, EllipticPoint value)
  {
    if (!bitA.curve.Equals((object) value.curve))
      throw new ArgumentException("Only points on the same curve can be added or subtracted");
  }

  internal override EllipticPoint SumValue(EllipticPoint value)
  {
    Finite2MPoint.ValidatePoints((EllipticPoint) this, value);
    return (EllipticPoint) this.AddSimple((Finite2MPoint) value);
  }

  internal Finite2MPoint AddSimple(Finite2MPoint value)
  {
    if (this.IsInfinity)
      return value;
    if (value.IsInfinity)
      return this;
    Finite2MFieldObject pointX1 = (Finite2MFieldObject) value.PointX;
    Finite2MFieldObject pointY1 = (Finite2MFieldObject) value.PointY;
    if (this.pointX.Equals((object) pointX1))
      return this.pointY.Equals((object) pointY1) ? (Finite2MPoint) this.Twice() : (Finite2MPoint) this.curve.IsInfinity;
    EllipticCurveElements ellipticCurveElements = this.pointX.SumValue((EllipticCurveElements) pointX1);
    Finite2MFieldObject finite2MfieldObject = (Finite2MFieldObject) this.pointY.SumValue((EllipticCurveElements) pointY1).Divide(ellipticCurveElements);
    Finite2MFieldObject pointX2 = (Finite2MFieldObject) finite2MfieldObject.Square().SumValue((EllipticCurveElements) finite2MfieldObject).SumValue(ellipticCurveElements).SumValue(this.curve.ElementA);
    Finite2MFieldObject pointY2 = (Finite2MFieldObject) finite2MfieldObject.Multiply(this.pointX.SumValue((EllipticCurveElements) pointX2)).SumValue((EllipticCurveElements) pointX2).SumValue(this.pointY);
    return new Finite2MPoint(this.curve, (EllipticCurveElements) pointX2, (EllipticCurveElements) pointY2, this.isCompress);
  }

  internal override EllipticPoint Subtract(EllipticPoint value)
  {
    Finite2MPoint.ValidatePoints((EllipticPoint) this, value);
    return (EllipticPoint) this.SubtractSimple((Finite2MPoint) value);
  }

  internal Finite2MPoint SubtractSimple(Finite2MPoint value)
  {
    return value.IsInfinity ? this : this.AddSimple((Finite2MPoint) value.Negate());
  }

  internal override EllipticPoint Twice()
  {
    if (this.IsInfinity)
      return (EllipticPoint) this;
    if (this.pointX.ToIntValue().SignValue == 0)
      return this.curve.IsInfinity;
    Finite2MFieldObject finite2MfieldObject = (Finite2MFieldObject) this.pointX.SumValue(this.pointY.Divide(this.pointX));
    Finite2MFieldObject pointX = (Finite2MFieldObject) finite2MfieldObject.Square().SumValue((EllipticCurveElements) finite2MfieldObject).SumValue(this.curve.ElementA);
    EllipticCurveElements ellipticCurveElements = this.curve.ECNumber(Number.One);
    Finite2MFieldObject pointY = (Finite2MFieldObject) this.pointX.Square().SumValue(pointX.Multiply(finite2MfieldObject.SumValue(ellipticCurveElements)));
    return (EllipticPoint) new Finite2MPoint(this.curve, (EllipticCurveElements) pointX, (EllipticCurveElements) pointY, this.isCompress);
  }

  internal override EllipticPoint Negate()
  {
    return (EllipticPoint) new Finite2MPoint(this.curve, this.pointX, this.pointX.SumValue(this.pointY), this.isCompress);
  }

  internal override void CheckMultiplier()
  {
    if (this.multiplier != null)
      return;
    lock (this)
    {
      if (this.multiplier != null)
        return;
      if (((Field2MCurves) this.curve).IsKOBLITZ)
        this.multiplier = (EllipticMultiplier) new EllipticTNMuliplier();
      else
        this.multiplier = (EllipticMultiplier) new ECWMultiplier();
    }
  }
}
