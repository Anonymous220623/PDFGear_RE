// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.Pcm8BitToSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class Pcm8BitToSampleProvider(IWaveProvider source) : SampleProviderConverterBase(source)
{
  public override int Read(float[] buffer, int offset, int count)
  {
    int num1 = count;
    this.EnsureSourceBuffer(num1);
    int num2 = this.source.Read(this.sourceBuffer, 0, num1);
    int num3 = offset;
    for (int index = 0; index < num2; ++index)
      buffer[num3++] = (float) ((double) this.sourceBuffer[index] / 128.0 - 1.0);
    return num2;
  }
}
