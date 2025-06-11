// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncWaitQueueExtensions
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

internal static class AsyncWaitQueueExtensions
{
  public static Task<T> Enqueue<T>(
    this IAsyncWaitQueue<T> @this,
    object mutex,
    CancellationToken token)
  {
    if (token.IsCancellationRequested)
      return Task.FromCanceled<T>(token);
    Task<T> ret = @this.Enqueue();
    if (!token.CanBeCanceled)
      return ret;
    CancellationTokenRegistration registration = token.Register((Action) (() =>
    {
      lock (mutex)
        @this.TryCancel((Task) ret, token);
    }), false);
    ret.ContinueWith((Action<Task<T>>) (_ => registration.Dispose()), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
    return ret;
  }
}
