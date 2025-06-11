// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.CmsSignedDetails
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class CmsSignedDetails
{
  private CryptographicMessageSyntaxBytes m_content;
  private SignedDetails m_signedCms;
  private ContentInformation m_contentInformation;
  private SignerInformationCollection m_signerInformationCollection;

  internal DerObjectID SignedContentType => this.m_signedCms.ContentInformation.ContentType;

  internal CryptographicMessageSyntaxBytes SignedContent => this.m_content;

  internal CmsSignedDetails(ContentInformation sigData)
  {
    this.m_contentInformation = sigData;
    this.m_signedCms = new SignedDetails(this.m_contentInformation.Content as Asn1Sequence);
    if (this.m_signedCms.ContentInformation.Content == null)
      return;
    this.m_content = new CryptographicMessageSyntaxBytes(((Asn1Octet) this.m_signedCms.ContentInformation.Content).GetOctets());
  }

  internal SignerInformationCollection GetSignerDetails()
  {
    if (this.m_signerInformationCollection == null)
    {
      List<CmsSignerDetails> signerInfos = new List<CmsSignerDetails>();
      foreach (object obj in this.m_signedCms.SignerInformation)
      {
        SignerDetails signerDetails = SignerDetails.GetSignerDetails(obj);
        DerObjectID contentType = this.m_signedCms.ContentInformation.ContentType;
        signerInfos.Add(new CmsSignerDetails(signerDetails, contentType, this.m_content));
      }
      this.m_signerInformationCollection = new SignerInformationCollection((ICollection) signerInfos);
    }
    return this.m_signerInformationCollection;
  }
}
