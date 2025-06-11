// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.StackTraceLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("stacktrace")]
[ThreadAgnostic]
[ThreadSafe]
public class StackTraceLayoutRenderer : LayoutRenderer, IUsesStackTrace
{
  public StackTraceLayoutRenderer()
  {
    this.Separator = " => ";
    this.TopFrames = 3;
    this.Format = StackTraceFormat.Flat;
  }

  [DefaultValue("Flat")]
  public StackTraceFormat Format { get; set; }

  [DefaultValue(3)]
  public int TopFrames { get; set; }

  [DefaultValue(0)]
  public int SkipFrames { get; set; }

  [DefaultValue(" => ")]
  public string Separator { get; set; }

  [DefaultValue(true)]
  public bool CaptureStackTrace { get; set; } = true;

  StackTraceUsage IUsesStackTrace.StackTraceUsage
  {
    get
    {
      if (!this.CaptureStackTrace)
        return StackTraceUsage.None;
      return this.Format == StackTraceFormat.Raw ? StackTraceUsage.WithSource : StackTraceUsage.WithoutSource;
    }
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    if (logEvent.StackTrace == null)
      return;
    int startingFrame = logEvent.UserStackFrameNumber + this.TopFrames - 1;
    if (startingFrame >= logEvent.StackTrace.GetFrameCount())
      startingFrame = logEvent.StackTrace.GetFrameCount() - 1;
    int endingFrame = logEvent.UserStackFrameNumber + this.SkipFrames;
    switch (this.Format)
    {
      case StackTraceFormat.Raw:
        StackTraceLayoutRenderer.AppendRaw(builder, logEvent, startingFrame, endingFrame);
        break;
      case StackTraceFormat.Flat:
        this.AppendFlat(builder, logEvent, startingFrame, endingFrame);
        break;
      case StackTraceFormat.DetailedFlat:
        this.AppendDetailedFlat(builder, logEvent, startingFrame, endingFrame);
        break;
    }
  }

  private static void AppendRaw(
    StringBuilder builder,
    LogEventInfo logEvent,
    int startingFrame,
    int endingFrame)
  {
    for (int index = startingFrame; index >= endingFrame; --index)
    {
      StackFrame frame = logEvent.StackTrace.GetFrame(index);
      builder.Append(frame.ToString());
    }
  }

  private void AppendFlat(
    StringBuilder builder,
    LogEventInfo logEvent,
    int startingFrame,
    int endingFrame)
  {
    bool flag = true;
    for (int index = startingFrame; index >= endingFrame; --index)
    {
      StackFrame frame = logEvent.StackTrace.GetFrame(index);
      if (!flag)
        builder.Append(this.Separator);
      MethodBase method = frame.GetMethod();
      if (!(method == (MethodBase) null))
      {
        Type declaringType = method.DeclaringType;
        if (declaringType != (Type) null)
          builder.Append(declaringType.Name);
        else
          builder.Append("<no type>");
        builder.Append(".");
        builder.Append(method.Name);
        flag = false;
      }
    }
  }

  private void AppendDetailedFlat(
    StringBuilder builder,
    LogEventInfo logEvent,
    int startingFrame,
    int endingFrame)
  {
    bool flag = true;
    for (int index = startingFrame; index >= endingFrame; --index)
    {
      MethodBase method = logEvent.StackTrace.GetFrame(index).GetMethod();
      if (!(method == (MethodBase) null))
      {
        if (!flag)
          builder.Append(this.Separator);
        builder.Append("[");
        builder.Append((object) method);
        builder.Append("]");
        flag = false;
      }
    }
  }
}
