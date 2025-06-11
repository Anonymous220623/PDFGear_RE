// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AwaitableDisposable`1
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

public readonly struct AwaitableDisposable<T>(Task<T> task) where T : IDisposable
{
  private readonly Task<T> _task = task != null ? task : throw new ArgumentNullException(nameof (task));

  public Task<T> AsTask() => this._task;

  public static implicit operator Task<T>(AwaitableDisposable<T> source) => source.AsTask();

  public TaskAwaiter<T> GetAwaiter() => this._task.GetAwaiter();

  public ConfiguredTaskAwaitable<T> ConfigureAwait(bool continueOnCapturedContext)
  {
    return this._task.ConfigureAwait(continueOnCapturedContext);
  }
}
