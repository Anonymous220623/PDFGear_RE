// Decompiled with JetBrains decompiler
// Type: NLog.Targets.MethodCallTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Reflection;

#nullable disable
namespace NLog.Targets;

[Target("MethodCall")]
public sealed class MethodCallTarget : MethodCallTargetBase
{
  private Action<LogEventInfo, object[]> _logEventAction;

  public string ClassName { get; set; }

  public string MethodName { get; set; }

  public MethodCallTarget()
  {
  }

  public MethodCallTarget(string name)
    : this(name, (Action<LogEventInfo, object[]>) null)
  {
  }

  public MethodCallTarget(string name, Action<LogEventInfo, object[]> logEventAction)
    : this()
  {
    this.Name = name;
    this._logEventAction = logEventAction;
  }

  protected override void InitializeTarget()
  {
    base.InitializeTarget();
    if (this.ClassName == null || this.MethodName == null)
      return;
    Type type = Type.GetType(this.ClassName);
    if (type != (Type) null)
    {
      MethodInfo method = type.GetMethod(this.MethodName);
      if (method == (MethodInfo) null)
        InternalLogger.Warn<string, string>("Initialize MethodCallTarget, method '{0}' in class '{1}' not found - it should be static", this.MethodName, this.ClassName);
      else
        this._logEventAction = MethodCallTarget.BuildLogEventAction(method);
    }
    else
      InternalLogger.Warn<string>("Initialize MethodCallTarget, class '{0}' not found", this.ClassName);
  }

  private static Action<LogEventInfo, object[]> BuildLogEventAction(MethodInfo methodInfo)
  {
    int neededParameters = methodInfo.GetParameters().Length;
    return (Action<LogEventInfo, object[]>) ((logEvent, parameters) =>
    {
      if (neededParameters - parameters.Length > 0)
      {
        object[] objArray = new object[neededParameters];
        for (int index = 0; index < parameters.Length; ++index)
          objArray[index] = parameters[index];
        for (int length = parameters.Length; length < neededParameters; ++length)
          objArray[length] = Type.Missing;
        parameters = objArray;
      }
      methodInfo.Invoke((object) null, parameters);
    });
  }

  protected override void DoInvoke(object[] parameters, AsyncLogEventInfo logEvent)
  {
    try
    {
      this.ExecuteLogMethod(parameters, logEvent.LogEvent);
      logEvent.Continuation((Exception) null);
    }
    catch (Exception ex)
    {
      if (this.ExceptionMustBeRethrown(ex))
        throw;
      logEvent.Continuation(ex);
    }
  }

  protected override void DoInvoke(object[] parameters)
  {
    this.ExecuteLogMethod(parameters, (LogEventInfo) null);
  }

  private void ExecuteLogMethod(object[] parameters, LogEventInfo logEvent)
  {
    if (this._logEventAction != null)
      this._logEventAction(logEvent, parameters);
    else
      InternalLogger.Trace("No invoke because class/method was not found or set");
  }
}
