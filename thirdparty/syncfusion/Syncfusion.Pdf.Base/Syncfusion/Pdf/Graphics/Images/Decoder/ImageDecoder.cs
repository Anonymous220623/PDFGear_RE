// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Decoder.ImageDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Decoder;

internal abstract class ImageDecoder
{
  private Stream m_internalStream;
  private int m_width;
  private int m_height;
  private byte[] m_imageData;
  private int m_bitsPerComponent;
  private ImageType m_format;
  private float m_horizontalResolution;
  private float m_verticalResolution;
  private float m_jpegDecoderOrientationAngle;
  private MemoryStream m_metadataStream;

  public Stream InternalStream
  {
    get => this.m_internalStream;
    set => this.m_internalStream = value;
  }

  public int Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  public int Height
  {
    get => this.m_height;
    set => this.m_height = value;
  }

  public byte[] ImageData
  {
    get => this.m_imageData;
    set => this.m_imageData = value;
  }

  public int BitsPerComponent
  {
    get => this.m_bitsPerComponent;
    set => this.m_bitsPerComponent = value;
  }

  public ImageType Format
  {
    get => this.m_format;
    set => this.m_format = value;
  }

  public float HorizontalResolution
  {
    get => this.m_horizontalResolution;
    set => this.m_horizontalResolution = value;
  }

  public float VerticalResolution
  {
    get => this.m_verticalResolution;
    set => this.m_verticalResolution = value;
  }

  internal float JpegDecoderOrientationAngle
  {
    get => this.m_jpegDecoderOrientationAngle;
    set => this.m_jpegDecoderOrientationAngle = value;
  }

  internal MemoryStream MetadataStream
  {
    get => this.m_metadataStream;
    set => this.m_metadataStream = value;
  }

  public Size Size => new Size(this.Width, this.Height);

  public static bool TryGetDecoder(Stream stream, bool enableMetadata, out ImageDecoder decoder)
  {
    decoder = (ImageDecoder) null;
    if (StreamExtensions.IsPng(stream))
      decoder = (ImageDecoder) new PngDecoder(stream, enableMetadata);
    else if (StreamExtensions.IsJpeg(stream))
      decoder = (ImageDecoder) new JpegDecoder(stream, enableMetadata);
    return decoder != null;
  }

  protected abstract void Initialize();

  internal abstract PdfStream GetImageDictionary();
}
