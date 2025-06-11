// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.KeyInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class KeyInformation : Asn1Encode
{
  private Asn1 m_privateKey;
  private Algorithms m_algorithms;
  private Asn1Set m_attributes;

  internal static KeyInformation GetInformation(object obj)
  {
    if (obj is KeyInformation)
      return (KeyInformation) obj;
    return obj != null ? new KeyInformation(Asn1Sequence.GetSequence(obj)) : (KeyInformation) null;
  }

  internal KeyInformation(Algorithms algorithms, Asn1 privateKey)
    : this(algorithms, privateKey, (Asn1Set) null)
  {
  }

  public KeyInformation(Algorithms algorithms, Asn1 privateKey, Asn1Set attributes)
  {
    this.m_privateKey = privateKey;
    this.m_algorithms = algorithms;
    this.m_attributes = attributes;
  }

  private KeyInformation(Asn1Sequence sequence)
  {
    IEnumerator enumerator = sequence.GetEnumerator();
    enumerator.MoveNext();
    Number number = ((DerInteger) enumerator.Current).Value;
    enumerator.MoveNext();
    this.m_algorithms = Algorithms.GetAlgorithms(enumerator.Current);
    try
    {
      enumerator.MoveNext();
      this.m_privateKey = Asn1.FromByteArray(((Asn1Octet) enumerator.Current).GetOctets());
    }
    catch (IOException ex)
    {
      throw new ArgumentException("Invalid private key in sequence");
    }
    if (!enumerator.MoveNext())
      return;
    this.m_attributes = Asn1Set.GetAsn1Set((Asn1Tag) enumerator.Current, false);
  }

  internal Algorithms AlgorithmID => this.m_algorithms;

  internal Asn1 PrivateKey => this.m_privateKey;

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[3]
    {
      (Asn1Encode) new DerInteger(0),
      (Asn1Encode) this.m_algorithms,
      (Asn1Encode) new DerOctet((Asn1Encode) this.m_privateKey)
    });
    if (this.m_attributes != null)
      collection.Add((Asn1Encode) new DerTag(false, 0, (Asn1Encode) this.m_attributes));
    return (Asn1) new DerSequence(collection);
  }
}
