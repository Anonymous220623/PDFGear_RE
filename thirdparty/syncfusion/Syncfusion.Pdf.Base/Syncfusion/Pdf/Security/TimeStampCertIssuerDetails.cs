// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.TimeStampCertIssuerDetails
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class TimeStampCertIssuerDetails : Asn1Encode
{
  private DerNames m_issuerName;
  private DerInteger m_serialNumber;
  private DerBitString m_issuerId;

  internal static TimeStampCertIssuerDetails GetIssuerDetails(object obj)
  {
    switch (obj)
    {
      case null:
      case TimeStampCertIssuerDetails _:
        return (TimeStampCertIssuerDetails) obj;
      case Asn1Sequence _:
        return new TimeStampCertIssuerDetails((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence : " + obj.GetType().Name, nameof (obj));
    }
  }

  private TimeStampCertIssuerDetails(Asn1Sequence sequence)
  {
    this.m_issuerName = sequence.Count == 2 || sequence.Count == 3 ? DerNames.GetDerNames((object) sequence[0]) : throw new ArgumentException("Invalid sequence size : " + (object) sequence.Count);
    this.m_serialNumber = DerInteger.GetNumber((object) sequence[1]);
    if (sequence.Count != 3)
      return;
    this.m_issuerId = DerBitString.GetString((object) sequence[2]);
  }

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[2]
    {
      (Asn1Encode) this.m_issuerName,
      (Asn1Encode) this.m_serialNumber
    });
    if (this.m_issuerId != null)
      collection.Add((Asn1Encode) this.m_issuerId);
    return (Asn1) new DerSequence(collection);
  }
}
