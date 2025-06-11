// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.Pcm24BitToSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class Pcm24BitToSampleProvider(IWaveProvider source) : SampleProviderConverterBase(source)
{
  public override int Read(float[] buffer, int offset, int count)
  {
    int num1 = count * 3;
    this.EnsureSourceBuffer(num1);
    int num2 = this.source.Read(this.sourceBuffer, 0, num1);
    int num3 = offset;
    for (int index = 0; index < num2; index += 3)
      buffer[num3++] = (float) ((int) (sbyte) this.sourceBuffer[index + 2] << 16 /*0x10*/ | (int) this.sourceBuffer[index + 1] << 8 | (int) this.sourceBuffer[index]) / 8388608f;
    return num2 / 3;
  }
}
