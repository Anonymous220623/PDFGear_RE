// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RIPEMD160MessageDigest
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RIPEMD160MessageDigest : MessageDigest
{
  private const int m_digestLength = 20;
  private int m_h0;
  private int m_h1;
  private int m_h2;
  private int m_h3;
  private int m_h4;
  private int[] m_x = new int[16 /*0x10*/];
  private int m_xOffset;

  internal RIPEMD160MessageDigest() => this.Reset();

  internal RIPEMD160MessageDigest(RIPEMD160MessageDigest t)
    : base((MessageDigest) t)
  {
    this.m_h0 = t.m_h0;
    this.m_h1 = t.m_h1;
    this.m_h2 = t.m_h2;
    this.m_h3 = t.m_h3;
    this.m_h4 = t.m_h4;
    Array.Copy((Array) t.m_x, 0, (Array) this.m_x, 0, t.m_x.Length);
    this.m_xOffset = t.m_xOffset;
  }

  public override string AlgorithmName => "RIPEMD160";

  public override int MessageDigestSize => 20;

  internal override void ProcessWord(byte[] input, int inOff)
  {
    this.m_x[this.m_xOffset++] = (int) input[inOff] & (int) byte.MaxValue | ((int) input[inOff + 1] & (int) byte.MaxValue) << 8 | ((int) input[inOff + 2] & (int) byte.MaxValue) << 16 /*0x10*/ | ((int) input[inOff + 3] & (int) byte.MaxValue) << 24;
    if (this.m_xOffset != 16 /*0x10*/)
      return;
    this.ProcessBlock();
  }

  internal override void ProcessLength(long bitLength)
  {
    if (this.m_xOffset > 14)
      this.ProcessBlock();
    this.m_x[14] = (int) (bitLength & (long) uint.MaxValue);
    this.m_x[15] = (int) (bitLength >>> 32 /*0x20*/);
  }

  private void UnpackWord(int word, byte[] output, int offset)
  {
    output[offset] = (byte) word;
    output[offset + 1] = (byte) (word >>> 8);
    output[offset + 2] = (byte) (word >>> 16 /*0x10*/);
    output[offset + 3] = (byte) (word >>> 24);
  }

  public override int DoFinal(byte[] bytes, int offset)
  {
    this.Finish();
    this.UnpackWord(this.m_h0, bytes, offset);
    this.UnpackWord(this.m_h1, bytes, offset + 4);
    this.UnpackWord(this.m_h2, bytes, offset + 8);
    this.UnpackWord(this.m_h3, bytes, offset + 12);
    this.UnpackWord(this.m_h4, bytes, offset + 16 /*0x10*/);
    this.Reset();
    return 20;
  }

  public override void Reset()
  {
    base.Reset();
    this.m_h0 = 1732584193;
    this.m_h1 = -271733879;
    this.m_h2 = -1732584194;
    this.m_h3 = 271733878;
    this.m_h4 = -1009589776;
    this.m_xOffset = 0;
    for (int index = 0; index != this.m_x.Length; ++index)
      this.m_x[index] = 0;
  }

  private int GetRightToLeft(int x, int n) => x << n | x >>> 32 /*0x20*/ - n;

  private int GetBitLevelEXOR(int x, int y, int z) => x ^ y ^ z;

  private int GetBitlevelMultiplexer(int x, int y, int z) => x & y | ~x & z;

  private int GetBitlevelNegative(int x, int y, int z) => (x | ~y) ^ z;

  private int GetBitlevelDemultiplexer(int x, int y, int z) => x & z | y & ~z;

  private int GetBitlevelReverseNegative(int x, int y, int z) => x ^ (y | ~z);

  internal override void ProcessBlock()
  {
    int h0;
    int num1 = h0 = this.m_h0;
    int h1;
    int num2 = h1 = this.m_h1;
    int h2;
    int num3 = h2 = this.m_h2;
    int h3;
    int z = h3 = this.m_h3;
    int h4;
    int num4 = h4 = this.m_h4;
    int num5 = this.GetRightToLeft(num1 + this.GetBitLevelEXOR(num2, num3, z) + this.m_x[0], 11) + num4;
    int rightToLeft1 = this.GetRightToLeft(num3, 10);
    int num6 = this.GetRightToLeft(num4 + this.GetBitLevelEXOR(num5, num2, rightToLeft1) + this.m_x[1], 14) + z;
    int rightToLeft2 = this.GetRightToLeft(num2, 10);
    int num7 = this.GetRightToLeft(z + this.GetBitLevelEXOR(num6, num5, rightToLeft2) + this.m_x[2], 15) + rightToLeft1;
    int rightToLeft3 = this.GetRightToLeft(num5, 10);
    int num8 = this.GetRightToLeft(rightToLeft1 + this.GetBitLevelEXOR(num7, num6, rightToLeft3) + this.m_x[3], 12) + rightToLeft2;
    int rightToLeft4 = this.GetRightToLeft(num6, 10);
    int num9 = this.GetRightToLeft(rightToLeft2 + this.GetBitLevelEXOR(num8, num7, rightToLeft4) + this.m_x[4], 5) + rightToLeft3;
    int rightToLeft5 = this.GetRightToLeft(num7, 10);
    int num10 = this.GetRightToLeft(rightToLeft3 + this.GetBitLevelEXOR(num9, num8, rightToLeft5) + this.m_x[5], 8) + rightToLeft4;
    int rightToLeft6 = this.GetRightToLeft(num8, 10);
    int num11 = this.GetRightToLeft(rightToLeft4 + this.GetBitLevelEXOR(num10, num9, rightToLeft6) + this.m_x[6], 7) + rightToLeft5;
    int rightToLeft7 = this.GetRightToLeft(num9, 10);
    int num12 = this.GetRightToLeft(rightToLeft5 + this.GetBitLevelEXOR(num11, num10, rightToLeft7) + this.m_x[7], 9) + rightToLeft6;
    int rightToLeft8 = this.GetRightToLeft(num10, 10);
    int num13 = this.GetRightToLeft(rightToLeft6 + this.GetBitLevelEXOR(num12, num11, rightToLeft8) + this.m_x[8], 11) + rightToLeft7;
    int rightToLeft9 = this.GetRightToLeft(num11, 10);
    int num14 = this.GetRightToLeft(rightToLeft7 + this.GetBitLevelEXOR(num13, num12, rightToLeft9) + this.m_x[9], 13) + rightToLeft8;
    int rightToLeft10 = this.GetRightToLeft(num12, 10);
    int num15 = this.GetRightToLeft(rightToLeft8 + this.GetBitLevelEXOR(num14, num13, rightToLeft10) + this.m_x[10], 14) + rightToLeft9;
    int rightToLeft11 = this.GetRightToLeft(num13, 10);
    int num16 = this.GetRightToLeft(rightToLeft9 + this.GetBitLevelEXOR(num15, num14, rightToLeft11) + this.m_x[11], 15) + rightToLeft10;
    int rightToLeft12 = this.GetRightToLeft(num14, 10);
    int num17 = this.GetRightToLeft(rightToLeft10 + this.GetBitLevelEXOR(num16, num15, rightToLeft12) + this.m_x[12], 6) + rightToLeft11;
    int rightToLeft13 = this.GetRightToLeft(num15, 10);
    int num18 = this.GetRightToLeft(rightToLeft11 + this.GetBitLevelEXOR(num17, num16, rightToLeft13) + this.m_x[13], 7) + rightToLeft12;
    int rightToLeft14 = this.GetRightToLeft(num16, 10);
    int num19 = this.GetRightToLeft(rightToLeft12 + this.GetBitLevelEXOR(num18, num17, rightToLeft14) + this.m_x[14], 9) + rightToLeft13;
    int rightToLeft15 = this.GetRightToLeft(num17, 10);
    int num20 = this.GetRightToLeft(rightToLeft13 + this.GetBitLevelEXOR(num19, num18, rightToLeft15) + this.m_x[15], 8) + rightToLeft14;
    int rightToLeft16 = this.GetRightToLeft(num18, 10);
    int num21 = this.GetRightToLeft(h0 + this.GetBitlevelReverseNegative(h1, h2, h3) + this.m_x[5] + 1352829926, 8) + h4;
    int rightToLeft17 = this.GetRightToLeft(h2, 10);
    int num22 = this.GetRightToLeft(h4 + this.GetBitlevelReverseNegative(num21, h1, rightToLeft17) + this.m_x[14] + 1352829926, 9) + h3;
    int rightToLeft18 = this.GetRightToLeft(h1, 10);
    int num23 = this.GetRightToLeft(h3 + this.GetBitlevelReverseNegative(num22, num21, rightToLeft18) + this.m_x[7] + 1352829926, 9) + rightToLeft17;
    int rightToLeft19 = this.GetRightToLeft(num21, 10);
    int num24 = this.GetRightToLeft(rightToLeft17 + this.GetBitlevelReverseNegative(num23, num22, rightToLeft19) + this.m_x[0] + 1352829926, 11) + rightToLeft18;
    int rightToLeft20 = this.GetRightToLeft(num22, 10);
    int num25 = this.GetRightToLeft(rightToLeft18 + this.GetBitlevelReverseNegative(num24, num23, rightToLeft20) + this.m_x[9] + 1352829926, 13) + rightToLeft19;
    int rightToLeft21 = this.GetRightToLeft(num23, 10);
    int num26 = this.GetRightToLeft(rightToLeft19 + this.GetBitlevelReverseNegative(num25, num24, rightToLeft21) + this.m_x[2] + 1352829926, 15) + rightToLeft20;
    int rightToLeft22 = this.GetRightToLeft(num24, 10);
    int num27 = this.GetRightToLeft(rightToLeft20 + this.GetBitlevelReverseNegative(num26, num25, rightToLeft22) + this.m_x[11] + 1352829926, 15) + rightToLeft21;
    int rightToLeft23 = this.GetRightToLeft(num25, 10);
    int num28 = this.GetRightToLeft(rightToLeft21 + this.GetBitlevelReverseNegative(num27, num26, rightToLeft23) + this.m_x[4] + 1352829926, 5) + rightToLeft22;
    int rightToLeft24 = this.GetRightToLeft(num26, 10);
    int num29 = this.GetRightToLeft(rightToLeft22 + this.GetBitlevelReverseNegative(num28, num27, rightToLeft24) + this.m_x[13] + 1352829926, 7) + rightToLeft23;
    int rightToLeft25 = this.GetRightToLeft(num27, 10);
    int num30 = this.GetRightToLeft(rightToLeft23 + this.GetBitlevelReverseNegative(num29, num28, rightToLeft25) + this.m_x[6] + 1352829926, 7) + rightToLeft24;
    int rightToLeft26 = this.GetRightToLeft(num28, 10);
    int num31 = this.GetRightToLeft(rightToLeft24 + this.GetBitlevelReverseNegative(num30, num29, rightToLeft26) + this.m_x[15] + 1352829926, 8) + rightToLeft25;
    int rightToLeft27 = this.GetRightToLeft(num29, 10);
    int num32 = this.GetRightToLeft(rightToLeft25 + this.GetBitlevelReverseNegative(num31, num30, rightToLeft27) + this.m_x[8] + 1352829926, 11) + rightToLeft26;
    int rightToLeft28 = this.GetRightToLeft(num30, 10);
    int num33 = this.GetRightToLeft(rightToLeft26 + this.GetBitlevelReverseNegative(num32, num31, rightToLeft28) + this.m_x[1] + 1352829926, 14) + rightToLeft27;
    int rightToLeft29 = this.GetRightToLeft(num31, 10);
    int num34 = this.GetRightToLeft(rightToLeft27 + this.GetBitlevelReverseNegative(num33, num32, rightToLeft29) + this.m_x[10] + 1352829926, 14) + rightToLeft28;
    int rightToLeft30 = this.GetRightToLeft(num32, 10);
    int num35 = this.GetRightToLeft(rightToLeft28 + this.GetBitlevelReverseNegative(num34, num33, rightToLeft30) + this.m_x[3] + 1352829926, 12) + rightToLeft29;
    int rightToLeft31 = this.GetRightToLeft(num33, 10);
    int num36 = this.GetRightToLeft(rightToLeft29 + this.GetBitlevelReverseNegative(num35, num34, rightToLeft31) + this.m_x[12] + 1352829926, 6) + rightToLeft30;
    int rightToLeft32 = this.GetRightToLeft(num34, 10);
    int num37 = this.GetRightToLeft(rightToLeft14 + this.GetBitlevelMultiplexer(num20, num19, rightToLeft16) + this.m_x[7] + 1518500249, 7) + rightToLeft15;
    int rightToLeft33 = this.GetRightToLeft(num19, 10);
    int num38 = this.GetRightToLeft(rightToLeft15 + this.GetBitlevelMultiplexer(num37, num20, rightToLeft33) + this.m_x[4] + 1518500249, 6) + rightToLeft16;
    int rightToLeft34 = this.GetRightToLeft(num20, 10);
    int num39 = this.GetRightToLeft(rightToLeft16 + this.GetBitlevelMultiplexer(num38, num37, rightToLeft34) + this.m_x[13] + 1518500249, 8) + rightToLeft33;
    int rightToLeft35 = this.GetRightToLeft(num37, 10);
    int num40 = this.GetRightToLeft(rightToLeft33 + this.GetBitlevelMultiplexer(num39, num38, rightToLeft35) + this.m_x[1] + 1518500249, 13) + rightToLeft34;
    int rightToLeft36 = this.GetRightToLeft(num38, 10);
    int num41 = this.GetRightToLeft(rightToLeft34 + this.GetBitlevelMultiplexer(num40, num39, rightToLeft36) + this.m_x[10] + 1518500249, 11) + rightToLeft35;
    int rightToLeft37 = this.GetRightToLeft(num39, 10);
    int num42 = this.GetRightToLeft(rightToLeft35 + this.GetBitlevelMultiplexer(num41, num40, rightToLeft37) + this.m_x[6] + 1518500249, 9) + rightToLeft36;
    int rightToLeft38 = this.GetRightToLeft(num40, 10);
    int num43 = this.GetRightToLeft(rightToLeft36 + this.GetBitlevelMultiplexer(num42, num41, rightToLeft38) + this.m_x[15] + 1518500249, 7) + rightToLeft37;
    int rightToLeft39 = this.GetRightToLeft(num41, 10);
    int num44 = this.GetRightToLeft(rightToLeft37 + this.GetBitlevelMultiplexer(num43, num42, rightToLeft39) + this.m_x[3] + 1518500249, 15) + rightToLeft38;
    int rightToLeft40 = this.GetRightToLeft(num42, 10);
    int num45 = this.GetRightToLeft(rightToLeft38 + this.GetBitlevelMultiplexer(num44, num43, rightToLeft40) + this.m_x[12] + 1518500249, 7) + rightToLeft39;
    int rightToLeft41 = this.GetRightToLeft(num43, 10);
    int num46 = this.GetRightToLeft(rightToLeft39 + this.GetBitlevelMultiplexer(num45, num44, rightToLeft41) + this.m_x[0] + 1518500249, 12) + rightToLeft40;
    int rightToLeft42 = this.GetRightToLeft(num44, 10);
    int num47 = this.GetRightToLeft(rightToLeft40 + this.GetBitlevelMultiplexer(num46, num45, rightToLeft42) + this.m_x[9] + 1518500249, 15) + rightToLeft41;
    int rightToLeft43 = this.GetRightToLeft(num45, 10);
    int num48 = this.GetRightToLeft(rightToLeft41 + this.GetBitlevelMultiplexer(num47, num46, rightToLeft43) + this.m_x[5] + 1518500249, 9) + rightToLeft42;
    int rightToLeft44 = this.GetRightToLeft(num46, 10);
    int num49 = this.GetRightToLeft(rightToLeft42 + this.GetBitlevelMultiplexer(num48, num47, rightToLeft44) + this.m_x[2] + 1518500249, 11) + rightToLeft43;
    int rightToLeft45 = this.GetRightToLeft(num47, 10);
    int num50 = this.GetRightToLeft(rightToLeft43 + this.GetBitlevelMultiplexer(num49, num48, rightToLeft45) + this.m_x[14] + 1518500249, 7) + rightToLeft44;
    int rightToLeft46 = this.GetRightToLeft(num48, 10);
    int num51 = this.GetRightToLeft(rightToLeft44 + this.GetBitlevelMultiplexer(num50, num49, rightToLeft46) + this.m_x[11] + 1518500249, 13) + rightToLeft45;
    int rightToLeft47 = this.GetRightToLeft(num49, 10);
    int num52 = this.GetRightToLeft(rightToLeft45 + this.GetBitlevelMultiplexer(num51, num50, rightToLeft47) + this.m_x[8] + 1518500249, 12) + rightToLeft46;
    int rightToLeft48 = this.GetRightToLeft(num50, 10);
    int num53 = this.GetRightToLeft(rightToLeft30 + this.GetBitlevelDemultiplexer(num36, num35, rightToLeft32) + this.m_x[6] + 1548603684, 9) + rightToLeft31;
    int rightToLeft49 = this.GetRightToLeft(num35, 10);
    int num54 = this.GetRightToLeft(rightToLeft31 + this.GetBitlevelDemultiplexer(num53, num36, rightToLeft49) + this.m_x[11] + 1548603684, 13) + rightToLeft32;
    int rightToLeft50 = this.GetRightToLeft(num36, 10);
    int num55 = this.GetRightToLeft(rightToLeft32 + this.GetBitlevelDemultiplexer(num54, num53, rightToLeft50) + this.m_x[3] + 1548603684, 15) + rightToLeft49;
    int rightToLeft51 = this.GetRightToLeft(num53, 10);
    int num56 = this.GetRightToLeft(rightToLeft49 + this.GetBitlevelDemultiplexer(num55, num54, rightToLeft51) + this.m_x[7] + 1548603684, 7) + rightToLeft50;
    int rightToLeft52 = this.GetRightToLeft(num54, 10);
    int num57 = this.GetRightToLeft(rightToLeft50 + this.GetBitlevelDemultiplexer(num56, num55, rightToLeft52) + this.m_x[0] + 1548603684, 12) + rightToLeft51;
    int rightToLeft53 = this.GetRightToLeft(num55, 10);
    int num58 = this.GetRightToLeft(rightToLeft51 + this.GetBitlevelDemultiplexer(num57, num56, rightToLeft53) + this.m_x[13] + 1548603684, 8) + rightToLeft52;
    int rightToLeft54 = this.GetRightToLeft(num56, 10);
    int num59 = this.GetRightToLeft(rightToLeft52 + this.GetBitlevelDemultiplexer(num58, num57, rightToLeft54) + this.m_x[5] + 1548603684, 9) + rightToLeft53;
    int rightToLeft55 = this.GetRightToLeft(num57, 10);
    int num60 = this.GetRightToLeft(rightToLeft53 + this.GetBitlevelDemultiplexer(num59, num58, rightToLeft55) + this.m_x[10] + 1548603684, 11) + rightToLeft54;
    int rightToLeft56 = this.GetRightToLeft(num58, 10);
    int num61 = this.GetRightToLeft(rightToLeft54 + this.GetBitlevelDemultiplexer(num60, num59, rightToLeft56) + this.m_x[14] + 1548603684, 7) + rightToLeft55;
    int rightToLeft57 = this.GetRightToLeft(num59, 10);
    int num62 = this.GetRightToLeft(rightToLeft55 + this.GetBitlevelDemultiplexer(num61, num60, rightToLeft57) + this.m_x[15] + 1548603684, 7) + rightToLeft56;
    int rightToLeft58 = this.GetRightToLeft(num60, 10);
    int num63 = this.GetRightToLeft(rightToLeft56 + this.GetBitlevelDemultiplexer(num62, num61, rightToLeft58) + this.m_x[8] + 1548603684, 12) + rightToLeft57;
    int rightToLeft59 = this.GetRightToLeft(num61, 10);
    int num64 = this.GetRightToLeft(rightToLeft57 + this.GetBitlevelDemultiplexer(num63, num62, rightToLeft59) + this.m_x[12] + 1548603684, 7) + rightToLeft58;
    int rightToLeft60 = this.GetRightToLeft(num62, 10);
    int num65 = this.GetRightToLeft(rightToLeft58 + this.GetBitlevelDemultiplexer(num64, num63, rightToLeft60) + this.m_x[4] + 1548603684, 6) + rightToLeft59;
    int rightToLeft61 = this.GetRightToLeft(num63, 10);
    int num66 = this.GetRightToLeft(rightToLeft59 + this.GetBitlevelDemultiplexer(num65, num64, rightToLeft61) + this.m_x[9] + 1548603684, 15) + rightToLeft60;
    int rightToLeft62 = this.GetRightToLeft(num64, 10);
    int num67 = this.GetRightToLeft(rightToLeft60 + this.GetBitlevelDemultiplexer(num66, num65, rightToLeft62) + this.m_x[1] + 1548603684, 13) + rightToLeft61;
    int rightToLeft63 = this.GetRightToLeft(num65, 10);
    int num68 = this.GetRightToLeft(rightToLeft61 + this.GetBitlevelDemultiplexer(num67, num66, rightToLeft63) + this.m_x[2] + 1548603684, 11) + rightToLeft62;
    int rightToLeft64 = this.GetRightToLeft(num66, 10);
    int num69 = this.GetRightToLeft(rightToLeft46 + this.GetBitlevelNegative(num52, num51, rightToLeft48) + this.m_x[3] + 1859775393, 11) + rightToLeft47;
    int rightToLeft65 = this.GetRightToLeft(num51, 10);
    int num70 = this.GetRightToLeft(rightToLeft47 + this.GetBitlevelNegative(num69, num52, rightToLeft65) + this.m_x[10] + 1859775393, 13) + rightToLeft48;
    int rightToLeft66 = this.GetRightToLeft(num52, 10);
    int num71 = this.GetRightToLeft(rightToLeft48 + this.GetBitlevelNegative(num70, num69, rightToLeft66) + this.m_x[14] + 1859775393, 6) + rightToLeft65;
    int rightToLeft67 = this.GetRightToLeft(num69, 10);
    int num72 = this.GetRightToLeft(rightToLeft65 + this.GetBitlevelNegative(num71, num70, rightToLeft67) + this.m_x[4] + 1859775393, 7) + rightToLeft66;
    int rightToLeft68 = this.GetRightToLeft(num70, 10);
    int num73 = this.GetRightToLeft(rightToLeft66 + this.GetBitlevelNegative(num72, num71, rightToLeft68) + this.m_x[9] + 1859775393, 14) + rightToLeft67;
    int rightToLeft69 = this.GetRightToLeft(num71, 10);
    int num74 = this.GetRightToLeft(rightToLeft67 + this.GetBitlevelNegative(num73, num72, rightToLeft69) + this.m_x[15] + 1859775393, 9) + rightToLeft68;
    int rightToLeft70 = this.GetRightToLeft(num72, 10);
    int num75 = this.GetRightToLeft(rightToLeft68 + this.GetBitlevelNegative(num74, num73, rightToLeft70) + this.m_x[8] + 1859775393, 13) + rightToLeft69;
    int rightToLeft71 = this.GetRightToLeft(num73, 10);
    int num76 = this.GetRightToLeft(rightToLeft69 + this.GetBitlevelNegative(num75, num74, rightToLeft71) + this.m_x[1] + 1859775393, 15) + rightToLeft70;
    int rightToLeft72 = this.GetRightToLeft(num74, 10);
    int num77 = this.GetRightToLeft(rightToLeft70 + this.GetBitlevelNegative(num76, num75, rightToLeft72) + this.m_x[2] + 1859775393, 14) + rightToLeft71;
    int rightToLeft73 = this.GetRightToLeft(num75, 10);
    int num78 = this.GetRightToLeft(rightToLeft71 + this.GetBitlevelNegative(num77, num76, rightToLeft73) + this.m_x[7] + 1859775393, 8) + rightToLeft72;
    int rightToLeft74 = this.GetRightToLeft(num76, 10);
    int num79 = this.GetRightToLeft(rightToLeft72 + this.GetBitlevelNegative(num78, num77, rightToLeft74) + this.m_x[0] + 1859775393, 13) + rightToLeft73;
    int rightToLeft75 = this.GetRightToLeft(num77, 10);
    int num80 = this.GetRightToLeft(rightToLeft73 + this.GetBitlevelNegative(num79, num78, rightToLeft75) + this.m_x[6] + 1859775393, 6) + rightToLeft74;
    int rightToLeft76 = this.GetRightToLeft(num78, 10);
    int num81 = this.GetRightToLeft(rightToLeft74 + this.GetBitlevelNegative(num80, num79, rightToLeft76) + this.m_x[13] + 1859775393, 5) + rightToLeft75;
    int rightToLeft77 = this.GetRightToLeft(num79, 10);
    int num82 = this.GetRightToLeft(rightToLeft75 + this.GetBitlevelNegative(num81, num80, rightToLeft77) + this.m_x[11] + 1859775393, 12) + rightToLeft76;
    int rightToLeft78 = this.GetRightToLeft(num80, 10);
    int num83 = this.GetRightToLeft(rightToLeft76 + this.GetBitlevelNegative(num82, num81, rightToLeft78) + this.m_x[5] + 1859775393, 7) + rightToLeft77;
    int rightToLeft79 = this.GetRightToLeft(num81, 10);
    int num84 = this.GetRightToLeft(rightToLeft77 + this.GetBitlevelNegative(num83, num82, rightToLeft79) + this.m_x[12] + 1859775393, 5) + rightToLeft78;
    int rightToLeft80 = this.GetRightToLeft(num82, 10);
    int num85 = this.GetRightToLeft(rightToLeft62 + this.GetBitlevelNegative(num68, num67, rightToLeft64) + this.m_x[15] + 1836072691, 9) + rightToLeft63;
    int rightToLeft81 = this.GetRightToLeft(num67, 10);
    int num86 = this.GetRightToLeft(rightToLeft63 + this.GetBitlevelNegative(num85, num68, rightToLeft81) + this.m_x[5] + 1836072691, 7) + rightToLeft64;
    int rightToLeft82 = this.GetRightToLeft(num68, 10);
    int num87 = this.GetRightToLeft(rightToLeft64 + this.GetBitlevelNegative(num86, num85, rightToLeft82) + this.m_x[1] + 1836072691, 15) + rightToLeft81;
    int rightToLeft83 = this.GetRightToLeft(num85, 10);
    int num88 = this.GetRightToLeft(rightToLeft81 + this.GetBitlevelNegative(num87, num86, rightToLeft83) + this.m_x[3] + 1836072691, 11) + rightToLeft82;
    int rightToLeft84 = this.GetRightToLeft(num86, 10);
    int num89 = this.GetRightToLeft(rightToLeft82 + this.GetBitlevelNegative(num88, num87, rightToLeft84) + this.m_x[7] + 1836072691, 8) + rightToLeft83;
    int rightToLeft85 = this.GetRightToLeft(num87, 10);
    int num90 = this.GetRightToLeft(rightToLeft83 + this.GetBitlevelNegative(num89, num88, rightToLeft85) + this.m_x[14] + 1836072691, 6) + rightToLeft84;
    int rightToLeft86 = this.GetRightToLeft(num88, 10);
    int num91 = this.GetRightToLeft(rightToLeft84 + this.GetBitlevelNegative(num90, num89, rightToLeft86) + this.m_x[6] + 1836072691, 6) + rightToLeft85;
    int rightToLeft87 = this.GetRightToLeft(num89, 10);
    int num92 = this.GetRightToLeft(rightToLeft85 + this.GetBitlevelNegative(num91, num90, rightToLeft87) + this.m_x[9] + 1836072691, 14) + rightToLeft86;
    int rightToLeft88 = this.GetRightToLeft(num90, 10);
    int num93 = this.GetRightToLeft(rightToLeft86 + this.GetBitlevelNegative(num92, num91, rightToLeft88) + this.m_x[11] + 1836072691, 12) + rightToLeft87;
    int rightToLeft89 = this.GetRightToLeft(num91, 10);
    int num94 = this.GetRightToLeft(rightToLeft87 + this.GetBitlevelNegative(num93, num92, rightToLeft89) + this.m_x[8] + 1836072691, 13) + rightToLeft88;
    int rightToLeft90 = this.GetRightToLeft(num92, 10);
    int num95 = this.GetRightToLeft(rightToLeft88 + this.GetBitlevelNegative(num94, num93, rightToLeft90) + this.m_x[12] + 1836072691, 5) + rightToLeft89;
    int rightToLeft91 = this.GetRightToLeft(num93, 10);
    int num96 = this.GetRightToLeft(rightToLeft89 + this.GetBitlevelNegative(num95, num94, rightToLeft91) + this.m_x[2] + 1836072691, 14) + rightToLeft90;
    int rightToLeft92 = this.GetRightToLeft(num94, 10);
    int num97 = this.GetRightToLeft(rightToLeft90 + this.GetBitlevelNegative(num96, num95, rightToLeft92) + this.m_x[10] + 1836072691, 13) + rightToLeft91;
    int rightToLeft93 = this.GetRightToLeft(num95, 10);
    int num98 = this.GetRightToLeft(rightToLeft91 + this.GetBitlevelNegative(num97, num96, rightToLeft93) + this.m_x[0] + 1836072691, 13) + rightToLeft92;
    int rightToLeft94 = this.GetRightToLeft(num96, 10);
    int num99 = this.GetRightToLeft(rightToLeft92 + this.GetBitlevelNegative(num98, num97, rightToLeft94) + this.m_x[4] + 1836072691, 7) + rightToLeft93;
    int rightToLeft95 = this.GetRightToLeft(num97, 10);
    int num100 = this.GetRightToLeft(rightToLeft93 + this.GetBitlevelNegative(num99, num98, rightToLeft95) + this.m_x[13] + 1836072691, 5) + rightToLeft94;
    int rightToLeft96 = this.GetRightToLeft(num98, 10);
    int num101 = this.GetRightToLeft(rightToLeft78 + this.GetBitlevelDemultiplexer(num84, num83, rightToLeft80) + this.m_x[1] - 1894007588, 11) + rightToLeft79;
    int rightToLeft97 = this.GetRightToLeft(num83, 10);
    int num102 = this.GetRightToLeft(rightToLeft79 + this.GetBitlevelDemultiplexer(num101, num84, rightToLeft97) + this.m_x[9] - 1894007588, 12) + rightToLeft80;
    int rightToLeft98 = this.GetRightToLeft(num84, 10);
    int num103 = this.GetRightToLeft(rightToLeft80 + this.GetBitlevelDemultiplexer(num102, num101, rightToLeft98) + this.m_x[11] - 1894007588, 14) + rightToLeft97;
    int rightToLeft99 = this.GetRightToLeft(num101, 10);
    int num104 = this.GetRightToLeft(rightToLeft97 + this.GetBitlevelDemultiplexer(num103, num102, rightToLeft99) + this.m_x[10] - 1894007588, 15) + rightToLeft98;
    int rightToLeft100 = this.GetRightToLeft(num102, 10);
    int num105 = this.GetRightToLeft(rightToLeft98 + this.GetBitlevelDemultiplexer(num104, num103, rightToLeft100) + this.m_x[0] - 1894007588, 14) + rightToLeft99;
    int rightToLeft101 = this.GetRightToLeft(num103, 10);
    int num106 = this.GetRightToLeft(rightToLeft99 + this.GetBitlevelDemultiplexer(num105, num104, rightToLeft101) + this.m_x[8] - 1894007588, 15) + rightToLeft100;
    int rightToLeft102 = this.GetRightToLeft(num104, 10);
    int num107 = this.GetRightToLeft(rightToLeft100 + this.GetBitlevelDemultiplexer(num106, num105, rightToLeft102) + this.m_x[12] - 1894007588, 9) + rightToLeft101;
    int rightToLeft103 = this.GetRightToLeft(num105, 10);
    int num108 = this.GetRightToLeft(rightToLeft101 + this.GetBitlevelDemultiplexer(num107, num106, rightToLeft103) + this.m_x[4] - 1894007588, 8) + rightToLeft102;
    int rightToLeft104 = this.GetRightToLeft(num106, 10);
    int num109 = this.GetRightToLeft(rightToLeft102 + this.GetBitlevelDemultiplexer(num108, num107, rightToLeft104) + this.m_x[13] - 1894007588, 9) + rightToLeft103;
    int rightToLeft105 = this.GetRightToLeft(num107, 10);
    int num110 = this.GetRightToLeft(rightToLeft103 + this.GetBitlevelDemultiplexer(num109, num108, rightToLeft105) + this.m_x[3] - 1894007588, 14) + rightToLeft104;
    int rightToLeft106 = this.GetRightToLeft(num108, 10);
    int num111 = this.GetRightToLeft(rightToLeft104 + this.GetBitlevelDemultiplexer(num110, num109, rightToLeft106) + this.m_x[7] - 1894007588, 5) + rightToLeft105;
    int rightToLeft107 = this.GetRightToLeft(num109, 10);
    int num112 = this.GetRightToLeft(rightToLeft105 + this.GetBitlevelDemultiplexer(num111, num110, rightToLeft107) + this.m_x[15] - 1894007588, 6) + rightToLeft106;
    int rightToLeft108 = this.GetRightToLeft(num110, 10);
    int num113 = this.GetRightToLeft(rightToLeft106 + this.GetBitlevelDemultiplexer(num112, num111, rightToLeft108) + this.m_x[14] - 1894007588, 8) + rightToLeft107;
    int rightToLeft109 = this.GetRightToLeft(num111, 10);
    int num114 = this.GetRightToLeft(rightToLeft107 + this.GetBitlevelDemultiplexer(num113, num112, rightToLeft109) + this.m_x[5] - 1894007588, 6) + rightToLeft108;
    int rightToLeft110 = this.GetRightToLeft(num112, 10);
    int num115 = this.GetRightToLeft(rightToLeft108 + this.GetBitlevelDemultiplexer(num114, num113, rightToLeft110) + this.m_x[6] - 1894007588, 5) + rightToLeft109;
    int rightToLeft111 = this.GetRightToLeft(num113, 10);
    int num116 = this.GetRightToLeft(rightToLeft109 + this.GetBitlevelDemultiplexer(num115, num114, rightToLeft111) + this.m_x[2] - 1894007588, 12) + rightToLeft110;
    int rightToLeft112 = this.GetRightToLeft(num114, 10);
    int num117 = this.GetRightToLeft(rightToLeft94 + this.GetBitlevelMultiplexer(num100, num99, rightToLeft96) + this.m_x[8] + 2053994217, 15) + rightToLeft95;
    int rightToLeft113 = this.GetRightToLeft(num99, 10);
    int num118 = this.GetRightToLeft(rightToLeft95 + this.GetBitlevelMultiplexer(num117, num100, rightToLeft113) + this.m_x[6] + 2053994217, 5) + rightToLeft96;
    int rightToLeft114 = this.GetRightToLeft(num100, 10);
    int num119 = this.GetRightToLeft(rightToLeft96 + this.GetBitlevelMultiplexer(num118, num117, rightToLeft114) + this.m_x[4] + 2053994217, 8) + rightToLeft113;
    int rightToLeft115 = this.GetRightToLeft(num117, 10);
    int num120 = this.GetRightToLeft(rightToLeft113 + this.GetBitlevelMultiplexer(num119, num118, rightToLeft115) + this.m_x[1] + 2053994217, 11) + rightToLeft114;
    int rightToLeft116 = this.GetRightToLeft(num118, 10);
    int num121 = this.GetRightToLeft(rightToLeft114 + this.GetBitlevelMultiplexer(num120, num119, rightToLeft116) + this.m_x[3] + 2053994217, 14) + rightToLeft115;
    int rightToLeft117 = this.GetRightToLeft(num119, 10);
    int num122 = this.GetRightToLeft(rightToLeft115 + this.GetBitlevelMultiplexer(num121, num120, rightToLeft117) + this.m_x[11] + 2053994217, 14) + rightToLeft116;
    int rightToLeft118 = this.GetRightToLeft(num120, 10);
    int num123 = this.GetRightToLeft(rightToLeft116 + this.GetBitlevelMultiplexer(num122, num121, rightToLeft118) + this.m_x[15] + 2053994217, 6) + rightToLeft117;
    int rightToLeft119 = this.GetRightToLeft(num121, 10);
    int num124 = this.GetRightToLeft(rightToLeft117 + this.GetBitlevelMultiplexer(num123, num122, rightToLeft119) + this.m_x[0] + 2053994217, 14) + rightToLeft118;
    int rightToLeft120 = this.GetRightToLeft(num122, 10);
    int num125 = this.GetRightToLeft(rightToLeft118 + this.GetBitlevelMultiplexer(num124, num123, rightToLeft120) + this.m_x[5] + 2053994217, 6) + rightToLeft119;
    int rightToLeft121 = this.GetRightToLeft(num123, 10);
    int num126 = this.GetRightToLeft(rightToLeft119 + this.GetBitlevelMultiplexer(num125, num124, rightToLeft121) + this.m_x[12] + 2053994217, 9) + rightToLeft120;
    int rightToLeft122 = this.GetRightToLeft(num124, 10);
    int num127 = this.GetRightToLeft(rightToLeft120 + this.GetBitlevelMultiplexer(num126, num125, rightToLeft122) + this.m_x[2] + 2053994217, 12) + rightToLeft121;
    int rightToLeft123 = this.GetRightToLeft(num125, 10);
    int num128 = this.GetRightToLeft(rightToLeft121 + this.GetBitlevelMultiplexer(num127, num126, rightToLeft123) + this.m_x[13] + 2053994217, 9) + rightToLeft122;
    int rightToLeft124 = this.GetRightToLeft(num126, 10);
    int num129 = this.GetRightToLeft(rightToLeft122 + this.GetBitlevelMultiplexer(num128, num127, rightToLeft124) + this.m_x[9] + 2053994217, 12) + rightToLeft123;
    int rightToLeft125 = this.GetRightToLeft(num127, 10);
    int num130 = this.GetRightToLeft(rightToLeft123 + this.GetBitlevelMultiplexer(num129, num128, rightToLeft125) + this.m_x[7] + 2053994217, 5) + rightToLeft124;
    int rightToLeft126 = this.GetRightToLeft(num128, 10);
    int num131 = this.GetRightToLeft(rightToLeft124 + this.GetBitlevelMultiplexer(num130, num129, rightToLeft126) + this.m_x[10] + 2053994217, 15) + rightToLeft125;
    int rightToLeft127 = this.GetRightToLeft(num129, 10);
    int num132 = this.GetRightToLeft(rightToLeft125 + this.GetBitlevelMultiplexer(num131, num130, rightToLeft127) + this.m_x[14] + 2053994217, 8) + rightToLeft126;
    int rightToLeft128 = this.GetRightToLeft(num130, 10);
    int num133 = this.GetRightToLeft(rightToLeft110 + this.GetBitlevelReverseNegative(num116, num115, rightToLeft112) + this.m_x[4] - 1454113458, 9) + rightToLeft111;
    int rightToLeft129 = this.GetRightToLeft(num115, 10);
    int num134 = this.GetRightToLeft(rightToLeft111 + this.GetBitlevelReverseNegative(num133, num116, rightToLeft129) + this.m_x[0] - 1454113458, 15) + rightToLeft112;
    int rightToLeft130 = this.GetRightToLeft(num116, 10);
    int num135 = this.GetRightToLeft(rightToLeft112 + this.GetBitlevelReverseNegative(num134, num133, rightToLeft130) + this.m_x[5] - 1454113458, 5) + rightToLeft129;
    int rightToLeft131 = this.GetRightToLeft(num133, 10);
    int num136 = this.GetRightToLeft(rightToLeft129 + this.GetBitlevelReverseNegative(num135, num134, rightToLeft131) + this.m_x[9] - 1454113458, 11) + rightToLeft130;
    int rightToLeft132 = this.GetRightToLeft(num134, 10);
    int num137 = this.GetRightToLeft(rightToLeft130 + this.GetBitlevelReverseNegative(num136, num135, rightToLeft132) + this.m_x[7] - 1454113458, 6) + rightToLeft131;
    int rightToLeft133 = this.GetRightToLeft(num135, 10);
    int num138 = this.GetRightToLeft(rightToLeft131 + this.GetBitlevelReverseNegative(num137, num136, rightToLeft133) + this.m_x[12] - 1454113458, 8) + rightToLeft132;
    int rightToLeft134 = this.GetRightToLeft(num136, 10);
    int num139 = this.GetRightToLeft(rightToLeft132 + this.GetBitlevelReverseNegative(num138, num137, rightToLeft134) + this.m_x[2] - 1454113458, 13) + rightToLeft133;
    int rightToLeft135 = this.GetRightToLeft(num137, 10);
    int num140 = this.GetRightToLeft(rightToLeft133 + this.GetBitlevelReverseNegative(num139, num138, rightToLeft135) + this.m_x[10] - 1454113458, 12) + rightToLeft134;
    int rightToLeft136 = this.GetRightToLeft(num138, 10);
    int num141 = this.GetRightToLeft(rightToLeft134 + this.GetBitlevelReverseNegative(num140, num139, rightToLeft136) + this.m_x[14] - 1454113458, 5) + rightToLeft135;
    int rightToLeft137 = this.GetRightToLeft(num139, 10);
    int num142 = this.GetRightToLeft(rightToLeft135 + this.GetBitlevelReverseNegative(num141, num140, rightToLeft137) + this.m_x[1] - 1454113458, 12) + rightToLeft136;
    int rightToLeft138 = this.GetRightToLeft(num140, 10);
    int num143 = this.GetRightToLeft(rightToLeft136 + this.GetBitlevelReverseNegative(num142, num141, rightToLeft138) + this.m_x[3] - 1454113458, 13) + rightToLeft137;
    int rightToLeft139 = this.GetRightToLeft(num141, 10);
    int num144 = this.GetRightToLeft(rightToLeft137 + this.GetBitlevelReverseNegative(num143, num142, rightToLeft139) + this.m_x[8] - 1454113458, 14) + rightToLeft138;
    int rightToLeft140 = this.GetRightToLeft(num142, 10);
    int num145 = this.GetRightToLeft(rightToLeft138 + this.GetBitlevelReverseNegative(num144, num143, rightToLeft140) + this.m_x[11] - 1454113458, 11) + rightToLeft139;
    int rightToLeft141 = this.GetRightToLeft(num143, 10);
    int num146 = this.GetRightToLeft(rightToLeft139 + this.GetBitlevelReverseNegative(num145, num144, rightToLeft141) + this.m_x[6] - 1454113458, 8) + rightToLeft140;
    int rightToLeft142 = this.GetRightToLeft(num144, 10);
    int x1 = this.GetRightToLeft(rightToLeft140 + this.GetBitlevelReverseNegative(num146, num145, rightToLeft142) + this.m_x[15] - 1454113458, 5) + rightToLeft141;
    int rightToLeft143 = this.GetRightToLeft(num145, 10);
    int num147 = this.GetRightToLeft(rightToLeft141 + this.GetBitlevelReverseNegative(x1, num146, rightToLeft143) + this.m_x[13] - 1454113458, 6) + rightToLeft142;
    int rightToLeft144 = this.GetRightToLeft(num146, 10);
    int num148 = this.GetRightToLeft(rightToLeft126 + this.GetBitLevelEXOR(num132, num131, rightToLeft128) + this.m_x[12], 8) + rightToLeft127;
    int rightToLeft145 = this.GetRightToLeft(num131, 10);
    int num149 = this.GetRightToLeft(rightToLeft127 + this.GetBitLevelEXOR(num148, num132, rightToLeft145) + this.m_x[15], 5) + rightToLeft128;
    int rightToLeft146 = this.GetRightToLeft(num132, 10);
    int num150 = this.GetRightToLeft(rightToLeft128 + this.GetBitLevelEXOR(num149, num148, rightToLeft146) + this.m_x[10], 12) + rightToLeft145;
    int rightToLeft147 = this.GetRightToLeft(num148, 10);
    int num151 = this.GetRightToLeft(rightToLeft145 + this.GetBitLevelEXOR(num150, num149, rightToLeft147) + this.m_x[4], 9) + rightToLeft146;
    int rightToLeft148 = this.GetRightToLeft(num149, 10);
    int num152 = this.GetRightToLeft(rightToLeft146 + this.GetBitLevelEXOR(num151, num150, rightToLeft148) + this.m_x[1], 12) + rightToLeft147;
    int rightToLeft149 = this.GetRightToLeft(num150, 10);
    int num153 = this.GetRightToLeft(rightToLeft147 + this.GetBitLevelEXOR(num152, num151, rightToLeft149) + this.m_x[5], 5) + rightToLeft148;
    int rightToLeft150 = this.GetRightToLeft(num151, 10);
    int num154 = this.GetRightToLeft(rightToLeft148 + this.GetBitLevelEXOR(num153, num152, rightToLeft150) + this.m_x[8], 14) + rightToLeft149;
    int rightToLeft151 = this.GetRightToLeft(num152, 10);
    int num155 = this.GetRightToLeft(rightToLeft149 + this.GetBitLevelEXOR(num154, num153, rightToLeft151) + this.m_x[7], 6) + rightToLeft150;
    int rightToLeft152 = this.GetRightToLeft(num153, 10);
    int num156 = this.GetRightToLeft(rightToLeft150 + this.GetBitLevelEXOR(num155, num154, rightToLeft152) + this.m_x[6], 8) + rightToLeft151;
    int rightToLeft153 = this.GetRightToLeft(num154, 10);
    int num157 = this.GetRightToLeft(rightToLeft151 + this.GetBitLevelEXOR(num156, num155, rightToLeft153) + this.m_x[2], 13) + rightToLeft152;
    int rightToLeft154 = this.GetRightToLeft(num155, 10);
    int num158 = this.GetRightToLeft(rightToLeft152 + this.GetBitLevelEXOR(num157, num156, rightToLeft154) + this.m_x[13], 6) + rightToLeft153;
    int rightToLeft155 = this.GetRightToLeft(num156, 10);
    int num159 = this.GetRightToLeft(rightToLeft153 + this.GetBitLevelEXOR(num158, num157, rightToLeft155) + this.m_x[14], 5) + rightToLeft154;
    int rightToLeft156 = this.GetRightToLeft(num157, 10);
    int num160 = this.GetRightToLeft(rightToLeft154 + this.GetBitLevelEXOR(num159, num158, rightToLeft156) + this.m_x[0], 15) + rightToLeft155;
    int rightToLeft157 = this.GetRightToLeft(num158, 10);
    int num161 = this.GetRightToLeft(rightToLeft155 + this.GetBitLevelEXOR(num160, num159, rightToLeft157) + this.m_x[3], 13) + rightToLeft156;
    int rightToLeft158 = this.GetRightToLeft(num159, 10);
    int x2 = this.GetRightToLeft(rightToLeft156 + this.GetBitLevelEXOR(num161, num160, rightToLeft158) + this.m_x[9], 11) + rightToLeft157;
    int rightToLeft159 = this.GetRightToLeft(num160, 10);
    int num162 = this.GetRightToLeft(rightToLeft157 + this.GetBitLevelEXOR(x2, num161, rightToLeft159) + this.m_x[11], 11) + rightToLeft158;
    int num163 = this.GetRightToLeft(num161, 10) + (x1 + this.m_h1);
    this.m_h1 = this.m_h2 + rightToLeft144 + rightToLeft159;
    this.m_h2 = this.m_h3 + rightToLeft143 + rightToLeft158;
    this.m_h3 = this.m_h4 + rightToLeft142 + num162;
    this.m_h4 = this.m_h0 + num147 + x2;
    this.m_h0 = num163;
    this.m_xOffset = 0;
    for (int index = 0; index != this.m_x.Length; ++index)
      this.m_x[index] = 0;
  }
}
