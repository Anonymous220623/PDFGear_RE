// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OneTimeResponseHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class OneTimeResponseHelper : Asn1Encode
{
  private CertificateIdentityHelper m_id;
  private OcspStatus m_certificateStatus;
  private GeneralizedTime m_currentUpdate;
  private GeneralizedTime m_nextUpdate;
  private X509Extensions m_extensions;

  internal CertificateIdentityHelper CertificateIdentification => this.m_id;

  internal GeneralizedTime NextUpdate => this.m_nextUpdate;

  internal GeneralizedTime CurrentUpdate => this.m_currentUpdate;

  public OneTimeResponseHelper(Asn1Sequence sequence)
  {
    CertificateIdentityHelper certificateIdentityHelper = new CertificateIdentityHelper();
    OcspStatus ocspStatus = new OcspStatus();
    this.m_id = certificateIdentityHelper.GetCertificateIdentity((object) sequence[0]);
    this.m_certificateStatus = ocspStatus.GetStatus((object) sequence[1]);
    this.m_currentUpdate = (GeneralizedTime) sequence[2];
    if (sequence.Count > 4)
    {
      this.m_nextUpdate = GeneralizedTime.GetGeneralizedTime((Asn1Tag) sequence[3], true);
      this.m_extensions = X509Extensions.GetInstance((Asn1Tag) sequence[4], true);
    }
    else
    {
      if (sequence.Count <= 3)
        return;
      Asn1Tag tag = (Asn1Tag) sequence[3];
      if (tag.m_tagNumber == 0)
        this.m_nextUpdate = GeneralizedTime.GetGeneralizedTime(tag, true);
      else
        this.m_extensions = X509Extensions.GetInstance(tag, true);
    }
  }

  internal OneTimeResponseHelper()
  {
  }

  internal OneTimeResponseHelper GetResponse(object obj)
  {
    switch (obj)
    {
      case null:
      case OneTimeResponseHelper _:
        return (OneTimeResponseHelper) obj;
      case Asn1Sequence _:
        return new OneTimeResponseHelper((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence");
    }
  }

  internal OcspStatus Status => this.m_certificateStatus;

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[3]
    {
      (Asn1Encode) this.m_id,
      (Asn1Encode) this.m_certificateStatus,
      (Asn1Encode) this.m_currentUpdate
    });
    if (this.m_nextUpdate != null)
      collection.Add((Asn1Encode) new DerTag(true, 0, (Asn1Encode) this.m_nextUpdate));
    if (this.m_extensions != null)
      collection.Add((Asn1Encode) new DerTag(true, 1, (Asn1Encode) this.m_extensions));
    return (Asn1) new DerSequence(collection);
  }
}
