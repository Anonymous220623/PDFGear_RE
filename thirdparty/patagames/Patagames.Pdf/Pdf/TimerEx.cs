// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.TimerEx
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Threading;

#nullable disable
namespace Patagames.Pdf;

internal class TimerEx
{
  private Patagames.Pdf.TimerCallback _lpTimerFunc;
  private int _interval;
  private Timer _timer;

  public int TimerId { get; private set; }

  private SynchronizationContext _synchronizationContext { get; set; }

  public TimerEx(
    SynchronizationContext synchronizationContext,
    int interval,
    int timerid,
    Patagames.Pdf.TimerCallback lpTimerFunc)
  {
    this._synchronizationContext = synchronizationContext;
    this._interval = interval;
    this.TimerId = timerid;
    this._lpTimerFunc = lpTimerFunc;
    this._timer = new Timer(new System.Threading.TimerCallback(this.TimerCallback), (object) this, -1, interval);
  }

  internal void Start() => this._timer.Change(this._interval, this._interval);

  internal void Stop() => this._timer.Change(-1, -1);

  private void TimerCallback(object stateInfo)
  {
    TimerEx tm = stateInfo as TimerEx;
    if (this._synchronizationContext == null)
      tm._lpTimerFunc(tm.TimerId);
    else
      this._synchronizationContext.Send((SendOrPostCallback) (o => tm._lpTimerFunc(tm.TimerId)), (object) tm);
  }
}
