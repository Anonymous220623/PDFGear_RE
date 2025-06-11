// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.Interop.ApmAsyncFactory
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using System;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx.Interop;

public static class ApmAsyncFactory
{
  public static IAsyncResult ToBegin(Task task, AsyncCallback callback, object state)
  {
    TaskCompletionSource<object> tcs = new TaskCompletionSource<object>(state, TaskCreationOptions.RunContinuationsAsynchronously);
    SynchronizationContextSwitcher.NoContext((Action) (() => ApmAsyncFactory.CompleteAsync(task, callback, tcs)));
    return (IAsyncResult) tcs.Task;
  }

  private static async void CompleteAsync(
    Task task,
    AsyncCallback callback,
    TaskCompletionSource<object?> tcs)
  {
    try
    {
      await task.ConfigureAwait(false);
      tcs.TrySetResult((object) null);
    }
    catch (OperationCanceledException ex)
    {
      tcs.TrySetCanceled(ex.CancellationToken);
    }
    catch (Exception ex)
    {
      tcs.TrySetException(ex);
    }
    finally
    {
      AsyncCallback asyncCallback = callback;
      if (asyncCallback != null)
        asyncCallback((IAsyncResult) tcs.Task);
    }
  }

  public static void ToEnd(IAsyncResult asyncResult)
  {
    ((Task) asyncResult).WaitAndUnwrapException();
  }

  public static IAsyncResult ToBegin<TResult>(
    Task<TResult> task,
    AsyncCallback callback,
    object state)
  {
    TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>(state, TaskCreationOptions.RunContinuationsAsynchronously);
    SynchronizationContextSwitcher.NoContext((Action) (() => ApmAsyncFactory.CompleteAsync<TResult>(task, callback, tcs)));
    return (IAsyncResult) tcs.Task;
  }

  private static async void CompleteAsync<TResult>(
    Task<TResult> task,
    AsyncCallback callback,
    TaskCompletionSource<TResult> tcs)
  {
    try
    {
      TaskCompletionSource<TResult> completionSource = tcs;
      completionSource.TrySetResult(await task.ConfigureAwait(false));
      completionSource = (TaskCompletionSource<TResult>) null;
    }
    catch (OperationCanceledException ex)
    {
      tcs.TrySetCanceled(ex.CancellationToken);
    }
    catch (Exception ex)
    {
      tcs.TrySetException(ex);
    }
    finally
    {
      AsyncCallback asyncCallback = callback;
      if (asyncCallback != null)
        asyncCallback((IAsyncResult) tcs.Task);
    }
  }

  public static TResult ToEnd<TResult>(IAsyncResult asyncResult)
  {
    return ((Task<TResult>) asyncResult).WaitAndUnwrapException<TResult>();
  }
}
