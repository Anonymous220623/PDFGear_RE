// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.MessageDigest5
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class MessageDigest5 : MessageDigest
{
  private const int m_digestLength = 16 /*0x10*/;
  private uint m_a;
  private uint m_b;
  private uint m_c;
  private uint m_d;
  private uint[] m_localarray = new uint[16 /*0x10*/];
  private int m_offset;
  private static readonly int r11 = 7;
  private static readonly int r12 = 12;
  private static readonly int r13 = 17;
  private static readonly int r14 = 22;
  private static readonly int r21 = 5;
  private static readonly int r22 = 9;
  private static readonly int r23 = 14;
  private static readonly int r24 = 20;
  private static readonly int r31 = 4;
  private static readonly int r32 = 11;
  private static readonly int r33 = 16 /*0x10*/;
  private static readonly int r34 = 23;
  private static readonly int r41 = 6;
  private static readonly int r42 = 10;
  private static readonly int r43 = 15;
  private static readonly int r44 = 21;

  public MessageDigest5() => this.Reset();

  public MessageDigest5(MessageDigest5 md5)
    : base((MessageDigest) md5)
  {
    this.m_a = md5.m_a;
    this.m_b = md5.m_b;
    this.m_c = md5.m_c;
    this.m_d = md5.m_d;
    Array.Copy((Array) md5.m_localarray, 0, (Array) this.m_localarray, 0, md5.m_localarray.Length);
    this.m_offset = md5.m_offset;
  }

  internal override void ProcessWord(byte[] input, int inOff)
  {
    this.m_localarray[this.m_offset] = Asn1Constants.LeToUInt32(input, inOff);
    if (++this.m_offset != 16 /*0x10*/)
      return;
    this.ProcessBlock();
  }

  internal override void ProcessLength(long Length)
  {
    if (this.m_offset > 14)
    {
      if (this.m_offset == 15)
        this.m_localarray[15] = 0U;
      this.ProcessBlock();
    }
    for (int offset = this.m_offset; offset < 14; ++offset)
      this.m_localarray[offset] = 0U;
    this.m_localarray[14] = (uint) Length;
    this.m_localarray[15] = (uint) (Length >>> 32 /*0x20*/);
  }

  internal override void ProcessBlock()
  {
    uint a = this.m_a;
    uint b = this.m_b;
    uint c = this.m_c;
    uint d = this.m_d;
    uint num1 = this.RotateLeft((uint) ((int) a + (int) this.F1(b, c, d) + (int) this.m_localarray[0] - 680876936), MessageDigest5.r11) + b;
    uint num2 = this.RotateLeft((uint) ((int) d + (int) this.F1(num1, b, c) + (int) this.m_localarray[1] - 389564586), MessageDigest5.r12) + num1;
    uint num3 = this.RotateLeft((uint) ((int) c + (int) this.F1(num2, num1, b) + (int) this.m_localarray[2] + 606105819), MessageDigest5.r13) + num2;
    uint num4 = this.RotateLeft((uint) ((int) b + (int) this.F1(num3, num2, num1) + (int) this.m_localarray[3] - 1044525330), MessageDigest5.r14) + num3;
    uint num5 = this.RotateLeft((uint) ((int) num1 + (int) this.F1(num4, num3, num2) + (int) this.m_localarray[4] - 176418897), MessageDigest5.r11) + num4;
    uint num6 = this.RotateLeft((uint) ((int) num2 + (int) this.F1(num5, num4, num3) + (int) this.m_localarray[5] + 1200080426), MessageDigest5.r12) + num5;
    uint num7 = this.RotateLeft((uint) ((int) num3 + (int) this.F1(num6, num5, num4) + (int) this.m_localarray[6] - 1473231341), MessageDigest5.r13) + num6;
    uint num8 = this.RotateLeft((uint) ((int) num4 + (int) this.F1(num7, num6, num5) + (int) this.m_localarray[7] - 45705983), MessageDigest5.r14) + num7;
    uint num9 = this.RotateLeft((uint) ((int) num5 + (int) this.F1(num8, num7, num6) + (int) this.m_localarray[8] + 1770035416), MessageDigest5.r11) + num8;
    uint num10 = this.RotateLeft((uint) ((int) num6 + (int) this.F1(num9, num8, num7) + (int) this.m_localarray[9] - 1958414417), MessageDigest5.r12) + num9;
    uint num11 = this.RotateLeft((uint) ((int) num7 + (int) this.F1(num10, num9, num8) + (int) this.m_localarray[10] - 42063), MessageDigest5.r13) + num10;
    uint num12 = this.RotateLeft((uint) ((int) num8 + (int) this.F1(num11, num10, num9) + (int) this.m_localarray[11] - 1990404162), MessageDigest5.r14) + num11;
    uint num13 = this.RotateLeft((uint) ((int) num9 + (int) this.F1(num12, num11, num10) + (int) this.m_localarray[12] + 1804603682), MessageDigest5.r11) + num12;
    uint num14 = this.RotateLeft((uint) ((int) num10 + (int) this.F1(num13, num12, num11) + (int) this.m_localarray[13] - 40341101), MessageDigest5.r12) + num13;
    uint num15 = this.RotateLeft((uint) ((int) num11 + (int) this.F1(num14, num13, num12) + (int) this.m_localarray[14] - 1502002290), MessageDigest5.r13) + num14;
    uint num16 = this.RotateLeft((uint) ((int) num12 + (int) this.F1(num15, num14, num13) + (int) this.m_localarray[15] + 1236535329), MessageDigest5.r14) + num15;
    uint num17 = this.RotateLeft((uint) ((int) num13 + (int) this.F2(num16, num15, num14) + (int) this.m_localarray[1] - 165796510), MessageDigest5.r21) + num16;
    uint num18 = this.RotateLeft((uint) ((int) num14 + (int) this.F2(num17, num16, num15) + (int) this.m_localarray[6] - 1069501632), MessageDigest5.r22) + num17;
    uint num19 = this.RotateLeft((uint) ((int) num15 + (int) this.F2(num18, num17, num16) + (int) this.m_localarray[11] + 643717713), MessageDigest5.r23) + num18;
    uint num20 = this.RotateLeft((uint) ((int) num16 + (int) this.F2(num19, num18, num17) + (int) this.m_localarray[0] - 373897302), MessageDigest5.r24) + num19;
    uint num21 = this.RotateLeft((uint) ((int) num17 + (int) this.F2(num20, num19, num18) + (int) this.m_localarray[5] - 701558691), MessageDigest5.r21) + num20;
    uint num22 = this.RotateLeft((uint) ((int) num18 + (int) this.F2(num21, num20, num19) + (int) this.m_localarray[10] + 38016083), MessageDigest5.r22) + num21;
    uint num23 = this.RotateLeft((uint) ((int) num19 + (int) this.F2(num22, num21, num20) + (int) this.m_localarray[15] - 660478335), MessageDigest5.r23) + num22;
    uint num24 = this.RotateLeft((uint) ((int) num20 + (int) this.F2(num23, num22, num21) + (int) this.m_localarray[4] - 405537848), MessageDigest5.r24) + num23;
    uint num25 = this.RotateLeft((uint) ((int) num21 + (int) this.F2(num24, num23, num22) + (int) this.m_localarray[9] + 568446438), MessageDigest5.r21) + num24;
    uint num26 = this.RotateLeft((uint) ((int) num22 + (int) this.F2(num25, num24, num23) + (int) this.m_localarray[14] - 1019803690), MessageDigest5.r22) + num25;
    uint num27 = this.RotateLeft((uint) ((int) num23 + (int) this.F2(num26, num25, num24) + (int) this.m_localarray[3] - 187363961), MessageDigest5.r23) + num26;
    uint num28 = this.RotateLeft((uint) ((int) num24 + (int) this.F2(num27, num26, num25) + (int) this.m_localarray[8] + 1163531501), MessageDigest5.r24) + num27;
    uint num29 = this.RotateLeft((uint) ((int) num25 + (int) this.F2(num28, num27, num26) + (int) this.m_localarray[13] - 1444681467), MessageDigest5.r21) + num28;
    uint num30 = this.RotateLeft((uint) ((int) num26 + (int) this.F2(num29, num28, num27) + (int) this.m_localarray[2] - 51403784), MessageDigest5.r22) + num29;
    uint num31 = this.RotateLeft((uint) ((int) num27 + (int) this.F2(num30, num29, num28) + (int) this.m_localarray[7] + 1735328473), MessageDigest5.r23) + num30;
    uint num32 = this.RotateLeft((uint) ((int) num28 + (int) this.F2(num31, num30, num29) + (int) this.m_localarray[12] - 1926607734), MessageDigest5.r24) + num31;
    uint num33 = this.RotateLeft((uint) ((int) num29 + (int) this.F3(num32, num31, num30) + (int) this.m_localarray[5] - 378558), MessageDigest5.r31) + num32;
    uint num34 = this.RotateLeft((uint) ((int) num30 + (int) this.F3(num33, num32, num31) + (int) this.m_localarray[8] - 2022574463), MessageDigest5.r32) + num33;
    uint num35 = this.RotateLeft((uint) ((int) num31 + (int) this.F3(num34, num33, num32) + (int) this.m_localarray[11] + 1839030562), MessageDigest5.r33) + num34;
    uint num36 = this.RotateLeft((uint) ((int) num32 + (int) this.F3(num35, num34, num33) + (int) this.m_localarray[14] - 35309556), MessageDigest5.r34) + num35;
    uint num37 = this.RotateLeft((uint) ((int) num33 + (int) this.F3(num36, num35, num34) + (int) this.m_localarray[1] - 1530992060), MessageDigest5.r31) + num36;
    uint num38 = this.RotateLeft((uint) ((int) num34 + (int) this.F3(num37, num36, num35) + (int) this.m_localarray[4] + 1272893353), MessageDigest5.r32) + num37;
    uint num39 = this.RotateLeft((uint) ((int) num35 + (int) this.F3(num38, num37, num36) + (int) this.m_localarray[7] - 155497632), MessageDigest5.r33) + num38;
    uint num40 = this.RotateLeft((uint) ((int) num36 + (int) this.F3(num39, num38, num37) + (int) this.m_localarray[10] - 1094730640), MessageDigest5.r34) + num39;
    uint num41 = this.RotateLeft((uint) ((int) num37 + (int) this.F3(num40, num39, num38) + (int) this.m_localarray[13] + 681279174), MessageDigest5.r31) + num40;
    uint num42 = this.RotateLeft((uint) ((int) num38 + (int) this.F3(num41, num40, num39) + (int) this.m_localarray[0] - 358537222), MessageDigest5.r32) + num41;
    uint num43 = this.RotateLeft((uint) ((int) num39 + (int) this.F3(num42, num41, num40) + (int) this.m_localarray[3] - 722521979), MessageDigest5.r33) + num42;
    uint num44 = this.RotateLeft((uint) ((int) num40 + (int) this.F3(num43, num42, num41) + (int) this.m_localarray[6] + 76029189), MessageDigest5.r34) + num43;
    uint num45 = this.RotateLeft((uint) ((int) num41 + (int) this.F3(num44, num43, num42) + (int) this.m_localarray[9] - 640364487), MessageDigest5.r31) + num44;
    uint num46 = this.RotateLeft((uint) ((int) num42 + (int) this.F3(num45, num44, num43) + (int) this.m_localarray[12] - 421815835), MessageDigest5.r32) + num45;
    uint num47 = this.RotateLeft((uint) ((int) num43 + (int) this.F3(num46, num45, num44) + (int) this.m_localarray[15] + 530742520), MessageDigest5.r33) + num46;
    uint num48 = this.RotateLeft((uint) ((int) num44 + (int) this.F3(num47, num46, num45) + (int) this.m_localarray[2] - 995338651), MessageDigest5.r34) + num47;
    uint num49 = this.RotateLeft((uint) ((int) num45 + (int) this.F4(num48, num47, num46) + (int) this.m_localarray[0] - 198630844), MessageDigest5.r41) + num48;
    uint num50 = this.RotateLeft((uint) ((int) num46 + (int) this.F4(num49, num48, num47) + (int) this.m_localarray[7] + 1126891415), MessageDigest5.r42) + num49;
    uint num51 = this.RotateLeft((uint) ((int) num47 + (int) this.F4(num50, num49, num48) + (int) this.m_localarray[14] - 1416354905), MessageDigest5.r43) + num50;
    uint num52 = this.RotateLeft((uint) ((int) num48 + (int) this.F4(num51, num50, num49) + (int) this.m_localarray[5] - 57434055), MessageDigest5.r44) + num51;
    uint num53 = this.RotateLeft((uint) ((int) num49 + (int) this.F4(num52, num51, num50) + (int) this.m_localarray[12] + 1700485571), MessageDigest5.r41) + num52;
    uint num54 = this.RotateLeft((uint) ((int) num50 + (int) this.F4(num53, num52, num51) + (int) this.m_localarray[3] - 1894986606), MessageDigest5.r42) + num53;
    uint num55 = this.RotateLeft((uint) ((int) num51 + (int) this.F4(num54, num53, num52) + (int) this.m_localarray[10] - 1051523), MessageDigest5.r43) + num54;
    uint num56 = this.RotateLeft((uint) ((int) num52 + (int) this.F4(num55, num54, num53) + (int) this.m_localarray[1] - 2054922799), MessageDigest5.r44) + num55;
    uint num57 = this.RotateLeft((uint) ((int) num53 + (int) this.F4(num56, num55, num54) + (int) this.m_localarray[8] + 1873313359), MessageDigest5.r41) + num56;
    uint num58 = this.RotateLeft((uint) ((int) num54 + (int) this.F4(num57, num56, num55) + (int) this.m_localarray[15] - 30611744), MessageDigest5.r42) + num57;
    uint num59 = this.RotateLeft((uint) ((int) num55 + (int) this.F4(num58, num57, num56) + (int) this.m_localarray[6] - 1560198380), MessageDigest5.r43) + num58;
    uint num60 = this.RotateLeft((uint) ((int) num56 + (int) this.F4(num59, num58, num57) + (int) this.m_localarray[13] + 1309151649), MessageDigest5.r44) + num59;
    uint num61 = this.RotateLeft((uint) ((int) num57 + (int) this.F4(num60, num59, num58) + (int) this.m_localarray[4] - 145523070), MessageDigest5.r41) + num60;
    uint num62 = this.RotateLeft((uint) ((int) num58 + (int) this.F4(num61, num60, num59) + (int) this.m_localarray[11] - 1120210379), MessageDigest5.r42) + num61;
    uint u = this.RotateLeft((uint) ((int) num59 + (int) this.F4(num62, num61, num60) + (int) this.m_localarray[2] + 718787259), MessageDigest5.r43) + num62;
    uint num63 = this.RotateLeft((uint) ((int) num60 + (int) this.F4(u, num62, num61) + (int) this.m_localarray[9] - 343485551), MessageDigest5.r44) + u;
    this.m_a += num61;
    this.m_b += num63;
    this.m_c += u;
    this.m_d += num62;
    this.m_offset = 0;
  }

  public override string AlgorithmName => "MD5";

  public override int MessageDigestSize => 16 /*0x10*/;

  public override int DoFinal(byte[] bytes, int offset)
  {
    this.Finish();
    Asn1Constants.UInt32ToLe(this.m_a, bytes, offset);
    Asn1Constants.UInt32ToLe(this.m_b, bytes, offset + 4);
    Asn1Constants.UInt32ToLe(this.m_c, bytes, offset + 8);
    Asn1Constants.UInt32ToLe(this.m_d, bytes, offset + 12);
    this.Reset();
    return 16 /*0x10*/;
  }

  private uint RotateLeft(uint x, int n) => x << n | x >> 32 /*0x20*/ - n;

  private uint F1(uint u, uint v, uint w) => (uint) ((int) u & (int) v | ~(int) u & (int) w);

  private uint F2(uint u, uint v, uint w) => (uint) ((int) u & (int) w | (int) v & ~(int) w);

  private uint F3(uint u, uint v, uint w) => u ^ v ^ w;

  private uint F4(uint u, uint v, uint w) => v ^ (u | ~w);

  public override void Reset()
  {
    base.Reset();
    this.m_a = 1732584193U;
    this.m_b = 4023233417U;
    this.m_c = 2562383102U;
    this.m_d = 271733878U;
    this.m_offset = 0;
    for (int index = 0; index != this.m_localarray.Length; ++index)
      this.m_localarray[index] = 0U;
  }
}
