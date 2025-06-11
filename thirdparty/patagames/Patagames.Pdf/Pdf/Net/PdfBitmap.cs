// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfBitmap
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Exceptions;
using System;
using System.Drawing;
using System.Drawing.Imaging;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Encapsulates a Device-Independent bitmap, which consists of the pixel data for a graphics image and its attributes. A <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> is an object used to work with images defined by pixel data.
/// </summary>
public class PdfBitmap : IDisposable
{
  private Bitmap _bitmap;
  private bool _forceAlphaChannel;

  private static int[] GetPalette(Bitmap image)
  {
    int[] palette = new int[image.Palette.Entries.Length];
    for (int index = 0; index < palette.Length; ++index)
      palette[index] = ((Color) image.Palette.Entries.GetValue(index)).ToArgb();
    return palette;
  }

  private static void GetPdfFormat(Bitmap image, out BitmapFormats pdfFormat, out int[] palette)
  {
    palette = (int[]) null;
    switch (image.PixelFormat)
    {
      case PixelFormat.Format24bppRgb:
        pdfFormat = BitmapFormats.FXDIB_Rgb;
        break;
      case PixelFormat.Format32bppRgb:
        pdfFormat = BitmapFormats.FXDIB_Rgb32;
        break;
      case PixelFormat.Format1bppIndexed:
        pdfFormat = BitmapFormats.FXDIB_1bppRgb;
        palette = PdfBitmap.GetPalette(image);
        break;
      case PixelFormat.Format8bppIndexed:
        pdfFormat = BitmapFormats.FXDIB_8bppRgb;
        palette = PdfBitmap.GetPalette(image);
        break;
      case PixelFormat.Format32bppPArgb:
      case PixelFormat.Format32bppArgb:
        pdfFormat = BitmapFormats.FXDIB_Argb;
        break;
      default:
        throw new UnsupportedImageFormatException();
    }
  }

  /// <summary>Gets the image that represents the PdfBitmap</summary>
  public Image Image
  {
    get
    {
      if (this._bitmap != null)
        return (Image) this._bitmap;
      if (this.IsDisposed)
        throw new ObjectDisposedException(nameof (PdfBitmap));
      int bpp = this.BPP;
      BitmapFormats format1 = this.Format;
      FS_COLOR[] fsColorArray = (FS_COLOR[]) null;
      PixelFormat format2;
      switch (format1)
      {
        case BitmapFormats.FXDIB_Invalid:
        case BitmapFormats.FXDIB_Rgba:
        case BitmapFormats.FXDIB_1bppCmyk:
        case BitmapFormats.FXDIB_Cmyk:
        case BitmapFormats.FXDIB_Cmyka:
          throw new UnsupportedImageFormatException();
        case BitmapFormats.FXDIB_1bppRgb:
          format2 = PixelFormat.Format1bppIndexed;
          fsColorArray = this.GetPalette();
          break;
        case BitmapFormats.FXDIB_8bppRgb:
        case BitmapFormats.FXDIB_8bppMask:
        case BitmapFormats.FXDIB_8bppRgba:
          format2 = PixelFormat.Format8bppIndexed;
          fsColorArray = this.GetPalette();
          break;
        case BitmapFormats.FXDIB_Rgb:
          if (bpp == 32 /*0x20*/)
          {
            format2 = PixelFormat.Format32bppArgb;
            break;
          }
          if (bpp != 24)
            throw new UnsupportedImageFormatException();
          format2 = PixelFormat.Format24bppRgb;
          break;
        case BitmapFormats.FXDIB_Rgb32:
          format2 = this._forceAlphaChannel ? PixelFormat.Format32bppArgb : PixelFormat.Format32bppRgb;
          break;
        case BitmapFormats.FXDIB_1bppMask:
          format2 = PixelFormat.Format1bppIndexed;
          fsColorArray = new FS_COLOR[2]
          {
            new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue),
            new FS_COLOR((int) byte.MaxValue, 0, 0, 0)
          };
          break;
        case BitmapFormats.FXDIB_Argb:
          format2 = PixelFormat.Format32bppArgb;
          break;
        default:
          if (bpp == 48 /*0x30*/)
          {
            format2 = PixelFormat.Format48bppRgb;
            break;
          }
          if (bpp != 64 /*0x40*/)
            throw new UnsupportedImageFormatException();
          format2 = PixelFormat.Format64bppArgb;
          break;
      }
      try
      {
        this._bitmap = new Bitmap(this.Width, this.Height, this.Stride, format2, this.Buffer);
      }
      catch (PlatformNotSupportedException ex)
      {
        throw;
      }
      this._disposeBitmap += new EventHandler(this.DisposeBitmap);
      if (fsColorArray != null)
      {
        int length1 = this._bitmap.Palette.Entries.Length;
        int length2 = fsColorArray.Length;
        ColorPalette palette = this._bitmap.Palette;
        for (int index = 0; index < length1 && index < length2; ++index)
          palette.Entries.SetValue((object) Color.FromArgb(fsColorArray[index].A, fsColorArray[index].R, fsColorArray[index].G, fsColorArray[index].B), index);
        this._bitmap.Palette = palette;
      }
      return (Image) this._bitmap;
    }
  }

  /// <summary>
  /// Creates an <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> from the specified file.
  /// </summary>
  /// <param name="path">A string that contains the name of the file from which to create the <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> this method creates.</returns>
  /// <exception cref="T:System.OutOfMemoryException">The file does not have a valid image format. -or- GDI+ does not support the pixel format of the file.</exception>
  /// <exception cref="T:System.IO.FileNotFoundException">The specified file does not exist.</exception>
  /// <exception cref="T:System.ArgumentException">Filename is a System.Uri.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnsupportedImageFormatException">Can't convert System.Drawing.Bitmap to PdfBitmap</exception>
  public static PdfBitmap FromFile(string path)
  {
    return PdfBitmap.FromBitmap(Image.FromFile(path) as Bitmap);
  }

  /// <summary>
  /// Creates an <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> from the specified <paramref name="image" />.
  /// </summary>
  /// <param name="image">System.Drawing.Bitmap</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> this method creates.</returns>
  /// <exception cref="T:System.OutOfMemoryException">The file does not have a valid image format. -or- GDI+ does not support the pixel format of the file.</exception>
  /// <exception cref="T:System.IO.FileNotFoundException">The specified file does not exist.</exception>
  /// <exception cref="T:System.ArgumentException">Filename is a System.Uri.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnsupportedImageFormatException">Can't convert System.Drawing.Bitmap to PdfBitmap</exception>
  public static PdfBitmap FromBitmap(Bitmap image)
  {
    BitmapFormats pdfFormat;
    int[] palette;
    PdfBitmap.GetPdfFormat(image, out pdfFormat, out palette);
    BitmapData bitmapdata = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
    PdfBitmap pdfBitmap = new PdfBitmap(image.Width, image.Height, pdfFormat, bitmapdata.Scan0, bitmapdata.Stride);
    image.UnlockBits(bitmapdata);
    if (palette != null)
      pdfBitmap.Palette = palette;
    pdfBitmap._bitmap = image;
    return pdfBitmap;
  }

  private void DisposeBitmap(object sender, EventArgs e)
  {
    if (this._bitmap != null)
      this._bitmap.Dispose();
    this._bitmap = (Bitmap) null;
    this._disposeBitmap -= new EventHandler(this.DisposeBitmap);
  }

  /// <summary>
  /// Fill a rectangle area in an PdfBitmap specified by a <see cref="T:System.Drawing.Rectangle" /> structure.
  /// </summary>
  /// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle to fill.</param>
  /// <param name="color">A <see cref="T:Patagames.Pdf.FS_COLOR" /> structure that represents the color.</param>
  /// <remarks><para>This method set the color and (optionally) alpha value in specified region of the bitmap.</para>
  /// <note>
  /// If alpha channel is used, this function does NOT composite the background with the source
  /// color, instead the background will be replaced by the source color and alpha.</note>
  /// <para>If alpha channel is not used, the "alpha" parameter is ignored.</para>
  /// </remarks>
  public void FillRect(Rectangle rect, FS_COLOR color)
  {
    this.FillRect(rect.Left, rect.Top, rect.Width, rect.Height, color);
  }

  private event EventHandler _disposeBitmap;

  private FS_COLOR[] GetPalette()
  {
    if (this.PaletteSize <= 0)
      return (FS_COLOR[]) null;
    FS_COLOR[] palette = new FS_COLOR[this.PaletteSize];
    for (int index = 0; index < palette.Length; ++index)
      palette[index] = this.GetPaletteColorByIndex(index);
    return palette;
  }

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the control has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>
  /// Gets the Pdfium.Net SDK handle that the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> is bound to.
  /// </summary>
  /// <value>A handle to the device independent bitmap.</value>
  public IntPtr Handle { get; private set; }

  /// <summary>
  /// Gets pointer to an array of bytes that contains the pixel data.
  /// </summary>
  /// <remarks>Applications can use this function to get the bitmap buffer pointer, then manipulate any color and/or alpha values for any pixels in the <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</remarks>
  public IntPtr Buffer => Pdfium.FPDFBitmap_GetBuffer(this.Handle);

  /// <summary>
  /// Gets the width, in pixels, of this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  /// <value>The width, in pixels, of this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</value>
  public int Width => Pdfium.FPDFBitmap_GetWidth(this.Handle);

  /// <summary>
  /// Gets the height, in pixels, of this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  /// <value>The height, in pixels, of this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</value>
  public int Height => Pdfium.FPDFBitmap_GetHeight(this.Handle);

  /// <summary>
  /// Gets number of bytes for each scan line in the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> <see cref="P:Patagames.Pdf.Net.PdfBitmap.Buffer" />.
  /// </summary>
  /// <value>Integer that specifies the byte offset between the beginning of one scan line and the next. This is usually (but not necessarily) the number of bytes in the pixel format (for example, 2 for 16 bits per pixel) multiplied by the width of the bitmap. The value passed to this parameter must be a multiple of four.</value>
  public int Stride => Pdfium.FPDFBitmap_GetStride(this.Handle);

  /// <summary>
  /// Gets the pixel format for this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  /// <value>A <see cref="T:Patagames.Pdf.Enums.BitmapFormats" /> that represents the pixel format for this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</value>
  public BitmapFormats Format => Pdfium.FPDFBitmap_GetFormat(this.Handle);

  /// <summary>
  /// Gets number of bytes for each scan line in the bitmap <see cref="P:Patagames.Pdf.Net.PdfBitmap.Buffer" />.
  /// </summary>
  /// <remarks>This property is obsolete and will be removed in a future release of the Pdfium .Net SDK. Please use <see cref="P:Patagames.Pdf.Net.PdfBitmap.Stride" /> instead.</remarks>
  [Obsolete("This property is obsolete and will be removed in a future release of the Pdfium .Net SDK. Please use Stride instead.", false)]
  public int Pitch => this.Stride;

  /// <summary>
  /// Gets the number of elements in color palette used for this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  /// <value>The number of elements in color palette used for this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</value>
  public int PaletteSize => Pdfium.FFPDFBitmap_GetPaletteSize(this.Handle);

  /// <summary>
  /// Gets or sets the color palette used for this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  /// <remarks>
  /// This property returns a copy of an array of the color palette used by this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </remarks>
  public int[] Palette
  {
    get => Pdfium.FFPDFBitmap_GetPalette(this.Handle);
    set => Pdfium.FFPDFBitmap_CopyPalette(this.Handle, value);
  }

  /// <summary>
  /// Gets the color depth, in number of bits per pixel, of this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  /// <value>The color depth of this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</value>
  public int BPP => Pdfium.FFPDFBitmap_GetBPP(this.Handle);

  /// <summary>
  /// Gets a flag indicating whether this <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> is a transparency mask.
  /// </summary>
  /// <value>A boolean value indicating whether this <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> is a transparency mask(true) or not(false).</value>
  public bool IsAlphaMask => Pdfium.FFPDFBitmap_IsAlphaMask(this.Handle);

  /// <summary>
  /// Gets a flag indicating whether this <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> contains an alpha channel.
  /// </summary>
  /// <value>A boolean value indicating whether this <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> contains an alpha channel(true) or not(false).</value>
  public bool HasAlpha => Pdfium.FFPDFBitmap_HasAlpha(this.Handle);

  /// <summary>
  /// Gets a flag indicating whether this <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> is opaque or not.
  /// </summary>
  /// <value>A boolean value indicating whether this <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> is opaque(true) or not(false).</value>
  public bool IsOpaque => Pdfium.FFPDFBitmap_IsOpaqueImage(this.Handle);

  /// <summary>
  /// Gets a flag indicating whether this <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> is a CMYK image or not.
  /// </summary>
  /// <value>Gets a boolean value indicating whether this <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> is a CMYK image(true) or not(false).</value>
  public bool IsCmyk => Pdfium.FFPDFBitmap_IsCmykImage(this.Handle);

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class with the specified <paramref name="width" />, <paramref name="height" /> and <paramref name="isUseAlpha" /> flag.
  /// </summary>
  /// <param name="width">The width, in pixels, of the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />. </param>
  /// <param name="height">The height, in pixels, of the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />. </param>
  /// <param name="isUseAlpha">A flag indicating whether alpha channel is used. True for using alpha, false for not using.</param>
  /// <param name="forceAlphaChannel">Reserved for internal use. Must be false.</param>
  /// <remarks>
  /// <para>An <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> created by this constructor always use 4 byte per pixel.</para>
  /// <para>The first byte of a pixel is always double word aligned. Each pixel contains red (R), green (G), blue (B) and optionally alpha (A) values.</para>
  /// <para>The byte order is BGRx (the last byte unused if no alpha channel) or BGRA. The pixels in a horizontal line
  /// (also called scan line) are stored side by side, with left most pixel stored first (with lower
  /// memory address)</para>.
  /// <para>Each scan line uses <paramref name="width" />*4 bytes. Scan lines are stored one after another,
  /// with top most scan line stored first. There is no gap between adjacent scan lines.</para>
  /// <para>This constructor allocates enough memory for holding all pixels in the bitmap, but it doesn't
  /// initialize the buffer. Applications can use <see cref="O:Patagames.Pdf.Net.PdfBitmap.FillRect" /> to fill the bitmap using any
  /// color.</para>
  /// </remarks>
  public PdfBitmap(int width, int height, bool isUseAlpha, bool forceAlphaChannel = false)
  {
    this._forceAlphaChannel = forceAlphaChannel;
    this.Handle = Pdfium.FPDFBitmap_Create(width, height, isUseAlpha);
    if (this.Handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class with the specified <paramref name="width" />, <paramref name="height" />, pixel <paramref name="format" />, and external buffer.
  /// </summary>
  /// <param name="width">The width, in pixels, of the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />. </param>
  /// <param name="height">The height, in pixels, of the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />. </param>
  /// <param name="format">The pixel format for the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</param>
  /// <param name="scan0">A pointer to the first byte of first scan line, for external buffer only. If this parameter is IntPtr.Zero, then the SDK will create its own buffer.</param>
  /// <param name="stride">Integer that specifies the byte offset between the beginning of one scan line and the next.
  /// This is usually (but not necessarily) the number of bytes in the pixel format (for example, 2 for 16 bits per pixel) multiplied by the width of the bitmap.
  /// The value passed to this parameter must be a multiple of four. Used for external buffer only.</param>
  /// <remarks><para>Similar to <see cref="M:Patagames.Pdf.Net.PdfBitmap.#ctor(System.Int32,System.Int32,System.Boolean,System.Boolean)" /> function, with support for more formats and an external buffer.</para>
  /// <para>If external scanline buffer is used, then the application should destroy the buffer by
  /// itself. <see cref="M:Patagames.Pdf.Net.PdfBitmap.Dispose" /> method will not destroy the buffer.</para>
  /// </remarks>
  public PdfBitmap(int width, int height, FXDIBFormats format, IntPtr scan0, int stride)
  {
    this.Handle = Pdfium.FPDFBitmap_CreateEx(width, height, format, scan0, stride);
    if (this.Handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class with the specified <paramref name="width" />, <paramref name="height" />, pixel <paramref name="format" />, and external buffer.
  /// </summary>
  /// <param name="width">The width, in pixels, of the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />. </param>
  /// <param name="height">The height, in pixels, of the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />. </param>
  /// <param name="format">The pixel format for the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</param>
  /// <param name="scan0">Pointer to an array of bytes that contains the pixel data.</param>
  /// <param name="stride">Integer that specifies the byte offset between the beginning of one scan line and the next.
  /// This is usually (but not necessarily) the number of bytes in the pixel format (for example, 2 for 16 bits per pixel) multiplied by the width of the bitmap.
  /// The value passed to this parameter must be a multiple of four. Used for external buffer only.</param>
  /// <remarks><para>Similar to <see cref="M:Patagames.Pdf.Net.PdfBitmap.#ctor(System.Int32,System.Int32,System.Boolean,System.Boolean)" /> function, with support for more formats and an external buffer.</para>
  /// <para>If external scanline buffer is used, then the application should destroy the buffer by
  /// itself. <see cref="M:Patagames.Pdf.Net.PdfBitmap.Dispose" /> method will not destroy the buffer.</para>
  /// </remarks>
  public PdfBitmap(int width, int height, BitmapFormats format, IntPtr scan0, int stride)
  {
    this.Handle = Pdfium.FPDFBitmap_CreateEx(width, height, format, scan0, stride);
    if (this.Handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class with the specified <paramref name="width" />, <paramref name="height" /> and pixel <paramref name="format" />.
  /// </summary>
  /// <param name="width">The width, in pixels, of the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />. </param>
  /// <param name="height">The height, in pixels, of the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />. </param>
  /// <param name="format">The pixel format for the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</param>
  /// <remarks><para>Similar to <see cref="M:Patagames.Pdf.Net.PdfBitmap.#ctor(System.Int32,System.Int32,System.Boolean,System.Boolean)" /> function, with support for more formats.</para></remarks>
  public PdfBitmap(int width, int height, FXDIBFormats format)
    : this(width, height, format, IntPtr.Zero, 0)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class with the specified <paramref name="width" />, <paramref name="height" /> and pixel <paramref name="format" />.
  /// </summary>
  /// <param name="width">The width, in pixels, of the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />. </param>
  /// <param name="height">The height, in pixels, of the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />. </param>
  /// <param name="format">The pixel format for the new <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</param>
  /// <remarks><para>Similar to <see cref="M:Patagames.Pdf.Net.PdfBitmap.#ctor(System.Int32,System.Int32,System.Boolean,System.Boolean)" /> function, with support for more formats.</para></remarks>
  public PdfBitmap(int width, int height, BitmapFormats format)
    : this(width, height, format, IntPtr.Zero, 0)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class with the specified bitmap descriptor.
  /// </summary>
  /// <param name="handle">A handle to the device independent bitmap.</param>
  public PdfBitmap(IntPtr handle)
  {
    this.Handle = handle;
    if (this.Handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
  }

  /// <summary>
  /// Releases all resources used by this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>
  /// Releases all resources used by this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    if (this.Handle != IntPtr.Zero)
      Pdfium.FPDFBitmap_Destroy(this.Handle);
    this.Handle = IntPtr.Zero;
    if (this._disposeBitmap != null)
      this._disposeBitmap((object) this, EventArgs.Empty);
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Finalize object</summary>
  ~PdfBitmap()
  {
    if (PdfCommon.IsCheckForMemoryLeaks && !this.IsDisposed)
      throw new MemoryLeakException(nameof (PdfBitmap));
  }

  /// <summary>
  /// Fills the rectangular area in the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> specified by a pair of coordinates, width and height.
  /// </summary>
  /// <param name="left">The left side position. Starting from 0 at the left-most pixel.</param>
  /// <param name="top">The top side position. Starting from 0 at the top-most scan line.</param>
  /// <param name="width">Number of pixels to be filled in each scan line</param>
  /// <param name="height">Number of scan lines to be filled.</param>
  /// <param name="color">A <see cref="T:Patagames.Pdf.FS_COLOR" /> structure that represents the color.</param>
  /// <remarks><para>This method set the color and (optionally) alpha value in specified region of the bitmap.</para>
  /// <note type="note">If alpha channel is used, this function does NOT composite the background with the source
  /// color, instead the background will be replaced by the source color and alpha.</note>
  /// <para>If alpha channel is not used, the "alpha" parameter is ignored.</para>
  /// </remarks>
  public void FillRect(int left, int top, int width, int height, FS_COLOR color)
  {
    Pdfium.FPDFBitmap_FillRect(this.Handle, left, top, width, height, color.ToArgb());
  }

  /// <summary>
  /// Fills the rectangular area in the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> specified by a pair of coordinates, width and height.
  /// </summary>
  /// <param name="left">The left side position. Starting from 0 at the left-most pixel.</param>
  /// <param name="top">The top side position. Starting from 0 at the top-most scan line.</param>
  /// <param name="width">Number of pixels to be filled in each scan line</param>
  /// <param name="height">Number of scan lines to be filled.</param>
  /// <param name="color">A <see cref="T:Patagames.Pdf.FS_COLOR" /> structure that represents the color.</param>
  /// <param name="blendType">A blend mode to be used in the transparent imaging model.</param>
  /// <remarks><para>This method set the color and (optionally) alpha value in specified region of the bitmap.</para>
  /// <note type="note">If alpha channel is used, this function does NOT composite the background with the source
  /// color, instead the background will be replaced by the source color and alpha.</note>
  /// <para>If alpha channel is not used, the "alpha" parameter is ignored.</para>
  /// </remarks>
  public void FillRect(
    int left,
    int top,
    int width,
    int height,
    FS_COLOR color,
    BlendTypes blendType)
  {
    if (width == 0 || height == 0)
      return;
    using (PdfBitmap pdfBitmap = new PdfBitmap(width, height, true))
    {
      pdfBitmap.FillRect(0, 0, width, height, color);
      Pdfium.FPDFBitmap_CompositeBitmap(this.Handle, left, top, width, height, pdfBitmap.Handle, 0, 0, blendType);
    }
  }

  /// <summary>
  /// Fills the rectangular area in the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> specified by a pair of coordinates, width and height.
  /// </summary>
  /// <param name="left">The left side position. Starting from 0 at the left-most pixel.</param>
  /// <param name="top">The top side position. Starting from 0 at the top-most scan line.</param>
  /// <param name="width">Number of pixels to be filled in each scan line</param>
  /// <param name="height">Number of scan lines to be filled.</param>
  /// <param name="argb">Represents the color in 8888 ARGB format.</param>
  /// <remarks><para>This method set the color and (optionally) alpha value in specified region of the bitmap.</para>
  /// <note type="note">If alpha channel is used, this function does NOT composite the background with the source
  /// color, instead the background will be replaced by the source color and alpha.</note>
  /// <para>If alpha channel is not used, the "alpha" parameter is ignored.</para>
  /// </remarks>
  public void FillRectEx(int left, int top, int width, int height, int argb)
  {
    Pdfium.FPDFBitmap_FillRect(this.Handle, left, top, width, height, argb);
  }

  /// <summary>
  /// Fills the rectangular area in the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> specified by a pair of coordinates, width and height.
  /// </summary>
  /// <param name="left">The left side position. Starting from 0 at the left-most pixel.</param>
  /// <param name="top">The top side position. Starting from 0 at the top-most scan line.</param>
  /// <param name="width">Number of pixels to be filled in each scan line</param>
  /// <param name="height">Number of scan lines to be filled.</param>
  /// <param name="argb">Represents the color in ARGB format</param>
  /// <param name="blendType">A blend mode to be used in the transparent imaging model.</param>
  /// <remarks><para>This method set the color and (optionally) alpha value in specified region of the bitmap.</para>
  /// <note type="note">If alpha channel is used, this function does NOT composite the background with the source
  /// color, instead the background will be replaced by the source color and alpha.</note>
  /// <para>If alpha channel is not used, the "alpha" parameter is ignored.</para>
  /// </remarks>
  public void FillRectEx(
    int left,
    int top,
    int width,
    int height,
    int argb,
    BlendTypes blendType)
  {
    if (width <= 0 || height <= 0)
      return;
    using (PdfBitmap pdfBitmap = new PdfBitmap(width, height, true))
    {
      pdfBitmap.FillRectEx(0, 0, width, height, argb);
      Pdfium.FPDFBitmap_CompositeBitmap(this.Handle, left, top, width, height, pdfBitmap.Handle, 0, 0, blendType);
    }
  }

  /// <summary>
  /// Fills the rectangular area in the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> specified by a <see cref="T:Patagames.Pdf.FX_RECT" /> structure.
  /// </summary>
  /// <param name="rect">A <see cref="T:Patagames.Pdf.FX_RECT" /> structure that represents the rectangle to fill.</param>
  /// <param name="color">A <see cref="T:Patagames.Pdf.FS_COLOR" /> structure that represents the color.</param>
  /// <remarks><para>This method set the color and (optionally) alpha value in specified region of the bitmap.</para>
  /// <note type="note">If alpha channel is used, this function does NOT composite the background with the source
  /// color, instead the background will be replaced by the source color and alpha.</note>
  /// <para>If alpha channel is not used, the "alpha" parameter is ignored.</para>
  /// </remarks>
  public void FillRect(FX_RECT rect, FS_COLOR color)
  {
    this.FillRect(rect.left, rect.top, rect.Width, rect.Height, color);
  }

  /// <summary>
  /// Returns the <see cref="T:Patagames.Pdf.FS_COLOR" /> in the color palette for a certain position.
  /// </summary>
  /// <param name="index">An index of the color in the color palette.</param>
  /// <returns>A <see cref="T:Patagames.Pdf.FS_COLOR" /> structure that represents the color in the color palette.</returns>
  public FS_COLOR GetPaletteColorByIndex(int index)
  {
    return new FS_COLOR(Pdfium.FFPDFBitmap_GetPaletteEntry(this.Handle, index));
  }

  /// <summary>
  /// Set given <see cref="T:Patagames.Pdf.FS_COLOR" /> to a certain position in the color palette of this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  /// <param name="index">An index of the <paramref name="color" /> in the color palette.</param>
  /// <param name="color">A color to be set.</param>
  public void SetPaletteEntryByIndex(int index, FS_COLOR color)
  {
    Pdfium.FFPDFBitmap_SetPaletteEntry(this.Handle, index, color.ToArgb());
  }

  /// <summary>
  /// Clone this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  /// <param name="clip">Clipping area.</param>
  /// <returns>A <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> containing copied pixels.</returns>
  /// <remarks>You should always call the <see cref="O:Patagames.Pdf.Net.PdfBitmap.Dispose" /> method to release the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> and related resources created by the <see cref="M:Patagames.Pdf.Net.PdfBitmap.Clone(Patagames.Pdf.FX_RECT)" /> method.</remarks>
  public PdfBitmap Clone(FX_RECT clip = null)
  {
    return new PdfBitmap(Pdfium.FFPDFBitmap_Clone(this.Handle, clip));
  }

  /// <summary>
  /// Clone and convert this <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> to specified <paramref name="format" />.
  /// </summary>
  /// <param name="format">The pixel format to convert <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> to.</param>
  /// <returns>A <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> containing copied pixels.</returns>
  /// <remarks>You should always call the <see cref="O:Patagames.Pdf.Net.PdfBitmap.Dispose" /> method to release the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> and related resources created by the <see cref="M:Patagames.Pdf.Net.PdfBitmap.Convert(Patagames.Pdf.Enums.BitmapFormats)" /> method.</remarks>
  public PdfBitmap Convert(BitmapFormats format)
  {
    return new PdfBitmap(Pdfium.FFPDFBitmap_CloneConvert(this.Handle, format));
  }

  /// <summary>
  /// Clone and stretch this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  /// <param name="width">The width, in pixels, of the resulting <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</param>
  /// <param name="height">The height, in pixels, of the resulting <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</param>
  /// <param name="interpolation">Interpolation mode.</param>
  /// <param name="clip">Clipping area.</param>
  /// <returns>A <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> containing copied pixels.</returns>
  /// <remarks>You should always call the <see cref="O:Patagames.Pdf.Net.PdfBitmap.Dispose" /> method to release the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> and related resources created by the <see cref="M:Patagames.Pdf.Net.PdfBitmap.StretchTo(System.Int32,System.Int32,Patagames.Pdf.Enums.ImageInterpolation,Patagames.Pdf.FX_RECT)" /> method.</remarks>
  public PdfBitmap StretchTo(
    int width,
    int height,
    ImageInterpolation interpolation = ImageInterpolation.Default,
    FX_RECT clip = null)
  {
    return new PdfBitmap(Pdfium.FFPDFBitmap_StretchTo(this.Handle, width, height, interpolation, clip));
  }

  /// <summary>
  /// Clone and stretch this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  /// <param name="width">The width, in pixels, of the resulting <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</param>
  /// <param name="height">The height, in pixels, of the resulting <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.</param>
  /// <param name="flags">Interpolation mode.</param>
  /// <param name="clip">Clipping area.</param>
  /// <returns>A <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> containing copied pixels.</returns>
  /// <remarks>This method is deprecated. Please use <see cref="M:Patagames.Pdf.Net.PdfBitmap.StretchTo(System.Int32,System.Int32,Patagames.Pdf.Enums.ImageInterpolation,Patagames.Pdf.FX_RECT)" /> instead.</remarks>
  [Obsolete("This method is deprecated. Please use StretchTo(int, int, ImageInterpolation, FX_RECT) instead.", false)]
  public PdfBitmap StretchTo(int width, int height, int flags, FX_RECT clip = null)
  {
    return this.StretchTo(width, height, (ImageInterpolation) flags, clip);
  }

  /// <summary>
  /// Clone and swap this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  /// <param name="swapX">A flag indicating whether to swap an image in horizontal dimension.</param>
  /// <param name="swapY">A flag indicating whether to swap an image in vertical dimension.</param>
  /// <param name="clip">Clipping area.</param>
  /// <returns>A <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> containing copied pixels.</returns>
  /// <remarks>You should always call the <see cref="O:Patagames.Pdf.Net.PdfBitmap.Dispose" /> method to release the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> and related resources created by the <see cref="M:Patagames.Pdf.Net.PdfBitmap.SwapXY(System.Boolean,System.Boolean,Patagames.Pdf.FX_RECT)" /> method.</remarks>
  public PdfBitmap SwapXY(bool swapX, bool swapY, FX_RECT clip = null)
  {
    return new PdfBitmap(Pdfium.FFPDFBitmap_SwapXY(this.Handle, swapX, swapY, clip));
  }

  /// <summary>
  /// Clone and flip this <see cref="T:Patagames.Pdf.Net.PdfBitmap" />.
  /// </summary>
  /// <param name="flipX">A flag indicating whether to flip an image in horizontal dimension.</param>
  /// <param name="flipY">A flag indicating whether to flip an image in vertical dimension.</param>
  /// <returns>A <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> containing copied pixels.</returns>
  /// <remarks>You should always call the <see cref="O:Patagames.Pdf.Net.PdfBitmap.Dispose" /> method to release the <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> and related resources created by the <see cref="M:Patagames.Pdf.Net.PdfBitmap.FlipXY(System.Boolean,System.Boolean)" /> method.</remarks>
  public PdfBitmap FlipXY(bool flipX, bool flipY)
  {
    return new PdfBitmap(Pdfium.FFPDFBitmap_FlipImage(this.Handle, flipX, flipY));
  }
}
