// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.DefaultCompressor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class DefaultCompressor : IPdfCompressor
{
  public string Name => string.Empty;

  public CompressionType Type => CompressionType.None;

  public byte[] Compress(byte[] data)
  {
    return data != null ? data : throw new ArgumentNullException(nameof (data));
  }

  public Stream Compress(Stream inputStream)
  {
    return inputStream != null ? inputStream : throw new ArgumentNullException(nameof (inputStream));
  }

  public byte[] Compress(string data)
  {
    return data != null ? PdfString.StringToByte(data) : throw new ArgumentNullException(nameof (data));
  }

  public byte[] Decompress(string value)
  {
    return value != null ? PdfString.StringToByte(value) : throw new ArgumentNullException(nameof (value));
  }

  public byte[] Decompress(byte[] value)
  {
    return value != null ? value : throw new ArgumentNullException(nameof (value));
  }

  public Stream Decompress(Stream inputStream)
  {
    return inputStream != null ? inputStream : throw new ArgumentNullException(nameof (inputStream));
  }
}
