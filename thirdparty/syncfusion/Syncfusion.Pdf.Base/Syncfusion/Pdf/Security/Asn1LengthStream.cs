// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1LengthStream
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1LengthStream : BaseStream
{
  private int m_byte;
  private bool m_isEndOfFile = true;

  internal Asn1LengthStream(Stream stream, int limit)
    : base(stream, limit)
  {
    this.m_byte = this.RequireByte();
    this.CheckEndOfFile();
  }

  internal void SetEndOfFileOnStart(bool isEOF)
  {
    this.m_isEndOfFile = isEOF;
    if (!this.m_isEndOfFile)
      return;
    this.CheckEndOfFile();
  }

  private bool CheckEndOfFile()
  {
    if (this.m_byte != 0)
      return this.m_byte < 0;
    if (this.RequireByte() != 0)
      throw new IOException("Invalid content");
    this.m_byte = -1;
    this.SetParentEndOfFileDetect(true);
    return true;
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    if (this.m_isEndOfFile || count <= 1)
      return base.Read(buffer, offset, count);
    if (this.m_byte < 0)
      return 0;
    int num = this.m_input.Read(buffer, offset + 1, count - 1);
    if (num <= 0)
      throw new EndOfStreamException();
    buffer[offset] = (byte) this.m_byte;
    this.m_byte = this.RequireByte();
    return num + 1;
  }

  public override int ReadByte()
  {
    if (this.m_isEndOfFile && this.CheckEndOfFile())
      return -1;
    int num = this.m_byte;
    this.m_byte = this.RequireByte();
    return num;
  }

  private int RequireByte()
  {
    int num = this.m_input.ReadByte();
    return num >= 0 ? num : throw new EndOfStreamException();
  }
}
