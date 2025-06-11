// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.ZlibConstants
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

#nullable disable
namespace Ionic.Zlib;

public static class ZlibConstants
{
  public const int WindowBitsMax = 15;
  public const int WindowBitsDefault = 15;
  public const int Z_OK = 0;
  public const int Z_STREAM_END = 1;
  public const int Z_NEED_DICT = 2;
  public const int Z_STREAM_ERROR = -2;
  public const int Z_DATA_ERROR = -3;
  public const int Z_BUF_ERROR = -5;
  public const int WorkingBufferSizeDefault = 16384 /*0x4000*/;
  public const int WorkingBufferSizeMin = 1024 /*0x0400*/;
}
