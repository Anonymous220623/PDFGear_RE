// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.QueuedNetworkSender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Internal.NetworkSenders;

internal abstract class QueuedNetworkSender(string url) : NetworkSender(url)
{
  private readonly Queue<QueuedNetworkSender.NetworkRequestArgs> _pendingRequests = new Queue<QueuedNetworkSender.NetworkRequestArgs>();
  private Exception _pendingError;
  private bool _asyncOperationInProgress;
  private AsyncContinuation _closeContinuation;
  private AsyncContinuation _flushContinuation;

  internal int MaxQueueSize { get; set; }

  protected override void DoSend(
    byte[] bytes,
    int offset,
    int length,
    AsyncContinuation asyncContinuation)
  {
    QueuedNetworkSender.NetworkRequestArgs eventArgs = new QueuedNetworkSender.NetworkRequestArgs(bytes, offset, length, asyncContinuation);
    lock (this._pendingRequests)
    {
      if (this.MaxQueueSize > 0 && this._pendingRequests.Count >= this.MaxQueueSize)
      {
        AsyncContinuation asyncContinuation1 = this._pendingRequests.Dequeue().AsyncContinuation;
        if (asyncContinuation1 != null)
          asyncContinuation1((Exception) null);
      }
      if (!this._asyncOperationInProgress && this._pendingError == null)
      {
        this._asyncOperationInProgress = true;
        this.BeginRequest(eventArgs);
        return;
      }
      this._pendingRequests.Enqueue(eventArgs);
    }
    this.ProcessNextQueuedItem();
  }

  protected override void DoFlush(AsyncContinuation continuation)
  {
    lock (this._pendingRequests)
    {
      if (!this._asyncOperationInProgress && this._pendingRequests.Count == 0)
        continuation((Exception) null);
      else if (this._flushContinuation != null)
      {
        AsyncContinuation flushChain = this._flushContinuation;
        this._flushContinuation = (AsyncContinuation) (ex =>
        {
          flushChain(ex);
          continuation(ex);
        });
      }
      else
        this._flushContinuation = continuation;
    }
  }

  protected override void DoClose(AsyncContinuation continuation)
  {
    lock (this._pendingRequests)
    {
      if (!this._asyncOperationInProgress)
        continuation((Exception) null);
      else
        this._closeContinuation = continuation;
    }
  }

  protected void BeginInitialize()
  {
    lock (this._pendingRequests)
      this._asyncOperationInProgress = true;
  }

  protected void EndRequest(AsyncContinuation asyncContinuation, Exception pendingException)
  {
    lock (this._pendingRequests)
    {
      this._asyncOperationInProgress = false;
      if (pendingException != null)
        this._pendingError = pendingException;
      if (asyncContinuation != null)
        asyncContinuation(pendingException);
      this.ProcessNextQueuedItem();
    }
  }

  protected abstract void BeginRequest(QueuedNetworkSender.NetworkRequestArgs eventArgs);

  private void ProcessNextQueuedItem()
  {
    lock (this._pendingRequests)
    {
      if (this._asyncOperationInProgress)
        return;
      if (this._pendingError != null)
      {
        while (this._pendingRequests.Count != 0)
        {
          AsyncContinuation asyncContinuation = this._pendingRequests.Dequeue().AsyncContinuation;
          if (asyncContinuation != null)
            asyncContinuation(this._pendingError);
        }
      }
      if (this._pendingRequests.Count == 0)
      {
        AsyncContinuation flushContinuation = this._flushContinuation;
        if (flushContinuation != null)
        {
          this._flushContinuation = (AsyncContinuation) null;
          flushContinuation(this._pendingError);
        }
        AsyncContinuation closeContinuation = this._closeContinuation;
        if (closeContinuation == null)
          return;
        this._closeContinuation = (AsyncContinuation) null;
        closeContinuation(this._pendingError);
      }
      else
      {
        QueuedNetworkSender.NetworkRequestArgs eventArgs = this._pendingRequests.Dequeue();
        this._asyncOperationInProgress = true;
        this.BeginRequest(eventArgs);
      }
    }
  }

  protected struct NetworkRequestArgs(
    byte[] buffer,
    int offset,
    int length,
    AsyncContinuation asyncContinuation)
  {
    public readonly AsyncContinuation AsyncContinuation = asyncContinuation;
    public readonly byte[] RequestBuffer = buffer;
    public readonly int RequestBufferOffset = offset;
    public readonly int RequestBufferLength = length;
  }
}
