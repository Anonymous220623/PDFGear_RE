// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.HttpNetworkSender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.IO;
using System.Net;

#nullable disable
namespace NLog.Internal.NetworkSenders;

internal class HttpNetworkSender : QueuedNetworkSender
{
  private readonly Uri _addressUri;

  internal IWebRequestFactory HttpRequestFactory { get; set; } = WebRequestFactory.Instance;

  public HttpNetworkSender(string url)
    : base(url)
  {
    this._addressUri = new Uri(this.Address);
  }

  protected override void BeginRequest(QueuedNetworkSender.NetworkRequestArgs eventArgs)
  {
    AsyncContinuation asyncContinuation = eventArgs.AsyncContinuation;
    byte[] bytes = eventArgs.RequestBuffer;
    int offset = eventArgs.RequestBufferOffset;
    int length = eventArgs.RequestBufferLength;
    WebRequest webRequest = this.HttpRequestFactory.CreateWebRequest(this._addressUri);
    webRequest.Method = "POST";
    AsyncCallback onResponse = (AsyncCallback) (r =>
    {
      try
      {
        using (webRequest.EndGetResponse(r))
          ;
        this.EndRequest(asyncContinuation, (Exception) null);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
          throw;
        this.EndRequest((AsyncContinuation) (_ => asyncContinuation(ex)), (Exception) null);
      }
    });
    webRequest.BeginGetRequestStream((AsyncCallback) (r =>
    {
      try
      {
        using (Stream requestStream = webRequest.EndGetRequestStream(r))
          requestStream.Write(bytes, offset, length);
        webRequest.BeginGetResponse(onResponse, (object) null);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
          throw;
        this.EndRequest((AsyncContinuation) (_ => asyncContinuation(ex)), (Exception) null);
      }
    }), (object) null);
  }
}
