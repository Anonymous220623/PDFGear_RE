// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RevocationListValidator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RevocationListValidator
{
  private RevocationList m_crl;
  private RevocationResult m_result;

  internal RevocationListValidator(X509Certificate signerCertificate, RevocationResult result)
  {
    this.m_crl = new RevocationList(signerCertificate);
    this.m_result = result;
  }

  internal bool Validate(
    X509Certificate signerCertificate,
    X509Certificate issuerCertificate,
    DateTime signDate)
  {
    ICollection<byte[]> encoded = this.m_crl.GetEncoded(signerCertificate, (string) null);
    DateTime now = DateTime.Now;
    foreach (byte[] input in (IEnumerable<byte[]>) encoded)
    {
      RevocationListHelper revocationListHelper = new RevocationListHelper(CertificateCollection.GetCertificateList((object) (Asn1Sequence) new Asn1Stream(input).ReadAsn1()));
      if (revocationListHelper.Validate(signerCertificate, issuerCertificate, signDate) || revocationListHelper.Validate(signerCertificate, issuerCertificate, now))
        return true;
    }
    return false;
  }
}
