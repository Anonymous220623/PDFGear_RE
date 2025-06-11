// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.Pcm16BitToSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class Pcm16BitToSampleProvider(IWaveProvider source) : SampleProviderConverterBase(source)
{
  public override int Read(float[] buffer, int offset, int count)
  {
    int num1 = count * 2;
    this.EnsureSourceBuffer(num1);
    int num2 = this.source.Read(this.sourceBuffer, 0, num1);
    int num3 = offset;
    for (int startIndex = 0; startIndex < num2; startIndex += 2)
      buffer[num3++] = (float) BitConverter.ToInt16(this.sourceBuffer, startIndex) / 32768f;
    return num2 / 2;
  }
}
