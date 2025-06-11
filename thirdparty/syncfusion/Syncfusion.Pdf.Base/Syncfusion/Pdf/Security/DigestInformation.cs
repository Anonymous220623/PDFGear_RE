// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DigestInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DigestInformation : Asn1Encode
{
  private byte[] m_bytes;
  private Algorithms m_algorithms;

  internal static DigestInformation GetDigestInformation(object obj)
  {
    switch (obj)
    {
      case DigestInformation _:
        return (DigestInformation) obj;
      case Asn1Sequence _:
        return new DigestInformation((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  internal DigestInformation(Algorithms algorithms, byte[] bytes)
  {
    this.m_bytes = bytes;
    this.m_algorithms = algorithms;
  }

  private DigestInformation(Asn1Sequence sequence)
  {
    this.m_algorithms = sequence.Count == 2 ? Algorithms.GetAlgorithms((object) sequence[0]) : throw new ArgumentException("Invalid length in sequence");
    this.m_bytes = Asn1Octet.GetOctetString((object) sequence[1]).GetOctets();
  }

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1Encode[2]
    {
      (Asn1Encode) this.m_algorithms,
      (Asn1Encode) new DerOctet(this.m_bytes)
    });
  }
}
