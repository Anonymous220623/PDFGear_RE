// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.FiniteCurves
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class FiniteCurves : EllipticCurves
{
  private readonly Number number;
  private readonly FinitePoint infinityPoint;

  public FiniteCurves(Number number, Number elementA, Number elementB)
  {
    this.number = number;
    this.elementA = this.ECNumber(elementA);
    this.elementB = this.ECNumber(elementB);
    this.infinityPoint = new FinitePoint((EllipticCurves) this, (EllipticCurveElements) null, (EllipticCurveElements) null);
  }

  public Number PointQ => this.number;

  public override EllipticPoint IsInfinity => (EllipticPoint) this.infinityPoint;

  public override int Size => this.number.BitLength;

  public override EllipticCurveElements ECNumber(Number num)
  {
    return (EllipticCurveElements) new FinitePFieldObject(this.number, num);
  }

  public override EllipticPoint ECPoints(Number numberX1, Number numberY1, bool isCompress)
  {
    return (EllipticPoint) new FinitePoint((EllipticCurves) this, this.ECNumber(numberX1), this.ECNumber(numberY1), isCompress);
  }

  protected override EllipticPoint GetDecompressECPoint(int point, Number numberX1)
  {
    EllipticCurveElements pointX = this.ECNumber(numberX1);
    EllipticCurveElements pointY = pointX.Multiply(pointX.Square().SumValue(this.elementA)).SumValue(this.elementB).SquareRoot();
    Number n = pointY != null ? pointY.ToIntValue() : throw new ArithmeticException("point is empty");
    if ((n.TestBit(0) ? 1 : 0) != point)
      pointY = this.ECNumber(this.number.Subtract(n));
    return (EllipticPoint) new FinitePoint((EllipticCurves) this, pointX, pointY, true);
  }

  public override bool Equals(object element)
  {
    if (element == this)
      return true;
    return element is FiniteCurves element1 && this.Equals(element1);
  }

  protected bool Equals(FiniteCurves element)
  {
    return this.Equals((EllipticCurves) element) && this.number.Equals((object) element.number);
  }

  public override int GetHashCode() => base.GetHashCode() ^ this.number.GetHashCode();
}
