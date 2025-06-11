// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RsaPrivateKeyParam
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RsaPrivateKeyParam : RsaKeyParam
{
  private Number m_publicExponent;
  private Number m_p;
  private Number m_q;
  private Number m_dP;
  private Number m_dQ;
  private Number m_inverse;

  internal RsaPrivateKeyParam(
    Number modulus,
    Number publicExponent,
    Number privateExponent,
    Number p,
    Number q,
    Number dP,
    Number dQ,
    Number inverse)
    : base(true, modulus, privateExponent)
  {
    RsaPrivateKeyParam.ValidateValue(publicExponent);
    RsaPrivateKeyParam.ValidateValue(p);
    RsaPrivateKeyParam.ValidateValue(q);
    RsaPrivateKeyParam.ValidateValue(dP);
    RsaPrivateKeyParam.ValidateValue(dQ);
    RsaPrivateKeyParam.ValidateValue(inverse);
    this.m_publicExponent = publicExponent;
    this.m_p = p;
    this.m_q = q;
    this.m_dP = dP;
    this.m_dQ = dQ;
    this.m_inverse = inverse;
  }

  internal Number PublicExponent => this.m_publicExponent;

  internal Number P => this.m_p;

  internal Number Q => this.m_q;

  internal Number DP => this.m_dP;

  internal Number DQ => this.m_dQ;

  internal Number QInv => this.m_inverse;

  public override bool Equals(object obj)
  {
    if (obj == this)
      return true;
    return obj is RsaPrivateKeyParam rsaPrivateKeyParam && rsaPrivateKeyParam.DP.Equals((object) this.m_dP) && rsaPrivateKeyParam.DQ.Equals((object) this.m_dQ) && rsaPrivateKeyParam.Exponent.Equals((object) this.Exponent) && rsaPrivateKeyParam.Modulus.Equals((object) this.Modulus) && rsaPrivateKeyParam.P.Equals((object) this.m_p) && rsaPrivateKeyParam.Q.Equals((object) this.m_q) && rsaPrivateKeyParam.PublicExponent.Equals((object) this.m_publicExponent) && rsaPrivateKeyParam.QInv.Equals((object) this.m_inverse);
  }

  public override int GetHashCode()
  {
    return this.DP.GetHashCode() ^ this.DQ.GetHashCode() ^ this.Exponent.GetHashCode() ^ this.Modulus.GetHashCode() ^ this.P.GetHashCode() ^ this.Q.GetHashCode() ^ this.PublicExponent.GetHashCode() ^ this.QInv.GetHashCode();
  }

  private static void ValidateValue(Number number)
  {
    if (number == null)
      throw new ArgumentNullException(nameof (number));
    if (number.SignValue <= 0)
      throw new ArgumentException("Invalid RSA entry");
  }
}
