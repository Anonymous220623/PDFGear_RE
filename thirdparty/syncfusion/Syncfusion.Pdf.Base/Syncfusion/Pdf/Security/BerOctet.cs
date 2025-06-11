// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.BerOctet
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class BerOctet : DerOctet, IEnumerable
{
  private const int m_max = 1000;
  private IEnumerable m_octets;

  public BerOctet(byte[] bytes)
    : base(bytes)
  {
  }

  public BerOctet(IEnumerable octets)
    : base(BerOctet.GetBytes(octets))
  {
    this.m_octets = octets;
  }

  public BerOctet(Asn1 asn1)
    : base((Syncfusion.Pdf.Security.Asn1Encode) asn1)
  {
  }

  public BerOctet(Syncfusion.Pdf.Security.Asn1Encode asn1)
    : base((Syncfusion.Pdf.Security.Asn1Encode) asn1.GetAsn1())
  {
  }

  internal override byte[] GetOctets() => this.m_value;

  private IList GenerateOctets()
  {
    IList octets = (IList) new ArrayList();
    for (int sourceIndex = 0; sourceIndex < this.m_value.Length; sourceIndex += 1000)
    {
      byte[] numArray = new byte[Math.Min(this.m_value.Length, sourceIndex + 1000) - sourceIndex];
      Array.Copy((Array) this.m_value, sourceIndex, (Array) numArray, 0, numArray.Length);
      octets.Add((object) new DerOctet(numArray));
    }
    return octets;
  }

  internal override void Encode(DerStream stream)
  {
    if (stream is Asn1DerStream)
    {
      stream.m_stream.WriteByte((byte) 36);
      stream.m_stream.WriteByte((byte) 128 /*0x80*/);
      foreach (DerOctet derOctet in this)
        stream.WriteObject((object) derOctet);
      stream.m_stream.WriteByte((byte) 0);
      stream.m_stream.WriteByte((byte) 0);
    }
    else
      base.Encode(stream);
  }

  internal static BerOctet GetBerOctet(Asn1Sequence sequence)
  {
    IList octets = (IList) new ArrayList();
    foreach (Syncfusion.Pdf.Security.Asn1Encode asn1Encode in sequence)
      octets.Add((object) asn1Encode);
    return new BerOctet((IEnumerable) octets);
  }

  private static byte[] GetBytes(IEnumerable octets)
  {
    MemoryStream memoryStream = new MemoryStream();
    foreach (Asn1Octet octet in octets)
    {
      byte[] octets1 = octet.GetOctets();
      memoryStream.Write(octets1, 0, octets1.Length);
    }
    return memoryStream.ToArray();
  }

  public IEnumerator GetEnumerator()
  {
    return this.m_octets == null ? this.GenerateOctets().GetEnumerator() : this.m_octets.GetEnumerator();
  }
}
