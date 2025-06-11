// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.X509RevocationResponse
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class X509RevocationResponse : X509ExtensionBase
{
  private OcspHelper m_helper;
  private ResponseInformation m_data;

  internal X509RevocationResponse(OcspHelper helper)
  {
    this.m_helper = helper;
    this.m_data = helper.ResponseInformation;
  }

  internal byte[] EncodedBytes => this.m_helper.GetEncoded();

  internal OneTimeResponse[] Responses
  {
    get
    {
      Asn1Sequence sequence = this.m_data.Sequence;
      OneTimeResponse[] responses = new OneTimeResponse[sequence.Count];
      for (int index = 0; index != responses.Length; ++index)
      {
        OneTimeResponseHelper timeResponseHelper = new OneTimeResponseHelper();
        responses[index] = new OneTimeResponse(timeResponseHelper.GetResponse((object) sequence[index]));
      }
      return responses;
    }
  }

  internal X509Certificate[] Certificates
  {
    get
    {
      IList list = (IList) new List<X509Certificate>();
      Asn1Sequence sequence = this.m_helper.Sequence;
      if (sequence != null)
      {
        foreach (Asn1Encode asn1Encode in sequence)
          list.Add((object) new X509CertificateParser().ReadCertificate(asn1Encode.GetEncoded()));
      }
      X509Certificate[] certificates = new X509Certificate[list.Count];
      for (int index = 0; index < list.Count; ++index)
        certificates[index] = (X509Certificate) list[index];
      return certificates;
    }
  }

  protected override X509Extensions GetX509Extensions() => (X509Extensions) null;

  internal bool Verify(CipherParameter publicKey)
  {
    SignerUtilities signerUtilities = new SignerUtilities();
    string algorithmName = signerUtilities.GetAlgorithmName(this.m_helper.Algorithm.ObjectID);
    ISigner signer = signerUtilities.GetSigner(algorithmName);
    signer.Initialize(false, (ICipherParam) publicKey);
    byte[] derEncoded = this.m_data.GetDerEncoded();
    signer.BlockUpdate(derEncoded, 0, derEncoded.Length);
    return signer.ValidateSignature(this.m_helper.Signature.GetBytes());
  }
}
