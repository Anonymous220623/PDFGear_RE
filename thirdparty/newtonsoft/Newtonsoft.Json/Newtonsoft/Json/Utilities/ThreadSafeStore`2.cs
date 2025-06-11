// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ThreadSafeStore`2
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;
using System.Collections.Concurrent;

#nullable enable
namespace Newtonsoft.Json.Utilities;

internal class ThreadSafeStore<TKey, TValue>
{
  private readonly ConcurrentDictionary<TKey, TValue> _concurrentStore;
  private readonly Func<TKey, TValue> _creator;

  public ThreadSafeStore(Func<TKey, TValue> creator)
  {
    ValidationUtils.ArgumentNotNull((object) creator, nameof (creator));
    this._creator = creator;
    this._concurrentStore = new ConcurrentDictionary<TKey, TValue>();
  }

  public TValue Get(TKey key) => this._concurrentStore.GetOrAdd(key, this._creator);
}
