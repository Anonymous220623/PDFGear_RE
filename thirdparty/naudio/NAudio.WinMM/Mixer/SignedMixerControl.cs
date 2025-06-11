// Decompiled with JetBrains decompiler
// Type: NAudio.Mixer.SignedMixerControl
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Mixer;

public class SignedMixerControl : MixerControl
{
  private MixerInterop.MIXERCONTROLDETAILS_SIGNED signedDetails;

  internal SignedMixerControl(
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
    this.signedDetails = Marshal.PtrToStructure<MixerInterop.MIXERCONTROLDETAILS_SIGNED>(this.mixerControlDetails.paDetails);
  }

  public int Value
  {
    get
    {
      this.GetControlDetails();
      return this.signedDetails.lValue;
    }
    set
    {
      this.signedDetails.lValue = value;
      this.mixerControlDetails.paDetails = Marshal.AllocHGlobal(Marshal.SizeOf<MixerInterop.MIXERCONTROLDETAILS_SIGNED>(this.signedDetails));
      Marshal.StructureToPtr<MixerInterop.MIXERCONTROLDETAILS_SIGNED>(this.signedDetails, this.mixerControlDetails.paDetails, false);
      MmException.Try(MixerInterop.mixerSetControlDetails(this.mixerHandle, ref this.mixerControlDetails, MixerFlags.Mixer | this.mixerHandleType), "mixerSetControlDetails");
      Marshal.FreeHGlobal(this.mixerControlDetails.paDetails);
    }
  }

  public int MinValue => this.mixerControl.Bounds.minimum;

  public int MaxValue => this.mixerControl.Bounds.maximum;

  public double Percent
  {
    get => 100.0 * (double) (this.Value - this.MinValue) / (double) (this.MaxValue - this.MinValue);
    set
    {
      this.Value = (int) ((double) this.MinValue + value / 100.0 * (double) (this.MaxValue - this.MinValue));
    }
  }

  public override string ToString() => $"{base.ToString()} {this.Percent}%";
}
