// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.CmsSignerDetails
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class CmsSignerDetails
{
  private SignerId m_id;
  private SignerDetails m_details;
  private Algorithms m_digestAlgorithm;
  private Algorithms m_encryptionAlgorithm;
  private readonly Asn1Set m_signedSet;
  private readonly Asn1Set m_unsignedSet;
  private CryptographicMessageSyntaxBytes m_bytes;
  private byte[] m_signatureData;
  private DerObjectID m_contentType;
  private TimeStampElements m_signedTable;

  internal SignerId ID => this.m_id;

  internal TimeStampElements SignedAttributes
  {
    get
    {
      if (this.m_signedSet != null && this.m_signedTable == null)
        this.m_signedTable = new TimeStampElements(this.m_signedSet);
      return this.m_signedTable;
    }
  }

  internal CmsSignerDetails(
    SignerDetails information,
    DerObjectID contentType,
    CryptographicMessageSyntaxBytes content)
  {
    this.m_details = information;
    this.m_id = new SignerId();
    this.m_contentType = contentType;
    try
    {
      SignerIdentity id = information.ID;
      if (id.IsTagged)
      {
        this.m_id.KeyIdentifier = Asn1Octet.GetOctetString((object) id.ID).GetEncoded();
      }
      else
      {
        CertificateInformation certificateInformation = CertificateInformation.GetCertificateInformation((object) id.ID);
        this.m_id.Issuer = certificateInformation.Name;
        this.m_id.SerialNumber = certificateInformation.SerialNumber.Value;
      }
    }
    catch (Exception ex)
    {
      throw new ArgumentException("Invalid entry in signer details");
    }
    this.m_digestAlgorithm = information.DigestAlgorithm;
    this.m_signedSet = information.Attributes;
    this.m_unsignedSet = information.Elements;
    this.m_encryptionAlgorithm = information.EncryptionAlgorithm;
    this.m_signatureData = information.EncryptedOctet.GetOctets();
    this.m_bytes = content;
  }
}
