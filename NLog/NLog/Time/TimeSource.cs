// Decompiled with JetBrains decompiler
// Type: NLog.Time.TimeSource
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;

#nullable disable
namespace NLog.Time;

[NLogConfigurationItem]
public abstract class TimeSource
{
  public abstract DateTime Time { get; }

  public static TimeSource Current { get; set; } = (TimeSource) new FastLocalTimeSource();

  public override string ToString()
  {
    TimeSourceAttribute customAttribute = this.GetType().GetCustomAttribute<TimeSourceAttribute>();
    return customAttribute != null ? customAttribute.Name + " (time source)" : this.GetType().Name;
  }

  public abstract DateTime FromSystemTime(DateTime systemTime);
}
