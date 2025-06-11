// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.CertificateIdentityHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class CertificateIdentityHelper : Asn1Encode
{
  private Algorithms m_hash;
  private Asn1Octet m_issuerName;
  private Asn1Octet m_issuerKey;
  private DerInteger m_serialNumber;

  internal DerInteger SerialNumber => this.m_serialNumber;

  internal CertificateIdentityHelper()
  {
  }

  internal CertificateIdentityHelper(
    Algorithms hashAlgorithm,
    Asn1Octet issuerNameHash,
    Asn1Octet issuerKeyHash,
    DerInteger serialNumber)
  {
    this.m_hash = hashAlgorithm;
    this.m_issuerName = issuerNameHash;
    this.m_issuerKey = issuerKeyHash;
    this.m_serialNumber = serialNumber;
  }

  private CertificateIdentityHelper(Asn1Sequence sequence)
  {
    this.m_hash = sequence.Count == 4 ? Algorithms.GetAlgorithms((object) sequence[0]) : throw new ArgumentException("Invalid length in sequence");
    this.m_issuerName = Asn1Octet.GetOctetString((object) sequence[1]);
    this.m_issuerKey = Asn1Octet.GetOctetString((object) sequence[2]);
    this.m_serialNumber = DerInteger.GetNumber((object) sequence[3]);
  }

  internal CertificateIdentityHelper GetCertificateIdentity(object obj)
  {
    switch (obj)
    {
      case null:
      case CertificateIdentityHelper _:
        return (CertificateIdentityHelper) obj;
      case Asn1Sequence _:
        return new CertificateIdentityHelper((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence");
    }
  }

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1Encode[4]
    {
      (Asn1Encode) this.m_hash,
      (Asn1Encode) this.m_issuerName,
      (Asn1Encode) this.m_issuerKey,
      (Asn1Encode) this.m_serialNumber
    });
  }
}
