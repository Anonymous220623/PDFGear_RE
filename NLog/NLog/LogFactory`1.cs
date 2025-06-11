// Decompiled with JetBrains decompiler
// Type: NLog.LogFactory`1
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
namespace NLog;

public class LogFactory<T> : LogFactory where T : Logger
{
  public T GetLogger(string name) => (T) this.GetLogger(name, typeof (T));

  [MethodImpl(MethodImplOptions.NoInlining)]
  public T GetCurrentClassLogger()
  {
    return this.GetLogger(StackTraceUsageUtils.GetClassFullName(new StackFrame(1, false)));
  }
}
