// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SignedCertificateCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SignedCertificateCollection : Asn1Encode
{
  private Asn1Sequence m_sequence;
  private DerInteger m_version;
  private Algorithms m_signature;
  private X509Name m_issuerName;
  private X509Time m_currentUpdate;
  private X509Time m_nextUpdate;
  private Asn1Sequence m_revokedCertificates;
  private X509Extensions m_crls;

  internal X509Name Issuer => this.m_issuerName;

  internal X509Time CurrentUpdate => this.m_currentUpdate;

  internal X509Time NextUpdate => this.m_nextUpdate;

  internal Algorithms Signature => this.m_signature;

  internal SignedCertificateCollection(Asn1Sequence sequence)
  {
    if (sequence.Count < 3 || sequence.Count > 7)
      throw new ArgumentException("Invalid size in sequence");
    int index1 = 0;
    this.m_sequence = sequence;
    this.m_version = !(sequence[index1] is DerInteger) ? new DerInteger(0) : DerInteger.GetNumber((object) sequence[index1++]);
    Asn1Sequence asn1Sequence1 = sequence;
    int index2 = index1;
    int num1 = index2 + 1;
    this.m_signature = Algorithms.GetAlgorithms((object) asn1Sequence1[index2]);
    Asn1Sequence asn1Sequence2 = sequence;
    int index3 = num1;
    int num2 = index3 + 1;
    this.m_issuerName = X509Name.GetName((object) asn1Sequence2[index3]);
    Asn1Sequence asn1Sequence3 = sequence;
    int index4 = num2;
    int index5 = index4 + 1;
    this.m_currentUpdate = X509Time.GetTime((object) asn1Sequence3[index4]);
    if (index5 < sequence.Count && (sequence[index5] is DerUtcTime || sequence[index5] is GeneralizedTime || sequence[index5] is X509Time))
      this.m_nextUpdate = X509Time.GetTime((object) sequence[index5++]);
    if (index5 < sequence.Count && !(sequence[index5] is DerTag))
      this.m_revokedCertificates = Asn1Sequence.GetSequence((object) sequence[index5++]);
    if (index5 >= sequence.Count || !(sequence[index5] is DerTag))
      return;
    this.m_crls = X509Extensions.GetInstance((object) sequence[index5]);
  }

  internal static SignedCertificateCollection GetCertificateList(object obj)
  {
    switch (obj)
    {
      case SignedCertificateCollection certificateList:
        return certificateList;
      case Asn1Sequence _:
        return new SignedCertificateCollection((Asn1Sequence) obj);
      default:
        return (SignedCertificateCollection) null;
    }
  }

  internal RevocationListEntry[] GetRevokedCertificates()
  {
    if (this.m_revokedCertificates == null)
      return new RevocationListEntry[0];
    RevocationListEntry[] revokedCertificates = new RevocationListEntry[this.m_revokedCertificates.Count];
    for (int index = 0; index < revokedCertificates.Length; ++index)
      revokedCertificates[index] = new RevocationListEntry(Asn1Sequence.GetSequence((object) this.m_revokedCertificates[index]));
    return revokedCertificates;
  }

  public override Asn1 GetAsn1() => (Asn1) this.m_sequence;
}
