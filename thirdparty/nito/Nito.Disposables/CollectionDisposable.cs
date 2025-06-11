// Decompiled with JetBrains decompiler
// Type: Nito.Disposables.CollectionDisposable
// Assembly: Nito.Disposables, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2B43B0E-C24B-48AC-BE52-0652CD6F6684
// Assembly location: D:\PDFGear\bin\Nito.Disposables.dll

using Nito.Disposables.Internals;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

#nullable enable
namespace Nito.Disposables;

public sealed class CollectionDisposable(IEnumerable<IDisposable?> disposables) : 
  SingleDisposable<ImmutableQueue<IDisposable>>(ImmutableQueue.CreateRange<IDisposable>(EnumerableExtensions.WhereNotNull<IDisposable>(disposables)))
{
  public CollectionDisposable(params IDisposable?[] disposables)
    : this((IEnumerable<IDisposable>) disposables)
  {
  }

  protected override void Dispose(ImmutableQueue<IDisposable> context)
  {
    foreach (IDisposable disposable in context)
      disposable?.Dispose();
  }

  public void Add(IDisposable? disposable)
  {
    if (disposable == null || this.TryUpdateContext((Func<ImmutableQueue<IDisposable>, ImmutableQueue<IDisposable>>) (x => x.Enqueue(disposable))))
      return;
    this.Dispose();
    disposable.Dispose();
  }

  public static CollectionDisposable Create(params IDisposable?[] disposables)
  {
    return new CollectionDisposable(disposables);
  }

  public static CollectionDisposable Create(IEnumerable<IDisposable?> disposables)
  {
    return new CollectionDisposable(disposables);
  }
}
