// Decompiled with JetBrains decompiler
// Type: NLog.ISuppress
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Threading.Tasks;

#nullable disable
namespace NLog;

public interface ISuppress
{
  void Swallow(Action action);

  T Swallow<T>(Func<T> func);

  T Swallow<T>(Func<T> func, T fallback);

  void Swallow(Task task);

  Task SwallowAsync(Task task);

  Task SwallowAsync(Func<Task> asyncAction);

  Task<TResult> SwallowAsync<TResult>(Func<Task<TResult>> asyncFunc);

  Task<TResult> SwallowAsync<TResult>(Func<Task<TResult>> asyncFunc, TResult fallback);
}
