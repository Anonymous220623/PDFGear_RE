// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.PdfWriter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class PdfWriter : IPdfWriter, IDisposable
{
  internal Stream m_stream;
  private PdfDocumentBase m_document;
  private bool m_cannotSeek;
  private long m_position;
  private long m_length;
  internal bool isCompress;

  private Stream Stream => this.ObtainStream();

  internal PdfWriter(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_stream = stream.CanWrite ? stream : throw new ArgumentException("Can't write to the specified stream", nameof (stream));
    if (stream.CanRead && stream.CanSeek)
      return;
    this.m_cannotSeek = true;
  }

  public void Dispose() => this.Close();

  internal void Close()
  {
    if (this.m_stream == null)
      return;
    this.m_stream.Flush();
    this.m_stream = (Stream) null;
  }

  public PdfDocumentBase Document
  {
    get => this.m_document;
    set
    {
      this.m_document = value != null ? value : throw new ArgumentNullException(nameof (Document));
    }
  }

  public long Position
  {
    get => this.m_cannotSeek ? this.m_position : this.m_stream.Position;
    set
    {
      this.m_stream.Position = value >= 0L ? value : throw new ArgumentOutOfRangeException(nameof (Position), "The stream position can't be less then zero.");
    }
  }

  public long Length => this.m_cannotSeek ? this.m_length : this.m_stream.Length;

  public void Write(IPdfPrimitive pdfObject) => pdfObject.Save((IPdfWriter) this);

  public void Write(long number) => new PdfNumber(number).Save((IPdfWriter) this);

  public void Write(float number) => new PdfNumber(number).Save((IPdfWriter) this);

  public void Write(string text) => this.Write(Encoding.UTF8.GetBytes(text));

  public void Write(char[] text) => this.Write(Encoding.UTF8.GetBytes(text));

  public void Write(byte[] data)
  {
    Stream stream = this.ObtainStream();
    int length = data.Length;
    this.m_length += (long) length;
    this.m_position += (long) length;
    stream.Write(data, 0, length);
  }

  internal void Write(byte[] data, int end)
  {
    Stream stream = this.ObtainStream();
    this.m_length += (long) end;
    this.m_position += (long) end;
    stream.Write(data, 0, end);
  }

  internal Stream ObtainStream() => this.m_stream;
}
