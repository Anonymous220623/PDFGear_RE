// Decompiled with JetBrains decompiler
// Type: InteropDotNet.SystemManager
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System;

#nullable disable
namespace InteropDotNet;

internal static class SystemManager
{
  public static string GetPlatformName() => IntPtr.Size != 4 ? "x64" : "x86";

  public static OperatingSystem GetOperatingSystem()
  {
    switch (Environment.OSVersion.Platform)
    {
      case PlatformID.Win32S:
      case PlatformID.Win32Windows:
      case PlatformID.Win32NT:
      case PlatformID.WinCE:
        return OperatingSystem.Windows;
      case PlatformID.Unix:
      case (PlatformID) 128 /*0x80*/:
        return OperatingSystem.Unix;
      case PlatformID.MacOSX:
        return OperatingSystem.MacOSX;
      default:
        return OperatingSystem.Unknown;
    }
  }
}
