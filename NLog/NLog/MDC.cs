// Decompiled with JetBrains decompiler
// Type: NLog.MDC
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog;

[Obsolete("Use MappedDiagnosticsContext class instead. Marked obsolete on NLog 2.0")]
public static class MDC
{
  public static void Set(string item, string value) => MappedDiagnosticsContext.Set(item, value);

  public static string Get(string item) => MappedDiagnosticsContext.Get(item);

  public static object GetObject(string item) => MappedDiagnosticsContext.GetObject(item);

  public static bool Contains(string item) => MappedDiagnosticsContext.Contains(item);

  public static void Remove(string item) => MappedDiagnosticsContext.Remove(item);

  public static void Clear() => MappedDiagnosticsContext.Clear();
}
