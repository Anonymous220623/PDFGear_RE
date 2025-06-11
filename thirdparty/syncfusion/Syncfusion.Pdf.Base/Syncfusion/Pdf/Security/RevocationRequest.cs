// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RevocationRequest
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RevocationRequest : Asn1Encode
{
  private CertificateIdentityHelper m_certificateID;
  private X509Extensions m_singleRequestExtensions;

  internal RevocationRequest(
    CertificateIdentityHelper certificateID,
    X509Extensions singleRequestExtensions)
  {
    this.m_certificateID = certificateID != null ? certificateID : throw new ArgumentNullException(nameof (certificateID));
    this.m_singleRequestExtensions = singleRequestExtensions;
  }

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[1]
    {
      (Asn1Encode) this.m_certificateID
    });
    if (this.m_singleRequestExtensions != null)
      collection.Add((Asn1Encode) new DerTag(true, 0, (Asn1Encode) this.m_singleRequestExtensions));
    return (Asn1) new DerSequence(collection);
  }
}
