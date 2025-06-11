// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.IPdfCompressor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal interface IPdfCompressor
{
  CompressionType Type { get; }

  string Name { get; }

  byte[] Compress(byte[] data);

  byte[] Compress(string data);

  Stream Compress(Stream inputStream);

  byte[] Decompress(string value);

  byte[] Decompress(byte[] value);

  Stream Decompress(Stream inputStream);
}
