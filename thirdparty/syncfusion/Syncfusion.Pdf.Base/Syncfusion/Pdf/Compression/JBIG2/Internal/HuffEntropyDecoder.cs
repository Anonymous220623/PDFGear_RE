// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.HuffEntropyDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class HuffEntropyDecoder : EntropyDecoder
{
  private BitReadPermState m_bitstate;
  private HuffEntropyDecoder.savable_state m_saved = new HuffEntropyDecoder.savable_state();
  private int m_restarts_to_go;
  private DDerivedTbl[] m_dc_derived_tbls = new DDerivedTbl[4];
  private DDerivedTbl[] m_ac_derived_tbls = new DDerivedTbl[4];
  private DDerivedTbl[] m_dc_cur_tbls = new DDerivedTbl[10];
  private DDerivedTbl[] m_ac_cur_tbls = new DDerivedTbl[10];
  private bool[] m_dc_needed = new bool[10];
  private bool[] m_ac_needed = new bool[10];

  public HuffEntropyDecoder(DecompressStruct cinfo)
  {
    this.m_cinfo = cinfo;
    for (int index = 0; index < 4; ++index)
      this.m_dc_derived_tbls[index] = this.m_ac_derived_tbls[index] = (DDerivedTbl) null;
  }

  public override void start_pass()
  {
    for (int index = 0; index < this.m_cinfo.m_comps_in_scan; ++index)
    {
      ComponentInfo componentInfo = this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[index]];
      int dcTblNo = componentInfo.Dc_tbl_no;
      int acTblNo = componentInfo.Ac_tbl_no;
      this.jpeg_make_d_derived_tbl(true, dcTblNo, ref this.m_dc_derived_tbls[dcTblNo]);
      this.jpeg_make_d_derived_tbl(false, acTblNo, ref this.m_ac_derived_tbls[acTblNo]);
      this.m_saved.last_dc_val[index] = 0;
    }
    for (int index = 0; index < this.m_cinfo.m_blocks_in_MCU; ++index)
    {
      ComponentInfo componentInfo = this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[this.m_cinfo.m_MCU_membership[index]]];
      this.m_dc_cur_tbls[index] = this.m_dc_derived_tbls[componentInfo.Dc_tbl_no];
      this.m_ac_cur_tbls[index] = this.m_ac_derived_tbls[componentInfo.Ac_tbl_no];
      if (componentInfo.component_needed)
      {
        this.m_dc_needed[index] = true;
        this.m_ac_needed[index] = componentInfo.DCT_scaled_size > 1;
      }
      else
        this.m_dc_needed[index] = this.m_ac_needed[index] = false;
    }
    this.m_bitstate.bits_left = 0;
    this.m_bitstate.get_buffer = 0;
    this.m_insufficient_data = false;
    this.m_restarts_to_go = this.m_cinfo.m_restart_interval;
  }

  public override bool decode_mcu(JBLOCK[] MCU_data)
  {
    if (this.m_cinfo.m_restart_interval != 0 && this.m_restarts_to_go == 0 && !this.process_restart())
      return false;
    if (!this.m_insufficient_data)
    {
      BitReadWorkingState readWorkingState = new BitReadWorkingState();
      int get_buffer;
      int bits_left;
      this.BITREAD_LOAD_STATE(this.m_bitstate, out get_buffer, out bits_left, ref readWorkingState);
      HuffEntropyDecoder.savable_state ss = new HuffEntropyDecoder.savable_state();
      ss.Assign(this.m_saved);
      for (int index1 = 0; index1 < this.m_cinfo.m_blocks_in_MCU; ++index1)
      {
        int result;
        if (!EntropyDecoder.HUFF_DECODE(out result, ref readWorkingState, this.m_dc_cur_tbls[index1], ref get_buffer, ref bits_left))
          return false;
        if (result != 0)
        {
          if (!EntropyDecoder.CHECK_BIT_BUFFER(ref readWorkingState, result, ref get_buffer, ref bits_left))
            return false;
          result = EntropyDecoder.HUFF_EXTEND(EntropyDecoder.GET_BITS(result, get_buffer, ref bits_left), result);
        }
        if (this.m_dc_needed[index1])
        {
          int index2 = this.m_cinfo.m_MCU_membership[index1];
          result += ss.last_dc_val[index2];
          ss.last_dc_val[index2] = result;
          MCU_data[index1][0] = (short) result;
        }
        int index3;
        int num1;
        if (this.m_ac_needed[index1])
        {
          for (int index4 = 1; index4 < 64 /*0x40*/; index4 = index3 + 1)
          {
            if (!EntropyDecoder.HUFF_DECODE(out result, ref readWorkingState, this.m_ac_cur_tbls[index1], ref get_buffer, ref bits_left))
              return false;
            int num2 = result >> 4;
            result &= 15;
            if (result != 0)
            {
              index3 = index4 + num2;
              if (!EntropyDecoder.CHECK_BIT_BUFFER(ref readWorkingState, result, ref get_buffer, ref bits_left))
                return false;
              result = EntropyDecoder.HUFF_EXTEND(EntropyDecoder.GET_BITS(result, get_buffer, ref bits_left), result);
              MCU_data[index1][JpegUtils.jpeg_natural_order[index3]] = (short) result;
            }
            else if (num2 == 15)
              index3 = index4 + 15;
            else
              break;
          }
        }
        else
        {
          for (int index5 = 1; index5 < 64 /*0x40*/; index5 = num1 + 1)
          {
            if (!EntropyDecoder.HUFF_DECODE(out result, ref readWorkingState, this.m_ac_cur_tbls[index1], ref get_buffer, ref bits_left))
              return false;
            int num3 = result >> 4;
            result &= 15;
            if (result != 0)
            {
              num1 = index5 + num3;
              if (!EntropyDecoder.CHECK_BIT_BUFFER(ref readWorkingState, result, ref get_buffer, ref bits_left))
                return false;
              EntropyDecoder.DROP_BITS(result, ref bits_left);
            }
            else if (num3 == 15)
              num1 = index5 + 15;
            else
              break;
          }
        }
      }
      EntropyDecoder.BITREAD_SAVE_STATE(ref this.m_bitstate, get_buffer, bits_left);
      this.m_saved.Assign(ss);
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
    this.m_restarts_to_go = this.m_cinfo.m_restart_interval;
    if (this.m_cinfo.m_unread_marker == 0)
      this.m_insufficient_data = false;
    return true;
  }

  private class savable_state
  {
    public int[] last_dc_val = new int[4];

    public void Assign(HuffEntropyDecoder.savable_state ss)
    {
      Buffer.BlockCopy((Array) ss.last_dc_val, 0, (Array) this.last_dc_val, 0, this.last_dc_val.Length * 4);
    }
  }
}
