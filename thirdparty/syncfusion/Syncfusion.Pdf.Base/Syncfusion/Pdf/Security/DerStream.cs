// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerStream
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerStream
{
  internal Stream m_stream;

  internal DerStream(Stream stream) => this.m_stream = stream;

  internal DerStream()
  {
  }

  private void WriteLength(int length)
  {
    if (length > (int) sbyte.MaxValue)
    {
      int num1 = 1;
      uint num2 = (uint) length;
      while ((num2 >>= 8) != 0U)
        ++num1;
      this.m_stream.WriteByte((byte) (num1 | 128 /*0x80*/));
      for (int index = (num1 - 1) * 8; index >= 0; index -= 8)
        this.m_stream.WriteByte((byte) (length >> index));
    }
    else
      this.m_stream.WriteByte((byte) length);
  }

  internal void WriteEncoded(int tagNumber, byte[] bytes)
  {
    this.m_stream.WriteByte((byte) tagNumber);
    this.WriteLength(bytes.Length);
    this.m_stream.Write(bytes, 0, bytes.Length);
  }

  internal void WriteEncoded(int tagNumber, byte[] bytes, int offset, int length)
  {
    this.m_stream.WriteByte((byte) tagNumber);
    this.WriteLength(length);
    this.m_stream.Write(bytes, offset, length);
  }

  internal void WriteTag(int flag, int tagNumber)
  {
    if (tagNumber < 31 /*0x1F*/)
    {
      this.m_stream.WriteByte((byte) (flag | tagNumber));
    }
    else
    {
      this.m_stream.WriteByte((byte) (flag | 31 /*0x1F*/));
      if (tagNumber < 128 /*0x80*/)
      {
        this.m_stream.WriteByte((byte) tagNumber);
      }
      else
      {
        byte[] buffer = new byte[5];
        int length = buffer.Length;
        int offset;
        buffer[offset = length - 1] = (byte) (tagNumber & (int) sbyte.MaxValue);
        do
        {
          tagNumber >>= 7;
          buffer[--offset] = (byte) (tagNumber & (int) sbyte.MaxValue | 128 /*0x80*/);
        }
        while (tagNumber > (int) sbyte.MaxValue);
        this.m_stream.Write(buffer, offset, buffer.Length - offset);
      }
    }
  }

  internal void WriteEncoded(int flag, int tagNumber, byte[] bytes)
  {
    this.WriteTag(flag, tagNumber);
    this.WriteLength(bytes.Length);
    this.m_stream.Write(bytes, 0, bytes.Length);
  }

  internal virtual void WriteObject(object obj)
  {
    switch (obj)
    {
      case null:
        this.m_stream.WriteByte((byte) 5);
        this.m_stream.WriteByte((byte) 0);
        break;
      case Asn1 _:
        ((Asn1) obj).Encode(this);
        break;
      case Asn1Encode _:
        ((Asn1Encode) obj).GetAsn1().Encode(this);
        break;
      default:
        throw new IOException("Invalid object specified");
    }
  }
}
