// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfSound
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfSound : IPdfWrapper
{
  private int m_rate = 22050;
  private PdfSoundEncoding m_encoding;
  private PdfSoundChannels m_channels = PdfSoundChannels.Mono;
  private int m_bits = 8;
  private string m_fileName = string.Empty;
  private PdfStream m_stream = new PdfStream();

  public PdfSound(string fileName)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    Utils.CheckFilePath(fileName);
    this.FileName = fileName;
    this.m_stream.SetString("T", fileName);
    this.m_stream.SetProperty("R", (IPdfPrimitive) new PdfNumber(this.m_rate));
    this.m_stream.BeginSave += new SavePdfPrimitiveEventHandler(this.Stream_BeginSave);
  }

  internal PdfSound(string fileName, bool test)
  {
    this.FileName = fileName != null ? fileName : throw new ArgumentNullException(nameof (fileName));
    this.m_stream.SetString("T", fileName);
    this.m_stream.SetProperty("R", (IPdfPrimitive) new PdfNumber(this.m_rate));
    this.m_stream.BeginSave += new SavePdfPrimitiveEventHandler(this.Stream_BeginSave);
  }

  internal PdfSound()
  {
    this.m_stream.SetProperty("R", (IPdfPrimitive) new PdfNumber(this.m_rate));
    this.m_stream.BeginSave += new SavePdfPrimitiveEventHandler(this.Stream_BeginSave);
  }

  public int Rate
  {
    get => this.m_rate;
    set
    {
      if (this.m_rate == value)
        return;
      this.m_rate = value;
      this.m_stream.SetNumber("R", this.m_rate);
    }
  }

  public int Bits
  {
    get => this.m_bits;
    set
    {
      if (this.m_bits == value)
        return;
      this.m_bits = value;
      this.m_stream.SetNumber("B", this.m_bits);
    }
  }

  public PdfSoundEncoding Encoding
  {
    get => this.m_encoding;
    set
    {
      if (this.m_encoding == value)
        return;
      this.m_encoding = value;
      this.m_stream.SetName("E", this.m_encoding.ToString());
    }
  }

  public PdfSoundChannels Channels
  {
    get => this.m_channels;
    set
    {
      if (this.m_channels == value)
        return;
      this.m_channels = value;
      this.m_stream.SetNumber("C", (int) this.m_channels);
    }
  }

  public string FileName
  {
    get => this.m_fileName;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (FileName));
        case "":
          throw new ArithmeticException("FileName can't be empty string.");
        default:
          this.m_fileName = Path.GetFullPath(value);
          break;
      }
    }
  }

  private void Stream_BeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.Save();

  protected void Save()
  {
    using (FileStream fileStream = new FileStream(this.FileName, FileMode.Open, FileAccess.Read))
    {
      byte[] bigEndian = PdfStream.StreamToBigEndian((Stream) fileStream);
      this.m_stream.Clear();
      this.m_stream.InternalStream.Write(bigEndian, 0, bigEndian.Length);
    }
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_stream;
}
