// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1Parser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1Parser
{
  private Stream m_stream;
  private int m_limit;
  private byte[][] m_buffers;

  internal Asn1Parser(Stream stream)
    : this(stream, Asn1Stream.GetLimit(stream))
  {
  }

  public Asn1Parser(Stream stream, int limit)
  {
    this.m_stream = stream.CanRead ? stream : throw new ArgumentException("Invalid stream");
    this.m_limit = limit;
    this.m_buffers = new byte[16 /*0x10*/][];
  }

  internal Asn1Parser(byte[] encoding)
    : this((Stream) new MemoryStream(encoding, false), encoding.Length)
  {
  }

  internal IAsn1 ReadIndefinite(int tagValue)
  {
    switch (tagValue)
    {
      case 4:
        return (IAsn1) new BerOctetHelper(this);
      case 16 /*0x10*/:
        return (IAsn1) new BerSequenceHelper(this);
      default:
        throw new Exception("Invalid entry in sequence");
    }
  }

  internal IAsn1 ReadImplicit(bool constructed, int tagNumber)
  {
    if (this.m_stream is Asn1LengthStream)
    {
      if (!constructed)
        throw new IOException("Invalid length specified");
      return this.ReadIndefinite(tagNumber);
    }
    if (constructed)
    {
      switch (tagNumber)
      {
        case 4:
          return (IAsn1) new BerOctetHelper(this);
        case 16 /*0x10*/:
          return (IAsn1) new DerSequenceHelper(this);
        case 17:
          return (IAsn1) new DerSetHelper(this);
      }
    }
    else
    {
      switch (tagNumber)
      {
        case 4:
          return (IAsn1) new DerOctetHelper((Asn1StreamHelper) this.m_stream);
        case 16 /*0x10*/:
          throw new Exception("Constructed encoding is not used in the sequence");
        case 17:
          throw new Exception("Constructed encoding is not used in the set");
      }
    }
    throw new Exception("Implicit tagging is not supported");
  }

  internal Asn1 ReadTaggedObject(bool constructed, int tagNumber)
  {
    if (!constructed)
    {
      Asn1StreamHelper stream = (Asn1StreamHelper) this.m_stream;
      return (Asn1) new DerTag(false, tagNumber, (Asn1Encode) new DerOctet(stream.ToArray()));
    }
    Asn1EncodeCollection collection = this.ReadCollection();
    return this.m_stream is Asn1LengthStream ? (collection.Count != 1 ? (Asn1) new BerTag(false, tagNumber, (Asn1Encode) BerSequence.FromCollection(collection)) : (Asn1) new BerTag(true, tagNumber, collection[0])) : (collection.Count != 1 ? (Asn1) new DerTag(false, tagNumber, (Asn1Encode) DerSequence.FromCollection(collection)) : (Asn1) new DerTag(true, tagNumber, collection[0]));
  }

  public virtual IAsn1 ReadObject()
  {
    int tagNumber = this.m_stream.ReadByte();
    if (tagNumber == -1)
      return (IAsn1) null;
    this.SetEndOfFile(false);
    int num = Asn1Stream.ReadTagNumber(this.m_stream, tagNumber);
    bool isConstructed = (tagNumber & 32 /*0x20*/) != 0;
    int length = Asn1Stream.GetLength(this.m_stream, this.m_limit);
    if (length < 0)
    {
      if (!isConstructed)
        throw new IOException("Invalid length specified");
      Asn1Parser helper = new Asn1Parser((Stream) new Asn1LengthStream(this.m_stream, this.m_limit), this.m_limit);
      return (tagNumber & 128 /*0x80*/) != 0 ? (IAsn1) new BerTagHelper(true, num, helper) : helper.ReadIndefinite(num);
    }
    Asn1StreamHelper stream = new Asn1StreamHelper(this.m_stream, length);
    if ((tagNumber & 128 /*0x80*/) != 0)
      return (IAsn1) new BerTagHelper(isConstructed, num, new Asn1Parser((Stream) stream));
    if (isConstructed)
    {
      switch (num)
      {
        case 4:
          return (IAsn1) new BerOctetHelper(new Asn1Parser((Stream) stream));
        case 16 /*0x10*/:
          return (IAsn1) new DerSequenceHelper(new Asn1Parser((Stream) stream));
        case 17:
          return (IAsn1) new DerSetHelper(new Asn1Parser((Stream) stream));
        default:
          return (IAsn1) null;
      }
    }
    else
    {
      if (num == 4)
        return (IAsn1) new DerOctetHelper(stream);
      try
      {
        return (IAsn1) Asn1Stream.GetPrimitiveObject(num, stream, this.m_buffers);
      }
      catch (ArgumentException ex)
      {
        throw new Exception("Invalid or corrupted stream", (Exception) ex);
      }
    }
  }

  private void SetEndOfFile(bool enabled)
  {
    if (!(this.m_stream is Asn1LengthStream))
      return;
    ((Asn1LengthStream) this.m_stream).SetEndOfFileOnStart(enabled);
  }

  internal Asn1EncodeCollection ReadCollection()
  {
    Asn1EncodeCollection encodeCollection = new Asn1EncodeCollection(new Asn1Encode[0]);
    IAsn1 asn1;
    while ((asn1 = this.ReadObject()) != null)
      encodeCollection.Add((Asn1Encode) asn1.GetAsn1());
    return encodeCollection;
  }
}
