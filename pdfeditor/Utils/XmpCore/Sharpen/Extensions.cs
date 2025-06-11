// Decompiled with JetBrains decompiler
// Type: Sharpen.Extensions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Collections.Generic;

#nullable disable
namespace Sharpen;

public static class Extensions
{
  public static Sharpen.Iterator<T> Iterator<T>(this IEnumerable<T> col)
  {
    return (Sharpen.Iterator<T>) new EnumeratorWrapper<T>((object) col, col.GetEnumerator());
  }
}
