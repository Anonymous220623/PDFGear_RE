// Decompiled with JetBrains decompiler
// Type: Nito.Disposables.NoopDisposable
// Assembly: Nito.Disposables, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2B43B0E-C24B-48AC-BE52-0652CD6F6684
// Assembly location: D:\PDFGear\bin\Nito.Disposables.dll

using System;

#nullable enable
namespace Nito.Disposables;

public sealed class NoopDisposable : IDisposable
{
  private NoopDisposable()
  {
  }

  public void Dispose()
  {
  }

  public static NoopDisposable Instance { get; } = new NoopDisposable();
}
