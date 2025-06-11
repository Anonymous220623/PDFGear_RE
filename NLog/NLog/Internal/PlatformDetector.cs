// Decompiled with JetBrains decompiler
// Type: NLog.Internal.PlatformDetector
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal.FileAppenders;
using System;

#nullable disable
namespace NLog.Internal;

internal static class PlatformDetector
{
  private static RuntimeOS currentOS = PlatformDetector.GetCurrentRuntimeOS();
  private static bool? _isMono;
  private static bool? runTimeSupportsSharableMutex;

  public static RuntimeOS CurrentOS => PlatformDetector.currentOS;

  public static bool IsWin32
  {
    get
    {
      return PlatformDetector.currentOS == RuntimeOS.Windows || PlatformDetector.currentOS == RuntimeOS.WindowsNT;
    }
  }

  public static bool IsUnix
  {
    get
    {
      return PlatformDetector.currentOS == RuntimeOS.Linux || PlatformDetector.currentOS == RuntimeOS.MacOSX;
    }
  }

  public static bool IsMono
  {
    get
    {
      return PlatformDetector._isMono ?? (PlatformDetector._isMono = new bool?(Type.GetType("Mono.Runtime") != (Type) null)).Value;
    }
  }

  public static bool SupportsSharableMutex
  {
    get
    {
      return (!PlatformDetector.IsMono || Environment.Version.Major >= 4) && PlatformDetector.RunTimeSupportsSharableMutex;
    }
  }

  private static bool RunTimeSupportsSharableMutex
  {
    get
    {
      if (PlatformDetector.runTimeSupportsSharableMutex.HasValue)
        return PlatformDetector.runTimeSupportsSharableMutex.Value;
      try
      {
        BaseMutexFileAppender.ForceCreateSharableMutex("NLogMutexTester").Close();
        PlatformDetector.runTimeSupportsSharableMutex = new bool?(true);
      }
      catch (Exception ex)
      {
        InternalLogger.Debug(ex, "Failed to create sharable mutex processes");
        PlatformDetector.runTimeSupportsSharableMutex = new bool?(false);
      }
      return PlatformDetector.runTimeSupportsSharableMutex.Value;
    }
  }

  private static RuntimeOS GetCurrentRuntimeOS()
  {
    switch (Environment.OSVersion.Platform)
    {
      case PlatformID.Win32Windows:
        return RuntimeOS.Windows;
      case PlatformID.Win32NT:
        return RuntimeOS.WindowsNT;
      case PlatformID.Unix:
      case (PlatformID) 128 /*0x80*/:
        return RuntimeOS.Linux;
      default:
        return RuntimeOS.Unknown;
    }
  }
}
