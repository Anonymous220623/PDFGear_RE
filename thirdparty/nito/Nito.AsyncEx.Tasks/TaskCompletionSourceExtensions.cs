// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.TaskCompletionSourceExtensions
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

public static class TaskCompletionSourceExtensions
{
  public static bool TryCompleteFromCompletedTask<TResult, TSourceResult>(
    this TaskCompletionSource<TResult> @this,
    Task<TSourceResult> task)
    where TSourceResult : TResult
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    if (task == null)
      throw new ArgumentNullException(nameof (task));
    if (task.IsFaulted)
      return @this.TrySetException((IEnumerable<Exception>) task.Exception.InnerExceptions);
    if (task.IsCanceled)
    {
      try
      {
        task.WaitAndUnwrapException<TSourceResult>();
      }
      catch (OperationCanceledException ex)
      {
        CancellationToken cancellationToken = ex.CancellationToken;
        return cancellationToken.IsCancellationRequested ? @this.TrySetCanceled(cancellationToken) : @this.TrySetCanceled();
      }
    }
    return @this.TrySetResult((TResult) task.Result);
  }

  public static bool TryCompleteFromCompletedTask<TResult>(
    this TaskCompletionSource<TResult> @this,
    Task task,
    Func<TResult> resultFunc)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    if (task == null)
      throw new ArgumentNullException(nameof (task));
    if (resultFunc == null)
      throw new ArgumentNullException(nameof (resultFunc));
    if (task.IsFaulted)
      return @this.TrySetException((IEnumerable<Exception>) task.Exception.InnerExceptions);
    if (task.IsCanceled)
    {
      try
      {
        task.WaitAndUnwrapException();
      }
      catch (OperationCanceledException ex)
      {
        CancellationToken cancellationToken = ex.CancellationToken;
        return cancellationToken.IsCancellationRequested ? @this.TrySetCanceled(cancellationToken) : @this.TrySetCanceled();
      }
    }
    return @this.TrySetResult(resultFunc());
  }

  public static TaskCompletionSource<TResult> CreateAsyncTaskSource<TResult>()
  {
    return new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);
  }
}
