// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1Stream
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1Stream
{
  private int m_limit;
  private byte[][] m_buffers;
  private Stream m_stream;

  internal Asn1Stream(Stream stream)
    : this(stream, Asn1Stream.GetLimit(stream))
  {
    this.m_stream = stream;
  }

  public Asn1Stream(Stream stream, int limit)
  {
    this.m_stream = stream;
    this.m_limit = limit;
    this.m_buffers = new byte[16 /*0x10*/][];
  }

  public Asn1Stream(byte[] input)
    : this((Stream) new MemoryStream(input, false), input.Length)
  {
  }

  private Asn1 BuildObject(int tag, int tagNumber, int length)
  {
    bool constructed = (tag & 32 /*0x20*/) != 0;
    Asn1StreamHelper stream = new Asn1StreamHelper(this.m_stream, length);
    if ((tag & 128 /*0x80*/) != 0)
      return new Asn1Parser((Stream) stream).ReadTaggedObject(constructed, tagNumber);
    if (constructed)
    {
      switch (tagNumber)
      {
        case 4:
          return (Asn1) new BerOctet((IEnumerable) this.GetDerEncodableCollection(stream));
        case 16 /*0x10*/:
          return (Asn1) this.CreateDerSequence(stream);
        case 17:
          return (Asn1) this.CreateDerSet(stream);
      }
    }
    return Asn1Stream.GetPrimitiveObject(tagNumber, stream, this.m_buffers);
  }

  internal Asn1EncodeCollection GetEncodableCollection()
  {
    Asn1EncodeCollection encodableCollection = new Asn1EncodeCollection(new Asn1Encode[0]);
    Asn1 asn1;
    while ((asn1 = this.ReadAsn1()) != null)
      encodableCollection.Add((Asn1Encode) asn1);
    return encodableCollection;
  }

  internal virtual Asn1EncodeCollection GetDerEncodableCollection(Asn1StreamHelper stream)
  {
    return new Asn1Stream((Stream) stream).GetEncodableCollection();
  }

  internal virtual DerSequence CreateDerSequence(Asn1StreamHelper stream)
  {
    return DerSequence.FromCollection(this.GetDerEncodableCollection(stream));
  }

  internal virtual DerSet CreateDerSet(Asn1StreamHelper stream)
  {
    return DerSet.FromCollection(this.GetDerEncodableCollection(stream), false);
  }

  internal Asn1 ReadAsn1()
  {
    int num = this.m_stream.ReadByte();
    if (num <= 0)
    {
      if (num == 0)
        throw new IOException("End of contents is invalid");
      return (Asn1) null;
    }
    int tagNumber = Asn1Stream.ReadTagNumber(this.m_stream, num);
    bool flag = (num & 32 /*0x20*/) != 0;
    int length = Asn1Stream.GetLength(this.m_stream, this.m_limit);
    if (length < 0)
    {
      if (!flag)
        throw new IOException("Encodeing length is invalid");
      Asn1Parser helper = new Asn1Parser((Stream) new Asn1LengthStream(this.m_stream, this.m_limit), this.m_limit);
      if ((num & 128 /*0x80*/) != 0)
        return new BerTagHelper(true, tagNumber, helper).GetAsn1();
      switch (tagNumber)
      {
        case 4:
          return new BerOctetHelper(helper).GetAsn1();
        case 16 /*0x10*/:
          return new BerSequenceHelper(helper).GetAsn1();
        default:
          throw new IOException("Invalid object in the sequence");
      }
    }
    else
    {
      try
      {
        return this.BuildObject(num, tagNumber, length);
      }
      catch (ArgumentException ex)
      {
        throw new Exception(ex.Message);
      }
    }
  }

  internal static int GetLimit(Stream input)
  {
    switch (input)
    {
      case BaseStream _:
        return ((BaseStream) input).Remaining;
      case MemoryStream _:
        MemoryStream memoryStream = (MemoryStream) input;
        return (int) (memoryStream.Length - memoryStream.Position);
      default:
        return int.MaxValue;
    }
  }

  internal static int ReadTagNumber(Stream stream, int tagNumber)
  {
    int num1 = tagNumber & 31 /*0x1F*/;
    if (num1 == 31 /*0x1F*/)
    {
      int num2 = 0;
      int num3 = stream.ReadByte();
      if ((num3 & (int) sbyte.MaxValue) == 0)
        throw new IOException("Invalid tag number specified");
      for (; num3 >= 0 && (num3 & 128 /*0x80*/) != 0; num3 = stream.ReadByte())
        num2 = (num2 | num3 & (int) sbyte.MaxValue) << 7;
      if (num3 < 0)
        throw new EndOfStreamException("End of file detected");
      num1 = num2 | num3 & (int) sbyte.MaxValue;
    }
    return num1;
  }

  internal static int GetLength(Stream stream, int limit)
  {
    int length = stream.ReadByte();
    if (length < 0)
      throw new EndOfStreamException("End of file detected");
    if (length == 128 /*0x80*/)
      return -1;
    if (length > (int) sbyte.MaxValue)
    {
      int num1 = length & (int) sbyte.MaxValue;
      if (num1 > 4)
        throw new IOException("Invalid length detected " + (object) num1);
      length = 0;
      for (int index = 0; index < num1; ++index)
      {
        int num2 = stream.ReadByte();
        if (num2 < 0)
          throw new EndOfStreamException("End of file detected");
        length = (length << 8) + num2;
      }
      if (length < 0)
        throw new IOException("Invalid length or corrupted input stream detected");
      if (length >= limit)
        throw new IOException("Out of bound or corrupted stream detected");
    }
    return length;
  }

  internal static byte[] GetBytes(Asn1StreamHelper stream, byte[][] buffers)
  {
    int remaining = stream.Remaining;
    if (remaining >= buffers.Length)
      return stream.ToArray();
    byte[] bytes = buffers[remaining] ?? (buffers[remaining] = new byte[remaining]);
    stream.ReadAll(bytes);
    return bytes;
  }

  internal static Asn1 GetPrimitiveObject(int tagNumber, Asn1StreamHelper stream, byte[][] buffers)
  {
    switch (tagNumber)
    {
      case 1:
        return (Asn1) new DerBoolean(Asn1Stream.GetBytes(stream, buffers));
      case 6:
        return (Asn1) DerObjectID.FromOctetString(Asn1Stream.GetBytes(stream, buffers));
      case 10:
        return (Asn1) new DerCatalogue(Asn1Stream.GetBytes(stream, buffers));
      default:
        byte[] array = stream.ToArray();
        switch (tagNumber)
        {
          case 2:
            return (Asn1) new DerInteger(array);
          case 3:
            return (Asn1) DerBitString.FromAsn1Octets(array);
          case 4:
            return (Asn1) new DerOctet(array);
          case 5:
            return (Asn1) DerNull.Value;
          case 12:
            return (Asn1) new DerUtf8String(array);
          case 19:
            return (Asn1) new DerPrintableString(array);
          case 20:
            return (Asn1) new DerTeleText(array);
          case 22:
            return (Asn1) new DerAsciiString(array);
          case 23:
            return (Asn1) new DerUtcTime(array);
          case 24:
            return (Asn1) new GeneralizedTime(array);
          case 30:
            return (Asn1) new DerBmpString(array);
          default:
            return (Asn1) null;
        }
    }
  }
}
