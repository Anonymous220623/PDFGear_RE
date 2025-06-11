// Decompiled with JetBrains decompiler
// Type: Nito.Disposables.SingleDisposable`1
// Assembly: Nito.Disposables, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2B43B0E-C24B-48AC-BE52-0652CD6F6684
// Assembly location: D:\PDFGear\bin\Nito.Disposables.dll

using Nito.Disposables.Internals;
using System;
using System.Threading.Tasks;

#nullable enable
namespace Nito.Disposables;

public abstract class SingleDisposable<T> : IDisposable
{
  private readonly BoundActionField<T> _context;
  private readonly TaskCompletionSource<object> _tcs = new TaskCompletionSource<object>();

  protected SingleDisposable(T context)
  {
    this._context = new BoundActionField<T>(new Action<T>(this.Dispose), context);
  }

  public bool IsDisposeStarted => this._context.IsEmpty;

  public bool IsDisposed => this._tcs.Task.IsCompleted;

  public bool IsDisposing => this.IsDisposeStarted && !this.IsDisposed;

  protected abstract void Dispose(T context);

  public void Dispose()
  {
    BoundActionField<T>.IBoundAction andUnset = this._context.TryGetAndUnset();
    if (andUnset == null)
    {
      this._tcs.Task.GetAwaiter().GetResult();
    }
    else
    {
      try
      {
        andUnset.Invoke();
      }
      finally
      {
        this._tcs.TrySetResult((object) null);
      }
    }
  }

  protected bool TryUpdateContext(Func<T, T> contextUpdater)
  {
    return this._context.TryUpdateContext(contextUpdater);
  }
}
