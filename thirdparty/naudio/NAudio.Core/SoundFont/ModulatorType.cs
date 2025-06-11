// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.ModulatorType
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.SoundFont;

public class ModulatorType
{
  private bool polarity;
  private bool direction;
  private bool midiContinuousController;
  private ControllerSourceEnum controllerSource;
  private SourceTypeEnum sourceType;
  private ushort midiContinuousControllerNumber;

  internal ModulatorType(ushort raw)
  {
    this.polarity = ((int) raw & 512 /*0x0200*/) == 512 /*0x0200*/;
    this.direction = ((int) raw & 256 /*0x0100*/) == 256 /*0x0100*/;
    this.midiContinuousController = ((int) raw & 128 /*0x80*/) == 128 /*0x80*/;
    this.sourceType = (SourceTypeEnum) (((int) raw & 64512) >> 10);
    this.controllerSource = (ControllerSourceEnum) ((int) raw & (int) sbyte.MaxValue);
    this.midiContinuousControllerNumber = (ushort) ((uint) raw & (uint) sbyte.MaxValue);
  }

  public override string ToString()
  {
    return this.midiContinuousController ? $"{this.sourceType} CC{this.midiContinuousControllerNumber}" : $"{this.sourceType} {this.controllerSource}";
  }
}
