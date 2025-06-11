// Decompiled with JetBrains decompiler
// Type: LruCacheNet.Node`2
// Assembly: LruCacheNet, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0571EC11-8C09-445B-9A07-07D97ABEC1CB
// Assembly location: D:\PDFGear\bin\LruCacheNet.dll

using System;

#nullable disable
namespace LruCacheNet;

public sealed class Node<TKey, TValue>
{
  internal Node(TKey key, TValue data)
  {
    if ((object) key == null)
      throw new ArgumentException("Key cannot be null", nameof (key));
    this.Value = (object) data != null ? data : throw new ArgumentException("Data cannot be null", nameof (data));
    this.Key = key;
    this.Next = (Node<TKey, TValue>) null;
    this.Previous = (Node<TKey, TValue>) null;
  }

  public TValue Value { get; set; }

  public TKey Key { get; set; }

  public Node<TKey, TValue> Next { get; set; }

  public Node<TKey, TValue> Previous { get; set; }

  public override string ToString()
  {
    return $"Key:{this.Key} Data:{this.Value.ToString()} Previous:{this.GetNodeSummary(this.Previous)} Next:{this.GetNodeSummary(this.Next)}";
  }

  private string GetNodeSummary(Node<TKey, TValue> node) => node == null ? "Null" : "Set";
}
