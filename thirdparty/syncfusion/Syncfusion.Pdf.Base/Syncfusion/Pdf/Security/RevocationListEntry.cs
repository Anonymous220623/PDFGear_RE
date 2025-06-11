// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RevocationListEntry
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RevocationListEntry : Asn1Encode
{
  internal Asn1Sequence m_sequence;
  internal DerInteger m_userCertificate;
  internal X509Time m_revocationDate;
  internal X509Extensions m_crlEntryExtensions;

  internal RevocationListEntry(Asn1Sequence sequence)
  {
    if (sequence.Count < 2 || sequence.Count > 3)
      return;
    this.m_sequence = sequence;
    this.m_userCertificate = DerInteger.GetNumber((object) sequence[0]);
    this.m_revocationDate = X509Time.GetTime((object) sequence[1]);
  }

  internal DerInteger UserCertificate => this.m_userCertificate;

  public override Asn1 GetAsn1() => (Asn1) this.m_sequence;
}
