// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.ImageMetadataParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Images.Decoder;
using Syncfusion.Pdf.Xmp;
using System;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class ImageMetadataParser
{
  private Stream m_stream;

  internal ImageMetadataParser(Stream stream, string type)
  {
    stream.Position = 0L;
    switch (type)
    {
      case "Jpeg":
        this.m_stream = (Stream) new JpegDecoder(stream, true).MetadataStream;
        break;
      case "Png":
        this.m_stream = (Stream) new PngDecoder(stream, true).MetadataStream;
        break;
      case "Tiff":
        this.m_stream = (Stream) new TiffMetadataParser(stream).GetMetadata();
        break;
      case "Gif":
        this.m_stream = (Stream) new GifMetadataParser(stream).GetMetadata();
        break;
    }
  }

  internal ImageMetadataParser(Stream stream) => this.m_stream = stream;

  internal XmpMetadata TryGetMetadata()
  {
    if (this.m_stream != null)
    {
      this.m_stream.Position = 0L;
      try
      {
        XmlDocument xmp = new XmlDocument();
        xmp.Load(this.m_stream);
        return new XmpMetadata(xmp);
      }
      catch (Exception ex)
      {
      }
    }
    return (XmpMetadata) null;
  }
}
