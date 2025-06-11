// Decompiled with JetBrains decompiler
// Type: NLog.NestedDiagnosticsContext
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NLog;

public static class NestedDiagnosticsContext
{
  private static readonly object dataSlot = ThreadLocalStorageHelper.AllocateDataSlot();

  public static string TopMessage
  {
    get => FormatHelper.ConvertToString(NestedDiagnosticsContext.TopObject, (IFormatProvider) null);
  }

  public static object TopObject => NestedDiagnosticsContext.PeekObject();

  private static Stack<object> GetThreadStack(bool create = true)
  {
    return ThreadLocalStorageHelper.GetDataForSlot<Stack<object>>(NestedDiagnosticsContext.dataSlot, create);
  }

  public static IDisposable Push(string text) => NestedDiagnosticsContext.Push((object) text);

  public static IDisposable Push(object value)
  {
    Stack<object> threadStack = NestedDiagnosticsContext.GetThreadStack();
    int count = threadStack.Count;
    threadStack.Push(value);
    return (IDisposable) new NestedDiagnosticsContext.StackPopper(threadStack, count);
  }

  public static string Pop() => NestedDiagnosticsContext.Pop((IFormatProvider) null);

  public static string Pop(IFormatProvider formatProvider)
  {
    return FormatHelper.ConvertToString(NestedDiagnosticsContext.PopObject() ?? (object) string.Empty, formatProvider);
  }

  public static object PopObject()
  {
    Stack<object> threadStack = NestedDiagnosticsContext.GetThreadStack();
    return threadStack.Count <= 0 ? (object) null : threadStack.Pop();
  }

  public static object PeekObject()
  {
    Stack<object> threadStack = NestedDiagnosticsContext.GetThreadStack(false);
    // ISSUE: explicit non-virtual call
    return threadStack == null || __nonvirtual (threadStack.Count) <= 0 ? (object) null : threadStack.Peek();
  }

  public static void Clear() => NestedDiagnosticsContext.GetThreadStack(false)?.Clear();

  public static string[] GetAllMessages()
  {
    return NestedDiagnosticsContext.GetAllMessages((IFormatProvider) null);
  }

  public static string[] GetAllMessages(IFormatProvider formatProvider)
  {
    Stack<object> threadStack = NestedDiagnosticsContext.GetThreadStack(false);
    return threadStack == null ? ArrayHelper.Empty<string>() : threadStack.Select<object, string>((Func<object, string>) (o => FormatHelper.ConvertToString(o, formatProvider))).ToArray<string>();
  }

  public static object[] GetAllObjects()
  {
    Stack<object> threadStack = NestedDiagnosticsContext.GetThreadStack(false);
    return threadStack == null ? ArrayHelper.Empty<object>() : threadStack.ToArray();
  }

  private sealed class StackPopper : IDisposable
  {
    private readonly Stack<object> _stack;
    private readonly int _previousCount;

    public StackPopper(Stack<object> stack, int previousCount)
    {
      this._stack = stack;
      this._previousCount = previousCount;
    }

    void IDisposable.Dispose()
    {
      while (this._stack.Count > this._previousCount)
        this._stack.Pop();
    }
  }
}
