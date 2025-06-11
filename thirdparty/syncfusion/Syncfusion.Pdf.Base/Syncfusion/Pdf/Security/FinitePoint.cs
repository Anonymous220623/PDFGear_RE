// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.FinitePoint
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class FinitePoint : ECPointBase
{
  internal FinitePoint(
    EllipticCurves curve,
    EllipticCurveElements pointX,
    EllipticCurveElements pointY)
    : this(curve, pointX, pointY, false)
  {
  }

  internal FinitePoint(
    EllipticCurves curve,
    EllipticCurveElements pointX,
    EllipticCurveElements pointY,
    bool isCompress)
    : base(curve, pointX, pointY, isCompress)
  {
    if (pointX == null != (pointY == null))
      throw new ArgumentException("field elements is null");
  }

  protected internal override bool BasePoint => this.PointY.ToIntValue().TestBit(0);

  internal override EllipticPoint SumValue(EllipticPoint value)
  {
    if (this.IsInfinity)
      return value;
    if (value.IsInfinity)
      return (EllipticPoint) this;
    if (this.pointX.Equals((object) value.pointX))
      return this.pointY.Equals((object) value.pointY) ? this.Twice() : this.curve.IsInfinity;
    EllipticCurveElements ellipticCurveElements = value.pointY.Subtract(this.pointY).Divide(value.pointX.Subtract(this.pointX));
    EllipticCurveElements pointX = ellipticCurveElements.Square().Subtract(this.pointX).Subtract(value.pointX);
    EllipticCurveElements pointY = ellipticCurveElements.Multiply(this.pointX.Subtract(pointX)).Subtract(this.pointY);
    return (EllipticPoint) new FinitePoint(this.curve, pointX, pointY, this.isCompress);
  }

  internal override EllipticPoint Twice()
  {
    if (this.IsInfinity)
      return (EllipticPoint) this;
    if (this.pointY.ToIntValue().SignValue == 0)
      return this.curve.IsInfinity;
    EllipticCurveElements ellipticCurveElements1 = this.curve.ECNumber(Number.Two);
    EllipticCurveElements ellipticCurveElements2 = this.curve.ECNumber(Number.Three);
    EllipticCurveElements ellipticCurveElements3 = this.pointX.Square().Multiply(ellipticCurveElements2).SumValue(this.curve.elementA).Divide(this.pointY.Multiply(ellipticCurveElements1));
    EllipticCurveElements pointX = ellipticCurveElements3.Square().Subtract(this.pointX.Multiply(ellipticCurveElements1));
    EllipticCurveElements pointY = ellipticCurveElements3.Multiply(this.pointX.Subtract(pointX)).Subtract(this.pointY);
    return (EllipticPoint) new FinitePoint(this.curve, pointX, pointY, this.isCompress);
  }

  internal override EllipticPoint Subtract(EllipticPoint value)
  {
    return value.IsInfinity ? (EllipticPoint) this : this.SumValue(value.Negate());
  }

  internal override EllipticPoint Negate()
  {
    return (EllipticPoint) new FinitePoint(this.curve, this.pointX, this.pointY.Negate(), this.isCompress);
  }

  internal override void CheckMultiplier()
  {
    if (this.multiplier != null)
      return;
    lock (this)
    {
      if (this.multiplier != null)
        return;
      this.multiplier = (EllipticMultiplier) new ECWMultiplier();
    }
  }
}
