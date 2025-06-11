// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.WcfLogReceiverTwoWayClient
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

#nullable disable
namespace NLog.LogReceiverService;

public sealed class WcfLogReceiverTwoWayClient : 
  WcfLogReceiverClientBase<ILogReceiverTwoWayClient>,
  ILogReceiverTwoWayClient
{
  public WcfLogReceiverTwoWayClient()
  {
  }

  public WcfLogReceiverTwoWayClient(string endpointConfigurationName)
    : base(endpointConfigurationName)
  {
  }

  public WcfLogReceiverTwoWayClient(string endpointConfigurationName, string remoteAddress)
    : base(endpointConfigurationName, remoteAddress)
  {
  }

  public WcfLogReceiverTwoWayClient(string endpointConfigurationName, EndpointAddress remoteAddress)
    : base(endpointConfigurationName, remoteAddress)
  {
  }

  public WcfLogReceiverTwoWayClient(Binding binding, EndpointAddress remoteAddress)
    : base(binding, remoteAddress)
  {
  }

  public override IAsyncResult BeginProcessLogMessages(
    NLogEvents events,
    AsyncCallback callback,
    object asyncState)
  {
    return this.Channel.BeginProcessLogMessages(events, callback, asyncState);
  }

  public override void EndProcessLogMessages(IAsyncResult result)
  {
    this.Channel.EndProcessLogMessages(result);
  }
}
