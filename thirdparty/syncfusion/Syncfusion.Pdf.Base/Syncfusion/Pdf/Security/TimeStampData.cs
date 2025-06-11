// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.TimeStampData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class TimeStampData : Asn1Encode
{
  private readonly DerInteger m_version;
  private readonly DerObjectID m_policyId;
  private readonly MessageStamp m_messageImprint;
  private readonly DerInteger m_serialNumber;
  private readonly GeneralizedTime m_generalizedTime;
  private readonly DerPrecision m_precision;
  private readonly DerBoolean m_isOrdered;
  private readonly DerInteger m_nonce;
  private readonly DerName m_timeStampName;
  private readonly X509Extensions m_extensions;

  internal DerInteger Version => this.m_version;

  internal MessageStamp MessageImprint => this.m_messageImprint;

  internal DerObjectID Policy => this.m_policyId;

  internal DerInteger SerialNumber => this.m_serialNumber;

  internal DerPrecision Precision => this.m_precision;

  internal GeneralizedTime GeneralizedTime => this.m_generalizedTime;

  internal DerBoolean IsOrdered => this.m_isOrdered;

  internal DerInteger Nonce => this.m_nonce;

  internal DerName TimeStampName => this.m_timeStampName;

  internal X509Extensions Extensions => this.m_extensions;

  internal TimeStampData(Asn1Sequence sequence)
  {
    IEnumerator enumerator = sequence.GetEnumerator();
    enumerator.MoveNext();
    this.m_version = DerInteger.GetNumber(enumerator.Current);
    enumerator.MoveNext();
    this.m_policyId = DerObjectID.GetID(enumerator.Current);
    enumerator.MoveNext();
    if ((enumerator.Current as Asn1Sequence).Count != 2)
      throw new ArgumentException("Invalid entry in sequence", "seq");
    this.m_messageImprint = MessageStamp.GetMessageStamp(enumerator.Current);
    enumerator.MoveNext();
    this.m_serialNumber = DerInteger.GetNumber(enumerator.Current);
    enumerator.MoveNext();
    this.m_generalizedTime = GeneralizedTime.GetGeneralizedTime(enumerator.Current);
    this.m_isOrdered = DerBoolean.False;
    while (enumerator.MoveNext())
    {
      Asn1 current = (Asn1) enumerator.Current;
      if (current is Asn1Tag)
      {
        DerTag tag = (DerTag) current;
        switch (tag.TagNumber)
        {
          case 0:
            this.m_timeStampName = DerName.GetDerName((Asn1Tag) tag, true);
            break;
          case 1:
            this.m_extensions = X509Extensions.GetInstance((Asn1Tag) tag, false);
            break;
          default:
            throw new ArgumentException("Invalid tag value");
        }
      }
      if (current is DerSequence)
        this.m_precision = DerPrecision.GetDerPrecision((object) current);
      if (current is DerBoolean)
        this.m_isOrdered = DerBoolean.GetBoolean((object) current);
      if (current is DerInteger)
        this.m_nonce = DerInteger.GetNumber((object) current);
    }
  }

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[5]
    {
      (Asn1Encode) this.m_version,
      (Asn1Encode) this.m_policyId,
      (Asn1Encode) this.m_messageImprint,
      (Asn1Encode) this.m_serialNumber,
      (Asn1Encode) this.m_generalizedTime
    });
    if (this.m_precision != null)
      collection.Add((Asn1Encode) this.m_precision);
    if (this.m_isOrdered != null && this.m_isOrdered.IsTrue)
      collection.Add((Asn1Encode) this.m_isOrdered);
    if (this.m_nonce != null)
      collection.Add((Asn1Encode) this.m_nonce);
    if (this.m_timeStampName != null)
      collection.Add((Asn1Encode) new DerTag(true, 0, (Asn1Encode) this.m_timeStampName));
    if (this.m_extensions != null)
      collection.Add((Asn1Encode) new DerTag(false, 1, (Asn1Encode) this.m_extensions));
    return (Asn1) new DerSequence(collection);
  }
}
