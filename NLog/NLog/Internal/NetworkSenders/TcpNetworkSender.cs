// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.TcpNetworkSender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Authentication;

#nullable disable
namespace NLog.Internal.NetworkSenders;

internal class TcpNetworkSender : QueuedNetworkSender
{
  private static bool? EnableKeepAliveSuccessful;
  private readonly EventHandler<SocketAsyncEventArgs> _socketOperationCompleted;
  private ISocket _socket;

  public TcpNetworkSender(string url, AddressFamily addressFamily)
    : base(url)
  {
    this.AddressFamily = addressFamily;
    this._socketOperationCompleted = new EventHandler<SocketAsyncEventArgs>(this.SocketOperationCompleted);
  }

  internal AddressFamily AddressFamily { get; set; }

  internal SslProtocols SslProtocols { get; set; }

  internal TimeSpan KeepAliveTime { get; set; }

  protected internal virtual ISocket CreateSocket(
    string host,
    AddressFamily addressFamily,
    SocketType socketType,
    ProtocolType protocolType)
  {
    SocketProxy socketProxy = new SocketProxy(addressFamily, socketType, protocolType);
    if (this.KeepAliveTime.TotalSeconds >= 1.0)
    {
      bool? keepAliveSuccessful = TcpNetworkSender.EnableKeepAliveSuccessful;
      bool flag = false;
      if (!(keepAliveSuccessful.GetValueOrDefault() == flag & keepAliveSuccessful.HasValue))
        TcpNetworkSender.EnableKeepAliveSuccessful = new bool?(TcpNetworkSender.TryEnableKeepAlive(socketProxy.UnderlyingSocket, (int) this.KeepAliveTime.TotalSeconds));
    }
    return this.SslProtocols != SslProtocols.None ? (ISocket) new SslSocketProxy(host, this.SslProtocols, socketProxy) : (ISocket) socketProxy;
  }

  private static bool TryEnableKeepAlive(Socket underlyingSocket, int keepAliveTimeSeconds)
  {
    if (TcpNetworkSender.TrySetSocketOption(underlyingSocket, SocketOptionName.KeepAlive, true))
    {
      SocketOptionName socketOption1 = SocketOptionName.TypeOfService;
      SocketOptionName socketOption2 = SocketOptionName.BlockSource;
      switch (PlatformDetector.CurrentOS)
      {
        case RuntimeOS.Linux:
          socketOption1 = SocketOptionName.ReuseAddress;
          socketOption2 = SocketOptionName.Debug | SocketOptionName.ReuseAddress;
          break;
        case RuntimeOS.MacOSX:
          socketOption1 = SocketOptionName.DontRoute;
          socketOption2 = SocketOptionName.Debug | SocketOptionName.OutOfBandInline;
          break;
      }
      if (TcpNetworkSender.TrySetTcpOption(underlyingSocket, socketOption1, keepAliveTimeSeconds))
      {
        TcpNetworkSender.TrySetTcpOption(underlyingSocket, socketOption2, 1);
        return true;
      }
    }
    return false;
  }

  private static bool TrySetSocketOption(
    Socket underlyingSocket,
    SocketOptionName socketOption,
    bool value)
  {
    try
    {
      underlyingSocket.SetSocketOption(SocketOptionLevel.Socket, socketOption, value);
      return true;
    }
    catch (Exception ex)
    {
      object[] objArray = new object[2]
      {
        (object) socketOption,
        (object) value
      };
      InternalLogger.Warn(ex, "NetworkTarget: Failed to configure Socket-option {0} = {1}", objArray);
      return false;
    }
  }

  private static bool TrySetTcpOption(
    Socket underlyingSocket,
    SocketOptionName socketOption,
    int value)
  {
    try
    {
      underlyingSocket.SetSocketOption(SocketOptionLevel.Tcp, socketOption, value);
      return true;
    }
    catch (Exception ex)
    {
      object[] objArray = new object[2]
      {
        (object) socketOption,
        (object) value
      };
      InternalLogger.Warn(ex, "NetworkTarget: Failed to configure TCP-option {0} = {1}", objArray);
      return false;
    }
  }

  protected override void DoInitialize()
  {
    Uri uri = new Uri(this.Address);
    TcpNetworkSender.MySocketAsyncEventArgs args = new TcpNetworkSender.MySocketAsyncEventArgs();
    args.RemoteEndPoint = this.ParseEndpointAddress(new Uri(this.Address), this.AddressFamily);
    args.Completed += this._socketOperationCompleted;
    args.UserToken = (object) null;
    this._socket = this.CreateSocket(uri.Host, args.RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
    this.BeginInitialize();
    bool flag = false;
    try
    {
      flag = this._socket.ConnectAsync((SocketAsyncEventArgs) args);
    }
    catch (SocketException ex)
    {
      args.SocketError = ex.SocketErrorCode;
    }
    catch (Exception ex)
    {
      if (ex.InnerException is SocketException innerException)
        args.SocketError = innerException.SocketErrorCode;
      else
        args.SocketError = SocketError.OperationAborted;
    }
    if (flag)
      return;
    this.SocketOperationCompleted((object) this._socket, (SocketAsyncEventArgs) args);
  }

  protected override void DoClose(AsyncContinuation continuation)
  {
    base.DoClose((AsyncContinuation) (ex => this.CloseSocket(continuation, ex)));
  }

  private void CloseSocket(AsyncContinuation continuation, Exception pendingException)
  {
    try
    {
      ISocket socket = this._socket;
      this._socket = (ISocket) null;
      socket?.Close();
      continuation(pendingException);
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrown())
        throw;
      continuation(ex);
    }
  }

  private void SocketOperationCompleted(object sender, SocketAsyncEventArgs args)
  {
    AsyncContinuation userToken = args.UserToken as AsyncContinuation;
    Exception pendingException = (Exception) null;
    if (args.SocketError != SocketError.Success)
      pendingException = (Exception) new IOException("Error: " + (object) args.SocketError);
    args.Completed -= this._socketOperationCompleted;
    args.Dispose();
    this.EndRequest(userToken, pendingException);
  }

  protected override void BeginRequest(QueuedNetworkSender.NetworkRequestArgs eventArgs)
  {
    TcpNetworkSender.MySocketAsyncEventArgs args = new TcpNetworkSender.MySocketAsyncEventArgs();
    args.SetBuffer(eventArgs.RequestBuffer, eventArgs.RequestBufferOffset, eventArgs.RequestBufferLength);
    args.UserToken = (object) eventArgs.AsyncContinuation;
    args.Completed += this._socketOperationCompleted;
    bool flag = false;
    try
    {
      flag = this._socket.SendAsync((SocketAsyncEventArgs) args);
    }
    catch (SocketException ex)
    {
      InternalLogger.Error((Exception) ex, "NetworkTarget: Error sending tcp request");
      args.SocketError = ex.SocketErrorCode;
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "NetworkTarget: Error sending tcp request");
      if (ex.InnerException is SocketException innerException)
        args.SocketError = innerException.SocketErrorCode;
      else
        args.SocketError = SocketError.OperationAborted;
    }
    if (flag)
      return;
    this.SocketOperationCompleted((object) this._socket, (SocketAsyncEventArgs) args);
  }

  public override void CheckSocket()
  {
    if (this._socket != null)
      return;
    this.DoInitialize();
  }

  internal class MySocketAsyncEventArgs : SocketAsyncEventArgs
  {
    public void RaiseCompleted() => this.OnCompleted((SocketAsyncEventArgs) this);
  }
}
