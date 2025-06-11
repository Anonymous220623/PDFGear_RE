// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.MessageStamp
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class MessageStamp : Asn1Sequence
{
  internal Asn1Sequence m_sequence;
  private Algorithms m_hashAlgorithm;
  private byte[] m_hash;

  internal MessageStamp(string id, byte[] hash)
  {
    this.Objects.Add((object) new Algorithms(new Asn1Identifier(id), (Asn1) DerNull.Value));
    this.Objects.Add((object) Asn1.FromByteArray(hash));
  }

  internal static MessageStamp GetMessageStamp(object obj)
  {
    switch (obj)
    {
      case null:
      case MessageStamp _:
        return (MessageStamp) obj;
      case Asn1Sequence _:
        return new MessageStamp((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence " + obj.GetType().FullName);
    }
  }

  private MessageStamp(Asn1Sequence sequence)
  {
    this.m_hashAlgorithm = sequence.Count == 2 ? Algorithms.GetAlgorithms((object) sequence[0]) : throw new ArgumentException("Invalid length in sequence", nameof (sequence));
    this.m_hash = Asn1Octet.GetOctetString((object) sequence[1]).GetOctets();
  }

  internal string HashAlgorithm => this.m_hashAlgorithm.ObjectID.ID;

  internal byte[] HashedMessage => this.m_hash;
}
