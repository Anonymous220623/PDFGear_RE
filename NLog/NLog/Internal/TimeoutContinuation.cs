// Decompiled with JetBrains decompiler
// Type: NLog.Internal.TimeoutContinuation
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Threading;

#nullable disable
namespace NLog.Internal;

internal sealed class TimeoutContinuation : IDisposable
{
  private AsyncContinuation _asyncContinuation;
  private Timer _timeoutTimer;

  public TimeoutContinuation(AsyncContinuation asyncContinuation, TimeSpan timeout)
  {
    this._asyncContinuation = asyncContinuation;
    this._timeoutTimer = new Timer(new TimerCallback(this.TimerElapsed), (object) null, (int) timeout.TotalMilliseconds, -1);
  }

  public void Function(Exception exception)
  {
    try
    {
      AsyncContinuation asyncContinuation = Interlocked.Exchange<AsyncContinuation>(ref this._asyncContinuation, (AsyncContinuation) null);
      this.StopTimer();
      if (asyncContinuation == null)
        return;
      asyncContinuation(exception);
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "Exception in asynchronous handler.");
      if (!ex.MustBeRethrown())
        return;
      throw;
    }
  }

  public void Dispose()
  {
    this.StopTimer();
    GC.SuppressFinalize((object) this);
  }

  private void StopTimer()
  {
    Timer timer = Interlocked.Exchange<Timer>(ref this._timeoutTimer, (Timer) null);
    if (timer == null)
      return;
    timer.WaitForDispose(TimeSpan.Zero);
  }

  private void TimerElapsed(object state)
  {
    this.Function((Exception) new TimeoutException("Timeout."));
  }
}
