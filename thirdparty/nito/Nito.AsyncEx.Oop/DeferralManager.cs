// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.DeferralManager
// Assembly: Nito.AsyncEx.Oop, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15B42D27-4022-4533-A6B4-AA9F1FAB0E34
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Oop.dll

using Nito.Disposables;
using System;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

public sealed class DeferralManager
{
  private readonly IDeferralSource _source;
  private readonly object _mutex;
  private AsyncCountdownEvent? _ce;

  public DeferralManager()
  {
    this._source = (IDeferralSource) new DeferralManager.ManagedDeferralSource(this);
    this._mutex = new object();
  }

  internal void IncrementCount()
  {
    lock (this._mutex)
    {
      if (this._ce == null)
        this._ce = new AsyncCountdownEvent(1L);
      else
        this._ce.AddCount();
    }
  }

  internal void DecrementCount()
  {
    if (this._ce == null)
      throw new InvalidOperationException("You must call IncrementCount before calling DecrementCount.");
    this._ce.Signal();
  }

  public IDeferralSource DeferralSource => this._source;

  public Task WaitForDeferralsAsync()
  {
    lock (this._mutex)
      return this._ce == null ? TaskConstants.Completed : this._ce.WaitAsync();
  }

  private sealed class ManagedDeferralSource : IDeferralSource
  {
    private readonly DeferralManager _manager;

    public ManagedDeferralSource(DeferralManager manager) => this._manager = manager;

    IDisposable IDeferralSource.GetDeferral()
    {
      this._manager.IncrementCount();
      return (IDisposable) new DeferralManager.ManagedDeferralSource.Deferral(this._manager);
    }

    private sealed class Deferral(DeferralManager manager) : SingleDisposable<DeferralManager>(manager)
    {
      protected override void Dispose(DeferralManager context) => context.DecrementCount();
    }
  }
}
