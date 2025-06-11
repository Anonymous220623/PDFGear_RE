// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RevocationResponseBytes
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RevocationResponseBytes : Asn1Encode
{
  private DerObjectID m_responseType;
  private Asn1Octet m_response;

  private RevocationResponseBytes(Asn1Sequence sequence)
  {
    this.m_responseType = sequence.Count == 2 ? DerObjectID.GetID((object) sequence[0]) : throw new ArgumentException("Invalid length in sequence");
    this.m_response = Asn1Octet.GetOctetString((object) sequence[1]);
  }

  internal RevocationResponseBytes()
  {
  }

  public RevocationResponseBytes GetResponseBytes(Asn1Tag tag, bool isExplicit)
  {
    return this.GetResponseBytes((object) Asn1Sequence.GetSequence(tag, isExplicit));
  }

  public RevocationResponseBytes GetResponseBytes(object obj)
  {
    switch (obj)
    {
      case null:
      case RevocationResponseBytes _:
        return (RevocationResponseBytes) obj;
      case Asn1Sequence _:
        return new RevocationResponseBytes((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence");
    }
  }

  public DerObjectID ResponseType => this.m_responseType;

  public Asn1Octet Response => this.m_response;

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1Encode[2]
    {
      (Asn1Encode) this.m_responseType,
      (Asn1Encode) this.m_response
    });
  }
}
