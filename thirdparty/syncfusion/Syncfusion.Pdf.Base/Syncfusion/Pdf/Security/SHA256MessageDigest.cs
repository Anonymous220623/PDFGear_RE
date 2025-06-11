// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SHA256MessageDigest
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SHA256MessageDigest : MessageDigest
{
  private const int c_digestLength = 32 /*0x20*/;
  private uint m_h1;
  private uint m_h2;
  private uint m_h3;
  private uint m_h4;
  private uint m_h5;
  private uint m_h6;
  private uint m_h7;
  private uint m_h8;
  private uint[] m_x = new uint[64 /*0x40*/];
  private int m_xOff;
  private static readonly uint[] K = new uint[64 /*0x40*/]
  {
    1116352408U,
    1899447441U,
    3049323471U,
    3921009573U,
    961987163U,
    1508970993U,
    2453635748U,
    2870763221U,
    3624381080U,
    310598401U,
    607225278U,
    1426881987U,
    1925078388U,
    2162078206U,
    2614888103U,
    3248222580U,
    3835390401U,
    4022224774U,
    264347078U,
    604807628U,
    770255983U,
    1249150122U,
    1555081692U,
    1996064986U,
    2554220882U,
    2821834349U,
    2952996808U,
    3210313671U,
    3336571891U,
    3584528711U,
    113926993U,
    338241895U,
    666307205U,
    773529912U,
    1294757372U,
    1396182291U,
    1695183700U,
    1986661051U,
    2177026350U,
    2456956037U,
    2730485921U,
    2820302411U,
    3259730800U,
    3345764771U,
    3516065817U,
    3600352804U,
    4094571909U,
    275423344U,
    430227734U,
    506948616U,
    659060556U,
    883997877U,
    958139571U,
    1322822218U,
    1537002063U,
    1747873779U,
    1955562222U,
    2024104815U,
    2227730452U,
    2361852424U,
    2428436474U,
    2756734187U,
    3204031479U,
    3329325298U
  };

  internal SHA256MessageDigest() => this.initHs();

  internal SHA256MessageDigest(SHA256MessageDigest t)
    : base((MessageDigest) t)
  {
    this.m_h1 = t.m_h1;
    this.m_h2 = t.m_h2;
    this.m_h3 = t.m_h3;
    this.m_h4 = t.m_h4;
    this.m_h5 = t.m_h5;
    this.m_h6 = t.m_h6;
    this.m_h7 = t.m_h7;
    this.m_h8 = t.m_h8;
    Array.Copy((Array) t.m_x, 0, (Array) this.m_x, 0, t.m_x.Length);
    this.m_xOff = t.m_xOff;
  }

  public override string AlgorithmName => "SHA-256";

  public override int MessageDigestSize => 32 /*0x20*/;

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
    Asn1Constants.UInt32ToBe(this.m_h6, bytes, offset + 20);
    Asn1Constants.UInt32ToBe(this.m_h7, bytes, offset + 24);
    Asn1Constants.UInt32ToBe(this.m_h8, bytes, offset + 28);
    this.Reset();
    return 32 /*0x20*/;
  }

  public override void Reset()
  {
    base.Reset();
    this.initHs();
    this.m_xOff = 0;
    Array.Clear((Array) this.m_x, 0, this.m_x.Length);
  }

  private void initHs()
  {
    this.m_h1 = 1779033703U;
    this.m_h2 = 3144134277U;
    this.m_h3 = 1013904242U;
    this.m_h4 = 2773480762U;
    this.m_h5 = 1359893119U;
    this.m_h6 = 2600822924U;
    this.m_h7 = 528734635U;
    this.m_h8 = 1541459225U;
  }

  internal override void ProcessBlock()
  {
    for (int index = 16 /*0x10*/; index <= 63 /*0x3F*/; ++index)
      this.m_x[index] = SHA256MessageDigest.Theta1(this.m_x[index - 2]) + this.m_x[index - 7] + SHA256MessageDigest.Theta0(this.m_x[index - 15]) + this.m_x[index - 16 /*0x10*/];
    uint num1 = this.m_h1;
    uint num2 = this.m_h2;
    uint num3 = this.m_h3;
    uint num4 = this.m_h4;
    uint num5 = this.m_h5;
    uint num6 = this.m_h6;
    uint num7 = this.m_h7;
    uint num8 = this.m_h8;
    int index1 = 0;
    for (int index2 = 0; index2 < 8; ++index2)
    {
      uint num9 = num8 + (SHA256MessageDigest.Sum1Ch(num5, num6, num7) + SHA256MessageDigest.K[index1] + this.m_x[index1]);
      uint num10 = num4 + num9;
      uint num11 = num9 + SHA256MessageDigest.Sum0Maj(num1, num2, num3);
      int index3 = index1 + 1;
      uint num12 = num7 + (SHA256MessageDigest.Sum1Ch(num10, num5, num6) + SHA256MessageDigest.K[index3] + this.m_x[index3]);
      uint num13 = num3 + num12;
      uint num14 = num12 + SHA256MessageDigest.Sum0Maj(num11, num1, num2);
      int index4 = index3 + 1;
      uint num15 = num6 + (SHA256MessageDigest.Sum1Ch(num13, num10, num5) + SHA256MessageDigest.K[index4] + this.m_x[index4]);
      uint num16 = num2 + num15;
      uint num17 = num15 + SHA256MessageDigest.Sum0Maj(num14, num11, num1);
      int index5 = index4 + 1;
      uint num18 = num5 + (SHA256MessageDigest.Sum1Ch(num16, num13, num10) + SHA256MessageDigest.K[index5] + this.m_x[index5]);
      uint num19 = num1 + num18;
      uint num20 = num18 + SHA256MessageDigest.Sum0Maj(num17, num14, num11);
      int index6 = index5 + 1;
      uint num21 = num10 + (SHA256MessageDigest.Sum1Ch(num19, num16, num13) + SHA256MessageDigest.K[index6] + this.m_x[index6]);
      num8 = num11 + num21;
      num4 = num21 + SHA256MessageDigest.Sum0Maj(num20, num17, num14);
      int index7 = index6 + 1;
      uint num22 = num13 + (SHA256MessageDigest.Sum1Ch(num8, num19, num16) + SHA256MessageDigest.K[index7] + this.m_x[index7]);
      num7 = num14 + num22;
      num3 = num22 + SHA256MessageDigest.Sum0Maj(num4, num20, num17);
      int index8 = index7 + 1;
      uint num23 = num16 + (SHA256MessageDigest.Sum1Ch(num7, num8, num19) + SHA256MessageDigest.K[index8] + this.m_x[index8]);
      num6 = num17 + num23;
      num2 = num23 + SHA256MessageDigest.Sum0Maj(num3, num4, num20);
      int index9 = index8 + 1;
      uint num24 = num19 + (SHA256MessageDigest.Sum1Ch(num6, num7, num8) + SHA256MessageDigest.K[index9] + this.m_x[index9]);
      num5 = num20 + num24;
      num1 = num24 + SHA256MessageDigest.Sum0Maj(num2, num3, num4);
      index1 = index9 + 1;
    }
    this.m_h1 += num1;
    this.m_h2 += num2;
    this.m_h3 += num3;
    this.m_h4 += num4;
    this.m_h5 += num5;
    this.m_h6 += num6;
    this.m_h7 += num7;
    this.m_h8 += num8;
    this.m_xOff = 0;
    Array.Clear((Array) this.m_x, 0, 16 /*0x10*/);
  }

  private static uint Sum1Ch(uint x, uint y, uint z)
  {
    return (uint) ((((int) (x >> 6) | (int) x << 26) ^ ((int) (x >> 11) | (int) x << 21) ^ ((int) (x >> 25) | (int) x << 7)) + ((int) x & (int) y ^ ~(int) x & (int) z));
  }

  private static uint Sum0Maj(uint x, uint y, uint z)
  {
    return (uint) ((((int) (x >> 2) | (int) x << 30) ^ ((int) (x >> 13) | (int) x << 19) ^ ((int) (x >> 22) | (int) x << 10)) + ((int) x & (int) y ^ (int) x & (int) z ^ (int) y & (int) z));
  }

  private static uint Theta0(uint x)
  {
    return (uint) (((int) (x >> 7) | (int) x << 25) ^ ((int) (x >> 18) | (int) x << 14)) ^ x >> 3;
  }

  private static uint Theta1(uint x)
  {
    return (uint) (((int) (x >> 17) | (int) x << 15) ^ ((int) (x >> 19) | (int) x << 13)) ^ x >> 10;
  }
}
