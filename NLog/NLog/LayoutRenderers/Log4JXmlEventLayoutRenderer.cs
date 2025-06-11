// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Log4JXmlEventLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Internal.Fakeables;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("log4jxmlevent")]
[ThreadSafe]
[MutableUnsafe]
public class Log4JXmlEventLayoutRenderer : LayoutRenderer, IUsesStackTrace, IIncludeContext
{
  private static readonly DateTime log4jDateBase = new DateTime(1970, 1, 1);
  private static readonly string dummyNamespace = "http://nlog-project.org/dummynamespace/" + (object) Guid.NewGuid();
  private static readonly string dummyNamespaceRemover = $" xmlns:log4j=\"{Log4JXmlEventLayoutRenderer.dummyNamespace}\"";
  private static readonly string dummyNLogNamespace = "http://nlog-project.org/dummynamespace/" + (object) Guid.NewGuid();
  private static readonly string dummyNLogNamespaceRemover = $" xmlns:nlog=\"{Log4JXmlEventLayoutRenderer.dummyNLogNamespace}\"";
  private readonly NdcLayoutRenderer _ndcLayoutRenderer = new NdcLayoutRenderer()
  {
    Separator = " "
  };
  private readonly NdlcLayoutRenderer _ndlcLayoutRenderer = new NdlcLayoutRenderer()
  {
    Separator = " "
  };
  private readonly string _machineName;
  private XmlWriterSettings _xmlWriterSettings;

  public Log4JXmlEventLayoutRenderer()
    : this(LogFactory.CurrentAppDomain)
  {
  }

  public Log4JXmlEventLayoutRenderer(IAppDomain appDomain)
  {
    this.IncludeNLogData = true;
    this.AppInfo = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}({1})", new object[2]
    {
      (object) appDomain.FriendlyName,
      (object) LogFactory.DefaultAppEnvironment.CurrentProcessId
    });
    this.Parameters = (IList<NLogViewerParameterInfo>) new List<NLogViewerParameterInfo>();
    try
    {
      this._machineName = EnvironmentHelper.GetMachineName();
      if (!string.IsNullOrEmpty(this._machineName))
        return;
      InternalLogger.Info("MachineName is not available.");
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "Error getting machine name.");
      if (ex.MustBeRethrown())
        throw;
      this._machineName = string.Empty;
    }
  }

  protected override void InitializeLayoutRenderer()
  {
    base.InitializeLayoutRenderer();
    this._xmlWriterSettings = new XmlWriterSettings()
    {
      Indent = this.IndentXml,
      ConformanceLevel = ConformanceLevel.Fragment,
      IndentChars = "  "
    };
  }

  [DefaultValue(true)]
  public bool IncludeNLogData { get; set; }

  public bool IndentXml { get; set; }

  public string AppInfo { get; set; }

  public bool IncludeCallSite { get; set; }

  public bool IncludeSourceInfo { get; set; }

  public bool IncludeMdc { get; set; }

  public bool IncludeMdlc { get; set; }

  public bool IncludeNdlc { get; set; }

  [DefaultValue(" ")]
  public string NdlcItemSeparator
  {
    get => this._ndlcLayoutRenderer.Separator;
    set => this._ndlcLayoutRenderer.Separator = value;
  }

  public bool IncludeAllProperties { get; set; }

  public bool IncludeNdc { get; set; }

  [DefaultValue(" ")]
  public string NdcItemSeparator
  {
    get => this._ndcLayoutRenderer.Separator;
    set => this._ndcLayoutRenderer.Separator = value;
  }

  public Layout LoggerName { get; set; }

  StackTraceUsage IUsesStackTrace.StackTraceUsage
  {
    get
    {
      if (this.IncludeSourceInfo)
        return StackTraceUsage.WithSource;
      return this.IncludeCallSite ? StackTraceUsage.WithoutSource : StackTraceUsage.None;
    }
  }

  internal IList<NLogViewerParameterInfo> Parameters { get; set; }

  internal void AppendToStringBuilder(StringBuilder sb, LogEventInfo logEvent)
  {
    this.Append(sb, logEvent);
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    StringBuilder stringBuilder = new StringBuilder();
    using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, this._xmlWriterSettings))
    {
      xmlWriter.WriteStartElement("log4j", "event", Log4JXmlEventLayoutRenderer.dummyNamespace);
      xmlWriter.WriteAttributeSafeString("xmlns", "nlog", (string) null, Log4JXmlEventLayoutRenderer.dummyNLogNamespace);
      xmlWriter.WriteAttributeSafeString("logger", this.LoggerName != null ? this.LoggerName.Render(logEvent) : logEvent.LoggerName);
      xmlWriter.WriteAttributeSafeString("level", logEvent.Level.Name.ToUpperInvariant());
      xmlWriter.WriteAttributeSafeString("timestamp", Convert.ToString((long) (logEvent.TimeStamp.ToUniversalTime() - Log4JXmlEventLayoutRenderer.log4jDateBase).TotalMilliseconds, (IFormatProvider) CultureInfo.InvariantCulture));
      xmlWriter.WriteAttributeSafeString("thread", AsyncHelpers.GetManagedThreadId().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      xmlWriter.WriteElementSafeString("log4j", "message", Log4JXmlEventLayoutRenderer.dummyNamespace, logEvent.FormattedMessage);
      if (logEvent.Exception != null)
        xmlWriter.WriteElementSafeString("log4j", "throwable", Log4JXmlEventLayoutRenderer.dummyNamespace, logEvent.Exception.ToString());
      this.AppendNdc(xmlWriter, logEvent);
      Log4JXmlEventLayoutRenderer.AppendException(logEvent, xmlWriter);
      this.AppendCallSite(logEvent, xmlWriter);
      this.AppendProperties(xmlWriter);
      this.AppendMdlc(xmlWriter);
      if (this.IncludeAllProperties)
        this.AppendProperties("log4j", xmlWriter, logEvent);
      this.AppendParameters(logEvent, xmlWriter);
      xmlWriter.WriteStartElement("log4j", "data", Log4JXmlEventLayoutRenderer.dummyNamespace);
      xmlWriter.WriteAttributeSafeString("name", "log4japp");
      xmlWriter.WriteAttributeSafeString("value", this.AppInfo);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("log4j", "data", Log4JXmlEventLayoutRenderer.dummyNamespace);
      xmlWriter.WriteAttributeSafeString("name", "log4jmachinename");
      xmlWriter.WriteAttributeSafeString("value", this._machineName);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.Flush();
      stringBuilder.Replace(Log4JXmlEventLayoutRenderer.dummyNamespaceRemover, string.Empty);
      stringBuilder.Replace(Log4JXmlEventLayoutRenderer.dummyNLogNamespaceRemover, string.Empty);
      stringBuilder.CopyTo(builder);
    }
  }

  private void AppendMdlc(XmlWriter xtw)
  {
    if (!this.IncludeMdlc)
      return;
    foreach (string name in (IEnumerable<string>) MappedDiagnosticsLogicalContext.GetNames())
    {
      string str = XmlHelper.XmlConvertToString(MappedDiagnosticsLogicalContext.GetObject(name));
      if (str != null)
      {
        xtw.WriteStartElement("log4j", "data", Log4JXmlEventLayoutRenderer.dummyNamespace);
        xtw.WriteAttributeSafeString("name", name);
        xtw.WriteAttributeSafeString("value", str);
        xtw.WriteEndElement();
      }
    }
  }

  private void AppendNdc(XmlWriter xtw, LogEventInfo logEvent)
  {
    string str = (string) null;
    if (this.IncludeNdc)
      str = this._ndcLayoutRenderer.Render(logEvent);
    if (this.IncludeNdlc)
    {
      if (str != null)
        str += this.NdcItemSeparator;
      str += this._ndlcLayoutRenderer.Render(logEvent);
    }
    if (str == null)
      return;
    xtw.WriteElementSafeString("log4j", "NDC", Log4JXmlEventLayoutRenderer.dummyNamespace, str);
  }

  private static void AppendException(LogEventInfo logEvent, XmlWriter xtw)
  {
    if (logEvent.Exception == null)
      return;
    xtw.WriteStartElement("log4j", "throwable", Log4JXmlEventLayoutRenderer.dummyNamespace);
    xtw.WriteSafeCData(logEvent.Exception.ToString());
    xtw.WriteEndElement();
  }

  private void AppendParameters(LogEventInfo logEvent, XmlWriter xtw)
  {
    int index = 0;
    while (true)
    {
      int num = index;
      int? count = this.Parameters?.Count;
      int valueOrDefault = count.GetValueOrDefault();
      if (num < valueOrDefault & count.HasValue)
      {
        NLogViewerParameterInfo parameter = this.Parameters[index];
        if (!string.IsNullOrEmpty(parameter?.Name))
        {
          string str = parameter.Layout?.Render(logEvent) ?? string.Empty;
          if (parameter.IncludeEmptyValue || !string.IsNullOrEmpty(str))
          {
            xtw.WriteStartElement("log4j", "data", Log4JXmlEventLayoutRenderer.dummyNamespace);
            xtw.WriteAttributeSafeString("name", parameter.Name);
            xtw.WriteAttributeSafeString("value", str);
            xtw.WriteEndElement();
          }
        }
        ++index;
      }
      else
        break;
    }
  }

  private void AppendProperties(XmlWriter xtw)
  {
    xtw.WriteStartElement("log4j", "properties", Log4JXmlEventLayoutRenderer.dummyNamespace);
    if (!this.IncludeMdc)
      return;
    foreach (string name in (IEnumerable<string>) MappedDiagnosticsContext.GetNames())
    {
      string str = XmlHelper.XmlConvertToString(MappedDiagnosticsContext.GetObject(name));
      if (str != null)
      {
        xtw.WriteStartElement("log4j", "data", Log4JXmlEventLayoutRenderer.dummyNamespace);
        xtw.WriteAttributeSafeString("name", name);
        xtw.WriteAttributeSafeString("value", str);
        xtw.WriteEndElement();
      }
    }
  }

  private void AppendCallSite(LogEventInfo logEvent, XmlWriter xtw)
  {
    if (!this.IncludeCallSite && !this.IncludeSourceInfo || logEvent.CallSiteInformation == null)
      return;
    MethodBase stackFrameMethod = logEvent.CallSiteInformation.GetCallerStackFrameMethod(0);
    string callerClassName = logEvent.CallSiteInformation.GetCallerClassName(stackFrameMethod, true, true, true);
    string callerMemberName = logEvent.CallSiteInformation.GetCallerMemberName(stackFrameMethod, true, true, true);
    xtw.WriteStartElement("log4j", "locationInfo", Log4JXmlEventLayoutRenderer.dummyNamespace);
    if (!string.IsNullOrEmpty(callerClassName))
      xtw.WriteAttributeSafeString("class", callerClassName);
    xtw.WriteAttributeSafeString("method", callerMemberName);
    int num;
    if (this.IncludeSourceInfo)
    {
      xtw.WriteAttributeSafeString("file", logEvent.CallSiteInformation.GetCallerFilePath(0));
      XmlWriter writer = xtw;
      num = logEvent.CallSiteInformation.GetCallerLineNumber(0);
      string str = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      writer.WriteAttributeSafeString("line", str);
    }
    xtw.WriteEndElement();
    if (!this.IncludeNLogData)
      return;
    XmlWriter writer1 = xtw;
    string dummyNlogNamespace = Log4JXmlEventLayoutRenderer.dummyNLogNamespace;
    num = logEvent.SequenceID;
    string str1 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    writer1.WriteElementSafeString("nlog", "eventSequenceNumber", dummyNlogNamespace, str1);
    xtw.WriteStartElement("nlog", "locationInfo", Log4JXmlEventLayoutRenderer.dummyNLogNamespace);
    Type declaringType = stackFrameMethod?.DeclaringType;
    if (declaringType != (Type) null)
      xtw.WriteAttributeSafeString("assembly", declaringType.GetAssembly().FullName);
    xtw.WriteEndElement();
    xtw.WriteStartElement("nlog", "properties", Log4JXmlEventLayoutRenderer.dummyNLogNamespace);
    this.AppendProperties("nlog", xtw, logEvent);
    xtw.WriteEndElement();
  }

  private void AppendProperties(string prefix, XmlWriter xtw, LogEventInfo logEvent)
  {
    if (!logEvent.HasProperties)
      return;
    foreach (KeyValuePair<object, object> property in (IEnumerable<KeyValuePair<object, object>>) logEvent.Properties)
    {
      string str1 = XmlHelper.XmlConvertToString(property.Key);
      if (!string.IsNullOrEmpty(str1))
      {
        string str2 = XmlHelper.XmlConvertToString(property.Value);
        if (str2 != null)
        {
          xtw.WriteStartElement(prefix, "data", Log4JXmlEventLayoutRenderer.dummyNamespace);
          xtw.WriteAttributeSafeString("name", str1);
          xtw.WriteAttributeSafeString("value", str2);
          xtw.WriteEndElement();
        }
      }
    }
  }
}
