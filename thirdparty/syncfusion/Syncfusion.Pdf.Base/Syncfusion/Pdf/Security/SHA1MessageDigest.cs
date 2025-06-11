// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SHA1MessageDigest
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SHA1MessageDigest : MessageDigest
{
  private const uint c_y1 = 1518500249;
  private const uint c_y2 = 1859775393;
  private const uint c_y3 = 2400959708;
  private const uint c_y4 = 3395469782;
  private const int c_digestLength = 20;
  private uint m_h1;
  private uint m_h2;
  private uint m_h3;
  private uint m_h4;
  private uint m_h5;
  private uint[] m_x = new uint[80 /*0x50*/];
  private int m_xOff;

  public override string AlgorithmName => "SHA-1";

  internal SHA1MessageDigest() => this.Reset();

  internal SHA1MessageDigest(SHA1MessageDigest t)
    : base((MessageDigest) t)
  {
    this.m_h1 = t.m_h1;
    this.m_h2 = t.m_h2;
    this.m_h3 = t.m_h3;
    this.m_h4 = t.m_h4;
    this.m_h5 = t.m_h5;
    Array.Copy((Array) t.m_x, 0, (Array) this.m_x, 0, t.m_x.Length);
    this.m_xOff = t.m_xOff;
  }

  public override int MessageDigestSize => 20;

  internal override void ProcessWord(byte[] input, int inOff)
  {
    this.m_x[this.m_xOff] = Asn1Constants.BeToUInt32(input, inOff);
    if (++this.m_xOff != 16 /*0x10*/)
      return;
    this.ProcessBlock();
  }

  internal override void ProcessLength(long bitLength)
  {
    if (this.m_xOff > 14)
      this.ProcessBlock();
    this.m_x[14] = (uint) (bitLength >>> 32 /*0x20*/);
    this.m_x[15] = (uint) bitLength;
  }

  public override int DoFinal(byte[] bytes, int offset)
  {
    this.Finish();
    Asn1Constants.UInt32ToBe(this.m_h1, bytes, offset);
    Asn1Constants.UInt32ToBe(this.m_h2, bytes, offset + 4);
    Asn1Constants.UInt32ToBe(this.m_h3, bytes, offset + 8);
    Asn1Constants.UInt32ToBe(this.m_h4, bytes, offset + 12);
    Asn1Constants.UInt32ToBe(this.m_h5, bytes, offset + 16 /*0x10*/);
    this.Reset();
    return 20;
  }

  public override void Reset()
  {
    base.Reset();
    this.m_h1 = 1732584193U;
    this.m_h2 = 4023233417U;
    this.m_h3 = 2562383102U;
    this.m_h4 = 271733878U;
    this.m_h5 = 3285377520U;
    this.m_xOff = 0;
    Array.Clear((Array) this.m_x, 0, this.m_x.Length);
  }

  internal override void ProcessBlock()
  {
    for (int index = 16 /*0x10*/; index < 80 /*0x50*/; ++index)
    {
      uint num = this.m_x[index - 3] ^ this.m_x[index - 8] ^ this.m_x[index - 14] ^ this.m_x[index - 16 /*0x10*/];
      this.m_x[index] = num << 1 | num >> 31 /*0x1F*/;
    }
    uint u1 = this.m_h1;
    uint u2 = this.m_h2;
    uint num1 = this.m_h3;
    uint num2 = this.m_h4;
    uint num3 = this.m_h5;
    int num4 = 0;
    for (int index1 = 0; index1 < 4; ++index1)
    {
      int num5 = (int) num3;
      int num6 = ((int) u1 << 5 | (int) (u1 >> 27)) + (int) this.F(u2, num1, num2);
      uint[] x1 = this.m_x;
      int index2 = num4;
      int num7 = index2 + 1;
      int num8 = (int) x1[index2];
      int num9 = num6 + num8 + 1518500249;
      uint u3 = (uint) (num5 + num9);
      uint num10 = u2 << 30 | u2 >> 2;
      int num11 = (int) num2;
      int num12 = ((int) u3 << 5 | (int) (u3 >> 27)) + (int) this.F(u1, num10, num1);
      uint[] x2 = this.m_x;
      int index3 = num7;
      int num13 = index3 + 1;
      int num14 = (int) x2[index3];
      int num15 = num12 + num14 + 1518500249;
      uint u4 = (uint) (num11 + num15);
      uint num16 = u1 << 30 | u1 >> 2;
      int num17 = (int) num1;
      int num18 = ((int) u4 << 5 | (int) (u4 >> 27)) + (int) this.F(u3, num16, num10);
      uint[] x3 = this.m_x;
      int index4 = num13;
      int num19 = index4 + 1;
      int num20 = (int) x3[index4];
      int num21 = num18 + num20 + 1518500249;
      uint u5 = (uint) (num17 + num21);
      num3 = u3 << 30 | u3 >> 2;
      int num22 = (int) num10;
      int num23 = ((int) u5 << 5 | (int) (u5 >> 27)) + (int) this.F(u4, num3, num16);
      uint[] x4 = this.m_x;
      int index5 = num19;
      int num24 = index5 + 1;
      int num25 = (int) x4[index5];
      int num26 = num23 + num25 + 1518500249;
      u2 = (uint) (num22 + num26);
      num2 = u4 << 30 | u4 >> 2;
      int num27 = (int) num16;
      int num28 = ((int) u2 << 5 | (int) (u2 >> 27)) + (int) this.F(u5, num2, num3);
      uint[] x5 = this.m_x;
      int index6 = num24;
      num4 = index6 + 1;
      int num29 = (int) x5[index6];
      int num30 = num28 + num29 + 1518500249;
      u1 = (uint) (num27 + num30);
      num1 = u5 << 30 | u5 >> 2;
    }
    for (int index7 = 0; index7 < 4; ++index7)
    {
      int num31 = (int) num3;
      int num32 = ((int) u1 << 5 | (int) (u1 >> 27)) + (int) this.H(u2, num1, num2);
      uint[] x6 = this.m_x;
      int index8 = num4;
      int num33 = index8 + 1;
      int num34 = (int) x6[index8];
      int num35 = num32 + num34 + 1859775393;
      uint u6 = (uint) (num31 + num35);
      uint num36 = u2 << 30 | u2 >> 2;
      int num37 = (int) num2;
      int num38 = ((int) u6 << 5 | (int) (u6 >> 27)) + (int) this.H(u1, num36, num1);
      uint[] x7 = this.m_x;
      int index9 = num33;
      int num39 = index9 + 1;
      int num40 = (int) x7[index9];
      int num41 = num38 + num40 + 1859775393;
      uint u7 = (uint) (num37 + num41);
      uint num42 = u1 << 30 | u1 >> 2;
      int num43 = (int) num1;
      int num44 = ((int) u7 << 5 | (int) (u7 >> 27)) + (int) this.H(u6, num42, num36);
      uint[] x8 = this.m_x;
      int index10 = num39;
      int num45 = index10 + 1;
      int num46 = (int) x8[index10];
      int num47 = num44 + num46 + 1859775393;
      uint u8 = (uint) (num43 + num47);
      num3 = u6 << 30 | u6 >> 2;
      int num48 = (int) num36;
      int num49 = ((int) u8 << 5 | (int) (u8 >> 27)) + (int) this.H(u7, num3, num42);
      uint[] x9 = this.m_x;
      int index11 = num45;
      int num50 = index11 + 1;
      int num51 = (int) x9[index11];
      int num52 = num49 + num51 + 1859775393;
      u2 = (uint) (num48 + num52);
      num2 = u7 << 30 | u7 >> 2;
      int num53 = (int) num42;
      int num54 = ((int) u2 << 5 | (int) (u2 >> 27)) + (int) this.H(u8, num2, num3);
      uint[] x10 = this.m_x;
      int index12 = num50;
      num4 = index12 + 1;
      int num55 = (int) x10[index12];
      int num56 = num54 + num55 + 1859775393;
      u1 = (uint) (num53 + num56);
      num1 = u8 << 30 | u8 >> 2;
    }
    for (int index13 = 0; index13 < 4; ++index13)
    {
      int num57 = (int) num3;
      int num58 = ((int) u1 << 5 | (int) (u1 >> 27)) + (int) this.G(u2, num1, num2);
      uint[] x11 = this.m_x;
      int index14 = num4;
      int num59 = index14 + 1;
      int num60 = (int) x11[index14];
      int num61 = num58 + num60 - 1894007588;
      uint u9 = (uint) (num57 + num61);
      uint num62 = u2 << 30 | u2 >> 2;
      int num63 = (int) num2;
      int num64 = ((int) u9 << 5 | (int) (u9 >> 27)) + (int) this.G(u1, num62, num1);
      uint[] x12 = this.m_x;
      int index15 = num59;
      int num65 = index15 + 1;
      int num66 = (int) x12[index15];
      int num67 = num64 + num66 - 1894007588;
      uint u10 = (uint) (num63 + num67);
      uint num68 = u1 << 30 | u1 >> 2;
      int num69 = (int) num1;
      int num70 = ((int) u10 << 5 | (int) (u10 >> 27)) + (int) this.G(u9, num68, num62);
      uint[] x13 = this.m_x;
      int index16 = num65;
      int num71 = index16 + 1;
      int num72 = (int) x13[index16];
      int num73 = num70 + num72 - 1894007588;
      uint u11 = (uint) (num69 + num73);
      num3 = u9 << 30 | u9 >> 2;
      int num74 = (int) num62;
      int num75 = ((int) u11 << 5 | (int) (u11 >> 27)) + (int) this.G(u10, num3, num68);
      uint[] x14 = this.m_x;
      int index17 = num71;
      int num76 = index17 + 1;
      int num77 = (int) x14[index17];
      int num78 = num75 + num77 - 1894007588;
      u2 = (uint) (num74 + num78);
      num2 = u10 << 30 | u10 >> 2;
      int num79 = (int) num68;
      int num80 = ((int) u2 << 5 | (int) (u2 >> 27)) + (int) this.G(u11, num2, num3);
      uint[] x15 = this.m_x;
      int index18 = num76;
      num4 = index18 + 1;
      int num81 = (int) x15[index18];
      int num82 = num80 + num81 - 1894007588;
      u1 = (uint) (num79 + num82);
      num1 = u11 << 30 | u11 >> 2;
    }
    for (int index19 = 0; index19 < 4; ++index19)
    {
      int num83 = (int) num3;
      int num84 = ((int) u1 << 5 | (int) (u1 >> 27)) + (int) this.H(u2, num1, num2);
      uint[] x16 = this.m_x;
      int index20 = num4;
      int num85 = index20 + 1;
      int num86 = (int) x16[index20];
      int num87 = num84 + num86 - 899497514;
      uint u12 = (uint) (num83 + num87);
      uint num88 = u2 << 30 | u2 >> 2;
      int num89 = (int) num2;
      int num90 = ((int) u12 << 5 | (int) (u12 >> 27)) + (int) this.H(u1, num88, num1);
      uint[] x17 = this.m_x;
      int index21 = num85;
      int num91 = index21 + 1;
      int num92 = (int) x17[index21];
      int num93 = num90 + num92 - 899497514;
      uint u13 = (uint) (num89 + num93);
      uint num94 = u1 << 30 | u1 >> 2;
      int num95 = (int) num1;
      int num96 = ((int) u13 << 5 | (int) (u13 >> 27)) + (int) this.H(u12, num94, num88);
      uint[] x18 = this.m_x;
      int index22 = num91;
      int num97 = index22 + 1;
      int num98 = (int) x18[index22];
      int num99 = num96 + num98 - 899497514;
      uint u14 = (uint) (num95 + num99);
      num3 = u12 << 30 | u12 >> 2;
      int num100 = (int) num88;
      int num101 = ((int) u14 << 5 | (int) (u14 >> 27)) + (int) this.H(u13, num3, num94);
      uint[] x19 = this.m_x;
      int index23 = num97;
      int num102 = index23 + 1;
      int num103 = (int) x19[index23];
      int num104 = num101 + num103 - 899497514;
      u2 = (uint) (num100 + num104);
      num2 = u13 << 30 | u13 >> 2;
      int num105 = (int) num94;
      int num106 = ((int) u2 << 5 | (int) (u2 >> 27)) + (int) this.H(u14, num2, num3);
      uint[] x20 = this.m_x;
      int index24 = num102;
      num4 = index24 + 1;
      int num107 = (int) x20[index24];
      int num108 = num106 + num107 - 899497514;
      u1 = (uint) (num105 + num108);
      num1 = u14 << 30 | u14 >> 2;
    }
    this.m_h1 += u1;
    this.m_h2 += u2;
    this.m_h3 += num1;
    this.m_h4 += num2;
    this.m_h5 += num3;
    this.m_xOff = 0;
    Array.Clear((Array) this.m_x, 0, 16 /*0x10*/);
  }

  private uint F(uint u, uint v, uint w) => (uint) ((int) u & (int) v | ~(int) u & (int) w);

  private uint H(uint u, uint v, uint w) => u ^ v ^ w;

  private uint G(uint u, uint v, uint w)
  {
    return (uint) ((int) u & (int) v | (int) u & (int) w | (int) v & (int) w);
  }
}
