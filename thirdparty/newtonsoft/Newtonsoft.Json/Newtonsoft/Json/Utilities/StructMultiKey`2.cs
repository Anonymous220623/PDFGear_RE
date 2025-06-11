// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.StructMultiKey`2
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;

#nullable enable
namespace Newtonsoft.Json.Utilities;

internal readonly struct StructMultiKey<T1, T2>(T1 v1, T2 v2) : IEquatable<StructMultiKey<T1, T2>>
{
  public readonly T1 Value1 = v1;
  public readonly T2 Value2 = v2;

  public override int GetHashCode()
  {
    T1 obj1 = this.Value1;
    ref T1 local1 = ref obj1;
    int hashCode1 = (object) local1 != null ? local1.GetHashCode() : 0;
    T2 obj2 = this.Value2;
    ref T2 local2 = ref obj2;
    int hashCode2 = (object) local2 != null ? local2.GetHashCode() : 0;
    return hashCode1 ^ hashCode2;
  }

  public override bool Equals(object obj)
  {
    return obj is StructMultiKey<T1, T2> other && this.Equals(other);
  }

  public bool Equals(StructMultiKey<T1, T2> other)
  {
    return object.Equals((object) this.Value1, (object) other.Value1) && object.Equals((object) this.Value2, (object) other.Value2);
  }
}
