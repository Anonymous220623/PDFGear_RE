// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmFormat
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

#nullable disable
namespace NAudio.Wave.Compression;

public class AcmFormat
{
  private readonly AcmFormatDetails formatDetails;

  internal AcmFormat(AcmFormatDetails formatDetails)
  {
    this.formatDetails = formatDetails;
    this.WaveFormat = WaveFormat.MarshalFromPtr(formatDetails.waveFormatPointer);
  }

  public int FormatIndex => this.formatDetails.formatIndex;

  public WaveFormatEncoding FormatTag => (WaveFormatEncoding) this.formatDetails.formatTag;

  public AcmDriverDetailsSupportFlags SupportFlags => this.formatDetails.supportFlags;

  public WaveFormat WaveFormat { get; private set; }

  public int WaveFormatByteSize => this.formatDetails.waveFormatByteSize;

  public string FormatDescription => this.formatDetails.formatDescription;
}
