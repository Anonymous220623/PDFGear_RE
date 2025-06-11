// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.WcfLogReceiverClient
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.ComponentModel;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

#nullable disable
namespace NLog.LogReceiverService;

public sealed class WcfLogReceiverClient : IWcfLogReceiverClient, ICommunicationObject
{
  public IWcfLogReceiverClient ProxiedClient { get; private set; }

  public bool UseOneWay { get; private set; }

  public WcfLogReceiverClient(bool useOneWay)
  {
    this.UseOneWay = useOneWay;
    this.ProxiedClient = useOneWay ? (IWcfLogReceiverClient) new WcfLogReceiverOneWayClient() : (IWcfLogReceiverClient) new WcfLogReceiverTwoWayClient();
  }

  public WcfLogReceiverClient(bool useOneWay, string endpointConfigurationName)
  {
    this.UseOneWay = useOneWay;
    this.ProxiedClient = useOneWay ? (IWcfLogReceiverClient) new WcfLogReceiverOneWayClient(endpointConfigurationName) : (IWcfLogReceiverClient) new WcfLogReceiverTwoWayClient(endpointConfigurationName);
  }

  public WcfLogReceiverClient(
    bool useOneWay,
    string endpointConfigurationName,
    string remoteAddress)
  {
    this.UseOneWay = useOneWay;
    this.ProxiedClient = useOneWay ? (IWcfLogReceiverClient) new WcfLogReceiverOneWayClient(endpointConfigurationName, remoteAddress) : (IWcfLogReceiverClient) new WcfLogReceiverTwoWayClient(endpointConfigurationName, remoteAddress);
  }

  public WcfLogReceiverClient(
    bool useOneWay,
    string endpointConfigurationName,
    EndpointAddress remoteAddress)
  {
    this.UseOneWay = useOneWay;
    this.ProxiedClient = useOneWay ? (IWcfLogReceiverClient) new WcfLogReceiverOneWayClient(endpointConfigurationName, remoteAddress) : (IWcfLogReceiverClient) new WcfLogReceiverTwoWayClient(endpointConfigurationName, remoteAddress);
  }

  public WcfLogReceiverClient(bool useOneWay, Binding binding, EndpointAddress remoteAddress)
  {
    this.UseOneWay = useOneWay;
    this.ProxiedClient = useOneWay ? (IWcfLogReceiverClient) new WcfLogReceiverOneWayClient(binding, remoteAddress) : (IWcfLogReceiverClient) new WcfLogReceiverTwoWayClient(binding, remoteAddress);
  }

  public void Abort() => this.ProxiedClient.Abort();

  public IAsyncResult BeginClose(AsyncCallback callback, object state)
  {
    return this.ProxiedClient.BeginClose(callback, state);
  }

  public IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
  {
    return this.ProxiedClient.BeginClose(timeout, callback, state);
  }

  public IAsyncResult BeginOpen(AsyncCallback callback, object state)
  {
    return this.ProxiedClient.BeginOpen(callback, state);
  }

  public IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
  {
    return this.ProxiedClient.BeginOpen(timeout, callback, state);
  }

  public IAsyncResult BeginProcessLogMessages(
    NLogEvents events,
    AsyncCallback callback,
    object asyncState)
  {
    return this.ProxiedClient.BeginProcessLogMessages(events, callback, asyncState);
  }

  public ClientCredentials ClientCredentials => this.ProxiedClient.ClientCredentials;

  public void Close(TimeSpan timeout) => this.ProxiedClient.Close(timeout);

  public void Close() => this.ProxiedClient.Close();

  public void CloseAsync(object userState) => this.ProxiedClient.CloseAsync(userState);

  public void CloseAsync() => this.ProxiedClient.CloseAsync();

  public event EventHandler<AsyncCompletedEventArgs> CloseCompleted
  {
    add => this.ProxiedClient.CloseCompleted += value;
    remove => this.ProxiedClient.CloseCompleted -= value;
  }

  public event EventHandler Closed
  {
    add => this.ProxiedClient.Closed += value;
    remove => this.ProxiedClient.Closed -= value;
  }

  public event EventHandler Closing
  {
    add => this.ProxiedClient.Closing += value;
    remove => this.ProxiedClient.Closing -= value;
  }

  public void DisplayInitializationUI() => this.ProxiedClient.DisplayInitializationUI();

  public CookieContainer CookieContainer
  {
    get => this.ProxiedClient.CookieContainer;
    set => this.ProxiedClient.CookieContainer = value;
  }

  public void EndClose(IAsyncResult result) => this.ProxiedClient.EndClose(result);

  public void EndOpen(IAsyncResult result) => this.ProxiedClient.EndOpen(result);

  public ServiceEndpoint Endpoint => this.ProxiedClient.Endpoint;

  public void EndProcessLogMessages(IAsyncResult result)
  {
    this.ProxiedClient.EndProcessLogMessages(result);
  }

  public event EventHandler Faulted
  {
    add => this.ProxiedClient.Faulted += value;
    remove => this.ProxiedClient.Faulted -= value;
  }

  public IClientChannel InnerChannel => this.ProxiedClient.InnerChannel;

  public void Open() => this.ProxiedClient.Open();

  public void Open(TimeSpan timeout) => this.ProxiedClient.Open(timeout);

  public void OpenAsync() => this.ProxiedClient.OpenAsync();

  public void OpenAsync(object userState) => this.ProxiedClient.OpenAsync(userState);

  public event EventHandler<AsyncCompletedEventArgs> OpenCompleted
  {
    add => this.ProxiedClient.OpenCompleted += value;
    remove => this.ProxiedClient.OpenCompleted -= value;
  }

  public event EventHandler Opened
  {
    add => this.ProxiedClient.Opened += value;
    remove => this.ProxiedClient.Opened -= value;
  }

  public event EventHandler Opening
  {
    add => this.ProxiedClient.Opening += value;
    remove => this.ProxiedClient.Opening -= value;
  }

  public void ProcessLogMessagesAsync(NLogEvents events)
  {
    this.ProxiedClient.ProcessLogMessagesAsync(events);
  }

  public void ProcessLogMessagesAsync(NLogEvents events, object userState)
  {
    this.ProxiedClient.ProcessLogMessagesAsync(events, userState);
  }

  public event EventHandler<AsyncCompletedEventArgs> ProcessLogMessagesCompleted
  {
    add => this.ProxiedClient.ProcessLogMessagesCompleted += value;
    remove => this.ProxiedClient.ProcessLogMessagesCompleted -= value;
  }

  public CommunicationState State => this.ProxiedClient.State;

  public void CloseCommunicationObject() => this.ProxiedClient.Close();
}
