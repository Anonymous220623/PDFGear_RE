// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveOutUtils
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave;

public static class WaveOutUtils
{
  public static float GetWaveOutVolume(IntPtr hWaveOut, object lockObject)
  {
    int dwVolume;
    MmResult volume;
    lock (lockObject)
      volume = WaveInterop.waveOutGetVolume(hWaveOut, out dwVolume);
    MmException.Try(volume, "waveOutGetVolume");
    return (float) (dwVolume & (int) ushort.MaxValue) / (float) ushort.MaxValue;
  }

  public static void SetWaveOutVolume(float value, IntPtr hWaveOut, object lockObject)
  {
    if ((double) value < 0.0)
      throw new ArgumentOutOfRangeException(nameof (value), "Volume must be between 0.0 and 1.0");
    if ((double) value > 1.0)
      throw new ArgumentOutOfRangeException(nameof (value), "Volume must be between 0.0 and 1.0");
    int dwVolume = (int) ((double) value * (double) ushort.MaxValue) + ((int) ((double) value * (double) ushort.MaxValue) << 16 /*0x10*/);
    MmResult result;
    lock (lockObject)
      result = WaveInterop.waveOutSetVolume(hWaveOut, dwVolume);
    MmException.Try(result, "waveOutSetVolume");
  }

  public static long GetPositionBytes(IntPtr hWaveOut, object lockObject)
  {
    lock (lockObject)
    {
      MmTime mmTime = new MmTime();
      mmTime.wType = 4U;
      MmException.Try(WaveInterop.waveOutGetPosition(hWaveOut, ref mmTime, Marshal.SizeOf<MmTime>(mmTime)), "waveOutGetPosition");
      return mmTime.wType == 4U ? (long) mmTime.cb : throw new Exception($"waveOutGetPosition: wType -> Expected {4}, Received {mmTime.wType}");
    }
  }
}
