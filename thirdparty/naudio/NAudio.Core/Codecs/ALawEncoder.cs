// Decompiled with JetBrains decompiler
// Type: NAudio.Codecs.ALawEncoder
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Codecs;

public static class ALawEncoder
{
  private const int cBias = 132;
  private const int cClip = 32635;
  private static readonly byte[] ALawCompressTable = new byte[128 /*0x80*/]
  {
    (byte) 1,
    (byte) 1,
    (byte) 2,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 6,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7,
    (byte) 7
  };

  public static byte LinearToALawSample(short sample)
  {
    int num1 = (int) ~sample >> 8 & 128 /*0x80*/;
    if (num1 == 0)
      sample = -sample;
    if (sample > (short) 32635)
      sample = (short) 32635;
    byte num2;
    if (sample >= (short) 256 /*0x0100*/)
    {
      int num3 = (int) ALawEncoder.ALawCompressTable[(int) sample >> 8 & (int) sbyte.MaxValue];
      int num4 = (int) sample >> num3 + 3 & 15;
      num2 = (byte) (num3 << 4 | num4);
    }
    else
      num2 = (byte) ((uint) sample >> 4);
    return (byte) ((uint) num2 ^ (uint) (byte) (num1 ^ 85));
  }
}
