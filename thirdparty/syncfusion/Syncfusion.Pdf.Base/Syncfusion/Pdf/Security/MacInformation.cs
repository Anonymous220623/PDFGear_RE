// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.MacInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class MacInformation : Asn1Encode
{
  internal DigestInformation m_digest;
  internal byte[] m_value;
  internal Number m_count;

  internal static MacInformation GetInformation(object obj)
  {
    switch (obj)
    {
      case MacInformation _:
        return (MacInformation) obj;
      case Asn1Sequence _:
        return new MacInformation((Asn1Sequence) obj);
      default:
        throw new Exception("Invalid entry");
    }
  }

  private MacInformation(Asn1Sequence sequence)
  {
    this.m_digest = DigestInformation.GetDigestInformation((object) sequence[0]);
    this.m_value = ((Asn1Octet) sequence[1]).GetOctets();
    if (sequence.Count == 3)
      this.m_count = ((DerInteger) sequence[2]).Value;
    else
      this.m_count = Number.One;
  }

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[2]
    {
      (Asn1Encode) this.m_digest,
      (Asn1Encode) new DerOctet(this.m_value)
    });
    if (!this.m_count.Equals((object) Number.One))
      collection.Add((Asn1Encode) new DerInteger(this.m_count));
    return (Asn1) new DerSequence(collection);
  }
}
