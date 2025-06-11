// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Primitives.Pdf3DStream
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Compression;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Security;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Primitives;

internal class Pdf3DStream : PdfDictionary, IPdfDecryptable
{
  private const string Prefix = "stream";
  private const string Suffix = "endstream";
  private byte[] m_content;
  private Pdf3DAnimation m_animation;
  private int m_defaultView;
  private string m_onInstatiate;
  private Pdf3DAnnotationType m_type;
  private Pdf3DViewCollection m_pdf3dViewCollection;
  private MemoryStream m_dataStream;
  private bool m_blockEncryption;
  private bool m_bDecrypted;
  private bool m_bCompress;

  public Pdf3DAnimation Animation
  {
    get => this.m_animation;
    set => this.m_animation = value;
  }

  public byte[] Content
  {
    get => this.m_content;
    set => this.m_content = value;
  }

  public int DefaultView
  {
    get => this.m_defaultView;
    set => this.m_defaultView = value;
  }

  internal Pdf3DAnnotationType Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  public string OnInstantiate
  {
    get => this.m_onInstatiate;
    set => this.m_onInstatiate = value;
  }

  public Pdf3DViewCollection Views => this.m_pdf3dViewCollection;

  internal MemoryStream InternalStream => this.m_dataStream;

  internal byte[] Data
  {
    get => this.m_dataStream.ToArray();
    set
    {
      this.m_dataStream.SetLength(0L);
      this.m_dataStream.Write(value, 0, value.Length);
      this.Modify();
    }
  }

  internal bool Compress
  {
    get => this.m_bCompress;
    set
    {
      this.m_bCompress = value;
      this.Modify();
    }
  }

  internal Pdf3DStream()
  {
    this.m_dataStream = new MemoryStream(100);
    this.m_bCompress = true;
    this.m_pdf3dViewCollection = new Pdf3DViewCollection();
  }

  internal Pdf3DStream(PdfDictionary dictionary, byte[] data)
    : base(dictionary)
  {
    this.m_dataStream = new MemoryStream(data.Length);
    this.Data = data;
    this.m_bCompress = false;
  }

  public static byte[] StreamToBytes(Stream stream)
  {
    return stream != null ? Pdf3DStream.StreamToBytes(stream, false) : throw new ArgumentNullException(nameof (stream));
  }

  public static byte[] StreamToBytes(Stream stream, bool writeWholeStream)
  {
    long num1 = stream != null ? stream.Position : throw new ArgumentNullException(nameof (stream));
    long num2 = stream.Position != 0L ? stream.Position : stream.Length;
    long count = writeWholeStream ? stream.Length : num2;
    byte[] buffer = new byte[count];
    stream.Position = 0L;
    stream.Read(buffer, 0, (int) count);
    stream.Position = num1;
    return buffer;
  }

  public static byte[] StreamToBigEndian(Stream stream)
  {
    return Encoding.Convert(Encoding.Unicode, Encoding.BigEndianUnicode, Pdf3DStream.StreamToBytes(stream));
  }

  internal void Write(char symbol) => this.Write(symbol.ToString());

  internal void Write(string text)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (text.Length <= 0)
      throw new ArgumentException("Can't write an empty string.", nameof (text));
    this.Write(Encoding.UTF8.GetBytes(text));
  }

  internal void Write(byte[] data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (data.Length <= 0)
      throw new ArgumentException("Can't write an empty array.", nameof (data));
    this.m_dataStream.Write(data, 0, data.Length);
    this.Modify();
  }

  internal void BlockEncryption() => this.m_blockEncryption = true;

  internal void Decompress()
  {
    IPdfPrimitive pdfPrimitive1 = this["Filter"];
    if ((object) (pdfPrimitive1 as PdfReferenceHolder) != null)
      pdfPrimitive1 = (pdfPrimitive1 as PdfReferenceHolder).Object;
    if (pdfPrimitive1 != null)
    {
      if ((object) (pdfPrimitive1 as PdfName) != null)
      {
        this.Data = this.Decompress(this.Data, (pdfPrimitive1 as PdfName).Value);
      }
      else
      {
        if (!(pdfPrimitive1 is PdfArray))
          throw new PdfDocumentException("Invalid/Unknown/Unsupported formatUnexpected object for filter.");
        foreach (IPdfPrimitive pdfPrimitive2 in pdfPrimitive1 as PdfArray)
          this.Data = this.Decompress(this.Data, (pdfPrimitive2 as PdfName).Value ?? throw new PdfDocumentException("Invalid/Unknown/Unsupported format"));
      }
    }
    this.Remove("Filter");
    this.m_bCompress = true;
  }

  internal new void Clear()
  {
    this.InternalStream.SetLength(0L);
    this.InternalStream.Position = 0L;
    this.Remove("Filter");
    this.m_bCompress = true;
    this.Modify();
  }

  public override void Save(IPdfWriter writer)
  {
    this.OnBeginSave(new SavePdfPrimitiveEventArgs(writer));
    byte[] data = this.CompressContent(writer);
    this["Type"] = (IPdfPrimitive) new PdfName("3D");
    if (this.m_type == Pdf3DAnnotationType.PRC)
      this["Subtype"] = (IPdfPrimitive) new PdfName("PRC");
    else
      this["Subtype"] = (IPdfPrimitive) new PdfName("U3D");
    if (this.m_animation != null)
      this["AN"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_animation);
    this["DV"] = (IPdfPrimitive) new PdfNumber(this.m_defaultView);
    if (this.m_pdf3dViewCollection != null && this.m_pdf3dViewCollection.Count > 0)
    {
      PdfArray array = new PdfArray();
      for (int index = 0; index < this.m_pdf3dViewCollection.Count; ++index)
        array.Insert(index, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_pdf3dViewCollection[index]));
      this["VA"] = (IPdfPrimitive) new PdfArray(array);
    }
    if (this.m_onInstatiate != null)
    {
      PdfStream pdfStream = new PdfStream();
      pdfStream.Write(this.m_onInstatiate);
      this["OnInstantiate"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream);
    }
    this["Length"] = (IPdfPrimitive) new PdfNumber(data.Length);
    this.Save(writer, false);
    writer.Write("stream");
    writer.Write("\r\n");
    if (data.Length > 0)
    {
      writer.Write(data);
      writer.Write("\r\n");
    }
    writer.Write("endstream");
    writer.Write("\r\n");
    this.OnEndSave(new SavePdfPrimitiveEventArgs(writer));
    if (!this.m_bCompress)
      return;
    this.Remove("Filter");
  }

  public bool WasEncrypted => throw new Exception("The method or operation is not implemented.");

  public bool Decrypted => this.m_bDecrypted;

  public void Decrypt(PdfEncryptor encryptor, long currObjNumber)
  {
    if (encryptor == null || this.m_bDecrypted)
      return;
    this.m_bDecrypted = true;
    this.Data = encryptor.EncryptData(currObjNumber, this.Data, false);
  }

  private byte[] Decompress(byte[] data, string filter)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (filter == null)
      throw new ArgumentNullException(nameof (filter));
    return data.Length == 0 ? data : this.PostProcess(this.DetermineCompressor(filter).Decompress(data), filter);
  }

  private IPdfCompressor DetermineCompressor(string filter)
  {
    if (filter == null)
      throw new ArgumentNullException(nameof (filter));
    switch (filter)
    {
      case "FlateDecode":
        return (IPdfCompressor) new PdfZlibCompressor();
      case "LZWDecode":
        return (IPdfCompressor) new PdfLzwCompressor();
      default:
        throw new PdfDocumentException($"Invalid/Unknown/Unsupported format Unsupported compressor ({filter}).");
    }
  }

  private byte[] PostProcess(byte[] data, string filter)
  {
    if (!(filter == "FlateDecode"))
      return data;
    IPdfPrimitive pdfPrimitive1 = this["DecodeParms"];
    if (pdfPrimitive1 == null)
      return data;
    int num1 = pdfPrimitive1 is PdfDictionary pdfDictionary ? (pdfDictionary["Predictor"] as PdfNumber).IntValue : throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
    switch (num1)
    {
      case 1:
        return data;
      case 2:
        throw new PdfDocumentException("Unsupported predictor: TIFF 2.");
      default:
        if (num1 >= 16 /*0x10*/ || num1 <= 2)
          throw new PdfDocumentException("Invalid/Unknown/Unsupported format Unknown predictor code: " + num1.ToString());
        int num2 = 1;
        int num3 = 1;
        IPdfPrimitive pdfPrimitive2 = pdfDictionary["Colors"];
        if (pdfPrimitive2 != null)
          num2 = (pdfPrimitive2 as PdfNumber).IntValue;
        IPdfPrimitive pdfPrimitive3 = pdfDictionary["Columns"];
        if (pdfPrimitive3 != null)
          num3 = (pdfPrimitive3 as PdfNumber).IntValue;
        IPdfPrimitive pdfPrimitive4 = pdfDictionary["BitsPerComponent"];
        if (pdfPrimitive4 != null)
        {
          int intValue = (pdfPrimitive4 as PdfNumber).IntValue;
        }
        return PdfPngFilter.Decompress(data, num2 * num3);
    }
  }

  private void NormalizeFilter()
  {
    if (!(this["Filter"] is PdfArray pdfArray) || pdfArray.Count != 1)
      return;
    this["Filter"] = pdfArray[0];
  }

  private byte[] CompressContent(IPdfWriter writer)
  {
    writer.Document.Compression = PdfCompressionLevel.AboveNormal;
    PdfCompressionLevel compression = writer.Document.Compression;
    bool flag = compression != PdfCompressionLevel.BestSpeed;
    byte[] data = this.Data;
    if (flag && this.m_bCompress)
    {
      data = new PdfZlibCompressor(compression).Compress(data);
      this.AddFilter("FlateDecode");
    }
    return data;
  }

  private void AddFilter(string filterName)
  {
    IPdfPrimitive pdfPrimitive = this["Filter"];
    if ((object) (pdfPrimitive as PdfReferenceHolder) != null)
      pdfPrimitive = (pdfPrimitive as PdfReferenceHolder).Object;
    PdfArray pdfArray = pdfPrimitive as PdfArray;
    PdfName element1 = pdfPrimitive as PdfName;
    if (element1 != (PdfName) null)
    {
      pdfArray = new PdfArray();
      pdfArray.Insert(0, (IPdfPrimitive) element1);
      this["Filter"] = (IPdfPrimitive) pdfArray;
    }
    PdfName element2 = new PdfName(filterName);
    if (pdfArray == null)
      this["Filter"] = (IPdfPrimitive) element2;
    else
      pdfArray.Insert(0, (IPdfPrimitive) element2);
  }

  private byte[] EncryptContent(byte[] data, IPdfWriter writer)
  {
    PdfDocumentBase document = writer.Document;
    PdfEncryptor encryptor = document.Security.Encryptor;
    if (encryptor.Encrypt && !this.m_blockEncryption)
      data = encryptor.EncryptData(document.CurrentSavingObj.ObjNum, data, true);
    return data;
  }
}
