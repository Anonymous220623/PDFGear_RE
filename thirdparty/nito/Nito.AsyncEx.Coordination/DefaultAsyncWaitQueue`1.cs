// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.DefaultAsyncWaitQueue`1
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using Nito.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof (DefaultAsyncWaitQueue<>.DebugView))]
internal sealed class DefaultAsyncWaitQueue<T> : IAsyncWaitQueue<T>
{
  private readonly Deque<TaskCompletionSource<T>> _queue = new Deque<TaskCompletionSource<T>>();

  private int Count => this._queue.Count;

  bool IAsyncWaitQueue<T>.IsEmpty => this.Count == 0;

  Task<T> IAsyncWaitQueue<T>.Enqueue()
  {
    TaskCompletionSource<T> asyncTaskSource = TaskCompletionSourceExtensions.CreateAsyncTaskSource<T>();
    this._queue.AddToBack(asyncTaskSource);
    return asyncTaskSource.Task;
  }

  void IAsyncWaitQueue<T>.Dequeue(T result) => this._queue.RemoveFromFront().TrySetResult(result);

  void IAsyncWaitQueue<T>.DequeueAll(T result)
  {
    foreach (TaskCompletionSource<T> completionSource in this._queue)
      completionSource.TrySetResult(result);
    this._queue.Clear();
  }

  bool IAsyncWaitQueue<T>.TryCancel(Task task, CancellationToken cancellationToken)
  {
    for (int index = 0; index != this._queue.Count; ++index)
    {
      if (this._queue[index].Task == task)
      {
        this._queue[index].TrySetCanceled(cancellationToken);
        this._queue.RemoveAt(index);
        return true;
      }
    }
    return false;
  }

  void IAsyncWaitQueue<T>.CancelAll(CancellationToken cancellationToken)
  {
    foreach (TaskCompletionSource<T> completionSource in this._queue)
      completionSource.TrySetCanceled(cancellationToken);
    this._queue.Clear();
  }

  [DebuggerNonUserCode]
  internal sealed class DebugView
  {
    private readonly DefaultAsyncWaitQueue<T> _queue;

    public DebugView(DefaultAsyncWaitQueue<T> queue) => this._queue = queue;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public Task<T>[] Tasks
    {
      get
      {
        List<Task<T>> taskList = new List<Task<T>>(this._queue._queue.Count);
        foreach (TaskCompletionSource<T> completionSource in this._queue._queue)
          taskList.Add(completionSource.Task);
        return taskList.ToArray();
      }
    }
  }
}
