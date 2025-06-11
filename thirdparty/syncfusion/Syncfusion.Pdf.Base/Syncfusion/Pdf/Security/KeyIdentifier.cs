// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.KeyIdentifier
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class KeyIdentifier : Asn1Encode
{
  internal Asn1Octet m_keyIdentifier;
  internal DerInteger m_serialNumber;

  internal byte[] KeyID
  {
    get => this.m_keyIdentifier != null ? this.m_keyIdentifier.GetOctets() : (byte[]) null;
  }

  internal static KeyIdentifier GetKeyIdentifier(object obj)
  {
    switch (obj)
    {
      case KeyIdentifier _:
        return (KeyIdentifier) obj;
      case Asn1Sequence _:
        return new KeyIdentifier((Asn1Sequence) obj);
      case X509Extension _:
        return KeyIdentifier.GetKeyIdentifier((object) X509Extension.ConvertValueToObject((X509Extension) obj));
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  protected internal KeyIdentifier(Asn1Sequence sequence)
  {
    foreach (Asn1Tag tag in sequence)
    {
      switch (tag.TagNumber)
      {
        case 0:
          this.m_keyIdentifier = Asn1Octet.GetOctetString((object) tag);
          continue;
        case 1:
          continue;
        case 2:
          this.m_serialNumber = DerInteger.GetNumber(tag, false);
          continue;
        default:
          throw new ArgumentException("Invalid entry in sequence");
      }
    }
  }

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[0]);
    if (this.m_keyIdentifier != null)
      collection.Add((Asn1Encode) new DerTag(false, 0, (Asn1Encode) this.m_keyIdentifier));
    if (this.m_serialNumber != null)
      collection.Add((Asn1Encode) new DerTag(false, 2, (Asn1Encode) this.m_serialNumber));
    return (Asn1) new DerSequence(collection);
  }

  public override string ToString()
  {
    return $"AuthorityKeyIdentifier: KeyID({(object) this.m_keyIdentifier.GetOctets()})";
  }
}
