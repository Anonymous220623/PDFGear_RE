// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.INetworkSenderFactory
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Security.Authentication;

#nullable disable
namespace NLog.Internal.NetworkSenders;

internal interface INetworkSenderFactory
{
  NetworkSender Create(
    string url,
    int maxQueueSize,
    SslProtocols sslProtocols,
    TimeSpan keepAliveTime);
}
