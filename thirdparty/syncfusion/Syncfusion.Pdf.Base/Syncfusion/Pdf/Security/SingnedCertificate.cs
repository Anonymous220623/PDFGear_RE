// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SingnedCertificate
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SingnedCertificate : Asn1Encode
{
  internal Asn1Sequence m_sequence;
  internal DerInteger m_version;
  internal DerInteger m_serialNumber;
  internal Algorithms m_signature;
  internal X509Name m_issuer;
  internal X509Time m_startDate;
  internal X509Time m_endDate;
  internal X509Name m_subject;
  internal PublicKeyInformation m_publicKeyInformation;
  internal DerBitString m_issuerID;
  internal DerBitString m_subjectID;
  internal X509Extensions m_extensions;

  internal static SingnedCertificate GetCertificate(object obj)
  {
    if (obj is SingnedCertificate)
      return (SingnedCertificate) obj;
    return obj != null ? new SingnedCertificate(Asn1Sequence.GetSequence(obj)) : (SingnedCertificate) null;
  }

  internal SingnedCertificate(Asn1Sequence sequence)
  {
    int num = 0;
    this.m_sequence = sequence;
    if (sequence[0] is DerTag || sequence[0] is Asn1Tag)
    {
      this.m_version = DerInteger.GetNumber((Asn1Tag) sequence[0], true);
    }
    else
    {
      num = -1;
      this.m_version = new DerInteger(0);
    }
    this.m_serialNumber = DerInteger.GetNumber((object) sequence[num + 1]);
    this.m_signature = Algorithms.GetAlgorithms((object) sequence[num + 2]);
    this.m_issuer = X509Name.GetName((object) sequence[num + 3]);
    Asn1Sequence asn1Sequence = (Asn1Sequence) sequence[num + 4];
    this.m_startDate = X509Time.GetTime((object) asn1Sequence[0]);
    this.m_endDate = X509Time.GetTime((object) asn1Sequence[1]);
    this.m_subject = X509Name.GetName((object) sequence[num + 5]);
    this.m_publicKeyInformation = PublicKeyInformation.GetPublicKeyInformation((object) sequence[num + 6]);
    for (int index = sequence.Count - (num + 6) - 1; index > 0; --index)
    {
      Asn1Tag tag = sequence[num + 6 + index] as Asn1Tag;
      switch (tag.TagNumber)
      {
        case 1:
          this.m_issuerID = DerBitString.GetString(tag, false);
          break;
        case 2:
          this.m_subjectID = DerBitString.GetString(tag, false);
          break;
        case 3:
          this.m_extensions = X509Extensions.GetInstance((object) tag);
          break;
      }
    }
  }

  internal int Version => this.m_version.Value.IntValue + 1;

  internal DerInteger SerialNumber => this.m_serialNumber;

  internal Algorithms Signature => this.m_signature;

  internal X509Name Issuer => this.m_issuer;

  internal X509Time StartDate => this.m_startDate;

  internal X509Time EndDate => this.m_endDate;

  internal X509Name Subject => this.m_subject;

  internal PublicKeyInformation SubjectPublicKeyInfo => this.m_publicKeyInformation;

  internal DerBitString IssuerUniqueID => this.m_issuerID;

  internal DerBitString SubjectUniqueID => this.m_subjectID;

  internal X509Extensions Extensions => this.m_extensions;

  public override Asn1 GetAsn1() => (Asn1) this.m_sequence;
}
