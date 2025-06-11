// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.EncryptedPrivateKey
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class EncryptedPrivateKey : Asn1Encode
{
  private Algorithms m_algorithms;
  private Asn1Octet m_octet;

  internal Algorithms EncryptionAlgorithm => this.m_algorithms;

  internal byte[] EncryptedData => this.m_octet.GetOctets();

  private EncryptedPrivateKey(Asn1Sequence sequence)
  {
    this.m_algorithms = sequence.Count == 2 ? Algorithms.GetAlgorithms((object) sequence[0]) : throw new ArgumentException("Invalid length in sequence");
    this.m_octet = Asn1Octet.GetOctetString((object) sequence[1]);
  }

  internal static EncryptedPrivateKey GetEncryptedPrivateKeyInformation(object obj)
  {
    switch (obj)
    {
      case EncryptedPrivateKey _:
        return (EncryptedPrivateKey) obj;
      case Asn1Sequence _:
        return new EncryptedPrivateKey((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence");
    }
  }

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1Encode[2]
    {
      (Asn1Encode) this.m_algorithms,
      (Asn1Encode) this.m_octet
    });
  }
}
