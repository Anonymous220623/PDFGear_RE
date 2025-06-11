// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncLazy`1
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable enable
namespace Nito.AsyncEx;

[DebuggerDisplay("Id = {Id}, State = {GetStateForDebugger}")]
[DebuggerTypeProxy(typeof (AsyncLazy<>.DebugView))]
public sealed class AsyncLazy<T>
{
  private readonly object _mutex;
  private readonly Func<System.Threading.Tasks.Task<T>> _factory;
  private Lazy<System.Threading.Tasks.Task<T>> _instance;
  private int _id;

  [DebuggerNonUserCode]
  internal AsyncLazy<
  #nullable disable
  T>.LazyState GetStateForDebugger
  {
    get
    {
      if (!this._instance.IsValueCreated)
        return AsyncLazy<T>.LazyState.NotStarted;
      return !this._instance.Value.IsCompleted ? AsyncLazy<T>.LazyState.Executing : AsyncLazy<T>.LazyState.Completed;
    }
  }

  public AsyncLazy(
  #nullable enable
  Func<System.Threading.Tasks.Task<T>> factory, AsyncLazyFlags flags = AsyncLazyFlags.None)
  {
    this._factory = factory != null ? factory : throw new ArgumentNullException(nameof (factory));
    if ((flags & AsyncLazyFlags.RetryOnFailure) == AsyncLazyFlags.RetryOnFailure)
      this._factory = this.RetryOnFailure(this._factory);
    if ((flags & AsyncLazyFlags.ExecuteOnCallingThread) != AsyncLazyFlags.ExecuteOnCallingThread)
      this._factory = AsyncLazy<T>.RunOnThreadPool(this._factory);
    this._mutex = new object();
    this._instance = new Lazy<System.Threading.Tasks.Task<T>>(this._factory);
  }

  public int Id => IdManager<AsyncLazy<object>>.GetId(ref this._id);

  public bool IsStarted
  {
    get
    {
      lock (this._mutex)
        return this._instance.IsValueCreated;
    }
  }

  public System.Threading.Tasks.Task<T> Task
  {
    get
    {
      lock (this._mutex)
        return this._instance.Value;
    }
  }

  private Func<System.Threading.Tasks.Task<T>> RetryOnFailure(Func<System.Threading.Tasks.Task<T>> factory)
  {
    return (Func<System.Threading.Tasks.Task<T>>) (async () =>
    {
      T obj;
      try
      {
        obj = await factory().ConfigureAwait(false);
      }
      catch
      {
        lock (this._mutex)
          this._instance = new Lazy<System.Threading.Tasks.Task<T>>(this._factory);
        throw;
      }
      return obj;
    });
  }

  private static Func<System.Threading.Tasks.Task<T>> RunOnThreadPool(Func<System.Threading.Tasks.Task<T>> factory)
  {
    return (Func<System.Threading.Tasks.Task<T>>) (() => System.Threading.Tasks.Task.Run<T>(factory));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public TaskAwaiter<T> GetAwaiter() => this.Task.GetAwaiter();

  public ConfiguredTaskAwaitable<T> ConfigureAwait(bool continueOnCapturedContext)
  {
    return this.Task.ConfigureAwait(continueOnCapturedContext);
  }

  public void Start()
  {
    System.Threading.Tasks.Task<T> task = this.Task;
  }

  internal enum LazyState
  {
    NotStarted,
    Executing,
    Completed,
  }

  [DebuggerNonUserCode]
  internal sealed class DebugView
  {
    private readonly AsyncLazy<T> _lazy;

    public DebugView(AsyncLazy<T> lazy) => this._lazy = lazy;

    public AsyncLazy<
    #nullable disable
    T>.LazyState State => this._lazy.GetStateForDebugger;

    public 
    #nullable enable
    System.Threading.Tasks.Task Task
    {
      get
      {
        if (!this._lazy._instance.IsValueCreated)
          throw new InvalidOperationException("Not yet created.");
        return (System.Threading.Tasks.Task) this._lazy._instance.Value;
      }
    }

    public T Value
    {
      get
      {
        if (!this._lazy._instance.IsValueCreated || !this._lazy._instance.Value.IsCompleted)
          throw new InvalidOperationException("Not yet created.");
        return this._lazy._instance.Value.Result;
      }
    }
  }
}
