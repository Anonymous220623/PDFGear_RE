// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.Stereo8SampleChunkConverter
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;

#nullable disable
namespace NAudio.Wave.SampleProviders;

internal class Stereo8SampleChunkConverter : ISampleChunkConverter
{
  private int offset;
  private byte[] sourceBuffer;
  private int sourceBytes;

  public bool Supports(WaveFormat waveFormat)
  {
    return waveFormat.Encoding == WaveFormatEncoding.Pcm && waveFormat.BitsPerSample == 8 && waveFormat.Channels == 2;
  }

  public void LoadNextChunk(IWaveProvider source, int samplePairsRequired)
  {
    int num = samplePairsRequired * 2;
    this.sourceBuffer = BufferHelpers.Ensure(this.sourceBuffer, num);
    this.sourceBytes = source.Read(this.sourceBuffer, 0, num);
    this.offset = 0;
  }

  public bool GetNextSample(out float sampleLeft, out float sampleRight)
  {
    if (this.offset < this.sourceBytes)
    {
      sampleLeft = (float) this.sourceBuffer[this.offset++] / 256f;
      sampleRight = (float) this.sourceBuffer[this.offset++] / 256f;
      return true;
    }
    sampleLeft = 0.0f;
    sampleRight = 0.0f;
    return false;
  }
}
