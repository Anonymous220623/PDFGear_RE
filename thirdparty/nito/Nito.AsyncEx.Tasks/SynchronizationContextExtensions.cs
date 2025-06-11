// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.SynchronizationContextExtensions
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

public static class SynchronizationContextExtensions
{
  public static void Send(this SynchronizationContext @this, Action action)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    @this.Send((SendOrPostCallback) (state => ((Action) state)()), (object) action);
  }

  public static T Send<T>(this SynchronizationContext @this, Func<T> action)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    T result = default (T);
    @this.Send((SendOrPostCallback) (state => result = ((Func<T>) state)()), (object) action);
    return result;
  }

  public static Task PostAsync(this SynchronizationContext @this, Action action)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    TaskCompletionSource<object> tcs = TaskCompletionSourceExtensions.CreateAsyncTaskSource<object>();
    @this.Post((SendOrPostCallback) (state =>
    {
      try
      {
        ((Action) state)();
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
    }), (object) action);
    return (Task) tcs.Task;
  }

  public static Task<T> PostAsync<T>(this SynchronizationContext @this, Func<T> action)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    TaskCompletionSource<T> tcs = TaskCompletionSourceExtensions.CreateAsyncTaskSource<T>();
    @this.Post((SendOrPostCallback) (state =>
    {
      try
      {
        tcs.SetResult(((Func<T>) state)());
      }
      catch (OperationCanceledException ex)
      {
        tcs.TrySetCanceled(ex.CancellationToken);
      }
      catch (Exception ex)
      {
        tcs.TrySetException(ex);
      }
    }), (object) action);
    return tcs.Task;
  }

  public static Task PostAsync(this SynchronizationContext @this, Func<Task> action)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    TaskCompletionSource<object> tcs = TaskCompletionSourceExtensions.CreateAsyncTaskSource<object>();
    @this.Post((SendOrPostCallback) (async state =>
    {
      try
      {
        await ((Func<Task>) state)().ConfigureAwait(false);
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
    }), (object) action);
    return (Task) tcs.Task;
  }

  public static Task<T> PostAsync<T>(this SynchronizationContext @this, Func<Task<T>> action)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    TaskCompletionSource<T> tcs = TaskCompletionSourceExtensions.CreateAsyncTaskSource<T>();
    @this.Post((SendOrPostCallback) (async state =>
    {
      try
      {
        TaskCompletionSource<T> completionSource = tcs;
        completionSource.SetResult(await ((Func<Task<T>>) state)().ConfigureAwait(false));
        completionSource = (TaskCompletionSource<T>) null;
      }
      catch (OperationCanceledException ex)
      {
        tcs.TrySetCanceled(ex.CancellationToken);
      }
      catch (Exception ex)
      {
        tcs.TrySetException(ex);
      }
    }), (object) action);
    return tcs.Task;
  }
}
