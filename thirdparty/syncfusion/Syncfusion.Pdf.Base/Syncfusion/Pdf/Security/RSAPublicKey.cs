// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RSAPublicKey
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RSAPublicKey : Asn1Encode
{
  private Number m_modulus;
  private Number m_publicExponent;

  internal Number Modulus => this.m_modulus;

  internal Number PublicExponent => this.m_publicExponent;

  internal static RSAPublicKey GetPublicKey(object obj)
  {
    switch (obj)
    {
      case null:
      case RSAPublicKey _:
        return (RSAPublicKey) obj;
      case Asn1Sequence _:
        return new RSAPublicKey((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  internal RSAPublicKey(Number modulus, Number publicExponent)
  {
    this.m_modulus = modulus;
    this.m_publicExponent = publicExponent;
  }

  private RSAPublicKey(Asn1Sequence sequence)
  {
    this.m_modulus = DerInteger.GetNumber((object) sequence[0]).PositiveValue;
    this.m_publicExponent = DerInteger.GetNumber((object) sequence[1]).PositiveValue;
  }

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1Encode[2]
    {
      (Asn1Encode) new DerInteger(this.Modulus),
      (Asn1Encode) new DerInteger(this.PublicExponent)
    });
  }
}
