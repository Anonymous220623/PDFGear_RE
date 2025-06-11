// Decompiled with JetBrains decompiler
// Type: PDFKit.TaskExceptionHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable disable
namespace PDFKit;

internal static class TaskExceptionHelper
{
  public static event TaskUnhandledExceptionEventHandler UnhandledException;

  public static Action ExceptionBoundary(Action action)
  {
    return (Action) (() =>
    {
      try
      {
        action();
      }
      catch (Exception ex)
      {
        throw TaskExceptionHelper.WrapException(ex);
      }
    });
  }

  public static Func<TResult> ExceptionBoundary<TResult>(Func<TResult> function)
  {
    return (Func<TResult>) (() =>
    {
      try
      {
        return function();
      }
      catch (Exception ex)
      {
        throw TaskExceptionHelper.WrapException(ex);
      }
    });
  }

  public static Func<Task> ExceptionBoundary(Func<Task> function)
  {
    return (Func<Task>) (async () =>
    {
      try
      {
        await function().ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        throw TaskExceptionHelper.WrapException(ex);
      }
    });
  }

  public static Func<Task<TResult>> ExceptionBoundary<TResult>(Func<Task<TResult>> function)
  {
    return (Func<Task<TResult>>) (async () =>
    {
      TResult result;
      try
      {
        result = await function().ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        throw TaskExceptionHelper.WrapException(ex);
      }
      return result;
    });
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static Exception WrapException(Exception ex)
  {
    TaskUnhandledExceptionEventHandler unhandledException = TaskExceptionHelper.UnhandledException;
    if (unhandledException != null)
    {
      TaskUnhandledExceptionEventArgs e = new TaskUnhandledExceptionEventArgs(ex);
      unhandledException(e);
    }
    string stackTrace = ex.StackTrace;
    ex.Data[(object) "OriginalStackTrace"] = (object) stackTrace;
    return ex;
  }
}
