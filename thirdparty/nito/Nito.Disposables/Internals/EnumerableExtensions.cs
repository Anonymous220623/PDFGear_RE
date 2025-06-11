// Decompiled with JetBrains decompiler
// Type: Nito.Disposables.Internals.EnumerableExtensions
// Assembly: Nito.Disposables, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2B43B0E-C24B-48AC-BE52-0652CD6F6684
// Assembly location: D:\PDFGear\bin\Nito.Disposables.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Nito.Disposables.Internals;

public static class EnumerableExtensions
{
  public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : class
  {
    return source.Where<T>((Func<T, bool>) (x => (object) x != null));
  }
}
