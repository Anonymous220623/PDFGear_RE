// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveOutCapabilities
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct WaveOutCapabilities
{
  private short manufacturerId;
  private short productId;
  private int driverVersion;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
  private string productName;
  private SupportedWaveFormat supportedFormats;
  private short channels;
  private short reserved;
  private WaveOutSupport support;
  private Guid manufacturerGuid;
  private Guid productGuid;
  private Guid nameGuid;
  private const int MaxProductNameLength = 32 /*0x20*/;

  public int Channels => (int) this.channels;

  public bool SupportsPlaybackRateControl
  {
    get => (this.support & WaveOutSupport.PlaybackRate) == WaveOutSupport.PlaybackRate;
  }

  public string ProductName => this.productName;

  public bool SupportsWaveFormat(SupportedWaveFormat waveFormat)
  {
    return (this.supportedFormats & waveFormat) == waveFormat;
  }

  public Guid NameGuid => this.nameGuid;

  public Guid ProductGuid => this.productGuid;

  public Guid ManufacturerGuid => this.manufacturerGuid;
}
