// Decompiled with JetBrains decompiler
// Type: RtfReader
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
internal class RtfReader
{
  private const byte m_endTag = 125;
  private byte[] m_rtfData;
  private Encoding m_encoding;
  private int m_position;
  private long m_length;

  internal Encoding Encoding => this.m_encoding;

  internal int Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  internal long Length
  {
    get => this.m_length;
    set => this.m_length = value;
  }

  internal RtfReader(Stream stream)
  {
    this.m_rtfData = new byte[stream.Length];
    stream.Read(this.m_rtfData, 0, (int) stream.Length);
    this.m_length = (long) this.m_rtfData.Length;
    this.m_encoding = Encoding.GetEncoding("Windows-1252");
  }

  internal char ReadChar()
  {
    char ch = (char) this.m_rtfData[this.m_position];
    ++this.m_position;
    return ch;
  }

  internal string ReadImageBytes()
  {
    int num = Array.IndexOf<byte>(this.m_rtfData, (byte) 125, this.m_position);
    string str = this.Encoding.GetString(this.m_rtfData, this.m_position, num - this.m_position);
    this.m_position = num;
    return str.Replace("\r\n", "");
  }

  internal void Close()
  {
    this.m_rtfData = (byte[]) null;
    this.m_encoding = (Encoding) null;
  }
}
