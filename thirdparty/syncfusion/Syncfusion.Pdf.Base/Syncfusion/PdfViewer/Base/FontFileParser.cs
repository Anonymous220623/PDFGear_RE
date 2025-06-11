// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.FontFileParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal abstract class FontFileParser
{
  private readonly byte[] data;
  private readonly Stack<long> beginReadingPositions;
  private long position;

  public bool EndOfFile => this.data == null || this.Position >= (long) this.Length;

  public int Length => this.data.Length;

  public virtual long Position
  {
    get => this.position;
    set => this.position = value;
  }

  public FontFileParser(byte[] data)
  {
    this.position = 0L;
    this.data = data;
    this.beginReadingPositions = new Stack<long>();
  }

  public virtual void BeginReadingBlock() => this.beginReadingPositions.Push(this.position);

  public virtual void EndReadingBlock()
  {
    if (this.beginReadingPositions.Count <= 0)
      return;
    this.position = this.beginReadingPositions.Pop();
  }

  public virtual byte Read()
  {
    byte[] data = this.data;
    long position;
    this.position = (position = this.position) + 1L;
    return data[(int) (IntPtr) position];
  }

  public virtual byte Peek(int skip) => this.data[(int) (IntPtr) (this.position + (long) skip)];

  public int Read(byte[] buffer, int count)
  {
    int num = 0;
    for (int index = 0; index < count && this.Position < (long) this.Length; ++index)
    {
      buffer[index] = this.Read();
      ++num;
    }
    return num;
  }

  public int ReadBE(byte[] buffer, int count)
  {
    int num = 0;
    for (int index = count - 1; index >= 0 && this.Position < (long) this.Length; --index)
    {
      buffer[index] = this.Read();
      ++num;
    }
    return num;
  }

  public virtual void Seek(long offset, SeekOrigin origin)
  {
    switch (origin)
    {
      case SeekOrigin.Begin:
        this.position = offset;
        break;
      case SeekOrigin.Current:
        this.position += offset;
        break;
      case SeekOrigin.End:
        this.position = (long) this.Length - offset;
        break;
    }
  }
}
