// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.EntropyDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal abstract class EntropyDecoder
{
  protected const int BIT_BUF_SIZE = 32 /*0x20*/;
  protected const int MIN_GET_BITS = 25;
  private static readonly int[] extend_test = new int[16 /*0x10*/]
  {
    0,
    1,
    2,
    4,
    8,
    16 /*0x10*/,
    32 /*0x20*/,
    64 /*0x40*/,
    128 /*0x80*/,
    256 /*0x0100*/,
    512 /*0x0200*/,
    1024 /*0x0400*/,
    2048 /*0x0800*/,
    4096 /*0x1000*/,
    8192 /*0x2000*/,
    16384 /*0x4000*/
  };
  private static readonly int[] extend_offset = new int[16 /*0x10*/]
  {
    0,
    -1,
    -3,
    -7,
    -15,
    -31,
    -63,
    -127,
    -255,
    -511,
    -1023,
    -2047,
    -4095,
    -8191,
    -16383,
    -32767
  };
  protected DecompressStruct m_cinfo;
  protected bool m_insufficient_data;

  public abstract void start_pass();

  public abstract bool decode_mcu(JBLOCK[] MCU_data);

  protected static int HUFF_EXTEND(int x, int s)
  {
    return x >= EntropyDecoder.extend_test[s] ? x : x + EntropyDecoder.extend_offset[s];
  }

  protected void BITREAD_LOAD_STATE(
    BitReadPermState bitstate,
    out int get_buffer,
    out int bits_left,
    ref BitReadWorkingState br_state)
  {
    br_state.cinfo = this.m_cinfo;
    get_buffer = bitstate.get_buffer;
    bits_left = bitstate.bits_left;
  }

  protected static void BITREAD_SAVE_STATE(
    ref BitReadPermState bitstate,
    int get_buffer,
    int bits_left)
  {
    bitstate.get_buffer = get_buffer;
    bitstate.bits_left = bits_left;
  }

  protected void jpeg_make_d_derived_tbl(bool isDC, int tblno, ref DDerivedTbl dtbl)
  {
    JHUFF_TBL jhuffTbl = isDC ? this.m_cinfo.m_dc_huff_tbl_ptrs[tblno] : this.m_cinfo.m_ac_huff_tbl_ptrs[tblno];
    if (dtbl == null)
      dtbl = new DDerivedTbl();
    dtbl.pub = jhuffTbl;
    int index1 = 0;
    char[] chArray = new char[257];
    for (int index2 = 1; index2 <= 16 /*0x10*/; ++index2)
    {
      int bit = (int) jhuffTbl.Bits[index2];
      while (bit-- != 0)
        chArray[index1++] = (char) index2;
    }
    chArray[index1] = char.MinValue;
    int num1 = index1;
    int num2 = 0;
    int num3 = (int) chArray[0];
    int[] numArray = new int[257];
    int index3 = 0;
    while (chArray[index3] != char.MinValue)
    {
      while ((int) chArray[index3] == num3)
      {
        numArray[index3++] = num2;
        ++num2;
      }
      num2 <<= 1;
      ++num3;
    }
    int index4 = 0;
    for (int index5 = 1; index5 <= 16 /*0x10*/; ++index5)
    {
      if (jhuffTbl.Bits[index5] != (byte) 0)
      {
        dtbl.valoffset[index5] = index4 - numArray[index4];
        index4 += (int) jhuffTbl.Bits[index5];
        dtbl.maxcode[index5] = numArray[index4 - 1];
      }
      else
        dtbl.maxcode[index5] = -1;
    }
    dtbl.maxcode[17] = 1048575 /*0x0FFFFF*/;
    Array.Clear((Array) dtbl.look_nbits, 0, dtbl.look_nbits.Length);
    int index6 = 0;
    for (int index7 = 1; index7 <= 8; ++index7)
    {
      int num4 = 1;
      while (num4 <= (int) jhuffTbl.Bits[index7])
      {
        int index8 = numArray[index6] << 8 - index7;
        for (int index9 = 1 << 8 - index7; index9 > 0; --index9)
        {
          dtbl.look_nbits[index8] = index7;
          dtbl.look_sym[index8] = jhuffTbl.Huffval[index6];
          ++index8;
        }
        ++num4;
        ++index6;
      }
    }
    if (!isDC)
      return;
    for (int index10 = 0; index10 < num1; ++index10)
    {
      int num5 = (int) jhuffTbl.Huffval[index10];
    }
  }

  protected static bool CHECK_BIT_BUFFER(
    ref BitReadWorkingState state,
    int nbits,
    ref int get_buffer,
    ref int bits_left)
  {
    if (bits_left < nbits)
    {
      if (!EntropyDecoder.jpeg_fill_bit_buffer(ref state, get_buffer, bits_left, nbits))
        return false;
      get_buffer = state.get_buffer;
      bits_left = state.bits_left;
    }
    return true;
  }

  protected static int GET_BITS(int nbits, int get_buffer, ref int bits_left)
  {
    return get_buffer >> (bits_left -= nbits) & (1 << nbits) - 1;
  }

  protected static int PEEK_BITS(int nbits, int get_buffer, int bits_left)
  {
    return get_buffer >> bits_left - nbits & (1 << nbits) - 1;
  }

  protected static void DROP_BITS(int nbits, ref int bits_left) => bits_left -= nbits;

  protected static bool jpeg_fill_bit_buffer(
    ref BitReadWorkingState state,
    int get_buffer,
    int bits_left,
    int nbits)
  {
    bool flag = false;
    if (state.cinfo.m_unread_marker == 0)
    {
      for (; bits_left < 25; bits_left += 8)
      {
        int V;
        state.cinfo.m_src.GetByte(out V);
        if (V == (int) byte.MaxValue)
        {
          do
          {
            state.cinfo.m_src.GetByte(out V);
          }
          while (V == (int) byte.MaxValue);
          if (V == 0)
          {
            V = (int) byte.MaxValue;
          }
          else
          {
            state.cinfo.m_unread_marker = V;
            flag = true;
            break;
          }
        }
        get_buffer = get_buffer << 8 | V;
      }
    }
    else
      flag = true;
    if (flag && nbits > bits_left)
    {
      if (!state.cinfo.m_entropy.m_insufficient_data)
        state.cinfo.m_entropy.m_insufficient_data = true;
      get_buffer <<= 25 - bits_left;
      bits_left = 25;
    }
    state.get_buffer = get_buffer;
    state.bits_left = bits_left;
    return true;
  }

  protected static bool HUFF_DECODE(
    out int result,
    ref BitReadWorkingState state,
    DDerivedTbl htbl,
    ref int get_buffer,
    ref int bits_left)
  {
    int min_bits = 0;
    bool flag = false;
    if (bits_left < 8)
    {
      if (!EntropyDecoder.jpeg_fill_bit_buffer(ref state, get_buffer, bits_left, 0))
      {
        result = -1;
        return false;
      }
      get_buffer = state.get_buffer;
      bits_left = state.bits_left;
      if (bits_left < 8)
      {
        min_bits = 1;
        flag = true;
      }
    }
    if (!flag)
    {
      int index = EntropyDecoder.PEEK_BITS(8, get_buffer, bits_left);
      int lookNbit;
      if ((lookNbit = htbl.look_nbits[index]) != 0)
      {
        EntropyDecoder.DROP_BITS(lookNbit, ref bits_left);
        result = (int) htbl.look_sym[index];
        return true;
      }
      min_bits = 9;
    }
    result = EntropyDecoder.jpeg_huff_decode(ref state, get_buffer, bits_left, htbl, min_bits);
    if (result < 0)
      return false;
    get_buffer = state.get_buffer;
    bits_left = state.bits_left;
    return true;
  }

  protected static int jpeg_huff_decode(
    ref BitReadWorkingState state,
    int get_buffer,
    int bits_left,
    DDerivedTbl htbl,
    int min_bits)
  {
    int nbits = min_bits;
    if (!EntropyDecoder.CHECK_BIT_BUFFER(ref state, nbits, ref get_buffer, ref bits_left))
      return -1;
    int num1;
    for (num1 = EntropyDecoder.GET_BITS(nbits, get_buffer, ref bits_left); num1 > htbl.maxcode[nbits]; ++nbits)
    {
      int num2 = num1 << 1;
      if (!EntropyDecoder.CHECK_BIT_BUFFER(ref state, 1, ref get_buffer, ref bits_left))
        return -1;
      num1 = num2 | EntropyDecoder.GET_BITS(1, get_buffer, ref bits_left);
    }
    state.get_buffer = get_buffer;
    state.bits_left = bits_left;
    return nbits > 16 /*0x10*/ ? 0 : (int) htbl.pub.Huffval[num1 + htbl.valoffset[nbits]];
  }
}
