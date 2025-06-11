// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OcspValidator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class OcspValidator
{
  private List<X509RevocationResponse> m_responseCollection;
  internal RevocationStatus RevocationStatus;
  internal bool m_ocspResposne;

  internal OcspValidator(List<X509RevocationResponse> responses)
  {
    this.m_responseCollection = responses;
  }

  internal RevocationStatus Validate(
    X509Certificate signerCertificate,
    X509Certificate issuerCertificate,
    DateTime signDate)
  {
    DateTime now = DateTime.Now;
    RevocationStatus status = RevocationStatus.None;
    if (this.m_responseCollection != null)
    {
      foreach (X509RevocationResponse response in this.m_responseCollection)
      {
        status = this.Validate(response, signerCertificate, issuerCertificate, signDate);
        this.SetStatus(status);
        status = this.Validate(response, signerCertificate, issuerCertificate, now);
        this.SetStatus(status);
      }
    }
    if (this.RevocationStatus != RevocationStatus.Good)
    {
      X509RevocationResponse ocspResponse = this.GetOcspResponse(signerCertificate, issuerCertificate);
      this.RevocationStatus = this.Validate(ocspResponse, signerCertificate, issuerCertificate, signDate);
      this.SetStatus(status);
      this.RevocationStatus = this.Validate(ocspResponse, signerCertificate, issuerCertificate, now);
      this.SetStatus(status);
    }
    return this.RevocationStatus;
  }

  private void SetStatus(RevocationStatus status)
  {
    if (status == RevocationStatus.None)
      return;
    if (status == RevocationStatus.Good)
      this.RevocationStatus = status;
    else if (status == RevocationStatus.Unknown && this.RevocationStatus != RevocationStatus.Good)
    {
      this.RevocationStatus = status;
    }
    else
    {
      if (status != RevocationStatus.Revoked || this.RevocationStatus == RevocationStatus.Good || this.RevocationStatus == RevocationStatus.Unknown)
        return;
      this.RevocationStatus = status;
    }
  }

  private RevocationStatus Validate(
    X509RevocationResponse response,
    X509Certificate signCertificate,
    X509Certificate issuerCertificate,
    DateTime signDate)
  {
    if (response == null)
      return RevocationStatus.None;
    OneTimeResponse[] responses = response.Responses;
    for (int index = 0; index < responses.Length; ++index)
    {
      if (signCertificate.SerialNumber.IntValue == responses[index].CertificateID.SerialNumber.Value.IntValue)
      {
        GeneralizedTime nextUpdate = responses[index].NextUpdate;
        DateTime dateTime = nextUpdate != null ? nextUpdate.ToDateTime() : responses[index].CurrentUpdate.ToDateTime().AddSeconds(180.0);
        if (signDate > dateTime)
        {
          if (index == responses.Length - 1)
            this.m_ocspResposne = true;
        }
        else
        {
          object certificateStatus = responses[index].CertificateStatus;
          if (certificateStatus == null || certificateStatus != null && (certificateStatus as OcspStatus).TagNumber == 0)
          {
            this.CheckResponse(response, issuerCertificate);
            return RevocationStatus.Good;
          }
          if (certificateStatus != null)
            return (certificateStatus as OcspStatus).TagNumber == 1 ? RevocationStatus.Revoked : RevocationStatus.Unknown;
        }
      }
    }
    return RevocationStatus.None;
  }

  private void CheckResponse(X509RevocationResponse ocspResp, X509Certificate issuerCert)
  {
    X509Certificate x509Certificate = (X509Certificate) null;
    if (this.CheckSignatureValidity(ocspResp, issuerCert))
      x509Certificate = issuerCert;
    if (x509Certificate == null)
    {
      foreach (X509Certificate certificate in ocspResp.Certificates)
      {
        X509Certificate responderCert;
        try
        {
          responderCert = certificate;
        }
        catch (Exception ex)
        {
          continue;
        }
        try
        {
          IList extendedKeyUsage = responderCert.GetExtendedKeyUsage();
          if (extendedKeyUsage != null)
          {
            if (extendedKeyUsage.Contains((object) "1.3.6.1.5.5.7.3.9"))
            {
              if (this.CheckSignatureValidity(ocspResp, responderCert))
              {
                x509Certificate = responderCert;
                break;
              }
            }
          }
        }
        catch (Exception ex)
        {
        }
      }
      if (x509Certificate == null)
        throw new Exception("OCSP response could not be verified");
    }
    x509Certificate.Verify(issuerCert.GetPublicKey());
    x509Certificate.CheckValidity();
  }

  private bool CheckSignatureValidity(
    X509RevocationResponse ocspResp,
    X509Certificate responderCert)
  {
    try
    {
      return ocspResp.Verify(responderCert.GetPublicKey());
    }
    catch (Exception ex)
    {
      return false;
    }
  }

  private X509RevocationResponse GetOcspResponse(
    X509Certificate signCert,
    X509Certificate issuerCert)
  {
    if (signCert == null && issuerCert == null)
      return (X509RevocationResponse) null;
    X509RevocationResponse basicOcspResponse = new Ocsp().GetBasicOCSPResponse(signCert, issuerCert, (string) null);
    if (basicOcspResponse == null)
      return (X509RevocationResponse) null;
    foreach (OneTimeResponse response in basicOcspResponse.Responses)
    {
      if (response.CertificateStatus == null)
        return basicOcspResponse;
    }
    return (X509RevocationResponse) null;
  }
}
