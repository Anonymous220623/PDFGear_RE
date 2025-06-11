// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.TimeStampToken
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class TimeStampToken
{
  private CmsSignedDetails m_timeStampData;
  private CmsSignerDetails m_sigerDetails;
  private TimeStampIdentifier m_certIdentifier;
  private TimeStampTokenInformation m_timestampTokenInformation;

  internal TimeStampToken(CmsSignedDetails signedData)
  {
    this.m_timeStampData = signedData;
    if (signedData.SignedContentType.ID.Equals("1.2.840.113549.1.9.16.1.4"))
    {
      ICollection signers = signedData.GetSignerDetails().GetSigners();
      if (signers.Count == 1)
      {
        IEnumerator enumerator = signers.GetEnumerator();
        enumerator.MoveNext();
        this.m_sigerDetails = (CmsSignerDetails) enumerator.Current;
      }
    }
    try
    {
      CryptographicMessageSyntaxBytes signedContent = this.m_timeStampData.SignedContent;
      MemoryStream memoryStream = new MemoryStream();
      signedContent.Write((Stream) memoryStream);
      this.m_timestampTokenInformation = new TimeStampTokenInformation(new TimeStampData(Asn1.FromByteArray(memoryStream.ToArray()) as Asn1Sequence));
      TimeStampElement signedAttribute1 = this.m_sigerDetails.SignedAttributes[PKCSOIDs.Pkcs9AtSigningCertV1];
      if (signedAttribute1 != null)
      {
        this.m_certIdentifier = TimeStampIdentifier.GetTimeStampCertID((object) TimeStampCertificate.GetTimeStanpCertificate((object) signedAttribute1.Values[0]).Certificates[0]);
      }
      else
      {
        TimeStampElement signedAttribute2 = this.m_sigerDetails.SignedAttributes[PKCSOIDs.Pkcs9AtSigningCertV2];
        if (signedAttribute2 == null)
          return;
        this.m_certIdentifier = TimeStampIdentifier.GetTimeStampCertID((object) TimeStampCertificate.GetTimeStanpCertificate((object) signedAttribute2.Values[0]).Certificates[0]);
      }
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message, ex.InnerException);
    }
  }

  internal TimeStampTokenInformation TimeStampInformation => this.m_timestampTokenInformation;
}
