// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.WrapperTargetBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using System;

#nullable disable
namespace NLog.Targets.Wrappers;

public abstract class WrapperTargetBase : Target
{
  [RequiredParameter]
  public Target WrappedTarget { get; set; }

  public override string ToString() => $"{base.ToString()}({this.WrappedTarget})";

  protected override void FlushAsync(AsyncContinuation asyncContinuation)
  {
    if (this.WrappedTarget != null)
      this.WrappedTarget.Flush(asyncContinuation);
    else
      asyncContinuation((Exception) null);
  }

  protected sealed override void Write(LogEventInfo logEvent)
  {
    throw new NotSupportedException("This target must not be invoked in a synchronous way.");
  }
}
