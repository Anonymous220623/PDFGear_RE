// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.TimeStampCertificate
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class TimeStampCertificate : Asn1Encode
{
  private Asn1Sequence m_certificates;
  private Asn1Sequence m_policies;

  internal static TimeStampCertificate GetTimeStanpCertificate(object obj)
  {
    switch (obj)
    {
      case null:
      case TimeStampCertificate _:
        return (TimeStampCertificate) obj;
      case Asn1Sequence _:
        return new TimeStampCertificate((Asn1Sequence) obj);
      default:
        throw new ArgumentException($"Invalid entry in sequence : {obj.GetType().Name}.");
    }
  }

  internal TimeStampCertificate(Asn1Sequence sequence)
  {
    this.m_certificates = sequence.Count >= 1 && sequence.Count <= 2 ? Asn1Sequence.GetSequence((object) sequence[0]) : throw new ArgumentException("Invalid sequence size " + (object) sequence.Count);
    if (sequence.Count <= 1)
      return;
    this.m_policies = Asn1Sequence.GetSequence((object) sequence[1]);
  }

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[1]
    {
      (Asn1Encode) this.m_certificates
    });
    if (this.m_policies != null)
      collection.Add((Asn1Encode) this.m_policies);
    return (Asn1) new DerSequence(collection);
  }

  internal TimeStampIdentifier[] Certificates
  {
    get
    {
      TimeStampIdentifier[] certificates = new TimeStampIdentifier[this.m_certificates.Count];
      for (int index = 0; index != this.m_certificates.Count; ++index)
        certificates[index] = TimeStampIdentifier.GetTimeStampCertID((object) this.m_certificates[index]);
      return certificates;
    }
  }
}
