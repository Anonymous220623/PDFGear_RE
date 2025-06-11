// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.TiffStream
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal class TiffStream
{
  public virtual int Read(object clientData, byte[] buffer, int offset, int count)
  {
    if (!(clientData is Stream stream))
      throw new ArgumentException("Can't get underlying stream to read from");
    return stream.Read(buffer, offset, count);
  }

  public virtual long Seek(object clientData, long offset, SeekOrigin origin)
  {
    if (offset == -1L)
      return -1;
    if (!(clientData is Stream stream))
      throw new ArgumentException("Can't get underlying stream to seek in");
    return stream.Seek(offset, origin);
  }

  public virtual void Close(object clientData)
  {
    if (!(clientData is Stream stream))
      throw new ArgumentException("Can't get underlying stream to close");
    stream.Close();
  }

  public virtual long Size(object clientData)
  {
    return clientData is Stream stream ? stream.Length : throw new ArgumentException("Can't get underlying stream to retrieve size from");
  }
}
