// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.SslSocketProxy
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace NLog.Internal.NetworkSenders;

internal class SslSocketProxy : ISocket, IDisposable
{
  private readonly AsyncCallback _sendCompleted;
  private readonly SocketProxy _socketProxy;
  private readonly string _host;
  private readonly SslProtocols _sslProtocol;
  private SslStream _sslStream;

  public SslSocketProxy(string host, SslProtocols sslProtocol, SocketProxy socketProxy)
  {
    this._socketProxy = socketProxy;
    this._host = host;
    this._sslProtocol = sslProtocol;
    this._sendCompleted = (AsyncCallback) (ar => this.SocketProxySendCompleted(ar));
  }

  public void Close()
  {
    if (this._sslStream != null)
      this._sslStream.Close();
    else
      this._socketProxy.Close();
  }

  public bool ConnectAsync(SocketAsyncEventArgs args)
  {
    TcpNetworkSender.MySocketAsyncEventArgs socketAsyncEventArgs = new TcpNetworkSender.MySocketAsyncEventArgs();
    socketAsyncEventArgs.RemoteEndPoint = args.RemoteEndPoint;
    socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.SocketProxyConnectCompleted);
    socketAsyncEventArgs.UserToken = (object) args;
    if (this._socketProxy.ConnectAsync((SocketAsyncEventArgs) socketAsyncEventArgs))
      return true;
    this.SocketProxyConnectCompleted((object) this, (SocketAsyncEventArgs) socketAsyncEventArgs);
    return false;
  }

  private void SocketProxySendCompleted(IAsyncResult asyncResult)
  {
    TcpNetworkSender.MySocketAsyncEventArgs asyncState = asyncResult.AsyncState as TcpNetworkSender.MySocketAsyncEventArgs;
    try
    {
      this._sslStream.EndWrite(asyncResult);
    }
    catch (SocketException ex)
    {
      if (asyncState == null)
        return;
      asyncState.SocketError = ex.SocketErrorCode;
    }
    catch (Exception ex)
    {
      if (asyncState == null)
        return;
      if (ex.InnerException is SocketException innerException)
        asyncState.SocketError = innerException.SocketErrorCode;
      else
        asyncState.SocketError = SocketError.OperationAborted;
    }
    finally
    {
      asyncState?.RaiseCompleted();
    }
  }

  private void SocketProxyConnectCompleted(object sender, SocketAsyncEventArgs e)
  {
    TcpNetworkSender.MySocketAsyncEventArgs userToken = e.UserToken as TcpNetworkSender.MySocketAsyncEventArgs;
    if (e.SocketError != SocketError.Success)
    {
      if (userToken == null)
        return;
      userToken.SocketError = e.SocketError;
      userToken.RaiseCompleted();
    }
    else
    {
      try
      {
        SslStream sslStream = new SslStream((Stream) new NetworkStream(this._socketProxy.UnderlyingSocket), false, new RemoteCertificateValidationCallback(SslSocketProxy.UserCertificateValidationCallback));
        sslStream.ReadTimeout = 20000;
        this._sslStream = sslStream;
        SslSocketProxy.AuthenticateAsClient(this._sslStream, this._host, this._sslProtocol);
      }
      catch (SocketException ex)
      {
        if (userToken == null)
          return;
        userToken.SocketError = ex.SocketErrorCode;
      }
      catch (Exception ex)
      {
        if (userToken == null)
          return;
        userToken.SocketError = SslSocketProxy.GetSocketError(ex);
      }
      finally
      {
        userToken?.RaiseCompleted();
      }
    }
  }

  private static SocketError GetSocketError(Exception ex)
  {
    return !(ex.InnerException is SocketException innerException) ? SocketError.ConnectionRefused : innerException.SocketErrorCode;
  }

  private static void AuthenticateAsClient(
    SslStream sslStream,
    string targetHost,
    SslProtocols enabledSslProtocols)
  {
    if (enabledSslProtocols != SslProtocols.Default)
      sslStream.AuthenticateAsClient(targetHost, (X509CertificateCollection) null, enabledSslProtocols, false);
    else
      sslStream.AuthenticateAsClient(targetHost);
  }

  private static bool UserCertificateValidationCallback(
    object sender,
    object certificate,
    object chain,
    SslPolicyErrors sslPolicyErrors)
  {
    if (sslPolicyErrors == SslPolicyErrors.None)
      return true;
    InternalLogger.Debug<SslPolicyErrors, object>("SSL certificate errors were encountered when establishing connection to the server: {0}, Certificate: {1}", sslPolicyErrors, certificate);
    return false;
  }

  public bool SendAsync(SocketAsyncEventArgs args)
  {
    this._sslStream.BeginWrite(args.Buffer, args.Offset, args.Count, this._sendCompleted, (object) args);
    return true;
  }

  public bool SendToAsync(SocketAsyncEventArgs args) => this.SendAsync(args);

  public void Dispose()
  {
    if (this._sslStream != null)
      this._sslStream.Dispose();
    else
      this._socketProxy.Dispose();
  }
}
