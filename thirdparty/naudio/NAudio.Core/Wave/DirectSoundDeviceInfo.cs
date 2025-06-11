// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.DirectSoundDeviceInfo
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave;

public class DirectSoundDeviceInfo
{
  public Guid Guid { get; set; }

  public string Description { get; set; }

  public string ModuleName { get; set; }
}
