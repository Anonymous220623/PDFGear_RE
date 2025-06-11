// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.BigDigest
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class BigDigest : IMessageDigest
{
  private int m_length = 128 /*0x80*/;
  private byte[] m_buf;
  private int m_bufOffset;
  private long m_byte1;
  private long m_byte2;
  private ulong m_h1;
  private ulong m_h2;
  private ulong m_h3;
  private ulong m_h4;
  private ulong m_h5;
  private ulong m_h6;
  private ulong m_h7;
  private ulong m_h8;
  private ulong[] m_word = new ulong[80 /*0x50*/];
  private int m_wordOffset;
  internal static readonly ulong[] K = new ulong[80 /*0x50*/]
  {
    4794697086780616226UL,
    8158064640168781261UL,
    13096744586834688815UL,
    16840607885511220156UL,
    4131703408338449720UL,
    6480981068601479193UL,
    10538285296894168987UL,
    12329834152419229976UL,
    15566598209576043074UL,
    1334009975649890238UL,
    2608012711638119052UL,
    6128411473006802146UL,
    8268148722764581231UL,
    9286055187155687089UL,
    11230858885718282805UL,
    13951009754708518548UL,
    16472876342353939154UL,
    17275323862435702243UL,
    1135362057144423861UL,
    2597628984639134821UL,
    3308224258029322869UL,
    5365058923640841347UL,
    6679025012923562964UL,
    8573033837759648693UL,
    10970295158949994411UL,
    12119686244451234320UL,
    12683024718118986047UL,
    13788192230050041572UL,
    14330467153632333762UL,
    15395433587784984357UL,
    489312712824947311UL,
    1452737877330783856UL,
    2861767655752347644UL,
    3322285676063803686UL,
    5560940570517711597UL,
    5996557281743188959UL,
    7280758554555802590UL,
    8532644243296465576UL,
    9350256976987008742UL,
    10552545826968843579UL,
    11727347734174303076UL,
    12113106623233404929UL,
    14000437183269869457UL,
    14369950271660146224UL,
    15101387698204529176UL,
    15463397548674623760UL,
    17586052441742319658UL,
    1182934255886127544UL,
    1847814050463011016UL,
    2177327727835720531UL,
    2830643537854262169UL,
    3796741975233480872UL,
    4115178125766777443UL,
    5681478168544905931UL,
    6601373596472566643UL,
    7507060721942968483UL,
    8399075790359081724UL,
    8693463985226723168UL,
    9568029438360202098UL,
    10144078919501101548UL,
    10430055236837252648UL,
    11840083180663258601UL,
    13761210420658862357UL,
    14299343276471374635UL,
    14566680578165727644UL,
    15097957966210449927UL,
    16922976911328602910UL,
    17689382322260857208UL,
    500013540394364858UL,
    748580250866718886UL,
    1242879168328830382UL,
    1977374033974150939UL,
    2944078676154940804UL,
    3659926193048069267UL,
    4368137639120453308UL,
    4836135668995329356UL,
    5532061633213252278UL,
    6448918945643986474UL,
    6902733635092675308UL,
    7801388544844847127UL
  };

  internal ulong Header1
  {
    get => this.m_h1;
    set => this.m_h1 = value;
  }

  internal ulong Header2
  {
    get => this.m_h2;
    set => this.m_h2 = value;
  }

  internal ulong Header3
  {
    get => this.m_h3;
    set => this.m_h3 = value;
  }

  internal ulong Header4
  {
    get => this.m_h4;
    set => this.m_h4 = value;
  }

  internal ulong Header5
  {
    get => this.m_h5;
    set => this.m_h5 = value;
  }

  internal ulong Header6
  {
    get => this.m_h6;
    set => this.m_h6 = value;
  }

  internal ulong Header7
  {
    get => this.m_h7;
    set => this.m_h7 = value;
  }

  internal ulong Header8
  {
    get => this.m_h8;
    set => this.m_h8 = value;
  }

  internal BigDigest()
  {
    this.m_buf = new byte[8];
    this.Reset();
  }

  internal BigDigest(BigDigest t)
  {
    this.m_buf = new byte[t.m_buf.Length];
    Array.Copy((Array) t.m_buf, 0, (Array) this.m_buf, 0, t.m_buf.Length);
    this.m_bufOffset = t.m_bufOffset;
    this.m_byte1 = t.m_byte1;
    this.m_byte2 = t.m_byte2;
    this.m_h1 = t.m_h1;
    this.m_h2 = t.m_h2;
    this.m_h3 = t.m_h3;
    this.m_h4 = t.m_h4;
    this.m_h5 = t.m_h5;
    this.m_h6 = t.m_h6;
    this.m_h7 = t.m_h7;
    this.m_h8 = t.m_h8;
    Array.Copy((Array) t.m_word, 0, (Array) this.m_word, 0, t.m_word.Length);
    this.m_wordOffset = t.m_wordOffset;
  }

  public void Update(byte input)
  {
    this.m_buf[this.m_bufOffset++] = input;
    if (this.m_bufOffset == this.m_buf.Length)
    {
      this.ProcessWord(this.m_buf, 0);
      this.m_bufOffset = 0;
    }
    ++this.m_byte1;
  }

  public void Update(byte[] bytes, int offset, int length)
  {
    for (; this.m_bufOffset != 0 && length > 0; --length)
    {
      this.Update(bytes[offset]);
      ++offset;
    }
    while (length > this.m_buf.Length)
    {
      this.ProcessWord(bytes, offset);
      offset += this.m_buf.Length;
      length -= this.m_buf.Length;
      this.m_byte1 += (long) this.m_buf.Length;
    }
    for (; length > 0; --length)
    {
      this.Update(bytes[offset]);
      ++offset;
    }
  }

  public void BlockUpdate(byte[] bytes, int offset, int length)
  {
    for (; this.m_bufOffset != 0 && length > 0; --length)
    {
      this.Update(bytes[offset]);
      ++offset;
    }
    while (length > this.m_buf.Length)
    {
      this.ProcessWord(bytes, offset);
      offset += this.m_buf.Length;
      length -= this.m_buf.Length;
      this.m_byte1 += (long) this.m_buf.Length;
    }
    for (; length > 0; --length)
    {
      this.Update(bytes[offset]);
      ++offset;
    }
  }

  public void Finish()
  {
    this.ChangeByteCounts();
    long lowW = this.m_byte1 << 3;
    long byte2 = this.m_byte2;
    this.Update((byte) 128 /*0x80*/);
    while (this.m_bufOffset != 0)
      this.Update((byte) 0);
    this.ProcessLength(lowW, byte2);
    this.ProcessBlock();
  }

  public virtual void Reset()
  {
    this.m_byte1 = 0L;
    this.m_byte2 = 0L;
    this.m_bufOffset = 0;
    for (int index = 0; index < this.m_buf.Length; ++index)
      this.m_buf[index] = (byte) 0;
    this.m_wordOffset = 0;
    Array.Clear((Array) this.m_word, 0, this.m_word.Length);
  }

  internal void ProcessWord(byte[] input, int inOff)
  {
    this.m_word[this.m_wordOffset] = Asn1Constants.BeToUInt64(input, inOff);
    if (++this.m_wordOffset != 16 /*0x10*/)
      return;
    this.ProcessBlock();
  }

  private void ChangeByteCounts()
  {
    if (this.m_byte1 <= 2305843009213693951L /*0x1FFFFFFFFFFFFFFF*/)
      return;
    this.m_byte2 += this.m_byte1 >>> 61;
    this.m_byte1 &= 2305843009213693951L /*0x1FFFFFFFFFFFFFFF*/;
  }

  internal void ProcessLength(long lowW, long hiW)
  {
    if (this.m_wordOffset > 14)
      this.ProcessBlock();
    this.m_word[14] = (ulong) hiW;
    this.m_word[15] = (ulong) lowW;
  }

  internal void ProcessBlock()
  {
    this.ChangeByteCounts();
    for (int index = 16 /*0x10*/; index <= 79; ++index)
      this.m_word[index] = BigDigest.Op6(this.m_word[index - 2]) + this.m_word[index - 7] + BigDigest.Op5(this.m_word[index - 15]) + this.m_word[index - 16 /*0x10*/];
    ulong num1 = this.m_h1;
    ulong num2 = this.m_h2;
    ulong num3 = this.m_h3;
    ulong num4 = this.m_h4;
    ulong num5 = this.m_h5;
    ulong num6 = this.m_h6;
    ulong num7 = this.m_h7;
    ulong num8 = this.m_h8;
    int index1 = 0;
    for (int index2 = 0; index2 < 10; ++index2)
    {
      long num9 = (long) num8;
      long num10 = (long) BigDigest.Op4(num5) + (long) BigDigest.Op1(num5, num6, num7) + (long) BigDigest.K[index1];
      ulong[] word1 = this.m_word;
      int index3 = index1;
      int index4 = index3 + 1;
      long num11 = (long) word1[index3];
      long num12 = num10 + num11;
      ulong num13 = (ulong) (num9 + num12);
      ulong num14 = num4 + num13;
      ulong num15 = num13 + (BigDigest.Op3(num1) + BigDigest.Op2(num1, num2, num3));
      long num16 = (long) num7;
      long num17 = (long) BigDigest.Op4(num14) + (long) BigDigest.Op1(num14, num5, num6) + (long) BigDigest.K[index4];
      ulong[] word2 = this.m_word;
      int index5 = index4;
      int index6 = index5 + 1;
      long num18 = (long) word2[index5];
      long num19 = num17 + num18;
      ulong num20 = (ulong) (num16 + num19);
      ulong num21 = num3 + num20;
      ulong num22 = num20 + (BigDigest.Op3(num15) + BigDigest.Op2(num15, num1, num2));
      long num23 = (long) num6;
      long num24 = (long) BigDigest.Op4(num21) + (long) BigDigest.Op1(num21, num14, num5) + (long) BigDigest.K[index6];
      ulong[] word3 = this.m_word;
      int index7 = index6;
      int index8 = index7 + 1;
      long num25 = (long) word3[index7];
      long num26 = num24 + num25;
      ulong num27 = (ulong) (num23 + num26);
      ulong num28 = num2 + num27;
      ulong num29 = num27 + (BigDigest.Op3(num22) + BigDigest.Op2(num22, num15, num1));
      long num30 = (long) num5;
      long num31 = (long) BigDigest.Op4(num28) + (long) BigDigest.Op1(num28, num21, num14) + (long) BigDigest.K[index8];
      ulong[] word4 = this.m_word;
      int index9 = index8;
      int index10 = index9 + 1;
      long num32 = (long) word4[index9];
      long num33 = num31 + num32;
      ulong num34 = (ulong) (num30 + num33);
      ulong num35 = num1 + num34;
      ulong num36 = num34 + (BigDigest.Op3(num29) + BigDigest.Op2(num29, num22, num15));
      long num37 = (long) num14;
      long num38 = (long) BigDigest.Op4(num35) + (long) BigDigest.Op1(num35, num28, num21) + (long) BigDigest.K[index10];
      ulong[] word5 = this.m_word;
      int index11 = index10;
      int index12 = index11 + 1;
      long num39 = (long) word5[index11];
      long num40 = num38 + num39;
      ulong num41 = (ulong) (num37 + num40);
      num8 = num15 + num41;
      num4 = num41 + (BigDigest.Op3(num36) + BigDigest.Op2(num36, num29, num22));
      long num42 = (long) num21;
      long num43 = (long) BigDigest.Op4(num8) + (long) BigDigest.Op1(num8, num35, num28) + (long) BigDigest.K[index12];
      ulong[] word6 = this.m_word;
      int index13 = index12;
      int index14 = index13 + 1;
      long num44 = (long) word6[index13];
      long num45 = num43 + num44;
      ulong num46 = (ulong) (num42 + num45);
      num7 = num22 + num46;
      num3 = num46 + (BigDigest.Op3(num4) + BigDigest.Op2(num4, num36, num29));
      long num47 = (long) num28;
      long num48 = (long) BigDigest.Op4(num7) + (long) BigDigest.Op1(num7, num8, num35) + (long) BigDigest.K[index14];
      ulong[] word7 = this.m_word;
      int index15 = index14;
      int index16 = index15 + 1;
      long num49 = (long) word7[index15];
      long num50 = num48 + num49;
      ulong num51 = (ulong) (num47 + num50);
      num6 = num29 + num51;
      num2 = num51 + (BigDigest.Op3(num3) + BigDigest.Op2(num3, num4, num36));
      long num52 = (long) num35;
      long num53 = (long) BigDigest.Op4(num6) + (long) BigDigest.Op1(num6, num7, num8) + (long) BigDigest.K[index16];
      ulong[] word8 = this.m_word;
      int index17 = index16;
      index1 = index17 + 1;
      long num54 = (long) word8[index17];
      long num55 = num53 + num54;
      ulong num56 = (ulong) (num52 + num55);
      num5 = num36 + num56;
      num1 = num56 + (BigDigest.Op3(num2) + BigDigest.Op2(num2, num3, num4));
    }
    this.m_h1 += num1;
    this.m_h2 += num2;
    this.m_h3 += num3;
    this.m_h4 += num4;
    this.m_h5 += num5;
    this.m_h6 += num6;
    this.m_h7 += num7;
    this.m_h8 += num8;
    this.m_wordOffset = 0;
    Array.Clear((Array) this.m_word, 0, 16 /*0x10*/);
  }

  private static ulong Op1(ulong x, ulong y, ulong z)
  {
    return (ulong) ((long) x & (long) y ^ ~(long) x & (long) z);
  }

  private static ulong Op2(ulong x, ulong y, ulong z)
  {
    return (ulong) ((long) x & (long) y ^ (long) x & (long) z ^ (long) y & (long) z);
  }

  private static ulong Op3(ulong x)
  {
    return (ulong) (((long) x << 36 | (long) (x >> 28)) ^ ((long) x << 30 | (long) (x >> 34)) ^ ((long) x << 25 | (long) (x >> 39)));
  }

  private static ulong Op4(ulong x)
  {
    return (ulong) (((long) x << 50 | (long) (x >> 14)) ^ ((long) x << 46 | (long) (x >> 18)) ^ ((long) x << 23 | (long) (x >> 41)));
  }

  private static ulong Op5(ulong x)
  {
    return (ulong) (((long) x << 63 /*0x3F*/ | (long) (x >> 1)) ^ ((long) x << 56 | (long) (x >> 8))) ^ x >> 7;
  }

  private static ulong Op6(ulong x)
  {
    return (ulong) (((long) x << 45 | (long) (x >> 19)) ^ ((long) x << 3 | (long) (x >> 61))) ^ x >> 6;
  }

  public int ByteLength => this.m_length;

  public abstract string AlgorithmName { get; }

  public abstract int MessageDigestSize { get; }

  public abstract int DoFinal(byte[] bytes, int offset);
}
