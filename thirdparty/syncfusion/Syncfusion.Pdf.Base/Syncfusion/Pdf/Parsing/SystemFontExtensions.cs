// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontExtensions
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal static class SystemFontExtensions
{
  private const int BUFFER_SIZE = 1024 /*0x0400*/;

  public static T1 FindElement<T1, T2>(
    IEnumerable<T1> collection,
    T2 index,
    SystemFontCompareDelegate<T1, T2> comparer)
    where T1 : class
  {
    return SystemFontEnumerable.Count<T1>(collection) == 0 ? default (T1) : SystemFontExtensions.FindElement<T1, T2>(collection, 0, SystemFontEnumerable.Count<T1>(collection) - 1, index, comparer);
  }

  internal static byte[] ReadAllBytes(Stream reader)
  {
    if (!reader.CanRead)
      return (byte[]) null;
    if (reader.CanSeek)
      reader.Seek(0L, SeekOrigin.Begin);
    List<byte> byteList = new List<byte>();
    byte[] buffer = new byte[1024 /*0x0400*/];
    int num;
    while ((num = reader.Read(buffer, 0, 1024 /*0x0400*/)) > 0)
    {
      for (int index = 0; index < num; ++index)
        byteList.Add(buffer[index]);
    }
    return byteList.ToArray();
  }

  private static T1 FindElement<T1, T2>(
    IEnumerable<T1> collection,
    int lo,
    int hi,
    T2 element,
    SystemFontCompareDelegate<T1, T2> comparer)
    where T1 : class
  {
    if (hi < lo)
      return SystemFontEnumerable.ElementAt<T1>(collection, Math.Max(0, Math.Min(hi, SystemFontEnumerable.Count<T1>(collection))));
    int index = lo + (hi - lo) / 2;
    T1 left = SystemFontEnumerable.ElementAt<T1>(collection, index);
    int num = comparer(left, element);
    if (num == 0)
      return left;
    return num > 0 ? SystemFontExtensions.FindElement<T1, T2>(collection, index + 1, hi, element, comparer) : SystemFontExtensions.FindElement<T1, T2>(collection, lo, index - 1, element, comparer);
  }
}
