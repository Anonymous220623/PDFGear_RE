// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1Octet
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1Octet : Asn1, IAsn1Octet, IAsn1
{
  internal byte[] m_value;

  internal IAsn1Octet Parser => (IAsn1Octet) this;

  internal Asn1Octet(byte[] value)
    : base(Asn1UniversalTags.OctetString)
  {
    this.m_value = value != null ? value : throw new ArgumentNullException(nameof (value));
  }

  internal Asn1Octet(Syncfusion.Pdf.Security.Asn1Encode obj)
  {
    try
    {
      this.m_value = obj.GetEncoded("DER");
    }
    catch (IOException ex)
    {
      throw new ArgumentException(ex.ToString());
    }
  }

  public Stream GetOctetStream() => (Stream) new MemoryStream(this.m_value, false);

  internal virtual byte[] GetOctets() => this.m_value;

  public override int GetHashCode() => Asn1Constants.GetHashCode(this.GetOctets());

  protected override bool IsEquals(Asn1 asn1Object)
  {
    return asn1Object is DerOctet derOctet && Asn1Constants.AreEqual(this.GetOctets(), derOctet.GetOctets());
  }

  public override string ToString() => this.m_value.ToString();

  internal byte[] AsnEncode() => this.Asn1Encode(this.m_value);

  internal override void Encode(DerStream stream) => throw new NotImplementedException();

  internal static Asn1Octet GetOctetString(Asn1Tag tag, bool isExplicit)
  {
    Asn1 asn1 = tag.GetObject();
    return isExplicit || asn1 is Asn1Octet ? Asn1Octet.GetOctetString((object) asn1) : (Asn1Octet) BerOctet.GetBerOctet(Asn1Sequence.GetSequence((object) asn1));
  }

  internal static Asn1Octet GetOctetString(object obj)
  {
    switch (obj)
    {
      case null:
      case Asn1Octet _:
        return (Asn1Octet) obj;
      case Asn1Tag _:
        return Asn1Octet.GetOctetString((object) ((Asn1Tag) obj).GetObject());
      default:
        throw new ArgumentException("Invalid object entry");
    }
  }
}
