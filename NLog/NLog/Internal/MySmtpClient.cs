// Decompiled with JetBrains decompiler
// Type: NLog.Internal.MySmtpClient
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;

#nullable disable
namespace NLog.Internal;

internal class MySmtpClient : SmtpClient, ISmtpClient, IDisposable
{
  [SpecialName]
  SmtpDeliveryMethod ISmtpClient.get_DeliveryMethod() => this.DeliveryMethod;

  [SpecialName]
  void ISmtpClient.set_DeliveryMethod(SmtpDeliveryMethod value) => this.DeliveryMethod = value;

  [SpecialName]
  string ISmtpClient.get_Host() => this.Host;

  [SpecialName]
  void ISmtpClient.set_Host(string value) => this.Host = value;

  [SpecialName]
  int ISmtpClient.get_Port() => this.Port;

  [SpecialName]
  void ISmtpClient.set_Port(int value) => this.Port = value;

  [SpecialName]
  int ISmtpClient.get_Timeout() => this.Timeout;

  [SpecialName]
  void ISmtpClient.set_Timeout(int value) => this.Timeout = value;

  [SpecialName]
  ICredentialsByHost ISmtpClient.get_Credentials() => this.Credentials;

  [SpecialName]
  void ISmtpClient.set_Credentials(ICredentialsByHost value) => this.Credentials = value;

  [SpecialName]
  bool ISmtpClient.get_EnableSsl() => this.EnableSsl;

  [SpecialName]
  void ISmtpClient.set_EnableSsl(bool value) => this.EnableSsl = value;

  void ISmtpClient.Send(MailMessage msg) => this.Send(msg);

  [SpecialName]
  string ISmtpClient.get_PickupDirectoryLocation() => this.PickupDirectoryLocation;

  [SpecialName]
  void ISmtpClient.set_PickupDirectoryLocation(string value)
  {
    this.PickupDirectoryLocation = value;
  }
}
