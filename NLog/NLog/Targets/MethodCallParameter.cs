// Decompiled with JetBrains decompiler
// Type: NLog.Targets.MethodCallParameter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Layouts;
using System;
using System.ComponentModel;

#nullable disable
namespace NLog.Targets;

[NLogConfigurationItem]
public class MethodCallParameter
{
  public MethodCallParameter() => this.ParameterType = typeof (string);

  public MethodCallParameter(Layout layout)
  {
    this.ParameterType = typeof (string);
    this.Layout = layout;
  }

  public MethodCallParameter(string parameterName, Layout layout)
  {
    this.ParameterType = typeof (string);
    this.Name = parameterName;
    this.Layout = layout;
  }

  public MethodCallParameter(string name, Layout layout, Type type)
  {
    this.ParameterType = type;
    this.Name = name;
    this.Layout = layout;
  }

  public string Name { get; set; }

  [Obsolete("Use property ParameterType instead. Marked obsolete on NLog 4.6")]
  public Type Type
  {
    get => this.ParameterType;
    set => this.ParameterType = value;
  }

  [DefaultValue(typeof (string))]
  public Type ParameterType { get; set; }

  [RequiredParameter]
  public Layout Layout { get; set; }
}
