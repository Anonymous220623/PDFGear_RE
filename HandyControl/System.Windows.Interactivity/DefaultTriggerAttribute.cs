// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.DefaultTriggerAttribute
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections;
using System.Globalization;

#nullable disable
namespace HandyControl.Interactivity;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
public sealed class DefaultTriggerAttribute : Attribute
{
  private readonly object[] _parameters;

  public DefaultTriggerAttribute(Type targetType, Type triggerType, object parameter)
    : this(targetType, triggerType, new object[1]
    {
      parameter
    })
  {
  }

  public DefaultTriggerAttribute(Type targetType, Type triggerType, params object[] parameters)
  {
    if (!typeof (TriggerBase).IsAssignableFrom(triggerType))
      throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage, new object[1]
      {
        (object) triggerType.Name
      }));
    this.TargetType = targetType;
    this.TriggerType = triggerType;
    this._parameters = parameters;
  }

  public IEnumerable Parameters => (IEnumerable) this._parameters;

  public Type TargetType { get; }

  public Type TriggerType { get; }

  public TriggerBase Instantiate()
  {
    object obj = (object) null;
    try
    {
      obj = Activator.CreateInstance(this.TriggerType, this._parameters);
    }
    catch
    {
    }
    return (TriggerBase) obj;
  }
}
