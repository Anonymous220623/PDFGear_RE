// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OcspHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class OcspHelper : Asn1Encode
{
  private ResponseInformation m_responseInformation;
  private Algorithms m_algorithms;
  private DerBitString m_signature;
  private Asn1Sequence m_sequence;

  internal DerBitString Signature => this.m_signature;

  internal Algorithms Algorithm => this.m_algorithms;

  internal Asn1Sequence Sequence => this.m_sequence;

  private OcspHelper(Asn1Sequence sequence)
  {
    this.m_responseInformation = new ResponseInformation().GetInformation((object) sequence[0]);
    this.m_algorithms = Algorithms.GetAlgorithms((object) sequence[1]);
    this.m_signature = (DerBitString) sequence[2];
    if (sequence.Count <= 3)
      return;
    this.m_sequence = Asn1Sequence.GetSequence((Asn1Tag) sequence[3], true);
  }

  internal OcspHelper()
  {
  }

  public OcspHelper GetOcspStructure(object obj)
  {
    switch (obj)
    {
      case null:
      case OcspHelper _:
        return (OcspHelper) obj;
      case Asn1Sequence _:
        return new OcspHelper((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence");
    }
  }

  public ResponseInformation ResponseInformation => this.m_responseInformation;

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[3]
    {
      (Asn1Encode) this.m_responseInformation,
      (Asn1Encode) this.m_algorithms,
      (Asn1Encode) this.m_signature
    });
    if (this.m_sequence != null)
      collection.Add((Asn1Encode) new DerTag(true, 0, (Asn1Encode) this.m_sequence));
    return (Asn1) new DerSequence(collection);
  }
}
