// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.NullMessageDigest
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class NullMessageDigest : IMessageDigest
{
  private readonly MemoryStream m_stream = new MemoryStream();

  public string AlgorithmName => "NULL";

  public int MessageDigestSize => 0;

  public int ByteLength => 0;

  public int GetByteLength() => 0;

  public int GetDigestSize() => (int) this.m_stream.Length;

  public void Update(byte b) => this.m_stream.WriteByte(b);

  public void BlockUpdate(byte[] inBytes, int inOff, int len)
  {
    this.m_stream.Write(inBytes, inOff, len);
  }

  public int DoFinal(byte[] outBytes, int outOff)
  {
    byte[] array = this.m_stream.ToArray();
    array.CopyTo((Array) outBytes, outOff);
    this.Reset();
    return array.Length;
  }

  public void Reset() => this.m_stream.SetLength(0L);

  public void Update(byte[] bytes, int offset, int length) => throw new NotImplementedException();
}
