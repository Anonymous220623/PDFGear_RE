// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.StdFontReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class StdFontReader
{
  private const int HeadersCount = 4;
  private const int LastHeader = 3;
  private byte[] data;
  private StdHeader[] headers;
  private long position;

  public void ReadHeaders()
  {
    long offset = 0;
    this.headers = new StdHeader[4];
    for (int index = 0; index < 3; ++index)
    {
      this.position = offset;
      StdHeader stdHeader = new StdHeader(offset, 6);
      stdHeader.Read(this);
      this.headers[index] = stdHeader;
      offset += (long) stdHeader.HeaderLength + (long) stdHeader.NextHeaderOffset;
    }
    this.headers[3] = new StdHeader((long) (this.data.Length - 2), 2);
  }

  public uint ReadUInt()
  {
    byte[] buffer = new byte[4];
    this.ReadLE(buffer, buffer.Length);
    return BitConverter.ToUInt32(buffer, 0);
  }

  public byte Read()
  {
    byte[] data = this.data;
    long position;
    this.position = (position = this.position) + 1L;
    return data[(int) (IntPtr) position];
  }

  public byte[] ReadData(byte[] data)
  {
    this.data = data;
    this.ReadHeaders();
    List<byte> byteList = new List<byte>(data.Length);
    for (int position = 0; position < this.data.Length; ++position)
    {
      if (!this.IsPositionInHeader((long) position))
        byteList.Add(this.data[position]);
    }
    return byteList.ToArray();
  }

  private bool ReadLE(byte[] buffer, int count)
  {
    for (int index = 0; index < count; ++index)
    {
      try
      {
        buffer[index] = this.Read();
      }
      catch (ArgumentOutOfRangeException ex)
      {
        return false;
      }
    }
    return true;
  }

  private bool IsPositionInHeader(long position)
  {
    foreach (StdHeader header in this.headers)
    {
      if (header.IsPositionInside(position))
        return true;
    }
    return false;
  }
}
