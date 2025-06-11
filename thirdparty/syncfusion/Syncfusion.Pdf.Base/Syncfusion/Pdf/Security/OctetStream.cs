// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OctetStream
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class OctetStream : Stream
{
  private Asn1Parser m_helper;
  private bool m_first = true;
  private Stream m_stream;
  private bool m_closed;

  internal OctetStream(Asn1Parser helper) => this.m_helper = helper;

  public override int Read(byte[] buffer, int offset, int count)
  {
    if (this.m_stream == null)
    {
      if (!this.m_first)
        return 0;
      IAsn1Octet asn1Octet = (IAsn1Octet) this.m_helper.ReadObject();
      if (asn1Octet == null)
        return 0;
      this.m_first = false;
      this.m_stream = asn1Octet.GetOctetStream();
    }
    int num1 = 0;
    while (true)
    {
      do
      {
        int num2 = this.m_stream.Read(buffer, offset + num1, count - num1);
        if (num2 > 0)
          num1 += num2;
        else
          goto label_10;
      }
      while (num1 != count);
      break;
label_10:
      IAsn1Octet asn1Octet = (IAsn1Octet) this.m_helper.ReadObject();
      if (asn1Octet != null)
        this.m_stream = asn1Octet.GetOctetStream();
      else
        goto label_11;
    }
    return num1;
label_11:
    this.m_stream = (Stream) null;
    return num1;
  }

  public override int ReadByte()
  {
    if (this.m_stream == null)
    {
      if (!this.m_first)
        return 0;
      IAsn1Octet asn1Octet = (IAsn1Octet) this.m_helper.ReadObject();
      if (asn1Octet == null)
        return 0;
      this.m_first = false;
      this.m_stream = asn1Octet.GetOctetStream();
    }
    int num;
    while (true)
    {
      num = this.m_stream.ReadByte();
      if (num < 0)
      {
        IAsn1Octet asn1Octet = (IAsn1Octet) this.m_helper.ReadObject();
        if (asn1Octet != null)
          this.m_stream = asn1Octet.GetOctetStream();
        else
          goto label_9;
      }
      else
        break;
    }
    return num;
label_9:
    this.m_stream = (Stream) null;
    return -1;
  }

  public sealed override bool CanRead => !this.m_closed;

  public sealed override bool CanSeek => false;

  public sealed override bool CanWrite => false;

  public override void Close() => this.m_closed = true;

  public sealed override void Flush()
  {
  }

  public sealed override long Length => throw new NotSupportedException();

  public sealed override long Position
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public sealed override long Seek(long offset, SeekOrigin origin)
  {
    throw new NotSupportedException();
  }

  public sealed override void SetLength(long value) => throw new NotSupportedException();

  public sealed override void Write(byte[] buffer, int offset, int count)
  {
    throw new NotSupportedException();
  }
}
