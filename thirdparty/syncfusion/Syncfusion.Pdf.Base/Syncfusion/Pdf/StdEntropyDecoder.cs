// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.StdEntropyDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class StdEntropyDecoder : EntropyDecoder
{
  private const bool DO_TIMING = false;
  private const int ZC_LUT_BITS = 8;
  private const int SC_LUT_BITS = 9;
  private const int SC_LUT_MASK = 15;
  private const int SC_SPRED_SHIFT = 31 /*0x1F*/;
  private const int INT_SIGN_BIT = -2147483648 /*0x80000000*/;
  private const int MR_LUT_BITS = 9;
  private const int NUM_CTXTS = 19;
  private const int RLC_CTXT = 1;
  private const int UNIF_CTXT = 0;
  private const int SEG_SYMBOL = 10;
  private const int STATE_SEP = 16 /*0x10*/;
  private const int STATE_SIG_R1 = 32768 /*0x8000*/;
  private const int STATE_VISITED_R1 = 16384 /*0x4000*/;
  private const int STATE_NZ_CTXT_R1 = 8192 /*0x2000*/;
  private const int STATE_H_L_SIGN_R1 = 4096 /*0x1000*/;
  private const int STATE_H_R_SIGN_R1 = 2048 /*0x0800*/;
  private const int STATE_V_U_SIGN_R1 = 1024 /*0x0400*/;
  private const int STATE_V_D_SIGN_R1 = 512 /*0x0200*/;
  private const int STATE_PREV_MR_R1 = 256 /*0x0100*/;
  private const int STATE_H_L_R1 = 128 /*0x80*/;
  private const int STATE_H_R_R1 = 64 /*0x40*/;
  private const int STATE_V_U_R1 = 32 /*0x20*/;
  private const int STATE_V_D_R1 = 16 /*0x10*/;
  private const int STATE_D_UL_R1 = 8;
  private const int STATE_D_UR_R1 = 4;
  private const int STATE_D_DL_R1 = 2;
  private const int STATE_D_DR_R1 = 1;
  private const int ZC_MASK = 255 /*0xFF*/;
  private const int SC_SHIFT_R1 = 4;
  private const int MR_MASK = 511 /*0x01FF*/;
  private ByteToBitInput bin;
  private MQDecoder mq;
  private DecodeHelper decSpec;
  private int options;
  private bool doer;
  private bool verber;
  private static readonly int[] ZC_LUT_LH = new int[256 /*0x0100*/];
  private static readonly int[] ZC_LUT_HL = new int[256 /*0x0100*/];
  private static readonly int[] ZC_LUT_HH = new int[256 /*0x0100*/];
  private static readonly int[] SC_LUT = new int[512 /*0x0200*/];
  private static readonly int[] MR_LUT = new int[512 /*0x0200*/];
  private static readonly int[] MQ_INIT = new int[19]
  {
    46,
    3,
    4,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0
  };
  private int[] state;
  private static readonly int STATE_SIG_R2 = int.MinValue;
  private static readonly int STATE_VISITED_R2 = 1073741824 /*0x40000000*/;
  private static readonly int STATE_NZ_CTXT_R2 = 536870912 /*0x20000000*/;
  private static readonly int STATE_H_L_SIGN_R2 = 268435456 /*0x10000000*/;
  private static readonly int STATE_H_R_SIGN_R2 = 134217728 /*0x08000000*/;
  private static readonly int STATE_V_U_SIGN_R2 = 67108864 /*0x04000000*/;
  private static readonly int STATE_V_D_SIGN_R2 = 33554432 /*0x02000000*/;
  private static readonly int STATE_PREV_MR_R2 = 16777216 /*0x01000000*/;
  private static readonly int STATE_H_L_R2 = 8388608 /*0x800000*/;
  private static readonly int STATE_H_R_R2 = 4194304 /*0x400000*/;
  private static readonly int STATE_V_U_R2 = 2097152 /*0x200000*/;
  private static readonly int STATE_V_D_R2 = 1048576 /*0x100000*/;
  private static readonly int STATE_D_UL_R2 = 524288 /*0x080000*/;
  private static readonly int STATE_D_UR_R2 = 262144 /*0x040000*/;
  private static readonly int STATE_D_DL_R2 = 131072 /*0x020000*/;
  private static readonly int STATE_D_DR_R2 = 65536 /*0x010000*/;
  private static readonly int SIG_MASK_R1R2 = 32768 /*0x8000*/ | StdEntropyDecoder.STATE_SIG_R2;
  private static readonly int VSTD_MASK_R1R2 = 16384 /*0x4000*/ | StdEntropyDecoder.STATE_VISITED_R2;
  private static readonly int RLC_MASK_R1R2 = 32768 /*0x8000*/ | StdEntropyDecoder.STATE_SIG_R2 | 16384 /*0x4000*/ | StdEntropyDecoder.STATE_VISITED_R2 | 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2;
  private static readonly int SC_SHIFT_R2 = 20;
  private static readonly int SC_MASK = 511 /*0x01FF*/;
  private DecLyrdCBlk srcblk;
  private int mQuit;

  internal StdEntropyDecoder(
    CodedCBlkDataSrcDec src,
    DecodeHelper decSpec,
    bool doer,
    bool verber,
    int mQuit)
    : base(src)
  {
    this.decSpec = decSpec;
    this.doer = doer;
    this.verber = verber;
    this.mQuit = mQuit;
    this.state = new int[(decSpec.cblks.MaxCBlkWidth + 2) * ((decSpec.cblks.MaxCBlkHeight + 1) / 2 + 2)];
  }

  public override DataBlock getCodeBlock(int c, int m, int n, SubbandSyn sb, DataBlock cblk)
  {
    ByteInputBuffer byteInputBuffer = (ByteInputBuffer) null;
    this.srcblk = this.src.getCodeBlock(c, m, n, sb, 1, -1, this.srcblk);
    this.options = (int) this.decSpec.ecopts.getTileCompVal(this.tIdx, c);
    ArrayUtil.intArraySet(this.state, 0);
    if (cblk == null)
      cblk = (DataBlock) new DataBlockInt();
    cblk.progressive = this.srcblk.prog;
    cblk.ulx = this.srcblk.ulx;
    cblk.uly = this.srcblk.uly;
    cblk.w = this.srcblk.w;
    cblk.h = this.srcblk.h;
    cblk.offset = 0;
    cblk.scanw = cblk.w;
    int[] data = (int[]) cblk.Data;
    if (data == null || data.Length < this.srcblk.w * this.srcblk.h)
    {
      int[] numArray = new int[this.srcblk.w * this.srcblk.h];
      cblk.Data = (object) numArray;
    }
    else
      ArrayUtil.intArraySet(data, 0);
    if (this.srcblk.nl <= 0 || this.srcblk.nTrunc <= 0)
      return cblk;
    int num1 = this.srcblk.tsLengths == null ? this.srcblk.dl : this.srcblk.tsLengths[0];
    int num2 = 0;
    int num3 = this.srcblk.nTrunc;
    if (this.mq == null)
    {
      byteInputBuffer = new ByteInputBuffer(this.srcblk.data, 0, num1);
      this.mq = new MQDecoder(byteInputBuffer, 19, StdEntropyDecoder.MQ_INIT);
    }
    else
    {
      this.mq.nextSegment(this.srcblk.data, 0, num1);
      this.mq.resetCtxts();
    }
    bool flag = false;
    if ((this.options & StdEntropyCoderOptions.OPT_BYPASS) != 0 && this.bin == null)
    {
      if (byteInputBuffer == null)
        byteInputBuffer = this.mq.ByteInputBuffer;
      this.bin = new ByteToBitInput(byteInputBuffer);
    }
    int[] zc_lut;
    switch (sb.orientation)
    {
      case 0:
      case 2:
        zc_lut = StdEntropyDecoder.ZC_LUT_LH;
        break;
      case 1:
        zc_lut = StdEntropyDecoder.ZC_LUT_HL;
        break;
      case 3:
        zc_lut = StdEntropyDecoder.ZC_LUT_HH;
        break;
      default:
        throw new Exception("JJ2000 internal error");
    }
    int bp = 30 - this.srcblk.skipMSBP;
    if (this.mQuit != -1 && this.mQuit * 3 - 2 < num3)
      num3 = this.mQuit * 3 - 2;
    if (bp >= 0 && num3 > 0)
    {
      bool isterm = (this.options & StdEntropyCoderOptions.OPT_TERM_PASS) != 0 || (this.options & StdEntropyCoderOptions.OPT_BYPASS) != 0 && 31 /*0x1F*/ - StdEntropyCoderOptions.NUM_NON_BYPASS_MS_BP - this.srcblk.skipMSBP >= bp;
      flag = this.cleanuppass(cblk, this.mq, bp, this.state, zc_lut, isterm);
      --num3;
      if (!flag || !this.doer)
        --bp;
    }
    if (!flag || !this.doer)
    {
      for (; bp >= 0 && num3 > 0; --bp)
      {
        int num4;
        if ((this.options & StdEntropyCoderOptions.OPT_BYPASS) != 0 && bp < 31 /*0x1F*/ - StdEntropyCoderOptions.NUM_NON_BYPASS_MS_BP - this.srcblk.skipMSBP)
        {
          this.bin.setByteArray((byte[]) null, -1, this.srcblk.tsLengths[++num2]);
          bool isterm1 = (this.options & StdEntropyCoderOptions.OPT_TERM_PASS) != 0;
          flag = this.rawSigProgPass(cblk, this.bin, bp, this.state, isterm1);
          num4 = num3 - 1;
          if (num4 > 0 && (!flag || !this.doer))
          {
            if ((this.options & StdEntropyCoderOptions.OPT_TERM_PASS) != 0)
              this.bin.setByteArray((byte[]) null, -1, this.srcblk.tsLengths[++num2]);
            bool isterm2 = (this.options & StdEntropyCoderOptions.OPT_TERM_PASS) != 0 || (this.options & StdEntropyCoderOptions.OPT_BYPASS) != 0 && 31 /*0x1F*/ - StdEntropyCoderOptions.NUM_NON_BYPASS_MS_BP - this.srcblk.skipMSBP > bp;
            flag = this.rawMagRefPass(cblk, this.bin, bp, this.state, isterm2);
          }
          else
            break;
        }
        else
        {
          if ((this.options & StdEntropyCoderOptions.OPT_TERM_PASS) != 0)
            this.mq.nextSegment((byte[]) null, -1, this.srcblk.tsLengths[++num2]);
          bool isterm3 = (this.options & StdEntropyCoderOptions.OPT_TERM_PASS) != 0;
          flag = this.sigProgPass(cblk, this.mq, bp, this.state, zc_lut, isterm3);
          num4 = num3 - 1;
          if (num4 > 0 && (!flag || !this.doer))
          {
            if ((this.options & StdEntropyCoderOptions.OPT_TERM_PASS) != 0)
              this.mq.nextSegment((byte[]) null, -1, this.srcblk.tsLengths[++num2]);
            bool isterm4 = (this.options & StdEntropyCoderOptions.OPT_TERM_PASS) != 0 || (this.options & StdEntropyCoderOptions.OPT_BYPASS) != 0 && 31 /*0x1F*/ - StdEntropyCoderOptions.NUM_NON_BYPASS_MS_BP - this.srcblk.skipMSBP > bp;
            flag = this.magRefPass(cblk, this.mq, bp, this.state, isterm4);
          }
          else
            break;
        }
        int num5 = num4 - 1;
        if (num5 > 0 && (!flag || !this.doer))
        {
          if ((this.options & StdEntropyCoderOptions.OPT_TERM_PASS) != 0 || (this.options & StdEntropyCoderOptions.OPT_BYPASS) != 0 && bp < 31 /*0x1F*/ - StdEntropyCoderOptions.NUM_NON_BYPASS_MS_BP - this.srcblk.skipMSBP)
            this.mq.nextSegment((byte[]) null, -1, this.srcblk.tsLengths[++num2]);
          bool isterm = (this.options & StdEntropyCoderOptions.OPT_TERM_PASS) != 0 || (this.options & StdEntropyCoderOptions.OPT_BYPASS) != 0 && 31 /*0x1F*/ - StdEntropyCoderOptions.NUM_NON_BYPASS_MS_BP - this.srcblk.skipMSBP >= bp;
          flag = this.cleanuppass(cblk, this.mq, bp, this.state, zc_lut, isterm);
          num3 = num5 - 1;
          if (flag && this.doer)
            break;
        }
        else
          break;
      }
    }
    if (flag && this.doer)
    {
      int num6 = this.verber ? 1 : 0;
      this.conceal(cblk, bp);
    }
    return cblk;
  }

  public override DataBlock getInternCodeBlock(
    int c,
    int m,
    int n,
    SubbandSyn sb,
    DataBlock cblk)
  {
    return this.getCodeBlock(c, m, n, sb, cblk);
  }

  private bool sigProgPass(
    DataBlock cblk,
    MQDecoder mq,
    int bp,
    int[] state,
    int[] zc_lut,
    bool isterm)
  {
    int scanw = cblk.scanw;
    int num1 = cblk.w + 2;
    int num2 = num1 * StdEntropyCoderOptions.STRIPE_HEIGHT / 2 - cblk.w;
    int num3 = scanw * StdEntropyCoderOptions.STRIPE_HEIGHT - cblk.w;
    int num4 = 3 << bp >> 1;
    int[] data = (int[]) cblk.Data;
    int num5 = (cblk.h + StdEntropyCoderOptions.STRIPE_HEIGHT - 1) / StdEntropyCoderOptions.STRIPE_HEIGHT;
    bool flag1 = (this.options & StdEntropyCoderOptions.OPT_VERT_STR_CAUSAL) != 0;
    int num6 = -num1 - 1;
    int num7 = -num1 + 1;
    int num8 = num1 + 1;
    int num9 = num1 - 1;
    int offset = cblk.offset;
    int num10 = num1 + 1;
    int num11 = num5 - 1;
    while (num11 >= 0)
    {
      int num12 = num11 != 0 ? StdEntropyCoderOptions.STRIPE_HEIGHT : cblk.h - (num5 - 1) * StdEntropyCoderOptions.STRIPE_HEIGHT;
      int num13 = offset + cblk.w;
      while (offset < num13)
      {
        int index1 = num10;
        int number1 = state[index1];
        if ((~number1 & number1 << 2 & StdEntropyDecoder.SIG_MASK_R1R2) != 0)
        {
          int index2 = offset;
          if ((number1 & 40960 /*0xA000*/) == 8192 /*0x2000*/)
          {
            if (mq.decodeSymbol(zc_lut[number1 & (int) byte.MaxValue]) != 0)
            {
              int number2 = StdEntropyDecoder.SC_LUT[SupportClass.URShift(number1, 4) & StdEntropyDecoder.SC_MASK];
              int num14 = mq.decodeSymbol(number2 & 15) ^ SupportClass.URShift(number2, 31 /*0x1F*/);
              data[index2] = num14 << 31 /*0x1F*/ | num4;
              if (!flag1)
              {
                state[index1 + num6] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DR_R2;
                state[index1 + num7] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DL_R2;
              }
              if (num14 != 0)
              {
                number1 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2 | StdEntropyDecoder.STATE_V_U_SIGN_R2;
                if (!flag1)
                  state[index1 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2 | StdEntropyDecoder.STATE_V_D_SIGN_R2;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | 4096 /*0x1000*/ | StdEntropyDecoder.STATE_D_UL_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | 2048 /*0x0800*/ | StdEntropyDecoder.STATE_D_UR_R2;
              }
              else
              {
                number1 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2;
                if (!flag1)
                  state[index1 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | StdEntropyDecoder.STATE_D_UL_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | StdEntropyDecoder.STATE_D_UR_R2;
              }
            }
            else
              number1 |= 16384 /*0x4000*/;
          }
          if (num12 < 2)
          {
            state[index1] = number1;
            goto label_41;
          }
          if ((number1 & (StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_NZ_CTXT_R2)) == StdEntropyDecoder.STATE_NZ_CTXT_R2)
          {
            int index3 = index2 + scanw;
            if (mq.decodeSymbol(zc_lut[SupportClass.URShift(number1, 16 /*0x10*/) & (int) byte.MaxValue]) != 0)
            {
              int number3 = StdEntropyDecoder.SC_LUT[SupportClass.URShift(number1, StdEntropyDecoder.SC_SHIFT_R2) & StdEntropyDecoder.SC_MASK];
              int num15 = mq.decodeSymbol(number3 & 15) ^ SupportClass.URShift(number3, 31 /*0x1F*/);
              data[index3] = num15 << 31 /*0x1F*/ | num4;
              state[index1 + num9] |= 8196;
              state[index1 + num8] |= 8200;
              if (num15 != 0)
              {
                number1 |= StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2 | 8192 /*0x2000*/ | 16 /*0x10*/ | 512 /*0x0200*/;
                state[index1 + num1] |= 9248;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2 | StdEntropyDecoder.STATE_H_L_SIGN_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2 | StdEntropyDecoder.STATE_H_R_SIGN_R2;
              }
              else
              {
                number1 |= StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2 | 8192 /*0x2000*/ | 16 /*0x10*/;
                state[index1 + num1] |= 8224;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2;
              }
            }
            else
              number1 |= StdEntropyDecoder.STATE_VISITED_R2;
          }
          state[index1] = number1;
        }
        if (num12 >= 3)
        {
          int index4 = index1 + num1;
          int number4 = state[index4];
          if ((~number4 & number4 << 2 & StdEntropyDecoder.SIG_MASK_R1R2) != 0)
          {
            int index5 = offset + (scanw << 1);
            if ((number4 & 40960 /*0xA000*/) == 8192 /*0x2000*/)
            {
              if (mq.decodeSymbol(zc_lut[number4 & (int) byte.MaxValue]) != 0)
              {
                int number5 = StdEntropyDecoder.SC_LUT[SupportClass.URShift(number4, 4) & StdEntropyDecoder.SC_MASK];
                int num16 = mq.decodeSymbol(number5 & 15) ^ SupportClass.URShift(number5, 31 /*0x1F*/);
                data[index5] = num16 << 31 /*0x1F*/ | num4;
                state[index4 + num6] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DR_R2;
                state[index4 + num7] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DL_R2;
                if (num16 != 0)
                {
                  number4 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2 | StdEntropyDecoder.STATE_V_U_SIGN_R2;
                  state[index4 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2 | StdEntropyDecoder.STATE_V_D_SIGN_R2;
                  state[index4 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | 4096 /*0x1000*/ | StdEntropyDecoder.STATE_D_UL_R2;
                  state[index4 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | 2048 /*0x0800*/ | StdEntropyDecoder.STATE_D_UR_R2;
                }
                else
                {
                  number4 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2;
                  state[index4 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2;
                  state[index4 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | StdEntropyDecoder.STATE_D_UL_R2;
                  state[index4 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | StdEntropyDecoder.STATE_D_UR_R2;
                }
              }
              else
                number4 |= 16384 /*0x4000*/;
            }
            if (num12 < 4)
            {
              state[index4] = number4;
            }
            else
            {
              if ((number4 & (StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_NZ_CTXT_R2)) == StdEntropyDecoder.STATE_NZ_CTXT_R2)
              {
                int index6 = index5 + scanw;
                if (mq.decodeSymbol(zc_lut[SupportClass.URShift(number4, 16 /*0x10*/) & (int) byte.MaxValue]) != 0)
                {
                  int number6 = StdEntropyDecoder.SC_LUT[SupportClass.URShift(number4, StdEntropyDecoder.SC_SHIFT_R2) & StdEntropyDecoder.SC_MASK];
                  int num17 = mq.decodeSymbol(number6 & 15) ^ SupportClass.URShift(number6, 31 /*0x1F*/);
                  data[index6] = num17 << 31 /*0x1F*/ | num4;
                  state[index4 + num9] |= 8196;
                  state[index4 + num8] |= 8200;
                  if (num17 != 0)
                  {
                    number4 |= StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2 | 8192 /*0x2000*/ | 16 /*0x10*/ | 512 /*0x0200*/;
                    state[index4 + num1] |= 9248;
                    state[index4 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2 | StdEntropyDecoder.STATE_H_L_SIGN_R2;
                    state[index4 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2 | StdEntropyDecoder.STATE_H_R_SIGN_R2;
                  }
                  else
                  {
                    number4 |= StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2 | 8192 /*0x2000*/ | 16 /*0x10*/;
                    state[index4 + num1] |= 8224;
                    state[index4 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2;
                    state[index4 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2;
                  }
                }
                else
                  number4 |= StdEntropyDecoder.STATE_VISITED_R2;
              }
              state[index4] = number4;
            }
          }
        }
label_41:
        ++offset;
        ++num10;
      }
      --num11;
      offset += num3;
      num10 += num2;
    }
    bool flag2 = false;
    if (isterm && (this.options & StdEntropyCoderOptions.OPT_PRED_TERM) != 0)
      flag2 = mq.checkPredTerm();
    if ((this.options & StdEntropyCoderOptions.OPT_RESET_MQ) != 0)
      mq.resetCtxts();
    return flag2;
  }

  private bool rawSigProgPass(
    DataBlock cblk,
    ByteToBitInput bin,
    int bp,
    int[] state,
    bool isterm)
  {
    int scanw = cblk.scanw;
    int num1 = cblk.w + 2;
    int num2 = num1 * StdEntropyCoderOptions.STRIPE_HEIGHT / 2 - cblk.w;
    int num3 = scanw * StdEntropyCoderOptions.STRIPE_HEIGHT - cblk.w;
    int num4 = 3 << bp >> 1;
    int[] data = (int[]) cblk.Data;
    int num5 = (cblk.h + StdEntropyCoderOptions.STRIPE_HEIGHT - 1) / StdEntropyCoderOptions.STRIPE_HEIGHT;
    bool flag1 = (this.options & StdEntropyCoderOptions.OPT_VERT_STR_CAUSAL) != 0;
    int num6 = -num1 - 1;
    int num7 = -num1 + 1;
    int num8 = num1 + 1;
    int num9 = num1 - 1;
    int offset = cblk.offset;
    int num10 = num1 + 1;
    int num11 = num5 - 1;
    while (num11 >= 0)
    {
      int num12 = num11 != 0 ? StdEntropyCoderOptions.STRIPE_HEIGHT : cblk.h - (num5 - 1) * StdEntropyCoderOptions.STRIPE_HEIGHT;
      int num13 = offset + cblk.w;
      while (offset < num13)
      {
        int index1 = num10;
        int num14 = state[index1];
        if ((~num14 & num14 << 2 & StdEntropyDecoder.SIG_MASK_R1R2) != 0)
        {
          int index2 = offset;
          if ((num14 & 40960 /*0xA000*/) == 8192 /*0x2000*/)
          {
            if (bin.readBit() != 0)
            {
              int num15 = bin.readBit();
              data[index2] = num15 << 31 /*0x1F*/ | num4;
              if (!flag1)
              {
                state[index1 + num6] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DR_R2;
                state[index1 + num7] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DL_R2;
              }
              if (num15 != 0)
              {
                num14 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2 | StdEntropyDecoder.STATE_V_U_SIGN_R2;
                if (!flag1)
                  state[index1 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2 | StdEntropyDecoder.STATE_V_D_SIGN_R2;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | 4096 /*0x1000*/ | StdEntropyDecoder.STATE_D_UL_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | 2048 /*0x0800*/ | StdEntropyDecoder.STATE_D_UR_R2;
              }
              else
              {
                num14 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2;
                if (!flag1)
                  state[index1 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | StdEntropyDecoder.STATE_D_UL_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | StdEntropyDecoder.STATE_D_UR_R2;
              }
            }
            else
              num14 |= 16384 /*0x4000*/;
          }
          if (num12 < 2)
          {
            state[index1] = num14;
            goto label_41;
          }
          if ((num14 & (StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_NZ_CTXT_R2)) == StdEntropyDecoder.STATE_NZ_CTXT_R2)
          {
            int index3 = index2 + scanw;
            if (bin.readBit() != 0)
            {
              int num16 = bin.readBit();
              data[index3] = num16 << 31 /*0x1F*/ | num4;
              state[index1 + num9] |= 8196;
              state[index1 + num8] |= 8200;
              if (num16 != 0)
              {
                num14 |= StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2 | 8192 /*0x2000*/ | 16 /*0x10*/ | 512 /*0x0200*/;
                state[index1 + num1] |= 9248;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2 | StdEntropyDecoder.STATE_H_L_SIGN_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2 | StdEntropyDecoder.STATE_H_R_SIGN_R2;
              }
              else
              {
                num14 |= StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2 | 8192 /*0x2000*/ | 16 /*0x10*/;
                state[index1 + num1] |= 8224;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2;
              }
            }
            else
              num14 |= StdEntropyDecoder.STATE_VISITED_R2;
          }
          state[index1] = num14;
        }
        if (num12 >= 3)
        {
          int index4 = index1 + num1;
          int num17 = state[index4];
          if ((~num17 & num17 << 2 & StdEntropyDecoder.SIG_MASK_R1R2) != 0)
          {
            int index5 = offset + (scanw << 1);
            if ((num17 & 40960 /*0xA000*/) == 8192 /*0x2000*/)
            {
              if (bin.readBit() != 0)
              {
                int num18 = bin.readBit();
                data[index5] = num18 << 31 /*0x1F*/ | num4;
                state[index4 + num6] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DR_R2;
                state[index4 + num7] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DL_R2;
                if (num18 != 0)
                {
                  num17 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2 | StdEntropyDecoder.STATE_V_U_SIGN_R2;
                  state[index4 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2 | StdEntropyDecoder.STATE_V_D_SIGN_R2;
                  state[index4 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | 4096 /*0x1000*/ | StdEntropyDecoder.STATE_D_UL_R2;
                  state[index4 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | 2048 /*0x0800*/ | StdEntropyDecoder.STATE_D_UR_R2;
                }
                else
                {
                  num17 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2;
                  state[index4 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2;
                  state[index4 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | StdEntropyDecoder.STATE_D_UL_R2;
                  state[index4 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | StdEntropyDecoder.STATE_D_UR_R2;
                }
              }
              else
                num17 |= 16384 /*0x4000*/;
            }
            if (num12 < 4)
            {
              state[index4] = num17;
            }
            else
            {
              if ((num17 & (StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_NZ_CTXT_R2)) == StdEntropyDecoder.STATE_NZ_CTXT_R2)
              {
                int index6 = index5 + scanw;
                if (bin.readBit() != 0)
                {
                  int num19 = bin.readBit();
                  data[index6] = num19 << 31 /*0x1F*/ | num4;
                  state[index4 + num9] |= 8196;
                  state[index4 + num8] |= 8200;
                  if (num19 != 0)
                  {
                    num17 |= StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2 | 8192 /*0x2000*/ | 16 /*0x10*/ | 512 /*0x0200*/;
                    state[index4 + num1] |= 9248;
                    state[index4 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2 | StdEntropyDecoder.STATE_H_L_SIGN_R2;
                    state[index4 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2 | StdEntropyDecoder.STATE_H_R_SIGN_R2;
                  }
                  else
                  {
                    num17 |= StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2 | 8192 /*0x2000*/ | 16 /*0x10*/;
                    state[index4 + num1] |= 8224;
                    state[index4 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2;
                    state[index4 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2;
                  }
                }
                else
                  num17 |= StdEntropyDecoder.STATE_VISITED_R2;
              }
              state[index4] = num17;
            }
          }
        }
label_41:
        ++offset;
        ++num10;
      }
      --num11;
      offset += num3;
      num10 += num2;
    }
    bool flag2 = false;
    if (isterm && (this.options & StdEntropyCoderOptions.OPT_PRED_TERM) != 0)
      flag2 = bin.checkBytePadding();
    return flag2;
  }

  private bool magRefPass(DataBlock cblk, MQDecoder mq, int bp, int[] state, bool isterm)
  {
    int scanw = cblk.scanw;
    int num1 = cblk.w + 2;
    int num2 = num1 * StdEntropyCoderOptions.STRIPE_HEIGHT / 2 - cblk.w;
    int num3 = scanw * StdEntropyCoderOptions.STRIPE_HEIGHT - cblk.w;
    int num4 = 1 << bp >> 1;
    int num5 = -1 << bp + 1;
    int[] data = (int[]) cblk.Data;
    int num6 = (cblk.h + StdEntropyCoderOptions.STRIPE_HEIGHT - 1) / StdEntropyCoderOptions.STRIPE_HEIGHT;
    int offset = cblk.offset;
    int num7 = num1 + 1;
    int num8 = num6 - 1;
    while (num8 >= 0)
    {
      int num9 = num8 != 0 ? StdEntropyCoderOptions.STRIPE_HEIGHT : cblk.h - (num6 - 1) * StdEntropyCoderOptions.STRIPE_HEIGHT;
      int num10 = offset + cblk.w;
      while (offset < num10)
      {
        int index1 = num7;
        int number1 = state[index1];
        if ((SupportClass.URShift(number1, 1) & ~number1 & StdEntropyDecoder.VSTD_MASK_R1R2) != 0)
        {
          int index2 = offset;
          if ((number1 & 49152 /*0xC000*/) == 32768 /*0x8000*/)
          {
            int num11 = mq.decodeSymbol(StdEntropyDecoder.MR_LUT[number1 & 511 /*0x01FF*/]);
            data[index2] &= num5;
            data[index2] |= num11 << bp | num4;
            number1 |= 256 /*0x0100*/;
          }
          if (num9 < 2)
          {
            state[index1] = number1;
            goto label_19;
          }
          if ((number1 & (StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2)) == StdEntropyDecoder.STATE_SIG_R2)
          {
            int index3 = index2 + scanw;
            int num12 = mq.decodeSymbol(StdEntropyDecoder.MR_LUT[SupportClass.URShift(number1, 16 /*0x10*/) & 511 /*0x01FF*/]);
            data[index3] &= num5;
            data[index3] |= num12 << bp | num4;
            number1 |= StdEntropyDecoder.STATE_PREV_MR_R2;
          }
          state[index1] = number1;
        }
        if (num9 >= 3)
        {
          int index4 = index1 + num1;
          int number2 = state[index4];
          if ((SupportClass.URShift(number2, 1) & ~number2 & StdEntropyDecoder.VSTD_MASK_R1R2) != 0)
          {
            int index5 = offset + (scanw << 1);
            if ((number2 & 49152 /*0xC000*/) == 32768 /*0x8000*/)
            {
              int num13 = mq.decodeSymbol(StdEntropyDecoder.MR_LUT[number2 & 511 /*0x01FF*/]);
              data[index5] &= num5;
              data[index5] |= num13 << bp | num4;
              number2 |= 256 /*0x0100*/;
            }
            if (num9 < 4)
            {
              state[index4] = number2;
            }
            else
            {
              if ((state[index4] & (StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2)) == StdEntropyDecoder.STATE_SIG_R2)
              {
                int index6 = index5 + scanw;
                int num14 = mq.decodeSymbol(StdEntropyDecoder.MR_LUT[SupportClass.URShift(number2, 16 /*0x10*/) & 511 /*0x01FF*/]);
                data[index6] &= num5;
                data[index6] |= num14 << bp | num4;
                number2 |= StdEntropyDecoder.STATE_PREV_MR_R2;
              }
              state[index4] = number2;
            }
          }
        }
label_19:
        ++offset;
        ++num7;
      }
      --num8;
      offset += num3;
      num7 += num2;
    }
    bool flag = false;
    if (isterm && (this.options & StdEntropyCoderOptions.OPT_PRED_TERM) != 0)
      flag = mq.checkPredTerm();
    if ((this.options & StdEntropyCoderOptions.OPT_RESET_MQ) != 0)
      mq.resetCtxts();
    return flag;
  }

  private bool rawMagRefPass(
    DataBlock cblk,
    ByteToBitInput bin,
    int bp,
    int[] state,
    bool isterm)
  {
    int scanw = cblk.scanw;
    int num1 = cblk.w + 2;
    int num2 = num1 * StdEntropyCoderOptions.STRIPE_HEIGHT / 2 - cblk.w;
    int num3 = scanw * StdEntropyCoderOptions.STRIPE_HEIGHT - cblk.w;
    int num4 = 1 << bp >> 1;
    int num5 = -1 << bp + 1;
    int[] data = (int[]) cblk.Data;
    int num6 = (cblk.h + StdEntropyCoderOptions.STRIPE_HEIGHT - 1) / StdEntropyCoderOptions.STRIPE_HEIGHT;
    int offset = cblk.offset;
    int num7 = num1 + 1;
    int num8 = num6 - 1;
    while (num8 >= 0)
    {
      int num9 = num8 != 0 ? StdEntropyCoderOptions.STRIPE_HEIGHT : cblk.h - (num6 - 1) * StdEntropyCoderOptions.STRIPE_HEIGHT;
      int num10 = offset + cblk.w;
      while (offset < num10)
      {
        int index1 = num7;
        int number1 = state[index1];
        if ((SupportClass.URShift(number1, 1) & ~number1 & StdEntropyDecoder.VSTD_MASK_R1R2) != 0)
        {
          int index2 = offset;
          if ((number1 & 49152 /*0xC000*/) == 32768 /*0x8000*/)
          {
            int num11 = bin.readBit();
            data[index2] &= num5;
            data[index2] |= num11 << bp | num4;
          }
          if (num9 >= 2)
          {
            if ((number1 & (StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2)) == StdEntropyDecoder.STATE_SIG_R2)
            {
              int index3 = index2 + scanw;
              int num12 = bin.readBit();
              data[index3] &= num5;
              data[index3] |= num12 << bp | num4;
            }
          }
          else
            goto label_14;
        }
        if (num9 >= 3)
        {
          int index4 = index1 + num1;
          int number2 = state[index4];
          if ((SupportClass.URShift(number2, 1) & ~number2 & StdEntropyDecoder.VSTD_MASK_R1R2) != 0)
          {
            int index5 = offset + (scanw << 1);
            if ((number2 & 49152 /*0xC000*/) == 32768 /*0x8000*/)
            {
              int num13 = bin.readBit();
              data[index5] &= num5;
              data[index5] |= num13 << bp | num4;
            }
            if (num9 >= 4 && (state[index4] & (StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2)) == StdEntropyDecoder.STATE_SIG_R2)
            {
              int index6 = index5 + scanw;
              int num14 = bin.readBit();
              data[index6] &= num5;
              data[index6] |= num14 << bp | num4;
            }
          }
        }
label_14:
        ++offset;
        ++num7;
      }
      --num8;
      offset += num3;
      num7 += num2;
    }
    bool flag = false;
    if (isterm && (this.options & StdEntropyCoderOptions.OPT_PRED_TERM) != 0)
      flag = bin.checkBytePadding();
    return flag;
  }

  private bool cleanuppass(
    DataBlock cblk,
    MQDecoder mq,
    int bp,
    int[] state,
    int[] zc_lut,
    bool isterm)
  {
    int scanw = cblk.scanw;
    int num1 = cblk.w + 2;
    int num2 = num1 * StdEntropyCoderOptions.STRIPE_HEIGHT / 2 - cblk.w;
    int num3 = scanw * StdEntropyCoderOptions.STRIPE_HEIGHT - cblk.w;
    int num4 = 3 << bp >> 1;
    int[] data = (int[]) cblk.Data;
    int num5 = (cblk.h + StdEntropyCoderOptions.STRIPE_HEIGHT - 1) / StdEntropyCoderOptions.STRIPE_HEIGHT;
    bool flag1 = (this.options & StdEntropyCoderOptions.OPT_VERT_STR_CAUSAL) != 0;
    int num6 = -num1 - 1;
    int num7 = -num1 + 1;
    int num8 = num1 + 1;
    int num9 = num1 - 1;
    int offset = cblk.offset;
    int num10 = num1 + 1;
    int num11 = num5 - 1;
    while (num11 >= 0)
    {
      int num12 = num11 != 0 ? StdEntropyCoderOptions.STRIPE_HEIGHT : cblk.h - (num5 - 1) * StdEntropyCoderOptions.STRIPE_HEIGHT;
      int num13 = offset + cblk.w;
      while (offset < num13)
      {
        int index1 = num10;
        int number1 = state[index1];
        if (number1 == 0 && state[index1 + num1] == 0 && num12 == StdEntropyCoderOptions.STRIPE_HEIGHT)
        {
          if (mq.decodeSymbol(1) != 0)
          {
            int num14 = mq.decodeSymbol(0) << 1 | mq.decodeSymbol(0);
            int index2 = offset + num14 * scanw;
            if (num14 > 1)
            {
              index1 += num1;
              number1 = state[index1];
            }
            if ((num14 & 1) == 0)
            {
              int number2 = StdEntropyDecoder.SC_LUT[number1 >> 4 & StdEntropyDecoder.SC_MASK];
              int num15 = mq.decodeSymbol(number2 & 15) ^ SupportClass.URShift(number2, 31 /*0x1F*/);
              data[index2] = num15 << 31 /*0x1F*/ | num4;
              if (num14 != 0 || !flag1)
              {
                state[index1 + num6] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DR_R2;
                state[index1 + num7] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DL_R2;
              }
              if (num15 != 0)
              {
                number1 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2 | StdEntropyDecoder.STATE_V_U_SIGN_R2;
                if (num14 != 0 || !flag1)
                  state[index1 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2 | StdEntropyDecoder.STATE_V_D_SIGN_R2;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | 4096 /*0x1000*/ | StdEntropyDecoder.STATE_D_UL_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | 2048 /*0x0800*/ | StdEntropyDecoder.STATE_D_UR_R2;
              }
              else
              {
                number1 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2;
                if (num14 != 0 || !flag1)
                  state[index1 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | StdEntropyDecoder.STATE_D_UL_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | StdEntropyDecoder.STATE_D_UR_R2;
              }
              if (num14 >> 1 != 0)
                goto label_42;
            }
            else
            {
              int number3 = StdEntropyDecoder.SC_LUT[number1 >> StdEntropyDecoder.SC_SHIFT_R2 & StdEntropyDecoder.SC_MASK];
              int num16 = mq.decodeSymbol(number3 & 15) ^ SupportClass.URShift(number3, 31 /*0x1F*/);
              data[index2] = num16 << 31 /*0x1F*/ | num4;
              state[index1 + num9] |= 8196;
              state[index1 + num8] |= 8200;
              int num17;
              if (num16 != 0)
              {
                num17 = number1 | StdEntropyDecoder.STATE_SIG_R2 | 8192 /*0x2000*/ | 16 /*0x10*/ | 512 /*0x0200*/;
                state[index1 + num1] |= 9248;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2 | StdEntropyDecoder.STATE_H_L_SIGN_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2 | StdEntropyDecoder.STATE_H_R_SIGN_R2;
              }
              else
              {
                num17 = number1 | StdEntropyDecoder.STATE_SIG_R2 | 8192 /*0x2000*/ | 16 /*0x10*/;
                state[index1 + num1] |= 8224;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2;
              }
              state[index1] = num17;
              if (num14 >> 1 == 0)
              {
                index1 += num1;
                number1 = state[index1];
                goto label_42;
              }
              goto label_55;
            }
          }
          else
            goto label_55;
        }
        if (((number1 >> 1 | number1) & StdEntropyDecoder.VSTD_MASK_R1R2) != StdEntropyDecoder.VSTD_MASK_R1R2)
        {
          int index3 = offset;
          if ((number1 & 49152 /*0xC000*/) == 0 && mq.decodeSymbol(zc_lut[number1 & (int) byte.MaxValue]) != 0)
          {
            int number4 = StdEntropyDecoder.SC_LUT[SupportClass.URShift(number1, 4) & StdEntropyDecoder.SC_MASK];
            int num18 = mq.decodeSymbol(number4 & 15) ^ SupportClass.URShift(number4, 31 /*0x1F*/);
            data[index3] = num18 << 31 /*0x1F*/ | num4;
            if (!flag1)
            {
              state[index1 + num6] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DR_R2;
              state[index1 + num7] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DL_R2;
            }
            if (num18 != 0)
            {
              number1 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2 | StdEntropyDecoder.STATE_V_U_SIGN_R2;
              if (!flag1)
                state[index1 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2 | StdEntropyDecoder.STATE_V_D_SIGN_R2;
              state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | 4096 /*0x1000*/ | StdEntropyDecoder.STATE_D_UL_R2;
              state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | 2048 /*0x0800*/ | StdEntropyDecoder.STATE_D_UR_R2;
            }
            else
            {
              number1 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2;
              if (!flag1)
                state[index1 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2;
              state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | StdEntropyDecoder.STATE_D_UL_R2;
              state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | StdEntropyDecoder.STATE_D_UR_R2;
            }
          }
          if (num12 < 2)
          {
            int num19 = number1 & ~(16384 /*0x4000*/ | StdEntropyDecoder.STATE_VISITED_R2);
            state[index1] = num19;
            goto label_55;
          }
          if ((number1 & (StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2)) == 0)
          {
            int index4 = index3 + scanw;
            if (mq.decodeSymbol(zc_lut[SupportClass.URShift(number1, 16 /*0x10*/) & (int) byte.MaxValue]) != 0)
            {
              int number5 = StdEntropyDecoder.SC_LUT[SupportClass.URShift(number1, StdEntropyDecoder.SC_SHIFT_R2) & StdEntropyDecoder.SC_MASK];
              int num20 = mq.decodeSymbol(number5 & 15) ^ SupportClass.URShift(number5, 31 /*0x1F*/);
              data[index4] = num20 << 31 /*0x1F*/ | num4;
              state[index1 + num9] |= 8196;
              state[index1 + num8] |= 8200;
              if (num20 != 0)
              {
                number1 |= StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2 | 8192 /*0x2000*/ | 16 /*0x10*/ | 512 /*0x0200*/;
                state[index1 + num1] |= 9248;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2 | StdEntropyDecoder.STATE_H_L_SIGN_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2 | StdEntropyDecoder.STATE_H_R_SIGN_R2;
              }
              else
              {
                number1 |= StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2 | 8192 /*0x2000*/ | 16 /*0x10*/;
                state[index1 + num1] |= 8224;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2;
              }
            }
          }
        }
        int num21 = number1 & ~(16384 /*0x4000*/ | StdEntropyDecoder.STATE_VISITED_R2);
        state[index1] = num21;
        if (num12 >= 3)
        {
          index1 += num1;
          number1 = state[index1];
        }
        else
          goto label_55;
label_42:
        if (((number1 >> 1 | number1) & StdEntropyDecoder.VSTD_MASK_R1R2) != StdEntropyDecoder.VSTD_MASK_R1R2)
        {
          int index5 = offset + (scanw << 1);
          if ((number1 & 49152 /*0xC000*/) == 0 && mq.decodeSymbol(zc_lut[number1 & (int) byte.MaxValue]) != 0)
          {
            int number6 = StdEntropyDecoder.SC_LUT[number1 >> 4 & StdEntropyDecoder.SC_MASK];
            int num22 = mq.decodeSymbol(number6 & 15) ^ SupportClass.URShift(number6, 31 /*0x1F*/);
            data[index5] = num22 << 31 /*0x1F*/ | num4;
            state[index1 + num6] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DR_R2;
            state[index1 + num7] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_D_DL_R2;
            if (num22 != 0)
            {
              number1 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2 | StdEntropyDecoder.STATE_V_U_SIGN_R2;
              state[index1 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2 | StdEntropyDecoder.STATE_V_D_SIGN_R2;
              state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | 4096 /*0x1000*/ | StdEntropyDecoder.STATE_D_UL_R2;
              state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | 2048 /*0x0800*/ | StdEntropyDecoder.STATE_D_UR_R2;
            }
            else
            {
              number1 |= 49152 /*0xC000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_U_R2;
              state[index1 - num1] |= StdEntropyDecoder.STATE_NZ_CTXT_R2 | StdEntropyDecoder.STATE_V_D_R2;
              state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 128 /*0x80*/ | StdEntropyDecoder.STATE_D_UL_R2;
              state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 64 /*0x40*/ | StdEntropyDecoder.STATE_D_UR_R2;
            }
          }
          if (num12 < 4)
          {
            int num23 = number1 & ~(16384 /*0x4000*/ | StdEntropyDecoder.STATE_VISITED_R2);
            state[index1] = num23;
            goto label_55;
          }
          if ((number1 & (StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2)) == 0)
          {
            int index6 = index5 + scanw;
            if (mq.decodeSymbol(zc_lut[SupportClass.URShift(number1, 16 /*0x10*/) & (int) byte.MaxValue]) != 0)
            {
              int number7 = StdEntropyDecoder.SC_LUT[SupportClass.URShift(number1, StdEntropyDecoder.SC_SHIFT_R2) & StdEntropyDecoder.SC_MASK];
              int num24 = mq.decodeSymbol(number7 & 15) ^ SupportClass.URShift(number7, 31 /*0x1F*/);
              data[index6] = num24 << 31 /*0x1F*/ | num4;
              state[index1 + num9] |= 8196;
              state[index1 + num8] |= 8200;
              if (num24 != 0)
              {
                number1 |= StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2 | 8192 /*0x2000*/ | 16 /*0x10*/ | 512 /*0x0200*/;
                state[index1 + num1] |= 9248;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2 | StdEntropyDecoder.STATE_H_L_SIGN_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2 | StdEntropyDecoder.STATE_H_R_SIGN_R2;
              }
              else
              {
                number1 |= StdEntropyDecoder.STATE_SIG_R2 | StdEntropyDecoder.STATE_VISITED_R2 | 8192 /*0x2000*/ | 16 /*0x10*/;
                state[index1 + num1] |= 8224;
                state[index1 + 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 2 | StdEntropyDecoder.STATE_H_L_R2;
                state[index1 - 1] |= 8192 /*0x2000*/ | StdEntropyDecoder.STATE_NZ_CTXT_R2 | 1 | StdEntropyDecoder.STATE_H_R_R2;
              }
            }
          }
        }
        int num25 = number1 & ~(16384 /*0x4000*/ | StdEntropyDecoder.STATE_VISITED_R2);
        state[index1] = num25;
label_55:
        ++offset;
        ++num10;
      }
      --num11;
      offset += num3;
      num10 += num2;
    }
    bool flag2 = (this.options & StdEntropyCoderOptions.OPT_SEG_SYMBOLS) != 0 && (mq.decodeSymbol(0) << 3 | mq.decodeSymbol(0) << 2 | mq.decodeSymbol(0) << 1 | mq.decodeSymbol(0)) != 10;
    if (isterm && (this.options & StdEntropyCoderOptions.OPT_PRED_TERM) != 0)
      flag2 = mq.checkPredTerm();
    if ((this.options & StdEntropyCoderOptions.OPT_RESET_MQ) != 0)
      mq.resetCtxts();
    return flag2;
  }

  private void conceal(DataBlock cblk, int bp)
  {
    int num1 = 1 << bp;
    int num2 = -1 << bp;
    int[] data = (int[]) cblk.Data;
    int num3 = cblk.h - 1;
    int offset = cblk.offset;
    for (; num3 >= 0; --num3)
    {
      for (int index = offset + cblk.w; offset < index; ++offset)
      {
        int num4 = data[offset];
        data[offset] = (num4 & num2 & int.MaxValue) == 0 ? 0 : num4 & num2 | num1;
      }
      offset += cblk.scanw - cblk.w;
    }
  }

  static StdEntropyDecoder()
  {
    StdEntropyDecoder.ZC_LUT_LH[0] = 2;
    for (int index = 1; index < 16 /*0x10*/; ++index)
      StdEntropyDecoder.ZC_LUT_LH[index] = 4;
    for (int index = 0; index < 4; ++index)
      StdEntropyDecoder.ZC_LUT_LH[1 << index] = 3;
    for (int index = 0; index < 16 /*0x10*/; ++index)
    {
      StdEntropyDecoder.ZC_LUT_LH[32 /*0x20*/ | index] = 5;
      StdEntropyDecoder.ZC_LUT_LH[16 /*0x10*/ | index] = 5;
      StdEntropyDecoder.ZC_LUT_LH[48 /*0x30*/ | index] = 6;
    }
    StdEntropyDecoder.ZC_LUT_LH[128 /*0x80*/] = 7;
    StdEntropyDecoder.ZC_LUT_LH[64 /*0x40*/] = 7;
    for (int index = 1; index < 16 /*0x10*/; ++index)
    {
      StdEntropyDecoder.ZC_LUT_LH[128 /*0x80*/ | index] = 8;
      StdEntropyDecoder.ZC_LUT_LH[64 /*0x40*/ | index] = 8;
    }
    for (int index1 = 1; index1 < 4; ++index1)
    {
      for (int index2 = 0; index2 < 16 /*0x10*/; ++index2)
      {
        StdEntropyDecoder.ZC_LUT_LH[128 /*0x80*/ | index1 << 4 | index2] = 9;
        StdEntropyDecoder.ZC_LUT_LH[64 /*0x40*/ | index1 << 4 | index2] = 9;
      }
    }
    for (int index = 0; index < 64 /*0x40*/; ++index)
      StdEntropyDecoder.ZC_LUT_LH[192 /*0xC0*/ | index] = 10;
    StdEntropyDecoder.ZC_LUT_HL[0] = 2;
    for (int index = 1; index < 16 /*0x10*/; ++index)
      StdEntropyDecoder.ZC_LUT_HL[index] = 4;
    for (int index = 0; index < 4; ++index)
      StdEntropyDecoder.ZC_LUT_HL[1 << index] = 3;
    for (int index = 0; index < 16 /*0x10*/; ++index)
    {
      StdEntropyDecoder.ZC_LUT_HL[128 /*0x80*/ | index] = 5;
      StdEntropyDecoder.ZC_LUT_HL[64 /*0x40*/ | index] = 5;
      StdEntropyDecoder.ZC_LUT_HL[192 /*0xC0*/ | index] = 6;
    }
    StdEntropyDecoder.ZC_LUT_HL[32 /*0x20*/] = 7;
    StdEntropyDecoder.ZC_LUT_HL[16 /*0x10*/] = 7;
    for (int index = 1; index < 16 /*0x10*/; ++index)
    {
      StdEntropyDecoder.ZC_LUT_HL[32 /*0x20*/ | index] = 8;
      StdEntropyDecoder.ZC_LUT_HL[16 /*0x10*/ | index] = 8;
    }
    for (int index3 = 1; index3 < 4; ++index3)
    {
      for (int index4 = 0; index4 < 16 /*0x10*/; ++index4)
      {
        StdEntropyDecoder.ZC_LUT_HL[index3 << 6 | 32 /*0x20*/ | index4] = 9;
        StdEntropyDecoder.ZC_LUT_HL[index3 << 6 | 16 /*0x10*/ | index4] = 9;
      }
    }
    for (int index5 = 0; index5 < 4; ++index5)
    {
      for (int index6 = 0; index6 < 16 /*0x10*/; ++index6)
        StdEntropyDecoder.ZC_LUT_HL[index5 << 6 | 32 /*0x20*/ | 16 /*0x10*/ | index6] = 10;
    }
    int[] numArray1 = new int[6]{ 3, 5, 6, 9, 10, 12 };
    int[] numArray2 = new int[4]{ 1, 2, 4, 8 };
    int[] numArray3 = new int[11]
    {
      3,
      5,
      6,
      7,
      9,
      10,
      11,
      12,
      13,
      14,
      15
    };
    int[] numArray4 = new int[5]{ 7, 11, 13, 14, 15 };
    StdEntropyDecoder.ZC_LUT_HH[0] = 2;
    for (int index = 0; index < numArray2.Length; ++index)
      StdEntropyDecoder.ZC_LUT_HH[numArray2[index] << 4] = 3;
    for (int index = 0; index < numArray3.Length; ++index)
      StdEntropyDecoder.ZC_LUT_HH[numArray3[index] << 4] = 4;
    for (int index = 0; index < numArray2.Length; ++index)
      StdEntropyDecoder.ZC_LUT_HH[numArray2[index]] = 5;
    for (int index7 = 0; index7 < numArray2.Length; ++index7)
    {
      for (int index8 = 0; index8 < numArray2.Length; ++index8)
        StdEntropyDecoder.ZC_LUT_HH[numArray2[index7] << 4 | numArray2[index8]] = 6;
    }
    for (int index9 = 0; index9 < numArray3.Length; ++index9)
    {
      for (int index10 = 0; index10 < numArray2.Length; ++index10)
        StdEntropyDecoder.ZC_LUT_HH[numArray3[index9] << 4 | numArray2[index10]] = 7;
    }
    for (int index = 0; index < numArray1.Length; ++index)
      StdEntropyDecoder.ZC_LUT_HH[numArray1[index]] = 8;
    for (int index11 = 0; index11 < numArray1.Length; ++index11)
    {
      for (int index12 = 1; index12 < 16 /*0x10*/; ++index12)
        StdEntropyDecoder.ZC_LUT_HH[index12 << 4 | numArray1[index11]] = 9;
    }
    for (int index13 = 0; index13 < 16 /*0x10*/; ++index13)
    {
      for (int index14 = 0; index14 < numArray4.Length; ++index14)
        StdEntropyDecoder.ZC_LUT_HH[index13 << 4 | numArray4[index14]] = 10;
    }
    int[] numArray5 = new int[36]
    {
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      15,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0
    };
    numArray5[17] = 14;
    numArray5[16 /*0x10*/] = 13;
    numArray5[10] = 12;
    numArray5[9] = 11;
    numArray5[8] = -2147483636 /*0x8000000C*/;
    numArray5[2] = -2147483635 /*0x8000000D*/;
    numArray5[1] = -2147483634 /*0x8000000E*/;
    numArray5[0] = -2147483633 /*0x8000000F*/;
    for (int index = 0; index < 511 /*0x01FF*/; ++index)
    {
      int num1 = index & 1;
      int num2 = index >> 1 & 1;
      int num3 = index >> 2 & 1;
      int num4 = index >> 3 & 1;
      int num5 = index >> 5 & 1;
      int num6 = index >> 6 & 1;
      int num7 = index >> 7 & 1;
      int num8 = index >> 8 & 1;
      int num9 = num4 * (1 - 2 * num8) + num3 * (1 - 2 * num7);
      int num10 = num9 >= -1 ? num9 : -1;
      int num11 = num10 <= 1 ? num10 : 1;
      int num12 = num2 * (1 - 2 * num6) + num1 * (1 - 2 * num5);
      int num13 = num12 >= -1 ? num12 : -1;
      int num14 = num13 <= 1 ? num13 : 1;
      StdEntropyDecoder.SC_LUT[index] = numArray5[num11 + 1 << 3 | num14 + 1];
    }
    StdEntropyDecoder.MR_LUT[0] = 16 /*0x10*/;
    int index15;
    for (index15 = 1; index15 < 256 /*0x0100*/; ++index15)
      StdEntropyDecoder.MR_LUT[index15] = 17;
    for (; index15 < 512 /*0x0200*/; ++index15)
      StdEntropyDecoder.MR_LUT[index15] = 18;
  }
}
