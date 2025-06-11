// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfTypeStream
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>Represents the Stream type of objects</summary>
/// <remarks>
/// A stream object, like a string object, is a sequence of bytes.
/// However, a PDF application can read a stream incrementally, while a string must be read in its entirety.
/// Furthermore, a stream can be of unlimited length, whereas a string is subject to an implementation limit.
/// For this reason, objects with potentially large amounts of data, such as images and page descriptions,
/// are represented as streams. All streams must be indirect objects and the stream dictionary must be a direct object.
/// </remarks>
/// <summary>
/// Construct new instance of PdfTypeStream class from given Handle
/// </summary>
/// <param name="Handle">A handle to the unmanaged Stream object</param>
public class PdfTypeStream(IntPtr Handle) : PdfTypeBase(Handle)
{
  private PdfTypeDictionary _dictionary;
  private byte[] _content;
  private byte[] _decodedData;

  /// <summary>
  /// Gets the length in bytes of the underlying buffer of the stream.
  /// </summary>
  public int Length => Pdfium.FPDFSTREAM_GetRawSize(this.Handle);

  /// <summary>Get underlying buffer of the stream.</summary>
  public IntPtr RawData => Pdfium.FPDFSTREAM_GetRawData(this.Handle);

  /// <summary>
  /// Get a managed copy of underlying buffer of the stream.
  /// </summary>
  public byte[] Content
  {
    get
    {
      if (this._content == null)
        this._content = Pdfium.FPDFSTREAM_GetData(this.Handle);
      return this._content;
    }
  }

  /// <summary>
  /// Gets a boolean value that indicates whether a Stream was initialized from memory.
  /// </summary>
  public bool IsMemoryBased => Pdfium.FPDFSTREAM_IsMemoryBased(this.Handle);

  /// <summary>
  /// Gets a Dictionary representation of the specified object.
  /// </summary>
  public PdfTypeDictionary Dictionary
  {
    get
    {
      IntPtr dict = Pdfium.FPDFOBJ_GetDict(this.Handle);
      if (dict == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      if (this._dictionary != null && this._dictionary.Handle == dict)
        return this._dictionary;
      this._dictionary = new PdfTypeDictionary(dict);
      return this._dictionary;
    }
  }

  /// <summary>Gets decoded strem content.</summary>
  public byte[] DecodedData
  {
    get
    {
      if (this._decodedData == null)
      {
        int size;
        IntPtr decodedData = Pdfium.FPDFSTREAM_GetDecodedData(this.Handle, out size);
        this._decodedData = new byte[size];
        Marshal.Copy(decodedData, this._decodedData, 0, this._decodedData.Length);
        Pdfium.FPDFSTREAM_ReleaseDecodedData(decodedData);
      }
      return this._decodedData;
    }
  }

  /// <summary>Gets decoded stream contetn as ASCII string</summary>
  public string DecodedText => PdfCommon.DefaultAnsiEncoding.GetString(this.DecodedData);

  /// <summary>Creates new Stream object</summary>
  /// <returns>The instance of a newly created object</returns>
  public static PdfTypeStream Create()
  {
    IntPtr Handle = Pdfium.FPDFSTREAM_Create();
    return !(Handle == IntPtr.Zero) ? new PdfTypeStream(Handle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>Creates new instance of PdfTypeStream class</summary>
  /// <param name="handle">A handle to the unmanaged Stream object</param>
  /// <returns>An instance of PdfTypeStream</returns>
  public static PdfTypeStream Create(IntPtr handle)
  {
    return !(handle == IntPtr.Zero) ? new PdfTypeStream(handle) : throw new ArgumentException();
  }

  /// <summary>Set specified raw data into current stream</summary>
  /// <param name="data">The stream raw data in unmanaged memory</param>
  /// <param name="size">The length in bytes of the data</param>
  /// <param name="bCompressed">Indicates whether a passed buffer is compressed or not.</param>
  public void SetRawData(IntPtr data, int size, bool bCompressed)
  {
    Pdfium.FPDFSTREAM_SetData(this.Handle, data, size, bCompressed);
    this._decodedData = (byte[]) null;
    this._content = (byte[]) null;
  }

  /// <summary>Replace the stream content with the specified data.</summary>
  /// <param name="data">Data to write to stream</param>
  /// <param name="bCompressed">Indicates whether a passed buffer is compressed or not.</param>
  public void SetContent(byte[] data, bool bCompressed)
  {
    Pdfium.FPDFSTREAM_SetData(this.Handle, data, bCompressed);
    this._decodedData = (byte[]) null;
    this._content = (byte[]) null;
  }

  /// <summary>Initialize the stream with a sequence of bytes.</summary>
  /// <param name="data">An array of bytes. This method copies all bytes from data to the current stream.</param>
  /// <param name="dictionary">The dictionary associated with the stream.</param>
  public void Init(byte[] data, PdfTypeDictionary dictionary = null)
  {
    Pdfium.FPDFSTREAM_InitStream(this.Handle, data, dictionary == null ? IntPtr.Zero : dictionary.Handle);
    this._dictionary = dictionary;
    this._decodedData = (byte[]) null;
    this._content = (byte[]) null;
  }

  /// <summary>
  /// Reads a specified number of bytes from the current stream from specified position.
  /// </summary>
  /// <param name="startPos">The ofset from the begining of stream</param>
  /// <param name="length">The number of bytes to read from stream</param>
  /// <param name="buffer">An array of readed bytes.</param>
  /// <returns>True for successfull, false otherwise.</returns>
  public bool Read(int startPos, int length, byte[] buffer)
  {
    return Pdfium.FPDFSTREAM_ReadRawData(this.Handle, startPos, length, buffer);
  }

  /// <summary>
  /// Initialize the stream with an empty sequence of bytes.
  /// </summary>
  public void InitEmpty()
  {
    byte[] data = new byte[0];
    PdfTypeDictionary dictionary = PdfTypeDictionary.Create();
    dictionary.SetIntegerAt("Length", 0);
    this.Init(data, dictionary);
    this._decodedData = (byte[]) null;
    this._content = (byte[]) null;
  }
}
