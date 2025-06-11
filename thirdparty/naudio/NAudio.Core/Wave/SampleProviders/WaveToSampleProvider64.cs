// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.WaveToSampleProvider64
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class WaveToSampleProvider64 : SampleProviderConverterBase
{
  public WaveToSampleProvider64(IWaveProvider source)
    : base(source)
  {
    if (source.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
      throw new ArgumentException("Must be already floating point");
  }

  public override int Read(float[] buffer, int offset, int count)
  {
    int num1 = count * 8;
    this.EnsureSourceBuffer(num1);
    int num2 = this.source.Read(this.sourceBuffer, 0, num1);
    int num3 = num2 / 8;
    int num4 = offset;
    for (int startIndex = 0; startIndex < num2; startIndex += 8)
    {
      long int64 = BitConverter.ToInt64(this.sourceBuffer, startIndex);
      buffer[num4++] = (float) BitConverter.Int64BitsToDouble(int64);
    }
    return num3;
  }
}
