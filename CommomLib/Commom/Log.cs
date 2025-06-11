// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.Log
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using NLog;
using System;

#nullable disable
namespace CommomLib.Commom;

public class Log
{
  public static Logger Instance { get; private set; }

  static Log()
  {
    LogManager.ReconfigExistingLoggers();
    Log.Instance = LogManager.GetCurrentClassLogger();
  }

  public static void WriteLog(string logStr)
  {
    string userName = Environment.UserName;
    string message = logStr;
    if (message.Contains(userName))
      message = message.Replace(userName, "sechide");
    Log.Instance.Info(message);
  }
}
