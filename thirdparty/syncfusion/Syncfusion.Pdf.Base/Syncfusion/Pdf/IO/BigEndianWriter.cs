// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.BigEndianWriter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class BigEndianWriter
{
  internal const int Int32Size = 4;
  internal const int Int16Size = 2;
  internal const int Int64Size = 8;
  private const float c_fraction = 16384f;
  private readonly Encoding c_encoding = Encoding.GetEncoding("windows-1252");
  private byte[] m_buffer;
  private int m_position;

  public byte[] Data => this.m_buffer;

  public int Position => this.m_position;

  public BigEndianWriter(int capacity) => this.m_buffer = new byte[capacity];

  public void Write(short value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    Array.Reverse((Array) bytes);
    this.Flush(bytes);
  }

  public void Write(ushort value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    Array.Reverse((Array) bytes);
    this.Flush(bytes);
  }

  public void Write(int value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    Array.Reverse((Array) bytes);
    this.Flush(bytes);
  }

  public void Write(uint value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    Array.Reverse((Array) bytes);
    this.Flush(bytes);
  }

  public void Write(string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    this.Flush(this.c_encoding.GetBytes(value));
  }

  public void Write(byte[] value) => this.Flush(value);

  private void Flush(byte[] buff)
  {
    if (buff == null)
      throw new ArgumentNullException(nameof (buff));
    Array.Copy((Array) buff, 0, (Array) this.m_buffer, this.m_position, buff.Length);
    this.m_position += buff.Length;
  }
}
