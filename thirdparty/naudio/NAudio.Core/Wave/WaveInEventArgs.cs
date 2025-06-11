// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveInEventArgs
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave;

public class WaveInEventArgs : EventArgs
{
  private byte[] buffer;
  private int bytes;

  public WaveInEventArgs(byte[] buffer, int bytes)
  {
    this.buffer = buffer;
    this.bytes = bytes;
  }

  public byte[] Buffer => this.buffer;

  public int BytesRecorded => this.bytes;
}
