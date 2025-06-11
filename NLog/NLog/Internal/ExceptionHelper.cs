// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ExceptionHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Threading;

#nullable disable
namespace NLog.Internal;

internal static class ExceptionHelper
{
  private const string LoggedKey = "NLog.ExceptionLoggedToInternalLogger";

  public static void MarkAsLoggedToInternalLogger(this Exception exception)
  {
    if (exception == null)
      return;
    exception.Data[(object) "NLog.ExceptionLoggedToInternalLogger"] = (object) true;
  }

  public static bool IsLoggedToInternalLogger(this Exception exception)
  {
    if (exception != null)
    {
      int? count = exception.Data?.Count;
      int num = 0;
      if (count.GetValueOrDefault() > num & count.HasValue)
        return exception.Data[(object) "NLog.ExceptionLoggedToInternalLogger"] as bool? ?? false;
    }
    return false;
  }

  public static bool MustBeRethrown(this Exception exception, IInternalLoggerContext loggerContext = null)
  {
    if (exception.MustBeRethrownImmediately())
      return true;
    bool flag1 = exception is NLogConfigurationException;
    if (!exception.IsLoggedToInternalLogger())
    {
      NLog.LogLevel level = flag1 ? NLog.LogLevel.Warn : NLog.LogLevel.Error;
      if (loggerContext != null)
        InternalLogger.Log(exception, level, "{0}: Error has been raised.", (object) loggerContext);
      else
        InternalLogger.Log(exception, level, "Error has been raised.");
    }
    LogFactory logFactory = loggerContext?.LogFactory;
    bool flag2 = logFactory != null && logFactory.ThrowExceptions || LogManager.ThrowExceptions;
    return !flag1 ? flag2 : (bool?) logFactory?.ThrowConfigExceptions ?? LogManager.ThrowConfigExceptions ?? flag2;
  }

  public static bool MustBeRethrownImmediately(this Exception exception)
  {
    switch (exception)
    {
      case StackOverflowException _:
        return true;
      case ThreadAbortException _:
        return true;
      case OutOfMemoryException _:
        return true;
      default:
        return false;
    }
  }
}
