// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.SocketProxy
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Net.Sockets;

#nullable disable
namespace NLog.Internal.NetworkSenders;

internal sealed class SocketProxy : ISocket, IDisposable
{
  private readonly Socket _socket;

  internal SocketProxy(
    AddressFamily addressFamily,
    SocketType socketType,
    ProtocolType protocolType)
  {
    this._socket = new Socket(addressFamily, socketType, protocolType);
  }

  public Socket UnderlyingSocket => this._socket;

  public void Close() => this._socket.Close();

  public bool ConnectAsync(SocketAsyncEventArgs args) => this._socket.ConnectAsync(args);

  public bool SendAsync(SocketAsyncEventArgs args) => this._socket.SendAsync(args);

  public bool SendToAsync(SocketAsyncEventArgs args) => this._socket.SendToAsync(args);

  public void Dispose() => this._socket.Dispose();
}
