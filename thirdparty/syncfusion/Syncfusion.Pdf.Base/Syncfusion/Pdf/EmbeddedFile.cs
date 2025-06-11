// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.EmbeddedFile
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Compression;
using Syncfusion.Pdf.Primitives;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class EmbeddedFile : IPdfWrapper
{
  private string m_fileName = string.Empty;
  private string m_filePath = string.Empty;
  private string m_mimeType = string.Empty;
  private byte[] m_data;
  private EmbeddedFileParams m_params = new EmbeddedFileParams();
  private PdfStream m_stream = new PdfStream();

  public EmbeddedFile(string fileName)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    this.Initialize();
    this.FileName = fileName;
    this.FilePath = fileName;
  }

  public EmbeddedFile(string fileName, byte[] data)
    : this(fileName)
  {
    this.Data = data != null ? data : throw new ArgumentNullException(nameof (data));
  }

  public EmbeddedFile(string fileName, Stream stream)
    : this(fileName)
  {
    int count = stream != null ? (int) stream.Length : throw new ArgumentNullException(nameof (stream));
    int offset = 0;
    this.m_data = new byte[stream.Length];
    int num;
    for (; count > 0; count -= num)
    {
      num = stream.Read(this.m_data, offset, count);
      offset += num;
    }
    this.m_stream.InternalStream.Write(this.m_data, 0, this.m_data.Length);
  }

  public string FileName
  {
    get => this.m_fileName;
    set
    {
      if (!(this.m_fileName != value))
        return;
      this.m_fileName = this.GetFileName(value);
    }
  }

  internal string FilePath
  {
    get => this.m_filePath;
    set
    {
      if (!(this.m_filePath != value))
        return;
      this.m_filePath = value;
    }
  }

  public byte[] Data
  {
    get => this.m_data;
    set => this.m_data = value;
  }

  public string MimeType
  {
    get => this.m_mimeType;
    set
    {
      if (!(this.m_mimeType != value))
        return;
      this.m_mimeType = value;
      this.m_stream.SetName("Subtype", this.m_mimeType, true);
    }
  }

  internal EmbeddedFileParams Params => this.m_params;

  protected void Initialize()
  {
    this.m_stream.SetProperty("Type", (IPdfPrimitive) new PdfName(nameof (EmbeddedFile)));
    this.m_stream.SetProperty("Params", (IPdfWrapper) this.m_params);
    this.m_stream.BeginSave += new SavePdfPrimitiveEventHandler(this.Stream_BeginSave);
  }

  protected void Save()
  {
    bool flag = false;
    if (this.m_data == null)
    {
      using (FileStream input = new FileStream(this.m_filePath, FileMode.Open, FileAccess.Read))
      {
        BinaryReader binaryReader = new BinaryReader((Stream) input);
        this.m_data = new PdfZlibCompressor(PdfCompressionLevel.Best).Compress(binaryReader.ReadBytes((int) input.Length));
        binaryReader.Close();
        flag = true;
      }
    }
    this.m_stream.Clear();
    this.m_stream.Compress = false;
    if (this.m_data == null)
      return;
    this.m_stream.InternalStream.Write(this.m_data, 0, this.m_data.Length);
    if (flag)
      this.m_stream.AddFilter("FlateDecode");
    this.m_params.Size = this.m_data.Length;
  }

  private void Stream_BeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.Save();

  private string GetFileName(string attachmentName)
  {
    char[] chArray = new char[2]{ '\\', '/' };
    string[] strArray = attachmentName.Split(chArray);
    return strArray[strArray.Length - 1];
  }

  public IPdfPrimitive Element => (IPdfPrimitive) this.m_stream;
}
