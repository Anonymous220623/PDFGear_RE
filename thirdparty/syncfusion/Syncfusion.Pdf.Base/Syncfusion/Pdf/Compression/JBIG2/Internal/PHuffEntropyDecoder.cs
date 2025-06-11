// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.PHuffEntropyDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class PHuffEntropyDecoder : EntropyDecoder
{
  private PHuffEntropyDecoder.MCUDecoder m_decoder;
  private BitReadPermState m_bitstate;
  private PHuffEntropyDecoder.savable_state m_saved = new PHuffEntropyDecoder.savable_state();
  private int m_restarts_to_go;
  private DDerivedTbl[] m_derived_tbls = new DDerivedTbl[4];
  private DDerivedTbl m_ac_derived_tbl;

  public PHuffEntropyDecoder(DecompressStruct cinfo)
  {
    this.m_cinfo = cinfo;
    for (int index = 0; index < 4; ++index)
      this.m_derived_tbls[index] = (DDerivedTbl) null;
    cinfo.m_coef_bits = new int[cinfo.m_num_components][];
    for (int index = 0; index < cinfo.m_num_components; ++index)
      cinfo.m_coef_bits[index] = new int[64 /*0x40*/];
    for (int index1 = 0; index1 < cinfo.m_num_components; ++index1)
    {
      for (int index2 = 0; index2 < 64 /*0x40*/; ++index2)
        cinfo.m_coef_bits[index1][index2] = -1;
    }
  }

  public override void start_pass()
  {
    bool flag = this.m_cinfo.m_Ss == 0;
    if (flag)
    {
      if (this.m_cinfo.m_Se == 0)
        ;
    }
    else
    {
      if (this.m_cinfo.m_Ss <= this.m_cinfo.m_Se)
      {
        int se = this.m_cinfo.m_Se;
      }
      int compsInScan = this.m_cinfo.m_comps_in_scan;
    }
    if (this.m_cinfo.m_Ah != 0)
    {
      int al = this.m_cinfo.m_Al;
      int num = this.m_cinfo.m_Ah - 1;
    }
    int al1 = this.m_cinfo.m_Al;
    for (int index = 0; index < this.m_cinfo.m_comps_in_scan; ++index)
    {
      int componentIndex = this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[index]].Component_index;
      for (int ss = this.m_cinfo.m_Ss; ss <= this.m_cinfo.m_Se; ++ss)
      {
        if (this.m_cinfo.m_coef_bits[componentIndex][ss] < 0)
          ;
        this.m_cinfo.m_coef_bits[componentIndex][ss] = this.m_cinfo.m_Al;
      }
    }
    this.m_decoder = this.m_cinfo.m_Ah != 0 ? (!flag ? PHuffEntropyDecoder.MCUDecoder.mcu_AC_refine_decoder : PHuffEntropyDecoder.MCUDecoder.mcu_DC_refine_decoder) : (!flag ? PHuffEntropyDecoder.MCUDecoder.mcu_AC_first_decoder : PHuffEntropyDecoder.MCUDecoder.mcu_DC_first_decoder);
    for (int index = 0; index < this.m_cinfo.m_comps_in_scan; ++index)
    {
      ComponentInfo componentInfo = this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[index]];
      if (flag)
      {
        if (this.m_cinfo.m_Ah == 0)
          this.jpeg_make_d_derived_tbl(true, componentInfo.Dc_tbl_no, ref this.m_derived_tbls[componentInfo.Dc_tbl_no]);
      }
      else
      {
        this.jpeg_make_d_derived_tbl(false, componentInfo.Ac_tbl_no, ref this.m_derived_tbls[componentInfo.Ac_tbl_no]);
        this.m_ac_derived_tbl = this.m_derived_tbls[componentInfo.Ac_tbl_no];
      }
      this.m_saved.last_dc_val[index] = 0;
    }
    this.m_bitstate.bits_left = 0;
    this.m_bitstate.get_buffer = 0;
    this.m_insufficient_data = false;
    this.m_saved.EOBRUN = 0;
    this.m_restarts_to_go = this.m_cinfo.m_restart_interval;
  }

  public override bool decode_mcu(JBLOCK[] MCU_data)
  {
    switch (this.m_decoder)
    {
      case PHuffEntropyDecoder.MCUDecoder.mcu_DC_first_decoder:
        return this.decode_mcu_DC_first(MCU_data);
      case PHuffEntropyDecoder.MCUDecoder.mcu_AC_first_decoder:
        return this.decode_mcu_AC_first(MCU_data);
      case PHuffEntropyDecoder.MCUDecoder.mcu_DC_refine_decoder:
        return this.decode_mcu_DC_refine(MCU_data);
      case PHuffEntropyDecoder.MCUDecoder.mcu_AC_refine_decoder:
        return this.decode_mcu_AC_refine(MCU_data);
      default:
        return false;
    }
  }

  private bool decode_mcu_DC_first(JBLOCK[] MCU_data)
  {
    if (this.m_cinfo.m_restart_interval != 0 && this.m_restarts_to_go == 0 && !this.process_restart())
      return false;
    if (!this.m_insufficient_data)
    {
      BitReadWorkingState readWorkingState = new BitReadWorkingState();
      int get_buffer;
      int bits_left;
      this.BITREAD_LOAD_STATE(this.m_bitstate, out get_buffer, out bits_left, ref readWorkingState);
      PHuffEntropyDecoder.savable_state ss = new PHuffEntropyDecoder.savable_state();
      ss.Assign(this.m_saved);
      for (int index1 = 0; index1 < this.m_cinfo.m_blocks_in_MCU; ++index1)
      {
        int index2 = this.m_cinfo.m_MCU_membership[index1];
        int result;
        if (!EntropyDecoder.HUFF_DECODE(out result, ref readWorkingState, this.m_derived_tbls[this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[index2]].Dc_tbl_no], ref get_buffer, ref bits_left))
          return false;
        if (result != 0)
        {
          if (!EntropyDecoder.CHECK_BIT_BUFFER(ref readWorkingState, result, ref get_buffer, ref bits_left))
            return false;
          result = EntropyDecoder.HUFF_EXTEND(EntropyDecoder.GET_BITS(result, get_buffer, ref bits_left), result);
        }
        result += ss.last_dc_val[index2];
        ss.last_dc_val[index2] = result;
        MCU_data[index1][0] = (short) (result << this.m_cinfo.m_Al);
      }
      EntropyDecoder.BITREAD_SAVE_STATE(ref this.m_bitstate, get_buffer, bits_left);
      this.m_saved.Assign(ss);
    }
    --this.m_restarts_to_go;
    return true;
  }

  private bool decode_mcu_AC_first(JBLOCK[] MCU_data)
  {
    if (this.m_cinfo.m_restart_interval != 0 && this.m_restarts_to_go == 0 && !this.process_restart())
      return false;
    if (!this.m_insufficient_data)
    {
      int num1 = this.m_saved.EOBRUN;
      if (num1 > 0)
      {
        --num1;
      }
      else
      {
        BitReadWorkingState readWorkingState = new BitReadWorkingState();
        int get_buffer;
        int bits_left;
        this.BITREAD_LOAD_STATE(this.m_bitstate, out get_buffer, out bits_left, ref readWorkingState);
        int index1;
        for (int index2 = this.m_cinfo.m_Ss; index2 <= this.m_cinfo.m_Se; index2 = index1 + 1)
        {
          int result;
          if (!EntropyDecoder.HUFF_DECODE(out result, ref readWorkingState, this.m_ac_derived_tbl, ref get_buffer, ref bits_left))
            return false;
          int nbits = result >> 4;
          result &= 15;
          if (result != 0)
          {
            index1 = index2 + nbits;
            if (!EntropyDecoder.CHECK_BIT_BUFFER(ref readWorkingState, result, ref get_buffer, ref bits_left))
              return false;
            result = EntropyDecoder.HUFF_EXTEND(EntropyDecoder.GET_BITS(result, get_buffer, ref bits_left), result);
            MCU_data[0][JpegUtils.jpeg_natural_order[index1]] = (short) (result << this.m_cinfo.m_Al);
          }
          else if (nbits == 15)
          {
            index1 = index2 + 15;
          }
          else
          {
            int num2 = 1 << nbits;
            if (nbits != 0)
            {
              if (!EntropyDecoder.CHECK_BIT_BUFFER(ref readWorkingState, nbits, ref get_buffer, ref bits_left))
                return false;
              int bits = EntropyDecoder.GET_BITS(nbits, get_buffer, ref bits_left);
              num2 += bits;
            }
            num1 = num2 - 1;
            break;
          }
        }
        EntropyDecoder.BITREAD_SAVE_STATE(ref this.m_bitstate, get_buffer, bits_left);
      }
      this.m_saved.EOBRUN = num1;
    }
    --this.m_restarts_to_go;
    return true;
  }

  private bool decode_mcu_DC_refine(JBLOCK[] MCU_data)
  {
    if (this.m_cinfo.m_restart_interval != 0 && this.m_restarts_to_go == 0 && !this.process_restart())
      return false;
    BitReadWorkingState readWorkingState = new BitReadWorkingState();
    int get_buffer;
    int bits_left;
    this.BITREAD_LOAD_STATE(this.m_bitstate, out get_buffer, out bits_left, ref readWorkingState);
    for (int index = 0; index < this.m_cinfo.m_blocks_in_MCU; ++index)
    {
      if (!EntropyDecoder.CHECK_BIT_BUFFER(ref readWorkingState, 1, ref get_buffer, ref bits_left))
        return false;
      if (EntropyDecoder.GET_BITS(1, get_buffer, ref bits_left) != 0)
        MCU_data[index][0] |= (short) (1 << this.m_cinfo.m_Al);
    }
    EntropyDecoder.BITREAD_SAVE_STATE(ref this.m_bitstate, get_buffer, bits_left);
    --this.m_restarts_to_go;
    return true;
  }

  private bool decode_mcu_AC_refine(JBLOCK[] MCU_data)
  {
    int num1 = 1 << this.m_cinfo.m_Al;
    int num2 = -1 << this.m_cinfo.m_Al;
    if (this.m_cinfo.m_restart_interval != 0 && this.m_restarts_to_go == 0 && !this.process_restart())
      return false;
    if (!this.m_insufficient_data)
    {
      BitReadWorkingState readWorkingState = new BitReadWorkingState();
      int get_buffer;
      int bits_left;
      this.BITREAD_LOAD_STATE(this.m_bitstate, out get_buffer, out bits_left, ref readWorkingState);
      int num3 = this.m_saved.EOBRUN;
      int num_newnz = 0;
      int[] newnz_pos = new int[64 /*0x40*/];
      int ss = this.m_cinfo.m_Ss;
      if (num3 == 0)
      {
        for (; ss <= this.m_cinfo.m_Se; ++ss)
        {
          int result;
          if (!EntropyDecoder.HUFF_DECODE(out result, ref readWorkingState, this.m_ac_derived_tbl, ref get_buffer, ref bits_left))
          {
            PHuffEntropyDecoder.undo_decode_mcu_AC_refine(MCU_data, newnz_pos, num_newnz);
            return false;
          }
          int nbits = result >> 4;
          result &= 15;
          if (result != 0)
          {
            if (!EntropyDecoder.CHECK_BIT_BUFFER(ref readWorkingState, 1, ref get_buffer, ref bits_left))
            {
              PHuffEntropyDecoder.undo_decode_mcu_AC_refine(MCU_data, newnz_pos, num_newnz);
              return false;
            }
            result = EntropyDecoder.GET_BITS(1, get_buffer, ref bits_left) == 0 ? num2 : num1;
          }
          else if (nbits != 15)
          {
            num3 = 1 << nbits;
            if (nbits != 0)
            {
              if (!EntropyDecoder.CHECK_BIT_BUFFER(ref readWorkingState, nbits, ref get_buffer, ref bits_left))
              {
                PHuffEntropyDecoder.undo_decode_mcu_AC_refine(MCU_data, newnz_pos, num_newnz);
                return false;
              }
              int bits = EntropyDecoder.GET_BITS(nbits, get_buffer, ref bits_left);
              num3 += bits;
              break;
            }
            break;
          }
          do
          {
            int index = JpegUtils.jpeg_natural_order[ss];
            short num4 = MCU_data[0][index];
            if (num4 != (short) 0)
            {
              if (!EntropyDecoder.CHECK_BIT_BUFFER(ref readWorkingState, 1, ref get_buffer, ref bits_left))
              {
                PHuffEntropyDecoder.undo_decode_mcu_AC_refine(MCU_data, newnz_pos, num_newnz);
                return false;
              }
              if (EntropyDecoder.GET_BITS(1, get_buffer, ref bits_left) != 0 && ((int) num4 & num1) == 0)
              {
                if (num4 >= (short) 0)
                  MCU_data[0][index] += (short) num1;
                else
                  MCU_data[0][index] += (short) num2;
              }
            }
            else if (--nbits < 0)
              break;
            ++ss;
          }
          while (ss <= this.m_cinfo.m_Se);
          if (result != 0)
          {
            int index = JpegUtils.jpeg_natural_order[ss];
            MCU_data[0][index] = (short) result;
            newnz_pos[num_newnz++] = index;
          }
        }
      }
      if (num3 > 0)
      {
        for (; ss <= this.m_cinfo.m_Se; ++ss)
        {
          int index = JpegUtils.jpeg_natural_order[ss];
          short num5 = MCU_data[0][index];
          if (num5 != (short) 0)
          {
            if (!EntropyDecoder.CHECK_BIT_BUFFER(ref readWorkingState, 1, ref get_buffer, ref bits_left))
            {
              PHuffEntropyDecoder.undo_decode_mcu_AC_refine(MCU_data, newnz_pos, num_newnz);
              return false;
            }
            if (EntropyDecoder.GET_BITS(1, get_buffer, ref bits_left) != 0 && ((int) num5 & num1) == 0)
            {
              if (num5 >= (short) 0)
                MCU_data[0][index] += (short) num1;
              else
                MCU_data[0][index] += (short) num2;
            }
          }
        }
        --num3;
      }
      EntropyDecoder.BITREAD_SAVE_STATE(ref this.m_bitstate, get_buffer, bits_left);
      this.m_saved.EOBRUN = num3;
    }
    --this.m_restarts_to_go;
    return true;
  }

  private bool process_restart()
  {
    this.m_cinfo.m_marker.SkipBytes(this.m_bitstate.bits_left / 8);
    this.m_bitstate.bits_left = 0;
    if (!this.m_cinfo.m_marker.read_restart_marker())
      return false;
    for (int index = 0; index < this.m_cinfo.m_comps_in_scan; ++index)
      this.m_saved.last_dc_val[index] = 0;
    this.m_saved.EOBRUN = 0;
    this.m_restarts_to_go = this.m_cinfo.m_restart_interval;
    if (this.m_cinfo.m_unread_marker == 0)
      this.m_insufficient_data = false;
    return true;
  }

  private static void undo_decode_mcu_AC_refine(JBLOCK[] block, int[] newnz_pos, int num_newnz)
  {
    while (num_newnz > 0)
      block[0][newnz_pos[--num_newnz]] = (short) 0;
  }

  private class savable_state
  {
    public int EOBRUN;
    public int[] last_dc_val = new int[4];

    public void Assign(PHuffEntropyDecoder.savable_state ss)
    {
      this.EOBRUN = ss.EOBRUN;
      Buffer.BlockCopy((Array) ss.last_dc_val, 0, (Array) this.last_dc_val, 0, this.last_dc_val.Length * 4);
    }
  }

  private enum MCUDecoder
  {
    mcu_DC_first_decoder,
    mcu_AC_first_decoder,
    mcu_DC_refine_decoder,
    mcu_AC_refine_decoder,
  }
}
