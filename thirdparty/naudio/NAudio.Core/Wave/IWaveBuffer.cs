// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.IWaveBuffer
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Wave;

public interface IWaveBuffer
{
  byte[] ByteBuffer { get; }

  float[] FloatBuffer { get; }

  short[] ShortBuffer { get; }

  int[] IntBuffer { get; }

  int MaxSize { get; }

  int ByteBufferCount { get; }

  int FloatBufferCount { get; }

  int ShortBufferCount { get; }

  int IntBufferCount { get; }
}
