// Decompiled with JetBrains decompiler
// Type: NLog.Targets.MailTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;

#nullable disable
namespace NLog.Targets;

[Target("Mail")]
public class MailTarget : TargetWithLayoutHeaderAndFooter
{
  private const string RequiredPropertyIsEmptyFormat = "After the processing of the MailTarget's '{0}' property it appears to be empty. The email message will not be sent.";
  private Layout _from;
  private SmtpSection _currentailSettings;

  public MailTarget()
  {
    this.Body = (Layout) "${message}${newline}";
    this.Subject = (Layout) "Message from NLog on ${machinename}";
    this.Encoding = Encoding.UTF8;
    this.SmtpPort = 25;
    this.SmtpAuthentication = SmtpAuthenticationMode.None;
    this.Timeout = 10000;
  }

  internal SmtpSection SmtpSection
  {
    get
    {
      if (this._currentailSettings == null)
      {
        try
        {
          this._currentailSettings = System.Configuration.ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
        }
        catch (Exception ex)
        {
          InternalLogger.Warn(ex, "MailTarget(Name={0}): Reading 'From' from .config failed.", (object) this.Name);
          if (this.ExceptionMustBeRethrown(ex))
            throw;
          this._currentailSettings = new SmtpSection();
        }
      }
      return this._currentailSettings;
    }
    set => this._currentailSettings = value;
  }

  public MailTarget(string name)
    : this()
  {
    this.Name = name;
  }

  public Layout From
  {
    get
    {
      return this.UseSystemNetMailSettings && this._from == null ? (Layout) this.SmtpSection.From : this._from;
    }
    set => this._from = value;
  }

  [RequiredParameter]
  public Layout To { get; set; }

  public Layout CC { get; set; }

  public Layout Bcc { get; set; }

  public bool AddNewLines { get; set; }

  [DefaultValue("Message from NLog on ${machinename}")]
  [RequiredParameter]
  public Layout Subject { get; set; }

  [DefaultValue("${message}${newline}")]
  public Layout Body
  {
    get => this.Layout;
    set => this.Layout = value;
  }

  [DefaultValue("UTF8")]
  public Encoding Encoding { get; set; }

  [DefaultValue(false)]
  public bool Html { get; set; }

  public Layout SmtpServer { get; set; }

  [DefaultValue("None")]
  public SmtpAuthenticationMode SmtpAuthentication { get; set; }

  public Layout SmtpUserName { get; set; }

  public Layout SmtpPassword { get; set; }

  [DefaultValue(false)]
  public bool EnableSsl { get; set; }

  [DefaultValue(25)]
  public int SmtpPort { get; set; }

  [DefaultValue(false)]
  public bool UseSystemNetMailSettings { get; set; }

  [DefaultValue(SmtpDeliveryMethod.Network)]
  public SmtpDeliveryMethod DeliveryMethod { get; set; }

  [DefaultValue(null)]
  public string PickupDirectoryLocation { get; set; }

  public Layout Priority { get; set; }

  [DefaultValue(false)]
  public bool ReplaceNewlineWithBrTagInHtml { get; set; }

  [DefaultValue(10000)]
  public int Timeout { get; set; }

  internal virtual ISmtpClient CreateSmtpClient() => (ISmtpClient) new MySmtpClient();

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    this.Write((IList<AsyncLogEventInfo>) new AsyncLogEventInfo[1]
    {
      logEvent
    });
  }

  [Obsolete("Instead override Write(IList<AsyncLogEventInfo> logEvents. Marked obsolete on NLog 4.5")]
  protected override void Write(AsyncLogEventInfo[] logEvents)
  {
    this.Write((IList<AsyncLogEventInfo>) logEvents);
  }

  protected override void Write(IList<AsyncLogEventInfo> logEvents)
  {
    foreach (KeyValuePair<string, IList<AsyncLogEventInfo>> keyValuePair in logEvents.BucketSort<AsyncLogEventInfo, string>((SortHelpers.KeySelector<AsyncLogEventInfo, string>) (c => this.GetSmtpSettingsKey(c.LogEvent))))
      this.ProcessSingleMailMessage(keyValuePair.Value);
  }

  protected override void InitializeTarget()
  {
    this.CheckRequiredParameters();
    base.InitializeTarget();
  }

  private void ProcessSingleMailMessage([NotNull] IList<AsyncLogEventInfo> events)
  {
    try
    {
      LogEventInfo firstEvent = events.Count != 0 ? events[0].LogEvent : throw new NLogRuntimeException("We need at least one event.");
      LogEventInfo logEvent = events[events.Count - 1].LogEvent;
      StringBuilder bodyBuffer = this.CreateBodyBuffer((IEnumerable<AsyncLogEventInfo>) events, firstEvent, logEvent);
      using (MailMessage mailMessage = this.CreateMailMessage(logEvent, bodyBuffer.ToString()))
      {
        using (ISmtpClient smtpClient = this.CreateSmtpClient())
        {
          if (!this.UseSystemNetMailSettings)
            this.ConfigureMailClient(logEvent, smtpClient);
          if (smtpClient.EnableSsl)
            InternalLogger.Debug("MailTarget(Name={0}): Sending mail to {1} using {2}:{3} (ssl=true)", (object) this.Name, (object) mailMessage.To, (object) smtpClient.Host, (object) smtpClient.Port);
          else
            InternalLogger.Debug("MailTarget(Name={0}): Sending mail to {1} using {2}:{3} (ssl=false)", (object) this.Name, (object) mailMessage.To, (object) smtpClient.Host, (object) smtpClient.Port);
          InternalLogger.Trace<string, string>("MailTarget(Name={0}):   Subject: '{1}'", this.Name, mailMessage.Subject);
          InternalLogger.Trace<string, string>("MailTarget(Name={0}):   From: '{1}'", this.Name, mailMessage.From.ToString());
          smtpClient.Send(mailMessage);
          foreach (AsyncLogEventInfo asyncLogEventInfo in (IEnumerable<AsyncLogEventInfo>) events)
            asyncLogEventInfo.Continuation((Exception) null);
        }
      }
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "MailTarget(Name={0}): Error sending mail.", (object) this.Name);
      if (this.ExceptionMustBeRethrown(ex))
        throw;
      foreach (AsyncLogEventInfo asyncLogEventInfo in (IEnumerable<AsyncLogEventInfo>) events)
        asyncLogEventInfo.Continuation(ex);
    }
  }

  private StringBuilder CreateBodyBuffer(
    IEnumerable<AsyncLogEventInfo> events,
    LogEventInfo firstEvent,
    LogEventInfo lastEvent)
  {
    StringBuilder bodyBuffer = new StringBuilder();
    if (this.Header != null)
    {
      bodyBuffer.Append(this.Header.Render(firstEvent));
      if (this.AddNewLines)
        bodyBuffer.Append("\n");
    }
    foreach (AsyncLogEventInfo asyncLogEventInfo in events)
    {
      bodyBuffer.Append(this.Layout.Render(asyncLogEventInfo.LogEvent));
      if (this.AddNewLines)
        bodyBuffer.Append("\n");
    }
    if (this.Footer != null)
    {
      bodyBuffer.Append(this.Footer.Render(lastEvent));
      if (this.AddNewLines)
        bodyBuffer.Append("\n");
    }
    return bodyBuffer;
  }

  internal void ConfigureMailClient(LogEventInfo lastEvent, ISmtpClient client)
  {
    this.CheckRequiredParameters();
    if (this.SmtpServer == null && string.IsNullOrEmpty(this.PickupDirectoryLocation))
      throw new NLogRuntimeException($"After the processing of the MailTarget's '{"SmtpServer/PickupDirectoryLocation"}' property it appears to be empty. The email message will not be sent.");
    if (this.DeliveryMethod == SmtpDeliveryMethod.Network && this.SmtpServer == null)
      throw new NLogRuntimeException($"After the processing of the MailTarget's '{"SmtpServer"}' property it appears to be empty. The email message will not be sent.");
    if (this.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory && string.IsNullOrEmpty(this.PickupDirectoryLocation))
      throw new NLogRuntimeException($"After the processing of the MailTarget's '{"PickupDirectoryLocation"}' property it appears to be empty. The email message will not be sent.");
    if (this.SmtpServer != null && this.DeliveryMethod == SmtpDeliveryMethod.Network)
    {
      string str = this.SmtpServer.Render(lastEvent);
      client.Host = !string.IsNullOrEmpty(str) ? str : throw new NLogRuntimeException($"After the processing of the MailTarget's '{"SmtpServer"}' property it appears to be empty. The email message will not be sent.");
      client.Port = this.SmtpPort;
      client.EnableSsl = this.EnableSsl;
      if (this.SmtpAuthentication == SmtpAuthenticationMode.Ntlm)
      {
        InternalLogger.Trace<string>("MailTarget(Name={0}):   Using NTLM authentication.", this.Name);
        client.Credentials = (ICredentialsByHost) CredentialCache.DefaultNetworkCredentials;
      }
      else if (this.SmtpAuthentication == SmtpAuthenticationMode.Basic)
      {
        string userName = this.SmtpUserName.Render(lastEvent);
        string password = this.SmtpPassword.Render(lastEvent);
        InternalLogger.Trace<string, string, string>("MailTarget(Name={0}):   Using basic authentication: Username='{1}' Password='{2}'", this.Name, userName, new string('*', password.Length));
        client.Credentials = (ICredentialsByHost) new NetworkCredential(userName, password);
      }
    }
    if (!string.IsNullOrEmpty(this.PickupDirectoryLocation) && this.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
      client.PickupDirectoryLocation = MailTarget.ConvertDirectoryLocation(this.PickupDirectoryLocation);
    client.DeliveryMethod = this.DeliveryMethod;
    client.Timeout = this.Timeout;
  }

  internal static string ConvertDirectoryLocation(string pickupDirectoryLocation)
  {
    return !pickupDirectoryLocation.StartsWith("~/") ? pickupDirectoryLocation : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pickupDirectoryLocation.Substring("~/".Length).Replace('/', Path.DirectorySeparatorChar));
  }

  private void CheckRequiredParameters()
  {
    if (!this.UseSystemNetMailSettings && this.SmtpServer == null && this.DeliveryMethod == SmtpDeliveryMethod.Network)
      throw new NLogConfigurationException("The MailTarget's '{0}' properties are not set - but needed because useSystemNetMailSettings=false and DeliveryMethod=Network. The email message will not be sent.", new object[1]
      {
        (object) "SmtpServer"
      });
    if (!this.UseSystemNetMailSettings && string.IsNullOrEmpty(this.PickupDirectoryLocation) && this.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
      throw new NLogConfigurationException("The MailTarget's '{0}' properties are not set - but needed because useSystemNetMailSettings=false and DeliveryMethod=SpecifiedPickupDirectory. The email message will not be sent.", new object[1]
      {
        (object) "PickupDirectoryLocation"
      });
    if (this.From == null)
      throw new NLogConfigurationException("After the processing of the MailTarget's '{0}' property it appears to be empty. The email message will not be sent.", new object[1]
      {
        (object) "From"
      });
  }

  private string GetSmtpSettingsKey(LogEventInfo logEvent)
  {
    StringBuilder sb = new StringBuilder();
    MailTarget.AppendLayout(sb, logEvent, this.From);
    MailTarget.AppendLayout(sb, logEvent, this.To);
    MailTarget.AppendLayout(sb, logEvent, this.CC);
    MailTarget.AppendLayout(sb, logEvent, this.Bcc);
    MailTarget.AppendLayout(sb, logEvent, this.SmtpServer);
    MailTarget.AppendLayout(sb, logEvent, this.SmtpPassword);
    MailTarget.AppendLayout(sb, logEvent, this.SmtpUserName);
    return sb.ToString();
  }

  private static void AppendLayout(StringBuilder sb, LogEventInfo logEvent, Layout layout)
  {
    sb.Append("|");
    if (layout == null)
      return;
    sb.Append(layout.Render(logEvent));
  }

  private MailMessage CreateMailMessage(LogEventInfo lastEvent, string body)
  {
    MailMessage mailMessage = new MailMessage();
    string address = this.From?.Render(lastEvent);
    mailMessage.From = !string.IsNullOrEmpty(address) ? new MailAddress(address) : throw new NLogRuntimeException("After the processing of the MailTarget's '{0}' property it appears to be empty. The email message will not be sent.", new object[1]
    {
      (object) "From"
    });
    int num = MailTarget.AddAddresses(mailMessage.To, this.To, lastEvent) ? 1 : 0;
    bool flag1 = MailTarget.AddAddresses(mailMessage.CC, this.CC, lastEvent);
    bool flag2 = MailTarget.AddAddresses(mailMessage.Bcc, this.Bcc, lastEvent);
    if (num == 0 && !flag1 && !flag2)
      throw new NLogRuntimeException("After the processing of the MailTarget's '{0}' property it appears to be empty. The email message will not be sent.", new object[1]
      {
        (object) "To/Cc/Bcc"
      });
    mailMessage.Subject = this.Subject == null ? string.Empty : this.Subject.Render(lastEvent).Trim();
    mailMessage.BodyEncoding = this.Encoding;
    mailMessage.IsBodyHtml = this.Html;
    if (this.Priority != null)
    {
      string inputValue = this.Priority.Render(lastEvent);
      if (string.IsNullOrEmpty(inputValue))
      {
        mailMessage.Priority = MailPriority.Normal;
      }
      else
      {
        MailPriority resultValue;
        if (ConversionHelpers.TryParseEnum<MailPriority>(inputValue, out resultValue))
        {
          mailMessage.Priority = resultValue;
        }
        else
        {
          mailMessage.Priority = MailPriority.Normal;
          InternalLogger.Warn<string, string>("MailTarget(Name={0}): Could not convert '{1}' to MailPriority, valid values are Low, Normal and High. Using normal priority as fallback.", this.Name, inputValue);
        }
      }
    }
    mailMessage.Body = body;
    if (mailMessage.IsBodyHtml && this.ReplaceNewlineWithBrTagInHtml && mailMessage.Body != null)
      mailMessage.Body = mailMessage.Body.Replace(EnvironmentHelper.NewLine, "<br/>");
    return mailMessage;
  }

  private static bool AddAddresses(
    MailAddressCollection mailAddressCollection,
    Layout layout,
    LogEventInfo logEvent)
  {
    bool flag = false;
    if (layout != null)
    {
      foreach (string splitAndTrimToken in layout.Render(logEvent).SplitAndTrimTokens(';'))
      {
        mailAddressCollection.Add(splitAndTrimToken);
        flag = true;
      }
    }
    return flag;
  }
}
