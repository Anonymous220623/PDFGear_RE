// Decompiled with JetBrains decompiler
// Type: NAudio.Mixer.ListTextMixerControl
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;

#nullable disable
namespace NAudio.Mixer;

public class ListTextMixerControl : MixerControl
{
  internal ListTextMixerControl(
    MixerInterop.MIXERCONTROL mixerControl,
    IntPtr mixerHandle,
    MixerFlags mixerHandleType,
    int nChannels)
  {
    this.mixerControl = mixerControl;
    this.mixerHandle = mixerHandle;
    this.mixerHandleType = mixerHandleType;
    this.nChannels = nChannels;
    this.mixerControlDetails = new MixerInterop.MIXERCONTROLDETAILS();
    this.GetControlDetails();
  }

  protected override void GetDetails(IntPtr pDetails)
  {
  }
}
