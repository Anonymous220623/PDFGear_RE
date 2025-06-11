// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.AutoFlushTargetWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Conditions;
using NLog.Internal;
using System;

#nullable disable
namespace NLog.Targets.Wrappers;

[Target("AutoFlushWrapper", IsWrapper = true)]
public class AutoFlushTargetWrapper : WrapperTargetBase
{
  private bool? _asyncFlush;
  private readonly AsyncOperationCounter _pendingManualFlushList = new AsyncOperationCounter();

  public ConditionExpression Condition { get; set; }

  public bool AsyncFlush
  {
    get => this._asyncFlush ?? true;
    set => this._asyncFlush = new bool?(value);
  }

  public bool FlushOnConditionOnly { get; set; }

  public AutoFlushTargetWrapper()
    : this((Target) null)
  {
  }

  public AutoFlushTargetWrapper(string name, Target wrappedTarget)
    : this(wrappedTarget)
  {
    this.Name = name;
  }

  public AutoFlushTargetWrapper(Target wrappedTarget) => this.WrappedTarget = wrappedTarget;

  protected override void InitializeTarget()
  {
    base.InitializeTarget();
    if (this._asyncFlush.HasValue || AutoFlushTargetWrapper.TargetSupportsAsyncFlush(this.WrappedTarget))
      return;
    this.AsyncFlush = false;
  }

  private static bool TargetSupportsAsyncFlush(Target wrappedTarget)
  {
    switch (wrappedTarget)
    {
      case BufferingTargetWrapper _:
        return false;
      case AsyncTaskTarget _:
        return false;
      default:
        return true;
    }
  }

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    if (this.Condition == null || this.Condition.Evaluate(logEvent.LogEvent).Equals((object) true))
    {
      if (this.AsyncFlush)
      {
        AsyncContinuation currentContinuation = logEvent.Continuation;
        AsyncContinuation asyncContinuation = (AsyncContinuation) (ex =>
        {
          if (ex == null)
            this.FlushOnCondition();
          this._pendingManualFlushList.CompleteOperation(ex);
          currentContinuation(ex);
        });
        this._pendingManualFlushList.BeginOperation();
        this.WrappedTarget.WriteAsyncLogEvent(logEvent.LogEvent.WithContinuation(asyncContinuation));
      }
      else
      {
        this.WrappedTarget.WriteAsyncLogEvent(logEvent);
        this.FlushOnCondition();
      }
    }
    else
      this.WrappedTarget.WriteAsyncLogEvent(logEvent);
  }

  protected override void FlushAsync(AsyncContinuation asyncContinuation)
  {
    if (this.FlushOnConditionOnly)
      asyncContinuation((Exception) null);
    else
      this.FlushWrappedTarget(asyncContinuation);
  }

  private void FlushOnCondition()
  {
    if (this.FlushOnConditionOnly)
      this.FlushWrappedTarget((AsyncContinuation) (e => { }));
    else
      this.FlushAsync((AsyncContinuation) (e => { }));
  }

  private void FlushWrappedTarget(AsyncContinuation asyncContinuation)
  {
    this.WrappedTarget.Flush(this._pendingManualFlushList.RegisterCompletionNotification(asyncContinuation));
  }

  protected override void CloseTarget()
  {
    this._pendingManualFlushList.Clear();
    base.CloseTarget();
  }
}
