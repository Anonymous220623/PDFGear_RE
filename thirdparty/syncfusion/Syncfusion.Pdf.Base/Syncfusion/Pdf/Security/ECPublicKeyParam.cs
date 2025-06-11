// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECPublicKeyParam
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECPublicKeyParam : EllipticKeyParam
{
  private readonly EllipticPoint pointQ;

  public ECPublicKeyParam(EllipticPoint pointQ, EllipticCurveParams parameters)
    : this("EC", pointQ, parameters)
  {
  }

  public ECPublicKeyParam(string algorithm, EllipticPoint pointQ, EllipticCurveParams parameters)
    : base(algorithm, false, parameters)
  {
    this.pointQ = pointQ != null ? pointQ : throw new ArgumentNullException(nameof (pointQ));
  }

  public ECPublicKeyParam(string algorithm, EllipticPoint pointQ, DerObjectID publicKeyParamSet)
    : base(algorithm, false, publicKeyParamSet)
  {
    this.pointQ = pointQ != null ? pointQ : throw new ArgumentNullException(nameof (pointQ));
  }

  public EllipticPoint PointQ => this.pointQ;

  public override bool Equals(object element)
  {
    if (element == this)
      return true;
    return element is ECPublicKeyParam element1 && this.Equals(element1);
  }

  protected bool Equals(ECPublicKeyParam element)
  {
    return this.pointQ.Equals((object) element.pointQ) && this.Equals((EllipticKeyParam) element);
  }

  public override int GetHashCode() => this.pointQ.GetHashCode() ^ base.GetHashCode();
}
