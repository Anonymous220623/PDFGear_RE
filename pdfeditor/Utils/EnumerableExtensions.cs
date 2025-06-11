// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.EnumerableExtensions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Collections.Generic;

#nullable disable
namespace pdfeditor.Utils;

public static class EnumerableExtensions
{
  public static void Deconstruct<TKey, TValue>(
    this KeyValuePair<TKey, TValue> pair,
    out TKey key,
    out TValue value)
  {
    key = pair.Key;
    value = pair.Value;
  }
}
