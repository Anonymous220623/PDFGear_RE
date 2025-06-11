// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SignerDetails
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SignerDetails : Asn1Encode
{
  private DerInteger m_version;
  private SignerIdentity m_id;
  private Algorithms m_algorithm;
  private Asn1Set m_attributes;
  private Algorithms m_encryptionAlgorithm;
  private Asn1Octet m_encryptedOctet;
  private Asn1Set m_elements;

  internal static SignerDetails GetSignerDetails(object obj)
  {
    switch (obj)
    {
      case null:
      case SignerDetails _:
        return (SignerDetails) obj;
      case Asn1Sequence _:
        return new SignerDetails((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in signer details" + obj.GetType().FullName, nameof (obj));
    }
  }

  internal SignerDetails(Asn1Sequence seq)
  {
    IEnumerator enumerator = seq.GetEnumerator();
    enumerator.MoveNext();
    this.m_version = (DerInteger) enumerator.Current;
    enumerator.MoveNext();
    this.m_id = SignerIdentity.GetIdentity(enumerator.Current);
    enumerator.MoveNext();
    this.m_algorithm = Algorithms.GetAlgorithms(enumerator.Current);
    enumerator.MoveNext();
    object current = enumerator.Current;
    if (current is Asn1Tag)
    {
      this.m_attributes = Asn1Set.GetAsn1Set((Asn1Tag) current, false);
      enumerator.MoveNext();
      this.m_encryptionAlgorithm = Algorithms.GetAlgorithms(enumerator.Current);
    }
    else
    {
      this.m_attributes = (Asn1Set) null;
      this.m_encryptionAlgorithm = Algorithms.GetAlgorithms(current);
    }
    enumerator.MoveNext();
    this.m_encryptedOctet = Asn1Octet.GetOctetString(enumerator.Current);
    if (enumerator.MoveNext())
      this.m_elements = Asn1Set.GetAsn1Set((Asn1Tag) enumerator.Current, false);
    else
      this.m_elements = (Asn1Set) null;
  }

  internal SignerIdentity ID => this.m_id;

  internal Asn1Set Attributes => this.m_attributes;

  internal Algorithms DigestAlgorithm => this.m_algorithm;

  internal Asn1Octet EncryptedOctet => this.m_encryptedOctet;

  internal Algorithms EncryptionAlgorithm => this.m_encryptionAlgorithm;

  internal Asn1Set Elements => this.m_elements;

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[3]
    {
      (Asn1Encode) this.m_version,
      (Asn1Encode) this.m_id,
      (Asn1Encode) this.m_algorithm
    });
    if (this.m_attributes != null)
      collection.Add((Asn1Encode) new DerTag(false, 0, (Asn1Encode) this.m_attributes));
    collection.Add((Asn1Encode) this.m_encryptionAlgorithm, (Asn1Encode) this.m_encryptedOctet);
    if (this.m_elements != null)
      collection.Add((Asn1Encode) new DerTag(false, 1, (Asn1Encode) this.m_elements));
    return (Asn1) new DerSequence(collection);
  }
}
