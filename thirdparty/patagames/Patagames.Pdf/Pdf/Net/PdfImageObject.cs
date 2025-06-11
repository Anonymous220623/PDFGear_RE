// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfImageObject
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using System;
using System.IO;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents a image object.</summary>
public class PdfImageObject : PdfPageObject, IDisposable
{
  private IntPtr _jpegHandle = IntPtr.Zero;
  private FPDF_FILEACCESS _loadJpeg = new FPDF_FILEACCESS(0U);
  private PdfBitmap _bitmap;
  private PdfTypeStream _stream;

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the control has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>
  /// Get the copy of bitmap from an image object / Set the bitmap to an image object. You do not need to call Dispose methof for getted PdfBitmap.
  /// </summary>
  /// <value>PdfBitmap object or null if this ImageObject does not contain any bitmaps</value>
  public PdfBitmap Bitmap
  {
    get
    {
      try
      {
        if (this._bitmap == null)
          this._bitmap = this.GetBitmap();
        return this._bitmap;
      }
      catch (ImageObjectIsEmptyException ex)
      {
        return (PdfBitmap) null;
      }
    }
    set
    {
      if (value == null)
        return;
      this.SetBitmap(value);
      this._bitmap.Dispose();
      this._bitmap = (PdfBitmap) null;
    }
  }

  /// <summary>Get the stream contains image data.</summary>
  public PdfTypeStream Stream
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeStream) null;
      IntPtr stream = Pdfium.FPDFImageObj_GetStream(this.Handle);
      if (stream == IntPtr.Zero)
        return (PdfTypeStream) null;
      if (this._stream == null || this._stream.IsDisposed || this._stream.Handle != stream)
        this._stream = new PdfTypeStream(stream);
      return this._stream;
    }
  }

  internal PdfImageObject(IntPtr handle)
    : base(handle)
  {
    this.Handle = handle;
    this._loadJpeg.GetBlock = new GetBlockCallback(this.LoadJpeg);
  }

  /// <summary>
  /// Create a new instance of <see cref="T:Patagames.Pdf.Net.PdfImageObject" /> class.
  /// </summary>
  /// <param name="document">Instance of <see cref="T:Patagames.Pdf.Net.PdfDocument" /> class</param>
  /// <returns>Instance of <see cref="T:Patagames.Pdf.Net.PdfImageObject" /> class.</returns>
  public static PdfImageObject Create(PdfDocument document)
  {
    IntPtr handle = Pdfium.FPDFPageObj_NewImgeObj(document.Handle);
    return !(handle == IntPtr.Zero) ? new PdfImageObject(handle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>
  /// Create a new instance of <see cref="T:Patagames.Pdf.Net.PdfImageObject" /> class and initialize it with the specified bitmap and position.
  /// </summary>
  /// <param name="document">Instance of <see cref="T:Patagames.Pdf.Net.PdfDocument" /> class</param>
  /// <param name="bitmap">The <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> to initialize for.</param>
  /// <param name="x">The x coordinate of the image object in standard user space</param>
  /// <param name="y">The x coordinate of the image object in standard user space</param>
  /// <returns>Instance of <see cref="T:Patagames.Pdf.Net.PdfImageObject" /> class.</returns>
  /// <remarks>PdfBitmap may be disposed after passing to this methos</remarks>
  public static PdfImageObject Create(PdfDocument document, PdfBitmap bitmap, float x, float y)
  {
    IntPtr handle = Pdfium.FPDFPageObj_NewImgeObj(document.Handle);
    PdfImageObject pdfImageObject = !(handle == IntPtr.Zero) ? new PdfImageObject(handle) : throw Pdfium.ProcessLastError();
    pdfImageObject.SetBitmap(bitmap);
    pdfImageObject.Matrix = new FS_MATRIX((float) bitmap.Width, 0.0f, 0.0f, (float) bitmap.Height, x, y);
    return pdfImageObject;
  }

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    if (this._jpegHandle != IntPtr.Zero)
      Pdfium.FPDFImageObj_DestroyJpegFile(this._jpegHandle);
    this._jpegHandle = IntPtr.Zero;
    if (this._bitmap != null)
      this._bitmap.Dispose();
    this._bitmap = (PdfBitmap) null;
    if (this._stream != null)
      this._stream.Dispose();
    this._stream = (PdfTypeStream) null;
    this._loadJpeg.Dispose();
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Finalize object</summary>
  ~PdfImageObject()
  {
    if (PdfCommon.IsCheckForMemoryLeaks && !this.IsDisposed)
      throw new MemoryLeakException(nameof (PdfImageObject));
  }

  /// <summary>Calculate bounding box</summary>
  public void CalcBoundingBox() => Pdfium.FPDFImageObj_CalcBoundingBox(this.Handle);

  /// <summary>
  /// This property is obsolete. Please use <see cref="P:Patagames.Pdf.Net.PdfPageObject.Matrix" /> property instead.
  /// </summary>
  /// <param name="a">The coefficient "a" of the matrix</param>
  /// <param name="b">The coefficient "b" of the matrix</param>
  /// <param name="c">The coefficient "c" of the matrix</param>
  /// <param name="d">The coefficient "d" of the matrix</param>
  /// <param name="e">The coefficient "e" of the matrix</param>
  /// <param name="f">The coefficient "f" of the matrix</param>
  [Obsolete("This method is obsolete. Please use Matrix property instead.", false)]
  public void SetMatrix(float a, float b, float c, float d, float e, float f)
  {
    Pdfium.FPDFImageObj_SetMatrix(this.Handle, (double) a, (double) b, (double) c, (double) d, (double) e, (double) f);
  }

  /// <summary>
  /// Load Image from a JPEG image file and then set it to an image object.
  /// </summary>
  /// <param name="filePath">Path to the jpeg file</param>
  public void LoadJpegFile(string filePath)
  {
    byte[] numArray = File.ReadAllBytes(filePath);
    this._loadJpeg.Param = numArray;
    this._loadJpeg.FileLen = (uint) numArray.Length;
    if (this._jpegHandle != IntPtr.Zero)
      Pdfium.FPDFImageObj_DestroyJpegFile(this._jpegHandle);
    this._jpegHandle = Pdfium.FPDFImageObj_LoadJpegFile(IntPtr.Zero, 0, this.Handle, this._loadJpeg);
    this.IsDisposed = false;
  }

  /// <summary>
  /// Load Image from a byte array containing a JPEG image and then set it to an image object.
  /// </summary>
  /// <param name="data">An array containing a jpeg image.</param>
  public void LoadJpegFile(byte[] data) => Pdfium.FPDFImageObj_SetJpegImage(this.Handle, data);

  /// <summary>Set the bitmap to an image object.</summary>
  /// <param name="bitmap">The PdfBitmap which you want to set it to the image object.</param>
  /// <returns>TRUE if successful, FALSE otherwise.</returns>
  public bool SetBitmap(PdfBitmap bitmap)
  {
    return Pdfium.FPDFImageObj_SetBitmap(IntPtr.Zero, 0, this.Handle, bitmap.Handle);
  }

  /// <summary>Get the copy of bitmap from an image object.</summary>
  /// <returns>PdfBitmap object.</returns>
  /// <remarks>You should always call the Dispose method to release the PdfBitmap and related resources created by the GetBitmap method.</remarks>
  public PdfBitmap GetBitmap() => new PdfBitmap(Pdfium.FPDFImageObj_GetCloneBitmap(this.Handle));

  private bool LoadJpeg(byte[] data, uint pos, byte[] buffer, uint buflen)
  {
    Array.Copy((Array) data, (int) pos, (Array) buffer, 0, (int) buflen);
    return true;
  }
}
