// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.JpegConstants
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal static class JpegConstants
{
  public const int DCTSIZE = 8;
  public const int DCTSIZE2 = 64 /*0x40*/;
  public const int NUM_QUANT_TBLS = 4;
  public const int NUM_HUFF_TBLS = 4;
  public const int MAX_COMPS_IN_SCAN = 4;
  public const int C_MAX_BLOCKS_IN_MCU = 10;
  public const int D_MAX_BLOCKS_IN_MCU = 10;
  public const int MAX_SAMP_FACTOR = 4;
  public const int MAX_COMPONENTS = 10;
  public const int BITS_IN_JSAMPLE = 8;
  public const J_DCT_METHOD JDCT_DEFAULT = J_DCT_METHOD.JDCT_ISLOW;
  public const J_DCT_METHOD JDCT_FASTEST = J_DCT_METHOD.JDCT_IFAST;
  public const int JPEG_MAX_DIMENSION = 65500;
  public const int MAXJSAMPLE = 255 /*0xFF*/;
  public const int CENTERJSAMPLE = 128 /*0x80*/;
  public const int RGB_RED = 0;
  public const int RGB_GREEN = 1;
  public const int RGB_BLUE = 2;
  public const int RGB_PIXELSIZE = 3;
  public const int HUFF_LOOKAHEAD = 8;
}
