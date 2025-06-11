// Decompiled with JetBrains decompiler
// Type: Nito.Disposables.Disposable
// Assembly: Nito.Disposables, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2B43B0E-C24B-48AC-BE52-0652CD6F6684
// Assembly location: D:\PDFGear\bin\Nito.Disposables.dll

using System;

#nullable enable
namespace Nito.Disposables;

public sealed class Disposable(Action? dispose) : SingleDisposable<Action>(dispose)
{
  protected override void Dispose(Action? context)
  {
    if (context == null)
      return;
    context();
  }

  public void Add(Action? dispose)
  {
    if (dispose == null || this.TryUpdateContext((Func<Action, Action>) (x => x + dispose)))
      return;
    this.Dispose();
    dispose();
  }

  public static Disposable Create(Action? dispose) => new Disposable(dispose);
}
