// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OcspResponse
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class OcspResponse : Asn1Encode
{
  private OcspResponseStatus m_responseStatus;
  private RevocationResponseBytes m_responseBytes;

  private OcspResponse(Asn1Sequence sequence)
  {
    this.m_responseStatus = new OcspResponseStatus(new DerCatalogue().GetEnumeration((object) sequence[0]));
    if (sequence.Count != 2)
      return;
    this.m_responseBytes = new RevocationResponseBytes().GetResponseBytes((Asn1Tag) sequence[1], true);
  }

  internal OcspResponse()
  {
  }

  internal OcspResponse GetOcspResponse(object obj)
  {
    switch (obj)
    {
      case null:
      case OcspResponse _:
        return (OcspResponse) obj;
      case Asn1Sequence _:
        return new OcspResponse((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence");
    }
  }

  internal OcspResponseStatus ResponseStatus => this.m_responseStatus;

  internal RevocationResponseBytes ResponseBytes => this.m_responseBytes;

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[1]
    {
      (Asn1Encode) this.m_responseStatus
    });
    if (this.m_responseBytes != null)
      collection.Add((Asn1Encode) new DerTag(true, 0, (Asn1Encode) this.m_responseBytes));
    return (Asn1) new DerSequence(collection);
  }
}
