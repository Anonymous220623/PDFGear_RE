// Decompiled with JetBrains decompiler
// Type: NLog.Internal.SingleCallContinuation
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Threading;

#nullable disable
namespace NLog.Internal;

internal class SingleCallContinuation
{
  internal static readonly AsyncContinuation Completed = new AsyncContinuation(new SingleCallContinuation((AsyncContinuation) null).CompletedFunction);
  private AsyncContinuation _asyncContinuation;

  public SingleCallContinuation(AsyncContinuation asyncContinuation)
  {
    this._asyncContinuation = asyncContinuation;
  }

  public void Function(Exception exception)
  {
    try
    {
      AsyncContinuation asyncContinuation = Interlocked.Exchange<AsyncContinuation>(ref this._asyncContinuation, (AsyncContinuation) null);
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

  private void CompletedFunction(Exception exception)
  {
  }
}
