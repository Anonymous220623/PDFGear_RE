// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.CertificateIdentity
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class CertificateIdentity
{
  internal const string Sha1 = "1.3.14.3.2.26";
  private readonly CertificateIdentityHelper m_id;

  internal CertificateIdentity(
    string hashAlgorithm,
    X509Certificate issuerCert,
    Number serialNumber)
  {
    Algorithms hashAlgorithm1 = new Algorithms(new DerObjectID(hashAlgorithm), (Asn1Encode) DerNull.Value);
    try
    {
      string id = hashAlgorithm1.ObjectID.ID;
      X509Name subject = SingnedCertificate.GetCertificate((object) Asn1.FromByteArray(issuerCert.GetTbsCertificate())).Subject;
      byte[] digest1 = new MessageDigestFinder().CalculateDigest(id, subject.GetEncoded());
      PublicKeyInformation subjectKeyId = SubjectKeyID.CreateSubjectKeyID(issuerCert.GetPublicKey());
      byte[] digest2 = new MessageDigestFinder().CalculateDigest(id, subjectKeyId.PublicKey.GetBytes());
      this.m_id = new CertificateIdentityHelper(hashAlgorithm1, (Asn1Octet) new DerOctet(digest1), (Asn1Octet) new DerOctet(digest2), new DerInteger(serialNumber));
    }
    catch (Exception ex)
    {
      throw new Exception("Invalid certificate ID");
    }
  }

  internal CertificateIdentityHelper ID => this.m_id;
}
