// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.CertificateCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class CertificateCollection : Asn1Encode
{
  private readonly SignedCertificateCollection m_certificates;
  private readonly Algorithms m_id;
  private readonly DerBitString m_signature;

  private CertificateCollection(Asn1Sequence sequence)
  {
    this.m_certificates = sequence.Count == 3 ? SignedCertificateCollection.GetCertificateList((object) sequence[0]) : throw new ArgumentException("Invalid size in sequence");
    this.m_id = Algorithms.GetAlgorithms((object) sequence[1]);
    this.m_signature = DerBitString.GetString((object) sequence[2]);
  }

  internal Algorithms SignatureAlgorithm => this.m_id;

  internal DerBitString Signature => this.m_signature;

  internal X509Time CurrentUpdate => this.m_certificates.CurrentUpdate;

  internal X509Time NextUpdate => this.m_certificates.NextUpdate;

  internal X509Name Issuer => this.m_certificates.Issuer;

  internal SignedCertificateCollection CertificateList => this.m_certificates;

  internal static CertificateCollection GetCertificateList(object obj)
  {
    if (obj is CertificateCollection)
      return (CertificateCollection) obj;
    return obj != null ? new CertificateCollection(Asn1Sequence.GetSequence(obj)) : (CertificateCollection) null;
  }

  internal bool IsRevoked(X509Certificate certificate)
  {
    RevocationListEntry[] revokedCertificates = this.m_certificates.GetRevokedCertificates();
    if (revokedCertificates != null)
    {
      Number serialNumber = certificate.SerialNumber;
      for (int index = 0; index < revokedCertificates.Length; ++index)
      {
        if (revokedCertificates[index].UserCertificate != null && revokedCertificates[index].UserCertificate.Value.Equals((object) serialNumber))
          return true;
      }
    }
    return false;
  }

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1Encode[3]
    {
      (Asn1Encode) this.m_certificates,
      (Asn1Encode) this.m_id,
      (Asn1Encode) this.m_signature
    });
  }
}
