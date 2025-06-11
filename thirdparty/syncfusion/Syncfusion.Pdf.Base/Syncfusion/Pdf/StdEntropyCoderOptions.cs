// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.StdEntropyCoderOptions
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf;

[StructLayout(LayoutKind.Sequential, Size = 1)]
internal struct StdEntropyCoderOptions
{
  public static readonly int OPT_BYPASS = 1;
  public static readonly int OPT_RESET_MQ = 2;
  public static readonly int OPT_TERM_PASS = 4;
  public static readonly int OPT_VERT_STR_CAUSAL = 8;
  public static readonly int OPT_PRED_TERM = 16 /*0x10*/;
  public static readonly int OPT_SEG_SYMBOLS = 32 /*0x20*/;
  public static readonly int MIN_CB_DIM = 4;
  public static readonly int MAX_CB_DIM = 1024 /*0x0400*/;
  public static readonly int MAX_CB_AREA = 4096 /*0x1000*/;
  public static readonly int STRIPE_HEIGHT = 4;
  public static readonly int NUM_PASSES = 3;
  public static readonly int NUM_NON_BYPASS_MS_BP = 4;
  public static readonly int NUM_EMPTY_PASSES_IN_MS_BP = 2;
  public static readonly int FIRST_BYPASS_PASS_IDX = StdEntropyCoderOptions.NUM_PASSES * StdEntropyCoderOptions.NUM_NON_BYPASS_MS_BP - StdEntropyCoderOptions.NUM_EMPTY_PASSES_IN_MS_BP;
}
