// Decompiled with JetBrains decompiler
// Type: NLog.Time.CachedTimeSource
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Time;

public abstract class CachedTimeSource : TimeSource
{
  private int _lastTicks = -1;
  private DateTime _lastTime = DateTime.MinValue;

  protected abstract DateTime FreshTime { get; }

  public override DateTime Time
  {
    get
    {
      int tickCount = Environment.TickCount;
      if (tickCount == this._lastTicks)
        return this._lastTime;
      DateTime freshTime = this.FreshTime;
      this._lastTicks = tickCount;
      this._lastTime = freshTime;
      return freshTime;
    }
  }
}
