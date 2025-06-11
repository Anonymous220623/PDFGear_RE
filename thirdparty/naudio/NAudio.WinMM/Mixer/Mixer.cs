// Decompiled with JetBrains decompiler
// Type: NAudio.Mixer.Mixer
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Mixer;

public class Mixer
{
  private MixerInterop.MIXERCAPS caps;
  private IntPtr mixerHandle;
  private MixerFlags mixerHandleType;

  public static int NumberOfDevices => MixerInterop.mixerGetNumDevs();

  public Mixer(int mixerIndex)
  {
    if (mixerIndex < 0 || mixerIndex >= NAudio.Mixer.Mixer.NumberOfDevices)
      throw new ArgumentOutOfRangeException("mixerID");
    this.caps = new MixerInterop.MIXERCAPS();
    MmException.Try(MixerInterop.mixerGetDevCaps((IntPtr) mixerIndex, ref this.caps, Marshal.SizeOf<MixerInterop.MIXERCAPS>(this.caps)), "mixerGetDevCaps");
    this.mixerHandle = (IntPtr) mixerIndex;
    this.mixerHandleType = MixerFlags.Mixer;
  }

  public int DestinationCount => (int) this.caps.cDestinations;

  public string Name => this.caps.szPname;

  public Manufacturers Manufacturer => (Manufacturers) this.caps.wMid;

  public int ProductID => (int) this.caps.wPid;

  public MixerLine GetDestination(int destinationIndex)
  {
    if (destinationIndex < 0 || destinationIndex >= this.DestinationCount)
      throw new ArgumentOutOfRangeException(nameof (destinationIndex));
    return new MixerLine(this.mixerHandle, destinationIndex, this.mixerHandleType);
  }

  public IEnumerable<MixerLine> Destinations
  {
    get
    {
      for (int destination = 0; destination < this.DestinationCount; ++destination)
        yield return this.GetDestination(destination);
    }
  }

  public static IEnumerable<NAudio.Mixer.Mixer> Mixers
  {
    get
    {
      for (int device = 0; device < NAudio.Mixer.Mixer.NumberOfDevices; ++device)
        yield return new NAudio.Mixer.Mixer(device);
    }
  }
}
