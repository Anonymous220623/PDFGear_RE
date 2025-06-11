// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ConsoleTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.Targets;

[Target("Console")]
public sealed class ConsoleTarget : TargetWithLayoutHeaderAndFooter
{
  private bool _pauseLogging;
  private readonly ReusableBufferCreator _reusableEncodingBuffer = new ReusableBufferCreator(16384 /*0x4000*/);
  private Encoding _encoding;

  [DefaultValue(false)]
  public bool Error { get; set; }

  public Encoding Encoding
  {
    get
    {
      return ConsoleTargetHelper.GetConsoleOutputEncoding(this._encoding, this.IsInitialized, this._pauseLogging);
    }
    set
    {
      if (!ConsoleTargetHelper.SetConsoleOutputEncoding(value, this.IsInitialized, this._pauseLogging))
        return;
      this._encoding = value;
    }
  }

  [DefaultValue(false)]
  public bool DetectConsoleAvailable { get; set; }

  [DefaultValue(false)]
  public bool AutoFlush { get; set; }

  [DefaultValue(false)]
  public bool WriteBuffer { get; set; }

  public ConsoleTarget() => this.OptimizeBufferReuse = true;

  public ConsoleTarget(string name)
    : this()
  {
    this.Name = name;
  }

  protected override void InitializeTarget()
  {
    this._pauseLogging = false;
    if (this.DetectConsoleAvailable)
    {
      string reason;
      this._pauseLogging = !ConsoleTargetHelper.IsConsoleAvailable(out reason);
      if (this._pauseLogging)
        InternalLogger.Info<string, string>("Console(Name={0}): Console has been detected as turned off. Disable DetectConsoleAvailable to skip detection. Reason: {1}", this.Name, reason);
    }
    if (this._encoding != null)
      ConsoleTargetHelper.SetConsoleOutputEncoding(this._encoding, true, this._pauseLogging);
    base.InitializeTarget();
    if (this.Header == null)
      return;
    this.RenderToOutput(this.Header, LogEventInfo.CreateNullEvent());
  }

  protected override void CloseTarget()
  {
    if (this.Footer != null)
      this.RenderToOutput(this.Footer, LogEventInfo.CreateNullEvent());
    this.ExplicitConsoleFlush();
    base.CloseTarget();
  }

  protected override void FlushAsync(AsyncContinuation asyncContinuation)
  {
    try
    {
      this.ExplicitConsoleFlush();
      base.FlushAsync(asyncContinuation);
    }
    catch (Exception ex)
    {
      asyncContinuation(ex);
    }
  }

  private void ExplicitConsoleFlush()
  {
    if (this._pauseLogging || this.AutoFlush)
      return;
    this.GetOutput().Flush();
  }

  protected override void Write(LogEventInfo logEvent)
  {
    if (this._pauseLogging)
      return;
    this.RenderToOutput(this.Layout, logEvent);
  }

  protected override void Write(IList<AsyncLogEventInfo> logEvents)
  {
    if (this._pauseLogging)
      return;
    if (this.WriteBuffer)
      this.WriteBufferToOutput(logEvents);
    else
      base.Write(logEvents);
  }

  private void RenderToOutput(Layout layout, LogEventInfo logEvent)
  {
    if (this._pauseLogging)
      return;
    TextWriter output = this.GetOutput();
    if (this.WriteBuffer)
      this.WriteBufferToOutput(output, layout, logEvent);
    else
      this.WriteLineToOutput(output, this.RenderLogEvent(layout, logEvent));
  }

  private void WriteBufferToOutput(TextWriter output, Layout layout, LogEventInfo logEvent)
  {
    int targetBufferPosition = 0;
    using (ReusableObjectCreator<char[]>.LockOject lockOject1 = this._reusableEncodingBuffer.Allocate())
    {
      using (ReusableObjectCreator<StringBuilder>.LockOject lockOject2 = this.ReusableLayoutBuilder.Allocate())
      {
        this.RenderLogEventToWriteBuffer(output, layout, logEvent, lockOject2.Result, lockOject1.Result, ref targetBufferPosition);
        if (targetBufferPosition <= 0)
          return;
        this.WriteBufferToOutput(output, lockOject1.Result, targetBufferPosition);
      }
    }
  }

  private void WriteBufferToOutput(IList<AsyncLogEventInfo> logEvents)
  {
    TextWriter output = this.GetOutput();
    using (ReusableObjectCreator<char[]>.LockOject lockOject1 = this._reusableEncodingBuffer.Allocate())
    {
      using (ReusableObjectCreator<StringBuilder>.LockOject lockOject2 = this.ReusableLayoutBuilder.Allocate())
      {
        int targetBufferPosition = 0;
        try
        {
          for (int index = 0; index < logEvents.Count; ++index)
          {
            lockOject2.Result.ClearBuilder();
            this.RenderLogEventToWriteBuffer(output, this.Layout, logEvents[index].LogEvent, lockOject2.Result, lockOject1.Result, ref targetBufferPosition);
            logEvents[index].Continuation((Exception) null);
          }
        }
        finally
        {
          if (targetBufferPosition > 0)
            this.WriteBufferToOutput(output, lockOject1.Result, targetBufferPosition);
        }
      }
    }
  }

  private void RenderLogEventToWriteBuffer(
    TextWriter output,
    Layout layout,
    LogEventInfo logEvent,
    StringBuilder targetBuilder,
    char[] targetBuffer,
    ref int targetBufferPosition)
  {
    int length = Environment.NewLine.Length;
    layout.RenderAppendBuilder(logEvent, targetBuilder);
    if (targetBuilder.Length > targetBuffer.Length - targetBufferPosition - length)
    {
      if (targetBufferPosition > 0)
      {
        this.WriteBufferToOutput(output, targetBuffer, targetBufferPosition);
        targetBufferPosition = 0;
      }
      if (targetBuilder.Length > targetBuffer.Length - length)
      {
        this.WriteLineToOutput(output, targetBuilder.ToString());
        return;
      }
    }
    targetBuilder.Append(Environment.NewLine);
    targetBuilder.CopyToBuffer(targetBuffer, targetBufferPosition);
    targetBufferPosition += targetBuilder.Length;
  }

  private void WriteLineToOutput(TextWriter output, string message)
  {
    try
    {
      ConsoleTargetHelper.WriteLineThreadSafe(output, message, this.AutoFlush);
    }
    catch (Exception ex) when (
    {
      // ISSUE: unable to correctly present filter
      int num;
      switch (ex)
      {
        case OverflowException _:
        case IndexOutOfRangeException _:
          num = 1;
          break;
        default:
          num = ex is ArgumentOutOfRangeException ? 1 : 0;
          break;
      }
      if ((uint) num > 0U)
      {
        SuccessfulFiltering;
      }
      else
        throw;
    }
    )
    {
      this._pauseLogging = true;
      InternalLogger.Warn(ex, "Console(Name={0}): {1} has been thrown and this is probably due to a race condition.Logging to the console will be paused. Enable by reloading the config or re-initialize the targets", (object) this.Name, (object) ex.GetType());
    }
  }

  private void WriteBufferToOutput(TextWriter output, char[] buffer, int length)
  {
    try
    {
      ConsoleTargetHelper.WriteBufferThreadSafe(output, buffer, length, this.AutoFlush);
    }
    catch (Exception ex) when (
    {
      // ISSUE: unable to correctly present filter
      int num;
      switch (ex)
      {
        case OverflowException _:
        case IndexOutOfRangeException _:
          num = 1;
          break;
        default:
          num = ex is ArgumentOutOfRangeException ? 1 : 0;
          break;
      }
      if ((uint) num > 0U)
      {
        SuccessfulFiltering;
      }
      else
        throw;
    }
    )
    {
      this._pauseLogging = true;
      InternalLogger.Warn(ex, "Console(Name={0}): {1} has been thrown and this is probably due to a race condition.Logging to the console will be paused. Enable by reloading the config or re-initialize the targets", (object) this.Name, (object) ex.GetType());
    }
  }

  private TextWriter GetOutput() => !this.Error ? Console.Out : Console.Error;
}
