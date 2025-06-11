// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.TaskUnhandledExceptionEventArgs
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;

#nullable disable
namespace CommomLib.Commom;

public class TaskUnhandledExceptionEventArgs
{
  public TaskUnhandledExceptionEventArgs(Exception ex) => this.ExceptionObject = (object) ex;

  public object ExceptionObject { get; }
}
