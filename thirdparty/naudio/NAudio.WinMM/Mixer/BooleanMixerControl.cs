// Decompiled with JetBrains decompiler
// Type: NAudio.Mixer.BooleanMixerControl
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Mixer;

public class BooleanMixerControl : MixerControl
{
  private MixerInterop.MIXERCONTROLDETAILS_BOOLEAN boolDetails;

  internal BooleanMixerControl(
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
    this.boolDetails = Marshal.PtrToStructure<MixerInterop.MIXERCONTROLDETAILS_BOOLEAN>(pDetails);
  }

  public bool Value
  {
    get
    {
      this.GetControlDetails();
      return this.boolDetails.fValue == 1;
    }
    set
    {
      this.boolDetails.fValue = value ? 1 : 0;
      this.mixerControlDetails.paDetails = Marshal.AllocHGlobal(Marshal.SizeOf<MixerInterop.MIXERCONTROLDETAILS_BOOLEAN>(this.boolDetails));
      Marshal.StructureToPtr<MixerInterop.MIXERCONTROLDETAILS_BOOLEAN>(this.boolDetails, this.mixerControlDetails.paDetails, false);
      MmException.Try(MixerInterop.mixerSetControlDetails(this.mixerHandle, ref this.mixerControlDetails, MixerFlags.Mixer | this.mixerHandleType), "mixerSetControlDetails");
      Marshal.FreeHGlobal(this.mixerControlDetails.paDetails);
    }
  }
}
