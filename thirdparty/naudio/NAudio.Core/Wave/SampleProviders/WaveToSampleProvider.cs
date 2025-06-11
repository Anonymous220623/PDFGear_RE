// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.WaveToSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class WaveToSampleProvider : SampleProviderConverterBase
{
  public WaveToSampleProvider(IWaveProvider source)
    : base(source)
  {
    if (source.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
      throw new ArgumentException("Must be already floating point");
  }

  public override unsafe int Read(float[] buffer, int offset, int count)
  {
    int num1 = count * 4;
    this.EnsureSourceBuffer(num1);
    int num2 = this.source.Read(this.sourceBuffer, 0, num1);
    int num3 = num2 / 4;
    int num4 = offset;
    fixed (byte* numPtr = &this.sourceBuffer[0])
    {
      int num5 = 0;
      int index = 0;
      while (num5 < num2)
      {
        buffer[num4++] = ((float*) numPtr)[index];
        num5 += 4;
        ++index;
      }
    }
    return num3;
  }
}
