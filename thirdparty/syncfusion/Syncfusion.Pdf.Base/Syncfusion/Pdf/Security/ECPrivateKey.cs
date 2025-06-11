// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECPrivateKey
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECPrivateKey : EllipticKeyParam
{
  private readonly Number number;

  public ECPrivateKey(Number number, EllipticCurveParams parameters)
    : this("EC", number, parameters)
  {
  }

  public ECPrivateKey(string algorithm, Number number, EllipticCurveParams parameters)
    : base(algorithm, true, parameters)
  {
    this.number = number != null ? number : throw new ArgumentNullException(nameof (number));
  }

  public ECPrivateKey(string algorithm, Number number, DerObjectID publicKeySet)
    : base(algorithm, true, publicKeySet)
  {
    this.number = number != null ? number : throw new ArgumentNullException(nameof (number));
  }

  public Number Key => this.number;

  public override bool Equals(object element)
  {
    if (element == this)
      return true;
    return element is ECPrivateKey element1 && this.Equals(element1);
  }

  protected bool Equals(ECPrivateKey element)
  {
    return this.number.Equals((object) element.number) && this.Equals((EllipticKeyParam) element);
  }

  public override int GetHashCode() => this.number.GetHashCode() ^ base.GetHashCode();
}
