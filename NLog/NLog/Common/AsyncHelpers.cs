// Decompiled with JetBrains decompiler
// Type: NLog.Common.AsyncHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog.Common;

public static class AsyncHelpers
{
  internal static int GetManagedThreadId() => Thread.CurrentThread.ManagedThreadId;

  internal static void StartAsyncTask(AsyncHelpersTask asyncTask, object state)
  {
    ThreadPool.QueueUserWorkItem(asyncTask.AsyncDelegate, state);
  }

  internal static void WaitForDelay(TimeSpan delay) => Thread.Sleep(delay);

  public static void ForEachItemSequentially<T>(
    IEnumerable<T> items,
    AsyncContinuation asyncContinuation,
    AsynchronousAction<T> action)
  {
    action = AsyncHelpers.ExceptionGuard<T>(action);
    IEnumerator<T> enumerator = items.GetEnumerator();
    InvokeNext((Exception) null);

    void InvokeNext(Exception ex)
    {
      if (ex != null)
        asyncContinuation(ex);
      else if (!enumerator.MoveNext())
      {
        enumerator.Dispose();
        asyncContinuation((Exception) null);
      }
      else
        action(enumerator.Current, AsyncHelpers.PreventMultipleCalls(new AsyncContinuation(InvokeNext)));
    }
  }

  public static void Repeat(
    int repeatCount,
    AsyncContinuation asyncContinuation,
    AsynchronousAction action)
  {
    action = AsyncHelpers.ExceptionGuard(action);
    int remaining = repeatCount;
    InvokeNext((Exception) null);

    void InvokeNext(Exception ex)
    {
      if (ex != null)
        asyncContinuation(ex);
      else if (remaining-- <= 0)
        asyncContinuation((Exception) null);
      else
        action(AsyncHelpers.PreventMultipleCalls(new AsyncContinuation(InvokeNext)));
    }
  }

  public static AsyncContinuation PrecededBy(
    AsyncContinuation asyncContinuation,
    AsynchronousAction action)
  {
    action = AsyncHelpers.ExceptionGuard(action);
    return (AsyncContinuation) (ex =>
    {
      if (ex != null)
        asyncContinuation(ex);
      else
        action(AsyncHelpers.PreventMultipleCalls(asyncContinuation));
    });
  }

  public static AsyncContinuation WithTimeout(AsyncContinuation asyncContinuation, TimeSpan timeout)
  {
    return new AsyncContinuation(new TimeoutContinuation(asyncContinuation, timeout).Function);
  }

  public static void ForEachItemInParallel<T>(
    IEnumerable<T> values,
    AsyncContinuation asyncContinuation,
    AsynchronousAction<T> action)
  {
    action = AsyncHelpers.ExceptionGuard<T>(action);
    List<T> objList = new List<T>(values);
    int remaining = objList.Count;
    List<Exception> exceptions = new List<Exception>();
    InternalLogger.Trace<int>("ForEachItemInParallel() {0} items", objList.Count);
    if (remaining == 0)
    {
      asyncContinuation((Exception) null);
    }
    else
    {
      AsyncContinuation continuation = (AsyncContinuation) (ex =>
      {
        InternalLogger.Trace<Exception>("Continuation invoked: {0}", ex);
        if (ex != null)
        {
          lock (exceptions)
            exceptions.Add(ex);
        }
        int num = Interlocked.Decrement(ref remaining);
        InternalLogger.Trace<int>("Parallel task completed. {0} items remaining", num);
        if (num != 0)
          return;
        asyncContinuation(AsyncHelpers.GetCombinedException((IList<Exception>) exceptions));
      });
      foreach (T obj in objList)
      {
        T itemCopy = obj;
        AsyncHelpers.StartAsyncTask(new AsyncHelpersTask((WaitCallback) (s =>
        {
          AsyncContinuation asyncContinuation1 = AsyncHelpers.PreventMultipleCalls(continuation);
          try
          {
            action(itemCopy, asyncContinuation1);
          }
          catch (Exception ex)
          {
            InternalLogger.Error(ex, "ForEachItemInParallel - Unhandled Exception");
            if (ex.MustBeRethrownImmediately())
              throw;
            asyncContinuation1(ex);
          }
        })), (object) null);
      }
    }
  }

  public static void RunSynchronously(AsynchronousAction action)
  {
    ManualResetEvent ev = new ManualResetEvent(false);
    Exception lastException = (Exception) null;
    action(AsyncHelpers.PreventMultipleCalls((AsyncContinuation) (ex =>
    {
      lastException = ex;
      ev.Set();
    })));
    ev.WaitOne();
    if (lastException != null)
      throw new NLogRuntimeException("Asynchronous exception has occurred.", lastException);
  }

  public static AsyncContinuation PreventMultipleCalls(AsyncContinuation asyncContinuation)
  {
    return asyncContinuation.Target is SingleCallContinuation ? asyncContinuation : new AsyncContinuation(new SingleCallContinuation(asyncContinuation).Function);
  }

  public static Exception GetCombinedException(IList<Exception> exceptions)
  {
    if (exceptions.Count == 0)
      return (Exception) null;
    if (exceptions.Count == 1)
      return exceptions[0];
    StringBuilder stringBuilder = new StringBuilder();
    string str = string.Empty;
    string newLine = EnvironmentHelper.NewLine;
    foreach (Exception exception in (IEnumerable<Exception>) exceptions)
    {
      stringBuilder.Append(str);
      stringBuilder.Append(exception.ToString());
      stringBuilder.Append(newLine);
      str = newLine;
    }
    return (Exception) new NLogRuntimeException("Got multiple exceptions:\r\n" + (object) stringBuilder);
  }

  private static AsynchronousAction ExceptionGuard(AsynchronousAction action)
  {
    return (AsynchronousAction) (cont =>
    {
      try
      {
        action(cont);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrown())
          throw;
        cont(ex);
      }
    });
  }

  private static AsynchronousAction<T> ExceptionGuard<T>(AsynchronousAction<T> action)
  {
    return (AsynchronousAction<T>) ((argument, cont) =>
    {
      try
      {
        action(argument, cont);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrown())
          throw;
        cont(ex);
      }
    });
  }

  internal static bool WaitForDispose(this Timer timer, TimeSpan timeout)
  {
    timer.Change(-1, -1);
    if (timeout != TimeSpan.Zero)
    {
      ManualResetEvent notifyObject = new ManualResetEvent(false);
      if (timer.Dispose((WaitHandle) notifyObject) && !notifyObject.WaitOne((int) timeout.TotalMilliseconds))
        return false;
      notifyObject.Close();
    }
    else
      timer.Dispose();
    return true;
  }
}
