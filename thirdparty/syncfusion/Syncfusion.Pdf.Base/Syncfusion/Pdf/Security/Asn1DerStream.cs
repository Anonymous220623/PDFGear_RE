// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1DerStream
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1DerStream : DerStream
{
  private MemoryStream m_stream;

  internal Asn1DerStream() => this.m_stream = new MemoryStream();

  public Asn1DerStream(Stream stream)
    : base(stream)
  {
  }

  internal byte[] ParseTimeStamp(Asn1 encodedObject)
  {
    Asn1DerStream asn1DerStream = new Asn1DerStream();
    this.m_stream.WriteByte((byte) 48 /*0x30*/);
    this.m_stream.WriteByte((byte) 128 /*0x80*/);
    if ((encodedObject as Asn1Sequence)[0] is Asn1Identifier)
    {
      byte[] buffer = ((encodedObject as Asn1Sequence)[0] as Asn1Identifier).Asn1Encode();
      this.m_stream.Write(buffer, 0, buffer.Length);
    }
    if ((encodedObject as Asn1Sequence)[1] is Asn1Tag)
    {
      Asn1 asn1 = encodedObject;
      this.m_stream.WriteByte((byte) (160 /*0xA0*/ | (asn1 as Asn1Tag).TagNumber));
      this.m_stream.WriteByte((byte) 128 /*0x80*/);
      Asn1Sequence sequence = Asn1Sequence.GetSequence((object) new List<Asn1>()
      {
        (asn1 as Asn1Tag).GetObject()
      });
      byte[] timeStampToken = asn1DerStream.ParseTimeStampToken((Asn1) sequence);
      this.m_stream.Write(timeStampToken, 0, timeStampToken.Length);
    }
    this.m_stream.WriteByte((byte) 0);
    this.m_stream.WriteByte((byte) 0);
    this.m_stream.WriteByte((byte) 0);
    this.m_stream.WriteByte((byte) 0);
    this.m_stream.Close();
    return this.m_stream.ToArray();
  }

  internal byte[] ParseTimeStampToken(Asn1 encodedObject)
  {
    Asn1Sequence asn1Sequence = (Asn1Sequence) null;
    switch (encodedObject)
    {
      case Asn1Sequence _:
        asn1Sequence = encodedObject as Asn1Sequence;
        break;
      case Asn1Set _:
        asn1Sequence = Asn1Sequence.GetSequence((object) encodedObject);
        break;
    }
    foreach (Asn1 encodedObject1 in asn1Sequence)
    {
      byte[] buffer = (byte[]) null;
      switch (encodedObject1)
      {
        case Asn1Integer _:
          buffer = (encodedObject1 as Asn1Integer).AsnEncode();
          break;
        case Asn1Boolean _:
          buffer = (encodedObject1 as Asn1Boolean).AsnEncode();
          break;
        case Asn1Null _:
          buffer = (encodedObject1 as Asn1Null).AsnEncode();
          break;
        case Asn1Identifier _:
          buffer = (encodedObject1 as Asn1Identifier).Asn1Encode();
          break;
        case Asn1Tag _:
          if ((encodedObject1 as Asn1Tag).GetObject() is Asn1Sequence)
            buffer = new Asn1DerStream().ParseTimeStampToken((Asn1) Asn1Sequence.GetSequence((object) new List<Asn1>()
            {
              (encodedObject1 as Asn1Tag).GetObject()
            }));
          else if ((encodedObject1 as Asn1Tag).GetObject() is Asn1Tag)
            buffer = new Asn1DerStream().ParseTimeStampToken((encodedObject1 as Asn1Tag).GetObject());
          else if ((encodedObject1 as Asn1Tag).GetObject() is Asn1Octet)
          {
            Asn1DerStream asn1DerStream = new Asn1DerStream();
            buffer = ((encodedObject1 as Asn1Tag).GetObject() as Asn1Octet).AsnEncode();
          }
          else if ((encodedObject1 as Asn1Tag).GetObject() is Asn1Integer)
          {
            Asn1DerStream asn1DerStream = new Asn1DerStream();
            buffer = ((encodedObject1 as Asn1Tag).GetObject() as Asn1Integer).AsnEncode();
          }
          if ((encodedObject1 as Asn1Tag).IsExplicit)
          {
            this.m_stream.WriteByte((byte) (160 /*0xA0*/ | (encodedObject1 as Asn1Tag).TagNumber));
            this.Write(buffer);
            break;
          }
          buffer[0] &= (byte) 32 /*0x20*/;
          buffer[0] |= (byte) (128 /*0x80*/ | (encodedObject1 as Asn1Tag).TagNumber);
          break;
        case Asn1Set _:
          buffer = new Asn1DerStream().ParseTimeStampToken(encodedObject1);
          this.m_stream.WriteByte((byte) 49);
          this.Write(buffer);
          break;
        case Asn1Sequence _:
          Asn1DerStream asn1DerStream1 = new Asn1DerStream();
          asn1DerStream1.ParseTimeStampToken(encodedObject1);
          buffer = asn1DerStream1.m_stream.ToArray();
          this.m_stream.WriteByte((byte) 48 /*0x30*/);
          this.Write(buffer);
          break;
        case Asn1Octet _:
          buffer = (encodedObject1 as Asn1Octet).AsnEncode();
          break;
        case DerUtcTime _:
          buffer = (encodedObject1 as DerUtcTime).GetDerEncoded();
          break;
        case DerPrintableString _:
          buffer = (encodedObject1 as DerPrintableString).Asn1Encode();
          break;
        case DerAsciiString _:
          buffer = (encodedObject1 as DerAsciiString).AsnEncode();
          break;
      }
      this.m_stream.Write(buffer, 0, buffer.Length);
    }
    this.m_stream.Close();
    return this.m_stream.ToArray();
  }

  private void Write(byte[] buffer)
  {
    if (buffer.Length > (int) sbyte.MaxValue)
    {
      int num = 1;
      uint length = (uint) buffer.Length;
      while ((length >>= 8) != 0U)
        ++num;
      this.m_stream.WriteByte((byte) (num | 128 /*0x80*/));
      for (int index = (num - 1) * 8; index >= 0; index -= 8)
        this.m_stream.WriteByte((byte) (buffer.Length >> index));
    }
    else
      this.m_stream.WriteByte((byte) buffer.Length);
  }
}
