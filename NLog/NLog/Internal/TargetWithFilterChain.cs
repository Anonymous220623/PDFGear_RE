// Decompiled with JetBrains decompiler
// Type: NLog.Internal.TargetWithFilterChain
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Filters;
using NLog.Targets;
using System.Collections.Generic;

#nullable disable
namespace NLog.Internal;

[NLogConfigurationItem]
internal class TargetWithFilterChain
{
  private StackTraceUsage? _stackTraceUsage;

  public TargetWithFilterChain(
    Target target,
    IList<Filter> filterChain,
    FilterResult defaultResult)
  {
    this.Target = target;
    this.FilterChain = filterChain;
    this.DefaultResult = defaultResult;
  }

  public Target Target { get; }

  public IList<Filter> FilterChain { get; }

  public FilterResult DefaultResult { get; }

  public TargetWithFilterChain NextInChain { get; set; }

  public StackTraceUsage GetStackTraceUsage() => this._stackTraceUsage ?? StackTraceUsage.None;

  internal StackTraceUsage PrecalculateStackTraceUsage()
  {
    StackTraceUsage stackTraceUsage1 = StackTraceUsage.None;
    if (this.Target != null)
      stackTraceUsage1 = this.Target.StackTraceUsage;
    if (this.NextInChain != null && stackTraceUsage1 != StackTraceUsage.WithSource)
    {
      StackTraceUsage stackTraceUsage2 = this.NextInChain.PrecalculateStackTraceUsage();
      if (stackTraceUsage2 > stackTraceUsage1)
        stackTraceUsage1 = stackTraceUsage2;
    }
    this._stackTraceUsage = new StackTraceUsage?(stackTraceUsage1);
    return stackTraceUsage1;
  }
}
