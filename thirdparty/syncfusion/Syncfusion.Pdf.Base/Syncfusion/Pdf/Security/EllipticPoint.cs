// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.EllipticPoint
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class EllipticPoint
{
  internal readonly EllipticCurves curve;
  internal readonly EllipticCurveElements pointX;
  internal readonly EllipticCurveElements pointY;
  internal readonly bool isCompress;
  internal EllipticMultiplier multiplier;
  internal EllipticComp preInfo;

  protected internal EllipticPoint(
    EllipticCurves curve,
    EllipticCurveElements pointX,
    EllipticCurveElements pointY,
    bool isCompress)
  {
    this.curve = curve != null ? curve : throw new ArgumentNullException(nameof (curve));
    this.pointX = pointX;
    this.pointY = pointY;
    this.isCompress = isCompress;
  }

  internal EllipticCurves Curve => this.curve;

  internal EllipticCurveElements PointX => this.pointX;

  internal EllipticCurveElements PointY => this.pointY;

  internal bool IsInfinity => this.pointX == null && this.pointY == null;

  internal bool IsCompressed => this.isCompress;

  public override bool Equals(object obj)
  {
    if (obj == this)
      return true;
    if (!(obj is EllipticPoint ellipticPoint))
      return false;
    if (this.IsInfinity)
      return ellipticPoint.IsInfinity;
    return this.pointX.Equals((object) ellipticPoint.pointX) && this.pointY.Equals((object) ellipticPoint.pointY);
  }

  public override int GetHashCode()
  {
    return this.IsInfinity ? 0 : this.pointX.GetHashCode() ^ this.pointY.GetHashCode();
  }

  internal void SetInfo(EllipticComp preInfo) => this.preInfo = preInfo;

  internal virtual byte[] Encoded() => this.Encoded(this.isCompress);

  internal abstract byte[] Encoded(bool compressed);

  internal abstract EllipticPoint SumValue(EllipticPoint value);

  internal abstract EllipticPoint Subtract(EllipticPoint value);

  internal abstract EllipticPoint Negate();

  internal abstract EllipticPoint Twice();

  internal abstract EllipticPoint Multiply(Number value);

  internal virtual void CheckMultiplier()
  {
    if (this.multiplier != null)
      return;
    lock (this)
    {
      if (this.multiplier != null)
        return;
      this.multiplier = (EllipticMultiplier) new FiniteFieldMulipler();
    }
  }
}
