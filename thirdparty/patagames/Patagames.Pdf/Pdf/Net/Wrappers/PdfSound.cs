// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Wrappers.PdfSound
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Wrappers;

/// <summary>Represents a sound object</summary>
/// <remarks>
/// <para>
/// Sample values are stored in the <see cref="P:Patagames.Pdf.Net.Wrappers.PdfSound.Stream" /> with the most significant bits first (big-endian order for samples larger than 8 bits).
/// Samples that are not a multiple of 8 bits are packed into consecutive bytes, starting at the most significant end.
/// If a sample extends across a byte boundary, the most significant bits are placed in the first byte, followed by less significant bits in subsequent bytes.
/// For dual-channel stereophonic sounds, the samples are stored in an interleaved format, with each
/// sample value for the left channel (channel 1) preceding the corresponding sample for the right(channel 2).
/// </para>
/// </remarks>
public class PdfSound : PdfWrapper
{
  /// <summary>Gets underlying stream.</summary>
  public PdfTypeStream Stream { get; private set; }

  /// <summary>
  /// Gets or sets the sampling rate, in samples per second.
  /// </summary>
  public int Rate
  {
    get => !this.IsExists("R") ? 0 : this.Dictionary["R"].As<PdfTypeNumber>().IntValue;
    set => this.Dictionary["R"] = (PdfTypeBase) PdfTypeNumber.Create(value);
  }

  /// <summary>Gets or sets the number of sound channels.</summary>
  /// <remarks>
  /// Default value: <strong>1</strong>.
  /// <note type="note">Acrobat supports a maximum of two sound channels.</note>
  /// </remarks>
  public int Channels
  {
    get => !this.IsExists("C") ? 1 : this.Dictionary["C"].As<PdfTypeNumber>().IntValue;
    set
    {
      if (value == 1 && this.Dictionary.ContainsKey("C"))
      {
        this.Dictionary.Remove("C");
      }
      else
      {
        if (value == 1)
          return;
        this.Dictionary["C"] = (PdfTypeBase) PdfTypeNumber.Create(value);
      }
    }
  }

  /// <summary>
  /// Gets or sets the number of bits per sample value per channel.
  /// </summary>
  /// <remarks>Default value: <strong>8</strong>.</remarks>
  public int Bps
  {
    get => !this.IsExists("B") ? 8 : this.Dictionary["B"].As<PdfTypeNumber>().IntValue;
    set
    {
      if (value == 8 && this.Dictionary.ContainsKey("B"))
      {
        this.Dictionary.Remove("B");
      }
      else
      {
        if (value == 8)
          return;
        this.Dictionary["B"] = (PdfTypeBase) PdfTypeNumber.Create(value);
      }
    }
  }

  /// <summary>Gets or sets the encoding format for the sample data.</summary>
  /// <remarks>Default value: <strong>Raw</strong>.</remarks>
  public SoundEncodingFormats Encoding
  {
    get
    {
      if (!this.IsExists("E"))
        return SoundEncodingFormats.Raw;
      SoundEncodingFormats result;
      if (Pdfium.GetEnumDescription<SoundEncodingFormats>(this.Dictionary["E"].As<PdfTypeName>().Value, out result))
        return result;
      throw new PdfParserException(string.Format(Error.err0045, (object) "E"));
    }
    set
    {
      if (value == SoundEncodingFormats.Raw && this.Dictionary.ContainsKey("E"))
      {
        this.Dictionary.Remove("E");
      }
      else
      {
        if (value == SoundEncodingFormats.Raw)
          return;
        string enumDescription = Pdfium.GetEnumDescription((Enum) value);
        this.Dictionary["E"] = !((enumDescription ?? "").Trim() == "") ? (PdfTypeBase) PdfTypeName.Create(enumDescription) : throw new ArgumentException(string.Format(Error.err0047, (object) nameof (Encoding), (object) "are Raw, Signed, muLaw and ALaw"));
      }
    }
  }

  /// <summary>The sound compression format used on the sample data.</summary>
  /// <remarks>
  /// If this property is null, no sound compression has been used;
  /// the data contains sampled waveforms to be played at <see cref="P:Patagames.Pdf.Net.Wrappers.PdfSound.Rate" /> samples per second per channel.
  /// <note type="note">At the time of publication, no standard values have been defined for the <see cref="P:Patagames.Pdf.Net.Wrappers.PdfSound.CompressionFormat" />
  /// and <see cref="P:Patagames.Pdf.Net.Wrappers.PdfSound.Parameters" /> properties.</note>
  /// </remarks>
  public string CompressionFormat
  {
    get => !this.IsExists("CO") ? (string) null : this.Dictionary["CO"].As<PdfTypeName>().Value;
    set
    {
      if (value == null && this.Dictionary.ContainsKey("CO"))
      {
        this.Dictionary.Remove("CO");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["CO"] = (PdfTypeBase) PdfTypeName.Create(value);
      }
    }
  }

  /// <summary>
  /// Gets or sets optional parameters specific to the sound compression format used.
  /// </summary>
  /// <remarks>
  /// <note type="note">At the time of publication, no standard values have been defined for the <see cref="P:Patagames.Pdf.Net.Wrappers.PdfSound.CompressionFormat" /> and <see cref="P:Patagames.Pdf.Net.Wrappers.PdfSound.Parameters" /> properties.</note>
  /// </remarks>
  public PdfTypeBase Parameters
  {
    get => !this.IsExists("CP") ? (PdfTypeBase) null : this.Dictionary["CP"];
    set
    {
      if (value == null && this.Dictionary.ContainsKey("CP"))
      {
        this.Dictionary.Remove("CP");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["CP"] = value;
      }
    }
  }

  /// <summary>
  /// Creates new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfSound" />.
  /// </summary>
  /// <param name="sampleData">Sample values which should be stored in the stream.</param>
  /// <param name="encoding">The encoding format for the sample data.</param>
  /// <param name="bps">The number of bits per sample value per channel.</param>
  /// <param name="rate">The sampling rate, in samples per second.</param>
  /// <param name="channels">The number of sound channels.</param>
  /// <remarks>
  /// For a description of the sampleData format, see the remarks section of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfSound" /> class.
  /// </remarks>
  public PdfSound(
    byte[] sampleData,
    SoundEncodingFormats encoding,
    int bps,
    int rate,
    int channels)
  {
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Sound");
    this.Stream = PdfTypeStream.Create();
    this.Stream.Init(sampleData, this.Dictionary);
    this.Bps = bps;
    this.Rate = rate;
    this.Channels = channels;
    this.Encoding = encoding;
  }

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfSound" /> and initialize it with specified stream
  /// </summary>
  /// <param name="stream">The dictionary or indirect dictionary</param>
  public PdfSound(PdfTypeStream stream)
    : base((PdfTypeBase) stream.As<PdfTypeStream>().Dictionary)
  {
    this.Stream = stream.As<PdfTypeStream>();
  }

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfSound" /> class from a waveform sound specified either by a byte array.
  /// </summary>
  /// <param name="waveData">A byte array containing a waveform sound.</param>
  /// <returns>New instance of PdfSound class.</returns>
  /// <remarks>Only WAV files in PCM format are supported.</remarks>
  public static PdfSound FromWave(byte[] waveData)
  {
    SoundEncodingFormats format;
    int chanels;
    int rate;
    int bps;
    return new PdfSound(Pdfium.FPDFTOOLS_GetWaveData(waveData, out format, out chanels, out rate, out bps), format, bps, rate, chanels);
  }

  /// <summary>
  /// Releases all resources used by the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfSound" />.
  /// </summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected override void Dispose(bool disposing)
  {
    this.Stream.Dispose();
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }
}
