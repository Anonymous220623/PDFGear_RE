// Decompiled with JetBrains decompiler
// Type: NAudio.Mixer.UnsignedMixerControl
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Mixer;

public class UnsignedMixerControl : MixerControl
{
  private MixerInterop.MIXERCONTROLDETAILS_UNSIGNED[] unsignedDetails;

  internal UnsignedMixerControl(
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
    this.unsignedDetails = new MixerInterop.MIXERCONTROLDETAILS_UNSIGNED[this.nChannels];
    for (int index = 0; index < this.nChannels; ++index)
      this.unsignedDetails[index] = Marshal.PtrToStructure<MixerInterop.MIXERCONTROLDETAILS_UNSIGNED>(this.mixerControlDetails.paDetails);
  }

  public uint Value
  {
    get
    {
      this.GetControlDetails();
      return this.unsignedDetails[0].dwValue;
    }
    set
    {
      int num = Marshal.SizeOf<MixerInterop.MIXERCONTROLDETAILS_UNSIGNED>(this.unsignedDetails[0]);
      this.mixerControlDetails.paDetails = Marshal.AllocHGlobal(num * this.nChannels);
      for (int index = 0; index < this.nChannels; ++index)
      {
        this.unsignedDetails[index].dwValue = value;
        long ptr = this.mixerControlDetails.paDetails.ToInt64() + (long) (num * index);
        Marshal.StructureToPtr<MixerInterop.MIXERCONTROLDETAILS_UNSIGNED>(this.unsignedDetails[index], (IntPtr) ptr, false);
      }
      MmException.Try(MixerInterop.mixerSetControlDetails(this.mixerHandle, ref this.mixerControlDetails, MixerFlags.Mixer | this.mixerHandleType), "mixerSetControlDetails");
      Marshal.FreeHGlobal(this.mixerControlDetails.paDetails);
    }
  }

  public uint MinValue => (uint) this.mixerControl.Bounds.minimum;

  public uint MaxValue => (uint) this.mixerControl.Bounds.maximum;

  public double Percent
  {
    get => 100.0 * (double) (this.Value - this.MinValue) / (double) (this.MaxValue - this.MinValue);
    set
    {
      this.Value = (uint) ((double) this.MinValue + value / 100.0 * (double) (this.MaxValue - this.MinValue));
    }
  }

  public override string ToString() => $"{base.ToString()} {this.Percent}%";
}
