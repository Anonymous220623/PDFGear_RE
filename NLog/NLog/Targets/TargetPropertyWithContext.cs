// Decompiled with JetBrains decompiler
// Type: NLog.Targets.TargetPropertyWithContext
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
public class TargetPropertyWithContext
{
  public TargetPropertyWithContext()
    : this((string) null, (Layout) null)
  {
  }

  public TargetPropertyWithContext(string name, Layout layout)
  {
    this.Name = name;
    this.Layout = layout;
  }

  [RequiredParameter]
  public string Name { get; set; }

  [RequiredParameter]
  public Layout Layout { get; set; }

  [DefaultValue(true)]
  public bool IncludeEmptyValue { get; set; } = true;

  [DefaultValue(typeof (string))]
  public Type PropertyType { get; set; } = typeof (string);
}
