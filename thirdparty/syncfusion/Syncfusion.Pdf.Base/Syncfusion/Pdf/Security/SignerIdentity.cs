// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SignerIdentity
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SignerIdentity : Asn1Encode
{
  private Asn1Encode m_id;

  internal SignerIdentity(CertificateInformation id) => this.m_id = (Asn1Encode) id;

  internal SignerIdentity(Asn1Octet id)
  {
    this.m_id = (Asn1Encode) new DerTag(false, 0, (Asn1Encode) id);
  }

  internal SignerIdentity(Asn1 id) => this.m_id = (Asn1Encode) id;

  internal static SignerIdentity GetIdentity(object o)
  {
    switch (o)
    {
      case null:
      case SignerIdentity _:
        return (SignerIdentity) o;
      case CertificateInformation _:
        return new SignerIdentity((CertificateInformation) o);
      case Asn1Octet _:
        return new SignerIdentity((Asn1Octet) o);
      case Asn1 _:
        return new SignerIdentity((Asn1) o);
      default:
        throw new ArgumentException("Invalid entry in sequence: " + o.GetType().Name);
    }
  }

  internal bool IsTagged => this.m_id is Asn1Tag;

  internal Asn1Encode ID
  {
    get
    {
      return this.m_id is Asn1Tag ? (Asn1Encode) Asn1Octet.GetOctetString((Asn1Tag) this.m_id, false) : this.m_id;
    }
  }

  public override Asn1 GetAsn1() => this.m_id.GetAsn1();
}
