// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.EllipticCurveParams
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class EllipticCurveParams
{
  internal EllipticCurves ecCurve;
  internal byte[] data;
  internal EllipticPoint pointG;
  internal Number numberX;
  internal Number numberY;

  internal EllipticCurveParams(EllipticCurves ecCurve, EllipticPoint pointG, Number numberX)
    : this(ecCurve, pointG, numberX, Number.One)
  {
  }

  internal EllipticCurveParams(
    EllipticCurves ecCurve,
    EllipticPoint pointG,
    Number numberX,
    Number numberY)
    : this(ecCurve, pointG, numberX, numberY, (byte[]) null)
  {
  }

  internal EllipticCurveParams(
    EllipticCurves ecCurve,
    EllipticPoint pointG,
    Number numberX,
    Number numberY,
    byte[] data)
  {
    if (ecCurve == null)
      throw new ArgumentNullException(nameof (ecCurve));
    if (pointG == null)
      throw new ArgumentNullException(nameof (pointG));
    if (numberX == null)
      throw new ArgumentNullException(nameof (numberX));
    if (numberY == null)
      throw new ArgumentNullException(nameof (numberY));
    this.ecCurve = ecCurve;
    this.pointG = pointG;
    this.numberX = numberX;
    this.numberY = numberY;
    this.data = Asn1Constants.Clone(data);
  }

  internal EllipticCurves Curve => this.ecCurve;

  internal EllipticPoint PointG => this.pointG;

  internal Number NumberX => this.numberX;

  internal Number NumberY => this.numberY;

  internal byte[] ECSeed() => Asn1Constants.Clone(this.data);

  public override bool Equals(object element)
  {
    if (element == this)
      return true;
    return element is EllipticCurveParams element1 && this.Equals(element1);
  }

  protected bool Equals(EllipticCurveParams element)
  {
    return this.ecCurve.Equals((object) element.ecCurve) && this.pointG.Equals((object) element.pointG) && this.numberX.Equals((object) element.numberX) && this.numberY.Equals((object) element.numberY) && Asn1Constants.AreEqual(this.data, element.data);
  }

  public override int GetHashCode()
  {
    return this.ecCurve.GetHashCode() ^ this.pointG.GetHashCode() ^ this.numberX.GetHashCode() ^ this.numberY.GetHashCode() ^ Asn1Constants.GetHashCode(this.data);
  }
}
