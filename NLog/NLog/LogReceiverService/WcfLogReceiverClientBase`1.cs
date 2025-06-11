// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.WcfLogReceiverClientBase`1
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading;

#nullable disable
namespace NLog.LogReceiverService;

public abstract class WcfLogReceiverClientBase<TService> : 
  ClientBase<TService>,
  IWcfLogReceiverClient,
  ICommunicationObject
  where TService : class
{
  protected WcfLogReceiverClientBase()
  {
  }

  protected WcfLogReceiverClientBase(string endpointConfigurationName)
    : base(endpointConfigurationName)
  {
  }

  protected WcfLogReceiverClientBase(string endpointConfigurationName, string remoteAddress)
    : base(endpointConfigurationName, remoteAddress)
  {
  }

  protected WcfLogReceiverClientBase(
    string endpointConfigurationName,
    EndpointAddress remoteAddress)
    : base(endpointConfigurationName, remoteAddress)
  {
  }

  protected WcfLogReceiverClientBase(Binding binding, EndpointAddress remoteAddress)
    : base(binding, remoteAddress)
  {
  }

  public event EventHandler<AsyncCompletedEventArgs> ProcessLogMessagesCompleted;

  public event EventHandler<AsyncCompletedEventArgs> OpenCompleted;

  public event EventHandler<AsyncCompletedEventArgs> CloseCompleted;

  public CookieContainer CookieContainer
  {
    get => this.InnerChannel.GetProperty<IHttpCookieContainerManager>()?.CookieContainer;
    set
    {
      IHttpCookieContainerManager property = this.InnerChannel.GetProperty<IHttpCookieContainerManager>();
      if (property == null)
        throw new InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpCookieContainerBindingElement.");
      property.CookieContainer = value;
    }
  }

  public void OpenAsync() => this.OpenAsync((object) null);

  public void OpenAsync(object userState)
  {
    this.InvokeAsync(new ClientBase<TService>.BeginOperationDelegate(this.OnBeginOpen), (object[]) null, new ClientBase<TService>.EndOperationDelegate(this.OnEndOpen), new SendOrPostCallback(this.OnOpenCompleted), userState);
  }

  public void CloseAsync() => this.CloseAsync((object) null);

  public void CloseAsync(object userState)
  {
    this.InvokeAsync(new ClientBase<TService>.BeginOperationDelegate(this.OnBeginClose), (object[]) null, new ClientBase<TService>.EndOperationDelegate(this.OnEndClose), new SendOrPostCallback(this.OnCloseCompleted), userState);
  }

  public void ProcessLogMessagesAsync(NLogEvents events)
  {
    this.ProcessLogMessagesAsync(events, (object) null);
  }

  public void ProcessLogMessagesAsync(NLogEvents events, object userState)
  {
    this.InvokeAsync(new ClientBase<TService>.BeginOperationDelegate(this.OnBeginProcessLogMessages), new object[1]
    {
      (object) events
    }, new ClientBase<TService>.EndOperationDelegate(this.OnEndProcessLogMessages), new SendOrPostCallback(this.OnProcessLogMessagesCompleted), userState);
  }

  public abstract IAsyncResult BeginProcessLogMessages(
    NLogEvents events,
    AsyncCallback callback,
    object asyncState);

  public abstract void EndProcessLogMessages(IAsyncResult result);

  private IAsyncResult OnBeginProcessLogMessages(
    object[] inValues,
    AsyncCallback callback,
    object asyncState)
  {
    return this.BeginProcessLogMessages((NLogEvents) inValues[0], callback, asyncState);
  }

  private object[] OnEndProcessLogMessages(IAsyncResult result)
  {
    this.EndProcessLogMessages(result);
    return (object[]) null;
  }

  private void OnProcessLogMessagesCompleted(object state)
  {
    if (this.ProcessLogMessagesCompleted == null)
      return;
    ClientBase<TService>.InvokeAsyncCompletedEventArgs completedEventArgs = (ClientBase<TService>.InvokeAsyncCompletedEventArgs) state;
    this.ProcessLogMessagesCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
  }

  private IAsyncResult OnBeginOpen(object[] inValues, AsyncCallback callback, object asyncState)
  {
    return ((ICommunicationObject) this).BeginOpen(callback, asyncState);
  }

  private object[] OnEndOpen(IAsyncResult result)
  {
    ((ICommunicationObject) this).EndOpen(result);
    return (object[]) null;
  }

  private void OnOpenCompleted(object state)
  {
    if (this.OpenCompleted == null)
      return;
    ClientBase<TService>.InvokeAsyncCompletedEventArgs completedEventArgs = (ClientBase<TService>.InvokeAsyncCompletedEventArgs) state;
    this.OpenCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
  }

  private IAsyncResult OnBeginClose(object[] inValues, AsyncCallback callback, object asyncState)
  {
    return ((ICommunicationObject) this).BeginClose(callback, asyncState);
  }

  private object[] OnEndClose(IAsyncResult result)
  {
    ((ICommunicationObject) this).EndClose(result);
    return (object[]) null;
  }

  private void OnCloseCompleted(object state)
  {
    if (this.CloseCompleted == null)
      return;
    ClientBase<TService>.InvokeAsyncCompletedEventArgs completedEventArgs = (ClientBase<TService>.InvokeAsyncCompletedEventArgs) state;
    this.CloseCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
  }

  [SpecialName]
  ClientCredentials IWcfLogReceiverClient.get_ClientCredentials() => this.ClientCredentials;

  [SpecialName]
  IClientChannel IWcfLogReceiverClient.get_InnerChannel() => this.InnerChannel;

  [SpecialName]
  ServiceEndpoint IWcfLogReceiverClient.get_Endpoint() => this.Endpoint;

  void IWcfLogReceiverClient.DisplayInitializationUI() => this.DisplayInitializationUI();
}
