// Decompiled with JetBrains decompiler
// Type: Nito.Disposables.Internals.BoundActionField`1
// Assembly: Nito.Disposables, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2B43B0E-C24B-48AC-BE52-0652CD6F6684
// Assembly location: D:\PDFGear\bin\Nito.Disposables.dll

using System;
using System.Threading;

#nullable enable
namespace Nito.Disposables.Internals;

public sealed class BoundActionField<T>
{
  private BoundActionField<
  #nullable disable
  T>.BoundAction
  #nullable enable
  ? _field;

  public BoundActionField(Action<T> action, T context)
  {
    this._field = new BoundActionField<T>.BoundAction(action, context);
  }

  public bool IsEmpty
  {
    get
    {
      return Interlocked.CompareExchange<BoundActionField<T>.BoundAction>(ref this._field, (BoundActionField<T>.BoundAction) null, (BoundActionField<T>.BoundAction) null) == null;
    }
  }

  public BoundActionField<
  #nullable disable
  T>.IBoundAction
  #nullable enable
  ? TryGetAndUnset()
  {
    return (BoundActionField<T>.IBoundAction) Interlocked.Exchange<BoundActionField<T>.BoundAction>(ref this._field, (BoundActionField<T>.BoundAction) null);
  }

  public bool TryUpdateContext(Func<T, T> contextUpdater)
  {
    if (contextUpdater == null)
      throw new ArgumentNullException(nameof (contextUpdater));
    BoundActionField<T>.BoundAction boundAction1;
    BoundActionField<T>.BoundAction boundAction2;
    do
    {
      boundAction1 = Interlocked.CompareExchange<BoundActionField<T>.BoundAction>(ref this._field, this._field, this._field);
      if (boundAction1 == null)
        return false;
      boundAction2 = Interlocked.CompareExchange<BoundActionField<T>.BoundAction>(ref this._field, new BoundActionField<T>.BoundAction(boundAction1, contextUpdater), boundAction1);
    }
    while (boundAction1 != boundAction2);
    return true;
  }

  public interface IBoundAction
  {
    void Invoke();
  }

  private sealed class BoundAction : BoundActionField<
  #nullable disable
  T>.IBoundAction
  {
    private readonly 
    #nullable enable
    Action<T> _action;
    private readonly T _context;

    public BoundAction(Action<T> action, T context)
    {
      this._action = action;
      this._context = context;
    }

    public BoundAction(
      BoundActionField<
      #nullable disable
      T>.BoundAction originalBoundAction,
      #nullable enable
      Func<T, T> contextUpdater)
    {
      this._action = originalBoundAction._action;
      this._context = contextUpdater(originalBoundAction._context);
    }

    public void Invoke()
    {
      Action<T> action = this._action;
      if (action == null)
        return;
      action(this._context);
    }
  }
}
