// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncCollection`1
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

[DebuggerDisplay("Count = {_collection.Count}, MaxCount = {_maxCount}")]
[DebuggerTypeProxy(typeof (AsyncCollection<>.DebugView))]
public sealed class AsyncCollection<T>
{
  private readonly IProducerConsumerCollection<T> _collection;
  private readonly int _maxCount;
  private readonly AsyncLock _mutex;
  private readonly AsyncConditionVariable _completedOrNotFull;
  private readonly AsyncConditionVariable _completedOrNotEmpty;
  private bool _completed;

  public AsyncCollection(IProducerConsumerCollection<T>? collection, int maxCount)
  {
    if (collection == null)
      collection = (IProducerConsumerCollection<T>) new ConcurrentQueue<T>();
    if (maxCount <= 0)
      throw new ArgumentOutOfRangeException(nameof (maxCount), "The maximum count must be greater than zero.");
    if (maxCount < collection.Count)
      throw new ArgumentException("The maximum count cannot be less than the number of elements in the collection.", nameof (maxCount));
    this._collection = collection;
    this._maxCount = maxCount;
    this._mutex = new AsyncLock();
    this._completedOrNotFull = new AsyncConditionVariable(this._mutex);
    this._completedOrNotEmpty = new AsyncConditionVariable(this._mutex);
  }

  public AsyncCollection(IProducerConsumerCollection<T>? collection)
    : this(collection, int.MaxValue)
  {
  }

  public AsyncCollection(int maxCount)
    : this((IProducerConsumerCollection<T>) null, maxCount)
  {
  }

  public AsyncCollection()
    : this((IProducerConsumerCollection<T>) null, int.MaxValue)
  {
  }

  private bool Empty => this._collection.Count == 0;

  private bool Full => this._collection.Count == this._maxCount;

  public void CompleteAdding()
  {
    using (this._mutex.Lock())
    {
      this._completed = true;
      this._completedOrNotEmpty.NotifyAll();
      this._completedOrNotFull.NotifyAll();
    }
  }

  internal async Task DoAddAsync(T item, CancellationToken cancellationToken, bool sync)
  {
    IDisposable disposable;
    if (sync)
      disposable = this._mutex.Lock();
    else
      disposable = await this._mutex.LockAsync().ConfigureAwait(false);
    using (disposable)
    {
      while (this.Full && !this._completed)
      {
        if (sync)
          this._completedOrNotFull.Wait(cancellationToken);
        else
          await this._completedOrNotFull.WaitAsync(cancellationToken).ConfigureAwait(false);
      }
      if (this._completed)
        throw new InvalidOperationException("Add failed; the producer/consumer collection has completed adding.");
      if (!this._collection.TryAdd(item))
        throw new InvalidOperationException("Add failed; the add to the underlying collection failed.");
      this._completedOrNotEmpty.Notify();
    }
  }

  public Task AddAsync(T item, CancellationToken cancellationToken)
  {
    return this.DoAddAsync(item, cancellationToken, false);
  }

  public void Add(T item, CancellationToken cancellationToken)
  {
    this.DoAddAsync(item, cancellationToken, true).WaitAndUnwrapException(CancellationToken.None);
  }

  public Task AddAsync(T item) => this.AddAsync(item, CancellationToken.None);

  public void Add(T item) => this.Add(item, CancellationToken.None);

  private async Task<bool> DoOutputAvailableAsync(CancellationToken cancellationToken, bool sync)
  {
    IDisposable disposable;
    if (sync)
      disposable = this._mutex.Lock();
    else
      disposable = await this._mutex.LockAsync().ConfigureAwait(false);
    bool flag;
    using (disposable)
    {
      while (this.Empty && !this._completed)
      {
        if (sync)
          this._completedOrNotEmpty.Wait(cancellationToken);
        else
          await this._completedOrNotEmpty.WaitAsync(cancellationToken).ConfigureAwait(false);
      }
      flag = !this.Empty;
    }
    return flag;
  }

  public Task<bool> OutputAvailableAsync(CancellationToken cancellationToken)
  {
    return this.DoOutputAvailableAsync(cancellationToken, false);
  }

  public Task<bool> OutputAvailableAsync() => this.OutputAvailableAsync(CancellationToken.None);

  public bool OutputAvailable(CancellationToken cancellationToken)
  {
    return this.DoOutputAvailableAsync(cancellationToken, true).WaitAndUnwrapException<bool>();
  }

  public bool OutputAvailable() => this.OutputAvailable(CancellationToken.None);

  public IEnumerable<T> GetConsumingEnumerable(CancellationToken cancellationToken)
  {
    while (true)
    {
      T consuming;
      try
      {
        consuming = this.Take(cancellationToken);
      }
      catch (InvalidOperationException ex)
      {
        break;
      }
      yield return consuming;
    }
  }

  public IEnumerable<T> GetConsumingEnumerable()
  {
    return this.GetConsumingEnumerable(CancellationToken.None);
  }

  private async Task<T> DoTakeAsync(CancellationToken cancellationToken, bool sync)
  {
    IDisposable disposable;
    if (sync)
      disposable = this._mutex.Lock();
    else
      disposable = await this._mutex.LockAsync().ConfigureAwait(false);
    T async;
    using (disposable)
    {
      while (this.Empty && !this._completed)
      {
        if (sync)
          this._completedOrNotEmpty.Wait(cancellationToken);
        else
          await this._completedOrNotEmpty.WaitAsync(cancellationToken).ConfigureAwait(false);
      }
      if (this._completed && this.Empty)
        throw new InvalidOperationException("Take failed; the producer/consumer collection has completed adding and is empty.");
      T obj;
      if (!this._collection.TryTake(out obj))
        throw new InvalidOperationException("Take failed; the take from the underlying collection failed.");
      this._completedOrNotFull.Notify();
      async = obj;
    }
    return async;
  }

  public Task<T> TakeAsync(CancellationToken cancellationToken)
  {
    return this.DoTakeAsync(cancellationToken, false);
  }

  public T Take(CancellationToken cancellationToken)
  {
    return this.DoTakeAsync(cancellationToken, true).WaitAndUnwrapException<T>();
  }

  public Task<T> TakeAsync() => this.TakeAsync(CancellationToken.None);

  public T Take() => this.Take(CancellationToken.None);

  [DebuggerNonUserCode]
  internal sealed class DebugView
  {
    private readonly AsyncCollection<T> _collection;

    public DebugView(AsyncCollection<T> collection) => this._collection = collection;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public T[] Items => this._collection._collection.ToArray();
  }
}
