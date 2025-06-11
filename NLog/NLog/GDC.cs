// Decompiled with JetBrains decompiler
// Type: NLog.GDC
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog;

[Obsolete("Use GlobalDiagnosticsContext class instead. Marked obsolete on NLog 2.0")]
public static class GDC
{
  public static void Set(string item, string value) => GlobalDiagnosticsContext.Set(item, value);

  public static string Get(string item) => GlobalDiagnosticsContext.Get(item);

  public static string Get(string item, IFormatProvider formatProvider)
  {
    return GlobalDiagnosticsContext.Get(item, formatProvider);
  }

  public static object GetObject(string item) => GlobalDiagnosticsContext.GetObject(item);

  public static bool Contains(string item) => GlobalDiagnosticsContext.Contains(item);

  public static void Remove(string item) => GlobalDiagnosticsContext.Remove(item);

  public static void Clear() => GlobalDiagnosticsContext.Clear();
}
