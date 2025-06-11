// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.TaskExtensions
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

public static class TaskExtensions
{
  public static Task WaitAsync(this Task @this, CancellationToken cancellationToken)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    if (!cancellationToken.CanBeCanceled)
      return @this;
    return cancellationToken.IsCancellationRequested ? Task.FromCanceled(cancellationToken) : TaskExtensions.DoWaitAsync(@this, cancellationToken);
  }

  private static async Task DoWaitAsync(Task task, CancellationToken cancellationToken)
  {
    using (CancellationTokenTaskSource<object> cancelTaskSource = new CancellationTokenTaskSource<object>(cancellationToken))
      await (await Task.WhenAny(task, (Task) cancelTaskSource.Task).ConfigureAwait(false)).ConfigureAwait(false);
  }

  public static Task<TResult> WaitAsync<TResult>(
    this Task<TResult> @this,
    CancellationToken cancellationToken)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    if (!cancellationToken.CanBeCanceled)
      return @this;
    return cancellationToken.IsCancellationRequested ? Task.FromCanceled<TResult>(cancellationToken) : TaskExtensions.DoWaitAsync<TResult>(@this, cancellationToken);
  }

  private static async Task<TResult> DoWaitAsync<TResult>(
    Task<TResult> task,
    CancellationToken cancellationToken)
  {
    TResult result;
    using (CancellationTokenTaskSource<TResult> cancelTaskSource = new CancellationTokenTaskSource<TResult>(cancellationToken))
      result = await (await Task.WhenAny<TResult>(new Task<TResult>[2]
      {
        task,
        cancelTaskSource.Task
      }).ConfigureAwait(false)).ConfigureAwait(false);
    return result;
  }

  public static Task<Task> WhenAny(
    this IEnumerable<Task> @this,
    CancellationToken cancellationToken)
  {
    return @this != null ? Task.WhenAny(@this).WaitAsync<Task>(cancellationToken) : throw new ArgumentNullException(nameof (@this));
  }

  public static Task<Task> WhenAny(this IEnumerable<Task> @this)
  {
    return @this != null ? Task.WhenAny(@this) : throw new ArgumentNullException(nameof (@this));
  }

  public static Task<Task<TResult>> WhenAny<TResult>(
    this IEnumerable<Task<TResult>> @this,
    CancellationToken cancellationToken)
  {
    return @this != null ? Task.WhenAny<TResult>(@this).WaitAsync<Task<TResult>>(cancellationToken) : throw new ArgumentNullException(nameof (@this));
  }

  public static Task<Task<TResult>> WhenAny<TResult>(this IEnumerable<Task<TResult>> @this)
  {
    return @this != null ? Task.WhenAny<TResult>(@this) : throw new ArgumentNullException(nameof (@this));
  }

  public static Task WhenAll(this IEnumerable<Task> @this)
  {
    return @this != null ? Task.WhenAll(@this) : throw new ArgumentNullException(nameof (@this));
  }

  public static Task<TResult[]> WhenAll<TResult>(this IEnumerable<Task<TResult>> @this)
  {
    return @this != null ? Task.WhenAll<TResult>(@this) : throw new ArgumentNullException(nameof (@this));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public static async void Ignore(this Task @this)
  {
    int num;
    if (num != 0 && @this == null)
      throw new ArgumentNullException(nameof (@this));
    try
    {
      await @this.ConfigureAwait(false);
    }
    catch
    {
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public static async void Ignore<T>(this Task<T> @this)
  {
    int num;
    if (num != 0 && @this == null)
      throw new ArgumentNullException(nameof (@this));
    try
    {
      T obj = await @this.ConfigureAwait(false);
    }
    catch
    {
    }
  }

  public static List<Task<T>> OrderByCompletion<T>(this IEnumerable<Task<T>> @this)
  {
    Task<T>[] taskArray = @this != null ? @this.ToArray<Task<T>>() : throw new ArgumentNullException(nameof (@this));
    int length = taskArray.Length;
    TaskCompletionSource<T>[] tcs = new TaskCompletionSource<T>[length];
    List<Task<T>> taskList = new List<Task<T>>(length);
    int lastIndex = -1;
    Action<Task<T>> continuationAction = (Action<Task<T>>) (task => tcs[Interlocked.Increment(ref lastIndex)].TryCompleteFromCompletedTask<T, T>(task));
    for (int index = 0; index != length; ++index)
    {
      tcs[index] = new TaskCompletionSource<T>();
      taskList.Add(tcs[index].Task);
      taskArray[index].ContinueWith(continuationAction, CancellationToken.None, TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
    }
    return taskList;
  }

  public static List<Task> OrderByCompletion(this IEnumerable<Task> @this)
  {
    Task[] taskArray = @this != null ? @this.ToArray<Task>() : throw new ArgumentNullException(nameof (@this));
    int length = taskArray.Length;
    TaskCompletionSource<object>[] tcs = new TaskCompletionSource<object>[length];
    List<Task> taskList = new List<Task>(length);
    int lastIndex = -1;
    Action<Task> continuationAction = (Action<Task>) (task => tcs[Interlocked.Increment(ref lastIndex)].TryCompleteFromCompletedTask<object>(task, TaskExtensions.NullResultFunc));
    for (int index = 0; index != length; ++index)
    {
      tcs[index] = new TaskCompletionSource<object>();
      taskList.Add((Task) tcs[index].Task);
      taskArray[index].ContinueWith(continuationAction, CancellationToken.None, TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
    }
    return taskList;
  }

  private static Func<object?> NullResultFunc { get; } = (Func<object>) (() => (object) null);
}
