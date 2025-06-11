// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.Upsampler
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal abstract class Upsampler
{
  protected bool m_need_context_rows;

  public abstract void start_pass();

  public abstract void upsample(
    ComponentBuffer[] input_buf,
    ref int in_row_group_ctr,
    int in_row_groups_avail,
    byte[][] output_buf,
    ref int out_row_ctr,
    int out_rows_avail);

  public bool NeedContextRows() => this.m_need_context_rows;
}
