// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OcspRequestCreator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class OcspRequestCreator
{
  private IList m_list = (IList) new ArrayList();
  private OcspTag m_requestorName;
  private X509Extensions m_requestExtensions;

  internal void AddRequest(CertificateIdentity id)
  {
    this.m_list.Add((object) new OcspRequestCreator.RequestCreatorHelper(id, (X509Extensions) null));
  }

  internal void SetRequestExtensions(X509Extensions extensions)
  {
    this.m_requestExtensions = extensions;
  }

  private OcspRequestHelper CreateRequest(
    DerObjectID signingAlgorithm,
    CipherParameter privateKey,
    X509Certificate[] chain,
    int count)
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[0]);
    foreach (OcspRequestCreator.RequestCreatorHelper requestCreatorHelper in (IEnumerable) this.m_list)
    {
      try
      {
        collection.Add((Asn1Encode) requestCreatorHelper.ToRequest());
      }
      catch (Exception ex)
      {
        throw new Exception("Invalid request creation");
      }
    }
    return new OcspRequestHelper(new RevocationListRequest(new OcspRequestCollection(this.m_requestorName, (Asn1Sequence) new DerSequence(collection), this.m_requestExtensions)));
  }

  internal OcspRequestHelper Generate()
  {
    return this.CreateRequest((DerObjectID) null, (CipherParameter) null, (X509Certificate[]) null, 0);
  }

  private class RequestCreatorHelper
  {
    private CertificateIdentity m_id;
    private X509Extensions m_extensions;

    internal RequestCreatorHelper(CertificateIdentity id, X509Extensions extensions)
    {
      this.m_id = id;
      this.m_extensions = extensions;
    }

    internal RevocationRequest ToRequest()
    {
      return new RevocationRequest(this.m_id.ID, this.m_extensions);
    }
  }
}
