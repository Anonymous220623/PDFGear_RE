// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Ocsp
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Ocsp
{
  internal Ocsp()
  {
  }

  internal X509RevocationResponse GetBasicOCSPResponse(
    X509Certificate checkCertificate,
    X509Certificate rootCertificate,
    string url)
  {
    try
    {
      OcspResponseHelper ocspResponse = this.GetOcspResponse(checkCertificate, rootCertificate, url);
      return ocspResponse == null || ocspResponse.Status != 0 ? (X509RevocationResponse) null : (X509RevocationResponse) ocspResponse.GetResponseObject();
    }
    catch
    {
    }
    return (X509RevocationResponse) null;
  }

  internal byte[] GetEncodedOcspRspnose(
    X509Certificate checkCert,
    X509Certificate rootCert,
    string url)
  {
    try
    {
      X509RevocationResponse basicOcspResponse = this.GetBasicOCSPResponse(checkCert, rootCert, url);
      if (basicOcspResponse != null)
      {
        OneTimeResponse[] responses = basicOcspResponse.Responses;
        if (responses.Length == 1)
        {
          if (responses[0].CertificateStatus == CerificateStatus.Good)
            return basicOcspResponse.EncodedBytes;
        }
      }
    }
    catch
    {
    }
    return (byte[]) null;
  }

  private OcspRequestHelper GenerateOCSPRequest(
    X509Certificate issuerCertificate,
    Number serialNumber)
  {
    CertificateIdentity id = new CertificateIdentity("1.3.14.3.2.26", issuerCertificate, serialNumber);
    OcspRequestCreator ocspRequestCreator = new OcspRequestCreator();
    ocspRequestCreator.AddRequest(id);
    ocspRequestCreator.SetRequestExtensions(new X509Extensions((IDictionary) new Dictionary<DerObjectID, X509Extension>()
    {
      [OcspConstants.OcspNonce] = new X509Extension(false, (Asn1Octet) new DerOctet(new DerOctet(PdfEncryption.CreateDocumentId()).GetEncoded()))
    }));
    return ocspRequestCreator.Generate();
  }

  private OcspResponseHelper GetOcspResponse(
    X509Certificate checkCertificate,
    X509Certificate rootCertificate,
    string url)
  {
    if (checkCertificate == null || rootCertificate == null)
      return (OcspResponseHelper) null;
    if (url == null)
      url = new CertificateUtililty().GetOcspUrl(checkCertificate);
    if (url == null)
      return (OcspResponseHelper) null;
    byte[] encoded = this.GenerateOCSPRequest(rootCertificate, checkCertificate.SerialNumber).GetEncoded();
    HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
    httpWebRequest.ContentLength = (long) encoded.Length;
    httpWebRequest.UserAgent = "syncfusion";
    httpWebRequest.ProtocolVersion = HttpVersion.Version10;
    httpWebRequest.ContentType = "application/ocsp-request";
    httpWebRequest.Accept = "application/ocsp-response";
    httpWebRequest.Method = "POST";
    Stream requestStream = httpWebRequest.GetRequestStream();
    requestStream.Write(encoded, 0, encoded.Length);
    requestStream.Close();
    HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
    Stream responseStream = response.GetResponseStream();
    OcspResponseHelper ocspResponse = new OcspResponseHelper(responseStream);
    responseStream.Close();
    response.Close();
    return ocspResponse;
  }
}
