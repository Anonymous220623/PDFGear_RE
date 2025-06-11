// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.X509CertificateStructure
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class X509CertificateStructure : Asn1Encode
{
  private SingnedCertificate m_tbsCert;
  private Algorithms m_sigAlgID;
  private DerBitString m_sig;

  internal static X509CertificateStructure GetInstance(object obj)
  {
    if (obj is X509CertificateStructure)
      return (X509CertificateStructure) obj;
    return obj != null ? new X509CertificateStructure(Asn1Sequence.GetSequence(obj)) : (X509CertificateStructure) null;
  }

  private X509CertificateStructure(Asn1Sequence seq)
  {
    this.m_tbsCert = SingnedCertificate.GetCertificate((object) seq[0]);
    this.m_sigAlgID = Algorithms.GetAlgorithms((object) seq[1]);
    this.m_sig = DerBitString.GetString((object) seq[2]);
  }

  internal SingnedCertificate TbsCertificate => this.m_tbsCert;

  internal int Version => this.m_tbsCert.Version;

  internal DerInteger SerialNumber => this.m_tbsCert.SerialNumber;

  internal X509Name Issuer => this.m_tbsCert.Issuer;

  internal X509Time StartDate => this.m_tbsCert.StartDate;

  internal X509Time EndDate => this.m_tbsCert.EndDate;

  internal X509Name Subject => this.m_tbsCert.Subject;

  internal PublicKeyInformation SubjectPublicKeyInfo => this.m_tbsCert.SubjectPublicKeyInfo;

  internal Algorithms SignatureAlgorithm => this.m_sigAlgID;

  internal DerBitString Signature => this.m_sig;

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1Encode[3]
    {
      (Asn1Encode) this.m_tbsCert,
      (Asn1Encode) this.m_sigAlgID,
      (Asn1Encode) this.m_sig
    });
  }
}
