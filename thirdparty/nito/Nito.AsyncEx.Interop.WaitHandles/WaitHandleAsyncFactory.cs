// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.Interop.WaitHandleAsyncFactory
// Assembly: Nito.AsyncEx.Interop.WaitHandles, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 553640E6-A144-4B4A-9FF0-F8BCD7049AA0
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Interop.WaitHandles.dll

using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx.Interop;

public static class WaitHandleAsyncFactory
{
  public static Task FromWaitHandle(WaitHandle handle)
  {
    return (Task) WaitHandleAsyncFactory.FromWaitHandle(handle, Timeout.InfiniteTimeSpan, CancellationToken.None);
  }

  public static Task<bool> FromWaitHandle(WaitHandle handle, TimeSpan timeout)
  {
    return WaitHandleAsyncFactory.FromWaitHandle(handle, timeout, CancellationToken.None);
  }

  public static Task FromWaitHandle(WaitHandle handle, CancellationToken token)
  {
    return (Task) WaitHandleAsyncFactory.FromWaitHandle(handle, Timeout.InfiniteTimeSpan, token);
  }

  public static Task<bool> FromWaitHandle(
    WaitHandle handle,
    TimeSpan timeout,
    CancellationToken token)
  {
    if (handle == null)
      throw new ArgumentNullException(nameof (handle));
    if (handle.WaitOne(0))
      return TaskConstants.BooleanTrue;
    if (timeout == TimeSpan.Zero)
      return TaskConstants.BooleanFalse;
    return token.IsCancellationRequested ? TaskConstants<bool>.Canceled : WaitHandleAsyncFactory.DoFromWaitHandle(handle, timeout, token);
  }

  private static async Task<bool> DoFromWaitHandle(
    WaitHandle handle,
    TimeSpan timeout,
    CancellationToken token)
  {
    TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
    bool flag;
    using (new WaitHandleAsyncFactory.ThreadPoolRegistration(handle, timeout, completionSource))
    {
      using (token.Register((Action<object>) (state => ((TaskCompletionSource<bool>) state).TrySetCanceled()), (object) completionSource, false))
        flag = await completionSource.Task.ConfigureAwait(false);
    }
    return flag;
  }

  private sealed class ThreadPoolRegistration : IDisposable
  {
    private readonly RegisteredWaitHandle _registeredWaitHandle;

    public ThreadPoolRegistration(
      WaitHandle handle,
      TimeSpan timeout,
      TaskCompletionSource<bool> tcs)
    {
      this._registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(handle, (WaitOrTimerCallback) ((state, timedOut) => ((TaskCompletionSource<bool>) state).TrySetResult(!timedOut)), (object) tcs, timeout, true);
    }

    void IDisposable.Dispose() => this._registeredWaitHandle.Unregister((WaitHandle) null);
  }
}
