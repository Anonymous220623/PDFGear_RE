// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmFormatTag
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

#nullable disable
namespace NAudio.Wave.Compression;

public class AcmFormatTag
{
  private AcmFormatTagDetails formatTagDetails;

  internal AcmFormatTag(AcmFormatTagDetails formatTagDetails)
  {
    this.formatTagDetails = formatTagDetails;
  }

  public int FormatTagIndex => this.formatTagDetails.formatTagIndex;

  public WaveFormatEncoding FormatTag => (WaveFormatEncoding) this.formatTagDetails.formatTag;

  public int FormatSize => this.formatTagDetails.formatSize;

  public AcmDriverDetailsSupportFlags SupportFlags => this.formatTagDetails.supportFlags;

  public int StandardFormatsCount => this.formatTagDetails.standardFormatsCount;

  public string FormatDescription => this.formatTagDetails.formatDescription;
}
