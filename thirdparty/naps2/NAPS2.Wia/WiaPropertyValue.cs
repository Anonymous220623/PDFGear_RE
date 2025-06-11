// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.WiaPropertyValue
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

#nullable disable
namespace NAPS2.Wia;

public static class WiaPropertyValue
{
  public const int FEEDER = 1;
  public const int FLATBED = 2;
  public const int DUPLEX = 4;
  public const int FRONT_FIRST = 8;
  public const int BACK_FIRST = 16 /*0x10*/;
  public const int FRONT_ONLY = 32 /*0x20*/;
  public const int BACK_ONLY = 64 /*0x40*/;
  public const int ADVANCED_DUPLEX = 1024 /*0x0400*/;
  public const int FEED_READY = 1;
  public const int ALL_PAGES = 0;
}
