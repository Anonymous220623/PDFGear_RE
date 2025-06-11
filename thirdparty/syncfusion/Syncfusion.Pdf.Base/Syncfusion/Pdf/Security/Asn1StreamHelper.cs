// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1StreamHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1StreamHelper : BaseStream
{
  private byte[] m_empty = new byte[0];
  private int m_originalLength;
  private int m_remaining;

  internal Asn1StreamHelper(Stream stream, int length)
    : base(stream, length)
  {
    this.m_originalLength = length >= 0 ? length : throw new ArgumentException("Invalid length specified");
    this.m_remaining = length;
    if (length != 0)
      return;
    this.SetParentEndOfFileDetect(true);
  }

  internal new int Remaining => this.m_remaining;

  public override int ReadByte()
  {
    if (this.m_remaining == 0)
      return -1;
    int num = this.m_input.ReadByte();
    if (num < 0)
      throw new EndOfStreamException("Invalid length in bytes");
    if (--this.m_remaining == 0)
      this.SetParentEndOfFileDetect(true);
    return num;
  }

  public override int Read(byte[] bytes, int offset, int length)
  {
    if (this.m_remaining == 0)
      return 0;
    int count = Math.Min(length, this.m_remaining);
    int num = this.m_input.Read(bytes, offset, count);
    if (num < 1)
      throw new EndOfStreamException("Object truncated");
    if ((this.m_remaining -= num) == 0)
      this.SetParentEndOfFileDetect(true);
    return num;
  }

  internal void ReadAll(byte[] bytes)
  {
    if (this.m_remaining != bytes.Length)
      throw new ArgumentException("Invalid length in bytes");
    if ((this.m_remaining -= this.Read(this.m_input, bytes, 0, bytes.Length)) != 0)
      throw new EndOfStreamException("Object truncated");
    this.SetParentEndOfFileDetect(true);
  }

  internal byte[] ToArray()
  {
    if (this.m_remaining == 0)
      return this.m_empty;
    byte[] bytes = new byte[this.m_remaining];
    if ((this.m_remaining -= this.Read(this.m_input, bytes, 0, bytes.Length)) != 0)
      throw new EndOfStreamException("Object truncated");
    this.SetParentEndOfFileDetect(true);
    return bytes;
  }

  private int Read(Stream stream, byte[] bytes, int offset, int length)
  {
    int num1;
    int num2;
    for (num1 = 0; num1 < length; num1 += num2)
    {
      num2 = stream.Read(bytes, offset + num1, length - num1);
      if (num2 < 1)
        break;
    }
    return num1;
  }
}
