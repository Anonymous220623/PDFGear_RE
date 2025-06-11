// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.NetworkSenderFactory
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Net.Sockets;
using System.Security.Authentication;

#nullable disable
namespace NLog.Internal.NetworkSenders;

internal class NetworkSenderFactory : INetworkSenderFactory
{
  public static readonly INetworkSenderFactory Default = (INetworkSenderFactory) new NetworkSenderFactory();

  public NetworkSender Create(
    string url,
    int maxQueueSize,
    SslProtocols sslProtocols,
    TimeSpan keepAliveTime)
  {
    if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
    {
      HttpNetworkSender httpNetworkSender = new HttpNetworkSender(url);
      httpNetworkSender.MaxQueueSize = maxQueueSize;
      return (NetworkSender) httpNetworkSender;
    }
    if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
    {
      HttpNetworkSender httpNetworkSender = new HttpNetworkSender(url);
      httpNetworkSender.MaxQueueSize = maxQueueSize;
      return (NetworkSender) httpNetworkSender;
    }
    if (url.StartsWith("tcp://", StringComparison.OrdinalIgnoreCase))
    {
      TcpNetworkSender tcpNetworkSender = new TcpNetworkSender(url, AddressFamily.Unspecified);
      tcpNetworkSender.MaxQueueSize = maxQueueSize;
      tcpNetworkSender.SslProtocols = sslProtocols;
      tcpNetworkSender.KeepAliveTime = keepAliveTime;
      return (NetworkSender) tcpNetworkSender;
    }
    if (url.StartsWith("tcp4://", StringComparison.OrdinalIgnoreCase))
    {
      TcpNetworkSender tcpNetworkSender = new TcpNetworkSender(url, AddressFamily.InterNetwork);
      tcpNetworkSender.MaxQueueSize = maxQueueSize;
      tcpNetworkSender.SslProtocols = sslProtocols;
      tcpNetworkSender.KeepAliveTime = keepAliveTime;
      return (NetworkSender) tcpNetworkSender;
    }
    if (url.StartsWith("tcp6://", StringComparison.OrdinalIgnoreCase))
    {
      TcpNetworkSender tcpNetworkSender = new TcpNetworkSender(url, AddressFamily.InterNetworkV6);
      tcpNetworkSender.MaxQueueSize = maxQueueSize;
      tcpNetworkSender.SslProtocols = sslProtocols;
      tcpNetworkSender.KeepAliveTime = keepAliveTime;
      return (NetworkSender) tcpNetworkSender;
    }
    if (url.StartsWith("udp://", StringComparison.OrdinalIgnoreCase))
      return (NetworkSender) new UdpNetworkSender(url, AddressFamily.Unspecified);
    if (url.StartsWith("udp4://", StringComparison.OrdinalIgnoreCase))
      return (NetworkSender) new UdpNetworkSender(url, AddressFamily.InterNetwork);
    return url.StartsWith("udp6://", StringComparison.OrdinalIgnoreCase) ? (NetworkSender) new UdpNetworkSender(url, AddressFamily.InterNetworkV6) : throw new ArgumentException("Unrecognized network address", nameof (url));
  }
}
