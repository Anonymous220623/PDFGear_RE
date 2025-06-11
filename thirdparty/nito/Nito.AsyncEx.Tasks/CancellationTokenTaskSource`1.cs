// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.CancellationTokenTaskSource`1
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

public sealed class CancellationTokenTaskSource<T> : IDisposable
{
  private readonly IDisposable? _registration;

  public CancellationTokenTaskSource(CancellationToken cancellationToken)
  {
    if (cancellationToken.IsCancellationRequested)
    {
      this.Task = System.Threading.Tasks.Task.FromCanceled<T>(cancellationToken);
    }
    else
    {
      TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
      this._registration = (IDisposable) cancellationToken.Register((Action) (() => tcs.TrySetCanceled(cancellationToken)), false);
      this.Task = tcs.Task;
    }
  }

  public System.Threading.Tasks.Task<T> Task { get; private set; }

  public void Dispose() => this._registration?.Dispose();
}
