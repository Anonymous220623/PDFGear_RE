// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.LinqExtensions
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace PDFKit.Contents;

internal static class LinqExtensions
{
  internal static int IndexOf<T>(this IReadOnlyList<T> readonlyList, T obj)
  {
    switch (readonlyList)
    {
      case T[] array:
        return Array.IndexOf<T>(array, obj);
      case List<T> objList:
        return objList.IndexOf(obj);
      default:
        for (int index = 0; index < readonlyList.Count; ++index)
        {
          if (object.Equals((object) readonlyList[index], (object) obj))
            return index;
        }
        return -1;
    }
  }

  internal static int IndexOf<T>(this IReadOnlyList<T> readonlyList, Func<T, bool> predicate)
  {
    for (int index = 0; index < readonlyList.Count; ++index)
    {
      if (predicate(readonlyList[index]))
        return index;
    }
    return -1;
  }
}
