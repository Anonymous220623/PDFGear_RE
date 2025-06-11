// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncProducerConsumerQueue`1
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

[DebuggerDisplay("Count = {_queue.Count}, MaxCount = {_maxCount}")]
[DebuggerTypeProxy(typeof (AsyncProducerConsumerQueue<>.DebugView))]
public sealed class AsyncProducerConsumerQueue<T>
{
  private readonly Queue<T> _queue;
  private readonly int _maxCount;
  private readonly AsyncLock _mutex;
  private readonly AsyncConditionVariable _completedOrNotFull;
  private readonly AsyncConditionVariable _completedOrNotEmpty;
  private bool _completed;

  public AsyncProducerConsumerQueue(IEnumerable<T>? collection, int maxCount)
  {
    if (maxCount <= 0)
      throw new ArgumentOutOfRangeException(nameof (maxCount), "The maximum count must be greater than zero.");
    this._queue = collection == null ? new Queue<T>() : new Queue<T>(collection);
    if (maxCount < this._queue.Count)
      throw new ArgumentException("The maximum count cannot be less than the number of elements in the collection.", nameof (maxCount));
    this._maxCount = maxCount;
    this._mutex = new AsyncLock();
    this._completedOrNotFull = new AsyncConditionVariable(this._mutex);
    this._completedOrNotEmpty = new AsyncConditionVariable(this._mutex);
  }

  public AsyncProducerConsumerQueue(IEnumerable<T>? collection)
    : this(collection, int.MaxValue)
  {
  }

  public AsyncProducerConsumerQueue(int maxCount)
    : this((IEnumerable<T>) null, maxCount)
  {
  }

  public AsyncProducerConsumerQueue()
    : this((IEnumerable<T>) null, int.MaxValue)
  {
  }

  private bool Empty => this._queue.Count == 0;

  private bool Full => this._queue.Count == this._maxCount;

  public void CompleteAdding()
  {
    using (this._mutex.Lock())
    {
      this._completed = true;
      this._completedOrNotEmpty.NotifyAll();
      this._completedOrNotFull.NotifyAll();
    }
  }

  private async Task DoEnqueueAsync(T item, CancellationToken cancellationToken, bool sync)
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
        throw new InvalidOperationException("Enqueue failed; the producer/consumer queue has completed adding.");
      this._queue.Enqueue(item);
      this._completedOrNotEmpty.Notify();
    }
  }

  public Task EnqueueAsync(T item, CancellationToken cancellationToken)
  {
    return this.DoEnqueueAsync(item, cancellationToken, false);
  }

  public Task EnqueueAsync(T item) => this.EnqueueAsync(item, CancellationToken.None);

  public void Enqueue(T item, CancellationToken cancellationToken)
  {
    this.DoEnqueueAsync(item, cancellationToken, true).WaitAndUnwrapException(CancellationToken.None);
  }

  public void Enqueue(T item) => this.Enqueue(item, CancellationToken.None);

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
      Tuple<bool, T> tuple = this.TryDoDequeueAsync(cancellationToken, true).WaitAndUnwrapException<Tuple<bool, T>>();
      if (tuple.Item1)
        yield return tuple.Item2;
      else
        break;
    }
  }

  public IEnumerable<T> GetConsumingEnumerable()
  {
    return this.GetConsumingEnumerable(CancellationToken.None);
  }

  private async Task<Tuple<bool, T>> TryDoDequeueAsync(
    CancellationToken cancellationToken,
    bool sync)
  {
    IDisposable disposable;
    if (sync)
      disposable = this._mutex.Lock();
    else
      disposable = await this._mutex.LockAsync().ConfigureAwait(false);
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
        return Tuple.Create<bool, T>(false, default (T));
      T obj = this._queue.Dequeue();
      this._completedOrNotFull.Notify();
      return Tuple.Create<bool, T>(true, obj);
    }
  }

  private async Task<T> DoDequeueAsync(CancellationToken cancellationToken, bool sync)
  {
    Tuple<bool, T> tuple = await this.TryDoDequeueAsync(cancellationToken, sync).ConfigureAwait(false);
    return tuple.Item1 ? tuple.Item2 : throw new InvalidOperationException("Dequeue failed; the producer/consumer queue has completed adding and is empty.");
  }

  public Task<T> DequeueAsync(CancellationToken cancellationToken)
  {
    return this.DoDequeueAsync(cancellationToken, false);
  }

  public Task<T> DequeueAsync() => this.DequeueAsync(CancellationToken.None);

  public T Dequeue(CancellationToken cancellationToken)
  {
    return this.DoDequeueAsync(cancellationToken, true).WaitAndUnwrapException<T>();
  }

  public T Dequeue() => this.Dequeue(CancellationToken.None);

  [DebuggerNonUserCode]
  internal sealed class DebugView
  {
    private readonly AsyncProducerConsumerQueue<T> _queue;

    public DebugView(AsyncProducerConsumerQueue<T> queue) => this._queue = queue;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public T[] Items => this._queue._queue.ToArray();
  }
}
