// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ISmtpClient
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Net;
using System.Net.Mail;

#nullable disable
namespace NLog.Internal;

internal interface ISmtpClient : IDisposable
{
  SmtpDeliveryMethod DeliveryMethod { get; set; }

  string Host { get; set; }

  int Port { get; set; }

  int Timeout { get; set; }

  ICredentialsByHost Credentials { get; set; }

  bool EnableSsl { get; set; }

  void Send(MailMessage msg);

  string PickupDirectoryLocation { get; set; }
}
