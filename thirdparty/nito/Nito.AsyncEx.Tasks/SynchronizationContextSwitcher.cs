// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.SynchronizationContextSwitcher
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using Nito.Disposables;
using System;
using System.Threading;

#nullable enable
namespace Nito.AsyncEx;

public sealed class SynchronizationContextSwitcher : SingleDisposable<object>
{
  private readonly SynchronizationContext? _oldContext;

  private SynchronizationContextSwitcher(SynchronizationContext? newContext)
    : base(new object())
  {
    this._oldContext = SynchronizationContext.Current;
    SynchronizationContext.SetSynchronizationContext(newContext);
  }

  protected override void Dispose(object context)
  {
    SynchronizationContext.SetSynchronizationContext(this._oldContext);
  }

  public static void NoContext(Action action)
  {
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    using (new SynchronizationContextSwitcher((SynchronizationContext) null))
      action();
  }

  public static T NoContext<T>(Func<T> action)
  {
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    using (new SynchronizationContextSwitcher((SynchronizationContext) null))
      return action();
  }

  public static void ApplyContext(SynchronizationContext context, Action action)
  {
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    using (new SynchronizationContextSwitcher(context))
      action();
  }

  public static T ApplyContext<T>(SynchronizationContext context, Func<T> action)
  {
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    using (new SynchronizationContextSwitcher(context))
      return action();
  }
}
