// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.SemaphoreSlimExtensions
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using Nito.Disposables;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

public static class SemaphoreSlimExtensions
{
  private static async Task<IDisposable> DoLockAsync(
    SemaphoreSlim @this,
    CancellationToken cancellationToken)
  {
    await @this.WaitAsync(cancellationToken).ConfigureAwait(false);
    return (IDisposable) Disposable.Create((Action) (() => @this.Release()));
  }

  public static AwaitableDisposable<IDisposable> LockAsync(
    this SemaphoreSlim @this,
    CancellationToken cancellationToken)
  {
    return @this != null ? new AwaitableDisposable<IDisposable>(SemaphoreSlimExtensions.DoLockAsync(@this, cancellationToken)) : throw new ArgumentNullException(nameof (@this));
  }

  public static AwaitableDisposable<IDisposable> LockAsync(this SemaphoreSlim @this)
  {
    return @this.LockAsync(CancellationToken.None);
  }

  public static IDisposable Lock(this SemaphoreSlim @this, CancellationToken cancellationToken)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    @this.Wait(cancellationToken);
    return (IDisposable) Disposable.Create((Action) (() => @this.Release()));
  }

  public static IDisposable Lock(this SemaphoreSlim @this) => @this.Lock(CancellationToken.None);
}
