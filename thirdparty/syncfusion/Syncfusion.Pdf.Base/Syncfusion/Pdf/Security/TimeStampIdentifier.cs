// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.TimeStampIdentifier
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class TimeStampIdentifier
{
  private Asn1Octet m_hash;
  private TimeStampCertIssuerDetails m_issuerDetail;

  internal static TimeStampIdentifier GetTimeStampCertID(object obj)
  {
    switch (obj)
    {
      case null:
      case TimeStampIdentifier _:
        return (TimeStampIdentifier) obj;
      case Asn1Sequence _:
        return new TimeStampIdentifier((Asn1Sequence) obj);
      default:
        throw new ArgumentException($"Invalid entry in sequence : {obj.GetType().Name}.");
    }
  }

  internal TimeStampIdentifier(Asn1Sequence sequence)
  {
    this.m_hash = sequence.Count >= 1 && sequence.Count <= 2 ? Asn1Octet.GetOctetString((object) sequence[0]) : throw new ArgumentException("Invalid sequence size : " + (object) sequence.Count);
    if (sequence.Count <= 1)
      return;
    this.m_issuerDetail = TimeStampCertIssuerDetails.GetIssuerDetails((object) sequence[1]);
  }
}
