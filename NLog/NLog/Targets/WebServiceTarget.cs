// Decompiled with JetBrains decompiler
// Type: NLog.Targets.WebServiceTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

#nullable disable
namespace NLog.Targets;

[Target("WebService")]
public sealed class WebServiceTarget : MethodCallTargetBase
{
  private const string SoapEnvelopeNamespaceUri = "http://schemas.xmlsoap.org/soap/envelope/";
  private const string Soap12EnvelopeNamespaceUri = "http://www.w3.org/2003/05/soap-envelope";
  private static Dictionary<WebServiceProtocol, Func<WebServiceTarget, WebServiceTarget.HttpPostFormatterBase>> _postFormatterFactories = new Dictionary<WebServiceProtocol, Func<WebServiceTarget, WebServiceTarget.HttpPostFormatterBase>>()
  {
    {
      WebServiceProtocol.Soap11,
      (Func<WebServiceTarget, WebServiceTarget.HttpPostFormatterBase>) (t => (WebServiceTarget.HttpPostFormatterBase) new WebServiceTarget.HttpPostSoap11Formatter(t))
    },
    {
      WebServiceProtocol.Soap12,
      (Func<WebServiceTarget, WebServiceTarget.HttpPostFormatterBase>) (t => (WebServiceTarget.HttpPostFormatterBase) new WebServiceTarget.HttpPostSoap12Formatter(t))
    },
    {
      WebServiceProtocol.HttpPost,
      (Func<WebServiceTarget, WebServiceTarget.HttpPostFormatterBase>) (t => (WebServiceTarget.HttpPostFormatterBase) new WebServiceTarget.HttpPostFormEncodedFormatter(t))
    },
    {
      WebServiceProtocol.JsonPost,
      (Func<WebServiceTarget, WebServiceTarget.HttpPostFormatterBase>) (t => (WebServiceTarget.HttpPostFormatterBase) new WebServiceTarget.HttpPostJsonFormatter(t))
    },
    {
      WebServiceProtocol.XmlPost,
      (Func<WebServiceTarget, WebServiceTarget.HttpPostFormatterBase>) (t => (WebServiceTarget.HttpPostFormatterBase) new WebServiceTarget.HttpPostXmlDocumentFormatter(t))
    }
  };
  private KeyValuePair<WebServiceProtocol, WebServiceTarget.HttpPostFormatterBase> _activeProtocol;
  private KeyValuePair<WebServiceProxyType, IWebProxy> _activeProxy;
  private readonly AsyncOperationCounter _pendingManualFlushList = new AsyncOperationCounter();

  public WebServiceTarget()
  {
    this.Protocol = WebServiceProtocol.Soap11;
    this.Encoding = (Encoding) new UTF8Encoding(false);
    this.IncludeBOM = new bool?(false);
    this.OptimizeBufferReuse = true;
    this.Headers = (IList<MethodCallParameter>) new List<MethodCallParameter>();
  }

  public WebServiceTarget(string name)
    : this()
  {
    this.Name = name;
  }

  public Uri Url { get; set; }

  public Layout UserAgent { get; set; }

  public string MethodName { get; set; }

  public string Namespace { get; set; }

  [DefaultValue("Soap11")]
  public WebServiceProtocol Protocol
  {
    get => this._activeProtocol.Key;
    set
    {
      this._activeProtocol = new KeyValuePair<WebServiceProtocol, WebServiceTarget.HttpPostFormatterBase>(value, (WebServiceTarget.HttpPostFormatterBase) null);
    }
  }

  [DefaultValue("DefaultWebProxy")]
  public WebServiceProxyType ProxyType
  {
    get => this._activeProxy.Key;
    set
    {
      this._activeProxy = new KeyValuePair<WebServiceProxyType, IWebProxy>(value, (IWebProxy) null);
    }
  }

  public string ProxyAddress { get; set; }

  public bool? IncludeBOM { get; set; }

  public Encoding Encoding { get; set; }

  public bool EscapeDataRfc3986 { get; set; }

  public bool EscapeDataNLogLegacy { get; set; }

  public string XmlRoot { get; set; }

  public string XmlRootNamespace { get; set; }

  [ArrayParameter(typeof (MethodCallParameter), "header")]
  public IList<MethodCallParameter> Headers { get; private set; }

  public bool PreAuthenticate { get; set; }

  protected override void DoInvoke(object[] parameters) => throw new NotImplementedException();

  protected override void DoInvoke(object[] parameters, AsyncContinuation continuation)
  {
    HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(this.BuildWebServiceUrl(parameters));
    this.DoInvoke(parameters, webRequest, continuation);
  }

  protected override void DoInvoke(object[] parameters, AsyncLogEventInfo logEvent)
  {
    HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(this.BuildWebServiceUrl(parameters));
    IList<MethodCallParameter> headers = this.Headers;
    if ((headers != null ? (headers.Count > 0 ? 1 : 0) : 0) != 0)
    {
      for (int index = 0; index < this.Headers.Count; ++index)
      {
        string str = this.RenderLogEvent(this.Headers[index].Layout, logEvent.LogEvent);
        if (str != null)
          webRequest.Headers[this.Headers[index].Name] = str;
      }
    }
    string str1 = this.RenderLogEvent(this.UserAgent, logEvent.LogEvent);
    if (!string.IsNullOrEmpty(str1))
      webRequest.UserAgent = str1;
    this.DoInvoke(parameters, webRequest, logEvent.Continuation);
  }

  private void DoInvoke(
    object[] parameters,
    HttpWebRequest webRequest,
    AsyncContinuation continuation)
  {
    Func<HttpWebRequest, AsyncCallback, IAsyncResult> beginGetRequest = (Func<HttpWebRequest, AsyncCallback, IAsyncResult>) ((request, result) => request.BeginGetRequestStream(result, (object) null));
    Func<HttpWebRequest, IAsyncResult, Stream> getRequestStream = (Func<HttpWebRequest, IAsyncResult, Stream>) ((request, result) => request.EndGetRequestStream(result));
    switch (this.ProxyType)
    {
      case WebServiceProxyType.DefaultWebProxy:
        if (this.PreAuthenticate || this.ProxyType == WebServiceProxyType.AutoProxy)
          webRequest.PreAuthenticate = true;
        this.DoInvoke(parameters, continuation, webRequest, beginGetRequest, getRequestStream);
        break;
      case WebServiceProxyType.AutoProxy:
        if (this._activeProxy.Value == null)
        {
          IWebProxy systemWebProxy = WebRequest.GetSystemWebProxy();
          systemWebProxy.Credentials = CredentialCache.DefaultCredentials;
          this._activeProxy = new KeyValuePair<WebServiceProxyType, IWebProxy>(this.ProxyType, systemWebProxy);
        }
        webRequest.Proxy = this._activeProxy.Value;
        goto case WebServiceProxyType.DefaultWebProxy;
      case WebServiceProxyType.ProxyAddress:
        if (!string.IsNullOrEmpty(this.ProxyAddress))
        {
          if (this._activeProxy.Value == null)
            this._activeProxy = new KeyValuePair<WebServiceProxyType, IWebProxy>(this.ProxyType, (IWebProxy) new WebProxy(this.ProxyAddress, true));
          webRequest.Proxy = this._activeProxy.Value;
          goto case WebServiceProxyType.DefaultWebProxy;
        }
        goto case WebServiceProxyType.DefaultWebProxy;
      default:
        webRequest.Proxy = (IWebProxy) null;
        goto case WebServiceProxyType.DefaultWebProxy;
    }
  }

  internal void DoInvoke(
    object[] parameters,
    AsyncContinuation continuation,
    HttpWebRequest webRequest,
    Func<HttpWebRequest, AsyncCallback, IAsyncResult> beginGetRequest,
    Func<HttpWebRequest, IAsyncResult, Stream> getRequestStream)
  {
    Stream postPayload = (Stream) null;
    if (this.Protocol == WebServiceProtocol.HttpGet)
    {
      this.PrepareGetRequest(webRequest);
    }
    else
    {
      if (this._activeProtocol.Value == null)
        this._activeProtocol = new KeyValuePair<WebServiceProtocol, WebServiceTarget.HttpPostFormatterBase>(this.Protocol, WebServiceTarget._postFormatterFactories[this.Protocol](this));
      postPayload = (Stream) this._activeProtocol.Value.PrepareRequest(webRequest, parameters);
    }
    AsyncContinuation sendContinuation = this.CreateSendContinuation(continuation, webRequest);
    this.PostPayload(continuation, webRequest, beginGetRequest, getRequestStream, postPayload, sendContinuation);
  }

  private AsyncContinuation CreateSendContinuation(
    AsyncContinuation continuation,
    HttpWebRequest webRequest)
  {
    return (AsyncContinuation) (ex =>
    {
      if (ex != null)
      {
        this.DoInvokeCompleted(continuation, ex);
      }
      else
      {
        try
        {
          webRequest.BeginGetResponse((AsyncCallback) (r =>
          {
            try
            {
              using (webRequest.EndGetResponse(r))
                ;
              this.DoInvokeCompleted(continuation, (Exception) null);
            }
            catch (Exception ex1)
            {
              InternalLogger.Error(ex1, "WebServiceTarget(Name={0}): Error sending request", (object) this.Name);
              if (ex1.MustBeRethrownImmediately())
                throw;
              this.DoInvokeCompleted(continuation, ex1);
            }
          }), (object) null);
        }
        catch (Exception ex2)
        {
          InternalLogger.Error(ex2, "WebServiceTarget(Name={0}): Error starting request", (object) this.Name);
          if (this.ExceptionMustBeRethrown(ex2))
            throw;
          this.DoInvokeCompleted(continuation, ex2);
        }
      }
    });
  }

  private void PostPayload(
    AsyncContinuation continuation,
    HttpWebRequest webRequest,
    Func<HttpWebRequest, AsyncCallback, IAsyncResult> beginGetRequest,
    Func<HttpWebRequest, IAsyncResult, Stream> getRequestStream,
    Stream postPayload,
    AsyncContinuation sendContinuation)
  {
    if (postPayload != null && postPayload.Length > 0L)
    {
      postPayload.Position = 0L;
      try
      {
        this._pendingManualFlushList.BeginOperation();
        IAsyncResult asyncResult = beginGetRequest(webRequest, (AsyncCallback) (result =>
        {
          try
          {
            using (Stream output = getRequestStream(webRequest, result))
            {
              WebServiceTarget.WriteStreamAndFixPreamble(postPayload, output, this.IncludeBOM, this.Encoding);
              postPayload.Dispose();
            }
            sendContinuation((Exception) null);
          }
          catch (Exception ex)
          {
            InternalLogger.Error(ex, "WebServiceTarget(Name={0}): Error sending post data", (object) this.Name);
            if (ex.MustBeRethrownImmediately())
              throw;
            postPayload.Dispose();
            this.DoInvokeCompleted(continuation, ex);
          }
        }));
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "WebServiceTarget(Name={0}): Error starting post data", (object) this.Name);
        if (this.ExceptionMustBeRethrown(ex))
          throw;
        this.DoInvokeCompleted(continuation, ex);
      }
    }
    else
    {
      this._pendingManualFlushList.BeginOperation();
      sendContinuation((Exception) null);
    }
  }

  private void DoInvokeCompleted(AsyncContinuation continuation, Exception ex)
  {
    this._pendingManualFlushList.CompleteOperation(ex);
    continuation(ex);
  }

  protected override void FlushAsync(AsyncContinuation asyncContinuation)
  {
    this._pendingManualFlushList.RegisterCompletionNotification(asyncContinuation)((Exception) null);
  }

  protected override void CloseTarget()
  {
    this._pendingManualFlushList.Clear();
    base.CloseTarget();
  }

  private Uri BuildWebServiceUrl(object[] parameterValues)
  {
    if (this.Protocol != WebServiceProtocol.HttpGet)
      return this.Url;
    string str;
    using (ReusableObjectCreator<StringBuilder>.LockOject lockOject = this.OptimizeBufferReuse ? this.ReusableLayoutBuilder.Allocate() : this.ReusableLayoutBuilder.None)
    {
      StringBuilder sb = lockOject.Result ?? new StringBuilder();
      UrlHelper.EscapeEncodingOptions stringEncodingFlags = UrlHelper.GetUriStringEncodingFlags(this.EscapeDataNLogLegacy, false, this.EscapeDataRfc3986);
      this.BuildWebServiceQueryParameters(parameterValues, sb, stringEncodingFlags);
      str = sb.ToString();
    }
    UriBuilder uriBuilder = new UriBuilder(this.Url);
    uriBuilder.Query = uriBuilder.Query == null || uriBuilder.Query.Length <= 1 ? str : $"{uriBuilder.Query.Substring(1)}&{str}";
    return uriBuilder.Uri;
  }

  private void BuildWebServiceQueryParameters(
    object[] parameterValues,
    StringBuilder sb,
    UrlHelper.EscapeEncodingOptions encodingOptions)
  {
    string str = string.Empty;
    for (int index = 0; index < this.Parameters.Count; ++index)
    {
      sb.Append(str);
      sb.Append(this.Parameters[index].Name);
      sb.Append("=");
      string source = XmlHelper.XmlConvertToString(parameterValues[index]);
      if (!string.IsNullOrEmpty(source))
        UrlHelper.EscapeDataEncode(source, sb, encodingOptions);
      str = "&";
    }
  }

  private void PrepareGetRequest(HttpWebRequest webRequest) => webRequest.Method = "GET";

  private static void WriteStreamAndFixPreamble(
    Stream input,
    Stream output,
    bool? writeUtf8BOM,
    Encoding encoding)
  {
    bool flag1 = !writeUtf8BOM.HasValue || !(encoding is UTF8Encoding);
    if (!flag1)
    {
      bool flag2 = encoding.GetPreamble().Length == 3;
      flag1 = writeUtf8BOM.Value & flag2 || !writeUtf8BOM.Value && !flag2;
    }
    int offset = flag1 ? 0 : 3;
    input.CopyWithOffset(output, offset);
  }

  private abstract class HttpPostFormatterBase
  {
    private string _contentType;

    protected HttpPostFormatterBase(WebServiceTarget target) => this.Target = target;

    protected string ContentType
    {
      get => this._contentType ?? (this._contentType = this.GetContentType(this.Target));
    }

    protected WebServiceTarget Target { get; private set; }

    protected virtual string GetContentType(WebServiceTarget target)
    {
      return "charset=" + target.Encoding.WebName;
    }

    public MemoryStream PrepareRequest(HttpWebRequest request, object[] parameterValues)
    {
      this.InitRequest(request);
      MemoryStream ms = new MemoryStream();
      this.WriteContent(ms, parameterValues);
      return ms;
    }

    protected virtual void InitRequest(HttpWebRequest request)
    {
      request.Method = "POST";
      request.ContentType = this.ContentType;
    }

    protected abstract void WriteContent(MemoryStream ms, object[] parameterValues);
  }

  private class HttpPostFormEncodedFormatter : WebServiceTarget.HttpPostTextFormatterBase
  {
    private readonly UrlHelper.EscapeEncodingOptions _encodingOptions;

    public HttpPostFormEncodedFormatter(WebServiceTarget target)
      : base(target)
    {
      this._encodingOptions = UrlHelper.GetUriStringEncodingFlags(target.EscapeDataNLogLegacy, true, target.EscapeDataRfc3986);
    }

    protected override string GetContentType(WebServiceTarget target)
    {
      return $"application/x-www-form-urlencoded; {base.GetContentType(target)}";
    }

    protected override void WriteStringContent(StringBuilder builder, object[] parameterValues)
    {
      this.Target.BuildWebServiceQueryParameters(parameterValues, builder, this._encodingOptions);
    }
  }

  private class HttpPostJsonFormatter(WebServiceTarget target) : 
    WebServiceTarget.HttpPostTextFormatterBase(target)
  {
    private IJsonConverter _jsonConverter;

    private IJsonConverter JsonConverter
    {
      get
      {
        return this._jsonConverter ?? (this._jsonConverter = ConfigurationItemFactory.Default.JsonConverter);
      }
    }

    protected override string GetContentType(WebServiceTarget target)
    {
      return $"application/json; {base.GetContentType(target)}";
    }

    protected override void WriteStringContent(StringBuilder builder, object[] parameterValues)
    {
      if (this.Target.Parameters.Count == 1 && string.IsNullOrEmpty(this.Target.Parameters[0].Name) && parameterValues[0] is string parameterValue)
      {
        builder.Append(parameterValue);
      }
      else
      {
        builder.Append("{");
        string str = string.Empty;
        for (int index = 0; index < this.Target.Parameters.Count; ++index)
        {
          MethodCallParameter parameter = this.Target.Parameters[index];
          builder.Append(str);
          builder.Append('"');
          builder.Append(parameter.Name);
          builder.Append("\":");
          this.JsonConverter.SerializeObject(parameterValues[index], builder);
          str = ",";
        }
        builder.Append('}');
      }
    }
  }

  private class HttpPostSoap11Formatter : WebServiceTarget.HttpPostSoapFormatterBase
  {
    private readonly string _defaultSoapAction;

    public HttpPostSoap11Formatter(WebServiceTarget target)
      : base(target)
    {
      this._defaultSoapAction = WebServiceTarget.HttpPostSoapFormatterBase.GetDefaultSoapAction(target);
    }

    protected override string SoapEnvelopeNamespace => "http://schemas.xmlsoap.org/soap/envelope/";

    protected override string SoapName => "soap";

    protected override string GetContentType(WebServiceTarget target)
    {
      return $"text/xml; {base.GetContentType(target)}";
    }

    protected override void InitRequest(HttpWebRequest request)
    {
      base.InitRequest(request);
      IList<MethodCallParameter> headers = this.Target.Headers;
      if ((headers != null ? (headers.Count == 0 ? 1 : 0) : 0) == 0 && !string.IsNullOrEmpty(request.Headers["SOAPAction"]))
        return;
      request.Headers["SOAPAction"] = this._defaultSoapAction;
    }
  }

  private class HttpPostSoap12Formatter(WebServiceTarget target) : 
    WebServiceTarget.HttpPostSoapFormatterBase(target)
  {
    protected override string SoapEnvelopeNamespace => "http://www.w3.org/2003/05/soap-envelope";

    protected override string SoapName => "soap12";

    protected override string GetContentType(WebServiceTarget target)
    {
      return this.GetContentTypeSoap12(target, WebServiceTarget.HttpPostSoapFormatterBase.GetDefaultSoapAction(target));
    }

    protected override void InitRequest(HttpWebRequest request)
    {
      base.InitRequest(request);
      IList<MethodCallParameter> headers = this.Target.Headers;
      string soapAction = (headers != null ? (headers.Count > 0 ? 1 : 0) : 0) != 0 ? request.Headers["SOAPAction"] : string.Empty;
      if (string.IsNullOrEmpty(soapAction))
        return;
      request.ContentType = this.GetContentTypeSoap12(this.Target, soapAction);
    }

    private string GetContentTypeSoap12(WebServiceTarget target, string soapAction)
    {
      return $"application/soap+xml; {base.GetContentType(target)}; action=\"{soapAction}\"";
    }
  }

  private abstract class HttpPostSoapFormatterBase : WebServiceTarget.HttpPostXmlFormatterBase
  {
    private readonly XmlWriterSettings _xmlWriterSettings;

    protected HttpPostSoapFormatterBase(WebServiceTarget target)
      : base(target)
    {
      this._xmlWriterSettings = new XmlWriterSettings()
      {
        Encoding = target.Encoding
      };
    }

    protected abstract string SoapEnvelopeNamespace { get; }

    protected abstract string SoapName { get; }

    protected override void WriteContent(MemoryStream ms, object[] parameterValues)
    {
      XmlWriter currentXmlWriter = XmlWriter.Create((Stream) ms, this._xmlWriterSettings);
      currentXmlWriter.WriteStartElement(this.SoapName, "Envelope", this.SoapEnvelopeNamespace);
      currentXmlWriter.WriteStartElement("Body", this.SoapEnvelopeNamespace);
      currentXmlWriter.WriteStartElement(this.Target.MethodName, this.Target.Namespace);
      this.WriteAllParametersToCurrenElement(currentXmlWriter, parameterValues);
      currentXmlWriter.WriteEndElement();
      currentXmlWriter.WriteEndElement();
      currentXmlWriter.WriteEndElement();
      currentXmlWriter.Flush();
    }

    protected static string GetDefaultSoapAction(WebServiceTarget target)
    {
      return !target.Namespace.EndsWith("/", StringComparison.Ordinal) ? $"{target.Namespace}/{target.MethodName}" : target.Namespace + target.MethodName;
    }
  }

  private abstract class HttpPostTextFormatterBase : WebServiceTarget.HttpPostFormatterBase
  {
    private readonly ReusableBuilderCreator _reusableStringBuilder = new ReusableBuilderCreator();
    private readonly ReusableBufferCreator _reusableEncodingBuffer = new ReusableBufferCreator(1024 /*0x0400*/);
    private readonly byte[] _encodingPreamble;

    protected HttpPostTextFormatterBase(WebServiceTarget target)
      : base(target)
    {
      this._encodingPreamble = target.Encoding.GetPreamble();
    }

    protected override void WriteContent(MemoryStream ms, object[] parameterValues)
    {
      lock (this._reusableStringBuilder)
      {
        using (ReusableObjectCreator<StringBuilder>.LockOject lockOject1 = this._reusableStringBuilder.Allocate())
        {
          this.WriteStringContent(lockOject1.Result, parameterValues);
          using (ReusableObjectCreator<char[]>.LockOject lockOject2 = this._reusableEncodingBuffer.Allocate())
          {
            if (this._encodingPreamble.Length != 0)
              ms.Write(this._encodingPreamble, 0, this._encodingPreamble.Length);
            lockOject1.Result.CopyToStream(ms, this.Target.Encoding, lockOject2.Result);
          }
        }
      }
    }

    protected abstract void WriteStringContent(StringBuilder builder, object[] parameterValues);
  }

  private class HttpPostXmlDocumentFormatter : WebServiceTarget.HttpPostXmlFormatterBase
  {
    private readonly XmlWriterSettings _xmlWriterSettings;

    public HttpPostXmlDocumentFormatter(WebServiceTarget target)
      : base(target)
    {
      this._xmlWriterSettings = !string.IsNullOrEmpty(target.XmlRoot) ? new XmlWriterSettings()
      {
        Encoding = target.Encoding,
        OmitXmlDeclaration = true,
        Indent = false
      } : throw new InvalidOperationException("WebServiceProtocol.Xml requires WebServiceTarget.XmlRoot to be set.");
    }

    protected override string GetContentType(WebServiceTarget target)
    {
      return $"application/xml; {base.GetContentType(target)}";
    }

    protected override void WriteContent(MemoryStream ms, object[] parameterValues)
    {
      XmlWriter currentXmlWriter = XmlWriter.Create((Stream) ms, this._xmlWriterSettings);
      currentXmlWriter.WriteStartElement(this.Target.XmlRoot, this.Target.XmlRootNamespace);
      this.WriteAllParametersToCurrenElement(currentXmlWriter, parameterValues);
      currentXmlWriter.WriteEndElement();
      currentXmlWriter.Flush();
    }
  }

  private abstract class HttpPostXmlFormatterBase(WebServiceTarget target) : 
    WebServiceTarget.HttpPostFormatterBase(target)
  {
    protected void WriteAllParametersToCurrenElement(
      XmlWriter currentXmlWriter,
      object[] parameterValues)
    {
      for (int index = 0; index < this.Target.Parameters.Count; ++index)
      {
        string stringSafe = XmlHelper.XmlConvertToStringSafe(parameterValues[index]);
        currentXmlWriter.WriteElementString(this.Target.Parameters[index].Name, stringSafe);
      }
    }
  }
}
