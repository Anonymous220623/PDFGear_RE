// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.RandomizeGroupTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;

#nullable disable
namespace NLog.Targets.Wrappers;

[Target("RandomizeGroup", IsCompound = true)]
public class RandomizeGroupTarget : CompoundTargetBase
{
  private readonly Random _random = new Random();

  public RandomizeGroupTarget()
    : this(new Target[0])
  {
  }

  public RandomizeGroupTarget(string name, params Target[] targets)
    : this(targets)
  {
    this.Name = name;
  }

  public RandomizeGroupTarget(params Target[] targets)
    : base(targets)
  {
    this.OptimizeBufferReuse = this.GetType() == typeof (RandomizeGroupTarget);
  }

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    if (this.Targets.Count == 0)
    {
      logEvent.Continuation((Exception) null);
    }
    else
    {
      int index;
      lock (this._random)
        index = this._random.Next(this.Targets.Count);
      this.Targets[index].WriteAsyncLogEvent(logEvent);
    }
  }
}
