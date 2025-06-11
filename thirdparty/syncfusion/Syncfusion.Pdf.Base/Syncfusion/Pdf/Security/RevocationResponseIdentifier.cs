// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RevocationResponseIdentifier
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RevocationResponseIdentifier : Asn1Encode
{
  private Asn1Encode m_id;

  public RevocationResponseIdentifier(Asn1Octet id)
  {
    this.m_id = id != null ? (Asn1Encode) id : throw new ArgumentNullException(nameof (id));
  }

  public RevocationResponseIdentifier(X509Name id)
  {
    this.m_id = id != null ? (Asn1Encode) id : throw new ArgumentNullException(nameof (id));
  }

  internal RevocationResponseIdentifier()
  {
  }

  internal RevocationResponseIdentifier GetResponseID(object obj)
  {
    switch (obj)
    {
      case null:
      case RevocationResponseIdentifier _:
        return (RevocationResponseIdentifier) obj;
      case DerOctet _:
        return new RevocationResponseIdentifier((Asn1Octet) obj);
      case Asn1Tag _:
        Asn1Tag tag = (Asn1Tag) obj;
        return tag.TagNumber == 1 ? new RevocationResponseIdentifier(X509Name.GetName(tag, true)) : new RevocationResponseIdentifier(Asn1Octet.GetOctetString(tag, true));
      default:
        return new RevocationResponseIdentifier(X509Name.GetName(obj));
    }
  }

  public override Asn1 GetAsn1()
  {
    return this.m_id is Asn1Octet ? (Asn1) new DerTag(true, 2, this.m_id) : (Asn1) new DerTag(true, 1, this.m_id);
  }
}
