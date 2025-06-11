// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Convertors.RtfReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.DLS.Convertors;

public class RtfReader
{
  private const byte b_endTag = 125;
  private byte[] m_rtfData;
  private Encoding m_encoding;
  private int m_position;
  private long m_length;

  public byte[] RtfData => this.m_rtfData;

  public Encoding Encoding => this.m_encoding;

  public int Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  public long Length
  {
    get => this.m_length;
    set => this.m_length = value;
  }

  public RtfReader(Stream stream)
  {
    this.m_rtfData = new byte[stream.Length];
    stream.Read(this.m_rtfData, 0, (int) stream.Length);
    this.m_length = (long) this.m_rtfData.Length;
    this.m_encoding = WordDocument.GetEncoding("Windows-1252");
  }

  public char ReadChar()
  {
    char ch = (char) this.m_rtfData[this.m_position];
    ++this.m_position;
    return ch;
  }

  public string ReadImageBytes()
  {
    int num = Array.IndexOf<byte>(this.m_rtfData, (byte) 125, this.m_position);
    string str = this.Encoding.GetString(this.m_rtfData, this.m_position, num - this.m_position);
    this.m_position = num;
    return str.Replace(ControlChar.CrLf, "");
  }

  public void Close()
  {
    this.m_rtfData = (byte[]) null;
    this.m_encoding = (Encoding) null;
  }
}
