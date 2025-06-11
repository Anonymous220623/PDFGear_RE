// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.NetworkSender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

#nullable disable
namespace NLog.Internal.NetworkSenders;

internal abstract class NetworkSender : IDisposable
{
  private static int currentSendTime;

  protected NetworkSender(string url)
  {
    this.Address = url;
    this.LastSendTime = Interlocked.Increment(ref NetworkSender.currentSendTime);
  }

  public string Address { get; private set; }

  public int LastSendTime { get; private set; }

  public void Initialize() => this.DoInitialize();

  public void Close(AsyncContinuation continuation) => this.DoClose(continuation);

  public void FlushAsync(AsyncContinuation continuation) => this.DoFlush(continuation);

  public void Send(byte[] bytes, int offset, int length, AsyncContinuation asyncContinuation)
  {
    try
    {
      this.LastSendTime = Interlocked.Increment(ref NetworkSender.currentSendTime);
      this.DoSend(bytes, offset, length, asyncContinuation);
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "NetworkTarget: Error sending network request");
      asyncContinuation(ex);
    }
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  protected virtual void DoInitialize()
  {
  }

  protected virtual void DoClose(AsyncContinuation continuation) => continuation((Exception) null);

  protected virtual void DoFlush(AsyncContinuation continuation) => continuation((Exception) null);

  protected abstract void DoSend(
    byte[] bytes,
    int offset,
    int length,
    AsyncContinuation asyncContinuation);

  protected virtual EndPoint ParseEndpointAddress(Uri uri, AddressFamily addressFamily)
  {
    switch (uri.HostNameType)
    {
      case UriHostNameType.IPv4:
      case UriHostNameType.IPv6:
        return (EndPoint) new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port);
      default:
        foreach (IPAddress address in Dns.GetHostEntry(uri.Host).AddressList)
        {
          if (address.AddressFamily == addressFamily || addressFamily == AddressFamily.Unspecified)
            return (EndPoint) new IPEndPoint(address, uri.Port);
        }
        throw new IOException($"Cannot resolve '{uri.Host}' to an address in '{addressFamily}'");
    }
  }

  public virtual void CheckSocket()
  {
  }

  private void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    this.Close((AsyncContinuation) (ex => { }));
  }
}
