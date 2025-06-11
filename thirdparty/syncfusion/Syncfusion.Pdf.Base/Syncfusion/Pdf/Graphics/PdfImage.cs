// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfImage
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Images;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Xmp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public abstract class PdfImage : PdfShapeElement, IPdfWrapper
{
  private PdfStream m_stream;
  private SizeF m_phisicalDimension;
  private float m_horizontalResolution;
  private float m_verticalResolution;
  protected bool m_softmask;
  private int[] m_matte;
  internal static string m_tiffPath;
  internal static System.IO.Stream m_tiffStream;
  internal PdfCompressionLevel m_tiffCompressionLevel = PdfCompressionLevel.Normal;
  internal XmpMetadata m_imageMetadata;
  internal bool m_enableMetadata;
  internal string m_filePath = string.Empty;
  internal System.IO.Stream m_fileStream;

  public int Height => this.InternalImage.Height;

  public int Width => this.InternalImage.Width;

  public float HorizontalResolution
  {
    get
    {
      float horizontalResolution = (double) this.m_horizontalResolution != 0.0 || this.InternalImage == null ? this.m_horizontalResolution : this.InternalImage.HorizontalResolution;
      if ((double) horizontalResolution <= 0.0)
        horizontalResolution = PdfUnitConvertor.HorizontalResolution;
      return horizontalResolution;
    }
  }

  public float VerticalResolution
  {
    get
    {
      float verticalResolution = (double) this.m_verticalResolution != 0.0 || this.InternalImage == null ? this.m_verticalResolution : this.InternalImage.VerticalResolution;
      if ((double) verticalResolution <= 0.0)
        verticalResolution = PdfUnitConvertor.VerticalResolution;
      return verticalResolution;
    }
  }

  public virtual SizeF PhysicalDimension
  {
    get
    {
      this.m_phisicalDimension = this.GetPointSize((float) this.Width, (float) this.Height, this.HorizontalResolution, this.VerticalResolution);
      return this.m_phisicalDimension;
    }
  }

  internal int[] Matte
  {
    get => this.m_matte;
    set => this.m_matte = value;
  }

  internal abstract Image InternalImage { get; }

  internal PdfStream Stream
  {
    get
    {
      if (this.m_stream == null)
        this.m_stream = new PdfStream();
      return this.m_stream;
    }
  }

  internal bool SoftMask => this.m_softmask;

  public static PdfImage FromFile(string path)
  {
    Image image = path != null ? Image.FromFile(Utils.CheckFilePath(path)) : throw new ArgumentNullException(nameof (path));
    PdfImage pdfImage = PdfImage.FromImage(image);
    if (image.RawFormat.Equals((object) ImageFormat.Tiff))
      PdfImage.m_tiffPath = path;
    return pdfImage;
  }

  public static PdfImage FromStream(System.IO.Stream stream)
  {
    Image image = stream != null ? Image.FromStream(PdfImage.CheckStreamExistance(stream)) : throw new ArgumentNullException(nameof (stream));
    PdfImage pdfImage = PdfImage.FromImage(image);
    if (image.RawFormat.Equals((object) ImageFormat.Tiff))
      PdfImage.m_tiffStream = stream;
    return pdfImage;
  }

  public static PdfImage FromImage(Image image)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    return !(image is Metafile) ? (PdfImage) new PdfBitmap(image) : (PdfImage) new PdfMetafile(image as Metafile);
  }

  public static PdfImage FromRtf(string rtf, float width, PdfImageType type)
  {
    return PdfImage.FromRtf(rtf, width, 0.0f, type);
  }

  public static PdfImage FromRtf(string rtf, float width, float height, PdfImageType type)
  {
    if (rtf == null)
      throw new ArgumentNullException(nameof (rtf));
    SizeF pixelSize = PdfImage.GetPixelSize(width, height);
    PdfImage pdfImage = PdfImage.FromImage(RtfToImage.ConvertToImage(rtf, pixelSize.Width, pixelSize.Height, type) ?? throw new PdfException("Couldn't convert RTF to Image"));
    pdfImage.SetResolution(PdfUnitConvertor.HorizontalResolution, PdfUnitConvertor.VerticalResolution);
    return pdfImage;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_stream;

  internal static System.IO.Stream CheckStreamExistance(System.IO.Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    return stream.Length > 0L ? stream : throw new ArgumentException("The stream can't be empty", nameof (stream));
  }

  public XmpMetadata Metadata
  {
    get
    {
      if (this.m_enableMetadata && this.m_imageMetadata == null)
        this.m_imageMetadata = new ImageMetadataParser(this.m_filePath != string.Empty ? (System.IO.Stream) new FileStream(this.m_filePath, FileMode.Open, FileAccess.Read, FileShare.Read) : this.m_fileStream, this.GetImageType()).TryGetMetadata();
      return this.m_imageMetadata;
    }
    set
    {
      if (!this.m_enableMetadata)
        return;
      this.m_imageMetadata = value;
    }
  }

  private string GetImageType()
  {
    ImageFormat rawFormat = this.InternalImage.RawFormat;
    if (rawFormat.Equals((object) ImageFormat.Jpeg))
      return "Jpeg";
    if (rawFormat.Equals((object) ImageFormat.Png))
      return "Png";
    if (rawFormat.Equals((object) ImageFormat.Gif))
      return "Gif";
    return rawFormat.Equals((object) ImageFormat.Tiff) ? "Tiff" : string.Empty;
  }

  internal void AddMetadata()
  {
    PdfStream xmpStream = this.m_imageMetadata.XmpStream;
    xmpStream["Type"] = (IPdfPrimitive) new PdfName("Metadata");
    xmpStream["Subtype"] = (IPdfPrimitive) new PdfName("XML");
    xmpStream["Length"] = (IPdfPrimitive) new PdfNumber(xmpStream.Data.Length);
    this.Stream["Metadata"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) xmpStream);
  }

  protected static SizeF GetPixelSize(float width, float height)
  {
    float horizontalResolution = PdfUnitConvertor.HorizontalResolution;
    float verticalResolution = PdfUnitConvertor.VerticalResolution;
    return new SizeF(new PdfUnitConvertor(horizontalResolution).ConvertToPixels(width, PdfGraphicsUnit.Point), new PdfUnitConvertor(verticalResolution).ConvertToPixels(height, PdfGraphicsUnit.Point));
  }

  internal abstract void Save();

  internal void SetContent(IPdfPrimitive content)
  {
    if (content == null)
      throw new ArgumentNullException(nameof (content));
    this.m_stream = content is PdfStream ? content as PdfStream : throw new ArgumentException("The content is not a stream.", nameof (content));
  }

  protected override void DrawInternal(PdfGraphics graphics)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    graphics.DrawImage(this, PointF.Empty);
  }

  protected override RectangleF GetBoundsInternal()
  {
    return new RectangleF(PointF.Empty, this.PhysicalDimension);
  }

  protected internal SizeF GetPointSize(float width, float height)
  {
    float horizontalResolution = PdfUnitConvertor.HorizontalResolution;
    float verticalResolution = PdfUnitConvertor.VerticalResolution;
    return this.GetPointSize(width, height, horizontalResolution, verticalResolution);
  }

  protected internal SizeF GetPointSize(
    float width,
    float height,
    float horizontalResolution,
    float verticalResolution)
  {
    PdfUnitConvertor pdfUnitConvertor1 = new PdfUnitConvertor(horizontalResolution);
    PdfUnitConvertor pdfUnitConvertor2 = new PdfUnitConvertor(verticalResolution);
    return new SizeF(pdfUnitConvertor1.ConvertUnits(width, PdfGraphicsUnit.Pixel, PdfGraphicsUnit.Point), pdfUnitConvertor2.ConvertUnits(height, PdfGraphicsUnit.Pixel, PdfGraphicsUnit.Point));
  }

  protected void SetResolution(float horizontalResolution, float verticalResolution)
  {
    this.m_horizontalResolution = horizontalResolution;
    this.m_verticalResolution = verticalResolution;
  }
}
