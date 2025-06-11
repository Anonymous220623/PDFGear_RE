// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.ComparerGenerator
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Collections.Generic;

#nullable disable
namespace HandyControl.Tools;

public class ComparerGenerator
{
  private static readonly Dictionary<Type, ComparerGenerator.ComparerTypeCode> TypeCodeDic = new Dictionary<Type, ComparerGenerator.ComparerTypeCode>()
  {
    [typeof (DateTimeRange)] = ComparerGenerator.ComparerTypeCode.DateTimeRange
  };

  public static IComparer<T> GetComparer<T>()
  {
    ComparerGenerator.ComparerTypeCode comparerTypeCode;
    if (!ComparerGenerator.TypeCodeDic.TryGetValue(typeof (T), out comparerTypeCode))
      return (IComparer<T>) null;
    return comparerTypeCode == ComparerGenerator.ComparerTypeCode.DateTimeRange ? (IComparer<T>) new DateTimeRangeComparer() : (IComparer<T>) null;
  }

  private enum ComparerTypeCode
  {
    DateTimeRange,
  }
}
