// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RevocationListHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RevocationListHelper
{
  private CertificateCollection m_certificateList;
  private string m_algorithm;
  private byte[] m_bytes;

  internal RevocationListHelper(CertificateCollection certificateList)
  {
    this.m_certificateList = certificateList;
    Algorithms signatureAlgorithm = certificateList.SignatureAlgorithm;
    Asn1Encode parameters = certificateList.SignatureAlgorithm.Parameters;
    if (parameters != null && !DerNull.Value.Equals((object) parameters))
    {
      if (signatureAlgorithm.ObjectID.ID == PKCSOIDs.RsaCrlAlgorithmIdntifier.ID)
      {
        Asn1Sequence sequence = Asn1Sequence.GetSequence((object) parameters);
        Algorithms algorithms = (Algorithms) null;
        for (int index = 0; index != sequence.Count; ++index)
        {
          Asn1Tag asn1Tag = (Asn1Tag) sequence[index];
          switch (asn1Tag.TagNumber)
          {
            case 0:
              algorithms = Algorithms.GetAlgorithms((object) asn1Tag);
              continue;
            case 1:
            case 2:
            case 3:
              continue;
            default:
              throw new ArgumentException("Invalid entry in sequence");
          }
        }
        string id = algorithms.ObjectID.ID;
        this.m_algorithm = (!PKCSOIDs.Sha1WithRsaEncryption.ID.Equals(id) ? (!NISTOIDs.SHA256.ID.Equals(id) ? (!NISTOIDs.SHA384.ID.Equals(id) ? (!NISTOIDs.SHA512.ID.Equals(id) ? (!NISTOIDs.RipeMD160.ID.Equals(id) ? id : "RIPEMD160") : "SHA512") : "SHA384") : "SHA256") : "SHA1") + "withRSAandMGF1";
      }
      else
        this.m_algorithm = signatureAlgorithm.ObjectID.ID;
    }
    else
      this.m_algorithm = signatureAlgorithm.ObjectID.ID;
    if (this.m_certificateList.SignatureAlgorithm.Parameters != null)
      this.m_bytes = this.m_certificateList.SignatureAlgorithm.Parameters.GetDerEncoded();
    else
      this.m_bytes = (byte[]) null;
  }

  internal bool Validate(
    X509Certificate signerCertificate,
    X509Certificate issuerCertificate,
    DateTime signDate)
  {
    return !(signDate == DateTime.MaxValue) && this.m_certificateList.Issuer.Equivalent(signerCertificate.IssuerDN) && signDate.CompareTo(this.m_certificateList.CurrentUpdate.ToDateTime()) > 0 && signDate.CompareTo(this.m_certificateList.NextUpdate.ToDateTime()) < 0 && issuerCertificate != null && this.Validate(issuerCertificate.GetPublicKey()) && this.m_certificateList.IsRevoked(signerCertificate);
  }

  private bool Validate(CipherParameter publicKey)
  {
    if (!this.m_certificateList.SignatureAlgorithm.Equals((object) this.m_certificateList.CertificateList.Signature))
      return false;
    ISigner signer = new SignerUtilities().GetSigner(this.m_algorithm);
    signer.Initialize(false, (ICipherParam) publicKey);
    byte[] derEncoded = this.m_certificateList.CertificateList.GetDerEncoded();
    signer.BlockUpdate(derEncoded, 0, derEncoded.Length);
    return signer.ValidateSignature(this.m_certificateList.Signature.GetBytes());
  }
}
