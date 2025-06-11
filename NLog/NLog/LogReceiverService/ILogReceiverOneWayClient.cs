// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.ILogReceiverOneWayClient
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.ServiceModel;

#nullable disable
namespace NLog.LogReceiverService;

[ServiceContract(Namespace = "http://nlog-project.org/ws/", ConfigurationName = "NLog.LogReceiverService.ILogReceiverOneWayClient")]
public interface ILogReceiverOneWayClient
{
  [OperationContract(IsOneWay = true, AsyncPattern = true, Action = "http://nlog-project.org/ws/ILogReceiverOneWayServer/ProcessLogMessages")]
  IAsyncResult BeginProcessLogMessages(
    NLogEvents events,
    AsyncCallback callback,
    object asyncState);

  void EndProcessLogMessages(IAsyncResult result);
}
