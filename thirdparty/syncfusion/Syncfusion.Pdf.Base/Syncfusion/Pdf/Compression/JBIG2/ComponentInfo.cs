// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.ComponentInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal class ComponentInfo
{
  private int component_id;
  private int component_index;
  private int h_samp_factor;
  private int v_samp_factor;
  private int quant_tbl_no;
  private int dc_tbl_no;
  private int ac_tbl_no;
  private int width_in_blocks;
  internal int height_in_blocks;
  internal int DCT_scaled_size;
  internal int downsampled_width;
  internal int downsampled_height;
  internal bool component_needed;
  internal int MCU_width;
  internal int MCU_height;
  internal int MCU_blocks;
  internal int MCU_sample_width;
  internal int last_col_width;
  internal int last_row_height;
  internal JQUANT_TBL quant_table;

  internal ComponentInfo()
  {
  }

  internal void Assign(ComponentInfo ci)
  {
    this.component_id = ci.component_id;
    this.component_index = ci.component_index;
    this.h_samp_factor = ci.h_samp_factor;
    this.v_samp_factor = ci.v_samp_factor;
    this.quant_tbl_no = ci.quant_tbl_no;
    this.dc_tbl_no = ci.dc_tbl_no;
    this.ac_tbl_no = ci.ac_tbl_no;
    this.width_in_blocks = ci.width_in_blocks;
    this.height_in_blocks = ci.height_in_blocks;
    this.DCT_scaled_size = ci.DCT_scaled_size;
    this.downsampled_width = ci.downsampled_width;
    this.downsampled_height = ci.downsampled_height;
    this.component_needed = ci.component_needed;
    this.MCU_width = ci.MCU_width;
    this.MCU_height = ci.MCU_height;
    this.MCU_blocks = ci.MCU_blocks;
    this.MCU_sample_width = ci.MCU_sample_width;
    this.last_col_width = ci.last_col_width;
    this.last_row_height = ci.last_row_height;
    this.quant_table = ci.quant_table;
  }

  public int Component_id
  {
    get => this.component_id;
    set => this.component_id = value;
  }

  public int Component_index
  {
    get => this.component_index;
    set => this.component_index = value;
  }

  public int H_samp_factor
  {
    get => this.h_samp_factor;
    set => this.h_samp_factor = value;
  }

  public int V_samp_factor
  {
    get => this.v_samp_factor;
    set => this.v_samp_factor = value;
  }

  public int Quant_tbl_no
  {
    get => this.quant_tbl_no;
    set => this.quant_tbl_no = value;
  }

  public int Dc_tbl_no
  {
    get => this.dc_tbl_no;
    set => this.dc_tbl_no = value;
  }

  public int Ac_tbl_no
  {
    get => this.ac_tbl_no;
    set => this.ac_tbl_no = value;
  }

  public int Width_in_blocks
  {
    get => this.width_in_blocks;
    set => this.width_in_blocks = value;
  }

  public int Downsampled_width => this.downsampled_width;

  internal static ComponentInfo[] createArrayOfComponents(int length)
  {
    ComponentInfo[] arrayOfComponents = length >= 0 ? new ComponentInfo[length] : throw new ArgumentOutOfRangeException(nameof (length));
    for (int index = 0; index < arrayOfComponents.Length; ++index)
      arrayOfComponents[index] = new ComponentInfo();
    return arrayOfComponents;
  }
}
