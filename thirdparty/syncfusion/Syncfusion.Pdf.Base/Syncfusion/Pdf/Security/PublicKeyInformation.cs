// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PublicKeyInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PublicKeyInformation : Asn1Encode
{
  private Algorithms m_algorithms;
  private DerBitString m_publicKey;

  internal Algorithms Algorithm => this.m_algorithms;

  internal DerBitString PublicKey => this.m_publicKey;

  internal static PublicKeyInformation GetPublicKeyInformation(object obj)
  {
    if (obj is PublicKeyInformation)
      return (PublicKeyInformation) obj;
    return obj != null ? new PublicKeyInformation(Asn1Sequence.GetSequence(obj)) : (PublicKeyInformation) null;
  }

  internal PublicKeyInformation(Algorithms algorithms, Asn1Encode publicKey)
  {
    this.m_publicKey = new DerBitString(publicKey);
    this.m_algorithms = algorithms;
  }

  internal PublicKeyInformation(Algorithms algorithms, byte[] publicKey)
  {
    this.m_publicKey = new DerBitString(publicKey);
    this.m_algorithms = algorithms;
  }

  private PublicKeyInformation(Asn1Sequence sequence)
  {
    this.m_algorithms = sequence.Count == 2 ? Algorithms.GetAlgorithms((object) sequence[0]) : throw new ArgumentException("Invalid length in sequence");
    this.m_publicKey = DerBitString.GetString((object) sequence[1]);
  }

  internal Asn1 GetPublicKey() => Asn1.FromByteArray(this.m_publicKey.GetBytes());

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1Encode[2]
    {
      (Asn1Encode) this.m_algorithms,
      (Asn1Encode) this.m_publicKey
    });
  }
}
