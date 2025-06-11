// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.InverseDCT
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class InverseDCT
{
  private const int IFAST_SCALE_BITS = 2;
  private const int RANGE_MASK = 1023 /*0x03FF*/;
  private const int SLOW_INTEGER_CONST_BITS = 13;
  private const int SLOW_INTEGER_PASS1_BITS = 2;
  private const int SLOW_INTEGER_FIX_0_298631336 = 2446;
  private const int SLOW_INTEGER_FIX_0_390180644 = 3196;
  private const int SLOW_INTEGER_FIX_0_541196100 = 4433;
  private const int SLOW_INTEGER_FIX_0_765366865 = 6270;
  private const int SLOW_INTEGER_FIX_0_899976223 = 7373;
  private const int SLOW_INTEGER_FIX_1_175875602 = 9633;
  private const int SLOW_INTEGER_FIX_1_501321110 = 12299;
  private const int SLOW_INTEGER_FIX_1_847759065 = 15137;
  private const int SLOW_INTEGER_FIX_1_961570560 = 16069;
  private const int SLOW_INTEGER_FIX_2_053119869 = 16819;
  private const int SLOW_INTEGER_FIX_2_562915447 = 20995;
  private const int SLOW_INTEGER_FIX_3_072711026 = 25172;
  private const int FAST_INTEGER_CONST_BITS = 8;
  private const int FAST_INTEGER_PASS1_BITS = 2;
  private const int FAST_INTEGER_FIX_1_082392200 = 277;
  private const int FAST_INTEGER_FIX_1_414213562 = 362;
  private const int FAST_INTEGER_FIX_1_847759065 = 473;
  private const int FAST_INTEGER_FIX_2_613125930 = 669;
  private const int REDUCED_CONST_BITS = 13;
  private const int REDUCED_PASS1_BITS = 2;
  private const int REDUCED_FIX_0_211164243 = 1730;
  private const int REDUCED_FIX_0_509795579 = 4176;
  private const int REDUCED_FIX_0_601344887 = 4926;
  private const int REDUCED_FIX_0_720959822 = 5906;
  private const int REDUCED_FIX_0_765366865 = 6270;
  private const int REDUCED_FIX_0_850430095 = 6967;
  private const int REDUCED_FIX_0_899976223 = 7373;
  private const int REDUCED_FIX_1_061594337 = 8697;
  private const int REDUCED_FIX_1_272758580 = 10426;
  private const int REDUCED_FIX_1_451774981 = 11893;
  private const int REDUCED_FIX_1_847759065 = 15137;
  private const int REDUCED_FIX_2_172734803 = 17799;
  private const int REDUCED_FIX_2_562915447 = 20995;
  private const int REDUCED_FIX_3_624509785 = 29692;
  private const int CONST_BITS = 14;
  private static readonly short[] aanscales = new short[64 /*0x40*/]
  {
    (short) 16384 /*0x4000*/,
    (short) 22725,
    (short) 21407,
    (short) 19266,
    (short) 16384 /*0x4000*/,
    (short) 12873,
    (short) 8867,
    (short) 4520,
    (short) 22725,
    (short) 31521,
    (short) 29692,
    (short) 26722,
    (short) 22725,
    (short) 17855,
    (short) 12299,
    (short) 6270,
    (short) 21407,
    (short) 29692,
    (short) 27969,
    (short) 25172,
    (short) 21407,
    (short) 16819,
    (short) 11585,
    (short) 5906,
    (short) 19266,
    (short) 26722,
    (short) 25172,
    (short) 22654,
    (short) 19266,
    (short) 15137,
    (short) 10426,
    (short) 5315,
    (short) 16384 /*0x4000*/,
    (short) 22725,
    (short) 21407,
    (short) 19266,
    (short) 16384 /*0x4000*/,
    (short) 12873,
    (short) 8867,
    (short) 4520,
    (short) 12873,
    (short) 17855,
    (short) 16819,
    (short) 15137,
    (short) 12873,
    (short) 10114,
    (short) 6967,
    (short) 3552,
    (short) 8867,
    (short) 12299,
    (short) 11585,
    (short) 10426,
    (short) 8867,
    (short) 6967,
    (short) 4799,
    (short) 2446,
    (short) 4520,
    (short) 6270,
    (short) 5906,
    (short) 5315,
    (short) 4520,
    (short) 3552,
    (short) 2446,
    (short) 1247
  };
  private static readonly double[] aanscalefactor = new double[8]
  {
    1.0,
    1.387039845,
    1.306562965,
    1.175875602,
    1.0,
    0.785694958,
    0.5411961,
    0.275899379
  };
  private InverseDCT.InverseMethod[] m_inverse_DCT_method = new InverseDCT.InverseMethod[10];
  private InverseDCT.multiplier_table[] m_dctTables;
  private DecompressStruct m_cinfo;
  private int[] m_cur_method = new int[10];
  private ComponentBuffer m_componentBuffer;

  public InverseDCT(DecompressStruct cinfo)
  {
    this.m_cinfo = cinfo;
    this.m_dctTables = new InverseDCT.multiplier_table[cinfo.m_num_components];
    for (int index = 0; index < cinfo.m_num_components; ++index)
    {
      this.m_dctTables[index] = new InverseDCT.multiplier_table();
      this.m_cur_method[index] = -1;
    }
  }

  public void start_pass()
  {
    for (int index1 = 0; index1 < this.m_cinfo.m_num_components; ++index1)
    {
      ComponentInfo componentInfo = this.m_cinfo.Comp_info[index1];
      InverseDCT.InverseMethod inverseMethod = InverseDCT.InverseMethod.Unknown;
      int num = 0;
      switch (componentInfo.DCT_scaled_size)
      {
        case 1:
          inverseMethod = InverseDCT.InverseMethod.idct_1x1_method;
          num = 0;
          break;
        case 2:
          inverseMethod = InverseDCT.InverseMethod.idct_2x2_method;
          num = 0;
          break;
        case 4:
          inverseMethod = InverseDCT.InverseMethod.idct_4x4_method;
          num = 0;
          break;
        case 8:
          switch (this.m_cinfo.m_dct_method)
          {
            case J_DCT_METHOD.JDCT_ISLOW:
              inverseMethod = InverseDCT.InverseMethod.idct_islow_method;
              num = 0;
              break;
            case J_DCT_METHOD.JDCT_IFAST:
              inverseMethod = InverseDCT.InverseMethod.idct_ifast_method;
              num = 1;
              break;
            case J_DCT_METHOD.JDCT_FLOAT:
              inverseMethod = InverseDCT.InverseMethod.idct_float_method;
              num = 2;
              break;
          }
          break;
      }
      this.m_inverse_DCT_method[index1] = inverseMethod;
      if (componentInfo.component_needed && this.m_cur_method[index1] != num && componentInfo.quant_table != null)
      {
        this.m_cur_method[index1] = num;
        switch (num)
        {
          case 0:
            int[] intArray1 = this.m_dctTables[index1].int_array;
            for (int index2 = 0; index2 < 64 /*0x40*/; ++index2)
              intArray1[index2] = (int) componentInfo.quant_table.quantval[index2];
            continue;
          case 1:
            int[] intArray2 = this.m_dctTables[index1].int_array;
            for (int index3 = 0; index3 < 64 /*0x40*/; ++index3)
              intArray2[index3] = JpegUtils.DESCALE((int) componentInfo.quant_table.quantval[index3] * (int) InverseDCT.aanscales[index3], 12);
            continue;
          case 2:
            float[] floatArray = this.m_dctTables[index1].float_array;
            int index4 = 0;
            for (int index5 = 0; index5 < 8; ++index5)
            {
              for (int index6 = 0; index6 < 8; ++index6)
              {
                floatArray[index4] = (float) ((double) componentInfo.quant_table.quantval[index4] * InverseDCT.aanscalefactor[index5] * InverseDCT.aanscalefactor[index6]);
                ++index4;
              }
            }
            continue;
          default:
            continue;
        }
      }
    }
  }

  public void inverse(
    int component_index,
    short[] coef_block,
    ComponentBuffer output_buf,
    int output_row,
    int output_col)
  {
    this.m_componentBuffer = output_buf;
    switch (this.m_inverse_DCT_method[component_index])
    {
      case InverseDCT.InverseMethod.idct_1x1_method:
        this.jpeg_idct_1x1(component_index, coef_block, output_row, output_col);
        break;
      case InverseDCT.InverseMethod.idct_2x2_method:
        this.jpeg_idct_2x2(component_index, coef_block, output_row, output_col);
        break;
      case InverseDCT.InverseMethod.idct_4x4_method:
        this.jpeg_idct_4x4(component_index, coef_block, output_row, output_col);
        break;
      case InverseDCT.InverseMethod.idct_islow_method:
        this.jpeg_idct_islow(component_index, coef_block, output_row, output_col);
        break;
      case InverseDCT.InverseMethod.idct_ifast_method:
        this.jpeg_idct_ifast(component_index, coef_block, output_row, output_col);
        break;
      case InverseDCT.InverseMethod.idct_float_method:
        this.jpeg_idct_float(component_index, coef_block, output_row, output_col);
        break;
    }
  }

  private void jpeg_idct_islow(
    int component_index,
    short[] coef_block,
    int output_row,
    int output_col)
  {
    int[] numArray = new int[64 /*0x40*/];
    int index1 = 0;
    int[] intArray = this.m_dctTables[component_index].int_array;
    int index2 = 0;
    int index3 = 0;
    for (int index4 = 8; index4 > 0; --index4)
    {
      if (coef_block[index1 + 8] == (short) 0 && coef_block[index1 + 16 /*0x10*/] == (short) 0 && coef_block[index1 + 24] == (short) 0 && coef_block[index1 + 32 /*0x20*/] == (short) 0 && coef_block[index1 + 40] == (short) 0 && coef_block[index1 + 48 /*0x30*/] == (short) 0 && coef_block[index1 + 56] == (short) 0)
      {
        int num = InverseDCT.SLOW_INTEGER_DEQUANTIZE((int) coef_block[index1], intArray[index2]) << 2;
        numArray[index3] = num;
        numArray[index3 + 8] = num;
        numArray[index3 + 16 /*0x10*/] = num;
        numArray[index3 + 24] = num;
        numArray[index3 + 32 /*0x20*/] = num;
        numArray[index3 + 40] = num;
        numArray[index3 + 48 /*0x30*/] = num;
        numArray[index3 + 56] = num;
        ++index1;
        ++index2;
        ++index3;
      }
      else
      {
        int num1 = InverseDCT.SLOW_INTEGER_DEQUANTIZE((int) coef_block[index1 + 16 /*0x10*/], intArray[index2 + 16 /*0x10*/]);
        int num2 = InverseDCT.SLOW_INTEGER_DEQUANTIZE((int) coef_block[index1 + 48 /*0x30*/], intArray[index2 + 48 /*0x30*/]);
        int num3 = (num1 + num2) * 4433;
        int num4 = num3 + num2 * -15137;
        int num5 = num3 + num1 * 6270;
        int num6 = InverseDCT.SLOW_INTEGER_DEQUANTIZE((int) coef_block[index1], intArray[index2]);
        int num7 = InverseDCT.SLOW_INTEGER_DEQUANTIZE((int) coef_block[index1 + 32 /*0x20*/], intArray[index2 + 32 /*0x20*/]);
        int num8 = num6 + num7 << 13;
        int num9 = num6 - num7 << 13;
        int num10 = num8 + num5;
        int num11 = num8 - num5;
        int num12 = num9 + num4;
        int num13 = num9 - num4;
        int num14 = InverseDCT.SLOW_INTEGER_DEQUANTIZE((int) coef_block[index1 + 56], intArray[index2 + 56]);
        int num15 = InverseDCT.SLOW_INTEGER_DEQUANTIZE((int) coef_block[index1 + 40], intArray[index2 + 40]);
        int num16 = InverseDCT.SLOW_INTEGER_DEQUANTIZE((int) coef_block[index1 + 24], intArray[index2 + 24]);
        int num17 = InverseDCT.SLOW_INTEGER_DEQUANTIZE((int) coef_block[index1 + 8], intArray[index2 + 8]);
        int num18 = num14 + num17;
        int num19 = num15 + num16;
        int num20 = num14 + num16;
        int num21 = num15 + num17;
        int num22 = (num20 + num21) * 9633;
        int num23 = num14 * 2446;
        int num24 = num15 * 16819;
        int num25 = num16 * 25172;
        int num26 = num17 * 12299;
        int num27 = num18 * -7373;
        int num28 = num19 * -20995;
        int num29 = num20 * -16069;
        int num30 = num21 * -3196;
        int num31 = num29 + num22;
        int num32 = num30 + num22;
        int num33 = num23 + (num27 + num31);
        int num34 = num24 + (num28 + num32);
        int num35 = num25 + (num28 + num31);
        int num36 = num26 + (num27 + num32);
        numArray[index3] = JpegUtils.DESCALE(num10 + num36, 11);
        numArray[index3 + 56] = JpegUtils.DESCALE(num10 - num36, 11);
        numArray[index3 + 8] = JpegUtils.DESCALE(num12 + num35, 11);
        numArray[index3 + 48 /*0x30*/] = JpegUtils.DESCALE(num12 - num35, 11);
        numArray[index3 + 16 /*0x10*/] = JpegUtils.DESCALE(num13 + num34, 11);
        numArray[index3 + 40] = JpegUtils.DESCALE(num13 - num34, 11);
        numArray[index3 + 24] = JpegUtils.DESCALE(num11 + num33, 11);
        numArray[index3 + 32 /*0x20*/] = JpegUtils.DESCALE(num11 - num33, 11);
        ++index1;
        ++index2;
        ++index3;
      }
    }
    int index5 = 0;
    byte[] sampleRangeLimit = this.m_cinfo.m_sample_range_limit;
    int num37 = this.m_cinfo.m_sampleRangeLimitOffset + 128 /*0x80*/;
    for (int index6 = 0; index6 < 8; ++index6)
    {
      int i = output_row + index6;
      if (numArray[index5 + 1] == 0 && numArray[index5 + 2] == 0 && numArray[index5 + 3] == 0 && numArray[index5 + 4] == 0 && numArray[index5 + 5] == 0 && numArray[index5 + 6] == 0 && numArray[index5 + 7] == 0)
      {
        byte num38 = sampleRangeLimit[num37 + JpegUtils.DESCALE(numArray[index5], 5) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col] = num38;
        this.m_componentBuffer[i][output_col + 1] = num38;
        this.m_componentBuffer[i][output_col + 2] = num38;
        this.m_componentBuffer[i][output_col + 3] = num38;
        this.m_componentBuffer[i][output_col + 4] = num38;
        this.m_componentBuffer[i][output_col + 5] = num38;
        this.m_componentBuffer[i][output_col + 6] = num38;
        this.m_componentBuffer[i][output_col + 7] = num38;
        index5 += 8;
      }
      else
      {
        int num39 = numArray[index5 + 2];
        int num40 = numArray[index5 + 6];
        int num41 = (num39 + num40) * 4433;
        int num42 = num41 + num40 * -15137;
        int num43 = num41 + num39 * 6270;
        int num44 = numArray[index5] + numArray[index5 + 4] << 13;
        int num45 = numArray[index5] - numArray[index5 + 4] << 13;
        int num46 = num44 + num43;
        int num47 = num44 - num43;
        int num48 = num45 + num42;
        int num49 = num45 - num42;
        int num50 = numArray[index5 + 7];
        int num51 = numArray[index5 + 5];
        int num52 = numArray[index5 + 3];
        int num53 = numArray[index5 + 1];
        int num54 = num50 + num53;
        int num55 = num51 + num52;
        int num56 = num50 + num52;
        int num57 = num51 + num53;
        int num58 = (num56 + num57) * 9633;
        int num59 = num50 * 2446;
        int num60 = num51 * 16819;
        int num61 = num52 * 25172;
        int num62 = num53 * 12299;
        int num63 = num54 * -7373;
        int num64 = num55 * -20995;
        int num65 = num56 * -16069;
        int num66 = num57 * -3196;
        int num67 = num65 + num58;
        int num68 = num66 + num58;
        int num69 = num59 + (num63 + num67);
        int num70 = num60 + (num64 + num68);
        int num71 = num61 + (num64 + num67);
        int num72 = num62 + (num63 + num68);
        this.m_componentBuffer[i][output_col] = sampleRangeLimit[num37 + JpegUtils.DESCALE(num46 + num72, 18) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 7] = sampleRangeLimit[num37 + JpegUtils.DESCALE(num46 - num72, 18) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 1] = sampleRangeLimit[num37 + JpegUtils.DESCALE(num48 + num71, 18) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 6] = sampleRangeLimit[num37 + JpegUtils.DESCALE(num48 - num71, 18) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 2] = sampleRangeLimit[num37 + JpegUtils.DESCALE(num49 + num70, 18) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 5] = sampleRangeLimit[num37 + JpegUtils.DESCALE(num49 - num70, 18) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 3] = sampleRangeLimit[num37 + JpegUtils.DESCALE(num47 + num69, 18) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 4] = sampleRangeLimit[num37 + JpegUtils.DESCALE(num47 - num69, 18) & 1023 /*0x03FF*/];
        index5 += 8;
      }
    }
  }

  private static int SLOW_INTEGER_DEQUANTIZE(int coef, int quantval) => coef * quantval;

  private void jpeg_idct_ifast(
    int component_index,
    short[] coef_block,
    int output_row,
    int output_col)
  {
    int[] numArray = new int[64 /*0x40*/];
    int index1 = 0;
    int index2 = 0;
    int[] intArray = this.m_dctTables[component_index].int_array;
    int index3 = 0;
    for (int index4 = 8; index4 > 0; --index4)
    {
      if (coef_block[index1 + 8] == (short) 0 && coef_block[index1 + 16 /*0x10*/] == (short) 0 && coef_block[index1 + 24] == (short) 0 && coef_block[index1 + 32 /*0x20*/] == (short) 0 && coef_block[index1 + 40] == (short) 0 && coef_block[index1 + 48 /*0x30*/] == (short) 0 && coef_block[index1 + 56] == (short) 0)
      {
        int num = InverseDCT.FAST_INTEGER_DEQUANTIZE(coef_block[index1], intArray[index3]);
        numArray[index2] = num;
        numArray[index2 + 8] = num;
        numArray[index2 + 16 /*0x10*/] = num;
        numArray[index2 + 24] = num;
        numArray[index2 + 32 /*0x20*/] = num;
        numArray[index2 + 40] = num;
        numArray[index2 + 48 /*0x30*/] = num;
        numArray[index2 + 56] = num;
        ++index1;
        ++index3;
        ++index2;
      }
      else
      {
        int num1 = InverseDCT.FAST_INTEGER_DEQUANTIZE(coef_block[index1], intArray[index3]);
        int num2 = InverseDCT.FAST_INTEGER_DEQUANTIZE(coef_block[index1 + 16 /*0x10*/], intArray[index3 + 16 /*0x10*/]);
        int num3 = InverseDCT.FAST_INTEGER_DEQUANTIZE(coef_block[index1 + 32 /*0x20*/], intArray[index3 + 32 /*0x20*/]);
        int num4 = InverseDCT.FAST_INTEGER_DEQUANTIZE(coef_block[index1 + 48 /*0x30*/], intArray[index3 + 48 /*0x30*/]);
        int num5 = num1 + num3;
        int num6 = num1 - num3;
        int num7 = num2 + num4;
        int num8 = InverseDCT.FAST_INTEGER_MULTIPLY(num2 - num4, 362) - num7;
        int num9 = num5 + num7;
        int num10 = num5 - num7;
        int num11 = num6 + num8;
        int num12 = num6 - num8;
        int num13 = InverseDCT.FAST_INTEGER_DEQUANTIZE(coef_block[index1 + 8], intArray[index3 + 8]);
        int num14 = InverseDCT.FAST_INTEGER_DEQUANTIZE(coef_block[index1 + 24], intArray[index3 + 24]);
        int num15 = InverseDCT.FAST_INTEGER_DEQUANTIZE(coef_block[index1 + 40], intArray[index3 + 40]);
        int num16 = InverseDCT.FAST_INTEGER_DEQUANTIZE(coef_block[index1 + 56], intArray[index3 + 56]);
        int num17 = num15 + num14;
        int var1 = num15 - num14;
        int num18 = num13 + num16;
        int var2 = num13 - num16;
        int num19 = num18 + num17;
        int num20 = InverseDCT.FAST_INTEGER_MULTIPLY(num18 - num17, 362);
        int num21 = InverseDCT.FAST_INTEGER_MULTIPLY(var1 + var2, 473);
        int num22 = InverseDCT.FAST_INTEGER_MULTIPLY(var2, 277) - num21;
        int num23 = InverseDCT.FAST_INTEGER_MULTIPLY(var1, -669) + num21 - num19;
        int num24 = num20 - num23;
        int num25 = num22 + num24;
        numArray[index2] = num9 + num19;
        numArray[index2 + 56] = num9 - num19;
        numArray[index2 + 8] = num11 + num23;
        numArray[index2 + 48 /*0x30*/] = num11 - num23;
        numArray[index2 + 16 /*0x10*/] = num12 + num24;
        numArray[index2 + 40] = num12 - num24;
        numArray[index2 + 32 /*0x20*/] = num10 + num25;
        numArray[index2 + 24] = num10 - num25;
        ++index1;
        ++index3;
        ++index2;
      }
    }
    int index5 = 0;
    byte[] sampleRangeLimit = this.m_cinfo.m_sample_range_limit;
    int num26 = this.m_cinfo.m_sampleRangeLimitOffset + 128 /*0x80*/;
    for (int index6 = 0; index6 < 8; ++index6)
    {
      int i = output_row + index6;
      if (numArray[index5 + 1] == 0 && numArray[index5 + 2] == 0 && numArray[index5 + 3] == 0 && numArray[index5 + 4] == 0 && numArray[index5 + 5] == 0 && numArray[index5 + 6] == 0 && numArray[index5 + 7] == 0)
      {
        byte num27 = sampleRangeLimit[num26 + InverseDCT.FAST_INTEGER_IDESCALE(numArray[index5], 5) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col] = num27;
        this.m_componentBuffer[i][output_col + 1] = num27;
        this.m_componentBuffer[i][output_col + 2] = num27;
        this.m_componentBuffer[i][output_col + 3] = num27;
        this.m_componentBuffer[i][output_col + 4] = num27;
        this.m_componentBuffer[i][output_col + 5] = num27;
        this.m_componentBuffer[i][output_col + 6] = num27;
        this.m_componentBuffer[i][output_col + 7] = num27;
        index5 += 8;
      }
      else
      {
        int num28 = numArray[index5] + numArray[index5 + 4];
        int num29 = numArray[index5] - numArray[index5 + 4];
        int num30 = numArray[index5 + 2] + numArray[index5 + 6];
        int num31 = InverseDCT.FAST_INTEGER_MULTIPLY(numArray[index5 + 2] - numArray[index5 + 6], 362) - num30;
        int num32 = num28 + num30;
        int num33 = num28 - num30;
        int num34 = num29 + num31;
        int num35 = num29 - num31;
        int num36 = numArray[index5 + 5] + numArray[index5 + 3];
        int var3 = numArray[index5 + 5] - numArray[index5 + 3];
        int num37 = numArray[index5 + 1] + numArray[index5 + 7];
        int var4 = numArray[index5 + 1] - numArray[index5 + 7];
        int num38 = num37 + num36;
        int num39 = InverseDCT.FAST_INTEGER_MULTIPLY(num37 - num36, 362);
        int num40 = InverseDCT.FAST_INTEGER_MULTIPLY(var3 + var4, 473);
        int num41 = InverseDCT.FAST_INTEGER_MULTIPLY(var4, 277) - num40;
        int num42 = InverseDCT.FAST_INTEGER_MULTIPLY(var3, -669) + num40 - num38;
        int num43 = num39 - num42;
        int num44 = num41 + num43;
        this.m_componentBuffer[i][output_col] = sampleRangeLimit[num26 + InverseDCT.FAST_INTEGER_IDESCALE(num32 + num38, 5) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 7] = sampleRangeLimit[num26 + InverseDCT.FAST_INTEGER_IDESCALE(num32 - num38, 5) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 1] = sampleRangeLimit[num26 + InverseDCT.FAST_INTEGER_IDESCALE(num34 + num42, 5) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 6] = sampleRangeLimit[num26 + InverseDCT.FAST_INTEGER_IDESCALE(num34 - num42, 5) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 2] = sampleRangeLimit[num26 + InverseDCT.FAST_INTEGER_IDESCALE(num35 + num43, 5) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 5] = sampleRangeLimit[num26 + InverseDCT.FAST_INTEGER_IDESCALE(num35 - num43, 5) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 4] = sampleRangeLimit[num26 + InverseDCT.FAST_INTEGER_IDESCALE(num33 + num44, 5) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 3] = sampleRangeLimit[num26 + InverseDCT.FAST_INTEGER_IDESCALE(num33 - num44, 5) & 1023 /*0x03FF*/];
        index5 += 8;
      }
    }
  }

  private static int FAST_INTEGER_MULTIPLY(int var, int c) => JpegUtils.RIGHT_SHIFT(var * c, 8);

  private static int FAST_INTEGER_DEQUANTIZE(short coef, int quantval) => (int) coef * quantval;

  private static int FAST_INTEGER_IRIGHT_SHIFT(int x, int shft) => x >> shft;

  private static int FAST_INTEGER_IDESCALE(int x, int n)
  {
    return InverseDCT.FAST_INTEGER_IRIGHT_SHIFT(x, n);
  }

  private void jpeg_idct_float(
    int component_index,
    short[] coef_block,
    int output_row,
    int output_col)
  {
    float[] numArray = new float[64 /*0x40*/];
    int index1 = 0;
    int index2 = 0;
    float[] floatArray = this.m_dctTables[component_index].float_array;
    int index3 = 0;
    for (int index4 = 8; index4 > 0; --index4)
    {
      if (coef_block[index1 + 8] == (short) 0 && coef_block[index1 + 16 /*0x10*/] == (short) 0 && coef_block[index1 + 24] == (short) 0 && coef_block[index1 + 32 /*0x20*/] == (short) 0 && coef_block[index1 + 40] == (short) 0 && coef_block[index1 + 48 /*0x30*/] == (short) 0 && coef_block[index1 + 56] == (short) 0)
      {
        float num = InverseDCT.FLOAT_DEQUANTIZE(coef_block[index1], floatArray[index3]);
        numArray[index2] = num;
        numArray[index2 + 8] = num;
        numArray[index2 + 16 /*0x10*/] = num;
        numArray[index2 + 24] = num;
        numArray[index2 + 32 /*0x20*/] = num;
        numArray[index2 + 40] = num;
        numArray[index2 + 48 /*0x30*/] = num;
        numArray[index2 + 56] = num;
        ++index1;
        ++index3;
        ++index2;
      }
      else
      {
        float num1 = InverseDCT.FLOAT_DEQUANTIZE(coef_block[index1], floatArray[index3]);
        float num2 = InverseDCT.FLOAT_DEQUANTIZE(coef_block[index1 + 16 /*0x10*/], floatArray[index3 + 16 /*0x10*/]);
        float num3 = InverseDCT.FLOAT_DEQUANTIZE(coef_block[index1 + 32 /*0x20*/], floatArray[index3 + 32 /*0x20*/]);
        float num4 = InverseDCT.FLOAT_DEQUANTIZE(coef_block[index1 + 48 /*0x30*/], floatArray[index3 + 48 /*0x30*/]);
        float num5 = num1 + num3;
        float num6 = num1 - num3;
        float num7 = num2 + num4;
        float num8 = (float) (((double) num2 - (double) num4) * 1.4142135381698608) - num7;
        float num9 = num5 + num7;
        float num10 = num5 - num7;
        float num11 = num6 + num8;
        float num12 = num6 - num8;
        float num13 = InverseDCT.FLOAT_DEQUANTIZE(coef_block[index1 + 8], floatArray[index3 + 8]);
        float num14 = InverseDCT.FLOAT_DEQUANTIZE(coef_block[index1 + 24], floatArray[index3 + 24]);
        float num15 = InverseDCT.FLOAT_DEQUANTIZE(coef_block[index1 + 40], floatArray[index3 + 40]);
        float num16 = InverseDCT.FLOAT_DEQUANTIZE(coef_block[index1 + 56], floatArray[index3 + 56]);
        float num17 = num15 + num14;
        float num18 = num15 - num14;
        float num19 = num13 + num16;
        float num20 = num13 - num16;
        float num21 = num19 + num17;
        float num22 = (float) (((double) num19 - (double) num17) * 1.4142135381698608);
        float num23 = (float) (((double) num18 + (double) num20) * 1.8477590084075928);
        float num24 = 1.08239222f * num20 - num23;
        float num25 = -2.613126f * num18 + num23 - num21;
        float num26 = num22 - num25;
        float num27 = num24 + num26;
        numArray[index2] = num9 + num21;
        numArray[index2 + 56] = num9 - num21;
        numArray[index2 + 8] = num11 + num25;
        numArray[index2 + 48 /*0x30*/] = num11 - num25;
        numArray[index2 + 16 /*0x10*/] = num12 + num26;
        numArray[index2 + 40] = num12 - num26;
        numArray[index2 + 32 /*0x20*/] = num10 + num27;
        numArray[index2 + 24] = num10 - num27;
        ++index1;
        ++index3;
        ++index2;
      }
    }
    int index5 = 0;
    byte[] sampleRangeLimit = this.m_cinfo.m_sample_range_limit;
    int num28 = this.m_cinfo.m_sampleRangeLimitOffset + 128 /*0x80*/;
    for (int index6 = 0; index6 < 8; ++index6)
    {
      float num29 = numArray[index5] + numArray[index5 + 4];
      float num30 = numArray[index5] - numArray[index5 + 4];
      float num31 = numArray[index5 + 2] + numArray[index5 + 6];
      float num32 = (float) (((double) numArray[index5 + 2] - (double) numArray[index5 + 6]) * 1.4142135381698608) - num31;
      float num33 = num29 + num31;
      float num34 = num29 - num31;
      float num35 = num30 + num32;
      float num36 = num30 - num32;
      float num37 = numArray[index5 + 5] + numArray[index5 + 3];
      float num38 = numArray[index5 + 5] - numArray[index5 + 3];
      float num39 = numArray[index5 + 1] + numArray[index5 + 7];
      float num40 = numArray[index5 + 1] - numArray[index5 + 7];
      float num41 = num39 + num37;
      float num42 = (float) (((double) num39 - (double) num37) * 1.4142135381698608);
      float num43 = (float) (((double) num38 + (double) num40) * 1.8477590084075928);
      float num44 = 1.08239222f * num40 - num43;
      float num45 = -2.613126f * num38 + num43 - num41;
      float num46 = num42 - num45;
      float num47 = num44 + num46;
      int i = output_row + index6;
      this.m_componentBuffer[i][output_col] = sampleRangeLimit[num28 + JpegUtils.DESCALE((int) ((double) num33 + (double) num41), 3) & 1023 /*0x03FF*/];
      this.m_componentBuffer[i][output_col + 7] = sampleRangeLimit[num28 + JpegUtils.DESCALE((int) ((double) num33 - (double) num41), 3) & 1023 /*0x03FF*/];
      this.m_componentBuffer[i][output_col + 1] = sampleRangeLimit[num28 + JpegUtils.DESCALE((int) ((double) num35 + (double) num45), 3) & 1023 /*0x03FF*/];
      this.m_componentBuffer[i][output_col + 6] = sampleRangeLimit[num28 + JpegUtils.DESCALE((int) ((double) num35 - (double) num45), 3) & 1023 /*0x03FF*/];
      this.m_componentBuffer[i][output_col + 2] = sampleRangeLimit[num28 + JpegUtils.DESCALE((int) ((double) num36 + (double) num46), 3) & 1023 /*0x03FF*/];
      this.m_componentBuffer[i][output_col + 5] = sampleRangeLimit[num28 + JpegUtils.DESCALE((int) ((double) num36 - (double) num46), 3) & 1023 /*0x03FF*/];
      this.m_componentBuffer[i][output_col + 4] = sampleRangeLimit[num28 + JpegUtils.DESCALE((int) ((double) num34 + (double) num47), 3) & 1023 /*0x03FF*/];
      this.m_componentBuffer[i][output_col + 3] = sampleRangeLimit[num28 + JpegUtils.DESCALE((int) ((double) num34 - (double) num47), 3) & 1023 /*0x03FF*/];
      index5 += 8;
    }
  }

  private static float FLOAT_DEQUANTIZE(short coef, float quantval) => (float) coef * quantval;

  private void jpeg_idct_4x4(
    int component_index,
    short[] coef_block,
    int output_row,
    int output_col)
  {
    int[] numArray = new int[32 /*0x20*/];
    int index1 = 0;
    int index2 = 0;
    int[] intArray = this.m_dctTables[component_index].int_array;
    int index3 = 0;
    for (int index4 = 8; index4 > 0; --index4)
    {
      if (index4 != 4)
      {
        if (coef_block[index1 + 8] == (short) 0 && coef_block[index1 + 16 /*0x10*/] == (short) 0 && coef_block[index1 + 24] == (short) 0 && coef_block[index1 + 40] == (short) 0 && coef_block[index1 + 48 /*0x30*/] == (short) 0 && coef_block[index1 + 56] == (short) 0)
        {
          int num = InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1], intArray[index3]) << 2;
          numArray[index2] = num;
          numArray[index2 + 8] = num;
          numArray[index2 + 16 /*0x10*/] = num;
          numArray[index2 + 24] = num;
        }
        else
        {
          int num1 = InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1], intArray[index3]) << 14;
          int num2 = InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1 + 16 /*0x10*/], intArray[index3 + 16 /*0x10*/]) * 15137 + InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1 + 48 /*0x30*/], intArray[index3 + 48 /*0x30*/]) * -6270;
          int num3 = num1 + num2;
          int num4 = num1 - num2;
          int num5 = InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1 + 56], intArray[index3 + 56]);
          int num6 = InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1 + 40], intArray[index3 + 40]);
          int num7 = InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1 + 24], intArray[index3 + 24]);
          int num8 = InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1 + 8], intArray[index3 + 8]);
          int num9 = num5 * -1730 + num6 * 11893 + num7 * -17799 + num8 * 8697;
          int num10 = num5 * -4176 + num6 * -4926 + num7 * 7373 + num8 * 20995;
          numArray[index2] = JpegUtils.DESCALE(num3 + num10, 12);
          numArray[index2 + 24] = JpegUtils.DESCALE(num3 - num10, 12);
          numArray[index2 + 8] = JpegUtils.DESCALE(num4 + num9, 12);
          numArray[index2 + 16 /*0x10*/] = JpegUtils.DESCALE(num4 - num9, 12);
        }
      }
      ++index1;
      ++index3;
      ++index2;
    }
    byte[] sampleRangeLimit = this.m_cinfo.m_sample_range_limit;
    int num11 = this.m_cinfo.m_sampleRangeLimitOffset + 128 /*0x80*/;
    int index5 = 0;
    for (int index6 = 0; index6 < 4; ++index6)
    {
      int i = output_row + index6;
      if (numArray[index5 + 1] == 0 && numArray[index5 + 2] == 0 && numArray[index5 + 3] == 0 && numArray[index5 + 5] == 0 && numArray[index5 + 6] == 0 && numArray[index5 + 7] == 0)
      {
        byte num12 = sampleRangeLimit[num11 + JpegUtils.DESCALE(numArray[index5], 5) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col] = num12;
        this.m_componentBuffer[i][output_col + 1] = num12;
        this.m_componentBuffer[i][output_col + 2] = num12;
        this.m_componentBuffer[i][output_col + 3] = num12;
        index5 += 8;
      }
      else
      {
        int num13 = numArray[index5] << 14;
        int num14 = numArray[index5 + 2] * 15137 + numArray[index5 + 6] * -6270;
        int num15 = num13 + num14;
        int num16 = num13 - num14;
        int num17 = numArray[index5 + 7];
        int num18 = numArray[index5 + 5];
        int num19 = numArray[index5 + 3];
        int num20 = numArray[index5 + 1];
        int num21 = num17 * -1730 + num18 * 11893 + num19 * -17799 + num20 * 8697;
        int num22 = num17 * -4176 + num18 * -4926 + num19 * 7373 + num20 * 20995;
        this.m_componentBuffer[i][output_col] = sampleRangeLimit[num11 + JpegUtils.DESCALE(num15 + num22, 19) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 3] = sampleRangeLimit[num11 + JpegUtils.DESCALE(num15 - num22, 19) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 1] = sampleRangeLimit[num11 + JpegUtils.DESCALE(num16 + num21, 19) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 2] = sampleRangeLimit[num11 + JpegUtils.DESCALE(num16 - num21, 19) & 1023 /*0x03FF*/];
        index5 += 8;
      }
    }
  }

  private void jpeg_idct_2x2(
    int component_index,
    short[] coef_block,
    int output_row,
    int output_col)
  {
    int[] numArray = new int[16 /*0x10*/];
    int index1 = 0;
    int index2 = 0;
    int[] intArray = this.m_dctTables[component_index].int_array;
    int index3 = 0;
    for (int index4 = 8; index4 > 0; --index4)
    {
      if (index4 != 6 && index4 != 4 && index4 != 2)
      {
        if (coef_block[index1 + 8] == (short) 0 && coef_block[index1 + 24] == (short) 0 && coef_block[index1 + 40] == (short) 0 && coef_block[index1 + 56] == (short) 0)
        {
          int num = InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1], intArray[index3]) << 2;
          numArray[index2] = num;
          numArray[index2 + 8] = num;
        }
        else
        {
          int num1 = InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1], intArray[index3]) << 15;
          int num2 = InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1 + 56], intArray[index3 + 56]) * -5906 + InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1 + 40], intArray[index3 + 40]) * 6967 + InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1 + 24], intArray[index3 + 24]) * -10426 + InverseDCT.REDUCED_DEQUANTIZE(coef_block[index1 + 8], intArray[index3 + 8]) * 29692;
          numArray[index2] = JpegUtils.DESCALE(num1 + num2, 13);
          numArray[index2 + 8] = JpegUtils.DESCALE(num1 - num2, 13);
        }
      }
      ++index1;
      ++index3;
      ++index2;
    }
    int index5 = 0;
    byte[] sampleRangeLimit = this.m_cinfo.m_sample_range_limit;
    int num3 = this.m_cinfo.m_sampleRangeLimitOffset + 128 /*0x80*/;
    for (int index6 = 0; index6 < 2; ++index6)
    {
      int i = output_row + index6;
      if (numArray[index5 + 1] == 0 && numArray[index5 + 3] == 0 && numArray[index5 + 5] == 0 && numArray[index5 + 7] == 0)
      {
        byte num4 = sampleRangeLimit[num3 + JpegUtils.DESCALE(numArray[index5], 5) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col] = num4;
        this.m_componentBuffer[i][output_col + 1] = num4;
        index5 += 8;
      }
      else
      {
        int num5 = numArray[index5] << 15;
        int num6 = numArray[index5 + 7] * -5906 + numArray[index5 + 5] * 6967 + numArray[index5 + 3] * -10426 + numArray[index5 + 1] * 29692;
        this.m_componentBuffer[i][output_col] = sampleRangeLimit[num3 + JpegUtils.DESCALE(num5 + num6, 20) & 1023 /*0x03FF*/];
        this.m_componentBuffer[i][output_col + 1] = sampleRangeLimit[num3 + JpegUtils.DESCALE(num5 - num6, 20) & 1023 /*0x03FF*/];
        index5 += 8;
      }
    }
  }

  private void jpeg_idct_1x1(
    int component_index,
    short[] coef_block,
    int output_row,
    int output_col)
  {
    int[] intArray = this.m_dctTables[component_index].int_array;
    int num1 = JpegUtils.DESCALE(InverseDCT.REDUCED_DEQUANTIZE(coef_block[0], intArray[0]), 3);
    byte[] sampleRangeLimit = this.m_cinfo.m_sample_range_limit;
    int num2 = this.m_cinfo.m_sampleRangeLimitOffset + 128 /*0x80*/;
    this.m_componentBuffer[output_row][output_col] = sampleRangeLimit[num2 + num1 & 1023 /*0x03FF*/];
  }

  private static int REDUCED_DEQUANTIZE(short coef, int quantval) => (int) coef * quantval;

  private enum InverseMethod
  {
    Unknown,
    idct_1x1_method,
    idct_2x2_method,
    idct_4x4_method,
    idct_islow_method,
    idct_ifast_method,
    idct_float_method,
  }

  private class multiplier_table
  {
    public int[] int_array = new int[64 /*0x40*/];
    public float[] float_array = new float[64 /*0x40*/];
  }
}
