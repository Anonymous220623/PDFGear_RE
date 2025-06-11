// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.Synchronous.TaskExtensions
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx.Synchronous;

public static class TaskExtensions
{
  public static void WaitAndUnwrapException(this Task task)
  {
    if (task == null)
      throw new ArgumentNullException(nameof (task));
    task.GetAwaiter().GetResult();
  }

  public static void WaitAndUnwrapException(this Task task, CancellationToken cancellationToken)
  {
    if (task == null)
      throw new ArgumentNullException(nameof (task));
    try
    {
      task.Wait(cancellationToken);
    }
    catch (AggregateException ex)
    {
      throw ExceptionHelpers.PrepareForRethrow(ex.InnerException);
    }
  }

  public static TResult WaitAndUnwrapException<TResult>(this Task<TResult> task)
  {
    return task != null ? task.GetAwaiter().GetResult() : throw new ArgumentNullException(nameof (task));
  }

  public static TResult WaitAndUnwrapException<TResult>(
    this Task<TResult> task,
    CancellationToken cancellationToken)
  {
    if (task == null)
      throw new ArgumentNullException(nameof (task));
    try
    {
      task.Wait(cancellationToken);
      return task.Result;
    }
    catch (AggregateException ex)
    {
      throw ExceptionHelpers.PrepareForRethrow(ex.InnerException);
    }
  }

  public static void WaitWithoutException(this Task task)
  {
    if (task == null)
      throw new ArgumentNullException(nameof (task));
    try
    {
      task.Wait();
    }
    catch (AggregateException ex)
    {
    }
  }

  public static void WaitWithoutException(this Task task, CancellationToken cancellationToken)
  {
    if (task == null)
      throw new ArgumentNullException(nameof (task));
    try
    {
      task.Wait(cancellationToken);
    }
    catch (AggregateException ex)
    {
      cancellationToken.ThrowIfCancellationRequested();
    }
  }
}
