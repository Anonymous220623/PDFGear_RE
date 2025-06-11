// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RSAKey
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RSAKey : Asn1Encode
{
  private Number m_modulus;
  private Number m_publicExponent;
  private Number m_privateExponent;
  private Number m_prime1;
  private Number m_prime2;
  private Number m_exponent1;
  private Number m_exponent2;
  private Number m_coefficient;

  internal RSAKey(
    Number modulus,
    Number publicExponent,
    Number privateExponent,
    Number prime1,
    Number prime2,
    Number exponent1,
    Number exponent2,
    Number coefficient)
  {
    this.m_modulus = modulus;
    this.m_publicExponent = publicExponent;
    this.m_privateExponent = privateExponent;
    this.m_prime1 = prime1;
    this.m_prime2 = prime2;
    this.m_exponent1 = exponent1;
    this.m_exponent2 = exponent2;
    this.m_coefficient = coefficient;
  }

  public RSAKey(Asn1Sequence sequence)
  {
    if (((DerInteger) sequence[0]).Value.IntValue != 0)
      throw new ArgumentException("Invalid RSA key");
    this.m_modulus = ((DerInteger) sequence[1]).Value;
    this.m_publicExponent = ((DerInteger) sequence[2]).Value;
    this.m_privateExponent = ((DerInteger) sequence[3]).Value;
    this.m_prime1 = ((DerInteger) sequence[4]).Value;
    this.m_prime2 = ((DerInteger) sequence[5]).Value;
    this.m_exponent1 = ((DerInteger) sequence[6]).Value;
    this.m_exponent2 = ((DerInteger) sequence[7]).Value;
    this.m_coefficient = ((DerInteger) sequence[8]).Value;
  }

  internal Number Modulus => this.m_modulus;

  internal Number PublicExponent => this.m_publicExponent;

  internal Number PrivateExponent => this.m_privateExponent;

  internal Number Prime1 => this.m_prime1;

  internal Number Prime2 => this.m_prime2;

  internal Number Exponent1 => this.m_exponent1;

  internal Number Exponent2 => this.m_exponent2;

  internal Number Coefficient => this.m_coefficient;

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1Encode[9]
    {
      (Asn1Encode) new DerInteger(0),
      (Asn1Encode) new DerInteger(this.Modulus),
      (Asn1Encode) new DerInteger(this.PublicExponent),
      (Asn1Encode) new DerInteger(this.PrivateExponent),
      (Asn1Encode) new DerInteger(this.Prime1),
      (Asn1Encode) new DerInteger(this.Prime2),
      (Asn1Encode) new DerInteger(this.Exponent1),
      (Asn1Encode) new DerInteger(this.Exponent2),
      (Asn1Encode) new DerInteger(this.Coefficient)
    });
  }
}
