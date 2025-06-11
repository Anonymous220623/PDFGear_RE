// Decompiled with JetBrains decompiler
// Type: NAudio.Codecs.G722Codec
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Codecs;

public class G722Codec
{
  private static readonly int[] wl = new int[8]
  {
    -60,
    -30,
    58,
    172,
    334,
    538,
    1198,
    3042
  };
  private static readonly int[] rl42 = new int[16 /*0x10*/]
  {
    0,
    7,
    6,
    5,
    4,
    3,
    2,
    1,
    7,
    6,
    5,
    4,
    3,
    2,
    1,
    0
  };
  private static readonly int[] ilb = new int[32 /*0x20*/]
  {
    2048 /*0x0800*/,
    2093,
    2139,
    2186,
    2233,
    2282,
    2332,
    2383,
    2435,
    2489,
    2543,
    2599,
    2656,
    2714,
    2774,
    2834,
    2896,
    2960,
    3025,
    3091,
    3158,
    3228,
    3298,
    3371,
    3444,
    3520,
    3597,
    3676,
    3756,
    3838,
    3922,
    4008
  };
  private static readonly int[] wh = new int[3]
  {
    0,
    -214,
    798
  };
  private static readonly int[] rh2 = new int[4]
  {
    2,
    1,
    2,
    1
  };
  private static readonly int[] qm2 = new int[4]
  {
    -7408,
    -1616,
    7408,
    1616
  };
  private static readonly int[] qm4 = new int[16 /*0x10*/]
  {
    0,
    -20456,
    -12896,
    -8968,
    -6288,
    -4240,
    -2584,
    -1200,
    20456,
    12896,
    8968,
    6288,
    4240,
    2584,
    1200,
    0
  };
  private static readonly int[] qm5 = new int[32 /*0x20*/]
  {
    -280,
    -280,
    -23352,
    -17560,
    -14120,
    -11664,
    -9752,
    -8184,
    -6864,
    -5712,
    -4696,
    -3784,
    -2960,
    -2208,
    -1520,
    -880,
    23352,
    17560,
    14120,
    11664,
    9752,
    8184,
    6864,
    5712,
    4696,
    3784,
    2960,
    2208,
    1520,
    880,
    280,
    -280
  };
  private static readonly int[] qm6 = new int[64 /*0x40*/]
  {
    -136,
    -136,
    -136,
    -136,
    -24808,
    -21904,
    -19008,
    -16704,
    -14984,
    -13512,
    -12280,
    -11192,
    -10232,
    -9360,
    -8576,
    -7856,
    -7192,
    -6576,
    -6000,
    -5456,
    -4944,
    -4464,
    -4008,
    -3576,
    -3168,
    -2776,
    -2400,
    -2032,
    -1688,
    -1360,
    -1040,
    -728,
    24808,
    21904,
    19008,
    16704,
    14984,
    13512,
    12280,
    11192,
    10232,
    9360,
    8576,
    7856,
    7192,
    6576,
    6000,
    5456,
    4944,
    4464,
    4008,
    3576,
    3168,
    2776,
    2400,
    2032,
    1688,
    1360,
    1040,
    728,
    432,
    136,
    -432,
    -136
  };
  private static readonly int[] qmf_coeffs = new int[12]
  {
    3,
    -11,
    12,
    32 /*0x20*/,
    -210,
    951,
    3876,
    -805,
    362,
    -156,
    53,
    -11
  };
  private static readonly int[] q6 = new int[32 /*0x20*/]
  {
    0,
    35,
    72,
    110,
    150,
    190,
    233,
    276,
    323,
    370,
    422,
    473,
    530,
    587,
    650,
    714,
    786,
    858,
    940,
    1023 /*0x03FF*/,
    1121,
    1219,
    1339,
    1458,
    1612,
    1765,
    1980,
    2195,
    2557,
    2919,
    0,
    0
  };
  private static readonly int[] iln = new int[32 /*0x20*/]
  {
    0,
    63 /*0x3F*/,
    62,
    31 /*0x1F*/,
    30,
    29,
    28,
    27,
    26,
    25,
    24,
    23,
    22,
    21,
    20,
    19,
    18,
    17,
    16 /*0x10*/,
    15,
    14,
    13,
    12,
    11,
    10,
    9,
    8,
    7,
    6,
    5,
    4,
    0
  };
  private static readonly int[] ilp = new int[32 /*0x20*/]
  {
    0,
    61,
    60,
    59,
    58,
    57,
    56,
    55,
    54,
    53,
    52,
    51,
    50,
    49,
    48 /*0x30*/,
    47,
    46,
    45,
    44,
    43,
    42,
    41,
    40,
    39,
    38,
    37,
    36,
    35,
    34,
    33,
    32 /*0x20*/,
    0
  };
  private static readonly int[] ihn = new int[3]{ 0, 1, 0 };
  private static readonly int[] ihp = new int[3]{ 0, 3, 2 };

  private static short Saturate(int amp)
  {
    short num = (short) amp;
    if (amp == (int) num)
      return num;
    return amp > (int) short.MaxValue ? short.MaxValue : short.MinValue;
  }

  private static void Block4(G722CodecState s, int band, int d)
  {
    s.Band[band].d[0] = d;
    s.Band[band].r[0] = (int) G722Codec.Saturate(s.Band[band].s + d);
    s.Band[band].p[0] = (int) G722Codec.Saturate(s.Band[band].sz + d);
    for (int index = 0; index < 3; ++index)
      s.Band[band].sg[index] = s.Band[band].p[index] >> 15;
    int num1 = (int) G722Codec.Saturate(s.Band[band].a[1] << 2);
    int num2 = s.Band[band].sg[0] == s.Band[band].sg[1] ? -num1 : num1;
    if (num2 > (int) short.MaxValue)
      num2 = (int) short.MaxValue;
    int num3 = (s.Band[band].sg[0] == s.Band[band].sg[2] ? 128 /*0x80*/ : (int) sbyte.MinValue) + (num2 >> 7) + (s.Band[band].a[2] * 32512 >> 15);
    if (num3 > 12288 /*0x3000*/)
      num3 = 12288 /*0x3000*/;
    else if (num3 < -12288)
      num3 = -12288;
    s.Band[band].ap[2] = num3;
    s.Band[band].sg[0] = s.Band[band].p[0] >> 15;
    s.Band[band].sg[1] = s.Band[band].p[1] >> 15;
    int num4 = s.Band[band].sg[0] == s.Band[band].sg[1] ? 192 /*0xC0*/ : -192;
    int num5 = s.Band[band].a[1] * 32640 >> 15;
    s.Band[band].ap[1] = (int) G722Codec.Saturate(num4 + num5);
    int num6 = (int) G722Codec.Saturate(15360 - s.Band[band].ap[2]);
    if (s.Band[band].ap[1] > num6)
      s.Band[band].ap[1] = num6;
    else if (s.Band[band].ap[1] < -num6)
      s.Band[band].ap[1] = -num6;
    int num7 = d == 0 ? 0 : 128 /*0x80*/;
    s.Band[band].sg[0] = d >> 15;
    for (int index = 1; index < 7; ++index)
    {
      s.Band[band].sg[index] = s.Band[band].d[index] >> 15;
      int num8 = s.Band[band].sg[index] == s.Band[band].sg[0] ? num7 : -num7;
      int num9 = s.Band[band].b[index] * 32640 >> 15;
      s.Band[band].bp[index] = (int) G722Codec.Saturate(num8 + num9);
    }
    for (int index = 6; index > 0; --index)
    {
      s.Band[band].d[index] = s.Band[band].d[index - 1];
      s.Band[band].b[index] = s.Band[band].bp[index];
    }
    for (int index = 2; index > 0; --index)
    {
      s.Band[band].r[index] = s.Band[band].r[index - 1];
      s.Band[band].p[index] = s.Band[band].p[index - 1];
      s.Band[band].a[index] = s.Band[band].ap[index];
    }
    int num10 = (int) G722Codec.Saturate(s.Band[band].r[1] + s.Band[band].r[1]);
    int num11 = s.Band[band].a[1] * num10 >> 15;
    int num12 = (int) G722Codec.Saturate(s.Band[band].r[2] + s.Band[band].r[2]);
    int num13 = s.Band[band].a[2] * num12 >> 15;
    s.Band[band].sp = (int) G722Codec.Saturate(num11 + num13);
    s.Band[band].sz = 0;
    for (int index = 6; index > 0; --index)
    {
      int num14 = (int) G722Codec.Saturate(s.Band[band].d[index] + s.Band[band].d[index]);
      s.Band[band].sz += s.Band[band].b[index] * num14 >> 15;
    }
    s.Band[band].sz = (int) G722Codec.Saturate(s.Band[band].sz);
    s.Band[band].s = (int) G722Codec.Saturate(s.Band[band].sp + s.Band[band].sz);
  }

  public int Decode(
    G722CodecState state,
    short[] outputBuffer,
    byte[] inputG722Data,
    int inputLength)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    while (num3 < inputLength)
    {
      int num4;
      if (state.Packed)
      {
        if (state.InBits < state.BitsPerSample)
        {
          state.InBuffer |= (uint) inputG722Data[num3++] << state.InBits;
          state.InBits += 8;
        }
        num4 = (int) state.InBuffer & (1 << state.BitsPerSample) - 1;
        state.InBuffer >>= state.BitsPerSample;
        state.InBits -= state.BitsPerSample;
      }
      else
        num4 = (int) inputG722Data[num3++];
      int index1;
      int num5;
      int index2;
      switch (state.BitsPerSample)
      {
        case 6:
          index2 = num4 & 15;
          index1 = num4 >> 4 & 3;
          num5 = G722Codec.qm4[index2];
          break;
        case 7:
          int index3 = num4 & 31 /*0x1F*/;
          index1 = num4 >> 5 & 3;
          num5 = G722Codec.qm5[index3];
          index2 = index3 >> 1;
          break;
        default:
          int index4 = num4 & 63 /*0x3F*/;
          index1 = num4 >> 6 & 3;
          num5 = G722Codec.qm6[index4];
          index2 = index4 >> 2;
          break;
      }
      int num6 = state.Band[0].det * num5 >> 15;
      int num7 = state.Band[0].s + num6;
      if (num7 > 16383 /*0x3FFF*/)
        num7 = 16383 /*0x3FFF*/;
      else if (num7 < -16384)
        num7 = -16384;
      int num8 = G722Codec.qm4[index2];
      int d1 = state.Band[0].det * num8 >> 15;
      int index5 = G722Codec.rl42[index2];
      int num9 = (state.Band[0].nb * (int) sbyte.MaxValue >> 7) + G722Codec.wl[index5];
      if (num9 < 0)
        num9 = 0;
      else if (num9 > 18432)
        num9 = 18432;
      state.Band[0].nb = num9;
      int index6 = state.Band[0].nb >> 6 & 31 /*0x1F*/;
      int num10 = 8 - (state.Band[0].nb >> 11);
      int num11 = num10 < 0 ? G722Codec.ilb[index6] << -num10 : G722Codec.ilb[index6] >> num10;
      state.Band[0].det = num11 << 2;
      G722Codec.Block4(state, 0, d1);
      if (!state.EncodeFrom8000Hz)
      {
        int num12 = G722Codec.qm2[index1];
        int d2 = state.Band[1].det * num12 >> 15;
        num2 = d2 + state.Band[1].s;
        if (num2 > 16383 /*0x3FFF*/)
          num2 = 16383 /*0x3FFF*/;
        else if (num2 < -16384)
          num2 = -16384;
        int index7 = G722Codec.rh2[index1];
        int num13 = (state.Band[1].nb * (int) sbyte.MaxValue >> 7) + G722Codec.wh[index7];
        if (num13 < 0)
          num13 = 0;
        else if (num13 > 22528)
          num13 = 22528;
        state.Band[1].nb = num13;
        int index8 = state.Band[1].nb >> 6 & 31 /*0x1F*/;
        int num14 = 10 - (state.Band[1].nb >> 11);
        int num15 = num14 < 0 ? G722Codec.ilb[index8] << -num14 : G722Codec.ilb[index8] >> num14;
        state.Band[1].det = num15 << 2;
        G722Codec.Block4(state, 1, d2);
      }
      if (state.ItuTestMode)
      {
        short[] numArray1 = outputBuffer;
        int index9 = num1;
        int num16 = index9 + 1;
        int num17 = (int) (short) (num7 << 1);
        numArray1[index9] = (short) num17;
        short[] numArray2 = outputBuffer;
        int index10 = num16;
        num1 = index10 + 1;
        int num18 = (int) (short) (num2 << 1);
        numArray2[index10] = (short) num18;
      }
      else if (state.EncodeFrom8000Hz)
      {
        outputBuffer[num1++] = (short) (num7 << 1);
      }
      else
      {
        for (int index11 = 0; index11 < 22; ++index11)
          state.QmfSignalHistory[index11] = state.QmfSignalHistory[index11 + 2];
        state.QmfSignalHistory[22] = num7 + num2;
        state.QmfSignalHistory[23] = num7 - num2;
        int num19 = 0;
        int num20 = 0;
        for (int index12 = 0; index12 < 12; ++index12)
        {
          num20 += state.QmfSignalHistory[2 * index12] * G722Codec.qmf_coeffs[index12];
          num19 += state.QmfSignalHistory[2 * index12 + 1] * G722Codec.qmf_coeffs[11 - index12];
        }
        short[] numArray3 = outputBuffer;
        int index13 = num1;
        int num21 = index13 + 1;
        int num22 = (int) (short) (num19 >> 11);
        numArray3[index13] = (short) num22;
        short[] numArray4 = outputBuffer;
        int index14 = num21;
        num1 = index14 + 1;
        int num23 = (int) (short) (num20 >> 11);
        numArray4[index14] = (short) num23;
      }
    }
    return num1;
  }

  public int Encode(
    G722CodecState state,
    byte[] outputBuffer,
    short[] inputBuffer,
    int inputBufferCount)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    while (num3 < inputBufferCount)
    {
      int num4;
      if (state.ItuTestMode)
      {
        short[] numArray = inputBuffer;
        int index = num3++;
        num4 = num2 = (int) numArray[index] >> 1;
      }
      else if (state.EncodeFrom8000Hz)
      {
        num4 = (int) inputBuffer[num3++] >> 1;
      }
      else
      {
        for (int index = 0; index < 22; ++index)
          state.QmfSignalHistory[index] = state.QmfSignalHistory[index + 2];
        int[] qmfSignalHistory1 = state.QmfSignalHistory;
        short[] numArray1 = inputBuffer;
        int index1 = num3;
        int num5 = index1 + 1;
        int num6 = (int) numArray1[index1];
        qmfSignalHistory1[22] = num6;
        int[] qmfSignalHistory2 = state.QmfSignalHistory;
        short[] numArray2 = inputBuffer;
        int index2 = num5;
        num3 = index2 + 1;
        int num7 = (int) numArray2[index2];
        qmfSignalHistory2[23] = num7;
        int num8 = 0;
        int num9 = 0;
        for (int index3 = 0; index3 < 12; ++index3)
        {
          num9 += state.QmfSignalHistory[2 * index3] * G722Codec.qmf_coeffs[index3];
          num8 += state.QmfSignalHistory[2 * index3 + 1] * G722Codec.qmf_coeffs[11 - index3];
        }
        num4 = num8 + num9 >> 14;
        num2 = num8 - num9 >> 14;
      }
      int num10 = (int) G722Codec.Saturate(num4 - state.Band[0].s);
      int num11 = num10 >= 0 ? num10 : -(num10 + 1);
      int index4;
      for (index4 = 1; index4 < 30; ++index4)
      {
        int num12 = G722Codec.q6[index4] * state.Band[0].det >> 12;
        if (num11 < num12)
          break;
      }
      int num13 = num10 < 0 ? G722Codec.iln[index4] : G722Codec.ilp[index4];
      int index5 = num13 >> 2;
      int num14 = G722Codec.qm4[index5];
      int d1 = state.Band[0].det * num14 >> 15;
      int index6 = G722Codec.rl42[index5];
      int num15 = state.Band[0].nb * (int) sbyte.MaxValue >> 7;
      state.Band[0].nb = num15 + G722Codec.wl[index6];
      if (state.Band[0].nb < 0)
        state.Band[0].nb = 0;
      else if (state.Band[0].nb > 18432)
        state.Band[0].nb = 18432;
      int index7 = state.Band[0].nb >> 6 & 31 /*0x1F*/;
      int num16 = 8 - (state.Band[0].nb >> 11);
      int num17 = num16 < 0 ? G722Codec.ilb[index7] << -num16 : G722Codec.ilb[index7] >> num16;
      state.Band[0].det = num17 << 2;
      G722Codec.Block4(state, 0, d1);
      int num18;
      if (state.EncodeFrom8000Hz)
      {
        num18 = (192 /*0xC0*/ | num13) >> 8 - state.BitsPerSample;
      }
      else
      {
        int num19 = (int) G722Codec.Saturate(num2 - state.Band[1].s);
        int index8 = (num19 >= 0 ? num19 : -(num19 + 1)) >= 564 * state.Band[1].det >> 12 ? 2 : 1;
        int index9 = num19 < 0 ? G722Codec.ihn[index8] : G722Codec.ihp[index8];
        int num20 = G722Codec.qm2[index9];
        int d2 = state.Band[1].det * num20 >> 15;
        int index10 = G722Codec.rh2[index9];
        int num21 = state.Band[1].nb * (int) sbyte.MaxValue >> 7;
        state.Band[1].nb = num21 + G722Codec.wh[index10];
        if (state.Band[1].nb < 0)
          state.Band[1].nb = 0;
        else if (state.Band[1].nb > 22528)
          state.Band[1].nb = 22528;
        int index11 = state.Band[1].nb >> 6 & 31 /*0x1F*/;
        int num22 = 10 - (state.Band[1].nb >> 11);
        int num23 = num22 < 0 ? G722Codec.ilb[index11] << -num22 : G722Codec.ilb[index11] >> num22;
        state.Band[1].det = num23 << 2;
        G722Codec.Block4(state, 1, d2);
        num18 = (index9 << 6 | num13) >> 8 - state.BitsPerSample;
      }
      if (state.Packed)
      {
        state.OutBuffer |= (uint) (num18 << state.OutBits);
        state.OutBits += state.BitsPerSample;
        if (state.OutBits >= 8)
        {
          outputBuffer[num1++] = (byte) (state.OutBuffer & (uint) byte.MaxValue);
          state.OutBits -= 8;
          state.OutBuffer >>= 8;
        }
      }
      else
        outputBuffer[num1++] = (byte) num18;
    }
    return num1;
  }
}
