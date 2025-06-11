// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ConsoleTargetHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.Targets;

internal static class ConsoleTargetHelper
{
  private static readonly object _lockObject = new object();

  public static bool IsConsoleAvailable(out string reason)
  {
    reason = string.Empty;
    try
    {
      if (!Environment.UserInteractive)
      {
        if (PlatformDetector.IsMono && Console.In is StreamReader)
          return true;
        reason = "Environment.UserInteractive = False";
        return false;
      }
      if (Console.OpenStandardInput(1) == Stream.Null)
      {
        reason = "Console.OpenStandardInput = Null";
        return false;
      }
    }
    catch (Exception ex)
    {
      reason = $"Unexpected exception: {ex.GetType().Name}:{ex.Message}";
      InternalLogger.Warn(ex, "Failed to detect whether console is available.");
      return false;
    }
    return true;
  }

  public static Encoding GetConsoleOutputEncoding(
    Encoding currentEncoding,
    bool isInitialized,
    bool pauseLogging)
  {
    if (currentEncoding != null)
      return currentEncoding;
    return isInitialized && !pauseLogging || ConsoleTargetHelper.IsConsoleAvailable(out string _) ? Console.OutputEncoding : Encoding.Default;
  }

  public static bool SetConsoleOutputEncoding(
    Encoding newEncoding,
    bool isInitialized,
    bool pauseLogging)
  {
    if (!isInitialized)
      return true;
    if (!pauseLogging)
    {
      try
      {
        Console.OutputEncoding = newEncoding;
        return true;
      }
      catch (Exception ex)
      {
        object[] objArray = new object[1]
        {
          (object) newEncoding
        };
        InternalLogger.Warn(ex, "Failed changing Console.OutputEncoding to {0}", objArray);
      }
    }
    return false;
  }

  public static void WriteLineThreadSafe(TextWriter console, string message, bool flush = false)
  {
    lock (ConsoleTargetHelper._lockObject)
    {
      console.WriteLine(message);
      if (!flush)
        return;
      console.Flush();
    }
  }

  public static void WriteBufferThreadSafe(
    TextWriter console,
    char[] buffer,
    int length,
    bool flush = false)
  {
    lock (ConsoleTargetHelper._lockObject)
    {
      console.Write(buffer, 0, length);
      if (!flush)
        return;
      console.Flush();
    }
  }
}
