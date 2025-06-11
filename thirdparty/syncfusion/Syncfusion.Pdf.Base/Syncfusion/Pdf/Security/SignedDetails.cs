// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SignedDetails
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SignedDetails : Asn1Encode
{
  private DerInteger m_version;
  private Asn1Set m_digestAlgorithms;
  private ContentInformation m_contentInformation;
  private Asn1Set m_certificates;
  private Asn1Set m_crls;
  private Asn1Set m_signerInformation;
  private bool m_certsBer;
  private bool m_crlsBerObject;

  internal SignedDetails(Asn1Sequence seq)
  {
    IEnumerator enumerator = seq.GetEnumerator();
    enumerator.MoveNext();
    this.m_version = (DerInteger) enumerator.Current;
    enumerator.MoveNext();
    this.m_digestAlgorithms = (Asn1Set) enumerator.Current;
    enumerator.MoveNext();
    this.m_contentInformation = ContentInformation.GetInformation(enumerator.Current);
    while (enumerator.MoveNext())
    {
      Asn1 current = (Asn1) enumerator.Current;
      if (current is Asn1Tag)
      {
        Asn1Tag taggedObject = (Asn1Tag) current;
        switch (taggedObject.TagNumber)
        {
          case 0:
            this.m_certsBer = taggedObject is BerTag;
            this.m_certificates = Asn1Set.GetAsn1Set(taggedObject, false);
            continue;
          case 1:
            this.m_crlsBerObject = taggedObject is BerTag;
            this.m_crls = Asn1Set.GetAsn1Set(taggedObject, false);
            continue;
          default:
            throw new ArgumentException("Invalid entry in tag value : " + (object) taggedObject.TagNumber);
        }
      }
      else
        this.m_signerInformation = (Asn1Set) current;
    }
  }

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[3]
    {
      (Asn1Encode) this.m_version,
      (Asn1Encode) this.m_digestAlgorithms,
      (Asn1Encode) this.m_contentInformation
    });
    if (this.m_certificates != null)
    {
      if (this.m_certsBer)
        collection.Add((Asn1Encode) new BerTag(false, 0, (Asn1Encode) this.m_certificates));
      else
        collection.Add((Asn1Encode) new DerTag(false, 0, (Asn1Encode) this.m_certificates));
    }
    if (this.m_crls != null)
    {
      if (this.m_crlsBerObject)
        collection.Add((Asn1Encode) new BerTag(false, 1, (Asn1Encode) this.m_crls));
      else
        collection.Add((Asn1Encode) new DerTag(false, 1, (Asn1Encode) this.m_crls));
    }
    collection.Add((Asn1Encode) this.m_signerInformation);
    return (Asn1) new BerSequence(collection);
  }

  internal ContentInformation ContentInformation => this.m_contentInformation;

  internal Asn1Set SignerInformation => this.m_signerInformation;
}
