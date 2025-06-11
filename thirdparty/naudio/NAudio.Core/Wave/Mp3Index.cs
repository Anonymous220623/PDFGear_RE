// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Mp3Index
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Wave;

internal class Mp3Index
{
  public long FilePosition { get; set; }

  public long SamplePosition { get; set; }

  public int SampleCount { get; set; }

  public int ByteCount { get; set; }
}
