// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.ILogReceiverOneWayServer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.ServiceModel;

#nullable disable
namespace NLog.LogReceiverService;

[ServiceContract(Namespace = "http://nlog-project.org/ws/")]
public interface ILogReceiverOneWayServer
{
  [OperationContract(IsOneWay = true)]
  void ProcessLogMessages(NLogEvents events);
}
