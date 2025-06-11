// Decompiled with JetBrains decompiler
// Type: NAudio.Dsp.ImpulseResponseConvolution
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Dsp;

public class ImpulseResponseConvolution
{
  public float[] Convolve(float[] input, float[] impulseResponse)
  {
    float[] data = new float[input.Length + impulseResponse.Length];
    for (int index1 = 0; index1 < data.Length; ++index1)
    {
      for (int index2 = 0; index2 < impulseResponse.Length; ++index2)
      {
        if (index1 >= index2 && index1 - index2 < input.Length)
          data[index1] += impulseResponse[index2] * input[index1 - index2];
      }
    }
    this.Normalize(data);
    return data;
  }

  public void Normalize(float[] data)
  {
    float val1 = 0.0f;
    for (int index = 0; index < data.Length; ++index)
      val1 = Math.Max(val1, Math.Abs(data[index]));
    if ((double) val1 <= 1.0)
      return;
    for (int index = 0; index < data.Length; ++index)
      data[index] /= val1;
  }
}
