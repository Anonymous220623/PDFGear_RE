// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.CertificateInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class CertificateInformation : Asn1Encode
{
  private X509Name m_name;
  private DerInteger m_serialNumber;

  internal static CertificateInformation GetCertificateInformation(object obj)
  {
    if (obj == null)
      return (CertificateInformation) null;
    return obj is CertificateInformation certificateInformation ? certificateInformation : new CertificateInformation(Asn1Sequence.GetSequence(obj));
  }

  internal CertificateInformation(Asn1Sequence sequence)
  {
    this.m_name = X509Name.GetName((object) sequence[0]);
    this.m_serialNumber = (DerInteger) sequence[1];
  }

  internal X509Name Name => this.m_name;

  internal DerInteger SerialNumber => this.m_serialNumber;

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1Encode[2]
    {
      (Asn1Encode) this.m_name,
      (Asn1Encode) this.m_serialNumber
    });
  }
}
