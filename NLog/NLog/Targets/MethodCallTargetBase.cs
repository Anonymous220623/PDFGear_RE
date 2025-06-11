// Decompiled with JetBrains decompiler
// Type: NLog.Targets.MethodCallTargetBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace NLog.Targets;

public abstract class MethodCallTargetBase : Target
{
  private IPropertyTypeConverter _propertyTypeConverter;

  protected MethodCallTargetBase()
  {
    this.Parameters = (IList<MethodCallParameter>) new List<MethodCallParameter>();
  }

  [ArrayParameter(typeof (MethodCallParameter), "parameter")]
  public IList<MethodCallParameter> Parameters { get; private set; }

  private IPropertyTypeConverter PropertyTypeConverter
  {
    get
    {
      return this._propertyTypeConverter ?? (this._propertyTypeConverter = ConfigurationItemFactory.Default.PropertyTypeConverter);
    }
    set => this._propertyTypeConverter = value;
  }

  protected override void CloseTarget()
  {
    this.PropertyTypeConverter = (IPropertyTypeConverter) null;
    base.CloseTarget();
  }

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    object[] parameters = this.Parameters.Count > 0 ? new object[this.Parameters.Count] : ArrayHelper.Empty<object>();
    for (int index = 0; index < parameters.Length; ++index)
    {
      try
      {
        parameters[index] = this.GetParameterValue(logEvent.LogEvent, this.Parameters[index]);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
          throw;
        object[] objArray = new object[3]
        {
          (object) this.GetType(),
          (object) this.Name,
          (object) this.Parameters[index].Name
        };
        InternalLogger.Warn(ex, "{0}(Name={1}): Failed to get parameter value {2}", objArray);
        throw;
      }
    }
    this.DoInvoke(parameters, logEvent);
  }

  private object GetParameterValue(LogEventInfo logEvent, MethodCallParameter param)
  {
    Type type1 = param.ParameterType;
    if ((object) type1 == null)
      type1 = typeof (string);
    Type type2 = type1;
    string propertyValue = this.RenderLogEvent(param.Layout, logEvent) ?? string.Empty;
    if (type2 == typeof (string) || type2 == typeof (object))
      return (object) propertyValue;
    return string.IsNullOrEmpty(propertyValue) && type2.IsValueType() ? Activator.CreateInstance(param.ParameterType) : this.PropertyTypeConverter.Convert((object) propertyValue, type2, (string) null, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  protected virtual void DoInvoke(object[] parameters, AsyncLogEventInfo logEvent)
  {
    this.DoInvoke(parameters, logEvent.Continuation);
  }

  protected virtual void DoInvoke(object[] parameters, AsyncContinuation continuation)
  {
    try
    {
      this.DoInvoke(parameters);
      continuation((Exception) null);
    }
    catch (Exception ex)
    {
      if (this.ExceptionMustBeRethrown(ex))
        throw;
      continuation(ex);
    }
  }

  protected abstract void DoInvoke(object[] parameters);
}
