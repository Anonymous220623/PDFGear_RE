// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.MarkerReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class MarkerReader
{
  private const int APP0_DATA_LEN = 14;
  private const int APP14_DATA_LEN = 12;
  private const int APPN_DATA_LEN = 14;
  private DecompressStruct m_cinfo;
  private DecompressStruct.jpeg_marker_parser_method m_process_COM;
  private DecompressStruct.jpeg_marker_parser_method[] m_process_APPn = new DecompressStruct.jpeg_marker_parser_method[16 /*0x10*/];
  private int m_length_limit_COM;
  private int[] m_length_limit_APPn = new int[16 /*0x10*/];
  private bool m_saw_SOI;
  private bool m_saw_SOF;
  private int m_next_restart_num;
  private int m_discarded_bytes;
  private MarkerStruct m_cur_marker;
  private int m_bytes_read;

  public MarkerReader(DecompressStruct cinfo)
  {
    this.m_cinfo = cinfo;
    this.m_process_COM = new DecompressStruct.jpeg_marker_parser_method(MarkerReader.skip_variable);
    for (int index = 0; index < 16 /*0x10*/; ++index)
    {
      this.m_process_APPn[index] = new DecompressStruct.jpeg_marker_parser_method(MarkerReader.skip_variable);
      this.m_length_limit_APPn[index] = 0;
    }
    this.m_process_APPn[0] = new DecompressStruct.jpeg_marker_parser_method(MarkerReader.get_interesting_appn);
    this.m_process_APPn[14] = new DecompressStruct.jpeg_marker_parser_method(MarkerReader.get_interesting_appn);
    this.reset_marker_reader();
  }

  public void reset_marker_reader()
  {
    this.m_cinfo.Comp_info = (ComponentInfo[]) null;
    this.m_cinfo.m_input_scan_number = 0;
    this.m_cinfo.m_unread_marker = 0;
    this.m_saw_SOI = false;
    this.m_saw_SOF = false;
    this.m_discarded_bytes = 0;
    this.m_cur_marker = (MarkerStruct) null;
  }

  public ReadResult read_markers()
  {
    while (true)
    {
      if (this.m_cinfo.m_unread_marker == 0)
      {
        if (!this.m_cinfo.m_marker.m_saw_SOI)
        {
          if (!this.first_marker())
            break;
        }
        else if (!this.next_marker())
          goto label_5;
      }
      switch ((JPEG_MARKER) this.m_cinfo.m_unread_marker)
      {
        case JPEG_MARKER.SOF0:
        case JPEG_MARKER.SOF1:
          if (this.get_sof(false))
            break;
          goto label_10;
        case JPEG_MARKER.SOF2:
          if (this.get_sof(true))
            break;
          goto label_12;
        case JPEG_MARKER.DHT:
          if (this.get_dht())
            break;
          goto label_20;
        case JPEG_MARKER.DAC:
          if (MarkerReader.skip_variable(this.m_cinfo))
            break;
          goto label_18;
        case JPEG_MARKER.SOI:
          if (this.get_soi())
            break;
          goto label_8;
        case JPEG_MARKER.EOI:
          goto label_16;
        case JPEG_MARKER.SOS:
          goto label_13;
        case JPEG_MARKER.DQT:
          if (this.get_dqt())
            break;
          goto label_22;
        case JPEG_MARKER.DNL:
          if (MarkerReader.skip_variable(this.m_cinfo))
            break;
          goto label_30;
        case JPEG_MARKER.DRI:
          if (this.get_dri())
            break;
          goto label_24;
        case JPEG_MARKER.APP0:
        case JPEG_MARKER.APP1:
        case JPEG_MARKER.APP2:
        case JPEG_MARKER.APP3:
        case JPEG_MARKER.APP4:
        case JPEG_MARKER.APP5:
        case JPEG_MARKER.APP6:
        case JPEG_MARKER.APP7:
        case JPEG_MARKER.APP8:
        case JPEG_MARKER.APP9:
        case JPEG_MARKER.APP10:
        case JPEG_MARKER.APP11:
        case JPEG_MARKER.APP12:
        case JPEG_MARKER.APP13:
        case JPEG_MARKER.APP14:
        case JPEG_MARKER.APP15:
          if (this.m_cinfo.m_marker.m_process_APPn[this.m_cinfo.m_unread_marker - 224 /*0xE0*/](this.m_cinfo))
            break;
          goto label_26;
        case JPEG_MARKER.COM:
          if (this.m_cinfo.m_marker.m_process_COM(this.m_cinfo))
            break;
          goto label_28;
      }
      this.m_cinfo.m_unread_marker = 0;
    }
    return ReadResult.JPEG_SUSPENDED;
label_5:
    return ReadResult.JPEG_SUSPENDED;
label_8:
    return ReadResult.JPEG_SUSPENDED;
label_10:
    return ReadResult.JPEG_SUSPENDED;
label_12:
    return ReadResult.JPEG_SUSPENDED;
label_13:
    if (!this.get_sos())
      return ReadResult.JPEG_SUSPENDED;
    this.m_cinfo.m_unread_marker = 0;
    return ReadResult.JPEG_REACHED_SOS;
label_16:
    this.m_cinfo.m_unread_marker = 0;
    return ReadResult.JPEG_REACHED_EOI;
label_18:
    return ReadResult.JPEG_SUSPENDED;
label_20:
    return ReadResult.JPEG_SUSPENDED;
label_22:
    return ReadResult.JPEG_SUSPENDED;
label_24:
    return ReadResult.JPEG_SUSPENDED;
label_26:
    return ReadResult.JPEG_SUSPENDED;
label_28:
    return ReadResult.JPEG_SUSPENDED;
label_30:
    return ReadResult.JPEG_SUSPENDED;
  }

  public bool read_restart_marker()
  {
    if (this.m_cinfo.m_unread_marker == 0 && !this.next_marker())
      return false;
    if (this.m_cinfo.m_unread_marker == 208 /*0xD0*/ + this.m_cinfo.m_marker.m_next_restart_num)
      this.m_cinfo.m_unread_marker = 0;
    else if (!this.m_cinfo.m_src.resync_to_restart(this.m_cinfo, this.m_cinfo.m_marker.m_next_restart_num))
      return false;
    this.m_cinfo.m_marker.m_next_restart_num = this.m_cinfo.m_marker.m_next_restart_num + 1 & 7;
    return true;
  }

  public bool next_marker()
  {
label_0:
    int V;
    if (!this.m_cinfo.m_src.GetByte(out V))
      return false;
    while (V != (int) byte.MaxValue)
    {
      ++this.m_cinfo.m_marker.m_discarded_bytes;
      if (!this.m_cinfo.m_src.GetByte(out V))
        return false;
    }
    while (this.m_cinfo.m_src.GetByte(out V))
    {
      switch (V)
      {
        case 0:
          this.m_cinfo.m_marker.m_discarded_bytes += 2;
          goto label_0;
        case (int) byte.MaxValue:
          continue;
        default:
          if (this.m_cinfo.m_marker.m_discarded_bytes != 0)
            this.m_cinfo.m_marker.m_discarded_bytes = 0;
          this.m_cinfo.m_unread_marker = V;
          return true;
      }
    }
    return false;
  }

  public void jpeg_set_marker_processor(
    int marker_code,
    DecompressStruct.jpeg_marker_parser_method routine)
  {
    if (marker_code == 254)
    {
      this.m_process_COM = routine;
    }
    else
    {
      if (marker_code < 224 /*0xE0*/ || marker_code > 239)
        return;
      this.m_process_APPn[marker_code - 224 /*0xE0*/] = routine;
    }
  }

  public void jpeg_save_markers(int marker_code, int length_limit)
  {
    DecompressStruct.jpeg_marker_parser_method markerParserMethod;
    if (length_limit != 0)
    {
      markerParserMethod = new DecompressStruct.jpeg_marker_parser_method(MarkerReader.save_marker);
      if (marker_code == 224 /*0xE0*/ && length_limit < 14)
        length_limit = 14;
      else if (marker_code == 238 && length_limit < 12)
        length_limit = 12;
    }
    else
    {
      markerParserMethod = new DecompressStruct.jpeg_marker_parser_method(MarkerReader.skip_variable);
      if (marker_code == 224 /*0xE0*/ || marker_code == 238)
        markerParserMethod = new DecompressStruct.jpeg_marker_parser_method(MarkerReader.get_interesting_appn);
    }
    if (marker_code == 254)
    {
      this.m_process_COM = markerParserMethod;
      this.m_length_limit_COM = length_limit;
    }
    else
    {
      if (marker_code < 224 /*0xE0*/ || marker_code > 239)
        return;
      this.m_process_APPn[marker_code - 224 /*0xE0*/] = markerParserMethod;
      this.m_length_limit_APPn[marker_code - 224 /*0xE0*/] = length_limit;
    }
  }

  public bool SawSOI() => this.m_saw_SOI;

  public bool SawSOF() => this.m_saw_SOF;

  public int NextRestartNumber() => this.m_next_restart_num;

  public int DiscardedByteCount() => this.m_discarded_bytes;

  public void SkipBytes(int count) => this.m_discarded_bytes += count;

  private static bool save_marker(DecompressStruct cinfo)
  {
    MarkerStruct markerStruct = cinfo.m_marker.m_cur_marker;
    int V = 0;
    int num1 = 0;
    byte[] numArray1;
    int num2;
    int datalen;
    if (markerStruct == null)
    {
      if (!cinfo.m_src.GetTwoBytes(out V))
        return false;
      V -= 2;
      if (V >= 0)
      {
        int lengthLimit = cinfo.m_unread_marker != 254 ? cinfo.m_marker.m_length_limit_APPn[cinfo.m_unread_marker - 224 /*0xE0*/] : cinfo.m_marker.m_length_limit_COM;
        if (V < lengthLimit)
          lengthLimit = V;
        markerStruct = new MarkerStruct((byte) cinfo.m_unread_marker, V, lengthLimit);
        numArray1 = markerStruct.Data;
        cinfo.m_marker.m_cur_marker = markerStruct;
        cinfo.m_marker.m_bytes_read = 0;
        num2 = 0;
        datalen = lengthLimit;
      }
      else
      {
        num2 = datalen = 0;
        numArray1 = (byte[]) null;
      }
    }
    else
    {
      num2 = cinfo.m_marker.m_bytes_read;
      datalen = markerStruct.Data.Length;
      numArray1 = markerStruct.Data;
      num1 = num2;
    }
    byte[] numArray2 = (byte[]) null;
    if (datalen != 0)
      numArray2 = new byte[numArray1.Length];
    while (num2 < datalen)
    {
      cinfo.m_marker.m_bytes_read = num2;
      if (!cinfo.m_src.MakeByteAvailable())
        return false;
      int bytes = cinfo.m_src.GetBytes(numArray2, datalen - num2);
      Buffer.BlockCopy((Array) numArray2, 0, (Array) numArray1, num1, bytes);
      num2 += bytes;
      num1 += bytes;
    }
    if (markerStruct != null)
    {
      cinfo.m_marker_list.Add(markerStruct);
      numArray1 = markerStruct.Data;
      num1 = 0;
      V = markerStruct.OriginalLength - datalen;
    }
    cinfo.m_marker.m_cur_marker = (MarkerStruct) null;
    JPEG_MARKER unreadMarker = (JPEG_MARKER) cinfo.m_unread_marker;
    if (datalen != 0 && (unreadMarker == JPEG_MARKER.APP0 || unreadMarker == JPEG_MARKER.APP14))
    {
      numArray2 = new byte[numArray1.Length];
      Buffer.BlockCopy((Array) numArray1, num1, (Array) numArray2, 0, numArray1.Length - num1);
    }
    switch ((JPEG_MARKER) cinfo.m_unread_marker)
    {
      case JPEG_MARKER.APP0:
        MarkerReader.examine_app0(cinfo, numArray2, datalen, V);
        break;
      case JPEG_MARKER.APP14:
        MarkerReader.examine_app14(cinfo, numArray2, datalen, V);
        break;
    }
    if (V > 0)
      cinfo.m_src.skip_input_data(V);
    return true;
  }

  private static bool skip_variable(DecompressStruct cinfo)
  {
    int V;
    if (!cinfo.m_src.GetTwoBytes(out V))
      return false;
    int num_bytes = V - 2;
    if (num_bytes > 0)
      cinfo.m_src.skip_input_data(num_bytes);
    return true;
  }

  private static bool get_interesting_appn(DecompressStruct cinfo)
  {
    int V1;
    if (!cinfo.m_src.GetTwoBytes(out V1))
      return false;
    int num1 = V1 - 2;
    int datalen = 0;
    if (num1 >= 14)
      datalen = 14;
    else if (num1 > 0)
      datalen = num1;
    byte[] data = new byte[14];
    for (int index = 0; index < datalen; ++index)
    {
      int V2 = 0;
      if (!cinfo.m_src.GetByte(out V2))
        return false;
      data[index] = (byte) V2;
    }
    int num2 = num1 - datalen;
    switch ((JPEG_MARKER) cinfo.m_unread_marker)
    {
      case JPEG_MARKER.APP0:
        MarkerReader.examine_app0(cinfo, data, datalen, num2);
        break;
      case JPEG_MARKER.APP14:
        MarkerReader.examine_app14(cinfo, data, datalen, num2);
        break;
    }
    if (num2 > 0)
      cinfo.m_src.skip_input_data(num2);
    return true;
  }

  private static void examine_app0(
    DecompressStruct cinfo,
    byte[] data,
    int datalen,
    int remaining)
  {
    int num1 = datalen + remaining;
    if (datalen >= 14 && data[0] == (byte) 74 && data[1] == (byte) 70 && data[2] == (byte) 73 && data[3] == (byte) 70 && data[4] == (byte) 0)
    {
      cinfo.m_saw_JFIF_marker = true;
      cinfo.m_JFIF_major_version = data[5];
      cinfo.m_JFIF_minor_version = data[6];
      cinfo.m_density_unit = (DensityUnit) data[7];
      cinfo.m_X_density = (short) (((int) data[8] << 8) + (int) data[9]);
      cinfo.m_Y_density = (short) (((int) data[10] << 8) + (int) data[11]);
      int num2 = num1 - 14;
    }
    else
    {
      if (datalen < 6 || data[0] != (byte) 74 || data[1] != (byte) 70 || data[2] != (byte) 88 || data[3] != (byte) 88)
        return;
      int num3 = (int) data[4];
    }
  }

  private static void examine_app14(
    DecompressStruct cinfo,
    byte[] data,
    int datalen,
    int remaining)
  {
    if (datalen < 12 || data[0] != (byte) 65 || data[1] != (byte) 100 || data[2] != (byte) 111 || data[3] != (byte) 98 || data[4] != (byte) 101)
      return;
    int num1 = (int) data[5];
    int num2 = (int) data[6];
    int num3 = (int) data[7];
    int num4 = (int) data[8];
    int num5 = (int) data[9];
    int num6 = (int) data[10];
    int num7 = (int) data[11];
    cinfo.m_saw_Adobe_marker = true;
    cinfo.m_Adobe_transform = (byte) num7;
  }

  private bool get_soi()
  {
    this.m_cinfo.m_restart_interval = 0;
    this.m_cinfo.m_jpeg_color_space = J_COLOR_SPACE.JCS_UNKNOWN;
    this.m_cinfo.m_CCIR601_sampling = false;
    this.m_cinfo.m_saw_JFIF_marker = false;
    this.m_cinfo.m_JFIF_major_version = (byte) 1;
    this.m_cinfo.m_JFIF_minor_version = (byte) 1;
    this.m_cinfo.m_density_unit = DensityUnit.Unknown;
    this.m_cinfo.m_X_density = (short) 1;
    this.m_cinfo.m_Y_density = (short) 1;
    this.m_cinfo.m_saw_Adobe_marker = false;
    this.m_cinfo.m_Adobe_transform = (byte) 0;
    this.m_cinfo.m_marker.m_saw_SOI = true;
    return true;
  }

  private bool get_sof(bool is_prog)
  {
    this.m_cinfo.m_progressive_mode = is_prog;
    int V1;
    if (!this.m_cinfo.m_src.GetTwoBytes(out V1) || !this.m_cinfo.m_src.GetByte(out this.m_cinfo.m_data_precision))
      return false;
    int V2 = 0;
    if (!this.m_cinfo.m_src.GetTwoBytes(out V2))
      return false;
    this.m_cinfo.m_image_height = V2;
    if (!this.m_cinfo.m_src.GetTwoBytes(out V2))
      return false;
    this.m_cinfo.m_image_width = V2;
    if (!this.m_cinfo.m_src.GetByte(out this.m_cinfo.m_num_components))
      return false;
    int num = V1 - 8;
    if (this.m_cinfo.Comp_info == null)
      this.m_cinfo.Comp_info = ComponentInfo.createArrayOfComponents(this.m_cinfo.m_num_components);
    for (int index = 0; index < this.m_cinfo.m_num_components; ++index)
    {
      this.m_cinfo.Comp_info[index].Component_index = index;
      int V3;
      if (!this.m_cinfo.m_src.GetByte(out V3))
        return false;
      this.m_cinfo.Comp_info[index].Component_id = V3;
      int V4;
      if (!this.m_cinfo.m_src.GetByte(out V4))
        return false;
      this.m_cinfo.Comp_info[index].H_samp_factor = V4 >> 4 & 15;
      this.m_cinfo.Comp_info[index].V_samp_factor = V4 & 15;
      int V5;
      if (!this.m_cinfo.m_src.GetByte(out V5))
        return false;
      this.m_cinfo.Comp_info[index].Quant_tbl_no = V5;
    }
    this.m_cinfo.m_marker.m_saw_SOF = true;
    return true;
  }

  private bool get_sos()
  {
    int V1;
    if (!this.m_cinfo.m_src.GetTwoBytes(out int _) || !this.m_cinfo.m_src.GetByte(out V1))
      return false;
    this.m_cinfo.m_comps_in_scan = V1;
    for (int index1 = 0; index1 < V1; ++index1)
    {
      int V2;
      int V3;
      if (!this.m_cinfo.m_src.GetByte(out V2) || !this.m_cinfo.m_src.GetByte(out V3))
        return false;
      int index2 = -1;
      for (int index3 = 0; index3 < this.m_cinfo.m_num_components; ++index3)
      {
        if (V2 == this.m_cinfo.Comp_info[index3].Component_id)
        {
          index2 = index3;
          break;
        }
      }
      this.m_cinfo.m_cur_comp_info[index1] = index2;
      this.m_cinfo.Comp_info[index2].Dc_tbl_no = V3 >> 4 & 15;
      this.m_cinfo.Comp_info[index2].Ac_tbl_no = V3 & 15;
    }
    int V4;
    if (!this.m_cinfo.m_src.GetByte(out V4))
      return false;
    this.m_cinfo.m_Ss = V4;
    if (!this.m_cinfo.m_src.GetByte(out V4))
      return false;
    this.m_cinfo.m_Se = V4;
    if (!this.m_cinfo.m_src.GetByte(out V4))
      return false;
    this.m_cinfo.m_Ah = V4 >> 4 & 15;
    this.m_cinfo.m_Al = V4 & 15;
    this.m_cinfo.m_marker.m_next_restart_num = 0;
    ++this.m_cinfo.m_input_scan_number;
    return true;
  }

  private bool get_dht()
  {
    int V1;
    if (!this.m_cinfo.m_src.GetTwoBytes(out V1))
      return false;
    int num1 = V1 - 2;
    byte[] src1 = new byte[17];
    byte[] src2 = new byte[256 /*0x0100*/];
    while (num1 > 16 /*0x10*/)
    {
      int V2;
      if (!this.m_cinfo.m_src.GetByte(out V2))
        return false;
      src1[0] = (byte) 0;
      int num2 = 0;
      for (int index = 1; index <= 16 /*0x10*/; ++index)
      {
        int V3 = 0;
        if (!this.m_cinfo.m_src.GetByte(out V3))
          return false;
        src1[index] = (byte) V3;
        num2 += (int) src1[index];
      }
      int num3 = num1 - 17;
      for (int index = 0; index < num2; ++index)
      {
        int V4 = 0;
        if (!this.m_cinfo.m_src.GetByte(out V4))
          return false;
        src2[index] = (byte) V4;
      }
      num1 = num3 - num2;
      JHUFF_TBL jhuffTbl;
      if ((V2 & 16 /*0x10*/) != 0)
      {
        V2 -= 16 /*0x10*/;
        if (this.m_cinfo.m_ac_huff_tbl_ptrs[V2] == null)
          this.m_cinfo.m_ac_huff_tbl_ptrs[V2] = new JHUFF_TBL();
        jhuffTbl = this.m_cinfo.m_ac_huff_tbl_ptrs[V2];
      }
      else
      {
        if (this.m_cinfo.m_dc_huff_tbl_ptrs[V2] == null)
          this.m_cinfo.m_dc_huff_tbl_ptrs[V2] = new JHUFF_TBL();
        jhuffTbl = this.m_cinfo.m_dc_huff_tbl_ptrs[V2];
      }
      Buffer.BlockCopy((Array) src1, 0, (Array) jhuffTbl.Bits, 0, jhuffTbl.Bits.Length);
      Buffer.BlockCopy((Array) src2, 0, (Array) jhuffTbl.Huffval, 0, jhuffTbl.Huffval.Length);
    }
    return true;
  }

  private bool get_dqt()
  {
    int V1;
    if (!this.m_cinfo.m_src.GetTwoBytes(out V1))
      return false;
    int num1 = V1 - 2;
    while (num1 > 0)
    {
      int V2;
      if (!this.m_cinfo.m_src.GetByte(out V2))
        return false;
      int num2 = V2 >> 4;
      V2 &= 15;
      if (this.m_cinfo.m_quant_tbl_ptrs[V2] == null)
        this.m_cinfo.m_quant_tbl_ptrs[V2] = new JQUANT_TBL();
      JQUANT_TBL quantTblPtr = this.m_cinfo.m_quant_tbl_ptrs[V2];
      for (int index = 0; index < 64 /*0x40*/; ++index)
      {
        int num3;
        if (num2 != 0)
        {
          int V3 = 0;
          if (!this.m_cinfo.m_src.GetTwoBytes(out V3))
            return false;
          num3 = V3;
        }
        else
        {
          int V4 = 0;
          if (!this.m_cinfo.m_src.GetByte(out V4))
            return false;
          num3 = V4;
        }
        quantTblPtr.quantval[JpegUtils.jpeg_natural_order[index]] = (short) num3;
      }
      num1 -= 65;
      if (num2 != 0)
        num1 -= 64 /*0x40*/;
    }
    return true;
  }

  private bool get_dri()
  {
    if (!this.m_cinfo.m_src.GetTwoBytes(out int _))
      return false;
    int V = 0;
    if (!this.m_cinfo.m_src.GetTwoBytes(out V))
      return false;
    this.m_cinfo.m_restart_interval = V;
    return true;
  }

  private bool first_marker()
  {
    int V;
    if (!this.m_cinfo.m_src.GetByte(out int _) || !this.m_cinfo.m_src.GetByte(out V))
      return false;
    this.m_cinfo.m_unread_marker = V;
    return true;
  }
}
