// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.HostNameLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;
using System.Net;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("hostname")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public class HostNameLayoutRenderer : LayoutRenderer
{
  private string _hostName;

  protected override void InitializeLayoutRenderer()
  {
    base.InitializeLayoutRenderer();
    try
    {
      this._hostName = HostNameLayoutRenderer.GetHostName();
      if (!string.IsNullOrEmpty(this._hostName))
        return;
      InternalLogger.Info("HostName is not available.");
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "Error getting host name.");
      if (ex.MustBeRethrown())
        throw;
      this._hostName = string.Empty;
    }
  }

  private static string GetHostName()
  {
    return HostNameLayoutRenderer.TryLookupValue((Func<string>) (() => Environment.GetEnvironmentVariable("HOSTNAME")), "HOSTNAME") ?? HostNameLayoutRenderer.TryLookupValue((Func<string>) (() => Dns.GetHostName()), "DnsHostName") ?? HostNameLayoutRenderer.TryLookupValue((Func<string>) (() => EnvironmentHelper.GetMachineName()), "MachineName");
  }

  private static string TryLookupValue(Func<string> lookupFunc, string lookupType)
  {
    try
    {
      string str = lookupFunc()?.Trim();
      return string.IsNullOrEmpty(str) ? (string) null : str;
    }
    catch (Exception ex)
    {
      object[] objArray = new object[1]
      {
        (object) lookupType
      };
      InternalLogger.Warn(ex, "Failed to lookup {0}", objArray);
      return (string) null;
    }
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.Append(this._hostName);
  }
}
