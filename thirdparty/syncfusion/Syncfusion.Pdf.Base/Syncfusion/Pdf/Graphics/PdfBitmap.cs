// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfBitmap
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Compression;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfBitmap : PdfImage, IDisposable
{
  private const ushort c_SoiMarker = 55551;
  private const ushort c_JfifMarker = 57599;
  private const ushort c_SosMarker = 56063;
  private const ushort c_EoiMarker = 55807;
  private Image m_image;
  private ImageFormat m_imageRawFormat;
  private System.IO.Stream m_imageStream;
  private int m_activeFrame;
  private FrameDimension m_frameDimention;
  private PdfMask m_mask;
  private int m_bits;
  private PdfColorSpace m_colorspace;
  private bool m_imageMask;
  private int[] m_frameList;
  private bool m_bDisposed;
  private long m_quality = 100;
  private int check;
  private System.IO.Stream m_internalImageStream;
  private List<byte> m_symbol;
  private PdfJBIG2Compressor m_jbig2Compressor;
  private EncodingType m_encodingType;
  internal bool isCustomQuality;
  private bool m_isCompressedTiff;

  internal int Check
  {
    get => this.check;
    set => this.check = value;
  }

  public int ActiveFrame
  {
    get => this.m_activeFrame;
    set
    {
      this.m_activeFrame = value >= 0 && value < this.FrameCount ? value : throw new ArgumentOutOfRangeException(nameof (ActiveFrame));
      if (this.m_image == null)
        return;
      this.m_image.SelectActiveFrame(this.m_frameDimention, this.m_activeFrame);
      if (!this.m_image.RawFormat.Equals((object) ImageFormat.Tiff))
        return;
      this.m_bits = this.GetBitsPerPixel(this.m_image.PixelFormat);
      this.Check = 0;
    }
  }

  public int FrameCount
  {
    get => this.m_image != null ? this.m_image.GetFrameCount(this.m_frameDimention) : 0;
  }

  public PdfMask Mask
  {
    get => this.m_mask;
    set
    {
      if (value is PdfImageMask)
      {
        PdfImageMask pdfImageMask = value as PdfImageMask;
        if (pdfImageMask.SoftMask)
        {
          Image image = pdfImageMask.Mask.m_image;
          if (this.m_image != null && this.m_image.Size != image.Size)
            throw new ArgumentException("Soft mask must be the same size as drawing image.");
        }
      }
      this.m_mask = value;
    }
  }

  public long Quality
  {
    get => this.m_quality;
    set
    {
      if (value > 100L || value < 0L)
        throw new ArgumentOutOfRangeException(nameof (Quality), (object) value, "The Quality should be in the range from 100 to 0.");
      this.isCustomQuality = true;
      this.m_quality = value;
    }
  }

  internal override Image InternalImage => this.m_image;

  public EncodingType Encoding
  {
    get => this.m_encodingType;
    set => this.m_encodingType = value;
  }

  public PdfBitmap(Image image)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    if (image is Metafile)
      image = (Image) new Bitmap(image);
    if (image.PixelFormat == (PixelFormat) 8207 && Environment.OSVersion.Version.Minor == 1 && Environment.OSVersion.Version.Major == 6)
    {
      this.m_internalImageStream = (System.IO.Stream) new MemoryStream();
      image.Save(this.m_internalImageStream, image.RawFormat);
      this.m_internalImageStream.Position = 0L;
      this.m_colorspace = PdfColorSpace.CMYK;
    }
    this.m_image = image;
    this.m_imageRawFormat = this.m_image.RawFormat;
    this.m_bits = this.GetBitsPerPixel(image.PixelFormat);
    this.m_frameDimention = new FrameDimension(this.m_image.FrameDimensionsList[0]);
    int frameCount = this.FrameCount;
    this.m_frameList = new int[frameCount];
    for (int index = 0; index < frameCount; ++index)
      this.m_frameList[index] = -1;
  }

  public PdfBitmap(string path)
    : this((Image) new Bitmap(Utils.CheckFilePath(path)))
  {
  }

  public PdfBitmap(string path, bool enableMetadata)
    : this((Image) new Bitmap(Utils.CheckFilePath(path)))
  {
    this.m_enableMetadata = enableMetadata;
    this.m_filePath = Utils.CheckFilePath(path);
  }

  public PdfBitmap(System.IO.Stream stream)
    : this(Image.FromStream(PdfImage.CheckStreamExistance(stream)))
  {
  }

  public PdfBitmap(System.IO.Stream stream, bool enableMetadata)
    : this(Image.FromStream(PdfImage.CheckStreamExistance(stream)))
  {
    this.m_enableMetadata = enableMetadata;
    this.m_fileStream = stream;
  }

  ~PdfBitmap() => this.Dispose(false);

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  private void Dispose(bool disposing)
  {
    if (this.m_bDisposed)
      return;
    if (disposing)
      this.InternalImage.Dispose();
    if (this.m_symbol != null)
      this.m_symbol.Clear();
    if (this.m_jbig2Compressor != null)
    {
      this.m_jbig2Compressor.Dispose();
      this.m_jbig2Compressor = (PdfJBIG2Compressor) null;
    }
    this.m_image = (Image) null;
    this.m_bDisposed = true;
  }

  internal override void Save()
  {
    if (this.IsFrameSaved(this.ActiveFrame))
      return;
    this.SetContent((IPdfPrimitive) new PdfStream());
    if (this.Metadata != null)
      this.AddMetadata();
    this.SaveImageByFormat();
    this.SetMask();
    this.SetColorSpace();
    this.SaveRequiredItems();
    this.SaveAddtionalItems();
  }

  private int GetBitsPerPixel(PixelFormat format)
  {
    switch (format)
    {
      case PixelFormat.Format16bppRgb555:
      case PixelFormat.Format16bppRgb565:
      case PixelFormat.Format24bppRgb:
      case PixelFormat.Format32bppRgb:
      case PixelFormat.Format8bppIndexed:
      case PixelFormat.Format16bppArgb1555:
      case PixelFormat.Format32bppPArgb:
      case PixelFormat.Format32bppArgb:
        return 8;
      case PixelFormat.Format1bppIndexed:
      case PixelFormat.Format16bppGrayScale:
        if (this.m_image.Palette.Entries.Length != 2 || this.m_image.Palette.Flags != 0)
          return 1;
        int index = 0;
        return this.m_image.Palette.Entries[index].Name == "ff000000" && this.m_image.Palette.Entries[index + 1].Name == "ffffffff" || this.m_image.Palette.Entries[index].Name == "ffffffff" && this.m_image.Palette.Entries[index + 1].Name == "ff000000" ? 1 : 8;
      case PixelFormat.Format48bppRgb:
      case PixelFormat.Format64bppPArgb:
      case PixelFormat.Format64bppArgb:
        return 16 /*0x10*/;
      default:
        return 8;
    }
  }

  private ImageCodecInfo GetEncoderInfo(string mimeType)
  {
    ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
    for (int index = 0; index < imageEncoders.Length; ++index)
    {
      if (imageEncoders[index].MimeType == mimeType)
        return imageEncoders[index];
    }
    return (ImageCodecInfo) null;
  }

  private void SaveAsJpg()
  {
    ImageCodecInfo encoderInfo = this.GetEncoderInfo("image/jpeg");
    EncoderParameters encoderParams = new EncoderParameters(1);
    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, this.m_quality);
    MemoryStream internalStream = this.Stream.InternalStream;
    if (this.m_internalImageStream != null)
    {
      byte[] buffer = new byte[this.m_internalImageStream.Length];
      this.m_internalImageStream.Position = 0L;
      this.m_internalImageStream.Read(buffer, 0, (int) this.m_internalImageStream.Length - 1);
      this.Stream.InternalStream = new MemoryStream(buffer);
      this.m_internalImageStream.Dispose();
    }
    else
    {
      if (ImageFormat.Jpeg.Equals((object) this.m_image.RawFormat) || ImageFormat.Jpeg.Equals((object) this.m_imageRawFormat))
      {
        if (this.m_quality < 100L)
          this.m_image.Save((System.IO.Stream) internalStream, encoderInfo, encoderParams);
        else
          this.m_image.Save((System.IO.Stream) internalStream, ImageFormat.Jpeg);
      }
      else
        this.m_image.Save((System.IO.Stream) internalStream, encoderInfo, encoderParams);
      this.m_colorspace = this.GetJPGColorSpace(internalStream);
    }
    this.m_bits = 8;
  }

  private void SaveAsRawImage()
  {
    Bitmap image = this.m_image as Bitmap;
    int width = this.m_image.Width;
    int height = this.m_image.Height;
    int bitsPerPixel = this.GetBitsPerPixel(this.m_image.PixelFormat);
    int pixelFormatSize = Image.GetPixelFormatSize(image.PixelFormat);
    int num1 = pixelFormatSize / 8;
    int length1 = num1 * width;
    if (this.m_quality == 100L || image.PixelFormat == PixelFormat.Format1bppIndexed || this.m_encodingType == EncodingType.JBIG2)
    {
      BitmapData bitmapdata = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, this.m_image.PixelFormat);
      switch (num1)
      {
        case 0:
          length1 = width * pixelFormatSize / 8;
          if (width * pixelFormatSize % 8 != 0)
            ++length1;
          if (this.m_image.RawFormat.Equals((object) ImageFormat.Tiff) && pixelFormatSize == 4)
          {
            num1 = bitsPerPixel / 8;
            break;
          }
          break;
        case 1:
          length1 = image.Width;
          break;
        case 3:
          length1 = num1 * width;
          break;
      }
      byte[] numArray1 = new byte[length1];
      byte[] numArray2 = (byte[]) null;
      MemoryStream memoryStream = new MemoryStream();
      IntPtr source = bitmapdata.Scan0;
      int num2 = height;
      int stride = bitmapdata.Stride;
      for (int index = 0; index < num2; ++index)
      {
        Marshal.Copy(source, numArray1, 0, numArray1.Length);
        source = new IntPtr((long) stride + (long) source);
        memoryStream.Write(numArray1, 0, numArray1.Length);
      }
      bool flag = false;
      switch (num1)
      {
        case 0:
          this.m_colorspace = PdfColorSpace.GrayScale;
          numArray2 = memoryStream.GetBuffer();
          if (!this.m_imageMask)
          {
            numArray2 = new PdfCcittEncoder().EncodeData(numArray2, width, height);
            flag = true;
            break;
          }
          break;
        case 1:
          this.m_colorspace = PdfColorSpace.Indexed;
          numArray2 = memoryStream.GetBuffer();
          if (this.m_image.RawFormat.Equals((object) ImageFormat.Tiff) && pixelFormatSize == 4)
          {
            this.m_bits = 4;
            break;
          }
          break;
        case 2:
          this.m_colorspace = PdfColorSpace.RGB;
          numArray2 = memoryStream.GetBuffer();
          break;
        case 3:
          this.m_colorspace = PdfColorSpace.RGB;
          numArray2 = memoryStream.GetBuffer();
          int length2 = (int) memoryStream.Length;
          for (int index1 = 0; index1 < length2; index1 += 3)
          {
            int index2 = index1 + 2;
            byte num3 = numArray2[index2];
            numArray2[index2] = numArray2[index1];
            numArray2[index1] = num3;
          }
          break;
        case 4:
          this.m_colorspace = PdfColorSpace.RGB;
          numArray2 = new byte[width * height * 3];
          int num4 = width * height * 4;
          byte[] buffer = memoryStream.GetBuffer();
          int index3 = 0;
          int index4 = 0;
          for (; index3 < num4; index3 += 4)
          {
            numArray2[index4 + 2] = buffer[index3];
            numArray2[index4 + 1] = buffer[index3 + 1];
            numArray2[index4] = buffer[index3 + 2];
            index4 += 3;
          }
          break;
      }
      MemoryStream internalStream = this.Stream.InternalStream;
      if (this.m_image.RawFormat.Equals((object) ImageFormat.Tiff) && this.m_encodingType == EncodingType.JBIG2)
      {
        if (this.m_frameList.Length == 1)
          this.m_jbig2Compressor = PdfImage.m_tiffStream != null ? new PdfJBIG2Compressor((object) PdfImage.m_tiffStream) : new PdfJBIG2Compressor((object) PdfImage.m_tiffPath);
        if (this.m_jbig2Compressor != null && this.m_jbig2Compressor.SymbolPage != null)
        {
          numArray2 = this.m_jbig2Compressor.SymbolPage.ToArray();
          this.m_symbol = this.m_jbig2Compressor.SymbolArray;
          this.m_colorspace = PdfColorSpace.GrayScale;
        }
      }
      else if (this.m_image.RawFormat.Equals((object) ImageFormat.Tiff) && !flag && this.m_bits != 1)
      {
        this.m_isCompressedTiff = true;
        numArray2 = new PdfZlibCompressor(this.m_tiffCompressionLevel).Compress(numArray2);
        this.Stream.AddFilter("FlateDecode");
        this.Stream.Compress = false;
      }
      internalStream.Write(numArray2, 0, numArray2.Length);
      image.UnlockBits(bitmapdata);
      memoryStream.Close();
    }
    else
    {
      MemoryStream internalStream = this.Stream.InternalStream;
      byte[] buffer = (byte[]) null;
      using (MemoryStream memoryStream1 = new MemoryStream())
      {
        using (MemoryStream memoryStream2 = new MemoryStream())
        {
          Bitmap bitmap1 = new Bitmap((Image) image);
          ImageCodecInfo encoderInfo = this.GetEncoderInfo("image/jpeg");
          EncoderParameters encoderParams = new EncoderParameters(1);
          encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, this.m_quality);
          bitmap1.Save((System.IO.Stream) memoryStream2, ImageFormat.Jpeg);
          bitmap1.Dispose();
          Bitmap bitmap2 = new Bitmap((System.IO.Stream) memoryStream2);
          bitmap2.Save((System.IO.Stream) memoryStream1, encoderInfo, encoderParams);
          buffer = memoryStream1.ToArray();
          bitmap2.Dispose();
        }
      }
      this.m_colorspace = PdfColorSpace.RGB;
      this.Stream["Filter"] = (IPdfPrimitive) new PdfArray()
      {
        (IPdfPrimitive) new PdfName("DCTDecode")
      };
      internalStream.Write(buffer, 0, buffer.Length);
      if (!this.m_image.RawFormat.Equals((object) ImageFormat.Tiff) || bitsPerPixel == this.m_bits)
        return;
      this.m_bits = bitsPerPixel;
    }
  }

  private PdfColorSpace GetJPGColorSpace(MemoryStream imageStream)
  {
    int startIndex1 = 0;
    int num1 = 2;
    byte[] buffer = imageStream.GetBuffer();
    if (BitConverter.ToUInt16(buffer, startIndex1) == (ushort) 55551)
    {
      int startIndex2 = startIndex1 + num1;
      if ((ushort) ((uint) BitConverter.ToUInt16(buffer, startIndex2) & 61695U) == (ushort) 57599)
      {
        ushort uint16_1;
        do
        {
          int startIndex3 = startIndex2 + num1;
          int uint16_2 = (int) BitConverter.ToUInt16(buffer, startIndex3);
          int num2 = uint16_2 >> 8 | uint16_2 << 8 & (int) ushort.MaxValue;
          startIndex2 = startIndex3 + num2;
          uint16_1 = BitConverter.ToUInt16(buffer, startIndex2);
          if (uint16_1 == (ushort) 56063)
          {
            startIndex2 += num1 + 2;
            switch (buffer[startIndex2])
            {
              case 1:
                return this.m_bits == 8 ? PdfColorSpace.RGB : PdfColorSpace.GrayScale;
              case 3:
                return PdfColorSpace.RGB;
              case 4:
                return PdfColorSpace.CMYK;
              default:
                continue;
            }
          }
        }
        while (uint16_1 != (ushort) 55807);
      }
    }
    return this.m_colorspace;
  }

  private void SetMask()
  {
    if (this.m_mask == null)
      return;
    if (this.m_mask is PdfColorMask)
    {
      PdfArray pdfArray = new PdfArray();
      PdfColorMask mask = this.m_mask as PdfColorMask;
      PdfColor startColor = mask.StartColor;
      PdfColor endColor = mask.EndColor;
      pdfArray.Add((IPdfPrimitive) new PdfNumber((int) startColor.R));
      pdfArray.Add((IPdfPrimitive) new PdfNumber((int) startColor.G));
      pdfArray.Add((IPdfPrimitive) new PdfNumber((int) startColor.B));
      pdfArray.Add((IPdfPrimitive) new PdfNumber((int) endColor.R));
      pdfArray.Add((IPdfPrimitive) new PdfNumber((int) endColor.G));
      pdfArray.Add((IPdfPrimitive) new PdfNumber((int) endColor.B));
      this.Stream["Mask"] = (IPdfPrimitive) pdfArray;
    }
    else
    {
      PdfImageMask mask1 = this.m_mask as PdfImageMask;
      PdfBitmap mask2 = mask1.Mask;
      if (this.Matte != null)
        mask2.Matte = this.Matte;
      mask2.m_imageMask = true;
      if (mask1.SoftMask)
      {
        mask2.Save();
        mask2.Stream["ColorSpace"] = (IPdfPrimitive) new PdfName("DeviceGray");
        this.Stream["SMask"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) mask2);
        this.m_softmask = true;
      }
      else
      {
        mask2.Save();
        mask2.Stream["ImageMask"] = (IPdfPrimitive) new PdfBoolean(true);
        this.Stream["Mask"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) mask2);
      }
    }
  }

  private void SaveRequiredItems()
  {
    PdfStream stream = this.Stream;
    PdfName name1 = stream.GetName("XObject");
    PdfDictionary pdfDictionary = (PdfDictionary) stream;
    if (pdfDictionary.ContainsKey("Filter") && !this.m_isCompressedTiff)
    {
      this.Check = 1;
      if (pdfDictionary["Filter"] is PdfArray && ((pdfDictionary["Filter"] as PdfArray)[0] as PdfName).Value == "CCITTFaxDecode")
        this.Check = 2;
    }
    stream["Type"] = (IPdfPrimitive) name1;
    PdfName name2 = stream.GetName("Image");
    stream["Subtype"] = (IPdfPrimitive) name2;
    stream["Width"] = (IPdfPrimitive) new PdfNumber(this.m_image.Width);
    stream["Height"] = (IPdfPrimitive) new PdfNumber(this.m_image.Height);
    stream["BitsPerComponent"] = (IPdfPrimitive) new PdfNumber(this.m_bits);
    if (this.Matte == null)
      return;
    stream["Matte"] = (IPdfPrimitive) new PdfArray(this.Matte);
  }

  internal static Bitmap CreateMaskFromARGBImage(Image argbImage)
  {
    Bitmap bitmap = argbImage as Bitmap;
    int width = argbImage.Width;
    int height = argbImage.Height;
    Bitmap maskFromArgbImage = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
    BitmapData bitmapdata1 = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, argbImage.PixelFormat);
    BitmapData bitmapdata2 = maskFromArgbImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, maskFromArgbImage.PixelFormat);
    IntPtr scan0_1 = bitmapdata2.Scan0;
    IntPtr scan0_2 = bitmapdata1.Scan0;
    int ofs = 0;
    int stride1 = bitmapdata2.Stride;
    int stride2 = bitmapdata1.Stride;
    int num1 = stride1 - maskFromArgbImage.Width;
    int num2 = 0;
    int num3 = 0;
    while (num2 < height)
    {
      int num4 = 0;
      for (int index = width * 4; num4 < index; num4 += 4)
      {
        byte val = Marshal.ReadByte(scan0_2, num3 + num4 + 3);
        if (val != (byte) 0)
          Marshal.WriteByte(scan0_1, ofs, val);
        ++ofs;
      }
      if (stride1 != width)
        ofs += num1;
      ++num2;
      num3 += stride2;
    }
    maskFromArgbImage.UnlockBits(bitmapdata2);
    bitmap.UnlockBits(bitmapdata1);
    return maskFromArgbImage;
  }

  internal static Image CreateMaskFromIndexedImage(Image image)
  {
    Bitmap bitmap = image as Bitmap;
    int width = image.Width;
    int height = image.Height;
    Bitmap fromIndexedImage = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
    BitmapData bitmapdata = fromIndexedImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, fromIndexedImage.PixelFormat);
    IntPtr scan0 = bitmapdata.Scan0;
    int ofs = 0;
    int stride = bitmapdata.Stride;
    int num = stride - fromIndexedImage.Width;
    for (int y = 0; y < height; ++y)
    {
      int x = 0;
      for (int index = width; x < index; ++x)
      {
        byte a = bitmap.GetPixel(x, y).A;
        if (a != (byte) 0)
          Marshal.WriteByte(scan0, ofs, a);
        ++ofs;
      }
      if (stride != width)
        ofs += num;
    }
    fromIndexedImage.UnlockBits(bitmapdata);
    fromIndexedImage.Save("c:/temp/mask.bmp");
    return (Image) fromIndexedImage;
  }

  private void SaveImageByFormat()
  {
    PdfArray filters = new PdfArray();
    ImageFormat rawFormat = this.m_image.RawFormat;
    if (!this.m_enableMetadata && rawFormat.Equals((object) ImageFormat.Gif))
    {
      foreach (PropertyItem propertyItem in this.m_image.PropertyItems)
      {
        if (propertyItem.Id == 20740)
        {
          int index = (int) propertyItem.Value[0];
          if (index >= 0 && this.m_image.Palette.Entries.Length > 0 && this.m_image.Palette.Entries.Length > index && this.m_image.Palette.Entries[index].A == (byte) 0)
          {
            this.m_internalImageStream = (System.IO.Stream) new MemoryStream();
            this.m_image.Save(this.m_internalImageStream, ImageFormat.Png);
            this.m_internalImageStream.Position = 0L;
            this.m_image = (Image) new Bitmap(this.m_internalImageStream);
            this.m_internalImageStream.Dispose();
            rawFormat = this.m_image.RawFormat;
            break;
          }
        }
      }
    }
    if (Image.IsAlphaPixelFormat(this.m_image.PixelFormat))
    {
      this.Mask = (PdfMask) new PdfImageMask(new PdfBitmap((Image) PdfBitmap.CreateMaskFromARGBImage(this.m_image)));
      this.m_imageMask = true;
    }
    if (rawFormat.Equals((object) ImageFormat.Jpeg) || this.m_imageRawFormat.Equals((object) ImageFormat.Jpeg))
      this.SaveImage(filters);
    else if (rawFormat.Equals((object) ImageFormat.Png) || this.m_imageRawFormat.Equals((object) ImageFormat.Png))
      this.SaveImage(filters);
    else if (rawFormat.Equals((object) ImageFormat.Tiff) || this.m_imageRawFormat.Equals((object) ImageFormat.Tiff))
    {
      if (this.m_image.PixelFormat == (PixelFormat) 8207)
      {
        try
        {
          this.SaveImage(filters);
        }
        catch
        {
          this.Stream.InternalStream = new PdfStream().InternalStream;
          this.SaveAsRawImage();
        }
      }
      else
        this.SaveAsRawImage();
    }
    else if (rawFormat.Equals((object) ImageFormat.Bmp) || this.m_imageRawFormat.Equals((object) ImageFormat.Bmp))
      this.SaveImage(filters);
    else if (rawFormat.Equals((object) ImageFormat.Gif) || this.m_imageRawFormat.Equals((object) ImageFormat.Gif))
    {
      this.m_image.SelectActiveFrame(this.m_frameDimention, this.m_activeFrame);
      this.SaveImage(filters);
    }
    else if (rawFormat.Equals((object) ImageFormat.MemoryBmp) || this.m_imageRawFormat.Equals((object) ImageFormat.MemoryBmp))
    {
      if (this.Mask != null)
        this.m_imageMask = true;
      this.SaveImage(filters);
    }
    else
      this.SaveImage(filters);
    if (filters.Count > 0)
      this.Stream["Filter"] = (IPdfPrimitive) filters;
    if (!this.isCustomQuality || this.Stream == null)
      return;
    this.Stream.isCustomQuality = this.isCustomQuality;
  }

  private void SaveImage(PdfArray filters)
  {
    if (this.m_imageMask)
      this.SaveAsRawImage();
    else if (this.m_bits == 1)
    {
      this.SaveAsRawImage();
      filters.Add((IPdfPrimitive) new PdfName("CCITTFaxDecode"));
    }
    else
    {
      this.SaveAsJpg();
      filters.Add((IPdfPrimitive) new PdfName("DCTDecode"));
    }
  }

  private void SetColorSpace()
  {
    PdfStream stream = this.Stream;
    switch (this.m_colorspace)
    {
      case PdfColorSpace.CMYK:
        stream["Decode"] = (IPdfPrimitive) new PdfArray(new float[8]
        {
          1f,
          0.0f,
          1f,
          0.0f,
          1f,
          0.0f,
          1f,
          0.0f
        });
        stream["ColorSpace"] = (IPdfPrimitive) stream.GetName("DeviceCMYK");
        break;
      case PdfColorSpace.GrayScale:
        stream["Decode"] = (IPdfPrimitive) new PdfArray(new float[2]
        {
          0.0f,
          1f
        });
        stream["ColorSpace"] = (IPdfPrimitive) stream.GetName("DeviceGray");
        if (!this.m_image.RawFormat.Equals((object) ImageFormat.Tiff))
          break;
        this.m_bits = 1;
        break;
      case PdfColorSpace.Indexed:
        PdfStream pdfStream = new PdfStream();
        Color[] entries = this.m_image.Palette.Entries;
        int length = entries.Length;
        byte[] numArray = new byte[length * 3];
        for (int index1 = 0; index1 < length; ++index1)
        {
          int index2 = index1 * 3;
          numArray[index2] = entries[index1].R;
          numArray[index2 + 1] = entries[index1].G;
          numArray[index2 + 2] = entries[index1].B;
        }
        pdfStream.Data = numArray;
        stream["ColorSpace"] = (IPdfPrimitive) new PdfArray()
        {
          (IPdfPrimitive) new PdfName("Indexed"),
          (IPdfPrimitive) new PdfName("DeviceRGB"),
          (IPdfPrimitive) new PdfNumber(length - 1),
          (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream)
        };
        break;
      default:
        stream["Decode"] = (IPdfPrimitive) new PdfArray(new float[6]
        {
          0.0f,
          1f,
          0.0f,
          1f,
          0.0f,
          1f
        });
        stream["ColorSpace"] = (IPdfPrimitive) stream.GetName("DeviceRGB");
        break;
    }
  }

  private void SaveAddtionalItems()
  {
    if (this.m_bits == 1 && this.m_image.RawFormat.Equals((object) ImageFormat.Tiff) || this.Check == 2)
    {
      PdfStream stream = this.Stream;
      this.Check = 2;
      stream["Filter"] = (IPdfPrimitive) new PdfName("CCITTFaxDecode");
      PdfDictionary pdfDictionary = new PdfDictionary();
      if (this.m_symbol == null && this.m_encodingType == EncodingType.Default)
      {
        pdfDictionary["K"] = (IPdfPrimitive) new PdfNumber(-1);
        pdfDictionary["Columns"] = (IPdfPrimitive) new PdfNumber(this.Width);
        pdfDictionary["Rows"] = (IPdfPrimitive) new PdfNumber(this.Height);
        pdfDictionary["BlackIs1"] = (IPdfPrimitive) new PdfBoolean(true);
        if (this.m_image.Palette.Entries.Length == 2)
        {
          int index = 0;
          if (this.m_image.Palette.Entries[index].Name == "ff000000" && this.m_image.Palette.Entries[index + 1].Name == "ffffffff")
            pdfDictionary["BlackIs1"] = (IPdfPrimitive) new PdfBoolean(true);
          else if (this.m_image.Palette.Entries[index].Name == "ffffffff" && this.m_image.Palette.Entries[index + 1].Name == "ff000000")
            pdfDictionary["BlackIs1"] = (IPdfPrimitive) new PdfBoolean(false);
        }
      }
      else
      {
        stream["Filter"] = (IPdfPrimitive) new PdfName("JBIG2Decode");
        pdfDictionary["JBIG2Globals"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) new PdfStream()
        {
          Data = this.m_symbol.ToArray()
        });
      }
      stream["DecodeParms"] = (IPdfPrimitive) pdfDictionary;
      stream.Compress = false;
    }
    if (this.Check != 1)
      return;
    PdfStream stream1 = this.Stream;
    stream1["Filter"] = (IPdfPrimitive) new PdfName("DCTDecode");
    stream1["DecodeParms"] = (IPdfPrimitive) new PdfDictionary()
    {
      ["K"] = (IPdfPrimitive) new PdfNumber(-1),
      ["Columns"] = (IPdfPrimitive) new PdfNumber(this.Width),
      ["Rows"] = (IPdfPrimitive) new PdfNumber(this.Height),
      ["BlackIs1"] = (IPdfPrimitive) new PdfBoolean(true)
    };
    stream1.Compress = false;
  }

  private bool IsFrameSaved(int frame)
  {
    int frameCount = this.FrameCount;
    for (int index = 0; index < frameCount; ++index)
    {
      if (this.m_frameList[index] == -1)
      {
        this.m_frameList[index] = frame;
        return false;
      }
      if (this.m_frameList[index] == frame)
        return true;
    }
    return false;
  }
}
