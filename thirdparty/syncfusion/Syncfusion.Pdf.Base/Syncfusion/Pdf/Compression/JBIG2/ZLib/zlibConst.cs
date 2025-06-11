// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.ZLib.zlibConst
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.ZLib;

internal sealed class zlibConst
{
  private const string version_Renamed_Field = "1.0.2";
  public const int Z_NO_COMPRESSION = 0;
  public const int Z_BEST_SPEED = 1;
  public const int Z_BEST_COMPRESSION = 9;
  public const int Z_DEFAULT_COMPRESSION = -1;
  public const int Z_FILTERED = 1;
  public const int Z_HUFFMAN_ONLY = 2;
  public const int Z_DEFAULT_STRATEGY = 0;
  public const int Z_NO_FLUSH = 0;
  public const int Z_PARTIAL_FLUSH = 1;
  public const int Z_SYNC_FLUSH = 2;
  public const int Z_FULL_FLUSH = 3;
  public const int Z_FINISH = 4;
  public const int Z_OK = 0;
  public const int Z_STREAM_END = 1;
  public const int Z_NEED_DICT = 2;
  public const int Z_ERRNO = -1;
  public const int Z_STREAM_ERROR = -2;
  public const int Z_DATA_ERROR = -3;
  public const int Z_MEM_ERROR = -4;
  public const int Z_BUF_ERROR = -5;
  public const int Z_VERSION_ERROR = -6;

  public static string version() => "1.0.2";
}
