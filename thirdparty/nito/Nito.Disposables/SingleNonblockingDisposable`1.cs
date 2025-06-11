// Decompiled with JetBrains decompiler
// Type: Nito.Disposables.SingleNonblockingDisposable`1
// Assembly: Nito.Disposables, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2B43B0E-C24B-48AC-BE52-0652CD6F6684
// Assembly location: D:\PDFGear\bin\Nito.Disposables.dll

using Nito.Disposables.Internals;
using System;

#nullable enable
namespace Nito.Disposables;

public abstract class SingleNonblockingDisposable<T> : IDisposable
{
  private readonly BoundActionField<T> _context;

  protected SingleNonblockingDisposable(T context)
  {
    this._context = new BoundActionField<T>(new Action<T>(this.Dispose), context);
  }

  public bool IsDisposed => this._context.IsEmpty;

  protected abstract void Dispose(T context);

  public void Dispose() => this._context.TryGetAndUnset()?.Invoke();

  protected bool TryUpdateContext(Func<T, T> contextUpdater)
  {
    return this._context.TryUpdateContext(contextUpdater);
  }
}
